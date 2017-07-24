// ***********************************************************
// 护理病历配置管理系统,报表树形列表窗口.
// Author : YangMingkun, Date : 2012-6-7
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
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Controls.VirtualTreeView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Config.DockForms;

namespace Heren.NurDoc.Config.Report
{
    internal partial class ReportTreeForm : DockContentBase
    {
        public ReportTreeForm(MainForm mainForm)
            : base(mainForm)
        {
            this.HideOnClose = true;
            this.ShowHint = DockState.DockRight;
            this.DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                | DockAreas.DockTop | DockAreas.DockBottom;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.UpdateBounds();
            this.Icon = Properties.Resources.ReportsIcon;

            string[] templetTypes = ServerData.ReportTypeApplyEnv.GetTypeNames();
            this.toolcboReportType.Items.AddRange(templetTypes);
            this.toolcboReportType.SelectedIndex = 0;
            this.toolcboReportType.SelectedIndexChanged +=
                new EventHandler(this.toolcboApplyEnv_SelectedIndexChanged);
        }

        private List<ReportTypeInfo> m_lstReportTypeInfos = null;

        public override void OnRefreshView()
        {
            this.treeView1.Nodes.Clear();
            this.Update();

            string szApplyEnv = this.toolcboReportType.Text;
            szApplyEnv = ServerData.ReportTypeApplyEnv.GetTypeCode(szApplyEnv);
            List<ReportTypeInfo> lstReportTypeInfos = null;
            short shRet = TempletService.Instance.GetReportTypeInfos(szApplyEnv, ref lstReportTypeInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("模板列表下载失败!");
                return;
            }
            m_lstReportTypeInfos = lstReportTypeInfos;
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
                    if (nodeTable.ContainsKey(reportTypeInfo.ParentID))
                    {
                        TreeNode parentNode = nodeTable[reportTypeInfo.ParentID];
                        if (parentNode != null)
                            nodes = parentNode.Nodes;
                    }
                    if (nodes == null)
                        nodes = this.treeView1.Nodes;
                    nodes.Add(pair.Value);
                }
            }
            this.treeView1.ExpandAll();
            if (this.treeView1.Nodes.Count > 0) this.treeView1.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// 返回当前选中的模板节点对应的文档类型信息
        /// </summary>
        /// <param name="bIsDir">是否是目录</param>
        /// <returns>DocTypeInfo</returns>
        private ReportTypeInfo MakeDocTypeInfo(bool bIsDir)
        {
            ReportTypeInfo reportTypeInfo = new ReportTypeInfo();
            reportTypeInfo.ReportTypeID = reportTypeInfo.MakeDocTypeID();
            reportTypeInfo.IsValid = true;
            reportTypeInfo.IsFolder = bIsDir;
            reportTypeInfo.ParentID = string.Empty;
            reportTypeInfo.ReportTypeName = bIsDir ? "新建目录" : "未命名模板";
            reportTypeInfo.ModifyTime = SysTimeService.Instance.Now;
            reportTypeInfo.ReportTypeNo = this.treeView1.Nodes.Count;

            string szApplyEnv = this.toolcboReportType.Text;
            szApplyEnv = ServerData.ReportTypeApplyEnv.GetTypeCode(szApplyEnv);
            reportTypeInfo.ApplyEnv = szApplyEnv;

            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return reportTypeInfo;

            ReportTypeInfo selectedReportType = selectedNode.Tag as ReportTypeInfo;
            if (selectedReportType == null)
                return reportTypeInfo;

            if (selectedReportType.IsFolder)
            {
                reportTypeInfo.ParentID = selectedReportType.ReportTypeID;
                reportTypeInfo.ReportTypeNo = selectedNode.Nodes.Count;
            }
            else
            {
                reportTypeInfo.ParentID = selectedReportType.ParentID;
                if (selectedNode.Parent != null)
                    reportTypeInfo.ReportTypeNo = selectedNode.Parent.Nodes.Count;
            }
            return reportTypeInfo;
        }

