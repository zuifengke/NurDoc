// ***********************************************************
// 后台配置管理中心配置值长文本编辑窗口.
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
        /// 获取或设置当前需要编辑的长文本
        /// </summary>
        public string LargeText
        {
            get { return this.m_szLargeText; }
            set { this.m_szLargeText = value; }
        }

        private List<string> m_lstMacroList = null;

        /// <summary>
        /// 获取或设置当前允许插入的宏列表
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
        /// 获取或设置当前对话框描述信息
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

            this.txtLargeText.Font = new Font("宋体", 9f, FontStyle.Regular);
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