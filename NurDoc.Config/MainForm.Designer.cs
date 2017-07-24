namespace Heren.NurDoc.Config
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtnTempletManage = new System.Windows.Forms.ToolStripButton();
            this.toolbtnReportManage = new System.Windows.Forms.ToolStripButton();
            this.toolbtnShiftConfig = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuShiftRank = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuShiftItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuShiftConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnNCPManage = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuNCPDict = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuNCPGridView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnGridViewManage = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuBatchRecManage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuNursingRecManage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuNursingApplyManage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnConfigManage = new System.Windows.Forms.ToolStripButton();
            this.toolbtnCommonDictManage = new System.Windows.Forms.ToolStripButton();
            this.toolbtnRightManage = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuUserGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuUserRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuEvaluationTableManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbtnModifyPwd = new System.Windows.Forms.ToolStripButton();
            this.toolbtnExit = new System.Windows.Forms.ToolStripButton();
            this.dockPanel1 = new Heren.Common.DockSuite.DockPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslblSystemStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblCopyright = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtnTempletManage,
            this.toolbtnReportManage,
            this.toolbtnShiftConfig,
            this.toolbtnNCPManage,
            this.toolbtnGridViewManage,
            this.toolbtnConfigManage,
            this.toolbtnCommonDictManage,
            this.toolbtnRightManage,
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.toolbtnModifyPwd,
            this.toolbtnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(956, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolbtnTempletManage
            // 
            this.toolbtnTempletManage.AutoSize = false;
            this.toolbtnTempletManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnTempletManage.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnTempletManage.Image")));
            this.toolbtnTempletManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnTempletManage.Name = "toolbtnTempletManage";
            this.toolbtnTempletManage.Size = new System.Drawing.Size(88, 22);
            this.toolbtnTempletManage.Text = "表单模板管理";
            this.toolbtnTempletManage.Click += new System.EventHandler(this.toolbtnTempletManage_Click);
            // 
            // toolbtnReportManage
            // 
            this.toolbtnReportManage.AutoSize = false;
            this.toolbtnReportManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnReportManage.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnReportManage.Image")));
            this.toolbtnReportManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnReportManage.Name = "toolbtnReportManage";
            this.toolbtnReportManage.Size = new System.Drawing.Size(88, 22);
            this.toolbtnReportManage.Text = "报表模板管理";
            this.toolbtnReportManage.Click += new System.EventHandler(this.toolbtnReportManage_Click);
            // 
            // toolbtnShiftConfig
            // 
            this.toolbtnShiftConfig.AutoSize = false;
            this.toolbtnShiftConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnShiftConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuShiftRank,
            this.toolmnuShiftItem,
            this.toolmnuShiftConfig});
            this.toolbtnShiftConfig.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnShiftConfig.Image")));
            this.toolbtnShiftConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnShiftConfig.Name = "toolbtnShiftConfig";
            this.toolbtnShiftConfig.Size = new System.Drawing.Size(100, 22);
            this.toolbtnShiftConfig.Text = "护理交班配置";
            // 
            // toolmnuShiftRank
            // 
            this.toolmnuShiftRank.Name = "toolmnuShiftRank";
            this.toolmnuShiftRank.Size = new System.Drawing.Size(152, 22);
            this.toolmnuShiftRank.Text = "交班班次配置";
            this.toolmnuShiftRank.Click += new System.EventHandler(this.toolmnuShiftRank_Click);
            // 
            // toolmnuShiftItem
            // 
            this.toolmnuShiftItem.Name = "toolmnuShiftItem";
            this.toolmnuShiftItem.Size = new System.Drawing.Size(152, 22);
            this.toolmnuShiftItem.Text = "交班项目配置";
            this.toolmnuShiftItem.Click += new System.EventHandler(this.toolmnuShiftItem_Click);
            // 
            // toolmnuShiftConfig
            // 
            this.toolmnuShiftConfig.Name = "toolmnuShiftConfig";
            this.toolmnuShiftConfig.Size = new System.Drawing.Size(152, 22);
            this.toolmnuShiftConfig.Text = "交班动态配置";
            this.toolmnuShiftConfig.Click += new System.EventHandler(this.toolmnuShiftConfig_Click);
            // 
            // toolbtnNCPManage
            // 
            this.toolbtnNCPManage.AutoSize = false;
            this.toolbtnNCPManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnNCPManage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuNCPDict,
            this.toolmnuNCPGridView});
            this.toolbtnNCPManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNCPManage.Name = "toolbtnNCPManage";
            this.toolbtnNCPManage.Size = new System.Drawing.Size(100, 22);
            this.toolbtnNCPManage.Text = "护理计划配置";
            // 
            // toolmnuNCPDict
            // 
            this.toolmnuNCPDict.Name = "toolmnuNCPDict";
            this.toolmnuNCPDict.Size = new System.Drawing.Size(160, 22);
            this.toolmnuNCPDict.Text = "护理计划字典";
            this.toolmnuNCPDict.Click += new System.EventHandler(this.toolmnuNCPDict_Click);
            // 
            // toolmnuNCPGridView
            // 
            this.toolmnuNCPGridView.Name = "toolmnuNCPGridView";
            this.toolmnuNCPGridView.Size = new System.Drawing.Size(160, 22);
            this.toolmnuNCPGridView.Text = "护理计划表格列";
            this.toolmnuNCPGridView.Click += new System.EventHandler(this.toolmnuNCPGridView_Click);
            // 
            // toolbtnGridViewManage
            // 
            this.toolbtnGridViewManage.AutoSize = false;
            this.toolbtnGridViewManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnGridViewManage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuBatchRecManage,
            this.toolmnuNursingRecManage,
            this.toolmnuNursingApplyManage});
            this.toolbtnGridViewManage.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnGridViewManage.Image")));
            this.toolbtnGridViewManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnGridViewManage.Name = "toolbtnGridViewManage";
            this.toolbtnGridViewManage.Size = new System.Drawing.Size(88, 22);
            this.toolbtnGridViewManage.Text = "表格列配置";
            // 
            // toolmnuBatchRecManage
            // 
            this.toolmnuBatchRecManage.Name = "toolmnuBatchRecManage";
            this.toolmnuBatchRecManage.Size = new System.Drawing.Size(148, 22);
            this.toolmnuBatchRecManage.Text = "批量录入配置";
            this.toolmnuBatchRecManage.Click += new System.EventHandler(this.toolmnuBatchRecManage_Click);
            // 
            // toolmnuNursingRecManage
            // 
            this.toolmnuNursingRecManage.Name = "toolmnuNursingRecManage";
            this.toolmnuNursingRecManage.Size = new System.Drawing.Size(148, 22);
            this.toolmnuNursingRecManage.Text = "护理记录配置";
            this.toolmnuNursingRecManage.Click += new System.EventHandler(this.toolmnuNursingRecManage_Click);
            // 
            // toolmnuNursingApplyManage
            // 
            this.toolmnuNursingApplyManage.Name = "toolmnuNursingApplyManage";
            this.toolmnuNursingApplyManage.Size = new System.Drawing.Size(148, 22);
            this.toolmnuNursingApplyManage.Text = "护理申报配置";
            this.toolmnuNursingApplyManage.Click += new System.EventHandler(this.toolmnuNursingApplyManage_Click);
            // 
            // toolbtnConfigManage
            // 
            this.toolbtnConfigManage.AutoSize = false;
            this.toolbtnConfigManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnConfigManage.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnConfigManage.Image")));
            this.toolbtnConfigManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnConfigManage.Name = "toolbtnConfigManage";
            this.toolbtnConfigManage.Size = new System.Drawing.Size(88, 22);
            this.toolbtnConfigManage.Text = "后台配置管理";
            this.toolbtnConfigManage.Click += new System.EventHandler(this.toolbtnConfigManage_Click);
            // 
            // toolbtnCommonDictManage
            // 
            this.toolbtnCommonDictManage.AutoSize = false;
            this.toolbtnCommonDictManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnCommonDictManage.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnCommonDictManage.Image")));
            this.toolbtnCommonDictManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnCommonDictManage.Name = "toolbtnCommonDictManage";
            this.toolbtnCommonDictManage.Size = new System.Drawing.Size(88, 22);
            this.toolbtnCommonDictManage.Text = "公共字典管理";
            this.toolbtnCommonDictManage.Click += new System.EventHandler(this.toolbtnCommonDictManage_Click);
            // 
            // toolbtnRightManage
            // 
            this.toolbtnRightManage.AutoSize = false;
            this.toolbtnRightManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnRightManage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuUserGroup,
            this.toolmnuUserRight});
            this.toolbtnRightManage.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnRightManage.Image")));
            this.toolbtnRightManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnRightManage.Name = "toolbtnRightManage";
            this.toolbtnRightManage.Size = new System.Drawing.Size(100, 22);
            this.toolbtnRightManage.Text = "用户权限管理";
            // 
            // toolmnuUserGroup
            // 
            this.toolmnuUserGroup.Name = "toolmnuUserGroup";
            this.toolmnuUserGroup.Size = new System.Drawing.Size(148, 22);
            this.toolmnuUserGroup.Text = "用户组管理";
            this.toolmnuUserGroup.Click += new System.EventHandler(this.toolmnuUserGroup_Click);
            // 
            // toolmnuUserRight
            // 
            this.toolmnuUserRight.Name = "toolmnuUserRight";
            this.toolmnuUserRight.Size = new System.Drawing.Size(148, 22);
            this.toolmnuUserRight.Text = "用户权限管理";
            this.toolmnuUserRight.Click += new System.EventHandler(this.toolmnuUserRight_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuEvaluationTableManagement});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(69, 24);
            this.toolStripDropDownButton1.Text = "护理质控";
            // 
            // toolmnuEvaluationTableManagement
            // 
            this.toolmnuEvaluationTableManagement.Name = "toolmnuEvaluationTableManagement";
            this.toolmnuEvaluationTableManagement.Size = new System.Drawing.Size(136, 22);
            this.toolmnuEvaluationTableManagement.Text = "评价表管理";
            this.toolmnuEvaluationTableManagement.Click += new System.EventHandler(this.toolmnuEvaluationTableManagement_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolbtnModifyPwd
            // 
            this.toolbtnModifyPwd.AutoSize = false;
            this.toolbtnModifyPwd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnModifyPwd.Name = "toolbtnModifyPwd";
            this.toolbtnModifyPwd.Size = new System.Drawing.Size(60, 22);
            this.toolbtnModifyPwd.Text = "修改口令";
            this.toolbtnModifyPwd.Click += new System.EventHandler(this.toolbtnModifyPwd_Click);
            // 
            // toolbtnExit
            // 
            this.toolbtnExit.AutoSize = false;
            this.toolbtnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnExit.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnExit.Image")));
            this.toolbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnExit.Name = "toolbtnExit";
            this.toolbtnExit.Size = new System.Drawing.Size(64, 22);
            this.toolbtnExit.Text = "退出系统";
            this.toolbtnExit.Click += new System.EventHandler(this.toolbtnExit_Click);
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel1.DocumentStyle = Heren.Common.DockSuite.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 27);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(956, 449);
            this.dockPanel1.TabIndex = 1;
            this.dockPanel1.ActiveDocumentChanged += new System.EventHandler(this.dockPanel1_ActiveDocumentChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslblSystemStatus,
            this.tsslblCopyright});
            this.statusStrip1.Location = new System.Drawing.Point(0, 476);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(956, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslblSystemStatus
            // 
            this.tsslblSystemStatus.Name = "tsslblSystemStatus";
            this.tsslblSystemStatus.Size = new System.Drawing.Size(745, 17);
            this.tsslblSystemStatus.Spring = true;
            this.tsslblSystemStatus.Text = "就绪";
            this.tsslblSystemStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslblCopyright
            // 
            this.tsslblCopyright.Name = "tsslblCopyright";
            this.tsslblCopyright.Size = new System.Drawing.Size(196, 17);
            this.tsslblCopyright.Text = "版权所有(C) 浙江和仁科技有限公司";
            this.tsslblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 498);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "护理电子病历配置管理中心";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolbtnTempletManage;
        private System.Windows.Forms.ToolStripButton toolbtnReportManage;
        private System.Windows.Forms.ToolStripButton toolbtnConfigManage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolbtnModifyPwd;
        private System.Windows.Forms.ToolStripButton toolbtnExit;
        private Heren.Common.DockSuite.DockPanel dockPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslblSystemStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslblCopyright;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnGridViewManage;
        private System.Windows.Forms.ToolStripMenuItem toolmnuNursingRecManage;
        private System.Windows.Forms.ToolStripMenuItem toolmnuBatchRecManage;
        private System.Windows.Forms.ToolStripMenuItem toolmnuNursingApplyManage;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnRightManage;
        private System.Windows.Forms.ToolStripMenuItem toolmnuUserGroup;
        private System.Windows.Forms.ToolStripMenuItem toolmnuUserRight;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnNCPManage;
        private System.Windows.Forms.ToolStripMenuItem toolmnuNCPDict;
        private System.Windows.Forms.ToolStripMenuItem toolmnuNCPGridView;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnShiftConfig;
        private System.Windows.Forms.ToolStripMenuItem toolmnuShiftRank;
        private System.Windows.Forms.ToolStripMenuItem toolmnuShiftItem;
        private System.Windows.Forms.ToolStripButton toolbtnCommonDictManage;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolmnuEvaluationTableManagement;
        private System.Windows.Forms.ToolStripMenuItem toolmnuShiftConfig;
    }
}

