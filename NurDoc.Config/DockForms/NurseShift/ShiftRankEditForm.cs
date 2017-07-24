// ***********************************************************
// ���������ù���ϵͳ,���������ù�����.
// Author : YangMingkun, Date : 2013-6-16
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities.Dialogs;
using Heren.NurDoc.Config.Dialogs;

namespace Heren.NurDoc.Config.DockForms
{
    public partial class ShiftRankEditForm : DockContentBase
    {
        public ShiftRankEditForm(MainForm mainForm)
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
        }

        public override void OnRefreshView()
        {
            base.OnRefreshView();
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadGridViewData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ����Ƿ���δ���������
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            return this.dataTableView1.HasModified();
        }

        /// <summary>
        /// ���浱ǰ���ڵ������޸�
        /// </summary>
        /// <returns>bool</returns>
        public override bool CommitModify()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            int index = 0;
            int count = 0;
            short shRet = SystemConst.ReturnValue.OK;
            while (index < this.dataTableView1.Rows.Count)
            {
                DataTableViewRow row = this.dataTableView1.Rows[index];
                bool bIsDeletedRow = this.dataTableView1.IsDeletedRow(row);
                shRet = this.SaveRowData(row);
                if (shRet == SystemConst.ReturnValue.OK)
                    count++;
                else if (shRet == SystemConst.ReturnValue.FAILED)
                    break;
                if (!bIsDeletedRow) index++;
            }
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
        /// ��ȡ���ݿ�,װ�ص�ǰDataGridView��������
        /// </summary>
        private void LoadGridViewData()
        {
            this.dataTableView1.Rows.Clear();
            List<ShiftRankInfo> lstShiftRankInfos = null;
            short shRet = NurShiftService.Instance.GetShiftRankInfos(null,null, ref lstShiftRankInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("�������ֵ��б�����ʧ��!");
                return;
            }
            if (lstShiftRankInfos == null || lstShiftRankInfos.Count <= 0)
                return;

            for (int index = 0; index < lstShiftRankInfos.Count; index++)
            {
                ShiftRankInfo shiftRankInfo = lstShiftRankInfos[index];
                if (shiftRankInfo == null)
                    continue;
                int nRowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                this.SetRowData(row, shiftRankInfo);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// ����ָ������ʾ������,�Լ��󶨵�����
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="shiftRankInfo">�󶨵�����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool SetRowData(DataTableViewRow row, ShiftRankInfo shiftRankInfo)
        {
            if (row == null || row.Index < 0 || shiftRankInfo == null)
                return false;
            row.Tag = shiftRankInfo;
            row.Cells[this.colRankNo.Index].Value = shiftRankInfo.RankNo;
            row.Cells[this.colRankCode.Index].Value = shiftRankInfo.RankCode;
            row.Cells[this.colRankName.Index].Value = shiftRankInfo.RankName;
            row.Cells[this.colStartPoint.Index].Value =
                GlobalMethods.Convert.StringToValue(shiftRankInfo.StartPoint, DateTime.Now);
            row.Cells[this.colEndPoint.Index].Value =
                GlobalMethods.Convert.StringToValue(shiftRankInfo.EndPoint, DateTime.Now);
            row.Cells[this.colWardName.Index].Tag = shiftRankInfo.WardCode;
            row.Cells[this.colWardName.Index].Value = shiftRankInfo.WardName;
            return true;
        }

        /// <summary>
        /// ��ȡָ���������޸ĺ������
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="shiftRankInfo">�����޸ĺ������</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref ShiftRankInfo shiftRankInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (row == null || row.Index < 0)
                return false;
            ShiftRankInfo old = row.Tag as ShiftRankInfo;
            if (old == null)
                return false;
            if (this.dataTableView1.IsDeletedRow(row))
            {
                shiftRankInfo = old;
                return true;
            }

            shiftRankInfo = old.Clone() as ShiftRankInfo;

            object cellValue = row.Cells[this.colRankCode.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("���������뽻���δ���!");
                return false;
            }
            shiftRankInfo.RankCode = cellValue.ToString();

            cellValue = row.Cells[this.colRankName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("���������뽻��������!");
                return false;
            }
            shiftRankInfo.RankName = cellValue.ToString();

            cellValue = row.Cells[this.colStartPoint.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("���������뽻������ʼʱ���!");
                return false;
            }
            shiftRankInfo.StartTime =
                GlobalMethods.Convert.StringToValue(cellValue, DateTime.Now);
            shiftRankInfo.StartPoint = shiftRankInfo.StartTime.ToString("HH:mm");

            cellValue = row.Cells[this.colEndPoint.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("���������뽻���ν�ֹʱ���!");
                return false;
            }
            shiftRankInfo.EndTime =
                GlobalMethods.Convert.StringToValue(cellValue, DateTime.Now);
            shiftRankInfo.EndPoint = shiftRankInfo.EndTime.ToString("HH:mm");

            cellValue = row.Cells[this.colWardName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                shiftRankInfo.WardName = "ȫԺ";
            else
                shiftRankInfo.WardName = cellValue.ToString();

            cellValue = row.Cells[this.colWardName.Index].Tag;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                shiftRankInfo.WardCode = "ALL";
            else
                shiftRankInfo.WardCode = cellValue.ToString();

            cellValue = row.Cells[this.colRankNo.Index].Value;
            shiftRankInfo.RankNo = GlobalMethods.Convert.StringToValue(cellValue, 0);
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

            if (this.dataTableView1.IsNormalRow(row) || this.dataTableView1.IsUnknownRow(row))
            {
                if (!this.dataTableView1.IsDeletedRow(row))
                    return SystemConst.ReturnValue.CANCEL;
            }

            ShiftRankInfo shiftRankInfo = row.Tag as ShiftRankInfo;
            if (shiftRankInfo == null)
                return SystemConst.ReturnValue.FAILED;
            string szRankCode = shiftRankInfo.RankCode;
            string szWardCode = shiftRankInfo.WardCode;

            shiftRankInfo = null;
            if (!this.MakeRowData(row, ref shiftRankInfo))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dataTableView1.IsDeletedRow(row))
            {
                if (!this.dataTableView1.IsNewRow(row))
                    shRet = NurShiftService.Instance.DeleteShiftRankInfo(szRankCode, szWardCode);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("�޷�ɾ����ǰ��¼!");
                    return SystemConst.ReturnValue.FAILED;
                }
                this.dataTableView1.Rows.Remove(row);
            }
            else if (this.dataTableView1.IsModifiedRow(row))
            {
                shRet = NurShiftService.Instance.UpdateShiftRankInfo(szRankCode, szWardCode, shiftRankInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("�޷����µ�ǰ��¼!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = shiftRankInfo;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            else if (this.dataTableView1.IsNewRow(row))
            {
                shRet = NurShiftService.Instance.SaveShiftRankInfo(shiftRankInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("�޷����浱ǰ��¼!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = shiftRankInfo;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ��ʾ�������ô���
        /// </summary>
        /// <param name="row">�����</param>
        private void ShowDeptSelectForm(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return;
            string szWardCode = string.Empty;
            object objValue = row.Cells[this.colWardName.Index].Tag;
            if (!GlobalMethods.Misc.IsEmptyString(objValue))
                szWardCode = objValue.ToString();

            DeptSelectDialog deptSelectDialog = new DeptSelectDialog();
            deptSelectDialog.MultiSelect = false;

            DeptInfo deptInfo = new DeptInfo();
            deptInfo.DeptCode = szWardCode;
            deptSelectDialog.DeptInfos = new DeptInfo[] { deptInfo };
            if (deptSelectDialog.ShowDialog() != DialogResult.OK)
                return;

            deptInfo = null;
            if (deptSelectDialog.DeptInfos != null
                && deptSelectDialog.DeptInfos.Length > 0)
            {
                deptInfo = deptSelectDialog.DeptInfos[0];
            }
            if (deptInfo == null)
            {
                row.Cells[this.colWardName.Index].Tag = "ALL";
                row.Cells[this.colWardName.Index].Value = "ȫԺ";
            }
            else
            {
                row.Cells[this.colWardName.Index].Tag = deptInfo.DeptCode;
                row.Cells[this.colWardName.Index].Value = deptInfo.DeptName;
            }
            if (this.dataTableView1.IsNormalRowUndeleted(row))
                this.dataTableView1.SetRowState(row, RowState.Update);
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            this.btnAdd.PerformClick();
        }

        private void mnnCancelModify_Click(object sender, EventArgs e)
        {
            int nCount = this.dataTableView1.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dataTableView1.SelectedRows[index];
                if (!this.dataTableView1.IsModifiedRow(row))
                    continue;
                this.SetRowData(row, row.Tag as ShiftRankInfo);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        private void mnuCancelDelete_Click(object sender, EventArgs e)
        {
            int nCount = this.dataTableView1.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dataTableView1.SelectedRows[index];
                if (!this.dataTableView1.IsDeletedRow(row))
                    continue;
                if (this.dataTableView1.IsNewRow(row))
                    this.dataTableView1.SetRowState(row, RowState.New);
                else if (this.dataTableView1.IsModifiedRow(row))
                    this.dataTableView1.SetRowState(row, RowState.Update);
                else
                    this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        private void mnuDeleteRecord_Click(object sender, EventArgs e)
        {
            int nCount = this.dataTableView1.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dataTableView1.SelectedRows[index];
                this.dataTableView1.SetRowState(row, RowState.Delete);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserInfo userInfo = SystemContext.Instance.LoginUser;
            if (userInfo == null)
                return;
            ShiftRankInfo shiftRankInfo = null;
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                shiftRankInfo = currRow.Tag as ShiftRankInfo;
            if (shiftRankInfo == null)
                shiftRankInfo = new ShiftRankInfo();
            else
                shiftRankInfo = shiftRankInfo.Clone() as ShiftRankInfo;
            shiftRankInfo.RankCode = shiftRankInfo.MakeRankCode();

            int nRowIndex = this.dataTableView1.Rows.Count;
            if (this.dataTableView1.CurrentRow != null)
                nRowIndex = this.dataTableView1.CurrentRow.Index + 1;
            this.dataTableView1.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
            this.SetRowData(row, shiftRankInfo);

            this.dataTableView1.Focus();
            this.dataTableView1.SelectRow(row);
            this.dataTableView1.SetRowState(row, RowState.New);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.HasUncommit())
                this.CommitModify();
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            if (row == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (e.ColumnIndex == this.colWardName.Index)
                this.ShowDeptSelectForm(row);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}