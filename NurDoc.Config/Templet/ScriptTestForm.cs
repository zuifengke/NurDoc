// ***********************************************************
// 护理病历配置管理系统,表单模板脚本测试窗口.
// Author : YangMingkun, Date : 2012-6-6
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Forms.Designer;
using Heren.Common.Forms.Runtime;
using Heren.Common.Forms.Loader;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Config.Templet
{
    internal partial class ScriptTestForm : HerenForm
    {
        private string m_szScriptData = null;

        /// <summary>
        /// 获取或设置脚本数据
        /// </summary>
        [Browsable(false)]
        public string ScriptData
        {
            get { return this.m_szScriptData; }
            set { this.m_szScriptData = value; }
        }

        private string m_szDesignData = null;

        /// <summary>
        /// 获取或设置设计数据
        /// </summary>
        [Browsable(false)]
        public string DesignData
        {
            get { return this.m_szDesignData; }
            set { this.m_szDesignData = value; }
        }

        public ScriptTestForm()
        {
            this.InitializeComponent();
            this.SaveWindowState = true;
            this.Icon = Properties.Resources.Test;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            FormFileParser parser = new FormFileParser();
            byte[] byteFileData = null;
            parser.MakeFormData(this.m_szDesignData, this.m_szScriptData, out byteFileData);
            this.formEditor1.Focus();
            this.formEditor1.Load(byteFileData);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "护理病历文件(*.hndf)|*.hndf|所有文件(*.*)|*.*";
            saveDialog.Title = "另存为本地病历文件";
            saveDialog.RestoreDirectory = true;

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.formEditor1.EndEdit();
            this.formEditor1.Save(saveDialog.FileName);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnMakeText_Click(object sender, EventArgs e)
        {
            MessageBoxEx.ShowMessage("脚本生成如下：\r\n" + this.formEditor1.GetFormData("表单摘要"));
        }

        private void formEditor1_ExecuteQuery(object sender, Heren.Common.Forms.Editor.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void formEditor1_ExecuteUpdate(object sender, Heren.Common.Forms.Editor.ExecuteUpdateEventArgs e)
        {
            if (CommonService.Instance.ExecuteUpdate(e.IsProc, e.SQL) == SystemConst.ReturnValue.OK)
                e.Success = true;
        }
    }
}
