namespace Heren.NurDoc.Frame.DockForms
{
    partial class EvaluationListForm
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
            this.pnlDocumentList = new System.Windows.Forms.Panel();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colEvaId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvaTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModifierTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreaterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModifierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnenuDocumentList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnNew = new System.Windows.Forms.ToolStripButton();
            this.pnlDocumentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.cmnenuDocumentList.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDocumentList
            // 
            this.pnlDocumentList.Controls.Add(this.dataTableView1);
            this.pnlDocumentList.Controls.Add(this.toolStrip1);
            this.pnlDocumentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDocumentList.Location = new System.Drawing.Point(2, 2);
            this.pnlDocumentList.Name = "pnlDocumentList";
            this.pnlDocumentList.Size = new System.Drawing.Size(795, 462);
            this.pnlDocumentList.TabIndex = 0;
            // 
            // dataTableView1
            // 
            this.dataTableView1.ColumnHeadersHeight = 32;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEvaId,
            this.colEvaTypeName,
            this.colModifierTime,
            this.colCreaterName,
            this.colModifierName,
            this.colScore,
            this.colRemark});
            this.dataTableView1.ContextMenuStrip = this.cmnenuDocumentList;
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(0, 32);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(795, 430);
            this.dataTableView1.TabIndex = 2;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            // 
            // colEvaId
            // 
            this.colEvaId.HeaderText = "主键";
            this.colEvaId.Name = "colEvaId";
            this.colEvaId.ReadOnly = true;
            this.colEvaId.Visible = false;
            // 
            // colEvaTypeName
            // 
            this.colEvaTypeName.HeaderText = "评价单类型";
            this.colEvaTypeName.Name = "colEvaTypeName";
            this.colEvaTypeName.ReadOnly = true;
            this.colEvaTypeName.Width = 200;
            // 
            // colModifierTime
            // 
            this.colModifierTime.HeaderText = "记录时间";
            this.colModifierTime.Name = "colModifierTime";
            this.colModifierTime.ReadOnly = true;
            this.colModifierTime.Width = 140;
            // 
            // colCreaterName
            // 
            this.colCreaterName.HeaderText = "创建人";
            this.colCreaterName.Name = "colCreaterName";
            this.colCreaterName.ReadOnly = true;
            // 
            // colModifierName
            // 
            this.colModifierName.HeaderText = "修改人";
            this.colModifierName.Name = "colModifierName";
            this.colModifierName.ReadOnly = true;
            // 
            // colScore
            // 
            this.colScore.HeaderText = "分值";
            this.colScore.Name = "colScore";
            this.colScore.ReadOnly = true;
            this.colScore.Width = 80;
            // 
            // colRemark
            // 
            this.colRemark.HeaderText = "备注";
            this.colRemark.Name = "colRemark";
            this.colRemark.ReadOnly = true;
            // 
            // cmnenuDocumentList
            // 
            this.cmnenuDocumentList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuDelete});
            this.cmnenuDocumentList.Name = "contextMenuStrip1";
            this.cmnenuDocumentList.Size = new System.Drawing.Size(153, 70);
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(152, 22);
            this.mnuNew.Text = "新增";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(152, 22);
            this.mnuDelete.Text = "删除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblDateFrom,
            this.tooldtpDateFrom,
            this.toollblDateTo,
            this.tooldtpDateTo,
            this.toolStripLabel2,
            this.toolbtnNew});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(795, 32);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toollblDateFrom
            // 
            this.toollblDateFrom.AutoSize = false;
            this.toollblDateFrom.Name = "toollblDateFrom";
            this.toollblDateFrom.Size = new System.Drawing.Size(47, 22);
            this.toollblDateFrom.Text = " 时段：";
            // 
            // tooldtpDateFrom
            // 
            this.tooldtpDateFrom.AutoSize = false;
            this.tooldtpDateFrom.BackColor = System.Drawing.Color.White;
            this.tooldtpDateFrom.Name = "tooldtpDateFrom";
            this.tooldtpDateFrom.ShowHour = false;
            this.tooldtpDateFrom.ShowMinute = false;
            this.tooldtpDateFrom.ShowSecond = false;
            this.tooldtpDateFrom.Size = new System.Drawing.Size(120, 22);
            this.tooldtpDateFrom.Text = "2012/2/20 13:38:21";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 13, 38, 21, 0);
            this.tooldtpDateFrom.ValueChanged += new System.EventHandler(this.tooldtpQueryDate_ValueChanged);
            // 
            // toollblDateTo
            // 
            this.toollblDateTo.AutoSize = false;
            this.toollblDateTo.Name = "toollblDateTo";
            this.toollblDateTo.Size = new System.Drawing.Size(11, 22);
            this.toollblDateTo.Text = "-";
            // 
            // tooldtpDateTo
            // 
            this.tooldtpDateTo.AutoSize = false;
            this.tooldtpDateTo.BackColor = System.Drawing.Color.White;
            this.tooldtpDateTo.Name = "tooldtpDateTo";
            this.tooldtpDateTo.ShowHour = false;
            this.tooldtpDateTo.ShowMinute = false;
            this.tooldtpDateTo.ShowSecond = false;
            this.tooldtpDateTo.Size = new System.Drawing.Size(120, 22);
            this.tooldtpDateTo.Text = "2012/2/20 13:38:21";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 13, 38, 21, 0);
            this.tooldtpDateTo.ValueChanged += new System.EventHandler(this.tooldtpQueryDate_ValueChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(10, 22);
            this.toolStripLabel2.Text = " ";
            // 
            // toolbtnNew
            // 
            this.toolbtnNew.AutoSize = false;
            this.toolbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNew.Name = "toolbtnNew";
            this.toolbtnNew.Size = new System.Drawing.Size(64, 22);
            this.toolbtnNew.Text = "新增";
            this.toolbtnNew.Click += new System.EventHandler(this.toolbtnNew_Click);
            // 
            // EvaluationListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(799, 466);
            this.Controls.Add(this.pnlDocumentList);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "EvaluationListForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "护理评价列表";
            this.pnlDocumentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.cmnenuDocumentList.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel pnlDocumentList;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ContextMenuStrip cmnenuDocumentList;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripButton toolbtnNew;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvaId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvaTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifierTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreaterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifierName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
    }
}