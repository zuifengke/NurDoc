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
using Heren.Common.VectorEditor.Shapes;
using Heren.Common.VectorEditor;

namespace Heren.NurDoc.PatPage.Document
{
    internal partial class GraphDocumentListForm : DockContentBase
    {
        private string m_SpecialShowOnlyDocs = "990001,990002,990003";
        private string m_SpecialMainDoc = "990000";
        private string m_SpecialMonitorID = "990002";
       
        private DataTable dtRecordsData = new DataTable();

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

        public bool bSizeChanged = false;

        /// <summary>
        /// 标识值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        /// <summary>
        /// 选中行的DocId
        /// </summary>
        private string m_szSelectedDocId = string.Empty;

        /// <summary>
        /// 文档编辑窗口
        /// </summary>
        private GraphDocumentEditForm m_DocumentEditForm = null;

        /// <summary>
        /// 获取当前显示文档编辑窗口
        /// </summary>
        [Browsable(false)]
        private GraphDocumentEditForm DocumentEditForm
        {
            get
            {
                if (this.m_DocumentEditForm == null)
                    return null;
                if (this.m_DocumentEditForm.IsDisposed)
                    return null;
                return this.m_DocumentEditForm;
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
                if (this.DocumentEditForm == null)
                    return false;
                return this.DocumentEditForm.IsModified;
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

        #region 内部类用于监护记录单数据显示传递
        public class PageData
        {
            public List<string> lstMedical = new List<string>();
            public DataTable dtData = null;

            public object Clone()
            {
                PageData pagedata = new PageData();
                pagedata.dtData = this.dtData.Copy();
                foreach (string szMedical in this.lstMedical)
                    pagedata.lstMedical.Add(szMedical);
                return pagedata;
            }
        }

        public class PageList : List<PageData>
        {
            public object Clone()
            {
                PageList pagelist = new PageList();
                foreach (PageData pd in this)
                {
                    object pd1 = pd.Clone();
                    pagelist.Add(pd1 as PageData);
                }
                return pagelist;
            }
        }
        #endregion

        /// <summary>
        /// 页码对应的页数据
        /// </summary>
        private PageList lstPageData = new PageList();

        public GraphDocumentListForm(PatientPageControl patientPageControl)
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

            if (this.toolcboPatDeptList.IsDisposed != true)
                this.toolcboPatDeptList.Items.Add(string.Empty);

            this.toolcboPatDeptList.DropDown +=
                new EventHandler(this.toolcboPatDeptList_DropDown);
            this.toolcboPatDeptList.SelectedIndexChanged +=
                new System.EventHandler(this.toolcboPatDeptList_SelectedIndexChanged);
            this.reportDesigner1.MouseWheel +=
                new MouseEventHandler(this.reportDesigner1_MouseWheel);
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

            //如果不允许重复创建,那么先显示编辑窗口
            if (!this.m_docTypeInfo.IsRepeated)
            {
                this.CreateDocumentEditForm();
            }
            else
            {
                if (this.DocumentEditForm != null)
                    this.DocumentEditForm.Hide();
            }

            if (!this.LoadRecordsTemplet())
            {
                GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
                return;
            }

            //加载显示的列方案
            this.LoadGridViewColumns();

            //加载已写的病历列表并修改数据表传递到打印格式中
            if (!this.LoadDocumentList(0, string.Empty))
            {
                GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
                return;
            }
            GlobalMethods.UI.SetCursor(this.PatientPage, Cursors.Default);
        }

        /// <summary>
        /// 加载文档摘要数据列集合
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewColumns()
        {
            //清除各摘要列
            this.dtRecordsData.Columns.Clear();
            if (this.m_docTypeInfo == null)
                return false;
            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            List<GridViewColumn> lstGridViewColumns = null;
            short shRet = ConfigService.Instance.GetGridViewColumns(szDocTypeID, ref lstGridViewColumns);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            if (lstGridViewColumns == null)
                lstGridViewColumns = new List<GridViewColumn>();

            this.InitializeColumns();

            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                if (gridViewColumn == null)
                    continue;
                if (!this.dtRecordsData.Columns.Contains(gridViewColumn.ColumnTag))
                    this.dtRecordsData.Columns.Add(new DataColumn(gridViewColumn.ColumnTag, typeof(string)));
            }
            return true;
        }

