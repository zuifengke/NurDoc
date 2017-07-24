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
    internal partial class NursingRecHistory : HerenForm
    {
        private PatientPageControl m_patientPageControl = null;
        private Hashtable m_htColumnTable = null;

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

        private GridViewSchema m_currentSchema = null;

        /// <summary>
        /// 获取或设置当前用户选择的护理记录单类型
        /// </summary>
        [Browsable(false)]
        public GridViewSchema CurrentSchema
        {
            get { return this.m_currentSchema; }
            set { this.m_currentSchema = value; }
        }

        public NursingRecHistory()
        {
            this.InitializeComponent();

            string text = SystemContext.Instance.WindowName.NursingRecord;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            this.LoadSchemasList();
            if (this.m_currentSchema != null)
            {
                this.LoadGridViewColumns(this.toolcboSchemaList.SelectedItem as GridViewSchema);
            }
            else
            {
                this.LoadGridViewColumns(this.m_currentSchema);
            }
            this.RefreshView();

            this.toolcboPatDeptList.Items.Add(string.Empty);

            this.toolcboSchemaList.SelectedIndexChanged +=
                new System.EventHandler(this.toolcboSchemaList_SelectedIndexChanged);
            this.toolcboPatDeptList.DropDown +=
                new EventHandler(this.toolcboPatDeptList_DropDown);
            this.toolcboPatDeptList.SelectedIndexChanged +=
                new System.EventHandler(this.toolcboPatDeptList_SelectedIndexChanged);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public void RefreshView()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            this.m_bValueChangedEnabled = false;
            DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1).Date;
            if (PatientTable.Instance.ActivePatient != null)
                dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(SysTimeService.Instance.Now);
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            this.LoadGridViewData(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
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
            string szPatientID = PatientTable.Instance.ActivePatient.PatientID;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitID;
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
                if (schema != null
                    && !GlobalMethods.Misc.IsEmptyString(schema.WardCode))
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
                if (bHasPrivate && GlobalMethods.Misc.IsEmptyString(schema.WardCode))
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
            string szPatientID = PatientTable.Instance.ActivePatient.PatientID;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitID;
            string szSubID = PatientTable.Instance.ActivePatient.SubID;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);
            List<NursingRecInfo> lstNursingRecInfos = null;
            short shRet = NurRecService.Instance.GetNursingRecList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ref lstNursingRecInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录列表下载失败!");
                return false;
            }

            DeptInfo currentDept = this.toolcboPatDeptList.SelectedItem as DeptInfo;

            Hashtable htNursingRec = new Hashtable();
            foreach (NursingRecInfo nursingRecInfo in lstNursingRecInfos)
            {
                if (currentDept != null && nursingRecInfo.WardCode != currentDept.DeptCode)
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
            shRet = DocumentService.Instance.GetSummaryData(szPatientID, szVisitID, ref lstSummaryDatas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理评估摘要数据列表下载失败!");
                return false;
            }
            if (lstSummaryDatas == null)
                lstSummaryDatas = new List<SummaryData>();

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
            }

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
        /// 获取用户当前选择的护理记录单类型
        /// </summary>
        /// <returns>列方案信息</returns>
        private GridViewSchema GetCurrentSchema()
        {
            return this.toolcboSchemaList.SelectedItem as GridViewSchema;
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadGridViewData(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboSchemaList_SelectedIndexChanged(object sender, EventArgs e)
        {
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
    }
}