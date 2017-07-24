// **************************************************************
// ������Ӳ���ϵͳ����ģ��֮�ı�ģ�������б���
// Creator:YangMingkun  Date:2012-9-5
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using Heren.Common.Libraries;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.InfoLib.DockForms
{
    /// <summary>
    ///Ŀ¼��
    /// </summary>
    internal partial class InfoLibTreeForm : Form
    {
        private string m_MenuID = null;

        /// <summary>
        /// Ŀ¼���ϼ��ڵ�ID
        /// </summary>
        public string MenuID
        {
            get
            {
                return this.MenuID;
            }

            set
            {
                this.m_MenuID = value;
            }
        }

        /// <summary>
        /// �����ڵ���ʱʹ�õ�ģ��ڵ��
        /// </summary>
        private Hashtable m_htTempletNodeList = null;

        /// <summary>
        /// ����ģ����ڵ�
        /// </summary>
        private TreeNode m_DepartTreeNode = null;

        /// <summary>
        /// ȫԺģ����ڵ�
        /// </summary>
        private TreeNode m_HospitalTreeNode = null;

        private InfoLibForm m_templetForm = null;

        public InfoLibTreeForm()
            : this(null)
        {
        }

        public InfoLibTreeForm(InfoLibForm templetForm)
        {
            this.InitializeComponent();
            this.m_templetForm = templetForm;
        }

        #region"�����ı�ģ���б�"
        /// <summary>
        /// װ�س�ʼ�ı�ģ���б�(�����б�)
        /// </summary>
        public void RefreshTempletList()
        {
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            this.Update();
            this.treeView1.Nodes.Clear();
            if (SystemContext.Instance.LoginUser == null)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }

            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<InfoLibInfo> lstInfoLibInfos = null;
            short shRet = InfoLibService.Instance.GetPersonalInfoLibInfos(szUserID, ref lstInfoLibInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("���������ı�ģ���б�����ʧ��!");
                return;
            }
            if (lstInfoLibInfos == null)
                lstInfoLibInfos = new List<InfoLibInfo>();

            this.m_DepartTreeNode = new TreeNode();
            this.m_DepartTreeNode.Text = "����ģ��";
            this.m_DepartTreeNode.ImageIndex = 2;
            this.m_DepartTreeNode.SelectedImageIndex = 2;
            this.m_DepartTreeNode.Nodes.Add(null, "<��>", 3, 3);
            this.m_DepartTreeNode.Collapse();

            this.m_HospitalTreeNode = new TreeNode();
            this.m_HospitalTreeNode.Text = "ȫԺģ��";
            this.m_HospitalTreeNode.ImageIndex = 2;
            this.m_HospitalTreeNode.SelectedImageIndex = 2;
            this.m_HospitalTreeNode.Nodes.Add(null, "<��>", 3, 3);
            this.m_HospitalTreeNode.Collapse();

            if (this.m_htTempletNodeList == null)
                this.m_htTempletNodeList = new Hashtable();
            this.m_htTempletNodeList.Clear();

            //������ģ���Ӧ�Ľڵ㴴����,��ӵ��ڵ�Hash����
            int index = 0;
            while (index < lstInfoLibInfos.Count)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                index++;
                TreeNode node = new TreeNode();
                node.Tag = infoLibInfo;
                if (infoLibInfo.IsFolder)
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                }
                else
                {
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                }
                node.Text = infoLibInfo.InfoName;
                if (!this.m_htTempletNodeList.Contains(infoLibInfo.InfoID))
                    this.m_htTempletNodeList.Add(infoLibInfo.InfoID, node);
            }

            //���ڵ�Hash�������нڵ���������,��ӵ�TreeView��
            for (index = 0; index < lstInfoLibInfos.Count; index++)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                if (infoLibInfo == null)
                    continue;
                object objItem = this.m_htTempletNodeList[infoLibInfo.InfoID];
                TreeNode templetNode = objItem as TreeNode;
                if (templetNode != null)
                    this.AddTempletNodeToTree(null, templetNode);
            }
            this.m_htTempletNodeList.Clear();

            //��󽫲�������ڵ��Լ�ȫԺ����ڵ�׷�ӵ�TreeView
            this.treeView1.Nodes.Add(this.m_DepartTreeNode);
            this.treeView1.Nodes.Add(this.m_HospitalTreeNode);
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
        }

        public void ShowChildInfoLib(string szValue)
        {
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            this.Update();
            this.treeView1.Nodes.Clear();

            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<InfoLibInfo> lstInfoLibInfos = null;
            short shRet = InfoLibService.Instance.GetPersonalInfoLibInfos(szUserID, ref lstInfoLibInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("���������ı�ģ���б�����ʧ��!");
                return;
            }
            if (lstInfoLibInfos == null)
                lstInfoLibInfos = new List<InfoLibInfo>();

            this.m_DepartTreeNode = new TreeNode();
            this.m_DepartTreeNode.Text = "����ģ��";
            this.m_DepartTreeNode.ImageIndex = 2;
            this.m_DepartTreeNode.SelectedImageIndex = 2;
            this.m_DepartTreeNode.Nodes.Add(null, "<��>", 3, 3);
            this.m_DepartTreeNode.Collapse();

            this.m_HospitalTreeNode = new TreeNode();
            this.m_HospitalTreeNode.Text = "ȫԺģ��";
            this.m_HospitalTreeNode.ImageIndex = 2;
            this.m_HospitalTreeNode.SelectedImageIndex = 2;
            this.m_HospitalTreeNode.Nodes.Add(null, "<��>", 3, 3);
            this.m_HospitalTreeNode.Collapse();

            if (this.m_htTempletNodeList == null)
                this.m_htTempletNodeList = new Hashtable();
            this.m_htTempletNodeList.Clear();

            //������ģ���Ӧ�Ľڵ㴴����,��ӵ��ڵ�Hash����
            int index = 0;
            while (index < lstInfoLibInfos.Count)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                index++;
                TreeNode node = new TreeNode();
                node.Tag = infoLibInfo;
                if (infoLibInfo.IsFolder)
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                }
                else
                {
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                }
                node.Text = infoLibInfo.InfoName;

                if (!this.m_htTempletNodeList.Contains(infoLibInfo.InfoID) && infoLibInfo.IsFolder)
                    this.m_htTempletNodeList.Add(infoLibInfo.InfoID, node);
            }

            //���ڵ�Hash�������нڵ���������,��ӵ�TreeView��
            for (index = 0; index < lstInfoLibInfos.Count; index++)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                if (infoLibInfo == null)
                    continue;
                object objItem = this.m_htTempletNodeList[infoLibInfo.InfoID];
                TreeNode templetNode = objItem as TreeNode;
                if (templetNode != null)
                    this.AddTempletNodeToTree(null, templetNode);
            }
            this.m_htTempletNodeList.Clear();

            //��󽫲�������ڵ��Լ�ȫԺ����ڵ�׷�ӵ�TreeView
            this.treeView1.Nodes.Add(this.m_DepartTreeNode);
            this.treeView1.Nodes.Add(this.m_HospitalTreeNode);
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
        }

        /// <summary>
        /// ���ָ����ģ��ڵ㵽TreeView����
        /// </summary>
        /// <param name="rootNode">���ڵ�</param>
        /// <param name="templetNode">ģ��ڵ�</param>
        private void AddTempletNodeToTree(TreeNode rootNode, TreeNode templetNode)
        {
            //���ģ��ڵ�Ϊ�ջ����Ѿ���ӵ�����
            if (templetNode == null || templetNode.Parent != null)
                return;
            if (templetNode.TreeView != null)
                return;

            InfoLibInfo infoLibInfo = templetNode.Tag as InfoLibInfo;
            if (infoLibInfo == null)
                return;

            if (this.m_htTempletNodeList == null)
                return;

            //���ģ��û�и�Ŀ¼,��ô��ӵ���Ŀ¼
            if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.ParentID))
            {
                //if (rootNode != null)
                //    rootNode.Nodes.Add(templetNode);
                //else
                //    this.treeView1.Nodes.Add(templetNode);
                return;
            }

            //���ģ���и�Ŀ¼,��ô��ӵ���Ŀ¼
            TreeNode parentNode = this.m_htTempletNodeList[infoLibInfo.ParentID] as TreeNode;
            if (parentNode == null)
            {
                if (rootNode != null)
                    rootNode.Nodes.Add(templetNode);
                else if (infoLibInfo.ParentID == this.m_MenuID)
                    this.treeView1.Nodes.Add(templetNode);
            }
            else
            {
                parentNode.Nodes.Add(templetNode);
                this.AddTempletNodeToTree(rootNode, parentNode);
            }
        }

        /// <summary>
        /// װ�ز�����������Ϣ���б�
        /// </summary>
        private void LoadWardInfoLibList()
        {
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            this.Update();
            if (this.m_DepartTreeNode == null)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }
            this.m_DepartTreeNode.Nodes.Clear();

            if (SystemContext.Instance.LoginUser == null)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<InfoLibInfo> lstInfoLibInfos = null;
            short shRet = InfoLibService.Instance.GetDeptInfoLibInfos(szWardCode, true, ref lstInfoLibInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("�ҿ��������˵Ļ�����Ϣ�б�����ʧ��!");
                return;
            }
            if (lstInfoLibInfos == null || lstInfoLibInfos.Count <= 0)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }

            if (this.m_htTempletNodeList == null)
                this.m_htTempletNodeList = new Hashtable();
            this.m_htTempletNodeList.Clear();

            //������ģ���Ӧ�Ľڵ㴴����,��ӵ��ڵ�Hash����
            int index = 0;
            while (index < lstInfoLibInfos.Count)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                if (infoLibInfo.CreatorID == SystemContext.Instance.LoginUser.ID)
                {
                    lstInfoLibInfos.RemoveAt(index);
                    continue;
                }
                index++;
                TreeNode node = new TreeNode();
                node.Tag = infoLibInfo;
                if (infoLibInfo.IsFolder)
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                }
                else
                {
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                }
                if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.CreatorName))
                    node.Text = infoLibInfo.InfoName;
                else
                    node.Text = string.Concat(infoLibInfo.InfoName, "��", infoLibInfo.CreatorName);

                if (!this.m_htTempletNodeList.Contains(infoLibInfo.InfoID) && infoLibInfo.IsFolder)
                    this.m_htTempletNodeList.Add(infoLibInfo.InfoID, node);
            }

            //���ڵ�Hash�������нڵ���������,��ӵ�TreeView��
            for (index = 0; index < lstInfoLibInfos.Count; index++)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                if (infoLibInfo == null)
                    continue;
                object objItem = this.m_htTempletNodeList[infoLibInfo.InfoID];
                TreeNode templetNode = objItem as TreeNode;
                if (templetNode != null)
                    this.AddTempletNodeToTree(this.m_DepartTreeNode, templetNode);
            }
            this.m_htTempletNodeList.Clear();
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
        }

        /// <summary>
        /// װ��ȫԺ�����ı�ģ���б�
        /// </summary>
        private void LoadHospitalInfoLibList()
        {
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            this.Update();
            if (this.m_HospitalTreeNode == null)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }
            this.m_HospitalTreeNode.Nodes.Clear();

            if (SystemContext.Instance.LoginUser == null)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }

            List<InfoLibInfo> lstInfoLibInfos = null;
            short shRet = InfoLibService.Instance.GetHospitalInfoLibInfos(ref lstInfoLibInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("ȫԺ�����ƵĻ�����Ϣ�����б�����ʧ��!");
                return;
            }
            if (lstInfoLibInfos == null || lstInfoLibInfos.Count <= 0)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }

            if (this.m_htTempletNodeList == null)
                this.m_htTempletNodeList = new Hashtable();
            this.m_htTempletNodeList.Clear();

            //������ģ���Ӧ�Ľڵ㴴����,��ӵ��ڵ�Hash����
            int index = 0;
            while (index < lstInfoLibInfos.Count)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                if (infoLibInfo.CreatorID == SystemContext.Instance.LoginUser.ID)
                {
                    lstInfoLibInfos.RemoveAt(index);
                    continue;
                }
                index++;
                TreeNode node = new TreeNode();
                node.Tag = infoLibInfo;
                if (infoLibInfo.IsFolder)
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                }
                else
                {
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                }
                if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.CreatorName))
                    node.Text = infoLibInfo.InfoName;
                else
                    node.Text = string.Concat(infoLibInfo.InfoName, "��", infoLibInfo.CreatorName);

                if (!this.m_htTempletNodeList.Contains(infoLibInfo.InfoID) && infoLibInfo.IsFolder)
                    this.m_htTempletNodeList.Add(infoLibInfo.InfoID, node);
            }

            //���ڵ�Hash�������нڵ���������,��ӵ�TreeView��
            for (index = 0; index < lstInfoLibInfos.Count; index++)
            {
                InfoLibInfo infoLibInfo = lstInfoLibInfos[index];
                if (infoLibInfo == null)
                    continue;
                object objItem = this.m_htTempletNodeList[infoLibInfo.InfoID];
                TreeNode templetNode = objItem as TreeNode;
                if (templetNode != null)
                    this.AddTempletNodeToTree(this.m_HospitalTreeNode, templetNode);
            }
            this.m_htTempletNodeList.Clear();
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
        }
        #endregion

        #region"ģ�������������"
        /// <summary>
        /// ����һ���µ�ģ����Ϣ����
        /// </summary>
        /// <returns>TextTempletInfo</returns>
        private InfoLibInfo MakeInfoLibInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;

            InfoLibInfo infoLibInfo = new InfoLibInfo();
            infoLibInfo.InfoName = string.Empty;
            infoLibInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            infoLibInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            infoLibInfo.CreateTime = SysTimeService.Instance.Now;
            infoLibInfo.ModifyTime = infoLibInfo.CreateTime;
            infoLibInfo.WardCode = SystemContext.Instance.LoginUser.WardCode;
            infoLibInfo.WardName = SystemContext.Instance.LoginUser.WardName;
            infoLibInfo.IsFolder = false;
            infoLibInfo.ShareLevel = ServerData.ShareLevel.PERSONAL;
            infoLibInfo.ParentID = string.Empty;
            infoLibInfo.InfoID = infoLibInfo.MakeInfoID();
            return infoLibInfo;
        }

        /// <summary>
        /// ��ȡ��ǰѡ�нڵ��Ӧ��ģ��Ŀ¼��Ϣ
        /// </summary>
        /// <param name="node">Ŀ¼��Ӧ�Ľڵ�</param>
        /// <param name="infoLibInfo">�ڵ��Ӧ����Ϣ</param>
        private void GetCurrentFolderInfo(ref TreeNode node, ref InfoLibInfo infoLibInfo)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return;

            InfoLibInfo selectedInfoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (selectedInfoLibInfo == null)
                return;

            if (selectedInfoLibInfo.IsFolder)
                node = selectedNode;
            else
                node = selectedNode.Parent;
            if (node != null)
                infoLibInfo = node.Tag as InfoLibInfo;
        }

        /// <summary>
        /// �����µ���Ŀ¼
        /// </summary>
        private void CreateTempletFolder()
        {
            if (SystemContext.Instance.LoginUser == null)
                return;
            string szUserID = SystemContext.Instance.LoginUser.ID;

            InfoLibInfo infoLibInfo = this.MakeInfoLibInfo();
            if (infoLibInfo == null)
                return;
            infoLibInfo.IsFolder = true;
            infoLibInfo.InfoName = "�½�Ŀ¼";

            TreeNode node = new TreeNode();
            node.Tag = infoLibInfo;
            node.Text = infoLibInfo.InfoName;

            TreeNode parentNode = null;
            InfoLibInfo parentInfoLibInfo = null;
            this.GetCurrentFolderInfo(ref parentNode, ref parentInfoLibInfo);
            if (parentNode != null && parentInfoLibInfo == null)
            {
                MessageBoxEx.Show("��û��Ȩ���ڴ�Ŀ¼���½�Ŀ¼!");
                return;
            }
            if (parentInfoLibInfo != null && parentInfoLibInfo.CreatorID != szUserID)
            {
                MessageBoxEx.Show("��û��Ȩ��������Ŀ¼���½�Ŀ¼!");
                return;
            }
            if (this.m_MenuID == null)
            {
                MessageBoxEx.Show("���˵���ʧ��������ѡ�����˵�!");
                return;
            }
            if (parentInfoLibInfo != null && parentInfoLibInfo.IsFolder)
                infoLibInfo.ParentID = parentInfoLibInfo.InfoID;
            else if (parentInfoLibInfo == null && this.m_MenuID != null)
            {
                infoLibInfo.ParentID = this.m_MenuID;
            }
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            short shRet = InfoLibService.Instance.SaveInfoLib(infoLibInfo, null);
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                if (parentNode != null)
                    parentNode.Nodes.Add(node);
                else
                    this.treeView1.Nodes.Add(node);
                this.treeView1.SelectedNode = node;
                node.BeginEdit();
                return;
            }
            MessageBoxEx.Show("�½�ģ��Ŀ¼ʧ��!");
        }

        /// <summary>
        /// �ڵ�ǰѡ�нڵ�λ�ô�������ģ��
        /// </summary>
        private void CreateNewTemplet()
        {
            if (this.m_templetForm == null || this.m_templetForm.IsDisposed)
                return;
            if (SystemContext.Instance.LoginUser == null)
                return;
            string szUserID = SystemContext.Instance.LoginUser.ID;

            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);

            TreeNode currFolderNode = null;
            InfoLibInfo currInfoLibInfo = null;
            this.GetCurrentFolderInfo(ref currFolderNode, ref currInfoLibInfo);
            if (currFolderNode != null && (currInfoLibInfo == null || !currInfoLibInfo.IsFolder))
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("��û��Ȩ���ڴ�Ŀ¼���½�ģ��!");
                return;
            }
            if (currInfoLibInfo != null && currInfoLibInfo.CreatorID != szUserID)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("��û��Ȩ��������Ŀ¼���½�ģ��!");
                return;
            }

            if (this.m_templetForm.CheckTempletModify() != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }

            InfoLibInfo infoLibInfo = null;
            infoLibInfo = this.MakeInfoLibInfo();
            infoLibInfo.InfoName = "��ģ��";
            if (currInfoLibInfo != null)
                infoLibInfo.ParentID = currInfoLibInfo.InfoID;

            short shRet = InfoLibService.Instance.SaveInfoLib(infoLibInfo, null);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                TreeNode templetNode = new TreeNode();
                templetNode.Tag = infoLibInfo;
                templetNode.Text = infoLibInfo.InfoName;
                templetNode.ImageIndex = 3;
                templetNode.SelectedImageIndex = 3;
                if (currFolderNode != null)
                    currFolderNode.Nodes.Add(templetNode);
                else
                    this.treeView1.Nodes.Add(templetNode);
                this.treeView1.SelectedNode = templetNode;

                //���´�����ģ��,ͬʱ��������޸�״̬
                this.m_templetForm.OpenTemplet(infoLibInfo);
                templetNode.BeginEdit();
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
            }
            else
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("ģ�崴��ʧ��,���Ժ�����!");
            }
        }

        /// <summary>
        /// ��ȡ�û�ѡ�е�ģ���ı���Ϣ
        /// </summary>
        private void ShowSelectedTempletInfo()
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
            {
                this.ShowStatusMassage("��ʾ���������ģ����ѡ�������ݣ���ôϵͳ��������ѡ������");
                return;
            }
            if (selectedNode == this.m_HospitalTreeNode)
            {
                this.ShowStatusMassage("ȫԺ�����Ʊ���ģ�������ΪȫԺ�����ģ��");
                return;
            }
            if (selectedNode == this.m_DepartTreeNode)
            {
                this.ShowStatusMassage("�ҿ������˱���ģ�������Ϊ���ڹ����ģ��");
                return;
            }

            InfoLibInfo selectedTempletInfo = selectedNode.Tag as InfoLibInfo;
            if (selectedTempletInfo == null)
            {
                this.ShowStatusMassage(selectedNode.Text);
                return;
            }

            string szCreatorName = null;
            if (GlobalMethods.Misc.IsEmptyString(selectedTempletInfo.CreatorName))
                szCreatorName = "���߲���";
            else
                szCreatorName = selectedTempletInfo.CreatorName;

            string szDeptName = null;
            if (GlobalMethods.Misc.IsEmptyString(selectedTempletInfo.WardName))
                szDeptName = "���Ҳ���";
            else
                szDeptName = selectedTempletInfo.WardName;

            string szLastModifyTime = null;
            if (selectedTempletInfo.ModifyTime == selectedTempletInfo.DefaultTime)
                szLastModifyTime = "����޸�ʱ�䲻��";
            else
                szLastModifyTime = selectedTempletInfo.ModifyTime.ToString("yyyy-MM-dd HH:mm");

            this.ShowStatusMassage(string.Format("{0}��{1}��{2}��{3}��{4}��{5}"
                , selectedTempletInfo.IsFolder ? "Ŀ¼" : "ģ��", selectedTempletInfo.InfoName, szCreatorName
                , szDeptName, ServerData.ShareLevel.TransShareLevel(selectedTempletInfo.ShareLevel), szLastModifyTime));
        }

        /// <summary>
        /// �ݹ�ȡ��ĳһ�ڵ��µ������ӽڵ�ģ����Ϣ�б�
        /// </summary>
        /// <param name="node">ָ���ڵ�</param>
        /// <param name="lstTempletIDs">�ӽڵ�ģ����Ϣ�б�</param>
        private void GetSubTempletInfo(TreeNode node, ref List<string> lstTempletIDs)
        {
            if (node == null)
                return;
            if (lstTempletIDs == null)
                lstTempletIDs = new List<string>();
            InfoLibInfo templetInfo = node.Tag as InfoLibInfo;
            if (templetInfo != null)
                lstTempletIDs.Add(templetInfo.InfoID);
            for (int index = 0; index < node.Nodes.Count; index++)
            {
                TreeNode subNode = node.Nodes[index];
                if (subNode == null) continue;
                this.GetSubTempletInfo(subNode, ref lstTempletIDs);
            }
        }

        /// <summary>
        /// �ݹ��޸�ĳһ�ڵ��µ����нڵ�Ĺ���ˮƽ
        /// </summary>
        /// <param name="node">ָ���ڵ�</param>
        /// <param name="szShareLevel">����ˮƽ</param>
        private void ModityNodeShareLevel(TreeNode node, string szShareLevel)
        {
            if (node == null)
                return;
            InfoLibInfo tempInfo = node.Tag as InfoLibInfo;
            if (tempInfo != null)
                tempInfo.ShareLevel = szShareLevel;
            node.Tag = tempInfo;
            for (int index = 0; index < node.Nodes.Count; index++)
            {
                TreeNode subNode = node.Nodes[index];
                if (subNode == null) continue;
                this.ModityNodeShareLevel(subNode, szShareLevel);
            }
        }

        /// <summary>
        /// �޸�ѡ��ģ��Ĺ���ȼ�
        /// </summary>
        /// <param name="szNewLevel">�µĹ���ȼ�</param>
        private void ModifyTempletShareLevel(string szNewLevel)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null || SystemContext.Instance.LoginUser == null)
                return;
            InfoLibInfo infoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (infoLibInfo == null)
                return;

            if (!infoLibInfo.IsFolder && szNewLevel == infoLibInfo.ShareLevel)
                return;

            if (infoLibInfo.CreatorID != SystemContext.Instance.LoginUser.ID)
            {
                MessageBoxEx.Show("��û��Ȩ���޸Ĵ�ģ��Ĺ���ȼ�!");
                return;
            }

            if (infoLibInfo.IsFolder)
            {
                string szTipInfo = "ϵͳ����������{0}��Ŀ¼������ģ��Ĺ���ȼ�,ȷ����";
                if (MessageBoxEx.ShowConfirmFormat(szTipInfo, null, infoLibInfo.InfoName) != DialogResult.OK)
                    return;
            }

            short shRet = SystemConst.ReturnValue.OK;
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            if (infoLibInfo.IsFolder)
            {
                List<string> lstInfoIDs = null;
                this.GetSubTempletInfo(selectedNode, ref lstInfoIDs);
                shRet = InfoLibService.Instance.ModifyInfoLibShareLevel(lstInfoIDs, szNewLevel);
            }
            else
            {
                shRet = InfoLibService.Instance.ModifyInfoLibShareLevel(infoLibInfo.InfoID, szNewLevel);
            }
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                this.ModityNodeShareLevel(selectedNode, szNewLevel);
                this.ShowSelectedTempletInfo();
                return;
            }
            MessageBoxEx.Show(string.Format("��{0}��ģ�干��ȼ��޸�ʧ��!", infoLibInfo.InfoName));
        }

        /// <summary>
        /// ��ȡ��Ҫɾ����ģ��ID�б�
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="lstInfoID">ģ��ID�б�</param>
        private void GetDeletedTempletList(TreeNode node, ref List<string> lstInfoID)
        {
            if (lstInfoID == null)
                lstInfoID = new List<string>();
            if (node == null)
                return;

            InfoLibInfo infoLibInfo = node.Tag as InfoLibInfo;
            if (infoLibInfo == null)
                return;
            lstInfoID.Add(infoLibInfo.InfoID);

            for (int index = 0; index < node.Nodes.Count; index++)
            {
                this.GetDeletedTempletList(node.Nodes[index], ref lstInfoID);
            }
        }

        /// <summary>
        /// ɾ��ѡ�е�ģ�����
        /// </summary>
        private void DeleteSelectedTemplet()
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null || SystemContext.Instance.LoginUser == null)
                return;
            InfoLibInfo infoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (infoLibInfo == null)
                return;

            if (!this.m_templetForm.IsAllowModifyTemplet(infoLibInfo))
            {
                MessageBoxEx.Show(string.Format("��û��Ȩ��ɾ����{0}!", infoLibInfo.IsFolder ? "Ŀ¼" : "ģ��"));
                return;
            }

            string szTipInfo = string.Empty;
            if (selectedNode.Nodes.Count > 0)
                szTipInfo = string.Format("Ŀ¼��{0}���ǿգ�ȷ��ɾ����Ŀ¼�����µ�����ģ�壿", infoLibInfo.InfoName);
            else
                szTipInfo = string.Format("ȷ��ɾ����{0}��{1}��", infoLibInfo.InfoName, infoLibInfo.IsFolder ? "Ŀ¼" : "ģ��");

            DialogResult eRet = MessageBoxEx.ShowConfirm(szTipInfo);
            if (eRet != DialogResult.OK)
                return;

            this.Update();
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);

            List<string> lstTempletID = null;
            this.GetDeletedTempletList(selectedNode, ref lstTempletID);

            short shRet = InfoLibService.Instance.DeleteInfoLib(lstTempletID);
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                selectedNode.Remove();
                return;
            }
            MessageBoxEx.Show(string.Format("��{0}��{1}ɾ��ʧ��!", infoLibInfo.InfoName, infoLibInfo.IsFolder ? "Ŀ¼" : "ģ��"));
        }
        #endregion

        private void ShowStatusMassage(string szMessage)
        {
            this.m_templetForm.ShowStatusMessage(szMessage);
        }

        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (this.m_templetForm == null || this.m_templetForm.IsDisposed)
            {
                e.CancelEdit = true;
                return;
            }

            if (SystemContext.Instance.LoginUser == null)
            {
                e.CancelEdit = true;
                return;
            }

            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
            {
                e.CancelEdit = true;
                return;
            }
            InfoLibInfo infoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (infoLibInfo == null)
            {
                e.CancelEdit = true;
                return;
            }
            this.m_templetForm.ShowInfoLibList(infoLibInfo);
            if (!this.m_templetForm.IsAllowModifyTemplet(infoLibInfo))
            {
                e.CancelEdit = true;
                MessageBoxEx.Show(string.Format("��û��Ȩ���޸ġ�{0}��{1}������!"
                    , infoLibInfo.InfoName, infoLibInfo.IsFolder ? "Ŀ¼" : "ģ��"));
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null || GlobalMethods.Misc.IsEmptyString(e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            InfoLibInfo infoLibInfo = e.Node.Tag as InfoLibInfo;
            if (infoLibInfo == null || infoLibInfo.InfoName == e.Label.Trim())
            {
                e.CancelEdit = true;
                return;
            }

            string szInfoID = infoLibInfo.InfoID;
            string szNewName = e.Label.Trim();

            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            short shRet = InfoLibService.Instance.ModifyInfoLibName(szInfoID, szNewName);
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                infoLibInfo.InfoName = szNewName;
                this.ShowSelectedTempletInfo();
                return;
            }
            e.CancelEdit = true;
            MessageBoxEx.Show(string.Format("��{0}��������ʧ��!", infoLibInfo.InfoName));
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return;
            InfoLibInfo infoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (infoLibInfo != null && infoLibInfo.IsFolder)
                this.m_templetForm.ShowInfoLibList(infoLibInfo);

            this.ShowSelectedTempletInfo();
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node == this.m_DepartTreeNode)
                return;
            if (e.Node == this.m_HospitalTreeNode)
                return;
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == this.m_DepartTreeNode)
            {
                if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null)
                    this.LoadWardInfoLibList();
                return;
            }
            if (e.Node == this.m_HospitalTreeNode)
            {
                if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null)
                    this.LoadHospitalInfoLibList();
                return;
            }
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }

        #region "�ڵ��϶�����"
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.treeView1.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (SystemContext.Instance.LoginUser == null || !e.Data.GetDataPresent(typeof(TreeNode)))
                return;

            TreeNode moveNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            InfoLibInfo templetInfo = moveNode.Tag as InfoLibInfo;
            if (templetInfo == null)
                return;

            //�������϶�����ģ��
            if (templetInfo.CreatorID == SystemContext.Instance.LoginUser.ID)
                e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (SystemContext.Instance.LoginUser == null)
                return;

            TreeNode moveNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (moveNode == null)
                return;

            Point pt = this.treeView1.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = this.treeView1.GetNodeAt(pt);

            //�Ѹ�Ŀ¼�Ϸŵ�������Ŀ¼,��ȡ��
            TreeNode parentNode = targetNode;
            while (parentNode != null)
            {
                if (parentNode == moveNode)
                    return;
                parentNode = parentNode.Parent;
            }
            if (targetNode == moveNode.Parent)
                return;

            InfoLibInfo moveTempletInfo = moveNode.Tag as InfoLibInfo;
            InfoLibInfo targetTempletInfo = null;
            if (targetNode != null)
                targetTempletInfo = targetNode.Tag as InfoLibInfo;

            string szParentID = string.Empty;
            if (targetTempletInfo == null)
            {
                targetNode = null;
            }
            else
            {
                if (targetTempletInfo.CreatorID != SystemContext.Instance.LoginUser.ID)
                    return;
                if (!targetTempletInfo.IsFolder)
                {
                    if (targetNode.Parent == moveNode.Parent)
                        return;
                    targetNode = targetNode.Parent;
                    szParentID = targetTempletInfo.ParentID;
                }
                else
                {
                    szParentID = targetTempletInfo.InfoID;
                }
            }

            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.WaitCursor);
            short shRet = InfoLibService.Instance.ModifyInfoLibParentID(moveTempletInfo.InfoID, szParentID);
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);

            if (shRet == SystemConst.ReturnValue.OK)
            {
                moveTempletInfo.ParentID = szParentID;
                TreeNode newNode = (TreeNode)moveNode.Clone();
                if (targetNode != null)
                    targetNode.Nodes.Add(newNode);
                else
                    this.treeView1.Nodes.Add(newNode);
                moveNode.Remove();
                this.treeView1.SelectedNode = newNode;
                return;
            }
            MessageBoxEx.Show(string.Format("{0}��{1}���ƶ�ʧ��!", moveTempletInfo.IsFolder ? "Ŀ¼" : "ģ��"
                , moveTempletInfo.InfoName));
        }
        #endregion

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (this.m_templetForm == null || this.m_templetForm.IsDisposed)
                return;

            if (e.Node == null || e.Node.Tag == null)
                return;
            InfoLibInfo infoLibInfo = e.Node.Tag as InfoLibInfo;
            if (infoLibInfo != null && !infoLibInfo.IsFolder)
                this.m_templetForm.ImportSelectedContent();
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;

            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null || selectedNode.Tag == null)
                return;
            InfoLibInfo infoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (infoLibInfo != null && !infoLibInfo.IsFolder)
                this.m_templetForm.OpenTemplet(infoLibInfo);
        }

        private void toolbtnNewFolder_Click(object sender, EventArgs e)
        {
            this.CreateTempletFolder();
        }

        private void toolbtnNewTemplet_Click(object sender, EventArgs e)
        {
            this.CreateNewTemplet();
        }

        private void toolbtnRename_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null)
                this.treeView1.SelectedNode.BeginEdit();
        }

        private void toolbtnShareLevel_DropDownOpening(object sender, EventArgs e)
        {
            this.toolmnuSharePersonal.Checked = false;
            this.toolmnuShareDepart.Checked = false;
            this.toolmnuShareHospital.Checked = false;

            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return;
            InfoLibInfo infoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (infoLibInfo == null)
                return;

            if (infoLibInfo.ShareLevel == ServerData.ShareLevel.PERSONAL)
                this.toolmnuSharePersonal.Checked = true;
            else if (infoLibInfo.ShareLevel == ServerData.ShareLevel.DEPART)
                this.toolmnuShareDepart.Checked = true;
            else if (infoLibInfo.ShareLevel == ServerData.ShareLevel.HOSPITAL)
                this.toolmnuShareHospital.Checked = true;
        }

        private void toolmnuSharePersonal_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.PERSONAL);
        }

        private void toolmnuShareDepart_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.DEPART);
        }

        private void toolmnuShareHospital_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.HOSPITAL);
        }

        private void toolbtnDelete_Click(object sender, EventArgs e)
        {
            this.DeleteSelectedTemplet();
        }

        private void cmenuTextTemplet_Opening(object sender, CancelEventArgs e)
        {
            this.mnuSharePersonal.Checked = false;
            this.mnuShareDepart.Checked = false;
            this.mnuShareHospital.Checked = false;

            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return;
            InfoLibInfo infoLibInfo = selectedNode.Tag as InfoLibInfo;
            if (infoLibInfo == null)
                return;

            if (infoLibInfo.ShareLevel == ServerData.ShareLevel.PERSONAL)
                this.mnuSharePersonal.Checked = true;
            else if (infoLibInfo.ShareLevel == ServerData.ShareLevel.DEPART)
                this.mnuShareDepart.Checked = true;
            else if (infoLibInfo.ShareLevel == ServerData.ShareLevel.HOSPITAL)
                this.mnuShareHospital.Checked = true;
        }

        private void mnuNewFolder_Click(object sender, EventArgs e)
        {
            this.CreateTempletFolder();
        }

        private void mnuNewTemplet_Click(object sender, EventArgs e)
        {
            this.CreateNewTemplet();
        }

        private void mnuRename_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null)
                this.treeView1.SelectedNode.BeginEdit();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            this.DeleteSelectedTemplet();
        }

        private void mnuSharePersonal_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.PERSONAL);
        }

        private void mnuShareDepart_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.DEPART);
        }

        private void mnuShareHospital_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.HOSPITAL);
        }
    }
}