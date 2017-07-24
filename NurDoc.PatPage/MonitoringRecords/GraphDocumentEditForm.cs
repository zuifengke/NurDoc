// ***********************************************************
// 护理电子病历系统,表单文档编辑窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.Forms.Editor;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.Common.Forms.Loader;

namespace Heren.NurDoc.PatPage.Document
{
    internal partial class GraphDocumentEditForm : HerenForm
    {
        private DocumentInfo m_document = null;

        /// <summary>
        /// 获取当前表单编辑器绑定病历文档信息
        /// </summary>
        [Browsable(false)]
        public DocumentInfo Document
        {
            get
            {
                if (this.documentControl1 == null)
                    return null;
                if (this.documentControl1.IsDisposed)
                    return null;
                return this.documentControl1.Document;
            }
        }

        /// <summary>
        /// 获取或设置表单编辑器是否显示返回按钮
        /// </summary>
        [DefaultValue(true)]
        [Description("获取或设置表单编辑器是否显示返回按钮")]
        public bool ShowReturnButton
        {
            get
            {
                if (this.toolbtnReturn == null)
                    return false;
                if (this.toolbtnReturn.IsDisposed)
                    return false;
                return this.toolbtnReturn.Visible;
            }

            set
            {
                if (this.toolbtnReturn == null)
                    return;
                if (this.toolbtnReturn.IsDisposed)
                    return;
                this.toolbtnReturn.Visible = value;
            }
        }

        private bool m_bIsFirstRecord = false;

        /// <summary>
        /// 获取或设置当前是否第一次新建护理记录
        /// </summary>
        [Browsable(false)]
        public bool IsFirstRecord
        {
            get { return this.m_bIsFirstRecord; }
            set { this.m_bIsFirstRecord = value; }
        }

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get
            {
                if (this.documentControl1 == null)
                    return false;
                if (this.documentControl1.IsDisposed)
                    return false;
                if (this.documentControl1.Document == null)
                    return false;
                return this.documentControl1.IsModified;
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
        /// 当文档被被保存是触发
        /// </summary>
        [Description("当文档被被保存是触发")]
        public event EventHandler DocumentSave;

        protected virtual void OnDocumentSave(EventArgs e)
        {
            if (this.DocumentSave != null)
                this.DocumentSave(this, e);
        }

        /// <summary>
        /// 获取当前文档编辑器表单是否更新过
        /// </summary>
        private bool m_documentUpdated = false;

        /// <summary>
        /// 该变量用于指示某些控件值改变事件是否可用
        /// </summary>
        private bool m_bControlValueChangedEventEnabled = true;

        public GraphDocumentEditForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.FormEdit;

            DateTime dtNow = SysTimeService.Instance.Now;
            dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day
                , dtNow.Hour, dtNow.Minute, 0);
            this.tooldtpRecordTime.Value = dtNow;

            if (this.documentControl1 == null)
                return;
            this.documentControl1.QueryContext +=
                new QueryContextEventHandler(this.documentControl1_QueryContext);
            this.documentControl1.CustomEvent +=
                new CustomEventHandler(this.documentControl1_CustomEvent);
        }

        public DialogResult ShowDialog(NurDocInfo docInfo, IWin32Window owner)
        {
            this.m_document = DocumentInfo.Create(docInfo);
            return base.ShowDialog(owner);
        }

        public DialogResult ShowDialog(DocTypeInfo docTypeInfo, IWin32Window owner)
        {
            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            this.m_document = DocumentInfo.Create(docTypeInfo, patVisit, null);
            return base.ShowDialog(owner);
        }

