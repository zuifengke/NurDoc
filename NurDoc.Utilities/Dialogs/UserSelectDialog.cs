// ***********************************************************
// ������Ӳ���ϵͳ����ģ��֮�����û�ѡ�񴰿�.
// Creator:YangMingkun  Date:2013-5-30
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
    public partial class UserSelectDialog : HerenForm
    {
        private bool m_bMultiSelect = true;

        /// <summary>
        /// ��ȡ�������Ƿ������ѡ
        /// </summary>
        [Browsable(false)]
        public bool MultiSelect
        {
            get { return this.m_bMultiSelect; }
            set { this.m_bMultiSelect = value; }
        }

        private UserInfo[] m_userInfos = null;

        /// <summary>
        /// ��ȡ��������Ҫ��ѡ�Ŀ�����Ϣ�б�
        /// </summary>
        [Browsable(false)]
        public UserInfo[] UserInfos
        {
            get { return this.m_userInfos; }
            set { this.m_userInfos = value; }
        }

        public UserSelectDialog()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!this.m_bMultiSelect)
            {
                this.listView1.CheckBoxes = false;
                this.listView1.MultiSelect = false;
                this.chkCheckAll.Visible = false;
            }
            this.LoadDeptList();
            this.LoadUserList();
            this.listView1.Focus();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ������Ҫ��ʾ�Ŀ���������Ŀ
        /// </summary>
        private void LoadDeptList()
        {
            if (this.cboDeptList == null || this.cboDeptList.IsDisposed)
                return;
            this.cboDeptList.Items.Clear();
            this.cboUserFind.Items.Clear();

            List<DeptInfo> lstDeptInfos = null;
            short shRet = CommonService.Instance.GetAllDeptInfos(ref lstDeptInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("�ٴ������б�����ʧ��!");
                return;
            }
            if (lstDeptInfos == null || lstDeptInfos.Count <= 0)
                return;

            for (int index = 0; index < lstDeptInfos.Count; index++)
            {
                DeptInfo deptInfo = lstDeptInfos[index];
                if (deptInfo != null && !string.IsNullOrEmpty(deptInfo.DeptCode))
                    this.cboDeptList.Items.Add(deptInfo);
            }
        }

        /// <summary>
        /// ������Ҫ��ʾ���û�������Ŀ
        /// </summary>
        private void LoadUserList()
        {
            if (this.listView1 == null || this.listView1.IsDisposed)
                return;
            this.listView1.Items.Clear();
            this.cboUserFind.Items.Clear();

            List<UserInfo> lstUserInfos = null;
            short shRet = SystemConst.ReturnValue.OK;

            DeptInfo deptInfo = this.cboDeptList.SelectedItem as DeptInfo;
            if (deptInfo != null)
            {
                string szDeptCode = deptInfo.DeptCode;
                shRet = AccountService.Instance.GetDeptUserList(szDeptCode, ref lstUserInfos);
            }
            else
            {
                shRet = AccountService.Instance.GetAllUserInfos(ref lstUserInfos);
            }
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("�û��б�����ʧ��!");
                return;
            }
            if (lstUserInfos == null || lstUserInfos.Count <= 0)
                return;

            List<ListViewItem> items = new List<ListViewItem>();
            List<UserInfo> userInfos = new List<UserInfo>();
            for (int index = 0; index < lstUserInfos.Count; index++)
            {
                UserInfo userInfo = lstUserInfos[index];
                if (userInfo == null || string.IsNullOrEmpty(userInfo.ID))
                    continue;
                ListViewItem item = new ListViewItem(userInfo.Name);
                item.Tag = userInfo;
                item.Checked = this.IsDefaultChecked(userInfo.ID);
                items.Add(item);
                userInfos.Add(userInfo);
            }
            this.listView1.Items.AddRange(items.ToArray());
            this.cboUserFind.Items.AddRange(userInfos.ToArray());
        }

        /// <summary>
        /// ���ָ��ID�Ŷ�Ӧ���û��Ƿ�Ĭ��ѡ��
        /// </summary>
        /// <param name="szUserID">�û�ID��</param>
        /// <returns>�Ƿ�Ĭ��ѡ��</returns>
        private bool IsDefaultChecked(string szUserID)
        {
            if (this.m_userInfos == null || this.m_userInfos.Length <= 0)
                return false;
            foreach (UserInfo userInfo in this.m_userInfos)
            {
                if (userInfo != null && userInfo.ID == szUserID)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// �����û����������û���Ŀ,������ѡ��
        /// </summary>
        /// <param name="szUserName">�û�����</param>
        private void FindUserItem(string szUserName)
        {
            this.listView1.SelectedItems.Clear();
            ListViewItem item = this.listView1.FindItemWithText(szUserName);
            if (item != null)
            {
                item.Focused = true;
                item.Selected = true;
                item.EnsureVisible();
                this.listView1.Focus();
            }
        }

        private void cboDeptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadUserList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void cboDeptFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.FindUserItem(this.cboUserFind.Text);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void cboDeptFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.FindUserItem(this.cboUserFind.Text);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            for (int index = 0; index < this.listView1.Items.Count; index++)
            {
                this.listView1.Items[index].Checked = this.chkCheckAll.Checked;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnOK.PerformClick();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_userInfos = null;
            if (!this.m_bMultiSelect)
            {
                if (this.listView1.SelectedItems.Count > 0)
                {
                    this.m_userInfos = new UserInfo[1];
                    this.m_userInfos[0] = this.listView1.SelectedItems[0].Tag as UserInfo;
                }
            }
            else
            {
                ListView.CheckedListViewItemCollection lstCheckedItems = this.listView1.CheckedItems;
                this.m_userInfos = new UserInfo[lstCheckedItems.Count];
                for (int index = 0; index < lstCheckedItems.Count; index++)
                {
                    this.m_userInfos[index] = lstCheckedItems[index].Tag as UserInfo;
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            this.DialogResult = DialogResult.OK;
        }
    }
}