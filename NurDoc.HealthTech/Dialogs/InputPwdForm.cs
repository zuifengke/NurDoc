using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.Common.Libraries;

namespace Heren.NurDoc.HealthTech
{
    public partial class InputPwdForm : Form
    {
        private UserInfo m_loginUser = null;

        public UserInfo loginUser
        {
            get { return this.m_loginUser; }
        }

        public InputPwdForm()
        {
            InitializeComponent();
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

            ////获取用户信息
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

            ////验证用户输入的密码
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

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void InfoLibRenameForm_Shown(object sender, EventArgs e)
        {
            ////base.OnShown(e);
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
    }
}