// ***********************************************************
// ������Ӳ���ϵͳ,�����밲ȫ������صı�������.
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
using Heren.NurDoc.Utilities.Forms;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class QCForm : DockContentBase
    {
        private Dictionary<string, FormControl> m_htQCForm = null;

        /// <summary>
        /// ��ʾ��д�����밲ȫ�����¼���б�Ĵ���
        /// </summary>
        private QCListForm m_qcListForm = null;

        /// <summary>
        /// ��ȡ�����õ�ǰ����ĵ����Ͷ���
        /// </summary>
        [Browsable(false)]
        public QCTypeInfo QCTypeInfo
        {
            get
            {
                if (this.m_qcListForm == null)
                    return null;
                if (this.m_qcListForm.IsDisposed)
                    return null;
                return this.m_qcListForm.QCTypeInfo;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ���������Ƿ��Ѿ����޸�
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.m_qcListForm == null)
                    return false;
                if (this.m_qcListForm.IsDisposed)
                    return false;
                return this.m_qcListForm.IsModified;
            }
        }

        /// <summary>
        /// �����밲ȫ�����¼������ڵ��Ӧ��ϵ��
        /// </summary>
        private Dictionary<string, VirtualNode> m_nodeTable = null;

        /// <summary>
        /// ȫ�ֱ���
        /// </summary>
        private List<QCTypeInfo> m_lstQCTypeInfos = null;

        public QCForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            this.Index = 9999;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.virtualTree1.ImageList.Images.Clear();
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderClose);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.Templet);

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadQCNodeList();
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_qcListForm != null && !this.m_qcListForm.IsDisposed
                && this.m_qcListForm.Visible)
            {
                this.ShowQCListForm(this.QCTypeInfo);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// �ж��û��Ƿ��д򿪸������ĵ���Ȩ��
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID��</param>
        /// <returns>bool</returns>
        public bool CanOpenQCInfo(string szDocTypeID)
        {
            NurUserRight userRight = RightController.Instance.UserRight;
            if (userRight.LeaderNurse.Value || userRight.HeadNurse.Value || userRight.HigherNurse.Value)
                return true;
            return false;
        }

        /// <summary>
        /// ��鵱ǰ�����¼�༭�����Ƿ��Ѿ��ı�
        /// </summary>
        /// <returns>�Ƿ�ȡ��</returns>
        public override bool CheckModifyState()
        {
            if (this.m_qcListForm == null)
                return false;
            if (this.m_qcListForm.IsDisposed)
                return false;
            return this.m_qcListForm.CheckModifyState();
        }

        /// <summary>
        /// �ָ�ģ�����б������нڵ����ɫ���ı���ʾ
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="bResetBold">�Ƿ�ԭ������ʽ</param>
        /// <param name="bResetColor">�Ƿ�ԭ�ı���ɫ</param>
        /// <param name="bResetState">�Ƿ�ԭ�޸�״̬</param>
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

        private bool LoadQCNodeList()
        {
            if (this.m_nodeTable == null)
                this.m_nodeTable = new Dictionary<string, VirtualNode>();
            this.m_nodeTable.Clear();

            this.virtualTree1.Nodes.Clear();
            Application.DoEvents();

            string szApplyEnv = ServerData.DocTypeApplyEnv.NURDOC_LIST4;
            List<QCTypeInfo> lstQCTypeInfo = FormCache.Instance.GetQCTypeList(szApplyEnv);
            if (lstQCTypeInfo == null)
            {
                MessageBoxEx.ShowError("�����밲ȫ�����¼�б�����ʧ��!");
                return false;
            }
            this.m_lstQCTypeInfos = lstQCTypeInfo;
           
            foreach (QCTypeInfo qcTypeInfo in lstQCTypeInfo)
            {
                if (qcTypeInfo == null)
                    continue;
                if (!qcTypeInfo.IsValid || !qcTypeInfo.IsVisible)
                    continue;

                VirtualNode qcTypeNode = new VirtualNode();
                qcTypeNode.Tag = qcTypeInfo;
                qcTypeNode.Text = qcTypeInfo.QCTypeName;

                if (!qcTypeInfo.IsFolder)
                    qcTypeNode.ImageIndex = 2;
                else
                    qcTypeNode.ImageIndex = 0;
                qcTypeNode.ImageIndex = qcTypeNode.ImageIndex;

                if (!this.m_nodeTable.ContainsKey(qcTypeInfo.QCTypeID))
                    this.m_nodeTable.Add(qcTypeInfo.QCTypeID, qcTypeNode);
            }

            //���ڵ���������,��ӵ�����
            foreach (KeyValuePair<string, VirtualNode> pair in this.m_nodeTable)
            {
                if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    continue;

                QCTypeInfo qcTypeInfo = pair.Value.Tag as QCTypeInfo;
                if (qcTypeInfo == null)
                    continue;

                if (string.IsNullOrEmpty(qcTypeInfo.ParentID))
                {
                    this.virtualTree1.Nodes.Add(pair.Value);
                }
                else
                {
                    if (!this.m_nodeTable.ContainsKey(qcTypeInfo.ParentID))
                        continue;
                    VirtualNodeList nodes = this.m_nodeTable[qcTypeInfo.ParentID].Nodes;
                    nodes.Add(pair.Value);
                }
            }
            this.m_nodeTable.Clear();
            if (SystemContext.Instance.GetConfig(SystemConst.ConfigKey.QC_EXPAND, true))
                this.virtualTree1.ExpandAll();
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        /// <summary>
        /// �ͷŵ���ǰ���б�
        /// </summary>
        private void DisposeAllForms()
        {
            if (this.m_htQCForm == null || this.m_htQCForm.Count <= 0)
                return;
            foreach (FormControl form in this.m_htQCForm.Values)
                form.Dispose();
            this.m_htQCForm.Clear();
        }

        /// <summary>
        /// ��ʾָ�������밲ȫ�����¼���Ͷ�Ӧ�������밲ȫ�����¼�б���
        /// </summary>
        /// <param name="qcTypeInfo">�����밲ȫ�����¼����</param>
        /// <returns>�Ƿ�ִ�гɹ�</returns>
        private bool ShowQCListForm(QCTypeInfo qcTypeInfo)
        {
            if (qcTypeInfo == null || qcTypeInfo.IsFolder)
                return false;

            this.CreateQCListForm(qcTypeInfo);
            this.m_qcListForm.RefreshView();
            return true;
        }

        /// <summary>
        /// �����ĵ��б���
        /// </summary>
        /// <param name="qcTypeInfo">�ĵ�����</param>
        private void CreateQCListForm(QCTypeInfo qcTypeInfo)
        {
            if (this.m_qcListForm == null || this.m_qcListForm.IsDisposed)
            {
                this.m_qcListForm = new QCListForm();
                this.m_qcListForm.TopLevel = false;
                this.m_qcListForm.FormBorderStyle = FormBorderStyle.None;
                this.m_qcListForm.Dock = DockStyle.Fill;
                this.m_qcListForm.Padding = new Padding(0);
                this.m_qcListForm.Parent = this;
                this.m_qcListForm.DocumentUpdated +=
                    new EventHandler(this.QCListForm_DocumentUpdated);
            }
            this.m_qcListForm.QCTypeInfo = qcTypeInfo;
            this.m_qcListForm.Visible = true;
            this.m_qcListForm.BringToFront();
        }

        /// <summary>
        /// ��ȡָ���ڵ�����ָ���ĵ����͵�ģ��ڵ�
        /// </summary>
        /// <param name="parent">ָ���ĸ��ڵ�</param>
        /// <param name="qcTypeInfo">ָ�����ĵ�����</param>
        /// <returns>���õı�ģ��ڵ�</returns>
        private VirtualNode GetTempletNode(VirtualNode parent, QCTypeInfo qcTypeInfo)
        {
            if (parent != null)
            {
                QCTypeInfo parentQCTypeInfo = parent.Tag as QCTypeInfo;
                if (parentQCTypeInfo != null && !parentQCTypeInfo.IsFolder
                    && qcTypeInfo != null
                    && qcTypeInfo.QCTypeID == parentQCTypeInfo.QCTypeID)
                    return parent;
            }

            VirtualNodeList nodes =
                (parent == null) ? this.virtualTree1.Nodes : parent.Nodes;
            foreach (VirtualNode childNode in nodes)
            {
                VirtualNode node = this.GetTempletNode(childNode, qcTypeInfo);
                if (node != null)
                    return node;
            }
            return null;
        }

        private void QCListForm_DocumentUpdated(object sender, EventArgs e)
        {
            this.formControl1.UpdateFormData("ˢ����ͼ", null);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.mnuExpandAll.Checked =
                SystemContext.Instance.GetConfig(SystemConst.ConfigKey.QC_EXPAND, true);
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            this.mnuExpandAll.Checked = !this.mnuExpandAll.Checked;
            if (this.mnuExpandAll.Checked)
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();
            SystemContext.Instance.WriteConfig(SystemConst.ConfigKey.QC_EXPAND
                , this.mnuExpandAll.Checked.ToString());
        }

        private void virtualTree1_NodeMouseClick(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;
            QCTypeInfo qcTypeInfo = e.Node.Tag as QCTypeInfo;
            if (qcTypeInfo == null || qcTypeInfo.IsFolder)
                return;
            if (this.QCTypeInfo != null && this.QCTypeInfo.QCTypeID == qcTypeInfo.QCTypeID)
                return;
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            if (this.ShowQCListForm(qcTypeInfo))
            {
                this.ResetNodeTextAndColor(null, true, false, false);
                e.Node.Font = new Font(this.virtualTree1.Font, FontStyle.Bold);
            }
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "���½ڵ���ɫ")
            {
                if (e.Param == null || e.Data == null)
                    return;
                if (!this.m_nodeTable.ContainsKey(e.Param.ToString()))
                    return;
                VirtualNode node = this.m_nodeTable[e.Param.ToString()];
                node.ForeColor = (Color)e.Data;
            }
            else if (e.Name == "���½ڵ�����")
            {
                if (e.Param == null || e.Data == null)
                    return;
                if (!this.m_nodeTable.ContainsKey(e.Param.ToString()))
                    return;
                VirtualNode node = this.m_nodeTable[e.Param.ToString()];
                node.Text = e.Data.ToString();
            }
            else if (e.Name == "ˢ������")
            {
                SystemContext.Instance.OnDataChanged(this, new EventArgs());
            }
        }

        //��� �շ��� ���ڵ�
        private void CleanEmptyNode(VirtualNodeList listNodes)
        {
            for (int index = listNodes.Count - 1; index >= 0; index--)
            {
                QCTypeInfo qcInfo = listNodes[index].Tag as QCTypeInfo;
                CleanEmptyNode(listNodes[index].Nodes);
                if (qcInfo.IsFolder && listNodes[index].Nodes.Count <= 0)
                {
                    listNodes.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// ��ȡָ���ڵ����������ӽڵ���,��һ���Ѽ�¼�����ĵ���Ӧ�ڵ�
        /// </summary>
        /// <param name="parent">ָ���ĸ��ڵ�</param>
        /// <returns>��һ���Ѽ�¼�����ĵ���Ӧ�ڵ�</returns>
        private VirtualNode GetFirstDocuemntNode(VirtualNode parent)
        {
            if (parent != null)
            {
                QCTypeInfo qcTypeInfo = parent.Tag as QCTypeInfo;
                if (qcTypeInfo != null && !qcTypeInfo.IsFolder)
                    return parent;
            }

            VirtualNodeList nodes =
                (parent == null) ? this.virtualTree1.Nodes : parent.Nodes;
            foreach (VirtualNode childNode in nodes)
            {
                VirtualNode node = this.GetFirstDocuemntNode(childNode);
                if (node != null)
                    return node;
            }
            return null;
        }

    }
}