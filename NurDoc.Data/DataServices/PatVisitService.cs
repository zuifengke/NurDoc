// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之病人就诊数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public class PatVisitService : DBAccessBase
    {
        private static PatVisitService m_instance = null;

        /// <summary>
        /// 获取病人就诊服务实例
        /// </summary>
        public static PatVisitService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PatVisitService();
                return m_instance;
            }
        }

        private PatVisitService()
        {
        }

        /// <summary>
        /// 查询指定病人所经过病区列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="lstDeptInfos">病区列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetPatientDeptList(string szPatientID, string szVisitID, ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                return RestHandler.Instance.Get<DeptInfo>("PatVisitAccess/GetPatientDeptList", ref lstDeptInfos);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetPatientDeptList(szPatientID, szVisitID, ref lstDeptInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 查询指定病人转科列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="lstTransferInfos">转科列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPatientTransferList(string szPatientID, string szVisitID, ref List<TransferInfo> lstTransferInfos)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                return RestHandler.Instance.Get<TransferInfo>("PatVisitAccess/GetPatientTransferList", ref lstTransferInfos);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetPatientTransferList(szPatientID, szVisitID, ref lstTransferInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定病人指定次入院对应的住院信息
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">住院标识</param>
        /// <param name="patVisitInfo">住院信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetPatVisitInfo(string szPatientID, string szVisitID, ref PatVisitInfo patVisitInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                return RestHandler.Instance.Get<PatVisitInfo>("PatVisitAccess/GetPatVisitInfo", ref patVisitInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetPatVisitInfo(szPatientID, szVisitID, ref patVisitInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.FAILED;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据病人ID和就诊号，获取该次住院的检验信息列表
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="lstLabTestInfo">检验信息列表</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short GetInpLabTestList(string szPatientID, string szVisitID, ref List<LabTestInfo> lstLabTestInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                return RestHandler.Instance.Get<LabTestInfo>("PatVisitAccess/GetInpLabTestList", ref lstLabTestInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetInpLabTestList(szPatientID, szVisitID, ref lstLabTestInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        ///  根据病人ID和就诊时间区间，获取该次门诊的检验信息列表
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="dtVisitTime">该次就诊时间</param>
        /// <param name="dtNextVisitTime">下次就诊时间</param>
        /// <param name="lstLabTestInfo">检验信息列表</param>
        /// <returns>Common.CommonData.ReturnValu</returns>
        public short GetClinicLabTestList(string szPatientID, DateTime dtVisitTime, DateTime dtNextVisitTime, ref List<LabTestInfo> lstLabTestInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("dtVisitTime", dtVisitTime);
                RestHandler.Instance.AddParameter("dtNextVisitTime", dtNextVisitTime);
                return RestHandler.Instance.Get<LabTestInfo>("PatVisitAccess/GetClinicLabTestList", ref lstLabTestInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetClinicLabTestList(szPatientID, dtVisitTime, dtNextVisitTime, ref lstLabTestInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据指定的申请序号，获取检验结果列表
        /// </summary>
        /// <param name="szTestNo">申请序号</param>
        /// <param name="lstTestResultInfo">检验结果信息类</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short GetTestResultList(string szTestNo, ref List<TestResultInfo> lstTestResultInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szTestNo", szTestNo);
                return RestHandler.Instance.Get<TestResultInfo>("PatVisitAccess/GetTestResultList", ref lstTestResultInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetTestResultList(szTestNo, ref lstTestResultInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据指定的就诊信息，获取该次住院产生的检查数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="dtVisitTime">该次就诊时间</param>
        /// <param name="dtNextVisitTime">下次就诊时间</param>
        /// <param name="lstExamInfo">检查信息列表</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short GetClinicExamList(string szPatientID, DateTime dtVisitTime, DateTime dtNextVisitTime, ref List<ExamInfo> lstExamInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("dtVisitTime", dtVisitTime);
                RestHandler.Instance.AddParameter("dtNextVisitTime", dtNextVisitTime);
                return RestHandler.Instance.Get<ExamInfo>("PatVisitAccess/GetClinicExamList", ref lstExamInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetClinicExamList(szPatientID, dtVisitTime, dtNextVisitTime, ref lstExamInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据指定的就诊信息，获取该次门诊产生的检查数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊次数</param>
        /// <param name="lstExamInfo">检查信息列表</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short GetInpExamList(string szPatientID, string szVisitID, ref List<ExamInfo> lstExamInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                return RestHandler.Instance.Get<ExamInfo>("PatVisitAccess/GetInpExamList", ref lstExamInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetInpExamList(szPatientID, szVisitID, ref lstExamInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据指定的就诊信息，获取该次就诊时，产生的检查报告详细信息
        /// </summary>
        /// <param name="szExamNo">申请序号</param>
        /// <param name="examReportInfo">检查报告信息</param>
        /// <returns>MedDocSys.Common.CommonData.ReturnValue</returns>
        public short GetExamResultInfo(string szExamNo, ref ExamResultInfo examReportInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szExamNo", szExamNo);
                return RestHandler.Instance.Get<ExamResultInfo>("PatVisitAccess/GetExamResultInfo", ref examReportInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetExamResultInfo(szExamNo, ref examReportInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取病人入院到护理评估期间产生的医嘱数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="dtFrom">入院时间</param>
        /// <param name="dtTo">评估时间</param>
        /// <param name="lstOrderInfo">医嘱信息列表</param>
        /// <returns>ReturnValue</returns>
        public short GetPerformOrderList(string szPatientID, string szVisitID, DateTime dtFrom, DateTime dtTo, ref List<OrderInfo> lstOrderInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("dtFrom", dtFrom);
                RestHandler.Instance.AddParameter("dtTo", dtTo);
                return RestHandler.Instance.Get<OrderInfo>("PatVisitAccess/GetPerformOrderList", ref lstOrderInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetPerformOrderList(szPatientID, szVisitID, dtFrom, dtTo, ref lstOrderInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取当次医嘱编号拆分后的医嘱
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊编号</param>
        /// <param name="szOrderNO">医嘱编号</param>
        /// <param name="lstPlanOrderInfo">医嘱拆分信息列表</param>
        /// <returns>ReturnValue</returns>
        public short GetPlanOrderList(string szPatientID, string szVisitID, string szOrderNO, ref List<PlanOrderInfo> lstPlanOrderInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szOrderNO", szOrderNO);
                return RestHandler.Instance.Get<PlanOrderInfo>("PatVisitAccess/GetPlanOrderList", ref lstPlanOrderInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetPlanOrderList(szPatientID, szVisitID, szOrderNO, ref lstPlanOrderInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="szPatStatus">病人状态:IN/OUT/IN_AND_OUT</param>
        /// <param name="szWardCode">护理单元代码</param>
        /// <param name="szPatientName">病人姓名</param>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szNurseClass">护理等级</param>
        /// <param name="szPatCondition">病人病情</param>
        /// <param name="dtStart">时间开始[病人状态所有、在院查入院时间，出院查出院时间]</param>
        /// <param name="dtEnd">时间结束[病人状态所有、在院查入院时间，出院查出院时间]</param>
        /// <param name="lstPatvisitInfo">病人信息列表</param>
        /// <returns>ReturnValue</returns>
        public short GetPatVisitInfo(string szPatStatus, string szWardCode, string szPatientName,
            string szPatientID, string szNurseClass, string szPatCondition,
            DateTime? dtStart, DateTime? dtEnd, ref List<PatVisitInfo> lstPatvisitInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatStatus", szPatientID);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter("szPatientName", szPatientName);
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szNurseClass", szNurseClass);
                RestHandler.Instance.AddParameter("szPatCondition", szPatCondition);
                RestHandler.Instance.AddParameter("dtStart", dtStart);
                RestHandler.Instance.AddParameter("dtEnd", dtEnd);
                return RestHandler.Instance.Get<PatVisitInfo>("PatVisitAccess/GetPatVisitInfo2", ref lstPatvisitInfo);
            }
            else
            {
                shRet = SystemContext.Instance.PatVisitAccess.GetPatVisitInfo(szPatStatus, szWardCode, szPatientName,
                szPatientID, szNurseClass, szPatCondition,
               dtStart, dtEnd, ref lstPatvisitInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return ServerData.ExecuteResult.OK;
        }
    }
}