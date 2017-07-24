// ***********************************************************
// 护理电子病历系统,护理记录单护理记录小结编辑窗口.
// Creator:YangMingkun  Date:2012-10-21
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.Forms.Editor;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;

namespace Heren.NurDoc.PatPage.NurRec
{
    internal partial class NurRecSummaryForm : HerenForm
    {
        private List<VitalSignsData> m_vitalSignsData = null;

        /// <summary>
        /// 获取当前小结记录中需要插入的出入量时间
        /// </summary>
        [Browsable(false)]
        public List<VitalSignsData> VitalSignsDate
        {
            get { return m_vitalSignsData; }
        }

        private NursingRecInfo m_nursingRecInfo = null;

        /// <summary>
        /// 获取当前护理记录编辑器绑定的护理记录信息
        /// </summary>
        [Browsable(false)]
        public NursingRecInfo NursingRecInfo
        {
            get { return this.m_nursingRecInfo; }
        }

        private GridViewSchema m_currentSchema = null;

        /// <summary>
        /// 获取或设置当前用户选择的护理记录单类型
        /// </summary>
        [Browsable(false)]
        public GridViewSchema CurrentSchema
        {
            get { return this.m_currentSchema; }
            set { this.m_currentSchema = value; }
        }

        /// <summary>
        /// 该变量用于指示当前护理记录是否是新建状态
        /// </summary>
        private bool m_bIsNewRecord = false;

        /// <summary>
        /// 该变量用于指示某些控件值改变事件是否可用
        /// </summary>
        private bool m_bControlValueChangedEventEnabled = true;

        public NurRecSummaryForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.FormEdit;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            //如果白板脚本里有配置,那么使用配置
            string[] names = SystemContext.Instance.SummaryNameList;
            if (names != null && names.Length > 0)
            {
                this.cboSummaryName.Items.Clear();
                this.cboSummaryName.Items.AddRange(names);
            }

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_nursingRecInfo != null)
            {
                this.m_bIsNewRecord = false;
            }
            else
            {
                this.m_bIsNewRecord = true;
                this.m_nursingRecInfo = this.CreateNursingRecord();
            }
            this.LoadNursingRecord();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public DialogResult ShowDialog(NursingRecInfo nursingRecInfo)
        {
            this.m_nursingRecInfo = nursingRecInfo;
            return base.ShowDialog();
        }

        /// <summary>
        /// 创建一条新的护理记录信息
        /// </summary>
        /// <returns>护理记录信息</returns>
        private NursingRecInfo CreateNursingRecord()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return null;
            if (SystemContext.Instance.LoginUser == null)
                return null;
            NursingRecInfo nursingRecInfo = new NursingRecInfo();
            nursingRecInfo.SummaryFlag = 1;
            nursingRecInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            nursingRecInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            nursingRecInfo.CreateTime = SysTimeService.Instance.Now;
            nursingRecInfo.ModifierID = nursingRecInfo.CreatorID;
            nursingRecInfo.ModifierName = nursingRecInfo.CreatorName;
            nursingRecInfo.ModifyTime = nursingRecInfo.CreateTime;
            nursingRecInfo.PatientID = PatientTable.Instance.ActivePatient.PatientID;
            nursingRecInfo.VisitID = PatientTable.Instance.ActivePatient.VisitID;
            nursingRecInfo.SubID = PatientTable.Instance.ActivePatient.SubID;
            nursingRecInfo.Recorder1Name = nursingRecInfo.CreatorName;

            nursingRecInfo.RecordTime = SysTimeService.Instance.Now;

            nursingRecInfo.RecordDate = nursingRecInfo.RecordTime.Date;
            nursingRecInfo.RecordTime = new DateTime(nursingRecInfo.RecordTime.Year
                , nursingRecInfo.RecordTime.Month, nursingRecInfo.RecordTime.Day
                , nursingRecInfo.RecordTime.Hour, nursingRecInfo.RecordTime.Minute, 0);

            nursingRecInfo.SummaryName = "24小时总结";
            nursingRecInfo.SummaryStartTime = nursingRecInfo.RecordTime.AddHours(-24);

