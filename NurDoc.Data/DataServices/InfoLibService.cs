using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class InfoLibService
    {
         private static InfoLibService m_instance = null;

        /// <summary>
        /// 获取护理文书模板服务实例
        /// </summary>
        public static InfoLibService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new InfoLibService();
                return m_instance;
            }
        }

        private InfoLibService()
        {
        }
        #region"护理信息库读写接口"
       
        /// <summary>
        /// 获取指定的护理信息文档信息
        /// </summary>
        /// <param name="szInfoID">信息ID</param>
        /// <param name="lstInfoLibInfo">信息数据</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetInfoLibInfos(string szInfoID, ref List<InfoLibInfo>lstInfoLibInfo)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetInfoLibInfos(szInfoID, ref lstInfoLibInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定的护理信息内容
        /// </summary>
        /// <param name="szInfoID">信息ID</param>
        /// <param name="byteTempletData">信息数据</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetInfoLib(string szInfoID, ref byte[] byteTempletData)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetInfoLib(szInfoID, ref byteTempletData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 保存护理信息到服务器
        /// </summary>
        /// <param name="infoLibInfo">护理信息文档</param>
        /// <param name="byteInfoData">系统缺省模板</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short SaveInfoLib(InfoLibInfo infoLibInfo, byte[] byteInfoData)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.SaveInfoLib(infoLibInfo, byteInfoData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 保存护理信息到服务器
        /// </summary>
        /// <param name="infoLibInfo">护理信息文档</param>
        /// <param name="szFilePath">文档路径</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short SaveInfoLibToFTP(InfoLibInfo infoLibInfo, string szFilePath)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.SaveInfoLibToFTP(infoLibInfo, szFilePath);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 更新结构化文本模板内容到服务器
        /// </summary>
        /// <param name="infoLibInfo">护理信息内容</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short UpdateInfoLib(InfoLibInfo infoLibInfo, byte[] byteTempletData)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.UpdateInfoLib(infoLibInfo, byteTempletData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 更新结构化文本模板内容到服务器
        /// </summary>
        /// <param name="infoLibInfo">护理信息内容</param>
        /// <param name="szFilePath">文档路径</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short UpdateInfoLibToFtp(InfoLibInfo infoLibInfo,string szFilePath)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.UpdateInfoLibToFtp(infoLibInfo, szFilePath);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 修改指定的结构化文本模板共享等级
        /// </summary>
        /// <param name="szInfoID">结构化文本模板ID</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyInfoLibShareLevel(string szInfoID, string szShareLevel)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.ModifyInfoLibShareLevel(szInfoID, szShareLevel);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 修改指定的护理信息共享等级（批量）
        /// </summary>
        /// <param name="lstInfoLibID">护理信息ID列表</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyInfoLibShareLevel(List<string> lstInfoLibID, string szShareLevel)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.ModifyInfoLibShareLevel(lstInfoLibID, szShareLevel);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 修改指定的护理信息父目录
        /// </summary>
        /// <param name="szInfoID">结构化文本模板ID</param>
        /// <param name="szParentID">模板新的父目录ID</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyInfoLibParentID(string szInfoID, string szParentID)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.ModifyInfoLibParentID(szInfoID, szParentID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }
        
        /// <summary>
        /// 修改指定的护理信息名称
        /// </summary>
        /// <param name="szInfoID">信息ID</param>
        /// <param name="szInfoName">模板新的名称</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyInfoLibName(string szInfoID, string szInfoName)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.ModifyInfoLibName(szInfoID, szInfoName);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 删除指定的护理信息及FTP文件
        /// </summary>
        /// <param name="infoLibInfo">文档信息类</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short DeleteInfoLibToFTP(InfoLibInfo infoLibInfo)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.DeleteInfoLibToFTP(infoLibInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 删除指定的一系列护理信息内容
        /// </summary>
        /// <param name="lstInfoLibID">要删除的文本模板ID列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short DeleteInfoLib(List<string> lstInfoLibID)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.DeleteInfoLib(lstInfoLibID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取文档信息类信息
        /// </summary>
        /// <param name="lstInfoLibInfos">文档信息列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetHospitalInfoLibInfos(ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetHospitalInfoLibInfos(ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取全院文本模板列表
        /// </summary>
        /// <param name="szParentID">病人ID</param>
        /// <param name="lstInfoLibInfos">文本模板列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetHospitalChildInfoLibInfos(string szParentID,ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetHospitalChildInfoLibInfos(szParentID, ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定用户ID和病人ID的护理信息列表
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetPersonalInfoLibInfos(string szUserID, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetPersonalInfoLibInfos(szUserID, ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 根据标题检索文档
        /// </summary>
        /// <param name="szInfoName">关键字</param>
        /// <param name="lstInfoLibInfos">护理信息库类列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInfoLibInfosByName(string szInfoName, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetInfoLibInfosByName(szInfoName, ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定用户ID的某项菜单下的护理信息文件夹列表
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetPersonalChildInfoLibInfos(string szUserID,string szPatientID, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetPersonalChildInfoLibInfos(szUserID,szPatientID, ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取所有护理信息列表
        /// </summary>
        /// <param name="lstInfoLibInfos">护理信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAllLibInfosFolder(ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetAllLibInfosFolder(ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定病区的护理信息列表
        /// </summary>
        /// <param name="szDeptCode">病区代码</param>
        /// <param name="bOnlyDeptShare">是否仅返回标记为病区共享的护理信息/param>
        /// <param name="lstInfoLibInfos">文本模板列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetDeptInfoLibInfos(string szDeptCode, bool bOnlyDeptShare, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetDeptInfoLibInfos(szDeptCode, bOnlyDeptShare, ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定病区的护理信息列表
        /// </summary>
        /// <param name="szDeptCode">病区代码</param>
        /// <param name="bOnlyDeptShare">是否仅返回标记为病区共享的护理信息/param>
        /// <param name="lstInfoLibInfos">文本模板列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetDeptChildInfoLibInfos(string szPatientID,string szDeptCode, bool bOnlyDeptShare, ref List<InfoLibInfo> lstInfoLibInfos)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.GetDeptChildInfoLibInfos(szPatientID,szDeptCode, bOnlyDeptShare, ref lstInfoLibInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 根据文件名字从FTP中下载文件
        /// </summary>
        /// <param name="strRemoteFileName">服务器上的文件名称</param>
        /// <param name="strLocalFileName">本地的文件名称</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DownloadInfoLibFile(string strRemoteFileName, string strLocalFileName)
        {
            short shRet = SystemContext.Instance.InfoLibAccess.DownloadInfoLibFile(strRemoteFileName, strLocalFileName);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }
        #endregion
    }
}
