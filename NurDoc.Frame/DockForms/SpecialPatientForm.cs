using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class SpecialPatientForm : DockContentBase
    {
        public SpecialPatientForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.DockAreas = DockAreas.Document;
            this.ShowHint = DockState.Document;

            string text = SystemContext.Instance.WindowName.SpecialPatient;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 200);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadSpecialPatientModule();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnActiveContentChanged()
        {
            base.OnActiveContentChanged();
            if (this.tabControl1.TabPages.Count <= 0)
                return;
            TabPage page = this.tabControl1.SelectedTab;
            DockContentBase content = page.Tag as DockContentBase;
            content.Show();
        }

        private void LoadSpecialPatientModule()
        {
            List<DockContentBase> contents = this.GetSpecialPatientModule();

            contents.Sort(new Comparison<DockContentBase>(this.CompareForm));
            foreach (DockContentBase content in contents)
            {
                TabPage tabPage = new System.Windows.Forms.TabPage();
                tabPage.Name = content.Name;
                tabPage.Text = content.Text;

                content.TopLevel = false;
                content.FormBorderStyle = FormBorderStyle.None;

                content.AutoScroll = true;
                tabPage.Controls.Add(content);
                content.Dock = DockStyle.Fill;
                if (content == contents[0])
                    content.Show();
                tabPage.Tag = content;
                this.tabControl1.Controls.Add(tabPage);
            }
        }

        /// <summary>
        /// 对窗口显示顺序进行排序
        /// </summary>
        /// <param name="content1">窗口1</param>
        /// <param name="content2">窗口2</param>
        /// <returns>比较结果</returns>
        private int CompareForm(DockContentBase content1, DockContentBase content2)
        {
            if (content1 == null && content2 != null)
                return -1;
            if (content1 != null && content2 == null)
                return 1;
            if (content1 == null && content2 == null)
                return 0;
            return content1.Index - content2.Index;
        }

        private List<DockContentBase> GetSpecialPatientModule()
        {
            List<DockContentBase> contents = new List<DockContentBase>();

            string szApply = ServerData.DocTypeApplyEnv.SPECIAL_PATIENT;
            List<DocTypeInfo> lstDocTypeInfo = FormCache.Instance.GetDocTypeList(szApply);
            if (lstDocTypeInfo == null)
            {
                MessageBoxEx.ShowError("加载专科一览表表单失败!");
                return contents;
            }

            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfo)
            {
                if (docTypeInfo == null
                    || GlobalMethods.Misc.IsEmptyString(docTypeInfo.DocTypeID))
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (docTypeInfo.IsFolder)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                PluginDocumentForm pluginDocumentForm = new PluginDocumentForm(this.MainFrame);
                pluginDocumentForm.DocTypeInfo = docTypeInfo;
                pluginDocumentForm.Text = docTypeInfo.DocTypeName;
                pluginDocumentForm.Index = docTypeInfo.DocTypeNo;
                contents.Add(pluginDocumentForm);
            }
            return contents;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnActiveContentChanged();
        }
    }
}