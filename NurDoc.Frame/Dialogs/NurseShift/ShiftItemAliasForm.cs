// ***********************************************************
// 护理电子病历系统,交接班项目别名新增、修改、删除窗口.
// Creator:YangMingkun  Date:2013-7-12
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Frame.DockForms;


namespace Heren.NurDoc.Frame.DockForms
{
    public partial class ShiftItemAliasForm : Form
    {
        private CommonDictItem[] m_wardShiftItemList = null;
        private string m_WardName = null;
        private string m_WardCode = null;

        public ShiftItemAliasForm()
        {
            InitializeComponent();

            this.btnAddItemAlias.Image = Properties.Resources.Add;
            this.btnDeleteItemAlias.Image = Properties.Resources.Delete;
            this.btnSave.Image = Properties.Resources.Save;
            this.Icon = Properties.Resources.ShiftIcon;
            this.m_WardName = SystemContext.Instance.LoginUser.WardName;
            this.m_WardCode = SystemContext.Instance.LoginUser.WardCode;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadShiftItemList();
            this.LoadShiftItemAliasList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载表格中的交班项目下拉列表
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadShiftItemList()
        {
            this.colShiftItemName.Items.Clear();
            this.m_wardShiftItemList = SystemContext.Instance.GetWardShiftItemList();
            this.colShiftItemName.Items.AddRange(this.m_wardShiftItemList);
            return true;
        }

        /// <summary>
        /// 加载交班项目配置别名列表
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadShiftItemAliasList()
        {
            this.dgvSetShiftItemAlias.Rows.Clear();
            List<ShiftItemAliasInfo> lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            short shRet = NurShiftService.Instance.GetShiftItemAliasInfos(this.m_WardCode, ref lstShiftItemAliasInfos);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                foreach (ShiftItemAliasInfo shiftItemAliasInfo in lstShiftItemAliasInfos)
                {
                    int rowIndex = this.dgvSetShiftItemAlias.Rows.Add();
                    DataTableViewRow row = this.dgvSetShiftItemAlias.Rows[rowIndex];
                    row.Tag = shiftItemAliasInfo;
                    row.Cells[this.colShiftItemCode.Index].Value = shiftItemAliasInfo.ItemCode;
                    row.Cells[this.colShiftItemName.Index].Value = shiftItemAliasInfo.ItemName;
                    row.Cells[this.colShiftItemAlias.Index].Value = shiftItemAliasInfo.ItemAlias;
                    row.Cells[this.colShiftItemAliasCode.Index].Value = shiftItemAliasInfo.ItemAliasCode;
                    row.Cells[this.colWardName.Index].Value = shiftItemAliasInfo.WardName;
                    this.dgvSetShiftItemAlias.SetRowState(row, RowState.Normal);
                }
                return true;
            }
            else
                return false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = this.CheckModifyState();
        }       

