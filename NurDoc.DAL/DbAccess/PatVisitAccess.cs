// ***********************************************************
// 数据库访问层与HIS系统数据库有关的数据的访问类.
// 全部基于DBLINK与数据视图来实现,以提高系统可配置性、适应性
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using Heren.Common.Libraries;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class PatVisitAccess : DBAccessBase
    {
        #region"病人就诊数据接口"
        /// <summary>
        /// 根据病人ID和就诊ID,获取病人就诊信息
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="patVisitInfo">病人就诊信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPatVisitInfo(string szPatientID, string szVisitID, ref PatVisitInfo patVisitInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}"
                , ServerData.PatVisitView.PATIENT_ID, ServerData.PatVisitView.PATIENT_NAME
                , ServerData.PatVisitView.PATIENT_SEX, ServerData.PatVisitView.BIRTH_TIME
                , ServerData.PatVisitView.CHARGE_TYPE, ServerData.PatVisitView.VISIT_ID
                , ServerData.PatVisitView.VISIT_TIME, ServerData.PatVisitView.BED_CODE
                , ServerData.PatVisitView.DEPT_CODE, ServerData.PatVisitView.DEPT_NAME
                , ServerData.PatVisitView.WARD_CODE, ServerData.PatVisitView.WARD_NAME
                , ServerData.PatVisitView.INCHARGE_DOCTOR, ServerData.PatVisitView.DIAGNOSIS
                , ServerData.PatVisitView.ALLERGY_DRUGS, ServerData.PatVisitView.PATIENT_CONDITION
                , ServerData.PatVisitView.NURSING_CLASS, ServerData.PatVisitView.INP_NO
                ,ServerData.PatVisitView.DISCHARGE_TIME);

            string szCondition = string.Format("{0} = '{1}' AND {2} = '{3}'"
                , ServerData.PatVisitView.PATIENT_ID, szPatientID
                , ServerData.PatVisitView.VISIT_ID, szVisitID);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataView.PAT_VISIT, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (patVisitInfo == null)
                    patVisitInfo = new PatVisitInfo();
                if (!dataReader.IsDBNull(0)) patVisitInfo.PatientID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1)) patVisitInfo.PatientName = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2)) patVisitInfo.PatientSex = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3)) patVisitInfo.BirthTime = dataReader.GetDateTime(3);
                if (!dataReader.IsDBNull(4)) patVisitInfo.ChargeType = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5)) patVisitInfo.VisitID = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6)) patVisitInfo.VisitTime = dataReader.GetDateTime(6);
                if (!dataReader.IsDBNull(7)) patVisitInfo.BedCode = dataReader.GetValue(7).ToString();
                if (!dataReader.IsDBNull(8)) patVisitInfo.DeptCode = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9)) patVisitInfo.DeptName = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10)) patVisitInfo.WardCode = dataReader.GetString(10);
                if (!dataReader.IsDBNull(11)) patVisitInfo.WardName = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12)) patVisitInfo.InchargeDoctor = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13)) patVisitInfo.Diagnosis = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14)) patVisitInfo.AllergyDrugs = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15)) patVisitInfo.PatientCondition = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16)) patVisitInfo.NursingClass = dataReader.GetString(16);
                if (!dataReader.IsDBNull(17)) patVisitInfo.InpNo = dataReader.GetString(17);
                if (!dataReader.IsDBNull(18)) patVisitInfo.DischargeTime = dataReader.GetDateTime(18);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="szPatStatus">病人状态:IN/OUT/IN_AND_OUT</param>
        /// <param name="szWardCode">护理单元代码</param>
        /// <param name="szPatientName">病人姓名</param>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szNurseClass">护理等级</param>
        /// <param name="szPatCondition">病人病情</param>
        /// <param name="dtStart">时间开始[病人状态所有、在院查入院时间，出院查出院时间]</param>
        /// <param name="dtEnd">时间结束[病人状态所有、在院查入院时间，出院查出院时间]</param>
        /// <param name="lstPatvisitInfo">病人信息列表</param>
        /// <returns>ReturnValue</returns>
        public short GetPatVisitInfo(string szPatStatus, string szWardCode, string szPatientName, string szPatientID, string szNurseClass,
            string szPatCondition, DateTime? dtStart, DateTime? dtEnd, ref List<PatVisitInfo> lstPatvisitInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.PatVisitView.PATIENT_ID, ServerData.PatVisitView.PATIENT_NAME
                , ServerData.PatVisitView.PATIENT_SEX, ServerData.PatVisitView.BIRTH_TIME
                , ServerData.PatVisitView.CHARGE_TYPE, ServerData.PatVisitView.VISIT_ID
                , ServerData.PatVisitView.VISIT_TIME, ServerData.PatVisitView.BED_CODE
                , ServerData.PatVisitView.DEPT_CODE, ServerData.PatVisitView.DEPT_NAME
                , ServerData.PatVisitView.WARD_CODE, ServerData.PatVisitView.WARD_NAME
                , ServerData.PatVisitView.INCHARGE_DOCTOR, ServerData.PatVisitView.DIAGNOSIS
                , ServerData.PatVisitView.ALLERGY_DRUGS, ServerData.PatVisitView.PATIENT_CONDITION
                , ServerData.PatVisitView.NURSING_CLASS, ServerData.PatVisitView.INP_NO);

            string szCondition = " 1=1 ";

            if (!string.IsNullOrEmpty(szWardCode))
            {
                szCondition += string.Format(" and {0}='{1}'", ServerData.PatVisitView.WARD_CODE, szWardCode);
            }
            if (!string.IsNullOrEmpty(szPatientName))
            {
                szCondition += string.Format(" and {0}='{1}'", ServerData.PatVisitView.PATIENT_NAME, szPatientName);
            }
            if (!string.IsNullOrEmpty(szPatientID))
            {
                szCondition += string.Format(" and {0}='{1}'", ServerData.PatVisitView.PATIENT_ID, szPatientID);
            }
            if (!string.IsNullOrEmpty(szNurseClass))
            {
                szCondition += string.Format(" and {0}='{1}'", ServerData.PatVisitView.NURSING_CLASS, szNurseClass);
            }
            if (!string.IsNullOrEmpty(szPatCondition))
            {
                szCondition += string.Format(" and {0}='{1}'", ServerData.PatVisitView.PATIENT_CONDITION, szPatCondition);
            }
            if (dtStart != null)
            {
                szCondition += string.Format(" and {0}>={1}", szPatStatus == "OUT" ? ServerData.PatVisitView.DISCHARGE_TIME : ServerData.PatVisitView.VISIT_TIME, base.DataAccess.GetSqlTimeFormat(dtStart));
            }
            if (dtEnd != null)
            {
                szCondition += string.Format(" and {0}<={1}", szPatStatus == "OUT" ? ServerData.PatVisitView.DISCHARGE_TIME : ServerData.PatVisitView.VISIT_TIME, base.DataAccess.GetSqlTimeFormat(dtEnd));
            }

            //如果是在院病人则查在院病人视图
            string szTalbe = ServerData.DataView.PAT_VISIT;
            if (szPatStatus == "IN")
                szTalbe = ServerData.DataView.INP_VISIT;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTalbe, szCondition,ServerData.PatVisitView.WARD_CODE);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstPatvisitInfo == null)
                    lstPatvisitInfo = new List<PatVisitInfo>();
                lstPatvisitInfo.Clear();
                do
                {
                    PatVisitInfo patVisitInfo = new PatVisitInfo();
                    if (!dataReader.IsDBNull(0)) patVisitInfo.PatientID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) patVisitInfo.PatientName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) patVisitInfo.PatientSex = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) patVisitInfo.BirthTime = dataReader.GetDateTime(3);
                    if (!dataReader.IsDBNull(4)) patVisitInfo.ChargeType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) patVisitInfo.VisitID = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) patVisitInfo.VisitTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) patVisitInfo.BedCode = dataReader.GetValue(7).ToString();
                    if (!dataReader.IsDBNull(8)) patVisitInfo.DeptCode = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) patVisitInfo.DeptName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) patVisitInfo.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) patVisitInfo.WardName = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) patVisitInfo.InchargeDoctor = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) patVisitInfo.Diagnosis = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) patVisitInfo.AllergyDrugs = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) patVisitInfo.PatientCondition = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) patVisitInfo.NursingClass = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) patVisitInfo.InpNo = dataReader.GetString(17);
                    lstPatvisitInfo.Add(patVisitInfo);
                } while (dataReader.Read());

                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 查询指定病人所经过病区列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="lstDeptInfos">病区列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPatientDeptList(string szPatientID, string szVisitID, ref List<DeptInfo> lstDeptInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstDeptInfos == null)
                lstDeptInfos = new List<DeptInfo>();
            lstDeptInfos.Clear();

            string szField = string.Format("distinct B.{0},B.{1}"
                , ServerData.BedRecordView.WARD_CODE, ServerData.BedRecordView.WARD_NAME);
            string szTable = string.Format("{0} A,{1} B"
                , ServerData.DataView.TRANSFER, ServerData.DataView.BED_RECORD);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}={5}"
                , ServerData.TransferView.PATIENT_ID, szPatientID
                , ServerData.TransferView.VISIT_ID, szVisitID
                , ServerData.TransferView.DEPT_STAYED, ServerData.BedRecordView.DEPT_CODE);
            string szOrderField = ServerData.BedRecordView.WARD_NAME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    DeptInfo deptInfo = new DeptInfo();
                    if (!dataReader.IsDBNull(0)) deptInfo.DeptCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) deptInfo.DeptName = dataReader.GetString(1);
                    lstDeptInfos.Add(deptInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 查询指定病人转科列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="lstTransferInfos">转科列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPatientTransferList(string szPatientID, string szVisitID, ref List<TransferInfo> lstTransferInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstTransferInfos == null)
                lstTransferInfos = new List<TransferInfo>();
            lstTransferInfos.Clear();

            string szField = string.Format("distinct A.{0},A.{1},A.{2},B.{3},A.{4},A.{5},A.{6},A.{7},A.{8},B.{9}"
                , ServerData.TransferView.PATIENT_ID, ServerData.TransferView.VISIT_ID
                , ServerData.TransferView.DEPT_STAYED, ServerData.BedRecordView.DEPT_NAME
                , ServerData.TransferView.ADMISSION_DATE_TIME, ServerData.TransferView.DISCHARGE_DATE_TIME
                , ServerData.TransferView.DEPT_TRANSFERED_TO, ServerData.TransferView.DOCTOR_IN_CHARGE
                , ServerData.TransferView.DOCTOR_DEPT, ServerData.BedRecordView.WARD_CODE);
            string szTable = string.Format("{0} A,{1} B"
                , ServerData.DataView.TRANSFER, ServerData.DataView.BED_RECORD);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}={5}"
                , ServerData.TransferView.PATIENT_ID, szPatientID
                , ServerData.TransferView.VISIT_ID, szVisitID
                , ServerData.TransferView.DEPT_STAYED, ServerData.BedRecordView.DEPT_CODE);
            string szOrderField = ServerData.TransferView.ADMISSION_DATE_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    TransferInfo transferInfo = new TransferInfo();
                    if (!dataReader.IsDBNull(0)) transferInfo.PatientId = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) transferInfo.VisitId = dataReader.GetValue(1).ToString();
                    if (!dataReader.IsDBNull(2)) transferInfo.DeptCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) transferInfo.DeptName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) transferInfo.AdmissionTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) transferInfo.DischargeTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6)) transferInfo.DeptTransfer = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) transferInfo.Doctor = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) transferInfo.DoctorDept = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) transferInfo.WardCode = dataReader.GetString(9);
                    lstTransferInfos.Add(transferInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据病区代码,获取在院病人就诊信息列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="szPatientIDList">病区代码</param>
        /// <param name="lstPatVisitInfo">病人就诊信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInPatVisitInfoList(string szWardCode, string szPatientIDList, ref List<PatVisitInfo> lstPatVisitInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.InPVisitView.PATIENT_ID, ServerData.InPVisitView.PATIENT_NAME
                , ServerData.InPVisitView.PATIENT_SEX, ServerData.InPVisitView.BIRTH_TIME
                , ServerData.InPVisitView.CHARGE_TYPE, ServerData.InPVisitView.VISIT_ID
                , ServerData.InPVisitView.VISIT_TIME, ServerData.InPVisitView.BED_CODE
                , ServerData.InPVisitView.DEPT_CODE, ServerData.InPVisitView.DEPT_NAME
                , ServerData.InPVisitView.WARD_CODE, ServerData.InPVisitView.WARD_NAME
                , ServerData.InPVisitView.INCHARGE_DOCTOR, ServerData.InPVisitView.DIAGNOSIS
                , ServerData.InPVisitView.ALLERGY_DRUGS
                , ServerData.InPVisitView.PATIENT_CONDITION, ServerData.InPVisitView.NURSING_CLASS
                , ServerData.InPVisitView.BED_LABEL);
            string szCondition = string.Format("1=1");
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
                szCondition = string.Format("{0} And {1} = '{2}'"
                    , szCondition
                    , ServerData.InPVisitView.WARD_CODE, szWardCode
                    );
            if (!GlobalMethods.Misc.IsEmptyString(szPatientIDList))
                szCondition = string.Format("{0} And {1} in({2}) "
                    , szCondition
                    , ServerData.InPVisitView.PATIENT_ID, szPatientIDList
                );

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataView.INP_VISIT, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstPatVisitInfo == null)
                    lstPatVisitInfo = new List<PatVisitInfo>();
                do
                {
                    PatVisitInfo patVisitInfo = new PatVisitInfo();
                    if (!dataReader.IsDBNull(0)) patVisitInfo.PatientID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) patVisitInfo.PatientName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) patVisitInfo.PatientSex = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) patVisitInfo.BirthTime = dataReader.GetDateTime(3);
                    if (!dataReader.IsDBNull(4)) patVisitInfo.ChargeType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) patVisitInfo.VisitID = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) patVisitInfo.VisitTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) patVisitInfo.BedCode = dataReader.GetValue(7).ToString();
                    if (!dataReader.IsDBNull(8)) patVisitInfo.DeptCode = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) patVisitInfo.DeptName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) patVisitInfo.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) patVisitInfo.WardName = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) patVisitInfo.InchargeDoctor = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) patVisitInfo.Diagnosis = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) patVisitInfo.AllergyDrugs = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) patVisitInfo.PatientCondition = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) patVisitInfo.NursingClass = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) patVisitInfo.BedLabel = dataReader.GetString(17);
                    lstPatVisitInfo.Add(patVisitInfo);
                }
                while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion

        #region"检查数据访问接口"
        /// <summary>
        /// 根据病人ID和就诊号，获取该次住院的检查信息列表
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="lstExamInfo">检查信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInpExamList(string szPatientID, string szVisitID, ref List<ExamInfo> lstExamInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("PatVisitAccess.GetInpExamList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstExamInfo == null)
                lstExamInfo = new List<ExamInfo>();
            else
                lstExamInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.ExamMasterView.EXAM_ID, ServerData.ExamMasterView.SUBJECT
                , ServerData.ExamMasterView.REQUEST_TIME, ServerData.ExamMasterView.REQUEST_DOCTOR
                , ServerData.ExamMasterView.RESULT_STATUS, ServerData.ExamMasterView.REPORT_TIME
                , ServerData.ExamMasterView.REPORT_DOCTOR);
            string szTable = ServerData.DataView.EXAM_MASTER;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ExamMasterView.PATIENT_ID, szPatientID, ServerData.ExamMasterView.VISIT_ID, szVisitID);
            string szExamField = ServerData.ExamMasterView.REQUEST_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szExamField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ExamInfo examInfo = new ExamInfo();
                    if (!dataReader.IsDBNull(0)) examInfo.ExamID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) examInfo.Subject = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) examInfo.RequestTime = dataReader.GetDateTime(2);
                    if (!dataReader.IsDBNull(3)) examInfo.RequestDoctor = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) examInfo.ResultStatus = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) examInfo.ReportTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6)) examInfo.ReportDoctor = dataReader.GetString(6);
                    lstExamInfo.Add(examInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据病人ID和就诊时间区间，获取该次门诊的检查信息列表
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="dtVisitTime">该次就诊时间</param>
        /// <param name="dtNextVisitTime">下次就诊时间</param>
        /// <param name="lstExamInfo">检查信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetClinicExamList(string szPatientID, DateTime dtVisitTime, DateTime dtNextVisitTime, ref List<ExamInfo> lstExamInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID))
            {
                LogManager.Instance.WriteLog("PatVisitAccess.GetClinicExamList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstExamInfo == null)
                lstExamInfo = new List<ExamInfo>();
            else
                lstExamInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.ExamMasterView.EXAM_ID, ServerData.ExamMasterView.SUBJECT
                , ServerData.ExamMasterView.REQUEST_TIME, ServerData.ExamMasterView.REQUEST_DOCTOR
                , ServerData.ExamMasterView.RESULT_STATUS, ServerData.ExamMasterView.REPORT_TIME
                , ServerData.ExamMasterView.REPORT_DOCTOR);
            string szTable = ServerData.DataView.EXAM_MASTER;
            string szCondition = string.Format("{0}='{1}' AND {2} >= {3} AND {4} < {5}"
                , ServerData.ExamMasterView.PATIENT_ID, szPatientID
                , ServerData.ExamMasterView.REQUEST_TIME, base.DataAccess.GetSqlTimeFormat(dtVisitTime)
                , ServerData.ExamMasterView.REQUEST_TIME, base.DataAccess.GetSqlTimeFormat(dtNextVisitTime));
            string szExamField = string.Format("{0}", ServerData.ExamMasterView.REQUEST_TIME);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szExamField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ExamInfo examInfo = new ExamInfo();
                    if (!dataReader.IsDBNull(0)) examInfo.ExamID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) examInfo.Subject = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) examInfo.RequestTime = dataReader.GetDateTime(2);
                    if (!dataReader.IsDBNull(3)) examInfo.RequestDoctor = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) examInfo.ResultStatus = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) examInfo.ReportTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6)) examInfo.ReportDoctor = dataReader.GetString(6);
                    lstExamInfo.Add(examInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据指定的就诊信息，获取该次就诊时，产生的检查报告详细信息
        /// </summary>
        /// <param name="szExamID">申请序号</param>
        /// <param name="examResultInfo">检查报告信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetExamResultInfo(string szExamID, ref ExamResultInfo examResultInfo)
        {
            if (szExamID == null || szExamID.Trim() == string.Empty)
            {
                LogManager.Instance.WriteLog("PatVisitAccess.GetExamResultInfo", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5}"
                , ServerData.ExamResultView.EXAM_ID, ServerData.ExamResultView.PARAMETERS
                , ServerData.ExamResultView.DESCRIPTION, ServerData.ExamResultView.IMPRESSION
                , ServerData.ExamResultView.RECOMMENDATION, ServerData.ExamResultView.IS_ABNORMAL);
            string szTable = ServerData.DataView.EXAM_RESULT;
            string szCondition = string.Format("{0}='{1}'", ServerData.ExamResultView.EXAM_ID, szExamID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    if (examResultInfo == null)
                        examResultInfo = new ExamResultInfo();
                    if (!dataReader.IsDBNull(0)) examResultInfo.ExamID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) examResultInfo.Parameters = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) examResultInfo.Discription = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) examResultInfo.Impression = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) examResultInfo.Recommendation = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) examResultInfo.Abnormal = dataReader.GetString(5);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion

        #region"检验数据访问接口"
        /// <summary>
        /// 根据病人ID和就诊号，获取该次住院的检验信息列表
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="lstLabTestInfo">检验信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInpLabTestList(string szPatientID, string szVisitID, ref List<LabTestInfo> lstLabTestInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("PatVisitAccess.GetInpLabTestList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (lstLabTestInfo == null)
                lstLabTestInfo = new List<LabTestInfo>();
            else
                lstLabTestInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.LabMasterView.TEST_ID, ServerData.LabMasterView.SUBJECT
                , ServerData.LabMasterView.SPECIMEN, ServerData.LabMasterView.REQUEST_TIME
                , ServerData.LabMasterView.REQUEST_DOCTOR, ServerData.LabMasterView.RESULT_STATUS
                , ServerData.LabMasterView.REPORT_TIME, ServerData.LabMasterView.REPORT_DOCTOR);
            string szTable = ServerData.DataView.LAB_MASTER;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.LabMasterView.PATIENT_ID, szPatientID, ServerData.LabMasterView.VISIT_ID, szVisitID);
            string szOrderField = ServerData.LabMasterView.REQUEST_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    LabTestInfo labTestInfo = new LabTestInfo();
                    if (!dataReader.IsDBNull(0)) labTestInfo.TestID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) labTestInfo.Subject = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) labTestInfo.Specimen = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) labTestInfo.RequestTime = dataReader.GetDateTime(3);
                    if (!dataReader.IsDBNull(4)) labTestInfo.RequestDoctor = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) labTestInfo.ResultStatus = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) labTestInfo.ReportTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) labTestInfo.ReportDoctor = dataReader.GetString(7);
                    lstLabTestInfo.Add(labTestInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据病人ID和就诊时间区间，获取该次门诊的检验信息列表
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="dtVisitTime">当次就诊时间</param>
        /// <param name="dtNextVisitTime">下次就诊时间</param>
        /// <param name="lstLabTestInfo">检验信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetClinicLabTestList(string szPatientID, DateTime dtVisitTime, DateTime dtNextVisitTime, ref List<LabTestInfo> lstLabTestInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID))
            {
                LogManager.Instance.WriteLog("PatVisitAccess.GetClinicLabTestList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (lstLabTestInfo == null)
                lstLabTestInfo = new List<LabTestInfo>();
            else
                lstLabTestInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.LabMasterView.TEST_ID, ServerData.LabMasterView.SUBJECT
                , ServerData.LabMasterView.SPECIMEN, ServerData.LabMasterView.REQUEST_TIME
                , ServerData.LabMasterView.REQUEST_DOCTOR, ServerData.LabMasterView.RESULT_STATUS
                , ServerData.LabMasterView.REPORT_TIME, ServerData.LabMasterView.REPORT_DOCTOR);
            string szTable = ServerData.DataView.LAB_MASTER;
            string szCondition = string.Format("{0}='{1}' AND {2} >= {3} AND {4} < {5}"
                , ServerData.LabMasterView.PATIENT_ID, szPatientID
                , ServerData.LabMasterView.REQUEST_TIME, base.DataAccess.GetSqlTimeFormat(dtVisitTime)
                , ServerData.LabMasterView.REQUEST_TIME, base.DataAccess.GetSqlTimeFormat(dtNextVisitTime));
            string szLabTestField = ServerData.LabMasterView.REQUEST_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szLabTestField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    LabTestInfo labTestInfo = new LabTestInfo();
                    if (!dataReader.IsDBNull(0)) labTestInfo.TestID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) labTestInfo.Subject = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) labTestInfo.Specimen = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) labTestInfo.RequestTime = dataReader.GetDateTime(3);
                    if (!dataReader.IsDBNull(4)) labTestInfo.RequestDoctor = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) labTestInfo.ResultStatus = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) labTestInfo.ReportTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) labTestInfo.ReportDoctor = dataReader.GetString(7);
                    lstLabTestInfo.Add(labTestInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据指定的申请序号，获取检验结果列表
        /// </summary>
        /// <param name="szTestID">申请序号</param>
        /// <param name="lstTestResultInfo">检验结果列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetTestResultList(string szTestID, ref List<TestResultInfo> lstTestResultInfo)
        {
            if (szTestID == null || szTestID.Trim() == string.Empty)
            {
                LogManager.Instance.WriteLog("PatVisitAccess.GetTestResultList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstTestResultInfo == null)
                lstTestResultInfo = new List<TestResultInfo>();
            else
                lstTestResultInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.LabResultView.TEST_ID, ServerData.LabResultView.ITEM_NO
                , ServerData.LabResultView.ITEM_NAME, ServerData.LabResultView.ITEM_RESULT
                , ServerData.LabResultView.ITEM_UNITS, ServerData.LabResultView.ITEM_REFER
                , ServerData.LabResultView.ABNORMAL_INDICATOR);
            string szTable = ServerData.DataView.LAB_RESULT;
            string szCondition = string.Format("{0}='{1}'", ServerData.LabResultView.TEST_ID, szTestID);
            string szOrderField = ServerData.LabResultView.ITEM_NO;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    TestResultInfo testResultInfo = new TestResultInfo();
                    if (!dataReader.IsDBNull(0)) testResultInfo.TestID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) testResultInfo.ItemNo = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) testResultInfo.ItemName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) testResultInfo.ItemResult = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) testResultInfo.ItemUnits = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) testResultInfo.ItemRefer = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) testResultInfo.AbnormalIndecator = dataReader.GetString(6);
                    lstTestResultInfo.Add(testResultInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion

        #region "医嘱数据访问接口"
        /// <summary>
        /// 获取病人入院到护理评估期间产生的医嘱数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="dtFrom">起始下达时间</param>
        /// <param name="dtTo">截止下达时间</param>
        /// <param name="lstOrderInfo">医嘱信息列表</param>
        /// <returns>ReturnValue</returns>
        public short GetPerformOrderList(string szPatientID, string szVisitID, DateTime dtFrom, DateTime dtTo, ref List<OrderInfo> lstOrderInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NursingAccess.GetInpOrderList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstOrderInfo == null)
                lstOrderInfo = new List<OrderInfo>();
            else
                lstOrderInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}"
                , ServerData.OrdersView.ORDER_NO, ServerData.OrdersView.ORDER_SUB_NO
                , ServerData.OrdersView.REPEAT_INDICATOR, ServerData.OrdersView.ORDER_CLASS
                , ServerData.OrdersView.ENTER_DATE_TIME, ServerData.OrdersView.ORDER_TEXT
                , ServerData.OrdersView.DRUG_BILLING_ATTR, ServerData.OrdersView.DOSAGE
                , ServerData.OrdersView.DOSAGE_UNITS, ServerData.OrdersView.ADMINISTRATION
                , ServerData.OrdersView.FREQUENCY, ServerData.OrdersView.FREQ_DETAIL
                , ServerData.OrdersView.END_DATE_TIME, ServerData.OrdersView.PACK_COUNT
                , ServerData.OrdersView.DOCTOR, ServerData.OrdersView.NURSE
                , ServerData.OrdersView.START_STOP_INDICATOR, ServerData.OrdersView.ORDER_STATUS
                , ServerData.OrdersView.PERFORM_SCHEDULE, ServerData.OrdersView.STOP_DOCTOR
                , ServerData.OrdersView.PROCESSING_STOP_DATE_TIME, ServerData.OrdersView.STOP_NURSE
                , ServerData.OrdersView.SHORT_PROCESSING_STOP_DATE, ServerData.OrdersView.SHORT_STOP_NURSE
                , ServerData.OrdersView.PERFORM_RESULT);
            string szTable = ServerData.DataView.PERFORM_ORDERS;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4} >= {5} AND {4} < {6}"
                , ServerData.OrdersView.PATIENT_ID, szPatientID, ServerData.OrdersView.VISIT_ID, szVisitID
                , ServerData.OrdersView.ENTER_DATE_TIME
                , base.DataAccess.GetSqlTimeFormat(dtFrom), base.DataAccess.GetSqlTimeFormat(dtTo));

            string szOrderField = string.Format("{0},{1},{2}"
               , ServerData.OrdersView.ENTER_DATE_TIME, ServerData.OrdersView.ORDER_NO, ServerData.OrdersView.ORDER_SUB_NO);
            string szSQL = string.Format(ServerData.SQL.SELECT_DISTINCT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    OrderInfo orderInfo = new OrderInfo();
                    if (!dataReader.IsDBNull(0)) orderInfo.OrderNO = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) orderInfo.OrderSubNO = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) orderInfo.IsRepeat = dataReader.GetValue(2).ToString().Equals("1");
                    if (!dataReader.IsDBNull(3)) orderInfo.OrderClass = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) orderInfo.EnterTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) orderInfo.OrderText = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) orderInfo.DrugBillingAttr = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) orderInfo.Dosage = float.Parse(dataReader.GetValue(7).ToString());
                    if (!dataReader.IsDBNull(8)) orderInfo.DosageUnits = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) orderInfo.Administration = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) orderInfo.Frequency = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) orderInfo.FreqDetail = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) orderInfo.StopTime = dataReader.GetDateTime(12);
                    if (!dataReader.IsDBNull(13)) orderInfo.PackCount = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) orderInfo.Doctor = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) orderInfo.Nurse = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) orderInfo.IsStartStop = dataReader.GetValue(16).ToString().Equals("1");
                    if (!dataReader.IsDBNull(17)) orderInfo.OrderStatus = dataReader.GetString(17);
                    if (dataReader.GetFieldType(18) == typeof(System.DateTime))
                        if (!dataReader.IsDBNull(18)) orderInfo.PerformSchedule = dataReader.GetDateTime(18).ToString("HH:mm");
                    if (dataReader.GetFieldType(18) == typeof(System.String))
                        if (!dataReader.IsDBNull(18)) orderInfo.PerformSchedule = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) orderInfo.StopDoctor = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20)) orderInfo.ProcessingEndDateTime = dataReader.GetDateTime(20);
                    if (!dataReader.IsDBNull(21)) orderInfo.StopNurse = dataReader.GetString(21);
                    if (!dataReader.IsDBNull(22)) orderInfo.ShortProcessingEndDateTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23)) orderInfo.ShortStopNurse = dataReader.GetString(23);
                    if (!dataReader.IsDBNull(24)) orderInfo.PerformResult = dataReader.GetString(24);
                    lstOrderInfo.Add(orderInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取病人入院到护理评估期间产生的医嘱数据列表  HJX
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="szOrderClass">医嘱类型</param>
        /// <param name="szOrderText">医嘱文本</param>
        /// <param name="dtFrom">起始下达时间</param>
        /// <param name="dtTo">截止下达时间</param>
        /// <param name="lstOrderInfo">医嘱信息列表</param>
        /// <returns>ReturnValue</returns>
        public short GetPerformOrderList(string szPatientID, string szVisitID, string szOrderClass, string szOrderText, DateTime dtFrom, DateTime dtTo, ref List<OrderInfo> lstOrderInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NursingAccess.GetInpOrderList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstOrderInfo == null)
                lstOrderInfo = new List<OrderInfo>();
            else
                lstOrderInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}"
                , ServerData.OrdersView.ORDER_NO, ServerData.OrdersView.ORDER_SUB_NO
                , ServerData.OrdersView.REPEAT_INDICATOR, ServerData.OrdersView.ORDER_CLASS
                , ServerData.OrdersView.ENTER_DATE_TIME, ServerData.OrdersView.ORDER_TEXT
                , ServerData.OrdersView.DRUG_BILLING_ATTR, ServerData.OrdersView.DOSAGE
                , ServerData.OrdersView.DOSAGE_UNITS, ServerData.OrdersView.ADMINISTRATION
                , ServerData.OrdersView.FREQUENCY, ServerData.OrdersView.FREQ_DETAIL
                , ServerData.OrdersView.END_DATE_TIME, ServerData.OrdersView.PACK_COUNT
                , ServerData.OrdersView.DOCTOR, ServerData.OrdersView.NURSE
                , ServerData.OrdersView.START_STOP_INDICATOR, ServerData.OrdersView.ORDER_STATUS
                , ServerData.OrdersView.PERFORM_SCHEDULE, ServerData.OrdersView.STOP_DOCTOR
                , ServerData.OrdersView.PROCESSING_STOP_DATE_TIME, ServerData.OrdersView.STOP_NURSE
                , ServerData.OrdersView.SHORT_PROCESSING_STOP_DATE, ServerData.OrdersView.SHORT_STOP_NURSE
                , ServerData.OrdersView.PERFORM_RESULT);
            string szTable = ServerData.DataView.PERFORM_ORDERS;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4} >= {5} AND {4} < {6}"
                , ServerData.OrdersView.PATIENT_ID, szPatientID, ServerData.OrdersView.VISIT_ID, szVisitID
                , ServerData.OrdersView.ENTER_DATE_TIME
                , base.DataAccess.GetSqlTimeFormat(dtFrom), base.DataAccess.GetSqlTimeFormat(dtTo));

            if (!GlobalMethods.Misc.IsEmptyString(szOrderClass))
            {
                szCondition = string.Format("{0} AND {1} in ('{2}')", szCondition
                    , ServerData.OrdersView.ORDER_CLASS, szOrderClass);
            }

            if (!GlobalMethods.Misc.IsEmptyString(szOrderText))
            {
                szCondition = string.Format("{0} AND {1} in ('{2}')", szCondition
                    , ServerData.OrdersView.ORDER_TEXT, szOrderText);
            }
            string szOrderField = ServerData.OrdersView.ENTER_DATE_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, szTable, szCondition, szOrderField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    OrderInfo orderInfo = new OrderInfo();
                    if (!dataReader.IsDBNull(0)) orderInfo.OrderNO = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) orderInfo.OrderSubNO = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) orderInfo.IsRepeat = dataReader.GetValue(2).ToString().Equals("1");
                    if (!dataReader.IsDBNull(3)) orderInfo.OrderClass = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) orderInfo.EnterTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) orderInfo.OrderText = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) orderInfo.DrugBillingAttr = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) orderInfo.Dosage = float.Parse(dataReader.GetValue(7).ToString());
                    if (!dataReader.IsDBNull(8)) orderInfo.DosageUnits = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) orderInfo.Administration = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) orderInfo.Frequency = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) orderInfo.FreqDetail = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) orderInfo.StopTime = dataReader.GetDateTime(12);
                    if (!dataReader.IsDBNull(13)) orderInfo.PackCount = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) orderInfo.Doctor = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) orderInfo.Nurse = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) orderInfo.IsStartStop = dataReader.GetValue(16).ToString().Equals("1");
                    if (!dataReader.IsDBNull(17)) orderInfo.OrderStatus = dataReader.GetString(17);
                    if (dataReader.GetFieldType(18) == typeof(System.DateTime))
                        if (!dataReader.IsDBNull(18)) orderInfo.PerformSchedule = dataReader.GetDateTime(18).ToString("HH:mm");
                    if (dataReader.GetFieldType(18) == typeof(System.String))
                        if (!dataReader.IsDBNull(18)) orderInfo.PerformSchedule = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) orderInfo.StopDoctor = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20)) orderInfo.ProcessingEndDateTime = dataReader.GetDateTime(20);
                    if (!dataReader.IsDBNull(21)) orderInfo.StopNurse = dataReader.GetString(21);
                    if (!dataReader.IsDBNull(22)) orderInfo.ShortProcessingEndDateTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23)) orderInfo.ShortStopNurse = dataReader.GetString(23);
                    if (!dataReader.IsDBNull(24)) orderInfo.PerformResult = dataReader.GetString(24);
                    lstOrderInfo.Add(orderInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取当次医嘱编号拆分后的医嘱
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="szOrderNO">医嘱编号</param>
        /// <param name="lstPlanOrderInfo">医嘱拆分信息列表</param>
        /// <returns>ReturnValue</returns>
        public short GetPlanOrderList(string szPatientID, string szVisitID, string szOrderNO, ref List<PlanOrderInfo> lstPlanOrderInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID) || GlobalMethods.Misc.IsEmptyString(szOrderNO))
            {
                LogManager.Instance.WriteLog("NursingAccess.GetPlanOrderList", "查询参数为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstPlanOrderInfo == null)
                lstPlanOrderInfo = new List<PlanOrderInfo>();
            else
                lstPlanOrderInfo.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.PlanOrdersView.ORDER_NO, ServerData.PlanOrdersView.ORDERPLAN_SUB_NO
                , ServerData.PlanOrdersView.EXEC_CLASS, ServerData.PlanOrdersView.SCHEDULE_TIME
                , ServerData.PlanOrdersView.PERFORMED_FLAG, ServerData.PlanOrdersView.OPERATOR
                , ServerData.PlanOrdersView.OPERATING_TIME, ServerData.PlanOrdersView.REAL_DOSAGE);
            string szTable = ServerData.DataView.PLAN_ORDERS;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'"
                , ServerData.PlanOrdersView.PATIENT_ID, szPatientID, ServerData.PlanOrdersView.VISIT_ID, szVisitID
                , ServerData.PlanOrdersView.ORDER_NO, szOrderNO);

            string szOrderField = string.Format("{0}"
               , ServerData.PlanOrdersView.ORDERPLAN_SUB_NO);
            string szSQL = string.Format(ServerData.SQL.SELECT_DISTINCT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderField);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    PlanOrderInfo planOrderInfo = new PlanOrderInfo();
                    if (!dataReader.IsDBNull(0)) planOrderInfo.OrderNO = dataReader.GetValue(0).ToString();
                    if (!dataReader.IsDBNull(1)) planOrderInfo.OrderPlanSubNO = dataReader.GetValue(1).ToString();
                    if (!dataReader.IsDBNull(2)) planOrderInfo.ExecClass = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) planOrderInfo.ScheduleTime = dataReader.GetDateTime(3);
                    if (!dataReader.IsDBNull(4)) planOrderInfo.PerformedFlag = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) planOrderInfo.Operator = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) planOrderInfo.OperatingTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) planOrderInfo.RealDosage = int.Parse(dataReader.GetValue(7).ToString());
                    lstPlanOrderInfo.Add(planOrderInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion


    }
}
