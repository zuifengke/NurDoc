// ***********************************************************
// 护理电子病历系统,护理记录单列表窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.Report;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.PatPage.Document;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.PatPage.NurRec
{
    internal partial class NursingRecordForm : DockContentBase
    {
        private PatientPageControl m_patientPageControl = null;
        private Hashtable m_htColumnTable = null;
        private RecordEditForm m_RecordEditForm = null;
        private int m_detpIndex = 0;

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 获取病人主页面
        /// </summary>
        [Browsable(false)]
        public PatientPageControl PatientPageControl
        {
            get { return this.m_patientPageControl; }
        }

        /// <summary>
        /// 是否读取脚本小结
        /// </summary>
        [Browsable(false)]
        public Boolean IsLoadFromSc
        {
            get { return SystemContext.Instance.SystemOption.RecSummaryName != string.Empty ? true : false; }
        }

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.m_RecordEditForm == null)
                    return false;
                if (this.m_RecordEditForm.IsDisposed)
                    return false;
                return this.m_RecordEditForm.IsModified;
            }
        }

        public NursingRecordForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
            this.m_patientPageControl = patientPageControl;

            string text = SystemContext.Instance.WindowName.NursingRecord;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 300);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            this.LoadSchemasList();
            this.LoadGridViewColumns(this.toolcboSchemaList.SelectedItem as GridViewSchema);
            this.RefreshView();

            //设置界面打印信息的显示
            this.SetGridViewPrint();

            this.toolcboPatDeptList.Items.Add(string.Empty);

            this.toolcboSchemaList.SelectedIndexChanged +=
                new System.EventHandler(this.toolcboSchemaList_SelectedIndexChanged);
            this.toolcboPatDeptList.DropDown +=
                new EventHandler(this.toolcboPatDeptList_DropDown);
            this.toolcboPatDeptList.SelectedIndexChanged +=
                new System.EventHandler(this.toolcboPatDeptList_SelectedIndexChanged);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            this.m_bValueChangedEnabled = false;
            DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1).Date;
            if (PatientTable.Instance.ActivePatient != null)
                dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime.Date;

            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(SysTimeService.Instance.Now);
            if (SystemContext.Instance.SystemOption.DocLoadTime > 0)
            {
                DateTime dtBeginShowTime = dtEndTime.AddHours(-SystemContext.Instance.SystemOption.DocLoadTime).AddTicks(1);
                if (dtBeginTime < dtBeginShowTime)
                    dtBeginTime = dtBeginShowTime;
            }
            
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            this.LoadGridViewData(null);

            //初始化列表的时候,定位到最后一行
            this.dataTableView1.SelectRow(this.dataTableView1.Rows.Count - 1);

            if (this.m_RecordEditForm != null && !this.m_RecordEditForm.IsDisposed
                && this.m_RecordEditForm.Visible)
            {
                this.m_RecordEditForm.ResetModifyState();
                if (this.dataTableView1.Rows.Count > 0)
                    this.m_RecordEditForm.Hide();
                else
                    this.m_RecordEditForm.LoadNursingRecord(null, null);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
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
            if (this.IsModified)
                this.DockHandler.Activate();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.m_RecordEditForm == null)
                return false;
            if (this.m_RecordEditForm.IsDisposed)
                return false;
            return this.m_RecordEditForm.CheckModifyState();
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
            List<TransferInfo> lstTransferInfos = null;
            short shRet = PatVisitService.Instance.GetPatientTransferList(szPatientID, szVisitID, ref lstTransferInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("查询当前病人病区列表失败!");
                return false;
            }
            if (lstTransferInfos == null || lstTransferInfos.Count <= 0)
                return false;

            foreach (TransferInfo transferInfo in lstTransferInfos)
                this.toolcboPatDeptList.Items.Add(transferInfo);

            return true;
        }

        /// <summary>
        /// 加载护理记录列显示方案列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadSchemasList()
        {
            this.toolcboSchemaList.Items.Clear();
            this.Update();
            if (SystemContext.Instance.LoginUser == null)
                return false;

            //获取列显示方案列表
            string szSchemaType = ServerData.GridViewSchema.NURSING_RECORD;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<GridViewSchema> lstGridViewSchemas = null;
            short shRet = ConfigService.Instance.GetGridViewSchemas(szSchemaType, szWardCode, ref lstGridViewSchemas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("列显示方案列表下载失败!");
                return false;
            }
            if (lstGridViewSchemas == null)
                lstGridViewSchemas = new List<GridViewSchema>();

            //检查是否有私有方案
            bool bHasPrivate = false;
            foreach (GridViewSchema schema in lstGridViewSchemas)
            {
                if (schema != null && !GlobalMethods.Misc.IsEmptyString(schema.WardCode))
                {
                    bHasPrivate = true;
                    break;
                }
            }

            //加载列显示方案列表
            GridViewSchema defaultSchema = null;
            foreach (GridViewSchema schema in lstGridViewSchemas)
            {
                if (schema == null)
                    continue;
                if (!SystemContext.Instance.SystemOption.ShowAllInNurRec && bHasPrivate && GlobalMethods.Misc.IsEmptyString(schema.WardCode))
                    continue;
                if (schema.IsDefault)
                    defaultSchema = schema;
                this.toolcboSchemaList.Items.Add(schema);
            }
            this.toolcboSchemaList.SelectedItem = defaultSchema;
            if (this.toolcboSchemaList.SelectedIndex < 0 && this.toolcboSchemaList.Items.Count > 0)
                this.toolcboSchemaList.SelectedIndex = 0;
            return true;
        }

        /// <summary>
        /// 加载指定方案下的护理记录列列表
        /// </summary>
        /// <param name="schema">列显示方案</param>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewColumns(GridViewSchema schema)
        {
            if (this.m_htColumnTable == null)
                this.m_htColumnTable = new Hashtable();
            this.m_htColumnTable.Clear();

            //清除已显示的各体征项目列
            int index = this.dataTableView1.Columns.Count - 1;
            while (index >= 0)
            {
                DataGridViewColumn column = this.dataTableView1.Columns[index--];
                if (column == this.colRecordTime)
                    continue;
                this.dataTableView1.Columns.Remove(column);
            }
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
                if (!this.m_htColumnTable.Contains(column.Name))
                    this.m_htColumnTable.Add(column.Name, column);
            }
            return true;
        }

        /// <summary>
        /// 加载护理记录数据列表
        /// </summary>
        /// <param name="szDefaultSelectedRecordID">默认选中的记录</param>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewData(string szDefaultSelectedRecordID)
        {
            if (this.dataTableView1.CurrentRow != null
                && string.IsNullOrEmpty(szDefaultSelectedRecordID))
            {
                NursingRecInfo recInfo = this.dataTableView1.CurrentRow.Tag as NursingRecInfo;
                if (recInfo != null)
                    szDefaultSelectedRecordID = recInfo.RecordID;
            }
            this.dataTableView1.Rows.Clear();
            Application.DoEvents();

            if (PatientTable.Instance.ActivePatient == null)
                return false;
            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            string szSubID = PatientTable.Instance.ActivePatient.SubID;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value;
            DateTime dtEndTime = this.tooldtpDateTo.Value;
            //DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);
            List<NursingRecInfo> lstNursingRecInfos = new List<NursingRecInfo>();
            short shRet = NurRecService.Instance.GetNursingRecList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ref lstNursingRecInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录列表下载失败!");
                return false;
            }

            bool bshowByType = SystemContext.Instance.SystemOption.OptionRecShowByType;
            DateTime ShowByTypeStartTime = SystemContext.Instance.SystemOption.OptionRecShowByTypeStartTime;

            TransferInfo currentDept = this.toolcboPatDeptList.SelectedItem as TransferInfo;
            m_detpIndex = this.toolcboPatDeptList.SelectedIndex;

            Hashtable htNursingRec = new Hashtable();
            this.dataTableView1.SuspendLayout();
            string szSchemaID = this.GetCurrentSchema().SchemaID;
            foreach (NursingRecInfo nursingRecInfo in lstNursingRecInfos)
            {
                if (currentDept != null && nursingRecInfo.WardCode != currentDept.WardCode)
                    continue;
                if (bshowByType && nursingRecInfo.SchemaID != szSchemaID && nursingRecInfo.RecordTime > ShowByTypeStartTime)
                    continue;

                int rowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[rowIndex];
                row.Tag = nursingRecInfo;
                row.Cells[this.colRecordTime.Index].Value = nursingRecInfo.RecordTime.ToString("yyyy-MM-dd HH:mm");
                this.dataTableView1.SetRowState(row, RowState.Normal);

                //特殊处理签名列
                DataGridViewColumn column = this.m_htColumnTable["签名人1"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                    row.Cells[column.Index].Value = nursingRecInfo.Recorder1Name;

                //特殊处理签名列
                column = this.m_htColumnTable["签名人2"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                    row.Cells[column.Index].Value = nursingRecInfo.Recorder2Name;

                //特殊处理记录内容列
                column = this.m_htColumnTable["记录内容"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                {
                    string content = nursingRecInfo.RecordContent + nursingRecInfo.RecordRemarks;
                    row.Cells[column.Index].Value = content;
                }

                this.HandleNursingRecordSummary(row, nursingRecInfo);

                if (!htNursingRec.ContainsKey(nursingRecInfo.RecordID))
                    htNursingRec.Add(nursingRecInfo.RecordID, row);
                if (nursingRecInfo.RecordID == szDefaultSelectedRecordID)
                    this.dataTableView1.SelectRow(row);
            }

            List<SummaryData> lstSummaryDatas = null;
            shRet = DocumentService.Instance.GetSummaryData(szPatientID, szVisitID, dtBeginTime, dtEndTime, ref lstSummaryDatas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理评估摘要数据列表下载失败!");
                return false;
            }
            if (lstSummaryDatas == null)
                lstSummaryDatas = new List<SummaryData>();
            //m_lstSummaryDatas = lstSummaryDatas;//获取关键数据用于数据快速重现
            foreach (SummaryData summaryData in lstSummaryDatas)
            {
                if (string.IsNullOrEmpty(summaryData.RecordID))
                    continue;
                if (string.IsNullOrEmpty(summaryData.DataName))
                    continue;
                DataGridViewRow row = htNursingRec[summaryData.RecordID] as DataGridViewRow;
                if (row == null)
                    continue;

                //特殊处理高/低血压
                bool success = this.HandleBloodPressData(row, summaryData);

                //特殊处理心率/脉搏
                if (!success)
                    this.HandlePulseHeartRateData(row, summaryData);

                //处理其他列的数据(即使success为true,也要执行)
                DataGridViewColumn column = this.m_htColumnTable[summaryData.DataName] as DataGridViewColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                    row.Cells[column.Index].Value = summaryData.DataValue;
                if (column == null || summaryData.DataValue == null)
                    continue;
                row.Cells[column.Index].Value = SystemContext.Instance.GetVitalSignsValue(summaryData.DataName, summaryData.DataValue);
            }
            this.dataTableView1.PerformLayout();

            this.OptimizeDataView();

            //根据单元格内容调整行高度
            int nMinHeight = this.dataTableView1.RowTemplate.Height;
            this.dataTableView1.AdjustRowHeight(0, this.dataTableView1.Rows.Count, nMinHeight, (nMinHeight * 20) + 4);

            //将滚动条自动滚动到当前行
            DataTableViewRow currentRow = this.dataTableView1.CurrentRow;
            this.dataTableView1.CurrentCell = null;
            this.dataTableView1.SelectRow(currentRow);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 处理护理记录小结数据行的显示
        /// </summary>
        /// <param name="row">当前行</param>
        /// <param name="nursingRecInfo">护理记录信息</param>
        /// <returns>是否处理成功</returns>
        private bool HandleNursingRecordSummary(DataTableViewRow row, NursingRecInfo nursingRecInfo)
        {
            if (row == null || nursingRecInfo == null || nursingRecInfo.SummaryFlag != 1)
                return false;

            row.DefaultCellStyle.ForeColor = Color.Red;

            string szSummaryName = null;
            if (nursingRecInfo.SummaryName != null)
                szSummaryName = nursingRecInfo.SummaryName;
            DataGridViewColumn column = null;

            //将小结名称放到入量名称列
            column = this.m_htColumnTable["入量名称"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = nursingRecInfo.SummaryName;

            string szEnterTotal = string.Empty, szOutTotal = string.Empty;
            if (this.IsLoadFromSc)
            {
                //将小结名称放到入量名称列并将入量信息详细显示
                string szEnterName = string.Empty, szOutName = string.Empty;
                if (nursingRecInfo.RecordRemarks != null)
                {
                    string[] array = nursingRecInfo.RecordRemarks.Split('$');
                    if (array != null && array.Length > 0)
                    {
                        string[] szNameValue = array[0].Split('￥');
                        if (szNameValue != null && szNameValue.Length > 0)
                        {
                            szEnterName = szNameValue[0];
                        }
                        if (szNameValue != null && szNameValue.Length > 1)
                        {
                            szEnterTotal = szNameValue[1];
                        }
                    }
                    if (array != null && array.Length > 1)
                    {
                        string[] szNameValue = array[1].Split('￥');
                        if (szNameValue != null && szNameValue.Length > 0)
                        {
                            szOutName = szNameValue[0];
                        }
                        if (szNameValue != null && szNameValue.Length > 1)
                        {
                            szOutTotal = szNameValue[1];
                        }
                    }
                }
                column = this.m_htColumnTable["入量名称"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                    row.Cells[column.Index].Value += szEnterName;
                column = this.m_htColumnTable["出量名称"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                    row.Cells[column.Index].Value = szOutName;
            }
            else
            {
                if (nursingRecInfo.RecordRemarks != null)
                {
                    string[] array = nursingRecInfo.RecordRemarks.Split('$');
                    if (array != null && array.Length > 0)
                        szEnterTotal = array[0];
                    if (array != null && array.Length > 1)
                        szOutTotal = array[1];
                }
            }

            column = this.m_htColumnTable["入量"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = szEnterTotal;
            column = this.m_htColumnTable["出量"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = szOutTotal;

            //特殊处理记录内容列
            column = this.m_htColumnTable["记录内容"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = nursingRecInfo.RecordContent;
            return true;
        }

        /// <summary>
        /// 特殊处理当前护理记录单高低血压显示数据,这俩数据需要显示在一个单元格
        /// </summary>
        /// <param name="row">当前行</param>
        /// <param name="summaryData">摘要数据</param>
        /// <returns>是否处理成功</returns>
        private bool HandleBloodPressData(DataGridViewRow row, SummaryData summaryData)
        {
            if (row == null || summaryData == null)
                return false;
            NursingRecInfo nursingRecInfo = row.Tag as NursingRecInfo;

            int nBPCount = SystemContext.Instance.SystemOption.OptionBPCount;
            if (nBPCount <= 0)
                return false;
            string strFlag = string.Empty;

            for (int count = 0; count < nBPCount; count++)
            {
                if (count == 0)
                    strFlag = string.Empty;
                else
                    strFlag = (count + 1) + string.Empty;
                if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("血压high" + strFlag)
                    && this.m_htColumnTable.Contains("血压"))
                {
                    DataGridViewTextBoxColumn column = this.m_htColumnTable["血压"] as DataGridViewTextBoxColumn;
                    if (column != null && column.Index >= 0)
                    {
                        string szCellValue = string.Empty;
                        if (row.Cells[column.Index].Value != null)
                            szCellValue = row.Cells[column.Index].Value.ToString().Trim();

                        if (szCellValue == null || !szCellValue.StartsWith("/"))
                            row.Cells[column.Index].Value = summaryData.DataValue + "/";
                        else
                            row.Cells[column.Index].Value = summaryData.DataValue + szCellValue;
                        return true;
                    }
                }
                else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("血压Low" + strFlag)
                    && this.m_htColumnTable.Contains("血压"))
                {
                    DataGridViewTextBoxColumn column = this.m_htColumnTable["血压"] as DataGridViewTextBoxColumn;
                    if (column != null && column.Index >= 0)
                    {
                        string szCellValue = string.Empty;
                        if (row.Cells[column.Index].Value != null)
                            szCellValue = row.Cells[column.Index].Value.ToString().Trim();

                        if (szCellValue == null || !szCellValue.EndsWith("/"))
                            row.Cells[column.Index].Value = "/" + summaryData.DataValue;
                        else
                            row.Cells[column.Index].Value = szCellValue + summaryData.DataValue;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 特殊处理当前护理记录单心率/脉搏显示数据,这俩数据需要显示在一个单元格
        /// </summary>
        /// <param name="row">当前行</param>
        /// <param name="summaryData">摘要数据</param>
        /// <returns>是否处理成功</returns>
        private bool HandlePulseHeartRateData(DataGridViewRow row, SummaryData summaryData)
        {
            if (row == null || summaryData == null)
                return false;

            if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("心率")
                && this.m_htColumnTable.Contains("心率/脉搏"))
            {
                DataGridViewTextBoxColumn column = this.m_htColumnTable["心率/脉搏"] as DataGridViewTextBoxColumn;
                if (column != null && column.Index >= 0)
                {
                    string szCellValue = string.Empty;
                    if (row.Cells[column.Index].Value != null)
                        szCellValue = row.Cells[column.Index].Value.ToString().Trim();

                    if (szCellValue == null || !szCellValue.StartsWith("/"))
                        row.Cells[column.Index].Value = summaryData.DataValue + "/";
                    else
                        row.Cells[column.Index].Value = summaryData.DataValue + szCellValue;
                    return true;
                }
            }
            else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("脉搏")
                && this.m_htColumnTable.Contains("心率/脉搏"))
            {
                DataGridViewTextBoxColumn column = this.m_htColumnTable["心率/脉搏"] as DataGridViewTextBoxColumn;
                if (column != null && column.Index >= 0)
                {
                    string szCellValue = string.Empty;
                    if (row.Cells[column.Index].Value != null)
                        szCellValue = row.Cells[column.Index].Value.ToString().Trim();

                    if (szCellValue == null || !szCellValue.EndsWith("/"))
                        row.Cells[column.Index].Value = "/" + summaryData.DataValue;
                    else
                        row.Cells[column.Index].Value = szCellValue + summaryData.DataValue;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 优化表格数据
        /// </summary>
        private void OptimizeDataView()
        {
            if (this.dataTableView1.Rows.Count <= 0
                && this.m_htColumnTable == null && this.m_htColumnTable.Count <= 0)
                return;
            DataGridViewTextBoxColumn column1 = this.m_htColumnTable["血压"] as DataGridViewTextBoxColumn;
            DataGridViewTextBoxColumn column2 = this.m_htColumnTable["心率/脉搏"] as DataGridViewTextBoxColumn;
            foreach (DataGridViewRow row in this.dataTableView1.Rows)
            {
                object value1 = null;
                if (column1 != null && column1.Index >= 0)
                    value1 = row.Cells[column1.Index].Value;
                object value2 = null;
                if (column2 != null && column2.Index >= 0)
                    value2 = row.Cells[column2.Index].Value;

                if (value1 != null && value1.ToString().Trim() == "/")
                    row.Cells[column1.Index].Value = string.Empty;
                if (value2 != null && value2.ToString().Trim() == "/")
                    row.Cells[column2.Index].Value = string.Empty;
            }
        }

        /// <summary>
        /// 删除指定护理记录包含的出入量累计时间数据
        /// </summary>
        /// <param name="recInfo">护理记录信息</param>
        /// <returns>是否删除成功</returns>
        private bool DeleteVitalSignsDataByTime(NursingRecInfo recInfo)
        {
            if (recInfo == null)
                return false;
            //添加小结时录入的累计时间随记录的删除而修改

            int index = recInfo.SummaryName.IndexOf("小时");
            if (index < 0)
            {
                MessageBoxEx.ShowError("小结名称错误");
                return false;
            }
            int num = 0;
            num = GlobalMethods.Convert.StringToValue(recInfo.SummaryName.Substring(0, index), 0);
            if (num < 24)
            {
                string szPatientID = recInfo.PatientID;
                string szVisitID = recInfo.VisitID;
                string szSubID = recInfo.SubID;
                DateTime reTime = recInfo.RecordTime;
                List<VitalSignsData> lstVitalSignsData = null;
                short shRec = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, szSubID
                    , reTime, reTime, ref lstVitalSignsData);
                if (shRec != SystemConst.ReturnValue.OK)
                    return false;
                if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
                {
                    List<VitalSignsData> lstSignsData = new List<VitalSignsData>();
                    foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                    {
                        if (vitalSignsData.RecordName.Contains("累计时间"))
                        {
                            vitalSignsData.RecordData = string.Empty;
                            lstSignsData.Add(vitalSignsData);
                        }
                    }
                    shRec = VitalSignsService.Instance.SaveVitalSignsData(lstSignsData);
                    if (shRec != SystemConst.ReturnValue.OK)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 删除指定护理记录包含的所有体征数据
        /// </summary>
        /// <param name="recInfo">护理记录信息</param>
        /// <returns>是否删除成功</returns>
        private bool DeleteVitalSignsData(NursingRecInfo recInfo)
        {
            if (recInfo == null)
                return false;
            string szPatientID = recInfo.PatientID;
            string szVisitID = recInfo.VisitID;
            string szSubID = recInfo.SubID;
            string szSourceTag = recInfo.RecordID;
            string szSourceType = ServerData.VitalSignsSourceType.NUR_REC;
            List<VitalSignsData> lstVitalSignsData = null;
            short shRet = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, szSubID
                , szSourceTag, szSourceType, ref lstVitalSignsData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;

            if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
            {
                foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                    vitalSignsData.RecordData = string.Empty;
                shRet = VitalSignsService.Instance.SaveVitalSignsData(lstVitalSignsData);
                if (shRet != SystemConst.ReturnValue.OK)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 删除当前选中的护理记录 同时更新文档状态为已作废 并删除体征表和摘要表
        /// </summary>
        /// <returns>是否删除成功</returns>
        private bool DeleteSelectedRecord()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return false;
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return false;
            NursingRecInfo recInfo = this.dataTableView1.SelectedRows[0].Tag as NursingRecInfo;
            if (!RightController.Instance.CanDeleteNurRec(recInfo))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }

            DialogResult result = MessageBoxEx.ShowConfirm("确认删除选中的护理记录吗？");
            if (result != DialogResult.OK)
                return false;
            Application.DoEvents();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            string szRecordID = recInfo.RecordID;
            string szModifierID = SystemContext.Instance.LoginUser.ID;
            string szModifierName = SystemContext.Instance.LoginUser.Name;
            short shRet = NurRecService.Instance.DeleteNursingRec(szRecordID, szModifierID, szModifierName);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录删除失败!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
            //根据记录id获取文档列表，再更新文档状态
            NurDocList lstDocInfos = null;
            shRet = DocumentService.Instance.GetRecordDocInfos(szRecordID, ref lstDocInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录摘要数据删除失败!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
            else
            {
                DocStatusInfo docstatusinfo = null;
                if (lstDocInfos != null)
                {
                    foreach (NurDocInfo nurdocinfo in lstDocInfos)
                    {
                        if (nurdocinfo != null)
                        {
                            shRet = DocumentService.Instance.GetDocStatusInfo(nurdocinfo.DocID, ref docstatusinfo);
                            if (shRet != SystemConst.ReturnValue.OK)
                            {
                                MessageBoxEx.ShowError("获取文档状态失败!");
                                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                                return false;
                            }
                            else
                            {
                                docstatusinfo.DocStatus = ServerData.DocStatus.CANCELED;
                                //更新文档状态并删除摘要数据
                                shRet = DocumentService.Instance.SetDocStatusInfo(ref docstatusinfo);
                                if (shRet != SystemConst.ReturnValue.OK)
                                {
                                    MessageBoxEx.ShowError("更新文档删除状态失败!");
                                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            //shRet = DocumentService.Instance.DeleteSummaryData(szRecordID);
            //if (shRet != SystemConst.ReturnValue.OK)
            //{
            //    MessageBoxEx.ShowError("护理记录摘要数据删除失败!");
            //    GlobalMethods.UI.SetCursor(this, Cursors.Default);
            //    return false;
            //}
            this.dataTableView1.Rows.Remove(this.dataTableView1.SelectedRows[0]);

            if (!this.DeleteVitalSignsData(recInfo))
                MessageBoxEx.ShowError("护理记录已删除成功! 但该护理记录中的体征数据删除失败!");

            //同步删除因小结而插入的累计时间
            if (recInfo.SummaryName.Contains("小时小结") || recInfo.SummaryName.Contains("小时总结"))
            {
                if (!this.DeleteVitalSignsDataByTime(recInfo))
                    MessageBoxEx.ShowError("护理记录已删除成功! 但该护理记录中的出入量累计时间体征数据删除失败!");
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 创建记录编辑器窗口
        /// </summary>
        private void CreateRecordEditForm()
        {
            if (this.m_RecordEditForm == null || this.m_RecordEditForm.IsDisposed)
            {
                this.m_RecordEditForm = new RecordEditForm();
                this.m_RecordEditForm.ShowInTaskbar = false;
                this.m_RecordEditForm.MinimizeBox = false;
                this.m_RecordEditForm.StartPosition = FormStartPosition.CenterParent;
                this.m_RecordEditForm.RecordUpdated +=
                    new EventHandler(this.RecordEditForm_RecordUpdated);
            }
            //this.m_RecordEditForm.IsFirstRecord = this.dataTableView1.Rows.Count <= 0;//由于显示内容可配置为非全部显示
            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            string szSubID = PatientTable.Instance.ActivePatient.SubID;
            DateTime dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime;
            int NursingRecCount = 0;
            short shRet = NurRecService.Instance.GetNursingRecCount(szPatientID, szVisitID,szSubID
                ,dtBeginTime, dtBeginTime.AddMonths(1), ref NursingRecCount);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录查询失败!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return ;
            }
            this.m_RecordEditForm.IsFirstRecord = NursingRecCount <= 0;
            
            this.m_RecordEditForm.DicSummaryDatas = null;

            string key = SystemConst.ConfigKey.NUR_REC_SHOW_AS_MODEL;
            if (!SystemContext.Instance.GetConfig(key, false))
            {
                this.m_RecordEditForm.TopLevel = false;
                this.m_RecordEditForm.FormBorderStyle = FormBorderStyle.None;
                this.m_RecordEditForm.Dock = DockStyle.Fill;
                this.m_RecordEditForm.Parent = this;
            }
            else
            {
                this.m_RecordEditForm.Parent = null;
                this.m_RecordEditForm.TopLevel = true;
                this.m_RecordEditForm.Dock = DockStyle.None;
                this.m_RecordEditForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        /// <summary>
        /// 获取用户当前选择的护理记录单类型
        /// </summary>
        /// <returns>列方案信息</returns>
        private GridViewSchema GetCurrentSchema()
        {
            return this.toolcboSchemaList.SelectedItem as GridViewSchema;
        }

        /// <summary>
        /// 显示指定护理记录对应的护理电子病历编辑器
        /// </summary>
        /// <param name="nursingRecInfo">关联的护理记录</param>
        /// <param name="docTypeInfo">默认显示的评估单</param>
        private void ShowRecordEditForm(NursingRecInfo nursingRecInfo, DocTypeInfo docTypeInfo)
        {
            if (nursingRecInfo != null && nursingRecInfo.SummaryFlag == 1)
            {
                if (this.IsLoadFromSc)
                {
                    NurRecSummaryInScForm dialog = new NurRecSummaryInScForm();
                    dialog.CurrentSchema = this.GetCurrentSchema();
                    if (dialog.ShowDialog(nursingRecInfo) == DialogResult.OK)
                    {
                        if (dialog.NursingRecInfo != null)
                            this.LoadGridViewData(nursingRecInfo.RecordID);
                    }
                    return;
                }
                else
                {
                    NurRecSummaryForm dialog = new NurRecSummaryForm();
                    dialog.CurrentSchema = this.GetCurrentSchema();
                    if (dialog.ShowDialog(nursingRecInfo) == DialogResult.OK)
                    {
                        if (dialog.NursingRecInfo != null)
                            this.LoadGridViewData(nursingRecInfo.RecordID);
                    }
                    return;
                }
            }
            this.CreateRecordEditForm();

            //List<SummaryData> lstSummaryDatas = null;
            //if (nursingRecInfo != null)
            //{
            //    short shRet = DocumentService.Instance.GetSummaryData(nursingRecInfo.RecordID, true, ref lstSummaryDatas);
            //    if (shRet != SystemConst.ReturnValue.OK)
            //    {
            //        MessageBoxEx.ShowError("护理评估摘要数据列表下载失败!");
            //        return;
            //    }
            //}
            //if (lstSummaryDatas == null)
            //    lstSummaryDatas = new List<SummaryData>();

            Dictionary<string, Heren.Common.Forms.Editor.KeyData> lstSummaryDatas =
                    new Dictionary<string, Heren.Common.Forms.Editor.KeyData>();
            if (nursingRecInfo != null)
            {
                short shRet = DocumentService.Instance.GetSummaryData(nursingRecInfo.RecordID, true, ref lstSummaryDatas);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("护理评估摘要数据列表下载失败!");
                    return;
                }
            }

            if (lstSummaryDatas == null)
                lstSummaryDatas = new Dictionary<string, Heren.Common.Forms.Editor.KeyData>();

            string key = SystemConst.ConfigKey.NUR_REC_SHOW_AS_MODEL;
            if (SystemContext.Instance.GetConfig(key, false))
            {
                this.m_RecordEditForm.CurrentSchema = this.GetCurrentSchema();
                this.m_RecordEditForm.DicSummaryDatas = lstSummaryDatas;
                this.m_RecordEditForm.ShowDialog(this, nursingRecInfo, docTypeInfo);
            }
            else
            {
                GlobalMethods.UI.SetCursor(this.m_RecordEditForm, Cursors.WaitCursor);
                this.m_RecordEditForm.Visible = true;
                this.m_RecordEditForm.BringToFront();
                this.m_RecordEditForm.CurrentSchema = this.GetCurrentSchema();
                this.m_RecordEditForm.DicSummaryDatas = lstSummaryDatas;
                this.m_RecordEditForm.LoadNursingRecord(nursingRecInfo, docTypeInfo);
                GlobalMethods.UI.SetCursor(this.m_RecordEditForm, Cursors.Default);
            }
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
            if (docTypeInfo.ApplyEnv != ServerData.DocTypeApplyEnv.NURSING_RECORD)
                return false;

            this.Activate();
            Application.DoEvents();
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
                return true;

            NurDocInfo docInfo = null;
            short shRet = DocumentService.Instance.GetDocInfo(szDocID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的病历记录!", szDocID));
                return true;
            }

            NursingRecInfo nursingRecInfo = null;
            shRet = NurRecService.Instance.GetNursingRec(docInfo.RecordID, docInfo.PatientID, docInfo.VisitID, ref nursingRecInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的护理记录!", docInfo.RecordID));
                return true;
            }

            //显示护理记录编辑窗体
            this.ShowRecordEditForm(nursingRecInfo, docTypeInfo);
            return true;
        }

        /// <summary>
        /// 加载护理记录的打印信息
        /// </summary>
        private void SetGridViewPrint()
        {
            for (int i = 0; i <= this.dataTableView1.Rows.Count - 1; i++)
            {
                NursingRecInfo nurRec = this.dataTableView1.Rows[i].Tag as NursingRecInfo;
                if (nurRec.RecordPrinted == 1)
                    this.dataTableView1.Rows[i].DefaultCellStyle.BackColor = Color.Silver;
            }
        }

        ///<summary>
        /// 设置指定的打印信息
        /// </summary>
        public void SetNursingRecPrint(int intPrint)
        {
            if (intPrint == 0)
            {
                for (int i = 0; i <= this.dataTableView1.Rows.Count - 1; i++)
                {
                    NursingRecInfo nurRec = this.dataTableView1.Rows[i].Tag as NursingRecInfo;
                    if (!GlobalMethods.Misc.IsEmptyString(nurRec.RecordID))
                    {
                        if (nurRec.RecordPrinted != 1)
                        {
                            short shUpdate = NurRecService.Instance.UpdatePrintState(nurRec.RecordID);
                            if (shUpdate != SystemConst.ReturnValue.OK)
                                MessageBoxEx.ShowError("更新护理打印信息失败！");
                        }
                    }
                    this.dataTableView1.Rows[i].DefaultCellStyle.BackColor = Color.Silver;
                }
            }
            if (intPrint == 1)
            {
                int sum = this.dataTableView1.SelectedRows[0].Index;
                NursingRecInfo nurRec = this.dataTableView1.Rows[sum].Tag as NursingRecInfo;
                if (!GlobalMethods.Misc.IsEmptyString(nurRec.RecordID))
                {
                    if (nurRec.RecordPrinted != 1)
                    {
                        short shUpdate = NurRecService.Instance.UpdatePrintState(nurRec.RecordID);
                        if (shUpdate != SystemConst.ReturnValue.OK)
                            MessageBoxEx.ShowError("更新护理打印信息失败！");
                    }
                }
                this.dataTableView1.Rows[sum].DefaultCellStyle.BackColor = Color.Silver;
            }
            if (intPrint == 2)
            {
                int sum = this.dataTableView1.SelectedRows[0].Index;
                for (int i = sum; i <= this.dataTableView1.Rows.Count - 1; i++)
                {
                    NursingRecInfo nurRec = this.dataTableView1.Rows[i].Tag as NursingRecInfo;
                    if (!GlobalMethods.Misc.IsEmptyString(nurRec.RecordID))
                    {
                        if (nurRec.RecordPrinted != 1)
                        {
                            short shUpdate = NurRecService.Instance.UpdatePrintState(nurRec.RecordID);
                            if (shUpdate != SystemConst.ReturnValue.OK)
                                MessageBoxEx.ShowError("更新护理打印信息失败！");
                        }
                    }
                    this.dataTableView1.Rows[i].DefaultCellStyle.BackColor = Color.Silver;
                }
            }
            if (intPrint == 3)
            {
                int sum = this.dataTableView1.SelectedRows[0].Index;
                for (int i = sum; i <= this.dataTableView1.Rows.Count - 1; i++)
                {
                    NursingRecInfo nurRec = this.dataTableView1.Rows[i].Tag as NursingRecInfo;
                    if (!GlobalMethods.Misc.IsEmptyString(nurRec.RecordID))
                    {
                        if (nurRec.RecordPrinted != 1)
                        {
                            short shUpdate = NurRecService.Instance.UpdatePrintState(nurRec.RecordID);
                            if (shUpdate != SystemConst.ReturnValue.OK)
                                MessageBoxEx.ShowError("更新护理打印信息失败！");
                        }
                    }
                    this.dataTableView1.Rows[i].DefaultCellStyle.BackColor = Color.Silver;
                }
            }
        }

        #region "打印护理记录"
        /// <summary>
        /// 加载护理记录单打印模板
        /// </summary>
        /// <param name="szReportName">护理记录单打印模板名</param>
        /// <returns>护理记录单打印模板byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_RECORD;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.toolcboSchemaList.Text.Trim();
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowErrorFormat("{0}打印报表还没有制作!", null, szReportName);
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowErrorFormat("{0}打印报表内容下载失败!", null, szReportName);
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
            reportExplorerForm.MaxPreviewPageCount = SystemContext.Instance.SystemOption.MaxPreviewPages;
            return reportExplorerForm;
        }
        #endregion

        private void RecordEditForm_RecordUpdated(object sender, EventArgs e)
        {
            if (this.m_RecordEditForm == null || this.m_RecordEditForm.IsDisposed)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_RecordEditForm.NursingRecInfo == null)
                this.LoadGridViewData(null);
            else
                this.LoadGridViewData(this.m_RecordEditForm.NursingRecInfo.RecordID);
            this.SetGridViewPrint();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadGridViewData(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintAll_Click(object sender, EventArgs e)
        {
            this.Update();

            //获取病人就诊时间
            DateTime dtVisitTime = PatientTable.Instance.ActivePatient.VisitTime.Date;
            TimeSpan ts = tooldtpDateFrom.Value.Date - dtVisitTime;
            DataTable table = new DataTable();
            DataTableView dataTableView = new DataTableView();
            if (ts.Days > 0)
            {
                for (int i = 0; i < this.dataTableView1.ColumnCount;i++ )
                { 
                    dataTableView.Columns.Add(new DataGridViewTextBoxColumn());
                    dataTableView.Columns[i].Name = this.dataTableView1.Columns[i].Name;
                    dataTableView.Columns[i].Tag = this.dataTableView1.Columns[i].Tag;
                }

                //获取病人所有护理记录
                string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
                string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
                string szSubID = PatientTable.Instance.ActivePatient.SubID;
                List<NursingRecInfo> lstNursingRecInfos = new List<NursingRecInfo>();
                short shRet = NurRecService.Instance.GetNursingRecList(szPatientID, szVisitID, szSubID, dtVisitTime, tooldtpDateTo.Value.Date.Add(new TimeSpan(23,59,59)), ref lstNursingRecInfos);
                if (shRet != SystemConst.ReturnValue.OK)
                    return;

                bool bshowByType = SystemContext.Instance.SystemOption.OptionRecShowByType;
                DateTime ShowByTypeStartTime = SystemContext.Instance.SystemOption.OptionRecShowByTypeStartTime;

                TransferInfo currentDept = this.toolcboPatDeptList.SelectedItem as TransferInfo;
                this.m_detpIndex = this.toolcboPatDeptList.SelectedIndex;

                Hashtable htNursingRec = new Hashtable();

                string szSchemaID = this.GetCurrentSchema().SchemaID;
                foreach (NursingRecInfo nursingRecInfo in lstNursingRecInfos)
                {
                    if (currentDept != null && nursingRecInfo.WardCode != currentDept.WardCode)
                        continue;
                    if (bshowByType && nursingRecInfo.SchemaID != szSchemaID && nursingRecInfo.RecordTime > ShowByTypeStartTime)
                        continue;

                    int rowIndex = dataTableView.Rows.Add();
                    DataTableViewRow row = dataTableView.Rows[rowIndex];
                    row.Tag = nursingRecInfo;
                    row.Cells[this.colRecordTime.Index].Value = nursingRecInfo.RecordTime.ToString("yyyy-MM-dd HH:mm");
                    dataTableView.SetRowState(row, RowState.Normal);

                    //特殊处理签名列
                    DataGridViewColumn column = this.m_htColumnTable["签名人1"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0)
                        row.Cells[column.Index].Value = nursingRecInfo.Recorder1Name;

                    //特殊处理签名列
                    column = this.m_htColumnTable["签名人2"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0)
                        row.Cells[column.Index].Value = nursingRecInfo.Recorder2Name;

                    //特殊处理记录内容列
                    column = this.m_htColumnTable["记录内容"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0)
                    {
                        string content = nursingRecInfo.RecordContent + nursingRecInfo.RecordRemarks;
                        row.Cells[column.Index].Value = content;
                    }

                    this.HandleNursingRecordSummary(row, nursingRecInfo);

                    if (!htNursingRec.ContainsKey(nursingRecInfo.RecordID))
                        htNursingRec.Add(nursingRecInfo.RecordID, row);
                }

                List<SummaryData> lstSummaryDatas = null;
                shRet = DocumentService.Instance.GetSummaryData(szPatientID, szVisitID, dtVisitTime, tooldtpDateTo.Value.Date.Add(new TimeSpan(23,59,59)), ref lstSummaryDatas);
                if (shRet != SystemConst.ReturnValue.OK)
                    return;
                if (lstSummaryDatas == null)
                    lstSummaryDatas = new List<SummaryData>();
                //m_lstSummaryDatas = lstSummaryDatas;//获取关键数据用于数据快速重现
                foreach (SummaryData summaryData in lstSummaryDatas)
                {
                    if (string.IsNullOrEmpty(summaryData.RecordID))
                        continue;
                    if (string.IsNullOrEmpty(summaryData.DataName))
                        continue;
                    DataGridViewRow row = htNursingRec[summaryData.RecordID] as DataGridViewRow;
                    if (row == null)
                        continue;

                    //特殊处理高/低血压
                    bool success = this.HandleBloodPressData(row, summaryData);

                    //特殊处理心率/脉搏
                    if (!success)
                        this.HandlePulseHeartRateData(row, summaryData);

                    //处理其他列的数据(即使success为true,也要执行)
                    DataGridViewColumn column = this.m_htColumnTable[summaryData.DataName] as DataGridViewColumn;
                    if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                        row.Cells[column.Index].Value = summaryData.DataValue;
                    if (column == null || summaryData.DataValue == null)
                        continue;
                    row.Cells[column.Index].Value = SystemContext.Instance.GetVitalSignsValue(summaryData.DataName, summaryData.DataValue);
                }
                table = GlobalMethods.Table.GetDataTable(dataTableView, false, 1);
            }
            else
            {
                table = GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);
            }
            
            if (this.dataTableView1.Rows.Count <= 0 && dataTableView.Rows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理记录!");
                return;
            }
            int ipagesNo = 0;
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                //DataTable table =
                //    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("转科情况", m_detpIndex);
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                //explorerForm.showPrintButton = true;
                if (explorerForm.ShowDialog() == DialogResult.OK)
                    this.SetNursingRecPrint(0);

                if (int.TryParse(explorerForm.GetReportData("获取打印页数").ToString(), out ipagesNo))
                {
                    RecPrintinfo RecPrintinfo = new RecPrintinfo();
                    GridViewSchema schema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
                    RecPrintinfo.PatientID = PatientTable.Instance.ActivePatient.PatientId;
                    RecPrintinfo.VisitID = PatientTable.Instance.ActivePatient.VisitId;
                    RecPrintinfo.WardCode = PatientTable.Instance.ActivePatient.WardCode;
                    RecPrintinfo.SchemaID = schema.SchemaID;
                    RecPrintinfo.PrintPages = ipagesNo;
                    RecPrintinfo.PrintTime = SysTimeService.Instance.Now;
                    RecPrintinfo.UserID = SystemContext.Instance.LoginUser.ID;
                    RecPrintinfo.UserName = SystemContext.Instance.LoginUser.Name;
                    NurRecService.Instance.SavePrintLog(RecPrintinfo);
                }

            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintSelected_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理记录!");
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
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, true, 1);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = nStartPageNumber;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("转科情况", m_detpIndex);
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("设置打印起始页码", nStartPageNumber);
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
            if (!RightController.Instance.UserRight.PrintNuringRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理记录!");
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
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, start, end, 1);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = nStartPageNumber;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("转科情况", m_detpIndex);
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("设置打印起始页码", nStartPageNumber);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
                //if (explorerForm.ShowDialog() == DialogResult.OK)
                //    this.SetNursingRecPrint(2);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintContinued_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理记录!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                int start = this.dataTableView1.SelectedRows[0].Index;

                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("转科情况", m_detpIndex);
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("续打行号", start);
                explorerForm.ReportParamData.Add("打印数据", table);
                //explorerForm.ShowDialog();
                if (explorerForm.ShowDialog() == DialogResult.OK)
                    this.SetNursingRecPrint(3);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintReset_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理记录!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("转科情况", m_detpIndex);
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnOption_DropDownOpening(object sender, EventArgs e)
        {
            string key = SystemConst.ConfigKey.NUR_REC_SHOW_AS_MODEL;
            if (SystemContext.Instance.GetConfig(key, false))
                this.toolmnuShowAsModel.Checked = true;
            else
                this.toolmnuShowAsModel.Checked = false;

            key = SystemConst.ConfigKey.NUR_REC_SAVE_RETURN;
            if (SystemContext.Instance.GetConfig(key, false))
                this.toolmnuSaveReturn.Checked = true;
            else
                this.toolmnuSaveReturn.Checked = false;
        }

        private void toolmnuShowAsModel_Click(object sender, EventArgs e)
        {
            this.toolmnuShowAsModel.Checked = !this.toolmnuShowAsModel.Checked;
            string key = SystemConst.ConfigKey.NUR_REC_SHOW_AS_MODEL;
            string value = this.toolmnuShowAsModel.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void toolmnuSaveReturn_Click(object sender, EventArgs e)
        {
            this.toolmnuSaveReturn.Checked = !this.toolmnuSaveReturn.Checked;
            string key = SystemConst.ConfigKey.NUR_REC_SAVE_RETURN;
            string value = this.toolmnuSaveReturn.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void toolmnuNewRecord_Click(object sender, EventArgs e)
        {
            //GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            //if (!RightController.Instance.CanCreateNurRec())
            //{
            //    GlobalMethods.UI.SetCursor(this, Cursors.Default);
            //    return;
            //}
            //this.ShowRecordEditForm(null, null);
            //GlobalMethods.UI.SetCursor(this, Cursors.Default);
            AddNewRecord(null, null);
        }

        private void toolmnuNewSummary_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.CanCreateNurRec())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            GridViewSchema gridViewSchema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            if (this.IsLoadFromSc)
            {
                NurRecSummaryInScForm dialog = new NurRecSummaryInScForm();
                dialog.CurrentSchema = gridViewSchema;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.NursingRecInfo != null)
                        this.LoadGridViewData(dialog.NursingRecInfo.RecordID);
                }
            }
            else
            {
                NurRecSummaryForm dialog = new NurRecSummaryForm();
                dialog.CurrentSchema = gridViewSchema;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.NursingRecInfo != null)
                        this.LoadGridViewData(dialog.NursingRecInfo.RecordID);
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuNewRecord_Click(object sender, EventArgs e)
        {
            this.toolmnuNewRecord.PerformClick();
        }

        private void mnuDeleteRecord_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedRecord();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuInsertSummary_Click(object sender, EventArgs e)
        {
            this.toolmnuNewSummary.PerformClick();
        }

        private void toolcboSchemaList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataTableView1.Focus();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.LoadGridViewColumns(this.toolcboSchemaList.SelectedItem as GridViewSchema))
                this.LoadGridViewData(null);
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
            this.LoadGridViewData(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            //是否归档校验 根据后台配置判断
            if (SystemContext.Instance.SystemOption.DocPigeonhole == true)
            {
                if (IsRecordExamined(this.dataTableView1.Rows[e.RowIndex].Tag as NursingRecInfo))
                {
                    MessageBox.Show("该条护理记录单已审核不可修改");
                    return;
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowRecordEditForm(this.dataTableView1.Rows[e.RowIndex].Tag as NursingRecInfo, null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Value == null)
                return;
            DataGridViewColumn column = this.dataTableView1.Columns[e.ColumnIndex];
            GridViewColumn gridViewColumn = column.Tag as GridViewColumn;
            if (gridViewColumn != null && gridViewColumn.ColumnTag == "记录内容")
                return;
            e.Handled = true;
            e.PaintBackground(e.ClipBounds, true);

            DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];

            if (cell.Selected && this.dataTableView1.Focused)
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Focus);

            string cellValue = e.Value.ToString();
            if (e.ColumnIndex != this.colRecordTime.Index)
            {
                if (cellValue == "True" || cellValue == "true")
                    cellValue = "√";
                else if (cellValue == "False" || cellValue == "false")
                    cellValue = "×";
            }
            else
            {
                NursingRecInfo currRec = null;
                DataTableViewRow currRow = this.dataTableView1.Rows[e.RowIndex];
                if (currRow != null)
                    currRec = currRow.Tag as NursingRecInfo;

                NursingRecInfo prevRec = null;
                DataTableViewRow prevRow = this.dataTableView1.Rows[e.RowIndex - 1];
                if (prevRow != null)
                    prevRec = prevRow.Tag as NursingRecInfo;

                if (prevRec != null && currRec != null)
                {
                    if (prevRec.RecordTime.Date == currRec.RecordTime.Date)
                        cellValue = currRec.RecordTime.ToString("HH:mm");
                }
            }

            Font font = cell.Style.Font;
            if (font == null)
                font = this.dataTableView1.Font;
            Rectangle bounds = e.CellBounds;
            bounds.Y += 3;

            Color foreColor = row.Selected ? Color.White : Color.Black;
            if (!row.Selected && row.DefaultCellStyle.ForeColor == Color.Red)
                foreColor = Color.Red;
            SolidBrush brush = new SolidBrush(foreColor);

            StringFormat format = GlobalMethods.Convert.GetStringFormat(column.DefaultCellStyle.Alignment);
            format.FormatFlags = StringFormatFlags.NoWrap;
            format.Trimming = StringTrimming.Character;
            e.Graphics.DrawString(cellValue, font, brush, bounds, format);
            format.Dispose();
            brush.Dispose();
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
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

        /// <summary>
        /// 新增护理记录
        /// </summary>
        /// <param name="nursingRecInfo">关联的护理记录</param>
        /// <param name="docTypeInfo">文档类型信息</param>
        public void AddNewRecord(NursingRecInfo nursingRecInfo, DocTypeInfo docTypeInfo)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.CanCreateNurRec())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.ShowRecordEditForm(null, docTypeInfo);
            this.m_RecordEditForm.SetSelectNode(docTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 判断该条护理记录单是否已通过审核
        /// </summary>
        /// <param name="nursingRecInfo">护理记录信息</param>
        /// <returns>true or false</returns>
        public bool IsRecordExamined(NursingRecInfo nursingRecInfo)
        {
            QCExamineInfo qcExamineInfo = new QCExamineInfo();
            short shRet =
                QCExamineService.Instance.GetQcExamineInfo(nursingRecInfo.RecordID,
                ServerData.ExamineType.RECORDS, nursingRecInfo.PatientID, nursingRecInfo.VisitID, ref qcExamineInfo);
            if (shRet == ServerData.ExecuteResult.OK)
            {
                if (qcExamineInfo.QcExamineStatus == "1")
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private void toolcboSchemaList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.m_RecordEditForm != null)
                this.m_RecordEditForm.CleanNodeTable();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DataTable table =
                GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);

            ReportExplorer reportExplorer1 = new ReportExplorer();
            reportExplorer1.ReportParamData.Add("转科情况", m_detpIndex);
            reportExplorer1.ReportParamData.Add("是否续打", false);
            reportExplorer1.ReportParamData.Add("虚拟打印", table);
            byte[] byteReportData = this.GetReportFileData(null);
            reportExplorer1.OpenDocument(byteReportData);
            int ipagesNo = 0;
            if (!int.TryParse(reportExplorer1.GetReportData("获取打印页数").ToString(), out ipagesNo))
                return;
            RecPrintinfo RecPrintinfo = new RecPrintinfo();
            GridViewSchema schema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            short shRet = NurRecService.Instance.GetPrintLog(
                PatientTable.Instance.ActivePatient.PatientId,
                PatientTable.Instance.ActivePatient.VisitId,
                PatientTable.Instance.ActivePatient.WardCode, schema.SchemaID,
                ref RecPrintinfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    MessageBox.Show("该模板还未打印。");
                    return;
                }
                MessageBox.Show("数据异常，请联系管理员");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("已打印{0}页", RecPrintinfo.PrintPages));
            sb.AppendFormat("未打印{0}页", ipagesNo - RecPrintinfo.PrintPages);

            MessageBox.Show(sb.ToString());
        }

        private void toolbtnPreView_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理记录!");
                return;
            }
            
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);

                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.showPrintButton = false;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("转科情况", m_detpIndex);
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;
            this.dataTableView1.SelectRow(this.dataTableView1.Rows[e.RowIndex]);
        }
    }
}
