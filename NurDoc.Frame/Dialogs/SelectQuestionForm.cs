// ***********************************************************
// 病案质控系统问题分类对话框.
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
        /// 获取或设置质检信息模板类
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
        ///获取或设置病历标题
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
        /// 获取或设置质检时间
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
        /// 获取或设置医生确认时间
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
        /// 获取或设置医生反馈
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
            this.txtQuestionType.Text = "<双击选择>";
            if (PatientTable.Instance.ActivePatient == null)
            {
                MessageBoxEx.Show("您没有选择一位病人！");
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
            if (string.IsNullOrEmpty(this.txtQuestionType.Text) || this.txtQuestionType.Text == "<双击选择>")
            {
                MessageBoxEx.Show("请双击选择问题类型!");
                return;
            }
            if (string.IsNullOrEmpty(this.txtMessage.Text))
            {
                MessageBoxEx.Show("请输入质检信息描述!");
                this.txtMessage.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtDocTitle.Text))
            {
                MessageBoxEx.Show("请输入问题主题!");
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
            if (this.txtQuestionType.Text == "<双击选择>")
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