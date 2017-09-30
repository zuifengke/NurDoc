// **************************************************************
// ������Ӳ���ϵͳ����ģ��֮�������ݵ��봰��
// Creator:YangMingkun  Date:2012-10-16
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
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
    public partial class TestImportForm : HerenForm
    {
        private DataTable m_selectedTestList = null;

        /// <summary>
        /// ��ȡ��ǰ�û�ɸѡ�����б�
        /// </summary>
        [Browsable(false)]
        [Description("��ȡ��ǰ�û�ɸѡ�����б�")]
        public DataTable SelectedTestTable
        {
            get { return this.m_selectedTestList; }
        }

        private string m_patientID = string.Empty;

        /// <summary>
        /// ��ȡ�����ò�ѯ�Ĳ���ID��
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("��ȡ�����ò�ѯ�Ĳ���ID��")]
        public string PatientID
        {
            get { return this.m_patientID; }
            set { this.m_patientID = value; }
        }

        private string m_visitID = string.Empty;

        /// <summary>
        /// ��ȡ�����ò�ѯ�ľ���ID��
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("��ȡ�����ò�ѯ�ľ���ID��")]
        public string VisitID
        {
            get { return this.m_visitID; }
            set { this.m_visitID = value; }
        }

        public TestImportForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            //����ⲿû�����ò���,��ôĬ��
            if (GlobalMethods.Misc.IsEmptyString(this.m_patientID)
                && PatientTable.Instance.ActivePatient != null)
            {
                this.m_patientID = PatientTable.Instance.ActivePatient.PatientId;
            }

            if (GlobalMethods.Misc.IsEmptyString(this.m_visitID)
                && PatientTable.Instance.ActivePatient != null)
            {
                this.m_visitID = PatientTable.Instance.ActivePatient.VisitId;
            }

            //����Ѿ�����,��ô�����ظ�����
            string szOldPatVisit = this.treeView1.Tag as string;
            string szCurrPatVisit = string.Concat(this.m_patientID, "_", this.m_visitID);
            if (szOldPatVisit == szCurrPatVisit)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadTestList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuRefreshList_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadTestList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ���ؼ��鵥�б�
        /// </summary>
        private void LoadTestList()
        {
            this.txtTestDetails.Text = string.Concat("���鵥�ţ�"
                , "   �걾��", "   ����ҽ����", "   �����ߣ�", "   ����ʱ�䣺");
            this.treeView1.Nodes.Clear();
            this.dataTableView1.Rows.Clear();
            this.chkCheckAllAbnormal.Checked = false;
            Application.DoEvents();

            string szPatientID = this.m_patientID;
            string szVisitID = this.m_visitID;
            this.treeView1.Tag = string.Concat(this.m_patientID, "_", this.m_visitID);

            if (GlobalMethods.Misc.IsEmptyString(szPatientID)
                || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                return;
            }

            List<LabTestInfo> lstLabTestInfos = null;
            short shRet = PatVisitService.Instance.GetInpLabTestList(szPatientID, szVisitID, ref lstLabTestInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("���鵥�б����ʧ��!");
                return;
            }
            if (lstLabTestInfos == null || lstLabTestInfos.Count <= 0)
                return;

            for (int index = 0; index < lstLabTestInfos.Count; index++)
            {
                LabTestInfo labTestInfo = lstLabTestInfos[index];
                TreeNode labTestNode = new TreeNode();
                labTestNode.Tag = labTestInfo;

                string szLabTestTimeText = labTestInfo.RequestTime.ToString("yyyy-MM-dd");
                if (labTestInfo.Subject == string.Empty)
                    labTestNode.Text = string.Concat(szLabTestTimeText, " ������");
                else
                    labTestNode.Text = string.Concat(szLabTestTimeText, " ", labTestInfo.Subject);
                if (labTestInfo.ReportTime != labTestInfo.DefaultTime)
                    labTestNode.ForeColor = Color.Blue;
                this.treeView1.Nodes.Add(labTestNode);
            }
        }

        /// <summary>
        /// ����ָ�����鵥�ļ����������б�
        /// </summary>
        /// <param name="labTestInfo">���鵥��Ϣ</param>
        private void LoadTestResultList(LabTestInfo labTestInfo)
        {
            this.txtTestDetails.Text = string.Concat("���鵥�ţ�", labTestInfo.TestID
                , "   �걾��", labTestInfo.Specimen
                , "   ����ҽ����", labTestInfo.RequestDoctor
                , "   �����ߣ�", labTestInfo.ReportDoctor
                , "   ����ʱ�䣺", (labTestInfo.ReportTime == labTestInfo.DefaultTime) ?
                string.Empty : labTestInfo.ReportTime.ToString("yyyy-MM-dd HH:mm"));
            this.dataTableView1.Rows.Clear();
            this.dataTableView1.Update();
            this.dataTableView1.Tag = labTestInfo;
            this.chkCheckAllAbnormal.Checked = false;

            List<TestResultInfo> lstTestResultInfo = null;
            short shRet = PatVisitService.Instance.GetTestResultList(labTestInfo.TestID, ref lstTestResultInfo);
            if (shRet != SystemConst.ReturnValue.OK && shRet != SystemConst.ReturnValue.CANCEL)
            {
                MessageBoxEx.Show("�������б����ʧ��!");
                return;
            }
            if (lstTestResultInfo == null || lstTestResultInfo.Count <= 0)
                return;

            for (int index = 0; index < lstTestResultInfo.Count; index++)
            {
                TestResultInfo testResultInfo = lstTestResultInfo[index];
                int nRowIndex = this.dataTableView1.Rows.Add();
                DataGridViewRow row = this.dataTableView1.Rows[nRowIndex];
                row.Tag = testResultInfo;

                row.Cells[this.colItemNo.Index].Value = testResultInfo.ItemNo;
                row.Cells[this.colItemName.Index].Value = testResultInfo.ItemName;
                row.Cells[this.colItemResult.Index].Value = testResultInfo.ItemResult;
                row.Cells[this.colAbnormal.Index].Value = testResultInfo.AbnormalIndecator;
                if (testResultInfo.AbnormalIndecator == "��")
                    row.DefaultCellStyle.ForeColor = Color.Red;
                else if (testResultInfo.AbnormalIndecator == "��(Σ��)")
                    row.DefaultCellStyle.ForeColor = Color.Red;
                else if (testResultInfo.AbnormalIndecator == "��")
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                else if (testResultInfo.AbnormalIndecator == "��(Σ��)")
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                else if (testResultInfo.AbnormalIndecator == "Σ��")
                    row.DefaultCellStyle.ForeColor = Color.Orange;
                row.Cells[this.colItemUnits.Index].Value = testResultInfo.ItemUnits;
                row.Cells[this.colNormalValue.Index].Value = testResultInfo.ItemRefer;
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadTestResultList(selectedNode.Tag as LabTestInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.colCheckTest.Index)
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (row == null)
                    return;
                bool checkedValue = false;
                object cellValue = row.Cells[this.colCheckTest.Index].Value;
                if (cellValue != null)
                    checkedValue = GlobalMethods.Convert.StringToValue(cellValue.ToString(), false);
                row.Cells[this.colCheckTest.Index].Value = !checkedValue;
            }
        }

        private void chkCheckAllAbnormal_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                DataGridViewRow row = this.dataTableView1.Rows[index];
                if (row != null)
                {
                    TestResultInfo testResultInfo = row.Tag as TestResultInfo;
                    if (!GlobalMethods.Misc.IsEmptyString(testResultInfo.AbnormalIndecator))
                        row.Cells[this.colCheckTest.Index].Value = this.chkCheckAllAbnormal.Checked;
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.m_selectedTestList = null;
            LabTestInfo labTestInfo = this.dataTableView1.Tag as LabTestInfo;
            if (labTestInfo == null)
                return;
            if (this.dataTableView1.Rows.Count <= 0)
                return;

            DataTable table = new DataTable();
            table.TableName = "test";
            table.Columns.Add("test_id", typeof(string));
            table.Columns.Add("subject", typeof(string));
            table.Columns.Add("request_doctor", typeof(string));
            table.Columns.Add("request_time", typeof(DateTime));
            table.Columns.Add("report_status", typeof(string));
            table.Columns.Add("report_doctor", typeof(string));
            table.Columns.Add("report_time", typeof(DateTime));
            table.Columns.Add("item_no", typeof(int));
            table.Columns.Add("item_name", typeof(string));
            table.Columns.Add("item_result", typeof(string));
            table.Columns.Add("item_units", typeof(string));
            table.Columns.Add("item_refer", typeof(string));
            table.Columns.Add("abnormal_indecator", typeof(string));

            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                DataGridViewRow row = this.dataTableView1.Rows[index];
                if (row == null)
                    continue;
                object cellValue = row.Cells[this.colCheckTest.Index].Value;
                if (cellValue == null || cellValue.ToString() == "False")
                    continue;
                TestResultInfo testResultInfo = row.Tag as TestResultInfo;
                if (testResultInfo == null)
                    continue;
                table.Rows.Add(new object[] { 
                    labTestInfo.TestID, labTestInfo.Subject, labTestInfo.RequestDoctor, labTestInfo.RequestTime, 
                    labTestInfo.ResultStatus, labTestInfo.ReportDoctor, labTestInfo.ReportTime, 
                    testResultInfo.ItemNo, testResultInfo.ItemName, testResultInfo.ItemResult, 
                    testResultInfo.ItemUnits, testResultInfo.ItemRefer, testResultInfo.AbnormalIndecator });
            }
            if (table.Rows.Count > 0)
                this.m_selectedTestList = table;
            this.DialogResult = DialogResult.OK;
        }

        private void dataTableView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}