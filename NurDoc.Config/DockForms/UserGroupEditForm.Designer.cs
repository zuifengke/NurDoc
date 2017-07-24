namespace Heren.NurDoc.Config.DockForms
{
    partial class UserGroupEditForm
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
            this.components = new System.ComponentModel.Container();
            this.gbxGroups = new System.Windows.Forms.GroupBox();
            this.dgvGroups = new Heren.Common.Controls.TableView.DataTableView();
            this.colGroupCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmenuGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnnCancelModifyGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDeleteGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddGroup = new Heren.Common.Controls.FlatButton();
            this.gbxUsers = new System.Windows.Forms.GroupBox();
            this.dgvUsers = new Heren.Common.Controls.TableView.DataTableView();
            this.colUserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmenuUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCancelModifyUser = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDeleteUser = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteUser = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddUser = new Heren.Common.Controls.FlatButton();
            this.btnSave = new Heren.Common.Controls.HerenButton();
            this.gbxGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).BeginInit();
            this.cmenuGroup.SuspendLayout();
            this.gbxUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.cmenuUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxGroups
            // 
            this.gbxGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxGroups.Controls.Add(this.dgvGroups);
            this.gbxGroups.Controls.Add(this.btnAddGroup);
            this.gbxGroups.Location = new System.Drawing.Point(5, 6);
            this.gbxGroups.Name = "gbxGroups";
            this.gbxGroups.Size = new System.Drawing.Size(329, 476);
            this.gbxGroups.TabIndex = 0;
            this.gbxGroups.TabStop = false;
            this.gbxGroups.Text = "用户组";
            // 
            // dgvGroups
            // 
            this.dgvGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGroups.ColumnHeadersHeight = 24;
            this.dgvGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroupCode,
            this.colGroupName});
            this.dgvGroups.ContextMenuStrip = this.cmenuGroup;
            this.dgvGroups.Location = new System.Drawing.Point(10, 20);
            this.dgvGroups.Name = "dgvGroups";
            this.dgvGroups.Size = new System.Drawing.Size(278, 448);
            this.dgvGroups.TabIndex = 1;
            this.dgvGroups.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroups_CellClick);
            this.dgvGroups.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroups_CellValueChanged);
            this.dgvGroups.SelectionChanged += new System.EventHandler(this.dgvGroups_SelectionChanged);
            // 
            // colGroupCode
            // 
            this.colGroupCode.HeaderText = "组代码";
            this.colGroupCode.Name = "colGroupCode";
            this.colGroupCode.Width = 64;
            // 
            // colGroupName
            // 
            this.colGroupName.HeaderText = "组名称";
            this.colGroupName.Name = "colGroupName";
            this.colGroupName.Width = 150;
            // 
            // cmenuGroup
            // 
            this.cmenuGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnnCancelModifyGroup,
            this.mnuCancelDeleteGroup,
            this.mnuDeleteGroup});
            this.cmenuGroup.Name = "contextMenuStrip1";
            this.cmenuGroup.Size = new System.Drawing.Size(190, 70);
            // 
            // mnnCancelModifyGroup
            // 
            this.mnnCancelModifyGroup.Name = "mnnCancelModifyGroup";
            this.mnnCancelModifyGroup.Size = new System.Drawing.Size(189, 22);
            this.mnnCancelModifyGroup.Text = "取消修改";
            this.mnnCancelModifyGroup.Click += new System.EventHandler(this.mnnCancelModifyGroup_Click);
            // 
            // mnuCancelDeleteGroup
            // 
            this.mnuCancelDeleteGroup.Name = "mnuCancelDeleteGroup";
            this.mnuCancelDeleteGroup.Size = new System.Drawing.Size(189, 22);
            this.mnuCancelDeleteGroup.Text = "取消删除";
            this.mnuCancelDeleteGroup.Click += new System.EventHandler(this.mnuCancelDeleteGroup_Click);
            // 
            // mnuDeleteGroup
            // 
            this.mnuDeleteGroup.Name = "mnuDeleteGroup";
            this.mnuDeleteGroup.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteGroup.Size = new System.Drawing.Size(189, 22);
            this.mnuDeleteGroup.Text = "删除选中项";
            this.mnuDeleteGroup.Click += new System.EventHandler(this.mnuDeleteGroup_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGroup.Location = new System.Drawing.Point(295, 45);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(28, 27);
            this.btnAddGroup.TabIndex = 2;
            this.btnAddGroup.ToolTipText = null;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // gbxUsers
            // 
            this.gbxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxUsers.Controls.Add(this.dgvUsers);
            this.gbxUsers.Controls.Add(this.btnAddUser);
            this.gbxUsers.Location = new System.Drawing.Point(340, 6);
            this.gbxUsers.Name = "gbxUsers";
            this.gbxUsers.Size = new System.Drawing.Size(488, 476);
            this.gbxUsers.TabIndex = 3;
            this.gbxUsers.TabStop = false;
            this.gbxUsers.Text = "组用户列表";
            // 
            // dgvUsers
            // 
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.ColumnHeadersHeight = 24;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserID,
            this.colUserName,
            this.colDeptName,
            this.colPriority});
            this.dgvUsers.ContextMenuStrip = this.cmenuUser;
            this.dgvUsers.Location = new System.Drawing.Point(10, 20);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.Size = new System.Drawing.Size(437, 448);
            this.dgvUsers.TabIndex = 4;
            // 
            // colUserID
            // 
            this.colUserID.HeaderText = "用户ID";
            this.colUserID.Name = "colUserID";
            this.colUserID.ReadOnly = true;
            // 
            // colUserName
            // 
            this.colUserName.HeaderText = "用户姓名";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            this.colUserName.Width = 130;
            // 
            // colDeptName
            // 
            this.colDeptName.HeaderText = "科室名称";
            this.colDeptName.Name = "colDeptName";
            this.colDeptName.ReadOnly = true;
            this.colDeptName.Width = 200;
            // 
            // colPriority
            // 
            this.colPriority.HeaderText = "优先级";
            this.colPriority.Name = "colPriority";
            this.colPriority.Width = 64;
            // 
            // cmenuUser
            // 
            this.cmenuUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCancelModifyUser,
            this.mnuCancelDeleteUser,
            this.mnuDeleteUser});
            this.cmenuUser.Name = "contextMenuStrip1";
            this.cmenuUser.Size = new System.Drawing.Size(190, 70);
            // 
            // mnuCancelModifyUser
            // 
            this.mnuCancelModifyUser.Name = "mnuCancelModifyUser";
            this.mnuCancelModifyUser.Size = new System.Drawing.Size(189, 22);
            this.mnuCancelModifyUser.Text = "取消修改";
            this.mnuCancelModifyUser.Click += new System.EventHandler(this.mnuCancelModifyUser_Click);
            // 
            // mnuCancelDeleteUser
            // 
            this.mnuCancelDeleteUser.Name = "mnuCancelDeleteUser";
            this.mnuCancelDeleteUser.Size = new System.Drawing.Size(189, 22);
            this.mnuCancelDeleteUser.Text = "取消删除";
            this.mnuCancelDeleteUser.Click += new System.EventHandler(this.mnuCancelDeleteUser_Click);
            // 
            // mnuDeleteUser
            // 
            this.mnuDeleteUser.Name = "mnuDeleteUser";
            this.mnuDeleteUser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteUser.Size = new System.Drawing.Size(189, 22);
            this.mnuDeleteUser.Text = "删除选中项";
            this.mnuDeleteUser.Click += new System.EventHandler(this.mnuDeleteUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUser.Location = new System.Drawing.Point(453, 45);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(28, 27);
            this.btnAddUser.TabIndex = 5;
            this.btnAddUser.ToolTipText = null;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(705, 488);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // UserGroupEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 520);
            this.Controls.Add(this.gbxGroups);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbxUsers);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserGroupEditForm";
            this.Text = "用户组配置";
            this.gbxGroups.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).EndInit();
            this.cmenuGroup.ResumeLayout(false);
            this.gbxUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.cmenuUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxGroups;
        private Heren.Common.Controls.TableView.DataTableView dgvGroups;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroupCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroupName;
        private Heren.Common.Controls.FlatButton btnAddGroup;
        private System.Windows.Forms.GroupBox gbxUsers;
        private Heren.Common.Controls.TableView.DataTableView dgvUsers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPriority;
        private Heren.Common.Controls.FlatButton btnAddUser;
        private Heren.Common.Controls.HerenButton btnSave;
        private System.Windows.Forms.ContextMenuStrip cmenuGroup;
        private System.Windows.Forms.ToolStripMenuItem mnnCancelModifyGroup;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDeleteGroup;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteGroup;
        private System.Windows.Forms.ContextMenuStrip cmenuUser;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelModifyUser;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDeleteUser;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteUser;
    }
}