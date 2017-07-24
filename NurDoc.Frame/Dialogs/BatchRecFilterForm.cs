// ***********************************************************
// 护理电子病历系统,主窗口顶部白板模块.
// Creator:ZhangLian  Date:2012-6-16
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
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class BatchRecFilterForm : HerenForm
    {
        private PatVisitInfo[] m_selectedPatients = null;

        /// <summary>
        /// 获取选中的病人列表
        /// </summary>
        [Browsable(false)]
        public PatVisitInfo[] SelectedPatients
        {
            get
            {
                if (this.m_selectedPatients != null)
                    return this.m_selectedPatients;
                return this.GetSelectedPatientList();
            }
        }

        private GridViewSchema m_schema = null;

        /// <summary>
        /// 获取或设置体征批量录入方案
        /// </summary>
        [Browsable(false)]
        [Description("获取或设置体征批量录入方案")]
        public GridViewSchema Schema
        {
            get { return this.m_schema; }
            set { this.m_schema = value; }
        }

        private DateTime m_dtRecordTime = DateTime.Now;

        /// <summary>
        /// 获取或设置体征批量录入时间
        /// </summary>
        [Browsable(false)]
        [Description("获取或设置体征批量录入时间")]
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
            set { this.m_dtRecordTime = value; }
        }

        public BatchRecFilterForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.PersonsIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadFilterTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_selectedPatients = this.GetSelectedPatientList();
            if (this.m_selectedPatients != null)
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private bool GetSystemContext(string name, ref object value)
        {
            if (name == "批量录入方案名称")
            {
                if (this.m_schema == null)
                    return false;
                value = this.m_schema.SchemaName;
                return true;
            }
            else if (name == "批量录入方案标记")
            {
                if (this.m_schema == null)
                    return false;
                value = this.m_schema.SchemaFlag;
                return true;
            }
            else if (name == "体征时间")
            {
                value = this.m_dtRecordTime;
                return true;
            }
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        /// <summary>
        /// 加载病人筛选表单
        /// </summary>
        /// <returns>是否成功</returns>
        public bool LoadFilterTemplet()
        {
            if (this.Visible) this.Update();

            string szApplyEnv = ServerData.DocTypeApplyEnv.BATCH_RECORD_FILTER;
            DocTypeInfo docTypeInfo = null;
            if (this.Schema == null)
            {
                docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            }
            else
            {
                string szTempletName = this.Schema.SchemaName;
                docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            }
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("病人列表筛选模板还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("病人列表筛选模板内容下载失败!");
                return false;
            }
            bool success = this.formControl1.Load(docTypeInfo, byteFormData);
            this.formControl1.MaximizeEditRegion();
             if (this.Visible) this.Update();
            this.formControl1.UpdateFormData("体征时间", this.m_dtRecordTime);
           
            return success;
        }

        /// <summary>
        /// 获取当前选中的病人列表(旧方法)
        /// </summary>
        /// <param name="formData">表单数据</param>
        /// <returns>选中的病人列表</returns>
        private PatVisitInfo[] GetSelectedPatientListOld(object formData)
        {
            List<string> patientInfoList = formData as List<string>;
            if (patientInfoList == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowError("体征批量录入病人列表筛选失败!");
                return null;
            }

            List<PatVisitInfo> lstPatVisitInfos = new List<PatVisitInfo>();
            for (int index = 0; index < patientInfoList.Count; index++)
            {
                string szPatientInfo = patientInfoList[index];
                if (string.IsNullOrEmpty(szPatientInfo))
                    continue;
                string[] arrPatientInfo = szPatientInfo.Split(';');
                if (arrPatientInfo.Length < 6)
                    continue;
                PatVisitInfo patVisitInfo = new PatVisitInfo();
                patVisitInfo.BedCode = arrPatientInfo[0].Trim();
                patVisitInfo.PatientID = arrPatientInfo[1].Trim();
                patVisitInfo.VisitID = arrPatientInfo[2].Trim();
                patVisitInfo.PatientName = arrPatientInfo[3].Trim();
                patVisitInfo.DeptCode = arrPatientInfo[4].Trim();
                patVisitInfo.DeptName = arrPatientInfo[5].Trim();
                patVisitInfo.WardCode = patVisitInfo.DeptCode;
                patVisitInfo.WardName = patVisitInfo.DeptName;
                lstPatVisitInfos.Add(patVisitInfo);
            }
            return lstPatVisitInfos.ToArray();
        }

        /// <summary>
        /// 获取当前选中的病人列表
        /// </summary>
        /// <returns>选中的病人列表</returns>
        public PatVisitInfo[] GetSelectedPatientList()
        {
            object formData = this.formControl1.GetFormData("病人列表");
            if (formData is List<string>)
            {
                return this.GetSelectedPatientListOld(formData);
            }
            DataTable table = formData as DataTable;
            if (table == null)
            {
                MessageBoxEx.ShowError("体征批量录入病人列表筛选失败!");
                return null;
            }

            List<PatVisitInfo> lstPatVisitInfos = new List<PatVisitInfo>();
            foreach (DataRow row in table.Rows)
            {
                PatVisitInfo patVisitInfo = new PatVisitInfo();
                patVisitInfo.FromDataRow(row);
                if (!GlobalMethods.Misc.IsEmptyString(patVisitInfo.PatientID)
                    && !GlobalMethods.Misc.IsEmptyString(patVisitInfo.VisitID)
                    && !GlobalMethods.Misc.IsEmptyString(patVisitInfo.SubID))
                    lstPatVisitInfos.Add(patVisitInfo);
            }
            return lstPatVisitInfos.ToArray();
        }

        private void formControl1_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }
    }
}