// ***********************************************************
// 护理电子病历系统,护理计划数据保存更新处理类.
// Creator:OuFengFang  Date:2013-4-3
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage.NCP
{
    internal class NurCarePlanHandler
    {
        private static NurCarePlanHandler m_instance = null;

        /// <summary>
        /// 获取实例
        /// </summary>
        public static NurCarePlanHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NurCarePlanHandler();
                return m_instance;
            }
        }

        private NurCarePlanHandler()
        {
        }

        /// <summary>
        /// 创建一条新的护理计划主索引信息
        /// </summary>
        /// <param name="document">文档信息类</param>
        /// <param name="szDiagCode">诊断编码</param>
        /// <param name="szDiagName">诊断名称</param>
        /// <param name="dtStartTime">开始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="szStatus">状态信息</param>
        /// <returns>护理计划记录主索引信息</returns>
        private NurCarePlanInfo CreateNurCarePlanInfo(DocumentInfo document
            , string szDiagCode, string szDiagName
            , DateTime dtStartTime, DateTime dtEndTime, string szStatus)
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            if (document == null)
                return null;
            NurCarePlanInfo ncpInfo = new NurCarePlanInfo();
            ncpInfo.CreateTime = SysTimeService.Instance.Now;
            ncpInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            ncpInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            ncpInfo.DiagCode = szDiagCode;
            ncpInfo.DiagName = szDiagName;
            ncpInfo.DocID = document.DocSetID;
            ncpInfo.DocTypeID = document.DocTypeID;
            ncpInfo.EndTime = dtEndTime;
            ncpInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            ncpInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            ncpInfo.ModifyTime = ncpInfo.CreateTime;
            ncpInfo.PatientID = document.PatientID;
            ncpInfo.PatientName = document.PatientName;
            ncpInfo.StartTime = dtStartTime;
            ncpInfo.Status = szStatus;
            ncpInfo.SubID = document.SubID;
            ncpInfo.VisitID = document.VisitID;
            ncpInfo.WardCode = document.WardCode;
            ncpInfo.WardName = document.WardName;
            ncpInfo.NCPID = ncpInfo.MakeNCPID();
            return ncpInfo;
        }

        /// <summary>
        /// 修改指定的待更新的护理计划记录单主索引信息
        /// </summary>
        /// <param name="ncpInfo">护理计划主索引信息</param>
        /// <param name="szDiagCode">诊断编码</param>
        /// <param name="szDiagName">诊断名称</param>
        /// <param name="dtStartTime">开始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="szStatus">护理计划状态</param>
        /// <returns>护理计划主索引信息</returns>
        private NurCarePlanInfo ModifyNurCarePlanInfo(NurCarePlanInfo ncpInfo
            , string szDiagCode, string szDiagName, DateTime dtStartTime
            , DateTime dtEndTime, string szStatus)
        {
            if (SystemContext.Instance.LoginUser == null)
                return ncpInfo;
            if (ncpInfo != null)
            {
                ncpInfo.DiagCode = szDiagCode;
                ncpInfo.DiagName = szDiagName;
                ncpInfo.EndTime = dtEndTime; ;
                ncpInfo.StartTime = dtStartTime;
                ncpInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
                ncpInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
                ncpInfo.ModifyTime = SysTimeService.Instance.Now;
                ncpInfo.Status = szStatus;
            }
            return ncpInfo;
        }

        /// <summary>
        /// 创建一条新的护理计划状态信息
        /// </summary>
        /// <param name="ncpInfo">护理计划信息</param>
        /// <param name="dtOperateTime">操作时间</param>
        /// <param name="szStatus">状态信息</param>
        /// <returns>护理计划状态信息类</returns>
        private NurCarePlanStatusInfo CreateNurCarePlanStatusInfo(NurCarePlanInfo ncpInfo
            , DateTime dtOperateTime, string szStatus)
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            if (ncpInfo == null)
                return null;
            NurCarePlanStatusInfo ncpStatusInfo = new NurCarePlanStatusInfo();
            ncpStatusInfo.NCPID = ncpInfo.NCPID;
            ncpStatusInfo.OperateTime = dtOperateTime;
            ncpStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
            ncpStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
            ncpStatusInfo.Status = szStatus;
            if (SystemContext.Instance.SystemOption.NpcNewModel)
            {
                NurCarePlanConfig config = SystemContext.Instance.GetConfigFromListByStatus(szStatus);
                ncpStatusInfo.StatusDesc = config == null ? string.Empty : config.StatusDesc;
            }
            else
            {
                ncpStatusInfo.StatusDesc = ServerData.NurCarePlanStatus.GetStatusDesc(szStatus);
            }
            return ncpStatusInfo;
        }

        /// <summary>
        /// <summary>
        ///保存护理计划以及状态信息
        /// </summary>
        /// <param name="document">文档信息类</param>
        /// <param name="szDiagCode">诊断编码</param>
        /// <param name="szDiagName">诊断名称</param>
        /// <param name="dtStartTime">开始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="szStatus">状态信息</param>
        /// <returns>是否成功</returns>
        public bool HandleNurCarePlanNewSave(DocumentInfo document, string szDiagCode, string szDiagName
            , DateTime dtStartTime, DateTime dtEndTime, string szStatus)
        {
            NurCarePlanInfo ncpInfo = this.CreateNurCarePlanInfo(document
                , szDiagCode, szDiagName, dtStartTime, dtEndTime, szStatus);
            if (ncpInfo == null)
                return false;

            NurCarePlanStatusInfo ncpStatusInfo = this.CreateNurCarePlanStatusInfo(ncpInfo, ncpInfo.CreateTime, szStatus);
            if (ncpStatusInfo == null)
                return false;

            short shRet = CarePlanService.Instance.SaveNurCarePlan(ncpInfo, ncpStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理计划数据保存失败!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// <summary>
        ///更新护理计划以及状态信息
        /// </summary>
        /// <param name="document">文档信息类</param>
        /// <param name="szNCPID">护理记录ID</param>
        /// <param name="szDiagCode">诊断编码</param>
        /// <param name="szDiagName">诊断名称</param>
        /// <param name="dtStartTime">开始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="szStatus">状态信息</param>
        /// <returns>是否成功</returns>
        public bool HandleNurCarePlanModifySave(NurCarePlanInfo ncpInfo, string szDiagCode, string szDiagName
            , DateTime dtStartTime, DateTime dtEndTime, string szStatus)
        {
            ncpInfo = this.ModifyNurCarePlanInfo(ncpInfo, szDiagCode, szDiagName, dtStartTime, dtEndTime, szStatus);
            NurCarePlanStatusInfo ncpStatusInfo = this.CreateNurCarePlanStatusInfo(ncpInfo, ncpInfo.ModifyTime, szStatus);
            if (ncpStatusInfo == null)
                return false;

            short shRet = CarePlanService.Instance.UpdateNurCarePlan(ncpInfo, ncpStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理计划数据更新失败!");
                return false;
            }
            return true;
        }
    }
}
