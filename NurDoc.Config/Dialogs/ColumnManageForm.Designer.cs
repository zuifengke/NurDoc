namespace Heren.NurDoc.Config.Dialogs
{
    partial class ColumnManageForm
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
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.cmenuEditColumn = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnnCancelModify = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMoveUp = new Heren.Common.Controls.FlatButton();
            this.btnMoveDown = new Heren.Common.Controls.FlatButton();
            this.colColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumnWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsMiddle = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsPrint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colColumnTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumnGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumnType = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colColumnUnit = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colColumnItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.cmenuEditColumn.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colColumnName,
            this.colColumnWidth,
            this.colIsMiddle,
            this.colIsVisible,
            this.colIsPrint,
            this.colColumnTag,
            this.colColumnGroup,
            this.colColumnType,
            this.colColumnUnit,
            this.colColumnItems,
            this.colDocType});
            this.dataTableView1.ContextMenuStrip = this.cmenuEditColumn;
            this.dataTableView1.Location = new System.Drawing.Point(3, 3);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(878, 379);
            this.dataTableView1.TabIndex = 0;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            this.dataTableView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellEndEdit);
            // 
            // cmenuEditColumn
            // 
            this.cmenuEditColumn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnnCancelModify,
            this.mnuCancelDelete,
            this.mnuDeleteRecord});
            this.cmenuEditColumn.Name = "contextMenuStrip2";
            this.cmenuEditColumn.Size = new System.Drawing.Size(213, 92);
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(212, 22);
            this.mnuNew.Text = "新增";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnnCancelModify
            // 
            this.mnnCancelModify.Name = "mnnCancelModify";
            this.mnnCancelModify.Size = new System.Drawing.Size(212, 22);
            this.mnnCancelModify.Text = "取消修改";
            this.mnnCancelModify.Click += new System.EventHandler(this.mnnCancelModify_Click);
            // 
            // mnuCancelDelete
            // 
            this.mnuCancelDelete.Name = "mnuCancelDelete";
            this.mnuCancelDelete.Size = new System.Drawing.Size(212, 22);
            this.mnuCancelDelete.Text = "取消删除";
            this.mnuCancelDelete.Click += new System.EventHandler(this.mnuCancelDelete_Click);
            // 
            // mnuDeleteRecord
            // 
            this.mnuDeleteRecord.Name = "mnuDeleteRecord";
            this.mnuDeleteRecord.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteRecord.Size = new System.Drawing.Size(212, 22);
            this.mnuDeleteRecord.Text = "删除选中项";
            this.mnuDeleteRecord.Click += new System.EventHandler(this.mnuDeleteRecord_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(697, 395);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(793, 395);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(15, 395);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMoveUp.Location = new System.Drawing.Point(96, 397);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(20, 20);
            this.btnMoveUp.TabIndex = 18;
            this.btnMoveUp.ToolTipText = null;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMoveDown.Location = new System.Drawing.Point(120, 397);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(20, 20);
            this.btnMoveDown.TabIndex = 17;
            this.btnMoveDown.ToolTipText = null;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // colColumnName
            // 
            this.colColumnName.FillWeight = 120F;
            this.colColumnName.HeaderText = "列名(10字内)";
            this.colColumnName.Name = "colColumnName";
            this.colColumnName.Width = 120;
            // 
            // colColumnWidth
            // 
            this.colColumnWidth.FillWeight = 40F;
            this.colColumnWidth.HeaderText = "宽度";
            this.colColumnWidth.Name = "colColumnWidth";
            this.colColumnWidth.Width = 40;
            // 
            // colIsMiddle
            // 
            this.colIsMiddle.FillWeight = 36F;
            this.colIsMiddle.HeaderText = "居中";
            this.colIsMiddle.Name = "colIsMiddle";
            this.colIsMiddle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsMiddle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colIsMiddle.Width = 36;
            // 
            // colIsVisible
            // 
            this.colIsVisible.FillWeight = 36F;
            this.colIsVisible.HeaderText = "可见";
            this.colIsVisible.Name = "colIsVisible";
            this.colIsVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colIsVisible.Width = 36;
            // 
            // colIsPrint
            // 
            this.colIsPrint.FillWeight = 36F;
            this.colIsPrint.HeaderText = "打印";
            this.colIsPrint.Name = "colIsPrint";
            this.colIsPrint.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsPrint.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colIsPrint.Width = 36;
            // 
            // colColumnTag
            // 
            this.colColumnTag.FillWeight = 110F;
            this.colColumnTag.HeaderText = "数据标识(10字内)";
            this.colColumnTag.Name = "colColumnTag";
            this.colColumnTag.Width = 110;
            // 
            // colColumnGroup
            // 
            this.colColumnGroup.HeaderText = "列组名(10字内)";
            this.colColumnGroup.Name = "colColumnGroup";
            // 
            // colColumnType
            // 
            this.colColumnType.FillWeight = 72F;
            this.colColumnType.HeaderText = "数据类型";
            this.colColumnType.Name = "colColumnType";
            this.colColumnType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colColumnType.Width = 72;
            // 
            // colColumnUnit
            // 
            this.colColumnUnit.FillWeight = 64F;
            this.colColumnUnit.HeaderText = "数据单位";
            this.colColumnUnit.Name = "colColumnUnit";
            this.colColumnUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colColumnUnit.Width = 64;
            // 
            // colColumnItems
            // 
            this.colColumnItems.FillWeight = 150F;
            this.colColumnItems.HeaderText = "可选项列表";
            this.colColumnItems.Name = "colColumnItems";
            this.colColumnItems.Width = 150;
            // 
            // colDocType
            // 
            this.colDocType.HeaderText = "关联表单类型";
            this.colDocType.Name = "colDocType";
            this.colDocType.ReadOnly = true;
            this.colDocType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDocType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDocType.Width = 150;
            // 
            // ColumnManageForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(884, 430);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColumnManageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑方案包含的列";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.cmenuEditColumn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ContextMenuStrip cmenuEditColumn;
        private System.Windows.Forms.ToolStripMenuItem mnnCancelModify;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteRecord;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private Heren.Common.Controls.FlatButton btnMoveUp;
        private Heren.Common.Controls.FlatButton btnMoveDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumnWidth;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsMiddle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsVisible;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsPrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumnTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumnGroup;
        private Heren.Common.Controls.TableView.ComboBoxColumn colColumnType;
        private Heren.Common.Controls.TableView.ComboBoxColumn colColumnUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumnItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocType;
    }
}