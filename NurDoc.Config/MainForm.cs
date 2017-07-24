// ***********************************************************
// 护理病历配置管理系统,系统主窗口.
// Author : YangMingkun, Date : 2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Forms.Loader;
using Heren.Common.Forms.Runtime;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Config.DockForms;
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Config.Report;
using Heren.NurDoc.Config.Templet;
using Heren.NurDoc.Config.FindReplace;

namespace Heren.NurDoc.Config
{
    public partial class MainForm : HerenForm
    {
        private ToolboxListForm m_ToolboxListForm = null;
        private TempletTreeForm m_TempletTreeForm = null;
        private ReportTreeForm m_ReportTreeForm = null;
        private PropertyEditForm m_PropertyEditForm = null;
        private ErrorsListForm m_ErrorsListForm = null;
        private FindReplaceForm m_FindReplaceForm = null;
        private FindResultForm m_FindResultForm = null;
        private ConfigDictForm m_ConfigDictForm = null;
        private CommonDictForm m_CommonDictForm = null;
        private RightManageForm m_RightManageForm = null;
        private UserGroupEditForm m_UserGroupEditForm = null;
        private SchemaManageForm m_NurRecSchemaManageForm = null;
        private SchemaManageForm m_BatchSchemaManageForm = null;
        private SchemaManageForm m_NurApplyManageForm = null;
        private SchemaManageForm m_NurCarePlanManageForm = null;
        private ShiftItemEditForm m_ShiftItemEditForm = null;
        private ShiftRankEditForm m_ShiftRankEditForm = null;
        private ShiftConfigEditForm m_ShiftConfigEditForm = null;
        private CarePlanEditForm m_CarePlanEditForm = null;
        private EvaluationTableManagementForm m_EvaluationTableManagementForm = null;

        #region"属性"
        /// <summary>
        /// 获取当活动的可停靠窗口
        /// </summary>
        [Description("获取当活动的可停靠窗口")]
        internal IDockContent ActiveContent
        {
            get { return this.dockPanel1.ActiveContent; }
        }

        /// <summary>
        /// 获取当活动的文档窗口
        /// </summary>
        [Description("获取当活动的文档窗口")]
        internal IDockContent ActiveDocument
        {
            get { return this.dockPanel1.ActiveDocument; }
        }

        /// <summary>
        /// 获取所有可停靠窗口的列表
        /// </summary>
        [Description("获取所有可停靠窗口的列表")]
        internal DockContentCollection Contents
        {
            get { return this.dockPanel1.Contents; }
        }

        /// <summary>
        /// 获取所有文档窗口的列表
        /// </summary>
        [Description("获取所有文档窗口的列表")]
        internal IDockContent[] Documents
        {
            get { return this.dockPanel1.DocumentsToArray(); }
        }

        /// <summary>
        /// 获取当活动的设计器窗口
        /// </summary>
        [Description("获取当活动的设计器窗口")]
        internal IDesignEditForm ActiveDesign
        {
            get { return this.dockPanel1.ActiveDocument as IDesignEditForm; }
        }

        /// <summary>
        /// 获取当活动的脚本编辑窗口
        /// </summary>
        [Description("获取当活动的脚本编辑窗口")]
        internal IScriptEditForm ActiveScript
        {
            get { return this.dockPanel1.ActiveDocument as IScriptEditForm; }
        }
        #endregion

        #region"事件"
        /// <summary>
        /// 当活动的文档窗口改变活动状态时触发
        /// </summary>
        [Description("当活动的文档窗口改变活动状态时触发")]
        internal event EventHandler ActiveDocumentChanged;

        internal virtual void OnActiveDocumentChanged(EventArgs e)
        {
            if (this.ActiveDocumentChanged == null)
                return;
            try
            {
                this.ActiveDocumentChanged(this, e);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("MainForm.OnActiveDocumentChanged", ex);
            }
        }

