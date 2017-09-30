// ***********************************************************
// 护理电子病历系统,表单文档列表窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.DockSuite;
using Heren.Common.Report;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.PatPage.Document
{
    internal partial class DocumentListForm : DockContentBase
    {
        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// 获取或设置当前文档列表窗口关联的文档类型对象
        /// </summary>
        [Browsable(false)]
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
            set { this.m_docTypeInfo = value; }
        }

        /// <summary>
        /// 文档编辑窗口
        /// </summary>
        private DocumentEditForm m_DocumentEditForm = null;

        /// <summary>
        /// 获取当前显示文档编辑窗口
        /// </summary>
        [Browsable(false)]
        private DocumentEditForm DocumentEditForm
        {
            get
            {
                if (this.m_DocumentEditForm == null)
                    return null;
                if (this.m_DocumentEditForm.IsDisposed)
                    return null;
                return this.m_DocumentEditForm;
            }
        }

        /// <summary>
        /// 获取当前文档是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.DocumentEditForm == null)
                    return false;
                return this.DocumentEditForm.IsModified;
            }
        }

        /// <summary>
        /// 当文档被更新完成时触发
        /// </summary>
        [Description("当文档被更新完成时触发")]
        public event EventHandler DocumentUpdated;

        protected virtual void OnDocumentUpdated(EventArgs e)
        {
            if (this.DocumentUpdated != null)
                this.DocumentUpdated(this, e);
        }

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 用于存放当前文档列表中自定义列的哈希表
        /// </summary>
        private Dictionary<string, DataGridViewColumn> m_columnsTable = null;

        public DocumentListForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RefreshView();

            this.HandleUILayout();

            if (this.toolcboPatDeptList.IsDisposed != true)
                this.toolcboPatDeptList.Items.Add(string.Empty);

            this.toolcboPatDeptList.DropDown +=
                new EventHandler(this.toolcboPatDeptList_DropDown);
            this.toolcboPatDeptList.SelectedIndexChanged +=
                new System.EventHandler(this.toolcboPatDeptList_SelectedIndexChanged);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            this.HandleUILayout();
            this.Update();

            DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1).Date;
            if (PatientTable.Instance.ActivePatient != null)
                dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime.Date;

            DateTime dtEndTime = SysTimeService.Instance.Now;
            dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 23, 59, 59);
            if (this.m_docTypeInfo.IsRepeated && SystemContext.Instance.SystemOption.DocLoadTime > 0)
            {
                DateTime dtBeginShowTime = dtEndTime.AddHours(-SystemContext.Instance.SystemOption.DocLoadTime).AddTicks(1);
                if (dtBeginTime < dtBeginShowTime)
                    dtBeginTime = dtBeginShowTime;
            }

            this.m_bValueChangedEnabled = false;
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            if (this.m_docTypeInfo == null)
                return;

            GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.WaitCursor);

            //如果不允许重复创建,那么先显示编辑窗口
            if (!this.m_docTypeInfo.IsRepeated)
            {
                this.CreateDocumentEditForm();
            }
            else
            {
                if (this.DocumentEditForm != null)
                    this.DocumentEditForm.Hide();
            }

            //加载显示的列方案
            this.LoadGridViewColumns();

            //加载已写的病历列表
            if (!this.LoadDocumentList(null))
            {
                GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
                return;
            }

            //如果未写过当前病历类型且为不可重复类型,那么自动创建。
            if (!this.m_docTypeInfo.IsRepeated)
            {
                int nLastIndex = this.dataTableView1.Rows.Count - 1;
                DataGridViewRow row = this.dataTableView1.Rows[nLastIndex];
                if (row == null)
                    this.ShowDocumentEditForm(this.m_docTypeInfo);
                else
                    this.ShowDocumentEditForm(row.Tag as NurDocInfo);
            }
            //判断是否是需要自动新增
            else if (SystemContext.Instance.SystemOption.SpecialNursingIncrease == true
                && this.dataTableView1.Rows.Count <= 0)
            {
                this.ShowDocumentEditForm(m_docTypeInfo);
            }
            GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
        }

        protected override void OnPatientInfoChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientInfoUpdated)
                this.RefreshView();
        }

        protected override void OnPatientInfoChanging(PatientInfoChangingEventArgs e)
        {
            if (this.IsModified && this.DockHandler != null)
                this.DockHandler.Activate();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// 处理界面上一些控件的可见性事宜
        /// </summary>
        private void HandleUILayout()
        {
            string szDocTypeID = null;
            if (this.m_docTypeInfo != null)
                szDocTypeID = this.m_docTypeInfo.DocTypeID;
            bool bIsPrintable = FormCache.Instance.IsFormListPrintable(szDocTypeID);
            if (this.toolbtnPrint.Visible != bIsPrintable)
                this.toolbtnPrint.Visible = bIsPrintable;
        }

        /// <summary>
        /// 检查当前文档编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.DocumentEditForm == null)
                return false;
            return this.DocumentEditForm.CheckModifyState();
        }

        /// <summary>
        /// 查询当前病人所经过的病区hash表
        /// </summary>
        /// <returns>是否查询成功</returns>
        private bool LoadPatientDeptList()
        {
            this.toolcboPatDeptList.Items.Clear();
            this.toolcboPatDeptList.Items.Add(string.Empty);
            this.Update();
            if (SystemContext.Instance.LoginUser == null)
                return false;
            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            List<DeptInfo> lstDeptInfos = null;
            short shRet = PatVisitService.Instance.GetPatientDeptList(szPatientID, szVisitID, ref lstDeptInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("查询当前病人病区列表失败!");
                return false;
            }
            if (lstDeptInfos == null || lstDeptInfos.Count <= 0)
                return false;
            foreach (DeptInfo deptInfo in lstDeptInfos)
                this.toolcboPatDeptList.Items.Add(deptInfo);
            return true;
        }

        /// <summary>
        /// 加载已评估过的历史病历列表
        /// </summary>
        /// <param name="szDefaultSelectedDocID">默认选中的文档ID</param>
        /// <returns>是否加载成功</returns>
        private bool LoadDocumentList(string szDefaultSelectedDocID)
        {
            //记录下当前行的文档ID,以便刷新后还原选中
            if (this.dataTableView1.CurrentRow != null
                && string.IsNullOrEmpty(szDefaultSelectedDocID))
            {
                NurDocInfo docInfo = this.dataTableView1.CurrentRow.Tag as NurDocInfo;
                if (docInfo != null)
                    szDefaultSelectedDocID = docInfo.DocSetID;
            }
            this.dataTableView1.Rows.Clear();
            if (this.dataTableView1.Columns.Count <= 0)
                return true;
            this.Update();

            if (this.m_docTypeInfo == null || PatientTable.Instance.ActivePatient == null)
                return false;
            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);
            NurDocList lstDocInfos = null;
            // 当文档不可重复时，需要查询出所有的该类型列表 方式记录时间小于入院时间和大于当前时间时 查询不到数据 导致表单不可保存问题
            if (!this.m_docTypeInfo.IsRepeated)
            {
                dtBeginTime = DateTime.MinValue;
                dtEndTime = DateTime.MaxValue;
            }
            short shRet = DocumentService.Instance.GetDocInfos(szPatientID, szVisitID, szDocTypeID
                , dtBeginTime, dtEndTime, ref lstDocInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("已写病历列表下载失败!");
                return false;
            }

            if (lstDocInfos == null || lstDocInfos.Count <= 0)
                return true;

            DeptInfo currentDept = this.toolcboPatDeptList.SelectedItem as DeptInfo;

            this.dataTableView1.SuspendLayout();
            foreach (NurDocInfo docInfo in lstDocInfos)
            {
                if (currentDept != null && docInfo.WardCode != currentDept.DeptCode)
                    continue;
                int index = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[index];
                row.Tag = docInfo;
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("创建人"))
                {
                    DataGridViewColumn column = this.m_columnsTable["创建人"];
                    row.Cells[column.Index].Value = docInfo.CreatorName;
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("创建时间"))
                {
                    DataGridViewColumn column = this.m_columnsTable["创建时间"];
                    row.Cells[column.Index].Value = docInfo.DocTime.ToString("yyyy-MM-dd HH:mm");
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("修改人"))
                {
                    DataGridViewColumn column = this.m_columnsTable["修改人"];
                    row.Cells[column.Index].Value = docInfo.ModifierName;
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("修改时间"))
                {
                    DataGridViewColumn column = this.m_columnsTable["修改时间"];
                    row.Cells[column.Index].Value = docInfo.ModifyTime.ToString("yyyy-MM-dd HH:mm");
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("记录时间"))
                {
                    DataGridViewColumn column = this.m_columnsTable["记录时间"];
                    row.Cells[column.Index].Value = docInfo.RecordTime.ToString("yyyy-MM-dd HH:mm");
                }
                this.LoadSummaryData(row);

                int nMinHeight = this.dataTableView1.RowTemplate.Height;
                this.dataTableView1.AdjustRowHeight(0, this.dataTableView1.Rows.Count, nMinHeight, (nMinHeight * 20) + 4);
                this.dataTableView1.SetRowState(row, RowState.Normal);
                if (docInfo.DocSetID == szDefaultSelectedDocID)
                    this.dataTableView1.SelectRow(row);
            }
            if (!string.IsNullOrEmpty(this.m_docTypeInfo.SortColumn)
                && this.m_docTypeInfo.SortMode != SortMode.None
                && dataTableView1.Columns.Contains(this.m_docTypeInfo.SortColumn))
            {
                ListSortDirection ListSortDirection = ListSortDirection.Ascending;
                if (this.m_docTypeInfo.SortMode == SortMode.Descending)
                    ListSortDirection = ListSortDirection.Descending;
                this.dataTableView1.Sort(dataTableView1.Columns[this.m_docTypeInfo.SortColumn], ListSortDirection);
            }
            this.dataTableView1.ResumeLayout();

            //将滚动条自动滚动到当前行
            DataTableViewRow currentRow = this.dataTableView1.CurrentRow;
            this.dataTableView1.CurrentCell = null;
            this.dataTableView1.SelectRow(currentRow);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);

            return true;
        }

        /// <summary>
        /// 加载文档摘要数据列集合
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewColumns()
        {
            //清除已显示的各摘要列
            this.dataTableView1.Columns.Clear();
            if (this.m_docTypeInfo == null)
                return false;
            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szDocTypeID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            if (lstGridViewColumns == null)
                lstGridViewColumns = new List<GridViewColumn>();

            if (this.m_columnsTable == null)
                this.m_columnsTable = new Dictionary<string, DataGridViewColumn>();
            this.m_columnsTable.Clear();

            if (lstGridViewColumns.Count <= 0)
            {
                GridViewColumn gridViewColumn = new GridViewColumn();
                gridViewColumn = new GridViewColumn();
                gridViewColumn.ColumnID = "0";
                gridViewColumn.ColumnIndex = 0;
                gridViewColumn.IsMiddle = false;
                gridViewColumn.ColumnTag = "创建人";
                gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
                gridViewColumn.ColumnType = "字符";
                gridViewColumn.ColumnWidth = 64;
                lstGridViewColumns.Add(gridViewColumn);

                gridViewColumn = new GridViewColumn();
                gridViewColumn.ColumnID = "1";
                gridViewColumn.ColumnIndex = 1;
                gridViewColumn.IsMiddle = false;
                gridViewColumn.ColumnTag = "创建时间";
                gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
                gridViewColumn.ColumnType = "日期";
                gridViewColumn.ColumnWidth = 110;
                lstGridViewColumns.Add(gridViewColumn);

                gridViewColumn = new GridViewColumn();
                gridViewColumn.ColumnID = "0";
                gridViewColumn.ColumnIndex = 0;
                gridViewColumn.IsMiddle = false;
                gridViewColumn.ColumnTag = "修改人";
                gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
                gridViewColumn.ColumnType = "字符";
                gridViewColumn.ColumnWidth = 64;
                lstGridViewColumns.Add(gridViewColumn);

                gridViewColumn = new GridViewColumn();
                gridViewColumn.ColumnID = "1";
                gridViewColumn.ColumnIndex = 1;
                gridViewColumn.IsMiddle = false;
                gridViewColumn.ColumnTag = "修改时间";
                gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
                gridViewColumn.ColumnType = "日期";
                gridViewColumn.ColumnWidth = 110;
                lstGridViewColumns.Add(gridViewColumn);
            }
            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                if (gridViewColumn == null)
                    continue;
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Tag = gridViewColumn;
                column.Width = gridViewColumn.ColumnWidth;
                column.Visible = gridViewColumn.IsVisible;

                if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnTag))
                    column.Name = gridViewColumn.ColumnTag;
                else
                    column.Name = gridViewColumn.ColumnName;
                if (GlobalMethods.Misc.IsEmptyString(column.Name))
                    continue;

                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                if (!gridViewColumn.IsMiddle)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnUnit))
                    column.HeaderText = gridViewColumn.ColumnName;
                else
                    column.HeaderText = string.Format("{0}({1})", gridViewColumn.ColumnName, gridViewColumn.ColumnUnit);
                this.dataTableView1.Columns.Add(column);
                if (!this.m_columnsTable.ContainsKey(column.Name))
                    this.m_columnsTable.Add(column.Name, column);
            }
            return true;
        }

        /// <summary>
        /// 加载指定行的摘要列的列数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <returns>是否加载成功</returns>
        private bool LoadSummaryData(DataTableViewRow row)
        {
            if (row == null || row.Tag == null)
                return false;
            NurDocInfo docInfo = row.Tag as NurDocInfo;

            string szDocSetID = docInfo.DocSetID;
            List<SummaryData> lstSummaryData = null;
            short shRet = DocumentService.Instance.GetSummaryData(szDocSetID, false, ref lstSummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            if (lstSummaryData == null || lstSummaryData.Count <= 0)
                return true;

            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData == null || string.IsNullOrEmpty(summaryData.DataName))
                    continue;
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey(summaryData.DataName))
                {
                    DataGridViewColumn column = this.m_columnsTable[summaryData.DataName];
                    row.Cells[column.Index].Value = summaryData.DataValue;
                }
            }
            return true;
        }

        /// <summary>
        /// 删除当前选中的文档
        /// </summary>
        /// <returns>是否删除成功</returns>
        private bool DeleteSelectedDocument()
        {
            if (!RightController.Instance.UserRight.DeleteNuringDoc.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限删除护理文书!");
                return false;
            }
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return false;
            NurDocInfo docInfo = this.dataTableView1.SelectedRows[0].Tag as NurDocInfo;
            if (!RightController.Instance.CanDeleteNurDoc(docInfo))
                return false;

            DialogResult result = MessageBoxEx.ShowConfirmFormat("确认删除“{0}”病历吗？"
                , null, docInfo.DocTitle);
            if (result != DialogResult.OK)
                return false;
            DocStatusInfo docStatusInfo = new DocStatusInfo();
            docStatusInfo.DocID = docInfo.DocID;
            docStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
            docStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
            docStatusInfo.OperateTime = SysTimeService.Instance.Now;
            docStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
            short shRet = DocumentService.Instance.SetDocStatusInfo(ref docStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("病历删除失败,{0}!", docStatusInfo.StatusMessage));
                return false;
            }
            this.dataTableView1.Rows.Remove(this.dataTableView1.SelectedRows[0]);
            return true;
        }

        /// <summary>
        /// 文档编辑窗口是否弹出框
        /// </summary>
        /// <returns>是否弹出</returns>
        private bool IsShowAsModel()
        {
            string key = SystemConst.ConfigKey.NUR_DOC_SHOW_AS_MODEL;
            return this.m_docTypeInfo != null && this.m_docTypeInfo.IsRepeated
                 && SystemContext.Instance.GetConfig(key, false);
        }

        /// <summary>
        /// 创建文档编辑器窗口
        /// </summary>
        /// <param name="bNotOnlyShowInListForm">是否只是已非弹出的方式显示</param>
        public void CreateDocumentEditForm(bool bNotOnlyShowInListForm)
        {
            if (this.DocumentEditForm == null)
            {
                this.m_DocumentEditForm = new DocumentEditForm();
                this.m_DocumentEditForm.ShowInTaskbar = false;
                this.m_DocumentEditForm.MinimizeBox = false;
                this.m_DocumentEditForm.StartPosition = FormStartPosition.CenterParent;
                this.m_DocumentEditForm.DocumentUpdated +=
                    new EventHandler(this.DocumentEditForm_DocumentUpdated);
            }
            this.DocumentEditForm.IsFirstRecord = this.dataTableView1.Rows.Count <= 0;

            if (bNotOnlyShowInListForm && this.IsShowAsModel())
            {
                this.DocumentEditForm.Visible = false;
                this.DocumentEditForm.Parent = null;
                this.DocumentEditForm.TopLevel = true;
                this.DocumentEditForm.Dock = DockStyle.None;
                this.DocumentEditForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                this.DocumentEditForm.TopLevel = false;
                this.DocumentEditForm.FormBorderStyle = FormBorderStyle.None;
                this.DocumentEditForm.Dock = DockStyle.Fill;
                this.DocumentEditForm.Parent = this;
                this.DocumentEditForm.Visible = true;
                this.DocumentEditForm.BringToFront();
            }
            if (this.m_docTypeInfo != null)
                this.DocumentEditForm.ShowReturnButton = this.m_docTypeInfo.IsRepeated;
        }

        /// <summary>
        /// 创建文档编辑器窗口
        /// </summary>
        private void CreateDocumentEditForm()
        {
            this.CreateDocumentEditForm(true);
        }

        /// <summary>
        /// 显示指定护理文档类型对应的护理电子病历编辑器
        /// </summary>
        /// <param name="docTypeInfo">护理文档类型</param>
        private void ShowDocumentEditForm(DocTypeInfo docTypeInfo)
        {
            this.CreateDocumentEditForm();
            Application.DoEvents();
            if (!this.IsShowAsModel())
                this.DocumentEditForm.CreateDocument(docTypeInfo);
            else
                this.DocumentEditForm.ShowDialog(docTypeInfo, this);
        }

        /// <summary>
        /// 显示指定护理文档对应的护理电子病历编辑器
        /// </summary>
        /// <param name="docInfo">关联的护理文档信息</param>
        private void ShowDocumentEditForm(NurDocInfo docInfo)
        {
            this.CreateDocumentEditForm();
            //默认获取相应的doctype以便保存时判断
            //DocTypeInfo docTypeInfo =
            //        FormCache.Instance.GetDocTypeInfo(docInfo.DocTypeID);

            DocumentEditForm.DocTypeInfo = this.DocTypeInfo;
            if (!this.IsShowAsModel())
                this.DocumentEditForm.OpenDocument(docInfo);
            else
                this.DocumentEditForm.ShowDialog(docInfo, this);
        }

        /// <summary>
        /// 显示指定护理文档对应的护理电子病历编辑器判断是否未驳回
        /// </summary>
        /// <param name="docInfo">关联的护理文档信息</param>
        /// <returns>成功与否</returns>
        private bool ShowDocumentEditFormWithCheck(NurDocInfo docInfo)
        {
            bool check = false;
            this.CreateDocumentEditForm(false);
            check = this.DocumentEditForm.CheckDocument(docInfo);
            return check;
        }

        /// <summary>
        /// 显示指定文档类型和文档记录对应的文档编辑器窗口
        /// </summary>
        /// <param name="szDocID">文档记录ID</param>
        public void ShowDocumentEditForm(string szDocID)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                this.ShowDocumentEditForm(this.m_docTypeInfo);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            NurDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(szDocID))
                shRet = DocumentService.Instance.GetDocInfo(szDocID, ref docInfo);
            else
                shRet = DocumentService.Instance.GetLatestDocInfo(szDocID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的表单!", szDocID));
                return;
            }
            this.ShowDocumentEditForm(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="docTypeInfo">病历类型信息</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public override bool LocateToModule(DocTypeInfo docTypeInfo, string szDocID)
        {
            if (docTypeInfo == null || this.m_docTypeInfo == null)
                return false;
            if (docTypeInfo.ApplyEnv != ServerData.DocTypeApplyEnv.NURSING_DOCUMENT)
                return false;
            if (docTypeInfo.DocTypeID != this.m_docTypeInfo.DocTypeID)
                return false;

            this.Activate();
            Application.DoEvents();

            this.ShowDocumentEditForm(szDocID);
            return true;
        }

        private void DocumentEditForm_DocumentUpdated(object sender, EventArgs e)
        {
            if (this.DocumentEditForm == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szDocSetID = null;
            if (this.DocumentEditForm.Document != null)
                szDocSetID = this.DocumentEditForm.Document.DocSetID;
            this.LoadDocumentList(szDocSetID);

            this.OnDocumentUpdated(e);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocumentList(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.CanCreateNurDoc())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.ShowDocumentEditForm(this.m_docTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            this.toolbtnNew.PerformClick();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            NurDocInfo docInfo = this.dataTableView1.SelectedRows[0].Tag as NurDocInfo;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (SystemContext.Instance.SystemOption.NursingSpecialDeleteDirectly)
            {
                this.DeleteSelectedDocument();
            }
            else
            {
                bool cheecktrue = this.ShowDocumentEditFormWithCheck(docInfo);
                if (cheecktrue)
                {
                    this.DeleteSelectedDocument();
                    this.DocumentEditForm.Hide();
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnOption_DropDownOpening(object sender, EventArgs e)
        {
            string key = SystemConst.ConfigKey.NUR_DOC_SHOW_AS_MODEL;
            if (SystemContext.Instance.GetConfig(key, false))
                this.toolmnuShowAsModel.Checked = true;
            else
                this.toolmnuShowAsModel.Checked = false;

            key = SystemConst.ConfigKey.NUR_DOC_SAVE_RETURN;
            if (SystemContext.Instance.GetConfig(key, false))
                this.toolmnuSaveReturn.Checked = true;
            else
                this.toolmnuSaveReturn.Checked = false;
        }

        private void toolmnuShowAsModel_Click(object sender, EventArgs e)
        {
            this.toolmnuShowAsModel.Checked = !this.toolmnuShowAsModel.Checked;
            string key = SystemConst.ConfigKey.NUR_DOC_SHOW_AS_MODEL;
            string value = this.toolmnuShowAsModel.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void toolmnuSaveReturn_Click(object sender, EventArgs e)
        {
            this.toolmnuSaveReturn.Checked = !this.toolmnuSaveReturn.Checked;
            string key = SystemConst.ConfigKey.NUR_DOC_SAVE_RETURN;
            string value = this.toolmnuSaveReturn.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            NurDocInfo docInfo = row.Tag as NurDocInfo;
            if (docInfo == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowDocumentEditForm(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        #region "打印文档记录"
        /// <summary>
        /// 加载护理记录单打印模板
        /// </summary>
        /// <param name="szReportName">护理记录单打印模板名</param>
        /// <returns>护理记录单打印模板byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            if (this.m_docTypeInfo == null
                || GlobalMethods.Misc.IsEmptyString(this.m_docTypeInfo.DocTypeName))
            {
                MessageBoxEx.ShowError("无法获取文档类型信息!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_DOC_LIST;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.m_docTypeInfo.DocTypeName;
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowError("当前文档列表的打印报表还没有制作!");
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("当前文档列表的打印报表内容下载失败!");
                return null;
            }
            return byteTempletData;
        }

        private bool GetSystemContext(string name, ref object value)
        {
            if (name == "查询起始时间")
            {
                value = this.tooldtpDateFrom.Value;
                return true;
            }
            if (name == "查询截止时间")
            {
                value = this.tooldtpDateTo.Value;
                return true;
            }
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        private ReportExplorerForm GetReportExplorerForm()
        {
            ReportExplorerForm reportExplorerForm = new ReportExplorerForm();
            reportExplorerForm.WindowState = FormWindowState.Maximized;
            reportExplorerForm.QueryContext +=
                new QueryContextEventHandler(this.ReportExplorerForm_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new ExecuteQueryEventHandler(this.ReportExplorerForm_ExecuteQuery);
            reportExplorerForm.NotifyNextReport +=
                new NotifyNextReportEventHandler(this.ReportExplorerForm_NotifyNextReport);
            return reportExplorerForm;
        }
        #endregion

        private void toolmnuPrintAll_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintFrom_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            SetPageNumberForm setPageNumberForm = new SetPageNumberForm();
            if (setPageNumberForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            int nStartPageNumber = setPageNumberForm.StartPageNumber;

            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                int start = this.dataTableView1.SelectedRows[0].Index;
                int end = this.dataTableView1.Rows.Count - 1;
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, start, end, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = nStartPageNumber;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("设置打印起始页码", nStartPageNumber);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintContinued_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                int start = this.dataTableView1.SelectedRows[0].Index;
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("续打行号", start);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboPatDeptList_DropDown(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.toolcboPatDeptList.Items.Count <= 1)
                this.LoadPatientDeptList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboPatDeptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocumentList(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            e.Value = value;
        }

        private void ReportExplorerForm_ExecuteQuery(object sender, Heren.Common.Report.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void ReportExplorerForm_NotifyNextReport(object sender, Heren.Common.Report.NotifyNextReportEventArgs e)
        {
            e.ReportData = this.GetReportFileData(e.ReportName);
        }
    }
}
