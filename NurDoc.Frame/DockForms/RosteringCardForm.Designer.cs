namespace Heren.NurDoc.Frame.DockForms
{
    partial class RosteringCardForm
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
            this.arrowSplitter1 = new Heren.Common.Controls.ArrowSplitter();
            this.formControl2 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.whiteBoardControl1 = new Heren.NurDoc.Frame.Controls.WhiteBoardControl();
            this.cardListView1 = new Heren.Common.Controls.CardView.CardListView();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.SuspendLayout();
            // 
            // arrowSplitter1
            // 
            this.arrowSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.arrowSplitter1.DotArrowGap = 8;
            this.arrowSplitter1.Location = new System.Drawing.Point(480, 70);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(10, 516);
            this.arrowSplitter1.TabIndex = 8;
            this.arrowSplitter1.TabStop = false;
            this.arrowSplitter1.Visible = false;
            // 
            // formControl2
            // 
            this.formControl2.BackColor = System.Drawing.Color.White;
            this.formControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.formControl2.Location = new System.Drawing.Point(490, 70);
            this.formControl2.Name = "formControl2";
            this.formControl2.Size = new System.Drawing.Size(272, 516);
            this.formControl2.TabIndex = 7;
            this.formControl2.Visible = false;
            // 
            // whiteBoardControl1
            // 
            this.whiteBoardControl1.BackColor = System.Drawing.Color.White;
            this.whiteBoardControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.whiteBoardControl1.Location = new System.Drawing.Point(0, 0);
            this.whiteBoardControl1.Name = "whiteBoardControl1";
            this.whiteBoardControl1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 4);
            this.whiteBoardControl1.Size = new System.Drawing.Size(762, 70);
            this.whiteBoardControl1.TabIndex = 4;
            // 
            // cardListView1
            // 
            this.cardListView1.CardSize = new System.Drawing.Size(204, 150);
            this.cardListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardListView1.Location = new System.Drawing.Point(0, 0);
            this.cardListView1.Name = "cardListView1";
            this.cardListView1.Size = new System.Drawing.Size(762, 586);
            this.cardListView1.TabIndex = 5;
            this.cardListView1.Text = "cardListView1";
            this.cardListView1.CardElementClick += new Heren.Common.Controls.CardView.CardElementEventHandler(this.cardListView1_CardElementClick);
            this.cardListView1.CardDoubleClick += new Heren.Common.Controls.CardView.CardEventHandler(this.cardListView1_CardDoubleClick);
            // 
            // formControl1
            // 
            this.formControl1.BackColor = System.Drawing.Color.White;
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(0, 70);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(480, 516);
            this.formControl1.TabIndex = 10;
            // 
            // RosteringCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 586);
            this.Controls.Add(this.formControl1);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.formControl2);
            this.Controls.Add(this.whiteBoardControl1);
            this.Controls.Add(this.cardListView1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "RosteringCardForm";
            this.Text = "护士排班一览表";
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl2;
        private Heren.NurDoc.Frame.Controls.WhiteBoardControl whiteBoardControl1;
        private Heren.Common.Controls.CardView.CardListView cardListView1;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;

    }
}