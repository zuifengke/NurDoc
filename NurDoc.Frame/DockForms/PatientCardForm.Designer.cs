namespace Heren.NurDoc.Frame.DockForms
{
    partial class PatientCardForm
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
            this.arrowSplitter1 = new Heren.Common.Controls.ArrowSplitter();
            this.cardListView1 = new Heren.Common.Controls.CardView.CardListView();
            this.formControl2 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.formControl1 = new Heren.NurDoc.Utilities.Forms.FormControl();
            this.whiteBoardControl1 = new Heren.NurDoc.Frame.Controls.WhiteBoardControl();
            this.SuspendLayout();
            // 
            // arrowSplitter1
            // 
            this.arrowSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.arrowSplitter1.DotArrowGap = 8;
            this.arrowSplitter1.Location = new System.Drawing.Point(493, 70);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(10, 410);
            this.arrowSplitter1.TabIndex = 3;
            this.arrowSplitter1.TabStop = false;
            this.arrowSplitter1.Visible = false;
            // 
            // cardListView1
            // 
            this.cardListView1.CardSize = new System.Drawing.Size(204, 150);
            this.cardListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardListView1.Location = new System.Drawing.Point(0, 0);
            this.cardListView1.Name = "cardListView1";
            this.cardListView1.Size = new System.Drawing.Size(775, 480);
            this.cardListView1.TabIndex = 0;
            this.cardListView1.Text = "cardListView1";
            this.cardListView1.CardElementClick += new Heren.Common.Controls.CardView.CardElementEventHandler(this.cardListView1_CardElementClick);
            this.cardListView1.CardDoubleClick += new Heren.Common.Controls.CardView.CardEventHandler(this.cardListView1_CardDoubleClick);
            // 
            // formControl2
            // 
            this.formControl2.BackColor = System.Drawing.Color.White;
            this.formControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.formControl2.Location = new System.Drawing.Point(503, 70);
            this.formControl2.Name = "formControl2";
            this.formControl2.Size = new System.Drawing.Size(272, 410);
            this.formControl2.TabIndex = 2;
            this.formControl2.Visible = false;
            // 
            // formControl1
            // 
            this.formControl1.BackColor = System.Drawing.Color.White;
            this.formControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formControl1.Location = new System.Drawing.Point(0, 70);
            this.formControl1.Name = "formControl1";
            this.formControl1.Size = new System.Drawing.Size(775, 410);
            this.formControl1.TabIndex = 1;
            this.formControl1.CustomEvent += new Heren.Common.Forms.Editor.CustomEventHandler(this.formControl1_CustomEvent);
            // 
            // whiteBoardControl1
            // 
            this.whiteBoardControl1.BackColor = System.Drawing.Color.White;
            this.whiteBoardControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.whiteBoardControl1.Location = new System.Drawing.Point(0, 0);
            this.whiteBoardControl1.Name = "whiteBoardControl1";
            this.whiteBoardControl1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 4);
            this.whiteBoardControl1.Size = new System.Drawing.Size(775, 70);
            this.whiteBoardControl1.TabIndex = 0;
            // 
            // PatientCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(775, 480);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.formControl2);
            this.Controls.Add(this.formControl1);
            this.Controls.Add(this.whiteBoardControl1);
            this.Controls.Add(this.cardListView1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PatientCardForm";
            this.Load += new System.EventHandler(this.PatientCardForm_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
        private Heren.Common.Controls.CardView.CardListView cardListView1;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl2;
        private Heren.NurDoc.Utilities.Forms.FormControl formControl1;
        private Heren.NurDoc.Frame.Controls.WhiteBoardControl whiteBoardControl1;

    }
}