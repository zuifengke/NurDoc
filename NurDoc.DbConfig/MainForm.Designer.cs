namespace Heren.NurDoc.DbConfig
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.txtConfigFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenConfigFile = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpMdsConfig = new System.Windows.Forms.TabPage();
            this.cboNdsConnString = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboNdsDbProvider = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboNdsDbType = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxRestFulString = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdbRestMode = new System.Windows.Forms.RadioButton();
            this.rdbDBMode = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tpMdsConfig.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(165, 361);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(140, 32);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "保存";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtConfigFile
            // 
            this.txtConfigFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConfigFile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtConfigFile.Location = new System.Drawing.Point(132, 12);
            this.txtConfigFile.Name = "txtConfigFile";
            this.txtConfigFile.ReadOnly = true;
            this.txtConfigFile.Size = new System.Drawing.Size(501, 21);
            this.txtConfigFile.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 27;
            this.label1.Text = "当前目标配置文件：";
            // 
            // btnOpenConfigFile
            // 
            this.btnOpenConfigFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenConfigFile.Image = global::Heren.NurDoc.DbConfig.Properties.Resources.open;
            this.btnOpenConfigFile.Location = new System.Drawing.Point(642, 11);
            this.btnOpenConfigFile.Name = "btnOpenConfigFile";
            this.btnOpenConfigFile.Size = new System.Drawing.Size(28, 23);
            this.btnOpenConfigFile.TabIndex = 1;
            this.btnOpenConfigFile.UseVisualStyleBackColor = true;
            this.btnOpenConfigFile.Click += new System.EventHandler(this.btnOpenConfigFile_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpMdsConfig);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(15, 85);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(655, 263);
            this.tabControl1.TabIndex = 34;
            // 
            // tpMdsConfig
            // 
            this.tpMdsConfig.Controls.Add(this.cboNdsConnString);
            this.tpMdsConfig.Controls.Add(this.label3);
            this.tpMdsConfig.Controls.Add(this.cboNdsDbProvider);
            this.tpMdsConfig.Controls.Add(this.label2);
            this.tpMdsConfig.Controls.Add(this.label6);
            this.tpMdsConfig.Controls.Add(this.cboNdsDbType);
            this.tpMdsConfig.Location = new System.Drawing.Point(4, 22);
            this.tpMdsConfig.Name = "tpMdsConfig";
            this.tpMdsConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tpMdsConfig.Size = new System.Drawing.Size(647, 237);
            this.tpMdsConfig.TabIndex = 0;
            this.tpMdsConfig.Text = "病历数据库配置";
            this.tpMdsConfig.UseVisualStyleBackColor = true;
            // 
            // cboNdsConnString
            // 
            this.cboNdsConnString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNdsConnString.FormattingEnabled = true;
            this.cboNdsConnString.Items.AddRange(new object[] {
            "provider=msdaora.1;data source=nurdoc;user id=nurdoc;password=nurdoc;",
            "provider=oraoledb.oracle.1;data source=nurdoc;user id=nurdoc;password=nurdoc;",
            "provider=sqloledb.1;server=.;database=nurdoc;user id=sa;password=nurdoc;",
            "data source=nurdoc;user id=nurdoc;password=nurdoc;",
            "server=.;database=nurdoc;user id=sa;password=nurdoc;",
            "dsn=nurdoc;uid=nurdoc;pwd=nurdoc;"});
            this.cboNdsConnString.Location = new System.Drawing.Point(146, 141);
            this.cboNdsConnString.Name = "cboNdsConnString";
            this.cboNdsConnString.Size = new System.Drawing.Size(484, 20);
            this.cboNdsConnString.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 41;
            this.label3.Text = "数据提供程序类型：";
            // 
            // cboNdsDbProvider
            // 
            this.cboNdsDbProvider.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cboNdsDbProvider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNdsDbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNdsDbProvider.FormattingEnabled = true;
            this.cboNdsDbProvider.Items.AddRange(new object[] {
            "System.Data.OracleClient",
            "System.Data.SqlClient",
            "System.Data.OleDb",
            "System.Data.Odbc",
            "Oracle.DataAccess.Client"});
            this.cboNdsDbProvider.Location = new System.Drawing.Point(146, 100);
            this.cboNdsDbProvider.Name = "cboNdsDbProvider";
            this.cboNdsDbProvider.Size = new System.Drawing.Size(484, 20);
            this.cboNdsDbProvider.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 40;
            this.label2.Text = "数据库连接字符串：";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 38;
            this.label6.Text = "数据库类型：";
            // 
            // cboNdsDbType
            // 
            this.cboNdsDbType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNdsDbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNdsDbType.FormattingEnabled = true;
            this.cboNdsDbType.Items.AddRange(new object[] {
            "ORACLE",
            "SQLSERVER"});
            this.cboNdsDbType.Location = new System.Drawing.Point(146, 61);
            this.cboNdsDbType.Name = "cboNdsDbType";
            this.cboNdsDbType.Size = new System.Drawing.Size(484, 20);
            this.cboNdsDbType.TabIndex = 34;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.tbxRestFulString);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(647, 237);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "RestFul数据配置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "RestFul地址：";
            // 
            // tbxRestFulString
            // 
            this.tbxRestFulString.Location = new System.Drawing.Point(92, 12);
            this.tbxRestFulString.Name = "tbxRestFulString";
            this.tbxRestFulString.Size = new System.Drawing.Size(448, 21);
            this.tbxRestFulString.TabIndex = 0;
            this.tbxRestFulString.Text = "localhost:88052/heren/api";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(377, 361);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 32);
            this.btnClose.TabIndex = 35;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbRestMode);
            this.panel1.Controls.Add(this.rdbDBMode);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(15, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(651, 39);
            this.panel1.TabIndex = 36;
            // 
            // rdbRestMode
            // 
            this.rdbRestMode.AutoSize = true;
            this.rdbRestMode.Location = new System.Drawing.Point(216, 11);
            this.rdbRestMode.Name = "rdbRestMode";
            this.rdbRestMode.Size = new System.Drawing.Size(107, 16);
            this.rdbRestMode.TabIndex = 2;
            this.rdbRestMode.TabStop = true;
            this.rdbRestMode.Text = "连接ResFul服务";
            this.rdbRestMode.UseVisualStyleBackColor = true;
            // 
            // rdbDBMode
            // 
            this.rdbDBMode.AutoSize = true;
            this.rdbDBMode.Location = new System.Drawing.Point(91, 11);
            this.rdbDBMode.Name = "rdbDBMode";
            this.rdbDBMode.Size = new System.Drawing.Size(119, 16);
            this.rdbDBMode.TabIndex = 1;
            this.rdbDBMode.TabStop = true;
            this.rdbDBMode.Text = "直接连数据库模式";
            this.rdbDBMode.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "使用的连接：";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(684, 405);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnOpenConfigFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConfigFile);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "护理病历系统数据库配置";
            this.tabControl1.ResumeLayout(false);
            this.tpMdsConfig.ResumeLayout(false);
            this.tpMdsConfig.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtConfigFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenConfigFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpMdsConfig;
        private System.Windows.Forms.ComboBox cboNdsConnString;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboNdsDbProvider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboNdsDbType;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxRestFulString;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdbRestMode;
        private System.Windows.Forms.RadioButton rdbDBMode;
    }
}