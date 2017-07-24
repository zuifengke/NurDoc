// ***********************************************************
// 数据库访问层护理评价访问类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class EvaTypeAccess : DBAccessBase
    {
        #region"评估表单模板读写接口"
        /// <summary>
        /// 获取所有文档类型信息列表
        /// </summary>
        /// <param name="szApplyEnv">表单应用环境</param>
        /// <param name="lstDocTypeInfos">文档类型信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetEvaTypeInfos(ref List<EvaTypeInfo> lstEvaTypeInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                , ServerData.EvaluationTypeTable.EVATYPE_ID, ServerData.EvaluationTypeTable.EVATYPE_NAME
                , ServerData.EvaluationTypeTable.IS_FOLDER, ServerData.EvaluationTypeTable.IS_VALID
                , ServerData.EvaluationTypeTable.IS_VISIBLE, ServerData.EvaluationTypeTable.PARENT_ID
                , ServerData.EvaluationTypeTable.EVATYPE_NO, ServerData.EvaluationTypeTable.CREATE_TIME
                , ServerData.EvaluationTypeTable.HAVE_REMARK, ServerData.EvaluationTypeTable.STANDARD);

            string szSQL = null;
            szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField
                , ServerData.DataTable.NUR_EVALUATION_TYPE, ServerData.EvaluationTypeTable.EVATYPE_ID);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstEvaTypeInfos == null)
                    lstEvaTypeInfos = new List<EvaTypeInfo>();
                do
                {
                    EvaTypeInfo evaTypeInfo = new EvaTypeInfo();
                    evaTypeInfo.EvaTypeID = dataReader.GetString(0);
                    evaTypeInfo.EvaTypeName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        evaTypeInfo.IsFolder = dataReader.GetValue(2).ToString().Equals("1");
                    if (!dataReader.IsDBNull(3))
                        evaTypeInfo.IsValid = dataReader.GetValue(3).ToString().Equals("1");
                    if (!dataReader.IsDBNull(4))
                        evaTypeInfo.IsVisible = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        evaTypeInfo.ParentID = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6))
                        evaTypeInfo.EvaTypeNo = int.Parse(dataReader.GetValue(6).ToString());
                    if (!dataReader.IsDBNull(7))
                        evaTypeInfo.CreateTime = dataReader.GetDateTime(7);
                    if (!dataReader.IsDBNull(8))
                        evaTypeInfo.HaveRemark = dataReader.GetValue(8).ToString().Equals("1");
                    if (!dataReader.IsDBNull(9))
                        evaTypeInfo.Standard = int.Parse(dataReader.GetValue(9).ToString());

                    lstEvaTypeInfos.Add(evaTypeInfo);
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
        /// 获取评估单选项
        /// </summary>
        /// <param name="evaTypeInfo"></param>
        /// <param name="listEvaTypeItem"></param>
        /// <returns></returns>
        public short GetEvaTypeItems(EvaTypeInfo evaTypeInfo, ref List<EvaTypeItem> listEvaTypeItem)
        {
            if (evaTypeInfo == null || GlobalMethods.Misc.IsEmptyString(evaTypeInfo.EvaTypeID))
            {
                LogManager.Instance.WriteLog("EvaTypeAccess.GetEvaTypeItems", new string[] { "evaTypeInfo" }
                    , new object[] { evaTypeInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
            , ServerData.EvaluationItemTable.EVATYPE_ID, ServerData.EvaluationItemTable.ITEM_NO, ServerData.EvaluationItemTable.ITEM_TEXT
            , ServerData.EvaluationItemTable.ITEM_TEXT_BOLD, ServerData.EvaluationItemTable.ITEM_TYPE, ServerData.EvaluationItemTable.ITEM_SCORE
            , ServerData.EvaluationItemTable.ITEM_DEFAULTVALUE, ServerData.EvaluationItemTable.ITEM_IN_COUNT, ServerData.EvaluationItemTable.ITEM_SORT
            , ServerData.EvaluationItemTable.ITEM_READONLY, ServerData.EvaluationItemTable.ITEM_ENABLE);

            string szCondition = string.Format("{0}='{1}'", ServerData.EvaluationItemTable.EVATYPE_ID, evaTypeInfo.EvaTypeID);

            string szSQL = null;
            szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataTable.NUR_EVALUATION_ITEM, szCondition, ServerData.EvaluationItemTable.ITEM_SORT);
            IDataReader dataReader = null;

            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (listEvaTypeItem == null)
                    listEvaTypeItem = new List<EvaTypeItem>();
                do
                {
                    EvaTypeItem EvaTypeItem = new EvaTypeItem();
                    EvaTypeItem.EvaTypeID = dataReader.GetString(0);
                    EvaTypeItem.ItemNo = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        EvaTypeItem.ItemText = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        EvaTypeItem.ItemTextBlod = dataReader.GetValue(3).ToString().Equals("1");
                    if (!dataReader.IsDBNull(4))
                        EvaTypeItem.ItemType = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5))
                        EvaTypeItem.ItemScore = int.Parse(dataReader.GetValue(5).ToString());
                    if (!dataReader.IsDBNull(6))
                        EvaTypeItem.ItemDefaultValue = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        EvaTypeItem.ItemInCount = dataReader.GetValue(7).ToString().Equals("1");
                    if (!dataReader.IsDBNull(8))
                        EvaTypeItem.ItemSort = int.Parse(dataReader.GetValue(8).ToString());
                    if (!dataReader.IsDBNull(9))
                        EvaTypeItem.ItemReadOnly = dataReader.GetValue(9).ToString().Equals("1");
                    if (!dataReader.IsDBNull(10))
                        EvaTypeItem.ItemEnable = dataReader.GetValue(10).ToString().Equals("1");

                    listEvaTypeItem.Add(EvaTypeItem);
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
        /// 保存一条新的病历类型配置信息
        /// </summary>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveEvaTypeInfo(EvaTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || GlobalMethods.Misc.IsEmptyString(docTypeInfo.EvaTypeID))
            {
                LogManager.Instance.WriteLog("EvaTypeAccess.SaveEvaTypeInfo", new string[] { "EvaTypeInfo" }
                    , new object[] { docTypeInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;


            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
            , ServerData.EvaluationTypeTable.EVATYPE_ID, ServerData.EvaluationTypeTable.EVATYPE_NAME, ServerData.EvaluationTypeTable.IS_FOLDER
            , ServerData.EvaluationTypeTable.IS_VALID, ServerData.EvaluationTypeTable.IS_VISIBLE, ServerData.EvaluationTypeTable.PARENT_ID
            , ServerData.EvaluationTypeTable.EVATYPE_NO, ServerData.EvaluationTypeTable.CREATE_TIME, ServerData.EvaluationTypeTable.HAVE_REMARK
            , ServerData.EvaluationTypeTable.STANDARD);
            string szValue = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}'"
            , docTypeInfo.EvaTypeID, docTypeInfo.EvaTypeName, docTypeInfo.IsFolder ? "1" : "0"
            , docTypeInfo.IsValid ? "1" : "0", docTypeInfo.IsVisible ? "1" : "0", docTypeInfo.ParentID
            , docTypeInfo.EvaTypeNo, base.DataAccess.GetSystemTimeSql(), docTypeInfo.HaveRemark ? "1" : "0"
            , docTypeInfo.Standard);

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_EVALUATION_TYPE, szField, szValue);

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
                LogManager.Instance.WriteLog("EvaTypeAccess.SaveDocTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        public short SaveEvaItem(EvaTypeItem evaTypeItem)
        {
            if (evaTypeItem == null || GlobalMethods.Misc.IsEmptyString(evaTypeItem.EvaTypeID))
            {
                LogManager.Instance.WriteLog("EvaTypeAccess.SaveEvaItem", new string[] { "evaTypeItem" }
                    , new object[] { evaTypeItem }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;


            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
            , ServerData.EvaluationItemTable.EVATYPE_ID, ServerData.EvaluationItemTable.ITEM_NO, ServerData.EvaluationItemTable.ITEM_TEXT
            , ServerData.EvaluationItemTable.ITEM_TEXT_BOLD, ServerData.EvaluationItemTable.ITEM_TYPE, ServerData.EvaluationItemTable.ITEM_SCORE
            , ServerData.EvaluationItemTable.ITEM_DEFAULTVALUE, ServerData.EvaluationItemTable.ITEM_IN_COUNT, ServerData.EvaluationItemTable.ITEM_SORT
            , ServerData.EvaluationItemTable.ITEM_READONLY, ServerData.EvaluationItemTable.ITEM_ENABLE);
            string szValue = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'"
                , evaTypeItem.EvaTypeID, evaTypeItem.ItemNo, evaTypeItem.ItemText
                , evaTypeItem.ItemTextBlod ? "1" : "0", evaTypeItem.ItemType, evaTypeItem.ItemScore
                , evaTypeItem.ItemDefaultValue, evaTypeItem.ItemInCount ? "1" : "0", evaTypeItem.ItemSort
                , evaTypeItem.ItemReadOnly ? "1" : "0", evaTypeItem.ItemEnable ? "1" : "0");

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_EVALUATION_ITEM, szField, szValue);

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
                LogManager.Instance.WriteLog("EvaTypeAccess.SaveDocTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        public short UpdateEvaItems(string szEvaTypeID, List<EvaTypeItem> lstEvaTypeItems)
        {
            if (lstEvaTypeItems == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;
            short shRet = ServerData.ExecuteResult.OK;
            shRet = DeleteEvaItems(szEvaTypeID);
            if (shRet != ServerData.ExecuteResult.OK && shRet != ServerData.ExecuteResult.RES_NO_FOUND)
            {
                base.DataAccess.AbortTransaction();
            }

            for (int index = 0; index < lstEvaTypeItems.Count; index++)
            {
                shRet = this.SaveEvaItem(lstEvaTypeItems[index]);
            }


            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
            }
            else
            {
                if (!base.DataAccess.CommitTransaction(true))
                    return ServerData.ExecuteResult.EXCEPTION;
            }
            return shRet;
        }

        public short DeleteEvaTypeInfos(List<string> lstDocTypeID)
        {
            if (lstDocTypeID == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = ServerData.ExecuteResult.OK;
            for (int index = 0; index < lstDocTypeID.Count; index++)
            {
                string szDocTypeID = lstDocTypeID[index];
                if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
                    continue;
                shRet = this.DeleteEvaTypeInfo(szDocTypeID);
                if (shRet != ServerData.ExecuteResult.OK)
                    break;
            }
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
            }
            else
            {
                if (!base.DataAccess.CommitTransaction(true))
                    return ServerData.ExecuteResult.EXCEPTION;
            }
            return shRet;
        }

        public short ModifyEvaTypeInfo(string szDocTypeID, EvaTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("EvaTypeAccess.ModifyEvaTypeInfo", new string[] { "docTypeInfo" }
                    , new object[] { docTypeInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocTypeID = szDocTypeID.Trim();
            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}={7},{8}={9},{10}='{11}',{12}={13},{14}={15},{16}={17},{18}='{19}'"
                , ServerData.EvaluationTypeTable.EVATYPE_ID, docTypeInfo.EvaTypeID
                , ServerData.EvaluationTypeTable.EVATYPE_NAME, docTypeInfo.EvaTypeName
                , ServerData.EvaluationTypeTable.IS_FOLDER, docTypeInfo.IsFolder ? "1" : "0"
                , ServerData.EvaluationTypeTable.IS_VALID, docTypeInfo.IsValid ? "1" : "0"
                , ServerData.EvaluationTypeTable.IS_VISIBLE, docTypeInfo.IsVisible ? "1" : "0"
                , ServerData.EvaluationTypeTable.PARENT_ID, docTypeInfo.ParentID
                , ServerData.EvaluationTypeTable.EVATYPE_NO, docTypeInfo.EvaTypeNo.ToString()
                , ServerData.EvaluationTypeTable.CREATE_TIME, base.DataAccess.GetSqlTimeFormat(docTypeInfo.CreateTime)
                , ServerData.EvaluationTypeTable.HAVE_REMARK, docTypeInfo.HaveRemark ? "1" : "0"
                , ServerData.EvaluationTypeTable.STANDARD, docTypeInfo.Standard);
            string szCondiction = string.Format("{0}='{1}'", ServerData.EvaluationTypeTable.EVATYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_EVALUATION_TYPE, szField, szCondiction);

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
                LogManager.Instance.WriteLog("EvaTypeAccess.ModifyDocTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        public short DeleteEvaTypeInfo(string szDocTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("EvaTypeAccess.DeleteEvaTypeInfo", new string[] { "szDocTypeID" },
                    new object[] { szDocTypeID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.EvaluationTypeTable.EVATYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_EVALUATION_TYPE, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            //this.DeleteWardDocTypes(szDocTypeID);        //删除病区病历类型
            //this.DeleteGridViewColumns(szDocTypeID);      //删除病历类型模板摘要列
            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }

        public short DeleteEvaItems(string szEvatypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szEvatypeID))
            {
                LogManager.Instance.WriteLog("EvaTypeAccess.DeleteEvaItems", new string[] { "szEvatypeID" },
                    new object[] { szEvatypeID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.EvaluationItemTable.EVATYPE_ID, szEvatypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_EVALUATION_ITEM, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }

        #endregion

        #region"病区护理评价类型配置数据访问接口"
        /// <summary>
        ///获取统一病区下的所有护理评价类
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstWardDocTypes">病区病历类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWardEvaTypeList(string szWardCode, ref List<WardEvaType> lstWardEvaTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardEvaTypeTable.EVATYPE_ID, ServerData.WardEvaTypeTable.EVATYPE_NAME
                , ServerData.WardEvaTypeTable.WARD_CODE, ServerData.WardEvaTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardEvaTypeTable.WARD_CODE, szWardCode);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_EVA_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_EVA_TYPE);
            }

            if (lstWardEvaTypes == null)
                lstWardEvaTypes = new List<WardEvaType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardEvaType wardEvaType = new WardEvaType();
                    wardEvaType.EvaTypeID = dataReader.GetString(0);
                    wardEvaType.EvaTypeName = dataReader.GetString(1);
                    wardEvaType.WardCode = dataReader.GetString(2);
                    wardEvaType.WardName = dataReader.GetString(3);
                    lstWardEvaTypes.Add(wardEvaType);
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
        /// 获取所有文档类型代码应用到的病区列表
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="lstWardDocTypes">病区病历类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetEvaTypeDeptList(string szEvaTypeID, ref List<WardEvaType> lstWardEvaTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardEvaTypeTable.EVATYPE_ID, ServerData.WardEvaTypeTable.EVATYPE_NAME
                , ServerData.WardEvaTypeTable.WARD_CODE, ServerData.WardEvaTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szEvaTypeID))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardEvaTypeTable.EVATYPE_ID, szEvaTypeID);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_EVA_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_EVA_TYPE);
            }

            if (lstWardEvaTypes == null)
                lstWardEvaTypes = new List<WardEvaType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardEvaType wardEvaType = new WardEvaType();
                    wardEvaType.EvaTypeID = dataReader.GetString(0);
                    wardEvaType.EvaTypeName = dataReader.GetString(1);
                    wardEvaType.WardCode = dataReader.GetString(2);
                    wardEvaType.WardName = dataReader.GetString(3);
                    lstWardEvaTypes.Add(wardEvaType);
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
        /// 保存指定的一系列病区护理评价类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">护理评价类型ID</param>
        /// <param name="wardDocTypes">病区护理评价类型配置信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardEvaTypes(string szEvaTypeID, List<WardEvaType> wardEvaTypes)
        {
            if (wardEvaTypes == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = this.DeleteWardEvaTypes(szEvaTypeID);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            foreach (WardEvaType wardEvaType in wardEvaTypes)
            {
                if (wardEvaType == null)
                    continue;
                shRet = this.SaveWardEvaType(wardEvaType);
                if (shRet != ServerData.ExecuteResult.OK)
                    break;
            }
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
            }
            else
            {
                if (!base.DataAccess.CommitTransaction(true))
                    return ServerData.ExecuteResult.EXCEPTION;
            }
            return shRet;
        }

        /// <summary>
        /// 保存一条新的病区护理评价类型配置信息
        /// </summary>
        /// <param name="wardDocType">病区护理评价类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardEvaType(WardEvaType wardEvaType)
        {
            if (wardEvaType == null || GlobalMethods.Misc.IsEmptyString(wardEvaType.EvaTypeID))
            {
                LogManager.Instance.WriteLog("EvaTypeAccess.SaveWardEvaType", new string[] { "wardEvaType" }
                    , new object[] { wardEvaType }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardEvaTypeTable.EVATYPE_ID, ServerData.WardEvaTypeTable.EVATYPE_NAME
                , ServerData.WardEvaTypeTable.WARD_CODE, ServerData.WardEvaTypeTable.WARD_NAME);
            string szValue = string.Format("'{0}','{1}','{2}','{3}'"
                , wardEvaType.EvaTypeID, wardEvaType.EvaTypeName, wardEvaType.WardCode, wardEvaType.WardName);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.WARD_EVA_TYPE, szField, szValue);
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
                LogManager.Instance.WriteLog("EvaTypeAccess.SaveWardEvaType", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据病历类型代码删除当前护理评价类型对应的病区护理评价类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">护理评价类型ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteWardEvaTypes(string szEvaTypeID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'"
                , ServerData.WardEvaTypeTable.EVATYPE_ID, szEvaTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.WARD_EVA_TYPE, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.OTHER_ERROR;
        }
        #endregion
    }
}
