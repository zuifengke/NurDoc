// ***********************************************************
// 数据库访问层与病历文档数据及索引有关的数据的访问类.
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
        #region"读写文档索引接口"
        /// <summary>
        /// 获取指定就诊，指定病人，指定类型中已有文档详细信息列表.
        /// 注意：该接口不对返回病历列表进行排序,需要外部主动调用Sort方法排序
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="szDocTypeID">文档类型代码,为空时返回当次就诊下所有类型的文档</param>
        /// <param name="lstDocInfos">文档信息列表</param>
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

            //创建用来确定一次就诊的查询条件
            string szCondition = string.Format("A.{0}={1} AND A.{2}={3}"
                    , ServerData.NurDocTable.PATIENT_ID, base.DataAccess.GetSqlParamName("PATIENT_ID")

                    , ServerData.NurDocTable.VISIT_ID, base.DataAccess.GetSqlParamName("VISIT_ID"));

            //过滤掉已作废的文档
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);

            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                , ServerData.NurDocTable.DOC_TYPE_ID, base.DataAccess.GetSqlParamName("DOC_TYPE_ID"));

            //如果传入了起始截止时间,则仅获取这个时间段内创建的
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            //return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取指定就诊，指定病人，指定类型中已有文档详细信息列表.       HJX
        /// 注意：该接口不对返回病历列表进行排序,需要外部主动调用Sort方法排序
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="szDocTypeID">文档类型代码,为空时返回当次就诊下所有类型的文档</param>
        /// <param name="dtBeginTime">查询起始创建时间</param>
        /// <param name="dtEndTime">查询截止创建时间</param>
        /// <param name="lstDocInfos">文档信息列表</param>
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

            //创建用来确定一次就诊的查询条件
            string szCondition = string.Format("A.{0}={1} AND A.{2}={3}"
                    , ServerData.NurDocTable.PATIENT_ID, base.DataAccess.GetSqlParamName("PATIENT_ID")

                    , ServerData.NurDocTable.VISIT_ID, base.DataAccess.GetSqlParamName("VISIT_ID"));

            //过滤掉已作废的文档
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                 , ServerData.NurDocTable.DOC_TYPE_ID, szDocTypeID);

            //如果传入了起始截止时间,则仅获取这个时间段内创建的

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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定就诊，指定病人，指定类型中已有文档详细信息列表.
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="szDocTypeID">文档类型代码,为空时返回当次就诊下所有类型的文档</param>
        /// <param name="dtBeginTime">查询起始创建时间</param>
        /// <param name="dtEndTime">查询截止创建时间</param>
        /// <param name="lstDocInfos">文档信息列表</param>
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

            //创建用来确定一次就诊的查询条件
            string szCondition = string.Format("A.{0}={1} AND A.{2}={3}"
                    , ServerData.NurDocTable.PATIENT_ID, base.DataAccess.GetSqlParamName("PATIENT_ID")

                    , ServerData.NurDocTable.VISIT_ID, base.DataAccess.GetSqlParamName("VISIT_ID"));

            //过滤掉已作废的文档
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);
            szCondition = string.Format("{0} AND A.{1} in ({2})", szCondition
                 , ServerData.NurDocTable.DOC_TYPE_ID, szDocTypeID);

            //如果传入了起始截止时间,则仅获取这个时间段内创建的

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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 注意：该接口返回评估视图分析所需的文档列表
        /// </summary>
        /// <param name="szWardCode">当前科室代码</param>
        /// <param name="szDocTypeID">文档类型代码,为空时返回当次就诊下所有类型的文档</param>
        /// <param name="lstDocInfos">文档信息列表</param>
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

            //创建用来确定一次就诊的查询条件
            string szCondition = string.Format("1=1");

            //过滤掉已作废的文档
            szCondition = string.Format("{0} AND A.{1}=B.{2} AND B.{3}!='{4}'", szCondition
                , ServerData.NurDocTable.DOC_ID, ServerData.DocStatusTable.DOC_ID
                , ServerData.DocStatusTable.DOC_STATUS, ServerData.DocStatus.CANCELED);

            //如果传入了文档类型,则仅获取该文档类型对应的文档列表
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定护理记录ID对应的护理病历列表
        /// </summary>
        /// <param name="szRecordID">护理记录ID</param>
        /// <param name="lstDocInfos">文档信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetRecordDocInfos(string szRecordID, ref NurDocList lstDocInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szRecordID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetRecordDocInfos"
                    , new string[] { "szRecordID" }, new object[] { szRecordID }, "参数不能为空");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据文档ID，获取文档基本信息
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="docInfo">文档信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfo(string szDocID, ref NurDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetDocInfo", new string[] { "szDocID" }, new object[] { szDocID }, "参数不能为空");
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
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "未查询到记录");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 新增文档索引信息
        /// </summary>
        /// <param name="docInfo">文档索引信息类</param>
        /// <param name="byteDocData">文档数据</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.AddDocIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改文档索引信息
        /// </summary>
        /// <param name="docInfo">文档索引信息类</param>
        /// <param name="byteDocData">文档数据</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocIndexInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 得到当前就诊条件下指定文档类型的最新版本的未作废的文档
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="szVisitType">就诊类型</param>
        /// <param name="dtVisitTime">就诊时间</param>
        /// <param name="szDocTypeID">文档类型编号</param>
        /// <param name="docInfo">文档信息</param>
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

            //创建用来确定一次就诊的查询条件
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定文档集中最新的文档版本信息
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <param name="docInfo">文档信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetLatestDocInfo(string szDocSetID, ref NurDocInfo docInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetLatestDocInfo", new string[] { "szDocSetID" }
                    , new object[] { szDocSetID }, "参数不能为空!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        ///  根据文档集ID查询当前文档集对应的历次修订的病历记录
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <param name="lstDocInfo">病历文档列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocInfoBySetID(string szDocSetID, ref List<NurDocInfo> lstDocInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetDocInfoBySetID", new string[] { "szDocSetID" }
                    , new object[] { szDocSetID }, "参数不能为空");
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
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocInfoBySetID", new string[] { "szSQL" }, new object[] { szSQL }, "未查询到记录");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据文档编号，修改文档标题
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="szDocTitle">文档标题</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyDocTitle(string szDocID, string szDocTitle)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocTitle", new string[] { "szDocID" }, new object[] { szDocID }, "文档ID不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (GlobalMethods.Misc.IsEmptyString(szDocTitle))
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocTitle", new string[] { "szDocTitle" }, new object[] { szDocTitle }, "文档标题不能为空!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocTitle", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"读写文档状态接口"
        /// <summary>
        /// 获取指定文档状态信息
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="docStatusInfo">文档状态信息类</param>
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
                        , new object[] { szSQL }, "没有查询到记录!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 设置指定文档状态信息
        /// </summary>
        /// <param name="newStatusInfo">文档状态信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SetDocStatusInfo(ref DocStatusInfo newStatusInfo)
        {
            if (newStatusInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", "参数不能为null!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            DocStatusInfo currDocStatus = null;
            short shRet = this.GetDocStatusInfo(newStatusInfo.DocID, ref currDocStatus);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                newStatusInfo.StatusMessage = "查询服务器文档状态失败";
                return shRet;
            }
            if (currDocStatus.DocStatus == ServerData.DocStatus.CANCELED)
            {
                newStatusInfo.StatusMessage = string.Format(
                    ServerData.DocStatus.CANCELED_STATUS_DESC,
                    currDocStatus.OperatorName,
                    currDocStatus.OperateTime.ToString("yyyy年M月d日 HH:mm"));
                LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
                return ServerData.ExecuteResult.RES_IS_EXIST;
            }
            else if (currDocStatus.DocStatus == ServerData.DocStatus.ARCHIVED)
            {
                newStatusInfo.StatusMessage = string.Format(
                    ServerData.DocStatus.ARCHIVED_STATUS_DESC,
                    currDocStatus.OperatorName,
                    currDocStatus.OperateTime.ToString("yyyy年M月d日 HH:mm"));
                LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
                return ServerData.ExecuteResult.OTHER_ERROR;
            }
            else if (currDocStatus.DocStatus == ServerData.DocStatus.LOCKED)
            {
                if (currDocStatus.OperatorID == newStatusInfo.OperatorID)
                {
                    newStatusInfo.StatusMessage = string.Format(
                        ServerData.DocStatus.LOCKED_STATUS_DESC1,
                        currDocStatus.OperateTime.ToString("yyyy年M月d日 HH:mm"));
                }
                else
                {
                    newStatusInfo.StatusMessage = string.Format(
                        ServerData.DocStatus.LOCKED_STATUS_DESC2,
                        currDocStatus.OperatorName,
                        currDocStatus.OperateTime.ToString("yyyy年M月d日 HH:mm"));
                    LogManager.Instance.WriteLog("DocumentAccess.SetDocStatusInfo", newStatusInfo.StatusMessage);
                    return ServerData.ExecuteResult.OTHER_ERROR;
                }
            }

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //修改文档状态
            shRet = this.ModifyDocStatusInfo(ref newStatusInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            //删除文档同时删除摘要数据
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
        /// 写文档状态记录
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        private short AddDocStatusInfo(DocStatusInfo docStatusInfo)
        {
            if (docStatusInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.AddDocStatusInfo", "参数不能为null!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!" + ex.Message);
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.AddDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改文档状态记录
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        private short ModifyDocStatusInfo(ref DocStatusInfo docStatusInfo)
        {
            if (docStatusInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", "参数不能为null!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改文档状态记录
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyOldDocStatusInfo(ref DocStatusInfo docStatusInfo)
        {
            if (docStatusInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", "参数不能为null!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.ModifyDocStatusInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        #endregion

        #region"获取文档内容接口"
        /// <summary>
        /// 根据文档ID获取文档内容
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="byteDocData">文档二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocByID(string szDocID, ref byte[] byteDocData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetDocByID"
                    , new string[] { "szDocID" }, new object[] { szDocID }, "文档ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetDocByID"
                    , new string[] { "szDocID" }, new object[] { szDocID }, "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.GetDocFromDB(szDocID, ref byteDocData);

            short shRet = this.GetDocFromFTP(szDocID, ref byteDocData);
            return shRet;
        }

        /// <summary>
        /// 根据文档ID从DB中获取文档内容
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="byteDocData">文档二进制内容</param>
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
                        , new string[] { "szSQL" }, new object[] { szSQL }, "没有查询到记录!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                return GlobalMethods.IO.GetBytes(dataReader, 0, ref byteDocData)
                    ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据文档ID从FTP中获取文档内容
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="byteDocData">文档二进制内容</param>
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
                    , new object[] { szCacheFile }, "GetFileBytes执行失败!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            base.FtpAccess.CloseConnection();
            GlobalMethods.IO.DeleteFile(szCacheFile);
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region"保存新文档接口"
        /// <summary>
        /// 保存新文档
        /// </summary>
        /// <param name="docInfo">文档信息</param>
        /// <param name="byteDocData">文档二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveDoc(NurDocInfo docInfo, byte[] byteDocData)
        {
            if (docInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "docInfo" }, new object[] { docInfo }, "文档信息类参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "docInfo" }, new object[] { docInfo }, "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.SaveDocToDB(docInfo, byteDocData);
            else
                return this.SaveDocToFTP(docInfo, byteDocData);
        }

        /// <summary>
        /// 保存新文档
        /// </summary>
        /// <param name="lstDocInfo">文档信息</param>
        /// <param name="htDocTypeData">文档二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveDoc(List<NurDocInfo> lstDocInfo, Hashtable htDocTypeData)
        {
            if (lstDocInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "lstDocInfo" }, new object[] { lstDocInfo }, "文档列表信息类参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (htDocTypeData == null)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "htDocTypeData" }, new object[] { htDocTypeData }, "文档信息类对应文档数据哈希表参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.SaveDoc"
                    , new string[] { "lstDocInfo" }, new object[] { lstDocInfo }, "配置字典表中文档存储模式配置不正确!");
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
        /// 把文档保存到数据库
        /// </summary>
        /// <param name="docInfo">文档信息</param>
        /// <param name="byteDocData">文档数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveDocToDB(NurDocInfo docInfo, byte[] byteDocData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //添加文档状态信息记录
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
            //保存文档索引信息记录,包括文档内容
            shRet = this.AddDocIndexInfo(docInfo, byteDocData);
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
        /// 把文档保存到FTP文档库
        /// </summary>
        /// <param name="docInfo">文档信息</param>
        /// <param name="byteDocData">文档数据</param>
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
                    , new object[] { szCacheFile }, "WriteFileBytes执行失败!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                string szRemoteFile = ServerParam.Instance.GetFtpDocPath(docInfo, ServerData.FileExt.NUR_DOCUMENT);
                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!base.FtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;

                if (!base.FtpAccess.Upload(szCacheFile, szRemoteFile))
                {
                    LogManager.Instance.WriteLog("DocumentAccess.Upload", new string[] { "szRemoteDir" }, new object[] { szRemoteDir }, "文档上传失败，返回异常!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                if (!base.FtpAccess.ResExists(szRemoteFile, false))
                {
                    LogManager.Instance.WriteLog("DocumentAccess.Upload", new string[] { "szRemoteDir" }, new object[] { szRemoteDir }, "文档上传失败，却返回成功!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), docInfo.ToString(), "文档上传保存失败!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
        }
        #endregion

        #region"更新已有文档接口"
        /// <summary>
        /// 更新已有文档
        /// </summary>
        /// <param name="szOldDocID">被更新的文档ID</param>
        /// <param name="newDocInfo">更新后的文档信息</param>
        /// <param name="szUpdateReason">病历更新原因描述</param>
        /// <param name="byteDocData">文档数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateDoc(string szOldDocID, NurDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("DocumentAccess.UpdateDoc", "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.UpdateDocToDB(szOldDocID, newDocInfo, szUpdateReason, byteDocData);
            else
                return this.UpdateDocToFTP(szOldDocID, newDocInfo, szUpdateReason, byteDocData);
        }

        /// <summary>
        /// 更新文档到数据库
        /// </summary>
        /// <param name="szOldDocID">被更新的文档ID</param>
        /// <param name="newDocInfo">更新后的文档信息</param>
        /// <param name="szUpdateReason">病历更新原因描述</param>
        /// <param name="byteDocData">文档数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short UpdateDocToDB(string szOldDocID, NurDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            if (newDocInfo == null || base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            DocStatusInfo docStatusInfo = new DocStatusInfo();

            //如果文档ID未变,那么覆盖已有文档
            if (szOldDocID == newDocInfo.DocID)
            {
                //添加文档状态信息记录
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

                //保存文档索引信息记录,包括文档内容
                shRet = this.ModifyDocIndexInfo(newDocInfo, byteDocData);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }
            }
            else
            {
                //修改旧的文档状态为作废
                docStatusInfo.DocID = szOldDocID;
                docStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
                short shRet = this.ModifyOldDocStatusInfo(ref docStatusInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }

                //添加文档状态信息记录
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

                //保存文档索引信息记录,包括文档内容
                shRet = this.AddDocIndexInfo(newDocInfo, byteDocData);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.DataAccess.AbortTransaction();
                    return shRet;
                }
            }
            //提交数据库更新
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新文档到FTP
        /// </summary>
        /// <param name="szOldDocID">被更新的文档ID</param>
        /// <param name="newDocInfo">更新后的文档信息</param>
        /// <param name="szUpdateReason">病历更新原因描述</param>
        /// <param name="byteDocData">文档数据</param>
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
                    , new object[] { szCacheFile }, "WriteFileBytes执行失败!");
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            try
            {
                string szRemoteFile = ServerParam.Instance.GetFtpDocPath(newDocInfo, ServerData.FileExt.NUR_DOCUMENT);
                string szRemoteFileBak = null;

                //如果需要覆盖,那么先把被覆盖的文件备份一次
                if (szOldDocID == newDocInfo.DocID)
                {
                    szRemoteFileBak = string.Format("{0}.bak", szRemoteFile);
                    if (!base.FtpAccess.RenameFile(szRemoteFile, szRemoteFileBak))
                        return ServerData.ExecuteResult.EXCEPTION;
                }

                if (!base.FtpAccess.Upload(szCacheFile, szRemoteFile))
                {
                    //还原备份的远程文件
                    if (!string.IsNullOrEmpty(szRemoteFileBak))
                        base.FtpAccess.RenameFile(szRemoteFileBak, szRemoteFile);
                    return ServerData.ExecuteResult.EXCEPTION;
                }

                short shRet = this.UpdateDocToDB(szOldDocID, newDocInfo, szUpdateReason, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.FtpAccess.DeleteFile(szRemoteFile);

                    //还原备份的远程文件
                    if (!string.IsNullOrEmpty(szRemoteFileBak))
                        base.FtpAccess.RenameFile(szRemoteFileBak, szRemoteFile);
                    return shRet;
                }

                //删除备份的远程文件
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

        #region"子文档关联信息接口"
        /// <summary>
        /// 保存或更新一条文档父子关系数据
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="szCaller">调用者</param>
        /// <param name="szChildID">子文档编号</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveChildDocInfo(string szDocID, string szCaller, string szChildID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //先删除之前的父子文档信息
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }

            //再插入新的父子文档信息
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            base.DataAccess.CommitTransaction(true);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取指定文档指定调用者的子文档ID号
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="szCaller">调用者</param>
        /// <param name="szChildID">返回的子文档编号</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }

        /// <summary>
        /// 获取指定文档指子文档编号序列
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="lstChilDocID">返回的子文档编号序列</param>
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
                    LogManager.Instance.WriteLog("DocumentAccess.GetDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "未查询到记录");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定文档子文档信息列表
        /// </summary>
        /// <param name="szDocSetID">文档编号</param>
        /// <param name="lstChilDocs">返回的子文档信息列表</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion

        #region"与文档有关的摘要数据"
        /// <summary>
        /// 保存或更新一系列摘要数据 HJX
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="lstSummaryData">子文档编号</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveSummaryData(string szDocID, List<SummaryData> lstSummaryData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            List<NurDocInfo> lstDocInfo = new List<NurDocInfo>();
            this.GetDocInfoBySetID(szDocID, ref lstDocInfo);
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //先删除之前的文档摘要数据
            string szCondition = string.Format("{0}='{1}'", ServerData.SummaryDataTable.DOC_ID, szDocID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.DOC_SUMMARY_DATA, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行异常!");
            }

            if (lstSummaryData == null)
                lstSummaryData = new List<SummaryData>();

            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData.Category == 5)
                {
                    //先删除之前的医嘱回写数据
                    szCondition = string.Format("{0} = '{1}'", ServerData.OrdersWriteBackDataTable.DOC_ID, szDocID);
                    szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.ORDERS_WRITE_BACK, szCondition);
                    try
                    {
                        base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                    }
                    catch (Exception ex)
                    {
                        base.DataAccess.AbortTransaction();
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行异常!");
                    }
                    break;
                }
            }
            //再插入新的文档摘要数据
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

                    //摘要数据最大长度为2000
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
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
                    }
                }

                //摘要数据中Category大于等于1且小于等于2,是体征数据
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
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "体征数据在存储过程保存执行失败!");
                    }
                }
                //保存过敏药物
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
                        return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "过敏药物在存储过程保存执行失败!");
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
                    //keydata保存时有固定格式
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
        /// 获取某患者指定的摘要数据 
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="szDataName">就诊ID号</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
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
            //    return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            //}
            //finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取某患者指定的摘要数据 
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="szDataName">就诊ID号</param>
        /// <param name="basc">是否升序</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取分析评估视图所需的摘要数据  YH
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="szDataNames">摘要名称</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取某患者指定文档集ID号下指定的摘要数据  HJX
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="szDocID">文档集ID号</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定文档的摘要数据列表
        /// </summary>
        /// <param name="szDocIDOrRecordID">文档编号</param>
        /// <param name="bIsRecordID">是否是记录ID</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定病人的护理记录文档摘要数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定文档集ID号下指定的摘要数据
        /// </summary>
        /// <param name="szDocSetID">文档集ID号</param>
        /// <param name="szDataName">就诊ID号</param>
        /// <param name="summaryData">返回的摘要数据</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定病人所有摘要数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">病人就诊号</param>
        /// <param name="szSubID">子ID号</param>
        /// <param name="dtBeginTime">起始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<SummaryData> lstSummaryData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetSummaryData"
                    , new string[] { "szPatientID", "szVisitID" }, new object[] { szPatientID, szVisitID }, "数据不能为空!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定病人指定文档类型下的同摘要名的所以数据
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">访问次</param>
        /// <param name="szDocTypeId">文档类型Id</param>
        /// <param name="szDataName">摘要名</param>
        /// <param name="lstSummaryData">摘要列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, string szDocTypeId, string szDataName, ref List<SummaryData> lstSummaryData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID)
                || GlobalMethods.Misc.IsEmptyString(szDocTypeId) || GlobalMethods.Misc.IsEmptyString(szDataName))
            {
                LogManager.Instance.WriteLog("DocumentAccess.GetSummaryData"
                    , new string[] { "szPatientID", "szVisitID", "szDocTypeId", "szDataName" }
                    , new object[] { szPatientID, szVisitID, szDocTypeId, szDataName }, "参数不能为空!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 删除指定的文档ID所包含的文档摘要数据
        /// </summary>
        /// <param name="szDocSetID">护理文档集ID</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteSummaryData(string szDocSetID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.DeleteSummaryData"
                    , new string[] { "szDocSetID" }, new object[] { szDocSetID }, "文档集数据不能为空!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            //finally { base.DataAccess.CloseConnnection(false); }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="szDocSetID">护理文档集</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteDocData(string szDocSetID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentAccess.DeleteDocData"
                    , new string[] { "szDocSetID" }, new object[] { szDocSetID }, "文档集数据不能为空!");
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

            //删除状态 
            string szSQL1 = string.Format(" delete from {1} t2 where exists (select 1 from {0} t1 where t1.{2} = '{5}' and t1.{3} = t2.{4})"
            , ServerData.DataTable.NUR_DOC, ServerData.DataTable.DOC_STATUS
            , ServerData.NurDocTable.DOC_SETID, ServerData.NurDocTable.DOC_ID
            , ServerData.DocStatusTable.DOC_ID, szDocSetID);
            //删除关键数据
            string szSQL2 = string.Format(" delete from {0} where {1} = '{2}' "
            , ServerData.DataTable.DOC_SUMMARY_DATA, ServerData.SummaryDataTable.DOC_ID, szDocSetID);
            //删除索引表数据
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL1, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        /// <summary>
        /// 新增一条医嘱回写记录
        /// </summary>
        /// <param name="ordersWriteBack">医嘱回写信息</param>
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
                LogManager.Instance.WriteLog("DocumentAccess.AddOrdersWriteBack", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新一条医嘱回写记录
        /// </summary>
        /// <param name="ordersWriteBack">医嘱回写信息</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocumentAccess.UpdateOrdersWriteBack", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
    }
}
