namespace Heren.NurDoc.PatPage
{
    partial class NursingAssessMentForm
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
            this.virtualTree1 = new Heren.Common.Controls.VirtualTreeView.VirtualTree();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
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
            this.virtualTree1.Size = new System.Drawing.Size(138, 398);
            this.virtualTree1.TabIndex = 5;
            this.virtualTree1.NodeMouseClick += new Heren.Common.Controls.VirtualTreeView.VirtualTreeEventHandler(this.virtualTree1_NodeMouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExpandAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnuExpandAll
            // 
            this.mnuExpandAll.Name = "mnuExpandAll";
            this.mnuExpandAll.Size = new System.Drawing.Size(146, 22);
            this.mnuExpandAll.Text = "默认展开所有";
            this.mnuExpandAll.Click += new System.EventHandler(this.mnuExpandAll_Click);
            // 
            // formControl1
            // 
            this.formControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.formControl1.Location = new System.Drawing.Point(153, 2);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(524, 398);
            this.formControl1.TabIndex = 0;
            this.formControl1.Text = "formControl1";
            this.formControl1.QueryContext += new Heren.Common.Forms.Editor.QueryContextEventHandler(this.formControl1_QueryContext);
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            // 
            // arrowSplitter1
            // 
            this.arrowSplitter1.Location = new System.Drawing.Point(140, 2);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(12, 398);
            this.arrowSplitter1.TabIndex = 6;
            this.arrowSplitter1.TabStop = false;
            // 
            // NursingAssessMentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(682, 402);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.virtualTree1);
            this.Controls.Add(this.formControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NursingAssessMentForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "护理评估";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.Common.Controls.VirtualTreeView.VirtualTree virtualTree1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuExpandAll;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
    }
}