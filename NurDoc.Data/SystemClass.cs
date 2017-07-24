// ***********************************************************
// 封装客户端解决方案内共享的类型集合
// Creator:YangMingkun  Date:2012-3-20
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Collections.Generic;

namespace Heren.NurDoc.Data
{
    #region "enum"
    /// <summary>
    /// 客户端文档状态
    /// </summary>
    public enum DocumentState
    {
        /// <summary>
        /// 未知状态(0)
        /// </summary>
        None = 0,

        /// <summary>
        /// 新建状态(1)
        /// </summary>
        New = 1,

        /// <summary>
        /// 编辑状态(2)
        /// </summary>
        Edit = 2,

        /// <summary>
        /// 修订状态(3)
        /// </summary>
        Revise = 3,

        /// <summary>
        /// 浏览状态(4)
        /// </summary>
        View = 4
    }

    /// <summary>
    /// 病人列表视图模式
    /// </summary>
    public enum PatientView
    {
        /// <summary>
        /// 细卡(0)
        /// </summary>
        DetailCard = 0,

        /// <summary>
        /// 简卡(1)
        /// </summary>
        SimpleCard = 1,

        /// <summary>
        /// 病人列表(2)
        /// </summary>
        PatientList = 2
    }
    #endregion

    #region "class"
    #endregion
}
