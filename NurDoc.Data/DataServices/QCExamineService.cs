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
    public class QCExamineService
    {
        private static QCExamineService m_instance = null;

        /// <summary>
        /// 获取病历指控审核类
        /// </summary>
        public static QCExamineService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new QCExamineService();
                return m_instance;
            }
        }

        private QCExamineService()
        {
        }

        /// <summary>
        /// 根据szQcContentKey，szQcContentType，szPatientID，szVisitID获取质控审核信息
        /// </summary>
        /// <param name="szQcContentKey">内容标识</param>
        /// <param name="szQcContentType">内容类型</param>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">病人VID</param>
        /// <param name="qcExamineInfo">审核类信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfo(string szQcContentKey, string szQcContentType, string szPatientID, string szVisitID, ref QCExamineInfo qcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.GetQcExamineInfo(szQcContentKey, szQcContentType, szPatientID, szVisitID, ref qcExamineInfo);
        }

        /// <summary>
        /// 根据质控审核数据类型查询数据
        /// </summary>
        /// <param name="szQcContentType">质控审核数据类型</param>
        /// <param name="lstQcExamineInfo">质控信息List</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfos(string szQcContentType, ref List<QCExamineInfo> lstQcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.GetQcExamineInfos(szQcContentType, ref lstQcExamineInfo);
        }

                /// <summary>
        /// 根据审核信息类型，病人ID，访问ID获取审核相关信息
        /// </summary>
        /// <param name="szQcContentType">信息类型</param>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">访问ID</param>
        /// <param name="lstQcExamineInfo">审核信息List</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfos(string szQcContentType, string szPatientID, string szVisitID, ref List<QCExamineInfo> lstQcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.GetQcExamineInfos(szQcContentType, szPatientID, szVisitID, ref lstQcExamineInfo);
        }

        /// <summary>
        /// 保存指定的一条质控审核信息
        /// </summary>
        /// <param name="qcExamineInfo">审核信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveQcExamine(QCExamineInfo qcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.SaveQcExamine(qcExamineInfo);
        }

        /// <summary>
        /// 更新指定的一条质控审核信息
        /// </summary>
        /// <param name="qcExamineInfo">审核信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateQcExamine(QCExamineInfo qcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.UpdateQcExamine(qcExamineInfo);
        }
    }
}
