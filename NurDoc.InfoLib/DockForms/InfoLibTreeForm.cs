// **************************************************************
// 护理电子病历系统公用模块之文本模板树形列表窗口
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
    ///目录树
    /// </summary>
    internal partial class InfoLibTreeForm : Form
    {
        private string m_MenuID = null;

        /// <summary>
        /// 目录树上级节点ID
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
        /// 创建节点树时使用的模板节点表
        /// </summary>
        private Hashtable m_htTempletNodeList = null;

        /// <summary>
        /// 病区模板根节点
        /// </summary>
        private TreeNode m_DepartTreeNode = null;

        /// <summary>
        /// 全院模板根节点
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

        #region"加载文本模板列表"
        /// <summary>
        /// 装载初始文本模板列表(个人列表)
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
                MessageBoxEx.Show("您创建的文本模板列表下载失败!");
                return;
            }
            if (lstInfoLibInfos == null)
                lstInfoLibInfos = new List<InfoLibInfo>();

            this.m_DepartTreeNode = new TreeNode();
            this.m_DepartTreeNode.Text = "科室模板";
            this.m_DepartTreeNode.ImageIndex = 2;
            this.m_DepartTreeNode.SelectedImageIndex = 2;
            this.m_DepartTreeNode.Nodes.Add(null, "<空>", 3, 3);
            this.m_DepartTreeNode.Collapse();

            this.m_HospitalTreeNode = new TreeNode();
            this.m_HospitalTreeNode.Text = "全院模板";
            this.m_HospitalTreeNode.ImageIndex = 2;
            this.m_HospitalTreeNode.SelectedImageIndex = 2;
            this.m_HospitalTreeNode.Nodes.Add(null, "<空>", 3, 3);
            this.m_HospitalTreeNode.Collapse();

            if (this.m_htTempletNodeList == null)
                this.m_htTempletNodeList = new Hashtable();
            this.m_htTempletNodeList.Clear();

            //将所有模板对应的节点创建好,添加到节点Hash表中
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

            //将节点Hash表中所有节点连接起来,添加到TreeView中
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

            //最后将病区共享节点以及全院共享节点追加到TreeView
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
                MessageBoxEx.Show("您创建的文本模板列表下载失败!");
                return;
            }
            if (lstInfoLibInfos == null)
                lstInfoLibInfos = new List<InfoLibInfo>();

            this.m_DepartTreeNode = new TreeNode();
            this.m_DepartTreeNode.Text = "科室模板";
            this.m_DepartTreeNode.ImageIndex = 2;
            this.m_DepartTreeNode.SelectedImageIndex = 2;
            this.m_DepartTreeNode.Nodes.Add(null, "<空>", 3, 3);
            this.m_DepartTreeNode.Collapse();

            this.m_HospitalTreeNode = new TreeNode();
            this.m_HospitalTreeNode.Text = "全院模板";
            this.m_HospitalTreeNode.ImageIndex = 2;
            this.m_HospitalTreeNode.SelectedImageIndex = 2;
            this.m_HospitalTreeNode.Nodes.Add(null, "<空>", 3, 3);
            this.m_HospitalTreeNode.Collapse();

            if (this.m_htTempletNodeList == null)
                this.m_htTempletNodeList = new Hashtable();
            this.m_htTempletNodeList.Clear();

            //将所有模板对应的节点创建好,添加到节点Hash表中
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

            //将节点Hash表中所有节点连接起来,添加到TreeView中
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

            //最后将病区共享节点以及全院共享节点追加到TreeView
            this.treeView1.Nodes.Add(this.m_DepartTreeNode);
            this.treeView1.Nodes.Add(this.m_HospitalTreeNode);
            GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
        }

        /// <summary>
        /// 添加指定的模板节点到TreeView树中
        /// </summary>
        /// <param name="rootNode">根节点</param>
        /// <param name="templetNode">模板节点</param>
        private void AddTempletNodeToTree(TreeNode rootNode, TreeNode templetNode)
        {
            //如果模板节点为空或者已经添加到树中
            if (templetNode == null || templetNode.Parent != null)
                return;
            if (templetNode.TreeView != null)
                return;

            InfoLibInfo infoLibInfo = templetNode.Tag as InfoLibInfo;
            if (infoLibInfo == null)
                return;

            if (this.m_htTempletNodeList == null)
                return;

            //如果模板没有父目录,那么添加到根目录
            if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.ParentID))
            {
                //if (rootNode != null)
                //    rootNode.Nodes.Add(templetNode);
                //else
                //    this.treeView1.Nodes.Add(templetNode);
                return;
            }

            //如果模板有父目录,那么添加到父目录
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
        /// 装载病区共享护理信息库列表
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
                MessageBoxEx.Show("我科室其他人的护理信息列表下载失败!");
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

            //将所有模板对应的节点创建好,添加到节点Hash表中
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
                    node.Text = string.Concat(infoLibInfo.InfoName, "，", infoLibInfo.CreatorName);

                if (!this.m_htTempletNodeList.Contains(infoLibInfo.InfoID) && infoLibInfo.IsFolder)
                    this.m_htTempletNodeList.Add(infoLibInfo.InfoID, node);
            }

            //将节点Hash表中所有节点连接起来,添加到TreeView中
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
        /// 装载全院共享文本模板列表
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
                MessageBoxEx.Show("全院其他科的护理信息内容列表下载失败!");
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

            //将所有模板对应的节点创建好,添加到节点Hash表中
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
                    node.Text = string.Concat(infoLibInfo.InfoName, "，", infoLibInfo.CreatorName);

                if (!this.m_htTempletNodeList.Contains(infoLibInfo.InfoID) && infoLibInfo.IsFolder)
                    this.m_htTempletNodeList.Add(infoLibInfo.InfoID, node);
            }

            //将节点Hash表中所有节点连接起来,添加到TreeView中
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

        #region"模板基本操作函数"
        /// <summary>
        /// 生成一个新的模板信息对象
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
        /// 获取当前选中节点对应的模板目录信息
        /// </summary>
        /// <param name="node">目录对应的节点</param>
        /// <param name="infoLibInfo">节点对应的信息</param>
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
        /// 创建新的子目录
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
            infoLibInfo.InfoName = "新建目录";

            TreeNode node = new TreeNode();
            node.Tag = infoLibInfo;
            node.Text = infoLibInfo.InfoName;

            TreeNode parentNode = null;
            InfoLibInfo parentInfoLibInfo = null;
            this.GetCurrentFolderInfo(ref parentNode, ref parentInfoLibInfo);
            if (parentNode != null && parentInfoLibInfo == null)
            {
                MessageBoxEx.Show("您没有权限在此目录下新建目录!");
                return;
            }
            if (parentInfoLibInfo != null && parentInfoLibInfo.CreatorID != szUserID)
            {
                MessageBoxEx.Show("您没有权限在他人目录下新建目录!");
                return;
            }
            if (this.m_MenuID == null)
            {
                MessageBoxEx.Show("主菜单丢失，请重新选择左侧菜单!");
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
            MessageBoxEx.Show("新建模板目录失败!");
        }

        /// <summary>
        /// 在当前选中节点位置处创建新模板
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
                MessageBoxEx.Show("您没有权限在此目录下新建模板!");
                return;
            }
            if (currInfoLibInfo != null && currInfoLibInfo.CreatorID != szUserID)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("您没有权限在他人目录下新建模板!");
                return;
            }

            if (this.m_templetForm.CheckTempletModify() != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                return;
            }

            InfoLibInfo infoLibInfo = null;
            infoLibInfo = this.MakeInfoLibInfo();
            infoLibInfo.InfoName = "新模板";
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

                //打开新创建的模板,同时标题进入修改状态
                this.m_templetForm.OpenTemplet(infoLibInfo);
                templetNode.BeginEdit();
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
            }
            else
            {
                GlobalMethods.UI.SetCursor(this.m_templetForm, Cursors.Default);
                MessageBoxEx.Show("模板创建失败,请稍后重试!");
            }
        }

        /// <summary>
        /// 获取用户选中的模板文本信息
        /// </summary>
        private void ShowSelectedTempletInfo()
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
            {
                this.ShowStatusMassage("提示：如果您在模板中选中有内容，那么系统将仅导入选中内容");
                return;
            }
            if (selectedNode == this.m_HospitalTreeNode)
            {
                this.ShowStatusMassage("全院其他科保存的，已设置为全院共享的模板");
                return;
            }
            if (selectedNode == this.m_DepartTreeNode)
            {
                this.ShowStatusMassage("我科其他人保存的，已设置为科内共享的模板");
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
                szCreatorName = "作者不详";
            else
                szCreatorName = selectedTempletInfo.CreatorName;

            string szDeptName = null;
            if (GlobalMethods.Misc.IsEmptyString(selectedTempletInfo.WardName))
                szDeptName = "科室不详";
            else
                szDeptName = selectedTempletInfo.WardName;

            string szLastModifyTime = null;
            if (selectedTempletInfo.ModifyTime == selectedTempletInfo.DefaultTime)
                szLastModifyTime = "最后修改时间不详";
            else
                szLastModifyTime = selectedTempletInfo.ModifyTime.ToString("yyyy-MM-dd HH:mm");

            this.ShowStatusMassage(string.Format("{0}：{1}，{2}，{3}，{4}，{5}"
                , selectedTempletInfo.IsFolder ? "目录" : "模板", selectedTempletInfo.InfoName, szCreatorName
                , szDeptName, ServerData.ShareLevel.TransShareLevel(selectedTempletInfo.ShareLevel), szLastModifyTime));
        }

        /// <summary>
        /// 递归取得某一节点下的所有子节点模板信息列表
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <param name="lstTempletIDs">子节点模板信息列表</param>
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
        /// 递归修改某一节点下的所有节点的共享水平
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <param name="szShareLevel">共享水平</param>
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
        /// 修改选中模板的共享等级
        /// </summary>
        /// <param name="szNewLevel">新的共享等级</param>
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
                MessageBoxEx.Show("您没有权限修改此模板的共享等级!");
                return;
            }

            if (infoLibInfo.IsFolder)
            {
                string szTipInfo = "系统即将调整“{0}”目录下所有模板的共享等级,确定吗？";
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
            MessageBoxEx.Show(string.Format("“{0}”模板共享等级修改失败!", infoLibInfo.InfoName));
        }

        /// <summary>
        /// 获取将要删除的模板ID列表
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="lstInfoID">模板ID列表</param>
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
        /// 删除选中的模板对象
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
                MessageBoxEx.Show(string.Format("您没有权限删除此{0}!", infoLibInfo.IsFolder ? "目录" : "模板"));
                return;
            }

            string szTipInfo = string.Empty;
            if (selectedNode.Nodes.Count > 0)
                szTipInfo = string.Format("目录“{0}”非空，确认删除该目录和其下的所有模板？", infoLibInfo.InfoName);
            else
                szTipInfo = string.Format("确认删除“{0}”{1}？", infoLibInfo.InfoName, infoLibInfo.IsFolder ? "目录" : "模板");

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
            MessageBoxEx.Show(string.Format("“{0}”{1}删除失败!", infoLibInfo.InfoName, infoLibInfo.IsFolder ? "目录" : "模板"));
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
                MessageBoxEx.Show(string.Format("您没有权限修改“{0}”{1}的名称!"
                    , infoLibInfo.InfoName, infoLibInfo.IsFolder ? "目录" : "模板"));
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
            MessageBoxEx.Show(string.Format("“{0}”重命名失败!", infoLibInfo.InfoName));
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

        #region "节点拖动过程"
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

            //不允许拖动他人模板
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

            //把父目录拖放到其下子目录,则取消
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
            MessageBoxEx.Show(string.Format("{0}“{1}”移动失败!", moveTempletInfo.IsFolder ? "目录" : "模板"
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