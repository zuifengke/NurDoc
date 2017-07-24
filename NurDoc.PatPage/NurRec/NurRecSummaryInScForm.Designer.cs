namespace Heren.NurDoc.PatPage.NurRec
{
    partial class NurRecSummaryInScForm
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
            this.formControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.formControl1.Location = new System.Drawing.Point(5, 5);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(690, 430);
            this.formControl1.TabIndex = 0;
            this.formControl1.Text = "formControl1";
            this.formControl1.QueryContext += new Heren.Common.Forms.Editor.QueryContextEventHandler(this.formControl1_QueryContext);
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            this.formControl1.ExecuteQuery += new Heren.Common.Forms.Editor.ExecuteQueryEventHandler(this.formControl1_ExecuteQuery);
            // 
            // NurRecSummaryInScForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(700, 440);
            this.Controls.Add(this.formControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NurRecSummaryInScForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑护理记录小结";
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
    }
}