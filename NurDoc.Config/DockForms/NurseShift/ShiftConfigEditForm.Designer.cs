namespace Heren.NurDoc.Config.DockForms
{
    partial class ShiftConfigEditForm
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmenuShiftItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnnCancelModify = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMiddle = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colImportant = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colWardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmenuShiftItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(12, 445);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 25);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(634, 445);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 25);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmenuShiftItem
            // 
            this.cmenuShiftItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnnCancelModify,
            this.mnuCancelDelete,
            this.mnuDeleteRecord});
            this.cmenuShiftItem.Name = "contextMenuStrip1";
            this.cmenuShiftItem.Size = new System.Drawing.Size(198, 92);
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(197, 22);
            this.mnuNew.Text = "新增";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnnCancelModify
            // 
            this.mnnCancelModify.Name = "mnnCancelModify";
            this.mnnCancelModify.Size = new System.Drawing.Size(197, 22);
            this.mnnCancelModify.Text = "取消修改";
            this.mnnCancelModify.Click += new System.EventHandler(this.mnnCancelModify_Click);
            // 
            // mnuCancelDelete
            // 
            this.mnuCancelDelete.Name = "mnuCancelDelete";
            this.mnuCancelDelete.Size = new System.Drawing.Size(197, 22);
            this.mnuCancelDelete.Text = "取消删除";
            this.mnuCancelDelete.Click += new System.EventHandler(this.mnuCancelDelete_Click);
            // 
            // mnuDeleteRecord
            // 
            this.mnuDeleteRecord.Name = "mnuDeleteRecord";
            this.mnuDeleteRecord.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteRecord.Size = new System.Drawing.Size(197, 22);
            this.mnuDeleteRecord.Text = "删除选中项";
            this.mnuDeleteRecord.Click += new System.EventHandler(this.mnuDeleteRecord_Click);
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemNo,
            this.colItemCode,
            this.colItemName,
            this.colWidth,
            this.colVisible,
            this.colMiddle,
            this.colImportant,
            this.colWardName});
            this.dataTableView1.ContextMenuStrip = this.cmenuShiftItem;
            this.dataTableView1.Location = new System.Drawing.Point(2, 2);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(729, 433);
            this.dataTableView1.TabIndex = 1;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            // 
            // colItemNo
            // 
            this.colItemNo.HeaderText = "序号";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.Width = 30;
            // 
            // colItemCode
            // 
            this.colItemCode.HeaderText = "项目编码";
            this.colItemCode.Name = "colItemCode";
            this.colItemCode.Width = 60;
            // 
            // colItemName
            // 
            this.colItemName.FillWeight = 140F;
            this.colItemName.HeaderText = "项目名称";
            this.colItemName.Name = "colItemName";
            this.colItemName.Width = 140;
            // 
            // colWidth
            // 
            this.colWidth.HeaderText = "宽度";
            this.colWidth.Name = "colWidth";
            // 
            // colVisible
            // 
            this.colVisible.HeaderText = "可见";
            this.colVisible.Name = "colVisible";
            this.colVisible.Width = 40;
            // 
            // colMiddle
            // 
            this.colMiddle.HeaderText = "居中";
            this.colMiddle.Name = "colMiddle";
            this.colMiddle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMiddle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colMiddle.Width = 40;
            // 
            // colImportant
            // 
            this.colImportant.HeaderText = "重要项目";
            this.colImportant.Name = "colImportant";
            this.colImportant.Width = 60;
            // 
            // colWardName
            // 
            this.colWardName.FillWeight = 140F;
            this.colWardName.HeaderText = "所属科室";
            this.colWardName.Name = "colWardName";
            this.colWardName.ReadOnly = true;
            this.colWardName.Width = 140;
            // 
            // ShiftConfigEditForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 478);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ShiftConfigEditForm";
            this.Text = "交班动态配置";
            this.cmenuShiftItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ContextMenuStrip cmenuShiftItem;
        private System.Windows.Forms.ToolStripMenuItem mnnCancelModify;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteRecord;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMiddle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colImportant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWardName;
    }
}