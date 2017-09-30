// ***********************************************************
// ������Ӳ���ϵͳ,ȫ�ֲ����б����ݻ�����.
// ���𻺴�װ�ģ���ѯ�����Ĳ����б�,�Լ�ά����ǰ��Ĳ���
// Creator:YangMingkun  Date:2012-8-21
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class PatientTable
    {
        private static PatientTable m_instance = null;

        /// <summary>
        /// ��ȡ������Ϣ��ʵ��
        /// </summary>
        public static PatientTable Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PatientTable();
                return m_instance;
            }
        }

        private PatientTable()
        {
        }

        private PatVisitInfo m_activePatient = null;

        /// <summary>
        /// ��ȡ��ǰ��Ĳ�����Ϣ
        /// </summary>
        public PatVisitInfo ActivePatient
        {
            get { return this.m_activePatient; }
            set { this.m_activePatient = value; }
        }

        private DataTable m_deptPatientTable = null;

        /// <summary>
        /// ��ȡ��ǰ���ҵĲ��������б�
        /// </summary>
        public DataTable DeptPatientTable
        {
            get { return this.m_deptPatientTable; }
        }

        private DataTable m_displayPatientTable = null;

        /// <summary>
        /// ��ȡ��ǰ��ʾ�Ĳ��������б�
        /// </summary>
        public DataTable DisplayPatientTable
        {
            get { return this.m_displayPatientTable; }
        }

        private Hashtable m_colorTable = null;

        /// <summary>
        /// ��ȡ�����ýű��ж������ɫ��
        /// </summary>
        public Hashtable ColorTable
        {
            get
            {
                if (this.m_colorTable == null)
                    return new Hashtable();
                return this.m_colorTable;
            }

            set
            {
                this.m_colorTable = value;
            }
        }

        /// <summary>
        /// ��ϵͳˢ�²����б�����ʱ����
        /// </summary>
        [Description("��ϵͳˢ�²����б�����ʱ����")]
        public event EventHandler PatientTableChanged;

        /// <summary>
        /// ��ϵͳˢ�²�������ʱ����
        /// </summary>
        [Description("��ϵͳˢ�²�������ʱ����")]
        public event EventHandler PatientInfoChanged;

        /// <summary>
        /// ˢ�µ�ǰ���ҵĲ����б�����
        /// </summary>
        /// <param name="patientList">�����б�����</param>
        public void RefreshDeptPatientList(DataTable patientList)
        {
            this.m_deptPatientTable = patientList;
            this.m_displayPatientTable = patientList;
            if (this.PatientTableChanged != null)
                this.PatientTableChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// ˢ�µ�ǰ��ʾ�Ĳ����б�����
        /// </summary>
        /// <param name="patientList">�����б�����</param>
        public void RefreshDisplayPatientList(DataTable patientList)
        {
            this.m_displayPatientTable = patientList;
            if (this.PatientTableChanged != null)
                this.PatientTableChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// �ж�ָ���Ĳ�����Ϣ�������Ƿ��ǿմ�
        /// </summary>
        /// <param name="row">������</param>
        /// <returns>�Ƿ��ǿմ�</returns>
        public bool IsEmptyBed(DataRow row)
        {
            if (row == null)
                return true;
            string szPatientID = this.GetRowCellValue(row, "patient_id");
            if (GlobalMethods.Misc.IsEmptyString(szPatientID))
                return true;
            string szVisitID = this.GetRowCellValue(row, "visit_id");
            if (GlobalMethods.Misc.IsEmptyString(szVisitID))
                return true;
            return false;
        }

        /// <summary>
        /// �Ӱװ�ű������õ���ɫ���л�ȡָ������ɫ
        /// </summary>
        /// <param name="key">�ؼ���</param>
        /// <returns>��ɫ</returns>
        public Color GetColor(string key)
        {
            if (GlobalMethods.Misc.IsEmptyString(key)
                || this.m_colorTable == null
                || !this.m_colorTable.Contains(key))
                return Color.White;
            object value = this.m_colorTable[key];
            return (value is Color) ? (Color)value : Color.White;
        }

        /// <summary>
        /// ��ȡָ�����������а����Ĳ��˾������ݶ���
        /// </summary>
        /// <param name="row">������</param>
        /// <returns>���˾������ݶ���</returns>
        public PatVisitInfo GetPatVisit(DataRow row)
        {
            PatVisitInfo patVisit = new PatVisitInfo();
            patVisit.BedCode = this.GetRowCellValue(row, "bed_code");
            patVisit.PatientId = this.GetRowCellValue(row, "patient_id");
            patVisit.PatientName = this.GetRowCellValue(row, "patient_name");
            patVisit.PatientSex = this.GetRowCellValue(row, "patient_sex");
            patVisit.ChargeType = this.GetRowCellValue(row, "charge_type");

            string value = this.GetRowCellValue(row, "birth_time");
            DateTime date = DateTime.Now;
            GlobalMethods.Convert.StringToDate(value, ref date);
            patVisit.BirthTime = date;

            patVisit.VisitId = this.GetRowCellValue(row, "visit_id");
            patVisit.InpNo = this.GetRowCellValue(row, "inp_no");

            value = this.GetRowCellValue(row, "visit_time");
            date = DateTime.Now;
            GlobalMethods.Convert.StringToDate(value, ref date);
            patVisit.VisitTime = date;

            patVisit.DeptCode = this.GetRowCellValue(row, "dept_code");
            patVisit.DeptName = this.GetRowCellValue(row, "dept_name");
            patVisit.WardCode = this.GetRowCellValue(row, "ward_code");
            patVisit.WardName = this.GetRowCellValue(row, "ward_name");
            patVisit.InchargeDoctor = this.GetRowCellValue(row, "incharge_doctor");
            patVisit.Diagnosis = this.GetRowCellValue(row, "diagnosis");
            patVisit.AllergyDrugs = this.GetRowCellValue(row, "allergy_drugs");
            patVisit.PatientCondition = this.GetRowCellValue(row, "patient_condition");
            patVisit.NursingClass = this.GetRowCellValue(row, "nursing_class");
            return patVisit;
        }

        /// <summary>
        /// �ڵ�ǰ�����б������в�ѯָ��������Ϣ
        /// </summary>
        /// <param name="patientID">����ID</param>
        /// <param name="visitID">����ID</param>
        /// <returns>������Ϣ</returns>
        public PatVisitInfo GetPatVisit(string patientID, string visitID)
        {
            if (this.m_deptPatientTable != null)
            {
                foreach (DataRow row in this.m_deptPatientTable.Rows)
                {
                    if (this.GetRowCellValue(row, "patient_id") == patientID
                        && this.GetRowCellValue(row, "visit_id") == visitID)
                        return this.GetPatVisit(row);
                }
            }
            if (this.m_displayPatientTable != null)
            {
                foreach (DataRow row in this.m_displayPatientTable.Rows)
                {
                    if (this.GetRowCellValue(row, "patient_id") == patientID
                        && this.GetRowCellValue(row, "visit_id") == visitID)
                        return this.GetPatVisit(row);
                }
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ�������ݼ�����ĳһ�е�ֵ
        /// </summary>
        /// <param name="row">���ݼ���</param>
        /// <param name="columnName">����</param>
        /// <returns>��Ԫ��ֵ</returns>
        private string GetRowCellValue(DataRow row, string columnName)
        {
            try
            {
                object cellValue = row[columnName];
                if (cellValue == null || cellValue == DBNull.Value)
                    return string.Empty;
                return cellValue.ToString().Trim();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("PatientTable.GetRowCellValue"
                    , new string[] { "row", "columnName" }, new object[] { row, columnName }, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// �ӷ��������¼���ˢ�µ�ǰ��ʾ�Ĳ�����Ϣ
        /// </summary>
        /// <returns>bool</returns>
        public bool ReloadPatientInfoFromServer()
        {
            if (this.m_activePatient == null)
                return false;
            string szPatientID = this.m_activePatient.PatientId;
            string szVisitID = this.m_activePatient.VisitId;
            PatVisitInfo patVisitInfo = null;
            short shRet = PatVisitService.Instance.GetPatVisitInfo(szPatientID, szVisitID, ref patVisitInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("���˾�����Ϣ����ʧ��!");
                return false;
            }
            GlobalMethods.Reflect.CopyProperties(patVisitInfo, this.m_activePatient);
            if (this.PatientInfoChanged != null)
                this.PatientInfoChanged(this, EventArgs.Empty);
            return true;
        }
    }
}
