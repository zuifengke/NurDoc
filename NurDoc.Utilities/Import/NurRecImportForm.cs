// **************************************************************
// 护理电子病历系统公用模块之护理记录数据导入窗口
// Creator:YangMingkun  Date:2012-9-12
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Import
{
    public partial class NurRecImportForm : HerenForm
    {
        private DataTable m_selectedNursingRecord = null;

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 获取当前选中的护理记录内容
        /// </summary>
        [Browsable(false)]
        [Description("获取当前选中的护理记录内容")]
        public DataTable SelectedNursingRecord
        {
            get { return this.m_selectedNursingRecord; }
        }

        private string m_patientID = string.Empty;

        /// <summary>
        /// 获取或设置查询的病人ID号
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置查询的病人ID号")]
        public string PatientID
        {
            get { return this.m_patientID; }
            set { this.m_patientID = value; }
        }

        private string m_visitID = string.Empty;

        /// <summary>
        /// 获取或设置查询的就诊ID号
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置查询的就诊ID号")]
        public string VisitID
        {
            get { return this.m_visitID; }
            set { this.m_visitID = value; }
        }

        private DateTime m_dtFrom = DateTime.MinValue;

        /// <summary>
        /// 获取或设置查询的起始时间
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置查询的起始时间")]
        public DateTime DateTimeFrom
        {
            get { return this.m_dtFrom; }
            set { this.m_dtFrom = value; }
        }

        private DateTime m_dtTo = DateTime.MinValue;

        /// <summary>
        /// 获取或设置查询的截止时间
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置查询的截止时间")]
        public DateTime DateTimeTo
        {
            get { return this.m_dtTo; }
            set { this.m_dtTo = value; }
        }

        public NurRecImportForm()
        {
            this.InitializeComponent();
            this.colRecordContent.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();
            m_selectedNursingRecord = null;
            //如果外部没有设置病人,那么默认
            if (GlobalMethods.Misc.IsEmptyString(this.m_patientID)
                && PatientTable.Instance.ActivePatient != null)
            {
                this.m_patientID = PatientTable.Instance.ActivePatient.PatientId;
            }

            if (GlobalMethods.Misc.IsEmptyString(this.m_visitID)
                && PatientTable.Instance.ActivePatient != null)
            {
                this.m_visitID = PatientTable.Instance.ActivePatient.VisitId;
            }

            //如果已经加载,那么不再重复加载
            string szOldPatVisit = this.dataTableView1.Tag as string;
            string szCurrPatVisit = string.Concat(this.m_patientID, "_", this.m_visitID);
            DateTime dtOldFrom = Convert.ToDateTime(this.tooldtpDateFrom.Tag);
            DateTime dtOldTo = Convert.ToDateTime(this.tooldtpDateTo.Tag);

            if (szOldPatVisit == szCurrPatVisit && dtOldFrom == m_dtFrom && dtOldTo == m_dtTo)
                return;

            this.m_bValueChangedEnabled = false;
            bool bshowExactlyTime = false;
            if (m_dtFrom == DateTime.MinValue || m_dtTo == DateTime.MinValue)
            {
                DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1);
                if (PatientTable.Instance.ActivePatient != null)
                    dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime;
                this.tooldtpDateFrom.Value = dtBeginTime;
                this.tooldtpDateTo.Value = SysTimeService.Instance.Now;
            }
            else
            {
                SetdtpReadOnly(false);
                this.tooldtpDateFrom.Value = m_dtFrom;
                this.tooldtpDateTo.Value = m_dtTo;
                this.tooldtpDateFrom.Tag = m_dtFrom;
                this.tooldtpDateTo.Tag = m_dtTo;
                bshowExactlyTime = true;
                SetdtpReadOnly(true);
            }

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.dataTableView1.CellValueChangedEventEnabled = false;
            this.LoadGridViewData(bshowExactlyTime);
            this.dataTableView1.CellValueChangedEventEnabled = true;
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载护理记录数据列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewData(bool bshowExactlyTime)
        {
            this.dataTableView1.Rows.Clear();
            this.chkSelectAll.Checked = false;
            Application.DoEvents();

            string szPatientID = this.m_patientID;
            string szVisitID = this.m_visitID;
            string szSubID = "0";
            this.dataTableView1.Tag = string.Concat(this.m_patientID, "_", this.m_visitID);

            if (GlobalMethods.Misc.IsEmptyString(szPatientID)
                || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                return false;
            }

            DateTime dtBeginTime = this.tooldtpDateFrom.Value;
            DateTime dtEndTime = bshowExactlyTime ? this.tooldtpDateTo.Value : this.tooldtpDateTo.Value.Date.AddHours(24);
            List<NursingRecInfo> lstNursingRecInfos = null;
            short shRet = NurRecService.Instance.GetNursingRecList(szPatientID, szVisitID, szSubID
                , dtBeginTime, dtEndTime, ref lstNursingRecInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("护理记录列表下载失败!");
                return false;
            }
            if (lstNursingRecInfos == null)
                lstNursingRecInfos = new List<NursingRecInfo>();

            Hashtable htNursingRec = new Hashtable();
            foreach (NursingRecInfo nursingRecInfo in lstNursingRecInfos)
            {
                int index = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[index];
                row.Tag = nursingRecInfo;
                this.dataTableView1.SetRowState(row, RowState.Normal);

                row.Cells[this.colRecordTime.Index].Value = nursingRecInfo.RecordTime.ToString("yyyy-M-d HH:mm");
                row.Cells[this.colRecorderName.Index].Value = nursingRecInfo.Recorder1Name;

                string content = nursingRecInfo.RecordContent + nursingRecInfo.RecordRemarks;
                row.Cells[this.colRecordContent.Index].Value = content;
            }
            int nMinHeight = this.dataTableView1.RowTemplate.Height;
            this.dataTableView1.AdjustRowHeight(0, this.dataTableView1.Rows.Count, nMinHeight, (nMinHeight * 5) + 4);
            return true;
        }

        private void SetdtpReadOnly(bool bRead)
        {
            this.tooldtpDateFrom.ShowHour = !bRead;
            this.tooldtpDateFrom.ShowMinute = !bRead;
            this.tooldtpDateFrom.ShowSecond = !bRead;
            this.tooldtpDateTo.ShowHour = !bRead;
            this.tooldtpDateTo.ShowMinute = !bRead;
            this.tooldtpDateTo.ShowSecond = !bRead;
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            m_bValueChangedEnabled = false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SetdtpReadOnly(false);
            this.tooldtpDateFrom.Value = new DateTime(this.tooldtpDateFrom.Value.Year
                , this.tooldtpDateFrom.Value.Month
                , this.tooldtpDateFrom.Value.Day, 0, 0, 0);
            this.tooldtpDateTo.Value = new DateTime(this.tooldtpDateTo.Value.Year
                , this.tooldtpDateTo.Value.Month
                , this.tooldtpDateTo.Value.Day, 0, 0, 0);
            this.SetdtpReadOnly(true);
            m_bValueChangedEnabled = true;
            this.LoadGridViewData(false);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnQuery_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadGridViewData(false);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.dataTableView1.CellValueChangedEventEnabled)
                return;
            //自动勾选当前行
            if (e.RowIndex >= 0 && e.ColumnIndex == this.colRecordContent.Index)
                this.dataTableView1.Rows[e.RowIndex].Cells[this.colCheckNurRec.Index].Value = true;
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.colCheckNurRec.Index)
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (row == null)
                    return;
                bool checkedValue = false;
                object cellValue = row.Cells[this.colCheckNurRec.Index].Value;
                if (cellValue != null)
                    checkedValue = GlobalMethods.Convert.StringToValue(cellValue.ToString(), false);
                row.Cells[this.colCheckNurRec.Index].Value = !checkedValue;
            }
        }

        private void dataTableView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == this.colCheckNurRec.Index)
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (row == null)
                    return;
                bool checkedValue = false;
                object cellValue = row.Cells[this.colCheckNurRec.Index].Value;
                if (cellValue != null)
                    checkedValue = GlobalMethods.Convert.StringToValue(cellValue.ToString(), false);
                row.Cells[this.colCheckNurRec.Index].Value = !checkedValue;
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                DataGridViewRow row = this.dataTableView1.Rows[index];
                if (row != null)
                    row.Cells[this.colCheckNurRec.Index].Value = this.chkSelectAll.Checked;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            this.dataTableView1.ClearSelection();
            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                DataGridViewRow row = this.dataTableView1.Rows[index];
                if (row == null)
                    continue;
                object cellValue = row.Cells[this.colCheckNurRec.Index].Value;
                if (cellValue != null && cellValue.ToString() == "True")
                    row.Selected = true;
            }
            this.m_selectedNursingRecord = GlobalMethods.Table.GetDataTable(this.dataTableView1, true, 0);
            this.DialogResult = DialogResult.OK;
        }
    }
}