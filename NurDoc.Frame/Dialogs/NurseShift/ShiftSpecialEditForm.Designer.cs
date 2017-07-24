namespace Heren.NurDoc.Frame.Dialogs
{
    partial class ShiftSpecialEditForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolbtnReturn = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDeleteAllPatient = new Heren.Common.Controls.FlatButton();
            this.dgvShiftPatient = new Heren.Common.Controls.TableView.DataTableView();
            this.colBedCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddPatient = new Heren.Common.Controls.FlatButton();
            this.btnDeletePatient = new Heren.Common.Controls.FlatButton();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShiftPatient)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolbtnSave,
            this.toolbtnReturn});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(785, 32);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(6, 22);
            // 
            // toolbtnSave
            // 
            this.toolbtnSave.AutoSize = false;
            this.toolbtnSave.Image = global::Heren.NurDoc.Frame.Properties.Resources.Save;
            this.toolbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSave.Name = "toolbtnSave";
            this.toolbtnSave.Size = new System.Drawing.Size(64, 22);
            this.toolbtnSave.Text = "保存";
            this.toolbtnSave.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // toolbtnReturn
            // 
            this.toolbtnReturn.AutoSize = false;
            this.toolbtnReturn.Image = global::Heren.NurDoc.Frame.Properties.Resources.Return;
            this.toolbtnReturn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnReturn.Name = "toolbtnReturn";
            this.toolbtnReturn.Size = new System.Drawing.Size(88, 22);
            this.toolbtnReturn.Text = "记录列表";
            this.toolbtnReturn.Click += new System.EventHandler(this.toolbtnReturn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.formControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(290, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 470);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "交班信息";
            // 
            // formControl1
            // 
            this.formControl1.AutoScroll = false;
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(3, 17);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(489, 450);
            this.formControl1.TabIndex = 1;
            this.formControl1.Text = "formControl1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDeleteAllPatient);
            this.groupBox2.Controls.Add(this.dgvShiftPatient);
            this.groupBox2.Controls.Add(this.btnAddPatient);
            this.groupBox2.Controls.Add(this.btnDeletePatient);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 470);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "交班病人";
            // 
            // btnDeleteAllPatient
            // 
            this.btnDeleteAllPatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteAllPatient.Location = new System.Drawing.Point(266, 102);
            this.btnDeleteAllPatient.Name = "btnDeleteAllPatient";
            this.btnDeleteAllPatient.Size = new System.Drawing.Size(22, 22);
            this.btnDeleteAllPatient.TabIndex = 3;
            this.btnDeleteAllPatient.ToolTipText = null;
            this.btnDeleteAllPatient.Click += new System.EventHandler(this.btnDeleteAllPatient_Click);
            // 
            // dgvShiftPatient
            // 
            this.dgvShiftPatient.AllowUserToOrderColumns = true;
            this.dgvShiftPatient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShiftPatient.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvShiftPatient.ColumnHeadersHeight = 24;
            this.dgvShiftPatient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBedCode,
            this.colPatientName,
            this.colID});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvShiftPatient.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvShiftPatient.Location = new System.Drawing.Point(6, 17);
            this.dgvShiftPatient.MultiSelect = false;
            this.dgvShiftPatient.Name = "dgvShiftPatient";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShiftPatient.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvShiftPatient.RowHeadersVisible = false;
            this.dgvShiftPatient.RowHeadersWidth = 30;
            this.dgvShiftPatient.Size = new System.Drawing.Size(254, 444);
            this.dgvShiftPatient.TabIndex = 0;
            this.dgvShiftPatient.SelectionChanged += new System.EventHandler(this.dgvShiftPatient_SelectionChanged);
            // 
            // colBedCode
            // 
            this.colBedCode.FillWeight = 40F;
            this.colBedCode.HeaderText = "床号";
            this.colBedCode.Name = "colBedCode";
            this.colBedCode.ReadOnly = true;
            this.colBedCode.Width = 60;
            // 
            // colPatientName
            // 
            this.colPatientName.FillWeight = 80F;
            this.colPatientName.HeaderText = "姓名";
            this.colPatientName.Name = "colPatientName";
            this.colPatientName.ReadOnly = true;
            this.colPatientName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPatientName.Width = 70;
            // 
            // colID
            // 
            this.colID.HeaderText = "ID号";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            // 
            // btnAddPatient
            // 
            this.btnAddPatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPatient.Location = new System.Drawing.Point(266, 44);
            this.btnAddPatient.Name = "btnAddPatient";
            this.btnAddPatient.Size = new System.Drawing.Size(22, 22);
            this.btnAddPatient.TabIndex = 1;
            this.btnAddPatient.ToolTipText = null;
            this.btnAddPatient.Click += new System.EventHandler(this.btnAddPatient_Click);
            // 
            // btnDeletePatient
            // 
            this.btnDeletePatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePatient.Location = new System.Drawing.Point(266, 74);
            this.btnDeletePatient.Name = "btnDeletePatient";
            this.btnDeletePatient.Size = new System.Drawing.Size(22, 22);
            this.btnDeletePatient.TabIndex = 2;
            this.btnDeletePatient.ToolTipText = null;
            this.btnDeletePatient.Click += new System.EventHandler(this.btnDeletePatient_Click);
            // 
            // ShiftSpecialEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 502);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MouseMoveForm = true;
            this.Name = "ShiftSpecialEditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑特殊病人病情交接";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShiftPatient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.ToolStripButton toolbtnReturn;
        private System.Windows.Forms.GroupBox groupBox1;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Heren.Common.Controls.FlatButton btnDeleteAllPatient;
        private Heren.Common.Controls.TableView.DataTableView dgvShiftPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBedCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private Heren.Common.Controls.FlatButton btnAddPatient;
        private Heren.Common.Controls.FlatButton btnDeletePatient;


    }
}