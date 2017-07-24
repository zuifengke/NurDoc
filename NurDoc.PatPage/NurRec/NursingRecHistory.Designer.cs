namespace Heren.NurDoc.PatPage.NurRec
{
    partial class NursingRecHistory
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
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colRecordTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtnSchemaList = new System.Windows.Forms.ToolStripLabel();
            this.toolcboSchemaList = new System.Windows.Forms.ToolStripComboBox();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboPatDeptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTableView1
            // 
            this.dataTableView1.AllowUserToResizeRows = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataTableView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataTableView1.ColumnHeadersHeight = 32;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRecordTime});
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(0, 32);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersWidth = 24;
            this.dataTableView1.Size = new System.Drawing.Size(943, 292);
            this.dataTableView1.TabIndex = 4;
            // 
            // colRecordTime
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colRecordTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.colRecordTime.FillWeight = 108F;
            this.colRecordTime.Frozen = true;
            this.colRecordTime.HeaderText = "时间";
            this.colRecordTime.Name = "colRecordTime";
            this.colRecordTime.ReadOnly = true;
            this.colRecordTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRecordTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRecordTime.Width = 108;
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
            this.toolStripLabel2});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(943, 32);
            this.toolStrip1.TabIndex = 3;
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
            this.tooldtpDateFrom.Text = "2012/2/20 14:47:03";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 14, 47, 3, 0);
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
            this.tooldtpDateTo.Text = "2012/2/20 14:47:03";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 14, 47, 3, 0);
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
            this.toolStripLabel2.Size = new System.Drawing.Size(11, 22);
            // 
            // NursingRecHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 324);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "NursingRecHistory";
            this.Text = "NursingRecHistory";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecordTime;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolbtnSchemaList;
        private System.Windows.Forms.ToolStripComboBox toolcboSchemaList;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolcboPatDeptList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    }
}