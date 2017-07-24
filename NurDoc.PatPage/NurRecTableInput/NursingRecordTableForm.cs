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

namespace Heren.NurDoc.PatPage.NurRecTableInput
{
    internal partial class NursingRecordTableForm : DockContentBase
    {
        private PatientPageControl m_patientPageControl = null;
        private Hashtable m_htColumnTable = null;

        private int m_deptIndex = 0;

        /// <summary>
        /// 批量录入数据的加载和保存处理器
        /// </summary>
        private NursingRecHandler m_nursingRecHandler = null;

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bCellValueChangedEnabled = true;

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

        public NursingRecordTableForm(PatientPageControl patientPageControl)
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
            this.HandlerSpecialColReadOnly();
            this.RefreshView();
            this.RiseRowHeight();
            this.dataTableView1.Refresh();

            //设置界面打印信息的显示
            this.SetGridViewPrint();
            this.toolcboPatDeptList.Items.Add(string.Empty);
            this.toolcboSchemaList.SelectedIndexChanged +=
             new System.EventHandler(this.toolcboSchemaList_SelectedIndexChanged);
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
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            this.m_bCellValueChangedEnabled = false;
            this.LoadGridViewData(null);
            this.m_bCellValueChangedEnabled = true;

            //初始化列表的时候,定位到最后一行
            this.dataTableView1.SelectRow(this.dataTableView1.Rows.Count - 1);

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void RiseRowHeight()
        {
            this.dataTableView1.Columns[colRecordTime.Index].Width = this.dataTableView1.Columns[colRecordTime.Index].Width + 1;
            this.dataTableView1.Columns[colRecordTime.Index].Width = this.dataTableView1.Columns[colRecordTime.Index].Width - 1;
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
        /// 查询当前病人所经过的病区hash表
        /// </summary>
        /// <returns>是否查询成功</returns>
        private bool LoadPatientDeptList()
        {
            this.Update();
            if (SystemContext.Instance.LoginUser == null)
                return false;
            string szPatientID = PatientTable.Instance.ActivePatient.PatientID;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitID;
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
            this.dataTableView1.Groups.Clear();
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
            DataTableViewGroup preDataTableViewGroup = null;
            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                DataGridViewColumn column = null;
                if (gridViewColumn.ColumnType != ServerData.DataType.CHECKBOX && !GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnItems))
                {
                    column = new ComboBoxColumn();
                    string[] items = gridViewColumn.ColumnItems.Split(';');
                    ((ComboBoxColumn)column).Items.AddRange(items);
                    //((ComboBoxEditingControl)column).Items.AddRange(items);
                    if (items != null && items.Length > 0 && string.IsNullOrEmpty(items[0]))
                        ((ComboBoxColumn)column).DisplayStyle = ComboBoxStyle.DropDown;//.DropDownList;95项目客户要求修改为可手动编辑
                }
                else
                {
                    column = new DataGridViewTextBoxColumn();
                }
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
                //配置列组名,实现多表头
                if (gridViewColumn.ColumnGroup != string.Empty)
                {
                    if (preDataTableViewGroup == null
                        || preDataTableViewGroup.Text != gridViewColumn.ColumnGroup)
                    {
                        int i = this.dataTableView1.Groups.Add();
                        preDataTableViewGroup = this.dataTableView1.Groups[i];
                        preDataTableViewGroup.Text = gridViewColumn.ColumnGroup;
                        preDataTableViewGroup.BeginColumn = gridViewColumn.ColumnIndex + 1;
                        preDataTableViewGroup.EndColumn = gridViewColumn.ColumnIndex + 1;
                    }
                    else if (preDataTableViewGroup.Text == gridViewColumn.ColumnGroup)
                    {
                        preDataTableViewGroup.EndColumn = gridViewColumn.ColumnIndex + 1;
                    }
                }
            }
            this.m_nursingRecHandler = new NursingRecHandler(this.dataTableView1, this.toolcboSchemaList.SelectedItem as GridViewSchema, this.m_htColumnTable);
            return true;
        }

