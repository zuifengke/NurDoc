// ***********************************************************
// ������Ӳ���ϵͳ,�����ڶ���ϵͳ��ʼҳģ��.
// Creator:YangMingkun  Date:2013-4-26
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame.Dialogs;
using Heren.NurDoc.Utilities;
using System.IO;
using System.Net;
using System.Threading;

namespace Heren.NurDoc.Frame.Controls
{
    internal partial class NursingHomePage : UserControl
    {
        private MainFrame m_mainFrame = null;

        /// <summary>
        /// ��ȡ�����������������򴰿�
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("��ȡ�����������������򴰿�")]
        public MainFrame MainFrame
        {
            get
            {
                if (this.m_mainFrame == null)
                    return null;
                if (this.m_mainFrame.IsDisposed)
                    return null;
                return this.m_mainFrame;
            }

            set
            {
                this.m_mainFrame = value;
            }
        }

        private Image m_logoIcon = null;

        /// <summary>
        /// ��ȡ��˾Logoͼ��
        /// </summary>
        [Browsable(false)]
        public Image LogoIcon
        {
            get
            {
                if (this.m_logoIcon == null)
                    this.m_logoIcon = Properties.Resources.Logo;
                return this.m_logoIcon;
            }
        }

        public NursingHomePage()
        {
            this.InitializeComponent();
            SystemContext.Instance.DisplayDeptChanged +=
                new EventHandler(this.System_DisplayDeptChanged);
        }

        private bool m_bSinglePatMod = false;

        /// <summary>
        /// ��ȡ�����õ�����ģʽ
        /// </summary>
        public bool SinglePatMod
        {
            get { return this.m_bSinglePatMod; }
            set { this.m_bSinglePatMod = value; }
        }

