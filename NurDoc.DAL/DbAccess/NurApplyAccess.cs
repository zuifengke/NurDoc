// ***********************************************************
// ���ݿ���ʲ��뻤�����뵥���ݼ������йص����ݵķ�����.
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
        #region"��д�������������ӿ�"
        /// <summary>
        /// �������뵥ID����ȡ���뵥������Ϣ
        /// </summary>
        /// <param name="szApplyID">���뵥���</param>
        /// <param name="nurApplyInfo">���뵥��Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfo(string szApplyID, ref NurApplyInfo nurApplyInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szApplyID))
            {
                LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo"
                    , new string[] { "szApplyID" }, new object[] { szApplyID }, "��������Ϊ��");
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
                    LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// �����ĵ���ID��ȡ���뵥������Ϣ
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID</param>
        /// <param name="nurApplyInfo">���뵥������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfoByDocSetID(string szDocSetID, ref NurApplyInfo nurApplyInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo"
                    , new string[] { "szApplyID" }, new object[] { szDocSetID }, "��������Ϊ��");
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
                    LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡ�������뵥�����б�
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="szSubID">������ID</param>
        /// <param name="dtBeginTime">��¼ʱ��</param>
        /// <param name="dtEndTime">��¼ʱ��</param>
        ///  <param name="szApplyType">���뵥����</param>
        /// <param name="lstNurApplyInfos">������Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfoList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, string szApplyType, ref List<NurApplyInfo> lstNurApplyInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("NurApplyAccess.GetNurApplyInfoList"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "���ݲ���Ϊ��!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��������������Ϣ
        /// </summary>
        /// <param name="nurApplyInfo">��������������Ϣ��</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.AddNurApplyIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ɾ����������������Ϣ
        /// </summary>
        /// <param name="nurApplyInfo">��������������Ϣ��</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.DelNurApplyIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ɾ����������״̬��Ϣ
        /// </summary>
        /// <param name="ApplicantID">��������������Ϣ��</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.DelNurApplyStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸Ļ���������Ϣ
        /// </summary>
        /// <param name="nurApplyInfo">��������������Ϣ��</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.ModifyNurApplyIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"��д���뵥״̬�ӿ�"
        /// <summary>
        /// ��ȡָ�����롢ָ������ʱ���Լ�ָ��״̬��Ϣ
        /// </summary>
        /// <param name="szApplyID">���뵥���</param>
        /// <param name="szStatus">���뵥״̬</param>
        /// <param name="dtOperateTime">���뵥״̬����ʱ��</param>
        /// <param name="nurApplyStatusInfo">���뵥״̬��Ϣ��</param>
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
                        , new object[] { szSQL }, "û�в�ѯ����¼!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡָ���������롢ָ��״̬��Ϣ
        /// </summary>
        /// <param name="szApplyID">���뵥���</param>
        /// <param name="szStatus">���뵥״̬</param>
        /// <param name="nurApplyStatusInfo">���뵥״̬��Ϣ��</param>
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
                        , new object[] { szSQL }, "û�в�ѯ����¼!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// д���뵥״̬��¼
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddNurApplyStatusInfo(NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (nurApplyStatusInfo == null)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.AddNurApplyStatusInfo", "��������Ϊnull!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurApplyAccess.AddNurApplyStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"���滤������ӿ�"
        /// <summary>
        /// ����ָ����һ������������Ϣ
        /// </summary>
        /// <param name="nurApplyInfo">���뵥��Ϣ</param>
        /// <param name="nurApplyStatusInfo">���뵥״̬��Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNurApply(NurApplyInfo nurApplyInfo, NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //�������뵥������Ϣ��¼
            short shRet = this.AddNurApplyIndexInfo(nurApplyInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //������뵥״̬��Ϣ��¼
            shRet = this.AddNurApplyStatusInfo(nurApplyStatusInfo);
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

        #region"�����������뵥�ӿ�"
        /// <summary>
        /// ����ָ�����ѱ�������뵥
        /// </summary>
        /// <param name="nurApplyInfo">������Ϣ</param>
        /// <param name="nurApplyStatusInfo">���뵥״̬��Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateNurApply(NurApplyInfo nurApplyInfo, NurApplyStatusInfo nurApplyStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //��������������Ϣ��¼
            short shRet = this.ModifyNurApplyIndexInfo(nurApplyInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //������뵥״̬��Ϣ��¼
            shRet = this.AddNurApplyStatusInfo(nurApplyStatusInfo);
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

        /// <summary>
        /// ɾ���������뵥
        /// </summary>
        /// <param name="nurApplyInfo">������Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DelNurApply(NurApplyInfo nurApplyInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
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

            //�ύ���ݿ����
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���ݻ����ĵ���ID��ȡ������Ϣ
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID</param>
        /// <param name="NurApplyStatusInfo">��������״̬��Ϣ��</param>
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
