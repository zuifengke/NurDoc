// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之护理评价类型信息数据访问接口封装类.
// Creator:YeChongchong  Date:2016-01-13
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class EvaTypeService
    {
        private static EvaTypeService m_instance = null;

        /// <summary>
        /// 获取护理文档类型服务实例
        /// </summary>
        public static EvaTypeService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new EvaTypeService();
                return m_instance;
            }
        }

        private EvaTypeService()
        {
        }

        #region"护理评估模板读写接口"
        /// <summary>
        /// 获取所有病历类型信息
        /// </summary>
        /// <param name="szApplyEnv">表单应用环境</param>
        /// <param name="lstDocTypeInfos">返回的病历类型信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetEvaTypeInfos(ref List<EvaTypeInfo> lstEvaTypeInfos)
        {
            short shRet = SystemContext.Instance.EvaTypeAccess.GetEvaTypeInfos(ref lstEvaTypeInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定病历类型ID号的病历类型信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="docTypeInfo">返回的病历类型信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetEvaTypeInfo(string szDocTypeID, ref DocTypeInfo docTypeInfo)
        {
            return SystemContext.Instance.DocTypeAccess.GetDocTypeInfo(szDocTypeID, ref docTypeInfo);
        }

        public short GetEvaTypeItems(EvaTypeInfo evaTypeInfo, ref List<EvaTypeItem> listEvaTypeItem)
        {
            return SystemContext.Instance.EvaTypeAccess.GetEvaTypeItems(evaTypeInfo, ref listEvaTypeItem);
        }

        /// <summary>
        /// 保存一条新的病历类型配置信息
        /// </summary>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveEvaTypeInfo(EvaTypeInfo docTypeInfo)
        {
            return SystemContext.Instance.EvaTypeAccess.SaveEvaTypeInfo(docTypeInfo);
        }

        /// <summary>
        /// 修改一条病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型代码</param>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short ModifyEvaTypeInfo(string szDocTypeID, EvaTypeInfo docTypeInfo)
        {
            return SystemContext.Instance.EvaTypeAccess.ModifyEvaTypeInfo(szDocTypeID, docTypeInfo);
        }

        /// <summary>
        /// 删除一条病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteEvaTypeInfo(string szDocTypeID)
        {
            return SystemContext.Instance.DocTypeAccess.DeleteDocTypeInfo(szDocTypeID);
        }

        public short UpdateEvaItems(string szEvaTypeID, List<EvaTypeItem> lstEvaTypeItems)
        {
            return SystemContext.Instance.EvaTypeAccess.UpdateEvaItems(szEvaTypeID, lstEvaTypeItems);
        }

        /// <summary>
        /// 删除多条病历类型配置信息
        /// </summary>
        /// <param name="lstDocTypeID">病历类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteEvaTypeInfos(List<string> lstDocTypeID)
        {
            return SystemContext.Instance.EvaTypeAccess.DeleteEvaTypeInfos(lstDocTypeID);
        }

        #endregion

        #region"病区护理评价类型配置数据访问接口"
        /// <summary>
        ///获取统一病区下的所有护理评价类
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstWardDocTypes">病区病历类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWardEvaTypeList(string szWardCode, ref List<WardEvaType> lstWardEvaTypes)
        {
            short shRet = SystemContext.Instance.EvaTypeAccess.GetWardEvaTypeList(szWardCode, ref lstWardEvaTypes);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取所有文档类型代码应用到的病区列表
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="lstWardDocTypes">病区病历类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetEvaTypeDeptList(string szEvaTypeID, ref List<WardEvaType> lstWardEvaTypes)
        {
            short shRet = SystemContext.Instance.EvaTypeAccess.GetEvaTypeDeptList(szEvaTypeID, ref lstWardEvaTypes);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 保存指定的一系列病区护理评价类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">护理评价类型ID</param>
        /// <param name="wardDocTypes">病区护理评价类型配置信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardEvaTypes(string szEvaTypeID, List<WardEvaType> lstWardEvaTypes)
        {
            return SystemContext.Instance.EvaTypeAccess.SaveWardEvaTypes(szEvaTypeID, lstWardEvaTypes);
        }

        /// <summary>
        /// 保存一条新的病区护理评价类型配置信息
        /// </summary>
        /// <param name="wardDocType">病区护理评价类型信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardEvaType(WardEvaType wardEvaType)
        {
            return SystemContext.Instance.EvaTypeAccess.SaveWardEvaType(wardEvaType);
        }

        /// <summary>
        /// 根据病历类型代码删除当前护理评价类型对应的病区护理评价类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">护理评价类型ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteWardEvaTypes(string szEvaTypeID)
        {
            return SystemContext.Instance.EvaTypeAccess.DeleteWardEvaTypes(szEvaTypeID);
        }
        #endregion
    }
}