// *******************************************************************
// ������ϵͳ��������ڳ���,��������ϵͳ�����û���ʵ�����п���
// ע�⣺�Ƕ��û�����ʵ��,���������û�,��ÿ���û�������һ��ʵ��
// Author : YangMingkun, Date : 2011-10-21
// Copyright : Heren Health Services Co.,Ltd.
// *******************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame;
using Heren.NurDoc.Upgrade;

namespace Heren.NurDoc.Startup
{
    public static class StartupMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            //��д�ı���־
            LogManager.Instance.TextLogOnly = true;
            SystemConfig.Instance.ConfigFile = SystemContext.Instance.ConfigFile;
            MessageBoxEx.Caption = SystemContext.Instance.SystemName;

            string mappingName = SystemConst.MappingName.NURDOC_SYS;
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
            StartupForm.Instance.Show();

            MainFrame mainFrame = new MainFrame();
            StartupForm.Instance.Owner = mainFrame;
            MainWindow mainWindow = new MainWindow(mainFrame);

            IntPtr hMainHandle = mainFrame.Handle;
            if (!fileMapping.WriteHandleValue(hMainHandle))
                goto LABEL_EXIT_APP;

            StartupForm.Instance.Update();
            if (UpgradeHandler.Instance.CheckNewVersion())
            {
                UpgradeHandler.Instance.StartUpgrade(args);
                goto LABEL_EXIT_APP;
            }

            if (args == null || args.Length <= 0)
            {
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() != DialogResult.OK)
                    goto LABEL_EXIT_APP;

                loginForm.Dispose();
                StartupForm.Instance.Update();
                mainFrame.SwitchSystemUser(loginForm.LoginUser, false);
            }
            else
            {
                if (!StartupMain.SendStartupArgs(hMainHandle, args, true))
                    goto LABEL_EXIT_APP;
            }

            StartupForm.Instance.Dispose();
            if (mainFrame == null || mainFrame.IsDisposed)
                Application.Exit();
            else
                Application.Run(mainFrame);

        LABEL_EXIT_APP:
            mainWindow.Dispose();
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
                //�����Ѵ��ڵ�ʵ��
                if (NativeMethods.User32.IsIconic(hMainHandle))
                    NativeMethods.User32.ShowWindow(hMainHandle, NativeConstants.SW_RESTORE);
                NativeMethods.User32.SetForegroundWindow(hMainHandle);

                //��ʵ�����Ͳ�����Ϣ
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
                lParam = StartupArgs.Instance.MakeArgsPtr(args);
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
    }
}