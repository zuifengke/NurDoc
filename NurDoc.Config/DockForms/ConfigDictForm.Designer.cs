namespace Heren.NurDoc.Config.DockForms
{
    partial class ConfigDictForm
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
            this.colConfigGroup = new Heren.Common.Controls.TableView.FindComboBoxColumn();
            this.colConfigName = new Heren.Common.Controls.TableView.FindComboBoxColumn();
            this.colConfigValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfigDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnnCancelModify = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancelDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new Heren.Common.Controls.HerenButton();
            this.btnCommit = new Heren.Common.Controls.HerenButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colConfigGroup,
            this.colConfigName,
            this.colConfigValue,
            this.colConfigDesc});
            this.dataTableView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataTableView1.Location = new System.Drawing.Point(1, 2);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(662, 407);
            this.dataTableView1.TabIndex = 0;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            // 
            // colConfigGroup
            // 
            this.colConfigGroup.FillWeight = 140F;
            this.colConfigGroup.HeaderText = "配置组";
            this.colConfigGroup.Name = "colConfigGroup";
            this.colConfigGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colConfigGroup.Width = 140;
            // 
            // colConfigName
            // 
            this.colConfigName.FillWeight = 240F;
            this.colConfigName.HeaderText = "配置项";
            this.colConfigName.Name = "colConfigName";
            this.colConfigName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colConfigName.Width = 240;
            // 
            // colConfigValue
            // 
            this.colConfigValue.FillWeight = 300F;
            this.colConfigValue.HeaderText = "配置值";
            this.colConfigValue.Name = "colConfigValue";
            this.colConfigValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colConfigValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colConfigValue.Width = 300;
            // 
            // colConfigDesc
            // 
            this.colConfigDesc.FillWeight = 400F;
            this.colConfigDesc.HeaderText = "配置描述";
            this.colConfigDesc.Name = "colConfigDesc";
            this.colConfigDesc.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colConfigDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colConfigDesc.Width = 400;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnnCancelModify,
            this.mnuCancelDelete,
            this.mnuDeleteRecord});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(216, 70);
            // 
            // mnnCancelModify
            // 
            this.mnnCancelModify.Name = "mnnCancelModify";
            this.mnnCancelModify.Size = new System.Drawing.Size(215, 22);
            this.mnnCancelModify.Text = "取消修改";
            this.mnnCancelModify.Click += new System.EventHandler(this.mnnCancelModify_Click);
            // 
            // mnuCancelDelete
            // 
            this.mnuCancelDelete.Name = "mnuCancelDelete";
            this.mnuCancelDelete.Size = new System.Drawing.Size(215, 22);
            this.mnuCancelDelete.Text = "取消删除";
            this.mnuCancelDelete.Click += new System.EventHandler(this.mnuCancelDelete_Click);
            // 
            // mnuDeleteRecord
            // 
            this.mnuDeleteRecord.Name = "mnuDeleteRecord";
            this.mnuDeleteRecord.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDeleteRecord.Size = new System.Drawing.Size(215, 22);
            this.mnuDeleteRecord.Text = "删除选中项";
            this.mnuDeleteRecord.Click += new System.EventHandler(this.mnuDeleteRecord_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(12, 420);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 25);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommit.Location = new System.Drawing.Point(565, 420);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(88, 25);
            this.btnCommit.TabIndex = 2;
            this.btnCommit.Text = "保存";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // ConfigDictForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 454);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.dataTableView1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ConfigDictForm";
            this.Text = "后台配置管理";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private Heren.Common.Controls.HerenButton btnAdd;
        private Heren.Common.Controls.HerenButton btnCommit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnnCancelModify;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteRecord;
        private Heren.Common.Controls.TableView.FindComboBoxColumn colConfigGroup;
        private Heren.Common.Controls.TableView.FindComboBoxColumn colConfigName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfigValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfigDesc;
    }
}