// ***********************************************************
// 数据库访问层生命体征操作有关的数据访问接口.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class VitalSignsAccess : DBAccessBase
    {
        /// <summary>
        /// 获取指定病人指定时间段内的体征数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">病人子ID</param>
        /// <param name="dtBeginTime">记录起始时间</param>
        /// <param name="dtEndTime">记录截止时间</param>
        /// <param name="lstVitalSignsData">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetVitalSignsList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<VitalSignsData> lstVitalSignsData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NursingAccess.GetVitalSignsList"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstVitalSignsData == null)
                lstVitalSignsData = new List<VitalSignsData>();
            lstVitalSignsData.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.VitalSignsView.PATIENT_ID, ServerData.VitalSignsView.VISIT_ID
                , ServerData.VitalSignsView.SUB_ID
                , ServerData.VitalSignsView.RECORD_DATE, ServerData.VitalSignsView.RECORD_TIME
                , ServerData.VitalSignsView.RECORD_NAME, ServerData.VitalSignsView.RECORD_DATA
                , ServerData.VitalSignsView.DATA_TYPE, ServerData.VitalSignsView.DATA_UNIT
                , ServerData.VitalSignsView.CATEGORY, ServerData.VitalSignsView.REMARKS
                , ServerData.VitalSignsView.SOURCE_TAG, ServerData.VitalSignsView.SOURCE_TYPE);
            string szTable = ServerData.DataView.VITAL_SIGNS;

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}>={7} AND {6}<={8}"
                , ServerData.VitalSignsView.PATIENT_ID, szPatientID
                , ServerData.VitalSignsView.VISIT_ID, szVisitID
                , ServerData.VitalSignsView.SUB_ID, szSubID
                , ServerData.VitalSignsView.RECORD_TIME
                , base.DataAccess.GetSqlTimeFormat(dtBeginTime), base.DataAccess.GetSqlTimeFormat(dtEndTime));
            string szOrder = ServerData.VitalSignsView.RECORD_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    VitalSignsData vitalSignsData = new VitalSignsData();
                    if (!dataReader.IsDBNull(0)) vitalSignsData.PatientID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) vitalSignsData.VisitID = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) vitalSignsData.SubID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) vitalSignsData.RecordDate = dataReader.GetDateTime(3);
                    if (!dataReader.IsDBNull(4)) vitalSignsData.RecordTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) vitalSignsData.RecordName = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) vitalSignsData.RecordData = dataReader.GetValue(6).ToString();
                    if (!dataReader.IsDBNull(7)) vitalSignsData.DataType = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) vitalSignsData.DataUnit = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) vitalSignsData.Category = int.Parse(dataReader.GetValue(9).ToString());
                    if (!dataReader.IsDBNull(10)) vitalSignsData.Remarks = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) vitalSignsData.SourceTag = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) vitalSignsData.SourceType = dataReader.GetString(12);
                    lstVitalSignsData.Add(vitalSignsData);
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
        /// 获取指定的病人指定的数据来源对应的体征数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="szSubID">子ID号</param>
        /// <param name="szSourceTag">数据来源标记</param>
        /// <param name="szSourceType">数据来源类型标记</param>
        /// <param name="lstVitalSignsData">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetVitalSignsList(string szPatientID, string szVisitID, string szSubID
            , string szSourceTag, string szSourceType, ref List<VitalSignsData> lstVitalSignsData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szSourceTag) || GlobalMethods.Misc.IsEmptyString(szSourceType))
            {
                LogManager.Instance.WriteLog("NursingAccess.GetVitalSignsList"
                    , new string[] { "szSourceTag", "szSourceType" }, new object[] { szSourceTag, szSourceType }, "数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstVitalSignsData == null)
                lstVitalSignsData = new List<VitalSignsData>();
            lstVitalSignsData.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.VitalSignsView.PATIENT_ID, ServerData.VitalSignsView.VISIT_ID
                , ServerData.VitalSignsView.SUB_ID
                , ServerData.VitalSignsView.RECORD_DATE, ServerData.VitalSignsView.RECORD_TIME
                , ServerData.VitalSignsView.RECORD_NAME, ServerData.VitalSignsView.RECORD_DATA
                , ServerData.VitalSignsView.DATA_TYPE, ServerData.VitalSignsView.DATA_UNIT
                , ServerData.VitalSignsView.CATEGORY, ServerData.VitalSignsView.REMARKS
                , ServerData.VitalSignsView.SOURCE_TAG, ServerData.VitalSignsView.SOURCE_TYPE);
            string szTable = ServerData.DataView.VITAL_SIGNS;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='{7}' AND {8}='{9}'"
                , ServerData.VitalSignsView.PATIENT_ID, szPatientID
                , ServerData.VitalSignsView.VISIT_ID, szVisitID
                , ServerData.VitalSignsView.SUB_ID, szSubID
                , ServerData.VitalSignsView.SOURCE_TAG, szSourceTag
                , ServerData.VitalSignsView.SOURCE_TYPE, szSourceType);
            string szOrder = ServerData.VitalSignsView.RECORD_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    VitalSignsData vitalSignsData = new VitalSignsData();
                    if (!dataReader.IsDBNull(0)) vitalSignsData.PatientID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) vitalSignsData.VisitID = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) vitalSignsData.SubID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) vitalSignsData.RecordDate = dataReader.GetDateTime(3);
                    if (!dataReader.IsDBNull(4)) vitalSignsData.RecordTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) vitalSignsData.RecordName = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) vitalSignsData.RecordData = dataReader.GetValue(6).ToString();
                    if (!dataReader.IsDBNull(7)) vitalSignsData.DataType = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) vitalSignsData.DataUnit = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) vitalSignsData.Category = int.Parse(dataReader.GetValue(9).ToString());
                    if (!dataReader.IsDBNull(10)) vitalSignsData.Remarks = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) vitalSignsData.SourceTag = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) vitalSignsData.SourceType = dataReader.GetString(12);
                    lstVitalSignsData.Add(vitalSignsData);
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
        /// 保存指定的一系列体征数据列表
        /// </summary>
        /// <param name="lstVitalSignsData">体征数据列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveVitalSignsData(List<VitalSignsData> lstVitalSignsData)
        {
            if (lstVitalSignsData == null || lstVitalSignsData.Count <= 0)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = ServerData.ExecuteResult.OK;
            foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
            {
                if (vitalSignsData == null)
                    continue;
                shRet = this.SaveVitalSignsData(vitalSignsData);
                if (shRet != ServerData.ExecuteResult.OK)
                    break;
            }
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
            }
            else
            {
                if (!base.DataAccess.CommitTransaction(true))
                    return ServerData.ExecuteResult.EXCEPTION;
            }
            return shRet;
        }

        /// <summary>
        /// 保存指定的一条体征数据列表
        /// </summary>
        /// <param name="vitalSignsData">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveVitalSignsData(VitalSignsData vitalSignsData)
        {
            DbParameter[] pmi = new DbParameter[14];
            pmi[0] = new DbParameter("PatientID", vitalSignsData.PatientID);
            pmi[1] = new DbParameter("VisitID", vitalSignsData.VisitID);
            pmi[2] = new DbParameter("SubID", vitalSignsData.SubID);
            pmi[3] = new DbParameter("RecordDate", vitalSignsData.RecordDate);
            pmi[4] = new DbParameter("RecordTime", vitalSignsData.RecordTime);
            pmi[5] = new DbParameter("RecordName", vitalSignsData.RecordName);
            pmi[6] = new DbParameter("RecordData", vitalSignsData.RecordData == null ? string.Empty : vitalSignsData.RecordData);
            pmi[7] = new DbParameter("DataType", vitalSignsData.DataType == null ? string.Empty : vitalSignsData.DataType);
            pmi[8] = new DbParameter("DataUnit", vitalSignsData.DataUnit == null ? string.Empty : vitalSignsData.DataUnit);
            pmi[9] = new DbParameter("Category", vitalSignsData.Category);
            pmi[10] = new DbParameter("ContainsTime", vitalSignsData.ContainsTime ? 1 : 0);
            pmi[11] = new DbParameter("Remarks", vitalSignsData.Remarks == null ? string.Empty : vitalSignsData.Remarks);
            pmi[12] = new DbParameter("SourceTag", vitalSignsData.SourceTag == null ? string.Empty : vitalSignsData.SourceTag);
            pmi[13] = new DbParameter("SourceType", vitalSignsData.SourceType == null ? string.Empty : vitalSignsData.SourceType);

            string szSQL = "SaveVitalSignsData";
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.StoredProcedure, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的一条体征数据信息
        /// </summary>
        /// <param name="vitalSignsData">待删除的体征信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteVitalSignsData(VitalSignsData vitalSignsData)
        {
            if (vitalSignsData == null)
            {
                LogManager.Instance.WriteLog("NursingAccess.DeleteVitalSignsData", new string[] { "vitalSignsData" },
                    new object[] { vitalSignsData }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            vitalSignsData.RecordData = string.Empty;
            return this.SaveVitalSignsData(vitalSignsData);
        }

        /// <summary>
        /// 获取病人体征数据表中存储的入院事件发生的时间
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">病人子ID</param>
        /// <param name="dtAdmissionTime">返回的入院事件时间</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAdmissionEventTime(string szPatientID, string szVisitID, string szSubID, ref DateTime? dtAdmissionTime)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NursingAccess.GetAdmissionEventTime"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1}"
                , ServerData.VitalSignsView.RECORD_TIME, ServerData.VitalSignsView.REMARKS);
            string szTable = ServerData.DataView.VITAL_SIGNS;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='入院'"
                , ServerData.VitalSignsView.PATIENT_ID, szPatientID
                , ServerData.VitalSignsView.VISIT_ID, szVisitID
                , ServerData.VitalSignsView.SUB_ID, szSubID
                , ServerData.VitalSignsView.RECORD_NAME);
            string szOrder = ServerData.VitalSignsView.RECORD_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    if (!dataReader.IsDBNull(0))
                    {
                        dtAdmissionTime = dataReader.GetDateTime(0);
                        break;
                    }
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
    }
}
