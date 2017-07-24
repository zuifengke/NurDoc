// ***********************************************************
// 护理电子病历系统公用模块之选项多选窗口.
// Creator:YangMingkun  Date:2012-9-22
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Heren.NurDoc.Utilities.Dialogs
{
    public partial class CheckBoxListForm : Form
    {
        private string[] m_ArrCheckItem = null;

        /// <summary>
        /// 多选项目
        /// </summary>
        public String[] ArrCheckItem
        {
            get
            {
                return this.m_ArrCheckItem;
            }

            set
            {
                this.m_ArrCheckItem = value;
            }
        }

        private string m_StrResult = null;

        /// <summary>
        /// 选择结果
        /// </summary>
        public String StrResult
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

        public CheckBoxListForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            if (this.m_ArrCheckItem != null)
            {
                this.listView1.Items.Clear();
                foreach (string item in this.m_ArrCheckItem)
                {
                    if (item == string.Empty)
                        return;
                    if (this.Width < item.Length * 12)
                    {
                        this.Width = item.Length * 12;
                    }
                    ListViewItem lvitem = this.listView1.Items.Add(item);
                    lvitem.Tag = item.Substring(0, 1);
                }
            }
            base.OnShown(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.m_StrResult = string.Empty;
            foreach (ListViewItem item in this.listView1.Items)
            {
                if (item != null && item.Checked)
                    this.m_StrResult += item.Tag;
            }
        }
    }
}