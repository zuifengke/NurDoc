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
        /// ��鵱ǰ���ݱ༭�����Ƿ��Ѿ��ı�
        /// </summary>
        /// <returns>�Ƿ���δ�ύ</returns>
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
        ///// ��鲢���浱ǰ���ݱ༭���ڵ��޸�
        ///// </summary>
        ///// <returns>�Ƿ�ȡ��</returns>
        //public override bool CheckModifiedData()
        //{
        //    if (!this.HasUncommit())
        //        return false;

        //    DialogResult result = MessageBoxEx.ShowQuestion("��ǰ��Ϣѡ���������޸�,�Ƿ񱣴棿");
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
                MessageBoxEx.ShowError("����������ģ���б�����ʧ��!");
                return;
            }
            m_lstEvaTypeInfos = lstEvaTypeInfos;

            if (lstEvaTypeInfos == null)
                return;

            ////�ȴ����ڵ��ϣ�б�
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

            //���ڵ���������,��ӵ�����
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
        /// �ڵ�ǰ���ڵ�λ�ô�����ģ��ڵ�
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
                MessageBoxEx.ShowError("������Ϣ����ʧ��,�޷����µ����ݿ�!");
                return false;
            }
            this.CreateNewNode(false, EvaTypeInfo);
            this.ShowEvaTemp(EvaTypeInfo);

            string szEvaTypeID = EvaTypeInfo.EvaTypeID;
            List<WardEvaType> lstWardEvaTypes = evaInfoForm.WardEvaTypes;
            shRet = EvaTypeService.Instance.SaveWardEvaTypes(szEvaTypeID, lstWardEvaTypes);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("������Ϣ�����ɹ�,������������Ϣ����ʧ��!");
                return true;
            }

            //List<GridViewColumn> lstGridViewColumns = templetInfoForm.GridViewColumns;
            //shRet = ConfigService.Instance.SaveGridViewColumns(szDocTypeID, lstGridViewColumns);
            //if (shRet != SystemConst.ReturnValue.OK)
            //    MessageBoxEx.ShowError("ģ�崴���ɹ�,��ģ��ժҪ��Ϣ�б���ʧ��!");
            return true;
        }

        /// <summary>
        /// ���ص�ǰѡ�е�ģ��ڵ��Ӧ���ĵ�������Ϣ
        /// </summary>
        /// <param name="bIsDir">�Ƿ���Ŀ¼</param>
        /// <returns>DocTypeInfo</returns>
        private EvaTypeInfo MakeEvaTypeInfo(bool bIsDir)
        {
            EvaTypeInfo docTypeInfo = new EvaTypeInfo();
            docTypeInfo.EvaTypeID = docTypeInfo.MakeEvaTypeID(SysTimeService.Instance.Now);
            docTypeInfo.IsValid = true;
            docTypeInfo.IsFolder = bIsDir;
            docTypeInfo.ParentID = string.Empty;
            docTypeInfo.EvaTypeName = bIsDir ? "�½�Ŀ¼" : "δ����ģ��";
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
        /// ɾ��ָ��TreeNode�ڵ�
        /// </summary>
        /// <param name="deletedNode">�ڵ����</param>
        private void DeleteNode(TreeNode deletedNode)
        {
            if (deletedNode == null || deletedNode.Tag == null)
                return;

            List<string> lstDocTypeID = null;
            this.GetDocTypeList(deletedNode, ref lstDocTypeID);
            if (lstDocTypeID == null || lstDocTypeID.Count <= 0)
                return;

            string szPopupInfo = "ȷ�ϳ���ɾ����" + deletedNode.Text + "����"
                        + "\r\n���棺\r\nɾ���������ͽ����»��ڸ�������д�Ĳ����޷��ٱ༭��"
                        + "\r\n���ϵͳ�Ѿ�����,���ǽ�������Ҫ����ɾ������������Ϊ���ɼ���";
            DialogResult result = MessageBoxEx.ShowConfirm(szPopupInfo);
            if (result != DialogResult.OK)
                return;

            short shRet = EvaTypeService.Instance.DeleteEvaTypeInfos(lstDocTypeID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("ɾ����{0}��ʧ��!", deletedNode.Text));
                return;
            }
            deletedNode.Remove();
        }

        /// <summary>
        /// ��ȡָ���ڵ��������ӽڵ���ĵ�����ID�б�
        /// </summary>
        /// <param name="parentNode">ģ�常�ڵ�</param>
        /// <param name="lstDocTypeID">�ĵ�����ID�б�</param>
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
        /// �ڵ�ǰ���ڵ�λ�ô�����Ŀ¼�ڵ�
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
                MessageBoxEx.ShowError("Ŀ¼����ʧ��,�޷����µ����ݿ�!");
                return false;
            }
            return this.CreateNewNode(true, evaTypeInfo) != null;
        }



        /// <summary>
        /// ��ʾѡ�е�ģ�����Ϣ,�������޸�
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
                MessageBoxEx.ShowError("����������Ϣ����ʧ��,�޷����µ����ݿ�!");
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
                MessageBoxEx.ShowError("����������Ϣ�����ɹ�,������������Ϣ����ʧ��!");
                return;
            }
        }


        private TreeNode CreateNewNode(bool bIsDir, EvaTypeInfo docTypeInfo)
        {
            TreeNode node = new TreeNode();
            node.Tag = docTypeInfo;
            if (docTypeInfo == null)
                node.Text = bIsDir ? "�½�Ŀ¼" : "δ����ģ��";
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
                string szPopupInfo = "��" + m_evaTypeInfo.EvaTypeID + "�����ڱ༭��δ����!"
                                    + "\r\n���棺\r\n�Ƿ񲻱���ֱ�ӹرգ�";
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
        /// ����ָ������ʾ������,�Լ��󶨵�����
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="evaTypeItem">�󶨵�����</param>
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
        /// ���浱ǰ���ڵ������޸�
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
        /// ����ָ���е����ݵ�Զ�����ݱ�,��Ҫע����ǣ��е�ɾ��״̬��������״̬����
        /// </summary>
        /// <param name="row">ָ����</param>
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
            //        MessageBoxEx.Show("�޷�ɾ����ǰ��¼!");
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
            //        MessageBoxEx.Show("�޷����µ�ǰ��¼!");
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
            //        MessageBoxEx.Show("�޷����浱ǰ��¼!");
            //        return SystemConst.ReturnValue.FAILED;
            //    }
            //    row.Tag = evaTypeItem;
            //    this.dataTableView1.SetRowState(row, RowState.Normal);
            //}
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ��ȡָ���������޸ĺ������
        /// </summary>
        /// <param name="row">ָ����</param>
        /// <param name="evaTypeItem">�����޸ĺ������</param>
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

            //����
            object cellValue = row.Cells[this.colItemNo.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("��������������!");
                return false;
            }
            evaTypeItem.ItemNo = cellValue.ToString();

            //����
            cellValue = row.Cells[this.colItemContent.Index].Value;
            if (GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                this.dataTableView1.SelectRow(row);
                MessageBoxEx.Show("��������������!");
                return false;
            }
            evaTypeItem.ItemText = cellValue.ToString();

            //����
            cellValue = row.Cells[this.colItemSort.Index].Value;
            if (cellValue != null)
            {
                int value = 0;
                if (int.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemSort = value;
            }
            else
                evaTypeItem.ItemSort = 0;

            //�Ƿ������
            cellValue = row.Cells[this.colItemInCount.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemInCount = value;
            }

            //�Ƿ����
            cellValue = row.Cells[this.colItemTextBlod.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemTextBlod = value;
            }

            //ֻ��
            cellValue = row.Cells[this.colItemReadOnly.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemReadOnly = value;
            }

            //�Ƿ�����
            cellValue = row.Cells[this.colItemEnable.Index].Value;
            if (!GlobalMethods.Misc.IsEmptyString(cellValue))
            {
                bool value = false;
                if (bool.TryParse(cellValue.ToString(), out value))
                    evaTypeItem.ItemEnable = value;
            }

            //����
            cellValue = row.Cells[this.colItemType.Index].Value;
            if (cellValue != null)
            {
                evaTypeItem.ItemType = cellValue.ToString();
            }
            else
            {
                evaTypeItem.ItemType = "�����";
            }

            //Ĭ��ֵ
            cellValue = row.Cells[this.colItemDefaultValue.Index].Value;
            evaTypeItem.ItemDefaultValue = cellValue == null ? "" : cellValue.ToString();

            //��ֵ
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
        /// �޸�ָ���Ľڵ���ʾ�ı�
        /// </summary>
        /// <param name="node">�ڵ����</param>
        /// <param name="szNewText">�޸��ı�</param>
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
            MessageBoxEx.Show(string.Format("��{0}��������ʧ��!", docTypeInfo.EvaTypeName));
            return false;
        }

        /// <summary>
        /// ������TreeView����,�ƶ�ָ���Ľڵ㵽Ŀ��ڵ�֮ǰ
        /// </summary>
        /// <param name="moveNode">���ƶ��Ľڵ�</param>
        /// <param name="targetNode">Ŀ��ڵ�</param>
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

            //�Ѹ��ڵ��Ϸŵ������ӽڵ�,��ȡ��
            TreeNode parentNode = targetNode;
            while (parentNode != null)
            {
                if (parentNode == moveNode)
                    return;
                parentNode = parentNode.Parent;
            }
            if (targetNode == moveNode.Parent)
                return;

            //Ѱ�Ҹ�Ŀ¼�ڵ��Լ�Ŀ��λ��
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

            //��ʼ�ƶ��ڵ�
            bool bIsExpanded = moveNode.IsExpanded;
            bool bIsSelected = moveNode.IsSelected;
            TreeNode newNode = (TreeNode)moveNode.Clone();
            nodes.Insert(nTargetIndex, newNode);
            moveNode.Remove();
            if (bIsExpanded)
                newNode.ExpandAll();
            if (bIsSelected)
                this.treeViewControl1.SelectedNode = newNode;

            //��������ֵܽڵ��˳��ֵ
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
                    //ʧ�ܺ�ԭ�����µĶ�������
                    evaTypeInfo.EvaTypeNo = nEvaTypeOldNo;
                    evaTypeInfo.ParentID = szOldParentID;
                    MessageBoxEx.Show(string.Format("��{0}���ƶ�ʧ��!", evaTypeInfo.EvaTypeName));
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
            evaTypeItem.ItemType = "ѡ���";
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