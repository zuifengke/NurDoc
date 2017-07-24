namespace Heren.NurDoc.Utilities.Templet
{
    partial class WordTempEditForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.cmenuTempletEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.wordTempletContent = new System.Windows.Forms.TextBox();
            this.winWordCtrl1 = new Heren.NurDoc.Utilities.WinWordCtrl();
            this.toolStrip1.SuspendLayout();
            this.cmenuTempletEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtnSave});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(653, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolbtnSave
            // 
            this.toolbtnSave.AutoSize = false;
            this.toolbtnSave.Font = new System.Drawing.Font("宋体", 9F);
            this.toolbtnSave.Image = global::Heren.NurDoc.Utilities.Properties.Resources.Save;
            this.toolbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSave.Name = "toolbtnSave";
            this.toolbtnSave.Size = new System.Drawing.Size(64, 24);
            this.toolbtnSave.Text = "保存";
            this.toolbtnSave.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // cmenuTempletEdit
            // 
            this.cmenuTempletEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.toolStripSeparator1,
            this.mnuImport});
            this.cmenuTempletEdit.Name = "cmenuTempletEdit";
            this.cmenuTempletEdit.Size = new System.Drawing.Size(149, 98);
            // 
            // mnuCut
            // 
            this.mnuCut.Name = "mnuCut";
            this.mnuCut.Size = new System.Drawing.Size(148, 22);
            this.mnuCut.Text = "剪切";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(148, 22);
            this.mnuCopy.Text = "复制";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.Size = new System.Drawing.Size(148, 22);
            this.mnuPaste.Text = "粘贴";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // mnuImport
            // 
            this.mnuImport.Name = "mnuImport";
            this.mnuImport.Size = new System.Drawing.Size(148, 22);
            this.mnuImport.Text = "导入选中内容";
            this.mnuImport.Click += new System.EventHandler(this.mnuImport_Click);
            // 
            // wordTempletContent
            // 
            this.wordTempletContent.ContextMenuStrip = this.cmenuTempletEdit;
            this.wordTempletContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wordTempletContent.Font = new System.Drawing.Font("宋体", 10.5F);
            this.wordTempletContent.HideSelection = false;
            this.wordTempletContent.Location = new System.Drawing.Point(0, 27);
            this.wordTempletContent.Multiline = true;
            this.wordTempletContent.Name = "wordTempletContent";
            this.wordTempletContent.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.wordTempletContent.Size = new System.Drawing.Size(653, 386);
            this.wordTempletContent.TabIndex = 3;
            // 
            // winWordCtrl1
            // 
            this.winWordCtrl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.winWordCtrl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.winWordCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winWordCtrl1.Location = new System.Drawing.Point(0, 27);
            this.winWordCtrl1.Name = "winWordCtrl1";
            this.winWordCtrl1.Size = new System.Drawing.Size(653, 386);
            this.winWordCtrl1.TabIndex = 4;
             // 
            // WordTempEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(653, 413);
            this.Controls.Add(this.winWordCtrl1);
            this.Controls.Add(this.wordTempletContent);
            this.Controls.Add(this.toolStrip1);
            this.Name = "WordTempEditForm";
            this.Text = "模板编辑";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmenuTempletEdit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.ContextMenuStrip cmenuTempletEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuCut;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuImport;
        private System.Windows.Forms.TextBox wordTempletContent;
        private WinWordCtrl winWordCtrl1;
    }
}