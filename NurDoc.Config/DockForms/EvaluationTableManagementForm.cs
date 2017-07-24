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
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Data;


namespace Heren.NurDoc.Config.DockForms
{
    public partial class EvaluationTableManagementForm : DockContentBase
    {
        private bool m_isModified = false;
        private EvaTypeInfo m_evaTypeInfo = null;

        public EvaluationTableManagementForm(MainForm mainForm)
            : base(mainForm)
        {
            InitializeComponent();
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
        }

        private List<EvaTypeInfo> m_lstEvaTypeInfos = null;

        /// <summary>
        /// 检查当前数据编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否有未提交</returns>
        public override bool HasUncommit()
        {
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                if (row.IsModifiedRow || row.IsNewRow || row.IsDeletedRow)
                    return true;
            }
            return false;
        }

        ///// <summary>
        ///// 检查并保存当前数据编辑窗口的修改
        ///// </summary>
        ///// <returns>是否取消</returns>
        //public override bool CheckModifiedData()
        //{
        //    if (!this.HasUncommit())
        //        return false;

        //    DialogResult result = MessageBoxEx.ShowQuestion("当前信息选项数据已修改,是否保存？");
        //    if (result == DialogResult.Yes)
        //        return this.CommitModify();
        //    return true;
        //}

        public override void OnRefreshView()
        {
            this.treeViewControl1.Nodes.Clear();
            this.Update();

            List<EvaTypeInfo> lstEvaTypeInfos = null;
            short shRet = EvaTypeService.Instance.GetEvaTypeInfos(ref lstEvaTypeInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理病历评估模板列表下载失败!");
                return;
            }
            m_lstEvaTypeInfos = lstEvaTypeInfos;

            if (lstEvaTypeInfos == null)
                return;

            ////先创建节点哈希列表
            Dictionary<string, TreeNode> nodeTable = new Dictionary<string, TreeNode>();
            foreach (EvaTypeInfo evaTypeInfo in lstEvaTypeInfos)
            {
                TreeNode evaTypeNode = new TreeNode();
                evaTypeNode.Tag = evaTypeInfo;
                evaTypeNode.Text = evaTypeInfo.EvaTypeName;

                if (!evaTypeInfo.IsValid || !evaTypeInfo.IsVisible)
                    evaTypeNode.ForeColor = Color.Silver;

                if (!evaTypeInfo.IsFolder)
                    evaTypeNode.ImageIndex = 2;
                else
                    evaTypeNode.ImageIndex = 0;
                evaTypeNode.SelectedImageIndex = evaTypeNode.ImageIndex;

                if (!nodeTable.ContainsKey(evaTypeInfo.EvaTypeID))
                    nodeTable.Add(evaTypeInfo.EvaTypeID, evaTypeNode);
            }

            //将节点连接起来,添加到树中
            foreach (KeyValuePair<string, TreeNode> pair in nodeTable)
            {
                if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    continue;

                EvaTypeInfo evaTypeInfo = pair.Value.Tag as EvaTypeInfo;
                if (evaTypeInfo == null)
                    continue;

                if (string.IsNullOrEmpty(evaTypeInfo.ParentID))
                {
                    this.treeViewControl1.Nodes.Add(pair.Value);
                }
                else
                {
                    TreeNodeCollection nodes = null;
                    if (nodeTable.ContainsKey(evaTypeInfo.ParentID))
                    {
                        TreeNode parentNode = nodeTable[evaTypeInfo.ParentID];
                        if (parentNode != null)
                            nodes = parentNode.Nodes;
                    }
                    if (nodes == null)
                        nodes = this.treeViewControl1.Nodes;
                    nodes.Add(pair.Value);
                }
            }
            this.treeViewControl1.ExpandAll();
            if (this.treeViewControl1.Nodes.Count > 0) this.treeViewControl1.Nodes[0].EnsureVisible();

            this.dataTableView1.Rows.Clear();
            this.m_evaTypeInfo = null;
        }

