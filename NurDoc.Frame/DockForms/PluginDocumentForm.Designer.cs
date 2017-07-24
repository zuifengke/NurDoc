namespace Heren.NurDoc.Frame.DockForms
{
    partial class PluginDocumentForm
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
            this.formControl1.Location = new System.Drawing.Point(2, 2);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(659, 444);
            this.formControl1.TabIndex = 0;
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            // 
            // PluginDocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(663, 448);
            this.Controls.Add(this.formControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PluginDocumentForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "ÎÄµµ´°¿Ú";
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
    }
}