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
    internal partial class ShiftStatusEditForm : HerenForm
    {
        private NursingShiftInfo m_nursingShiftInfo = null;

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


        private string m_szShiftStatusName = "";

        public string ShiftStatusName
        {
            get { return this.m_szShiftStatusName; }
            set { this.m_szShiftStatusName = value; }
        }

        public ShiftStatusEditForm()
        {
            this.InitializeComponent();
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

            this.Update();

            //加载交班班次下拉列表
            this.LoadShiftRankList();

            //加载病区动态编辑表单
            this.Update();
            this.LoadWardStatusTemplet();

            //加载当前传入的交班记录
            if (this.m_nursingShiftInfo == null)
                this.m_nursingShiftInfo = this.GetSelectedShiftInfo();
            this.LoadNursingShiftData(this.m_nursingShiftInfo);

            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
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
            string szTempletName = this.m_szShiftStatusName;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError(string.Format("表单{{0}}编辑表单还没有制作!", szTempletName));
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError(string.Format("表单{{0}}编辑表单下载失败!", szTempletName));
                return false;
            }
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// 根据当前交班记录对象信息加载当次交班数据
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        /// <returns>是否成功</returns>
        private bool LoadNursingShiftData(NursingShiftInfo nursingShiftInfo)
        {
            if (this.CheckModifyState())
            {
                return false;
            }
            if (nursingShiftInfo == null)
            {
                this.Text = string.Format("编辑{0}(新增)", this.ShiftStatusName);
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
                this.Text = string.Format("编辑{0}(修改)", this.ShiftStatusName);
                this.m_bIsNewNursingShift = false;
                this.m_nursingShiftInfo = nursingShiftInfo;
            }
            this.ShowShiftDateAndRank(this.m_nursingShiftInfo);

            if (!this.m_bIsNewNursingShift)
            {
                this.LoadWardStatusData(this.m_nursingShiftInfo);
            }
            else
            {
                this.LoadWardStatusData(null);
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
                this.formControl1.UpdateFormData("加载动态信息", null);
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
            this.formControl1.UpdateFormData("加载动态信息", lstShiftWardStatus);
            if (!this.m_bIsNewNursingShift)
                this.formControl1.IsModified = false;
            return true;
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
        /// 创建一条新的护理交班主记录
        /// </summary>
        /// <returns>护理交班主记录</returns>
        private NursingShiftInfo CreateNursingShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            int iSRSelectedIndex = this.toolcboShiftRank.SelectedIndex;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value;
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return null;
            return NursingShiftHandler.Instance.Create(SystemContext.Instance.LoginUser, dtShiftDate, SysTimeService.Instance.Now, shiftRankInfo, iSRSelectedIndex);
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
                this.Close();
            }

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
        /// 获取交班时病区动态信息列表
        /// </summary>
        /// <returns>病区动态信息列表</returns>
        public List<ShiftWardStatus> GetShiftWardStatusList()
        {
            if (!this.formControl1.EndEdit())
                return null;

            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            DataTable table = this.formControl1.GetFormData("获取动态信息") as DataTable;
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

        private void tooldtpShiftDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;
            this.LoadNursingShiftData(this.GetSelectedShiftInfo());
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
            this.LoadNursingShiftData(this.GetSelectedShiftInfo());
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
    }
}