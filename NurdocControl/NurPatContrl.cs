using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Heren.NurDoc.DAL;
using Heren.NurDoc.PatPage;
using Heren.Common.Libraries;
using Heren.NurDoc.Data;

namespace NurdocControl
{
    [Guid("A6C6DB5B-15B8-3D1E-9E8E-43F17A577B8F")]
    [ComVisible(true)]
    [ToolboxItem(false)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(IAxNurPatControl))]
    public partial class NurPatContrl : UserControl, IObjectSafety, IAxNurPatControl
    {
        public void GetInterfacceSafyOptions(Int32 riid, out Int32 pdwSupportedOptions, out Int32 pdwEnabledOptions)
        {

            // TODO: 添加 WebCamControl.GetInterfacceSafyOptions 实现 

            pdwSupportedOptions = 1;

            pdwEnabledOptions = 2;

        }

        public void SetInterfaceSafetyOptions(Int32 riid, Int32 dwOptionsSetMask, Int32 dwEnabledOptions)
        {

            // TODO: 添加 WebCamControl.SetInterfaceSafetyOptions 实现             

        }

        //#region IObjectSafety Members
        //void IObjectSafety.SetInterfaceSafetyOptions(int riid()
        //    , int dwOptionSetMask, int dwEnabledOptions)
        //{
        //}

        //void IObjectSafety.GetInterfaceSafetyOptions(int riid
        //    , out int pdwSupportedOptions, out int pdwEnabledOptions)
        //{
        //    pdwSupportedOptions = 1;
        //    pdwEnabledOptions = 2;
        //}
        //#endregion

        public NurPatContrl()
        {
            InitializeComponent();
            SystemConfig.Instance.ConfigFile = GlobalMethods.Misc.GetWorkingPath().ToString() + "\\UserData\\NurDocSys.xml";
            //MessageBox.Show(SystemConfig.Instance.ConfigFile);
        }

        public void test()
        {
            MessageBox.Show("测试成功（无参数）");
        }

        private void patientPageControl1_Load(object sender, EventArgs e)
        {
            //this.patientPageControl1.PatVisitInfo = new PatVisitInfo();
            //this.patientPageControl1.SetPatientInfoStripVisible(false);
            //this.SwitchPatient("P099953","4");
        }

        private PatVisitInfo m_patientVisit = null;

        /// <summary>
        /// 获取或设置当前窗口的病人就诊信息
        /// </summary>
        [Browsable(false)]
        public PatVisitInfo PatVisitInfo
        {
            get { return this.m_patientVisit; }
        }

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get
            {
                if (this.patientPageControl1 == null)
                    return false;
                if (this.patientPageControl1.IsDisposed)
                    return false;
                return this.patientPageControl1.IsModified;
            }
        }

        public void RefreshView()
        {
            //base.RefreshView();
            this.SwitchPatient(this.m_patientVisit);
        }

        public bool CheckModifyState()
        {
            if (this.patientPageControl1 == null
                || this.patientPageControl1.IsDisposed)
            {
                return false;
            }
            return !this.patientPageControl1.SwitchPatient(null);
        }

        /// <summary>
        /// 切换当前窗口中显示的病人及就诊信息
        /// </summary>
        /// <param name="patVisit">病人就诊信息</param>
        /// <returns>是否成功</returns>
        public void SwitchPatient(string szPatientID, string szVisitID, string szUserID)
        {
            try
            {
                LogManager.Instance.TextLogOnly = true;
                LogManager.Instance.WriteLog("接收到PID：" + szPatientID + ";VID:" + szVisitID + ";UserID:" + szUserID);
                if (string.IsNullOrEmpty(szPatientID) || string.IsNullOrEmpty(szVisitID) || string.IsNullOrEmpty(szUserID))
                {
                    MessageBoxEx.Show("接收到的参数异常:PID：" + szPatientID + ";VID:" + szVisitID + ";UserID:" + szUserID);
                    return;
                }
                int nRet = ExternService.HerenCert.VerifyUser(
                    "和仁护理电子病历软件",
                    SystemContext.Instance.SystemOption.HospitalName,
                    SystemContext.Instance.SystemOption.CertCode);
                if (nRet != 0)
                {
                    MessageBoxEx.Show(string.Format("当前{0}未经授权许可!", SystemContext.Instance.SystemName));
                    this.Dispose();
                    return;
                }

                short shRet = SystemConst.ReturnValue.OK;
                //获取用户info
                //MessageBox.Show("获取用户info");
                UserInfo UserInfo = null;
                shRet = AccountService.Instance.GetUserInfo(szUserID, ref UserInfo);
                if (shRet == SystemConst.ReturnValue.FAILED)
                {
                    MessageBoxEx.ShowError(string.Format("您没有权限登录{0}!", SystemContext.Instance.SystemName));
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                if (UserInfo == null)
                {
                    MessageBoxEx.ShowError(string.Format("抱歉没有查到{0}用户记录.", szUserID));
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                SystemContext.Instance.LoginUser = UserInfo.Clone() as UserInfo;

                //获取用户权限
                //MessageBox.Show("获取用户权限");
                UserRightBase userRight = null;
                AccountService.Instance.GetUserRight(SystemContext.Instance.LoginUser.ID, UserRightType.NurDoc, ref userRight);
                NurUserRight nurUserRight = userRight as NurUserRight;
                if (nurUserRight == null || !nurUserRight.NurDocSystem.Value)
                {
                    MessageBoxEx.ShowError(string.Format("您没有权限登录{0}!", SystemContext.Instance.SystemName));
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }

                //MessageBox.Show(szPatientID + szVisitID);
                PatVisitInfo PatVisitInfo = new PatVisitInfo();
                //MessageBox.Show("开始获取病人info");
                shRet = PatVisitService.Instance.GetPatVisitInfo(szPatientID, szVisitID, ref PatVisitInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBox.Show("获取患者信息过程中出现异常！");
                    return;
                }
                this.m_patientVisit = null;
                this.Update();
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                //MessageBox.Show(m_patientVisit.PatientID);
                PatientTable.Instance.ActivePatient = PatVisitInfo;
                bool success = this.patientPageControl1.SwitchPatient(PatVisitInfo);
                if (success)
                {
                    this.m_patientVisit = PatVisitInfo;
                    //this.RefreshWindowCaption();
                }

                this.patientPageControl1.SetPatientInfoStripVisible(false);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                this.Dispose();
            }
        }

        /// <summary>
        /// 切换当前窗口中显示的病人及就诊信息
        /// </summary>
        /// <param name="patVisit">病人就诊信息</param>
        /// <returns>是否成功</returns>
        public bool SwitchPatient(PatVisitInfo PatVisitInfo)
        {
            //MessageBox.Show("2");
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //MessageBox.Show("1");
            //MessageBox.Show(PatVisitInfo.PatientID);
            bool success = this.patientPageControl1.SwitchPatient(PatVisitInfo);
            if (success)
            {
                this.m_patientVisit = PatVisitInfo;
                //this.RefreshWindowCaption();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        ///// <summary>
        ///// 根据当前病人就诊信息刷新病人窗口标题
        ///// </summary>
        //private void RefreshWindowCaption()
        //{
        //    if (this.m_patientVisit == null
        //        || this.m_patientVisit.VisitTime == this.m_patientVisit.DefaultTime)
        //    {
        //        this.Text = "空床无病人";
        //        return;
        //    }
        //    this.Text = string.Format("{0}({1})"
        //        , this.m_patientVisit.PatientName, this.m_patientVisit.PatientID);
        //    this.TabSubhead = string.Format("入院时间:{0}"
        //        , this.m_patientVisit.VisitTime.ToString("yyyy年M月d日"));
        //}

        /// <summary>
        /// 切换患者并定位到文档
        /// </summary>
        /// <param name="szPatientID">患者ID</param>
        /// <param name="szVisitID">患者就诊次</param>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="szDocID">文档ID</param>
        public void OpenNurDoc(string szPatientID, string szVisitID, string szUserID, string szDocTypeID, string szDocID)
        {
            SwitchPatient(szPatientID, szVisitID, szUserID);
            if (string.IsNullOrEmpty(szDocTypeID))
            {
                MessageBoxEx.Show("DocTypeID参数不可为空!");
                return;
            }
            LocateToModule(szDocTypeID, szDocID);
            return;
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            bool success = this.patientPageControl1.LocateToModule(szDocTypeID, szDocID);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <param name="bshowList">是否隐藏左侧列表</param>
        /// <returns>是否成功</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID, bool bshowList)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            bool success = this.patientPageControl1.LocateToModule(szDocTypeID, szDocID, bshowList);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        private void PatientPageControl_PatientInfoChanged(object sender, EventArgs e)
        {
            this.m_patientVisit = this.patientPageControl1.PatVisitInfo;
            //this.RefreshWindowCaption();
        }

        private void PatientPageControl_PatientInfoChanging(object sender, PatientInfoChangingEventArgs e)
        {
            //PatientPageForm patientPageForm = this.MainFrame.GetPatientPageForm(e.NewPatVisit);
            //if (patientPageForm != null && patientPageForm != this)
            //{
            //    e.Cancel = true;
            //    patientPageForm.DockHandler.Activate();
            //}
        }

    }
}