        /// <summary>
        /// 加载文档摘要数据列集合
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool InitializeColumns()
        {
            this.dtRecordsData.Columns.Clear();

            string[] szDefalutColumns = { "文档编号", "文档集编号", "创建人", "创建时间", "修改人", "修改时间", "记录时间" };

            DataColumn dtColumn = null;
            foreach (string colName in szDefalutColumns)
            {
                dtColumn = new DataColumn(colName, typeof(string));
                if (!this.dtRecordsData.Columns.Contains(colName))
                {
                    dtRecordsData.Columns.Add(dtColumn);
                }
            }
            return true;
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
            this.toolbtnPrint.Visible = bIsPrintable;
            if (bIsPrintable)
            {
                if (this.m_docTypeInfo.DocTypeID == m_SpecialMonitorID)
                {
                    this.toolmnuPrintAllPage.Visible = true;
                    this.toolmnuPrintPage.Visible = true;
                    this.toolmnuPrintAll.Visible = false;
                    //this.toolmnuPrintContinued.Visible = false;
                    //this.toolmnuPrintFrom.Visible = false;
                }
                else
                {
                    this.toolmnuPrintAllPage.Visible = false;
                    this.toolmnuPrintPage.Visible = false;
                    this.toolmnuPrintAll.Visible = true;
                    //this.toolmnuPrintContinued.Visible = true;
                    //this.toolmnuPrintFrom.Visible = true;
                }
            }
            if (this.m_SpecialShowOnlyDocs.IndexOf(this.m_docTypeInfo.DocTypeID) >= 0)
                this.toolbtnNew.Visible = false;
            else
                this.toolbtnNew.Visible = true;
        }

