namespace Heren.NurDoc.Frame.Controls
{
    partial class NursingHomePage
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

        #region Component Designer generated code

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
            this.formControl1.Size = new System.Drawing.Size(753, 52);
            this.formControl1.TabIndex = 0;
            this.formControl1.Text = "formControl1";
            this.formControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.formControl1_Paint);
            this.formControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.formControl1_MouseClick);
            this.formControl1.SizeChanged += new System.EventHandler(this.formControl1_SizeChanged);
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            // 
            // NursingHomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.formControl1);
            this.Name = "NursingHomePage";
            this.Size = new System.Drawing.Size(753, 52);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
    }
}
