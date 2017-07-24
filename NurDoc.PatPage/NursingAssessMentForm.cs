// ***********************************************************
// 护理电子病历系统,专科护理相关的评估单管理窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage
{
    internal partial class NursingAssessMentForm : DockContentBase
    {
        /// <summary>
        /// 获取或设置当前活动的文档类型对象
        /// </summary>
        [Browsable(false)]
        public DocTypeInfo DocTypeInfo
        {
            get
            {
                if (this.m_DocumentListEditForm == null)
                    return null;
                if (this.m_DocumentListEditForm.IsDisposed)
                    return null;
                return this.m_DocumentListEditForm.DocTypeInfo;
            }
        }

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.m_DocumentListEditForm == null)
                    return false;
                if (this.m_DocumentListEditForm.IsDisposed)
                    return false;
                return this.m_DocumentListEditForm.IsModified;
            }
        }

        /// <summary>
        /// 显示已写病历的列表的窗口
        /// </summary>
        private DocumentListEditForm m_DocumentListEditForm = null;

        /// <summary>
        /// 病历类型与节点对应关系表
        /// </summary>
        private Dictionary<string, VirtualNode> m_nodeTable = null;

        public NursingAssessMentForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text =  SystemContext.Instance.WindowName.NursingAssessMent;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 900);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.virtualTree1.ImageList.Images.Clear();
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderClose);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.NursingDoc);

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocTypeList();
            this.LoadModuleRemarksTemplet();
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_DocumentListEditForm != null && !this.m_DocumentListEditForm.IsDisposed
                && this.m_DocumentListEditForm.Visible)
            {
                this.ShowDocumentListEditForm(this.DocTypeInfo);
            }
            this.formControl1.UpdateFormData("刷新视图", null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnPatientInfoChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientInfoUpdated)
                this.RefreshView();
        }

        protected override void OnPatientInfoChanging(PatientInfoChangingEventArgs e)
        {
            if (this.IsModified)
                this.DockHandler.Activate();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.m_DocumentListEditForm == null)
                return false;
            if (this.m_DocumentListEditForm.IsDisposed)
                return false;
            return this.m_DocumentListEditForm.CheckModifyState();
        }

        /// <summary>
        /// 恢复模板树列表中所有节点的颜色和文本显示
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="bResetBold">是否还原粗体样式</param>
        /// <param name="bResetColor">是否还原文本颜色</param>
        /// <param name="bResetState">是否还原修改状态</param>
        private void ResetNodeTextAndColor(VirtualNode node
            , bool bResetBold, bool bResetColor, bool bResetState)
        {
            if (node != null)
            {
                if (bResetBold && node.Font != null)
                    node.Font = null;
                if (bResetColor && node.ForeColor == Color.Red)
                    node.ForeColor = Color.Black;
                if (bResetState && node.Text.EndsWith(" *"))
                    node.Text = node.Text.Remove(node.Text.Length - 2);
            }
            VirtualNodeList nodes = null;
            if (node == null)
                nodes = this.virtualTree1.Nodes;
            else
                nodes = node.Nodes;
            foreach (VirtualNode child in nodes)
            {
                this.ResetNodeTextAndColor(child, bResetBold, bResetColor, bResetState);
            }
        }

        /// <summary>
        /// 加载所有护理记录评估单列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadDocTypeList()
        {
            if (this.m_nodeTable == null)
                this.m_nodeTable = new Dictionary<string, VirtualNode>();
            this.m_nodeTable.Clear();

            this.virtualTree1.Nodes.Clear();
            Application.DoEvents();

            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_ASSESSMENT;
            string szApplyEnvName =
                ServerData.DocTypeApplyEnv.GetApplyEnvName(szApplyEnv);
            List<DocTypeInfo> lstDocTypeInfos =
                FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
            {
                MessageBoxEx.ShowError("护理记录模板列表下载失败!");
                return false;
            }
            if (lstDocTypeInfos == null || lstDocTypeInfos.Count <= 0)
                return true;

            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (docTypeInfo == null)
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;

                //不加载以类别名称开头的表单
                if (!string.IsNullOrEmpty(docTypeInfo.DocTypeName)
                    && docTypeInfo.DocTypeName.StartsWith(szApplyEnvName))
                    continue;

                VirtualNode docTypeNode = new VirtualNode();
                docTypeNode.Tag = docTypeInfo;
                docTypeNode.Text = docTypeInfo.DocTypeName;

                if (!docTypeInfo.IsFolder)
                    docTypeNode.ImageIndex = 2;
                else
                    docTypeNode.ImageIndex = 0;
                docTypeNode.ImageIndex = docTypeNode.ImageIndex;

                if (!this.m_nodeTable.ContainsKey(docTypeInfo.DocTypeID))
                    this.m_nodeTable.Add(docTypeInfo.DocTypeID, docTypeNode);
            }

            //将节点连接起来,添加到树中
            foreach (KeyValuePair<string, VirtualNode> pair in this.m_nodeTable)
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
                    if (!this.m_nodeTable.ContainsKey(docTypeInfo.ParentID))
                        continue;
                    VirtualNodeList nodes = this.m_nodeTable[docTypeInfo.ParentID].Nodes;
                    nodes.Add(pair.Value);
                }
            }

            string key = this.GetConfigKey();
            if (SystemContext.Instance.GetConfig(key, true))
                this.virtualTree1.ExpandAll();
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        /// <summary>
        /// 获取当前窗口显示的表单类型的应用环境名称
        /// </summary>
        /// <returns>应用环境名称</returns>
        private string GetApplyEnv()
        {
            return ServerData.DocTypeApplyEnv.NURSING_ASSESSMENT;
        }

        /// <summary>
        /// 获取默认是否展开所有节点的本地配置关键字
        /// </summary>
        /// <returns>本地配置关键字</returns>
        private string GetConfigKey()
        {
            return SystemConst.ConfigKey.NURSING_ASSESSMENT_EXPAND;
        }

        /// <summary>
        /// 加载当前窗口功能说明区域对应的表单模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadModuleRemarksTemplet()
        {
            string szApplyEnv = this.GetApplyEnv();
            string szTempletName =
                ServerData.DocTypeApplyEnv.GetApplyEnvName(szApplyEnv) + "功能说明";
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
                return false;

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
                return false;
            this.formControl1.Load(docTypeInfo, byteFormData);
            this.formControl1.MaximizeEditRegion();
            return true;
        }

        /// <summary>
        /// 显示指定病历类型对应的病历列表窗口
        /// </summary>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>是否执行成功</returns>
        private bool ShowDocumentListEditForm(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || docTypeInfo.IsFolder)
                return false;

            this.CreateDocumentListEditForm(docTypeInfo);
            this.m_DocumentListEditForm.RefreshView();
            return true;
        }
       
        /// <summary>
        /// 创建文档列表窗口
        /// </summary>
        /// <param name="docTypeInfo">文档类型</param>
        private void CreateDocumentListEditForm(DocTypeInfo docTypeInfo)
        {
            if (this.m_DocumentListEditForm == null || this.m_DocumentListEditForm.IsDisposed)
            {
                this.m_DocumentListEditForm = new DocumentListEditForm(null);
                this.m_DocumentListEditForm.TopLevel = false;
                this.m_DocumentListEditForm.FormBorderStyle = FormBorderStyle.None;
                this.m_DocumentListEditForm.Dock = DockStyle.Fill;
                this.m_DocumentListEditForm.Padding = new Padding(0);
                this.m_DocumentListEditForm.Parent = this;
            }
            this.m_DocumentListEditForm.DocTypeInfo = docTypeInfo;
            this.m_DocumentListEditForm.Visible = true;
            this.m_DocumentListEditForm.BringToFront();
        }

        /// <summary>
        /// 获取指定节点下面指定文档类型的模板节点
        /// </summary>
        /// <param name="parent">指定的父节点</param>
        /// <param name="docTypeInfo">指定的文档类型</param>
        /// <returns>可用的表单模板节点</returns>
        private VirtualNode GetTempletNode(VirtualNode parent, DocTypeInfo docTypeInfo)
        {
            if (parent != null)
            {
                DocTypeInfo parentDocTypeInfo = parent.Tag as DocTypeInfo;
                if (parentDocTypeInfo != null && !parentDocTypeInfo.IsFolder 
                    && docTypeInfo != null 
                    && docTypeInfo.DocTypeID == parentDocTypeInfo.DocTypeID)
                    return parent;
            }

            VirtualNodeList nodes =
                (parent == null) ? this.virtualTree1.Nodes : parent.Nodes;
            foreach (VirtualNode childNode in nodes)
            {
                VirtualNode node = this.GetTempletNode(childNode, docTypeInfo);
                if (node != null)
                    return node;
            }
            return null;
        }

        private void virtualTree1_NodeMouseClick(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;

            //检查是否已经打开
            DocTypeInfo docTypeInfo = e.Node.Tag as DocTypeInfo;
            if (docTypeInfo == null || docTypeInfo.IsFolder)
                return;
            if (this.DocTypeInfo != null && this.DocTypeInfo.DocTypeID == docTypeInfo.DocTypeID)
                return;

            //检查是否需要保存
            if (this.CheckModifyState())
                return;

            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            if (this.ShowDocumentListEditForm(docTypeInfo))
            {
                this.ResetNodeTextAndColor(null, true, false, false);
                e.Node.Font = new Font(this.virtualTree1.Font, FontStyle.Bold);
            }
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            string key = this.GetConfigKey();
            this.mnuExpandAll.Checked = SystemContext.Instance.GetConfig(key, true);
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            this.mnuExpandAll.Checked = !this.mnuExpandAll.Checked;
            if (this.mnuExpandAll.Checked)
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();

            string key = this.GetConfigKey();
            SystemContext.Instance.WriteConfig(key, this.mnuExpandAll.Checked.ToString());
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = this.virtualTree1.Bounds;
            using (Pen pen = new Pen(Color.DodgerBlue))
            {
                e.Graphics.DrawRectangle(pen, rect.X - 1, rect.Y - 2, rect.Width + 2, rect.Height + 2);
            }
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "更新节点颜色")
            {
                if (e.Param == null || e.Data == null)
                    return;
                if (!this.m_nodeTable.ContainsKey(e.Param.ToString()))
                    return;
                VirtualNode node = this.m_nodeTable[e.Param.ToString()];
                node.ForeColor = (Color)e.Data;
            }
            else if (e.Name == "更新节点名称")
            {
                if (e.Param == null || e.Data == null)
                    return;
                if (!this.m_nodeTable.ContainsKey(e.Param.ToString()))
                    return;
                VirtualNode node = this.m_nodeTable[e.Param.ToString()];
                node.Text = e.Data.ToString();
            }
            else if (e.Name == "刷新病人信息")
            {
                if (this.IsDisposed || !this.IsHandleCreated)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.Update();
                PatientTable.Instance.ReloadPatientInfoFromServer();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        private void formControl1_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
        }
    }
}
