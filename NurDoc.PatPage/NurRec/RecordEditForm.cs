// ***********************************************************
// 护理电子病历系统,护理记录中各种可生成文本的护理评估编辑窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.VirtualTreeView;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.PatPage.Document;
using Heren.Common.Report;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.PatPage.NurRec
{
    internal partial class RecordEditForm : HerenForm
    {
        private NursingRecInfo m_nursingRecInfo = null;

        /// <summary>
        /// 获取当前护理记录编辑器绑定的护理记录信息
        /// </summary>
        [Browsable(false)]
        public NursingRecInfo NursingRecInfo
        {
            get { return this.m_nursingRecInfo; }
        }

        private bool m_bIsFirstRecord = false;

        /// <summary>
        /// 获取或设置当前是否第一次新建护理记录
        /// </summary>
        [Browsable(false)]
        public bool IsFirstRecord
        {
            get { return this.m_bIsFirstRecord; }
            set { this.m_bIsFirstRecord = value; }
        }

        private GridViewSchema m_currentSchema = null;

        /// <summary>
        /// 获取或设置当前用户选择的护理记录单类型
        /// </summary>
        [Browsable(false)]
        public GridViewSchema CurrentSchema
        {
            get { return this.m_currentSchema; }
            set { this.m_currentSchema = value; }
        }

        private NursingRecHistory m_nursingRecHistory = null;

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get
            {
                DocumentControl[] formEditorList = GetModifiedDocuments();
                return formEditorList != null && formEditorList.Length > 0;
            }
        }

        private Dictionary<string, Heren.Common.Forms.Editor.KeyData> m_dicSummaryDatas = null;

        /// <summary>
        /// 相关联的护理记录单关键数据
        /// </summary>
        public Dictionary<string, Heren.Common.Forms.Editor.KeyData> DicSummaryDatas
        {
            set { this.m_dicSummaryDatas = value; }
        }

        /// <summary>
        /// 当当前护理记录被更新时触发
        /// </summary>
        [Description("当当前护理记录被更新时触发")]
        public event EventHandler RecordUpdated;

        protected virtual void OnRecordUpdated(EventArgs e)
        {
            if (this.RecordUpdated != null)
                this.RecordUpdated(this, e);
        }

        public void CleanNodeTable()
        {
            this.m_htNodeTable.Clear();
        }

        /// <summary>
        /// 该哈希表用于缓存文档类型ID与树节点的关系
        /// </summary>
        private Dictionary<string, VirtualNode> m_htNodeTable = null;

        /// <summary>
        /// 该哈希表用于缓存文档类型ID与已写文档的关系
        /// </summary>
        private Dictionary<string, NurDocInfo> m_htDocTable = null;

        /// <summary>
        /// 该哈希表用于缓存文档类型ID与表单编辑器的关系
        /// </summary>
        private Dictionary<string, DocumentControl> m_htFormTable = null;

        /// <summary>
        /// 该变量用于指示当前护理记录是否已经被更新过
        /// </summary>
        private bool m_recordUpdated = false;

        /// <summary>
        /// 该变量用于指示当前护理记录是否是新建状态
        /// </summary>
        private bool m_bIsNewRecord = false;

        /// <summary>
        /// 该变量用于缓存当前处于活动状态的表单编辑器
        /// </summary>
        private DocumentControl m_activeFormEditor = null;

        /// <summary>
        /// 该变量用于缓存当前窗口打开后默认显示的表单
        /// </summary>
        private DocTypeInfo m_docTypeInfo = null;

        public RecordEditForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.FormEdit;
            this.SaveWindowState = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.toolbtnSaveAs.Visible = RightController.Instance.CanShowSaveAsButton();

            this.m_recordUpdated = false;
            this.virtualTree1.ImageList.Images.Clear();
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderClose);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.NursingDoc);
            if (this.Modal)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.LoadNursingRecord(this.m_nursingRecInfo, this.m_docTypeInfo);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.toolbtnReturn.PerformClick();
            return base.ProcessDialogKey(keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        public DialogResult ShowDialog(IWin32Window owner
            , NursingRecInfo nursingRecInfo, DocTypeInfo docTypeInfo)
        {
            this.m_nursingRecInfo = nursingRecInfo;
            this.m_docTypeInfo = docTypeInfo;
            return base.ShowDialog(owner);
        }

        new public void Hide()
        {
            if (!this.CheckModifyState())
            {
                base.Hide();
                this.ResetModifyState();
                if (this.m_recordUpdated)
                    this.OnRecordUpdated(EventArgs.Empty);
            }
        }

        /// <summary>
        /// 释放加载的所有表单编辑器
        /// </summary>
        private void DisposeFormEditor()
        {
            if (this.m_htFormTable == null || this.m_htFormTable.Count <= 0)
                return;
            foreach (DocumentControl formEditor in this.m_htFormTable.Values)
            {
                if (formEditor != null && !formEditor.IsDisposed)
                    formEditor.Dispose();
            }
        }

        /// <summary>
        /// 判断指定的表单编辑器内容是否属于当前护理记录单
        /// </summary>
        /// <param name="formEditor">表单编辑器</param>
        /// <returns>是否属于当前护理记录单</returns>
        private bool IsCurrentNursingRecord(DocumentControl formEditor)
        {
            if (formEditor == null || formEditor.IsDisposed)
                return false;
            if (this.m_nursingRecInfo == null)
                return false;
            DocumentInfo document = formEditor.Document;
            if (document == null)
                return false;
            return (document.RecordID == this.m_nursingRecInfo.RecordID);
        }

        /// <summary>
        /// 获取已被修改的评估文档信息列表
        /// </summary>
        /// <returns>评估文档信息列表</returns>
        private DocumentControl[] GetModifiedDocuments()
        {
            if (this.m_nursingRecInfo == null)
                return null;
            if (this.m_htFormTable == null)
                this.m_htFormTable = new Dictionary<string, DocumentControl>();

            List<DocumentControl> formEditorList = new List<DocumentControl>();
            foreach (DocumentControl formEditor in this.m_htFormTable.Values)
            {
                if (!this.IsCurrentNursingRecord(formEditor))
                    continue;
                if (formEditor.IsModified)
                    formEditorList.Add(formEditor);
            }
            if (this.documentControl1 != null && !this.documentControl1.IsDisposed
                && this.documentControl1.IsModified)
                formEditorList.Add(this.documentControl1);
            return formEditorList.ToArray();
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public bool CheckModifyState()
        {
            DocumentControl[] formEditorList = this.GetModifiedDocuments();
            if (formEditorList == null || formEditorList.Length <= 0)
                return false;

            string message = "当前护理记录已经被修改,是否保存？";
            StringBuilder sbTitle = new StringBuilder();
            sbTitle.AppendLine();
            sbTitle.AppendLine("下面是被修改的评估列表：");
            foreach (DocumentControl formEditor in formEditorList)
            {
                if (formEditor == null || formEditor.IsDisposed)
                    continue;
                if (formEditor.Document != null)
                    sbTitle.AppendLine(formEditor.Document.ToString());
            }
            string details = sbTitle.ToString();

            DialogResult result = MessageBoxEx.ShowQuestion(message, details);
            if (result == DialogResult.Yes)
            {
                if (!this.m_bIsNewRecord)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    if (!RightController.Instance.CanEditNurRec(this.m_nursingRecInfo))
                        return false;
                }
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                bool success = this.SaveNursingRecord();
                if (success)
                {
                    this.SaveAllFormsData();
                    this.OnRecordUpdated(EventArgs.Empty);
                }
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return !success;
            }
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// 还原所有修改过的评估单的修改状态
        /// </summary>
        public void ResetModifyState()
        {
            DocumentControl[] formEditorList = this.GetModifiedDocuments();
            if (formEditorList == null || formEditorList.Length <= 0)
                return;
            foreach (DocumentControl formEditor in formEditorList)
            {
                if (formEditor != null && !formEditor.IsDisposed)
                    formEditor.IsModified = false;
            }
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
                if (bResetColor && node.ForeColor != Color.Black)
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
        /// 设置选中的节点
        /// </summary>
        /// <param name="docTypeInfo">文档对象</param>
        public void SetSelectNode(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null)
                return;
            VirtualNode selectedNode = null;
            if (docTypeInfo != null && this.m_htNodeTable != null)
            {
                if (this.m_htNodeTable.ContainsKey(docTypeInfo.DocTypeID))
                {
                    selectedNode = this.m_htNodeTable[docTypeInfo.DocTypeID];
                    this.virtualTree1.SelectNode(selectedNode);
                }
            }
        }

        /// <summary>
        /// 加载护理记录评估单列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadDocTypeList()
        {
            this.virtualTree1.Nodes.Clear();
            Application.DoEvents();

            if (this.m_htNodeTable == null)
                this.m_htNodeTable = new Dictionary<string, VirtualNode>();
            this.m_htNodeTable.Clear();

            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_RECORD;
            List<DocTypeInfo> lstDocTypeInfos =
                FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
            {
                MessageBoxEx.ShowError("护理记录模板列表下载失败!");
                return true;
            }

            bool bshowByType = SystemContext.Instance.SystemOption.OptionRecShowByType;

            //先创建节点哈希列表
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (docTypeInfo == null)
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                if (bshowByType
                    && docTypeInfo.SchemaID != null
                    && docTypeInfo.SchemaID != this.m_currentSchema.SchemaID)
                    continue;

                VirtualNode docTypeNode = new VirtualNode();
                docTypeNode.Tag = docTypeInfo;
                docTypeNode.Text = docTypeInfo.DocTypeName;

                if (!docTypeInfo.IsFolder)
                    docTypeNode.ImageIndex = 2;
                else
                    docTypeNode.ImageIndex = 0;
                docTypeNode.ImageIndex = docTypeNode.ImageIndex;

                if (!this.m_htNodeTable.ContainsKey(docTypeInfo.DocTypeID))
                    this.m_htNodeTable.Add(docTypeInfo.DocTypeID, docTypeNode);
            }

            //将节点连接起来,添加到树中
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

            if (this.GetVaildTempletCount(null) <= 1)
            {
                this.virtualTree1.Visible = false;
                this.arrowSplitter1.Visible = false;
            }
            else
            {
                this.virtualTree1.Visible = true;
                this.arrowSplitter1.Visible = true;
            }

            if (SystemContext.Instance.GetConfig(SystemConst.ConfigKey.NUR_REC_EXPAND, true))
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        /// <summary>
        /// 创建一条新的护理记录信息
        /// </summary>
        /// <returns>护理记录信息</returns>
        private NursingRecInfo CreateNursingRecord()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return null;
            if (SystemContext.Instance.LoginUser == null)
                return null;
            NursingRecInfo nursingRecInfo = new NursingRecInfo();
            nursingRecInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            nursingRecInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            nursingRecInfo.CreateTime = SysTimeService.Instance.Now;
            nursingRecInfo.ModifierID = nursingRecInfo.CreatorID;
            nursingRecInfo.ModifierName = nursingRecInfo.CreatorName;
            nursingRecInfo.ModifyTime = nursingRecInfo.CreateTime;
            nursingRecInfo.PatientID = PatientTable.Instance.ActivePatient.PatientID;
            nursingRecInfo.VisitID = PatientTable.Instance.ActivePatient.VisitID;
            nursingRecInfo.SubID = PatientTable.Instance.ActivePatient.SubID;
            nursingRecInfo.Recorder1Name = nursingRecInfo.CreatorName;
            nursingRecInfo.Recorder1ID = nursingRecInfo.CreatorID;
            nursingRecInfo.Recorder2ID = nursingRecInfo.CreatorID;
            nursingRecInfo.Recorder2Name = nursingRecInfo.CreatorName;

            nursingRecInfo.RecordTime = SysTimeService.Instance.Now;
            if (this.m_bIsFirstRecord)
            {
                DateTime? dtEventTime = null;
                VitalSignsService.Instance.GetAdmissionEventTime(nursingRecInfo.PatientID
                    , nursingRecInfo.VisitID, nursingRecInfo.SubID, ref dtEventTime);
                if (dtEventTime != null)
                    nursingRecInfo.RecordTime = dtEventTime.Value;
                else
                    nursingRecInfo.RecordTime = PatientTable.Instance.ActivePatient.VisitTime;
            }
            nursingRecInfo.RecordDate = nursingRecInfo.RecordTime.Date;
            nursingRecInfo.RecordTime = new DateTime(nursingRecInfo.RecordTime.Year
                , nursingRecInfo.RecordTime.Month, nursingRecInfo.RecordTime.Day
                , nursingRecInfo.RecordTime.Hour, nursingRecInfo.RecordTime.Minute, 0);

            nursingRecInfo.RecordID = nursingRecInfo.MakeRecordID();
            nursingRecInfo.PatientName = PatientTable.Instance.ActivePatient.PatientName;
            nursingRecInfo.WardCode = PatientTable.Instance.ActivePatient.WardCode;
            nursingRecInfo.WardName = PatientTable.Instance.ActivePatient.WardName;
            return nursingRecInfo;
        }

        /// <summary>
        /// 加载指定护理记录对应的护理评估单列表
        /// </summary>
        /// <param name="nursingRecInfo">护理记录信息</param>
        /// <param name="docTypeInfo">默认显示的评估单</param>
        /// <returns>是否加载成功</returns>
        public bool LoadNursingRecord(NursingRecInfo nursingRecInfo, DocTypeInfo docTypeInfo)
        {
            Application.DoEvents();
            if (this.m_htDocTable != null) this.m_htDocTable.Clear();
            if (this.m_htFormTable != null) this.m_htFormTable.Clear();
            this.m_nursingRecInfo = nursingRecInfo;
            this.m_recordUpdated = false;

            if (this.m_nursingRecInfo != null)
            {
                this.m_bIsNewRecord = false;
            }
            else
            {
                this.m_bIsNewRecord = true;
                this.m_nursingRecInfo = this.CreateNursingRecord();
                if (this.m_nursingRecInfo == null)
                    return false;
            }
            this.tooltxtAssessor.Text = this.m_nursingRecInfo.Recorder1Name;
            this.tooldtpAssessTime.Value = this.m_nursingRecInfo.RecordTime;

            if (this.m_activeFormEditor != null && !this.m_activeFormEditor.IsDisposed)
                this.m_activeFormEditor.CloseDocument();

            if (this.m_htNodeTable == null || this.m_htNodeTable.Count <= 0)
            {
                if (!this.LoadDocTypeList())
                    return false;
                if (!this.LoadRecordContentTemplet())
                    this.splitContainer2.Panel2Collapsed = true;
                else
                    this.splitContainer2.Panel2Collapsed = false;
            }
            this.LoadRecordContent(this.m_nursingRecInfo);

            if (!this.LoadRecordDocumentList())
                return false;

            //如果是已有记录,则打开做过的第一个评估
            if (this.m_htDocTable != null && this.m_htDocTable.Count > 0)
            {
                this.virtualTree1.SelectedNode = this.GetFirstDocuemntNode(null);
            }

            //默认打开第一个可用表单或者上次打开的表单
            VirtualNode selectedNode = this.virtualTree1.SelectedNode;
            if (selectedNode == null && this.m_htNodeTable != null)
            {
                selectedNode = this.GetFirstTempletNode(null);
                if (selectedNode != null)
                    this.virtualTree1.SelectedNode = selectedNode;
            }

            //如果上层已传入需要默认显示的表单,则选中
            if (docTypeInfo != null && this.m_htNodeTable != null)
            {
                if (this.m_htNodeTable.ContainsKey(docTypeInfo.DocTypeID))
                    selectedNode = this.m_htNodeTable[docTypeInfo.DocTypeID];
            }

            //依次展开当前选中的所有层次的父节点
            if (selectedNode != null)
            {
                this.virtualTree1.SuspendLayout();
                VirtualNode parentNode = selectedNode.Parent;
                while (parentNode != null)
                {
                    parentNode.Expand();
                    parentNode = parentNode.Parent;
                }
                this.virtualTree1.PerformLayout();
                this.virtualTree1.EnsureVisible(selectedNode);
            }

            //自动打开当前选中的节点对应的表单文档或者模板
            if (selectedNode != null && this.OpenFormDocument(selectedNode.Tag as DocTypeInfo))
            {
                this.ResetNodeTextAndColor(null, true, false, false);
                selectedNode.Font = new Font(this.virtualTree1.Font, FontStyle.Bold);
            }
            return true;
        }

        /// <summary>
        /// 加载护理记录对应的文档列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadRecordDocumentList()
        {
            if (this.m_htDocTable == null)
                this.m_htDocTable = new Dictionary<string, NurDocInfo>();
            this.m_htDocTable.Clear();

            this.ResetNodeTextAndColor(null, true, true, true);
            if (this.m_nursingRecInfo == null)
                return false;
            if (this.m_htNodeTable == null || this.m_htNodeTable.Count <= 0)
                return false;

            string szRecordID = this.m_nursingRecInfo.RecordID;
            NurDocList lstDocInfos = null;
            short shRet = DocumentService.Instance.GetRecordDocInfos(szRecordID, ref lstDocInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录模板列表下载失败!");
                return false;
            }
            if (lstDocInfos == null)
                lstDocInfos = new NurDocList();
            foreach (NurDocInfo docInfo in lstDocInfos)
            {
                if (!this.m_htDocTable.ContainsKey(docInfo.DocTypeID))
                {
                    this.m_htDocTable.Add(docInfo.DocTypeID, docInfo);
                }
                if (this.m_htNodeTable.ContainsKey(docInfo.DocTypeID))
                {
                    VirtualNode node = this.m_htNodeTable[docInfo.DocTypeID];
                    if (node != null) node.ForeColor = Color.Red;
                    if (this.virtualTree1.SelectedNode == null) this.virtualTree1.SelectedNode = node;
                }
            }
            return true;
        }

        /// <summary>
        /// 加载护理记录内容录入模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadRecordContentTemplet()
        {
            Application.DoEvents();
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_REC_CONTENT;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            if (docTypeInfo == null)
                return false;

            string szRecordID = null;
            if (this.m_nursingRecInfo != null)
                szRecordID = this.m_nursingRecInfo.RecordID;
            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            DocumentInfo document = DocumentInfo.Create(docTypeInfo, patVisit, szRecordID);
            bool result = this.documentControl1.OpenDocument(document);

            //自适应模板高度
            if (this.documentControl1.Controls.Count > 0)
            {
                int nContainerHeight = this.splitContainer2.Height;
                int nHeight = nContainerHeight - this.splitContainer2.SplitterDistance;
                if (this.documentControl1.Controls[0].Height > nHeight)
                {
                    nHeight = this.documentControl1.Controls[0].Height;
                    this.splitContainer2.SplitterDistance = nContainerHeight - nHeight - 8;
                }
            }
            Application.DoEvents();
            this.LoadRecordContent(this.m_nursingRecInfo);
            return result;
        }

        /// <summary>
        /// 加载护理记录单护理记录内容
        /// </summary>
        /// <param name="nursingRecInfo">护理记录信息</param>
        /// <returns>是否加载成功</returns>
        private bool LoadRecordContent(NursingRecInfo nursingRecInfo)
        {
            if (nursingRecInfo == null)
                return false;
            bool result = false;
            this.documentControl1.UpdateFormData("表单摘要", nursingRecInfo.RecordContent);
            this.documentControl1.UpdateFormData("备注内容", nursingRecInfo.RecordRemarks);
            this.documentControl1.IsModified = false;
            return result;
        }

        /// <summary>
        /// 获取指定目录节点下面可用的模板的数量
        /// </summary>
        /// <param name="parent">指定的父节点</param>
        /// <returns>可用的模板的数量</returns>
        private int GetVaildTempletCount(VirtualNode parent)
        {
            if (parent != null)
            {
                DocTypeInfo docTypeInfo = parent.Tag as DocTypeInfo;
                if (docTypeInfo != null && !docTypeInfo.IsFolder)
                    return 1;
            }

            int count = 0;
            VirtualNodeList nodes =
                (parent == null) ? this.virtualTree1.Nodes : parent.Nodes;
            foreach (VirtualNode childNode in nodes)
                count += this.GetVaildTempletCount(childNode);
            return count;
        }

        /// <summary>
        /// 获取指定节点下面所有子节点中,第一个可用的表单模板节点
        /// </summary>
        /// <param name="parent">指定的父节点</param>
        /// <returns>第一个可用的表单模板节点</returns>
        private VirtualNode GetFirstTempletNode(VirtualNode parent)
        {
            if (parent != null)
            {
                DocTypeInfo docTypeInfo = parent.Tag as DocTypeInfo;
                if (docTypeInfo != null && !docTypeInfo.IsFolder)
                    return parent;
            }

            VirtualNodeList nodes =
                (parent == null) ? this.virtualTree1.Nodes : parent.Nodes;
            foreach (VirtualNode childNode in nodes)
            {
                VirtualNode node = this.GetFirstTempletNode(childNode);
                if (node != null)
                    return node;
            }
            return null;
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
                DocTypeInfo docTypeInfo = parent.Tag as DocTypeInfo;
                if (docTypeInfo != null && !docTypeInfo.IsFolder
                    && this.m_htDocTable != null
                    && this.m_htDocTable.ContainsKey(docTypeInfo.DocTypeID))
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
        
        /// <summary>
        /// 保存当前修改的护理记录记录本身,不包含评估数据
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool SaveNursingRecord()
        {
            return this.SaveNursingRecord(DateTime.MinValue);
        }

        /// <summary>
        /// 保存当前修改的护理记录记录本身,不包含评估数据
        /// </summary>
        /// <param name="dtSaveTime">保存时间</param>
        /// <returns>是否加载成功</returns>
        private bool SaveNursingRecord(DateTime dtSaveTime)
        {
            Application.DoEvents();
            if (this.m_nursingRecInfo == null)
            {
                MessageBoxEx.ShowMessage("您没有打开任何模板!");
                return false;
            }
            if (GlobalMethods.Misc.IsEmptyString(this.tooltxtAssessor.Text))
            {
                MessageBoxEx.ShowMessage("记录人不能为空!");
                return false;
            }
            if (SystemContext.Instance.LoginUser == null)
                return false;

            this.m_nursingRecInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            this.m_nursingRecInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            this.m_nursingRecInfo.ModifyTime = SysTimeService.Instance.Now;
            this.m_nursingRecInfo.RecordDate = dtSaveTime != DateTime.MinValue ? dtSaveTime : this.tooldtpAssessTime.Value.Date;
            this.m_nursingRecInfo.RecordTime = dtSaveTime != DateTime.MinValue ? dtSaveTime : this.tooldtpAssessTime.Value.AddSeconds(-this.tooldtpAssessTime.Value.Second);//时间必须秒必须为00
            this.m_nursingRecInfo.Recorder2ID = this.m_nursingRecInfo.ModifierID;
            this.m_nursingRecInfo.Recorder2Name = this.m_nursingRecInfo.ModifierName;
            this.m_nursingRecInfo.SchemaID = this.m_currentSchema.SchemaID;

            //有些医院每次保存时都重新生成文本
            object value = this.documentControl1.GetFormData("保存重新生成文本");
            bool bMakeRecordContent = (value != null) ? (bool)value : false;
            object content = this.documentControl1.GetFormData("表单摘要");
            if (bMakeRecordContent || GlobalMethods.Misc.IsEmptyString(content))
            {
                content = this.MakeRecordContent(null);
                this.documentControl1.UpdateFormData("表单摘要", content);
            }
            if (GlobalMethods.Misc.IsEmptyString(content))
                this.m_nursingRecInfo.RecordContent = string.Empty;
            else
                this.m_nursingRecInfo.RecordContent = content as string;

            content = this.documentControl1.GetFormData("备注内容");
            if (GlobalMethods.Misc.IsEmptyString(content))
                this.m_nursingRecInfo.RecordRemarks = string.Empty;
            else
                this.m_nursingRecInfo.RecordRemarks = content as string;

            short shRet = SystemConst.ReturnValue.OK;
            if (this.m_bIsNewRecord)
            {
                shRet = NurRecService.Instance.SaveNursingRec(this.m_nursingRecInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("保存护理记录失败!");
                    return false;
                }
                this.m_bIsNewRecord = false;
            }
            else
            {
                shRet = NurRecService.Instance.UpdateNursingRec(this.m_nursingRecInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("更新护理记录失败!");
                    return false;
                }
            }
            this.documentControl1.IsModified = false;
            this.m_recordUpdated = true;

            if (!this.UpdateVitalSignsTime(this.m_nursingRecInfo))
                MessageBoxEx.ShowError("护理记录已更新成功! 但该护理记录中的体征数据更新失败!");
            return true;
        }

        /// <summary>
        /// 保存修改过的表单的文件数据,以及摘要、体征等数据
        /// </summary>
        /// <returns>是否保存成功</returns>
        private bool SaveAllFormsData()
        {
            if (this.m_htFormTable == null || this.m_htFormTable.Count <= 0)
                return true;
            if (this.m_nursingRecInfo == null)
                return false;
            foreach (DocumentControl formEditor in this.m_htFormTable.Values)
            {
                if (!this.IsCurrentNursingRecord(formEditor))
                    continue;
                if (!formEditor.EndEdit())
                    return false;
                if (!formEditor.IsModified)
                    continue;

                if (!formEditor.CheckFormData(null))
                    return false;

                //同步表单的记录时间
                DocumentInfo document = formEditor.Document;
                document.ModifyRecordTime(this.m_nursingRecInfo.RecordTime);
                if (string.IsNullOrEmpty(document.DocTypeID))
                    continue;

                //生成表单的文件数据
                if (!formEditor.SaveDocument())
                    continue;
                if (!formEditor.CommitDocument(document))
                {
                    if (!formEditor.Visible)
                        formEditor.Visible = true;
                    formEditor.BringToFront();
                    return false;
                }
                formEditor.IsModified = false;
                if (this.m_htDocTable != null && !this.m_htDocTable.ContainsKey(document.DocTypeID))
                    this.m_htDocTable.Add(document.DocTypeID, document.ToDocInfo());
            }
            return true;
        }

        /// <summary>
        /// 另存为保存修改过的表单的文件数据,以及摘要、体征等数据
        /// </summary>
        /// <returns>是否另存为成功</returns>
        private bool SaveAsAllFormsData()
        {
            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            if (this.m_htFormTable == null || this.m_htFormTable.Count <= 0)
                return true;
            if (this.m_nursingRecInfo == null)
                return false;
            Hashtable htAddedFrom = new Hashtable();

            int count = 0;//执行次数
            #region  保存已点开文档
            try
            {
                WorkProcess.Instance.Initialize(ParentForm, this.m_htFormTable.Values.Count, "保存当前已打开文档，请稍候...");
                //修改文档集合的记录ID
                foreach (DocumentControl formEditor in this.m_htFormTable.Values)
                {
                    WorkProcess.Instance.Show(++count, false);
                    if (!formEditor.EndEdit())
                        return false;
                    if (!formEditor.CheckFormData(null))
                        return false;
                    if (!formEditor.IsModified && !this.m_htDocTable.ContainsKey(formEditor.Document.DocTypeID))
                        continue;
                    string szOldDocSetID = formEditor.Document.DocSetID;
                    byte[] data = null;
                    formEditor.Save(ref data);
                    ChildDocumentCollection childs = formEditor.Document.Childs;
                    //同步表单的记录时间
                    formEditor.Document = DocumentInfo.Create(formEditor.Document.DocTypeID, formEditor.Document.DocTitle, patVisit, this.m_nursingRecInfo.RecordID);
                    DocumentInfo document = formEditor.Document;
                    document.ModifyRecordTime(this.m_nursingRecInfo.RecordTime);

                    #region 获取子文档并绑定到新的父文档中
                    //修改后未保存直接 另存为  数据应该为当前修改后的数据
                    foreach (DocumentInfo child in childs)
                    {
                        DocTypeInfo childDocTypeInfo = FormCache.Instance.GetDocTypeInfo(child.DocTypeID);
                        DocumentInfo info = DocumentInfo.Create(childDocTypeInfo, PatientTable.Instance.ActivePatient, null, this.m_nursingRecInfo.RecordTime);
                        info.Caller = child.Caller;
                        info.DocData = child.DocData;
                        document.Childs.Add(info);
                    }
                    List<ChildDocInfo> lstChildInfos = new List<ChildDocInfo>();
                    short shRet = DocumentService.Instance.GetChildDocList(szOldDocSetID, ref lstChildInfos);
                    foreach (ChildDocInfo child in lstChildInfos)
                    {
                        foreach (DocumentInfo m_child in childs)
                        {
                            if (m_child.DocSetID == child.ChildDocID)
                                goto CHECK_NEXT_CHILD;
                        }
                        NurDocInfo docChild = null;
                        byte[] child_data = null;
                        shRet = DocumentService.Instance.GetLatestDocInfo(child.ChildDocID, ref docChild);
                        if (shRet != SystemConst.ReturnValue.OK)
                            continue;
                        shRet = DocumentService.Instance.GetDocByID(docChild.DocID, ref child_data);
                        if (shRet != SystemConst.ReturnValue.OK)
                            continue;
                        DocTypeInfo childDocTypeInfo = FormCache.Instance.GetDocTypeInfo(docChild.DocTypeID);
                        DocumentInfo info = DocumentInfo.Create(childDocTypeInfo, PatientTable.Instance.ActivePatient, null, this.m_nursingRecInfo.RecordTime);
                        info.Caller = child.Caller;
                        info.DocData = child_data;
                        document.Childs.Add(info);
                    CHECK_NEXT_CHILD:
                        continue;
                    }
                    #endregion

                    //生成表单的文件数据
                    if (!formEditor.SaveDocument())
                        continue;
                    if (!formEditor.CommitDocument(document))
                    {
                        if (!formEditor.Visible)
                            formEditor.Visible = true;
                        formEditor.BringToFront();
                        return false;
                    }
                    if (this.m_htDocTable != null && !this.m_htDocTable.ContainsKey(document.DocTypeID))
                    {
                        this.m_htDocTable.Add(document.DocTypeID, document.ToDocInfo());
                    }
                    else if (this.m_htDocTable != null && this.m_htDocTable.ContainsKey(document.DocTypeID))
                    {
                        this.m_htDocTable[document.DocTypeID] = document.ToDocInfo();
                    }
                    formEditor.IsModified = false;
                    htAddedFrom.Add(document.DocTypeID, string.Empty);
                }

                WorkProcess.Instance.Close();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("已打开文档另存为失败", ex.Message.ToString());
                WorkProcess.Instance.Close();
                return false;
            }
            #endregion

            #region  保存未点开 但是已存在于上一个护理记录单中的其他文档
            try
            {
                DocumentControl control = new DocumentControl();
                WorkProcess.Instance.Initialize(ParentForm, this.m_htFormTable.Values.Count, "保存未打开但属于当前记录的表单，请稍候...");
                count = 0;
                foreach (NurDocInfo nurdoc in this.m_htDocTable.Values)
                {
                    WorkProcess.Instance.Show(++count, false);
                    if (htAddedFrom.ContainsKey(nurdoc.DocTypeID))
                        continue;
                    string szOldDocSetID = nurdoc.DocSetID;
                    byte[] data = null;
                    short shRet = DocumentService.Instance.GetDocByID(nurdoc.DocID, ref data);
                    if (shRet != SystemConst.ReturnValue.OK)
                        continue;

                    DocumentInfo document = DocumentInfo.Create(nurdoc.DocTypeID, nurdoc.DocTitle, patVisit, this.m_nursingRecInfo.RecordID);
                    document.DocData = data;
                    document.ModifyRecordTime(this.m_nursingRecInfo.RecordTime);
                    control.OpenDocument(document);

                    #region 获取子文档并绑定到新的父文档中
                    List<ChildDocInfo> lstChildInfos = new List<ChildDocInfo>();
                    shRet = DocumentService.Instance.GetChildDocList(szOldDocSetID, ref lstChildInfos);
                    foreach (ChildDocInfo child in lstChildInfos)
                    {
                        NurDocInfo docChild = null;
                        byte[] child_data = null;
                        shRet = DocumentService.Instance.GetLatestDocInfo(child.ChildDocID, ref docChild);
                        if (shRet != SystemConst.ReturnValue.OK)
                            continue;
                        shRet = DocumentService.Instance.GetDocByID(docChild.DocID, ref child_data);
                        if (shRet != SystemConst.ReturnValue.OK)
                            continue;
                        DocTypeInfo childDocTypeInfo = FormCache.Instance.GetDocTypeInfo(docChild.DocTypeID);
                        DocumentInfo info = DocumentInfo.Create(childDocTypeInfo, PatientTable.Instance.ActivePatient, null, this.m_nursingRecInfo.RecordTime);
                        info.Caller = child.Caller;
                        info.DocData = child_data;
                        document.Childs.Add(info);
                    }
                    #endregion

                    document = control.Document;
                    //生成表单的文件数据
                    if (!control.SaveDocument())
                        continue;
                    if (!control.CommitDocument(document))
                    {
                        continue;
                    }
                }

                WorkProcess.Instance.Close();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("未打开但属于当前记录的表单另存为失败！", ex.Message.ToString());
                WorkProcess.Instance.Close();
                return false;
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 修改指定护理记录中录入的体征记录的时间  
        ///  原考虑更新时间不如删除数据，但在后续的保存中未修改的单子不能保存原有的体征数据导致出错 - ycc
        /// </summary>
        /// <param name="recInfo">指定的护理记录</param>
        /// <returns>是否更新成功</returns>
        private bool UpdateVitalSignsTime(NursingRecInfo recInfo)
        {
            if (recInfo == null)
                return false;
            string szPatientID = recInfo.PatientID;
            string szVisitID = recInfo.VisitID;
            string szSubID = recInfo.SubID;
            string szSourceTag = recInfo.RecordID;
            string szSourceType = ServerData.VitalSignsSourceType.NUR_REC;
            List<VitalSignsData> lstVitalSignsData = null;
            short shRet = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, szSubID
                , szSourceTag, szSourceType, ref lstVitalSignsData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;

            if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
            {
                List<VitalSignsData> lstVitalSignsDataClone = new List<VitalSignsData>();
                foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                {
                    VitalSignsData vitalSignsDataClone = vitalSignsData.Clone() as VitalSignsData;
                    vitalSignsDataClone.RecordDate = recInfo.RecordDate;
                    vitalSignsDataClone.RecordTime = recInfo.RecordTime;
                    lstVitalSignsDataClone.Add(vitalSignsDataClone);
                }

                foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                {
                    vitalSignsData.RecordData = string.Empty;
                }
                lstVitalSignsData.AddRange(lstVitalSignsDataClone);
                shRet = VitalSignsService.Instance.SaveVitalSignsData(lstVitalSignsData);
                if (shRet != SystemConst.ReturnValue.OK)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 检查有没有编辑表单的权限
        /// </summary>
        private void CheckModifyRight()
        {
            if (!RightController.Instance.CanEditNurRec(this.m_nursingRecInfo)
                && this.m_activeFormEditor != null && this.documentControl1 != null)
            {
                this.m_activeFormEditor.UpdateFormData("更新控件状态", false);
                this.documentControl1.UpdateFormData("更新控件状态", false);
            }
            else
            {
                this.m_activeFormEditor.IsModified = this.m_activeFormEditor.UpdateFormData("更新控件状态", true);
                this.documentControl1.UpdateFormData("更新控件状态", true);
            }
        }

        /// <summary>
        /// 打开指定文档类型对应的表单文档,如果没有创建过,则创建
        /// </summary>
        /// <param name="docTypeInfo">文档类型信息</param>
        /// <returns>表单编辑器</returns>
        public bool OpenFormDocument(DocTypeInfo docTypeInfo)
        {
            this.m_activeFormEditor = null;
            if (docTypeInfo == null || string.IsNullOrEmpty(docTypeInfo.DocTypeID))
            {
                MessageBoxEx.ShowError("护理评估文档加载失败!文档类型信息非法!");
                return false;
            }
            if (docTypeInfo.IsFolder)
                return false;
            if (this.m_nursingRecInfo == null
                || string.IsNullOrEmpty(this.m_nursingRecInfo.RecordID))
            {
                MessageBoxEx.ShowError("护理评估文档加载失败!护理记录信息非法!");
                return false;
            }

            bool bPrintable = FormCache.Instance.IsFormPrintable(docTypeInfo.DocTypeID);
            if (this.toolbtnPreview.Visible != bPrintable)
                this.toolbtnPreview.Visible = bPrintable;
            if (this.toolbtnPrint.Visible != bPrintable)
                this.toolbtnPrint.Visible = bPrintable;

            if (this.m_htFormTable == null)
                this.m_htFormTable = new Dictionary<string, DocumentControl>();

            if (this.m_htFormTable.ContainsKey(docTypeInfo.DocTypeID))
                this.m_activeFormEditor = this.m_htFormTable[docTypeInfo.DocTypeID];
            if (this.m_activeFormEditor == null || this.m_activeFormEditor.IsDisposed)
            {
                this.m_activeFormEditor = new DocumentControl();
                this.m_activeFormEditor.Dock = DockStyle.Fill;
                this.m_activeFormEditor.Parent = this.splitContainer2.Panel1;
                this.m_activeFormEditor.DocumentChanged +=
                    new EventHandler(this.FormEditor_DocumentChanged);
                this.m_activeFormEditor.QueryContext +=
                    new Heren.Common.Forms.Editor.QueryContextEventHandler(this.FormEditor_QueryContext);
                this.m_activeFormEditor.CustomEvent +=
                    new Heren.Common.Forms.Editor.CustomEventHandler(this.FormEditor_CustomEvent);
            }

            if (!this.m_activeFormEditor.Visible)
                this.m_activeFormEditor.Visible = true;
            this.m_activeFormEditor.BringToFront();

            if (this.m_htFormTable.ContainsKey(docTypeInfo.DocTypeID))
                this.m_htFormTable[docTypeInfo.DocTypeID] = this.m_activeFormEditor;
            else
                this.m_htFormTable.Add(docTypeInfo.DocTypeID, this.m_activeFormEditor);

            if (this.IsCurrentNursingRecord(this.m_activeFormEditor)
                && this.m_activeFormEditor.Document.DocTypeID == docTypeInfo.DocTypeID)
            {
                return true;
            }

            DocumentInfo document = null;
            if (this.m_htDocTable != null
                && this.m_htDocTable.ContainsKey(docTypeInfo.DocTypeID))
            {
                document = DocumentInfo.Create(this.m_htDocTable[docTypeInfo.DocTypeID]);
            }
            else
            {
                PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
                document = DocumentInfo.Create(docTypeInfo, patVisit, this.m_nursingRecInfo.RecordID);
            }
            bool result = this.m_activeFormEditor.OpenDocument(document);
            if (result)
            {
                this.m_activeFormEditor.IsModified = this.m_activeFormEditor.UpdateFormData("数据同步", null);
                if (this.m_dicSummaryDatas != null && this.m_dicSummaryDatas.Count > 0)
                    this.m_activeFormEditor.UpdateFormKeyData(this.m_dicSummaryDatas);
                this.m_activeFormEditor.IsModified = false;
            }

            this.CheckModifyRight();
            return result;
        }

        private void FormEditor_DocumentChanged(object sender, EventArgs e)
        {
            DocumentControl formEditor = sender as DocumentControl;
            if (!this.IsCurrentNursingRecord(formEditor))
                return;
            if (string.IsNullOrEmpty(formEditor.Document.DocTypeID))
                return;
            if (!this.m_htNodeTable.ContainsKey(formEditor.Document.DocTypeID))
                return;

            VirtualNode node = this.m_htNodeTable[formEditor.Document.DocTypeID];
            if (node == null)
                return;
            if (formEditor.IsModified)
            {
                if (formEditor.Document.DocState == DocumentState.New
                    && node.ForeColor != Color.Blue)
                {
                    node.ForeColor = Color.Blue;
                }
                if (!node.Text.EndsWith(" *")) node.Text = node.Text + " *";
            }
            else
            {
                if (formEditor.Document.DocState == DocumentState.New
                    && node.ForeColor != Color.Black)
                {
                    node.ForeColor = Color.Black;
                }
                if (node.Text.EndsWith(" *")) node.Text = node.Text.Remove(node.Text.Length - 2);
            }
            if (formEditor.Document.DocState != DocumentState.New && node.ForeColor != Color.Red)
                node.ForeColor = Color.Red;
        }

        private void FormEditor_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            if (e.Name == "评估时间" || e.Name == "记录时间")
            {
                e.Success = true;
                e.Value = this.tooldtpAssessTime.Value.AddSeconds(-this.tooldtpAssessTime.Value.Second);
            }
            else if (e.Name == "列显示方案代码")
            {
                e.Success = true;
                if (this.m_currentSchema != null)
                    e.Value = this.m_currentSchema.SchemaID;
            }
            else if (e.Name == "列显示方案名称")
            {
                e.Success = true;
                if (this.m_currentSchema != null)
                    e.Value = this.m_currentSchema.SchemaName;
            }
            else if (e.Name == "列显示方案标记")
            {
                e.Success = true;
                if (this.m_currentSchema != null)
                    e.Value = this.m_currentSchema.SchemaFlag;
            }
        }

        private void FormEditor_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "保存")
            {
                this.toolbtnSave.PerformClick();
            }
            else if (e.Name == "生成表单摘要")
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                string szRecordContent = this.MakeRecordContent(null);
                this.documentControl1.UpdateFormData("表单摘要", szRecordContent);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            else if (e.Name == "更新记录时间")
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                DateTime recordTime = DateTime.Parse(e.Param.ToString());
                this.tooldtpAssessTime.Value = DateTime.Parse(e.Param.ToString());
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            else if (e.Name == "更新表单数据")
            {
                if (this.m_nursingRecInfo == null || e.Param == null)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                foreach (DocumentControl formEditor in this.m_htFormTable.Values)
                {
                    if (sender != formEditor && this.IsCurrentNursingRecord(formEditor))
                        formEditor.UpdateFormData(e.Param.ToString(), e.Data);
                }
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        /// <summary>
        /// 生成当前护理记录单的自动生成部分的记录内容
        /// </summary>
        /// <param name="parentNode">根节点</param>
        /// <returns>记录内容</returns>
        private string MakeRecordContent(VirtualNode parentNode)
        {
            string szRecordContent = string.Empty;

            if (parentNode != null)
            {
                DocTypeInfo docTypeInfo = parentNode.Tag as DocTypeInfo;
                if (parentNode.Text.EndsWith(" *") || parentNode.ForeColor != Color.Black)
                    szRecordContent = this.GetSummaryContent(docTypeInfo);
            }

            VirtualNodeList nodes = null;
            if (parentNode == null)
                nodes = this.virtualTree1.Nodes;
            else
                nodes = parentNode.Nodes;
            foreach (VirtualNode node in nodes)
                szRecordContent += this.MakeRecordContent(node);
            return szRecordContent;
        }

        /// <summary>
        /// 获取当前已写的指定类型评估单的摘要数据
        /// </summary>
        /// <param name="docTypeInfo">病历类型</param>
        /// <returns>摘要数据</returns>
        private string GetSummaryContent(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || string.IsNullOrEmpty(docTypeInfo.DocTypeID))
                return string.Empty;
            if (this.m_nursingRecInfo == null)
                return string.Empty;

            //如果已经打开了评估单,那么从评估单中生成
            if (this.m_htFormTable != null && this.m_htFormTable.ContainsKey(docTypeInfo.DocTypeID))
            {
                DocumentControl formEditor = this.m_htFormTable[docTypeInfo.DocTypeID];
                if (this.IsCurrentNursingRecord(formEditor))
                {
                    formEditor.EndEdit();
                    if (formEditor.IsModified)
                        return formEditor.GetFormData("表单摘要") as string;
                }
            }

            //如果没有打开该评估单,那么从数据库中读取摘要
            if (this.m_htDocTable != null && this.m_htDocTable.ContainsKey(docTypeInfo.DocTypeID))
            {
                NurDocInfo docInfo = this.m_htDocTable[docTypeInfo.DocTypeID];
                if (docInfo == null)
                    return string.Empty;

                string szDocSetID = docInfo.DocSetID;
                SummaryData summaryData = null;
                short shRet = DocumentService.Instance.GetSummaryData(szDocSetID, "表单摘要", ref summaryData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowErrorFormat("表单“{0}”的摘要数据下载失败!", null, docInfo.DocTitle);
                }
                if (summaryData != null)
                    return summaryData.DataValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断指定的文档类型的评估单是否可以取消评估
        /// </summary>
        /// <param name="docTypeInfo">文档类型信息</param>
        /// <returns>是否可以取消评估</returns>
        private bool IsDocumentAllowCanceled(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || string.IsNullOrEmpty(docTypeInfo.DocTypeID))
                return false;
            if (this.m_htFormTable != null && this.m_htFormTable.ContainsKey(docTypeInfo.DocTypeID))
            {
                DocumentControl formEditor = this.m_htFormTable[docTypeInfo.DocTypeID];
                if (this.IsCurrentNursingRecord(formEditor) && formEditor.IsModified)
                    return true;
            }
            if (this.m_htDocTable != null && this.m_htDocTable.ContainsKey(docTypeInfo.DocTypeID))
                return true;
            return false;
        }

        /// <summary>
        /// 删除指定的文档类型已经做过的评估单
        /// </summary>
        /// <param name="docTypeInfo">文档类型信息</param>
        /// <returns>是否删除成功</returns>
        private bool DeleteAssess(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || string.IsNullOrEmpty(docTypeInfo.DocTypeID))
                return false;

            DialogResult result = MessageBoxEx.ShowConfirmFormat("确定清除已做的评估单“{0}”吗？"
                , null, docTypeInfo.DocTypeName);
            if (result != DialogResult.OK)
                return false;

            Application.DoEvents();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_htDocTable != null && this.m_htDocTable.ContainsKey(docTypeInfo.DocTypeID))
            {
                if (!RightController.Instance.UserRight.EditNuringRec.Value)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    MessageBoxEx.ShowWarning("您没有权限修改护理记录!");
                    return false;
                }
                NurDocInfo docInfo = this.m_htDocTable[docTypeInfo.DocTypeID];
                DocStatusInfo docStatus = new DocStatusInfo();
                docStatus.DocID = docInfo.DocID;
                docStatus.OperatorID = SystemContext.Instance.LoginUser.ID;
                docStatus.OperatorName = SystemContext.Instance.LoginUser.Name;
                docStatus.DocStatus = ServerData.DocStatus.CANCELED;
                short shRet = DocumentService.Instance.SetDocStatusInfo(ref docStatus);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    MessageBoxEx.ShowErrorFormat("评估单“{0}”清除失败!无法更新评估单状态!"
                        , docStatus.StatusMessage, docTypeInfo.DocTypeName);
                    return false;
                }
                this.m_htDocTable.Remove(docTypeInfo.DocTypeID);
            }

            if (this.m_htFormTable != null && this.m_htFormTable.ContainsKey(docTypeInfo.DocTypeID))
            {
                DocumentControl formEditor = this.m_htFormTable[docTypeInfo.DocTypeID];
                if (this.IsCurrentNursingRecord(formEditor))
                {
                    formEditor.CloseDocument();
                    this.OpenFormDocument(docTypeInfo);
                    this.ResetNodeTextAndColor(this.virtualTree1.SelectedNode, false, true, true);
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.mnuCancelAssess.Enabled = false;
            VirtualNode selectedNode = this.virtualTree1.SelectedNode;
            if (selectedNode != null)
            {
                DocTypeInfo docTypeInfo = selectedNode.Tag as DocTypeInfo;
                this.mnuCancelAssess.Enabled = this.IsDocumentAllowCanceled(docTypeInfo);
            }
            this.mnuExpandAll.Checked =
                SystemContext.Instance.GetConfig(SystemConst.ConfigKey.NUR_REC_EXPAND, true);
        }

        private void mnuCancelAssess_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.virtualTree1.SelectedNode != null)
                this.DeleteAssess(this.virtualTree1.SelectedNode.Tag as DocTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            this.mnuExpandAll.Checked = !this.mnuExpandAll.Checked;
            if (this.mnuExpandAll.Checked)
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();
            SystemContext.Instance.WriteConfig(SystemConst.ConfigKey.NUR_REC_EXPAND
                , this.mnuExpandAll.Checked.ToString());
        }

        private void tooldtpAssessTime_ValueChanged(object sender, EventArgs e)
        {
            if (this.m_activeFormEditor != null && this.m_nursingRecInfo != null)
                this.m_activeFormEditor.IsModified = this.m_activeFormEditor.UpdateFormData("数据同步", null);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.tooldtpAssessTime.EndEdit();

            if (!this.m_bIsNewRecord)
            {
                if (!RightController.Instance.CanEditNurRec(this.m_nursingRecInfo))
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
            }

            if (this.SaveNursingRecord())
            {
                this.SaveAllFormsData();

                string key = SystemConst.ConfigKey.NUR_REC_SAVE_RETURN;
                if (SystemContext.Instance.GetConfig(key, false))
                {
                    base.Hide();
                    this.ResetModifyState();
                    this.OnRecordUpdated(EventArgs.Empty);
                }
                //MessageBoxEx.ShowMessage("护理记录保存成功!");
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSaveAs_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.tooldtpAssessTime.EndEdit();
            //if (!this.m_bIsNewRecord)
            //{
            //    if (!RightController.Instance.CanEditNurRec(this.m_nursingRecInfo))
            //    {
            //        GlobalMethods.UI.SetCursor(this, Cursors.Default);
            //        return;
            //    }
            //}
            SetDateTimeForm setDateTimeForm = new SetDateTimeForm();
            setDateTimeForm.Text = "另存为时间";
            if (setDateTimeForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            DateTime dtSaveAsTime = setDateTimeForm.SelectedTime;

            this.m_bIsNewRecord = true;
            string szOldRecID = this.m_nursingRecInfo.RecordID;
            this.m_nursingRecInfo = this.CreateNursingRecord();
            if (this.SaveNursingRecord(dtSaveAsTime))
            {
                this.BringToFront();
                if (this.SaveAsAllFormsData())
                {
                    MessageBox.Show("另存为成功");
                    this.tooldtpAssessTime.Value = this.m_nursingRecInfo.RecordTime;
                    string key = SystemConst.ConfigKey.NUR_REC_SAVE_RETURN;
                    if (SystemContext.Instance.GetConfig(key, false))
                    {
                        base.Hide();
                        this.ResetModifyState();
                        this.OnRecordUpdated(EventArgs.Empty);
                    }
                }
            }

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            if (this.CheckModifyState())
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_dicSummaryDatas = null;
            this.LoadNursingRecord(null, null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPreview_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            if (this.m_activeFormEditor != null && !this.m_activeFormEditor.IsDisposed)
                this.m_activeFormEditor.PreviewDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPrint_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            if (this.m_activeFormEditor != null && !this.m_activeFormEditor.IsDisposed)
                this.m_activeFormEditor.PrintDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            if (this.m_nursingRecHistory != null)
            {
                this.m_nursingRecHistory.Dispose();
            }
            this.Hide();
        }

        private void toolbtnHistory_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.m_nursingRecHistory == null || this.m_nursingRecHistory.IsDisposed)
            {
                this.m_nursingRecHistory = new NursingRecHistory();
                if (this.documentControl1 != null && !this.documentControl1.IsDisposed)
                {
                    this.m_nursingRecHistory.Location = this.documentControl1.PointToScreen(new Point());
                    m_nursingRecHistory.Size = new Size(this.documentControl1.Width, this.documentControl1.Height);
                    m_nursingRecHistory.StartPosition = FormStartPosition.Manual;
                }
                m_nursingRecHistory.CurrentSchema = this.m_currentSchema;
                m_nursingRecHistory.Show(this);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void virtualTree1_NodeMouseClick(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null)
                return;
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            if (this.OpenFormDocument(e.Node.Tag as DocTypeInfo))
            {
                this.ResetNodeTextAndColor(null, true, false, false);
                e.Node.Font = new Font(this.virtualTree1.Font, FontStyle.Bold);
            }
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }

        private void splitContainer2_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = this.splitContainer2.SplitterRectangle;
            using (Pen pen = new Pen(Color.DodgerBlue))
            {
                e.Graphics.DrawLine(pen, rect.X, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = this.virtualTree1.Bounds;
            using (Pen pen = new Pen(Color.DodgerBlue))
            {
                e.Graphics.DrawRectangle(pen, rect.X - 1, rect.Y - 2, rect.Width + 2, rect.Height + 2);
            }
        }
    }
}