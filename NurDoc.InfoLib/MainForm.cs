using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Heren.Common.Controls.VirtualTreeView;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.InfoLib.DockForms;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.InfoLib
{
    public partial class MainForm : Form
    {
        private InfoLibForm m_InfoLibForm = null;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Icon = Properties.Resources.SysInfoLib;
            this.Update();
            this.ShowInfoLibForm();
        }

        private void ShowInfoLibForm()
        {
            this.dockPanel1.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_InfoLibForm == null || this.m_InfoLibForm.IsDisposed)
            {
                this.m_InfoLibForm = new InfoLibForm(this);
                this.m_InfoLibForm.Show(this.dockPanel1);
            }
            this.m_InfoLibForm.Activate();
            this.m_InfoLibForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 切换当前登录用户为新的用户
        /// </summary>
        /// <param name="userInfo">新用户信息</param>
        /// <returns>是否切换成功</returns>
        public bool SwitchSystemUser(UserInfo userInfo)
        {
            if (userInfo == null || string.IsNullOrEmpty(userInfo.ID))
                return false;

            UserInfo currentUser = null;
            if (SystemContext.Instance.LoginUser != null)
                currentUser = SystemContext.Instance.LoginUser;
            if (currentUser != null && currentUser.Equals(userInfo))
                return true;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            SystemContext.Instance.LoginUser = userInfo.Clone() as UserInfo;
            
            this.Show();
            string szDeptName = SystemContext.Instance.LoginUser.DeptName;
            string szUserName = SystemContext.Instance.LoginUser.Name;
            this.Text = string.Format("护理信息库 - {0} {1}", szDeptName, szUserName);
            Application.DoEvents();

            SystemConfig.Instance.Write(SystemConst.ConfigKey.DEFAULT_LOGIN_USERID, userInfo.ID);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }
    }
}