        /// <summary>
        /// 当活动的停靠窗口改变活动状态时触发
        /// </summary>
        [Description("当活动的停靠窗口改变活动状态时触发")]
        internal event EventHandler ActiveContentChanged;

        internal virtual void OnActiveContentChanged(EventArgs e)
        {
            if (this.ActiveContentChanged == null)
                return;
            try
            {
                this.ActiveContentChanged(this, e);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("MainForm.OnActiveContentChanged", ex);
            }
        }

        private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            this.OnActiveDocumentChanged(e);
        }
        #endregion

        #region"初始化"
        public MainForm()
        {
            this.InitializeComponent();
            this.dockPanel1.ShowDocumentSubhead = true;
            ReportHandler.Instance.InitReportHandler(this);
            TempletHandler.Instance.InitTempletHandler(this);
            this.Text = string.Format("{0}配置管理中心", SystemContext.Instance.SystemName);
        }

        protected override void OnLoad(EventArgs e)
        {
            SystemLoginForm frmSystemLogin = new SystemLoginForm();
            if (frmSystemLogin.ShowDialog(this) != DialogResult.OK)
            {
                this.Close();
                return;
            }
            base.OnLoad(e);
            this.RestoreWindowState();
            this.Icon = Heren.NurDoc.Config.Properties.Resources.SysIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //设置为自动保存窗口状态
            this.SaveWindowState = true;

            //读取界面布局配置
            this.LoadLayoutConfig();
            this.ShowStatusMessage("已就绪!");
        }

        /// <summary>
        /// 加载DockPanel界面布局配置文件
        /// </summary>
        private void LoadLayoutConfig()
        {
            string szFileName = GlobalMethods.Misc.GetWorkingPath()
                + "\\UserData\\NdsConfig.Dock.config";
            DeserializeDockContent deserializeDockContent =
                new DeserializeDockContent(this.GetContentFromPersistString);
            try
            {
                if (System.IO.File.Exists(szFileName))
                    this.dockPanel1.LoadFromXml(szFileName, deserializeDockContent);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("MainForm.LoadLayoutConfig"
                    , new string[] { "szFileName" }, new object[] { szFileName }, ex);
            }
        }

        /// <summary>
        /// DockPanel组件初始化各停靠窗口时,回调函数实现
        /// </summary>
        /// <param name="szPersistString">停靠窗口类型全名字符串</param>
        /// <returns>IDockContent</returns>
        private IDockContent GetContentFromPersistString(string szPersistString)
        {
            if (szPersistString == typeof(FindReplaceForm).ToString())
            {
                if (this.m_FindReplaceForm == null || this.m_FindReplaceForm.IsDisposed)
                    this.m_FindReplaceForm = new FindReplaceForm(this);
                return this.m_FindReplaceForm;
            }
            if (szPersistString == typeof(FindResultForm).ToString())
            {
                if (this.m_FindResultForm == null || this.m_FindResultForm.IsDisposed)
                    this.m_FindResultForm = new FindResultForm(this);
                return this.m_FindResultForm;
            }
            if (szPersistString == typeof(ToolboxListForm).ToString())
            {
                if (this.m_ToolboxListForm == null || this.m_ToolboxListForm.IsDisposed)
                    this.m_ToolboxListForm = new ToolboxListForm(this);
                return this.m_ToolboxListForm;
            }
            if (szPersistString == typeof(PropertyEditForm).ToString())
            {
                if (this.m_PropertyEditForm == null || this.m_PropertyEditForm.IsDisposed)
                    this.m_PropertyEditForm = new PropertyEditForm(this);
                return this.m_PropertyEditForm;
            }
            if (szPersistString == typeof(ReportTreeForm).ToString())
            {
                if (this.m_ReportTreeForm == null || this.m_ReportTreeForm.IsDisposed)
                    this.m_ReportTreeForm = new ReportTreeForm(this);
                return this.m_ReportTreeForm;
            }
            if (szPersistString == typeof(TempletTreeForm).ToString())
            {
                if (this.m_TempletTreeForm == null || this.m_TempletTreeForm.IsDisposed)
                    this.m_TempletTreeForm = new TempletTreeForm(this);
                return this.m_TempletTreeForm;
            }
            return null;
        }
        #endregion

