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

namespace Heren.NurDoc.InfoLib
{
    public partial class LoginForm : HerenForm
    {
        private UserInfo m_loginUser = null;

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
            this.Icon = Properties.Resources.LoginIcon;
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
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //
            Rectangle rect = new Rectangle(0, 0, this.Width, 72);
            Color beginColor = Color.RoyalBlue;
            Color endColor = Color.CornflowerBlue;
            LinearGradientBrush brush = new LinearGradientBrush(rect
                , beginColor, endColor, LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, rect);
            brush.Dispose();
            //
            Image img = Properties.Resources.LoginBgImage;
            e.Graphics.DrawImage(img, 24, 18);
            //
            Font font = new Font("宋体", 9, FontStyle.Bold);
            SolidBrush solidBrush = new SolidBrush(Color.White);
            e.Graphics.DrawString("请输入您的登录ID和口令...", font, solidBrush, 68, 32);
            solidBrush.Dispose();
            font.Dispose();
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
            RightController.Instance.UserRight = nurUserRight;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            this.DialogResult = DialogResult.OK;
        }
    }
}