        /// <summary>
        /// 检查并保存当前数据编辑窗口的修改
        /// </summary>
        /// <returns>是否取消</returns>
        private bool CheckModifyState()
        {
            if (!this.HasUncommit())
                return false;

            DialogResult result = MessageBoxEx.ShowQuestion("当前交班项目配置别名数据已修改,是否保存？");
            if (result == DialogResult.Yes)
                return !this.SaveShiftItemAliasData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// 检查当前数据编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否有未提交</returns>
        private bool HasUncommit()
        {
            foreach (DataTableViewRow row in this.dgvSetShiftItemAlias.Rows)
            {
                if (row.IsModifiedRow || row.IsNewRow)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 删除表中当前选中的交班项目
        /// </summary>
        /// <returns>是否成功</returns>
        private bool DeleteSelectedShiftItem()
        {
            DataTableViewRow row = this.dgvSetShiftItemAlias.CurrentRow;

            if (row == null)
                return false;
            if (!RightController.Instance.CanEditItemAlias(this.m_WardCode))
                return false;
            DialogResult result = MessageBoxEx.ShowConfirm("确认删除当前选中的项目吗？");
            if (result != DialogResult.OK)
                return false;
            if (row.State == RowState.New)
            {
                this.dgvSetShiftItemAlias.Rows.Remove(row);
                return true;
            }
            ShiftItemAliasInfo shiftItemAliasInfo = row.Tag as ShiftItemAliasInfo;
            if (shiftItemAliasInfo == null)
                return false;
            string szItemAliasCode = shiftItemAliasInfo.ItemAliasCode;
            
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            short shRet = NurShiftService.Instance.DeleteShiftItemAlias(szItemAliasCode, this.m_WardCode);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                this.dgvSetShiftItemAlias.Rows.Remove(row);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }
            else
            {
                MessageBoxEx.Show("删除交班项目配置别名记录失败!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
        }

        /// <summary>
        /// 保存当前已修改的交班项目配置数据
        /// </summary>
        /// <returns>是否成功</returns>
        private bool SaveShiftItemAliasData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (!RightController.Instance.CanEditItemAlias(this.m_WardCode))
                return false;
            short shRet = SystemConst.ReturnValue.OK;
            dgvSetShiftItemAlias.EndEdit();
            foreach (DataTableViewRow row in this.dgvSetShiftItemAlias.Rows)
            {
                if (Convert.ToString(row.Cells[this.colShiftItemName.Index].Value) == null || Convert.ToString(row.Cells[this.colShiftItemName.Index].Value).Equals(""))
                {
                    MessageBoxEx.Show("项目不能为空！");
                    return false;
                }
                if (Convert.ToString(row.Cells[this.colShiftItemAlias.Index].Value) == null || Convert.ToString(row.Cells[this.colShiftItemAlias.Index].Value).Equals(""))
                {
                    MessageBoxEx.Show("项目别名不能为空！");
                    return false;
                }
                ShiftItemAliasInfo shiftItemAliasInfo = new ShiftItemAliasInfo();
                shiftItemAliasInfo.ItemName = Convert.ToString(row.Cells[this.colShiftItemName.Index].Value);
                shiftItemAliasInfo.ItemCode = Convert.ToString(row.Cells[this.colShiftItemCode.Index].Value);
                shiftItemAliasInfo.ItemAlias = Convert.ToString(row.Cells[this.colShiftItemAlias.Index].Value);
                shiftItemAliasInfo.ItemAliasCode = Convert.ToString(row.Cells[this.colShiftItemAliasCode.Index].Value);
                shiftItemAliasInfo.WardName = this.m_WardName;
                shiftItemAliasInfo.WardCode = this.m_WardCode;
                if (shiftItemAliasInfo == null)
                    continue;
                shRet = NurShiftService.Instance.SaveShiftItemAlias(shiftItemAliasInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班项目配置信息失败!");
                    return false;
                }

                this.dgvSetShiftItemAlias.SetRowState(row, RowState.Normal);
            }
            //保存交班项目配置列表
            foreach (DataTableViewRow row in this.dgvSetShiftItemAlias.Rows)
            {
                if (this.dgvSetShiftItemAlias.IsNormalRow(row))
                    continue;

                ShiftItemAliasInfo shiftItemAliasInfo = new ShiftItemAliasInfo();
                shiftItemAliasInfo.ItemName = Convert.ToString(row.Cells[this.colShiftItemName.Index].Value);
                shiftItemAliasInfo.ItemCode = Convert.ToString(row.Cells[this.colShiftItemCode.Index].Value);
                shiftItemAliasInfo.ItemAlias = Convert.ToString(row.Cells[this.colShiftItemAlias.Index].Value);
                shiftItemAliasInfo.ItemAliasCode = Convert.ToString(row.Cells[this.colShiftItemAliasCode.Index].Value);
                shiftItemAliasInfo.WardName = this.m_WardName;
                shiftItemAliasInfo.WardCode = this.m_WardCode;
                if (shiftItemAliasInfo == null)
                    continue;
                shRet = NurShiftService.Instance.SaveShiftItemAlias(shiftItemAliasInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班项目配置信息失败!");
                    return false;
                }

                this.dgvSetShiftItemAlias.SetRowState(row, RowState.Normal);
            }
            return true;
        }

        
        private void btnAddItemAlias_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftItemAliasInfo shiftItemAliasInfo = new ShiftItemAliasInfo();
            this.dgvSetShiftItemAlias.Rows.Add();
            int rowsCount = this.dgvSetShiftItemAlias.Rows.Count - 1;
            this.dgvSetShiftItemAlias.Rows[rowsCount].Cells[this.colWardName.Index].Value = this.m_WardName;
            this.dgvSetShiftItemAlias.Rows[rowsCount].Cells[this.colShiftItemAliasCode.Index].Value = shiftItemAliasInfo.MakeAliasCode();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnDeleteItemAlias_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedShiftItem();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveShiftItemAliasData())
            {
                MessageBox.Show("保存成功！");
                this.DialogResult = DialogResult.OK;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private ComboBox m_ComboBox = null;
        private void dgvSetShiftItemAlias_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dgvSetShiftItemAlias.CurrentCell.RowIndex >= 0 || this.dgvSetShiftItemAlias.CurrentCell.ColumnIndex == this.colShiftItemName.Index)
            {
                m_ComboBox = e.Control as ComboBox;
                if (this.m_ComboBox == null)
                    return;
                if (!Convert.ToString(this.dgvSetShiftItemAlias.CurrentRow.Cells[this.colShiftItemName.Index].Value).Equals(this.m_ComboBox.SelectedItem))
                {
                    //每次注册事件的时候先移除事件，避免不断被递归调用 
                    m_ComboBox.SelectedIndexChanged -= new EventHandler(dgvSetShiftItemAliasCbo_SelectedIndexChanged);
                    m_ComboBox.SelectedIndexChanged += new EventHandler(dgvSetShiftItemAliasCbo_SelectedIndexChanged);
                }
            }
        }

        private void dgvSetShiftItemAliasCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (CommonDictItem commonDictItem in this.m_wardShiftItemList)
            {
                if (this.m_ComboBox.Text.Equals(commonDictItem.ItemName))
                {
                    this.dgvSetShiftItemAlias.CurrentRow.Cells[this.colShiftItemCode.Index].Value = commonDictItem.ItemCode;
                    break;
                }
            }
        }
    }
}