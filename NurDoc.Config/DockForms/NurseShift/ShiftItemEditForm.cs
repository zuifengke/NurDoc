// ***********************************************************
// 护理病历配置管理系统,交班项目配置管理窗口.
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
    public partial class ShiftItemEditForm : DockContentBase
    {
        public ShiftItemEditForm(MainForm mainForm)
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

            string szDictType = ServerData.CommonDictType.SHIFT_ITEM;
            List<CommonDictItem> lstCommonDictItems = null;
            short shRet = CommonService.Instance.GetCommonDict(szDictType, ref lstCommonDictItems);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("交班项字典列表下载失败!");
                return;
            }
            if (lstCommonDictItems == null || lstCommonDictItems.Count <= 0)
                return;

            for (int index = 0; index < lstCommonDictItems.Count; index++)
            {
                CommonDictItem commonDictItem = lstCommonDictItems[index];
                if (commonDictItem == null)
                    continue;
                int nRowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                this.SetRowData(row, commonDictItem);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="commonDictItem">绑定的数据</param>
        /// <returns>bool</returns>
        private bool SetRowData(DataTableViewRow row, CommonDictItem commonDictItem)
        {
            if (row == null || row.Index < 0 || commonDictItem == null)
                return false;
            row.Tag = commonDictItem;
            row.Cells[this.colItemNo.Index].Value = commonDictItem.ItemNo;
            row.Cells[this.colItemCode.Index].Value = commonDictItem.ItemCode;
            row.Cells[this.colItemName.Index].Value = commonDictItem.ItemName;
            row.Cells[this.colWardName.Index].Tag = commonDictItem.WardCode;
            row.Cells[this.colWardName.Index].Value = commonDictItem.WardName;
            return true;
        }

        /// <summary>
        /// 获取指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="commonDictItem">最新修改后的数据</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref CommonDictItem commonDictItem)
        {
            if (row == null || row.Index < 0)
                return false;
            CommonDictItem old = row.Tag as CommonDictItem;
            if (old == null)
                return false;

            if (this.dataTableView1.IsDeletedRow(row))
            {
                commonDictItem = old;
                return true;
            }

            commonDictItem = old.Clone() as CommonDictItem;

            object cellValue = row.Cells[this.colItemCode.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入交班项目代码!");
                return false;
            }
            commonDictItem.ItemCode = cellValue.ToString();

            cellValue = row.Cells[this.colItemName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入交班项目名称!");
                return false;
            }
            commonDictItem.ItemName = cellValue.ToString();

            cellValue = row.Cells[this.colWardName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                commonDictItem.WardName = "ALL";
            else
                commonDictItem.WardName = cellValue.ToString();

            cellValue = row.Cells[this.colWardName.Index].Tag;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                commonDictItem.WardCode = "全院";
            else
                commonDictItem.WardCode = cellValue.ToString();

            cellValue = row.Cells[this.colItemNo.Index].Value;
            commonDictItem.ItemNo = GlobalMethods.Convert.StringToValue(cellValue, 0);
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

            CommonDictItem commonDictItem = row.Tag as CommonDictItem;
            if (commonDictItem == null)
                return SystemConst.ReturnValue.FAILED;
            string szItemCode = commonDictItem.ItemCode;
            string szWardCode = commonDictItem.WardCode;

            commonDictItem = null;
            if (!this.MakeRowData(row, ref commonDictItem))
                return SystemConst.ReturnValue.FAILED;

            string szDictType = ServerData.CommonDictType.SHIFT_ITEM;
            short shRet = SystemConst.ReturnValue.OK;
            if (this.dataTableView1.IsDeletedRow(row))
            {
                if (!this.dataTableView1.IsNewRow(row))
                    shRet = CommonService.Instance.DeleteCommonDictItem(szDictType, szItemCode, szWardCode);
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
                shRet = CommonService.Instance.UpdateCommonDictItem(szDictType, szItemCode, szWardCode, commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("无法更新当前记录!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            else if (this.dataTableView1.IsNewRow(row))
            {
                shRet = CommonService.Instance.SaveCommonDictItem(commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("无法保存当前记录!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;
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
                this.SetRowData(row, row.Tag as CommonDictItem);
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
            CommonDictItem commonDictItem = null;
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                commonDictItem = currRow.Tag as CommonDictItem;
            if (commonDictItem == null)
                commonDictItem = new CommonDictItem();
            else
                commonDictItem = commonDictItem.Clone() as CommonDictItem;
            commonDictItem.ItemType = ServerData.CommonDictType.SHIFT_ITEM;
            commonDictItem.ItemCode = commonDictItem.MakeItemCode();

            int nRowIndex = this.dataTableView1.Rows.Count;
            if (this.dataTableView1.CurrentRow != null)
                nRowIndex = this.dataTableView1.CurrentRow.Index + 1;
            this.dataTableView1.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
            this.SetRowData(row, commonDictItem);

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