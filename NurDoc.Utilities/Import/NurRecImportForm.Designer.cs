namespace Heren.NurDoc.Utilities.Import
{
    partial class NurRecImportForm
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
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colCheckNurRec = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRecordTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRecordContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRecorderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnImport = new Heren.Common.Controls.HerenButton();
            this.btnClose = new Heren.Common.Controls.HerenButton();
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
            this.toolStrip1.Size = new System.Drawing.Size(716, 32);
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
            this.tooldtpDateFrom.Text = "2012/2/20 0:00:00";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 0, 0, 0, 0);
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
            this.tooldtpDateTo.Text = "2012/2/20 0:00:00";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 0, 0, 0, 0);
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
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTableView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckNurRec,
            this.colRecordTime,
            this.colRecordContent,
            this.colRecorderName});
            this.dataTableView1.Location = new System.Drawing.Point(2, 32);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.Size = new System.Drawing.Size(712, 396);
            this.dataTableView1.TabIndex = 1;
            this.dataTableView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTableView1_CellMouseClick);
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            this.dataTableView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellValueChanged);
            // 
            // colCheckNurRec
            // 
            this.colCheckNurRec.FillWeight = 28F;
            this.colCheckNurRec.Frozen = true;
            this.colCheckNurRec.HeaderText = string.Empty;
            this.colCheckNurRec.Name = "colCheckNurRec";
            this.colCheckNurRec.ReadOnly = true;
            this.colCheckNurRec.Width = 28;
            // 
            // colRecordTime
            // 
            this.colRecordTime.FillWeight = 115F;
            this.colRecordTime.Frozen = true;
            this.colRecordTime.HeaderText = "记录时间";
            this.colRecordTime.Name = "colRecordTime";
            this.colRecordTime.ReadOnly = true;
            this.colRecordTime.Width = 115;
            // 
            // colRecordContent
            // 
            this.colRecordContent.FillWeight = 480F;
            this.colRecordContent.HeaderText = "护理记录内容";
            this.colRecordContent.Name = "colRecordContent";
            this.colRecordContent.ReadOnly = true;
            this.colRecordContent.Width = 480;
            // 
            // colRecorderName
            // 
            this.colRecorderName.FillWeight = 64F;
            this.colRecorderName.HeaderText = "记录人";
            this.colRecorderName.Name = "colRecorderName";
            this.colRecorderName.ReadOnly = true;
            this.colRecorderName.Width = 64;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(20, 442);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll.TabIndex = 2;
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
            this.btnImport.Location = new System.Drawing.Point(501, 437);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(88, 24);
            this.btnImport.TabIndex = 3;
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
            this.btnClose.Location = new System.Drawing.Point(609, 437);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 24);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // NurRecImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(716, 470);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnClose);
            this.MinimizeBox = false;
            this.Name = "NurRecImportForm";
            this.SaveWindowState = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "护理记录列表";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
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
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckNurRec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecordTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecordContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecorderName;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private Heren.Common.Controls.HerenButton btnImport;
        private Heren.Common.Controls.HerenButton btnClose;
    }
}