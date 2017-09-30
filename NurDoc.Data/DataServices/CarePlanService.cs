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
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public class CarePlanService : DBAccessBase
    {
        private static CarePlanService m_instance = null;

        /// <summary>
        /// 获取护理计划服务实例
        /// </summary>
        public static CarePlanService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new CarePlanService();
                return m_instance;
            }
        }

        private CarePlanService()
        {
        }

        #region "护理计划字典表访问"
        /// <summary>
        /// 查询指定的护理诊断对应的护理计划,护理措施等字典数据信息
        /// </summary>
        /// <param name="szDiagCode">护理诊断代码</param>
        /// <param name="lstNurCarePlanDictInfos">护理计划,护理措施等字典数据列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanDictInfo(string szDiagCode, ref List<NurCarePlanDictInfo> lstNurCarePlanDictInfos)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDiagCode", szDiagCode);
                shRet = RestHandler.Instance.Get<NurCarePlanDictInfo>("CarePlanAccess/GetNurCarePlanDictInfo1",ref lstNurCarePlanDictInfos);
            }
            else
            {
                shRet = SystemContext.Instance.CarePlanAccess.GetNurCarePlanDictInfo(szDiagCode, ref lstNurCarePlanDictInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除指定的护理诊断及包含的所有护理计划,护理措施等字典数据
        /// </summary>
        /// <param name="szDiagCode">护理诊断代码</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteNurCarePlanDictInfo(string szDiagCode)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDiagCode", szDiagCode);
                shRet = RestHandler.Instance.Post("CarePlanAccess/DeleteNurCarePlanDictInfo");
            }
            else
            {
                shRet = SystemContext.Instance.CarePlanAccess.DeleteNurCarePlanDictInfo(szDiagCode);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除指定的护理诊断及包含的指定的护理计划,护理措施等字典数据
        /// </summary>
        /// <param name="szDiagCode">护理诊断代码</param>
        /// <param name="szItemCode">护理计划字典数据代码</param>
        /// <param name="szItemType">护理计划字典数据类型</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteNurCarePlanDictInfo(string szDiagCode, string szItemCode, string szItemType)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDiagCode", szDiagCode);
                RestHandler.Instance.AddParameter("szItemCode", szItemCode);
                RestHandler.Instance.AddParameter("szItemType", szItemType);
                shRet = RestHandler.Instance.Post("CarePlanAccess/DeleteNurCarePlanDictInfo");
            }
            else
            {
                shRet = SystemContext.Instance.CarePlanAccess.DeleteNurCarePlanDictInfo(szDiagCode, szItemCode, szItemType);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的一条护理诊断对应的护理计划字典数据
        /// </summary>
        /// <param name="ncpDictInfo">护理计划字典数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveNurCarePlanDictInfo(NurCarePlanDictInfo ncpDictInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(ncpDictInfo.DiagCode, ncpDictInfo.Item);
                shRet = RestHandler.Instance.Post("CarePlanAccess/SaveNurCarePlanDictInfo");
            }
            else
            { 
                shRet = SystemContext.Instance.CarePlanAccess.SaveNurCarePlanDictInfo(ncpDictInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新指定的护理诊断对应的指定的护理计划等字典数据
        /// </summary>
        /// <param name="szDiagCode">护理诊断代码</param>
        /// <param name="szItemCode">护理计划字典数据代码</param>
        /// <param name="szItemType">护理计划字典数据类型</param>
        /// <param name="ncpDictInfo">护理计划新的字典数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateNurCarePlanDictInfo(string szDiagCode, string szItemCode, string szItemType, NurCarePlanDictInfo ncpDictInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDiagCode", szDiagCode);
                RestHandler.Instance.AddParameter("szItemCode", szItemCode);
                RestHandler.Instance.AddParameter("szItemType", szItemType);
                RestHandler.Instance.AddParameter(ncpDictInfo.DiagCode,ncpDictInfo.Item);
                shRet = RestHandler.Instance.Post("CarePlanAccess/UpdateNurCarePlanDictInfo");
            }
            else
            {
                shRet = SystemContext.Instance.CarePlanAccess.UpdateNurCarePlanDictInfo(szDiagCode, szItemCode, szItemType, ncpDictInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
        #endregion

        /// <summary>
        /// 根据计划记录ID，获取护理计划基本信息
        /// </summary>
        /// <param name="szNCPID">护理计划记录ID</param>
        /// <param name="ncpInfo">护理计划信息类</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanInfo(string szNCPID, ref NurCarePlanInfo ncpInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szNCPID", szNCPID);
                return RestHandler.Instance.Get<NurCarePlanInfo>("CarePlanAccess/GetNurCarePlanInfo", ref ncpInfo);
            }
            else
            {
                return SystemContext.Instance.CarePlanAccess.GetNurCarePlanInfo(szNCPID, ref ncpInfo);
            }
        }

        /// <summary>
        /// 获取病人护理计划记录数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="dtBeginTime">记录时间</param>
        /// <param name="dtEndTime">记录时间</param>
        /// <param name="lstNurCarePlanInfos">护理计划信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanInfoList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<NurCarePlanInfo> lstNurCarePlanInfos)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                RestHandler.Instance.AddParameter("dtBeginTime", dtBeginTime);
                RestHandler.Instance.AddParameter("dtEndTime", dtEndTime);
                return RestHandler.Instance.Get<NurCarePlanInfo>("CarePlanAccess/GetNurCarePlanInfoList", ref lstNurCarePlanInfos);
            }
            else
            {
                return SystemContext.Instance.CarePlanAccess.GetNurCarePlanInfoList(szPatientID, szVisitID, szSubID, dtBeginTime, dtEndTime, ref lstNurCarePlanInfos);
            }
        }

        /// <summary>
        /// 保存护理计划
        /// </summary>
        /// <param name="ncpInfo">护理计划信息</param>
        /// <param name="ncpStatusInfo">护理计划状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveNurCarePlan(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("ncpInfo", ncpInfo);
                RestHandler.Instance.AddParameter("ncpStatusInfo", ncpStatusInfo);
                return RestHandler.Instance.Post("CarePlanAccess/SaveNurCarePlan");
            }
            else
            {
                return SystemContext.Instance.CarePlanAccess.SaveNurCarePlan(ncpInfo, ncpStatusInfo);
            }
        }

        /// <summary>
        /// 更新已有护理计划
        /// </summary>
        /// <param name="ncpInfo">护理计划信息</param>
        /// <param name="ncpStatusInfo">护理计划状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateNurCarePlan(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("ncpInfo", ncpInfo);
                RestHandler.Instance.AddParameter("ncpStatusInfo", ncpStatusInfo);
                return RestHandler.Instance.Put("CarePlanAccess/UpdateNurCarePlan");
            }
            else
            {
                return SystemContext.Instance.CarePlanAccess.UpdateNurCarePlan(ncpInfo, ncpStatusInfo);
            }
        }

        /// <summary>
        /// 获取指定护理计划记录ID、指定操作时间以及指定状态信息
        /// </summary>
        /// <param name="szNCPID">护理计划记录ID</param>
        /// <param name="szStatus">护理计划状态</param>
        /// <param name="dtOperateTime">护理计划状态创建时间</param>
        /// <param name="ncpStatusInfo">护理计划状态信息类</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanStatusInfo(string szNCPID, string szStatus, DateTime dtOperateTime, ref NurCarePlanStatusInfo ncpStatusInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szNCPID", szNCPID);
                RestHandler.Instance.AddParameter("szStatus", szStatus);
                RestHandler.Instance.AddParameter("dtOperateTime", dtOperateTime);
                return RestHandler.Instance.Get<NurCarePlanStatusInfo>("CarePlanAccess/GetNurCarePlanStatusInfo",ref ncpStatusInfo);
            }
            else
            {
                return SystemContext.Instance.CarePlanAccess.GetNurCarePlanStatusInfo(szNCPID, szStatus, dtOperateTime, ref ncpStatusInfo);
            }
        }
    }
}