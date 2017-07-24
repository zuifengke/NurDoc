// ***********************************************************
// ���������ù���ϵͳ,�����û������ù�����.
// Author : YangMingkun, Date : 2013-5-30
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.Config.Dialogs;

namespace Heren.NurDoc.Config.DockForms
{
    public partial class UserGroupEditForm : DockContentBase
    {
        public UserGroupEditForm(MainForm mainForm)
            : base(mainForm)
        {
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.UpdateBounds();
            this.btnAddGroup.Image = Properties.Resources.Add;
            this.btnAddUser.Image = Properties.Resources.Add;
        }

        public override void OnRefreshView()
        {
            base.OnRefreshView();
            this.Update();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadGroupGridViewData();

            CommonDictItem commonDictItem = null;
            if (this.dgvGroups.CurrentRow != null)
                commonDictItem = this.dgvGroups.CurrentRow.Tag as CommonDictItem;
            this.LoadUserGridViewData(commonDictItem);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// �Ƿ���δ�ύ���޸�
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            return this.dgvGroups.HasModified() || this.dgvUsers.HasModified();
        }

        /// <summary>
        /// �ύ��ǰ�������޸�����
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        public override bool CommitModify()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            int index = 0;
            int count = 0;
            short shRet = SystemConst.ReturnValue.OK;

            while (index < this.dgvGroups.Rows.Count)
            {
                DataTableViewRow row = this.dgvGroups.Rows[index];
                bool bIsDeleteRow = this.dgvGroups.IsDeletedRow(row);
                shRet = this.SaveGroupData(row);
                if (shRet == SystemConst.ReturnValue.OK)
                    count++;
                else if (shRet == SystemConst.ReturnValue.FAILED)
                    break;
                if (!bIsDeleteRow) index++;
            }

            index = 0;
            while (index < this.dgvUsers.Rows.Count)
            {
                DataTableViewRow row = this.dgvUsers.Rows[index];
                bool bIsDeleteRow = this.dgvUsers.IsDeletedRow(row);
                shRet = this.SaveUserData(row);
                if (shRet == SystemConst.ReturnValue.OK)
                    count++;
                else if (shRet == SystemConst.ReturnValue.FAILED)
                    break;
                if (!bIsDeleteRow) index++;
            }

