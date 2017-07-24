using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Controls;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Utilities.Forms
{
    public partial class DocumentReaderForm : HerenForm
    {
        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// 获取当前打开的表单的类型信息
        /// </summary>
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
        }

        private byte[] m_data = null;

        public DocumentReaderForm(DocTypeInfo docTypeInfo, byte[] data)
        {
            this.InitializeComponent();
            this.m_docTypeInfo = docTypeInfo;
            if (this.m_docTypeInfo != null)
                this.Text = this.m_docTypeInfo.DocTypeName;
            this.m_data = data;
            this.Icon = Properties.Resources.FormEdit;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.formControl1.Load(this.m_docTypeInfo, this.m_data);
            if (this.formControl1.Controls.Count > 0)
            {
                Size size = this.formControl1.Controls[0].Size;
                size.Width += this.Width - this.formControl1.Width;
                size.Width += SystemInformation.VerticalScrollBarWidth;
                size.Height += this.Height - this.formControl1.Height;
                size.Height += SystemInformation.HorizontalScrollBarHeight;
                this.formControl1.Controls[0].Enabled = false;
                GlobalMethods.UI.LocateScreenCenter(this, size);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}