// **************************************************************
// ������Ӳ���ϵͳ����ģ��֮�ı�ģ��������
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
        /// ��ȡ��ǰѡ�е�ģ������
        /// </summary>
        [Browsable(false)]
        [Description("��ȡ��ǰѡ�е�ģ������")]
        public string SelectedContent
        {
            get { return this.m_selectedContent; }
        }

        private WordTempTreeForm m_WordTempTreeForm = null;

        /// <summary>
        /// ��ȡ�ı�ģ�������б���
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
        /// ��ȡ�ı�ģ�����ݱ༭����
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

            //���ݵ�ǰ�û�,ˢ���ı�ģ���б�
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
        /// �رյ�ǰ�ĵ�
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
        /// ˢ����������ʾ����
        /// </summary>
        private void RefreshFormText()
        {
            string szFormText = "����wordģ�������";
            WordTempInfo templetInfo = null;
            if (this.WordTempEditForm != null)
                templetInfo = this.WordTempEditForm.WordTempInfo;
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
            if (this.WordTempEditForm == null || this.WordTempEditForm.IsDisposed)
                return false;

            if (this.WordTempEditForm.WordTempInfo == null)
                return false;

            if (templetInfo == null)
                return false;

            //����ı�ģ���Ѿ���,�����ظ���
            WordTempInfo currTempletInfo = this.WordTempEditForm.WordTempInfo;
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
            this.WordTempEditForm.WordTempInfo = templetInfo;
            this.WordTempEditForm.szFilePath = filePath;
            this.RefreshFormText();
            this.Update();

            short shRet = this.WordTempEditForm.OpenDocument(filePath);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return shRet;
        }

        /// <summary>
        /// ���뵱ǰѡ�е�ģ�嵽��ǰ�����ĵ�
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
        /// ��鵱ǰģ���Ƿ��Ѿ����޸�
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
            result = MessageBoxEx.ShowQuestion(string.Format("�Ƿ񱣴�ԡ�{0}���ı�ģ����޸ģ�", templetInfo.TempletName));

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
            //ɾ����ʱ�ļ�
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