        /// <summary>
        /// 检查当前文档编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public override bool CheckModifyState()
        {
            if (this.DocumentEditForm == null)
                return false;
            return this.DocumentEditForm.CheckModifyState();
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
        /// <param name="defaultIndex">默认页码</param>
        /// <param name="szDefalutDocId">默认选中行</param>
        /// <returns>是否加载成功</returns>
        private bool LoadDocumentList(int defaultIndex, string szDefalutDocId)
        {
            this.Update();
            if (this.m_docTypeInfo == null || PatientTable.Instance.ActivePatient == null)
                return false;
            string szPatientID = PatientTable.Instance.ActivePatient.PatientId;
            string szVisitID = PatientTable.Instance.ActivePatient.VisitId;
            string szDocTypeID = this.m_docTypeInfo.DocTypeID;
            if (this.m_SpecialShowOnlyDocs.IndexOf(szDocTypeID) >= 0)
                szDocTypeID = this.m_SpecialMainDoc;
            DateTime dtBeginTime = this.tooldtpDateFrom.Value.Date;
            DateTime dtEndTime = GlobalMethods.SysTime.GetDayLastTime(this.tooldtpDateTo.Value.Date);
            NurDocList lstDocInfos = null;
            short shRet = DocumentService.Instance.GetDocInfosOrderByRecordTime(szPatientID, szVisitID, szDocTypeID
                , dtBeginTime, dtEndTime, ref lstDocInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("已写病历列表下载失败!");
                return false;
            }

            if (lstDocInfos == null || lstDocInfos.Count <= 0)
                lstDocInfos = new NurDocList();

            DeptInfo currentDept = this.toolcboPatDeptList.SelectedItem as DeptInfo;
            this.dtRecordsData.Rows.Clear();
            foreach (NurDocInfo docInfo in lstDocInfos)
            {
                if (currentDept != null && docInfo.WardCode != currentDept.DeptCode)
                    continue;
                DataRow row = this.dtRecordsData.NewRow();
                if (this.dtRecordsData != null)
                {
                    row["文档编号"] = docInfo.DocID;
                    row["文档集编号"] = docInfo.DocSetID;
                    row["创建人"] = docInfo.CreatorName;
                    row["创建时间"] = docInfo.DocTime.ToString("yyyy-MM-dd HH:mm");
                    row["修改人"] = docInfo.ModifierName;
                    row["修改时间"] = docInfo.ModifyTime.ToString("yyyy-MM-dd HH:mm");
                    row["记录时间"] = docInfo.RecordTime.ToString("yyyy-MM-dd HH:mm");
                }

                this.LoadSummaryData(ref row);
                this.dtRecordsData.Rows.Add(row);
            }

            this.bSizeChanged = true;
            if (this.dtRecordsData.Rows.Count <= 0)
            {
                this.panel1.Width = this.reportDesigner1.Width;
                this.reportDesigner2.Width = this.reportDesigner1.Width;
            }
            else
            {
                if (this.DocTypeInfo.DocTypeID == m_SpecialMonitorID)
                {
                    this.SeparateRecordData();
                    this.toolcboPageList.Items.Clear();
                    this.toolcboPageList.Visible = true;
                    this.toolbPageIndex.Visible = true;
                    for (int iPageIndex = 0; iPageIndex < lstPageData.Count; iPageIndex++)
                    {
                        this.toolcboPageList.Items.Add(iPageIndex + 1);
                    }
                    this.toolcboPageList.SelectedIndex = (defaultIndex <= 0 || defaultIndex > lstPageData.Count) ? lstPageData.Count - 1 : defaultIndex - 1;

                    //刷新界面使竖向滚动条能出现
                    this.reportDesigner1.PerformLayout(false);
                    //默认选中行
                    int scrollHeight = this.reportDesigner1.Height;
                    if (this.ReShowSelectedRow(szDefalutDocId, ref scrollHeight))
                        this.reportDesigner1.AutoScrollPosition = new Point(0, scrollHeight - this.reportDesigner1.AutoScrollPosition.Y);
                }
                else
                {
                    this.toolcboPageList.Visible = false;
                    this.toolbPageIndex.Visible = false;
                    this.reportDesigner1.UpdateReport("预览数据", this.dtRecordsData);
                    //刷新界面使竖向滚动条能出现  并将列表下拉到最下方
                    this.reportDesigner1.PerformLayout(false);
                    //默认选中行
                    int scrollHeight = this.reportDesigner1.Height;
                    if (this.ReShowSelectedRow(szDefalutDocId, ref scrollHeight))
                        this.reportDesigner1.AutoScrollPosition = new Point(0, scrollHeight - this.reportDesigner1.AutoScrollPosition.Y);
                }
                //使两个报表控件初始化相同的横向滚动值
                this.reportDesigner2.HorizontalScroll.Value = this.reportDesigner1.HorizontalScroll.Value;
            }
            this.bSizeChanged = false;
            return true;
        }

        #region 控速药物分页
        /// <summary>
        /// 处理控速药物分页事件
        /// </summary>
        private void SeparateRecordData()
        {
            lstPageData = new PageList();
            PageData pageData = new PageData();
            if (!this.dtRecordsData.Columns.Contains("控速药物") || !this.dtRecordsData.Columns.Contains("控速药物速率"))
            {
                pageData.lstMedical = new List<string>();
                pageData.dtData = this.dtRecordsData;
                lstPageData.Add(pageData);
                return;
            }
            if (!this.dtRecordsData.Columns.Contains("控速药物速率排列"))
                this.dtRecordsData.Columns.Add(new DataColumn("控速药物速率排列", typeof(string)));
            int lastindex = 0;
            bool IsNextPage = false;
            string value = string.Empty;
            string CleanValue = string.Empty;
            string[] szMedicalName;
            string[] szMedicalRate;
            int AddTimes = 0;
            for (int recIndex = 0; recIndex < dtRecordsData.Rows.Count; recIndex++)
            {
                if (IsNextPage)
                    IsNextPage = false;
                szMedicalName = dtRecordsData.Rows[recIndex]["控速药物"].ToString().Replace("\r", string.Empty).Split('\n');
                szMedicalRate = dtRecordsData.Rows[recIndex]["控速药物速率"].ToString().Replace("\r", string.Empty).Split('\n');
                AddTimes = 0;
                for (int i = 0; i < szMedicalName.Length; i++)
                {
                    CleanValue = szMedicalName[i].Trim();
                    if (CleanValue != string.Empty && !pageData.lstMedical.Contains(CleanValue))
                    {
                        if (pageData.lstMedical.Count < 6)
                        {
                            AddTimes++;
                            pageData.lstMedical.Add(CleanValue);
                        }
                        else
                        {
                            pageData.lstMedical.RemoveRange(6 - AddTimes, AddTimes);
                            pageData.dtData = this.GetRangeData(this.dtRecordsData, lastindex, --recIndex);
                            lstPageData.Add(pageData);
                            pageData = new PageData();
                            lastindex = recIndex + 1;
                            IsNextPage = true;
                            break;
                        }
                    }
                }
                if (!IsNextPage)
                {
                    value = string.Empty;
                    for (int i = 0; i < pageData.lstMedical.Count; i++)
                    {
                        for (int j = 0; j < szMedicalName.Length; j++)
                        {
                            CleanValue = szMedicalName[j].Trim();
                            if (pageData.lstMedical[i] == CleanValue)
                            {
                                value += j < szMedicalRate.Length ? szMedicalRate[j] : "0";
                            }
                        }
                        value += "；";
                    }
                    dtRecordsData.Rows[recIndex]["控速药物速率排列"] = value;
                }
                if (recIndex == this.dtRecordsData.Rows.Count - 1)
                {
                    pageData.dtData = this.GetRangeData(this.dtRecordsData, lastindex, recIndex);
                    lstPageData.Add(pageData);
                }
            }
        }
        #endregion

        /// <summary>
        /// 以开始结尾index截取datatable的数据
        /// </summary>
        /// <param name="dtData">原数据</param>
        /// <param name="FirstIndex">开始index</param>
        /// <param name="LastIndex">结尾index</param>
        /// <returns>截取后的datatable</returns>
        private DataTable GetRangeData(DataTable dtData, int FirstIndex, int LastIndex)
        {
            DataTable dtReturn = dtData.Clone();
            for (int index = FirstIndex; index <= LastIndex; index++)
            {
                dtReturn.Rows.Add(dtData.Rows[index].ItemArray);
            }
            return dtReturn;
        }

        /// <summary>
        /// 加载指定行的摘要列的列数据
        /// </summary>
        /// <param name="row">指定行</param>
        /// <returns>是否加载成功</returns>
        private bool LoadSummaryData(ref DataRow row)
        {
            if (row == null)
                return false;

            string szDocSetID = row["文档集编号"].ToString();
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
                if (this.dtRecordsData != null && this.dtRecordsData.Columns.Contains(summaryData.DataName))
                {
                    row[summaryData.DataName] = summaryData.DataValue;
                }
            }
            return true;
        }

