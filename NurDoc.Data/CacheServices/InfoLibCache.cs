// ***********************************************************
// 护理电子病历系统,护理信息库文档缓存管理器.
// 主要缓存护理信息文档列表,以及文件,免得重复下载
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
        /// 获取DocTypeCache实例
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
        /// 模板本地缓存索引文件路径
        /// </summary>
        private string m_szCacheIndexFile = null;

        /// <summary>
        /// 获取指定ID的文档类型信息
        /// </summary>
        /// <param name="szInfoID">文档类型代码</param>
        /// <returns>文档类型信息</returns>
        public InfoLibInfo GetInfoLibInfo(string szInfoID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szInfoID))
                return null;

            //重新查询获取文档类型信息
            List<InfoLibInfo> lstInfoLibInfo = null;
            short shRet = InfoLibService.Instance.GetInfoLibInfos(szInfoID, ref lstInfoLibInfo);
            if (shRet != SystemConst.ReturnValue.OK)
                return null;
            return lstInfoLibInfo[0];
        }

        /// <summary>
        /// 获取表单模板本地缓存路径
        /// </summary>
        /// <param name="szInfoID">信息ID号</param>
        /// <param name="szInfoType">信息文档类型</param>
        /// <returns>表单模板本地缓存路径</returns>
        private string GetInfoLibCachePath(string szInfoID, string szInfoType)
        {
            return string.Format(@"{0}\Templets\InfoLibs\{1}{2}", GlobalMethods.Misc.GetWorkingPath(), szInfoID, szInfoType);
        }

        /// <summary>
        /// 获取文档模板的修改时间
        /// </summary>
        /// <param name="szInfoID">表单类型ID(如果是表单模板)</param>
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
        /// 获取指定表单类型对应的表单模板数据
        /// </summary>
        /// <param name="infoLibInfo">文档信息</param>
        /// <param name="szTempletPath">文档路径</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short GetInfoLibTemplet(InfoLibInfo infoLibInfo, ref string szTempletPath)
        {
            if (infoLibInfo == null || GlobalMethods.Misc.IsEmptyString(infoLibInfo.InfoID))
                return SystemConst.ReturnValue.EXCEPTION;

            szTempletPath = this.GetInfoLibCachePath(infoLibInfo.InfoID, infoLibInfo.InfoType);

            // 如果本地已经有文档缓存,那么返回本地的
            DateTime dtModifyTime = this.GetInfoLibModifyTime(infoLibInfo.InfoID);
            if (dtModifyTime.CompareTo(infoLibInfo.ModifyTime) == 0)
            {
                return SystemConst.ReturnValue.OK;
            }
            // 创建本地目录
            string szParentDir = GlobalMethods.IO.GetFilePath(szTempletPath);
            if (!GlobalMethods.IO.CreateDirectory(szParentDir))
            {
                LogManager.Instance.WriteLog("InfoLibCache.GetInfoLibTemplet"
                    , new string[] { "infoLibInfo" }, new object[] { infoLibInfo }, "信息库文件缓存目录创建失败!", null);
                return SystemConst.ReturnValue.EXCEPTION;
            }
            // 写用户模板本地索引信息
            if (!this.CacheInfoLib(infoLibInfo))
            {
                LogManager.Instance.WriteLog("InfoLibCache.GetInfoLibTemplet"
                    , new string[] { "infoLibInfo" }, new object[] { infoLibInfo }, "表单模板缓存到本地失败!", null);
                return SystemConst.ReturnValue.EXCEPTION;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 将指定的系统模板数据缓存到本地
        /// </summary>
        /// <param name="infoLibInfo">文档类型信息</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool CacheInfoLib(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null)
                return false;

            //缓存系统模板内容到本地
            string szTempletPath = this.GetInfoLibCachePath(infoLibInfo.InfoID, infoLibInfo.InfoType);
            // 下载文档模板内容
            string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(infoLibInfo);
            short shRet = InfoLibService.Instance.DownloadInfoLibFile(szRemoteFile, szTempletPath);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                LogManager.Instance.WriteLog("InfoLibCache.CacheInfoLib"
                   , new string[] { "infoLibInfo" }, new object[] { infoLibInfo }
                    , "文件ftp下载失败!", null);
                return false;
            }
            //装载模板本地索引文件
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

            // 添加或更新模板本地索引节点
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
                    , new string[] { "docTypeInfo" }, new object[] { infoLibInfo }, "表单模板本地索引信息写入失败!", ex);
                return false;
            }
            if (templetXmlNode == null)
            {
                LogManager.Instance.WriteLog("FormCache.CacheInfoLib"
                    , new string[] { "docTypeInfo", "szXPath" }, new object[] { infoLibInfo, szXPath }
                    , "表单模板本地索引信息写入失败!无法创建节点!", null);
                return false;
            }

            //设置模板索引节点的属性值
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
