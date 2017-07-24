// ***********************************************************
// 护理病历配置管理系统,脚本编辑器查找替换窗口.
// Author : YangMingkun, Date : 2013-5-4
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Config.DockForms;
using Heren.NurDoc.Config.Templet;
using Heren.NurDoc.Config.Report;

namespace Heren.NurDoc.Config.FindReplace
{
    internal partial class FindResultForm : DockContentBase
    {
        private IScriptEditForm m_ScriptEditForm = null;

        /// <summary>
        /// 获取或设置查找的目标脚本窗口
        /// </summary>
        public IScriptEditForm ScriptEditForm
        {
            get { return this.m_ScriptEditForm; }
            set { this.m_ScriptEditForm = value; }
        }

        private string m_findText = string.Empty;

        /// <summary>
        /// 获取或设置查找的原始文本
        /// </summary>
        public string FindText
        {
            get { return this.m_findText; }
            set { this.m_findText = value; }
        }

        private List<FindResult> m_results = null;

        /// <summary>
        /// 获取或设置查找到的结果集列表
        /// </summary>
        public List<FindResult> Results
        {
            get { return this.m_results; }
            set { this.m_results = value; }
        }

        private bool m_showFileName = false;

        public bool ShowFileName
        {
            get
            {
                return this.m_showFileName;
            }

            set
            {
                this.m_showFileName = value;
                if (m_showFileName)
                {
                    this.colFileName.Visible = true;
                }
                else
                {
                    this.colFileName.Visible = false;
                }
            }
        }

        public FindResultForm(MainForm mainForm)
            : base(mainForm)
        {
            this.HideOnClose = true;
            this.ShowHint = DockState.DockBottom;
            this.DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                | DockAreas.DockTop | DockAreas.DockBottom;
        }

        //对于需要记忆位置的停靠窗口,请将控件创建代码放入Load事件内
        //这样当窗口被构造时,就不会加载界面元素,用以提高系统启动速度
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.UpdateBounds();
            this.Icon = Heren.NurDoc.Config.Properties.Resources.FindIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            this.OnRefreshView();
        }

        protected override void OnActiveDocumentChanged()
        {
            this.OnRefreshView();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F3)
                this.RefreshResultList();
            return base.ProcessDialogKey(keyData);
        }

        public override void OnRefreshView()
        {
            this.dataTableView1.Rows.Clear();
            this.Update();
            if (this.m_results == null || this.m_results.Count <= 0)
                return;
            foreach (FindResult result in this.m_results)
            {
                int rowIndex = this.dataTableView1.Rows.Add();
                DataGridViewRow row = this.dataTableView1.Rows[rowIndex];
                row.Tag = result;
                row.Cells[this.colNo.Index].Value = rowIndex + 1;
                row.Cells[this.colContent.Index].Value = result.Text;
                row.Cells[this.colLineNo.Index].Value = result.Line + 1;
                row.Cells[this.colFileName.Index].Value = result.TempletName;
            }
        }

        private void RefreshResultList()
        {
            int selectedIndex = 0;
            if (this.dataTableView1.SelectedRows.Count > 0)
                selectedIndex = this.dataTableView1.SelectedRows[0].Index;

            if (this.ScriptEditForm != null)
                this.ScriptEditForm.FindText(this.m_findText, false);
            this.dataTableView1.SelectRow(selectedIndex);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
            if (row == null || row.Index < 0)
                return;
            FindResult result = row.Tag as FindResult;

            if (result.FileType == ServerData.FileType.TEMPLET)
            {
                DocTypeInfo DocTypeInfo = new DocTypeInfo();
                DocTypeInfo.DocTypeID = result.TempletID;
                DocTypeInfo.DocTypeName = result.TempletName;
                Heren.NurDoc.Config.Templet.DesignEditForm designEditForm = TempletHandler.Instance.GetDesignForm(DocTypeInfo);
                this.m_ScriptEditForm = TempletHandler.Instance.GetScriptForm(designEditForm);
                if (this.m_ScriptEditForm == null)
                {
                    TempletHandler.Instance.OpenTemplet(DocTypeInfo);
                    designEditForm = TempletHandler.Instance.GetDesignForm(DocTypeInfo);
                    Application.DoEvents();
                    TempletHandler.Instance.OpenScriptEditForm(designEditForm);
                    this.m_ScriptEditForm = TempletHandler.Instance.GetScriptForm(designEditForm);
                    Application.DoEvents();
                }
            }
            else if (result.FileType == ServerData.FileType.REPORT)
            {
                ReportTypeInfo ReportType = new ReportTypeInfo();
                ReportType.ReportTypeID = result.TempletID;
                ReportType.ReportTypeName = result.TempletName;
                NurDoc.Config.Report.DesignEditForm reportDesignEditForm = ReportHandler.Instance.GetDesignForm(ReportType);
                this.m_ScriptEditForm = ReportHandler.Instance.GetScriptForm(reportDesignEditForm);
                if (this.m_ScriptEditForm == null)
                {
                    ReportHandler.Instance.OpenReport(ReportType);
                    reportDesignEditForm = ReportHandler.Instance.GetDesignForm(ReportType);
                    Application.DoEvents();
                    ReportHandler.Instance.OpenScriptEditForm(reportDesignEditForm);
                    this.m_ScriptEditForm = ReportHandler.Instance.GetScriptForm(reportDesignEditForm);
                    Application.DoEvents();
                }
            }

            if (result == null)
                return;
            if (this.m_ScriptEditForm != null && !this.m_ScriptEditForm.IsDisposed)
                this.m_ScriptEditForm.LocateToText(result.Offset, result.Length);
            this.dataTableView1.SelectRow(e.RowIndex);
        }
    }
}
