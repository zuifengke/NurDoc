namespace Heren.NurDoc.PatPage.Consult
{
    partial class ConsultEditForm
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
            this.toollblDateFrom = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpDateFrom = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblCreatorName = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtCreatorName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolbtnNew = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolbtnSubmit = new System.Windows.Forms.ToolStripButton();
            this.toolbtnUndo = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPreview = new System.Windows.Forms.ToolStripButton();
            this.toolbtnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolbtnReturn = new System.Windows.Forms.ToolStripButton();
            this.documentControl1 = new Heren.NurDoc.PatPage.Document.DocumentControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollblDateFrom,
            this.tooldtpDateFrom,
            this.toollblCreatorName,
            this.tooltxtCreatorName,
            this.toolStripLabel1,
            this.toolbtnSave,
            this.toolbtnNew,
            this.toolbtnSubmit,
            this.toolbtnUndo,
            this.toolbtnPreview,
            this.toolbtnPrint,
            this.toolbtnReturn});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(738, 32);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "baseToolStrip1";
            // 
            // toollblDateFrom
            // 
            this.toollblDateFrom.AutoSize = false;
            this.toollblDateFrom.Name = "toollblDateFrom";
            this.toollblDateFrom.Size = new System.Drawing.Size(65, 22);
            this.toollblDateFrom.Text = "记录时间：";
            // 
            // tooldtpDateFrom
            // 
            this.tooldtpDateFrom.AutoSize = false;
            this.tooldtpDateFrom.BackColor = System.Drawing.Color.White;
            this.tooldtpDateFrom.Name = "tooldtpDateFrom";
            this.tooldtpDateFrom.ShowSecond = false;
            this.tooldtpDateFrom.Size = new System.Drawing.Size(150, 22);
            this.tooldtpDateFrom.Text = "2012-2-20 19:21:00";
            this.tooldtpDateFrom.Value = new System.DateTime(2012, 2, 20, 19, 21, 0, 0);
            this.tooldtpDateFrom.ValueChanged += new System.EventHandler(this.tooldtpDateFrom_ValueChanged);
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
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(11, 22);
            this.toolStripLabel1.Text = " ";
            // 
            // toolbtnSave
            // 
            this.toolbtnSave.AutoSize = false;
            this.toolbtnSave.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Save;
            this.toolbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSave.Name = "toolbtnSave";
            this.toolbtnSave.Size = new System.Drawing.Size(64, 22);
            this.toolbtnSave.Text = "保存";
            this.toolbtnSave.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // toolbtnNew
            // 
            this.toolbtnNew.AutoSize = false;
            this.toolbtnNew.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Add;
            this.toolbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNew.Name = "toolbtnNew";
            this.toolbtnNew.Size = new System.Drawing.Size(64, 22);
            this.toolbtnNew.Text = "新增";
            // 
            // toolbtnSubmit
            // 
            this.toolbtnSubmit.AutoSize = false;
            this.toolbtnSubmit.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Commit;
            this.toolbtnSubmit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSubmit.Name = "toolbtnSubmit";
            this.toolbtnSubmit.Size = new System.Drawing.Size(64, 22);
            this.toolbtnSubmit.Text = "提交";
            this.toolbtnSubmit.Click += new System.EventHandler(this.toolbtnSubmit_Click);
            // 
            // toolbtnUndo
            // 
            this.toolbtnUndo.AutoSize = false;
            this.toolbtnUndo.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Rollback;
            this.toolbtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnUndo.Name = "toolbtnUndo";
            this.toolbtnUndo.Size = new System.Drawing.Size(64, 22);
            this.toolbtnUndo.Text = "驳回";
            this.toolbtnUndo.Click += new System.EventHandler(this.toolbtnUndo_Click);
            // 
            // toolbtnPreview
            // 
            this.toolbtnPreview.AutoSize = false;
            this.toolbtnPreview.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Preview;
            this.toolbtnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnPreview.Name = "toolbtnPreview";
            this.toolbtnPreview.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPreview.Text = "预览";
            this.toolbtnPreview.Visible = false;
            this.toolbtnPreview.Click += new System.EventHandler(this.toolbtnPreview_Click);
            // 
            // toolbtnPrint
            // 
            this.toolbtnPrint.AutoSize = false;
            this.toolbtnPrint.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Print;
            this.toolbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnPrint.Name = "toolbtnPrint";
            this.toolbtnPrint.Size = new System.Drawing.Size(64, 22);
            this.toolbtnPrint.Text = "打印";
            this.toolbtnPrint.Visible = false;
            this.toolbtnPrint.Click += new System.EventHandler(this.toolbtnPrint_Click);
            // 
            // toolbtnReturn
            // 
            this.toolbtnReturn.AutoSize = false;
            this.toolbtnReturn.Image = global::Heren.NurDoc.PatPage.Properties.Resources.Return;
            this.toolbtnReturn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnReturn.Name = "toolbtnReturn";
            this.toolbtnReturn.Size = new System.Drawing.Size(64, 22);
            this.toolbtnReturn.Text = "返回";
            this.toolbtnReturn.Click += new System.EventHandler(this.toolbtnReturn_Click);
            // 
            // documentControl1
            // 
            this.documentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentControl1.Location = new System.Drawing.Point(2, 34);
            this.documentControl1.Name = "documentControl1";
            this.documentControl1.Size = new System.Drawing.Size(738, 430);
            this.documentControl1.TabIndex = 3;
            this.documentControl1.Text = "documentControl1";
            // 
            // ConsultEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(742, 466);
            this.Controls.Add(this.documentControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ConsultEditForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "护理会诊编辑窗口";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblDateFrom;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpDateFrom;
        private System.Windows.Forms.ToolStripLabel toollblCreatorName;
        private System.Windows.Forms.ToolStripTextBox tooltxtCreatorName;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private System.Windows.Forms.ToolStripButton toolbtnPrint;
        private System.Windows.Forms.ToolStripButton toolbtnPreview;
        private System.Windows.Forms.ToolStripButton toolbtnReturn;
        private Heren.NurDoc.PatPage.Document.DocumentControl documentControl1;
        private System.Windows.Forms.ToolStripButton toolbtnSubmit;
        private System.Windows.Forms.ToolStripButton toolbtnUndo;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnNew;
    }
}