namespace Heren.NurDoc.PatPage.Consult
{
    partial class NursingConsultForm
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblDateTo = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateTo = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblSpace = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnNew = new System.Windows.Forms.ToolStripDropDownButton();
            this.pnlDocumentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.pnlDocumentList.Size = new System.Drawing.Size(833, 462);
            this.pnlDocumentList.TabIndex = 0;
            // 
            // dataTableView1
            // 
            this.dataTableView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataTableView1.ColumnHeadersHeight = 32;
            this.dataTableView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(0, 32);
            this.dataTableView1.MultiSelect = false;
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(833, 430);
            this.dataTableView1.TabIndex = 2;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 26);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(146, 22);
            this.mnuDelete.Text = "删除选中记录";
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
            this.toollblSpace,
            this.toolbtnNew});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(833, 32);
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
            this.tooldtpDateFrom.Text = "2013/11/6 14:28:56";
            this.tooldtpDateFrom.Value = new System.DateTime(2013, 11, 6, 14, 28, 56, 0);
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
            this.tooldtpDateTo.Text = "2012/2/20 14:28:56";
            this.tooldtpDateTo.Value = new System.DateTime(2012, 2, 20, 14, 28, 56, 0);
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
            // 
            // NursingConsultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(837, 466);
            this.Controls.Add(this.pnlDocumentList);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NursingConsultForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "护理会诊";
            this.pnlDocumentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripDropDownButton toolbtnNew;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
    }
}