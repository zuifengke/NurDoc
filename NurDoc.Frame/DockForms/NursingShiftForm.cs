// ***********************************************************
// ������Ӳ���ϵͳ,�����Ӱര��.
// Creator:YangMingkun  Date:2013-7-12
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.Frame.Dialogs;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class NursingShiftForm : DockContentBase
    {
        /// <summary>
        /// ��ʶ�ؼ�ֵ�ı��¼��Ƿ����,�����ظ�ִ��
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// ��������  ������չʾ���⻤����ʱ ��������
        /// </summary>
        private enum FilterType
        {
            All, SepcialCare, Normal
        }

        private ShiftRankInfo[] lstShiftRankInfos = null;

        private List<ShiftConfigInfo> lstShiftConfigInfos = null;

        private Dictionary<string, bool> dicConfigImprotant = new Dictionary<string, bool>();

        public NursingShiftForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.WorkShiftRecord;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 500);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.RefreshView();
        }

        public override void RefreshView()
        {
            base.RefreshView();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.m_bValueChangedEnabled = false;

            //���õ�ǰĬ����ʾ�Ľ�������
            this.tooldtpShiftDate.Value = SysTimeService.Instance.Now.Date;

            //�����������ü�����
            this.ClearShiftPatient(false);
            this.ClearWardStatus(false);
            this.ClearSpecialShiftPatient(true);

            //�޸���Ŀ�����ɼ���
            this.EditcolShiftItemVisible();

            //������������н������б�
            this.LoadShiftRankList();

            //���ز�����̬�����ͳ����Ϣ��
            this.LoadWardStatusColumns();

            this.Update();

            //���ص�ǰ�����ڶ�Ӧ�Ľ�������
            this.LoadNursingShiftData();

            if (this.dgvSpecialPatient.Visible == true)
            {
                //���ص�ǰ���ڶ�Ӧ�����ⲡ�˲��齻������
                this.LoadSpecialShiftData();
            }
            else
            {
                this.dgvShiftPatient.Dock = DockStyle.Fill;
            }

            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnDisplayDeptChanged(EventArgs e)
        {
            base.OnDisplayDeptChanged(e);
            this.RefreshView();
        }

        #region �����������ü�����
        /// <summary>
        /// ���������̬��������ü�����
        /// </summary>
        /// <param name="bOnlyData">�Ƿ�ֻ�������</param>
        private void ClearWardStatus(bool bOnlyData)
        {
            if (!bOnlyData)
            {
                //������ͨ ��̬
                int columnIndex = this.dgvWardStatus.Columns.Count - 1;
                while (columnIndex >= 0)
                {
                    DataGridViewColumn column = this.dgvWardStatus.Columns[columnIndex];
                    ShiftConfigInfo shiftConfigInfo = column.Tag as ShiftConfigInfo;
                    if (shiftConfigInfo != null)
                        this.dgvWardStatus.Columns.RemoveAt(columnIndex);
                    columnIndex--;
                }
            }

            for (int iRowIndex = 0; iRowIndex < this.dgvWardStatus.Rows.Count; iRowIndex++)
            {
                for (int iColumnIndex = 0; iColumnIndex < this.dgvWardStatus.Columns.Count; iColumnIndex++)
                {
                    ShiftConfigInfo shiftConfigInfo = this.dgvWardStatus.Columns[iColumnIndex].Tag as ShiftConfigInfo;
                    if (shiftConfigInfo != null)
                        this.dgvWardStatus.Rows[iRowIndex].Cells[iColumnIndex].Value = null;
                }
            }
        }

        /// <summary>
        /// ������ಡ����Ϣ
        /// </summary>
        /// <param name="bOnlyData">�Ƿ�ֻ�������</param>
        private void ClearShiftPatient(bool bOnlyData)
        {
            if (!bOnlyData)
            {
                //���˽�����Ϣ
                int columnIndex = this.dgvShiftPatient.Columns.Count - 1;
                while (columnIndex >= 0)
                {
                    DataGridViewColumn column = this.dgvShiftPatient.Columns[columnIndex];
                    ShiftRankInfo shiftRankInfo = column.Tag as ShiftRankInfo;
                    if (shiftRankInfo != null)
                        this.dgvShiftPatient.Columns.RemoveAt(columnIndex);
                    columnIndex--;
                }
            }
            this.dgvShiftPatient.Rows.Clear();
        }

        /// <summary>
        /// ������ⲡ�˽�����Ϣ
        /// </summary>
        /// <param name="bOnlyData">�Ƿ�ֻ�������</param>
        private void ClearSpecialShiftPatient(bool bOnlyData)
        {
            if (!bOnlyData)
            {
                //���ⲡ�˽�����Ϣ
                int columnIndex = this.dgvSpecialPatient.Columns.Count - 1;
                while (columnIndex >= 0)
                {
                    DataGridViewColumn column = this.dgvSpecialPatient.Columns[columnIndex];
                    ShiftRankInfo shiftRankInfo = column.Tag as ShiftRankInfo;
                    this.dgvSpecialPatient.Columns.RemoveAt(columnIndex);
                    columnIndex--;
                }
            }
            this.dgvSpecialPatient.Rows.Clear();
        }
        #endregion

        /// <summary>
        /// �޸���Ŀ�����пɼ���
        /// </summary>
        /// <returns></returns>
        private bool EditcolShiftItemVisible()
        {
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            //��ȡ�Ƿ���ڽ�����Ŀ���ñ���
            List<ShiftItemAliasInfo> lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            short shRet = NurShiftService.Instance.GetShiftItemAliasInfos(szWardCode, ref lstShiftItemAliasInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible = false;
                MessageBoxEx.Show("��ѯ������Ŀ���ñ����б�ʧ��!");
                return false;
            }
            if (lstShiftItemAliasInfos == null || lstShiftItemAliasInfos.Count <= 0)
            {
                this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible = false;
                return false;
            }
            else
            {
                this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible = true;
                return true;
            }
        }

        /// <summary>
        /// ���ز�����̬���б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadWardStatusColumns()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            string szWardCode = SystemContext.Instance.DisplayDept.DeptCode;
            short shRet = NurShiftService.Instance.GetShiftConfigInfos(new string[] { szWardCode, "ALL" }, ref lstShiftConfigInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ������̬������б�ʧ��!");
                return false;
            }

            if (lstShiftConfigInfos == null || lstShiftConfigInfos.Count <= 0)
                return true;
            HandlerUI(lstShiftConfigInfos);

            dicConfigImprotant.Clear();
            foreach (ShiftConfigInfo shiftConfigInfo in lstShiftConfigInfos)
            {
                if (!dicConfigImprotant.ContainsKey(shiftConfigInfo.ItemName))
                    dicConfigImprotant.Add(shiftConfigInfo.ItemName, shiftConfigInfo.Important);
                if (!shiftConfigInfo.Important)
                {
                    DataGridViewTextBoxColumn colItem = new DataGridViewTextBoxColumn();
                    colItem.Tag = shiftConfigInfo;
                    colItem.Visible = shiftConfigInfo.Visible;
                    colItem.Width = shiftConfigInfo.ItemWidth;
                    colItem.HeaderText = shiftConfigInfo.ItemName;
                    colItem.SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (!shiftConfigInfo.Middle)
                    {
                        colItem.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        colItem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                    }
                    else
                    {
                        colItem.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        colItem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                    }
                    this.dgvWardStatus.Columns.Add(colItem);
                }
            }
            return true;
        }

        /// <summary>
        /// ���ؽ������б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftRankList()
        {
            lstShiftRankInfos = SystemContext.Instance.GetWardShiftRankList();
            for (int index = 0; index < lstShiftRankInfos.Length; index++)
            {
                ShiftRankInfo shiftRankInfo = lstShiftRankInfos[index];
                if (shiftRankInfo == null)
                    continue;

                //������̬����а���б�
                int rowIndex = this.dgvWardStatus.Rows.Add();
                DataTableViewRow row = this.dgvWardStatus.Rows[rowIndex];
                row.Cells[this.colRank.Index].Tag = shiftRankInfo;
                row.Cells[this.colRank.Index].Value = shiftRankInfo.RankName;
                this.dgvWardStatus.SetRowState(row, RowState.Normal);

                //�����¼����а���б�
                DataGridViewTextBoxColumn colRank = new DataGridViewTextBoxColumn();
                colRank.Tag = shiftRankInfo;
                colRank.Width = 190;
                colRank.SortMode = DataGridViewColumnSortMode.NotSortable;
                colRank.HeaderText = string.Format("{0}({1}-{2})", shiftRankInfo.RankName
                    , shiftRankInfo.StartPoint, shiftRankInfo.EndPoint);
                this.dgvShiftPatient.Columns.Add(colRank);

                DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
                GlobalMethods.Reflect.CopyProperties(colRank, colStatus);
            }
            return true;
        }

        /// <summary>
        /// ���ݶ�̬�����Ƿ���ʾ��Ҫ��Ŀ��̬
        /// </summary>
        /// <param name="lstShiftConfigInfos">��̬�����б�</param>
        private void HandlerUI(List<ShiftConfigInfo> lstShiftConfigInfos)
        {
            foreach (ShiftConfigInfo config in lstShiftConfigInfos)
            {
                if (config.Important)
                {
                    this.toolStrip2.Visible = true;
                    this.dgvSpecialPatient.Visible = true;
                    return;
                }
            }
            this.toolStrip2.Visible = false;
            this.dgvSpecialPatient.Visible = false;
        }

        /// <summary>
        /// ��ȡָ���Ľ����ζ�Ӧ�Ĳ�����̬�����
        /// </summary>
        /// <param name="szRankCode">�����δ���</param>
        /// <returns>������̬�����</returns>
        private DataTableViewRow GetWardStatusTableRow(string szRankCode)
        {
            foreach (DataTableViewRow row in this.dgvWardStatus.Rows)
            {
                ShiftRankInfo shiftRankInfo = row.Cells[this.colRank.Index].Tag as ShiftRankInfo;
                if (shiftRankInfo != null && shiftRankInfo.RankCode == szRankCode)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ���Ľ�����Ŀ��Ӧ�Ĳ�����̬�����
        /// </summary>
        /// <param name="szItemName">������Ŀ����</param>
        /// <returns>������̬�����</returns>
        private DataGridViewColumn GetWardStatusTableColumn(string szItemName)
        {
            foreach (DataGridViewColumn column in this.dgvWardStatus.Columns)
            {
                ShiftConfigInfo shiftConfigInfo = column.Tag as ShiftConfigInfo;
                if (shiftConfigInfo != null && shiftConfigInfo.ItemName == szItemName)
                    return column;
            }
            return null;
        }

        /// <summary>
        /// ��ȡ������̬�����ָ���ж�Ӧ�Ľ�������¼��Ϣ
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <returns>��������¼��Ϣ</returns>
        private NursingShiftInfo GetWardStatusTableShiftInfo(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return null;
            return row.Tag as NursingShiftInfo;
        }

        /// <summary>
        /// ��յ�ǰ��ʾ�Ĳ�����̬���������
        /// </summary>
        private void ClearWardStatusTableData()
        {
            foreach (DataTableViewRow row in this.dgvWardStatus.Rows)
            {
                row.Tag = null;
                foreach (DataGridViewColumn column in this.dgvWardStatus.Columns)
                {
                    if (column.Tag is ShiftConfigInfo)
                        row.Cells[column.Index].Value = null;
                }
            }
        }

        /// <summary>
        /// ��ȡָ���Ľ��ಡ�˶�Ӧ�Ľ��ಡ�˱����
        /// </summary>
        /// <param name="shiftPatient">���ಡ����Ϣ</param>
        /// <returns>���ಡ�˱����</returns>
        private DataTableViewRow GetShiftPatientTableRow(ShiftPatient shiftPatient)
        {
            if (shiftPatient == null)
                return null;
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                ShiftPatient patient = row.Tag as ShiftPatient;
                if (patient == null)
                    continue;
                if (patient.PatientID == shiftPatient.PatientID
                    && patient.VisitID == shiftPatient.VisitID && patient.SubID == shiftPatient.SubID)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ���Ľ����ζ�Ӧ�Ľ��ಡ�˱����
        /// </summary>
        /// <param name="szRankCode">�����δ���</param>
        /// <returns>���ಡ�˱����</returns>
        private DataGridViewColumn GetShiftPatientTableColumn(string szRankCode)
        {
            for (int index = this.dgvShiftPatient.Columns.Count - 1; index >= 0; index--)
            {
                DataGridViewColumn column = this.dgvShiftPatient.Columns[index];

                ShiftRankInfo shiftRankInfo = column.Tag as ShiftRankInfo;
                if (shiftRankInfo != null && shiftRankInfo.RankCode == szRankCode)
                    return column;
                NursingShiftInfo nursingShiftInfo = column.Tag as NursingShiftInfo;
                if (nursingShiftInfo != null && nursingShiftInfo.ShiftRankCode == szRankCode)
                    return column;
            }
            return null;
        }

        /// <summary>
        /// ��ȡ������̬�����ָ����Ԫ���Ӧ�Ľ�������¼��Ϣ
        /// </summary>
        /// <param name="cell">ָ����Ԫ��</param>
        /// <returns>��������¼��Ϣ</returns>
        private NursingShiftInfo GetShiftPatientTableShiftInfo(DataGridViewCell cell)
        {
            if (cell == null || cell.RowIndex < 0 || cell.ColumnIndex < 0)
                return null;
            int columnIndex = cell.ColumnIndex;
            DataGridViewColumn column = this.dgvShiftPatient.Columns[columnIndex];
            if (!(column.Tag is ShiftRankInfo))
            {
                foreach (DataGridViewColumn col in this.dgvShiftPatient.Columns)
                {
                    if (col.Tag is ShiftRankInfo)
                    {
                        columnIndex = col.Index;
                        break;
                    }
                }
            }
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (row.Cells[columnIndex].Tag is NursingShiftInfo)
                    return row.Cells[columnIndex].Tag as NursingShiftInfo;
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ���Ľ��ಡ�˶�Ӧ�Ľ��ಡ�˱����
        /// </summary>
        /// <param name="shiftPatient">���ಡ����Ϣ</param>
        /// <returns>���ಡ�˱����</returns>
        private DataTableViewRow GetShiftSpecialPatientTableRow(ShiftSpecialPatient shiftSpecialPatient)
        {
            if (shiftSpecialPatient == null)
                return null;
            foreach (DataTableViewRow row in this.dgvSpecialPatient.Rows)
            {
                ShiftSpecialPatient patient = row.Tag as ShiftSpecialPatient;
                if (patient == null)
                    continue;
                if (patient.PatientID == shiftSpecialPatient.PatientID
                    && patient.VisitID == shiftSpecialPatient.VisitID && patient.SubID == shiftSpecialPatient.SubID)
                    return row;
            }
            return null;
        }    

        /// <summary>
        /// ��ָ���Ľ��ಡ���б�������Ŀ˳���������
        /// </summary>
        /// <param name="lstShiftPatients">���ಡ���б�</param>
        /// <returns>����Ľ��ಡ���б�</returns>
        private List<ShiftPatient> GetOrderedShiftPatients(List<ShiftPatient> lstShiftPatients)
        {
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return lstShiftPatients;

            List<ShiftPatient> lstResultList = new List<ShiftPatient>();
            CommonDictItem[] commonDictItems = SystemContext.Instance.GetWardShiftItemList();
            foreach (CommonDictItem commonDictItem in commonDictItems)
            {
                int index = 0;
                while (index < lstShiftPatients.Count)
                {
                    ShiftPatient shiftPatient = lstShiftPatients[index++];
                    if (string.IsNullOrEmpty(shiftPatient.ShiftItemName))
                        continue;
                    if (shiftPatient.ShiftItemName.StartsWith(commonDictItem.ItemName))
                    {
                        index--;
                        lstShiftPatients.Remove(shiftPatient);
                        lstResultList.Add(shiftPatient);
                    }
                }
            }
            lstResultList.AddRange(lstShiftPatients);
            return lstResultList;
        }

        /// <summary>
        /// ��ָ���Ľ��ಡ���б�,��������ĿΪ�յĲ��˷������
        /// </summary>
        /// <param name="lstShiftPatients">���ಡ���б�</param>
        /// <returns>����Ľ��ಡ���б�</returns>
        private List<ShiftPatient> GetOrderedItemShiftPatients(List<ShiftPatient> lstShiftPatients)
        {
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return lstShiftPatients;
            bool flag = true;
            foreach (ShiftPatient shiftPatient in lstShiftPatients)
            {
                if (!string.IsNullOrEmpty(shiftPatient.ShiftItemName))
                {
                    flag = false;
                    break;
                }
            }
            if (flag == false)
            {
                List<ShiftPatient> lstResultList = new List<ShiftPatient>();
                int index = 0;
                while (index < lstShiftPatients.Count)
                {
                    ShiftPatient shiftPatient = lstShiftPatients[index++];
                    if (string.IsNullOrEmpty(shiftPatient.ShiftItemName))
                    {
                        index--;
                        lstShiftPatients.Remove(shiftPatient);
                        lstResultList.Add(shiftPatient);
                    }
                    else
                        continue;
                }
                lstShiftPatients.AddRange(lstResultList);
                return lstShiftPatients;
            }
            else
                return lstShiftPatients;
        }

        /// <summary>
        /// ���ص�ǰ�������ڶ�Ӧ�Ĳ�����̬�������
        /// </summary>
        /// <param name="lstNursingShiftInfos">�����¼�б�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadWardStatusData(List<NursingShiftInfo> lstNursingShiftInfos)
        {
            this.ClearWardStatus(true);
            if (lstNursingShiftInfos == null || lstNursingShiftInfos.Count <= 0)
                return false;

            if (SystemContext.Instance.LoginUser == null)
                return false;

            //��������������Ϣ���еĶ�Ӧ��ϵ�ŵ���ϣ����
            Dictionary<string, DataTableViewRow> dictShiftRow = new Dictionary<string, DataTableViewRow>();

            foreach (NursingShiftInfo nursingShiftInfo in lstNursingShiftInfos)
            {
                if (nursingShiftInfo == null)
                    continue;
                DataTableViewRow row = this.GetWardStatusTableRow(nursingShiftInfo.ShiftRankCode);
                if (row == null)
                    continue;
                row.Tag = nursingShiftInfo;
                if (!dictShiftRow.ContainsKey(nursingShiftInfo.ShiftRecordID))
                    dictShiftRow.Add(nursingShiftInfo.ShiftRecordID, row);
            }

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            List<ShiftWardStatus> lstShiftWardStatus = null;
            short shRet = NurShiftService.Instance.GetShiftWardStatusList(szWardCode, dtShiftDate, ref lstShiftWardStatus);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ���ಡ����̬��Ϣʧ��!");
                return false;
            }
            if (lstShiftWardStatus == null || lstShiftWardStatus.Count <= 0)
                return true;

            for (int index = 0; index < lstShiftWardStatus.Count; index++)
            {
                ShiftWardStatus shiftWardStatus = lstShiftWardStatus[index];
                if (shiftWardStatus == null)
                    continue;

                if (!dictShiftRow.ContainsKey(shiftWardStatus.ShiftRecordID))
                    continue;
                DataTableViewRow row = dictShiftRow[shiftWardStatus.ShiftRecordID];
                //���ݲ�����̬�е�row["colRank"]  ��ȡ��ε�ǰ���
                if (dicConfigImprotant.ContainsKey(shiftWardStatus.ShiftItemName) && dicConfigImprotant[shiftWardStatus.ShiftItemName])
                {
                    ShiftRankInfo shiftRankInfo = row.Cells[this.colRank.Index].Tag as ShiftRankInfo;
                    continue;
                }
                DataGridViewColumn column = this.GetWardStatusTableColumn(shiftWardStatus.ShiftItemName);
                if (column != null)
                    row.Cells[column.Index].Value = shiftWardStatus.ShiftItemDesc;
                row.Cells[this.colShiftID.Index].Value = shiftWardStatus.ShiftRecordID;
            }

            this.dgvSpecialPatient.AdjustRowHeight(0, this.dgvSpecialPatient.Rows.Count, 24, 24 * 20);
            return true;
        }     

        /// <summary>
        /// ���ص�ǰ�������ڶ�Ӧ�Ĳ����б�������
        /// </summary>
        /// <param name="lstNursingShiftInfos">�����¼�б�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftPatientList(List<NursingShiftInfo> lstNursingShiftInfos)
        {
            this.dgvShiftPatient.Rows.Clear();

            if (SystemContext.Instance.LoginUser == null)
                return false;

            if (lstNursingShiftInfos == null || lstNursingShiftInfos.Count <= 0)
                return false;
           
            //��������������Ϣ�ŵ���ϣ����
            Dictionary<string, NursingShiftInfo> dictShiftInfo = new Dictionary<string, NursingShiftInfo>();

            foreach (NursingShiftInfo nursingShiftInfo in lstNursingShiftInfos)
            {
                if (nursingShiftInfo == null)
                    continue;
                if (!dictShiftInfo.ContainsKey(nursingShiftInfo.ShiftRecordID))
                    dictShiftInfo.Add(nursingShiftInfo.ShiftRecordID, nursingShiftInfo);
            }

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            List<ShiftPatient> lstShiftPatients = null;
            DateTime now = DateTime.Now;
            //��ȡ�����б���Ϣʱ��ϳ�
            short shRet = NurShiftService.Instance.GetShiftPatientList(szWardCode, dtShiftDate, ref lstShiftPatients);
            DateTime now1 = DateTime.Now;
            TimeSpan span = now1 - now;
            LogManager.Instance.WriteLog(span.TotalMilliseconds.ToString());
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ���ಡ���б�ʧ��!");
                return false;
            }
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return true;

            //�Խ��ಡ���б�,��������ĿΪ�յĲ��˷������
            lstShiftPatients = this.GetOrderedItemShiftPatients(lstShiftPatients);
            
            for (int index = 0; index < lstShiftPatients.Count; index++)
            {
                ShiftPatient shiftPatient = lstShiftPatients[index];
                
                if (!dictShiftInfo.ContainsKey(shiftPatient.ShiftRecordID))
                    continue;

                DataTableViewRow row = this.GetShiftPatientTableRow(shiftPatient);
                if (row == null)
                {
                    int rowIndex = this.dgvShiftPatient.Rows.Add();
                    row = this.dgvShiftPatient.Rows[rowIndex];
                    row.Tag = shiftPatient;
                    row.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                    row.Cells[this.colShiftItemAlias.Index].Value = shiftPatient.ShiftItemAlias;
                    row.Cells[this.colBedCode.Index].Value = shiftPatient.BedCode;
                    row.Cells[this.colPatientName.Index].Value = shiftPatient.PatientName;
                    row.Cells[this.colPatientAge.Index].Value = shiftPatient.PatientAge;
                    PatVisitInfo PatVisitInfo = new PatVisitInfo();
                    PatVisitService.Instance.GetPatVisitInfo(shiftPatient.PatientID, shiftPatient.VisitID, ref PatVisitInfo);
                    row.Cells[this.ColSex.Index].Value = PatVisitInfo.PatientSex;
                    //PatientTable.Instance.GetPatVisit(shiftPatient.PatientID, shiftPatient.VisitID).PatientSex;
                    string diagnosis = shiftPatient.Diagnosis;
                    if (diagnosis.Contains("��"))
                    {
                        string[] array = diagnosis.Split('��');
                        row.Cells[this.colDiagnosis.Index].Value = array[0];
                        row.Cells[this.colTraDiagnosis.Index].Value = array[1];
                    }
                    else
                    {
                        row.Cells[this.colDiagnosis.Index].Value = diagnosis;
                    }
                    row.Cells[this.colVisitTime.Index].Value = shiftPatient.VisitTime;
                    row.Cells[this.colShiftRecordID.Index].Value = shiftPatient.ShiftRecordID;
                    row.Cells[this.colPatID.Index].Value = shiftPatient.PatientID;
                    row.Cells[this.colVisitID.Index].Value = shiftPatient.VisitID;
                    row.Cells[this.colRequestDoctor.Index].Value = shiftPatient.RequestDoctor;
                    row.Cells[this.colParentDoctor.Index].Value = shiftPatient.ParentDoctor;
                    this.dgvShiftPatient.SetRowState(row, RowState.Normal);
                }
                else
                {
                    object itemName = row.Cells[this.colShiftItem.Index].Value;
                    object itemAlias = row.Cells[this.colShiftItemAlias.Index].Value;
                    if (GlobalMethods.Misc.IsEmptyString(itemName))
                    {
                        row.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                        row.Cells[this.colShiftItemAlias.Index].Value = shiftPatient.ShiftItemAlias;
                    }
                    else if (!GlobalMethods.Misc.IsEmptyString(shiftPatient.ShiftItemName))
                    {
                        row.Cells[this.colShiftItem.Index].Value
                            = getMixItemString(itemName.ToString(), shiftPatient.ShiftItemName);
                        row.Cells[this.colShiftItemAlias.Index].Value
                            = getMixItemString(itemAlias.ToString(), shiftPatient.ShiftItemAlias);
                    }

                    object requestDoctor = row.Cells[this.colRequestDoctor.Index].Value;
                    if (GlobalMethods.Misc.IsEmptyString(requestDoctor))
                        row.Cells[this.colRequestDoctor.Index].Value = shiftPatient.RequestDoctor;
                    else
                        row.Cells[this.colRequestDoctor.Index].Value
                              = getMixItemString(requestDoctor.ToString(), shiftPatient.RequestDoctor);

                    object parentDoctor = row.Cells[this.colParentDoctor.Index].Value;
                    if (GlobalMethods.Misc.IsEmptyString(parentDoctor))
                        row.Cells[this.colParentDoctor.Index].Value = shiftPatient.ParentDoctor;
                    else
                        row.Cells[this.colParentDoctor.Index].Value
                              = getMixItemString(parentDoctor.ToString(), shiftPatient.ParentDoctor);
                }

                NursingShiftInfo nursingShiftInfo = dictShiftInfo[shiftPatient.ShiftRecordID];
                string szShiftRankCode = nursingShiftInfo.ShiftRankCode;
                DataGridViewColumn column = this.GetShiftPatientTableColumn(szShiftRankCode);
                if (column != null)
                {
                    row.Cells[column.Index].Tag = nursingShiftInfo;
                    row.Cells[column.Index].Value = this.GetShiftContentForDisplay(shiftPatient);
                }
            }
            this.dgvShiftPatient.AdjustRowHeight(0, this.dgvShiftPatient.Rows.Count, 24, 24 * 20);
            if (!SystemContext.Instance.SystemOption.ShiftItemSort)           
                this.dgvShiftPatient.Sort(new BedCodeComparer());
            //if (SystemContext.Instance.SystemOption.ShiftItemSort)
            //    this.dgvShiftPatient.Sort(new ShiftItemComparer());
            //else
            //    this.dgvShiftPatient.Sort(new BedCodeComparer());
    
            return true;
        }

        #region �Զ�������
        //��ϵ���Ľ�����Ŀ
        private string getMixItemString(string strold, string strnew)
        {
            string szmixed = string.Empty;
            List<string> lstUnionItem = new List<string>();
            string[] strolds = strold.Split(',', '��', '��', '+');
            string[] strnews = strnew.Split(',', '��', '��', '+');

            //���������Ŀ
            foreach (string strItem in strolds)
            {
                if (!lstUnionItem.Contains(strItem))
                    lstUnionItem.Add(strItem);
            }
            foreach (string strItem in strnews)
            {
                if (!lstUnionItem.Contains(strItem))
                    lstUnionItem.Add(strItem);
            }

            //ȥ�ظ�
            for (int i = 0; i < lstUnionItem.Count; i++)  //��ѭ����ѭ���Ĵ���
            {
                for (int j = lstUnionItem.Count - 1; j > i; j--)  //��ѭ���� ��ѭ��һ�αȽϵĴ���
                {
                    if (lstUnionItem[i] == lstUnionItem[j] || string.IsNullOrEmpty(lstUnionItem[j]))
                    {
                        lstUnionItem.RemoveAt(j);
                    }
                }
            }

            List<string> lstUnionItem1 = new List<string>();
            foreach (string strItem in lstUnionItem)
            {
                if (lstUnionItem.Contains("��" + strItem))
                {
                    lstUnionItem1.Add(strItem);
                    continue;
                }
                if (strItem.Contains("��") && lstUnionItem.Contains(strItem.Remove(0, 1)))
                {
                    continue;
                }

                lstUnionItem1.Add(strItem);
            }
            foreach (string strItem in lstUnionItem1)
            {
                if (string.IsNullOrEmpty(szmixed))
                    szmixed = strItem;
                else
                    szmixed += "��" + strItem;
            }
            return szmixed;
        }

        #region ��������
        /// <summary>
        /// �Ƚϴ��Ŵ�С��ʵ�ֽӿ�IComparer   �����ֺ�����
        /// </summary>
        public class BedCodeComparer : IComparer
        {
            #region IComparer Members
            public int Compare(object x, object y)
            {
                DataGridViewRow xRow = x as DataGridViewRow;
                DataGridViewRow yRow = y as DataGridViewRow;
                if (xRow.Cells["colBedCode"].Value == null)
                {
                    if (yRow.Cells["colBedCode"].Value == null)
                        return 0;
                    else
                        return -1;
                }
                else
                {
                    if (yRow.Cells["colBedCode"].Value == null)
                        return 1;
                }
                string szXBedCode = xRow.Cells["colBedCode"].Value.ToString();
                string szYBedCode = yRow.Cells["colBedCode"].Value.ToString();
                int iXBedCode;
                int iYBedCode;
                bool bXIsNumber = int.TryParse(szXBedCode, out iXBedCode);
                bool bYIsNumber = int.TryParse(szYBedCode, out iYBedCode);
                if (bXIsNumber)
                {
                    if (bYIsNumber)
                    {
                        if (iXBedCode > iYBedCode)
                            return 1;
                        else if (iXBedCode < iYBedCode)
                            return -1;
                        else
                            return 0;
                    }
                    else
                        return 1;
                }
                else
                {
                    if (bYIsNumber)
                        return -1;
                    else
                    {
                        int temp = string.Compare(szXBedCode, szYBedCode);
                        if (temp > 0)
                            return -1;
                        else if (temp < 0)
                            return 1;
                    }
                }
                return 0;
            }

            #endregion
        }
        #endregion

        #region ��Ŀ����
        /// <summary>
        /// ������Ŀ��������
        /// </summary>
        public class ShiftItemComparer : IComparer
        {
            #region IComparer Members
            public int Compare(object x, object y)
            {
                DataGridViewRow xRow = x as DataGridViewRow;
                DataGridViewRow yRow = y as DataGridViewRow;
                if (xRow.Cells["colShiftItem"].Value == null)
                {
                    if (yRow.Cells["colShiftItem"].Value == null)
                        return 0;
                    else
                        return -1;
                }
                else
                {
                    if (yRow.Cells["colShiftItem"].Value == null)
                        return 1;
                }
                string szXShiftItem = xRow.Cells["colShiftItem"].Value.ToString();
                if (szXShiftItem.Contains("��"))
                {
                    szXShiftItem = szXShiftItem.Substring(0, szXShiftItem.IndexOf('��'));
                }
                string szYShiftItem = yRow.Cells["colShiftItem"].Value.ToString();
                if (szYShiftItem.Contains("��"))
                {
                    szYShiftItem = szYShiftItem.Substring(0, szYShiftItem.IndexOf('��'));
                }
                string szXBedCode = string.Empty;
                string szYBedCode = string.Empty;
                if (xRow.Cells["colBedCode"].Value == null)
                    szXBedCode = "0";
                else
                    szXBedCode = xRow.Cells["colBedCode"].Value.ToString();
                if (yRow.Cells["colBedCode"].Value == null)
                    szYBedCode = "0";
                else
                    szYBedCode = yRow.Cells["colBedCode"].Value.ToString();
                bool bXEmpty = GlobalMethods.Misc.IsEmptyString(szXShiftItem);
                bool bYEmpty = GlobalMethods.Misc.IsEmptyString(szYShiftItem);
                
                if (bXEmpty && bYEmpty)
                    return 0;
                else if (bXEmpty && !bYEmpty)
                    return 1;
                else if (!bXEmpty && bYEmpty)
                    return -1;

                string szComparer = SystemContext.Instance.ShiftItemComparer;
                int iXIndex, iYIndex;
                if (szComparer.Contains(szXShiftItem))
                {
                    iXIndex = szComparer.IndexOf(szXShiftItem);
                    if (szComparer.Contains(szYShiftItem))
                    {
                        iYIndex = szComparer.IndexOf(szYShiftItem);
                        if (iXIndex > iYIndex)
                            return 1;
                        else if (iXIndex == iYIndex)
                            return 0;
                        else
                            return -1;
                    }
                    else
                        return -1;
                }
                else
                {
                    if (szComparer.Contains(szYShiftItem))
                        return 1;
                    else
                        return 0;
                }
            }

            #endregion
        }
        #endregion

        #region ���ⲡ�˴�������
        /// <summary>
        /// �Ƚϴ��Ŵ�С��ʵ�ֽӿ�IComparer   �����ֺ�����
        /// </summary>
        public class SpecialPatinetBedCodeComparer : IComparer
        {
            #region IComparer Members
            public int Compare(object x, object y)
            {
                DataGridViewRow xRow = x as DataGridViewRow;
                DataGridViewRow yRow = y as DataGridViewRow;
                if (xRow.Cells["ScolBedCode"].Value == null)
                {
                    if (yRow.Cells["ScolBedCode"].Value == null)
                        return 0;
                    else
                        return -1;
                }
                else
                {
                    if (yRow.Cells["ScolBedCode"].Value == null)
                        return 1;
                }
                string szXBedCode = xRow.Cells["ScolBedCode"].Value.ToString();
                string szYBedCode = yRow.Cells["ScolBedCode"].Value.ToString();
                int iXBedCode;
                int iYBedCode;
                bool bXIsNumber = int.TryParse(szXBedCode, out iXBedCode);
                bool bYIsNumber = int.TryParse(szYBedCode, out iYBedCode);
                if (bXIsNumber)
                {
                    if (bYIsNumber)
                    {
                        if (iXBedCode > iYBedCode)
                            return 1;
                        else if (iXBedCode < iYBedCode)
                            return -1;
                        else
                            return 0;
                    }
                    else
                        return 1;
                }
                else
                {
                    if (bYIsNumber)
                        return -1;
                    else
                    {
                        int temp = string.Compare(szXBedCode, szYBedCode);
                        if (temp > 0)
                            return -1;
                        else if (temp < 0)
                            return 1;
                    }
                }
                return 0;
            }

            #endregion
        }
        #endregion

        #endregion

        /// <summary>
        /// ��ȡ��Ҫ�ڱ��Ԫ������ʾ�Ľ�������
        /// </summary>
        /// <param name="shiftPatient">���ಡ����Ϣ������</param>
        /// <returns>string</returns>
        private string GetShiftContentForDisplay(ShiftPatient shiftPatient)
        {
            //����TPR�ı�
            StringBuilder sbTPRText = new StringBuilder();
            if (shiftPatient.ShowValue)
            {
                if (shiftPatient.TemperatureValue > -1)
                {
                    sbTPRText.AppendFormat("T��{0}��", shiftPatient.TemperatureValue);
                }
                if (shiftPatient.PulseValue > -1)
                {
                    if (sbTPRText.Length > 0)
                        sbTPRText.Append("  ");
                    sbTPRText.AppendFormat("P��{0}", shiftPatient.PulseValue);
                }
                if (shiftPatient.BreathValue > -1)
                {
                    if (sbTPRText.Length > 0)
                        sbTPRText.Append("  ");
                    sbTPRText.AppendFormat("R��{0}", shiftPatient.BreathValue);
                }
                if (shiftPatient.VitalTime != shiftPatient.DefaultTime)
                {
                    if (sbTPRText.Length > 0)
                        sbTPRText.Append("  ");
                    sbTPRText.Append(shiftPatient.VitalTime.ToString("HH:mm"));
                }
            }

            //����������Ŀ�ı�
            StringBuilder sbSpecialItemText = new StringBuilder();
            if (shiftPatient.ShowValue)
            {
                if (!string.IsNullOrEmpty(shiftPatient.SpecialItem1) && shiftPatient.SpecialItem1.Length > 4)//������
                {
                    sbSpecialItemText.Append(shiftPatient.SpecialItem1);
                }
                if (!string.IsNullOrEmpty(shiftPatient.SpecialItem2) && shiftPatient.SpecialItem2.Length > 4)//�Ӱ���
                {
                    if (sbSpecialItemText.Length > 0)
                        sbSpecialItemText.Append("  ");
                    sbSpecialItemText.Append(shiftPatient.SpecialItem2);
                }
                if (!string.IsNullOrEmpty(shiftPatient.SpecialItem3))
                {
                    if (sbSpecialItemText.Length > 0)
                        sbSpecialItemText.Append("  ");
                    sbSpecialItemText.Append(shiftPatient.SpecialItem3);
                }
                if (!string.IsNullOrEmpty(shiftPatient.SpecialItem4))
                {
                    if (sbSpecialItemText.Length > 0)
                        sbSpecialItemText.Append("  ");
                    sbSpecialItemText.Append(shiftPatient.SpecialItem4);
                }
                if (!string.IsNullOrEmpty(shiftPatient.SpecialItem5))
                {
                    if (sbSpecialItemText.Length > 0)
                        sbSpecialItemText.Append("  ");
                    sbSpecialItemText.Append(shiftPatient.SpecialItem5);
                }               
            }

            //����Ѫѹ����ʳ��������Ӧ�ı�
            StringBuilder sbOtherText = new StringBuilder();
            if (!string.IsNullOrEmpty(shiftPatient.BloodPressure) && shiftPatient.BloodPressure.Length > 4)
            {
                sbOtherText.AppendFormat("{0}", shiftPatient.BloodPressure);
            }
            if (!string.IsNullOrEmpty(shiftPatient.Diet) && shiftPatient.Diet.Length > 3)
            {
                if (sbOtherText.Length > 0)
                    sbOtherText.Append("  ");
                sbOtherText.Append(shiftPatient.Diet);
            }
            if (!string.IsNullOrEmpty(shiftPatient.AdverseReaction) && shiftPatient.AdverseReaction.Length > 5)
            {
                if (sbOtherText.Length > 0)
                    sbOtherText.Append("  ");
                sbOtherText.Append(shiftPatient.AdverseReaction);
            }

            if (sbOtherText.Length > 0)
            {
                if (sbSpecialItemText.Length > 0)
                    sbSpecialItemText.AppendLine();
                sbSpecialItemText.Append(sbOtherText.ToString());
            }
            
            //���Ӹ��ı����ɽ�������
            bool bHasContent = !string.IsNullOrEmpty(shiftPatient.ShiftContent);
            if (sbTPRText.Length > 0 && (sbSpecialItemText.Length > 0 || bHasContent))
                sbTPRText.AppendLine();
            if (sbSpecialItemText.Length > 0 && bHasContent)
                sbSpecialItemText.AppendLine();
            return string.Concat(sbTPRText, sbSpecialItemText, shiftPatient.ShiftContent);
        }

        /// <summary>
        /// ���ص�ǰ�������ڶ�Ӧ�����н����¼�б�����
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadNursingShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            List<NursingShiftInfo> lstNursingShiftInfos = null;
            short shRet = NurShiftService.Instance.GetNursingShiftInfos(szWardCode, dtShiftDate, ref lstNursingShiftInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ�����¼�б�ʧ��!");
                return false;
            }
            this.LoadWardStatusData(lstNursingShiftInfos);
            this.LoadShiftPatientList(lstNursingShiftInfos);
            return true;
        }

        /// <summary>
        /// ���ص�ǰ�������ڶ�Ӧ���������ⲡ�˽����¼�б�����
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadSpecialShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            SpecialShiftInfo specialShiftInfo = null;
            short shRet = NurShiftService.Instance.GetSpecialShiftInfo(szWardCode, dtShiftDate, ref specialShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK && shRet != SystemConst.ReturnValue.NO_FOUND)
            {
                MessageBoxEx.Show("��ѯ���ⲡ�˽����¼�б�ʧ��!");
                return false;
            }

            this.LoadSpecialShiftPatientList(specialShiftInfo);
            return true;
        }

        /// <summary>
        /// ���ص�ǰ�������ڶ�Ӧ�����ⲡ���б�������
        /// </summary>
        /// <param name="specialShiftInfo">�����¼�б�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadSpecialShiftPatientList(SpecialShiftInfo specialShiftInfo)
        {
            this.dgvSpecialPatient.Rows.Clear();

            if (SystemContext.Instance.LoginUser == null)
                return false;

            if (specialShiftInfo == null)
                return false;

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            List<ShiftSpecialPatient> lstShiftSpecialPatients = null;
            DateTime now = DateTime.Now;
            //��ȡ�����б���Ϣʱ��ϳ�
            short shRet = NurShiftService.Instance.GetShiftSpecialPatientList(szWardCode, dtShiftDate, ref lstShiftSpecialPatients);
            DateTime now1 = DateTime.Now;
            TimeSpan span = now1 - now;
            LogManager.Instance.WriteLog(span.TotalMilliseconds.ToString());
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ���ಡ���б�ʧ��!");
                return false;
            }
            if (lstShiftSpecialPatients == null || lstShiftSpecialPatients.Count <= 0)
                return true;

            //��������������Ϣ�ŵ���ϣ����
            Dictionary<string, SpecialShiftInfo> dictSpecialShiftInfo = new Dictionary<string, SpecialShiftInfo>();

            if (dictSpecialShiftInfo != null && !dictSpecialShiftInfo.ContainsKey(specialShiftInfo.ShiftRecordID))
                dictSpecialShiftInfo.Add(specialShiftInfo.ShiftRecordID, specialShiftInfo);
            
            for (int index = 0; index < lstShiftSpecialPatients.Count; index++)
            {
                ShiftSpecialPatient shiftSpecialPatient = lstShiftSpecialPatients[index];

                if (!dictSpecialShiftInfo.ContainsKey(shiftSpecialPatient.ShiftRecordID))
                    continue;

                DataTableViewRow row = this.GetShiftSpecialPatientTableRow(shiftSpecialPatient);
                int rowIndex = -1;
                if (row == null)
                    rowIndex = this.dgvSpecialPatient.Rows.Add();
                else
                    rowIndex = index;

                StringBuilder strNurseDiet = new StringBuilder();
                strNurseDiet.AppendFormat("{0}", shiftSpecialPatient.NursingClass);
                if (strNurseDiet.Length > 0 && shiftSpecialPatient.Diet.Length > 0)
                    strNurseDiet.AppendLine();
                strNurseDiet.AppendFormat("{0}", shiftSpecialPatient.Diet);
                row = this.dgvSpecialPatient.Rows[rowIndex];
                row.Tag = shiftSpecialPatient;
                row.Cells[this.ScolBedCode.Index].Value = shiftSpecialPatient.BedCode;
                row.Cells[this.ScolID.Index].Value = shiftSpecialPatient.PatientID;
                row.Cells[this.ScolPatientName.Index].Value = shiftSpecialPatient.PatientName;
                row.Cells[this.ScolSex.Index].Value = shiftSpecialPatient.PatientSex;
                row.Cells[this.ScolAge.Index].Value = shiftSpecialPatient.PatientAge;
                if (shiftSpecialPatient.LogDateTime != DateTime.MinValue)
                    row.Cells[this.ScolVisitDeptTime.Index].Value = shiftSpecialPatient.LogDateTime.ToShortDateString();
                row.Cells[this.ScolNurseDiet.Index].Value = strNurseDiet.ToString();
                row.Cells[this.ScolRequestDoctor.Index].Value = shiftSpecialPatient.RequestDoctor;
                row.Cells[this.ScolAllergicDrug.Index].Value = shiftSpecialPatient.AllergicDrug;
                row.Cells[this.ScolAdverseReactionDrug.Index].Value = shiftSpecialPatient.AdverseReactionDrug;
                row.Cells[this.ScolDiagnosis.Index].Value = shiftSpecialPatient.Diagnosis;
                row.Cells[this.ScolOthers.Index].Value = shiftSpecialPatient.OthersInfo;
                row.Cells[this.ScolRemark.Index].Value = shiftSpecialPatient.Remark;
                this.dgvShiftPatient.SetRowState(row, RowState.Normal);
            }
            this.dgvSpecialPatient.AdjustRowHeight(0, this.dgvSpecialPatient.Rows.Count, 24, 24 * 20);
            this.dgvSpecialPatient.Sort(new SpecialPatinetBedCodeComparer());
            return true;
        }

        #region "��ӡ���Ӱ౨��"
        /// <summary>
        /// ���ػ����Ӱ��ӡ����
        /// </summary>
        /// <param name="szReportName">������������</param>
        /// <returns>��������</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_SHIFT;
            ReportTypeInfo reportTypeInfo = new ReportTypeInfo();
            if (string.IsNullOrEmpty(szReportName))
                reportTypeInfo = ReportCache.Instance.GetWardReportType(szApplyEnv);
            else
                reportTypeInfo = ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowError("���౨���ӡ����û������!");
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("���౨���ӡ������������ʧ��!");
                return null;
            }
            return byteTempletData;
        }

        private Heren.Common.Report.ReportExplorerForm GetReportExplorerForm()
        {
            Heren.Common.Report.ReportExplorerForm reportExplorerForm =
                new Heren.Common.Report.ReportExplorerForm();
            reportExplorerForm.WindowState = FormWindowState.Maximized;
            reportExplorerForm.QueryContext +=
                new Heren.Common.Report.QueryContextEventHandler(this.ReportExplorerForm_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new Heren.Common.Report.ExecuteQueryEventHandler(this.ReportExplorerForm_ExecuteQuery);
            reportExplorerForm.NotifyNextReport +=
                new Heren.Common.Report.NotifyNextReportEventHandler(this.ReportExplorerForm_NotifyNextReport);
            return reportExplorerForm;
        }
        #endregion

        private void tooldtpShiftDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadNursingShiftData();
            this.LoadSpecialShiftData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNewShift_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (RightController.Instance.CanCreateShiftRec())
            {
                ShiftRecEditForm shiftRecEditForm = new ShiftRecEditForm();
                shiftRecEditForm.DefaultShiftDate = this.tooldtpShiftDate.Value.Date;
                if (shiftRecEditForm.ShowDialog() == DialogResult.OK)
                    this.LoadNursingShiftData();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPrint_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintShiftRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("��û��Ȩ�޴�ӡ���౨��!");
                return;
            }

            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable tableWardStatus =
                    GlobalMethods.Table.GetDataTable(this.dgvWardStatus, false, 1);
                DataTable tableShiftPatient = new DataTable();
                DataTable tableImportantStatus =
                    GlobalMethods.Table.GetDataTable(this.dgvSpecialPatient, false, 1);
                DataTable dtShiftItem = GetShiftRankTable();
                Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                if (this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible)
                {
                    tableShiftPatient = GlobalMethods.Table.GetDataTable(this.dgvShiftPatient, false, 1);
                    explorerForm.ReportParamData.Add("�Ƿ���ڽ�����Ŀ���ñ���", true);
                }
                else
                {
                    tableShiftPatient = GlobalMethods.Table.GetDataTable(this.dgvShiftPatient,false,true, 1);
                    explorerForm.ReportParamData.Add("�Ƿ���ڽ�����Ŀ���ñ���", false);
                }
                explorerForm.ReportParamData.Add("�Ƿ�����", false);
                explorerForm.ReportParamData.Add("������̬", tableWardStatus);
                explorerForm.ReportParamData.Add("�������б�", dtShiftItem);
                explorerForm.ReportParamData.Add("��������", this.tooldtpShiftDate.Value.Date);
                explorerForm.ReportParamData.Add("������Ϣ", tableShiftPatient);
                explorerForm.ReportParamData.Add("��Ҫ��̬��Ϣ", tableImportantStatus);
                explorerForm.ReportParamData.Add("��ӡ����", null);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private DataTable GetShiftRankTable()
        {
            ShiftRankInfo[] ShiftRankInfos = SystemContext.Instance.GetWardShiftRankList();
            DataTable dtShiftItem = new DataTable();
            dtShiftItem.Columns.Add("RankName");
            dtShiftItem.Columns.Add("RankCode");
            dtShiftItem.Columns.Add("WardCode");
            dtShiftItem.Columns.Add("WardName");
            dtShiftItem.Columns.Add("StartPoint");
            dtShiftItem.Columns.Add("StartTime");
            dtShiftItem.Columns.Add("EndPoint");
            dtShiftItem.Columns.Add("EndTime");
            dtShiftItem.Rows.Clear();
            for (int index = 0; index < ShiftRankInfos.Length; index++)
            {
                dtShiftItem.Rows.Add(ShiftRankInfos[index].RankName, ShiftRankInfos[index].RankCode
                    , ShiftRankInfos[index].WardCode, ShiftRankInfos[index].WardName
                , ShiftRankInfos[index].StartPoint, ShiftRankInfos[index].StartTime
                , ShiftRankInfos[index].EndPoint, ShiftRankInfos[index].EndTime);
            }
            return dtShiftItem;
        }

        private void dgvWardStatus_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataGridViewColumn column = this.dgvWardStatus.Columns[e.ColumnIndex];
            DataTableViewRow row = this.dgvWardStatus.Rows[e.RowIndex];

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftRecEditForm shiftRecEditForm = new ShiftRecEditForm();
            ShiftRankInfo shiftRankInfo = row.Cells[this.colRank.Index].Tag as ShiftRankInfo;
            if (shiftRankInfo != null)
                shiftRecEditForm.DefaultRankCode = shiftRankInfo.RankCode;
            shiftRecEditForm.DefaultShiftDate = this.tooldtpShiftDate.Value.Date;
            shiftRecEditForm.DefaultShiftPatient = null;

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            List<NursingShiftInfo> lstNursingShiftInfos = null;
            short shRet = NurShiftService.Instance.GetNursingShiftInfos(szWardCode, dtShiftDate, ref lstNursingShiftInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ�����¼�б�ʧ��!");
                return ;
            }
            if (lstNursingShiftInfos.Count > 2)
                shiftRecEditForm.NursingShiftInfo = this.GetWardStatusTableShiftInfo(row);
            else
                shiftRecEditForm.NursingShiftInfo = null;

            shiftRecEditForm.ShowDialog();
            if (shiftRecEditForm.RecordUpdated)
                this.LoadNursingShiftData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            if (e.Name == "��������")
            {
                e.Success = true;
                e.Value = this.tooldtpShiftDate.Value.Date;
                return;
            }
            object value = e.Value;
            e.Success = SystemContext.Instance.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }

        private void ReportExplorerForm_ExecuteQuery(object sender, Heren.Common.Report.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void ReportExplorerForm_NotifyNextReport(object sender, Heren.Common.Report.NotifyNextReportEventArgs e)
        {
            e.ReportData = this.GetReportFileData(e.ReportName);
        }

        private void dgvShiftPatient_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            DataGridViewCell cell = this.dgvShiftPatient.CurrentCell;
            string szRankCode = string.Empty;
            ShiftPatient shiftPatient = null;
            if (cell != null && cell.ColumnIndex >= 0)
            {
                DataTableViewRow row = this.dgvShiftPatient.Rows[cell.RowIndex];
                if (row != null)
                    shiftPatient = row.Tag as ShiftPatient;

                DataGridViewColumn column = this.dgvShiftPatient.Columns[cell.ColumnIndex];
                ShiftRankInfo shiftRankInfo = column.Tag as ShiftRankInfo;
                if (shiftRankInfo != null)
                    szRankCode = shiftRankInfo.RankCode;
            }

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftRecEditForm shiftRecEditForm = new ShiftRecEditForm();
            shiftRecEditForm.DefaultRankCode = szRankCode;
            shiftRecEditForm.DefaultShiftDate = this.tooldtpShiftDate.Value.Date;
            shiftRecEditForm.DefaultShiftPatient = shiftPatient;
            shiftRecEditForm.NursingShiftInfo = this.GetShiftPatientTableShiftInfo(cell);
            shiftRecEditForm.ShowDialog();
            if (shiftRecEditForm.RecordUpdated)
                this.LoadNursingShiftData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnShiftSetItemAlias_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (RightController.Instance.CanCreateShiftRec())
            {
                ShiftItemAliasForm shiftItemAliasForm = new ShiftItemAliasForm();
                shiftItemAliasForm.ShowDialog();
                this.EditcolShiftItemVisible();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNewSpecial_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (RightController.Instance.CanCreateShiftRec())
            {
                ShiftSpecialEditForm shiftSpecialEditForm = new ShiftSpecialEditForm();
                shiftSpecialEditForm.ShiftDate = this.tooldtpShiftDate.Value.Date;
                if (shiftSpecialEditForm.ShowDialog() == DialogResult.OK)
                    this.LoadSpecialShiftData();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPrintSpecial_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintShiftRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("��û��Ȩ�޴�ӡ���ⲡ�˲��齻�౨��!");
                return;
            }

            byte[] byteReportData = this.GetReportFileData("���ⲡ�˲��齻�Ӱ�");
            if (byteReportData != null)
            {
                DataTable tableSpecialPatient =
                    GlobalMethods.Table.GetDataTable(this.dgvSpecialPatient, false, 1);
                Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("��������", this.tooldtpShiftDate.Value.Date);
                explorerForm.ReportParamData.Add("���ⲡ�˽�����Ϣ", tableSpecialPatient);
                explorerForm.ReportParamData.Add("��ӡ����", null);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvSpecialPatient_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataGridViewCell cell = this.dgvSpecialPatient.CurrentCell;

            ShiftSpecialPatient shiftSpecialPatient = null;
            if (cell != null && cell.ColumnIndex >= 0)
            {
                DataTableViewRow row = this.dgvSpecialPatient.Rows[cell.RowIndex];
                if (row != null)
                    shiftSpecialPatient = row.Tag as ShiftSpecialPatient;

                DataGridViewColumn column = this.dgvSpecialPatient.Columns[cell.ColumnIndex];
            }

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftSpecialEditForm shiftSpecialEditForm = new ShiftSpecialEditForm();
            shiftSpecialEditForm.ShiftDate= this.tooldtpShiftDate.Value.Date;
            shiftSpecialEditForm.DefaultShiftSpecialPatient = shiftSpecialPatient;
            shiftSpecialEditForm.ShowDialog();
            if (shiftSpecialEditForm.RecordUpdated)
                this.LoadSpecialShiftData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}