// ***********************************************************
// 数据库访问层数据访问基类,本基类负责提供一些共享变量与方法.
// 用于防止重复实例化,提高效率
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Reflection;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;
using Heren.Common.Libraries.Ftp;

namespace Heren.NurDoc.DAL.DbAccess
{
    public abstract class DBAccessBase
    {
        private DataAccess m_DbAccess = null;
        private FtpAccess m_FtpAccess = null;
        private StorageMode m_StorageMode = StorageMode.Unknown;
        private ConnectionMode m_ConnectionMode = ConnectionMode.Unknown;

        public DBAccessBase()
        {
        }

        /// <summary>
        /// 获取数据库访问对象实例
        /// </summary>
        protected DataAccess DataAccess
        {
            get
            {
                if (this.m_DbAccess == null)
                    this.m_DbAccess = ServerParam.Instance.GetDocDbAccess();
                return this.m_DbAccess;
            }
        }

        /// <summary>
        /// 获取病历及模板的存储模式
        /// </summary>
        protected StorageMode StorageMode
        {
            get
            {
                if (this.m_StorageMode == StorageMode.Unknown)
                    this.m_StorageMode = ServerParam.Instance.GetStorageMode();
                return this.m_StorageMode;
            }
        }

        /// <summary>
        /// 获取护理文书系统连接模式
        /// </summary>
        protected ConnectionMode ConnectionMode
        {
            get
            {
                if (this.m_ConnectionMode == ConnectionMode.Unknown)
                    this.m_ConnectionMode = ServerParam.Instance.GetConnectionMode();
                return this.m_ConnectionMode;
            }
        }

        /// <summary>
        /// 获取FTP服务器访问对象实例
        /// </summary>
        protected FtpAccess FtpAccess
        {
            get
            {
                if (this.m_FtpAccess == null)
                    this.m_FtpAccess = ServerParam.Instance.GetDocFtpAccess();
                return this.m_FtpAccess;
            }
        }

        /// <summary>
        /// 处理接口产生的异常,主要记录异常日志,以及决定是否向调用层抛出
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="method">异常方法</param>
        /// <param name="param">方法参数</param>
        /// <param name="error">其他错误信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        protected short HandleException(Exception ex, MethodBase method, string param, string error)
        {
            string szMethodDesc = string.Empty;
            if (method != null)
                szMethodDesc = method.ToString();
            LogManager.Instance.WriteLog(szMethodDesc, new string[] { "param", "db-info", "ftp-info" }
                , new object[] { param, this.m_DbAccess, this.m_FtpAccess }, error, ex);

            if (ServerParam.Instance.ThrowExOnError)
                throw ex;
            return ServerData.ExecuteResult.EXCEPTION;
        }
    }
}
