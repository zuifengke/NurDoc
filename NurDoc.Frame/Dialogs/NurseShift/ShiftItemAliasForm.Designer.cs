namespace Heren.NurDoc.Frame.DockForms
{
    partial class ShiftItemAliasForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvSetShiftItemAlias = new Heren.Common.Controls.TableView.DataTableView();
            this.btnAddItemAlias = new Heren.Common.Controls.FlatButton();
            this.btnDeleteItemAlias = new Heren.Common.Controls.FlatButton();
            this.btnSave = new Heren.Common.Controls.FlatButton();
            this.colShiftItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShiftItemAliasCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShiftItemName = new Heren.Common.Controls.TableView.ComboBoxColumn();
            this.colShiftItemAlias = new Heren.Common.Forms.XTextBoxColumn();
            this.colWardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetShiftItemAlias)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSetShiftItemAlias
            // 
            this.dgvSetShiftItemAlias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSetShiftItemAlias.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSetShiftItemAlias.ColumnHeadersHeight = 24;
            this.dgvSetShiftItemAlias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colShiftItemCode,
            this.colShiftItemAliasCode,
            this.colShiftItemName,
            this.colShiftItemAlias,
            this.colWardName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSetShiftItemAlias.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSetShiftItemAlias.Location = new System.Drawing.Point(2, 1);
            this.dgvSetShiftItemAlias.MultiSelect = false;
            this.dgvSetShiftItemAlias.Name = "dgvSetShiftItemAlias";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSetShiftItemAlias.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSetShiftItemAlias.RowHeadersWidth = 30;
            this.dgvSetShiftItemAlias.Size = new System.Drawing.Size(372, 453);
            this.dgvSetShiftItemAlias.TabIndex = 1;
            this.dgvSetShiftItemAlias.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvSetShiftItemAlias_EditingControlShowing);
            // 
            // btnAddItemAlias
            // 
            this.btnAddItemAlias.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItemAlias.Location = new System.Drawing.Point(380, 5);
            this.btnAddItemAlias.Name = "btnAddItemAlias";
            this.btnAddItemAlias.Size = new System.Drawing.Size(22, 22);
            this.btnAddItemAlias.TabIndex = 3;
            this.btnAddItemAlias.ToolTipText = null;
            this.btnAddItemAlias.Click += new System.EventHandler(this.btnAddItemAlias_Click);
            // 
            // btnDeleteItemAlias
            // 
            this.btnDeleteItemAlias.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteItemAlias.Location = new System.Drawing.Point(380, 35);
            this.btnDeleteItemAlias.Name = "btnDeleteItemAlias";
            this.btnDeleteItemAlias.Size = new System.Drawing.Size(22, 22);
            this.btnDeleteItemAlias.TabIndex = 4;
            this.btnDeleteItemAlias.ToolTipText = null;
            this.btnDeleteItemAlias.Click += new System.EventHandler(this.btnDeleteItemAlias_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(380, 63);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(22, 22);
            this.btnSave.TabIndex = 5;
            this.btnSave.ToolTipText = null;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // colShiftItemCode
            // 
            this.colShiftItemCode.FillWeight = 50F;
            this.colShiftItemCode.HeaderText = "项目代码";
            this.colShiftItemCode.Name = "colShiftItemCode";
            this.colShiftItemCode.ReadOnly = true;
            this.colShiftItemCode.Visible = false;
            this.colShiftItemCode.Width = 70;
            // 
            // colShiftItemAliasCode
            // 
            this.colShiftItemAliasCode.HeaderText = "项目别名代码";
            this.colShiftItemAliasCode.Name = "colShiftItemAliasCode";
            this.colShiftItemAliasCode.Visible = false;
            // 
            // colShiftItemName
            // 
            this.colShiftItemName.DisplayStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colShiftItemName.FillWeight = 60F;
            this.colShiftItemName.HeaderText = "项目";
            this.colShiftItemName.Name = "colShiftItemName";
            this.colShiftItemName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colShiftItemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colShiftItemAlias
            // 
            this.colShiftItemAlias.HeaderText = "项目别名";
            this.colShiftItemAlias.Name = "colShiftItemAlias";
            // 
            // colWardName
            // 
            this.colWardName.FillWeight = 80F;
            this.colWardName.HeaderText = "所属病区";
            this.colWardName.Name = "colWardName";
            this.colWardName.ReadOnly = true;
            this.colWardName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWardName.Width = 120;
            // 
            // ShiftItemAliasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 454);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAddItemAlias);
            this.Controls.Add(this.btnDeleteItemAlias);
            this.Controls.Add(this.dgvSetShiftItemAlias);
            this.Name = "ShiftItemAliasForm";
            this.Text = "交接班记录项目配置";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetShiftItemAlias)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.TableView.DataTableView dgvSetShiftItemAlias;
        private Heren.Common.Controls.FlatButton btnAddItemAlias;
        private Heren.Common.Controls.FlatButton btnDeleteItemAlias;
        private Heren.Common.Controls.FlatButton btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShiftItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShiftItemAliasCode;
        private Heren.Common.Controls.TableView.ComboBoxColumn colShiftItemName;
        private Heren.Common.Forms.XTextBoxColumn colShiftItemAlias;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWardName;

    }
}