        /// <summary>
        /// 加载护理记录数据列表
        /// </summary>
        /// <param name="szDefaultSelectedRecordID">默认选中的记录</param>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewData(string szDefaultSelectedRecordID)
        {
            if (this.toolcboSchemaList.SelectedItem == null)
                return false;
            GridViewSchema schema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            if (schema == null)
                return false;
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

            TransferInfo currentDept = this.toolcboPatDeptList.SelectedItem as TransferInfo;
            m_deptIndex = this.toolcboPatDeptList.SelectedIndex;

            Hashtable htNursingRec = new Hashtable();
            this.dataTableView1.SuspendLayout();
            foreach (NursingRecInfo nursingRecInfo in lstNursingRecInfos)
            {
                if (currentDept != null && nursingRecInfo.WardCode != currentDept.WardCode)
                    continue;
                if (nursingRecInfo.SchemaID != schema.SchemaID)
                    continue;
                int rowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[rowIndex];
                row.Tag = nursingRecInfo;
                if (!nursingRecInfo.RecordID.Contains("$"))
                {
                    row.Cells[this.colRecordTime.Index].Value = nursingRecInfo.RecordTime.ToString("yyyy-MM-dd HH:mm");
                }
                this.dataTableView1.SetRowState(row, RowState.Normal);

                if (nursingRecInfo.RecordID.Contains("$"))
                {
                    row.Cells[colRecordTime.Index].ReadOnly = true;
                }

                //特殊处理创建人列
                DataGridViewColumn column = this.m_htColumnTable["创建人"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                {
                    row.Cells[column.Index].Value = nursingRecInfo.CreatorName;
                    row.Cells[column.Index].ReadOnly = true;
                }

                //特殊处理修改人列
                column = this.m_htColumnTable["修改人"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                {
                    row.Cells[column.Index].Value = nursingRecInfo.ModifierName;
                    row.Cells[column.Index].ReadOnly = true;
                }

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
                if (nursingRecInfo.SummaryFlag == 1)
                    row.ReadOnly = true;
            }
            this.dataTableView1.ResumeLayout();

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
            this.CountEnterOutData();
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

        private void CountEnterOutData()
        {
            DataGridViewColumn enterColumn = this.m_htColumnTable["入量量"] as DataGridViewColumn;
            DataGridViewColumn outColumn = this.m_htColumnTable["出量量"] as DataGridViewColumn;
            DataGridViewColumn currentEnterColumn = this.m_htColumnTable["入量累计"] as DataGridViewColumn;
            DataGridViewColumn currentOutColumn = this.m_htColumnTable["出量累计"] as DataGridViewColumn;
            float ftotalEnter = 0, ftotalOut = 0, num = -1;
            DateTime preDate = DateTime.MinValue.AddHours(7).AddSeconds(1);//由于在计算时间差时需要减去7个小时，故设置初始值时需加上7小时
            NursingRecInfo recInfo = null;

            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                recInfo = this.dataTableView1.Rows[index].Tag as NursingRecInfo;
                if (recInfo == null || recInfo.SummaryFlag == 1)
                    continue;
                if (!IsTimeInDay(preDate, recInfo.RecordTime))
                {
                    ftotalEnter = 0;
                    ftotalOut = 0;
                }
                num = -1;
                if (enterColumn != null && currentEnterColumn != null)
                {
                    if (!GlobalMethods.Misc.IsEmptyString(this.dataTableView1.Rows[index].Cells[enterColumn.Index].Value))
                    {
                        if (float.TryParse(this.dataTableView1.Rows[index].Cells[enterColumn.Index].Value.ToString(), out num))
                        {
                            ftotalEnter += num;
                            this.dataTableView1.Rows[index].Cells[currentEnterColumn.Index].Value = ftotalEnter;
                        }
                        else
                            this.dataTableView1.Rows[index].Cells[currentEnterColumn.Index].Value = string.Empty;
                    }
                    else
                    {
                        this.dataTableView1.Rows[index].Cells[currentEnterColumn.Index].Value = string.Empty;
                    }
                }
                num = -1;
                if (outColumn != null && currentOutColumn != null)
                {
                    if (!GlobalMethods.Misc.IsEmptyString(this.dataTableView1.Rows[index].Cells[outColumn.Index].Value))
                    {
                        if (float.TryParse(this.dataTableView1.Rows[index].Cells[outColumn.Index].Value.ToString(), out num))
                        {
                            ftotalOut += num;
                            this.dataTableView1.Rows[index].Cells[currentOutColumn.Index].Value = ftotalOut;
                        }
                        else
                            this.dataTableView1.Rows[index].Cells[currentOutColumn.Index].Value = string.Empty;
                    }
                    else
                    {
                        this.dataTableView1.Rows[index].Cells[currentOutColumn.Index].Value = string.Empty;
                    }
                }
                preDate = recInfo.RecordTime;
            }
        }

        /// <summary>
        /// 判断两个时间点 是否在同一天(7:00:01-next7.00)
        /// </summary>
        /// <param name="preDate">上一个时间点</param>
        /// <param name="currentDate">当前时间点</param>
        /// <returns>true or false</returns>
        private bool IsTimeInDay(DateTime preDate, DateTime currentDate)
        {
            if (preDate.AddHours(-7).AddSeconds(-1).Date == currentDate.AddHours(-7).AddSeconds(-1).Date)
            {
                return true;
            }
            return false;
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

            column = this.m_htColumnTable["入量累计"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = szEnterTotal;
            column = this.m_htColumnTable["出量累计"] as DataGridViewColumn;
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
            //添加小结是录入的累计时间随记录的删除而修改

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
        /// 删除当前选中的护理记录 同时更新文档状态为已作废 并删除体征表和摘要表
        /// </summary>
        /// <returns>是否删除成功</returns>
        private bool DeleteSelectedRecord()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return false;
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return false;
            DataTableViewRow selectRow = this.dataTableView1.SelectedRows[0];
            NursingRecInfo recInfo = selectRow.Tag as NursingRecInfo;
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
            int selectRowIndex = this.dataTableView1.SelectedRows[0].Index;
            for (int index = this.dataTableView1.Rows.Count - 1; index >= selectRowIndex; index--)
            {
                NursingRecInfo nursingRecInfo = this.dataTableView1.Rows[index].Tag as NursingRecInfo;
                DataTableViewRow nurRecRow = this.dataTableView1.Rows[index];
                if (!nursingRecInfo.RecordID.Contains(recInfo.RecordID))
                    continue;
                short shRet = NurRecService.Instance.DeleteNursingRec(nursingRecInfo.RecordID, szModifierID, szModifierName);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("护理记录删除失败!");
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return false;
                }
                //根据记录id获取文档列表，再更新文档状态
                shRet = NurRecService.Instance.DeleteSummaryData(nursingRecInfo.RecordID);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("护理记录摘要数据删除失败!");
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return false;
                }
                this.dataTableView1.Rows.Remove(nurRecRow);
                //nurRecRow.Visible = false;
            }

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
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
            shRet = NurRecService.Instance.GetNursingRec(docInfo.RecordID,docInfo.PatientID,docInfo.VisitID, ref nursingRecInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的护理记录!", docInfo.RecordID));
                return true;
            }

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
            return reportExplorerForm;
        }
        #endregion

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bCellValueChangedEnabled = false;
            this.LoadGridViewData(null);
            this.m_bCellValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintAll_Click(object sender, EventArgs e)
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
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, false, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ReportParamData.Add("转科情况", m_deptIndex);
                if (explorerForm.ShowDialog() == DialogResult.OK)
                    this.SetNursingRecPrint(0);
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
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, false, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = nStartPageNumber;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("设置打印起始页码", nStartPageNumber);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ReportParamData.Add("转科情况", m_deptIndex);
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
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, start, end, false, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = nStartPageNumber;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("设置打印起始页码", nStartPageNumber);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ReportParamData.Add("转科情况", m_deptIndex);
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
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, false, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("续打行号", start);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ReportParamData.Add("转科情况", m_deptIndex);
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
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, false, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ReportParamData.Add("转科情况", m_deptIndex);
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
        }

