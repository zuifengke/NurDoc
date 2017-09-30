// *******************************************************************
// 护理病历系统主窗口消息处理器
// Author : YangMingkun, Date : 2012-8-18
// Copyright : Heren Health Services Co.,Ltd.
// *******************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame;

namespace Heren.NurDoc.Startup
{
    public class MainWindow : NativeWindow, IDisposable
    {
        private bool m_bMessageHandling = false;
        private MainFrame m_mainFrame = null;

        /// <summary>
        /// 用户切换消息常量
        /// </summary>
        const int WM_SWITCHUSER = NativeConstants.WM_USER + 1;

        public MainWindow(MainFrame mainFrame)
        {
            this.m_mainFrame = mainFrame;
            if (this.m_mainFrame == null)
                return;
            if (!this.m_mainFrame.IsHandleCreated)
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
                return this.m_mainFrame.SwitchSystemUser(loginForm.LoginUser, false);
            return false;
        }

        private bool HandleArgsMessage(IntPtr lParam, IntPtr wParam)
        {
            if (this.m_mainFrame == null || !this.m_mainFrame.IsHandleCreated)
                return false;
            try
            {
                StartupArgs.Instance.ParsePtrArgs(lParam);
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
            if (StartupArgs.Instance.UserInfo == null)
                return false;
            
            bool isFirst = (SystemContext.Instance.LoginUser == null);
            if (StartupArgs.Instance.SinglePatMod)
            {
                if (!this.m_mainFrame.SwitchSystemUser(StartupArgs.Instance.UserInfo, true))
                    return false;
            }
            else if (!this.m_mainFrame.SwitchSystemUser(StartupArgs.Instance.UserInfo, false))
                return false;
            
            //打开传入的病人对应的病人窗口
            //this.m_mainFrame.ShowPatientPageForm(StartupArgs.Instance.PatientID, StartupArgs.Instance.VisitID);
            this.m_mainFrame.ShowPatientPageForm(StartupArgs.Instance.PatientID, StartupArgs.Instance.VisitID, StartupArgs.Instance.DoctypeID, StartupArgs.Instance.DocID);
            return true;
        }
    }
}