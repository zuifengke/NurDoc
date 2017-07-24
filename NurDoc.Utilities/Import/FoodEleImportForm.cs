// **************************************************************
// ������Ӳ���ϵͳ����ģ��֮����ʳ�ﺬˮ�����ݵ��봰��
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
        /// ��ȡʳ�ﺬˮ��
        /// </summary>
        [Browsable(false)]
        [Description("��ȡʳ�ﺬˮ��")]
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
        /// ����ʳ�����Ƽ���ɷ֣���ˮ����
        /// </summary>
        private void LoadFNameList()
        {
            List<FoodEleInfo> lstFoodEles = null;
            short shRet = FoodEleService.Instance.GetAllFoodElesFolder(ref lstFoodEles);
            
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("ʳ��ɷ��б�����ʧ��!");
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
        /// ������ѡ���ʳ�����ƣ����¼��㺬ˮ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboFNameFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtWeight.Focus();
            CountWater();
        }
        /// <summary>
        /// ����ʳ�����������¼��㺬ˮ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            CountWater();
        }
        /// <summary>
        /// ����ʳ�ﺬˮ��
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
                    this.txtWater.Text = "ʳ����������";
            }
            else
            {
                this.txtWater.Text = "ʳ����������";
            }
        }  

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.m_selectedFName = null;
            if (this.cboFNameFind.Text.Trim().Equals("") || this.cboFNameFind.Text.Trim() == string.Empty)
            {
                MessageBoxEx.Show("��ѡ��ʳ�����ƣ�");
                return;
            }
            if (this.txtWater.Text.Trim().Equals("") || this.txtWater.Text.Trim() == string.Empty || this.txtWater.Text.Trim().Equals("ʳ����������"))
            {
                MessageBoxEx.Show("����д��ȷ��ʳ��������");
                return;
            }
            this.m_selectedFName = string.Format("{0},{1}g,{2}gˮ;", this.cboFNameFind.Text.Trim(), this.txtWeight.Text, this.txtWater.Text);
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// �½���������ݿ���ʳ��ɷֱ�
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
        /// �������ݿ���ʳ��ɷֱ�
        /// </summary>
        private void UpdateFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "��ѡ���ļ�";
            fileDialog.Filter = "excel�ĵ�|*.xls;*.xlsx|�����ĵ�(*.*)|*.*";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            string szFilePath = fileDialog.FileName;
            FoodEleInfo foodEleInfo = new FoodEleInfo();
            if (MessageBoxEx.ShowConfirm("�˲�����ɾ�����ڶ�Ӧ���ԭ�����ݣ�ȷ��Ҫ�����ļ���?") == DialogResult.OK)
            {
                List<FoodEleInfo> lstFoodEles = null;
                short shRet = FoodEleService.Instance.GetExcelValues(szFilePath, ref lstFoodEles);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    toolStripStatusLabel1.Text = "��ȡ�ļ�ʧ��!";
                    return;
                }
                if (lstFoodEles == null || lstFoodEles.Count <= 0)
                    return;

                shRet = FoodEleService.Instance.UpdateFoodEleToOrcale(lstFoodEles);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    toolStripStatusLabel1.Text = "ʳ��ɷָ���ʧ��!";
                    return;
                }
                toolStripStatusLabel1.Text = "�������";
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