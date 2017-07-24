// ***********************************************************
// 护理电子病历系统升级程序主入口
// Creator:OuFengfang  Date:2012-11-23
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Upgrade
{
    public class Startup
    {
        [STAThread]
        public static void Main(string[] args)
        {
            LogManager.Instance.TextLogOnly = true;
            SystemConfig.Instance.ConfigFile = SystemContext.Instance.ConfigFile;
            MessageBoxEx.Caption = SystemContext.Instance.SystemName;

            FileMapping fileMapping = new FileMapping(SystemConst.MappingName.UPGRADE_SYS);
            if (fileMapping.IsFirstInstance)
            {
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();
                WorkProcess.Instance.Initialize(null, 10, "正在准备升级系统，请稍候...");
                UpgradeHandler.Instance.BeginUpgradeSystem(args.Length > 0 ? args[0] : null);
                fileMapping.Dispose(true);
            }
        }
    }
}