        #region"工具栏"
        private void toolbtnTempletManage_Click(object sender, EventArgs e)
        {
            this.ShowTempletTreeForm();
        }

        private void toolbtnReportManage_Click(object sender, EventArgs e)
        {
            this.ShowReportTreeForm();
        }

        private void toolmnuShiftRank_Click(object sender, EventArgs e)
        {
            this.ShowShiftRankEditForm();
        }

        private void toolmnuShiftItem_Click(object sender, EventArgs e)
        {
            this.ShowShiftItemEditForm();
        }

        private void toolmnuShiftConfig_Click(object sender, EventArgs e)
        {
            this.ShowShiftConfigEditForm();
        }

        private void toolmnuNCPDict_Click(object sender, EventArgs e)
        {
            this.ShowCarePlanEditForm();
        }

        private void toolmnuNCPGridView_Click(object sender, EventArgs e)
        {
            this.ShowNurCarePlanManageForm();
        }

        private void toolmnuNursingRecManage_Click(object sender, EventArgs e)
        {
            this.ShowNurRecSchemaManageForm();
        }

        private void toolmnuBatchRecManage_Click(object sender, EventArgs e)
        {
            this.ShowBatchSchemaManageForm();
        }

        private void toolmnuNursingApplyManage_Click(object sender, EventArgs e)
        {
            this.ShowNurApplyManageForm();
        }

        private void toolbtnConfigManage_Click(object sender, EventArgs e)
        {
            this.ShowConfigManageForm();
        }

        private void toolbtnCommonDictManage_Click(object sender, EventArgs e)
        {
            this.ShowCommonDictManageForm();
        }

        private void toolmnuUserGroup_Click(object sender, EventArgs e)
        {
            this.ShowUserGroupEditForm();
        }

        private void toolmnuUserRight_Click(object sender, EventArgs e)
        {
            this.ShowRightManageForm();
        }

        private void toolbtnModifyPwd_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            (new ModifyPwdForm()).ShowDialog();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolmnuEvaluationTableManagement_Click(object sender, EventArgs e)
        {
            this.ShowEvaluationTableManagementForm();
        }
        #endregion