        /// <summary>
        /// 在当前树节点位置处创建模板节点
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateEvaType()
        {
            EvaInfoForm evaInfoForm = new EvaInfoForm();
            evaInfoForm.IsNew = true;
            evaInfoForm.IsFolder = false;
            evaInfoForm.EvaTypeInfo = this.MakeEvaTypeInfo(false);
            if (evaInfoForm.ShowDialog() != DialogResult.OK)
                return false;

            EvaTypeInfo EvaTypeInfo = evaInfoForm.EvaTypeInfo;
            if (EvaTypeInfo == null)
                return false;
            short shRet = EvaTypeService.Instance.SaveEvaTypeInfo(EvaTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("评价信息创建失败,无法更新到数据库!");
                return false;
            }
            this.CreateNewNode(false, EvaTypeInfo);
            this.ShowEvaTemp(EvaTypeInfo);

            string szEvaTypeID = EvaTypeInfo.EvaTypeID;
            List<WardEvaType> lstWardEvaTypes = evaInfoForm.WardEvaTypes;
            shRet = EvaTypeService.Instance.SaveWardEvaTypes(szEvaTypeID, lstWardEvaTypes);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("评价信息创建成功,但所属科室信息保存失败!");
                return true;
            }

            //List<GridViewColumn> lstGridViewColumns = templetInfoForm.GridViewColumns;
            //shRet = ConfigService.Instance.SaveGridViewColumns(szDocTypeID, lstGridViewColumns);
            //if (shRet != SystemConst.ReturnValue.OK)
            //    MessageBoxEx.ShowError("模板创建成功,但模板摘要信息列保存失败!");
            return true;
        }

        /// <summary>
        /// 返回当前选中的模板节点对应的文档类型信息
        /// </summary>
        /// <param name="bIsDir">是否是目录</param>
        /// <returns>DocTypeInfo</returns>
        private EvaTypeInfo MakeEvaTypeInfo(bool bIsDir)
        {
            EvaTypeInfo docTypeInfo = new EvaTypeInfo();
            docTypeInfo.EvaTypeID = docTypeInfo.MakeEvaTypeID(SysTimeService.Instance.Now);
            docTypeInfo.IsValid = true;
            docTypeInfo.IsFolder = bIsDir;
            docTypeInfo.ParentID = string.Empty;
            docTypeInfo.EvaTypeName = bIsDir ? "新建目录" : "未命名模板";
            docTypeInfo.CreateTime = SysTimeService.Instance.Now;
            docTypeInfo.EvaTypeNo = this.treeViewControl1.Nodes.Count;

            //string szApplyEnv = this.toolcboApplyEnv.Text;
            //szApplyEnv = ServerData.DocTypeApplyEnv.GetApplyEnvCode(szApplyEnv);
            //docTypeInfo.ApplyEnv = szApplyEnv;

            TreeNode selectedNode = this.treeViewControl1.SelectedNode;
            if (selectedNode == null)
                return docTypeInfo;

            EvaTypeInfo selectedDocType = selectedNode.Tag as EvaTypeInfo;
            if (selectedDocType == null)
                return docTypeInfo;

            if (selectedDocType.IsFolder)
            {
                docTypeInfo.ParentID = selectedDocType.EvaTypeID;
                docTypeInfo.EvaTypeNo = selectedNode.Nodes.Count;
            }
            else
            {
                docTypeInfo.ParentID = selectedDocType.ParentID;
                if (selectedNode.Parent != null)
                    docTypeInfo.EvaTypeNo = selectedNode.Parent.Nodes.Count;
            }
            return docTypeInfo;
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
                        + "\r\n警告：\r\n删除病历类型将导致基于该类型书写的病历无法再编辑。"
                        + "\r\n如果系统已经上线,我们建议您不要彻底删除而将其设置为不可见。";
            DialogResult result = MessageBoxEx.ShowConfirm(szPopupInfo);
            if (result != DialogResult.OK)
                return;

            short shRet = EvaTypeService.Instance.DeleteEvaTypeInfos(lstDocTypeID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("删除“{0}”失败!", deletedNode.Text));
                return;
            }
            deletedNode.Remove();
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

            EvaTypeInfo docTypeInfo = parentNode.Tag as EvaTypeInfo;
            if (docTypeInfo == null)
                return;
            lstDocTypeID.Add(docTypeInfo.EvaTypeID);
            for (int index = 0; index < parentNode.Nodes.Count; index++)
                this.GetDocTypeList(parentNode.Nodes[index], ref lstDocTypeID);
        }

        /// <summary>
        /// 在当前树节点位置处创建目录节点
        /// </summary>
        /// <returns>bool</returns>
        private bool CreateFolder()
        {
            EvaInfoForm evaInfoForm = new EvaInfoForm();
            evaInfoForm.IsNew = true;
            evaInfoForm.IsFolder = true;
            evaInfoForm.EvaTypeInfo = this.MakeEvaTypeInfo(true);
            if (evaInfoForm.ShowDialog() != DialogResult.OK)
                return false;

            EvaTypeInfo evaTypeInfo = evaInfoForm.EvaTypeInfo;
            if (evaTypeInfo == null)
                return false;
            short shRet = EvaTypeService.Instance.SaveEvaTypeInfo(evaTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("目录创建失败,无法更新到数据库!");
                return false;
            }
            return this.CreateNewNode(true, evaTypeInfo) != null;
        }



        /// <summary>
        /// 显示选中的模板的信息,并接受修改
        /// </summary>
        private void ShowEvaTypeInfoEditForm()
        {
            TreeNode selectedNode = this.treeViewControl1.SelectedNode;
            if (selectedNode == null)
                return;
            EvaTypeInfo evaTypeInfo = selectedNode.Tag as EvaTypeInfo;
            if (evaTypeInfo == null)
                return;
            string szEvaTypeID = evaTypeInfo.EvaTypeID;

            EvaInfoForm evaInfoForm = new EvaInfoForm();
            evaInfoForm.IsNew = false;
            evaInfoForm.IsFolder = evaTypeInfo.IsFolder;
            evaInfoForm.EvaTypeInfo = evaTypeInfo.Clone() as EvaTypeInfo;
            DialogResult result = evaInfoForm.ShowDialog();
            if (result != DialogResult.OK)
                return;

            evaTypeInfo = evaInfoForm.EvaTypeInfo;
            if (evaTypeInfo == null)
                return;
            short shRet = EvaTypeService.Instance.ModifyEvaTypeInfo(szEvaTypeID, evaTypeInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理评价信息更新失败,无法更新到数据库!");
                return;
            }
            selectedNode.Tag = evaTypeInfo;
            selectedNode.Text = evaTypeInfo.EvaTypeName;
            if (!evaTypeInfo.IsValid || !evaTypeInfo.IsVisible)
                selectedNode.ForeColor = Color.Silver;
            else
                selectedNode.ForeColor = Color.Black;

            List<WardEvaType> lstWardEvaTypes = evaInfoForm.WardEvaTypes;
            shRet = EvaTypeService.Instance.SaveWardEvaTypes(szEvaTypeID, lstWardEvaTypes);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理评价信息创建成功,但所属科室信息保存失败!");
                return;
            }
        }


        private TreeNode CreateNewNode(bool bIsDir, EvaTypeInfo docTypeInfo)
        {
            TreeNode node = new TreeNode();
            node.Tag = docTypeInfo;
            if (docTypeInfo == null)
                node.Text = bIsDir ? "新建目录" : "未命名模板";
            else
                node.Text = docTypeInfo.EvaTypeName;
            node.ImageIndex = bIsDir ? 0 : 2;
            node.SelectedImageIndex = node.ImageIndex;

            if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                node.ForeColor = Color.Silver;

            TreeNode selectedNode = this.treeViewControl1.SelectedNode;
            if (selectedNode == null)
            {
                this.treeViewControl1.Nodes.Add(node);
                return node;
            }

            EvaTypeInfo selectedEvaType = selectedNode.Tag as EvaTypeInfo;
            if (selectedEvaType == null)
            {
                this.treeViewControl1.Nodes.Add(node);
                return node;
            }

            TreeNode parentNode = selectedNode;
            if (!selectedEvaType.IsFolder)
                parentNode = selectedNode.Parent;
            if (parentNode != null)
            {
                parentNode.Nodes.Add(node);
                parentNode.Expand();
            }
            else
            {
                this.treeViewControl1.Nodes.Add(node);
            }

            if (node.TreeView != null)
            {
                this.treeViewControl1.SelectedNode = node;
                node.BeginEdit();
                return node;
            }
            return null;
        }

        private void ShowEvaTemp(EvaTypeInfo evaTypeInfo)
        {
            if (this.m_evaTypeInfo == evaTypeInfo)
                return;
            if (this.m_isModified && this.m_evaTypeInfo != null)
            {
                string szPopupInfo = "“" + m_evaTypeInfo.EvaTypeID + "”正在编辑还未保存!"
                                    + "\r\n警告：\r\n是否不保存直接关闭？";
                DialogResult result = MessageBoxEx.ShowConfirm(szPopupInfo);
                if (result != DialogResult.OK)
                    return;
            }

            this.m_evaTypeInfo = evaTypeInfo;
            this.LoadEvaItems(evaTypeInfo);
        }

        private void LoadEvaItems(EvaTypeInfo evaTypeInfo)
        {
            this.dataTableView1.Rows.Clear();
            List<EvaTypeItem> lstEvaTypeItem = new List<EvaTypeItem>();
            short shRet = EvaTypeService.Instance.GetEvaTypeItems(evaTypeInfo, ref lstEvaTypeItem);
            if (shRet == SystemConst.ReturnValue.NO_FOUND)
            {
                return;
            }
            else if (shRet != SystemConst.ReturnValue.OK)
            {
                return;
            }

            for (int index = 0; index < lstEvaTypeItem.Count; index++)
            {
                EvaTypeItem evaTypeItem = lstEvaTypeItem[index];
                if (evaTypeItem == null)
                    continue;
                
                int nRowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                this.SetRowData(row, evaTypeItem);
                this.dataTableView1.SetRowState(row, RowState.Normal);

                if (nRowIndex <= 0)
                    continue;

                DataTableViewRow prevRow = this.dataTableView1.Rows[nRowIndex - 1];
                if (prevRow.DefaultCellStyle.BackColor == Color.Gainsboro)
                    row.DefaultCellStyle.BackColor = Color.White;
                else
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;

            }
        }

        /// <summary>
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="evaTypeItem">绑定的数据</param>
        /// <returns>bool</returns>
        private bool SetRowData(DataTableViewRow row, EvaTypeItem evaTypeItem)
        {
            if (row == null || row.Index < 0 || evaTypeItem == null)
                return false;
            row.Tag = evaTypeItem;
            row.Cells[this.colItemNo.Index].Value = evaTypeItem.ItemNo;
            row.Cells[this.colItemSort.Index].Value = evaTypeItem.ItemSort;
            row.Cells[this.colItemType.Index].Value = evaTypeItem.ItemType;
            row.Cells[this.colItemDefaultValue.Index].Value = evaTypeItem.ItemDefaultValue;
            row.Cells[this.colItemInCount.Index].Value = evaTypeItem.ItemInCount;
            row.Cells[this.colItemScore.Index].Value = evaTypeItem.ItemScore;
            row.Cells[this.colItemContent.Index].Value = evaTypeItem.ItemText;
            row.Cells[this.colItemTextBlod.Index].Value = evaTypeItem.ItemTextBlod;
            row.Cells[this.colItemReadOnly.Index].Value = evaTypeItem.ItemReadOnly;
            row.Cells[this.colItemEnable.Index].Value = evaTypeItem.ItemEnable;

            return true;
        }

        /// <summary>
        /// 保存当前窗口的数据修改
        /// </summary>
        /// <returns>bool</returns>
        public override bool CommitModify()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            int index = 0;
            short shRet = SystemConst.ReturnValue.OK;
            List<EvaTypeItem> lstEvaTypeItem = new List<EvaTypeItem>();
            while (index < this.dataTableView1.Rows.Count)
            {
                DataTableViewRow row = this.dataTableView1.Rows[index];
                bool bIsDeletedRow = this.dataTableView1.IsDeletedRow(row);
                EvaTypeItem evaTypeItem = new EvaTypeItem();
                MakeRowData(row, ref evaTypeItem);
                if (!bIsDeletedRow)
                    lstEvaTypeItem.Add(evaTypeItem);
                index++;
            }

            shRet = EvaTypeService.Instance.UpdateEvaItems(this.m_evaTypeInfo.EvaTypeID, lstEvaTypeItem);

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 保存指定行的数据到远程数据表,需要注意的是：行的删除状态会与其他状态共存
        /// </summary>
        /// <param name="row">指定行</param>
        /// <returns>SystemConst.ReturnValue</returns>
        private short SaveRowData(DataTableViewRow row)
        {
            if (row == null || row.Index < 0)
                return SystemConst.ReturnValue.FAILED;
            if (this.dataTableView1.IsNormalRow(row) || this.dataTableView1.IsUnknownRow(row))
            {
                if (!this.dataTableView1.IsDeletedRow(row))
                    return SystemConst.ReturnValue.CANCEL;
            }

            EvaTypeItem evaTypeItem = row.Tag as EvaTypeItem;
            if (evaTypeItem == null)
                return SystemConst.ReturnValue.FAILED;
            string szEvaID = evaTypeItem.EvaTypeID;
            string szItemNo = evaTypeItem.ItemNo;

            evaTypeItem = null;
            if (!this.MakeRowData(row, ref evaTypeItem))
                return SystemConst.ReturnValue.FAILED;



            //if (this.dataTableView1.IsDeletedRow(row))
            //{
            //    if (!this.dataTableView1.IsNewRow(row))
            //        shRet = ConfigService.Instance.DeleteConfigData(szGroupName, szConfigName);
            //    if (shRet != SystemConst.ReturnValue.OK)
            //    {
            //        this.dataTableView1.SelectRow(row);
            //        MessageBoxEx.Show("无法删除当前记录!");
            //        return SystemConst.ReturnValue.FAILED;
            //    }
            //    this.dataTableView1.Rows.Remove(row);
            //}
            //else if (this.dataTableView1.IsModifiedRow(row))
            //{
            //    shRet = ConfigService.Instance.UpdateConfigData(szGroupName, szConfigName, evaTypeItem);
            //    if (shRet != SystemConst.ReturnValue.OK)
            //    {
            //        this.dataTableView1.SelectRow(row);
            //        MessageBoxEx.Show("无法更新当前记录!");
            //        return SystemConst.ReturnValue.FAILED;
            //    }
            //    row.Tag = evaTypeItem;
            //    this.dataTableView1.SetRowState(row, RowState.Normal);
            //}
            //else if (this.dataTableView1.IsNewRow(row))
            //{
            //    shRet = ConfigService.Instance.SaveConfigData(evaTypeItem);
            //    if (shRet != SystemConst.ReturnValue.OK)
            //    {
            //        this.dataTableView1.SelectRow(row);
            //        MessageBoxEx.Show("无法保存当前记录!");
            //        return SystemConst.ReturnValue.FAILED;
            //    }
            //    row.Tag = evaTypeItem;
            //    this.dataTableView1.SetRowState(row, RowState.Normal);
            //}
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定行最新修改后的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="evaTypeItem">最新修改后的数据</param>
        /// <returns>bool</returns>
        private bool MakeRowData(DataTableViewRow row, ref EvaTypeItem evaTypeItem)
        {
            if (row == null || row.Index < 0)
                return false;
            EvaTypeItem old = row.Tag as EvaTypeItem;
            if (old == null)
                return false;

            if (this.dataTableView1.IsDeletedRow(row))
            {
                evaTypeItem = old;
                return true;
            }
            evaTypeItem = old.Clone() as EvaTypeItem;

            //主键
            object cellValue = row.Cells[this.colItemNo.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入主键!");
                return false;
            }
            evaTypeItem.ItemNo = cellValue.ToString();

            //内容
            cellValue = row.Cells[this.colItemContent.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("您必须输入内容!");
                return false;
            }
            evaTypeItem.ItemText = cellValue.ToString();

            //排序
            cellValue = row.Cells[this.colItemSort.Index].Value;
            if (cellValue != null)
            {
                int value = 0;
                if (int.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemSort = value;
            }
            else
                evaTypeItem.ItemSort = 0;

            //是否计算项
            cellValue = row.Cells[this.colItemInCount.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemInCount = value;
            }

            //是否粗体
            cellValue = row.Cells[this.colItemTextBlod.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemTextBlod = value;
            }

            //只读
            cellValue = row.Cells[this.colItemReadOnly.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemReadOnly = value;
            }

            //是否作废
            cellValue = row.Cells[this.colItemEnable.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemEnable = value;
            }

            //类型
            cellValue = row.Cells[this.colItemType.Index].Value;
            if (cellValue != null)
            {
                evaTypeItem.ItemType = cellValue.ToString();
            }
            else
            {
                evaTypeItem.ItemType = "输入框";
            }

            //默认值
            cellValue = row.Cells[this.colItemDefaultValue.Index].Value;
            evaTypeItem.ItemDefaultValue = cellValue == null ? "" : cellValue.ToString();

            //分值
            cellValue = row.Cells[this.colItemScore.Index].Value;
            if (cellValue != null)
            {
                int value = 0;
                if (int.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemScore = value;
            }
            else
                evaTypeItem.ItemScore = 0;

            return true;
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
            EvaTypeInfo docTypeInfo = node.Tag as EvaTypeInfo;
            if (docTypeInfo == null || docTypeInfo.EvaTypeName == szNewText)
                return false;

            string szDocTypeID = docTypeInfo.EvaTypeID;
            string szOldDocTypeName = docTypeInfo.EvaTypeName;
            docTypeInfo.EvaTypeName = szNewText;

            short shRet = EvaTypeService.Instance.ModifyEvaTypeInfo(szDocTypeID, docTypeInfo);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                node.Text = szNewText;
                return true;
            }
            docTypeInfo.EvaTypeName = szOldDocTypeName;
            MessageBoxEx.Show(string.Format("“{0}”重命名失败!", docTypeInfo.EvaTypeName));
            return false;
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

            EvaTypeInfo moveEvaTypeInfo = moveNode.Tag as EvaTypeInfo;
            if (moveEvaTypeInfo == null)
                return;

            EvaTypeInfo targetEvaTypeInfo = null;
            if (targetNode != null)
                targetEvaTypeInfo = targetNode.Tag as EvaTypeInfo;

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
            int nTargetIndex = this.treeViewControl1.Nodes.Count;
            if (targetEvaTypeInfo != null)
            {
                nTargetIndex = targetNode.Index;
                if (!targetEvaTypeInfo.IsFolder)
                {
                    targetNode = targetNode.Parent;
                    szParentID = targetEvaTypeInfo.ParentID;
                }
                else
                {
                    nTargetIndex = targetNode.Nodes.Count;
                    szParentID = targetEvaTypeInfo.EvaTypeID;
                }
            }

            TreeNodeCollection nodes = null;
            if (targetNode != null)
                nodes = targetNode.Nodes;
            else
                nodes = this.treeViewControl1.Nodes;

            //开始移动节点
            bool bIsExpanded = moveNode.IsExpanded;
            bool bIsSelected = moveNode.IsSelected;
            TreeNode newNode = (TreeNode)moveNode.Clone();
            nodes.Insert(nTargetIndex, newNode);
            moveNode.Remove();
            if (bIsExpanded)
                newNode.ExpandAll();
            if (bIsSelected)
                this.treeViewControl1.SelectedNode = newNode;

            //更新相关兄弟节点的顺序值
            for (int childIndex = 0; childIndex < nodes.Count; childIndex++)
            {
                TreeNode childNode = nodes[childIndex];
                if (childNode == null)
                    continue;
                EvaTypeInfo evaTypeInfo = childNode.Tag as EvaTypeInfo;
                if (evaTypeInfo == null)
                    continue;
                string szOldParentID = evaTypeInfo.ParentID;
                int nEvaTypeOldNo = evaTypeInfo.EvaTypeNo;

                evaTypeInfo.EvaTypeNo = childIndex;
                evaTypeInfo.ParentID = szParentID;
                short shRet = EvaTypeService.Instance.ModifyEvaTypeInfo(evaTypeInfo.EvaTypeID, evaTypeInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    //失败后还原被更新的对象属性
                    evaTypeInfo.EvaTypeNo = nEvaTypeOldNo;
                    evaTypeInfo.ParentID = szOldParentID;
                    MessageBoxEx.Show(string.Format("“{0}”移动失败!", evaTypeInfo.EvaTypeName));
                    return;
                }
            }
        }

        private void NewRow()
        {
            if (this.m_evaTypeInfo == null)
                return;
            EvaTypeItem evaTypeItem = null;
            int nRowCount = this.dataTableView1.Rows.Count;
            evaTypeItem = new EvaTypeItem();
            evaTypeItem.EvaTypeID = this.m_evaTypeInfo.EvaTypeID;
            evaTypeItem.ItemDefaultValue = "";
            evaTypeItem.ItemNo = EvaTypeItem.MakeEvaItemNo();
            evaTypeItem.ItemTextBlod = false;
            evaTypeItem.ItemType = "选择框";
            evaTypeItem.ItemScore = 1;
            evaTypeItem.ItemInCount = true;
            evaTypeItem.ItemReadOnly = false;
            evaTypeItem.ItemEnable = false;
            evaTypeItem.ItemSort = this.dataTableView1.Rows.Count;

            this.dataTableView1.Rows.Add();
            DataTableViewRow row = this.dataTableView1.Rows[nRowCount];
            this.SetRowData(row, evaTypeItem);

            this.dataTableView1.Focus();
            this.dataTableView1.SelectRow(row);
            this.dataTableView1.SetRowState(row, RowState.New);
        }

        private void DeleteRows()
        {
            int nCount = this.dataTableView1.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dataTableView1.SelectedRows[index];
                this.dataTableView1.SetRowState(row, RowState.Delete);
            }
        }

        private void CancelModifyRows()
        {
            int nCount = this.dataTableView1.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dataTableView1.SelectedRows[index];
                if (!this.dataTableView1.IsModifiedRow(row))
                    continue;
                this.SetRowData(row, row.Tag as EvaTypeItem);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        private void CancelDeleteRows()
        {
            int nCount = this.dataTableView1.SelectedRows.Count;
            if (nCount <= 0)
                return;
            for (int index = 0; index < nCount; index++)
            {
                DataTableViewRow row = this.dataTableView1.SelectedRows[index];
                if (!this.dataTableView1.IsDeletedRow(row))
                    continue;
                if (this.dataTableView1.IsNewRow(row))
                    this.dataTableView1.SetRowState(row, RowState.New);
                else if (this.dataTableView1.IsModifiedRow(row))
                    this.dataTableView1.SetRowState(row, RowState.Update);
                else
                    this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        private void treeViewControl1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.WaitCursor);
            EvaTypeInfo evaTypeInfo = e.Node.Tag as EvaTypeInfo;
            if (evaTypeInfo != null && !evaTypeInfo.IsFolder)
            {
                ShowEvaTemp(evaTypeInfo);
            }
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.Default);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            this.NewRow();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.CommitModify())
            {
                this.LoadEvaItems(this.m_evaTypeInfo);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void treeViewControl1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null || GlobalMethods.Misc.IsEmptyString(e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.WaitCursor);
            e.CancelEdit = !this.ModifyNodeText(e.Node, e.Label.Trim());
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.Default);
        }

        private void munDeleteEvaInfo_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.WaitCursor);
            this.DeleteNode(this.treeViewControl1.SelectedNode);
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.Default);
        }

        private void munCreateEvaFolder_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.WaitCursor);
            this.CreateFolder();
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.Default);
        }

        private void mnuCreatEvaInfo_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.WaitCursor);
            this.CreateEvaType();
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.Default);
        }

        private void mnuShowEvaTypeInfo_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.WaitCursor);
            this.ShowEvaTypeInfoEditForm();
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.Default);
        }

        private void munRefresh_Click(object sender, EventArgs e)
        {
            this.OnRefreshView();
        }

        private void treeViewControl1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode moveNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (moveNode == null)
                return;

            Point ptMousePos = new Point(e.X, e.Y);
            ptMousePos = this.treeViewControl1.PointToClient(ptMousePos);
            TreeNode targetNode = this.treeViewControl1.GetNodeAt(ptMousePos);

            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.WaitCursor);
            this.DragNode(moveNode, targetNode);
            GlobalMethods.UI.SetCursor(this.treeViewControl1, Cursors.Default);
        }

        private void treeViewControl1_DragEnter(object sender, DragEventArgs e)
        {

            if (!e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }

        private void treeViewControl1_DragOver(object sender, DragEventArgs e)
        {
            Point ptMousePos = new Point(e.X, e.Y);
            ptMousePos = this.treeViewControl1.PointToClient(ptMousePos);
            TreeNode node = this.treeViewControl1.GetNodeAt(ptMousePos);
            if (node == null)
                return;

            int nHeight = this.treeViewControl1.Height;
            int nItemHeight = this.treeViewControl1.ItemHeight;
            if (ptMousePos.Y < nItemHeight && node.PrevVisibleNode != null)
                node.PrevVisibleNode.EnsureVisible();
            else if (ptMousePos.Y > nHeight - nItemHeight && node.NextVisibleNode != null)
                node.NextVisibleNode.EnsureVisible();
        }

        private void treeViewControl1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node == null)
                return;
            this.treeViewControl1.SelectedNode = node;
            if (e.Button == MouseButtons.Left)
            {
                this.treeViewControl1.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void treeViewControl1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void treeViewControl1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void munNewItem_Click(object sender, EventArgs e)
        {
            this.NewRow();
        }

        //private void munDeleteItems_Click(object sender, EventArgs e)
        //{
        //    this.DeleteRows();
        //}

        private void munCancelModify_Click(object sender, EventArgs e)
        {
            this.CancelModifyRows();
        }

        //private void menuCancelDelete_Click(object sender, EventArgs e)
        //{
        //    this.CancelDeleteRows();
        //}
    }
}