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
using System.IO;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Templet
{
    public partial class WordTempForm : HerenForm
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

        private WordTempTreeForm m_WordTempTreeForm = null;

        /// <summary>
        /// 获取文本模板树形列表窗口
        /// </summary>
        [Browsable(false)]
        private WordTempTreeForm WordTempTreeForm
        {
            get
            {
                if (this.m_WordTempTreeForm == null
                    || this.m_WordTempTreeForm.IsDisposed)
                {
                    this.m_WordTempTreeForm = new WordTempTreeForm(this);
                    this.m_WordTempTreeForm.TopLevel = false;
                    this.m_WordTempTreeForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_WordTempTreeForm.Dock = DockStyle.Fill;
                    this.m_WordTempTreeForm.Parent = this.splitContainer1.Panel1;
                }
                return this.m_WordTempTreeForm;
            }
        }

        private WordTempEditForm m_WordTempEditForm = null;

        /// <summary>
        /// 获取文本模板内容编辑窗口
        /// </summary>
        [Browsable(false)]
        private WordTempEditForm WordTempEditForm
        {
            get
            {
                if (this.m_WordTempEditForm == null
                    || this.m_WordTempEditForm.IsDisposed)
                {
                    this.m_WordTempEditForm = new WordTempEditForm(this);
                    this.m_WordTempEditForm.TopLevel = false;
                    this.m_WordTempEditForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_WordTempEditForm.Dock = DockStyle.Fill;
                    this.m_WordTempEditForm.Parent = this.splitContainer1.Panel2;
                }
                return this.m_WordTempEditForm;
            }
        }

        public WordTempForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.WordTempTreeForm.Show();
            this.WordTempEditForm.Show();
            this.WordTempTreeForm.Focus();
            Application.DoEvents();

            //根据当前用户,刷新文本模板列表
            string szOldUser = this.WordTempTreeForm.Tag as string;
            string szCurrUser = null;
            if (SystemContext.Instance.LoginUser != null)
                szCurrUser = SystemContext.Instance.LoginUser.ID;
            if (szCurrUser != szOldUser)
            {
                this.WordTempTreeForm.Tag = szCurrUser;
                this.WordTempTreeForm.RefreshTempletList();
            }
        }

        /// <summary>
        /// 关闭当前文档
        /// </summary>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public void CloseDocument()
        {
            short shRet = this.m_WordTempEditForm.CloseDocument();
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
            string szFormText = "护理word模板管理器";
            WordTempInfo templetInfo = null;
            if (this.WordTempEditForm != null)
                templetInfo = this.WordTempEditForm.WordTempInfo;
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
        private bool IsTempletOpened(WordTempInfo templetInfo)
        {
            if (this.WordTempEditForm == null || this.WordTempEditForm.IsDisposed)
                return false;

            if (this.WordTempEditForm.WordTempInfo == null)
                return false;

            if (templetInfo == null)
                return false;

            //如果文本模板已经打开,则不再重复打开
            WordTempInfo currTempletInfo = this.WordTempEditForm.WordTempInfo;
            if (currTempletInfo != null && currTempletInfo.TempletID == templetInfo.TempletID)
                return true;
            return false;
        }

        /// <summary>
        /// 检查指定的模板是否允许当前用户修改
        /// </summary>
        /// <param name="templetInfo">文本模板信息</param>
        /// <returns>bool</returns>
        internal bool IsAllowModifyTemplet(WordTempInfo templetInfo)
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
        /// <param name="templetInfo">word模板信息</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short OpenTemplet(WordTempInfo templetInfo)
        {
            if (templetInfo == null || templetInfo.IsFolder)
                return SystemConst.ReturnValue.OK;
            if (SystemContext.Instance.LoginUser == null)
                return SystemConst.ReturnValue.OK;
            if (this.CheckTempletModify() != SystemConst.ReturnValue.OK)
                return SystemConst.ReturnValue.CANCEL;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.WordTempEditForm.WordTempInfo = templetInfo;
            this.RefreshFormText();
            this.Update();

            short shRet = this.WordTempEditForm.OpenTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return shRet;
        }

        /// <summary>
        /// 下载并打开指定的文本模板
        /// </summary>
        /// <param name="templetInfo">word模板信息</param>
        /// <param name="filePath">文档路径</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short OpenTemplet(WordTempInfo templetInfo, String filePath)
        {
            if (templetInfo == null || templetInfo.IsFolder)
                return SystemConst.ReturnValue.OK;

            if (SystemContext.Instance.LoginUser == null)
                return SystemConst.ReturnValue.OK;

            //if (this.IsTempletOpened(templetInfo))
            //    return SystemConst.ReturnValue.OK;

            if (this.CheckTempletModify() != SystemConst.ReturnValue.OK)
                return SystemConst.ReturnValue.CANCEL;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.WordTempEditForm.WordTempInfo = templetInfo;
            this.WordTempEditForm.szFilePath = filePath;
            this.RefreshFormText();
            this.Update();

            short shRet = this.WordTempEditForm.OpenDocument(filePath);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return shRet;
        }

        /// <summary>
        /// 导入当前选中的模板到当前病历文档
        /// </summary>
        internal void ImportSelectedContent()
        {
            if (this.WordTempEditForm == null || this.WordTempEditForm.IsDisposed)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            this.m_selectedContent = this.WordTempEditForm.GetSelectedContent();
            if (GlobalMethods.Misc.IsEmptyString(this.m_selectedContent))
                this.WordTempEditForm.SelectAll();
            this.m_selectedContent = this.WordTempEditForm.GetSelectedContent();

            this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 检查当前模板是否已经被修改
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short CheckTempletModify()
        {
            if (this.WordTempEditForm == null || this.WordTempEditForm.IsDisposed)
                return SystemConst.ReturnValue.OK;

            if (!this.WordTempEditForm.IsModified)
                return SystemConst.ReturnValue.OK;

            WordTempInfo templetInfo = this.WordTempEditForm.WordTempInfo;
            if (SystemContext.Instance.LoginUser == null || templetInfo == null || templetInfo.IsFolder)
                return SystemConst.ReturnValue.OK;

            if (!this.IsAllowModifyTemplet(templetInfo))
                return SystemConst.ReturnValue.OK;

            DialogResult result = DialogResult.OK;
            result = MessageBoxEx.ShowQuestion(string.Format("是否保存对“{0}”文本模板的修改？", templetInfo.TempletName));

            short shRet = SystemConst.ReturnValue.OK;
            if (result == DialogResult.Yes)
                shRet = this.WordTempEditForm.SaveTemplet();
            else if (result == DialogResult.No)
                this.WordTempEditForm.CloseTemplet();
            else if (result == DialogResult.Cancel)
                return SystemConst.ReturnValue.CANCEL;
            return shRet != SystemConst.ReturnValue.OK ? SystemConst.ReturnValue.CANCEL : shRet;
        }

        private void WordTempForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.m_WordTempEditForm.CloseWordApplication();
            //删除临时文件
            string szTempPath = Application.StartupPath + @"\Templets\WordTemp" + @"\";
            this.Dispose();
            FileInfo[] fileInfos = GlobalMethods.IO.GetFiles(szTempPath);
            foreach (FileInfo item in fileInfos)
            {
                GlobalMethods.IO.DeleteFile(item.FullName);
            }
        }
    }
}