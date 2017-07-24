namespace Heren.NurDoc.Frame.DockForms
{
    partial class EvaluationEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EvaluationEditForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblRecordTime = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpRecordTime = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblCreatorName = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtCreatorName = new System.Windows.Forms.ToolStripTextBox();
            this.toollblRemark = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtRemark = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtScore = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnCalculate = new System.Windows.Forms.ToolStripButton();
            this.toolDropDownbtnSave = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolbtnSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnReturn = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblRecordTime,
            this.tooldtpRecordTime,
            this.toollblCreatorName,
            this.tooltxtCreatorName,
            this.toollblRemark,
            this.tooltxtRemark,
            this.toolStripLabel3,
            this.tooltxtScore,
            this.toolStripLabel1,
            this.toolbtnCalculate,
            this.toolDropDownbtnSave,
            this.toolbtnReturn,
            this.toolbtnPrint});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(891, 32);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "baseToolStrip1";
            // 
            // toollblRecordTime
            // 
            this.toollblRecordTime.AutoSize = false;
            this.toollblRecordTime.Name = "toollblRecordTime";
            this.toollblRecordTime.Size = new System.Drawing.Size(65, 22);
            this.toollblRecordTime.Text = "记录时间：";
            // 
            // tooldtpRecordTime
            // 
            this.tooldtpRecordTime.AutoSize = false;
            this.tooldtpRecordTime.BackColor = System.Drawing.Color.White;
            this.tooldtpRecordTime.Name = "tooldtpRecordTime";
            this.tooldtpRecordTime.ShowSecond = false;
            this.tooldtpRecordTime.Size = new System.Drawing.Size(150, 22);
            this.tooldtpRecordTime.Text = "2012/2/20 19:21:59";
            this.tooldtpRecordTime.Value = new System.DateTime(2012, 2, 20, 19, 21, 59, 0);
            this.tooldtpRecordTime.ValueChanged += new System.EventHandler(this.tooldtpRecordTime_ValueChanged);
            // 
            // toollblCreatorName
            // 
            this.toollblCreatorName.AutoSize = false;
            this.toollblCreatorName.Name = "toollblCreatorName";
            this.toollblCreatorName.Size = new System.Drawing.Size(59, 22);
            this.toollblCreatorName.Text = " 创建人：";
            // 
            // tooltxtCreatorName
            // 
            this.tooltxtCreatorName.AutoSize = false;
            this.tooltxtCreatorName.Name = "tooltxtCreatorName";
            this.tooltxtCreatorName.ReadOnly = true;
            this.tooltxtCreatorName.Size = new System.Drawing.Size(64, 28);
            // 
            // toollblRemark
            // 
            this.toollblRemark.AutoSize = false;
            this.toollblRemark.Name = "toollblRemark";
            this.toollblRemark.Size = new System.Drawing.Size(59, 22);
            this.toollblRemark.Text = "备注：";
            // 
            // tooltxtRemark
            // 
            this.tooltxtRemark.AutoSize = false;
            this.tooltxtRemark.Name = "tooltxtRemark";
            this.tooltxtRemark.Size = new System.Drawing.Size(104, 28);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.AutoSize = false;
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabel3.Text = "分值：";
            // 
            // tooltxtScore
            // 
            this.tooltxtScore.AutoSize = false;
            this.tooltxtScore.Name = "tooltxtScore";
            this.tooltxtScore.ReadOnly = true;
            this.tooltxtScore.Size = new System.Drawing.Size(64, 28);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(11, 22);
            this.toolStripLabel1.Text = " ";
            // 
            // toolbtnCalculate
            // 
            this.toolbtnCalculate.AutoSize = false;
            this.toolbtnCalculate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnCalculate.Name = "toolbtnCalculate";
            this.toolbtnCalculate.Size = new System.Drawing.Size(64, 22);
            this.toolbtnCalculate.Text = "计算分值";
            this.toolbtnCalculate.Click += new System.EventHandler(this.toolbtnCalculate_Click);
            // 
            // toolDropDownbtnSave
            // 
            this.toolDropDownbtnSave.AutoSize = false;
            this.toolDropDownbtnSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtnSave,
            this.toolbtnSaveAs});
            this.toolDropDownbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDropDownbtnSave.Name = "toolDropDownbtnSave";
            this.toolDropDownbtnSave.Size = new System.Drawing.Size(64, 22);
            this.toolDropDownbtnSave.Text = "保存";
            // 
            // toolbtnSave
            // 
            this.toolbtnSave.Name = "toolbtnSave";
            this.toolbtnSave.Size = new System.Drawing.Size(152, 22);
            this.toolbtnSave.Text = "保存";
            this.toolbtnSave.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // toolbtnSaveAs
            // 
            this.toolbtnSaveAs.Name = "toolbtnSaveAs";
            this.toolbtnSaveAs.Size = new System.Drawing.Size(152, 22);
            this.toolbtnSaveAs.Text = "另存为模板";
            this.toolbtnSaveAs.Click += new System.EventHandler(this.toolbtnSaveAs_Click);
            // 
            // toolbtnReturn
            // 
            this.toolbtnReturn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnReturn.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnReturn.Image")));
            this.toolbtnReturn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnReturn.Name = "toolbtnReturn";
            this.toolbtnReturn.Size = new System.Drawing.Size(36, 29);
            this.toolbtnReturn.Text = "返回";
            this.toolbtnReturn.Click += new System.EventHandler(this.toolbtnReturn_Click);
            // 
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnPrint.Name = "toolbtnPrint";
            this.toolbtnPrint.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrint.Text = "打印";
            this.toolbtnPrint.Click += new System.EventHandler(this.toolbtnPrint_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 23);
            // 
            // dataTableView1
            // 
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colContent,
            this.colRemark,
            this.colValue});
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Location = new System.Drawing.Point(2, 34);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(891, 430);
            this.dataTableView1.TabIndex = 5;
            // 
            // colContent
            // 
            this.colContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colContent.FillWeight = 300F;
            this.colContent.HeaderText = "内容";
            this.colContent.Name = "colContent";
            this.colContent.ReadOnly = true;
            this.colContent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colRemark
            // 
            this.colRemark.HeaderText = "备注";
            this.colRemark.Name = "colRemark";
            this.colRemark.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRemark.Width = 150;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "值";
            this.colValue.Name = "colValue";
            // 
            // EvaluationEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(895, 466);
            this.Controls.Add(this.dataTableView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "EvaluationEditForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "病历编辑窗口";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblRecordTime;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpRecordTime;
        private System.Windows.Forms.ToolStripLabel toollblCreatorName;
        private System.Windows.Forms.ToolStripTextBox tooltxtCreatorName;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolbtnPrint;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolbtnReturn;
        private System.Windows.Forms.ToolStripLabel toollblRemark;
        private System.Windows.Forms.ToolStripTextBox tooltxtScore;
        private System.Windows.Forms.ToolStripButton toolbtnCalculate;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.ToolStripTextBox tooltxtRemark;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripDropDownButton toolDropDownbtnSave;
        private System.Windows.Forms.ToolStripMenuItem toolbtnSave;
        private System.Windows.Forms.ToolStripMenuItem toolbtnSaveAs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colValue;
    }
}