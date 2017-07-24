// ***********************************************************
// 护理病历配置管理系统,权限管理模块权限点选择对话框.
// Author : YangMingkun, Date : 2012-3-29
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Config.Dialogs
{
    internal partial class RightsSelectForm : HerenForm
    {
        public event RightCheckStateChangedHandler RightCheckStateChanged;

        #region "公共属性定义"
        private List<RightInfo> m_lstDisplayRights = null;

        /// <summary>
        /// 获取或设置需要显示的权限点列表
        /// </summary>
        [Browsable(false)]
        public List<RightInfo> DisplayRights
        {
            get { return this.m_lstDisplayRights; }
            set { this.m_lstDisplayRights = value; }
        }

        private List<RightInfo> m_lstCheckedRights = null;

        /// <summary>
        /// 获取或设置已勾选的权限点列表
        /// </summary>
        [Browsable(false)]
        public List<RightInfo> CheckedRights
        {
            get { return this.m_lstCheckedRights; }
            set { this.m_lstCheckedRights = value; }
        }

        private bool m_bShowAsModel = false;

        /// <summary>
        /// 获取或设置是否显示确定和取消按钮
        /// </summary>
        public bool ShowAsModel
        {
            get { return this.m_bShowAsModel; }
            set { this.m_bShowAsModel = value; }
        }
        #endregion

        private Hashtable m_htRightItem = null;

        private bool m_bItemCheckEventEnabled = true;

        public RightsSelectForm()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.m_bShowAsModel)
            {
                this.btnOK.Visible = false;
                this.btnCancel.Visible = false;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            this.LoadDisplayRightList();
            this.InitRightCheckState();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen pen = new Pen(Color.DodgerBlue);
            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0
                , this.ClientSize.Width - 1, this.ClientSize.Height - 1));
            pen.Dispose();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible) this.Focus();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (!this.m_bShowAsModel)
                this.Visible = false;
        }

        /// <summary>
        /// 加载需要显示的权限点项目
        /// </summary>
        private void LoadDisplayRightList()
        {
            if (this.m_htRightItem == null) this.m_htRightItem = new Hashtable();
            this.m_htRightItem.Clear();

            if (this.m_lstDisplayRights == null)
                return;

            this.m_bItemCheckEventEnabled = false;
            for (int index = 0; index < this.m_lstDisplayRights.Count; index++)
            {
                RightInfo rightInfo = this.m_lstDisplayRights[index];
                if (rightInfo == null)
                    continue;

                ListViewItem item = this.listView1.Items.Add(rightInfo.Name);
                item.Tag = rightInfo;
                if (!this.m_htRightItem.Contains(rightInfo.Name))
                    this.m_htRightItem.Add(rightInfo.Name, item);
            }
            this.m_bItemCheckEventEnabled = true;
        }

        /// <summary>
        /// 初始化权限点勾选状态
        /// </summary>
        private void InitRightCheckState()
        {
            if (this.m_lstCheckedRights == null)
                return;
            if (this.m_lstCheckedRights.Count <= 0)
                return;
            this.m_bItemCheckEventEnabled = false;
            for (int index = 0; index < this.m_lstCheckedRights.Count; index++)
            {
                RightInfo rightInfo = this.m_lstCheckedRights[index];
                if (rightInfo == null || rightInfo.Name == null)
                    continue;
                if (this.m_htRightItem == null)
                    continue;
                if (!this.m_htRightItem.Contains(rightInfo.Name))
                    continue;
                ListViewItem item = this.m_htRightItem[rightInfo.Name] as ListViewItem;
                if (item != null) item.Checked = true;
            }
            this.m_bItemCheckEventEnabled = true;
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (this.RightCheckStateChanged == null || e.Item == null)
                return;
            if (!this.m_bItemCheckEventEnabled)
                return;
            RightCheckStateChangedEventArgs eventArgs =
                new RightCheckStateChangedEventArgs(e.Item.Text, e.Item.Checked);
            this.RightCheckStateChanged(this, eventArgs);
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int index = 0; index < this.listView1.Items.Count; index++)
            {
                this.listView1.Items[index].Checked = this.chkCheckAll.Checked;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.m_lstCheckedRights == null)
                this.m_lstCheckedRights = new List<RightInfo>();
            this.m_lstCheckedRights.Clear();
            for (int index = 0; index < this.listView1.Items.Count; index++)
            {
                ListViewItem item = this.listView1.Items[index];
                RightInfo rightInfo = item.Tag as RightInfo;
                if (rightInfo == null)
                    continue;
                rightInfo.Value = item.Checked;
                this.m_lstCheckedRights.Add(rightInfo);
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public class RightCheckStateChangedEventArgs
        {
            private string m_szRightName = string.Empty;

            public string RightName
            {
                get { return this.m_szRightName; }
            }

            private bool m_bIsChecked = false;

            public bool IsChecked
            {
                get { return this.m_bIsChecked; }
            }

            public RightCheckStateChangedEventArgs(string szRightName, bool bIsChecked)
            {
                this.m_szRightName = szRightName;
                this.m_bIsChecked = bIsChecked;
            }
        }

        public delegate void RightCheckStateChangedHandler(object sender, RightCheckStateChangedEventArgs e);
    }
}