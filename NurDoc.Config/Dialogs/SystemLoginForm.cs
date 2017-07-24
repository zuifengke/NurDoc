// ***********************************************************
// ���������ù���ϵͳ,�û���¼�Ի��򴰿�.
// Author : YangMingkun, Date : 2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Config.Dialogs
{
    internal partial class SystemLoginForm : HerenForm
    {
        public SystemLoginForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.LoginIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            this.txtUserID.Focus();
            this.txtUserID.Text = "Administrator";
            this.txtUserID.Enabled = false;
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
            Font font = new Font("����", 9, FontStyle.Bold);
            SolidBrush solidBrush = new SolidBrush(Color.White);
            e.Graphics.DrawString("���������ĵ�¼ID�Ϳ���...", font, solidBrush, 68, 32);
            solidBrush.Dispose();
            font.Dispose();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string szUserID = this.txtUserID.Text.Trim();
            if (GlobalMethods.Misc.IsEmptyString(szUserID))
            {
                MessageBoxEx.Show("�����������û�ID!");
                this.txtUserID.Focus();
                this.txtUserID.SelectAll();
                return;
            }
            szUserID = szUserID.ToUpper();
            this.Cursor = Cursors.WaitCursor;

            //��֤�û����������
            short shRet = SystemConst.ReturnValue.OK;
            try
            {
                shRet = AccountService.Instance.IsUserValid(szUserID, this.txtUserPwd.Text);
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowError("��¼ʧ��,ϵͳ�޷���֤�û���Ϣ!", ex.ToString());
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (shRet == SystemConst.ReturnValue.FAILED)
            {
                MessageBoxEx.ShowError("��������˺Ż��¼�������!");
                this.txtUserPwd.Focus();
                this.txtUserPwd.SelectAll();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("��¼ʧ��,ϵͳ�޷���֤�û���Ϣ!");
                this.txtUserPwd.Focus();
                this.txtUserPwd.SelectAll();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            SystemContext.Instance.LoginUser = new UserInfo(szUserID, "ADMIN", "ADMIN", "ADMIN");
            this.Cursor = Cursors.Default;
            this.DialogResult = DialogResult.OK;
        }
    }
}