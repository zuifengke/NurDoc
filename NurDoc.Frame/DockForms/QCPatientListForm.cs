using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.Common.Libraries;
using MedQCSys.Controls.PatInfoList;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class QCPatientListForm : DockContentBase
    {
        public QCPatientListForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.PersonsIcon;
            this.AutoHidePortion = this.Width;
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.DockLeft;
            this.DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                | DockAreas.DockTop | DockAreas.DockBottom;
            InitNurseDept();
            this.patInfoList1.CardSelectedChanged += new EventHandler(patInfoList1_CardSelectedChanged);
            cbbPatientStatus.SelectedIndex = 1;//Ĭ��ѡ����Ժ����
        }

        void SelectedCard_DoubleClick(object sender, EventArgs e)
        {
            PatInfoCard patInfoCard = (PatInfoCard)sender;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.MainFrame.ShowPatientPageForm(patInfoCard.PatVisitInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        void patInfoList1_CardSelectedChanged(object sender, EventArgs e)
        {
            if (this.patInfoList1.SelectedCard == null)
                return;
            this.patInfoList1.SelectedCard.DoubleClick -= new EventHandler(SelectedCard_DoubleClick);
            this.patInfoList1.SelectedCard.DoubleClick += new EventHandler(SelectedCard_DoubleClick);
            PatientTable.Instance.ActivePatient = this.patInfoList1.SelectedCard.PatVisitInfo;
            IDockContent content = this.MainFrame.GetContent(typeof(QuestionListForm));
            if (content != null)
                ((QuestionListForm)content).RefreshView();
        }
        /// <summary>
        /// ��ʼ������Ԫ����������
        /// </summary>
        private void InitNurseDept()
        {
            List<DeptInfo> lstDeptInfos = null;
            short shRet = CommonService.Instance.GetNurseDeptList(ref lstDeptInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("�ٴ������б�����ʧ��!");
                return;
            }
            if (lstDeptInfos == null || lstDeptInfos.Count <= 0)
                return;
            for (int index = 0; index < lstDeptInfos.Count; index++)
            {
                DeptInfo deptInfo = lstDeptInfos[index];
                if (deptInfo == null || string.IsNullOrEmpty(deptInfo.DeptCode))
                    continue;
                this.cbbNurseDept.Items.Add(deptInfo);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }
        #region ���Ʋ�ѯ����Enabled
        private void ckbPatientStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPatientStatus.Checked)
                cbbPatientStatus.Enabled = true;
            else
                cbbPatientStatus.Enabled = false;
        }

        private void ckbNurseDept_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbNurseDept.Checked)
                cbbNurseDept.Enabled = true;
            else
                cbbNurseDept.Enabled = false;
        }

        private void ckbPatientName_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPatientName.Checked)
                txtPatientName.Enabled = true;
            else
                txtPatientName.Enabled = false;
        }

        private void ckbPatientID_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPatientID.Checked)
                txtPatientID.Enabled = true;
            else
                txtPatientID.Enabled = false;
        }

        private void ckbNursingClass_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbNursingClass.Checked)
                cbbNursingClass.Enabled = true;
            else
                cbbNursingClass.Enabled = false;
        }

        private void ckbPatCondition_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPatCondition.Checked)
                cbbPatientCondition.Enabled = true;
            else
                cbbPatientCondition.Enabled = false;
        }

        private void ckbStartTime_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbStartTime.Checked)
                dtPStart.Enabled = true;
            else
                dtPStart.Enabled = false;
        }

        private void ckbEndTime_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEndTime.Checked)
                dtPEnd.Enabled = true;
            else
                dtPEnd.Enabled = false;
        }
        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!IsSearching)
                SearchAndBindPatInfo();
        }
        private bool IsSearching = false;
        private void SearchAndBindPatInfo()
        {
            this.patInfoList1.ClearPatInfo();
            string szPatStatus = string.Empty;
            string szNurseDeptCode = string.Empty;
            string szPatName = string.Empty;
            string szPatID = string.Empty;
            string szNurseClass = string.Empty;
            string szPatCodition = string.Empty;
            DateTime? dtStart = null;
            DateTime? dtEnd = null;

            #region ����״̬
            if (ckbPatientStatus.Checked)
            {
                if (cbbPatientStatus.SelectedItem == null)
                {
                    MessageBoxEx.Show("��ѡ����״̬����ȡ����ѡ��");
                    return;
                }
                if (cbbPatientStatus.SelectedIndex == 0)
                {
                    szPatStatus = "IN_AND_OUT";
                }
                else if (cbbPatientStatus.SelectedIndex == 1)
                {
                    szPatStatus = "IN";
                }
                else if (cbbPatientStatus.SelectedIndex == 2)
                {
                    szPatStatus = "OUT";
                }
                //��ѡ�����в��˺ͳ�Ժ����ʱ�����������ֹʱ��
                if (cbbPatientStatus.SelectedIndex == 0 || cbbPatientStatus.SelectedIndex == 2)
                {
                    if (!ckbStartTime.Checked || !ckbEndTime.Checked)
                    {
                        MessageBoxEx.Show("ѡ���Ժ���˻������в���ʱ�������ֹʱ�䣡");
                        return;
                    }
                }

            }
            #endregion
            #region ����Ԫ
            if (ckbNurseDept.Checked)
            {
                if (cbbNurseDept.SelectedItem == null)
                {
                    MessageBoxEx.Show("��ѡ����Ԫ����ȡ����ѡ��");
                    return;
                }
                DeptInfo dept = cbbNurseDept.SelectedItem as DeptInfo;
                if (dept == null || string.IsNullOrEmpty(dept.DeptCode))
                {
                    MessageBoxEx.Show("����Ԫ��Ϣ����ȷ��");
                    return;
                }
                szNurseDeptCode = dept.DeptCode;
            }
            #endregion
            #region  ��������
            if (ckbPatientName.Checked)
            {
                if (string.IsNullOrEmpty(txtPatientName.Text.Trim()))
                {
                    MessageBoxEx.Show("�����뻼����������ȡ����ѡ��");
                    return;
                }
                szPatName = txtPatientName.Text.Trim();
            }
            #endregion
            #region  ����ID
            if (ckbPatientID.Checked)
            {
                if (string.IsNullOrEmpty(txtPatientID.Text.Trim()))
                {
                    MessageBoxEx.Show("�����벡��ID����ȡ����ѡ��");
                    return;
                }
                szPatID = txtPatientID.Text.Trim();
            }
            #endregion
            #region ����ȼ�
            if (ckbNursingClass.Checked)
            {
                if (cbbNursingClass.SelectedItem == null)
                {
                    MessageBoxEx.Show("��ѡ����ȼ�����ȡ����ѡ��");
                    return;
                }
                if (cbbNursingClass.SelectedIndex == 0)
                {
                    szNurseClass = "�ؼ�";
                }
                else if (cbbNursingClass.SelectedIndex == 1)
                {
                    szNurseClass = "һ��";
                }
                else if (cbbNursingClass.SelectedIndex == 2)
                {
                    szNurseClass = "����";
                }
                else if (cbbNursingClass.SelectedIndex == 3)
                {
                    szNurseClass = "����";
                }
            }
            #endregion
            #region ����
            if (ckbPatCondition.Checked)
            {
                if (cbbPatientCondition.SelectedItem == null)
                {
                    MessageBoxEx.Show("��ѡ�������ȡ����ѡ��");
                    return;
                }
                if (cbbPatientCondition.SelectedIndex == 0)
                {
                    szPatCodition = "��Σ";
                }
                else if (cbbPatientCondition.SelectedIndex == 1)
                {
                    szPatCodition = "����";
                }
                else if (cbbPatientCondition.SelectedIndex == 2)
                {
                    szPatCodition = "һ��";
                }
            }
            #endregion
            #region ��ֹʱ��
            if (ckbStartTime.Checked)
            {
                dtStart = Convert.ToDateTime(dtPStart.Value.ToString("yyyy/MM/dd 00:00:00"));
            }
            if (ckbEndTime.Checked)
            {
                dtEnd = Convert.ToDateTime(dtPEnd.Value.ToString("yyyy/MM/dd 23:59:59"));
            }
            if (dtStart != null && dtEnd != null)
            {
                if (dtEnd < dtStart)
                {
                    MessageBoxEx.Show("��ʼʱ���벻Ҫ����ʱ�䣡");
                    return;
                }
                TimeSpan ts = (TimeSpan)(dtEnd - dtStart);
                if (ts.Days > 366)
                {
                    MessageBoxEx.Show("��ֹʱ���벻Ҫ����һ�꣡");
                    return;
                }
            }
            #endregion

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            List<PatVisitInfo> listPatVisitInfo = null;
            IsSearching = true;
            short shRet = PatVisitService.Instance.GetPatVisitInfo(szPatStatus, szNurseDeptCode, szPatName,
                szPatID, szNurseClass, szPatCodition, dtStart, dtEnd, ref listPatVisitInfo);
        
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ������Ϣʧ�ܣ�");
                IsSearching = false;
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (listPatVisitInfo == null || listPatVisitInfo.Count == 0)
            {
                MessageBoxEx.Show("û�в�ѯ��������Ϣ��");
                IsSearching = false;
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.patInfoList1.SuspendLayout();
            for (int index = listPatVisitInfo.Count - 1; index >= 0; index--)
            {
                this.patInfoList1.AddPatInfo(listPatVisitInfo[index]);
                //��ӻ���ԪLable
                AddNurseDept(listPatVisitInfo, index);
            }
            this.patInfoList1.ResumeLayout();
            this.patInfoList1.Update();
            this.patInfoList1.SelectedCard = null;
            IsSearching = false;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void AddNurseDept(List<PatVisitInfo> listPatVisitInfo, int index)
        {
            //listPatVisitInfo�����谴�ջ���Ԫ��������
            if (listPatVisitInfo == null || listPatVisitInfo.Count == 0)
                return;
            if (index < 0 || index > listPatVisitInfo.Count - 1)
                return;
            PatVisitInfo prevPatVisitInfo = null;
            if (index > 0)
                prevPatVisitInfo = listPatVisitInfo[index - 1];
            PatVisitInfo currPatVisitInfo = listPatVisitInfo[index];

            if (index == 0 || currPatVisitInfo.WardCode != prevPatVisitInfo.WardCode)
            {
                Label lblDeptName = new Label();
                lblDeptName.BackColor = Color.LightSteelBlue;
                lblDeptName.ForeColor = Color.Black;
                lblDeptName.Height = 24;
                lblDeptName.Width = this.patInfoList1.Width;
                lblDeptName.Dock = DockStyle.Top;
                lblDeptName.Text = currPatVisitInfo.WardName;
                lblDeptName.TextAlign = ContentAlignment.MiddleLeft;
                lblDeptName.Parent = this.patInfoList1;
                lblDeptName.Font = new Font("����", 10.5f, FontStyle.Regular);
            }
        }
    }
}