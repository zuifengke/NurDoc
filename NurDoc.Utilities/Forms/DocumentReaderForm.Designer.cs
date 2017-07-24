namespace Heren.NurDoc.Utilities.Forms
{
    partial class DocumentReaderForm
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
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.SuspendLayout();
            // 
            // formControl1
            // 
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(0, 0);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(843, 463);
            this.formControl1.TabIndex = 0;
            this.formControl1.Text = "formControl1";
            // 
            // DocumentReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 463);
            this.Controls.Add(this.formControl1);
            this.Name = "DocumentReaderForm";
            this.Text = "ÎÄµµä¯ÀÀ";
            this.ResumeLayout(false);

        }

        #endregion

        private FormControl formControl1;
    }
}