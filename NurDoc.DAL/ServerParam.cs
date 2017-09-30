// ***********************************************************
// ���ݿ���ʲ�ϵͳ���в���������,���ڵ��ò����޸����в���.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Text;
using System.Collections.Generic;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;
using Heren.Common.Libraries.Ftp;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.DAL
{
    public class ServerParam
    {
        private string m_szWorkPath = string.Empty;
        private string m_szIISAddress = string.Empty;
        private bool m_bAutoClearPool = true;
        private bool m_bThrowExOnError = false;
        private ConfigAccess m_ConfigAccess = null;
        private FtpAccess m_DocFtpAccess = null;
        private FtpAccess m_UpgradeFtpAccess = null;
        private FtpAccess m_InfoLibFtpAccess = null;
        private DataAccess m_DocDbAccess = null;
        private string m_RestAccess =string.Empty;
        private StorageMode m_eStorageMode = StorageMode.Unknown;
        private ConnectionMode m_ConnectionMode = ConnectionMode.Unknown;

        private ServerParam()
        {
        }

        private static ServerParam m_Instance = null;

        /// <summary>
        /// ��ȡSystemParam����ʵ��
        /// </summary>
        public static ServerParam Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new ServerParam();
                return m_Instance;
            }
        }

        /// <summary>
        /// ��ȡ�����ó�����·��
        /// </summary>
        public string WorkPath
        {
            set
            {
                this.m_szWorkPath = value;
            }

            get
            {
                if (GlobalMethods.Misc.IsEmptyString(this.m_szWorkPath))
                    this.m_szWorkPath = GlobalMethods.Misc.GetWorkingPath();
                return this.m_szWorkPath;
            }
        }

        /// <summary>
        /// ��ȡ������MDSDBLib�Ĵ�����־�ı���Ŀ¼
        /// </summary>
        public string LogFilePath
        {
            get { return LogManager.Instance.LogFilePath; }
            set { LogManager.Instance.LogFilePath = value; }
        }

        /// <summary>
        /// ��ȡ�����õ�����ORA-12571����ʱ,
        /// �Ƿ�ִ�н�����ջ������XDB���ӵĲ���
        /// </summary>
        public bool AutoClearPool
        {
            get
            {
                return this.m_bAutoClearPool;
            }

            set
            {
                this.m_bAutoClearPool = value;
                if (this.m_DocDbAccess != null)
                    this.m_DocDbAccess.ClearPoolEnabled = value;
            }
        }

        /// <summary>
        /// ��ȡ�����õ���������ʱ�Ƿ��׳��쳣
        /// </summary>
        public bool ThrowExOnError
        {
            get { return this.m_bThrowExOnError; }
            set { this.m_bThrowExOnError = value; }
        }

        /// <summary>
        /// ��ȡFtp������
        /// </summary>
        /// <returns>FtpAccess����</returns>
        public FtpAccess GetDocFtpAccess()
        {
            if (this.m_DocFtpAccess != null)
                return this.m_DocFtpAccess;

            if (this.m_ConfigAccess == null)
                this.m_ConfigAccess = new ConfigAccess();
            FtpConfig ftpConfig = null;
            short shRet = this.m_ConfigAccess.GetDocFtpParams(ref ftpConfig);
            if (shRet != ServerData.ExecuteResult.OK)
                return null;
            if (ftpConfig == null)
                return null;
            this.m_DocFtpAccess = new FtpAccess();
            this.m_DocFtpAccess.FtpIP = ftpConfig.FtpIP;
            this.m_DocFtpAccess.FtpPort = ftpConfig.FtpPort;
            this.m_DocFtpAccess.UserName = ftpConfig.FtpUser;
            this.m_DocFtpAccess.Password = ftpConfig.FtpPwd;
            this.m_DocFtpAccess.FtpMode = ftpConfig.FtpMode;
            return this.m_DocFtpAccess;
        }

        /// <summary>
        /// ��ȡ��������IIS�����ַ
        /// </summary>
        /// <returns>stringIIS�����ַ</returns>
        public string GetHealthTechIISAddress()
        {
            if (!string.IsNullOrEmpty(m_szIISAddress))
                return this.m_szIISAddress;

            if (this.m_ConfigAccess == null)
                this.m_ConfigAccess = new ConfigAccess();
            string szIISAddress = string.Empty;
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.m_ConfigAccess.GetConfigData(ServerData.ConfigKey.IIS, ServerData.ConfigKey.IIS_ADDRESS, ref lstConfigInfos);
            if (lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return string.Empty;
            if (string.IsNullOrEmpty(lstConfigInfos[0].ConfigValue))
                return string.Empty;
            m_szIISAddress = lstConfigInfos[0].ConfigValue;
            return m_szIISAddress;
        }

        /// <summary>
        /// ��ȡ��������Ftp������
        /// </summary>
        /// <returns>FtpAccess����</returns>
        public FtpAccess GetUpgradeFtpAccess()
        {
            if (this.m_UpgradeFtpAccess != null)
                return this.m_UpgradeFtpAccess;

            if (this.m_ConfigAccess == null)
                this.m_ConfigAccess = new ConfigAccess();
            FtpConfig ftpConfig = null;
            short shRet = this.m_ConfigAccess.GetUpgradeFtpParams(ref ftpConfig);
            if (shRet != ServerData.ExecuteResult.OK)
                return null;
            if (ftpConfig == null)
                return null;
            this.m_UpgradeFtpAccess = new FtpAccess();
            this.m_UpgradeFtpAccess.FtpIP = ftpConfig.FtpIP;
            this.m_UpgradeFtpAccess.FtpPort = ftpConfig.FtpPort;
            this.m_UpgradeFtpAccess.UserName = ftpConfig.FtpUser;
            this.m_UpgradeFtpAccess.Password = ftpConfig.FtpPwd;
            this.m_UpgradeFtpAccess.FtpMode = ftpConfig.FtpMode;
            return this.m_UpgradeFtpAccess;
        }

        /// <summary>
        /// ��ȡ������Ϣ��Ftp������
        /// </summary>
        /// <returns>FtpAccess����</returns>
        public FtpAccess GetInfoLibFtpAccess()
        {
            if (this.m_InfoLibFtpAccess != null)
                return this.m_InfoLibFtpAccess;

            if (this.m_ConfigAccess == null)
                this.m_ConfigAccess = new ConfigAccess();
            FtpConfig ftpConfig = null;
            short shRet = this.m_ConfigAccess.GetInfoLibFtpParams(ref ftpConfig);
            if (shRet != ServerData.ExecuteResult.OK)
                return null;
            if (ftpConfig == null)
                return null;
            this.m_InfoLibFtpAccess = new FtpAccess();
            this.m_InfoLibFtpAccess.FtpIP = ftpConfig.FtpIP;
            this.m_InfoLibFtpAccess.FtpPort = ftpConfig.FtpPort;
            this.m_InfoLibFtpAccess.UserName = ftpConfig.FtpUser;
            this.m_InfoLibFtpAccess.Password = ftpConfig.FtpPwd;
            this.m_InfoLibFtpAccess.FtpMode = ftpConfig.FtpMode;
            return this.m_InfoLibFtpAccess;
        }

        /// <summary>
        /// ��ȡ�ĵ��洢ģʽ
        /// </summary>
        /// <returns>StorageModeö��</returns>
        public StorageMode GetStorageMode()
        {
            if (this.m_eStorageMode != StorageMode.Unknown)
                return this.m_eStorageMode;

            if (this.m_ConfigAccess == null)
                this.m_ConfigAccess = new ConfigAccess();
            this.m_eStorageMode = this.m_ConfigAccess.GetStorageMode();
            return this.m_eStorageMode;
        }

        /// <summary>
        /// ��ȡϵͳ����ģʽ
        /// </summary>
        public ConnectionMode GetConnectionMode()
        {
            if (this.m_ConnectionMode == ConnectionMode.Unknown)
            { return this.m_ConnectionMode; }
            else if (this.m_ConnectionMode == ConnectionMode.DB)
                return this.m_ConnectionMode;
            else if (this.m_ConnectionMode == ConnectionMode.Rest)
                return this.m_ConnectionMode;
            else
                return ConnectionMode.Unknown;
        }

        /// <summary>
        /// �����ݿ������ַ���ת��Ϊ���ݿ�����ö��
        /// </summary>
        /// <param name="szDbType">���ݿ������ַ���</param>
        /// <returns>���ݿ�����ö��</returns>
        private DatabaseType GetDatabaseType(string szDbType)
        {
            if (szDbType == ServerData.DatabaseType.SQLSERVER)
            {
                return DatabaseType.SQLSERVER;
            }
            else if (szDbType == ServerData.DatabaseType.ORACLE)
            {
                return DatabaseType.ORACLE;
            }
            else
            {
                LogManager.Instance.WriteLog("SystemParam.GetDatabaseType", "��֧�ֵ����ݿ�����!");
                return DatabaseType.ORACLE;
            }
        }

        /// <summary>
        /// ��ȡϵͳ����ģʽ
        /// </summary>
        private ConnectionMode GetConnectionMode(string szConnectionMode)
        {
            if (szConnectionMode == "DB")
             return ConnectionMode.DB; 
            else if (szConnectionMode == "REST")
             return ConnectionMode.Rest;
            else
             return ConnectionMode.Unknown; 
        }

        /// <summary>
        /// �����ݿ�����ṩ���������ַ���ת��Ϊ����ö��
        /// </summary>
        /// <param name="szDbDriverType">�ṩ���������ַ���</param>
        /// <returns>�ṩ��������ö��</returns>
        private DataProvider GetDataProvider(string szDbDriverType)
        {
            if (szDbDriverType == ServerData.DataProvider.ODBC)
            {
                return DataProvider.Odbc;
            }
            else if (szDbDriverType == ServerData.DataProvider.ODPNET)
            {
                return DataProvider.ODPNET;
            }
            else if (szDbDriverType == ServerData.DataProvider.ORACLE)
            {
                return DataProvider.OracleClient;
            }
            else if (szDbDriverType == ServerData.DataProvider.SQLCLIENT)
            {
                return DataProvider.SqlClient;
            }
            else if (szDbDriverType == ServerData.DataProvider.OLEDB)
            {
                return DataProvider.OleDb;
            }
            else
            {
                LogManager.Instance.WriteLog("SystemParam.GetDataProvider", "��֧�ֵ����ݿ������ṩ����!");
                return DataProvider.OleDb;
            }
        }

        /// <summary>
        /// ��ȡ���ݿ���ʶ���
        /// </summary>
        /// <returns>DataAccess����</returns>
        public DataAccess GetDocDbAccess()
        {
            if (this.m_DocDbAccess != null)
                return this.m_DocDbAccess;

            //��ȡ�����ļ������ݿ�����
            string szDBType = SystemConfig.Instance.Get(ServerData.ConfigFile.NDS_DB_TYPE, string.Empty);
            string szDBDriverType = SystemConfig.Instance.Get(ServerData.ConfigFile.NDS_PROVIDER_TYPE, string.Empty);
            string szConnectionString = SystemConfig.Instance.Get(ServerData.ConfigFile.NDS_CONN_STRING, string.Empty);
            string szConnectionMode = SystemConfig.Instance.Get(ServerData.ConfigFile.NUR_CONN_MODE, string.Empty);
            szDBType = GlobalMethods.Security.DecryptText(szDBType, ServerData.ConfigFile.CONFIG_ENCRYPT_KEY);
            szDBDriverType = GlobalMethods.Security.DecryptText(szDBDriverType, ServerData.ConfigFile.CONFIG_ENCRYPT_KEY);
            szConnectionString = GlobalMethods.Security.DecryptText(szConnectionString, ServerData.ConfigFile.CONFIG_ENCRYPT_KEY);
            szConnectionMode = GlobalMethods.Security.DecryptText(szConnectionMode, ServerData.ConfigFile.NUR_CONN_MODE);
            if (GlobalMethods.Misc.IsEmptyString(szDBType) || GlobalMethods.Misc.IsEmptyString(szDBDriverType)
                || GlobalMethods.Misc.IsEmptyString(szConnectionString))
            {
                LogManager.Instance.WriteLog("SystemParam.GetDbAccess", new string[] { "ConfigFile" }
                    , new object[] { SystemConfig.Instance.ConfigFile }, "���ݿ����ò����а����Ƿ���ֵ!");
                return null;
            }

            this.m_DocDbAccess = new DataAccess();
            this.m_DocDbAccess.ConnectionString = szConnectionString;
            this.m_DocDbAccess.ClearPoolEnabled = this.m_bAutoClearPool;
            this.m_DocDbAccess.DatabaseType = this.GetDatabaseType(szDBType);
            this.m_DocDbAccess.DataProvider = this.GetDataProvider(szDBDriverType);
            this.m_ConnectionMode = this.GetConnectionMode(szConnectionMode);

            return this.m_DocDbAccess;
        }

        /// <summary>
        /// ��ȡResFul������Ϣ
        /// </summary>
        public String GetRestAccess()
        {
            string szRestString = SystemConfig.Instance.Get(ServerData.ConfigFile.NRS_CONN_STRING, string.Empty);
            string szConnectionMode = SystemConfig.Instance.Get(ServerData.ConfigFile.NUR_CONN_MODE, string.Empty);
            szRestString = GlobalMethods.Security.DecryptText(szRestString, ServerData.ConfigFile.CONFIG_ENCRYPT_KEY);
            szConnectionMode = GlobalMethods.Security.DecryptText(szConnectionMode, ServerData.ConfigFile.NUR_CONN_MODE);
            this.m_RestAccess = szRestString;
            this.m_ConnectionMode = this.GetConnectionMode(szConnectionMode);
            return this.m_RestAccess;
        }

        /// <summary>
        /// ��ȡָ�����ĵ���Ϣ��֯ҽ���ĵ�FTP·��
        /// </summary>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <param name="szFileExt">�ĵ���չ��</param>
        /// <returns>�ĵ�FTP·��</returns>
        public string GetFtpDocPath(NurDocInfo docInfo, string szFileExt)
        {
            //���Ӳ��˸�Ŀ¼
            StringBuilder sbDocPath = new StringBuilder();
            sbDocPath.Append("/NURDOC");

            if (docInfo == null || docInfo.PatientID == null)
                return sbDocPath.ToString();

            string szPatientID = docInfo.PatientID.PadLeft(10, '0');

            for (int index = 0; index < 10; index += 2)
            {
                sbDocPath.Append("/");
                sbDocPath.Append(szPatientID.Substring(index, 2));
            }

            // ���Ӿ���Ŀ¼
            sbDocPath.Append("/");
            sbDocPath.Append(docInfo.VisitType);
            sbDocPath.Append("_");
            sbDocPath.Append(docInfo.VisitID);
            sbDocPath.Append("/");
            sbDocPath.Append(string.Format("{0}.{1}", docInfo.DocID, szFileExt));
            return sbDocPath.ToString();
        }

        /// <summary>
        /// ��ȡ������Ϣ��ָ�����ĵ���ϢFTP·��
        /// </summary>
        /// <param name="infoLibInfo">������Ϣ</param>
        /// <returns>�ĵ�FTP·��</returns>
        public string GetFtpInfoPath(InfoLibInfo infoLibInfo)
        {
            //���Ӳ��˸�Ŀ¼
            StringBuilder sbInfoPath = new StringBuilder();
            sbInfoPath.Append("/INFOLIB");

            if (infoLibInfo == null)
                return sbInfoPath.ToString();
            string szTimePoint = infoLibInfo.InfoID.Split('_')[1].PadLeft(18, '0');
            for (int index = 0; index < 14; index += 2)
            {
                sbInfoPath.Append("/");
                sbInfoPath.Append(szTimePoint.Substring(index, 2));
            }

            sbInfoPath.Append(string.Format("{0}{1}", infoLibInfo.InfoID, infoLibInfo.InfoType));
            return sbInfoPath.ToString();
        }

             /// <summary>
        /// ��ȡ������Ϣ��ָ����word�ĵ���ϢFTP·��
        /// </summary>
        /// <param name="WordTempInfo">word</param>
        /// <returns>word�ĵ�FTP·��</returns>
        public string GetFtpWordPath(WordTempInfo WordTempInfo)
        {
            //���Ӳ��˸�Ŀ¼
            StringBuilder sbWordPath = new StringBuilder();
            sbWordPath.Append("/WORDLIB");

            if (WordTempInfo == null)
                return sbWordPath.ToString();
            string szTimePoint = WordTempInfo.TempletID.Split('_')[1].PadLeft(18, '0');
            for (int index = 0; index < 14; index += 2)
            {
                sbWordPath.Append("/");
                sbWordPath.Append(szTimePoint.Substring(index, 2));
            }

            sbWordPath.Append(string.Format("{0}", WordTempInfo.TempletID));
            return sbWordPath.ToString();
        }

        #region "��QC�й�"
        /// <summary>
        /// ��ȡָ�����ĵ���Ϣ��֯ҽ���ĵ�FTP·��
        /// </summary>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <param name="szFileExt">�ĵ���չ��</param>
        /// <returns>�ĵ�FTP·��</returns>
        public string GetFtpDocPath(QCDocInfo docInfo, string szFileExt)
        {
            //���������밲ȫ�����¼�ĵ���Ŀ¼
            StringBuilder sbDocPath = new StringBuilder();
            sbDocPath.Append("/QCDOC");

            if (docInfo == null || docInfo.CreatorID == null)
                return sbDocPath.ToString();

            string szCreatorID = docInfo.CreatorID.PadLeft(10, '0');

            for (int index = 0; index < 10; index += 2)
            {
                sbDocPath.Append("/");
                sbDocPath.Append(szCreatorID.Substring(index, 2));
            }

            sbDocPath.Append("/");
            sbDocPath.Append(string.Format("{0}.{1}", docInfo.DocID, szFileExt));
            return sbDocPath.ToString();
        }
        #endregion
    }
}
