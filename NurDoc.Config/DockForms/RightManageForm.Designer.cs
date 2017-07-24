namespace Heren.NurDoc.Config.DockForms
{
    partial class RightManageForm
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
            if (this.m_RightsSelectForm != null && !this.m_RightsSelectForm.IsDisposed)
                this.m_RightsSelectForm.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RightManageForm));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuModifyUserRight = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelModify = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResetUserPassWord = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new Heren.Common.Controls.HerenButton();
            this.btnSelectAll = new Heren.Common.Controls.HerenButton();
            this.dgvUserRight = new Heren.Common.Controls.TableView.DataTableView();
            this.colUserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnUserRightSelect = new Heren.Common.Controls.FlatButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtUserName = new Heren.Common.Controls.TextBoxButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtUserID = new Heren.Common.Controls.TextBoxButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnQueryByDept = new Heren.Common.Controls.FlatButton();
            this.cboDeptList = new Heren.Common.Controls.DictInput.FindComboBox();
            this.cmenuSelectUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDeptAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDeptWard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeptOutp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeptNurse = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeptOther = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserRight)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.cmenuSelectUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuModifyUserRight,
            this.mnuCancelModify,
            this.mnuResetUserPassWord});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(185, 92);
            // 
            // mnuModifyUserRight
            // 
            this.mnuModifyUserRight.Name = "mnuModifyUserRight";
            this.mnuModifyUserRight.Size = new System.Drawing.Size(184, 22);
            this.mnuModifyUserRight.Text = "�޸�ѡ���û���Ȩ��";
            this.mnuModifyUserRight.Click += new System.EventHandler(this.mnuModifyUserRight_Click);
            // 
            // mnuCancelModify
            // 
            this.mnuCancelModify.Name = "mnuCancelModify";
            this.mnuCancelModify.Size = new System.Drawing.Size(184, 22);
            this.mnuCancelModify.Text = "ȡ����ѡ���е��޸�";
            this.mnuCancelModify.Click += new System.EventHandler(this.mnuCancelModify_Click);
            // 
            // mnuResetUserPassWord
            // 
            this.mnuResetUserPassWord.Name = "mnuResetUserPassWord";
            this.mnuResetUserPassWord.Size = new System.Drawing.Size(184, 22);
            this.mnuResetUserPassWord.Text = "����ѡ���û�������";
            this.mnuResetUserPassWord.Click += new System.EventHandler(this.mnuResetUserPassWord_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("����", 9F);
            this.btnSave.Location = new System.Drawing.Point(659, 22);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 28);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "����";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("����", 9F);
            this.btnSelectAll.Location = new System.Drawing.Point(564, 22);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(88, 28);
            this.btnSelectAll.TabIndex = 12;
            this.btnSelectAll.Text = "�鿴����";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // dgvUserRight
            // 
            this.dgvUserRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserRight.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUserRight.ColumnHeadersHeight = 35;
            this.dgvUserRight.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserID,
            this.colUserName,
            this.colDeptName});
            this.dgvUserRight.ContextMenuStrip = this.contextMenuStrip;
            this.dgvUserRight.Location = new System.Drawing.Point(1, 67);
            this.dgvUserRight.Name = "dgvUserRight";
            this.dgvUserRight.RowHeadersWidth = 32;
            this.dgvUserRight.Size = new System.Drawing.Size(806, 446);
            this.dgvUserRight.TabIndex = 15;
            this.dgvUserRight.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserRight_CellDoubleClick);
            // 
            // colUserID
            // 
            this.colUserID.FillWeight = 72F;
            this.colUserID.Frozen = true;
            this.colUserID.HeaderText = "�û�ID";
            this.colUserID.Name = "colUserID";
            this.colUserID.ReadOnly = true;
            this.colUserID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colUserID.Width = 72;
            // 
            // colUserName
            // 
            this.colUserName.FillWeight = 72F;
            this.colUserName.Frozen = true;
            this.colUserName.HeaderText = "����";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            this.colUserName.Width = 72;
            // 
            // colDeptName
            // 
            this.colDeptName.FillWeight = 120F;
            this.colDeptName.Frozen = true;
            this.colDeptName.HeaderText = "��������";
            this.colDeptName.Name = "colDeptName";
            this.colDeptName.ReadOnly = true;
            this.colDeptName.Width = 120;
            // 
            // btnUserRightSelect
            // 
            this.btnUserRightSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserRightSelect.Location = new System.Drawing.Point(780, 38);
            this.btnUserRightSelect.Name = "btnUserRightSelect";
            this.btnUserRightSelect.Size = new System.Drawing.Size(24, 24);
            this.btnUserRightSelect.TabIndex = 14;
            this.btnUserRightSelect.ToolTipText = "ѡ����Ҫ��ʾ��Ȩ�޵�";
            this.btnUserRightSelect.Click += new System.EventHandler(this.btnUserRightSelect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("����", 9F);
            this.label3.Location = new System.Drawing.Point(6, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "����";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("����", 9F);
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "ID��";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("����", 9F);
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "����";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtUserName);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(396, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(159, 56);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "������(ģ��ƥ��)";
            // 
            // txtUserName
            // 
            this.txtUserName.ButtonImage = ((System.Drawing.Image)(resources.GetObject("txtUserName.ButtonImage")));
            this.txtUserName.ButtonToolTip = null;
            this.txtUserName.Font = new System.Drawing.Font("����", 9F);
            this.txtUserName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtUserName.Location = new System.Drawing.Point(38, 22);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(114, 21);
            this.txtUserName.TabIndex = 10;
            this.txtUserName.ButtonClick += new System.EventHandler(this.txtUserName_ButtonClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtUserID);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(233, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 56);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "��ID��(���ִ�Сд)";
            // 
            // txtUserID
            // 
            this.txtUserID.BackColor = System.Drawing.SystemColors.Window;
            this.txtUserID.ButtonImage = ((System.Drawing.Image)(resources.GetObject("txtUserID.ButtonImage")));
            this.txtUserID.ButtonToolTip = null;
            this.txtUserID.Font = new System.Drawing.Font("����", 9F);
            this.txtUserID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtUserID.Location = new System.Drawing.Point(39, 22);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(110, 21);
            this.txtUserID.TabIndex = 6;
            this.txtUserID.ButtonClick += new System.EventHandler(this.txtUserID_ButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnQueryByDept);
            this.groupBox1.Controls.Add(this.cboDeptList);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "������";
            // 
            // btnQueryByDept
            // 
            this.btnQueryByDept.Location = new System.Drawing.Point(193, 19);
            this.btnQueryByDept.Name = "btnQueryByDept";
            this.btnQueryByDept.Size = new System.Drawing.Size(24, 24);
            this.btnQueryByDept.TabIndex = 3;
            this.btnQueryByDept.ToolTipText = null;
            this.btnQueryByDept.Click += new System.EventHandler(this.btnQueryByDept_Click);
            // 
            // cboDeptList
            // 
            this.cboDeptList.DroppedDown = false;
            this.cboDeptList.Location = new System.Drawing.Point(42, 20);
            this.cboDeptList.Name = "cboDeptList";
            this.cboDeptList.SelectedItem = null;
            this.cboDeptList.Size = new System.Drawing.Size(146, 23);
            this.cboDeptList.TabIndex = 2;
            this.cboDeptList.SelectedIndexChanged += new System.EventHandler(this.cboDeptList_SelectedIndexChanged);
            this.cboDeptList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboDeptList_KeyDown);
            // 
            // cmenuSelectUser
            // 
            this.cmenuSelectUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDeptAll,
            this.toolStripSeparator1,
            this.mnuDeptWard,
            this.mnuDeptOutp,
            this.mnuDeptNurse,
            this.mnuDeptOther});
            this.cmenuSelectUser.Name = "cmenuSelectUser";
            this.cmenuSelectUser.Size = new System.Drawing.Size(125, 120);
            // 
            // mnuDeptAll
            // 
            this.mnuDeptAll.Name = "mnuDeptAll";
            this.mnuDeptAll.Size = new System.Drawing.Size(124, 22);
            this.mnuDeptAll.Text = "���п���";
            this.mnuDeptAll.Click += new System.EventHandler(this.mnuDeptAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // mnuDeptWard
            // 
            this.mnuDeptWard.Name = "mnuDeptWard";
            this.mnuDeptWard.Size = new System.Drawing.Size(124, 22);
            this.mnuDeptWard.Text = "סԺ����";
            this.mnuDeptWard.Click += new System.EventHandler(this.mnuDeptWard_Click);
            // 
            // mnuDeptOutp
            // 
            this.mnuDeptOutp.Name = "mnuDeptOutp";
            this.mnuDeptOutp.Size = new System.Drawing.Size(124, 22);
            this.mnuDeptOutp.Text = "�������";
            this.mnuDeptOutp.Click += new System.EventHandler(this.mnuDeptOutp_Click);
            // 
            // mnuDeptNurse
            // 
            this.mnuDeptNurse.Name = "mnuDeptNurse";
            this.mnuDeptNurse.Size = new System.Drawing.Size(124, 22);
            this.mnuDeptNurse.Text = "����Ԫ";
            this.mnuDeptNurse.Click += new System.EventHandler(this.mnuDeptNurse_Click);
            // 
            // mnuDeptOther
            // 
            this.mnuDeptOther.Name = "mnuDeptOther";
            this.mnuDeptOther.Size = new System.Drawing.Size(124, 22);
            this.mnuDeptOther.Text = "��������";
            this.mnuDeptOther.Click += new System.EventHandler(this.mnuDeptOther_Click);
            // 
            // RightManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 515);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnUserRightSelect);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvUserRight);
            this.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "RightManageForm";
            this.Text = "�û�Ȩ�޹���";
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserRight)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.cmenuSelectUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.HerenButton btnSave;
        private Heren.Common.Controls.HerenButton btnSelectAll;
        private Heren.Common.Controls.TableView.DataTableView dgvUserRight;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelModify;
        private Heren.Common.Controls.FlatButton btnUserRightSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private Heren.Common.Controls.TextBoxButton txtUserName;
        private System.Windows.Forms.GroupBox groupBox2;
        private Heren.Common.Controls.TextBoxButton txtUserID;
        private System.Windows.Forms.GroupBox groupBox1;
        private Heren.Common.Controls.DictInput.FindComboBox cboDeptList;
        private Heren.Common.Controls.FlatButton btnQueryByDept;
        private System.Windows.Forms.ToolStripMenuItem mnuModifyUserRight;
        private System.Windows.Forms.ContextMenuStrip cmenuSelectUser;
        private System.Windows.Forms.ToolStripMenuItem mnuDeptAll;
        private System.Windows.Forms.ToolStripMenuItem mnuDeptWard;
        private System.Windows.Forms.ToolStripMenuItem mnuDeptOutp;
        private System.Windows.Forms.ToolStripMenuItem mnuDeptNurse;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuDeptOther;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptName;
        private System.Windows.Forms.ToolStripMenuItem mnuResetUserPassWord;
    }
}