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

        //���������ļ�
        //  string strLocalFilePath = GlobalMethods.Misc.GetWorkingPath()";
        private bool DownUpgradeFiles(ref string strLocalFilePath)
        {
            //��ȡ�ļ��б�
            string strFTPFolder = "/NurdocUpgrade";
            List<string> lstFilePath = new List<string>();
            short shRet = 0;
            shRet = UpgradeService.Instance.GetFilesFromFTP(strFTPFolder, false, ref lstFilePath);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("�޷���ȡ����������!");
                return false;
            }
            //���س���������
            if (lstFilePath.Count > 0)
            {
                strLocalFilePath = strLocalFilePath + "\\" + GlobalMethods.IO.GetFileName(lstFilePath[0], true);
                shRet = UpgradeService.Instance.DownLoadUpgradeFileFromFTP(lstFilePath[0], strLocalFilePath);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("��������������ʧ��!");
                    return false;
                }
            }
            return true;

        }
        //����������Ӳ���ϵͳ
        private bool Upgrade()
        {
            string strLocalFilePath = GlobalMethods.Misc.GetWorkingPath();
            //1.���������ļ�
            if (!DownUpgradeFiles(ref strLocalFilePath))
            {
                return false;
            }
            RefreshprogressBar(1, 5);
            //2.��ѹ�ļ�
            //��ȡ��ѹ·��
            string unZipDir = GlobalMethods.IO.GetFilePath(strLocalFilePath);
            try
            {
                this.UncompressFile(strLocalFilePath, ref unZipDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show("�ļ���ѹʧ��,ԭ����:" + ex.ToString());
                LogManager.Instance.WriteLog("MainForm.UncompressFile", null, null, "�ļ���ѹʧ��!", ex);
                return false;
            }
            RefreshprogressBar(2, 5);

            //3.ɾ�����ص�ѹ���ļ�
            if (!GlobalMethods.IO.DeleteFile(strLocalFilePath))
            {
                MessageBox.Show("ѹ���ļ�ɾ��ʧ��!");
                return false;
            }
            RefreshprogressBar(3, 5);

            // 4.�����ļ�����������Ӳ���ϵͳ
            //��ȡ������Ӳ���ϵͳ·��
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
                MessageBox.Show("�ļ�����ʧ��,ԭ����:" + ex.ToString());
                LogManager.Instance.WriteLog("MainForm.CopyFolder", null, null, "�ļ�����ʧ��!", ex);
                return false;
            }
            RefreshprogressBar(4, 5);

            //5.ɾ����ѹ�ļ�
            if (!GlobalMethods.IO.DeleteDirectory(unZipDir, true))
            {
                MessageBox.Show("��ѹ�ļ�ɾ��ʧ��!");
                return false;
            }
            RefreshprogressBar(5, 5);
            return true;
        }

        /// <summary>
        /// ��ѹָ����ѹ���ļ�
        /// </summary>
        /// <param name="zipFile">ѹ���ļ�</param>
        /// <returns>��ѹ�Ƿ�ɹ�</returns>
        private void UncompressFile(string zipFile, ref string unZipDir)
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
        /// �����ļ���  
        /// </summary>  
        /// <param name="sourceFolder">�����Ƶ��ļ���</param>  
        /// <param name="destFolder">���Ƶ����ļ���</param>  
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
        /// ����������Ӳ���ϵͳ
        /// </summary>
        public static void StartNurDocSys()
        {
            //��ȡ��ǰ����·�����ϼ�Ŀ¼����Ŀ¼��
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
                MessageBoxEx.Show("�޷�����������Ӳ���ϵͳ!");
                LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, "������Ӳ���ϵͳ����ʧ��!", ex);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            //��������
            if (this.Upgrade())
            {
                //�������汾��Ϣд�뱾�������ļ�
                if (WriteCurrentVerion())
                {
                    //����������Ӳ���ϵͳ
                    StartNurDocSys();
                }
            }
            this.Cursor = Cursors.Default;
            //������������
            this.ExitSystem(new FileMapping("NdsUpgrade"));
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //��������
            this.timer1.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //���ô���ͼ��
            this.Icon = Heren.NurDoc.Upgrade.Properties.Resources.SysIcon;
        }

        //���½�����
        private void RefreshprogressBar(int nPresentStep, int nTotalStep)
        {
            this.progressBar1.Step = int.Parse(Math.Ceiling((double)(nPresentStep * 100 / nTotalStep)).ToString());
            this.progressBar1.PerformStep();
        }

        //�˳�ϵͳ����
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
        //�������汾��Ϣд�뱾�������ļ�
        private bool WriteCurrentVerion()
        {
            string strApplicationUpgradeVersion = string.Empty;
            //��ȡ���������汾��Ϣ
            short shRet = UpgradeService.Instance.GetApplicationUpgrdateVersion(ref strApplicationUpgradeVersion);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("�޷���ȡ���������汾��Ϣ!");
                LogManager.Instance.WriteLog("MainForm.WriteCurrentVerion", null, null, "�޷���ȡ���������汾��Ϣ!");
                return false;
            }
            if (!SystemConfig.Instance.Write(SystemConst.ConfigKey.CURRENT_VERSION, strApplicationUpgradeVersion))
            {
                MessageBoxEx.Show("���������ɹ������ڱ��������ļ�д��汾��Ϣʱ����!");
                LogManager.Instance.WriteLog("MainForm.WriteCurrentVerion", null, null, "���������ɹ������ڱ��������ļ�д��汾��Ϣʱ����!!");
                return false;
            }
            return true;
        }

    }
}