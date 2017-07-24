namespace Heren.NurDoc.Utilities.Templet
{
    partial class WordTempTreeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WordTempTreeForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtnNewFolder = new System.Windows.Forms.ToolStripButton();
            this.toolbtnNewTemplet = new System.Windows.Forms.ToolStripButton();
            this.toolbtnRename = new System.Windows.Forms.ToolStripButton();
            this.toolbtnShareLevel = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolmnuSharePersonal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuShareDepart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolmnuShareHospital = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new Heren.Common.Controls.TreeViewControl();
            this.cmenuTextTemplet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewTemplet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSharePersonal = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShareDepart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShareHospital = new System.Windows.Forms.ToolStripMenuItem();
            this.smallIconList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.cmenuTextTemplet.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtnNewFolder,
            this.toolbtnNewTemplet,
            this.toolbtnRename,
            this.toolbtnShareLevel,
            this.toolbtnDelete,
            this.toolStripButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(282, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolbtnNewFolder
            // 
            this.toolbtnNewFolder.AutoSize = false;
            this.toolbtnNewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbtnNewFolder.Image = global::Heren.NurDoc.Utilities.Properties.Resources.NewFolder;
            this.toolbtnNewFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNewFolder.Name = "toolbtnNewFolder";
            this.toolbtnNewFolder.Size = new System.Drawing.Size(32, 24);
            this.toolbtnNewFolder.Text = "新建目录";
            this.toolbtnNewFolder.Click += new System.EventHandler(this.toolbtnNewFolder_Click);
            // 
            // toolbtnNewTemplet
            // 
            this.toolbtnNewTemplet.AutoSize = false;
            this.toolbtnNewTemplet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbtnNewTemplet.Image = global::Heren.NurDoc.Utilities.Properties.Resources.NewDoc;
            this.toolbtnNewTemplet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnNewTemplet.Name = "toolbtnNewTemplet";
            this.toolbtnNewTemplet.Size = new System.Drawing.Size(32, 24);
            this.toolbtnNewTemplet.Text = "新建模板";
            this.toolbtnNewTemplet.Click += new System.EventHandler(this.toolbtnNewTemplet_Click);
            // 
            // toolbtnRename
            // 
            this.toolbtnRename.AutoSize = false;
            this.toolbtnRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbtnRename.Image = global::Heren.NurDoc.Utilities.Properties.Resources.Rename;
            this.toolbtnRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnRename.Name = "toolbtnRename";
            this.toolbtnRename.Size = new System.Drawing.Size(32, 24);
            this.toolbtnRename.Text = "重命名";
            this.toolbtnRename.ToolTipText = "重命名选中项";
            this.toolbtnRename.Click += new System.EventHandler(this.toolbtnRename_Click);
            // 
            // toolbtnShareLevel
            // 
            this.toolbtnShareLevel.AutoSize = false;
            this.toolbtnShareLevel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbtnShareLevel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolmnuSharePersonal,
            this.toolmnuShareDepart,
            this.toolmnuShareHospital});
            this.toolbtnShareLevel.Image = global::Heren.NurDoc.Utilities.Properties.Resources.Share;
            this.toolbtnShareLevel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnShareLevel.Name = "toolbtnShareLevel";
            this.toolbtnShareLevel.Size = new System.Drawing.Size(42, 24);
            this.toolbtnShareLevel.Text = "共享等级";
            this.toolbtnShareLevel.Click += new System.EventHandler(this.toolbtnShareLevel_DropDownOpening);
            // 
            // toolmnuSharePersonal
            // 
            this.toolmnuSharePersonal.Name = "toolmnuSharePersonal";
            this.toolmnuSharePersonal.Size = new System.Drawing.Size(130, 22);
            this.toolmnuSharePersonal.Text = "个人使用";
            this.toolmnuSharePersonal.Click += new System.EventHandler(this.toolmnuSharePersonal_Click);
            // 
            // toolmnuShareDepart
            // 
            this.toolmnuShareDepart.Name = "toolmnuShareDepart";
            this.toolmnuShareDepart.Size = new System.Drawing.Size(130, 22);
            this.toolmnuShareDepart.Text = "科室共享";
            this.toolmnuShareDepart.Click += new System.EventHandler(this.toolmnuShareDepart_Click);
            // 
            // toolmnuShareHospital
            // 
            this.toolmnuShareHospital.Name = "toolmnuShareHospital";
            this.toolmnuShareHospital.Size = new System.Drawing.Size(130, 22);
            this.toolmnuShareHospital.Text = "全院共享";
            this.toolmnuShareHospital.Click += new System.EventHandler(this.toolmnuShareHospital_Click);
            // 
            // toolbtnDelete
            // 
            this.toolbtnDelete.AutoSize = false;
            this.toolbtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbtnDelete.Image = global::Heren.NurDoc.Utilities.Properties.Resources.Cancel;
            this.toolbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnDelete.Name = "toolbtnDelete";
            this.toolbtnDelete.Size = new System.Drawing.Size(32, 24);
            this.toolbtnDelete.Text = "删除";
            this.toolbtnDelete.ToolTipText = "删除选中项";
            this.toolbtnDelete.Click += new System.EventHandler(this.toolbtnDelete_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.ContextMenuStrip = this.cmenuTextTemplet;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.smallIconList;
            this.treeView1.ItemHeight = 18;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(282, 466);
            this.treeView1.TabIndex = 3;
            this.treeView1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCollapse);
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_BeforeLabelEdit);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            // 
            // cmenuTextTemplet
            // 
            this.cmenuTextTemplet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewFolder,
            this.mnuNewTemplet,
            this.toolStripSeparator1,
            this.mnuRename,
            this.mnuDelete,
            this.toolStripSeparator2,
            this.mnuSharePersonal,
            this.mnuShareDepart,
            this.mnuShareHospital});
            this.cmenuTextTemplet.Name = "cmenuSmallTemplet";
            this.cmenuTextTemplet.Size = new System.Drawing.Size(180, 170);
            this.cmenuTextTemplet.Opening += new System.ComponentModel.CancelEventHandler(this.cmenuTextTemplet_Opening);
            // 
            // mnuNewFolder
            // 
            this.mnuNewFolder.Name = "mnuNewFolder";
            this.mnuNewFolder.Size = new System.Drawing.Size(179, 22);
            this.mnuNewFolder.Text = "新建目录";
            this.mnuNewFolder.Click += new System.EventHandler(this.mnuNewFolder_Click);
            // 
            // mnuNewTemplet
            // 
            this.mnuNewTemplet.Name = "mnuNewTemplet";
            this.mnuNewTemplet.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuNewTemplet.Size = new System.Drawing.Size(179, 22);
            this.mnuNewTemplet.Text = "新建模板";
            this.mnuNewTemplet.Click += new System.EventHandler(this.mnuNewTemplet_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // mnuRename
            // 
            this.mnuRename.Name = "mnuRename";
            this.mnuRename.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnuRename.Size = new System.Drawing.Size(179, 22);
            this.mnuRename.Text = "重命名";
            this.mnuRename.Click += new System.EventHandler(this.mnuRename_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.mnuDelete.Size = new System.Drawing.Size(179, 22);
            this.mnuDelete.Text = "删除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // mnuSharePersonal
            // 
            this.mnuSharePersonal.Name = "mnuSharePersonal";
            this.mnuSharePersonal.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mnuSharePersonal.Size = new System.Drawing.Size(179, 22);
            this.mnuSharePersonal.Text = "个人使用";
            this.mnuSharePersonal.Click += new System.EventHandler(this.mnuSharePersonal_Click);
            // 
            // mnuShareDepart
            // 
            this.mnuShareDepart.Name = "mnuShareDepart";
            this.mnuShareDepart.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mnuShareDepart.Size = new System.Drawing.Size(179, 22);
            this.mnuShareDepart.Text = "科室共享";
            this.mnuShareDepart.Click += new System.EventHandler(this.mnuShareDepart_Click);
            // 
            // mnuShareHospital
            // 
            this.mnuShareHospital.Name = "mnuShareHospital";
            this.mnuShareHospital.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.mnuShareHospital.Size = new System.Drawing.Size(179, 22);
            this.mnuShareHospital.Text = "全院共享";
            this.mnuShareHospital.Click += new System.EventHandler(this.mnuShareHospital_Click);
            // 
            // smallIconList
            // 
            this.smallIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallIconList.ImageStream")));
            this.smallIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.smallIconList.Images.SetKeyName(0, "FolderClose.png");
            this.smallIconList.Images.SetKeyName(1, "FolderOpen.png");
            this.smallIconList.Images.SetKeyName(2, "Share.png");
            this.smallIconList.Images.SetKeyName(3, "SmallTemplet.png");
            // 
            // WordTempTreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(282, 493);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "WordTempTreeForm";
            this.Text = "模板列表";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmenuTextTemplet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolbtnNewFolder;
        private System.Windows.Forms.ToolStripButton toolbtnNewTemplet;
        private System.Windows.Forms.ToolStripButton toolbtnRename;
        private System.Windows.Forms.ToolStripDropDownButton toolbtnShareLevel;
        private System.Windows.Forms.ToolStripMenuItem toolmnuSharePersonal;
        private System.Windows.Forms.ToolStripMenuItem toolmnuShareDepart;
        private System.Windows.Forms.ToolStripMenuItem toolmnuShareHospital;
        private System.Windows.Forms.ToolStripButton toolbtnDelete;
        private Heren.Common.Controls.TreeViewControl treeView1;
        private System.Windows.Forms.ImageList smallIconList;
        private System.Windows.Forms.ContextMenuStrip cmenuTextTemplet;
        private System.Windows.Forms.ToolStripMenuItem mnuNewFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuNewTemplet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuRename;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuSharePersonal;
        private System.Windows.Forms.ToolStripMenuItem mnuShareDepart;
        private System.Windows.Forms.ToolStripMenuItem mnuShareHospital;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}