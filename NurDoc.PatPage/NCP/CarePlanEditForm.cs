// ***********************************************************
// 护理电子病历系统,护理计划编辑窗口.
// Creator:OuFengFang  Date:2013-3-27
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
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.PatPage.NCP
{
    internal partial class CarePlanEditForm : HerenForm
    {
        private NurCarePlanInfo m_ncpInfo = null;

        /// <summary>
        /// 获取当前护理计划记录编辑器绑定的护理计划记录信息
        /// </summary>
        [Browsable(false)]
        public NurCarePlanInfo NurCarePlanInfo
        {
            get { return this.m_ncpInfo; }
        }

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

        private bool m_documentUpdated = false;

        /// <summary>
        /// 获取当前文档编辑器表单是否更新过
        /// </summary>
        [Browsable(false)]
        public bool IsDocumentUpdated
        {
            get { return this.m_documentUpdated; }
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
        /// 当文档被保存时触发
        /// </summary>
        [Description("当文档被保存时触发")]
        public event EventHandler DocumentSaved;

        protected virtual void OnDocumentSaved(EventArgs e)
        {
            if (this.DocumentSaved != null)
                this.DocumentSaved(this, e);
        }

        /// <summary>
        /// 该变量用于指示当前护理计划记录是否是新建状态
        /// </summary>
        private bool m_bIsNewRecord = false;

        /// <summary>
        /// 该变量用于指示某些控件值改变事件是否可用
        /// </summary>
        private bool m_bControlValueChangedEventEnabled = true;

        public CarePlanEditForm()
        {
            this.InitializeComponent();

            DateTime dtNow = SysTimeService.Instance.Now;
            dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day
                , dtNow.Hour, dtNow.Minute, 0);
            this.tooldtpDateFrom.Value = dtNow;

            if (this.documentControl1 == null)
                return;
            this.documentControl1.QueryContext +=
                new QueryContextEventHandler(this.documentControl1_QueryContext);
            this.documentControl1.CustomEvent +=
                new CustomEventHandler(this.documentControl1_CustomEvent);
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
            this.m_bIsNewRecord = true;

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
            this.tooldtpDateFrom.Value = dtNow;

            if (this.Document != null)
                this.Document.ModifyRecordTime(this.tooldtpDateFrom.Value);

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
        public bool OpenDocument(NurDocInfo docInfo, NurCarePlanInfo ncpInfo)
        {
            if (docInfo == null)
                return false;

            this.m_ncpInfo = ncpInfo;
            if (this.m_ncpInfo != null)
            {
                this.m_bIsNewRecord = false;
            }
            else
            {
                this.m_bIsNewRecord = true;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.m_documentUpdated = false;

            this.tooldtpDateFrom.Value = docInfo.RecordTime;
            DocumentInfo document = DocumentInfo.Create(docInfo);
            bool success = this.documentControl1.OpenDocument(document);
            if (this.documentControl1.Document != null)
            {
                this.tooldtpDateFrom.Value = this.documentControl1.Document.RecordTime;
                this.tooltxtCreatorName.Text = this.documentControl1.Document.CreatorName;
            }
            this.HandleUILayout(docInfo.DocTypeID);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        /// <summary>
        /// 用于护理记录列表窗口调用
        /// </summary>
        /// <param name="szValue">字符标示</param>
        /// <returns>boolean</returns>
        public bool UpdateDocument(string szValue)
        {
            if (this.documentControl1.Document != null) 
            {
                this.documentControl1.UpdateFormData(szValue, null);
            }
            return true;
        }

        /// <summary>
        /// 保存当前护理计划数据
        /// </summary>
        /// <returns>是否成功</returns>
        public bool SaveDocument()
        {
            if (this.Document == null)
                return false;
            Application.DoEvents();

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
                this.OnDocumentSaved(EventArgs.Empty);

                object param = string.Empty;
                this.documentControl1.SaveFormData(ref param);
            }
            return success;
        }

        /// <summary>
        /// 保存护理计划记录本身
        /// </summary>
        /// <returns>是否保存成功</returns>
        private bool SaveNurCarePlan()
        {
            Application.DoEvents();

            //获取诊断编码
            string szDiagCode = string.Empty;
            object content = this.documentControl1.GetFormData("诊断编码");
            if (content != null)
                szDiagCode = content.ToString();

            //获取诊断名称
            string szDiagName = string.Empty;
            content = this.documentControl1.GetFormData("诊断名称");
            if (content != null)
                szDiagName = content.ToString();

            //获取开始时间
            DateTime dtStartTime = SysTimeService.Instance.Now;
            content = this.documentControl1.GetFormData("开始时间");
            if (content != null)
                dtStartTime = GlobalMethods.Convert.StringToValue(content, DateTime.Now);

            bool success = true;
            if (this.m_bIsNewRecord)
            {
                success = NurCarePlanHandler.Instance.HandleNurCarePlanNewSave(this.Document
                    , szDiagCode, szDiagName, dtStartTime, DateTime.MinValue, ServerData.NurCarePlanStatus.PROGRESS);
                if (!success)
                    return false;
                this.m_bIsNewRecord = false;
            }
            else
            {
                if (this.NurCarePlanInfo == null)
                    return false;
                success = NurCarePlanHandler.Instance.HandleNurCarePlanModifySave(this.NurCarePlanInfo
                    , szDiagCode, szDiagName, dtStartTime, this.NurCarePlanInfo.EndTime, this.NurCarePlanInfo.Status);
                if (!success)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 根据病历属性信息处理UI界面上显示的按钮
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        private void HandleUILayout(string szDocTypeID)
        {
            Application.DoEvents();
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        public bool CheckModifyState()
        {
            if (!this.IsModified || this.Document == null)
                return false;

            string message = string.Format("{0}已经被修改,是否保存？", this.Document.DocTitle);
            DialogResult result = MessageBoxEx.ShowQuestion(message);
            if (result == DialogResult.Yes)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                bool success = false;
                if (this.SaveDocument())
                    success = this.SaveNurCarePlan();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return !success;
            }
            return (result == DialogResult.Cancel);
        }

        private void toolbtnNew_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RecreateDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveDocument())
            {
                if (this.SaveNurCarePlan())
                {
                    string key = SystemConst.ConfigKey.NCP_SAVE_RETURN;
                    if (SystemContext.Instance.GetConfig(key, false))
                        this.Hide();
                }
            }
            //this.SaveDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            if (!this.CheckModifyState())
            {
                this.documentControl1.IsModified = false;
                this.Hide();
            }
        }

        private void tooldtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime dtRecordTime = this.tooldtpDateFrom.Value;
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
                e.Value = this.tooldtpDateFrom.Value;
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
                    this.tooldtpDateFrom.Value = (DateTime)e.Data;
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
        }
    }
}
