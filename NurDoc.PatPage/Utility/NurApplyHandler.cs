// ***********************************************************
// ������Ӳ���ϵͳ,�����������ݱ�����´�����.
// Creator:OuFengFang  Date:2013-3-26
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage.Utility
{
    internal class NurApplyHandler
    {
        private static NurApplyHandler m_instance = null;

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        public static NurApplyHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NurApplyHandler();
                return m_instance;
            }
        }

        private NurApplyHandler()
        {
        }

        /// <summary>
        /// ����һ���µĻ���������������Ϣ
        /// </summary>
        /// <param name="document">�ĵ���Ϣ��</param>
        /// <returns>�����¼��������Ϣ</returns>
        private NurApplyInfo CreateNurApplyInfo(DocumentInfo document)
        {
            if (document == null || SystemContext.Instance.LoginUser == null)
                return null;

            NurApplyInfo nurApplyInfo = new NurApplyInfo();
            nurApplyInfo.ApplicantID = SystemContext.Instance.LoginUser.ID;
            nurApplyInfo.ApplicantName = SystemContext.Instance.LoginUser.Name;
            nurApplyInfo.ApplyTime = SysTimeService.Instance.Now;
            nurApplyInfo.DocID = document.DocSetID;
            nurApplyInfo.DocTypeID = document.DocTypeID;
            nurApplyInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            nurApplyInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            nurApplyInfo.ModifyTime = nurApplyInfo.ApplyTime;
            nurApplyInfo.PatientID = document.PatientID;
            nurApplyInfo.PatientName = document.PatientName;
            nurApplyInfo.SubID = document.SubID;
            nurApplyInfo.VisitID = document.VisitID;
            nurApplyInfo.WardCode = document.WardCode;
            nurApplyInfo.WardName = document.WardName;
            nurApplyInfo.ApplyName = nurApplyInfo.PatientName + nurApplyInfo.ApplyType;
            nurApplyInfo.ApplyID = nurApplyInfo.MakeApplyID();
            return nurApplyInfo;
        }

        /// <summary>
        /// �޸�ָ���Ĵ����µĻ������뵥��������Ϣ
        /// </summary>
        /// <param name="nurApplyInfo">����������������Ϣ</param>
        /// <returns>����������������Ϣ</returns>
        private NurApplyInfo ModifyNurApplyInfo(NurApplyInfo nurApplyInfo)
        {
            if (nurApplyInfo == null || SystemContext.Instance.LoginUser == null)
                return null;

            nurApplyInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            nurApplyInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            nurApplyInfo.ModifyTime = SysTimeService.Instance.Now;
            return nurApplyInfo;
        }

        /// <summary>
        /// ����һ���µĻ�������״̬��Ϣ
        /// </summary>
        /// <param name="nurApplyInfo">���뵥��Ϣ</param>
        /// <returns>��������״̬��Ϣ��</returns>
        private NurApplyStatusInfo CreateNurApplyStatusInfo(NurApplyInfo nurApplyInfo)
        {
            if (nurApplyInfo == null || SystemContext.Instance.LoginUser == null)
                return null;

            NurApplyStatusInfo nurApplyStatusInfo = new NurApplyStatusInfo();
            nurApplyStatusInfo.ApplyID = nurApplyInfo.ApplyID;
            nurApplyStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
            nurApplyStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
            return nurApplyStatusInfo;
        }

        /// <summary>
        /// ����͸��»���������Ϣ
        /// </summary>
        /// <param name="document">�ĵ���Ϣ��</param>
        /// <param name="bIsNew">�������߸���</param>
        /// <param name="applyData">�ű����ص���������</param>
        /// <returns>�Ƿ�ִ�гɹ�</returns>
        public bool HandleNurApplySave(DocumentInfo document, bool bIsNew, object applyData)
        {
            if (document == null || applyData == null)
                return false;
            NurApplyInfo nurApplyInfo = this.CreateNurApplyInfo(document);
            NurApplyStatusInfo nurApplyStatusInfo = this.CreateNurApplyStatusInfo(nurApplyInfo);
            if (nurApplyInfo == null || nurApplyStatusInfo == null)
                return false;

            if (applyData is DataTable)
            {
                DataTable table = applyData as DataTable;
                if (table == null)
                    return false;
                nurApplyInfo.FromDataRow(table.Rows[0]);
                nurApplyStatusInfo.FromDataRow(table.Rows[0]);
            }
            else
            {
                string[] strParamDatas = applyData.ToString().Split(';');
                if (strParamDatas.Length < 10)
                    return false;

                //����������Ϣ
                if (!bIsNew)
                {
                    nurApplyInfo.ApplyID = strParamDatas[0];
                    nurApplyStatusInfo.ApplyID = strParamDatas[0];
                }
                nurApplyInfo.ApplyType = strParamDatas[1];
                nurApplyInfo.Urgency = GlobalMethods.Convert.StringToValue(strParamDatas[2], 0);
                nurApplyInfo.Status = strParamDatas[7];
                //��������״̬��Ϣ
                nurApplyStatusInfo.NextOperatorID = strParamDatas[3];
                nurApplyStatusInfo.NextOperatorName = strParamDatas[4];
                nurApplyStatusInfo.NextOperatorWardCode = strParamDatas[5];
                nurApplyStatusInfo.NextOperatorWardName = strParamDatas[6];
                nurApplyStatusInfo.Status = strParamDatas[7];
                nurApplyStatusInfo.StatusDesc = strParamDatas[8];
                nurApplyStatusInfo.StatusMessage = strParamDatas[9];
                nurApplyStatusInfo.OperateTime = nurApplyInfo.ModifyTime;
                if (strParamDatas.Length >= 11)
                {
                    DateTime time = DateTime.MinValue;
                    if (DateTime.TryParse(strParamDatas[10], out time))
                        nurApplyInfo.ApplyTime = time;
                }
            }
            if (bIsNew)
            {
                short shRet = NurApplyService.Instance.SaveNurApply(nurApplyInfo, nurApplyStatusInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("����������Ϣ����ʧ��!");
                    return false;
                }
            }
            else
            {
                nurApplyInfo = this.ModifyNurApplyInfo(nurApplyInfo);
                short shRet = NurApplyService.Instance.UpdateNurApply(nurApplyInfo, nurApplyStatusInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("����������Ϣ����ʧ��!");
                    return false;
                }
            }
            return true;
        }
    }
}