        /// <summary>
        /// 创建一个新的目录或者类型节点
        /// </summary>
        /// <param name="bIsDir">是否是目录</param>
        /// <param name="reportTypeInfo">病历类型信息</param>
        /// <returns>节点</returns>
        private TreeNode CreateNewNode(bool bIsDir, ReportTypeInfo reportTypeInfo)
        {
            TreeNode node = new TreeNode();
            node.Tag = reportTypeInfo;
            if (reportTypeInfo == null)
                node.Text = bIsDir ? "新建目录" : "未命名模板";
            else
                node.Text = reportTypeInfo.ReportTypeName;
            node.ImageIndex = bIsDir ? 0 : 2;
            node.SelectedImageIndex = node.ImageIndex;

            if (!reportTypeInfo.IsValid)
                node.ForeColor = Color.Silver;

            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
            {
                this.treeView1.Nodes.Add(node);
                return node;
            }

            ReportTypeInfo selectedReportType = selectedNode.Tag as ReportTypeInfo;
            if (selectedReportType == null)
            {
                this.treeView1.Nodes.Add(node);
                return node;
            }

            TreeNode parentNode = selectedNode;
            if (!selectedReportType.IsFolder)
                parentNode = selectedNode.Parent;
            if (parentNode != null)
            {
                parentNode.Nodes.Add(node);
                parentNode.Expand();
            }

            if (node.TreeView != null)
            {
                this.treeView1.SelectedNode = node;
                node.BeginEdit();
                return node;
            }
            return null;
        }

        /// <summary>
        /// 在当前树节点位置处创建目录节点
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateFolder()
        {
            ReportInfoForm templetInfoForm = new ReportInfoForm();
            templetInfoForm.IsNew = true;
            templetInfoForm.IsFolder = true;
            templetInfoForm.ReportTypeInfo = this.MakeDocTypeInfo(true);
            if (templetInfoForm.ShowDialog() != DialogResult.OK)
                return false;

            ReportTypeInfo reportTypeInfo = templetInfoForm.ReportTypeInfo;
            if (reportTypeInfo == null)
                return false;
            short shRet = TempletService.Instance.SaveReportTypeInfo(reportTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("目录创建失败,无法更新到数据库!");
                return false;
            }
            return this.CreateNewNode(true, reportTypeInfo) != null;
        }

        /// <summary>
        /// 在当前树节点位置处创建模板节点
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateDocType()
        {
            ReportInfoForm templetInfoForm = new ReportInfoForm();
            templetInfoForm.IsNew = true;
            templetInfoForm.IsFolder = false;
            templetInfoForm.ReportTypeInfo = this.MakeDocTypeInfo(false);
            if (templetInfoForm.ShowDialog() != DialogResult.OK)
                return false;

            ReportTypeInfo reportTypeInfo = templetInfoForm.ReportTypeInfo;
            if (reportTypeInfo == null)
                return false;
            short shRet = TempletService.Instance.SaveReportTypeInfo(reportTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("模板创建失败,无法更新到数据库!");
                return false;
            }
            this.CreateNewNode(false, reportTypeInfo);
            ReportHandler.Instance.OpenReport(reportTypeInfo);

            string szDocTypeID = reportTypeInfo.ReportTypeID;
            List<WardReportType> lstWardReportTypes = templetInfoForm.WardReportTypes;
            shRet = TempletService.Instance.SaveWardReportTypes(szDocTypeID, lstWardReportTypes);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("模板创建成功,但所属科室信息保存失败!");
                return true;
            }

