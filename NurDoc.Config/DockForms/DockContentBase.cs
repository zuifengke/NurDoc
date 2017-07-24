// ***********************************************************
// 护理病历配置管理系统,配置窗口基类.
// Author : YangMingkun, Date : 2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;

namespace Heren.NurDoc.Config.DockForms
{
    public partial class DockContentBase : DockContent
    {
        private MainForm m_mainForm = null;

        /// <summary>
        /// 获取或设置主程序控件
        /// </summary>
        [Browsable(false)]
        protected virtual MainForm MainForm
        {
            get { return this.m_mainForm; }
            set { this.m_mainForm = value; }
        }

        public DockContentBase()
            : this(null)
        {
        }

        public DockContentBase(MainForm parent)
        {
            this.m_mainForm = parent;
            if (this.m_mainForm == null || this.m_mainForm.IsDisposed)
                return;
            this.m_mainForm.ActiveContentChanged +=
                new EventHandler(this.MainForm_ActiveContentChanged);
            this.m_mainForm.ActiveDocumentChanged +=
                new EventHandler(this.MainForm_ActiveDocumentChanged);
        }

        //对于需要记忆位置的停靠窗口,请将控件创建代码放入Load事件内
        //这样当窗口被构造时,就不会加载界面元素,用以提高系统启动速度
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.TabPageContextMenuStrip = this.contextMenuStrip;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                this.CommitModify();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// 刷新视图方法
        /// </summary>
        public virtual void OnRefreshView()
        {
        }

        /// <summary>
        /// 获取是否有未保存的记录
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool HasUncommit()
        {
            return false;
        }

        /// <summary>
        /// 保存当前对记录的修改
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool CommitModify()
        {
            return true;
        }

        /// <summary>
        /// 检查是否有需要保存的数据
        /// </summary>
        /// <returns>是否保存成功</returns>
        public virtual bool CheckModifiedData()
        {
            if (!this.HasUncommit())
                return true;
            this.DockHandler.Activate();
            DialogResult result = MessageBoxEx.ShowQuestion("当前有未保存的修改,是否保存？");
            if (result == DialogResult.Cancel)
                return false;
            else if (result == DialogResult.Yes)
                return this.CommitModify();
            return true;
        }

        /// <summary>
        /// 停靠窗口的活动状态改变时自动调用的方法
        /// 在子类中重写此方法来初始化当前的数据
        /// </summary>
        protected virtual void OnActiveContentChanged()
        {
        }

        /// <summary>
        /// 当文档打开后自动调用的方法
        /// 在子类中重写此方法来初始化当前的数据
        /// </summary>
        protected virtual void OnActiveDocumentChanged()
        {
        }

        protected void ShowStatusMessage(string szMessage)
        {
            if (this.m_mainForm != null && !this.m_mainForm.IsDisposed)
                this.m_mainForm.ShowStatusMessage(szMessage);
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            if (this.DockHandler == null)
            {
                this.Close();
            }
            else
            {
                this.Pane.Focus();
                this.DockHandler.Close();
            }
        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            this.OnRefreshView();
        }

        private void MainForm_ActiveContentChanged(object sender, EventArgs e)
        {
            if (!this.DockHandler.IsHidden)
                this.OnActiveContentChanged();
        }

        private void MainForm_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (!this.DockHandler.IsHidden)
                this.OnActiveDocumentChanged();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = !this.CheckModifiedData();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            this.m_mainForm.ActiveContentChanged -= new EventHandler(this.MainForm_ActiveContentChanged);
            this.m_mainForm.ActiveDocumentChanged -= new EventHandler(this.MainForm_ActiveDocumentChanged);
        }
    }
}