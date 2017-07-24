// ***********************************************************
// 护理病历配置管理系统,服务器表单属性信息编辑对话框.
// Author : YangMingkun, Date : 2012-7-30
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Utilities.Dialogs;
using Heren.Common.VectorEditor.Shapes;

namespace Heren.NurDoc.Config.Templet
{
    internal partial class TempletInfoForm : HerenForm
    {
        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// 获取或设置当前表单类型信息
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置当前表单类型信息")]
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
            set { this.m_docTypeInfo = value; }
        }

        private bool m_bIsNew = false;

        /// <summary>
        /// 获取或设置当前是否是新建
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        [Description("获取或设置当前是否是新建")]
        public bool IsNew
        {
            get { return this.m_bIsNew; }
            set { this.m_bIsNew = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置当前是否是目录
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        [Description("获取或设置当前是否是目录")]
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private List<WardDocType> m_lstWardDocTypes = null;

        /// <summary>
        /// 获取或设置当前表单所属病区
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置当前表单所属病区")]
        public List<WardDocType> WardDocTypes
        {
            get { return this.m_lstWardDocTypes; }
        }

        private List<GridViewColumn> m_lstGridViewColumns = null;

        /// <summary>
        /// 获取或设置当前表单类型的列列表
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置当前表单类型的列列表")]
        public List<GridViewColumn> GridViewColumns
        {
            get { return this.m_lstGridViewColumns; }
        }

        public TempletInfoForm()
        {
            this.InitializeComponent();
            this.m_lstWardDocTypes = new List<WardDocType>();
            this.m_lstGridViewColumns = new List<GridViewColumn>();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            if (this.m_bIsFolder)
            {
                this.Text = "目录信息";
                this.chkIsValid.Enabled = false;
                this.chkIsRepeat.Enabled = false;
                this.chkListPrintable.Enabled = false;
                this.chkFormPrintable.Enabled = false;
                this.btnMoveUp.Enabled = false;
                this.btnMoveDown.Enabled = false;
                this.dataTableView1.Enabled = false;
                this.btnNewColumn.Enabled = false;
            }
            string[] types = ServerData.DataType.GetDataTypeNames();
            this.colColumnType.Items.AddRange(types);
            this.colColumnType.DisplayStyle = ComboBoxStyle.DropDownList;
            this.btnMoveUp.Image = Properties.Resources.MoveUp;
            this.btnMoveDown.Image = Properties.Resources.MoveDown;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            this.LoadWardDocType();
            this.LoadGridViewColumns();
            this.LoadDocTypeInfo();
            this.txtTempletName.Focus();
            this.txtTempletName.SelectAll();
            this.LoadSchemas();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void LoadDocTypeInfo()
        {
            this.txtTempletID.Text = string.Empty;
            this.txtTempletName.Text = string.Empty;
            this.chkIsValid.Checked = true;
            this.chkIsVisible.Checked = true;
            this.chkIsRepeat.Checked = true;
            this.chkFormPrintable.Checked = false;
            this.chkListPrintable.Checked = false;
            this.txtDocTypeNo.Text = string.Empty;
            if (this.m_docTypeInfo == null)
                return;
            this.txtTempletID.Text = this.m_docTypeInfo.DocTypeID;
            this.txtTempletName.Text = this.m_docTypeInfo.DocTypeName;
            this.chkIsValid.Checked = this.m_docTypeInfo.IsValid;
            this.chkIsVisible.Checked = this.m_docTypeInfo.IsVisible;
            this.chkIsRepeat.Checked = this.m_docTypeInfo.IsRepeated;
            this.chkQCVisible.Checked = this.m_docTypeInfo.IsQCVisible;
            this.txtDocTypeNo.Text = this.m_docTypeInfo.DocTypeNo.ToString();

            ReflashcboSortColumns(true);

            if (this.m_docTypeInfo.PrintMode == FormPrintMode.Form
                || this.m_docTypeInfo.PrintMode == FormPrintMode.FormAndList)
                this.chkFormPrintable.Checked = true;
            if (this.m_docTypeInfo.PrintMode == FormPrintMode.List
                || this.m_docTypeInfo.PrintMode == FormPrintMode.FormAndList)
                this.chkListPrintable.Checked = true;
        }

        private void LoadSchemas()
        {
            //获取列显示方案列表
            string szSchemaType = ServerData.GridViewSchema.NURSING_RECORD;
            List<GridViewSchema> lstGridViewSchemas = null;
            short shRet = ConfigService.Instance.GetGridViewSchemas(szSchemaType, null, ref lstGridViewSchemas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("列显示方案列表下载失败!");
                return;
            }
            this.comboBox1.Items.Add(string.Empty);
            foreach (GridViewSchema GridViewSchema in lstGridViewSchemas)
            {
                int index = this.comboBox1.Items.Add(GridViewSchema);
                if (this.m_docTypeInfo.SchemaID == GridViewSchema.SchemaID)
                    this.comboBox1.SelectedIndex = index;
            }
        }

        private void ReflashcboSortColumns(bool bFirst)
        {
            int SelectedIndex = this.cboSortColumns.SelectedIndex;
            bool bSortColumnInGride = false;
            string szSelectValue = bFirst ? this.m_docTypeInfo.SortColumn : this.cboSortColumns.SelectedItem.ToString();
            this.cboSortColumns.Items.Clear();
            this.cboSortColumns.Items.Add(string.Empty);
            for (int rowindex = 0; rowindex <= this.dataTableView1.Rows.Count - 1; rowindex++)
            {
                string szColumnName = this.dataTableView1.Rows[rowindex].Cells[5].Value.ToString();
                if (!string.IsNullOrEmpty(szSelectValue)
                && szSelectValue == szColumnName)
                {
                    SelectedIndex = rowindex + 1;
                    bSortColumnInGride = true;
                }
                this.cboSortColumns.Items.Add(szColumnName);
            }

            if (this.m_docTypeInfo.SortMode == SortMode.Ascending)
            {
                this.cboSortMode.SelectedIndex = 1;
            }
            else if (this.m_docTypeInfo.SortMode == SortMode.Descending)
            {
                this.cboSortMode.SelectedIndex = 2;
            }
            else
            {
                this.cboSortMode.SelectedIndex = 0;
            }

            if (!bSortColumnInGride)
            {
                SelectedIndex = 0;
                this.cboSortMode.SelectedIndex = 0;
            }
            this.cboSortColumns.SelectedIndex = SelectedIndex;
        }

        private void LoadWardDocType()
        {
            this.txtWardList.Text = "全院<可双击修改>";
            this.txtWardList.Tag = null;
            if (this.m_docTypeInfo == null)
                return;
            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            List<WardDocType> lstWardDocTypes = null;
            short shRet = DocTypeService.Instance.GetDocTypeDeptList(szDocTypeID, ref lstWardDocTypes);
            if (shRet != SystemConst.ReturnValue.OK)
                return;
            if (lstWardDocTypes == null || lstWardDocTypes.Count <= 0)
                return;
            this.txtWardList.Tag = lstWardDocTypes;

            StringBuilder sbDeptList = new StringBuilder();
            foreach (WardDocType wardDocType in lstWardDocTypes)
                sbDeptList.Append(wardDocType.WardName + ";");
            this.txtWardList.Text = sbDeptList.ToString();
        }

        private void LoadGridViewColumns()
        {
            this.dataTableView1.Rows.Clear();
            if (this.m_bIsNew && !this.m_bIsFolder)
            {
                //this.LoadDefaultColumns();
            }
            if (this.m_docTypeInfo == null)
                return;

            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szDocTypeID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
                return;
            if (lstGridViewColumns == null || lstGridViewColumns.Count <= 0)
                return;

            StringBuilder sbDeptList = new StringBuilder();
            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                int index = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[index];
                this.SetRowData(row, gridViewColumn);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        private void LoadDefaultColumns()
        {
            GridViewColumn gridViewColumn = new GridViewColumn();
            gridViewColumn.ColumnID = "0";
            gridViewColumn.ColumnIndex = 0;
            gridViewColumn.IsMiddle = false;
            gridViewColumn.ColumnTag = "创建人";
            gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
            gridViewColumn.ColumnType = "字符";
            gridViewColumn.ColumnWidth = 64;
            int index = this.dataTableView1.Rows.Add();
            this.SetRowData(this.dataTableView1.Rows[index], gridViewColumn);
            this.dataTableView1.Rows[index].State = RowState.New;

            gridViewColumn = new GridViewColumn();
            gridViewColumn.ColumnID = "1";
            gridViewColumn.ColumnIndex = 1;
            gridViewColumn.IsMiddle = false;
            gridViewColumn.ColumnTag = "创建时间";
            gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
            gridViewColumn.ColumnType = "日期";
            gridViewColumn.ColumnWidth = 110;
            index = this.dataTableView1.Rows.Add();
            this.SetRowData(this.dataTableView1.Rows[index], gridViewColumn);
            this.dataTableView1.Rows[index].State = RowState.New;

            gridViewColumn = new GridViewColumn();
            gridViewColumn.ColumnID = "2";
            gridViewColumn.ColumnIndex = 2;
            gridViewColumn.IsMiddle = false;
            gridViewColumn.ColumnTag = "修改人";
            gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
            gridViewColumn.ColumnType = "字符";
            gridViewColumn.ColumnWidth = 64;
            index = this.dataTableView1.Rows.Add();
            this.SetRowData(this.dataTableView1.Rows[index], gridViewColumn);
            this.dataTableView1.Rows[index].State = RowState.New;

            gridViewColumn = new GridViewColumn();
            gridViewColumn.ColumnID = "3";
            gridViewColumn.ColumnIndex = 3;
            gridViewColumn.IsMiddle = false;
            gridViewColumn.ColumnTag = "修改时间";
            gridViewColumn.ColumnName = gridViewColumn.ColumnTag;
            gridViewColumn.ColumnType = "日期";
            gridViewColumn.ColumnWidth = 110;
            index = this.dataTableView1.Rows.Add();
            this.SetRowData(this.dataTableView1.Rows[index], gridViewColumn);
            this.dataTableView1.Rows[index].State = RowState.New;
        }

        private void SetRowData(DataTableViewRow row, GridViewColumn gridViewColumn)
        {
            if (row == null || gridViewColumn == null)
                return;
            row.Tag = gridViewColumn;
            row.Cells[this.colColumnName.Index].Value = gridViewColumn.ColumnName;
            row.Cells[this.colColumnWidth.Index].Value = gridViewColumn.ColumnWidth;
            row.Cells[this.colIsMiddle.Index].Value = gridViewColumn.IsMiddle;
            row.Cells[this.colIsVisible.Index].Value = gridViewColumn.IsVisible;
            row.Cells[this.colColumnGroup.Index].Value = gridViewColumn.ColumnGroup;
            row.Cells[this.colColumnItems.Index].Value = gridViewColumn.ColumnItems;
            row.Cells[this.colIsPrint.Index].Value = gridViewColumn.IsPrint;
            row.Cells[this.colColumnTag.Index].Value = gridViewColumn.ColumnTag;
            row.Cells[this.colColumnType.Index].Value = gridViewColumn.ColumnType;
            row.Cells[this.colColumnUnit.Index].Value = gridViewColumn.ColumnUnit;
        }

        /// <summary>
        /// 获取指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="gridViewColumn">最新修改后的数据</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref GridViewColumn gridViewColumn)
        {
            if (this.m_docTypeInfo == null || row == null || row.Index < 0)
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

            gridViewColumn.SchemaID = this.m_docTypeInfo.DocTypeID;
            gridViewColumn.ColumnID = row.Index.ToString();
            gridViewColumn.ColumnIndex = row.Index;

            object objValue = row.Cells[this.colColumnName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(objValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入列标题!");
                return false;
            }
            gridViewColumn.ColumnName = objValue.ToString();

            objValue = row.Cells[this.colColumnWidth.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(objValue))
            {
                int width = 100;
                if (!int.TryParse(objValue.ToString(), out width))
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("您必须正确输入列宽!");
                    return false;
                }
                gridViewColumn.ColumnWidth = width;
            }
            gridViewColumn.ColumnIndex = row.Index;

            objValue = row.Cells[this.colIsMiddle.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(objValue))
            {
                bool value = false;
                if (bool.TryParse(objValue.ToString(), out value))
                    gridViewColumn.IsMiddle = value;
            }

            objValue = row.Cells[this.colIsVisible.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(objValue))
            {
                bool value = true;
                if (bool.TryParse(objValue.ToString(), out value))
                    gridViewColumn.IsVisible = value;
            }

            objValue = row.Cells[this.colIsPrint.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(objValue))
            {
                bool value = true;
                if (bool.TryParse(objValue.ToString(), out value))
                    gridViewColumn.IsPrint = value;
            }

            objValue = row.Cells[this.colColumnGroup.Index].Value;
            gridViewColumn.ColumnGroup = objValue == null ? string.Empty : objValue.ToString();

            objValue = row.Cells[this.colColumnItems.Index].Value;
            gridViewColumn.ColumnItems = objValue == null ? string.Empty : objValue.ToString();

            gridViewColumn.ColumnTag = gridViewColumn.ColumnName;
            objValue = row.Cells[this.colColumnTag.Index].Value;
            gridViewColumn.ColumnTag = objValue == null ? string.Empty : objValue.ToString();
            objValue = row.Cells[this.colColumnType.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(objValue))
                gridViewColumn.ColumnType = objValue.ToString();
            objValue = row.Cells[this.colColumnUnit.Index].Value;
            gridViewColumn.ColumnUnit = objValue == null ? string.Empty : objValue.ToString();

            return true;
        }

        /// <summary>
        /// 显示所属病区列表数据编辑对话框
        /// </summary>
        private void ShowDeptListEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //初始化科室列表对话框中默认勾选的科室
            List<DeptInfo> lstDeptInfos = new List<DeptInfo>();
            List<WardDocType> lstWardDocTypes =
                this.txtWardList.Tag as List<WardDocType>;
            if (lstWardDocTypes == null)
                lstWardDocTypes = new List<WardDocType>();
            for (int index = 0;
                index < lstWardDocTypes.Count; index++)
            {
                WardDocType wardDocType = lstWardDocTypes[index];
                if (wardDocType == null)
                    continue;
                DeptInfo deptInfo = new DeptInfo();
                deptInfo.DeptCode = wardDocType.WardCode;
                deptInfo.DeptName = wardDocType.WardName;
                lstDeptInfos.Add(deptInfo);
            }

            DeptSelectDialog deptSelectDialog = new DeptSelectDialog();
            deptSelectDialog.DeptInfos = lstDeptInfos.ToArray();
            if (deptSelectDialog.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            this.txtWardList.Text = "全院<可双击修改>";
            this.txtWardList.Tag = null;
            DeptInfo[] deptInfos = deptSelectDialog.DeptInfos;
            if (deptInfos == null || deptInfos.Length <= 0)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            lstWardDocTypes.Clear();
            StringBuilder sbDeptList = new StringBuilder();
            for (int index = 0; index < deptInfos.Length; index++)
            {
                DeptInfo deptInfo = deptInfos[index];
                if (deptInfo == null)
                    continue;
                sbDeptList.Append(deptInfo.DeptName + ";");

                WardDocType wardDocType = new WardDocType();
                wardDocType.WardCode = deptInfo.DeptCode;
                wardDocType.WardName = deptInfo.DeptName;
                wardDocType.DocTypeID = this.txtTempletID.Text;
                wardDocType.DocTypeName = this.txtTempletName.Text;
                lstWardDocTypes.Add(wardDocType);
            }
            this.txtWardList.Tag = lstWardDocTypes;
            this.txtWardList.Text = sbDeptList.ToString();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 在病历类型模板摘要列列表中新增一行
        /// </summary>
        private void NewRow()
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

        /// <summary>
        /// 在病历类型模板摘要列列表中删除选中行
        /// </summary>
        private void DeleteRows()
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

        /// <summary>
        /// 在病历类型模板摘要列列表中取消修改选中行
        /// </summary>
        private void CancelModifyRows()
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

        /// <summary>
        /// 在病历类型模板摘要列列表中取消删除选中行
        /// </summary>
        private void CancelDeleteRows()
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

        /// <summary>
        /// 生成待保存的文档类型信息
        /// </summary>
        /// <returns>bool</returns>
        private bool MakeDocTypeInfo()
        {
            if (this.m_docTypeInfo == null)
                this.m_docTypeInfo = new DocTypeInfo();

            if (this.txtTempletID.Text.Trim() == string.Empty)
            {
                MessageBoxEx.ShowError("请输入病历类型ID号");
                return false;
            }

            if (this.txtTempletName.Text.Trim() == string.Empty)
            {
                MessageBoxEx.ShowError("请输入病历类型名称");
                return false;
            }

            this.m_docTypeInfo.DocTypeID = this.txtTempletID.Text.Trim();
            this.m_docTypeInfo.DocTypeName = this.txtTempletName.Text.Trim();
            this.m_docTypeInfo.IsRepeated = this.chkIsRepeat.Checked;
            this.m_docTypeInfo.IsVisible = this.chkIsVisible.Checked;
            this.m_docTypeInfo.IsValid = this.chkIsValid.Checked;
            this.m_docTypeInfo.IsQCVisible = this.chkQCVisible.Checked;
            GridViewSchema GridViewSchema = this.comboBox1.SelectedItem as GridViewSchema;
            if (GridViewSchema == null)
                this.m_docTypeInfo.SchemaID = string.Empty;
            else
                this.m_docTypeInfo.SchemaID = GridViewSchema.SchemaID;

            this.m_docTypeInfo.PrintMode = FormPrintMode.None;
            if (this.chkFormPrintable.Checked)
                this.m_docTypeInfo.PrintMode = FormPrintMode.Form;
            if (this.chkListPrintable.Checked)
                this.m_docTypeInfo.PrintMode = FormPrintMode.List;
            if (this.chkFormPrintable.Checked && this.chkListPrintable.Checked)
                this.m_docTypeInfo.PrintMode = FormPrintMode.FormAndList;
            if (this.cboSortColumns.SelectedItem != null)
            {
                this.m_docTypeInfo.SortColumn = this.cboSortColumns.SelectedItem.ToString();
            }
            else
            { this.m_docTypeInfo.SortColumn = string.Empty; }

            if (this.cboSortMode.SelectedIndex == 0 || this.cboSortMode.SelectedItem == null)
            { this.m_docTypeInfo.SortMode = SortMode.None; }
            else if (this.cboSortMode.SelectedIndex == 1)
            { this.m_docTypeInfo.SortMode = SortMode.Ascending; }
            else if (this.cboSortMode.SelectedIndex == 2)
            { this.m_docTypeInfo.SortMode = SortMode.Descending; }

            this.m_docTypeInfo.DocTypeNo =
                GlobalMethods.Convert.StringToValue(this.txtDocTypeNo.Text.Trim(), 0);
            return true;
        }

        /// <summary>
        /// 生成待保存的科室病历类型配置信息
        /// </summary>
        /// <returns>bool</returns>
        private bool MakeDeptInfoList()
        {
            if (this.m_lstWardDocTypes == null)
                this.m_lstWardDocTypes = new List<WardDocType>();
            this.m_lstWardDocTypes.Clear();

            if (this.m_docTypeInfo == null)
                return false;
            List<WardDocType> lstWardDocTypes = null;
            lstWardDocTypes = this.txtWardList.Tag as List<WardDocType>;
            if (lstWardDocTypes == null)
                return true;

            foreach (WardDocType wardDocType in lstWardDocTypes)
            {
                if (wardDocType == null)
                    continue;
                wardDocType.DocTypeID = this.m_docTypeInfo.DocTypeID;
                wardDocType.DocTypeName = this.m_docTypeInfo.DocTypeName;
                this.m_lstWardDocTypes.Add(wardDocType);
            }
            return true;
        }

        /// <summary>
        /// 生成待保存的病历类型模板摘要列信息
        /// </summary>
        /// <returns>bool</returns>
        private bool MakeGridViewColumnList()
        {
            if (this.m_lstGridViewColumns == null)
                this.m_lstGridViewColumns = new List<GridViewColumn>();
            this.m_lstGridViewColumns.Clear();

            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                if (row.IsDeletedRow)
                    continue;
                GridViewColumn gridViewColumn = null;
                if (!this.MakeRowData(row, ref gridViewColumn))
                    return false;
                if (gridViewColumn != null)
                    this.m_lstGridViewColumns.Add(gridViewColumn);
            }
            return true;
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

        private void mnuNewColumn_Click(object sender, EventArgs e)
        {
            this.NewRow();
        }

        private void mnuDeleteColumn_Click(object sender, EventArgs e)
        {
            this.DeleteRows();
        }

        private void mnuCancelModify_Click(object sender, EventArgs e)
        {
            this.CancelModifyRows();
        }

        private void mnuCancelDelete_Click(object sender, EventArgs e)
        {
            this.CancelDeleteRows();
        }

        private void txtWardList_DoubleClick(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowDeptListEditForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnNewColumn_Click(object sender, EventArgs e)
        {
            this.NewRow();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.MakeDocTypeInfo())
                return;
            if (!this.MakeDeptInfoList())
                return;
            if (!this.MakeGridViewColumnList())
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DialogResult = DialogResult.OK;
        }

        private void chkDocTypeNo_CheckedChanged(object sender, EventArgs e)
        {
            this.txtDocTypeNo.Enabled = this.chkDocTypeNo.Checked;
        }

        private void dataTableView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            //当数据标识列为空的时候,自动和列名一致
            if (e.ColumnIndex == this.colColumnName.Index)
            {
                DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (GlobalMethods.Misc.IsEmptyString(row.Cells[this.colColumnTag.Index].Value)
                    && !GlobalMethods.Misc.IsEmptyString(row.Cells[this.colColumnName.Index].Value))
                    row.Cells[this.colColumnTag.Index].Value = row.Cells[e.ColumnIndex].Value;
            }
            ReflashcboSortColumns(false);
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            List<ElementBase> elements = this.GetGridElements();
            Heren.NurDoc.Config.Report.ReportHandler.Instance.CreateReport(elements);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        #region 初始化到报表
        /// <summary>
        /// 初始化表格和标题
        /// </summary>
        /// <returns>List<ElementBase></returns>
        private List<ElementBase> GetGridElements()
        {
            bool bIsBuildByTag = false;
            bIsBuildByTag = MessageBox.Show("是否根据数据标识初始化报表表格", "选择", MessageBoxButtons.YesNo) == DialogResult.Yes;
            List<ElementBase> lstElements = new List<ElementBase>();
            RowColumnElement grid = new RowColumnElement();
            grid.Name = "评估表";
            DataGridViewRow row = null;
            int intGridwidth = 0;
            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                row = this.dataTableView1.Rows[index];
                TableColumn tabcol = new TableColumn();
                tabcol.Name = Convert.ToString(bIsBuildByTag ? row.Cells[this.colColumnTag.Index].Value : row.Cells[this.colColumnName.Index].Value);
                tabcol.Width = Convert.ToSingle(row.Cells[this.colColumnWidth.Index].Value);
                intGridwidth += Convert.ToInt32(tabcol.Width);
                grid.Columns.Add(tabcol);
            }
            grid.Top = TempletGridE.Top;
            grid.Left = TempletGridE.Left;
            grid.Height = TempletGridE.Heigth;
            grid.Width = intGridwidth == 0 ? TempletGridE.Width : intGridwidth;
            List<TableRow> lstRows = new List<TableRow>();
            for (int i = 1; i <= 30; i++)
            {
                lstRows.Add(new TableRow());
            }
            grid.Rows.AddRange(lstRows.ToArray());

            lstElements.Add(grid);

            TextFieldElement textElement = new TextFieldElement();
            textElement.Text = this.txtTempletName.Text;
            textElement.Top = TempletTextE.Top;
            textElement.Left = TempletTextE.Left;
            textElement.Width = TempletTextE.Width;
            textElement.Height = TempletTextE.Heigth;
            textElement.TextFont = new Font("宋体", 14.25f);
            textElement.TextAlign = ContentAlignment.MiddleCenter;
            lstElements.Add(textElement);

            return lstElements;
        }

        struct TempletTextE
        {
            public const int Top = 30;
            public const int Left = 280;
            public const int Width = 300;
            public const int Heigth = 50;
        }

        struct TempletGridE
        {
            public const int Top = 120;
            public const int Left = 50;
            public const int Width = 600;
            public const int Heigth = 720;
        }
        #endregion

        private void cboQCVisible_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
