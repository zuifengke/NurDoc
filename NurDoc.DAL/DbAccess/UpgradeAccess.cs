// ***********************************************************
// ���ݿ���ʲ�ϵͳ�汾�����йص����ݽӿڷ�����.
// Creator:OuFengfang  Date:2012-11-23
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;
using Heren.Common.Libraries.Ftp;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class UpgradeAccess : DBAccessBase
    {
        private FtpAccess m_UpgradeFtpAccess = null;

        /// <summary>
        /// ��ȡFTP���������ʶ���ʵ��
        /// </summary>
        protected FtpAccess UpgradeFtpAccess
        {
            get
            {
                if (this.m_UpgradeFtpAccess == null)
                    this.m_UpgradeFtpAccess = ServerParam.Instance.GetUpgradeFtpAccess();
                return this.m_UpgradeFtpAccess;
            }
        }

        private FtpAccess m_LogFtpAccess = null;

        /// <summary>
        /// ��ȡFTP���������ʶ���ʵ��
        /// </summary>
        protected FtpAccess LogFtpAccess
        {
            get
            {
                if (this.m_LogFtpAccess == null)
                    this.m_LogFtpAccess = ServerParam.Instance.GetDocFtpAccess();
                return this.m_LogFtpAccess;
            }
        }

        /// <summary>
        /// �����ļ����ִ�FTP�������ļ�
        /// </summary>
        /// <param name="strRemoteFileName">�������ϵ��ļ�����</param>
        /// <param name="strLocalFileName">���ص��ļ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DownloadUpgradeFile(string strRemoteFileName, string strLocalFileName)
        {
            if (this.UpgradeFtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!this.UpgradeFtpAccess.OpenConnection())
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (!this.UpgradeFtpAccess.Download(strRemoteFileName, strLocalFileName))
            {
                this.UpgradeFtpAccess.CloseConnection();
                return ServerData.ExecuteResult.EXCEPTION;
            }
            this.UpgradeFtpAccess.CloseConnection();
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ��Log�ϴ���Ftp������
        /// </summary>
        /// <param name="strLocalFile">���ص��ļ�����</param>
        /// <param name="szRemoteFile">�������ϵ��ļ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UploadLogFile(string strLocalFile, string szRemoteFile)
        {
            if (base.FtpAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            try
            {
                if (!base.FtpAccess.OpenConnection())
                    return ServerData.ExecuteResult.EXCEPTION;
                string szRemoteDir = GlobalMethods.IO.GetFilePath(szRemoteFile);
                if (!base.FtpAccess.CreateDirectory(szRemoteDir))
                    return ServerData.ExecuteResult.EXCEPTION;
                if (!base.FtpAccess.Upload(strLocalFile, szRemoteFile))
                    return ServerData.ExecuteResult.EXCEPTION;
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), strLocalFile, "Log�ĵ��ϴ�ʧ��!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
            }
        }
    }
}
