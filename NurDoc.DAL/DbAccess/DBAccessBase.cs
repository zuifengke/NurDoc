// ***********************************************************
// ���ݿ���ʲ����ݷ��ʻ���,�����ฺ���ṩһЩ��������뷽��.
// ���ڷ�ֹ�ظ�ʵ����,���Ч��
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
        /// ��ȡ���ݿ���ʶ���ʵ��
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
        /// ��ȡ������ģ��Ĵ洢ģʽ
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
        /// ��ȡ��������ϵͳ����ģʽ
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
        /// ��ȡFTP���������ʶ���ʵ��
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
        /// ����ӿڲ������쳣,��Ҫ��¼�쳣��־,�Լ������Ƿ�����ò��׳�
        /// </summary>
        /// <param name="ex">�쳣��Ϣ</param>
        /// <param name="method">�쳣����</param>
        /// <param name="param">��������</param>
        /// <param name="error">����������Ϣ</param>
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
