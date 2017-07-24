namespace Heren.NurDoc.Frame.DockForms
{
    partial class PatientListForm
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
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.colNursingClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBedNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataTableView1
            // 
            this.dataTableView1.AllowDrop = true;
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNursingClass,
            this.colBedNumber,
            this.colName});
            this.dataTableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableView1.Font = new System.Drawing.Font("宋体", 10F);
            this.dataTableView1.Location = new System.Drawing.Point(0, 0);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.ReadOnly = true;
            this.dataTableView1.RowHeadersVisible = false;
            this.dataTableView1.Size = new System.Drawing.Size(200, 470);
            this.dataTableView1.TabIndex = 1;
            this.dataTableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTableView1_CellDoubleClick);
            this.dataTableView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataTableView1_CellPainting);
            // 
            // colNursingClass
            // 
            this.colNursingClass.FillWeight = 30F;
            this.colNursingClass.HeaderText = "";
            this.colNursingClass.Name = "colNursingClass";
            this.colNursingClass.ReadOnly = true;
            this.colNursingClass.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colNursingClass.Width = 30;
            // 
            // colBedNumber
            // 
            this.colBedNumber.FillWeight = 64F;
            this.colBedNumber.HeaderText = "床号";
            this.colBedNumber.Name = "colBedNumber";
            this.colBedNumber.ReadOnly = true;
            this.colBedNumber.Width = 64;
            // 
            // colName
            // 
            this.colName.FillWeight = 90F;
            this.colName.HeaderText = "姓名";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 90;
            // 
            // PatientListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(200, 470);
            this.Controls.Add(this.dataTableView1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PatientListForm";
            this.Text = "病人一览表";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNursingClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBedNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    }
}