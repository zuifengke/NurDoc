namespace Heren.NurDoc.PatPage.Document
{
    partial class GraphDocumentListForm
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
            this.components = new System.ComponentModel.Container();
            this.cmnenuDocumentList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.reportDesigner2 = new Heren.Common.Report.ReportDesigner();
            this.reportDesigner1 = new Heren.Common.Report.ReportDesigner();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboPatDeptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolbPageIndex = new System.Windows.Forms.ToolStripLabel();
            this.toolcboPageList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnNew = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuPrintAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintContinued = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintAllPage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintPage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnOption = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuShowAsModel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuSavePreView = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnenuDocumentList.SuspendLayout();
            this.pnlGraph.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmnenuDocumentList
            // 
            this.cmnenuDocumentList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuDelete});
            this.cmnenuDocumentList.Name = "contextMenuStrip1";
            this.cmnenuDocumentList.Size = new System.Drawing.Size(101, 48);
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(100, 22);
            this.mnuNew.Text = "新增";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(100, 22);
            this.mnuDelete.Text = "删除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // pnlGraph
            // 
            this.pnlGraph.Controls.Add(this.panel1);
            this.pnlGraph.Controls.Add(this.reportDesigner1);
            this.pnlGraph.Controls.Add(this.toolStrip1);
            this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraph.Location = new System.Drawing.Point(2, 2);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(808, 452);
            this.pnlGraph.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.reportDesigner2);
            this.panel1.Location = new System.Drawing.Point(0, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(793, 100);
            this.panel1.TabIndex = 7;
            // 
            // reportDesigner2
            // 
            this.reportDesigner2.AutoScrollMinSize = new System.Drawing.Size(794, 1123);
            this.reportDesigner2.CanvasOffset = new System.Drawing.Point(0, 0);
            this.reportDesigner2.Enabled = false;
            this.reportDesigner2.Location = new System.Drawing.Point(0, 0);
            this.reportDesigner2.Name = "reportDesigner2";
            this.reportDesigner2.Readonly = true;
            this.reportDesigner2.Size = new System.Drawing.Size(793, 100);
            this.reportDesigner2.TabIndex = 7;
            this.reportDesigner2.Text = "reportDesigner2";
            // 
            // reportDesigner1
            // 
            this.reportDesigner1.AutoScrollMinSize = new System.Drawing.Size(794, 1123);
            this.reportDesigner1.CanvasOffset = new System.Drawing.Point(0, 0);
            this.reportDesigner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportDesigner1.Location = new System.Drawing.Point(0, 32);
            this.reportDesigner1.Name = "reportDesigner1";
            this.reportDesigner1.Readonly = true;
            this.reportDesigner1.Size = new System.Drawing.Size(808, 420);
            this.reportDesigner1.TabIndex = 5;
            this.reportDesigner1.Text = "reportDesigner1";
            this.reportDesigner1.ExecuteQuery += new Heren.Common.Report.ExecuteQueryEventHandler(this.reportDesigner1_ExecuteQuery);
            this.reportDesigner1.QueryContext += new Heren.Common.Report.QueryContextEventHandler(this.reportDesigner1_QueryContext);
            this.reportDesigner1.ElementDoubleClick += new Heren.Common.VectorEditor.CanvasEventHandler(this.reportDesigner1_ElementDoubleClick);
            this.reportDesigner1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.reportDesigner1_Scroll);
            this.reportDesigner1.SizeChanged += new System.EventHandler(this.reportDesigner1_SizeChanged);
            this.reportDesigner1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.reportDesigner1_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblDateFrom,
            this.tooldtpDateFrom,
            this.toollblDateTo,
            this.tooldtpDateTo,
            this.toolStripLabel1,
            this.toolcboPatDeptList,
            this.toolbPageIndex,
            this.toolcboPageList,
            this.toolStripLabel2,
            this.toolbtnNew,
            this.toolbtnPrint,
            this.toolbtnOption});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(808, 32);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toollblDateFrom
            // 
            this.toollblDateFrom.AutoSize = false;
            this.toollblDateFrom.Name = "toollblDateFrom";
            this.toollblDateFrom.Size = new System.Drawing.Size(47, 22);
            this.toollblDateFrom.Text = " 时段：";
            // 
            // tooldtpDateFrom
            // 
            this.tooldtpDateFrom.AutoSize = false;
            this.tooldtpDateFrom.BackColor = System.Drawing.Color.White;
            this.tooldtpDateFrom.Name = "tooldtpDateFrom";
            this.tooldtpDateFrom.ShowHour = false;
            this.tooldtpDateFrom.ShowMinute = false;
            this.tooldtpDateFrom.ShowSecond = false;
            this.tooldtpDateFrom.Size = new System.Drawing.Size(120, 22);
            this.tooldtpDateFrom.Text = "2012/2/20 13:18:12";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 13, 18, 12, 0);
            this.tooldtpDateFrom.ValueChanged += new System.EventHandler(this.tooldtpQueryDate_ValueChanged);
            // 
            // toollblDateTo
            // 
            this.toollblDateTo.AutoSize = false;
            this.toollblDateTo.Name = "toollblDateTo";
            this.toollblDateTo.Size = new System.Drawing.Size(11, 22);
            this.toollblDateTo.Text = "-";
            // 
            // tooldtpDateTo
            // 
            this.tooldtpDateTo.AutoSize = false;
            this.tooldtpDateTo.BackColor = System.Drawing.Color.White;
            this.tooldtpDateTo.Name = "tooldtpDateTo";
            this.tooldtpDateTo.ShowHour = false;
            this.tooldtpDateTo.ShowMinute = false;
            this.tooldtpDateTo.ShowSecond = false;
            this.tooldtpDateTo.Size = new System.Drawing.Size(120, 22);
            this.tooldtpDateTo.Text = "2012/2/20 13:18:12";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 13, 18, 12, 0);
            this.tooldtpDateTo.ValueChanged += new System.EventHandler(this.tooldtpQueryDate_ValueChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel1.Text = " 病区：";
            // 
            // toolcboPatDeptList
            // 
            this.toolcboPatDeptList.AutoSize = false;
            this.toolcboPatDeptList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboPatDeptList.Name = "toolcboPatDeptList";
            this.toolcboPatDeptList.Size = new System.Drawing.Size(120, 25);
            // 
            // toolbPageIndex
            // 
            this.toolbPageIndex.AutoSize = false;
            this.toolbPageIndex.Name = "toolbPageIndex";
            this.toolbPageIndex.Size = new System.Drawing.Size(48, 22);
            this.toolbPageIndex.Text = "页码：";
            this.toolbPageIndex.Visible = false;
            // 
            // toolcboPageList
            // 
            this.toolcboPageList.AutoSize = false;
            this.toolcboPageList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboPageList.Name = "toolcboPageList";
            this.toolcboPageList.Size = new System.Drawing.Size(40, 25);
            this.toolcboPageList.Visible = false;
            this.toolcboPageList.SelectedIndexChanged += new System.EventHandler(this.toolcboPageList_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(10, 22);
            this.toolStripLabel2.Text = " ";
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
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuPrintAll,
            this.toolmnuPrintFrom,
            this.toolmnuPrintContinued,
            this.toolmnuPrintAllPage,
            this.toolmnuPrintPage});
            this.toolbtnPrint.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Print;
            this.toolbtnPrint.Name = "toolbtnPrint";
            this.toolbtnPrint.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrint.Text = "打印";
            this.toolbtnPrint.Visible = false;
            // 
            // toolmnuPrintAll
            // 
            this.toolmnuPrintAll.Name = "toolmnuPrintAll";
            this.toolmnuPrintAll.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintAll.Text = "打印所有行";
            this.toolmnuPrintAll.Click += new System.EventHandler(this.toolmnuPrintAll_Click);
            // 
            // toolmnuPrintFrom
            // 
            this.toolmnuPrintFrom.Name = "toolmnuPrintFrom";
            this.toolmnuPrintFrom.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintFrom.Text = "从选中行开始打印";
            this.toolmnuPrintFrom.Visible = false;
            this.toolmnuPrintFrom.Click += new System.EventHandler(this.toolmnuPrintFrom_Click);
            // 
            // toolmnuPrintContinued
            // 
            this.toolmnuPrintContinued.Name = "toolmnuPrintContinued";
            this.toolmnuPrintContinued.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintContinued.Text = "从选中行开始续打";
            this.toolmnuPrintContinued.Visible = false;
            this.toolmnuPrintContinued.Click += new System.EventHandler(this.toolmnuPrintContinued_Click);
            // 
            // toolmnuPrintAllPage
            // 
            this.toolmnuPrintAllPage.Name = "toolmnuPrintAllPage";
            this.toolmnuPrintAllPage.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintAllPage.Text = "打印所有页";
            this.toolmnuPrintAllPage.Click += new System.EventHandler(this.toolmnuPrintAllPage_Click);
            // 
            // toolmnuPrintPage
            // 
            this.toolmnuPrintPage.Name = "toolmnuPrintPage";
            this.toolmnuPrintPage.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintPage.Text = "打印当前页";
            this.toolmnuPrintPage.Click += new System.EventHandler(this.toolmnuPrintPage_Click);
            // 
            // toolbtnOption
            // 
            this.toolbtnOption.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolbtnOption.AutoSize = false;
            this.toolbtnOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuShowAsModel,
            this.toolmnuSavePreView});
            this.toolbtnOption.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Option;
            this.toolbtnOption.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnOption.Name = "toolbtnOption";
            this.toolbtnOption.Size = new System.Drawing.Size(64, 22);
            this.toolbtnOption.Text = "选项";
            this.toolbtnOption.DropDownOpening += new System.EventHandler(this.toolbtnOption_DropDownOpening);
            // 
            // toolmnuShowAsModel
            // 
            this.toolmnuShowAsModel.Name = "toolmnuShowAsModel";
            this.toolmnuShowAsModel.Size = new System.Drawing.Size(160, 22);
            this.toolmnuShowAsModel.Text = "以弹出方式编辑";
            this.toolmnuShowAsModel.Click += new System.EventHandler(this.toolmnuShowAsModel_Click);
            // 
            // toolmnuSavePreView
            // 
            this.toolmnuSavePreView.Name = "toolmnuSavePreView";
            this.toolmnuSavePreView.Size = new System.Drawing.Size(160, 22);
            this.toolmnuSavePreView.Text = "保存后显示预览";
            this.toolmnuSavePreView.Click += new System.EventHandler(this.toolmnuSavePreView_Click);
            // 
            // GraphDocumentListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(812, 456);
            this.Controls.Add(this.pnlGraph);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "GraphDocumentListForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "病历列表";
            this.cmnenuDocumentList.ResumeLayout(false);
            this.pnlGraph.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ContextMenuStrip cmnenuDocumentList;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.Panel pnlGraph;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolbtnNew;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnPrint;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintAll;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintFrom;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintContinued;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnOption;
        private System.Windows.Forms.ToolStripMenuItem toolmnuShowAsModel;
        private Heren.Common.Report.ReportDesigner reportDesigner1;
        private System.Windows.Forms.Panel panel1;
        private Heren.Common.Report.ReportDesigner reportDesigner2;
        private System.Windows.Forms.ToolStripLabel toolbPageIndex;
        private System.Windows.Forms.ToolStripComboBox toolcboPageList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolcboPatDeptList;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintAllPage;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintPage;
        private System.Windows.Forms.ToolStripMenuItem toolmnuSavePreView;
    }
}