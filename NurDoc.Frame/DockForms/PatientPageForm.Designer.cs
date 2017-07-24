namespace Heren.NurDoc.Frame.DockForms
{
    partial class PatientPageForm
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
            this.patientPageControl1 = new Heren.NurDoc.PatPage.PatientPageControl();
            this.SuspendLayout();
            // 
            // patientPageControl1
            // 
            this.patientPageControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.patientPageControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientPageControl1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.patientPageControl1.Location = new System.Drawing.Point(0, 0);
            this.patientPageControl1.Name = "patientPageControl1";
            this.patientPageControl1.Size = new System.Drawing.Size(709, 453);
            this.patientPageControl1.TabIndex = 0;
            this.patientPageControl1.PatientInfoChanged += new System.EventHandler(this.PatientPageControl_PatientInfoChanged);
            this.patientPageControl1.PatientInfoChanging += new Heren.NurDoc.PatPage.PatientInfoChangingEventHandler(this.PatientPageControl_PatientInfoChanging);
            // 
            // PatientPageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(709, 453);
            this.Controls.Add(this.patientPageControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PatientPageForm";
            this.Text = "病人窗口";
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.NurDoc.PatPage.PatientPageControl patientPageControl1;
    }
}