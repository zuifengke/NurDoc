// ***********************************************************
// 护理电子病历系统,护理评估单综合查询管理窗口.
// Creator:XiaoPingping  Date:2013-5-8
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.Controls.VirtualTreeView;
using Heren.Common.DockSuite;
using Heren.Common.Forms;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.PatPage
{
    internal partial class IntegratedQueryForm : DockContentBase
    {
        private PluginDocumentForm m_pluginDocumentForm = null;

        public IntegratedQueryForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.IntegrateQuery;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 700);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.virtualTree1.ImageList.Images.Clear();
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.NursingDoc);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocTypeList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private bool LoadDocTypeList()
        {
            this.virtualTree1.Nodes.Clear();
            Application.DoEvents();

            string szApplyEnv = ServerData.DocTypeApplyEnv.INTEGRATE_QUERY;
            List<DocTypeInfo> lstDocTypeInfo = FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfo == null)
            {
                MessageBoxEx.ShowError("查询统计表单列表下载失败!");
                return false;
            }

            //先创建节点哈希列表
            Dictionary<string, VirtualNode> nodeTable =
                new Dictionary<string, VirtualNode>();
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfo)
            {
                if (docTypeInfo == null)
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;

                VirtualNode docTypeNode = new VirtualNode();
                docTypeNode.Tag = docTypeInfo;
                docTypeNode.Text = docTypeInfo.DocTypeName;

                if (!docTypeInfo.IsFolder)
                    docTypeNode.ImageIndex = 2;
                else
                    docTypeNode.ImageIndex = 0;
                docTypeNode.ImageIndex = docTypeNode.ImageIndex;

                if (!nodeTable.ContainsKey(docTypeInfo.DocTypeID))
                    nodeTable.Add(docTypeInfo.DocTypeID, docTypeNode);
            }

            //将节点连接起来,添加到树中
            foreach (KeyValuePair<string, VirtualNode> pair in nodeTable)
            {
                if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    continue;

                DocTypeInfo docTypeInfo = pair.Value.Tag as DocTypeInfo;
                if (docTypeInfo == null)
                    continue;

                if (string.IsNullOrEmpty(docTypeInfo.ParentID))
                {
                    this.virtualTree1.Nodes.Add(pair.Value);
                }
                else
                {
                    if (!nodeTable.ContainsKey(docTypeInfo.ParentID))
                        continue;
                    VirtualNodeList nodes = nodeTable[docTypeInfo.ParentID].Nodes;
                    nodes.Add(pair.Value);
                }
            }
            nodeTable.Clear();
            if (SystemContext.Instance.GetConfig(SystemConst.ConfigKey.SPECIAL_NURSING_EXPAND, true))
                this.virtualTree1.ExpandAll();
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            this.mnuExpandAll.Checked = !this.mnuExpandAll.Checked;
            if (this.mnuExpandAll.Checked)
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();
            SystemContext.Instance.WriteConfig(SystemConst.ConfigKey.STATISTIC_EXPAND
                , this.mnuExpandAll.Checked.ToString());
        }

        private void virtualTree1_NodeMouseClick(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;

            //检查是否已经打开
            DocTypeInfo docTypeInfo = e.Node.Tag as DocTypeInfo;
            if (docTypeInfo == null || docTypeInfo.IsFolder)
                return;

            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            this.ShowPluginDocumentForm(docTypeInfo);
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }

        private void ShowPluginDocumentForm(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || docTypeInfo.IsFolder)
                return;
            bool bManualLoadDocTypeInfo = true;
            if (this.m_pluginDocumentForm == null || this.m_pluginDocumentForm.IsDisposed)
            {
                bManualLoadDocTypeInfo = false;
                this.m_pluginDocumentForm = new PluginDocumentForm(null);
                this.m_pluginDocumentForm.TopLevel = false;
                this.m_pluginDocumentForm.FormBorderStyle = FormBorderStyle.None;
                this.m_pluginDocumentForm.Dock = DockStyle.Fill;
                this.m_pluginDocumentForm.Padding = new Padding(0);
                this.m_pluginDocumentForm.Parent = this.splitContainer1.Panel2;
            }
            this.m_pluginDocumentForm.DocTypeInfo = docTypeInfo;
            this.m_pluginDocumentForm.Visible = true;
            this.m_pluginDocumentForm.BringToFront();

            if (bManualLoadDocTypeInfo) this.m_pluginDocumentForm.LoadDocTypeInfo();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = this.virtualTree1.Bounds;
            using (Pen pen = new Pen(Color.DodgerBlue))
            {
                e.Graphics.DrawRectangle(pen, rect.X - 1, rect.Y - 2, rect.Width + 2, rect.Height + 2);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.mnuExpandAll.Checked = SystemContext.Instance.GetConfig(SystemConst.ConfigKey.STATISTIC_EXPAND, true);
        }
    }
}