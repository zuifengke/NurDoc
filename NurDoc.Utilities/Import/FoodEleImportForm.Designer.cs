namespace Heren.NurDoc.Utilities.Import
{
    partial class FoodEleImportForm
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
            this.toollblfNameFrom = new System.Windows.Forms.Label();
            this.toollblfWeightFrom = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.toollblfWaterFrom = new System.Windows.Forms.Label();
            this.txtWater = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.toollblfunitFrom = new System.Windows.Forms.Label();
            this.toollblwunitFrom = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboFNameFind = new Heren.Common.Controls.DictInput.FindComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toollblfNameFrom
            // 
            this.toollblfNameFrom.AutoSize = true;
            this.toollblfNameFrom.Location = new System.Drawing.Point(3, 10);
            this.toollblfNameFrom.Name = "toollblfNameFrom";
            this.toollblfNameFrom.Size = new System.Drawing.Size(65, 12);
            this.toollblfNameFrom.TabIndex = 4;
            this.toollblfNameFrom.Text = "食物名称：";
            // 
            // toollblfWeightFrom
            // 
            this.toollblfWeightFrom.AutoSize = true;
            this.toollblfWeightFrom.Location = new System.Drawing.Point(287, 10);
            this.toollblfWeightFrom.Name = "toollblfWeightFrom";
            this.toollblfWeightFrom.Size = new System.Drawing.Size(65, 12);
            this.toollblfWeightFrom.TabIndex = 5;
            this.toollblfWeightFrom.Text = "食物质量：";
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(356, 6);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(80, 21);
            this.txtWeight.TabIndex = 2;
            this.txtWeight.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // toollblfWaterFrom
            // 
            this.toollblfWaterFrom.AutoSize = true;
            this.toollblfWaterFrom.Location = new System.Drawing.Point(467, 10);
            this.toollblfWaterFrom.Name = "toollblfWaterFrom";
            this.toollblfWaterFrom.Size = new System.Drawing.Size(77, 12);
            this.toollblfWaterFrom.TabIndex = 7;
            this.toollblfWaterFrom.Text = "食物含水量：";
            // 
            // txtWater
            // 
            this.txtWater.Location = new System.Drawing.Point(550, 7);
            this.txtWater.Name = "txtWater";
            this.txtWater.ReadOnly = true;
            this.txtWater.Size = new System.Drawing.Size(80, 21);
            this.txtWater.TabIndex = 3;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(3, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(98, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toollblfunitFrom
            // 
            this.toollblfunitFrom.AutoSize = true;
            this.toollblfunitFrom.Location = new System.Drawing.Point(444, 10);
            this.toollblfunitFrom.Name = "toollblfunitFrom";
            this.toollblfunitFrom.Size = new System.Drawing.Size(17, 12);
            this.toollblfunitFrom.TabIndex = 6;
            this.toollblfunitFrom.Text = "克";
            // 
            // toollblwunitFrom
            // 
            this.toollblwunitFrom.AutoSize = true;
            this.toollblwunitFrom.Location = new System.Drawing.Point(640, 10);
            this.toollblwunitFrom.Name = "toollblwunitFrom";
            this.toollblwunitFrom.Size = new System.Drawing.Size(17, 12);
            this.toollblwunitFrom.TabIndex = 8;
            this.toollblwunitFrom.Text = "ml";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(193, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboFNameFind);
            this.panel1.Controls.Add(this.toollblfNameFrom);
            this.panel1.Controls.Add(this.toollblwunitFrom);
            this.panel1.Controls.Add(this.txtWeight);
            this.panel1.Controls.Add(this.toollblfunitFrom);
            this.panel1.Controls.Add(this.toollblfWeightFrom);
            this.panel1.Controls.Add(this.txtWater);
            this.panel1.Controls.Add(this.toollblfWaterFrom);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(665, 37);
            this.panel1.TabIndex = 9;
            // 
            // cboFNameFind
            // 
            this.cboFNameFind.Location = new System.Drawing.Point(74, 7);
            this.cboFNameFind.Name = "cboFNameFind";
            this.cboFNameFind.Size = new System.Drawing.Size(201, 20);
            this.cboFNameFind.TabIndex = 1;
            this.cboFNameFind.SelectedIndexChanged += new System.EventHandler(this.cboFNameFind_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnImport);
            this.panel2.Controls.Add(this.btnUpdate);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Location = new System.Drawing.Point(404, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(273, 29);
            this.panel2.TabIndex = 13;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 93);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(689, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // FoodEleImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 115);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FoodEleImportForm";
            this.Text = "食物含水量";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FoodEleImportForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label toollblfNameFrom;
        private System.Windows.Forms.Label toollblfWeightFrom;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label toollblfWaterFrom;
        private System.Windows.Forms.TextBox txtWater;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label toollblfunitFrom;
        private System.Windows.Forms.Label toollblwunitFrom;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Heren.Common.Controls.DictInput.FindComboBox cboFNameFind;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}