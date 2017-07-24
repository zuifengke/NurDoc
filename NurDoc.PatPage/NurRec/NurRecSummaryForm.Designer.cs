namespace Heren.NurDoc.PatPage.NurRec
{
    partial class NurRecSummaryForm
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
            this.lblSummaryName = new System.Windows.Forms.Label();
            this.cboSummaryName = new System.Windows.Forms.ComboBox();
            this.lblRecordTime = new System.Windows.Forms.Label();
            this.dtpRecordTime = new Heren.Common.Controls.TimeControl.DateTimeControl();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.dtpStartTime = new Heren.Common.Controls.TimeControl.DateTimeControl();
            this.lblEnterDetails = new System.Windows.Forms.Label();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colEnterTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblOutDetails = new System.Windows.Forms.Label();
            this.dataTableView2 = new Heren.Common.Controls.TableView.DataTableView();
            this.colOutTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblModifyTip = new System.Windows.Forms.Label();
            this.lblEnterTotal = new System.Windows.Forms.Label();
            this.lblOutTotal = new System.Windows.Forms.Label();
            this.lblSummaryDesc = new System.Windows.Forms.Label();
            this.btnTextTemplet = new Heren.Common.Controls.FlatButton();
            this.txtSummaryDesc = new System.Windows.Forms.RichTextBox();
            this.separateStrip1 = new Heren.Common.Controls.SeparateStrip();
            this.btnSave = new Heren.Common.Controls.HerenButton();
            this.btnClose = new Heren.Common.Controls.HerenButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSummaryName
            // 
            this.lblSummaryName.Location = new System.Drawing.Point(6, 12);
            this.lblSummaryName.Name = "lblSummaryName";
            this.lblSummaryName.Size = new System.Drawing.Size(65, 12);
            this.lblSummaryName.TabIndex = 0;
            this.lblSummaryName.Text = "小结名称：";
            // 
            // cboSummaryName
            // 
            this.cboSummaryName.FormattingEnabled = true;
            this.cboSummaryName.Items.AddRange(new object[] {
            "12小时小结",
            "24小时总结",
            "日间小结",
            "23小时总结",
            "22小时总结",
            "21小时总结",
            "20小时总结",
            "19小时总结",
            "18小时总结",
            "17小时总结",
            "16小时总结",
            "15小时总结",
            "14小时总结",
            "13小时总结",
            "11小时小结",
            "10小时小结",
            "9小时小结",
            "8小时小结",
            "7小时小结",
            "6小时小结"});
            this.cboSummaryName.Location = new System.Drawing.Point(75, 8);
            this.cboSummaryName.MaxDropDownItems = 16;
            this.cboSummaryName.Name = "cboSummaryName";
            this.cboSummaryName.Size = new System.Drawing.Size(130, 20);
            this.cboSummaryName.TabIndex = 1;
            this.cboSummaryName.SelectedIndexChanged += new System.EventHandler(this.cboSummaryName_SelectedIndexChanged);
            this.cboSummaryName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboSummaryName_KeyDown);
            // 
            // lblRecordTime
            // 
            this.lblRecordTime.Location = new System.Drawing.Point(224, 12);
            this.lblRecordTime.Name = "lblRecordTime";
            this.lblRecordTime.Size = new System.Drawing.Size(65, 12);
            this.lblRecordTime.TabIndex = 2;
            this.lblRecordTime.Text = "记录时间：";
            // 
            // dtpRecordTime
            // 
            this.dtpRecordTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dtpRecordTime.Location = new System.Drawing.Point(295, 7);
            this.dtpRecordTime.Name = "dtpRecordTime";
            this.dtpRecordTime.ShowSecond = false;
            this.dtpRecordTime.Size = new System.Drawing.Size(140, 21);
            this.dtpRecordTime.TabIndex = 3;
            this.dtpRecordTime.ValueChanged += new System.EventHandler(this.dtpRecordTime_ValueChanged);
            // 
            // lblStartTime
            // 
            this.lblStartTime.Location = new System.Drawing.Point(461, 12);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(89, 12);
            this.lblStartTime.TabIndex = 12;
            this.lblStartTime.Text = "统计起始时间：";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dtpStartTime.Location = new System.Drawing.Point(556, 8);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowSecond = false;
            this.dtpStartTime.Size = new System.Drawing.Size(137, 21);
            this.dtpStartTime.TabIndex = 4;
            this.dtpStartTime.ValueChanged += new System.EventHandler(this.dtpStartTime_ValueChanged);
            // 
            // lblEnterDetails
            // 
            this.lblEnterDetails.Location = new System.Drawing.Point(6, 38);
            this.lblEnterDetails.Name = "lblEnterDetails";
            this.lblEnterDetails.Size = new System.Drawing.Size(65, 12);
            this.lblEnterDetails.TabIndex = 5;
            this.lblEnterDetails.Text = "入量明细：";
            // 
            // dataTableView1
            // 
            this.dataTableView1.BackgroundColor = System.Drawing.Color.White;
            this.dataTableView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataTableView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataTableView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEnterTime,
            this.colEnterName,
            this.colEnterValue});
            this.dataTableView1.Location = new System.Drawing.Point(6, 53);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTableView1.Size = new System.Drawing.Size(340, 244);
            this.dataTableView1.TabIndex = 6;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            // 
            // colEnterTime
            // 
            this.colEnterTime.FillWeight = 122F;
            this.colEnterTime.HeaderText = "时间";
            this.colEnterTime.Name = "colEnterTime";
            this.colEnterTime.ReadOnly = true;
            this.colEnterTime.Width = 122;
            // 
            // colEnterName
            // 
            this.colEnterName.FillWeight = 146F;
            this.colEnterName.HeaderText = "名称";
            this.colEnterName.Name = "colEnterName";
            this.colEnterName.ReadOnly = true;
            this.colEnterName.Width = 146;
            // 
            // colEnterValue
            // 
            this.colEnterValue.FillWeight = 48F;
            this.colEnterValue.HeaderText = "量";
            this.colEnterValue.Name = "colEnterValue";
            this.colEnterValue.ReadOnly = true;
            this.colEnterValue.Width = 48;
            // 
            // lblOutDetails
            // 
            this.lblOutDetails.Location = new System.Drawing.Point(356, 38);
            this.lblOutDetails.Name = "lblOutDetails";
            this.lblOutDetails.Size = new System.Drawing.Size(65, 12);
            this.lblOutDetails.TabIndex = 7;
            this.lblOutDetails.Text = "出量明细：";
            // 
            // dataTableView2
            // 
            this.dataTableView2.BackgroundColor = System.Drawing.Color.White;
            this.dataTableView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataTableView2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataTableView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTableView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOutTime,
            this.colOutName,
            this.colOutValue});
            this.dataTableView2.Location = new System.Drawing.Point(356, 53);
            this.dataTableView2.Name = "dataTableView2";
            this.dataTableView2.ReadOnly = true;
            this.dataTableView2.RowHeadersVisible = false;
            this.dataTableView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTableView2.Size = new System.Drawing.Size(337, 244);
            this.dataTableView2.TabIndex = 8;
            this.dataTableView2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView2_CellDoubleClick);
            // 
            // colOutTime
            // 
            this.colOutTime.FillWeight = 122F;
            this.colOutTime.HeaderText = "时间";
            this.colOutTime.Name = "colOutTime";
            this.colOutTime.ReadOnly = true;
            this.colOutTime.Width = 122;
            // 
            // colOutName
            // 
            this.colOutName.FillWeight = 146F;
            this.colOutName.HeaderText = "名称";
            this.colOutName.Name = "colOutName";
            this.colOutName.ReadOnly = true;
            this.colOutName.Width = 146;
            // 
            // colOutValue
            // 
            this.colOutValue.FillWeight = 48F;
            this.colOutValue.HeaderText = "量";
            this.colOutValue.Name = "colOutValue";
            this.colOutValue.ReadOnly = true;
            this.colOutValue.Width = 48;
            // 
            // lblModifyTip
            // 
            this.lblModifyTip.ForeColor = System.Drawing.Color.Red;
            this.lblModifyTip.Location = new System.Drawing.Point(6, 303);
            this.lblModifyTip.Name = "lblModifyTip";
            this.lblModifyTip.Size = new System.Drawing.Size(167, 12);
            this.lblModifyTip.TabIndex = 9;
            this.lblModifyTip.Text = "※如需修改明细,请返回原记录";
            // 
            // lblEnterTotal
            // 
            this.lblEnterTotal.ForeColor = System.Drawing.Color.Blue;
            this.lblEnterTotal.Location = new System.Drawing.Point(197, 303);
            this.lblEnterTotal.Name = "lblEnterTotal";
            this.lblEnterTotal.Size = new System.Drawing.Size(149, 12);
            this.lblEnterTotal.TabIndex = 10;
            this.lblEnterTotal.Text = "入量总计：2300";
            this.lblEnterTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutTotal
            // 
            this.lblOutTotal.ForeColor = System.Drawing.Color.Blue;
            this.lblOutTotal.Location = new System.Drawing.Point(550, 303);
            this.lblOutTotal.Name = "lblOutTotal";
            this.lblOutTotal.Size = new System.Drawing.Size(143, 12);
            this.lblOutTotal.TabIndex = 11;
            this.lblOutTotal.Text = "出量总计：300";
            this.lblOutTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSummaryDesc
            // 
            this.lblSummaryDesc.Location = new System.Drawing.Point(6, 325);
            this.lblSummaryDesc.Name = "lblSummaryDesc";
            this.lblSummaryDesc.Size = new System.Drawing.Size(65, 12);
            this.lblSummaryDesc.TabIndex = 12;
            this.lblSummaryDesc.Text = "小结说明：";
            // 
            // btnTextTemplet
            // 
            this.btnTextTemplet.Location = new System.Drawing.Point(614, 321);
            this.btnTextTemplet.Name = "btnTextTemplet";
            this.btnTextTemplet.Size = new System.Drawing.Size(79, 21);
            this.btnTextTemplet.TabIndex = 13;
            this.btnTextTemplet.Text = "导入文本模板";
            this.btnTextTemplet.ToolTipText = null;
            this.btnTextTemplet.Click += new System.EventHandler(this.btnTextTemplet_Click);
            // 
            // txtSummaryDesc
            // 
            this.txtSummaryDesc.Location = new System.Drawing.Point(6, 343);
            this.txtSummaryDesc.Name = "txtSummaryDesc";
            this.txtSummaryDesc.Size = new System.Drawing.Size(687, 51);
            this.txtSummaryDesc.TabIndex = 14;
            this.txtSummaryDesc.Text = string.Empty;
            // 
            // separateStrip1
            // 
            this.separateStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.separateStrip1.ForeColor = System.Drawing.Color.MediumBlue;
            this.separateStrip1.Location = new System.Drawing.Point(6, 394);
            this.separateStrip1.Name = "separateStrip1";
            this.separateStrip1.Size = new System.Drawing.Size(687, 11);
            this.separateStrip1.TabIndex = 15;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(483, 408);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 24);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(592, 408);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 24);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // NurRecSummaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(700, 440);
            this.Controls.Add(this.lblSummaryName);
            this.Controls.Add(this.cboSummaryName);
            this.Controls.Add(this.lblRecordTime);
            this.Controls.Add(this.dtpRecordTime);
            this.Controls.Add(this.lblStartTime);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.lblEnterDetails);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.lblOutDetails);
            this.Controls.Add(this.dataTableView2);
            this.Controls.Add(this.lblModifyTip);
            this.Controls.Add(this.lblEnterTotal);
            this.Controls.Add(this.lblOutTotal);
            this.Controls.Add(this.lblSummaryDesc);
            this.Controls.Add(this.btnTextTemplet);
            this.Controls.Add(this.txtSummaryDesc);
            this.Controls.Add(this.separateStrip1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NurRecSummaryForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑护理记录小结";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView2)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label lblSummaryName;
        private System.Windows.Forms.ComboBox cboSummaryName;
        private System.Windows.Forms.Label lblRecordTime;
        private Heren.Common.Controls.TimeControl.DateTimeControl dtpRecordTime;
        private System.Windows.Forms.Label lblStartTime;
        private Heren.Common.Controls.TimeControl.DateTimeControl dtpStartTime;
        private System.Windows.Forms.Label lblEnterDetails;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.Label lblOutDetails;
        private Heren.Common.Controls.TableView.DataTableView dataTableView2;
        private System.Windows.Forms.Label lblModifyTip;
        private System.Windows.Forms.Label lblEnterTotal;
        private System.Windows.Forms.Label lblOutTotal;
        private System.Windows.Forms.Label lblSummaryDesc;
        private Heren.Common.Controls.FlatButton btnTextTemplet;
        private System.Windows.Forms.RichTextBox txtSummaryDesc;
        private Heren.Common.Controls.SeparateStrip separateStrip1;
        private Heren.Common.Controls.HerenButton btnSave;
        private Heren.Common.Controls.HerenButton btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnterTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnterValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutValue;
    }
}