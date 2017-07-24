using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Import
{
    public partial class AntenatalInfoForm : HerenForm
    {
        private DataTable m_selectedAntenatalList = null;

        /// <summary>
        /// 获取当前用户筛选检查列表
        /// </summary>
        [Browsable(false)]
        [Description("获取当前用户筛选产前列表列表")]
        public DataTable SelectedAntenatalTable
        {
            get { return this.m_selectedAntenatalList; }
        }

        private DataTable m_getAntenatalList = null;

        /// <summary>
        /// 获取从表达中传递过来的data数据
        /// </summary>
        [Browsable(false)]
        [Description("获取从表达中传递过来的data数据")]
        public DataTable GetAntenatalTable
        {
            set { this.m_getAntenatalList = value; }
        }

        private string m_DocTypeID = string.Empty;

        /// <summary>
        /// 获取或设置查询的文档ID号
        /// </summary>
        /// [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置查询的文档ID号")]
        public string DocTypeID
        {
            //get { return this.m_DocTypeID; }
            set { this.m_DocTypeID = value; }
        }

        public AntenatalInfoForm()
        {
            InitializeComponent();
            this.chkAll.Checked = false;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            string m_patientID = string.Empty;
            string m_visitID = string.Empty;
            //如果外部没有设置病人,那么默认
            if (PatientTable.Instance.ActivePatient != null)
            {
                m_patientID = PatientTable.Instance.ActivePatient.PatientID;
                m_visitID = PatientTable.Instance.ActivePatient.VisitID;
            }

            //如果已经加载,那么不再重复加载
            string szOldPatVisit = this.dataTableView1.Tag as string;
            string szCurrPatVisit = string.Concat(m_patientID, "_", m_visitID);
            if (szOldPatVisit == szCurrPatVisit)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadGridViewColumns();
            this.LoadAntenatalList(m_patientID, m_visitID);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void LoadGridViewColumns()
        {
            if (this.m_getAntenatalList.Columns.Count <= 0)
                return;
            for (int index = 0; index < this.m_getAntenatalList.Columns.Count; index++)
            {
                string szName = this.m_getAntenatalList.Columns[index].ColumnName;
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                DataGridViewCheckBoxColumn column2 = new DataGridViewCheckBoxColumn();

                if (szName == "胎心监护")
                {
                    if (GlobalMethods.Misc.IsEmptyString(szName))
                        continue;
                    else
                        column2.Name = szName;
                    column2.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                    column2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column2.Width = 40;
                    this.dataTableView1.Columns.Add(column2);
                    continue;
                }
                if (GlobalMethods.Misc.IsEmptyString(szName))
                    continue;
                else
                    column.Name = szName;
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (index == 0)
                {
                    column.Width = 140;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                this.dataTableView1.Columns.Add(column);
            }
        }

        private void LoadAntenatalList(string m_patientID, string m_visitID)
        {
            string szPatient = string.Empty;
            string szVisit = string.Empty;
            if (!GlobalMethods.Misc.IsEmptyString(m_patientID))
                szPatient = m_patientID;
            if (!GlobalMethods.Misc.IsEmptyString(m_visitID))
                szVisit = m_visitID;
            string szDocID = this.m_DocTypeID;
            this.dataTableView1.Tag = string.Concat(m_patientID, "_", m_visitID);

            List<SummaryData> lstSummaryData = null;
            short shRet = DocumentService.Instance.GetSummaryData(szDocID, false, ref lstSummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("下载产前关键数据失败！");
                return;
            }
            if (lstSummaryData == null || lstSummaryData.Count <= 0)
                return;
            Dictionary<String, List<SummaryData>> hasTable = new Dictionary<String, List<SummaryData>>();
            List<SummaryData> disListSummary = new List<SummaryData>();
            for (int index = 0; index < lstSummaryData.Count; index++)
            {
                SummaryData summaryInfo = lstSummaryData[index];

                if (disListSummary == null || disListSummary.Count <= 0)
                    disListSummary = new List<SummaryData>();

                if (summaryInfo == null)
                    continue;

                if (hasTable == null)
                    hasTable = new Dictionary<String, List<SummaryData>>();

                if (!hasTable.ContainsKey(summaryInfo.DataUnit))
                {
                    disListSummary = new List<SummaryData>();
                    disListSummary.Add(summaryInfo);
                    hasTable.Add(summaryInfo.DataUnit, disListSummary);
                }
                else
                {
                    disListSummary = hasTable[summaryInfo.DataUnit];
                    disListSummary.Add(summaryInfo);
                }
            }
            if (hasTable == null || hasTable.Count <= 0)
                return;

            List<SummaryData>[] arrListSummary = new List<SummaryData>[hasTable.Values.Count];
            hasTable.Values.CopyTo(arrListSummary, 0);

            this.dataTableView1.Rows.Clear();
            for (int nIndex = 0; nIndex < arrListSummary.Length; nIndex++)
            {
                List<SummaryData> lastSummary = new List<SummaryData>();
                lastSummary = arrListSummary[nIndex];

                int row = this.dataTableView1.Rows.Add();
                DataGridViewRow rowData = this.dataTableView1.Rows[row];
                for (int indexRow = 0; indexRow < lastSummary.Count; indexRow++)
                {
                    SummaryData summary = lastSummary[indexRow];
                    for (int col = 0; col < this.dataTableView1.Columns.Count; col++)
                    {
                        DataGridViewColumn column = this.dataTableView1.Columns[col];
                        for (int nCount = 0; nCount < arrListSummary.Length; nCount++)
                        {
                            string compareName = column.HeaderText + nCount.ToString();
                            if (summary.DataName == compareName)
                            {
                                rowData.Cells[col].Value = summary.DataValue;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            for (int row = 0; row < this.dataTableView1.Rows.Count; row++)
            {
                this.dataTableView1.Rows[row].Cells[this.Column1.Index].Value = this.chkAll.Checked;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ImportAntenatalData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void ImportAntenatalData()
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            this.m_selectedAntenatalList = new DataTable();
            this.m_selectedAntenatalList = this.m_getAntenatalList.Clone();
            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                DataGridViewRow row = this.dataTableView1.Rows[index];
                if (row == null)
                    continue;
                object cellValue = row.Cells[this.Column1.Index].Value;
                if (cellValue != null && cellValue.ToString() == "True")
                {
                    object[] arrData = new object[row.Cells.Count - 1];
                    for (int nCount = 0; nCount < arrData.Length; nCount++)
                        arrData[nCount] = row.Cells[nCount + 1].Value;
                    this.m_selectedAntenatalList.Rows.Add(arrData);
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.Column1.Index)
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (row == null)
                    return;
                bool checkedValue = false;
                object cellValue = row.Cells[this.Column1.Index].Value;
                if (cellValue != null)
                    checkedValue = GlobalMethods.Convert.StringToValue(cellValue.ToString(), false);
                row.Cells[this.Column1.Index].Value = !checkedValue;
            }
        }

        private void dataTableView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.Column1.Index)
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (row == null)
                    return;
                bool checkedValue = false;
                object cellValue = row.Cells[this.Column1.Index].Value;
                if (cellValue != null)
                    checkedValue = GlobalMethods.Convert.StringToValue(cellValue.ToString(), false);
                row.Cells[this.Column1.Index].Value = !checkedValue;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}