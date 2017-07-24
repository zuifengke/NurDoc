// ***********************************************************
// 护理电子病历系统,交接班模块病人筛选窗口.
// Creator:YangMingkun  Date:2013-7-12
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Controls;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class ShiftRecFilterForm : HerenForm
    {
        private NursingShiftInfo m_nursingShiftInfo = null;

        /// <summary>
        ///  获取或设置交班主记录信息
        /// </summary>
        [Browsable(false)]
        public NursingShiftInfo NursingShiftInfo
        {
            get { return this.m_nursingShiftInfo; }
            set { this.m_nursingShiftInfo = value; }
        }

        private ShiftRankInfo m_shiftRankInfo = null;

        /// <summary>
        /// 设置交班主记录信息
        /// </summary>
        [Browsable(false)]
        public ShiftRankInfo ShiftRankInfo
        {
            //get { return this.m_shiftRankInfo; }
            set { this.m_shiftRankInfo = value; }
        }

        private ShiftPatient[] m_selectedPatients = null;

        /// <summary>
        /// 获取用户勾选的病人列表
        /// </summary>
        [Browsable(false)]
        public ShiftPatient[] SelectedPatients
        {
            get { return this.m_selectedPatients; }
            set { this.m_selectedPatients = value; }
        }

        private SpecialShiftInfo m_specialShiftInfo = null;

        /// <summary>
        ///  获取或设置特殊病人病情交班主记录信息
        /// </summary>
        [Browsable(false)]
        public SpecialShiftInfo SpecialShiftInfo
        {
            get { return this.m_specialShiftInfo; }
            set { this.m_specialShiftInfo = value; }
        }

        private ShiftSpecialPatient[] m_selectedSpecialPatients = null;

        /// <summary>
        /// 获取用户勾选的特殊病人列表
        /// </summary>
        [Browsable(false)]
        public ShiftSpecialPatient[] SelectedSpecialPatients
        {
            get { return this.m_selectedSpecialPatients; }
            set { this.m_selectedSpecialPatients = value; }
        }

        /// <summary>
        /// 标识当前窗口是否被特殊病人交班调用
        /// </summary>
        private bool m_bIsSpecialPatient = false;

        /// <summary>
        /// 获取或设置当前窗口是否被特殊病人交班调用
        /// </summary>
        [Browsable(false)]
        public bool IsSpecialPatient
        {
            get { return this.m_bIsSpecialPatient; }
            set { this.m_bIsSpecialPatient = value; }
        }

        /// <summary>
        /// 标识当前是否通过新增病人按钮调用筛选病人功能
        /// </summary>
        private bool m_bIsbtnAddPatientClick = false;

        /// <summary>
        /// 获取或设置当前是否通过新增病人按钮调用筛选病人功能
        /// </summary>
        [Browsable(false)]
        public bool IsbtnAddPatientClick
        {
            get { return this.m_bIsbtnAddPatientClick; }
            set { this.m_bIsbtnAddPatientClick = value; }
        }

        public ShiftRecFilterForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.PersonsIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadFilterTemplet();
            Application.DoEvents();
            this.LoadDefaultPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            Application.DoEvents();
            LoadDefaultSpecialPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            Application.DoEvents();
            if (this.m_bIsbtnAddPatientClick == true)
                this.formControl1.UpdateFormData("默认不勾选病人", true);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_bIsSpecialPatient == true)
            {
                this.m_selectedSpecialPatients = this.GetSelectedSpecialPatientList();
                if (this.m_selectedSpecialPatients != null)
                    this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.m_selectedPatients = this.GetSelectedPatientList();
                if (this.m_selectedPatients != null)
                    this.DialogResult = DialogResult.OK;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载病人筛选表单
        /// </summary>
        /// <returns>是否成功</returns>
        public bool LoadFilterTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_PATIENT_FILTER;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("病人筛选表单还没有制作!");
                return false;
            }
            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("病人筛选表单下载失败!");
                return false;
            }
            bool bsuccess = this.formControl1.Load(docTypeInfo, byteFormData);
            this.formControl1.UpdateFormData("特殊病人筛选", this.m_bIsSpecialPatient);
            return bsuccess;
        }

        /// <summary>
        /// 加载缺省勾选的病人列表
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadDefaultPatient()
        {
            if (this.m_bIsSpecialPatient == true)
                return false;
            if (this.m_selectedPatients == null
                || this.m_selectedPatients.Length <= 0)
                return false;
            DataTable tablePatient = GlobalMethods.Table.GetDataTable(this.m_selectedPatients);
            return this.formControl1.UpdateFormData("病人列表", tablePatient);
        }

        /// <summary>
        /// 加载缺省勾选的特殊病人列表
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadDefaultSpecialPatient()
        {
            if (this.m_bIsSpecialPatient == false)
                return false;
            if (this.m_selectedSpecialPatients == null
                || this.m_selectedSpecialPatients.Length <= 0)
                return false;
            DataTable tablePatient = GlobalMethods.Table.GetDataTable(this.m_selectedSpecialPatients);
            return this.formControl1.UpdateFormData("病人列表", tablePatient);
        }

        /// <summary>
        /// 获取当前用户勾选的需要交班的病人列表
        /// </summary>
        /// <returns>勾选的病人列表</returns>
        public ShiftPatient[] GetSelectedPatientList()
        {
            DataTable table = this.formControl1.GetFormData("病人列表") as DataTable;
            if (table == null)
            {
                MessageBoxEx.ShowError("交班病人列表筛选失败!");
                return null;
            }

            List<ShiftPatient> lstShiftPatients = new List<ShiftPatient>();
            foreach (DataRow row in table.Rows)
            {
                ShiftPatient shiftPatient = new ShiftPatient();
                shiftPatient.FromDataRow(row);
                if (!GlobalMethods.Misc.IsEmptyString(shiftPatient.PatientID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftPatient.VisitID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftPatient.SubID))
                    lstShiftPatients.Add(shiftPatient);
            }
            return lstShiftPatients.ToArray();
        }

        /// <summary>
        /// 获取当前用户勾选的需要交班的特殊病人列表
        /// </summary>
        /// <returns>勾选的特殊病人列表</returns>
        public ShiftSpecialPatient[] GetSelectedSpecialPatientList()
        {
            DataTable table = this.formControl1.GetFormData("特殊病人列表") as DataTable;
            if (table == null)
            {
                MessageBoxEx.ShowError("特殊病人交班列表筛选失败!");
                return null;
            }
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            List<ShiftSpecialPatient> lstShiftSpecialPatients = new List<ShiftSpecialPatient>();
            foreach (DataRow row in table.Rows)
            {
                ShiftSpecialPatient shiftSpecialPatient = new ShiftSpecialPatient();
                shiftSpecialPatient.FromDataRow(row);
                if (!GlobalMethods.Misc.IsEmptyString(shiftSpecialPatient.PatientID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftSpecialPatient.VisitID)
                    && !GlobalMethods.Misc.IsEmptyString(shiftSpecialPatient.SubID))
                    lstShiftSpecialPatients.Add(shiftSpecialPatient);
            }
            return lstShiftSpecialPatients.ToArray();
        }

        private void formControl1_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            if (e.Name == "交班信息")
            {
                if (this.m_bIsSpecialPatient == true)
                { e.Success = true; }
                else
                {
                    e.Value = this.m_shiftRankInfo.ToDataTable();
                    e.Success = true;
                }
            }
        }
    }
}