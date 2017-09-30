// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之生命体征数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public class VitalSignsService : DBAccessBase
    {
        private static VitalSignsService m_instance = null;

        /// <summary>
        /// 获取护士护理工作相关服务实例
        /// </summary>
        public static VitalSignsService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new VitalSignsService();
                return m_instance;
            }
        }

        private VitalSignsService()
        {
        }

        /// <summary>
        /// 保存指定的一系列体征数据列表
        /// </summary>
        /// <param name="lstVitalSignsData">体征数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveVitalSignsData(List<VitalSignsData> lstVitalSignsData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(lstVitalSignsData);
                return RestHandler.Instance.Post("VitalSignsAccess/SaveVitalSignsData1");
            }
            else
            {
                return SystemContext.Instance.VitalSignsAccess.SaveVitalSignsData(lstVitalSignsData);
            }
        }

        /// <summary>
        /// 保存指定的一条体征数据
        /// </summary>
        /// <param name="vitalSignsData">体征数据</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveVitalSignsData(VitalSignsData vitalSignsData)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(vitalSignsData);
                return RestHandler.Instance.Post("VitalSignsAccess/SaveVitalSignsData2");
            }
            else
            {
                return SystemContext.Instance.VitalSignsAccess.SaveVitalSignsData(vitalSignsData);
            }
        }

        /// <summary>
        /// 获取病人体征数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">病人子ID</param>
        /// <param name="dtBeginTime">记录起始时间</param>
        /// <param name="dtEndTime">记录截止时间</param>
        /// <param name="lstVitalSignsData">体征数据</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetVitalSignsList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<VitalSignsData> lstVitalSignsData)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                RestHandler.Instance.AddParameter("dtBeginTime", dtBeginTime);
                RestHandler.Instance.AddParameter("dtEndTime", dtEndTime);
                shRet = RestHandler.Instance.Get<VitalSignsData>("VitalSignsAccess/GetVitalSignsList1", ref lstVitalSignsData);
            }
            else
            {
                 shRet = SystemContext.Instance.VitalSignsAccess.GetVitalSignsList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ref lstVitalSignsData);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定的病人指定的数据来源对应的体征数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="szSubID">子ID号</param>
        /// <param name="szSourceTag">数据来源标记</param>
        /// <param name="szSourceType">数据来源类型标记</param>
        /// <param name="lstVitalSignsData">体征数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetVitalSignsList(string szPatientID, string szVisitID, string szSubID
            , string szSourceTag, string szSourceType, ref List<VitalSignsData> lstVitalSignsData)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                RestHandler.Instance.AddParameter("szSourceTag", szSourceTag);
                RestHandler.Instance.AddParameter("szSourceType", szSourceType);
                shRet = RestHandler.Instance.Get<VitalSignsData>("VitalSignsAccess/GetVitalSignsList2", ref lstVitalSignsData);
            }
            else
            {
                shRet = SystemContext.Instance.VitalSignsAccess.GetVitalSignsList(szPatientID, szVisitID, szSubID
                , szSourceTag, szSourceType, ref lstVitalSignsData);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取病人体征数据表中存储的入院事件发生的时间
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">病人子ID</param>
        /// <param name="dtAdmissionTime">返回的入院事件时间</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetAdmissionEventTime(string szPatientID, string szVisitID, string szSubID, ref DateTime? dtAdmissionTime)
        {
            short shRet = SystemConst.ReturnValue.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                shRet = RestHandler.Instance.Get<DateTime?>("VitalSignsAccess/GetAdmissionEventTime", ref dtAdmissionTime);
            }
            else
            {
                shRet = SystemContext.Instance.VitalSignsAccess.GetAdmissionEventTime(szPatientID, szVisitID, szSubID, ref dtAdmissionTime);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
    }
}