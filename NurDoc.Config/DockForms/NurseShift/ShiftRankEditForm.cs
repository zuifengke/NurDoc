// ***********************************************************
// 护理病历配置管理系统,交班班次配置管理窗口.
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
        /// 检查是否有未保存的数据
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            return this.dataTableView1.HasModified();
        }

        /// <summary>
        /// 保存当前窗口的数据修改
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
                szMessageText = string.Format("保存中止,已保存{0}条记录!", count);
            else
                szMessageText = string.Format("保存成功,已保存{0}条记录!", count);
            MessageBoxEx.Show(szMessageText, MessageBoxIcon.Information);
            return shRet == SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 读取数据库,装载当前DataGridView各行数据
        /// </summary>
        private void LoadGridViewData()
        {
            this.dataTableView1.Rows.Clear();
            List<ShiftRankInfo> lstShiftRankInfos = null;
            short shRet = NurShiftService.Instance.GetShiftRankInfos(null,null, ref lstShiftRankInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("交班项字典列表下载失败!");
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
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="shiftRankInfo">绑定的数据</param>
        /// <returns>是否成功</returns>
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
        /// 获取指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="shiftRankInfo">最新修改后的数据</param>
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
                MessageBoxEx.Show("您必须输入交班班次代码!");
                return false;
            }
            shiftRankInfo.RankCode = cellValue.ToString();

            cellValue = row.Cells[this.colRankName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入交班班次名称!");
                return false;
            }
            shiftRankInfo.RankName = cellValue.ToString();

            cellValue = row.Cells[this.colStartPoint.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入交班班次起始时间点!");
                return false;
            }
            shiftRankInfo.StartTime =
                GlobalMethods.Convert.StringToValue(cellValue, DateTime.Now);
            shiftRankInfo.StartPoint = shiftRankInfo.StartTime.ToString("HH:mm");

            cellValue = row.Cells[this.colEndPoint.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入交班班次截止时间点!");
                return false;
            }
            shiftRankInfo.EndTime =
                GlobalMethods.Convert.StringToValue(cellValue, DateTime.Now);
            shiftRankInfo.EndPoint = shiftRankInfo.EndTime.ToString("HH:mm");

            cellValue = row.Cells[this.colWardName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                shiftRankInfo.WardName = "全院";
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
        /// 保存指定行的数据到远程数据表,需要注意的是：行的删除状态会与其他状态共存
        /// </summary>
        /// <param name="row">指定行</param>
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
                    MessageBoxEx.Show("无法删除当前记录!");
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
                    MessageBoxEx.Show("无法更新当前记录!");
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
                    MessageBoxEx.Show("无法保存当前记录!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = shiftRankInfo;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 显示病区设置窗口
        /// </summary>
        /// <param name="row">表格行</param>
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
                row.Cells[this.colWardName.Index].Value = "全院";
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