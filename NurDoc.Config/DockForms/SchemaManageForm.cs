// ***********************************************************
// 护理记录单表格的列的列表方案配置模块中,列列表方案管理窗口.
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
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Config.DockForms
{
    public partial class SchemaManageForm : DockContentBase
    {
        private string m_schemaType = string.Empty;

        /// <summary>
        /// 获取当前列显示方案类型
        /// </summary>
        public string SchemaType
        {
            get { return this.m_schemaType; }
        }

        public SchemaManageForm(MainForm mainForm, string szSchemaType)
            : base(mainForm)
        {
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
            this.m_schemaType = szSchemaType;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.UpdateBounds();

            string[] flags = ServerData.SchemaFlag.GetFlagNames();
            this.colSchemaFlag.Items.Add(string.Empty);
            this.colSchemaFlag.Items.AddRange(flags);
            this.colSchemaFlag.DisplayStyle = ComboBoxStyle.DropDownList;
            if (this.m_schemaType == ServerData.GridViewSchema.BATCH_RECORD)
            {
                this.colBatchRelativeSchemaId.Visible = true;
            }
        }

        #region"DataGridView行数据信息类"
        /// <summary>
        /// 行数据信息类,目前每行拥有schema和columns两种数据
        /// </summary>
        private class RowDataInfo : ICloneable
        {
            private GridViewSchema m_schema = null;

            /// <summary>
            /// 获取或设置行数据信息中的方案信息
            /// </summary>
            public GridViewSchema Schema
            {
                get { return this.m_schema; }
                set { this.m_schema = value; }
            }

            private List<GridViewColumn> m_columns = null;

            /// <summary>
            /// 获取或设置行数据信息中的方案列集合
            /// </summary>
            public List<GridViewColumn> Columns
            {
                get { return this.m_columns; }
                set { this.m_columns = value; }
            }

            public object Clone()
            {
                RowDataInfo rowData = new RowDataInfo();
                if (this.m_schema != null)
                    rowData.Schema = this.m_schema.Clone() as GridViewSchema;
                if (this.m_columns == null)
                    return rowData;

                rowData.Columns = new List<GridViewColumn>();
                for (int index = 0; index < this.m_columns.Count; index++)
                {
                    GridViewColumn column = this.m_columns[index];
                    if (column == null)
                        continue;
                    rowData.Columns.Add(column.Clone() as GridViewColumn);
                }
                return rowData;
            }
        }
        #endregion

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

            string szSchemaType = this.SchemaType;
            List<GridViewSchema> lstGridViewSchemas = null;
            short shRet = ConfigService.Instance.GetGridViewSchemas(szSchemaType, null, ref lstGridViewSchemas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录列方案列表下载失败!");
                return;
            }
            if (lstGridViewSchemas == null || lstGridViewSchemas.Count <= 0)
                return;

            for (int index = 0; index < lstGridViewSchemas.Count; index++)
            {
                GridViewSchema gridViewSchema = lstGridViewSchemas[index];
                if (gridViewSchema == null)
                    continue;

                int nRowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                if (gridViewSchema.IsDefault)
                {
                    this.dataTableView1.SelectRow(row);
                }
                RowDataInfo rowData = new RowDataInfo();
                rowData.Schema = gridViewSchema;
                rowData.Columns = this.GetSchemaColumns(gridViewSchema.SchemaID);
                this.SetRowData(row, rowData);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// 获取指定方案的列列表
        /// </summary>
        /// <param name="szSchemaID">方案ID</param>
        /// <returns>列列表</returns>
        private List<GridViewColumn> GetSchemaColumns(string szSchemaID)
        {
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szSchemaID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("方案列列表下载失败!");
                return null;
            }
            if (lstGridViewColumns == null || lstGridViewColumns.Count <= 0)
                return null;
            return lstGridViewColumns;
        }

        /// <summary>
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="rowData">绑定的数据</param>
        private void SetRowData(DataTableViewRow row, RowDataInfo rowData)
        {
            if (row == null || row.Index < 0 || rowData == null)
                return;
            row.Tag = rowData;
            if (rowData.Schema != null)
            {
                row.Cells[this.colSchemaID.Index].Value = rowData.Schema.SchemaID;
                row.Cells[this.colSchemaName.Index].Value = rowData.Schema.SchemaName;
                row.Cells[this.colSchemaFlag.Index].Value =
                    ServerData.SchemaFlag.GetFlagName(rowData.Schema.SchemaFlag);

                row.Cells[this.colWardName.Index].Value = rowData.Schema.WardName;
                row.Cells[this.colWardName.Index].Tag = rowData.Schema.WardCode;
                if (GlobalMethods.Misc.IsEmptyString(rowData.Schema.WardCode))
                    row.Cells[this.colWardName.Index].Value = "全院";

                row.Cells[this.colCreateTime.Index].Value =
                    rowData.Schema.CreateTime.ToString("yyyy-MM-dd HH:mm");

                row.Cells[this.colBatchRelativeSchemaId.Index].Value = rowData.Schema.RelatvieSchemaId;

            }
            if (rowData.Columns != null)
            {
                StringBuilder sbColumnName = new StringBuilder();
                int index = 0;
                while (index < rowData.Columns.Count)
                {
                    sbColumnName.Append(rowData.Columns[index++].ColumnName);
                    sbColumnName.Append(';');
                }
                row.Cells[this.colColumns.Index].Tag = rowData.Columns;
                row.Cells[this.colColumns.Index].Value = sbColumnName.ToString();
            }
        }

        /// <summary>
        /// 获取指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="rowData">最新修改后的数据</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref RowDataInfo rowData)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (row == null || row.Index < 0)
                return false;
            RowDataInfo old = row.Tag as RowDataInfo;
            if (old == null)
                return false;
            if (this.dataTableView1.IsDeletedRow(row))
            {
                rowData = old;
                return true;
            }

            rowData = old.Clone() as RowDataInfo;

            object cellValue = row.Cells[this.colSchemaName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入方案名称!");
                return false;
            }
            rowData.Schema.SchemaName = cellValue.ToString();

            cellValue = row.Cells[this.colSchemaFlag.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                rowData.Schema.SchemaFlag = string.Empty;
            }
            else
            {
                rowData.Schema.SchemaFlag =
                    ServerData.SchemaFlag.GetFlagCode(cellValue.ToString());
            }

            cellValue = row.Cells[this.colWardName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                rowData.Schema.WardName = string.Empty;
            else
                rowData.Schema.WardName = cellValue.ToString();

            cellValue = row.Cells[this.colWardName.Index].Tag;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                rowData.Schema.WardCode = string.Empty;
            else
                rowData.Schema.WardCode = cellValue.ToString();

            cellValue = row.Cells[this.colBatchRelativeSchemaId.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
                rowData.Schema.RelatvieSchemaId = string.Empty;
            else
                rowData.Schema.RelatvieSchemaId = cellValue.ToString();

            rowData.Schema.ModiferID = SystemContext.Instance.LoginUser.ID;
            rowData.Schema.ModiferName = SystemContext.Instance.LoginUser.Name;

            rowData.Columns = row.Cells[this.colColumns.Index].Tag as List<GridViewColumn>;
            int index = 0;
            while (rowData.Columns != null && index < rowData.Columns.Count)
                rowData.Columns[index++].SchemaID = rowData.Schema.SchemaID;
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

            RowDataInfo rowData = row.Tag as RowDataInfo;
            if (rowData == null)
                return SystemConst.ReturnValue.FAILED;
            string szSchemaID = rowData.Schema.SchemaID;
            rowData = null;

            if (!this.MakeRowData(row, ref rowData))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dataTableView1.IsDeletedRow(row))
            {
                if (!this.dataTableView1.IsNewRow(row))
                    shRet = ConfigService.Instance.DeleteGridViewSchema(szSchemaID);
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
                shRet = ConfigService.Instance.UpdateGridViewSchema(szSchemaID, rowData.Schema);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("无法更新当前记录!");
                    return SystemConst.ReturnValue.FAILED;
                }
                shRet = ConfigService.Instance.SaveGridViewColumns(szSchemaID, rowData.Columns);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("无法保存当前记录!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = rowData;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            else if (this.dataTableView1.IsNewRow(row))
            {
                shRet = ConfigService.Instance.SaveGridViewSchema(rowData.Schema);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("无法保存当前记录!");
                    return SystemConst.ReturnValue.FAILED;
                }
                shRet = ConfigService.Instance.SaveGridViewColumns(szSchemaID, rowData.Columns);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("无法更新当前记录!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = rowData;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        private bool SetDefaultSchema()
        {
            if (this.HasUncommit())
            {
                if (!this.CommitModify())
                    return false;
            }

            DataTableViewRow row = this.dataTableView1.CurrentRow;
            if (row == null)
                return false;

            RowDataInfo rowData = row.Tag as RowDataInfo;
            if (rowData == null || rowData.Schema == null)
                return false;

            string szSchemaID = rowData.Schema.SchemaID;
            string szSchemaType = this.SchemaType;
            string szWardCode = rowData.Schema.WardCode;
            short shRet = ConfigService.Instance.SetDefaultGridViewSchema(szSchemaID, szSchemaType, szWardCode);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("无法设置默认模板!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 显示方案包含列集合编辑窗口
        /// </summary>
        /// <param name="row">方案信息行</param>
        private void ShowDeptSelectForm(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return;
            string szDeptCode = string.Empty;
            object objValue = row.Cells[this.colWardName.Index].Tag;
            if (!GlobalMethods.Misc.IsEmptyString(objValue))
                szDeptCode = objValue.ToString();

            DeptSelectDialog deptSelectDialog = new DeptSelectDialog();
            deptSelectDialog.MultiSelect = false;

            DeptInfo deptInfo = new DeptInfo();
            deptInfo.DeptCode = szDeptCode;
            deptSelectDialog.DeptInfos = new DeptInfo[] { deptInfo };
            if (deptSelectDialog.ShowDialog() != DialogResult.OK)
                return;

            string szSelectedDeptCode = string.Empty;
            string szSelectedDeptName = string.Empty;
            if (deptSelectDialog.DeptInfos != null && deptSelectDialog.DeptInfos.Length > 0)
            {
                szSelectedDeptCode = deptSelectDialog.DeptInfos[0].DeptCode;
                szSelectedDeptName = deptSelectDialog.DeptInfos[0].DeptName;
            }
            if (szSelectedDeptCode == szDeptCode)
                return;
            row.Cells[this.colWardName.Index].Tag = szSelectedDeptCode;
            row.Cells[this.colWardName.Index].Value = szSelectedDeptName;
            if (this.dataTableView1.IsNormalRowUndeleted(row))
                this.dataTableView1.SetRowState(row, RowState.Update);
        }

        /// <summary>
        /// 显示方案包含列集合编辑窗口
        /// </summary>
        /// <param name="row">方案信息行</param>
        private void ShowColumnsEditForm(DataTableViewRow row)
        {
            if (row == null)
                return;
            string szSchemaID = null;
            object objValue = row.Cells[this.colSchemaID.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(objValue))
                return;
            szSchemaID = objValue.ToString();

            List<GridViewColumn> lstSchemaColumns = null;
            objValue = row.Cells[this.colColumns.Index].Tag;
            lstSchemaColumns = objValue as List<GridViewColumn>;

            ColumnManageForm schemaColumnForm = new ColumnManageForm();
            schemaColumnForm.SchemaID = szSchemaID;
            schemaColumnForm.SchemaColumns = lstSchemaColumns;
            if (schemaColumnForm.ShowDialog() != DialogResult.OK)
                return;

            lstSchemaColumns = schemaColumnForm.SchemaColumns;
            row.Cells[this.colColumns.Index].Tag = lstSchemaColumns;
            row.Cells[this.colColumns.Index].Value = null;
            if (this.dataTableView1.IsNormalRowUndeleted(row))
                this.dataTableView1.SetRowState(row, RowState.Update);
            if (lstSchemaColumns == null || lstSchemaColumns.Count <= 0)
                return;

            StringBuilder sbColumnText = new StringBuilder();
            foreach (GridViewColumn column in lstSchemaColumns)
            {
                if (column != null)
                    sbColumnText.Append(column.ColumnName + ";");
            }
            row.Cells[this.colColumns.Index].Value = sbColumnText.ToString();
            if (this.dataTableView1.IsNormalRowUndeleted(row))
                this.dataTableView1.SetRowState(row, RowState.Update);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserInfo userInfo = SystemContext.Instance.LoginUser;
            if (userInfo == null)
                return;
            RowDataInfo rowData = null;
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                rowData = currRow.Tag as RowDataInfo;
            if (rowData == null)
                rowData = new RowDataInfo();
            else
                rowData = rowData.Clone() as RowDataInfo;
            if (rowData.Schema == null)
            {
                rowData.Schema = new GridViewSchema();
                rowData.Schema.WardName = "全院";
            }
            rowData.Schema.SchemaType = this.SchemaType;
            rowData.Schema.CreateID = "ADMIN";
            rowData.Schema.CreateName = "ADMIN";
            rowData.Schema.CreateTime = SysTimeService.Instance.Now;
            rowData.Schema.SchemaID = rowData.Schema.MakeSchemaID();

            int nRowIndex = this.dataTableView1.Rows.Count;
            if (this.dataTableView1.CurrentRow != null)
                nRowIndex = this.dataTableView1.CurrentRow.Index + 1;
            this.dataTableView1.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
            this.SetRowData(row, rowData);

            this.dataTableView1.Focus();
            this.dataTableView1.SelectRow(row);
            this.dataTableView1.SetRowState(row, RowState.New);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.HasUncommit())
                this.CommitModify();
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
                this.SetRowData(row, row.Tag as RowDataInfo);
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
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            if (row == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (e.ColumnIndex == this.colWardName.Index)
            {
                this.ShowDeptSelectForm(row);
            }
            else if (e.ColumnIndex == this.colColumns.Index)
            {
                this.ShowColumnsEditForm(row);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == this.colSchemaID.Index || e.ColumnIndex == this.colColumns.Index)
                e.Cancel = true;
            else if (e.ColumnIndex == this.colWardName.Index || e.ColumnIndex == this.colCreateTime.Index)
                e.Cancel = true;
        }
    }
}