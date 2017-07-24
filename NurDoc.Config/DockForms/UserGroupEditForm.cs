// ***********************************************************
// 护理病历配置管理系统,护理用户组配置管理窗口.
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
        /// 是否有未提交的修改
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            return this.dgvGroups.HasModified() || this.dgvUsers.HasModified();
        }

        /// <summary>
        /// 提交当前窗口中修改数据
        /// </summary>
        /// <returns>是否成功</returns>
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
                szMessageText = string.Format("保存中止,已保存{0}条记录!", count);
            else
                szMessageText = string.Format("保存成功,已保存{0}条记录!", count);
            MessageBoxEx.Show(szMessageText, MessageBoxIcon.Information);
            return shRet == SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 加载用户组列表数据
        /// </summary>
        private void LoadGroupGridViewData()
        {
            this.dgvGroups.Rows.Clear();

            string szItemType = ServerData.CommonDictType.USER_GROUP;
            List<CommonDictItem> lstCommonDictItems = null;
            short shRot = CommonService.Instance.GetCommonDict(szItemType, ref lstCommonDictItems);
            if (shRot != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("用户组字典数据下载失败!");
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
        /// 加载组包含的用户列表数据
        /// </summary>
        /// <param name="commonDictItem">组对象</param>
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
                MessageBoxEx.ShowError("组用户列表数据下载失败!");
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
        /// 设置用户组表格行数据
        /// </summary>
        /// <param name="row">用户组表格行</param>
        /// <param name="commonDictItem">行数据</param>
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
        /// 设置用户列表表格行数据
        /// </summary>
        /// <param name="row">用户列表表格行</param>
        /// <param name="userGroup">行数据</param>
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
        /// 判断指定的用户是否在当前组用户列表中
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <returns>true-在列表中</returns>
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
        /// 根据当前选择的组信息刷新GroupBox标题
        /// </summary>
        private void RefreshUserGroupBoxText()
        {
            string szItemName = string.Empty;
            DataGridViewRow currentRow = this.dgvGroups.CurrentRow;
            if (currentRow != null)
                szItemName = currentRow.Cells[this.colGroupName.Index].Value as string;
            this.gbxUsers.Text = string.Format("{0}组用户列表", szItemName);
        }

        /// <summary>
        /// 获取用户组表格行最新修改的数据
        /// </summary>
        /// <param name="row">用户组表格行</param>
        /// <param name="commonDictItem">行数据</param>
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
                MessageBoxEx.Show("您必须输入用户组代码!");
                return false;
            }
            commonDictItem.ItemCode = cellValue.ToString();

            cellValue = row.Cells[this.colGroupName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvGroups.SelectRow(row);
                MessageBoxEx.Show("您必须输入用户组名称!");
                return false;
            }
            commonDictItem.ItemName = cellValue.ToString();

            commonDictItem.ItemType = ServerData.CommonDictType.USER_GROUP;
            return true;
        }

        /// <summary>
        /// 获取用户列表表格行最新修改的数据
        /// </summary>
        /// <param name="row">用户列表表格行</param>
        /// <param name="userGroup">行数据</param>
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
                MessageBoxEx.Show("您必须选择一个用户组!");
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
        /// 保存用户组表格行最新修改的数据
        /// </summary>
        /// <param name="row">用户组表格行</param>
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
                    MessageBoxEx.Show("用户组信息删除失败!");
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
                    MessageBoxEx.Show("用户组信息更新失败!");
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
                    MessageBoxEx.Show("用户组信息保存失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;
                this.dgvGroups.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存用户列表表格行最新修改的数据
        /// </summary>
        /// <param name="row">用户列表表格行</param>
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
                    MessageBoxEx.Show("组用户信息删除失败!");
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
                    MessageBoxEx.Show("组用户信息更新失败!");
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
                    MessageBoxEx.Show("组用户信息保存失败!");
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
                MessageBoxEx.Show("请选择一个用户组后,再向其添加用户!");
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