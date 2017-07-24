using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Controls;

namespace Heren.NurDoc.Utilities.Dialogs
{
    public partial class SetPageNumberForm : HerenForm
    {
        private int m_nStartPageNumber = 1;

        /// <summary>
        /// 获取或设置打印起始页码（或者是记录条数）
        /// </summary>
        public int StartPageNumber
        {
            get { return this.m_nStartPageNumber; }
            set { this.m_nStartPageNumber = value; }
        }

        public SetPageNumberForm()
        {
            InitializeComponent();
        }

        public SetPageNumberForm(string strTitle, string strDescript)
        {
            InitializeComponent();
            this.Text = strTitle;
            this.label1.Text = strDescript + "：";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.StartPageNumber = (int)this.nudStartPageNumber.Value;
            this.DialogResult = DialogResult.OK;
        }
    }
}