        new public void Hide()
        {
            if (!this.CheckModifyState())
            {
                base.Hide();
                this.documentControl1.IsModified = false;
                //由于不同表单切换时 执行了此update操作 导致报表数据传递出错
                //if (this.m_documentUpdated)
                //    this.OnDocumentUpdated(EventArgs.Empty);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
            if (!this.Modal || this.m_document == null)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_document.DocState == DocumentState.New)
            {
                DocTypeInfo docTypeInfo =
                    FormCache.Instance.GetDocTypeInfo(this.m_document.DocTypeID);
                this.CreateDocument(docTypeInfo);
            }
            else
            {
                this.OpenDocument(this.m_document.ToDocInfo());
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 关闭当前文档的显示
        /// </summary>
        public void CloseDocument()
        {
            this.documentControl1.CloseDocument();
        }

        /// <summary>
        /// 根据指定的病历类型创建新的病历
        /// </summary>
        /// <param name="docTypeInfo">病历类型信息</param>
        /// <returns>是否成功</returns>
        public bool CreateDocument(DocTypeInfo docTypeInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (docTypeInfo == null)
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.m_documentUpdated = false;

            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            DocumentInfo document = DocumentInfo.Create(docTypeInfo, patVisit, null);

            bool success = this.documentControl1.OpenDocument(document);
            this.HandleUILayout(docTypeInfo.DocTypeID);

            //设置下默认的记录时间为当前时间（由于第二次新增取上次记录时间）xpp
            DateTime dtNow = SysTimeService.Instance.Now;
            dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day
                , dtNow.Hour, dtNow.Minute, 0);
            if (this.tooldtpRecordTime.IsDisposed != true)
                this.tooldtpRecordTime.Value = dtNow;

            if (this.m_bIsFirstRecord)
            {
                object firstTime = this.documentControl1.GetFormData("第一次记录时间");
                if (firstTime is DateTime)
                    this.tooldtpRecordTime.Value = (DateTime)firstTime;
            }
            if (this.Document != null)
                this.Document.ModifyRecordTime(this.tooldtpRecordTime.Value);
            if (this.tooltxtCreatorName.IsDisposed != true)
                this.tooltxtCreatorName.Text = SystemContext.Instance.LoginUser.Name;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        /// <summary>
        /// 根据当前已显示的文档重新创建文档
        /// </summary>
        /// <returns>是否成功</returns>
        public bool RecreateDocument()
        {
            if (this.Document == null)
                return false;
            if (this.CheckModifyState())
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            string szDocTypeID = this.Document.DocTypeID;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowFormat("没有找到“{0}”表单类型!", null, szDocTypeID);
                return false;
            }
            bool bDocumentUpdated = this.m_documentUpdated;
            this.CreateDocument(docTypeInfo);
            this.m_documentUpdated = bDocumentUpdated;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 打开指定的已写的病历信息的病历文档
        /// </summary>
        /// <param name="docInfo">已写病历信息</param>
        /// <returns>是否成功</returns>
        public bool OpenDocument(NurDocInfo docInfo)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return false;
            if (docInfo == null)
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            this.m_documentUpdated = false;

            DocumentInfo document = DocumentInfo.Create(docInfo);
            bool success = this.documentControl1.OpenDocument(document);
            this.HandleUILayout(docInfo.DocTypeID);
            this.ClearData();
            if (this.documentControl1.Document != null && this.tooltxtCreatorName.IsDisposed != true)
            {
                this.tooldtpRecordTime.Value = this.documentControl1.Document.RecordTime;
                this.tooltxtCreatorName.Text = this.documentControl1.Document.CreatorName;
                //是否归档校验 根据后台配置判断
                if (SystemContext.Instance.SystemOption.DocPigeonhole == true)
                {
                    if (IsDocExamined(this.documentControl1.Document))
                    {
                        if (this.documentControl1.Controls.Count > 0)
                            this.documentControl1.Controls[0].Enabled = false;
                        this.toolbtnSave.Visible = false;
                        this.toolbtnButton1.Visible = false;
                        this.toolbtnButton2.Visible = false;
                    }
                }
            }
            //this.HandleUILayout(docInfo.DocTypeID);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        /// <summary>
        /// 保存当前病历数据
        /// </summary>
        /// <returns>是否成功</returns>
        public bool SaveDocument()
        {
            if (this.Document == null)
                return false;
            Application.DoEvents();
            if (this.Document.DocState == DocumentState.New)
            {
                if (!RightController.Instance.CanCreateNurDoc())
                    return false;
            }
            if (!this.documentControl1.EndEdit())
                return false;
            if (!this.documentControl1.CheckFormData(null))
                return false;
            if (!this.documentControl1.SaveDocument())
                return false;

            DocumentInfo document = this.documentControl1.Document;
            bool success = this.documentControl1.CommitDocument(document);
            if (success)
            {
                this.m_documentUpdated = true;
                this.documentControl1.IsModified = false;

                if (this.IsSavePreView(document.DocTypeID))
                {
                    base.Hide();
                    this.OnDocumentSave(EventArgs.Empty);
                }
                else
                {
                    this.OnDocumentUpdated(EventArgs.Empty);
                }

                object param = string.Empty;
                this.documentControl1.SaveFormData(ref param);
            }
            return success;
        }

        /// <summary>
        /// 保存后是否显示预览
        /// </summary>
        /// <returns>true or false</returns>
        private bool IsSavePreView(string szDocTypeID)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                return false;
            }
            string key = SystemConst.ConfigKey.NUR_DOC_SAVE_PREVIEW;
            return docTypeInfo.IsRepeated && SystemContext.Instance.GetConfig(key, false);
        }

