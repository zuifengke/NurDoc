namespace Heren.NurDoc.Config.DockForms
{
    partial class EvaluationTableManagementForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EvaluationTableManagementForm));
            this.treeViewControl1 = new Heren.Common.Controls.TreeViewControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.munCreateEvaFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCreatEvaInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowEvaTypeInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.munDeleteEvaInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.munRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.arrowSplitter1 = new Heren.Common.Controls.ArrowSplitter();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.munNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.munCancelModify = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.dataTableView1 = new Heren.Common.Controls.TableView.DataTableView();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.colItemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemSort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colItemDefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemInCount = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colItemTextBlod = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colItemReadOnly = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colItemEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colItemContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewControl1
            // 
            this.treeViewControl1.AllowDrop = true;
            this.treeViewControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeViewControl1.HideSelection = false;
            this.treeViewControl1.ImageIndex = 0;
            this.treeViewControl1.ImageList = this.imageList1;
            this.treeViewControl1.LabelEdit = true;
            this.treeViewControl1.Location = new System.Drawing.Point(0, 0);
            this.treeViewControl1.Name = "treeViewControl1";
            this.treeViewControl1.SelectedImageIndex = 0;
            this.treeViewControl1.Size = new System.Drawing.Size(138, 437);
            this.treeViewControl1.TabIndex = 0;
            this.treeViewControl1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewControl1_NodeMouseDoubleClick);
            this.treeViewControl1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewControl1_AfterCollapse);
            this.treeViewControl1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewControl1_AfterLabelEdit);
            this.treeViewControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewControl1_DragDrop);
            this.treeViewControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewControl1_DragEnter);
            this.treeViewControl1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewControl1_AfterExpand);
            this.treeViewControl1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewControl1_ItemDrag);
            this.treeViewControl1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewControl1_DragOver);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munCreateEvaFolder,
            this.mnuCreatEvaInfo,
            this.mnuShowEvaTypeInfo,
            this.toolStripSeparator1,
            this.munDeleteEvaInfo,
            this.munRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 120);
            // 
            // munCreateEvaFolder
            // 
            this.munCreateEvaFolder.Name = "munCreateEvaFolder";
            this.munCreateEvaFolder.Size = new System.Drawing.Size(136, 22);
            this.munCreateEvaFolder.Text = "新建目录";
            this.munCreateEvaFolder.Click += new System.EventHandler(this.munCreateEvaFolder_Click);
            // 
            // mnuCreatEvaInfo
            // 
            this.mnuCreatEvaInfo.Name = "mnuCreatEvaInfo";
            this.mnuCreatEvaInfo.Size = new System.Drawing.Size(136, 22);
            this.mnuCreatEvaInfo.Text = "新建评价表";
            this.mnuCreatEvaInfo.Click += new System.EventHandler(this.mnuCreatEvaInfo_Click);
            // 
            // mnuShowEvaTypeInfo
            // 
            this.mnuShowEvaTypeInfo.Name = "mnuShowEvaTypeInfo";
            this.mnuShowEvaTypeInfo.Size = new System.Drawing.Size(136, 22);
            this.mnuShowEvaTypeInfo.Text = "评价表属性";
            this.mnuShowEvaTypeInfo.Click += new System.EventHandler(this.mnuShowEvaTypeInfo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // munDeleteEvaInfo
            // 
            this.munDeleteEvaInfo.Name = "munDeleteEvaInfo";
            this.munDeleteEvaInfo.Size = new System.Drawing.Size(136, 22);
            this.munDeleteEvaInfo.Text = "删除选中项";
            this.munDeleteEvaInfo.Click += new System.EventHandler(this.munDeleteEvaInfo_Click);
            // 
            // munRefresh
            // 
            this.munRefresh.Name = "munRefresh";
            this.munRefresh.Size = new System.Drawing.Size(136, 22);
            this.munRefresh.Text = "刷新";
            this.munRefresh.Click += new System.EventHandler(this.munRefresh_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "FolderClose.png");
            this.imageList1.Images.SetKeyName(1, "FolderOpen.png");
            this.imageList1.Images.SetKeyName(2, "Templet.png");
            // 
            // arrowSplitter1
            // 
            this.arrowSplitter1.Location = new System.Drawing.Point(138, 0);
            this.arrowSplitter1.Name = "arrowSplitter1";
            this.arrowSplitter1.Size = new System.Drawing.Size(10, 437);
            this.arrowSplitter1.TabIndex = 1;
            this.arrowSplitter1.TabStop = false;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munNewItem,
            this.toolStripSeparator2,
            this.munCancelModify});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(125, 54);
            // 
            // munNewItem
            // 
            this.munNewItem.Name = "munNewItem";
            this.munNewItem.Size = new System.Drawing.Size(124, 22);
            this.munNewItem.Text = "新增";
            this.munNewItem.Click += new System.EventHandler(this.munNewItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(121, 6);
            // 
            // munCancelModify
            // 
            this.munCancelModify.Name = "munCancelModify";
            this.munCancelModify.Size = new System.Drawing.Size(124, 22);
            this.munCancelModify.Text = "取消修改";
            this.munCancelModify.Click += new System.EventHandler(this.munCancelModify_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.dataTableView1);
            this.panel1.Controls.Add(this.btnAddItem);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(148, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(886, 437);
            this.panel1.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::Heren.NurDoc.Config.Properties.Resources.SaveDoc;
            this.btnSave.Location = new System.Drawing.Point(848, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(26, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dataTableView1
            // 
            this.dataTableView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTableView1.ColumnHeadersHeight = 24;
            this.dataTableView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemNo,
            this.colItemSort,
            this.colItemType,
            this.colItemDefaultValue,
            this.colItemScore,
            this.colItemInCount,
            this.colItemTextBlod,
            this.colItemReadOnly,
            this.colItemEnable,
            this.colItemContent});
            this.dataTableView1.ContextMenuStrip = this.contextMenuStrip2;
            this.dataTableView1.Location = new System.Drawing.Point(0, 32);
            this.dataTableView1.Name = "dataTableView1";
            this.dataTableView1.RowHeadersWidth = 36;
            this.dataTableView1.Size = new System.Drawing.Size(886, 402);
            this.dataTableView1.TabIndex = 4;
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.Image = global::Heren.NurDoc.Config.Properties.Resources.Add;
            this.btnAddItem.Location = new System.Drawing.Point(817, 3);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(25, 23);
            this.btnAddItem.TabIndex = 3;
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // colItemNo
            // 
            this.colItemNo.FillWeight = 60F;
            this.colItemNo.HeaderText = "主键";
            this.colItemNo.Name = "colItemNo";
            this.colItemNo.ReadOnly = true;
            this.colItemNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemNo.Width = 60;
            // 
            // colItemSort
            // 
            this.colItemSort.FillWeight = 240F;
            this.colItemSort.HeaderText = "序号";
            this.colItemSort.Name = "colItemSort";
            this.colItemSort.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemSort.Width = 40;
            // 
            // colItemType
            // 
            this.colItemType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colItemType.FillWeight = 70F;
            this.colItemType.HeaderText = "类型";
            this.colItemType.Items.AddRange(new object[] {
            "选择框",
            "输入框"});
            this.colItemType.Name = "colItemType";
            this.colItemType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colItemType.Width = 70;
            // 
            // colItemDefaultValue
            // 
            this.colItemDefaultValue.HeaderText = "默认值";
            this.colItemDefaultValue.Name = "colItemDefaultValue";
            this.colItemDefaultValue.Width = 50;
            // 
            // colItemScore
            // 
            this.colItemScore.FillWeight = 400F;
            this.colItemScore.HeaderText = "分值";
            this.colItemScore.Name = "colItemScore";
            this.colItemScore.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemScore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colItemScore.Width = 40;
            // 
            // colItemInCount
            // 
            this.colItemInCount.HeaderText = "是否计算项";
            this.colItemInCount.Name = "colItemInCount";
            this.colItemInCount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemInCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colItemInCount.Width = 70;
            // 
            // colItemTextBlod
            // 
            this.colItemTextBlod.HeaderText = "粗体";
            this.colItemTextBlod.Name = "colItemTextBlod";
            this.colItemTextBlod.Width = 40;
            // 
            // colItemReadOnly
            // 
            this.colItemReadOnly.HeaderText = "只读";
            this.colItemReadOnly.Name = "colItemReadOnly";
            this.colItemReadOnly.Width = 50;
            // 
            // colItemEnable
            // 
            this.colItemEnable.HeaderText = "是否作废";
            this.colItemEnable.Name = "colItemEnable";
            this.colItemEnable.Width = 60;
            // 
            // colItemContent
            // 
            this.colItemContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemContent.FillWeight = 300F;
            this.colItemContent.HeaderText = "内容";
            this.colItemContent.Name = "colItemContent";
            this.colItemContent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EvaluationTableManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 437);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.arrowSplitter1);
            this.Controls.Add(this.treeViewControl1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "EvaluationTableManagementForm";
            this.Text = "EvaluationTableManagementForm";
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTableView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Heren.Common.Controls.TreeViewControl treeViewControl1;
        private Heren.Common.Controls.ArrowSplitter arrowSplitter1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuCreatEvaInfo;
        private System.Windows.Forms.ToolStripMenuItem mnuShowEvaTypeInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem munNewItem;
        private System.Windows.Forms.ImageList imageList1;
        private Heren.Common.Controls.TableView.DataTableView dataTableView1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStripMenuItem munCreateEvaFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem munDeleteEvaInfo;
        private System.Windows.Forms.ToolStripMenuItem munRefresh;
        private System.Windows.Forms.ToolStripMenuItem munCancelModify;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemSort;
        private System.Windows.Forms.DataGridViewComboBoxColumn colItemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemDefaultValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemScore;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colItemInCount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colItemTextBlod;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colItemReadOnly;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colItemEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemContent;
    }
}