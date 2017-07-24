namespace Heren.NurDoc.Config.Dialogs
{
    partial class EvaInfoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTempletID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTempletName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWardList = new System.Windows.Forms.TextBox();
            this.chkIsValid = new System.Windows.Forms.CheckBox();
            this.chkIsVisible = new System.Windows.Forms.CheckBox();
            this.chkDocTypeNo = new System.Windows.Forms.CheckBox();
            this.txtDocTypeNo = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkRemark = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtStandard = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "模板ID号：";
            // 
            // txtTempletID
            // 
            this.txtTempletID.Location = new System.Drawing.Point(71, 3);
            this.txtTempletID.Name = "txtTempletID";
            this.txtTempletID.Size = new System.Drawing.Size(153, 21);
            this.txtTempletID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "模板名称：";
            // 
            // txtTempletName
            // 
            this.txtTempletName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTempletName.Location = new System.Drawing.Point(303, 3);
            this.txtTempletName.Name = "txtTempletName";
            this.txtTempletName.Size = new System.Drawing.Size(272, 21);
            this.txtTempletName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "所属科室：";
            // 
            // txtWardList
            // 
            this.txtWardList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWardList.BackColor = System.Drawing.Color.Lavender;
            this.txtWardList.Location = new System.Drawing.Point(71, 31);
            this.txtWardList.Name = "txtWardList";
            this.txtWardList.ReadOnly = true;
            this.txtWardList.Size = new System.Drawing.Size(647, 21);
            this.txtWardList.TabIndex = 5;
            this.txtWardList.DoubleClick += new System.EventHandler(this.txtWardList_DoubleClick);
            // 
            // chkIsValid
            // 
            this.chkIsValid.AutoSize = true;
            this.chkIsValid.Location = new System.Drawing.Point(71, 3);
            this.chkIsValid.Name = "chkIsValid";
            this.chkIsValid.Size = new System.Drawing.Size(60, 16);
            this.chkIsValid.TabIndex = 6;
            this.chkIsValid.Text = "可用的";
            this.chkIsValid.UseVisualStyleBackColor = true;
            this.chkIsValid.CheckedChanged += new System.EventHandler(this.chkDocTypeNo_CheckedChanged);
            // 
            // chkIsVisible
            // 
            this.chkIsVisible.AutoSize = true;
            this.chkIsVisible.Location = new System.Drawing.Point(172, 3);
            this.chkIsVisible.Name = "chkIsVisible";
            this.chkIsVisible.Size = new System.Drawing.Size(60, 16);
            this.chkIsVisible.TabIndex = 7;
            this.chkIsVisible.Text = "可见的";
            this.chkIsVisible.UseVisualStyleBackColor = true;
            // 
            // chkDocTypeNo
            // 
            this.chkDocTypeNo.AutoSize = true;
            this.chkDocTypeNo.Location = new System.Drawing.Point(257, 4);
            this.chkDocTypeNo.Name = "chkDocTypeNo";
            this.chkDocTypeNo.Size = new System.Drawing.Size(96, 16);
            this.chkDocTypeNo.TabIndex = 9;
            this.chkDocTypeNo.Text = "修改排序序号";
            this.chkDocTypeNo.UseVisualStyleBackColor = true;
            this.chkDocTypeNo.CheckedChanged += new System.EventHandler(this.chkDocTypeNo_CheckedChanged);
            // 
            // txtDocTypeNo
            // 
            this.txtDocTypeNo.Enabled = false;
            this.txtDocTypeNo.Location = new System.Drawing.Point(353, 0);
            this.txtDocTypeNo.Name = "txtDocTypeNo";
            this.txtDocTypeNo.Size = new System.Drawing.Size(74, 21);
            this.txtDocTypeNo.TabIndex = 10;
            this.txtDocTypeNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(539, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "保存";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(635, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtStandard);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtWardList);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtTempletName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtTempletID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(724, 52);
            this.panel1.TabIndex = 27;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkRemark);
            this.panel3.Controls.Add(this.txtDocTypeNo);
            this.panel3.Controls.Add(this.chkDocTypeNo);
            this.panel3.Controls.Add(this.chkIsVisible);
            this.panel3.Controls.Add(this.chkIsValid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 52);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(724, 30);
            this.panel3.TabIndex = 29;
            // 
            // chkRemark
            // 
            this.chkRemark.AutoSize = true;
            this.chkRemark.Location = new System.Drawing.Point(446, 5);
            this.chkRemark.Name = "chkRemark";
            this.chkRemark.Size = new System.Drawing.Size(96, 16);
            this.chkRemark.TabIndex = 11;
            this.chkRemark.Text = "是否显示备注";
            this.chkRemark.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnCancel);
            this.panel5.Controls.Add(this.btnOK);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 82);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(724, 30);
            this.panel5.TabIndex = 31;
            // 
            // txtStandard
            // 
            this.txtStandard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStandard.Location = new System.Drawing.Point(638, 5);
            this.txtStandard.MaxLength = 100;
            this.txtStandard.Name = "txtStandard";
            this.txtStandard.Size = new System.Drawing.Size(80, 21);
            this.txtStandard.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(581, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "达标率：";
            // 
            // EvaInfoForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(724, 115);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EvaInfoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "评价单类型信息";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTempletID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTempletName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWardList;
        private System.Windows.Forms.CheckBox chkIsValid;
        private System.Windows.Forms.CheckBox chkIsVisible;
        private System.Windows.Forms.CheckBox chkDocTypeNo;
        private System.Windows.Forms.TextBox txtDocTypeNo;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox chkRemark;
        private System.Windows.Forms.TextBox txtStandard;
        private System.Windows.Forms.Label label4;
    }
}