// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之表单模板数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public class WordTempService : DBAccessBase
    {
        private static WordTempService m_instance = null;

        /// <summary>
        /// 获取护理文书模板服务实例
        /// </summary>
        public static WordTempService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new WordTempService();
                return m_instance;
            }
        }

        private WordTempService()
        {
        }

        #region"表单模板读写接口"

        /// <summary>
        /// 获取系统自带的指定文档类型的文档模板内容
        /// </summary>
        /// <param name="szDocTypeID">文档类型代码</param>
        /// <param name="byteTempletData">模板二进制内容</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetFormWordTemp(string szDocTypeID, ref byte[] byteTempletData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                return RestHandler.Instance.Get<byte[]>("WordTempAccess/GetFormWordTemp1",ref byteTempletData);
            }
            else
            {
                if (SystemContext.Instance.WordTempAccess == null)
                    return SystemConst.ReturnValue.FAILED;

                return SystemContext.Instance.WordTempAccess.GetFormWordTemp(szDocTypeID, ref byteTempletData);
            }
        }

        /// <summary>
        /// 获取所有模版
        /// </summary>
        /// <param name="lstDocTypeData">模版列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetFormTemplet(ref List<DocTypeData> lstDocTypeData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                return RestHandler.Instance.Get<DocTypeData>("WordTempAccess/GetFormWordTemp2", ref lstDocTypeData);
            }
            else
            {
                if (SystemContext.Instance.WordTempAccess == null)
                    return SystemConst.ReturnValue.FAILED;

                return SystemContext.Instance.WordTempAccess.GetFormWordTemp(ref lstDocTypeData);
            }
        }

        /// <summary>
        /// 保存系统缺省模板到服务器
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveFormTemplet(string szDocTypeID, byte[] byteTempletData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                RestHandler.Instance.AddParameter("byteTempletData", byteTempletData);
                return RestHandler.Instance.Post("WordTempAccess/SaveFormTemplet");
            }
            else
            {
                if (SystemContext.Instance.WordTempAccess == null)
                    return SystemConst.ReturnValue.FAILED;

                return SystemContext.Instance.WordTempAccess.SaveFormTemplet(szDocTypeID, byteTempletData);
            }
        }
        #endregion

        #region"报表模板读写接口"
        /// <summary>
        /// 获取报告文档模板内容
        /// </summary>
        /// <param name="szTempletID">报表模板ID</param>
        /// <param name="byteTempletData">报表模板数据</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetReportTemplet(string szTempletID, ref byte[] byteTempletData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szTempletID", szTempletID);
                return RestHandler.Instance.Get<byte[]>("WordTempAccess/GetReportTemplet1", ref byteTempletData);
            }
            else
            {
                if (SystemContext.Instance.TempletAccess == null)
                    return SystemConst.ReturnValue.FAILED;

                return SystemContext.Instance.TempletAccess.GetReportTemplet(szTempletID, ref byteTempletData);
            }
        }

        /// <summary>
        /// 获取所有报表文件
        /// </summary>
        /// <param name="lstReportTypeData">报表模板数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetReportTemplet(ref List<ReportTypeData> lstReportTypeData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                return RestHandler.Instance.Get<ReportTypeData>("WordTempAccess/GetReportTemplet2", ref lstReportTypeData);
            }
            else
            {
                if (SystemContext.Instance.TempletAccess == null)
                    return SystemConst.ReturnValue.FAILED;

                return SystemContext.Instance.TempletAccess.GetReportTemplet(ref lstReportTypeData);
            }
        }

        /// <summary>
        /// 获取指定ID号的报表类型信息
        /// </summary>
        /// <param name="szReportTypeID">报表ID</param>
        /// <param name="reportTypeInfo">返回的报表类型信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetWordTempInfos(string szReportTypeID, ref ReportTypeInfo reportTypeInfo)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szReportTypeID", szReportTypeID);
                shRet = RestHandler.Instance.Get<ReportTypeInfo>("WordTempAccess/GetWordTempInfos", ref reportTypeInfo);
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.GetReportTypeInfo(szReportTypeID, ref reportTypeInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取所有报表类型信息
        /// </summary>
        /// <param name="szApplyEnv">报表应用环境</param>
        /// <param name="lstReportTypeInfos">返回的报表类型信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetReportTypeInfos(string szApplyEnv, ref List<ReportTypeInfo> lstReportTypeInfos)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szApplyEnv", szApplyEnv);
                shRet = RestHandler.Instance.Get<ReportTypeInfo>("WordTempAccess/GetReportTypeInfos", ref lstReportTypeInfos);
            }
            else
            {
                shRet = SystemContext.Instance.TempletAccess.GetReportTypeInfos(szApplyEnv, ref lstReportTypeInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 保存报表模板到服务器
        /// </summary>
        /// <param name="szDocTypeID">模板ID</param>
        /// <param name="byteTempletData">模板数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveReportTemplet(string szDocTypeID, byte[] byteTempletData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                RestHandler.Instance.AddParameter("byteTempletData", byteTempletData);
                return RestHandler.Instance.Post("WordTempAccess/SaveReportTemplet");
            }
            else
            {
                if (SystemContext.Instance.TempletAccess == null)
                    return SystemConst.ReturnValue.FAILED;

                return SystemContext.Instance.TempletAccess.SaveReportTemplet(szDocTypeID, byteTempletData);
            }
        }

        /// <summary>
        /// 保存一条新的报表类型配置信息
        /// </summary>
        /// <param name="reportTypeInfo">报表类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveReportTypeInfo(ReportTypeInfo reportTypeInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(reportTypeInfo);
                return RestHandler.Instance.Post("WordTempAccess/SaveReportTypeInfo");
            }
            else
            {
                return SystemContext.Instance.TempletAccess.SaveReportTypeInfo(reportTypeInfo);
            }
        }

        /// <summary>
        /// 修改一条病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型代码</param>
        /// <param name="reportTypeInfo">病历类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short ModifyReportTypeInfo(string szDocTypeID, ReportTypeInfo reportTypeInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                RestHandler.Instance.AddParameter(reportTypeInfo);
                return RestHandler.Instance.Post("WordTempAccess/ModifyReportTypeInfo");
            }
            else
            {
                return SystemContext.Instance.TempletAccess.ModifyReportTypeInfo(szDocTypeID, reportTypeInfo);
            }
        }

        /// <summary>
        /// 删除多条报表类型配置信息
        /// </summary>
        /// <param name="lstDocTypeID">报表类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteReportTypeInfos(List<string> lstDocTypeID)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(lstDocTypeID);
                return RestHandler.Instance.Post("WordTempAccess/DeleteReportTypeInfos");
            }
            else
            {
                return SystemContext.Instance.TempletAccess.DeleteReportTypeInfos(lstDocTypeID);
            }
        }

        #endregion

        #region "病区报告类型读写接口"
        /// <summary>
        /// 获取病区病历类型信息列表
        /// </summary>
        /// <param name="szDeptCode">病区代码</param>
        /// <param name="lstWardReportTypes">返回的病区病历类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetWardReportTypeList(string szDeptCode, ref List<WardReportType> lstWardReportTypes)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDeptCode", szDeptCode);
                return RestHandler.Instance.Get<WardReportType>("WordTempAccess/GetWardReportTypeList", ref lstWardReportTypes);
            }
            else
            {
                return SystemContext.Instance.TempletAccess.GetWardReportTypeList(szDeptCode, ref lstWardReportTypes);
            }
        }

        /// <summary>
        /// 获取病区病历类型信息列表
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="lstWardReportTypes">返回的病区病历类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetReportTypeDeptList(string szDocTypeID, ref List<WardReportType> lstWardReportTypes)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                return RestHandler.Instance.Get<WardReportType>("WordTempAccess/GetReportTypeDeptList", ref lstWardReportTypes);
            }
            else
            {
                return SystemContext.Instance.TempletAccess.GetReportTypeDeptList(szDocTypeID, ref lstWardReportTypes);
            }
        }

        /// <summary>
        /// 保存病区报表类型信息列表
        /// </summary>
        /// <param name="szDocTypeID">报表类型ID号</param>
        /// <param name="lstWardReportTypes">病区报表类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveWardReportTypes(string szDocTypeID, List<WardReportType> lstWardReportTypes)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                RestHandler.Instance.AddParameter(lstWardReportTypes);
                return RestHandler.Instance.Post("WordTempAccess/SaveWardReportTypes");
            }
            else
            {
                return SystemContext.Instance.TempletAccess.SaveWardReportTypes(szDocTypeID, lstWardReportTypes);
            }
        }
        #endregion

        #region"护理文本模板读写接口"
        /// <summary>
        /// 获取指定的结构化文本模板内容
        /// </summary>
        /// <param name="szTempletID">文本模板ID</param>
        /// <param name="byteTempletData">模板数据</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetWordTemp(string szTempletID, ref byte[] byteTempletData)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szTempletID", szTempletID);
                shRet = RestHandler.Instance.Get<byte[]>("WordTempAccess/GetWordTemp", ref byteTempletData);
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.GetWordTemp(szTempletID, ref byteTempletData);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 保存结构化文本模板到服务器
        /// </summary>
        /// <param name="WordTempInfo">文本模板信息</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short SaveWordTemp(WordTempInfo WordTempInfo, byte[] byteTempletData)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(WordTempInfo, byteTempletData);
                shRet = RestHandler.Instance.Post("WordTempAccess/SaveWordTemp");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.SaveWordTemp(WordTempInfo, byteTempletData);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 更新结构化文本模板内容到服务器
        /// </summary>
        /// <param name="m_WordTempInfo">结构化模板信息</param>
        /// <param name="byteTempletData">系统缺省模板</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short UpdateWordTemp(WordTempInfo m_WordTempInfo, byte[] byteTempletData)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(m_WordTempInfo, byteTempletData);
                shRet = RestHandler.Instance.Post("WordTempAccess/UpdateWordTemp");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.UpdateWordTemp(m_WordTempInfo, byteTempletData);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 修改指定的结构化文本模板共享等级
        /// </summary>
        /// <param name="szTempletID">结构化文本模板ID</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyWordTempShareLevel(string szTempletID, string szShareLevel)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szTempletID", szTempletID);
                RestHandler.Instance.AddParameter("szShareLevel", szShareLevel);
                shRet = RestHandler.Instance.Put("WordTempAccess/ModifyWordTempShareLevel1");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.ModifyWordTempShareLevel(szTempletID, szShareLevel);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 修改指定的结构化文本模板共享等级（批量）
        /// </summary>
        /// <param name="lstTempletID">结构化文本模板ID列表</param>
        /// <param name="szShareLevel">模板新的共享级别</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyWordTempShareLevel(List<string> lstTempletID, string szShareLevel)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(lstTempletID);
                RestHandler.Instance.AddParameter("szShareLevel", szShareLevel);
                shRet = RestHandler.Instance.Post("WordTempAccess/ModifyWordTempShareLevel2");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.ModifyWordTempShareLevel(lstTempletID, szShareLevel);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 修改指定的结构化文本模板父目录
        /// </summary>
        /// <param name="szTempletID">结构化文本模板ID</param>
        /// <param name="szParentID">模板新的父目录ID</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyTextTempletParentID(string szTempletID, string szParentID)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szTempletID", szTempletID);
                RestHandler.Instance.AddParameter("szParentID", szParentID);
                shRet = RestHandler.Instance.Put("WordTempAccess/ModifyTextTempletParentID");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.ModifyTextTempletParentID(szTempletID, szParentID);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 修改指定的结构化文本模板名称
        /// </summary>
        /// <param name="szTempletID">结构化文本模板ID</param>
        /// <param name="szTempletName">模板新的名称</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short ModifyWordTempName(string szTempletID, string szTempletName)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szTempletID", szTempletID);
                RestHandler.Instance.AddParameter("szTempletName", szTempletName);
                shRet = RestHandler.Instance.Put("WordTempAccess/ModifyWordTempName");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.ModifyWordTempName(szTempletID, szTempletName);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 删除指定的结构化文本模板
        /// </summary>
        /// <param name="szTempletID">结构化文本模板ID</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short DeleteWordTemp(string szTempletID)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szTempletID", szTempletID);
                shRet = RestHandler.Instance.Put("WordTempAccess/DeleteWordTemp1");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.DeleteWordTemp(szTempletID);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 删除指定的一系列结构化文本模板
        /// </summary>
        /// <param name="lstTempletID">要删除的文本模板ID列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short DeleteWordTemp(List<string> lstTempletID)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(lstTempletID);
                shRet = RestHandler.Instance.Post("WordTempAccess/DeleteWordTemp2");
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.DeleteWordTemp(lstTempletID);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取全院文本模板列表
        /// </summary>
        /// <param name="lstTempletInfos">文本模板列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetHospitalWordTempInfos(ref List<WordTempInfo> lstTempletInfos)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                shRet = RestHandler.Instance.Get<WordTempInfo>("WordTempAccess/GetHospitalWordTempInfos", ref lstTempletInfos);
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.GetHospitalWordTempInfos(ref lstTempletInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定用户ID的文本模板列表
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="lstTempletInfos">文本模板列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetPersonalWordTempInfos(string szUserID, ref List<WordTempInfo> lstTempletInfos)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szUserID", szUserID);
                shRet = RestHandler.Instance.Get<WordTempInfo>("WordTempAccess/GetPersonalWordTempInfos", ref lstTempletInfos);
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.GetPersonalWordTempInfos(szUserID, ref lstTempletInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定病区的文本模板列表
        /// </summary>
        /// <param name="szDeptCode">病区代码</param>
        /// <param name="bOnlyDeptShare">是否仅返回标记为病区共享的文本模板</param>
        /// <param name="lstTempletInfos">文本模板列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetDeptWordTempInfos(string szDeptCode, bool bOnlyDeptShare, ref List<WordTempInfo> lstTempletInfos)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDeptCode", szDeptCode);
                RestHandler.Instance.AddParameter("bOnlyDeptShare", bOnlyDeptShare);
                shRet = RestHandler.Instance.Get<WordTempInfo>("WordTempAccess/GetDeptWordTempInfos", ref lstTempletInfos);
            }
            else
            {
                shRet = SystemContext.Instance.WordTempAccess.GetDeptWordTempInfos(szDeptCode, bOnlyDeptShare, ref lstTempletInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }
        #endregion
    }
}