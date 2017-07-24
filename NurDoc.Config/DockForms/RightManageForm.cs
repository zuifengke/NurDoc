// ***********************************************************
// ���������ù���ϵͳ,�û�Ȩ�����ù�����.
// Author : YangMingkun, Date : 2012-6-7
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Config.DockForms
{
    internal partial class RightManageForm : DockContentBase
    {
        private UserRightType m_userRightType = UserRightType.NurDoc;

        /// <summary>
        /// ��ȡ��ǰ������ά�����û�Ȩ������
        /// </summary>
        [Browsable(false)]
        public UserRightType UserRightType
        {
            get { return this.m_userRightType; }
        }

        private PropertyInfo[] m_userRightPropertys = null;

        /// <summary>
        /// ��ȡ�û�Ȩ����Ϣ�����Լ���
        /// </summary>
        [Browsable(false)]
        private PropertyInfo[] UserRightPropertys
        {
            get
            {
                if (this.m_userRightPropertys == null)
                {
                    this.m_userRightPropertys = typeof(NurUserRight).GetProperties();
                }
                return this.m_userRightPropertys;
            }
        }

        /// <summary>
        /// ��¼Ȩ�޵���������ֵ��
        /// </summary>
        private Hashtable m_htRightColumn = null;

        /// <summary>
        /// ��Ҫ��ʾ��Ȩ�޵�ѡ�񴰿�
        /// </summary>
        private RightsSelectForm m_RightsSelectForm = null;

        public RightManageForm(MainForm mainForm, UserRightType rightType)
            : base(mainForm)
        {
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
            this.m_userRightType = rightType;
            this.Text = "������ϵͳȨ�޹���";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.UpdateBounds();
            this.LoadRightColumns();
            this.btnQueryByDept.Image = Properties.Resources.Query;
            this.btnUserRightSelect.Image = Properties.Resources.DisplayColumn;
        }

        public override void OnRefreshView()
        {
            base.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDeptList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ����Ƿ���δ���������
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            int nCount = this.dgvUserRight.Rows.Count;
            if (nCount <= 0)
                return false;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvUserRight.Rows[index];
                if (this.dgvUserRight.IsDeletedRow(row))
                    return true;
                if (this.dgvUserRight.IsNewRow(row))
                    return true;
                if (this.dgvUserRight.IsModifiedRow(row))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ���浱ǰ���ڵ������޸�
        /// </summary>
        /// <returns>bool</returns>
        public override bool CommitModify()
        {
            WorkProcess.Instance.Initialize(this, this.dgvUserRight.RowCount
                , "�����ύ�û�Ȩ�ޣ����Ժ�...");
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            int index = 0;
            int count = 0;
            short shRet = SystemConst.ReturnValue.OK;
            while (index < this.dgvUserRight.Rows.Count)
            {
                WorkProcess.Instance.Show(null, index + 1);
                if (WorkProcess.Instance.Canceled)
                    break;

                DataTableViewRow row = this.dgvUserRight.Rows[index];
                bool bIsDeletedRow = this.dgvUserRight.IsDeletedRow(row);
                shRet = this.SaveRowData(row);
                if (shRet == SystemConst.ReturnValue.OK)
                    count++;
                else if (shRet == SystemConst.ReturnValue.FAILED)
                    break;
                if (!bIsDeletedRow) index++;
            }
            WorkProcess.Instance.Close();
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
        /// ���ظ�Ȩ�޵�������
        /// </summary>
        private void LoadRightColumns()
        {
            if (this.m_htRightColumn == null)
                this.m_htRightColumn = new Hashtable();
            this.m_htRightColumn.Clear();

            UserRightBase userRight =
                UserRightBase.Create(this.m_userRightType);
            for (int index = 0; index < this.UserRightPropertys.Length; index++)
            {
                PropertyInfo propertyInfo = this.UserRightPropertys[index];
                if (propertyInfo.PropertyType != typeof(RightInfo))
                    continue;

                object objRightValue = propertyInfo.GetValue(userRight, null);
                RightInfo rightInfo = objRightValue as RightInfo;
                if (rightInfo == null)
                    continue;

                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
                column.Tag = rightInfo;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                column.HeaderText = rightInfo.Name;
                column.Width = 60;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                int nColIndex = this.dgvUserRight.Columns.Add(column);
                if (!this.m_htRightColumn.Contains(rightInfo.Name))
                    this.m_htRightColumn.Add(rightInfo.Name, nColIndex);
            }
        }

        /// <summary>
        /// �����ٴ������б�
        /// </summary>
        private void LoadDeptList()
        {
            this.cboDeptList.Items.Clear();

            List<DeptInfo> lstDeptInfos = null;
            short shResult = CommonService.Instance.GetAllDeptInfos(ref lstDeptInfos);
            if (shResult != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ȡ������Ϣ�б�ʱ��������");
                return;
            }
            if (lstDeptInfos == null)
                return;
            for (int index = 0; index < lstDeptInfos.Count; index++)
            {
                DeptInfo deptInfo = lstDeptInfos[index];
                if (deptInfo != null)
                    this.cboDeptList.Items.Add(deptInfo);
            }
        }

        /// <summary>
        /// ��ָ�����û��б���ص�DataGridView����
        /// </summary>
        /// <param name="lstUserInfos">�û���Ϣ�б�</param>
        private void LoadGridViewData(List<UserInfo> lstUserInfos)
        {
            this.dgvUserRight.Rows.Clear();
            this.Update();
            if (lstUserInfos == null || lstUserInfos.Count <= 0)
            {
                MessageBoxEx.Show("û�в�ѯ�����������ļ�¼��"
                    , MessageBoxIcon.Information);
                return;
            }
            Hashtable htUserRow = new Hashtable();
            short shRet = SystemConst.ReturnValue.OK;
            for (int index = 0; index < lstUserInfos.Count; index++)
            {
                UserInfo userInfo = lstUserInfos[index];
                if (userInfo == null)
                    continue;
                int nRowIndex = this.dgvUserRight.Rows.Add();
                DataTableViewRow row = this.dgvUserRight.Rows[nRowIndex];
                if (htUserRow.Contains(userInfo.ID))
                    continue;
                htUserRow.Add(userInfo.ID, row);

                if (userInfo.ID != null)
                    row.Cells[this.colUserID.Index].Value = userInfo.ID.Trim();
                if (userInfo.Name != null)
                    row.Cells[this.colUserName.Index].Value = userInfo.Name.Trim();
                if (userInfo.DeptName != null)
                    row.Cells[this.colDeptName.Index].Value = userInfo.DeptName.Trim();

                if (index % 2 == 0)
                    row.DefaultCellStyle.BackColor = Color.White;
                else
                    row.DefaultCellStyle.BackColor = Color.WhiteSmoke;

                UserRightBase userRight = null;
                //С��100����¼ʱ,�͵�����ȡ�û���Ϣ
                if (lstUserInfos.Count <= 100)
                {
                    string szUserID = userInfo.ID.Trim();
                    shRet = AccountService.Instance.GetUserRight(userInfo.ID, this.m_userRightType, ref userRight);
                    if (shRet != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.Show("�û�Ȩ���б�����ʧ��!");
                        return;
                    }
                }
                this.SetRowData(row, userRight);
                this.dgvUserRight.SetRowState(row, RowState.Normal);
            }
            if (lstUserInfos.Count > 100)
            {
                List<UserRightBase> lstUserRights = null;
                shRet = AccountService.Instance.GetUserRight(this.m_userRightType, ref lstUserRights);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("�û�Ȩ���б�����ʧ��!");
                    return;
                }
                if (lstUserRights == null || lstUserRights.Count <= 0)
                    return;
                for (int index = 0; index < lstUserRights.Count; index++)
                {
                    UserRightBase userRight = lstUserRights[index];
                    if (userRight == null)
                        continue;

                    string szUserID = userRight.UserID.Trim();
                    if (!htUserRow.Contains(szUserID))
                        continue;

                    this.SetRowData(htUserRow[szUserID] as DataTableViewRow, userRight);
                }
            }
        }

        /// <summary>
        /// ����ָ���û���Ȩ����Ϣ����ǰDataGridView�ؼ�ĩβ
        /// </summary>
        /// <param name="row">Ȩ�޶������Լ���(�ɿ�)</param>
        /// <param name="userRight">�û���Ȩ����Ϣ</param>
        private void SetRowData(DataTableViewRow row, UserRightBase userRight)
        {
            if (row == null || row.Index < 0)
                return;
            if (userRight == null)
                userRight = UserRightBase.Create(this.m_userRightType);
            row.Tag = userRight;

            if (this.dgvUserRight.ColumnCount <= this.colDeptName.Index + 1)
                return;
            if (userRight == null)
                return;
            for (int index = 0; index < this.UserRightPropertys.Length; index++)
            {
                PropertyInfo propertyInfo = this.UserRightPropertys[index];
                if (propertyInfo.PropertyType != typeof(RightInfo))
                    continue;

                RightInfo rightInfo = null;
                rightInfo = propertyInfo.GetValue(userRight, null) as RightInfo;
                if (rightInfo == null)
                    continue;

                if (!this.m_htRightColumn.Contains(rightInfo.Name))
                    continue;
                object value = this.m_htRightColumn[rightInfo.Name];
                if (value == null)
                    continue;

                int nColumnIndex = 0;
                if (!int.TryParse(value.ToString(), out nColumnIndex))
                    continue;
                if (nColumnIndex < 0 || nColumnIndex >= this.dgvUserRight.ColumnCount)
                    continue;
                row.Cells[nColumnIndex].Value = rightInfo.Value;
            }
        }

        /// <summary>
        /// ��ȡָ���е�ǰ��Ȩ������
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="userRight">�û���Ȩ����Ϣ</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref UserRightBase userRight)
        {
            if (row == null || row.Index < 0)
                return false;

            object cellValue = row.Cells[this.colUserID.Index].Value;
            if (cellValue == null || cellValue.ToString().Trim() == string.Empty)
                return false;

            userRight = UserRightBase.Create(this.m_userRightType);
            userRight.UserID = cellValue.ToString().Trim();

            for (int index = 0; index < this.UserRightPropertys.Length; index++)
            {
                PropertyInfo propertyInfo = this.UserRightPropertys[index];
                if (propertyInfo.PropertyType != typeof(RightInfo))
                    continue;

                RightInfo rightInfo = null;
                rightInfo = propertyInfo.GetValue(userRight, null) as RightInfo;
                if (rightInfo == null)
                    continue;

                if (!this.m_htRightColumn.Contains(rightInfo.Name))
                    continue;
                object value = this.m_htRightColumn[rightInfo.Name];
                if (value == null)
                    continue;

                int nColumnIndex = 0;
                if (!int.TryParse(value.ToString(), out nColumnIndex))
                    continue;
                if (nColumnIndex < 0 || nColumnIndex >= this.dgvUserRight.ColumnCount)
                    continue;

                rightInfo.Value = false;
                cellValue = row.Cells[nColumnIndex].Value;
                if (cellValue != null && cellValue.ToString().Trim() != string.Empty)
                {
                    bool bValue = false;
                    if (bool.TryParse(cellValue.ToString(), out bValue))
                        rightInfo.Value = bValue;
                }
            }
            return true;
        }

        /// <summary>
        /// ����ָ���е����ݵ�Զ�����ݱ�,��Ҫע����ǣ��е�ɾ��״̬��������״̬����
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <returns>SystemConst.ReturnValue</returns>
        private short SaveRowData(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return SystemConst.ReturnValue.FAILED;

            if (this.dgvUserRight.IsNormalRow(row) || this.dgvUserRight.IsUnknownRow(row))
            {
                if (!this.dgvUserRight.IsDeletedRow(row))
                    return SystemConst.ReturnValue.CANCEL;
            }

            UserRightBase userRight = null;
            if (!this.MakeRowData(row, ref userRight))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dgvUserRight.IsDeletedRow(row))
            {
                //��ɾ������
            }
            else if (this.dgvUserRight.IsNewRow(row))
            {
                shRet = AccountService.Instance.SaveUserRight(userRight);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvUserRight.SelectRow(row);
                    MessageBoxEx.Show(string.Format("Ȩ��{0}����ʧ��!", userRight.UserID));
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = userRight;
                this.dgvUserRight.SetRowState(row, RowState.Normal);
            }
            else if (this.dgvUserRight.IsModifiedRow(row))
            {
                shRet = AccountService.Instance.SaveUserRight(userRight);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvUserRight.SelectRow(row);
                    MessageBoxEx.Show(string.Format("Ȩ��{0}����ʧ��!", userRight.UserID));
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = userRight;
                this.dgvUserRight.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ����ָ����Ȩ�޵���Ϣ,����ָ���и�Ȩ�޵�Ĺ�ѡ״̬
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="lstRightInfos">Ȩ�޵���Ϣ�б�</param>
        private void UpdateCheckState(DataTableViewRow row, List<RightInfo> lstRightInfos)
        {
            if (row == null || row.Index < 0)
                return;
            if (lstRightInfos == null || lstRightInfos.Count <= 0)
                return;
            bool bRowUpdated = false;
            for (int index = 0; index < lstRightInfos.Count; index++)
            {
                RightInfo rightInfo = lstRightInfos[index];
                if (!this.m_htRightColumn.Contains(rightInfo.Name))
                    continue;
                object objValue = this.m_htRightColumn[rightInfo.Name];
                if (objValue == null || objValue.ToString().Trim() == string.Empty)
                    continue;
                int colIndex = 0;
                if (!int.TryParse(objValue.ToString(), out colIndex))
                    continue;
                if (colIndex < 0 || colIndex >= this.dgvUserRight.Columns.Count)
                    continue;
                object cellValue = row.Cells[colIndex].Value;
                bool oldRight = false;
                if (bool.TryParse(cellValue.ToString(), out oldRight))
                {
                    if (oldRight != rightInfo.Value)
                        bRowUpdated = true;
                }
                row.Cells[colIndex].Value = rightInfo.Value;
            }
            if (bRowUpdated)
                this.dgvUserRight.SetRowState(row, RowState.Update);
        }

        /// <summary>
        /// ��ʾȨ�޵����öԻ���
        /// </summary>
        private void ShowRightSettingForm()
        {
            DataTableViewSelectedRowCollection selectedRows = this.dgvUserRight.SelectedRows;
            if (selectedRows.Count <= 0)
                return;
            List<RightInfo> lstDisplayRights = new List<RightInfo>();
            List<RightInfo> lstCheckedRights = new List<RightInfo>();

            DataGridViewColumnCollection columns = this.dgvUserRight.Columns;
            int index = 0;
            while (index < columns.Count)
            {
                DataGridViewColumn column = columns[index];
                index++;
                if (!column.Visible)
                    continue;
                RightInfo rightInfo = column.Tag as RightInfo;
                if (rightInfo == null)
                    continue;
                lstDisplayRights.Add(rightInfo);

                //��ѡ���ж���ʱ,Ĭ��ȫ����ѡ,����continue
                if (selectedRows.Count > 1)
                    continue;

                object cellValue = selectedRows[0].Cells[column.Index].Value;
                if (cellValue == null)
                    continue;
                bool bIsChecked = false;
                if (!bool.TryParse(cellValue.ToString(), out bIsChecked))
                    bIsChecked = false;
                if (bIsChecked) lstCheckedRights.Add(rightInfo);
            }

            RightsSelectForm frmRightsSelect = new RightsSelectForm();
            frmRightsSelect.ShowAsModel = true;
            frmRightsSelect.DisplayRights = lstDisplayRights;
            frmRightsSelect.CheckedRights = lstCheckedRights;
            if (frmRightsSelect.ShowDialog() != DialogResult.OK)
                return;
            lstCheckedRights = frmRightsSelect.CheckedRights;
            for (index = 0; index < selectedRows.Count; index++)
            {
                this.UpdateCheckState(selectedRows[index], lstCheckedRights);
            }
        }

        private void QueryByDept()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szDeptName = this.cboDeptList.Text;
            DeptInfo deptInfo = this.cboDeptList.SelectedItem as DeptInfo;
            if (deptInfo == null || deptInfo.DeptName != szDeptName)
            {
                object objItem = null;
                if (szDeptName != string.Empty)
                    objItem = this.cboDeptList.GetItem(szDeptName, false);
                deptInfo = objItem as DeptInfo;
            }
            if (deptInfo == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("������Ŀ��Ҳ�����!"
                    , MessageBoxIcon.Information);
                return;
            }
            List<UserInfo> lstUserInfos = null;
            short shRet = AccountService.Instance.GetDeptUserList(deptInfo.DeptCode, ref lstUserInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("�û��б��ѯ����ʧ��!");
                return;
            }
            this.LoadGridViewData(lstUserInfos);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void QueryByUserID()
        {
            string szUserID = this.txtUserID.Text.Trim();
            if (GlobalMethods.Misc.IsEmptyString(szUserID))
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            UserInfo userInfo = null;
            short shRet = AccountService.Instance.GetUserInfo(szUserID, ref userInfo);
            if (userInfo == null)
            {
                userInfo = new UserInfo();
                userInfo.ID = szUserID;
                userInfo.Name = "�ⲿ��Ա";
            }

            List<UserInfo> lstUserInfos = new List<UserInfo>();
            if (userInfo != null) lstUserInfos.Add(userInfo);
            this.LoadGridViewData(lstUserInfos);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void QueryByUserName()
        {
            string szUserName = this.txtUserName.Text.Trim();
            if (GlobalMethods.Misc.IsEmptyString(szUserName))
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            List<UserInfo> lstUserInfos = null;
            short shRet = AccountService.Instance.GetUserInfo(szUserName, ref lstUserInfos);
            if (shRet == SystemConst.ReturnValue.NO_FOUND)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("δ��ѯ���û�����!");
                return;
            }
            else if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("�û��б��ѯ����ʧ��!");
                return;
            }
            this.LoadGridViewData(lstUserInfos);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void QueryAllUsers()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            List<UserInfo> lstUserInfos = null;
            short shRet = AccountService.Instance.GetAllUserInfos(ref lstUserInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("�û��б��ѯ����ʧ��!");
                return;
            }
            this.LoadGridViewData(lstUserInfos);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void QueryByDeptType(bool bIsWard, bool bIsOutp, bool bIsNurse)
        {
            int nDeptCount = this.cboDeptList.Items.Count;
            if (nDeptCount <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            short shRet = SystemConst.ReturnValue.OK;
            List<UserInfo> lstTotalUserInfos = null;
            for (int index = 0; index < nDeptCount; index++)
            {
                object objItem = this.cboDeptList.Items[index];
                DeptInfo deptInfo = objItem as DeptInfo;
                if (deptInfo == null)
                    continue;
                if (bIsWard && !deptInfo.IsWardDept)
                    continue;
                if (bIsOutp && !deptInfo.IsOutpDept)
                    continue;
                if (bIsNurse && !deptInfo.IsNurseDept)
                    continue;
                if (!bIsWard && !bIsOutp && !bIsNurse
                    && (deptInfo.IsWardDept
                    || deptInfo.IsOutpDept
                    || deptInfo.IsNurseDept))
                    continue;

                string szDeptCode = deptInfo.DeptCode;
                List<UserInfo> lstUserInfos = null;
                shRet = AccountService.Instance.GetDeptUserList(szDeptCode, ref lstUserInfos);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("�û��б��ѯ����ʧ��!");
                    break;
                }
                if (lstUserInfos == null || lstUserInfos.Count <= 0)
                    continue;
                if (lstTotalUserInfos == null)
                    lstTotalUserInfos = new List<UserInfo>();
                lstTotalUserInfos.AddRange(lstUserInfos);
            }
            this.LoadGridViewData(lstTotalUserInfos);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void cboDeptList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                this.QueryByDept();
        }

        private void cboDeptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.QueryByDept();
        }

        private void txtUserID_ButtonClick(object sender, EventArgs e)
        {
            this.QueryByUserID();
        }

        private void txtUserName_ButtonClick(object sender, EventArgs e)
        {
            this.QueryByUserName();
        }

        private void btnQueryByDept_Click(object sender, EventArgs e)
        {
            this.QueryByDept();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.cmenuSelectUser.Show(this.btnSelectAll, 0, this.btnSelectAll.Height);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.CommitModify();
        }

        private void mnuDeptAll_Click(object sender, EventArgs e)
        {
            this.QueryAllUsers();
        }

        private void mnuDeptWard_Click(object sender, EventArgs e)
        {
            this.QueryByDeptType(true, false, false);
        }

        private void mnuDeptOutp_Click(object sender, EventArgs e)
        {
            this.QueryByDeptType(false, true, false);
        }

        private void mnuDeptNurse_Click(object sender, EventArgs e)
        {
            this.QueryByDeptType(false, false, true);
        }

        private void mnuDeptOther_Click(object sender, EventArgs e)
        {
            this.QueryByDeptType(false, false, false);
        }

        private void mnuModifyUserRight_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowRightSettingForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuCancelModify_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvUserRight.SelectedCells.Count;
            if (nCount <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            for (int index = 0; index < nCount; index++)
            {
                DataGridViewCell cell = this.dgvUserRight.SelectedCells[index];
                DataTableViewRow row = this.dgvUserRight.Rows[cell.RowIndex];
                if (this.dgvUserRight.IsNormalRow(row))
                    continue;
                if (this.dgvUserRight.IsNewRow(row))
                    continue;
                this.SetRowData(row, row.Tag as UserRightBase);
                this.dgvUserRight.SetRowState(row, RowState.Normal);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvUserRight_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowRightSettingForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnUserRightSelect_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_RightsSelectForm == null || this.m_RightsSelectForm.IsDisposed)
            {
                List<RightInfo> lstDisplayRights = new List<RightInfo>();
                List<RightInfo> lstCheckedRights = new List<RightInfo>();
                for (int index = 0; index < this.dgvUserRight.Columns.Count; index++)
                {
                    DataGridViewColumn column = this.dgvUserRight.Columns[index];
                    RightInfo rightInfo = column.Tag as RightInfo;
                    lstDisplayRights.Add(rightInfo);
                    if (rightInfo != null && column.Visible)
                        lstCheckedRights.Add(rightInfo);
                }
                this.m_RightsSelectForm = new RightsSelectForm();
                this.m_RightsSelectForm.DisplayRights = lstDisplayRights;
                this.m_RightsSelectForm.CheckedRights = lstCheckedRights;
                this.m_RightsSelectForm.FormBorderStyle = FormBorderStyle.None;
                this.m_RightsSelectForm.TopLevel = false;
                this.m_RightsSelectForm.Parent = this;
                this.m_RightsSelectForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.m_RightsSelectForm.RightCheckStateChanged += this.RightsSelectForm_RightCheckStateChanged;
            }
            if (this.m_RightsSelectForm.Visible)
            {
                this.m_RightsSelectForm.Visible = false;
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.m_RightsSelectForm.Location = new Point(
                this.btnUserRightSelect.Right - this.m_RightsSelectForm.Width, this.btnUserRightSelect.Bottom);
            this.m_RightsSelectForm.Show();
            this.m_RightsSelectForm.BringToFront();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void RightsSelectForm_RightCheckStateChanged(object sender, RightsSelectForm.RightCheckStateChangedEventArgs e)
        {
            if (this.m_htRightColumn == null || this.m_htRightColumn.Count <= 0)
                return;

            if (!this.m_htRightColumn.Contains(e.RightName))
                return;
            object value = this.m_htRightColumn[e.RightName];
            if (value == null)
                return;
            int nColumnIndex = 0;
            if (!int.TryParse(value.ToString(), out nColumnIndex))
                return;
            if (nColumnIndex >= 0 && nColumnIndex < this.dgvUserRight.ColumnCount)
                this.dgvUserRight.Columns[nColumnIndex].Visible = e.IsChecked;
        }

        private void mnuResetUserPassWord_Click(object sender, EventArgs e)
        {
            DataTableViewRow row = this.dgvUserRight.SelectedRows[0];
            UserRightBase userRight = null;
            if (!this.MakeRowData(row, ref userRight))
                return ;
            short shRet = AccountService.Instance.ResetUserPwd(userRight.UserID);
        }
    }
}