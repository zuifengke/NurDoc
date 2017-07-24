// ***********************************************************
// 护理病历配置管理系统,服务器报表属性信息编辑对话框.
// Author : YangMingkun, Date : 2012-7-30
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Config.Report
{
    internal partial class ReportInfoForm : HerenForm
    {
        private ReportTypeInfo m_reportTypeInfo = null;

        /// <summary>
        /// 获取或设置当前报表类型信息
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置当前报表类型信息")]
        public ReportTypeInfo ReportTypeInfo
        {
            get { return this.m_reportTypeInfo; }
            set { this.m_reportTypeInfo = value; }
        }

        private bool m_bIsNew = false;

        /// <summary>
        /// 获取或设置当前是否是新建
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        [Description("获取或设置当前是否是新建")]
        public bool IsNew
        {
            get { return this.m_bIsNew; }
            set { this.m_bIsNew = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置当前是否是目录
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        [Description("获取或设置当前是否是目录")]
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private List<WardReportType> m_lstWardReportTypes = null;

        /// <summary>
        /// 获取或设置当前报表所属病区
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置当前报表所属病区")]
        public List<WardReportType> WardReportTypes
        {
            get { return this.m_lstWardReportTypes; }
        }

        public ReportInfoForm()
        {
            this.InitializeComponent();
            this.m_lstWardReportTypes = new List<WardReportType>();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            if (this.m_bIsFolder)
            {
                this.Text = "目录信息";
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocTypeInfo();
            this.LoadWardDocType();
            this.txtTempletName.Focus();
            this.txtTempletName.SelectAll();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void LoadDocTypeInfo()
        {
            this.txtTempletID.Text = string.Empty;
            this.txtTempletName.Text = string.Empty;
            this.chkIsValid.Checked = true;
            if (this.m_reportTypeInfo == null)
                return;
            this.txtTempletID.Text = this.m_reportTypeInfo.ReportTypeID;
            this.txtTempletName.Text = this.m_reportTypeInfo.ReportTypeName;
            this.chkIsValid.Checked = this.m_reportTypeInfo.IsValid;
        }

        private void LoadWardDocType()
        {
            this.txtWardList.Text = "全院<可双击修改>";
            this.txtWardList.Tag = null;
            if (this.m_reportTypeInfo == null)
                return;
            string szDocTypeID = this.m_reportTypeInfo.ReportTypeID;
            List<WardReportType> lstWardReportTypes = null;
            short shRet = TempletService.Instance.GetReportTypeDeptList(szDocTypeID, ref lstWardReportTypes);
            if (shRet != SystemConst.ReturnValue.OK)
                return;
            if (lstWardReportTypes == null || lstWardReportTypes.Count <= 0)
                return;
            this.txtWardList.Tag = lstWardReportTypes;

            StringBuilder sbDeptList = new StringBuilder();
            foreach (WardReportType wardReportType in lstWardReportTypes)
                sbDeptList.Append(wardReportType.WardName + ";");
            this.txtWardList.Text = sbDeptList.ToString();
        }

        /// <summary>
        /// 显示所属病区列表数据编辑对话框
        /// </summary>
        private void ShowDeptListEditForm()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //初始化科室列表对话框中默认勾选的科室
            List<DeptInfo> lstDeptInfos = new List<DeptInfo>();
            List<WardReportType> lstWardReportTypes =
                this.txtWardList.Tag as List<WardReportType>;
            if (lstWardReportTypes == null)
                lstWardReportTypes = new List<WardReportType>();
            for (int index = 0;
                index < lstWardReportTypes.Count; index++)
            {
                WardReportType wardReportType = lstWardReportTypes[index];
                if (wardReportType == null)
                    continue;
                DeptInfo deptInfo = new DeptInfo();
                deptInfo.DeptCode = wardReportType.WardCode;
                deptInfo.DeptName = wardReportType.WardName;
                lstDeptInfos.Add(deptInfo);
            }

            DeptSelectDialog deptSelectDialog = new DeptSelectDialog();
            deptSelectDialog.DeptInfos = lstDeptInfos.ToArray();
            if (deptSelectDialog.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            this.txtWardList.Text = "全院<可双击修改>";
            this.txtWardList.Tag = null;
            DeptInfo[] deptInfos = deptSelectDialog.DeptInfos;
            if (deptInfos == null || deptInfos.Length <= 0)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            lstWardReportTypes.Clear();
            StringBuilder sbDeptList = new StringBuilder();
            for (int index = 0; index < deptInfos.Length; index++)
            {
                DeptInfo deptInfo = deptInfos[index];
                if (deptInfo == null)
                    continue;
                sbDeptList.Append(deptInfo.DeptName + ";");

                WardReportType wardReportType = new WardReportType();
                wardReportType.WardCode = deptInfo.DeptCode;
                wardReportType.WardName = deptInfo.DeptName;
                wardReportType.ReportTypeID = this.txtTempletID.Text;
                wardReportType.ReportTypeName = this.txtTempletName.Text;
                lstWardReportTypes.Add(wardReportType);
            }
            this.txtWardList.Tag = lstWardReportTypes;
            this.txtWardList.Text = sbDeptList.ToString();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 生成待保存的文档类型信息
        /// </summary>
        /// <returns>bool</returns>
        private bool MakeDocTypeInfo()
        {
            if (this.m_reportTypeInfo == null)
                this.m_reportTypeInfo = new ReportTypeInfo();

            if (this.txtTempletID.Text.Trim() == string.Empty)
            {
                MessageBoxEx.ShowError("请输入表单类型ID号");
                return false;
            }

            if (this.txtTempletName.Text.Trim() == string.Empty)
            {
                MessageBoxEx.ShowError("请输入表单类型名称");
                return false;
            }

            this.m_reportTypeInfo.ReportTypeID = this.txtTempletID.Text.Trim();
            this.m_reportTypeInfo.ReportTypeName = this.txtTempletName.Text.Trim();
            this.m_reportTypeInfo.IsValid = this.chkIsValid.Checked;
            return true;
        }

        /// <summary>
        /// 生成待保存的科室病历类型配置信息
        /// </summary>
        /// <returns>bool</returns>
        private bool MakeDeptInfoList()
        {
            if (this.m_lstWardReportTypes == null)
                this.m_lstWardReportTypes = new List<WardReportType>();
            this.m_lstWardReportTypes.Clear();

            if (this.m_reportTypeInfo == null)
                return false;
            List<WardReportType> lstWardReportTypes = null;
            lstWardReportTypes = this.txtWardList.Tag as List<WardReportType>;
            if (lstWardReportTypes == null)
                return true;

            foreach (WardReportType wardReportType in lstWardReportTypes)
            {
                if (wardReportType == null)
                    continue;
                wardReportType.ReportTypeID = this.m_reportTypeInfo.ReportTypeID;
                wardReportType.ReportTypeName = this.m_reportTypeInfo.ReportTypeName;
                this.m_lstWardReportTypes.Add(wardReportType);
            }
            return true;
        }

        private void txtWardList_DoubleClick(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowDeptListEditForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.MakeDocTypeInfo())
                return;
            if (!this.MakeDeptInfoList())
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DialogResult = DialogResult.OK;
        }
    }
}
