namespace Heren.NurDoc.Frame.DockForms
{
    partial class QCForm
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.virtualTree1 = new Heren.Common.Controls.VirtualTreeView.VirtualTree();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.arrowSplitter1 = new Heren.Common.Controls.ArrowSplitter();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.virtualTree1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 511);
            this.panel1.TabIndex = 0;
            // 
            // virtualTree1
            // 
            this.virtualTree1.AutoScrollMinSize = new System.Drawing.Size(200, 0);
            this.virtualTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualTree1.Location = new System.Drawing.Point(0, 0);
            this.virtualTree1.Name = "virtualTree1";
            this.virtualTree1.ShowColumnHeader = false;
            this.virtualTree1.ShowToolTip = false;
            this.virtualTree1.Size = new System.Drawing.Size(200, 511);
            this.virtualTree1.TabIndex = 7;
            this.virtualTree1.NodeMouseClick += new Heren.Common.Controls.VirtualTreeView.VirtualTreeEventHandler(this.virtualTree1_NodeMouseClick);
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
            this.arrowSplitter1.Location = new System.Drawing.Point(200, 0);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(10, 511);
            this.arrowSplitter1.TabIndex = 12;
            this.arrowSplitter1.TabStop = false;
            // 
            // formControl1
            // 
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(210, 0);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(474, 511);
            this.formControl1.TabIndex = 13;
            this.formControl1.Text = "formControl1";
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            // 
            // QSMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 511);
            this.Controls.Add(this.formControl1);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "QSMForm";
            this.Text = "质量与安全管理记录";
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Heren.Common.Controls.VirtualTreeView.VirtualTree virtualTree1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuExpandAll;
        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;


    }
}