        /// <summary>
        /// 根据病历属性信息处理UI界面上显示的按钮
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        private void HandleUILayout(string szDocTypeID)
        {
            Application.DoEvents();
            object visible = this.documentControl1.GetFormData("工具栏可见");
            if (visible != null)
                if (visible.ToString().ToLower() == "false")
                    this.toolStrip1.Visible = false;
                else
                    this.toolStrip1.Visible = true;
            else
                this.toolStrip1.Visible = true;

            bool bIsPrintable = FormCache.Instance.IsFormPrintable(szDocTypeID);
            if (this.toolbtnPrint.Visible != bIsPrintable)
                this.toolbtnPrint.Visible = bIsPrintable;
            if (this.toolbtnPreview.Visible != bIsPrintable)
                this.toolbtnPreview.Visible = bIsPrintable;

            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
                return;
            this.Text = docTypeInfo.DocTypeName;
            if (this.toolbtnNew.Visible != docTypeInfo.IsRepeated)
                this.toolbtnNew.Visible = docTypeInfo.IsRepeated;
            // this.toolbtnButton1.Visible = false;
            // this.toolbtnButton2.Visible = false;
        }

        /// <summary>
        /// 移动端数据重现  先清空数据 再从summary中读取(也可用于其他表单的数据清除)
        /// </summary>
        /// <returns>true or false</returns>
        private bool ClearData()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            object bclearData = this.documentControl1.GetFormData("是否清空数据");
            if ((bclearData is bool) ? (bool)bclearData : false)
            {
                this.documentControl1.ClearFormData();
                this.documentControl1.UpdateFormData("数据重新加载", string.Empty);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        /// 
        public bool CheckModifyState()
        {
            if (!this.IsModified)
                return false;

            string message = string.Format("{0}已经被修改,是否保存？", this.Document.DocTitle);
            DialogResult result = MessageBoxEx.ShowQuestion(message);
            if (result == DialogResult.Yes)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                bool success = this.SaveDocument();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return !success;
            }
            return (result == DialogResult.Cancel);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SaveDocument();
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
            this.RecreateDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPreview_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }

