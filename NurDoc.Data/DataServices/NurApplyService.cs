// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之护理申请数据访问接口封装类.
// Creator:OuFengFang  Date:2013-3-24
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class NurApplyService
    {
        private static NurApplyService m_instance = null;

        /// <summary>
        /// 获取护理申请服务实例
        /// </summary>
        public static NurApplyService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NurApplyService();
                return m_instance;
            }
        }

        private NurApplyService()
        {
        }

        /// <summary>
        /// 根据申请单ID，获取申请单基本信息
        /// </summary>
        /// <param name="szApplyID">申请单编号</param>
        /// <param name="nurApplyInfo">申请单信息类</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyInfo(string szApplyID, ref NurApplyInfo nurApplyInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyInfo(szApplyID, ref nurApplyInfo);
        }

        /// <summary>
        /// 根据文档集ID获取申请单基本信息
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <param name="nurApplyInfo">申请单基本信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfoByDocSetID(string szDocSetID, ref NurApplyInfo nurApplyInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyInfoByDocSetID(szDocSetID, ref nurApplyInfo);
        }

        /// <summary>
        /// 根据会诊文档集ID获取申请信息
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <param name="NurApplyStatusInfo">护理申请状态信息类</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyStatusInfoByDocSetID(string szDocSetID, ref NurApplyStatusInfo NurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyStatusInfoByDocSetID(szDocSetID, ref NurApplyStatusInfo);
        }

        /// <summary>
        /// 获取病人申请单数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="dtBeginTime">记录时间</param>
        /// <param name="dtEndTime">记录时间</param>
        ///  <param name="szApplyType">申请单类型</param>
        /// <param name="lstNurApplyInfos">申请信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyInfoList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, string szApplyType, ref List<NurApplyInfo> lstNurApplyInfos)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyInfoList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime,szApplyType, ref lstNurApplyInfos);
        }

        /// <summary>
        /// 保存护理申请
        /// </summary>
        /// <param name="nurApplyInfo">申请单信息</param>
        /// <param name="nurApplyStatusInfo">申请单状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveNurApply(NurApplyInfo nurApplyInfo,NurApplyStatusInfo nurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.SaveNurApply(nurApplyInfo, nurApplyStatusInfo);
        }

        /// <summary>
        /// 更新已有申请单
        /// </summary>
        /// <param name="nurApplyInfo">申请信息</param>
        /// <param name="nurApplyStatusInfo">申请单状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateNurApply(NurApplyInfo nurApplyInfo,NurApplyStatusInfo nurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.UpdateNurApply(nurApplyInfo, nurApplyStatusInfo);
        }

        /// <summary>
        /// 删除已有申请单
        /// </summary>
        /// <param name="nurApplyInfo">申请信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DelNurApply(NurApplyInfo nurApplyInfo)
        {
            return SystemContext.Instance.NurApplyAccess.DelNurApply(nurApplyInfo);
        }

        /// <summary>
        /// 获取指定申请、指定操作时间以及指定状态信息
        /// </summary>
        /// <param name="szApplyID">申请单编号</param>
        /// <param name="szStatus">申请单状态</param>
        /// <param name="dtOperateTime">申请单状态创建时间</param>
        /// <param name="nurApplyStatusInfo">申请单状态信息类</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyStatusInfo(string szApplyID, string szStatus, DateTime dtOperateTime, ref NurApplyStatusInfo nurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyStatusInfo(szApplyID, szStatus, dtOperateTime, ref nurApplyStatusInfo);
        }
    }
}