// ***********************************************************
// 数据库访问层系统版本升级有关的数据接口访问类.
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
        /// 获取FTP服务器访问对象实例
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
        /// 获取FTP服务器访问对象实例
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
        /// 根据文件名字从FTP中下载文件
        /// </summary>
        /// <param name="strRemoteFileName">服务器上的文件名称</param>
        /// <param name="strLocalFileName">本地的文件名称</param>
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
        /// 将Log上传到Ftp服务器
        /// </summary>
        /// <param name="strLocalFile">本地的文件名称</param>
        /// <param name="szRemoteFile">服务器上的文件名称</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), strLocalFile, "Log文档上传失败!");
            }
            finally
            {
                base.FtpAccess.CloseConnection();
            }
        }
    }
}
