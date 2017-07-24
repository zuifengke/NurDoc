// ***********************************************************
// ������Ӳ���ϵͳ,��������Ϣ���������.
// ��Ҫ����������б�,�Լ���ģ���ļ�,����ظ�����
// Creator:YangMingkun  Date:2012-8-26
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
    public class FormCache
    {
        private static FormCache m_instance = null;

        /// <summary>
        /// ��ȡDocTypeCacheʵ��
        /// </summary>
        public static FormCache Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new FormCache();
                return m_instance;
            }
        }

        private FormCache()
        {
            this.m_szCacheIndexFile = GlobalMethods.Misc.GetWorkingPath()
                + @"\Templets\Forms.xml";
        }

        /// <summary>
        /// ģ�屾�ػ��������ļ�·��
        /// </summary>
        private string m_szCacheIndexFile = null;

        /// <summary>
        /// ��ű��ĵ����͵Ĺ�ϣ��
        /// </summary>
        private Dictionary<string, DocTypeInfo> m_htDocTypeTable = null;

        /// <summary>
        /// ������ű��ĵ����͵Ĺ�ϣ��
        /// </summary>
        private Dictionary<string, List<DocTypeInfo>> m_htDocClassTable = null;

        /// <summary>
        /// �ж��ĵ�������������ʱʹ�õ�Hash��
        /// </summary>
        private Dictionary<string, WardDocType> m_htWardDocTable = null;

        /// <summary>
        /// ���ز��������б�
        /// </summary>
        /// <param name="szApplyEnv">Ӧ�û���</param>
        /// <returns>�ĵ�������Ϣ�б�</returns>
        public List<DocTypeInfo> GetDocTypeList(string szApplyEnv)
        {
            if (GlobalMethods.Misc.IsEmptyString(szApplyEnv))
                return null;
            if (this.m_htDocClassTable != null
                && this.m_htDocClassTable.ContainsKey(szApplyEnv))
            {
                return this.m_htDocClassTable[szApplyEnv];
            }

            List<DocTypeInfo> lstDocTypeInfos = null;
            short shRet = DocTypeService.Instance.GetDocTypeInfos(szApplyEnv, ref lstDocTypeInfos);
            if (shRet != SystemConst.ReturnValue.OK)
                return null;

            if (lstDocTypeInfos == null || lstDocTypeInfos.Count <= 0)
                return new List<DocTypeInfo>();

            if (this.m_htDocClassTable == null)
                this.m_htDocClassTable = new Dictionary<string, List<DocTypeInfo>>();
            this.m_htDocClassTable.Add(szApplyEnv, lstDocTypeInfos);

            if (this.m_htDocTypeTable == null)
                this.m_htDocTypeTable = new Dictionary<string, DocTypeInfo>();

            for (int index = 0; index < lstDocTypeInfos.Count; index++)
            {
                DocTypeInfo docTypeInfo = lstDocTypeInfos[index];
                if (docTypeInfo == null || string.IsNullOrEmpty(docTypeInfo.DocTypeID))
                    continue;
                if (!this.m_htDocTypeTable.ContainsKey(docTypeInfo.DocTypeID))
                    this.m_htDocTypeTable.Add(docTypeInfo.DocTypeID, docTypeInfo);
            }
            return lstDocTypeInfos;
        }

        /// <summary>
        /// ��ȡָ��ID���ĵ�������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">�ĵ����ʹ���</param>
        /// <returns>�ĵ�������Ϣ</returns>
        public DocTypeInfo GetDocTypeInfo(string szDocTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
                return null;

            //������Ӳ�������
            if (this.m_htDocTypeTable == null)
                this.m_htDocTypeTable = new Dictionary<string, DocTypeInfo>();
            if (this.m_htDocTypeTable.ContainsKey(szDocTypeID))
                return this.m_htDocTypeTable[szDocTypeID];

            //���²�ѯ��ȡ�ĵ�������Ϣ
            DocTypeInfo docTypeInfo = null;
            short shRet = DocTypeService.Instance.GetDocTypeInfo(szDocTypeID, ref docTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK || docTypeInfo == null)
                return null;
            this.m_htDocTypeTable.Add(szDocTypeID, docTypeInfo);
            return docTypeInfo;
        }

        /// <summary>
        /// ��ȡָ��Ӧ�û����µĲ����ĵ�ģ��
        /// </summary>
        /// <param name="szApplyEnv">Ӧ�û���</param>
        /// <returns>�ĵ�ģ����Ϣ</returns>
        public DocTypeInfo GetWardDocType(string szApplyEnv)
        {
            List<DocTypeInfo> lstDocTypeInfos = this.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null || lstDocTypeInfos.Count <= 0)
                return null;

            //����ѡȡ���������õı�ģ��
            DocTypeInfo hospitalDocTypeInfo = null;
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (docTypeInfo.IsValid && !docTypeInfo.IsFolder)
                {
                    hospitalDocTypeInfo = docTypeInfo;
                    if (this.IsWardDocType(docTypeInfo.DocTypeID))
                        return docTypeInfo;
                }
            }
            return hospitalDocTypeInfo;
        }

        /// <summary>
        /// ��ȡָ��Ӧ�û���ָ��������ָ�����Ƶı�ģ��
        /// </summary>
        /// <param name="szApplyEnv">Ӧ�û���</param>
        /// <param name="szDocTypeName">����������</param>
        /// <returns>��ģ����Ϣ</returns>
        public DocTypeInfo GetWardDocType(string szApplyEnv, string szDocTypeName)
        {
            List<DocTypeInfo> lstDocTypeInfos = this.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null || lstDocTypeInfos.Count <= 0)
                return null;

            //����ѡȡ������ָ�����Ƶı�ģ��
            DocTypeInfo docTypeInfo1 = null;
            DocTypeInfo docTypeInfo2 = null;
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (docTypeInfo.IsValid && !docTypeInfo.IsFolder)
                {
                    if (docTypeInfo.DocTypeName.StartsWith(szDocTypeName))
                        docTypeInfo1 = docTypeInfo;
                    if (this.IsWardDocType(docTypeInfo.DocTypeID))
                        docTypeInfo2 = docTypeInfo;
                }
            }
            if (docTypeInfo2 != null && docTypeInfo2.DocTypeName.StartsWith(szDocTypeName))
                return docTypeInfo2;
            return docTypeInfo1;
        }

        /// <summary>
        /// ���ص�ǰ�����Ĳ��������б�
        /// </summary>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool LoadWardDocTypeList()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            List<WardDocType> lstWardDocTypes = null;
            short shRet = DocTypeService.Instance.GetWardDocTypeList(string.Empty, ref lstWardDocTypes);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;

            if (this.m_htWardDocTable == null)
                this.m_htWardDocTable = new Dictionary<string, WardDocType>();
            this.m_htWardDocTable.Clear();

            if (lstWardDocTypes == null || lstWardDocTypes.Count <= 0)
                return true;

            string szWardDocType = null;
            for (int index = 0; index < lstWardDocTypes.Count; index++)
            {
                WardDocType wardDocType = lstWardDocTypes[index];
                if (wardDocType == null || string.IsNullOrEmpty(wardDocType.DocTypeID))
                    continue;

                //���������͵ĵ�һ������������Ϣ���뵽hash��
                //�Ա�����Ϸ�����жϸò��������Ƿ���������������
                if (!this.m_htWardDocTable.ContainsKey(wardDocType.DocTypeID))
                    this.m_htWardDocTable.Add(wardDocType.DocTypeID, wardDocType);

                //ͬʱ�����������벡���Ķ�Ӧ��ϵ���뵽hash��
                szWardDocType = string.Concat(wardDocType.DocTypeID, "_", wardDocType.WardCode);
                if (!this.m_htWardDocTable.ContainsKey(szWardDocType))
                    this.m_htWardDocTable.Add(szWardDocType, wardDocType);
            }
            return true;
        }

        /// <summary>
        /// ��ȡָ�����ĵ������Ƿ��ǵ�ǰ�û�������
        /// </summary>
        /// <param name="szDocTypeID">ָ�����ĵ�����</param>
        /// <returns>�Ƿ��ǵ�ǰ��������</returns>
        public bool IsWardDocType(string szDocTypeID)
        {
            if (this.m_htWardDocTable == null)
            {
                if (!this.LoadWardDocTypeList())
                    return false;
            }
            if (this.m_htWardDocTable == null || SystemContext.Instance.LoginUser == null)
                return true;

            //�����������δ������������,��ô��Ϊ��ͨ��
            if (string.IsNullOrEmpty(szDocTypeID) || !this.m_htWardDocTable.ContainsKey(szDocTypeID))
                return true;

            //���ظò������������������Ƿ��е�ǰ�û�����
            string szWardDocType = string.Concat(szDocTypeID, "_", SystemContext.Instance.LoginUser.WardCode);
            if (this.m_htWardDocTable.ContainsKey(szWardDocType))
                return true;

            //���ظò������������û������Ƿ��и��û����ڵ�С��
            return RightController.Instance.CanOpenNurDoc(m_htWardDocTable, szDocTypeID);
        }

        /// <summary>
        /// ��ȡָ�����ĵ����Ͷ�Ӧ�ı��Ƿ�������Ϊ�����ӡ
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <returns>�Ƿ������ӡ</returns>
        public bool IsFormPrintable(string szDocTypeID)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
                return false;
            if (docTypeInfo.PrintMode == FormPrintMode.Form || docTypeInfo.PrintMode == FormPrintMode.FormAndList)
                return true;
            return false;
        }

        /// <summary>
        /// ��ȡָ�����ĵ����Ͷ�Ӧ����д���б��Ƿ�������Ϊ�����ӡ
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <returns>�Ƿ������ӡ</returns>
        public bool IsFormListPrintable(string szDocTypeID)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
                return false;
            if (docTypeInfo.PrintMode == FormPrintMode.List || docTypeInfo.PrintMode == FormPrintMode.FormAndList)
                return true;
            return false;
        }

        /// <summary>
        /// ��ȡָ�����ĵ����Ͷ�Ӧ�����������Ƿ�����Ϊ���ظ�
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <returns>true or false</returns>
        public bool IsFormRepeatable(string szDocTypeID)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
                return false;
            return docTypeInfo.IsRepeated;
        }

        /// <summary>
        /// ��ȡ��ģ�屾�ػ���·��
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <returns>ģ�屾�ػ���·��</returns>
        private string GetFormCachePath(string szDocTypeID)
        {
            return string.Format(@"{0}\Templets\Forms\{1}.hndt", GlobalMethods.Misc.GetWorkingPath(), szDocTypeID);
        }

        /// <summary>
        /// ��ȡ�ĵ�ģ����޸�ʱ��
        /// </summary>
        /// <param name="szDocTypeID">������ID(����Ǳ�ģ��)</param>
        /// <returns>DateTime</returns>
        public DateTime GetFormModifyTime(string szDocTypeID)
        {
            if (!System.IO.File.Exists(this.m_szCacheIndexFile))
                return DateTime.Now;

            XmlDocument xmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_szCacheIndexFile);
            if (xmlDoc == null)
                return DateTime.Now;

            string szXPath = null;
            if (!GlobalMethods.Misc.IsEmptyString(szDocTypeID))
                szXPath = string.Format("/Forms/Templet[@ID='{0}']", szDocTypeID.Trim());
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
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="byteTempletData">���ص�ģ������</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public bool GetFormTemplet(string szDocTypeID, ref byte[] byteTempletData)
        {
            DocTypeInfo docTypeInfo = this.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                LogManager.Instance.WriteLog("FormCache.GetFormTemplet"
                     , new string[] { "szDocTypeID" }, new object[] { szDocTypeID }, "��������!");
                return false;
            }
            return this.GetFormTemplet(docTypeInfo, ref byteTempletData);
        }

        /// <summary>
        /// ��ȡָ�������Ͷ�Ӧ�ı�ģ������
        /// </summary>
        /// <param name="docTypeInfo">�ĵ�������Ϣ</param>
        /// <param name="byteTempletData">���ص�ģ������</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public bool GetFormTemplet(DocTypeInfo docTypeInfo, ref byte[] byteTempletData)
        {
            if (docTypeInfo == null || GlobalMethods.Misc.IsEmptyString(docTypeInfo.DocTypeID))
                return false;

            string szTempletPath = this.GetFormCachePath(docTypeInfo.DocTypeID);

            // ��������Ѿ���ģ�建��,��ô���ر��ص�
            DateTime dtModifyTime = this.GetFormModifyTime(docTypeInfo.DocTypeID);
            if (dtModifyTime.CompareTo(docTypeInfo.ModifyTime) == 0)
            {
                if (GlobalMethods.IO.GetFileBytes(szTempletPath, ref byteTempletData))
                    return true;
            }

            // ��������Ŀ¼
            string szParentDir = GlobalMethods.IO.GetFilePath(szTempletPath);
            if (!GlobalMethods.IO.CreateDirectory(szParentDir))
            {
                LogManager.Instance.WriteLog("FormCache.GetFormTemplet"
                    , new string[] { "docTypeInfo" }, new object[] { docTypeInfo }, "��ģ�建��Ŀ¼����ʧ��!", null);
                return false;
            }

            // �����ĵ�ģ������
            short shRet = TempletService.Instance.GetFormTemplet(docTypeInfo.DocTypeID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                LogManager.Instance.WriteLog("FormCache.GetFormTemplet"
                    , new string[] { "docTypeInfo" }, new object[] { docTypeInfo }, "��ģ������ʧ��!", null);
                return false;
            }

            // д�û�ģ�屾��������Ϣ
            if (!this.CacheFormTemplet(docTypeInfo, byteTempletData))
            {
                LogManager.Instance.WriteLog("FormCache.GetFormTemplet"
                    , new string[] { "docTypeInfo" }, new object[] { docTypeInfo }, "��ģ�建�浽����ʧ��!", null);
            }
            return true;
        }

        /// <summary>
        /// ��ָ����ϵͳģ�����ݻ��浽����
        /// </summary>
        /// <param name="docTypeInfo">�ĵ�������Ϣ</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool CacheFormTemplet(DocTypeInfo docTypeInfo, byte[] byteTempletData)
        {
            if (docTypeInfo == null || byteTempletData == null || byteTempletData.Length <= 0)
                return false;

            //����ϵͳģ�����ݵ�����
            string szTempletPath = this.GetFormCachePath(docTypeInfo.DocTypeID);
            if (!GlobalMethods.IO.WriteFileBytes(szTempletPath, byteTempletData))
            {
                LogManager.Instance.WriteLog("FormCache.CacheFormTemplet"
                    , new string[] { "docTypeInfo", "szTempletPath" }, new object[] { docTypeInfo, szTempletPath }
                    , "��ģ�����ݻ��浽����ʧ��!", null);
                return false;
            }

            //װ��ģ�屾�������ļ�
            if (!System.IO.File.Exists(this.m_szCacheIndexFile))
            {
                if (!GlobalMethods.Xml.CreateXmlFile(this.m_szCacheIndexFile, "Forms"))
                    return false;
            }
            XmlDocument xmlDoc = GlobalMethods.Xml.GetXmlDocument(this.m_szCacheIndexFile);
            if (xmlDoc == null)
            {
                return false;
            }

            // ��ӻ����ģ�屾�������ڵ�
            string szXPath = string.Format("/Forms/Templet[@ID='{0}']", docTypeInfo.DocTypeID);
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
                LogManager.Instance.WriteLog("FormCache.CacheFormTemplet"
                    , new string[] { "docTypeInfo" }, new object[] { docTypeInfo }, "��ģ�屾��������Ϣд��ʧ��!", ex);
                return false;
            }
            if (templetXmlNode == null)
            {
                LogManager.Instance.WriteLog("FormCache.CacheFormTemplet"
                    , new string[] { "docTypeInfo", "szXPath" }, new object[] { docTypeInfo, szXPath }
                    , "��ģ�屾��������Ϣд��ʧ��!�޷������ڵ�!", null);
                return false;
            }

            //����ģ�������ڵ������ֵ
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "ID", docTypeInfo.DocTypeID))
                return false;
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "Name", docTypeInfo.DocTypeName))
                return false;
            if (!GlobalMethods.Xml.SetXmlAttrValue(templetXmlNode, "ModifyTime", docTypeInfo.ModifyTime.ToString()))
                return false;
            if (!GlobalMethods.Xml.SaveXmlDocument(xmlDoc, this.m_szCacheIndexFile))
                return false;
            return true;
        }
        #region "��QC�й�"
        /// <summary>
        /// ��ű��ĵ����͵Ĺ�ϣ��
        /// </summary>
        private Dictionary<string, QCTypeInfo> m_htQCTypeTable = null;

        /// <summary>
        /// ������ű��ĵ����͵Ĺ�ϣ��
        /// </summary>
        private Dictionary<string, List<QCTypeInfo>> m_htQCClassTable = null;
        
        /// <summary>
        /// ���������밲ȫ�����¼�����б�
        /// </summary>
        /// <param name="szApplyEnv">Ӧ�û���</param>
        /// <returns>�ĵ�������Ϣ�б�</returns>
        public List<QCTypeInfo> GetQCTypeList(string szApplyEnv)
        {
            if (GlobalMethods.Misc.IsEmptyString(szApplyEnv))
                return null;
            if (this.m_htQCClassTable != null
                && this.m_htQCClassTable.ContainsKey(szApplyEnv))
            {
                return this.m_htQCClassTable[szApplyEnv];
            }

            List<QCTypeInfo> lstQCTypeInfos = null;
            short shRet = QCTypeService.Instance.GetQCTypeInfos(szApplyEnv, ref lstQCTypeInfos);
            if (shRet != SystemConst.ReturnValue.OK)
                return null;

            if (lstQCTypeInfos == null || lstQCTypeInfos.Count <= 0)
                return new List<QCTypeInfo>();

            if (this.m_htQCClassTable == null)
                this.m_htQCClassTable = new Dictionary<string, List<QCTypeInfo>>();
            this.m_htQCClassTable.Add(szApplyEnv, lstQCTypeInfos);

            if (this.m_htQCTypeTable == null)
                this.m_htQCTypeTable = new Dictionary<string, QCTypeInfo>();

            for (int index = 0; index < lstQCTypeInfos.Count; index++)
            {
                QCTypeInfo qcTypeInfo = lstQCTypeInfos[index];
                if (qcTypeInfo == null || string.IsNullOrEmpty(qcTypeInfo.QCTypeID))
                    continue;
                if (!this.m_htQCTypeTable.ContainsKey(qcTypeInfo.QCTypeID))
                    this.m_htQCTypeTable.Add(qcTypeInfo.QCTypeID, qcTypeInfo);
            }
            return lstQCTypeInfos;
        }

        /// <summary>
        /// ��ȡָ��ID���ĵ�������Ϣ
        /// </summary>
        /// <param name="szQCTypeID">�ĵ����ʹ���</param>
        /// <returns>�ĵ�������Ϣ</returns>
        public QCTypeInfo GetQCTypeInfo(string szQCTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQCTypeID))
                return null;

            //������Ӳ�������
            if (this.m_htQCTypeTable == null)
                this.m_htQCTypeTable = new Dictionary<string, QCTypeInfo>();
            if (this.m_htQCTypeTable.ContainsKey(szQCTypeID))
                return this.m_htQCTypeTable[szQCTypeID];

            //���²�ѯ��ȡ�ĵ�������Ϣ
            QCTypeInfo qcTypeInfo = null;
            short shRet = QCTypeService.Instance.GetQCTypeInfo(szQCTypeID, ref qcTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK || qcTypeInfo == null)
                return null;
            this.m_htQCTypeTable.Add(szQCTypeID, qcTypeInfo);
            return qcTypeInfo;
        }
        #endregion
    }
}
