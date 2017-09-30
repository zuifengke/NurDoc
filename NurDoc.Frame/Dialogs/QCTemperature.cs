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
using Heren.Common.Report;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Frame.DockForms;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class QcTemperature : HerenForm
    {
        //当前页面病人信息
        private PatVisitInfo m_patVisitInfo = null;

        //缓存显示的周次，防止显示的周次和体温单显示数据不同
        private long m_week = 0;

                /// <summary>
        /// 病人信息表 用于前后病人筛选
        /// </summary>
        private DataTable m_dtPatVisit = null;

        /// <summary>
        /// 缓存当前索引
        /// </summary>
        private int m_index = 0;

        public QcTemperature(DataTable dtPatVisit, int Index)
        {
            this.InitializeComponent();
            this.m_dtPatVisit = dtPatVisit;
            this.m_index = Index;
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(dtPatVisit.Rows[Index]);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.LoadBodyTemperatureTemplet();
            this.reportDesigner1.Focus();
            this.toollblPatientID.Text = this.GetPatientId();
            this.toollblPatientName.Text = this.GetPatientName();
            this.tooltxtRecordWeek.Text = this.GetWeek(null).ToString();

            this.RefreshReportView(false,this.GetSelectedWeek());
            this.reportDesigner1.Focus();
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Size size = new Size(this.toolStrip1.Width, SystemInformation.WorkingArea.Height);
            GlobalMethods.UI.LocateScreenCenter(this, size);
        }

        private void reportDesigner1_QueryContext(object sender, QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }

        private void reportDesigner1_ExecuteQuery(object sender, ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        /// <summary>
        /// 加载体温单报表模板
        /// </summary>
        private void LoadBodyTemperatureTemplet()
        {
            if (this.m_patVisitInfo == null)
            {
                return;
            }
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

        /// <summary>
        /// 获取指定的日期时间对应的周次
        /// </summary>
        /// <param name="dtWeekDate">日期时间</param>
        /// <returns>周次</returns>
        private long GetWeek(DateTime? dtWeekDate)
        {
            if (this.m_patVisitInfo == null)
                return 1;
            DateTime dtVisitTime = this.m_patVisitInfo.VisitTime;
            DateTime dtEndTime = DateTime.Now;
            if (dtWeekDate != null && dtWeekDate.HasValue)
                dtEndTime = dtWeekDate.Value;
            else
                dtEndTime = SysTimeService.Instance.Now;
            long days = GlobalMethods.SysTime.DateDiff(DateInterval.Day, dtVisitTime.Date, dtEndTime.Date);
            long weeks = (days / 7) + 1;
            return weeks;
        }

        /// <summary>
        /// 根据病人及周次刷新当前报表视图显示
        /// </summary>
        /// <param name="IsPatChanged">病人是否是否切换</param>
        /// <param name="week">周次</param>
        private void RefreshReportView(bool IsPatChanged,long week)
        {
            long end = this.GetWeek(this.GetEndWeekDate(true));
            if (end < 1)
                end = 1;
            if (week < 1)
            {
                week = 1;
            }
            if (week > end)
            {
                week = end;
            }
            if (IsPatChanged)
            {
                week = this.GetWeek(this.GetEndWeekDate(true));
            }
            this.m_week = week;
            this.tooltxtRecordWeek.Text = week.ToString();
            this.SetQcStatus(week, this.m_patVisitInfo);
            Application.DoEvents();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.reportDesigner1.UpdateReport("显示数据", week);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
        
        /// <summary>
        /// 根据病人信息和周次获取是否存在改审核信息
        /// </summary>
        /// <param name="week">周次</param>
        /// <param name="patVisitInfo">病人信息</param>
        private void SetQcStatus(long week,PatVisitInfo patVisitInfo)
        {
            QCExamineInfo qcExamineInfo = new QCExamineInfo();
            short shRet = QCExamineService.Instance.GetQcExamineInfo(week.ToString(), ServerData.ExamineType.TEMPERATURE, patVisitInfo.PatientId, patVisitInfo.VisitId, ref qcExamineInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                this.toollblStatus.Text = ServerData.ExamineStatus.QC_NONE;
                this.toolbtnQc.Enabled = true;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = true;
            }
            else
            {
                if (qcExamineInfo.QcExamineStatus == "1")
                {
                    this.toollblStatus.Text = ServerData.ExamineStatus.QC_OK;
                    this.toolbtnQc.Enabled = false;
                    this.toolbtnQcCancel.Enabled = true;
                    this.toolbtnQcQuestion.Enabled = false;
                }
                else
                {
                    this.toollblStatus.Text = ServerData.ExamineStatus.QC_MARK;
                    this.toolbtnQc.Enabled = true;
                    this.toolbtnQcCancel.Enabled = false;
                    this.toolbtnQcQuestion.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 获取当前周次文本框中的周次
        /// </summary>
        /// <returns>周次</returns>
        private long GetSelectedWeek()
        {
            return GlobalMethods.Convert.StringToValue(this.tooltxtRecordWeek.Text, 1);
        }

        private string GetPatientName()
        {
            return this.m_patVisitInfo.PatientName;
        }

        private string GetPatientId()
        {
            return this.m_patVisitInfo.PatientId;
        }

        public bool GetSystemContext(string name, ref object value)
        {
            #region"打印单上下文信息"
            if (name == "用户ID号" || name == "用户编号")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.ID;
                return true;
            }
            if (name == "用户姓名")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Name;
                return true;
            }
            if (name == "用户密码")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Password;
                return true;
            }
            if (name == "用户科室代码")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptCode;
                return true;
            }
            if (name == "用户科室名称")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptName;
                return true;
            }
            if (name == "用户病区代码")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardCode;
                return true;
            }
            if (name == "用户病区名称")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardName;
                return true;
            }
            if (name == "病人ID号" || name == "病人编号")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientId;
                return true;
            }
            if (name == "病人姓名")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientName;
                return true;
            }
            if (name == "病人性别")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientSex;
                return true;
            }
            if (name == "病人生日")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.BirthTime;
                return true;
            }
            if (name == "病人年龄")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = GlobalMethods.SysTime.GetAgeText(
                    m_patVisitInfo.BirthTime,
                    m_patVisitInfo.VisitTime);
                return true;
            }
            if (name == "病人病情")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientCondition;
                return true;
            }
            if (name == "护理等级")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.NursingClass;
                return true;
            }
            if (name == "入院次" || name == "就诊号")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.VisitId;
                return true;
            }
            if (name == "入院时间")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.VisitTime;
                return true;
            }
            if (name == "就诊科室代码")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.DeptCode;
                return true;
            }
            if (name == "就诊科室名称")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.DeptName;
                return true;
            }
            if (name == "就诊病区代码")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.WardCode;
                return true;
            }
            if (name == "就诊病区名称")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.WardName;
                return true;
            }
            if (name == "就诊类型")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.VisitType;
                return true;
            }
            if (name == "床号")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.BedCode;
                return true;
            }
            if (name == "住院号")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.InpNo;
                return true;
            }
            if (name == "诊断")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.Diagnosis;
                return true;
            }
            if (name == "过敏药物")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.AllergyDrugs;
                return true;
            }
            if (name == "当前时间")
            {
                value = SysTimeService.Instance.Now;
                return true;
            }
            return false;
            #endregion
        }

        private void toolbtnPrev_Click(object sender, EventArgs e)
        {
            if (this.m_index <= 0)
            {
                MessageBox.Show("已经是第一位病人");
                return;
            }

            this.reportDesigner1.Focus();
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(this.m_dtPatVisit.Rows[--this.m_index]);
            this.RefreshReportView(true,this.GetSelectedWeek() - 1);
        }

        private void toolbtnNext_Click(object sender, EventArgs e)
        {
            if (this.m_index >= this.m_dtPatVisit.Rows.Count)
            {
                MessageBox.Show("已经是最后一位病人");
                return;
            }
            this.reportDesigner1.Focus();
            this.m_patVisitInfo = this.GetPatVisitFromDataRow(this.m_dtPatVisit.Rows[++this.m_index]);
            this.RefreshReportView(true,this.GetSelectedWeek() + 1);
        }

        private void toolbtnQc_Click(object sender, EventArgs e)
        {
            if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_NONE)
            {
                QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, this.m_week.ToString(), ServerData.ExamineType.TEMPERATURE);
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核失败！");
                    return;
                }
            }
            else if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_week.ToString(), ServerData.ExamineType.TEMPERATURE, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
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
            this.RefreshReportView(false,this.m_week);
        }

        private void toolbtnQcCancel_Click(object sender, EventArgs e)
        {
            if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_OK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_week.ToString(), ServerData.ExamineType.TEMPERATURE, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
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
            this.RefreshReportView(false,this.m_week);
        }

        private void toolbtnQcQuestion_Click(object sender, EventArgs e)
        {
            if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_NONE)
            {
                ReasonDialog reasonDialog = new ReasonDialog("理由", string.Empty);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                QCExamineInfo qcExamineInfo = QcExamineHandler.Instance.Create(this.m_patVisitInfo, this.m_week.ToString(), ServerData.ExamineType.TEMPERATURE);
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("标记审核内容失败！");
                    return;
                }
            }
            else if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_week.ToString(), ServerData.ExamineType.TEMPERATURE, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
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

            this.RefreshReportView(false,this.m_week);
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
                this.RefreshReportView(false,this.GetSelectedWeek());
        }
    }
}