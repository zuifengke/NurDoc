namespace Heren.NurDoc.PatPage
{
    partial class PatientPageControl
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
            this.dockPanel1 = new Heren.Common.DockSuite.DockPanel();
            this.patientInfoStrip1 = new Heren.NurDoc.PatPage.Controls.PatientInfoStrip();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel1.DocumentStyle = Heren.Common.DockSuite.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 30);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.ShowDocumentBorder = false;
            this.dockPanel1.ShowDocumentSubhead = true;
            this.dockPanel1.Size = new System.Drawing.Size(709, 423);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.ActiveContentChanged += new System.EventHandler(this.dockPanel1_ActiveContentChanged);
            this.dockPanel1.ActiveDocumentChanged += new System.EventHandler(this.dockPanel1_ActiveDocumentChanged);
            // 
            // patientInfoStrip1
            // 
            this.patientInfoStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.patientInfoStrip1.Location = new System.Drawing.Point(0, 0);
            this.patientInfoStrip1.Name = "patientInfoStrip1";
            this.patientInfoStrip1.PatientPageControl = null;
            this.patientInfoStrip1.Size = new System.Drawing.Size(709, 30);
            this.patientInfoStrip1.TabIndex = 1;
            // 
            // PatientPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.patientInfoStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PatientPageControl";
            this.Size = new System.Drawing.Size(709, 453);
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.NurDoc.PatPage.Controls.PatientInfoStrip patientInfoStrip1;
        private Heren.Common.DockSuite.DockPanel dockPanel1;
    }
}