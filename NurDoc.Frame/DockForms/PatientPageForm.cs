// ***********************************************************
// 护理电子病历系统,病人选项卡主窗口.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.PatPage;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class PatientPageForm : DockContentBase
    {
        private PatVisitInfo m_patientVisit = null;

        /// <summary>
        /// 获取或设置当前窗口的病人就诊信息
        /// </summary>
        [Browsable(false)]
        public PatVisitInfo PatVisitInfo
        {
            get { return this.m_patientVisit; }
        }

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.patientPageControl1 == null)
                    return false;
                if (this.patientPageControl1.IsDisposed)
                    return false;
                return this.patientPageControl1.IsModified;
            }
        }

        public PatientPageForm(MainFrame parent)
            : base(parent)
        { 
            this.InitializeComponent();
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
        }

        public override void RefreshView()
        {
            base.RefreshView();
            this.SwitchPatient(this.m_patientVisit);
        }

        public override bool CheckModifyState()
        {
            if (this.patientPageControl1 == null
                || this.patientPageControl1.IsDisposed)
            {
                return false;
            }
            return !this.patientPageControl1.SwitchPatient(null);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// 切换当前窗口中显示的病人及就诊信息
        /// </summary>
        /// <param name="patVisit">病人就诊信息</param>
        /// <returns>是否成功</returns>
        public bool SwitchPatient(PatVisitInfo patVisit)
        {
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            bool success = this.patientPageControl1.SwitchPatient(patVisit);
            if (success)
            {
                this.m_patientVisit = patVisit;
                this.RefreshWindowCaption();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        /// <summary>
        /// 根据当前病人就诊信息刷新病人窗口标题
        /// </summary>
        private void RefreshWindowCaption()
        {
            if (this.m_patientVisit == null
                || this.m_patientVisit.VisitTime == this.m_patientVisit.DefaultTime)
            {
                this.Text = "空床无病人";
                return;
            }
            this.Text = string.Format("{0}({1})"
                , this.m_patientVisit.PatientName, this.m_patientVisit.PatientID);
            this.TabSubhead = string.Format("入院时间:{0}"
                , this.m_patientVisit.VisitTime.ToString("yyyy年M月d日"));
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            bool success = this.patientPageControl1.LocateToModule(szDocTypeID, szDocID);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <param name="bshowList">是否隐藏左侧列表</param>
        /// <returns>是否成功</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID, bool bshowList)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            bool success = this.patientPageControl1.LocateToModule(szDocTypeID, szDocID,bshowList);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        private void PatientPageControl_PatientInfoChanged(object sender, EventArgs e)
        {
            this.m_patientVisit = this.patientPageControl1.PatVisitInfo;
            this.RefreshWindowCaption();
        }

        private void PatientPageControl_PatientInfoChanging(object sender, PatientInfoChangingEventArgs e)
        {
            PatientPageForm patientPageForm = this.MainFrame.GetPatientPageForm(e.NewPatVisit);
            if (patientPageForm != null && patientPageForm != this)
            {
                e.Cancel = true;
                patientPageForm.DockHandler.Activate();
            }
        }

       /// <summary>
        /// 获取活动文档
        /// </summary>
        public IDockContent ActiveDocument
        {
            get
            {
                return this.patientPageControl1.ActiveDocument;
            }
        }
    }
}
