// ***********************************************************
// 护理电子病历系统,全局病人列表数据缓存类.
// 负责缓存白板模块查询出来的病人列表,以及维护当前活动的病人
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
        /// 获取病人信息表实例
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
        /// 获取当前活动的病人信息
        /// </summary>
        public PatVisitInfo ActivePatient
        {
            get { return this.m_activePatient; }
            set { this.m_activePatient = value; }
        }

        private DataTable m_deptPatientTable = null;

        /// <summary>
        /// 获取当前科室的病人数据列表
        /// </summary>
        public DataTable DeptPatientTable
        {
            get { return this.m_deptPatientTable; }
        }

        private DataTable m_displayPatientTable = null;

        /// <summary>
        /// 获取当前显示的病人数据列表
        /// </summary>
        public DataTable DisplayPatientTable
        {
            get { return this.m_displayPatientTable; }
        }

        private Hashtable m_colorTable = null;

        /// <summary>
        /// 获取或设置脚本中定义的颜色表
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
        /// 当系统刷新病人列表数据时触发
        /// </summary>
        [Description("当系统刷新病人列表数据时触发")]
        public event EventHandler PatientTableChanged;

        /// <summary>
        /// 当系统刷新病人数据时触发
        /// </summary>
        [Description("当系统刷新病人数据时触发")]
        public event EventHandler PatientInfoChanged;

        /// <summary>
        /// 刷新当前科室的病人列表数据
        /// </summary>
        /// <param name="patientList">病人列表数据</param>
        public void RefreshDeptPatientList(DataTable patientList)
        {
            this.m_deptPatientTable = patientList;
            this.m_displayPatientTable = patientList;
            if (this.PatientTableChanged != null)
                this.PatientTableChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// 刷新当前显示的病人列表数据
        /// </summary>
        /// <param name="patientList">病人列表数据</param>
        public void RefreshDisplayPatientList(DataTable patientList)
        {
            this.m_displayPatientTable = patientList;
            if (this.PatientTableChanged != null)
                this.PatientTableChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// 判断指定的病人信息数据行是否是空床
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>是否是空床</returns>
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
        /// 从白板脚本中配置的颜色表中获取指定的颜色
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>颜色</returns>
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
        /// 获取指定的数据行中包含的病人就诊数据对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>病人就诊数据对象</returns>
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
        /// 在当前病人列表数据中查询指定病人信息
        /// </summary>
        /// <param name="patientID">病人ID</param>
        /// <param name="visitID">就诊ID</param>
        /// <returns>病人信息</returns>
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
        /// 获取指定的数据集行中某一列的值
        /// </summary>
        /// <param name="row">数据集行</param>
        /// <param name="columnName">列名</param>
        /// <returns>单元格值</returns>
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
        /// 从服务器重新加载刷新当前显示的病人信息
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
                MessageBoxEx.ShowError("病人就诊信息下载失败!");
                return false;
            }
            GlobalMethods.Reflect.CopyProperties(patVisitInfo, this.m_activePatient);
            if (this.PatientInfoChanged != null)
                this.PatientInfoChanged(this, EventArgs.Empty);
            return true;
        }
    }
}
