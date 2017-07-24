using Heren.NurDoc.Utilities;

namespace Heren.NurDoc.PatPage.WordDocument
{
    partial class WordDocEditForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtnImport = new System.Windows.Forms.ToolStripButton();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.wordTempletContent = new System.Windows.Forms.TextBox();
            this.winWordCtrl1 = new Heren.NurDoc.Utilities.WinWordCtrl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtnImport,
            this.toolbtnSave});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(653, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolbtnImport
            // 
            this.toolbtnImport.Font = new System.Drawing.Font("SimSun", 9F);
            this.toolbtnImport.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Record;
            this.toolbtnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnImport.Name = "toolbtnImport";
            this.toolbtnImport.Size = new System.Drawing.Size(49, 24);
            this.toolbtnImport.Text = "导入";
            this.toolbtnImport.Click += new System.EventHandler(this.toolbtnImport_Click);
            // 
            // toolbtnSave
            // 
            this.toolbtnSave.AutoSize = false;
            this.toolbtnSave.Font = new System.Drawing.Font("SimSun", 9F);
            this.toolbtnSave.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Save;
            this.toolbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSave.Name = "toolbtnSave";
            this.toolbtnSave.Size = new System.Drawing.Size(64, 24);
            this.toolbtnSave.Text = "同步";
            this.toolbtnSave.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // wordTempletContent
            // 
            this.wordTempletContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wordTempletContent.Font = new System.Drawing.Font("SimSun", 10.5F);
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
            // WordDocEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(653, 413);
            this.Controls.Add(this.winWordCtrl1);
            this.Controls.Add(this.wordTempletContent);
            this.Controls.Add(this.toolStrip1);
            this.Name = "WordDocEditForm";
            this.Text = "模板编辑";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.TextBox wordTempletContent;
        private WinWordCtrl winWordCtrl1;
        private System.Windows.Forms.ToolStripButton toolbtnImport;
    }
}