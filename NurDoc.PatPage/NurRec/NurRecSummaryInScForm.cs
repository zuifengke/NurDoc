// ***********************************************************
// 护理电子病历系统,护理记录单护理记录小结编辑脚本窗口.
// Creator:YeChongchong  Date:2014-02-10
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
    internal partial class NurRecSummaryInScForm : HerenForm
    {
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

        public NurRecSummaryInScForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.FormEdit;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

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
            if (this.LoadSummaryTemplet())
            {
                Size size = this.formControl1.Controls[0].Size;
                size.Width += this.Width - this.formControl1.Width;
                size.Width += SystemInformation.VerticalScrollBarWidth;
                size.Height += this.Height - this.formControl1.Height;
                size.Height += SystemInformation.HorizontalScrollBarHeight;
                GlobalMethods.UI.LocateScreenCenter(this, size);
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            this.UpdateDataByRecStatus(this.m_bIsNewRecord);

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public DialogResult ShowDialog(NursingRecInfo nursingRecInfo)
        {
            this.m_nursingRecInfo = nursingRecInfo;
            return base.ShowDialog();
        }

        /// <summary>
        /// 根据是否为新增状态，更新脚本内容，新增护理小结传递空，修改护理小结传递recid
        /// </summary>
        /// <param name="IsNewRecord">是否为新增</param>
        public void UpdateDataByRecStatus(Boolean IsNewRecord)
        {
            if (IsNewRecord)
            {
                this.formControl1.UpdateFormData("新增护理记录小结", string.Empty);
            }
            else
            {
                this.formControl1.UpdateFormData("修改护理记录小结", this.m_nursingRecInfo.RecordID);
            }
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

        /// <summary>
        /// 保存当前修改的护理记录记录本身,不包含评估数据
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool SaveNursingRecord(Hashtable hstSummary)
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

            if (GlobalMethods.Misc.IsEmptyString(hstSummary["统计起始时间"]))
            {
                MessageBoxEx.ShowMessage("您没有选择统计起始时间!");
                return false;
            }
            if (GlobalMethods.Misc.IsEmptyString(hstSummary["记录时间"]))
            {
                MessageBoxEx.ShowMessage("您没有选择记录时间!");
                return false;
            }
            this.m_nursingRecInfo.SummaryName = hstSummary["护理小结名称"].ToString();
            DateTime dtTime = new DateTime();
            if (!GlobalMethods.Convert.StringToDate(hstSummary["记录时间"], ref dtTime))
            {
                MessageBoxEx.Show("记录时间格式错误!");
                return false;
            }
            this.m_nursingRecInfo.RecordTime = dtTime;
            this.m_nursingRecInfo.RecordDate = this.m_nursingRecInfo.RecordTime.Date;
            if (!GlobalMethods.Convert.StringToDate(hstSummary["统计起始时间"], ref dtTime))
            {
                MessageBoxEx.Show("统计起始时间格式错误!");
                return false;
            }
            this.m_nursingRecInfo.SummaryStartTime = dtTime;
            this.m_nursingRecInfo.RecordContent = hstSummary["小结总结"].ToString();
            this.m_nursingRecInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            this.m_nursingRecInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            this.m_nursingRecInfo.ModifyTime = SysTimeService.Instance.Now;

            string szEnterTotal = hstSummary["入量"].ToString();
            string sznOutTotal = hstSummary["出量"].ToString();
            this.m_nursingRecInfo.RecordRemarks = string.Concat(szEnterTotal, "$", sznOutTotal);

            short shRet = SystemConst.ReturnValue.OK;
            if (this.m_bIsNewRecord)
            {
                this.m_nursingRecInfo.SchemaID = this.m_currentSchema.SchemaID;
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
        /// 加载表单
        /// </summary>
        /// <returns>加载成功与否</returns>
        private bool LoadSummaryTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_SUMMARY;
            string szTempletName = SystemContext.Instance.SystemOption.RecSummaryName;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError(szTempletName + "表单还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError(szTempletName + "表单下载失败!");
                return false;
            }
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        private void formControl1_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.Name == "导入医嘱模板")
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                string szValue = string.Empty;
                szValue = UtilitiesHandler.Instance.ShowTextTempletForm();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                e.Result = szValue;
            }
            if (e.Name == "保存护理小结")
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                Hashtable hstSummaryData = e.Data as Hashtable;
                if (hstSummaryData.Count != 6 || !hstSummaryData.ContainsKey("护理小结名称") || !hstSummaryData.ContainsKey("记录时间") || 
                    !hstSummaryData.ContainsKey("统计起始时间") || !hstSummaryData.ContainsKey("入量") || !hstSummaryData.ContainsKey("出量") || !hstSummaryData.ContainsKey("小结总结"))
                {
                    StringBuilder sbEx = new StringBuilder("保存数据传递错误！");
                    sbEx.AppendLine("需要传递：护理小结名称|记录时间|统计起始时间|入量|出量|小结总结");
                    MessageBoxEx.Show(sbEx.ToString());
                    return;
                }
                if (this.SaveNursingRecord(hstSummaryData))
                {
                    this.DialogResult = DialogResult.OK;
                }
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            if (e.Name == "取消")
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.DialogResult = DialogResult.Cancel;
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        private void formControl1_QueryContext(object sender, QueryContextEventArgs e)
        {
            if (e.Name == "小结名称配置")
            {
                e.Value = SystemContext.Instance.SummaryNameList;
                e.Success = true;
                return;
            }
            if (e.Name == "24小结出入量的算法")
            {
                e.Value = SystemContext.Instance.SystemOption.OptionCalculateSummary;
                e.Success = true;
                return;
            }
            if (e.Name == "护理记录列配置方案编号") 
            {
                e.Value = this.m_currentSchema.SchemaID;
                e.Success = true;
                return;
            }
        }

        private void formControl1_ExecuteQuery(object sender, ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }
    }
}
