// ***********************************************************
// 护理电子病历系统,病人医嘱单窗口.
// Creator:YangMingkun  Date:2012-7-7
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.Common.Report;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.PatPage
{
    internal partial class OrdersRecordForm : DockContentBase
    {
        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 用药方式集合
        /// </summary>
        private string m_strOrdersWay = null;

        public OrdersRecordForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            if (!SystemContext.Instance.SystemOption.OptionOrdersPrint)
                this.toolbtnPrint.Visible = false;

            string text = SystemContext.Instance.WindowName.OrdersRecord;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 200);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();

            this.m_bValueChangedEnabled = false;
            if (this.toolcboOrdersType.Items.Count > 0)
                this.toolcboOrdersType.SelectedIndex = 0;

            DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1).Date;
            if (PatientTable.Instance.ActivePatient != null)
                dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(SysTimeService.Instance.Now);
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            this.RefreshView();
        }

        protected override void OnPatientInfoChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
            {
                if (PatientTable.Instance.ActivePatient != null)
                    this.tooldtpDateFrom.Value = PatientTable.Instance.ActivePatient.VisitTime;
                this.RefreshView();
            }
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientInfoUpdated)
            {
                if (PatientTable.Instance.ActivePatient != null)
                    this.tooldtpDateFrom.Value = PatientTable.Instance.ActivePatient.VisitTime;
                this.RefreshView();
            }
        }

        public override void RefreshView()
        {
            base.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrdersList();
            InittoolcboTreatmentItems();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void InittoolcboTreatmentItems()
        {
            this.toolcboTreatment.Items.Clear();
            if (this.m_strOrdersWay == string.Empty)
                return;
            if (this.m_strOrdersWay == null)
                return;
            string[] arrOrdersWay = this.m_strOrdersWay.Split(',');
            this.toolcboTreatment.Items.AddRange(arrOrdersWay);
        }

        private bool LoadOrdersList()
        {
            this.dataTableView1.Rows.Clear();
            if (PatientTable.Instance.ActivePatient == null)
                return false;
            this.Update();
            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            DateTime dtBeginDate = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndDate = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value);

            bool bIsRepeat = this.toolcboOrdersType.SelectedIndex == 1;
            String bIsRepeatAll = this.toolcboOrdersType.Text;

            //医嘱标识 0-医生下达的医嘱 1-护士转抄的医嘱
            List<OrderInfo> lstOrderInfo = null;
            short shRet = PatVisitService.Instance.GetPerformOrderList(szPatientID, szVisitID
                , dtBeginDate, dtEndDate, ref lstOrderInfo);
            if (shRet != SystemConst.ReturnValue.OK && shRet != SystemConst.ReturnValue.CANCEL)
            {
                this.DockHandler.Activate();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("查询医嘱列表失败!");
                return false;
            }
            if (lstOrderInfo == null || lstOrderInfo.Count <= 0)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }
            this.m_strOrdersWay = string.Empty;
            //先对父子医嘱进行分组
            Dictionary<string, List<OrderInfo>> dicOrderList =
                new Dictionary<string, List<OrderInfo>>();
            for (int index = 0; index < lstOrderInfo.Count; index++)
            {
                OrderInfo orderInfo = lstOrderInfo[index];
                if (orderInfo == null || string.IsNullOrEmpty(orderInfo.OrderNO))
                    continue;
                if (bIsRepeatAll != "全部")
                { 
                   if (orderInfo.IsRepeat != bIsRepeat)
                      continue;
                 }
                List<OrderInfo> lstOrderList = null;
                if (dicOrderList.ContainsKey(orderInfo.OrderNO))
                {
                    lstOrderList = dicOrderList[orderInfo.OrderNO];
                }
                else
                {
                    lstOrderList = new List<OrderInfo>();
                    dicOrderList.Add(orderInfo.OrderNO, lstOrderList);
                }
                if (orderInfo.OrderSubNO != "1")
                    lstOrderList.Add(orderInfo);
                else
                    lstOrderList.Insert(0, orderInfo);
                if (!this.m_strOrdersWay.Contains(orderInfo.Administration))
                    this.m_strOrdersWay = this.m_strOrdersWay + "," + orderInfo.Administration;
            }

            List<OrderInfo>[] arrOrderList =
                new List<OrderInfo>[dicOrderList.Values.Count];
            dicOrderList.Values.CopyTo(arrOrderList, 0);
            string strOrderCatagrate = string.Empty;

            if (this.toolcboOrdersCategory.SelectedItem != null)
                strOrderCatagrate = SystemContext.Instance.GetOrderCatagrateTable(this.toolcboOrdersCategory.SelectedItem.ToString());
           
            //倒序加载并显示医嘱列表 474顺序显示医嘱列表 2014-03-11 rqc
            for (int nListIndex = 0; nListIndex < arrOrderList.Length; nListIndex++)
            {
                List<OrderInfo> lstOrderList = arrOrderList[nListIndex];
                for (int index = 0; index < lstOrderList.Count; index++)
                {                   
                    OrderInfo orderInfo = lstOrderList[index];
                    if (orderInfo == null)
                        continue;
                    if (!this.CheckOrderShow(orderInfo))
                        continue;
                    if (strOrderCatagrate != string.Empty
                        && !strOrderCatagrate.Contains(orderInfo.OrderClass))
                        continue;
                    if (this.toolcboTreatment.SelectedItem != null
                        && this.toolcboTreatment.SelectedItem.ToString() != string.Empty
                        && orderInfo.Administration != this.toolcboTreatment.SelectedItem.ToString()
                        )
                        continue;
                    int nRowIndex = this.dataTableView1.Rows.Add();
                    DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                    this.SetRowData(row, orderInfo);

                    if (lstOrderList.Count > 1)
                    {
                        if (index == 0)
                            row.Cells[this.colSubFlag.Index].Value = "┓";
                        else if (index == lstOrderList.Count - 1)
                            row.Cells[this.colSubFlag.Index].Value = "┛";
                        else
                            row.Cells[this.colSubFlag.Index].Value = "┃";
                    }
                    this.dataTableView1.SetRowState(row, RowState.Normal);
                }
            }
            return true;
          }

        /// <summary>
        /// 根据是否停止来判断是否显示在界面上
        /// <param name="orderInfo">医嘱</param>
        /// <returns>是否应该显示</returns>
        private bool CheckOrderShow(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;
            if (this.toolcboOrdersStatus.Text == string.Empty)
            {
                if (orderInfo.OrderStatus == "2" && orderInfo.IsRepeat)
                {
                    return true;
                }
                if (!orderInfo.IsRepeat || (orderInfo.IsRepeat && (orderInfo.OrderStatus == "3" || orderInfo.OrderStatus == "4")))
                {
                    return true;
                }
            }
            if (this.toolcboOrdersStatus.Text == "未停" && orderInfo.OrderStatus == "2" && orderInfo.IsRepeat)
            {
                return true;
            }
            if (this.toolcboOrdersStatus.Text == "已停" 
                && (!orderInfo.IsRepeat || (orderInfo.IsRepeat && (orderInfo.OrderStatus == "3" ))))
            {
                return true;
            }
            if (this.toolcboOrdersStatus.Text == "已作废"
               && (!orderInfo.IsRepeat || (orderInfo.IsRepeat && (orderInfo.OrderStatus == "4"))))
            {
                return true;
            }
            if (this.toolcboOrdersStatus.Text == "全部" && (!orderInfo.IsRepeat || (orderInfo.IsRepeat )))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置指定行显示的数据,以及绑定的数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <param name="orderInfo">绑定的数据</param>
        private void SetRowData(DataTableViewRow row, OrderInfo orderInfo)
        {
            if (row == null || row.Index < 0 || orderInfo == null)
                return;
            row.Tag = orderInfo;

            //红色表示作废，蓝色表示停止
            if (orderInfo.OrderStatus == "4")
                row.DefaultCellStyle.ForeColor = Color.Red;
            else if (orderInfo.IsStartStop)
                row.DefaultCellStyle.ForeColor = Color.Blue;

            row.Cells[this.colNumber.Index].Value = (row.Index + 1).ToString();

            row.Cells[this.colOrderNo.Index].Value = orderInfo.OrderNO;
            row.Cells[this.colOrderSubNo.Index].Value = orderInfo.OrderSubNO;
            row.Cells[this.colOrderClass.Index].Value = orderInfo.OrderClass;
            row.Cells[this.colEnterDate.Index].Value = orderInfo.EnterTime.ToString("yyyy-MM-dd HH:mm");
            row.Cells[this.colOrderText.Index].Value = orderInfo.OrderText;
            if (orderInfo.Dosage > 0f)
                row.Cells[this.colDosage.Index].Value = orderInfo.Dosage;
            row.Cells[this.colDosageUnits.Index].Value = orderInfo.DosageUnits;
            row.Cells[this.colAdministration.Index].Value = orderInfo.Administration;
            row.Cells[this.colFrequency.Index].Value = orderInfo.Frequency;
            if (orderInfo.StopTime != orderInfo.DefaultTime)
                row.Cells[this.colEndDate.Index].Value = orderInfo.StopTime.ToString("yyyy-MM-dd HH:mm");
            row.Cells[this.colDoctor.Index].Value = orderInfo.Doctor;
            row.Cells[this.colFreqDetail.Index].Value = orderInfo.FreqDetail;
            row.Cells[this.colNurse.Index].Value = orderInfo.Nurse;
            row.Cells[this.colPerformTime.Index].Value = orderInfo.PerformSchedule;
            row.Cells[this.colStopDcotor.Index].Value = orderInfo.StopDoctor;
            if (orderInfo.ProcessingEndDateTime != orderInfo.DefaultTime)
                row.Cells[this.colProcessingEndDateTime.Index].Value = orderInfo.ProcessingEndDateTime.ToString("yyyy-MM-dd HH:mm");
            else
                row.Cells[this.colProcessingEndDateTime.Index].Value = string.Empty;
            row.Cells[this.colStopNurse.Index].Value = orderInfo.StopNurse;
            if (orderInfo.ShortProcessingEndDateTime != orderInfo.DefaultTime)
                row.Cells[this.colShorttime.Index].Value = orderInfo.ShortProcessingEndDateTime.ToString("yyyy-MM-dd HH:mm");
            else
                row.Cells[this.colShorttime.Index].Value = string.Empty;
            row.Cells[this.colShortNurse.Index].Value = orderInfo.ShortStopNurse;
            if (this.toolcboOrdersType.SelectedIndex == 0)
            {
                this.colShorttime.Visible = false;
                this.colShortNurse.Visible = false;
                this.colStopNurse.Visible = true;
                this.colProcessingEndDateTime.Visible = true;
            }
            if (this.toolcboOrdersType.SelectedIndex == 1)
            {
                this.colShorttime.Visible = true; ;
                this.colShortNurse.Visible = true;
                this.colStopNurse.Visible = false;
                this.colProcessingEndDateTime.Visible = false;
            }
            row.Cells[colPerformResult.Index].Value = orderInfo.PerformResult;
        }

        private void toolcboOrdersType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrdersList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboOrdersStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrdersList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrdersList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        #region "打印医嘱记录"
        /// <summary>
        /// 加载护理记录单打印模板
        /// </summary>
        /// <param name="szReportName">护理记录单打印模板名</param>
        /// <returns>护理记录单打印模板byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            if (GlobalMethods.Misc.IsEmptyString(this.toolcboOrdersType.Text))
            {
                MessageBoxEx.ShowError("您未选择医嘱单长/临类型!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.ORDERS;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.toolcboOrdersType.Text;
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowError("医嘱本打印报表还没有制作!");
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("医嘱本打印报表内容下载失败!");
                return null;
            }
            return byteTempletData;
        }

        private bool GetSystemContext(string name, ref object value)
        {
            if (name == "查询起始时间")
            {
                value = this.tooldtpDateFrom.Value;
                return true;
            }
            if (name == "查询截止时间")
            {
                value = this.tooldtpDateTo.Value;
                return true;
            }
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        private ReportExplorerForm GetReportExplorerForm()
        {
            ReportExplorerForm reportExplorerForm = new ReportExplorerForm();
            reportExplorerForm.WindowState = FormWindowState.Maximized;
            reportExplorerForm.QueryContext +=
                new QueryContextEventHandler(this.ReportExplorerForm_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new ExecuteQueryEventHandler(this.ReportExplorerForm_ExecuteQuery);
            reportExplorerForm.NotifyNextReport +=
                new NotifyNextReportEventHandler(this.ReportExplorerForm_NotifyNextReport);
            return reportExplorerForm;
        }
        #endregion

        private void toolmnuPrintAll_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintOrderRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印医嘱记录!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPreview_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintOrderRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印医嘱记录!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintFrom_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintOrderRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印医嘱记录!");
                return;
            }

            SetPageNumberForm setPageNumberForm = new SetPageNumberForm();
            if (setPageNumberForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            int nStartPageNumber = setPageNumberForm.StartPageNumber;

            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                int start = this.dataTableView1.SelectedRows[0].Index;
                int end = this.dataTableView1.Rows.Count - 1;
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, start, end, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = nStartPageNumber;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintContinued_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintOrderRec.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印医嘱记录!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                int start = this.dataTableView1.SelectedRows[0].Index;
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("续打行号", start);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //子医嘱对以下列不显示
            if (e.ColumnIndex == this.colOrderClass.Index || e.ColumnIndex == this.colEnterDate.Index)
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                OrderInfo orderInfo = row.Tag as OrderInfo;
                if (orderInfo != null && orderInfo.OrderSubNO != "1")
                    e.Value = string.Empty;
            }
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }

        private void ReportExplorerForm_ExecuteQuery(object sender, Heren.Common.Report.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void ReportExplorerForm_NotifyNextReport(object sender, Heren.Common.Report.NotifyNextReportEventArgs e)
        {
            e.ReportData = this.GetReportFileData(e.ReportName);
        }

        private void toolcboOrdersCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrdersList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboTreatment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrdersList();
          
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}
