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

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class EvaluationForm : DockContentBase
    {
        /// <summary>
        /// 获取或设置当前活动的文档类型对象
        /// </summary>
        [Browsable(false)]
        public EvaTypeInfo EvaTypeInfo
        {
            get
            {
                if (this.m_EvaluationListForm == null)
                    return null;
                if (this.m_EvaluationListForm.IsDisposed)
                    return null;
                return this.m_EvaluationListForm.EvaTypeInfo;
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
                if (this.m_EvaluationListForm == null)
                    return false;
                if (this.m_EvaluationListForm.IsDisposed)
                    return false;
                return this.m_EvaluationListForm.IsModified;
            }
        }

        /// <summary>
        /// 显示已写病历的列表的窗口
        /// </summary>
        private EvaluationListForm m_EvaluationListForm = null;

        /// <summary>
        /// 选择的科室信息
        /// </summary>
        private DeptInfo m_SelectDeptInfo = null;

        /// <summary>
        /// 病历类型与节点对应关系表
        /// </summary>
        private Dictionary<string, VirtualNode> m_nodeTable = null;

        /// <summary>
        /// 全局变量
        /// </summary>
        private List<EvaTypeInfo> m_lstEvaTypeInfos = null;

        /// <summary>
        /// 判断护理评价类型所属病区时使用的Hash表
        /// </summary>
        private Dictionary<string, WardEvaType> m_htWardEvaTable = null;

        public EvaluationForm(MainFrame parent)
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
            this.InitDeptList();
            //this.LoadEvaTypeList();
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_EvaluationListForm != null && !this.m_EvaluationListForm.IsDisposed
                && this.m_EvaluationListForm.Visible)
            {
                this.ShowDocumentListForm(this.EvaTypeInfo);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 获取指定的文档类型是否是当前用户病区的
        /// </summary>
        /// <param name="szDocTypeID">指定的文档类型</param>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>是否是当前病区病历</returns>
        public bool IsWardDocType(string szEvaTypeID, string szWardCode)
        {
            if (this.m_htWardEvaTable == null)
            {
                if (!this.LoadWardEvaTypeList())
                    return false;
            }
            if (this.m_htWardEvaTable == null || SystemContext.Instance.LoginUser == null)
                return true;

            //如果护理评价类型未设置所属病区,那么认为是通用
            if (string.IsNullOrEmpty(szEvaTypeID) || !this.m_htWardEvaTable.ContainsKey(szEvaTypeID))
                return true;

            //返回该护理评价类型所属病区中是否有当前用户病区
            string szWardEvaType = string.Concat(szEvaTypeID, "_", szWardCode);
            if (this.m_htWardEvaTable.ContainsKey(szWardEvaType))
                return true;
            return false;
            //返回该护理评价类型所属用户组中是否有该用户所在的小组
            //return this.CanOpenEvaInfo(m_htWardEvaTable, szEvaTypeID);
        }

        /// <summary>
        /// 判断用户是否有打开该类型文档的权限
        /// </summary>
        /// <param name="htWardDocTable">文档类型所属病区时使用的Hash表</param>
        /// <param name="szDocTypeID">文档类型ID号</param>
        /// <returns>bool</returns>
        public bool CanOpenEvaInfo(Dictionary<string, WardEvaType> htWardEvaTable, string szEvaTypeID)
        {
            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<DeptInfo> lstDeptInfos = SystemContext.Instance.GetUserDeptList(szUserID);
            foreach (DeptInfo deptInfo in lstDeptInfos)
            {
                string szWardDocType = string.Concat(szEvaTypeID, "_", deptInfo.DeptCode);
                if (htWardEvaTable.ContainsKey(szWardDocType))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 加载当前病区的病历类型列表
        /// </summary>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        private bool LoadWardEvaTypeList()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            List<WardEvaType> lstWardEvaTypes = null;
            short shRet = EvaTypeService.Instance.GetEvaTypeDeptList(string.Empty, ref lstWardEvaTypes);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;

            if (this.m_htWardEvaTable == null)
                this.m_htWardEvaTable = new Dictionary<string, WardEvaType>();
            this.m_htWardEvaTable.Clear();

            if (lstWardEvaTypes == null || lstWardEvaTypes.Count <= 0)
                return true;

            string szWardEvaType = null;
            for (int index = 0; index < lstWardEvaTypes.Count; index++)
            {
                WardEvaType wardEvaType = lstWardEvaTypes[index];
                if (wardEvaType == null || string.IsNullOrEmpty(wardEvaType.EvaTypeID))
                    continue;

                //将病历类型的第一个所属病区信息加入到hash表
                //以便后续较方便的判断该病历类型是否设置了所属病区
                if (!this.m_htWardEvaTable.ContainsKey(wardEvaType.EvaTypeID))
                    this.m_htWardEvaTable.Add(wardEvaType.EvaTypeID, wardEvaType);

                //同时将病历类型与病区的对应关系加入到hash表
                szWardEvaType = string.Concat(wardEvaType.EvaTypeID, "_", wardEvaType.WardCode);
                if (!this.m_htWardEvaTable.ContainsKey(szWardEvaType))
                    this.m_htWardEvaTable.Add(szWardEvaType, wardEvaType);
            }
            return true;
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.m_EvaluationListForm == null)
                return false;
            if (this.m_EvaluationListForm.IsDisposed)
                return false;
            return this.m_EvaluationListForm.CheckModifyState();
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

        private bool InitDeptList()
        {
            List<DeptInfo> lstDepts = null;
            short shRet = CommonService.Instance.GetNurseDeptList(ref lstDepts);

            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("获取科室列表失败!");
                return false;
            }
            if (lstDepts == null || lstDepts.Count <= 0)
                return false;

            this.toolstpDept.Items.Clear();
            this.toolstpDept.ComboBox.DataSource = lstDepts;
            this.toolstpDept.ComboBox.DisplayMember = "DeptName";
            this.toolstpDept.ComboBox.ValueMember = "DeptCode";

            this.toolstpDept.ComboBox.SelectedValue = SystemContext.Instance.DisplayDept.DeptCode;

            NurUserRight userRight = RightController.Instance.UserRight;
            if (userRight.LeaderNurse.Value)
                this.toolstpDept.Enabled = true;
            else
                this.toolstpDept.Enabled = false;
            return true;
        }

        private bool LoadEvaTypeList()
        {
            if (this.m_nodeTable == null)
                this.m_nodeTable = new Dictionary<string, VirtualNode>();
            this.m_nodeTable.Clear();

            this.virtualTree1.Nodes.Clear();
            Application.DoEvents();

            List<EvaTypeInfo> lstEvaTypeInfos = null;
            short shRet = EvaTypeService.Instance.GetEvaTypeInfos(ref lstEvaTypeInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理病历评估模板列表下载失败!");
                return false;
            }
            if (lstEvaTypeInfos == null || lstEvaTypeInfos.Count <= 0)
                return true;
            m_lstEvaTypeInfos = lstEvaTypeInfos;

            foreach (EvaTypeInfo evaTypeInfo in lstEvaTypeInfos)
            {
                if (evaTypeInfo == null)
                    continue;
                if (!evaTypeInfo.IsValid || !evaTypeInfo.IsVisible)
                    continue;
                if (!this.IsWardDocType(evaTypeInfo.EvaTypeID, this.m_SelectDeptInfo.DeptCode))
                    continue;

                VirtualNode evaTypeNode = new VirtualNode();
                evaTypeNode.Tag = evaTypeInfo;
                evaTypeNode.Text = evaTypeInfo.EvaTypeName;

                if (!evaTypeInfo.IsFolder)
                    evaTypeNode.ImageIndex = 2;
                else
                    evaTypeNode.ImageIndex = 0;
                evaTypeNode.ImageIndex = evaTypeNode.ImageIndex;

                if (!this.m_nodeTable.ContainsKey(evaTypeInfo.EvaTypeID))
                    this.m_nodeTable.Add(evaTypeInfo.EvaTypeID, evaTypeNode);
            }

            //将节点连接起来,添加到树中
            foreach (KeyValuePair<string, VirtualNode> pair in this.m_nodeTable)
            {
                if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    continue;

                EvaTypeInfo evaTypeInfo = pair.Value.Tag as EvaTypeInfo;
                if (evaTypeInfo == null)
                    continue;

                if (string.IsNullOrEmpty(evaTypeInfo.ParentID))
                {
                    this.virtualTree1.Nodes.Add(pair.Value);
                }
                else
                {
                    if (!this.m_nodeTable.ContainsKey(evaTypeInfo.ParentID))
                        continue;
                    VirtualNodeList nodes = this.m_nodeTable[evaTypeInfo.ParentID].Nodes;
                    nodes.Add(pair.Value);
                }
            }

            string key = SystemConst.ConfigKey.EVALUATION_EXPAND;
            if (SystemContext.Instance.GetConfig(key, true))
                this.virtualTree1.ExpandAll();
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        /// <summary>
        /// 显示指定病历类型对应的病历列表窗口
        /// </summary>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>是否执行成功</returns>
        private bool ShowDocumentListForm(EvaTypeInfo evaTypeInfo)
        {
            if (evaTypeInfo == null || evaTypeInfo.IsFolder)
                return false;

            this.CreateDocumentListForm(evaTypeInfo);
            this.m_EvaluationListForm.SelectDeptInfo = this.m_SelectDeptInfo;
            this.m_EvaluationListForm.RefreshView();
            return true;
        }


        /// <summary>
        /// 创建文档列表窗口
        /// </summary>
        /// <param name="docTypeInfo">文档类型</param>
        private void CreateDocumentListForm(EvaTypeInfo evaTypeInfo)
        {
            if (this.m_EvaluationListForm == null || this.m_EvaluationListForm.IsDisposed)
            {
                this.m_EvaluationListForm = new EvaluationListForm();
                this.m_EvaluationListForm.TopLevel = false;
                this.m_EvaluationListForm.FormBorderStyle = FormBorderStyle.None;
                this.m_EvaluationListForm.Dock = DockStyle.Fill;
                this.m_EvaluationListForm.Padding = new Padding(0);
                this.m_EvaluationListForm.Parent = this;
                this.m_EvaluationListForm.DocumentUpdated +=
                    new EventHandler(this.DocumentListForm_DocumentUpdated);
            }
            this.m_EvaluationListForm.EvaTypeInfo = evaTypeInfo;
            this.m_EvaluationListForm.Visible = true;
            this.m_EvaluationListForm.BringToFront();
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

        private void DocumentListForm_DocumentUpdated(object sender, EventArgs e)
        {
            this.formControl1.UpdateFormData("刷新视图", null);
        }

        private void virtualTree1_NodeMouseClick(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;

            //检查是否已经打开
            EvaTypeInfo evaTypeInfo = e.Node.Tag as EvaTypeInfo;
            if (evaTypeInfo == null || evaTypeInfo.IsFolder)
                return;
            if (this.EvaTypeInfo != null && this.EvaTypeInfo.EvaTypeID == evaTypeInfo.EvaTypeID)
                return;

            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            if (this.ShowDocumentListForm(evaTypeInfo))
            {
                this.ResetNodeTextAndColor(null, true, false, false);
                e.Node.Font = new Font(this.virtualTree1.Font, FontStyle.Bold);
            }
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.mnuExpandAll.Checked = SystemContext.Instance.GetConfig(SystemConst.ConfigKey.EVALUATION_EXPAND, true); ;
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            this.mnuExpandAll.Checked = !this.mnuExpandAll.Checked;
            if (this.mnuExpandAll.Checked)
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();

            string key = SystemConst.ConfigKey.EVALUATION_EXPAND;
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
            else if (e.Name == "刷新数据")
            {
                //SystemContext.Instance.OnUserChanged(this, new UserChangedEventArgs(currentUser));
                SystemContext.Instance.OnDataChanged(this, new EventArgs());
            }
        }

        //清除 空分类 树节点
        private void CleanEmptyNode(VirtualNodeList listNodes)
        {
            for (int index = listNodes.Count - 1; index >= 0; index--)
            {
                DocTypeInfo docinfo = listNodes[index].Tag as DocTypeInfo;
                CleanEmptyNode(listNodes[index].Nodes);
                if (docinfo.IsFolder && listNodes[index].Nodes.Count <= 0)
                {
                    listNodes.RemoveAt(index);
                }
            }
        }

        private void toolstpDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_SelectDeptInfo = this.toolstpDept.SelectedItem as DeptInfo;
            this.LoadEvaTypeList();
            if (this.m_EvaluationListForm != null)
                this.m_EvaluationListForm.Dispose();
            //this.
        }

        /// <summary>
        /// 获取指定节点下面所有子节点中,第一个已评估过的文档对应节点
        /// </summary>
        /// <param name="parent">指定的父节点</param>
        /// <returns>第一个已评估过的文档对应节点</returns>
        private VirtualNode GetFirstDocuemntNode(VirtualNode parent)
        {
            if (parent != null)
            {
                EvaTypeInfo evaTypeInfo = parent.Tag as EvaTypeInfo;
                if (evaTypeInfo != null && !evaTypeInfo.IsFolder
                    && this.m_htWardEvaTable != null
                    && this.m_htWardEvaTable.ContainsKey(evaTypeInfo.EvaTypeID))
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
