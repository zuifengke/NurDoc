namespace NurDoc.HealthTech
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColRowNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUpdateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cboQuickFind = new System.Windows.Forms.ComboBox();
            this.btnLock = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(228)))), ((int)(((byte)(242)))));
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(946, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColRowNo,
            this.ColName,
            this.ColType,
            this.ColFileSize,
            this.ColUpdateTime,
            this.ColStatue,
            this.ColFrom});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView1.Location = new System.Drawing.Point(0, 76);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(946, 456);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // ColRowNo
            // 
            this.ColRowNo.HeaderText = "序号";
            this.ColRowNo.Name = "ColRowNo";
            this.ColRowNo.ReadOnly = true;
            this.ColRowNo.Width = 80;
            // 
            // ColName
            // 
            this.ColName.HeaderText = "名称";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Width = 300;
            // 
            // ColType
            // 
            this.ColType.HeaderText = "类型";
            this.ColType.Name = "ColType";
            this.ColType.ReadOnly = true;
            // 
            // ColFileSize
            // 
            this.ColFileSize.HeaderText = "大小";
            this.ColFileSize.Name = "ColFileSize";
            this.ColFileSize.ReadOnly = true;
            // 
            // ColUpdateTime
            // 
            this.ColUpdateTime.HeaderText = "更新日期";
            this.ColUpdateTime.Name = "ColUpdateTime";
            this.ColUpdateTime.ReadOnly = true;
            this.ColUpdateTime.Width = 150;
            // 
            // ColStatue
            // 
            this.ColStatue.HeaderText = "状态";
            this.ColStatue.Name = "ColStatue";
            this.ColStatue.ReadOnly = true;
            // 
            // ColFrom
            // 
            this.ColFrom.HeaderText = "来源";
            this.ColFrom.Name = "ColFrom";
            this.ColFrom.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "当前位置：";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblAddress.Location = new System.Drawing.Point(83, 50);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(0, 12);
            this.lblAddress.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Image = global::Heren.NurDoc.HealthTech.Properties.Resources.Query;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(796, 45);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(54, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "搜索";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cboQuickFind
            // 
            this.cboQuickFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboQuickFind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboQuickFind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboQuickFind.FormattingEnabled = true;
            this.cboQuickFind.Location = new System.Drawing.Point(538, 47);
            this.cboQuickFind.Name = "cboQuickFind";
            this.cboQuickFind.Size = new System.Drawing.Size(252, 20);
            this.cboQuickFind.TabIndex = 8;
            this.cboQuickFind.SelectedIndexChanged += new System.EventHandler(this.cboQuickFind_SelectedIndexChanged);
            this.cboQuickFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboQuickFind_KeyDown);
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLock.ImageIndex = 0;
            this.btnLock.ImageList = this.imageList1;
            this.btnLock.Location = new System.Drawing.Point(863, 12);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(83, 23);
            this.btnLock.TabIndex = 9;
            this.btnLock.Text = "安全模式";
            this.btnLock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "lock .png");
            this.imageList1.Images.SetKeyName(1, "unlock.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(946, 533);
            this.Controls.Add(this.btnLock);
            this.Controls.Add(this.cboQuickFind);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "健康教育";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRowNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUpdateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFrom;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cboQuickFind;
        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.ImageList imageList1;
    }
}