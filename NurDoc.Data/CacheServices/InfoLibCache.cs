// ***********************************************************
// ������Ӳ���ϵͳ,������Ϣ���ĵ����������.
// ��Ҫ���滤����Ϣ�ĵ��б�,�Լ��ļ�,����ظ�����
// Creator:YangMingkun  Date:2013-11-14
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class InfoLibCache
    {
        private static InfoLibCache m_instance = null;

        /// <summary>
        /// ��ȡDocTypeCacheʵ��
        /// </summary>
        public static InfoLibCache Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new InfoLibCache();
                return m_instance;
            }
        }

        private InfoLibCache()
        {
            this.m_szCacheIndexFile = GlobalMethods.Misc.GetWorkingPath()
                + @"\Templets\InfoLibs.xml";
        }

        /// <summary>
        /// ģ�屾�ػ��������ļ�·��
        /// </summary>
        private string m_szCacheIndexFile = null;

        /// <summary>
        /// ��ȡָ��ID���ĵ�������Ϣ
        /// </summary>
        /// <param name="szInfoID">�ĵ����ʹ���</param>
        /// <returns>�ĵ�������Ϣ</returns>
        public InfoLibInfo GetInfoLibInfo(string szInfoID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szInfoID))
                return null;

            //���²�ѯ��ȡ�ĵ�������Ϣ
            List<InfoLibInfo> lstInfoLibInfo = null;
            short shRet = InfoLibService.Instance.GetInfoLibInfos(szInfoID, ref lstInfoLibInfo);
            if (shRet != SystemConst.ReturnValue.OK)
                return null;
            return lstInfoLibInfo[0];
        }

        /// <summary>
        /// ��ȡ��ģ�屾�ػ���·��
        /// </summary>
        /// <param name="szInfoID">��ϢID��</param>
        /// <param name="szInfoType">��Ϣ�ĵ�����</param>
        /// <returns>��ģ�屾�ػ���·��</returns>
        private string GetInfoLibCachePath(string szInfoID, string szInfoType)
        {
            return string.Format(@"{0}\Templets\InfoLibs\{1}{2}", GlobalMethods.Misc.GetWorkingPath(), szInfoID, szInfoType);
        }

        /// <summary>
        /// ��ȡ�ĵ�ģ����޸�ʱ��
        /// </summary>
        /// <param name="szInfoID">������ID(����Ǳ�ģ��)</param>
        /// <returns>DateTime</returns>
        public DateTime GetInfoLibModifyTime(string szInfoID)
        {
            if (!System.IO.File.Exists(this.m_szCacheIndexFile))
                return DateTime.Now;

            XmlDocument xmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_szCacheIndexFile);
            if (xmlDoc == null)
                return DateTime.Now;

            string szXPath = null;
            if (!GlobalMethods.Misc.IsEmptyString(szInfoID))
                szXPath = string.Format("/InfoLibs/Templet[@ID='{0}']", szInfoID.Trim());
            else
                return DateTime.Now;

            XmlNode templetNode = GlobalMethods.Xml.SelectXmlNode(xmlDoc, szXPath);
            if (templetNode == null)
                return DateTime.Now;

            string szModifyTime = null;
            if (!GlobalMethods.Xml.GetXmlNodeValue(templetNode, "./@ModifyTime", ref szModifyTime))
                return DateTime.Now;

            DateTime dtModifyTime = DateTime.Now;
            GlobalMethods.Convert.StringToDate(szModifyTime, ref dtModifyTime);
            return dtModifyTime;
        }

        /// <summary>
        /// ��ȡָ�������Ͷ�Ӧ�ı�ģ������
        /// </summary>
        /// <param name="infoLibInfo">�ĵ���Ϣ</param>
        /// <param name="szTempletPath">�ĵ�·��</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short GetInfoLibTemplet(InfoLibInfo infoLibInfo, ref string szTempletPath)
        {
            if (infoLibInfo == null || GlobalMethods.Misc.IsEmptyString(infoLibInfo.InfoID))
                return SystemConst.ReturnValue.EXCEPTION;

            szTempletPath = this.GetInfoLibCachePath(infoLibInfo.InfoID, infoLibInfo.InfoType);

            // ��������Ѿ����ĵ�����,��ô���ر��ص�
            DateTime dtModifyTime = this.GetInfoLibModifyTime(infoLibInfo.InfoID);
            if (dtModifyTime.CompareTo(infoLibInfo.ModifyTime) == 0)
            {
                return SystemConst.ReturnValue.OK;
            }
            // ��������Ŀ¼
            string szParentDir = GlobalMethods.IO.GetFilePath(szTempletPath);
            if (!GlobalMethods.IO.CreateDirectory(szParentDir))
            {
                LogManager.Instance.WriteLog("InfoLibCache.GetInfoLibTemplet"
                    , new string[] { "infoLibInfo" }, new object[] { infoLibInfo }, "��Ϣ���ļ�����Ŀ¼����ʧ��!", null);
                return SystemConst.ReturnValue.EXCEPTION;
            }
            // д�û�ģ�屾��������Ϣ
            if (!this.CacheInfoLib(infoLibInfo))
            {
                LogManager.Instance.WriteLog("InfoLibCache.GetInfoLibTemplet"
                    , new string[] { "infoLibInfo" }, new object[] { infoLibInfo }, "��ģ�建�浽����ʧ��!", null);
                return SystemConst.ReturnValue.EXCEPTION;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ��ָ����ϵͳģ�����ݻ��浽����
        /// </summary>
        /// <param name="infoLibInfo">�ĵ�������Ϣ</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool CacheInfoLib(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null)
                return false;

            //����ϵͳģ�����ݵ�����
            string szTempletPath = this.GetInfoLibCachePath(infoLibInfo.InfoID, infoLibInfo.InfoType);
            // �����ĵ�ģ������
            string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(infoLibInfo);
            short shRet = InfoLibService.Instance.DownloadInfoLibFile(szRemoteFile, szTempletPath);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                LogManager.Instance.WriteLog("InfoLibCache.CacheInfoLib"
                   , new string[] { "infoLibInfo" }, new object[] { infoLibInfo }
                    , "�ļ�ftp����ʧ��!", null);
                return false;
            }
            //װ��ģ�屾�������ļ�
            if (!System.IO.File.Exists(this.m_szCacheIndexFile))
            {
                if (!GlobalMethods.Xml.CreateXmlFile(this.m_szCacheIndexFile, "InfoLibs"))
                    return false;
            }
            XmlDocument xmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_szCacheIndexFile);
            if (xmlDoc == null)
            {
                return false;
            }

            // ��ӻ����ģ�屾�������ڵ�
            string szXPath = string.Format("/InfoLibs/Templet[@ID='{0}']", infoLibInfo.InfoID);
            XmlNode templetXmlNode = null;
            try
            {
                templetXmlNode = GlobalMethods.Xml.SelectXmlNode(xmlDoc, szXPath);
                if (templetXmlNode == null)
                {
                    templetXmlNode = GlobalMethods.Xml.CreateXmlNode(xmlDoc, null, "Templet", null);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("FormCache.CacheInfoLib"
                    , new string[] { "docTypeInfo" }, new object[] { infoLibInfo }, "��ģ�屾��������Ϣд��ʧ��!", ex);
                return false;
            }
            if (templetXmlNode == null)
            {
                LogManager.Instance.WriteLog("FormCache.CacheInfoLib"
                    , new string[] { "docTypeInfo", "szXPath" }, new object[] { infoLibInfo, szXPath }
                    , "��ģ�屾��������Ϣд��ʧ��!�޷������ڵ�!", null);
                return false;
            }

            //����ģ�������ڵ������ֵ
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "ID", infoLibInfo.InfoID))
                return false;
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "Name", infoLibInfo.InfoName))
                return false;
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "ModifyTime", infoLibInfo.ModifyTime.ToString()))
                return false;
            if (!GlobalMethods.Xml.SaveXmlDocument(xmlDoc, this.m_szCacheIndexFile))
                return false;
            return true;
        }
    }
}
