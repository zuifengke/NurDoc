// **************************************************************
// ������Ӳ���ϵͳ����ģ��֮�ı�ģ��༭����
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
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Templet
{
    internal partial class TempletEditForm : Form
    {
        private TextTempletInfo m_TempletInfo = null;

        /// <summary>
        /// ��ȡ�����õ�ǰģ�����Ϣ
        /// </summary>
        [Browsable(false)]
        public TextTempletInfo TempletInfo
        {
            get { return this.m_TempletInfo; }
            set { this.m_TempletInfo = value; }
        }

        private bool m_bIsModified = false;

        /// <summary>
        /// ��ȡ�����õ�ǰģ���Ƿ����޸�
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get { return this.m_bIsModified; }
            set { this.m_bIsModified = value; }
        }

        private TextTempletForm m_templetForm = null;

        public TempletEditForm()
            : this(null)
        {
        }

        public TempletEditForm(TextTempletForm templetForm)
        {
            this.InitializeComponent();
            this.m_templetForm = templetForm;
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.txtTempletContent.Cut();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.txtTempletContent.Copy();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.txtTempletContent.Paste();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_templetForm != null && !this.m_templetForm.IsDisposed)
                this.m_templetForm.ImportSelectedContent();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SaveTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void txtTempletContent_TextChanged(object sender, EventArgs e)
        {
            this.IsModified = true;
        }

        /// <summary>
        /// ��ȡ��ǰ�ı�ģ����ѡ�е�����
        /// </summary>
        /// <returns>�ı�����</returns>
        public string GetSelectedContent()
        {
            return this.txtTempletContent.SelectedText;
        }

        /// <summary>
        /// ѡ�е�ǰ�ı�ģ������������
        /// </summary>
        public void SelectAll()
        {
            this.txtTempletContent.SelectAll();
        }

        /// <summary>
        /// �رյ�ǰģ��
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short CloseTemplet()
        {
            this.m_TempletInfo = null;
            this.txtTempletContent.Text = null;
            this.IsModified = false;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ���ز���ָ�����ı�ģ��
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short OpenTemplet()
        {
            this.IsModified = false;
            if (this.m_TempletInfo == null)
                return SystemConst.ReturnValue.FAILED;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            string szTempletID = this.m_TempletInfo.TempletID;
            byte[] byteTempletData = null;
            short shRet = TempletService.Instance.GetTextTemplet(szTempletID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("��{0}���ı�ģ������ʧ��!", this.m_TempletInfo.TempletName));
                return SystemConst.ReturnValue.FAILED;
            }

            string szTempletContent = string.Empty;
            if (!GlobalMethods.Convert.BytesToString(byteTempletData, ref szTempletContent))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("��{0}���ı�ģ�����ʧ��!", this.m_TempletInfo.TempletName));
                return SystemConst.ReturnValue.FAILED;
            }
            this.txtTempletContent.Text = szTempletContent;
            this.IsModified = false;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ���浱ǰ��ʾ���ı�ģ��
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short SaveTemplet()
        {
            if (this.m_TempletInfo == null || this.m_TempletInfo.IsFolder)
            {
                MessageBoxEx.Show("��ǰ��ʾ��ģ�廹û�б�������,���ȴ���!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (!this.m_templetForm.IsAllowModifyTemplet(this.m_TempletInfo))
            {
                MessageBoxEx.Show("��û��Ȩ�ޱ��浱ǰģ��!");
                return SystemConst.ReturnValue.FAILED;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //�����ĵ�����
            string szTempletContent = this.txtTempletContent.Text;
            byte[] byteTempletData = null;
            if (!GlobalMethods.Convert.StringToBytes(szTempletContent, ref byteTempletData))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return SystemConst.ReturnValue.EXCEPTION;
            }

            //�ύ��������
            this.Update();
            short shRet = TempletService.Instance.UpdateTextTemplet(this.m_TempletInfo, byteTempletData);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
                this.IsModified = false;
            else
                MessageBoxEx.Show(string.Format("��{0}���ı�ģ�屣��ʧ��!", this.m_TempletInfo.TempletName));
            return shRet;
        }
    }
}