namespace Heren.NurDoc.Config.DockForms
{
    partial class CommonDictForm
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
            this.gbxDicts = new System.Windows.Forms.GroupBox();
            this.dgvDicts = new Heren.Common.Controls.TableView.DataTableView();
            this.colDictName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbxItems = new System.Windows.Forms.GroupBox();
            this.dgvItems = new Heren.Common.Controls.TableView.DataTableView();
            this.colItemType = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmenuItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCancelModifyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddItem = new Heren.Common.Controls.FlatButton();
            this.btnSave = new Heren.Common.Controls.HerenButton();
            this.gbxDicts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDicts)).BeginInit();
            this.gbxItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.cmenuItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxDicts
            // 
            this.gbxDicts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxDicts.Controls.Add(this.dgvDicts);
            this.gbxDicts.Location = new System.Drawing.Point(5, 6);
            this.gbxDicts.Name = "gbxDicts";
            this.gbxDicts.Size = new System.Drawing.Size(233, 476);
            this.gbxDicts.TabIndex = 0;
            this.gbxDicts.TabStop = false;
            this.gbxDicts.Text = "字典类别";
            // 
            // dgvDicts
            // 
            this.dgvDicts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDicts.ColumnHeadersHeight = 24;
            this.dgvDicts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDictName});
            this.dgvDicts.Location = new System.Drawing.Point(10, 20);
            this.dgvDicts.Name = "dgvDicts";
            this.dgvDicts.Size = new System.Drawing.Size(215, 448);
            this.dgvDicts.TabIndex = 1;
            this.dgvDicts.SelectionChanged += new System.EventHandler(this.dgvDicts_SelectionChanged);
            // 
            // colDictName
            // 
            this.colDictName.HeaderText = "字典类别";
            this.colDictName.Name = "colDictName";
            this.colDictName.ReadOnly = true;
            this.colDictName.Width = 150;
            // 
            // gbxItems
            // 
            this.gbxItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxItems.Controls.Add(this.dgvItems);
            this.gbxItems.Controls.Add(this.btnAddItem);
            this.gbxItems.Location = new System.Drawing.Point(244, 6);
            this.gbxItems.Name = "gbxItems";
            this.gbxItems.Size = new System.Drawing.Size(584, 476);
            this.gbxItems.TabIndex = 3;
            this.gbxItems.TabStop = false;
            this.gbxItems.Text = "字典项目列表";
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
            this.colItemName,
            this.colWardName});
            this.dgvItems.ContextMenuStrip = this.cmenuItems;
            this.dgvItems.Location = new System.Drawing.Point(10, 20);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(533, 448);
            this.dgvItems.TabIndex = 4;
            this.dgvItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellDoubleClick);
            this.dgvItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellValueChanged);
            // 
            // colItemType
            // 
            this.colItemType.FillWeight = 140F;
            this.colItemType.HeaderText = "类别";
            this.colItemType.Name = "colItemType";
            this.colItemType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemType.Width = 140;
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
            this.colItemName.FillWeight = 500F;
            this.colItemName.HeaderText = "项目内容";
            this.colItemName.Name = "colItemName";
            this.colItemName.Width = 500;
            // 
            // colWardName
            // 
            this.colWardName.FillWeight = 150F;
            this.colWardName.HeaderText = "所属病区";
            this.colWardName.Name = "colWardName";
            this.colWardName.ReadOnly = true;
            this.colWardName.Width = 150;
            // 
            // cmenuItems
            // 
            this.cmenuItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCancelModifyItem,
            this.mnuCancelDeleteItem,
            this.mnuDeleteItem});
            this.cmenuItems.Name = "contextMenuStrip1";
            this.cmenuItems.Size = new System.Drawing.Size(216, 70);
            // 
            // mnuCancelModifyItem
            // 
            this.mnuCancelModifyItem.Name = "mnuCancelModifyItem";
            this.mnuCancelModifyItem.Size = new System.Drawing.Size(215, 22);
            this.mnuCancelModifyItem.Text = "取消修改";
            this.mnuCancelModifyItem.Click += new System.EventHandler(this.mnuCancelModifyItem_Click);
            // 
            // mnuCancelDeleteItem
            // 
            this.mnuCancelDeleteItem.Name = "mnuCancelDeleteItem";
            this.mnuCancelDeleteItem.Size = new System.Drawing.Size(215, 22);
            this.mnuCancelDeleteItem.Text = "取消删除";
            this.mnuCancelDeleteItem.Click += new System.EventHandler(this.mnuCancelDeleteItem_Click);
            // 
            // mnuDeleteItem
            // 
            this.mnuDeleteItem.Name = "mnuDeleteItem";
            this.mnuDeleteItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteItem.Size = new System.Drawing.Size(215, 22);
            this.mnuDeleteItem.Text = "删除选中项";
            this.mnuDeleteItem.Click += new System.EventHandler(this.mnuDeleteItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.Location = new System.Drawing.Point(549, 45);
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
            // CommonDictForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 520);
            this.Controls.Add(this.gbxItems);
            this.Controls.Add(this.gbxDicts);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "CommonDictForm";
            this.Text = "公共字典管理";
            this.gbxDicts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDicts)).EndInit();
            this.gbxItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.cmenuItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxDicts;
        private Heren.Common.Controls.TableView.DataTableView dgvDicts;
        private System.Windows.Forms.GroupBox gbxItems;
        private Heren.Common.Controls.TableView.DataTableView dgvItems;
        private Heren.Common.Controls.FlatButton btnAddItem;
        private Heren.Common.Controls.HerenButton btnSave;
        private System.Windows.Forms.ContextMenuStrip cmenuItems;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelModifyItem;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDeleteItem;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDictName;
        private Heren.Common.Controls.TableView.ComboBoxColumn colItemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWardName;
    }
}