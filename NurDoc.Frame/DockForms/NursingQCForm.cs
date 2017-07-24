// ***********************************************************
// 护理电子病历系统,病历指控窗口.
// Creator:Yechongchong  Date:2014-1-5
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.Frame.Dialogs;
using Heren.NurDoc.Utilities.Forms;
using Heren.NurDoc.PatPage.Utility;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class NursingQCForm : DockContentBase
    {
        /// <summary>
        /// 用于保存病历指控的相关表单
        /// </summary>
        private List<DocTypeInfo> lstDocTypeInfo = null;

        public NursingQCForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.NursingQC;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 500);
            this.Hide();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();

            this.dtBeginTime.Value = SysTimeService.Instance.Now.AddDays(1 - SysTimeService.Instance.Now.Day);
            this.dtEndTime.Value = SysTimeService.Instance.Now.AddMonths(1).AddDays(-SysTimeService.Instance.Now.Day);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadQcExamineModule();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 绑定病历指控表单列表并设置初始值
        /// </summary>
        private void LoadQcExamineModule()
        {
            if (this.BindQCExamintModule())
            {
                this.cboBingLiType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 绑定病历指控表单列表到下拉控件
        /// </summary>
        /// <returns>true Or false</returns>
        private bool BindQCExamintModule()
        {
            string szApply = ServerData.DocTypeApplyEnv.NURSING_QC;
            lstDocTypeInfo = FormCache.Instance.GetDocTypeList(szApply);
            if (lstDocTypeInfo == null || lstDocTypeInfo.Count <= 0)
            {
                MessageBoxEx.ShowError("加载病历指控表单失败!");
                return false;
            }
            bool bExistsValueAble = false;
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfo)
            {
                if (docTypeInfo == null
                    || GlobalMethods.Misc.IsEmptyString(docTypeInfo.DocTypeID))
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (docTypeInfo.IsFolder)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                this.cboBingLiType.Items.Add(docTypeInfo.DocTypeName);
                bExistsValueAble = true;
            }
            return bExistsValueAble;
        }

        /// <summary>
        /// 加载表单
        /// </summary>
        /// <returns>加载成功与否</returns>
        private bool LoadQcExamine()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_QC;
            string szTempletName = this.cboBingLiType.Text;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                //MessageBoxEx.ShowError(szTempletName + "表单还没有制作!");
                this.Close();
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

        /// <summary>
        /// 查询并更新界面数据
        /// </summary>
        private void SearchAndUpdateForm()
        {
            if (this.cboBingLiType.Text == string.Empty)
            {
                return;
            }
            if (this.LoadQcExamine() == false)
            {
                MessageBoxEx.ShowError("表单加载失败!");
            }
            else
            {
                object visible = this.formControl1.GetFormData("是否显示批量审核按钮");
                this.btnQCAll.Visible = (visible is bool) ? (bool)visible : false;
                StringBuilder szSearchCondition = new StringBuilder();
                string szFormat = "{0};{1};{2};{3};{4}";
                if (this.chkInHospital.Checked == true)
                {
                    szSearchCondition.AppendFormat(szFormat, "TRUE", "FALSE", string.Empty, string.Empty, string.Empty);
                }
                else if (this.chkOutHospital.Checked == true)
                {
                    szSearchCondition.AppendFormat(szFormat, "FALSE", "TRUE", this.dtBeginTime.Text, this.dtEndTime.Text, string.Empty);
                }
                else
                {
                    if (this.txtPatientID.Enabled == true && this.txtPatientID.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("病人ID不能为空!");
                        return;
                    }
                    szSearchCondition.AppendFormat(szFormat, "FALSE", "FALSE", string.Empty, string.Empty, this.txtPatientID.Text.Trim());
                }
                szSearchCondition.AppendFormat(this.chkNotExamine.Checked ? ";TRUE" : ";");
                szSearchCondition.AppendFormat(this.chkExamine.Checked ? ";TRUE" : ";");
                this.formControl1.UpdateFormData("查询条件", szSearchCondition.ToString());
                this.formControl1.UpdateFormData("刷新视图", true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SearchAndUpdateForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void chkInHospital_Click(object sender, EventArgs e)
        {
            if (this.chkInHospital.Checked)
            {
                this.chkOutHospital.Checked = false;
                this.dtBeginTime.Enabled = false;
                this.dtEndTime.Enabled = false;
            }

            if (this.chkInHospital.Checked == false && this.chkOutHospital.Checked == false)
            {
                this.txtPatientID.Enabled = true;
            }
            else
            {
                this.txtPatientID.Enabled = false;
            }
        }

        private void chkOutHospital_Click(object sender, EventArgs e)
        {
            if (this.chkOutHospital.Checked)
            {
                this.chkInHospital.Checked = false;
                this.dtBeginTime.Enabled = true;
                this.dtEndTime.Enabled = true;
            }
            else
            {
                this.dtBeginTime.Enabled = false;
                this.dtEndTime.Enabled = false;
            }

            if (this.chkInHospital.Checked == false && this.chkOutHospital.Checked == false)
            {
                this.txtPatientID.Enabled = true;
            }
            else
            {
                this.txtPatientID.Enabled = false;
            }
        }

        private void cboBingLiType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SearchAndUpdateForm();
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "保存病历质控信息")
            {
                QcExamineHandler.Instance.HandleQcExamineSave(e.Data);
            }
            else if (e.Name == "打开文档审核")
            {
                List<NurDocInfo> lstNurDocInfo = new List<NurDocInfo>();
                QcExamineHandler.Instance.getNurDocInfoFromString(e.Data, ref lstNurDocInfo);
                QcNurDocForm qcform = new QcNurDocForm(lstNurDocInfo, GlobalMethods.Convert.StringToValue(e.Param, 0));
                qcform.ShowDialog();

                this.SearchAndUpdateForm();
            }
            else if (e.Name == "打开体温单审核")
            {
                //体温单传值为DataTable及对应索引行号   DataTable 包含两列 pid vid
                if (e.Data is DataTable)
                {
                    DataTable dt = e.Data as DataTable;
                    if (dt.Columns.Count != 2)
                    {
                        MessageBoxEx.Show("体温单打开参数错误");
                        return;
                    }
                    QcTemperature qcform = new QcTemperature(e.Data as DataTable, GlobalMethods.Convert.StringToValue(e.Param, 0));
                    qcform.ShowDialog();

                    this.SearchAndUpdateForm();
                }
            }
            else if (e.Name == "打开医嘱单审核")
            {
                //医嘱单传值为DataTable及对应索引行号   DataTable 包含两列 pid vid
                if (e.Data is DataTable)
                {
                    DataTable dt = e.Data as DataTable;
                    if (dt.Columns.Count != 2)
                    {
                        MessageBoxEx.Show("医嘱单打开参数错误");
                        return;
                    }
                    QcOrderRecForm qcform = new QcOrderRecForm(e.Data as DataTable, GlobalMethods.Convert.StringToValue(e.Param, 0));
                    qcform.ShowDialog();

                    this.SearchAndUpdateForm();
                }
            }
            else if (e.Name == "打开护理记录单审核")
            {
                //护理记录单传值为DataTable及对应索引行号   DataTable 包含两列 pid vid
                if (e.Data is DataTable)
                {
                    DataTable dt = e.Data as DataTable;
                    if (dt.Columns.Count != 2)
                    {
                        MessageBoxEx.Show("护理记录单打开参数错误");
                        return;
                    }
                    QcNurRecForm qcform = new QcNurRecForm(e.Data as DataTable, GlobalMethods.Convert.StringToValue(e.Param, 0));
                    qcform.ShowDialog();

                    this.SearchAndUpdateForm();
                }
            }
        }

        private void chkNotExamine_CheckedChanged(object sender, EventArgs e)
        {
            StringBuilder sbExamineState = new StringBuilder();
            sbExamineState.Append(this.chkNotExamine.Checked ? "未审核" : string.Empty);
            sbExamineState.Append("||");
            sbExamineState.Append(this.chkExamine.Checked ? "已审核" : string.Empty);
            this.formControl1.UpdateFormData("根据状态刷新视图", sbExamineState.ToString());
        }

        private void chkExamine_CheckedChanged(object sender, EventArgs e)
        {
            StringBuilder sbExamineState = new StringBuilder();
            sbExamineState.Append(this.chkNotExamine.Checked ? "未审核" : string.Empty);
            sbExamineState.Append("||");
            sbExamineState.Append(this.chkExamine.Checked ? "已审核" : string.Empty);
            this.formControl1.UpdateFormData("根据状态刷新视图", sbExamineState.ToString());
        }

        private void btnQCAll_Click(object sender, EventArgs e)
        {
            this.formControl1.GetFormData("批量审核");
        }
    }
}