using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Heren.Common.Libraries;
using Heren.NurDoc.Data;
using Heren.Common.Controls;
using Heren.Common.ZipLib.Zip;


namespace Heren.NurDoc.Upgrade
{
    public partial class MainForm : HerenForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //下载升级文件
        //  string strLocalFilePath = GlobalMethods.Misc.GetWorkingPath()";
        private bool DownUpgradeFiles(ref string strLocalFilePath)
        {
            //获取文件列表
            string strFTPFolder = "/NurdocUpgrade";
            List<string> lstFilePath = new List<string>();
            short shRet = 0;
            shRet = UpgradeService.Instance.GetFilesFromFTP(strFTPFolder, false, ref lstFilePath);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("无法获取程序升级包!");
                return false;
            }
            //下载程序升级包
            if (lstFilePath.Count > 0)
            {
                strLocalFilePath = strLocalFilePath + "\\" + GlobalMethods.IO.GetFileName(lstFilePath[0], true);
                shRet = UpgradeService.Instance.DownLoadUpgradeFileFromFTP(lstFilePath[0], strLocalFilePath);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("程序升级包下载失败!");
                    return false;
                }
            }
            return true;

        }
        //升级护理电子病历系统
        private bool Upgrade()
        {
            string strLocalFilePath = GlobalMethods.Misc.GetWorkingPath();
            //1.下载升级文件
            if (!DownUpgradeFiles(ref strLocalFilePath))
            {
                return false;
            }
            RefreshprogressBar(1, 5);
            //2.解压文件
            //获取解压路径
            string unZipDir = GlobalMethods.IO.GetFilePath(strLocalFilePath);
            try
            {
                this.UncompressFile(strLocalFilePath, ref unZipDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件解压失败,原因是:" + ex.ToString());
                LogManager.Instance.WriteLog("MainForm.UncompressFile", null, null, "文件解压失败!", ex);
                return false;
            }
            RefreshprogressBar(2, 5);

            //3.删除下载的压缩文件
            if (!GlobalMethods.IO.DeleteFile(strLocalFilePath))
            {
                MessageBox.Show("压缩文件删除失败!");
                return false;
            }
            RefreshprogressBar(3, 5);

            // 4.复制文件升级护理电子病历系统
            //获取护理电子病历系统路径
            string strDesDir = string.Empty;
            DirectoryInfo topDir = Directory.GetParent(strLocalFilePath).Parent;
            if (topDir == null)
                return false;
            strDesDir = topDir.FullName;
            if (GlobalMethods.Misc.IsEmptyString(unZipDir) || GlobalMethods.Misc.IsEmptyString(strDesDir))
                return false;
            try
            {
                CopyFolder(unZipDir, strDesDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件复制失败,原因是:" + ex.ToString());
                LogManager.Instance.WriteLog("MainForm.CopyFolder", null, null, "文件复制失败!", ex);
                return false;
            }
            RefreshprogressBar(4, 5);

            //5.删除解压文件
            if (!GlobalMethods.IO.DeleteDirectory(unZipDir, true))
            {
                MessageBox.Show("解压文件删除失败!");
                return false;
            }
            RefreshprogressBar(5, 5);
            return true;
        }

        /// <summary>
        /// 解压指定的压缩文件
        /// </summary>
        /// <param name="zipFile">压缩文件</param>
        /// <returns>解压是否成功</returns>
        private void UncompressFile(string zipFile, ref string unZipDir)
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
        /// 复制文件夹  
        /// </summary>  
        /// <param name="sourceFolder">待复制的文件夹</param>  
        /// <param name="destFolder">复制到的文件夹</param>  
        private void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                string dest = Path.Combine(destFolder, name);

                File.Copy(file, dest, true);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                if (name.Equals("Skins") || name.Equals("Templets") || name.Equals("UserData") || name.Equals("Upgrade"))
                {
                    if (Directory.Exists(destFolder + "\\" + name))
                    {
                        continue;
                    }
                }
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        /// <summary>
        /// 启动护理电子病历系统
        /// </summary>
        public static void StartNurDocSys()
        {
            //获取当前运行路径的上级目录（父目录）
            string strNurDocSysPath = GlobalMethods.Misc.GetWorkingPath();
            DirectoryInfo topDir = Directory.GetParent(strNurDocSysPath);
            if (topDir != null)
            {
                strNurDocSysPath = topDir.FullName;
            }
            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.FileName = string.Format(@"{0}\NurDoc.exe", strNurDocSysPath);
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("无法启动护理电子病历系统!");
                LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, "护理电子病历系统启动失败!", ex);
            }
        }

        /// <summary>
        /// 升级程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            //程序升级
            if (this.Upgrade())
            {
                //把升级版本信息写入本地配置文件
                if (WriteCurrentVerion())
                {
                    //启动护理电子病历系统
                    StartNurDocSys();
                }
            }
            this.Cursor = Cursors.Default;
            //结束升级程序
            this.ExitSystem(new FileMapping("NdsUpgrade"));
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //启动升级
            this.timer1.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //设置窗体图标
            this.Icon = Heren.NurDoc.Upgrade.Properties.Resources.SysIcon;
        }

        //更新进度条
        private void RefreshprogressBar(int nPresentStep, int nTotalStep)
        {
            this.progressBar1.Step = int.Parse(Math.Ceiling((double)(nPresentStep * 100 / nTotalStep)).ToString());
            this.progressBar1.PerformStep();
        }

        //退出系统处理
        private bool ExitSystem(FileMapping fileMapping)
        {
            if (fileMapping.IsFirstInstance)
            {
                fileMapping.Dispose(false);
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
            if (!NativeMethods.User32.IsWindow(hMainHandle))
            {
                return true;
            }
            return false;
        }
        //把升级版本信息写入本地配置文件
        private bool WriteCurrentVerion()
        {
            string strApplicationUpgradeVersion = string.Empty;
            //获取程序升级版本信息
            short shRet = UpgradeService.Instance.GetApplicationUpgrdateVersion(ref strApplicationUpgradeVersion);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("无法获取程序升级版本信息!");
                LogManager.Instance.WriteLog("MainForm.WriteCurrentVerion", null, null, "无法获取程序升级版本信息!");
                return false;
            }
            if (!SystemConfig.Instance.Write(SystemConst.ConfigKey.CURRENT_VERSION, strApplicationUpgradeVersion))
            {
                MessageBoxEx.Show("程序升级成功，但在本地配置文件写入版本信息时出错!");
                LogManager.Instance.WriteLog("MainForm.WriteCurrentVerion", null, null, "程序升级成功，但在本地配置文件写入版本信息时出错!!");
                return false;
            }
            return true;
        }

    }
}