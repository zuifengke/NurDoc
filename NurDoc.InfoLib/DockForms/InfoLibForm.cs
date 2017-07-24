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
using Heren.Common.DockSuite;

namespace Heren.NurDoc.InfoLib.DockForms
{
    internal partial class InfoLibForm : DockContentBase
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

        private InfoLibMenuForm m_InfoLibMenuForm = null;

        /// <summary>
        /// 获取文本模板树形列表窗口
        /// </summary>
        [Browsable(false)]
        private InfoLibMenuForm InfoLibMenuForm
        {
            get
            {
                if (this.m_InfoLibMenuForm == null
                    || this.m_InfoLibMenuForm.IsDisposed)
                {
                    this.m_InfoLibMenuForm = new InfoLibMenuForm(this);
                    this.m_InfoLibMenuForm.TopLevel = false;
                    this.m_InfoLibMenuForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_InfoLibMenuForm.Dock = DockStyle.Fill;
                    this.m_InfoLibMenuForm.Parent = this.splitContainer2.Panel1;
                }
                return this.m_InfoLibMenuForm;
            }
        }

        private InfoLibTreeForm m_InfoLibTreeForm = null;

        /// <summary>
        /// 获取文本模板树形列表窗口
        /// </summary>
        [Browsable(false)]
        private InfoLibTreeForm InfoLibTreeForm
        {
            get
            {
                if (this.m_InfoLibTreeForm == null
                    || this.m_InfoLibTreeForm.IsDisposed)
                {
                    this.m_InfoLibTreeForm = new InfoLibTreeForm(this);
                    this.m_InfoLibTreeForm.TopLevel = false;
                    this.m_InfoLibTreeForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_InfoLibTreeForm.Dock = DockStyle.Fill;
                    this.m_InfoLibTreeForm.Parent = this.splitContainer1.Panel1;
                }
                return this.m_InfoLibTreeForm;
            }
        }

        private InfoLibEditForm m_InfoLibEditForm = null;

        /// <summary>
        /// 获取文本模板内容编辑窗口
        /// </summary>
        [Browsable(false)]
        private InfoLibEditForm InfoLibEditForm
        {
            get
            {
                if (this.m_InfoLibEditForm == null
                    || this.m_InfoLibEditForm.IsDisposed)
                {
                    this.m_InfoLibEditForm = new InfoLibEditForm(this);
                    this.m_InfoLibEditForm.TopLevel = false;
                    this.m_InfoLibEditForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_InfoLibEditForm.Dock = DockStyle.Fill;
                    this.m_InfoLibEditForm.Parent = this.splitContainer1.Panel2;
                }
                return this.m_InfoLibEditForm;
            }
        }

        private InfoLibListForm m_InfoLibListForm = null;

        /// <summary>
        /// 获取文本模板内容编辑窗口
        /// </summary>
        [Browsable(false)]
        private InfoLibListForm InfoLibListForm
        {
            get
            {
                if (this.m_InfoLibListForm == null
                    || this.m_InfoLibListForm.IsDisposed)
                {
                    this.m_InfoLibListForm = new InfoLibListForm(this);
                    this.m_InfoLibListForm.TopLevel = false;
                    this.m_InfoLibListForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_InfoLibListForm.Dock = DockStyle.Fill;
                    this.m_InfoLibListForm.Parent = this.splitContainer1.Panel2;
                }
                return this.m_InfoLibListForm;
            }
        }

        public InfoLibForm(MainForm parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
            string text = SystemContext.Instance.WindowName.InfoLib;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 400);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.InfoLibMenuForm.Show();
            this.InfoLibTreeForm.Show();
            //this.InfoLibEditForm.Show();
            this.InfoLibListForm.Show();
            this.InfoLibTreeForm.Focus();
            Application.DoEvents();

