// **************************************************************
// 护理电子病历系统公用模块之文本模板编辑窗口
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
        /// 获取或设置当前模板的信息
        /// </summary>
        [Browsable(false)]
        public TextTempletInfo TempletInfo
        {
            get { return this.m_TempletInfo; }
            set { this.m_TempletInfo = value; }
        }

        private bool m_bIsModified = false;

        /// <summary>
        /// 获取或设置当前模板是否已修改
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
        /// 获取当前文本模板中选中的内容
        /// </summary>
        /// <returns>文本内容</returns>
        public string GetSelectedContent()
        {
            return this.txtTempletContent.SelectedText;
        }

        /// <summary>
        /// 选中当前文本模板中所有内容
        /// </summary>
        public void SelectAll()
        {
            this.txtTempletContent.SelectAll();
        }

        /// <summary>
        /// 关闭当前模板
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
        /// 下载并打开指定的文本模板
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
                MessageBoxEx.Show(string.Format("“{0}”文本模板下载失败!", this.m_TempletInfo.TempletName));
                return SystemConst.ReturnValue.FAILED;
            }

            string szTempletContent = string.Empty;
            if (!GlobalMethods.Convert.BytesToString(byteTempletData, ref szTempletContent))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("“{0}”文本模板加载失败!", this.m_TempletInfo.TempletName));
                return SystemConst.ReturnValue.FAILED;
            }
            this.txtTempletContent.Text = szTempletContent;
            this.IsModified = false;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存当前显示的文本模板
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short SaveTemplet()
        {
            if (this.m_TempletInfo == null || this.m_TempletInfo.IsFolder)
            {
                MessageBoxEx.Show("当前显示的模板还没有被创建过,请先创建!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (!this.m_templetForm.IsAllowModifyTemplet(this.m_TempletInfo))
            {
                MessageBoxEx.Show("您没有权限保存当前模板!");
                return SystemConst.ReturnValue.FAILED;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //生成文档数据
            string szTempletContent = this.txtTempletContent.Text;
            byte[] byteTempletData = null;
            if (!GlobalMethods.Convert.StringToBytes(szTempletContent, ref byteTempletData))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return SystemConst.ReturnValue.EXCEPTION;
            }

            //提交到服务器
            this.Update();
            short shRet = TempletService.Instance.UpdateTextTemplet(this.m_TempletInfo, byteTempletData);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
                this.IsModified = false;
            else
                MessageBoxEx.Show(string.Format("“{0}”文本模板保存失败!", this.m_TempletInfo.TempletName));
            return shRet;
        }
    }
}