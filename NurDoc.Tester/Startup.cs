// ***********************************************************
// ������Ӳ���ϵͳ�����͵��Գ��������
// Creator:YangMingkun  Date:2012-8-20
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Tester
{
    public class Startup
    {
        [STAThread]
        static void Main(string[] args)
        {
            LogManager.Instance.TextLogOnly = true;
            SystemConfig.Instance.ConfigFile = SystemContext.Instance.ConfigFile;
            MessageBoxEx.Caption = "��ʿ����վϵͳģ�����";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            LogManager.Instance.Dispose();
        }
    }
}