        /// <summary>
        /// 删除当前选中的文档
        /// </summary>
        /// <returns>是否删除成功</returns>
        private bool DeleteDocument(string szDocID)
        {
            if (!RightController.Instance.UserRight.DeleteNuringDoc.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限删除护理文书!");
                return false;
            }

            NurDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(this.m_szSelectedDocId))
                shRet = DocumentService.Instance.GetDocInfo(szDocID, ref docInfo);
            else
                shRet = DocumentService.Instance.GetLatestDocInfo(szDocID, ref docInfo);
            if (!RightController.Instance.CanDeleteNurDoc(docInfo))
                return false;

            DialogResult result = MessageBoxEx.ShowConfirmFormat("确认删除“{0}”病历吗？"
                , null, docInfo.DocTitle);
            if (result != DialogResult.OK)
                return false;
            DocStatusInfo docStatusInfo = new DocStatusInfo();
            docStatusInfo.DocID = docInfo.DocID;
            docStatusInfo.OperatorID = SystemContext.Instance.LoginUser.ID;
            docStatusInfo.OperatorName = SystemContext.Instance.LoginUser.Name;
            docStatusInfo.OperateTime = SysTimeService.Instance.Now;
            docStatusInfo.DocStatus = ServerData.DocStatus.CANCELED;
            shRet = DocumentService.Instance.SetDocStatusInfo(ref docStatusInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("病历删除失败,{0}!", docStatusInfo.StatusMessage));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置选中行背景色
        /// </summary>
        /// <param name="rcElement">表格元素</param>
        /// <param name="RowIndex">行索引</param>
        private void SetSelectedRowColor(RowColumnElement rcElement, int RowIndex)
        {
            for (int Iindex = 0; Iindex < rcElement.Rows.Count; Iindex++)
            {
                for (int Jindex = 0; Jindex < rcElement.Columns.Count; Jindex++)
                {
                    rcElement.Rows[Iindex].Cells[Jindex].Style.BackColor = Iindex == RowIndex ? Color.LightBlue : Color.White;
                }
            }
        }