            this.RefreshUserGroupBoxText();

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            string szMessageText = null;
            if (shRet == SystemConst.ReturnValue.FAILED)
                szMessageText = string.Format("������ֹ,�ѱ���{0}����¼!", count);
            else
                szMessageText = string.Format("����ɹ�,�ѱ���{0}����¼!", count);
            MessageBoxEx.Show(szMessageText, MessageBoxIcon.Information);
            return shRet == SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// �����û����б�����
        /// </summary>
        private void LoadGroupGridViewData()
        {
            this.dgvGroups.Rows.Clear();

            string szItemType = ServerData.CommonDictType.USER_GROUP;
            List<CommonDictItem> lstCommonDictItems = null;
            short shRot = CommonService.Instance.GetCommonDict(szItemType, ref lstCommonDictItems);
            if (shRot != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("�û����ֵ���������ʧ��!");
                return;
            }
            if (lstCommonDictItems == null || lstCommonDictItems.Count <= 0)
                return;

            foreach (CommonDictItem commonDictItem in lstCommonDictItems)
            {
                if (commonDictItem == null)
                    continue;
                if (GlobalMethods.Misc.IsEmptyString(commonDictItem.ItemCode))
                    continue;
                int rowIndex = this.dgvGroups.Rows.Add();
                DataTableViewRow row = this.dgvGroups.Rows[rowIndex];
                this.SetGroupRowData(row, commonDictItem);
                this.dgvGroups.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// ������������û��б�����
        /// </summary>
        /// <param name="commonDictItem">�����</param>
        private void LoadUserGridViewData(CommonDictItem commonDictItem)
        {
            CommonDictItem currentItem = this.dgvUsers.Tag as CommonDictItem;
            if (currentItem != null && commonDictItem != null
                && currentItem.ItemCode == commonDictItem.ItemCode)
            {
                return;
            }
            this.dgvUsers.Rows.Clear();
            this.dgvUsers.Tag = commonDictItem;
            this.RefreshUserGroupBoxText();
            if (commonDictItem == null)
                return;

            string szGroupCode = commonDictItem.ItemCode;
            List<UserGroup> lstUserGroups = null;
            short shRot = AccountService.Instance.GetGroupUserList(szGroupCode, ref lstUserGroups);
            if (shRot != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("���û��б���������ʧ��!");
                return;
            }
            if (lstUserGroups == null || lstUserGroups.Count <= 0)
                return;

            foreach (UserGroup userGroup in lstUserGroups)
            {
                if (userGroup == null)
                    continue;
                int rowIndex = this.dgvUsers.Rows.Add();
                DataTableViewRow row = this.dgvUsers.Rows[rowIndex];
                this.SetUserRowData(row, userGroup);
                this.dgvUsers.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// �����û�����������
        /// </summary>
        /// <param name="row">�û�������</param>
        /// <param name="commonDictItem">������</param>
        private void SetGroupRowData(DataTableViewRow row, CommonDictItem commonDictItem)
        {
            if (row == null || row.Index < 0)
                return;
            row.Tag = commonDictItem;

            if (commonDictItem == null)
            {
                row.Cells[this.colGroupCode.Index].Value = null;
                row.Cells[this.colGroupName.Index].Value = null;
            }
            else
            {
                row.Cells[this.colGroupCode.Index].Value = commonDictItem.ItemCode;
                row.Cells[this.colGroupName.Index].Value = commonDictItem.ItemName;
            }
        }

        /// <summary>
        /// �����û��б���������
        /// </summary>
        /// <param name="row">�û��б�����</param>
        /// <param name="userGroup">������</param>
        private void SetUserRowData(DataTableViewRow row, UserGroup userGroup)
        {
            if (row == null || row.Index < 0)
                return;
            row.Tag = userGroup;

            if (userGroup == null)
            {
                row.Cells[this.colUserID.Index].Value = null;
                row.Cells[this.colUserName.Index].Value = null;
                row.Cells[this.colDeptName.Index].Value = null;
                row.Cells[this.colPriority.Index].Value = null;
            }
            else
            {
                row.Cells[this.colUserID.Index].Value = userGroup.UserInfo.ID;
                row.Cells[this.colUserName.Index].Value = userGroup.UserInfo.Name;
                row.Cells[this.colDeptName.Index].Value = userGroup.UserInfo.DeptName;
                row.Cells[this.colPriority.Index].Value = userGroup.Priority;
            }
        }

        /// <summary>
        /// �ж�ָ�����û��Ƿ��ڵ�ǰ���û��б���
        /// </summary>
        /// <param name="szUserID">�û�ID</param>
        /// <returns>true-���б���</returns>
        private bool IsUserInGroup(string szUserID)
        {
            foreach (DataGridViewRow row in this.dgvUsers.Rows)
            {
                UserGroup userGroup = row.Tag as UserGroup;
                if (userGroup != null && userGroup.UserInfo.ID == szUserID)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ���ݵ�ǰѡ�������Ϣˢ��GroupBox����
        /// </summary>
        private void RefreshUserGroupBoxText()
        {
            string szItemName = string.Empty;
            DataGridViewRow currentRow = this.dgvGroups.CurrentRow;
            if (currentRow != null)
                szItemName = currentRow.Cells[this.colGroupName.Index].Value as string;
            this.gbxUsers.Text = string.Format("{0}���û��б�", szItemName);
        }

        /// <summary>
        /// ��ȡ�û������������޸ĵ�����
        /// </summary>
        /// <param name="row">�û�������</param>
        /// <param name="commonDictItem">������</param>
        /// <returns>bool</returns>
        private bool MakeGroupRowData(DataTableViewRow row, ref CommonDictItem commonDictItem)
        {
            if (row == null || row.Index < 0)
                return false;
            CommonDictItem old = row.Tag as CommonDictItem;
            if (old == null)
                return false;

            if (this.dgvGroups.IsDeletedRow(row))
            {
                commonDictItem = old;
                return true;
            }
            commonDictItem = old.Clone() as CommonDictItem;

            object cellValue = row.Cells[this.colGroupCode.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvGroups.SelectRow(row);
                MessageBoxEx.Show("�����������û������!");
                return false;
            }
            commonDictItem.ItemCode = cellValue.ToString();

            cellValue = row.Cells[this.colGroupName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvGroups.SelectRow(row);
                MessageBoxEx.Show("�����������û�������!");
                return false;
            }
            commonDictItem.ItemName = cellValue.ToString();

            commonDictItem.ItemType = ServerData.CommonDictType.USER_GROUP;
            return true;
        }

        /// <summary>
        /// ��ȡ�û��б����������޸ĵ�����
        /// </summary>
        /// <param name="row">�û��б�����</param>
        /// <param name="userGroup">������</param>
        /// <returns>bool</returns>
        private bool MakeUserRowData(DataTableViewRow row, ref UserGroup userGroup)
        {
            if (row == null || row.Index < 0)
                return false;
            UserGroup old = row.Tag as UserGroup;
            if (old == null)
                return false;
            if (this.dgvUsers.IsDeletedRow(row))
            {
                userGroup = old;
                return true;
            }

            DataGridViewRow currentRow = this.dgvGroups.CurrentRow;
            CommonDictItem commonDictItem = null;
            if (currentRow != null)
                commonDictItem = currentRow.Tag as CommonDictItem;
            if (commonDictItem == null)
            {
                this.dgvUsers.SelectRow(row);
                MessageBoxEx.Show("������ѡ��һ���û���!");
                return false;
            }

            userGroup = old.Clone() as UserGroup;
            userGroup.GroupCode = commonDictItem.ItemCode;
            object priorityValue = row.Cells[this.colPriority.Index].Value;
            if (priorityValue != null)
                userGroup.Priority = GlobalMethods.Convert.StringToValue(priorityValue.ToString(), 9);
            return true;
        }

        /// <summary>
        /// �����û������������޸ĵ�����
        /// </summary>
        /// <param name="row">�û�������</param>
        /// <returns>SystemConst.ReturnValue</returns>
        private short SaveGroupData(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return SystemConst.ReturnValue.FAILED;

            if (this.dgvGroups.IsNormalRow(row) || this.dgvGroups.IsUnknownRow(row))
            {
                if (!this.dgvGroups.IsDeletedRow(row))
                    return SystemConst.ReturnValue.CANCEL;
            }

            string szOldItemType = string.Empty;
            string szOldItemCode = string.Empty;
            CommonDictItem commonDictItem = row.Tag as CommonDictItem;
            if (commonDictItem == null)
                return SystemConst.ReturnValue.FAILED;
            szOldItemType = commonDictItem.ItemType;
            szOldItemCode = commonDictItem.ItemCode;

            commonDictItem = null;
            if (!this.MakeGroupRowData(row, ref commonDictItem))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dgvGroups.IsDeletedRow(row))
            {
                string szItemType = commonDictItem.ItemType;
                string szItemCode = commonDictItem.ItemCode;
                if (!this.dgvGroups.IsNewRow(row))
                    shRet = CommonService.Instance.DeleteCommonDictItem(szItemType, szItemCode);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvGroups.SelectRow(row);
                    MessageBoxEx.Show("�û�����Ϣɾ��ʧ��!");
                    return SystemConst.ReturnValue.FAILED;
                }

                CommonDictItem currentItem = this.dgvUsers.Tag as CommonDictItem;
                if (currentItem != null && szOldItemCode == currentItem.ItemCode)
                    this.LoadUserGridViewData(null);
                this.dgvGroups.Rows.Remove(row);
            }
            else if (this.dgvGroups.IsModifiedRow(row))
            {
                shRet = CommonService.Instance.UpdateCommonDictItem(szOldItemType, szOldItemCode, commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvGroups.SelectRow(row);
                    MessageBoxEx.Show("�û�����Ϣ����ʧ��!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;

                CommonDictItem currentItem = this.dgvUsers.Tag as CommonDictItem;
                if (currentItem != null && szOldItemCode == currentItem.ItemCode)
                    this.LoadUserGridViewData(commonDictItem);
                this.dgvGroups.SetRowState(row, RowState.Normal);
            }
            else if (this.dgvGroups.IsNewRow(row))
            {
                shRet = CommonService.Instance.SaveCommonDictItem(commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvGroups.SelectRow(row);
                    MessageBoxEx.Show("�û�����Ϣ����ʧ��!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;
                this.dgvGroups.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// �����û��б����������޸ĵ�����
        /// </summary>
        /// <param name="row">�û��б�����</param>
        /// <returns>SystemConst.ReturnValue</returns>
        private short SaveUserData(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return SystemConst.ReturnValue.FAILED;

            if (this.dgvUsers.IsNormalRow(row) || this.dgvUsers.IsUnknownRow(row))
            {
                if (!this.dgvUsers.IsDeletedRow(row))
                    return SystemConst.ReturnValue.CANCEL;
            }

            UserGroup userGroup = row.Tag as UserGroup;
            if (userGroup == null)
                return SystemConst.ReturnValue.FAILED;

            string szOldGroupCode = userGroup.GroupCode;
            string szOldUserID = userGroup.UserInfo.ID;

            userGroup = null;
            if (!this.MakeUserRowData(row, ref userGroup))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dgvUsers.IsDeletedRow(row))
            {
                string szGroupCode = userGroup.GroupCode;
                string szUserID = userGroup.UserInfo.ID;
                if (!this.dgvUsers.IsNewRow(row))
                    shRet = AccountService.Instance.DeleteUserGroup(szGroupCode, szUserID);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvUsers.SelectRow(row);
                    MessageBoxEx.Show("���û���Ϣɾ��ʧ��!");
                    return SystemConst.ReturnValue.FAILED;
                }
                this.dgvUsers.Rows.Remove(row);
            }
            else if (this.dgvGroups.IsModifiedRow(row))
            {
                shRet = AccountService.Instance.UpdateUserGroup(szOldGroupCode, szOldUserID, userGroup);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvUsers.SelectRow(row);
                    MessageBoxEx.Show("���û���Ϣ����ʧ��!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = userGroup;
                this.dgvUsers.SetRowState(row, RowState.Normal);
            }
            else if (this.dgvUsers.IsNewRow(row))
            {
                shRet = AccountService.Instance.SaveUserGroup(userGroup);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvUsers.SelectRow(row);
                    MessageBoxEx.Show("���û���Ϣ����ʧ��!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = userGroup;
                this.dgvUsers.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        private void mnuDeleteGroup_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvGroups.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvGroups.SelectedRows[index];
                this.dgvGroups.SetRowState(row, RowState.Delete);
            }
        }

        private void mnnCancelModifyGroup_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvGroups.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvGroups.SelectedRows[index];
                if (!this.dgvGroups.IsModifiedRow(row))
                    continue;
                this.SetGroupRowData(row, row.Tag as CommonDictItem);
                this.dgvGroups.SetRowState(row, RowState.Normal);
            }
        }

        private void mnuCancelDeleteGroup_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvGroups.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvGroups.SelectedRows[index];
                if (!this.dgvGroups.IsDeletedRow(row))
                    continue;
                if (this.dgvGroups.IsNewRow(row))
                    this.dgvGroups.SetRowState(row, RowState.New);
                else if (this.dgvGroups.IsModifiedRow(row))
                    this.dgvGroups.SetRowState(row, RowState.Update);
                else
                    this.dgvGroups.SetRowState(row, RowState.Normal);
            }
        }

        private void mnuDeleteUser_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvUsers.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvUsers.SelectedRows[index];
                this.dgvUsers.SetRowState(row, RowState.Delete);
            }
        }

        private void mnuCancelModifyUser_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvUsers.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvUsers.SelectedRows[index];
                if (!this.dgvUsers.IsModifiedRow(row))
                    continue;
                this.SetGroupRowData(row, row.Tag as CommonDictItem);
                this.dgvUsers.SetRowState(row, RowState.Normal);
            }
        }

        private void mnuCancelDeleteUser_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvUsers.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvUsers.SelectedRows[index];
                if (!this.dgvUsers.IsDeletedRow(row))
                    continue;
                if (this.dgvUsers.IsNewRow(row))
                    this.dgvUsers.SetRowState(row, RowState.New);
                else if (this.dgvUsers.IsModifiedRow(row))
                    this.dgvUsers.SetRowState(row, RowState.Update);
                else
                    this.dgvUsers.SetRowState(row, RowState.Normal);
            }
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            CommonDictItem commonDictItem = null;
            DataTableViewRow currRow = this.dgvGroups.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                commonDictItem = currRow.Tag as CommonDictItem;
            if (commonDictItem == null)
                commonDictItem = new CommonDictItem();
            else
                commonDictItem = commonDictItem.Clone() as CommonDictItem;
            commonDictItem.ItemType = ServerData.CommonDictType.USER_GROUP;
            commonDictItem.ItemCode = commonDictItem.MakeItemCode();

            int nRowIndex = this.dgvGroups.Rows.Count;
            if (this.dgvGroups.CurrentRow != null)
                nRowIndex = this.dgvGroups.CurrentRow.Index + 1;
            this.dgvGroups.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dgvGroups.Rows[nRowIndex];
            this.SetGroupRowData(row, commonDictItem);

            this.dgvGroups.Focus();
            this.dgvGroups.SelectRow(row);
            this.dgvGroups.SetRowState(row, RowState.New);
            this.LoadUserGridViewData(commonDictItem);
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            CommonDictItem commonDictItem = null;
            if (this.dgvGroups.CurrentRow != null)
                commonDictItem = this.dgvGroups.CurrentRow.Tag as CommonDictItem;
            if (commonDictItem == null
                || GlobalMethods.Misc.IsEmptyString(commonDictItem.ItemCode))
            {
                MessageBoxEx.Show("��ѡ��һ���û����,����������û�!");
                return;
            }

            List<UserInfo> lstUserInfos = new List<UserInfo>();
            foreach (DataGridViewRow row in this.dgvUsers.Rows)
            {
                UserGroup group = row.Tag as UserGroup;
                if (group == null)
                    continue;
                UserInfo userInfo = group.UserInfo.Clone() as UserInfo;
                if (!string.IsNullOrEmpty(userInfo.ID))
                    lstUserInfos.Add(userInfo);
            }
            UserInfo[] userInfos = lstUserInfos.ToArray();
            userInfos = UtilitiesHandler.Instance.ShowUserSelectDialog(true, userInfos);

            if (userInfos == null)
                return;
            foreach (UserInfo userInfo in userInfos)
            {
                if (userInfo == null)
                    continue;
                if (this.IsUserInGroup(userInfo.ID))
                    continue;
                UserGroup userGroup = null;
                DataTableViewRow currRow = this.dgvUsers.CurrentRow;
                if (currRow != null && currRow.Index >= 0)
                    userGroup = currRow.Tag as UserGroup;
                if (userGroup == null)
                    userGroup = new UserGroup();
                else
                    userGroup = userGroup.Clone() as UserGroup;
                userGroup.GroupCode = commonDictItem.ItemCode;
                userGroup.UserInfo = userInfo.Clone() as UserInfo;

                int nRowIndex = this.dgvUsers.Rows.Add();
                DataTableViewRow newRow = this.dgvUsers.Rows[nRowIndex];
                this.SetUserRowData(newRow, userGroup);

                this.dgvUsers.Focus();
                this.dgvUsers.SelectRow(newRow);
                this.dgvUsers.SetRowState(newRow, RowState.New);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.CommitModify();
        }

        private void dgvGroups_SelectionChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.dgvGroups.CurrentRow != null)
                this.LoadUserGridViewData(this.dgvGroups.CurrentRow.Tag as CommonDictItem);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvGroups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadUserGridViewData(this.dgvGroups.Rows[e.RowIndex].Tag as CommonDictItem);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvGroups_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.RefreshUserGroupBoxText();
        }
    }
}