﻿namespace Heren.NurDoc.InfoLib.DockForms
{
    partial class InfoLibMenuForm
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
            this.virtualTree1 = new Heren.Common.Controls.VirtualTreeView.VirtualTree();
            this.SuspendLayout();
            // 
            // virtualTree1
            // 
            this.virtualTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualTree1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.virtualTree1.Location = new System.Drawing.Point(0, 0);
            this.virtualTree1.Name = "virtualTree1";
            this.virtualTree1.ShowColumnHeader = false;
            this.virtualTree1.ShowToolTip = false;
            this.virtualTree1.Size = new System.Drawing.Size(219, 458);
            this.virtualTree1.TabIndex = 7;
            this.virtualTree1.NodeMouseClick += new Heren.Common.Controls.VirtualTreeView.VirtualTreeEventHandler(this.virtualTree1_NodeMouseClick);
            this.virtualTree1.AfterSelect += new Heren.Common.Controls.VirtualTreeView.VirtualTreeEventHandler(this.virtualTree1_AfterSelect);
            // 
            // InfoLibMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 458);
            this.Controls.Add(this.virtualTree1);
            this.Name = "InfoLibMenuForm";
            this.Text = "InfoLibMenuTree";
            this.ResumeLayout(false);
        }

        #endregion

        private Heren.Common.Controls.VirtualTreeView.VirtualTree virtualTree1;
    }
}