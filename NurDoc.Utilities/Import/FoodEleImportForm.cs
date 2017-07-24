// **************************************************************
// 护理电子病历系统公用模块之计算食物含水量数据导入窗口
// Creator:YangMingkun  Date:2012-10-16
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;

namespace Heren.NurDoc.Utilities.Import
{
    public partial class FoodEleImportForm : HerenForm
    {
        public FoodEleImportForm()
        {
            this.InitializeComponent();
            toolStripStatusLabel1.Text = string.Empty;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private string m_selectedFName = null;

        /// <summary>
        /// 获取食物含水量
        /// </summary>
        [Browsable(false)]
        [Description("获取食物含水量")]
        public string SelectedFName
        {
            get { return this.m_selectedFName; }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.Initialize_Control();
            this.LoadFNameList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载食物名称及其成分（含水量）
        /// </summary>
        private void LoadFNameList()
        {
            List<FoodEleInfo> lstFoodEles = null;
            short shRet = FoodEleService.Instance.GetAllFoodElesFolder(ref lstFoodEles);
            
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("食物成分列表下载失败!");
                return;
            }
            if (lstFoodEles == null || lstFoodEles.Count <= 0)
                return;

            for (int index = 0; index < lstFoodEles.Count; index++)
            {
                this.cboFNameFind.Items.Add(lstFoodEles[index]);
            }  
        }

        /// <summary>
        /// 更改所选择的食物名称，重新计算含水量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboFNameFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtWeight.Focus();
            CountWater();
        }
        /// <summary>
        /// 更改食物质量，重新计算含水量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            CountWater();
        }
        /// <summary>
        /// 计算食物含水量
        /// </summary>
        private void CountWater()
        {
            if (txtWeight.Text.Trim().Equals(""))
            {
                this.txtWater.Text = string.Empty;
                return;
            }
            if (GlobalMethods.Convert.IsNumericValue(txtWeight.Text) == true && Convert.ToDouble(txtWeight.Text) >= 0)
            {
                FoodEleInfo fEleinfo = this.cboFNameFind.SelectedItem as FoodEleInfo;
                if (fEleinfo != null && fEleinfo.FoodName.Equals(this.cboFNameFind.Text.Trim()))
                {
                    this.txtWater.Text = (Convert.ToDouble(fEleinfo.Water) * Convert.ToDouble(this.txtWeight.Text) / 100).ToString();
                }
                else
                    this.txtWater.Text = "食物名称有误";
            }
            else
            {
                this.txtWater.Text = "食物质量有误";
            }
        }  

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.m_selectedFName = null;
            if (this.cboFNameFind.Text.Trim().Equals("") || this.cboFNameFind.Text.Trim() == string.Empty)
            {
                MessageBoxEx.Show("请选择食物名称！");
                return;
            }
            if (this.txtWater.Text.Trim().Equals("") || this.txtWater.Text.Trim() == string.Empty || this.txtWater.Text.Trim().Equals("食物质量有误"))
            {
                MessageBoxEx.Show("请填写正确的食物质量！");
                return;
            }
            this.m_selectedFName = string.Format("{0},{1}g,{2}g水;", this.cboFNameFind.Text.Trim(), this.txtWeight.Text, this.txtWater.Text);
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 新建或更新数据库中食物成分表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.Initialize_Control();
            this.UpdateFile();
            this.LoadFNameList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 更新数据库中食物成分表
        /// </summary>
        private void UpdateFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "excel文档|*.xls;*.xlsx|所有文档(*.*)|*.*";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            string szFilePath = fileDialog.FileName;
            FoodEleInfo foodEleInfo = new FoodEleInfo();
            if (MessageBoxEx.ShowConfirm("此操作会删除库内对应表的原有数据！确认要更新文件吗?") == DialogResult.OK)
            {
                List<FoodEleInfo> lstFoodEles = null;
                short shRet = FoodEleService.Instance.GetExcelValues(szFilePath, ref lstFoodEles);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    toolStripStatusLabel1.Text = "读取文件失败!";
                    return;
                }
                if (lstFoodEles == null || lstFoodEles.Count <= 0)
                    return;

                shRet = FoodEleService.Instance.UpdateFoodEleToOrcale(lstFoodEles);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    toolStripStatusLabel1.Text = "食物成分更新失败!";
                    return;
                }
                toolStripStatusLabel1.Text = "更新完成";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Initialize_Control();
            this.Close();
        }  

        private void FoodEleImportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Initialize_Control();
        }

        private void Initialize_Control()
        {
            this.cboFNameFind.Items.Clear();
            this.cboFNameFind.Text = string.Empty;
            this.txtWeight.Text = string.Empty;
            this.txtWater.Text = string.Empty;
            this.toolStripStatusLabel1.Text = string.Empty;
        }
    }
}