        private void toolmnuShowAsModel_Click(object sender, EventArgs e)
        {
            this.toolmnuShowAsModel.Checked = !this.toolmnuShowAsModel.Checked;
            string key = SystemConst.ConfigKey.NUR_REC_SHOW_AS_MODEL;
            string value = this.toolmnuShowAsModel.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void mnuNewRecord_Click(object sender, EventArgs e)
        {
            if (this.toolcboSchemaList.SelectedItem == null)
            {
                MessageBoxEx.Show("没有选择相应的护理记录单类型");
                return;
            }
            GridViewSchema schema = this.toolcboSchemaList.SelectedItem as GridViewSchema;

            DateTime dtRecordTime = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            NursingRecInfo nursingRecInfo = this.m_nursingRecHandler.CreateNursingRecord(PatientTable.Instance.ActivePatient, dtRecordTime);
            nursingRecInfo.SchemaID = schema.SchemaID;
            int count = this.dataTableView1.Rows.Count;
            this.dataTableView1.Rows.Insert(count, 1);
            DataTableViewRow row = this.dataTableView1.Rows[count];
            this.dataTableView1.SelectRow(row);
            row.State = RowState.New;
            row.Cells[this.colRecordTime.Index].Value = dtRecordTime;
            row.Tag = nursingRecInfo;

            //特殊处理签名列
            DataGridViewColumn column = this.m_htColumnTable["创建人"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
            {
                row.Cells[column.Index].Value = nursingRecInfo.CreatorName;
            }

            //特殊处理签名列
            column = this.m_htColumnTable["修改人"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
            {
                row.Cells[column.Index].Value = nursingRecInfo.ModifierName;
            }
        }

        private void mnuDeleteRecord_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedRecord();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboSchemaList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.LoadGridViewColumns(this.toolcboSchemaList.SelectedItem as GridViewSchema))
            {
                this.HandlerSpecialColReadOnly();
                this.m_bCellValueChangedEnabled = false;
                this.LoadGridViewData(null);
                this.m_bCellValueChangedEnabled = true;
                this.RiseRowHeight();
                this.dataTableView1.Refresh();
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
            this.m_bCellValueChangedEnabled = false;
            this.LoadGridViewData(null);
            this.m_bCellValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex <= 0 || e.RowIndex < 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            GridViewColumn gridViewColumn = (GridViewColumn)this.dataTableView1.Columns[e.ColumnIndex].Tag;
            if (gridViewColumn.ColumnType == ServerData.DataType.CHECKBOX)
            {
                CheckBoxListForm chkForm = new CheckBoxListForm();
                chkForm.Text = gridViewColumn.ColumnName;
                chkForm.ArrCheckItem = gridViewColumn.ColumnItems.Split(';');
                if (chkForm.ShowDialog() == DialogResult.OK)
                {
                    this.dataTableView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = chkForm.StrResult;
                    this.dataTableView1.EndEdit();
                }
            }
            if (gridViewColumn.ColumnTag == "记录内容")
            {
                NurRecordContent frmNurRecordContent = new NurRecordContent();

                frmNurRecordContent.StrResult = this.dataTableView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? string.Empty : this.dataTableView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (frmNurRecordContent.ShowDialog() == DialogResult.OK)
                {
                    this.dataTableView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = frmNurRecordContent.StrResult;
                    this.dataTableView1.EndEdit();
                }
            }

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
            format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;//.NoWrap;
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

        private void toolmnuNewSummary_Click(object sender, EventArgs e)
        {
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            this.m_nursingRecHandler.SaveGridViewData();
            this.m_bCellValueChangedEnabled = false;
            this.LoadGridViewData(null);
            this.m_bCellValueChangedEnabled = true;
        }

        private void toolmnuNewRecord_Click(object sender, EventArgs e)
        {
        }

        private void toolbtnSubOrder_Click(object sender, EventArgs e)
        {
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow == null)
                return;
            int nRowIndex = this.dataTableView1.Rows.Count;
            if (this.dataTableView1.CurrentRow != null)
                nRowIndex = this.dataTableView1.CurrentRow.Index + 1;
            this.dataTableView1.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
            NursingRecInfo nursingRecInfo = (currRow.Tag as NursingRecInfo).Clone() as NursingRecInfo;
            nursingRecInfo.RecordID = this.MarkRecordIDSubOrder(currRow);
            row.Tag = nursingRecInfo;
            this.dataTableView1.SetRowState(row, RowState.New);
            row.Cells[this.colRecordTime.Index].ReadOnly = true;

            this.dataTableView1.Focus();
            this.dataTableView1.SelectRow(row);
        }

        /// <summary>
        /// 特殊行只读
        /// </summary>
        private void HandlerSpecialColReadOnly()
        {
            Hashtable m_htReadOnlyTable = new Hashtable();
            m_htReadOnlyTable.Add("创建人", string.Empty);
            m_htReadOnlyTable.Add("修改人", string.Empty);
            m_htReadOnlyTable.Add("入量累计", string.Empty);
            m_htReadOnlyTable.Add("出量累计", string.Empty);
            GridViewColumn column = null;
            for (int index = 1; index < this.dataTableView1.Columns.Count; index++)
            {
                column = this.dataTableView1.Columns[index].Tag as GridViewColumn;
                if (m_htReadOnlyTable.ContainsKey(column.ColumnTag))
                {
                    this.dataTableView1.Columns[index].ReadOnly = true;
                }
            }
        }

        /// <summary>
        /// 产生子医嘱护理记录ID号
        /// </summary>
        /// <returns>string</returns>
        private string MarkRecordIDSubOrder(DataTableViewRow currRow)
        {
            NursingRecInfo currNursingRecInfo = currRow.Tag as NursingRecInfo;
            int subNo = 0;
            string szRecordID = string.Empty;
            szRecordID = currNursingRecInfo.RecordID.Split('$')[0];
            if (currNursingRecInfo.RecordID.Contains("$"))
                subNo = int.Parse(currNursingRecInfo.RecordID.Split('$')[1]);
            for (int index = currRow.Index; index < this.dataTableView1.Rows.Count; index++)
            {
                NursingRecInfo nursingRecInfo = this.dataTableView1.Rows[index].Tag as NursingRecInfo;
                if (nursingRecInfo == null)
                    continue;
                if (!nursingRecInfo.RecordID.Contains(currNursingRecInfo.RecordID.Split('$')[0]))
                    break;
                if (nursingRecInfo.RecordID.Contains("$"))
                {
                    subNo = subNo < int.Parse(nursingRecInfo.RecordID.Split('$')[1])
                        ? int.Parse(nursingRecInfo.RecordID.Split('$')[1]) : subNo;
                }
            }
            return string.Format("{0}${1}", szRecordID, (subNo + 1).ToString());
        }

        private void toolbtnCopy_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.CurrentRow == null)
                return;
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;

            DateTime dtRecordTime = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            int nRowIndex = this.GetInsertRowIndex(dtRecordTime);

            this.dataTableView1.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
            this.SetRowData(row, currRow, dtRecordTime);

            this.dataTableView1.Focus();
            this.dataTableView1.SelectRow(row);
            this.dataTableView1.SetRowState(row, RowState.New);
        }