            if (this.CheckModifyState())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.documentControl1.PreviewDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPrint_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintNuringDoc.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印护理文书!");
                return;
            }

            if (this.CheckModifyState())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.documentControl1.PrintDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnButton1_Click(object sender, EventArgs e)
        {
            object param = this.toolbtnButton1.Text;
            this.documentControl1.SaveFormData(ref param);
        }

        private void toolbtnButton2_Click(object sender, EventArgs e)
        {
            object param = this.toolbtnButton2.Text;
            this.documentControl1.SaveFormData(ref param);
        }

        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            ///由于hide方法内已经有检查修改状态的语句，所以取消这里的修改状态检查。
            //if (!this.CheckModifyState())
            //{
            //    this.documentControl1.IsModified = false;
            //}
            this.Hide();
        }

        private void tooldtpRecordTime_ValueChanged(object sender, EventArgs e)
        {
            DateTime dtRecordTime = this.tooldtpRecordTime.Value;
            if (this.Document != null)
                this.Document.ModifyRecordTime(dtRecordTime);

            if (!this.m_bControlValueChangedEventEnabled)
                return;
            this.documentControl1.UpdateFormData("更新记录时间", dtRecordTime);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.toolbtnReturn.PerformClick();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void documentControl1_QueryContext(object sender, QueryContextEventArgs e)
        {
            if (e.Name == "评估时间" || e.Name == "记录时间")
            {
                e.Success = true;
                e.Value = this.tooldtpRecordTime.Value.AddSeconds(-this.tooldtpRecordTime.Value.Second);
            }
        }

        private void documentControl1_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.Name == "更新记录时间")
            {
                if (e.Data == null)
                    return;
                this.m_bControlValueChangedEventEnabled = false;
                if (e.Data is DateTime)
                    this.tooldtpRecordTime.Value = (DateTime)e.Data;
                this.m_bControlValueChangedEventEnabled = true;
            }
            else if (e.Name == "新增")
            {
                this.toolbtnNew.PerformClick();
            }
            else if (e.Name == "保存")
            {
                this.toolbtnSave.PerformClick();
            }
            else if (e.Name == "返回")
            {
                this.toolbtnReturn.PerformClick();
            }
            else if (e.Name == "按钮1名称")
            {
                this.toolbtnButton1.Text = e.Param as string;
            }
            else if (e.Name == "按钮2名称")
            {
                this.toolbtnButton2.Text = e.Param as string;
            }
            else if (e.Name == "按钮1图标")
            {
                this.toolbtnButton1.Image = e.Param as Image;
            }
            else if (e.Name == "按钮2图标")
            {
                this.toolbtnButton2.Image = e.Param as Image;
            }
            else if (e.Name == "按钮1可见")
            {
                if (e.Param is bool)
                    this.toolbtnButton1.Visible = (bool)e.Param;
            }
            else if (e.Name == "按钮2可见")
            {
                if (e.Param is bool)
                    this.toolbtnButton2.Visible = (bool)e.Param;
            }
            else if (e.Name == "按钮1可用")
            {
                if (e.Param is bool)
                    this.toolbtnButton1.Enabled = (bool)e.Param;
            }
            else if (e.Name == "按钮2可用")
            {
                if (e.Param is bool)
                    this.toolbtnButton2.Enabled = (bool)e.Param;
            }
            else if (e.Name == "再次新增")
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.RecreateDocument();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            else if (e.Name == "重复新增")
            {
                if (this.CheckModifyState())
                    return;
                if (this.Document == null)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                bool bDocumentUpdated = this.m_documentUpdated;
                XmlDocument xmldoc = this.documentControl1.ExportXml(true);
                DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(this.Document.DocTypeID);
                if (this.CreateDocument(docTypeInfo))
                    this.documentControl1.SetControlValue(xmldoc);
                this.documentControl1.IsModified = false;
                this.m_documentUpdated = bDocumentUpdated;
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            else if (e.Name == "单病人评估任务")
            {
                if (this.IsDisposed || !this.IsHandleCreated)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                DataTable dtPatientInfo = null;
                if (e.Data is DataTable)
                {
                    dtPatientInfo = e.Data as DataTable;
                }
                else
                {
                    DateTime dtCurrentTime = SysTimeService.Instance.Now;
                    dtPatientInfo = new DataTable();
                    dtPatientInfo.Columns.Add("PATIENT_ID");
                    dtPatientInfo.Columns.Add("VISIT_ID");
                    dtPatientInfo.Columns.Add("BeginTime");
                    dtPatientInfo.Columns.Add("EndTime");
                    dtPatientInfo.Columns.Add("PreBeginTime");
                    dtPatientInfo.Columns.Add("PreEndTime");
                    ShiftRankInfo shiftRankInfo = new ShiftRankInfo();
                    List<ShiftRankInfo> lstNursingShiftInfo = new List<ShiftRankInfo>();
                    string szWardCode = SystemContext.Instance.LoginUser.WardCode;
                    short shRet = NurShiftService.Instance.GetShiftRankInfos(szWardCode, null, ref lstNursingShiftInfo);
                    foreach (ShiftRankInfo item in lstNursingShiftInfo)
                    {
                        item.UpdateShiftRankTime(dtCurrentTime);
                        if (dtCurrentTime >= item.StartTime && dtCurrentTime <= item.EndTime)
                        {
                            shiftRankInfo = item;
                            break;
                        }
                    }
                    DataRow row = dtPatientInfo.Rows.Add();
                    row["PATIENT_ID"] = PatientTable.Instance.ActivePatient.PatientID;
                    row["VISIT_ID"] = PatientTable.Instance.ActivePatient.VisitID;
                    row["BeginTime"] = shiftRankInfo.StartTime;
                    row["EndTime"] = shiftRankInfo.EndTime;
                }
                DataTable dtNursingAssessInfo = new DataTable();
                PatientTaskService.Instance.GetPatientAssessTaskList(dtPatientInfo, ref dtNursingAssessInfo);
                e.Result = dtNursingAssessInfo;
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
        }

        /// <summary>
        /// 判断该文档是否已通过审核
        /// </summary>
        /// <param name="documentInfo">文档信息信息</param>
        /// <returns>true or false</returns>
        private bool IsDocExamined(DocumentInfo documentInfo)
        {
            QCExamineInfo qcExamineInfo = new QCExamineInfo();
            short shRet = QCExamineService.Instance.GetQcExamineInfo(documentInfo.DocID, documentInfo.DocTitle, documentInfo.PatientID, documentInfo.VisitID, ref qcExamineInfo);
            if (shRet == ServerData.ExecuteResult.OK)
            {
                if (qcExamineInfo.QcExamineStatus == "1")
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
