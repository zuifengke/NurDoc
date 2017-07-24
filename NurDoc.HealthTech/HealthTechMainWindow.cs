using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame;

namespace NurDoc.HealthTech
{
    class HealthTechMainWindow : NativeWindow, IDisposable
    {
        private bool m_bMessageHandling = false;
        private MainForm m_mainFrame = null;

        private IntPtr m_hHostHandle = IntPtr.Zero;

        /// <summary>
        /// ��ȡ���������ϵͳ���
        /// </summary>
        public IntPtr HostHandle
        {
            get { return this.m_hHostHandle; }
        }

        private UserInfo m_userInfo = null;

        /// <summary>
        /// ��ȡ������û�ID
        /// </summary>
        public UserInfo UserInfo
        {
            get { return this.m_userInfo; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// ��ȡ����Ĳ���ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// ��ȡ����Ĳ��˾���ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
        }

        /// <summary>
        /// �û��л���Ϣ����
        /// </summary>
        const int WM_SWITCHUSER = NativeConstants.WM_USER + 1;

        public HealthTechMainWindow(MainForm mainFrame)
        {
            this.m_mainFrame = mainFrame;
            if (this.m_mainFrame == null)
                return;
            if (this.m_mainFrame.IsDisposed)
                return;
            this.AssignHandle(this.m_mainFrame.Handle);
        }

        public void Dispose()
        {
            this.m_mainFrame = null;
            this.DestroyHandle();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SWITCHUSER)
            {
                if (this.m_bMessageHandling)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
                this.m_bMessageHandling = true;
                if (this.SwitchSystemUser())
                    m.Result = (IntPtr)1;
                this.m_bMessageHandling = false;
                return;
            }
            if (m.Msg == NativeConstants.WM_COPYDATA)
            {
                if (this.m_bMessageHandling)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
                this.m_bMessageHandling = true;

                GlobalMethods.UI.SetCursor(this.m_mainFrame, Cursors.WaitCursor);
                if (this.HandleArgsMessage(m.LParam, m.WParam))
                    m.Result = (IntPtr)1;
                GlobalMethods.UI.SetCursor(this.m_mainFrame, Cursors.Default);
                this.m_bMessageHandling = false;
                return;
            }
            base.WndProc(ref m);
        }

        private bool SwitchSystemUser()
        {
            //�����ǰ����ʵ��������,���Ҹ�ʵ����û�е�¼
            if (SystemContext.Instance.LoginUser == null)
                return false;

            LoginForm loginForm = new LoginForm();
            loginForm.ShowInTaskbar = false;
            if (loginForm.ShowDialog() != DialogResult.OK)
                return false;

            loginForm.Dispose();
            if (this.m_mainFrame != null && this.m_mainFrame.IsHandleCreated)
                return this.m_mainFrame.SwitchSystemUser(loginForm.loginUser);
            return false;
        }

        private bool HandleArgsMessage(IntPtr lParam, IntPtr wParam)
        {
            if (this.m_mainFrame == null || !this.m_mainFrame.IsHandleCreated)
                return false;
            try
            {
                ParsePtrArgs(lParam);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return false;
            }

            //�����ǰ����ʵ��������,���Ҹ�ʵ����û�е�¼
            if (wParam == IntPtr.Zero && SystemContext.Instance.LoginUser == null)
                return false;

            //����ϵͳ�û���Ϣ
            if (m_userInfo == null)
                return false;

            bool isFirst = (SystemContext.Instance.LoginUser == null);
            if (!this.m_mainFrame.SwitchSystemUser(m_userInfo))
                return false;
            return true;
        }

        /// <summary>
        /// �����ⲿϵͳ������ϵͳ����������;
        /// �����ɹ�������ñ����еĸ�����
        /// </summary>
        /// <param name="hArgsData">COPYDATASTRUCT�ṹ���</param>
        public void ParsePtrArgs(IntPtr hArgsData)
        {
            this.ParseArrArgs(GlobalMethods.Win32.PtrToString(hArgsData));
        }

        private void ParseArrArgs(params string[] args)
        {
            this.m_hHostHandle = IntPtr.Zero;
            this.m_userInfo = new UserInfo();
            this.m_szPatientID = string.Empty;
            this.m_szVisitID = string.Empty;
            if (args == null || args.Length <= 0)
                throw new Exception("���������������Ϊ��!");

            string szEscapeChar = SystemConst.StartupArgs.ESCAPE_CHAR;
            string szEscapedChar = SystemConst.StartupArgs.ESCAPED_CHAR;
            StringBuilder sbArgsData = new StringBuilder();
            foreach (string arg in args)
                sbArgsData.Append(arg);
            string szArgsData = sbArgsData.ToString().Replace(szEscapeChar, szEscapedChar);

            int nStartIndex = 0;
            int nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex < nStartIndex)
                throw new Exception("���������������Ϊ��!");
            string szHostHandle = szArgsData.Substring(nStartIndex, nFieldIndex);
            this.m_hHostHandle = (IntPtr)GlobalMethods.Convert.StringToValue(szHostHandle, 0);

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.ID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex).ToUpper();
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.ID))
                throw new Exception("�û�ID��������Ϊ��!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.Name = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.Name))
                throw new Exception("�û�������������Ϊ��!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex < nStartIndex)
                throw new Exception("�����봫���������!");
            this.m_userInfo.Password = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.DeptCode = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.DeptCode))
                throw new Exception("�û����Ҵ����������Ϊ��!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.DeptName = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.DeptName))
                throw new Exception("�û��������Ʋ�������Ϊ��!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.WardCode = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.WardCode))
                throw new Exception("�û����������������Ϊ��!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.WardName = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.WardName))
                throw new Exception("�û��������Ʋ�������Ϊ��!");

            nStartIndex = nFieldIndex + 1;
            if (nStartIndex >= szArgsData.Length)
                return;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_szPatientID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_szPatientID))
                throw new Exception("����ID��������Ϊ��!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex) this.m_szVisitID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_szVisitID))
                throw new Exception("���˾���ID��������Ϊ��!");
        }
    }
}
