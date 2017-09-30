// ***********************************************************
// �����ʿ�ϵͳ�������Ի���.
// Creator:LiChunYing  Date:2011-09-13
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class SelectQuestionForm : HerenForm
    {
        private QCMessageTemplet m_qcMessageTemplet = null;
        /// <summary>
        /// ��ȡ�������ʼ���Ϣģ����
        /// </summary>
        public QCMessageTemplet QCMessageTemplet
        {
            get
            {
                return this.m_qcMessageTemplet;
            }
            set
            {
                this.m_qcMessageTemplet = value;
            }
        }


        private string m_szDocTitle;
        /// <summary>
        ///��ȡ�����ò�������
        /// </summary>
        public string DocTitle
        {
            get
            {
                return this.m_szDocTitle;
            }
            set
            {
                this.m_szDocTitle = value;
            }
        }

        private DateTime m_dtQCCheckTime = DateTime.Now;
        /// <summary>
        /// ��ȡ�������ʼ�ʱ��
        /// </summary>
        public DateTime QCCheckTime
        {
            get
            {
                return this.m_dtQCCheckTime;
            }
            set
            {
                this.m_dtQCCheckTime = value;
            }
        }

        private string m_szQCAskDateTime = string.Empty;
        /// <summary>
        /// ��ȡ������ҽ��ȷ��ʱ��
        /// </summary>
        public string QCAskDateTime
        {
            get
            {
                return this.m_szQCAskDateTime;
            }
            set
            {
                this.m_szQCAskDateTime = value;
            }
        }

        private string m_szDoctorComment = string.Empty;
        /// <summary>
        /// ��ȡ������ҽ������
        /// </summary>
        public string DoctorComment
        {
            get
            {
                return this.m_szDoctorComment;
            }
            set
            {
                this.m_szDoctorComment = value;
            }
        }

        public SelectQuestionForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.txtChecker.Text = SystemContext.Instance.LoginUser.ID;
            this.txtCheckDate.Text = this.QCCheckTime.ToString("yyyy-M-d HH:mm");
            this.txtQuestionType.Text = "<˫��ѡ��>";
            if (PatientTable.Instance.ActivePatient == null)
            {
                MessageBoxEx.Show("��û��ѡ��һλ���ˣ�");
                this.Close();
                return;
            }

            this.txtPatientID.Text = PatientTable.Instance.ActivePatient.PatientId;
            this.txtPatName.Text = PatientTable.Instance.ActivePatient.PatientName;
            this.txtPatSex.Text = PatientTable.Instance.ActivePatient.PatientSex;
            this.txtDocTitle.Text = this.DocTitle;
            this.txtAskDateTime.Text = this.QCAskDateTime;
            this.txtDoctorComment.Text = this.DoctorComment;
            if (this.QCMessageTemplet != null)
            {
                this.txtQuestionType.Text = this.QCMessageTemplet.QCEventType;
                this.txtMessage.Text = this.QCMessageTemplet.Message;
                this.txtMessage.Focus();
                this.txtMessage.SelectAll();
                this.txtMessage.Tag = this.QCMessageTemplet.QCMsgCode;
            }
        }



        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtQuestionType.Text) || this.txtQuestionType.Text == "<˫��ѡ��>")
            {
                MessageBoxEx.Show("��˫��ѡ����������!");
                return;
            }
            if (string.IsNullOrEmpty(this.txtMessage.Text))
            {
                MessageBoxEx.Show("�������ʼ���Ϣ����!");
                this.txtMessage.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtDocTitle.Text))
            {
                MessageBoxEx.Show("��������������!");
                this.txtDocTitle.Focus();
                return;
            }
            if (this.QCMessageTemplet == null)
                this.QCMessageTemplet = new QCMessageTemplet();
            this.QCMessageTemplet.QCEventType = this.txtQuestionType.Text;
            this.QCMessageTemplet.QCMsgCode = (string)this.txtMessage.Tag;
            this.QCMessageTemplet.Message = this.txtMessage.Text;
            this.QCMessageTemplet.MessageTitle = this.txtDocTitle.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtQuestionType_DoubleClick(object sender, EventArgs e)
        {
            if (this.txtQuestionType.Text == "<˫��ѡ��>")
                this.txtQuestionType.Clear();
            QuestionTypeForm frmQuestionType = new QuestionTypeForm();
            if (frmQuestionType.ShowDialog() != DialogResult.OK)
                return;
            this.txtQuestionType.Text = frmQuestionType.QuestionType;
            this.txtMessage.Text = string.Concat(this.txtMessage.Text, frmQuestionType.MessageTemplet);
            this.txtMessage.Tag = frmQuestionType.MessageCode;
            this.txtMessage.Focus();
            this.txtMessage.SelectAll();
        }
    }
}