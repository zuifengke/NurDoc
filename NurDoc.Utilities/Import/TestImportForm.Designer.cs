namespace Heren.NurDoc.Utilities.Import
{
    partial class TestImportForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new Heren.Common.Controls.TreeViewControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRefreshList = new System.Windows.Forms.ToolStripMenuItem();
            this.txtTestDetails = new System.Windows.Forms.TextBox();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colCheckTest = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAbnormal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNormalValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnImport = new Heren.Common.Controls.HerenButton();
            this.btnClose = new Heren.Common.Controls.HerenButton();
            this.chkCheckAllAbnormal = new System.Windows.Forms.CheckBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtTestDetails);
            this.splitContainer1.Panel2.Controls.Add(this.dataTableView1);
            this.splitContainer1.Size = new System.Drawing.Size(832, 407);
            this.splitContainer1.SplitterDistance = 214;
            this.splitContainer1.TabIndex = 12;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(1, 1);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(212, 405);
            this.treeView1.TabIndex = 5;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRefreshList});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 26);
            // 
            // mnuRefreshList
            // 
            this.mnuRefreshList.Name = "mnuRefreshList";
            this.mnuRefreshList.Size = new System.Drawing.Size(122, 22);
            this.mnuRefreshList.Text = "刷新列表";
            this.mnuRefreshList.Click += new System.EventHandler(this.mnuRefreshList_Click);
            // 
            // txtTestDetails
            // 
            this.txtTestDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.txtTestDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTestDetails.Location = new System.Drawing.Point(3, 3);
            this.txtTestDetails.Name = "txtTestDetails";
            this.txtTestDetails.ReadOnly = true;
            this.txtTestDetails.Size = new System.Drawing.Size(610, 14);
            this.txtTestDetails.TabIndex = 15;
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckTest,
            this.colItemNo,
            this.colItemName,
            this.colItemResult,
            this.colItemUnits,
            this.colAbnormal,
            this.colNormalValue});
            this.dataTableView1.Location = new System.Drawing.Point(1, 19);
            this.dataTableView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.Size = new System.Drawing.Size(612, 387);
            this.dataTableView1.TabIndex = 14;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            this.dataTableView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellContentClick);
            // 
            // colCheckTest
            // 
            this.colCheckTest.FillWeight = 24F;
            this.colCheckTest.HeaderText = string.Empty;
            this.colCheckTest.Name = "colCheckTest";
            this.colCheckTest.Width = 24;
            // 
            // colItemNo
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colItemNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colItemNo.FillWeight = 36F;
            this.colItemNo.HeaderText = "序号";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.ReadOnly = true;
            this.colItemNo.Width = 36;
            // 
            // colItemName
            // 
            this.colItemName.FillWeight = 200F;
            this.colItemName.HeaderText = "项目名称";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            this.colItemName.Width = 200;
            // 
            // colItemResult
            // 
            this.colItemResult.FillWeight = 72F;
            this.colItemResult.HeaderText = "结果";
            this.colItemResult.Name = "colItemResult";
            this.colItemResult.ReadOnly = true;
            this.colItemResult.Width = 72;
            // 
            // colItemUnits
            // 
            this.colItemUnits.FillWeight = 72F;
            this.colItemUnits.HeaderText = "单位";
            this.colItemUnits.Name = "colItemUnits";
            this.colItemUnits.ReadOnly = true;
            this.colItemUnits.Width = 72;
            // 
            // colAbnormal
            // 
            this.colAbnormal.FillWeight = 48F;
            this.colAbnormal.HeaderText = "异常";
            this.colAbnormal.Name = "colAbnormal";
            this.colAbnormal.ReadOnly = true;
            this.colAbnormal.Width = 48;
            // 
            // colNormalValue
            // 
            this.colNormalValue.FillWeight = 130F;
            this.colNormalValue.HeaderText = "正常参考值";
            this.colNormalValue.Name = "colNormalValue";
            this.colNormalValue.ReadOnly = true;
            this.colNormalValue.Width = 130;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnImport.Font = new System.Drawing.Font("宋体", 9F);
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(622, 419);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(88, 24);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F);
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(730, 419);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // chkCheckAllAbnormal
            // 
            this.chkCheckAllAbnormal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCheckAllAbnormal.AutoSize = true;
            this.chkCheckAllAbnormal.Location = new System.Drawing.Point(12, 424);
            this.chkCheckAllAbnormal.Name = "chkCheckAllAbnormal";
            this.chkCheckAllAbnormal.Size = new System.Drawing.Size(108, 16);
            this.chkCheckAllAbnormal.TabIndex = 13;
            this.chkCheckAllAbnormal.Text = "勾选所有异常值";
            this.chkCheckAllAbnormal.UseVisualStyleBackColor = true;
            this.chkCheckAllAbnormal.CheckedChanged += new System.EventHandler(this.chkCheckAllAbnormal_CheckedChanged);
            // 
            // TestImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(836, 451);
            this.Controls.Add(this.chkCheckAllAbnormal);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnClose);
            this.MinimizeBox = false;
            this.Name = "TestImportForm";
            this.SaveWindowState = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "检验结果";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Heren.Common.Controls.HerenButton btnImport;
        private Heren.Common.Controls.HerenButton btnClose;
        private System.Windows.Forms.TextBox txtTestDetails;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private Heren.Common.Controls.TreeViewControl treeView1;
        private System.Windows.Forms.CheckBox chkCheckAllAbnormal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckTest;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAbnormal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNormalValue;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuRefreshList;
    }
}