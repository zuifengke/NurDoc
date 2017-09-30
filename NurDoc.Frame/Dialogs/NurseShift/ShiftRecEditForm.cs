// ***********************************************************
// 护理电子病历系统,交接班新增和修改窗口.
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
    internal partial class ShiftRecEditForm : HerenForm
    {
        /// <summary>
        /// 交接班列表分割线尺寸
        /// </summary>
        private const string NDS_Shift_Edit_Split2_Distance = "NdsShiftEditSplit2Distance";

        /// <summary>
        /// 交接班项目宽度
        /// </summary>
        private const string NDS_Shift_Edit_Item_Width = "NdsShiftEditItemWidth";

        /// <summary>
        /// 交接班床号宽度
        /// </summary>
        private const string NDS_Shift_Edit_BedCode_Width = "NdsShiftEditBedCodeWidth";

        /// <summary>
        /// 交接班姓名宽度
        /// </summary>
        private const string NDS_Shift_Edit_Name_Width = "NdsShiftEditNameWidth";

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

        private ShiftPatient m_defaultShiftPatient = null;

        /// <summary>
        ///  获取或设置默认选中的交班病人
        /// </summary>
        [Browsable(false)]
        public ShiftPatient DefaultShiftPatient
        {
            get { return this.m_defaultShiftPatient; }
            set { this.m_defaultShiftPatient = value; }
        }

        private DateTime? m_defaultShiftDate = null;

        /// <summary>
        ///  获取或设置默认显示的交班时间
        /// </summary>
        [Browsable(false)]
        public DateTime? DefaultShiftDate
        {
            get { return this.m_defaultShiftDate; }
            set { this.m_defaultShiftDate = value; }
        }

        private string m_defaultRankCode = string.Empty;

        /// <summary>
        ///  获取或设置默认显示的交班班次
        /// </summary>
        [Browsable(false)]
        public string DefaultRankCode
        {
            get { return this.m_defaultRankCode; }
            set { this.m_defaultRankCode = value; }
        }

        private bool m_bIsNewNursingShift = true;

        /// <summary>
        /// 获取当前交班是否是新交班
        /// </summary>
        public bool IsNewNursingShift
        {
            get { return this.m_bIsNewNursingShift; }
        }

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

        /// <summary>
        /// 标识控件值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        public ShiftRecEditForm()
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
            this.m_bValueChangedEnabled = false;

            //设置默认显示的交班日期
            if (this.m_defaultShiftDate == null || !this.m_defaultShiftDate.HasValue)
                this.tooldtpShiftDate.Value = SysTimeService.Instance.Now.Date;
            else
                this.tooldtpShiftDate.Value = this.m_defaultShiftDate.Value.Date;

            //加载交班病人表格中交班项目下拉列表
            this.Update();
            this.LoadShiftItemList();

            //加载交班班次下拉列表
            this.Update();
            this.LoadShiftRankList();

            //加载当前文档状态
            if (this.m_nursingShiftInfo == null)
                this.m_nursingShiftInfo = this.GetSelectedShiftInfo();
            this.EditState(this.m_nursingShiftInfo);

            //加载病区动态编辑表单
            this.Update();
            this.LoadWardStatusTemplet();

            //加载交班信息编辑表单
            this.LoadRecordEditTemplet();

            //加载当前传入的交班记录
            this.LoadNursingShiftData();

            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.dgvShiftPatient.EndEdit();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// 检查当前数据编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否有未提交</returns>
        private bool HasUncommit()
        {
            if (this.formControl1.IsModified)
                return true;
            if (this.dgvShiftPatient.CurrentRow != null
                && this.formControl2.IsModified)
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

            DialogResult result = MessageBoxEx.ShowQuestion("当前交班数据已修改,是否保存？");
            if (result == DialogResult.Yes)
                return !this.SaveNursingShiftData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// 检查并保存this.toolcboShiftRank.SelectedIndexChanged/tooldtpShiftDate_ValueChanged前编辑窗口的修改
        /// </summary>
        /// <returns>是否取消</returns>
        private bool CheckLastModifyState()
        {
            if (!this.HasUncommit())
                return false;

            DialogResult result = MessageBoxEx.ShowQuestion("当前交班数据已修改,是否保存？");
            if (result == DialogResult.Yes)
                return !this.SaveLastNursingShiftData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// 加载交班班次下拉列表
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadShiftRankList()
        {
            this.toolcboShiftRank.Items.Clear();
            this.toolcboShiftRank.Items.AddRange(SystemContext.Instance.GetWardShiftRankList());

            object selectedItem = null;
            foreach (object item in this.toolcboShiftRank.Items)
            {
                ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                if (shiftRankInfo.RankCode == this.m_defaultRankCode)
                    selectedItem = item;
            }
            if (selectedItem != null)
                this.toolcboShiftRank.SelectedItem = selectedItem;
            else
                this.toolcboShiftRank.SelectedItem = this.GetCurrentShiftRank();
            return true;
        }

        /// <summary>
        /// 加载病区动态信息编辑表单
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadWardStatusTemplet()
        {
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
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// 加班交班记录内容编辑表单
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadRecordEditTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = SystemConst.FormName.SHIFT_PATIENT;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("交班信息编辑表单还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("交班信息编辑表单下载失败!");
                return false;
            }
            return this.formControl2.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// 根据当前交班记录对象信息判断当前文档状态（新增/修改）
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        /// <returns>是否为新增</returns>
        private bool EditState(NursingShiftInfo nursingShiftInfo)
        {
            if (this.CheckModifyState())
            {
                return false;
            }
            if (nursingShiftInfo == null)
            {
                this.Text = "编辑交班记录(新增)";
                this.m_bIsNewNursingShift = true;
                this.m_nursingShiftInfo = this.CreateNursingShiftInfo();
                NursingShiftInfo nursingShiftInfoDB = new NursingShiftInfo();
                short shRet = NurShiftService.Instance.GetNursingShiftInfo(
                    this.m_nursingShiftInfo.WardCode,
                    this.m_nursingShiftInfo.ShiftRecordDate,
                    this.m_nursingShiftInfo.ShiftRankCode,
                    ref nursingShiftInfoDB);
                if (shRet != ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    MessageBoxEx.Show("发生错误：数据库中已有该班次数据，无法新增，请重新打开页面。");
                    this.m_bRecordUpdated = true;//使交接班主界面刷新数据。
                    this.Close();
                }
            }
            else
            {
                this.Text = "编辑交班记录(修改)";
                this.m_bIsNewNursingShift = false;
                this.m_nursingShiftInfo = nursingShiftInfo;
            }
            return true;
        }

        /// <summary>
        /// 根据当前交班记录对象信息加载当次交班数据
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        /// <returns>是否成功</returns>
        private bool LoadNursingShiftData()
        {
            this.ShowShiftDateAndRank(this.m_nursingShiftInfo);
            if (!this.m_bIsNewNursingShift)
            {
                this.LoadWardStatusData(this.m_nursingShiftInfo);
                this.LoadShiftPatientList(this.m_nursingShiftInfo);
            }
            else
            {
                this.LoadWardStatusData(null);
                this.LoadShiftPatientList(null);
                //如果是新交班,那么根据规则加载默认病人
                ShiftPatient[] arrShiftPatients = null;
                ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
                ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
                shiftRecFilterForm.ShiftRankInfo = shiftRankInfo;
                shiftRecFilterForm.NursingShiftInfo = this.m_nursingShiftInfo;
                if (shiftRecFilterForm.LoadFilterTemplet())
                    arrShiftPatients = shiftRecFilterForm.GetSelectedPatientList();
                shiftRecFilterForm.Dispose();
                this.LoadShiftPatientList(arrShiftPatients, true);
            }
            return true;
        }

        /// <summary>
        /// 加载指定交班记录对应的病区动态表格数据
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        /// <returns>是否成功</returns>
        private bool LoadWardStatusData(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
            {
                this.formControl1.UpdateFormData("刷新视图", null);
                if (!this.m_bIsNewNursingShift)
                    this.formControl1.IsModified = false;
                return true;
            }
            string szShiftID = nursingShiftInfo.ShiftRecordID;
            List<ShiftWardStatus> lstShiftWardStatus = null;
            short shRet = NurShiftService.Instance.GetShiftWardStatusList(szShiftID, ref lstShiftWardStatus);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("查询交班病区动态信息失败!");
                return false;
            }
            if (lstShiftWardStatus == null || lstShiftWardStatus.Count <= 0)
                return true;

            for (int index = 0; index < lstShiftWardStatus.Count; index++)
            {
                ShiftWardStatus shiftWardStatus = lstShiftWardStatus[index];
                if (shiftWardStatus != null)
                    this.formControl1.UpdateFormData("刷新视图", shiftWardStatus.ToDataTable());
            }
            if (!this.m_bIsNewNursingShift)
                this.formControl1.IsModified = false;
            return true;
        }

        /// <summary>
        /// 加载指定交班记录关联的病人列表数据
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        /// <returns>是否成功</returns>
        private bool LoadShiftPatientList(NursingShiftInfo nursingShiftInfo)
        {
            this.dgvShiftPatient.Focus();
            this.dgvShiftPatient.Rows.Clear();
            this.ShowSelectedShiftPatient();
            this.Update();
            if (nursingShiftInfo == null)
                return true;

            string szShiftRecordID = nursingShiftInfo.ShiftRecordID;
            List<ShiftPatient> lstShiftPatients = null;
            short shRet = NurShiftService.Instance.GetShiftPatientList(szShiftRecordID, ref lstShiftPatients);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("查询交班病人列表失败!");
                return false;
            }
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return true;

            lstShiftPatients = this.GetOrderedShiftPatients(lstShiftPatients);
            lstShiftPatients = this.GetOrderedShiftPatientsByAlias(lstShiftPatients);
            bool success = this.LoadShiftPatientList(lstShiftPatients.ToArray(), false);
            this.dgvShiftPatient.SelectRow(this.GetShiftPatientTableRow(this.m_defaultShiftPatient));
            return success;
        }

        /// <summary>
        /// 添加指定的交班病人列表到交班病人表格中
        /// </summary>
        /// <param name="arrShiftPatients">交班病人列表</param>
        /// <param name="append">是否是追加</param>
        /// <returns>是否成功</returns>
        private bool LoadShiftPatientList(ShiftPatient[] arrShiftPatients, bool append)
        {
            if (arrShiftPatients == null || arrShiftPatients.Length <= 0)
                return false;
            foreach (ShiftPatient shiftPatient in arrShiftPatients)
            {
                if (shiftPatient == null)
                    continue;
                if (append && this.GetShiftPatItemTableRow(shiftPatient) != null)
                    continue;
                int rowIndex = this.dgvShiftPatient.Rows.Add();
                DataTableViewRow row = this.dgvShiftPatient.Rows[rowIndex];
                row.Tag = shiftPatient;
                if (shiftPatient.SpecialItem2.Equals("1"))
                {
                    row.Cells[this.colNewPatient.Index].Value = picBoxBlue.Image;
                }
                else {
                    row.Cells[this.colNewPatient.Index].Value = picBoxWhite.Image;
                }
                row.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                row.Cells[this.colShiftItemAlias.Index].Value = shiftPatient.ShiftItemAlias;
                row.Cells[this.colBedCode.Index].Value = shiftPatient.BedCode;
                row.Cells[this.colPatientName.Index].Value = shiftPatient.PatientName;
                this.dgvShiftPatient.SetRowState(row, append ? RowState.New : RowState.Normal);
            }
            this.dgvShiftPatient.Sort(new BedCodeComparer());
            //this.dgvShiftPatient.Sort(dgvShiftPatient.Columns[1], ListSortDirection.Ascending);
            this.ShowSelectedShiftPatient();
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

        private ComboBox m_comboBox = new ComboBox();

        /// <summary>
        /// 加载交班病人表格中的交班项目下拉列表
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadShiftItemList()
        {
            this.colShiftItem.Items.Clear();
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<ShiftItemAliasInfo> lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            short shRet = NurShiftService.Instance.GetShiftItemAliasInfos(szWardCode, ref lstShiftItemAliasInfos);
            if (shRet == SystemConst.ReturnValue.OK && lstShiftItemAliasInfos.Count > 0)
            {
                this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible = true;
                this.colShiftItemAlias.Items.Add("");
                this.m_comboBox.Items.Add("");
                foreach (ShiftItemAliasInfo shiftItemAliasInfo in lstShiftItemAliasInfos)
                {
                    this.m_comboBox.Items.Add(shiftItemAliasInfo.ItemName);
                    this.colShiftItemAlias.Items.Add(shiftItemAliasInfo.ItemAlias);
                }
                this.colShiftItem.Items.AddRange(SystemContext.Instance.GetWardShiftItemList());
            }
            else
            {
                this.dgvShiftPatient.Columns[this.colShiftItemAlias.Index].Visible = false;
                this.colShiftItem.Items.AddRange(SystemContext.Instance.GetWardShiftItemList());
            }
            return true;
        }

        /// <summary>
        /// 对指定的交班病人列表按交班项目顺序进行排序
        /// </summary>
        /// <param name="lstShiftPatients">交班病人列表</param>
        /// <returns>有序的交班病人列表</returns>
        private List<ShiftPatient> GetOrderedShiftPatients(List<ShiftPatient> lstShiftPatients)
        {
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return lstShiftPatients;

            List<ShiftPatient> lstResultList = new List<ShiftPatient>();
            CommonDictItem[] commonDictItems = SystemContext.Instance.GetWardShiftItemList();
            foreach (CommonDictItem commonDictItem in commonDictItems)
            {
                int index = 0;
                while (index < lstShiftPatients.Count)
                {
                    ShiftPatient shiftPatient = lstShiftPatients[index++];
                    if (string.IsNullOrEmpty(shiftPatient.ShiftItemName))
                        continue;
                    if (shiftPatient.ShiftItemName.StartsWith(commonDictItem.ItemName))
                    {
                        index--;
                        lstShiftPatients.Remove(shiftPatient);
                        lstResultList.Add(shiftPatient);
                    }
                }
            }
            lstResultList.AddRange(lstShiftPatients);
            return lstResultList;
        }

        /// <summary>
        /// 对指定的交班病人列表按交班项目别名顺序进行排序
        /// </summary>
        /// <param name="lstShiftPatients">交班病人列表</param>
        /// <returns>有序的交班病人列表</returns>
        private List<ShiftPatient> GetOrderedShiftPatientsByAlias(List<ShiftPatient> lstShiftPatients)
        {
            if (lstShiftPatients == null || lstShiftPatients.Count <= 0)
                return lstShiftPatients;

            List<ShiftPatient> lstResultList = new List<ShiftPatient>();
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<ShiftItemAliasInfo> lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            short shRet = NurShiftService.Instance.GetShiftItemAliasInfos(szWardCode, ref lstShiftItemAliasInfos);
            foreach (ShiftItemAliasInfo shiftItemAliasInfo in lstShiftItemAliasInfos)
            {
                int index = 0;
                while (index < lstShiftPatients.Count)
                {
                    ShiftPatient shiftPatient = lstShiftPatients[index++];
                    if (string.IsNullOrEmpty(shiftPatient.ShiftItemName))
                        continue;
                    if (shiftPatient.ShiftItemAlias.StartsWith(shiftItemAliasInfo.ItemAlias))
                    {
                        index--;
                        lstShiftPatients.Remove(shiftPatient);
                        lstResultList.Add(shiftPatient);
                    }
                }
            }
            lstResultList.AddRange(lstShiftPatients);
            return lstResultList;
        }

        /// <summary>
        /// 获取班次下拉列表中当前选中的交班班次
        /// </summary>
        /// <returns>交班班次</returns>
        private ShiftRankInfo GetSelectedShiftRank()
        {
            ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            if (shiftRankInfo != null)
                shiftRankInfo.UpdateShiftRankTime(this.tooldtpShiftDate.Value);
            return shiftRankInfo;
        }

        /// <summary>
        /// 根据当前时间计算并返回交班班次
        /// </summary>
        /// <returns>交班班次</returns>
        private ShiftRankInfo GetCurrentShiftRank()
        {
            if (this.toolcboShiftRank.Items.Count <= 0)
                return null;
            DateTime dtShiftTime = SysTimeService.Instance.Now;
            foreach (object item in this.toolcboShiftRank.Items)
            {
                ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                shiftRankInfo.UpdateShiftRankTime(dtShiftTime.Date);
                if (dtShiftTime >= shiftRankInfo.StartTime && dtShiftTime < shiftRankInfo.EndTime)
                    return shiftRankInfo;
            }
            return this.toolcboShiftRank.Items[0] as ShiftRankInfo;
        }

        /// <summary>
        /// 显示指定交班记录对应的交班班次和日期
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        private void ShowShiftDateAndRank(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
                return;
            DateTime dtShiftDate = nursingShiftInfo.ShiftRecordDate.Date;
            if (this.tooldtpShiftDate.Value != dtShiftDate)
                this.tooldtpShiftDate.Value = dtShiftDate;

            ShiftRankInfo selectedRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            if (selectedRankInfo == null || selectedRankInfo.RankCode != nursingShiftInfo.ShiftRankCode)
            {
                foreach (object item in this.toolcboShiftRank.Items)
                {
                    ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                    if (shiftRankInfo.RankCode == nursingShiftInfo.ShiftRankCode)
                    {
                        this.toolcboShiftRank.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前交班病人表格中显示的交班病人数据
        /// </summary>
        /// <returns>交班病人数据</returns>
        private ShiftPatient[] GetShiftPatientTableData()
        {
            List<ShiftPatient> lstShiftPatients = new List<ShiftPatient>();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                ShiftPatient patient = row.Tag as ShiftPatient;
                if (patient != null)
                    lstShiftPatients.Add(patient);
            }
            return lstShiftPatients.ToArray();
        }

        /// <summary>
        /// 获取指定的交班病人对应的交班病人表格行
        /// </summary>
        /// <param name="shiftPatient">交班病人信息</param>
        /// <returns>交班病人表格行</returns>
        private DataTableViewRow GetShiftPatientTableRow(ShiftPatient shiftPatient)
        {
            if (shiftPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftPatient patient = currentRow.Tag as ShiftPatient;
            if (patient != null && patient.PatientID == shiftPatient.PatientID
                && patient.VisitID == shiftPatient.VisitID
                && patient.SubID == shiftPatient.SubID)
                return currentRow;

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftPatient;
                if (patient != null && patient.PatientID == shiftPatient.PatientID
                    && patient.VisitID == shiftPatient.VisitID
                    && patient.SubID == shiftPatient.SubID)
                    return row;
            }
            return null;
        }

        /// <summary>
        /// 获取指定的交班病人对应的交班病人表格行(包括交班项目)
        /// </summary>
        /// <param name="shiftPatient">交班病人信息</param>
        /// <returns>交班病人表格行</returns>
        private DataTableViewRow GetShiftPatItemTableRow(ShiftPatient shiftPatient)
        {
            if (shiftPatient == null || this.dgvShiftPatient.Rows.Count <= 0)
                return null;

            DataTableViewRow currentRow = this.dgvShiftPatient.CurrentRow;
            ShiftPatient patient = currentRow.Tag as ShiftPatient;
            if (patient != null && patient.PatientID == shiftPatient.PatientID
                && patient.VisitID == shiftPatient.VisitID
                && patient.SubID == shiftPatient.SubID
                )
            {
                if (patient.ShiftItemName != shiftPatient.ShiftItemName)
                {
                    currentRow.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                    this.dgvShiftPatient.SetRowState(currentRow, RowState.Update);                  
                }
                return currentRow;
            }

            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                patient = row.Tag as ShiftPatient;
                if (patient != null && patient.PatientID == shiftPatient.PatientID
                    && patient.VisitID == shiftPatient.VisitID
                    && patient.SubID == shiftPatient.SubID)
                {
                    if (patient.ShiftItemName != shiftPatient.ShiftItemName)
                    {
                        row.Cells[this.colShiftItem.Index].Value = shiftPatient.ShiftItemName;
                        this.dgvShiftPatient.SetRowState(row, RowState.Update);
                    }
                    return row;
                }
            }
            return null;
        }

        /// <summary>
        /// 创建一条新的护理交班主记录
        /// </summary>
        /// <returns>护理交班主记录</returns>
        private NursingShiftInfo CreateNursingShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string strShiftDate = string.Format("{0} {1}", this.tooldtpShiftDate.Value.ToShortDateString(),
                this.toollblRankTime.Text.Substring(0, this.toollblRankTime.Text.IndexOf(" ")));
            int iSRSelectedIndex = this.toolcboShiftRank.SelectedIndex;
            DateTime dtShiftDate = DateTime.Now;
            GlobalMethods.Convert.StringToDate(strShiftDate, ref dtShiftDate);
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return null;

            return NursingShiftHandler.Instance.Create(SystemContext.Instance.LoginUser, dtShiftDate, SysTimeService.Instance.Now, shiftRankInfo, iSRSelectedIndex);
        }

        /// <summary>
        /// 将编辑的交班内容更新到交班病人表格行中
        /// </summary>
        /// <returns>是否成功</returns>
        private bool UpdateSelectedShiftPatient()
        {
            if (!this.dgvShiftPatient.EndEdit())
                return false;
            if (!this.formControl2.EndEdit())
                return false;

            ShiftPatient shiftPatient = this.formControl2.Tag as ShiftPatient;
            if (shiftPatient == null)
                return false;

            DataTableViewRow row = this.GetShiftPatientTableRow(shiftPatient);
            if (row == null)
                return false;

            shiftPatient.ShiftItemName =
                row.Cells[this.colShiftItem.Index].Value as string;
            shiftPatient.ShiftItemAlias =
               row.Cells[this.colShiftItemAlias.Index].Value as string;

            if (this.formControl2.IsModified)
            {
                if (this.dgvShiftPatient.IsNormalRowUndeleted(row))
                    this.dgvShiftPatient.SetRowState(row, RowState.Update);
            }

            DataTable table = this.formControl2.GetFormData("交班信息") as DataTable;
            if (table == null || table.Rows.Count <= 0)
                return false;

            ShiftPatient shiftPatientModify = new ShiftPatient();
            shiftPatientModify.FromDataRow(table.Rows[0]);

            shiftPatient.BedCode = shiftPatientModify.BedCode;
            shiftPatient.Diagnosis = shiftPatientModify.Diagnosis;
            shiftPatient.ShowValue = shiftPatientModify.ShowValue;
            shiftPatient.SpecialItem1 = shiftPatientModify.SpecialItem1;
            shiftPatient.SpecialItem2 = shiftPatientModify.SpecialItem2;
            shiftPatient.SpecialItem3 = shiftPatientModify.SpecialItem3;
            shiftPatient.SpecialItem4 = shiftPatientModify.SpecialItem4;
            shiftPatient.SpecialItem5 = shiftPatientModify.SpecialItem5;
            shiftPatient.ShiftContent = shiftPatientModify.ShiftContent;
            shiftPatient.PulseValue = shiftPatientModify.PulseValue;
            shiftPatient.BreathValue = shiftPatientModify.BreathValue;
            shiftPatient.TemperatureType = shiftPatientModify.TemperatureType;
            shiftPatient.TemperatureValue = shiftPatientModify.TemperatureValue;
            shiftPatient.VitalTime = shiftPatientModify.VitalTime;
            shiftPatient.BloodPressure = shiftPatientModify.BloodPressure;
            shiftPatient.Diet = shiftPatientModify.Diet;
            shiftPatient.AdverseReaction = shiftPatientModify.AdverseReaction;
            shiftPatient.RequestDoctor = shiftPatientModify.RequestDoctor;
            shiftPatient.ParentDoctor = shiftPatientModify.ParentDoctor;
            return true;
        }

        /// <summary>
        /// 加载交班内容数据到交班内容编辑窗口中
        /// </summary>
        /// <returns>是否成功</returns>
        private bool ShowSelectedShiftPatient()
        {
            this.UpdateSelectedShiftPatient();

            if (this.dgvShiftPatient.CurrentRow == null)
            {
                this.formControl2.Tag = null;
                this.formControl2.UpdateFormData("刷新视图", null);
                this.formControl2.IsModified = false;
                return false;
            }

            ShiftPatient shiftPatient = this.dgvShiftPatient.CurrentRow.Tag as ShiftPatient;
            if (shiftPatient != null)
                shiftPatient.ShiftDate = this.tooldtpShiftDate.Value;
            this.formControl2.Tag = shiftPatient;
            DataTable table = GlobalMethods.Table.GetDataTable(shiftPatient);
            this.formControl2.UpdateFormData("刷新视图", table);

            object Visible = this.formControl2.GetFormData("交接班是否可编辑");
            if (Visible != null)
            {
                this.btnAddPatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.btnDeletePatient.Visible = (Visible is bool) ? (bool)Visible : false;
                this.dgvShiftPatient.ReadOnly = (Visible is bool) ? !(bool)Visible : true;
                this.formControl1.Enabled = (Visible is bool) ? (bool)Visible : false;
                this.toolbtnSave.Enabled = (Visible is bool) ? (bool)Visible : false;
            }

            this.formControl2.IsModified = false;
            return true;
        }

        /// <summary>
        /// 删除交班病人表中当前选中的交班病人
        /// </summary>
        /// <returns>是否成功</returns>
        private bool DeleteSelectedShiftPatient()
        {
            DataTableViewRow row = this.dgvShiftPatient.CurrentRow;
            if (row == null)
                return false;
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;
            DialogResult result = MessageBoxEx.ShowConfirm("确认删除当前选中的病人吗？");
            if (result != DialogResult.OK)
                return false;
            if (row.State == RowState.New)
            {
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedShiftPatient();
                return true;
            }
            ShiftPatient shiftPatient = row.Tag as ShiftPatient;
            if (shiftPatient == null)
                return false;
            string szRecordID = shiftPatient.ShiftRecordID;
            string szPatientID = shiftPatient.PatientID;
            string szVisitID = shiftPatient.VisitID;
            string szSubID = shiftPatient.SubID;

            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            short shRet = NurShiftService.Instance.DeleteShiftPatient(szRecordID
                , szPatientID, szVisitID, szSubID);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                this.m_bRecordUpdated = true;
                this.dgvShiftPatient.Rows.Remove(row);
                if (this.dgvShiftPatient.Rows.Count <= 0)
                    this.ShowSelectedShiftPatient();
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
        private bool DeleteAllShiftPatient()
        {
            this.dgvShiftPatient.EndEdit();
            if (this.dgvShiftPatient.Rows.Count <= 0)
                return false;
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
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
                    ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                    if (shiftPatient == null)
                        return false;
                    string szRecordID = shiftPatient.ShiftRecordID;
                    string szPatientID = shiftPatient.PatientID;
                    string szVisitID = shiftPatient.VisitID;
                    string szSubID = shiftPatient.SubID;

                    short shRet = NurShiftService.Instance.DeleteShiftPatient(szRecordID
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
            this.ShowSelectedShiftPatient();
            this.m_bRecordUpdated = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 获取用户当前选择的日期和班次对应的交班信息
        /// </summary>
        /// <returns>交班信息</returns>
        private NursingShiftInfo GetSelectedShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return null;
            string szRankCode = shiftRankInfo.RankCode;
            NursingShiftInfo nursingShiftInfo = null;
            short shRet = NurShiftService.Instance.GetNursingShiftInfo(szWardCode, dtShiftDate
                , szRankCode, ref nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK
                && shRet != ServerData.ExecuteResult.RES_NO_FOUND)
            {
                MessageBoxEx.Show("查询交班主记录失败!");
                return null;
            }
            return nursingShiftInfo;
        }

        /// <summary>
        /// 获取交班时病区动态信息列表
        /// </summary>
        /// <returns>病区动态信息列表</returns>
        public List<ShiftWardStatus> GetShiftWardStatusList()
        {
            if (!this.formControl1.EndEdit())
                return null;

            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            DataTable table = this.formControl1.GetFormData("病区动态") as DataTable;
            if (table == null)
                return lstShiftWardStatus;
            foreach (DataRow row in table.Rows)
            {
                ShiftWardStatus shiftWardStatus = new ShiftWardStatus();
                if (shiftWardStatus.FromDataRow(row))
                    lstShiftWardStatus.Add(shiftWardStatus);
            }
            return lstShiftWardStatus;
        }

        /// <summary>
        /// 保存当前已修改的病区动态及交班病人等交班数据
        /// </summary>
        /// <returns>是否成功</returns>
        private bool SaveNursingShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_nursingShiftInfo == null)
            {
                MessageBoxEx.Show("交班主记录信息为空,无法保存!");
                return false;
            }
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;

            short shRet = SystemConst.ReturnValue.OK;

            UserInfo userInfo = SystemContext.Instance.LoginUser;
            this.m_nursingShiftInfo.ModifierID = userInfo.ID;
            this.m_nursingShiftInfo.ModifierName = userInfo.Name;
            this.m_nursingShiftInfo.ModifyTime = SysTimeService.Instance.Now;

            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo != null)
                this.m_nursingShiftInfo.ShiftRankCode = shiftRankInfo.RankCode;

            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            this.m_nursingShiftInfo.ShiftRecordDate = dtShiftDate;

            //检查该班次交班主索引是否重复
            NursingShiftInfo nursingShiftInfoDB = null;
            shRet = NurShiftService.Instance.GetNursingShiftInfo(
               this.m_nursingShiftInfo.WardCode,
               this.m_nursingShiftInfo.ShiftRecordDate,
               this.m_nursingShiftInfo.ShiftRankCode,
               ref nursingShiftInfoDB);
            if (shRet != ServerData.ExecuteResult.RES_NO_FOUND
                && nursingShiftInfoDB.ShiftRecordID != this.m_nursingShiftInfo.ShiftRecordID)
            {
                MessageBoxEx.Show("发生错误：数据库中已有该班次数据，无法新增，请重新打开页面。");
                this.m_bRecordUpdated = true;//使交接班主界面刷新数据。
                //屏蔽是否保存提示框
                this.formControl1.IsModified = false;
                this.formControl2.IsModified = false;
                this.Close();
            }

            //保存交班病人列表
            this.UpdateSelectedShiftPatient();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (this.dgvShiftPatient.IsNormalRow(row))
                    continue;
                ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                if (shiftPatient == null)
                    continue;
                shiftPatient.PatientNo = row.Index;
                shiftPatient.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                if (shiftPatient.SpecialItem2 == null || shiftPatient.SpecialItem2.Equals(""))
                    shiftPatient.SpecialItem2 = "0";
                shRet = NurShiftService.Instance.SaveShiftPatient(shiftPatient);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班病人信息失败!");
                    return false;
                }
                this.dgvShiftPatient.SetRowState(row, RowState.Normal);
            }
            this.formControl2.IsModified = false;

            //保存交班病区动态
            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            if (this.formControl1.IsModified || this.m_bIsNewNursingShift)
                lstShiftWardStatus = this.GetShiftWardStatusList();
            foreach (ShiftWardStatus shiftWardStatus in lstShiftWardStatus)
            {
                shiftWardStatus.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftWardStatus(shiftWardStatus);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班病区动态失败!");
                    return false;
                }
            }
            this.formControl1.IsModified = false;

            //保存交班主索引记录
            shRet = NurShiftService.Instance.SaveNursingShiftInfo(this.m_nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("保存交班主记录信息失败!");
                return false;
            }

            this.m_bRecordUpdated = true;
            return true;
        }

        /// <summary>
        /// 保存this.toolcboShiftRank.SelectedIndexChanged/tooldtpShiftDate_ValueChanged前已修改的病区动态及交班病人等交班数据
        /// </summary>
        /// <returns>是否成功</returns>
        private bool SaveLastNursingShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_nursingShiftInfo == null)
            {
                MessageBoxEx.Show("交班主记录信息为空,无法保存!");
                return false;
            }
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;

            short shRet = SystemConst.ReturnValue.OK;

            UserInfo userInfo = SystemContext.Instance.LoginUser;
            this.m_nursingShiftInfo.ModifierID = userInfo.ID;
            this.m_nursingShiftInfo.ModifierName = userInfo.Name;
            this.m_nursingShiftInfo.ModifyTime = SysTimeService.Instance.Now;


            //检查该班次交班主索引是否重复
            NursingShiftInfo nursingShiftInfoDB = null;
            shRet = NurShiftService.Instance.GetNursingShiftInfo(
               this.m_nursingShiftInfo.WardCode,
               this.m_nursingShiftInfo.ShiftRecordDate,
               this.m_nursingShiftInfo.ShiftRankCode,
               ref nursingShiftInfoDB);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
            {
                //保存交班主索引记录
                shRet = NurShiftService.Instance.SaveNursingShiftInfo(this.m_nursingShiftInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班主记录信息失败!");
                    return false;
                }
                this.m_bRecordUpdated = true;
            }

            //保存交班病人列表
            this.UpdateSelectedShiftPatient();
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (this.dgvShiftPatient.IsNormalRow(row))
                    continue;
                ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                if (shiftPatient == null)
                    continue;
                shiftPatient.PatientNo = row.Index;
                shiftPatient.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftPatient(shiftPatient);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班病人信息失败!");
                    return false;
                }
                this.dgvShiftPatient.SetRowState(row, RowState.Normal);
            }
            this.formControl2.IsModified = false;

            //保存交班病区动态
            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            if (this.formControl1.IsModified || this.m_bIsNewNursingShift)
                lstShiftWardStatus = this.GetShiftWardStatusList();
            foreach (ShiftWardStatus shiftWardStatus in lstShiftWardStatus)
            {
                shiftWardStatus.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftWardStatus(shiftWardStatus);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("保存交班病区动态失败!");
                    return false;
                }
            }
            this.formControl1.IsModified = false;

            return true;
        }

        private void tooldtpShiftDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;

            this.dgvShiftPatient.EndEdit();
            this.CheckLastModifyState();
            this.formControl1.IsModified = false;
            this.formControl2.IsModified = false;
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
                row.State = RowState.Normal;

            this.EditState(this.GetSelectedShiftInfo());
            this.LoadNursingShiftData();
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboShiftRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return;
            this.toollblRankTime.Text = string.Format("{0} - {1}"
                , shiftRankInfo.StartPoint, shiftRankInfo.EndPoint);

            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;

            this.dgvShiftPatient.EndEdit();
            this.CheckLastModifyState();
            this.formControl1.IsModified = false;
            this.formControl2.IsModified = false;
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
                row.State = RowState.Normal;

            this.EditState(this.GetSelectedShiftInfo());
            this.LoadNursingShiftData();
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveNursingShiftData())
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

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            ShiftRecFilterForm shiftRecFilterForm = new ShiftRecFilterForm();
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            shiftRecFilterForm.ShiftRankInfo = shiftRankInfo;
            shiftRecFilterForm.IsbtnAddPatientClick = true;
            shiftRecFilterForm.SelectedPatients = this.GetShiftPatientTableData();
            if (shiftRecFilterForm.ShowDialog() == DialogResult.OK)
                this.LoadShiftPatientList(shiftRecFilterForm.SelectedPatients, true);
            //this.dgvShiftPatient.Sort(new BedCodeComparer());
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnDeletePatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnDeleteAllPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteAllShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dgvShiftPatient_SelectionChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowSelectedShiftPatient();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void FormControl_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            if (e.Name == "是新交班")
            {
                e.Value = this.m_bIsNewNursingShift;
                e.Success = true;
            }
            else if (e.Name == "交班日期")
            {
                e.Value = this.tooldtpShiftDate.Value;
                e.Success = true;
            }
            else if (e.Name == "交班ID号")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.ShiftRecordID;
                e.Success = true;
            }
            else if (e.Name == "交班班次")
            {
                ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
                if (shiftRankInfo != null)
                    e.Value = shiftRankInfo.ToDataTable();
                e.Success = true;
            }
            else if (e.Name == "创建者ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.CreatorID;
                e.Success = true;
            }
            else if (e.Name == "创建者姓名")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.CreatorName;
                e.Success = true;
            }
            else if (e.Name == "交班病区名")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.WardName;
                e.Success = true;
            }
            else if (e.Name == "交班病区ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.WardCode;
                e.Success = true;
            }
            else if (e.Name == "修改者ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.ModifierID;
                e.Success = true;
            }
            else if (e.Name == "修改者姓名")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.ModifierName;
                e.Success = true;
            }
            else if (e.Name == "第一签名人ID")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.FirstSignID;
                e.Success = true;
            }
            else if (e.Name == "第一签名人ID姓名")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.FirstSignName;
                e.Success = true;
            }
            else if (e.Name == "第一签名时间")
            {
                if (this.m_nursingShiftInfo != null)
                    e.Value = this.m_nursingShiftInfo.FirstSignTime;
                e.Success = true;
            }
            if (e.Name == "前一班次代码")
            {
                if (this.toolcboShiftRank.SelectedIndex == 0)
                {
                    ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.Items[2] as ShiftRankInfo;
                    if (shiftRankInfo != null)
                        shiftRankInfo.StartTime = this.tooldtpShiftDate.Value.AddDays(-1);

                    e.Value = GlobalMethods.Table.GetDataTable(shiftRankInfo);
                }
                else
                {
                    ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.Items[this.toolcboShiftRank.SelectedIndex - 1] as ShiftRankInfo;
                    if (shiftRankInfo != null)
                        shiftRankInfo.StartTime = this.tooldtpShiftDate.Value;

                    e.Value = GlobalMethods.Table.GetDataTable(shiftRankInfo);
                }
            }
        }

        private void ShiftRecEditForm_Load(object sender, EventArgs e)
        {
            string szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_Split2_Distance, string.Empty);
            int iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.splitContainer2.SplitterDistance = iDistance;
            else
                this.splitContainer2.SplitterDistance = 290;
            szValue = string.Empty;

            szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_Item_Width, string.Empty);
            iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.colShiftItem.Width = iDistance;
            else
                this.colShiftItem.Width = 60;
            szValue = string.Empty;

            szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_BedCode_Width, string.Empty);
            iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.colBedCode.Width = iDistance;
            else
                this.colBedCode.Width = 40;
            szValue = string.Empty;

            szValue = SystemConfig.Instance.Get(ShiftRecEditForm.NDS_Shift_Edit_Name_Width, string.Empty);
            iDistance = 0;
            if (int.TryParse(szValue, out iDistance))
                this.colPatientName.Width = iDistance;
            else
                this.colPatientName.Width = 80;
            szValue = string.Empty;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            bool bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_Split2_Distance
                , this.splitContainer2.SplitterDistance.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("界面布局写入失败!");
                return;
            }

            bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_Item_Width
                , this.colShiftItem.Width.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("界面布局交班项目写入失败!");
                return;
            }

            bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_BedCode_Width
                , this.colBedCode.Width.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("界面布局床号写入失败!");
                return;
            }

            bSuccess = SystemConfig.Instance.Write(
                ShiftRecEditForm.NDS_Shift_Edit_Name_Width
                , this.colPatientName.Width.ToString());
            if (!bSuccess)
            {
                MessageBox.Show("界面布局姓名写入失败!");
                return;
            }
        }

        private void dgvShiftPatient_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dgvShiftPatient.CurrentCell.OwningColumn.Name == "colShiftItemAlias")
            {
                ComboBox comboBox = (ComboBox)e.Control;
                comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (this.dgvShiftPatient.CurrentCell.OwningColumn.Name == "colShiftItemAlias" && comboBox.SelectedIndex != 0)
            {
                this.dgvShiftPatient.Rows[this.dgvShiftPatient.CurrentRow.Index].Cells[this.colShiftItem.Index].Value = this.m_comboBox.Items[comboBox.SelectedIndex];
            }
        }

        private void toolbtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBoxEx.ShowQuestion("是否删除当前班次所有交班信息");
            if (result != DialogResult.Yes)
                return;

            short shRet = SystemConst.ReturnValue.OK;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            //获取当前班次索引信息
            shRet = NurShiftService.Instance.GetNursingShiftInfo(szWardCode, dtShiftDate, shiftRankInfo.RankCode, ref nursingShiftInfo);
            if (shRet == SystemConst.ReturnValue.NO_FOUND)
            {
                MessageBoxEx.Show("当前班次不存在交班信息!");
                return;
            }
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("获取当前班次交班索引信息失败!");
                return;
            }

            //删除交班病人信息
            string szRecordID = "";
            string szPatientID = "";
            string szVisitID = "";
            string szSubID = "";
            foreach (DataTableViewRow row in this.dgvShiftPatient.Rows)
            {
                if (row.State != RowState.New)
                {
                    ShiftPatient shiftPatient = row.Tag as ShiftPatient;
                    if (shiftPatient == null)
                        continue;
                    szRecordID = shiftPatient.ShiftRecordID;
                    szPatientID = shiftPatient.PatientID;
                    szVisitID = shiftPatient.VisitID;
                    szSubID = shiftPatient.SubID;
                    this.Update();
                    GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                    shRet = NurShiftService.Instance.DeleteShiftPatient(szRecordID
                        , szPatientID, szVisitID, szSubID);
                    if (shRet != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.Show("删除当前班次交班病人记录失败!");
                        GlobalMethods.UI.SetCursor(this, Cursors.Default);
                        return;
                    }
                }
            }

            szRecordID = nursingShiftInfo.ShiftRecordID;
            if (!string.IsNullOrEmpty(szRecordID))
            {
                //删除病区动态信息
                shRet = NurShiftService.Instance.DeleteShiftWardStatusInfo(szRecordID);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("删除当前班次交班动态信息失败!");
                    return;
                }

                //删除交班索引信息
                shRet = NurShiftService.Instance.DeleteShiftIndexInfo(szRecordID, szWardCode);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("删除当前班次交班病人索引信息失败!");
                    return;
                }
            }

            MessageBox.Show("删除当前班次所有交班信息成功！");
            this.Close();
            this.m_bRecordUpdated = true;
        }
    }
}