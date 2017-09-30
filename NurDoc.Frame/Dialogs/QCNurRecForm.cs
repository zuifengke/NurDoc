// ***********************************************************
// 护理电子病历系统,病历指控窗口.
// Creator:YeChongchong  Date:2014-1-5
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.DockSuite;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Frame.DockForms;
using Heren.NurDoc.PatPage.Document;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class QcNurRecForm : HerenForm
    {
        // 存放显示列信息
        Hashtable m_htColumnTable = new Hashtable();

        /// <summary>
        /// 病人信息表 用于前后病人索引
        /// </summary>
        private DataTable m_dtPatVisit = null;

        /// <summary>
        /// 缓存当前索引
        /// </summary>
        private int m_index = 0;

        /// <summary>
        /// 当前显示的病人信息
        /// </summary>
        private PatVisitInfo m_patVisitInfo = null;

        public QcNurRecForm(DataTable dtPatVisit, int Index)
        {
            this.InitializeComponent();
            this.m_dtPatVisit = dtPatVisit;
            this.m_index = Index;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();

            if (this.m_dtPatVisit.Columns.Count != 2 || !this.m_dtPatVisit.Columns.Contains("PatientId") || !this.m_dtPatVisit.Columns.Contains("VisitId"))
            {
                MessageBoxEx.Show("护理记录审核数据传递错误");
                this.DialogResult = DialogResult.Cancel;
            }
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(this.m_dtPatVisit.Rows[this.m_index]);
            this.LoadGridColumns();
            this.RefreshView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Size size = new Size(SystemInformation.WorkingArea.Width, SystemInformation.WorkingArea.Height);
            GlobalMethods.UI.LocateScreenCenter(this, size);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshView()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadPatientInfo();
            this.LoadGridViewData(null);
            this.LoadExamineStatus();
            this.SelectedByStatus();
            if (this.dataTableView1.Rows.Count > 0)
            {
                this.dataTableView1.Rows[0].Selected = true;
            }
            this.BtnHandler();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示病人信息
        /// </summary>
        private void LoadPatientInfo()
        {
            this.toollblPatientID.Text = this.m_patVisitInfo.PatientId;
            this.toollblPatientName.Text = this.m_patVisitInfo.PatientName;
        }

        /// <summary>
        /// 根据病人信息
        /// </summary>
        /// <param name="drPatientInfo">Dr病人信息</param>
        /// <returns>病人就诊信息</returns>
        private PatVisitInfo GetPatVisitFromDataRow(DataRow drPatientInfo)
        {
            PatVisitInfo patVisitInfo = new PatVisitInfo();
            short shRet = PatVisitService.Instance.GetPatVisitInfo(drPatientInfo["PatientId"].ToString(), drPatientInfo["VisitId"].ToString(), ref patVisitInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("病人信息查询失败");
                return null;
            }
            return patVisitInfo;
        }

        /// <summary>
        /// 加载护理记录列列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadGridColumns()
        {
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

            //List<GridViewColumn> lstGlobalGvColumn = new List<GridViewColumn>();
            //将所有的列显示方案合并
            foreach (GridViewSchema schema in lstGridViewSchemas)
            {
                if (schema == null)
                {
                    continue;
                }
                List<GridViewColumn> lstGridViewColumns = new List<GridViewColumn>();
                if (!this.GetGridViewColumns(schema, ref lstGridViewColumns))
                {
                    continue;
                }

                foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
                {
                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                    column.Tag = gridViewColumn;
                    if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnTag))
                        column.Name = gridViewColumn.ColumnTag;
                    else
                        column.Name = gridViewColumn.ColumnName;
                    if (!this.m_htColumnTable.Contains(column.Name))
                    {
                        this.m_htColumnTable.Add(column.Name, column);
                        this.dataTableView1.Columns.Add(column);

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
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定方案下的护理记录列列表
        /// </summary>
        /// <param name="schema">列显示方案</param>
        /// <returns>是否加载成功</returns>
        private bool GetGridViewColumns(GridViewSchema schema, ref List<GridViewColumn> lstGridViewColumns)
        {
            //获取列显示方案中的列列表
            if (schema == null)
                return false;
            string szSchemaID = schema.SchemaID;
            short shRet = ConfigService.Instance.GetGridViewColumns(szSchemaID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("列显示方案的列列表下载失败!");
                return false;
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

            if (this.m_patVisitInfo == null)
                return false;
            string szPatientID = this.m_patVisitInfo.PatientId;
            string szVisitID = this.m_patVisitInfo.VisitId;
            string szSubID = this.m_patVisitInfo.SubID;
            DateTime dtBeginTime = this.m_patVisitInfo.VisitTime;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.GetEndDate(true));
            List<NursingRecInfo> lstNursingRecInfos = null;
            short shRet = NurRecService.Instance.GetNursingRecList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ref lstNursingRecInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录列表下载失败!");
                return false;
            }

            Hashtable htNursingRec = new Hashtable();
            foreach (NursingRecInfo nursingRecInfo in lstNursingRecInfos)
            {
                int rowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[rowIndex];
                row.Tag = nursingRecInfo;
                row.Cells[this.colRecordTime.Index].Value = nursingRecInfo.RecordTime.ToString("yyyy-MM-dd HH:mm");
                row.Cells[this.Status.Index].Value = ServerData.ExamineStatus.QC_NONE;
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
        /// 获取并绑定审核状态信息
        /// </summary>
        private void LoadExamineStatus()
        {
            if (this.m_patVisitInfo == null)
            {
                return;
            }
            List<QCExamineInfo> lstQCExamineInfo = new List<QCExamineInfo>();
            short shRet = QCExamineService.Instance.GetQcExamineInfos(ServerData.ExamineType.RECORDS,
                this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref lstQCExamineInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return;
            }

            for (int RowIndex = 0; RowIndex < this.dataTableView1.Rows.Count; RowIndex++)
            {
                NursingRecInfo nursingRecInfo = this.dataTableView1.Rows[RowIndex].Tag as NursingRecInfo;
                string szStatusText = this.Matching(nursingRecInfo, lstQCExamineInfo);
                this.dataTableView1.Rows[RowIndex].Cells[this.Status.Index].Value = szStatusText;
            }
        }

        /// <summary>
        /// 比较对应的护理记录信息是否已存在于质控审核信息中
        /// </summary>
        /// <param name="nursingRecInfo">护理记录信息</param>
        /// <param name="lstQcExamineInfo">质控护理记录信息list</param>
        /// <returns>护理记录的对应质控状态</returns>
        private string Matching(NursingRecInfo nursingRecInfo, List<QCExamineInfo> lstQcExamineInfo)
        {
            if (lstQcExamineInfo == null || lstQcExamineInfo.Count <= 0)
            {
                return ServerData.ExamineStatus.QC_NONE;
            }
            foreach (QCExamineInfo qcExamineInfo in lstQcExamineInfo)
            {
                if (nursingRecInfo.RecordID == qcExamineInfo.QcContentKey)
                {
                    if (qcExamineInfo.QcExamineStatus == "1")
                    {
                        return ServerData.ExamineStatus.QC_OK;
                    }
                    return ServerData.ExamineStatus.QC_MARK;
                }
            }
            return ServerData.ExamineStatus.QC_NONE;
        }

        /// <summary>
        /// 获取当前病人住院截止的日期
        /// </summary>
        /// <param name="bStopToCurrentTime">是否截止到当前时间</param>
        /// <returns>截止周日期</returns>
        private DateTime GetEndDate(bool bStopToCurrentTime)
        {
            if (this.m_patVisitInfo == null)
                return SysTimeService.Instance.Now;
            DateTime dtWeekDate = this.m_patVisitInfo.DischargeTime;
            if (dtWeekDate != this.m_patVisitInfo.DefaultTime)
                return dtWeekDate;
            return bStopToCurrentTime ? SysTimeService.Instance.Now : DateTime.MaxValue;
        }

        /// <summary>
        /// 根据审核状态筛选数据
        /// </summary>
        private void SelectedByStatus()
        {
            if (this.toolcboStatus.SelectedIndex <= 0)
            {
                return;
            }
            else
            {
                for (int Index = 0; Index < this.dataTableView1.Rows.Count; Index++)
                {
                    if (this.dataTableView1.Rows[Index].Cells[this.Status.Index].Value.ToString() != this.toolcboStatus.Text)
                    {
                        this.dataTableView1.Rows.RemoveAt(Index--);
                    }
                }
            }
        }

        /// <summary>
        /// 根据选择行控制相关按钮
        /// </summary>
        private void BtnHandler()
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                this.toolbtnQc.Enabled = false;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = false;
                return;
            }
            if (this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString() == ServerData.ExamineStatus.QC_NONE)
            {
                this.toolbtnQc.Enabled = true;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = true;
            }
            else if (this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString() == ServerData.ExamineStatus.QC_OK)
            {
                this.toolbtnQc.Enabled = false;
                this.toolbtnQcCancel.Enabled = true;
                this.toolbtnQcQuestion.Enabled = false;
            }
            else
            {
                this.toolbtnQc.Enabled = true;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = true;
            }
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
            if (nursingRecInfo.RecordRemarks != null)
            {
                string[] array = nursingRecInfo.RecordRemarks.Split('$');
                if (array != null && array.Length > 0)
                    szEnterTotal = array[0];
                if (array != null && array.Length > 1)
                    szOutTotal = array[1];
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

        private void toolbtnPrev_Click(object sender, EventArgs e)
        {
            if (this.m_index <= 0)
            {
                MessageBox.Show("已经是第一位病人");
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(this.m_dtPatVisit.Rows[--this.m_index]);
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNext_Click(object sender, EventArgs e)
        {
            if (this.m_index >= this.m_dtPatVisit.Rows.Count)
            {
                MessageBox.Show("已经是最后一位病人");
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(this.m_dtPatVisit.Rows[++this.m_index]);
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnQc_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                return;
            }
            string szRowStatus = this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString();
            NursingRecInfo rowNursingRecInfo = this.dataTableView1.CurrentRow.Tag as NursingRecInfo;
            string key = rowNursingRecInfo.RecordID;
            if (szRowStatus == ServerData.ExamineStatus.QC_NONE)
            {
                QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, key, ServerData.ExamineType.RECORDS);
                qcExamineInfo.QcExamineStatus = "1";
                qcExamineInfo.QcExamineContent = string.Empty;
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核失败！");
                    return;
                }
            }
            else if (szRowStatus == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.RECORDS, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }
                qcExamineInfo.QcExamineStatus = "1";
                qcExamineInfo.QcExamineContent = string.Empty;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核失败！");
                    return;
                }
            }
            this.RefreshView();
        }

        private void toolbtnQcCancel_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                return;
            }
            string szRowStatus = this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString();
            NursingRecInfo rowNursingRecInfo = this.dataTableView1.CurrentRow.Tag as NursingRecInfo;
            string key = rowNursingRecInfo.RecordID;
            if (szRowStatus == ServerData.ExamineStatus.QC_OK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.RECORDS, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("理由", qcExamineInfo.QcExamineContent);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("取消审核失败！");
                    return;
                }
            }
            this.RefreshView();
        }

        private void toolbtnQcQuestion_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                return;
            }
            string szRowStatus = this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString();
            NursingRecInfo rowNursingRecInfo = this.dataTableView1.CurrentRow.Tag as NursingRecInfo;
            string key = rowNursingRecInfo.RecordID;
            if (szRowStatus == ServerData.ExamineStatus.QC_NONE)
            {
                ReasonDialog reasonDialog = new ReasonDialog("理由", string.Empty);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, key, ServerData.ExamineType.RECORDS);
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("标记审核内容失败！");
                    return;
                }
            }
            else if (szRowStatus == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.RECORDS, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("理由", qcExamineInfo.QcExamineContent);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("标记审核内容失败！");
                    return;
                }
            }
            this.RefreshView();
        }

        private void toolcboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.BtnHandler();
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                Point mousePosition = Control.MousePosition;
                this.contextMenuStrip1.Show(mousePosition.X, mousePosition.Y);
            }
        }

        private void btnQcAll_Click(object sender, EventArgs e)
        {
            DialogResult dRValue = MessageBox.Show("是否审核所有未审核和标记中的护理记录单", "审核", MessageBoxButtons.YesNo);
            if (dRValue != DialogResult.Yes)
            {
                return;
            }
            int ErrorCount = 0;
            int NeedQcCount = this.dataTableView1.Rows.Count;
            foreach (DataGridViewRow dgvRow in this.dataTableView1.Rows)
            {
                string szRowStatus = dgvRow.Cells[this.Status.Index].Value.ToString();
                NursingRecInfo rowNursingRecInfo = dgvRow.Tag as NursingRecInfo;
                string key = rowNursingRecInfo.RecordID;
                if (szRowStatus == ServerData.ExamineStatus.QC_NONE)
                {
                    QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, key, ServerData.ExamineType.RECORDS);
                    qcExamineInfo.QcExamineStatus = "1";
                    qcExamineInfo.QcExamineContent = string.Empty;
                    short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        ErrorCount++;
                    }
                }
                else if (szRowStatus == ServerData.ExamineStatus.QC_MARK)
                {
                    QCExamineInfo qcExamineInfo = new QCExamineInfo();
                    short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.RECORDS, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        ErrorCount++;
                        continue;
                    }
                    qcExamineInfo.QcExamineStatus = "1";
                    qcExamineInfo.QcExamineContent = string.Empty;
                    qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                    qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                    qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                    shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        ErrorCount++;
                    }
                }
                else
                {
                    NeedQcCount--;
                }
            }
            StringBuilder sbError = new StringBuilder();
            sbError.AppendFormat("需审核护理记录单：{0}", NeedQcCount);
            sbError.AppendLine();
            sbError.AppendFormat("失败：{0}", ErrorCount);
            MessageBox.Show(sbError.ToString());

            this.RefreshView();
        }
    }
}