using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame;

namespace NurDoc.HealthTech
{
    static class StartupMain
    {
        ///// <summary>
        ///// 应用程序的主入口点。
        ///// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new LoginForm());
        //}

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            //仅写文本日志
            LogManager.Instance.TextLogOnly = true;
            MessageBoxEx.Caption = SystemContext.Instance.SystemName;
            SystemConfig.Instance.ConfigFile = SystemContext.Instance.ConfigFile;

            string mappingName = SystemConst.MappingName.HEALTH_TECH_SYS;
            FileMapping fileMapping = new FileMapping(mappingName);
            if (fileMapping.IsFirstInstance)
            {
                StartupMain.StartNewInstance(args, fileMapping);
            }
            else
            {
                StartupMain.HandleRunningInstance(args, fileMapping);
            }
        }

        private static void StartNewInstance(string[] args, FileMapping fileMapping)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //StartupForm.Instance.Show();

            MainForm mainFrame = new MainForm();
            //StartupForm.Instance.Owner = mainFrame;
            HealthTechMainWindow HealthTechMainWindow = new HealthTechMainWindow(mainFrame);

            IntPtr hMainHandle = mainFrame.Handle;
            if (!fileMapping.WriteHandleValue(hMainHandle))
                goto LABEL_EXIT_APP;

            //StartupForm.Instance.Update();
            //if (UpgradeHandler.Instance.CheckNewVersion())
            //{
            //    UpgradeHandler.Instance.StartUpgrade(args);
            //    goto LABEL_EXIT_APP;
            //}

            if (args == null || args.Length <= 0)
            {
                LoginForm loginForm = new LoginForm();

                IntPtr hloginWnd = loginForm.Handle;

                if (!fileMapping.WriteHandleValue(hloginWnd))
                    return;

                if (loginForm.ShowDialog() != DialogResult.OK)
                    goto LABEL_EXIT_APP;

                loginForm.Dispose();
                mainFrame.loginUser = loginForm.loginUser;
                if (!fileMapping.WriteHandleValue(hMainHandle))
                    goto LABEL_EXIT_APP;

                //StartupForm.Instance.Update();
                //mainFrame.SwitchSystemUser(LoginForm.LoginUser);
            }
            else
            {
                if (!StartupMain.SendStartupArgs(hMainHandle, args, true))
                    goto LABEL_EXIT_APP;
            }

            //StartupForm.Instance.Dispose();
            if (mainFrame == null || mainFrame.IsDisposed)
                Application.Exit();
            else
                Application.Run(mainFrame);

        LABEL_EXIT_APP:
            HealthTechMainWindow.Dispose();
            mainFrame.Dispose();
            fileMapping.Dispose(true);
            LogManager.Instance.Dispose();
        }

        private static void HandleRunningInstance(string[] args, FileMapping fileMapping)
        {
            IntPtr hMainHandle = fileMapping.ReadHandleValue(30);
            fileMapping.Dispose(false);
            if (hMainHandle == IntPtr.Zero)
                return;
            try
            {
                //激活已存在的实例
                if (NativeMethods.User32.IsIconic(hMainHandle))
                    NativeMethods.User32.ShowWindow(hMainHandle, NativeConstants.SW_RESTORE);
                bool a = NativeMethods.User32.SetForegroundWindow(hMainHandle);

                //向实例发送参数消息
                StartupMain.SendStartupArgs(hMainHandle, args, false);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("StartupMain.HandleRunningInstance", ex);
            }
        }

        private static bool SendStartupArgs(IntPtr hMainHandle, string[] args, bool isFirstIntance)
        {
            IntPtr lParam = IntPtr.Zero;
            try
            {
                lParam = MakeArgsPtr(args);// .. StartupArgs.Instance.MakeArgsPtr(args);
                if (lParam == IntPtr.Zero)
                    return false;
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("StartupMain.SendStartupArgs", ex);
            }
            IntPtr wParam = (IntPtr)(isFirstIntance ? 1 : 0);
            int nRet = NativeMethods.User32.SendMessage(hMainHandle, NativeConstants.WM_COPYDATA, wParam, lParam);
            return (nRet == 1) ? true : false;
        }

        /// <summary>
        /// 将指定的启动参数解析为所在内存地址
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns>IntPtr</returns>
        private static IntPtr MakeArgsPtr(params string[] args)
        {
            if (args == null || args.Length <= 0)
                return IntPtr.Zero;

            StringBuilder sbArgsData = new StringBuilder();
            foreach (string arg in args)
            {
                sbArgsData.Append(arg);
            }
            return GlobalMethods.Win32.StringToPtr(sbArgsData.ToString());
        }
    }
}