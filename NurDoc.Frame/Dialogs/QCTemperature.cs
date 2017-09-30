// ***********************************************************
// ������Ӳ���ϵͳ,����ָ�ش���.
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
        //��ǰҳ�没����Ϣ
        private PatVisitInfo m_patVisitInfo = null;

        //������ʾ���ܴΣ���ֹ��ʾ���ܴκ����µ���ʾ���ݲ�ͬ
        private long m_week = 0;

                /// <summary>
        /// ������Ϣ�� ����ǰ����ɸѡ
        /// </summary>
        private DataTable m_dtPatVisit = null;

        /// <summary>
        /// ���浱ǰ����
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
                MessageBoxEx.Show("������Ϣ��ѯʧ��");
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
        /// �������µ�����ģ��
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
                MessageBoxEx.ShowError("���µ�ģ�廹û������!");
                return;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("���µ�ģ����������ʧ��!");
                return;
            }
            this.reportDesigner1.OpenDocument(byteTempletData);
        }

        /// <summary>
        /// ��ȡ��ǰ����סԺ��ֹ�ܵ�����
        /// </summary>
        /// <param name="bStopToCurrentTime">�Ƿ��ֹ����ǰʱ��</param>
        /// <returns>��ֹ������</returns>
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
        /// ��ȡָ��������ʱ���Ӧ���ܴ�
        /// </summary>
        /// <param name="dtWeekDate">����ʱ��</param>
        /// <returns>�ܴ�</returns>
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
        /// ���ݲ��˼��ܴ�ˢ�µ�ǰ������ͼ��ʾ
        /// </summary>
        /// <param name="IsPatChanged">�����Ƿ��Ƿ��л�</param>
        /// <param name="week">�ܴ�</param>
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
            this.reportDesigner1.UpdateReport("��ʾ����", week);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
        
        /// <summary>
        /// ���ݲ�����Ϣ���ܴλ�ȡ�Ƿ���ڸ������Ϣ
        /// </summary>
        /// <param name="week">�ܴ�</param>
        /// <param name="patVisitInfo">������Ϣ</param>
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
        /// ��ȡ��ǰ�ܴ��ı����е��ܴ�
        /// </summary>
        /// <returns>�ܴ�</returns>
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
            #region"��ӡ����������Ϣ"
            if (name == "�û�ID��" || name == "�û����")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.ID;
                return true;
            }
            if (name == "�û�����")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Name;
                return true;
            }
            if (name == "�û�����")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Password;
                return true;
            }
            if (name == "�û����Ҵ���")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptCode;
                return true;
            }
            if (name == "�û���������")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptName;
                return true;
            }
            if (name == "�û���������")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardCode;
                return true;
            }
            if (name == "�û���������")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardName;
                return true;
            }
            if (name == "����ID��" || name == "���˱��")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientId;
                return true;
            }
            if (name == "��������")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientName;
                return true;
            }
            if (name == "�����Ա�")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientSex;
                return true;
            }
            if (name == "��������")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.BirthTime;
                return true;
            }
            if (name == "��������")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = GlobalMethods.SysTime.GetAgeText(
                    m_patVisitInfo.BirthTime,
                    m_patVisitInfo.VisitTime);
                return true;
            }
            if (name == "���˲���")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.PatientCondition;
                return true;
            }
            if (name == "����ȼ�")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.NursingClass;
                return true;
            }
            if (name == "��Ժ��" || name == "�����")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.VisitId;
                return true;
            }
            if (name == "��Ժʱ��")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.VisitTime;
                return true;
            }
            if (name == "������Ҵ���")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.DeptCode;
                return true;
            }
            if (name == "�����������")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.DeptName;
                return true;
            }
            if (name == "���ﲡ������")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.WardCode;
                return true;
            }
            if (name == "���ﲡ������")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.WardName;
                return true;
            }
            if (name == "��������")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.VisitType;
                return true;
            }
            if (name == "����")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.BedCode;
                return true;
            }
            if (name == "סԺ��")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.InpNo;
                return true;
            }
            if (name == "���")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.Diagnosis;
                return true;
            }
            if (name == "����ҩ��")
            {
                if (m_patVisitInfo == null)
                    return false;
                value = m_patVisitInfo.AllergyDrugs;
                return true;
            }
            if (name == "��ǰʱ��")
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
                MessageBox.Show("�Ѿ��ǵ�һλ����");
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
                MessageBox.Show("�Ѿ������һλ����");
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
                    MessageBoxEx.Show("���ʧ�ܣ�");
                    return;
                }
            }
            else if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_week.ToString(), ServerData.ExamineType.TEMPERATURE, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("������ݲ�ѯʧ�ܣ�");
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
                    MessageBoxEx.Show("���ʧ�ܣ�");
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
                    MessageBoxEx.Show("������ݲ�ѯʧ�ܣ�");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("����", qcExamineInfo.QcExamineContent);
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
                    MessageBoxEx.Show("ȡ�����ʧ�ܣ�");
                    return;
                }
            }
            this.RefreshReportView(false,this.m_week);
        }

        private void toolbtnQcQuestion_Click(object sender, EventArgs e)
        {
            if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_NONE)
            {
                ReasonDialog reasonDialog = new ReasonDialog("����", string.Empty);
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
                    MessageBoxEx.Show("����������ʧ�ܣ�");
                    return;
                }
            }
            else if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_week.ToString(), ServerData.ExamineType.TEMPERATURE, this.m_patVisitInfo.PatientId, this.m_patVisitInfo.VisitId, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("������ݲ�ѯʧ�ܣ�");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("����", qcExamineInfo.QcExamineContent);
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
                    MessageBoxEx.Show("����������ʧ�ܣ�");
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