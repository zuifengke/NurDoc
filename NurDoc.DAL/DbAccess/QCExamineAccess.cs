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
    public class QCExamineAccess : DBAccessBase
    {
        #region"��дָ����˽ӿ�"
        /// <summary>
        /// ����szQcContentKey��szQcContentType��szPatientID��szVisitID��ȡ�ʿ������Ϣ
        /// </summary>
        /// <param name="szQcContentKey">���ݱ�ʶ</param>
        /// <param name="szQcContentType">��������</param>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����VID</param>
        /// <param name="qcExamineInfo">�������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfo(string szQcContentKey, string szQcContentType, string szPatientID, string szVisitID, ref QCExamineInfo qcExamineInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQcContentKey) || GlobalMethods.Misc.IsEmptyString(szQcContentType) || GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("QCExamineAccess.GetQcExamineInfo"
                    , new string[] { "szQcContentKey", "szQcContentType", "szPatientID", "szVisitID" }
                    , new object[] { szQcContentKey, szQcContentType, szPatientID, szVisitID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                , ServerData.QCExamineTable.QC_CONTENT_KEY, ServerData.QCExamineTable.QC_CONTENT_TYPE
                , ServerData.QCExamineTable.PATIENT_ID, ServerData.QCExamineTable.PATIENT_NAME
                , ServerData.QCExamineTable.VISIT_ID, ServerData.QCExamineTable.WARD_CODE
                , ServerData.QCExamineTable.WARD_NAME, ServerData.QCExamineTable.QC_EXAMINE_STATUS
                , ServerData.QCExamineTable.QC_EXAMINE_CONTENT, ServerData.QCExamineTable.QC_EXAMINE_ID
                , ServerData.QCExamineTable.QC_EXAMINE_NAME, ServerData.QCExamineTable.QC_EXAMINE_TIME);
            string szCondition = string.Format("{0}='{4}' and {1}='{5}' and {2}='{6}' and {3}='{7}' "
                , ServerData.QCExamineTable.QC_CONTENT_KEY, ServerData.QCExamineTable.QC_CONTENT_TYPE
                , ServerData.QCExamineTable.PATIENT_ID, ServerData.QCExamineTable.VISIT_ID,
                szQcContentKey.Trim(), szQcContentType.Trim(), szPatientID.Trim(), szVisitID.Trim());
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_QC_EXAMINE, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCExamineAccess.GetQcExamineInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (qcExamineInfo == null)
                    qcExamineInfo = new QCExamineInfo();
                do
                {
                    if (!dataReader.IsDBNull(0)) qcExamineInfo.QcContentKey = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) qcExamineInfo.QcContentType = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) qcExamineInfo.PatientID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) qcExamineInfo.PatientName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcExamineInfo.VisitID = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) qcExamineInfo.WardCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) qcExamineInfo.WardName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) qcExamineInfo.QcExamineStatus = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) qcExamineInfo.QcExamineContent = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) qcExamineInfo.QcExamineID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) qcExamineInfo.QcExamineName = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) qcExamineInfo.QcExamineTime = dataReader.GetDateTime(11);
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
        /// �����ʿ�����������Ͳ�ѯ����
        /// </summary>
        /// <param name="szQcContentType">�ʿ������������</param>
        /// <param name="lstQcExamineInfo">�ʿ���ϢList</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfos(string szQcContentType, ref List<QCExamineInfo> lstQcExamineInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQcContentType))
            {
                LogManager.Instance.WriteLog("QCExamineAccess.GetQcExamineInfo"
                    , new string[] { "szQcContentType" }
                    , new object[] { szQcContentType }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                , ServerData.QCExamineTable.QC_CONTENT_KEY, ServerData.QCExamineTable.QC_CONTENT_TYPE
                , ServerData.QCExamineTable.PATIENT_ID, ServerData.QCExamineTable.PATIENT_NAME
                , ServerData.QCExamineTable.VISIT_ID, ServerData.QCExamineTable.WARD_CODE
                , ServerData.QCExamineTable.WARD_NAME, ServerData.QCExamineTable.QC_EXAMINE_STATUS
                , ServerData.QCExamineTable.QC_EXAMINE_CONTENT, ServerData.QCExamineTable.QC_EXAMINE_ID
                , ServerData.QCExamineTable.QC_EXAMINE_NAME, ServerData.QCExamineTable.QC_EXAMINE_TIME);
            string szCondition = string.Format("{0}='{1}'  "
                , ServerData.QCExamineTable.QC_CONTENT_TYPE
                , szQcContentType.Trim());
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_QC_EXAMINE, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCExamineAccess.GetQcExamineInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstQcExamineInfo == null)
                    lstQcExamineInfo = new List<QCExamineInfo>();
                do
                {
                    QCExamineInfo qcExamineInfo = new QCExamineInfo();
                    if (!dataReader.IsDBNull(0)) qcExamineInfo.QcContentKey = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) qcExamineInfo.QcContentType = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) qcExamineInfo.PatientID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) qcExamineInfo.PatientName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcExamineInfo.VisitID = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) qcExamineInfo.WardCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) qcExamineInfo.WardName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) qcExamineInfo.QcExamineStatus = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) qcExamineInfo.QcExamineContent = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) qcExamineInfo.QcExamineID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) qcExamineInfo.QcExamineName = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) qcExamineInfo.QcExamineTime = dataReader.GetDateTime(11);
                    lstQcExamineInfo.Add(qcExamineInfo);
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
        /// ���������Ϣ���ͣ�����ID������ID��ȡ��������Ϣ
        /// </summary>
        /// <param name="szQcContentType">��Ϣ����</param>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="lstQcExamineInfo">�����ϢList</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfos(string szQcContentType, string szPatientID, string szVisitID, ref List<QCExamineInfo> lstQcExamineInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQcContentType) || GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("QCExamineAccess.GetQcExamineInfo"
                    , new string[] {"szQcContentType", "szPatientID", "szVisitID" }
                    , new object[] { szQcContentType, szPatientID, szVisitID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                , ServerData.QCExamineTable.QC_CONTENT_KEY, ServerData.QCExamineTable.QC_CONTENT_TYPE
                , ServerData.QCExamineTable.PATIENT_ID, ServerData.QCExamineTable.PATIENT_NAME
                , ServerData.QCExamineTable.VISIT_ID, ServerData.QCExamineTable.WARD_CODE
                , ServerData.QCExamineTable.WARD_NAME, ServerData.QCExamineTable.QC_EXAMINE_STATUS
                , ServerData.QCExamineTable.QC_EXAMINE_CONTENT, ServerData.QCExamineTable.QC_EXAMINE_ID
                , ServerData.QCExamineTable.QC_EXAMINE_NAME, ServerData.QCExamineTable.QC_EXAMINE_TIME);
            string szCondition = string.Format("{0}='{3}' and {1}='{4}' and {2}='{5}' "
                , ServerData.QCExamineTable.QC_CONTENT_TYPE
                , ServerData.QCExamineTable.PATIENT_ID, ServerData.QCExamineTable.VISIT_ID,
                 szQcContentType.Trim(), szPatientID.Trim(), szVisitID.Trim());
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_QC_EXAMINE, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCExamineAccess.GetQcExamineInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstQcExamineInfo == null)
                    lstQcExamineInfo = new List<QCExamineInfo>();
                do
                {
                    QCExamineInfo qcExamineInfo = new QCExamineInfo();
                    if (!dataReader.IsDBNull(0)) qcExamineInfo.QcContentKey = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) qcExamineInfo.QcContentType = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) qcExamineInfo.PatientID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) qcExamineInfo.PatientName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcExamineInfo.VisitID = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) qcExamineInfo.WardCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) qcExamineInfo.WardName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) qcExamineInfo.QcExamineStatus = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) qcExamineInfo.QcExamineContent = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) qcExamineInfo.QcExamineID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) qcExamineInfo.QcExamineName = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) qcExamineInfo.QcExamineTime = dataReader.GetDateTime(11);
                    lstQcExamineInfo.Add(qcExamineInfo);
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
        /// ����ָ�������Ϣ
        /// </summary>
        /// <param name="qcExamineInfo">�ʿ������Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddQcExamineInfo(QCExamineInfo qcExamineInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                , ServerData.QCExamineTable.QC_CONTENT_KEY, ServerData.QCExamineTable.QC_CONTENT_TYPE
                , ServerData.QCExamineTable.PATIENT_ID, ServerData.QCExamineTable.PATIENT_NAME
                , ServerData.QCExamineTable.VISIT_ID, ServerData.QCExamineTable.WARD_CODE
                , ServerData.QCExamineTable.WARD_NAME, ServerData.QCExamineTable.QC_EXAMINE_STATUS
                , ServerData.QCExamineTable.QC_EXAMINE_CONTENT, ServerData.QCExamineTable.QC_EXAMINE_ID
                , ServerData.QCExamineTable.QC_EXAMINE_NAME, ServerData.QCExamineTable.QC_EXAMINE_TIME);
            string szValue = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11}"
                , qcExamineInfo.QcContentKey, qcExamineInfo.QcContentType
                , qcExamineInfo.PatientID, qcExamineInfo.PatientName
                , qcExamineInfo.VisitID, qcExamineInfo.WardCode
                , qcExamineInfo.WardName, qcExamineInfo.QcExamineStatus
                , qcExamineInfo.QcExamineContent, qcExamineInfo.QcExamineID
                , qcExamineInfo.QcExamineName, base.DataAccess.GetSqlTimeFormat(qcExamineInfo.QcExamineTime));
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_QC_EXAMINE, szField, szValue);

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
                LogManager.Instance.WriteLog("QCExamineAccess.AddQcExamineInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸��ʿ������Ϣ
        /// </summary>
        /// <param name="qcExamineInfo">�ʿ������Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short ModifyQcExamineInfo(QCExamineInfo qcExamineInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}='{7}',{8}={9}"
                , ServerData.QCExamineTable.QC_EXAMINE_STATUS, qcExamineInfo.QcExamineStatus
                , ServerData.QCExamineTable.QC_EXAMINE_CONTENT, qcExamineInfo.QcExamineContent
                , ServerData.QCExamineTable.QC_EXAMINE_ID, qcExamineInfo.QcExamineID
                , ServerData.QCExamineTable.QC_EXAMINE_NAME, qcExamineInfo.QcExamineName
                , ServerData.QCExamineTable.QC_EXAMINE_TIME, base.DataAccess.GetSqlTimeFormat(qcExamineInfo.QcExamineTime));
            string szCondition = string.Format("{0}='{4}' and {1}='{5}' and {2}='{6}' and {3}='{7}' "
                , ServerData.QCExamineTable.QC_CONTENT_KEY, ServerData.QCExamineTable.QC_CONTENT_TYPE
                , ServerData.QCExamineTable.PATIENT_ID, ServerData.QCExamineTable.VISIT_ID,
                qcExamineInfo.QcContentKey, qcExamineInfo.QcContentType, qcExamineInfo.PatientID, qcExamineInfo.VisitID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_QC_EXAMINE, szField, szCondition);

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
                LogManager.Instance.WriteLog("QCExamineAccess.ModifyQcExamineInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"���������Ϣ�ӿ�"
        /// <summary>
        /// ����ָ����һ���ʿ������Ϣ
        /// </summary>
        /// <param name="qcExamineInfo">�����Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveQcExamine(QCExamineInfo qcExamineInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //���������Ϣ
            short shRet = this.AddQcExamineInfo(qcExamineInfo);
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

        #region"���������Ϣ�ӿ�"
        /// <summary>
        /// ����ָ����һ���ʿ������Ϣ
        /// </summary>
        /// <param name="qcExamineInfo">�����Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateQcExamine(QCExamineInfo qcExamineInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //���������Ϣ
            short shRet = this.ModifyQcExamineInfo(qcExamineInfo);
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
    }
}
