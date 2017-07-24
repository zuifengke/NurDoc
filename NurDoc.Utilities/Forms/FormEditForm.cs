// **************************************************************
// 护理电子病历系统之表单弹出子表单时用来承载子表单的窗口
// Creator:YangMingkun  Date:2012-9-28
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

namespace Heren.NurDoc.Utilities.Forms
{
    public partial class FormEditForm : HerenForm
    {
        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// 获取当前打开的表单的类型信息
        /// </summary>
        [Browsable(false)]
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
        }

        /// <summary>
        /// 获取当前窗口中的表单编辑器
        /// </summary>
        [Browsable(false)]
        public FormControl FormEditor
        {
            get { return this.formControl1; }
        }

        public FormEditForm()
            : this(null)
        {
        }

        public FormEditForm(DocTypeInfo docTypeInfo)
        {
            this.InitializeComponent();
            this.m_docTypeInfo = docTypeInfo;
            if (this.m_docTypeInfo != null)
                this.Text = this.m_docTypeInfo.DocTypeName;
            this.Icon = Properties.Resources.FormEdit;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.HandleUILayout();
            this.formControl1.OpenForm(this.m_docTypeInfo);
            if (this.formControl1.Controls.Count > 0)
            {
                Size size = this.formControl1.Controls[0].Size;
                size.Width += this.Width - this.formControl1.Width;
                size.Width += SystemInformation.VerticalScrollBarWidth;
                size.Height += this.Height - this.formControl1.Height;
                size.Height += SystemInformation.HorizontalScrollBarHeight;
                GlobalMethods.UI.LocateScreenCenter(this, size);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void HandleUILayout()
        {
            string szDocTypeID = null;
            if (this.m_docTypeInfo != null)
                szDocTypeID = this.m_docTypeInfo.DocTypeID;
            bool bIsPrintable =
                FormCache.Instance.IsFormPrintable(szDocTypeID);
            if (this.btnPrint.Visible != bIsPrintable)
                this.btnPrint.Visible = bIsPrintable;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            this.formControl1.PrintForm(false);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_docTypeInfo == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("无法保存文档,当前文档信息非法!");
                return;
            }
            if (this.formControl1.SaveForm())
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}
