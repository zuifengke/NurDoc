// ***********************************************************
// 护理记录单表格的列的列表方案配置模块中,列列表管理窗口.
// Author : YangMingkun, Date : 2012-7-3
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Config.Templet;

namespace Heren.NurDoc.Config.Dialogs
{
    public partial class ColumnManageForm : HerenForm
    {
        private string m_szSchemaID = null;

        /// <summary>
        /// 获取或设置方案编号
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        private List<GridViewColumn> m_lstSchemaColumns = null;

        /// <summary>
        /// 获取或设置方案包含的列列表
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public List<GridViewColumn> SchemaColumns
        {
            get { return this.m_lstSchemaColumns; }
            set { this.m_lstSchemaColumns = value; }
        }

        public ColumnManageForm()
        {
            this.InitializeComponent();
            //
            this.colColumnUnit.Items.Add("℃");
            this.colColumnUnit.Items.Add("mmHg");
            this.colColumnUnit.Items.Add("ml");
            this.colColumnUnit.Items.Add("mmol");
            this.colColumnUnit.Items.Add("mmol/l");
            this.colColumnUnit.Items.Add("㎜");
            this.colColumnUnit.Items.Add("㏄");
            this.colColumnUnit.Items.Add("cmH2O");
            this.colColumnUnit.Items.Add("‰");
            this.colColumnUnit.Items.Add("次/分");
            this.colColumnUnit.Items.Add("升/分");
            //
            string[] types = ServerData.DataType.GetDataTypeNames();
            this.colColumnType.Items.AddRange(types);
            this.colColumnType.DisplayStyle = ComboBoxStyle.DropDownList;

            this.btnMoveUp.Image = Properties.Resources.MoveUp;
            this.btnMoveDown.Image = Properties.Resources.MoveDown;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadGridViewData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 读取数据库,装载当前DataGridView各行数据
        /// </summary>
        private void LoadGridViewData()
        {
            this.dataTableView1.Rows.Clear();
            if (GlobalMethods.Misc.IsEmptyString(this.m_szSchemaID))
                return;
            if (this.m_lstSchemaColumns == null || this.m_lstSchemaColumns.Count <= 0)
                return;

            for (int index = 0; index < this.m_lstSchemaColumns.Count; index++)
            {
                GridViewColumn gridViewColumn = this.m_lstSchemaColumns[index];
                if (gridViewColumn == null)
                    continue;

                int nRowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                this.SetRowData(row, gridViewColumn);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="gridViewColumn">绑定的数据</param>
        private void SetRowData(DataTableViewRow row, GridViewColumn gridViewColumn)
        {
            if (row == null || row.Index < 0 || gridViewColumn == null)
                return;
            row.Tag = gridViewColumn;
            row.Cells[this.colColumnName.Index].Value = gridViewColumn.ColumnName;
            row.Cells[this.colColumnTag.Index].Value = gridViewColumn.ColumnTag;
            row.Cells[this.colColumnType.Index].Value = gridViewColumn.ColumnType;
            row.Cells[this.colColumnUnit.Index].Value = gridViewColumn.ColumnUnit;
            row.Cells[this.colColumnWidth.Index].Value = gridViewColumn.ColumnWidth;
            row.Cells[this.colIsVisible.Index].Value = gridViewColumn.IsVisible;
            row.Cells[this.colIsPrint.Index].Value = gridViewColumn.IsPrint;
            row.Cells[this.colIsMiddle.Index].Value = gridViewColumn.IsMiddle;
            row.Cells[this.colColumnItems.Index].Value = gridViewColumn.ColumnItems;
            row.Cells[this.colColumnGroup.Index].Value = gridViewColumn.ColumnGroup;
            if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.DocTypeID))
            {
                row.Cells[this.colDocType.Index].Tag = gridViewColumn.DocTypeID;
                DocTypeInfo docTypeInfo = new DocTypeInfo();
                DocTypeService.Instance.GetDocTypeInfo(gridViewColumn.DocTypeID, ref docTypeInfo);
                if (docTypeInfo != null)
                {
                    row.Cells[this.colDocType.Index].Value = docTypeInfo.DocTypeName;
                }
            }
        }

        /// <summary>
        /// 获取指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="gridViewColumn">最新修改后的数据</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref GridViewColumn gridViewColumn)
        {
            if (row == null || row.Index < 0)
                return false;
            GridViewColumn old = row.Tag as GridViewColumn;
            if (old == null)
                return false;

            if (this.dataTableView1.IsDeletedRow(row))
            {
                gridViewColumn = old;
                return true;
            }
            gridViewColumn = old.Clone() as GridViewColumn;

            gridViewColumn.SchemaID = this.m_szSchemaID;
            gridViewColumn.ColumnID = row.Index.ToString();
            gridViewColumn.ColumnIndex = row.Index;

            object cellValue = row.Cells[this.colColumnName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入列名!");
                return false;
            }
            gridViewColumn.ColumnName = cellValue.ToString();
            gridViewColumn.ColumnTag = gridViewColumn.ColumnName;

            cellValue = row.Cells[this.colColumnTag.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
                gridViewColumn.ColumnTag = cellValue.ToString();

            cellValue = row.Cells[this.colColumnType.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
                gridViewColumn.ColumnType = cellValue.ToString();

            cellValue = row.Cells[this.colColumnWidth.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                int value = 0;
                if (!int.TryParse(cellValue.ToString(), out value))
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("您必须正确输入列的宽度!");
                    return false;
                }
                gridViewColumn.ColumnWidth = value;
            }

            cellValue = row.Cells[this.colColumnUnit.Index].Value;
            gridViewColumn.ColumnUnit = cellValue == null ? string.Empty : cellValue.ToString();

            cellValue = row.Cells[this.colIsVisible.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = true;
                if (bool.TryParse(cellValue.ToString(), out value))
                    gridViewColumn.IsVisible = value;
            }

            cellValue = row.Cells[this.colIsPrint.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = true;
                if (bool.TryParse(cellValue.ToString(), out value))
                    gridViewColumn.IsPrint = value;
            }

            cellValue = row.Cells[this.colIsMiddle.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = true;
                if (bool.TryParse(cellValue.ToString(), out value))
                    gridViewColumn.IsMiddle = value;
            }

            cellValue = row.Cells[this.colColumnItems.Index].Value;

            gridViewColumn.ColumnItems = cellValue == null ? string.Empty : cellValue.ToString();

            cellValue = row.Cells[this.colDocType.Index].Value;
            gridViewColumn.DocTypeID = cellValue == null ? string.Empty : row.Cells[this.colDocType.Index].Tag.ToString();
            cellValue = row.Cells[this.colColumnGroup.Index].Value;
            gridViewColumn.ColumnGroup = cellValue == null ? string.Empty : cellValue.ToString();
            return true;
        }

        /// <summary>
        /// 显示文档类型编辑对话框
        /// </summary>
        /// <param name="row">方案信息行</param>
        private void ShowDocTypeSelectForm(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return;
            TempletSelectForm frmTempletSelect = new TempletSelectForm();
            frmTempletSelect.MultiSelect = false;
            frmTempletSelect.Description = "请选择需要列关联的病历类型模板：";
            object tag = row.Cells[this.colDocType.Index].Tag;
            if (tag != null)
                frmTempletSelect.DefaultDocTypeID = tag.ToString();
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_RECORD;
            frmTempletSelect.ApplyEnv = szApplyEnv;

            if (frmTempletSelect.ShowDialog() != DialogResult.OK)
                return;

            row.Cells[this.colDocType.Index].Value = null;
            row.Cells[this.colDocType.Index].Tag = null;

            List<DocTypeInfo> lstDocTypeInfos = frmTempletSelect.SelectedDocTypes;
            if (lstDocTypeInfos != null && lstDocTypeInfos.Count > 0)
            {
                row.Cells[this.colDocType.Index].Value = lstDocTypeInfos[0].DocTypeName;
                row.Cells[this.colDocType.Index].Tag = lstDocTypeInfos[0].DocTypeID;
            }
            if (this.dataTableView1.IsNormalRowUndeleted(row))
                this.dataTableView1.SetRowState(row, RowState.Update);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow == null)
                return;
            int index = currRow.Index;
            if (!this.dataTableView1.MoveRow(currRow, index - 1))
                return;
            this.dataTableView1.SetRowState(index, RowState.Update);
            this.dataTableView1.SetRowState(index - 1, RowState.Update);
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow == null)
                return;
            int index = currRow.Index;
            if (!this.dataTableView1.MoveRow(currRow, index + 1))
                return;
            this.dataTableView1.SetRowState(index, RowState.Update);
            this.dataTableView1.SetRowState(index + 1, RowState.Update);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GridViewColumn gridViewColumn = null;
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                gridViewColumn = currRow.Tag as GridViewColumn;
            if (gridViewColumn == null)
                gridViewColumn = new GridViewColumn();
            else
                gridViewColumn = gridViewColumn.Clone() as GridViewColumn;

            int nRowIndex = this.dataTableView1.Rows.Count;
            if (this.dataTableView1.CurrentRow != null)
                nRowIndex = this.dataTableView1.CurrentRow.Index + 1;
            this.dataTableView1.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
            this.SetRowData(row, gridViewColumn);

            this.dataTableView1.Focus();
            this.dataTableView1.SelectRow(row);
            this.dataTableView1.SetRowState(row, RowState.New);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.m_lstSchemaColumns == null)
                this.m_lstSchemaColumns = new List<GridViewColumn>();
            this.m_lstSchemaColumns.Clear();

            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                if (row.IsDeletedRow)
                    continue;
                GridViewColumn gridViewColumn = null;
                if (!this.MakeRowData(row, ref gridViewColumn))
                    return;
                this.m_lstSchemaColumns.Add(gridViewColumn);
            }
            this.DialogResult = DialogResult.OK;
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
                this.SetRowData(row, row.Tag as GridViewColumn);
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

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            if (row == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (e.ColumnIndex == this.colDocType.Index)
            {
                this.ShowDocTypeSelectForm(row);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            //当数据标识列为空的时候,自动和列名一致
            if (e.ColumnIndex == this.colColumnName.Index)
            {
                DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (GlobalMethods.Misc.IsEmptyString(row.Cells[this.colColumnTag.Index].Value))
                    row.Cells[this.colColumnTag.Index].Value = row.Cells[e.ColumnIndex].Value;
            }
        }
    }
}