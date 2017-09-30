// ***********************************************************
// 护理电子病历系统,用户登陆对话框窗口.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame;
using System.Security.Cryptography;
using System.IO;

namespace Heren.NurDoc.Startup
{
    public partial class LoginForm : HerenForm
    {
        private UserInfo m_loginUser = null;
        private string Md5Code = string.Empty;
        private string Md5HerenCert = "898EE74DB20832CF3D3B43FCBE7DA382";

        /// <summary>
        /// 获取当前登录成功的用户信息
        /// </summary>
        public UserInfo LoginUser
        {
            get { return this.m_loginUser; }
        }

        public LoginForm()
        {
            this.InitializeComponent();
            this.Text = string.Format("登录{0}", SystemContext.Instance.SystemName);
            this.Icon = Properties.Resources.LoginIcon;
            this.btnModifyPwd.Image = Properties.Resources.ModifyPwd;

            if (File.Exists(GlobalMethods.Misc.GetWorkingPath() + "\\HospitalLogo.Png"))
            {
                this.pictureBox1.Image = Image.FromFile(GlobalMethods.Misc.GetWorkingPath() + "\\HospitalLogo.Png");
            }
            else
            { this.pictureBox1.Visible = false; }

            //MD5 md5 = new MD5CryptoServiceProvider();
            //MemoryStream memStream = new MemoryStream();
            //this.pictureBox1.Image.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
            //byte[] Md5Data = md5.ComputeHash(memStream);
            //Md5Code = BitConverter.ToString(Md5Data).Replace("-", "");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            this.txtUserID.Focus();
            string key = SystemConst.ConfigKey.DEFAULT_LOGIN_USERID;
            this.txtUserID.Text = SystemConfig.Instance.Get(key, null);
            if (GlobalMethods.Misc.IsEmptyString(this.txtUserID.Text))
                this.txtUserID.Focus();
            else
                this.txtUserPwd.Focus();
            this.txtUserID.ImeMode = ImeMode.Alpha;

            //如果系统已经登陆则跳过文件验证
            if (SystemContext.Instance.LoginUser != null)
            { return; }
            Stream streamHerenCert = File.Open("HerenCert.dll", FileMode.Open);
            byte[] bytes = new byte[(int)streamHerenCert.Length];//初始化一个字节数组
            streamHerenCert.Read(bytes, 0, bytes.Length);//将文件读到字节数组中

            MemoryStream memHerenCert = new MemoryStream(bytes);//用MemoryStream接收
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] Md5HerenCerData = md5.ComputeHash(memHerenCert);
            string Md5Cert = BitConverter.ToString(Md5HerenCerData).Replace("-", "");
            if (Md5HerenCert != Md5Cert)
            {
                MessageBox.Show("系统产品授权系统自检异常！");
                this.btnCancel.PerformClick();
            }
            streamHerenCert.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //根据MD5编码判断医院图片是否进行了自定义，如果自定义了则显示医院图片。
            //if (Md5Code == "D41D8CD98F00B204E9800998ECF8427E" || string.IsNullOrEmpty(Md5Code))
            //{ this.pictureBox1.Visible = false; }

            int iWhdth = this.Width;
            int iHeight = this.Height / 2;
            Rectangle rect = new Rectangle(0, 0, iWhdth, iHeight);
            Color beginColor = Color.RoyalBlue;
            Color endColor = Color.CornflowerBlue;
            LinearGradientBrush brush = new LinearGradientBrush(rect
                , beginColor, endColor, LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, rect);
            brush.Dispose();

            Image img = Properties.Resources.LoginBgImage;
            e.Graphics.DrawImage(img, iWhdth / 10, (iHeight / 2) - 16);

            Font font = new Font("宋体", 9, FontStyle.Bold);
            SolidBrush solidBrush = new SolidBrush(Color.White);
            e.Graphics.DrawString("请输入您的登录ID和口令...", font, solidBrush
                , (iWhdth / 10) + Properties.Resources.LoginBgImage.Width + 10, (iHeight / 2) - 8);
            solidBrush.Dispose();
            font.Dispose();
        }

