namespace Heren.NurDoc.Frame.DockForms
{
    partial class QCEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QCEditForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollblRecordTime = new System.Windows.Forms.ToolStripLabel();
            this.tooldtpRecordTime = new Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker();
            this.toollblCreatorName = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtCreatorName = new System.Windows.Forms.ToolStripTextBox();
            this.toolbtnSave = new System.Windows.Forms.ToolStripButton();
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
            this.toollblRecordTime,
            this.tooldtpRecordTime,
            this.toollblCreatorName,
            this.tooltxtCreatorName,
            this.toolbtnSave,
            this.toolbtnReturn});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(738, 32);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "baseToolStrip1";
            // 
            // toollblRecordTime
            // 
            this.toollblRecordTime.AutoSize = false;
            this.toollblRecordTime.Name = "toollblRecordTime";
            this.toollblRecordTime.Size = new System.Drawing.Size(65, 22);
            this.toollblRecordTime.Text = " 记录时间：";
            // 
            // tooldtpRecordTime
            // 
            this.tooldtpRecordTime.AutoSize = false;
            this.tooldtpRecordTime.BackColor = System.Drawing.Color.White;
            this.tooldtpRecordTime.Name = "tooldtpRecordTime";
            this.tooldtpRecordTime.ShowSecond = false;
            this.tooldtpRecordTime.Size = new System.Drawing.Size(130, 22);
            this.tooldtpRecordTime.Text = "2012/2/20 19:21:21";
            this.tooldtpRecordTime.Value = new System.DateTime(2012, 2, 20, 19, 21, 21, 0);
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
            this.tooltxtCreatorName.Enabled = false;
            this.tooltxtCreatorName.Name = "tooltxtCreatorName";
            this.tooltxtCreatorName.Size = new System.Drawing.Size(60, 32);
            // 
            // toolbtnSave
            // 
            this.toolbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnSave.Image")));
            this.toolbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnSave.Name = "toolbtnSave";
            this.toolbtnSave.Size = new System.Drawing.Size(36, 29);
            this.toolbtnSave.Text = "保存";
            this.toolbtnSave.Click += new System.EventHandler(this.toolbtnSave_Click);
            // 
            // toolbtnReturn
            // 
            this.toolbtnReturn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtnReturn.Image = ((System.Drawing.Image)(resources.GetObject("toolbtnReturn.Image")));
            this.toolbtnReturn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnReturn.Name = "toolbtnReturn";
            this.toolbtnReturn.Size = new System.Drawing.Size(36, 29);
            this.toolbtnReturn.Text = "返回";
            this.toolbtnReturn.Click += new System.EventHandler(this.toolbtnReturn_Click);
            // 
            // documentControl1
            // 
            this.documentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentControl1.Document = null;
            this.documentControl1.Location = new System.Drawing.Point(0, 32);
            this.documentControl1.Name = "documentControl1";
            this.documentControl1.Size = new System.Drawing.Size(738, 482);
            this.documentControl1.TabIndex = 3;
            this.documentControl1.Text = "documentControl1";
            // 
            // QSMEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 514);
            this.Controls.Add(this.documentControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "QSMEditForm";
            this.Text = "QSMEditForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toollblRecordTime;
        private Heren.Common.Controls.ToolStrip.ToolStripDateTimePicker tooldtpRecordTime;
        private System.Windows.Forms.ToolStripLabel toollblCreatorName;
        private System.Windows.Forms.ToolStripTextBox tooltxtCreatorName;
        private System.Windows.Forms.ToolStripButton toolbtnReturn;
        private System.Windows.Forms.ToolStripButton toolbtnSave;
        private Heren.NurDoc.PatPage.Document.DocumentControl documentControl1;
    }
}