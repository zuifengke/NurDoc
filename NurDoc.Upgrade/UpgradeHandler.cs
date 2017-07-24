using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Heren.Common.Libraries;
using Heren.Common.ZipLib.Zip;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Upgrade
{
    public class UpgradeHandler
    {
        private static UpgradeHandler m_instance = null;

        /// <summary>
        /// 获取升级处理器实例
        /// </summary>
        public static UpgradeHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new UpgradeHandler();
                return m_instance;
            }
        }

        private UpgradeHandler()
        {
        }

        private string m_upgradeVersion = null;

        /// <summary>
        /// 获取服务器上配置的待升级的版本号
        /// </summary>
        public string UpgradeVersion
        {
            get
            {
                if (this.m_upgradeVersion != null)
                    return this.m_upgradeVersion;
                string szUpgradeVersion = null;
                short shRet = UpgradeService.Instance.GetUpgradeVersion(ref szUpgradeVersion);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("新版本检测失败,无法升级系统,请联系管理员!");
                    return string.Empty;
                }
                this.m_upgradeVersion = (szUpgradeVersion == null) ? string.Empty : szUpgradeVersion;
                return this.m_upgradeVersion;
            }
        }

        private string[] m_mutexSystemList = new string[] { 
            SystemConst.MappingName.CONFIG_SYS, 
            SystemConst.MappingName.NURDOC_SYS 
        };

        /// <summary>
        /// 关闭互斥的各个程序
        /// </summary>
        /// <returns>bool</returns>
        private bool ExitMutexSystem()
        {
            if (this.m_mutexSystemList == null)
                return true;
            foreach (string system in this.m_mutexSystemList)
            {
                if (!this.ExitSystem(system))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 检测是否有新版本待升级
        /// </summary>
        /// <returns>是否升级</returns>
        public bool CheckNewVersion()
        {
            //升级版本号
            if (GlobalMethods.Misc.IsEmptyString(this.UpgradeVersion))
                return false;

            //当前版本号
            string key = SystemConst.ConfigKey.CURRENT_VERSION;
            string strCurrentVersion = SystemConfig.Instance.Get(key, string.Empty);

            //如果升级版本同当前当前版本信息不符合就升级
            return !strCurrentVersion.Equals(this.UpgradeVersion);
        }

        /// <summary>
        /// 启动自动升级程序
        /// </summary>
        /// <param name="args">参数</param>
        public void StartUpgrade(string[] args)
        {
            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.FileName = string.Format(@"{0}\NdsUpgrade.exe", GlobalMethods.Misc.GetWorkingPath());
            if (args.Length > 0)
                proc.StartInfo.Arguments = args[0];
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("UpgradeHandler.StartUpgrade", null, null, "升级程序启动失败!", ex);
            }
        }

        internal bool BeginUpgradeSystem(string args)
        {
            //1.结束应用程序
            WorkProcess.Instance.Show(0, false);
            if (!this.ExitMutexSystem())
            {
                WorkProcess.Instance.Close();
                return false;
            }

            //2.下载升级文件
            string text = string.Format("正在下载新版本“{0}”，请稍候...", this.UpgradeVersion);
            WorkProcess.Instance.Show(text, 2, false);
            if (GlobalMethods.Misc.IsEmptyString(this.UpgradeVersion))
            {
                WorkProcess.Instance.Close();
                return false;
            }
            string strRemoteFile = string.Format("/UPGRADE/{0}.zip", this.UpgradeVersion);
            string strLocalFile = string.Format(@"{0}\Upgrade\{1}.zip"
                , GlobalMethods.Misc.GetWorkingPath(), this.UpgradeVersion);
            short shRet = UpgradeService.Instance.DownloadUpgradeFile(strRemoteFile, strLocalFile);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                WorkProcess.Instance.Close();
                return false;
            }

            //3.解压文件
            WorkProcess.Instance.Show("正在分析升级包，请稍候...", 5, false);
            string unZipDir = GlobalMethods.IO.GetFilePath(strLocalFile);
            try
            {
                this.UncompressFile(strLocalFile, unZipDir);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("UpgradeHandler.UncompressFile", null, null, ex);
                WorkProcess.Instance.Close();
                return false;
            }

            //4.更新本地版本信息
            WorkProcess.Instance.Show("正在更新本地版本信息，请稍候...", 8, false);
            string strCurrentVersion = SystemConfig.Instance.Get(SystemConst.ConfigKey.CURRENT_VERSION, string.Empty);
            SystemConfig.Instance.Write(SystemConst.ConfigKey.CURRENT_VERSION, this.UpgradeVersion);
            //string szNewConfigFile = string.Format(@"{0}\NurDocSys.xml", GlobalMethods.Misc.GetWorkingPath());
            //GlobalMethods.IO.CopyFile(SystemConfig.Instance.ConfigFile, szNewConfigFile);
            //SystemConfig.Instance.Write(SystemConst.ConfigKey.CURRENT_VERSION, strCurrentVersion);

            //5.将解压文件覆盖程序文件
            WorkProcess.Instance.Show("正在更新应用程序文件，请稍候...", 10, false);
            if (!this.ExitMutexSystem())
            {
                WorkProcess.Instance.Close();
                return false;
            }
            WorkProcess.Instance.Close();
            this.UpdateAppcalitionFiles(args);
            return true;
        }

        /// <summary>
        /// 解压指定的压缩文件
        /// </summary>
        /// <param name="zipFile">压缩文件</param>
        /// <param name="unZipDir">解压目录</param>
        /// <returns>解压是否成功</returns>
        private void UncompressFile(string zipFile, string unZipDir)
        {
            if (string.IsNullOrEmpty(zipFile) || !File.Exists(zipFile))
            {
                throw new Exception("压缩文件不存在!");
            }
            ZipInputStream zipInputStream = null;
            FileStream zipFileStream = null;
            FileStream streamWriter = null;
            try
            {
                zipFileStream = File.OpenRead(zipFile);
                zipInputStream = new ZipInputStream(zipFileStream);
                ZipEntry zipEntry = null;
                while ((zipEntry = zipInputStream.GetNextEntry()) != null)
                {
                    if (zipEntry.IsDirectory)
                    {
                        Directory.CreateDirectory(unZipDir + "\\" + zipEntry.Name);
                        continue;
                    }
                    string entryFile = unZipDir + "\\" + zipEntry.Name;
                    streamWriter = File.Create(entryFile);
                    int size = 0;
                    byte[] data = new byte[2048];
                    do
                    {
                        size = zipInputStream.Read(data, 0, data.Length);
                        if (size > 0) streamWriter.Write(data, 0, size);
                    } while (size > 0);
                    streamWriter.Flush();
                    streamWriter.Dispose();
                }
                unZipDir = unZipDir + "\\" + GlobalMethods.IO.GetFileName(zipFile, false);
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (streamWriter != null) streamWriter.Dispose();
                if (zipFileStream != null) zipFileStream.Close();
                if (zipFileStream != null) zipFileStream.Dispose();
                if (zipInputStream != null) zipInputStream.Close();
                if (zipInputStream != null) zipInputStream.Dispose();
            }
        }

        /// <summary>
        /// 退出指定的系统,如果正在运行
        /// </summary>
        /// <param name="szSystemName">系统标识</param>
        /// <returns>是否退出</returns>
        private bool ExitSystem(string szSystemName)
        {
            FileMapping fileMapping = new FileMapping(szSystemName);
            if (fileMapping.IsFirstInstance)
            {
                fileMapping.Dispose(true);
                return true;
            }
            IntPtr hMainHandle = fileMapping.ReadHandleValue(1);
            fileMapping.Dispose(false);
            if (!NativeMethods.User32.IsWindow(hMainHandle))
            {
                return true;
            }
            if (NativeMethods.User32.IsIconic(hMainHandle))
                NativeMethods.User32.ShowWindow(hMainHandle, NativeConstants.SW_RESTORE);
            NativeMethods.User32.SetForegroundWindow(hMainHandle);
            NativeMethods.User32.SendMessage(hMainHandle, NativeConstants.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            return !(NativeMethods.User32.IsWindow(hMainHandle));
        }

        /// <summary>
        /// 更新应用程序文件
        /// </summary>
        private void UpdateAppcalitionFiles(string args)
        {
            string szUpgradeBatText = Properties.Resources.ResourceManager.GetObject("Upgrade") as string;
            string szUpgradeBatFile = string.Format(@"{0}\AutoUpgrade.bat", GlobalMethods.Misc.GetWorkingPath());
            szUpgradeBatText = GlobalMethods.Convert.ReplaceText(szUpgradeBatText
                , new string[] { "{app_path}", "{app_version}", "{app_args}" }
                , new string[] { GlobalMethods.Misc.GetWorkingPath(), this.UpgradeVersion, args });
            if (!GlobalMethods.IO.WriteFileText(szUpgradeBatFile, szUpgradeBatText))
                return;
            try
            {
                Process.Start(szUpgradeBatFile);
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("UpgradeHandler.UpdateAppcalitionFiles", null, null, string.Format("{0}升级失败!", SystemContext.Instance.SystemName), ex);
            }
        }
    }
}
