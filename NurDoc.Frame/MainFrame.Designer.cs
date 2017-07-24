namespace Heren.NurDoc.Frame
{
    partial class MainFrame
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
            this.dockPanel1 = new Heren.Common.DockSuite.DockPanel();
            this.statusBarControl1 = new Heren.NurDoc.Frame.Controls.StatusBarControl();
            this.nursingHomePage1 = new Heren.NurDoc.Frame.Controls.NursingHomePage();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dockPanel1.DocumentStyle = Heren.Common.DockSuite.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 25);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.ShowDocumentBorder = false;
            this.dockPanel1.Size = new System.Drawing.Size(765, 450);
            this.dockPanel1.TabIndex = 1;
            this.dockPanel1.ActiveContentChanged += new System.EventHandler(this.dockPanel1_ActiveContentChanged);
            this.dockPanel1.ActiveDocumentChanged += new System.EventHandler(this.dockPanel1_ActiveDocumentChanged);
            // 
            // statusBarControl1
            // 
            this.statusBarControl1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusBarControl1.Location = new System.Drawing.Point(0, 475);
            this.statusBarControl1.Name = "statusBarControl1";
            this.statusBarControl1.Size = new System.Drawing.Size(765, 22);
            this.statusBarControl1.TabIndex = 2;
            this.statusBarControl1.Text = "statusBarControl1";
            // 
            // nursingHomePage1
            // 
            this.nursingHomePage1.Dock = System.Windows.Forms.DockStyle.Top;
            this.nursingHomePage1.Location = new System.Drawing.Point(0, 0);
            this.nursingHomePage1.Name = "nursingHomePage1";
            this.nursingHomePage1.SinglePatMod = false;
            this.nursingHomePage1.Size = new System.Drawing.Size(765, 25);
            this.nursingHomePage1.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(765, 497);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.statusBarControl1);
            this.Controls.Add(this.nursingHomePage1);
            this.Name = "MainFrame";
            this.Text = "护理电子病历系统";
            this.statusBarControl1.ResumeLayout(false);
            this.statusBarControl1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Heren.NurDoc.Frame.Controls.NursingHomePage nursingHomePage1;
        private Heren.Common.DockSuite.DockPanel dockPanel1;
        private Heren.NurDoc.Frame.Controls.StatusBarControl statusBarControl1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}