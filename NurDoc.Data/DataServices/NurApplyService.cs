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
    public class NurApplyService
    {
        private static NurApplyService m_instance = null;

        /// <summary>
        /// ��ȡ�����������ʵ��
        /// </summary>
        public static NurApplyService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NurApplyService();
                return m_instance;
            }
        }

        private NurApplyService()
        {
        }

        /// <summary>
        /// �������뵥ID����ȡ���뵥������Ϣ
        /// </summary>
        /// <param name="szApplyID">���뵥���</param>
        /// <param name="nurApplyInfo">���뵥��Ϣ��</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyInfo(string szApplyID, ref NurApplyInfo nurApplyInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyInfo(szApplyID, ref nurApplyInfo);
        }

        /// <summary>
        /// �����ĵ���ID��ȡ���뵥������Ϣ
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID</param>
        /// <param name="nurApplyInfo">���뵥������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurApplyInfoByDocSetID(string szDocSetID, ref NurApplyInfo nurApplyInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyInfoByDocSetID(szDocSetID, ref nurApplyInfo);
        }

        /// <summary>
        /// ���ݻ����ĵ���ID��ȡ������Ϣ
        /// </summary>
        /// <param name="szDocSetID">�ĵ���ID</param>
        /// <param name="NurApplyStatusInfo">��������״̬��Ϣ��</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyStatusInfoByDocSetID(string szDocSetID, ref NurApplyStatusInfo NurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyStatusInfoByDocSetID(szDocSetID, ref NurApplyStatusInfo);
        }

        /// <summary>
        /// ��ȡ�������뵥�����б�
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="szSubID">������ID</param>
        /// <param name="dtBeginTime">��¼ʱ��</param>
        /// <param name="dtEndTime">��¼ʱ��</param>
        ///  <param name="szApplyType">���뵥����</param>
        /// <param name="lstNurApplyInfos">������Ϣ�б�</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyInfoList(string szPatientID, string szVisitID, string szSubID
            , DateTime dtBeginTime, DateTime dtEndTime, string szApplyType, ref List<NurApplyInfo> lstNurApplyInfos)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyInfoList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime,szApplyType, ref lstNurApplyInfos);
        }

        /// <summary>
        /// ���滤������
        /// </summary>
        /// <param name="nurApplyInfo">���뵥��Ϣ</param>
        /// <param name="nurApplyStatusInfo">���뵥״̬��Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveNurApply(NurApplyInfo nurApplyInfo,NurApplyStatusInfo nurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.SaveNurApply(nurApplyInfo, nurApplyStatusInfo);
        }

        /// <summary>
        /// �����������뵥
        /// </summary>
        /// <param name="nurApplyInfo">������Ϣ</param>
        /// <param name="nurApplyStatusInfo">���뵥״̬��Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateNurApply(NurApplyInfo nurApplyInfo,NurApplyStatusInfo nurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.UpdateNurApply(nurApplyInfo, nurApplyStatusInfo);
        }

        /// <summary>
        /// ɾ���������뵥
        /// </summary>
        /// <param name="nurApplyInfo">������Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DelNurApply(NurApplyInfo nurApplyInfo)
        {
            return SystemContext.Instance.NurApplyAccess.DelNurApply(nurApplyInfo);
        }

        /// <summary>
        /// ��ȡָ�����롢ָ������ʱ���Լ�ָ��״̬��Ϣ
        /// </summary>
        /// <param name="szApplyID">���뵥���</param>
        /// <param name="szStatus">���뵥״̬</param>
        /// <param name="dtOperateTime">���뵥״̬����ʱ��</param>
        /// <param name="nurApplyStatusInfo">���뵥״̬��Ϣ��</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetNurApplyStatusInfo(string szApplyID, string szStatus, DateTime dtOperateTime, ref NurApplyStatusInfo nurApplyStatusInfo)
        {
            return SystemContext.Instance.NurApplyAccess.GetNurApplyStatusInfo(szApplyID, szStatus, dtOperateTime, ref nurApplyStatusInfo);
        }
    }
}