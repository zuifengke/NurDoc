// ***********************************************************
// ������Ӳ���ϵͳ,ȫ�ֹ���ģ��������.
// ���ڼ��غ�̨ά���ĸ����Զ���ȫ�ֹ��ܲ��
// Creator:YangMingkun  Date:2012-9-5
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.Common.Report;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class PluginDocumentForm : DockContentBase
    {
        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// ��ȡ�����õ�ǰ�ĵ��б��ڹ������ĵ����Ͷ���
        /// </summary>
        [Browsable(false)]
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
            set { this.m_docTypeInfo = value; }
        }

        /// <summary>
        /// ��ȡ��ǰ���������Ƿ��Ѿ����޸�
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get { return this.formControl1.IsModified; }
        }

        private bool m_IsRostering = false;

        /// <summary>
        /// ��ȡ�����õ�ǰ�Ƿ�Ϊ��ʿ�Űര��
        /// </summary>
        public bool IsRostering
        {
            get { return this.m_IsRostering; }
            set { this.m_IsRostering = value; }
        }

        private string m_szDeptCode = string.Empty;

        public string DeptCode
        {
            get { return this.m_szDeptCode; }
            set { this.m_szDeptCode = value; }
        }

        public PluginDocumentForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
        }

        protected override void OnPatientTableChanged()
        {
            base.OnPatientTableChanged();
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientTableChanged)
                this.RefreshView();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(this.m_docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("ȫ�ֹ���ģ��������ʧ��!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.formControl1.Load(this.m_docTypeInfo, byteFormData);
            this.formControl1.GetFormData("ˢ�������б�");
            this.formControl1.UpdateFormData("ˢ���Ű���Ϣ", this.m_szDeptCode);
            this.m_IsRostering = false;
            this.formControl1.MaximizeEditRegion();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            bool bUserChanged = this.UserChanged;
            bool bPatientTableChanged = this.PatientTableChanged;
            base.RefreshView();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.formControl1.UpdateFormData("ˢ����ͼ", null);
            if (bUserChanged)
                this.formControl1.UpdateFormData("�л�ϵͳ�û�", null);
            if (bPatientTableChanged)
                this.formControl1.UpdateFormData("ˢ�²����б�", null);
            if (this.m_IsRostering)
            {
                this.formControl1.UpdateFormData("ˢ���Ű���Ϣ", this.m_szDeptCode);
                this.m_IsRostering = false;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override bool CheckModifyState()
        {
            this.formControl1.EndEdit();
            if (!this.IsModified)
                return false;

            DialogResult result = MessageBoxEx.ShowQuestion("��ǰ�����Ѿ����޸�,�Ƿ񱣴棿");
            if (result == DialogResult.Yes)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                object param = string.Empty;
                bool success = this.formControl1.SaveFormData(ref param);
                if (success)
                    this.formControl1.IsModified = false;
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return !success;
            }
            return (result == DialogResult.Cancel);
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "�򿪲��˴���")
            {
                if (this.MainFrame != null)
                    this.MainFrame.ShowPatientPageForm(e.Param);
            }
        }
    }
}