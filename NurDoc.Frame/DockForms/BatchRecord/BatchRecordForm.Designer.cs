namespace Heren.NurDoc.Frame.DockForms
{
    partial class BatchRecordForm
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
            this.toollblSchema = new System.Windows.Forms.ToolStripLabel();
            this.toolcboSchemaList = new System.Windows.Forms.ToolStripComboBox();
            this.toollblRecordDate = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpRecordDate = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.tooldtpRecordTime = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblRecordTime = new System.Windows.Forms.ToolStripLabel();
            this.toolcboRecordTime = new System.Windows.Forms.ToolStripComboBox();
            this.toollblSpace = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnFilterPatient = new System.Windows.Forms.ToolStripButton();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripButton();
            this.colPatientID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBedNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblSchema,
            this.toolcboSchemaList,
            this.toollblRecordDate,
            this.tooldtpRecordDate,
            this.tooldtpRecordTime,
            this.toollblRecordTime,
            this.toolcboRecordTime,
            this.toollblSpace,
            this.toolbtnFilterPatient,
            this.toolbtnSave,
            this.toolbtnPrint});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(866, 35);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toollblSchema
            // 
            this.toollblSchema.AutoSize = false;
            this.toollblSchema.Name = "toollblSchema";
            this.toollblSchema.Size = new System.Drawing.Size(88, 22);
            this.toollblSchema.Text = "列显示方案：";
            // 
            // toolcboSchemaList
            // 
            this.toolcboSchemaList.AutoSize = false;
            this.toolcboSchemaList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboSchemaList.MaxDropDownItems = 16;
            this.toolcboSchemaList.Name = "toolcboSchemaList";
            this.toolcboSchemaList.Size = new System.Drawing.Size(120, 25);
            this.toolcboSchemaList.SelectedIndexChanged += new System.EventHandler(this.toolcboSchemaList_SelectedIndexChanged);
            // 
            // toollblRecordDate
            // 
            this.toollblRecordDate.AutoSize = false;
            this.toollblRecordDate.Name = "toollblRecordDate";
            this.toollblRecordDate.Size = new System.Drawing.Size(72, 22);
            this.toollblRecordDate.Text = "记录日期：";
            // 
            // tooldtpRecordDate
            // 
            this.tooldtpRecordDate.AutoSize = false;
            this.tooldtpRecordDate.BackColor = System.Drawing.Color.White;
            this.tooldtpRecordDate.Name = "tooldtpRecordDate";
            this.tooldtpRecordDate.ShowHour = false;
            this.tooldtpRecordDate.ShowMinute = false;
            this.tooldtpRecordDate.ShowSecond = false;
            this.tooldtpRecordDate.Size = new System.Drawing.Size(100, 22);
            this.tooldtpRecordDate.Text = "2012/02/20 15:30:40";
            this.tooldtpRecordDate.Value = new System.DateTime(2012, 2, 20, 15, 30, 40, 0);
            this.tooldtpRecordDate.ValueChanged += new System.EventHandler(this.tooldtpRecordDate_ValueChanged);
            // 
            // tooldtpRecordTime
            // 
            this.tooldtpRecordTime.AutoSize = false;
            this.tooldtpRecordTime.BackColor = System.Drawing.Color.White;
            this.tooldtpRecordTime.Name = "tooldtpRecordTime";
            this.tooldtpRecordTime.ShowSecond = false;
            this.tooldtpRecordTime.Size = new System.Drawing.Size(140, 22);
            this.tooldtpRecordTime.Text = "2012/02/20 10:14:00";
            this.tooldtpRecordTime.Value = new System.DateTime(2012, 2, 20, 10, 14, 00, 0);
            this.tooldtpRecordTime.ValueChanged += new System.EventHandler(this.tooldtpRecordTime_ValueChanged);
            // 
            // toollblRecordTime
            // 
            this.toollblRecordTime.AutoSize = false;
            this.toollblRecordTime.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toollblRecordTime.Name = "toollblRecordTime";
            this.toollblRecordTime.Size = new System.Drawing.Size(64, 22);
            this.toollblRecordTime.Text = "时间点：";
            // 
            // toolcboRecordTime
            // 
            this.toolcboRecordTime.AutoSize = false;
            this.toolcboRecordTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcboRecordTime.MaxDropDownItems = 16;
            this.toolcboRecordTime.Name = "toolcboRecordTime";
            this.toolcboRecordTime.Size = new System.Drawing.Size(64, 25);
            this.toolcboRecordTime.SelectedIndexChanged += new System.EventHandler(this.toolcboRecordTime_SelectedIndexChanged);
            // 
            // toollblSpace
            // 
            this.toollblSpace.AutoSize = false;
            this.toollblSpace.Name = "toollblSpace";
            this.toollblSpace.Size = new System.Drawing.Size(8, 22);
            // 
            // toolbtnFilterPatient
            // 
            this.toolbtnFilterPatient.AutoSize = false;
            this.toolbtnFilterPatient.Image = global::Heren.NurDoc.Frame.Properties.Resources.Persons;
            this.toolbtnFilterPatient.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnFilterPatient.Name = "toolbtnFilterPatient";
            this.toolbtnFilterPatient.Size = new System.Drawing.Size(64, 22);
            this.toolbtnFilterPatient.Text = "筛选";
            this.toolbtnFilterPatient.Click += new System.EventHandler(this.toolbtnFilterPatient_Click);
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
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.Image = global::Heren.NurDoc.Frame.Properties.Resources.Print;
            this.toolbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnPrint.Name = "toolbtnPrint";
            this.toolbtnPrint.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrint.Text = "打印";
            this.toolbtnPrint.Click += new System.EventHandler(this.toolbtnPrint_Click);
            // 
            // colPatientID
            // 
            this.colPatientID.FillWeight = 72F;
            this.colPatientID.Frozen = true;
            this.colPatientID.HeaderText = "ID号";
            this.colPatientID.Name = "colPatientID";
            this.colPatientID.Width = 72;
            // 
            // colPatientName
            // 
            this.colPatientName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPatientName.FillWeight = 56F;
            this.colPatientName.Frozen = true;
            this.colPatientName.HeaderText = "姓名";
            this.colPatientName.Name = "colPatientName";
            this.colPatientName.Width = 56;
            // 
            // colBedNo
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colBedNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colBedNo.FillWeight = 60F;
            this.colBedNo.Frozen = true;
            this.colBedNo.HeaderText = "床号";
            this.colBedNo.Name = "colBedNo";
            this.colBedNo.Width = 60;
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTableView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataTableView1.ColumnHeadersHeight = 40;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBedNo,
            this.colPatientName,
            this.colPatientID});
            this.dataTableView1.Location = new System.Drawing.Point(1, 38);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(864, 462);
            this.dataTableView1.TabIndex = 1;
            this.dataTableView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataTableView1_CellBeginEdit);
            this.dataTableView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTableView1_ColumnHeaderMouseClick);
            this.dataTableView1.NumericValidating += new Heren.Common.Controls.TableView.NumericValidatingEventHandler(this.dataTableView1_NumericValidating);
            this.dataTableView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataTableView1_CellValidating);
            this.dataTableView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellEndEdit);
            this.dataTableView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellEnter);
            // 
            // formControl1
            // 
            this.formControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.formControl1.Location = new System.Drawing.Point(1, 38);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(864, 462);
            this.formControl1.TabIndex = 2;
            this.formControl1.Text = "formControl1";
            this.formControl1.QueryContext += new Heren.Common.Forms.Editor.QueryContextEventHandler(this.formControl1_QueryContext);
            // 
            // BatchRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(866, 501);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.formControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "BatchRecordForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblSchema;
        private System.Windows.Forms.ToolStripComboBox toolcboSchemaList;
        private System.Windows.Forms.ToolStripLabel toollblRecordDate;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpRecordDate;
        private System.Windows.Forms.ToolStripComboBox toolcboRecordTime;
        private System.Windows.Forms.ToolStripButton toolbtnFilterPatient;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.ToolStripLabel toollblSpace;
        private System.Windows.Forms.ToolStripButton toolbtnPrint;
        private System.Windows.Forms.ToolStripLabel toollblRecordTime;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpRecordTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBedNo;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
    }
}

