// ***********************************************************
// 护理病历配置管理系统,报表选择对话框.
// 主要用于导出和导入报表模板功能中,选择导出导入哪些模板
// Author : YangMingkun, Date : 2012-6-10
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Config.Report
{
    internal partial class ReportSelectForm : HerenForm
    {
        private string m_szDescription = "请选择模板：";

        /// <summary>
        /// 获取或设置当前对话框描述信息
        /// </summary>
        [DefaultValue("请选择模板：")]
        public string Description
        {
            get { return this.m_szDescription; }
            set { this.m_szDescription = value; }
        }

        private bool m_bMultiSelect = true;

        /// <summary>
        /// 获取或设置当前是否允许多选
        /// </summary>
        [DefaultValue(true)]
        public bool MultiSelect
        {
            get { return this.m_bMultiSelect; }
            set { this.m_bMultiSelect = value; }
        }

        private string m_szReportType = null;

        /// <summary>
        /// 获取或设置当前加载的报告列表类型
        /// </summary>
        [Browsable(false)]
        public string ReportType
        {
            get { return this.m_szReportType; }
            set { this.m_szReportType = value; }
        }

        private string m_szDefaultTempletID = null;

        /// <summary>
        /// 获取或设置当前默认选中的模板编号.
        /// 注意当有多个默认时,可以使用分号分隔
        /// </summary>
        [Browsable(false)]
        public string DefaultTempletID
        {
            get { return this.m_szDefaultTempletID; }
            set { this.m_szDefaultTempletID = value; }
        }

        private List<ReportTypeInfo> m_lstSelectedTemplets = null;

        /// <summary>
        /// 获取或设置已经勾选的病历类型
        /// </summary>
        [Browsable(false)]
        public List<ReportTypeInfo> SelectedTemplets
        {
            get { return this.m_lstSelectedTemplets; }
        }

        private Hashtable m_htTempletNodes = null;

        public ReportSelectForm()
        {
            this.InitializeComponent();

            string[] templetTypes = ServerData.ReportTypeApplyEnv.GetTypeNames();
            this.cboReportType.Items.AddRange(templetTypes);
            this.cboReportType.Items.Add("<所有报告类型>");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_bMultiSelect)
            {
                this.treeView1.CheckBoxes = true;
                this.treeView1.FullRowSelect = false;
                this.chkCheckAll.Visible = true;
            }
            else
            {
                this.treeView1.CheckBoxes = false;
                this.treeView1.FullRowSelect = true;
                this.chkCheckAll.Visible = false;
            }
            this.lblTipInfo.Text = this.m_szDescription;
            if (!GlobalMethods.Misc.IsEmptyString(this.m_szReportType))
                this.LoadReportTempletList(this.m_szReportType);
            this.treeView1.Focus();

            this.SetDefaultSelectedNodes();

            this.cboReportType.Text =
                ServerData.ReportTypeApplyEnv.GetTypeName(this.m_szReportType);
            this.cboReportType.SelectedIndexChanged +=
                new EventHandler(this.cboReportType_SelectedIndexChanged);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载病历类型信息列表树
        /// </summary>
        private void LoadReportTempletList(string szReportType)
        {
            this.treeView1.Nodes.Clear();
            if (this.m_htTempletNodes == null)
                this.m_htTempletNodes = new Hashtable();
            this.m_htTempletNodes.Clear();
            this.Update();

            List<ReportTypeInfo> lstReportTypeInfos = null;
            short shRet = TempletService.Instance.GetReportTypeInfos(szReportType, ref lstReportTypeInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("报表模板列表下载失败!");
                return;
            }
            if (lstReportTypeInfos == null)
                return;

            //先创建节点哈希列表
            Dictionary<string, TreeNode> nodeTable = new Dictionary<string, TreeNode>();
            foreach (ReportTypeInfo reportTypeInfo in lstReportTypeInfos)
            {
                TreeNode docTypeNode = new TreeNode();
                docTypeNode.Tag = reportTypeInfo;
                docTypeNode.Text = reportTypeInfo.ReportTypeName;

                if (!reportTypeInfo.IsValid)
                    docTypeNode.ForeColor = Color.Silver;

                if (!reportTypeInfo.IsFolder)
                    docTypeNode.ImageIndex = 2;
                else
                    docTypeNode.ImageIndex = 0;
                docTypeNode.SelectedImageIndex = docTypeNode.ImageIndex;

                if (!nodeTable.ContainsKey(reportTypeInfo.ReportTypeID))
                    nodeTable.Add(reportTypeInfo.ReportTypeID, docTypeNode);
                if (!this.m_htTempletNodes.Contains(reportTypeInfo.ReportTypeID))
                    this.m_htTempletNodes.Add(reportTypeInfo.ReportTypeID, docTypeNode);
            }

            //将节点连接起来,添加到树中
            foreach (KeyValuePair<string, TreeNode> pair in nodeTable)
            {
                if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    continue;

                ReportTypeInfo reportTypeInfo = pair.Value.Tag as ReportTypeInfo;
                if (reportTypeInfo == null)
                    continue;

                if (string.IsNullOrEmpty(reportTypeInfo.ParentID))
                {
                    this.treeView1.Nodes.Add(pair.Value);
                }
                else
                {
                    TreeNodeCollection nodes = null;
                    if (!nodeTable.ContainsKey(reportTypeInfo.ParentID))
                        nodes = this.treeView1.Nodes;

                    TreeNode parentNode = nodeTable[reportTypeInfo.ParentID];
                    if (parentNode != null)
                        parentNode.Nodes.Add(pair.Value);
                    else
                        this.treeView1.Nodes.Add(pair.Value);
                }
            }
            this.treeView1.ExpandAll();
            if (this.treeView1.Nodes.Count > 0) this.treeView1.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// 显示默认选中的节点
        /// </summary>
        private void SetDefaultSelectedNodes()
        {
            if (this.m_htTempletNodes == null)
                return;
            if (GlobalMethods.Misc.IsEmptyString(this.m_szDefaultTempletID))
                return;
            //默认选中所有
            if (this.m_szDefaultTempletID.IndexOf("所有") >= 0)
            {
                foreach (DictionaryEntry dic in this.m_htTempletNodes)
                {
                    TreeNode defaultSelectedNode = null;
                    defaultSelectedNode = dic.Value as TreeNode;
                    if (defaultSelectedNode == null)
                        return;
                    defaultSelectedNode.Checked = true;
                }
                return;
            }
            string[] lstTempletIDs = this.m_szDefaultTempletID.Split(';');
            if (lstTempletIDs == null || lstTempletIDs.Length == 0)
                return;
            if (!this.treeView1.CheckBoxes)
            {
                TreeNode defaultSelectedNode = null;
                defaultSelectedNode = this.m_htTempletNodes[lstTempletIDs[0].Trim()] as TreeNode;
                if (defaultSelectedNode == null)
                    return;
                defaultSelectedNode.EnsureVisible();
                this.treeView1.SelectedNode = defaultSelectedNode;
            }
            for (int index = 0; index < lstTempletIDs.Length; index++)
            {
                string szTempletID = lstTempletIDs[index];
                if (!this.m_htTempletNodes.Contains(szTempletID))
                    continue;
                TreeNode defaultSelectedNode = null;
                defaultSelectedNode = this.m_htTempletNodes[szTempletID] as TreeNode;
                if (defaultSelectedNode == null)
                    return;
                defaultSelectedNode.Checked = true;
            }
            this.m_htTempletNodes.Clear();
        }

        /// <summary>
        /// 获取当前TreeView树中已勾选的报表模板信息列表
        /// </summary>
        /// <returns>报表模板信息列表</returns>
        private List<ReportTypeInfo> GetSelectedTempletList()
        {
            List<ReportTypeInfo> lstReportTemplets = new List<ReportTypeInfo>();
            if (!this.treeView1.CheckBoxes)
            {
                TreeNode selectedNode = this.treeView1.SelectedNode;
                if (selectedNode == null)
                    return lstReportTemplets;
                ReportTypeInfo reportTemplet = selectedNode.Tag as ReportTypeInfo;
                if (reportTemplet == null)
                    return lstReportTemplets;
                lstReportTemplets.Add(reportTemplet);
                return lstReportTemplets;
            }
            for (int parentIndex = 0; parentIndex < this.treeView1.Nodes.Count; parentIndex++)
            {
                TreeNode parentNode = this.treeView1.Nodes[parentIndex];
                if (parentNode.Checked)
                    lstReportTemplets.Add(parentNode.Tag as ReportTypeInfo);
                if (parentNode.Nodes.Count > 0)
                    this.GetSelectedTempletList(parentNode, ref lstReportTemplets);
            }
            return lstReportTemplets;
        }

        private void GetSelectedTempletList(TreeNode parentNode, ref List<ReportTypeInfo> lstReportTemplets)
        {
            if (lstReportTemplets == null)
            {
                lstReportTemplets = new List<ReportTypeInfo>();
            }
            for (int nodeIndex = 0; nodeIndex < parentNode.Nodes.Count; nodeIndex++)
            {
                TreeNode tempNode = parentNode.Nodes[nodeIndex];
                if (tempNode.Checked)
                    lstReportTemplets.Add(tempNode.Tag as ReportTypeInfo);
                if (tempNode.Nodes.Count > 0)
                {
                    this.GetSelectedTempletList(tempNode, ref lstReportTemplets);
                }
            }
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int parentIndex = 0; parentIndex < this.treeView1.Nodes.Count; parentIndex++)
            {
                TreeNode parentNode = this.treeView1.Nodes[parentIndex];
                parentNode.Checked = this.chkCheckAll.Checked;
                for (int childIndex = 0; childIndex < parentNode.Nodes.Count; childIndex++)
                {
                    parentNode.Nodes[childIndex].Checked = this.chkCheckAll.Checked;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.btnOK, Cursors.WaitCursor);
            this.m_lstSelectedTemplets = this.GetSelectedTempletList();
            this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this.btnOK, Cursors.Default);
        }

        private void cboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string szReportType = this.cboReportType.Text;
            szReportType = ServerData.ReportTypeApplyEnv.GetTypeCode(szReportType);
            this.LoadReportTempletList(szReportType);
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
                return;
            foreach (TreeNode node in e.Node.Nodes)
                node.Checked = e.Node.Checked;
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.treeView1.CheckBoxes)
                return;
            TreeNode node = this.treeView1.GetNodeAt(e.X, e.Y);
            if (node == null || !node.Bounds.Contains(e.Location))
                this.treeView1.SelectedNode = null;
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.m_bMultiSelect)
                return;
            TreeNode node = this.treeView1.GetNodeAt(e.X, e.Y);
            if (node == null)
                return;
            GlobalMethods.UI.SetCursor(this.btnOK, Cursors.WaitCursor);
            this.m_lstSelectedTemplets = this.GetSelectedTempletList();
            if (this.m_lstSelectedTemplets != null && this.m_lstSelectedTemplets.Count > 0)
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this.btnOK, Cursors.Default);
        }
    }
}