// **************************************************************
// ������Ӳ���ϵͳword�ĵ�������
// Creator:Ycc  Date:2015-7-16
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
using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.PatPage.WordDocument
{
    internal partial class WordDocForm : DockContentBase
    {
        public WordDocForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.WordDocument;
            m_WindowName = SystemContext.Instance.WindowName.GetName(text);
            this.Text = m_WindowName;
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 100);
        }

        private string m_WindowName = string.Empty;

        private string m_selectedContent = string.Empty;

        /// <summary>
        /// ��ȡ��ǰѡ�е�ģ������
        /// </summary>
        [Browsable(false)]
        [Description("��ȡ��ǰѡ�е�ģ������")]
        public string SelectedContent
        {
            get { return this.m_selectedContent; }
        }

        private WordDocTreeForm m_WordDocTreeForm = null;

        /// <summary>
        /// ��ȡ�ı�ģ�������б���
        /// </summary>
        [Browsable(false)]
        private WordDocTreeForm WordDocTreeForm
        {
            get
            {
                if (this.m_WordDocTreeForm == null
                    || this.m_WordDocTreeForm.IsDisposed)
                {
                    this.m_WordDocTreeForm = new WordDocTreeForm(this);
                    this.m_WordDocTreeForm.TopLevel = false;
                    this.m_WordDocTreeForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_WordDocTreeForm.Dock = DockStyle.Fill;
                    this.m_WordDocTreeForm.Parent = this.splitContainer1.Panel1;
                }
                return this.m_WordDocTreeForm;
            }
        }

        private WordDocEditForm m_WordDocEditForm = null;

        /// <summary>
        /// ��ȡ�ı�ģ�����ݱ༭����
        /// </summary>
        [Browsable(false)]
        private WordDocEditForm WordDocEditForm
        {
            get
            {
                if (this.m_WordDocEditForm == null
                    || this.m_WordDocEditForm.IsDisposed)
                {
                    this.m_WordDocEditForm = new WordDocEditForm(this);
                    this.m_WordDocEditForm.TopLevel = false;
                    this.m_WordDocEditForm.FormBorderStyle = FormBorderStyle.None;
                    this.m_WordDocEditForm.Dock = DockStyle.Fill;
                    this.m_WordDocEditForm.Parent = this.splitContainer1.Panel2;
                }
                return this.m_WordDocEditForm;
            }
        }

        public WordDocForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.WordDocTreeForm.Show();
            this.WordDocEditForm.Show();
            this.WordDocTreeForm.Focus();
            Application.DoEvents();

            //���ݵ�ǰ�û�,ˢ���ı�ģ���б�
            string szOldUser = this.WordDocTreeForm.Tag as string;
            string szCurrUser = null;
            if (SystemContext.Instance.LoginUser != null)
                szCurrUser = SystemContext.Instance.LoginUser.ID;
            if (szCurrUser != szOldUser)
            {
                this.WordDocTreeForm.Tag = szCurrUser;
                this.WordDocTreeForm.RefreshTempletList();
            }
        }

        /// <summary>
        /// �رյ�ǰ�ĵ�
        /// </summary>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public void CloseDocument()
        {
            short shRet = this.m_WordDocEditForm.CloseDocument();
        }

        internal void ShowStatusMassage(string szMessage)
        {
            this.lblStatusInfo.Text = szMessage;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.Close();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ˢ����������ʾ����
        /// </summary>
        private void RefreshFormText()
        {
            string szFormText = m_WindowName;
            WordTempInfo templetInfo = null;
            if (this.WordDocEditForm != null)
                templetInfo = this.WordDocEditForm.WordTempInfo;
            if (templetInfo != null)
                szFormText = string.Concat(szFormText, " - ", templetInfo.TempletName);
            if (this.Text != szFormText)
                this.Text = szFormText;
        }

        /// <summary>
        /// ���ָ��ģ���Ƿ��Ѿ���,����Ѿ������伤��
        /// </summary>
        /// <param name="templetInfo">�ı�ģ����Ϣ</param>
        /// <returns>true-�Ǵ򿪵�;false-δ��</returns>
        private bool IsTempletOpened(WordTempInfo templetInfo)
        {
            if (this.WordDocEditForm == null || this.WordDocEditForm.IsDisposed)
                return false;

            if (this.WordDocEditForm.WordTempInfo == null)
                return false;

            if (templetInfo == null)
                return false;

            //����ı�ģ���Ѿ���,�����ظ���
            WordTempInfo currTempletInfo = this.WordDocEditForm.WordTempInfo;
            if (currTempletInfo != null && currTempletInfo.TempletID == templetInfo.TempletID)
                return true;
            return false;
        }

        /// <summary>
        /// ���ָ����ģ���Ƿ�����ǰ�û��޸�
        /// </summary>
        /// <param name="templetInfo">�ı�ģ����Ϣ</param>
        /// <returns>bool</returns>
        internal bool IsAllowModifyTemplet(WordTempInfo templetInfo)
        {
            if (SystemContext.Instance.LoginUser == null || templetInfo == null)
                return false;

            //���ҹ���������ߵ�ģ�岻�����޸�
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
        /// ���ز���ָ�����ı�ģ��
        /// </summary>
        /// <param name="templetInfo">wordģ����Ϣ</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short OpenTemplet(WordTempInfo templetInfo)
        {
            if (templetInfo == null || templetInfo.IsFolder || SystemContext.Instance.LoginUser == null)
            {
                this.m_WordDocEditForm.CloseWordApplication();
                return SystemConst.ReturnValue.OK;
            }
            if (this.CheckTempletModify() != SystemConst.ReturnValue.OK)
                return SystemConst.ReturnValue.CANCEL;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            short shRet = this.WordDocEditForm.CloseWordApplication();
            this.WordDocEditForm.WordTempInfo = templetInfo;
            this.RefreshFormText();
            this.Update();

            shRet = this.WordDocEditForm.OpenTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return shRet;
        }

        /// <summary>
        /// ���ز���ָ�����ı�ģ��
        /// </summary>
        /// <param name="templetInfo">wordģ����Ϣ</param>
        /// <param name="filePath">�ĵ�·��</param>
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
            short shRet = this.WordDocEditForm.CloseDocument();
            this.WordDocEditForm.WordTempInfo = templetInfo;
            this.WordDocEditForm.szFilePath = filePath;
            this.RefreshFormText();
            this.Update();

            shRet = this.WordDocEditForm.OpenDocument(filePath);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return shRet;
        }

        /// <summary>
        /// ��鵱ǰģ���Ƿ��Ѿ����޸�
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short CheckTempletModify()
        {
            if (this.WordDocEditForm == null || this.WordDocEditForm.IsDisposed)
                return SystemConst.ReturnValue.OK;

            if (!this.WordDocEditForm.IsModified)
                return SystemConst.ReturnValue.OK;

            WordTempInfo templetInfo = this.WordDocEditForm.WordTempInfo;
            if (SystemContext.Instance.LoginUser == null || templetInfo == null || templetInfo.IsFolder)
                return SystemConst.ReturnValue.OK;

            if (!this.IsAllowModifyTemplet(templetInfo))
                return SystemConst.ReturnValue.OK;

            DialogResult result = DialogResult.OK;
            result = MessageBoxEx.ShowQuestion(string.Format("�Ƿ񱣴�ԡ�{0}���ı�ģ����޸ģ�", templetInfo.TempletName));

            short shRet = SystemConst.ReturnValue.OK;
            if (result == DialogResult.Yes)
                shRet = this.WordDocEditForm.SaveTemplet();
            else if (result == DialogResult.No)
                this.WordDocEditForm.CloseTemplet();
            else if (result == DialogResult.Cancel)
                return SystemConst.ReturnValue.CANCEL;
            return shRet != SystemConst.ReturnValue.OK ? SystemConst.ReturnValue.CANCEL : shRet;
        }

        private void WordDocForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.m_WordDocEditForm.CloseWordApplication();
            //ɾ����ʱ�ļ�
            string szTempPath = Application.StartupPath + @"\Templets\WordTemp" + @"\";
            this.Dispose();
            try
            {
                FileInfo[] fileInfos = GlobalMethods.IO.GetFiles(szTempPath);
                foreach (FileInfo item in fileInfos)
                {
                    GlobalMethods.IO.DeleteFile(item.FullName);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("�ĵ�ģ��ر�ʧ��!", ex.Message);
            }
        }
    }
}