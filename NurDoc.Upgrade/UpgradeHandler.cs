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
        /// ��ȡ����������ʵ��
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
        /// ��ȡ�����������õĴ������İ汾��
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
                    MessageBoxEx.Show("�°汾���ʧ��,�޷�����ϵͳ,����ϵ����Ա!");
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
        /// �رջ���ĸ�������
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
        /// ����Ƿ����°汾������
        /// </summary>
        /// <returns>�Ƿ�����</returns>
        public bool CheckNewVersion()
        {
            //�����汾��
            if (GlobalMethods.Misc.IsEmptyString(this.UpgradeVersion))
                return false;

            //��ǰ�汾��
            string key = SystemConst.ConfigKey.CURRENT_VERSION;
            string strCurrentVersion = SystemConfig.Instance.Get(key, string.Empty);

            //��������汾ͬ��ǰ��ǰ�汾��Ϣ�����Ͼ�����
            return !strCurrentVersion.Equals(this.UpgradeVersion);
        }

        /// <summary>
        /// �����Զ���������
        /// </summary>
        /// <param name="args">����</param>
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
                LogManager.Instance.WriteLog("UpgradeHandler.StartUpgrade", null, null, "������������ʧ��!", ex);
            }
        }

        internal bool BeginUpgradeSystem(string args)
        {
            //1.����Ӧ�ó���
            WorkProcess.Instance.Show(0, false);
            if (!this.ExitMutexSystem())
            {
                WorkProcess.Instance.Close();
                return false;
            }

            //2.���������ļ�
            string text = string.Format("���������°汾��{0}�������Ժ�...", this.UpgradeVersion);
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

            //3.��ѹ�ļ�
            WorkProcess.Instance.Show("���ڷ��������������Ժ�...", 5, false);
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

            //4.���±��ذ汾��Ϣ
            WorkProcess.Instance.Show("���ڸ��±��ذ汾��Ϣ�����Ժ�...", 8, false);
            string strCurrentVersion = SystemConfig.Instance.Get(SystemConst.ConfigKey.CURRENT_VERSION, string.Empty);
            SystemConfig.Instance.Write(SystemConst.ConfigKey.CURRENT_VERSION, this.UpgradeVersion);
            //string szNewConfigFile = string.Format(@"{0}\NurDocSys.xml", GlobalMethods.Misc.GetWorkingPath());
            //GlobalMethods.IO.CopyFile(SystemConfig.Instance.ConfigFile, szNewConfigFile);
            //SystemConfig.Instance.Write(SystemConst.ConfigKey.CURRENT_VERSION, strCurrentVersion);

            //5.����ѹ�ļ����ǳ����ļ�
            WorkProcess.Instance.Show("���ڸ���Ӧ�ó����ļ������Ժ�...", 10, false);
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
        /// ��ѹָ����ѹ���ļ�
        /// </summary>
        /// <param name="zipFile">ѹ���ļ�</param>
        /// <param name="unZipDir">��ѹĿ¼</param>
        /// <returns>��ѹ�Ƿ�ɹ�</returns>
        private void UncompressFile(string zipFile, string unZipDir)
        {
            if (string.IsNullOrEmpty(zipFile) || !File.Exists(zipFile))
            {
                throw new Exception("ѹ���ļ�������!");
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
        /// �˳�ָ����ϵͳ,�����������
        /// </summary>
        /// <param name="szSystemName">ϵͳ��ʶ</param>
        /// <returns>�Ƿ��˳�</returns>
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
        /// ����Ӧ�ó����ļ�
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
                LogManager.Instance.WriteLog("UpgradeHandler.UpdateAppcalitionFiles", null, null, string.Format("{0}����ʧ��!", SystemContext.Instance.SystemName), ex);
            }
        }
    }
}
