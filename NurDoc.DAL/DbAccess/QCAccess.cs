// ***********************************************************
// ���ݿ���ʲ��������밲ȫ�����¼�ĵ����ݼ������йص����ݵķ�����.
// Creator:YangMingkun  Date:2012-3-20
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
    public class QCAccess : DBAccessBase
    {
        #region"��д�ĵ������ӿ�"
        /// <summary>
        /// ��ȡָ�������������ĵ���ϸ��Ϣ�б�.
        /// ע�⣺�ýӿڲ��Է��������밲ȫ�����¼�б��������,��Ҫ�ⲿ��������Sort��������
        /// </summary>
        /// <param name="szCreatorID">������ID</param>
        /// <param name="szQCTypeID">�ĵ����ʹ���,Ϊ��ʱ���ص�ǰ�������͵��ĵ�</param>
        /// <param name="lstQCInfos">�ĵ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQCDocInfos(string szCreatorID, string szQCTypeID, ref QCDocList lstQCInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);

            string szCondition = string.Format("A.{0}='{1}'", ServerData.QCDocTable.CREATOR_ID, szCreatorID);
            //���˵������ϵ��ĵ�
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.QCDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);

            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                , ServerData.QCDocTable.DOC_TYPE_ID, base.DataAccess.GetSqlParamName("DOC_TYPE_ID"));

            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.QC_DOC, ServerData.DataTable.QC_DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            DbParameter[] pmi = new DbParameter[1];
            pmi[0] = new DbParameter("DOC_TYPE_ID", szQCTypeID);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text, ref pmi);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstQCInfos == null)
                    lstQCInfos = new QCDocList();
                do
                {
                    QCDocInfo docInfo = new QCDocInfo();
                    docInfo.DocID = dataReader.GetString(0);
                    docInfo.DocTypeID = dataReader.GetString(1);
                    docInfo.DocTitle = dataReader.GetString(2);
                    docInfo.DocTime = dataReader.GetDateTime(3);
                    docInfo.DocSetID = dataReader.GetString(4);
                    docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                    docInfo.CreatorID = dataReader.GetString(6);
                    docInfo.CreatorName = dataReader.GetString(7);
                    docInfo.ModifierID = dataReader.GetString(8);
                    docInfo.ModifierName = dataReader.GetString(9);
                    docInfo.ModifyTime = dataReader.GetDateTime(10);
                    docInfo.WardCode = dataReader.GetString(11);
                    docInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        docInfo.SignCode = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        docInfo.ConfidCode = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16))
                        docInfo.RecordTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17))
                        docInfo.RecordID = dataReader.GetString(17);

                    lstQCInfos.Add(docInfo);
                } while (dataReader.Read());
                lstQCInfos.SortByTime(true);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡָ�������밲ȫ�����¼�����������ĵ���ϸ��Ϣ�б�.      HJX
        /// ע�⣺�ýӿڲ��Է��ز����б��������,��Ҫ�ⲿ��������Sort��������
        /// </summary>
        /// <param name="szCreatorID">������ID</param>
        /// <param name="szQCTypeID">�ĵ����ʹ���,Ϊ��ʱ���ص��ξ������������͵��ĵ�</param>
        /// <param name="dtBeginTime">��ѯ��ʼ����ʱ��</param>
        /// <param name="dtEndTime">��ѯ��ֹ����ʱ��</param>
        /// <param name="lstQCInfos">�ĵ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQCInfos(string szCreatorID, string szQCTypeID, DateTime dtBeginTime, DateTime dtEndTime, ref QCDocList lstQCInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);

            string szCondition = string.Format("A.{0}={1} ", ServerData.QCDocTable.CREATOR_ID, base.DataAccess.GetSqlParamName("CREATOR_ID"));
            //���˵������ϵ��ĵ�
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.QCDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                 , ServerData.QCDocTable.DOC_TYPE_ID, szQCTypeID);

            //�����������ʼ��ֹʱ��,�����ȡ���ʱ����ڴ�����

            szCondition = string.Format("{0} AND A.{1}>={2} AND A.{3}<={4}", szCondition
                , ServerData.QCDocTable.RECORD_TIME, base.DataAccess.GetSqlParamName("BEGIN_TIME")
                , ServerData.QCDocTable.RECORD_TIME, base.DataAccess.GetSqlParamName("END_TIME"));

            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.QC_DOC, ServerData.DataTable.QC_DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            DbParameter[] pmi = new DbParameter[3];
            pmi[0] = new DbParameter("CREATOR_ID", szCreatorID);
            pmi[1] = new DbParameter("BEGIN_TIME", dtBeginTime);
            pmi[2] = new DbParameter("END_TIME", dtEndTime);
            IDataReader dataReader = null;
            
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text, ref pmi);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstQCInfos == null)
                    lstQCInfos = new QCDocList();
                do
                {
                    QCDocInfo QCdocInfo = new QCDocInfo();
                    QCdocInfo.DocID = dataReader.GetString(0);
                    QCdocInfo.DocTypeID = dataReader.GetString(1);
                    QCdocInfo.DocTitle = dataReader.GetString(2);
                    QCdocInfo.DocTime = dataReader.GetDateTime(3);
                    QCdocInfo.DocSetID = dataReader.GetString(4);
                    QCdocInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                    QCdocInfo.CreatorID = dataReader.GetString(6);
                    QCdocInfo.CreatorName = dataReader.GetString(7);
                    QCdocInfo.ModifierID = dataReader.GetString(8);
                    QCdocInfo.ModifierName = dataReader.GetString(9);
                    QCdocInfo.ModifyTime = dataReader.GetDateTime(10);
                    QCdocInfo.WardCode = dataReader.GetString(11);
                    QCdocInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        QCdocInfo.SignCode = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        QCdocInfo.ConfidCode = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15))
                        QCdocInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16))
                        QCdocInfo.RecordTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17))
                        QCdocInfo.RecordID = dataReader.GetString(17);

                    lstQCInfos.Add(QCdocInfo);
                } while (dataReader.Read());
                lstQCInfos.SortByTime(true);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// �����ĵ�ID����ȡ�ĵ�������Ϣ
        /// </summary>
        /// <param name="szQCDocID">�ĵ����</param>
        /// <param name="docInfo">�ĵ���Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQCDocInfo(string szQCDocID, ref QCDocInfo qcInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQCDocID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetQCDocInfo", new string[] { "szQCDocID" }, new object[] { szQCDocID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szQCDocID = szQCDocID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE , ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);
               
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_ID, szQCDocID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.QC_DOC, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCAccess.GetDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (qcInfo == null)
                    qcInfo = new QCDocInfo();
                do
                {
                    qcInfo.DocID = dataReader.GetString(0);
                    qcInfo.DocTypeID = dataReader.GetString(1);
                    qcInfo.DocTitle = dataReader.GetString(2);
                    qcInfo.DocTime = dataReader.GetDateTime(3);
                    qcInfo.DocSetID = dataReader.GetString(4);
                    qcInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                    qcInfo.CreatorID = dataReader.GetString(6);
                    qcInfo.CreatorName = dataReader.GetString(7);
                    qcInfo.ModifierID = dataReader.GetString(8);
                    qcInfo.ModifierName = dataReader.GetString(9);
                    qcInfo.ModifyTime = dataReader.GetDateTime(10);
                    qcInfo.WardCode = dataReader.GetString(11);
                    qcInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        qcInfo.SignCode = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        qcInfo.ConfidCode = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15))
                        qcInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16))
                        qcInfo.RecordTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17))
                        qcInfo.RecordID = dataReader.GetString(17);
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
        /// ��ȡָ���ĵ��������µ��ĵ��汾��Ϣ
        /// </summary>
        /// <param name="szQCDocSetID">�ĵ���ID</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetLatestQCDocInfo(string szQCDocSetID, ref QCDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQCDocSetID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetLatestQCDocInfo", new string[] { "szQCDocSetID" }
                    , new object[] { szQCDocSetID }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szQCDocSetID = szQCDocSetID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);
                
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_SETID, szQCDocSetID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, ServerData.DataTable.QC_DOC, szCondition
                , ServerData.QCDocTable.DOC_VERSION);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (docInfo == null)
                    docInfo = new QCDocInfo();

                docInfo.DocID = dataReader.GetString(0);
                docInfo.DocTypeID = dataReader.GetString(1);
                docInfo.DocTitle = dataReader.GetString(2);
                docInfo.DocTime = dataReader.GetDateTime(3);
                docInfo.DocSetID = dataReader.GetString(4);
                docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                docInfo.CreatorID = dataReader.GetString(6);
                docInfo.CreatorName = dataReader.GetString(7);
                docInfo.ModifierID = dataReader.GetString(8);
                docInfo.ModifierName = dataReader.GetString(9);
                docInfo.ModifyTime = dataReader.GetDateTime(10);
                docInfo.WardCode = dataReader.GetString(11);
                docInfo.WardName = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13))
                    docInfo.SignCode = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14))
                    docInfo.ConfidCode = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                if (!dataReader.IsDBNull(16))
                    docInfo.RecordTime = dataReader.GetDateTime(16);
                if (!dataReader.IsDBNull(17))
                    docInfo.RecordID = dataReader.GetString(17);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// �����ĵ�������Ϣ
        /// </summary>
        /// <param name="docInfo">�ĵ�������Ϣ��</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddDocIndexInfo(QCDocInfo docInfo, byte[] byteDocData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);
                
            string szValue = string.Format("'{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}',{16},'{17}'"
                , docInfo.DocID, docInfo.DocTypeID, docInfo.DocTitle, base.DataAccess.GetSqlTimeFormat(docInfo.DocTime)
                , docInfo.DocSetID, docInfo.DocVersion, docInfo.CreatorID, docInfo.CreatorName, docInfo.ModifierID
                , docInfo.ModifierName, base.DataAccess.GetSqlTimeFormat(docInfo.ModifyTime), docInfo.WardCode
                , docInfo.WardName, docInfo.SignCode, docInfo.ConfidCode, docInfo.OrderValue
                , base.DataAccess.GetSqlTimeFormat(docInfo.RecordTime), docInfo.RecordID);

            DbParameter[] pmi = null;
            if (byteDocData != null)
            {
                szField = string.Format("{0},{1}", szField, ServerData.QCDocTable.DOC_DATA);
                szValue = string.Format("{0},{1}", szValue, base.DataAccess.GetSqlParamName("DocData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("DocData", byteDocData);
            }
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.QC_DOC, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("QCAccess.AddDocIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸��ĵ�������Ϣ
        /// </summary>
        /// <param name="docInfo">�ĵ�������Ϣ��</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short ModifyDocIndexInfo(QCDocInfo docInfo, byte[] byteDocData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3},{4}='{5}',{6}='{7}',{8}={9},{10}='{11}',{12}='{13}',{14}={15}"
                , ServerData.QCDocTable.DOC_TITLE, docInfo.DocTitle
                , ServerData.QCDocTable.DOC_VERSION, docInfo.DocVersion
                , ServerData.QCDocTable.MODIFIER_ID, docInfo.ModifierID
                , ServerData.QCDocTable.MODIFIER_NAME, docInfo.ModifierName
                , ServerData.QCDocTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(docInfo.ModifyTime)
                , ServerData.QCDocTable.SIGN_CODE, docInfo.SignCode
                , ServerData.QCDocTable.CONFID_CODE, docInfo.ConfidCode
                , ServerData.QCDocTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(docInfo.RecordTime));
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_ID, docInfo.DocID);

            DbParameter[] pmi = null;
            if (byteDocData != null)
            {
                szField = string.Format("{0},{1}={2}", szField, ServerData.QCDocTable.DOC_DATA, base.DataAccess.GetSqlParamName("DocData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("DocData", byteDocData);
            }
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.QC_DOC, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("QCAccess.ModifyDocIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �õ���ǰ������ָ���ĵ����͵����°汾��δ���ϵ��ĵ�
        /// </summary>
        /// <param name="szCreatorID">�����˱��</param>
        /// <param name="szDocTypeID">�ĵ����ͱ��</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetActiveDocInfo(string szCreatorID, string szDocTypeID, ref QCDocInfo docInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);

            //��������ȷ��һ�ξ���Ĳ�ѯ����
            string szCondition = string.Format("A.{0}='{1}'", ServerData.QCDocTable.CREATOR_ID, szCreatorID);

            szCondition = string.Format("{0} AND A.{1}='{2}' AND A.{3}=B.{4} AND B.{5}!='{6}'"
                , szCondition, ServerData.QCDocTable.DOC_TYPE_ID, szDocTypeID
                , ServerData.QCDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.QC_DOC, ServerData.DataTable.QC_DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (docInfo == null)
                    docInfo = new QCDocInfo();
                docInfo.DocID = dataReader.GetString(0);
                docInfo.DocTypeID = dataReader.GetString(1);
                docInfo.DocTitle = dataReader.GetString(2);
                docInfo.DocTime = dataReader.GetDateTime(3);
                docInfo.DocSetID = dataReader.GetString(4);
                docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                docInfo.CreatorID = dataReader.GetString(6);
                docInfo.CreatorName = dataReader.GetString(7);
                docInfo.ModifierID = dataReader.GetString(8);
                docInfo.ModifierName = dataReader.GetString(9);
                docInfo.ModifyTime = dataReader.GetDateTime(10);
                docInfo.WardCode = dataReader.GetString(11);
                docInfo.WardName = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13))
                    docInfo.SignCode = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14))
                    docInfo.ConfidCode = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                if (!dataReader.IsDBNull(16))
                    docInfo.RecordTime = dataReader.GetDateTime(16);
                if (!dataReader.IsDBNull(17))
                    docInfo.RecordID = dataReader.GetString(17);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡָ���ĵ��������µ��ĵ��汾��Ϣ
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetLatestDocInfo(string szDocSetID, ref QCDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetLatestDocInfo", new string[] { "szDocSetID" }
                    , new object[] { szDocSetID }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocSetID = szDocSetID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_SETID, szDocSetID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, ServerData.DataTable.QC_DOC, szCondition
                , ServerData.QCDocTable.DOC_VERSION);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (docInfo == null)
                    docInfo = new QCDocInfo();

                docInfo.DocID = dataReader.GetString(0);
                docInfo.DocTypeID = dataReader.GetString(1);
                docInfo.DocTitle = dataReader.GetString(2);
                docInfo.DocTime = dataReader.GetDateTime(3);
                docInfo.DocSetID = dataReader.GetString(4);
                docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                docInfo.CreatorID = dataReader.GetString(6);
                docInfo.CreatorName = dataReader.GetString(7);
                docInfo.ModifierID = dataReader.GetString(8);
                docInfo.ModifierName = dataReader.GetString(9);
                docInfo.ModifyTime = dataReader.GetDateTime(10);
                docInfo.WardCode = dataReader.GetString(11);
                docInfo.WardName = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13))
                    docInfo.SignCode = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14))
                    docInfo.ConfidCode = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                if (!dataReader.IsDBNull(16))
                    docInfo.RecordTime = dataReader.GetDateTime(16);
                if (!dataReader.IsDBNull(17))
                    docInfo.RecordID = dataReader.GetString(17);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        ///  �����ĵ���ID��ѯ��ǰ�ĵ�����Ӧ�������޶��������밲ȫ�����¼
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID</param>
        /// <param name="lstDocInfo">�����ĵ��б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfoBySetID(string szDocSetID, ref List<QCDocInfo> lstDocInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetDocInfoBySetID", new string[] { "szDocSetID" }
                    , new object[] { szDocSetID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocSetID = szDocSetID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);
                
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_SETID, szDocSetID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, ServerData.DataTable.QC_DOC, szCondition
                , ServerData.QCDocTable.MODIFY_TIME);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCAccess.GetDocInfoBySetID", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstDocInfo == null)
                    lstDocInfo = new List<QCDocInfo>();
                do
                {
                    QCDocInfo docInfo = new QCDocInfo();
                    docInfo.DocID = dataReader.GetString(0);
                    docInfo.DocTypeID = dataReader.GetString(1);
                    docInfo.DocTitle = dataReader.GetString(2);
                    docInfo.DocTime = dataReader.GetDateTime(3);
                    docInfo.DocSetID = dataReader.GetString(4);
                    docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                    docInfo.CreatorID = dataReader.GetString(6);
                    docInfo.CreatorName = dataReader.GetString(7);
                    docInfo.ModifierID = dataReader.GetString(8);
                    docInfo.ModifierName = dataReader.GetString(9);
                    docInfo.ModifyTime = dataReader.GetDateTime(10);
                    docInfo.WardCode = dataReader.GetString(11);
                    docInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        docInfo.SignCode = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        docInfo.ConfidCode = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16))
                        docInfo.RecordTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17))
                        docInfo.RecordID = dataReader.GetString(17);
                    lstDocInfo.Add(docInfo);
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
        /// �����ĵ���ţ��޸��ĵ�����
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="szDocTitle">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyDocTitle(string szDocID, string szDocTitle)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                LogManager.Instance.WriteLog("QCAccess.ModifyDocTitle", new string[] { "szDocID" }, new object[] { szDocID }, "�ĵ�ID����Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (GlobalMethods.Misc.IsEmptyString(szDocTitle))
            {
                LogManager.Instance.WriteLog("QCAccess.ModifyDocTitle", new string[] { "szDocTitle" }, new object[] { szDocTitle }, "�ĵ����ⲻ��Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_TITLE, szDocTitle);
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.QC_DOC, szField, szCondition);

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
                LogManager.Instance.WriteLog("QCAccess.ModifyDocTitle", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        //

        /// <summary>
        /// ��ȡָ���ĵ��������µ��ĵ��汾��Ϣ
        /// </summary>
        /// <param name="szQCSetID">�ĵ���ID</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetLatestQCInfo(string szQCSetID, ref QCDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQCSetID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetLatestQCInfo", new string[] { "szQCSetID" }
                    , new object[] { szQCSetID }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szQCSetID = szQCSetID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_SETID, szQCSetID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, ServerData.DataTable.QC_DOC, szCondition
                , ServerData.QCDocTable.DOC_VERSION);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (docInfo == null)
                    docInfo = new QCDocInfo();

                docInfo.DocID = dataReader.GetString(0);
                docInfo.DocTypeID = dataReader.GetString(1);
                docInfo.DocTitle = dataReader.GetString(2);
                docInfo.DocTime = dataReader.GetDateTime(3);
                docInfo.DocSetID = dataReader.GetString(4);
                docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                docInfo.CreatorID = dataReader.GetString(6);
                docInfo.CreatorName = dataReader.GetString(7);
                docInfo.ModifierID = dataReader.GetString(8);
                docInfo.ModifierName = dataReader.GetString(9);
                docInfo.ModifyTime = dataReader.GetDateTime(10);
                docInfo.WardCode = dataReader.GetString(11);
                docInfo.WardName = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13))
                    docInfo.SignCode = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14))
                    docInfo.ConfidCode = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                if (!dataReader.IsDBNull(16))
                    docInfo.RecordTime = dataReader.GetDateTime(16);
                if (!dataReader.IsDBNull(17))
                    docInfo.RecordID = dataReader.GetString(17);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        
        /// <summary>
        /// �����ĵ�ID����ȡ�ĵ�������Ϣ
        /// </summary>
        /// <param name="szQCID">�ĵ����</param>
        /// <param name="docInfo">�ĵ���Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQCInfo(string szQCID, ref QCDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQCID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetQCInfo", new string[] { "szQCID" }
                    , new object[] { szQCID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szQCID = szQCID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);

            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_ID, szQCID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.QC_DOC, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCAccess.GetQCInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (docInfo == null)
                    docInfo = new QCDocInfo();
                do
                {
                    docInfo.DocID = dataReader.GetString(0);
                    docInfo.DocTypeID = dataReader.GetString(1);
                    docInfo.DocTitle = dataReader.GetString(2);
                    docInfo.DocTime = dataReader.GetDateTime(3);
                    docInfo.DocSetID = dataReader.GetString(4);
                    docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                    docInfo.CreatorID = dataReader.GetString(6);
                    docInfo.CreatorName = dataReader.GetString(7);
                    docInfo.ModifierID = dataReader.GetString(8);
                    docInfo.ModifierName = dataReader.GetString(9);
                    docInfo.ModifyTime = dataReader.GetDateTime(10);
                    docInfo.WardCode = dataReader.GetString(11);
                    docInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        docInfo.SignCode = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        docInfo.ConfidCode = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16))
                        docInfo.RecordTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17))
                        docInfo.RecordID = dataReader.GetString(17);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion

        #region"��д�ĵ�״̬�ӿ�"
        /// <summary>
        /// ��ȡָ���ĵ�״̬��Ϣ
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="docStatusInfo">�ĵ�״̬��Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocStatusInfo(string szDocID, ref DocStatusInfo docStatusInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5}"
                , ServerData.DocStatusTable.DOC_ID, ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatusTable.OPERATOR_ID
                , ServerData.DocStatusTable.OPERATOR_NAME, ServerData.DocStatusTable.OPERATE_TIME, ServerData.DocStatusTable.STATUS_DESC);
            string szCondition = string.Format("{0}='{1}'", ServerData.DocStatusTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.QC_DOC_STATUS, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCAccess.GetDocStatusInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "û�в�ѯ����¼!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (docStatusInfo == null)
                    docStatusInfo = new DocStatusInfo();
                docStatusInfo.DocID = dataReader.GetString(0);
                docStatusInfo.DocStatus = dataReader.GetString(1);
                docStatusInfo.OperatorID = dataReader.GetString(2);
                docStatusInfo.OperatorName = dataReader.GetString(3);
                docStatusInfo.OperateTime = dataReader.GetDateTime(4);
                if (!dataReader.IsDBNull(5))
                    docStatusInfo.StatusDesc = dataReader.GetString(5);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ����ָ���ĵ�״̬��Ϣ
        /// </summary>
        /// <param name="newStatusInfo">�ĵ�״̬��Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SetDocStatusInfo(ref DocStatusInfo newStatusInfo)
        {
            if (newStatusInfo == null)
            {
                LogManager.Instance.WriteLog("QCAccess.SetDocStatusInfo", "��������Ϊnull!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            DocStatusInfo currDocStatus = null;
            short shRet = this.GetDocStatusInfo(newStatusInfo.DocID, ref currDocStatus);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                newStatusInfo.StatusMessage = "��ѯ�������ĵ�״̬ʧ��";
                return shRet;
            }
            if (currDocStatus.DocStatus == ServerData.DocStatus.CANCELED)
            {
                newStatusInfo.StatusMessage = string.Format(
                    ServerData.DocStatus.CANCELED_STATUS_DESC,
                    currDocStatus.OperatorName,
                    currDocStatus.OperateTime.ToString("yyyy��M��d�� HH:mm"));
                LogManager.Instance.WriteLog("QCAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
                return ServerData.ExecuteResult.RES_IS_EXIST;
            }
            else if (currDocStatus.DocStatus == ServerData.DocStatus.ARCHIVED)
            {
                newStatusInfo.StatusMessage = string.Format(
                    ServerData.DocStatus.ARCHIVED_STATUS_DESC,
                    currDocStatus.OperatorName,
                    currDocStatus.OperateTime.ToString("yyyy��M��d�� HH:mm"));
                LogManager.Instance.WriteLog("QCAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
                return ServerData.ExecuteResult.OTHER_ERROR;
            }
            else if (currDocStatus.DocStatus == ServerData.DocStatus.LOCKED)
            {
                if (currDocStatus.OperatorID == newStatusInfo.OperatorID)
                {
                    newStatusInfo.StatusMessage = string.Format(
                        ServerData.DocStatus.LOCKED_STATUS_DESC1,
                        currDocStatus.OperateTime.ToString("yyyy��M��d�� HH:mm"));
                }
                else
                {
                    newStatusInfo.StatusMessage = string.Format(
                        ServerData.DocStatus.LOCKED_STATUS_DESC2,
                        currDocStatus.OperatorName,
                        currDocStatus.OperateTime.ToString("yyyy��M��d�� HH:mm"));
                    LogManager.Instance.WriteLog("QCAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
                    return ServerData.ExecuteResult.OTHER_ERROR;
                }
            }

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //�޸��ĵ�״̬
            shRet = this.ModifyDocStatusInfo(ref newStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //ɾ���ĵ�ͬʱɾ��ժҪ����
            if (newStatusInfo.DocStatus == ServerData.DocStatus.CANCELED)
            {
                shRet = this.DeleteSummaryData(newStatusInfo.DocID.Substring(0, newStatusInfo.DocID.LastIndexOf('_')));
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }
            }
            return base.DataAccess.CommitTransaction() ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// д�ĵ�״̬��¼
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddDocStatusInfo(DocStatusInfo docStatusInfo)
        {
            if (docStatusInfo == null)
            {
                LogManager.Instance.WriteLog("QCAccess.AddDocStatusInfo", "��������Ϊnull!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5}"
                , ServerData.DocStatusTable.DOC_ID, ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatusTable.OPERATOR_ID
                , ServerData.DocStatusTable.OPERATOR_NAME, ServerData.DocStatusTable.OPERATE_TIME, ServerData.DocStatusTable.STATUS_DESC);
            string szValue = string.Format("'{0}','{1}','{2}','{3}',{4},'{5}'"
                , docStatusInfo.DocID, docStatusInfo.DocStatus, docStatusInfo.OperatorID
                , docStatusInfo.OperatorName, base.DataAccess.GetSystemTimeSql(), docStatusInfo.StatusDesc);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.QC_DOC_STATUS, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!" + ex.Message);
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("QCAccess.AddDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸��ĵ�״̬��¼
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        private short ModifyDocStatusInfo(ref DocStatusInfo docStatusInfo)
        {
            if (docStatusInfo == null)
            {
                LogManager.Instance.WriteLog("QCAccess.ModifyDocStatusInfo", "��������Ϊnull!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}={7}"
                , ServerData.DocStatusTable.DOC_STATUS, docStatusInfo.DocStatus
                , ServerData.DocStatusTable.OPERATOR_ID, docStatusInfo.OperatorID
                , ServerData.DocStatusTable.OPERATOR_NAME, docStatusInfo.OperatorName
                    , ServerData.DocStatusTable.OPERATE_TIME, base.DataAccess.GetSqlTimeFormat(docStatusInfo.OperateTime));
            string szCondition = string.Format("{0}='{1}'", ServerData.DocStatusTable.DOC_ID, docStatusInfo.DocID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.QC_DOC_STATUS, szField, szCondition);

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
                LogManager.Instance.WriteLog("QCAccess.ModifyDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸��ĵ�״̬��¼
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyOldDocStatusInfo(ref DocStatusInfo docStatusInfo)
        {
            if (docStatusInfo == null)
            {
                LogManager.Instance.WriteLog("QCAccess.ModifyDocStatusInfo", "��������Ϊnull!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}'"
                , ServerData.DocStatusTable.DOC_STATUS, docStatusInfo.DocStatus);
            string szCondition = string.Format("{0}='{1}'", ServerData.DocStatusTable.DOC_ID, docStatusInfo.DocID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.QC_DOC_STATUS, szField, szCondition);

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
                LogManager.Instance.WriteLog("QCAccess.ModifyDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"��ȡ�ĵ����ݽӿ�"
        /// <summary>
        /// �����ĵ�ID��ȡ�ĵ�����
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="byteDocData">�ĵ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocByID(string szDocID, ref byte[] byteDocData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetDocByID"
                    , new string[] { "szDocID" }, new object[] { szDocID }, "�ĵ�ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("QCAccess.GetDocByID"
                    , new string[] { "szDocID" }, new object[] { szDocID }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.GetDocFromDB(szDocID, ref byteDocData);

            short shRet = this.GetDocFromFTP(szDocID, ref byteDocData);
            return shRet;
        }

        /// <summary>
        /// �����ĵ�ID��DB�л�ȡ�ĵ�����
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="byteDocData">�ĵ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetDocFromDB(string szDocID, ref byte[] byteDocData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                , ServerData.QCDocTable.DOC_DATA, ServerData.DataTable.QC_DOC, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCAccess.GetDocFromDB"
                        , new string[] { "szSQL" }, new object[] { szSQL }, "û�в�ѯ����¼!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                return GlobalMethods.IO.GetBytes(dataReader, 0, ref byteDocData)
                    ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// �����ĵ�ID��FTP�л�ȡ�ĵ�����
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="byteDocData">�ĵ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetDocFromFTP(string szDocID, ref byte[] byteDocData)
        {
            QCDocInfo docInfo = null;
            short shRet = this.GetDocInfo(szDocID, ref docInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return shRet;

            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }

            string szRemoteFile = ServerParam.Instance.GetFtpDocPath(docInfo, ServerData.FileExt.NUR_DOCUMENT);
            string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                , docInfo.DocID, ServerData.FileExt.NUR_DOCUMENT);
            if (!base.FtpAccess.Download(szRemoteFile, szCacheFile))
            {
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (!GlobalMethods.IO.GetFileBytes(szCacheFile, ref byteDocData))
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
                LogManager.Instance.WriteLog("QCAccess.GetDocFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytesִ��ʧ��!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            base.FtpAccess.CloseConnection();
            GlobalMethods.IO.DeleteFile(szCacheFile);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �����ĵ�ID����ȡ�ĵ�������Ϣ
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="docInfo">�ĵ���Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfo(string szDocID, ref QCDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                LogManager.Instance.WriteLog("QCAccess.GetDocInfo", new string[] { "szDocID" }, new object[] { szDocID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocID = szDocID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.QCDocTable.DOC_ID, ServerData.QCDocTable.DOC_TYPE_ID, ServerData.QCDocTable.DOC_TITLE
                , ServerData.QCDocTable.DOC_TIME, ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_VERSION
                , ServerData.QCDocTable.CREATOR_ID, ServerData.QCDocTable.CREATOR_NAME, ServerData.QCDocTable.MODIFIER_ID
                , ServerData.QCDocTable.MODIFIER_NAME, ServerData.QCDocTable.MODIFY_TIME, ServerData.QCDocTable.WARD_CODE
                , ServerData.QCDocTable.WARD_NAME, ServerData.QCDocTable.SIGN_CODE, ServerData.QCDocTable.CONFID_CODE
                , ServerData.QCDocTable.ORDER_VALUE, ServerData.QCDocTable.RECORD_TIME, ServerData.QCDocTable.RECORD_ID);
                
            string szCondition = string.Format("{0}='{1}'", ServerData.QCDocTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.QC_DOC, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCAccess.GetDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (docInfo == null)
                    docInfo = new QCDocInfo();
                do
                {
                    docInfo.DocID = dataReader.GetString(0);
                    docInfo.DocTypeID = dataReader.GetString(1);
                    docInfo.DocTitle = dataReader.GetString(2);
                    docInfo.DocTime = dataReader.GetDateTime(3);
                    docInfo.DocSetID = dataReader.GetString(4);
                    docInfo.DocVersion = int.Parse(dataReader.GetValue(5).ToString());
                    docInfo.CreatorID = dataReader.GetString(6);
                    docInfo.CreatorName = dataReader.GetString(7);
                    docInfo.ModifierID = dataReader.GetString(8);
                    docInfo.ModifierName = dataReader.GetString(9);
                    docInfo.ModifyTime = dataReader.GetDateTime(10);
                    docInfo.WardCode = dataReader.GetString(11);
                    docInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        docInfo.SignCode = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        docInfo.ConfidCode = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16))
                        docInfo.RecordTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17))
                        docInfo.RecordID = dataReader.GetString(17);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        #endregion

        #region"�������ĵ��ӿ�"
        /// <summary>
        /// �������ĵ�
        /// </summary>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <param name="byteDocData">�ĵ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveDoc(QCDocInfo docInfo, byte[] byteDocData)
        {
            if (docInfo == null)
            {
                LogManager.Instance.WriteLog("QCAccess.SaveDoc"
                    , new string[] { "docInfo" }, new object[] { docInfo }, "�ĵ���Ϣ���������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("QCAccess.SaveDoc"
                    , new string[] { "docInfo" }, new object[] { docInfo }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.SaveDocToDB(docInfo, byteDocData);
            else
                return this.SaveDocToFTP(docInfo, byteDocData);
        }

        /// <summary>
        /// ���ĵ����浽���ݿ�
        /// </summary>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveDocToDB(QCDocInfo docInfo, byte[] byteDocData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //����ĵ�״̬��Ϣ��¼
            DocStatusInfo docStatusInfo = new DocStatusInfo();
            docStatusInfo.DocID = docInfo.DocID;
            docStatusInfo.DocStatus = ServerData.DocStatus.NORMAL;
            docStatusInfo.OperatorID = docInfo.CreatorID;
            docStatusInfo.OperatorName = docInfo.CreatorName;
            docStatusInfo.OperateTime = DateTime.Now;
            short shRet = this.AddDocStatusInfo(docStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //�����ĵ�������Ϣ��¼,�����ĵ�����
            shRet = this.AddDocIndexInfo(docInfo, byteDocData);
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
        /// ���ĵ����浽FTP�ĵ���
        /// </summary>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveDocToFTP(QCDocInfo docInfo, byte[] byteDocData)
        {
            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
            {
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                , docInfo.DocID, ServerData.FileExt.NUR_DOCUMENT);
            try
            {
                if (!GlobalMethods.IO.WriteFileBytes(szCacheFile, byteDocData))
                {
                    LogManager.Instance.WriteLog("QCAccess.SaveDocToFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "WriteFileBytesִ��ʧ��!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                string szRemoteFile = ServerParam.Instance.GetFtpDocPath(docInfo, ServerData.FileExt.NUR_DOCUMENT);
                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!base.FtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;

                if (!base.FtpAccess.Upload(szCacheFile, szRemoteFile))
                {
                    LogManager.Instance.WriteLog("QCAccess.Upload", new string[] { "szRemoteDir" }, new object[] { szRemoteDir }, "�ĵ��ϴ�ʧ�ܣ������쳣!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                if (!base.FtpAccess.ResExists(szRemoteFile, false))
                {
                    LogManager.Instance.WriteLog("QCAccess.Upload", new string[] { "szRemoteDir" }, new object[] { szRemoteDir }, "�ĵ��ϴ�ʧ�ܣ�ȴ���سɹ�!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                short shRet = this.SaveDocToDB(docInfo, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.FtpAccess.DeleteFile(szRemoteFile);
                    return shRet;
                }
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), docInfo.ToString(), "�ĵ��ϴ�����ʧ��!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
        }
        
        #endregion

        #region"���������ĵ��ӿ�"
        /// <summary>
        /// ���������ĵ�
        /// </summary>
        /// <param name="szOldDocID">�����µ��ĵ�ID</param>
        /// <param name="newDocInfo">���º���ĵ���Ϣ</param>
        /// <param name="szUpdateReason">��������ԭ������</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateDoc(string szOldDocID, QCDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("QCAccess.UpdateDoc", "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.UpdateDocToDB(szOldDocID, newDocInfo, szUpdateReason, byteDocData);
            else
                return this.UpdateDocToFTP(szOldDocID, newDocInfo, szUpdateReason, byteDocData);
        }

        /// <summary>
        /// �����ĵ������ݿ�
        /// </summary>
        /// <param name="szOldDocID">�����µ��ĵ�ID</param>
        /// <param name="newDocInfo">���º���ĵ���Ϣ</param>
        /// <param name="szUpdateReason">��������ԭ������</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short UpdateDocToDB(string szOldDocID, QCDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            if (newDocInfo == null || base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //��ʼ���ݿ�����
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            DocStatusInfo docStatusInfo = new DocStatusInfo();

            //����ĵ�IDδ��,��ô���������ĵ�
            if (szOldDocID == newDocInfo.DocID)
            {
                //����ĵ�״̬��Ϣ��¼
                docStatusInfo.DocID = newDocInfo.DocID;
                docStatusInfo.DocStatus = ServerData.DocStatus.NORMAL;
                docStatusInfo.StatusDesc = szUpdateReason;
                docStatusInfo.OperatorID = newDocInfo.ModifierID;
                docStatusInfo.OperatorName = newDocInfo.ModifierName;
                docStatusInfo.OperateTime = newDocInfo.ModifyTime;
                short shRet = this.ModifyDocStatusInfo(ref docStatusInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }

                //�����ĵ�������Ϣ��¼,�����ĵ�����
                shRet = this.ModifyDocIndexInfo(newDocInfo, byteDocData);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }
            }
            else
            {
                //�޸ľɵ��ĵ�״̬Ϊ����
                docStatusInfo.DocID = szOldDocID;
                docStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
                short shRet = this.ModifyOldDocStatusInfo(ref docStatusInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }

                //����ĵ�״̬��Ϣ��¼
                docStatusInfo.DocID = newDocInfo.DocID;
                docStatusInfo.DocStatus = ServerData.DocStatus.NORMAL;
                docStatusInfo.StatusDesc = szUpdateReason;
                docStatusInfo.OperatorID = newDocInfo.ModifierID;
                docStatusInfo.OperatorName = newDocInfo.ModifierName;
                docStatusInfo.OperateTime = newDocInfo.ModifyTime;
                shRet = this.AddDocStatusInfo(docStatusInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }

                //�����ĵ�������Ϣ��¼,�����ĵ�����
                shRet = this.AddDocIndexInfo(newDocInfo, byteDocData);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }
            }
            //�ύ���ݿ����
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �����ĵ���FTP
        /// </summary>
        /// <param name="szOldDocID">�����µ��ĵ�ID</param>
        /// <param name="newDocInfo">���º���ĵ���Ϣ</param>
        /// <param name="szUpdateReason">��������ԭ������</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short UpdateDocToFTP(string szOldDocID, QCDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            if (newDocInfo == null || base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
            {
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                , newDocInfo.DocID, ServerData.FileExt.NUR_DOCUMENT);
            if (!GlobalMethods.IO.WriteFileBytes(szCacheFile, byteDocData))
            {
                LogManager.Instance.WriteLog("QCAccess.UpdateDocToFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "WriteFileBytesִ��ʧ��!");
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            try
            {
                string szRemoteFile = ServerParam.Instance.GetFtpDocPath(newDocInfo, ServerData.FileExt.NUR_DOCUMENT);
                string szRemoteFileBak = null;

                //�����Ҫ����,��ô�Ȱѱ����ǵ��ļ�����һ��
                if (szOldDocID == newDocInfo.DocID)
                {
                    szRemoteFileBak = string.Format("{0}.bak", szRemoteFile);
                    if (!base.FtpAccess.RenameFile(szRemoteFile, szRemoteFileBak))
                        return ServerData.ExecuteResult.EXCEPTION;
                }

                if (!base.FtpAccess.Upload(szCacheFile, szRemoteFile))
                {
                    //��ԭ���ݵ�Զ���ļ�
                    if (!string.IsNullOrEmpty(szRemoteFileBak))
                        base.FtpAccess.RenameFile(szRemoteFileBak, szRemoteFile);
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                short shRet = this.UpdateDocToDB(szOldDocID, newDocInfo, szUpdateReason, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.FtpAccess.DeleteFile(szRemoteFile);

                    //��ԭ���ݵ�Զ���ļ�
                    if (!string.IsNullOrEmpty(szRemoteFileBak))
                        base.FtpAccess.RenameFile(szRemoteFileBak, szRemoteFile);
                    return shRet;
                }

                //ɾ�����ݵ�Զ���ļ�
                if (!string.IsNullOrEmpty(szRemoteFileBak))
                    base.FtpAccess.DeleteFile(szRemoteFileBak);
            }
            finally
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"���ĵ��йص�ժҪ����"

        /// <summary>
        /// ɾ��ָ�����ĵ�ID���������ĵ�ժҪ����
        /// </summary>
        /// <param name="szDocSetID">�����ĵ���ID</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteSummaryData(string szDocSetID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("QCAccess.DeleteSummaryData"
                    , new string[] { "szDocSetID" }, new object[] { szDocSetID }, "�ĵ������ݲ���Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.QCSummaryDataTable.DOC_ID, szDocSetID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.QC_SUMMARY_DATA, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            //finally { base.DataAccess.CloseConnnection(false); }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ɾ���ĵ�
        /// </summary>
        /// <param name="szDocSetID">�����ĵ���</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteDocData(string szDocSetID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("QCAccess.DeleteDocData"
                    , new string[] { "szDocSetID" }, new object[] { szDocSetID }, "�ĵ������ݲ���Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //ɾ��״̬ 
            string szSQL1 = string.Format(" delete from {1} t2 where exists (select 1 from {0} t1 where t1.{2} = '{5}' and t1.{3} = t2.{4})"
            , ServerData.DataTable.QC_DOC, ServerData.DataTable.QC_DOC_STATUS
            , ServerData.QCDocTable.DOC_SETID, ServerData.QCDocTable.DOC_ID
            , ServerData.DocStatusTable.DOC_ID, szDocSetID);
            //ɾ���ؼ�����
            string szSQL2 = string.Format(" delete from {0} where {1} = '{2}' "
            , ServerData.DataTable.QC_SUMMARY_DATA, ServerData.QCSummaryDataTable.DOC_ID, szDocSetID);
            //ɾ������������
            string szSQL3 = string.Format(" delete from {0} where  {1} = '{2}'"
            , ServerData.DataTable.QC_DOC, ServerData.QCDocTable.DOC_SETID, szDocSetID);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL1, CommandType.Text);
                base.DataAccess.ExecuteNonQuery(szSQL2, CommandType.Text);
                base.DataAccess.ExecuteNonQuery(szSQL3, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL1, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ��������һϵ��ժҪ����
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="lstSummaryData">���ĵ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveQCSummaryData(string szDocID, List<QCSummaryData> lstQCSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //��ɾ��֮ǰ���ĵ�ժҪ����
            string szCondition = string.Format("{0}='{1}'", ServerData.QCSummaryDataTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.QC_SUMMARY_DATA, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ���쳣!");
            }
            if (lstQCSummaryData == null)
                lstQCSummaryData = new List<QCSummaryData>();

            //�ٲ����µ��ĵ�ժҪ����
            foreach (QCSummaryData qcSummaryData in lstQCSummaryData)
            {
                DbParameter[] pmi = new DbParameter[1];
                if (!GlobalMethods.Misc.IsEmptyString(qcSummaryData.DataValue))
                {
                    string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                        , ServerData.QCSummaryDataTable.DOC_ID, ServerData.QCSummaryDataTable.DATA_NAME
                        , ServerData.QCSummaryDataTable.DATA_CODE, ServerData.QCSummaryDataTable.DATA_VALUE
                        , ServerData.QCSummaryDataTable.DATA_TYPE, ServerData.QCSummaryDataTable.DATA_UNIT
                        , ServerData.QCSummaryDataTable.DATA_TIME, ServerData.QCSummaryDataTable.WARD_CODE
                        , ServerData.QCSummaryDataTable.RECORD_ID, ServerData.QCSummaryDataTable.DOCTYPE_ID);
                    string szValue = string.Format("'{0}','{1}','{2}',{3},'{4}','{5}',{6},'{7}','{8}','{9}'"
                        , qcSummaryData.DocID, qcSummaryData.DataName, qcSummaryData.DataCode, base.DataAccess.GetSqlParamName("DataValue")
                        , qcSummaryData.DataType, qcSummaryData.DataUnit, base.DataAccess.GetSqlTimeFormat(qcSummaryData.DataTime)
                        , qcSummaryData.WardCode , qcSummaryData.RecordID, qcSummaryData.DocTypeID);
                       
                    szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.QC_SUMMARY_DATA, szField, szValue);

                    //ժҪ������󳤶�Ϊ2000
                    if (qcSummaryData.DataValue != null && qcSummaryData.DataValue.Length > 2000)
                        qcSummaryData.DataValue = qcSummaryData.DataValue.Substring(0, 2000);

                    pmi[0] = new DbParameter("DataValue", qcSummaryData.DataValue == null ? string.Empty : qcSummaryData.DataValue);
                    try
                    {
                        base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
                    }
                    catch (Exception ex)
                    {
                        base.DataAccess.AbortTransaction();
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
                    }
                }
            }
            base.DataAccess.CommitTransaction(true);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ��ȡָ���ĵ���ժҪ�����б�
        /// </summary>
        /// <param name="szDocIDOrRecordID">�ĵ����</param>
        /// <param name="bIsRecordID">�Ƿ��Ǽ�¼ID</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szDocIDOrRecordID, bool bIsRecordID, ref List<QCSummaryData> lstQCSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                , ServerData.QCSummaryDataTable.DOC_ID, ServerData.QCSummaryDataTable.DATA_NAME
                , ServerData.QCSummaryDataTable.DATA_CODE, ServerData.QCSummaryDataTable.DATA_VALUE
                , ServerData.QCSummaryDataTable.DATA_TYPE, ServerData.QCSummaryDataTable.DATA_UNIT
                , ServerData.QCSummaryDataTable.DATA_TIME, ServerData.QCSummaryDataTable.WARD_CODE
                , ServerData.QCSummaryDataTable.RECORD_ID, ServerData.QCSummaryDataTable.DOCTYPE_ID);

            string szCondition = null;
            if (bIsRecordID)
                szCondition = string.Format("{0}='{1}'", ServerData.QCSummaryDataTable.RECORD_ID, szDocIDOrRecordID);
            else
                szCondition = string.Format("{0}='{1}'", ServerData.QCSummaryDataTable.DOC_ID, szDocIDOrRecordID);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.QC_SUMMARY_DATA, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstQCSummaryData == null)
                    lstQCSummaryData = new List<QCSummaryData>();
                do
                {
                    QCSummaryData qcSummaryData = new QCSummaryData();
                    if (!dataReader.IsDBNull(0)) qcSummaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) qcSummaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) qcSummaryData.DataCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) qcSummaryData.DataValue = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcSummaryData.DataType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) qcSummaryData.DataUnit = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) qcSummaryData.DataTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) qcSummaryData.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) qcSummaryData.RecordID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) qcSummaryData.DocTypeID = dataReader.GetString(9);
                    if (!lstQCSummaryData.Contains(qcSummaryData))
                    {
                        lstQCSummaryData.Add(qcSummaryData);
                    }
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
        /// ��ȡָ���ĵ���ID����ָ����ժҪ����
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID��</param>
        /// <param name="szDataName">����ID��</param>
        /// <param name="summaryData">���ص�ժҪ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szDocSetID, string szDataName, ref QCSummaryData qcSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                , ServerData.QCSummaryDataTable.DOC_ID, ServerData.QCSummaryDataTable.DATA_NAME
                , ServerData.QCSummaryDataTable.DATA_CODE, ServerData.QCSummaryDataTable.DATA_VALUE
                , ServerData.QCSummaryDataTable.DATA_TYPE, ServerData.QCSummaryDataTable.DATA_UNIT
                , ServerData.QCSummaryDataTable.DATA_TIME, ServerData.QCSummaryDataTable.WARD_CODE
                , ServerData.QCSummaryDataTable.RECORD_ID, ServerData.QCSummaryDataTable.DOCTYPE_ID);

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.QCSummaryDataTable.DOC_ID, szDocSetID
                , ServerData.QCSummaryDataTable.DATA_NAME, szDataName);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.QC_SUMMARY_DATA, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                qcSummaryData = new QCSummaryData();
                do
                {
                    if (!dataReader.IsDBNull(0)) qcSummaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) qcSummaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) qcSummaryData.DataCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) qcSummaryData.DataValue = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcSummaryData.DataType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) qcSummaryData.DataUnit = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) qcSummaryData.DataTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) qcSummaryData.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) qcSummaryData.RecordID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) qcSummaryData.DocTypeID = dataReader.GetString(9);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion
    }
}
