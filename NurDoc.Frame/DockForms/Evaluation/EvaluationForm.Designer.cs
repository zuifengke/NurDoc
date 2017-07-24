namespace Heren.NurDoc.Frame.DockForms
{
    partial class EvaluationForm
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.arrowSplitter1 = new Heren.Common.Controls.ArrowSplitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.virtualTree1 = new Heren.Common.Controls.VirtualTreeView.VirtualTree();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolstpDept = new System.Windows.Forms.ToolStripComboBox();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExpandAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnuExpandAll
            // 
            this.mnuExpandAll.Name = "mnuExpandAll";
            this.mnuExpandAll.Size = new System.Drawing.Size(148, 22);
            this.mnuExpandAll.Text = "默认展开所有";
            this.mnuExpandAll.Click += new System.EventHandler(this.mnuExpandAll_Click);
            // 
            // arrowSplitter1
            // 
            this.arrowSplitter1.Location = new System.Drawing.Point(233, 2);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(10, 546);
            this.arrowSplitter1.TabIndex = 10;
            this.arrowSplitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.virtualTree1);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 546);
            this.panel1.TabIndex = 9;
            // 
            // virtualTree1
            // 
            this.virtualTree1.ContextMenuStrip = this.contextMenuStrip1;
            this.virtualTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualTree1.Location = new System.Drawing.Point(0, 25);
            this.virtualTree1.Name = "virtualTree1";
            this.virtualTree1.ShowColumnHeader = false;
            this.virtualTree1.ShowToolTip = false;
            this.virtualTree1.Size = new System.Drawing.Size(231, 521);
            this.virtualTree1.TabIndex = 6;
            this.virtualTree1.NodeMouseClick += new Heren.Common.Controls.VirtualTreeView.VirtualTreeEventHandler(this.virtualTree1_NodeMouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolstpDept});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(231, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1.Text = "科室：";
            // 
            // toolstpDept
            // 
            this.toolstpDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolstpDept.Name = "toolstpDept";
            this.toolstpDept.Size = new System.Drawing.Size(150, 25);
            this.toolstpDept.SelectedIndexChanged += new System.EventHandler(this.toolstpDept_SelectedIndexChanged);
            // 
            // formControl1
            // 
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(233, 2);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(481, 546);
            this.formControl1.TabIndex = 0;
            this.formControl1.Text = "formControl1";
            // 
            // EvaluationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(716, 550);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.formControl1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "EvaluationForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "护理评价";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuExpandAll;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolstpDept;
        private Heren.Common.Controls.VirtualTreeView.VirtualTree virtualTree1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}