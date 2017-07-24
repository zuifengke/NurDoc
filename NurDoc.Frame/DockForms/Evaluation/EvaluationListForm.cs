// ***********************************************************
// 护理电子病历系统,表单文档列表窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.DockSuite;
using Heren.Common.Report;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class EvaluationListForm : DockContentBase
    {
        private EvaTypeInfo m_evaTypeInfo = null;

        /// <summary>
        /// 获取或设置当前文档列表窗口关联的文档类型对象
        /// </summary>
        [Browsable(false)]
        public EvaTypeInfo EvaTypeInfo
        {
            get { return this.m_evaTypeInfo; }
            set { this.m_evaTypeInfo = value; }
        }

        public DeptInfo m_SelectDeptInfo = null;

        /// <summary>
        /// 选择科室
        /// </summary>
        public DeptInfo SelectDeptInfo
        {
            get { return this.m_SelectDeptInfo; }
            set { this.m_SelectDeptInfo = value; }
        }

        /// <summary>
        /// 文档编辑窗口
        /// </summary>
        private EvaluationEditForm m_EvaluationEditForm = null;

        /// <summary>
        /// 获取当前显示文档编辑窗口
        /// </summary>
        [Browsable(false)]
        private EvaluationEditForm EvaluationEditForm
        {
            get
            {
                if (this.m_EvaluationEditForm == null)
                    return null;
                if (this.m_EvaluationEditForm.IsDisposed)
                    return null;
                return this.m_EvaluationEditForm;
            }
        }

        /// <summary>
        /// 获取当前文档是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                if (this.EvaluationEditForm == null)
                    return false;
                return this.EvaluationEditForm.IsModified;
            }
        }

        /// <summary>
        /// 当文档被更新完成时触发
        /// </summary>
        [Description("当文档被更新完成时触发")]
        public event EventHandler DocumentUpdated;

        protected virtual void OnDocumentUpdated(EventArgs e)
        {
            if (this.DocumentUpdated != null)
                this.DocumentUpdated(this, e);
        }

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        public EvaluationListForm()
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
        }

        public override void RefreshView()
        {
            base.RefreshView();
            this.Update();

            DateTime dtBeginTime = new DateTime(2016, 1, 1, 0, 0, 0);

            DateTime dtEndTime = SysTimeService.Instance.Now;
            dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 23, 59, 59);
            if (SystemContext.Instance.SystemOption.DocLoadTime > 0)
            {
                DateTime dtBeginShowTime = dtEndTime.AddHours(-SystemContext.Instance.SystemOption.DocLoadTime).AddTicks(1);
                if (dtBeginTime < dtBeginShowTime)
                    dtBeginTime = dtBeginShowTime;
            }

            this.m_bValueChangedEnabled = false;
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            if (this.m_evaTypeInfo == null)
                return;

            this.colRemark.Visible = this.m_evaTypeInfo.HaveRemark;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.EvaluationEditForm != null)
                this.EvaluationEditForm.Hide();

            //加载已写的病历列表
            if (!this.LoadDocumentList(null))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        /// <summary>
        /// 检查当前文档编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.EvaluationEditForm == null)
                return false;
            return this.EvaluationEditForm.CheckModifyState();
        }

        /// <summary>
        /// 加载已评估过的历史病历列表
        /// </summary>
        /// <param name="szDefaultSelectedDocID">默认选中的文档ID</param>
        /// <returns>是否加载成功</returns>
        private bool LoadDocumentList(string szDefaultSelectedEvaID)
        {
            //记录下当前行的文档ID,以便刷新后还原选中
            if (this.dataTableView1.CurrentRow != null && string.IsNullOrEmpty(szDefaultSelectedEvaID))
            {
                EvaDocInfo docInfo = this.dataTableView1.CurrentRow.Tag as EvaDocInfo;
                if (docInfo != null)
                    szDefaultSelectedEvaID = docInfo.EvaID;
            }
            this.dataTableView1.Rows.Clear();
            if (this.dataTableView1.Columns.Count <= 0)
                return true;
            this.Update();

            if (this.m_evaTypeInfo == null)
                return false;
            string szWardCode = this.m_SelectDeptInfo.DeptCode; // SystemContext.Instance.LoginUser.WardCode;
            string szEvaTypeID = this.m_evaTypeInfo.EvaTypeID;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);
            List<EvaDocInfo> lstEvaDocInfos = null;
            short shRet = EvaDocInfoService.Instance.GetEvaDocInfos(szWardCode, szEvaTypeID
                , dtBeginTime, dtEndTime, ref lstEvaDocInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("已写病历列表下载失败!");
                return false;
            }

            if (lstEvaDocInfos == null || lstEvaDocInfos.Count <= 0)
                return true;

            this.dataTableView1.SuspendLayout();
            foreach (EvaDocInfo docInfo in lstEvaDocInfos)
            {
                int index = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[index];
                row.Tag = docInfo;
                row.Cells[this.colEvaId.Index].Value = docInfo.EvaID;
                row.Cells[this.colEvaTypeName.Index].Value = docInfo.EvaTypeName;
                row.Cells[this.colModifierTime.Index].Value = docInfo.ModifyTime;
                row.Cells[this.colCreaterName.Index].Value = docInfo.CreatorName;
                row.Cells[this.colModifierName.Index].Value = docInfo.ModifierName;
                row.Cells[this.colScore.Index].Value = docInfo.Score;
                row.Cells[this.colRemark.Index].Value = docInfo.Remark;

                int nMinHeight = this.dataTableView1.RowTemplate.Height;
                this.dataTableView1.AdjustRowHeight(0, this.dataTableView1.Rows.Count, nMinHeight, (nMinHeight * 20) + 4);
                this.dataTableView1.SetRowState(row, RowState.Normal);
                if (docInfo.EvaID == szDefaultSelectedEvaID)
                    this.dataTableView1.SelectRow(row);
            }
            this.dataTableView1.ResumeLayout();

            //将滚动条自动滚动到当前行
            DataTableViewRow currentRow = this.dataTableView1.CurrentRow;
            this.dataTableView1.CurrentCell = null;
            this.dataTableView1.SelectRow(currentRow);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);

            return true;
        }

        /// <summary>
        /// 删除当前选中的文档
        /// </summary>
        /// <returns>是否删除成功</returns>
        private bool DeleteSelectedDocument()
        {
            if (this.dataTableView1.SelectedRows.Count <= 0)
                return false;
            EvaDocInfo docInfo = this.dataTableView1.SelectedRows[0].Tag as EvaDocInfo;

            DialogResult result = MessageBoxEx.ShowConfirmFormat("确认删除“{0}”的护理评价信息吗？"
                , null, docInfo.EvaTypeName);
            if (result != DialogResult.OK)
                return false;

            short shRet = EvaDocInfoService.Instance.DeleteEvaDocInfo(docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("护理评价信息删除失败!");
                return false;
            }
            this.dataTableView1.Rows.Remove(this.dataTableView1.SelectedRows[0]);
            return true;
        }

        /// <summary>
        /// 创建文档编辑器窗口
        /// </summary>
        private void CreateEvaluationEditForm()
        {
            if (this.EvaluationEditForm == null)
            {
                this.m_EvaluationEditForm = new EvaluationEditForm();
                this.m_EvaluationEditForm.ShowInTaskbar = false;
                this.m_EvaluationEditForm.MinimizeBox = false;
                this.m_EvaluationEditForm.StartPosition = FormStartPosition.CenterParent;
                this.m_EvaluationEditForm.DocumentUpdated +=
                    new EventHandler(this.EvaluationEditForm_DocumentUpdated);
            }
            this.EvaluationEditForm.TopLevel = false;
            this.EvaluationEditForm.FormBorderStyle = FormBorderStyle.None;
            this.EvaluationEditForm.Dock = DockStyle.Fill;
            this.EvaluationEditForm.Parent = this;
            this.EvaluationEditForm.Visible = true;
            this.EvaluationEditForm.BringToFront();
        }

        /// <summary>
        /// 显示指定护理文档类型对应的护理电子病历编辑器
        /// </summary>
        /// <param name="docTypeInfo">护理文档类型</param>
        private void ShowEvaluationEditForm(EvaTypeInfo evaTypeInfo)
        {
            this.CreateEvaluationEditForm();
            Application.DoEvents();
            EvaDocInfo evaDocInfo = EvaDocInfo.CreateEvaDocInfo(SystemContext.Instance.LoginUser, this.m_SelectDeptInfo, DateTime.Now, EvaTypeInfo);
            this.EvaluationEditForm.OpenDocument(evaTypeInfo, evaDocInfo, true);
        }

        /// <summary>
        /// 显示指定护理文档对应的护理电子病历编辑器
        /// </summary>
        /// <param name="docInfo">关联的护理文档信息</param>
        private void ShowEvaluationEditForm(EvaDocInfo docInfo)
        {
            this.CreateEvaluationEditForm();
            Application.DoEvents();
            this.EvaluationEditForm.OpenDocument(this.EvaTypeInfo, docInfo, false);
        }

        /// <summary>
        /// 显示指定文档类型和文档记录对应的文档编辑器窗口
        /// </summary>
        /// <param name="szDocID">文档记录ID</param>
        public void ShowEvaluationEditForm(string szEvaID)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (GlobalMethods.Misc.IsEmptyString(szEvaID))
            {
                this.ShowEvaluationEditForm(this.EvaTypeInfo);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            //EvaDocInfo docInfo = null;
            //short shRet = DocumentService.Instance.GetLatestDocInfo(szEvaID, ref docInfo);
            //if (shRet != SystemConst.ReturnValue.OK)
            //{
            //    GlobalMethods.UI.SetCursor(this, Cursors.Default);
            //    MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的表单!", szDocID));
            //    return;
            //}
            //this.ShowEvaluationEditForm(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void EvaluationEditForm_DocumentUpdated(object sender, EventArgs e)
        {
            if (this.EvaluationEditForm == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szEvaID = null;
            if (this.EvaluationEditForm.EvaDocInfo != null)
                szEvaID = this.EvaluationEditForm.EvaDocInfo.EvaID;
            this.LoadDocumentList(szEvaID);

            this.OnDocumentUpdated(e);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocumentList(null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowEvaluationEditForm(this.m_evaTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            this.toolbtnNew.PerformClick();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            NurDocInfo docInfo = this.dataTableView1.SelectedRows[0].Tag as NurDocInfo;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            EvaDocInfo docInfo = row.Tag as EvaDocInfo;
            if (docInfo == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowEvaluationEditForm(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
    }
}
