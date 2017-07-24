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
    public class CarePlanService
    {
        private static CarePlanService m_instance = null;

        /// <summary>
        /// ��ȡ����ƻ�����ʵ��
        /// </summary>
        public static CarePlanService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new CarePlanService();
                return m_instance;
            }
        }

        private CarePlanService()
        {
        }

        #region "����ƻ��ֵ�����"
        /// <summary>
        /// ��ѯָ���Ļ�����϶�Ӧ�Ļ���ƻ�,�����ʩ���ֵ�������Ϣ
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <param name="lstNurCarePlanDictInfos">����ƻ�,�����ʩ���ֵ������б�</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanDictInfo(string szDiagCode, ref List<NurCarePlanDictInfo> lstNurCarePlanDictInfos)
        {
            short shRet = SystemContext.Instance.CarePlanAccess.GetNurCarePlanDictInfo(szDiagCode, ref lstNurCarePlanDictInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ɾ��ָ���Ļ�����ϼ����������л���ƻ�,�����ʩ���ֵ�����
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteNurCarePlanDictInfo(string szDiagCode)
        {
            short shRet = SystemContext.Instance.CarePlanAccess.DeleteNurCarePlanDictInfo(szDiagCode);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ɾ��ָ���Ļ�����ϼ�������ָ���Ļ���ƻ�,�����ʩ���ֵ�����
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <param name="szItemCode">����ƻ��ֵ����ݴ���</param>
        /// <param name="szItemType">����ƻ��ֵ���������</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteNurCarePlanDictInfo(string szDiagCode, string szItemCode, string szItemType)
        {
            short shRet = SystemContext.Instance.CarePlanAccess.DeleteNurCarePlanDictInfo(szDiagCode, szItemCode, szItemType);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ����ָ����һ��������϶�Ӧ�Ļ���ƻ��ֵ�����
        /// </summary>
        /// <param name="ncpDictInfo">����ƻ��ֵ�����</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveNurCarePlanDictInfo(NurCarePlanDictInfo ncpDictInfo)
        {
            short shRet = SystemContext.Instance.CarePlanAccess.SaveNurCarePlanDictInfo(ncpDictInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ����ָ���Ļ�����϶�Ӧ��ָ���Ļ���ƻ����ֵ�����
        /// </summary>
        /// <param name="szDiagCode">������ϴ���</param>
        /// <param name="szItemCode">����ƻ��ֵ����ݴ���</param>
        /// <param name="szItemType">����ƻ��ֵ���������</param>
        /// <param name="ncpDictInfo">����ƻ��µ��ֵ�����</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateNurCarePlanDictInfo(string szDiagCode, string szItemCode, string szItemType, NurCarePlanDictInfo ncpDictInfo)
        {
            short shRet = SystemContext.Instance.CarePlanAccess.UpdateNurCarePlanDictInfo(szDiagCode, szItemCode, szItemType, ncpDictInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
        #endregion

        /// <summary>
        /// ���ݼƻ���¼ID����ȡ����ƻ�������Ϣ
        /// </summary>
        /// <param name="szNCPID">����ƻ���¼ID</param>
        /// <param name="ncpInfo">����ƻ���Ϣ��</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanInfo(string szNCPID, ref NurCarePlanInfo ncpInfo)
        {
            return SystemContext.Instance.CarePlanAccess.GetNurCarePlanInfo(szNCPID, ref ncpInfo);
        }

        /// <summary>
        /// ��ȡ���˻���ƻ���¼�����б�
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="szSubID">������ID</param>
        /// <param name="dtBeginTime">��¼ʱ��</param>
        /// <param name="dtEndTime">��¼ʱ��</param>
        /// <param name="lstNurCarePlanInfos">����ƻ���Ϣ�б�</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanInfoList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<NurCarePlanInfo> lstNurCarePlanInfos)
        {
            return SystemContext.Instance.CarePlanAccess.GetNurCarePlanInfoList(szPatientID, szVisitID, szSubID, dtBeginTime, dtEndTime, ref lstNurCarePlanInfos);
        }

        /// <summary>
        /// ���滤��ƻ�
        /// </summary>
        /// <param name="ncpInfo">����ƻ���Ϣ</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveNurCarePlan(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            return SystemContext.Instance.CarePlanAccess.SaveNurCarePlan(ncpInfo, ncpStatusInfo);
        }

        /// <summary>
        /// �������л���ƻ�
        /// </summary>
        /// <param name="ncpInfo">����ƻ���Ϣ</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateNurCarePlan(NurCarePlanInfo ncpInfo, NurCarePlanStatusInfo ncpStatusInfo)
        {
            return SystemContext.Instance.CarePlanAccess.UpdateNurCarePlan(ncpInfo, ncpStatusInfo);
        }

        /// <summary>
        /// ��ȡָ������ƻ���¼ID��ָ������ʱ���Լ�ָ��״̬��Ϣ
        /// </summary>
        /// <param name="szNCPID">����ƻ���¼ID</param>
        /// <param name="szStatus">����ƻ�״̬</param>
        /// <param name="dtOperateTime">����ƻ�״̬����ʱ��</param>
        /// <param name="ncpStatusInfo">����ƻ�״̬��Ϣ��</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurCarePlanStatusInfo(string szNCPID, string szStatus, DateTime dtOperateTime, ref NurCarePlanStatusInfo ncpStatusInfo)
        {
            return SystemContext.Instance.CarePlanAccess.GetNurCarePlanStatusInfo(szNCPID, szStatus, dtOperateTime, ref ncpStatusInfo);
        }
    }
}