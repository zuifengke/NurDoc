using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Heren.NurDoc.DAL;
using Heren.NurDoc.PatPage;
using Heren.Common.Libraries;
using Heren.NurDoc.Data;

namespace NurdocControl
{
    [Guid("A6C6DB5B-15B8-3D1E-9E8E-43F17A577B8F")]
    [ComVisible(true)]
    [ToolboxItem(false)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(IAxNurPatControl))]
    public partial class NurPatContrl : UserControl, IObjectSafety, IAxNurPatControl
    {
        public void GetInterfacceSafyOptions(Int32 riid, out Int32 pdwSupportedOptions, out Int32 pdwEnabledOptions)
        {

            // TODO: ��� WebCamControl.GetInterfacceSafyOptions ʵ�� 

            pdwSupportedOptions = 1;

            pdwEnabledOptions = 2;

        }

        public void SetInterfaceSafetyOptions(Int32 riid, Int32 dwOptionsSetMask, Int32 dwEnabledOptions)
        {

            // TODO: ��� WebCamControl.SetInterfaceSafetyOptions ʵ��             

        }

        //#region IObjectSafety Members
        //void IObjectSafety.SetInterfaceSafetyOptions(int riid()
        //    , int dwOptionSetMask, int dwEnabledOptions)
        //{
        //}

        //void IObjectSafety.GetInterfaceSafetyOptions(int riid
        //    , out int pdwSupportedOptions, out int pdwEnabledOptions)
        //{
        //    pdwSupportedOptions = 1;
        //    pdwEnabledOptions = 2;
        //}
        //#endregion

        public NurPatContrl()
        {
            InitializeComponent();
            SystemConfig.Instance.ConfigFile = GlobalMethods.Misc.GetWorkingPath().ToString() + "\\UserData\\NurDocSys.xml";
            //MessageBox.Show(SystemConfig.Instance.ConfigFile);
        }

        public void test()
        {
            MessageBox.Show("���Գɹ����޲�����");
        }

        private void patientPageControl1_Load(object sender, EventArgs e)
        {
            //this.patientPageControl1.PatVisitInfo = new PatVisitInfo();
            //this.patientPageControl1.SetPatientInfoStripVisible(false);
            //this.SwitchPatient("P099953","4");
        }

        private PatVisitInfo m_patientVisit = null;

        /// <summary>
        /// ��ȡ�����õ�ǰ���ڵĲ��˾�����Ϣ
        /// </summary>
        [Browsable(false)]
        public PatVisitInfo PatVisitInfo
        {
            get { return this.m_patientVisit; }
        }

        /// <summary>
        /// ��ȡ��ǰ���������Ƿ��Ѿ����޸�
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get
            {
                if (this.patientPageControl1 == null)
                    return false;
                if (this.patientPageControl1.IsDisposed)
                    return false;
                return this.patientPageControl1.IsModified;
            }
        }

        public void RefreshView()
        {
            //base.RefreshView();
            this.SwitchPatient(this.m_patientVisit);
        }

        public bool CheckModifyState()
        {
            if (this.patientPageControl1 == null
                || this.patientPageControl1.IsDisposed)
            {
                return false;
            }
            return !this.patientPageControl1.SwitchPatient(null);
        }

        /// <summary>
        /// �л���ǰ��������ʾ�Ĳ��˼�������Ϣ
        /// </summary>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public void SwitchPatient(string szPatientID, string szVisitID, string szUserID)
        {
            try
            {
                LogManager.Instance.TextLogOnly = true;
                LogManager.Instance.WriteLog("���յ�PID��" + szPatientID + ";VID:" + szVisitID + ";UserID:" + szUserID);
                if (string.IsNullOrEmpty(szPatientID) || string.IsNullOrEmpty(szVisitID) || string.IsNullOrEmpty(szUserID))
                {
                    MessageBoxEx.Show("���յ��Ĳ����쳣:PID��" + szPatientID + ";VID:" + szVisitID + ";UserID:" + szUserID);
                    return;
                }
                int nRet = ExternService.HerenCert.VerifyUser(
                    "���ʻ�����Ӳ������",
                    SystemContext.Instance.SystemOption.HospitalName,
                    SystemContext.Instance.SystemOption.CertCode);
                if (nRet != 0)
                {
                    MessageBoxEx.Show(string.Format("��ǰ{0}δ����Ȩ���!", SystemContext.Instance.SystemName));
                    this.Dispose();
                    return;
                }

                short shRet = SystemConst.ReturnValue.OK;
                //��ȡ�û�info
                //MessageBox.Show("��ȡ�û�info");
                UserInfo UserInfo = null;
                shRet = AccountService.Instance.GetUserInfo(szUserID, ref UserInfo);
                if (shRet == SystemConst.ReturnValue.FAILED)
                {
                    MessageBoxEx.ShowError(string.Format("��û��Ȩ�޵�¼{0}!", SystemContext.Instance.SystemName));
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                if (UserInfo == null)
                {
                    MessageBoxEx.ShowError(string.Format("��Ǹû�в鵽{0}�û���¼.", szUserID));
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                SystemContext.Instance.LoginUser = UserInfo.Clone() as UserInfo;

                //��ȡ�û�Ȩ��
                //MessageBox.Show("��ȡ�û�Ȩ��");
                UserRightBase userRight = null;
                AccountService.Instance.GetUserRight(SystemContext.Instance.LoginUser.ID, UserRightType.NurDoc, ref userRight);
                NurUserRight nurUserRight = userRight as NurUserRight;
                if (nurUserRight == null || !nurUserRight.NurDocSystem.Value)
                {
                    MessageBoxEx.ShowError(string.Format("��û��Ȩ�޵�¼{0}!", SystemContext.Instance.SystemName));
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }

                //MessageBox.Show(szPatientID + szVisitID);
                PatVisitInfo PatVisitInfo = new PatVisitInfo();
                //MessageBox.Show("��ʼ��ȡ����info");
                shRet = PatVisitService.Instance.GetPatVisitInfo(szPatientID, szVisitID, ref PatVisitInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBox.Show("��ȡ������Ϣ�����г����쳣��");
                    return;
                }
                this.m_patientVisit = null;
                this.Update();
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                //MessageBox.Show(m_patientVisit.PatientID);
                PatientTable.Instance.ActivePatient = PatVisitInfo;
                bool success = this.patientPageControl1.SwitchPatient(PatVisitInfo);
                if (success)
                {
                    this.m_patientVisit = PatVisitInfo;
                    //this.RefreshWindowCaption();
                }

                this.patientPageControl1.SetPatientInfoStripVisible(false);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                this.Dispose();
            }
        }

        /// <summary>
        /// �л���ǰ��������ʾ�Ĳ��˼�������Ϣ
        /// </summary>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool SwitchPatient(PatVisitInfo PatVisitInfo)
        {
            //MessageBox.Show("2");
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //MessageBox.Show("1");
            //MessageBox.Show(PatVisitInfo.PatientID);
            bool success = this.patientPageControl1.SwitchPatient(PatVisitInfo);
            if (success)
            {
                this.m_patientVisit = PatVisitInfo;
                //this.RefreshWindowCaption();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        ///// <summary>
        ///// ���ݵ�ǰ���˾�����Ϣˢ�²��˴��ڱ���
        ///// </summary>
        //private void RefreshWindowCaption()
        //{
        //    if (this.m_patientVisit == null
        //        || this.m_patientVisit.VisitTime == this.m_patientVisit.DefaultTime)
        //    {
        //        this.Text = "�մ��޲���";
        //        return;
        //    }
        //    this.Text = string.Format("{0}({1})"
        //        , this.m_patientVisit.PatientName, this.m_patientVisit.PatientID);
        //    this.TabSubhead = string.Format("��Ժʱ��:{0}"
        //        , this.m_patientVisit.VisitTime.ToString("yyyy��M��d��"));
        //}

        /// <summary>
        /// �л����߲���λ���ĵ�
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">���߾����</param>
        /// <param name="szUserID">�û�ID</param>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="szDocID">�ĵ�ID</param>
        public void OpenNurDoc(string szPatientID, string szVisitID, string szUserID, string szDocTypeID, string szDocID)
        {
            SwitchPatient(szPatientID, szVisitID, szUserID);
            if (string.IsNullOrEmpty(szDocTypeID))
            {
                MessageBoxEx.Show("DocTypeID��������Ϊ��!");
                return;
            }
            LocateToModule(szDocTypeID, szDocID);
            return;
        }

        /// <summary>
        /// ���ݴ���Ĳ���������Ϣ��λ��ָ���Ĳ���ģ��
        /// </summary>
        /// <param name="szDocTypeID">��������ID��</param>
        /// <param name="szDocID">��д����ID��</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            bool success = this.patientPageControl1.LocateToModule(szDocTypeID, szDocID);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        /// <summary>
        /// ���ݴ���Ĳ���������Ϣ��λ��ָ���Ĳ���ģ��
        /// </summary>
        /// <param name="szDocTypeID">��������ID��</param>
        /// <param name="szDocID">��д����ID��</param>
        /// <param name="bshowList">�Ƿ���������б�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID, bool bshowList)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            bool success = this.patientPageControl1.LocateToModule(szDocTypeID, szDocID, bshowList);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        private void PatientPageControl_PatientInfoChanged(object sender, EventArgs e)
        {
            this.m_patientVisit = this.patientPageControl1.PatVisitInfo;
            //this.RefreshWindowCaption();
        }

        private void PatientPageControl_PatientInfoChanging(object sender, PatientInfoChangingEventArgs e)
        {
            //PatientPageForm patientPageForm = this.MainFrame.GetPatientPageForm(e.NewPatVisit);
            //if (patientPageForm != null && patientPageForm != this)
            //{
            //    e.Cancel = true;
            //    patientPageForm.DockHandler.Activate();
            //}
        }

    }
}