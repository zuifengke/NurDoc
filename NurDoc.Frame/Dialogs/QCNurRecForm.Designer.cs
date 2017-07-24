namespace Heren.NurDoc.Frame.Dialogs
{
    partial class QcNurRecForm
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
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboStatus = new System.Windows.Forms.ToolStripComboBox();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRecordTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnQcAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.toolStripLabel5,
            this.toolcboStatus});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(911, 32);
            this.toolStrip1.TabIndex = 25;
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
            this.Status,
            this.colRecordTime});
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(0, 32);
            this.dataTableView1.MultiSelect = false;
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.RowHeadersWidth = 24;
            this.dataTableView1.Size = new System.Drawing.Size(911, 394);
            this.dataTableView1.TabIndex = 26;
            this.dataTableView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTableView1_CellMouseClick);
            // 
            // Status
            // 
            this.Status.HeaderText = "状态";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // colRecordTime
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colRecordTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.colRecordTime.FillWeight = 108F;
            this.colRecordTime.HeaderText = "时间";
            this.colRecordTime.Name = "colRecordTime";
            this.colRecordTime.ReadOnly = true;
            this.colRecordTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRecordTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRecordTime.Width = 108;
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
            this.btnQcAll.Text = "审核全部";
            this.btnQcAll.Click += new System.EventHandler(this.btnQcAll_Click);
            // 
            // QcNurRecForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 426);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "QcNurRecForm";
            this.Text = "QCNurRecForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

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
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox toolcboStatus;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecordTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnQcAll;
    }
}