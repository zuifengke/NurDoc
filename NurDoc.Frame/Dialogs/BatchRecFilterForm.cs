// ***********************************************************
// ������Ӳ���ϵͳ,�����ڶ����װ�ģ��.
// Creator:ZhangLian  Date:2012-6-16
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
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class BatchRecFilterForm : HerenForm
    {
        private PatVisitInfo[] m_selectedPatients = null;

        /// <summary>
        /// ��ȡѡ�еĲ����б�
        /// </summary>
        [Browsable(false)]
        public PatVisitInfo[] SelectedPatients
        {
            get
            {
                if (this.m_selectedPatients != null)
                    return this.m_selectedPatients;
                return this.GetSelectedPatientList();
            }
        }

        private GridViewSchema m_schema = null;

        /// <summary>
        /// ��ȡ��������������¼�뷽��
        /// </summary>
        [Browsable(false)]
        [Description("��ȡ��������������¼�뷽��")]
        public GridViewSchema Schema
        {
            get { return this.m_schema; }
            set { this.m_schema = value; }
        }

        private DateTime m_dtRecordTime = DateTime.Now;

        /// <summary>
        /// ��ȡ��������������¼��ʱ��
        /// </summary>
        [Browsable(false)]
        [Description("��ȡ��������������¼��ʱ��")]
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
            set { this.m_dtRecordTime = value; }
        }

        public BatchRecFilterForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.PersonsIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadFilterTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_selectedPatients = this.GetSelectedPatientList();
            if (this.m_selectedPatients != null)
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private bool GetSystemContext(string name, ref object value)
        {
            if (name == "����¼�뷽������")
            {
                if (this.m_schema == null)
                    return false;
                value = this.m_schema.SchemaName;
                return true;
            }
            else if (name == "����¼�뷽�����")
            {
                if (this.m_schema == null)
                    return false;
                value = this.m_schema.SchemaFlag;
                return true;
            }
            else if (name == "����ʱ��")
            {
                value = this.m_dtRecordTime;
                return true;
            }
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        /// <summary>
        /// ���ز���ɸѡ��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool LoadFilterTemplet()
        {
            if (this.Visible) this.Update();

            string szApplyEnv = ServerData.DocTypeApplyEnv.BATCH_RECORD_FILTER;
            DocTypeInfo docTypeInfo = null;
            if (this.Schema == null)
            {
                docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            }
            else
            {
                string szTempletName = this.Schema.SchemaName;
                docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            }
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("�����б�ɸѡģ�廹û������!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("�����б�ɸѡģ����������ʧ��!");
                return false;
            }
            bool success = this.formControl1.Load(docTypeInfo, byteFormData);
            this.formControl1.MaximizeEditRegion();
             if (this.Visible) this.Update();
            this.formControl1.UpdateFormData("����ʱ��", this.m_dtRecordTime);
           
            return success;
        }

        /// <summary>
        /// ��ȡ��ǰѡ�еĲ����б�(�ɷ���)
        /// </summary>
        /// <param name="formData">������</param>
        /// <returns>ѡ�еĲ����б�</returns>
        private PatVisitInfo[] GetSelectedPatientListOld(object formData)
        {
            List<string> patientInfoList = formData as List<string>;
            if (patientInfoList == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowError("��������¼�벡���б�ɸѡʧ��!");
                return null;
            }

            List<PatVisitInfo> lstPatVisitInfos = new List<PatVisitInfo>();
            for (int index = 0; index < patientInfoList.Count; index++)
            {
                string szPatientInfo = patientInfoList[index];
                if (string.IsNullOrEmpty(szPatientInfo))
                    continue;
                string[] arrPatientInfo = szPatientInfo.Split(';');
                if (arrPatientInfo.Length < 6)
                    continue;
                PatVisitInfo patVisitInfo = new PatVisitInfo();
                patVisitInfo.BedCode = arrPatientInfo[0].Trim();
                patVisitInfo.PatientID = arrPatientInfo[1].Trim();
                patVisitInfo.VisitID = arrPatientInfo[2].Trim();
                patVisitInfo.PatientName = arrPatientInfo[3].Trim();
                patVisitInfo.DeptCode = arrPatientInfo[4].Trim();
                patVisitInfo.DeptName = arrPatientInfo[5].Trim();
                patVisitInfo.WardCode = patVisitInfo.DeptCode;
                patVisitInfo.WardName = patVisitInfo.DeptName;
                lstPatVisitInfos.Add(patVisitInfo);
            }
            return lstPatVisitInfos.ToArray();
        }

        /// <summary>
        /// ��ȡ��ǰѡ�еĲ����б�
        /// </summary>
        /// <returns>ѡ�еĲ����б�</returns>
        public PatVisitInfo[] GetSelectedPatientList()
        {
            object formData = this.formControl1.GetFormData("�����б�");
            if (formData is List<string>)
            {
                return this.GetSelectedPatientListOld(formData);
            }
            DataTable table = formData as DataTable;
            if (table == null)
            {
                MessageBoxEx.ShowError("��������¼�벡���б�ɸѡʧ��!");
                return null;
            }

            List<PatVisitInfo> lstPatVisitInfos = new List<PatVisitInfo>();
            foreach (DataRow row in table.Rows)
            {
                PatVisitInfo patVisitInfo = new PatVisitInfo();
                patVisitInfo.FromDataRow(row);
                if (!GlobalMethods.Misc.IsEmptyString(patVisitInfo.PatientID)
                    && !GlobalMethods.Misc.IsEmptyString(patVisitInfo.VisitID)
                    && !GlobalMethods.Misc.IsEmptyString(patVisitInfo.SubID))
                    lstPatVisitInfos.Add(patVisitInfo);
            }
            return lstPatVisitInfos.ToArray();
        }

        private void formControl1_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }
    }
}