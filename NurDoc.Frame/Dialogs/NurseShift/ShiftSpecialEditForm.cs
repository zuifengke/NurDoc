// ***********************************************************
// 护理电子病历系统,交接班特殊病人特殊病情新增和修改窗口.
// Creator:YangMingkun  Date:2013-7-12
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Frame.DockForms;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class ShiftSpecialEditForm : HerenForm
    {
        /// <summary>
        /// 标识当前编辑窗口中是否对数据库有更新过
        /// </summary>
        private bool m_bRecordUpdated = false;

        /// <summary>
        /// 获取当前编辑窗口中是否对数据库有更新过
        /// </summary>
        public bool RecordUpdated
        {
            get { return this.m_bRecordUpdated; }
        }

        private ShiftSpecialPatient m_defaultShiftSpecialPatient = null;

        /// <summary>
        ///  获取或设置默认选中的交班病人
        /// </summary>
        [Browsable(false)]
        public ShiftSpecialPatient DefaultShiftSpecialPatient
        {
            get { return this.m_defaultShiftSpecialPatient; }
            set { this.m_defaultShiftSpecialPatient = value; }
        }

        private DateTime m_ShiftDate = DateTime.Now;

        /// <summary>
        /// 获取或设置当前特殊病人病情交接班对应的时间
        /// </summary>
        public DateTime ShiftDate
        {
            get { return this.m_ShiftDate; }
            set { this.m_ShiftDate = value; }
        }

        /// <summary>
        /// 标识当前交班是否是新交班
        /// </summary>
        private bool m_bIsNewSpecialShift = true;

        /// <summary>
        /// 获取当前交班是否是新交班
        /// </summary>
        public bool IsNewSpecialShift
        {
            get { return this.m_bIsNewSpecialShift; }
        }

        /// <summary>
        /// 标识特殊病人交班主记录信息
        /// </summary>
        private SpecialShiftInfo m_specialShiftInfo = null;

        public ShiftSpecialEditForm()
        {
            this.InitializeComponent();
            this.btnAddPatient.Image = Properties.Resources.Add;
            this.btnDeletePatient.Image = Properties.Resources.Delete;
            this.btnDeleteAllPatient.Visible = false;
            this.btnDeleteAllPatient.Image = Properties.Resources.Delete;
            this.Icon = Properties.Resources.ShiftIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //加载当前文档状态
            if (this.m_specialShiftInfo == null)
                this.m_specialShiftInfo = this.GetSelectedShiftInfo();
            this.EditState(this.m_specialShiftInfo);

            //加载交班信息编辑表单
            this.LoadRecordEditTemplet();

            //加载当前传入的特殊病人交班记录
            this.LoadSpecialShiftData();
            
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.dgvShiftPatient.EndEdit();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// 获取用户当前选择的交班信息
        /// </summary>
        /// <returns>交班信息</returns>
        private SpecialShiftInfo GetSelectedShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.m_ShiftDate.Date;
            SpecialShiftInfo specialShiftInfo = null;
            short shRet = NurShiftService.Instance.GetSpecialShiftInfo(szWardCode, dtShiftDate, ref specialShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK
                && shRet != ServerData.ExecuteResult.RES_NO_FOUND)
            {
                MessageBoxEx.Show("查询特殊病人病情交班记录失败!");
                return null;
            }
            return specialShiftInfo;
        }

        /// <summary>
        /// 检查当前数据编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否有未提交</returns>
        private bool HasUncommit()
        {
            if (this.dgvShiftPatient.CurrentRow != null
                && this.formControl1.IsModified)
                return true;
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (row.IsModifiedRow || row.IsNewRow)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 检查并保存当前数据编辑窗口的修改
        /// </summary>
        /// <returns>是否取消</returns>
        private bool CheckModifyState()
        {
            if (!this.HasUncommit())
                return false;

            DialogResult result = MessageBoxEx.ShowQuestion("当前数据已修改,是否保存？");
            if (result == DialogResult.Yes)
                return !this.SaveSpecialShiftData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// 加班交班记录内容编辑表单
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadRecordEditTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_SPECIALPATIENT;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("特殊病人交班信息编辑表单还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("特殊病人交班信息编辑表单下载失败!");
                return false;
            }
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// 根据当前交班记录对象信息判断当前文档状态（新增/修改）
        /// </summary>
        /// <param name="specialShiftInfo">交班记录</param>
        /// <returns>是否为新增</returns>
        private bool EditState(SpecialShiftInfo specialShiftInfo)
        {
            if (this.CheckModifyState())
            {
                return false;
            }
            if (specialShiftInfo == null)
            {
                this.Text = "编辑特殊病人病情交接(新增)";
                this.m_bIsNewSpecialShift = true;
                this.m_specialShiftInfo = this.CreateSpecialShiftInfo();
                SpecialShiftInfo specialShiftInfoDB = new SpecialShiftInfo();
                short shRet = NurShiftService.Instance.GetSpecialShiftInfo(
                    this.m_specialShiftInfo.WardCode,
                    this.m_specialShiftInfo.ShiftRecordDate,
                    ref specialShiftInfoDB);
                if (shRet != ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    MessageBoxEx.Show("发生错误：数据库中已有该班次数据，无法新增，请重新打开页面。");
                    this.m_bRecordUpdated = true;//使交接班主界面刷新数据。
                    this.Close();
                }
            }
            else
            {
                this.Text = "编辑特殊病人病情交接(修改)";
                this.m_bIsNewSpecialShift = false;
                this.m_specialShiftInfo = specialShiftInfo;
            }
            return true;
        }

        /// <summary>
        /// 根据当前交班记录对象信息加载当次交班数据
        /// </summary>
        /// <param name="specialShiftInfo">交班记录</param>
        /// <returns>是否成功</returns>
        private bool LoadSpecialShiftData()
        {
            if (!this.m_bIsNewSpecialShift)
            {
                this.LoadShiftSpecialPatientList(this.m_specialShiftInfo);
            }
            else
            {
                this.LoadShiftSpecialPatientList(null);
                //如果是新交班,那么根据规则加载默认病人
                ShiftSpecialPatient[] arrShiftSpecialPatient = null;
                ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
                shiftRecFilterForm.IsSpecialPatient = true;
                shiftRecFilterForm.SpecialShiftInfo = this.m_specialShiftInfo;
                if (shiftRecFilterForm.LoadFilterTemplet())
                {
                    arrShiftSpecialPatient = shiftRecFilterForm.GetSelectedSpecialPatientList();
                }
                shiftRecFilterForm.Dispose();
                this.LoadShiftSpecialPatientList(arrShiftSpecialPatient, true);
            }
            return true;
        }

        /// <summary>
        /// 加载指定交班记录关联的病人列表数据
        /// </summary>
        /// <param name="specialShiftInfo">交班记录</param>
        /// <returns>是否成功</returns>
        private bool LoadShiftSpecialPatientList(SpecialShiftInfo specialShiftInfo)
        {
            this.dgvShiftPatient.Focus();
            this.dgvShiftPatient.Rows.Clear();
            this.ShowSelectedSpecialShiftPatient();
            this.Update();
            if (specialShiftInfo == null)
                return true;

            string szShiftRecordID = specialShiftInfo.ShiftRecordID;
            List<ShiftSpecialPatient> lstShiftSpecialPatients = null;
            short shRet = NurShiftService.Instance.GetShiftSpecialPatientList(szShiftRecordID, ref lstShiftSpecialPatients);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("查询特殊病人病情交班列表失败!");
                return false;
            }
            if (lstShiftSpecialPatients == null || lstShiftSpecialPatients.Count <= 0)
                return true;

            //lstShiftPatients = this.GetOrderedShiftPatients(lstShiftPatients);
            bool success = this.LoadShiftSpecialPatientList(lstShiftSpecialPatients.ToArray(), false);
            this.dgvShiftPatient.SelectRow(this.GetShiftPatientTableRow(this.m_defaultShiftSpecialPatient));
            return success;
        }

        /// <summary>
        /// 添加指定的交班病人列表到交班病人表格中
        /// </summary>
        /// <param name="arrShiftSpecialPatients">交班病人列表</param>
        /// <param name="append">是否是追加</param>
        /// <returns>是否成功</returns>
        private bool LoadShiftSpecialPatientList(ShiftSpecialPatient[] arrShiftSpecialPatients, bool append)
        {
            if (arrShiftSpecialPatients == null || arrShiftSpecialPatients.Length <= 0)
                return false;
            foreach (ShiftSpecialPatient shiftSpecialPatient in arrShiftSpecialPatients)
            {
                if (shiftSpecialPatient == null)
                    continue;
                if (append && this.GetShiftPatientTableRow(shiftSpecialPatient) != null)
                    continue;
                int rowIndex = this.dgvShiftPatient.Rows.Add();
                DataTableViewRow row = this.dgvShiftPatient.Rows[rowIndex];
                row.Tag = shiftSpecialPatient;
                row.Cells[this.colBedCode.Index].Value = shiftSpecialPatient.BedCode;
                row.Cells[this.colPatientName.Index].Value = shiftSpecialPatient.PatientName;
                row.Cells[this.colID.Index].Value = shiftSpecialPatient.PatientID;
                this.dgvShiftPatient.SetRowState(row, append ? RowState.New : RowState.Normal);
            }
            this.dgvShiftPatient.Sort(new BedCodeComparer());
            this.ShowSelectedSpecialShiftPatient();
            return true;
        }

        #region 自定义排序
        /// <summary>
        /// 比较床号大小，实现接口IComparer   先数字后其他
        /// </summary>
        public class BedCodeComparer : IComparer
        {
            #region IComparer Members
            public int Compare(object x, object y)
            {
                DataGridViewRow xRow = x as DataGridViewRow;
                DataGridViewRow yRow = y as DataGridViewRow;
                if (xRow.Cells["colBedCode"].Value == null)
                {
                    if (yRow.Cells["colBedCode"].Value == null)
                        return 0;
                    else
                        return -1;
                }
                else
                {
                    if (yRow.Cells["colBedCode"].Value == null)
                        return 1;
                }

                string szXBedCode = xRow.Cells["colBedCode"].Value.ToString();
                string szYBedCode = yRow.Cells["colBedCode"].Value.ToString();
                int iXBedCode;
                int iYBedCode;
                bool bXIsNumber = int.TryParse(szXBedCode, out iXBedCode);
                bool bYIsNumber = int.TryParse(szYBedCode, out iYBedCode);
                if (bXIsNumber)
                {
                    if (bYIsNumber)
                    {
                        if (iXBedCode > iYBedCode)
                            return 1;
                        else if (iXBedCode < iYBedCode)
                            return -1;
                        else
                            return 0;
                    }
                    else
                        return 1;
                }
                else
                {
                    if (bYIsNumber)
                        return -1;
                    else
                    {
                        int temp = string.Compare(szXBedCode, szYBedCode);
                        if (temp > 0)
                            return -1;
                        else if (temp < 0)
                            return 1;
                    }
                }
                return 0;
            }

            #endregion
        }
        #endregion

        /// <summary>
        /// 加载交班内容数据到交班内容编辑窗口中
        /// </summary>
        /// <returns>是否成功</returns>
        private bool ShowSelectedSpecialShiftPatient()
        {
            this.UpdateSelectedSpecialShiftPatient();

            if (this.dgvShiftPatient.CurrentRow == null)
            {
                this.formControl1.Tag = null;
                this.formControl1.UpdateFormData("刷新视图", null);
                this.formControl1.IsModified = false;
                return false;
            }

            ShiftSpecialPatient shiftSpecialPatient = this.dgvShiftPatient.CurrentRow.Tag as ShiftSpecialPatient;
            if (shiftSpecialPatient != null)
                shiftSpecialPatient.ShiftDate = this.m_ShiftDate;
            this.formControl1.Tag = shiftSpecialPatient;
            DataTable table = GlobalMethods.Table.GetDataTable(shiftSpecialPatient);
            this.formControl1.UpdateFormData("刷新视图", table);

            object Visible = this.formControl1.GetFormData("交接班是否可编辑");
            if (Visible != null)
            {
                this.btnAddPatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.btnDeletePatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.dgvShiftPatient.ReadOnly = (Visible is bool) ? !(bool)Visible : true;
                this.toolbtnSave.Enabled = (Visible is bool) ? (bool)Visible : false;
            }

            this.formControl1.IsModified = false;
            return true;
        }

        /// <summary>
        /// 获取指定的交班病人对应的交班病人表格行
        /// </summary>
        /// <param name="shiftSpecialPatient">交班病人信息</param>
        /// <returns>交班病人表格行</returns>
        private DataTableViewRow GetShiftPatientTableRow(ShiftSpecialPatient shiftSpecialPatient)
        {
            if (shiftSpecialPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftSpecialPatient patient = currentRow.Tag as ShiftSpecialPatient;
            if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                && patient.VisitID == shiftSpecialPatient.VisitID
                && patient.SubID == shiftSpecialPatient.SubID)
                return currentRow;

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftSpecialPatient;
                if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                    && patient.VisitID == shiftSpecialPatient.VisitID
                    && patient.SubID == shiftSpecialPatient.SubID)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// 创建一条新的特殊病人病情交班主记录
        /// </summary>
        /// <returns>特殊病人病情主记录</returns>
        private SpecialShiftInfo CreateSpecialShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string strShiftDate = this.m_ShiftDate.ToShortDateString();
            DateTime dtShiftDate = DateTime.Now;
            GlobalMethods.Convert.StringToDate(strShiftDate, ref dtShiftDate);
           
            return NursingShiftHandler.Instance.Create(SystemContext.Instance.LoginUser, dtShiftDate, SysTimeService.Instance.Now);
        }

        /// <summary>
        /// 保存当前已修改的特殊病人病情交班数据
        /// </summary>
        /// <returns>是否成功</returns>
        private bool SaveSpecialShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_specialShiftInfo == null)
            {
                MessageBoxEx.Show("交班主记录信息为空,无法保存!");
                return false;
            }
            //用于判断是否拥有权限
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo = null;
            if (!RightController.Instance.CanEditShiftRec(nursingShiftInfo))
                return false;

            short shRet = SystemConst.ReturnValue.OK;

            UserInfo userInfo = SystemContext.Instance.LoginUser;
            this.m_specialShiftInfo.ModifierID = userInfo.ID;
            this.m_specialShiftInfo.ModifierName = userInfo.Name;
            this.m_specialShiftInfo.ModifyTime = SysTimeService.Instance.Now;

            DateTime dtShiftDate = this.m_ShiftDate.Date;
            this.m_specialShiftInfo.ShiftRecordDate = dtShiftDate;

            //检查该班次交班主索引是否重复
            SpecialShiftInfo specialShiftInfoDB = null;
            shRet = NurShiftService.Instance.GetSpecialShiftInfo(
               this.m_specialShiftInfo.WardCode,
               this.m_specialShiftInfo.ShiftRecordDate,
               ref specialShiftInfoDB);
            if (shRet != ServerData.ExecuteResult.RES_NO_FOUND
                && specialShiftInfoDB.ShiftRecordID != this.m_specialShiftInfo.ShiftRecordID)
            {
                MessageBoxEx.Show("发生错误：数据库中已有该班次数据，无法新增，请重新打开页面。");
                this.m_bRecordUpdated = true;//使交接班主界面刷新数据。
                //屏蔽是否保存提示框
                this.formControl1.IsModified = false;
                this.Close();
            }

            //保存交班病人列表
            this.UpdateSelectedSpecialShiftPatient();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (this.dgvShiftPatient.IsNormalRow(row))
                    continue;
                ShiftSpecialPatient shiftSpecialPatient = row.Tag as ShiftSpecialPatient;
                if (shiftSpecialPatient == null)
                    continue;
                shiftSpecialPatient.PatientNo = row.Index;
                shiftSpecialPatient.ShiftRecordID = this.m_specialShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftSpecialPatient(shiftSpecialPatient);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班病人信息失败!");
                    return false;
                }
                this.dgvShiftPatient.SetRowState(row, RowState.Normal);
            }
            this.formControl1.IsModified = false;

            //保存特殊病人病情交班主索引记录
            shRet = NurShiftService.Instance.SaveSpecialShiftInfo(this.m_specialShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("保存交班主记录信息失败!");
                return false;
            }

            this.m_bRecordUpdated = true;
            return true;
        }

        /// <summary>
        /// 将编辑的特殊病人病情交班内容更新到特殊病人病情交班表格行中
        /// </summary>
        /// <returns>是否成功</returns>
        private bool UpdateSelectedSpecialShiftPatient()
        {
            if (!this.dgvShiftPatient.EndEdit())
                return false;
            if (!this.formControl1.EndEdit())
                return false;

            ShiftSpecialPatient shiftSpecialPatient = this.formControl1.Tag as ShiftSpecialPatient;
            if (shiftSpecialPatient == null)
                return false;

            DataTableViewRow row = this.GetShiftPatientTableRow(shiftSpecialPatient);
            if (row == null)
                return false;

            if (this.formControl1.IsModified)
            {
                if (this.dgvShiftPatient.IsNormalRowUndeleted(row))
                    this.dgvShiftPatient.SetRowState(row, RowState.Update);
            }

            DataTable table = this.formControl1.GetFormData("交班信息") as DataTable;
            if (table == null || table.Rows.Count <= 0)
                return false;

            ShiftSpecialPatient shiftSpecialPatientModify = new ShiftSpecialPatient();
            shiftSpecialPatientModify.FromDataRow(table.Rows[0]);

            shiftSpecialPatient.BedCode = shiftSpecialPatientModify.BedCode;
            shiftSpecialPatient.PatientSex = shiftSpecialPatientModify.PatientSex;
            shiftSpecialPatient.PatientAge = shiftSpecialPatientModify.PatientAge;
            shiftSpecialPatient.Diagnosis = shiftSpecialPatientModify.Diagnosis;
            
            if (shiftSpecialPatientModify.LogDateTime != DateTime.MinValue)
                shiftSpecialPatient.LogDateTime = shiftSpecialPatientModify.LogDateTime.Date;
            shiftSpecialPatient.NursingClass = shiftSpecialPatientModify.NursingClass;
            shiftSpecialPatient.Diet = shiftSpecialPatientModify.Diet;
            shiftSpecialPatient.RequestDoctor = shiftSpecialPatientModify.RequestDoctor;
            shiftSpecialPatient.AllergicDrug = shiftSpecialPatientModify.AllergicDrug;
            shiftSpecialPatient.AdverseReactionDrug = shiftSpecialPatientModify.AdverseReactionDrug;
            shiftSpecialPatient.OthersInfo = shiftSpecialPatientModify.OthersInfo;
            shiftSpecialPatient.Remark = shiftSpecialPatientModify.Remark;
            return true;
        }

        /// <summary>
        /// 获取当前交班病人表格中显示的交班病人数据
        /// </summary>
        /// <returns>交班病人数据</returns>
        private ShiftSpecialPatient[] GetShiftSpecialPatientTableData()
        {
            List<ShiftSpecialPatient> lstShiftSpecialPatients = new List<ShiftSpecialPatient>();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                ShiftSpecialPatient patient = row.Tag as ShiftSpecialPatient;
                if (patient != null)
                    lstShiftSpecialPatients.Add(patient);
            }
            return lstShiftSpecialPatients.ToArray();
        }

        /// <summary>
        /// 获取指定的特殊病人对应的特殊病人交班表格行
        /// </summary>
        /// <param name="shiftPatient">交班病人信息</param>
        /// <returns>交班病人表格行</returns>
        private DataTableViewRow GetShiftSpecialPatientTableRow(ShiftSpecialPatient shiftSpecialPatient)
        {
            if (shiftSpecialPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftSpecialPatient patient = currentRow.Tag as ShiftSpecialPatient;
            if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                && patient.VisitID == shiftSpecialPatient.VisitID
                && patient.SubID == shiftSpecialPatient.SubID)
                return currentRow;

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftSpecialPatient;
                if (patient != null && patient.PatientID == shiftSpecialPatient.PatientID
                    && patient.VisitID == shiftSpecialPatient.VisitID
                    && patient.SubID == shiftSpecialPatient.SubID)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// 删除交班病人表中当前选中的交班病人
        /// </summary>
        /// <returns>是否成功</returns>
        private bool DeleteSelectedShiftSpecialPatient()
        {
            DataTableViewRow row = this.dgvShiftPatient.CurrentRow;
            if (row == null)
                return false;
            //用于判断是否对当前模块有编辑权限
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo = null;
            if (!RightController.Instance.CanEditShiftRec(nursingShiftInfo))
                return false;
            DialogResult result = MessageBoxEx.ShowConfirm("确认删除当前选中的病人吗？");
            if (result != DialogResult.OK)
                return false;
            if (row.State == RowState.New)
            {
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedSpecialShiftPatient();
                return true;
            }
            ShiftSpecialPatient shiftSpecialPatient = row.Tag as ShiftSpecialPatient;
            if (shiftSpecialPatient == null)
                return false;
            string szRecordID = shiftSpecialPatient.ShiftRecordID;
            string szPatientID = shiftSpecialPatient.PatientID;
            string szVisitID = shiftSpecialPatient.VisitID;
            string szSubID = shiftSpecialPatient.SubID;

            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            short shRet = NurShiftService.Instance.DeleteShiftSpecialPatient(szRecordID
                , szPatientID, szVisitID, szSubID);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                this.m_bRecordUpdated = true;
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedSpecialShiftPatient();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }
            else
            {
                MessageBoxEx.Show("删除交班病人记录失败!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }
        }

        /// <summary>
        /// 删除交班病人表中所有的交班病人
        /// </summary>
        /// <returns>是否成功</returns>
        private bool DeleteAllShiftSpecialPatient()
        {
            this.dgvShiftPatient.EndEdit();
            if (this.dgvShiftPatient.Rows.Count <= 0)
                return false;
            //用于判断是否对当前模块有编辑权限
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo = null;
            if (!RightController.Instance.CanEditShiftRec(nursingShiftInfo))
                return false;
            DialogResult result = MessageBoxEx.ShowConfirm("确认删除所有病人吗？");
            if (result != DialogResult.OK)
                return false;
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            while (this.dgvShiftPatient.Rows.Count > 0)
            {
                DataTableViewRow row = this.dgvShiftPatient.Rows[0];
                if (row.State == RowState.New)
                    this.dgvShiftPatient.Rows.Remove(row);
                else
                {
                    ShiftSpecialPatient shiftSpecialPatient = row.Tag as ShiftSpecialPatient;
                    if (shiftSpecialPatient == null)
                        return false;
                    string szRecordID = shiftSpecialPatient.ShiftRecordID;
                    string szPatientID = shiftSpecialPatient.PatientID;
                    string szVisitID = shiftSpecialPatient.VisitID;
                    string szSubID = shiftSpecialPatient.SubID;

                    short shRet = NurShiftService.Instance.DeleteShiftSpecialPatient(szRecordID
                        , szPatientID, szVisitID, szSubID);
                    if (shRet == SystemConst.ReturnValue.OK)
                        this.dgvShiftPatient.Rows.Remove(row);
                    else
                    {
                        MessageBoxEx.Show("删除交班病人记录失败!");
                        GlobalMethods.UI.SetCursor(this, Cursors.Default);
                        return false;
                    }
                }
            }
            this.ShowSelectedSpecialShiftPatient();
            this.m_bRecordUpdated = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
            
            shiftRecFilterForm.IsSpecialPatient = true;
            shiftRecFilterForm.IsbtnAddPatientClick = true;
            shiftRecFilterForm.SelectedSpecialPatients = this.GetShiftSpecialPatientTableData();
            if (shiftRecFilterForm.ShowDialog() == DialogResult.OK)
                this.LoadShiftSpecialPatientList(shiftRecFilterForm.SelectedSpecialPatients, true);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnDeletePatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedShiftSpecialPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);            
        }

        private void btnDeleteAllPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteAllShiftSpecialPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvShiftPatient_SelectionChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowSelectedSpecialShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveSpecialShiftData())
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            if (this.m_bRecordUpdated)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
        }      
    }
}