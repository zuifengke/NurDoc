namespace Heren.NurDoc.Config.DockForms
{
    partial class CarePlanEditForm
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
            this.gbxDiag = new System.Windows.Forms.GroupBox();
            this.dgvDiag = new Heren.Common.Controls.TableView.DataTableView();
            this.colDiagNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiagCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiagName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmenuDiag = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnnCancelModifyDiag = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDeleteDiag = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteDiag = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddDiag = new Heren.Common.Controls.FlatButton();
            this.gbxItems = new System.Windows.Forms.GroupBox();
            this.dgvItems = new Heren.Common.Controls.TableView.DataTableView();
            this.colItemType = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmenuItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCancelModifyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddItem = new Heren.Common.Controls.FlatButton();
            this.btnSave = new Heren.Common.Controls.HerenButton();
            this.gbxDiag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiag)).BeginInit();
            this.cmenuDiag.SuspendLayout();
            this.gbxItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.cmenuItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxDiag
            // 
            this.gbxDiag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxDiag.Controls.Add(this.dgvDiag);
            this.gbxDiag.Controls.Add(this.btnAddDiag);
            this.gbxDiag.Location = new System.Drawing.Point(5, 6);
            this.gbxDiag.Name = "gbxDiag";
            this.gbxDiag.Size = new System.Drawing.Size(376, 476);
            this.gbxDiag.TabIndex = 0;
            this.gbxDiag.TabStop = false;
            this.gbxDiag.Text = "护理诊断";
            // 
            // dgvDiag
            // 
            this.dgvDiag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiag.ColumnHeadersHeight = 24;
            this.dgvDiag.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDiagNo,
            this.colDiagCode,
            this.colDiagName});
            this.dgvDiag.ContextMenuStrip = this.cmenuDiag;
            this.dgvDiag.Location = new System.Drawing.Point(10, 20);
            this.dgvDiag.Name = "dgvDiag";
            this.dgvDiag.Size = new System.Drawing.Size(325, 448);
            this.dgvDiag.TabIndex = 1;
            this.dgvDiag.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiags_CellClick);
            this.dgvDiag.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiags_CellValueChanged);
            this.dgvDiag.SelectionChanged += new System.EventHandler(this.dgvDiags_SelectionChanged);
            // 
            // colDiagNo
            // 
            this.colDiagNo.FillWeight = 48F;
            this.colDiagNo.HeaderText = "序号";
            this.colDiagNo.Name = "colDiagNo";
            this.colDiagNo.Width = 48;
            // 
            // colDiagCode
            // 
            this.colDiagCode.HeaderText = "诊断代码";
            this.colDiagCode.Name = "colDiagCode";
            this.colDiagCode.Width = 64;
            // 
            // colDiagName
            // 
            this.colDiagName.HeaderText = "诊断名称";
            this.colDiagName.Name = "colDiagName";
            this.colDiagName.Width = 150;
            // 
            // cmenuDiag
            // 
            this.cmenuDiag.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnnCancelModifyDiag,
            this.mnuCancelDeleteDiag,
            this.mnuDeleteDiag});
            this.cmenuDiag.Name = "contextMenuStrip1";
            this.cmenuDiag.Size = new System.Drawing.Size(190, 70);
            // 
            // mnnCancelModifyDiag
            // 
            this.mnnCancelModifyDiag.Name = "mnnCancelModifyDiag";
            this.mnnCancelModifyDiag.Size = new System.Drawing.Size(189, 22);
            this.mnnCancelModifyDiag.Text = "取消修改";
            this.mnnCancelModifyDiag.Click += new System.EventHandler(this.mnnCancelModifyDiag_Click);
            // 
            // mnuCancelDeleteDiag
            // 
            this.mnuCancelDeleteDiag.Name = "mnuCancelDeleteDiag";
            this.mnuCancelDeleteDiag.Size = new System.Drawing.Size(189, 22);
            this.mnuCancelDeleteDiag.Text = "取消删除";
            this.mnuCancelDeleteDiag.Click += new System.EventHandler(this.mnuCancelDeleteDiag_Click);
            // 
            // mnuDeleteDiag
            // 
            this.mnuDeleteDiag.Name = "mnuDeleteDiag";
            this.mnuDeleteDiag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteDiag.Size = new System.Drawing.Size(189, 22);
            this.mnuDeleteDiag.Text = "删除选中项";
            this.mnuDeleteDiag.Click += new System.EventHandler(this.mnuDeleteDiag_Click);
            // 
            // btnAddDiag
            // 
            this.btnAddDiag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDiag.Location = new System.Drawing.Point(342, 45);
            this.btnAddDiag.Name = "btnAddDiag";
            this.btnAddDiag.Size = new System.Drawing.Size(28, 27);
            this.btnAddDiag.TabIndex = 2;
            this.btnAddDiag.ToolTipText = null;
            this.btnAddDiag.Click += new System.EventHandler(this.btnAddDiag_Click);
            // 
            // gbxItems
            // 
            this.gbxItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxItems.Controls.Add(this.dgvItems);
            this.gbxItems.Controls.Add(this.btnAddItem);
            this.gbxItems.Location = new System.Drawing.Point(387, 6);
            this.gbxItems.Name = "gbxItems";
            this.gbxItems.Size = new System.Drawing.Size(441, 476);
            this.gbxItems.TabIndex = 3;
            this.gbxItems.TabStop = false;
            this.gbxItems.Text = "诊断护理计划";
            // 
            // dgvItems
            // 
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.ColumnHeadersHeight = 24;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemType,
            this.colItemNo,
            this.colItemCode,
            this.colItemName});
            this.dgvItems.ContextMenuStrip = this.cmenuItems;
            this.dgvItems.Location = new System.Drawing.Point(10, 20);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(390, 448);
            this.dgvItems.TabIndex = 4;
            this.dgvItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellValueChanged);
            // 
            // colItemType
            // 
            this.colItemType.DisplayStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colItemType.HeaderText = "类别";
            this.colItemType.Name = "colItemType";
            this.colItemType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colItemNo
            // 
            this.colItemNo.FillWeight = 48F;
            this.colItemNo.HeaderText = "序号";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.Width = 48;
            // 
            // colItemCode
            // 
            this.colItemCode.FillWeight = 64F;
            this.colItemCode.HeaderText = "项目代码";
            this.colItemCode.Name = "colItemCode";
            this.colItemCode.Width = 64;
            // 
            // colItemName
            // 
            this.colItemName.FillWeight = 600F;
            this.colItemName.HeaderText = "项目内容";
            this.colItemName.Name = "colItemName";
            this.colItemName.Width = 600;
            // 
            // cmenuItems
            // 
            this.cmenuItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCancelModifyItem,
            this.mnuCancelDeleteItem,
            this.mnuDeleteItem});
            this.cmenuItems.Name = "contextMenuStrip1";
            this.cmenuItems.Size = new System.Drawing.Size(190, 70);
            // 
            // mnuCancelModifyItem
            // 
            this.mnuCancelModifyItem.Name = "mnuCancelModifyItem";
            this.mnuCancelModifyItem.Size = new System.Drawing.Size(189, 22);
            this.mnuCancelModifyItem.Text = "取消修改";
            this.mnuCancelModifyItem.Click += new System.EventHandler(this.mnuCancelModifyItem_Click);
            // 
            // mnuCancelDeleteItem
            // 
            this.mnuCancelDeleteItem.Name = "mnuCancelDeleteItem";
            this.mnuCancelDeleteItem.Size = new System.Drawing.Size(189, 22);
            this.mnuCancelDeleteItem.Text = "取消删除";
            this.mnuCancelDeleteItem.Click += new System.EventHandler(this.mnuCancelDeleteItem_Click);
            // 
            // mnuDeleteItem
            // 
            this.mnuDeleteItem.Name = "mnuDeleteItem";
            this.mnuDeleteItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteItem.Size = new System.Drawing.Size(189, 22);
            this.mnuDeleteItem.Text = "删除选中项";
            this.mnuDeleteItem.Click += new System.EventHandler(this.mnuDeleteItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.Location = new System.Drawing.Point(406, 45);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(28, 27);
            this.btnAddItem.TabIndex = 5;
            this.btnAddItem.ToolTipText = null;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(712, 490);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // CarePlanEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 520);
            this.Controls.Add(this.gbxItems);
            this.Controls.Add(this.gbxDiag);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "CarePlanEditForm";
            this.Text = "护理计划配置";
            this.gbxDiag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiag)).EndInit();
            this.cmenuDiag.ResumeLayout(false);
            this.gbxItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.cmenuItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxDiag;
        private Heren.Common.Controls.TableView.DataTableView dgvDiag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiagNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiagCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiagName;
        private Heren.Common.Controls.FlatButton btnAddDiag;
        private System.Windows.Forms.GroupBox gbxItems;
        private Heren.Common.Controls.TableView.DataTableView dgvItems;
        private Heren.Common.Controls.TableView.ComboBoxColumn colItemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private Heren.Common.Controls.FlatButton btnAddItem;
        private Heren.Common.Controls.HerenButton btnSave;
        private System.Windows.Forms.ContextMenuStrip cmenuDiag;
        private System.Windows.Forms.ToolStripMenuItem mnnCancelModifyDiag;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDeleteDiag;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteDiag;
        private System.Windows.Forms.ContextMenuStrip cmenuItems;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelModifyItem;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDeleteItem;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteItem;
    }
}