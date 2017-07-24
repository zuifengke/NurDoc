// ***********************************************************
// 数据库访问层文档类型字典数据访问类.
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
    public class DocTypeAccess : DBAccessBase
    {
        #region"病历类型数据访问接口"
        /// <summary>
        /// 获取所有文档类型信息列表
        /// </summary>
        /// <param name="szApplyEnv">表单应用环境</param>
        /// <param name="lstDocTypeInfos">文档类型信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocTypeInfos(string szApplyEnv, ref List<DocTypeInfo> lstDocTypeInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA, ServerData.DocTypeTable.IS_QC_VISIBLE);

            string szSQL = null;
            if (GlobalMethods.Misc.IsEmptyString(szApplyEnv))
            {
                szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField
                    , ServerData.DataTable.DOC_TYPE, ServerData.DocTypeTable.DOCTYPE_NO);
            }
            else
            {
                string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.APPLY_ENV, szApplyEnv);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                    , ServerData.DataTable.DOC_TYPE, szCondition, ServerData.DocTypeTable.DOCTYPE_NO);
            }
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocTypeInfos == null)
                    lstDocTypeInfos = new List<DocTypeInfo>();
                do
                {
                    DocTypeInfo docTypeInfo = new DocTypeInfo();
                    docTypeInfo.DocTypeID = dataReader.GetString(0);
                    docTypeInfo.DocTypeName = dataReader.GetString(1);
                    docTypeInfo.ApplyEnv = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        docTypeInfo.DocTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4))
                        docTypeInfo.IsRepeated = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        docTypeInfo.IsValid = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6))
                        docTypeInfo.IsVisible = dataReader.GetValue(6).ToString().Equals("1");
                    if (!dataReader.IsDBNull(7))
                        docTypeInfo.PrintMode = (FormPrintMode)int.Parse(dataReader.GetValue(7).ToString());
                    if (!dataReader.IsDBNull(8))
                        docTypeInfo.IsFolder = dataReader.GetValue(8).ToString().Equals("1");
                    if (!dataReader.IsDBNull(9))
                        docTypeInfo.ParentID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10))
                        docTypeInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11))
                        docTypeInfo.SortColumn = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12))
                        docTypeInfo.SortMode = (SortMode)int.Parse(dataReader.GetValue(12).ToString());
                    if (!dataReader.IsDBNull(13))
                        docTypeInfo.SchemaID = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        docTypeInfo.IsQCVisible = dataReader.GetValue(14).ToString().Equals("1");
                    lstDocTypeInfos.Add(docTypeInfo);
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
        /// 根据指定文档类型代码获取文档类型信息
        /// </summary>
        /// <param name="szDocTypeID">文档类型代码</param>
        /// <param name="docTypeInfo">文档类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocTypeInfo(string szDocTypeID, ref DocTypeInfo docTypeInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.GetDocTypeInfo", new string[] { "szDocTypeID" }
                    , new object[] { szDocTypeID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocTypeID = szDocTypeID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA, ServerData.DocTypeTable.IS_QC_VISIBLE);
            string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataTable.DOC_TYPE, szCondition, ServerData.DocTypeTable.DOCTYPE_NO);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocTypeAccess.GetDocTypeInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "记录不存在!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (docTypeInfo == null)
                    docTypeInfo = new DocTypeInfo();
                docTypeInfo.DocTypeID = dataReader.GetString(0);
                docTypeInfo.DocTypeName = dataReader.GetString(1);
                docTypeInfo.ApplyEnv = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    docTypeInfo.DocTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                if (!dataReader.IsDBNull(4))
                    docTypeInfo.IsRepeated = dataReader.GetValue(4).ToString().Equals("1");
                if (!dataReader.IsDBNull(5))
                    docTypeInfo.IsValid = dataReader.GetValue(5).ToString().Equals("1");
                if (!dataReader.IsDBNull(6))
                    docTypeInfo.IsVisible = dataReader.GetValue(6).ToString().Equals("1");
                if (!dataReader.IsDBNull(7))
                    docTypeInfo.PrintMode = (FormPrintMode)int.Parse(dataReader.GetValue(7).ToString());
                if (!dataReader.IsDBNull(8))
                    docTypeInfo.IsFolder = dataReader.GetValue(8).ToString().Equals("1");
                if (!dataReader.IsDBNull(9))
                    docTypeInfo.ParentID = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10))
                    docTypeInfo.ModifyTime = dataReader.GetDateTime(10);
                if (!dataReader.IsDBNull(11))
                    docTypeInfo.SortColumn = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12))
                    docTypeInfo.SortMode = (SortMode)int.Parse(dataReader.GetValue(12).ToString());
                if (!dataReader.IsDBNull(13))
                    docTypeInfo.SchemaID = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14))
                    docTypeInfo.IsQCVisible = dataReader.GetValue(14).ToString().Equals("1");
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
        public short SaveDocTypeInfo(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || GlobalMethods.Misc.IsEmptyString(docTypeInfo.DocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.SaveDocTypeInfo", new string[] { "docTypeInfo" }
                    , new object[] { docTypeInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA,ServerData.DocTypeTable.IS_QC_VISIBLE);
            string szValue = string.Format("'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8},'{9}',{10},'{11}','{12}','{13}',{14}"
                , docTypeInfo.DocTypeID, docTypeInfo.DocTypeName, docTypeInfo.ApplyEnv, docTypeInfo.DocTypeNo
                , docTypeInfo.IsRepeated ? "1" : "0", docTypeInfo.IsValid ? "1" : "0"
                , docTypeInfo.IsVisible ? "1" : "0", ((int)docTypeInfo.PrintMode).ToString(), docTypeInfo.IsFolder ? "1" : "0"
                , docTypeInfo.ParentID, base.DataAccess.GetSystemTimeSql(), docTypeInfo.SortColumn, ((int)docTypeInfo.SortMode).ToString()
                , docTypeInfo.SchemaID,docTypeInfo.IsQCVisible ?"1":"0");
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.DOC_TYPE, szField, szValue);
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
                LogManager.Instance.WriteLog("DocTypeAccess.SaveDocTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据指定文档类型代码更新文档类型信息
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID号</param>
        /// <param name="docTypeInfo">文档类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyDocTypeInfo(string szDocTypeID, DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.ModifyDocTypeInfo", new string[] { "docTypeInfo" }
                    , new object[] { docTypeInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocTypeID = szDocTypeID.Trim();
            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}={7},{8}={9},{10}={11},{12}={13},{14}={15},{16}={17},{18}='{19}',{20}={21},{22}='{23}',{24}={25},{26}='{27}',{28}={29}"
                , ServerData.DocTypeTable.DOCTYPE_ID, docTypeInfo.DocTypeID
                , ServerData.DocTypeTable.DOCTYPE_NAME, docTypeInfo.DocTypeName
                , ServerData.DocTypeTable.APPLY_ENV, docTypeInfo.ApplyEnv
                , ServerData.DocTypeTable.DOCTYPE_NO, docTypeInfo.DocTypeNo.ToString()
                , ServerData.DocTypeTable.IS_REPEATED, docTypeInfo.IsRepeated ? "1" : "0"
                , ServerData.DocTypeTable.IS_VALID, docTypeInfo.IsValid ? "1" : "0"
                , ServerData.DocTypeTable.IS_VISIBLE, docTypeInfo.IsVisible ? "1" : "0"
                , ServerData.DocTypeTable.PRINT_MODE, ((int)docTypeInfo.PrintMode).ToString()
                , ServerData.DocTypeTable.IS_FOLDER, docTypeInfo.IsFolder ? "1" : "0"
                , ServerData.DocTypeTable.PARENT_ID, docTypeInfo.ParentID
                , ServerData.DocTypeTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql()
                , ServerData.DocTypeTable.SORT_COLUMN, docTypeInfo.SortColumn
                , ServerData.DocTypeTable.SORT_MODE, ((int)docTypeInfo.SortMode).ToString()
                , ServerData.DocTypeTable.SCHEMA, docTypeInfo.SchemaID
                , ServerData.DocTypeTable.IS_QC_VISIBLE, docTypeInfo.IsQCVisible ? "1" : "0");
            string szCondiction = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.DOC_TYPE, szField, szCondiction);

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
                LogManager.Instance.WriteLog("DocTypeAccess.ModifyDocTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的一系列的病历类型
        /// </summary>
        /// <param name="lstDocTypeID">病历类型ID列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteDocTypeInfos(List<string> lstDocTypeID)
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
                shRet = this.DeleteDocTypeInfo(szDocTypeID);
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
        /// 根据病历类型ID,删除一条病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteDocTypeInfo(string szDocTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.DeleteDocTypeInfo", new string[] { "szDocTypeID" },
                    new object[] { szDocTypeID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.DOC_TYPE, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            this.DeleteWardDocTypes(szDocTypeID);        //删除病区病历类型
            this.DeleteGridViewColumns(szDocTypeID);      //删除病历类型模板摘要列
            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }

        /// <summary>
        /// 删除指定表格模板包含的所有列
        /// </summary>
        /// <param name="szSchemaID">表格模板ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteGridViewColumns(string szSchemaID)
        {
            if (string.IsNullOrEmpty(szSchemaID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'"
                , ServerData.GridViewColumnTable.SCHEMA_ID, szSchemaID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.GRID_VIEW_COLUMN, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }
        #endregion

        #region"病区病历类型配置数据访问接口"
        /// <summary>
        /// 获取所有文档类型代码应用到的病区列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstWardDocTypes">病区病历类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWardDocTypeList(string szWardCode, ref List<WardDocType> lstWardDocTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, ServerData.WardDocTypeTable.DOCTYPE_NAME
                , ServerData.WardDocTypeTable.WARD_CODE, ServerData.WardDocTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardDocTypeTable.WARD_CODE, szWardCode);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_DOC_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_DOC_TYPE);
            }

            if (lstWardDocTypes == null)
                lstWardDocTypes = new List<WardDocType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardDocType wardDocType = new WardDocType();
                    wardDocType.DocTypeID = dataReader.GetString(0);
                    wardDocType.DocTypeName = dataReader.GetString(1);
                    wardDocType.WardCode = dataReader.GetString(2);
                    wardDocType.WardName = dataReader.GetString(3);
                    lstWardDocTypes.Add(wardDocType);
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
        public short GetDocTypeDeptList(string szDocTypeID, ref List<WardDocType> lstWardDocTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, ServerData.WardDocTypeTable.DOCTYPE_NAME
                , ServerData.WardDocTypeTable.WARD_CODE, ServerData.WardDocTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardDocTypeTable.DOCTYPE_ID, szDocTypeID);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_DOC_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_DOC_TYPE);
            }

            if (lstWardDocTypes == null)
                lstWardDocTypes = new List<WardDocType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardDocType wardDocType = new WardDocType();
                    wardDocType.DocTypeID = dataReader.GetString(0);
                    wardDocType.DocTypeName = dataReader.GetString(1);
                    wardDocType.WardCode = dataReader.GetString(2);
                    wardDocType.WardName = dataReader.GetString(3);
                    lstWardDocTypes.Add(wardDocType);
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
        /// 保存指定的一系列病区病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="wardDocTypes">病区病历类型配置信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardDocTypes(string szDocTypeID, List<WardDocType> wardDocTypes)
        {
            if (wardDocTypes == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = this.DeleteWardDocTypes(szDocTypeID);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            foreach (WardDocType wardDocType in wardDocTypes)
            {
                if (wardDocType == null)
                    continue;
                shRet = this.SaveWardDocType(wardDocType);
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
        /// 保存一条新的病区病历类型配置信息
        /// </summary>
        /// <param name="wardDocType">病区病历类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardDocType(WardDocType wardDocType)
        {
            if (wardDocType == null || GlobalMethods.Misc.IsEmptyString(wardDocType.DocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.SaveWardDocType", new string[] { "wardDocType" }
                    , new object[] { wardDocType }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, ServerData.WardDocTypeTable.DOCTYPE_NAME
                , ServerData.WardDocTypeTable.WARD_CODE, ServerData.WardDocTypeTable.WARD_NAME);
            string szValue = string.Format("'{0}','{1}','{2}','{3}'"
                , wardDocType.DocTypeID, wardDocType.DocTypeName, wardDocType.WardCode, wardDocType.WardName);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.WARD_DOC_TYPE, szField, szValue);
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
                LogManager.Instance.WriteLog("DocTypeAccess.SaveWardDocType", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据病历类型代码删除当前病历类型对应的病区病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteWardDocTypes(string szDocTypeID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.WARD_DOC_TYPE, szCondition);

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
