namespace Heren.NurDoc.PatPage
{
    partial class OrdersRecordForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblOrdersType = new System.Windows.Forms.ToolStripLabel();
            this.toolcboOrdersType = new System.Windows.Forms.ToolStripComboBox();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblSpace1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboOrdersStatus = new System.Windows.Forms.ToolStripComboBox();
            this.toolbtnPreview = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuPrintAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintContinued = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboOrdersCategory = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboTreatment = new System.Windows.Forms.ToolStripComboBox();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderSubNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnterDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDosage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPerformResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDosageUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdministration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFreqDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDoctor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPerformTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNurse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStopDcotor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessingEndDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStopNurse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShorttime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShortNurse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblOrdersType,
            this.toolcboOrdersType,
            this.toollblDateFrom,
            this.tooldtpDateFrom,
            this.toollblDateTo,
            this.tooldtpDateTo,
            this.toollblSpace1,
            this.toolStripLabel1,
            this.toolcboOrdersStatus,
            this.toolbtnPreview,
            this.toolbtnPrint,
            this.toolStripLabel2,
            this.toolcboOrdersCategory,
            this.toolStripLabel3,
            this.toolcboTreatment});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(1040, 32);
            this.toolStrip1.TabIndex = 21;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toollblOrdersType
            // 
            this.toollblOrdersType.AutoSize = false;
            this.toollblOrdersType.Name = "toollblOrdersType";
            this.toollblOrdersType.Size = new System.Drawing.Size(72, 22);
            this.toollblOrdersType.Text = " 医嘱类型：";
            // 
            // toolcboOrdersType
            // 
            this.toolcboOrdersType.AutoCompleteCustomSource.AddRange(new string[] {
            "长期医嘱",
            "临时医嘱"});
            this.toolcboOrdersType.AutoSize = false;
            this.toolcboOrdersType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboOrdersType.Items.AddRange(new object[] {
            "全部",
            "长期医嘱",
            "临时医嘱"});
            this.toolcboOrdersType.Name = "toolcboOrdersType";
            this.toolcboOrdersType.Size = new System.Drawing.Size(100, 25);
            this.toolcboOrdersType.SelectedIndexChanged += new System.EventHandler(this.toolcboOrdersType_SelectedIndexChanged);
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
            this.tooldtpDateFrom.Text = "2012/2/20 9:09:46";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 9, 9, 46, 0);
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
            this.tooldtpDateTo.Text = "2012/8/28 9:09:46";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 8, 28, 9, 9, 46, 0);
            this.tooldtpDateTo.ValueChanged += new System.EventHandler(this.tooldtpQueryDate_ValueChanged);
            // 
            // toollblSpace1
            // 
            this.toollblSpace1.AutoSize = false;
            this.toollblSpace1.Name = "toollblSpace1";
            this.toollblSpace1.Size = new System.Drawing.Size(8, 22);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(67, 29);
            this.toolStripLabel1.Text = "停止类型：";
            // 
            // toolcboOrdersStatus
            // 
            this.toolcboOrdersStatus.AutoSize = false;
            this.toolcboOrdersStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboOrdersStatus.Items.AddRange(new object[] {
            "",
            "已停",
            "未停",
            "已撤销",
            "全部"});
            this.toolcboOrdersStatus.Name = "toolcboOrdersStatus";
            this.toolcboOrdersStatus.Size = new System.Drawing.Size(75, 25);
            this.toolcboOrdersStatus.SelectedIndexChanged += new System.EventHandler(this.toolcboOrdersStatus_SelectedIndexChanged);
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
            this.toolbtnPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuPrintAll,
            this.toolmnuPrintFrom,
            this.toolmnuPrintContinued});
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
            // toolStripLabel2
            // 
            this.toolStripLabel2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(44, 29);
            this.toolStripLabel2.Text = "类别：";
            // 
            // toolcboOrdersCategory
            // 
            this.toolcboOrdersCategory.DropDownWidth = 121;
            this.toolcboOrdersCategory.Items.AddRange(new object[] {
            "",
            "药物医嘱",
            "诊疗医嘱",
            "检查医嘱",
            "检验医嘱",
            "护理医嘱"});
            this.toolcboOrdersCategory.Name = "toolcboOrdersCategory";
            this.toolcboOrdersCategory.Size = new System.Drawing.Size(100, 32);
            this.toolcboOrdersCategory.SelectedIndexChanged += new System.EventHandler(this.toolcboOrdersCategory_SelectedIndexChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 29);
            this.toolStripLabel3.Text = "用药方式";
            // 
            // toolcboTreatment
            // 
            this.toolcboTreatment.Items.AddRange(new object[] {
            "全部"});
            this.toolcboTreatment.Name = "toolcboTreatment";
            this.toolcboTreatment.Size = new System.Drawing.Size(121, 32);
            this.toolcboTreatment.SelectedIndexChanged += new System.EventHandler(this.toolcboTreatment_SelectedIndexChanged);
            // 
            // dataTableView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTableView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colOrderNo,
            this.colOrderSubNo,
            this.colOrderClass,
            this.colEnterDate,
            this.colOrderText,
            this.colSubFlag,
            this.colDosage,
            this.colPerformResult,
            this.colDosageUnits,
            this.colAdministration,
            this.colFrequency,
            this.colFreqDetail,
            this.colEndDate,
            this.colDoctor,
            this.colPerformTime,
            this.colNurse,
            this.colStopDcotor,
            this.colProcessingEndDateTime,
            this.colStopNurse,
            this.colShorttime,
            this.colShortNurse});
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(2, 34);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersWidth = 24;
            this.dataTableView1.Size = new System.Drawing.Size(1040, 466);
            this.dataTableView1.TabIndex = 22;
            this.dataTableView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataTableView1_CellFormatting);
            // 
            // colNumber
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colNumber.DefaultCellStyle = dataGridViewCellStyle2;
            this.colNumber.FillWeight = 36F;
            this.colNumber.Frozen = true;
            this.colNumber.HeaderText = "序号";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNumber.Width = 36;
            // 
            // colOrderNo
            // 
            this.colOrderNo.HeaderText = "医嘱序号";
            this.colOrderNo.Name = "colOrderNo";
            this.colOrderNo.ReadOnly = true;
            this.colOrderNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderNo.Visible = false;
            // 
            // colOrderSubNo
            // 
            this.colOrderSubNo.HeaderText = "医嘱子序号";
            this.colOrderSubNo.Name = "colOrderSubNo";
            this.colOrderSubNo.ReadOnly = true;
            this.colOrderSubNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderSubNo.Visible = false;
            // 
            // colOrderClass
            // 
            this.colOrderClass.FillWeight = 40F;
            this.colOrderClass.Frozen = true;
            this.colOrderClass.HeaderText = "类别";
            this.colOrderClass.Name = "colOrderClass";
            this.colOrderClass.ReadOnly = true;
            this.colOrderClass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderClass.Width = 40;
            // 
            // colEnterDate
            // 
            this.colEnterDate.FillWeight = 120F;
            this.colEnterDate.Frozen = true;
            this.colEnterDate.HeaderText = "下达时间";
            this.colEnterDate.Name = "colEnterDate";
            this.colEnterDate.ReadOnly = true;
            this.colEnterDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEnterDate.Width = 120;
            // 
            // colOrderText
            // 
            this.colOrderText.FillWeight = 200F;
            this.colOrderText.Frozen = true;
            this.colOrderText.HeaderText = "医嘱内容";
            this.colOrderText.Name = "colOrderText";
            this.colOrderText.ReadOnly = true;
            this.colOrderText.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderText.Width = 200;
            // 
            // colSubFlag
            // 
            this.colSubFlag.FillWeight = 24F;
            this.colSubFlag.Frozen = true;
            this.colSubFlag.HeaderText = "";
            this.colSubFlag.Name = "colSubFlag";
            this.colSubFlag.ReadOnly = true;
            this.colSubFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSubFlag.Width = 24;
            // 
            // colDosage
            // 
            this.colDosage.FillWeight = 50F;
            this.colDosage.HeaderText = "剂量";
            this.colDosage.Name = "colDosage";
            this.colDosage.ReadOnly = true;
            this.colDosage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDosage.Width = 50;
            // 
            // colPerformResult
            // 
            this.colPerformResult.HeaderText = "阴性";
            this.colPerformResult.Name = "colPerformResult";
            this.colPerformResult.ReadOnly = true;
            this.colPerformResult.Width = 35;
            // 
            // colDosageUnits
            // 
            this.colDosageUnits.FillWeight = 50F;
            this.colDosageUnits.HeaderText = "单位";
            this.colDosageUnits.Name = "colDosageUnits";
            this.colDosageUnits.ReadOnly = true;
            this.colDosageUnits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDosageUnits.Width = 50;
            // 
            // colAdministration
            // 
            this.colAdministration.FillWeight = 64F;
            this.colAdministration.HeaderText = "途径";
            this.colAdministration.Name = "colAdministration";
            this.colAdministration.ReadOnly = true;
            this.colAdministration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAdministration.Width = 64;
            // 
            // colFrequency
            // 
            this.colFrequency.FillWeight = 60F;
            this.colFrequency.HeaderText = "频次";
            this.colFrequency.Name = "colFrequency";
            this.colFrequency.ReadOnly = true;
            this.colFrequency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFrequency.Width = 60;
            // 
            // colFreqDetail
            // 
            this.colFreqDetail.HeaderText = "医生说明";
            this.colFreqDetail.Name = "colFreqDetail";
            this.colFreqDetail.ReadOnly = true;
            this.colFreqDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colEndDate
            // 
            this.colEndDate.FillWeight = 120F;
            this.colEndDate.HeaderText = "停止时间";
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.ReadOnly = true;
            this.colEndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEndDate.Width = 120;
            // 
            // colDoctor
            // 
            this.colDoctor.FillWeight = 64F;
            this.colDoctor.HeaderText = "医生";
            this.colDoctor.Name = "colDoctor";
            this.colDoctor.ReadOnly = true;
            this.colDoctor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDoctor.Width = 64;
            // 
            // colPerformTime
            // 
            this.colPerformTime.FillWeight = 60F;
            this.colPerformTime.HeaderText = "执行时间";
            this.colPerformTime.Name = "colPerformTime";
            this.colPerformTime.ReadOnly = true;
            this.colPerformTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPerformTime.Width = 60;
            // 
            // colNurse
            // 
            this.colNurse.FillWeight = 64F;
            this.colNurse.HeaderText = "护士";
            this.colNurse.Name = "colNurse";
            this.colNurse.ReadOnly = true;
            this.colNurse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNurse.Width = 64;
            // 
            // colStopDcotor
            // 
            this.colStopDcotor.HeaderText = "停止医生签名";
            this.colStopDcotor.Name = "colStopDcotor";
            this.colStopDcotor.ReadOnly = true;
            this.colStopDcotor.Width = 64;
            // 
            // colProcessingEndDateTime
            // 
            this.colProcessingEndDateTime.HeaderText = "停止执行时间";
            this.colProcessingEndDateTime.Name = "colProcessingEndDateTime";
            this.colProcessingEndDateTime.ReadOnly = true;
            // 
            // colStopNurse
            // 
            this.colStopNurse.HeaderText = "停止护士签名";
            this.colStopNurse.Name = "colStopNurse";
            this.colStopNurse.ReadOnly = true;
            this.colStopNurse.Width = 64;
            // 
            // colShorttime
            // 
            this.colShorttime.HeaderText = "临时执行时间";
            this.colShorttime.Name = "colShorttime";
            this.colShorttime.ReadOnly = true;
            // 
            // colShortNurse
            // 
            this.colShortNurse.HeaderText = "临时护士签名";
            this.colShortNurse.Name = "colShortNurse";
            this.colShortNurse.ReadOnly = true;
            this.colShortNurse.Width = 64;
            // 
            // OrdersRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1044, 502);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "OrdersRecordForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "医嘱单";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripLabel toollblOrdersType;
        private System.Windows.Forms.ToolStripComboBox toolcboOrdersType;
        private System.Windows.Forms.ToolStripLabel toollblSpace1;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnPrint;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintAll;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintFrom;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintContinued;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolcboOrdersStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderSubNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnterDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDosage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPerformResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDosageUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdministration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrequency;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFreqDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDoctor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPerformTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNurse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStopDcotor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessingEndDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStopNurse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShorttime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShortNurse;
        private System.Windows.Forms.ToolStripButton toolbtnPreview;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolcboOrdersCategory;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox toolcboTreatment;
    }
}