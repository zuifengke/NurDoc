using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Controls;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Dialogs
{
    public partial class SetDateTimeForm : HerenForm
    {
        private DateTime m_dtSelectedTime = DateTime.Now;

        /// <summary>
        /// 获取或设置选中的时间值
        /// </summary>
        public DateTime SelectedTime
        {
            get { return this.m_dtSelectedTime; }
            set { this.m_dtSelectedTime = value; }
        }

        public SetDateTimeForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            this.m_dtSelectedTime = SysTimeService.Instance.Now;
            this.dateTimeControl1.Value = this.m_dtSelectedTime;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DateTime? dtSelectedTime = this.dateTimeControl1.Value;
            if (dtSelectedTime != null && dtSelectedTime.HasValue)
                this.m_dtSelectedTime = dtSelectedTime.Value;
            else
                this.m_dtSelectedTime = SysTimeService.Instance.Now;
            this.DialogResult = DialogResult.OK;
        }
    }
}