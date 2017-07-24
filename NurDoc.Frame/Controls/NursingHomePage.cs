// ***********************************************************
// 护理电子病历系统,主窗口顶部系统起始页模块.
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
        /// 获取或设置所属的主程序窗口
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置所属的主程序窗口")]
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
        /// 获取公司Logo图标
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
        /// 获取或设置单病人模式
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

            object data = this.formControl1.GetFormData("颜色表");
            PatientTable.Instance.ColorTable = data as Hashtable;

            data = this.formControl1.GetFormData("体温类型表");
            SystemContext.Instance.TempTypeList = data as string[];

            data = this.formControl1.GetFormData("床卡颜色表");
            SystemContext.Instance.PatientCardColorList = data as Dictionary<string, Color>;

            data = this.formControl1.GetFormData("体征时间点表");
            SystemContext.Instance.TimePointTable = data as Hashtable;

            data = this.formControl1.GetFormData("标准版血压时间点");
            SystemContext.Instance.StandardBPTimePoint = data as int[];

            data = this.formControl1.GetFormData("体征映射表");
            SystemContext.Instance.VitalSignsNameTable = data as Hashtable;

            data = this.formControl1.GetFormData("体征值映射表");
            SystemContext.Instance.VitalSignsValueTable = data as Hashtable;

            data = this.formControl1.GetFormData("医嘱类别映射表");
            SystemContext.Instance.OrderCategoryTable = data as Hashtable;

            data = this.formControl1.GetFormData("医嘱途径映射表");
            SystemContext.Instance.OrderWayValueTable = data == null ? null : data as Hashtable;

            data = this.formControl1.GetFormData("液体医嘱表");
            SystemContext.Instance.LiquidOrderList = data as string[];

            data = this.formControl1.GetFormData("体征数据不带时间点");
            SystemContext.Instance.VitalSignNoTimePoint = data as string;

            data = this.formControl1.GetFormData("录入体征时间减一天");
            SystemContext.Instance.VitalSignTimeBeforeOneDay = data as string;

            data = this.formControl1.GetFormData("护理记录小结名称表");
            SystemContext.Instance.SummaryNameList = data as string[];

            data = this.formControl1.GetFormData("交接班项目排序");
            SystemContext.Instance.ShiftItemComparer = data as string;

            data = this.formControl1.GetFormData("护理计划配置按钮");
            SystemContext.Instance.NCPButtonConfigTable = SystemContext.Instance.GetNursCarPlanConfigs(data as DataTable);

            data = this.formControl1.GetFormData("病人列表");
            PatientTable.Instance.RefreshDeptPatientList(data as DataTable);

            data = this.formControl1.GetFormData("科室列表");
            SystemContext.Instance.DeptTable = data as DataTable;

            if (this.m_bSinglePatMod)
                this.formControl1.UpdateFormData("单病人模式", true);
            else
                this.formControl1.UpdateFormData("单病人模式", false);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void System_DisplayDeptChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            //当科室被切换后,通知表单去刷新视图
            this.formControl1.UpdateFormData("刷新视图", null);

            //当科室被切换后,重置当前科室病人列表
            object data = this.formControl1.GetFormData("病人列表");
            PatientTable.Instance.RefreshDeptPatientList(data as DataTable);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void LoadHomePageTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_HOME_PAGE;
            DocTypeInfo docInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            if (docInfo == null || GlobalMethods.Misc.IsEmptyString(docInfo.DocTypeID))
            {
                MessageBoxEx.ShowError("系统起始页表单下载失败！");
                return;
            }
            byte[] byteTempletData = null;
            if (!FormCache.Instance.GetFormTemplet(docInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("系统起始页表单内容下载失败！");
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
            if (e.Name == "切换病区" || e.Name == "切换科室")
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
            else if (e.Name == "导入word模板")
            {
                string path = @"\Templets\WordTemp";
                string newPath = GetPath() + @"" + path + @"\";
                if (!System.IO.Directory.Exists(newPath))
                {
                    System.IO.Directory.CreateDirectory(newPath);
                }
                e.Result = UtilitiesHandler.Instance.ShowWordTempForm();
            }
            else if (e.Name == "切换用户" || e.Name == "切换登录用户")
            {
                const int WM_SWITCHUSER = NativeConstants.WM_USER + 1;
                int result = 0;
                if (this.MainFrame != null)
                    result = NativeMethods.User32.SendMessage(this.MainFrame.Handle, WM_SWITCHUSER, IntPtr.Zero, IntPtr.Zero);
                e.Cancel = (result == 0);
            }
            else if (e.Name == "护理信息库")
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
                    MessageBoxEx.Show("无法启动护理信息系统!");
                    LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, "护理信息系统启动失败!", ex);
                }
            }
            else if (e.Name == "健康教育")
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
                    MessageBoxEx.Show("无法启动健康教育系统!");
                    LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, "健康教育系统启动失败!", ex);
                }
            }
            else if (e.Name == "上传日志")
            {
                try
                {
                   Thread threadUploadLogs = new Thread(new ThreadStart(this.UploadLogFiles));
                   threadUploadLogs.Start();
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show("上传日志失败!");
                    LogManager.Instance.WriteLog("上传日志失败!", ex);
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
        /// 获取本地IP地址信息
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
