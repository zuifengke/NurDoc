namespace Heren.NurDoc.Frame.Dialogs
{
    partial class ShiftStatusEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShiftStatusEditForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpShiftDate = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcboShiftRank = new System.Windows.Forms.ToolStripComboBox();
            this.toollblRankTime = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolbtnReturn = new System.Windows.Forms.ToolStripButton();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.toolStrip1.SuspendLayout();
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
            this.toolbtnReturn});
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
            this.tooldtpShiftDate.Enabled = false;
            this.tooldtpShiftDate.Name = "tooldtpShiftDate";
            this.tooldtpShiftDate.ShowHour = false;
            this.tooldtpShiftDate.ShowMinute = false;
            this.tooldtpShiftDate.ShowSecond = false;
            this.tooldtpShiftDate.Size = new System.Drawing.Size(100, 22);
            this.tooldtpShiftDate.Text = "2012/02/20 17:13:31";
            this.tooldtpShiftDate.Value = new System.DateTime(2012, 2, 20, 17, 13, 31, 0);
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
            this.toolcboShiftRank.Enabled = false;
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
            // formControl1
            // 
            this.formControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.formControl1.AutoScroll = false;
            this.formControl1.Location = new System.Drawing.Point(2, 37);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(820, 212);
            this.formControl1.TabIndex = 3;
            // 
            // ShiftStatusEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 254);
            this.Controls.Add(this.formControl1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShiftStatusEditForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑交班记录";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
    }
}