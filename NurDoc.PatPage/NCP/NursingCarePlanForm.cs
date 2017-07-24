// ***********************************************************
// 护理电子病历系统,护理计划列表窗口.
// Creator:OuFengFang  Date:2013-4-3
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
using Heren.NurDoc.PatPage.NCP;
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage.NCP
{
    internal partial class NursingCarePlanForm : DockContentBase
    {
        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.m_CarePlanEditForm == null)
                    return false;
                if (this.m_CarePlanEditForm.IsDisposed)
                    return false;
                if (this.m_CarePlanEditForm.Document == null)
                    return false;
                return this.m_CarePlanEditForm.IsModified;
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
        /// 当前护理计划列表窗口关联的文档类型对象
        /// </summary>
        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// 护理计划编辑窗口
        /// </summary>
        private CarePlanEditForm m_CarePlanEditForm = null;

        public NursingCarePlanForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.NurCarePlan;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 600);
            if (SystemContext.Instance.SystemOption.NpcNewModel)
            {
                this.cmenuCarePlanList.Items.Clear();
                //this.cmenuCarePlanList.Opening -= new System.ComponentModel.CancelEventHandler(this.cmenuCarePlanList_Opening);
                this.AddSpecialMenu();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_CARE_PLAN;
            this.m_docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv);

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

            //加载显示的列方案
            this.LoadGridViewColumns();

            //加载已写的护理计划列表
            if (!this.LoadNurCarePlanList(null))
            {
                GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
                return;
            }
            if (this.m_CarePlanEditForm != null && !this.m_CarePlanEditForm.IsDisposed)
                this.m_CarePlanEditForm.Hide();
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
            if (this.m_CarePlanEditForm == null)
                return false;
            if (this.m_CarePlanEditForm.IsDisposed)
                return false;
            return this.m_CarePlanEditForm.CheckModifyState();
        }

        /// <summary>
        /// 加载护理计划列显示方案列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private GridViewSchema LoadSchemasList()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;

            //获取已保存的列显示方案的列表
            string szSchemaType = ServerData.GridViewSchema.NURSING_CARE_PLAN;
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
        /// 加载护理计划列列表
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
        /// 加载护理计划列表
        /// </summary>
        /// <param name="szDefaultSelectedDocID">默认选中的记录</param>
        /// <returns>是否加载成功</returns>
        private bool LoadNurCarePlanList(string szDefaultSelectedDocID)
        {
            //记录下当前行的文档ID,以便刷新后还原选中
            if (this.dataTableView1.CurrentRow != null
                && string.IsNullOrEmpty(szDefaultSelectedDocID))
            {
                NurCarePlanInfo ncpInfo = this.dataTableView1.CurrentRow.Tag as NurCarePlanInfo;
                if (ncpInfo != null)
                    szDefaultSelectedDocID = ncpInfo.DocID;
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

            List<NurCarePlanInfo> lstNurCarePlanInfos = null;
            short shRet = 0;

            shRet = CarePlanService.Instance.GetNurCarePlanInfoList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ref lstNurCarePlanInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                return false;
            }
            if (lstNurCarePlanInfos == null || lstNurCarePlanInfos.Count <= 0)
                return true;
            if (SystemContext.Instance.SystemOption.NcpListStatusSort)//护理计划 状态排序
                lstNurCarePlanInfos.Sort(new YiWuStatusComparer());
            foreach (NurCarePlanInfo ncpInfo in lstNurCarePlanInfos)
            {
                int index = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[index];
                row.Tag = ncpInfo;
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("序号"))
                {
                    DataGridViewColumn column = this.m_columnsTable["序号"];
                    row.Cells[column.Index].Value = index + 1;
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("护理诊断"))
                {
                    DataGridViewColumn column = this.m_columnsTable["护理诊断"];
                    row.Cells[column.Index].Value = ncpInfo.DiagName;
                }
                if (ncpInfo.Status != ServerData.NurCarePlanStatus.PROGRESS)
                {
                    if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("结束者"))
                    {
                        DataGridViewColumn column = this.m_columnsTable["结束者"];
                        row.Cells[column.Index].Value = ncpInfo.ModifierName;
                    }
                    if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("结束者编号"))
                    {
                        DataGridViewColumn column = this.m_columnsTable["结束者编号"];
                        row.Cells[column.Index].Value = ncpInfo.ModifierID;
                    }
                }

                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("建立者"))
                {
                    DataGridViewColumn column = this.m_columnsTable["建立者"];
                    row.Cells[column.Index].Value = ncpInfo.CreatorName;
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("建立者编号"))
                {
                    DataGridViewColumn column = this.m_columnsTable["建立者编号"];
                    row.Cells[column.Index].Value = ncpInfo.CreatorID;
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("创建时间"))
                {
                    DataGridViewColumn column = this.m_columnsTable["创建时间"];
                    row.Cells[column.Index].Value = ncpInfo.CreateTime.ToString("yyyy-MM-dd HH:mm");
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("状态"))
                {
                    NurCarePlanStatusInfo ncpStatusInfo = new NurCarePlanStatusInfo();
                    CarePlanService.Instance.GetNurCarePlanStatusInfo(ncpInfo.NCPID
                        , ncpInfo.Status, ncpInfo.ModifyTime, ref ncpStatusInfo);
                    if (ncpStatusInfo != null)
                    {
                        DataGridViewColumn column = this.m_columnsTable["状态"];
                        row.Cells[column.Index].Value = ncpStatusInfo.StatusDesc;
                    }
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("开始时间"))
                {
                    DataGridViewColumn column = this.m_columnsTable["开始时间"];
                    row.Cells[column.Index].Value = ncpInfo.StartTime.ToString("yyyy-MM-dd HH:mm");
                }
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey("结束时间"))
                {
                    DataGridViewColumn column = this.m_columnsTable["结束时间"];
                    if (!ncpInfo.EndTime.Equals(DateTime.MinValue))
                    {
                        row.Cells[column.Index].Value = ncpInfo.EndTime.ToString("yyyy-MM-dd HH:mm");
                    }
                }

                this.LoadSummaryData(row);
                //根据单元格内容调整行高度
                int nMinHeight = this.dataTableView1.RowTemplate.Height;
                this.dataTableView1.AdjustRowHeight(0, this.dataTableView1.Rows.Count, nMinHeight, (nMinHeight * 20) + 4);
                this.dataTableView1.SetRowState(row, RowState.Normal);
                if (ncpInfo.DocID == szDefaultSelectedDocID)
                    this.dataTableView1.SelectRow(row);
            }
            return true;
        }

        #region 义乌需求  状态排序
        public class YiWuStatusComparer : IComparer<NurCarePlanInfo>
        {
            #region IComparer<NurCarePlanInfo> Members

            public int Compare(NurCarePlanInfo x, NurCarePlanInfo y)
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;

                int xStatus = GlobalMethods.Convert.StringToValue(x.Status, 0);
                int yStatus = GlobalMethods.Convert.StringToValue(y.Status, 0);
                if (xStatus > yStatus)
                    return 1;
                else if (xStatus < yStatus)
                    return -1;
                return 0;
            }
            #endregion
        }

        #endregion

        /// <summary>
        /// 加载指定行的摘要列的列数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <returns>是否加载成功</returns>
        private bool LoadSummaryData(DataTableViewRow row)
        {
            if (row == null || row.Tag == null)
                return false;
            NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;

            string szDocSetID = ncpInfo.DocID;
            if (!DocumentInfo.IsDocSetID(szDocSetID))
                szDocSetID = DocumentInfo.GetDocSetID(szDocSetID);

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
        /// 删除当前选中的护理计划
        /// </summary>
        /// <returns>是否删除成功</returns>
        private bool DeleteSelectedDocument()
        {
            if (this.dataTableView1.SelectedRows.Count <= 0)
            {
                MessageBoxEx.Show("请选择需要操作的护理计划!");
                return false;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            DataTableViewRow row = this.dataTableView1.SelectedRows[0];
            NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;
            if (ncpInfo == null)
                return false;

            NurDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(ncpInfo.DocID))
                shRet = DocumentService.Instance.GetDocInfo(ncpInfo.DocID, ref docInfo);
            else
                shRet = DocumentService.Instance.GetLatestDocInfo(ncpInfo.DocID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("文档信息下载失败!");
                return false;
            }
            if (docInfo == null)
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
            shRet = DocumentService.Instance.SetDocStatusInfo(ref docStatusInfo);
            if (shRet != ServerData.ExecuteResult.RES_IS_EXIST && shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("病历删除失败,{0}!", docStatusInfo.StatusMessage));
                return false;
            }
            //获取子文档Doc_idlist,修改子文档状态
            List<string> lstChilDocID = new List<string>();
            shRet = DocumentService.Instance.GetChildDocIDList(docInfo.DocSetID, ref lstChilDocID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("子文档信息获取失败!");
                return false;
            }
            List<DocStatusInfo> lstDocStatusInfo = new List<DocStatusInfo>();
            foreach (string Doc_id in lstChilDocID)
            {
                DocStatusInfo childocStatusInfo = new DocStatusInfo();
                childocStatusInfo.DocID = Doc_id;
                childocStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
                childocStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
                childocStatusInfo.OperateTime = SysTimeService.Instance.Now;
                childocStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
                shRet = DocumentService.Instance.SetDocStatusInfo(ref childocStatusInfo);
                if (shRet != ServerData.ExecuteResult.RES_IS_EXIST && shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("子文档信息删除失败!");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 更新选中的护理计划
        /// </summary>
        /// <param name="dtEndTime">结束时间</param>
        /// <returns>是否成功</returns>
        private bool CompleteAllCarePlan(DateTime dtEndTime)
        {
            WorkProcess.Instance.Initialize(this, this.dataTableView1.Rows.Count, "批量完成护理计划中，请稍后...");
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                WorkProcess.Instance.Show(row.Index, false);
                NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;
                if (ncpInfo == null||ncpInfo.Status != ServerData.NurCarePlanStatus.PROGRESS)
                    continue;

                if (!NurCarePlanHandler.Instance.HandleNurCarePlanModifySave(ncpInfo
                    , ncpInfo.DiagCode, ncpInfo.DiagName, ncpInfo.StartTime, dtEndTime, ServerData.NurCarePlanStatus.COMPLETE))
                {
                    continue;
                }
            }
            WorkProcess.Instance.Close();
            return this.LoadNurCarePlanList(null);
        }

        /// <summary>
        /// 更新选中的护理计划
        /// </summary>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="szStatus">状态</param>
        /// <returns>是否成功</returns>
        private bool UpdateCarePlan(DateTime dtEndTime, string szStatus)
        {
            DataTableViewRow row = this.dataTableView1.CurrentRow;
            if (row == null || row.Index < 0)
            {
                MessageBoxEx.Show("请选择需要操作的护理计划!");
                return false;
            }
            NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;
            if (ncpInfo == null)
                return false;

            if (!NurCarePlanHandler.Instance.HandleNurCarePlanModifySave(ncpInfo
                , ncpInfo.DiagCode, ncpInfo.DiagName, ncpInfo.StartTime, dtEndTime, szStatus))
            {
                return false;
            }
            return this.LoadNurCarePlanList(null);
        }

        /// <summary>
        /// 显示护理计划编辑窗口
        /// </summary>
        private void ShowCarePlanEditForm()
        {
            this.Update();
            if (this.m_CarePlanEditForm == null || this.m_CarePlanEditForm.IsDisposed)
            {
                this.m_CarePlanEditForm = new CarePlanEditForm();
                this.m_CarePlanEditForm.TopLevel = false;
                this.m_CarePlanEditForm.FormBorderStyle = FormBorderStyle.None;
                this.m_CarePlanEditForm.Dock = DockStyle.Fill;
                this.m_CarePlanEditForm.Padding = new Padding(0);
                this.m_CarePlanEditForm.Parent = this;
                this.m_CarePlanEditForm.VisibleChanged +=
                    new EventHandler(this.CarePlanEditForm_VisibleChanged);
            }
            this.m_CarePlanEditForm.CloseDocument();
            if (!this.m_CarePlanEditForm.Visible)
                this.m_CarePlanEditForm.Visible = true;
            this.m_CarePlanEditForm.BringToFront();
        }

        /// <summary>
        /// 按市三护理计划要求，提供特性化按钮及事件
        /// </summary>
        private void AddSpecialMenu()
        {
            if (SystemContext.Instance.NCPButtonConfigTable != null && SystemContext.Instance.NCPButtonConfigTable.Count == 0)
                return;

            foreach (NurCarePlanConfig config in SystemContext.Instance.NCPButtonConfigTable)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Tag = config;
                item.Text = config.ButtonName;
                if (config.Status == "4")
                {
                    item.Click += mnuDelete_Click;
                }
                else
                {
                    item.Click += mnuSpecial_Click;
                }
                this.cmenuCarePlanList.Items.Add(item);
            }
        }

        private void CarePlanEditForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.m_CarePlanEditForm == null || this.m_CarePlanEditForm.IsDisposed)
                return;
            if (!this.m_CarePlanEditForm.IsDocumentUpdated)
                return;
            if (!this.m_CarePlanEditForm.Visible)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                string szDocID = null;
                if (this.m_CarePlanEditForm.Document != null)
                    szDocID = this.m_CarePlanEditForm.Document.DocID;
                this.LoadNurCarePlanList(szDocID);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        #region "打印处理"
        /// <summary>
        /// 加载护理文书打印模板
        /// </summary>
        /// <param name="szReportName">护理文书打印模板名</param>
        /// <returns>护理文书印模板byte[]</returns>
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
                MessageBoxEx.ShowError(szReportName + "的打印报表还没有制作!");
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError(szReportName + "的打印报表内容下载失败!");
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

        #region "右键菜单"
        private void cmenuCarePlanList_Opening(object sender, CancelEventArgs e)
        {
            if (SystemContext.Instance.SystemOption.NpcNewModel)
            {
                DataTableViewRow row = this.dataTableView1.CurrentRow;
                NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;
                NurCarePlanConfig config = SystemContext.Instance.GetConfigFromListByStatus(ncpInfo.Status);
                for (int index = 0; index < config.BtnHandler.Length; index++)
                {
                    if (config.BtnHandler[index] == '1')
                    {
                        this.cmenuCarePlanList.Items[index].Enabled = true;
                    }
                    else
                    {
                        this.cmenuCarePlanList.Items[index].Enabled = false;
                    }
                }
            }
            else
            {
                this.mnuDelete.Enabled = false;
                this.mnuComplete.Enabled = false;
                this.mnuStop.Enabled = false;
                this.mnuRollback.Enabled = false;

                DataTableViewRow row = this.dataTableView1.CurrentRow;
                if (row == null || row.Index < 0)
                    return;
                NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;
                if (ncpInfo == null)
                    return;

                if (ncpInfo.Status == ServerData.NurCarePlanStatus.PROGRESS)
                {
                    this.mnuDelete.Enabled = true;
                    this.mnuStop.Enabled = true;
                    this.mnuComplete.Enabled = true;
                }
                if (ncpInfo.Status == ServerData.NurCarePlanStatus.COMPLETE)
                {
                    this.mnuRollback.Enabled = true;
                }
                if (ncpInfo.Status == ServerData.NurCarePlanStatus.STOP)
                {
                    this.mnuRollback.Enabled = true;
                }
                if (ncpInfo.Status == ServerData.NurCarePlanStatus.CANCELED)
                {
                    this.mnuComplete.Enabled = true;
                    this.mnuStop.Enabled = true;
                    this.mnuRollback.Enabled = true;
                }
            }
        }

        private void mnuComplete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            SetDateTimeForm setDateTimeForm = new SetDateTimeForm();
            setDateTimeForm.Text = "完成时间";
            if (setDateTimeForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            DateTime dtCompleteTime = setDateTimeForm.SelectedTime;
            this.UpdateCarePlan(dtCompleteTime, ServerData.NurCarePlanStatus.COMPLETE);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuCompleteAll_Click_1(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            SetDateTimeForm setDateTimeForm = new SetDateTimeForm();
            setDateTimeForm.Text = "完成时间";
            if (setDateTimeForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            DateTime dtCompleteTime = setDateTimeForm.SelectedTime;
            this.CompleteAllCarePlan(dtCompleteTime);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            SetDateTimeForm setDateTimeForm = new SetDateTimeForm();
            setDateTimeForm.Text = "停止时间";
            if (setDateTimeForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            DateTime dtEndTime = setDateTimeForm.SelectedTime;
            this.UpdateCarePlan(dtEndTime, ServerData.NurCarePlanStatus.STOP);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuRollback_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.UpdateCarePlan(DateTime.MinValue, ServerData.NurCarePlanStatus.PROGRESS);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            //删除文档  会同时删除子文档，但是在删除过程中出现子文档删除失败，会导致下面不会执行删除状态的更新，故在Delete方法中当状态为5时亦能通过
            if (this.DeleteSelectedDocument())
            {
                DateTime dtNow = SysTimeService.Instance.Now;
                if (SystemContext.Instance.SystemOption.NpcNewModel)
                {
                    ToolStripItem item = sender as ToolStripItem;
                    NurCarePlanConfig config = item.Tag as NurCarePlanConfig;
                    this.UpdateCarePlan(dtNow, config.Status);
                }
                else
                {
                    this.UpdateCarePlan(dtNow, ServerData.NurCarePlanStatus.CANCELED);
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuSpecial_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            NurCarePlanConfig config = item.Tag as NurCarePlanConfig;
            if (config == null)
                return;
            DateTime dtOperTime = DateTime.MinValue;
            if (config.Status != "1")
            {
                SetDateTimeForm setDateTimeForm = new SetDateTimeForm();
                setDateTimeForm.Text = "评估时间";
                if (setDateTimeForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                dtOperTime = setDateTimeForm.SelectedTime;
            }
            this.UpdateCarePlan(dtOperTime, config.Status);
            if (config.IsNeedGetIn == false)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            DataTableViewRow row = this.dataTableView1.CurrentRow;
            NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;
            if (ncpInfo == null)
                return;

            NurDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(ncpInfo.DocID))
                shRet = DocumentService.Instance.GetDocInfo(ncpInfo.DocID, ref docInfo);
            else
                shRet = DocumentService.Instance.GetLatestDocInfo(ncpInfo.DocID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("文档信息下载失败!");
                return;
            }
            if (docInfo == null)
                return;

            this.ShowCarePlanEditForm();
            this.m_CarePlanEditForm.OpenDocument(docInfo, ncpInfo);
            this.m_CarePlanEditForm.UpdateDocument(sender.ToString());
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
        #endregion

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadNurCarePlanList(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowCarePlanEditForm();
            this.m_CarePlanEditForm.CreateDocument(this.m_docTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

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
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintDataTable_Click(object sender, EventArgs e)
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
            byte[] byteReportData;
            if (this.m_docTypeInfo != null)
                byteReportData = this.GetReportFileData(this.m_docTypeInfo.DocTypeName + "_1");
            else
                byteReportData = this.GetReportFileData("护理计划_1");
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuSaveReturn_Click(object sender, EventArgs e)
        {
            this.toolmnuSaveReturn.Checked = !this.toolmnuSaveReturn.Checked;
            string key = SystemConst.ConfigKey.NCP_SAVE_RETURN;
            string value = this.toolmnuSaveReturn.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void toolbtnOption_DropDownOpening(object sender, EventArgs e)
        {
            string key = SystemConst.ConfigKey.NCP_SAVE_RETURN;
            if (SystemContext.Instance.GetConfig(key, false))
                this.toolmnuSaveReturn.Checked = true;
            else
                this.toolmnuSaveReturn.Checked = false;
        }

        #region 删除续打   -ycc
        //private void toolmnuPrintFrom_Click(object sender, EventArgs e)
        //{
        //    this.Update();
        //    if (this.dataTableView1.SelectedRows.Count <= 0)
        //        return;
        //    GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
        //    if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
        //    {
        //        GlobalMethods.UI.SetCursor(this, Cursors.Default);
        //        MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
        //        return;
        //    }
        //    SetPageNumberForm setPageNumberForm = new SetPageNumberForm();
        //    if (setPageNumberForm.ShowDialog() != DialogResult.OK)
        //    {
        //        GlobalMethods.UI.SetCursor(this, Cursors.Default);
        //        return;
        //    }
        //    int nStartPageNumber = setPageNumberForm.StartPageNumber;

        //    byte[] byteReportData = this.GetReportFileData(null);
        //    if (byteReportData != null)
        //    {
        //        int start = this.dataTableView1.SelectedRows[0].Index;
        //        int end = this.dataTableView1.Rows.Count - 1;
        //        DataTable table =
        //            GlobalMethods.Table.GetDataTable(this.dataTableView1, start, end, 0);
        //        ReportExplorerForm explorerForm = this.GetReportExplorerForm();
        //        explorerForm.StartPageNo = nStartPageNumber;
        //        explorerForm.ReportFileData = byteReportData;
        //        explorerForm.ReportParamData.Add("是否续打", false);
        //        explorerForm.ReportParamData.Add("设置打印起始页码", nStartPageNumber);
        //        explorerForm.ReportParamData.Add("打印数据", table);
        //        explorerForm.ShowDialog();
        //    }
        //    GlobalMethods.UI.SetCursor(this, Cursors.Default);
        //}

        //private void toolmnuPrintContinued_Click(object sender, EventArgs e)
        //{
        //    this.Update();
        //    if (this.dataTableView1.SelectedRows.Count <= 0)
        //        return;
        //    GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
        //    if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
        //    {
        //        GlobalMethods.UI.SetCursor(this, Cursors.Default);
        //        MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
        //        return;
        //    }
        //    byte[] byteReportData = this.GetReportFileData(null);
        //    if (byteReportData != null)
        //    {
        //        int start = this.dataTableView1.SelectedRows[0].Index;
        //        DataTable table =
        //            GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
        //        ReportExplorerForm explorerForm = this.GetReportExplorerForm();
        //        explorerForm.ReportFileData = byteReportData;
        //        explorerForm.ReportParamData.Add("是否续打", true);
        //        explorerForm.ReportParamData.Add("续打行号", start);
        //        explorerForm.ReportParamData.Add("打印数据", table);
        //        explorerForm.ShowDialog();
        //    }
        //    GlobalMethods.UI.SetCursor(this, Cursors.Default);
        //}
        #endregion

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            NurCarePlanInfo ncpInfo = row.Tag as NurCarePlanInfo;
            if (ncpInfo == null)
                return;

            NurDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(ncpInfo.DocID))
                shRet = DocumentService.Instance.GetDocInfo(ncpInfo.DocID, ref docInfo);
            else
                shRet = DocumentService.Instance.GetLatestDocInfo(ncpInfo.DocID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("文档信息下载失败!");
                return;
            }
            if (docInfo == null)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowCarePlanEditForm();
            this.m_CarePlanEditForm.OpenDocument(docInfo, ncpInfo);
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
