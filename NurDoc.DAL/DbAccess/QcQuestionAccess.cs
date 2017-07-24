// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之护理质检信息数据访问接口封装类
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class QcQuestionAccess : DBAccessBase
    {
        /// <summary>
        /// 保存质检信息
        /// </summary>
        /// <param name="qcQuestionInfo"></param>
        /// <returns></returns>
        public short SaveQuestionInfo(QCQuestionInfo qcQuestionInfo)
        {

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            DbParameter[] pmi = new DbParameter[23];
            pmi[0] = new DbParameter("patientID", qcQuestionInfo.PatientID);
            pmi[1] = new DbParameter("visitID", qcQuestionInfo.VisitID);
            pmi[2] = new DbParameter("deptStayed", qcQuestionInfo.DeptCode);
            pmi[3] = new DbParameter("doctorInCharge", qcQuestionInfo.DoctorInCharge);
            pmi[4] = new DbParameter("qaEventType", qcQuestionInfo.QuestionType);
            pmi[5] = new DbParameter("qcMsgCode", qcQuestionInfo.QuestionCode);
            pmi[6] = new DbParameter("strMessage", qcQuestionInfo.QuestionContent);
            pmi[7] = new DbParameter("issuedBy", qcQuestionInfo.CheckerName);
            pmi[8] = new DbParameter("issuedDateTime", qcQuestionInfo.CheckTime);
            pmi[9] = new DbParameter("msgStatus", qcQuestionInfo.QuestionStatus);
            pmi[10] = new DbParameter("qcModule", qcQuestionInfo.QcModule);
            pmi[11] = new DbParameter("topicID", string.IsNullOrEmpty(qcQuestionInfo.TopicID) ? string.Empty : qcQuestionInfo.TopicID);
            pmi[12] = new DbParameter("strTopic", string.IsNullOrEmpty(qcQuestionInfo.Topic) ? string.Empty : qcQuestionInfo.Topic);
            pmi[13] = new DbParameter("strPoint", string.IsNullOrEmpty(qcQuestionInfo.Point) ? string.Empty : qcQuestionInfo.Point);
            pmi[14] = new DbParameter("strPointType", string.IsNullOrEmpty(qcQuestionInfo.PointType) ? string.Empty : qcQuestionInfo.PointType);
            pmi[15] = new DbParameter("oldQcMsgCode", string.Empty);
            pmi[16] = new DbParameter("oldIssuedDateTime", DateTime.Now);
            pmi[17] = new DbParameter("operFlag", "1");
            pmi[18] = new DbParameter("applyEnv", "NURDOC");
            pmi[19] = new DbParameter("parentDoctor", qcQuestionInfo.ParentDoctor == null ? string.Empty : qcQuestionInfo.ParentDoctor);//未开启三级审签的为空，添加报错
            pmi[20] = new DbParameter("superDoctor", qcQuestionInfo.SuperDoctor == null ? string.Empty : qcQuestionInfo.SuperDoctor);
            pmi[21] = new DbParameter("msgid", qcQuestionInfo.MSG_ID);
            pmi[22] = new DbParameter("issuedID", qcQuestionInfo.CheckerID);
            int nInsertResult = 0;
            string szSQL = "SaveQuestionInfo";
            try
            {
                nInsertResult = base.DataAccess.ExecuteNonQuery("UPDATE_QC_MSG", CommandType.StoredProcedure, ref pmi);
                if (nInsertResult <= 0)
                {
                    return ServerData.ExecuteResult.OTHER_ERROR;
                }
                else
                {
                    return ServerData.ExecuteResult.OK;
                }
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally
            {
                base.DataAccess.CloseConnnection(false);
            }

        }

        /// <summary>
        /// 删除质检信息
        /// </summary>
        /// <param name="szMsgID">质检信息ID</param>
        /// <returns></returns>
        public short DeleteQuestion(QCQuestionInfo qcQuestionInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            DbParameter[] pmi = new DbParameter[23];
            pmi[0] = new DbParameter("patientID", qcQuestionInfo.PatientID);
            pmi[1] = new DbParameter("visitID", qcQuestionInfo.VisitID);
            pmi[2] = new DbParameter("deptStayed", string.Empty);
            pmi[3] = new DbParameter("doctorInCharge", string.Empty);
            pmi[4] = new DbParameter("qaEventType", string.Empty);
            pmi[5] = new DbParameter("qcMsgCode", qcQuestionInfo.QuestionCode);
            pmi[6] = new DbParameter("strMessage", string.Empty);
            pmi[7] = new DbParameter("issuedBy", string.Empty);
            pmi[8] = new DbParameter("issuedDateTime", qcQuestionInfo.CheckTime);
            pmi[9] = new DbParameter("msgStatus", string.Empty);
            pmi[10] = new DbParameter("qcModule", string.Empty);
            pmi[11] = new DbParameter("topicID", string.Empty);
            pmi[12] = new DbParameter("strTopic", string.Empty);
            pmi[13] = new DbParameter("strPoint", string.Empty);
            pmi[14] = new DbParameter("strPointType", string.Empty);
            pmi[15] = new DbParameter("oldQcMsgCode", string.Empty);
            pmi[16] = new DbParameter("oldIssuedDateTime", DateTime.Now);
            pmi[17] = new DbParameter("operFlag", "3");
            pmi[18] = new DbParameter("applyEnv", "NURDOC");
            pmi[19] = new DbParameter("parentDoctor", string.Empty);
            pmi[20] = new DbParameter("superDoctor", string.Empty);
            pmi[21] = new DbParameter("msgid", string.Empty);
            pmi[22] = new DbParameter("issuedID", string.Empty);
            int nInsertResult = 0;
            string szSQL = "SaveQuestionInfo";
            try
            {
                nInsertResult = base.DataAccess.ExecuteNonQuery("UPDATE_QC_MSG", CommandType.StoredProcedure, ref pmi);
                if (nInsertResult <= 0)
                {
                    return ServerData.ExecuteResult.OTHER_ERROR;
                }
                else
                {
                    return ServerData.ExecuteResult.OK;
                }
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally
            {
                base.DataAccess.CloseConnnection(false);
            }
        }


        /// <summary>
        /// 病历质控系统,获取指定病人指定就诊下的病案质控信息列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szQuestionStatus">问题状态</param>
        /// <param name="lstQCQuestionInfos">病案质控信息列表</param>
        public short GetQCQuestionList(string szPatientID, string szVisitID, string szQuestionStatus, ref List<QCQuestionInfo> lstQCQuestionInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}"
               , ServerData.QCMessageView.PATIENT_ID, ServerData.QCMessageView.VISIT_ID, ServerData.QCMessageView.DEPT_STAYED
               , ServerData.QCMessageView.INCHARGE_DOCTOR, ServerData.QCMessageView.QA_EVENT_TYPE, ServerData.QCMessageView.QC_MSG_CODE
               , ServerData.QCMessageView.MESSAGE, ServerData.QCMessageView.ISSUED_BY, ServerData.QCMessageView.ISSUED_DATE_TIME
               , ServerData.QCMessageView.MSG_STATUS, ServerData.QCMessageView.ASK_DATE_TIME, ServerData.QCMessageView.QC_MODULE
               , ServerData.QCMessageView.TOPIC_ID, ServerData.QCMessageView.TOPIC, ServerData.QCMessageView.DOCTOR_COMMENT
               , ServerData.QCMessageView.POINT, ServerData.QCMessageView.POINT_TYPE, ServerData.QCMessageView.MSG_ID, ServerData.QCMessageView.LOCK_STATUS);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.QCMessageView.PATIENT_ID, szPatientID, ServerData.QCMessageView.VISIT_ID, szVisitID);
            if (szQuestionStatus != null)
                szCondition = string.Format("{0} AND {1}='{2}'", szCondition, ServerData.QCMessageView.MSG_STATUS, szQuestionStatus);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataView.QC_MSG_V, szCondition, ServerData.QCMessageView.ISSUED_DATE_TIME);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstQCQuestionInfos == null)
                    lstQCQuestionInfos = new List<QCQuestionInfo>();
                do
                {
                    QCQuestionInfo qcQuestionInfo = new QCQuestionInfo(false);
                    if (!dataReader.IsDBNull(0)) qcQuestionInfo.PatientID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) qcQuestionInfo.VisitID = dataReader.GetValue(1).ToString();
                    if (!dataReader.IsDBNull(2)) qcQuestionInfo.DeptCode = dataReader.GetString(2);
                    //if (!dataReader.IsDBNull(3)) qcQuestionInfo.DoctorInCharge = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcQuestionInfo.QuestionType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) qcQuestionInfo.QuestionCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) qcQuestionInfo.QuestionContent = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) qcQuestionInfo.CheckerName = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) qcQuestionInfo.CheckTime = dataReader.GetDateTime(8);
                    if (!dataReader.IsDBNull(9)) qcQuestionInfo.QuestionStatus = dataReader.GetValue(9).ToString();
                    if (!dataReader.IsDBNull(10)) qcQuestionInfo.ConfirmTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11)) qcQuestionInfo.QcModule = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) qcQuestionInfo.TopicID = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) qcQuestionInfo.Topic = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) qcQuestionInfo.DoctorComment = dataReader.GetString(14);
                    //  if (!dataReader.IsDBNull(15)) qcQuestionInfo.Point = dataReader.GetValue(15).ToString();
                    //  if (!dataReader.IsDBNull(16)) qcQuestionInfo.PointType = dataReader.GetValue(16).ToString();
                    if (!dataReader.IsDBNull(17)) qcQuestionInfo.MSG_ID = dataReader.GetString(17);
                    //  if (!dataReader.IsDBNull(18)) qcQuestionInfo.LockStatus = int.Parse(dataReader.GetValue(18).ToString()) == 1 ? true : false;
                    lstQCQuestionInfos.Add(qcQuestionInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally
            {
                base.DataAccess.CloseConnnection(false);
            }
        }
        /// <summary>
        /// 更改质检信息
        /// </summary>
        /// <param name="questionInfo">质检信息</param>
        /// <param name="newIssuedTime">新的质检时间</param>
        /// <param name="szOldMsgCode">老的问题代码</param>
        /// <returns></returns>
        public short UpdateQuestion(QCQuestionInfo qcQuestionInfo, DateTime newIssuedTime, string szOldMsgCode)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            DbParameter[] pmi = new DbParameter[23];
            pmi[0] = new DbParameter("patientID", qcQuestionInfo.PatientID);
            pmi[1] = new DbParameter("visitID", qcQuestionInfo.VisitID);
            pmi[2] = new DbParameter("deptStayed", qcQuestionInfo.DeptCode);
            pmi[3] = new DbParameter("doctorInCharge", qcQuestionInfo.DoctorInCharge);
            pmi[4] = new DbParameter("qaEventType", qcQuestionInfo.QuestionType);
            pmi[5] = new DbParameter("qcMsgCode", qcQuestionInfo.QuestionCode);
            pmi[6] = new DbParameter("strMessage", qcQuestionInfo.QuestionContent);
            pmi[7] = new DbParameter("issuedBy", qcQuestionInfo.CheckerName);
            pmi[8] = new DbParameter("issuedDateTime", newIssuedTime);
            pmi[9] = new DbParameter("msgStatus", qcQuestionInfo.QuestionStatus);
            pmi[10] = new DbParameter("qcModule", qcQuestionInfo.QcModule);
            pmi[11] = new DbParameter("topicID", string.Empty);
            pmi[12] = new DbParameter("strTopic", qcQuestionInfo.Topic);
            pmi[13] = new DbParameter("strPoint", qcQuestionInfo.Point == null ? string.Empty : qcQuestionInfo.Point);
            pmi[14] = new DbParameter("strPointType", qcQuestionInfo.PointType == null ? string.Empty : qcQuestionInfo.PointType);
            pmi[15] = new DbParameter("oldQcMsgCode", szOldMsgCode);
            pmi[16] = new DbParameter("oldIssuedDateTime", qcQuestionInfo.CheckTime);
            pmi[17] = new DbParameter("operFlag", "2");
            pmi[18] = new DbParameter("applyEnv", "NURDOC");
            pmi[19] = new DbParameter("parentDoctor", qcQuestionInfo.ParentDoctor);
            pmi[20] = new DbParameter("superDoctor", qcQuestionInfo.SuperDoctor);
            pmi[21] = new DbParameter("msgid", qcQuestionInfo.MSG_ID);
            pmi[22] = new DbParameter("issuedID", qcQuestionInfo.CheckerID);
            int nInsertResult = 0;
            string szSQL = "SaveQuestionInfo";
            try
            {
                nInsertResult = base.DataAccess.ExecuteNonQuery("UPDATE_QC_MSG", CommandType.StoredProcedure, ref pmi);
                if (nInsertResult <= 0)
                {
                    return ServerData.ExecuteResult.OTHER_ERROR;
                }
                else
                {
                    return ServerData.ExecuteResult.OK;
                }
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally
            {
                base.DataAccess.CloseConnnection(false);
            }
        }
    }
}
