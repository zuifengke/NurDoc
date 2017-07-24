// ***********************************************************
// 数据库访问层护理记录操作有关的数据访问接口.
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
    public class NurRecAccess : DBAccessBase
    {
        /// <summary>
        /// 通过护理记录ID号，获取护理记录
        /// </summary>
        /// <param name="szRecordID">护理记录ID号</param>
        /// <param name="nursingRecInfo">返回护理记录信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingRec(string szRecordID, string PatientID, string VisitID, ref NursingRecInfo nursingRecInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szRecordID))
            {
                LogManager.Instance.WriteLog("NurRecAccess.GetNursingRec"
                    , new string[] { "szRecordID" }, new object[] { szRecordID }, "数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25}"
                , ServerData.NursingRecInfoTable.PATIENT_ID, ServerData.NursingRecInfoTable.VISIT_ID
                , ServerData.NursingRecInfoTable.SUB_ID, ServerData.NursingRecInfoTable.RECORD_ID
                , ServerData.NursingRecInfoTable.RECORD_DATE, ServerData.NursingRecInfoTable.RECORD_TIME
                , ServerData.NursingRecInfoTable.PATIENT_NAME, ServerData.NursingRecInfoTable.WARD_CODE
                , ServerData.NursingRecInfoTable.WARD_NAME
                , ServerData.NursingRecInfoTable.CREATOR_ID, ServerData.NursingRecInfoTable.CREATOR_NAME
                , ServerData.NursingRecInfoTable.CREATE_TIME
                , ServerData.NursingRecInfoTable.MODIFIER_ID, ServerData.NursingRecInfoTable.MODIFIER_NAME
                , ServerData.NursingRecInfoTable.MODIFY_TIME
                , ServerData.NursingRecInfoTable.RECORDER1_ID, ServerData.NursingRecInfoTable.RECORDER1_NAME
                , ServerData.NursingRecInfoTable.RECORDER2_ID, ServerData.NursingRecInfoTable.RECORDER2_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_FLAG, ServerData.NursingRecInfoTable.SUMMARY_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_START_TIME
                , ServerData.NursingRecInfoTable.RECORD_CONTENT, ServerData.NursingRecInfoTable.RECORD_REMARKS
                , ServerData.NursingRecInfoTable.RECORD_PRINTED
                , ServerData.NursingRecInfoTable.SCHEMA_ID);
            string szTable = ServerData.DataTable.NUR_REC;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'"
                , ServerData.NursingRecInfoTable.RECORD_ID, szRecordID
                , ServerData.NursingRecInfoTable.PATIENT_ID, PatientID
                , ServerData.NursingRecInfoTable.VISIT_ID, VisitID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (nursingRecInfo == null)
                    nursingRecInfo = new NursingRecInfo();
                if (!dataReader.IsDBNull(0)) nursingRecInfo.PatientID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1)) nursingRecInfo.VisitID = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2)) nursingRecInfo.SubID = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3)) nursingRecInfo.RecordID = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4)) nursingRecInfo.RecordDate = dataReader.GetDateTime(4);
                if (!dataReader.IsDBNull(5)) nursingRecInfo.RecordTime = dataReader.GetDateTime(5);
                if (!dataReader.IsDBNull(6)) nursingRecInfo.PatientName = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7)) nursingRecInfo.WardCode = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8)) nursingRecInfo.WardName = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9)) nursingRecInfo.CreatorID = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10)) nursingRecInfo.CreatorName = dataReader.GetString(10);
                if (!dataReader.IsDBNull(11)) nursingRecInfo.CreateTime = dataReader.GetDateTime(11);
                if (!dataReader.IsDBNull(12)) nursingRecInfo.ModifierID = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13)) nursingRecInfo.ModifierName = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14)) nursingRecInfo.ModifyTime = dataReader.GetDateTime(14);
                if (!dataReader.IsDBNull(15)) nursingRecInfo.Recorder1ID = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16)) nursingRecInfo.Recorder1Name = dataReader.GetString(16);
                if (!dataReader.IsDBNull(17)) nursingRecInfo.Recorder2ID = dataReader.GetString(17);
                if (!dataReader.IsDBNull(18)) nursingRecInfo.Recorder2Name = dataReader.GetString(18);
                if (!dataReader.IsDBNull(19)) nursingRecInfo.SummaryFlag = int.Parse(dataReader.GetValue(19).ToString());
                if (!dataReader.IsDBNull(20)) nursingRecInfo.SummaryName = dataReader.GetString(20);
                if (!dataReader.IsDBNull(21)) nursingRecInfo.SummaryStartTime = dataReader.GetDateTime(21);
                if (!dataReader.IsDBNull(22)) nursingRecInfo.RecordContent = dataReader.GetString(22);
                if (!dataReader.IsDBNull(23)) nursingRecInfo.RecordRemarks = dataReader.GetString(23);
                if (!dataReader.IsDBNull(24)) nursingRecInfo.RecordPrinted = int.Parse(dataReader.GetValue(24).ToString());
                if (!dataReader.IsDBNull(25)) nursingRecInfo.SchemaID = dataReader.GetValue(25).ToString();
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取病人护理记录数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="dtBeginTime">记录时间</param>
        /// <param name="dtEndTime">记录时间</param>
        /// <param name="lstNursingRecInfos">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingRecList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<NursingRecInfo> lstNursingRecInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NurRecAccess.GetNursingRecList"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstNursingRecInfos == null)
                lstNursingRecInfos = new List<NursingRecInfo>();
            lstNursingRecInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25}"
                , ServerData.NursingRecInfoTable.PATIENT_ID, ServerData.NursingRecInfoTable.VISIT_ID
                , ServerData.NursingRecInfoTable.SUB_ID, ServerData.NursingRecInfoTable.RECORD_ID
                , ServerData.NursingRecInfoTable.RECORD_DATE, ServerData.NursingRecInfoTable.RECORD_TIME
                , ServerData.NursingRecInfoTable.PATIENT_NAME, ServerData.NursingRecInfoTable.WARD_CODE
                , ServerData.NursingRecInfoTable.WARD_NAME
                , ServerData.NursingRecInfoTable.CREATOR_ID, ServerData.NursingRecInfoTable.CREATOR_NAME
                , ServerData.NursingRecInfoTable.CREATE_TIME
                , ServerData.NursingRecInfoTable.MODIFIER_ID, ServerData.NursingRecInfoTable.MODIFIER_NAME
                , ServerData.NursingRecInfoTable.MODIFY_TIME
                , ServerData.NursingRecInfoTable.RECORDER1_ID, ServerData.NursingRecInfoTable.RECORDER1_NAME
                , ServerData.NursingRecInfoTable.RECORDER2_ID, ServerData.NursingRecInfoTable.RECORDER2_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_FLAG, ServerData.NursingRecInfoTable.SUMMARY_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_START_TIME
                , ServerData.NursingRecInfoTable.RECORD_CONTENT, ServerData.NursingRecInfoTable.RECORD_REMARKS
                , ServerData.NursingRecInfoTable.RECORD_PRINTED
                , ServerData.NursingRecInfoTable.SCHEMA_ID);
            string szTable = ServerData.DataTable.NUR_REC;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}>={7} AND {8}<={9} AND ({10} IS NULL OR {10}!=2)"
                , ServerData.NursingRecInfoTable.PATIENT_ID, szPatientID
                , ServerData.NursingRecInfoTable.VISIT_ID, szVisitID
                , ServerData.NursingRecInfoTable.SUB_ID, szSubID
                , ServerData.NursingRecInfoTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(dtBeginTime)
                , ServerData.NursingRecInfoTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(dtEndTime)
                , ServerData.NursingRecInfoTable.RECORD_STATUS);
            string szOrder = string.Format("{0},{1},{2},{3},{4}", ServerData.NursingRecInfoTable.RECORD_TIME
                , ServerData.NursingRecInfoTable.CREATE_TIME
                , ServerData.NursingRecInfoTable.SUMMARY_FLAG, ServerData.NursingRecInfoTable.SUMMARY_NAME, ServerData.NursingRecInfoTable.RECORD_ID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    NursingRecInfo nursingRecInfo = new NursingRecInfo();
                    if (!dataReader.IsDBNull(0)) nursingRecInfo.PatientID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nursingRecInfo.VisitID = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) nursingRecInfo.SubID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) nursingRecInfo.RecordID = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nursingRecInfo.RecordDate = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) nursingRecInfo.RecordTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6)) nursingRecInfo.PatientName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nursingRecInfo.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) nursingRecInfo.WardName = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) nursingRecInfo.CreatorID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) nursingRecInfo.CreatorName = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) nursingRecInfo.CreateTime = dataReader.GetDateTime(11);
                    if (!dataReader.IsDBNull(12)) nursingRecInfo.ModifierID = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) nursingRecInfo.ModifierName = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) nursingRecInfo.ModifyTime = dataReader.GetDateTime(14);
                    if (!dataReader.IsDBNull(15)) nursingRecInfo.Recorder1ID = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) nursingRecInfo.Recorder1Name = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) nursingRecInfo.Recorder2ID = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) nursingRecInfo.Recorder2Name = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) nursingRecInfo.SummaryFlag = int.Parse(dataReader.GetValue(19).ToString());
                    if (!dataReader.IsDBNull(20)) nursingRecInfo.SummaryName = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21)) nursingRecInfo.SummaryStartTime = dataReader.GetDateTime(21);
                    if (!dataReader.IsDBNull(22)) nursingRecInfo.RecordContent = dataReader.GetString(22);
                    if (!dataReader.IsDBNull(23)) nursingRecInfo.RecordRemarks = dataReader.GetString(23);
                    if (!dataReader.IsDBNull(24)) nursingRecInfo.RecordPrinted = int.Parse(dataReader.GetValue(24).ToString());
                    if (!dataReader.IsDBNull(25)) nursingRecInfo.SchemaID = dataReader.GetValue(25).ToString();
                    lstNursingRecInfos.Add(nursingRecInfo);
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
        /// 获取病人护理记录数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="szSchemaID">列配置ID</param>
        /// <param name="dtBeginTime">记录时间</param>
        /// <param name="dtEndTime">记录时间</param>
        /// <param name="lstNursingRecInfos">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingRecList(string szPatientID, string szVisitID, string szSubID, string szSchemaID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<NursingRecInfo> lstNursingRecInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NurRecAccess.GetNursingRecList"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstNursingRecInfos == null)
                lstNursingRecInfos = new List<NursingRecInfo>();
            lstNursingRecInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25}"
                , ServerData.NursingRecInfoTable.PATIENT_ID, ServerData.NursingRecInfoTable.VISIT_ID
                , ServerData.NursingRecInfoTable.SUB_ID, ServerData.NursingRecInfoTable.RECORD_ID
                , ServerData.NursingRecInfoTable.RECORD_DATE, ServerData.NursingRecInfoTable.RECORD_TIME
                , ServerData.NursingRecInfoTable.PATIENT_NAME, ServerData.NursingRecInfoTable.WARD_CODE
                , ServerData.NursingRecInfoTable.WARD_NAME
                , ServerData.NursingRecInfoTable.CREATOR_ID, ServerData.NursingRecInfoTable.CREATOR_NAME
                , ServerData.NursingRecInfoTable.CREATE_TIME
                , ServerData.NursingRecInfoTable.MODIFIER_ID, ServerData.NursingRecInfoTable.MODIFIER_NAME
                , ServerData.NursingRecInfoTable.MODIFY_TIME
                , ServerData.NursingRecInfoTable.RECORDER1_ID, ServerData.NursingRecInfoTable.RECORDER1_NAME
                , ServerData.NursingRecInfoTable.RECORDER2_ID, ServerData.NursingRecInfoTable.RECORDER2_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_FLAG, ServerData.NursingRecInfoTable.SUMMARY_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_START_TIME
                , ServerData.NursingRecInfoTable.RECORD_CONTENT, ServerData.NursingRecInfoTable.RECORD_REMARKS
                , ServerData.NursingRecInfoTable.RECORD_PRINTED
                , ServerData.NursingRecInfoTable.SCHEMA_ID);
            string szTable = ServerData.DataTable.NUR_REC;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'AND {6}='{7}'AND {8}>={9} AND {10}<={11} AND ({12} IS NULL OR {12}!=2)"
                , ServerData.NursingRecInfoTable.PATIENT_ID, szPatientID
                , ServerData.NursingRecInfoTable.VISIT_ID, szVisitID
                , ServerData.NursingRecInfoTable.SUB_ID, szSubID
                , ServerData.NursingRecInfoTable.SCHEMA_ID, szSchemaID
                , ServerData.NursingRecInfoTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(dtBeginTime)
                , ServerData.NursingRecInfoTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(dtEndTime)
                , ServerData.NursingRecInfoTable.RECORD_STATUS);
            string szOrder = string.Format("{0},{1},{2},{3},{4}", ServerData.NursingRecInfoTable.RECORD_TIME
                , ServerData.NursingRecInfoTable.CREATE_TIME
                , ServerData.NursingRecInfoTable.SUMMARY_FLAG, ServerData.NursingRecInfoTable.SUMMARY_NAME, ServerData.NursingRecInfoTable.RECORD_ID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    NursingRecInfo nursingRecInfo = new NursingRecInfo();
                    if (!dataReader.IsDBNull(0)) nursingRecInfo.PatientID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nursingRecInfo.VisitID = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) nursingRecInfo.SubID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) nursingRecInfo.RecordID = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nursingRecInfo.RecordDate = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5)) nursingRecInfo.RecordTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6)) nursingRecInfo.PatientName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nursingRecInfo.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) nursingRecInfo.WardName = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) nursingRecInfo.CreatorID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) nursingRecInfo.CreatorName = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) nursingRecInfo.CreateTime = dataReader.GetDateTime(11);
                    if (!dataReader.IsDBNull(12)) nursingRecInfo.ModifierID = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) nursingRecInfo.ModifierName = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) nursingRecInfo.ModifyTime = dataReader.GetDateTime(14);
                    if (!dataReader.IsDBNull(15)) nursingRecInfo.Recorder1ID = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) nursingRecInfo.Recorder1Name = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) nursingRecInfo.Recorder2ID = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) nursingRecInfo.Recorder2Name = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) nursingRecInfo.SummaryFlag = int.Parse(dataReader.GetValue(19).ToString());
                    if (!dataReader.IsDBNull(20)) nursingRecInfo.SummaryName = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21)) nursingRecInfo.SummaryStartTime = dataReader.GetDateTime(21);
                    if (!dataReader.IsDBNull(22)) nursingRecInfo.RecordContent = dataReader.GetString(22);
                    if (!dataReader.IsDBNull(23)) nursingRecInfo.RecordRemarks = dataReader.GetString(23);
                    if (!dataReader.IsDBNull(24)) nursingRecInfo.RecordPrinted = int.Parse(dataReader.GetValue(24).ToString());
                    if (!dataReader.IsDBNull(25)) nursingRecInfo.SchemaID = dataReader.GetValue(25).ToString();
                    lstNursingRecInfos.Add(nursingRecInfo);
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
        /// 保存指定的护理记录
        /// </summary>
        /// <param name="nursingRecInfo">护理记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNursingRec(NursingRecInfo nursingRecInfo)
        {
            if (nursingRecInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}"
                , ServerData.NursingRecInfoTable.PATIENT_ID, ServerData.NursingRecInfoTable.VISIT_ID
                , ServerData.NursingRecInfoTable.SUB_ID, ServerData.NursingRecInfoTable.RECORD_ID
                , ServerData.NursingRecInfoTable.RECORD_DATE, ServerData.NursingRecInfoTable.RECORD_TIME
                , ServerData.NursingRecInfoTable.PATIENT_NAME, ServerData.NursingRecInfoTable.WARD_CODE
                , ServerData.NursingRecInfoTable.WARD_NAME
                , ServerData.NursingRecInfoTable.CREATOR_ID, ServerData.NursingRecInfoTable.CREATOR_NAME
                , ServerData.NursingRecInfoTable.CREATE_TIME
                , ServerData.NursingRecInfoTable.MODIFIER_ID, ServerData.NursingRecInfoTable.MODIFIER_NAME
                , ServerData.NursingRecInfoTable.MODIFY_TIME
                , ServerData.NursingRecInfoTable.RECORDER1_ID, ServerData.NursingRecInfoTable.RECORDER1_NAME
                , ServerData.NursingRecInfoTable.RECORDER2_ID, ServerData.NursingRecInfoTable.RECORDER2_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_FLAG, ServerData.NursingRecInfoTable.SUMMARY_NAME
                , ServerData.NursingRecInfoTable.SUMMARY_START_TIME
                , ServerData.NursingRecInfoTable.RECORD_CONTENT, ServerData.NursingRecInfoTable.RECORD_REMARKS
                , ServerData.NursingRecInfoTable.SCHEMA_ID);
            string szValues = string.Format("'{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}','{10}',{11},'{12}','{13}',{14},'{15}','{16}','{17}','{18}',{19},'{20}',{21},{22},{23},'{24}'"
                , nursingRecInfo.PatientID, nursingRecInfo.VisitID, nursingRecInfo.SubID, nursingRecInfo.RecordID
                , base.DataAccess.GetSqlTimeFormat(nursingRecInfo.RecordDate)
                , base.DataAccess.GetSqlTimeFormat(nursingRecInfo.RecordTime)
                , nursingRecInfo.PatientName, nursingRecInfo.WardCode, nursingRecInfo.WardName
                , nursingRecInfo.CreatorID, nursingRecInfo.CreatorName
                , base.DataAccess.GetSqlTimeFormat(nursingRecInfo.CreateTime)
                , nursingRecInfo.ModifierID, nursingRecInfo.ModifierName
                , base.DataAccess.GetSqlTimeFormat(nursingRecInfo.ModifyTime)
                , nursingRecInfo.Recorder1ID, nursingRecInfo.Recorder1Name
                , nursingRecInfo.Recorder2ID, nursingRecInfo.Recorder2Name
                , nursingRecInfo.SummaryFlag, nursingRecInfo.SummaryName
                , base.DataAccess.GetSqlTimeFormat(nursingRecInfo.SummaryStartTime)
                , base.DataAccess.GetSqlParamName("RecordContent"), base.DataAccess.GetSqlParamName("RecordRemarks")
                , nursingRecInfo.SchemaID);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_REC, szFields, szValues);

            DbParameter[] pmi = new DbParameter[2];
            pmi[0] = new DbParameter("RecordContent", nursingRecInfo.RecordContent);
            pmi[1] = new DbParameter("RecordRemarks", nursingRecInfo.RecordRemarks);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存指定的一条体征数据列表
        /// </summary>
        /// <param name="nursingRecInfo">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateNursingRec(NursingRecInfo nursingRecInfo)
        {
            if (nursingRecInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}={1},{2}={3},{4}='{5}',{6}='{7}',{8}={9},{10}='{11}',{12}='{13}',{14}='{15}',{16}='{17}',{18}={19},{20}='{21}',{22}={23},{24}={25},{26}={27},{28}=1,{29}='{30}'"
                , ServerData.NursingRecInfoTable.RECORD_DATE, base.DataAccess.GetSqlTimeFormat(nursingRecInfo.RecordDate)
                , ServerData.NursingRecInfoTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(nursingRecInfo.RecordTime)
                , ServerData.NursingRecInfoTable.MODIFIER_ID, nursingRecInfo.ModifierID
                , ServerData.NursingRecInfoTable.MODIFIER_NAME, nursingRecInfo.ModifierName
                , ServerData.NursingRecInfoTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(nursingRecInfo.ModifyTime)
                , ServerData.NursingRecInfoTable.RECORDER1_ID, nursingRecInfo.Recorder1ID
                , ServerData.NursingRecInfoTable.RECORDER1_NAME, nursingRecInfo.Recorder1Name
                , ServerData.NursingRecInfoTable.RECORDER2_ID, nursingRecInfo.Recorder2ID
                , ServerData.NursingRecInfoTable.RECORDER2_NAME, nursingRecInfo.Recorder2Name
                , ServerData.NursingRecInfoTable.SUMMARY_FLAG, nursingRecInfo.SummaryFlag
                , ServerData.NursingRecInfoTable.SUMMARY_NAME, nursingRecInfo.SummaryName
                , ServerData.NursingRecInfoTable.SUMMARY_START_TIME, base.DataAccess.GetSqlTimeFormat(nursingRecInfo.SummaryStartTime)
                , ServerData.NursingRecInfoTable.RECORD_CONTENT, base.DataAccess.GetSqlParamName("RecordContent")
                , ServerData.NursingRecInfoTable.RECORD_REMARKS, base.DataAccess.GetSqlParamName("RecordRemarks")
                , ServerData.NursingRecInfoTable.RECORD_STATUS
                , ServerData.NursingRecInfoTable.SCHEMA_ID, nursingRecInfo.SchemaID);

            string szCondition = string.Format("{0}='{1}'", ServerData.NursingRecInfoTable.RECORD_ID, nursingRecInfo.RecordID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_REC, szFields, szCondition);

            DbParameter[] pmi = new DbParameter[2];
            pmi[0] = new DbParameter("RecordContent", nursingRecInfo.RecordContent);
            pmi[1] = new DbParameter("RecordRemarks", nursingRecInfo.RecordRemarks);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据病人ID号,住院标识以及记录时间,删除一条护理记录数据信息
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">住院标识</param>
        /// <param name="szSubID">子ID</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <param name="szModifierID">修改人ID</param>
        /// <param name="szModifierName">修改人姓名</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteNursingRec(string szPatientID, string szVisitID, string szSubID
            , DateTime dtRecordTime, string szModifierID, string szModifierName)
        {
            if (string.IsNullOrEmpty(szPatientID) || string.IsNullOrEmpty(szVisitID)
                || string.IsNullOrEmpty(szModifierID) || string.IsNullOrEmpty(szModifierName))
            {
                LogManager.Instance.WriteLog("NurRecAccess.DeleteNursingRec"
                    , new string[] { "szPatientID", "szVisitID", "szModifierID", "szModifierName" }
                    , new object[] { szPatientID, szVisitID, szModifierID, szModifierName }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}='{1}',{2}='{3}',{4}={5},{6}=2"
                , ServerData.NursingRecInfoTable.MODIFIER_ID, szModifierID
                , ServerData.NursingRecInfoTable.MODIFIER_NAME, szModifierName
                , ServerData.NursingRecInfoTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql()
                , ServerData.NursingRecInfoTable.RECORD_STATUS);

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}={7}"
                , ServerData.NursingRecInfoTable.PATIENT_ID, szPatientID
                , ServerData.NursingRecInfoTable.VISIT_ID, szVisitID
                , ServerData.NursingRecInfoTable.SUB_ID, szSubID
                , ServerData.NursingRecInfoTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(dtRecordTime));
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_REC, szFields, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }

        /// <summary>
        /// 根据护理记录ID号,删除一条护理记录数据信息
        /// </summary>
        /// <param name="szRecordID">记录ID</param>
        /// <param name="szModifierID">修改人ID</param>
        /// <param name="szModifierName">修改人姓名</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteNursingRec(string szRecordID, string szModifierID, string szModifierName)
        {
            if (string.IsNullOrEmpty(szRecordID)
                || string.IsNullOrEmpty(szModifierID) || string.IsNullOrEmpty(szModifierName))
            {
                LogManager.Instance.WriteLog("NurRecAccess.DeleteNursingRec"
                    , new string[] { "szRecordID", "szModifierID", "szModifierName" }
                    , new object[] { szRecordID, szModifierID, szModifierName }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}='{1}',{2}='{3}',{4}={5},{6}=2"
                , ServerData.NursingRecInfoTable.MODIFIER_ID, szModifierID
                , ServerData.NursingRecInfoTable.MODIFIER_NAME, szModifierName
                , ServerData.NursingRecInfoTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql()
                , ServerData.NursingRecInfoTable.RECORD_STATUS);

            string szCondition = string.Format("{0} = '{1}'", ServerData.NursingRecInfoTable.RECORD_ID, szRecordID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_REC, szFields, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }

        /// <summary>
        /// 根据病人ID和就诊号及护理记录记录时间删除指定的护理记录
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitId">病人就诊号</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <param name="szModifierID">修改人ID号</param>
        /// <param name="szModifierName">修改人姓名</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteNursingRec(string szPatientID, string szVisitId, DateTime dtRecordTime
            , string szModifierID, string szModifierName)
        {
            if (string.IsNullOrEmpty(szPatientID)
                || string.IsNullOrEmpty(szModifierID) || string.IsNullOrEmpty(szModifierName)
                || string.IsNullOrEmpty(szVisitId))
            {
                LogManager.Instance.WriteLog("NurRecAccess.DeleteNursingRec"
                    , new string[] { "szPatientID", "szModifierID", "szModifierName", "szVisitId" }
                    , new object[] { szPatientID, szModifierID, szModifierName, szVisitId }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}='{1}',{2}='{3}',{4}={5},{6}=2"
                , ServerData.NursingRecInfoTable.MODIFIER_ID, szModifierID
                , ServerData.NursingRecInfoTable.MODIFIER_NAME, szModifierName
                , ServerData.NursingRecInfoTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql()
                , ServerData.NursingRecInfoTable.RECORD_STATUS);

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}={5}"
                , ServerData.NursingRecInfoTable.PATIENT_ID, szPatientID
                , ServerData.NursingRecInfoTable.VISIT_ID, szVisitId
                , ServerData.NursingRecInfoTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(dtRecordTime));
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_REC, szFields, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }

        /// <summary>
        /// 删除指定的文档ID所包含的文档摘要数据
        /// </summary>
        /// <param name="szRecordID">护理文档集ID</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteSummaryData(string szRecordID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szRecordID))
            {
                LogManager.Instance.WriteLog("NurRecAccess.DeleteSummaryData"
                    , new string[] { "szRecordID" }, new object[] { szRecordID }, "文档集数据不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.SummaryDataTable.RECORD_ID, szRecordID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        ///<summary>
        /// 更新护理记录打印状态信息
        /// </summary>
        ///<param name="szRecordID">护理记录ID</param>
        ///<returns>ServerData.ExecuteResult</returns>
        public short UpdatePrintState(string szRecordID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szRecordID))
            {
                LogManager.Instance.WriteLog("NurRecAccess.UpdatePrintState"
                    , new string[] { "szRecordID" }, new object[] { szRecordID }, "数据不可为空！");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}={1}", ServerData.NursingRecInfoTable.RECORD_PRINTED, 1);
            string szTable = ServerData.DataTable.NUR_REC;
            string szCondition = string.Format("{0}='{1}'", ServerData.NursingRecInfoTable.RECORD_ID, szRecordID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }

        public short SavePrintLog(RecPrintinfo RecPrintinfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7} "
                , ServerData.RecPrintLog.USER_ID, ServerData.RecPrintLog.USER_NAME
                , ServerData.RecPrintLog.WARD_CODE, ServerData.RecPrintLog.PATIENT_ID
                , ServerData.RecPrintLog.VISIT_ID, ServerData.RecPrintLog.SCHEMA_ID
                , ServerData.RecPrintLog.PRINT_PAGES, ServerData.RecPrintLog.PRINT_DATE);
            string szValue = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7}"
                , RecPrintinfo.UserID, RecPrintinfo.UserName
                , RecPrintinfo.WardCode, RecPrintinfo.PatientID
                , RecPrintinfo.VisitID, RecPrintinfo.SchemaID
                , RecPrintinfo.PrintPages, base.DataAccess.GetSqlTimeFormat(RecPrintinfo.PrintTime));

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_REC_PRINT_LOG, szField, szValue);

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
                LogManager.Instance.WriteLog("DocumentAccess.AddDocIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        public short GetPrintLog(string szPatientID, string szVisitID, string szWardCode, string szSchemaID, ref RecPrintinfo RecPrintinfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7} "
                , ServerData.RecPrintLog.USER_ID, ServerData.RecPrintLog.USER_NAME
                , ServerData.RecPrintLog.WARD_CODE, ServerData.RecPrintLog.PATIENT_ID
                , ServerData.RecPrintLog.VISIT_ID, ServerData.RecPrintLog.SCHEMA_ID
                , ServerData.RecPrintLog.PRINT_PAGES, ServerData.RecPrintLog.PRINT_DATE);
            string szCondition = string.Format("{0}='{1}'and {2}='{3}' and {4}='{5}' and {6}='{7}'"
                , ServerData.RecPrintLog.PATIENT_ID, szPatientID
                , ServerData.RecPrintLog.VISIT_ID, szVisitID
                , ServerData.RecPrintLog.WARD_CODE, szWardCode
                , ServerData.RecPrintLog.SCHEMA_ID, szSchemaID);
            string szSQL = string.Format(ServerData.SQL.SELECT_DISTINCT_WHERE_ORDER_DESC, szField
                , ServerData.DataTable.NUR_REC_PRINT_LOG, szCondition, ServerData.RecPrintLog.PRINT_DATE);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocTypeAccess.GetDocTypeInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "记录不存在!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (RecPrintinfo == null)
                    RecPrintinfo = new RecPrintinfo();
                if (!dataReader.IsDBNull(0))
                    RecPrintinfo.UserID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1))
                    RecPrintinfo.UserName = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    RecPrintinfo.WardCode = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    RecPrintinfo.PatientID = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    RecPrintinfo.VisitID = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    RecPrintinfo.SchemaID = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
                    RecPrintinfo.PrintPages = int.Parse(dataReader.GetValue(6).ToString());
                if (!dataReader.IsDBNull(7))
                    RecPrintinfo.PrintTime = dataReader.GetDateTime(7);
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