        /// <summary>
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="currRow">绑定的数据</param>
        private void SetRowData(DataTableViewRow row, DataTableViewRow currRow, DateTime dtRecordTime)
        {
            if (row == null || row.Index < 0 || currRow == null)
                return;
            NursingRecInfo nursingRecInfo = this.m_nursingRecHandler.CreateNursingRecord(PatientTable.Instance.ActivePatient, dtRecordTime);
            if (nursingRecInfo == null)
                return;
            GridViewSchema gridViewSchema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            nursingRecInfo.SchemaID = gridViewSchema.SchemaID;
            row.Cells[colRecordTime.Index].Value = nursingRecInfo.RecordTime;
            row.Tag = nursingRecInfo;
            //复制记录项到新的行
            for (int i = 1; i < currRow.Cells.Count; i++)
            {
                row.Cells[i].Value = currRow.Cells[i].Value;
            }
        }

        /// <summary>
        /// 获取需要插入的行号
        /// </summary>
        /// <param name="dtInsertTime">时间</param>
        /// <returns>行号</returns>
        private int GetInsertRowIndex(DateTime dtInsertTime)
        {
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                if (row.Cells[0].Value != null && Convert.ToDateTime(row.Cells[0].Value) > dtInsertTime)
                {
                    return row.Index;
                }
            }
            return this.dataTableView1.Rows.Count;
        }

        private void toolbtnDelete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedRecord();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.m_bCellValueChangedEnabled == false)
                return;
            if (e.ColumnIndex == 0)
            {
                DataTableViewRow currRow = this.dataTableView1.Rows[e.RowIndex];
                NursingRecInfo currNursingRecInfo = currRow.Tag as NursingRecInfo;
                if (currRow.Cells[0].Value == null || currNursingRecInfo == null
                    || currRow.Cells[0].Value.ToString() == string.Empty)
                    return;
                currNursingRecInfo.RecordTime = DateTime.Parse(currRow.Cells[0].Value.ToString());
                currNursingRecInfo.RecordDate = DateTime.Parse(currRow.Cells[0].Value.ToString()).Date;
                for (int index = currRow.Index; index < this.dataTableView1.Rows.Count; index++)
                {
                    NursingRecInfo nursingRecInfo = this.dataTableView1.Rows[index].Tag as NursingRecInfo;
                    if (nursingRecInfo == null)
                        continue;
                    //属于子医嘱，更改记录时间
                    if (nursingRecInfo.RecordID.Contains(currNursingRecInfo.RecordID.Split('$')[0]))
                    {
                        nursingRecInfo.RecordTime = currNursingRecInfo.RecordTime;
                        nursingRecInfo.RecordDate = currNursingRecInfo.RecordDate;
                        this.dataTableView1.Rows[index].State = RowState.Update;
                    }
                }
            }
            DataGridViewColumn enterColumn = this.m_htColumnTable["入量量"] as DataGridViewColumn;
            if (enterColumn != null && enterColumn.Index == e.ColumnIndex)
            {
                CountEnterOutData();
            }
            DataGridViewColumn outColumn = this.m_htColumnTable["出量量"] as DataGridViewColumn;
            if (outColumn != null && outColumn.Index == e.ColumnIndex)
            {
                CountEnterOutData();
            }
        }

        private void toolbtnImportOrder_Click(object sender, EventArgs e)
        {
            GridViewSchema schema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            DateTime dtRecordTime = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            DataTableViewRow currRow = this.dataTableView1.SelectedRows[0];

            DataTable result = UtilitiesHandler.Instance.ShowOrdersImportForm();

            if (result == null || result.Rows.Count <= 0)
                return;
            int nRowIndex = 0;
            string szSubNo = string.Empty;
            for (int index = 0; index < result.Rows.Count; index++)
            {
                //休眠50毫秒 防止生成护理记录单id时主键冲突
                System.Threading.Thread.Sleep(50);
                //如果是第一条或者是新的父医嘱，则以新记录方式添加。
                //如果是子医嘱，则以子医嘱记录方式添加。
                if (result.Rows[index]["OrderNo"].ToString() != szSubNo)
                {
                    szSubNo = result.Rows[index]["OrderNo"].ToString();
                    //列表新增一行记录
                    NursingRecInfo nursingRecInfo = this.m_nursingRecHandler.CreateNursingRecord(PatientTable.Instance.ActivePatient, dtRecordTime);
                    nursingRecInfo.SchemaID = schema.SchemaID;
                    nRowIndex = this.dataTableView1.Rows.Add();
                    DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                    row.State = RowState.New;
                    row.Cells[this.colRecordTime.Index].Value = dtRecordTime;
                    row.Tag = nursingRecInfo;
                    //入量名称非ml处理
                    if (SystemContext.Instance.SystemOption.NurRecordUnit == true && (!result.Rows[index]["单位"].ToString().Contains("ml") && !result.Rows[index]["单位"].ToString().Contains("毫升")))
                    {
                        row.Cells["入量名称"].Value = result.Rows[index]["医嘱内容"].ToString() + "(" + result.Rows[index]["剂量"].ToString() + " " + result.Rows[index]["单位"].ToString() + ")";
                        row.Cells["入量量"].Value = string.Empty;
                    }
                    else
                    {
                        row.Cells["入量名称"].Value = result.Rows[index]["医嘱内容"].ToString();
                        row.Cells["入量量"].Value = result.Rows[index]["剂量"].ToString();
                    }
                    row.Cells["入量途径"].Value = SystemContext.Instance.GetOrderWayCode(result.Rows[index]["途径"].ToString());
                    row.State = RowState.New;

                    this.dataTableView1.SelectRow(row);
                }
                else
                {
                    DataTableViewRow preRow = this.dataTableView1.Rows[nRowIndex];
                    NursingRecInfo nursingRecInfo = (preRow.Tag as NursingRecInfo).Clone() as NursingRecInfo;
                    nursingRecInfo.RecordID = this.MarkRecordIDSubOrder(preRow);
                    //新增一行子医嘱
                    nRowIndex = this.dataTableView1.Rows.Add();
                    DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                    row.Tag = nursingRecInfo;
                    //入量名称非ml处理
                    if (SystemContext.Instance.SystemOption.NurRecordUnit == true && (!result.Rows[index]["单位"].ToString().Contains("ml") && !result.Rows[index]["单位"].ToString().Contains("毫升")))
                    {
                        row.Cells["入量名称"].Value = result.Rows[index]["医嘱内容"].ToString() + "(" + result.Rows[index]["剂量"].ToString() + " " + result.Rows[index]["单位"].ToString() + ")";
                        row.Cells["入量量"].Value = string.Empty;
                    }
                    else
                    {
                        row.Cells["入量名称"].Value = result.Rows[index]["医嘱内容"].ToString();
                        row.Cells["入量量"].Value = result.Rows[index]["剂量"].ToString();
                    }
                    //row.Cells["入量途径"].Value = SystemContext.Instance.GetOrderWayCode(result.Rows[index]["途径"].ToString());
                    row.Cells[this.colRecordTime.Index].ReadOnly = true;

                    this.dataTableView1.SetRowState(row, RowState.New);
                    this.dataTableView1.SelectRow(row);
                }
            }
            this.dataTableView1.EndEdit();
            this.RiseRowHeight();
        }

        private void toolbtnStatistics_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            GridViewSchema gridViewSchema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            //如果当前未选中护理小结或者选中的行不是护理小结
            if (this.dataTableView1.SelectedRows.Count <= 0)
            {
                if (this.IsLoadFromSc)
                {
                    Heren.NurDoc.PatPage.NurRec.NurRecSummaryInScForm dialog =
                        new Heren.NurDoc.PatPage.NurRec.NurRecSummaryInScForm();
                    dialog.CurrentSchema = gridViewSchema;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (dialog.NursingRecInfo != null)
                        {
                            this.m_bCellValueChangedEnabled = false;
                            this.LoadGridViewData(dialog.NursingRecInfo.RecordID);
                            this.m_bCellValueChangedEnabled = true;
                        }
                    }
                }
                return;
            }

            NursingRecInfo nursingRecInfo = this.dataTableView1.SelectedRows[0].Tag as NursingRecInfo;
            if (nursingRecInfo != null && nursingRecInfo.SummaryFlag == 1)
            {
                if (this.IsLoadFromSc)
                {
                    Heren.NurDoc.PatPage.NurRec.NurRecSummaryInScForm dialog =
                        new Heren.NurDoc.PatPage.NurRec.NurRecSummaryInScForm();
                    dialog.CurrentSchema = gridViewSchema;
                    if (dialog.ShowDialog(nursingRecInfo) == DialogResult.OK)
                    {
                        if (dialog.NursingRecInfo != null)
                        {
                            this.m_bCellValueChangedEnabled = false;
                            this.LoadGridViewData(nursingRecInfo.RecordID);
                            this.m_bCellValueChangedEnabled = true;
                        }
                    }
                }
            }
            else
            {
                if (this.IsLoadFromSc)
                {
                    Heren.NurDoc.PatPage.NurRec.NurRecSummaryInScForm dialog =
                        new Heren.NurDoc.PatPage.NurRec.NurRecSummaryInScForm();
                    dialog.CurrentSchema = gridViewSchema;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (dialog.NursingRecInfo != null)
                        {
                            this.m_bCellValueChangedEnabled = false;
                            this.LoadGridViewData(dialog.NursingRecInfo.RecordID);
                            this.m_bCellValueChangedEnabled = true;
                        }
                    }
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}
