using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Heren.NurDoc.Tester
{
    public partial class SinglePatForm : Form
    {
        public SinglePatForm()
        {
            InitializeComponent();
        }

        private string m_patientID = string.Empty;
        public string PatientID
        {
            get { return this.m_patientID; }
            set { this.m_patientID = value; }
        }

        private string m_visitID = string.Empty;
        public string VisitID
        {
            get { return this.m_visitID; }
            set { this.m_visitID = value; }
        }

        private string m_UserID = string.Empty;
        public string UserID
        {
            get { return this.m_UserID; }
            set { this.m_UserID = value; }
        }

        private string m_DocTypeID = string.Empty;
        public string DocType
        {
            get { return this.m_DocTypeID; }
            set { this.m_DocTypeID = value; }
        }

        private string m_DocID = string.Empty;
        public string DocID
        {
            get { return this.m_DocID; }
            set { this.m_DocID = value; }
        }

        public bool LocateToDoc = false;

        private void SinglePatForm_Load(object sender, EventArgs e)
        {
            NurdocControl.NurPatContrl NurPatContrl = new NurdocControl.NurPatContrl();
            this.Controls.Add(NurPatContrl);
            NurPatContrl.Dock = DockStyle.Fill;
            if (LocateToDoc)
                NurPatContrl.OpenNurDoc(m_patientID, m_visitID, m_UserID, m_DocTypeID,m_DocID);
            else
                NurPatContrl.SwitchPatient(m_patientID, m_visitID, m_UserID);
        }
    }
}
