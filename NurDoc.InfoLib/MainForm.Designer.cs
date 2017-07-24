namespace Heren.NurDoc.InfoLib
{
    partial class MainForm
    {
        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslblSystemStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblCopyright = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel1 = new Heren.Common.DockSuite.DockPanel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslblSystemStatus,
            this.tsslblCopyright});
            this.statusStrip1.Location = new System.Drawing.Point(0, 416);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(915, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslblSystemStatus
            // 
            this.tsslblSystemStatus.Name = "tsslblSystemStatus";
            this.tsslblSystemStatus.Size = new System.Drawing.Size(705, 17);
            this.tsslblSystemStatus.Spring = true;
            this.tsslblSystemStatus.Text = "就绪";
            this.tsslblSystemStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslblCopyright
            // 
            this.tsslblCopyright.Name = "tsslblCopyright";
            this.tsslblCopyright.Size = new System.Drawing.Size(195, 17);
            this.tsslblCopyright.Text = "版权所有(C) 浙江和仁科技有限公司";
            this.tsslblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel1.DocumentStyle = Heren.Common.DockSuite.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(915, 416);
            this.dockPanel1.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 438);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "护理信息库";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslblSystemStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslblCopyright;
        private Heren.Common.DockSuite.DockPanel dockPanel1;
    }
}

