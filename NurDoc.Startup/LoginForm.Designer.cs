namespace Heren.NurDoc.Startup
{
    partial class LoginForm
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
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.lblUserPwd = new System.Windows.Forms.Label();
            this.txtUserPwd = new System.Windows.Forms.TextBox();
            this.btnModifyPwd = new Heren.Common.Controls.FlatButton();
            this.btnOK = new Heren.Common.Controls.HerenButton();
            this.btnCancel = new Heren.Common.Controls.HerenButton();
            this.cbRightSelect = new System.Windows.Forms.ComboBox();
            this.lblUserRight = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(229, 224);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(53, 12);
            this.lblUserID.TabIndex = 0;
            this.lblUserID.Text = "用户ID：";
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(285, 219);
            this.txtUserID.MaxLength = 50;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(158, 21);
            this.txtUserID.TabIndex = 1;
            this.txtUserID.TextChanged += new System.EventHandler(this.txtUserID_TextChanged);
            // 
            // lblUserPwd
            // 
            this.lblUserPwd.AutoSize = true;
            this.lblUserPwd.Location = new System.Drawing.Point(229, 258);
            this.lblUserPwd.Name = "lblUserPwd";
            this.lblUserPwd.Size = new System.Drawing.Size(53, 12);
            this.lblUserPwd.TabIndex = 2;
            this.lblUserPwd.Text = "口  令：";
            // 
            // txtUserPwd
            // 
            this.txtUserPwd.Location = new System.Drawing.Point(285, 253);
            this.txtUserPwd.MaxLength = 50;
            this.txtUserPwd.Name = "txtUserPwd";
            this.txtUserPwd.PasswordChar = '*';
            this.txtUserPwd.Size = new System.Drawing.Size(158, 21);
            this.txtUserPwd.TabIndex = 3;
            // 
            // btnModifyPwd
            // 
            this.btnModifyPwd.Location = new System.Drawing.Point(444, 253);
            this.btnModifyPwd.Name = "btnModifyPwd";
            this.btnModifyPwd.Size = new System.Drawing.Size(21, 21);
            this.btnModifyPwd.TabIndex = 4;
            this.btnModifyPwd.ToolTipText = "修改密码";
            this.btnModifyPwd.Click += new System.EventHandler(this.btnModifyPwd_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(230, 292);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定(&O)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(369, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "退出(&E)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbRightSelect
            // 
            this.cbRightSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRightSelect.FormattingEnabled = true;
            this.cbRightSelect.Items.AddRange(new object[] {
            "护理文书",
            "护理文书质控"});
            this.cbRightSelect.Location = new System.Drawing.Point(285, 287);
            this.cbRightSelect.Name = "cbRightSelect";
            this.cbRightSelect.Size = new System.Drawing.Size(158, 20);
            this.cbRightSelect.TabIndex = 7;
            this.cbRightSelect.Visible = false;
            // 
            // lblUserRight
            // 
            this.lblUserRight.AutoSize = true;
            this.lblUserRight.Location = new System.Drawing.Point(231, 294);
            this.lblUserRight.Name = "lblUserRight";
            this.lblUserRight.Size = new System.Drawing.Size(53, 12);
            this.lblUserRight.TabIndex = 8;
            this.lblUserRight.Text = "登陆至：";
            this.lblUserRight.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(688, 213);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(688, 369);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblUserRight);
            this.Controls.Add(this.cbRightSelect);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.lblUserPwd);
            this.Controls.Add(this.txtUserPwd);
            this.Controls.Add(this.btnModifyPwd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录护理电子病历系统";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label lblUserPwd;
        private System.Windows.Forms.TextBox txtUserPwd;
        private Heren.Common.Controls.FlatButton btnModifyPwd;
        private Heren.Common.Controls.HerenButton btnOK;
        private Heren.Common.Controls.HerenButton btnCancel;
        private System.Windows.Forms.ComboBox cbRightSelect;
        private System.Windows.Forms.Label lblUserRight;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

