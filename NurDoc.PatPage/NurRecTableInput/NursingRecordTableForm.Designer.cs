namespace Heren.NurDoc.PatPage.NurRecTableInput
{
    partial class NursingRecordTableForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NursingRecordTableForm));
            this.pnlNursingRecord = new System.Windows.Forms.Panel();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colRecordTime = new Heren.Common.Forms.XDateTimeColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtnSchemaList = new System.Windows.Forms.ToolStripLabel();
            this.toolcboSchemaList = new System.Windows.Forms.ToolStripComboBox();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboPatDeptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolbtnDelete = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolbtnImportOrder = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolbtnSubOrder = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolbtnCopy = new System.Windows.Forms.ToolStripButton();
            this.toolbtnStatistics = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuPrintAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintContinued = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnOption = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuShowAsModel = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlNursingRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNursingRecord
            // 
            this.pnlNursingRecord.Controls.Add(this.dataTableView1);
            this.pnlNursingRecord.Controls.Add(this.toolStrip1);
            this.pnlNursingRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNursingRecord.Location = new System.Drawing.Point(2, 2);
            this.pnlNursingRecord.Name = "pnlNursingRecord";
            this.pnlNursingRecord.Size = new System.Drawing.Size(1124, 508);
            this.pnlNursingRecord.TabIndex = 0;
            // 
            // dataTableView1
            // 
            this.dataTableView1.AllowUserToResizeRows = true;
            this.dataTableView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataTableView1.AutoWidthColumns.Add(0);
            this.dataTableView1.AutoWidthColumns.Add(1);
            this.dataTableView1.AutoWidthColumns.Add(2);
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTableView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataTableView1.ColumnHeadersHeight = 44;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRecordTime});
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(0, 32);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersWidth = 24;
            this.dataTableView1.Size = new System.Drawing.Size(1124, 476);
            this.dataTableView1.TabIndex = 2;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            this.dataTableView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataTableView1_CellPainting);
            this.dataTableView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellValueChanged);
            // 
            // colRecordTime
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colRecordTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.colRecordTime.FillWeight = 108F;
            this.colRecordTime.Frozen = true;
            this.colRecordTime.HeaderText = "时间";
            this.colRecordTime.Name = "colRecordTime";
            this.colRecordTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRecordTime.ShowSecond = false;
            this.colRecordTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRecordTime.Width = 120;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtnSchemaList,
            this.toolcboSchemaList,
            this.toollblDateFrom,
            this.tooldtpDateFrom,
            this.toollblDateTo,
            this.tooldtpDateTo,
            this.toolStripLabel1,
            this.toolcboPatDeptList,
            this.toolStripDropDownButton1,
            this.toolbtnDelete,
            this.toolbtnImportOrder,
            this.toolbtnSubOrder,
            this.toolStripButton1,
            this.toolbtnCopy,
            this.toolbtnStatistics,
            this.toolbtnPrint,
            this.toolbtnOption});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(1124, 32);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "baseToolStrip1";
            // 
            // toolbtnSchemaList
            // 
            this.toolbtnSchemaList.AutoSize = false;
            this.toolbtnSchemaList.Name = "toolbtnSchemaList";
            this.toolbtnSchemaList.Size = new System.Drawing.Size(80, 22);
            this.toolbtnSchemaList.Text = " 列显示方案：";
            this.toolbtnSchemaList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolcboSchemaList
            // 
            this.toolcboSchemaList.AutoSize = false;
            this.toolcboSchemaList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboSchemaList.Name = "toolcboSchemaList";
            this.toolcboSchemaList.Size = new System.Drawing.Size(120, 25);
            this.toolcboSchemaList.SelectedIndexChanged += new System.EventHandler(this.toolcboSchemaList_SelectedIndexChanged);
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
            this.tooldtpDateFrom.Size = new System.Drawing.Size(96, 22);
            this.tooldtpDateFrom.Text = "2012/2/20 15:41:39";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 15, 41, 39, 0);
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
            this.tooldtpDateTo.Size = new System.Drawing.Size(96, 22);
            this.tooldtpDateTo.Text = "2012/2/20 15:41:39";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 15, 41, 39, 0);
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
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoSize = false;
            this.toolStripDropDownButton1.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Add;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(64, 22);
            this.toolStripDropDownButton1.Text = "新增";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.mnuNewRecord_Click);
            // 
            // toolbtnDelete
            // 
            this.toolbtnDelete.AutoSize = false;
            this.toolbtnDelete.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Delete;
            this.toolbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnDelete.Name = "toolbtnDelete";
            this.toolbtnDelete.ShowDropDownArrow = false;
            this.toolbtnDelete.Size = new System.Drawing.Size(64, 22);
            this.toolbtnDelete.Text = "删除";
            this.toolbtnDelete.Click += new System.EventHandler(this.toolbtnDelete_Click);
            // 
            // toolbtnImportOrder
            // 
            this.toolbtnImportOrder.AutoSize = false;
            this.toolbtnImportOrder.Image = global::Heren.NurDoc.PatPage.Properties.Resources.SubOrder;
            this.toolbtnImportOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnImportOrder.Name = "toolbtnImportOrder";
            this.toolbtnImportOrder.ShowDropDownArrow = false;
            this.toolbtnImportOrder.Size = new System.Drawing.Size(70, 22);
            this.toolbtnImportOrder.Text = "导入医嘱";
            this.toolbtnImportOrder.Click += new System.EventHandler(this.toolbtnImportOrder_Click);
            // 
            // toolbtnSubOrder
            // 
            this.toolbtnSubOrder.AutoSize = false;
            this.toolbtnSubOrder.Image = global::Heren.NurDoc.PatPage.Properties.Resources.SubOrder;
            this.toolbtnSubOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSubOrder.Name = "toolbtnSubOrder";
            this.toolbtnSubOrder.ShowDropDownArrow = false;
            this.toolbtnSubOrder.Size = new System.Drawing.Size(70, 22);
            this.toolbtnSubOrder.Text = "子医嘱";
            this.toolbtnSubOrder.Click += new System.EventHandler(this.toolbtnSubOrder_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Save;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(64, 22);
            this.toolStripButton1.Text = "保存";
            this.toolStripButton1.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // toolbtnCopy
            // 
            this.toolbtnCopy.AutoSize = false;
            this.toolbtnCopy.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Copy;
            this.toolbtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnCopy.Name = "toolbtnCopy";
            this.toolbtnCopy.Size = new System.Drawing.Size(64, 22);
            this.toolbtnCopy.Text = "复制";
            this.toolbtnCopy.Click += new System.EventHandler(this.toolbtnCopy_Click);
            // 
            // toolbtnStatistics
            // 
            this.toolbtnStatistics.AutoSize = false;
            this.toolbtnStatistics.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Statistic;
            this.toolbtnStatistics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnStatistics.Name = "toolbtnStatistics";
            this.toolbtnStatistics.Size = new System.Drawing.Size(64, 22);
            this.toolbtnStatistics.Text = "统计";
            this.toolbtnStatistics.Click += new System.EventHandler(this.toolbtnStatistics_Click);
            // 
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuPrintAll,
            this.toolmnuPrintSelected,
            this.toolmnuPrintReset,
            this.toolmnuPrintFrom,
            this.toolmnuPrintContinued});
            this.toolbtnPrint.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Print;
            this.toolbtnPrint.Name = "toolbtnPrint";
            this.toolbtnPrint.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrint.Text = "打印";
            // 
            // toolmnuPrintAll
            // 
            this.toolmnuPrintAll.Name = "toolmnuPrintAll";
            this.toolmnuPrintAll.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintAll.Text = "打印所有行";
            this.toolmnuPrintAll.Click += new System.EventHandler(this.toolmnuPrintAll_Click);
            // 
            // toolmnuPrintSelected
            // 
            this.toolmnuPrintSelected.Name = "toolmnuPrintSelected";
            this.toolmnuPrintSelected.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintSelected.Text = "打印选中行";
            this.toolmnuPrintSelected.Click += new System.EventHandler(this.toolmnuPrintSelected_Click);
            // 
            // toolmnuPrintReset
            // 
            this.toolmnuPrintReset.Name = "toolmnuPrintReset";
            this.toolmnuPrintReset.Size = new System.Drawing.Size(172, 22);
            this.toolmnuPrintReset.Text = "重打所有行";
            this.toolmnuPrintReset.Click += new System.EventHandler(this.toolmnuPrintReset_Click);
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
            // toolbtnOption
            // 
            this.toolbtnOption.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolbtnOption.AutoSize = false;
            this.toolbtnOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuShowAsModel});
            this.toolbtnOption.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Option;
            this.toolbtnOption.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnOption.Name = "toolbtnOption";
            this.toolbtnOption.Size = new System.Drawing.Size(64, 22);
            this.toolbtnOption.Text = "选项";
            // 
            // toolmnuShowAsModel
            // 
            this.toolmnuShowAsModel.Name = "toolmnuShowAsModel";
            this.toolmnuShowAsModel.Size = new System.Drawing.Size(160, 22);
            this.toolmnuShowAsModel.Text = "以弹出方式编辑";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "FolderClose.png");
            this.imageList1.Images.SetKeyName(1, "FolderOpen.png");
            this.imageList1.Images.SetKeyName(2, "NursingDoc.png");
            // 
            // NursingRecordTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1128, 512);
            this.Controls.Add(this.pnlNursingRecord);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NursingRecordTableForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.pnlNursingRecord.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNursingRecord;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripLabel toolbtnSchemaList;
        private System.Windows.Forms.ToolStripComboBox toolcboSchemaList;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnPrint;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintAll;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintFrom;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintContinued;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintReset;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintSelected;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolcboPatDeptList;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnDelete;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnOption;
        private System.Windows.Forms.ToolStripMenuItem toolmnuShowAsModel;
        private System.Windows.Forms.ToolStripButton toolbtnCopy;
        private Heren.Common.Forms.XDateTimeColumn colRecordTime;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnSubOrder;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnImportOrder;
        private System.Windows.Forms.ToolStripButton toolbtnStatistics;
    }
}