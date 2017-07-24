// ***********************************************************
// 护理电子病历系统,质量与安全管理记录列表管理窗口.
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
using Heren.NurDoc.PatPage.Document;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class QCListForm : DockContentBase
    {
        private QCTypeInfo m_qcTypeInfo = null;

        /// <summary>
        /// 获取或设置当前文档列表窗口关联的文档类型对象
        /// </summary>
        [Browsable(false)]
        public QCTypeInfo QCTypeInfo
        {
            get { return this.m_qcTypeInfo; }
            set { this.m_qcTypeInfo = value; }
        }

        /// <summary>
        /// 文档编辑窗口
        /// </summary>
        private QCEditForm m_qcEditForm = null;

        /// <summary>
        /// 获取当前显示文档编辑窗口
        /// </summary>
        [Browsable(false)]
        private QCEditForm QCEditForm
        {
            get
            {
                if (this.m_qcEditForm == null)
                    return null;
                if (this.m_qcEditForm.IsDisposed)
                    return null;
                return this.m_qcEditForm;
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
                if (this.QCEditForm == null)
                    return false;
                return this.QCEditForm.IsModified;
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

        public QCListForm()
        {
            InitializeComponent();
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
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            this.Update();

            DateTime dtBeginTime = SysTimeService.Instance.Now.AddMonths(-1).Date;
            DateTime dtEndTime = SysTimeService.Instance.Now;
            dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 23, 59, 59);
            if (this.m_qcTypeInfo.IsRepeated && SystemContext.Instance.SystemOption.DocLoadTime > 0)
            {
                DateTime dtBeginShowTime = dtEndTime.AddHours(-SystemContext.Instance.SystemOption.DocLoadTime).AddTicks(1);
                if (dtBeginTime < dtBeginShowTime)
                    dtBeginTime = dtBeginShowTime;
            }

            this.m_bValueChangedEnabled = false;
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            if (this.m_qcTypeInfo == null)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //如果不允许重复创建,那么先显示编辑窗口
            if (!this.m_qcTypeInfo.IsRepeated)
            {
                this.CreateQCEditForm();
            }
            else
            {
                if (this.QCEditForm != null)
                    this.QCEditForm.Hide();
            }

            //加载显示的列方案
            this.LoadGridViewColumns();

            //加载已写的质量与安全管理记录列表
            if (!this.LoadQCList(null))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            //如果未写过当前病历类型且为不可重复类型,那么自动创建。
            if (!this.m_qcTypeInfo.IsRepeated)
            {
                int nLastIndex = this.dataTableView1.Rows.Count - 1;
                DataGridViewRow row = this.dataTableView1.Rows[nLastIndex];
                if (row == null)
                    this.ShowQCEditForm(this.m_qcTypeInfo);
                else
                    this.ShowQCEditForm(row.Tag as QCDocInfo);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }


        /// <summary>
        /// 检查当前文档编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.QCEditForm == null)
                return false;
            return this.QCEditForm.CheckModifyState();
        }

        /// <summary>
        /// 加载历史质量与安全管理记录列表
        /// </summary>
        /// <param name="szDefaultSelectedDocID">默认选中的文档ID</param>
        /// <returns>是否加载成功</returns>
        private bool LoadQCList(string szDefaultSelectedDocID)
        {
            //记录下当前行的文档ID,以便刷新后还原选中
            if (this.dataTableView1.CurrentRow != null
                && string.IsNullOrEmpty(szDefaultSelectedDocID))
            {
                QCDocInfo docInfo = this.dataTableView1.CurrentRow.Tag as QCDocInfo;
                if (docInfo != null)
                    szDefaultSelectedDocID = docInfo.DocSetID;
            }
            this.dataTableView1.Rows.Clear();
            if (this.dataTableView1.Columns.Count <= 0)
                return true;
            this.Update();

            if (this.m_qcTypeInfo == null)
                return false;

            string szQCTypeID = this.m_qcTypeInfo.QCTypeID;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);

            QCDocList lstQCInfos = null;
            // 当文档不可重复时，需要查询出所有的该类型列表 方式记录时间小于当前时间时 查询不到数据 导致表单不可保存问题
            if (!this.m_qcTypeInfo.IsRepeated)
            {
                dtBeginTime = DateTime.MinValue;
                dtEndTime = DateTime.MaxValue;
            }
            string szCreatorID = SystemContext.Instance.LoginUser.ID;
            short shRet = QCService.Instance.GetQCInfos(szCreatorID, szQCTypeID, dtBeginTime, dtEndTime, ref lstQCInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("质量与安全管理历史记录列表下载失败!");
                return false;
            }

            if (lstQCInfos == null || lstQCInfos.Count <= 0)
                return true;

            this.dataTableView1.SuspendLayout();
            foreach (QCDocInfo docInfo in lstQCInfos)
            {
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
            if (!string.IsNullOrEmpty(this.m_qcTypeInfo.SortColumn)
                && this.m_qcTypeInfo.SortMode != SortMode.None
                && dataTableView1.Columns.Contains(this.m_qcTypeInfo.SortColumn))
            {
                ListSortDirection ListSortDirection = ListSortDirection.Ascending;
                if (this.m_qcTypeInfo.SortMode == SortMode.Descending)
                    ListSortDirection = ListSortDirection.Descending;
                this.dataTableView1.Sort(dataTableView1.Columns[this.m_qcTypeInfo.SortColumn], ListSortDirection);
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
            if (this.m_qcTypeInfo == null)
                return false;
            string szQCTypeID = this.m_qcTypeInfo.QCTypeID;
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szQCTypeID, ref lstGridViewColumns);
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
            QCDocInfo docInfo = row.Tag as QCDocInfo;

            string szDocSetID = docInfo.DocSetID;
            List<QCSummaryData> lstQCSummaryData = null;
            short shRet = QCService.Instance.GetSummaryData(szDocSetID, false, ref lstQCSummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            if (lstQCSummaryData == null || lstQCSummaryData.Count <= 0)
                return true;

            foreach (QCSummaryData qcSummaryData in lstQCSummaryData)
            {
                if (qcSummaryData == null || string.IsNullOrEmpty(qcSummaryData.DataName))
                    continue;
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey(qcSummaryData.DataName))
                {
                    DataGridViewColumn column = this.m_columnsTable[qcSummaryData.DataName];
                    row.Cells[column.Index].Value = qcSummaryData.DataValue;
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
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return false;
            QCDocInfo docInfo = this.dataTableView1.SelectedRows[0].Tag as QCDocInfo;
            NurUserRight userRight = RightController.Instance.UserRight;
            if (!userRight.ShowNursingAssessForm.Value)
                return false;

            DialogResult result = MessageBoxEx.ShowConfirmFormat("确认删除“{0}”记录吗？"
                , null, docInfo.DocTitle);
            if (result != DialogResult.OK)
                return false;
            DocStatusInfo docStatusInfo = new DocStatusInfo();
            docStatusInfo.DocID = docInfo.DocID;
            docStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
            docStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
            docStatusInfo.OperateTime = SysTimeService.Instance.Now;
            docStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
            short shRet = QCService.Instance.SetDocStatusInfo(ref docStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("记录删除失败,{0}!", docStatusInfo.StatusMessage));
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
            string key = SystemConst.ConfigKey.QC_SHOW_AS_MODEL;
            return this.m_qcTypeInfo != null && this.m_qcTypeInfo.IsRepeated
                 && SystemContext.Instance.GetConfig(key, false);
        }

        /// <summary>
        /// 创建文档编辑器窗口
        /// </summary>
        /// <param name="bNotOnlyShowInListForm">是否只是以非弹出的方式显示</param>
        public void CreateQCEditForm(bool bNotOnlyShowInListForm)
        {
            if (this.QCEditForm == null)
            {
                this.m_qcEditForm = new QCEditForm();
                this.m_qcEditForm.ShowInTaskbar = false;
                this.m_qcEditForm.MinimizeBox = false;
                this.m_qcEditForm.StartPosition = FormStartPosition.CenterParent;
                this.m_qcEditForm.DocumentUpdated +=
                    new EventHandler(this.QCEditForm_DocumentUpdated);
            }
            this.QCEditForm.IsFirstRecord = this.dataTableView1.Rows.Count <= 0;

            if (bNotOnlyShowInListForm && this.IsShowAsModel())
            {
                this.QCEditForm.Visible = false;
                this.QCEditForm.Parent = null;
                this.QCEditForm.TopLevel = true;
                this.QCEditForm.Dock = DockStyle.None;
                this.QCEditForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                this.QCEditForm.TopLevel = false;
                this.QCEditForm.FormBorderStyle = FormBorderStyle.None;
                this.QCEditForm.Dock = DockStyle.Fill;
                this.QCEditForm.Parent = this;
                this.QCEditForm.Visible = true;
                this.QCEditForm.BringToFront();
            }
            if (this.m_qcTypeInfo != null)
                this.QCEditForm.ShowReturnButton = this.m_qcTypeInfo.IsRepeated;
        }

        /// <summary>
        /// 创建文档编辑器窗口
        /// </summary>
        private void CreateQCEditForm()
        {
            this.CreateQCEditForm(true);
        }

        /// <summary>
        /// 显示指定质量与安全管理记录文档类型对应的质量与安全管理记录文档编辑器
        /// </summary>
        /// <param name="qcTypeInfo">质量与安全管理记录文档类型</param>
        private void ShowQCEditForm(QCTypeInfo qcTypeInfo)
        {
            this.CreateQCEditForm();
            Application.DoEvents();
            if (!this.IsShowAsModel())
                this.QCEditForm.CreateDocument(qcTypeInfo);
            else
                this.QCEditForm.ShowDialog(qcTypeInfo, this);
        }

        /// <summary>
        /// 显示指定质量与安全管理记录文档类型对应的质量与安全管理记录文档编辑器
        /// </summary>
        /// <param name="docInfo">关联的质量与安全管理记录文档信息</param>
        private void ShowQCEditForm(QCDocInfo docInfo)
        {
            this.CreateQCEditForm();
            QCEditForm.QCTypeInfo = this.QCTypeInfo;
            if (!this.IsShowAsModel())
                this.QCEditForm.OpenDocument(docInfo);
            else
                this.QCEditForm.ShowDialog(docInfo, this);
        }

        /// <summary>
        /// 显示指定质量与安全管理记录文档对应的质量与安全管理记录编辑器判断是否未驳回
        /// </summary>
        /// <param name="docInfo">关联的质量与安全管理记录文档信息</param>
        /// <returns>成功与否</returns>
        private bool ShowQCEditFormWithCheck(QCDocInfo docInfo)
        {
            bool check = false;
            this.CreateQCEditForm(false);
            check = this.QCEditForm.CheckDocument(docInfo);
            return check;
        }

        /// <summary>
        /// 显示指定文档类型和文档记录对应的文档编辑器窗口
        /// </summary>
        /// <param name="szQCID">文档记录ID</param>
        public void ShowQCEditForm(string szQCID)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (GlobalMethods.Misc.IsEmptyString(szQCID))
            {
                this.ShowQCEditForm(this.m_qcTypeInfo);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            QCDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(szQCID))
                shRet = QCService.Instance.GetQCInfo(szQCID, ref docInfo);
            else
                shRet = QCService.Instance.GetLatestQCInfo(szQCID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的表单!", szQCID));
                return;
            }
            this.ShowQCEditForm(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void QCEditForm_DocumentUpdated(object sender, EventArgs e)
        {
            if (this.QCEditForm == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szQCSetID = null;
            if (this.QCEditForm.Document != null)
                szQCSetID = this.QCEditForm.Document.DocSetID;
            this.LoadQCList(szQCSetID);

            this.OnDocumentUpdated(e);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void QCtEditForm_QCUpdated(object sender, EventArgs e)
        {
            if (this.QCEditForm == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szDocSetID = null;
            if (this.QCEditForm.Document != null)
                szDocSetID = this.QCEditForm.Document.DocSetID;
            this.LoadQCList(szDocSetID);

            this.OnDocumentUpdated(e);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadQCList(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            NurUserRight userRight = RightController.Instance.UserRight;
            if (!userRight.ShowNursingAssessForm.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.ShowQCEditForm(this.m_qcTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            QCDocInfo docInfo = this.dataTableView1.SelectedRows[0].Tag as QCDocInfo;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (SystemContext.Instance.SystemOption.NursingSpecialDeleteDirectly)
            {
                this.DeleteSelectedDocument();
            }
            else
            {
                bool cheecktrue = this.ShowQCEditFormWithCheck(docInfo);
                if (cheecktrue)
                {
                    this.DeleteSelectedDocument();
                    this.QCEditForm.Hide();
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            QCDocInfo docInfo = row.Tag as QCDocInfo;
            if (docInfo == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowQCEditForm(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuShowAsModel_Click(object sender, EventArgs e)
        {
            this.toolmnuShowAsModel.Checked = !this.toolmnuShowAsModel.Checked;
            string key = SystemConst.ConfigKey.QC_SHOW_AS_MODEL;
            string value = this.toolmnuShowAsModel.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void toolmnuSaveReturn_Click(object sender, EventArgs e)
        {
            this.toolmnuSaveReturn.Checked = !this.toolmnuSaveReturn.Checked;
            string key = SystemConst.ConfigKey.QC_SAVE_RETURN;
            string value = this.toolmnuSaveReturn.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }       
    }
}