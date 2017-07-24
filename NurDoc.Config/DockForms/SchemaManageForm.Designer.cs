namespace Heren.NurDoc.Config.DockForms
{
    partial class SchemaManageForm
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
            this.cmenuEditSchema = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnnCancelModify = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.colSchemaID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchemaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchemaFlag = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colWardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchRelativeSchemaId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.cmenuEditSchema.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSchemaID,
            this.colSchemaName,
            this.colSchemaFlag,
            this.colWardName,
            this.colCreateTime,
            this.colColumns,
            this.colBatchRelativeSchemaId});
            this.dataTableView1.ContextMenuStrip = this.cmenuEditSchema;
            this.dataTableView1.Location = new System.Drawing.Point(2, 2);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(730, 433);
            this.dataTableView1.TabIndex = 1;
            this.dataTableView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataTableView1_CellBeginEdit);
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            // 
            // cmenuEditSchema
            // 
            this.cmenuEditSchema.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnnCancelModify,
            this.mnuCancelDelete,
            this.mnuDeleteRecord});
            this.cmenuEditSchema.Name = "contextMenuStrip1";
            this.cmenuEditSchema.Size = new System.Drawing.Size(198, 92);
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
            // colSchemaID
            // 
            this.colSchemaID.FillWeight = 120F;
            this.colSchemaID.HeaderText = "方案编号";
            this.colSchemaID.Name = "colSchemaID";
            this.colSchemaID.Width = 120;
            // 
            // colSchemaName
            // 
            this.colSchemaName.HeaderText = "方案名称";
            this.colSchemaName.Name = "colSchemaName";
            // 
            // colSchemaFlag
            // 
            this.colSchemaFlag.HeaderText = "方案标记";
            this.colSchemaFlag.Name = "colSchemaFlag";
            this.colSchemaFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSchemaFlag.Width = 140;
            // 
            // colWardName
            // 
            this.colWardName.FillWeight = 120F;
            this.colWardName.HeaderText = "所属科室";
            this.colWardName.Name = "colWardName";
            this.colWardName.Width = 120;
            // 
            // colCreateTime
            // 
            this.colCreateTime.FillWeight = 120F;
            this.colCreateTime.HeaderText = "创建时间";
            this.colCreateTime.Name = "colCreateTime";
            this.colCreateTime.Width = 120;
            // 
            // colColumns
            // 
            this.colColumns.FillWeight = 500F;
            this.colColumns.HeaderText = "包含的列";
            this.colColumns.Name = "colColumns";
            this.colColumns.Width = 500;
            // 
            // colBatchRelativeSchemaId
            // 
            this.colBatchRelativeSchemaId.FillWeight = 160F;
            this.colBatchRelativeSchemaId.HeaderText = "批量入录关联护理配置";
            this.colBatchRelativeSchemaId.Name = "colBatchRelativeSchemaId";
            this.colBatchRelativeSchemaId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colBatchRelativeSchemaId.Visible = false;
            this.colBatchRelativeSchemaId.Width = 160;
            // 
            // SchemaManageForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 478);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "SchemaManageForm";
            this.Text = "列方案管理窗口";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.cmenuEditSchema.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ContextMenuStrip cmenuEditSchema;
        private System.Windows.Forms.ToolStripMenuItem mnnCancelModify;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteRecord;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchemaID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchemaName;
        private Heren.Common.Controls.TableView.ComboBoxColumn colSchemaFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWardName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchRelativeSchemaId;
    }
}