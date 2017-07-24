namespace Heren.NurDoc.PatPage.NCP
{
    partial class NursingCarePlanForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlDocumentList = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblSpace = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnNew = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuPrintAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuPrintDataTable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnOption = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuSaveReturn = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.cmenuCarePlanList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuComplete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRollback = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCompleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlDocumentList.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.cmenuCarePlanList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDocumentList
            // 
            this.pnlDocumentList.Controls.Add(this.toolStrip1);
            this.pnlDocumentList.Controls.Add(this.dataTableView1);
            this.pnlDocumentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDocumentList.Location = new System.Drawing.Point(2, 2);
            this.pnlDocumentList.Name = "pnlDocumentList";
            this.pnlDocumentList.Size = new System.Drawing.Size(865, 462);
            this.pnlDocumentList.TabIndex = 0;
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
            this.toollblSpace,
            this.toolbtnNew,
            this.toolbtnPrint,
            this.toolbtnOption});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(865, 32);
            this.toolStrip1.TabIndex = 3;
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
            this.tooldtpDateFrom.Text = "2012/02/20 14:54:16";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 14, 54, 16, 0);
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
            this.tooldtpDateTo.Text = "2012/02/20 14:54:16";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 14, 54, 16, 0);
            this.tooldtpDateTo.ValueChanged += new System.EventHandler(this.tooldtpQueryDate_ValueChanged);
            // 
            // toollblSpace
            // 
            this.toollblSpace.AutoSize = false;
            this.toollblSpace.Name = "toollblSpace";
            this.toollblSpace.Size = new System.Drawing.Size(10, 22);
            this.toollblSpace.Text = " ";
            // 
            // toolbtnNew
            // 
            this.toolbtnNew.AutoSize = false;
            this.toolbtnNew.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Add;
            this.toolbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNew.Name = "toolbtnNew";
            this.toolbtnNew.Size = new System.Drawing.Size(64, 22);
            this.toolbtnNew.Text = "新增";
            this.toolbtnNew.Click += new System.EventHandler(this.toolbtnNew_Click);
            // 
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuPrintAll,
            this.toolmnuPrintDataTable});
            this.toolbtnPrint.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Print;
            this.toolbtnPrint.Name = "toolbtnPrint";
            this.toolbtnPrint.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrint.Text = "打印";
            // 
            // toolmnuPrintAll
            // 
            this.toolmnuPrintAll.Name = "toolmnuPrintAll";
            this.toolmnuPrintAll.Size = new System.Drawing.Size(148, 22);
            this.toolmnuPrintAll.Text = "打印所有行";
            this.toolmnuPrintAll.Click += new System.EventHandler(this.toolmnuPrintAll_Click);
            // 
            // toolmnuPrintDataTable
            // 
            this.toolmnuPrintDataTable.Name = "toolmnuPrintDataTable";
            this.toolmnuPrintDataTable.Size = new System.Drawing.Size(148, 22);
            this.toolmnuPrintDataTable.Text = "打印所见数据";
            this.toolmnuPrintDataTable.Click += new System.EventHandler(this.toolmnuPrintDataTable_Click);
            // 
            // toolbtnOption
            // 
            this.toolbtnOption.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolbtnOption.AutoSize = false;
            this.toolbtnOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuSaveReturn});
            this.toolbtnOption.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Option;
            this.toolbtnOption.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnOption.Name = "toolbtnOption";
            this.toolbtnOption.Size = new System.Drawing.Size(64, 22);
            this.toolbtnOption.Text = "选项";
            this.toolbtnOption.DropDownOpening += new System.EventHandler(this.toolbtnOption_DropDownOpening);
            // 
            // toolmnuSaveReturn
            // 
            this.toolmnuSaveReturn.Name = "toolmnuSaveReturn";
            this.toolmnuSaveReturn.Size = new System.Drawing.Size(160, 22);
            this.toolmnuSaveReturn.Text = "保存后直接返回";
            this.toolmnuSaveReturn.Click += new System.EventHandler(this.toolmnuSaveReturn_Click);
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataTableView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataTableView1.ColumnHeadersHeight = 32;
            this.dataTableView1.ContextMenuStrip = this.cmenuCarePlanList;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTableView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataTableView1.Location = new System.Drawing.Point(3, 35);
            this.dataTableView1.MultiSelect = false;
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersWidth = 36;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTableView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataTableView1.Size = new System.Drawing.Size(862, 427);
            this.dataTableView1.TabIndex = 2;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            // 
            // cmenuCarePlanList
            // 
            this.cmenuCarePlanList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuComplete,
            this.mnuStop,
            this.mnuRollback,
            this.mnuDelete,
            this.mnuCompleteAll});
            this.cmenuCarePlanList.Name = "contextMenuStrip1";
            this.cmenuCarePlanList.Size = new System.Drawing.Size(153, 136);
            this.cmenuCarePlanList.Opening += new System.ComponentModel.CancelEventHandler(this.cmenuCarePlanList_Opening);
            // 
            // mnuComplete
            // 
            this.mnuComplete.Name = "mnuComplete";
            this.mnuComplete.Size = new System.Drawing.Size(152, 22);
            this.mnuComplete.Text = "完成";
            this.mnuComplete.Click += new System.EventHandler(this.mnuComplete_Click);
            // 
            // mnuStop
            // 
            this.mnuStop.Name = "mnuStop";
            this.mnuStop.Size = new System.Drawing.Size(152, 22);
            this.mnuStop.Text = "停止";
            this.mnuStop.Click += new System.EventHandler(this.mnuStop_Click);
            // 
            // mnuRollback
            // 
            this.mnuRollback.Name = "mnuRollback";
            this.mnuRollback.Size = new System.Drawing.Size(152, 22);
            this.mnuRollback.Text = "撤销";
            this.mnuRollback.Click += new System.EventHandler(this.mnuRollback_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(152, 22);
            this.mnuDelete.Text = "删除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // mnuCompleteAll
            // 
            this.mnuCompleteAll.Name = "mnuCompleteAll";
            this.mnuCompleteAll.Size = new System.Drawing.Size(152, 22);
            this.mnuCompleteAll.Text = "全部完成";
            this.mnuCompleteAll.Click += new System.EventHandler(this.mnuCompleteAll_Click_1);
            // 
            // NursingCarePlanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(869, 466);
            this.Controls.Add(this.pnlDocumentList);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NursingCarePlanForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "护理计划";
            this.pnlDocumentList.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.cmenuCarePlanList.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel pnlDocumentList;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblDateTo;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateTo;
        private System.Windows.Forms.ToolStripLabel toollblSpace;
        private System.Windows.Forms.ToolStripButton toolbtnNew;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnPrint;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintAll;
        private System.Windows.Forms.ContextMenuStrip cmenuCarePlanList;
        private System.Windows.Forms.ToolStripMenuItem mnuComplete;
        private System.Windows.Forms.ToolStripMenuItem mnuStop;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuRollback;
        private System.Windows.Forms.ToolStripMenuItem toolmnuPrintDataTable;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnOption;
        private System.Windows.Forms.ToolStripMenuItem toolmnuSaveReturn;
        private System.Windows.Forms.ToolStripMenuItem mnuCompleteAll;
    }
}