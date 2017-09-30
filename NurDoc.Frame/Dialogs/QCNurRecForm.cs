// ***********************************************************
// ������Ӳ���ϵͳ,����ָ�ش���.
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
        // �����ʾ����Ϣ
        Hashtable m_htColumnTable = new Hashtable();

        /// <summary>
        /// ������Ϣ�� ����ǰ��������
        /// </summary>
        private DataTable m_dtPatVisit = null;

        /// <summary>
        /// ���浱ǰ����
        /// </summary>
        private int m_index = 0;

        /// <summary>
        /// ��ǰ��ʾ�Ĳ�����Ϣ
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
                MessageBoxEx.Show("�����¼������ݴ��ݴ���");
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
        /// ˢ������
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
        /// ��ʾ������Ϣ
        /// </summary>
        private void LoadPatientInfo()
        {
            this.toollblPatientID.Text = this.m_patVisitInfo.PatientId;
            this.toollblPatientName.Text = this.m_patVisitInfo.PatientName;
        }

        /// <summary>
        /// ���ݲ�����Ϣ
        /// </summary>
        /// <param name="drPatientInfo">Dr������Ϣ</param>
        /// <returns>���˾�����Ϣ</returns>
        private PatVisitInfo GetPatVisitFromDataRow(DataRow drPatientInfo)
        {
            PatVisitInfo patVisitInfo = new PatVisitInfo();
            short shRet = PatVisitService.Instance.GetPatVisitInfo(drPatientInfo["PatientId"].ToString(), drPatientInfo["VisitId"].ToString(), ref patVisitInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("������Ϣ��ѯʧ��");
                return null;
            }
            return patVisitInfo;
        }

        /// <summary>
        /// ���ػ����¼���б�
        /// </summary>
        /// <returns>�Ƿ���سɹ�</returns>
        private bool LoadGridColumns()
        {
            this.Update();
            if (SystemContext.Instance.LoginUser == null)
                return false;

            //��ȡ����ʾ�����б�
            string szSchemaType = ServerData.GridViewSchema.NURSING_RECORD;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<GridViewSchema> lstGridViewSchemas = null;
            short shRet = ConfigService.Instance.GetGridViewSchemas(szSchemaType, szWardCode, ref lstGridViewSchemas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("����ʾ�����б�����ʧ��!");
                return false;
            }
            if (lstGridViewSchemas == null)
                lstGridViewSchemas = new List<GridViewSchema>();

            //List<GridViewColumn> lstGlobalGvColumn = new List<GridViewColumn>();
            //�����е�����ʾ�����ϲ�
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
        /// ��ȡָ�������µĻ����¼���б�
        /// </summary>
        /// <param name="schema">����ʾ����</param>
        /// <returns>�Ƿ���سɹ�</returns>
        private bool GetGridViewColumns(GridViewSchema schema, ref List<GridViewColumn> lstGridViewColumns)
        {
            //��ȡ����ʾ�����е����б�
            if (schema == null)
                return false;
            string szSchemaID = schema.SchemaID;
            short shRet = ConfigService.Instance.GetGridViewColumns(szSchemaID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("����ʾ���������б�����ʧ��!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ���ػ����¼�����б�
        /// </summary>
        /// <param name="szDefaultSelectedRecordID">Ĭ��ѡ�еļ�¼</param>
        /// <returns>�Ƿ���سɹ�</returns>
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
                MessageBoxEx.ShowError("�����¼�б�����ʧ��!");
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

                //���⴦��ǩ����
                DataGridViewColumn column = this.m_htColumnTable["ǩ����1"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                    row.Cells[column.Index].Value = nursingRecInfo.Recorder1Name;

                //���⴦��ǩ����
                column = this.m_htColumnTable["ǩ����2"] as DataGridViewColumn;
                if (column != null && column.Index >= 0)
                    row.Cells[column.Index].Value = nursingRecInfo.Recorder2Name;

                //���⴦���¼������
                column = this.m_htColumnTable["��¼����"] as DataGridViewColumn;
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
                MessageBoxEx.ShowError("��������ժҪ�����б�����ʧ��!");
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

                //���⴦���/��Ѫѹ
                bool success = this.HandleBloodPressData(row, summaryData);

                //���⴦������/����
                if (!success)
                    this.HandlePulseHeartRateData(row, summaryData);

                //���������е�����(��ʹsuccessΪtrue,ҲҪִ��)
                DataGridViewColumn column = this.m_htColumnTable[summaryData.DataName] as DataGridViewColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                    row.Cells[column.Index].Value = summaryData.DataValue;
            }

            this.OptimizeDataView();

            //���ݵ�Ԫ�����ݵ����и߶�
            int nMinHeight = this.dataTableView1.RowTemplate.Height;
            this.dataTableView1.AdjustRowHeight(0, this.dataTableView1.Rows.Count, nMinHeight, (nMinHeight * 20) + 4);

            //���������Զ���������ǰ��
            DataTableViewRow currentRow = this.dataTableView1.CurrentRow;
            this.dataTableView1.CurrentCell = null;
            this.dataTableView1.SelectRow(currentRow);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// ��ȡ�������״̬��Ϣ
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
        /// �Ƚ϶�Ӧ�Ļ����¼��Ϣ�Ƿ��Ѵ������ʿ������Ϣ��
        /// </summary>
        /// <param name="nursingRecInfo">�����¼��Ϣ</param>
        /// <param name="lstQcExamineInfo">�ʿػ����¼��Ϣlist</param>
        /// <returns>�����¼�Ķ�Ӧ�ʿ�״̬</returns>
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
        /// ��ȡ��ǰ����סԺ��ֹ������
        /// </summary>
        /// <param name="bStopToCurrentTime">�Ƿ��ֹ����ǰʱ��</param>
        /// <returns>��ֹ������</returns>
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
        /// �������״̬ɸѡ����
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
        /// ����ѡ���п�����ذ�ť
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
        /// �������¼С�������е���ʾ
        /// </summary>
        /// <param name="row">��ǰ��</param>
        /// <param name="nursingRecInfo">�����¼��Ϣ</param>
        /// <returns>�Ƿ���ɹ�</returns>
        private bool HandleNursingRecordSummary(DataTableViewRow row, NursingRecInfo nursingRecInfo)
        {
            if (row == null || nursingRecInfo == null || nursingRecInfo.SummaryFlag != 1)
                return false;

            row.DefaultCellStyle.ForeColor = Color.Red;

            string szSummaryName = null;
            if (nursingRecInfo.SummaryName != null)
                szSummaryName = nursingRecInfo.SummaryName;
            DataGridViewColumn column = null;

            //��С�����Ʒŵ�����������
            column = this.m_htColumnTable["��������"] as DataGridViewColumn;
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

            column = this.m_htColumnTable["����"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = szEnterTotal;
            column = this.m_htColumnTable["����"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = szOutTotal;

            //���⴦���¼������
            column = this.m_htColumnTable["��¼����"] as DataGridViewColumn;
            if (column != null && column.Index >= 0)
                row.Cells[column.Index].Value = nursingRecInfo.RecordContent;
            return true;
        }

        /// <summary>
        /// ���⴦��ǰ�����¼���ߵ�Ѫѹ��ʾ����,����������Ҫ��ʾ��һ����Ԫ��
        /// </summary>
        /// <param name="row">��ǰ��</param>
        /// <param name="summaryData">ժҪ����</param>
        /// <returns>�Ƿ���ɹ�</returns>
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
                if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("Ѫѹhigh" + strFlag)
                    && this.m_htColumnTable.Contains("Ѫѹ"))
                {
                    DataGridViewTextBoxColumn column = this.m_htColumnTable["Ѫѹ"] as DataGridViewTextBoxColumn;
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
                else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("ѪѹLow" + strFlag)
                    && this.m_htColumnTable.Contains("Ѫѹ"))
                {
                    DataGridViewTextBoxColumn column = this.m_htColumnTable["Ѫѹ"] as DataGridViewTextBoxColumn;
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
        /// ���⴦��ǰ�����¼������/������ʾ����,����������Ҫ��ʾ��һ����Ԫ��
        /// </summary>
        /// <param name="row">��ǰ��</param>
        /// <param name="summaryData">ժҪ����</param>
        /// <returns>�Ƿ���ɹ�</returns>
        private bool HandlePulseHeartRateData(DataGridViewRow row, SummaryData summaryData)
        {
            if (row == null || summaryData == null)
                return false;

            if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("����")
                && this.m_htColumnTable.Contains("����/����"))
            {
                DataGridViewTextBoxColumn column = this.m_htColumnTable["����/����"] as DataGridViewTextBoxColumn;
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
            else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("����")
                && this.m_htColumnTable.Contains("����/����"))
            {
                DataGridViewTextBoxColumn column = this.m_htColumnTable["����/����"] as DataGridViewTextBoxColumn;
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
        /// �Ż��������
        /// </summary>
        private void OptimizeDataView()
        {
            if (this.dataTableView1.Rows.Count <= 0
                && this.m_htColumnTable == null && this.m_htColumnTable.Count <= 0)
                return;
            DataGridViewTextBoxColumn column1 = this.m_htColumnTable["Ѫѹ"] as DataGridViewTextBoxColumn;
            DataGridViewTextBoxColumn column2 = this.m_htColumnTable["����/����"] as DataGridViewTextBoxColumn;
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
                MessageBox.Show("�Ѿ��ǵ�һλ����");
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
                MessageBox.Show("�Ѿ������һλ����");
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
                    MessageBoxEx.Show("���ʧ�ܣ�");
                    return;
                }
            }
            else if (szRowStatus == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.RECORDS, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("������ݲ�ѯʧ�ܣ�");
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
                    MessageBoxEx.Show("���ʧ�ܣ�");
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
                    MessageBoxEx.Show("������ݲ�ѯʧ�ܣ�");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("����", qcExamineInfo.QcExamineContent);
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
                    MessageBoxEx.Show("ȡ�����ʧ�ܣ�");
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
                ReasonDialog reasonDialog = new ReasonDialog("����", string.Empty);
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
                    MessageBoxEx.Show("����������ʧ�ܣ�");
                    return;
                }
            }
            else if (szRowStatus == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.RECORDS, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("������ݲ�ѯʧ�ܣ�");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("����", qcExamineInfo.QcExamineContent);
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
                    MessageBoxEx.Show("����������ʧ�ܣ�");
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
            DialogResult dRValue = MessageBox.Show("�Ƿ��������δ��˺ͱ���еĻ����¼��", "���", MessageBoxButtons.YesNo);
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
            sbError.AppendFormat("����˻����¼����{0}", NeedQcCount);
            sbError.AppendLine();
            sbError.AppendFormat("ʧ�ܣ�{0}", ErrorCount);
            MessageBox.Show(sbError.ToString());

            this.RefreshView();
        }
    }
}