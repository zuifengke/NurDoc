// **************************************************************
// 护理电子病历系统,本地配置文件中数据库访问参数读写窗口.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;

namespace Heren.NurDoc.DbConfig
{
    internal partial class MainForm : Form
    {
        /// <summary>
        /// 配置数据加密密钥
        /// </summary>
        private const string CONFIG_ENCRYPT_KEY = "SUPCON.MEDDOC.ENCRYPT.KEY";

        /// <summary>
        /// 病历数据库类型
        /// </summary>
        private const string NDS_DB_TYPE = "NdsDbType";

        /// <summary>
        /// 病历数据库驱动类型
        /// </summary>
        private const string NDS_PROVIDER_TYPE = "NdsDbProvider";

        /// <summary>
        /// 病历数据库连接串
        /// </summary>
        private const string NDS_CONN_STRING = "NdsDbConnString";

        /// <summary>
        /// 病历数据库RestFul连接串
        /// </summary>
        private const string NRS_CONN_STRING = "NResConnString";

        /// <summary>
        /// 护理文书系统连接模式
        /// </summary>
        private const string NUR_CONN_MODE = "NurConnMode";

        public MainForm()
        {
            this.InitializeComponent();
            this.Icon = DbConfig.Properties.Resources.SysIcon;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            if (this.ReadConfigFile(Application.StartupPath + "\\UserData\\NurDocSys.xml"))
                return;
        }

        private bool ReadConfigFile(string szConfigFile)
        {
            this.txtConfigFile.Text = szConfigFile;
            this.tabControl1.TabPages.Remove(this.tpMdsConfig);
            if (!System.IO.File.Exists(szConfigFile))
                return false;

            string szFileName = GlobalMethods.IO.GetFileName(szConfigFile, true).ToLower();
            if (szFileName == "nurdocsys.xml")
            {
                this.tabControl1.TabPages.Add(this.tpMdsConfig);
            }
            else
            {
                return false;
            }

            SystemConfig.Instance.ConfigFile = szConfigFile;
            if (this.tabControl1.TabPages.Contains(this.tpMdsConfig))
            {
                string szDbType = SystemConfig.Instance.Get(MainForm.NDS_DB_TYPE, string.Empty);
                this.cboNdsDbType.Text = GlobalMethods.Security.DecryptText(szDbType, CONFIG_ENCRYPT_KEY);

                string szDbDriverType = SystemConfig.Instance.Get(MainForm.NDS_PROVIDER_TYPE, string.Empty);
                this.cboNdsDbProvider.Text = GlobalMethods.Security.DecryptText(szDbDriverType, CONFIG_ENCRYPT_KEY);

                string szConnectionString = SystemConfig.Instance.Get(MainForm.NDS_CONN_STRING, string.Empty);
                this.cboNdsConnString.Text = GlobalMethods.Security.DecryptText(szConnectionString, CONFIG_ENCRYPT_KEY);

                string szRestConnectionString = SystemConfig.Instance.Get(MainForm.NRS_CONN_STRING, string.Empty);
                this.tbxRestFulString.Text = GlobalMethods.Security.DecryptText(szRestConnectionString, CONFIG_ENCRYPT_KEY);

                string szNurConnModeString = SystemConfig.Instance.Get(MainForm.NUR_CONN_MODE, string.Empty);
                string szNurConnMode = GlobalMethods.Security.DecryptText(szNurConnModeString, CONFIG_ENCRYPT_KEY);
                if (szNurConnMode == "DB")
                {
                    this.rdbDBMode.Checked = true;
                    this.rdbRestMode.Checked = false;
                }
                else if (szNurConnMode == "REST")
                {
                    this.rdbDBMode.Checked = false;
                    this.rdbRestMode.Checked = true;
                }
                else
                {
                    this.rdbDBMode.Checked = false;
                    this.rdbRestMode.Checked = false;
                }
            }
            return true;
        }

        private void btnOpenConfigFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "配置文件(*.xml)|*.xml";
            openDialog.Multiselect = false;
            if (openDialog.ShowDialog() == DialogResult.OK)
                this.ReadConfigFile(openDialog.FileName);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.TabPages.Contains(this.tpMdsConfig))
            {
                string szDbType = GlobalMethods.Security.EncryptText(this.cboNdsDbType.Text, CONFIG_ENCRYPT_KEY);
                string szDbDriverType = GlobalMethods.Security.EncryptText(this.cboNdsDbProvider.Text, CONFIG_ENCRYPT_KEY);
                string szConnectionString = GlobalMethods.Security.EncryptText(this.cboNdsConnString.Text, CONFIG_ENCRYPT_KEY);
                string szRestConnectionString = GlobalMethods.Security.EncryptText(this.tbxRestFulString.Text, CONFIG_ENCRYPT_KEY);
                string szNurConnMode = string.Empty;
                if (this.rdbDBMode.Checked)
                { szNurConnMode = "DB"; }
                if (this.rdbRestMode.Checked)
                { szNurConnMode = "REST"; }
                string szNurConnModeString = GlobalMethods.Security.EncryptText(szNurConnMode, CONFIG_ENCRYPT_KEY);
                SystemConfig.Instance.Write(MainForm.NDS_DB_TYPE, szDbType);
                SystemConfig.Instance.Write(MainForm.NDS_PROVIDER_TYPE, szDbDriverType);
                SystemConfig.Instance.Write(MainForm.NDS_CONN_STRING, szConnectionString);
                SystemConfig.Instance.Write(MainForm.NRS_CONN_STRING, szRestConnectionString);
                SystemConfig.Instance.Write(MainForm.NUR_CONN_MODE, szNurConnModeString);
            }
            MessageBox.Show("配置保存完成!", "系统配置", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}