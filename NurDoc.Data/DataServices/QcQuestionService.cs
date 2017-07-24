// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之护理质检信息数据访问接口封装类
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class QcQuestionService
    {
        private static QcQuestionService m_instance = null;

        /// <summary>
        /// 获取护理申请服务实例
        /// </summary>
        public static QcQuestionService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new QcQuestionService();
                return m_instance;
            }
        }

        /// <summary>
        /// 保存质检信息
        /// </summary>
        /// <param name="qcQuestionInfo">质检信息</param>
        public short SaveQuestionInfo(QCQuestionInfo qcQuestionInfo)
        {
            return SystemContext.Instance.QcQuestionAccess.SaveQuestionInfo(qcQuestionInfo);
        }

        /// <summary>
        /// 删除质检信息
        /// </summary>
        /// <param name="qcQuestionInfo">质检信息</param>
        /// <returns></returns>
        public short DeleteQuestion(QCQuestionInfo qcQuestionInfo)
        {
            return SystemContext.Instance.QcQuestionAccess.DeleteQuestion(qcQuestionInfo);
        }

        /// <summary>
        /// 病历质控系统,获取指定病人指定就诊下的病案质控信息列表
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">就诊ID</param>
        /// <param name="szQuestionStatus">问题状态</param>
        /// <param name="lstQCQuestionInfos">病案质控信息列表</param>
        /// <returns></returns>
        public short GetQCQuestionList(string szPatientID, string szVisitID, string szQuestionStatus, ref List<QCQuestionInfo> lstQCQuestionInfos)
        {
            short shRet = SystemContext.Instance.QcQuestionAccess.GetQCQuestionList(szPatientID, szVisitID, szQuestionStatus, ref lstQCQuestionInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                shRet = ServerData.ExecuteResult.OK;
            return shRet;
        }
        /// <summary>
        /// 更改质检信息
        /// </summary>
        /// <param name="questionInfo">质检信息</param>
        /// <param name="newIssuedTime">新的质检时间</param>
        /// <param name="szOldMsgCode">老的问题代码</param>
        /// <returns></returns>
        public short UpdateQuestion(QCQuestionInfo questionInfo, DateTime newIssuedTime, string szOldMsgCode)
        {
            return SystemContext.Instance.QcQuestionAccess.UpdateQuestion(questionInfo, newIssuedTime, szOldMsgCode);
        }
    }
}
