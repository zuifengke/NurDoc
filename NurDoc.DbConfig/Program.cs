// **************************************************************
// ������Ӳ���ϵͳ,���������ļ������ݿ���ʲ�����д���������.
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
                MessageBox.Show(string.Format("δ�ҵ���{0}���ļ�", szCommonDllPath), "���ù���", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new MainForm());
        }
    }
}