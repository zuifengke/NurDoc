// ***********************************************************
// ������Ӳ���ϵͳ,���Ӱ�ģ�鲡��ɸѡ����.
// Creator:YangMingkun  Date:2013-7-12
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Controls;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class ShiftRecFilterForm : HerenForm
    {
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

        private ShiftRankInfo m_shiftRankInfo = null;

        /// <summary>
        /// ���ý�������¼��Ϣ
        /// </summary>
        [Browsable(false)]
        public ShiftRankInfo ShiftRankInfo
        {
            //get { return this.m_shiftRankInfo; }
            set { this.m_shiftRankInfo = value; }
        }

        private ShiftPatient[] m_selectedPatients = null;

        /// <summary>
        /// ��ȡ�û���ѡ�Ĳ����б�
        /// </summary>
        [Browsable(false)]
        public ShiftPatient[] SelectedPatients
        {
            get { return this.m_selectedPatients; }
            set { this.m_selectedPatients = value; }
        }

        private SpecialShiftInfo m_specialShiftInfo = null;

        /// <summary>
        ///  ��ȡ���������ⲡ�˲��齻������¼��Ϣ
        /// </summary>
        [Browsable(false)]
        public SpecialShiftInfo SpecialShiftInfo
        {
            get { return this.m_specialShiftInfo; }
            set { this.m_specialShiftInfo = value; }
        }

        private ShiftSpecialPatient[] m_selectedSpecialPatients = null;

        /// <summary>
        /// ��ȡ�û���ѡ�����ⲡ���б�
        /// </summary>
        [Browsable(false)]
        public ShiftSpecialPatient[] SelectedSpecialPatients
        {
            get { return this.m_selectedSpecialPatients; }
            set { this.m_selectedSpecialPatients = value; }
        }

        /// <summary>
        /// ��ʶ��ǰ�����Ƿ����ⲡ�˽������
        /// </summary>
        private bool m_bIsSpecialPatient = false;

        /// <summary>
        /// ��ȡ�����õ�ǰ�����Ƿ����ⲡ�˽������
        /// </summary>
        [Browsable(false)]
        public bool IsSpecialPatient
        {
            get { return this.m_bIsSpecialPatient; }
            set { this.m_bIsSpecialPatient = value; }
        }

        /// <summary>
        /// ��ʶ��ǰ�Ƿ�ͨ���������˰�ť����ɸѡ���˹���
        /// </summary>
        private bool m_bIsbtnAddPatientClick = false;

        /// <summary>
        /// ��ȡ�����õ�ǰ�Ƿ�ͨ���������˰�ť����ɸѡ���˹���
        /// </summary>
        [Browsable(false)]
        public bool IsbtnAddPatientClick
        {
            get { return this.m_bIsbtnAddPatientClick; }
            set { this.m_bIsbtnAddPatientClick = value; }
        }

        public ShiftRecFilterForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.PersonsIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadFilterTemplet();
            Application.DoEvents();
            this.LoadDefaultPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            Application.DoEvents();
            LoadDefaultSpecialPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            Application.DoEvents();
            if (this.m_bIsbtnAddPatientClick == true)
                this.formControl1.UpdateFormData("Ĭ�ϲ���ѡ����", true);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_bIsSpecialPatient == true)
            {
                this.m_selectedSpecialPatients = this.GetSelectedSpecialPatientList();
                if (this.m_selectedSpecialPatients != null)
                    this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.m_selectedPatients = this.GetSelectedPatientList();
                if (this.m_selectedPatients != null)
                    this.DialogResult = DialogResult.OK;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ���ز���ɸѡ��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool LoadFilterTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_PATIENT_FILTER;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("����ɸѡ����û������!");
                return false;
            }
            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("����ɸѡ������ʧ��!");
                return false;
            }
            bool bsuccess = this.formControl1.Load(docTypeInfo, byteFormData);
            this.formControl1.UpdateFormData("���ⲡ��ɸѡ", this.m_bIsSpecialPatient);
            return bsuccess;
        }

        /// <summary>
        /// ����ȱʡ��ѡ�Ĳ����б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadDefaultPatient()
        {
            if (this.m_bIsSpecialPatient == true)
                return false;
            if (this.m_selectedPatients == null
                || this.m_selectedPatients.Length <= 0)
                return false;
            DataTable tablePatient = GlobalMethods.Table.GetDataTable(this.m_selectedPatients);
            return this.formControl1.UpdateFormData("�����б�", tablePatient);
        }

        /// <summary>
        /// ����ȱʡ��ѡ�����ⲡ���б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadDefaultSpecialPatient()
        {
            if (this.m_bIsSpecialPatient == false)
                return false;
            if (this.m_selectedSpecialPatients == null
                || this.m_selectedSpecialPatients.Length <= 0)
                return false;
            DataTable tablePatient = GlobalMethods.Table.GetDataTable(this.m_selectedSpecialPatients);
            return this.formControl1.UpdateFormData("�����б�", tablePatient);
        }

        /// <summary>
        /// ��ȡ��ǰ�û���ѡ����Ҫ����Ĳ����б�
        /// </summary>
        /// <returns>��ѡ�Ĳ����б�</returns>
        public ShiftPatient[] GetSelectedPatientList()
        {
            DataTable table = this.formControl1.GetFormData("�����б�") as DataTable;
            if (table == null)
            {
                MessageBoxEx.ShowError("���ಡ���б�ɸѡʧ��!");
                return null;
            }

            List<ShiftPatient> lstShiftPatients = new List<ShiftPatient>();
            foreach (DataRow row in table.Rows)
            {
                ShiftPatient shiftPatient = new ShiftPatient();
                shiftPatient.FromDataRow(row);
                if (!GlobalMethods.Misc.IsEmptyString(shiftPatient.PatientID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftPatient.VisitID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftPatient.SubID))
                    lstShiftPatients.Add(shiftPatient);
            }
            return lstShiftPatients.ToArray();
        }

        /// <summary>
        /// ��ȡ��ǰ�û���ѡ����Ҫ��������ⲡ���б�
        /// </summary>
        /// <returns>��ѡ�����ⲡ���б�</returns>
        public ShiftSpecialPatient[] GetSelectedSpecialPatientList()
        {
            DataTable table = this.formControl1.GetFormData("���ⲡ���б�") as DataTable;
            if (table == null)
            {
                MessageBoxEx.ShowError("���ⲡ�˽����б�ɸѡʧ��!");
                return null;
            }
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            List<ShiftSpecialPatient> lstShiftSpecialPatients = new List<ShiftSpecialPatient>();
            foreach (DataRow row in table.Rows)
            {
                ShiftSpecialPatient shiftSpecialPatient = new ShiftSpecialPatient();
                shiftSpecialPatient.FromDataRow(row);
                if (!GlobalMethods.Misc.IsEmptyString(shiftSpecialPatient.PatientID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftSpecialPatient.VisitID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftSpecialPatient.SubID))
                    lstShiftSpecialPatients.Add(shiftSpecialPatient);
            }
            return lstShiftSpecialPatients.ToArray();
        }

        private void formControl1_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            if (e.Name == "������Ϣ")
            {
                if (this.m_bIsSpecialPatient == true)
                { e.Success = true; }
                else
                {
                    e.Value = this.m_shiftRankInfo.ToDataTable();
                    e.Success = true;
                }
            }
        }
    }
}