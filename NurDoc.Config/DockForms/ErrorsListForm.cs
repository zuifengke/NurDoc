// ***********************************************************
// ���������ù���ϵͳ,ģ��ű������д���ʱ,�����б���ʾ����.
// Author : YangMingkun, Date : 2012-6-6
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.NurDoc.Config.DockForms;

namespace Heren.NurDoc.Config.DockForms
{
    internal partial class ErrorsListForm : DockContentBase
    {
        #region"������Ϣ����"
        public class CompileError
        {
            private string m_fileName = string.Empty;

            /// <summary>
            /// ��ȡ�����ô����Ӧ��Դ�����ļ�
            /// </summary>
            public string FileName
            {
                get { return this.m_fileName; }
                set { this.m_fileName = value; }
            }

            private int m_line = 0;

            /// <summary>
            /// ��ȡ�����ô����Ӧ���к�
            /// </summary>
            public int Line
            {
                get { return this.m_line; }
                set { this.m_line = value; }
            }

            private int m_column = 0;

            /// <summary>
            /// ��ȡ�����ô����Ӧ���к�
            /// </summary>
            public int Column
            {
                get { return this.m_column; }
                set { this.m_column = value; }
            }

            private string m_errorNumber = string.Empty;

            /// <summary>
            /// ��ȡ�����ô����Ӧ�ı��
            /// </summary>
            public string ErrorNumber
            {
                get { return this.m_errorNumber; }
                set { this.m_errorNumber = value; }
            }

            private string m_errorText = string.Empty;

            /// <summary>
            /// ��ȡ�����ô����Ӧ���ı�
            /// </summary>
            public string ErrorText
            {
                get { return this.m_errorText; }
                set { this.m_errorText = value; }
            }

            private bool m_isWarning = false;

            /// <summary>
            /// ��ȡ�������Ƿ��Ǿ������
            /// </summary>
            public bool IsWarning
            {
                get { return this.m_isWarning; }
                set { this.m_isWarning = value; }
            }
        }
        #endregion

        private CompileError[] m_compileErrors = null;

        /// <summary>
        /// ��ȡ�����ñ�������б�
        /// </summary>
        internal CompileError[] CompileErrors
        {
            get { return this.m_compileErrors; }
            set { this.m_compileErrors = value; }
        }

        private IScriptEditForm m_ScriptEditForm = null;

        /// <summary>
        /// ��ȡ�����õ�ǰ����������ĵ�����
        /// </summary>
        internal IScriptEditForm ScriptEditForm
        {
            get { return this.m_ScriptEditForm; }
            set { this.m_ScriptEditForm = value; }
        }

        public ErrorsListForm(MainForm mainForm)
            : base(mainForm)
        {
            this.ShowHint = DockState.DockBottom;
            this.DockAreas = DockAreas.DockBottom | DockAreas.DockRight;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.UpdateBounds();
            this.Icon = Properties.Resources.ScriptErrorIcon;
        }

        public override void OnRefreshView()
        {
            base.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadErrorList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void LoadErrorList()
        {
            this.listView1.Items.Clear();
            if (this.m_compileErrors == null)
                return;
            foreach (CompileError error in this.m_compileErrors)
            {
                if (error == null)
                    continue;
                ListViewItem item = new ListViewItem();
                item.Tag = error;
                item.ImageIndex = error.IsWarning ? 0 : 1;
                item.SubItems.Add(error.ErrorText);
                item.SubItems.Add(error.Line.ToString());
                item.SubItems.Add(error.Column.ToString());
                this.listView1.Items.Add(item);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count <= 0)
                return;
            CompileError error = this.listView1.SelectedItems[0].Tag as CompileError;
            if (error == null)
                return;
            if (this.m_ScriptEditForm != null && !this.m_ScriptEditForm.IsDisposed)
                this.m_ScriptEditForm.LocateTo(error.Line, error.Column);
        }
    }
}