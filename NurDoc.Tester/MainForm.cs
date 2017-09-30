// ***********************************************************
// 护理电子病历系统启动和调试程序.用来模拟医护工作站调用本系统
// Creator:YangMingkun  Date:2012-8-20
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Data.DataServices;

namespace Heren.NurDoc.Tester
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.SysIcon;
        }

        private void txtUserID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.txtUserName.Text = string.Empty;
            this.txtUserPwd.Text = string.Empty;
            this.txtDeptName.Text = string.Empty;
            this.txtDeptCode.Text = string.Empty;
            this.txtWardName.Text = string.Empty;
            this.txtWardCode.Text = string.Empty;
            //
            string szUserID = this.txtUserID.Text.Trim().ToUpper();
            UserInfo userInfo = null;
            AccountService.Instance.GetUserInfo(szUserID, ref userInfo);
            if (userInfo == null)
            {
                MessageBoxEx.Show("您输入的用户账号不存在!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.txtUserID.Text = userInfo.ID;
            this.txtUserName.Text = userInfo.Name;
            this.txtUserPwd.Text = userInfo.Password;
            this.txtDeptCode.Text = userInfo.DeptCode;
            this.txtDeptName.Text = userInfo.DeptName;
            this.txtWardCode.Text = userInfo.WardCode;
            this.txtWardName.Text = userInfo.WardName;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnStartNurDocSys_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.StartNurDocSys();
            this.Cursor = Cursors.Default;
        }

        private void btnExitNurDocSys_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (this.ExitNurDocSys())
                this.Close();
            this.Cursor = Cursors.Default;
        }

        private bool ExitNurDocSys()
        {
            string mappingName = SystemConst.MappingName.NURDOC_SYS;
            IntPtr hMainFormHandle = GlobalMethods.Win32.GetSystemHandle(mappingName);
            if (!NativeMethods.User32.IsWindow(hMainFormHandle))
                return true;
            NativeMethods.User32.SetForegroundWindow(hMainFormHandle);
            NativeMethods.User32.SetActiveWindow(hMainFormHandle);
            NativeMethods.User32.SendMessage(hMainFormHandle, 0x10, IntPtr.Zero, IntPtr.Zero);
            return !NativeMethods.User32.IsWindow(hMainFormHandle);
        }

        private void StartNurDocSys()
        {
            StringBuilder sbArgs = new StringBuilder();
            sbArgs.Append(this.Handle.ToString());
            sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtUserID.Text))
                sbArgs.Append(this.txtUserID.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtUserName.Text))
                sbArgs.Append(this.txtUserName.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtUserPwd.Text))
                sbArgs.Append(this.txtUserPwd.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtDeptCode.Text))
                sbArgs.Append(this.txtDeptCode.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtDeptName.Text))
                sbArgs.Append(this.txtDeptName.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtWardCode.Text))
                sbArgs.Append(this.txtWardCode.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtWardName.Text))
                sbArgs.Append(this.txtWardName.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtPatientID.Text))
            {
                sbArgs.Append(this.txtPatientID.Text.Trim());
                sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);
            }

            if (!GlobalMethods.Misc.IsEmptyString(this.txtVisitID.Text))
            {
                sbArgs.Append(this.txtVisitID.Text.Trim());
                sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);
            }

            if (this.checkBox1.Checked)
            {
                sbArgs.Append("1");
                sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);
            }
            else
            {
                sbArgs.Append("0");
                sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);
            }

            if (!GlobalMethods.Misc.IsEmptyString(this.txbDocType.Text))
            {
                sbArgs.Append(this.txbDocType.Text.Trim());
                sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);
            }
            if (!GlobalMethods.Misc.IsEmptyString(this.txbDocType.Text))
            {
                sbArgs.Append(this.txbDocID.Text.Trim());
                sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);
            }

            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.Arguments = sbArgs.ToString().Replace(SystemConst.StartupArgs.ESCAPED_CHAR, SystemConst.StartupArgs.ESCAPE_CHAR);
            proc.StartInfo.FileName = string.Format(@"{0}\NurDoc.exe", Application.StartupPath);
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(string.Format("无法启动{0}!", SystemContext.Instance.SystemName));
                LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, string.Format("{0}启动失败!", SystemContext.Instance.SystemName), ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //    DataTable dt =new DataTable();
            //    dt.Columns.Add("patient_id");
            //    dt.Columns.Add("visit_id");
            //    dt.Columns.Add("BeginTime");
            //    dt.Columns.Add("EndTime");
            //    dt.Rows.Add("2012050999","1","2013/08/14 08:00:00","2013/08/14 08:00:00");
            //    DataTable result=new DataTable();
            //    FormDataService.Instance.GetWardPatientsNursingAssess(dt, ref result);
            List<NurCarePlanDictInfo> lstNurCarePlanDictInfos = new List<NurCarePlanDictInfo>();
            List<string> lstDiagCode = new List<string>();
            lstDiagCode.Add("2_DGHTCS");
            lstDiagCode.Add("ND00002");
            SystemContext.Instance.CarePlanAccess.GetNurCarePlanDictInfo(lstDiagCode, ref lstNurCarePlanDictInfos);
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("patient_id");
            dt.Columns.Add("visit_id");
            dt.Columns.Add("BeginTime");
            dt.Columns.Add("EndTime");
            dt.Columns.Add("PreBeginTime");
            dt.Columns.Add("PreEndTime");
            dt.Rows.Add("F449310", "1", "2013/09/24 08:00:00", "2013/09/24 16:00:00", "2013/09/24 00:00:00", "2013/09/24 08:00:00");
            DataTable result = new DataTable();
            DataTable dtAssessTaskInfo = new DataTable();
            PatientTaskService.Instance.GetPatientAssessTaskList(dt, ref dtAssessTaskInfo);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable result = new DataTable();
            dt.Columns.Add("patient_id");
            dt.Columns.Add("visit_id");
            dt.Rows.Add("F449310", "1");
            WardPatientsService.Instance.GetWardPatientsNursingAssess(dt, ref result);
        }

        /// <summary>
        /// 启动护理信息库
        /// </summary>
        private void StartInfoLib()
        {
            StringBuilder sbArgs = new StringBuilder();
            sbArgs.Append(this.Handle.ToString());
            sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtUserID.Text))
                sbArgs.Append(this.txtUserID.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtUserName.Text))
                sbArgs.Append(this.txtUserName.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtUserPwd.Text))
                sbArgs.Append(this.txtUserPwd.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtDeptCode.Text))
                sbArgs.Append(this.txtDeptCode.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtDeptName.Text))
                sbArgs.Append(this.txtDeptName.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtWardCode.Text))
                sbArgs.Append(this.txtWardCode.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtWardName.Text))
                sbArgs.Append(this.txtWardName.Text.Trim());
            sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);

            if (!GlobalMethods.Misc.IsEmptyString(this.txtPatientID.Text))
            {
                sbArgs.Append(this.txtPatientID.Text.Trim());
                sbArgs.Append(SystemConst.StartupArgs.FIELD_SPLIT);
            }

            if (!GlobalMethods.Misc.IsEmptyString(this.txtVisitID.Text))
            {
                sbArgs.Append(this.txtVisitID.Text.Trim());
                sbArgs.Append(SystemConst.StartupArgs.GROUP_SPLIT);
            }

            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.Arguments = sbArgs.ToString().Replace(SystemConst.StartupArgs.ESCAPED_CHAR, SystemConst.StartupArgs.ESCAPE_CHAR);
            proc.StartInfo.FileName = string.Format(@"{0}\InfoLib.exe", Application.StartupPath);
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(string.Format("无法启动{0}!", SystemContext.Instance.SystemName));
                LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, string.Format("启动失败!", SystemContext.Instance.SystemName), ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.StartInfoLib();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SinglePatForm single = new SinglePatForm();
            single.PatientID = this.txtPatientID.Text;
            single.VisitID = this.txtVisitID.Text;
            single.UserID = this.txtUserID.Text;
            single.LocateToDoc = this.chkLocate.Checked;
            single.DocType = this.txbDocType.Text;
            single.DocID = this.txbDocID.Text;
            single.ShowDialog();
        }
    }
}