using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Heren.Common.Controls.VirtualTreeView;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.InfoLib.DockForms
{
    public partial class InfoLibMenuForm : Form
    {
        private InfoLibForm m_templetForm = null;

        internal InfoLibMenuForm(InfoLibForm templetForm)
        {
            InitializeComponent();
            this.m_templetForm = templetForm;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.virtualTree1.ImageList.Images.Clear();
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderClose);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.NewDoc);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.Arrow);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadInfoLibList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 病历类型与节点对应关系表
        /// </summary>
        private Dictionary<string, VirtualNode> m_nodeTable = null;

        /// <summary>
        /// 加载所有护理信息库主目录列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadInfoLibList()
        {
            if (this.m_nodeTable == null)
                this.m_nodeTable = new Dictionary<string, VirtualNode>();
            this.m_nodeTable.Clear();

            this.virtualTree1.Nodes.Clear();
            Application.DoEvents();

            FieldInfo[] fieldInfos = typeof(ServerData.InfoLibList).GetFields();
            if (fieldInfos == null || fieldInfos.Length <= 0)
                return true;
            for (int index = 0; index < fieldInfos.Length; index++)
            {
                FieldInfo fieldInfo = fieldInfos[index];
                if (fieldInfo == null)
                    continue;
                object value = fieldInfo.GetValue(null);
                if (value == null)
                    continue;
                string szValue = ServerData.InfoLibList.GetInfoLibListName(value.ToString());
                VirtualNode infoLibNode = new VirtualNode();
                infoLibNode.Tag = value.ToString();
                infoLibNode.Text = szValue;

                infoLibNode.ImageIndex = 3;

                if (!this.m_nodeTable.ContainsKey(szValue))
                    this.m_nodeTable.Add(value.ToString(), infoLibNode);
            }

            //将节点连接起来,添加到树中
            foreach (KeyValuePair<string, VirtualNode> pair in this.m_nodeTable)
            {
                if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    continue;

                string value = pair.Value.Tag as string;
                if (value == null)
                    continue;
                this.virtualTree1.Nodes.Add(pair.Value);
            }
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        private void virtualTree1_AfterSelect(object sender, VirtualTreeEventArgs e)
        {
        }

        private void virtualTree1_NodeMouseClick(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;

            //检查是否已经打开
            object szValue = e.Node.Tag as object;
            if (szValue == null)
                return;

            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            this.m_templetForm.ShowChildInfoLib(szValue.ToString());
            e.Node.Font = new Font(this.virtualTree1.Font, FontStyle.Bold);
        
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }
    }
}