// ***********************************************************
// 护理电子病历系统,数据批量录入模块处理器基类.
// Creator:YangMingkun  Date:2013-1-9
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame.Dialogs;

namespace Heren.NurDoc.Frame.DockForms
{
    internal class BatchRecordHandler
    {
        /// <summary>
        /// 用来支持批量录入数据的显示和编辑的表格控件
        /// </summary>
        protected DataTableView m_dataTableView = null;

        /// <summary>
        /// 用于存储批量录入列信息和列控件间关系的哈希表
        /// </summary>
        protected Hashtable m_htColumnTable = null;

        /// <summary>
        /// 用于缓存当前处于活动状态的列录入方案
        /// </summary>
        protected GridViewSchema m_schema = null;

        public BatchRecordHandler(DataTableView dataTableView, GridViewSchema schema)
        {
            this.m_dataTableView = dataTableView;
            this.m_schema = schema;
        }

        /// <summary>
        /// 针对当前病人列表,加载指定时间点的体征数据
        /// </summary>
        /// <param name="dtRecordTime">时间点</param>
        /// <returns>是否加载成功</returns>
        public virtual bool LoadGridViewData(DateTime dtRecordTime)
        {
            return true;
        }

        /// <summary>
        /// 针对当前病人列表,保存所有体征数据
        /// </summary>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>是否保存成功</returns>
        public virtual bool SaveGridViewData(DateTime dtRecordTime, string szSchemaId)
        {
            return true;
        }

        /// <summary>
        /// 加载指定的列显示方案对应的体征录入表格的自定义列
        /// </summary>
        /// <param name="schema">列显示方案</param>
        /// <returns>是否加载成功</returns>
        public virtual bool LoadGridViewColumns(GridViewSchema schema)
        {
            if (this.m_htColumnTable == null)
                this.m_htColumnTable = new Hashtable();
            this.m_htColumnTable.Clear();

            //清除已显示的各体征项目列
            int index = this.m_dataTableView.Columns.Count - 1;
            while (index >= 3)
            {
                this.m_dataTableView.Columns.RemoveAt(index--);
            }

            //获取列显示方案中的列列表
            if (schema == null)
                return false;
            string szSchemaID = schema.SchemaID;
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szSchemaID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("列显示方案的列列表下载失败!");
                return false;
            }

            //加载缺省列显示方案中的各列
            if (lstGridViewColumns == null || lstGridViewColumns.Count <= 0)
                return true;
            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                if (gridViewColumn == null)
                    continue;
                if (gridViewColumn.ColumnName == null)
                    gridViewColumn.ColumnName = string.Empty;

                DataGridViewColumn column = null;
                if (gridViewColumn.ColumnTag == "体温")
                {
                    column = new ComboBoxTextBoxColumn();
                    ((ComboBoxTextBoxColumn)column).Items.AddRange(SystemContext.Instance.TempTypeList);
                    ((ComboBoxTextBoxColumn)column).DefaultSelectedIndex = 0;
                }
                else if (gridViewColumn.ColumnType == ServerData.DataType.BOOLEAN)
                {
                    column = new DataGridViewCheckBoxColumn();
                }
                else if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnItems))
                {
                    column = new ComboBoxColumn();
                    string[] items = gridViewColumn.ColumnItems.Split(';');
                    ((ComboBoxColumn)column).Items.AddRange(items);
                    if (items != null && items.Length > 0 && string.IsNullOrEmpty(items[0]))
                        ((ComboBoxColumn)column).DisplayStyle = ComboBoxStyle.DropDownList;
                }
                else
                {
                    column = new DataGridViewTextBoxColumn();
                }

                column.Tag = gridViewColumn;
                column.HeaderText = gridViewColumn.ColumnName + " " + gridViewColumn.ColumnUnit;
                column.Width = gridViewColumn.ColumnWidth;
                column.Visible = gridViewColumn.IsVisible;

                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                if (!gridViewColumn.IsMiddle)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                this.m_dataTableView.Columns.Add(column);

                string szColumnKey = gridViewColumn.ColumnTag;
                if (GlobalMethods.Misc.IsEmptyString(szColumnKey))
                    szColumnKey = gridViewColumn.ColumnName;
                if (!this.m_htColumnTable.Contains(szColumnKey))
                    this.m_htColumnTable.Add(szColumnKey, column);
            }
            return true;
        }

        public static BatchRecordHandler CreateBatchHandler(DataTableView dataTableView, GridViewSchema schema)
        {
            if (dataTableView == null || schema == null)
                return null;
            if (schema.SchemaFlag == ServerData.SchemaFlag.NUR_REC)
                return new NursingRecHandler(dataTableView, schema);
            return new VitalSignsHandler(dataTableView, schema);
        }
    }
}
