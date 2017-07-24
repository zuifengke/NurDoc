namespace Heren.NurDoc.Frame.DockForms
{
    partial class NursingTaskForm
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
            this.DisposeAllForms();
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
            this.virtualTree1 = new Heren.Common.Controls.VirtualTreeView.VirtualTree();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.arrowSplitter1 = new Heren.Common.Controls.ArrowSplitter();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // virtualTree1
            // 
            this.virtualTree1.ContextMenuStrip = this.contextMenuStrip1;
            this.virtualTree1.Dock = System.Windows.Forms.DockStyle.Left;
            this.virtualTree1.Location = new System.Drawing.Point(2, 2);
            this.virtualTree1.Name = "virtualTree1";
            this.virtualTree1.ShowColumnHeader = false;
            this.virtualTree1.ShowToolTip = false;
            this.virtualTree1.Size = new System.Drawing.Size(172, 444);
            this.virtualTree1.TabIndex = 1;
            this.virtualTree1.AfterSelect += new Heren.Common.Controls.VirtualTreeView.VirtualTreeEventHandler(this.virtualTree1_AfterSelect);
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
            this.arrowSplitter1.Location = new System.Drawing.Point(174, 2);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(10, 444);
            this.arrowSplitter1.TabIndex = 25;
            this.arrowSplitter1.TabStop = false;
            // 
            // NursingTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(687, 448);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.virtualTree1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NursingTaskForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "待办任务";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.Common.Controls.VirtualTreeView.VirtualTree virtualTree1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuExpandAll;
        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
    }
}