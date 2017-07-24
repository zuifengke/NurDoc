// **************************************************************
// 护理电子病历系统,本地配置文件中数据库访问参数读写程序主入口.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Heren.NurDoc.DbConfig
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string szCommonDllPath = Application.StartupPath + "\\Common.Libraries.dll";
            if (!System.IO.File.Exists(szCommonDllPath))
            {
                MessageBox.Show(string.Format("未找到“{0}”文件", szCommonDllPath), "配置工具", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new MainForm());
        }
    }
}