namespace Heren.NurDoc.Frame
{
    partial class QcOrderRecForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtnPrev = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbtnNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnQc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbtnQcCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbtnQcQuestion = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toollblPatientName = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toollblPatientID = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboOrdersType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboStatus = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnQcAll = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTableView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Status,
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
            this.dataTableView1.Location = new System.Drawing.Point(0, 35);
            this.dataTableView1.MultiSelect = false;
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.RowHeadersWidth = 24;
            this.dataTableView1.Size = new System.Drawing.Size(963, 443);
            this.dataTableView1.TabIndex = 23;
            this.dataTableView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTableView1_CellMouseClick);
            // 
            // Status
            // 
            this.Status.HeaderText = "审核状态";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 60;
            // 
            // colNumber
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colNumber.DefaultCellStyle = dataGridViewCellStyle6;
            this.colNumber.FillWeight = 36F;
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
            this.colOrderClass.HeaderText = "类别";
            this.colOrderClass.Name = "colOrderClass";
            this.colOrderClass.ReadOnly = true;
            this.colOrderClass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderClass.Width = 40;
            // 
            // colEnterDate
            // 
            this.colEnterDate.FillWeight = 120F;
            this.colEnterDate.HeaderText = "下达时间";
            this.colEnterDate.Name = "colEnterDate";
            this.colEnterDate.ReadOnly = true;
            this.colEnterDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEnterDate.Width = 120;
            // 
            // colOrderText
            // 
            this.colOrderText.FillWeight = 200F;
            this.colOrderText.HeaderText = "医嘱内容";
            this.colOrderText.Name = "colOrderText";
            this.colOrderText.ReadOnly = true;
            this.colOrderText.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderText.Width = 200;
            // 
            // colSubFlag
            // 
            this.colSubFlag.FillWeight = 24F;
            this.colSubFlag.HeaderText = string.Empty;
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
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtnPrev,
            this.toolStripSeparator1,
            this.toolbtnNext,
            this.toolStripLabel2,
            this.toolbtnQc,
            this.toolStripSeparator2,
            this.toolbtnQcCancel,
            this.toolStripSeparator3,
            this.toolbtnQcQuestion,
            this.toolStripLabel4,
            this.toollblPatientName,
            this.toolStripLabel1,
            this.toolStripLabel3,
            this.toollblPatientID,
            this.toolStripLabel6,
            this.toolStripLabel8,
            this.toolcboOrdersType,
            this.toolStripLabel5,
            this.toolcboStatus});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(963, 32);
            this.toolStrip1.TabIndex = 24;
            this.toolStrip1.Text = "baseToolStrip1";
            // 
            // toolbtnPrev
            // 
            this.toolbtnPrev.AutoSize = false;
            this.toolbtnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnPrev.Name = "toolbtnPrev";
            this.toolbtnPrev.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrev.Text = "上一位";
            this.toolbtnPrev.Click += new System.EventHandler(this.toolbtnPrev_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // toolbtnNext
            // 
            this.toolbtnNext.AutoSize = false;
            this.toolbtnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNext.Name = "toolbtnNext";
            this.toolbtnNext.Size = new System.Drawing.Size(64, 22);
            this.toolbtnNext.Text = "下一位";
            this.toolbtnNext.Click += new System.EventHandler(this.toolbtnNext_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(60, 29);
            this.toolStripLabel2.Text = "             ";
            // 
            // toolbtnQc
            // 
            this.toolbtnQc.AutoSize = false;
            this.toolbtnQc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnQc.Name = "toolbtnQc";
            this.toolbtnQc.Size = new System.Drawing.Size(60, 22);
            this.toolbtnQc.Text = "审核";
            this.toolbtnQc.Click += new System.EventHandler(this.toolbtnQc_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // toolbtnQcCancel
            // 
            this.toolbtnQcCancel.AutoSize = false;
            this.toolbtnQcCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnQcCancel.Name = "toolbtnQcCancel";
            this.toolbtnQcCancel.Size = new System.Drawing.Size(60, 22);
            this.toolbtnQcCancel.Text = "取消审核";
            this.toolbtnQcCancel.Click += new System.EventHandler(this.toolbtnQcCancel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // toolbtnQcQuestion
            // 
            this.toolbtnQcQuestion.AutoSize = false;
            this.toolbtnQcQuestion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnQcQuestion.Name = "toolbtnQcQuestion";
            this.toolbtnQcQuestion.Size = new System.Drawing.Size(64, 22);
            this.toolbtnQcQuestion.Text = "标记";
            this.toolbtnQcQuestion.Click += new System.EventHandler(this.toolbtnQcQuestion_Click);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(44, 29);
            this.toolStripLabel4.Text = "姓名：";
            // 
            // toollblPatientName
            // 
            this.toollblPatientName.Name = "toollblPatientName";
            this.toollblPatientName.Size = new System.Drawing.Size(12, 29);
            this.toollblPatientName.Text = " ";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(20, 29);
            this.toolStripLabel1.Text = "   ";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 29);
            this.toolStripLabel3.Text = "病案号：";
            // 
            // toollblPatientID
            // 
            this.toollblPatientID.Name = "toollblPatientID";
            this.toollblPatientID.Size = new System.Drawing.Size(12, 29);
            this.toollblPatientID.Text = " ";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(20, 29);
            this.toolStripLabel6.Text = "   ";
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(68, 29);
            this.toolStripLabel8.Text = "医嘱类型：";
            // 
            // toolcboOrdersType
            // 
            this.toolcboOrdersType.AutoCompleteCustomSource.AddRange(new string[] {
            "长期医嘱",
            "短期医嘱"});
            this.toolcboOrdersType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboOrdersType.Items.AddRange(new object[] {
            "长期医嘱",
            "临时医嘱"});
            this.toolcboOrdersType.Name = "toolcboOrdersType";
            this.toolcboOrdersType.Size = new System.Drawing.Size(100, 32);
            this.toolcboOrdersType.SelectedIndexChanged += new System.EventHandler(this.toolcboOrdersType_SelectedIndexChanged);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(68, 29);
            this.toolStripLabel5.Text = "审核状态：";
            // 
            // toolcboStatus
            // 
            this.toolcboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboStatus.Items.AddRange(new object[] {
            string.Empty,
            "已审核",
            "标记中",
            "未审核"});
            this.toolcboStatus.Name = "toolcboStatus";
            this.toolcboStatus.Size = new System.Drawing.Size(100, 32);
            this.toolcboStatus.SelectedIndexChanged += new System.EventHandler(this.toolcboStatus_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnQcAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // btnQcAll
            // 
            this.btnQcAll.Name = "btnQcAll";
            this.btnQcAll.Size = new System.Drawing.Size(124, 22);
            this.btnQcAll.Text = "全部审核";
            this.btnQcAll.Click += new System.EventHandler(this.btnQcAll_Click);
            // 
            // QcOrderRecForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 480);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataTableView1);
            this.Name = "QcOrderRecForm";
            this.Text = "QCOrderRedForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolbtnPrev;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolbtnNext;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolbtnQc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolbtnQcCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolbtnQcQuestion;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toollblPatientName;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel toollblPatientID;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripComboBox toolcboOrdersType;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnQcAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
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
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox toolcboStatus;
    }
}