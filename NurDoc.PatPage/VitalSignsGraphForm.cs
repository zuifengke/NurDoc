// ***********************************************************
// 护理电子病历系统,病人体征三测单窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Report;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.PatPage
{
    internal partial class VitalSignsGraphForm : DockContentBase
    {
        /// <summary>
        /// 标识日期时间控件改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 缓存下载当前体温单报表文件数据,以便打印使用
        /// </summary>
        private byte[] m_byteReportData = null;

        public VitalSignsGraphForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.VitalSignsGraph;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 100);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.toolbtnHide.Visible = false;
            this.toolbtnSave.Visible = false;
            this.splitContainer1.Panel2Collapsed = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.toolcboZoom.Text = "100%";
            this.LoadBodyTemperatureTemplet();
            if (!this.splitContainer1.Panel2Collapsed)
                this.LoadDataRecordTemplet();
            this.reportDesigner1.Focus();

            this.m_bValueChangedEnabled = false;
            this.tooldtpDateFrom.Value = this.GetEndWeekDate(true);
            this.m_bValueChangedEnabled = true;

            this.tooltxtRecordWeek.Text = this.GetWeek(null).ToString();

            this.RefreshView();
            this.reportDesigner1.Focus();
        }

        protected override void OnPatientInfoChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
            {
                this.m_bValueChangedEnabled = false;
                this.tooltxtRecordWeek.Text = this.GetWeek(null).ToString();
                this.RefreshView();
                this.m_bValueChangedEnabled = true;
            }
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientInfoUpdated)
            {
                this.m_bValueChangedEnabled = false;
                this.tooltxtRecordWeek.Text = this.GetWeek(null).ToString();
                this.RefreshView();
                this.m_bValueChangedEnabled = true;
            }
        }

        public override void RefreshView()
        {
            base.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.Update();
            this.RefreshReportView(this.GetSelectedWeek());
            if (!this.splitContainer1.Panel2Collapsed)
            {
                DateTime dtNow = SysTimeService.Instance.Now;
                dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, dtNow.Hour, dtNow.Minute, 0);
                this.formControl1.UpdateFormData("体征时间", dtNow);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 获取当前病人住院起始周的日期
        /// </summary>
        /// <returns>起始周日期</returns>
        private DateTime GetBeginWeekDate()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return SysTimeService.Instance.Now;
            DateTime dtWeekDate = PatientTable.Instance.ActivePatient.VisitTime;
            if (dtWeekDate != PatientTable.Instance.ActivePatient.DefaultTime)
                return dtWeekDate;
            return SysTimeService.Instance.Now;
        }

        /// <summary>
        /// 获取当前病人住院截止周的日期
        /// </summary>
        /// <param name="bStopToCurrentTime">是否截止到当前时间</param>
        /// <returns>截止周日期</returns>
        private DateTime GetEndWeekDate(bool bStopToCurrentTime)
        {
            if (PatientTable.Instance.ActivePatient == null)
                return SysTimeService.Instance.Now;
            DateTime dtWeekDate = PatientTable.Instance.ActivePatient.DischargeTime;
            if (dtWeekDate != PatientTable.Instance.ActivePatient.DefaultTime)
                return dtWeekDate;
            return bStopToCurrentTime ? SysTimeService.Instance.Now : DateTime.MaxValue;
        }

        /// <summary>
        /// 获取指定的日期时间对应的周次
        /// </summary>
        /// <param name="dtWeekDate">日期时间</param>
        /// <returns>周次</returns>
        private long GetWeek(DateTime? dtWeekDate)
        {
            if (PatientTable.Instance.ActivePatient == null)
                return 1;
            DateTime dtVisitTime = PatientTable.Instance.ActivePatient.VisitTime;
            DateTime dtEndTime = DateTime.Now;

            if (dtWeekDate != null && dtWeekDate.HasValue)
                dtEndTime = dtWeekDate.Value;
            else if (PatientTable.Instance.ActivePatient.DischargeTime != PatientTable.Instance.ActivePatient.DefaultTime)
                dtEndTime = PatientTable.Instance.ActivePatient.DischargeTime;
            else
                dtEndTime = SysTimeService.Instance.Now;

            long days = GlobalMethods.SysTime.DateDiff(DateInterval.Day, dtVisitTime.Date, dtEndTime.Date);
            long weeks = (days / 7) + 1;
            return weeks;
        }

        /// <summary>
        /// 获取当前周次文本框中的周次
        /// </summary>
        /// <returns>周次</returns>
        private long GetSelectedWeek()
        {
            return GlobalMethods.Convert.StringToValue(this.tooltxtRecordWeek.Text, 1);
        }

        /// <summary>
        /// 根据周次刷新当前报表视图显示
        /// </summary>
        /// <param name="week">周次</param>
        private void RefreshReportView(long week)
        {
            long end = this.GetWeek(this.GetEndWeekDate(false));
            if (end < 1)
                end = 1;
            if (week < 1)
            {
                week = 1;
                this.m_bValueChangedEnabled = false;
                this.tooldtpDateFrom.Value = this.GetBeginWeekDate();
                this.m_bValueChangedEnabled = true;
            }
            if (week > end)
            {
                week = end;
                this.m_bValueChangedEnabled = false;
                this.tooldtpDateFrom.Value = this.GetEndWeekDate(false);
                this.m_bValueChangedEnabled = true;
            }
            this.tooltxtRecordWeek.Text = week.ToString();

            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.reportDesigner1.UpdateReport("显示数据", week);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载体温单报表模板
        /// </summary>
        private void LoadBodyTemperatureTemplet()
        {
            this.m_byteReportData = null;
            if (PatientTable.Instance.ActivePatient == null)
                return;
            if (SystemContext.Instance.LoginUser == null)
                return;
            this.Update();
            string szApplyEnv = ServerData.ReportTypeApplyEnv.TEMPERATURE;
            ReportTypeInfo reportTypeInfo = ReportCache.Instance.GetWardReportType(szApplyEnv);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowError("体温单模板还没有制作!");
                return;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("体温单模板内容下载失败!");
                return;
            }
            this.reportDesigner1.OpenDocument(byteTempletData);
            this.m_byteReportData = byteTempletData;
        }

        /// <summary>
        /// 加载体征快捷录入模板
        /// </summary>
        private void LoadDataRecordTemplet()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return;
            if (SystemContext.Instance.LoginUser == null)
                return;
            this.Update();
            string szApplyEnv = ServerData.DocTypeApplyEnv.SIGNS_DATA_RECORD;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("体征快捷录入模板还没有制作!");
                return;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("体征快捷录入模板内容下载失败!");
                return;
            }
            this.formControl1.Load(docTypeInfo, byteFormData);
            if (this.formControl1.Controls.Count > 0)
            {
                this.splitContainer1.SplitterDistance =
                    this.splitContainer1.Width - this.formControl1.Controls[0].Width;
                this.formControl1.Controls[0].Dock = DockStyle.Fill;
            }
            // 检查有没有编辑生命体征的权限
            if (!RightController.Instance.CanEditVitalSigns())
                this.formControl1.UpdateFormData("更新控件状态", null);
            this.formControl1.IsModified = false;
        }

        private ReportExplorerForm GetReportExplorerForm()
        {
            ReportExplorerForm reportExplorerForm = new ReportExplorerForm();
            reportExplorerForm.WindowState = FormWindowState.Maximized;
            reportExplorerForm.QueryContext +=
                new QueryContextEventHandler(this.reportDesigner1_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new ExecuteQueryEventHandler(this.reportDesigner1_ExecuteQuery);
            return reportExplorerForm;
        }

        private void tooldtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            this.RefreshReportView(this.GetWeek(this.tooldtpDateFrom.Value));
        }

        private void tooltxtRecordWeek_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9')
                && e.KeyChar != (char)Keys.Enter
                && e.KeyChar != (char)Keys.Delete
                && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == (char)Keys.Enter)
                this.RefreshReportView(this.GetSelectedWeek());
        }

        private void toolbtnPrevWeek_Click(object sender, EventArgs e)
        {
            this.reportDesigner1.Focus();
            this.RefreshReportView(this.GetSelectedWeek() - 1);
        }

        private void toolbtnNextWeek_Click(object sender, EventArgs e)
        {
            this.reportDesigner1.Focus();
            this.RefreshReportView(this.GetSelectedWeek() + 1);
        }

        private void toolbtnRefresh_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            string szZoomText = this.toolcboZoom.Text;
            if (GlobalMethods.Misc.IsEmptyString(szZoomText))
                return;
            szZoomText = szZoomText.Remove(szZoomText.Length - 1);
            float zoom = GlobalMethods.Convert.StringToValue(szZoomText, 100f);
            this.reportDesigner1.Zoom = zoom / 100f;
        }

        private void toolbtnRecord_Click(object sender, EventArgs e)
        {
            this.toolbtnHide.Visible =
                this.splitContainer1.Panel2Collapsed;
            this.toolbtnSave.Visible =
                this.splitContainer1.Panel2Collapsed;
            this.splitContainer1.Panel2Collapsed =
                !this.splitContainer1.Panel2Collapsed;
            if (!this.splitContainer1.Panel2Collapsed)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.LoadDataRecordTemplet();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        private void toolmnuPrintAll_Click(object sender, EventArgs e)
        {
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintVitalSigns.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印体征记录!");
                return;
            }
            if (this.m_byteReportData != null)
            {
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = this.m_byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("打印数据", 1);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintCurrent_Click(object sender, EventArgs e)
        {
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintVitalSigns.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印体征记录!");
                return;
            }
            if (this.m_byteReportData != null)
            {
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = (int)this.GetSelectedWeek();
                explorerForm.ReportFileData = this.m_byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", this.GetSelectedWeek());
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintFrom_Click(object sender, EventArgs e)
        {
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintVitalSigns.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印体征记录!");
                return;
            }
            if (this.m_byteReportData != null)
            {
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = (int)this.GetSelectedWeek();
                explorerForm.ReportFileData = this.m_byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("打印数据", this.GetSelectedWeek());
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnHide_Click(object sender, EventArgs e)
        {
            this.toolbtnHide.Visible = false;
            this.toolbtnSave.Visible = false;
            this.splitContainer1.Panel2Collapsed = true;
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.CanEditVitalSigns())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (!this.formControl1.EndEdit())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            object param = null;
            if (this.formControl1.SaveFormData(ref param))
                this.RefreshReportView(this.GetSelectedWeek());
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void reportDesigner1_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = SystemContext.Instance.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }

        private void reportDesigner1_ExecuteQuery(object sender, Heren.Common.Report.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void reportDesigner1_ElementDoubleClick(object sender, Heren.Common.VectorEditor.CanvasEventArgs e)
        {
            if (e.Element is Heren.Common.VectorEditor.Shapes.TextFieldElement)
            {
                if (e.Element.Data == null)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                if (this.splitContainer1.Panel2Collapsed)
                    this.toolbtnRecord.PerformClick();
                System.Threading.Thread.Sleep(100);//这里做个延迟,以便给用户一个感觉上的反馈
                e.Handled = true;
                this.formControl1.UpdateFormData(e.Element.Name, e.Element.Data);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            Heren.Common.VectorEditor.Shapes.CoordinateElement coordinateElement =
                e.Element as Heren.Common.VectorEditor.Shapes.CoordinateElement;
            if (coordinateElement != null && coordinateElement.XAxisDataType ==
                Heren.Common.VectorEditor.Shapes.CoordinateElement.DataType.DateTime)
            {
                Heren.Common.VectorEditor.Shapes.CoordinateData data =
                    e.Data as Heren.Common.VectorEditor.Shapes.CoordinateData;
                if (data == null)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                if (this.splitContainer1.Panel2Collapsed)
                    this.toolbtnRecord.PerformClick();
                System.Threading.Thread.Sleep(100);//这里做个延迟,以便给用户一个感觉上的反馈
                e.Handled = true;
                this.formControl1.UpdateFormData(data.DataName, (data.XOriginData is DateTime) ? data.XOriginData : data.XDateTimeData);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }
    }
}