        /// <summary>
        /// 设置选中行背景色
        /// </summary>
        /// <param name="szDocId">默认行值</param>
        /// <param name="iHeight">选中行对应的Y值</param>
        /// <returns>exists or not</returns>
        private bool ReShowSelectedRow(string szDocId, ref int iHeight)
        {
            if (szDocId.Trim() == string.Empty)
                return false;
            foreach (ElementBase element in this.reportDesigner1.Elements)
            {
                RowColumnElement rowcolElement = element as RowColumnElement;
                if (rowcolElement != null)
                {
                    int colIndex = this.GetColumnIndexByName(rowcolElement, "文档编号");
                    for (int index = 0; index < rowcolElement.Rows.Count; index++)
                    {
                        if (szDocId == rowcolElement.Rows[index].Cells[colIndex].ToString())
                        {
                            this.SetSelectedRowColor(rowcolElement, index);
                            iHeight = (int)rowcolElement.Rows[index].Bounds.Bottom + 1;
                            return true; ;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 创建文档编辑器窗口
        /// </summary>
        private void CreateDocumentEditForm()
        {
            if (this.DocumentEditForm == null)
            {
                this.m_DocumentEditForm = new GraphDocumentEditForm();
                this.m_DocumentEditForm.ShowInTaskbar = false;
                this.m_DocumentEditForm.MinimizeBox = false;
                this.m_DocumentEditForm.StartPosition = FormStartPosition.CenterParent;
                this.m_DocumentEditForm.DocumentUpdated +=
                    new EventHandler(this.DocumentEditForm_DocumentUpdated);
                this.m_DocumentEditForm.DocumentSave +=
                    new EventHandler(this.DocumentEditForm_DocumentSave);
            }

            if (this.IsShowAsModel())
            {
                this.DocumentEditForm.Visible = false;
                this.DocumentEditForm.Parent = null;
                this.DocumentEditForm.TopLevel = true;
                this.DocumentEditForm.Dock = DockStyle.None;
                this.DocumentEditForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                this.DocumentEditForm.TopLevel = false;
                this.DocumentEditForm.FormBorderStyle = FormBorderStyle.None;
                this.DocumentEditForm.Dock = DockStyle.Fill;
                this.DocumentEditForm.Parent = this;
                this.DocumentEditForm.Visible = true;
                this.DocumentEditForm.BringToFront();
            }
            if (this.m_docTypeInfo != null)
                this.DocumentEditForm.ShowReturnButton = this.m_docTypeInfo.IsRepeated;
        }

        /// <summary>
        /// 显示指定护理文档类型对应的护理电子病历编辑器
        /// </summary>
        /// <param name="docTypeInfo">护理文档类型</param>
        private void ShowDocumentEditForm(DocTypeInfo docTypeInfo)
        {
            this.CreateDocumentEditForm();

            Application.DoEvents();
            if (!this.IsShowAsModel())
                this.DocumentEditForm.CreateDocument(docTypeInfo);
            else
                this.DocumentEditForm.ShowDialog(docTypeInfo, this);
        }

        /// <summary>
        /// 显示指定护理文档对应的护理电子病历编辑器
        /// </summary>
        /// <param name="docInfo">关联的护理文档信息</param>
        private void ShowDocumentEditForm(NurDocInfo docInfo)
        {
            this.CreateDocumentEditForm();

            if (!this.IsShowAsModel())
                this.DocumentEditForm.OpenDocument(docInfo);
            else
                this.DocumentEditForm.ShowDialog(docInfo, this);
        }

        /// <summary>
        /// 显示指定文档类型和文档记录对应的文档编辑器窗口
        /// </summary>
        /// <param name="szDocID">文档记录ID</param>
        public void ShowDocumentEditForm(string szDocID)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (GlobalMethods.Misc.IsEmptyString(szDocID))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            NurDocInfo docInfo = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (!DocumentInfo.IsDocSetID(szDocID))
                shRet = DocumentService.Instance.GetDocInfo(szDocID, ref docInfo);
            else
                shRet = DocumentService.Instance.GetLatestDocInfo(szDocID, ref docInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的表单!", szDocID));
                return;
            }
            this.ShowDocumentEditForm(docInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="docTypeInfo">病历类型信息</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public override bool LocateToModule(DocTypeInfo docTypeInfo, string szDocID)
        {
            if (docTypeInfo == null || this.m_docTypeInfo == null)
                return false;
            if (docTypeInfo.ApplyEnv != ServerData.DocTypeApplyEnv.NURSING_DOCUMENT)
                return false;
            if (docTypeInfo.DocTypeID != this.m_docTypeInfo.DocTypeID)
                return false;

            this.Activate();
            Application.DoEvents();

            this.ShowDocumentEditForm(szDocID);
            return true;
        }

        /// <summary>
        /// 加载监护记录报表模板
        /// </summary>
        /// <returns>success or not</returns>
        private bool LoadRecordsTemplet()
        {
            if (PatientTable.Instance.ActivePatient == null)
                return false;
            if (SystemContext.Instance.LoginUser == null)
                return false;
            this.Update();
            string szApplyEnv = ServerData.ReportTypeApplyEnv.MONITOR_RECORD;

            ReportTypeInfo reportTypeInfo = ReportCache.Instance.GetWardReportType(szApplyEnv, this.DocTypeInfo.DocTypeName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowErrorFormat("{0}模板还没有制作!", this.DocTypeInfo.DocTypeName);
                return false;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowErrorFormat("{0}模板内容下载失败!", this.DocTypeInfo.DocTypeName);
                return false;
            }

            this.bSizeChanged = true;
            this.reportDesigner1.OpenDocument(byteTempletData);
            this.reportDesigner2.OpenDocument(byteTempletData);
            this.reportDesigner1.PerformLayout(false);
            int iTableHeaderHeight = this.GetTableHeaderHeight();//获取表头的高度 需要从打印单中获取
            //由于reportDesigner控件继承自ScrollableControl，滚动条可见性存在问题，需要将表头的长宽置于panel中进行手动设置
            //同时需要判断滚动条的宽度，将panel的大小减去滚动条高宽实现隐藏滚动条，1：1像素为表头底部的实现显示
            this.reportDesigner2.Height = iTableHeaderHeight + SystemInformation.HorizontalScrollBarHeight + 1;
            this.reportDesigner2.Width = this.reportDesigner1.Width - SystemInformation.VerticalScrollBarWidth;
            this.panel1.Height = iTableHeaderHeight + 1;
            this.panel1.Width = this.reportDesigner1.Width - SystemInformation.VerticalScrollBarWidth;
            this.bSizeChanged = false;
            return true;
        }

        private void DocumentEditForm_DocumentUpdated(object sender, EventArgs e)
        {
            if (this.DocumentEditForm == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szDocID = null;
            if (this.DocumentEditForm.Document != null)
                szDocID = this.DocumentEditForm.Document.DocID;
            this.LoadDocumentList(GlobalMethods.Convert.StringToValue(this.toolcboPageList.Text, 0), szDocID);

            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void DocumentEditForm_DocumentSave(object sender, EventArgs e)
        {
            if (this.DocumentEditForm == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szDocID = null;
            if (this.DocumentEditForm.Document != null)
                szDocID = this.DocumentEditForm.Document.DocID;
            this.LoadDocumentList(GlobalMethods.Convert.StringToValue(this.toolcboPageList.Text, 0), szDocID);
            if (this.toolmnuSavePreView.Checked)
            {
                if (this.m_docTypeInfo.DocTypeID == m_SpecialMonitorID)
                {
                    this.toolmnuPrintAllPage.PerformClick();
                }
                else
                {
                    this.toolmnuPrintAll.PerformClick();
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpQueryDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadDocumentList(0, string.Empty);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.CanCreateNurDoc())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.ShowDocumentEditForm(this.m_docTypeInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            this.toolbtnNew.PerformClick();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (GlobalMethods.Misc.IsEmptyString(this.m_szSelectedDocId))
            {
                MessageBoxEx.ShowError("该行数据不可删除!");
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.DeleteDocument(this.m_szSelectedDocId);
            this.LoadDocumentList(GlobalMethods.Convert.StringToValue(this.toolcboPageList.Text, 0), string.Empty);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 获取指定位置下的所有图形元素
        /// </summary>
        /// <param name="x">鼠标x坐标</param>
        /// <param name="y">鼠标y坐标</param>
        /// <returns>图形元素列表</returns>
        public List<ElementBase> GetElementAt(int x, int y)
        {
            if (this.reportDesigner1 == null)
                return null;
            Point point = this.reportDesigner1.GetOffsetPoint(x, y);
            List<ElementBase> elements = new List<ElementBase>();
            foreach (ElementBase element in this.reportDesigner1.Elements)
            {
                if (element.HitTest(point.X, point.Y, false).Element != null)
                    elements.Add(element);
            }
            return elements;
        }

        /// <summary>
        /// 获取表头高度  “根据表格元素的top值获取高度值”
        /// </summary>
        /// <returns>高度</returns>
        private int GetTableHeaderHeight()
        {
            foreach (ElementBase element in this.reportDesigner2.Elements)
            {
                RowColumnElement rowcolElement = element as RowColumnElement;
                if (rowcolElement != null)
                {
                    return rowcolElement.Top;
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据列名称获取行=列索引
        /// </summary>
        /// <param name="element">表格元素</param>
        /// <param name="columnName">列名称</param>
        /// <returns>列索引</returns>
        public int GetColumnIndexByName(RowColumnElement element, string columnName)
        {
            for (int index = 0; index < element.Columns.Count; index++)
            {
                if (element.Columns[index].Name == columnName)
                {
                    return index;
                }
            }
            return -1;
        }

        #region 选项配置  本地相关
        /// <summary>
        /// 文档编辑窗口是否弹出框
        /// </summary>
        /// <returns>是否弹出</returns>
        private bool IsShowAsModel()
        {
            string key = SystemConst.ConfigKey.NUR_DOC_SHOW_AS_MODEL;
            return this.m_docTypeInfo != null && this.m_docTypeInfo.IsRepeated
                 && SystemContext.Instance.GetConfig(key, false);
        }

        private void toolbtnOption_DropDownOpening(object sender, EventArgs e)
        {
            string key = SystemConst.ConfigKey.NUR_DOC_SHOW_AS_MODEL;
            if (SystemContext.Instance.GetConfig(key, false))
                this.toolmnuShowAsModel.Checked = true;
            else
                this.toolmnuShowAsModel.Checked = false;

            key = SystemConst.ConfigKey.NUR_DOC_SAVE_PREVIEW;
            if (SystemContext.Instance.GetConfig(key, false))
                this.toolmnuSavePreView.Checked = true;
            else
                this.toolmnuSavePreView.Checked = false;
        }

        private void toolmnuShowAsModel_Click(object sender, EventArgs e)
        {
            this.toolmnuShowAsModel.Checked = !this.toolmnuShowAsModel.Checked;
            string key = SystemConst.ConfigKey.NUR_DOC_SHOW_AS_MODEL;
            string value = this.toolmnuShowAsModel.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }

        private void toolmnuSavePreView_Click(object sender, EventArgs e)
        {
            this.toolmnuSavePreView.Checked = !this.toolmnuSavePreView.Checked;
            string key = SystemConst.ConfigKey.NUR_DOC_SAVE_PREVIEW;
            string value = this.toolmnuSavePreView.Checked.ToString();
            SystemContext.Instance.WriteConfig(key, value);
        }
        #endregion

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
                new QueryContextEventHandler(this.ReportExplorerForm_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new ExecuteQueryEventHandler(this.ReportExplorerForm_ExecuteQuery);
            reportExplorerForm.NotifyNextReport +=
                new NotifyNextReportEventHandler(this.ReportExplorerForm_NotifyNextReport);
            return reportExplorerForm;
        }
        #endregion

        private void toolmnuPrintAll_Click(object sender, EventArgs e)
        {
            this.Update();
            if (this.dtRecordsData.Rows.Count <= 0)
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
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", this.dtRecordsData);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintFrom_Click(object sender, EventArgs e)
        {
            this.Update();
            //if (this.dataTableView1.SelectedRows.Count <= 0)
            //    return;
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
                DataTable table = new DataTable();
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
            //if (this.dataTableView1.SelectedRows.Count <= 0)
            //    return;
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
                //int start = this.dataTableView1.SelectedRows[0].Index;
                DataTable table = new DataTable();
                //GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("续打行号", 1);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        #region 监护记录单 控速药物分页 打印
        private void toolmnuPrintAllPage_Click(object sender, EventArgs e)
        {
            this.Update();
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
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("药物页码", -1);
                explorerForm.ReportParamData.Add("打印数据", this.lstPageData);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolmnuPrintPage_Click(object sender, EventArgs e)
        {
            this.Update();
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
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;

                explorerForm.ReportParamData.Add("起始页码", nStartPageNumber);
                explorerForm.ReportParamData.Add("药物页码", GlobalMethods.Convert.StringToValue(this.toolcboPageList.Text, -1));
                explorerForm.ReportParamData.Add("打印数据", this.lstPageData);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }
        #endregion

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
            this.LoadDocumentList(0, string.Empty);
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
            e.ReportData = this.GetReportFileData(e.ReportName);
        }

        private void reportDesigner1_NotifyNextReport(object sender, Heren.Common.Report.NotifyNextReportEventArgs e)
        {
            e.ReportData = this.GetReportFileData(e.ReportName);
        }

        private void reportDesigner1_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = SystemContext.Instance.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }

        private void reportDesigner1_ExecuteQuery(object sender, Heren.Common.Report.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void reportDesigner1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                this.reportDesigner2.AutoScrollPosition = new Point(e.NewValue, 0);
            }
        }

        private void reportDesigner1_MouseWheel(object sender, MouseEventArgs e)
        {
            this.reportDesigner2.AutoScrollPosition = new Point(this.reportDesigner1.HorizontalScroll.Value, 0);
        }

        private void reportDesigner1_ElementDoubleClick(object sender, Heren.Common.VectorEditor.CanvasEventArgs e)
        {
            RowColumnElement rowcolElement = e.Element as RowColumnElement;
            if (rowcolElement != null)
            {
                int height = Convert.ToInt32(rowcolElement.MouseDownPoint.Y) - rowcolElement.Top;
                int PreRowBottonY = 0;
                int colIndex = this.GetColumnIndexByName(rowcolElement, "文档编号");
                if (colIndex < 0)
                    return;
                for (int index = 0; index < rowcolElement.Rows.Count; index++)
                {
                    if (PreRowBottonY < height && height < Convert.ToInt32(rowcolElement.Rows[index].Bounds.Bottom))
                    {
                        m_szSelectedDocId = rowcolElement.Rows[index].Cells[colIndex].ToString();
                        this.ShowDocumentEditForm(m_szSelectedDocId);
                        break;
                    }
                    PreRowBottonY = Convert.ToInt32(rowcolElement.Rows[index].Bounds.Bottom);
                }
            }
        }

        private void reportDesigner1_MouseClick(object sender, MouseEventArgs e)
        {
            List<ElementBase> lstElement = this.GetElementAt(e.X, e.Y);
            foreach (ElementBase element in lstElement)
            {
                RowColumnElement rowcolElement = element as RowColumnElement;
                if (rowcolElement != null)
                {
                    int height = Convert.ToInt32(rowcolElement.MouseDownPoint.Y) - rowcolElement.Top;
                    int PreRowBottonY = 0;
                    int colIndex = this.GetColumnIndexByName(rowcolElement, "文档编号");
                    for (int index = 0; index < rowcolElement.Rows.Count; index++)
                    {
                        if (PreRowBottonY < height && height < Convert.ToInt32(rowcolElement.Rows[index].Bounds.Bottom))
                        {
                            this.SetSelectedRowColor(rowcolElement, index);
                            if (e.Button == MouseButtons.Right)
                            {
                                m_szSelectedDocId = rowcolElement.Rows[index].Cells[colIndex].ToString();
                                this.cmnenuDocumentList.Show(this.reportDesigner1, e.X, e.Y);
                            }
                        }
                        PreRowBottonY = Convert.ToInt32(rowcolElement.Rows[index].Bounds.Bottom);
                    }
                }
            }
        }

        private void toolcboPageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            int pageIndex = GlobalMethods.Convert.StringToValue(this.toolcboPageList.Text, -1);
            if (pageIndex > 0)
                this.reportDesigner2.UpdateReport("显示药物", this.lstPageData[pageIndex - 1].lstMedical);

            this.reportDesigner2.UpdateReport("显示引流管", this.lstPageData[pageIndex - 1].dtData);
            this.reportDesigner1.UpdateReport("预览页码", pageIndex);
            this.reportDesigner1.UpdateReport("预览数据", this.lstPageData);
            this.reportDesigner1.PerformLayout(false);
            this.reportDesigner1.AutoScrollPosition = new Point(0, this.reportDesigner1.Height - this.reportDesigner1.AutoScrollPosition.Y);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void reportDesigner1_SizeChanged(object sender, EventArgs e)
        {
            if (this.bSizeChanged)
                return;
            int iTableHeaderHeight = this.GetTableHeaderHeight();//获取表头的高度 需要从打印单中获取
            //由于reportDesigner控件继承自ScrollableControl，滚动条可见性存在问题，需要将表头的长宽置于panel中进行手动设置
            //同时需要判断滚动条的宽度，将panel的大小减去滚动条高宽实现隐藏滚动条，1：1像素为表头底部的实现显示
            this.reportDesigner2.Height = iTableHeaderHeight + SystemInformation.HorizontalScrollBarHeight + 1;
            this.reportDesigner2.Width = this.reportDesigner1.Width - SystemInformation.VerticalScrollBarWidth - 10;
            this.panel1.Height = iTableHeaderHeight + 1;
            this.panel1.Width = this.reportDesigner1.Width - SystemInformation.VerticalScrollBarWidth;
        }
    }
}
