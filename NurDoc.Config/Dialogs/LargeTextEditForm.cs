// ***********************************************************
// ��̨���ù�����������ֵ���ı��༭����.
// Author : YangMingkun, Date : 2012-3-25
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Controls;

namespace Heren.NurDoc.Config.Dialogs
{
    internal partial class LargeTextEditForm : HerenForm
    {
        private string m_szLargeText = string.Empty;

        /// <summary>
        /// ��ȡ�����õ�ǰ��Ҫ�༭�ĳ��ı�
        /// </summary>
        public string LargeText
        {
            get { return this.m_szLargeText; }
            set { this.m_szLargeText = value; }
        }

        private List<string> m_lstMacroList = null;

        /// <summary>
        /// ��ȡ�����õ�ǰ�������ĺ��б�
        /// </summary>
        public List<string> MacroList
        {
            get
            {
                return this.m_lstMacroList;
            }

            set
            {
                if (value == null)
                    this.m_lstMacroList.Clear();
                else
                    this.m_lstMacroList = value;
            }
        }

        private string m_szDescription = string.Empty;

        /// <summary>
        /// ��ȡ�����õ�ǰ�Ի���������Ϣ
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get { return this.m_szDescription; }
            set { this.m_szDescription = value; }
        }

        public LargeTextEditForm()
        {
            this.InitializeComponent();
            this.m_lstMacroList = new List<string>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.m_lstMacroList.Count <= 0)
                this.splitContainer1.Panel1Collapsed = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            if (string.IsNullOrEmpty(this.m_szDescription))
                this.lblTextContent.Text = this.Text;
            else
                this.lblTextContent.Text = this.m_szDescription;

            this.txtLargeText.Font = new Font("����", 9f, FontStyle.Regular);
            this.txtLargeText.Text = this.m_szLargeText;
            if (this.m_lstMacroList != null && this.m_lstMacroList.Count > 0)
                this.lbxMacroList.Items.AddRange(this.m_lstMacroList.ToArray());
            this.txtLargeText.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.m_szLargeText = this.txtLargeText.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void lbxMacroList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.lbxMacroList.SelectedIndex < 0)
                return;
            if (this.lbxMacroList.SelectedItem == null)
                return;
            this.txtLargeText.SelectedText = this.lbxMacroList.SelectedItem.ToString();
            this.txtLargeText.Focus();
        }
    }
}