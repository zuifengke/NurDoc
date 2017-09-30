// ***********************************************************
// ���ݿ���ʲ��벡���ĵ����ݼ������йص����ݵķ�����.
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
    public class DocumentAccess : DBAccessBase
    {
        #region"��д�ĵ������ӿ�"
        /// <summary>
        /// ��ȡָ�����ָ�����ˣ�ָ�������������ĵ���ϸ��Ϣ�б�.
        /// ע�⣺�ýӿڲ��Է��ز����б��������,��Ҫ�ⲿ��������Sort��������
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">������</param>
        /// <param name="szDocTypeID">�ĵ����ʹ���,Ϊ��ʱ���ص��ξ������������͵��ĵ�</param>
        /// <param name="lstDocInfos">�ĵ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfos(string szPatientID, string szVisitID, string szDocTypeID, ref NurDocList lstDocInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17},A.{18},A.{19},A.{20},A.{21},A.{22},A.{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);

            //��������ȷ��һ�ξ���Ĳ�ѯ����
            string szCondition = string.Format("A.{0}={1} AND A.{2}={3}"
                    , ServerData.NurDocTable.PATIENT_ID, base.DataAccess.GetSqlParamName("PATIENT_ID")

                    , ServerData.NurDocTable.VISIT_ID, base.DataAccess.GetSqlParamName("VISIT_ID"));

            //���˵������ϵ��ĵ�
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);

            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                , ServerData.NurDocTable.DOC_TYPE_ID, base.DataAccess.GetSqlParamName("DOC_TYPE_ID"));

            //�����������ʼ��ֹʱ��,�����ȡ���ʱ����ڴ�����
            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            DbParameter[] pmi = new DbParameter[3];
            pmi[0] = new DbParameter("PATIENT_ID", szPatientID);
            pmi[1] = new DbParameter("VISIT_ID", szVisitID);
            pmi[2] = new DbParameter("DOC_TYPE_ID", szDocTypeID);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text, ref pmi);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocInfos == null)
                    lstDocInfos = new NurDocList();
                do
                {
                    NurDocInfo docInfo = new NurDocInfo();
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
                    docInfo.PatientID = dataReader.GetString(11);
                    docInfo.PatientName = dataReader.GetString(12);
                    docInfo.VisitID = dataReader.GetString(13);
                    docInfo.VisitTime = dataReader.GetDateTime(14);
                    docInfo.VisitType = dataReader.GetString(15);
                    docInfo.SubID = dataReader.GetString(16);
                    docInfo.WardCode = dataReader.GetString(17);
                    docInfo.WardName = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19))
                        docInfo.SignCode = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20))
                        docInfo.ConfidCode = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                    if (!dataReader.IsDBNull(22))
                        docInfo.RecordTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23))
                        docInfo.RecordID = dataReader.GetString(23);

                    lstDocInfos.Add(docInfo);
                } while (dataReader.Read());
                lstDocInfos.SortByTime(true);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            //return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ��ȡָ�����ָ�����ˣ�ָ�������������ĵ���ϸ��Ϣ�б�.       HJX
        /// ע�⣺�ýӿڲ��Է��ز����б��������,��Ҫ�ⲿ��������Sort��������
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">������</param>
        /// <param name="szDocTypeID">�ĵ����ʹ���,Ϊ��ʱ���ص��ξ������������͵��ĵ�</param>
        /// <param name="dtBeginTime">��ѯ��ʼ����ʱ��</param>
        /// <param name="dtEndTime">��ѯ��ֹ����ʱ��</param>
        /// <param name="lstDocInfos">�ĵ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfos(string szPatientID, string szVisitID, string szDocTypeID
            , DateTime dtBeginTime, DateTime dtEndTime, ref NurDocList lstDocInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17},A.{18},A.{19},A.{20},A.{21},A.{22},A.{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);

            //��������ȷ��һ�ξ���Ĳ�ѯ����
            string szCondition = string.Format("A.{0}={1} AND A.{2}={3}"
                    , ServerData.NurDocTable.PATIENT_ID, base.DataAccess.GetSqlParamName("PATIENT_ID")

                    , ServerData.NurDocTable.VISIT_ID, base.DataAccess.GetSqlParamName("VISIT_ID"));

            //���˵������ϵ��ĵ�
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                 , ServerData.NurDocTable.DOC_TYPE_ID, szDocTypeID);

            //�����������ʼ��ֹʱ��,�����ȡ���ʱ����ڴ�����

            szCondition = string.Format("{0} AND A.{1}>={2} AND A.{3}<={4}", szCondition
                , ServerData.NurDocTable.RECORD_TIME, base.DataAccess.GetSqlParamName("BEGIN_TIME")
                , ServerData.NurDocTable.RECORD_TIME, base.DataAccess.GetSqlParamName("END_TIME"));

            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            DbParameter[] pmi = new DbParameter[4];
            pmi[0] = new DbParameter("PATIENT_ID", szPatientID);
            pmi[1] = new DbParameter("VISIT_ID", szVisitID);
            pmi[2] = new DbParameter("BEGIN_TIME", dtBeginTime);
            pmi[3] = new DbParameter("END_TIME", dtEndTime);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text, ref pmi);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocInfos == null)
                    lstDocInfos = new NurDocList();
                do
                {
                    NurDocInfo docInfo = new NurDocInfo();
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
                    docInfo.PatientID = dataReader.GetString(11);
                    docInfo.PatientName = dataReader.GetString(12);
                    docInfo.VisitID = dataReader.GetString(13);
                    docInfo.VisitTime = dataReader.GetDateTime(14);
                    docInfo.VisitType = dataReader.GetString(15);
                    docInfo.SubID = dataReader.GetString(16);
                    docInfo.WardCode = dataReader.GetString(17);
                    docInfo.WardName = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19))
                        docInfo.SignCode = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20))
                        docInfo.ConfidCode = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                    if (!dataReader.IsDBNull(22))
                        docInfo.RecordTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23))
                        docInfo.RecordID = dataReader.GetString(23);

                    lstDocInfos.Add(docInfo);
                } while (dataReader.Read());
                lstDocInfos.SortByTime(true);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡָ�����ָ�����ˣ�ָ�������������ĵ���ϸ��Ϣ�б�.
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">������</param>
        /// <param name="szDocTypeID">�ĵ����ʹ���,Ϊ��ʱ���ص��ξ������������͵��ĵ�</param>
        /// <param name="dtBeginTime">��ѯ��ʼ����ʱ��</param>
        /// <param name="dtEndTime">��ѯ��ֹ����ʱ��</param>
        /// <param name="lstDocInfos">�ĵ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfosOrderByRecordTime(string szPatientID, string szVisitID, string szDocTypeID
            , DateTime dtBeginTime, DateTime dtEndTime, ref NurDocList lstDocInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17},A.{18},A.{19},A.{20},A.{21},A.{22},A.{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);

            //��������ȷ��һ�ξ���Ĳ�ѯ����
            string szCondition = string.Format("A.{0}={1} AND A.{2}={3}"
                    , ServerData.NurDocTable.PATIENT_ID, base.DataAccess.GetSqlParamName("PATIENT_ID")

                    , ServerData.NurDocTable.VISIT_ID, base.DataAccess.GetSqlParamName("VISIT_ID"));

            //���˵������ϵ��ĵ�
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                 , ServerData.NurDocTable.DOC_TYPE_ID, szDocTypeID);

            //�����������ʼ��ֹʱ��,�����ȡ���ʱ����ڴ�����

            szCondition = string.Format("{0} AND A.{1}>={2} AND A.{3}<={4}", szCondition
                , ServerData.NurDocTable.RECORD_TIME, base.DataAccess.GetSqlParamName("BEGIN_TIME")
                , ServerData.NurDocTable.RECORD_TIME, base.DataAccess.GetSqlParamName("END_TIME"));

            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, ServerData.NurDocTable.RECORD_TIME);

            DbParameter[] pmi = new DbParameter[4];
            pmi[0] = new DbParameter("PATIENT_ID", szPatientID);
            pmi[1] = new DbParameter("VISIT_ID", szVisitID);
            pmi[2] = new DbParameter("BEGIN_TIME", dtBeginTime);
            pmi[3] = new DbParameter("END_TIME", dtEndTime);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text, ref pmi);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocInfos == null)
                    lstDocInfos = new NurDocList();
                do
                {
                    NurDocInfo docInfo = new NurDocInfo();
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
                    docInfo.PatientID = dataReader.GetString(11);
                    docInfo.PatientName = dataReader.GetString(12);
                    docInfo.VisitID = dataReader.GetString(13);
                    docInfo.VisitTime = dataReader.GetDateTime(14);
                    docInfo.VisitType = dataReader.GetString(15);
                    docInfo.SubID = dataReader.GetString(16);
                    docInfo.WardCode = dataReader.GetString(17);
                    docInfo.WardName = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19))
                        docInfo.SignCode = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20))
                        docInfo.ConfidCode = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                    if (!dataReader.IsDBNull(22))
                        docInfo.RecordTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23))
                        docInfo.RecordID = dataReader.GetString(23);

                    lstDocInfos.Add(docInfo);
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
        /// ע�⣺�ýӿڷ���������ͼ����������ĵ��б�
        /// </summary>
        /// <param name="szWardCode">��ǰ���Ҵ���</param>
        /// <param name="szDocTypeID">�ĵ����ʹ���,Ϊ��ʱ���ص��ξ������������͵��ĵ�</param>
        /// <param name="lstDocInfos">�ĵ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfos(string szWardCode, string szDocTypeID
            , ref NurDocList lstDocInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17},A.{18},A.{19},A.{20},A.{21},A.{22},A.{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);

            //��������ȷ��һ�ξ���Ĳ�ѯ����
            string szCondition = string.Format("1=1");

            //���˵������ϵ��ĵ�
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);

            //����������ĵ�����,�����ȡ���ĵ����Ͷ�Ӧ���ĵ��б�
            if (!GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                    , ServerData.NurDocTable.DOC_TYPE_ID, szDocTypeID);
            }
            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, szTable, szCondition, " patient_id, doc_type,doc_time ");

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocInfos == null)
                    lstDocInfos = new NurDocList();
                do
                {
                    NurDocInfo docInfo = new NurDocInfo();
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
                    docInfo.PatientID = dataReader.GetString(11);
                    docInfo.PatientName = dataReader.GetString(12);
                    docInfo.VisitID = dataReader.GetString(13);
                    docInfo.VisitTime = dataReader.GetDateTime(14);
                    docInfo.VisitType = dataReader.GetString(15);
                    docInfo.SubID = dataReader.GetString(16);
                    docInfo.WardCode = dataReader.GetString(17);
                    docInfo.WardName = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19))
                        docInfo.SignCode = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20))
                        docInfo.ConfidCode = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                    if (!dataReader.IsDBNull(22))
                        docInfo.RecordTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23))
                        docInfo.RecordID = dataReader.GetString(23);
                    lstDocInfos.Add(docInfo);
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
        /// ��ȡָ�������¼ID��Ӧ�Ļ������б�
        /// </summary>
        /// <param name="szRecordID">�����¼ID</param>
        /// <param name="lstDocInfos">�ĵ���Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetRecordDocInfos(string szRecordID, ref NurDocList lstDocInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szRecordID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetRecordDocInfos"
                    , new string[] { "szRecordID" }, new object[] { szRecordID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17},A.{18},A.{19},A.{20},A.{21},A.{22},A.{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);
            string szCondition = string.Format("{0}='{1}' AND A.{2}=B.{3} AND B.{4}!='{5}'"
                , ServerData.NurDocTable.RECORD_ID, szRecordID
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, ServerData.NurDocTable.DOC_TIME);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocInfos == null)
                    lstDocInfos = new NurDocList();
                do
                {
                    NurDocInfo docInfo = new NurDocInfo();
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
                    docInfo.PatientID = dataReader.GetString(11);
                    docInfo.PatientName = dataReader.GetString(12);
                    docInfo.VisitID = dataReader.GetString(13);
                    docInfo.VisitTime = dataReader.GetDateTime(14);
                    docInfo.VisitType = dataReader.GetString(15);
                    docInfo.SubID = dataReader.GetString(16);
                    docInfo.WardCode = dataReader.GetString(17);
                    docInfo.WardName = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19))
                        docInfo.SignCode = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20))
                        docInfo.ConfidCode = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                    if (!dataReader.IsDBNull(22))
                        docInfo.RecordTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23))
                        docInfo.RecordID = dataReader.GetString(23);
                    lstDocInfos.Add(docInfo);
                } while (dataReader.Read());
                lstDocInfos.SortByTime(false);
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
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="docInfo">�ĵ���Ϣ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfo(string szDocID, ref NurDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetDocInfo", new string[] { "szDocID" }, new object[] { szDocID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocID = szDocID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurDocTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_DOC, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (docInfo == null)
                    docInfo = new NurDocInfo();
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
                    docInfo.PatientID = dataReader.GetString(11);
                    docInfo.PatientName = dataReader.GetString(12);
                    docInfo.VisitID = dataReader.GetString(13);
                    docInfo.VisitTime = dataReader.GetDateTime(14);
                    docInfo.VisitType = dataReader.GetString(15);
                    docInfo.SubID = dataReader.GetString(16);
                    docInfo.WardCode = dataReader.GetString(17);
                    docInfo.WardName = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19))
                        docInfo.SignCode = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20))
                        docInfo.ConfidCode = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                    if (!dataReader.IsDBNull(22))
                        docInfo.RecordTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23))
                        docInfo.RecordID = dataReader.GetString(23);
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
        /// �����ĵ�������Ϣ
        /// </summary>
        /// <param name="docInfo">�ĵ�������Ϣ��</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddDocIndexInfo(NurDocInfo docInfo, byte[] byteDocData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);
            string szValue = string.Format("'{0}','{1}','{2}',{3},'{4}',{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}',{14},'{15}','{16}','{17}','{18}','{19}','{20}',{21},{22},'{23}'"
                , docInfo.DocID, docInfo.DocTypeID, docInfo.DocTitle, base.DataAccess.GetSqlTimeFormat(docInfo.DocTime)
                , docInfo.DocSetID, docInfo.DocVersion, docInfo.CreatorID, docInfo.CreatorName, docInfo.ModifierID
                , docInfo.ModifierName, base.DataAccess.GetSqlTimeFormat(docInfo.ModifyTime), docInfo.PatientID
                , docInfo.PatientName, docInfo.VisitID, base.DataAccess.GetSqlTimeFormat(docInfo.VisitTime), docInfo.VisitType
                , docInfo.SubID, docInfo.WardCode, docInfo.WardName, docInfo.SignCode, docInfo.ConfidCode, docInfo.OrderValue
                , base.DataAccess.GetSqlTimeFormat(docInfo.RecordTime), docInfo.RecordID);

            DbParameter[] pmi = null;
            if (byteDocData != null)
            {
                szField = string.Format("{0},{1}", szField, ServerData.NurDocTable.DOC_DATA);
                szValue = string.Format("{0},{1}", szValue, base.DataAccess.GetSqlParamName("DocData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("DocData", byteDocData);
            }
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_DOC, szField, szValue);

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
                LogManager.Instance.WriteLog("DocumentAccess.AddDocIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
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
        private short ModifyDocIndexInfo(NurDocInfo docInfo, byte[] byteDocData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3},{4}='{5}',{6}='{7}',{8}={9},{10}='{11}',{12}='{13}',{14}={15}"
                , ServerData.NurDocTable.DOC_TITLE, docInfo.DocTitle
                , ServerData.NurDocTable.DOC_VERSION, docInfo.DocVersion
                , ServerData.NurDocTable.MODIFIER_ID, docInfo.ModifierID
                , ServerData.NurDocTable.MODIFIER_NAME, docInfo.ModifierName
                , ServerData.NurDocTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(docInfo.ModifyTime)
                , ServerData.NurDocTable.SIGN_CODE, docInfo.SignCode
                , ServerData.NurDocTable.CONFID_CODE, docInfo.ConfidCode
                , ServerData.NurDocTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(docInfo.RecordTime));
            string szCondition = string.Format("{0}='{1}'", ServerData.NurDocTable.DOC_ID, docInfo.DocID);

            DbParameter[] pmi = null;
            if (byteDocData != null)
            {
                szField = string.Format("{0},{1}={2}", szField, ServerData.NurDocTable.DOC_DATA, base.DataAccess.GetSqlParamName("DocData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("DocData", byteDocData);
            }
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_DOC, szField, szCondition);

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
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �õ���ǰ����������ָ���ĵ����͵����°汾��δ���ϵ��ĵ�
        /// </summary>
        /// <param name="szPatientID">���˱��</param>
        /// <param name="szVisitID">������</param>
        /// <param name="szVisitType">��������</param>
        /// <param name="dtVisitTime">����ʱ��</param>
        /// <param name="szDocTypeID">�ĵ����ͱ��</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetActiveDocInfo(string szPatientID, string szVisitID, string szVisitType, DateTime dtVisitTime, string szDocTypeID, ref NurDocInfo docInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},A.{2},A.{3},A.{4},A.{5},A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12},A.{13},A.{14},A.{15},A.{16},A.{17},A.{18},A.{19},A.{20},A.{21},A.{22},A.{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);

            //��������ȷ��һ�ξ���Ĳ�ѯ����
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}='{3}'", ServerData.NurDocTable.VISIT_ID, szVisitID
                    , ServerData.NurDocTable.PATIENT_ID, szPatientID);

            szCondition = string.Format("{0} AND A.{1}='{2}' AND A.{3}=B.{4} AND B.{5}!='{6}'"
                , szCondition, ServerData.NurDocTable.DOC_TYPE_ID, szDocTypeID
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (docInfo == null)
                    docInfo = new NurDocInfo();
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
                docInfo.PatientID = dataReader.GetString(11);
                docInfo.PatientName = dataReader.GetString(12);
                docInfo.VisitID = dataReader.GetString(13);
                docInfo.VisitTime = dataReader.GetDateTime(14);
                docInfo.VisitType = dataReader.GetString(15);
                docInfo.SubID = dataReader.GetString(16);
                docInfo.WardCode = dataReader.GetString(17);
                docInfo.WardName = dataReader.GetString(18);
                if (!dataReader.IsDBNull(19))
                    docInfo.SignCode = dataReader.GetString(19);
                if (!dataReader.IsDBNull(20))
                    docInfo.ConfidCode = dataReader.GetString(20);
                if (!dataReader.IsDBNull(21))
                    docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                if (!dataReader.IsDBNull(22))
                    docInfo.RecordTime = dataReader.GetDateTime(22);
                if (!dataReader.IsDBNull(23))
                    docInfo.RecordID = dataReader.GetString(23);
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
        public short GetLatestDocInfo(string szDocSetID, ref NurDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetLatestDocInfo", new string[] { "szDocSetID" }
                    , new object[] { szDocSetID }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocSetID = szDocSetID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurDocTable.DOC_SETID, szDocSetID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, ServerData.DataTable.NUR_DOC, szCondition
                , ServerData.NurDocTable.DOC_VERSION);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (docInfo == null)
                    docInfo = new NurDocInfo();

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
                docInfo.PatientID = dataReader.GetString(11);
                docInfo.PatientName = dataReader.GetString(12);
                docInfo.VisitID = dataReader.GetString(13);
                docInfo.VisitTime = dataReader.GetDateTime(14);
                docInfo.VisitType = dataReader.GetString(15);
                docInfo.SubID = dataReader.GetString(16);
                docInfo.WardCode = dataReader.GetString(17);
                docInfo.WardName = dataReader.GetString(18);
                if (!dataReader.IsDBNull(19))
                    docInfo.SignCode = dataReader.GetString(19);
                if (!dataReader.IsDBNull(20))
                    docInfo.ConfidCode = dataReader.GetString(20);
                if (!dataReader.IsDBNull(21))
                    docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                if (!dataReader.IsDBNull(22))
                    docInfo.RecordTime = dataReader.GetDateTime(22);
                if (!dataReader.IsDBNull(23))
                    docInfo.RecordID = dataReader.GetString(23);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        ///  �����ĵ���ID��ѯ��ǰ�ĵ�����Ӧ�������޶��Ĳ�����¼
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID</param>
        /// <param name="lstDocInfo">�����ĵ��б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfoBySetID(string szDocSetID, ref List<NurDocInfo> lstDocInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetDocInfoBySetID", new string[] { "szDocSetID" }
                    , new object[] { szDocSetID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocSetID = szDocSetID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}"
                , ServerData.NurDocTable.DOC_ID, ServerData.NurDocTable.DOC_TYPE_ID, ServerData.NurDocTable.DOC_TITLE
                , ServerData.NurDocTable.DOC_TIME, ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_VERSION
                , ServerData.NurDocTable.CREATOR_ID, ServerData.NurDocTable.CREATOR_NAME, ServerData.NurDocTable.MODIFIER_ID
                , ServerData.NurDocTable.MODIFIER_NAME, ServerData.NurDocTable.MODIFY_TIME, ServerData.NurDocTable.PATIENT_ID
                , ServerData.NurDocTable.PATIENT_NAME, ServerData.NurDocTable.VISIT_ID, ServerData.NurDocTable.VISIT_TIME
                , ServerData.NurDocTable.VISIT_TYPE, ServerData.NurDocTable.SUB_ID, ServerData.NurDocTable.WARD_CODE
                , ServerData.NurDocTable.WARD_NAME
                , ServerData.NurDocTable.SIGN_CODE, ServerData.NurDocTable.CONFID_CODE, ServerData.NurDocTable.ORDER_VALUE
                , ServerData.NurDocTable.RECORD_TIME, ServerData.NurDocTable.RECORD_ID);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurDocTable.DOC_SETID, szDocSetID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, ServerData.DataTable.NUR_DOC, szCondition
                , ServerData.NurDocTable.MODIFY_TIME);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocInfoBySetID", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstDocInfo == null)
                    lstDocInfo = new List<NurDocInfo>();
                do
                {
                    NurDocInfo docInfo = new NurDocInfo();
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
                    docInfo.PatientID = dataReader.GetString(11);
                    docInfo.PatientName = dataReader.GetString(12);
                    docInfo.VisitID = dataReader.GetString(13);
                    docInfo.VisitTime = dataReader.GetDateTime(14);
                    docInfo.VisitType = dataReader.GetString(15);
                    docInfo.SubID = dataReader.GetString(16);
                    docInfo.WardCode = dataReader.GetString(17);
                    docInfo.WardName = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19))
                        docInfo.SignCode = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20))
                        docInfo.ConfidCode = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21))
                        docInfo.OrderValue = int.Parse(dataReader.GetValue(21).ToString());
                    if (!dataReader.IsDBNull(22))
                        docInfo.RecordTime = dataReader.GetDateTime(22);
                    if (!dataReader.IsDBNull(23))
                        docInfo.RecordID = dataReader.GetString(23);
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
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocTitle", new string[] { "szDocID" }, new object[] { szDocID }, "�ĵ�ID����Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (GlobalMethods.Misc.IsEmptyString(szDocTitle))
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocTitle", new string[] { "szDocTitle" }, new object[] { szDocTitle }, "�ĵ����ⲻ��Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}'", ServerData.NurDocTable.DOC_TITLE, szDocTitle);
            string szCondition = string.Format("{0}='{1}'", ServerData.NurDocTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_DOC, szField, szCondition);

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
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocTitle", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
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
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.DOC_STATUS, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocStatusInfo", new string[] { "szSQL" }
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
                LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", "��������Ϊnull!");
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
                LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
                return ServerData.ExecuteResult.RES_IS_EXIST;
            }
            else if (currDocStatus.DocStatus == ServerData.DocStatus.ARCHIVED)
            {
                newStatusInfo.StatusMessage = string.Format(
                    ServerData.DocStatus.ARCHIVED_STATUS_DESC,
                    currDocStatus.OperatorName,
                    currDocStatus.OperateTime.ToString("yyyy��M��d�� HH:mm"));
                LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
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
                    LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
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
                LogManager.Instance.WriteLog("DocumentAccess.AddDocStatusInfo", "��������Ϊnull!");
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
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.DOC_STATUS, szField, szValue);

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
                LogManager.Instance.WriteLog("DocumentAccess.AddDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
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
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", "��������Ϊnull!");
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
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.DOC_STATUS, szField, szCondition);

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
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
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
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", "��������Ϊnull!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}'"
                , ServerData.DocStatusTable.DOC_STATUS, docStatusInfo.DocStatus);
            string szCondition = string.Format("{0}='{1}'", ServerData.DocStatusTable.DOC_ID, docStatusInfo.DocID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.DOC_STATUS, szField, szCondition);

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
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
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
                LogManager.Instance.WriteLog("DocumentAccess.GetDocByID"
                    , new string[] { "szDocID" }, new object[] { szDocID }, "�ĵ�ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetDocByID"
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

            string szCondition = string.Format("{0}='{1}'", ServerData.NurDocTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                , ServerData.NurDocTable.DOC_DATA, ServerData.DataTable.NUR_DOC, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocFromDB"
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
            NurDocInfo docInfo = null;
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
                LogManager.Instance.WriteLog("DocumentAccess.GetDocFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytesִ��ʧ��!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            base.FtpAccess.CloseConnection();
            GlobalMethods.IO.DeleteFile(szCacheFile);
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"�������ĵ��ӿ�"
        /// <summary>
        /// �������ĵ�
        /// </summary>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <param name="byteDocData">�ĵ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveDoc(NurDocInfo docInfo, byte[] byteDocData)
        {
            if (docInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "docInfo" }, new object[] { docInfo }, "�ĵ���Ϣ���������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "docInfo" }, new object[] { docInfo }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.SaveDocToDB(docInfo, byteDocData);
            else
                return this.SaveDocToFTP(docInfo, byteDocData);
        }

        /// <summary>
        /// �������ĵ�
        /// </summary>
        /// <param name="lstDocInfo">�ĵ���Ϣ</param>
        /// <param name="htDocTypeData">�ĵ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveDoc(List<NurDocInfo> lstDocInfo, Hashtable htDocTypeData)
        {
            if (lstDocInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "lstDocInfo" }, new object[] { lstDocInfo }, "�ĵ��б���Ϣ���������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (htDocTypeData == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "htDocTypeData" }, new object[] { htDocTypeData }, "�ĵ���Ϣ���Ӧ�ĵ����ݹ�ϣ���������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "lstDocInfo" }, new object[] { lstDocInfo }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            foreach (NurDocInfo nurDocInfo in lstDocInfo)
            {
                if (nurDocInfo == null)
                    continue;
                byte[] byteTempletData = htDocTypeData[nurDocInfo.DocTypeID] as byte[];
                if (byteTempletData == null)
                    continue;
                if (base.StorageMode == StorageMode.DB)
                {
                    short shRet = this.SaveDocToDB(nurDocInfo, byteTempletData);
                    if (shRet != ServerData.ExecuteResult.OK)
                        return shRet;
                }
                else
                {
                    short shRet = this.SaveDocToFTP(nurDocInfo, byteTempletData);
                    if (shRet != ServerData.ExecuteResult.OK)
                        return shRet;
                }
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���ĵ����浽���ݿ�
        /// </summary>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <param name="byteDocData">�ĵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveDocToDB(NurDocInfo docInfo, byte[] byteDocData)
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
        private short SaveDocToFTP(NurDocInfo docInfo, byte[] byteDocData)
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
                    LogManager.Instance.WriteLog("DocumentAccess.SaveDocToFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "WriteFileBytesִ��ʧ��!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                string szRemoteFile = ServerParam.Instance.GetFtpDocPath(docInfo, ServerData.FileExt.NUR_DOCUMENT);
                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!base.FtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;

                if (!base.FtpAccess.Upload(szCacheFile, szRemoteFile))
                {
                    LogManager.Instance.WriteLog("DocumentAccess.Upload", new string[] { "szRemoteDir" }, new object[] { szRemoteDir }, "�ĵ��ϴ�ʧ�ܣ������쳣!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                if (!base.FtpAccess.ResExists(szRemoteFile, false))
                {
                    LogManager.Instance.WriteLog("DocumentAccess.Upload", new string[] { "szRemoteDir" }, new object[] { szRemoteDir }, "�ĵ��ϴ�ʧ�ܣ�ȴ���سɹ�!");
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
        public short UpdateDoc(string szOldDocID, NurDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.UpdateDoc", "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
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
        private short UpdateDocToDB(string szOldDocID, NurDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
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
        private short UpdateDocToFTP(string szOldDocID, NurDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
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
                LogManager.Instance.WriteLog("DocumentAccess.UpdateDocToFTP", new string[] { "szCacheFile" }
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

        #region"���ĵ�������Ϣ�ӿ�"
        /// <summary>
        /// ��������һ���ĵ����ӹ�ϵ����
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="szCaller">������</param>
        /// <param name="szChildID">���ĵ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveChildDocInfo(string szDocID, string szCaller, string szChildID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //��ɾ��֮ǰ�ĸ����ĵ���Ϣ
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ChildDocTable.DOC_ID, szDocID, ServerData.ChildDocTable.CALLER, szCaller);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.CHILD_DOC, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }

            //�ٲ����µĸ����ĵ���Ϣ
            string szField = string.Format("{0},{1},{2}"
                , ServerData.ChildDocTable.DOC_ID, ServerData.ChildDocTable.CALLER
                , ServerData.ChildDocTable.CHILD_ID);
            string szValue = string.Format("'{0}','{1}','{2}'", szDocID, szCaller, szChildID);
            szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.CHILD_DOC, szField, szValue);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            base.DataAccess.CommitTransaction(true);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ��ȡָ���ĵ�ָ�������ߵ����ĵ�ID��
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="szCaller">������</param>
        /// <param name="szChildID">���ص����ĵ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetChildDocID(string szDocID, string szCaller, ref string szChildID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = ServerData.ChildDocTable.CHILD_ID;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ChildDocTable.DOC_ID, szDocID, ServerData.ChildDocTable.CALLER, szCaller);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.CHILD_DOC, szCondition);

            try
            {
                object objValue = base.DataAccess.ExecuteScalar(szSQL, CommandType.Text);
                if (objValue == null || objValue == System.DBNull.Value)
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                szChildID = objValue.ToString();
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
        }

        /// <summary>
        /// ��ȡָ���ĵ�ָ���ĵ��������
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="lstChilDocID">���ص����ĵ��������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetChildDocIDList(string szDocID, ref List<string> lstChilDocID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition_1 = string.Format("{0}='{1}'"
                , ServerData.ChildDocTable.DOC_ID, szDocID);
            string szSQL_1 = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.ChildDocTable.CHILD_ID, ServerData.DataTable.CHILD_DOC, szCondition_1);
            string szCondition_2 = string.Format("{0} IN ({1})"
                , ServerData.NurDocTable.DOC_SETID, szSQL_1);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.NurDocTable.DOC_ID, ServerData.DataTable.NUR_DOC, szCondition_2);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "δ��ѯ����¼");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }

                do
                {
                    lstChilDocID.Add(dataReader.GetString(0));
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
        /// ��ȡָ���ĵ����ĵ���Ϣ�б�
        /// </summary>
        /// <param name="szDocSetID">�ĵ����</param>
        /// <param name="lstChilDocs">���ص����ĵ���Ϣ�б�</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetChildDocList(string szDocSetID, ref List<ChildDocInfo> lstChilDocs)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1}", ServerData.ChildDocTable.CHILD_ID, ServerData.ChildDocTable.CALLER);
            string szCondition_1 = string.Format("{0}='{1}'"
                , ServerData.ChildDocTable.DOC_ID, szDocSetID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.CHILD_DOC, szCondition_1);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstChilDocs == null)
                    lstChilDocs = new List<ChildDocInfo>();
                do
                {
                    ChildDocInfo childDocInfo = new ChildDocInfo();
                    childDocInfo.DocID = szDocSetID;
                    if (!dataReader.IsDBNull(0))
                        childDocInfo.ChildDocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1))
                        childDocInfo.Caller = dataReader.GetString(1);

                    lstChilDocs.Add(childDocInfo);
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

        #region"���ĵ��йص�ժҪ����"
        /// <summary>
        /// ��������һϵ��ժҪ���� HJX
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="lstSummaryData">���ĵ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveSummaryData(string szDocID, List<SummaryData> lstSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            List<NurDocInfo> lstDocInfo = new List<NurDocInfo>();
            this.GetDocInfoBySetID(szDocID, ref lstDocInfo);
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //��ɾ��֮ǰ���ĵ�ժҪ����
            string szCondition = string.Format("{0}='{1}'", ServerData.SummaryDataTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ���쳣!");
            }

            if (lstSummaryData == null)
                lstSummaryData = new List<SummaryData>();

            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData.Category == 5)
                {
                    //��ɾ��֮ǰ��ҽ����д����
                    szCondition = string.Format("{0} = '{1}'", ServerData.OrdersWriteBackDataTable.DOC_ID, szDocID);
                    szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.ORDERS_WRITE_BACK, szCondition);
                    try
                    {
                        base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                    }
                    catch (Exception ex)
                    {
                        base.DataAccess.AbortTransaction();
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ���쳣!");
                    }
                    break;
                }
            }
            //�ٲ����µ��ĵ�ժҪ����
            foreach (SummaryData summaryData in lstSummaryData)
            {
                DbParameter[] pmi = new DbParameter[1];
                if (!GlobalMethods.Misc.IsEmptyString(summaryData.DataValue) && summaryData.Category != 5)
                {
                    string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                        , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                        , ServerData.SummaryDataTable.DATA_CODE
                        , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                        , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                        , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                        , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                        , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);
                    string szValue = string.Format("'{0}','{1}','{2}',{3},'{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}','{12}'"
                        , summaryData.DocID, summaryData.DataName, summaryData.DataCode, base.DataAccess.GetSqlParamName("DataValue")
                        , summaryData.DataType, summaryData.DataUnit, base.DataAccess.GetSqlTimeFormat(summaryData.DataTime)
                        , summaryData.PatientID, summaryData.VisitID, summaryData.SubID, summaryData.WardCode
                        , summaryData.RecordID, summaryData.DocTypeID);
                    szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.DOC_SUMMARY_DATA, szField, szValue);

                    //ժҪ������󳤶�Ϊ2000
                    if (summaryData.DataValue != null && summaryData.DataValue.Length > 2000)
                        summaryData.DataValue = summaryData.DataValue.Substring(0, 2000);

                    pmi[0] = new DbParameter("DataValue", summaryData.DataValue == null ? string.Empty : summaryData.DataValue);
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

                //ժҪ������Category���ڵ���1��С�ڵ���2,����������
                if (summaryData.Category >= 1 && summaryData.Category <= 2)
                {
                    szSQL = "SaveVitalSignsData";

                    pmi = new DbParameter[14];
                    pmi[0] = new DbParameter("PatientID", summaryData.PatientID);
                    pmi[1] = new DbParameter("VisitID", summaryData.VisitID);
                    pmi[2] = new DbParameter("SubID", summaryData.SubID);
                    pmi[3] = new DbParameter("RecordDate", summaryData.DataTime.Date);
                    pmi[4] = new DbParameter("RecordTime", summaryData.DataTime);
                    pmi[5] = new DbParameter("RecordName", summaryData.DataName);
                    pmi[6] = new DbParameter("RecordData", summaryData.DataValue == null ? string.Empty : summaryData.DataValue);
                    pmi[7] = new DbParameter("DataType", summaryData.DataType == null ? string.Empty : summaryData.DataType);
                    pmi[8] = new DbParameter("DataUnit", summaryData.DataUnit == null ? string.Empty : summaryData.DataUnit);
                    pmi[9] = new DbParameter("Category", summaryData.Category);
                    pmi[10] = new DbParameter("ContainsTime", summaryData.ContainsTime ? 1 : 0);
                    pmi[11] = new DbParameter("Remarks", summaryData.Remarks == null ? string.Empty : summaryData.Remarks);

                    if (GlobalMethods.Misc.IsEmptyString(summaryData.RecordID))
                    {
                        pmi[12] = new DbParameter("SourceTag", summaryData.DocID == null ? string.Empty : summaryData.DocID);
                        pmi[13] = new DbParameter("SourceType", ServerData.VitalSignsSourceType.NUR_DOC);
                    }
                    else
                    {
                        pmi[12] = new DbParameter("SourceTag", summaryData.RecordID == null ? string.Empty : summaryData.RecordID);
                        pmi[13] = new DbParameter("SourceType", ServerData.VitalSignsSourceType.NUR_REC);
                    }
                    try
                    {
                        base.DataAccess.ExecuteNonQuery(szSQL, CommandType.StoredProcedure, ref pmi);
                    }
                    catch (Exception ex)
                    {
                        base.DataAccess.AbortTransaction();
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "���������ڴ洢���̱���ִ��ʧ��!");
                    }
                }
                //�������ҩ��
                else if (summaryData.Category == 3)
                {
                    pmi = new DbParameter[3];
                    pmi[0] = new DbParameter("patientId", summaryData.PatientID);
                    pmi[1] = new DbParameter("visitId", summaryData.VisitID);
                    pmi[2] = new DbParameter("allergyDrugs", summaryData.DataValue);
                    try
                    {
                        base.DataAccess.ExecuteNonQuery("SaveAllergyDrugs", CommandType.StoredProcedure, ref pmi);
                    }
                    catch (Exception ex)
                    {
                        base.DataAccess.AbortTransaction();
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "����ҩ���ڴ洢���̱���ִ��ʧ��!");
                    }
                }
                else if (summaryData.Category == 4)
                {
                    if (lstDocInfo == null || lstDocInfo.Count <= 0)
                        continue;
                    decimal cycle = 0;
                    if (!decimal.TryParse(summaryData.DataValue, out cycle))
                    {
                        continue;
                    }
                    AssementCycleAccess assementCycleAccess = new AssementCycleAccess();
                    AssessmentCycle assessmentCycle = null;
                    short shRet = assementCycleAccess.GetAssementCycle(szDocID, ref assessmentCycle);
                    if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                    {
                        assessmentCycle = new AssessmentCycle();
                        assessmentCycle.ID = assessmentCycle.MakeID();
                        assessmentCycle.CREATOR_ID = lstDocInfo[0].CreatorID;
                        assessmentCycle.CREATOR_NAME = lstDocInfo[0].CreatorName;
                        assessmentCycle.CYCLE = cycle;
                        assessmentCycle.DATA = summaryData.DataName;
                        assessmentCycle.DOCTYPE_ID = lstDocInfo[0].DocTypeID;
                        assessmentCycle.DOCTYPE_NAME = lstDocInfo[0].DocTitle;
                        assessmentCycle.DOC_ID = lstDocInfo[0].DocSetID;
                        assessmentCycle.PATIENT_ID = lstDocInfo[0].PatientID;
                        assessmentCycle.RECORD_TIME = lstDocInfo[0].RecordTime;
                        assessmentCycle.UNIT = summaryData.DataUnit;
                        assessmentCycle.VISIT_ID = lstDocInfo[0].VisitID;
                        assessmentCycle.WARD_CODE = lstDocInfo[0].WardCode;
                        assessmentCycle.WARD_NAME = lstDocInfo[0].WardName;
                        shRet = assementCycleAccess.Insert(assessmentCycle);
                    }
                    else if (shRet == ServerData.ExecuteResult.OK)
                    {
                        assessmentCycle.CREATOR_ID = lstDocInfo[0].CreatorID;
                        assessmentCycle.CREATOR_NAME = lstDocInfo[0].CreatorName;
                        assessmentCycle.CYCLE = cycle;
                        assessmentCycle.DATA = summaryData.DataName;
                        assessmentCycle.DOCTYPE_ID = lstDocInfo[0].DocTypeID;
                        assessmentCycle.DOCTYPE_NAME = lstDocInfo[0].DocTitle;
                        assessmentCycle.DOC_ID = lstDocInfo[0].DocSetID;
                        assessmentCycle.PATIENT_ID = lstDocInfo[0].PatientID;
                        assessmentCycle.RECORD_TIME = lstDocInfo[0].RecordTime;
                        assessmentCycle.UNIT = summaryData.DataUnit;
                        assessmentCycle.VISIT_ID = lstDocInfo[0].VisitID;
                        assessmentCycle.WARD_CODE = lstDocInfo[0].WardCode;
                        assessmentCycle.WARD_NAME = lstDocInfo[0].WardName;
                        shRet = assementCycleAccess.Update(assessmentCycle);
                    }
                }
                else if (summaryData.Category == 5)
                {
                    short shRet = ServerData.ExecuteResult.OK;
                    OrdersWriteBack ordersWriteBack = new OrdersWriteBack();
                    //keydata����ʱ�й̶���ʽ
                    //KeyData(dataName, "","", status, 5, recordTime,"")
                    //KeyData(dataName, orderNo, orderSubNo, "", 5, recordTime,"")
                    ordersWriteBack.PatientID = summaryData.PatientID;
                    ordersWriteBack.VisitID = summaryData.VisitID;
                    ordersWriteBack.OrderNo = summaryData.DataValue;
                    ordersWriteBack.OrderSubNo = summaryData.DataType;
                    ordersWriteBack.RecordTime = summaryData.DataTime;
                    ordersWriteBack.RecordID = summaryData.RecordID;
                    ordersWriteBack.DataName = summaryData.DataName;
                    ordersWriteBack.Status = summaryData.DataUnit;
                    ordersWriteBack.DocID = summaryData.DocID;
                    shRet = AddOrdersWriteBack(ordersWriteBack);
                    if (shRet != ServerData.ExecuteResult.OK)
                        return shRet;
                }
            }
            base.DataAccess.CommitTransaction(true);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ��ȡĳ����ָ����ժҪ���� 
        /// </summary>
        /// <param name="szPatientID">����ID��</param>
        /// <param name="szVisitID">����ID��</param>
        /// <param name="szDataName">����ID��</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryDatas(string szPatientID, string szVisitID, string szDataName, ref List<SummaryData> lstSummaryData)
        {
            return this.GetSummaryDatas(szPatientID, szVisitID, szDataName, true, ref lstSummaryData);
            //if (base.DataAccess == null)
            //    return ServerData.ExecuteResult.PARAM_ERROR;

            //string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
            //    , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
            //    , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
            //    , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
            //    , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
            //    , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
            //    , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);

            //string szCondition = string.Format("{0}='{1}' AND {2}='{3}' "
            //    , ServerData.SummaryDataTable.PATIENT_ID, szPatientID
            //    , ServerData.SummaryDataTable.VISIT_ID, szVisitID);

            //if (!GlobalMethods.Misc.IsEmptyString(szDataName))
            //{
            //    szCondition = string.Format("{0} AND {1} in ({2})", szCondition
            //        , ServerData.SummaryDataTable.DATA_NAME, szDataName);
            //}

            //string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
            //    , ServerData.DataTable.DOC_SUMMARY_DATA, szCondition, ServerData.SummaryDataTable.DATA_TIME);

            //IDataReader dataReader = null;
            //try
            //{
            //    dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
            //    if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
            //        return ServerData.ExecuteResult.RES_NO_FOUND;

            //    if (lstSummaryData == null)
            //        lstSummaryData = new List<SummaryData>();
            //    do
            //    {
            //        SummaryData summaryData = new SummaryData();
            //        summaryData.DocID = dataReader.GetString(0);
            //        if (!dataReader.IsDBNull(1))
            //            summaryData.DataName = dataReader.GetString(1);
            //        if (!dataReader.IsDBNull(2))
            //            summaryData.DataValue = dataReader.GetString(2);
            //        if (!dataReader.IsDBNull(3))
            //            summaryData.DataType = dataReader.GetString(3);
            //        if (!dataReader.IsDBNull(4))
            //            summaryData.DataUnit = dataReader.GetString(4);
            //        if (!dataReader.IsDBNull(5))
            //            summaryData.DataTime = dataReader.GetDateTime(5);
            //        if (!dataReader.IsDBNull(6))
            //            summaryData.PatientID = dataReader.GetString(6);
            //        if (!dataReader.IsDBNull(7))
            //            summaryData.VisitID = dataReader.GetString(7);
            //        if (!dataReader.IsDBNull(8))
            //            summaryData.SubID = dataReader.GetString(8);
            //        if (!dataReader.IsDBNull(9))
            //            summaryData.WardCode = dataReader.GetString(9);
            //        if (!dataReader.IsDBNull(10))
            //            summaryData.RecordID = dataReader.GetString(10);
            //        if (!dataReader.IsDBNull(11))
            //            summaryData.DocTypeID = dataReader.GetString(11);
            //        lstSummaryData.Add(summaryData);
            //    } while (dataReader.Read());
            //    return ServerData.ExecuteResult.OK;
            //}
            //catch (Exception ex)
            //{
            //    return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            //}
            //finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡĳ����ָ����ժҪ���� 
        /// </summary>
        /// <param name="szPatientID">����ID��</param>
        /// <param name="szVisitID">����ID��</param>
        /// <param name="szDataName">����ID��</param>
        /// <param name="basc">�Ƿ�����</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryDatas(string szPatientID, string szVisitID, string szDataName, bool basc, ref List<SummaryData> lstSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' "
                , ServerData.SummaryDataTable.PATIENT_ID, szPatientID
                , ServerData.SummaryDataTable.VISIT_ID, szVisitID);

            if (!GlobalMethods.Misc.IsEmptyString(szDataName))
            {
                szCondition = string.Format("{0} AND {1} in ({2})", szCondition
                    , ServerData.SummaryDataTable.DATA_NAME, szDataName);
            }

            string szSQL = string.Empty;
            if (basc)
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                     , ServerData.DataTable.DOC_SUMMARY_DATA, szCondition, ServerData.SummaryDataTable.DATA_TIME);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField
                 , ServerData.DataTable.DOC_SUMMARY_DATA, szCondition, ServerData.SummaryDataTable.DATA_TIME);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstSummaryData == null)
                    lstSummaryData = new List<SummaryData>();
                do
                {
                    SummaryData summaryData = new SummaryData();
                    summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1))
                        summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        summaryData.DataValue = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        summaryData.DataType = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4))
                        summaryData.DataUnit = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5))
                        summaryData.DataTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6))
                        summaryData.PatientID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        summaryData.VisitID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8))
                        summaryData.SubID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9))
                        summaryData.WardCode = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10))
                        summaryData.RecordID = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11))
                        summaryData.DocTypeID = dataReader.GetString(11);
                    lstSummaryData.Add(summaryData);
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
        /// ��ȡ����������ͼ�����ժҪ����  YH
        /// </summary>
        /// <param name="szWardCode">��������</param>
        /// <param name="szDataNames">ժҪ����</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryDatas(string szWardCode, string szDataNames, ref List<SummaryData> lstSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);

            string szCondition = string.Format("{0}='{1}' AND {2} in ({3})"
                , ServerData.SummaryDataTable.WARD_CODE, szWardCode
                , ServerData.SummaryDataTable.DATA_NAME, szDataNames);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField
                , ServerData.DataTable.DOC_SUMMARY_DATA, szCondition, "  patient_id,visit_id, doctype_id, data_time desc, data_name ");

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstSummaryData == null)
                    lstSummaryData = new List<SummaryData>();
                do
                {
                    SummaryData summaryData = new SummaryData();
                    summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1))
                        summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        summaryData.DataValue = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        summaryData.DataType = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4))
                        summaryData.DataUnit = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5))
                        summaryData.DataTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6))
                        summaryData.PatientID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        summaryData.VisitID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8))
                        summaryData.SubID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9))
                        summaryData.WardCode = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10))
                        summaryData.RecordID = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11))
                        summaryData.DocTypeID = dataReader.GetString(11);
                    lstSummaryData.Add(summaryData);
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
        /// ��ȡĳ����ָ���ĵ���ID����ָ����ժҪ����  HJX
        /// </summary>
        /// <param name="szPatientID">����ID��</param>
        /// <param name="szVisitID">����ID��</param>
        /// <param name="szDocID">�ĵ���ID��</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, string szDocID, ref List<SummaryData> lstSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'"
                , ServerData.SummaryDataTable.PATIENT_ID, szPatientID
                , ServerData.SummaryDataTable.VISIT_ID, szVisitID, ServerData.SummaryDataTable.DOC_ID, szDocID);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition, ServerData.SummaryDataTable.DATA_TIME);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstSummaryData == null)
                    lstSummaryData = new List<SummaryData>();
                do
                {
                    SummaryData summaryData = new SummaryData();
                    summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1))
                        summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        summaryData.DataValue = dataReader.GetString(2);
                    else
                        summaryData.DataValue = dataReader.GetValue(2).ToString();
                    if (!dataReader.IsDBNull(3))
                        summaryData.DataType = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4))
                        summaryData.DataUnit = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5))
                        summaryData.DataTime = dataReader.GetDateTime(5);
                    if (!dataReader.IsDBNull(6))
                        summaryData.PatientID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        summaryData.VisitID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8))
                        summaryData.SubID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9))
                        summaryData.WardCode = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10))
                        summaryData.RecordID = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11))
                        summaryData.DocTypeID = dataReader.GetString(11);
                    lstSummaryData.Add(summaryData);
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
        /// ��ȡָ���ĵ���ժҪ�����б�
        /// </summary>
        /// <param name="szDocIDOrRecordID">�ĵ����</param>
        /// <param name="bIsRecordID">�Ƿ��Ǽ�¼ID</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szDocIDOrRecordID, bool bIsRecordID, ref List<SummaryData> lstSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_CODE
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);

            string szCondition = null;
            if (bIsRecordID)
                szCondition = string.Format("{0}='{1}'", ServerData.SummaryDataTable.RECORD_ID, szDocIDOrRecordID);
            else
                szCondition = string.Format("{0}='{1}'", ServerData.SummaryDataTable.DOC_ID, szDocIDOrRecordID);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstSummaryData == null)
                    lstSummaryData = new List<SummaryData>();
                do
                {
                    SummaryData summaryData = new SummaryData();
                    if (!dataReader.IsDBNull(0)) summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) summaryData.DataCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) summaryData.DataValue = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) summaryData.DataType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) summaryData.DataUnit = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) summaryData.DataTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) summaryData.PatientID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) summaryData.VisitID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) summaryData.SubID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) summaryData.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) summaryData.RecordID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) summaryData.DocTypeID = dataReader.GetString(12);
                    if (!lstSummaryData.Contains(summaryData))
                    {
                        lstSummaryData.Add(summaryData);
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
        /// ��ȡָ�����˵Ļ����¼�ĵ�ժҪ�����б�
        /// </summary>
        /// <param name="szPatientID">����ID��</param>
        /// <param name="szVisitID">����ID��</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, DateTime dtBegin, DateTime dtEnd, ref List<SummaryData> lstSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_CODE
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);

            //string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}!=' ' AND {4} IS NOT NULL"
            //    , ServerData.SummaryDataTable.PATIENT_ID, szPatientID
            //    , ServerData.SummaryDataTable.VISIT_ID, szVisitID, ServerData.SummaryDataTable.RECORD_ID);
            string szCondition = string.Format("{0}={1} AND {2}={3} AND {4}!=' ' AND {4} IS NOT NULL"
                , ServerData.SummaryDataTable.PATIENT_ID, base.DataAccess.GetSqlParamName("PATIENT_ID")
                , ServerData.SummaryDataTable.VISIT_ID, base.DataAccess.GetSqlParamName("VISIT_ID")
                , ServerData.SummaryDataTable.RECORD_ID);
            if (dtEnd != null)
            {
                szCondition = string.Format("{0} AND {1} >= {2}", szCondition, ServerData.SummaryDataTable.DATA_TIME, base.DataAccess.GetSqlTimeFormat(dtBegin));
            }
            if (dtBegin != null)
            {
                szCondition = string.Format("{0} AND {1} <= {2}", szCondition, ServerData.SummaryDataTable.DATA_TIME, base.DataAccess.GetSqlTimeFormat(dtEnd));
            }

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition);
            DbParameter[] pmi = new DbParameter[2];
            pmi[0] = new DbParameter("PATIENT_ID", szPatientID);
            pmi[1] = new DbParameter("VISIT_ID", szVisitID);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text, ref pmi);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstSummaryData == null)
                    lstSummaryData = new List<SummaryData>();
                do
                {
                    SummaryData summaryData = new SummaryData();
                    if (!dataReader.IsDBNull(0)) summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) summaryData.DataCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) summaryData.DataValue = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) summaryData.DataType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) summaryData.DataUnit = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) summaryData.DataTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) summaryData.PatientID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) summaryData.VisitID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) summaryData.SubID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) summaryData.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) summaryData.RecordID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) summaryData.DocTypeID = dataReader.GetString(12);
                    lstSummaryData.Add(summaryData);
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
        public short GetSummaryData(string szDocSetID, string szDataName, ref SummaryData summaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_CODE
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.SummaryDataTable.DOC_ID, szDocSetID
                , ServerData.SummaryDataTable.DATA_NAME, szDataName);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                summaryData = new SummaryData();
                do
                {
                    if (!dataReader.IsDBNull(0)) summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) summaryData.DataCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) summaryData.DataValue = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) summaryData.DataType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) summaryData.DataUnit = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) summaryData.DataTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) summaryData.PatientID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) summaryData.VisitID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) summaryData.SubID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) summaryData.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) summaryData.RecordID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) summaryData.DocTypeID = dataReader.GetString(12);
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
        /// ��ȡָ����������ժҪ�����б�
        /// </summary>
        /// <param name="szPatientID">����ID��</param>
        /// <param name="szVisitID">���˾����</param>
        /// <param name="szSubID">��ID��</param>
        /// <param name="dtBeginTime">��ʼʱ��</param>
        /// <param name="dtEndTime">����ʱ��</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<SummaryData> lstSummaryData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetSummaryData"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "���ݲ���Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstSummaryData == null)
                lstSummaryData = new List<SummaryData>();
            lstSummaryData.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_CODE
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);
            string szTable = ServerData.DataTable.DOC_SUMMARY_DATA;

            string szCondition = null;
            if (dtBeginTime == dtEndTime)
            {
                szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}>={7} AND {6}<={8}"
                     , ServerData.SummaryDataTable.PATIENT_ID, szPatientID
                     , ServerData.SummaryDataTable.VISIT_ID, szVisitID
                     , ServerData.SummaryDataTable.SUB_ID, szSubID
                     , ServerData.SummaryDataTable.DATA_TIME
                     , base.DataAccess.GetSqlTimeFormat(dtBeginTime), base.DataAccess.GetSqlTimeFormat(dtEndTime));
            }
            else
            {
                szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}>={7} AND {6}<{8}"
                    , ServerData.SummaryDataTable.PATIENT_ID, szPatientID
                    , ServerData.SummaryDataTable.VISIT_ID, szVisitID
                    , ServerData.SummaryDataTable.SUB_ID, szSubID
                    , ServerData.SummaryDataTable.DATA_TIME
                    , base.DataAccess.GetSqlTimeFormat(dtBeginTime), base.DataAccess.GetSqlTimeFormat(dtEndTime));
            }

            string szOrder = ServerData.SummaryDataTable.DATA_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    SummaryData summaryData = new SummaryData();
                    if (!dataReader.IsDBNull(0)) summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) summaryData.DataCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) summaryData.DataValue = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) summaryData.DataType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) summaryData.DataUnit = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) summaryData.DataTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) summaryData.PatientID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) summaryData.VisitID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) summaryData.SubID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) summaryData.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) summaryData.RecordID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) summaryData.DocTypeID = dataReader.GetString(12);
                    lstSummaryData.Add(summaryData);
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
        /// ��ȡָ������ָ���ĵ������µ�ͬժҪ������������
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">���ʴ�</param>
        /// <param name="szDocTypeId">�ĵ�����Id</param>
        /// <param name="szDataName">ժҪ��</param>
        /// <param name="lstSummaryData">ժҪ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, string szDocTypeId, string szDataName, ref List<SummaryData> lstSummaryData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID)
                || GlobalMethods.Misc.IsEmptyString(szDocTypeId) || GlobalMethods.Misc.IsEmptyString(szDataName))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetSummaryData"
                    , new string[] { "szPatientID", "szVisitID", "szDocTypeId", "szDataName" }
                    , new object[] { szPatientID, szVisitID, szDocTypeId, szDataName }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstSummaryData == null)
                lstSummaryData = new List<SummaryData>();
            lstSummaryData.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.SummaryDataTable.DOC_ID, ServerData.SummaryDataTable.DATA_NAME
                , ServerData.SummaryDataTable.DATA_CODE
                , ServerData.SummaryDataTable.DATA_VALUE, ServerData.SummaryDataTable.DATA_TYPE
                , ServerData.SummaryDataTable.DATA_UNIT, ServerData.SummaryDataTable.DATA_TIME
                , ServerData.SummaryDataTable.PATIENT_ID, ServerData.SummaryDataTable.VISIT_ID
                , ServerData.SummaryDataTable.SUB_ID, ServerData.SummaryDataTable.WARD_CODE
                , ServerData.SummaryDataTable.RECORD_ID, ServerData.SummaryDataTable.DOCTYPE_ID);
            string szTable = ServerData.DataTable.DOC_SUMMARY_DATA;

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='{7}'"
                     , ServerData.SummaryDataTable.PATIENT_ID, szPatientID
                     , ServerData.SummaryDataTable.VISIT_ID, szVisitID
                     , ServerData.SummaryDataTable.DOCTYPE_ID, szDocTypeId
                     , ServerData.SummaryDataTable.DATA_NAME, szDataName);

            string szOrder = ServerData.SummaryDataTable.DATA_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    SummaryData summaryData = new SummaryData();
                    if (!dataReader.IsDBNull(0)) summaryData.DocID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) summaryData.DataName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) summaryData.DataCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) summaryData.DataValue = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) summaryData.DataType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) summaryData.DataUnit = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) summaryData.DataTime = dataReader.GetDateTime(6);
                    if (!dataReader.IsDBNull(7)) summaryData.PatientID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) summaryData.VisitID = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) summaryData.SubID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) summaryData.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) summaryData.RecordID = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) summaryData.DocTypeID = dataReader.GetString(12);
                    lstSummaryData.Add(summaryData);
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
        /// ɾ��ָ�����ĵ�ID���������ĵ�ժҪ����
        /// </summary>
        /// <param name="szDocSetID">�����ĵ���ID</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteSummaryData(string szDocSetID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.DeleteSummaryData"
                    , new string[] { "szDocSetID" }, new object[] { szDocSetID }, "�ĵ������ݲ���Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.SummaryDataTable.DOC_ID, szDocSetID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition);
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
                LogManager.Instance.WriteLog("DocumentAccess.DeleteDocData"
                    , new string[] { "szDocSetID" }, new object[] { szDocSetID }, "�ĵ������ݲ���Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //string szSQL = string.Format(" delete from (select 1 from {0} t, {1} t1 where t.doc_id = t1.doc_id and t.doc_setid = '{2}' )"
            //    , ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS
            //    , szDocSetID);
            //string szSQL = string.Format(" delete from {1} where exists (select 1 "
            //    + " from {0} t1, {1} t2 where t1.{4} = '{3}' and t1.{5} = t2.{6}); "
            //    + " delete from {2} where exists (select 1 from {2} t3 where t3.{7} = '{3}'); "
            //    + " delete from {0} where exists (select 1 from {0} t1 where t1.{4} = '{3}'); "
            //    , ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS
            //    , ServerData.DataTable.DOC_SUMMARY_DATA, szDocSetID
            //    , ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_ID
            //    , ServerData.DocStatusTable.DOC_ID, ServerData.SummaryDataTable.DOC_ID);

            //ɾ��״̬ 
            string szSQL1 = string.Format(" delete from {1} t2 where exists (select 1 from {0} t1 where t1.{2} = '{5}' and t1.{3} = t2.{4})"
            , ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS
            , ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_ID
            , ServerData.DocStatusTable.DOC_ID, szDocSetID);
            //ɾ���ؼ�����
            string szSQL2 = string.Format(" delete from {0} where {1} = '{2}' "
            , ServerData.DataTable.DOC_SUMMARY_DATA, ServerData.SummaryDataTable.DOC_ID, szDocSetID);
            //ɾ������������
            string szSQL3 = string.Format(" delete from {0} where  {1} = '{2}'"
            , ServerData.DataTable.NUR_DOC, ServerData.NurDocTable.DOC_SETID, szDocSetID);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL1, CommandType.Text);
                base.DataAccess.ExecuteNonQuery(szSQL2, CommandType.Text);
                base.DataAccess.ExecuteNonQuery(szSQL3, CommandType.Text);
                //base.DataAccess.
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL1, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        /// <summary>
        /// ����һ��ҽ����д��¼
        /// </summary>
        /// <param name="ordersWriteBack">ҽ����д��Ϣ</param>
        /// <returns></returns>
        private short AddOrdersWriteBack(OrdersWriteBack ordersWriteBack)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            if (ordersWriteBack == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}"
                , ServerData.OrdersWriteBackDataTable.PATIENT_ID, ServerData.OrdersWriteBackDataTable.VISIT_ID
                , ServerData.OrdersWriteBackDataTable.ORDER_NO, ServerData.OrdersWriteBackDataTable.ORDER_SUB_NO
                , ServerData.OrdersWriteBackDataTable.RECORD_TIME, ServerData.OrdersWriteBackDataTable.RECORD_ID
                , ServerData.OrdersWriteBackDataTable.DATA_NAME, ServerData.OrdersWriteBackDataTable.STATUS
                , ServerData.OrdersWriteBackDataTable.DOC_ID);

            string szValue = string.Format("'{0}','{1}','{2}','{3}',to_date('{4}','yyyy-MM-dd hh24:mi:ss'),'{5}','{6}','{7}','{8}'"
                , ordersWriteBack.PatientID, ordersWriteBack.VisitID
                , ordersWriteBack.OrderNo, ordersWriteBack.OrderSubNo
                , ordersWriteBack.RecordTime, ordersWriteBack.RecordID
                , ordersWriteBack.DataName, ordersWriteBack.Status
                , ordersWriteBack.DocID);

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.ORDERS_WRITE_BACK
                , szField, szValue);
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                if (base.DataAccess.IsConstraintConflictExpception(ex))
                {
                    if (this.UpdateOrdersWriteBack(ordersWriteBack) == ServerData.ExecuteResult.OK)
                        return ServerData.ExecuteResult.OK;
                }
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.AddOrdersWriteBack", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ����һ��ҽ����д��¼
        /// </summary>
        /// <param name="ordersWriteBack">ҽ����д��Ϣ</param>
        /// <returns></returns>
        private short UpdateOrdersWriteBack(OrdersWriteBack ordersWriteBack)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            if (ordersWriteBack == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0} = to_date('{1}','yyyy-MM-dd hh24:mi:ss'), {2} = '{3}'"
                , ServerData.OrdersWriteBackDataTable.RECORD_TIME, ordersWriteBack.RecordTime
                , ServerData.OrdersWriteBackDataTable.STATUS, ordersWriteBack.Status);

            string szCondition = string.Format("{0}='{1}' and {2}='{3}' and {4}='{5}' and {6}=to_date('{7}','yyyy-MM-dd hh24:mi:ss')"
                , ServerData.OrdersWriteBackDataTable.PATIENT_ID, ordersWriteBack.PatientID
                , ServerData.OrdersWriteBackDataTable.VISIT_ID, ordersWriteBack.VisitID
                , ServerData.OrdersWriteBackDataTable.DATA_NAME, ordersWriteBack.DataName
                , ServerData.OrdersWriteBackDataTable.RECORD_TIME, ordersWriteBack.RecordTime);

            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.ORDERS_WRITE_BACK
                , szField, szCondition);
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
                LogManager.Instance.WriteLog("DocumentAccess.UpdateOrdersWriteBack", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
    }
}
