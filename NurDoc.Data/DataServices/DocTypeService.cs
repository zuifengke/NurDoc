﻿// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之表单类型信息数据访问接口封装类.
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
    public class DocTypeService : DBAccessBase
    {
        private static DocTypeService m_instance = null;

        /// <summary>
        /// 获取护理文档类型服务实例
        /// </summary>
        public static DocTypeService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new DocTypeService();
                return m_instance;
            }
        }

        private DocTypeService()
        {
        }

        #region"病历类型数据访问接口"
        /// <summary>
        /// 获取所有病历类型信息
        /// </summary>
        /// <param name="szApplyEnv">表单应用环境</param>
        /// <param name="lstDocTypeInfos">返回的病历类型信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocTypeInfos(string szApplyEnv, ref List<DocTypeInfo> lstDocTypeInfos)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szApplyEnv", szApplyEnv);
                shRet = RestHandler.Instance.Get<DocTypeInfo>("DocTypeAccess/GetDocTypeInfos", ref lstDocTypeInfos);
            }
            else
            {
                shRet = SystemContext.Instance.DocTypeAccess.GetDocTypeInfos(szApplyEnv, ref lstDocTypeInfos);
            }
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
        public short GetDocTypeInfo(string szDocTypeID, ref DocTypeInfo docTypeInfo)
        { 
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                return RestHandler.Instance.Get<DocTypeInfo>("DocTypeAccess/GetDocTypeInfo", ref docTypeInfo);
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.GetDocTypeInfo(szDocTypeID, ref docTypeInfo);
            }
        }

        /// <summary>
        /// 保存一条新的病历类型配置信息
        /// </summary>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveDocTypeInfo(DocTypeInfo docTypeInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(docTypeInfo);
                return RestHandler.Instance.Post("DocTypeAccess/SaveDocTypeInfo");
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.SaveDocTypeInfo(docTypeInfo);
            }
        }

        /// <summary>
        /// 修改一条病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型代码</param>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short ModifyDocTypeInfo(string szDocTypeID, DocTypeInfo docTypeInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                RestHandler.Instance.AddParameter(docTypeInfo);
                return RestHandler.Instance.Put("DocTypeAccess/ModifyDocTypeInfo");
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.ModifyDocTypeInfo(szDocTypeID, docTypeInfo);
            }
        }

        /// <summary>
        /// 删除一条病历类型配置信息
        /// </summary>
        /// <param name="szDocTypeID">病历类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteDocTypeInfo(string szDocTypeID)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                return RestHandler.Instance.Post("DocTypeAccess/DeleteDocTypeInfo");
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.DeleteDocTypeInfo(szDocTypeID);
            }
        }

        /// <summary>
        /// 删除多条病历类型配置信息
        /// </summary>
        /// <param name="lstDocTypeID">病历类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteDocTypeInfos(List<string> lstDocTypeID)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(lstDocTypeID);
                return RestHandler.Instance.Post("DocTypeAccess/DeleteDocTypeInfos");
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.DeleteDocTypeInfos(lstDocTypeID);
            }
        }
        #endregion

        #region"病区病历类型配置数据访问接口"
        /// <summary>
        /// 获取病区病历类型信息列表
        /// </summary>
        /// <param name="szDeptCode">病区代码</param>
        /// <param name="lstWardDocTypes">返回的病区病历类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetWardDocTypeList(string szDeptCode, ref List<WardDocType> lstWardDocTypes)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDeptCode", szDeptCode);
                return RestHandler.Instance.Get<WardDocType>("DocTypeAccess/GetWardDocTypeList",ref lstWardDocTypes);
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.GetWardDocTypeList(szDeptCode, ref lstWardDocTypes);
            }
        }

        /// <summary>
        /// 获取病区病历类型信息列表
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="lstWardDocTypes">返回的病区病历类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocTypeDeptList(string szDocTypeID, ref List<WardDocType> lstWardDocTypes)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                return RestHandler.Instance.Get<WardDocType>("DocTypeAccess/GetDocTypeDeptList",ref lstWardDocTypes);
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.GetDocTypeDeptList(szDocTypeID, ref lstWardDocTypes);
            }
        }

        /// <summary>
        /// 获取病区病历类型信息列表
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="lstWardDocTypes">病区病历类型列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveWardDocTypes(string szDocTypeID, List<WardDocType> lstWardDocTypes)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocTypeID", szDocTypeID);
                RestHandler.Instance.AddParameter(lstWardDocTypes);
                return RestHandler.Instance.Post("DocTypeAccess/SaveWardDocTypes");
            }
            else
            {
                return SystemContext.Instance.DocTypeAccess.SaveWardDocTypes(szDocTypeID, lstWardDocTypes);
            }
      }
        #endregion
    }
}