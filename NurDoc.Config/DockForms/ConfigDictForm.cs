// ***********************************************************
// ���������ù���ϵͳ,��̨���ñ����ù�����.
// Author : YangMingkun, Date : 2012-3-25
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Config.Dialogs;

namespace Heren.NurDoc.Config.DockForms
{
    internal partial class ConfigDictForm : DockContentBase
    {
        public ConfigDictForm(MainForm mainForm)
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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();

            //�������������������б�
            FieldInfo[] fieldInfos = typeof(ServerData.ConfigKey).GetFields();
            if (fieldInfos == null || fieldInfos.Length <= 0)
                return;
            for (int index = 0; index < fieldInfos.Length; index++)
            {
                FieldInfo fieldInfo = fieldInfos[index];
                if (fieldInfo == null)
                    continue;
                object value = fieldInfo.GetValue(null);
                if (value == null)
                    continue;
                string szValue = value.ToString();
                if (!this.colConfigName.Items.Contains(szValue))
                    this.colConfigName.Items.Add(szValue);
                if (!this.colConfigGroup.Items.Contains(szValue))
                    this.colConfigGroup.Items.Add(szValue);
            }
            if (this.colConfigName.Items.Count > 0) this.colConfigName.Items.Sort();
            if (this.colConfigGroup.Items.Count > 0) this.colConfigGroup.Items.Sort();
        }

