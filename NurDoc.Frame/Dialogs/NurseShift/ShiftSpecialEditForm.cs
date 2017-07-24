// ***********************************************************
// ������Ӳ���ϵͳ,���Ӱ����ⲡ�����ⲡ���������޸Ĵ���.
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
    internal partial class ShiftSpecialEditForm : HerenForm
    {
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

        private ShiftSpecialPatient m_defaultShiftSpecialPatient = null;

        /// <summary>
        ///  ��ȡ������Ĭ��ѡ�еĽ��ಡ��
        /// </summary>
        [Browsable(false)]
        public ShiftSpecialPatient DefaultShiftSpecialPatient
        {
            get { return this.m_defaultShiftSpecialPatient; }
            set { this.m_defaultShiftSpecialPatient = value; }
        }

        private DateTime m_ShiftDate = DateTime.Now;

        /// <summary>
        /// ��ȡ�����õ�ǰ���ⲡ�˲��齻�Ӱ��Ӧ��ʱ��
        /// </summary>
        public DateTime ShiftDate
        {
            get { return this.m_ShiftDate; }
            set { this.m_ShiftDate = value; }
        }

        /// <summary>
        /// ��ʶ��ǰ�����Ƿ����½���
        /// </summary>
        private bool m_bIsNewSpecialShift = true;

        /// <summary>
        /// ��ȡ��ǰ�����Ƿ����½���
        /// </summary>
        public bool IsNewSpecialShift
        {
            get { return this.m_bIsNewSpecialShift; }
        }

        /// <summary>
        /// ��ʶ���ⲡ�˽�������¼��Ϣ
        /// </summary>
        private SpecialShiftInfo m_specialShiftInfo = null;

        public ShiftSpecialEditForm()
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

            //���ص�ǰ�ĵ�״̬
            if (this.m_specialShiftInfo == null)
                this.m_specialShiftInfo = this.GetSelectedShiftInfo();
            this.EditState(this.m_specialShiftInfo);

            //���ؽ�����Ϣ�༭��
            this.LoadRecordEditTemplet();

            //���ص�ǰ��������ⲡ�˽����¼
            this.LoadSpecialShiftData();
            
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.dgvShiftPatient.EndEdit();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// ��ȡ�û���ǰѡ��Ľ�����Ϣ
        /// </summary>
        /// <returns>������Ϣ</returns>
        private SpecialShiftInfo GetSelectedShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.m_ShiftDate.Date;
            SpecialShiftInfo specialShiftInfo = null;
            short shRet = NurShiftService.Instance.GetSpecialShiftInfo(szWardCode, dtShiftDate, ref specialShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK
                && shRet != ServerData.ExecuteResult.RES_NO_FOUND)
            {
                MessageBoxEx.Show("��ѯ���ⲡ�˲��齻���¼ʧ��!");
                return null;
            }
            return specialShiftInfo;
        }

        /// <summary>
        /// ��鵱ǰ���ݱ༭�����Ƿ��Ѿ��ı�
        /// </summary>
        /// <returns>�Ƿ���δ�ύ</returns>
        private bool HasUncommit()
        {
            if (this.dgvShiftPatient.CurrentRow != null
                && this.formControl1.IsModified)
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

            DialogResult result = MessageBoxEx.ShowQuestion("��ǰ�������޸�,�Ƿ񱣴棿");
            if (result == DialogResult.Yes)
                return !this.SaveSpecialShiftData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// �Ӱཻ���¼���ݱ༭��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadRecordEditTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_SPECIALPATIENT;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("���ⲡ�˽�����Ϣ�༭����û������!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("���ⲡ�˽�����Ϣ�༭������ʧ��!");
                return false;
            }
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// ���ݵ�ǰ�����¼������Ϣ�жϵ�ǰ�ĵ�״̬������/�޸ģ�
        /// </summary>
        /// <param name="specialShiftInfo">�����¼</param>
        /// <returns>�Ƿ�Ϊ����</returns>
        private bool EditState(SpecialShiftInfo specialShiftInfo)
        {
            if (this.CheckModifyState())
            {
                return false;
            }
            if (specialShiftInfo == null)
            {
                this.Text = "�༭���ⲡ�˲��齻��(����)";
                this.m_bIsNewSpecialShift = true;
                this.m_specialShiftInfo = this.CreateSpecialShiftInfo();
                SpecialShiftInfo specialShiftInfoDB = new SpecialShiftInfo();
                short shRet = NurShiftService.Instance.GetSpecialShiftInfo(
                    this.m_specialShiftInfo.WardCode,
                    this.m_specialShiftInfo.ShiftRecordDate,
                    ref specialShiftInfoDB);
                if (shRet != ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    MessageBoxEx.Show("�����������ݿ������иð�����ݣ��޷������������´�ҳ�档");
                    this.m_bRecordUpdated = true;//ʹ���Ӱ�������ˢ�����ݡ�
                    this.Close();
                }
            }
            else
            {
                this.Text = "�༭���ⲡ�˲��齻��(�޸�)";
                this.m_bIsNewSpecialShift = false;
                this.m_specialShiftInfo = specialShiftInfo;
            }
            return true;
        }

        /// <summary>
        /// ���ݵ�ǰ�����¼������Ϣ���ص��ν�������
        /// </summary>
        /// <param name="specialShiftInfo">�����¼</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadSpecialShiftData()
        {
            if (!this.m_bIsNewSpecialShift)
            {
                this.LoadShiftSpecialPatientList(this.m_specialShiftInfo);
            }
            else
            {
                this.LoadShiftSpecialPatientList(null);
                //������½���,��ô���ݹ������Ĭ�ϲ���
                ShiftSpecialPatient[] arrShiftSpecialPatient = null;
                ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
                shiftRecFilterForm.IsSpecialPatient = true;
                shiftRecFilterForm.SpecialShiftInfo = this.m_specialShiftInfo;
                if (shiftRecFilterForm.LoadFilterTemplet())
                {
                    arrShiftSpecialPatient = shiftRecFilterForm.GetSelectedSpecialPatientList();
                }
                shiftRecFilterForm.Dispose();
                this.LoadShiftSpecialPatientList(arrShiftSpecialPatient, true);
            }
            return true;
        }

        /// <summary>
        /// ����ָ�������¼�����Ĳ����б�����
        /// </summary>
        /// <param name="specialShiftInfo">�����¼</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftSpecialPatientList(SpecialShiftInfo specialShiftInfo)
        {
            this.dgvShiftPatient.Focus();
            this.dgvShiftPatient.Rows.Clear();
            this.ShowSelectedSpecialShiftPatient();
            this.Update();
            if (specialShiftInfo == null)
                return true;

            string szShiftRecordID = specialShiftInfo.ShiftRecordID;
            List<ShiftSpecialPatient> lstShiftSpecialPatients = null;
            short shRet = NurShiftService.Instance.GetShiftSpecialPatientList(szShiftRecordID, ref lstShiftSpecialPatients);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ���ⲡ�˲��齻���б�ʧ��!");
                return false;
            }
            if (lstShiftSpecialPatients == null || lstShiftSpecialPatients.Count <= 0)
                return true;

            //lstShiftPatients = this.GetOrderedShiftPatients(lstShiftPatients);
            bool success = this.LoadShiftSpecialPatientList(lstShiftSpecialPatients.ToArray(), false);
            this.dgvShiftPatient.SelectRow(this.GetShiftPatientTableRow(this.m_defaultShiftSpecialPatient));
            return success;
        }

        /// <summary>
        /// ���ָ���Ľ��ಡ���б����ಡ�˱����
        /// </summary>
        /// <param name="arrShiftSpecialPatients">���ಡ���б�</param>
        /// <param name="append">�Ƿ���׷��</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftSpecialPatientList(ShiftSpecialPatient[] arrShiftSpecialPatients, bool append)
        {
            if (arrShiftSpecialPatients == null || arrShiftSpecialPatients.Length <= 0)
                return false;
            foreach (ShiftSpecialPatient shiftSpecialPatient in arrShiftSpecialPatients)
            {
                if (shiftSpecialPatient == null)
                    continue;
                if (append && this.GetShiftPatientTableRow(shiftSpecialPatient) != null)
                    continue;
                int rowIndex = this.dgvShiftPatient.Rows.Add();
                DataTableViewRow row = this.dgvShiftPatient.Rows[rowIndex];
                row.Tag = shiftSpecialPatient;
                row.Cells[this.colBedCode.Index].Value = shiftSpecialPatient.BedCode;
                row.Cells[this.colPatientName.Index].Value = shiftSpecialPatient.PatientName;
                row.Cells[this.colID.Index].Value = shiftSpecialPatient.PatientID;
                this.dgvShiftPatient.SetRowState(row, append ? RowState.New : RowState.Normal);
            }
            this.dgvShiftPatient.Sort(new BedCodeComparer());
            this.ShowSelectedSpecialShiftPatient();
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

        /// <summary>
        /// ���ؽ����������ݵ��������ݱ༭������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool ShowSelectedSpecialShiftPatient()
        {
            this.UpdateSelectedSpecialShiftPatient();

            if (this.dgvShiftPatient.CurrentRow == null)
            {
                this.formControl1.Tag = null;
                this.formControl1.UpdateFormData("ˢ����ͼ", null);
                this.formControl1.IsModified = false;
                return false;
            }

            ShiftSpecialPatient shiftSpecialPatient = this.dgvShiftPatient.CurrentRow.Tag as ShiftSpecialPatient;
            if (shiftSpecialPatient != null)
                shiftSpecialPatient.ShiftDate = this.m_ShiftDate;
            this.formControl1.Tag = shiftSpecialPatient;
            DataTable table = GlobalMethods.Table.GetDataTable(shiftSpecialPatient);
            this.formControl1.UpdateFormData("ˢ����ͼ", table);

            object Visible = this.formControl1.GetFormData("���Ӱ��Ƿ�ɱ༭");
            if (Visible != null)
            {
                this.btnAddPatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.btnDeletePatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.dgvShiftPatient.ReadOnly = (Visible is bool) ? !(bool)Visible : true;
                this.toolbtnSave.Enabled = (Visible is bool) ? (bool)Visible : false;
            }

            this.formControl1.IsModified = false;
            return true;
        }

        /// <summary>
        /// ��ȡָ���Ľ��ಡ�˶�Ӧ�Ľ��ಡ�˱����
        /// </summary>
        /// <param name="shiftSpecialPatient">���ಡ����Ϣ</param>
        /// <returns>���ಡ�˱����</returns>
        private DataTableViewRow GetShiftPatientTableRow(ShiftSpecialPatient shiftSpecialPatient)
        {
            if (shiftSpecialPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftSpecialPatient patient = currentRow.Tag as ShiftSpecialPatient;
            if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                && patient.VisitID == shiftSpecialPatient.VisitID
                && patient.SubID == shiftSpecialPatient.SubID)
                return currentRow;

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftSpecialPatient;
                if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                    && patient.VisitID == shiftSpecialPatient.VisitID
                    && patient.SubID == shiftSpecialPatient.SubID)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// ����һ���µ����ⲡ�˲��齻������¼
        /// </summary>
        /// <returns>���ⲡ�˲�������¼</returns>
        private SpecialShiftInfo CreateSpecialShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string strShiftDate = this.m_ShiftDate.ToShortDateString();
            DateTime dtShiftDate = DateTime.Now;
            GlobalMethods.Convert.StringToDate(strShiftDate, ref dtShiftDate);
           
            return NursingShiftHandler.Instance.Create(SystemContext.Instance.LoginUser, dtShiftDate, SysTimeService.Instance.Now);
        }

        /// <summary>
        /// ���浱ǰ���޸ĵ����ⲡ�˲��齻������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool SaveSpecialShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_specialShiftInfo == null)
            {
                MessageBoxEx.Show("��������¼��ϢΪ��,�޷�����!");
                return false;
            }
            //�����ж��Ƿ�ӵ��Ȩ��
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo = null;
            if (!RightController.Instance.CanEditShiftRec(nursingShiftInfo))
                return false;

            short shRet = SystemConst.ReturnValue.OK;

            UserInfo userInfo = SystemContext.Instance.LoginUser;
            this.m_specialShiftInfo.ModifierID = userInfo.ID;
            this.m_specialShiftInfo.ModifierName = userInfo.Name;
            this.m_specialShiftInfo.ModifyTime = SysTimeService.Instance.Now;

            DateTime dtShiftDate = this.m_ShiftDate.Date;
            this.m_specialShiftInfo.ShiftRecordDate = dtShiftDate;

            //���ð�ν����������Ƿ��ظ�
            SpecialShiftInfo specialShiftInfoDB = null;
            shRet = NurShiftService.Instance.GetSpecialShiftInfo(
               this.m_specialShiftInfo.WardCode,
               this.m_specialShiftInfo.ShiftRecordDate,
               ref specialShiftInfoDB);
            if (shRet != ServerData.ExecuteResult.RES_NO_FOUND
                && specialShiftInfoDB.ShiftRecordID != this.m_specialShiftInfo.ShiftRecordID)
            {
                MessageBoxEx.Show("�����������ݿ������иð�����ݣ��޷������������´�ҳ�档");
                this.m_bRecordUpdated = true;//ʹ���Ӱ�������ˢ�����ݡ�
                //�����Ƿ񱣴���ʾ��
                this.formControl1.IsModified = false;
                this.Close();
            }

            //���潻�ಡ���б�
            this.UpdateSelectedSpecialShiftPatient();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (this.dgvShiftPatient.IsNormalRow(row))
                    continue;
                ShiftSpecialPatient shiftSpecialPatient = row.Tag as ShiftSpecialPatient;
                if (shiftSpecialPatient == null)
                    continue;
                shiftSpecialPatient.PatientNo = row.Index;
                shiftSpecialPatient.ShiftRecordID = this.m_specialShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftSpecialPatient(shiftSpecialPatient);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("���潻�ಡ����Ϣʧ��!");
                    return false;
                }
                this.dgvShiftPatient.SetRowState(row, RowState.Normal);
            }
            this.formControl1.IsModified = false;

            //�������ⲡ�˲��齻����������¼
            shRet = NurShiftService.Instance.SaveSpecialShiftInfo(this.m_specialShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("���潻������¼��Ϣʧ��!");
                return false;
            }

            this.m_bRecordUpdated = true;
            return true;
        }

        /// <summary>
        /// ���༭�����ⲡ�˲��齻�����ݸ��µ����ⲡ�˲��齻��������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool UpdateSelectedSpecialShiftPatient()
        {
            if (!this.dgvShiftPatient.EndEdit())
                return false;
            if (!this.formControl1.EndEdit())
                return false;

            ShiftSpecialPatient shiftSpecialPatient = this.formControl1.Tag as ShiftSpecialPatient;
            if (shiftSpecialPatient == null)
                return false;

            DataTableViewRow row = this.GetShiftPatientTableRow(shiftSpecialPatient);
            if (row == null)
                return false;

            if (this.formControl1.IsModified)
            {
                if (this.dgvShiftPatient.IsNormalRowUndeleted(row))
                    this.dgvShiftPatient.SetRowState(row, RowState.Update);
            }

            DataTable table = this.formControl1.GetFormData("������Ϣ") as DataTable;
            if (table == null || table.Rows.Count <= 0)
                return false;

            ShiftSpecialPatient shiftSpecialPatientModify = new ShiftSpecialPatient();
            shiftSpecialPatientModify.FromDataRow(table.Rows[0]);

            shiftSpecialPatient.BedCode = shiftSpecialPatientModify.BedCode;
            shiftSpecialPatient.PatientSex = shiftSpecialPatientModify.PatientSex;
            shiftSpecialPatient.PatientAge = shiftSpecialPatientModify.PatientAge;
            shiftSpecialPatient.Diagnosis = shiftSpecialPatientModify.Diagnosis;
            
            if (shiftSpecialPatientModify.LogDateTime != DateTime.MinValue)
                shiftSpecialPatient.LogDateTime = shiftSpecialPatientModify.LogDateTime.Date;
            shiftSpecialPatient.NursingClass = shiftSpecialPatientModify.NursingClass;
            shiftSpecialPatient.Diet = shiftSpecialPatientModify.Diet;
            shiftSpecialPatient.RequestDoctor = shiftSpecialPatientModify.RequestDoctor;
            shiftSpecialPatient.AllergicDrug = shiftSpecialPatientModify.AllergicDrug;
            shiftSpecialPatient.AdverseReactionDrug = shiftSpecialPatientModify.AdverseReactionDrug;
            shiftSpecialPatient.OthersInfo = shiftSpecialPatientModify.OthersInfo;
            shiftSpecialPatient.Remark = shiftSpecialPatientModify.Remark;
            return true;
        }

        /// <summary>
        /// ��ȡ��ǰ���ಡ�˱������ʾ�Ľ��ಡ������
        /// </summary>
        /// <returns>���ಡ������</returns>
        private ShiftSpecialPatient[] GetShiftSpecialPatientTableData()
        {
            List<ShiftSpecialPatient> lstShiftSpecialPatients = new List<ShiftSpecialPatient>();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                ShiftSpecialPatient patient = row.Tag as ShiftSpecialPatient;
                if (patient != null)
                    lstShiftSpecialPatients.Add(patient);
            }
            return lstShiftSpecialPatients.ToArray();
        }

        /// <summary>
        /// ��ȡָ�������ⲡ�˶�Ӧ�����ⲡ�˽�������
        /// </summary>
        /// <param name="shiftPatient">���ಡ����Ϣ</param>
        /// <returns>���ಡ�˱����</returns>
        private DataTableViewRow GetShiftSpecialPatientTableRow(ShiftSpecialPatient shiftSpecialPatient)
        {
            if (shiftSpecialPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftSpecialPatient patient = currentRow.Tag as ShiftSpecialPatient;
            if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                && patient.VisitID == shiftSpecialPatient.VisitID
                && patient.SubID == shiftSpecialPatient.SubID)
                return currentRow;

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftSpecialPatient;
                if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                    && patient.VisitID == shiftSpecialPatient.VisitID
                    && patient.SubID == shiftSpecialPatient.SubID)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// ɾ�����ಡ�˱��е�ǰѡ�еĽ��ಡ��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool DeleteSelectedShiftSpecialPatient()
        {
            DataTableViewRow row = this.dgvShiftPatient.CurrentRow;
            if (row == null)
                return false;
            //�����ж��Ƿ�Ե�ǰģ���б༭Ȩ��
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo = null;
            if (!RightController.Instance.CanEditShiftRec(nursingShiftInfo))
                return false;
            DialogResult result = MessageBoxEx.ShowConfirm("ȷ��ɾ����ǰѡ�еĲ�����");
            if (result != DialogResult.OK)
                return false;
            if (row.State == RowState.New)
            {
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedSpecialShiftPatient();
                return true;
            }
            ShiftSpecialPatient shiftSpecialPatient = row.Tag as ShiftSpecialPatient;
            if (shiftSpecialPatient == null)
                return false;
            string szRecordID = shiftSpecialPatient.ShiftRecordID;
            string szPatientID = shiftSpecialPatient.PatientID;
            string szVisitID = shiftSpecialPatient.VisitID;
            string szSubID = shiftSpecialPatient.SubID;

            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            short shRet = NurShiftService.Instance.DeleteShiftSpecialPatient(szRecordID
                , szPatientID, szVisitID, szSubID);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                this.m_bRecordUpdated = true;
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedSpecialShiftPatient();
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
        private bool DeleteAllShiftSpecialPatient()
        {
            this.dgvShiftPatient.EndEdit();
            if (this.dgvShiftPatient.Rows.Count <= 0)
                return false;
            //�����ж��Ƿ�Ե�ǰģ���б༭Ȩ��
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo = null;
            if (!RightController.Instance.CanEditShiftRec(nursingShiftInfo))
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
                    ShiftSpecialPatient shiftSpecialPatient = row.Tag as ShiftSpecialPatient;
                    if (shiftSpecialPatient == null)
                        return false;
                    string szRecordID = shiftSpecialPatient.ShiftRecordID;
                    string szPatientID = shiftSpecialPatient.PatientID;
                    string szVisitID = shiftSpecialPatient.VisitID;
                    string szSubID = shiftSpecialPatient.SubID;

                    short shRet = NurShiftService.Instance.DeleteShiftSpecialPatient(szRecordID
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
            this.ShowSelectedSpecialShiftPatient();
            this.m_bRecordUpdated = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
            
            shiftRecFilterForm.IsSpecialPatient = true;
            shiftRecFilterForm.IsbtnAddPatientClick = true;
            shiftRecFilterForm.SelectedSpecialPatients = this.GetShiftSpecialPatientTableData();
            if (shiftRecFilterForm.ShowDialog() == DialogResult.OK)
                this.LoadShiftSpecialPatientList(shiftRecFilterForm.SelectedSpecialPatients, true);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnDeletePatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedShiftSpecialPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);            
        }

        private void btnDeleteAllPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteAllShiftSpecialPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvShiftPatient_SelectionChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowSelectedSpecialShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveSpecialShiftData())
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
    }
}