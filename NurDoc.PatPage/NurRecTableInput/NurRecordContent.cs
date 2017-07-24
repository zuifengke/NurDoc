using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.NurDoc.Utilities;

namespace Heren.NurDoc.PatPage.NurRecTableInput
{
    public partial class NurRecordContent : Form
    {
        public NurRecordContent()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            this.txtReason.Text = this.m_StrResult;
            base.OnShown(e);
        }

        private string m_StrResult = string.Empty;

        /// <summary>
        /// 文本内容
        /// </summary>
        public string StrResult
        {
            get
            {
                return this.m_StrResult;
            }

            set
            {
                this.m_StrResult = value;
            }
        }

        private void btnTextTemplet_Click(object sender, EventArgs e)
        {
            this.txtReason.SelectedText = UtilitiesHandler.Instance.ShowTextTempletForm();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.m_StrResult = this.txtReason.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}