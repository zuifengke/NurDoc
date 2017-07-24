// ***********************************************************
// ������Ӳ���ϵͳ,
// ���ݷ��ʲ�ӿڷ�װ֮�����������ݷ��ʽӿڷ�װ��.
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
        /// ��ȡ����ָ�������
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
        /// ����szQcContentKey��szQcContentType��szPatientID��szVisitID��ȡ�ʿ������Ϣ
        /// </summary>
        /// <param name="szQcContentKey">���ݱ�ʶ</param>
        /// <param name="szQcContentType">��������</param>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����VID</param>
        /// <param name="qcExamineInfo">�������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfo(string szQcContentKey, string szQcContentType, string szPatientID, string szVisitID, ref QCExamineInfo qcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.GetQcExamineInfo(szQcContentKey, szQcContentType, szPatientID, szVisitID, ref qcExamineInfo);
        }

        /// <summary>
        /// �����ʿ�����������Ͳ�ѯ����
        /// </summary>
        /// <param name="szQcContentType">�ʿ������������</param>
        /// <param name="lstQcExamineInfo">�ʿ���ϢList</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfos(string szQcContentType, ref List<QCExamineInfo> lstQcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.GetQcExamineInfos(szQcContentType, ref lstQcExamineInfo);
        }

                /// <summary>
        /// ���������Ϣ���ͣ�����ID������ID��ȡ��������Ϣ
        /// </summary>
        /// <param name="szQcContentType">��Ϣ����</param>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="lstQcExamineInfo">�����ϢList</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQcExamineInfos(string szQcContentType, string szPatientID, string szVisitID, ref List<QCExamineInfo> lstQcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.GetQcExamineInfos(szQcContentType, szPatientID, szVisitID, ref lstQcExamineInfo);
        }

        /// <summary>
        /// ����ָ����һ���ʿ������Ϣ
        /// </summary>
        /// <param name="qcExamineInfo">�����Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveQcExamine(QCExamineInfo qcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.SaveQcExamine(qcExamineInfo);
        }

        /// <summary>
        /// ����ָ����һ���ʿ������Ϣ
        /// </summary>
        /// <param name="qcExamineInfo">�����Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateQcExamine(QCExamineInfo qcExamineInfo)
        {
            return SystemContext.Instance.QCExamineAccess.UpdateQcExamine(qcExamineInfo);
        }
    }
}
