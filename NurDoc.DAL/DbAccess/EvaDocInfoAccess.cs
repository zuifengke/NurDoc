// ***********************************************************
// 数据库访问层与护理评价(NUR_EVALUATION_INDEX)有关的数据的访问类.
// Creator:YeChongchong  Date:2016-1-15
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
    public class EvaDocInfoAccess : DBAccessBase
    {
        #region"读写文档索引接口"
        public short GetEvaDocInfos(string szWardCode, string szEvaTypeID, DateTime dtBeginTime, DateTime dtEndTime, ref List<EvaDocInfo> lstEvaDocInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.EvaluationDocTable.EVA_ID, ServerData.EvaluationDocTable.EVA_TYPE, ServerData.EvaluationDocTable.EVA_NAME
                , ServerData.EvaluationDocTable.EVA_TIME, ServerData.EvaluationDocTable.EVA_VERSION, ServerData.EvaluationDocTable.EVA_STATUS
                , ServerData.EvaluationDocTable.CREATOR_ID, ServerData.EvaluationDocTable.CREATOR_NAME, ServerData.EvaluationDocTable.MODIFIER_ID
                , ServerData.EvaluationDocTable.MODIFIER_NAME, ServerData.EvaluationDocTable.MODIFY_TIME, ServerData.EvaluationDocTable.WARD_CODE
                , ServerData.EvaluationDocTable.WARD_NAME, ServerData.EvaluationDocTable.EVA_SCORE, ServerData.EvaluationDocTable.EVA_REMARK);

            string szCondition = string.Format("{0} = '{1}' and {2} = '{3}' and {4} != {5} AND {6}>={7} AND {8}<={9}"
                , ServerData.EvaluationDocTable.WARD_CODE, szWardCode
                , ServerData.EvaluationDocTable.EVA_TYPE, szEvaTypeID
                , ServerData.EvaluationDocTable.EVA_STATUS, ServerData.DocStatus.CANCELED
                , ServerData.EvaluationDocTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(dtBeginTime)
                , ServerData.EvaluationDocTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(dtEndTime));

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_DESC, szField, ServerData.DataTable.NUR_EVALUATION
                , szCondition, ServerData.EvaluationDocTable.MODIFY_TIME);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstEvaDocInfos == null)
                    lstEvaDocInfos = new List<EvaDocInfo>();
                do
                {
                    EvaDocInfo evadocInfo = new EvaDocInfo();
                    evadocInfo.EvaID = dataReader.GetString(0);
                    evadocInfo.EvaTypeID = dataReader.GetString(1);
                    evadocInfo.EvaTypeName = dataReader.GetString(2);
                    evadocInfo.EvaTime = dataReader.GetDateTime(3);
                    evadocInfo.Version = int.Parse(dataReader.GetValue(4).ToString());
                    evadocInfo.Status = dataReader.GetString(5);
                    evadocInfo.CreatorId = dataReader.GetString(6);
                    evadocInfo.CreatorName = dataReader.GetString(7);
                    evadocInfo.ModifierID = dataReader.GetString(8);
                    evadocInfo.ModifierName = dataReader.GetString(9);
                    evadocInfo.ModifyTime = dataReader.GetDateTime(10);
                    evadocInfo.WardCode = dataReader.GetString(11);
                    evadocInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                    {
                        if (!GlobalMethods.Misc.IsEmptyString(dataReader.GetValue(13).ToString()))
                            evadocInfo.Score = decimal.Parse(dataReader.GetValue(13).ToString());
                    }
                    if (!dataReader.IsDBNull(14))
                        evadocInfo.Remark = dataReader.GetString(14);

                    lstEvaDocInfos.Add(evadocInfo);
                } while (dataReader.Read());
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
        /// 根据文档ID，获取文档基本信息
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="docInfo">文档信息类</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetEvaDocInfo(string szEvaID, ref EvaDocInfo evadocInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szEvaID))
            {
                LogManager.Instance.WriteLog("EvaDocInfoAccess.GetEvaDocInfo", new string[] { "szDocID" }, new object[] { szEvaID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szEvaID = szEvaID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.EvaluationDocTable.EVA_ID, ServerData.EvaluationDocTable.EVA_TYPE, ServerData.EvaluationDocTable.EVA_NAME
                , ServerData.EvaluationDocTable.EVA_TIME, ServerData.EvaluationDocTable.EVA_VERSION, ServerData.EvaluationDocTable.EVA_STATUS
                , ServerData.EvaluationDocTable.CREATOR_ID, ServerData.EvaluationDocTable.CREATOR_NAME, ServerData.EvaluationDocTable.MODIFIER_ID
                , ServerData.EvaluationDocTable.MODIFIER_NAME, ServerData.EvaluationDocTable.MODIFY_TIME, ServerData.EvaluationDocTable.WARD_CODE
                , ServerData.EvaluationDocTable.WARD_NAME, ServerData.EvaluationDocTable.EVA_SCORE);
            string szCondition = string.Format("{0}='{1}'", ServerData.EvaluationDocTable.EVA_ID, szEvaID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_EVALUATION, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("EvaDocInfoAccess.GetEvaDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "未查询到记录");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (evadocInfo == null)
                    evadocInfo = new EvaDocInfo();
                do
                {
                    evadocInfo = new EvaDocInfo();
                    evadocInfo.EvaID = dataReader.GetString(0);
                    evadocInfo.EvaTypeID = dataReader.GetString(1);
                    evadocInfo.EvaTypeName = dataReader.GetString(2);
                    evadocInfo.EvaTime = dataReader.GetDateTime(3);
                    evadocInfo.Version = int.Parse(dataReader.GetValue(4).ToString());
                    evadocInfo.Status = dataReader.GetString(5);
                    evadocInfo.CreatorId = dataReader.GetString(6);
                    evadocInfo.CreatorName = dataReader.GetString(7);
                    evadocInfo.ModifierID = dataReader.GetString(8);
                    evadocInfo.ModifierName = dataReader.GetString(9);
                    evadocInfo.ModifyTime = dataReader.GetDateTime(10);
                    evadocInfo.WardCode = dataReader.GetString(11);
                    evadocInfo.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        evadocInfo.Score = decimal.Parse(dataReader.GetValue(13).ToString());

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
        private short AddEvaDocInfo(EvaDocInfo docInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.EvaluationDocTable.EVA_ID, ServerData.EvaluationDocTable.EVA_TYPE, ServerData.EvaluationDocTable.EVA_NAME
                , ServerData.EvaluationDocTable.EVA_TIME, ServerData.EvaluationDocTable.EVA_VERSION, ServerData.EvaluationDocTable.EVA_STATUS
                , ServerData.EvaluationDocTable.CREATOR_ID, ServerData.EvaluationDocTable.CREATOR_NAME, ServerData.EvaluationDocTable.MODIFIER_ID
                , ServerData.EvaluationDocTable.MODIFIER_NAME, ServerData.EvaluationDocTable.MODIFY_TIME, ServerData.EvaluationDocTable.WARD_CODE
                , ServerData.EvaluationDocTable.WARD_NAME, ServerData.EvaluationDocTable.EVA_SCORE, ServerData.EvaluationDocTable.EVA_REMARK);
            string szValue = string.Format("'{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}',{13},'{14}'"
                , docInfo.EvaID, docInfo.EvaTypeID, docInfo.EvaTypeName, base.DataAccess.GetSqlTimeFormat(docInfo.EvaTime)
                , docInfo.Version, docInfo.Status, docInfo.CreatorId, docInfo.CreatorName, docInfo.ModifierID
                , docInfo.ModifierName, base.DataAccess.GetSqlTimeFormat(docInfo.ModifyTime), docInfo.WardCode
                , docInfo.WardName, docInfo.Score, docInfo.Remark);

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_EVALUATION, szField, szValue);

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
                LogManager.Instance.WriteLog("EvaDocInfoAccess.AddEvaDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        public short DeleteEvaDocInfo(EvaDocInfo docInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}'"
                , ServerData.EvaluationDocTable.EVA_STATUS, ServerData.DocStatus.CANCELED);
            string szCondition = string.Format("{0}='{1}'", ServerData.EvaluationDocTable.EVA_ID, docInfo.EvaID);

            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_EVALUATION, szField, szCondition);

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
                LogManager.Instance.WriteLog("EvaDocInfoAccess.DeleteEvaDocInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        public short UpdateEvaDocDatas(EvaDocInfo oldDocInfo, EvaDocInfo newDocInfo, List<EvaDocItemInfo> datas)
        {
            if (newDocInfo == null || oldDocInfo == null || base.DataAccess == null || oldDocInfo.Version != newDocInfo.Version - 1)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;
            this.DeleteEvaDocInfo(oldDocInfo);
            this.AddEvaDocInfo(newDocInfo);
            foreach (EvaDocItemInfo item in datas)
            {
                this.AddEvaDocDataInfo(item);
            }
            //提交数据库更新
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }

        public short AddEvaDocDatas(EvaDocInfo newDocInfo, List<EvaDocItemInfo> datas)
        {
            if (newDocInfo == null || base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            this.AddEvaDocInfo(newDocInfo);
            foreach (EvaDocItemInfo item in datas)
            {
                this.AddEvaDocDataInfo(item);
            }
            //提交数据库更新
            if (!base.DataAccess.CommitTransaction(true))
                return ServerData.ExecuteResult.EXCEPTION;
            return ServerData.ExecuteResult.OK;
        }

        #endregion

        #region"评价内容接口"
        public short GetItemDatasByID(string szEvaID, ref List<EvaDocItemInfo> lstEvaDatas)
        {
            if (GlobalMethods.Misc.IsEmptyString(szEvaID))
            {
                LogManager.Instance.WriteLog("EvaDocInfoAccess.GetItemDatasByID"
                    , new string[] { "szEvaID" }, new object[] { szEvaID }, "文档ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (this.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.EvaluationDataTable.EVA_ID, ServerData.EvaluationDataTable.ITEM_NO
                , ServerData.EvaluationDataTable.ITEM_VALUE, ServerData.EvaluationDataTable.ITEM_REMARK); 
            string szCondition = string.Format("{0} = '{1}'", ServerData.EvaluationDataTable.EVA_ID, szEvaID);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_EVALUATION_DATA, szCondition);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstEvaDatas == null)
                    lstEvaDatas = new List<EvaDocItemInfo>();
                do
                {
                    EvaDocItemInfo dataInfo = new EvaDocItemInfo();
                    dataInfo.EvaID = dataReader.GetString(0);
                    dataInfo.ItemNo = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        dataInfo.ItemValue = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        dataInfo.ItemRemark = dataReader.GetString(3);
                    lstEvaDatas.Add(dataInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        private short AddEvaDocDataInfo(EvaDocItemInfo item)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.EvaluationDataTable.EVA_ID, ServerData.EvaluationDataTable.ITEM_NO
                , ServerData.EvaluationDataTable.ITEM_VALUE, ServerData.EvaluationDataTable.ITEM_REMARK);
            string szValue = string.Format("'{0}','{1}','{2}','{3}'"
                , item.EvaID, item.ItemNo, item.ItemValue, item.ItemRemark);

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_EVALUATION_DATA, szField, szValue);

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
                LogManager.Instance.WriteLog("EvaDocInfoAccess.AddEvaDocDataInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion
    }
}
