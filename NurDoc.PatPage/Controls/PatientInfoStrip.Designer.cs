namespace Heren.NurDoc.PatPage.Controls
{
    partial class PatientInfoStrip
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
            if (this.m_patientPageControl != null && !this.m_patientPageControl.IsDisposed)
            {
                this.m_patientPageControl.PatientInfoChanged -=
                    new System.EventHandler(this.PatientPageControl_PatientInfoChanged);
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
            this.picSexFlag = new System.Windows.Forms.PictureBox();
            this.lblDiagnosis = new System.Windows.Forms.Label();
            this.lblAllergy = new System.Windows.Forms.Label();
            this.lblDoctorName = new System.Windows.Forms.Label();
            this.lblInpDays = new System.Windows.Forms.Label();
            this.lblPatientCondition = new System.Windows.Forms.Label();
            this.lblPatientAge = new System.Windows.Forms.Label();
            this.lblChargeType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.patientListControl1 = new Heren.NurDoc.PatPage.Controls.PatientListControl();
            ((System.ComponentModel.ISupportInitialize)(this.picSexFlag)).BeginInit();
            this.SuspendLayout();
            // 
            // picSexFlag
            // 
            this.picSexFlag.BackColor = System.Drawing.Color.Transparent;
            this.picSexFlag.Location = new System.Drawing.Point(16, 2);
            this.picSexFlag.Name = "picSexFlag";
            this.picSexFlag.Size = new System.Drawing.Size(27, 27);
            this.picSexFlag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSexFlag.TabIndex = 13;
            this.picSexFlag.TabStop = false;
            // 
            // lblDiagnosis
            // 
            this.lblDiagnosis.AutoEllipsis = true;
            this.lblDiagnosis.BackColor = System.Drawing.Color.Transparent;
            this.lblDiagnosis.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDiagnosis.Location = new System.Drawing.Point(598, 9);
            this.lblDiagnosis.Name = "lblDiagnosis";
            this.lblDiagnosis.Size = new System.Drawing.Size(180, 15);
            this.lblDiagnosis.TabIndex = 24;
            this.lblDiagnosis.Text = "诊断：";
            this.lblDiagnosis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAllergy
            // 
            this.lblAllergy.AutoEllipsis = true;
            this.lblAllergy.BackColor = System.Drawing.Color.Transparent;
            this.lblAllergy.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAllergy.Location = new System.Drawing.Point(780, 9);
            this.lblAllergy.Name = "lblAllergy";
            this.lblAllergy.Size = new System.Drawing.Size(180, 15);
            this.lblAllergy.TabIndex = 23;
            this.lblAllergy.Text = "过敏：";
            // 
            // lblDoctorName
            // 
            this.lblDoctorName.BackColor = System.Drawing.Color.Transparent;
            this.lblDoctorName.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDoctorName.Location = new System.Drawing.Point(503, 9);
            this.lblDoctorName.Name = "lblDoctorName";
            this.lblDoctorName.Size = new System.Drawing.Size(89, 15);
            this.lblDoctorName.TabIndex = 22;
            this.lblDoctorName.Text = "医生：";
            this.lblDoctorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInpDays
            // 
            this.lblInpDays.AutoEllipsis = true;
            this.lblInpDays.BackColor = System.Drawing.Color.Transparent;
            this.lblInpDays.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInpDays.Location = new System.Drawing.Point(419, 10);
            this.lblInpDays.Name = "lblInpDays";
            this.lblInpDays.Size = new System.Drawing.Size(78, 15);
            this.lblInpDays.TabIndex = 21;
            this.lblInpDays.Text = "在院：";
            // 
            // lblPatientCondition
            // 
            this.lblPatientCondition.AutoEllipsis = true;
            this.lblPatientCondition.BackColor = System.Drawing.Color.Transparent;
            this.lblPatientCondition.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPatientCondition.Location = new System.Drawing.Point(320, 10);
            this.lblPatientCondition.Name = "lblPatientCondition";
            this.lblPatientCondition.Size = new System.Drawing.Size(96, 15);
            this.lblPatientCondition.TabIndex = 19;
            this.lblPatientCondition.Text = "病情：";
            // 
            // lblPatientAge
            // 
            this.lblPatientAge.AutoEllipsis = true;
            this.lblPatientAge.BackColor = System.Drawing.Color.Transparent;
            this.lblPatientAge.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPatientAge.ForeColor = System.Drawing.Color.Black;
            this.lblPatientAge.Location = new System.Drawing.Point(222, 9);
            this.lblPatientAge.Name = "lblPatientAge";
            this.lblPatientAge.Size = new System.Drawing.Size(92, 18);
            this.lblPatientAge.TabIndex = 18;
            this.lblPatientAge.Text = "1岁11月23天";
            this.lblPatientAge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblChargeType
            // 
            this.lblChargeType.AutoEllipsis = true;
            this.lblChargeType.BackColor = System.Drawing.Color.Transparent;
            this.lblChargeType.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblChargeType.Location = new System.Drawing.Point(966, 9);
            this.lblChargeType.Name = "lblChargeType";
            this.lblChargeType.Size = new System.Drawing.Size(100, 15);
            this.lblChargeType.TabIndex = 27;
            this.lblChargeType.Text = "费别：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(191, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 28;
            this.label1.Text = "年龄：";
            // 
            // patientListControl1
            // 
            this.patientListControl1.BackColor = System.Drawing.Color.Transparent;
            this.patientListControl1.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.patientListControl1.ForeColor = System.Drawing.Color.Black;
            this.patientListControl1.Location = new System.Drawing.Point(50, 4);
            this.patientListControl1.Name = "patientListControl1";
            this.patientListControl1.Size = new System.Drawing.Size(139, 23);
            this.patientListControl1.TabIndex = 26;
            this.patientListControl1.PatientInfoChanging += new Heren.NurDoc.PatPage.PatientInfoChangingEventHandler(this.patientListControl1_PatientInfoChanging);
            // 
            // PatientInfoStrip
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblChargeType);
            this.Controls.Add(this.patientListControl1);
            this.Controls.Add(this.lblDiagnosis);
            this.Controls.Add(this.lblAllergy);
            this.Controls.Add(this.lblDoctorName);
            this.Controls.Add(this.lblInpDays);
            this.Controls.Add(this.lblPatientCondition);
            this.Controls.Add(this.lblPatientAge);
            this.Controls.Add(this.picSexFlag);
            this.Name = "PatientInfoStrip";
            this.Size = new System.Drawing.Size(1112, 33);
            ((System.ComponentModel.ISupportInitialize)(this.picSexFlag)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDiagnosis;
        private System.Windows.Forms.Label lblAllergy;
        private System.Windows.Forms.Label lblDoctorName;
        private System.Windows.Forms.Label lblInpDays;
        private System.Windows.Forms.Label lblPatientCondition;
        private System.Windows.Forms.Label lblPatientAge;
        private System.Windows.Forms.PictureBox picSexFlag;
        private Heren.NurDoc.PatPage.Controls.PatientListControl patientListControl1;
        private System.Windows.Forms.Label lblChargeType;
        private System.Windows.Forms.Label label1;
    }
}