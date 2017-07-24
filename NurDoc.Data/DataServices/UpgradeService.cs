// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之程序升级数据访问接口封装类.
// Creator:OuFengfang  Date:2012-11-23
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class UpgradeService
    {
        private static UpgradeService m_instance = null;

        /// <summary>
        /// 获取程序升级服务实例
        /// </summary>
        public static UpgradeService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new UpgradeService();
                return m_instance;
            }
        }

        private UpgradeService()
        {
        }

        /// <summary>
        /// 获取程序升级版本信息
        /// </summary>
        /// <param name="strUpgrdateVersion">程序升级版本信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUpgradeVersion(ref string strUpgrdateVersion)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            //测试方法后续删除
            short shRet1 = SystemContext.Instance.ConfigAccess.Test("Test");

            List<ConfigInfo> lstConfigInfos = new List<ConfigInfo>();
            short shRet = SystemContext.Instance.ConfigAccess.GetConfigData(ServerData.ConfigKey.UPGRADE_VERSION
                , ServerData.ConfigKey.UPGRADE_VERSION, ref lstConfigInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            if (lstConfigInfos.Count > 0)
            {
                strUpgrdateVersion = lstConfigInfos[0].ConfigValue;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据文件名字从FTP中下载文件
        /// </summary>
        /// <param name="strRemoteFileName">服务器上的文件名称</param>
        /// <param name="strLocalFileName">本地的文件名称</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DownloadUpgradeFile(string strRemoteFileName, string strLocalFileName)
        {
            if (SystemContext.Instance.UpgradeAccess == null)
                return SystemConst.ReturnValue.FAILED;
            return SystemContext.Instance.UpgradeAccess.DownloadUpgradeFile(strRemoteFileName, strLocalFileName);
        }

        /// <summary>
        /// 将Log上传到Ftp服务器
        /// </summary>
        /// <param name="strLocalFile">本地的文件名称</param>
        /// <param name="szRemoteFile">服务器上的文件名称</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UploadLogFile(string strLocalFile, string szRemoteFile)
        {
            if (SystemContext.Instance.UpgradeAccess == null)
                return SystemConst.ReturnValue.FAILED;
            return SystemContext.Instance.UpgradeAccess.UploadLogFile(strLocalFile, szRemoteFile);
        }
    }
}
