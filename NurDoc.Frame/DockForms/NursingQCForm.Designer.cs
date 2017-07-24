namespace Heren.NurDoc.Frame.DockForms
{
    partial class NursingQCForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnQCAll = new System.Windows.Forms.Button();
            this.cboBingLiType = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.chkExamine = new System.Windows.Forms.CheckBox();
            this.chkInHospital = new System.Windows.Forms.CheckBox();
            this.chkNotExamine = new System.Windows.Forms.CheckBox();
            this.txtPatientID = new System.Windows.Forms.TextBox();
            this.lblPatientID = new System.Windows.Forms.Label();
            this.dtEndTime = new Heren.Common.Controls.TimeControl.DateTimeControl();
            this.lbl1 = new System.Windows.Forms.Label();
            this.dtBeginTime = new Heren.Common.Controls.TimeControl.DateTimeControl();
            this.chkOutHospital = new System.Windows.Forms.CheckBox();
            this.lblBingLiType = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formControl1
            // 
            this.formControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.formControl1.Location = new System.Drawing.Point(7, 46);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(1073, 410);
            this.formControl1.TabIndex = 0;
            this.formControl1.Text = "formControl1";
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnQCAll);
            this.panel1.Controls.Add(this.cboBingLiType);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.chkExamine);
            this.panel1.Controls.Add(this.chkInHospital);
            this.panel1.Controls.Add(this.chkNotExamine);
            this.panel1.Controls.Add(this.txtPatientID);
            this.panel1.Controls.Add(this.lblPatientID);
            this.panel1.Controls.Add(this.dtEndTime);
            this.panel1.Controls.Add(this.lbl1);
            this.panel1.Controls.Add(this.dtBeginTime);
            this.panel1.Controls.Add(this.chkOutHospital);
            this.panel1.Controls.Add(this.lblBingLiType);
            this.panel1.Location = new System.Drawing.Point(7, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1073, 38);
            this.panel1.TabIndex = 2;
            // 
            // btnQCAll
            // 
            this.btnQCAll.Image = global::Heren.NurDoc.Frame.Properties.Resources.Templet;
            this.btnQCAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQCAll.Location = new System.Drawing.Point(881, 7);
            this.btnQCAll.Name = "btnQCAll";
            this.btnQCAll.Size = new System.Drawing.Size(80, 23);
            this.btnQCAll.TabIndex = 12;
            this.btnQCAll.Text = "批量审核";
            this.btnQCAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnQCAll.UseVisualStyleBackColor = true;
            this.btnQCAll.Click += new System.EventHandler(this.btnQCAll_Click);
            // 
            // cboBingLiType
            // 
            this.cboBingLiType.FormattingEnabled = true;
            this.cboBingLiType.Location = new System.Drawing.Point(67, 12);
            this.cboBingLiType.Name = "cboBingLiType";
            this.cboBingLiType.Size = new System.Drawing.Size(121, 20);
            this.cboBingLiType.TabIndex = 3;
            this.cboBingLiType.SelectedIndexChanged += new System.EventHandler(this.cboBingLiType_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::Heren.NurDoc.Frame.Properties.Resources.Query;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(816, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(59, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "刷新";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkExamine
            // 
            this.chkExamine.AutoSize = true;
            this.chkExamine.Checked = true;
            this.chkExamine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExamine.Location = new System.Drawing.Point(750, 13);
            this.chkExamine.Name = "chkExamine";
            this.chkExamine.Size = new System.Drawing.Size(60, 16);
            this.chkExamine.TabIndex = 10;
            this.chkExamine.Text = "已审核";
            this.chkExamine.UseVisualStyleBackColor = true;
            this.chkExamine.CheckedChanged += new System.EventHandler(this.chkExamine_CheckedChanged);
            // 
            // chkInHospital
            // 
            this.chkInHospital.AutoSize = true;
            this.chkInHospital.Checked = true;
            this.chkInHospital.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInHospital.Location = new System.Drawing.Point(194, 12);
            this.chkInHospital.Name = "chkInHospital";
            this.chkInHospital.Size = new System.Drawing.Size(48, 16);
            this.chkInHospital.TabIndex = 3;
            this.chkInHospital.Text = "在院";
            this.chkInHospital.UseVisualStyleBackColor = true;
            this.chkInHospital.Click += new System.EventHandler(this.chkInHospital_Click);
            // 
            // chkNotExamine
            // 
            this.chkNotExamine.AutoSize = true;
            this.chkNotExamine.Checked = true;
            this.chkNotExamine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotExamine.Location = new System.Drawing.Point(687, 13);
            this.chkNotExamine.Name = "chkNotExamine";
            this.chkNotExamine.Size = new System.Drawing.Size(60, 16);
            this.chkNotExamine.TabIndex = 9;
            this.chkNotExamine.Text = "未审核";
            this.chkNotExamine.UseVisualStyleBackColor = true;
            this.chkNotExamine.CheckedChanged += new System.EventHandler(this.chkNotExamine_CheckedChanged);
            // 
            // txtPatientID
            // 
            this.txtPatientID.Enabled = false;
            this.txtPatientID.Location = new System.Drawing.Point(581, 9);
            this.txtPatientID.Name = "txtPatientID";
            this.txtPatientID.Size = new System.Drawing.Size(100, 21);
            this.txtPatientID.TabIndex = 8;
            // 
            // lblPatientID
            // 
            this.lblPatientID.AutoSize = true;
            this.lblPatientID.Location = new System.Drawing.Point(528, 12);
            this.lblPatientID.Name = "lblPatientID";
            this.lblPatientID.Size = new System.Drawing.Size(53, 12);
            this.lblPatientID.TabIndex = 3;
            this.lblPatientID.Text = "病人ID：";
            // 
            // dtEndTime
            // 
            this.dtEndTime.Enabled = false;
            this.dtEndTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dtEndTime.Location = new System.Drawing.Point(419, 10);
            this.dtEndTime.Name = "dtEndTime";
            this.dtEndTime.ShowHour = false;
            this.dtEndTime.ShowMinute = false;
            this.dtEndTime.ShowSecond = false;
            this.dtEndTime.Size = new System.Drawing.Size(100, 20);
            this.dtEndTime.TabIndex = 7;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(399, 15);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(17, 12);
            this.lbl1.TabIndex = 6;
            this.lbl1.Text = "—";
            // 
            // dtBeginTime
            // 
            this.dtBeginTime.Enabled = false;
            this.dtBeginTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dtBeginTime.Location = new System.Drawing.Point(297, 10);
            this.dtBeginTime.Name = "dtBeginTime";
            this.dtBeginTime.ShowHour = false;
            this.dtBeginTime.ShowMinute = false;
            this.dtBeginTime.ShowSecond = false;
            this.dtBeginTime.Size = new System.Drawing.Size(100, 20);
            this.dtBeginTime.TabIndex = 5;
            // 
            // chkOutHospital
            // 
            this.chkOutHospital.AutoSize = true;
            this.chkOutHospital.Location = new System.Drawing.Point(248, 11);
            this.chkOutHospital.Name = "chkOutHospital";
            this.chkOutHospital.Size = new System.Drawing.Size(48, 16);
            this.chkOutHospital.TabIndex = 4;
            this.chkOutHospital.Text = "出院";
            this.chkOutHospital.UseVisualStyleBackColor = true;
            this.chkOutHospital.Click += new System.EventHandler(this.chkOutHospital_Click);
            // 
            // lblBingLiType
            // 
            this.lblBingLiType.AutoSize = true;
            this.lblBingLiType.Location = new System.Drawing.Point(6, 12);
            this.lblBingLiType.Name = "lblBingLiType";
            this.lblBingLiType.Size = new System.Drawing.Size(65, 12);
            this.lblBingLiType.TabIndex = 2;
            this.lblBingLiType.Text = "病历类型：";
            // 
            // NursingQCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 455);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NursingQCForm";
            this.Text = "NursingQCForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBingLiType;
        private System.Windows.Forms.Label lblPatientID;
        private Heren.Common.Controls.TimeControl.DateTimeControl dtEndTime;
        private System.Windows.Forms.Label lbl1;
        private Heren.Common.Controls.TimeControl.DateTimeControl dtBeginTime;
        private System.Windows.Forms.CheckBox chkOutHospital;
        private System.Windows.Forms.CheckBox chkInHospital;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox chkExamine;
        private System.Windows.Forms.CheckBox chkNotExamine;
        private System.Windows.Forms.TextBox txtPatientID;
        private System.Windows.Forms.ComboBox cboBingLiType;
        private System.Windows.Forms.Button btnQCAll;
    }
}