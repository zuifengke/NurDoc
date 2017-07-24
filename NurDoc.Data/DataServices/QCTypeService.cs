// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之患者满意度记录等表单文档数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class QCTypeService
    {
        private static QCTypeService m_instance = null;

        /// <summary>
        /// 获取质量与安全管理记录文档服务实例
        /// </summary>
        public static QCTypeService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new QCTypeService();
                return m_instance;
            }
        }

        private QCTypeService()
        {
        }

        #region"质量与安全管理记录类型数据访问接口"
        /// <summary>
        /// 获取所有质量与安全管理类型信息
        /// </summary>
        /// <param name="szApplyEnv">表单应用环境</param>
        /// <param name="lstQCTypeInfos">返回的质量与安全管理类型信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetQCTypeInfos(string szApplyEnv, ref List<QCTypeInfo> lstQCTypeInfos)
        {
            short shRet = SystemContext.Instance.QCTypeAccess.GetQCTypeInfos(szApplyEnv, ref lstQCTypeInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取指定质量与安全管理记录类型ID号的质量与安全管理记录类型信息
        /// </summary>
        /// <param name="szQCTypeID">质量与安全管理记录类型ID号</param>
        /// <param name="qcTypeInfo">返回的质量与安全管理记录类型信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetQCTypeInfo(string szQCTypeID, ref QCTypeInfo qcTypeInfo)
        {
            return SystemContext.Instance.QCTypeAccess.GetQCTypeInfo(szQCTypeID, ref qcTypeInfo);
        }
        #endregion
    }
}
