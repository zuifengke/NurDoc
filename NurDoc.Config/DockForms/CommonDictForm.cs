// ***********************************************************
// 护理病历配置管理系统,护理计划字典配置管理窗口.
// Author : YangMingkun, Date : 2013-6-4
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.Utilities.Dialogs;
using Heren.NurDoc.Config.Dialogs;

namespace Heren.NurDoc.Config.DockForms
{
    public partial class CommonDictForm : DockContentBase
    {
        /// <summary>
        /// 用于标记值改变事件是否可用
        /// </summary>
        private bool m_bValueChangedEventEnabled = true;

        public CommonDictForm(MainForm mainForm)
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
            this.btnAddItem.Image = Properties.Resources.Add;
        }

        public override void OnRefreshView()
        {
            base.OnRefreshView();
            this.Update();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDictGridViewData();

            string szDictType = null;
            if (this.dgvDicts.CurrentRow != null)
                szDictType = this.dgvDicts.CurrentRow.Tag as string;
            this.LoadItemGridViewData(szDictType);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 检查是否有未保存的数据
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            return this.dgvDicts.HasModified() || this.dgvItems.HasModified();
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
            while (index < this.dgvItems.Rows.Count)
            {
                DataTableViewRow row = this.dgvItems.Rows[index];
                bool bIsDeleteRow = this.dgvItems.IsDeletedRow(row);
                shRet = this.SaveItemRowData(row);
                if (shRet == SystemConst.ReturnValue.OK)
                    count++;
                else if (shRet == SystemConst.ReturnValue.FAILED)
                    break;
                if (!bIsDeleteRow) index++;
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
        /// 加载字典类型列表数据
        /// </summary>
        private void LoadDictGridViewData()
        {
            this.dgvDicts.Rows.Clear();

            List<string> lstDictTypeList = null;
            short shRot = CommonService.Instance.GetCommonDictTypeList(ref lstDictTypeList);
            if (shRot != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理诊断字典数据下载失败!");
                return;
            }
            if (lstDictTypeList == null || lstDictTypeList.Count <= 0)
                return;

            foreach (string szDictType in lstDictTypeList)
            {
                if (GlobalMethods.Misc.IsEmptyString(szDictType))
                    continue;
                int rowIndex = this.dgvDicts.Rows.Add();
                DataTableViewRow row = this.dgvDicts.Rows[rowIndex];
                row.Tag = szDictType;
                row.Cells[this.colDictName.Index].Value = szDictType;
                this.dgvDicts.SetRowState(row, RowState.Normal);
                this.colItemType.Items.Add(szDictType);
            }
        }

        /// <summary>
        /// 加载指定的字典类型包含的字典项目列表数据
        /// </summary>
        /// <param name="szDictType">字典类型</param>
        private void LoadItemGridViewData(string szDictType)
        {
            string szCurrentDictType = this.dgvItems.Tag as string;
            if (szDictType == szCurrentDictType)
                return;
            this.dgvItems.Rows.Clear();
            this.dgvItems.Tag = szDictType;

            List<CommonDictItem> lstCommonDictItems = null;
            short shRot = CommonService.Instance.GetCommonDict(szDictType, ref lstCommonDictItems);
            if (shRot != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理计划字典数据下载失败!");
                return;
            }
            if (lstCommonDictItems == null || lstCommonDictItems.Count <= 0)
                return;

            foreach (CommonDictItem commonDictItem in lstCommonDictItems)
            {
                if (commonDictItem == null)
                    continue;
                int rowIndex = this.dgvItems.Rows.Add();
                DataTableViewRow row = this.dgvItems.Rows[rowIndex];
                this.SetItemRowData(row, commonDictItem);
                this.dgvItems.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// 设置字典项目表格行数据
        /// </summary>
        /// <param name="row">字典项目表格行</param>
        /// <param name="commonDictItem">行数据</param>
        private void SetItemRowData(DataTableViewRow row, CommonDictItem commonDictItem)
        {
            if (row == null || row.Index < 0)
                return;
            row.Tag = commonDictItem;

            this.m_bValueChangedEventEnabled = false;
            if (commonDictItem == null)
            {
                row.Cells[this.colItemNo.Index].Value = null;
                row.Cells[this.colItemCode.Index].Value = null;
                row.Cells[this.colItemName.Index].Value = null;
                row.Cells[this.colItemType.Index].Value = null;
                row.Cells[this.colWardName.Index].Value = null;
                row.Cells[this.colWardName.Index].Tag = null;
            }
            else
            {
                row.Cells[this.colItemNo.Index].Value = commonDictItem.ItemNo;
                row.Cells[this.colItemCode.Index].Value = commonDictItem.ItemCode;
                row.Cells[this.colItemName.Index].Value = commonDictItem.ItemName;
                row.Cells[this.colItemType.Index].Value = commonDictItem.ItemType;
                row.Cells[this.colWardName.Index].Tag = commonDictItem.WardCode;
                row.Cells[this.colWardName.Index].Value = commonDictItem.WardName;
            }
            this.m_bValueChangedEventEnabled = true;
        }

        /// <summary>
        /// 获取字典项目表格行最新修改的数据
        /// </summary>
        /// <param name="row">字典项目表格行</param>
        /// <param name="commonDictItem">行数据</param>
        /// <returns>bool</returns>
        private bool MakeItemRowData(DataTableViewRow row, ref CommonDictItem commonDictItem)
        {
            if (row == null || row.Index < 0)
                return false;
            CommonDictItem old = row.Tag as CommonDictItem;
            if (old == null)
                return false;

            if (this.dgvItems.IsDeletedRow(row))
            {
                commonDictItem = old;
                return true;
            }
            commonDictItem = old.Clone() as CommonDictItem;

            object cellValue = row.Cells[this.colItemCode.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvItems.SelectRow(row);
                MessageBoxEx.Show("您必须输入字典项目代码!");
                return false;
            }
            commonDictItem.ItemCode = cellValue.ToString();

            cellValue = row.Cells[this.colItemName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvItems.SelectRow(row);
                MessageBoxEx.Show("您必须输入字典项目内容!");
                return false;
            }
            commonDictItem.ItemName = cellValue.ToString();

            cellValue = row.Cells[this.colItemType.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvItems.SelectRow(row);
                MessageBoxEx.Show("您必须输入字典项目类型!");
                return false;
            }
            commonDictItem.ItemType = cellValue.ToString().ToUpper();

            cellValue = row.Cells[this.colWardName.Index].Tag;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                commonDictItem.WardCode = "ALL";
            else
                commonDictItem.WardCode = cellValue.ToString();

            cellValue = row.Cells[this.colWardName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                commonDictItem.WardName = "全院";
            else
                commonDictItem.WardName = cellValue.ToString();

            cellValue = row.Cells[this.colItemNo.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
                commonDictItem.ItemNo = GlobalMethods.Convert.StringToValue(cellValue.ToString(), 0);
            return true;
        }

        /// <summary>
        /// 保存字典项目表格行数据
        /// </summary>
        /// <param name="row">字典项目表格行</param>
        /// <returns>SystemConst.ReturnValue</returns>
        private short SaveItemRowData(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return SystemConst.ReturnValue.FAILED;

            if (this.dgvItems.IsNormalRow(row) || this.dgvItems.IsUnknownRow(row))
            {
                if (!this.dgvItems.IsDeletedRow(row))
                    return SystemConst.ReturnValue.CANCEL;
            }

            string szOldItemType = string.Empty;
            string szOldItemCode = string.Empty;
            string szOldWardCode = string.Empty;
            CommonDictItem commonDictItem = row.Tag as CommonDictItem;
            if (commonDictItem == null)
                return SystemConst.ReturnValue.FAILED;
            szOldItemType = commonDictItem.ItemType;
            szOldItemCode = commonDictItem.ItemCode;
            szOldWardCode = commonDictItem.WardCode;

            if (!this.MakeItemRowData(row, ref commonDictItem))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dgvItems.IsDeletedRow(row))
            {
                if (!this.dgvItems.IsNewRow(row))
                    shRet = CommonService.Instance.DeleteCommonDictItem(szOldItemType, szOldItemCode, szOldWardCode);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvItems.SelectRow(row);
                    MessageBoxEx.Show("字典项目信息删除失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                this.dgvItems.Rows.Remove(row);
            }
            else if (this.dgvItems.IsModifiedRow(row))
            {
                shRet = CommonService.Instance.UpdateCommonDictItem(szOldItemType, szOldItemCode, szOldWardCode, commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvDicts.SelectRow(row);
                    MessageBoxEx.Show("字典项目信息更新失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;
                this.dgvDicts.SetRowState(row, RowState.Normal);
            }
            else if (this.dgvItems.IsNewRow(row))
            {
                shRet = CommonService.Instance.SaveCommonDictItem(commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvItems.SelectRow(row);
                    MessageBoxEx.Show("字典项目信息保存失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;
                this.dgvItems.SetRowState(row, RowState.Normal);
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

            if (deptSelectDialog.DeptInfos != null && deptSelectDialog.DeptInfos.Length > 0)
                deptInfo = deptSelectDialog.DeptInfos[0];
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
            if (this.dgvItems.IsNormalRowUndeleted(row))
                this.dgvItems.SetRowState(row, RowState.Update);
        }

        /// <summary>
        /// 显示文本编辑对话框
        /// </summary>
        /// <param name="row">指定行</param>
        private void ShowConfigValueEditForm(DataTableViewRow row)
        {
            if (row == null || row.Index < 0 || this.dgvItems.IsDeletedRow(row))
                return;
            if (this.dgvItems.EditingControl != null)
                this.dgvItems.EndEdit();
            LargeTextEditForm largeTextEditForm = new LargeTextEditForm();
            largeTextEditForm.Text = "编辑项目数据";
            DataGridViewCell cell = row.Cells[this.colItemName.Index];
            if (cell.Value != null)
                largeTextEditForm.LargeText = cell.Value.ToString();
            if (largeTextEditForm.ShowDialog() != DialogResult.OK)
                return;
            string szConfigValue = largeTextEditForm.LargeText;
            if (szConfigValue.Equals(cell.Value))
                return;
            cell.Value = szConfigValue;
            if (this.dgvItems.IsNormalRowUndeleted(row))
                this.dgvItems.SetRowState(row, RowState.Update);
        }

        private void mnuDeleteItem_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvItems.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvItems.SelectedRows[index];
                this.dgvItems.SetRowState(row, RowState.Delete);
            }
        }

        private void mnuCancelModifyItem_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvItems.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvItems.SelectedRows[index];
                if (!this.dgvItems.IsModifiedRow(row))
                    continue;
                this.SetItemRowData(row, row.Tag as CommonDictItem);
                this.dgvItems.SetRowState(row, RowState.Normal);
            }
        }

        private void mnuCancelDeleteItem_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvItems.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvItems.SelectedRows[index];
                if (!this.dgvItems.IsDeletedRow(row))
                    continue;
                if (this.dgvItems.IsNewRow(row))
                    this.dgvItems.SetRowState(row, RowState.New);
                else if (this.dgvItems.IsModifiedRow(row))
                    this.dgvItems.SetRowState(row, RowState.Update);
                else
                    this.dgvItems.SetRowState(row, RowState.Normal);
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            CommonDictItem commonDictItem = null;
            DataTableViewRow currRow = this.dgvItems.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                commonDictItem = currRow.Tag as CommonDictItem;
            if (commonDictItem == null)
                commonDictItem = new CommonDictItem();
            else
                commonDictItem = commonDictItem.Clone() as CommonDictItem;

            string szDictType = this.dgvItems.Tag as string;
            if (string.IsNullOrEmpty(commonDictItem.ItemType))
                commonDictItem.ItemType = szDictType;

            commonDictItem.ItemCode = commonDictItem.MakeItemCode();

            int nRowIndex = this.dgvItems.Rows.Count;
            if (this.dgvItems.CurrentRow != null)
                nRowIndex = this.dgvItems.CurrentRow.Index + 1;
            this.dgvItems.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dgvItems.Rows[nRowIndex];
            this.SetItemRowData(row, commonDictItem);

            this.dgvItems.Focus();
            this.dgvItems.SelectRow(row);
            this.dgvItems.SetRowState(row, RowState.New);
            if (currRow != null && currRow.Index >= 0)
                row.DefaultCellStyle.BackColor = currRow.DefaultCellStyle.BackColor;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.CommitModify();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvDicts_SelectionChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.dgvDicts.CurrentRow != null)
                this.LoadItemGridViewData(this.dgvDicts.CurrentRow.Tag as string);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            DataTableViewRow row = this.dgvItems.Rows[e.RowIndex];
            if (row == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (e.ColumnIndex == this.colWardName.Index)
                this.ShowDeptSelectForm(row);
            if (e.ColumnIndex == this.colItemName.Index)
                this.ShowConfigValueEditForm(row);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.m_bValueChangedEventEnabled)
                return;
            if (e.ColumnIndex != this.colItemType.Index)
                return;
            DataTableViewRow currRow = this.dgvItems.Rows[e.RowIndex];
            if (currRow == null)
                return;
            CommonDictItem commonDictItem = new CommonDictItem();
            if (currRow.Cells[this.colItemType.Index].Value != null)
                commonDictItem.ItemType = currRow.Cells[this.colItemType.Index].Value.ToString();
            currRow.Cells[this.colItemCode.Index].Value = commonDictItem.MakeItemCode();
            if (this.dgvItems.IsNormalRowUndeleted(currRow))
                this.dgvItems.SetRowState(currRow, RowState.Update);
        }
    }
}