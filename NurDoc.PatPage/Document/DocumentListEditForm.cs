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
using Heren.Common.Forms.Editor;

namespace Heren.NurDoc.PatPage.Document
{
    internal partial class DocumentListEditForm : DockContentBase
    {
        /// <summary>
        /// 是否显示分值行
        /// </summary>
        private bool bshowScoreRow = false;

        /// <summary>
        /// 是否是打印form
        /// </summary>
        private bool bSingle = false;

        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// 获取或设置当前文档列表窗口关联的文档类型对象
        /// </summary>
        [Browsable(false)]
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
            set { this.m_docTypeInfo = value; }
        }

        /// <summary>
        /// 记录最后一次新增时间，用于防止频繁新增引起的docid主键重复问题
        /// </summary>
        private Hashtable m_hashLastestTime = new Hashtable();

        ///// <summary>
        ///// 获取或设置当前文档的最新新增时间
        ///// </summary>
        //[Browsable(false)]
        //public DateTime DtLastestAddTime
        //{
        //    set { this.m_dtLastestAddTime = value; }
        //}

        /// <summary>
        /// 获取当前文档是否已经被修改
        /// </summary>
        [Browsable(false)]
        public override bool IsModified
        {
            get
            {
                return this.CheckModified();
            }
        }

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 用于存放当前文档列表中自定义列的哈希表
        /// </summary>
        private Dictionary<string, DataGridViewColumn> m_columnsTable = null;

        public DocumentListEditForm(PatientPageControl patientPageControl)
            : base(patientPageControl)
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
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RefreshView();
            Application.DoEvents();
            this.HandleUILayout();

            if (this.toolcboPatDeptList.IsDisposed != true)
                this.toolcboPatDeptList.Items.Add(string.Empty);

            this.toolcboPatDeptList.DropDown +=
                new EventHandler(this.toolcboPatDeptList_DropDown);
            this.toolcboPatDeptList.SelectedIndexChanged +=
                new System.EventHandler(this.toolcboPatDeptList_SelectedIndexChanged);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override void RefreshView()
        {
            base.RefreshView();
            this.HandleUILayout();
            this.Update();

            DateTime dtBeginTime = SysTimeService.Instance.Now.AddDays(-1).Date;
            if (PatientTable.Instance.ActivePatient != null)
                dtBeginTime = PatientTable.Instance.ActivePatient.VisitTime.Date;
            DateTime dtEndTime = SysTimeService.Instance.Now;
            dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 23, 59, 59);

            this.m_bValueChangedEnabled = false;
            this.tooldtpDateFrom.Value = dtBeginTime;
            this.tooldtpDateTo.Value = dtEndTime;
            this.m_bValueChangedEnabled = true;

            if (this.m_docTypeInfo == null)
                return;

            GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.WaitCursor);

            //显示或者隐藏说明文档
            this.ViewDocument(this.m_docTypeInfo);

            //加载显示的列方案
            this.LoadGridViewColumns();

            //加载已写的病历列表
            if (!this.LoadDocumentList(null))
            {
                GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
                return;
            }

            //绑定数值行
            this.FreezeScoreRow();

            GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
        }

        protected override void OnPatientInfoChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientInfoUpdated)
                this.RefreshView();
        }

        protected override void OnPatientInfoChanging(PatientInfoChangingEventArgs e)
        {
            if (this.IsModified && this.DockHandler != null)
                this.DockHandler.Activate();
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// 处理界面上一些控件的可见性事宜
        /// </summary>
        private void HandleUILayout()
        {
            string szDocTypeID = null;
            if (this.m_docTypeInfo != null)
                szDocTypeID = this.m_docTypeInfo.DocTypeID;
            bool bIsPrintable = FormCache.Instance.IsFormListPrintable(szDocTypeID);
            if (this.toolbtnPrint.Visible != bIsPrintable)
                this.toolbtnPrint.Visible = bIsPrintable;
        }

        /// <summary>
        /// 检查当前文档编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (!this.IsModified)
                return false;

            string message = string.Format("{0}已经被修改,是否保存？", this.m_docTypeInfo.DocTypeName);
            DialogResult result = MessageBoxEx.ShowQuestion(message);
            if (result == DialogResult.Yes)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                bool success = this.CommitModify();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return !success;
            }
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// 判断当前文档是否已修改
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckModified()
        {
            foreach (DataTableViewRow row in dtvDocInfos.Rows)
            {
                if (!this.dtvDocInfos.IsNormalRow(row))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 查询当前病人所经过的病区hash表
        /// </summary>
        /// <returns>是否查询成功</returns>
        private bool LoadPatientDeptList()
        {
            this.toolcboPatDeptList.Items.Clear();
            this.toolcboPatDeptList.Items.Add(string.Empty);
            this.Update();
            if (SystemContext.Instance.LoginUser == null)
                return false;
            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            List<DeptInfo> lstDeptInfos = null;
            short shRet = PatVisitService.Instance.GetPatientDeptList(szPatientID, szVisitID, ref lstDeptInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("查询当前病人病区列表失败!");
                return false;
            }
            if (lstDeptInfos == null || lstDeptInfos.Count <= 0)
                return false;
            foreach (DeptInfo deptInfo in lstDeptInfos)
                this.toolcboPatDeptList.Items.Add(deptInfo);
            return true;
        }

        /// <summary>
        /// 加载已评估过的历史病历列表
        /// </summary>
        /// <param name="szDefaultSelectedDocID">默认选中的文档ID</param>
        /// <returns>是否加载成功</returns>
        private bool LoadDocumentList(string szDefaultSelectedDocID)
        {
            //记录下当前行的文档ID,以便刷新后还原选中
            if (this.dtvDocInfos.CurrentRow != null
                && string.IsNullOrEmpty(szDefaultSelectedDocID))
            {
                NurDocInfo docInfo = this.dtvDocInfos.CurrentRow.Tag as NurDocInfo;
                if (docInfo != null)
                    szDefaultSelectedDocID = docInfo.DocSetID;
            }
            this.dtvDocInfos.Rows.Clear();

            if (this.dtvDocInfos.Columns.Count <= 0)
                return true;
            this.Update();

            if (this.m_docTypeInfo == null || PatientTable.Instance.ActivePatient == null)
                return false;

            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);
            NurDocList lstDocInfos = null;
            short shRet = DocumentService.Instance.GetDocInfos(szPatientID, szVisitID, szDocTypeID
                , dtBeginTime, dtEndTime, ref lstDocInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("已写病历列表下载失败!");
                return false;
            }

            if (lstDocInfos == null || lstDocInfos.Count <= 0)
                return true;

            DeptInfo currentDept = this.toolcboPatDeptList.SelectedItem as DeptInfo;
            
            this.dtvDocInfos.SuspendLayout();
            foreach (NurDocInfo docInfo in lstDocInfos)
            {
                if (currentDept != null && docInfo.WardCode != currentDept.DeptCode)
                    continue;
                int index = this.dtvDocInfos.Rows.Add();
                DataTableViewRow row = this.dtvDocInfos.Rows[index];
                row.Tag = docInfo;
                row.Cells[this.colRecordTime.Index].Value = docInfo.RecordTime.ToString("yyyy-MM-dd HH:mm:ss");

                this.LoadSummaryData(row);
                this.dtvDocInfos.SetRowState(row, RowState.Normal);

                int nMinHeight = this.dtvDocInfos.RowTemplate.Height;
                this.dtvDocInfos.AdjustRowHeight(0, this.dtvDocInfos.Rows.Count, nMinHeight, (nMinHeight * 20) + 4);
                this.dtvDocInfos.SetRowState(row, RowState.Normal);
                if (docInfo.DocSetID == szDefaultSelectedDocID)
                    this.dtvDocInfos.SelectRow(row);
            }
            if (!string.IsNullOrEmpty(this.m_docTypeInfo.SortColumn)
                && this.m_docTypeInfo.SortMode != SortMode.None
                && dtvDocInfos.Columns.Contains(this.m_docTypeInfo.SortColumn))
            {
                ListSortDirection ListSortDirection = ListSortDirection.Ascending;
                if (this.m_docTypeInfo.SortMode == SortMode.Descending)
                    ListSortDirection = ListSortDirection.Descending;
                this.dtvDocInfos.Sort(dtvDocInfos.Columns[this.m_docTypeInfo.SortColumn], ListSortDirection);
            }
            else
            {
                this.dtvDocInfos.Sort(dtvDocInfos.Columns[this.colRecordTime.Index], ListSortDirection.Ascending);
            }
            this.dtvDocInfos.ResumeLayout();
            return true;
        }

        /// <summary>
        /// 加载文档摘要数据列集合
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewColumns()
        {
            //清除已显示的各摘要列
            if (this.m_docTypeInfo == null)
                return false;
            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szDocTypeID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            if (lstGridViewColumns == null)
                lstGridViewColumns = new List<GridViewColumn>();

            if (this.m_columnsTable == null)
                this.m_columnsTable = new Dictionary<string, DataGridViewColumn>();
            this.m_columnsTable.Clear();

            int index = this.dtvDocInfos.Columns.Count - 1;
            while (index >= 0)
            {
                DataGridViewColumn column = this.dtvDocInfos.Columns[index--];
                if (column == this.colRecordTime)
                    continue;
                this.dtvDocInfos.Columns.Remove(column);
            }
            this.dtvDocInfos.Groups.Clear();

            DataTableViewGroup preDataTableViewGroup = null;
            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                if (gridViewColumn == null)
                    continue;
                //DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                DataGridViewColumn column = null;
                if (gridViewColumn.ColumnType != ServerData.DataType.CHECKBOX && !GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnItems))
                {
                    string[] items = gridViewColumn.ColumnItems.Split(';');
                    if (items.Length <= 1)
                    {
                        column = new DataGridViewTextBoxColumn();
                    }
                    else
                    {
                        column = new ComboBoxColumn();
                        ((ComboBoxColumn)column).Items.AddRange(items);
                        if (items != null && items.Length > 0 && string.IsNullOrEmpty(items[0]))
                            ((ComboBoxColumn)column).DisplayStyle = ComboBoxStyle.DropDownList;
                    }
                }
                else if (gridViewColumn.ColumnType == ServerData.DataType.BOOLEAN)
                {
                    column = new DataGridViewCheckBoxColumn();
                }
                else
                {
                    column = new DataGridViewTextBoxColumn();
                }
                column.Tag = gridViewColumn;
                column.Width = gridViewColumn.ColumnWidth;
                column.Visible = gridViewColumn.IsVisible;

                if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnTag))
                    column.Name = gridViewColumn.ColumnTag;
                else
                    column.Name = gridViewColumn.ColumnName;
                if (GlobalMethods.Misc.IsEmptyString(column.Name))
                    continue;

                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                if (!gridViewColumn.IsMiddle)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                string szHeaderText = string.Empty;
                string szTooltiptext = string.Empty;
                if (gridViewColumn.ColumnName.Contains("||"))
                {
                    szHeaderText = gridViewColumn.ColumnName.Substring(0, gridViewColumn.ColumnName.IndexOf("||"));
                    szTooltiptext = gridViewColumn.ColumnName.Substring(gridViewColumn.ColumnName.IndexOf("||") + 2);
                }
                else
                {
                    szHeaderText = gridViewColumn.ColumnName;
                }
                if (GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnUnit))
                {
                    column.HeaderText = szHeaderText;
                    if (!GlobalMethods.Misc.IsEmptyString(szTooltiptext))
                    {
                        column.ToolTipText = szTooltiptext;
                    }
                }
                else
                {
                    column.HeaderText = column.HeaderText = string.Format("{0}({1})", szHeaderText, gridViewColumn.ColumnUnit); ;
                    if (!GlobalMethods.Misc.IsEmptyString(szTooltiptext))
                    {
                        column.ToolTipText = szTooltiptext;
                    }
                }

                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //配置列组名,实现多表头
                if (gridViewColumn.ColumnGroup != string.Empty)
                {
                    if (preDataTableViewGroup == null
                        || preDataTableViewGroup.Text != gridViewColumn.ColumnGroup)
                    {
                        int i = this.dtvDocInfos.Groups.Add();
                        preDataTableViewGroup = this.dtvDocInfos.Groups[i];
                        preDataTableViewGroup.Text = gridViewColumn.ColumnGroup;
                        preDataTableViewGroup.BeginColumn = gridViewColumn.ColumnIndex + 1;
                        preDataTableViewGroup.EndColumn = gridViewColumn.ColumnIndex + 1;
                    }
                    else if (preDataTableViewGroup.Text == gridViewColumn.ColumnGroup)
                    {
                        preDataTableViewGroup.EndColumn = gridViewColumn.ColumnIndex + 1;
                    }
                }

                this.dtvDocInfos.Columns.Add(column);
                if (!this.m_columnsTable.ContainsKey(column.Name))
                    this.m_columnsTable.Add(column.Name, column);
            }
            return true;
        }

        private void FreezeScoreRow()
        {
            bshowScoreRow = false;
            if (this.dtvDocInfos.Rows.Count > 0)
            {
                this.dtvDocInfos.Rows.InsertCopy(0, 0);
            }
            else
            {
                this.dtvDocInfos.Rows.Add();
            }
            GridViewColumn gridViewColumn = null;
            for (int index = 1; index < this.dtvDocInfos.Columns.Count; index++)
            {
                if (this.dtvDocInfos.Columns[index].Tag is GridViewColumn == false)
                {
                    continue;
                }
                gridViewColumn = this.dtvDocInfos.Columns[index].Tag as GridViewColumn;
                if (gridViewColumn.ColumnType == "数值")
                {
                    this.dtvDocInfos.Rows[0].Cells[index].Value = gridViewColumn.ColumnItems;
                    this.bshowScoreRow = true;
                }
            }
            if (bshowScoreRow == true)
            {
                this.dtvDocInfos.Rows[0].Cells[0].Value = "分值";
                this.dtvDocInfos.SetRowState(this.dtvDocInfos.Rows[0], RowState.Normal);
                this.dtvDocInfos.Rows[0].Frozen = true;
                this.dtvDocInfos.Rows[0].ReadOnly = true;
            }
            else
            {
                this.dtvDocInfos.Rows.RemoveAt(0);
            }
        }

        /// </summary>
        /// <param name="row">指定行</param>
        /// <returns>是否加载成功</returns>
        private bool LoadSummaryData(DataTableViewRow row)
        {
            if (row == null || row.Tag == null)
                return false;
            NurDocInfo docInfo = row.Tag as NurDocInfo;

            string szDocSetID = docInfo.DocSetID;
            List<SummaryData> lstSummaryData = null;
            short shRet = DocumentService.Instance.GetSummaryData(szDocSetID, false, ref lstSummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            if (lstSummaryData == null || lstSummaryData.Count <= 0)
                return true;

            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData == null || string.IsNullOrEmpty(summaryData.DataName))
                    continue;
                if (this.m_columnsTable != null && this.m_columnsTable.ContainsKey(summaryData.DataName))
                {
                    DataGridViewColumn column = this.m_columnsTable[summaryData.DataName];
                    row.Cells[column.Index].Value = summaryData.DataValue;
                }
            }
            return true;
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocumentList(null);

            //绑定数值行
            this.FreezeScoreRow();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.AddNewGridRow(this.m_docTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.dtvDocInfos.EndEdit();
            this.CommitModify();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnDelete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.AddNewGridRow(this.m_docTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.DeleteSelectedDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuScore_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.ScoreRow())
            {
                this.dtvDocInfos.SetRowState(this.dtvDocInfos.CurrentRow, RowState.Update);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dtvDocInfos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex <= 0 || (e.RowIndex < 0 && !bshowScoreRow) || (e.RowIndex < 1 && bshowScoreRow))
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            GridViewColumn gridViewColumn = (GridViewColumn)this.dtvDocInfos.Columns[e.ColumnIndex].Tag;
            if (gridViewColumn.ColumnType == ServerData.DataType.NUMERIC)
            {
                string[] items = gridViewColumn.ColumnItems.Split(';');
                if (GlobalMethods.Misc.IsEmptyString(this.dtvDocInfos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) && items.Length <= 1)
                {
                    this.dtvDocInfos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = gridViewColumn.ColumnItems;
                    this.dtvDocInfos.EndEdit();
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dtvDocInfos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex <= 0 || (e.RowIndex < 0 && !bshowScoreRow) || (e.RowIndex < 1 && bshowScoreRow))
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            GridViewColumn gridViewColumn = (GridViewColumn)this.dtvDocInfos.Columns[e.ColumnIndex].Tag;
            if (gridViewColumn.ColumnType == ServerData.DataType.CHECKBOX)
            {
                CheckBoxListForm chkForm = new CheckBoxListForm();
                chkForm.Text = gridViewColumn.ColumnName;
                chkForm.ArrCheckItem = gridViewColumn.ColumnItems.Split(';');
                if (chkForm.ShowDialog() == DialogResult.OK)
                {
                    this.dtvDocInfos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = chkForm.StrResult;
                    this.dtvDocInfos.EndEdit();
                }
            }

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 评分
        /// </summary>
        /// <returns>bool</returns>
        private bool ScoreRow()
        {
            if (this.dtvDocInfos.CurrentRow == null)
            {
                MessageBoxEx.Show("请选择表格中的其中一行");
                return false;
            }
            if (!this.m_columnsTable.ContainsKey("总分"))
            {
                MessageBoxEx.Show("表格中不存在总分列");
                return false;
            }
            if (this.dtvDocInfos.CurrentRow.Index == 0 && bshowScoreRow)
            {
                return false;
            }
            this.dtvDocInfos.EndEdit();
            int score = 0;
            GridViewColumn gridViewColumn = null;
            for (int index = 1; index <= this.dtvDocInfos.Columns.Count - 1; index++)
            {
                if (this.dtvDocInfos.Columns[index].Tag is GridViewColumn == false)
                {
                    continue;
                }
                gridViewColumn = this.dtvDocInfos.Columns[index].Tag as GridViewColumn;
                if (gridViewColumn.ColumnType == "数值" && gridViewColumn.ColumnTag != "总分")
                {
                    score += GlobalMethods.Convert.StringToValue(this.dtvDocInfos.CurrentRow.Cells[index].Value, 0);
                }
            }

            this.dtvDocInfos.CurrentRow.Cells["总分"].Value = score;
            return true;
        }

        /// <summary>
        /// 根据指定的病历类型创建新的病历
        /// </summary>
        /// <param name="docTypeInfo">病历类型信息</param>
        /// <returns>NurDocInfo</returns>
        public bool AddNewGridRow(DocTypeInfo docTypeInfo)
        {
            if (!this.CheckDocIdPrimary())
            {
                MessageBoxEx.Show("两次新增之间相差最少10秒！");
                return false;
            }
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (docTypeInfo == null)
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.CanCreateNurDoc())
            {
                MessageBoxEx.Show("您没有权限新增病历文书！");
                return false;
            }

            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            DocumentInfo document = DocumentInfo.Create(docTypeInfo, patVisit, null);

            int iIndex = this.dtvDocInfos.Rows.Add();
            DataTableViewRow row = this.dtvDocInfos.Rows[iIndex];
            row.Tag = document.ToDocInfo();
            row.Cells[this.colRecordTime.Index].Value = DateTime.Now.ToString();
            if (dtvDocInfos.Columns.Contains("护士签名"))
                row.Cells["护士签名"].Value = document.CreatorName;            
            this.dtvDocInfos.Focus();
            this.dtvDocInfos.SelectRow(row);
            this.dtvDocInfos.SetRowState(row, RowState.New);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 用于判断频繁新增引起的docid主键重复问题
        /// </summary>
        /// <returns>Bool</returns>
        private bool CheckDocIdPrimary()
        {
            if (!m_hashLastestTime.ContainsKey(this.m_docTypeInfo.DocTypeID))
            {
                m_hashLastestTime.Add(this.m_docTypeInfo.DocTypeID, DateTime.MinValue.ToString());
            }
            DateTime dtLastTime = Convert.ToDateTime(this.m_hashLastestTime[this.m_docTypeInfo.DocTypeID]);
            if (dtLastTime != null)
            {
                TimeSpan ts = new TimeSpan();
                ts = DateTime.Now - dtLastTime;
                if (ts.TotalSeconds < 10)
                {
                    return false;
                }
                this.m_hashLastestTime[this.m_docTypeInfo.DocTypeID] = DateTime.Now.ToString();
                return true;
            }
            return true;
        }

        /// <summary>
        /// 保存当前窗口的数据修改
        /// </summary>
        /// <returns>bool</returns>
        public bool CommitModify()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!this.dtvDocInfos.EndEdit())
                return false;
            int nRowsCount = this.dtvDocInfos.Rows.Count;
            WorkProcess.Instance.Initialize(this, nRowsCount, "正在更新数据，请稍候...");

            foreach (DataTableViewRow row in this.dtvDocInfos.Rows)
            {
                WorkProcess.Instance.Show(row.Index, false);
                if (!row.IsModifiedRow && !row.IsDeletedRow && !row.IsNewRow)
                    continue;

                if (!this.SaveDocument(row))
                {
                    this.dtvDocInfos.SelectRow(row);
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowError("数据保存失败!");
                    return false;
                }
                else
                {
                    this.dtvDocInfos.SetRowState(row, RowState.Normal);
                }
            }
            WorkProcess.Instance.Close();

            //重新刷新数据以保证数据绑定的完整性（主要是tag）
            this.LoadDocumentList(null);
            //绑定数值行
            this.FreezeScoreRow();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="row">行数据</param>
        /// <returns>boolean</returns>
        private bool SaveDocument(DataTableViewRow row)
        {
            if (row == null)
                return false;
            RowState status = this.dtvDocInfos.IsNewRow(row) ? RowState.New : RowState.Update;
            NurDocInfo nurDocInfo = row.Tag as NurDocInfo;
            DocumentInfo documentInfo = DocumentInfo.Create(nurDocInfo);
            documentInfo.SummaryData.AddRange(this.GetSummaryData(row.Tag as NurDocInfo, this.GetKeyDataList(row)));
            //根据记录时间修改文档的修改时间用于在筛选是能筛选正确时间
            DateTime recordTime = Convert.ToDateTime(row.Cells[this.colRecordTime.Index].Value);
            if (this.CommitDocument(documentInfo, status, recordTime, ref nurDocInfo))
            {
                row.Tag = nurDocInfo;
                return true;
            }
            return false;            
        }

        /// <summary>
        /// 将当前病历提交到服务器
        /// </summary>
        /// <param name="document">病历信息</param>
        /// <param name="status">行状态</param>
        /// <param name="recordTime">记录修改时间</param>
        /// <returns>是否提交成功</returns>
        public bool CommitDocument(DocumentInfo document, RowState status, DateTime recordTime,ref NurDocInfo newNurDocInfo)
        {
            if (document == null)
                return false;

            short shRet = SystemConst.ReturnValue.OK;
            if (status == RowState.New)
            {
                NurDocInfo docInfo = document.ToDocInfo();
                docInfo.RecordTime = recordTime;
                shRet = DocumentService.Instance.SaveDoc(docInfo, null);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("无法保存病历文档,保存至服务器失败!");
                    return false;
                }
                newNurDocInfo = docInfo;
            }
            else
            {
                document.HandleDocumentEdit(SystemContext.Instance.LoginUser);

                NurDocInfo docInfo = document.ToDocInfo();
                docInfo.RecordTime = recordTime;
                string szDocID = document.DocID;
                if (document.DocState == DocumentState.Revise)
                    szDocID = document.GetPrevVersionID();
                shRet = DocumentService.Instance.UpdateDoc(szDocID, docInfo, null, null);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("无法更新病历文档,更新至服务器失败!");
                    return false;
                }
                newNurDocInfo = docInfo;
            }

            //保存当前病历摘要数据列表
            shRet = DocumentService.Instance.SaveSummaryData(document.DocSetID, document.SummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
                //MessageBoxEx.Show("病历保存成功,但摘要数据保存失败!");
            return true;
        }

        /// <summary>
        /// 获取当前表单中的摘要数据
        /// </summary>
        /// <param name="nurDocInfo">文档信息</param>
        /// <param name="lstKeyData">摘要信息</param>
        /// <returns>摘要数据列表</returns>
        private List<SummaryData> GetSummaryData(NurDocInfo nurDocInfo, List<KeyData> lstKeyData)
        {
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            if (nurDocInfo == null)
                return lstSummaryData;

            if (lstKeyData == null)
                return lstSummaryData;

            IEnumerator<KeyData> enumerator = lstKeyData.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SummaryData summaryData = new SummaryData();
                summaryData.DocID = nurDocInfo.DocSetID;
                summaryData.DocTypeID = nurDocInfo.DocTypeID;
                summaryData.PatientID = nurDocInfo.PatientID;
                summaryData.VisitID = nurDocInfo.VisitID;
                summaryData.RecordID = nurDocInfo.RecordID;
                summaryData.WardCode = nurDocInfo.WardCode;

                summaryData.SubID = nurDocInfo.SubID;
                if (!GlobalMethods.Misc.IsEmptyString(enumerator.Current.Tag))
                    summaryData.SubID = enumerator.Current.Tag;
                summaryData.DataName = enumerator.Current.Name;
                summaryData.DataCode = enumerator.Current.Code;
                summaryData.DataType = enumerator.Current.Type;
                summaryData.DataUnit = enumerator.Current.Unit;
                if (enumerator.Current.RecordTime == null)
                    summaryData.DataTime = nurDocInfo.RecordTime;
                else
                    summaryData.DataTime = enumerator.Current.RecordTime.Value.AddSeconds(-enumerator.Current.RecordTime.Value.Second);
                if (enumerator.Current.Value != null)
                    summaryData.DataValue = enumerator.Current.Value.ToString();

                summaryData.Category = enumerator.Current.Category;
                summaryData.ContainsTime = enumerator.Current.ContainsTime;
                summaryData.Remarks = enumerator.Current.Remarks;
                lstSummaryData.Add(summaryData);
            }
            return lstSummaryData;
        }

        /// <summary>
        /// 获取界面信息到keydata中
        /// </summary>
        /// <param name="row">行数据</param>
        /// <returns>List<KeyData></returns>
        private List<KeyData> GetKeyDataList(DataTableViewRow row)
        {
            List<KeyData> lstKeyData = new List<KeyData>();
            DataGridViewColumn column = new DataGridViewColumn();
            GridViewColumn gridViewColumn = null;
            for (int index = 1; index < row.Cells.Count; index++)
            {
                column = this.dtvDocInfos.Columns[index];
                KeyData key = new KeyData();
                gridViewColumn = column.Tag as GridViewColumn;
                key = new KeyData(gridViewColumn.ColumnTag, row.Cells[index].Value, gridViewColumn.ColumnType, gridViewColumn.ColumnUnit);
                lstKeyData.Add(key);
            }
            return lstKeyData;
        }

        /// <summary>
        /// 删除当前选中的文档
        /// </summary>
        /// <returns>是否删除成功</returns>
        private bool DeleteSelectedDocument()
        {
            if (!RightController.Instance.UserRight.DeleteNuringDoc.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限删除护理文书!");
                return false;
            }
            if (this.dtvDocInfos.SelectedRows.Count <= 0)
                return false;
            if (this.dtvDocInfos.SelectedRows[0].Index == 0 && bshowScoreRow)
            {
                return false;
            }
            NurDocInfo docInfo = this.dtvDocInfos.SelectedRows[0].Tag as NurDocInfo;
            if (!RightController.Instance.CanDeleteNurDoc(docInfo))
                return false;

            DialogResult result = MessageBoxEx.ShowConfirmFormat("确认删除“{0}”病历吗？"
                , null, docInfo.DocTitle);
            if (result != DialogResult.OK)
                return false;
            if (!this.dtvDocInfos.IsNewRow(this.dtvDocInfos.SelectedRows[0]))
            {
                DocStatusInfo docStatusInfo = new DocStatusInfo();
                docStatusInfo.DocID = docInfo.DocID;
                docStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
                docStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
                docStatusInfo.OperateTime = SysTimeService.Instance.Now;
                docStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
                short shRet = DocumentService.Instance.SetDocStatusInfo(ref docStatusInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show(string.Format("病历删除失败,{0}!", docStatusInfo.StatusMessage));
                    return false;
                }
            }
            this.dtvDocInfos.Rows.Remove(this.dtvDocInfos.SelectedRows[0]);
            return true;
        }

        /// <summary>
        /// 显示文档用于说明显示
        /// </summary>
        /// <param name="docTypeInfo">文档状态类</param>
        /// <returns>bool</returns>
        private bool ViewDocument(DocTypeInfo docTypeInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (docTypeInfo == null)
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            DocumentInfo document = DocumentInfo.Create(docTypeInfo, patVisit, null);

            this.HandleUILayout();
            bool success = this.documentControl1.OpenDocument(document);
            if (success)
            {
                if (this.documentControl1.Controls.Count > 0 && this.documentControl1.Controls[0].Controls.Count <= 0)
                {
                    this.splitContainer1.Panel2Collapsed = true;
                }
                else
                {
                    this.splitContainer1.Panel2Collapsed = false;
                }
            }
            else
            {
                MessageBoxEx.Show("模板说明打开失败");
                this.splitContainer1.Panel2Collapsed = true;
            }

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        #region "打印文档记录"
        /// <summary>
        /// 加载护理记录单打印模板
        /// </summary>
        /// <param name="szReportName">护理记录单打印模板名</param>
        /// <returns>护理记录单打印模板byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            if (this.m_docTypeInfo == null
                || GlobalMethods.Misc.IsEmptyString(this.m_docTypeInfo.DocTypeName))
            {
                MessageBoxEx.ShowError("无法获取文档类型信息!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_DOC_LIST;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.m_docTypeInfo.DocTypeName;
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowError("当前文档列表的打印报表还没有制作!");
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("当前文档列表的打印报表内容下载失败!");
                return null;
            }
            return byteTempletData;
        }

        /// <summary>
        /// 加载单文档打印模板
        /// </summary>
        /// <param name="szReportName">打印模板名</param>
        /// <returns>打印模板byte[]</returns>
        private byte[] GetFormReportFileData(string szReportName)
        {
            if (this.m_docTypeInfo == null
                || GlobalMethods.Misc.IsEmptyString(this.m_docTypeInfo.DocTypeName))
            {
                MessageBoxEx.ShowError("无法获取文档类型信息!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_DOC_FORM;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.m_docTypeInfo.DocTypeName;
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowError("当前文档列表的打印报表还没有制作!");
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("当前文档列表的打印报表内容下载失败!");
                return null;
            }
            return byteTempletData;
        }

        private bool GetSystemContext(string name, ref object value)
        {
            if (name == "查询起始时间")
            {
                value = this.tooldtpDateFrom.Value;
                return true;
            }
            if (name == "查询截止时间")
            {
                value = this.tooldtpDateTo.Value;
                return true;
            }
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        private ReportExplorerForm GetReportExplorerForm()
        {
            ReportExplorerForm reportExplorerForm = new ReportExplorerForm();
            reportExplorerForm.WindowState = FormWindowState.Maximized;
            reportExplorerForm.QueryContext +=
                new Heren.Common.Report.QueryContextEventHandler(this.ReportExplorerForm_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new Heren.Common.Report.ExecuteQueryEventHandler(this.ReportExplorerForm_ExecuteQuery);
            reportExplorerForm.NotifyNextReport +=
                new NotifyNextReportEventHandler(this.ReportExplorerForm_NotifyNextReport);
            return reportExplorerForm;
        }

        private void toolmnuPrintAll_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dtvDocInfos.Rows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dtvDocInfos, false, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintFrom_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dtvDocInfos.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            SetPageNumberForm setPageNumberForm = new SetPageNumberForm();
            if (setPageNumberForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            int nStartPageNumber = setPageNumberForm.StartPageNumber;

            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                int start = this.dtvDocInfos.SelectedRows[0].Index;
                int end = this.dtvDocInfos.Rows.Count - 1;
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dtvDocInfos, start, end, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.StartPageNo = nStartPageNumber;
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("设置打印起始页码", nStartPageNumber);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintContinued_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dtvDocInfos.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                int start = this.dtvDocInfos.SelectedRows[0].Index;
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dtvDocInfos, false, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("续打行号", start);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintSingle_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dtvDocInfos.SelectedRows.Count <= 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }

            SetPageNumberForm setPageNumberForm = new SetPageNumberForm("从选中行开始打印", "打印条数");
            if (setPageNumberForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            int nStartPageNumber = setPageNumberForm.StartPageNumber;

            byte[] byteReportData = this.GetFormReportFileData(null);
            if (byteReportData != null)
            {
                bSingle = true;
                int start = this.dtvDocInfos.SelectedRows[0].Index;
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dtvDocInfos, start, start + nStartPageNumber - 1, 2);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboPatDeptList_DropDown(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.toolcboPatDeptList.Items.Count <= 1)
                this.LoadPatientDeptList();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboPatDeptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocumentList(null);

            //绑定数值行
            this.FreezeScoreRow();

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            e.Value = value;
        }

        private void ReportExplorerForm_ExecuteQuery(object sender, Heren.Common.Report.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void ReportExplorerForm_NotifyNextReport(object sender, Heren.Common.Report.NotifyNextReportEventArgs e)
        {
            if (!bSingle)
                e.ReportData = this.GetReportFileData(e.ReportName);
            else
            {
                e.ReportData = this.GetFormReportFileData(e.ReportName);
            }
            bSingle = false;
        }
        #endregion
    }
}
