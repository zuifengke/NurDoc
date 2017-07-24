using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Heren.NurDoc.Frame.Dialogs;
using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using System.Collections;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class QuestionListForm : DockContentBase
    {
        Hashtable m_htMsgDict = null;
        public QuestionListForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.AutoHidePortion = this.Width;
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.DockBottom;
            this.DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                | DockAreas.DockTop | DockAreas.DockBottom;
            LoadQCMsgDict();

        }

        public override void RefreshView()
        {
            base.RefreshView();
            BindQuestionList();
        }
        private void LoadQCMsgDict()
        {
            if (this.m_htMsgDict == null)
                this.m_htMsgDict = new Hashtable();
            List<QCMessageTemplet> lstQCMessageTemplet = null;
            short shRet = CommonService.Instance.GetQCMessageTempletList(string.Empty, ref lstQCMessageTemplet);
            if (shRet != ServerData.ExecuteResult.OK || lstQCMessageTemplet == null)
                return;
            for (int index = 0; index < lstQCMessageTemplet.Count; index++)
            {
                QCMessageTemplet messageTemplet = lstQCMessageTemplet[index];
                if (!m_htMsgDict.ContainsKey(messageTemplet.QCMsgCode))
                    this.m_htMsgDict.Add(messageTemplet.QCMsgCode, messageTemplet);
            }
        }
        private void BindQuestionList()
        {
            if (dtView.Rows.Count > 0)
                dtView.Rows.Clear();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            List<QCQuestionInfo> lstQCQuestionInfos = null;
            string szPatientID = PatientTable.Instance.ActivePatient.PatientID;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitID;
            short shRet = QcQuestionService.Instance.GetQCQuestionList(szPatientID, szVisitID, null, ref lstQCQuestionInfos);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("获取质检信息失败！");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (lstQCQuestionInfos == null || lstQCQuestionInfos.Count == 0)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            for (int index = 0; index < lstQCQuestionInfos.Count; index++)
            {
                QCQuestionInfo qcQuestionInfo = lstQCQuestionInfos[index];
                if (qcQuestionInfo == null)
                    continue;
                int nRowIndex = this.dtView.Rows.Add();
                DataGridViewRow row = this.dtView.Rows[nRowIndex];
                this.SetRowData(row, qcQuestionInfo);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }


        /// <summary>
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="qcQuestionInfo">绑定的数据</param>
        /// <returns>bool</returns>
        private bool SetRowData(DataGridViewRow row, QCQuestionInfo qcQuestionInfo)
        {
            if (row == null || row.Index < 0 || qcQuestionInfo == null)
                return false;
            row.Tag = qcQuestionInfo;
            row.Cells[this.colQCEventType.Index].Value = qcQuestionInfo.QuestionType;
            if (qcQuestionInfo.CheckTime != qcQuestionInfo.DefaultTime)
                row.Cells[this.colCheckTime.Index].Value = qcQuestionInfo.CheckTime.ToString("yyyy-M-d HH:mm");
            if (qcQuestionInfo.ConfirmTime != qcQuestionInfo.DefaultTime)
                row.Cells[this.colConfirmTime.Index].Value = qcQuestionInfo.ConfirmTime.ToString("yyyy-M-d HH:mm");
            row.Cells[this.colDocTitle.Index].Value = qcQuestionInfo.Topic;
            row.Cells[this.colFeedBackInfo.Index].Value = qcQuestionInfo.DoctorComment;
            row.Cells[this.colDetailInfo.Index].Value = qcQuestionInfo.QuestionContent;
            if (this.m_htMsgDict.Contains(qcQuestionInfo.QuestionCode))
            {
                QCMessageTemplet qcMessage = this.m_htMsgDict[qcQuestionInfo.QuestionCode] as QCMessageTemplet;
                if (qcMessage == null)
                    return false;
             }
            if (qcQuestionInfo.QuestionStatus == "0") //0为未接收、未确认状态
            {
                row.DefaultCellStyle.ForeColor = Color.Red;
                row.Cells[this.colQuestionStatus.Index].Value = "未接收";
            }
            else if (qcQuestionInfo.QuestionStatus == "1")//1为已接收、已确认状态
            {
                row.DefaultCellStyle.ForeColor = Color.Green;
                row.Cells[this.colQuestionStatus.Index].Value = "已接收";
            }
            else if (qcQuestionInfo.QuestionStatus == "2")//2为已修改状态
            {
                row.DefaultCellStyle.ForeColor = Color.Blue;
                row.Cells[this.colQuestionStatus.Index].Value = "已修改";
            }
            else if (qcQuestionInfo.QuestionStatus == "3")//3为合格状态
            {
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.Cells[this.colQuestionStatus.Index].Value = "合格";
            }
            return true;
        }


        private void toolBtnAdd_Click(object sender, EventArgs e)
        {
            if (PatientTable.Instance.ActivePatient == null)
            {
                MessageBoxEx.Show("请选择一位病人！");
                return;
            }
            string szTopic = GetActiveDocumentTttle();
           
            SelectQuestionForm selectQuestion = new SelectQuestionForm();
            selectQuestion.DocTitle = szTopic;
            DialogResult result = selectQuestion.ShowDialog();
            if (result != DialogResult.OK)
                return;
            QCQuestionInfo qcQuestionInfo = new QCQuestionInfo(true);
            DateTime now = DateTime.Now;
            CommonService.Instance.GetServerTime(ref now);
            qcQuestionInfo.PatientID = PatientTable.Instance.ActivePatient.PatientID;
            qcQuestionInfo.VisitID = PatientTable.Instance.ActivePatient.VisitID;
            qcQuestionInfo.DeptCode = PatientTable.Instance.ActivePatient.WardCode;
            qcQuestionInfo.QuestionType = selectQuestion.QCMessageTemplet.QCEventType;
            qcQuestionInfo.QuestionCode = selectQuestion.QCMessageTemplet.QCMsgCode;
            qcQuestionInfo.QuestionContent = selectQuestion.QCMessageTemplet.Message;
            qcQuestionInfo.CheckerID = SystemContext.Instance.LoginUser.ID;
            qcQuestionInfo.CheckerName = SystemContext.Instance.LoginUser.Name;
            qcQuestionInfo.CheckTime = now;
            qcQuestionInfo.QuestionStatus = "0";
            // qcQuestionInfo.TopicID = "护理文书ID";
            // qcQuestionInfo.Topic = "护理文书主题";
            qcQuestionInfo.Topic = selectQuestion.QCMessageTemplet.MessageTitle;
            short shRet = QcQuestionService.Instance.SaveQuestionInfo(qcQuestionInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("保存质检信息失败！");
                return;
            }

            //创建行
            int index = this.dtView.Rows.Add();
            DataGridViewRow row = this.dtView.Rows[index];
            row.Cells[this.colQuestionStatus.Index].Value = "未接收";
            row.Cells[this.colQCEventType.Index].Value = selectQuestion.QCMessageTemplet.QCEventType;
            row.Cells[this.colDetailInfo.Index].Value = selectQuestion.QCMessageTemplet.Message;
            row.Cells[this.colCheckTime.Index].Value = now.ToString("yyyy MM-dd HH:mm");
            row.Cells[this.colDocTitle.Index].Value = selectQuestion.QCMessageTemplet.MessageTitle;
            row.DefaultCellStyle.ForeColor = Color.Red;
            row.Tag = qcQuestionInfo;
            //保存
        }

        private string GetActiveDocumentTttle()
        {
            PatientPageForm patientPageForm = this.MainFrame.GetPatientPageForm(PatientTable.Instance.ActivePatient);
            if (patientPageForm == null)
                return string.Empty;
            IDockContent content = patientPageForm.ActiveDocument as IDockContent;
            if (content == null)
                return string.Empty;
            Form contentBase = content as Form;
            if (contentBase == null)
                return string.Empty;
            return contentBase.Text;
        }

        private void toolBtnUpdate_Click(object sender, EventArgs e)
        {
            if (this.dtView.SelectedRows.Count <= 0)
                return;

            DataGridViewRow row = this.dtView.SelectedRows[0];
            if (row == null)
                return;
            QCQuestionInfo questionInfo = row.Tag as QCQuestionInfo;
            if (questionInfo == null)
                return;
            string szOldMsgCode = questionInfo.QuestionCode;
            SelectQuestionForm frmQuestion = new SelectQuestionForm();
            QCMessageTemplet messageTemplet = this.m_htMsgDict[questionInfo.QuestionCode] as QCMessageTemplet;
            frmQuestion.QCMessageTemplet = messageTemplet;
            frmQuestion.QCAskDateTime = (string)row.Cells[this.colConfirmTime.Index].Value;
            frmQuestion.DoctorComment = (string)row.Cells[this.colFeedBackInfo.Index].Value;
            frmQuestion.Text = "问题信息查询修改";
            frmQuestion.DocTitle = (string)row.Cells[this.colDocTitle.Index].Value;
            if (row.Cells[this.colCheckTime.Index].Value != null)
                frmQuestion.QCCheckTime = DateTime.Parse(row.Cells[this.colCheckTime.Index].Value.ToString());
            if (frmQuestion.ShowDialog() != DialogResult.OK)
                return;
            DateTime now = DateTime.Now;
            CommonService.Instance.GetServerTime(ref now);
            questionInfo.QuestionType = frmQuestion.QCMessageTemplet.QCEventType;
            questionInfo.QuestionCode = frmQuestion.QCMessageTemplet.QCMsgCode;
            questionInfo.QuestionContent = frmQuestion.QCMessageTemplet.Message;
            questionInfo.Topic = frmQuestion.QCMessageTemplet.MessageTitle;
            short shRet = QcQuestionService.Instance.UpdateQuestion(questionInfo, now, szOldMsgCode);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("更新失败！");
                return;
            }
            BindQuestionList();
        }

        private void toolBtnDelete_Click(object sender, EventArgs e)
        {
            if (this.dtView.SelectedRows.Count <= 0)
                return;

            DataGridViewRow selectedRow = this.dtView.SelectedRows[0];
            if (selectedRow == null)
                return;
            QCQuestionInfo questionInfo = selectedRow.Tag as QCQuestionInfo;
            if (questionInfo == null)
                return;
            DialogResult dResult = MessageBoxEx.ShowConfirm("是否删除这条质检信息？");
            if (dResult != DialogResult.OK)
                return;

            short shRet = QcQuestionService.Instance.DeleteQuestion(questionInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("删除质检信息失败！");
                return;
            }
            else
            {
                MessageBoxEx.Show("删除成功！", MessageBoxIcon.Information);
                this.dtView.Rows.Remove(selectedRow);
            }
        }

        private void toolBtnQCPass_Click(object sender, EventArgs e)
        {
            if (this.dtView.SelectedRows.Count <= 0)
                return;

            DataGridViewRow row = this.dtView.SelectedRows[0];
            if (row == null)
                return;
            QCQuestionInfo questionInfo = row.Tag as QCQuestionInfo;
            if (questionInfo == null)
                return;
            DialogResult dResult = MessageBoxEx.ShowConfirm("是否将这条质检信息标记为合格？");
            if (dResult != DialogResult.OK)
                return;
            if (questionInfo.QuestionStatus == "3")
            {
                MessageBoxEx.Show("这条质检已经是合格状态！", MessageBoxIcon.Information);
                return;
            }
            questionInfo.QuestionStatus = "3";
            short shRet = QcQuestionService.Instance.UpdateQuestion(questionInfo, questionInfo.CheckTime, questionInfo.QuestionCode);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("更新失败！");
                return;
            }
            BindQuestionList();
        }
    }
}