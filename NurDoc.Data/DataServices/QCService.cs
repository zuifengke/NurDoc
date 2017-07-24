// ***********************************************************
// ������Ӳ���ϵͳ,
// ���ݷ��ʲ�ӿڷ�װ֮��������ȼ�¼�ȱ��ĵ����ݷ��ʽӿڷ�װ��.
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
        /// ��ȡ�����밲ȫ�����¼�ĵ�����ʵ��
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
        /// ��ȡָ���ĵ��������µ��ĵ��汾��Ϣ
        /// </summary>
        /// <param name="szQCDocSetID">�ĵ���ID</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
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
        /// ��ȡָ�������밲ȫ�����¼���ʹ����Ӧ���б�
        /// </summary>
        /// <param name="szCreatorID">������ID��</param>
        /// <param name="szQCTypeID">�����밲ȫ�����¼ID��</param>
        /// <param name="dtBeginTime">��ʼʱ��</param>
        /// <param name="dtEndTime">����ʱ��</param>
        /// <param name="lstQCInfos">�����밲ȫ�����¼�����б�</param>
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
        /// ��ȡָ�������밲ȫ�����¼���ʹ����Ӧ���ĵ�������Ϣ
        /// </summary>
        /// <param name="szCreatorID">������ID</param>
        /// <param name="szQCTypeID">�����밲ȫ�����¼ID��</param>
        /// <param name="lstQCInfos">�����밲ȫ�����¼�����б�</param>
        /// <returns></returns>
        public short GetQCDocInfos(string szCreatorID, string szQCTypeID, ref QCDocList lstQCInfos)
        {
            short shRet = SystemContext.Instance.QCAccess.GetQCDocInfos(szCreatorID, szQCTypeID, ref lstQCInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

               /// <summary>
        /// ��ȡָ���ĵ��������µ��ĵ��汾��Ϣ
        /// </summary>
        /// <param name="szQCSetID">�ĵ���ID</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
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
        /// ��ȡָ���ĵ�ID��Ӧ���ĵ���Ϣ
        /// </summary>
        /// <param name="szQCID">�ĵ�ID</param>
        /// <param name="docInfo">�ĵ���Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetQCInfo(string szQCID, ref QCDocInfo docInfo)
        {
            return SystemContext.Instance.QCAccess.GetQCInfo(szQCID, ref docInfo);
        }

        /// <summary>
        /// ����һ���ѱ�������ĵ�������Ϣ�Լ��ĵ��ļ�����
        /// </summary>
        /// <param name="szOldDocID">�ĵ��ɵ�ID</param>
        /// <param name="newDocMInfo">�ĵ��µ�������Ϣ</param>
        /// <param name="szUpdateReason">����ԭ��</param>
        /// <param name="byteDocData">�ĵ��ļ�����</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateDoc(string szOldDocID, QCDocInfo newDocInfo, string szUpdateReason, byte[] byteDocData)
        {
            return SystemContext.Instance.QCAccess.UpdateDoc(szOldDocID, newDocInfo, szUpdateReason, byteDocData);
        }

        /// <summary>
        /// ��������һϵ��ժҪ����
        /// </summary>
        /// <param name="szDocID">�ĵ����</param>
        /// <param name="lstSummaryData">���ĵ����</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveQCSummaryData(string szDocID, List<QCSummaryData> lstQCSummaryData)
        {
            return SystemContext.Instance.QCAccess.SaveQCSummaryData(szDocID, lstQCSummaryData);
        }

        /// <summary>
        /// ����һ���µ��ĵ�������Ϣ�Լ��ĵ��ļ�����
        /// </summary>
        /// <param name="docInfo">�ĵ�������Ϣ</param>
        /// <param name="byteDocData">�ĵ��ļ�����</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveDoc(QCDocInfo docInfo, byte[] byteDocData)
        {
            return SystemContext.Instance.QCAccess.SaveDoc(docInfo, byteDocData);
        }

        /// <summary>
        /// ��ȡָ��ID�ŵ��ĵ���Ӧ���ĵ���������������
        /// </summary>
        /// <param name="szDocID">�ĵ�ID</param>
        /// <param name="byteDocData">��������������</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocByID(string szDocID, ref byte[] byteDocData)
        {
            return SystemContext.Instance.QCAccess.GetDocByID(szDocID, ref byteDocData);
        }

        /// <summary>
        /// ��ȡָ���ĵ���ժҪ�����б�
        /// </summary>
        /// <param name="szDocIDOrRecordID">�ĵ����</param>
        /// <param name="bIsRecordID">�Ƿ��Ǽ�¼ID</param>
        /// <param name="lstSummaryData">���ص�ժҪ�����б�</param>
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
        /// ��ȡָ���ĵ���ID����ָ����ժҪ����
        /// </summary>
        /// <param name="szDocID">�ĵ���ID��</param>
        /// <param name="szDataName">����ID��</param>
        /// <param name="summaryData">���ص�ժҪ����</param>
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
        /// ����ָ���ĵ��µ�״̬��Ϣ,����ɾ��״̬
        /// </summary>
        /// <param name="newStatus">�µ�״̬��Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SetDocStatusInfo(ref DocStatusInfo newStatus)
        {
            return SystemContext.Instance.QCAccess.SetDocStatusInfo(ref newStatus);
        }
    }
}
