namespace NurdocControl
{
    partial class NurPatContrl
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.patientPageControl1 = new Heren.NurDoc.PatPage.PatientPageControl();
            this.SuspendLayout();
            // 
            // patientPageControl1
            // 
            this.patientPageControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.patientPageControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientPageControl1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.patientPageControl1.Location = new System.Drawing.Point(0, 0);
            this.patientPageControl1.Name = "patientPageControl1";
            this.patientPageControl1.Size = new System.Drawing.Size(935, 423);
            this.patientPageControl1.TabIndex = 0;
            this.patientPageControl1.Load += new System.EventHandler(this.patientPageControl1_Load);
            // 
            // NurPatContrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.patientPageControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NurPatContrl";
            this.Size = new System.Drawing.Size(935, 423);
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.NurDoc.PatPage.PatientPageControl patientPageControl1;
    }
}
