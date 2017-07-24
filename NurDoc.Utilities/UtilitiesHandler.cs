// **************************************************************
// 护理电子病历系统公用模块之公用模块调用管理器
// Creator:YangMingkun  Date:2012-9-5
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Utilities.Templet;
using Heren.NurDoc.Utilities.Import;
using Heren.NurDoc.Utilities.Dialogs;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities
{
    public class UtilitiesHandler
    {
        private static UtilitiesHandler m_instance = null;

        /// <summary>
        /// 获取公用模块管理器实例
        /// </summary>
        public static UtilitiesHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new UtilitiesHandler();
                return m_instance;
            }
        }

        private UtilitiesHandler()
        {
        }

        private TextTempletForm m_TextTempletForm = null;
        private WordTempForm m_WordTempForm = null;
        private OrdersImportForm m_OrdersImportForm = null;
        private TestImportForm m_TestImportForm = null;
        private ExamImportForm m_ExamImportForm = null;
        private NurRecImportForm m_NurRecImportForm = null;
        private FoodEleImportForm m_FoodEleImportForm = null;
        private UserSelectDialog m_UserSelectDialog = null;
        private DeptSelectDialog m_DeptSelectDialog = null;
        private AntenatalInfoForm m_AntenatalInfoForm = null;
        private ReasonDialog m_ReasonDialog = null;

        public string ShowTextTempletForm()
        {
            if (this.m_TextTempletForm == null || this.m_TextTempletForm.IsDisposed)
                this.m_TextTempletForm = new TextTempletForm();
            if (this.m_TextTempletForm.ShowDialog() == DialogResult.OK)
                return this.m_TextTempletForm.SelectedContent;
            return string.Empty;
        }

        public string ShowWordTempForm()
        {
            if (this.m_WordTempForm == null || this.m_WordTempForm.IsDisposed)
                this.m_WordTempForm = new WordTempForm();
            if (this.m_WordTempForm.ShowDialog() == DialogResult.OK)
                return this.m_WordTempForm.SelectedContent;
            return string.Empty;
        }

        public DataTable ShowOrdersImportForm()
        {
            if (this.m_OrdersImportForm == null || this.m_OrdersImportForm.IsDisposed)
                this.m_OrdersImportForm = new OrdersImportForm();
            this.m_OrdersImportForm.PatientID = null;
            this.m_OrdersImportForm.VisitID = null;
            if (this.m_OrdersImportForm.ShowDialog() == DialogResult.OK)
                return this.m_OrdersImportForm.SelectedOrderTable;
            return null;
        }

        public DataTable ShowOrdersImportForm(string szSelectedItem)
        {
            if (this.m_OrdersImportForm == null || this.m_OrdersImportForm.IsDisposed)
                this.m_OrdersImportForm = new OrdersImportForm();
            this.m_OrdersImportForm.PatientID = null;
            this.m_OrdersImportForm.VisitID = null;
            this.m_OrdersImportForm.SelectedDefaultItem = szSelectedItem;
            if (this.m_OrdersImportForm.ShowDialog() == DialogResult.OK)
                return this.m_OrdersImportForm.SelectedOrderTable;
            return null;
        }

        /// <summary>
        /// 导入到当前交接班记录
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool ImportCurrentShift(string szShiftContent)
        {
            if (string.IsNullOrEmpty(szShiftContent))
                return false;
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_WARD_STATUS;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("病区动态编辑表单还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("病区动态编辑表单下载失败!");
                return false;
            }

            ShiftRankInfo[] wardShiftRankList = SystemContext.Instance.GetWardShiftRankList();
            if (wardShiftRankList.Length <= 0)
                return false;
            DateTime dtShiftTime = SysTimeService.Instance.Now;
            ShiftRankInfo CurrentShift = new ShiftRankInfo();
            foreach (object item in wardShiftRankList)
            {
                ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                shiftRankInfo.UpdateShiftRankTime(dtShiftTime.Date);
                if (dtShiftTime >= shiftRankInfo.StartTime && dtShiftTime < shiftRankInfo.EndTime)
                    CurrentShift = shiftRankInfo;
            }

            Heren.NurDoc.Utilities.Forms.FormControl formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            formControl1.Load(docTypeInfo, byteFormData);
            formControl1.UpdateFormData("刷新当前班次视图", CurrentShift.ToDataTable());
            DataTable table = formControl1.GetFormData("病区动态") as DataTable;
            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            foreach (DataRow row in table.Rows)
            {
                ShiftWardStatus shiftWardStatus = new ShiftWardStatus();
                if (shiftWardStatus.FromDataRow(row))
                    lstShiftWardStatus.Add(shiftWardStatus);
            }

            short shRet = SystemConst.ReturnValue.OK;

            if (SystemContext.Instance.LoginUser == null)
                return false;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = DateTime.Today;
            string szRankCode = CurrentShift.RankCode;
            NursingShiftInfo nursingShiftInfo = null;
            shRet = NurShiftService.Instance.GetNursingShiftInfo(szWardCode, dtShiftDate
                , szRankCode, ref nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK || nursingShiftInfo == null)
            {
                nursingShiftInfo = CreateNursingShiftInfo(CurrentShift);
            }

            ShiftPatient shiftPatient = new ShiftPatient();
            if (PatientTable.Instance.ActivePatient != null)
            {
                shiftPatient.PatientID = PatientTable.Instance.ActivePatient.PatientID;
                shiftPatient.VisitID = PatientTable.Instance.ActivePatient.VisitID;
                shiftPatient.SubID = PatientTable.Instance.ActivePatient.SubID;
                shiftPatient.PatientName = PatientTable.Instance.ActivePatient.PatientName;
                shiftPatient.PatientAge = GlobalMethods.SysTime.GetAgeText(PatientTable.Instance.ActivePatient.BirthTime);
                shiftPatient.WardCode = PatientTable.Instance.ActivePatient.WardCode;
                shiftPatient.WardName = PatientTable.Instance.ActivePatient.WardName;
                shiftPatient.BedCode = PatientTable.Instance.ActivePatient.BedCode;
                shiftPatient.Diagnosis = PatientTable.Instance.ActivePatient.Diagnosis;
                shiftPatient.ShiftItemName = string.Empty;
                shiftPatient.ShiftContent = szShiftContent;
            }
            else
            { return false; }

            //保存交班病人列表
            if (shiftPatient == null)
                return false;
            shiftPatient.PatientNo = 0;
            shiftPatient.ShiftRecordID = nursingShiftInfo.ShiftRecordID;
            shRet = NurShiftService.Instance.SaveShiftPatient(shiftPatient);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("保存交班病人信息失败!");
                return false;
            }

            //保存交班病区动态
            foreach (ShiftWardStatus shiftWardStatus in lstShiftWardStatus)
            {
                shiftWardStatus.ShiftRecordID = nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftWardStatus(shiftWardStatus);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班病区动态失败!");
                    return false;
                }
            }

            ////保存交班主索引记录
            shRet = NurShiftService.Instance.SaveNursingShiftInfo(nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("保存交班主记录信息失败!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建一条新的护理交班主记录
        /// </summary>
        /// <returns>护理交班主记录</returns>
        private NursingShiftInfo CreateNursingShiftInfo(ShiftRankInfo shiftRankInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            DateTime dtShiftDate = DateTime.Today;
            if (shiftRankInfo == null)
                return null;

            DateTime dtNow = SysTimeService.Instance.Now;

            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo.ShiftRecordDate = dtShiftDate.Date;
            nursingShiftInfo.ShiftRecordTime = new DateTime(dtShiftDate.Year
                , dtShiftDate.Month, dtShiftDate.Day, dtNow.Hour, dtNow.Minute, dtNow.Second);
            nursingShiftInfo.ShiftRankCode = shiftRankInfo.RankCode;
            nursingShiftInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            nursingShiftInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            nursingShiftInfo.CreateTime = SysTimeService.Instance.Now;
            nursingShiftInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            nursingShiftInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            nursingShiftInfo.ModifyTime = SysTimeService.Instance.Now;
            nursingShiftInfo.WardCode = SystemContext.Instance.LoginUser.WardCode;
            nursingShiftInfo.WardName = SystemContext.Instance.LoginUser.WardName;
            nursingShiftInfo.FirstSignID = nursingShiftInfo.CreatorID;
            nursingShiftInfo.FirstSignName = nursingShiftInfo.CreatorName;
            nursingShiftInfo.FirstSignTime = nursingShiftInfo.CreateTime;
            nursingShiftInfo.ShiftRecordID = nursingShiftInfo.MakeRecordID(SysTimeService.Instance.Now, 0);
            return nursingShiftInfo;
        }

        public DataTable ShowOrdersImportForm(string patientID, string visitID)
        {
            if (this.m_OrdersImportForm == null || this.m_OrdersImportForm.IsDisposed)
                this.m_OrdersImportForm = new OrdersImportForm();
            this.m_OrdersImportForm.PatientID = patientID;
            this.m_OrdersImportForm.VisitID = visitID;
            if (this.m_OrdersImportForm.ShowDialog() == DialogResult.OK)
                return this.m_OrdersImportForm.SelectedOrderTable;
            return null;
        }

        public DataTable ShowExamImportForm()
        {
            if (this.m_ExamImportForm == null || this.m_ExamImportForm.IsDisposed)
                this.m_ExamImportForm = new ExamImportForm();
            this.m_ExamImportForm.PatientID = null;
            this.m_ExamImportForm.VisitID = null;
            if (this.m_ExamImportForm.ShowDialog() == DialogResult.OK)
                return this.m_ExamImportForm.SelectedExamTable;
            return null;
        }

        public DataTable ShowExamImportForm(string patientID, string visitID)
        {
            if (this.m_ExamImportForm == null || this.m_ExamImportForm.IsDisposed)
                this.m_ExamImportForm = new ExamImportForm();
            this.m_ExamImportForm.PatientID = patientID;
            this.m_ExamImportForm.VisitID = visitID;
            if (this.m_ExamImportForm.ShowDialog() == DialogResult.OK)
                return this.m_ExamImportForm.SelectedExamTable;
            return null;
        }

        public DataTable ShowTestImportForm()
        {
            if (this.m_TestImportForm == null || this.m_TestImportForm.IsDisposed)
                this.m_TestImportForm = new TestImportForm();
            this.m_TestImportForm.PatientID = null;
            this.m_TestImportForm.VisitID = null;
            if (this.m_TestImportForm.ShowDialog() == DialogResult.OK)
                return this.m_TestImportForm.SelectedTestTable;
            return null;
        }

        public DataTable ShowTestImportForm(string patientID, string visitID)
        {
            if (this.m_TestImportForm == null || this.m_TestImportForm.IsDisposed)
                this.m_TestImportForm = new TestImportForm();
            this.m_TestImportForm.PatientID = patientID;
            this.m_TestImportForm.VisitID = visitID;
            if (this.m_TestImportForm.ShowDialog() == DialogResult.OK)
                return this.m_TestImportForm.SelectedTestTable;
            return null;
        }

        public DataTable ShowNurRecImportForm()
        {
            if (this.m_NurRecImportForm == null || this.m_NurRecImportForm.IsDisposed)
                this.m_NurRecImportForm = new NurRecImportForm();
            this.m_NurRecImportForm.PatientID = null;
            this.m_NurRecImportForm.VisitID = null;
            if (this.m_NurRecImportForm.ShowDialog() == DialogResult.OK)
                return this.m_NurRecImportForm.SelectedNursingRecord;
            return null;
        }

        public DataTable ShowNurRecImportForm(string patientID, string visitID)
        {
            if (this.m_NurRecImportForm == null || this.m_NurRecImportForm.IsDisposed)
                this.m_NurRecImportForm = new NurRecImportForm();
            this.m_NurRecImportForm.PatientID = patientID;
            this.m_NurRecImportForm.VisitID = visitID;
            if (this.m_NurRecImportForm.ShowDialog() == DialogResult.OK)
                return this.m_NurRecImportForm.SelectedNursingRecord;
            return null;
        }

        public DataTable ShowNurRecImportForm(string patientID, string visitID, DateTime dtFrom, DateTime dtTo)
        {
            if (this.m_NurRecImportForm == null || this.m_NurRecImportForm.IsDisposed)
                this.m_NurRecImportForm = new NurRecImportForm();
            this.m_NurRecImportForm.PatientID = patientID;
            this.m_NurRecImportForm.VisitID = visitID;
            this.m_NurRecImportForm.DateTimeFrom = dtFrom;
            this.m_NurRecImportForm.DateTimeTo = dtTo;
            if (this.m_NurRecImportForm.ShowDialog() == DialogResult.OK)
                return this.m_NurRecImportForm.SelectedNursingRecord;
            return null;
        }

        public string ShowFoodEleImportForm()
        {
            if (this.m_FoodEleImportForm == null || this.m_FoodEleImportForm.IsDisposed)
                this.m_FoodEleImportForm = new FoodEleImportForm();
            if (this.m_FoodEleImportForm.ShowDialog() == DialogResult.OK)
                return this.m_FoodEleImportForm.SelectedFName;
            return null;
        }

        public DataTable ShowAntenatalDialog(string szDocTypeID, DataTable dtAntenatal)
        {
            if (this.m_AntenatalInfoForm == null || this.m_AntenatalInfoForm.IsDisposed)
                this.m_AntenatalInfoForm = new AntenatalInfoForm();
            this.m_AntenatalInfoForm.GetAntenatalTable = dtAntenatal;
            this.m_AntenatalInfoForm.DocTypeID = szDocTypeID;
            if (this.m_AntenatalInfoForm.ShowDialog() == DialogResult.OK)
                return this.m_AntenatalInfoForm.SelectedAntenatalTable;
            return null;
        }

        public UserInfo[] ShowUserSelectDialog(bool multiSelect, UserInfo[] userList)
        {
            if (this.m_UserSelectDialog == null || this.m_UserSelectDialog.IsDisposed)
                this.m_UserSelectDialog = new UserSelectDialog();
            this.m_UserSelectDialog.MultiSelect = multiSelect;
            this.m_UserSelectDialog.UserInfos = userList;
            if (this.m_UserSelectDialog.ShowDialog() == DialogResult.OK)
                return this.m_UserSelectDialog.UserInfos;
            return userList;
        }

        public DataTable ShowUserSelectDialog(bool multiSelect, DataTable defaultUserList)
        {
            if (this.m_UserSelectDialog == null || this.m_UserSelectDialog.IsDisposed)
                this.m_UserSelectDialog = new UserSelectDialog();
            this.m_UserSelectDialog.MultiSelect = multiSelect;
            if (defaultUserList != null)
            {
                List<UserInfo> result = new List<UserInfo>();
                foreach (DataRow row in defaultUserList.Rows)
                {
                    UserInfo userInfo = new UserInfo();
                    if (!row.IsNull("user_id"))
                        userInfo.ID = row["user_id"].ToString();
                    if (!row.IsNull("user_name"))
                        userInfo.Name = row["user_name"].ToString();
                    result.Add(userInfo);
                }
                this.m_UserSelectDialog.UserInfos = result.ToArray();
            }
            if (this.m_UserSelectDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable table = new DataTable("users");
                table.Columns.Add("user_id", typeof(string));
                table.Columns.Add("user_name", typeof(string));
                table.Columns.Add("dept_code", typeof(string));
                table.Columns.Add("dept_name", typeof(string));
                table.Columns.Add("ward_code", typeof(string));
                table.Columns.Add("ward_name", typeof(string));

                UserInfo[] arrUserList = this.m_UserSelectDialog.UserInfos;
                if (arrUserList == null || arrUserList.Length <= 0)
                    return null;
                foreach (UserInfo userInfo in arrUserList)
                {
                    table.Rows.Add(userInfo.ID, userInfo.Name, userInfo.DeptCode
                        , userInfo.DeptName, userInfo.WardCode, userInfo.WardName);
                }
                return table;
            }
            return defaultUserList;
        }

        public DeptInfo[] ShowDeptSelectDialog(int defaultDeptType
            , bool multiSelect, DeptInfo[] deptList)
        {
            if (this.m_DeptSelectDialog == null || this.m_DeptSelectDialog.IsDisposed)
                this.m_DeptSelectDialog = new DeptSelectDialog();
            this.m_DeptSelectDialog.MultiSelect = multiSelect;
            this.m_DeptSelectDialog.DefaultDeptType = defaultDeptType;
            this.m_DeptSelectDialog.DeptInfos = deptList;
            if (this.m_DeptSelectDialog.ShowDialog() == DialogResult.OK)
                return this.m_DeptSelectDialog.DeptInfos;
            return deptList;
        }

        public DataTable ShowDeptSelectDialog(int defaultDeptType
            , bool multiSelect, DataTable defaultDeptList)
        {
            if (this.m_DeptSelectDialog == null || this.m_DeptSelectDialog.IsDisposed)
                this.m_DeptSelectDialog = new DeptSelectDialog();
            this.m_DeptSelectDialog.MultiSelect = multiSelect;
            this.m_DeptSelectDialog.DefaultDeptType = defaultDeptType;
            if (defaultDeptList != null)
            {
                List<DeptInfo> result = new List<DeptInfo>();
                foreach (DataRow row in defaultDeptList.Rows)
                {
                    DeptInfo deptInfo = new DeptInfo();
                    if (!row.IsNull("dept_code"))
                        deptInfo.DeptCode = row["dept_code"].ToString();
                    if (!row.IsNull("dept_name"))
                        deptInfo.DeptName = row["dept_name"].ToString();
                    result.Add(deptInfo);
                }
                this.m_DeptSelectDialog.DeptInfos = result.ToArray();
            }
            if (this.m_DeptSelectDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable table = new DataTable("depts");
                table.Columns.Add("dept_code", typeof(string));
                table.Columns.Add("dept_name", typeof(string));
                table.Columns.Add("is_clinic_dept", typeof(bool));
                table.Columns.Add("is_ward_dept", typeof(bool));
                table.Columns.Add("is_outp_dept", typeof(bool));
                table.Columns.Add("is_nurse_dept", typeof(bool));
                table.Columns.Add("is_user_group", typeof(bool));

                DeptInfo[] arrDeptList = this.m_DeptSelectDialog.DeptInfos;
                if (arrDeptList == null || arrDeptList.Length <= 0)
                    return null;
                foreach (DeptInfo deptInfo in arrDeptList)
                {
                    table.Rows.Add(deptInfo.DeptCode, deptInfo.DeptName, deptInfo.IsClinicDept
                        , deptInfo.IsWardDept, deptInfo.IsOutpDept, deptInfo.IsNurseDept, deptInfo.IsUserGroup);
                }
                return table;
            }
            return defaultDeptList;
        }

        /// <summary>
        /// 在护理评价中显示科室选择FORM
        /// </summary>
        /// <param name="defaultDeptType"></param>
        /// <param name="multiSelect"></param>
        /// <param name="defaultDeptList"></param>
        /// <param name="deptTypeEnabled"></param>
        /// <returns></returns>
        public DataTable ShowDeptSelectDialog(int defaultDeptType
            , bool multiSelect, DataTable defaultDeptList, bool deptTypeEnabled)
        {
            if (this.m_DeptSelectDialog == null || this.m_DeptSelectDialog.IsDisposed)
                this.m_DeptSelectDialog = new DeptSelectDialog();
            
            this.m_DeptSelectDialog.MultiSelect = multiSelect;
            this.m_DeptSelectDialog.DefaultDeptType = defaultDeptType;
            this.m_DeptSelectDialog.DeptTypeEnabled = deptTypeEnabled;
            if (defaultDeptList != null)
            {
                List<DeptInfo> result = new List<DeptInfo>();
                foreach (DataRow row in defaultDeptList.Rows)
                {
                    DeptInfo deptInfo = new DeptInfo();
                    if (!row.IsNull("dept_code"))
                        deptInfo.DeptCode = row["dept_code"].ToString();
                    if (!row.IsNull("dept_name"))
                        deptInfo.DeptName = row["dept_name"].ToString();
                    result.Add(deptInfo);
                }
                this.m_DeptSelectDialog.DeptInfos = result.ToArray();
            }
            if (this.m_DeptSelectDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable table = new DataTable("depts");
                table.Columns.Add("dept_code", typeof(string));
                table.Columns.Add("dept_name", typeof(string));
                table.Columns.Add("is_clinic_dept", typeof(bool));
                table.Columns.Add("is_ward_dept", typeof(bool));
                table.Columns.Add("is_outp_dept", typeof(bool));
                table.Columns.Add("is_nurse_dept", typeof(bool));
                table.Columns.Add("is_user_group", typeof(bool));

                DeptInfo[] arrDeptList = this.m_DeptSelectDialog.DeptInfos;
                if (arrDeptList == null || arrDeptList.Length <= 0)
                    return null;
                foreach (DeptInfo deptInfo in arrDeptList)
                {
                    table.Rows.Add(deptInfo.DeptCode, deptInfo.DeptName, deptInfo.IsClinicDept
                        , deptInfo.IsWardDept, deptInfo.IsOutpDept, deptInfo.IsNurseDept, deptInfo.IsUserGroup);
                }
                return table;
            }
            return defaultDeptList;
        }

        public string ShowReasonDialog(string catalog, string Reason)
        {
            if (this.m_ReasonDialog == null || this.m_ReasonDialog.IsDisposed)
                this.m_ReasonDialog = new ReasonDialog(catalog, Reason);
            if (this.m_ReasonDialog.ShowDialog() == DialogResult.OK)
                return this.m_ReasonDialog.Reason;
            return string.Empty;
        }
    }
}
