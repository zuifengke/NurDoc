// ***********************************************************
// ������Ӳ���ϵͳ,����ƻ����ݱ�����´�����.
// Creator:OuFengFang  Date:2013-4-3
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage.NCP
{
    internal class NurCarePlanHandler
    {
        private static NurCarePlanHandler m_instance = null;

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        public static NurCarePlanHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NurCarePlanHandler();
                return m_instance;
            }
        }

        private NurCarePlanHandler()
        {
        }

        /// <summary>
        /// ����һ���µĻ���ƻ���������Ϣ
        /// </summary>
        /// <param name="document">�ĵ���Ϣ��</param>
        /// <param name="szDiagCode">��ϱ���</param>
        /// <param name="szDiagName">�������</param>
        /// <param name="dtStartTime">��ʼʱ��</param>
        /// <param name="dtEndTime">����ʱ��</param>
        /// <param name="szStatus">״̬��Ϣ</param>
        /// <returns>����ƻ���¼��������Ϣ</returns>
        private NurCarePlanInfo CreateNurCarePlanInfo(DocumentInfo document
            , string szDiagCode, string szDiagName
            , DateTime dtStartTime, DateTime dtEndTime, string szStatus)
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            if (document == null)
                return null;
            NurCarePlanInfo ncpInfo = new NurCarePlanInfo();
            ncpInfo.CreateTime = SysTimeService.Instance.Now;
            ncpInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            ncpInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            ncpInfo.DiagCode = szDiagCode;
            ncpInfo.DiagName = szDiagName;
            ncpInfo.DocID = document.DocSetID;
            ncpInfo.DocTypeID = document.DocTypeID;
            ncpInfo.EndTime = dtEndTime;
            ncpInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            ncpInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            ncpInfo.ModifyTime = ncpInfo.CreateTime;
            ncpInfo.PatientID = document.PatientID;
            ncpInfo.PatientName = document.PatientName;
            ncpInfo.StartTime = dtStartTime;
            ncpInfo.Status = szStatus;
            ncpInfo.SubID = document.SubID;
            ncpInfo.VisitID = document.VisitID;
            ncpInfo.WardCode = document.WardCode;
            ncpInfo.WardName = document.WardName;
            ncpInfo.NCPID = ncpInfo.MakeNCPID();
            return ncpInfo;
        }

        /// <summary>
        /// �޸�ָ���Ĵ����µĻ���ƻ���¼����������Ϣ
        /// </summary>
        /// <param name="ncpInfo">����ƻ���������Ϣ</param>
        /// <param name="szDiagCode">��ϱ���</param>
        /// <param name="szDiagName">�������</param>
        /// <param name="dtStartTime">��ʼʱ��</param>
        /// <param name="dtEndTime">����ʱ��</param>
        /// <param name="szStatus">����ƻ�״̬</param>
        /// <returns>����ƻ���������Ϣ</returns>
        private NurCarePlanInfo ModifyNurCarePlanInfo(NurCarePlanInfo ncpInfo
            , string szDiagCode, string szDiagName, DateTime dtStartTime
            , DateTime dtEndTime, string szStatus)
        {
            if (SystemContext.Instance.LoginUser == null)
                return ncpInfo;
            if (ncpInfo != null)
            {
                ncpInfo.DiagCode = szDiagCode;
                ncpInfo.DiagName = szDiagName;
                ncpInfo.EndTime = dtEndTime; ;
                ncpInfo.StartTime = dtStartTime;
                ncpInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
                ncpInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
                ncpInfo.ModifyTime = SysTimeService.Instance.Now;
                ncpInfo.Status = szStatus;
            }
            return ncpInfo;
        }

        /// <summary>
        /// ����һ���µĻ���ƻ�״̬��Ϣ
        /// </summary>
        /// <param name="ncpInfo">����ƻ���Ϣ</param>
        /// <param name="dtOperateTime">����ʱ��</param>
        /// <param name="szStatus">״̬��Ϣ</param>
        /// <returns>����ƻ�״̬��Ϣ��</returns>
        private NurCarePlanStatusInfo CreateNurCarePlanStatusInfo(NurCarePlanInfo ncpInfo
            , DateTime dtOperateTime, string szStatus)
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            if (ncpInfo == null)
                return null;
            NurCarePlanStatusInfo ncpStatusInfo = new NurCarePlanStatusInfo();
            ncpStatusInfo.NCPID = ncpInfo.NCPID;
            ncpStatusInfo.OperateTime = dtOperateTime;
            ncpStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
            ncpStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
            ncpStatusInfo.Status = szStatus;
            if (SystemContext.Instance.SystemOption.NpcNewModel)
            {
                NurCarePlanConfig config = SystemContext.Instance.GetConfigFromListByStatus(szStatus);
                ncpStatusInfo.StatusDesc = config == null ? string.Empty : config.StatusDesc;
            }
            else
            {
                ncpStatusInfo.StatusDesc = ServerData.NurCarePlanStatus.GetStatusDesc(szStatus);
            }
            return ncpStatusInfo;
        }

        /// <summary>
        /// <summary>
        ///���滤��ƻ��Լ�״̬��Ϣ
        /// </summary>
        /// <param name="document">�ĵ���Ϣ��</param>
        /// <param name="szDiagCode">��ϱ���</param>
        /// <param name="szDiagName">�������</param>
        /// <param name="dtStartTime">��ʼʱ��</param>
        /// <param name="dtEndTime">����ʱ��</param>
        /// <param name="szStatus">״̬��Ϣ</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool HandleNurCarePlanNewSave(DocumentInfo document, string szDiagCode, string szDiagName
            , DateTime dtStartTime, DateTime dtEndTime, string szStatus)
        {
            NurCarePlanInfo ncpInfo = this.CreateNurCarePlanInfo(document
                , szDiagCode, szDiagName, dtStartTime, dtEndTime, szStatus);
            if (ncpInfo == null)
                return false;

            NurCarePlanStatusInfo ncpStatusInfo = this.CreateNurCarePlanStatusInfo(ncpInfo, ncpInfo.CreateTime, szStatus);
            if (ncpStatusInfo == null)
                return false;

            short shRet = CarePlanService.Instance.SaveNurCarePlan(ncpInfo, ncpStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("����ƻ����ݱ���ʧ��!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// <summary>
        ///���»���ƻ��Լ�״̬��Ϣ
        /// </summary>
        /// <param name="document">�ĵ���Ϣ��</param>
        /// <param name="szNCPID">�����¼ID</param>
        /// <param name="szDiagCode">��ϱ���</param>
        /// <param name="szDiagName">�������</param>
        /// <param name="dtStartTime">��ʼʱ��</param>
        /// <param name="dtEndTime">����ʱ��</param>
        /// <param name="szStatus">״̬��Ϣ</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool HandleNurCarePlanModifySave(NurCarePlanInfo ncpInfo, string szDiagCode, string szDiagName
            , DateTime dtStartTime, DateTime dtEndTime, string szStatus)
        {
            ncpInfo = this.ModifyNurCarePlanInfo(ncpInfo, szDiagCode, szDiagName, dtStartTime, dtEndTime, szStatus);
            NurCarePlanStatusInfo ncpStatusInfo = this.CreateNurCarePlanStatusInfo(ncpInfo, ncpInfo.ModifyTime, szStatus);
            if (ncpStatusInfo == null)
                return false;

            short shRet = CarePlanService.Instance.UpdateNurCarePlan(ncpInfo, ncpStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("����ƻ����ݸ���ʧ��!");
                return false;
            }
            return true;
        }
    }
}
