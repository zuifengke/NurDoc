// ***********************************************************
// 护理电子病历系统,护理申请列表窗口.
// Creator:OuFengFang  Date:2013-3-26
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
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage.Consult
{
    internal partial class NursingConsultForm : DockContentBase
    {
        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.m_ConsultEditForm == null)
                    return false;
                if (this.m_ConsultEditForm.IsDisposed)
                    return false;
                if (this.m_ConsultEditForm.Document == null)
                    return false;
                return this.m_ConsultEditForm.IsModified;
            }
        }

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 用于存放当前文档列表中自定义列的哈希表
        /// </summary>
        private Dictionary<string, DataGridViewColumn> m_columnsTable = null;

        /// <summary>
        /// 护理申请编辑窗口
        /// </summary>
        private ConsultEditForm m_ConsultEditForm = null;

        public NursingConsultForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.NursingConsult;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 500);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            this.Update();

            this.m_bValueChangedEnabled = false;
            DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1).Date;
            if (PatientTable.Instance.ActivePatient != null)
                dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime.Date;
            DateTime dtEndTime = SysTimeService.Instance.Now;
            dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 23, 59, 59);
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.WaitCursor);
            this.LoadConsultDocumentTypes();

            //加载显示的列方案
            this.LoadGridViewColumns();

            //加载已写的护理会诊列表
            if (!this.LoadNurApplyList(null))
            {
                GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
                return;
            }
            if (this.m_ConsultEditForm != null && !this.m_ConsultEditForm.IsDisposed)
                this.m_ConsultEditForm.Hide();
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
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.m_ConsultEditForm == null)
                return false;
            if (this.m_ConsultEditForm.IsDisposed)
                return false;
            return this.m_ConsultEditForm.CheckModifyState();
        }

        /// <summary>
        /// 加载会诊申请列表
        /// </summary>
        /// <param name="szDefaultSelectedDocID">默认选中的记录</param>
        /// <returns>是否加载成功</returns>
        private bool LoadNurApplyList(string szDefaultSelectedDocID)
        {
            //记录下当前行的文档ID,以便刷新后还原选中
            if (this.dataTableView1.CurrentRow != null
                && string.IsNullOrEmpty(szDefaultSelectedDocID))
            {
                NurApplyInfo nurApplyInfo = this.dataTableView1.CurrentRow.Tag as NurApplyInfo;
                if (nurApplyInfo != null)
                    szDefaultSelectedDocID = nurApplyInfo.DocID;
            }
            this.dataTableView1.Rows.Clear();
            if (this.dataTableView1.Columns.Count <= 0)
                return true;
            this.Update();

            if (PatientTable.Instance.ActivePatient == null)
                return false;

            string szPatientID = PatientTable.Instance.ActivePatient.PatientID;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitID;
            string szSubID = PatientTable.Instance.ActivePatient.SubID;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);
            
            List<NurApplyInfo> lstNurApplyInfos = null;
            short shRet = 0;

            shRet = NurApplyService.Instance.GetNurApplyInfoList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ServerData.NurApplyType.CONSULTATION_APPLY, ref lstNurApplyInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                return false;
            }
            if (lstNurApplyInfos == null || lstNurApplyInfos.Count <= 0)
                return true;

            foreach (NurApplyInfo nurApplyInfo in lstNurApplyInfos)
            {
                int index = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[index];
                row.Tag = nurApplyInfo;
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("申请人"))
                {
                    DataGridViewColumn column = this.m_columnsTable["申请人"];
                    row.Cells[column.Index].Value = nurApplyInfo.ApplicantName;
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("申请时间"))
                {
                    DataGridViewColumn column = this.m_columnsTable["申请时间"];
                    row.Cells[column.Index].Value = nurApplyInfo.ApplyTime.ToString("yyyy-MM-dd HH:mm");
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("状态"))
                {
                    NurApplyStatusInfo nurApplyStatusInfo = new NurApplyStatusInfo();
                    NurApplyService.Instance.GetNurApplyStatusInfo(nurApplyInfo.ApplyID
                        , nurApplyInfo.Status, nurApplyInfo.ModifyTime, ref nurApplyStatusInfo);
                    if (nurApplyStatusInfo != null)
                    {
                        DataGridViewColumn column = this.m_columnsTable["状态"];
                        row.Cells[column.Index].Value = nurApplyStatusInfo.StatusDesc;
                    }
                }

                //根据单元格内容调整行高度
                //int nMinHeight = this.dataTableView1.RowTemplate.Height;
                //this.dataTableView1.AdjustRowHeight(0, this.dataTableView1.Rows.Count, nMinHeight, (nMinHeight * 3) + 4);

                this.LoadSummaryData(row);
                this.dataTableView1.SetRowState(row, RowState.Normal);
                if (nurApplyInfo.DocID == szDefaultSelectedDocID)
                    this.dataTableView1.SelectRow(row);
            }
            //设置一下行宽使控件刷新一下单元格行高
            this.dataTableView1.Columns[0].Width = this.dataTableView1.Columns[0].Width + 1;
            this.dataTableView1.Columns[0].Width = this.dataTableView1.Columns[0].Width - 1;
            return true;
        }

        /// <summary>
        /// 加载护理申请列显示方案列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private GridViewSchema LoadSchemasList()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;

            //获取已保存的列显示方案的列表
            string szSchemaType = ServerData.GridViewSchema.NURSING_APPLY;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<GridViewSchema> lstGridViewSchemas = null;
            short shRet = ConfigService.Instance.GetGridViewSchemas(szSchemaType, szWardCode, ref lstGridViewSchemas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("列显示方案列表下载失败!");
                return null;
            }

            //获取列显示方案中缺省的方案
            if (lstGridViewSchemas == null || lstGridViewSchemas.Count <= 0)
                return null;
            return lstGridViewSchemas[0];
        }

        /// <summary>
        /// 加载护理申请列列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewColumns()
        {
            GridViewSchema schema = this.LoadSchemasList();
            if (this.m_columnsTable == null)
                this.m_columnsTable = new Dictionary<string, DataGridViewColumn>();
            this.m_columnsTable.Clear();

            //清除已显示的各摘要列
            this.dataTableView1.Columns.Clear();
            Application.DoEvents();

            //获取列显示方案中的列列表
            if (schema == null)
                return false;
            string szSchemaID = schema.SchemaID; 
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szSchemaID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("列显示方案的列列表下载失败!");
                return false;
            }

            //加载缺省列显示方案中的各列
            if (lstGridViewColumns == null || lstGridViewColumns.Count <= 0)
                return true;
            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Tag = gridViewColumn;
                this.dataTableView1.Columns.Add(column);

                if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnTag))
                    column.Name = gridViewColumn.ColumnTag;
                else
                    column.Name = gridViewColumn.ColumnName;

                column.HeaderText = gridViewColumn.ColumnName + " " + gridViewColumn.ColumnUnit;
                column.Width = gridViewColumn.ColumnWidth;
                column.Visible = gridViewColumn.IsVisible;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                if (!gridViewColumn.IsMiddle)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }
                else
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                }
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
            NurApplyInfo nurApplyInfo = row.Tag as NurApplyInfo;
            if (nurApplyInfo == null)
                return false;

            string szDocID = nurApplyInfo.DocID;
            List<SummaryData> lstSummaryData = null;
            short shRet = DocumentService.Instance.GetSummaryData(szDocID, false, ref lstSummaryData);
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
            NurApplyInfo NurApplyInfo = this.dataTableView1.SelectedRows[0].Tag as NurApplyInfo;
            NurDocInfo DocInfo = new NurDocInfo();
            DocumentService.Instance.GetLatestDocInfo(NurApplyInfo.DocID, ref DocInfo);
            if (!RightController.Instance.CanDeleteNurDoc(DocInfo))
                return false;

            DialogResult result = MessageBoxEx.ShowConfirmFormat("确认删除“{0}”会诊申请吗？"
                , null, DocInfo.DocTitle);
            if (result != DialogResult.OK)
                return false;
            DocStatusInfo docStatusInfo = new DocStatusInfo();
            docStatusInfo.DocID = DocInfo.DocID;
            docStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
            docStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
            docStatusInfo.OperateTime = SysTimeService.Instance.Now;
            docStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
            short shRet = DocumentService.Instance.SetDocStatusInfo(ref docStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK && shRet != ServerData.ExecuteResult.RES_IS_EXIST)
            {
                MessageBoxEx.Show(string.Format("病历删除失败,{0}!", docStatusInfo.StatusMessage));
                return false;
            }
            shRet = NurApplyService.Instance.DelNurApply(NurApplyInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("护理申请信息删除失败,{0}!", docStatusInfo.StatusMessage));
                return false;
            }
            this.dataTableView1.Rows.Remove(this.dataTableView1.SelectedRows[0]);
            return true;
        }

        /// <summary>
        /// 显示护理会诊编辑窗口
        /// </summary>
        private void ShowConsultEditForm()
        {
            Application.DoEvents();
            if (this.m_ConsultEditForm == null || this.m_ConsultEditForm.IsDisposed)
            {
                this.m_ConsultEditForm = new ConsultEditForm();
                this.m_ConsultEditForm.TopLevel = false;
                this.m_ConsultEditForm.FormBorderStyle = FormBorderStyle.None;
                this.m_ConsultEditForm.Dock = DockStyle.Fill;
                this.m_ConsultEditForm.Padding = new Padding(0);
                this.m_ConsultEditForm.Parent = this;
                this.m_ConsultEditForm.VisibleChanged +=
                    new EventHandler(this.ConsultEditForm_VisibleChanged);
            }
            this.m_ConsultEditForm.CloseDocument();
            if (!this.m_ConsultEditForm.Visible)
                this.m_ConsultEditForm.Visible = true;
            this.m_ConsultEditForm.BringToFront();
        }

        /// <summary>
        ///加载会诊所有申请单模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadConsultDocumentTypes()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_CONSULT;
            List<DocTypeInfo> lstDocTypeInfos =
                FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
                lstDocTypeInfos = new List<DocTypeInfo>();

            this.toolbtnNew.DropDownItems.Clear();
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (docTypeInfo == null)
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = docTypeInfo.DocTypeName;
                menuItem.Tag = docTypeInfo;
                menuItem.Click += new System.EventHandler(this.toolMenuItem_Click);
                this.toolbtnNew.DropDownItems.Add(menuItem);
            }

            if (this.toolbtnNew.DropDownItems.Count <= 0)
            {
                ToolStripMenuItem toolbtnEmpty = new ToolStripMenuItem("<空>");
                toolbtnEmpty.Enabled = false;
                this.toolbtnNew.DropDownItems.Add(toolbtnEmpty);
            }
            return true;
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="docTypeInfo">病历类型信息</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public override bool LocateToModule(DocTypeInfo docTypeInfo, string szDocID)
        {
            if (docTypeInfo == null)
                return false;
            if (docTypeInfo.ApplyEnv != ServerData.DocTypeApplyEnv.NURSING_CONSULT)
                return false;

            this.Activate();
            Application.DoEvents();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowConsultEditForm();

            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                this.m_ConsultEditForm.CreateDocument(docTypeInfo);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
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
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的会诊单!", szDocID));
                return true;
            }
            this.m_ConsultEditForm.OpenDocument(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        private void ConsultEditForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.m_ConsultEditForm == null || this.m_ConsultEditForm.IsDisposed)
                return;
            if (!this.m_ConsultEditForm.IsDocumentUpdated)
                return;
            if (!this.m_ConsultEditForm.Visible)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                string szDocSetID = null;
                if (this.m_ConsultEditForm.Document != null)
                    szDocSetID = this.m_ConsultEditForm.Document.DocSetID;
                this.LoadNurApplyList(szDocSetID);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolMenuItem_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ToolStripMenuItem toolMi = (ToolStripMenuItem)sender;
            DocTypeInfo docTypeInfo = (DocTypeInfo)toolMi.Tag;
            if (docTypeInfo == null)
                return;
            if (!RightController.Instance.CanCreateNurDoc())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.ShowConsultEditForm();
            this.m_ConsultEditForm.CreateDocument(docTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadNurApplyList(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            NurApplyInfo nurApplyInfo = row.Tag as NurApplyInfo;
            if (nurApplyInfo == null)
                return;
            NurDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(nurApplyInfo.DocID))
                shRet = DocumentService.Instance.GetDocInfo(nurApplyInfo.DocID, ref docInfo);
            else
                shRet = DocumentService.Instance.GetLatestDocInfo(nurApplyInfo.DocID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("文档信息下载失败!");
                return;
            }
            if (docInfo == null)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowConsultEditForm();
            this.m_ConsultEditForm.OpenDocument(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}
