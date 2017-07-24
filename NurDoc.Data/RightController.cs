// ***********************************************************
// �û�Ȩ�޿�����.�����жϵ�ǰ��¼�û��Ƿ���Ȩ��ʹ��ָ������.
// Creator:YangMingkun  Date:2012-10-31
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class RightController
    {
        private static RightController m_instance = null;

        /// <summary>
        /// ��ȡȨ�޿���������ʵ��
        /// </summary>
        public static RightController Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new RightController();
                return m_instance;
            }
        }

        private RightController()
        {
        }

        private RightConfig m_rightConfig = null;

        /// <summary>
        /// ��ȡ�����������õ��û�Ȩ������
        /// </summary>
        public RightConfig RightConfig
        {
            get
            {
                if (SystemContext.Instance.LoginUser == null)
                    return new RightConfig();
                if (this.m_rightConfig != null)
                    return this.m_rightConfig;

                ConfigService.Instance.GetRightConfig(ref this.m_rightConfig);
                if (this.m_rightConfig == null)
                    return new RightConfig();
                return this.m_rightConfig;
            }
        }

        private NurUserRight m_userRight = null;

        /// <summary>
        /// ��ȡ��ǰ������ϵͳ�û���Ȩ����Ϣ(�ö��󲻻�Ϊ��)
        /// </summary>
        public NurUserRight UserRight
        {
            set
            {
                this.m_userRight = value;
            }

            get
            {
                if (SystemContext.Instance.LoginUser == null)
                    return new NurUserRight();

                string szUserID = SystemContext.Instance.LoginUser.ID;
                if (this.m_userRight != null
                    && this.m_userRight.UserID == szUserID)
                {
                    return this.m_userRight;
                }

                UserRightType rightType = UserRightType.NurDoc;
                UserRightBase userRight = null;
                AccountService.Instance.GetUserRight(szUserID, rightType, ref userRight);
                this.m_userRight = userRight as NurUserRight;
                return (this.m_userRight == null) ? new NurUserRight() : this.m_userRight;
            }
        }

        /// <summary>
        /// ��鵱ǰ�û��Ƿ���ָ���Ĳ����ķ���Ȩ��
        /// </summary>
        /// <param name="szWardCode">��������</param>
        /// <returns>�Ƿ��з���Ȩ��</returns>
        private bool IsAccessibleWard(string szWardCode)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (szWardCode == SystemContext.Instance.LoginUser.WardCode)
                return true;

            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<DeptInfo> lstDeptInfos = SystemContext.Instance.GetUserDeptList(szUserID);
            foreach (DeptInfo deptInfo in lstDeptInfos)
            {
                if (szWardCode == deptInfo.DeptCode)
                    return true;
            }
            return false;
        }

        #region"������¼Ȩ�޿���"

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ��༭��ǰ���˵�������¼
        /// </summary>
        /// <returns>true-�ܴ���;false-���ܴ���</returns>
        public bool CanEditVitalSigns()
        {
            return this.CanEditVitalSigns(PatientTable.Instance.ActivePatient);
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ��༭ָ�����˵�������¼
        /// </summary>
        /// <param name="patVisit">������Ϣ</param>
        /// <returns>true-�ܴ���;false-���ܴ���</returns>
        public bool CanEditVitalSigns(PatVisitInfo patVisit)
        {
            if (SystemContext.Instance.LoginUser == null || patVisit == null)
                return false;

            if (!this.UserRight.EditVitalSigns.Value)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭������¼!");
                return false;
            }
            if (patVisit.WardCode != SystemContext.Instance.LoginUser.WardCode)
            {
                if (!this.UserRight.EditAllVitalSigns.Value)
                {
                    MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭����������������¼!");
                    return false;
                }
            }

            //��Ժ����,��Ҫ����Ժʱ��
            if (patVisit.DischargeTime != patVisit.DefaultTime)
            {
                return this.IsRightEnabled("VITAL_SIGNS", "������¼", string.Empty, patVisit.DischargeTime, "�༭");
            }
            return true;
        }
        #endregion

        #region"�����¼Ȩ�޿���"
        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ�������ǰ���˵Ļ����¼
        /// </summary>
        /// <returns>true-�ܴ���;false-���ܴ���</returns>
        public bool CanCreateNurRec()
        {
            return this.CanCreateNurRec(PatientTable.Instance.ActivePatient);
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ�����ָ�����˵Ļ����¼
        /// </summary>
        /// <param name="patVisit">������Ϣ</param>
        /// <returns>true-�ܴ���;false-���ܴ���</returns>
        public bool CanCreateNurRec(PatVisitInfo patVisit)
        {
            if (SystemContext.Instance.LoginUser == null || patVisit == null)
                return false;

            if (!this.UserRight.CreateNuringRec.Value)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�޴��������¼!");
                return false;
            }
            if (patVisit.WardCode != SystemContext.Instance.LoginUser.WardCode)
            {
                if (this.UserRight.LeaderNurse.Value)
                {
                    return true;
                }
                else if (this.UserRight.EditWardNuringRec.Value)
                {
                    if (!this.IsAccessibleWard(patVisit.WardCode))
                    {
                        MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭���������Ļ����¼!");
                        return false;
                    }
                }
                else if (!this.UserRight.EditAllNuringRec.Value)
                {
                    MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭���������Ļ����¼!");
                    return false;
                }
            }

            //��Ժ����,��Ҫ����Ժʱ��
            if (patVisit.DischargeTime != patVisit.DefaultTime)
            {
                return this.IsRightEnabled("NUR_REC", "�����¼", string.Empty, patVisit.DischargeTime, "�½�");
            }
            return true;
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ��޸�ָ���Ļ����¼
        /// </summary>
        /// <param name="recInfo">�����¼��Ϣ</param>
        /// <returns>true-���޸�;false-�����޸�</returns>
        public bool CanEditNurRec(NursingRecInfo recInfo)
        {
            if (SystemContext.Instance.LoginUser == null || recInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == recInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringRec.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringRec.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(recInfo.WardCode);
            }
            if (!this.UserRight.EditNuringRec.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭��ǰ�����¼!");
                return false;
            }
            return this.IsRightEnabled("NUR_REC", "�����¼", recInfo.RecordID, recInfo.CreateTime, "�༭");
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ�ɾ��ָ���Ļ����¼
        /// </summary>
        /// <param name="recInfo">�����¼��Ϣ</param>
        /// <returns>true-��ɾ��;false-����ɾ��</returns>
        public bool CanDeleteNurRec(NursingRecInfo recInfo)
        {
            if (SystemContext.Instance.LoginUser == null || recInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == recInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringRec.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringRec.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(recInfo.WardCode);
            }
            if (!this.UserRight.DeleteNuringRec.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ��ɾ����ǰ�����¼!");
                return false;
            }
            return this.IsRightEnabled("NUR_REC", "�����¼", recInfo.RecordID, recInfo.CreateTime, "ɾ��");
        }
        #endregion

        #region"��������Ȩ�޿���"
        /// <summary>
        /// �ж��û��Ƿ��д򿪸������ĵ���Ȩ��
        /// </summary>
        /// <param name="htWardDocTable">�ĵ�������������ʱʹ�õ�Hash��</param>
        /// <param name="szDocTypeID">�ĵ�����ID��</param>
        /// <returns>bool</returns>
        public bool CanOpenNurDoc(Dictionary<string, WardDocType> htWardDocTable, string szDocTypeID)
        {
            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<DeptInfo> lstDeptInfos = SystemContext.Instance.GetUserDeptList(szUserID);
            foreach (DeptInfo deptInfo in lstDeptInfos)
            {
                string szWardDocType = string.Concat(szDocTypeID, "_", deptInfo.DeptCode);
                if (htWardDocTable.ContainsKey(szWardDocType))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ�������ǰ���˵Ļ�������
        /// </summary>
        /// <returns>true-�ܴ���;false-���ܴ���</returns>
        public bool CanCreateNurDoc()
        {
            return this.CanCreateNurDoc(PatientTable.Instance.ActivePatient);
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ�����ָ�����˵Ļ�������
        /// </summary>
        /// <param name="patVisit">������Ϣ</param>
        /// <returns>true-�ܴ���;false-���ܴ���</returns>
        public bool CanCreateNurDoc(PatVisitInfo patVisit)
        {
            if (SystemContext.Instance.LoginUser == null || patVisit == null)
                return false;

            if (!this.UserRight.CreateNuringDoc.Value)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�޴�����������!");
                return false;
            }
            if (patVisit.WardCode != SystemContext.Instance.LoginUser.WardCode)
            {
                if (this.UserRight.LeaderNurse.Value)
                {
                    return true;
                }
                else if (this.UserRight.EditWardNuringRec.Value)
                {
                    if (!this.IsAccessibleWard(patVisit.WardCode))
                        return false;
                }
                else if (!this.UserRight.EditAllNuringDoc.Value)
                {
                    MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭���������Ļ�������!");
                    return false;
                }
            }

            //��Ժ����,��Ҫ����Ժʱ��
            if (patVisit.DischargeTime != patVisit.DefaultTime)
            {
                return this.IsRightEnabled("NUR_DOC", "��������", string.Empty, patVisit.DischargeTime, "�½�");
            }
            return true;
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ��޸�ָ���Ļ�������
        /// </summary>
        /// <param name="docInfo">����������Ϣ</param>
        /// <returns>true-���޸�;false-�����޸�</returns>
        public bool CanEditNurDoc(NurDocInfo docInfo)
        {
            if (SystemContext.Instance.LoginUser == null || docInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == docInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringDoc.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringDoc.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(docInfo.WardCode);
            }
            if (!this.UserRight.EditNuringDoc.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭��ǰ��������!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ�ɾ��ָ���Ļ�������
        /// </summary>
        /// <param name="docInfo">����������Ϣ</param>
        /// <returns>true-��ɾ��;false-����ɾ��</returns>
        public bool CanDeleteNurDoc(NurDocInfo docInfo)
        {
            if (SystemContext.Instance.LoginUser == null || docInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == docInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringDoc.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringDoc.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(docInfo.WardCode);
            }
            if (!this.UserRight.DeleteNuringDoc.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ��ɾ����ǰ��������!");
                return false;
            }
            return this.IsRightEnabled("NUR_DOC", "��������", docInfo.DocID, docInfo.DocTime, "ɾ��");
        }

        #endregion

        #region"������Ȩ�޿���"
        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ������µĽ����¼
        /// </summary>
        /// <returns>true-�ܴ���;false-���ܴ���</returns>
        public bool CanCreateShiftRec()
        {
            if (!this.CanEditShiftRec(null))
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�޴��������¼!");
                return false;
            }
            return true;
        }
        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ����ý�����Ŀ���ñ���
        /// </summary>
        /// <returns>true-������;false-��������</returns>
        public bool CanSetItemAlias()
        {
            if (!this.CanEditItemAlias(null))
            {
                MessageBoxEx.ShowWarning("��û��Ȩ��������Ŀ����!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ��޸�ָ���Ľ����¼
        /// </summary>
        /// <param name="shiftInfo">�����¼��Ϣ</param>
        /// <returns>true-���޸�;false-�����޸�</returns>
        public bool CanEditShiftRec(NursingShiftInfo shiftInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            if (shiftInfo == null)
                return this.UserRight.EditShiftRec.Value;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == shiftInfo.CreatorID
                || this.IsAccessibleWard(shiftInfo.WardCode))
            {
                bEditEnabled = true;
            }
            if (!this.UserRight.EditShiftRec.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭��ǰ�����¼!");
                return false;
            }
            return this.IsRightEnabled("SHIFT_REC", "�����¼", shiftInfo.ShiftRecordID, shiftInfo.CreateTime, "�༭");
        }

        /// <summary>
        /// �жϵ�ǰ��¼�û��Ƿ��ܹ��޸�ָ���Ľ�����Ŀ���ñ���
        /// </summary>
        /// <param name="shiftInfo">�����¼��Ϣ</param>
        /// <returns>true-���޸�;false-�����޸�</returns>
        public bool CanEditItemAlias(string szWardCode)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            if (szWardCode == null)
                return this.UserRight.HigherNurse.Value;

            bool bEditEnabled = false;
            if (this.IsAccessibleWard(szWardCode))
            {
                bEditEnabled = true;
            }
            if (!this.UserRight.HigherNurse.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("��û��Ȩ�ޱ༭��ǰ������Ŀ���ñ���!");
                return false;
            }
            return this.IsRightEnabled("SHIFT_ITEMALIAS", "������Ŀ���ñ���", null, DateTime.Now, "�༭");
        }
        #endregion

        /// <summary>
        /// ����Ȩ�޵㲻���õ�ʱ�����Ϣ��
        /// </summary>
        /// <param name="operationType">��������</param>
        /// <param name="hours">���޸ĵ�Сʱ��</param>
        /// <param name="moduleName">ģ������</param>
        private void ShowRightInvalidMessage(string operationType, int hours, string moduleName)
        {
            int timeSpan = hours;
            string spanName = "Сʱ";
            if (hours > 72)
            {
                timeSpan = hours / 24;
                spanName = "��";
            }
            MessageBoxEx.ShowWarningFormat("��û��Ȩ��{0}{1}{2}��ǰ��{3}!", null, operationType
                , timeSpan, spanName, moduleName);
        }

        /// <summary>
        /// �ж�ָ�������ݲ���Ȩ�޵��Ƿ����
        /// </summary>
        /// <param name="moduleCode">ģ�����</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="assignData">��������</param>
        /// <param name="dataTime">����ʱ��</param>
        /// <param name="operationType">��������</param>
        /// <returns>���ݲ���Ȩ�޵��Ƿ����</returns>
        private bool IsRightEnabled(string moduleCode, string moduleName
            , string assignData, DateTime dataTime, string operationType)
        {
            DateTime dtNow = SysTimeService.Instance.Now;
            long span = GlobalMethods.SysTime.DateDiff(DateInterval.Hour, dataTime, dtNow);

            if (this.UserRight.LeaderNurse.Value)
            {
                if (this.RightConfig.LeaderNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.LeaderNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.LeaderNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.HeadNurse.Value)
            {
                if (this.RightConfig.HeadNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.HeadNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.HeadNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.HigherNurse.Value)
            {
                if (this.RightConfig.HigherNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.HigherNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.HigherNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.QualityNurse.Value)
            {
                if (this.RightConfig.QualityNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.QualityNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.QualityNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.GeneralNurse.Value)
            {
                if (this.RightConfig.GeneralNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.GeneralNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.GeneralNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.StudentNurse.Value)
            {
                if (this.RightConfig.StudentNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.StudentNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.StudentNurseEditTime, moduleName);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// �ж��Ƿ�������ʾ���Ϊ��ť
        /// </summary>
        /// <returns></returns>
        public bool CanShowSaveAsButton()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            return this.UserRight.ShowSaveAsButton.Value;
        }
    }
}
