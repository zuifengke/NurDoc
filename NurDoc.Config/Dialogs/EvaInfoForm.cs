// ***********************************************************
// 护理病历配置管理系统,评价单信息.
// Author : YeChongchong, Date : 2016-01-13
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
using Heren.Common.VectorEditor.Shapes;

namespace Heren.NurDoc.Config.Dialogs
{
    internal partial class EvaInfoForm : HerenForm
    {
        private EvaTypeInfo m_evaTypeInfo = null;

        /// <summary>
        /// 获取或设置当前表单类型信息
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置当前表单类型信息")]
        public EvaTypeInfo EvaTypeInfo
        {
            get { return this.m_evaTypeInfo; }
            set { this.m_evaTypeInfo = value; }
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

        private List<WardEvaType> m_lstWardEvaTypes = null;

        /// <summary>
        /// 获取或设置当前表单所属病区
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置当前表单所属病区")]
        public List<WardEvaType> WardEvaTypes
        {
            get { return this.m_lstWardEvaTypes; }
        }

        public EvaInfoForm()
        {
            this.InitializeComponent();
            this.m_lstWardEvaTypes = new List<WardEvaType>();
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
            this.LoadWardEvaType();
            this.LoadDocTypeInfo();
            this.txtTempletName.Focus();
            this.txtTempletName.SelectAll();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void LoadDocTypeInfo()
        {
            this.txtTempletID.Text = string.Empty;
            this.txtTempletName.Text = string.Empty;
            this.chkIsValid.Checked = true;
            this.chkIsVisible.Checked = true;
            this.txtDocTypeNo.Text = string.Empty;
            if (this.m_evaTypeInfo == null)
                return;
            this.txtTempletID.Text = this.m_evaTypeInfo.EvaTypeID;
            this.txtTempletName.Text = this.m_evaTypeInfo.EvaTypeName;
            this.chkIsValid.Checked = this.m_evaTypeInfo.IsValid;
            this.chkIsVisible.Checked = this.m_evaTypeInfo.IsVisible;
            this.txtDocTypeNo.Text = this.m_evaTypeInfo.EvaTypeNo.ToString();
            this.chkRemark.Checked = this.m_evaTypeInfo.HaveRemark;
            this.txtStandard.Text = this.m_evaTypeInfo.Standard.ToString();
        }

        private void LoadWardEvaType()
        {
            this.txtWardList.Text = "全院<可双击修改>";
            this.txtWardList.Tag = null;
            if (this.m_evaTypeInfo == null)
                return;
            string szEvaTypeID = this.m_evaTypeInfo.EvaTypeID;
            List<WardEvaType> lstWardEvaTypes = null;
            short shRet = EvaTypeService.Instance.GetEvaTypeDeptList(szEvaTypeID, ref lstWardEvaTypes);
            if (shRet != SystemConst.ReturnValue.OK)
                return;
            if (lstWardEvaTypes == null || lstWardEvaTypes.Count <= 0)
                return;
            this.txtWardList.Tag = lstWardEvaTypes;

            StringBuilder sbDeptList = new StringBuilder();
            foreach (WardEvaType WardEvaType in lstWardEvaTypes)
                sbDeptList.Append(WardEvaType.WardName + ";");
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
            List<WardEvaType> lstWardEvaTypes =
                this.txtWardList.Tag as List<WardEvaType>;
            if (lstWardEvaTypes == null)
                lstWardEvaTypes = new List<WardEvaType>();
            for (int index = 0;
                index < lstWardEvaTypes.Count; index++)
            {
                WardEvaType wardEvaType = lstWardEvaTypes[index];
                if (wardEvaType == null)
                    continue;
                DeptInfo deptInfo = new DeptInfo();
                deptInfo.DeptCode = wardEvaType.WardCode;
                deptInfo.DeptName = wardEvaType.WardName;
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

            lstWardEvaTypes.Clear();
            StringBuilder sbDeptList = new StringBuilder();
            for (int index = 0; index < deptInfos.Length; index++)
            {
                DeptInfo deptInfo = deptInfos[index];
                if (deptInfo == null)
                    continue;
                sbDeptList.Append(deptInfo.DeptName + ";");

                WardEvaType wardEvaType = new WardEvaType();
                wardEvaType.WardCode = deptInfo.DeptCode;
                wardEvaType.WardName = deptInfo.DeptName;
                wardEvaType.EvaTypeID = this.txtTempletID.Text;
                wardEvaType.EvaTypeName = this.txtTempletName.Text;
                lstWardEvaTypes.Add(wardEvaType);
            }
            this.txtWardList.Tag = lstWardEvaTypes;
            this.txtWardList.Text = sbDeptList.ToString();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 生成待保存的文档类型信息
        /// </summary>
        /// <returns>bool</returns>
        private bool MakeDocTypeInfo()
        {
            if (this.m_evaTypeInfo == null)
                this.m_evaTypeInfo = new EvaTypeInfo();

            if (this.txtTempletID.Text.Trim() == string.Empty)
            {
                MessageBoxEx.ShowError("请输入病历类型ID号");
                return false;
            }

            if (this.txtTempletName.Text.Trim() == string.Empty)
            {
                MessageBoxEx.ShowError("请输入病历类型名称");
                return false;
            }

            if (!GlobalMethods.Convert.IsIntegerValue(this.txtStandard.Text.Trim()) || Convert.ToInt32(this.txtStandard.Text.Trim()) < 0)
            {
                MessageBoxEx.ShowError("达标率为正整数");
                return false;
            }

            this.m_evaTypeInfo.EvaTypeID = this.txtTempletID.Text.Trim();
            this.m_evaTypeInfo.EvaTypeName = this.txtTempletName.Text.Trim();
            this.m_evaTypeInfo.IsVisible = this.chkIsVisible.Checked;
            this.m_evaTypeInfo.IsValid = this.chkIsValid.Checked;
            this.m_evaTypeInfo.HaveRemark = this.chkRemark.Checked;
            this.m_evaTypeInfo.Standard = Convert.ToInt32(this.txtStandard.Text.Trim());

            this.m_evaTypeInfo.EvaTypeNo =
                GlobalMethods.Convert.StringToValue(this.txtDocTypeNo.Text.Trim(), 0);
            return true;
        }

        /// <summary>
        /// 生成待保存的科室病历类型配置信息
        /// </summary>
        /// <returns>bool</returns>
        private bool MakeDeptInfoList()
        {
            if (this.m_lstWardEvaTypes == null)
                this.m_lstWardEvaTypes = new List<WardEvaType>();
            this.m_lstWardEvaTypes.Clear();

            if (this.m_evaTypeInfo == null)
                return false;
            List<WardEvaType> lstWardEvaTypes = null;
            lstWardEvaTypes = this.txtWardList.Tag as List<WardEvaType>;
            if (lstWardEvaTypes == null)
                return true;

            foreach (WardEvaType wardEvaType in lstWardEvaTypes)
            {
                if (wardEvaType == null)
                    continue;
                wardEvaType.EvaTypeID = this.m_evaTypeInfo.EvaTypeID;
                wardEvaType.EvaTypeName = this.m_evaTypeInfo.EvaTypeName;
                this.m_lstWardEvaTypes.Add(wardEvaType);
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

        private void chkDocTypeNo_CheckedChanged(object sender, EventArgs e)
        {
            this.txtDocTypeNo.Enabled = this.chkDocTypeNo.Checked;
        }
    }
}
