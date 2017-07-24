// ***********************************************************
// 数据库访问层与护理信息库内容有关的数据的访问类.
// Creator:YangMingkun  Date:2013-11-07
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;
using Heren.Common.Libraries.Ftp;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class InfoLibAccess : DBAccessBase
    {
        #region"护理信息库读写接口"
        /// <summary>
        /// 获取指定的护理信息
        /// </summary>
        /// <param name="szInfoID">信息ID</param>
        /// <param name="byteTempletData">模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInfoLib(string szInfoID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szInfoID))
            {
                LogManager.Instance.WriteLog("TempletAccess.GetTextTemplet", new string[] { "szTempletID" }, new object[] { szInfoID }, "模板ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.INFO_ID, szInfoID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.InfoLibTable.INFO_DATA, ServerData.DataTable.INFO_LIB, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetInfoLib", new string[] { "szSQL" }, new object[] { szSQL }, "没有查询到记录!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                return GlobalMethods.IO.GetBytes(dataReader, 0, ref byteTempletData)
                    ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定ID号的文档信息
        /// </summary>
        /// <param name="szInfoID">文档ID</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInfoLibInfos(string szInfoID, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.INFO_ID, szInfoID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.INFO_NAME);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, true, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 保存护理信息内容到服务器
        /// </summary>
        /// <param name="infoLibInfo">护理信息</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveInfoLib(InfoLibInfo infoLibInfo, byte[] byteTempletData)
        {
            if (infoLibInfo == null)
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveInfoLib", new string[] { "textTempletInfo" }, new object[] { infoLibInfo }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.InfoLibTable.INFO_ID, ServerData.InfoLibTable.INFO_NAME
                , ServerData.InfoLibTable.CREATOR_ID, ServerData.InfoLibTable.CREATOR_NAME
                , ServerData.InfoLibTable.CREATE_TIME, ServerData.InfoLibTable.MODIFY_TIME
                , ServerData.InfoLibTable.SHARE_LEVEL, ServerData.InfoLibTable.WARD_CODE
                , ServerData.InfoLibTable.WARD_NAME, ServerData.InfoLibTable.PARENT_ID
                , ServerData.InfoLibTable.IS_FOLDER, ServerData.InfoLibTable.INFO_TYPE
                , ServerData.InfoLibTable.INFO_SIZE);

            string szValue = string.Format("'{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}'"
                , infoLibInfo.InfoID, infoLibInfo.InfoName, infoLibInfo.CreatorID, infoLibInfo.CreatorName
                , base.DataAccess.GetSqlTimeFormat(infoLibInfo.CreateTime), base.DataAccess.GetSystemTimeSql()
                , infoLibInfo.ShareLevel, infoLibInfo.WardCode, infoLibInfo.WardName, infoLibInfo.ParentID
                , infoLibInfo.IsFolder ? 1 : 0, infoLibInfo.InfoType
                , infoLibInfo.InfoSize);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}", szField, ServerData.InfoLibTable.INFO_DATA);
                szValue = string.Format("{0},{1}", szValue, base.DataAccess.GetSqlParamName("InfoData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("InfoData", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.INFO_LIB, szField, szValue);

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
                LogManager.Instance.WriteLog("TempletAccess.SaveInfoLib", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新护理信息内容到服务器
        /// </summary>
        /// <param name="infoLibInfo">护理信息内容</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateInfoLib(InfoLibInfo infoLibInfo, byte[] byteTempletData)
        {
            if (infoLibInfo == null)
            {
                LogManager.Instance.WriteLog("TempletAccess.UpdateInfoLib", new string[] { "infoLibInfo" }, new object[] { infoLibInfo }
                    , "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}={1} , {2} = '{3}'"
                , ServerData.InfoLibTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql()
                , ServerData.InfoLibTable.INFO_SIZE,infoLibInfo.InfoSize);
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.INFO_ID, infoLibInfo.InfoID);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}={2}", szField
                    , ServerData.InfoLibTable.INFO_DATA, base.DataAccess.GetSqlParamName("InfoData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("InfoData", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.INFO_LIB, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.UpdateInfoLib", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新护理信息文档保存到FTP文档库
        /// </summary>
        /// <param name="infoLibInfo">护理信息</param>
        /// <param name="szFilePath">文档路径</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateInfoLibToFtp(InfoLibInfo infoLibInfo, string szFilePath)
        {
            if (this.InfoLibFtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!this.InfoLibFtpAccess.OpenConnection())
            {
                this.InfoLibFtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }
            try
            {
                string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(infoLibInfo);
                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!this.InfoLibFtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;

                if (!this.InfoLibFtpAccess.Upload(szFilePath, szRemoteFile))
                    return ServerData.ExecuteResult.EXCEPTION;

                short shRet = this.UpdateInfoLib(infoLibInfo, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    return shRet;
                }
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), infoLibInfo.ToString(), "文件更新保存失败!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
            }
        }

        /// <summary>
        /// 修改指定的护理文本模板共享等级
        /// </summary>
        /// <param name="szInfoID">护理信息ID</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyInfoLibShareLevel(string szInfoID, string szShareLevel)
        {
            if (GlobalMethods.Misc.IsEmptyString(szInfoID) || GlobalMethods.Misc.IsEmptyString(szShareLevel))
            {
                LogManager.Instance.WriteLog("TempletAccess.ModifyTextTempletShareLevel", new string[] { "szInfoID", "szShareLevel" }
                    , new object[] { szInfoID, szShareLevel }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.TextTempletTable.SHARE_LEVEL, szShareLevel
                , ServerData.InfoLibTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.INFO_ID, szInfoID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.INFO_LIB, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.ModifyInfoLibShareLevel", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改指定的护理信息内容共享等级（批量）
        /// </summary>
        /// <param name="lstInfoID">护理信息ID列表</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyInfoLibShareLevel(List<string> lstInfoID, string szShareLevel)
        {
            if (lstInfoID == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = ServerData.ExecuteResult.OK;
            for (int index = 0; index < lstInfoID.Count; index++)
            {
                string szInfoID = lstInfoID[index];
                if (GlobalMethods.Misc.IsEmptyString(szInfoID))
                    continue;
                shRet = this.ModifyInfoLibShareLevel(szInfoID, szShareLevel);
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
        /// 修改指定的护理文本模板父目录
        /// </summary>
        /// <param name="szInfoID">护理信息ID</param>
        /// <param name="szParentID">模板新的父目录ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyInfoLibParentID(string szInfoID, string szParentID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szInfoID))
            {
                LogManager.Instance.WriteLog("TempletAccess.ModifyInfoLibParentID", new string[] { "szInfoID" }, new object[] { szInfoID }
                    , "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.InfoLibTable.PARENT_ID, szParentID
                , ServerData.InfoLibTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.INFO_ID, szInfoID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.INFO_LIB, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.ModifyInfoLibParentID", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改指定的护理信息名称
        /// </summary>
        /// <param name="szInfoID">护理信息ID</param>
        /// <param name="szInfoName">模板新的名称</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyInfoLibName(string szInfoID, string szInfoName)
        {
            if (GlobalMethods.Misc.IsEmptyString(szInfoID) || GlobalMethods.Misc.IsEmptyString(szInfoName))
            {
                LogManager.Instance.WriteLog("TempletAccess.ModifyInfoLibName", new string[] { "szInfoID", "szInfoName" }
                    , new object[] { szInfoID, szInfoName }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.InfoLibTable.INFO_NAME, szInfoName
                , ServerData.InfoLibTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.INFO_ID, szInfoID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.INFO_LIB, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.ModifyTextTempletName", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的护理信息内容
        /// </summary>
        /// <param name="szInfoID">护理信息ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteInfoLib(string szInfoID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.INFO_ID, szInfoID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.INFO_LIB, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.DeleteInfoLib", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的一系列护理文本模板
        /// </summary>
        /// <param name="lstInfoID">要删除的护理信息ID列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteInfoLib(List<string> lstInfoID)
        {
            if (lstInfoID == null || lstInfoID.Count <= 0)
                return ServerData.ExecuteResult.OK;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = ServerData.ExecuteResult.OK;
            for (int index = 0; index < lstInfoID.Count; index++)
            {
                shRet = this.DeleteInfoLib(lstInfoID[index]);
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
        /// 根据指定的查询条件获取对应的护理信息列表
        /// </summary>
        /// <param name="szCondition">查询条件</param>
        /// <param name="szOrderSQL">排序子SQL</param>
        /// <param name="bContainsData">是否包含数据</param>
        /// <param name="lstInfoLibs">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetInfoLibInfosInternal(string szCondition, string szOrderSQL, bool bContainsData
            , ref List<InfoLibInfo> lstInfoLibs)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.InfoLibTable.INFO_ID, ServerData.InfoLibTable.INFO_NAME
                , ServerData.InfoLibTable.CREATOR_ID, ServerData.InfoLibTable.CREATOR_NAME
                , ServerData.InfoLibTable.CREATE_TIME, ServerData.InfoLibTable.MODIFY_TIME
                , ServerData.InfoLibTable.SHARE_LEVEL, ServerData.InfoLibTable.WARD_CODE
                , ServerData.InfoLibTable.WARD_NAME, ServerData.InfoLibTable.PARENT_ID
                , ServerData.InfoLibTable.IS_FOLDER, ServerData.InfoLibTable.STATUS
                , ServerData.InfoLibTable.INFO_TYPE, ServerData.InfoLibTable.INFO_SIZE);

            if (bContainsData)
            {
                szField = string.Format("{0},{1}", szField, ServerData.InfoLibTable.INFO_DATA);
            }

            string szSQL = string.Empty;
            if (GlobalMethods.Misc.IsEmptyString(szCondition))
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.INFO_LIB);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.INFO_LIB, szCondition);

            if (!GlobalMethods.Misc.IsEmptyString(szOrderSQL))
                szSQL = string.Concat(szSQL, " ", szOrderSQL);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstInfoLibs == null)
                    lstInfoLibs = new List<InfoLibInfo>();
                do
                {
                    InfoLibInfo infoLibInfo = new InfoLibInfo();
                    infoLibInfo.InfoID = dataReader.GetString(0);
                    infoLibInfo.InfoName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        infoLibInfo.CreatorID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        infoLibInfo.CreatorName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4))
                        infoLibInfo.CreateTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5))
                        infoLibInfo.ModifyTime = dataReader.GetDateTime(5);
                    infoLibInfo.ShareLevel = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        infoLibInfo.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8))
                        infoLibInfo.WardName = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9))
                        infoLibInfo.ParentID = dataReader.GetString(9);
                    infoLibInfo.IsFolder = dataReader.GetValue(10).ToString().Equals("1");
                    if (!dataReader.IsDBNull(11))
                        infoLibInfo.Status = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12))
                        infoLibInfo.InfoType = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13))
                        infoLibInfo.InfoSize = dataReader.GetString(13);
                    if (!infoLibInfo.IsFolder && bContainsData)
                    {
                        byte[] byteTempletData = null;
                        string szTempletData = string.Empty;
                        GlobalMethods.IO.GetBytes(dataReader, 11, ref byteTempletData);
                        GlobalMethods.Convert.BytesToString(byteTempletData, ref szTempletData);
                        infoLibInfo.InfoData = szTempletData;
                    }
                    lstInfoLibs.Add(infoLibInfo);
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
        /// 获取指定目录ID的护理信息列表
        /// </summary>
        /// <param name="szParentID">目录ID</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetChildInfoLibInfos(string szParentID, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.PARENT_ID, szParentID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.INFO_NAME);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, true, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 获取所有护理信息列表
        /// </summary>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAllLibInfosFolder(ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.IS_FOLDER, "1");
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.INFO_NAME);
            return this.GetInfoLibInfosInternal(string.Empty, string.Empty, true, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 获取指定目录ID的护理信息列表
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szParentID">病人ID</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPersonalChildInfoLibInfos(string szUserID, string szParentID, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}' and {2}='{3}'"
                , ServerData.InfoLibTable.PARENT_ID, szParentID
                , ServerData.InfoLibTable.CREATOR_ID, szUserID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.INFO_NAME);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, true, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 获取指定用户ID的护理信息列表
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPersonalInfoLibInfos(string szUserID, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.CREATOR_ID, szUserID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.PARENT_ID);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, false, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 根据标题检索文档
        /// </summary>
        /// <param name="szInfoName">关键字</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInfoLibInfosByName(string szInfoName, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0} like '%{1}%'"
                , ServerData.InfoLibTable.INFO_NAME, szInfoName);
            string szOrderSQL = string.Format("ORDER BY {0} DESC", ServerData.InfoLibTable.CREATE_TIME);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, false, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 获取指定病区的护理信息列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="bOnlyDeptShare">是否仅返回标记为病区共享的护理信息列表</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDeptInfoLibInfos(string szWardCode, bool bOnlyDeptShare, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.InfoLibTable.WARD_CODE, szWardCode);
            if (bOnlyDeptShare)
            {
                szCondition = string.Format("{0} AND ({1}='{2}' OR {3}=1)", szCondition
                    , ServerData.InfoLibTable.SHARE_LEVEL, ServerData.ShareLevel.DEPART, ServerData.InfoLibTable.IS_FOLDER);
            }
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.PARENT_ID);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, false, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 获取指定病区的护理信息列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="bOnlyDeptShare">是否仅返回标记为病区共享的护理信息列表</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDeptChildInfoLibInfos(string szPatientID,string szWardCode, bool bOnlyDeptShare, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}' And {2}='{3}'", ServerData.InfoLibTable.WARD_CODE, szWardCode
                ,ServerData.InfoLibTable.PARENT_ID,szPatientID);
            if (bOnlyDeptShare)
            {
                szCondition = string.Format("{0} AND ({1}='{2}' OR {3}=1)", szCondition
                    , ServerData.InfoLibTable.SHARE_LEVEL, ServerData.ShareLevel.DEPART, ServerData.InfoLibTable.IS_FOLDER);
            }
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.PARENT_ID);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, false, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 获取全院护理信息内容列表
        /// </summary>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetHospitalInfoLibInfos(ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}' "
                , ServerData.InfoLibTable.SHARE_LEVEL, ServerData.ShareLevel.HOSPITAL
                , ServerData.InfoLibTable.IS_FOLDER);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.PARENT_ID);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, false, ref lstInfoLibInfos);
        }

        /// <summary>
        /// 获取全院护理信息内容列表
        /// </summary>
        /// <param name="szParentID">病人ID</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetHospitalChildInfoLibInfos(string szParentID,ref List<InfoLibInfo> lstInfoLibInfos)
        {
            string szCondition = string.Format("{0}='{1}' And {2}='{3}'"
                , ServerData.InfoLibTable.SHARE_LEVEL, ServerData.ShareLevel.HOSPITAL
                , ServerData.InfoLibTable.PARENT_ID,szParentID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.InfoLibTable.PARENT_ID);
            return this.GetInfoLibInfosInternal(szCondition, szOrderSQL, false, ref lstInfoLibInfos);
        }
        #endregion

        #region 数据Ftp存储访问
        private FtpAccess m_InfoLibFtpAccess = null;

        /// <summary>
        /// 获取FTP服务器访问对象实例
        /// </summary>
        protected FtpAccess InfoLibFtpAccess
        {
            get
            {
                if (this.m_InfoLibFtpAccess == null)
                    this.m_InfoLibFtpAccess = ServerParam.Instance.GetInfoLibFtpAccess();
                return this.m_InfoLibFtpAccess;
            }
        }

        /// <summary>
        /// 根据文件名字从FTP中下载文件
        /// </summary>
        /// <param name="strRemoteFileName">服务器上的文件名称</param>
        /// <param name="strLocalFileName">本地的文件名称</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DownloadInfoLibFile(string strRemoteFileName, string strLocalFileName)
        {
            if (this.InfoLibFtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!this.InfoLibFtpAccess.OpenConnection())
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (!this.InfoLibFtpAccess.Download(strRemoteFileName, strLocalFileName))
            {
                this.InfoLibFtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }
            this.InfoLibFtpAccess.CloseConnection();
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 把护理信息文档保存到FTP文档库
        /// </summary>
        /// <param name="infoLibInfo">文档信息</param>
        /// <param name="szFilePath">文档路径</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveInfoLibToFTP(InfoLibInfo infoLibInfo, string szFilePath)
        {
            if (this.InfoLibFtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!this.InfoLibFtpAccess.OpenConnection())
            {
                this.InfoLibFtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }
            try
            {
                string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(infoLibInfo);
                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!this.InfoLibFtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;

                if (!this.InfoLibFtpAccess.Upload(szFilePath, szRemoteFile))
                    return ServerData.ExecuteResult.EXCEPTION;
            
                short shRet = this.SaveInfoLib(infoLibInfo, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    this.InfoLibFtpAccess.DeleteFile(szRemoteFile);
                    return shRet;
                }
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), infoLibInfo.ToString(), "文件上传保存失败!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
            }
        }

        /// <summary>
        /// 删除FTP文档库护理信息文档
        /// </summary>
        /// <param name="infoLibInfo">文档信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteInfoLibToFTP(InfoLibInfo infoLibInfo)
        {
            if (this.InfoLibFtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!this.InfoLibFtpAccess.OpenConnection())
            {
                this.InfoLibFtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }
            try
            {
                string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(infoLibInfo);
                if (this.InfoLibFtpAccess.ResExists(szRemoteFile, false))
                {
                    if (!this.InfoLibFtpAccess.DeleteFile(szRemoteFile))
                        return ServerData.ExecuteResult.EXCEPTION;
                }
                short shRet = this.DeleteInfoLib(infoLibInfo.InfoID);
                if (shRet != ServerData.ExecuteResult.OK)
                    return shRet;
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), infoLibInfo.ToString(), "文件删除失败!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
            }
        }
        #endregion
    }
}
