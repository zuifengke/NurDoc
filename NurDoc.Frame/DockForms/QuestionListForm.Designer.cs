namespace Heren.NurDoc.Frame.DockForms
{
    partial class QuestionListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestionListForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolBtnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolBtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolBtnQCPass = new System.Windows.Forms.ToolStripButton();
            this.dtView = new System.Windows.Forms.DataGridView();
            this.colQuestionStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQCEventType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFeedBackInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnAdd,
            this.toolBtnUpdate,
            this.toolBtnDelete,
            this.toolBtnQCPass});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolBtnAdd
            // 
            this.toolBtnAdd.Image = global::Heren.NurDoc.Frame.Properties.Resources.Add;
            this.toolBtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnAdd.Name = "toolBtnAdd";
            this.toolBtnAdd.Size = new System.Drawing.Size(52, 22);
            this.toolBtnAdd.Text = "新增";
            this.toolBtnAdd.Click += new System.EventHandler(this.toolBtnAdd_Click);
            // 
            // toolBtnUpdate
            // 
            this.toolBtnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnUpdate.Image")));
            this.toolBtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnUpdate.Name = "toolBtnUpdate";
            this.toolBtnUpdate.Size = new System.Drawing.Size(52, 22);
            this.toolBtnUpdate.Text = "修改";
            this.toolBtnUpdate.Click += new System.EventHandler(this.toolBtnUpdate_Click);
            // 
            // toolBtnDelete
            // 
            this.toolBtnDelete.Image = global::Heren.NurDoc.Frame.Properties.Resources.Delete;
            this.toolBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnDelete.Name = "toolBtnDelete";
            this.toolBtnDelete.Size = new System.Drawing.Size(52, 22);
            this.toolBtnDelete.Text = "删除";
            this.toolBtnDelete.Click += new System.EventHandler(this.toolBtnDelete_Click);
            // 
            // toolBtnQCPass
            // 
            this.toolBtnQCPass.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnQCPass.Image")));
            this.toolBtnQCPass.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnQCPass.Name = "toolBtnQCPass";
            this.toolBtnQCPass.Size = new System.Drawing.Size(76, 22);
            this.toolBtnQCPass.Text = "问题合格";
            this.toolBtnQCPass.Click += new System.EventHandler(this.toolBtnQCPass_Click);
            // 
            // dtView
            // 
            this.dtView.AllowUserToAddRows = false;
            this.dtView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtView.BackgroundColor = System.Drawing.Color.White;
            this.dtView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colQuestionStatus,
            this.colQCEventType,
            this.colDetailInfo,
            this.colDocTitle,
            this.colCheckTime,
            this.colFeedBackInfo,
            this.colConfirmTime});
            this.dtView.Location = new System.Drawing.Point(0, 28);
            this.dtView.MultiSelect = false;
            this.dtView.Name = "dtView";
            this.dtView.RowHeadersVisible = false;
            this.dtView.RowTemplate.Height = 23;
            this.dtView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtView.Size = new System.Drawing.Size(800, 154);
            this.dtView.TabIndex = 1;
            // 
            // colQuestionStatus
            // 
            this.colQuestionStatus.HeaderText = "问题状态";
            this.colQuestionStatus.Name = "colQuestionStatus";
            this.colQuestionStatus.Width = 80;
            // 
            // colQCEventType
            // 
            this.colQCEventType.HeaderText = "问题类型";
            this.colQCEventType.Name = "colQCEventType";
            this.colQCEventType.Width = 150;
            // 
            // colDetailInfo
            // 
            this.colDetailInfo.HeaderText = "质检信息";
            this.colDetailInfo.Name = "colDetailInfo";
            this.colDetailInfo.Width = 260;
            // 
            // colDocTitle
            // 
            this.colDocTitle.HeaderText = "问题主题";
            this.colDocTitle.Name = "colDocTitle";
            // 
            // colCheckTime
            // 
            this.colCheckTime.HeaderText = "记录时间";
            this.colCheckTime.Name = "colCheckTime";
            this.colCheckTime.Width = 150;
            // 
            // colFeedBackInfo
            // 
            this.colFeedBackInfo.HeaderText = "护士反馈";
            this.colFeedBackInfo.Name = "colFeedBackInfo";
            this.colFeedBackInfo.Width = 200;
            // 
            // colConfirmTime
            // 
            this.colConfirmTime.HeaderText = "反馈时间";
            this.colConfirmTime.Name = "colConfirmTime";
            // 
            // QuestionListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 181);
            this.Controls.Add(this.dtView);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "QuestionListForm";
            this.Text = "质检信息";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolBtnAdd;
        private System.Windows.Forms.ToolStripButton toolBtnUpdate;
        private System.Windows.Forms.ToolStripButton toolBtnDelete;
        private System.Windows.Forms.ToolStripButton toolBtnQCPass;
        private System.Windows.Forms.DataGridView dtView;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuestionStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQCEventType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeedBackInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmTime;
    }
}