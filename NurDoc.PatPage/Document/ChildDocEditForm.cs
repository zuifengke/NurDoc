// ***********************************************************
// 护理电子病历系统,病人窗口之表单弹出的各种子表单的编辑窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Forms.Editor;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage
{
    internal partial class ChildDocEditForm : HerenForm
    {
        private DocumentInfo m_document = null;

        /// <summary>
        /// 获取当前表单的文档信息
        /// </summary>
        [Browsable(false)]
        public DocumentInfo Document
        {
            get { return this.m_document; }
        }

        /// <summary>
        /// 获取当前窗口的表单编辑器
        /// </summary>
        [Browsable(false)]
        public DocumentControl FormEditor
        {
            get { return this.documentControl1; }
        }

        public ChildDocEditForm()
            : this(null)
        {
        }

        public ChildDocEditForm(DocumentInfo document)
        {
            this.InitializeComponent();
            this.m_document = document;
            if (this.m_document != null)
                this.Text = this.m_document.DocTitle;
            this.Icon = Properties.Resources.FormEdit;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.HandleUILayout();
            this.documentControl1.OpenDocument(this.m_document);
            if (this.documentControl1.Controls.Count > 0)
            {
                Size size = this.documentControl1.Controls[0].Size;
                size.Width += this.Width - this.documentControl1.Width;
                size.Width += SystemInformation.VerticalScrollBarWidth;
                size.Height += this.Height - this.documentControl1.Height;
                size.Height += SystemInformation.HorizontalScrollBarHeight;
                GlobalMethods.UI.LocateScreenCenter(this, size);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void HandleUILayout()
        {
            string szDocTypeID = null;
            if (this.m_document != null)
                szDocTypeID = this.m_document.DocTypeID;
            bool bIsPrintable =
                FormCache.Instance.IsFormPrintable(szDocTypeID);
            if (this.btnPrint.Visible != bIsPrintable)
                this.btnPrint.Visible = bIsPrintable;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            if (this.m_document == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("无法保存文档,当前文档信息非法!");
                return;
            }
            if (this.documentControl1.EndEdit() && this.documentControl1.CheckFormData(null))
            {
                object param = null;
                this.documentControl1.SaveFormData(ref param);
                if (this.documentControl1.SaveDocument())
                {
                    this.m_document = this.documentControl1.Document;
                    this.documentControl1.PrintDocument();
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_document == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("无法保存文档,当前文档信息非法!");
                return;
            }
            if (this.documentControl1.EndEdit() && this.documentControl1.CheckFormData(null))
            {
                object param = null;
                this.documentControl1.SaveFormData(ref param);
                if (this.documentControl1.SaveDocument())
                {
                    this.m_document = this.documentControl1.Document;
                    this.DialogResult = DialogResult.OK;
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}
