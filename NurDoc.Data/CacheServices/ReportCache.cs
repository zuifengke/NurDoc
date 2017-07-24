// ***********************************************************
// 护理电子病历系统,报表类型信息缓存管理器.
// 主要缓存报表类型列表,以及报表模板文件,免得重复下载
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
        /// 获取ReportCache实例
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
        /// 模板本地缓存索引文件路径
        /// </summary>
        private string m_szCacheIndexFile = null;

        /// <summary>
        /// 存放报表类型的哈希表
        /// </summary>
        private Dictionary<string, ReportTypeInfo> m_htReportTypeTable = null;

        /// <summary>
        /// 按类别存放报表类型的哈希表
        /// </summary>
        private Dictionary<string, List<ReportTypeInfo>> m_htReportClassTable = null;

        /// <summary>
        /// 判断报表类型所属病区时使用的Hash表
        /// </summary>
        private Dictionary<string, WardReportType> m_htWardReportTable = null;

        /// <summary>
        /// 加载病历类型列表
        /// </summary>
        /// <param name="szApplyEnv">应用环境</param>
        /// <returns>报表类型信息列表</returns>
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
        /// 获取指定ID的报表类型信息
        /// </summary>
        /// <param name="szReportTypeID">报表类型代码</param>
        /// <returns>报表类型信息</returns>
        public ReportTypeInfo GetReportTypeInfo(string szReportTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szReportTypeID))
                return null;

            //如果是子病历类型
            if (this.m_htReportTypeTable == null)
                this.m_htReportTypeTable = new Dictionary<string, ReportTypeInfo>();
            if (this.m_htReportTypeTable.ContainsKey(szReportTypeID))
                return this.m_htReportTypeTable[szReportTypeID];

            //重新查询获取报表类型信息
            ReportTypeInfo reportTypeInfo = null;
            short shRet = TempletService.Instance.GetReportTypeInfo(szReportTypeID, ref reportTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK || reportTypeInfo == null)
                return null;
            this.m_htReportTypeTable.Add(szReportTypeID, reportTypeInfo);
            return reportTypeInfo;
        }

        /// <summary>
        /// 获取指定应用环境下的病区报表模板
        /// </summary>
        /// <param name="szApplyEnv">应用环境</param>
        /// <returns>报表模板信息</returns>
        public ReportTypeInfo GetWardReportType(string szApplyEnv)
        {
            List<ReportTypeInfo> lstReportTypeInfos = this.GetReportTypeList(szApplyEnv);
            if (lstReportTypeInfos == null || lstReportTypeInfos.Count <= 0)
                return null;

            //优先选取本病区可用的报表模板
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
        /// 获取指定应用环境指定病区下指定名称的报表模板
        /// </summary>
        /// <param name="szApplyEnv">应用环境</param>
        /// <param name="szReportName">报告类型名称</param>
        /// <returns>报表模板信息</returns>
        public ReportTypeInfo GetWardReportType(string szApplyEnv, string szReportName)
        {
            List<ReportTypeInfo> lstReportTypeInfos = this.GetReportTypeList(szApplyEnv);
            if (lstReportTypeInfos == null)
                return null;

            //优先选取本病区指定名称的报表模板
            //1当前病区，名字完全匹配
            //2当前病区，名字前部分匹配
            //3全院，名字完全匹配
            //4全院，名字前部分匹配
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
        /// 加载当前病区的病历类型列表
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

                //将病历类型的第一个所属病区信息加入到hash表
                //以便后续较方便的判断该病历类型是否设置了所属病区
                if (!this.m_htWardReportTable.ContainsKey(wardReportType.ReportTypeID))
                    this.m_htWardReportTable.Add(wardReportType.ReportTypeID, wardReportType);

                //同时将病历类型与病区的对应关系加入到hash表
                szWardReportType = string.Concat(wardReportType.ReportTypeID, "_", wardReportType.WardCode);
                if (!this.m_htWardReportTable.ContainsKey(szWardReportType))
                    this.m_htWardReportTable.Add(szWardReportType, wardReportType);
            }
            return true;
        }

        /// <summary>
        /// 获取指定的报表类型是否是当前用户病区的
        /// </summary>
        /// <param name="szDocTypeID">指定的报表类型</param>
        /// <returns>是否是当前病区病历</returns>
        public bool IsWardReportType(string szDocTypeID)
        {
            if (this.m_htWardReportTable == null)
            {
                if (!this.LoadWardReportTypeList())
                    return false;
            }
            if (this.m_htWardReportTable == null || SystemContext.Instance.LoginUser == null)
                return true;

            //如果病历类型未设置所属病区,那么认为是通用
            if (string.IsNullOrEmpty(szDocTypeID) || !this.m_htWardReportTable.ContainsKey(szDocTypeID))
                return true;

            //返回该病历类型所属病区中是否有当前用户病区
            string szWardReportType = string.Concat(szDocTypeID, "_", SystemContext.Instance.LoginUser.WardCode);
            return this.m_htWardReportTable.ContainsKey(szWardReportType);
        }

        /// <summary>
        /// 获取指定的报表类型是否是全院的
        /// </summary>
        /// <param name="szDocTypeID">指定的报表类型</param>
        /// <returns>是否是全院的病历</returns>
        public bool IshospitalReportType(string szDocTypeID)
        {
            if (this.m_htWardReportTable == null)
            {
                if (!this.LoadWardReportTypeList())
                    return false;
            }
            if (this.m_htWardReportTable == null || SystemContext.Instance.LoginUser == null)
                return true;

            //如果病历类型未设置所属病区,那么认为是通用
            if (string.IsNullOrEmpty(szDocTypeID) || !this.m_htWardReportTable.ContainsKey(szDocTypeID))
                return true;
            return false;
        }

        /// <summary>
        /// 获取报表模板本地缓存路径
        /// </summary>
        /// <param name="szTempletID">模板ID</param>
        /// <returns>模板本地缓存路径</returns>
        private string GetReportCachePath(string szTempletID)
        {
            return string.Format(@"{0}\Templets\Reports\{1}.hrdt", GlobalMethods.Misc.GetWorkingPath(), szTempletID);
        }

        /// <summary>
        /// 获取报表模板的修改时间
        /// </summary>
        /// <param name="szTempletID">报表模板ID(如果是报表模板)</param>
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
        /// 根据报表模板ID,获取报表模板数据
        /// </summary>
        /// <param name="szReportTypeID">报表模板ID</param>
        /// <param name="byteTempletData">返回的模板数据</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public bool GetReportTemplet(string szReportTypeID, ref byte[] byteTempletData)
        {
            ReportTypeInfo reportTypeInfo = this.GetReportTypeInfo(szReportTypeID);
            if (reportTypeInfo == null)
            {
                LogManager.Instance.WriteLog("FormCache.GetReportTemplet"
                     , new string[] { "szReportTypeID" }, new object[] { szReportTypeID }, "报表不存在!");
                return false;
            }
            return this.GetReportTemplet(reportTypeInfo, ref byteTempletData);
        }

        /// <summary>
        /// 根据报表模板信息,获取报表模板数据
        /// </summary>
        /// <param name="reportTypeInfo">报表模板信息</param>
        /// <param name="byteTempletData">报表模板数据</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public bool GetReportTemplet(ReportTypeInfo reportTypeInfo, ref byte[] byteTempletData)
        {
            if (reportTypeInfo == null || GlobalMethods.Misc.IsEmptyString(reportTypeInfo.ReportTypeID))
                return false;

            string szTempletPath = this.GetReportCachePath(reportTypeInfo.ReportTypeID);

            // 如果本地已经有模板缓存,那么返回本地的
            DateTime dtModifyTime = this.GetReportModifyTime(reportTypeInfo.ReportTypeID);
            if (dtModifyTime.CompareTo(reportTypeInfo.ModifyTime) == 0)
            {
                if (GlobalMethods.IO.GetFileBytes(szTempletPath, ref byteTempletData))
                    return true;
            }

            // 创建本地目录
            string szParentDir = GlobalMethods.IO.GetFilePath(szTempletPath);
            if (!GlobalMethods.IO.CreateDirectory(szParentDir))
            {
                LogManager.Instance.WriteLog("ReportCache.GetReportTemplet"
                    , new string[] { "templetInfo" }, new object[] { reportTypeInfo }, "报表模板缓存目录创建失败!", null);
                return false;
            }

            // 下载报表模板内容
            short shRet = TempletService.Instance.GetReportTemplet(reportTypeInfo.ReportTypeID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                LogManager.Instance.WriteLog("ReportCache.GetReportTemplet"
                    , new string[] { "templetInfo" }, new object[] { reportTypeInfo }, "报表模板下载失败!", null);
                return false;
            }

            // 写用户模板本地索引信息
            if (!this.CacheReportTemplet(reportTypeInfo, byteTempletData))
            {
                LogManager.Instance.WriteLog("ReportCache.GetReportTemplet"
                    , new string[] { "templetInfo" }, new object[] { reportTypeInfo }, "报表模板缓存到本地失败!", null);
            }
            return true;
        }

        /// <summary>
        /// 将指定的用户模板数据缓存到本地
        /// </summary>
        /// <param name="reportTypeInfo">模板信息</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool CacheReportTemplet(ReportTypeInfo reportTypeInfo, byte[] byteTempletData)
        {
            if (reportTypeInfo == null || byteTempletData == null || byteTempletData.Length <= 0)
                return false;

            //缓存模版文件内容到本地
            string szTempletPath = this.GetReportCachePath(reportTypeInfo.ReportTypeID);
            if (!GlobalMethods.IO.WriteFileBytes(szTempletPath, byteTempletData))
            {
                LogManager.Instance.WriteLog("ReportCache.CacheReportTemplet"
                    , new string[] { "reportTypeInfo", "szTempletPath" }, new object[] { reportTypeInfo, szTempletPath }
                    , "报表模板数据缓存到本地失败!", null);
                return false;
            }

            //装载模板本地索引文件
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

            //添加或更新当前模板索引节点
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
                    , "报表模板本地索引信息写入失败!", ex);
                return false;
            }
            if (templetXmlNode == null)
            {
                LogManager.Instance.WriteLog("ReportCache.CacheReportTemplet"
                    , new string[] { "docTypeInfo", "szXPath" }, new string[] { reportTypeInfo.ToString(), szXPath }
                    , "报表模板本地索引信息写入失败!无法创建节点!", null);
                return false;
            }

            //设置模板本地索引节点的属性值
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
