// ***********************************************************
// ������Ӳ���ϵͳ,
// ���ݷ��ʲ�ӿڷ�װ֮�����ʼ���Ϣ���ݷ��ʽӿڷ�װ��
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
        /// ��ȡ�����������ʵ��
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
        /// �����ʼ���Ϣ
        /// </summary>
        /// <param name="qcQuestionInfo">�ʼ���Ϣ</param>
        public short SaveQuestionInfo(QCQuestionInfo qcQuestionInfo)
        {
            return SystemContext.Instance.QcQuestionAccess.SaveQuestionInfo(qcQuestionInfo);
        }

        /// <summary>
        /// ɾ���ʼ���Ϣ
        /// </summary>
        /// <param name="qcQuestionInfo">�ʼ���Ϣ</param>
        /// <returns></returns>
        public short DeleteQuestion(QCQuestionInfo qcQuestionInfo)
        {
            return SystemContext.Instance.QcQuestionAccess.DeleteQuestion(qcQuestionInfo);
        }

        /// <summary>
        /// �����ʿ�ϵͳ,��ȡָ������ָ�������µĲ����ʿ���Ϣ�б�
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="szQuestionStatus">����״̬</param>
        /// <param name="lstQCQuestionInfos">�����ʿ���Ϣ�б�</param>
        /// <returns></returns>
        public short GetQCQuestionList(string szPatientID, string szVisitID, string szQuestionStatus, ref List<QCQuestionInfo> lstQCQuestionInfos)
        {
            short shRet = SystemContext.Instance.QcQuestionAccess.GetQCQuestionList(szPatientID, szVisitID, szQuestionStatus, ref lstQCQuestionInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                shRet = ServerData.ExecuteResult.OK;
            return shRet;
        }
        /// <summary>
        /// �����ʼ���Ϣ
        /// </summary>
        /// <param name="questionInfo">�ʼ���Ϣ</param>
        /// <param name="newIssuedTime">�µ��ʼ�ʱ��</param>
        /// <param name="szOldMsgCode">�ϵ��������</param>
        /// <returns></returns>
        public short UpdateQuestion(QCQuestionInfo questionInfo, DateTime newIssuedTime, string szOldMsgCode)
        {
            return SystemContext.Instance.QcQuestionAccess.UpdateQuestion(questionInfo, newIssuedTime, szOldMsgCode);
        }
    }
}
