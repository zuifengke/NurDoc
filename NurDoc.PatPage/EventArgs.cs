// ***********************************************************
// ������Ӳ���ϵͳ,���˴��ڿؼ������¼��Ĳ���������.
// Creator:YangMingkun  Date:2012-7-3
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.PatPage
{
    public class PatientInfoChangingEventArgs : CancelEventArgs
    {
        private PatVisitInfo m_currPatVisit = null;

        /// <summary>
        /// ��ȡ���ı�Ĳ�����Ϣ
        /// </summary>
        public PatVisitInfo CurrPatVisit
        {
            get { return this.m_currPatVisit; }
        }

        private PatVisitInfo m_newPatVisit = null;

        /// <summary>
        /// ��ȡ����Ҫ�ı�Ĳ�����Ϣ
        /// </summary>
        public PatVisitInfo NewPatVisit
        {
            get { return this.m_newPatVisit; }
        }

        public PatientInfoChangingEventArgs(PatVisitInfo currPatVisit, PatVisitInfo newPatVisit)
        {
            this.Cancel = false;
            this.m_currPatVisit = currPatVisit;
            this.m_newPatVisit = newPatVisit;
        }
    }

    public delegate void PatientInfoChangingEventHandler(object sender, PatientInfoChangingEventArgs e);
}
