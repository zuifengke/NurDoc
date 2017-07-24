namespace Heren.NurDoc.Utilities.Import
{
    partial class OrdersImportForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblSpace = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnQuery = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvOrderType = new System.Windows.Forms.ListView();
            this.colFilter = new System.Windows.Forms.ColumnHeader();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.dataTableView2 = new Heren.Common.Controls.TableView.DataTableView();
            this.colScheduleTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExecClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPerformedFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperatingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealDosage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnImport = new Heren.Common.Controls.HerenButton();
            this.btnClose = new Heren.Common.Controls.HerenButton();
            this.colCheckOrder = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRepeatIndicator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnterDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDosage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDosageUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdministration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFreqDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDoctor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNurse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderSubNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView2)).BeginInit();
            this.SuspendLayout();
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
            this.toollblSpace,
            this.toolbtnQuery});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(889, 32);
            this.toolStrip1.TabIndex = 0;
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
            this.tooldtpDateFrom.Text = "2012/02/20 14:14:31";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 14, 14, 31, 0);
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
            this.tooldtpDateTo.Text = "2012/02/20 14:14:31";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 14, 14, 31, 0);
            this.tooldtpDateTo.ValueChanged += new System.EventHandler(this.tooldtpQueryDate_ValueChanged);
            // 
            // toollblSpace
            // 
            this.toollblSpace.AutoSize = false;
            this.toollblSpace.Name = "toollblSpace";
            this.toollblSpace.Size = new System.Drawing.Size(11, 22);
            this.toollblSpace.Text = " ";
            // 
            // toolbtnQuery
            // 
            this.toolbtnQuery.AutoSize = false;
            this.toolbtnQuery.Image = global::Heren.NurDoc.Utilities.Properties.Resources.Query;
            this.toolbtnQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnQuery.Name = "toolbtnQuery";
            this.toolbtnQuery.Size = new System.Drawing.Size(64, 22);
            this.toolbtnQuery.Text = "查询";
            this.toolbtnQuery.Click += new System.EventHandler(this.toolbtnQuery_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(2, 32);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvOrderType);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(885, 436);
            this.splitContainer1.SplitterDistance = 107;
            this.splitContainer1.TabIndex = 1;
            // 
            // lvOrderType
            // 
            this.lvOrderType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilter});
            this.lvOrderType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvOrderType.FullRowSelect = true;
            this.lvOrderType.Location = new System.Drawing.Point(0, 0);
            this.lvOrderType.Name = "lvOrderType";
            this.lvOrderType.Size = new System.Drawing.Size(107, 436);
            this.lvOrderType.TabIndex = 2;
            this.lvOrderType.UseCompatibleStateImageBehavior = false;
            this.lvOrderType.View = System.Windows.Forms.View.Details;
            this.lvOrderType.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvOrderType_MouseClick);
            // 
            // colFilter
            // 
            this.colFilter.Text = "选择医嘱类型";
            this.colFilter.Width = 100;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataTableView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataTableView2);
            this.splitContainer2.Size = new System.Drawing.Size(774, 436);
            this.splitContainer2.SplitterDistance = 300;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataTableView1
            // 
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckOrder,
            this.colRepeatIndicator,
            this.colOrderClass,
            this.colEnterDate,
            this.colOrderText,
            this.colSubFlag,
            this.colDosage,
            this.colDosageUnits,
            this.colAdministration,
            this.colFrequency,
            this.colFreqDetail,
            this.colEndDate,
            this.colDoctor,
            this.colNurse,
            this.OrderNo,
            this.OrderSubNo});
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(0, 0);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.Size = new System.Drawing.Size(774, 300);
            this.dataTableView1.TabIndex = 3;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            this.dataTableView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataTableView1_CellFormatting);
            // 
            // dataTableView2
            // 
            this.dataTableView2.ColumnHeadersHeight = 24;
            this.dataTableView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colScheduleTime,
            this.colExecClass,
            this.colPerformedFlag,
            this.colOperator,
            this.colOperatingTime,
            this.colRealDosage});
            this.dataTableView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView2.Location = new System.Drawing.Point(0, 0);
            this.dataTableView2.Name = "dataTableView2";
            this.dataTableView2.RowHeadersVisible = false;
            this.dataTableView2.Size = new System.Drawing.Size(774, 132);
            this.dataTableView2.TabIndex = 4;
            // 
            // colScheduleTime
            // 
            this.colScheduleTime.FillWeight = 120F;
            this.colScheduleTime.HeaderText = "计划执行时间";
            this.colScheduleTime.Name = "colScheduleTime";
            this.colScheduleTime.ReadOnly = true;
            this.colScheduleTime.Width = 120;
            // 
            // colExecClass
            // 
            this.colExecClass.HeaderText = "执行类别";
            this.colExecClass.Name = "colExecClass";
            this.colExecClass.ReadOnly = true;
            // 
            // colPerformedFlag
            // 
            this.colPerformedFlag.HeaderText = "执行状态";
            this.colPerformedFlag.Name = "colPerformedFlag";
            this.colPerformedFlag.ReadOnly = true;
            // 
            // colOperator
            // 
            this.colOperator.HeaderText = "执行人";
            this.colOperator.Name = "colOperator";
            this.colOperator.ReadOnly = true;
            // 
            // colOperatingTime
            // 
            this.colOperatingTime.FillWeight = 120F;
            this.colOperatingTime.HeaderText = "执行时间";
            this.colOperatingTime.Name = "colOperatingTime";
            this.colOperatingTime.ReadOnly = true;
            this.colOperatingTime.Width = 120;
            // 
            // colRealDosage
            // 
            this.colRealDosage.HeaderText = "实入量";
            this.colRealDosage.Name = "colRealDosage";
            this.colRealDosage.ReadOnly = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(12, 483);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll.TabIndex = 4;
            this.chkSelectAll.Text = "全选";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnImport.Font = new System.Drawing.Font("SimSun", 9F);
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(675, 478);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(88, 24);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("SimSun", 9F);
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(783, 478);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 24);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // colCheckOrder
            // 
            this.colCheckOrder.FillWeight = 28F;
            this.colCheckOrder.Frozen = true;
            this.colCheckOrder.HeaderText = "";
            this.colCheckOrder.Name = "colCheckOrder";
            this.colCheckOrder.Width = 28;
            // 
            // colRepeatIndicator
            // 
            this.colRepeatIndicator.FillWeight = 24F;
            this.colRepeatIndicator.Frozen = true;
            this.colRepeatIndicator.HeaderText = "";
            this.colRepeatIndicator.Name = "colRepeatIndicator";
            this.colRepeatIndicator.ReadOnly = true;
            this.colRepeatIndicator.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRepeatIndicator.Width = 24;
            // 
            // colOrderClass
            // 
            this.colOrderClass.FillWeight = 36F;
            this.colOrderClass.Frozen = true;
            this.colOrderClass.HeaderText = "类别";
            this.colOrderClass.Name = "colOrderClass";
            this.colOrderClass.ReadOnly = true;
            this.colOrderClass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderClass.Width = 36;
            // 
            // colEnterDate
            // 
            this.colEnterDate.FillWeight = 115F;
            this.colEnterDate.Frozen = true;
            this.colEnterDate.HeaderText = "下达时间";
            this.colEnterDate.Name = "colEnterDate";
            this.colEnterDate.ReadOnly = true;
            this.colEnterDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEnterDate.Width = 115;
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
            this.colDosage.FillWeight = 80F;
            this.colDosage.HeaderText = "剂量";
            this.colDosage.Name = "colDosage";
            this.colDosage.ReadOnly = true;
            this.colDosage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDosage.Width = 80;
            // 
            // colDosageUnits
            // 
            this.colDosageUnits.FillWeight = 64F;
            this.colDosageUnits.HeaderText = "单位";
            this.colDosageUnits.Name = "colDosageUnits";
            this.colDosageUnits.ReadOnly = true;
            this.colDosageUnits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDosageUnits.Width = 64;
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
            this.colFrequency.FillWeight = 64F;
            this.colFrequency.HeaderText = "频次";
            this.colFrequency.Name = "colFrequency";
            this.colFrequency.ReadOnly = true;
            this.colFrequency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFrequency.Width = 64;
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
            this.colEndDate.FillWeight = 115F;
            this.colEndDate.HeaderText = "停止时间";
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.ReadOnly = true;
            this.colEndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEndDate.Width = 115;
            // 
            // colDoctor
            // 
            this.colDoctor.FillWeight = 72F;
            this.colDoctor.HeaderText = "医生";
            this.colDoctor.Name = "colDoctor";
            this.colDoctor.ReadOnly = true;
            this.colDoctor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDoctor.Width = 72;
            // 
            // colNurse
            // 
            this.colNurse.FillWeight = 72F;
            this.colNurse.HeaderText = "护士";
            this.colNurse.Name = "colNurse";
            this.colNurse.ReadOnly = true;
            this.colNurse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNurse.Width = 72;
            // 
            // OrderNo
            // 
            this.OrderNo.HeaderText = "OrderNo";
            this.OrderNo.Name = "OrderNo";
            this.OrderNo.ReadOnly = true;
            this.OrderNo.Visible = false;
            // 
            // OrderSubNo
            // 
            this.OrderSubNo.HeaderText = "OrderSubNo";
            this.OrderSubNo.Name = "OrderSubNo";
            this.OrderSubNo.ReadOnly = true;
            this.OrderSubNo.Visible = false;
            // 
            // OrdersImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(889, 510);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnClose);
            this.MinimizeBox = false;
            this.Name = "OrdersImportForm";
            this.SaveWindowState = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "医嘱列表";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripLabel toollblSpace;
        private System.Windows.Forms.ToolStripButton toolbtnQuery;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvOrderType;
        private System.Windows.Forms.ColumnHeader colFilter;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private Heren.Common.Controls.HerenButton btnImport;
        private Heren.Common.Controls.HerenButton btnClose;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Heren.Common.Controls.TableView.DataTableView dataTableView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScheduleTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExecClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPerformedFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperatingTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealDosage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepeatIndicator;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnterDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDosage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDosageUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdministration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrequency;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFreqDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDoctor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNurse;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderSubNo;
    }
}