// ***********************************************************
// ������Ӳ���ϵͳ,
// ���ݷ��ʲ�ӿڷ�װ֮�����������ݷ��ʽӿڷ�װ��.
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
        /// ��ȡ������������ʵ��
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
        /// ��ȡ���������汾��Ϣ
        /// </summary>
        /// <param name="strUpgrdateVersion">���������汾��Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUpgradeVersion(ref string strUpgrdateVersion)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            //���Է�������ɾ��
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
        /// �����ļ����ִ�FTP�������ļ�
        /// </summary>
        /// <param name="strRemoteFileName">�������ϵ��ļ�����</param>
        /// <param name="strLocalFileName">���ص��ļ�����</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DownloadUpgradeFile(string strRemoteFileName, string strLocalFileName)
        {
            if (SystemContext.Instance.UpgradeAccess == null)
                return SystemConst.ReturnValue.FAILED;
            return SystemContext.Instance.UpgradeAccess.DownloadUpgradeFile(strRemoteFileName, strLocalFileName);
        }

        /// <summary>
        /// ��Log�ϴ���Ftp������
        /// </summary>
        /// <param name="strLocalFile">���ص��ļ�����</param>
        /// <param name="szRemoteFile">�������ϵ��ļ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UploadLogFile(string strLocalFile, string szRemoteFile)
        {
            if (SystemContext.Instance.UpgradeAccess == null)
                return SystemConst.ReturnValue.FAILED;
            return SystemContext.Instance.UpgradeAccess.UploadLogFile(strLocalFile, szRemoteFile);
        }
    }
}
