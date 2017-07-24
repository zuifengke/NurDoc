// **************************************************************
// 护理电子病历系统公用模块之检查结果数据导入窗口
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
    public partial class ExamImportForm : HerenForm
    {
        private DataTable m_selectedExamList = null;

        /// <summary>
        /// 获取当前用户筛选检查列表
        /// </summary>
        [Browsable(false)]
        [Description("获取当前用户筛选检查列表")]
        public DataTable SelectedExamTable
        {
            get { return this.m_selectedExamList; }
        }

        private string m_patientID = string.Empty;

        /// <summary>
        /// 获取或设置查询的病人ID号
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置查询的病人ID号")]
        public string PatientID
        {
            get { return this.m_patientID; }
            set { this.m_patientID = value; }
        }

        private string m_visitID = string.Empty;

        /// <summary>
        /// 获取或设置查询的就诊ID号
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置查询的就诊ID号")]
        public string VisitID
        {
            get { return this.m_visitID; }
            set { this.m_visitID = value; }
        }

        public ExamImportForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            //如果外部没有设置病人,那么默认
            if (GlobalMethods.Misc.IsEmptyString(this.m_patientID)
                && PatientTable.Instance.ActivePatient != null)
            {
                this.m_patientID = PatientTable.Instance.ActivePatient.PatientID;
            }

            if (GlobalMethods.Misc.IsEmptyString(this.m_visitID)
                && PatientTable.Instance.ActivePatient != null)
            {
                this.m_visitID = PatientTable.Instance.ActivePatient.VisitID;
            }

            //如果已经加载,那么不再重复加载
            string szOldPatVisit = this.treeView1.Tag as string;
            string szCurrPatVisit = string.Concat(this.m_patientID, "_", this.m_visitID);
            if (szOldPatVisit == szCurrPatVisit)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadExamList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuRefreshList_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadExamList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载检查单列表
        /// </summary>
        private void LoadExamList()
        {
            this.txtExamDetails.Text = string.Concat("检查单号："
                 , "   申请医生：", "   报告者：", "   报告时间：");
            this.txtReportDetails.Clear();
            this.treeView1.Nodes.Clear();
            Application.DoEvents();

            string szPatientID = this.m_patientID;
            string szVisitID = this.m_visitID;
            this.treeView1.Tag = string.Concat(this.m_patientID, "_", this.m_visitID);

            if (GlobalMethods.Misc.IsEmptyString(szPatientID)
                || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                return;
            }

            List<ExamInfo> lstExamInfos = null;
            short shRet = PatVisitService.Instance.GetInpExamList(szPatientID, szVisitID, ref lstExamInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("检查单列表加载失败!");
                return;
            }
            if (lstExamInfos == null || lstExamInfos.Count <= 0)
                return;

            for (int index = 0; index < lstExamInfos.Count; index++)
            {
                ExamInfo examInfo = lstExamInfos[index];
                TreeNode examNode = new TreeNode();
                examNode.Tag = examInfo;

                string szExamTimeText = examInfo.RequestTime.ToString("yyyy-MM-dd");
                if (examInfo.Subject == string.Empty)
                    examNode.Text = string.Concat(szExamTimeText, " 无主题");
                else
                    examNode.Text = string.Concat(szExamTimeText, " ", examInfo.Subject);
                if (examInfo.ReportTime != examInfo.DefaultTime)
                    examNode.ForeColor = Color.Blue;
                this.treeView1.Nodes.Add(examNode);
            }
        }

        /// <summary>
        /// 加载检查报告信息
        /// </summary>
        private void LoadExamReportDetial(ExamInfo examInfo)
        {
            if (examInfo == null || GlobalMethods.Misc.IsEmptyString(examInfo.ExamID))
                return;
            this.txtExamDetails.Text = string.Concat("检查单号：", examInfo.ExamID
                , "   申请医生：", examInfo.RequestDoctor
                , "   报告者：", examInfo.ReportDoctor
                , "   报告时间：", examInfo.ReportTime == examInfo.DefaultTime ?
                string.Empty : examInfo.ReportTime.ToString("yyyy-MM-dd HH:mm"));
            this.txtReportDetails.Clear();
            if (examInfo.ReportTime == examInfo.DefaultTime)
                return;

            ExamResultInfo examReportInfo = null;
            short shRet = PatVisitService.Instance.GetExamResultInfo(examInfo.ExamID, ref examReportInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("检查报告下载失败!");
                return;
            }
            if (examReportInfo == null)
                return;

            examInfo.ExamResultInfo = examReportInfo;
            this.txtReportDetails.Tag = examInfo;

            Font boldFont = new Font("SimSun", 10.5f, System.Drawing.FontStyle.Bold);
            Font regularFont = new Font(this.txtReportDetails.Font, System.Drawing.FontStyle.Regular);

            int nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("检查参数：");
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = boldFont;
            this.txtReportDetails.SelectionColor = Color.Black;

            nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("\n");
            this.txtReportDetails.AppendText(examReportInfo.Parameters);
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = regularFont;
            this.txtReportDetails.SelectionColor = Color.Blue;

            nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("\n检查印象：");
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = boldFont;
            this.txtReportDetails.SelectionColor = Color.Black;

            nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("\n");
            this.txtReportDetails.AppendText(examReportInfo.Impression);
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = regularFont;
            this.txtReportDetails.SelectionColor = Color.Blue;

            nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("\n检查所见：");
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = boldFont;
            this.txtReportDetails.SelectionColor = Color.Black;

            nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("\n");
            this.txtReportDetails.AppendText(examReportInfo.Discription);
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = regularFont;
            this.txtReportDetails.SelectionColor = Color.Blue;

            nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("\n建议：");
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = boldFont;
            this.txtReportDetails.SelectionColor = Color.Black;

            nLength = this.txtReportDetails.TextLength;
            this.txtReportDetails.AppendText("\n");
            this.txtReportDetails.AppendText(examReportInfo.Recommendation);
            this.txtReportDetails.SelectionStart = nLength;
            this.txtReportDetails.SelectionLength = this.txtReportDetails.TextLength - nLength;
            this.txtReportDetails.SelectionFont = regularFont;
            this.txtReportDetails.SelectionColor = Color.Blue;

            this.txtReportDetails.SelectionStart = 0;
            this.txtReportDetails.SelectionLength = 0;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode != null)
                this.LoadExamReportDetial(selectedNode.Tag as ExamInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.m_selectedExamList = null;
            if (this.treeView1.SelectedNode == null)
                return;
            ExamInfo examInfo = this.txtReportDetails.Tag as ExamInfo;
            if (examInfo == null || examInfo.ExamResultInfo == null)
                return;
            this.m_selectedExamList = new DataTable();
            this.m_selectedExamList.TableName = "exam";
            this.m_selectedExamList.Columns.Add("exam_id", typeof(string));
            this.m_selectedExamList.Columns.Add("subject", typeof(string));
            this.m_selectedExamList.Columns.Add("request_doctor", typeof(string));
            this.m_selectedExamList.Columns.Add("request_time", typeof(DateTime));
            this.m_selectedExamList.Columns.Add("report_status", typeof(string));
            this.m_selectedExamList.Columns.Add("report_doctor", typeof(string));
            this.m_selectedExamList.Columns.Add("report_time", typeof(DateTime));
            this.m_selectedExamList.Columns.Add("parameters", typeof(string));
            this.m_selectedExamList.Columns.Add("impression", typeof(string));
            this.m_selectedExamList.Columns.Add("discription", typeof(string));
            this.m_selectedExamList.Columns.Add("recommendation", typeof(string));
            this.m_selectedExamList.Columns.Add("selected_text", typeof(string));

            this.m_selectedExamList.Rows.Add(new object[] { 
                examInfo.ExamID, examInfo.Subject, examInfo.RequestDoctor, 
                examInfo.RequestTime, examInfo.ResultStatus, examInfo.ReportDoctor, examInfo.ReportTime, 
                examInfo.ExamResultInfo.Parameters, examInfo.ExamResultInfo.Impression, 
                examInfo.ExamResultInfo.Discription, examInfo.ExamResultInfo.Recommendation, this.txtReportDetails.SelectedText });
            this.DialogResult = DialogResult.OK;
        }
    }
}