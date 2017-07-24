// ***********************************************************
// 护理病历配置管理系统,系统启动入口程序.
// Author : YangMingkun, Date : 2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Config
{
    public static class Startup
    {
        private static FileMapping m_FileMapping = null;

        /// <summary>
        /// 获取或设置内存文件映射对象
        /// </summary>
        private static FileMapping FileMapping
        {
            get { return m_FileMapping; }
            set { m_FileMapping = value; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            LogManager.Instance.TextLogOnly = true;
            SystemConfig.Instance.ConfigFile = SystemContext.Instance.ConfigFile;
            MessageBoxEx.Caption = SystemContext.Instance.SystemName;

            Startup.FileMapping = new FileMapping(SystemConst.MappingName.CONFIG_SYS);
            if (Startup.FileMapping.IsFirstInstance)
            {
                Startup.StartNewInstance(args);
            }
            else
            {
                Startup.HandleRunningInstance(args, 30);
            }
        }

        private static void StartNewInstance(string[] args)
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            MainForm mainForm = new MainForm();
            IntPtr hMainWnd = mainForm.Handle;
            if (!Startup.FileMapping.WriteHandleValue(hMainWnd))
                return;
            mainForm.Show();

            GlobalMethods.UI.SetCursor(mainForm, Cursors.WaitCursor);
            GC.KeepAlive(mainForm);

            if (mainForm == null || mainForm.IsDisposed)
            {
                Application.Exit();
                return;
            }
            GlobalMethods.UI.SetCursor(mainForm, Cursors.Default);
            Application.Run(mainForm);
            Startup.FileMapping.Dispose(true);
        }

        private static void HandleRunningInstance(string[] args, int timeoutSeconds)
        {
            IntPtr hMainWnd = Startup.FileMapping.ReadHandleValue(timeoutSeconds);
            if (hMainWnd == IntPtr.Zero)
                return;
            try
            {
                if (NativeMethods.User32.IsIconic(hMainWnd))
                {
                    NativeMethods.User32.ShowWindow(hMainWnd, NativeConstants.SW_RESTORE);
                }
                NativeMethods.User32.SetForegroundWindow(hMainWnd);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("Program.HandleRunningInstance", ex);
            }
        }
    }
}