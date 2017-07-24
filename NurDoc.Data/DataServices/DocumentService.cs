// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之入院评估单等表单文档数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Heren.NurDoc.DAL;
using Heren.Common.Forms.Editor;

namespace Heren.NurDoc.Data
{
    public class DocumentService
    {
        private static DocumentService m_instance = null;

        /// <summary>
        /// 获取护理文档服务实例
        /// </summary>
        public static DocumentService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new DocumentService();
                return m_instance;
            }
        }

        private DocumentService()
        {
        }

        /// <summary>
        /// 获取指定文档ID对应的文档信息
        /// </summary>
        /// <param name="szDocID">文档ID</param>
        /// <param name="docInfo">文档信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocInfo(string szDocID, ref NurDocInfo docInfo)
        {
            return SystemContext.Instance.DocumentAccess.GetDocInfo(szDocID, ref docInfo);
        }

        /// <summary>
        /// 获取指定病人指定就诊下指定病历类型对应的已写病历列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szDocTypeID">病历类型代码</param>
        /// <param name="lstDocInfos">已写病历列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocInfos(string szPatientID, string szVisitID, string szDocTypeID, ref NurDocList lstDocInfos)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetDocInfos(szPatientID, szVisitID, szDocTypeID, ref lstDocInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定病人指定就诊下指定病历类型对应的已写病历列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szDocTypeID">病历类型代码</param>
        /// <param name="dtBeginTime">起始时间</param>
        /// <param name="dtEndTime">截止时间</param>
        /// <param name="lstDocInfos">已写病历列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocInfos(string szPatientID, string szVisitID, string szDocTypeID
            , DateTime dtBeginTime, DateTime dtEndTime, ref NurDocList lstDocInfos)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetDocInfos(szPatientID, szVisitID, szDocTypeID
                , dtBeginTime, dtEndTime, ref lstDocInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定病人指定就诊下指定病历类型对应的已写病历列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szDocTypeID">病历类型代码</param>
        /// <param name="dtBeginTime">起始时间</param>
        /// <param name="dtEndTime">截止时间</param>
        /// <param name="lstDocInfos">已写病历列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocInfosOrderByRecordTime(string szPatientID, string szVisitID, string szDocTypeID
            , DateTime dtBeginTime, DateTime dtEndTime, ref NurDocList lstDocInfos)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetDocInfosOrderByRecordTime(szPatientID, szVisitID, szDocTypeID
                , dtBeginTime, dtEndTime, ref lstDocInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定护理记录ID对应的护理病历列表
        /// </summary>
        /// <param name="szRecordID">护理记录ID</param>
        /// <param name="lstDocInfos">文档信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetRecordDocInfos(string szRecordID, ref NurDocList lstDocInfos)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetRecordDocInfos(szRecordID, ref lstDocInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定文档集中最新的文档版本信息
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <param name="docInfo">文档信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetLatestDocInfo(string szDocSetID, ref NurDocInfo docInfo)
        {
            if (SystemContext.Instance.DocumentAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.DocumentAccess.GetLatestDocInfo(szDocSetID, ref docInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.NO_FOUND;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定ID号的文档对应的文档二进制数据内容
        /// </summary>
        /// <param name="szDocID">文档ID</param>
        /// <param name="byteDocData">二进制数据内容</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocByID(string szDocID, ref byte[] byteDocData)
        {
            return SystemContext.Instance.DocumentAccess.GetDocByID(szDocID, ref byteDocData);
        }

        /// <summary>
        /// 设置指定文档新的状态信息,包括删除状态
        /// </summary>
        /// <param name="newStatus">新的状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SetDocStatusInfo(ref DocStatusInfo newStatus)
        {
            return SystemContext.Instance.DocumentAccess.SetDocStatusInfo(ref newStatus);
        }

        /// <summary>
        /// 根据文档编号获取文档状态
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="docStatusInfo">状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocStatusInfo(string szDocID, ref DocStatusInfo docStatusInfo)
        {
            return SystemContext.Instance.DocumentAccess.GetDocStatusInfo(szDocID, ref docStatusInfo);
        }

        /// <summary>
        /// 修改文档状态
        /// </summary>
        /// <param name="docStatusInfo">文档状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short ModifyOldDocStatus(ref DocStatusInfo docStatusInfo)
        {
            return SystemContext.Instance.DocumentAccess.ModifyOldDocStatusInfo(ref docStatusInfo);
        }

        /// <summary>
        /// 删除文档数据
        /// </summary>
        /// <param name="szDocSetID">文档集ID</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteDocData(string szDocSetID)
        {
            return SystemContext.Instance.DocumentAccess.DeleteDocData(szDocSetID);
        }

        /// <summary>
        /// 删除中间层录入的护理记录单摘要数据
        /// </summary>
        /// <param name="szDocSetID">文档集</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteSummaryData(string szDocSetID)
        {
            return SystemContext.Instance.DocumentAccess.DeleteSummaryData(szDocSetID);
        }

        /// <summary>
        /// 保存一条新的文档索引信息以及文档文件数据
        /// </summary>
        /// <param name="docInfo">文档索引信息</param>
        /// <param name="byteDocData">文档文件数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveDoc(NurDocInfo docInfo, byte[] byteDocData)
        {
            return SystemContext.Instance.DocumentAccess.SaveDoc(docInfo, byteDocData);
        }

        /// <summary>
        /// 更新一条已保存过的文档索引信息以及文档文件数据
        /// </summary>
        /// <param name="szOldDocID">文档旧的ID</param>
        /// <param name="newDocInfo">文档新的索引信息</param>
        /// <param name="szUpdateReason">更新原因</param>
        /// <param name="byteDocData">文档文件数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateDoc(string szOldDocID, NurDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            return SystemContext.Instance.DocumentAccess.UpdateDoc(szOldDocID, newDocInfo, szUpdateReason, byteDocData);
        }

        /// <summary>
        /// 保存或更新一条文档父子关系数据
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="szCaller">调用者</param>
        /// <param name="szChildID">子文档编号</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveChildDocInfo(string szDocID, string szCaller, string szChildID)
        {
            return SystemContext.Instance.DocumentAccess.SaveChildDocInfo(szDocID, szCaller, szChildID);
        }

        /// <summary>
        /// 获取指定文档指定调用者的子文档ID号
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="szCaller">调用者</param>
        /// <param name="szChildID">返回的子文档编号</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetChildDocID(string szDocID, string szCaller, ref string szChildID)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetChildDocID(szDocID, szCaller, ref szChildID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定文档子文档编号
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="lstChilDocID">返回的子文档ID列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetChildDocIDList(string szDocID, ref List<string> lstChilDocID)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetChildDocIDList(szDocID, ref lstChilDocID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定文档子文档信息列表
        /// </summary>
        /// <param name="szDocSetID">文档编号</param>
        /// <param name="lstChilDocs">返回的子文档信息列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetChildDocList(string szDocSetID, ref List<ChildDocInfo> lstChilDocs)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetChildDocList(szDocSetID, ref lstChilDocs);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存或更新一系列摘要数据
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="lstSummaryData">子文档编号</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveSummaryData(string szDocID, List<SummaryData> lstSummaryData)
        {
            return SystemContext.Instance.DocumentAccess.SaveSummaryData(szDocID, lstSummaryData);
        }

        /// <summary>
        /// 获取指定文档集ID号下指定的摘要数据
        /// </summary>
        /// <param name="szDocID">文档集ID号</param>
        /// <param name="szDataName">就诊ID号</param>
        /// <param name="summaryData">返回的摘要数据</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szDocID, string szDataName, ref SummaryData summaryData)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetSummaryData(szDocID, szDataName, ref summaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定病人的文档摘要数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, ref List<SummaryData> lstSummaryData)
        {
            return this.GetSummaryData(szPatientID, szVisitID, DateTime.MinValue, DateTime.MaxValue, ref lstSummaryData);
        }

        /// <summary>
        /// 获取指定病人的文档摘要数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊ID号</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, DateTime dtBegin, DateTime dtEnd, ref List<SummaryData> lstSummaryData)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetSummaryData(szPatientID, szVisitID, dtBegin, dtEnd, ref lstSummaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定文档的摘要数据列表
        /// </summary>
        /// <param name="szDocIDOrRecordID">文档编号</param>
        /// <param name="bIsRecordID">是否是记录ID</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szDocIDOrRecordID, bool bIsRecordID, ref List<SummaryData> lstSummaryData)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetSummaryData(szDocIDOrRecordID, bIsRecordID, ref lstSummaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定文档的摘要数据列表
        /// </summary>
        /// <param name="szDocIDOrRecordID">文档编号</param>
        /// <param name="bIsRecordID">是否是记录ID</param>
        /// <param name="dicSummaryDatas">返回的摘要数据字典</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szDocIDOrRecordID, bool bIsRecordID, ref Dictionary<string, KeyData> dicSummaryDatas)
        {
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            short shRet = SystemContext.Instance.DocumentAccess.GetSummaryData(szDocIDOrRecordID, bIsRecordID, ref lstSummaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return ServerData.ExecuteResult.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            if (lstSummaryData.Count <= 0)
                return ServerData.ExecuteResult.OK;

            foreach (SummaryData SummaryData in lstSummaryData)
            {
                if (string.IsNullOrEmpty(SummaryData.DataName))
                    continue;
                KeyData KeyData = new KeyData();
                KeyData.Category = SummaryData.Category;
                KeyData.Name = SummaryData.DataName;
                KeyData.RecordTime = SummaryData.DataTime;
                KeyData.Remarks = SummaryData.Remarks;
                KeyData.Type = SummaryData.DataType;
                KeyData.Unit = SummaryData.DataUnit;
                KeyData.Value = SummaryData.DataValue;
                if (!dicSummaryDatas.ContainsKey(KeyData.Name))
                {
                    dicSummaryDatas.Add(KeyData.Name, KeyData);
                }
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取指定病人所有摘要数据列表
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">病人就诊号</param>
        /// <param name="szSubID">子ID号</param>
        /// <param name="dtBeginTime">起始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, string szSubID, DateTime dtBeginTime, DateTime dtEndTime, ref List<SummaryData> lstSummaryData)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetSummaryData(szPatientID, szVisitID, szSubID, dtBeginTime, dtEndTime, ref lstSummaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定病人指定文档类型下的同摘要名的所以数据
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">访问次</param>
        /// <param name="szDocTypeId">文档类型Id</param>
        /// <param name="szDataName">摘要名</param>
        /// <param name="lstSummaryData">摘要列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSummaryData(string szPatientID, string szVisitID, string szDocTypeId, string szDataName, ref List<SummaryData> lstSummaryData)
        {
            short shRet = SystemContext.Instance.DocumentAccess.GetSummaryData(szPatientID, szVisitID, szDocTypeId, szDataName, ref lstSummaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
    }
}