        private void btnModifyPwd_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            (new ModifyPwdForm()).ShowDialog();
            this.txtUserPwd.Focus();
            this.txtUserPwd.SelectAll();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string szUserID = this.txtUserID.Text.Trim().ToUpper();
            if (GlobalMethods.Misc.IsEmptyString(szUserID))
            {
                MessageBoxEx.Show("请输入您的用户ID!");
                this.txtUserID.Focus();
                this.txtUserID.SelectAll();
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            short shRet = SystemConst.ReturnValue.OK;

            //获取用户信息
            this.m_loginUser = null;
            shRet = AccountService.Instance.GetUserInfo(szUserID, ref this.m_loginUser);
            if (shRet == SystemConst.ReturnValue.FAILED)
            {
                MessageBoxEx.ShowError(string.Format("您没有权限登录{0}!", SystemContext.Instance.SystemName));
                this.txtUserID.Focus();
                this.txtUserID.SelectAll();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (this.m_loginUser == null || shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("登录失败,系统获取用户信息!");
                this.txtUserID.Focus();
                this.txtUserID.SelectAll();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            //验证用户输入的密码
            shRet = AccountService.Instance.IsUserValid(szUserID, this.txtUserPwd.Text);
            if (shRet == SystemConst.ReturnValue.FAILED)
            {
                MessageBoxEx.ShowError("您输入的登录口令错误!");
                this.txtUserPwd.Focus();
                this.txtUserPwd.SelectAll();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("登录失败,系统无法验证用户信息!");
                this.txtUserPwd.Focus();
                this.txtUserPwd.SelectAll();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            //验证用户的权限
            UserRightBase userRight = null;
            AccountService.Instance.GetUserRight(this.m_loginUser.ID, UserRightType.NurDoc, ref userRight);
            NurUserRight nurUserRight = userRight as NurUserRight;
            if (nurUserRight == null || !nurUserRight.NurDocSystem.Value)
            {
                MessageBoxEx.ShowError(string.Format("您没有权限登录{0}!", SystemContext.Instance.SystemName));
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            //过滤质控系统不用功能
            if (this.cbRightSelect.SelectedIndex != 0)
            {
                nurUserRight.CreateNuringDoc.Value = false;
                nurUserRight.ShowBedViewForm.Value = false;
                nurUserRight.ShowNursingTaskForm.Value = false;
                nurUserRight.ShowPatientListForm.Value = false;
                nurUserRight.ShowBatchRecordForm.Value = false;
                nurUserRight.ShowShiftRecordForm.Value = false;
                nurUserRight.ShowNursingStatForm.Value = false;
            }
            else
            {
                //由于新桥医院要求个性化
                if (SystemContext.Instance.SystemOption.HospitalName == "第三军医大学新桥医院")
                {
                    nurUserRight.ShowNursingAssessForm.Value = false;
                }
                nurUserRight.ShowNursingQCForm.Value = false;

            }

            RightController.Instance.UserRight = nurUserRight;

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            this.DialogResult = DialogResult.OK;
        }

        private bool CheckUserRight(string szUserID)
        {
            //验证用户的权限
            UserRightBase userRight = null;
            AccountService.Instance.GetUserRight(szUserID.ToUpper().Trim(), UserRightType.NurDoc, ref userRight);
            NurUserRight nurUserRight = userRight as NurUserRight;
            if (nurUserRight == null || !nurUserRight.ShowNursingQCForm.Value)
            {
                //MessageBoxEx.ShowError(string.Format("您没有权限登录{0}!", SystemContext.Instance.SystemName));
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
            return true;
        }

        private void txtUserID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUserID.Text))
                return;

            //判断是否有质控权限
            if (!CheckUserRight(this.txtUserID.Text))
            {
                this.cbRightSelect.SelectedIndex = 0;
                this.cbRightSelect.Visible = false;
                this.lblUserRight.Visible = false;
                this.Height = 223 + 175;
                this.btnOK.Location = new Point(this.btnOK.Location.X, 292);
                this.btnCancel.Location = new Point(this.btnCancel.Location.X, 292);
                return;
            }
            //添加权限选择下拉框
            int height = 34;
            this.cbRightSelect.SelectedIndex = 0;
            this.cbRightSelect.Visible = true;
            this.lblUserRight.Visible = true;

            this.Height = 223 + 175 + height;
            this.btnOK.Location = new Point(this.btnOK.Location.X, this.lblUserRight.Location.Y + 34);
            this.btnCancel.Location = new Point(this.btnCancel.Location.X, this.lblUserRight.Location.Y + 34);

        }
    }
}
