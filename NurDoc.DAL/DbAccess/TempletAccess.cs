// ***********************************************************
// 数据库访问层与各种病历模板有关的数据的访问类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class TempletAccess : DBAccessBase
    {
        #region"表单模板读写接口"
        /// <summary>
        /// 生成系统文档模板路径
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="szTempletPath">文档模板路径</param>
        /// <returns>short</returns>
        private short MakeFormTempletPath(string szDocTypeID, ref string szTempletPath)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("TempletAccess.MakeFormTempletPath", new string[] { "szDocTypeID" }
                        , new object[] { szDocTypeID }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            szTempletPath = string.Format("/TEMPLET/SYSTEM/{0}.{1}", szDocTypeID.Trim(), ServerData.FileExt.NUR_TEMPLET);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取系统自带的指定文档类型的文档模板内容
        /// </summary>
        /// <param name="szDocTypeID">文档类型代码</param>
        /// <param name="byteTempletData">模板二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetFormTemplet(string szDocTypeID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("TempletAccess.GetFormTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "文档类型ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("TempletAccess.GetFormTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            //    if (base.StorageMode == StorageMode.DB)
            return this.GetFormTempletFromDB(szDocTypeID, ref byteTempletData);
            //  else
            //     return this.GetFormTempletFromFTP(szDocTypeID, ref byteTempletData);
        }

        /// <summary>
        /// 获取所有模版
        /// </summary>
        /// <param name="lstDocTypeDatas">模版信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetFormTemplet(ref List<DocTypeData> lstDocTypeDatas)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("TempletAccess.GetFormTemplet", "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            //if (base.StorageMode == StorageMode.DB)
            return this.GetFormTempletFromDB(ref lstDocTypeDatas);
            //else
            //    return this.GetFormTempletFromFTP(ref lstDocTypeDatas);
        }

        /// <summary>
        /// 获取从数据库所有模版文件
        /// </summary>
        /// <param name="lstDocTypeDatas">模版列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetFormTempletFromDB(ref List<DocTypeData> lstDocTypeDatas)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.TEMPLET_DATA);
            string szCondiction = string.Format("{0}='{1}'", ServerData.DocTypeTable.IS_FOLDER, "0");
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField
                            , ServerData.DataTable.DOC_TYPE, szCondiction);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocTypeDatas == null)
                    lstDocTypeDatas = new List<DocTypeData>();
                do
                {
                    DocTypeData DocTypeData = new DocTypeData();
                    DocTypeData.DocTypeID = dataReader.GetString(0);
                    DocTypeData.DocTypeName = dataReader.GetString(1);
                    DocTypeData.ApplyEnv = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        DocTypeData.DocTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4))
                        DocTypeData.IsRepeated = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        DocTypeData.IsValid = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6))
                        DocTypeData.IsVisible = dataReader.GetValue(6).ToString().Equals("1");
                    if (!dataReader.IsDBNull(7))
                        DocTypeData.PrintMode = (FormPrintMode)int.Parse(dataReader.GetValue(7).ToString());
                    if (!dataReader.IsDBNull(8))
                        DocTypeData.IsFolder = dataReader.GetValue(8).ToString().Equals("1");
                    if (!dataReader.IsDBNull(9))
                        DocTypeData.ParentID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10))
                        DocTypeData.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11))
                        DocTypeData.SortColumn = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12))
                        DocTypeData.SortMode = (SortMode)int.Parse(dataReader.GetValue(12).ToString());
                    if (!dataReader.IsDBNull(13))
                    {
                        byte[] ByteTempletData = null;
                        GlobalMethods.IO.GetBytes(dataReader, 13, ref ByteTempletData);
                        DocTypeData.ByteTempletData = ByteTempletData;
                    }
                    lstDocTypeDatas.Add(DocTypeData);
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
        /// 根据文档类型代码从DB中获取系统文档模板内容
        /// </summary>
        /// <param name="szDocTypeID">文档类型编号</param>
        /// <param name="byteTempletData">系统文档模板二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetFormTempletFromDB(string szDocTypeID, ref byte[] byteTempletData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.DocTypeTable.TEMPLET_DATA
                , ServerData.DataTable.DOC_TYPE, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetFormTempletFromDB", new string[] { "szSQL" }, new object[] { szSQL }, "没有查询到记录!");
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
        /// 从FTP服务器中获取所有模版文件
        /// </summary>
        /// <param name="lstDocTypeDatas">模版列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetFormTempletFromFTP(ref List<DocTypeData> lstDocTypeDatas)
        {
            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = this.GetFormTempletFromDB(ref lstDocTypeDatas);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }

            for (int index = 0; index < lstDocTypeDatas.Count; index++)
            {
                string szRemoteFile = null;
                shRet = this.MakeFormTempletPath(lstDocTypeDatas[index].DocTypeID, ref szRemoteFile);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetFormTempletFromFTP", new string[] { "DocTypeData.DocTypeID" }
                    , new object[] { lstDocTypeDatas[index].DocTypeID }, "MakeFormTempletPath执行失败!");
                    continue;
                }

                string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                    , lstDocTypeDatas[index].DocTypeID, ServerData.FileExt.NUR_TEMPLET);
                if (!base.FtpAccess.Download(szRemoteFile, szCacheFile))
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetFormTempletFromFTP", new string[] { "szRemoteFile" }
                    , new object[] { szRemoteFile }, "FtpAccess.Download执行失败!");
                    continue;
                }

                byte[] byteTempletData = null;
                if (!GlobalMethods.IO.GetFileBytes(szCacheFile, ref byteTempletData))
                {
                    GlobalMethods.IO.DeleteFile(szCacheFile);
                    LogManager.Instance.WriteLog("TempletAccess.GetFormTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytes执行失败!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }
                lstDocTypeDatas[index].ByteTempletData = byteTempletData;
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
            base.FtpAccess.CloseConnection();
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据文档类型代码从FTP中获取系统文档模板内容
        /// </summary>
        /// <param name="szDocTypeID">文档类型编号</param>
        /// <param name="byteTempletData">系统文档模板二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetFormTempletFromFTP(string szDocTypeID, ref byte[] byteTempletData)
        {
            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
                return ServerData.ExecuteResult.EXCEPTION;

            string szRemoteFile = null;
            short shRet = this.MakeFormTempletPath(szDocTypeID, ref szRemoteFile);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.FtpAccess.CloseConnection();
                return shRet;
            }

            string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                , szDocTypeID, ServerData.FileExt.NUR_TEMPLET);
            if (!base.FtpAccess.Download(szRemoteFile, szCacheFile))
            {
                base.FtpAccess.CloseConnection();
                return shRet;
            }

            if (!GlobalMethods.IO.GetFileBytes(szCacheFile, ref byteTempletData))
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
                LogManager.Instance.WriteLog("TempletAccess.GetFormTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytes执行失败!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            base.FtpAccess.CloseConnection();
            GlobalMethods.IO.DeleteFile(szCacheFile);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存系统缺省模板到服务器
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveFormTemplet(string szDocTypeID, byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveFormTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "文档类型ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveFormTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            // if (base.StorageMode == StorageMode.DB)
            return this.SaveFormTempletToDB(szDocTypeID, byteTempletData);
            // else
            //     return this.SaveFormTempletToFTP(szDocTypeID, byteTempletData);
        }

        /// <summary>
        /// 保存系统缺省模板到数据库服务器
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveFormTempletToDB(string szDocTypeID, byte[] byteTempletData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}={1}", ServerData.DocTypeTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}={2}", szField, ServerData.DocTypeTable.TEMPLET_DATA, base.DataAccess.GetSqlParamName("TempletData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("TempletData", byteTempletData);
            }

            string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.DOC_TYPE, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.SaveFormTempletToDB", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存系统缺省模板到FTP服务器
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveFormTempletToFTP(string szDocTypeID, byte[] byteTempletData)
        {
            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
            {
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                , szDocTypeID, ServerData.FileExt.NUR_TEMPLET);
            if (!GlobalMethods.IO.WriteFileBytes(szCacheFile, byteTempletData))
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveFormTempletToFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "WriteFileBytes执行失败!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            try
            {
                string szRemoteFile = null;
                short shRet = this.MakeFormTempletPath(szDocTypeID, ref szRemoteFile);
                if (shRet != ServerData.ExecuteResult.OK)
                    return shRet;

                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!base.FtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;

                if (!base.FtpAccess.Upload(szCacheFile, szRemoteFile))
                    return ServerData.ExecuteResult.EXCEPTION;

                shRet = this.SaveFormTempletToDB(szDocTypeID, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.FtpAccess.DeleteFile(szRemoteFile);
                    return shRet;
                }
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szDocTypeID, "SQL语句执行失败!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
        }
        #endregion

        #region"报表模板读写接口"
        /// <summary>
        /// 生成用户文档模板路径
        /// </summary>
        /// <param name="szTempletID">模板ID</param>
        /// <param name="szTempletPath">用户文档模板FTP路径</param>
        /// <returns>short</returns>
        private short MakeReportTempletPath(string szTempletID, ref string szTempletPath)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("TempletAccess.MakeReportTempletPath", new string[] { "szTempletID" }
                        , new object[] { szTempletID }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            szTempletPath = string.Format("/REPORT/SYSTEM/{0}.{1}", szTempletID.Trim(), ServerData.FileExt.REPORT_TEMPLET);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 通过ID获取报表类型信息
        /// </summary>
        /// <param name="szReportTypeID">文档类型ID</param>
        /// <param name="reportTypeInfo">报表类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTypeInfo(string szReportTypeID, ref ReportTypeInfo reportTypeInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.ReportTypeTable.REPORT_TYPE_ID, ServerData.ReportTypeTable.REPORT_TYPE_NAME
                , ServerData.ReportTypeTable.APPLY_ENV, ServerData.ReportTypeTable.REPORT_TYPE_NO
                , ServerData.ReportTypeTable.IS_VALID, ServerData.ReportTypeTable.IS_FOLDER
                , ServerData.ReportTypeTable.PARENT_ID, ServerData.ReportTypeTable.MODIFY_TIME);

            string szCondition = string.Format("{0}='{1}'", ServerData.ReportTypeTable.REPORT_TYPE_ID, szReportTypeID);

            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataTable.REPORT_TYPE, szCondition, ServerData.ReportTypeTable.REPORT_TYPE_NO);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (reportTypeInfo == null)
                    reportTypeInfo = new ReportTypeInfo();

                reportTypeInfo.ReportTypeID = dataReader.GetString(0);
                reportTypeInfo.ReportTypeName = dataReader.GetString(1);
                reportTypeInfo.ApplyEnv = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    reportTypeInfo.ReportTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                if (!dataReader.IsDBNull(4))
                    reportTypeInfo.IsValid = dataReader.GetValue(4).ToString().Equals("1");
                if (!dataReader.IsDBNull(5))
                    reportTypeInfo.IsFolder = dataReader.GetValue(5).ToString().Equals("1");
                if (!dataReader.IsDBNull(6))
                    reportTypeInfo.ParentID = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    reportTypeInfo.ModifyTime = dataReader.GetDateTime(7);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取所有报表类型信息列表
        /// </summary>
        /// <param name="szApplyEnv">报表应用环境</param>
        /// <param name="lstReportTypeInfos">报表类型信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTypeInfos(string szApplyEnv, ref List<ReportTypeInfo> lstReportTypeInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.ReportTypeTable.REPORT_TYPE_ID, ServerData.ReportTypeTable.REPORT_TYPE_NAME
                , ServerData.ReportTypeTable.APPLY_ENV, ServerData.ReportTypeTable.REPORT_TYPE_NO
                , ServerData.ReportTypeTable.IS_VALID, ServerData.ReportTypeTable.IS_FOLDER
                , ServerData.ReportTypeTable.PARENT_ID, ServerData.ReportTypeTable.MODIFY_TIME);

            string szSQL = null;
            if (GlobalMethods.Misc.IsEmptyString(szApplyEnv))
            {
                szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField
                    , ServerData.DataTable.REPORT_TYPE, ServerData.ReportTypeTable.REPORT_TYPE_NO);
            }
            else
            {
                string szCondition = string.Format("{0}='{1}'", ServerData.ReportTypeTable.APPLY_ENV, szApplyEnv);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                    , ServerData.DataTable.REPORT_TYPE, szCondition, ServerData.ReportTypeTable.REPORT_TYPE_NAME);
            }

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstReportTypeInfos == null)
                    lstReportTypeInfos = new List<ReportTypeInfo>();
                do
                {
                    ReportTypeInfo reportTypeInfo = new ReportTypeInfo();
                    reportTypeInfo.ReportTypeID = dataReader.GetString(0);
                    reportTypeInfo.ReportTypeName = dataReader.GetString(1);
                    reportTypeInfo.ApplyEnv = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        reportTypeInfo.ReportTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4))
                        reportTypeInfo.IsValid = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        reportTypeInfo.IsFolder = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6))
                        reportTypeInfo.ParentID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        reportTypeInfo.ModifyTime = dataReader.GetDateTime(7);
                    lstReportTypeInfos.Add(reportTypeInfo);
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
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstWardReportTypes">病区病历类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWardReportTypeList(string szWardCode, ref List<WardReportType> lstWardReportTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardReportTypeTable.REPORT_TYPE_ID, ServerData.WardReportTypeTable.REPORT_TYPE_NAME
                , ServerData.WardReportTypeTable.WARD_CODE, ServerData.WardReportTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardReportTypeTable.WARD_CODE, szWardCode);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_REPORT_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_REPORT_TYPE);
            }

            if (lstWardReportTypes == null)
                lstWardReportTypes = new List<WardReportType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardReportType wardReportType = new WardReportType();
                    wardReportType.ReportTypeID = dataReader.GetString(0);
                    wardReportType.ReportTypeName = dataReader.GetString(1);
                    wardReportType.WardCode = dataReader.GetString(2);
                    wardReportType.WardName = dataReader.GetString(3);
                    lstWardReportTypes.Add(wardReportType);
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
        /// 获取报告文档模板内容
        /// </summary>
        /// <param name="szTempletID">报表模板ID</param>
        /// <param name="byteTempletData">报表模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTemplet(string szTempletID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("TempletAccess.GetReportTemplet", new string[] { "szTempletID" }, new object[] { szTempletID }, "模板ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("TempletAccess.GetReportTemplet", new string[] { "szTempletID" }, new object[] { szTempletID }, "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            // if (base.StorageMode == StorageMode.DB)
            return this.GetReportTempletFromDB(szTempletID, ref byteTempletData);
            //  else
            //     return this.GetReportTempletFromFTP(szTempletID, ref byteTempletData);
        }

        /// <summary>
        /// 获取所有报表文件
        /// </summary>
        /// <param name="lstReportTypeData">报表模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTemplet(ref List<ReportTypeData> lstReportTypeData)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("TempletAccess.GetReportTemplet", "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            //if (base.StorageMode == StorageMode.DB)
            return this.GetReportTempletFromDB(ref lstReportTypeData);
            //else
            //    return this.GetReportTempletFromFTP(ref lstReportTypeData);
        }

        /// <summary>
        /// 从数据库获取所有报表文件
        /// </summary>
        /// <param name="lstReportTypeDatas">报表模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetReportTempletFromDB(ref List<ReportTypeData> lstReportTypeDatas)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}"
                , ServerData.ReportTypeTable.REPORT_TYPE_ID, ServerData.ReportTypeTable.REPORT_TYPE_NAME
                , ServerData.ReportTypeTable.APPLY_ENV, ServerData.ReportTypeTable.REPORT_TYPE_NO
                , ServerData.ReportTypeTable.IS_VALID, ServerData.ReportTypeTable.IS_FOLDER
                , ServerData.ReportTypeTable.PARENT_ID, ServerData.ReportTypeTable.MODIFY_TIME
                , ServerData.ReportTypeTable.REPORT_DATA);
            string szCondiction = string.Format("{0}='{1}'", ServerData.ReportTypeTable.IS_FOLDER, "0");
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField
                            , ServerData.DataTable.REPORT_TYPE, szCondiction);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstReportTypeDatas == null)
                    lstReportTypeDatas = new List<ReportTypeData>();
                byte[] ByteTempletData = null;
                do
                {
                    ReportTypeData ReportTypeData = new ReportTypeData();
                    ReportTypeData.ReportTypeID = dataReader.GetString(0);
                    ReportTypeData.ReportTypeName = dataReader.GetString(1);
                    ReportTypeData.ApplyEnv = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        ReportTypeData.ApplyEnv = dataReader.GetValue(3).ToString();
                    if (!dataReader.IsDBNull(4))
                        ReportTypeData.IsValid = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        ReportTypeData.IsFolder = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6))
                        ReportTypeData.ParentID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        ReportTypeData.ModifyTime = dataReader.GetDateTime(7);
                    if (!dataReader.IsDBNull(8))
                    {
                        ByteTempletData = null;
                        GlobalMethods.IO.GetBytes(dataReader, 8, ref ByteTempletData);
                        ReportTypeData.ByteTempletData = ByteTempletData;
                    }
                    lstReportTypeDatas.Add(ReportTypeData);
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
        /// 根据报表模板ID从DB中获取报表模板内容
        /// </summary>
        /// <param name="szTempletID">报表模板ID</param>
        /// <param name="byteTempletData">报表模板二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetReportTempletFromDB(string szTempletID, ref byte[] byteTempletData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.ReportTypeTable.REPORT_TYPE_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.ReportTypeTable.REPORT_DATA, ServerData.DataTable.REPORT_TYPE, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetReportTempletFromDB", new string[] { "szSQL" }, new object[] { szSQL }, "没有查询到记录!");
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
        /// 从FTP服务器中获取所有报表文件
        /// </summary>
        /// <param name="lstReportTypeDatas">报表模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetReportTempletFromFTP(ref List<ReportTypeData> lstReportTypeDatas)
        {
            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }

            short shRet = this.GetReportTempletFromDB(ref lstReportTypeDatas);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }

            foreach (ReportTypeData ReportTypeData in lstReportTypeDatas)
            {
                string szRemoteFile = null;
                shRet = this.MakeReportTempletPath(ReportTypeData.ReportTypeID, ref szRemoteFile);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetReportTempletFromFTP", new string[] { "ReportTypeData.ReportTypeID" }
                    , new object[] { ReportTypeData.ReportTypeID }, "MakeReportTempletPath执行失败!");
                    continue;
                }

                string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                    , ReportTypeData.ReportTypeID, ServerData.FileExt.REPORT_TEMPLET);
                if (!base.FtpAccess.Download(szRemoteFile, szCacheFile))
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetReportTempletFromFTP", new string[] { "szRemoteFile" }
                    , new object[] { szRemoteFile }, "FtpAccess.Download执行失败!");
                    continue;
                }

                byte[] byteTempletData = null;
                if (!GlobalMethods.IO.GetFileBytes(szCacheFile, ref byteTempletData))
                {
                    GlobalMethods.IO.DeleteFile(szCacheFile);
                    LogManager.Instance.WriteLog("TempletAccess.GetReportTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytes执行失败!");
                    continue;
                }
                ReportTypeData.ByteTempletData = byteTempletData;
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
            base.FtpAccess.CloseConnection();
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据报表模板ID从FTP中获取报表模板内容
        /// </summary>
        /// <param name="szTempletID">报表模板ID</param>
        /// <param name="byteTempletData">报表模板二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetReportTempletFromFTP(string szTempletID, ref byte[] byteTempletData)
        {
            ReportTypeInfo reportTemplet = null;
            short shRet = this.GetReportTypeInfo(szTempletID, ref reportTemplet);
            if (shRet != ServerData.ExecuteResult.OK)
                return shRet;

            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }

            string szRemoteFile = null;
            shRet = this.MakeReportTempletPath(szTempletID, ref szRemoteFile);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.FtpAccess.CloseConnection();
                return shRet;
            }

            string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                , szTempletID, ServerData.FileExt.REPORT_TEMPLET);
            if (!base.FtpAccess.Download(szRemoteFile, szCacheFile))
            {
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (!GlobalMethods.IO.GetFileBytes(szCacheFile, ref byteTempletData))
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
                LogManager.Instance.WriteLog("TempletAccess.GetReportTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytes执行失败!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            base.FtpAccess.CloseConnection();
            GlobalMethods.IO.DeleteFile(szCacheFile);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存系统缺省模板到服务器
        /// </summary>
        /// <param name="szDocTypeID">报表类型ID</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveReportTemplet(string szDocTypeID, byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveReportTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "文档类型ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveReportTemplet"
                    , new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "配置字典表中文档存储模式配置不正确!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            //if (base.StorageMode == StorageMode.DB)
            return this.SaveReportTempletToDB(szDocTypeID, byteTempletData);
            //else
            //    return this.SaveReportTempletToFTP(szDocTypeID, byteTempletData);
        }

        /// <summary>
        /// 保存一条新的报表类型配置信息
        /// </summary>
        /// <param name="reportTypeInfo">报表类型</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveReportTypeInfo(ReportTypeInfo reportTypeInfo)
        {
            if (reportTypeInfo == null || GlobalMethods.Misc.IsEmptyString(reportTypeInfo.ReportTypeID))
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveReportTypeInfo", new string[] { "reportTypeInfo" }
                    , new object[] { reportTypeInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.ReportTypeTable.REPORT_TYPE_ID, ServerData.ReportTypeTable.REPORT_TYPE_NAME
                , ServerData.ReportTypeTable.APPLY_ENV, ServerData.ReportTypeTable.REPORT_TYPE_NO
                , ServerData.ReportTypeTable.IS_VALID, ServerData.ReportTypeTable.IS_FOLDER
                , ServerData.ReportTypeTable.PARENT_ID, ServerData.ReportTypeTable.MODIFY_TIME);
            string szValue = string.Format("'{0}','{1}','{2}',{3},{4},{5},'{6}',{7}"
                , reportTypeInfo.ReportTypeID, reportTypeInfo.ReportTypeName
                , reportTypeInfo.ApplyEnv, reportTypeInfo.ReportTypeNo
                , reportTypeInfo.IsValid ? "1" : "0", reportTypeInfo.IsFolder ? "1" : "0"
                , reportTypeInfo.ParentID, base.DataAccess.GetSystemTimeSql());
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.REPORT_TYPE, szField, szValue);
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
                LogManager.Instance.WriteLog("TempletAccess.SaveReportTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存报表模板到FTP服务器
        /// </summary>
        /// <param name="szDocTypeID">报表模板信息</param>
        /// <param name="byteTempletData">报表模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveReportTempletToFTP(string szDocTypeID, byte[] byteTempletData)
        {
            if (string.IsNullOrEmpty(szDocTypeID))
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveReportTempletToFTP", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
            {
                base.FtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }

            string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                , szDocTypeID, ServerData.FileExt.REPORT_TEMPLET);
            if (!GlobalMethods.IO.WriteFileBytes(szCacheFile, byteTempletData))
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveReportTempletToFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "WriteFileBytes执行失败!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            try
            {
                string szRemoteFile = null;
                short shRet = this.MakeReportTempletPath(szDocTypeID, ref szRemoteFile);
                if (shRet != ServerData.ExecuteResult.OK)
                    return shRet;

                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!base.FtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;

                if (!base.FtpAccess.Upload(szCacheFile, szRemoteFile))
                    return ServerData.ExecuteResult.EXCEPTION;

                shRet = this.SaveReportTempletToDB(szDocTypeID, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    base.FtpAccess.DeleteFile(szRemoteFile);
                    return shRet;
                }
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szDocTypeID, "SQL语句执行失败!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
        }

        /// <summary>
        /// 保存系统缺省模板到数据库服务器
        /// </summary>
        /// <param name="szDocTypeID">报表类型ID</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveReportTempletToDB(string szDocTypeID, byte[] byteTempletData)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}={1}", ServerData.ReportTypeTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}={2}", szField, ServerData.ReportTypeTable.REPORT_DATA, base.DataAccess.GetSqlParamName("REPORT_DATA"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("REPORT_DATA", byteTempletData);
            }

            string szCondition = string.Format("{0}='{1}'", ServerData.ReportTypeTable.REPORT_TYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.REPORT_TYPE, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.SaveSystemReportToDB", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据指定报表类型代码更新文档类型信息
        /// </summary>
        /// <param name="szDocTypeID">报表类型ID号</param>
        /// <param name="reportTypeInfo">报表类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyReportTypeInfo(string szDocTypeID, ReportTypeInfo reportTypeInfo)
        {
            if (reportTypeInfo == null || GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("TempleteAccess.ModifyReportTypeInfo", new string[] { "reportTypeInfo" }
                    , new object[] { reportTypeInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocTypeID = szDocTypeID.Trim();
            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}={7},{8}={9},{10}={11},{12}='{13}',{14}={15}"
                , ServerData.ReportTypeTable.REPORT_TYPE_ID, reportTypeInfo.ReportTypeID
                , ServerData.ReportTypeTable.REPORT_TYPE_NAME, reportTypeInfo.ReportTypeName
                , ServerData.ReportTypeTable.APPLY_ENV, reportTypeInfo.ApplyEnv
                , ServerData.ReportTypeTable.REPORT_TYPE_NO, reportTypeInfo.ReportTypeNo.ToString()
                , ServerData.ReportTypeTable.IS_VALID, reportTypeInfo.IsValid ? "1" : "0"
                , ServerData.ReportTypeTable.IS_FOLDER, reportTypeInfo.IsFolder ? "1" : "0"
                , ServerData.ReportTypeTable.PARENT_ID, reportTypeInfo.ParentID
                , ServerData.ReportTypeTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondiction = string.Format("{0}='{1}'", ServerData.ReportTypeTable.REPORT_TYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.REPORT_TYPE, szField, szCondiction);

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
                LogManager.Instance.WriteLog("TempletAccess.ModifyReportTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的一系列的报表类型
        /// </summary>
        /// <param name="lstDocTypeID">报表类型ID列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteReportTypeInfos(List<string> lstDocTypeID)
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
                shRet = this.DeleteReportTypeInfo(szDocTypeID);
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
        /// 根据报表类型ID,删除一条病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">报表类型ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteReportTypeInfo(string szDocTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("TempletAccess.DeleteReportTypeInfo", new string[] { "szDocTypeID" },
                    new object[] { szDocTypeID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.ReportTypeTable.REPORT_TYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.REPORT_TYPE, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            this.DeleteWardReportTypes(szDocTypeID);        //删除病区病历类型

            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }
        #endregion

        #region "病区报告类型读写接口"
        /// <summary>
        /// 获取所有文档类型代码应用到的病区列表
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="lstWardReportTypes">病区病历类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTypeDeptList(string szDocTypeID, ref List<WardReportType> lstWardReportTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardReportTypeTable.REPORT_TYPE_ID, ServerData.WardReportTypeTable.REPORT_TYPE_NAME
                , ServerData.WardReportTypeTable.WARD_CODE, ServerData.WardReportTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardReportTypeTable.REPORT_TYPE_ID, szDocTypeID);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_REPORT_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_REPORT_TYPE);
            }

            if (lstWardReportTypes == null)
                lstWardReportTypes = new List<WardReportType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardReportType wardReportType = new WardReportType();
                    wardReportType.ReportTypeID = dataReader.GetString(0);
                    wardReportType.ReportTypeName = dataReader.GetString(1);
                    wardReportType.WardCode = dataReader.GetString(2);
                    wardReportType.WardName = dataReader.GetString(3);
                    lstWardReportTypes.Add(wardReportType);
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
        /// 保存指定的一系列病区报表类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">报表类型ID</param>
        /// <param name="wardReportTypes">病区报表类型配置信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardReportTypes(string szDocTypeID, List<WardReportType> wardReportTypes)
        {
            if (wardReportTypes == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = this.DeleteWardReportTypes(szDocTypeID);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            foreach (WardReportType wardReportType in wardReportTypes)
            {
                if (wardReportType == null)
                    continue;
                shRet = this.SaveWardReportType(wardReportType);
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
        /// <param name="wardReportType">病区病历类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardReportType(WardReportType wardReportType)
        {
            if (wardReportType == null || GlobalMethods.Misc.IsEmptyString(wardReportType.ReportTypeID))
            {
                LogManager.Instance.WriteLog("TempleteAccess.SaveWardReportType", new string[] { "wardReportType" }
                    , new object[] { wardReportType }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardReportTypeTable.REPORT_TYPE_ID, ServerData.WardReportTypeTable.REPORT_TYPE_NAME
                , ServerData.WardReportTypeTable.WARD_CODE, ServerData.WardReportTypeTable.WARD_NAME);
            string szValue = string.Format("'{0}','{1}','{2}','{3}'"
                , wardReportType.ReportTypeID, wardReportType.ReportTypeName, wardReportType.WardCode, wardReportType.WardName);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.WARD_REPORT_TYPE, szField, szValue);
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("TempleteAccess.SaveWardReportType", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行失败!", ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("TempleteAccess.SaveWardReportType", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据病历类型代码删除当前病历类型对应的病区病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteWardReportTypes(string szDocTypeID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'"
                , ServerData.WardReportTypeTable.REPORT_TYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.WARD_REPORT_TYPE, szCondition);

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

        #region"护理文本模板读写接口"
        /// <summary>
        /// 获取指定的护理文本模板内容
        /// </summary>
        /// <param name="szTempletID">文本模板ID</param>
        /// <param name="byteTempletData">模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetTextTemplet(string szTempletID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("TempletAccess.GetTextTemplet", new string[] { "szTempletID" }, new object[] { szTempletID }, "模板ID参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.TEMPLET_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.TextTempletTable.TEMPLET_DATA, ServerData.DataTable.TEXT_TEMPLET, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetTextTemplet", new string[] { "szSQL" }, new object[] { szSQL }, "没有查询到记录!");
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
        /// 保存护理文本模板到服务器
        /// </summary>
        /// <param name="textTempletInfo">文本模板信息</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveTextTemplet(TextTempletInfo textTempletInfo, byte[] byteTempletData)
        {
            if (textTempletInfo == null)
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveTextTemplet", new string[] { "textTempletInfo" }, new object[] { textTempletInfo }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
                , ServerData.TextTempletTable.TEMPLET_ID, ServerData.TextTempletTable.TEMPLET_NAME
                , ServerData.TextTempletTable.CREATOR_ID, ServerData.TextTempletTable.CREATOR_NAME
                , ServerData.TextTempletTable.CREATE_TIME, ServerData.TextTempletTable.MODIFY_TIME
                , ServerData.TextTempletTable.SHARE_LEVEL, ServerData.TextTempletTable.WARD_CODE
                , ServerData.TextTempletTable.WARD_NAME, ServerData.TextTempletTable.PARENT_ID
                , ServerData.TextTempletTable.IS_FOLDER);

            string szValue = string.Format("'{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}',{10}"
                , textTempletInfo.TempletID, textTempletInfo.TempletName, textTempletInfo.CreatorID, textTempletInfo.CreatorName
                , base.DataAccess.GetSqlTimeFormat(textTempletInfo.CreateTime), base.DataAccess.GetSystemTimeSql()
                , textTempletInfo.ShareLevel, textTempletInfo.WardCode, textTempletInfo.WardName, textTempletInfo.ParentID
                , textTempletInfo.IsFolder ? 1 : 0);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}", szField, ServerData.TextTempletTable.TEMPLET_DATA);
                szValue = string.Format("{0},{1}", szValue, base.DataAccess.GetSqlParamName("TempletData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("TempletData", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.TEXT_TEMPLET, szField, szValue);

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
                LogManager.Instance.WriteLog("TempletAccess.SaveTextTemplet", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新护理文本模板内容到服务器
        /// </summary>
        /// <param name="textTempletInfo">结构化模板信息</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateTextTemplet(TextTempletInfo textTempletInfo, byte[] byteTempletData)
        {
            if (textTempletInfo == null)
            {
                LogManager.Instance.WriteLog("TempletAccess.UpdateTextTemplet", new string[] { "textTempletInfo" }, new object[] { textTempletInfo }
                    , "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}={1}", ServerData.TextTempletTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.TEMPLET_ID, textTempletInfo.TempletID);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}={2}", szField
                    , ServerData.TextTempletTable.TEMPLET_DATA, base.DataAccess.GetSqlParamName("TempletData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("TempletData", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.TEXT_TEMPLET, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.UpdateTextTemplet", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改指定的护理文本模板共享等级
        /// </summary>
        /// <param name="szTempletID">护理文本模板ID</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyTextTempletShareLevel(string szTempletID, string szShareLevel)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID) || GlobalMethods.Misc.IsEmptyString(szShareLevel))
            {
                LogManager.Instance.WriteLog("TempletAccess.ModifyTextTempletShareLevel", new string[] { "szTempletID", "szShareLevel" }
                    , new object[] { szTempletID, szShareLevel }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.TextTempletTable.SHARE_LEVEL, szShareLevel
                , ServerData.TextTempletTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.TEMPLET_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.TEXT_TEMPLET, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.ModifyTextTempletShareLevel", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改指定的护理文本模板共享等级（批量）
        /// </summary>
        /// <param name="lstTempletID">护理文本模板ID列表</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyTextTempletShareLevel(List<string> lstTempletID, string szShareLevel)
        {
            if (lstTempletID == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = ServerData.ExecuteResult.OK;
            for (int index = 0; index < lstTempletID.Count; index++)
            {
                string szTempletID = lstTempletID[index];
                if (GlobalMethods.Misc.IsEmptyString(szTempletID))
                    continue;
                shRet = this.ModifyTextTempletShareLevel(szTempletID, szShareLevel);
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
        /// <param name="szTempletID">护理文本模板ID</param>
        /// <param name="szParentID">模板新的父目录ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyTextTempletParentID(string szTempletID, string szParentID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("TempletAccess.ModifyTextTempletParentID", new string[] { "szTempletID" }, new object[] { szTempletID }
                    , "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.TextTempletTable.PARENT_ID, szParentID
                , ServerData.TextTempletTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.TEMPLET_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.TEXT_TEMPLET, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.ModifyTextTempletParentID", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 修改指定的护理文本模板名称
        /// </summary>
        /// <param name="szTempletID">护理文本模板ID</param>
        /// <param name="szTempletName">模板新的名称</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyTextTempletName(string szTempletID, string szTempletName)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID) || GlobalMethods.Misc.IsEmptyString(szTempletName))
            {
                LogManager.Instance.WriteLog("TempletAccess.ModifyTextTempletName", new string[] { "szTempletID", "szTempletName" }
                    , new object[] { szTempletID, szTempletName }, "参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.TextTempletTable.TEMPLET_NAME, szTempletName
                , ServerData.TextTempletTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.TEMPLET_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.TEXT_TEMPLET, szField, szCondition);

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
        /// 删除指定的护理文本模板
        /// </summary>
        /// <param name="szTempletID">护理文本模板ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteTextTemplet(string szTempletID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.TEMPLET_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.TEXT_TEMPLET, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.DeleteTextTemplet", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的一系列护理文本模板
        /// </summary>
        /// <param name="lstTempletID">要删除的文本模板ID列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteTextTemplet(List<string> lstTempletID)
        {
            if (lstTempletID == null || lstTempletID.Count <= 0)
                return ServerData.ExecuteResult.OK;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = ServerData.ExecuteResult.OK;
            for (int index = 0; index < lstTempletID.Count; index++)
            {
                shRet = this.DeleteTextTemplet(lstTempletID[index]);
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
        /// 根据指定的查询条件获取对应的护理文本模板列表
        /// </summary>
        /// <param name="szCondition">查询条件</param>
        /// <param name="szOrderSQL">排序子SQL</param>
        /// <param name="bContainsData">是否包含数据</param>
        /// <param name="lstTempletInfos">文档模板信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetTextTempletInfosInternal(string szCondition, string szOrderSQL, bool bContainsData
            , ref List<TextTempletInfo> lstTempletInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
                , ServerData.TextTempletTable.TEMPLET_ID, ServerData.TextTempletTable.TEMPLET_NAME
                , ServerData.TextTempletTable.CREATOR_ID, ServerData.TextTempletTable.CREATOR_NAME
                , ServerData.TextTempletTable.CREATE_TIME, ServerData.TextTempletTable.MODIFY_TIME
                , ServerData.TextTempletTable.SHARE_LEVEL, ServerData.TextTempletTable.WARD_CODE
                , ServerData.TextTempletTable.WARD_NAME, ServerData.TextTempletTable.PARENT_ID
                , ServerData.TextTempletTable.IS_FOLDER);

            if (bContainsData)
            {
                szField = string.Format("{0},{1}", szField, ServerData.TextTempletTable.TEMPLET_DATA);
            }

            string szSQL = string.Empty;
            if (GlobalMethods.Misc.IsEmptyString(szCondition))
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.TEXT_TEMPLET);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.TEXT_TEMPLET, szCondition);

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
                if (lstTempletInfos == null)
                    lstTempletInfos = new List<TextTempletInfo>();
                do
                {
                    TextTempletInfo textTempletInfo = new TextTempletInfo();
                    textTempletInfo.TempletID = dataReader.GetString(0);
                    textTempletInfo.TempletName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        textTempletInfo.CreatorID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        textTempletInfo.CreatorName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4))
                        textTempletInfo.CreateTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5))
                        textTempletInfo.ModifyTime = dataReader.GetDateTime(5);
                    textTempletInfo.ShareLevel = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        textTempletInfo.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8))
                        textTempletInfo.WardName = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9))
                        textTempletInfo.ParentID = dataReader.GetString(9);
                    textTempletInfo.IsFolder = dataReader.GetValue(10).ToString().Equals("1");

                    if (!textTempletInfo.IsFolder && bContainsData)
                    {
                        byte[] byteTempletData = null;
                        string szTempletData = string.Empty;
                        GlobalMethods.IO.GetBytes(dataReader, 11, ref byteTempletData);
                        GlobalMethods.Convert.BytesToString(byteTempletData, ref szTempletData);
                        textTempletInfo.TempletData = szTempletData;
                    }
                    lstTempletInfos.Add(textTempletInfo);
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
        /// 获取指定目录ID的文本模板列表
        /// </summary>
        /// <param name="szParentID">目录ID</param>
        /// <param name="lstTempletInfos">文本模板列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetChildTextTempletInfos(string szParentID, ref List<TextTempletInfo> lstTempletInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.PARENT_ID, szParentID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.TextTempletTable.TEMPLET_NAME);
            return this.GetTextTempletInfosInternal(szCondition, szOrderSQL, true, ref lstTempletInfos);
        }

        /// <summary>
        /// 获取指定用户ID的文本模板列表
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="lstTempletInfos">文本模板列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPersonalTextTempletInfos(string szUserID, ref List<TextTempletInfo> lstTempletInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.CREATOR_ID, szUserID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.TextTempletTable.PARENT_ID);
            return this.GetTextTempletInfosInternal(szCondition, szOrderSQL, false, ref lstTempletInfos);
        }

        /// <summary>
        /// 获取指定病区的文本模板列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="bOnlyDeptShare">是否仅返回标记为病区共享的文本模板</param>
        /// <param name="lstTempletInfos">文本模板列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDeptTextTempletInfos(string szWardCode, bool bOnlyDeptShare, ref List<TextTempletInfo> lstTempletInfos)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.TextTempletTable.WARD_CODE, szWardCode);
            if (bOnlyDeptShare)
            {
                //szCondition = string.Format("{0} AND ({1}='{2}' AND {3}=1)", szCondition
                //    , ServerData.TextTempletTable.SHARE_LEVEL, ServerData.ShareLevel.DEPART, ServerData.TextTempletTable.IS_FOLDER);
                szCondition = string.Format("{0} AND {1}='{2}' ", szCondition
                    , ServerData.TextTempletTable.SHARE_LEVEL, ServerData.ShareLevel.DEPART);
            }
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.TextTempletTable.CREATE_TIME);
            return this.GetTextTempletInfosInternal(szCondition, szOrderSQL, false, ref lstTempletInfos);
        }

        /// <summary>
        /// 获取全院文本模板列表
        /// </summary>
        /// <param name="lstTempletInfos">文本模板列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetHospitalTextTempletInfos(ref List<TextTempletInfo> lstTempletInfos)
        {
            //string szCondition = string.Format("{0}='{1}' AND {2}=1"
            //    , ServerData.TextTempletTable.SHARE_LEVEL, ServerData.ShareLevel.HOSPITAL
            //    , ServerData.TextTempletTable.IS_FOLDER);
            string szCondition = string.Format("{0}='{1}' "
                , ServerData.TextTempletTable.SHARE_LEVEL, ServerData.ShareLevel.HOSPITAL);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.TextTempletTable.CREATE_TIME);
            return this.GetTextTempletInfosInternal(szCondition, szOrderSQL, false, ref lstTempletInfos);
        }
        #endregion
    }
}
