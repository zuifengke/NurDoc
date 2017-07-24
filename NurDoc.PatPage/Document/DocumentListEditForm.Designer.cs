namespace Heren.NurDoc.PatPage.Document
{
    partial class DocumentListEditForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlDocumentList = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dtvDocInfos = new Heren.Common.Controls.TableView.DataTableView();
            this.colRecordTime = new Heren.Common.Controls.TableView.DateTimeColumn();
            this.cmnenuDocumentList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScore = new System.Windows.Forms.ToolStripMenuItem();
            this.documentControl1 = new Heren.NurDoc.PatPage.Document.DocumentControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboPatDeptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnNew = new System.Windows.Forms.ToolStripButton();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolbtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuPrintAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintContinued = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintSingle = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlDocumentList.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvDocInfos)).BeginInit();
            this.cmnenuDocumentList.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDocumentList
            // 
            this.pnlDocumentList.Controls.Add(this.splitContainer1);
            this.pnlDocumentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDocumentList.Location = new System.Drawing.Point(2, 2);
            this.pnlDocumentList.Name = "pnlDocumentList";
            this.pnlDocumentList.Size = new System.Drawing.Size(795, 462);
            this.pnlDocumentList.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 35);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dtvDocInfos);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.documentControl1);
            this.splitContainer1.Size = new System.Drawing.Size(789, 427);
            this.splitContainer1.SplitterDistance = 334;
            this.splitContainer1.TabIndex = 4;
            // 
            // dtvDocInfos
            // 
            this.dtvDocInfos.AllowUserToResizeColumns = false;
            this.dtvDocInfos.AllowUserToResizeRows = true;
            this.dtvDocInfos.AutoWidthForAllColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtvDocInfos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtvDocInfos.ColumnHeadersHeight = 120;
            this.dtvDocInfos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRecordTime});
            this.dtvDocInfos.ContextMenuStrip = this.cmnenuDocumentList;
            this.dtvDocInfos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtvDocInfos.GroupHeight = 50;
            this.dtvDocInfos.Location = new System.Drawing.Point(0, 0);
            this.dtvDocInfos.MultiSelect = false;
            this.dtvDocInfos.Name = "dtvDocInfos";
            this.dtvDocInfos.RowHeadersWidth = 35;
            this.dtvDocInfos.Size = new System.Drawing.Size(789, 334);
            this.dtvDocInfos.TabIndex = 2;
            this.dtvDocInfos.Tag = "记录日期";
            this.dtvDocInfos.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtvDocInfos_CellMouseClick);
            this.dtvDocInfos.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtvDocInfos_CellMouseDoubleClick);
            // 
            // colRecordTime
            // 
            this.colRecordTime.FillWeight = 124F;
            this.colRecordTime.HeaderText = "时间";
            this.colRecordTime.Name = "colRecordTime";
            this.colRecordTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRecordTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRecordTime.Width = 124;
            // 
            // cmnenuDocumentList
            // 
            this.cmnenuDocumentList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuDelete,
            this.mnuScore});
            this.cmnenuDocumentList.Name = "contextMenuStrip1";
            this.cmnenuDocumentList.Size = new System.Drawing.Size(137, 70);
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(136, 22);
            this.mnuNew.Text = "新增";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(136, 22);
            this.mnuDelete.Text = "删除选中行";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // mnuScore
            // 
            this.mnuScore.Name = "mnuScore";
            this.mnuScore.Size = new System.Drawing.Size(136, 22);
            this.mnuScore.Text = "评分";
            this.mnuScore.Click += new System.EventHandler(this.mnuScore_Click);
            // 
            // documentControl1
            // 
            this.documentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentControl1.Document = null;
            this.documentControl1.Location = new System.Drawing.Point(0, 0);
            this.documentControl1.Name = "documentControl1";
            this.documentControl1.Size = new System.Drawing.Size(789, 89);
            this.documentControl1.TabIndex = 3;
            this.documentControl1.Text = "documentControl1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblDateFrom,
            this.tooldtpDateFrom,
            this.toollblDateTo,
            this.tooldtpDateTo,
            this.toolStripLabel1,
            this.toolcboPatDeptList,
            this.toolStripLabel2,
            this.toolbtnNew,
            this.toolbtnSave,
            this.toolbtnDelete,
            this.toolbtnPrint});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(795, 32);
            this.toolStrip1.TabIndex = 1;
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
            this.tooldtpDateFrom.Text = "2012/02/20 11:03:12";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 11, 3, 12, 0);
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
            this.tooldtpDateTo.Text = "2012/02/20 11:03:12";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 11, 3, 12, 0);
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
            // toolbtnDelete
            // 
            this.toolbtnDelete.AutoSize = false;
            this.toolbtnDelete.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Delete;
            this.toolbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnDelete.Name = "toolbtnDelete";
            this.toolbtnDelete.Size = new System.Drawing.Size(64, 22);
            this.toolbtnDelete.Text = "删除";
            this.toolbtnDelete.Click += new System.EventHandler(this.toolbtnDelete_Click);
            // 
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuPrintAll,
            this.toolmnuPrintFrom,
            this.toolmnuPrintContinued,
            this.toolmnuPrintSingle});
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
            this.toolmnuPrintFrom.Click += new System.EventHandler(this.toolmnuPrintFrom_Click);
            // 
            // toolmnuPrintContinued
            // 
            this.toolmnuPrintContinued.Name = "toolmnuPrintContinued";
            this.toolmnuPrintContinued.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintContinued.Text = "从选中行开始续打";
            this.toolmnuPrintContinued.Click += new System.EventHandler(this.toolmnuPrintContinued_Click);
            // 
            // toolmnuPrintSingle
            // 
            this.toolmnuPrintSingle.Name = "toolmnuPrintSingle";
            this.toolmnuPrintSingle.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintSingle.Text = "打印多行记录";
            this.toolmnuPrintSingle.Click += new System.EventHandler(this.toolmnuPrintSingle_Click);
            // 
            // DocumentListEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(799, 466);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pnlDocumentList);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "DocumentListEditForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "病历列表";
            this.pnlDocumentList.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtvDocInfos)).EndInit();
            this.cmnenuDocumentList.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel pnlDocumentList;
        private Heren.Common.Controls.TableView.DataTableView dtvDocInfos;
        private System.Windows.Forms.ContextMenuStrip cmnenuDocumentList;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripButton toolbtnNew;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnPrint;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintAll;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintFrom;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintContinued;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolcboPatDeptList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private DocumentControl documentControl1;
        private System.Windows.Forms.ToolStripButton toolbtnDelete;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem mnuScore;
        private Heren.Common.Controls.TableView.DateTimeColumn colRecordTime;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintSingle;
    }
}