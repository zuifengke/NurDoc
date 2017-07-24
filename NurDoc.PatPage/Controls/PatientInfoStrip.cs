// ***********************************************************
// 护理电子病历系统,病人窗口之病人信息条控件.
// Creator:YangMingkun  Date:2012-7-7
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.PatPage.Controls
{
    internal partial class PatientInfoStrip : UserControl
    {
        private PatientPageControl m_patientPageControl = null;

        public PatientPageControl PatientPageControl
        {
            get { return this.m_patientPageControl; }
            set { this.SetPatientPageControl(value); }
        }

        private PatVisitInfo m_patientVisit = null;

        public PatientInfoStrip()
            : this(null)
        {
        }

        public PatientInfoStrip(PatientPageControl patientPageControl)
        {
            this.InitializeComponent();
            this.SetPatientPageControl(patientPageControl);
        }

        private void SetPatientPageControl(PatientPageControl patientPageControl)
        {
            this.m_patientPageControl = patientPageControl;
            if (this.m_patientPageControl == null || this.m_patientPageControl.IsDisposed)
                return;
            this.m_patientPageControl.PatientInfoChanged +=
                new EventHandler(this.PatientPageControl_PatientInfoChanged);
            PatientTable.Instance.PatientInfoChanged -=
                new EventHandler(this.PatientPageControl_PatientInfoChanged);
            PatientTable.Instance.PatientInfoChanged +=
                new EventHandler(this.PatientPageControl_PatientInfoChanged);
        }

        private void RefreshView(PatVisitInfo patientVisit)
        {
            this.m_patientVisit = patientVisit;
            this.picSexFlag.Image = null;
            this.lblPatientAge.Text = "0岁";
            this.lblPatientCondition.Text = "病情：";
            this.lblInpDays.Text = "在院：";
            this.lblDoctorName.Text = "医生：";
            this.lblDiagnosis.Text = "诊断：";
            this.lblAllergy.Text = string.Empty;
            this.patientListControl1.RefreshView(patientVisit);
            if (patientVisit == null || patientVisit.VisitTime == patientVisit.DefaultTime)
                return;

            if (this.m_patientVisit.PatientSex == ServerData.PatientSex.Male)
                this.picSexFlag.Image = Properties.Resources.Male;
            else if (this.m_patientVisit.PatientSex == ServerData.PatientSex.Female)
                this.picSexFlag.Image = Properties.Resources.Female;

            DateTime dtBirthTime = patientVisit.BirthTime;
            this.lblPatientAge.Text =
                GlobalMethods.SysTime.GetAgeText(dtBirthTime, patientVisit.VisitTime);
            this.lblPatientCondition.Text = "病情：" + patientVisit.PatientCondition;
            this.lblInpDays.Text = string.Format("在院：{0}天"
                , GlobalMethods.SysTime.GetInpDays(patientVisit.VisitTime, DateTime.Now).ToString());
            this.lblDoctorName.Text = "医生：" + patientVisit.InchargeDoctor;
            this.lblDiagnosis.Text = "诊断：" + patientVisit.Diagnosis;
            this.lblAllergy.Text = "过敏：" + (string.IsNullOrEmpty(patientVisit.AllergyDrugs) ? "无" : patientVisit.AllergyDrugs);
            this.lblChargeType.Text = "费别：" + patientVisit.ChargeType;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle ctrlRect = this.ClientRectangle;
            LinearGradientBrush brush = new LinearGradientBrush(
                ctrlRect, Color.White, Color.WhiteSmoke, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, ctrlRect);
            brush.Dispose();
        }

        private void PatientPageControl_PatientInfoChanged(object sender, EventArgs e)
        {
            this.RefreshView(PatientTable.Instance.ActivePatient);
        }

        private void patientListControl1_PatientInfoChanging(object sender, PatientInfoChangingEventArgs e)
        {
            if (this.m_patientPageControl == null || this.m_patientPageControl.IsDisposed)
                return;
            this.m_patientPageControl.Update();
            e.Cancel = !this.m_patientPageControl.SwitchPatient(e.NewPatVisit);
        }
    }
}
