// **************************************************************
// 护理电子病历系统公用模块之文本模板主窗口
// Creator:YangMingkun  Date:2012-9-5
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Templet
{
    public partial class TextTempletForm : HerenForm
    {
        private string m_selectedContent = string.Empty;

        /// <summary>
        /// 获取当前选中的模板内容
        /// </summary>
        [Browsable(false)]
        [Description("获取当前选中的模板内容")]
        public string SelectedContent
        {
            get { return this.m_selectedContent; }
        }

        private TempletTreeForm m_TempletTreeForm = null;

        /// <summary>
        /// 获取文本模板树形列表窗口
        /// </summary>
        [Browsable(false)]
        private TempletTreeForm TempletTreeForm
        {
            get
            {
                if (this.m_TempletTreeForm == null
                    || this.m_TempletTreeForm.IsDisposed)
                {
                    this.m_TempletTreeForm = new TempletTreeForm(this);
                    this.m_TempletTreeForm.TopLevel = false;
                    this.m_TempletTreeForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_TempletTreeForm.Dock = DockStyle.Fill;
                    this.m_TempletTreeForm.Parent = this.splitContainer1.Panel1;
                }
                return this.m_TempletTreeForm;
            }
        }

        private TempletEditForm m_TempletEditForm = null;

        /// <summary>
        /// 获取文本模板内容编辑窗口
        /// </summary>
        [Browsable(false)]
        private TempletEditForm TempletEditForm
        {
            get
            {
                if (this.m_TempletEditForm == null
                    || this.m_TempletEditForm.IsDisposed)
                {
                    this.m_TempletEditForm = new TempletEditForm(this);
                    this.m_TempletEditForm.TopLevel = false;
                    this.m_TempletEditForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_TempletEditForm.Dock = DockStyle.Fill;
                    this.m_TempletEditForm.Parent = this.splitContainer1.Panel2;
                }
                return this.m_TempletEditForm;
            }
        }

        public TextTempletForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.TempletTreeForm.Show();
            this.TempletEditForm.Show();
            this.TempletTreeForm.Focus();
            Application.DoEvents();

            //根据当前用户,刷新文本模板列表
            string szOldUser = this.TempletTreeForm.Tag as string;
            string szCurrUser = null;
            if (SystemContext.Instance.LoginUser != null)
                szCurrUser = SystemContext.Instance.LoginUser.ID;
            if (szCurrUser != szOldUser)
            {
                this.TempletTreeForm.Tag = szCurrUser;
                this.TempletTreeForm.RefreshTempletList();
            }
        }

        internal void ShowStatusMassage(string szMessage)
        {
            this.lblStatusInfo.Text = szMessage;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.ImportSelectedContent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.Close();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 刷新主窗口显示标题
        /// </summary>
        private void RefreshFormText()
        {
            string szFormText = "护理文本模板管理器";
            TextTempletInfo templetInfo = null;
            if (this.TempletEditForm != null)
                templetInfo = this.TempletEditForm.TempletInfo;
            if (templetInfo != null)
                szFormText = string.Concat(szFormText, " - ", templetInfo.TempletName);
            if (this.Text != szFormText)
                this.Text = szFormText;
        }

        /// <summary>
        /// 检查指定模板是否已经打开,如果已经打开则将其激活
        /// </summary>
        /// <param name="templetInfo">文本模板信息</param>
        /// <returns>true-是打开的;false-未打开</returns>
        private bool IsTempletOpened(TextTempletInfo templetInfo)
        {
            if (this.TempletEditForm == null || this.TempletEditForm.IsDisposed)
                return false;

            if (this.TempletEditForm.TempletInfo == null)
                return false;

            if (templetInfo == null)
                return false;

            //如果文本模板已经打开,则不再重复打开
            TextTempletInfo currTempletInfo = this.TempletEditForm.TempletInfo;
            if (currTempletInfo != null && currTempletInfo.TempletID == templetInfo.TempletID)
                return true;
            return false;
        }

        /// <summary>
        /// 检查指定的模板是否允许当前用户修改
        /// </summary>
        /// <param name="templetInfo">文本模板信息</param>
        /// <returns>bool</returns>
        internal bool IsAllowModifyTemplet(TextTempletInfo templetInfo)
        {
            if (SystemContext.Instance.LoginUser == null || templetInfo == null)
                return false;

            //科室共享的有作者的模板不允许修改
            if (GlobalMethods.Misc.IsEmptyString(templetInfo.CreatorID)
                && templetInfo.ShareLevel == ServerData.ShareLevel.DEPART
                && templetInfo.WardCode == SystemContext.Instance.LoginUser.WardCode)
                return true;

            bool bAllowModify = false;
            if (templetInfo.CreatorID == SystemContext.Instance.LoginUser.ID)
                bAllowModify = true;
            return bAllowModify;
        }

        /// <summary>
        /// 下载并打开指定的文本模板
        /// </summary>
        /// <param name="templetInfo">文本模板信息</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short OpenTemplet(TextTempletInfo templetInfo)
        {
            if (templetInfo == null || templetInfo.IsFolder)
                return SystemConst.ReturnValue.OK;

            if (SystemContext.Instance.LoginUser == null)
                return SystemConst.ReturnValue.OK;

            if (this.IsTempletOpened(templetInfo))
                return SystemConst.ReturnValue.OK;

            if (this.CheckTempletModify() != SystemConst.ReturnValue.OK)
                return SystemConst.ReturnValue.CANCEL;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.TempletEditForm.TempletInfo = templetInfo;
            this.RefreshFormText();
            this.Update();
            short shRet = this.TempletEditForm.OpenTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return shRet;
        }

        /// <summary>
        /// 导入当前选中的模板到当前病历文档
        /// </summary>
        internal void ImportSelectedContent()
        {
            if (this.TempletEditForm == null || this.TempletEditForm.IsDisposed)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            this.m_selectedContent = this.TempletEditForm.GetSelectedContent();
            if (GlobalMethods.Misc.IsEmptyString(this.m_selectedContent))
                this.TempletEditForm.SelectAll();
            this.m_selectedContent = this.TempletEditForm.GetSelectedContent();

            this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 检查当前模板是否已经被修改
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short CheckTempletModify()
        {
            if (this.TempletEditForm == null || this.TempletEditForm.IsDisposed)
                return SystemConst.ReturnValue.OK;

            if (!this.TempletEditForm.IsModified)
                return SystemConst.ReturnValue.OK;

            TextTempletInfo templetInfo = this.TempletEditForm.TempletInfo;
            if (SystemContext.Instance.LoginUser == null || templetInfo == null || templetInfo.IsFolder)
                return SystemConst.ReturnValue.OK;

            if (!this.IsAllowModifyTemplet(templetInfo))
                return SystemConst.ReturnValue.OK;

            DialogResult result = DialogResult.OK;
            result = MessageBoxEx.ShowQuestion(string.Format("是否保存对“{0}”文本模板的修改？", templetInfo.TempletName));

            short shRet = SystemConst.ReturnValue.OK;
            if (result == DialogResult.Yes)
                shRet = this.TempletEditForm.SaveTemplet();
            else if (result == DialogResult.No)
                this.TempletEditForm.CloseTemplet();
            else if (result == DialogResult.Cancel)
                return SystemConst.ReturnValue.CANCEL;
            return shRet != SystemConst.ReturnValue.OK ? SystemConst.ReturnValue.CANCEL : shRet;
        }
    }
}