        /// <summary>
        /// ˢ�µ�ǰ���ڵ�������ʾ
        /// </summary>
        public override void OnRefreshView()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!this.CheckModifiedData())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.dataTableView1.Rows.Clear();
            this.Update();

            List<ConfigInfo> lstConfigInfos = null;
            short shRet = ConfigService.Instance.GetConfigData(null, null, ref lstConfigInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("�����ֵ�����ʧ��!");
            }
            if (lstConfigInfos == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            for (int index = 0; index < lstConfigInfos.Count; index++)
            {
                ConfigInfo configInfo = lstConfigInfos[index];
                if (configInfo == null)
                    continue;
                int nRowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                this.SetRowData(row, configInfo);
                this.dataTableView1.SetRowState(row, RowState.Normal);

                if (nRowIndex <= 0)
                    continue;

                DataTableViewRow prevRow = this.dataTableView1.Rows[nRowIndex - 1];
                if (!string.Equals(row.Cells[this.colConfigGroup.Index].Value
                    , prevRow.Cells[this.colConfigGroup.Index].Value))
                {
                    if (prevRow.DefaultCellStyle.BackColor == Color.Gainsboro)
                        row.DefaultCellStyle.BackColor = Color.White;
                    else
                        row.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = prevRow.DefaultCellStyle.BackColor;
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ����Ƿ���δ���������
        /// </summary>
        /// <returns>bool</returns>
        public override bool HasUncommit()
        {
            int nCount = this.dataTableView1.Rows.Count;
            if (nCount <= 0)
                return false;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dataTableView1.Rows[index];
                if (this.dataTableView1.IsDeletedRow(row))
                    return true;
                if (this.dataTableView1.IsNewRow(row))
                    return true;
                if (this.dataTableView1.IsModifiedRow(row))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ���浱ǰ���ڵ������޸�
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
                szMessageText = string.Format("������ֹ,�ѱ���{0}����¼!", count);
            else
                szMessageText = string.Format("����ɹ�,�ѱ���{0}����¼!", count);
            if (count > 0)
                szMessageText += "\r\nϵͳ�����ѱ�����,�������µ�¼����Ч!";
            MessageBoxEx.Show(szMessageText, MessageBoxIcon.Information);
            return shRet == SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ����ָ������ʾ������,�Լ��󶨵�����
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="configInfo">�󶨵�����</param>
        /// <returns>bool</returns>
        private bool SetRowData(DataTableViewRow row, ConfigInfo configInfo)
        {
            if (row == null || row.Index < 0 || configInfo == null)
                return false;
            row.Tag = configInfo;
            row.Cells[this.colConfigGroup.Index].Value = configInfo.GroupName;
            row.Cells[this.colConfigName.Index].Value = configInfo.ConfigName;
            row.Cells[this.colConfigValue.Index].Value = configInfo.ConfigValue;
            row.Cells[this.colConfigDesc.Index].Value = configInfo.ConfigDesc;
            return true;
        }

        /// <summary>
        /// ��ȡָ���������޸ĺ������
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="configInfo">�����޸ĺ������</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref ConfigInfo configInfo)
        {
            if (row == null || row.Index < 0)
                return false;
            ConfigInfo old = row.Tag as ConfigInfo;
            if (old == null)
                return false;

            if (this.dataTableView1.IsDeletedRow(row))
            {
                configInfo = old;
                return true;
            }
            configInfo = old.Clone() as ConfigInfo;

            object cellValue = row.Cells[this.colConfigGroup.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("��������������������!");
                return false;
            }
            configInfo.GroupName = cellValue.ToString();

            cellValue = row.Cells[this.colConfigName.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("��������������������!");
                return false;
            }
            configInfo.ConfigName = cellValue.ToString();

            cellValue = row.Cells[this.colConfigValue.Index].Value;
            if (cellValue == null)
                configInfo.ConfigValue = string.Empty;
            else
                configInfo.ConfigValue = cellValue.ToString();

            cellValue = row.Cells[this.colConfigDesc.Index].Value;
            if (cellValue == null)
                configInfo.ConfigDesc = string.Empty;
            else
                configInfo.ConfigDesc = cellValue.ToString();
            return true;
        }

        /// <summary>
        /// ����ָ���е����ݵ�Զ�����ݱ�,��Ҫע����ǣ��е�ɾ��״̬��������״̬����
        /// </summary>
        /// <param name="row">ָ����</param>
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

            ConfigInfo configInfo = row.Tag as ConfigInfo;
            if (configInfo == null)
                return SystemConst.ReturnValue.FAILED;
            string szGroupName = configInfo.GroupName;
            string szConfigName = configInfo.ConfigName;

            configInfo = null;
            if (!this.MakeRowData(row, ref configInfo))
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.dataTableView1.IsDeletedRow(row))
            {
                if (!this.dataTableView1.IsNewRow(row))
                    shRet = ConfigService.Instance.DeleteConfigData(szGroupName, szConfigName);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("�޷�ɾ����ǰ��¼!");
                    return SystemConst.ReturnValue.FAILED;
                }
                this.dataTableView1.Rows.Remove(row);
            }
            else if (this.dataTableView1.IsModifiedRow(row))
            {
                shRet = ConfigService.Instance.UpdateConfigData(szGroupName, szConfigName, configInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("�޷����µ�ǰ��¼!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = configInfo;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            else if (this.dataTableView1.IsNewRow(row))
            {
                shRet = ConfigService.Instance.SaveConfigData(configInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    this.dataTableView1.SelectRow(row);
                    MessageBoxEx.Show("�޷����浱ǰ��¼!");
                    return SystemConst.ReturnValue.FAILED;
                }
                row.Tag = configInfo;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ��ʾ��ѯ������öԻ���
        /// </summary>
        /// <param name="row">ָ����</param>
        private void ShowConfigValueEditForm(DataTableViewRow row)
        {
            if (row == null || row.Index < 0 || this.dataTableView1.IsDeletedRow(row))
                return;
            if (this.dataTableView1.EditingControl != null)
                this.dataTableView1.EndEdit();
            LargeTextEditForm largeTextEditForm = new LargeTextEditForm();
            largeTextEditForm.Text = "�༭��������";
            DataGridViewCell cell = row.Cells[this.colConfigValue.Index];
            if (cell.Value != null)
                largeTextEditForm.LargeText = cell.Value.ToString();
            if (largeTextEditForm.ShowDialog() != DialogResult.OK)
                return;
            string szConfigValue = largeTextEditForm.LargeText;
            if (szConfigValue.Equals(cell.Value))
                return;
            cell.Value = szConfigValue;
            if (this.dataTableView1.IsNormalRowUndeleted(row))
                this.dataTableView1.SetRowState(row, RowState.Update);
        }

        /// <summary>
        /// ��ʾ���������ı��༭�Ի���
        /// </summary>
        /// <param name="row">ָ����</param>
        private void ShowConfigDescEditForm(DataTableViewRow row)
        {
            if (row == null || row.Index < 0 || this.dataTableView1.IsDeletedRow(row))
                return;
            if (this.dataTableView1.EditingControl != null)
                this.dataTableView1.EndEdit();
            LargeTextEditForm largeTextEditForm = new LargeTextEditForm();
            largeTextEditForm.Text = "�༭��������";
            DataGridViewCell cell = row.Cells[this.colConfigDesc.Index];
            if (cell.Value != null)
                largeTextEditForm.LargeText = cell.Value.ToString();
            if (largeTextEditForm.ShowDialog() != DialogResult.OK)
                return;
            string szConfigDesc = largeTextEditForm.LargeText.Trim();
            if (szConfigDesc.Equals(cell.Value))
                return;
            cell.Value = szConfigDesc;
            if (this.dataTableView1.IsNormalRowUndeleted(row))
                this.dataTableView1.SetRowState(row, RowState.Update);
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
                this.SetRowData(row, row.Tag as ConfigInfo);
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

        private void btnCommit_Click(object sender, EventArgs e)
        {
            this.CommitModify();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ConfigInfo configInfo = null;
            DataTableViewRow currRow = this.dataTableView1.CurrentRow;
            if (currRow != null && currRow.Index >= 0)
                configInfo = currRow.Tag as ConfigInfo;
            if (configInfo == null)
                configInfo = new ConfigInfo();
            else
                configInfo = configInfo.Clone() as ConfigInfo;

            int nRowIndex = this.dataTableView1.Rows.Count;
            if (this.dataTableView1.CurrentRow != null)
                nRowIndex = this.dataTableView1.CurrentRow.Index + 1;
            this.dataTableView1.Rows.Insert(nRowIndex, 1);
            DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
            this.SetRowData(row, configInfo);

            this.dataTableView1.Focus();
            this.dataTableView1.SelectRow(row);
            this.dataTableView1.SetRowState(row, RowState.New);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            if (e.ColumnIndex == this.colConfigValue.Index)
                this.ShowConfigValueEditForm(row);
            else if (e.ColumnIndex == this.colConfigDesc.Index)
                this.ShowConfigDescEditForm(row);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}