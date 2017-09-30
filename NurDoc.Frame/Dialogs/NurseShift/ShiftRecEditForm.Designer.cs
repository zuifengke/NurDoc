namespace Heren.NurDoc.Frame.Dialogs
{
    partial class ShiftRecEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShiftRecEditForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpShiftDate = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboShiftRank = new System.Windows.Forms.ToolStripComboBox();
            this.toollblRankTime = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolbtnReturn = new System.Windows.Forms.ToolStripButton();
            this.toolbtnDelete = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picBoxWhite = new System.Windows.Forms.PictureBox();
            this.picBoxBlue = new System.Windows.Forms.PictureBox();
            this.btnDeleteAllPatient = new Heren.Common.Controls.FlatButton();
            this.dgvShiftPatient = new Heren.Common.Controls.TableView.DataTableView();
            this.btnAddPatient = new Heren.Common.Controls.FlatButton();
            this.btnDeletePatient = new Heren.Common.Controls.FlatButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.formControl2 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.colNewPatient = new System.Windows.Forms.DataGridViewImageColumn();
            this.colShiftItem = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colShiftItemAlias = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colBedCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWhite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShiftPatient)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblDateFrom,
            this.tooldtpShiftDate,
            this.toolStripLabel1,
            this.toolcboShiftRank,
            this.toollblRankTime,
            this.toolbtnSave,
            this.toolbtnReturn,
            this.toolbtnDelete});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(826, 32);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toollblDateFrom
            // 
            this.toollblDateFrom.AutoSize = false;
            this.toollblDateFrom.Name = "toollblDateFrom";
            this.toollblDateFrom.Size = new System.Drawing.Size(72, 22);
            this.toollblDateFrom.Text = "交班日期：";
            // 
            // tooldtpShiftDate
            // 
            this.tooldtpShiftDate.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tooldtpShiftDate.AutoSize = false;
            this.tooldtpShiftDate.BackColor = System.Drawing.Color.White;
            this.tooldtpShiftDate.Name = "tooldtpShiftDate";
            this.tooldtpShiftDate.ShowHour = false;
            this.tooldtpShiftDate.ShowMinute = false;
            this.tooldtpShiftDate.ShowSecond = false;
            this.tooldtpShiftDate.Size = new System.Drawing.Size(100, 22);
            this.tooldtpShiftDate.Text = "2012/2/20 9:54:47";
            this.tooldtpShiftDate.Value = new System.DateTime(2012, 2, 20, 9, 54, 47, 0);
            this.tooldtpShiftDate.ValueChanged += new System.EventHandler(this.tooldtpShiftDate_ValueChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(6, 22);
            // 
            // toolcboShiftRank
            // 
            this.toolcboShiftRank.AutoSize = false;
            this.toolcboShiftRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboShiftRank.MaxDropDownItems = 16;
            this.toolcboShiftRank.Name = "toolcboShiftRank";
            this.toolcboShiftRank.Size = new System.Drawing.Size(75, 25);
            this.toolcboShiftRank.SelectedIndexChanged += new System.EventHandler(this.toolcboShiftRank_SelectedIndexChanged);
            // 
            // toollblRankTime
            // 
            this.toollblRankTime.AutoSize = false;
            this.toollblRankTime.Name = "toollblRankTime";
            this.toollblRankTime.Size = new System.Drawing.Size(90, 22);
            this.toollblRankTime.Text = "8:00 - 16:00";
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
            // toolbtnDelete
            // 
            this.toolbtnDelete.Image = global::Heren.NurDoc.Frame.Properties.Resources.Delete;
            this.toolbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnDelete.Name = "toolbtnDelete";
            this.toolbtnDelete.Size = new System.Drawing.Size(148, 29);
            this.toolbtnDelete.Text = "删除当前班次交班信息";
            this.toolbtnDelete.Click += new System.EventHandler(this.toolbtnDelete_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(2, 34);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(826, 616);
            this.splitContainer1.SplitterDistance = 109;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.formControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(826, 109);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "病区动态";
            // 
            // formControl1
            // 
            this.formControl1.AutoScroll = false;
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(3, 17);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(820, 89);
            this.formControl1.TabIndex = 2;
            this.formControl1.QueryContext += new Heren.Common.Forms.Editor.QueryContextEventHandler(this.FormControl_QueryContext);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Size = new System.Drawing.Size(826, 503);
            this.splitContainer2.SplitterDistance = 290;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picBoxWhite);
            this.groupBox2.Controls.Add(this.picBoxBlue);
            this.groupBox2.Controls.Add(this.btnDeleteAllPatient);
            this.groupBox2.Controls.Add(this.dgvShiftPatient);
            this.groupBox2.Controls.Add(this.btnAddPatient);
            this.groupBox2.Controls.Add(this.btnDeletePatient);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 503);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "交班病人";
            // 
            // picBoxWhite
            // 
            this.picBoxWhite.BackColor = System.Drawing.Color.Transparent;
            this.picBoxWhite.Image = ((System.Drawing.Image)(resources.GetObject("picBoxWhite.Image")));
            this.picBoxWhite.Location = new System.Drawing.Point(266, 172);
            this.picBoxWhite.Name = "picBoxWhite";
            this.picBoxWhite.Size = new System.Drawing.Size(10, 10);
            this.picBoxWhite.TabIndex = 4;
            this.picBoxWhite.TabStop = false;
            this.picBoxWhite.Visible = false;
            // 
            // picBoxBlue
            // 
            this.picBoxBlue.BackColor = System.Drawing.Color.Transparent;
            this.picBoxBlue.Image = ((System.Drawing.Image)(resources.GetObject("picBoxBlue.Image")));
            this.picBoxBlue.Location = new System.Drawing.Point(266, 140);
            this.picBoxBlue.Name = "picBoxBlue";
            this.picBoxBlue.Size = new System.Drawing.Size(20, 20);
            this.picBoxBlue.TabIndex = 3;
            this.picBoxBlue.TabStop = false;
            this.picBoxBlue.Visible = false;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShiftPatient.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvShiftPatient.ColumnHeadersHeight = 24;
            this.dgvShiftPatient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNewPatient,
            this.colShiftItem,
            this.colShiftItemAlias,
            this.colBedCode,
            this.colPatientName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvShiftPatient.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvShiftPatient.Location = new System.Drawing.Point(6, 17);
            this.dgvShiftPatient.MultiSelect = false;
            this.dgvShiftPatient.Name = "dgvShiftPatient";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShiftPatient.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvShiftPatient.RowHeadersVisible = false;
            this.dgvShiftPatient.RowHeadersWidth = 30;
            this.dgvShiftPatient.Size = new System.Drawing.Size(254, 477);
            this.dgvShiftPatient.TabIndex = 0;
            this.dgvShiftPatient.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvShiftPatient_EditingControlShowing);
            this.dgvShiftPatient.SelectionChanged += new System.EventHandler(this.dgvShiftPatient_SelectionChanged);
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.formControl2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(532, 503);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "交班信息";
            // 
            // formControl2
            // 
            this.formControl2.AutoScroll = false;
            this.formControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl2.Location = new System.Drawing.Point(3, 17);
            this.formControl2.Name = "formControl2";
            this.formControl2.Size = new System.Drawing.Size(526, 483);
            this.formControl2.TabIndex = 1;
            this.formControl2.Text = "formControl1";
            this.formControl2.QueryContext += new Heren.Common.Forms.Editor.QueryContextEventHandler(this.FormControl_QueryContext);
            // 
            // colNewPatient
            // 
            this.colNewPatient.HeaderText = " ";
            this.colNewPatient.Name = "colNewPatient";
            this.colNewPatient.ReadOnly = true;
            this.colNewPatient.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colNewPatient.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colNewPatient.Width = 15;
            // 
            // colShiftItem
            // 
            this.colShiftItem.DisplayStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colShiftItem.FillWeight = 60F;
            this.colShiftItem.HeaderText = "项目";
            this.colShiftItem.Name = "colShiftItem";
            this.colShiftItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colShiftItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colShiftItem.Width = 50;
            // 
            // colShiftItemAlias
            // 
            this.colShiftItemAlias.DisplayStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colShiftItemAlias.HeaderText = "项目别名";
            this.colShiftItemAlias.Name = "colShiftItemAlias";
            this.colShiftItemAlias.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colShiftItemAlias.Width = 60;
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
            this.colPatientName.Width = 60;
            // 
            // ShiftRecEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 652);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShiftRecEditForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑交班记录";
            this.Load += new System.EventHandler(this.ShiftRecEditForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWhite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShiftPatient)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpShiftDate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolcboShiftRank;
        private System.Windows.Forms.ToolStripLabel toollblRankTime;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.ToolStripButton toolbtnReturn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private Heren.Common.Controls.TableView.DataTableView dgvShiftPatient;
        private Heren.Common.Controls.FlatButton btnAddPatient;
        private Heren.Common.Controls.FlatButton btnDeletePatient;
        private System.Windows.Forms.GroupBox groupBox3;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl2;
        private Heren.Common.Controls.FlatButton btnDeleteAllPatient;
        private System.Windows.Forms.PictureBox picBoxBlue;
        private System.Windows.Forms.PictureBox picBoxWhite;
        private System.Windows.Forms.ToolStripButton toolbtnDelete;
        private System.Windows.Forms.DataGridViewImageColumn colNewPatient;
        private Common.Controls.TableView.ComboBoxColumn colShiftItem;
        private Common.Controls.TableView.ComboBoxColumn colShiftItemAlias;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBedCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientName;
    }
}