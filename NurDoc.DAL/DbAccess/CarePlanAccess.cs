// ***********************************************************
// ���ݿ���ʲ��뻤��ƻ����ݼ������йص����ݵķ�����.
// Creator:OuFengFang  Date:2013-4-2
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
    public class CarePlanAccess : DBAccessBase
    {
        #region"��д����ƻ���¼�����ӿ�"
        /// <summary>
        /// ���ݼƻ���¼ID����ȡ����ƻ�������Ϣ
        /// </summary>
        /// <param name="szNCPID">����ƻ���¼ID</param>
        /// <param name="ncpInfo">����ƻ���Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurCarePlanInfo(string szNCPID, ref NurCarePlanInfo ncpInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szNCPID))
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.GetNurCarePlanInfo"
                    , new string[] { "szNCPID" }, new object[] { szNCPID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szNCPID = szNCPID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}"
              , ServerData.NurCarePlanTable.CREATE_TIME, ServerData.NurCarePlanTable.CREATOR_ID, ServerData.NurCarePlanTable.CREATOR_NAME
              , ServerData.NurCarePlanTable.DIAGNOSIS_CODE, ServerData.NurCarePlanTable.DIAGNOSIS_NAME, ServerData.NurCarePlanTable.DOC_ID
              , ServerData.NurCarePlanTable.DOCTYPE_ID, ServerData.NurCarePlanTable.END_TIME
              , ServerData.NurCarePlanTable.MODIFIER_ID, ServerData.NurCarePlanTable.MODIFIER_NAME, ServerData.NurCarePlanTable.MODIFY_TIME
              , ServerData.NurCarePlanTable.NCP_ID, ServerData.NurCarePlanTable.PATIENT_ID, ServerData.NurCarePlanTable.PATIENT_NAME
              , ServerData.NurCarePlanTable.START_TIME, ServerData.NurCarePlanTable.STATUS, ServerData.NurCarePlanTable.SUB_ID
              , ServerData.NurCarePlanTable.VISIT_ID, ServerData.NurCarePlanTable.WARD_CODE, ServerData.NurCarePlanTable.WARD_NAME);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurCarePlanTable.NCP_ID, szNCPID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_CARE_PLAN_INDEX, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("NurCarePlanAccess.GetNurCarePlanInfo"
                        , new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (ncpInfo == null)
                    ncpInfo = new NurCarePlanInfo();
                do
                {
                    if (!dataReader.IsDBNull(0)) ncpInfo.CreateTime = dataReader.GetDateTime(0);
                    if (!dataReader.IsDBNull(1)) ncpInfo.CreatorID = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) ncpInfo.CreatorName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) ncpInfo.DiagCode = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) ncpInfo.DiagName = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) ncpInfo.DocID = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) ncpInfo.DocTypeID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) ncpInfo.EndTime = dataReader.GetDateTime(7);
                    if (!dataReader.IsDBNull(8)) ncpInfo.ModifierID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) ncpInfo.ModifierName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) ncpInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11)) ncpInfo.NCPID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) ncpInfo.PatientID = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) ncpInfo.PatientName = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) ncpInfo.StartTime = dataReader.GetDateTime(14);
                    if (!dataReader.IsDBNull(15)) ncpInfo.Status = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) ncpInfo.SubID = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) ncpInfo.VisitID = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) ncpInfo.WardCode = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) ncpInfo.WardName = dataReader.GetString(19);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡ���˻���ƻ���¼�����б�
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="szSubID">������ID</param>
        /// <param name="dtBeginTime">��¼ʱ��</param>
        /// <param name="dtEndTime">��¼ʱ��</param>
        /// <param name="lstNurCarePlanInfos">����ƻ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurCarePlanInfoList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<NurCarePlanInfo> lstNurCarePlanInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.GetNurCarePlanInfoList"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "���ݲ���Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstNurCarePlanInfos == null)
                lstNurCarePlanInfos = new List<NurCarePlanInfo>();
            lstNurCarePlanInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}"
               , ServerData.NurCarePlanTable.CREATE_TIME, ServerData.NurCarePlanTable.CREATOR_ID, ServerData.NurCarePlanTable.CREATOR_NAME
               , ServerData.NurCarePlanTable.DIAGNOSIS_CODE, ServerData.NurCarePlanTable.DIAGNOSIS_NAME, ServerData.NurCarePlanTable.DOC_ID
               , ServerData.NurCarePlanTable.DOCTYPE_ID, ServerData.NurCarePlanTable.END_TIME
               , ServerData.NurCarePlanTable.MODIFIER_ID, ServerData.NurCarePlanTable.MODIFIER_NAME, ServerData.NurCarePlanTable.MODIFY_TIME
               , ServerData.NurCarePlanTable.NCP_ID, ServerData.NurCarePlanTable.PATIENT_ID, ServerData.NurCarePlanTable.PATIENT_NAME
               , ServerData.NurCarePlanTable.START_TIME, ServerData.NurCarePlanTable.STATUS, ServerData.NurCarePlanTable.SUB_ID
               , ServerData.NurCarePlanTable.VISIT_ID, ServerData.NurCarePlanTable.WARD_CODE, ServerData.NurCarePlanTable.WARD_NAME);
            string szTable = ServerData.DataTable.NUR_CARE_PLAN_INDEX;
            string szCondition = string.Empty;

            szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}>={7} AND {8}<={9} AND ({10} IS NULL OR {10}!='4') "
                , ServerData.NurCarePlanTable.PATIENT_ID, szPatientID
                , ServerData.NurCarePlanTable.VISIT_ID, szVisitID
                , ServerData.NurCarePlanTable.SUB_ID, szSubID
                , ServerData.NurCarePlanTable.CREATE_TIME, base.DataAccess.GetSqlTimeFormat(dtBeginTime)
                , ServerData.NurCarePlanTable.CREATE_TIME, base.DataAccess.GetSqlTimeFormat(dtEndTime)
                , ServerData.NurCarePlanTable.STATUS);

            string szOrder = string.Format("{0}", ServerData.NurCarePlanTable.CREATE_TIME);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    NurCarePlanInfo ncpInfo = new NurCarePlanInfo();
                    if (!dataReader.IsDBNull(0)) ncpInfo.CreateTime = dataReader.GetDateTime(0);
                    if (!dataReader.IsDBNull(1)) ncpInfo.CreatorID = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) ncpInfo.CreatorName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) ncpInfo.DiagCode = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) ncpInfo.DiagName = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) ncpInfo.DocID = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) ncpInfo.DocTypeID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) ncpInfo.EndTime = dataReader.GetDateTime(7);
                    if (!dataReader.IsDBNull(8)) ncpInfo.ModifierID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) ncpInfo.ModifierName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) ncpInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11)) ncpInfo.NCPID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) ncpInfo.PatientID = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) ncpInfo.PatientName = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) ncpInfo.StartTime = dataReader.GetDateTime(14);
                    if (!dataReader.IsDBNull(15)) ncpInfo.Status = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) ncpInfo.SubID = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) ncpInfo.VisitID = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) ncpInfo.WardCode = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) ncpInfo.WardName = dataReader.GetString(19);
                    lstNurCarePlanInfos.Add(ncpInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��������ƻ���Ϣ
        /// </summary>
        /// <param name="ncpInfo">����ƻ�������Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddNurCarePlanIndexInfo(NurCarePlanInfo ncpInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}"
             , ServerData.NurCarePlanTable.CREATE_TIME, ServerData.NurCarePlanTable.CREATOR_ID, ServerData.NurCarePlanTable.CREATOR_NAME
             , ServerData.NurCarePlanTable.DIAGNOSIS_CODE, ServerData.NurCarePlanTable.DIAGNOSIS_NAME, ServerData.NurCarePlanTable.DOC_ID
             , ServerData.NurCarePlanTable.DOCTYPE_ID, ServerData.NurCarePlanTable.END_TIME
             , ServerData.NurCarePlanTable.MODIFIER_ID, ServerData.NurCarePlanTable.MODIFIER_NAME, ServerData.NurCarePlanTable.MODIFY_TIME
             , ServerData.NurCarePlanTable.NCP_ID, ServerData.NurCarePlanTable.PATIENT_ID, ServerData.NurCarePlanTable.PATIENT_NAME
             , ServerData.NurCarePlanTable.START_TIME, ServerData.NurCarePlanTable.STATUS, ServerData.NurCarePlanTable.SUB_ID
             , ServerData.NurCarePlanTable.VISIT_ID, ServerData.NurCarePlanTable.WARD_CODE, ServerData.NurCarePlanTable.WARD_NAME);
            string szValue = string.Format("{0},'{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}',{10},'{11}','{12}','{13}',{14},'{15}','{16}','{17}','{18}','{19}'"
                , base.DataAccess.GetSqlTimeFormat(ncpInfo.CreateTime), ncpInfo.CreatorID, ncpInfo.CreatorName
                , ncpInfo.DiagCode, ncpInfo.DiagName, ncpInfo.DocID
                , ncpInfo.DocTypeID, base.DataAccess.GetSqlTimeFormat(ncpInfo.EndTime)
                , ncpInfo.ModifierID, ncpInfo.ModifierName, base.DataAccess.GetSqlTimeFormat(ncpInfo.ModifyTime)
                , ncpInfo.NCPID, ncpInfo.PatientID, ncpInfo.PatientName
                , base.DataAccess.GetSqlTimeFormat(ncpInfo.StartTime), ncpInfo.Status, ncpInfo.SubID
                , ncpInfo.VisitID, ncpInfo.WardCode, ncpInfo.WardName);

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_CARE_PLAN_INDEX, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.AddNurCarePlanIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸Ļ���ƻ���Ϣ
        /// </summary>
        /// <param name="ncpInfo">����ƻ�������Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short ModifyNurCarePlanIndexInfo(NurCarePlanInfo ncpInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}='{3}',{4}={5},{6}='{7}',{8}='{9}',{10}={11},{12}={13},{14}='{15}'"
                , ServerData.NurCarePlanTable.DIAGNOSIS_CODE, ncpInfo.DiagCode
                , ServerData.NurCarePlanTable.DIAGNOSIS_NAME, ncpInfo.DiagName
                , ServerData.NurCarePlanTable.END_TIME, base.DataAccess.GetSqlTimeFormat(ncpInfo.EndTime)
                , ServerData.NurCarePlanTable.MODIFIER_ID, ncpInfo.ModifierID
                , ServerData.NurCarePlanTable.MODIFIER_NAME, ncpInfo.ModifierName
                , ServerData.NurCarePlanTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(ncpInfo.ModifyTime)
                , ServerData.NurCarePlanTable.START_TIME, base.DataAccess.GetSqlTimeFormat(ncpInfo.StartTime)
                , ServerData.NurCarePlanTable.STATUS, ncpInfo.Status);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurCarePlanTable.NCP_ID, ncpInfo.NCPID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_CARE_PLAN_INDEX, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.ModifyNurCarePlanIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"��д����ƻ�״̬�ӿ�"
        /// <summary>
        /// ��ȡָ������ƻ���¼ID��ָ������ʱ���Լ�ָ��״̬��Ϣ
        /// </summary>
        /// <param name="szNCPID">����ƻ���¼ID</param>
        /// <param name="szStatus">����ƻ�״̬</param>
        /// <param name="dtOperateTime">����ƻ�״̬����ʱ��</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurCarePlanStatusInfo(string szNCPID, string szStatus, DateTime dtOperateTime, ref NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5}"
               , ServerData.NurCarePlanStatusTable.NCP_ID, ServerData.NurCarePlanStatusTable.OPERATE_TIME, ServerData.NurCarePlanStatusTable.OPERATOR_ID
               , ServerData.NurCarePlanStatusTable.OPERATOR_NAME, ServerData.NurCarePlanStatusTable.STATUS, ServerData.NurCarePlanStatusTable.STATUS_DESC);
            string szCondition = string.Format("{0}='{1}' and {2}='{3}' and {4}={5}", ServerData.NurCarePlanStatusTable.NCP_ID, szNCPID, ServerData.NurCarePlanStatusTable.STATUS, szStatus, ServerData.NurCarePlanStatusTable.OPERATE_TIME, base.DataAccess.GetSqlTimeFormat(dtOperateTime));
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_CARE_PLAN_STATUS, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("NurCarePlanAccess.GetNurCarePlanStatusInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "û�в�ѯ����¼!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (ncpStatusInfo == null)
                    ncpStatusInfo = new NurCarePlanStatusInfo();
                ncpStatusInfo.NCPID = dataReader.GetString(0);
                ncpStatusInfo.OperateTime = dataReader.GetDateTime(1);
                ncpStatusInfo.OperatorID = dataReader.GetString(2);
                ncpStatusInfo.OperatorName = dataReader.GetString(3);
                ncpStatusInfo.Status = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    ncpStatusInfo.StatusDesc = dataReader.GetString(5);

                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// д����ƻ�״̬��¼
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddNurCarePlanStatusInfo(NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (ncpStatusInfo == null)
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.AddNurCarePlanStatusInfo", "��������Ϊnull!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5}"
               , ServerData.NurCarePlanStatusTable.NCP_ID, ServerData.NurCarePlanStatusTable.OPERATE_TIME, ServerData.NurCarePlanStatusTable.OPERATOR_ID
               , ServerData.NurCarePlanStatusTable.OPERATOR_NAME, ServerData.NurCarePlanStatusTable.STATUS, ServerData.NurCarePlanStatusTable.STATUS_DESC);
            string szValue = string.Format("'{0}',{1},'{2}','{3}','{4}','{5}'"
                , ncpStatusInfo.NCPID, base.DataAccess.GetSqlTimeFormat(ncpStatusInfo.OperateTime), ncpStatusInfo.OperatorID
                , ncpStatusInfo.OperatorName, ncpStatusInfo.Status, ncpStatusInfo.StatusDesc);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_CARE_PLAN_STATUS, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.AddNurCarePlanStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        #endregion

        #region"���滤��ƻ��ӿ�"
        /// <summary>
        /// ���滤��ƻ�
        /// </summary>
        /// <param name="ncpInfo">����ƻ���Ϣ</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNurCarePlan(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (ncpInfo == null || ncpStatusInfo == null)
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.SaveNurCarePlan"
                    , new string[] { "ncpInfo", "ncpStatusInfo" }, new object[] { ncpInfo, ncpStatusInfo }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            return this.SaveNurCarePlanToDB(ncpInfo, ncpStatusInfo);
        }

        /// <summary>
        /// �ѻ���ƻ���Ϣ���浽���ݿ�
        /// </summary>
        /// <param name="ncpInfo">����ƻ���Ϣ</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveNurCarePlanToDB(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //����껤��ƻ�״̬��Ϣ��¼
            short shRet = this.AddNurCarePlanStatusInfo(ncpStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //���滤��ƻ�������Ϣ��¼
            shRet = this.AddNurCarePlanIndexInfo(ncpInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //�ύ���ݿ����
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"�������л���ƻ��ӿ�"
        /// <summary>
        /// �������л���ƻ�
        /// </summary>
        /// <param name="ncpInfo">����ƻ���Ϣ</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateNurCarePlan(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("NurCarePlanAccess.UpdateNurCarePlan", "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return this.UpdateNurCarePlanToDB(ncpInfo, ncpStatusInfo);
        }

        /// <summary>
        /// ���»���ƻ������ݿ�
        /// </summary>
        /// <param name="ncpInfo">����ƻ���Ϣ</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short UpdateNurCarePlanToDB(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (ncpInfo == null || base.DataAccess == null || ncpStatusInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //��ӻ����¼״̬��Ϣ��¼
            short shRet = this.AddNurCarePlanStatusInfo(ncpStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //���滤���¼������Ϣ��¼
            shRet = this.ModifyNurCarePlanIndexInfo(ncpInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //�ύ���ݿ����
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region "����ƻ��ֵ�����"
        /// <summary>
        /// ��ѯָ���Ļ�����϶�Ӧ�Ļ���ƻ�,�����ʩ���ֵ�������Ϣ
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <param name="lstNurCarePlanInfos">����ƻ�,�����ʩ���ֵ������б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurCarePlanDictInfo(string szDiagCode, ref List<NurCarePlanDictInfo> lstNurCarePlanInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            if (lstNurCarePlanInfos == null)
                lstNurCarePlanInfos = new List<NurCarePlanDictInfo>();
            lstNurCarePlanInfos.Clear();

            string szField = string.Format("A.{0},B.{1},B.{2},B.{3},B.{4},B.{5},B.{6},B.{7}"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, ServerData.CommonDictTable.ITEM_NO
                , ServerData.CommonDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_NAME
                , ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictTable.WARD_CODE
                , ServerData.CommonDictTable.WARD_NAME, ServerData.CommonDictTable.INPUT_CODE);
            string szTable = string.Format("{0} A, {1} B"
                , ServerData.DataTable.NUR_CARE_PLAN_DICT, ServerData.DataTable.NUR_COMMON_DICT);
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}=B.{3}"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, szDiagCode
                , ServerData.NurCarePlanDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_CODE);
            string szOrderBy = string.Format("B.{0}, B.{1}", ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictTable.ITEM_NO);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderBy);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    NurCarePlanDictInfo nurCarePlan = new NurCarePlanDictInfo();
                    if (!dataReader.IsDBNull(0)) nurCarePlan.DiagCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nurCarePlan.Item.ItemNo = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) nurCarePlan.Item.ItemCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) nurCarePlan.Item.ItemName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nurCarePlan.Item.ItemType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) nurCarePlan.Item.WardCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) nurCarePlan.Item.WardName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nurCarePlan.Item.InputCode = dataReader.GetString(7);
                    lstNurCarePlanInfos.Add(nurCarePlan);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ѯָ�������л����ʩ���ֵ�������Ϣ
        /// </summary>
        /// <param name="lstDiagCode">������ϴ����б�</param>
        /// <param name="lstNurCarePlanInfos">����ƻ�,�����ʩ���ֵ������б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurCarePlanDictInfo(List<string> lstDiagCode, ref List<NurCarePlanDictInfo> lstNurCarePlanInfos)
        {
            if (lstDiagCode == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            if (lstDiagCode.Count == 0)
                return ServerData.ExecuteResult.OK;
            string szDiagCodes = string.Empty;
            foreach (string item in lstDiagCode)
            {
                if (szDiagCodes == string.Empty)
                    szDiagCodes = string.Format("'{0}'", item);
                else
                    szDiagCodes = string.Format("{0},'{1}'", szDiagCodes, item);
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            if (lstNurCarePlanInfos == null)
                lstNurCarePlanInfos = new List<NurCarePlanDictInfo>();
            lstNurCarePlanInfos.Clear();

            string szField = string.Format("A.{0},B.{1},B.{2},B.{3},B.{4},B.{5},B.{6},B.{7}"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, ServerData.CommonDictTable.ITEM_NO
                , ServerData.CommonDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_NAME
                , ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictTable.WARD_CODE
                , ServerData.CommonDictTable.WARD_NAME, ServerData.CommonDictTable.INPUT_CODE);
            string szTable = string.Format("{0} A, {1} B"
                , ServerData.DataTable.NUR_CARE_PLAN_DICT, ServerData.DataTable.NUR_COMMON_DICT);
            string szCondition = string.Format("A.{0} in ({1}) AND A.{2}=B.{3}"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, szDiagCodes
                , ServerData.NurCarePlanDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_CODE);
            string szOrderBy = string.Format("B.{0}, B.{1}", ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictTable.ITEM_NO);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrderBy);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    NurCarePlanDictInfo nurCarePlan = new NurCarePlanDictInfo();
                    if (!dataReader.IsDBNull(0)) nurCarePlan.DiagCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nurCarePlan.Item.ItemNo = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) nurCarePlan.Item.ItemCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) nurCarePlan.Item.ItemName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nurCarePlan.Item.ItemType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) nurCarePlan.Item.WardCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) nurCarePlan.Item.WardName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nurCarePlan.Item.InputCode = dataReader.GetString(7);
                    lstNurCarePlanInfos.Add(nurCarePlan);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ɾ��ָ���Ļ�����ϼ����������л���ƻ�,�����ʩ���ֵ�����
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteNurCarePlanDictInfo(string szDiagCode)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //ɾ���û�����Ϲ����ĸ��ֵ���Ŀ
            string szField = string.Format("B.{0}, B.{1}"
                , ServerData.CommonDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_TYPE);
            string szTable = string.Format("{0} A, {1} B"
                , ServerData.DataTable.NUR_CARE_PLAN_DICT, ServerData.DataTable.NUR_COMMON_DICT);
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}=B.{3}"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, szDiagCode
                , ServerData.NurCarePlanDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_CODE);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader reader = null;
            try
            {
                reader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            while (reader != null && reader.Read())
            {
                string szItemCode = reader.GetString(0);
                string szItemType = reader.GetString(1);

                szTable = ServerData.DataTable.NUR_COMMON_DICT;
                szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                    , ServerData.CommonDictTable.ITEM_CODE, szItemCode
                    , ServerData.CommonDictTable.ITEM_TYPE, szItemType);
                szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
                try
                {
                    base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                }
                catch (Exception ex)
                {
                    base.DataAccess.AbortTransaction();
                    return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
                }
                finally { base.DataAccess.CloseConnnection(false); }
            }

            //��NUR_CARE_PLAN_DICT����ɾ�������
            szTable = ServerData.DataTable.NUR_CARE_PLAN_DICT;
            szCondition = string.Format("{0}='{1}'", ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, szDiagCode);
            szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }

            //��NUR_COMMON_DICT����ɾ���û������
            szTable = ServerData.DataTable.NUR_COMMON_DICT;
            szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.CommonDictTable.ITEM_CODE, szDiagCode
                , ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictType.NUR_DIAGNOSIS);
            szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            return base.DataAccess.CommitTransaction() ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// ɾ��ָ���Ļ�����ϼ�������ָ���Ļ���ƻ�,�����ʩ���ֵ�����
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <param name="szItemCode">����ƻ��ֵ����ݴ���</param>
        /// <param name="szItemType">����ƻ��ֵ���������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteNurCarePlanDictInfo(string szDiagCode, string szItemCode, string szItemType)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //�ӻ���ƻ��ֵ����ɾ������ƻ�
            string szTable = ServerData.DataTable.NUR_CARE_PLAN_DICT;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, szDiagCode
                , ServerData.NurCarePlanDictTable.ITEM_CODE, szItemCode
                , ServerData.NurCarePlanDictTable.ITEM_TYPE, szItemType);
            string szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }

            //��NUR_COMMON_DICT����ɾ���û������
            szTable = ServerData.DataTable.NUR_COMMON_DICT;
            szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.CommonDictTable.ITEM_CODE, szDiagCode
                , ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictType.NUR_DIAGNOSIS);
            szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }

            //��NUR_COMMON_DICT����ɾ��������϶�Ӧ���ֵ�����
            szTable = ServerData.DataTable.NUR_COMMON_DICT;
            szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.CommonDictTable.ITEM_CODE, szItemCode
                , ServerData.CommonDictTable.ITEM_TYPE, szItemType);
            szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            return base.DataAccess.CommitTransaction() ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// ����ָ����һ��������϶�Ӧ�Ļ���ƻ��ֵ�����
        /// </summary>
        /// <param name="ncpDictInfo">����ƻ��ֵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNurCarePlanDictInfo(NurCarePlanDictInfo ncpDictInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //���뵽����ƻ��ֵ��NUR_CARE_PLAN_DICT��
            string szField = string.Format("{0},{1},{2}"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, ServerData.NurCarePlanDictTable.ITEM_CODE
                , ServerData.NurCarePlanDictTable.ITEM_TYPE);
            string szValue = string.Format("'{0}','{1}','{2}'"
                , ncpDictInfo.DiagCode, ncpDictInfo.Item.ItemCode, ncpDictInfo.Item.ItemType);
            string szTable = ServerData.DataTable.NUR_CARE_PLAN_DICT;
            string szSQL = string.Format(ServerData.SQL.INSERT, szTable, szField, szValue);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }

            //���뵽�������ֵ��NUR_COMMON_DICT��
            szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.CommonDictTable.ITEM_NO, ServerData.CommonDictTable.ITEM_CODE
                , ServerData.CommonDictTable.ITEM_NAME, ServerData.CommonDictTable.ITEM_TYPE
                , ServerData.CommonDictTable.WARD_CODE, ServerData.CommonDictTable.WARD_NAME
                , ServerData.CommonDictTable.INPUT_CODE);
            szValue = string.Format("{0},'{1}','{2}','{3}','{4}','{5}','{6}'"
                , ncpDictInfo.Item.ItemNo, ncpDictInfo.Item.ItemCode
                , ncpDictInfo.Item.ItemName, ncpDictInfo.Item.ItemType
                , ncpDictInfo.Item.WardCode, ncpDictInfo.Item.WardName, ncpDictInfo.Item.InputCode);
            szTable = ServerData.DataTable.NUR_COMMON_DICT;
            szSQL = string.Format(ServerData.SQL.INSERT, szTable, szField, szValue);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            return base.DataAccess.CommitTransaction() ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// ����ָ���Ļ�����϶�Ӧ��ָ���Ļ���ƻ����ֵ�����
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <param name="szItemCode">����ƻ��ֵ����ݴ���</param>
        /// <param name="szItemType">����ƻ��ֵ���������</param>
        /// <param name="ncpDictInfo">����ƻ��µ��ֵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateNurCarePlanDictInfo(string szDiagCode, string szItemCode, string szItemType, NurCarePlanDictInfo ncpDictInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //���»���ƻ��ֵ��NUR_CARE_PLAN_DICT
            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}'"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, ncpDictInfo.DiagCode
                , ServerData.NurCarePlanDictTable.ITEM_CODE, ncpDictInfo.Item.ItemCode
                , ServerData.NurCarePlanDictTable.ITEM_TYPE, ncpDictInfo.Item.ItemType);
            string szTable = ServerData.DataTable.NUR_CARE_PLAN_DICT;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'"
                , ServerData.NurCarePlanDictTable.DIAGNOSIS_CODE, szDiagCode
                , ServerData.NurCarePlanDictTable.ITEM_CODE, szItemCode
                , ServerData.NurCarePlanDictTable.ITEM_TYPE, szItemType);
            string szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }

            //���»������ֵ��NUR_COMMON_DICT
            szField = string.Format("{0}={1},{2}='{3}',{4}='{5}',{6}='{7}',{8}='{9}'"
                , ServerData.CommonDictTable.ITEM_NO, ncpDictInfo.Item.ItemNo
                , ServerData.CommonDictTable.ITEM_CODE, ncpDictInfo.Item.ItemCode
                , ServerData.CommonDictTable.ITEM_NAME, ncpDictInfo.Item.ItemName
                , ServerData.CommonDictTable.ITEM_TYPE, ncpDictInfo.Item.ItemType
                , ServerData.CommonDictTable.INPUT_CODE, ncpDictInfo.Item.InputCode);
            szTable = ServerData.DataTable.NUR_COMMON_DICT;
            szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.CommonDictTable.ITEM_CODE, szItemCode
                , ServerData.CommonDictTable.ITEM_TYPE, szItemType);
            szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            return base.DataAccess.CommitTransaction() ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }
        #endregion
    }
}