        protected override bool ProcessDialogKey(Keys keyData)
        {
            //按F1打开系统帮助文档
            if (keyData == Keys.F1)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.Update();

                //打开用户手册(chm文档)
                string szHelpFile = string.Format("{0}\\护理电子病历系统.chm", GlobalMethods.Misc.GetWorkingPath());
                try
                {
                    if (System.IO.File.Exists(szHelpFile))
                        System.Diagnostics.Process.Start(szHelpFile);
                    else
                        MessageBoxEx.Show("未找到系统帮助手册!");
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("MainForm.ShowSysHelpForm", ex);
                    MessageBoxEx.Show("无法打开帮助手册文件!", ex.Message);
                }
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// 显示控件工具箱窗口
        /// </summary>
        internal void ShowToolboxListForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_ToolboxListForm == null || this.m_ToolboxListForm.IsDisposed)
                this.m_ToolboxListForm = new ToolboxListForm(this);
            this.m_ToolboxListForm.Show(this.dockPanel1);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示表单模板管理窗口
        /// </summary>
        internal void ShowTempletTreeForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_TempletTreeForm == null || this.m_TempletTreeForm.IsDisposed)
                this.m_TempletTreeForm = new TempletTreeForm(this);
            this.m_TempletTreeForm.Show(this.dockPanel1);
            this.m_TempletTreeForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示报表模板管理窗口
        /// </summary>
        internal void ShowReportTreeForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_ReportTreeForm == null || this.m_ReportTreeForm.IsDisposed)
                this.m_ReportTreeForm = new ReportTreeForm(this);
            this.m_ReportTreeForm.Show(this.dockPanel1);
            this.m_ReportTreeForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示脚本文本查找替换窗口
        /// </summary>
        internal void ShowFindReplaceForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_FindReplaceForm == null || this.m_FindReplaceForm.IsDisposed)
                this.m_FindReplaceForm = new FindReplaceForm(this);
            this.m_FindReplaceForm.Show(this.dockPanel1);
            if (this.ActiveScript != null)
                this.m_FindReplaceForm.DefaultFindText = this.ActiveScript.GetSelectedText();
            this.m_FindReplaceForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示脚本文本查找结果窗口
        /// </summary>
        /// <param name="scriptEditForm">脚本编辑器窗口</param>
        /// <param name="szFindText">查找文本</param>
        /// <param name="results">查找结果集</param>
        /// <param name="bshowFileName">设置是否显示文件名列</param>
        internal void ShowFindResultForm(IScriptEditForm scriptEditForm
            , string szFindText, List<FindResult> results, bool bshowFileName)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_FindResultForm == null || this.m_FindResultForm.IsDisposed)
                this.m_FindResultForm = new FindResultForm(this);
            this.m_FindResultForm.ScriptEditForm = scriptEditForm;
            this.m_FindResultForm.FindText = szFindText;
            this.m_FindResultForm.Results = results;
            
            this.m_FindResultForm.Show(this.dockPanel1);
            this.m_FindResultForm.ShowFileName = bshowFileName;
            this.m_FindResultForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示对象属性编辑窗口
        /// </summary>
        internal void ShowPropertyEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_PropertyEditForm == null || this.m_PropertyEditForm.IsDisposed)
                this.m_PropertyEditForm = new PropertyEditForm(this);
            this.m_PropertyEditForm.Show(this.dockPanel1);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 打开表单或报表设计器窗口
        /// </summary>
        /// <param name="designEditForm">设计器窗口</param>
        internal void OpenDesignEditForm(IDesignEditForm designEditForm)
        {
            if (designEditForm == null || designEditForm.IsDisposed)
                return;
            this.dockPanel1.Update();
            IDockContent content = designEditForm as IDockContent;
            if (content != null)
                content.DockHandler.Show(this.dockPanel1);
        }

        /// <summary>
        /// 打开表单或报表脚本编辑器窗口
        /// </summary>
        /// <param name="scriptEditForm">脚本编辑器窗口</param>
        internal void OpenScriptEditForm(IScriptEditForm scriptEditForm)
        {
            if (scriptEditForm == null || scriptEditForm.IsDisposed)
                return;
            this.dockPanel1.Update();
            IDockContent content = scriptEditForm as IDockContent;
            if (content != null)
                content.DockHandler.Show(this.dockPanel1);
        }

        /// <summary>
        /// 显示脚本编译错误列表窗口
        /// </summary>
        internal void ShowCompileErrorForm(ErrorsListForm.CompileError[] errors)
        {
            if (this.m_ErrorsListForm != null && !this.m_ErrorsListForm.IsDisposed)
                this.m_ErrorsListForm.Close();
            if (errors == null || errors.Length <= 0)
                return;
            if (this.m_ErrorsListForm == null || this.m_ErrorsListForm.IsDisposed)
            {
                this.m_ErrorsListForm = new ErrorsListForm(this);
                this.m_ErrorsListForm.Show(this.dockPanel1, DockState.DockBottom);
            }
            this.m_ErrorsListForm.Activate();
            this.m_ErrorsListForm.CompileErrors = errors;
            this.m_ErrorsListForm.ScriptEditForm = this.ActiveScript;
            this.m_ErrorsListForm.OnRefreshView();
        }

        /// <summary>
        /// 显示护理计划字典配置窗口
        /// </summary>
        internal void ShowCarePlanEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_CarePlanEditForm != null && !this.m_CarePlanEditForm.IsDisposed)
            {
                this.m_CarePlanEditForm.Activate();
            }
            else
            {
                this.m_CarePlanEditForm = new CarePlanEditForm(this);
                this.m_CarePlanEditForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_CarePlanEditForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示配置字典表配置窗口
        /// </summary>
        internal void ShowConfigManageForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_ConfigDictForm != null && !this.m_ConfigDictForm.IsDisposed)
            {
                this.m_ConfigDictForm.Activate();
            }
            else
            {
                this.m_ConfigDictForm = new ConfigDictForm(this);
                this.m_ConfigDictForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_ConfigDictForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示公共字典表配置配置窗口
        /// </summary>
        internal void ShowCommonDictManageForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_CommonDictForm != null && !this.m_CommonDictForm.IsDisposed)
            {
                this.m_CommonDictForm.Activate();
            }
            else
            {
                this.m_CommonDictForm = new CommonDictForm(this);
                this.m_CommonDictForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_CommonDictForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示用户组编辑窗口
        /// </summary>
        internal void ShowUserGroupEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_UserGroupEditForm != null && !this.m_UserGroupEditForm.IsDisposed)
            {
                this.m_UserGroupEditForm.Activate();
            }
            else
            {
                this.m_UserGroupEditForm = new UserGroupEditForm(this);
                this.m_UserGroupEditForm.Show(this.dockPanel1);
            }
            this.m_UserGroupEditForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示用户权限管理窗口
        /// </summary>
        internal void ShowRightManageForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_RightManageForm != null && !this.m_RightManageForm.IsDisposed)
            {
                this.m_RightManageForm.Activate();
            }
            else
            {
                this.m_RightManageForm = new RightManageForm(this, UserRightType.NurDoc);
                this.m_RightManageForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_RightManageForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示交班项目配置管理窗口
        /// </summary>
        internal void ShowShiftItemEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_ShiftItemEditForm != null && !this.m_ShiftItemEditForm.IsDisposed)
            {
                this.m_ShiftItemEditForm.Activate();
            }
            else
            {
                this.m_ShiftItemEditForm = new ShiftItemEditForm(this);
                this.m_ShiftItemEditForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_ShiftItemEditForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示交班班次配置管理窗口
        /// </summary>
        internal void ShowShiftRankEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_ShiftRankEditForm != null && !this.m_ShiftRankEditForm.IsDisposed)
            {
                this.m_ShiftRankEditForm.Activate();
            }
            else
            {
                this.m_ShiftRankEditForm = new ShiftRankEditForm(this);
                this.m_ShiftRankEditForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_ShiftRankEditForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示交班动态配置管理窗口
        /// </summary>
        internal void ShowShiftConfigEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_ShiftConfigEditForm != null && !this.m_ShiftConfigEditForm.IsDisposed)
            {
                this.m_ShiftConfigEditForm.Activate();
            }
            else
            {
                this.m_ShiftConfigEditForm = new ShiftConfigEditForm(this);
                this.m_ShiftConfigEditForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_ShiftConfigEditForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示护理记录列方案管理窗口
        /// </summary>
        internal void ShowNurRecSchemaManageForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_NurRecSchemaManageForm != null && !this.m_NurRecSchemaManageForm.IsDisposed)
            {
                this.m_NurRecSchemaManageForm.Activate();
            }
            else
            {
                string szSchemaType = ServerData.GridViewSchema.NURSING_RECORD;
                this.m_NurRecSchemaManageForm = new SchemaManageForm(this, szSchemaType);
                this.m_NurRecSchemaManageForm.Text = "护理记录表格列配置";
                this.m_NurRecSchemaManageForm.TabText = this.m_NurRecSchemaManageForm.Text;
                this.m_NurRecSchemaManageForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_NurRecSchemaManageForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示批量录入模板管理窗口
        /// </summary>
        internal void ShowBatchSchemaManageForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_BatchSchemaManageForm != null && !this.m_BatchSchemaManageForm.IsDisposed)
            {
                this.m_BatchSchemaManageForm.Activate();
            }
            else
            {
                string szSchemaType = ServerData.GridViewSchema.BATCH_RECORD;
                this.m_BatchSchemaManageForm = new SchemaManageForm(this, szSchemaType);
                this.m_BatchSchemaManageForm.Text = "批量录入表格列配置";
                this.m_BatchSchemaManageForm.TabText = this.m_BatchSchemaManageForm.Text;
                this.m_BatchSchemaManageForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_BatchSchemaManageForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示护理申请列管理窗口
        /// </summary>
        internal void ShowNurApplyManageForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_NurApplyManageForm != null && !this.m_NurApplyManageForm.IsDisposed)
            {
                this.m_NurApplyManageForm.Activate();
            }
            else
            {
                string szSchemaType = ServerData.GridViewSchema.NURSING_APPLY;
                this.m_NurApplyManageForm = new SchemaManageForm(this, szSchemaType);
                this.m_NurApplyManageForm.Text = "护理申请表格列配置";
                this.m_NurApplyManageForm.TabText = this.m_NurApplyManageForm.Text;
                this.m_NurApplyManageForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_NurApplyManageForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 显示护理计划列管理窗口
        /// </summary>
        internal void ShowNurCarePlanManageForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_NurCarePlanManageForm != null && !this.m_NurCarePlanManageForm.IsDisposed)
            {
                this.m_NurCarePlanManageForm.Activate();
            }
            else
            {
                string szSchemaType = ServerData.GridViewSchema.NURSING_CARE_PLAN;
                this.m_NurCarePlanManageForm = new SchemaManageForm(this, szSchemaType);
                this.m_NurCarePlanManageForm.Text = "护理计划表格列配置";
                this.m_NurCarePlanManageForm.TabText = this.m_NurCarePlanManageForm.Text;
                this.m_NurCarePlanManageForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_NurCarePlanManageForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        internal void ShowEvaluationTableManagementForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_EvaluationTableManagementForm != null 
                && !this.m_EvaluationTableManagementForm.IsDisposed)
            {
                this.m_EvaluationTableManagementForm.Activate();
            }
            else
            {
                this.m_EvaluationTableManagementForm = new EvaluationTableManagementForm(this);
                this.m_EvaluationTableManagementForm.Text = "护理评价表配置";
                this.m_EvaluationTableManagementForm.TabText = this.m_EvaluationTableManagementForm.Text;
                this.m_EvaluationTableManagementForm.Show(this.dockPanel1, DockState.Document);
            }
            this.m_EvaluationTableManagementForm.OnRefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public void ShowStatusMessage(string szMessage)
        {
            this.tsslblSystemStatus.Text = szMessage;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            //如果SaveWindowState值为false,说明未登录
            if (!this.SaveWindowState)
                return;

            //检查并提示是否需要保存
            int index = 0;
            DockContentBase content = null;
            while (index < this.dockPanel1.Contents.Count)
            {
                content = this.dockPanel1.Contents[index] as DockContentBase;
                index++;
                if (content == null || content.IsDisposed)
                    continue;
                e.Cancel = !content.CheckModifiedData();
                if (e.Cancel) return;
            }

            //隐藏所有停靠窗口,以便下次启动后不会自动显示
            index = 0;
            while (index < this.dockPanel1.Contents.Count)
            {
                content = this.dockPanel1.Contents[index] as DockContentBase;
                index++;
                if (content != null && !content.IsDisposed)
                    content.Hide();
            }
            try
            {
                this.dockPanel1.SaveAsXml(GlobalMethods.Misc.GetWorkingPath()
                    + "\\UserData\\NdsConfig.Dock.config");
            }
            catch { }

            //删除临时缓存目录和文件
            GlobalMethods.IO.DeleteDirectory(GlobalMethods.Misc.GetWorkingPath() + "\\Cache", true);
        }

    }
}