        public void Initialize()
        {
            if (this.MainFrame == null || !this.IsHandleCreated)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadHomePageTemplet();
            Application.DoEvents();

            object data = this.formControl1.GetFormData("��ɫ��");
            PatientTable.Instance.ColorTable = data as Hashtable;

            data = this.formControl1.GetFormData("�������ͱ�");
            SystemContext.Instance.TempTypeList = data as string[];

            data = this.formControl1.GetFormData("������ɫ��");
            SystemContext.Instance.PatientCardColorList = data as Dictionary<string, Color>;

            data = this.formControl1.GetFormData("����ʱ����");
            SystemContext.Instance.TimePointTable = data as Hashtable;

            data = this.formControl1.GetFormData("��׼��Ѫѹʱ���");
            SystemContext.Instance.StandardBPTimePoint = data as int[];

            data = this.formControl1.GetFormData("����ӳ���");
            SystemContext.Instance.VitalSignsNameTable = data as Hashtable;

            data = this.formControl1.GetFormData("����ֵӳ���");
            SystemContext.Instance.VitalSignsValueTable = data as Hashtable;

            data = this.formControl1.GetFormData("ҽ�����ӳ���");
            SystemContext.Instance.OrderCategoryTable = data as Hashtable;

            data = this.formControl1.GetFormData("ҽ��;��ӳ���");
            SystemContext.Instance.OrderWayValueTable = data == null ? null : data as Hashtable;

            data = this.formControl1.GetFormData("Һ��ҽ����");
            SystemContext.Instance.LiquidOrderList = data as string[];

            data = this.formControl1.GetFormData("�������ݲ���ʱ���");
            SystemContext.Instance.VitalSignNoTimePoint = data as string;

            data = this.formControl1.GetFormData("¼������ʱ���һ��");
            SystemContext.Instance.VitalSignTimeBeforeOneDay = data as string;

            data = this.formControl1.GetFormData("�����¼С�����Ʊ�");
            SystemContext.Instance.SummaryNameList = data as string[];

            data = this.formControl1.GetFormData("���Ӱ���Ŀ����");
            SystemContext.Instance.ShiftItemComparer = data as string;

            data = this.formControl1.GetFormData("����ƻ����ð�ť");
            SystemContext.Instance.NCPButtonConfigTable = SystemContext.Instance.GetNursCarPlanConfigs(data as DataTable);

            data = this.formControl1.GetFormData("�����б�");
            PatientTable.Instance.RefreshDeptPatientList(data as DataTable);

            data = this.formControl1.GetFormData("�����б�");
            SystemContext.Instance.DeptTable = data as DataTable;

            if (this.m_bSinglePatMod)
                this.formControl1.UpdateFormData("������ģʽ", true);
            else
                this.formControl1.UpdateFormData("������ģʽ", false);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void System_DisplayDeptChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            //�����ұ��л���,֪ͨ��ȥˢ����ͼ
            this.formControl1.UpdateFormData("ˢ����ͼ", null);

            //�����ұ��л���,���õ�ǰ���Ҳ����б�
            object data = this.formControl1.GetFormData("�����б�");
            PatientTable.Instance.RefreshDeptPatientList(data as DataTable);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void LoadHomePageTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_HOME_PAGE;
            DocTypeInfo docInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            if (docInfo == null || GlobalMethods.Misc.IsEmptyString(docInfo.DocTypeID))
            {
                MessageBoxEx.ShowError("ϵͳ��ʼҳ������ʧ�ܣ�");
                return;
            }
            byte[] byteTempletData = null;
            if (!FormCache.Instance.GetFormTemplet(docInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("ϵͳ��ʼҳ����������ʧ�ܣ�");
                return;
            }
            this.formControl1.Load(byteTempletData);
            if (this.formControl1.Controls.Count > 0)
            {
                Control control = this.formControl1.Controls[0];
                if (control.Height > 0)
                    this.Height = control.Height;
                control.Dock = DockStyle.Fill;
                control.MouseClick += new MouseEventHandler(this.formControl1_MouseClick);
            }
        }

        private void DrawCompanyLogo(Graphics graphics)
        {
            if (SkinService.Instance.ToolStripSkin.ShowCompanyLogo && this.LogoIcon != null)
            {
                if (this.Width <= 0 || this.Height <= 0)
                    return;
                if (this.Height < this.LogoIcon.Height)
                    return;
                int nLeft = this.Width - this.LogoIcon.Width - 8;
                graphics.DrawImage(this.LogoIcon, nLeft, (this.Height - this.LogoIcon.Height) / 2);
            }
        }

        private void DrawBackground(Graphics graphics)
        {
            Rectangle clipRect = this.ClientRectangle;
            if (clipRect.Width <= 0 || clipRect.Height <= 0)
                return;
            Color beginColor = SkinService.Instance.ToolStripSkin.PaneBackgroundGradient.StartColor;
            Color endColor = SkinService.Instance.ToolStripSkin.PaneBackgroundGradient.EndColor;
            LinearGradientMode gradientMode = SkinService.Instance.ToolStripSkin.PaneBackgroundGradient.GradientMode;
            LinearGradientBrush brush = new LinearGradientBrush(clipRect, beginColor, endColor, gradientMode);
            graphics.FillRectangle(brush, clipRect);
            brush.Dispose();
        }

        private void formControl1_Paint(object sender, PaintEventArgs e)
        {
            this.DrawBackground(e.Graphics);
            this.DrawCompanyLogo(e.Graphics);
        }

        private void formControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!SkinService.Instance.ToolStripSkin.ShowCompanyLogo || this.LogoIcon == null)
                return;
            Rectangle rectLogo = new Rectangle(this.Width - this.LogoIcon.Width - 8
                , this.Height - this.LogoIcon.Height, this.LogoIcon.Width, this.LogoIcon.Height);
            if (rectLogo.Contains(e.Location) && this.m_mainFrame != null && !this.m_mainFrame.IsDisposed)
                (new AboutSystemForm()).ShowDialog();
        }

        private void formControl1_SizeChanged(object sender, System.EventArgs e)
        {
            this.Invalidate(true);
        }

        public string GetPath()
        {
            return Application.StartupPath;
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "�л�����" || e.Name == "�л�����")
            {
                if (GlobalMethods.Misc.IsEmptyString(e.Param) || GlobalMethods.Misc.IsEmptyString(e.Data))
                    return;
                DeptInfo displayDept = new DeptInfo();
                displayDept.DeptCode = e.Param.ToString();
                displayDept.DeptName = e.Data.ToString();
                SystemContext.Instance.LoginUser.DeptCode = e.Param.ToString();
                SystemContext.Instance.LoginUser.DeptName = e.Data.ToString();
                SystemContext.Instance.LoginUser.WardCode = e.Param.ToString();
                SystemContext.Instance.LoginUser.WardName = e.Data.ToString();
                SystemContext.Instance.DisplayDept = displayDept;
                SystemContext.Instance.OnDisplayDeptChanged(this, EventArgs.Empty);
                //this.m_mainFrame.DeptChange();
            }
            else if (e.Name == "����wordģ��")
            {
                string path = @"\Templets\WordTemp";
                string newPath = GetPath() + @"" + path + @"\";
                if (!System.IO.Directory.Exists(newPath))
                {
                    System.IO.Directory.CreateDirectory(newPath);
                }
                e.Result = UtilitiesHandler.Instance.ShowWordTempForm();
            }
            else if (e.Name == "�л��û�" || e.Name == "�л���¼�û�")
            {
                const int WM_SWITCHUSER = NativeConstants.WM_USER + 1;
                int result = 0;
                if (this.MainFrame != null)
                    result = NativeMethods.User32.SendMessage(this.MainFrame.Handle, WM_SWITCHUSER, IntPtr.Zero, IntPtr.Zero);
                e.Cancel = (result == 0);
            }
            else if (e.Name == "������Ϣ��")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.Arguments = e.Param.ToString().Replace(SystemConst.StartupArgs.ESCAPED_CHAR, SystemConst.StartupArgs.ESCAPE_CHAR);
                proc.StartInfo.FileName = string.Format(@"{0}\InfoLib.exe", Application.StartupPath);
                try
                {
                    proc.Start();
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show("�޷�����������Ϣϵͳ!");
                    LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, "������Ϣϵͳ����ʧ��!", ex);
                }
            }
            else if (e.Name == "��������")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.Arguments = e.Param.ToString().Replace(SystemConst.StartupArgs.ESCAPED_CHAR, SystemConst.StartupArgs.ESCAPE_CHAR);
                proc.StartInfo.FileName = string.Format(@"{0}\HealthTech.exe", Application.StartupPath);
                try
                {
                    proc.Start();
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show("�޷�������������ϵͳ!");
                    LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, "��������ϵͳ����ʧ��!", ex);
                }
            }
            else if (e.Name == "�ϴ���־")
            {
                try
                {
                   Thread threadUploadLogs = new Thread(new ThreadStart(this.UploadLogFiles));
                   threadUploadLogs.Start();
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show("�ϴ���־ʧ��!");
                    LogManager.Instance.WriteLog("�ϴ���־ʧ��!", ex);
                }
            }
        }

        private void UploadLogFiles()
        {
            DateTime dtLog = DateTime.Now.AddDays(-4);
            string szIpAddress = this.GetAddressIP();
            if (GlobalMethods.Misc.IsEmptyString(szIpAddress))
                return;
            string szLogLocalPath = string.Format("{0}\\TxtLog", LogManager.Instance.LogFilePath);
            FileInfo[] files = GlobalMethods.IO.GetFiles(szLogLocalPath);
            for (int i = 0; i <= 4; i++)
            {
                string szFileName = dtLog.AddDays(i).ToString("yyyyMMdd") + ".log";
                foreach (FileInfo file in files)
                {
                    if (file.Name == szFileName)
                    {
                        string szLocalLogFile = string.Format("{0}\\TxtLog\\{1}", LogManager.Instance.LogFilePath, szFileName);
                        string strRemoteFile = string.Format("/Logs/{0}/{1}.log", szIpAddress, dtLog.AddDays(i).ToString("yyyyMMdd"));
                        UpgradeService.Instance.UploadLogFile(szLocalLogFile, strRemoteFile);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ����IP��ַ��Ϣ
        /// </summary>
        private string GetAddressIP()
        {
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    return _IPAddress.ToString();
                }
            }
            return "";
        }
    }
}
