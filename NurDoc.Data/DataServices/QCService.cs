// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之患者满意度记录等表单文档数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Heren.NurDoc.DAL;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.Data
{
    public class QCService
    {
        private static QCService m_instance = null;

        /// <summary>
        /// 获取质量与安全管理记录文档服务实例
        /// </summary>
        public static QCService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new QCService();
                return m_instance;
            }
        }

        private QCService()
        {
        }

        /// <summary>
        /// 获取指定文档集中最新的文档版本信息
        /// </summary>
        /// <param name="szQCDocSetID">文档集ID</param>
        /// <param name="docInfo">文档信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetLatestDocInfo(string szQCDocSetID, ref QCDocInfo docInfo)
        {
            if (SystemContext.Instance.DocumentAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.QCAccess.GetLatestQCDocInfo(szQCDocSetID, ref docInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.NO_FOUND;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定质量与安全管理记录类型代码对应的列表
        /// </summary>
        /// <param name="szCreatorID">创建人ID号</param>
        /// <param name="szQCTypeID">质量与安全管理记录ID号</param>
        /// <param name="dtBeginTime">开始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="lstQCInfos">质量与安全管理记录类型列表</param>
        /// <returns></returns>
        public short GetQCInfos(string szCreatorID, string szQCTypeID, DateTime dtBeginTime, DateTime dtEndTime, ref QCDocList lstQCInfos)
        {
            short shRet = SystemContext.Instance.QCAccess.GetQCInfos(szCreatorID, szQCTypeID, dtBeginTime
                , dtEndTime, ref lstQCInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return shRet;
        }

        /// <summary>
        /// 获取指定质量与安全管理记录类型代码对应的文档基本信息
        /// </summary>
        /// <param name="szCreatorID">创建人ID</param>
        /// <param name="szQCTypeID">质量与安全管理记录ID号</param>
        /// <param name="lstQCInfos">质量与安全管理记录类型列表</param>
        /// <returns></returns>
        public short GetQCDocInfos(string szCreatorID, string szQCTypeID, ref QCDocList lstQCInfos)
        {
            short shRet = SystemContext.Instance.QCAccess.GetQCDocInfos(szCreatorID, szQCTypeID, ref lstQCInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

               /// <summary>
        /// 获取指定文档集中最新的文档版本信息
        /// </summary>
        /// <param name="szQCSetID">文档集ID</param>
        /// <param name="docInfo">文档信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetLatestQCInfo(string szQCSetID, ref QCDocInfo docInfo)
        {
            if (SystemContext.Instance.DocumentAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.QCAccess.GetLatestQCInfo(szQCSetID, ref docInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.NO_FOUND;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定文档ID对应的文档信息
        /// </summary>
        /// <param name="szQCID">文档ID</param>
        /// <param name="docInfo">文档信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetQCInfo(string szQCID, ref QCDocInfo docInfo)
        {
            return SystemContext.Instance.QCAccess.GetQCInfo(szQCID, ref docInfo);
        }

        /// <summary>
        /// 更新一条已保存过的文档索引信息以及文档文件数据
        /// </summary>
        /// <param name="szOldDocID">文档旧的ID</param>
        /// <param name="newDocMInfo">文档新的索引信息</param>
        /// <param name="szUpdateReason">更新原因</param>
        /// <param name="byteDocData">文档文件数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateDoc(string szOldDocID, QCDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            return SystemContext.Instance.QCAccess.UpdateDoc(szOldDocID, newDocInfo, szUpdateReason, byteDocData);
        }

        /// <summary>
        /// 保存或更新一系列摘要数据
        /// </summary>
        /// <param name="szDocID">文档编号</param>
        /// <param name="lstSummaryData">子文档编号</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveQCSummaryData(string szDocID, List<QCSummaryData> lstQCSummaryData)
        {
            return SystemContext.Instance.QCAccess.SaveQCSummaryData(szDocID, lstQCSummaryData);
        }

        /// <summary>
        /// 保存一条新的文档索引信息以及文档文件数据
        /// </summary>
        /// <param name="docInfo">文档索引信息</param>
        /// <param name="byteDocData">文档文件数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveDoc(QCDocInfo docInfo, byte[] byteDocData)
        {
            return SystemContext.Instance.QCAccess.SaveDoc(docInfo, byteDocData);
        }

        /// <summary>
        /// 获取指定ID号的文档对应的文档二进制数据内容
        /// </summary>
        /// <param name="szDocID">文档ID</param>
        /// <param name="byteDocData">二进制数据内容</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocByID(string szDocID, ref byte[] byteDocData)
        {
            return SystemContext.Instance.QCAccess.GetDocByID(szDocID, ref byteDocData);
        }

        /// <summary>
        /// 获取指定文档的摘要数据列表
        /// </summary>
        /// <param name="szDocIDOrRecordID">文档编号</param>
        /// <param name="bIsRecordID">是否是记录ID</param>
        /// <param name="lstSummaryData">返回的摘要数据列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szDocIDOrRecordID, bool bIsRecordID, ref List<QCSummaryData> lstQCSummaryData)
        {
            short shRet = SystemContext.Instance.QCAccess.GetSummaryData(szDocIDOrRecordID, bIsRecordID, ref lstQCSummaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定文档集ID号下指定的摘要数据
        /// </summary>
        /// <param name="szDocID">文档集ID号</param>
        /// <param name="szDataName">就诊ID号</param>
        /// <param name="summaryData">返回的摘要数据</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetSummaryData(string szDocID, string szDataName, ref QCSummaryData qcSummaryData)
        {
            short shRet = SystemContext.Instance.QCAccess.GetSummaryData(szDocID, szDataName, ref qcSummaryData);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 设置指定文档新的状态信息,包括删除状态
        /// </summary>
        /// <param name="newStatus">新的状态信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SetDocStatusInfo(ref DocStatusInfo newStatus)
        {
            return SystemContext.Instance.QCAccess.SetDocStatusInfo(ref newStatus);
        }
    }
}
