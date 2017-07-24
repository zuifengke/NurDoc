// ***********************************************************
// ������Ӳ���ϵͳ,��������¼��ģ�鴦��������.
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
        /// ����֧������¼�����ݵ���ʾ�ͱ༭�ı��ؼ�
        /// </summary>
        protected DataTableView m_dataTableView = null;

        /// <summary>
        /// ���ڴ洢����¼������Ϣ���пؼ����ϵ�Ĺ�ϣ��
        /// </summary>
        protected Hashtable m_htColumnTable = null;

        /// <summary>
        /// ���ڻ��浱ǰ���ڻ״̬����¼�뷽��
        /// </summary>
        protected GridViewSchema m_schema = null;

        public BatchRecordHandler(DataTableView dataTableView, GridViewSchema schema)
        {
            this.m_dataTableView = dataTableView;
            this.m_schema = schema;
        }

        /// <summary>
        /// ��Ե�ǰ�����б�,����ָ��ʱ������������
        /// </summary>
        /// <param name="dtRecordTime">ʱ���</param>
        /// <returns>�Ƿ���سɹ�</returns>
        public virtual bool LoadGridViewData(DateTime dtRecordTime)
        {
            return true;
        }

        /// <summary>
        /// ��Ե�ǰ�����б�,����������������
        /// </summary>
        /// <param name="dtRecordTime">��¼ʱ��</param>
        /// <returns>�Ƿ񱣴�ɹ�</returns>
        public virtual bool SaveGridViewData(DateTime dtRecordTime, string szSchemaId)
        {
            return true;
        }

        /// <summary>
        /// ����ָ��������ʾ������Ӧ������¼������Զ�����
        /// </summary>
        /// <param name="schema">����ʾ����</param>
        /// <returns>�Ƿ���سɹ�</returns>
        public virtual bool LoadGridViewColumns(GridViewSchema schema)
        {
            if (this.m_htColumnTable == null)
                this.m_htColumnTable = new Hashtable();
            this.m_htColumnTable.Clear();

            //�������ʾ�ĸ�������Ŀ��
            int index = this.m_dataTableView.Columns.Count - 1;
            while (index >= 3)
            {
                this.m_dataTableView.Columns.RemoveAt(index--);
            }

            //��ȡ����ʾ�����е����б�
            if (schema == null)
                return false;
            string szSchemaID = schema.SchemaID;
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szSchemaID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("����ʾ���������б�����ʧ��!");
                return false;
            }

            //����ȱʡ����ʾ�����еĸ���
            if (lstGridViewColumns == null || lstGridViewColumns.Count <= 0)
                return true;
            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                if (gridViewColumn == null)
                    continue;
                if (gridViewColumn.ColumnName == null)
                    gridViewColumn.ColumnName = string.Empty;

                DataGridViewColumn column = null;
                if (gridViewColumn.ColumnTag == "����")
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