            return true;
        }

        /// <summary>
        /// 显示选中的模板的信息,并接受修改
        /// </summary>
        private void ShowTempletInfoEditForm()
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return;
            ReportTypeInfo reportTypeInfo = selectedNode.Tag as ReportTypeInfo;
            if (reportTypeInfo == null)
                return;
            string szDocTypeID = reportTypeInfo.ReportTypeID;

            ReportInfoForm templetInfoForm = new ReportInfoForm();
            templetInfoForm.IsNew = false;
            templetInfoForm.IsFolder = reportTypeInfo.IsFolder;
            templetInfoForm.ReportTypeInfo = reportTypeInfo.Clone() as ReportTypeInfo;
            DialogResult result = templetInfoForm.ShowDialog();
            if (result != DialogResult.OK)
                return;

            reportTypeInfo = templetInfoForm.ReportTypeInfo;
            if (reportTypeInfo == null)
                return;
            short shRet = TempletService.Instance.ModifyReportTypeInfo(szDocTypeID, reportTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("模板创建失败,无法更新到数据库!");
                return;
            }
            selectedNode.Tag = reportTypeInfo;
            selectedNode.Text = reportTypeInfo.ReportTypeName;
            if (!reportTypeInfo.IsValid)
                selectedNode.ForeColor = Color.Silver;
            else
                selectedNode.ForeColor = Color.Black;

            List<WardReportType> lstWardReportTypes = templetInfoForm.WardReportTypes;
            shRet = TempletService.Instance.SaveWardReportTypes(szDocTypeID, lstWardReportTypes);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("模板创建成功,但所属科室信息保存失败!");
                return;
            }
        }

        /// <summary>
        /// 修改指定的节点显示文本
        /// </summary>
        /// <param name="node">节点对象</param>
        /// <param name="szNewText">修改文本</param>
        /// <returns>bool</returns>
        private bool ModifyNodeText(TreeNode node, string szNewText)
        {
            if (node == null || GlobalMethods.Misc.IsEmptyString(szNewText))
                return false;
            ReportTypeInfo reportTypeInfo = node.Tag as ReportTypeInfo;
            if (reportTypeInfo == null || reportTypeInfo.ReportTypeName == szNewText)
                return false;

            string szDocTypeID = reportTypeInfo.ReportTypeID;
            string szOldDocTypeName = reportTypeInfo.ReportTypeName;
            reportTypeInfo.ReportTypeName = szNewText;

            short shRet = TempletService.Instance.ModifyReportTypeInfo(szDocTypeID, reportTypeInfo);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                node.Text = szNewText;
                return true;
            }
            reportTypeInfo.ReportTypeName = szOldDocTypeName;
            MessageBoxEx.Show(string.Format("“{0}”重命名失败!", reportTypeInfo.ReportTypeName));
            return false;
        }

        /// <summary>
        /// 获取指定节点下所有子节点的文档类型ID列表
        /// </summary>
        /// <param name="parentNode">模板父节点</param>
        /// <param name="lstDocTypeID">文档类型ID列表</param>
        private void GetDocTypeList(TreeNode parentNode, ref List<string> lstDocTypeID)
        {
            if (lstDocTypeID == null)
                lstDocTypeID = new List<string>();
            if (parentNode == null)
                return;

            ReportTypeInfo reportTypeInfo = parentNode.Tag as ReportTypeInfo;
            if (reportTypeInfo == null)
                return;
            lstDocTypeID.Add(reportTypeInfo.ReportTypeID);
            for (int index = 0; index < parentNode.Nodes.Count; index++)
                this.GetDocTypeList(parentNode.Nodes[index], ref lstDocTypeID);
        }

        /// <summary>
        /// 删除指定TreeNode节点
        /// </summary>
        /// <param name="deletedNode">节点对象</param>
        private void DeleteNode(TreeNode deletedNode)
        {
            if (deletedNode == null || deletedNode.Tag == null)
                return;

            List<string> lstDocTypeID = null;
            this.GetDocTypeList(deletedNode, ref lstDocTypeID);
            if (lstDocTypeID == null || lstDocTypeID.Count <= 0)
                return;

            string szPopupInfo = "确认彻底删除“" + deletedNode.Text + "”吗？"
                        + "\r\n警告：\r\n删除报表类型将导致基于该类型书写的报表无法再编辑。"
                        + "\r\n如果系统已经上线,我们建议您不要彻底删除而将其设置为不可见。";
            DialogResult result = MessageBoxEx.ShowConfirm(szPopupInfo);
            if (result != DialogResult.OK)
                return;

            short shRet = TempletService.Instance.DeleteReportTypeInfos(lstDocTypeID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("删除“{0}”失败!", deletedNode.Text));
                return;
            }
            deletedNode.Remove();
        }

        /// <summary>
        /// 在同一个父节点内,向上或者向下移动指定的节点
        /// </summary>
        /// <param name="moveNode">被移动的节点</param>
        /// <param name="bIsMoveToDown">是否是向下移动</param>
        private void MoveNode(TreeNode moveNode, bool bIsMoveToDown)
        {
            if (moveNode == null || moveNode.Index < 0 || moveNode.Tag == null)
                return;

            ReportTypeInfo reportTypeInfo = moveNode.Tag as ReportTypeInfo;
            if (reportTypeInfo == null)
                return;

            TreeNodeCollection nodes = null;
            if (moveNode.Parent != null)
                nodes = moveNode.Parent.Nodes;
            else
                nodes = this.treeView1.Nodes;

            if (bIsMoveToDown && moveNode.Index >= nodes.Count - 1)
                return;
            if (!bIsMoveToDown && moveNode.Index <= 0)
                return;

            bool bIsExpanded = moveNode.IsExpanded;
            bool bIsSelected = moveNode.IsSelected;

            int index = 0;
            if (bIsMoveToDown)
                index = moveNode.Index + 2;
            else
                index = moveNode.Index - 1;
            TreeNode targetNode = moveNode.Clone() as TreeNode;
            nodes.Insert(index, targetNode);
            nodes.Remove(moveNode);

            if (bIsExpanded)
                targetNode.ExpandAll();
            if (bIsSelected)
                this.treeView1.SelectedNode = targetNode;

            //更新相关兄弟节点的顺序值
            for (int childIndex = 0; childIndex < nodes.Count; childIndex++)
            {
                TreeNode childNode = nodes[childIndex];
                if (childNode == null)
                    continue;
                reportTypeInfo = childNode.Tag as ReportTypeInfo;
                if (reportTypeInfo == null)
                    continue;
                int nOldValue = reportTypeInfo.ReportTypeNo;
                reportTypeInfo.ReportTypeNo = childIndex;
                short shRet = TempletService.Instance.ModifyReportTypeInfo(reportTypeInfo.ReportTypeID, reportTypeInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    //失败后还原被更新的对象属性
                    reportTypeInfo.ReportTypeNo = nOldValue;
                    MessageBoxEx.Show(string.Format("无法移动“{0}”!", reportTypeInfo.ReportTypeName));
                    return;
                }
            }
        }

        /// <summary>
        /// 在整棵TreeView树内,移动指定的节点到目标节点之前
        /// </summary>
        /// <param name="moveNode">被移动的节点</param>
        /// <param name="targetNode">目标节点</param>
        private void DragNode(TreeNode moveNode, TreeNode targetNode)
        {
            if (moveNode == null || moveNode.Index < 0 || moveNode.Tag == null)
                return;

            ReportTypeInfo moveReportTypeInfo = moveNode.Tag as ReportTypeInfo;
            if (moveReportTypeInfo == null)
                return;

            ReportTypeInfo targetReportTypeInfo = null;
            if (targetNode != null)
                targetReportTypeInfo = targetNode.Tag as ReportTypeInfo;

            //把父节点拖放到其下子节点,则取消
            TreeNode parentNode = targetNode;
            while (parentNode != null)
            {
                if (parentNode == moveNode)
                    return;
                parentNode = parentNode.Parent;
            }
            if (targetNode == moveNode.Parent)
                return;

            //寻找父目录节点以及目标位置
            string szParentID = string.Empty;
            int nTargetIndex = this.treeView1.Nodes.Count;
            if (targetReportTypeInfo != null)
            {
                nTargetIndex = targetNode.Index;
                if (!targetReportTypeInfo.IsFolder)
                {
                    targetNode = targetNode.Parent;
                    szParentID = targetReportTypeInfo.ParentID;
                }
                else
                {
                    nTargetIndex = targetNode.Nodes.Count;
                    szParentID = targetReportTypeInfo.ReportTypeID;
                }
            }

            TreeNodeCollection nodes = null;
            if (targetNode != null)
                nodes = targetNode.Nodes;
            else
                nodes = this.treeView1.Nodes;

            //开始移动节点
            bool bIsExpanded = moveNode.IsExpanded;
            bool bIsSelected = moveNode.IsSelected;
            TreeNode newNode = (TreeNode)moveNode.Clone();
            nodes.Insert(nTargetIndex, newNode);
            moveNode.Remove();
            if (bIsExpanded)
                newNode.ExpandAll();
            if (bIsSelected)
                this.treeView1.SelectedNode = newNode;

            //更新相关兄弟节点的顺序值
            for (int childIndex = 0; childIndex < nodes.Count; childIndex++)
            {
                TreeNode childNode = nodes[childIndex];
                if (childNode == null)
                    continue;
                ReportTypeInfo reportTypeInfo = childNode.Tag as ReportTypeInfo;
                if (reportTypeInfo == null)
                    continue;
                string szOldParentID = reportTypeInfo.ParentID;
                int nDocTypeOldNo = reportTypeInfo.ReportTypeNo;

                reportTypeInfo.ReportTypeNo = childIndex;
                reportTypeInfo.ParentID = szParentID;
                short shRet = TempletService.Instance.ModifyReportTypeInfo(reportTypeInfo.ReportTypeID, reportTypeInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    //失败后还原被更新的对象属性
                    reportTypeInfo.ReportTypeNo = nDocTypeOldNo;
                    reportTypeInfo.ParentID = szOldParentID;
                    MessageBoxEx.Show(string.Format("“{0}”移动失败!", reportTypeInfo.ReportTypeName));
                    return;
                }
            }
        }

        /// <summary>
        /// 将指定的文本数据写入result.txt中,同时使用记事本打开
        /// </summary>
        /// <param name="szTextData">文本数据</param>
        private void ShowTextData(string szTextData)
        {
            string szResultFile = string.Format("{0}\\result.txt", Application.StartupPath);
            GlobalMethods.IO.WriteFileText(szResultFile, szTextData);
            try
            {
                System.Diagnostics.Process.Start(szResultFile);
            }
            catch { MessageBoxEx.Show("无法显示模板导出结果信息!"); }
        }

        /// <summary>
        /// 将服务器上的模板导出到本地目录
        /// </summary>
        public void ExportTemplet()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "请选择模板导出目录：";
            folderDialog.ShowNewFolderButton = true;
            if (folderDialog.ShowDialog() != DialogResult.OK)
                return;
            string szDirPath = folderDialog.SelectedPath;

            ReportSelectForm frmTempletSelect = new ReportSelectForm();
            frmTempletSelect.MultiSelect = true;
            frmTempletSelect.Description = "请选择需要导出的报表类型模板：";

            string szApplyEnv = this.toolcboReportType.Text;
            szApplyEnv = ServerData.ReportTypeApplyEnv.GetTypeCode(szApplyEnv);
            frmTempletSelect.ReportType = szApplyEnv;

            if (frmTempletSelect.ShowDialog() != DialogResult.OK)
                return;
            List<ReportTypeInfo> lstReportTypeInfos = frmTempletSelect.SelectedTemplets;
            if (lstReportTypeInfos == null || lstReportTypeInfos.Count <= 0)
                return;

            WorkProcess.Instance.Initialize(this.MainForm, lstReportTypeInfos.Count, "正在导出系统模板...");

            StringBuilder sbExecuteResult = new StringBuilder();
            short shRet = SystemConst.ReturnValue.OK;
            for (int index = 0; index <= lstReportTypeInfos.Count - 1; index++)
            {
                WorkProcess.Instance.Show(null, index + 1);
                if (WorkProcess.Instance.Canceled)
                    break;

                ReportTypeInfo reportTypeInfo = lstReportTypeInfos[index];
                if (reportTypeInfo == null || reportTypeInfo.IsFolder)
                    continue;

                //从服务器上下载模板
                byte[] byteTempletData = null;
                shRet = TempletService.Instance.GetReportTemplet(reportTypeInfo.ReportTypeID, ref byteTempletData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    sbExecuteResult.AppendLine("----导出失败! ReportTypeName=" + reportTypeInfo.ReportTypeName);
                    continue;
                }

                //将模板写入本地文件
                string szTempletFile = string.Format("{0}\\{1}.hrdt", szDirPath, reportTypeInfo.ReportTypeName);
                if (!GlobalMethods.IO.WriteFileBytes(szTempletFile, byteTempletData))
                {
                    sbExecuteResult.AppendLine("----导出失败! ReportTypeName=" + reportTypeInfo.ReportTypeName);
                    continue;
                }
                sbExecuteResult.AppendLine("导出成功! ReportTypeName=" + reportTypeInfo.ReportTypeName);
            }
            WorkProcess.Instance.Close();
            this.ShowTextData(sbExecuteResult.ToString());
        }

        /// <summary>
        /// 将本地目录下的模板提交到服务器
        /// </summary>
        public void ImportTemplet()
        {
            ReportSelectForm frmTempletSelect = new ReportSelectForm();
            frmTempletSelect.MultiSelect = true;
            frmTempletSelect.Description = "请选择需要导入的报表类型模板：";

            string szApplyEnv = this.toolcboReportType.Text;
            szApplyEnv = ServerData.ReportTypeApplyEnv.GetTypeCode(szApplyEnv);
            frmTempletSelect.ReportType = szApplyEnv;

            if (frmTempletSelect.ShowDialog() != DialogResult.OK)
                return;
            List<ReportTypeInfo> lstReportTypeInfos = frmTempletSelect.SelectedTemplets;
            if (lstReportTypeInfos == null || lstReportTypeInfos.Count <= 0)
                return;

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "请选择模板在本地的存放目录：";
            folderDialog.ShowNewFolderButton = false;
            if (folderDialog.ShowDialog() != DialogResult.OK)
                return;
            string szDirPath = folderDialog.SelectedPath;

            WorkProcess.Instance.Initialize(this.MainForm, lstReportTypeInfos.Count, "正在导入系统模板...");

            StringBuilder sbExecuteResult = new StringBuilder();
            short shRet = SystemConst.ReturnValue.OK;
            for (int index = 0; index <= lstReportTypeInfos.Count - 1; index++)
            {
                WorkProcess.Instance.Show(null, index + 1);
                if (WorkProcess.Instance.Canceled)
                    break;

                ReportTypeInfo reportTypeInfo = lstReportTypeInfos[index];
                if (reportTypeInfo == null || reportTypeInfo.IsFolder)
                    continue;

                //读取本地模板文件
                byte[] byteTempletData = null;
                string szTempletFile = string.Format("{0}\\{1}.hrdt", szDirPath, reportTypeInfo.ReportTypeName);
                if (!GlobalMethods.IO.GetFileBytes(szTempletFile, ref byteTempletData))
                {
                    sbExecuteResult.AppendLine("----导入失败! ReportTypeName=" + reportTypeInfo.ReportTypeName);
                    continue;
                }

                //保存模板到服务器
                shRet = TempletService.Instance.SaveReportTemplet(reportTypeInfo.ReportTypeID, byteTempletData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    sbExecuteResult.AppendLine("----导入失败! ReportTypeName=" + reportTypeInfo.ReportTypeName);
                    continue;
                }
                sbExecuteResult.AppendLine("导入成功! ReportTypeName=" + reportTypeInfo.ReportTypeName);
            }
            WorkProcess.Instance.Close();
            this.ShowTextData(sbExecuteResult.ToString());
        }

        private void toolbtnOpen_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            ReportHandler.Instance.OpenReport();
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void toolbtnImport_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            this.ImportTemplet();
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void toolbtnExport_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            this.ExportTemplet();
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void mnuNewFolder_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            this.CreateFolder();
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void mnuNewTemplet_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            this.CreateDocType();
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void mnuTempletProperty_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            this.ShowTempletInfoEditForm();
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            this.DeleteNode(this.treeView1.SelectedNode);
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void toolcboApplyEnv_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnRefreshView();
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null || GlobalMethods.Misc.IsEmptyString(e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            e.CancelEdit = !this.ModifyNodeText(e.Node, e.Label.Trim());
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node == null)
                return;
            this.treeView1.SelectedNode = node;
            if (e.Button == MouseButtons.Left)
            {
                this.treeView1.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            Point ptMousePos = new Point(e.X, e.Y);
            ptMousePos = this.treeView1.PointToClient(ptMousePos);
            TreeNode node = this.treeView1.GetNodeAt(ptMousePos);
            if (node == null)
                return;

            int nHeight = this.treeView1.Height;
            int nItemHeight = this.treeView1.ItemHeight;
            if (ptMousePos.Y < nItemHeight && node.PrevVisibleNode != null)
                node.PrevVisibleNode.EnsureVisible();
            else if (ptMousePos.Y > nHeight - nItemHeight && node.NextVisibleNode != null)
                node.NextVisibleNode.EnsureVisible();
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode moveNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (moveNode == null)
                return;

            Point ptMousePos = new Point(e.X, e.Y);
            ptMousePos = this.treeView1.PointToClient(ptMousePos);
            TreeNode targetNode = this.treeView1.GetNodeAt(ptMousePos);

            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            this.DragNode(moveNode, targetNode);
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.WaitCursor);
            ReportTypeInfo reportTypeInfo = e.Node.Tag as ReportTypeInfo;
            if (reportTypeInfo != null && !reportTypeInfo.IsFolder)
                ReportHandler.Instance.OpenReport(e.Node.Tag as ReportTypeInfo);
            GlobalMethods.UI.SetCursor(this.treeView1, Cursors.Default);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (this.textBox1.Text == "")
            //    this.OnRefreshView();
            //else
            this.QueryTempInfos(this.textBox1.Text);
        }

        private void QueryTempInfos(string sztext)
        {
            this.treeView1.Nodes.Clear();

            //先创建节点哈希列表
            Dictionary<string, TreeNode> nodeTable = new Dictionary<string, TreeNode>();
            foreach (ReportTypeInfo reportTypeInfo in m_lstReportTypeInfos)
            {
                TreeNode docTypeNode = new TreeNode();
                docTypeNode.Tag = reportTypeInfo;
                docTypeNode.Text = reportTypeInfo.ReportTypeName;

                if (!reportTypeInfo.IsValid )
                    docTypeNode.ForeColor = Color.Silver;

                if (!reportTypeInfo.IsFolder)
                    docTypeNode.ImageIndex = 2;
                else
                    docTypeNode.ImageIndex = 0;
                docTypeNode.SelectedImageIndex = docTypeNode.ImageIndex;

                if (!nodeTable.ContainsKey(reportTypeInfo.ReportTypeID))
                    nodeTable.Add(reportTypeInfo.ReportTypeID, docTypeNode);
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
                    if (reportTypeInfo.IsFolder)
                        this.treeView1.Nodes.Add(pair.Value);
                    else if (reportTypeInfo.ReportTypeName.ToLower().Contains(sztext.ToLower()))
                        this.treeView1.Nodes.Add(pair.Value);
                }
                else
                {
                    TreeNodeCollection nodes = null;
                    if (nodeTable.ContainsKey(reportTypeInfo.ParentID))
                    {
                        TreeNode parentNode = nodeTable[reportTypeInfo.ParentID];
                        if (parentNode != null)
                            nodes = parentNode.Nodes;
                    }
                    if (nodes == null)
                        nodes = this.treeView1.Nodes;
                    if (reportTypeInfo.IsFolder)
                        nodes.Add(pair.Value);
                    else if (reportTypeInfo.ReportTypeName.ToLower().Contains(sztext.ToLower()))
                        nodes.Add(pair.Value);
                }
            }

            this.treeView1.ExpandAll();
            for (int index = treeView1.Nodes.Count - 1; index >= 0; index--)
            {
                if (this.treeView1.Nodes.Count <= 0)
                    continue;
                ReportTypeInfo reportTypeInfo = this.treeView1.Nodes[index].Tag as ReportTypeInfo;
                if (this.treeView1.Nodes[index].Nodes.Count <= 0
                    && !(reportTypeInfo.ReportTypeName.ToLower().Contains(sztext.ToLower())
                    && !reportTypeInfo.IsFolder))
                {
                    this.treeView1.Nodes.RemoveAt(index);
                }
                else if (this.treeView1.Nodes[index].Nodes.Count > 0)
                {
                    CleanEmptyNode(this.treeView1.Nodes[index], sztext);
                }
            }

            this.treeView1.ExpandAll();
            if (this.treeView1.Nodes.Count > 0)
                this.treeView1.Nodes[0].EnsureVisible();
        }

        private void CleanEmptyNode(TreeNode FatherNode, string sztext)
        {
            for (int index = FatherNode.Nodes.Count - 1; index >= 0; index--)
            {
                if (FatherNode.Nodes.Count <= 0)
                    continue;
                ReportTypeInfo reportTypeInfo = FatherNode.Nodes[index].Tag as ReportTypeInfo;
                if (FatherNode.Nodes[index].Nodes.Count <= 0
                   && !(reportTypeInfo.ReportTypeName.ToLower().Contains(sztext.ToLower())
                   && !reportTypeInfo.IsFolder))
                {
                    FatherNode.Nodes[index].Remove();
                }
                else if (FatherNode.Nodes[index].Nodes.Count > 0)
                    CleanEmptyNode(FatherNode.Nodes[index], sztext);
            }
            if (FatherNode.Nodes.Count == 0)
                FatherNode.Remove();
        }
    }
}