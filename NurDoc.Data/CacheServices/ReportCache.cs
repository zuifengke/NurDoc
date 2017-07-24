// ***********************************************************
// ������Ӳ���ϵͳ,����������Ϣ���������.
// ��Ҫ���汨�������б�,�Լ�����ģ���ļ�,����ظ�����
// Creator:YangMingkun  Date:2012-8-26
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class ReportCache
    {
        private static ReportCache m_instance = null;

        /// <summary>
        /// ��ȡReportCacheʵ��
        /// </summary>
        public static ReportCache Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ReportCache();
                return m_instance;
            }
        }

        private ReportCache()
        {
            this.m_szCacheIndexFile = GlobalMethods.Misc.GetWorkingPath()
                + @"\Templets\Reports.xml";
        }

        /// <summary>
        /// ģ�屾�ػ��������ļ�·��
        /// </summary>
        private string m_szCacheIndexFile = null;

        /// <summary>
        /// ��ű������͵Ĺ�ϣ��
        /// </summary>
        private Dictionary<string, ReportTypeInfo> m_htReportTypeTable = null;

        /// <summary>
        /// ������ű������͵Ĺ�ϣ��
        /// </summary>
        private Dictionary<string, List<ReportTypeInfo>> m_htReportClassTable = null;

        /// <summary>
        /// �жϱ���������������ʱʹ�õ�Hash��
        /// </summary>
        private Dictionary<string, WardReportType> m_htWardReportTable = null;

        /// <summary>
        /// ���ز��������б�
        /// </summary>
        /// <param name="szApplyEnv">Ӧ�û���</param>
        /// <returns>����������Ϣ�б�</returns>
        public List<ReportTypeInfo> GetReportTypeList(string szApplyEnv)
        {
            if (GlobalMethods.Misc.IsEmptyString(szApplyEnv))
                return null;
            if (this.m_htReportClassTable != null
                && this.m_htReportClassTable.ContainsKey(szApplyEnv))
            {
                return this.m_htReportClassTable[szApplyEnv];
            }

            List<ReportTypeInfo> lstReportTypeInfos = null;
            short shRet = TempletService.Instance.GetReportTypeInfos(szApplyEnv, ref lstReportTypeInfos);
            if (shRet != SystemConst.ReturnValue.OK)
                return null;

            if (lstReportTypeInfos == null || lstReportTypeInfos.Count <= 0)
                return new List<ReportTypeInfo>();

            if (this.m_htReportClassTable == null)
                this.m_htReportClassTable = new Dictionary<string, List<ReportTypeInfo>>();
            this.m_htReportClassTable.Add(szApplyEnv, lstReportTypeInfos);

            if (this.m_htReportTypeTable == null)
                this.m_htReportTypeTable = new Dictionary<string, ReportTypeInfo>();

            for (int index = 0; index < lstReportTypeInfos.Count; index++)
            {
                ReportTypeInfo reportTypeInfo = lstReportTypeInfos[index];
                if (reportTypeInfo == null || string.IsNullOrEmpty(reportTypeInfo.ReportTypeID))
                    continue;
                if (!this.m_htReportTypeTable.ContainsKey(reportTypeInfo.ReportTypeID))
                    this.m_htReportTypeTable.Add(reportTypeInfo.ReportTypeID, reportTypeInfo);
            }
            return lstReportTypeInfos;
        }

        /// <summary>
        /// ��ȡָ��ID�ı���������Ϣ
        /// </summary>
        /// <param name="szReportTypeID">�������ʹ���</param>
        /// <returns>����������Ϣ</returns>
        public ReportTypeInfo GetReportTypeInfo(string szReportTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szReportTypeID))
                return null;

            //������Ӳ�������
            if (this.m_htReportTypeTable == null)
                this.m_htReportTypeTable = new Dictionary<string, ReportTypeInfo>();
            if (this.m_htReportTypeTable.ContainsKey(szReportTypeID))
                return this.m_htReportTypeTable[szReportTypeID];

            //���²�ѯ��ȡ����������Ϣ
            ReportTypeInfo reportTypeInfo = null;
            short shRet = TempletService.Instance.GetReportTypeInfo(szReportTypeID, ref reportTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK || reportTypeInfo == null)
                return null;
            this.m_htReportTypeTable.Add(szReportTypeID, reportTypeInfo);
            return reportTypeInfo;
        }

        /// <summary>
        /// ��ȡָ��Ӧ�û����µĲ�������ģ��
        /// </summary>
        /// <param name="szApplyEnv">Ӧ�û���</param>
        /// <returns>����ģ����Ϣ</returns>
        public ReportTypeInfo GetWardReportType(string szApplyEnv)
        {
            List<ReportTypeInfo> lstReportTypeInfos = this.GetReportTypeList(szApplyEnv);
            if (lstReportTypeInfos == null || lstReportTypeInfos.Count <= 0)
                return null;

            //����ѡȡ���������õı���ģ��
            ReportTypeInfo hospitalReportTypeInfo = null;
            ReportTypeInfo WardReportTypeInfo = null;
            foreach (ReportTypeInfo reportTypeInfo in lstReportTypeInfos)
            {
                if (reportTypeInfo.IsValid && !reportTypeInfo.IsFolder)
                {
                    if (this.IshospitalReportType(reportTypeInfo.ReportTypeID))
                    {
                        if (hospitalReportTypeInfo == null)
                            hospitalReportTypeInfo = reportTypeInfo;
                        else
                            continue;
                    }
                    else if (this.IsWardReportType(reportTypeInfo.ReportTypeID))
                        WardReportTypeInfo = reportTypeInfo;
                }
            }
            if (WardReportTypeInfo == null)
                return hospitalReportTypeInfo;
            else
                return WardReportTypeInfo;
        }

        /// <summary>
        /// ��ȡָ��Ӧ�û���ָ��������ָ�����Ƶı���ģ��
        /// </summary>
        /// <param name="szApplyEnv">Ӧ�û���</param>
        /// <param name="szReportName">������������</param>
        /// <returns>����ģ����Ϣ</returns>
        public ReportTypeInfo GetWardReportType(string szApplyEnv, string szReportName)
        {
            List<ReportTypeInfo> lstReportTypeInfos = this.GetReportTypeList(szApplyEnv);
            if (lstReportTypeInfos == null)
                return null;

            //����ѡȡ������ָ�����Ƶı���ģ��
            //1��ǰ������������ȫƥ��
            //2��ǰ����������ǰ����ƥ��
            //3ȫԺ��������ȫƥ��
            //4ȫԺ������ǰ����ƥ��
            ReportTypeInfo reportTypeInfo1 = null;
            ReportTypeInfo reportTypeInfo2 = null;
            ReportTypeInfo reportTypeInfo3 = null;
            ReportTypeInfo reportTypeInfo4 = null;
            foreach (ReportTypeInfo reportTypeInfo in lstReportTypeInfos)
            {
                if (reportTypeInfo.IsValid && !reportTypeInfo.IsFolder)
                {
                    if (reportTypeInfo.ReportTypeName.StartsWith(szReportName)
                        && this.IshospitalReportType(reportTypeInfo.ReportTypeID))
                    {
                        if (reportTypeInfo.ReportTypeName == szReportName)
                            reportTypeInfo3 = reportTypeInfo;
                        if (reportTypeInfo4 == null)
                            reportTypeInfo4 = reportTypeInfo;
                    }
                    else if (reportTypeInfo.ReportTypeName.StartsWith(szReportName)
                        && this.IsWardReportType(reportTypeInfo.ReportTypeID))
                    {
                        if (reportTypeInfo.ReportTypeName == szReportName)
                            reportTypeInfo1 = reportTypeInfo;
                        if (reportTypeInfo2 == null)
                            reportTypeInfo2 = reportTypeInfo;
                    }
                }
            }
            if (reportTypeInfo1 != null)
                return reportTypeInfo1;
            else if (reportTypeInfo2 != null)
                return reportTypeInfo2;
            else if (reportTypeInfo3 != null)
                return reportTypeInfo3;
            else 
                return reportTypeInfo4;
        }

        /// <summary>
        /// ���ص�ǰ�����Ĳ��������б�
        /// </summary>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool LoadWardReportTypeList()
        {
            List<WardReportType> lstWardReportTypes = null;
            short shRet = TempletService.Instance.GetWardReportTypeList(string.Empty, ref lstWardReportTypes);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;

            if (this.m_htWardReportTable == null)
                this.m_htWardReportTable = new Dictionary<string, WardReportType>();
            this.m_htWardReportTable.Clear();

            if (lstWardReportTypes == null || lstWardReportTypes.Count <= 0)
                return true;

            string szWardReportType = null;
            for (int index = 0; index < lstWardReportTypes.Count; index++)
            {
                WardReportType wardReportType = lstWardReportTypes[index];
                if (wardReportType == null || string.IsNullOrEmpty(wardReportType.ReportTypeID))
                    continue;

                //���������͵ĵ�һ������������Ϣ���뵽hash��
                //�Ա�����Ϸ�����жϸò��������Ƿ���������������
                if (!this.m_htWardReportTable.ContainsKey(wardReportType.ReportTypeID))
                    this.m_htWardReportTable.Add(wardReportType.ReportTypeID, wardReportType);

                //ͬʱ�����������벡���Ķ�Ӧ��ϵ���뵽hash��
                szWardReportType = string.Concat(wardReportType.ReportTypeID, "_", wardReportType.WardCode);
                if (!this.m_htWardReportTable.ContainsKey(szWardReportType))
                    this.m_htWardReportTable.Add(szWardReportType, wardReportType);
            }
            return true;
        }

        /// <summary>
        /// ��ȡָ���ı��������Ƿ��ǵ�ǰ�û�������
        /// </summary>
        /// <param name="szDocTypeID">ָ���ı�������</param>
        /// <returns>�Ƿ��ǵ�ǰ��������</returns>
        public bool IsWardReportType(string szDocTypeID)
        {
            if (this.m_htWardReportTable == null)
            {
                if (!this.LoadWardReportTypeList())
                    return false;
            }
            if (this.m_htWardReportTable == null || SystemContext.Instance.LoginUser == null)
                return true;

            //�����������δ������������,��ô��Ϊ��ͨ��
            if (string.IsNullOrEmpty(szDocTypeID) || !this.m_htWardReportTable.ContainsKey(szDocTypeID))
                return true;

            //���ظò������������������Ƿ��е�ǰ�û�����
            string szWardReportType = string.Concat(szDocTypeID, "_", SystemContext.Instance.LoginUser.WardCode);
            return this.m_htWardReportTable.ContainsKey(szWardReportType);
        }

        /// <summary>
        /// ��ȡָ���ı��������Ƿ���ȫԺ��
        /// </summary>
        /// <param name="szDocTypeID">ָ���ı�������</param>
        /// <returns>�Ƿ���ȫԺ�Ĳ���</returns>
        public bool IshospitalReportType(string szDocTypeID)
        {
            if (this.m_htWardReportTable == null)
            {
                if (!this.LoadWardReportTypeList())
                    return false;
            }
            if (this.m_htWardReportTable == null || SystemContext.Instance.LoginUser == null)
                return true;

            //�����������δ������������,��ô��Ϊ��ͨ��
            if (string.IsNullOrEmpty(szDocTypeID) || !this.m_htWardReportTable.ContainsKey(szDocTypeID))
                return true;
            return false;
        }

        /// <summary>
        /// ��ȡ����ģ�屾�ػ���·��
        /// </summary>
        /// <param name="szTempletID">ģ��ID</param>
        /// <returns>ģ�屾�ػ���·��</returns>
        private string GetReportCachePath(string szTempletID)
        {
            return string.Format(@"{0}\Templets\Reports\{1}.hrdt", GlobalMethods.Misc.GetWorkingPath(), szTempletID);
        }

        /// <summary>
        /// ��ȡ����ģ����޸�ʱ��
        /// </summary>
        /// <param name="szTempletID">����ģ��ID(����Ǳ���ģ��)</param>
        /// <returns>DateTime</returns>
        public DateTime GetReportModifyTime(string szTempletID)
        {
            if (!System.IO.File.Exists(this.m_szCacheIndexFile))
                return DateTime.Now;

            XmlDocument xmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_szCacheIndexFile);
            if (xmlDoc == null)
                return DateTime.Now;

            string szXPath = null;
            if (!GlobalMethods.Misc.IsEmptyString(szTempletID))
                szXPath = string.Format("/Reports/Templet[@ID='{0}']", szTempletID.Trim());
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
        /// ���ݱ���ģ��ID,��ȡ����ģ������
        /// </summary>
        /// <param name="szReportTypeID">����ģ��ID</param>
        /// <param name="byteTempletData">���ص�ģ������</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public bool GetReportTemplet(string szReportTypeID, ref byte[] byteTempletData)
        {
            ReportTypeInfo reportTypeInfo = this.GetReportTypeInfo(szReportTypeID);
            if (reportTypeInfo == null)
            {
                LogManager.Instance.WriteLog("FormCache.GetReportTemplet"
                     , new string[] { "szReportTypeID" }, new object[] { szReportTypeID }, "��������!");
                return false;
            }
            return this.GetReportTemplet(reportTypeInfo, ref byteTempletData);
        }

        /// <summary>
        /// ���ݱ���ģ����Ϣ,��ȡ����ģ������
        /// </summary>
        /// <param name="reportTypeInfo">����ģ����Ϣ</param>
        /// <param name="byteTempletData">����ģ������</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public bool GetReportTemplet(ReportTypeInfo reportTypeInfo, ref byte[] byteTempletData)
        {
            if (reportTypeInfo == null || GlobalMethods.Misc.IsEmptyString(reportTypeInfo.ReportTypeID))
                return false;

            string szTempletPath = this.GetReportCachePath(reportTypeInfo.ReportTypeID);

            // ��������Ѿ���ģ�建��,��ô���ر��ص�
            DateTime dtModifyTime = this.GetReportModifyTime(reportTypeInfo.ReportTypeID);
            if (dtModifyTime.CompareTo(reportTypeInfo.ModifyTime) == 0)
            {
                if (GlobalMethods.IO.GetFileBytes(szTempletPath, ref byteTempletData))
                    return true;
            }

            // ��������Ŀ¼
            string szParentDir = GlobalMethods.IO.GetFilePath(szTempletPath);
            if (!GlobalMethods.IO.CreateDirectory(szParentDir))
            {
                LogManager.Instance.WriteLog("ReportCache.GetReportTemplet"
                    , new string[] { "templetInfo" }, new object[] { reportTypeInfo }, "����ģ�建��Ŀ¼����ʧ��!", null);
                return false;
            }

            // ���ر���ģ������
            short shRet = TempletService.Instance.GetReportTemplet(reportTypeInfo.ReportTypeID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                LogManager.Instance.WriteLog("ReportCache.GetReportTemplet"
                    , new string[] { "templetInfo" }, new object[] { reportTypeInfo }, "����ģ������ʧ��!", null);
                return false;
            }

            // д�û�ģ�屾��������Ϣ
            if (!this.CacheReportTemplet(reportTypeInfo, byteTempletData))
            {
                LogManager.Instance.WriteLog("ReportCache.GetReportTemplet"
                    , new string[] { "templetInfo" }, new object[] { reportTypeInfo }, "����ģ�建�浽����ʧ��!", null);
            }
            return true;
        }

        /// <summary>
        /// ��ָ�����û�ģ�����ݻ��浽����
        /// </summary>
        /// <param name="reportTypeInfo">ģ����Ϣ</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool CacheReportTemplet(ReportTypeInfo reportTypeInfo, byte[] byteTempletData)
        {
            if (reportTypeInfo == null || byteTempletData == null || byteTempletData.Length <= 0)
                return false;

            //����ģ���ļ����ݵ�����
            string szTempletPath = this.GetReportCachePath(reportTypeInfo.ReportTypeID);
            if (!GlobalMethods.IO.WriteFileBytes(szTempletPath, byteTempletData))
            {
                LogManager.Instance.WriteLog("ReportCache.CacheReportTemplet"
                    , new string[] { "reportTypeInfo", "szTempletPath" }, new object[] { reportTypeInfo, szTempletPath }
                    , "����ģ�����ݻ��浽����ʧ��!", null);
                return false;
            }

            //װ��ģ�屾�������ļ�
            if (!System.IO.File.Exists(this.m_szCacheIndexFile))
            {
                if (!GlobalMethods.Xml.CreateXmlFile(this.m_szCacheIndexFile, "Reports"))
                    return false;
            }
            XmlDocument xmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_szCacheIndexFile);
            if (xmlDoc == null)
            {
                return false;
            }

            //��ӻ���µ�ǰģ�������ڵ�
            string szXPath = string.Format("/Reports/Templet[@ID='{0}']", reportTypeInfo.ReportTypeID);
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
                LogManager.Instance.WriteLog("ReportCache.CacheReportTemplet"
                    , new string[] { "reportTypeInfo" }, new string[] { reportTypeInfo.ToString() }
                    , "����ģ�屾��������Ϣд��ʧ��!", ex);
                return false;
            }
            if (templetXmlNode == null)
            {
                LogManager.Instance.WriteLog("ReportCache.CacheReportTemplet"
                    , new string[] { "docTypeInfo", "szXPath" }, new string[] { reportTypeInfo.ToString(), szXPath }
                    , "����ģ�屾��������Ϣд��ʧ��!�޷������ڵ�!", null);
                return false;
            }

            //����ģ�屾�������ڵ������ֵ
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "ID", reportTypeInfo.ReportTypeID.ToString()))
                return false;
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "Name", reportTypeInfo.ReportTypeName))
                return false;
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "ModifyTime", reportTypeInfo.ModifyTime.ToString()))
                return false;
            if (!GlobalMethods.Xml.SaveXmlDocument(xmlDoc, this.m_szCacheIndexFile))
                return false;
            return true;
        }
    }
}
