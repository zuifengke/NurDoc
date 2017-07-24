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
        /// 获取传入的宿主系统句柄
        /// </summary>
        public IntPtr HostHandle
        {
            get { return this.m_hHostHandle; }
        }

        private UserInfo m_userInfo = null;

        /// <summary>
        /// 获取传入的用户ID
        /// </summary>
        public UserInfo UserInfo
        {
            get { return this.m_userInfo; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取传入的病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取传入的病人就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
        }

        /// <summary>
        /// 用户切换消息常量
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
            //如果当前已有实例在运行,并且该实例还没有登录
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

            //如果当前已有实例在运行,并且该实例还没有登录
            if (wParam == IntPtr.Zero && SystemContext.Instance.LoginUser == null)
                return false;

            //更新系统用户信息
            if (m_userInfo == null)
                return false;

            bool isFirst = (SystemContext.Instance.LoginUser == null);
            if (!this.m_mainFrame.SwitchSystemUser(m_userInfo))
                return false;
            return true;
        }

        /// <summary>
        /// 解析外部系统传给本系统的启动参数;
        /// 解析成功后会设置本类中的各属性
        /// </summary>
        /// <param name="hArgsData">COPYDATASTRUCT结构句柄</param>
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
                throw new Exception("您传入的启动参数为空!");

            string szEscapeChar = SystemConst.StartupArgs.ESCAPE_CHAR;
            string szEscapedChar = SystemConst.StartupArgs.ESCAPED_CHAR;
            StringBuilder sbArgsData = new StringBuilder();
            foreach (string arg in args)
                sbArgsData.Append(arg);
            string szArgsData = sbArgsData.ToString().Replace(szEscapeChar, szEscapedChar);

            int nStartIndex = 0;
            int nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex < nStartIndex)
                throw new Exception("宿主句柄参数不能为空!");
            string szHostHandle = szArgsData.Substring(nStartIndex, nFieldIndex);
            this.m_hHostHandle = (IntPtr)GlobalMethods.Convert.StringToValue(szHostHandle, 0);

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.ID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex).ToUpper();
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.ID))
                throw new Exception("用户ID参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.Name = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.Name))
                throw new Exception("用户姓名参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex < nStartIndex)
                throw new Exception("您必须传入密码参数!");
            this.m_userInfo.Password = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.DeptCode = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.DeptCode))
                throw new Exception("用户科室代码参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.DeptName = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.DeptName))
                throw new Exception("用户科室名称参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.WardCode = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.WardCode))
                throw new Exception("用户病区代码参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.WardName = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.WardName))
                throw new Exception("用户病区名称参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            if (nStartIndex >= szArgsData.Length)
                return;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_szPatientID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_szPatientID))
                throw new Exception("病人ID参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex) this.m_szVisitID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_szVisitID))
                throw new Exception("病人就诊ID参数不能为空!");
        }
    }
}
