// **************************************************************
// 护理电子病历系统公用模块之医嘱数据导入窗口
// Creator:YangMingkun  Date:2012-9-5
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Import
{
    public partial class OrdersImportForm : HerenForm
    {
        private DataTable m_selectedOrderList = null;

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 获取当前用户筛选医嘱列表
        /// </summary>
        [Browsable(false)]
        [Description("获取当前用户筛选医嘱列表")]
        public DataTable SelectedOrderTable
        {
            get { return this.m_selectedOrderList; }
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

        private string m_selectedDefaultItem = string.Empty;

        /// <summary>
        /// 获取或设置默认的选择类别
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        [Description("获取或设置默认的选择类别")]
        public string SelectedDefaultItem
        {
            get { return this.m_selectedDefaultItem; }
            set { this.m_selectedDefaultItem = value; }
        }

        private string m_szCurrentOrderNO = string.Empty;

        public OrdersImportForm()
        {
            this.InitializeComponent();
            this.lvOrderType.Items.Add(new ListViewItem("长期")).Checked = false;
            this.lvOrderType.Items.Add(new ListViewItem("临时")).Checked = false;
            this.lvOrderType.Items.Add(new ListViewItem("液体")).Checked = true;
            this.lvOrderType.Items.Add(new ListViewItem("未停")).Checked = false;
            this.lvOrderType.Items.Add(new ListViewItem("已停")).Checked = false;
            this.lvOrderType.Items.Add(new ListViewItem("所有")).Checked = false;
            this.lvOrderType.Items.Add(new ListViewItem("西药")).Checked = false;
            this.lvOrderType.Items.Add(new ListViewItem("膳食")).Checked = false;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

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
            string szOldPatVisit = this.lvOrderType.Tag as string;
            string szCurrPatVisit = string.Concat(this.m_patientID, "_", this.m_visitID);
            if (szOldPatVisit == szCurrPatVisit)
            {
                this.SelectedDefultItem(this.m_selectedDefaultItem);
                this.ShowOrderList();
                return;
            }
            this.m_bValueChangedEnabled = false;
            DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1);
            if (PatientTable.Instance.ActivePatient != null)
                dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime;
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = SysTimeService.Instance.Now;

            this.InitSystemOption();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SelectedDefultItem(this.m_selectedDefaultItem);
            this.LoadOrderList();
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载医嘱列表
        /// </summary>
        private void LoadOrderList()
        {
            this.dataTableView1.Rows.Clear();
            this.dataTableView1.Tag = null;
            this.chkSelectAll.Checked = false;
            Application.DoEvents();

            string szPatientID = this.m_patientID;
            string szVisitID = this.m_visitID;
            this.lvOrderType.Tag = string.Concat(this.m_patientID, "_", this.m_visitID);

            if (GlobalMethods.Misc.IsEmptyString(szPatientID)
                || GlobalMethods.Misc.IsEmptyString(szVisitID))
            {
                return;
            }

            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value);
            List<OrderInfo> lstOrderInfo = null;
            short shRet = PatVisitService.Instance.GetPerformOrderList(szPatientID, szVisitID
                , dtBeginTime, dtEndTime, ref lstOrderInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("查询医嘱列表失败!");
                return;
            }
            this.dataTableView1.Tag = lstOrderInfo;
            this.ShowOrderList();
        }

        /// <summary>
        /// 根据系统设置，显示或者隐藏医嘱计划 以及事件绑定
        /// </summary>
        private void InitSystemOption()
        {
            if (!SystemContext.Instance.SystemOption.PlanOrderRec)
            {
                this.splitContainer2.Panel2Collapsed = true;
                this.dataTableView1.CellMouseClick -=
                    new DataGridViewCellMouseEventHandler(this.dataTableView1_CellMouseClick);
            }
            else
            {
                this.splitContainer2.Panel2Collapsed = false;
                this.dataTableView1.CellMouseClick -=
                    new DataGridViewCellMouseEventHandler(this.dataTableView1_CellMouseClick);
                this.dataTableView1.CellMouseClick +=
                    new DataGridViewCellMouseEventHandler(this.dataTableView1_CellMouseClick);
            }
        }

        /// <summary>
        /// 选择默认列
        /// </summary>
        /// <param name="szSelecteItem">默认选项</param>
        private void SelectedDefultItem(string szSelecteItem)
        {
            if (GlobalMethods.Misc.IsEmptyString(szSelecteItem))
                return;
            foreach (ListViewItem item in this.lvOrderType.Items)
            {
                if (item.Text == szSelecteItem)
                {
                    item.Checked = true;
                    item.Selected = true;
                }
                else
                {
                    item.Checked = false;
                    item.Selected = false;
                }
            }
        }

        private void ShowOrderList()
        {
            this.dataTableView1.Rows.Clear();
            this.chkSelectAll.Checked = false;
            List<OrderInfo> lstOrderInfo = this.dataTableView1.Tag as List<OrderInfo>;
            if (lstOrderInfo == null || lstOrderInfo.Count <= 0)
                return;

            //先对父子医嘱进行分组
            Dictionary<string, List<OrderInfo>> dicOrderList =
                new Dictionary<string, List<OrderInfo>>();
            for (int index = 0; index < lstOrderInfo.Count; index++)
            {
                OrderInfo orderInfo = lstOrderInfo[index];
                if (orderInfo == null || string.IsNullOrEmpty(orderInfo.OrderNO))
                    continue;

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
            }

            List<OrderInfo>[] arrOrderList = new List<OrderInfo>[dicOrderList.Values.Count];
            dicOrderList.Values.CopyTo(arrOrderList, 0);
            for (int nListIndex = arrOrderList.Length - 1; nListIndex >= 0; nListIndex--)
            {
                List<OrderInfo> lstOrderList = arrOrderList[nListIndex];

                bool ordercomp = CheckOrderNeed(lstOrderList);
                //count记录复合医嘱中除去不符合筛选条件的医嘱数目
                //int count = 0;
                //bool badd = true;
                for (int index = 0; index < lstOrderList.Count; index++)
                {
                    OrderInfo orderInfo = lstOrderList[index];
                    //if (!this.CheckOrderShouldDisplay(orderInfo))
                    //    continue;

                    //根据勾选条件获取医嘱列表的交集
                    if (this.lvOrderType.Items[0].Checked)
                    {
                        if (!this.CheckOrderShouldDisplay(orderInfo))
                            continue;
                    }
                    if (this.lvOrderType.Items[1].Checked)
                    {
                        if (!this.CheckOrderShouldDispla(orderInfo))
                            continue;
                    }
                    if (this.lvOrderType.Items[2].Checked)
                    {
                        if (!this.CheckOrderShouldDispl(orderInfo))
                        {
                            //badd=false;
                            continue;
                        }
                    }
                    if (this.lvOrderType.Items[3].Checked)
                    {
                        if (!this.CheckOrderShouldDisp(orderInfo))
                            continue;
                    }
                    if (this.lvOrderType.Items[4].Checked)
                    {
                        if (!this.CheckOrderShouldDis(orderInfo))
                            continue;
                    }
                    if (this.lvOrderType.Items[5].Checked)
                    {
                        if (!this.CheckOrderShouldDi(orderInfo))
                            continue;
                    }
                    if (this.lvOrderType.Items[6].Checked)
                    {
                        if (!this.CheckOrderShouldWesternMed(orderInfo))
                            continue;
                    }
                    if (this.lvOrderType.Items[7].Checked)
                    {
                        if (!this.CheckOrderShouldWesternMe(orderInfo))
                            continue;
                    }
                    if (!this.lvOrderType.Items[0].Checked && !this.lvOrderType.Items[1].Checked && !this.lvOrderType.Items[2].Checked &&
                        !this.lvOrderType.Items[3].Checked && !this.lvOrderType.Items[4].Checked && !this.lvOrderType.Items[5].Checked &&
                        !this.lvOrderType.Items[6].Checked && !this.lvOrderType.Items[7].Checked)
                        continue;

                    int nRowIndex = this.dataTableView1.Rows.Add();
                    //count++;
                    DataGridViewRow row = this.dataTableView1.Rows[nRowIndex];
                    OrderInfo newOrderInfo = new OrderInfo();
                    newOrderInfo = orderInfo.Clone() as OrderInfo;
                    if (!ordercomp)
                    {
                        newOrderInfo.OrderSubNO = "1";
                    }
                    row.Tag = newOrderInfo;

                    //红色表示作废，蓝色表示停止
                    if (orderInfo.OrderStatus == "4")
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    else if (orderInfo.IsStartStop)
                        row.DefaultCellStyle.ForeColor = Color.Blue;

                    if (lstOrderList.Count > 1 && ordercomp)
                    {
                        if (index == 0)
                            row.Cells[this.colSubFlag.Index].Value = "┓";
                        else if (index == lstOrderList.Count - 1)
                            row.Cells[this.colSubFlag.Index].Value = "┛";
                        else
                            row.Cells[this.colSubFlag.Index].Value = "┃";
                    }

                    row.Cells[this.colRepeatIndicator.Index].Value = orderInfo.IsRepeat ? "长" : "临";
                    row.Cells[this.colOrderClass.Index].Value = orderInfo.OrderClass;
                    row.Cells[this.colEnterDate.Index].Value = orderInfo.EnterTime.ToString("yyyy-MM-dd HH:mm");
                    row.Cells[this.colOrderText.Index].Value = orderInfo.OrderText;
                    if (orderInfo.Dosage > 0f)
                        row.Cells[this.colDosage.Index].Value = orderInfo.Dosage;
                    row.Cells[this.colDosageUnits.Index].Value = orderInfo.DosageUnits;
                    row.Cells[this.colAdministration.Index].Value = orderInfo.Administration;
                    row.Cells[this.colFrequency.Index].Value = orderInfo.Frequency;
                    row.Cells[this.colFreqDetail.Index].Value = orderInfo.FreqDetail;
                    row.Cells[this.colDoctor.Index].Value = orderInfo.Doctor;
                    row.Cells[this.colNurse.Index].Value = orderInfo.Nurse;
                    row.Cells[this.OrderNo.Index].Value = orderInfo.OrderNO;
                    row.Cells[this.OrderSubNo.Index].Value = orderInfo.OrderSubNO;
                    if (orderInfo.StopTime != orderInfo.DefaultTime)
                        row.Cells[this.colEndDate.Index].Value = orderInfo.StopTime.ToString("yyyy-MM-dd HH:mm");
                }
                //如果复合医嘱复合筛选条件的医嘱少于总的医嘱数
                //if (lstOrderList.Count > count)
                //{
                //    for (int index=0; index < count; index++)
                //    {
                //        DataGridViewRow row = this.dataTableView1.Rows[this.dataTableView1.Rows.Count - 1-index];
                //        string m = string.Empty;
                //        row.Cells[this.colSubFlag.Index].Value = m;
                //    }
                //    DataGridViewRow row = this.dataTableView1.Rows[this.dataTableView1.Rows.Count - count];
                //    string m = string.Empty;
                //    row.Cells[this.colSubFlag.Index].Value = m;
            }
        }

        private bool CheckOrderNeed(List<OrderInfo> lstOrderList)
        {
            for (int index = 0; index < lstOrderList.Count; index++)
            {
                OrderInfo orderInfo = lstOrderList[index];

                //根据勾选条件获取医嘱列表的交集
                if (this.lvOrderType.Items[0].Checked)
                {
                    if (!this.CheckOrderShouldDisplay(orderInfo))
                        return false;
                }
                if (this.lvOrderType.Items[1].Checked)
                {
                    if (!this.CheckOrderShouldDispla(orderInfo))
                        return false;
                }
                if (this.lvOrderType.Items[2].Checked)
                {
                    if (!this.CheckOrderShouldDispl(orderInfo))
                    {
                        //badd=false;
                        return false;
                    }
                }
                if (this.lvOrderType.Items[3].Checked)
                {
                    if (!this.CheckOrderShouldDisp(orderInfo))
                        return false;
                }
                if (this.lvOrderType.Items[4].Checked)
                {
                    if (!this.CheckOrderShouldDis(orderInfo))
                        return false;
                }
                if (this.lvOrderType.Items[5].Checked)
                {
                    if (!this.CheckOrderShouldDi(orderInfo))
                        return false;
                }
                if (this.lvOrderType.Items[6].Checked)
                {
                    if (!this.CheckOrderShouldWesternMed(orderInfo))
                        return false;
                }
                if (this.lvOrderType.Items[7].Checked)
                {
                    if (!this.CheckOrderShouldWesternMe(orderInfo))
                        return false;
                }
                if (!this.lvOrderType.Items[0].Checked && !this.lvOrderType.Items[1].Checked && !this.lvOrderType.Items[2].Checked &&
                    !this.lvOrderType.Items[3].Checked && !this.lvOrderType.Items[4].Checked && !this.lvOrderType.Items[5].Checked &&
                    !this.lvOrderType.Items[6].Checked && !this.lvOrderType.Items[7].Checked)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 按照勾选的过滤条件,检查指定医嘱是否应该显示
        /// </summary>
        /// <param name="orderInfo">医嘱</param>
        /// <returns>是否应该显示</returns>
        //private bool CheckOrderShouldDisplay(OrderInfo orderInfo)
        //{
        //    //并集
        //    if (orderInfo == null || orderInfo.OrderClass == null)
        //        return false;

        //    if (this.lvOrderType.Items[0].Checked && orderInfo.IsRepeat)
        //        return true;

        //    if (this.lvOrderType.Items[1].Checked && !orderInfo.IsRepeat)
        //        return true;

        //    if (this.lvOrderType.Items[2].Checked)
        //    {
        //        if (SystemContext.Instance.IsLiquidOrder(orderInfo.Administration))
        //            return true;
        //        if (orderInfo.Administration == null)
        //            orderInfo.Administration = "";
        //        if (orderInfo.DosageUnits == null)
        //            orderInfo.DosageUnits = "";
        //        if (orderInfo.Administration.Contains("静滴")
        //            || orderInfo.DosageUnits == "毫升" || orderInfo.DosageUnits.ToLower() == "ml"
        //            || orderInfo.DosageUnits == "升" || orderInfo.DosageUnits.ToLower() == "l")
        //            return true;
        //    }
        //    if (this.lvOrderType.Items[3].Checked && orderInfo.OrderStatus == "2" && orderInfo.IsRepeat)
        //        return true;
        //    if ((this.lvOrderType.Items[4].Checked && !orderInfo.IsRepeat) || (this.lvOrderType.Items[4].Checked && orderInfo.IsRepeat && (orderInfo.OrderStatus == "3" || orderInfo.OrderStatus == "4")))
        //        return true;
        //    if (this.lvOrderType.Items[5].Checked)
        //        return true;
        //    return false;
        //}

        #region //逐个判断勾选的条件来查询医嘱并集数据列表
        private bool CheckOrderShouldDisplay(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if (this.lvOrderType.Items[0].Checked && orderInfo.IsRepeat)
                return true;
            return false;
        }

        private bool CheckOrderShouldDispla(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if (this.lvOrderType.Items[1].Checked && !orderInfo.IsRepeat)
                return true;

            return false;
        }

        private bool CheckOrderShouldDispl(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if (this.lvOrderType.Items[2].Checked)
            {
                if (SystemContext.Instance.IsLiquidOrder(orderInfo.Administration))
                    return true;
                if (orderInfo.Administration == null)
                    orderInfo.Administration = string.Empty;
                if (orderInfo.DosageUnits == null)
                    orderInfo.DosageUnits = string.Empty;
                if (orderInfo.Administration.Contains("静滴")
                    || orderInfo.DosageUnits == "毫升" || orderInfo.DosageUnits.ToLower() == "ml"
                    || orderInfo.DosageUnits == "升" || orderInfo.DosageUnits.ToLower() == "l")
                    return true;
            }
            return false;
        }

        private bool CheckOrderShouldDisp(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if (this.lvOrderType.Items[3].Checked && orderInfo.OrderStatus == "2" && orderInfo.IsRepeat)
                return true;
            return false;
        }

        private bool CheckOrderShouldDis(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if ((this.lvOrderType.Items[4].Checked && !orderInfo.IsRepeat) || (this.lvOrderType.Items[4].Checked && orderInfo.IsRepeat && (orderInfo.OrderStatus == "3" || orderInfo.OrderStatus == "4")))
                return true;
            return false;
        }

        private bool CheckOrderShouldDi(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if (this.lvOrderType.Items[5].Checked)
                return true;
            return false;
        }

        private bool CheckOrderShouldWesternMed(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if (this.lvOrderType.Items[6].Checked && orderInfo.OrderClass == "西药")
                return true;
            return false;
        }

        private bool CheckOrderShouldWesternMe(OrderInfo orderInfo)
        {
            if (orderInfo == null || orderInfo.OrderClass == null)
                return false;

            if (this.lvOrderType.Items[7].Checked && orderInfo.OrderClass == "膳食")
                return true;
            return false;
        }

        #endregion

        /// <summary>
        /// 当前表格中父医嘱勾选的时候，子医嘱默认勾选
        /// </summary>
        /// <param name="rowIndex">当前行号</param>
        /// <param name="bIsChecked">是否被勾选</param>
        private void HandleOrdersSelect(int rowIndex, bool bIsChecked)
        {
            if (this.dataTableView1 != null && this.dataTableView1.Rows.Count <= 1)
                return;
            if (rowIndex < 0)
                return;
            for (int index = rowIndex + 1; index < this.dataTableView1.Rows.Count - 1; index++)
            {
                DataGridViewRow row = this.dataTableView1.Rows[index];
                if (row.Cells[this.colSubFlag.Index].Value != null
                    && row.Cells[this.colSubFlag.Index].Value.ToString().Trim() == "┃")
                    row.Cells[this.colCheckOrder.Index].Value = bIsChecked;
                else if (row.Cells[this.colSubFlag.Index].Value != null
                    && row.Cells[this.colSubFlag.Index].Value.ToString().Trim() == "┛")
                {
                    row.Cells[this.colCheckOrder.Index].Value = bIsChecked;
                    return;
                }
                else if (row.Cells[this.colSubFlag.Index].Value != null
                    && row.Cells[this.colSubFlag.Index].Value.ToString().Trim() == "┓")
                    return;
                else if (row.Cells[this.colSubFlag.Index].Value != null
               && row.Cells[this.colSubFlag.Index].Value.ToString().Trim() == string.Empty)
                    return;
                else if (row.Cells[this.colSubFlag.Index].Value == null)
                    return;
            }
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrderList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnQuery_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrderList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void lvOrderType_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = this.lvOrderType.GetItemAt(e.X, e.Y);
            if (item == null)
                return;
            //修改导入医嘱分类筛选为单选，故取消勾选框。
            foreach (ListViewItem Checkeditem in this.lvOrderType.Items)
            {
                Checkeditem.Checked = false;
            }

            Rectangle rect = this.lvOrderType.GetItemRect(item.Index, ItemBoundsPortion.Label);
            if (rect.Contains(e.Location))
            {
                item.Checked = true;
                this.ShowOrderList();
            }
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.colCheckOrder.Index)
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                if (row == null)
                    return;
                bool checkedValue = false;
                object cellValue = row.Cells[this.colCheckOrder.Index].Value;
                if (cellValue != null)
                    checkedValue = GlobalMethods.Convert.StringToValue(cellValue.ToString(), false);
                row.Cells[this.colCheckOrder.Index].Value = !checkedValue;
                if (row.Cells[this.colSubFlag.Index].Value != null
                    && row.Cells[this.colSubFlag.Index].Value.ToString() == "┓")
                    this.HandleOrdersSelect(e.RowIndex, !checkedValue);
            }
        }

        private void dataTableView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex <= 0)
            {
                return;
            }
            string szOrderNO = this.dataTableView1.Rows[e.RowIndex].Cells[this.OrderNo.Index].Value.ToString();
            if (GlobalMethods.Misc.IsEmptyString(this.m_patientID)
                || GlobalMethods.Misc.IsEmptyString(this.m_visitID) || this.m_szCurrentOrderNO == szOrderNO)
            {
                return;
            }
            List<PlanOrderInfo> lstOrderInfo = null;
            short shRet = PatVisitService.Instance.GetPlanOrderList(this.m_patientID, this.m_visitID
                , szOrderNO, ref lstOrderInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("查询医嘱拆分列表失败!");
                return;
            }
            this.m_szCurrentOrderNO = szOrderNO;
            this.dataTableView2.Rows.Clear();
            foreach (PlanOrderInfo planOrderInfo in lstOrderInfo)
            {
                int nRowIndex = this.dataTableView2.Rows.Add();

                DataGridViewRow row = this.dataTableView2.Rows[nRowIndex];
                if (planOrderInfo.ScheduleTime != planOrderInfo.DefaultTime)
                {
                    row.Cells[this.colScheduleTime.Index].Value = planOrderInfo.ScheduleTime.ToString("yyyy-MM-dd HH:mm");
                }
                row.Cells[this.colExecClass.Index].Value = planOrderInfo.ExecClass;
                row.Cells[this.colPerformedFlag.Index].Value = planOrderInfo.PerformedFlag;
                row.Cells[this.colOperator.Index].Value = planOrderInfo.Operator;
                if (planOrderInfo.OperatingTime != planOrderInfo.DefaultTime)
                {
                    row.Cells[this.colOperatingTime.Index].Value = planOrderInfo.OperatingTime.ToString("yyyy-MM-dd HH:mm");
                }
                row.Cells[this.colRealDosage.Index].Value = planOrderInfo.RealDosage == 0 ? string.Empty : planOrderInfo.RealDosage.ToString();
            }
        }

        private void dataTableView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //子医嘱对以下列不显示
            if ((e.ColumnIndex == this.colRepeatIndicator.Index
                || e.ColumnIndex == this.colOrderClass.Index || e.ColumnIndex == this.colEnterDate.Index))
            {
                DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
                OrderInfo orderInfo = row.Tag as OrderInfo;
                if (orderInfo != null && orderInfo.OrderSubNO != "1")
                    e.Value = string.Empty;
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
                    row.Cells[this.colCheckOrder.Index].Value = this.chkSelectAll.Checked;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            //判断勾选中的医嘱导入，选中而不勾选的不导入
            for (int index = 0; index < this.dataTableView1.Rows.Count; index++)
            {
                DataGridViewRow row = this.dataTableView1.Rows[index];
                if (row == null)
                    continue;
                object cellValue = row.Cells[this.colCheckOrder.Index].Value;
                if (cellValue != null && cellValue.ToString() == "True")
                    row.Selected = true;
                else
                    row.Selected = false;
            }
            this.m_selectedOrderList = GlobalMethods.Table.GetDataTable(this.dataTableView1, true, 0);
            this.DialogResult = DialogResult.OK;
        }
    }
}