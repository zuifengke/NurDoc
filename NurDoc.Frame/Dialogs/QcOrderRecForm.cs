// ***********************************************************
// 护理电子病历系统,病历指控窗口.
// Creator:YeChongchong  Date:2014-1-5
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.DockSuite;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Frame.DockForms;
using Heren.NurDoc.PatPage.Document;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Frame
{
    internal partial class QcOrderRecForm : HerenForm
    {
        /// <summary>
        /// 病人信息表 用于前后病人筛选
        /// </summary>
        private DataTable m_dtPatVisit = null;

        /// <summary>
        /// 缓存当前索引
        /// </summary>
        private int m_index = 0;

        /// <summary>
        /// 当前显示的病人信息
        /// </summary>
        private PatVisitInfo m_patVisitInfo = null;

        public QcOrderRecForm(DataTable dtPatVisit, int Index)
        {
            this.InitializeComponent();
            this.m_dtPatVisit = dtPatVisit;
            this.m_index = Index;
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(dtPatVisit.Rows[Index]);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();

            if (this.toolcboOrdersType.Items.Count > 0)
                this.toolcboOrdersType.SelectedIndex = 0;
            this.RefreshView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Size size = new Size(SystemInformation.WorkingArea.Width, SystemInformation.WorkingArea.Height);
            GlobalMethods.UI.LocateScreenCenter(this, size);
        }

        public void RefreshView()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadOrdersList();
            this.LoadExamineStatus();
            this.SelectedByStatus();
            if (this.dataTableView1.Rows.Count > 0)
            {
                this.dataTableView1.Rows[0].Selected = true;
            }
            this.BtnHandler();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private PatVisitInfo GetPatVisitFromDataRow(DataRow drPatVisit)
        {
            PatVisitInfo patVisitInfo = new PatVisitInfo();
            short shRet = PatVisitService.Instance.GetPatVisitInfo(drPatVisit[0].ToString(), drPatVisit[1].ToString(), ref patVisitInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                MessageBoxEx.Show("病人信息查询失败");
                return null;
            }
            return patVisitInfo;
        }

        /// <summary>
        /// 读取并绑定医嘱信息
        /// </summary>
        /// <returns>true or false</returns>
        private bool LoadOrdersList()
        {
            this.dataTableView1.Rows.Clear();
            if (this.m_patVisitInfo == null)
                return false;
            this.Update();
            string szPatientID = this.m_patVisitInfo.PatientID;
            string szVisitID = this.m_patVisitInfo.VisitID;
            this.toollblPatientID.Text = this.m_patVisitInfo.PatientID;
            this.toollblPatientName.Text = this.m_patVisitInfo.PatientName;
            DateTime dtBeginDate = SysTimeService.Instance.Now.AddDays(-1).Date;
            if (this.m_patVisitInfo != null)
                dtBeginDate = this.m_patVisitInfo.VisitTime.Date;
            DateTime dtEndDate = GlobalMethods.SysTime.GetDayLastTime(this.GetEndWeekDate(true));

            bool bIsRepeat = this.toolcboOrdersType.SelectedIndex == 0;

            //医嘱标识 0-医生下达的医嘱 1-护士转抄的医嘱
            List<OrderInfo> lstOrderInfo = null;
            short shRet = PatVisitService.Instance.GetPerformOrderList(szPatientID, szVisitID
                , dtBeginDate, dtEndDate, ref lstOrderInfo);
            if (shRet != SystemConst.ReturnValue.OK && shRet != SystemConst.ReturnValue.CANCEL)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show("查询医嘱列表失败!");
                return false;
            }
            if (lstOrderInfo == null || lstOrderInfo.Count <= 0)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }

            //先对父子医嘱进行分组
            Dictionary<string, List<OrderInfo>> dicOrderList =
                new Dictionary<string, List<OrderInfo>>();
            for (int index = 0; index < lstOrderInfo.Count; index++)
            {
                OrderInfo orderInfo = lstOrderInfo[index];
                if (orderInfo == null || string.IsNullOrEmpty(orderInfo.OrderNO))
                    continue;
                if (orderInfo.IsRepeat != bIsRepeat)
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

            List<OrderInfo>[] arrOrderList =
                new List<OrderInfo>[dicOrderList.Values.Count];
            dicOrderList.Values.CopyTo(arrOrderList, 0);

            //倒序加载并显示医嘱列表
            for (int nListIndex = arrOrderList.Length - 1; nListIndex >= 0; nListIndex--)
            {
                List<OrderInfo> lstOrderList = arrOrderList[nListIndex];
                for (int index = 0; index < lstOrderList.Count; index++)
                {
                    OrderInfo orderInfo = lstOrderList[index];
                    if (orderInfo == null)
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
            //默认显示未审核
            row.Cells[this.Status.Index].Value = ServerData.ExamineStatus.QC_NONE;
        }

        /// <summary>
        /// 获取并绑定审核状态信息
        /// </summary>
        private void LoadExamineStatus()
        {
            if (this.m_patVisitInfo == null)
            {
                return;
            }
            List<QCExamineInfo> lstQCExamineInfo = new List<QCExamineInfo>();
            short shRet = QCExamineService.Instance.GetQcExamineInfos(ServerData.ExamineType.ORDERS,
                this.m_patVisitInfo.PatientID, this.m_patVisitInfo.VisitID, ref lstQCExamineInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return;
            }

            for (int RowIndex = 0; RowIndex < this.dataTableView1.Rows.Count; RowIndex++)
            {
                OrderInfo orderInfo = this.dataTableView1.Rows[RowIndex].Tag as OrderInfo;
                string szStatusText = this.Matching(orderInfo, lstQCExamineInfo);
                this.dataTableView1.Rows[RowIndex].Cells[this.Status.Index].Value = szStatusText;
            }
        }

        private string Matching(OrderInfo orderInfo, List<QCExamineInfo> lstQcExamineInfo)
        {
            if (lstQcExamineInfo == null || lstQcExamineInfo.Count <= 0)
            {
                return ServerData.ExamineStatus.QC_NONE;
            }
            //用于存放医嘱单医嘱编号和子编号的特殊格式 用“;”组合
            string key = orderInfo.OrderNO + ";" + orderInfo.OrderSubNO;
            foreach (QCExamineInfo qcExamineInfo in lstQcExamineInfo)
            {
                if (key == qcExamineInfo.QcContentKey)
                {
                    if (qcExamineInfo.QcExamineStatus == "1")
                    {
                        return ServerData.ExamineStatus.QC_OK;
                    }
                    return ServerData.ExamineStatus.QC_MARK;
                }
            }
            return ServerData.ExamineStatus.QC_NONE;
        }

        /// <summary>
        /// 获取当前病人住院截止周的日期
        /// </summary>
        /// <param name="bStopToCurrentTime">是否截止到当前时间</param>
        /// <returns>截止周日期</returns>
        private DateTime GetEndWeekDate(bool bStopToCurrentTime)
        {
            if (this.m_patVisitInfo == null)
                return SysTimeService.Instance.Now;
            DateTime dtWeekDate = this.m_patVisitInfo.DischargeTime;
            if (dtWeekDate != this.m_patVisitInfo.DefaultTime)
                return dtWeekDate;
            return bStopToCurrentTime ? SysTimeService.Instance.Now : DateTime.MaxValue;
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

        /// <summary>
        /// 根据审核状态筛选数据
        /// </summary>
        private void SelectedByStatus()
        {
            if (this.toolcboStatus.SelectedIndex <= 0)
            {
                return;
            }
            else
            {
                //偏移量
                for (int Index = 0; Index < this.dataTableView1.Rows.Count; Index++)
                {
                    if (this.dataTableView1.Rows[Index].Cells[this.Status.Index].Value.ToString() != this.toolcboStatus.Text)
                    {
                        this.dataTableView1.Rows.RemoveAt(Index--);
                    }
                }
            }
        }

        private void BtnHandler()
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                this.toolbtnQc.Enabled = false;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = false;
                return;
            }
            if (this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString() == ServerData.ExamineStatus.QC_NONE)
            {
                this.toolbtnQc.Enabled = true;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = true;
            }
            else if (this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString() == ServerData.ExamineStatus.QC_OK)
            {
                this.toolbtnQc.Enabled = false;
                this.toolbtnQcCancel.Enabled = true;
                this.toolbtnQcQuestion.Enabled = false;
            }
            else
            {
                this.toolbtnQc.Enabled = true;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = true;
            }
        }

        private void toolbtnPrev_Click(object sender, EventArgs e)
        {
            if (this.m_index <= 0)
            {
                MessageBox.Show("已经是第一位病人");
                return;
            }
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(this.m_dtPatVisit.Rows[--this.m_index]);
            this.RefreshView();
        }

        private void toolbtnNext_Click(object sender, EventArgs e)
        {
            if (this.m_index >= this.m_dtPatVisit.Rows.Count)
            {
                MessageBox.Show("已经是最后一位病人");
                return;
            }
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(this.m_dtPatVisit.Rows[++this.m_index]);
            this.RefreshView();
        }

        private void toolbtnQc_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                return;
            }
            string szCurrentRowStatus = this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString();
            OrderInfo currentRowOrderInfo = this.dataTableView1.CurrentRow.Tag as OrderInfo;
            string key = currentRowOrderInfo.OrderNO + ";" + currentRowOrderInfo.OrderSubNO;
            if (szCurrentRowStatus == ServerData.ExamineStatus.QC_NONE)
            {
                QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, key, ServerData.ExamineType.ORDERS);
                qcExamineInfo.QcExamineStatus = "1";
                qcExamineInfo.QcExamineContent = string.Empty;
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核失败！");
                    return;
                }
            }
            else if (szCurrentRowStatus == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.ORDERS, this.m_patVisitInfo.PatientID, this.m_patVisitInfo.VisitID, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }
                qcExamineInfo.QcExamineStatus = "1";
                qcExamineInfo.QcExamineContent = string.Empty;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核失败！");
                    return;
                }
            }
            this.RefreshView();
        }

        private void toolbtnQcCancel_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                return;
            }
            string szCurrentRowStatus = this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString();
            OrderInfo currentRowOrderInfo = this.dataTableView1.CurrentRow.Tag as OrderInfo;
            string key = currentRowOrderInfo.OrderNO + ";" + currentRowOrderInfo.OrderSubNO;
            if (szCurrentRowStatus == ServerData.ExamineStatus.QC_OK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.ORDERS, this.m_patVisitInfo.PatientID, this.m_patVisitInfo.VisitID, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("理由", qcExamineInfo.QcExamineContent);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("取消审核失败！");
                    return;
                }
            }
            this.RefreshView();
        }

        private void toolbtnQcQuestion_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.CurrentRow == null)
            {
                return;
            }
            string szCurrentRowStatus = this.dataTableView1.CurrentRow.Cells[this.Status.Index].Value.ToString();
            OrderInfo currentRowOrderInfo = this.dataTableView1.CurrentRow.Tag as OrderInfo;
            string key = currentRowOrderInfo.OrderNO + ";" + currentRowOrderInfo.OrderSubNO;
            if (szCurrentRowStatus == ServerData.ExamineStatus.QC_NONE)
            {
                ReasonDialog reasonDialog = new ReasonDialog("理由", string.Empty);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, key, ServerData.ExamineType.ORDERS);
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("标记审核内容失败！");
                    return;
                }
            }
            else if (szCurrentRowStatus == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.ORDERS, this.m_patVisitInfo.PatientID, this.m_patVisitInfo.VisitID, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("理由", qcExamineInfo.QcExamineContent);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("标记审核内容失败！");
                    return;
                }
            }
            this.RefreshView();
        }

        private void toolcboOrdersType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshView();
        }

        private void dataTableView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.BtnHandler();
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                Point mousePosition = Control.MousePosition;
                this.contextMenuStrip1.Show(mousePosition.X, mousePosition.Y);
            }
        }

        private void btnQcAll_Click(object sender, EventArgs e)
        {
            DialogResult dRValue = MessageBox.Show("是否审核所有未审核和标记中的医嘱单", "审核", MessageBoxButtons.YesNo);
            if (dRValue != DialogResult.Yes)
            {
                return;
            }
            int ErrorCount = 0;
            int NeedQcCount = this.dataTableView1.Rows.Count;
            foreach (DataGridViewRow dgvRow in this.dataTableView1.Rows)
            {
                string szRowStatus = dgvRow.Cells[this.Status.Index].Value.ToString();
                OrderInfo rowOrderInfo = dgvRow.Tag as OrderInfo;
                string key = rowOrderInfo.OrderNO + ";" + rowOrderInfo.OrderSubNO;
                if (szRowStatus == ServerData.ExamineStatus.QC_NONE)
                {
                    QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, key, ServerData.ExamineType.ORDERS);
                    qcExamineInfo.QcExamineStatus = "1";
                    qcExamineInfo.QcExamineContent = string.Empty;
                    short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        ErrorCount++;
                    }
                }
                else if (szRowStatus == ServerData.ExamineStatus.QC_MARK)
                {
                    QCExamineInfo qcExamineInfo = new QCExamineInfo();
                    short shRet = QCExamineService.Instance.GetQcExamineInfo(key, ServerData.ExamineType.ORDERS, this.m_patVisitInfo.PatientID, this.m_patVisitInfo.VisitID, ref qcExamineInfo);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        ErrorCount++;
                        continue;
                    }
                    qcExamineInfo.QcExamineStatus = "1";
                    qcExamineInfo.QcExamineContent = string.Empty;
                    qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                    qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                    qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                    shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        ErrorCount++;
                    }
                }
                else
                {
                    NeedQcCount--;
                }
            }
            StringBuilder sbError = new StringBuilder();
            sbError.AppendFormat("需审核医嘱单：{0}", NeedQcCount);
            sbError.AppendLine();
            sbError.AppendFormat("失败：{0}", ErrorCount);
            MessageBox.Show(sbError.ToString());

            this.RefreshView();
        }

        private void toolcboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshView();
        }
    }
}