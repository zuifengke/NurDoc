﻿namespace Heren.NurDoc.Config.Report
{
    partial class ReportInfoForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "模板ID号：";
            // 
            // txtTempletID
            // 
            this.txtTempletID.Location = new System.Drawing.Point(77, 11);
            this.txtTempletID.Name = "txtTempletID";
            this.txtTempletID.Size = new System.Drawing.Size(153, 21);
            this.txtTempletID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "模板名称：";
            // 
            // txtTempletName
            // 
            this.txtTempletName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTempletName.Location = new System.Drawing.Point(309, 11);
            this.txtTempletName.Name = "txtTempletName";
            this.txtTempletName.Size = new System.Drawing.Size(240, 21);
            this.txtTempletName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 44);
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
            this.txtWardList.Location = new System.Drawing.Point(77, 39);
            this.txtWardList.Name = "txtWardList";
            this.txtWardList.ReadOnly = true;
            this.txtWardList.Size = new System.Drawing.Size(471, 21);
            this.txtWardList.TabIndex = 5;
            this.txtWardList.DoubleClick += new System.EventHandler(this.txtWardList_DoubleClick);
            // 
            // chkIsValid
            // 
            this.chkIsValid.AutoSize = true;
            this.chkIsValid.Location = new System.Drawing.Point(77, 66);
            this.chkIsValid.Name = "chkIsValid";
            this.chkIsValid.Size = new System.Drawing.Size(60, 16);
            this.chkIsValid.TabIndex = 6;
            this.chkIsValid.Text = "可用的";
            this.chkIsValid.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(378, 92);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "保存";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(474, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ReportInfoForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 125);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTempletID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTempletName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWardList);
            this.Controls.Add(this.chkIsValid);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportInfoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "报告类型信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTempletID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTempletName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWardList;
        private System.Windows.Forms.CheckBox chkIsValid;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}