// ***********************************************************
// 护理电子病历系统,病人窗口下各种护理文书窗口基类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.PatPage
{
    internal partial class DockContentBase : DockContent
    {
        private PatientPageControl m_patientPageControl = null;

        /// <summary>
        /// 获取或设置主程序控件
        /// </summary>
        [Browsable(false)]
        protected virtual PatientPageControl PatientPage
        {
            get { return this.m_patientPageControl; }
            set { this.m_patientPageControl = value; }
        }

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public virtual bool IsModified
        {
            get { return false; }
        }

        private bool m_bPatientInfoUpdated = true;

        /// <summary>
        /// 获取当前病人信息是否已经改变,改变后则刷新视图
        /// </summary>
        [Browsable(false)]
        public virtual bool PatientInfoUpdated
        {
            get { return this.m_bPatientInfoUpdated; }
        }

        private bool m_bEventEnabledOnHidden = false;

        /// <summary>
        /// 获取或设置当当前窗口处于隐藏状态时,是否需要处理上层发来的事件消息
        /// </summary>
        [Browsable(false)]
        public virtual bool EventEnabledOnHidden
        {
            get { return this.m_bEventEnabledOnHidden; }
            set { this.m_bEventEnabledOnHidden = value; }
        }

        private int m_index = 0;

        /// <summary>
        /// 获取或设置当前窗口选项卡在DockPanel控件中的显示索引编号
        /// </summary>
        [Browsable(false)]
        public virtual int Index
        {
            get { return this.m_index; }
            set { this.m_index = value; }
        }

        public DockContentBase()
            : this(null)
        {
        }

        public DockContentBase(PatientPageControl patientPageControl)
        {
            this.m_patientPageControl = patientPageControl;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.BindingSystemEvents();
        }

        protected virtual void BindingSystemEvents()
        {
            if (this.m_patientPageControl == null || this.m_patientPageControl.IsDisposed)
                return;

            EventHandler activeContentChangedEventHandler =
                new EventHandler(this.PatientPageControl_ActiveContentChanged);
            this.m_patientPageControl.ActiveContentChanged -= activeContentChangedEventHandler;
            this.m_patientPageControl.ActiveContentChanged += activeContentChangedEventHandler;

            EventHandler activeDocumentChangedEventHandler =
                new EventHandler(this.PatientPageControl_ActiveDocumentChanged);
            this.m_patientPageControl.ActiveDocumentChanged -= activeDocumentChangedEventHandler;
            this.m_patientPageControl.ActiveDocumentChanged += activeDocumentChangedEventHandler;

            EventHandler patientInfoChangedEventHandler =
                new EventHandler(this.PatientPageControl_PatientInfoChanged);
            this.m_patientPageControl.PatientInfoChanged -= patientInfoChangedEventHandler;
            this.m_patientPageControl.PatientInfoChanged += patientInfoChangedEventHandler;

            PatientInfoChangingEventHandler patientInfoChangingEventHandler =
                new PatientInfoChangingEventHandler(this.PatientPageControl_PatientInfoChanging);
            this.m_patientPageControl.PatientInfoChanging -= patientInfoChangingEventHandler;
            this.m_patientPageControl.PatientInfoChanging += patientInfoChangingEventHandler;
        }

        private void PatientPageControl_PatientInfoChanging(object sender, PatientInfoChangingEventArgs e)
        {
            if (!e.Cancel)
                this.OnPatientInfoChanging(e);
        }

        private void PatientPageControl_PatientInfoChanged(object sender, EventArgs e)
        {
            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            if (patVisit != null)
            {
                this.m_bPatientInfoUpdated = true;
                if (!this.DockHandler.IsHidden || this.m_bEventEnabledOnHidden)
                    this.OnPatientInfoChanged();
            }
        }

        private void PatientPageControl_ActiveContentChanged(object sender, EventArgs e)
        {
            if (!this.DockHandler.IsHidden || this.m_bEventEnabledOnHidden)
                this.OnActiveContentChanged();
            //
            if (this.Pane != null && !this.Pane.IsDisposed && this.Pane.ActiveContent == this)
                this.m_bPatientInfoUpdated = false;
        }

        private void PatientPageControl_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (!this.DockHandler.IsHidden || this.m_bEventEnabledOnHidden)
                this.OnActiveDocumentChanged();
        }

        public virtual void RefreshView()
        {
            this.m_bPatientInfoUpdated = false;
        }

        /// <summary>
        /// 检查当前数据编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public virtual bool CheckModifyState()
        {
            return false;
        }

        /// <summary>
        /// 病人信息改变前自动调用的方法
        /// 在子类中重写此方法来刷新当前的数据
        /// </summary>
        protected virtual void OnPatientInfoChanging(PatientInfoChangingEventArgs e)
        {
        }

        /// <summary>
        /// 病人信息改变时自动调用的方法
        /// 在子类中重写此方法来刷新当前的数据
        /// </summary>
        protected virtual void OnPatientInfoChanged()
        {
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

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="docTypeInfo">病历类型信息</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public virtual bool LocateToModule(DocTypeInfo docTypeInfo, string szDocID)
        {
            return false;
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="docTypeInfo">病历类型信息</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public virtual bool LocateToModule(DocTypeInfo docTypeInfo, string szDocID, bool bshowList)
        {
            return false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (this.m_patientPageControl == null || this.m_patientPageControl.IsDisposed)
                return;
            this.m_patientPageControl.PatientInfoChanging -=
                new PatientInfoChangingEventHandler(this.PatientPageControl_PatientInfoChanging);
            this.m_patientPageControl.PatientInfoChanged -= new EventHandler(this.PatientPageControl_PatientInfoChanged);
            this.m_patientPageControl.ActiveContentChanged -= new EventHandler(this.PatientPageControl_ActiveContentChanged);
            this.m_patientPageControl.ActiveDocumentChanged -= new EventHandler(this.PatientPageControl_ActiveDocumentChanged);
        }
    }
}
