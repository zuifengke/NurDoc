// ***********************************************************
// 病案质控系统质检信息对话框.
// Creator:LiChunYing  Date:2011-09-13
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.Dialogs
{
    public partial class QuestionTypeForm : HerenForm
    {
        private string m_szQuestionType;
        /// <summary>
        /// 获取或设置质检信息问题类型
        /// </summary>
        public string QuestionType
        {
            get { return this.m_szQuestionType; }
            set { this.m_szQuestionType = value; }
        }

        private string m_szMessageTemplet;
        /// <summary>
        /// 获取或设置质检信息模板
        /// </summary>
        public string MessageTemplet
        {
            get { return this.m_szMessageTemplet; }
            set { this.m_szMessageTemplet = value; }
        }

        private string m_szMessageTitle;
        /// <summary>
        /// 获取或设置质检信息标题
        /// </summary>
        public string MessageTempletTitle
        {
            get { return this.m_szMessageTitle; }
            set { this.m_szMessageTitle = value; }
        }


        private string m_szScore;
        /// <summary>
        /// 获取或设置扣分
        /// </summary>
        public string Score
        {
            get { return this.m_szScore; }
            set { this.m_szScore = value; }
        }

        private string m_szMessageCode;
        /// <summary>
        /// 获取或设置质检信息模板编码
        /// </summary>
        public string MessageCode
        {
            get { return this.m_szMessageCode; }
            set { this.m_szMessageCode = value; }
        }

        public QuestionTypeForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            List<QCEventType> lstQCEventTypes = null;
            short shRet = CommonService.Instance.GetQCEventTypeList(ref lstQCEventTypes);
            if (shRet != ServerData.ExecuteResult .OK)
            {
                MessageBoxEx.Show("病案质量问题分类字典下载失败!");
                return;
            }
            if (lstQCEventTypes == null || lstQCEventTypes.Count <= 0)
                return;
            for (int index = 0; index < lstQCEventTypes.Count; index++)
            {
                if (!string.IsNullOrEmpty(lstQCEventTypes[index].ParnetCode))
                    continue;
                QCEventType qcEventType = lstQCEventTypes[index];
                TreeNode treeNode = new TreeNode();
                treeNode.Text = qcEventType.TypeDesc;
                treeNode.ImageIndex = 0;
                treeNode.SelectedImageIndex = 0;
                this.treeView1.Nodes.Add(treeNode);
                this.AppendChildNode(qcEventType.TypeDesc, treeNode);
            }
        }

        /// <summary>
        /// 根据类型查找对应的质检信息模板列表
        /// <param name="szQuestionType">问题类型</param>
        /// <param name="node">当前父节点</param>
        /// </summary>
        private void AppendChildNode(string szQuestionType, TreeNode parentNode)
        {
            List<QCMessageTemplet> lstMessage = null;
            short shRet =CommonService.Instance.GetQCMessageTempletList(szQuestionType, ref lstMessage);
            if (shRet != ServerData.ExecuteResult.OK || lstMessage == null)
                return;
            //根据MessageTitle再分组
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            for (int index = 0; index < lstMessage.Count; index++)
            {
                QCMessageTemplet qcMessageTemplet = lstMessage[index];
                if (!ht.ContainsKey(qcMessageTemplet.MessageTitle))
                {
                    List<QCMessageTemplet> lstQCMessageTemplet = this.GetSameTitleTemplet(qcMessageTemplet.MessageTitle, lstMessage);
                    if (lstQCMessageTemplet == null || lstQCMessageTemplet.Count == 0)
                        continue;
                    ht.Add(qcMessageTemplet.MessageTitle, lstQCMessageTemplet);
                }
            }

            foreach (System.Collections.DictionaryEntry entry in ht)
            {
                List<QCMessageTemplet> lsts = (List<QCMessageTemplet>)entry.Value;
                if (entry.Key.ToString() != "")
                {
                    TreeNode level2Node = new TreeNode();
                    level2Node.Text = entry.Key.ToString();
                    level2Node.ImageIndex = 0;
                    level2Node.SelectedImageIndex = 0;
                    parentNode.Nodes.Add(level2Node);
                    //创建有标题的节点
                    for (int index = 0; index < lsts.Count; index++)
                    {
                        TreeNode childNode = new TreeNode();
                        childNode.Tag = lsts[index];
                        childNode.Text = lsts[index].Message;
                        childNode.ImageIndex = 2;
                        childNode.SelectedImageIndex = 2;
                        level2Node.Nodes.Add(childNode);
                    }
                }
            }
            
            //创建没有标题的节点
            List<QCMessageTemplet> lstEmpytTitle = (List<QCMessageTemplet>)ht[string.Empty];
            if (lstEmpytTitle == null || lstEmpytTitle.Count == 0)
                return;
            for (int index = 0; index < lstEmpytTitle.Count; index++)
            {
                TreeNode childNode = new TreeNode();
                childNode.Tag = lstEmpytTitle[index];
                childNode.Text = lstEmpytTitle[index].Message;
                childNode.ImageIndex = 2;
                childNode.SelectedImageIndex = 2;
                parentNode.Nodes.Add(childNode);
            }
        }

        /// <summary>
        /// 获取相同MessageTitle的质检信息反馈模板
        /// </summary>
        /// <param name="szMessageTitle"></param>
        /// <param name="lstMessage"></param>
        /// <returns></returns>
        private List<QCMessageTemplet> GetSameTitleTemplet(string szMessageTitle, List<QCMessageTemplet> lstMessage)
        {
            if (lstMessage == null || lstMessage.Count == 0)
                return null;
            List<QCMessageTemplet> lstSameTitleTemplet = new List<QCMessageTemplet>();
            for (int index = 0; index < lstMessage.Count; index++)
            {
                if (lstMessage[index].MessageTitle.Equals(szMessageTitle))
                    lstSameTitleTemplet.Add(lstMessage[index]);
            }
            return lstSameTitleTemplet;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = this.treeView1.SelectedNode;
            if (selectNode == null || selectNode.Parent == null)
            {
                MessageBoxEx.Show("选择一个问题项目!", MessageBoxIcon.Warning);
                return;
            }
            QCMessageTemplet qcMessage = selectNode.Tag as QCMessageTemplet;
            if (qcMessage == null)
            {
                MessageBoxEx.Show("选择一个问题项目!", MessageBoxIcon.Warning);
                return;
            }

            SetQcMessage(qcMessage);
            this.selectedQCMessageTemplet = qcMessage;
            this.DialogResult = DialogResult.OK;
        }

        private QCMessageTemplet selectedQCMessageTemplet = null;

        public QCMessageTemplet SelectedQCMessageTemplet
        {
            get { return selectedQCMessageTemplet; }
            set { selectedQCMessageTemplet = value; }
        }

        private void SetQcMessage(QCMessageTemplet qcMessage)
        {
            this.QuestionType = qcMessage.QCEventType;
            this.MessageTemplet = qcMessage.Message;
            this.MessageTempletTitle = string.IsNullOrEmpty(qcMessage.MessageTitle) ? qcMessage.Message : qcMessage.MessageTitle;
            this.MessageCode = qcMessage.QCMsgCode;
            this.Score = qcMessage.Score;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
                return;
            if (selectedNode.Parent == null)
                return;

            QCMessageTemplet qcMessage = selectedNode.Tag as QCMessageTemplet;
            if (qcMessage == null)
                return;

            SetQcMessage(qcMessage);
            this.selectedQCMessageTemplet = qcMessage;
            this.DialogResult = DialogResult.OK;
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }
    }
}