// ***********************************************************
// 数据库访问层系统运行参数管理类,用于调用层来修改运行参数.
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
        /// 获取SystemParam对象实例
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
        /// 获取或设置程序工作路径
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
        /// 获取或设置MDSDBLib的错误日志的备份目录
        /// </summary>
        public string LogFilePath
        {
            get { return LogManager.Instance.LogFilePath; }
            set { LogManager.Instance.LogFilePath = value; }
        }

        /// <summary>
        /// 获取或设置当出现ORA-12571错误时,
        /// 是否执行进行清空缓存池中XDB连接的操作
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
        /// 获取或设置当发生错误时是否抛出异常
        /// </summary>
        public bool ThrowExOnError
        {
            get { return this.m_bThrowExOnError; }
            set { this.m_bThrowExOnError = value; }
        }

        /// <summary>
        /// 获取Ftp访问类
        /// </summary>
        /// <returns>FtpAccess对象</returns>
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
        /// 获取健康教育IIS服务地址
        /// </summary>
        /// <returns>stringIIS服务地址</returns>
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
        /// 获取程序升级Ftp访问类
        /// </summary>
        /// <returns>FtpAccess对象</returns>
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
        /// 获取护理信息库Ftp访问类
        /// </summary>
        /// <returns>FtpAccess对象</returns>
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
        /// 获取文档存储模式
        /// </summary>
        /// <returns>StorageMode枚举</returns>
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
        /// 获取系统连接模式
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
        /// 将数据库类型字符串转换为数据库类型枚举
        /// </summary>
        /// <param name="szDbType">数据库类型字符串</param>
        /// <returns>数据库类型枚举</returns>
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
                LogManager.Instance.WriteLog("SystemParam.GetDatabaseType", "不支持的数据库驱动!");
                return DatabaseType.ORACLE;
            }
        }

        /// <summary>
        /// 获取系统连接模式
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
        /// 将数据库访问提供程序类型字符串转换为类型枚举
        /// </summary>
        /// <param name="szDbDriverType">提供程序类型字符串</param>
        /// <returns>提供程序类型枚举</returns>
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
                LogManager.Instance.WriteLog("SystemParam.GetDataProvider", "不支持的数据库驱动提供程序!");
                return DataProvider.OleDb;
            }
        }

        /// <summary>
        /// 获取数据库访问对象
        /// </summary>
        /// <returns>DataAccess对象</returns>
        public DataAccess GetDocDbAccess()
        {
            if (this.m_DocDbAccess != null)
                return this.m_DocDbAccess;

            //读取配置文件中数据库配置
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
                    , new object[] { SystemConfig.Instance.ConfigFile }, "数据库配置参数中包含非法的值!");
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
        /// 获取ResFul连接信息
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
        /// 获取指定的文档信息组织医疗文档FTP路径
        /// </summary>
        /// <param name="docInfo">文档信息</param>
        /// <param name="szFileExt">文档扩展名</param>
        /// <returns>文档FTP路径</returns>
        public string GetFtpDocPath(NurDocInfo docInfo, string szFileExt)
        {
            //链接病人根目录
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

            // 链接就诊目录
            sbDocPath.Append("/");
            sbDocPath.Append(docInfo.VisitType);
            sbDocPath.Append("_");
            sbDocPath.Append(docInfo.VisitID);
            sbDocPath.Append("/");
            sbDocPath.Append(string.Format("{0}.{1}", docInfo.DocID, szFileExt));
            return sbDocPath.ToString();
        }

        /// <summary>
        /// 获取护理信息库指定的文档信息FTP路径
        /// </summary>
        /// <param name="infoLibInfo">护理信息</param>
        /// <returns>文档FTP路径</returns>
        public string GetFtpInfoPath(InfoLibInfo infoLibInfo)
        {
            //链接病人根目录
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
        /// 获取护理信息库指定的word文档信息FTP路径
        /// </summary>
        /// <param name="WordTempInfo">word</param>
        /// <returns>word文档FTP路径</returns>
        public string GetFtpWordPath(WordTempInfo WordTempInfo)
        {
            //链接病人根目录
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

        #region "与QC有关"
        /// <summary>
        /// 获取指定的文档信息组织医疗文档FTP路径
        /// </summary>
        /// <param name="docInfo">文档信息</param>
        /// <param name="szFileExt">文档扩展名</param>
        /// <returns>文档FTP路径</returns>
        public string GetFtpDocPath(QCDocInfo docInfo, string szFileExt)
        {
            //链接质量与安全管理记录文档根目录
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
