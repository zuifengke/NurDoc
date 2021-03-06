// ***********************************************************
// 护理病历文档系统关于窗口
// Creator:YangMingkun  Date:2012-7-8
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class AboutSystemForm : HerenForm
    {
        public AboutSystemForm()
        {
            this.InitializeComponent();
            this.Text = string.Format("和仁{0}", SystemContext.Instance.SystemName);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szLogoFile = GlobalMethods.Misc.GetWorkingPath() + "\\Skins\\About.png";
            this.picSoftwareLogo.Image = Image.FromFile(szLogoFile);

            Assembly assembly = this.GetType().Assembly;
            string szVersionText = assembly.GetName().Version.ToString();
            DateTime dtModifyTime = DateTime.Now;
            GlobalMethods.IO.GetFileLastModifyTime(assembly.Location, ref dtModifyTime);
            this.txtSoftware.Text = string.Format("和仁{2} (NurDocSys)  v{0},  Build {1}",
                szVersionText, dtModifyTime.ToString("yyMMddHHmm"), SystemContext.Instance.SystemName);

            this.txtCopyrightCH.Text = string.Empty;// "本产品及各项专利  浙江和仁科技有限公司 版权所有";
            this.txtCopyrightEN.Text = string.Format("© 2011-{0} 浙江和仁科技有限公司版权所有，保留所有权利。", DateTime.Now.Year.ToString());

            if (SystemContext.Instance.LoginUser == null)
            {
                this.txtCertText.Text = "未检测到产品授权许可信息";
            }
            else if (string.IsNullOrEmpty(SystemContext.Instance.SystemOption.HospitalName))
            {
                this.txtCertText.Text = "产品未经授权许可";
            }
            else
            {
                this.txtCertText.Text = string.Format("{0} 专用,  产品密钥:{1}"
                    , SystemContext.Instance.SystemOption.HospitalName
                    , SystemContext.Instance.SystemOption.CertCode);
            }

            this.txtVersionInfo.Focus();
            string szVersionFile = GlobalMethods.Misc.GetWorkingPath() + "\\Versions.dat";
            if (GlobalMethods.Misc.IsEmptyString(szVersionFile))
            {
                this.txtVersionInfo.AppendText("版本信息文件不存在！");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            try
            {
                this.txtVersionInfo.LoadFile(szVersionFile, RichTextBoxStreamType.RichText);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            catch (Exception ex)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                this.txtVersionInfo.Text = "软件版本发布信息文件加载失败！异常信息是：\n" + ex.ToString();
            }
        }

        private void picSoftwareLogo_Paint(object sender, PaintEventArgs e)
        {
            Image logoImage = Properties.Resources.LogoBig;//.Logo;
            e.Graphics.DrawImage(logoImage, 8, 4);

            //图片中已有公司名称所以不用再次画公司名称
            //SolidBrush brush = new SolidBrush(Color.YellowGreen);
            //Point ptLocation = new Point(8, logoImage.Height + 8);
            //e.Graphics.DrawString("浙江和仁科技有限公司", this.Font, brush, ptLocation);
            //brush.Dispose();
        }

        private void txtVersionInfo_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (GlobalMethods.Misc.IsEmptyString(e.LinkText))
                return;
            string szLinkText = e.LinkText;
            try
            {
                szLinkText = szLinkText.Replace("{app_path}", GlobalMethods.Misc.GetWorkingPath());
                System.Diagnostics.Process.Start(szLinkText);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(string.Format("无法打开此链接“{0}”!\r\n{1}", szLinkText, ex.Message));
            }
        }
    }
}