            nursingRecInfo.RecordID = nursingRecInfo.MakeRecordID();
            nursingRecInfo.PatientName = PatientTable.Instance.ActivePatient.PatientName;
            nursingRecInfo.WardCode = PatientTable.Instance.ActivePatient.WardCode;
            nursingRecInfo.WardName = PatientTable.Instance.ActivePatient.WardName;
            return nursingRecInfo;
        }
        //设置需要保存的出入量记录时间数据
        private List<VitalSignsData> CreateVitalSignsData()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return null;
            if (SystemContext.Instance.LoginUser == null)
                return null;

            int index = this.cboSummaryName.Text.IndexOf("小时");
            int num = 0;
            num = GlobalMethods.Convert.StringToValue(this.cboSummaryName.Text.Substring(0, index), 0);
            List<VitalSignsData> lstVitalSigns = new List<VitalSignsData>();

            VitalSignsData vitalSigns = new VitalSignsData();
            vitalSigns.PatientID = PatientTable.Instance.ActivePatient.PatientID;
            vitalSigns.VisitID = PatientTable.Instance.ActivePatient.VisitID;
            vitalSigns.SubID = PatientTable.Instance.ActivePatient.SubID;
            vitalSigns.RecordTime = this.dtpRecordTime.Value.Value;
            vitalSigns.RecordDate = vitalSigns.RecordTime.Date;
            vitalSigns.DataType = string.Empty;
            vitalSigns.DataUnit = "小时";
            vitalSigns.Category = 1;
            vitalSigns.ContainsTime = false;
            vitalSigns.Remarks = string.Empty;
            vitalSigns.SourceTag = string.Empty;
            vitalSigns.SourceType = string.Empty;
            vitalSigns.RecordData = this.cboSummaryName.Text.Substring(0, index);

            if (this.dataTableView1.Rows.Count > 0)
            {
                vitalSigns.RecordName = "入量累计时间";
                lstVitalSigns.Add(vitalSigns);
            }
            if (this.dataTableView2.Rows.Count > 0)
            {
                vitalSigns = vitalSigns.Clone() as VitalSignsData;
                vitalSigns.RecordName = "出量累计时间";
                lstVitalSigns.Add(vitalSigns);

                for (int i = 0; i < this.dataTableView2.Rows.Count; i++)
                {
                    if (this.dataTableView2.Rows[i].Cells[1].Value.ToString().Contains("尿量"))
                    {
                        vitalSigns = vitalSigns.Clone() as VitalSignsData;
                        vitalSigns.RecordName = "尿量累计时间";
                        lstVitalSigns.Add(vitalSigns);
                        break;
                    }
                }
            }
            return lstVitalSigns;
        }

        /// <summary>
        /// 加载指定护理记录对应的护理评估单列表
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadNursingRecord()
        {
            this.dataTableView1.Rows.Clear();
            this.dataTableView2.Rows.Clear();
            this.lblEnterTotal.Text = "入量总计：";
            this.lblOutTotal.Text = "出量总计：";
            this.txtSummaryDesc.Text = string.Empty;

            Application.DoEvents();
            if (this.m_nursingRecInfo == null)
                return false;

            this.m_bControlValueChangedEventEnabled = false;
            this.cboSummaryName.Text = this.m_nursingRecInfo.SummaryName;
            this.dtpRecordTime.Value = this.m_nursingRecInfo.RecordTime;
            this.dtpStartTime.Value = this.m_nursingRecInfo.SummaryStartTime;
            this.txtSummaryDesc.Text = this.m_nursingRecInfo.RecordContent;
            this.m_bControlValueChangedEventEnabled = true;

            this.LoadEnterOutData();
            return true;
        }

        private bool LoadEnterOutData()
        {
            this.dataTableView1.Rows.Clear();
            this.dataTableView2.Rows.Clear();
            this.lblEnterTotal.Tag = string.Empty;
            this.lblOutTotal.Tag = string.Empty;
            this.lblEnterTotal.Text = "入量总计：";
            this.lblOutTotal.Text = "出量总计：";
            if (this.m_nursingRecInfo == null)
                return false;
            if (!this.dtpStartTime.Value.HasValue || !this.dtpRecordTime.Value.HasValue)
                return false;
            string szPatientID = this.m_nursingRecInfo.PatientID;
            string szVisitID = this.m_nursingRecInfo.VisitID;
            DateTime dtBeginTime = this.dtpStartTime.Value.Value;
            DateTime dtEndTime = this.dtpRecordTime.Value.Value;
            List<VitalSignsData> lstVitalSignsData = null;
            short shRet = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, "0"
                , dtBeginTime, dtEndTime, ref lstVitalSignsData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("下载病人体征数据失败!");
                return false;
            }
            if (lstVitalSignsData == null || lstVitalSignsData.Count <= 0)
                return true;

            float nEnterTotal = 0f, nOutTotal = 0f;
            foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
            {
                if (vitalSignsData.Remarks == "入量")
                {
                    if (SystemContext.Instance.SystemOption.OptionCalculateSummary == 0
                        && vitalSignsData.RecordTime != dtBeginTime)
                    {
                        nEnterTotal += GlobalMethods.Convert.StringToValue(vitalSignsData.RecordData, 0f);
                        this.dataTableView1.Rows.Add(vitalSignsData.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")
                            , vitalSignsData.RecordName, vitalSignsData.RecordData);
                    }
                    else if (SystemContext.Instance.SystemOption.OptionCalculateSummary == 1
                        && vitalSignsData.RecordTime != dtEndTime)
                    {
                        nEnterTotal += GlobalMethods.Convert.StringToValue(vitalSignsData.RecordData, 0f);
                        this.dataTableView1.Rows.Add(vitalSignsData.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")
                            , vitalSignsData.RecordName, vitalSignsData.RecordData);
                    }
                    else if (SystemContext.Instance.SystemOption.OptionCalculateSummary == 2
                        && vitalSignsData.RecordTime != dtEndTime)
                    {
                        nEnterTotal += GlobalMethods.Convert.StringToValue(vitalSignsData.RecordData, 0f);
                        this.dataTableView1.Rows.Add(vitalSignsData.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")
                            , vitalSignsData.RecordName, vitalSignsData.RecordData);
                    }
                }
                else if (vitalSignsData.Remarks == "出量" || vitalSignsData.RecordName == "尿量")
                {
                    if (SystemContext.Instance.SystemOption.OptionCalculateSummary == 0
                        && vitalSignsData.RecordTime != dtBeginTime)
                    {
                        nOutTotal += GlobalMethods.Convert.StringToValue(vitalSignsData.RecordData, 0f);
                        this.dataTableView2.Rows.Add(vitalSignsData.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")
                            , vitalSignsData.RecordName, vitalSignsData.RecordData);
                    }
                    else if (SystemContext.Instance.SystemOption.OptionCalculateSummary == 1
                        && vitalSignsData.RecordTime != dtEndTime)
                    {
                        nOutTotal += GlobalMethods.Convert.StringToValue(vitalSignsData.RecordData, 0f);
                        this.dataTableView2.Rows.Add(vitalSignsData.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")
                            , vitalSignsData.RecordName, vitalSignsData.RecordData);
                    }
                    else if (SystemContext.Instance.SystemOption.OptionCalculateSummary == 2
                        && vitalSignsData.RecordTime != dtBeginTime)
                    {
                        nOutTotal += GlobalMethods.Convert.StringToValue(vitalSignsData.RecordData, 0f);
                        this.dataTableView2.Rows.Add(vitalSignsData.RecordTime.ToString("yyyy-MM-dd HH:mm:ss")
                            , vitalSignsData.RecordName, vitalSignsData.RecordData);
                    }
                }
            }
            if (this.dataTableView1.Rows.Count <= 0)
                this.lblEnterTotal.Tag = string.Empty;
            else
                this.lblEnterTotal.Tag = nEnterTotal.ToString();

            if (this.dataTableView2.Rows.Count <= 0)
                this.lblOutTotal.Tag = string.Empty;
            else
                this.lblOutTotal.Tag = nOutTotal.ToString();

            this.lblEnterTotal.Text = "入量总计：" + this.lblEnterTotal.Tag.ToString();
            this.lblOutTotal.Text = "出量总计：" + this.lblOutTotal.Tag.ToString();

            if (this.dataTableView1.Rows.Count > 0)
                this.dataTableView1.CurrentCell = this.dataTableView1.Rows[0].Cells[this.colEnterName.Index];
            if (this.dataTableView2.Rows.Count > 0)
                this.dataTableView2.CurrentCell = this.dataTableView2.Rows[0].Cells[this.colOutName.Index];
            return true;
        }

        /// <summary>
        /// 保存当前修改的护理记录记录本身,不包含评估数据
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool SaveNursingRecord()
        {
            Application.DoEvents();
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_nursingRecInfo == null)
                return false;

            if (!this.m_bIsNewRecord)
            {
                if (!RightController.Instance.CanEditNurRec(this.m_nursingRecInfo))
                    return false;
            }

            if (GlobalMethods.Misc.IsEmptyString(this.cboSummaryName.Text))
            {
                MessageBoxEx.ShowMessage("您没有输入小结名称!");
                return false;
            }
            if (!this.dtpStartTime.Value.HasValue)
            {
                MessageBoxEx.ShowMessage("您没有选择统计起始时间!");
                return false;
            }
            if (!this.dtpRecordTime.Value.HasValue)
            {
                MessageBoxEx.ShowMessage("您没有选择记录时间!");
                return false;
            }
            this.m_nursingRecInfo.SummaryName = this.cboSummaryName.Text;
            this.m_nursingRecInfo.RecordTime = this.dtpRecordTime.Value.Value;
            this.m_nursingRecInfo.RecordDate = this.m_nursingRecInfo.RecordTime.Date;
            this.m_nursingRecInfo.SummaryStartTime = this.dtpStartTime.Value.Value;
            this.m_nursingRecInfo.RecordContent = this.txtSummaryDesc.Text;
            this.m_nursingRecInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            this.m_nursingRecInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            this.m_nursingRecInfo.ModifyTime = SysTimeService.Instance.Now;

            string szEnterTotal = string.Empty;
            string sznOutTotal = string.Empty;
            if (this.lblEnterTotal.Tag.ToString() != string.Empty)
                szEnterTotal = this.lblEnterTotal.Tag.ToString();
            if (this.lblOutTotal.Tag.ToString() != string.Empty)
                sznOutTotal = this.lblOutTotal.Tag.ToString();
            this.m_nursingRecInfo.RecordRemarks = string.Concat(szEnterTotal, "$", sznOutTotal);

            //获取需要保存的出入量时间数据   xpp

            if (this.cboSummaryName.Text.Contains("小时小结") || this.cboSummaryName.Text.Contains("小时总结"))
            {
                int index = this.cboSummaryName.Text.IndexOf("小时");
                if (index < 0)
                {
                    MessageBoxEx.ShowError("小结名称错误");
                    return false;
                }
                int num = 0;
                num = GlobalMethods.Convert.StringToValue(this.cboSummaryName.Text.Substring(0, index), 0);
                if (num < 24)
                {
                    this.m_vitalSignsData = this.CreateVitalSignsData();
                    short shVital = SystemConst.ReturnValue.OK;
                    shVital = VitalSignsService.Instance.SaveVitalSignsData(this.m_vitalSignsData);
                    if (shVital != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.ShowError("保存出入量累计时间失败!");
                        return false;
                    }
                }
            }

            short shRet = SystemConst.ReturnValue.OK;
            if (this.m_bIsNewRecord)
            {
                this.m_nursingRecInfo.SchemaID = this.CurrentSchema.SchemaID;
                shRet = NurRecService.Instance.SaveNursingRec(this.m_nursingRecInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("保存护理记录失败!");
                    return false;
                }
                this.m_bIsNewRecord = false;
            }
            else
            {
                shRet = NurRecService.Instance.UpdateNursingRec(this.m_nursingRecInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("更新护理记录失败!");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 根据用户选择的统计起始时间和记录时间自动计算小结名称
        /// </summary>
        private void UpdateSummaryNameByTime()
        {
            if (this.dtpRecordTime.Value == null || this.dtpStartTime.Value == null)
                return;

            //仅当小结名字以小时数开头的时候,自动更新小结名字
            string szSummaryName = this.cboSummaryName.Text;
            szSummaryName = GlobalMethods.Convert.SBCToDBC(szSummaryName);

            if (szSummaryName.Length <= 0
                || szSummaryName == "小时小结"
                || szSummaryName == "小时总结"
                || (szSummaryName[0] >= '0' && szSummaryName[0] <= '9'))
            {
                long hours = GlobalMethods.SysTime.DateDiff(DateInterval.Hour
                    , this.dtpStartTime.Value.Value, this.dtpRecordTime.Value.Value);
                if (hours <= 0)
                    return;
                else if (hours <= 12)
                    this.cboSummaryName.Text = string.Concat(hours, "小时小结");
                else
                    this.cboSummaryName.Text = string.Concat(hours, "小时总结");
            }
        }

        /// <summary>
        /// 根据用户输入的小结名字自动计算并更新统计起始时间
        /// </summary>
        private void UpdateSummaryStartTimeByName()
        {
            if (!this.dtpRecordTime.Value.HasValue)
                return;
            if (this.cboSummaryName.Text == null)
                return;
            StringBuilder sbHoursText = new StringBuilder();
            foreach (char ch in this.cboSummaryName.Text)
            {
                if (ch >= '0' && ch <= '9')
                    sbHoursText.Append(ch);
            }
            if (sbHoursText.Length <= 0)
                return;
            int hours = 24;
            hours = GlobalMethods.Convert.StringToValue(sbHoursText.ToString(), hours);
            this.dtpStartTime.Value = this.dtpRecordTime.Value.Value.AddHours(0 - hours);
        }

        private void cboSummaryName_KeyDown(object sender, KeyEventArgs e)
        {
            //修改小结名字后,自动调整时间
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bControlValueChangedEventEnabled = false;
            this.UpdateSummaryStartTimeByName();

            Application.DoEvents();
            this.LoadEnterOutData();
            this.m_bControlValueChangedEventEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void cboSummaryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_bControlValueChangedEventEnabled)
                return;
            //修改小结名字后,自动调整时间
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bControlValueChangedEventEnabled = false;
            this.UpdateSummaryStartTimeByName();

            Application.DoEvents();
            this.LoadEnterOutData();
            this.m_bControlValueChangedEventEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dtpRecordTime_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bControlValueChangedEventEnabled)
                return;
            //修改时间后,自动调整小结名字
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.UpdateSummaryNameByTime();

            Application.DoEvents();
            this.LoadEnterOutData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dtpStartTime_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bControlValueChangedEventEnabled)
                return;
            //修改时间后,自动调整小结名字
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.UpdateSummaryNameByTime();

            Application.DoEvents();
            this.LoadEnterOutData();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveNursingRecord())
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnTextTemplet_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.txtSummaryDesc.SelectedText = UtilitiesHandler.Instance.ShowTextTempletForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataTableViewRow currentRow = this.dataTableView1.Rows[e.RowIndex];
            if (currentRow.Cells[this.colEnterName.Index].Value == null)
                return;
            string name = currentRow.Cells[this.colEnterName.Index].Value.ToString();
            float total = 0f;
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                if (name.Equals(row.Cells[this.colEnterName.Index].Value))
                {
                    object value = row.Cells[this.colEnterValue.Index].Value;
                    if (value != null)
                        total += GlobalMethods.Convert.StringToValue(value.ToString(), 0f);
                }
            }
            this.txtSummaryDesc.Focus();
            this.txtSummaryDesc.SelectedText = string.Concat(name, "：", total, "；");
        }

        private void dataTableView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataTableViewRow currentRow = this.dataTableView2.Rows[e.RowIndex];
            object name = currentRow.Cells[this.colOutName.Index].Value;
            float total = 0;
            foreach (DataTableViewRow row in this.dataTableView2.Rows)
            {
                if (name.Equals(row.Cells[this.colOutName.Index].Value))
                {
                    object value = row.Cells[this.colOutValue.Index].Value;
                    if (value != null)
                        total += GlobalMethods.Convert.StringToValue(value.ToString(), 0f);
                }
            }
            this.txtSummaryDesc.Focus();
            this.txtSummaryDesc.SelectedText = string.Concat(name, "：", total, "；");
        }
    }
}
