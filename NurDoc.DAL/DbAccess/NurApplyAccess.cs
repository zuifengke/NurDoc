// ***********************************************************
// 数据库访问层与护理申请单数据及索引有关的数据的访问类.
// Creator:OuFengFang  Date:2013-3-22
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class NurApplyAccess : DBAccessBase
    {
        #region"读写护理申请索引接口"
        /// <summary>
        /// 根据申请单ID，获取申请单基本信息
        /// </summary>
        /// <param name="szApplyID">申请单编号</param>
        /// <param name="nurApplyInfo">申请单信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfo(string szApplyID, ref NurApplyInfo nurApplyInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szApplyID))
            {
                LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo"
                    , new string[] { "szApplyID" }, new object[] { szApplyID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}"
                , ServerData.NurApplyTable.APPLICANT_ID, ServerData.NurApplyTable.APPLICANT_NAME
                , ServerData.NurApplyTable.APPLY_ID, ServerData.NurApplyTable.APPLY_NAME
                , ServerData.NurApplyTable.APPLY_TIME, ServerData.NurApplyTable.APPLY_TYPE
                , ServerData.NurApplyTable.DOC_ID, ServerData.NurApplyTable.DOCTYPE_ID
                , ServerData.NurApplyTable.MODIFIER_ID, ServerData.NurApplyTable.MODIFIER_NAME
                , ServerData.NurApplyTable.MODIFY_TIME, ServerData.NurApplyTable.PATIENT_ID
                , ServerData.NurApplyTable.PATIENT_NAME, ServerData.NurApplyTable.STATUS
                , ServerData.NurApplyTable.SUB_ID, ServerData.NurApplyTable.URGENCY
                , ServerData.NurApplyTable.VISIT_ID, ServerData.NurApplyTable.WARD_CODE
                , ServerData.NurApplyTable.WARD_NAME);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurApplyTable.APPLY_ID, szApplyID.Trim());
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_APPLY_INDEX, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo", new string[] { "szSQL" }, new object[] { szSQL }, "未查询到记录");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (nurApplyInfo == null)
                    nurApplyInfo = new NurApplyInfo();
                do
                {
                    if (!dataReader.IsDBNull(0)) nurApplyInfo.ApplicantID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nurApplyInfo.ApplicantName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) nurApplyInfo.ApplyID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) nurApplyInfo.ApplyName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nurApplyInfo.ApplyTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) nurApplyInfo.ApplyType = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) nurApplyInfo.DocID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nurApplyInfo.DocTypeID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) nurApplyInfo.ModifierID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) nurApplyInfo.ModifierName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) nurApplyInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11)) nurApplyInfo.PatientID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) nurApplyInfo.PatientName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) nurApplyInfo.Status = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) nurApplyInfo.SubID = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) nurApplyInfo.Urgency = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16)) nurApplyInfo.VisitID = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) nurApplyInfo.WardCode = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) nurApplyInfo.WardName = dataReader.GetString(18);
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
        /// 根据文档集ID获取申请单基本信息
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <param name="nurApplyInfo">申请单基本信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfoByDocSetID(string szDocSetID, ref NurApplyInfo nurApplyInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo"
                    , new string[] { "szApplyID" }, new object[] { szDocSetID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}"
                , ServerData.NurApplyTable.APPLICANT_ID, ServerData.NurApplyTable.APPLICANT_NAME
                , ServerData.NurApplyTable.APPLY_ID, ServerData.NurApplyTable.APPLY_NAME
                , ServerData.NurApplyTable.APPLY_TIME, ServerData.NurApplyTable.APPLY_TYPE
                , ServerData.NurApplyTable.DOC_ID, ServerData.NurApplyTable.DOCTYPE_ID
                , ServerData.NurApplyTable.MODIFIER_ID, ServerData.NurApplyTable.MODIFIER_NAME
                , ServerData.NurApplyTable.MODIFY_TIME, ServerData.NurApplyTable.PATIENT_ID
                , ServerData.NurApplyTable.PATIENT_NAME, ServerData.NurApplyTable.STATUS
                , ServerData.NurApplyTable.SUB_ID, ServerData.NurApplyTable.URGENCY
                , ServerData.NurApplyTable.VISIT_ID, ServerData.NurApplyTable.WARD_CODE
                , ServerData.NurApplyTable.WARD_NAME);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurApplyTable.DOC_ID, szDocSetID.Trim());
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_APPLY_INDEX, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo", new string[] { "szSQL" }, new object[] { szSQL }, "未查询到记录");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (nurApplyInfo == null)
                    nurApplyInfo = new NurApplyInfo();
                do
                {
                    if (!dataReader.IsDBNull(0)) nurApplyInfo.ApplicantID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nurApplyInfo.ApplicantName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) nurApplyInfo.ApplyID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) nurApplyInfo.ApplyName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nurApplyInfo.ApplyTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) nurApplyInfo.ApplyType = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) nurApplyInfo.DocID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nurApplyInfo.DocTypeID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) nurApplyInfo.ModifierID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) nurApplyInfo.ModifierName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) nurApplyInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11)) nurApplyInfo.PatientID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) nurApplyInfo.PatientName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) nurApplyInfo.Status = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) nurApplyInfo.SubID = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) nurApplyInfo.Urgency = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16)) nurApplyInfo.VisitID = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) nurApplyInfo.WardCode = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) nurApplyInfo.WardName = dataReader.GetString(18);
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
        /// 获取病人申请单数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="dtBeginTime">记录时间</param>
        /// <param name="dtEndTime">记录时间</param>
        ///  <param name="szApplyType">申请单类型</param>
        /// <param name="lstNurApplyInfos">申请信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfoList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, string szApplyType, ref List<NurApplyInfo> lstNurApplyInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfoList"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstNurApplyInfos == null)
                lstNurApplyInfos = new List<NurApplyInfo>();
            lstNurApplyInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}"
                , ServerData.NurApplyTable.APPLICANT_ID, ServerData.NurApplyTable.APPLICANT_NAME
                , ServerData.NurApplyTable.APPLY_ID, ServerData.NurApplyTable.APPLY_NAME
                , ServerData.NurApplyTable.APPLY_TIME, ServerData.NurApplyTable.APPLY_TYPE
                , ServerData.NurApplyTable.DOC_ID, ServerData.NurApplyTable.DOCTYPE_ID
                , ServerData.NurApplyTable.MODIFIER_ID, ServerData.NurApplyTable.MODIFIER_NAME
                , ServerData.NurApplyTable.MODIFY_TIME, ServerData.NurApplyTable.PATIENT_ID
                , ServerData.NurApplyTable.PATIENT_NAME, ServerData.NurApplyTable.STATUS
                , ServerData.NurApplyTable.SUB_ID, ServerData.NurApplyTable.URGENCY
                , ServerData.NurApplyTable.VISIT_ID, ServerData.NurApplyTable.WARD_CODE
                , ServerData.NurApplyTable.WARD_NAME);
            string szTable = ServerData.DataTable.NUR_APPLY_INDEX;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}>={7} AND {8}<={9} AND {10}='{11}' "
                , ServerData.NurApplyTable.PATIENT_ID, szPatientID
                , ServerData.NurApplyTable.VISIT_ID, szVisitID
                , ServerData.NurApplyTable.SUB_ID, szSubID
                , ServerData.NurApplyTable.APPLY_TIME, base.DataAccess.GetSqlTimeFormat(dtBeginTime)
                , ServerData.NurApplyTable.APPLY_TIME, base.DataAccess.GetSqlTimeFormat(dtEndTime)
                , ServerData.NurApplyTable.APPLY_TYPE, szApplyType);

            string szOrder = string.Format("{0}", ServerData.NurApplyTable.APPLY_TIME);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    NurApplyInfo nurApplyInfo = new NurApplyInfo();
                    if (!dataReader.IsDBNull(0)) nurApplyInfo.ApplicantID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nurApplyInfo.ApplicantName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) nurApplyInfo.ApplyID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) nurApplyInfo.ApplyName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nurApplyInfo.ApplyTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) nurApplyInfo.ApplyType = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) nurApplyInfo.DocID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nurApplyInfo.DocTypeID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) nurApplyInfo.ModifierID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) nurApplyInfo.ModifierName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) nurApplyInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11)) nurApplyInfo.PatientID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) nurApplyInfo.PatientName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) nurApplyInfo.Status = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) nurApplyInfo.SubID = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) nurApplyInfo.Urgency = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16)) nurApplyInfo.VisitID = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) nurApplyInfo.WardCode = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) nurApplyInfo.WardName = dataReader.GetString(18);
                    lstNurApplyInfos.Add(nurApplyInfo);
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
        /// 新增护理申请信息
        /// </summary>
        /// <param name="nurApplyInfo">护理申请索引信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddNurApplyIndexInfo(NurApplyInfo nurApplyInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}"
                , ServerData.NurApplyTable.APPLICANT_ID, ServerData.NurApplyTable.APPLICANT_NAME
                , ServerData.NurApplyTable.APPLY_ID, ServerData.NurApplyTable.APPLY_NAME
                , ServerData.NurApplyTable.APPLY_TIME, ServerData.NurApplyTable.APPLY_TYPE
                , ServerData.NurApplyTable.DOC_ID, ServerData.NurApplyTable.DOCTYPE_ID
                , ServerData.NurApplyTable.MODIFIER_ID, ServerData.NurApplyTable.MODIFIER_NAME
                , ServerData.NurApplyTable.MODIFY_TIME, ServerData.NurApplyTable.PATIENT_ID
                , ServerData.NurApplyTable.PATIENT_NAME, ServerData.NurApplyTable.STATUS
                , ServerData.NurApplyTable.SUB_ID, ServerData.NurApplyTable.URGENCY
                , ServerData.NurApplyTable.VISIT_ID, ServerData.NurApplyTable.WARD_CODE
                , ServerData.NurApplyTable.WARD_NAME);
            string szValue = string.Format("'{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}',{15},'{16}','{17}','{18}'"
                , nurApplyInfo.ApplicantID, nurApplyInfo.ApplicantName, nurApplyInfo.ApplyID
                , nurApplyInfo.ApplyName, base.DataAccess.GetSqlTimeFormat(nurApplyInfo.ApplyTime), nurApplyInfo.ApplyType
                , nurApplyInfo.DocID, nurApplyInfo.DocTypeID
                , nurApplyInfo.ModifierID, nurApplyInfo.ModifierName, base.DataAccess.GetSqlTimeFormat(nurApplyInfo.ModifyTime)
                , nurApplyInfo.PatientID, nurApplyInfo.PatientName, nurApplyInfo.Status
                , nurApplyInfo.SubID, nurApplyInfo.Urgency, nurApplyInfo.VisitID
                , nurApplyInfo.WardCode, nurApplyInfo.WardName);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_APPLY_INDEX, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.AddNurApplyIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除护理申请索引信息
        /// </summary>
        /// <param name="nurApplyInfo">护理申请索引信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short DelNurApplyIndexInfo(NurApplyInfo nurApplyInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0} = '{1}'"
                , ServerData.NurApplyTable.APPLY_ID, nurApplyInfo.ApplyID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_APPLY_INDEX, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.DelNurApplyIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除护理申请状态信息
        /// </summary>
        /// <param name="ApplicantID">护理申请索引信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short DelNurApplyStatusInfo(string ApplicantID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0} = '{1}'"
                , ServerData.NurApplyTable.APPLY_ID, ApplicantID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_APPLY_STATUS, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.DelNurApplyStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改护理申请信息
        /// </summary>
        /// <param name="nurApplyInfo">护理申请索引信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short ModifyNurApplyIndexInfo(NurApplyInfo nurApplyInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}={7},{8}={9},{10}='{11}'"
                , ServerData.NurApplyTable.DOC_ID, nurApplyInfo.DocID
                , ServerData.NurApplyTable.MODIFIER_ID, nurApplyInfo.ModifierID
                , ServerData.NurApplyTable.MODIFIER_NAME, nurApplyInfo.ModifierName
                , ServerData.NurApplyTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(nurApplyInfo.ModifyTime)
                ,ServerData.NurApplyTable.APPLY_TIME, base.DataAccess.GetSqlTimeFormat(nurApplyInfo.ApplyTime)
                , ServerData.NurApplyTable.STATUS, nurApplyInfo.Status);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurApplyTable.APPLY_ID, nurApplyInfo.ApplyID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_APPLY_INDEX, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.ModifyNurApplyIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"读写申请单状态接口"
        /// <summary>
        /// 获取指定申请、指定操作时间以及指定状态信息
        /// </summary>
        /// <param name="szApplyID">申请单编号</param>
        /// <param name="szStatus">申请单状态</param>
        /// <param name="dtOperateTime">申请单状态创建时间</param>
        /// <param name="nurApplyStatusInfo">申请单状态信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyStatusInfo(string szApplyID, string szStatus, DateTime dtOperateTime, ref NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
               , ServerData.NurApplyStatusTable.APPLY_ID, ServerData.NurApplyStatusTable.NEXT_OPERATOR_ID
               , ServerData.NurApplyStatusTable.NEXT_OPERATOR_NAME, ServerData.NurApplyStatusTable.NEXT_OPERATOR_WARD_CODE
               , ServerData.NurApplyStatusTable.NEXT_OPERATOR_WARD_NAME, ServerData.NurApplyStatusTable.OPERATE_TIME
               , ServerData.NurApplyStatusTable.OPERATOR_ID, ServerData.NurApplyStatusTable.OPERATOR_NAME
               , ServerData.NurApplyStatusTable.STATUS, ServerData.NurApplyStatusTable.STATUS_DESC
               , ServerData.NurApplyStatusTable.STATUS_MESSAGE);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}={5}"
                , ServerData.NurApplyStatusTable.APPLY_ID, szApplyID
                , ServerData.NurApplyStatusTable.STATUS, szStatus
                , ServerData.NurApplyStatusTable.OPERATE_TIME, base.DataAccess.GetSqlTimeFormat(dtOperateTime));
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_APPLY_STATUS, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyStatusInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "没有查询到记录!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (nurApplyStatusInfo == null)
                    nurApplyStatusInfo = new NurApplyStatusInfo();
                if (!dataReader.IsDBNull(0)) nurApplyStatusInfo.ApplyID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1)) nurApplyStatusInfo.NextOperatorID = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2)) nurApplyStatusInfo.NextOperatorName = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3)) nurApplyStatusInfo.NextOperatorWardCode = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4)) nurApplyStatusInfo.NextOperatorWardName = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5)) nurApplyStatusInfo.OperateTime = dataReader.GetDateTime(5);
                if (!dataReader.IsDBNull(6)) nurApplyStatusInfo.OperatorID = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7)) nurApplyStatusInfo.OperatorName = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8)) nurApplyStatusInfo.Status = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9)) nurApplyStatusInfo.StatusDesc = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10)) nurApplyStatusInfo.StatusMessage = dataReader.GetString(10);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定会诊申请、指定状态信息
        /// </summary>
        /// <param name="szApplyID">申请单编号</param>
        /// <param name="szStatus">申请单状态</param>
        /// <param name="nurApplyStatusInfo">申请单状态信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyStatusInfo(string szApplyID, string szStatus, ref NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
               , ServerData.NurApplyStatusTable.APPLY_ID, ServerData.NurApplyStatusTable.NEXT_OPERATOR_ID
               , ServerData.NurApplyStatusTable.NEXT_OPERATOR_NAME, ServerData.NurApplyStatusTable.NEXT_OPERATOR_WARD_CODE
               , ServerData.NurApplyStatusTable.NEXT_OPERATOR_WARD_NAME, ServerData.NurApplyStatusTable.OPERATE_TIME
               , ServerData.NurApplyStatusTable.OPERATOR_ID, ServerData.NurApplyStatusTable.OPERATOR_NAME
               , ServerData.NurApplyStatusTable.STATUS, ServerData.NurApplyStatusTable.STATUS_DESC
               , ServerData.NurApplyStatusTable.STATUS_MESSAGE);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' "
                , ServerData.NurApplyStatusTable.APPLY_ID, szApplyID
                , ServerData.NurApplyStatusTable.STATUS, szStatus);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_APPLY_STATUS, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyStatusInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "没有查询到记录!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (nurApplyStatusInfo == null)
                    nurApplyStatusInfo = new NurApplyStatusInfo();
                if (!dataReader.IsDBNull(0)) nurApplyStatusInfo.ApplyID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1)) nurApplyStatusInfo.NextOperatorID = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2)) nurApplyStatusInfo.NextOperatorName = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3)) nurApplyStatusInfo.NextOperatorWardCode = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4)) nurApplyStatusInfo.NextOperatorWardName = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5)) nurApplyStatusInfo.OperateTime = dataReader.GetDateTime(5);
                if (!dataReader.IsDBNull(6)) nurApplyStatusInfo.OperatorID = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7)) nurApplyStatusInfo.OperatorName = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8)) nurApplyStatusInfo.Status = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9)) nurApplyStatusInfo.StatusDesc = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10)) nurApplyStatusInfo.StatusMessage = dataReader.GetString(10);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 写申请单状态记录
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddNurApplyStatusInfo(NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (nurApplyStatusInfo == null)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.AddNurApplyStatusInfo", "参数不能为null!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
                , ServerData.NurApplyStatusTable.APPLY_ID, ServerData.NurApplyStatusTable.NEXT_OPERATOR_ID
                , ServerData.NurApplyStatusTable.NEXT_OPERATOR_NAME, ServerData.NurApplyStatusTable.NEXT_OPERATOR_WARD_CODE
                , ServerData.NurApplyStatusTable.NEXT_OPERATOR_WARD_NAME, ServerData.NurApplyStatusTable.OPERATE_TIME
                , ServerData.NurApplyStatusTable.OPERATOR_ID, ServerData.NurApplyStatusTable.OPERATOR_NAME
                , ServerData.NurApplyStatusTable.STATUS, ServerData.NurApplyStatusTable.STATUS_DESC
                , ServerData.NurApplyStatusTable.STATUS_MESSAGE);
            string szValue = string.Format("'{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}'"
                , nurApplyStatusInfo.ApplyID, nurApplyStatusInfo.NextOperatorID, nurApplyStatusInfo.NextOperatorName
                , nurApplyStatusInfo.NextOperatorWardCode, nurApplyStatusInfo.NextOperatorWardName
                , base.DataAccess.GetSqlTimeFormat(nurApplyStatusInfo.OperateTime)
                , nurApplyStatusInfo.OperatorID, nurApplyStatusInfo.OperatorName, nurApplyStatusInfo.Status
                , nurApplyStatusInfo.StatusDesc, nurApplyStatusInfo.StatusMessage);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_APPLY_STATUS, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.AddNurApplyStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"保存护理申请接口"
        /// <summary>
        /// 保存指定的一条护理申请信息
        /// </summary>
        /// <param name="nurApplyInfo">申请单信息</param>
        /// <param name="nurApplyStatusInfo">申请单状态信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNurApply(NurApplyInfo nurApplyInfo, NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //保存申请单索引信息记录
            short shRet = this.AddNurApplyIndexInfo(nurApplyInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //添加申请单状态信息记录
            shRet = this.AddNurApplyStatusInfo(nurApplyStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //提交数据库更新
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"更新已有申请单接口"
        /// <summary>
        /// 更新指定的已保存的申请单
        /// </summary>
        /// <param name="nurApplyInfo">申请信息</param>
        /// <param name="nurApplyStatusInfo">申请单状态信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateNurApply(NurApplyInfo nurApplyInfo, NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //保存申请索引信息记录
            short shRet = this.ModifyNurApplyIndexInfo(nurApplyInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //添加申请单状态信息记录
            shRet = this.AddNurApplyStatusInfo(nurApplyStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //提交数据库更新
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        /// <summary>
        /// 删除已有申请单
        /// </summary>
        /// <param name="nurApplyInfo">申请信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DelNurApply(NurApplyInfo nurApplyInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = this.DelNurApplyIndexInfo(nurApplyInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            shRet = this.DelNurApplyStatusInfo(nurApplyInfo.ApplyID);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //提交数据库更新
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据会诊文档集ID获取申请信息
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <param name="NurApplyStatusInfo">护理申请状态信息类</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyStatusInfoByDocSetID(string szDocSetID, ref NurApplyStatusInfo NurApplyStatusInfo)
        {
            NurApplyInfo nurApplyInfo = new NurApplyInfo();
            short shRet = this.GetNurApplyInfoByDocSetID(szDocSetID, ref nurApplyInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return shRet;
            }

            shRet = this.GetNurApplyStatusInfo(nurApplyInfo.ApplyID, nurApplyInfo.Status, ref NurApplyStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return shRet;
            }
            return ServerData.ExecuteResult.OK;
        }
    }
}
