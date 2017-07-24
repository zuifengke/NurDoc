// ***********************************************************
// ������Ӳ���ϵͳ,����������񴰿�.
// Creator:YangMingkun  Date:2012-10-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls.VirtualTreeView;
using Heren.Common.Report;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities.Forms;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class NursingTaskForm : DockContentBase
    {
        private Dictionary<string, FormControl> m_htTaskForm = null;
        private Dictionary<string, VirtualNode> m_htNodeTable = null;
        private Thread m_taskCounterThread = null;

        public NursingTaskForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.NursingTask;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 300);
            this.MainFrame = parent;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();

            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.Templet);

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_htNodeTable == null)
            {
                this.LoadTaskNodeList();
                this.RefreshTaskCount();
                if (m_itaskCount <= 1)
                {
                    arrowSplitter1.IsExpand = false;
                    this.virtualTree1.SelectedNode = GetDocNode(this.virtualTree1.Nodes);
                }// = true;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        int m_itaskCount = 0;

        /// ���ص�ǰ��ʿ�Ĵ�������ڵ��б�
        /// </summary>
        /// <returns>�Ƿ���سɹ�</returns>
        public bool LoadTaskNodeList()
        {
            this.DisposeAllForms();
            this.virtualTree1.Nodes.Clear();
            Application.DoEvents();

            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_TASK;
            List<DocTypeInfo> lstDocTypeInfos =
                FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
            {
                MessageBoxEx.ShowError("���������б�����ʧ��!");
                return true;
            }

            if (this.m_htNodeTable == null)
                this.m_htNodeTable = new Dictionary<string, VirtualNode>();
            this.m_htNodeTable.Clear();
            this.virtualTree1.Nodes.Clear();
            //�ȴ����ڵ��ϣ�б�
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (docTypeInfo == null)
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;

                VirtualNode docTypeNode = new VirtualNode();
                docTypeNode.Tag = docTypeInfo;
                if (docTypeInfo.IsFolder)
                    docTypeNode.Text = docTypeInfo.DocTypeName;
                else
                {
                    docTypeNode.Text = docTypeInfo.DocTypeName + "(...)";
                    m_itaskCount++;
                }

                if (!docTypeInfo.IsFolder)
                    docTypeNode.ImageIndex = 2;
                else
                    docTypeNode.ImageIndex = 0;
                docTypeNode.ImageIndex = docTypeNode.ImageIndex;
                if (!this.m_htNodeTable.ContainsKey(docTypeInfo.DocTypeID))
                    this.m_htNodeTable.Add(docTypeInfo.DocTypeID, docTypeNode);
            }

            //���ڵ���������,��ӵ�����
            foreach (KeyValuePair<string, VirtualNode> pair in this.m_htNodeTable)
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
                    if (!this.m_htNodeTable.ContainsKey(docTypeInfo.ParentID))
                        continue;
                    VirtualNodeList nodes = this.m_htNodeTable[docTypeInfo.ParentID].Nodes;
                    nodes.Add(pair.Value);
                }
            }

            if (SystemContext.Instance.GetConfig(SystemConst.ConfigKey.TASK_NODE_EXPAND, true))
                this.virtualTree1.ExpandAll();
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        /// <summary>
        /// �ͷŵ���ǰ���б�
        /// </summary>
        private void DisposeAllForms()
        {
            try
            {
                if (this.m_taskCounterThread != null)
                    this.m_taskCounterThread.Abort();
            }
            catch
            {
                LogManager.Instance.WriteLog("����������ͣ��Դʧ�ܣ�");
            }
            this.m_taskCounterThread = null;
            if (this.m_htTaskForm == null || this.m_htTaskForm.Count <= 0)
                return;
            foreach (FormControl form in this.m_htTaskForm.Values)
                form.Dispose();
            this.m_htTaskForm.Clear();
        }

        /// <summary>
        /// ˢ���������и�����ڵ���ʾ����������
        /// </summary>
        public void RefreshTaskCount()
        {
            if (this.m_htNodeTable == null)
                this.LoadTaskNodeList();
            if (this.m_htNodeTable == null)
                return;
            foreach (VirtualNode node in this.m_htNodeTable.Values)
                this.ShowTaskForm(node.Tag as DocTypeInfo, false);

            if (this.m_taskCounterThread == null
                || !this.m_taskCounterThread.IsAlive)
            {
                this.m_taskCounterThread =
                    new Thread(new ThreadStart(this.StartTaskCounter));
            }
            if (this.m_taskCounterThread.ThreadState == ThreadState.Running)
                return;
            this.m_taskCounterThread.Start();
        }

        /// <summary>
        /// ˢ�´򿪵Ľ�����Ϣ
        /// </summary>
        public void RefreshActiveTask()
        {
            if (this.m_htNodeTable == null)
                return;
            foreach (VirtualNode node in this.m_htNodeTable.Values)
            {
                if (node.IsSelected)
                {
                    this.ShowTaskForm(node.Tag as DocTypeInfo, true);
                    return;
                }
            }
        }

        private int m_TaskCount;

        /// <summary>
        /// ������������
        /// </summary>
        public int TaskCount
        {
            get
            {
                return m_TaskCount;
            }

            set
            {
                this.m_TaskCount = value;
            }
        }

        /// <summary>
        /// �û��л�����ʱ ˢ�������б�
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnDisplayDeptChanged(EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_htNodeTable = null;
            this.RefreshTaskCount();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ˢ���������и�����ڵ���ʾ����������
        /// ֮��������������ӹ���
        /// </summary>
        private void StartTaskCounter()
        {
            if (this.m_htTaskForm == null || this.m_htNodeTable == null)
                return;
            this.m_TaskCount = 0;
            foreach (FormControl taskForm in this.m_htTaskForm.Values)
            {
                if (taskForm == null || taskForm.IsDisposed)
                    continue;
                DocTypeInfo docTypeInfo = taskForm.DocTypeInfo;
                if (docTypeInfo == null)
                    continue;
                if (!this.m_htNodeTable.ContainsKey(docTypeInfo.DocTypeID))
                    continue;
                VirtualNode node = this.m_htNodeTable[docTypeInfo.DocTypeID];
                if (node == null)
                    continue;
                object value = taskForm.GetFormData("������������");
                if (value == null)
                    continue;
                int count = GlobalMethods.Convert.StringToValue(value.ToString(), 0);
                this.m_TaskCount += count;
                node.Text = string.Format("{0}({1})", docTypeInfo.DocTypeName, count);
            }
            this.MainFrame.ShowTaskCountMessage(this.m_TaskCount);
        }

        /// <summary>
        /// ��ȡΨһ�Ĵ�������ģ��ڵ�
        /// </summary>
        /// <param name="node">TreeNodeList</param>
        /// <returns>VirtualNode</returns>
        private VirtualNode GetDocNode(VirtualNodeList node)
        {
            foreach (VirtualNode Node in node)
            {
                DocTypeInfo docTypeInfo = Node.Tag as DocTypeInfo;
                if (docTypeInfo != null && !docTypeInfo.IsFolder)
                    return Node;
                VirtualNode NodeResult = GetDocNode(Node.Nodes);
                if (NodeResult != null)
                    return NodeResult;
            }
            return null;
        }

        /// <summary>
        /// ��ָ����������Ϣ�������
        /// </summary>
        /// <param name="docTypeInfo">�������Ϣ</param>
        /// <param name="activate">�Ƿ񼤻��</param>
        /// <returns>�Ƿ�򿪳ɹ�</returns>
        private bool ShowTaskForm(DocTypeInfo docTypeInfo, bool activate)
        {
            if (docTypeInfo == null
                || docTypeInfo.IsFolder || string.IsNullOrEmpty(docTypeInfo.DocTypeID))
                return false;
            if (this.m_htTaskForm == null)
                this.m_htTaskForm = new Dictionary<string, FormControl>();

            FormControl taskForm = null;
            if (this.m_htTaskForm.ContainsKey(docTypeInfo.DocTypeID))
                taskForm = this.m_htTaskForm[docTypeInfo.DocTypeID];
            if (taskForm != null && !taskForm.IsDisposed)
            {
                if (activate && !taskForm.Visible)
                    taskForm.Visible = true;
                if (activate) taskForm.BringToFront();
                taskForm.UpdateFormData("ˢ�������б�", null);
                return true;
            }

            taskForm = new FormControl();
            if (!activate)
                taskForm.Visible = false;
            taskForm.Dock = DockStyle.Fill;
            taskForm.Parent = this;
            if (activate)
                taskForm.BringToFront();
            taskForm.CustomEvent +=
                new Heren.Common.Forms.Editor.CustomEventHandler(this.TaskForm_CustomEvent);

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                string szTaskName = docTypeInfo.DocTypeName;
                MessageBoxEx.ShowErrorFormat("���������{0}������ʧ��!", null, szTaskName);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
            bool success = taskForm.Load(docTypeInfo, byteFormData);
            if (success)
            {
                if (this.m_htTaskForm.ContainsKey(docTypeInfo.DocTypeID))
                    this.m_htTaskForm.Remove(docTypeInfo.DocTypeID);
                this.m_htTaskForm.Add(docTypeInfo.DocTypeID, taskForm);
            }
            return success;
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            this.mnuExpandAll.Checked = !this.mnuExpandAll.Checked;
            if (this.mnuExpandAll.Checked)
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();
            SystemContext.Instance.WriteConfig(SystemConst.ConfigKey.TASK_NODE_EXPAND
                , this.mnuExpandAll.Checked.ToString());
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.mnuExpandAll.Checked =
                SystemContext.Instance.GetConfig(SystemConst.ConfigKey.TASK_NODE_EXPAND, true);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = this.virtualTree1.Bounds;
            Pen pen = new Pen(Color.DodgerBlue);
            e.Graphics.DrawRectangle(pen, rect.X - 1, rect.Y - 2, rect.Width + 2, rect.Height + 2);
            pen.Dispose();
        }

        private void TaskForm_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "�򿪲��˴���")
            {
                if (this.MainFrame != null)
                    this.MainFrame.ShowPatientPageForm(e.Param);
            }
            else if (e.Name == "��ߴ򿪲��˴���")
            {
                if (this.MainFrame != null)
                    this.MainFrame.ShowPatientPageFormAtSide(e.Param, e.Data);
            }
            else if (e.Name == "������������")
            {
                if (e.Param == null || e.Data == null)
                    return;
                string szDocTypeID = e.Param.ToString();
                int nCount = GlobalMethods.Convert.StringToValue(e.Data.ToString(), 0);
                if (this.m_htNodeTable == null || string.IsNullOrEmpty(szDocTypeID)
                    || !this.m_htNodeTable.ContainsKey(szDocTypeID))
                {
                    return;
                }
                VirtualNode node = this.m_htNodeTable[szDocTypeID];
                if (node != null)
                {
                    DocTypeInfo docTypeInfo = node.Tag as DocTypeInfo;
                    if (docTypeInfo != null)
                        node.Text = string.Format("{0}({1})", docTypeInfo.DocTypeName, nCount);
                }
            }
        }

        private void virtualTree1_AfterSelect(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null)
                return;
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            this.ShowTaskForm(e.Node.Tag as DocTypeInfo, true);
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }

        protected override void OnDataChanged(EventArgs e)
        {
            base.OnDataChanged(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            RefreshActiveTask();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnPatientTableChanged()
        {
            base.OnPatientTableChanged();
        }
    }
}