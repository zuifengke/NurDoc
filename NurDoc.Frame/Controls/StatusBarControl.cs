// ***********************************************************
// 护理电子病历系统,主窗口状态栏控件.
// Creator:YangMingkun  Date:2012-8-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame.Dialogs;

namespace Heren.NurDoc.Frame.Controls
{
    internal class StatusBarControl : StatusStrip
    {
        private Timer m_timer = null;
        private Timer m_timer2 = null;
        private ToolStripStatusLabel m_lblReady = null;
        //private ToolStripStatusLabel m_lblCompany = null;
        private ToolStripStatusLabel m_lblSysTime = null;
        public ToolStripButton m_btnTaskMessage = null;

        private MainFrame m_mainFrame = null;

        /// <summary>
        /// 获取或设置所属的主程序窗口
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置所属的主程序窗口")]
        public MainFrame MainFrame
        {
            get { return this.m_mainFrame; }
            set { this.m_mainFrame = value; }
        }

        public void Initialize()
        {
            this.Items.Clear();

            //if (this.m_lblCompany == null)
            //{
            //    this.m_lblCompany = new ToolStripStatusLabel();
            //    this.m_lblCompany.AutoSize = false;
            //    this.m_lblCompany.Width = 200;
            //    this.m_lblCompany.BackColor = this.BackColor;
            //    this.m_lblCompany.Text = "浙江和仁科技有限公司";
            //}
            //this.Items.Add(this.m_lblCompany);

            if (this.m_lblSysTime == null)
            {
                this.m_lblSysTime = new ToolStripStatusLabel();
                this.m_lblSysTime.AutoSize = false;
                this.m_lblSysTime.Width = 200;
                this.m_lblSysTime.BackColor = this.BackColor;
                this.m_lblSysTime.Text = DateTime.Now.ToString("yyyy年M月d日 HH:mm dddd");
            }
            this.Items.Add(this.m_lblSysTime);

            if (this.m_btnTaskMessage == null)
            {
                this.m_btnTaskMessage = new ToolStripButton();
                this.m_btnTaskMessage.Text = "(0)";
                this.m_btnTaskMessage.Image = Properties.Resources.Mail;
                this.m_btnTaskMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                this.m_btnTaskMessage.Click += new EventHandler(this.btnTaskMessage_Click);
            }
            this.Items.Add(this.m_btnTaskMessage);

            if (this.m_lblReady == null)
            {
                this.m_lblReady = new ToolStripStatusLabel();
                this.m_lblReady.Spring = true;
                this.m_lblReady.AutoSize = false;
                this.m_lblReady.BackColor = this.BackColor;
                this.m_lblReady.TextAlign = ContentAlignment.MiddleLeft;
                this.m_lblReady.Click += new EventHandler(this.lblReady_Click);
            }
            this.Items.Insert(0, this.m_lblReady);

            if (this.m_timer == null)
            {
                this.m_timer = new Timer();
                this.m_timer.Interval = 1000;
                this.m_timer.Tick += new EventHandler(this.Timer_Tick);
            }
            this.m_timer.Start();

            if (this.m_timer2 == null)
            {
                this.m_timer2 = new Timer();
                if (SystemContext.Instance.SystemOption.TaskMessageTime != 0)
                {
                    this.m_timer2.Start();
                    this.m_timer2.Interval = SystemContext.Instance.SystemOption.TaskMessageTime * 1000;
                    this.m_timer2.Tick += new EventHandler(this.Timer_Tick2);
                }
            }

            string szHospitalName = SystemContext.Instance.SystemOption.HospitalName;
            if (string.IsNullOrEmpty(szHospitalName))
                this.m_lblReady.Text = " 软件未经授权许可.";
            else
                this.m_lblReady.Text = string.Format(" {0} 专用.", szHospitalName);
        }

        private void lblReady_Click(object sender, EventArgs e)
        {
            (new AboutSystemForm()).ShowDialog();
        }

        private void btnTaskMessage_Click(object sender, EventArgs e)
        {
            this.m_btnTaskMessage.Image = Properties.Resources.Mail;
            this.m_mainFrame.ShowNursingTaskForm(true);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string szCurrentTime = SysTimeService.Instance.Now.ToString("yyyy年M月d日 HH:mm:ss dddd");
            if (this.m_lblSysTime.Text != szCurrentTime)
                this.m_lblSysTime.Text = szCurrentTime;
        }

        private void Timer_Tick2(object sender, EventArgs e)
        {
            this.m_mainFrame.ShowNursingTaskForm(false);
        }
    }
}
