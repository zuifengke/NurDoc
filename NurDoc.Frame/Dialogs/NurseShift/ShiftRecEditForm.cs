// ***********************************************************
// ������Ӳ���ϵͳ,���Ӱ��������޸Ĵ���.
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

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class ShiftRecEditForm : HerenForm
    {
        /// <summary>
        /// ���Ӱ��б�ָ��߳ߴ�
        /// </summary>
        private const string NDS_Shift_Edit_Split2_Distance = "NdsShiftEditSplit2Distance";

        /// <summary>
        /// ���Ӱ���Ŀ���
        /// </summary>
        private const string NDS_Shift_Edit_Item_Width = "NdsShiftEditItemWidth";

        /// <summary>
        /// ���Ӱല�ſ��
        /// </summary>
        private const string NDS_Shift_Edit_BedCode_Width = "NdsShiftEditBedCodeWidth";

        /// <summary>
        /// ���Ӱ��������
        /// </summary>
        private const string NDS_Shift_Edit_Name_Width = "NdsShiftEditNameWidth";

        private NursingShiftInfo m_nursingShiftInfo = null;

        /// <summary>
        ///  ��ȡ�����ý�������¼��Ϣ
        /// </summary>
        [Browsable(false)]
        public NursingShiftInfo NursingShiftInfo
        {
            get { return this.m_nursingShiftInfo; }
            set { this.m_nursingShiftInfo = value; }
        }

        private ShiftPatient m_defaultShiftPatient = null;

        /// <summary>
        ///  ��ȡ������Ĭ��ѡ�еĽ��ಡ��
        /// </summary>
        [Browsable(false)]
        public ShiftPatient DefaultShiftPatient
        {
            get { return this.m_defaultShiftPatient; }
            set { this.m_defaultShiftPatient = value; }
        }

        private DateTime? m_defaultShiftDate = null;

        /// <summary>
        ///  ��ȡ������Ĭ����ʾ�Ľ���ʱ��
        /// </summary>
        [Browsable(false)]
        public DateTime? DefaultShiftDate
        {
            get { return this.m_defaultShiftDate; }
            set { this.m_defaultShiftDate = value; }
        }

        private string m_defaultRankCode = string.Empty;

        /// <summary>
        ///  ��ȡ������Ĭ����ʾ�Ľ�����
        /// </summary>
        [Browsable(false)]
        public string DefaultRankCode
        {
            get { return this.m_defaultRankCode; }
            set { this.m_defaultRankCode = value; }
        }

        private bool m_bIsNewNursingShift = true;

        /// <summary>
        /// ��ȡ��ǰ�����Ƿ����½���
        /// </summary>
        public bool IsNewNursingShift
        {
            get { return this.m_bIsNewNursingShift; }
        }

        /// <summary>
        /// ��ʶ��ǰ�༭�������Ƿ�����ݿ��и��¹�
        /// </summary>
        private bool m_bRecordUpdated = false;

        /// <summary>
        /// ��ȡ��ǰ�༭�������Ƿ�����ݿ��и��¹�
        /// </summary>
        public bool RecordUpdated
        {
            get { return this.m_bRecordUpdated; }
        }

        /// <summary>
        /// ��ʶ�ؼ�ֵ�ı��¼��Ƿ����,�����ظ�ִ��
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        public ShiftRecEditForm()
        {
            this.InitializeComponent();
            this.btnAddPatient.Image = Properties.Resources.Add;
            this.btnDeletePatient.Image = Properties.Resources.Delete;
            this.btnDeleteAllPatient.Visible = false;
            this.btnDeleteAllPatient.Image = Properties.Resources.Delete;
            this.Icon = Properties.Resources.ShiftIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;

            //����Ĭ����ʾ�Ľ�������
            if (this.m_defaultShiftDate == null || !this.m_defaultShiftDate.HasValue)
                this.tooldtpShiftDate.Value = SysTimeService.Instance.Now.Date;
            else
                this.tooldtpShiftDate.Value = this.m_defaultShiftDate.Value.Date;

            //���ؽ��ಡ�˱���н�����Ŀ�����б�
            this.Update();
            this.LoadShiftItemList();

            //���ؽ����������б�
            this.Update();
            this.LoadShiftRankList();

            //���ص�ǰ�ĵ�״̬
            if (this.m_nursingShiftInfo == null)
                this.m_nursingShiftInfo = this.GetSelectedShiftInfo();
            this.EditState(this.m_nursingShiftInfo);

            //���ز�����̬�༭��
            this.Update();
            this.LoadWardStatusTemplet();

            //���ؽ�����Ϣ�༭��
            this.LoadRecordEditTemplet();

            //���ص�ǰ����Ľ����¼
            this.LoadNursingShiftData();

            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.dgvShiftPatient.EndEdit();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// ��鵱ǰ���ݱ༭�����Ƿ��Ѿ��ı�
        /// </summary>
        /// <returns>�Ƿ���δ�ύ</returns>
        private bool HasUncommit()
        {
            if (this.formControl1.IsModified)
                return true;
            if (this.dgvShiftPatient.CurrentRow != null
                && this.formControl2.IsModified)
                return true;
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (row.IsModifiedRow || row.IsNewRow)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ��鲢���浱ǰ���ݱ༭���ڵ��޸�
        /// </summary>
        /// <returns>�Ƿ�ȡ��</returns>
        private bool CheckModifyState()
        {
            if (!this.HasUncommit())
                return false;

            DialogResult result = MessageBoxEx.ShowQuestion("��ǰ�����������޸�,�Ƿ񱣴棿");
            if (result == DialogResult.Yes)
                return !this.SaveNursingShiftData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// ��鲢����this.toolcboShiftRank.SelectedIndexChanged/tooldtpShiftDate_ValueChangedǰ�༭���ڵ��޸�
        /// </summary>
        /// <returns>�Ƿ�ȡ��</returns>
        private bool CheckLastModifyState()
        {
            if (!this.HasUncommit())
                return false;

            DialogResult result = MessageBoxEx.ShowQuestion("��ǰ�����������޸�,�Ƿ񱣴棿");
            if (result == DialogResult.Yes)
                return !this.SaveLastNursingShiftData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// ���ؽ����������б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftRankList()
        {
            this.toolcboShiftRank.Items.Clear();
            this.toolcboShiftRank.Items.AddRange(SystemContext.Instance.GetWardShiftRankList());

            object selectedItem = null;
            foreach (object item in this.toolcboShiftRank.Items)
            {
                ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                if (shiftRankInfo.RankCode == this.m_defaultRankCode)
                    selectedItem = item;
            }
            if (selectedItem != null)
                this.toolcboShiftRank.SelectedItem = selectedItem;
            else
                this.toolcboShiftRank.SelectedItem = this.GetCurrentShiftRank();
            return true;
        }

        /// <summary>
        /// ���ز�����̬��Ϣ�༭��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadWardStatusTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_WARD_STATUS;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("������̬�༭����û������!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("������̬�༭������ʧ��!");
                return false;
            }
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// �Ӱཻ���¼���ݱ༭��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadRecordEditTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_PATIENT;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("������Ϣ�༭����û������!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("������Ϣ�༭������ʧ��!");
                return false;
            }
            return this.formControl2.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// ���ݵ�ǰ�����¼������Ϣ�жϵ�ǰ�ĵ�״̬������/�޸ģ�
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        /// <returns>�Ƿ�Ϊ����</returns>
        private bool EditState(NursingShiftInfo nursingShiftInfo)
        {
            if (this.CheckModifyState())
            {
                return false;
            }
            if (nursingShiftInfo == null)
            {
                this.Text = "�༭�����¼(����)";
                this.m_bIsNewNursingShift = true;
                this.m_nursingShiftInfo = this.CreateNursingShiftInfo();
                NursingShiftInfo nursingShiftInfoDB = new NursingShiftInfo();
                short shRet = NurShiftService.Instance.GetNursingShiftInfo(
                    this.m_nursingShiftInfo.WardCode,
                    this.m_nursingShiftInfo.ShiftRecordDate,
                    this.m_nursingShiftInfo.ShiftRankCode,
                    ref nursingShiftInfoDB);
                if (shRet != ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    MessageBoxEx.Show("�����������ݿ������иð�����ݣ��޷������������´�ҳ�档");
                    this.m_bRecordUpdated = true;//ʹ���Ӱ�������ˢ�����ݡ�
                    this.Close();
                }
            }
            else
            {
                this.Text = "�༭�����¼(�޸�)";
                this.m_bIsNewNursingShift = false;
                this.m_nursingShiftInfo = nursingShiftInfo;
            }
            return true;
        }

        /// <summary>
        /// ���ݵ�ǰ�����¼������Ϣ���ص��ν�������
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadNursingShiftData()
        {
            this.ShowShiftDateAndRank(this.m_nursingShiftInfo);
            if (!this.m_bIsNewNursingShift)
            {
                this.LoadWardStatusData(this.m_nursingShiftInfo);
                this.LoadShiftPatientList(this.m_nursingShiftInfo);
            }
            else
            {
                this.LoadWardStatusData(null);
                this.LoadShiftPatientList(null);
                //������½���,��ô���ݹ������Ĭ�ϲ���
                ShiftPatient[] arrShiftPatients = null;
                ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
                ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
                shiftRecFilterForm.ShiftRankInfo = shiftRankInfo;
                shiftRecFilterForm.NursingShiftInfo = this.m_nursingShiftInfo;
                if (shiftRecFilterForm.LoadFilterTemplet())
                    arrShiftPatients = shiftRecFilterForm.GetSelectedPatientList();
                shiftRecFilterForm.Dispose();
                this.LoadShiftPatientList(arrShiftPatients, true);
            }
            return true;
        }

        /// <summary>
        /// ����ָ�������¼��Ӧ�Ĳ�����̬�������
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadWardStatusData(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
            {
                this.formControl1.UpdateFormData("ˢ����ͼ", null);
                if (!this.m_bIsNewNursingShift)
                    this.formControl1.IsModified = false;
                return true;
            }
            string szShiftID = nursingShiftInfo.ShiftRecordID;
            List<ShiftWardStatus> lstShiftWardStatus = null;
            short shRet = NurShiftService.Instance.GetShiftWardStatusList(szShiftID, ref lstShiftWardStatus);
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
                if (shiftWardStatus != null)
                    this.formControl1.UpdateFormData("ˢ����ͼ", shiftWardStatus.ToDataTable());
            }
            if (!this.m_bIsNewNursingShift)
                this.formControl1.IsModified = false;
            return true;
        }

        /// <summary>
        /// ����ָ�������¼�����Ĳ����б�����
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftPatientList(NursingShiftInfo nursingShiftInfo)
        {
            this.dgvShiftPatient.Focus();
            this.dgvShiftPatient.Rows.Clear();
            this.ShowSelectedShiftPatient();
            this.Update();
            if (nursingShiftInfo == null)
                return true;

            string szShiftRecordID = nursingShiftInfo.ShiftRecordID;
            List<ShiftPatient> lstShiftPatients = null;
            short shRet = NurShiftService.Instance.GetShiftPatientList(szShiftRecordID, ref lstShiftPatients);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ���ಡ���б�ʧ��!");
                return false;
            }
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return true;

            lstShiftPatients = this.GetOrderedShiftPatients(lstShiftPatients);
            lstShiftPatients = this.GetOrderedShiftPatientsByAlias(lstShiftPatients);
            bool success = this.LoadShiftPatientList(lstShiftPatients.ToArray(), false);
            this.dgvShiftPatient.SelectRow(this.GetShiftPatientTableRow(this.m_defaultShiftPatient));
            return success;
        }

        /// <summary>
        /// ���ָ���Ľ��ಡ���б����ಡ�˱����
        /// </summary>
        /// <param name="arrShiftPatients">���ಡ���б�</param>
        /// <param name="append">�Ƿ���׷��</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftPatientList(ShiftPatient[] arrShiftPatients, bool append)
        {
            if (arrShiftPatients == null || arrShiftPatients.Length <= 0)
                return false;
            foreach (ShiftPatient shiftPatient in arrShiftPatients)
            {
                if (shiftPatient == null)
                    continue;
                if (append && this.GetShiftPatItemTableRow(shiftPatient) != null)
                    continue;
                int rowIndex = this.dgvShiftPatient.Rows.Add();
                DataTableViewRow row = this.dgvShiftPatient.Rows[rowIndex];
                row.Tag = shiftPatient;
                if (shiftPatient.SpecialItem2.Equals("1"))
                {
                    row.Cells[this.colNewPatient.Index].Value = picBoxBlue.Image;
                }
                else {
                    row.Cells[this.colNewPatient.Index].Value = picBoxWhite.Image;
                }
                row.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                row.Cells[this.colShiftItemAlias.Index].Value = shiftPatient.ShiftItemAlias;
                row.Cells[this.colBedCode.Index].Value = shiftPatient.BedCode;
                row.Cells[this.colPatientName.Index].Value = shiftPatient.PatientName;
                this.dgvShiftPatient.SetRowState(row, append ? RowState.New : RowState.Normal);
            }
            this.dgvShiftPatient.Sort(new BedCodeComparer());
            //this.dgvShiftPatient.Sort(dgvShiftPatient.Columns[1], ListSortDirection.Ascending);
            this.ShowSelectedShiftPatient();
            return true;
        }

        #region �Զ�������
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

        private ComboBox m_comboBox = new ComboBox();

        /// <summary>
        /// ���ؽ��ಡ�˱���еĽ�����Ŀ�����б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftItemList()
        {
            this.colShiftItem.Items.Clear();
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<ShiftItemAliasInfo> lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            short shRet = NurShiftService.Instance.GetShiftItemAliasInfos(szWardCode, ref lstShiftItemAliasInfos);
            if (shRet == SystemConst.ReturnValue.OK && lstShiftItemAliasInfos.Count > 0)
            {
                this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible = true;
                this.colShiftItemAlias.Items.Add("");
                this.m_comboBox.Items.Add("");
                foreach (ShiftItemAliasInfo shiftItemAliasInfo in lstShiftItemAliasInfos)
                {
                    this.m_comboBox.Items.Add(shiftItemAliasInfo.ItemName);
                    this.colShiftItemAlias.Items.Add(shiftItemAliasInfo.ItemAlias);
                }
                this.colShiftItem.Items.AddRange(SystemContext.Instance.GetWardShiftItemList());
            }
            else
            {
                this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible = false;
                this.colShiftItem.Items.AddRange(SystemContext.Instance.GetWardShiftItemList());
            }
            return true;
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
        /// ��ָ���Ľ��ಡ���б�������Ŀ����˳���������
        /// </summary>
        /// <param name="lstShiftPatients">���ಡ���б�</param>
        /// <returns>����Ľ��ಡ���б�</returns>
        private List<ShiftPatient> GetOrderedShiftPatientsByAlias(List<ShiftPatient> lstShiftPatients)
        {
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return lstShiftPatients;

            List<ShiftPatient> lstResultList = new List<ShiftPatient>();
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<ShiftItemAliasInfo> lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            short shRet = NurShiftService.Instance.GetShiftItemAliasInfos(szWardCode, ref lstShiftItemAliasInfos);
            foreach (ShiftItemAliasInfo shiftItemAliasInfo in lstShiftItemAliasInfos)
            {
                int index = 0;
                while (index < lstShiftPatients.Count)
                {
                    ShiftPatient shiftPatient = lstShiftPatients[index++];
                    if (string.IsNullOrEmpty(shiftPatient.ShiftItemName))
                        continue;
                    if (shiftPatient.ShiftItemAlias.StartsWith(shiftItemAliasInfo.ItemAlias))
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
        /// ��ȡ��������б��е�ǰѡ�еĽ�����
        /// </summary>
        /// <returns>������</returns>
        private ShiftRankInfo GetSelectedShiftRank()
        {
            ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            if (shiftRankInfo != null)
                shiftRankInfo.UpdateShiftRankTime(this.tooldtpShiftDate.Value);
            return shiftRankInfo;
        }

        /// <summary>
        /// ���ݵ�ǰʱ����㲢���ؽ�����
        /// </summary>
        /// <returns>������</returns>
        private ShiftRankInfo GetCurrentShiftRank()
        {
            if (this.toolcboShiftRank.Items.Count <= 0)
                return null;
            DateTime dtShiftTime = SysTimeService.Instance.Now;
            foreach (object item in this.toolcboShiftRank.Items)
            {
                ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                shiftRankInfo.UpdateShiftRankTime(dtShiftTime.Date);
                if (dtShiftTime >= shiftRankInfo.StartTime && dtShiftTime < shiftRankInfo.EndTime)
                    return shiftRankInfo;
            }
            return this.toolcboShiftRank.Items[0] as ShiftRankInfo;
        }

        /// <summary>
        /// ��ʾָ�������¼��Ӧ�Ľ����κ�����
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        private void ShowShiftDateAndRank(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
                return;
            DateTime dtShiftDate = nursingShiftInfo.ShiftRecordDate.Date;
            if (this.tooldtpShiftDate.Value != dtShiftDate)
                this.tooldtpShiftDate.Value = dtShiftDate;

            ShiftRankInfo selectedRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            if (selectedRankInfo == null || selectedRankInfo.RankCode != nursingShiftInfo.ShiftRankCode)
            {
                foreach (object item in this.toolcboShiftRank.Items)
                {
                    ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                    if (shiftRankInfo.RankCode == nursingShiftInfo.ShiftRankCode)
                    {
                        this.toolcboShiftRank.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ��ǰ���ಡ�˱������ʾ�Ľ��ಡ������
        /// </summary>
        /// <returns>���ಡ������</returns>
        private ShiftPatient[] GetShiftPatientTableData()
        {
            List<ShiftPatient> lstShiftPatients = new List<ShiftPatient>();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                ShiftPatient patient = row.Tag as ShiftPatient;
                if (patient != null)
                    lstShiftPatients.Add(patient);
            }
            return lstShiftPatients.ToArray();
        }

        /// <summary>
        /// ��ȡָ���Ľ��ಡ�˶�Ӧ�Ľ��ಡ�˱����
        /// </summary>
        /// <param name="shiftPatient">���ಡ����Ϣ</param>
        /// <returns>���ಡ�˱����</returns>
        private DataTableViewRow GetShiftPatientTableRow(ShiftPatient shiftPatient)
        {
            if (shiftPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftPatient patient = currentRow.Tag as ShiftPatient;
            if (patient != null && patient.PatientID == shiftPatient.PatientID
                && patient.VisitID == shiftPatient.VisitID
                && patient.SubID == shiftPatient.SubID)
                return currentRow;

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftPatient;
                if (patient != null && patient.PatientID == shiftPatient.PatientID
                    && patient.VisitID == shiftPatient.VisitID
                    && patient.SubID == shiftPatient.SubID)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ���Ľ��ಡ�˶�Ӧ�Ľ��ಡ�˱����(����������Ŀ)
        /// </summary>
        /// <param name="shiftPatient">���ಡ����Ϣ</param>
        /// <returns>���ಡ�˱����</returns>
        private DataTableViewRow GetShiftPatItemTableRow(ShiftPatient shiftPatient)
        {
            if (shiftPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftPatient patient = currentRow.Tag as ShiftPatient;
            if (patient != null && patient.PatientID == shiftPatient.PatientID
                && patient.VisitID == shiftPatient.VisitID
                && patient.SubID == shiftPatient.SubID
                )
            {
                if (patient.ShiftItemName != shiftPatient.ShiftItemName)
                {
                    currentRow.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                    this.dgvShiftPatient.SetRowState(currentRow, RowState.Update);                  
                }
                return currentRow;
            }

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftPatient;
                if (patient != null && patient.PatientID == shiftPatient.PatientID
                    && patient.VisitID == shiftPatient.VisitID
                    && patient.SubID == shiftPatient.SubID)
                {
                    if (patient.ShiftItemName != shiftPatient.ShiftItemName)
                    {
                        row.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                        this.dgvShiftPatient.SetRowState(row, RowState.Update);
                    }
                    return row;
                }
            }
            return null;
        }

        /// <summary>
        /// ����һ���µĻ���������¼
        /// </summary>
        /// <returns>����������¼</returns>
        private NursingShiftInfo CreateNursingShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string strShiftDate = string.Format("{0} {1}", this.tooldtpShiftDate.Value.ToShortDateString(),
                this.toollblRankTime.Text.Substring(0, this.toollblRankTime.Text.IndexOf(" ")));
            int iSRSelectedIndex = this.toolcboShiftRank.SelectedIndex;
            DateTime dtShiftDate = DateTime.Now;
            GlobalMethods.Convert.StringToDate(strShiftDate, ref dtShiftDate);
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return null;

            return NursingShiftHandler.Instance.Create(SystemContext.Instance.LoginUser, dtShiftDate, SysTimeService.Instance.Now, shiftRankInfo, iSRSelectedIndex);
        }

        /// <summary>
        /// ���༭�Ľ������ݸ��µ����ಡ�˱������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool UpdateSelectedShiftPatient()
        {
            if (!this.dgvShiftPatient.EndEdit())
                return false;
            if (!this.formControl2.EndEdit())
                return false;

            ShiftPatient shiftPatient = this.formControl2.Tag as ShiftPatient;
            if (shiftPatient == null)
                return false;

            DataTableViewRow row = this.GetShiftPatientTableRow(shiftPatient);
            if (row == null)
                return false;

            shiftPatient.ShiftItemName =
                row.Cells[this.colShiftItem.Index].Value as string;
            shiftPatient.ShiftItemAlias =
               row.Cells[this.colShiftItemAlias.Index].Value as string;

            if (this.formControl2.IsModified)
            {
                if (this.dgvShiftPatient.IsNormalRowUndeleted(row))
                    this.dgvShiftPatient.SetRowState(row, RowState.Update);
            }

            DataTable table = this.formControl2.GetFormData("������Ϣ") as DataTable;
            if (table == null || table.Rows.Count <= 0)
                return false;

            ShiftPatient shiftPatientModify = new ShiftPatient();
            shiftPatientModify.FromDataRow(table.Rows[0]);

            shiftPatient.BedCode = shiftPatientModify.BedCode;
            shiftPatient.Diagnosis = shiftPatientModify.Diagnosis;
            shiftPatient.ShowValue = shiftPatientModify.ShowValue;
            shiftPatient.SpecialItem1 = shiftPatientModify.SpecialItem1;
            shiftPatient.SpecialItem2 = shiftPatientModify.SpecialItem2;
            shiftPatient.SpecialItem3 = shiftPatientModify.SpecialItem3;
            shiftPatient.SpecialItem4 = shiftPatientModify.SpecialItem4;
            shiftPatient.SpecialItem5 = shiftPatientModify.SpecialItem5;
            shiftPatient.ShiftContent = shiftPatientModify.ShiftContent;
            shiftPatient.PulseValue = shiftPatientModify.PulseValue;
            shiftPatient.BreathValue = shiftPatientModify.BreathValue;
            shiftPatient.TemperatureType = shiftPatientModify.TemperatureType;
            shiftPatient.TemperatureValue = shiftPatientModify.TemperatureValue;
            shiftPatient.VitalTime = shiftPatientModify.VitalTime;
            shiftPatient.BloodPressure = shiftPatientModify.BloodPressure;
            shiftPatient.Diet = shiftPatientModify.Diet;
            shiftPatient.AdverseReaction = shiftPatientModify.AdverseReaction;
            shiftPatient.RequestDoctor = shiftPatientModify.RequestDoctor;
            shiftPatient.ParentDoctor = shiftPatientModify.ParentDoctor;
            return true;
        }

        /// <summary>
        /// ���ؽ����������ݵ��������ݱ༭������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool ShowSelectedShiftPatient()
        {
            this.UpdateSelectedShiftPatient();

            if (this.dgvShiftPatient.CurrentRow == null)
            {
                this.formControl2.Tag = null;
                this.formControl2.UpdateFormData("ˢ����ͼ", null);
                this.formControl2.IsModified = false;
                return false;
            }

            ShiftPatient shiftPatient = this.dgvShiftPatient.CurrentRow.Tag as ShiftPatient;
            if (shiftPatient != null)
                shiftPatient.ShiftDate = this.tooldtpShiftDate.Value;
            this.formControl2.Tag = shiftPatient;
            DataTable table = GlobalMethods.Table.GetDataTable(shiftPatient);
            this.formControl2.UpdateFormData("ˢ����ͼ", table);

            object Visible = this.formControl2.GetFormData("���Ӱ��Ƿ�ɱ༭");
            if (Visible != null)
            {
                this.btnAddPatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.btnDeletePatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.dgvShiftPatient.ReadOnly = (Visible is bool) ? !(bool)Visible : true;
                this.formControl1.Enabled = (Visible is bool) ? (bool)Visible : false;
                this.toolbtnSave.Enabled = (Visible is bool) ? (bool)Visible : false;
            }

            this.formControl2.IsModified = false;
            return true;
        }

        /// <summary>
        /// ɾ�����ಡ�˱��е�ǰѡ�еĽ��ಡ��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool DeleteSelectedShiftPatient()
        {
            DataTableViewRow row = this.dgvShiftPatient.CurrentRow;
            if (row == null)
                return false;
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;
            DialogResult result = MessageBoxEx.ShowConfirm("ȷ��ɾ����ǰѡ�еĲ�����");
            if (result != DialogResult.OK)
                return false;
            if (row.State == RowState.New)
            {
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedShiftPatient();
                return true;
            }
            ShiftPatient shiftPatient = row.Tag as ShiftPatient;
            if (shiftPatient == null)
                return false;
            string szRecordID = shiftPatient.ShiftRecordID;
            string szPatientID = shiftPatient.PatientID;
            string szVisitID = shiftPatient.VisitID;
            string szSubID = shiftPatient.SubID;

            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            short shRet = NurShiftService.Instance.DeleteShiftPatient(szRecordID
                , szPatientID, szVisitID, szSubID);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                this.m_bRecordUpdated = true;
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedShiftPatient();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }
            else
            {
                MessageBoxEx.Show("ɾ�����ಡ�˼�¼ʧ��!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
        }


        /// <summary>
        /// ɾ�����ಡ�˱������еĽ��ಡ��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool DeleteAllShiftPatient()
        {
            this.dgvShiftPatient.EndEdit();
            if (this.dgvShiftPatient.Rows.Count <= 0)
                return false;
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;
            DialogResult result = MessageBoxEx.ShowConfirm("ȷ��ɾ�����в�����");
            if (result != DialogResult.OK)
                return false;
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            while (this.dgvShiftPatient.Rows.Count > 0)
            {
                DataTableViewRow row = this.dgvShiftPatient.Rows[0];
                if (row.State == RowState.New)
                    this.dgvShiftPatient.Rows.Remove(row);
                else
                {
                    ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                    if (shiftPatient == null)
                        return false;
                    string szRecordID = shiftPatient.ShiftRecordID;
                    string szPatientID = shiftPatient.PatientID;
                    string szVisitID = shiftPatient.VisitID;
                    string szSubID = shiftPatient.SubID;

                    short shRet = NurShiftService.Instance.DeleteShiftPatient(szRecordID
                        , szPatientID, szVisitID, szSubID);
                    if (shRet == SystemConst.ReturnValue.OK)
                        this.dgvShiftPatient.Rows.Remove(row);
                    else
                    {
                        MessageBoxEx.Show("ɾ�����ಡ�˼�¼ʧ��!");
                        GlobalMethods.UI.SetCursor(this, Cursors.Default);
                        return false;
                    }
                }
            }
            this.ShowSelectedShiftPatient();
            this.m_bRecordUpdated = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// ��ȡ�û���ǰѡ������ںͰ�ζ�Ӧ�Ľ�����Ϣ
        /// </summary>
        /// <returns>������Ϣ</returns>
        private NursingShiftInfo GetSelectedShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return null;
            string szRankCode = shiftRankInfo.RankCode;
            NursingShiftInfo nursingShiftInfo = null;
            short shRet = NurShiftService.Instance.GetNursingShiftInfo(szWardCode, dtShiftDate
                , szRankCode, ref nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK
                && shRet != ServerData.ExecuteResult.RES_NO_FOUND)
            {
                MessageBoxEx.Show("��ѯ��������¼ʧ��!");
                return null;
            }
            return nursingShiftInfo;
        }

        /// <summary>
        /// ��ȡ����ʱ������̬��Ϣ�б�
        /// </summary>
        /// <returns>������̬��Ϣ�б�</returns>
        public List<ShiftWardStatus> GetShiftWardStatusList()
        {
            if (!this.formControl1.EndEdit())
                return null;

            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            DataTable table = this.formControl1.GetFormData("������̬") as DataTable;
            if (table == null)
                return lstShiftWardStatus;
            foreach (DataRow row in table.Rows)
            {
                ShiftWardStatus shiftWardStatus = new ShiftWardStatus();
                if (shiftWardStatus.FromDataRow(row))
                    lstShiftWardStatus.Add(shiftWardStatus);
            }
            return lstShiftWardStatus;
        }

        /// <summary>
        /// ���浱ǰ���޸ĵĲ�����̬�����ಡ�˵Ƚ�������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool SaveNursingShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_nursingShiftInfo == null)
            {
                MessageBoxEx.Show("��������¼��ϢΪ��,�޷�����!");
                return false;
            }
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;

            short shRet = SystemConst.ReturnValue.OK;

            UserInfo userInfo = SystemContext.Instance.LoginUser;
            this.m_nursingShiftInfo.ModifierID = userInfo.ID;
            this.m_nursingShiftInfo.ModifierName = userInfo.Name;
            this.m_nursingShiftInfo.ModifyTime = SysTimeService.Instance.Now;

            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo != null)
                this.m_nursingShiftInfo.ShiftRankCode = shiftRankInfo.RankCode;

            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            this.m_nursingShiftInfo.ShiftRecordDate = dtShiftDate;

            //���ð�ν����������Ƿ��ظ�
            NursingShiftInfo nursingShiftInfoDB = null;
            shRet = NurShiftService.Instance.GetNursingShiftInfo(
               this.m_nursingShiftInfo.WardCode,
               this.m_nursingShiftInfo.ShiftRecordDate,
               this.m_nursingShiftInfo.ShiftRankCode,
               ref nursingShiftInfoDB);
            if (shRet != ServerData.ExecuteResult.RES_NO_FOUND
                && nursingShiftInfoDB.ShiftRecordID != this.m_nursingShiftInfo.ShiftRecordID)
            {
                MessageBoxEx.Show("�����������ݿ������иð�����ݣ��޷������������´�ҳ�档");
                this.m_bRecordUpdated = true;//ʹ���Ӱ�������ˢ�����ݡ�
                //�����Ƿ񱣴���ʾ��
                this.formControl1.IsModified = false;
                this.formControl2.IsModified = false;
                this.Close();
            }

            //���潻�ಡ���б�
            this.UpdateSelectedShiftPatient();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (this.dgvShiftPatient.IsNormalRow(row))
                    continue;
                ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                if (shiftPatient == null)
                    continue;
                shiftPatient.PatientNo = row.Index;
                shiftPatient.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                if (shiftPatient.SpecialItem2 == null || shiftPatient.SpecialItem2.Equals(""))
                    shiftPatient.SpecialItem2 = "0";
                shRet = NurShiftService.Instance.SaveShiftPatient(shiftPatient);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("���潻�ಡ����Ϣʧ��!");
                    return false;
                }
                this.dgvShiftPatient.SetRowState(row, RowState.Normal);
            }
            this.formControl2.IsModified = false;

            //���潻�ಡ����̬
            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            if (this.formControl1.IsModified || this.m_bIsNewNursingShift)
                lstShiftWardStatus = this.GetShiftWardStatusList();
            foreach (ShiftWardStatus shiftWardStatus in lstShiftWardStatus)
            {
                shiftWardStatus.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftWardStatus(shiftWardStatus);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("���潻�ಡ����̬ʧ��!");
                    return false;
                }
            }
            this.formControl1.IsModified = false;

            //���潻����������¼
            shRet = NurShiftService.Instance.SaveNursingShiftInfo(this.m_nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("���潻������¼��Ϣʧ��!");
                return false;
            }

            this.m_bRecordUpdated = true;
            return true;
        }

        /// <summary>
        /// ����this.toolcboShiftRank.SelectedIndexChanged/tooldtpShiftDate_ValueChangedǰ���޸ĵĲ�����̬�����ಡ�˵Ƚ�������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool SaveLastNursingShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_nursingShiftInfo == null)
            {
                MessageBoxEx.Show("��������¼��ϢΪ��,�޷�����!");
                return false;
            }
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;

            short shRet = SystemConst.ReturnValue.OK;

            UserInfo userInfo = SystemContext.Instance.LoginUser;
            this.m_nursingShiftInfo.ModifierID = userInfo.ID;
            this.m_nursingShiftInfo.ModifierName = userInfo.Name;
            this.m_nursingShiftInfo.ModifyTime = SysTimeService.Instance.Now;


            //���ð�ν����������Ƿ��ظ�
            NursingShiftInfo nursingShiftInfoDB = null;
            shRet = NurShiftService.Instance.GetNursingShiftInfo(
               this.m_nursingShiftInfo.WardCode,
               this.m_nursingShiftInfo.ShiftRecordDate,
               this.m_nursingShiftInfo.ShiftRankCode,
               ref nursingShiftInfoDB);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
            {
                //���潻����������¼
                shRet = NurShiftService.Instance.SaveNursingShiftInfo(this.m_nursingShiftInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("���潻������¼��Ϣʧ��!");
                    return false;
                }
                this.m_bRecordUpdated = true;
            }

            //���潻�ಡ���б�
            this.UpdateSelectedShiftPatient();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (this.dgvShiftPatient.IsNormalRow(row))
                    continue;
                ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                if (shiftPatient == null)
                    continue;
                shiftPatient.PatientNo = row.Index;
                shiftPatient.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftPatient(shiftPatient);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("���潻�ಡ����Ϣʧ��!");
                    return false;
                }
                this.dgvShiftPatient.SetRowState(row, RowState.Normal);
            }
            this.formControl2.IsModified = false;

            //���潻�ಡ����̬
            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            if (this.formControl1.IsModified || this.m_bIsNewNursingShift)
                lstShiftWardStatus = this.GetShiftWardStatusList();
            foreach (ShiftWardStatus shiftWardStatus in lstShiftWardStatus)
            {
                shiftWardStatus.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftWardStatus(shiftWardStatus);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("���潻�ಡ����̬ʧ��!");
                    return false;
                }
            }
            this.formControl1.IsModified = false;

            return true;
        }

        private void tooldtpShiftDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;

            this.dgvShiftPatient.EndEdit();
            this.CheckLastModifyState();
            this.formControl1.IsModified = false;
            this.formControl2.IsModified = false;
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
                row.State = RowState.Normal;

            this.EditState(this.GetSelectedShiftInfo());
            this.LoadNursingShiftData();
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboShiftRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return;
            this.toollblRankTime.Text = string.Format("{0} - {1}"
                , shiftRankInfo.StartPoint, shiftRankInfo.EndPoint);

            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;

            this.dgvShiftPatient.EndEdit();
            this.CheckLastModifyState();
            this.formControl1.IsModified = false;
            this.formControl2.IsModified = false;
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
                row.State = RowState.Normal;

            this.EditState(this.GetSelectedShiftInfo());
            this.LoadNursingShiftData();
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveNursingShiftData())
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            if (this.m_bRecordUpdated)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            shiftRecFilterForm.ShiftRankInfo = shiftRankInfo;
            shiftRecFilterForm.IsbtnAddPatientClick = true;
            shiftRecFilterForm.SelectedPatients = this.GetShiftPatientTableData();
            if (shiftRecFilterForm.ShowDialog() == DialogResult.OK)
                this.LoadShiftPatientList(shiftRecFilterForm.SelectedPatients, true);
            //this.dgvShiftPatient.Sort(new BedCodeComparer());
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnDeletePatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnDeleteAllPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteAllShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvShiftPatient_SelectionChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowSelectedShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void FormControl_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            if (e.Name == "���½���")
            {
                e.Value = this.m_bIsNewNursingShift;
                e.Success = true;
            }
            else if (e.Name == "��������")
            {
                e.Value = this.tooldtpShiftDate.Value;
                e.Success = true;
            }
            else if (e.Name == "����ID��")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.ShiftRecordID;
                e.Success = true;
            }
            else if (e.Name == "������")
            {
                ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
                if (shiftRankInfo != null)
                    e.Value = shiftRankInfo.ToDataTable();
                e.Success = true;
            }
            else if (e.Name == "������ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.CreatorID;
                e.Success = true;
            }
            else if (e.Name == "����������")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.CreatorName;
                e.Success = true;
            }
            else if (e.Name == "���ಡ����")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.WardName;
                e.Success = true;
            }
            else if (e.Name == "���ಡ��ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.WardCode;
                e.Success = true;
            }
            else if (e.Name == "�޸���ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.ModifierID;
                e.Success = true;
            }
            else if (e.Name == "�޸�������")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.ModifierName;
                e.Success = true;
            }
            else if (e.Name == "��һǩ����ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.FirstSignID;
                e.Success = true;
            }
            else if (e.Name == "��һǩ����ID����")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.FirstSignName;
                e.Success = true;
            }
            else if (e.Name == "��һǩ��ʱ��")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.FirstSignTime;
                e.Success = true;
            }
            if (e.Name == "ǰһ��δ���")
            {
                if (this.toolcboShiftRank.SelectedIndex == 0)
                {
                    ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.Items[2] as ShiftRankInfo;
                    if (shiftRankInfo != null)
                        shiftRankInfo.StartTime = this.tooldtpShiftDate.Value.AddDays(-1);

                    e.Value = GlobalMethods.Table.GetDataTable(shiftRankInfo);
                }
                else
                {
                    ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.Items[this.toolcboShiftRank.SelectedIndex - 1] as ShiftRankInfo;
                    if (shiftRankInfo != null)
                        shiftRankInfo.StartTime = this.tooldtpShiftDate.Value;

                    e.Value = GlobalMethods.Table.GetDataTable(shiftRankInfo);
                }
            }
        }

        private void ShiftRecEditForm_Load(object sender, EventArgs e)
        {
            string szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_Split2_Distance, string.Empty);
            int iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.splitContainer2.SplitterDistance = iDistance;
            else
                this.splitContainer2.SplitterDistance = 290;
            szValue = string.Empty;

            szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_Item_Width, string.Empty);
            iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.colShiftItem.Width = iDistance;
            else
                this.colShiftItem.Width = 60;
            szValue = string.Empty;

            szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_BedCode_Width, string.Empty);
            iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.colBedCode.Width = iDistance;
            else
                this.colBedCode.Width = 40;
            szValue = string.Empty;

            szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_Name_Width, string.Empty);
            iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.colPatientName.Width = iDistance;
            else
                this.colPatientName.Width = 80;
            szValue = string.Empty;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            bool bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_Split2_Distance
                , this.splitContainer2.SplitterDistance.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("���沼��д��ʧ��!");
                return;
            }

            bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_Item_Width
                , this.colShiftItem.Width.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("���沼�ֽ�����Ŀд��ʧ��!");
                return;
            }

            bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_BedCode_Width
                , this.colBedCode.Width.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("���沼�ִ���д��ʧ��!");
                return;
            }

            bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_Name_Width
                , this.colPatientName.Width.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("���沼������д��ʧ��!");
                return;
            }
        }

        private void dgvShiftPatient_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dgvShiftPatient.CurrentCell.OwningColumn.Name == "colShiftItemAlias")
            {
                ComboBox comboBox = (ComboBox)e.Control;
                comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (this.dgvShiftPatient.CurrentCell.OwningColumn.Name == "colShiftItemAlias" && comboBox.SelectedIndex != 0)
            {
                this.dgvShiftPatient.Rows[this.dgvShiftPatient.CurrentRow.Index].Cells[this.colShiftItem.Index].Value = this.m_comboBox.Items[comboBox.SelectedIndex];
            }
        }

        private void toolbtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBoxEx.ShowQuestion("�Ƿ�ɾ����ǰ������н�����Ϣ");
            if (result != DialogResult.Yes)
                return;

            short shRet = SystemConst.ReturnValue.OK;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            //��ȡ��ǰ���������Ϣ
            shRet = NurShiftService.Instance.GetNursingShiftInfo(szWardCode, dtShiftDate, shiftRankInfo.RankCode, ref nursingShiftInfo);
            if (shRet == SystemConst.ReturnValue.NO_FOUND)
            {
                MessageBoxEx.Show("��ǰ��β����ڽ�����Ϣ!");
                return;
            }
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ȡ��ǰ��ν���������Ϣʧ��!");
                return;
            }

            //ɾ�����ಡ����Ϣ
            string szRecordID = "";
            string szPatientID = "";
            string szVisitID = "";
            string szSubID = "";
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (row.State != RowState.New)
                {
                    ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                    if (shiftPatient == null)
                        continue;
                    szRecordID = shiftPatient.ShiftRecordID;
                    szPatientID = shiftPatient.PatientID;
                    szVisitID = shiftPatient.VisitID;
                    szSubID = shiftPatient.SubID;
                    this.Update();
                    GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                    shRet = NurShiftService.Instance.DeleteShiftPatient(szRecordID
                        , szPatientID, szVisitID, szSubID);
                    if (shRet != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.Show("ɾ����ǰ��ν��ಡ�˼�¼ʧ��!");
                        GlobalMethods.UI.SetCursor(this, Cursors.Default);
                        return;
                    }
                }
            }

            szRecordID = nursingShiftInfo.ShiftRecordID;
            if (!string.IsNullOrEmpty(szRecordID))
            {
                //ɾ��������̬��Ϣ
                shRet = NurShiftService.Instance.DeleteShiftWardStatusInfo(szRecordID);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("ɾ����ǰ��ν��ද̬��Ϣʧ��!");
                    return;
                }

                //ɾ������������Ϣ
                shRet = NurShiftService.Instance.DeleteShiftIndexInfo(szRecordID, szWardCode);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("ɾ����ǰ��ν��ಡ��������Ϣʧ��!");
                    return;
                }
            }

            MessageBox.Show("ɾ����ǰ������н�����Ϣ�ɹ���");
            this.Close();
            this.m_bRecordUpdated = true;
        }
    }
}