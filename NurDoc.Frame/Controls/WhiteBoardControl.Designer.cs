namespace Heren.NurDoc.Frame.Controls
{
    partial class WhiteBoardControl
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
            this.DisposeControl();
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
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.SuspendLayout();
            // 
            // formControl1
            // 
            this.formControl1.AutoScroll = false;
            this.formControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(6, 6);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(772, 71);
            this.formControl1.TabIndex = 0;
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            // 
            // WhiteBoardControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.formControl1);
            this.Name = "WhiteBoardControl";
            this.Padding = new System.Windows.Forms.Padding(6, 6, 6, 4);
            this.Size = new System.Drawing.Size(784, 81);
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
    }
}