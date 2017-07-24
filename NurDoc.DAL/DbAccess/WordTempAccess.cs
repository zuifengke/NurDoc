// ***********************************************************
// ���ݿ���ʲ�����ֲ���ģ���йص����ݵķ�����.
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
    public class WordTempAccess : DBAccessBase
    {
        #region"��ģ���д�ӿ�"
        /// <summary>
        /// ����ϵͳ�ĵ�ģ��·��
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="szTempletPath">�ĵ�ģ��·��</param>
        /// <returns>short</returns>
        private short MakeFormWordTempPath(string szDocTypeID, ref string szTempletPath)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.MakeFormWordTempPath", new string[] { "szDocTypeID" }
                        , new object[] { szDocTypeID }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            szTempletPath = string.Format("/TEMPLET/SYSTEM/{0}.{1}", szDocTypeID.Trim(), ServerData.FileExt.NUR_TEMPLET);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ��ȡϵͳ�Դ���ָ���ĵ����͵��ĵ�ģ������
        /// </summary>
        /// <param name="szDocTypeID">�ĵ����ʹ���</param>
        /// <param name="byteTempletData">ģ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetFormWordTemp(string szDocTypeID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.GetFormWordTemp", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "�ĵ�����ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("WordTempAccess.GetFormWordTemp", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.GetFormTempletFromDB(szDocTypeID, ref byteTempletData);
            else
                return this.GetFormTempletFromFTP(szDocTypeID, ref byteTempletData);
        }

        /// <summary>
        /// ��ȡ����ģ��
        /// </summary>
        /// <param name="lstDocTypeDatas">ģ����Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetFormWordTemp(ref List<DocTypeData> lstDocTypeDatas)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("WordTempAccess.GetFormTemplet", "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (base.StorageMode == StorageMode.DB)
                return this.GetFormTempletFromDB(ref lstDocTypeDatas);
            else
                return this.GetFormTempletFromFTP(ref lstDocTypeDatas);
        }

        /// <summary>
        /// ��ȡ�����ݿ�����ģ���ļ�
        /// </summary>
        /// <param name="lstDocTypeDatas">ģ���б�</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// �����ĵ����ʹ����DB�л�ȡϵͳ�ĵ�ģ������
        /// </summary>
        /// <param name="szDocTypeID">�ĵ����ͱ��</param>
        /// <param name="byteTempletData">ϵͳ�ĵ�ģ�����������</param>
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
                    LogManager.Instance.WriteLog("WordTempAccess.GetFormTempletFromDB", new string[] { "szSQL" }, new object[] { szSQL }, "û�в�ѯ����¼!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                return GlobalMethods.IO.GetBytes(dataReader, 0, ref byteTempletData)
                    ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��FTP�������л�ȡ����ģ���ļ�
        /// </summary>
        /// <param name="lstDocTypeDatas">ģ���б�</param>
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
                shRet = this.MakeFormWordTempPath(lstDocTypeDatas[index].DocTypeID, ref szRemoteFile);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    LogManager.Instance.WriteLog("WordTempAccess.GetFormTempletFromFTP", new string[] { "DocTypeData.DocTypeID" }
                    , new object[] { lstDocTypeDatas[index].DocTypeID }, "MakeFormWordTempPathִ��ʧ��!");
                    continue;
                }

                string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                    , lstDocTypeDatas[index].DocTypeID, ServerData.FileExt.NUR_TEMPLET);
                if (!base.FtpAccess.Download(szRemoteFile, szCacheFile))
                {
                    LogManager.Instance.WriteLog("WordTempAccess.GetFormTempletFromFTP", new string[] { "szRemoteFile" }
                    , new object[] { szRemoteFile }, "FtpAccess.Downloadִ��ʧ��!");
                    continue;
                }

                byte[] byteTempletData = null;
                if (!GlobalMethods.IO.GetFileBytes(szCacheFile, ref byteTempletData))
                {
                    GlobalMethods.IO.DeleteFile(szCacheFile);
                    LogManager.Instance.WriteLog("WordTempAccess.GetFormTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytesִ��ʧ��!");
                    return ServerData.ExecuteResult.EXCEPTION;
                }
                lstDocTypeDatas[index].ByteTempletData = byteTempletData;
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
            base.FtpAccess.CloseConnection();
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �����ĵ����ʹ����FTP�л�ȡϵͳ�ĵ�ģ������
        /// </summary>
        /// <param name="szDocTypeID">�ĵ����ͱ��</param>
        /// <param name="byteTempletData">ϵͳ�ĵ�ģ�����������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetFormTempletFromFTP(string szDocTypeID, ref byte[] byteTempletData)
        {
            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.FtpAccess.OpenConnection())
                return ServerData.ExecuteResult.EXCEPTION;

            string szRemoteFile = null;
            short shRet = this.MakeFormWordTempPath(szDocTypeID, ref szRemoteFile);
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
                LogManager.Instance.WriteLog("WordTempAccess.GetFormTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytesִ��ʧ��!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            base.FtpAccess.CloseConnection();
            GlobalMethods.IO.DeleteFile(szCacheFile);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ����ϵͳȱʡģ�嵽������
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveFormTemplet(string szDocTypeID, byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveFormTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "�ĵ�����ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveFormTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.SaveFormTempletToDB(szDocTypeID, byteTempletData);
            else
                return this.SaveFormTempletToFTP(szDocTypeID, byteTempletData);
        }

        /// <summary>
        /// ����ϵͳȱʡģ�嵽���ݿ������
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveFormTempletToDB", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ����ϵͳȱʡģ�嵽FTP������
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
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
                LogManager.Instance.WriteLog("WordTempAccess.SaveFormTempletToFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "WriteFileBytesִ��ʧ��!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            try
            {
                string szRemoteFile = null;
                short shRet = this.MakeFormWordTempPath(szDocTypeID, ref szRemoteFile);
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szDocTypeID, "SQL���ִ��ʧ��!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
        }
        #endregion

        #region"����ģ���д�ӿ�"
        /// <summary>
        /// �����û��ĵ�ģ��·��
        /// </summary>
        /// <param name="szTempletID">ģ��ID</param>
        /// <param name="szTempletPath">�û��ĵ�ģ��FTP·��</param>
        /// <returns>short</returns>
        private short MakeReportTempletPath(string szTempletID, ref string szTempletPath)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.MakeReportTempletPath", new string[] { "szTempletID" }
                        , new object[] { szTempletID }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            szTempletPath = string.Format("/REPORT/SYSTEM/{0}.{1}", szTempletID.Trim(), ServerData.FileExt.REPORT_TEMPLET);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ͨ��ID��ȡ����������Ϣ
        /// </summary>
        /// <param name="szReportTypeID">�ĵ�����ID</param>
        /// <param name="reportTypeWord">����������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTypeInfo(string szReportTypeID, ref ReportTypeInfo reportTypeWord)
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

                if (reportTypeWord == null)
                    reportTypeWord = new ReportTypeInfo();

                reportTypeWord.ReportTypeID = dataReader.GetString(0);
                reportTypeWord.ReportTypeName = dataReader.GetString(1);
                reportTypeWord.ApplyEnv = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    reportTypeWord.ReportTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                if (!dataReader.IsDBNull(4))
                    reportTypeWord.IsValid = dataReader.GetValue(4).ToString().Equals("1");
                if (!dataReader.IsDBNull(5))
                    reportTypeWord.IsFolder = dataReader.GetValue(5).ToString().Equals("1");
                if (!dataReader.IsDBNull(6))
                    reportTypeWord.ParentID = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    reportTypeWord.ModifyTime = dataReader.GetDateTime(7);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡ���б���������Ϣ�б�
        /// </summary>
        /// <param name="szApplyEnv">����Ӧ�û���</param>
        /// <param name="lstReportTypeWords">����������Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTypeWords(string szApplyEnv, ref List<ReportTypeInfo> lstReportTypeWords)
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
                    , ServerData.DataTable.REPORT_TYPE, szCondition, ServerData.ReportTypeTable.REPORT_TYPE_NO);
            }

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstReportTypeWords == null)
                    lstReportTypeWords = new List<ReportTypeInfo>();
                do
                {
                    ReportTypeInfo reportTypeWord = new ReportTypeInfo();
                    reportTypeWord.ReportTypeID = dataReader.GetString(0);
                    reportTypeWord.ReportTypeName = dataReader.GetString(1);
                    reportTypeWord.ApplyEnv = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        reportTypeWord.ReportTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4))
                        reportTypeWord.IsValid = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        reportTypeWord.IsFolder = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6))
                        reportTypeWord.ParentID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        reportTypeWord.ModifyTime = dataReader.GetDateTime(7);
                    lstReportTypeWords.Add(reportTypeWord);
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
        /// ��ȡ�����ĵ����ʹ���Ӧ�õ��Ĳ����б�
        /// </summary>
        /// <param name="szWardCode">��������</param>
        /// <param name="lstWardReportTypes">�������������б�</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡ�����ĵ�ģ������
        /// </summary>
        /// <param name="szTempletID">����ģ��ID</param>
        /// <param name="byteTempletData">����ģ������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTemplet(string szTempletID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.GetReportTemplet", new string[] { "szTempletID" }, new object[] { szTempletID }, "ģ��ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("WordTempAccess.GetReportTemplet", new string[] { "szTempletID" }, new object[] { szTempletID }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.GetReportTempletFromDB(szTempletID, ref byteTempletData);
            else
                return this.GetReportTempletFromFTP(szTempletID, ref byteTempletData);
        }

        /// <summary>
        /// ��ȡ���б����ļ�
        /// </summary>
        /// <param name="lstReportTypeData">����ģ������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTemplet(ref List<ReportTypeData> lstReportTypeData)
        {
            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("WordTempAccess.GetReportTemplet", "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.GetReportTempletFromDB(ref lstReportTypeData);
            else
                return this.GetReportTempletFromFTP(ref lstReportTypeData);
        }

        /// <summary>
        /// �����ݿ��ȡ���б����ļ�
        /// </summary>
        /// <param name="lstReportTypeDatas">����ģ������</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ���ݱ���ģ��ID��DB�л�ȡ����ģ������
        /// </summary>
        /// <param name="szTempletID">����ģ��ID</param>
        /// <param name="byteTempletData">����ģ�����������</param>
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
                    LogManager.Instance.WriteLog("WordTempAccess.GetReportTempletFromDB", new string[] { "szSQL" }, new object[] { szSQL }, "û�в�ѯ����¼!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                return GlobalMethods.IO.GetBytes(dataReader, 0, ref byteTempletData)
                    ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��FTP�������л�ȡ���б����ļ�
        /// </summary>
        /// <param name="lstReportTypeDatas">����ģ������</param>
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
                    LogManager.Instance.WriteLog("WordTempAccess.GetReportTempletFromFTP", new string[] { "ReportTypeData.ReportTypeID" }
                    , new object[] { ReportTypeData.ReportTypeID }, "MakeReportTempletPathִ��ʧ��!");
                    continue;
                }

                string szCacheFile = string.Format("{0}\\Cache\\DAL\\{1}.{2}", ServerParam.Instance.WorkPath
                    , ReportTypeData.ReportTypeID, ServerData.FileExt.REPORT_TEMPLET);
                if (!base.FtpAccess.Download(szRemoteFile, szCacheFile))
                {
                    LogManager.Instance.WriteLog("WordTempAccess.GetReportTempletFromFTP", new string[] { "szRemoteFile" }
                    , new object[] { szRemoteFile }, "FtpAccess.Downloadִ��ʧ��!");
                    continue;
                }

                byte[] byteTempletData = null;
                if (!GlobalMethods.IO.GetFileBytes(szCacheFile, ref byteTempletData))
                {
                    GlobalMethods.IO.DeleteFile(szCacheFile);
                    LogManager.Instance.WriteLog("WordTempAccess.GetReportTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytesִ��ʧ��!");
                    continue;
                }
                ReportTypeData.ByteTempletData = byteTempletData;
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
            base.FtpAccess.CloseConnection();
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���ݱ���ģ��ID��FTP�л�ȡ����ģ������
        /// </summary>
        /// <param name="szTempletID">����ģ��ID</param>
        /// <param name="byteTempletData">����ģ�����������</param>
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
                LogManager.Instance.WriteLog("WordTempAccess.GetReportTempletFromFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "GetFileBytesִ��ʧ��!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            base.FtpAccess.CloseConnection();
            GlobalMethods.IO.DeleteFile(szCacheFile);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ����ϵͳȱʡģ�嵽������
        /// </summary>
        /// <param name="szDocTypeID">��������ID</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveReportTemplet(string szDocTypeID, byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveReportTemplet", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "�ĵ�����ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.StorageMode == StorageMode.Unknown)
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveReportTemplet"
                    , new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "�����ֵ�����ĵ��洢ģʽ���ò���ȷ!");
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (base.StorageMode == StorageMode.DB)
                return this.SaveReportTempletToDB(szDocTypeID, byteTempletData);
            else
                return this.SaveReportTempletToFTP(szDocTypeID, byteTempletData);
        }

        /// <summary>
        /// ����һ���µı�������������Ϣ
        /// </summary>
        /// <param name="reportTypeWord">��������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveReportTypeWord(ReportTypeInfo reportTypeWord)
        {
            if (reportTypeWord == null || GlobalMethods.Misc.IsEmptyString(reportTypeWord.ReportTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveReportTypeWord", new string[] { "reportTypeWord" }
                    , new object[] { reportTypeWord }, "��������Ϊ��");
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
                , reportTypeWord.ReportTypeID, reportTypeWord.ReportTypeName
                , reportTypeWord.ApplyEnv, reportTypeWord.ReportTypeNo
                , reportTypeWord.IsValid ? "1" : "0", reportTypeWord.IsFolder ? "1" : "0"
                , reportTypeWord.ParentID, base.DataAccess.GetSystemTimeSql());
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.REPORT_TYPE, szField, szValue);
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
                LogManager.Instance.WriteLog("WordTempAccess.SaveReportTypeWord", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���汨��ģ�嵽FTP������
        /// </summary>
        /// <param name="szDocTypeID">����ģ����Ϣ</param>
        /// <param name="byteTempletData">����ģ������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short SaveReportTempletToFTP(string szDocTypeID, byte[] byteTempletData)
        {
            if (string.IsNullOrEmpty(szDocTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveReportTempletToFTP", new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "��������Ϊ��!");
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
                LogManager.Instance.WriteLog("WordTempAccess.SaveReportTempletToFTP", new string[] { "szCacheFile" }
                    , new object[] { szCacheFile }, "WriteFileBytesִ��ʧ��!");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szDocTypeID, "SQL���ִ��ʧ��!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
                GlobalMethods.IO.DeleteFile(szCacheFile);
            }
        }

        /// <summary>
        /// ����ϵͳȱʡģ�嵽���ݿ������
        /// </summary>
        /// <param name="szDocTypeID">��������ID</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveSystemReportToDB", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ����ָ���������ʹ�������ĵ�������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">��������ID��</param>
        /// <param name="reportTypeWord">����������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyReportTypeWord(string szDocTypeID, ReportTypeInfo reportTypeWord)
        {
            if (reportTypeWord == null || GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.ModifyReportTypeWord", new string[] { "reportTypeWord" }
                    , new object[] { reportTypeWord }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocTypeID = szDocTypeID.Trim();
            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}={7},{8}={9},{10}={11},{12}='{13}',{14}={15}"
                , ServerData.ReportTypeTable.REPORT_TYPE_ID, reportTypeWord.ReportTypeID
                , ServerData.ReportTypeTable.REPORT_TYPE_NAME, reportTypeWord.ReportTypeName
                , ServerData.ReportTypeTable.APPLY_ENV, reportTypeWord.ApplyEnv
                , ServerData.ReportTypeTable.REPORT_TYPE_NO, reportTypeWord.ReportTypeNo.ToString()
                , ServerData.ReportTypeTable.IS_VALID, reportTypeWord.IsValid ? "1" : "0"
                , ServerData.ReportTypeTable.IS_FOLDER, reportTypeWord.IsFolder ? "1" : "0"
                , ServerData.ReportTypeTable.PARENT_ID, reportTypeWord.ParentID
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("WordTempAccess.ModifyReportTypeWord", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ɾ��ָ����һϵ�еı�������
        /// </summary>
        /// <param name="lstDocTypeID">��������ID�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteReportTypeWords(List<string> lstDocTypeID)
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
                shRet = this.DeleteReportTypeWord(szDocTypeID);
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
        /// ���ݱ�������ID,ɾ��һ����������������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">��������ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteReportTypeWord(string szDocTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.DeleteReportTypeWord", new string[] { "szDocTypeID" },
                    new object[] { szDocTypeID }, "��������Ϊ��");
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            this.DeleteWardReportTypes(szDocTypeID);        //ɾ��������������

            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }
        #endregion

        #region "�����������Ͷ�д�ӿ�"
        /// <summary>
        /// ��ȡ�����ĵ����ʹ���Ӧ�õ��Ĳ����б�
        /// </summary>
        /// <param name="szDocTypeID">��������ID��</param>
        /// <param name="lstWardReportTypes">�������������б�</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ����ָ����һϵ�в�����������������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">��������ID</param>
        /// <param name="wardReportTypes">������������������Ϣ�б�</param>
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
        /// ����һ���µĲ�����������������Ϣ
        /// </summary>
        /// <param name="wardReportType">��������������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardReportType(WardReportType wardReportType)
        {
            if (wardReportType == null || GlobalMethods.Misc.IsEmptyString(wardReportType.ReportTypeID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveWardReportType", new string[] { "wardReportType" }
                    , new object[] { wardReportType }, "��������Ϊ��");
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
                LogManager.Instance.WriteLog("WordTempAccess.SaveWardReportType", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ��ʧ��!", ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveWardReportType", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���ݲ������ʹ���ɾ����ǰ�������Ͷ�Ӧ�Ĳ�����������������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">��������ID</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.OTHER_ERROR;
        }
        #endregion

        #region"�����ı�ģ���д�ӿ�"
        /// <summary>
        /// ��ȡָ���Ļ���wordģ������
        /// </summary>
        /// <param name="szTempletID">wordģ��ID</param>
        /// <param name="byteTempletData">ģ������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWordTemp(string szTempletID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.GetWordTemp", new string[] { "szTempletID" }, new object[] { szTempletID }, "ģ��ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.WordTempTable.WORDTEMP_DATA, ServerData.DataTable.WORD_TEMP, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("WordTempAccess.GetWordTemp", new string[] { "szSQL" }, new object[] { szSQL }, "û�в�ѯ����¼!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                return GlobalMethods.IO.GetBytes(dataReader, 0, ref byteTempletData)
                    ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ���滤��wordģ�嵽������
        /// </summary>
        /// <param name="textTempletWord">wordģ����Ϣ</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWordTemp(WordTempInfo textTempletWord, byte[] byteTempletData)
        {
            if (textTempletWord == null)
            {
                LogManager.Instance.WriteLog("WordTempAccess.SaveWordTemp", new string[] { "textTempletWord" }, new object[] { textTempletWord }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
                , ServerData.WordTempTable.WORDTEMP_ID, ServerData.WordTempTable.WORDTEMP_NAME
                , ServerData.WordTempTable.CREATOR_ID, ServerData.WordTempTable.CREATOR_NAME
                , ServerData.WordTempTable.CREATE_TIME, ServerData.WordTempTable.MODIFY_TIME
                , ServerData.WordTempTable.SHARE_LEVEL, ServerData.WordTempTable.WARD_CODE
                , ServerData.WordTempTable.WARD_NAME, ServerData.WordTempTable.PARENT_ID
                , ServerData.WordTempTable.IS_FOLDER);

            string szValue = string.Format("'{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}',{10}"
                , textTempletWord.TempletID, textTempletWord.TempletName, textTempletWord.CreatorID, textTempletWord.CreatorName
                , base.DataAccess.GetSqlTimeFormat(textTempletWord.CreateTime), base.DataAccess.GetSystemTimeSql()
                , textTempletWord.ShareLevel, textTempletWord.WardCode, textTempletWord.WardName, textTempletWord.ParentID
                , textTempletWord.IsFolder ? 1 : 0);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}", szField, ServerData.WordTempTable.WORDTEMP_DATA);
                szValue = string.Format("{0},{1}", szValue, base.DataAccess.GetSqlParamName("TempletData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("TempletData", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.WORD_TEMP, szField, szValue);

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
                LogManager.Instance.WriteLog("WordTempAccess.SaveWordTemp", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���»���wordģ�����ݵ�������
        /// </summary>
        /// <param name="textTempletWord">�ṹ��ģ����Ϣ</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateWordTemp(WordTempInfo textTempletWord, byte[] byteTempletData)
        {
            if (textTempletWord == null)
            {
                LogManager.Instance.WriteLog("WordTempAccess.UpdateWordTemp", new string[] { "textTempletWord" }, new object[] { textTempletWord }
                    , "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}={1}", ServerData.WordTempTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, textTempletWord.TempletID);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}={2}", szField
                    , ServerData.WordTempTable.WORDTEMP_DATA, base.DataAccess.GetSqlParamName("WORDTEMP_DATA"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("WORDTEMP_DATA", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.WORD_TEMP, szField, szCondition);

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
                LogManager.Instance.WriteLog("WordTempAccess.UpdateTextTemplet", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸�ָ���Ļ���wordģ�干��ȼ�
        /// </summary>
        /// <param name="szTempletID">����wordģ��ID</param>
        /// <param name="szShareLevel">ģ���µĹ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyWordTempShareLevel(string szTempletID, string szShareLevel)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID) || GlobalMethods.Misc.IsEmptyString(szShareLevel))
            {
                LogManager.Instance.WriteLog("WordTempAccess.ModifyWordTempShareLevel", new string[] { "szTempletID", "szShareLevel" }
                    , new object[] { szTempletID, szShareLevel }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.WordTempTable.SHARE_LEVEL, szShareLevel
                , ServerData.WordTempTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.WORD_TEMP, szField, szCondition);

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
                LogManager.Instance.WriteLog("WordTempAccess.ModifyWordTempShareLevel", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸�ָ���Ļ���wordģ�干��ȼ���������
        /// </summary>
        /// <param name="lstTempletID">����wordģ��ID�б�</param>
        /// <param name="szShareLevel">ģ���µĹ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyWordTempShareLevel(List<string> lstTempletID, string szShareLevel)
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
                shRet = this.ModifyWordTempShareLevel(szTempletID, szShareLevel);
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
        /// �޸�ָ���Ļ���wordģ�常Ŀ¼
        /// </summary>
        /// <param name="szTempletID">����wordģ��ID</param>
        /// <param name="szParentID">ģ���µĸ�Ŀ¼ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyTextTempletParentID(string szTempletID, string szParentID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID))
            {
                LogManager.Instance.WriteLog("WordTempAccess.ModifyTextTempletParentID", new string[] { "szTempletID" }, new object[] { szTempletID }
                    , "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.WordTempTable.PARENT_ID, szParentID
                , ServerData.WordTempTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.WORD_TEMP, szField, szCondition);

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
                LogManager.Instance.WriteLog("WordTempAccess.ModifyTextTempletParentID", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// �޸�ָ���Ļ���wordģ������
        /// </summary>
        /// <param name="szTempletID">����wordģ��ID</param>
        /// <param name="szTempletName">ģ���µ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyWordTempName(string szTempletID, string szTempletName)
        {
            if (GlobalMethods.Misc.IsEmptyString(szTempletID) || GlobalMethods.Misc.IsEmptyString(szTempletName))
            {
                LogManager.Instance.WriteLog("WordTempAccess.ModifyWordTempName", new string[] { "szTempletID", "szTempletName" }
                    , new object[] { szTempletID, szTempletName }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3}", ServerData.WordTempTable.WORDTEMP_NAME, szTempletName
                , ServerData.WordTempTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.WORD_TEMP, szField, szCondition);

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
                LogManager.Instance.WriteLog("WordTempAccess.ModifyWordTempName", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ɾ��ָ���Ļ���wordģ��
        /// </summary>
        /// <param name="szTempletID">����wordģ��ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteWordTemp(string szTempletID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, szTempletID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.WORD_TEMP, szCondition);

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
                LogManager.Instance.WriteLog("WordTempAccess.DeleteWordTemp", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ɾ��ָ����һϵ�л���wordģ��
        /// </summary>
        /// <param name="lstTempletID">Ҫɾ����wordģ��ID�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteWordTemp(List<string> lstTempletID)
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
                shRet = this.DeleteWordTemp(lstTempletID[index]);
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
        /// ����ָ���Ĳ�ѯ������ȡ��Ӧ�Ļ���wordģ���б�
        /// </summary>
        /// <param name="szCondition">��ѯ����</param>
        /// <param name="szOrderSQL">������SQL</param>
        /// <param name="bContainsData">�Ƿ��������</param>
        /// <param name="lstTempletWords">�ĵ�ģ����Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetWordTempWordsInternal(string szCondition, string szOrderSQL, bool bContainsData
            , ref List<WordTempInfo> lstTempletWords)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
                , ServerData.WordTempTable.WORDTEMP_ID, ServerData.WordTempTable.WORDTEMP_NAME
                , ServerData.WordTempTable.CREATOR_ID, ServerData.WordTempTable.CREATOR_NAME
                , ServerData.WordTempTable.CREATE_TIME, ServerData.WordTempTable.MODIFY_TIME
                , ServerData.WordTempTable.SHARE_LEVEL, ServerData.WordTempTable.WARD_CODE
                , ServerData.WordTempTable.WARD_NAME, ServerData.WordTempTable.PARENT_ID
                , ServerData.WordTempTable.IS_FOLDER);

            if (bContainsData)
            {
                szField = string.Format("{0},{1}", szField, ServerData.WordTempTable.WORDTEMP_DATA);
            }

            string szSQL = string.Empty;
            if (GlobalMethods.Misc.IsEmptyString(szCondition))
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WORD_TEMP);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.WORD_TEMP, szCondition);

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
                if (lstTempletWords == null)
                    lstTempletWords = new List<WordTempInfo>();
                do
                {
                    WordTempInfo textTempletWord = new WordTempInfo();
                    textTempletWord.TempletID = dataReader.GetString(0);
                    textTempletWord.TempletName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        textTempletWord.CreatorID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        textTempletWord.CreatorName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4))
                        textTempletWord.CreateTime = dataReader.GetDateTime(4);
                    if (!dataReader.IsDBNull(5))
                        textTempletWord.ModifyTime = dataReader.GetDateTime(5);
                    textTempletWord.ShareLevel = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7))
                        textTempletWord.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8))
                        textTempletWord.WardName = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9))
                        textTempletWord.ParentID = dataReader.GetString(9);
                    textTempletWord.IsFolder = dataReader.GetValue(10).ToString().Equals("1");

                    if (!textTempletWord.IsFolder && bContainsData)
                    {
                        byte[] byteTempletData = null;
                        string szTempletData = string.Empty;
                        GlobalMethods.IO.GetBytes(dataReader, 11, ref byteTempletData);
                        GlobalMethods.Convert.BytesToString(byteTempletData, ref szTempletData);
                        textTempletWord.TempletData = szTempletData;
                    }
                    lstTempletWords.Add(textTempletWord);
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
        /// ��ȡָ��Ŀ¼ID��wordģ���б�
        /// </summary>
        /// <param name="szParentID">Ŀ¼ID</param>
        /// <param name="lstTempletWords">wordģ���б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetChildTextTempletWords(string szParentID, ref List<WordTempInfo> lstTempletWords)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.PARENT_ID, szParentID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.WordTempTable.WORDTEMP_NAME);
            return this.GetWordTempWordsInternal(szCondition, szOrderSQL, true, ref lstTempletWords);
        }

        /// <summary>
        /// ��ȡָ���û�ID��wordģ���б�
        /// </summary>
        /// <param name="szUserID">�û�ID</param>
        /// <param name="lstTempletWords">wordģ���б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPersonalWordTempInfos(string szUserID, ref List<WordTempInfo> lstTempletWords)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.CREATOR_ID, szUserID);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.WordTempTable.PARENT_ID);
            return this.GetWordTempWordsInternal(szCondition, szOrderSQL, false, ref lstTempletWords);
        }

        /// <summary>
        /// ��ȡָ��������wordģ���б�
        /// </summary>
        /// <param name="szWardCode">��������</param>
        /// <param name="bOnlyDeptShare">�Ƿ�����ر��Ϊ���������wordģ��</param>
        /// <param name="lstTempletWords">wordģ���б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDeptWordTempInfos(string szWardCode, bool bOnlyDeptShare, ref List<WordTempInfo> lstTempletWords)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WARD_CODE, szWardCode);
            if (bOnlyDeptShare)
            {
                szCondition = string.Format("{0} AND ({1}='{2}' OR {3}=1)", szCondition
                    , ServerData.WordTempTable.SHARE_LEVEL, ServerData.ShareLevel.DEPART, ServerData.WordTempTable.IS_FOLDER);
            }
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.WordTempTable.PARENT_ID);
            return this.GetWordTempWordsInternal(szCondition, szOrderSQL, false, ref lstTempletWords);
        }

        /// <summary>
        /// ��ȡȫԺwordģ���б�
        /// </summary>
        /// <param name="lstTempletWords">wordģ���б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetHospitalWordTempInfos(ref List<WordTempInfo> lstTempletWords)
        {
            string szCondition = string.Format("{0}='{1}' OR {2}=1"
                , ServerData.WordTempTable.SHARE_LEVEL, ServerData.ShareLevel.HOSPITAL
                , ServerData.WordTempTable.IS_FOLDER);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.WordTempTable.PARENT_ID);
            return this.GetWordTempWordsInternal(szCondition, szOrderSQL, false, ref lstTempletWords);
        }

        public short GetWordLib(string szWordID, ref byte[] byteTempletData)
        {
            if (GlobalMethods.Misc.IsEmptyString(szWordID))
            {
                LogManager.Instance.WriteLog("TempletAccess.GetTextTemplet", new string[] { "szTempletID" }, new object[] { szWordID }, "ģ��ID��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, szWordID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, ServerData.WordTempTable.WORDTEMP_DATA, ServerData.DataTable.INFO_LIB, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("TempletAccess.GetWordLib", new string[] { "szSQL" }, new object[] { szSQL }, "û�в�ѯ����¼!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                return GlobalMethods.IO.GetBytes(dataReader, 0, ref byteTempletData)
                    ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡָ��ID�ŵ��ĵ���Ϣ
        /// </summary>
        /// <param name="szWordID">�ĵ�ID</param>
        /// <param name="lstWordTempWords">������Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        //public short GetWordLibWords(string TempletID, ref List<WordTempInfo> lstWordLibWords)
        //{
        //    string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, TempletID);
        //    string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.WordTempTable.WORDTEMP_NAME);
        //    return this.GetWordLibWordsInternal(szCondition, szOrderSQL, true, ref lstWordLibWords);
        //}

        /// <summary>
        /// ���滤����Ϣ���ݵ�������
        /// </summary>
        /// <param name="WordTempWord">������Ϣ</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWordLib(WordTempInfo WordTempWord, byte[] byteTempletData)
        {
            if (WordTempWord == null)
            {
                LogManager.Instance.WriteLog("TempletAccess.SaveWordLib", new string[] { "textTempletWord" }, new object[] { WordTempWord }, "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                , ServerData.WordTempTable.WORDTEMP_ID, ServerData.WordTempTable.WORDTEMP_NAME
                , ServerData.WordTempTable.CREATOR_ID, ServerData.WordTempTable.CREATOR_NAME
                , ServerData.WordTempTable.CREATE_TIME, ServerData.WordTempTable.MODIFY_TIME
                , ServerData.WordTempTable.SHARE_LEVEL, ServerData.WordTempTable.WARD_CODE
                , ServerData.WordTempTable.WARD_NAME, ServerData.WordTempTable.PARENT_ID
                , ServerData.WordTempTable.IS_FOLDER);

            string szValue = string.Format("'{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}'"
                , WordTempWord.TempletID, WordTempWord.TempletName, WordTempWord.CreatorID, WordTempWord.CreatorName
                , base.DataAccess.GetSqlTimeFormat(WordTempWord.CreateTime), base.DataAccess.GetSystemTimeSql()
                , WordTempWord.ShareLevel, WordTempWord.WardCode, WordTempWord.WardName, WordTempWord.ParentID
                , WordTempWord.IsFolder ? 1 : 0);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}", szField, ServerData.WordTempTable.WORDTEMP_DATA);
                szValue = string.Format("{0},{1}", szValue, base.DataAccess.GetSqlParamName("WordData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("WordData", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.INFO_LIB, szField, szValue);

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
                LogManager.Instance.WriteLog("TempletAccess.SaveWordLib", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���»�����Ϣ���ݵ�������
        /// </summary>
        /// <param name="WordTempWord">������Ϣ����</param>
        /// <param name="byteTempletData">ϵͳȱʡģ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateWordLib(WordTempInfo WordTempWord, byte[] byteTempletData)
        {
            if (WordTempWord == null)
            {
                LogManager.Instance.WriteLog("TempletAccess.UpdateWordLib", new string[] { "WordTempWord" }, new object[] { WordTempWord }
                    , "��������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}={1} , {2} = '{3}'"
                , ServerData.WordTempTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql());
            string szCondition = string.Format("{0}='{1}'", ServerData.WordTempTable.WORDTEMP_ID, WordTempWord.TempletID);

            DbParameter[] pmi = null;
            if (byteTempletData != null)
            {
                szField = string.Format("{0},{1}={2}", szField
                    , ServerData.WordTempTable.WORDTEMP_DATA, base.DataAccess.GetSqlParamName("WordData"));

                pmi = new DbParameter[1];
                pmi[0] = new DbParameter("WordData", byteTempletData);
            }

            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.INFO_LIB, szField, szCondition);

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
                LogManager.Instance.WriteLog("TempletAccess.UpdateWordLib", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
    }
}
        #endregion