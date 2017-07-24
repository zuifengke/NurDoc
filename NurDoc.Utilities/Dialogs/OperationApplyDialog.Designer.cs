namespace Heren.NurDoc.Utilities.Dialogs
{
    partial class OperationApplyDialog
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
            this.btnOK = new Heren.Common.Controls.HerenButton();
            this.btnCancel = new Heren.Common.Controls.HerenButton();
            this.lblRequestDesc = new System.Windows.Forms.Label();
            this.txtRequestDesc = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(188, 154);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(275, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblRequestDesc
            // 
            this.lblRequestDesc.AutoSize = true;
            this.lblRequestDesc.Location = new System.Drawing.Point(5, 8);
            this.lblRequestDesc.Name = "lblRequestDesc";
            this.lblRequestDesc.Size = new System.Drawing.Size(161, 12);
            this.lblRequestDesc.TabIndex = 8;
            this.lblRequestDesc.Text = "申请修改护理记录，原因是：";
            // 
            // txtRequestDesc
            // 
            this.txtRequestDesc.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txtRequestDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestDesc.Location = new System.Drawing.Point(7, 23);
            this.txtRequestDesc.Name = "txtRequestDesc";
            this.txtRequestDesc.Size = new System.Drawing.Size(348, 122);
            this.txtRequestDesc.TabIndex = 9;
            this.txtRequestDesc.Text = string.Empty;
            // 
            // OperationApplyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(362, 189);
            this.Controls.Add(this.txtRequestDesc);
            this.Controls.Add(this.lblRequestDesc);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OperationApplyDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "操作申请";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Heren.Common.Controls.HerenButton btnOK;
        private Heren.Common.Controls.HerenButton btnCancel;
        private System.Windows.Forms.Label lblRequestDesc;
        private System.Windows.Forms.RichTextBox txtRequestDesc;
    }
}