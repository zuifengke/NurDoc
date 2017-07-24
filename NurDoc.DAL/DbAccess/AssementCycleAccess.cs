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
    /// <summary>
    /// 评估任务表
    /// </summary>
    public class AssementCycleAccess : DBAccessBase
    {

        public short GetAssementCycleList()
        {
            return ServerData.ExecuteResult.OK;
        }
        public short GetAssementCycle(string szDocID, ref AssessmentCycle model)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            if (string.IsNullOrEmpty(szDocID))
                return ServerData.ExecuteResult.PARAM_ERROR;
            string szField = string.Format("*");
            string szCondition = string.Format(" 1=1 and {0}='{1}'"
                , ServerData.AssessmentCycleTable.DOC_ID
                , szDocID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                , szField, ServerData.DataTable.ASSESSMENT_CYCLE, szCondition);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                if (model == null)
                    model = new AssessmentCycle();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    if (dataReader.IsDBNull(i))
                        continue;
                    switch (dataReader.GetName(i))
                    {
                        case ServerData.AssessmentCycleTable.CREATOR_ID:
                            model.CREATOR_ID = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.CREATOR_NAME:
                            model.CREATOR_NAME = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.CYCLE:
                            model.CYCLE = decimal.Parse(dataReader.GetValue(i).ToString());
                            break;
                        case ServerData.AssessmentCycleTable.DATA:
                            model.DATA = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.DOCTYPE_ID:
                            model.DOCTYPE_ID = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.DOCTYPE_NAME:
                            model.DOCTYPE_NAME = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.DOC_ID:
                            model.DOC_ID = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.ID:
                            model.ID = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.PATIENT_ID:
                            model.PATIENT_ID = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.RECORD_TIME:
                            model.RECORD_TIME = DateTime.Parse(dataReader.GetValue(i).ToString());
                            break;
                        case ServerData.AssessmentCycleTable.UNIT:
                            model.UNIT = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.VISIT_ID:
                            model.VISIT_ID = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.WARD_CODE:
                            model.WARD_CODE = dataReader.GetValue(i).ToString();
                            break;
                        case ServerData.AssessmentCycleTable.WARD_NAME:
                            model.WARD_NAME = dataReader.GetValue(i).ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("获取评估任务失败", ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        public short Insert(AssessmentCycle model)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            if (model == null || model.ID == string.Empty)
                return ServerData.ExecuteResult.PARAM_ERROR;

            StringBuilder sbField = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.CREATOR_ID);
            sbValue.AppendFormat("'{0}',", model.CREATOR_ID);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.CREATOR_NAME);
            sbValue.AppendFormat("'{0}',", model.CREATOR_NAME);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.CYCLE);
            sbValue.AppendFormat("{0},", model.CYCLE);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.DATA);
            sbValue.AppendFormat("'{0}',", model.DATA);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.DOCTYPE_ID);
            sbValue.AppendFormat("'{0}',", model.DOCTYPE_ID);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.DOCTYPE_NAME);
            sbValue.AppendFormat("'{0}',", model.DOCTYPE_NAME);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.DOC_ID);
            sbValue.AppendFormat("'{0}',", model.DOC_ID);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.ID);
            sbValue.AppendFormat("'{0}',", model.ID);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.PATIENT_ID);
            sbValue.AppendFormat("'{0}',", model.PATIENT_ID);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.RECORD_TIME);
            sbValue.AppendFormat("{0},", base.DataAccess.GetSqlTimeFormat(model.RECORD_TIME));
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.UNIT);
            sbValue.AppendFormat("'{0}',", model.UNIT);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.VISIT_ID);
            sbValue.AppendFormat("'{0}',", model.VISIT_ID);
            sbField.AppendFormat("{0},", ServerData.AssessmentCycleTable.WARD_CODE);
            sbValue.AppendFormat("'{0}',", model.WARD_CODE);
            sbField.AppendFormat("{0}", ServerData.AssessmentCycleTable.WARD_NAME);
            sbValue.AppendFormat("'{0}'", model.WARD_NAME);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.ASSESSMENT_CYCLE, sbField.ToString(), sbValue.ToString());
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("", new string[] { "szSQL" }, new object[] { szSQL }, ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="model">评估任务类</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short Update(AssessmentCycle model)
        {
            if (model == null)
            {
                LogManager.Instance.WriteLog("", new string[] { "" }
                    , new object[] { model }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            StringBuilder sbField = new StringBuilder();
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.CREATOR_ID, model.CREATOR_ID);
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.CREATOR_NAME, model.CREATOR_NAME
                );
            sbField.AppendFormat("{0}={1},"
                , ServerData.AssessmentCycleTable.CYCLE, model.CYCLE
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.DATA, model.DATA
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.DOCTYPE_ID, model.DOCTYPE_ID
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.DOCTYPE_NAME, model.DOCTYPE_NAME
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.DOC_ID, model.DOC_ID
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.PATIENT_ID, model.PATIENT_ID
                );
            sbField.AppendFormat("{0}={1},"
                , ServerData.AssessmentCycleTable.RECORD_TIME, base.DataAccess.GetSqlTimeFormat(model.RECORD_TIME)
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.UNIT, model.UNIT
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.VISIT_ID, model.VISIT_ID
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.WARD_CODE, model.WARD_CODE
                );
            sbField.AppendFormat("{0}='{1}',"
                , ServerData.AssessmentCycleTable.WARD_NAME, model.WARD_NAME
                );
            string szCondition = string.Format("{0}='{1}'", ServerData.AssessmentCycleTable.ID, model.ID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.ASSESSMENT_CYCLE, sbField.ToString(), szCondition);
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("", new string[] { "szSQL" }, new object[] { szSQL }, ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        
        public short Delete(string szID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (GlobalMethods.Misc.IsEmptyString(szID))
            {
                LogManager.Instance.WriteLog("", new string[] { "" }
                    , new object[] { szID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            string szCondition = string.Format("{0}='{1}'", ServerData.AssessmentCycleTable.ID, szID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.ASSESSMENT_CYCLE, szCondition);
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("", new string[] { "szSQL" }, new object[] { szSQL }, ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
    }
}
