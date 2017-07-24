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
using Heren.NurDoc.Config.Dialogs;

namespace Heren.NurDoc.Config.DockForms
{
    public partial class CarePlanEditForm : DockContentBase
    {
        /// <summary>
        /// 用于标记值改变事件是否可用
        /// </summary>
        private bool m_bValueChangedEventEnabled = true;

        public CarePlanEditForm(MainForm mainForm)
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
            this.btnAddDiag.Image = Properties.Resources.Add;
            this.btnAddItem.Image = Properties.Resources.Add;
            this.colItemType.Items.AddRange(ServerData.CommonDictType.GetNCPItemType());
        }

        public override void OnRefreshView()
        {
            base.OnRefreshView();
            this.Update();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDiagGridViewData();

            CommonDictItem commonDictItem = null;
            if (this.dgvDiag.CurrentRow != null)
                commonDictItem = this.dgvDiag.CurrentRow.Tag as CommonDictItem;
            this.LoadItemGridViewData(commonDictItem);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 检查是否有未保存的数据
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            return this.dgvDiag.HasModified() || this.dgvItems.HasModified();
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

            while (index < this.dgvDiag.Rows.Count)
            {
                DataTableViewRow row = this.dgvDiag.Rows[index];
                bool bIsDeleteRow = this.dgvDiag.IsDeletedRow(row);
                shRet = this.SaveDiagRowData(row);
                if (shRet == SystemConst.ReturnValue.OK)
                    count++;
                else if (shRet == SystemConst.ReturnValue.FAILED)
                    break;
                if (!bIsDeleteRow) index++;
            }

            index = 0;
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

            this.RefreshItemsGroupBoxText();

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
        /// 加载护理诊断列表数据
        /// </summary>
        private void LoadDiagGridViewData()
        {
            this.dgvDiag.Rows.Clear();

            string szItemType = ServerData.CommonDictType.NUR_DIAGNOSIS;
            List<CommonDictItem> lstCommonDictItems = null;
            short shRot = CommonService.Instance.GetCommonDict(szItemType, ref lstCommonDictItems);
            if (shRot != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理诊断字典数据下载失败!");
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
                int rowIndex = this.dgvDiag.Rows.Add();
                DataTableViewRow row = this.dgvDiag.Rows[rowIndex];
                this.SetDiagRowData(row, commonDictItem);
                this.dgvDiag.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// 加载组包含的用户列表数据
        /// </summary>
        /// <param name="diagItem">组对象</param>
        private void LoadItemGridViewData(CommonDictItem diagItem)
        {
            CommonDictItem currentItem = this.dgvItems.Tag as CommonDictItem;
            if (currentItem != null && diagItem != null
                && currentItem.ItemCode == diagItem.ItemCode)
            {
                return;
            }
            this.dgvItems.Rows.Clear();
            this.dgvItems.Tag = diagItem;
            this.RefreshItemsGroupBoxText();
            if (diagItem == null)
                return;

            List<NurCarePlanDictInfo> lstNurCarePlanDictInfos = null;
            short shRot = CarePlanService.Instance.GetNurCarePlanDictInfo(diagItem.ItemCode, ref lstNurCarePlanDictInfos);
            if (shRot != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理计划字典数据下载失败!");
                return;
            }
            if (lstNurCarePlanDictInfos == null || lstNurCarePlanDictInfos.Count <= 0)
                return;

            NurCarePlanDictInfo prevNcpDictInfo = null;
            Color prevColor = Color.White;
            foreach (NurCarePlanDictInfo ncpDictInfo in lstNurCarePlanDictInfos)
            {
                if (ncpDictInfo == null)
                    continue;
                int rowIndex = this.dgvItems.Rows.Add();
                DataTableViewRow row = this.dgvItems.Rows[rowIndex];
                this.SetItemRowData(row, ncpDictInfo);
                this.dgvItems.SetRowState(row, RowState.Normal);

                if (prevNcpDictInfo != null
                    && ncpDictInfo.Item.ItemType != prevNcpDictInfo.Item.ItemType)
                {
                    prevColor = (prevColor == Color.White) ? Color.LightGray : Color.White;
                }
                prevNcpDictInfo = ncpDictInfo;
                row.DefaultCellStyle.BackColor = prevColor;
            }
        }

        /// <summary>
        /// 设置护理诊断表格行数据
        /// </summary>
        /// <param name="row">护理诊断表格行</param>
        /// <param name="diagItem">行数据</param>
        private void SetDiagRowData(DataTableViewRow row, CommonDictItem diagItem)
        {
            if (row == null || row.Index < 0)
                return;
            row.Tag = diagItem;

            this.m_bValueChangedEventEnabled = false;
            if (diagItem == null)
            {
                row.Cells[this.colDiagNo.Index].Value = null;
                row.Cells[this.colDiagCode.Index].Value = null;
                row.Cells[this.colDiagName.Index].Value = null;
            }
            else
            {
                row.Cells[this.colDiagNo.Index].Value = diagItem.ItemNo;
                row.Cells[this.colDiagCode.Index].Value = diagItem.ItemCode;
                row.Cells[this.colDiagName.Index].Value = diagItem.ItemName;
            }
            this.m_bValueChangedEventEnabled = true;
        }

        /// <summary>
        /// 设置护理措施项目表格行数据
        /// </summary>
        /// <param name="row">护理措施项目表格行</param>
        /// <param name="ncpDictInfo">行数据</param>
        private void SetItemRowData(DataTableViewRow row, NurCarePlanDictInfo ncpDictInfo)
        {
            if (row == null || row.Index < 0)
                return;
            row.Tag = ncpDictInfo;

            this.m_bValueChangedEventEnabled = false;
            if (ncpDictInfo == null)
            {
                row.Cells[this.colItemNo.Index].Value = null;
                row.Cells[this.colItemCode.Index].Value = null;
                row.Cells[this.colItemName.Index].Value = null;
                row.Cells[this.colItemType.Index].Value = null;
            }
            else
            {
                row.Cells[this.colItemNo.Index].Value = ncpDictInfo.Item.ItemNo;
                row.Cells[this.colItemCode.Index].Value = ncpDictInfo.Item.ItemCode;
                row.Cells[this.colItemName.Index].Value = ncpDictInfo.Item.ItemName;
                row.Cells[this.colItemType.Index].Value =
                    ServerData.CommonDictType.GetItemTypeByCode(ncpDictInfo.Item.ItemType);
            }
            this.m_bValueChangedEventEnabled = true;
        }

        /// <summary>
        /// 根据当前选择的诊断信息刷新GroupBox标题
        /// </summary>
        private void RefreshItemsGroupBoxText()
        {
            string szItemName = string.Empty;
            DataGridViewRow currentRow = this.dgvDiag.CurrentRow;
            if (currentRow != null)
                szItemName = currentRow.Cells[this.colDiagName.Index].Value as string;
            this.gbxItems.Text = string.Format("{0}诊断护理计划", szItemName);
        }

        /// <summary>
        /// 获取护理诊断表格指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="ncpDiag">最新修改后的数据</param>
        /// <returns>bool</returns>
        private bool MakeDiagRowData(DataTableViewRow row, ref CommonDictItem ncpDiag)
        {
            if (row == null || row.Index < 0)
                return false;
            CommonDictItem old = row.Tag as CommonDictItem;
            if (old == null)
                return false;

            if (this.dgvDiag.IsDeletedRow(row))
            {
                ncpDiag = old;
                return true;
            }
            ncpDiag = old.Clone() as CommonDictItem;

            object cellValue = row.Cells[this.colDiagCode.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvDiag.SelectRow(row);
                MessageBoxEx.Show("您必须输入护理诊断代码!");
                return false;
            }
            ncpDiag.ItemCode = cellValue.ToString();

            cellValue = row.Cells[this.colDiagName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvDiag.SelectRow(row);
                MessageBoxEx.Show("您必须输入护理诊断名称!");
                return false;
            }
            ncpDiag.ItemName = cellValue.ToString();

            ncpDiag.ItemType = ServerData.CommonDictType.NUR_DIAGNOSIS;

            cellValue = row.Cells[this.colDiagNo.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
                ncpDiag.ItemNo = GlobalMethods.Convert.StringToValue(cellValue.ToString(), 0);
            return true;
        }

        /// <summary>
        /// 获取护理措施项目表格指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="ncpDictInfo">最新修改后的数据</param>
        /// <returns>bool</returns>
        private bool MakeItemRowData(DataTableViewRow row, ref NurCarePlanDictInfo ncpDictInfo)
        {
            if (row == null || row.Index < 0)
                return false;
            NurCarePlanDictInfo old = row.Tag as NurCarePlanDictInfo;
            if (old == null)
                return false;
            if (this.dgvItems.IsDeletedRow(row))
            {
                ncpDictInfo = old;
                return true;
            }

            DataGridViewRow currentRow = this.dgvDiag.CurrentRow;
            CommonDictItem ncpDiag = null;
            if (currentRow != null)
                ncpDiag = currentRow.Tag as CommonDictItem;
            if (ncpDiag == null)
            {
                this.dgvItems.SelectRow(row);
                MessageBoxEx.Show("您必须选择一个护理诊断!");
                return false;
            }

            ncpDictInfo = old.Clone() as NurCarePlanDictInfo;
            ncpDictInfo.DiagCode = ncpDiag.ItemCode;

            object cellValue = row.Cells[this.colItemCode.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvItems.SelectRow(row);
                MessageBoxEx.Show("您必须输入护理计划项目代码!");
                return false;
            }
            ncpDictInfo.Item.ItemCode = cellValue.ToString();

            cellValue = row.Cells[this.colItemName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvItems.SelectRow(row);
                MessageBoxEx.Show("您必须输入护理计划项目内容!");
                return false;
            }
            ncpDictInfo.Item.ItemName = cellValue.ToString();

            cellValue = row.Cells[this.colItemType.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dgvItems.SelectRow(row);
                MessageBoxEx.Show("您必须输入护理计划项目类型!");
                return false;
            }
            ncpDictInfo.Item.ItemType =
                ServerData.CommonDictType.GetItemTypeByName(cellValue.ToString());

            cellValue = row.Cells[this.colItemNo.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
                ncpDictInfo.Item.ItemNo = GlobalMethods.Convert.StringToValue(cellValue.ToString(), 0);
            return true;
        }

        /// <summary>
        /// 保存指定的护理诊断表格行数据
        /// </summary>
        /// <param name="row">护理诊断表格行</param>
        /// <returns>SystemConst.ReturnValue</returns>
        private short SaveDiagRowData(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return SystemConst.ReturnValue.FAILED;

            if (this.dgvDiag.IsNormalRow(row) || this.dgvDiag.IsUnknownRow(row))
            {
                if (!this.dgvDiag.IsDeletedRow(row))
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
            if (!this.MakeDiagRowData(row, ref commonDictItem))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dgvDiag.IsDeletedRow(row))
            {
                if (!this.dgvDiag.IsNewRow(row))
                    shRet = CarePlanService.Instance.DeleteNurCarePlanDictInfo(szOldItemCode);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvDiag.SelectRow(row);
                    MessageBoxEx.Show("护理计划信息删除失败!");
                    return SystemConst.ReturnValue.FAILED;
                }

                CommonDictItem currentItem = this.dgvItems.Tag as CommonDictItem;
                if (currentItem != null && szOldItemCode == currentItem.ItemCode)
                    this.LoadItemGridViewData(null);
                this.dgvDiag.Rows.Remove(row);
            }
            else if (this.dgvDiag.IsModifiedRow(row))
            {
                shRet = CommonService.Instance.UpdateCommonDictItem(szOldItemType, szOldItemCode, commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvDiag.SelectRow(row);
                    MessageBoxEx.Show("护理计划信息更新失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;

                CommonDictItem currentItem = this.dgvItems.Tag as CommonDictItem;
                if (currentItem != null && szOldItemCode == currentItem.ItemCode)
                    this.LoadItemGridViewData(commonDictItem);
                this.dgvDiag.SetRowState(row, RowState.Normal);
            }
            else if (this.dgvDiag.IsNewRow(row))
            {
                shRet = CommonService.Instance.SaveCommonDictItem(commonDictItem);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvDiag.SelectRow(row);
                    MessageBoxEx.Show("护理计划信息保存失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = commonDictItem;
                this.dgvDiag.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的护理措施项目表格行数据
        /// </summary>
        /// <param name="row">护理措施项目表格行</param>
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
            NurCarePlanDictInfo ncpDictInfo = row.Tag as NurCarePlanDictInfo;
            if (ncpDictInfo == null)
                return SystemConst.ReturnValue.FAILED;
            szOldItemType = ncpDictInfo.Item.ItemType;
            szOldItemCode = ncpDictInfo.Item.ItemCode;

            if (!this.MakeItemRowData(row, ref ncpDictInfo))
                return SystemConst.ReturnValue.FAILED;
            string szDiagCode = ncpDictInfo.DiagCode;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dgvItems.IsDeletedRow(row))
            {
                if (!this.dgvItems.IsNewRow(row))
                    shRet = CarePlanService.Instance.DeleteNurCarePlanDictInfo(szDiagCode, szOldItemCode, szOldItemType);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvItems.SelectRow(row);
                    MessageBoxEx.Show("组用户信息删除失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                this.dgvItems.Rows.Remove(row);
            }
            else if (this.dgvItems.IsModifiedRow(row))
            {
                shRet = CarePlanService.Instance.UpdateNurCarePlanDictInfo(szDiagCode, szOldItemCode, szOldItemType, ncpDictInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvDiag.SelectRow(row);
                    MessageBoxEx.Show("护理计划信息更新失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = ncpDictInfo;
                this.dgvDiag.SetRowState(row, RowState.Normal);
            }
            else if (this.dgvItems.IsNewRow(row))
            {
                shRet = CarePlanService.Instance.SaveNurCarePlanDictInfo(ncpDictInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dgvItems.SelectRow(row);
                    MessageBoxEx.Show("组用户信息保存失败!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = ncpDictInfo;
                this.dgvItems.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        private void mnuDeleteDiag_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvDiag.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvDiag.SelectedRows[index];
                this.dgvDiag.SetRowState(row, RowState.Delete);
            }
        }

        private void mnnCancelModifyDiag_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvDiag.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvDiag.SelectedRows[index];
                if (!this.dgvDiag.IsModifiedRow(row))
                    continue;
                this.SetDiagRowData(row, row.Tag as CommonDictItem);
                this.dgvDiag.SetRowState(row, RowState.Normal);
            }
        }

        private void mnuCancelDeleteDiag_Click(object sender, EventArgs e)
        {
            int nCount = this.dgvDiag.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dgvDiag.SelectedRows[index];
                if (!this.dgvDiag.IsDeletedRow(row))
                    continue;
                if (this.dgvDiag.IsNewRow(row))
                    this.dgvDiag.SetRowState(row, RowState.New);
                else if (this.dgvDiag.IsModifiedRow(row))
                    this.dgvDiag.SetRowState(row, RowState.Update);
                else
                    this.dgvDiag.SetRowState(row, RowState.Normal);
            }
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
                this.SetItemRowData(row, row.Tag as NurCarePlanDictInfo);
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

        private void btnAddDiag_Click(object sender, EventArgs e)
        {
            CommonDictItem commonDictItem = null;
            DataTableViewRow currRow = this.dgvDiag.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                commonDictItem = currRow.Tag as CommonDictItem;
            if (commonDictItem == null)
                commonDictItem = new CommonDictItem();
            else
                commonDictItem = commonDictItem.Clone() as CommonDictItem;
            commonDictItem.ItemType = ServerData.CommonDictType.NUR_DIAGNOSIS;
            commonDictItem.ItemCode = commonDictItem.MakeItemCode();

            int nRowIndex = this.dgvDiag.Rows.Count;
            if (this.dgvDiag.CurrentRow != null)
                nRowIndex = this.dgvDiag.CurrentRow.Index + 1;
            this.dgvDiag.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dgvDiag.Rows[nRowIndex];
            this.SetDiagRowData(row, commonDictItem);

            this.dgvDiag.Focus();
            this.dgvDiag.SelectRow(row);
            this.dgvDiag.SetRowState(row, RowState.New);
            this.LoadItemGridViewData(commonDictItem);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            CommonDictItem commonDictItem = null;
            if (this.dgvDiag.CurrentRow != null)
                commonDictItem = this.dgvDiag.CurrentRow.Tag as CommonDictItem;
            if (commonDictItem == null
                || GlobalMethods.Misc.IsEmptyString(commonDictItem.ItemCode))
            {
                MessageBoxEx.Show("请选择一个用户组后,再向其添加用户!");
                return;
            }

            NurCarePlanDictInfo ncpDictInfo = null;
            DataTableViewRow currRow = this.dgvItems.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                ncpDictInfo = currRow.Tag as NurCarePlanDictInfo;
            if (ncpDictInfo == null)
                ncpDictInfo = new NurCarePlanDictInfo();
            else
                ncpDictInfo = ncpDictInfo.Clone() as NurCarePlanDictInfo;
            ncpDictInfo.DiagCode = commonDictItem.ItemCode;
            if (string.IsNullOrEmpty(ncpDictInfo.Item.ItemType))
                ncpDictInfo.Item.ItemType = ServerData.CommonDictType.NCP_FACTOR;
            ncpDictInfo.Item.ItemCode = ncpDictInfo.Item.MakeItemCode();

            int nRowIndex = this.dgvItems.Rows.Count;
            if (this.dgvItems.CurrentRow != null)
                nRowIndex = this.dgvItems.CurrentRow.Index + 1;
            this.dgvItems.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dgvItems.Rows[nRowIndex];
            this.SetItemRowData(row, ncpDictInfo);

            this.dgvItems.Focus();
            this.dgvItems.SelectRow(row);
            this.dgvItems.SetRowState(row, RowState.New);
            if (currRow != null && currRow.Index >= 0)
                row.DefaultCellStyle.BackColor = currRow.DefaultCellStyle.BackColor;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.CommitModify();
        }

        private void dgvDiags_SelectionChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.dgvDiag.CurrentRow != null)
                this.LoadItemGridViewData(this.dgvDiag.CurrentRow.Tag as CommonDictItem);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvDiags_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadItemGridViewData(this.dgvDiag.Rows[e.RowIndex].Tag as CommonDictItem);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvDiags_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.RefreshItemsGroupBoxText();

            //当修改诊断编码时,所有字典项目全部都需更新
            if (e.ColumnIndex == this.colDiagCode.Index)
            {
                foreach (DataTableViewRow row in this.dgvItems.Rows)
                {
                    if (this.dgvItems.IsNormalRowUndeleted(row))
                        this.dgvItems.SetRowState(row, RowState.Update);
                }
            }
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
            string szItemType = string.Empty;
            if (currRow.Cells[this.colItemType.Index].Value != null)
                szItemType = currRow.Cells[this.colItemType.Index].Value.ToString();
            NurCarePlanDictInfo ncpDictInfo = new NurCarePlanDictInfo();
            ncpDictInfo.Item.ItemType = ServerData.CommonDictType.GetItemTypeByName(szItemType);
            currRow.Cells[this.colItemCode.Index].Value = ncpDictInfo.Item.MakeItemCode();
            if (this.dgvItems.IsNormalRowUndeleted(currRow))
                this.dgvItems.SetRowState(currRow, RowState.Update);
        }
    }
}