            //根据当前用户,刷新文本模板列表
            string szOldUser = this.InfoLibTreeForm.Tag as string;
            string szCurrUser = null;
            if (SystemContext.Instance.LoginUser != null)
                szCurrUser = SystemContext.Instance.LoginUser.ID;
            if (szCurrUser != szOldUser)
            {
                this.InfoLibTreeForm.Tag = szCurrUser;
                this.InfoLibTreeForm.RefreshTempletList();
            }
        }

        public override void OnRefreshView()
        {
            base.OnRefreshView();
            this.Update();
        }

        internal void ShowStatusMessage(string szMessage)
        {
            this.StatusLabel.Text = szMessage;
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
        /// 刷新主窗口护理信息库
        /// </summary>
        private void RefreshFormText()
        {
            string szFormText = "护理信息库";
            InfoLibInfo infoLibInfo = null;
            if (this.InfoLibEditForm != null)
                infoLibInfo = this.InfoLibEditForm.InfoLibInfo;
            if (infoLibInfo != null)
                szFormText = string.Concat(szFormText, " - ", infoLibInfo.InfoName);
            if (this.Text != szFormText)
                this.Text = szFormText;
        }

        /// <summary>
        /// 检查指定模板是否已经打开,如果已经打开则将其激活
        /// </summary>
        /// <param name="infoLibInfo">文本模板信息</param>
        /// <returns>true-是打开的;false-未打开</returns>
        private bool IsTempletOpened(InfoLibInfo infoLibInfo)
        {
            if (this.InfoLibEditForm == null || this.InfoLibEditForm.IsDisposed)
                return false;

            if (this.InfoLibEditForm.InfoLibInfo == null)
                return false;

            if (infoLibInfo == null)
                return false;

            //如果文本模板已经打开,则不再重复打开
            InfoLibInfo currInfoLibInfo = this.InfoLibEditForm.InfoLibInfo;
            if (currInfoLibInfo != null && currInfoLibInfo.InfoID == infoLibInfo.InfoID)
                return true;
            return false;
        }

        /// <summary>
        /// 检查指定的模板是否允许当前用户修改
        /// </summary>
        /// <param name="infoLibInfo">文本模板信息</param>
        /// <returns>bool</returns>
        internal bool IsAllowModifyTemplet(InfoLibInfo infoLibInfo)
        {
            if (SystemContext.Instance.LoginUser == null || infoLibInfo == null)
                return false;

            //科室共享的有作者的模板不允许修改
            if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.CreatorID)
                && infoLibInfo.ShareLevel == ServerData.ShareLevel.DEPART
                && infoLibInfo.WardCode == SystemContext.Instance.LoginUser.WardCode)
                return true;

            bool bAllowModify = false;
            if (infoLibInfo.CreatorID == SystemContext.Instance.LoginUser.ID)
                bAllowModify = true;
            return bAllowModify;
        }

        /// <summary>
        /// 下载并打开指定的文本模板
        /// </summary>
        /// <param name="infoLibInfo">文本模板信息</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short OpenTemplet(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null || infoLibInfo.IsFolder)
                return SystemConst.ReturnValue.OK;

            if (SystemContext.Instance.LoginUser == null)
                return SystemConst.ReturnValue.OK;

            if (this.IsTempletOpened(infoLibInfo))
                return SystemConst.ReturnValue.OK;

            if (this.CheckTempletModify() != SystemConst.ReturnValue.OK)
                return SystemConst.ReturnValue.CANCEL;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.InfoLibEditForm.InfoLibInfo = infoLibInfo;
            this.RefreshFormText();
            this.Update();
            short shRet = this.InfoLibEditForm.OpenTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return shRet;
        }

        /// <summary>
        /// 下载并打开主目录下的文件夹
        /// </summary>
        /// <param name="szValue">文本模板信息</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short ShowChildInfoLib(string szValue)
        {
            if (szValue == null)
                return SystemConst.ReturnValue.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_InfoLibTreeForm.MenuID = szValue;
            this.Update();
            this.InfoLibTreeForm.ShowChildInfoLib(szValue);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 下载并打开文件夹下的文档列表
        /// </summary>
        /// <param name="infoLibInfo">文本模板信息</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short ShowInfoLibList(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null)
                return SystemConst.ReturnValue.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_InfoLibListForm.InfoLibInfo = infoLibInfo;
            this.Update();
            this.m_InfoLibListForm.ShowInfoLibList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 下载并打开文件夹下的文档列表
        /// </summary>
        /// <param name="infoLibInfo">文本模板信息</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short OpenInfoLibInfo(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null)
                return SystemConst.ReturnValue.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_InfoLibListForm.InfoLibInfo = infoLibInfo;
            this.Update();
            this.m_InfoLibListForm.ShowInfoLibList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 导入当前选中的模板到当前病历文档
        /// </summary>
        internal void ImportSelectedContent()
        {
            if (this.InfoLibEditForm == null || this.InfoLibEditForm.IsDisposed)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            this.m_selectedContent = this.InfoLibEditForm.GetSelectedContent();
            if (GlobalMethods.Misc.IsEmptyString(this.m_selectedContent))
                this.InfoLibEditForm.SelectAll();
            this.m_selectedContent = this.InfoLibEditForm.GetSelectedContent();

            this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 检查当前模板是否已经被修改
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short CheckTempletModify()
        {
            if (this.InfoLibEditForm == null || this.InfoLibEditForm.IsDisposed)
                return SystemConst.ReturnValue.OK;

            if (!this.InfoLibEditForm.IsModified)
                return SystemConst.ReturnValue.OK;

            InfoLibInfo infoLibInfo = this.InfoLibEditForm.InfoLibInfo;
            if (SystemContext.Instance.LoginUser == null || infoLibInfo == null || infoLibInfo.IsFolder)
                return SystemConst.ReturnValue.OK;

            if (!this.IsAllowModifyTemplet(infoLibInfo))
                return SystemConst.ReturnValue.OK;

            DialogResult result = DialogResult.OK;
            result = MessageBoxEx.ShowQuestion(string.Format("是否保存对“{0}”文档的修改？", infoLibInfo.InfoName));

            short shRet = SystemConst.ReturnValue.OK;
            if (result == DialogResult.Yes)
                shRet = this.InfoLibEditForm.SaveTemplet();
            else if (result == DialogResult.No)
                this.InfoLibEditForm.CloseTemplet();
            else if (result == DialogResult.Cancel)
                return SystemConst.ReturnValue.CANCEL;
            return shRet != SystemConst.ReturnValue.OK ? SystemConst.ReturnValue.CANCEL : shRet;
        }
    }
}