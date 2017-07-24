// ***********************************************************
// 护理电子病历系统公用模块之操作是否允许执行的申请窗口.
// Creator:YangMingkun  Date:2012-10-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Dialogs
{
    public partial class ReasonDialog : HerenForm
    {
        private string m_reason = string.Empty;

        public string Reason
        {
            get { return m_reason; }
            set { this.m_reason = value; }
        }

        public ReasonDialog()
        {
            InitializeComponent();
        }

        public ReasonDialog(string strCatalog,string strText)
        {
            InitializeComponent();
            this.Text = strCatalog;
            this.txtReason.Text = strText;
        }

        public ReasonDialog(string strCatalog, string strText,string strbtnOk,string strbtnCancel)
        {
            InitializeComponent();
            this.Text = strCatalog;
            this.txtReason.Text = strText;
            this.btnOK.Text = strbtnOk;
            this.btnCancel.Text = strbtnCancel;
        } 

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.m_reason = this.txtReason.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}