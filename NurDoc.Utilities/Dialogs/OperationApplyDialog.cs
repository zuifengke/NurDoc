// ***********************************************************
// 护理电子病历系统公用模块之操作是否允许执行的申请窗口.
// Creator:YangMingkun  Date:2012-10-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Dialogs
{
    public partial class OperationApplyDialog : HerenForm
    {
        private OperationApplyInfo m_operationApplyInfo = null;

        /// <summary>
        /// 获取或设置操作申请信息
        /// </summary>
        [Browsable(false)]
        public OperationApplyInfo OperationApplyInfo
        {
            get { return this.m_operationApplyInfo; }
            set { this.m_operationApplyInfo = value; }
        }

        public OperationApplyDialog()
        {
            this.InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_operationApplyInfo != null)
                this.m_operationApplyInfo.RequestDesc = this.txtRequestDesc.Text;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            this.DialogResult = DialogResult.OK;
        }
    }
}