using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.InfoLib
{
    public partial class InfoLibRenameForm : Form
    {
        private InfoLibInfo m_InfoLibInfo = null;

        public InfoLibInfo InfoLibInfo
        {
            get
            {
                return this.m_InfoLibInfo;
            }

            set
            {
                this.m_InfoLibInfo = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.txtInfoName.Text = this.m_InfoLibInfo.InfoName;
        }

        public InfoLibRenameForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.m_InfoLibInfo.InfoName = this.txtInfoName.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}