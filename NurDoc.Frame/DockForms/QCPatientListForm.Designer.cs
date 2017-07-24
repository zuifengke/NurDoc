namespace Heren.NurDoc.Frame.DockForms
{
    partial class QCPatientListForm
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
            this.ckbPatientStatus = new System.Windows.Forms.CheckBox();
            this.ckbNurseDept = new System.Windows.Forms.CheckBox();
            this.ckbPatientName = new System.Windows.Forms.CheckBox();
            this.ckbPatientID = new System.Windows.Forms.CheckBox();
            this.ckbNursingClass = new System.Windows.Forms.CheckBox();
            this.ckbPatCondition = new System.Windows.Forms.CheckBox();
            this.ckbStartTime = new System.Windows.Forms.CheckBox();
            this.ckbEndTime = new System.Windows.Forms.CheckBox();
            this.cbbPatientStatus = new System.Windows.Forms.ComboBox();
            this.cbbNurseDept = new Heren.Common.Controls.DictInput.FindComboBox();
            this.cbbNursingClass = new System.Windows.Forms.ComboBox();
            this.cbbPatientCondition = new System.Windows.Forms.ComboBox();
            this.dtPStart = new System.Windows.Forms.DateTimePicker();
            this.dtPEnd = new System.Windows.Forms.DateTimePicker();
            this.txtPatientName = new System.Windows.Forms.TextBox();
            this.txtPatientID = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.patInfoList1 = new MedQCSys.Controls.PatInfoList.PatInfoList();
            this.SuspendLayout();
            // 
            // ckbPatientStatus
            // 
            this.ckbPatientStatus.AutoSize = true;
            this.ckbPatientStatus.Checked = true;
            this.ckbPatientStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbPatientStatus.Location = new System.Drawing.Point(13, 13);
            this.ckbPatientStatus.Name = "ckbPatientStatus";
            this.ckbPatientStatus.Size = new System.Drawing.Size(72, 16);
            this.ckbPatientStatus.TabIndex = 0;
            this.ckbPatientStatus.Text = "病人状态";
            this.ckbPatientStatus.UseVisualStyleBackColor = true;
            this.ckbPatientStatus.CheckedChanged += new System.EventHandler(this.ckbPatientStatus_CheckedChanged);
            // 
            // ckbNurseDept
            // 
            this.ckbNurseDept.AutoSize = true;
            this.ckbNurseDept.Checked = true;
            this.ckbNurseDept.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNurseDept.Location = new System.Drawing.Point(12, 38);
            this.ckbNurseDept.Name = "ckbNurseDept";
            this.ckbNurseDept.Size = new System.Drawing.Size(72, 16);
            this.ckbNurseDept.TabIndex = 1;
            this.ckbNurseDept.Text = "护理单元";
            this.ckbNurseDept.UseVisualStyleBackColor = true;
            this.ckbNurseDept.CheckedChanged += new System.EventHandler(this.ckbNurseDept_CheckedChanged);
            // 
            // ckbPatientName
            // 
            this.ckbPatientName.AutoSize = true;
            this.ckbPatientName.Location = new System.Drawing.Point(12, 63);
            this.ckbPatientName.Name = "ckbPatientName";
            this.ckbPatientName.Size = new System.Drawing.Size(72, 16);
            this.ckbPatientName.TabIndex = 2;
            this.ckbPatientName.Text = "患者姓名";
            this.ckbPatientName.UseVisualStyleBackColor = true;
            this.ckbPatientName.CheckedChanged += new System.EventHandler(this.ckbPatientName_CheckedChanged);
            // 
            // ckbPatientID
            // 
            this.ckbPatientID.AutoSize = true;
            this.ckbPatientID.Location = new System.Drawing.Point(12, 89);
            this.ckbPatientID.Name = "ckbPatientID";
            this.ckbPatientID.Size = new System.Drawing.Size(72, 16);
            this.ckbPatientID.TabIndex = 3;
            this.ckbPatientID.Text = "病人ID号";
            this.ckbPatientID.UseVisualStyleBackColor = true;
            this.ckbPatientID.CheckedChanged += new System.EventHandler(this.ckbPatientID_CheckedChanged);
            // 
            // ckbNursingClass
            // 
            this.ckbNursingClass.AutoSize = true;
            this.ckbNursingClass.Location = new System.Drawing.Point(12, 115);
            this.ckbNursingClass.Name = "ckbNursingClass";
            this.ckbNursingClass.Size = new System.Drawing.Size(72, 16);
            this.ckbNursingClass.TabIndex = 4;
            this.ckbNursingClass.Text = "护理等级";
            this.ckbNursingClass.UseVisualStyleBackColor = true;
            this.ckbNursingClass.CheckedChanged += new System.EventHandler(this.ckbNursingClass_CheckedChanged);
            // 
            // ckbPatCondition
            // 
            this.ckbPatCondition.AutoSize = true;
            this.ckbPatCondition.Location = new System.Drawing.Point(12, 140);
            this.ckbPatCondition.Name = "ckbPatCondition";
            this.ckbPatCondition.Size = new System.Drawing.Size(48, 16);
            this.ckbPatCondition.TabIndex = 5;
            this.ckbPatCondition.Text = "病情";
            this.ckbPatCondition.UseVisualStyleBackColor = true;
            this.ckbPatCondition.CheckedChanged += new System.EventHandler(this.ckbPatCondition_CheckedChanged);
            // 
            // ckbStartTime
            // 
            this.ckbStartTime.AutoSize = true;
            this.ckbStartTime.Location = new System.Drawing.Point(12, 170);
            this.ckbStartTime.Name = "ckbStartTime";
            this.ckbStartTime.Size = new System.Drawing.Size(72, 16);
            this.ckbStartTime.TabIndex = 6;
            this.ckbStartTime.Text = "起始时间";
            this.ckbStartTime.UseVisualStyleBackColor = true;
            this.ckbStartTime.CheckedChanged += new System.EventHandler(this.ckbStartTime_CheckedChanged);
            // 
            // ckbEndTime
            // 
            this.ckbEndTime.AutoSize = true;
            this.ckbEndTime.Location = new System.Drawing.Point(12, 196);
            this.ckbEndTime.Name = "ckbEndTime";
            this.ckbEndTime.Size = new System.Drawing.Size(72, 16);
            this.ckbEndTime.TabIndex = 7;
            this.ckbEndTime.Text = "结束时间";
            this.ckbEndTime.UseVisualStyleBackColor = true;
            this.ckbEndTime.CheckedChanged += new System.EventHandler(this.ckbEndTime_CheckedChanged);
            // 
            // cbbPatientStatus
            // 
            this.cbbPatientStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbPatientStatus.FormattingEnabled = true;
            this.cbbPatientStatus.Items.AddRange(new object[] {
            "所有病人",
            "在院病人",
            "出院病人"});
            this.cbbPatientStatus.Location = new System.Drawing.Point(96, 13);
            this.cbbPatientStatus.Name = "cbbPatientStatus";
            this.cbbPatientStatus.Size = new System.Drawing.Size(126, 20);
            this.cbbPatientStatus.TabIndex = 8;
            // 
            // cbbNurseDept
            // 
            this.cbbNurseDept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbNurseDept.Location = new System.Drawing.Point(96, 38);
            this.cbbNurseDept.Name = "cbbNurseDept";
            this.cbbNurseDept.Size = new System.Drawing.Size(126, 20);
            this.cbbNurseDept.TabIndex = 9;
            // 
            // cbbNursingClass
            // 
            this.cbbNursingClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbNursingClass.Enabled = false;
            this.cbbNursingClass.FormattingEnabled = true;
            this.cbbNursingClass.Items.AddRange(new object[] {
            "特级",
            "一级",
            "二级",
            "三级"});
            this.cbbNursingClass.Location = new System.Drawing.Point(96, 115);
            this.cbbNursingClass.Name = "cbbNursingClass";
            this.cbbNursingClass.Size = new System.Drawing.Size(126, 20);
            this.cbbNursingClass.TabIndex = 10;
            // 
            // cbbPatientCondition
            // 
            this.cbbPatientCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbPatientCondition.Enabled = false;
            this.cbbPatientCondition.FormattingEnabled = true;
            this.cbbPatientCondition.Items.AddRange(new object[] {
            "病危",
            "病重",
            "一般"});
            this.cbbPatientCondition.Location = new System.Drawing.Point(96, 139);
            this.cbbPatientCondition.Name = "cbbPatientCondition";
            this.cbbPatientCondition.Size = new System.Drawing.Size(126, 20);
            this.cbbPatientCondition.TabIndex = 11;
            // 
            // dtPStart
            // 
            this.dtPStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPStart.Enabled = false;
            this.dtPStart.Location = new System.Drawing.Point(96, 165);
            this.dtPStart.Name = "dtPStart";
            this.dtPStart.Size = new System.Drawing.Size(124, 21);
            this.dtPStart.TabIndex = 12;
            // 
            // dtPEnd
            // 
            this.dtPEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPEnd.Enabled = false;
            this.dtPEnd.Location = new System.Drawing.Point(96, 191);
            this.dtPEnd.Name = "dtPEnd";
            this.dtPEnd.Size = new System.Drawing.Size(124, 21);
            this.dtPEnd.TabIndex = 13;
            // 
            // txtPatientName
            // 
            this.txtPatientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPatientName.Enabled = false;
            this.txtPatientName.Location = new System.Drawing.Point(96, 63);
            this.txtPatientName.Name = "txtPatientName";
            this.txtPatientName.Size = new System.Drawing.Size(126, 21);
            this.txtPatientName.TabIndex = 14;
            // 
            // txtPatientID
            // 
            this.txtPatientID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPatientID.Enabled = false;
            this.txtPatientID.Location = new System.Drawing.Point(96, 89);
            this.txtPatientID.Name = "txtPatientID";
            this.txtPatientID.Size = new System.Drawing.Size(126, 21);
            this.txtPatientID.TabIndex = 15;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(147, 218);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "查询";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // patInfoList1
            // 
            this.patInfoList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.patInfoList1.AutoScroll = true;
            this.patInfoList1.BackColor = System.Drawing.Color.White;
            this.patInfoList1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.patInfoList1.Location = new System.Drawing.Point(3, 251);
            this.patInfoList1.Name = "patInfoList1";
            this.patInfoList1.Padding = new System.Windows.Forms.Padding(1);
            this.patInfoList1.SelectedCard = null;
            this.patInfoList1.Size = new System.Drawing.Size(217, 221);
            this.patInfoList1.TabIndex = 17;
            // 
            // QCPatientListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 475);
            this.Controls.Add(this.patInfoList1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtPatientID);
            this.Controls.Add(this.txtPatientName);
            this.Controls.Add(this.dtPEnd);
            this.Controls.Add(this.dtPStart);
            this.Controls.Add(this.cbbPatientCondition);
            this.Controls.Add(this.cbbNursingClass);
            this.Controls.Add(this.cbbNurseDept);
            this.Controls.Add(this.cbbPatientStatus);
            this.Controls.Add(this.ckbEndTime);
            this.Controls.Add(this.ckbStartTime);
            this.Controls.Add(this.ckbPatCondition);
            this.Controls.Add(this.ckbNursingClass);
            this.Controls.Add(this.ckbPatientID);
            this.Controls.Add(this.ckbPatientName);
            this.Controls.Add(this.ckbNurseDept);
            this.Controls.Add(this.ckbPatientStatus);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "QCPatientListForm";
            this.Text = "病人列表";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ckbPatientStatus;
        private System.Windows.Forms.CheckBox ckbNurseDept;
        private System.Windows.Forms.CheckBox ckbPatientName;
        private System.Windows.Forms.CheckBox ckbPatientID;
        private System.Windows.Forms.CheckBox ckbNursingClass;
        private System.Windows.Forms.CheckBox ckbPatCondition;
        private System.Windows.Forms.CheckBox ckbStartTime;
        private System.Windows.Forms.CheckBox ckbEndTime;
        private System.Windows.Forms.ComboBox cbbPatientStatus;
        private Heren.Common.Controls.DictInput.FindComboBox cbbNurseDept;
        private System.Windows.Forms.ComboBox cbbNursingClass;
        private System.Windows.Forms.ComboBox cbbPatientCondition;
        private System.Windows.Forms.DateTimePicker dtPStart;
        private System.Windows.Forms.DateTimePicker dtPEnd;
        private System.Windows.Forms.TextBox txtPatientName;
        private System.Windows.Forms.TextBox txtPatientID;
        private System.Windows.Forms.Button btnOK;
        private MedQCSys.Controls.PatInfoList.PatInfoList patInfoList1;
    }
}