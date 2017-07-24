// ***********************************************************
// 护理电子病历系统,护理评估单查询统计管理窗口.
// Creator:XiaoPingping  Date:2013-4-1
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
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.Controls.VirtualTreeView;
using Heren.Common.DockSuite;
using Heren.Common.Forms;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities.Forms;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class NursingStatForm : DockContentBase
    {
        private Dictionary<string, FormControl> m_htStatForm = null;

        public NursingStatForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.NursingStat;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 2000);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();

            this.virtualTree1.ImageList.Images.Clear();
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.FolderOpen);
            this.virtualTree1.ImageList.Images.Add(Properties.Resources.Templet);

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadStatisticNodeList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private bool LoadStatisticNodeList()
        {
            this.virtualTree1.Nodes.Clear();
            this.DisposeAllForms();
            Application.DoEvents();

            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_STATISTIC;
            List<DocTypeInfo> lstDocTypeInfo = FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfo == null)
            {
                MessageBoxEx.ShowError("查询统计列表下载失败!");
                return false;
            }

            //先创建节点哈希列表
            Dictionary<string, VirtualNode> nodeTable =
                new Dictionary<string, VirtualNode>();
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfo)
            {
                if (docTypeInfo == null)
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                //if (!RightController.Instance.UserRight.ShowNursingQCForm.Value && docTypeInfo.IsQCVisible)
                //    continue;
                if (!docTypeInfo.IsQCVisible)
                    continue;

                VirtualNode docTypeNode = new VirtualNode();
                docTypeNode.Tag = docTypeInfo;
                docTypeNode.Text = docTypeInfo.DocTypeName;

                if (!docTypeInfo.IsFolder)
                    docTypeNode.ImageIndex = 2;
                else
                    docTypeNode.ImageIndex = 0;
                docTypeNode.ImageIndex = docTypeNode.ImageIndex;

                if (!nodeTable.ContainsKey(docTypeInfo.DocTypeID))
                    nodeTable.Add(docTypeInfo.DocTypeID, docTypeNode);
            }

            //将节点连接起来,添加到树中
            foreach (KeyValuePair<string, VirtualNode> pair in nodeTable)
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
                    if (!nodeTable.ContainsKey(docTypeInfo.ParentID))
                        continue;
                    VirtualNodeList nodes = nodeTable[docTypeInfo.ParentID].Nodes;
                    nodes.Add(pair.Value);
                }
            }
            nodeTable.Clear();
            if (SystemContext.Instance.GetConfig(SystemConst.ConfigKey.STATISTIC_EXPAND, true))
                this.virtualTree1.ExpandAll();
            if (this.virtualTree1.Nodes.Count > 0) this.virtualTree1.Nodes[0].EnsureVisible();
            return true;
        }

        /// <summary>
        /// 释放掉当前所有表单
        /// </summary>
        private void DisposeAllForms()
        {
            if (this.m_htStatForm == null || this.m_htStatForm.Count <= 0)
                return;
            foreach (FormControl form in this.m_htStatForm.Values)
                form.Dispose();
            this.m_htStatForm.Clear();
        }

        /// <summary>
        /// 打开指定的任务表单
        /// </summary>
        /// <param name="docTypeInfo">任务表单信息</param>
        /// <returns>是否打开成功</returns>
        private bool ShowStatForm(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null
                || docTypeInfo.IsFolder || string.IsNullOrEmpty(docTypeInfo.DocTypeID))
                return false;
            if (this.m_htStatForm == null)
                this.m_htStatForm = new Dictionary<string, FormControl>();

            FormControl statForm = null;
            if (this.m_htStatForm.ContainsKey(docTypeInfo.DocTypeID))
                statForm = this.m_htStatForm[docTypeInfo.DocTypeID];
            if (statForm != null && !statForm.IsDisposed)
            {
                if (!statForm.Visible)
                    statForm.Visible = true;
                statForm.BringToFront();
                return true;
            }

            statForm = new FormControl();
            statForm.Dock = DockStyle.Fill;
            statForm.Parent = this.splitContainer1.Panel2;
            statForm.BringToFront();
            statForm.CustomEvent +=
              new Heren.Common.Forms.Editor.CustomEventHandler(this.StatForm_CustomEvent);
            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                string szStatName = docTypeInfo.DocTypeName;
                MessageBoxEx.ShowErrorFormat("统计项“{0}”下载失败!", null, szStatName);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
            bool success = statForm.Load(docTypeInfo, byteFormData);
            if (success)
            {
                if (this.m_htStatForm.ContainsKey(docTypeInfo.DocTypeID))
                    this.m_htStatForm.Remove(docTypeInfo.DocTypeID);
                this.m_htStatForm.Add(docTypeInfo.DocTypeID, statForm);
            }
            return success;
        }

        private void virtualTree1_NodeMouseClick(object sender, VirtualTreeEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;

            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.WaitCursor);
            this.ShowStatForm(e.Node.Tag as DocTypeInfo);
            GlobalMethods.UI.SetCursor(this.virtualTree1, Cursors.Default);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.mnuExpandAll.Checked =
                SystemContext.Instance.GetConfig(SystemConst.ConfigKey.STATISTIC_EXPAND, true);
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            this.mnuExpandAll.Checked = !this.mnuExpandAll.Checked;
            if (this.mnuExpandAll.Checked)
                this.virtualTree1.ExpandAll();
            else
                this.virtualTree1.CollapseAll();
            SystemContext.Instance.WriteConfig(SystemConst.ConfigKey.STATISTIC_EXPAND
                , this.mnuExpandAll.Checked.ToString());
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = this.virtualTree1.Bounds;
            Pen pen = new Pen(Color.DodgerBlue);
            e.Graphics.DrawRectangle(pen, rect.X - 1, rect.Y - 2, rect.Width + 2, rect.Height + 2);
            pen.Dispose();
        }

        private void StatForm_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "打开病人窗口")
            {
                if (this.MainFrame != null)
                    this.MainFrame.ShowPatientPageForm(e.Param);
            }
            if (e.Name == "打开病人同床婴儿窗口")
            {
                if (!RightController.Instance.UserRight.BabyRecVitalSign.Value)
                {
                    MessageBoxEx.Show("婴儿体征录入权限不够");
                    return;
                }
                if (this.MainFrame != null)
                {
                    string szPatientID = null, szVisitID = null;
                    string szBaby = null, szSubID = null;
                    string szBirthTime = null;
                    if (e.Param is string)
                    {
                        string[] arrParams = e.Param.ToString().Split(';');
                        szPatientID = (arrParams.Length > 0) ? arrParams[0] : string.Empty;
                        szVisitID = (arrParams.Length > 1) ? arrParams[1] : string.Empty;
                        szBaby = (arrParams.Length > 2) ? arrParams[2] : string.Empty;
                        szSubID = (arrParams.Length > 3) ? arrParams[3] : string.Empty;
                        szBirthTime = (arrParams.Length > 4) ? arrParams[4] : string.Empty;
                    }
                    PatVisitInfo patVisitInfo = null;
                    short shRet = PatVisitService.Instance.GetPatVisitInfo(szPatientID, szVisitID, ref patVisitInfo);
                    if (shRet == SystemConst.ReturnValue.FAILED)
                        MessageBoxEx.ShowErrorFormat("没有找到病历号={0},就诊号={1}的病人!", null, szPatientID, szVisitID);
                    else if (shRet != SystemConst.ReturnValue.OK)
                        MessageBoxEx.ShowErrorFormat("检索病历号={0},就诊号={1}的病人时失败!", null, szPatientID, szVisitID);
                    if (patVisitInfo == null)
                        return;
                    if (szBirthTime == null || szBirthTime == string.Empty)
                        return;
                    patVisitInfo.BirthTime = DateTime.Parse(szBirthTime.ToString());
                    patVisitInfo.VisitTime = patVisitInfo.BirthTime;
                    patVisitInfo.SubID = szSubID;
                    patVisitInfo.PatientName = string.Format("{0}{1}", patVisitInfo.PatientName, szBaby);
                    patVisitInfo.PatientID = string.Format("{0}_{1}", patVisitInfo.PatientID, patVisitInfo.SubID);
                    if (szBaby.Contains("子"))
                        patVisitInfo.PatientSex = ServerData.PatientSex.Male;
                    else
                        patVisitInfo.PatientSex = ServerData.PatientSex.Female;
                    this.MainFrame.ShowPatientPageForm(patVisitInfo);
                }
            }
        }
    }
}