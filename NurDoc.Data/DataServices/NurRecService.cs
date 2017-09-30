// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之护理记录数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.DAL.DbAccess;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.Data
{
    public class NurRecService : DBAccessBase
    {
        private static NurRecService m_instance = null;

        /// <summary>
        /// 获取护士护理工作相关服务实例
        /// </summary>
        public static NurRecService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NurRecService();
                return m_instance;
            }
        }

        private NurRecService()
        {
        }

        /// <summary>
        /// 通过护理记录ID号，获取护理记录
        /// </summary>
        /// <param name="szRecordID">护理记录ID号</param>
        /// <param name="nursingRecInfo">返回护理记录信息</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetNursingRec(string szRecordID, string PatientID, string VisitID, ref NursingRecInfo nursingRecInfo)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szRecordID", szRecordID);
                RestHandler.Instance.AddParameter("szPatientID", PatientID);
                RestHandler.Instance.AddParameter("szVisitID", VisitID);
                shRet = RestHandler.Instance.Get<NursingRecInfo>("NurRecAccess/GetNursingRec", ref nursingRecInfo);
            }
            else
            {
                shRet = SystemContext.Instance.NurRecAccess.GetNursingRec(szRecordID, PatientID, VisitID, ref nursingRecInfo);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取病人护理记录数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="dtBeginTime">记录时间开始时间</param>
        /// <param name="dtEndTime">记录时间结束时间</param>
        /// <param name="lstNursingRecInfos">护理记录列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetNursingRecList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<NursingRecInfo> lstNursingRecInfos)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                RestHandler.Instance.AddParameter("dtBeginTime", dtBeginTime);
                RestHandler.Instance.AddParameter("dtEndTime", dtEndTime);
                shRet = RestHandler.Instance.Get<NursingRecInfo>("NurRecAccess/GetNursingRecList1", ref lstNursingRecInfos);
            }
            else
            {
                shRet = SystemContext.Instance.NurRecAccess.GetNursingRecList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ref lstNursingRecInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取病人护理记录数据数量
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="dtBeginTime">记录时间</param>
        /// <param name="dtEndTime">记录时间</param>
        /// <param name="count">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingRecCount(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref int count)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                List<NursingRecInfo> lstNursingRecInfos = new List<NursingRecInfo>();
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                RestHandler.Instance.AddParameter("dtBeginTime", dtBeginTime);
                RestHandler.Instance.AddParameter("dtEndTime", dtEndTime);
                shRet = RestHandler.Instance.Get<NursingRecInfo>("NurRecAccess/GetNursingRecCount", ref lstNursingRecInfos);
                if (lstNursingRecInfos != null)
                    count = lstNursingRecInfos.Count;
                else
                    count = 0;
            }
            else
            {
                shRet = SystemContext.Instance.NurRecAccess.GetNursingRecCount(szPatientID, szVisitID, szSubID
               , dtBeginTime, dtEndTime, ref count);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取病人护理记录数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szSubID">就诊子ID</param>
        /// <param name="szSchemaID">列配置ID</param>
        /// <param name="dtBeginTime">记录时间</param>
        /// <param name="dtEndTime">记录时间</param>
        /// <param name="lstNursingRecInfos">体征数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingRecList(string szPatientID, string szVisitID, string szSubID, string szSchemaID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<NursingRecInfo> lstNursingRecInfos)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                RestHandler.Instance.AddParameter("szSchemaID", szSchemaID);
                RestHandler.Instance.AddParameter("dtBeginTime", dtBeginTime);
                RestHandler.Instance.AddParameter("dtEndTime", dtEndTime);
                shRet = RestHandler.Instance.Get<NursingRecInfo>("NurRecAccess/GetNursingRecList2", ref lstNursingRecInfos);
            }
            else
            {
                shRet = SystemContext.Instance.NurRecAccess.GetNursingRecList(szPatientID, szVisitID, szSubID, szSchemaID
                , dtBeginTime, dtEndTime, ref lstNursingRecInfos);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;

        }

        /// <summary>
        /// 保存指定的护理记录
        /// </summary>
        /// <param name="nursingRecInfo">护理记录信息</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveNursingRec(NursingRecInfo nursingRecInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(nursingRecInfo);
                return RestHandler.Instance.Post("NurRecAccess/SaveNursingRec");
            }
            else
            {
                return SystemContext.Instance.NurRecAccess.SaveNursingRec(nursingRecInfo);
            }
        }

        /// <summary>
        /// 保存指定的一系列体征数据列表
        /// </summary>
        /// <param name="nursingRecInfo">体征数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short UpdateNursingRec(NursingRecInfo nursingRecInfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(nursingRecInfo);
                return RestHandler.Instance.Post("NurRecAccess/UpdateNursingRec");
            }
            else
            {
                return SystemContext.Instance.NurRecAccess.UpdateNursingRec(nursingRecInfo);
            }
        }

        /// <summary>
        /// 更新指定的护理打印信息
        /// </summary>
        /// <param name="RecordID">护理打印记录信息</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short UpdatePrintState(string RecordID)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("RecordID", RecordID);
                return RestHandler.Instance.Put("NurRecAccess/UpdatePrintState");
            }
            else
            {
                return SystemContext.Instance.NurRecAccess.UpdatePrintState(RecordID);
            }
        }

        public short GetPrintLog(string szPatientID, string szVisitID, string szWardCode, string szSchemaID, ref RecPrintinfo RecPrintinfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter("szSchemaID", szSchemaID);
                return RestHandler.Instance.Get<RecPrintinfo>("NurRecAccess/GetPrintLog", ref RecPrintinfo);
            }
            else
            {
                return SystemContext.Instance.NurRecAccess.GetPrintLog(szPatientID, szVisitID, szWardCode, szSchemaID, ref RecPrintinfo);
            }
        }

        public short SavePrintLog(RecPrintinfo RecPrintinfo)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(RecPrintinfo);
                return RestHandler.Instance.Post("NurRecAccess/SavePrintLog");
            }
            else
            {
                return SystemContext.Instance.NurRecAccess.SavePrintLog(RecPrintinfo);
            }
        }

        /// <summary>
        /// 保存或更新护理记录数据,及其包含的摘要数据
        /// </summary>
        /// <param name="dtRecordTime">护理记录时间</param>
        /// <param name="nursingRecInfo">护理记录数据(数据返回 用于表格式数据录入重绑定)</param>
        /// <param name="lstSummaryData">摘要数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveNursingRec(DateTime dtRecordTime
            , ref NursingRecInfo nursingRecInfo, List<SummaryData> lstSummaryData)
        {
            if (nursingRecInfo == null)
                return SystemConst.ReturnValue.FAILED;

            string szPatientID = nursingRecInfo.PatientID;
            string szVisitID = nursingRecInfo.VisitID;
            string szSubID = nursingRecInfo.SubID;
            NursingRecInfo recInfo = null;
            short shRet = this.GetNursingRec(nursingRecInfo.RecordID, nursingRecInfo.PatientID, nursingRecInfo.VisitID, ref recInfo);
            if (shRet != SystemConst.ReturnValue.OK)
                return shRet;

            string szRecordID = nursingRecInfo.RecordID;
            if (recInfo == null)
            {
                shRet = this.SaveNursingRec(nursingRecInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                    return shRet;
                return this.SaveNursingRecData(nursingRecInfo, lstSummaryData);
            }
            else
            {
                if (recInfo == null)
                    return SystemConst.ReturnValue.FAILED;
                szRecordID = recInfo.RecordID;
                recInfo.RecordTime = nursingRecInfo.RecordTime;
                recInfo.RecordRemarks = nursingRecInfo.RecordRemarks;
                recInfo.RecordContent = nursingRecInfo.RecordContent;
                recInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
                recInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
                recInfo.ModifyTime = SysTimeService.Instance.Now;
                recInfo.Recorder1ID = nursingRecInfo.Recorder1ID;
                recInfo.Recorder1Name = nursingRecInfo.Recorder1Name;
                recInfo.Recorder2ID = nursingRecInfo.Recorder2ID;
                recInfo.Recorder2Name = nursingRecInfo.Recorder2Name;
                recInfo.RecordDate = nursingRecInfo.RecordTime.Date;
                shRet = this.UpdateNursingRec(recInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                    return shRet;
                nursingRecInfo = recInfo;
                return this.SaveNursingRecData(recInfo, lstSummaryData);
            }
        }

        /// <summary>
        /// 保存护理记录详情数据,比如体温、脉搏等
        /// </summary>
        /// <param name="nursingRecInfo">护理记录信息</param>
        /// <param name="lstSummaryData">详情数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        private short SaveNursingRecData(NursingRecInfo nursingRecInfo, List<SummaryData> lstSummaryData)
        {
            if (nursingRecInfo == null || lstSummaryData == null)
                return SystemConst.ReturnValue.FAILED;

            string szRecordID = nursingRecInfo.RecordID;

            foreach (SummaryData summaryData in lstSummaryData)
            {
                summaryData.DocID = nursingRecInfo.RecordID;
                summaryData.RecordID = nursingRecInfo.RecordID;
                summaryData.PatientID = nursingRecInfo.PatientID;
                summaryData.VisitID = nursingRecInfo.VisitID;
                summaryData.SubID = nursingRecInfo.SubID;
                summaryData.WardCode = nursingRecInfo.WardCode;
            }
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szDocID", szRecordID);
                RestHandler.Instance.AddParameter(lstSummaryData);
                shRet = RestHandler.Instance.Post("DocumentAccess/SaveSummaryData");
            }
            else
            {
                shRet = SystemContext.Instance.DocumentAccess.SaveSummaryData(szRecordID, lstSummaryData);
            }
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 合并指定的两个摘要数据列表
        /// </summary>
        /// <param name="souList">源列表</param>
        /// <param name="desList">目标列表</param>
        public void MergeSummaryDataList(List<SummaryData> souList, List<SummaryData> desList)
        {
            if (souList == null)
                return;
            if (desList == null)
                desList = new List<SummaryData>();

            if (desList.Count > 0)
            {
                int souIndex = souList.Count - 1;
                while (souIndex >= 0)
                {
                    SummaryData souData = souList[souIndex--];
                    int desIndex = desList.Count - 1;
                    while (desIndex >= 0)
                    {
                        SummaryData desData = desList[desIndex--];
                        if (desData.DataName == souData.DataName)
                        {
                            desList[desIndex + 1] = souData;
                            souList.RemoveAt(souIndex + 1);
                            break;
                        }
                    }
                }
            }
            desList.AddRange(souList);
        }

        /// <summary>
        /// 根据病人ID号,住院标识以及记录时间,删除一条护理记录数据信息
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">住院标识</param>
        /// <param name="szSubID">子ID</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <param name="szModifierID">修改人ID</param>
        /// <param name="szModifierName">修改人姓名</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteNursingRec(string szPatientID, string szVisitID, string szSubID
            , DateTime dtRecordTime, string szModifierID, string szModifierName)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID);
                RestHandler.Instance.AddParameter("dtRecordTime", dtRecordTime);
                RestHandler.Instance.AddParameter("szModifierID", szModifierID);
                RestHandler.Instance.AddParameter("szModifierName", szModifierName);
                shRet = RestHandler.Instance.Put("NurRecAccess/DeleteNursingRec1");
            }
            else
            {
                shRet = SystemContext.Instance.NurRecAccess.DeleteNursingRec(szPatientID, szVisitID, szSubID
                , dtRecordTime, szModifierID, szModifierName);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据病历类型ID,删除一条病历类型配置信息
        /// </summary>
        /// <param name="szRecordID">记录ID</param>
        /// <param name="szModifierID">修改人ID</param>
        /// <param name="szModifierName">修改人姓名</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteNursingRec(string szRecordID, string szModifierID, string szModifierName)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szRecordID", szRecordID);
                RestHandler.Instance.AddParameter("szModifierID", szModifierID);
                RestHandler.Instance.AddParameter("szModifierName", szModifierName);
                shRet = RestHandler.Instance.Put("NurRecAccess/DeleteNursingRec2");
            }
            else
            {
                shRet = SystemContext.Instance.NurRecAccess.DeleteNursingRec(szRecordID, szModifierID, szModifierName);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除摘要信息
        /// </summary>
        /// <param name="szRecordID">记录ID</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteSummaryData(string szRecordID)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szRecordID", szRecordID);
                shRet = RestHandler.Instance.Put("NurRecAccess/DeleteSummaryData");
            }
            else
            {
                shRet = SystemContext.Instance.NurRecAccess.DeleteSummaryData(szRecordID);
            }
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
    }
}