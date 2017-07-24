namespace Heren.NurDoc.PatPage.NurRec
{
    partial class RecordEditForm
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
            this.DisposeFormEditor();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.virtualTree1 = new Heren.Common.Controls.VirtualTreeView.VirtualTree();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCancelAssess = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpAssessTime = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtAssessor = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolbtnNew = new System.Windows.Forms.ToolStripButton();
            this.toolbtnSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPreview = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolbtnReturn = new System.Windows.Forms.ToolStripButton();
            this.toolbtnHistory = new System.Windows.Forms.ToolStripButton();
            this.xPanel1 = new Heren.Common.Forms.XPanel();
            this.arrowSplitter1 = new Heren.Common.Controls.ArrowSplitter();
            this.documentControl1 = new Heren.NurDoc.PatPage.Document.DocumentControl();
            this.contextMenuStrip1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.xPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // virtualTree1
            // 
            this.virtualTree1.ContextMenuStrip = this.contextMenuStrip1;
            this.virtualTree1.Dock = System.Windows.Forms.DockStyle.Left;
            this.virtualTree1.Location = new System.Drawing.Point(2, 2);
            this.virtualTree1.Name = "virtualTree1";
            this.virtualTree1.ShowColumnHeader = false;
            this.virtualTree1.ShowToolTip = false;
            this.virtualTree1.Size = new System.Drawing.Size(188, 499);
            this.virtualTree1.TabIndex = 1;
            this.virtualTree1.NodeMouseClick += new Heren.Common.Controls.VirtualTreeView.VirtualTreeEventHandler(this.virtualTree1_NodeMouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCancelAssess,
            this.toolStripSeparator1,
            this.mnuExpandAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 54);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnuCancelAssess
            // 
            this.mnuCancelAssess.Name = "mnuCancelAssess";
            this.mnuCancelAssess.Size = new System.Drawing.Size(146, 22);
            this.mnuCancelAssess.Text = "取消此项评估";
            this.mnuCancelAssess.Click += new System.EventHandler(this.mnuCancelAssess_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // mnuExpandAll
            // 
            this.mnuExpandAll.Name = "mnuExpandAll";
            this.mnuExpandAll.Size = new System.Drawing.Size(146, 22);
            this.mnuExpandAll.Text = "默认展开所有";
            this.mnuExpandAll.Click += new System.EventHandler(this.mnuExpandAll_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 32);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.documentControl1);
            this.splitContainer2.Size = new System.Drawing.Size(636, 467);
            this.splitContainer2.SplitterDistance = 362;
            this.splitContainer2.TabIndex = 3;
            this.splitContainer2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Paint);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tooldtpAssessTime,
            this.toolStripLabel2,
            this.tooltxtAssessor,
            this.toolStripLabel3,
            this.toolbtnSave,
            this.toolbtnNew,
            this.toolbtnSaveAs,
            this.toolbtnPreview,
            this.toolbtnPrint,
            this.toolbtnReturn,
            this.toolbtnHistory});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(636, 32);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "baseToolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel1.Text = "记录时间：";
            // 
            // tooldtpAssessTime
            // 
            this.tooldtpAssessTime.AutoSize = false;
            this.tooldtpAssessTime.BackColor = System.Drawing.Color.White;
            this.tooldtpAssessTime.Name = "tooldtpAssessTime";
            this.tooldtpAssessTime.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tooldtpAssessTime.ShowSecond = false;
            this.tooldtpAssessTime.Size = new System.Drawing.Size(130, 22);
            this.tooldtpAssessTime.Text = "2012/6/29 16:03:17";
            this.tooldtpAssessTime.Value = new System.DateTime(2012, 6, 29, 16, 3, 17, 0);
            this.tooldtpAssessTime.ValueChanged += new System.EventHandler(this.tooldtpAssessTime_ValueChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabel2.Text = " 记录人：";
            // 
            // tooltxtAssessor
            // 
            this.tooltxtAssessor.AutoSize = false;
            this.tooltxtAssessor.Name = "tooltxtAssessor";
            this.tooltxtAssessor.ReadOnly = true;
            this.tooltxtAssessor.Size = new System.Drawing.Size(64, 28);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.AutoSize = false;
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(11, 22);
            this.toolStripLabel3.Text = " ";
            // 
            // toolbtnSave
            // 
            this.toolbtnSave.AutoSize = false;
            this.toolbtnSave.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Save;
            this.toolbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSave.Name = "toolbtnSave";
            this.toolbtnSave.Size = new System.Drawing.Size(64, 22);
            this.toolbtnSave.Text = "保存";
            this.toolbtnSave.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // toolbtnNew
            // 
            this.toolbtnNew.AutoSize = false;
            this.toolbtnNew.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Add;
            this.toolbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNew.Name = "toolbtnNew";
            this.toolbtnNew.Size = new System.Drawing.Size(64, 22);
            this.toolbtnNew.Text = "新增";
            this.toolbtnNew.Click += new System.EventHandler(this.toolbtnNew_Click);
            // 
            // toolbtnSaveAs
            // 
            this.toolbtnSaveAs.AutoSize = false;
            this.toolbtnSaveAs.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Save_as;
            this.toolbtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSaveAs.Name = "toolbtnSaveAs";
            this.toolbtnSaveAs.Size = new System.Drawing.Size(64, 20);
            this.toolbtnSaveAs.Text = "另存为";
            this.toolbtnSaveAs.Click += new System.EventHandler(this.toolbtnSaveAs_Click);
            // 
            // toolbtnPreview
            // 
            this.toolbtnPreview.AutoSize = false;
            this.toolbtnPreview.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Preview;
            this.toolbtnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnPreview.Name = "toolbtnPreview";
            this.toolbtnPreview.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPreview.Text = "预览";
            this.toolbtnPreview.Click += new System.EventHandler(this.toolbtnPreview_Click);
            // 
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Print;
            this.toolbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnPrint.Name = "toolbtnPrint";
            this.toolbtnPrint.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrint.Text = "打印";
            this.toolbtnPrint.Click += new System.EventHandler(this.toolbtnPrint_Click);
            // 
            // toolbtnReturn
            // 
            this.toolbtnReturn.AutoSize = false;
            this.toolbtnReturn.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Return;
            this.toolbtnReturn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnReturn.Name = "toolbtnReturn";
            this.toolbtnReturn.Size = new System.Drawing.Size(88, 22);
            this.toolbtnReturn.Text = "记录列表";
            this.toolbtnReturn.Click += new System.EventHandler(this.toolbtnReturn_Click);
            // 
            // toolbtnHistory
            // 
            this.toolbtnHistory.AutoSize = false;
            this.toolbtnHistory.Image = global::Heren.NurDoc.PatPage.Properties.Resources.NursingDoc;
            this.toolbtnHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnHistory.Name = "toolbtnHistory";
            this.toolbtnHistory.Size = new System.Drawing.Size(88, 22);
            this.toolbtnHistory.Text = "历史参考";
            this.toolbtnHistory.Click += new System.EventHandler(this.toolbtnHistory_Click);
            // 
            // xPanel1
            // 
            this.xPanel1.Controls.Add(this.splitContainer2);
            this.xPanel1.Controls.Add(this.toolStrip1);
            this.xPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPanel1.Location = new System.Drawing.Point(202, 2);
            this.xPanel1.Name = "xPanel1";
            this.xPanel1.Size = new System.Drawing.Size(636, 499);
            this.xPanel1.TabIndex = 1;
            // 
            // arrowSplitter1
            // 
            this.arrowSplitter1.Location = new System.Drawing.Point(190, 2);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(12, 499);
            this.arrowSplitter1.TabIndex = 2;
            this.arrowSplitter1.TabStop = false;
            // 
            // documentControl1
            // 
            this.documentControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.documentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentControl1.Document = null;
            this.documentControl1.Location = new System.Drawing.Point(0, 0);
            this.documentControl1.Name = "documentControl1";
            this.documentControl1.Size = new System.Drawing.Size(636, 101);
            this.documentControl1.TabIndex = 5;
            this.documentControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.FormEditor_CustomEvent);
            // 
            // RecordEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(840, 503);
            this.Controls.Add(this.xPanel1);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.virtualTree1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "RecordEditForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "护理记录编辑窗口";
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.xPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.VirtualTreeView.VirtualTree virtualTree1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpAssessTime;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox tooltxtAssessor;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.ToolStripButton toolbtnNew;
        private System.Windows.Forms.ToolStripButton toolbtnPrint;
        private System.Windows.Forms.ToolStripButton toolbtnPreview;
        private System.Windows.Forms.ToolStripButton toolbtnReturn;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Heren.NurDoc.PatPage.Document.DocumentControl documentControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelAssess;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuExpandAll;
        private System.Windows.Forms.ToolStripButton toolbtnSaveAs;
        private System.Windows.Forms.ToolStripButton toolbtnHistory;
        private Heren.Common.Forms.XPanel xPanel1;
        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
    }
}