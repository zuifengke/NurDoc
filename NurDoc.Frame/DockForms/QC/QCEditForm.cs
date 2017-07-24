// ***********************************************************
// 护理电子病历系统,质量与安全管理记录表单文档编辑窗口.
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
using Heren.NurDoc.PatPage.Document;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Frame.DockForms
{
    public partial class QCEditForm : HerenForm
    {
        private DocumentInfo m_document = null;

        /// <summary>
        /// 获取当前表单编辑器绑定质量与安全管理记录文档信息
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
        
        private QCTypeInfo m_QCTypeInfo = null;

        public QCTypeInfo QCTypeInfo
        {
            get { return this.m_QCTypeInfo; }
            set { this.m_QCTypeInfo = value; }
        }

        private QCDocInfo m_QCDocInfo = null;

        public QCDocInfo QCDocInfo
        {
            get { return this.m_QCDocInfo; }
            set { this.m_QCDocInfo = value; }
        }

        private bool m_bIsFirstRecord = false;

        /// <summary>
        /// 获取或设置当前是否第一次新建质量与安全管理记录
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
        /// 获取当前文档编辑器表单是否更新过
        /// </summary>
        private bool m_documentUpdated = false;

        public QCEditForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.SysIcon;

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

        /// <summary>
        /// 标识控件值改变事件是否可用,避免重复执行
        /// </summary>
        private bool m_bValueChangedEnabled = true;

        public DialogResult ShowDialog(QCDocInfo docInfo, IWin32Window owner)
        {
            this.m_document = DocumentInfo.Create(docInfo);
            return base.ShowDialog(owner);
        }

        public DialogResult ShowDialog(QCTypeInfo qcTypeInfo, IWin32Window owner)
        {
            this.m_document = DocumentInfo.Create(qcTypeInfo, null);
            return base.ShowDialog(owner);
        }

        public void Show(QCDocInfo qcInfo, IWin32Window owner)
        {
            this.m_document = DocumentInfo.Create(qcInfo);
            base.Show(owner);
        }

        new public void Hide()
        {
            if (!this.CheckModifyState())
            {
                base.Hide();
                this.documentControl1.IsModified = false;
                if (this.m_documentUpdated)
                    this.OnDocumentUpdated(EventArgs.Empty);
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
            //默认获取相应的doctype以便保存时判断
            QCTypeInfo qcTypeInfo = FormCache.Instance.GetQCTypeInfo(this.m_document.DocTypeID);
            this.m_QCTypeInfo = qcTypeInfo;
            if (this.m_document.DocState == DocumentState.New)
                this.CreateDocument(qcTypeInfo);
            else
                this.OpenDocument(this.m_document.ToQCInfo());
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
        /// 根据指定的质量与安全管理记录类型创建新的质量与安全管理记录
        /// </summary>
        /// <param name="qcTypeInfo">质量与安全管理记录类型信息</param>
        /// <returns>是否成功</returns>
        public bool CreateDocument(QCTypeInfo qcTypeInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (qcTypeInfo == null)
                return false;
            this.QCTypeInfo = qcTypeInfo;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.m_documentUpdated = false;

            DocumentInfo document = DocumentInfo.Create(qcTypeInfo, null);
            bool success = this.documentControl1.OpenDocument(document);
            this.HandleUILayout(qcTypeInfo.QCTypeID);

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
        /// 根据指定的质量与安全管理记录类型创建新的质量与安全管理记录
        /// </summary>
        /// <param name="qcTypeInfo">质量与安全管理记录类型信息</param>
        /// <param name="creatTime">创建时间</param>
        /// <returns>是否成功</returns>
        public bool CreateDocument(QCTypeInfo qcTypeInfo, DateTime creatTime)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (qcTypeInfo == null)
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.m_documentUpdated = false;

            DocumentInfo document = DocumentInfo.Create(qcTypeInfo, null);

            bool success = this.documentControl1.OpenDocument(document);
            this.HandleUILayout(qcTypeInfo.QCTypeID);

            //设置下默认的记录时间为当前时间（由于第二次新增取上次记录时间）xpp
            DateTime dtTime = creatTime;
            dtTime = new DateTime(dtTime.Year, dtTime.Month, dtTime.Day
                , dtTime.Hour, dtTime.Minute, 0);
            if (this.tooldtpRecordTime.IsDisposed != true)
                this.tooldtpRecordTime.Value = dtTime;

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
            string szQCTypeID = this.Document.DocTypeID;
            QCTypeInfo qcTypeInfo = FormCache.Instance.GetQCTypeInfo(szQCTypeID);
            if (qcTypeInfo == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowFormat("没有找到“{0}”表单类型!", null, szQCTypeID);
                return false;
            }
            bool bDocumentUpdated = this.m_documentUpdated;
            this.CreateDocument(qcTypeInfo);
            this.m_documentUpdated = bDocumentUpdated;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 打开指定的已写的质量与安全管理记录信息文档
        /// </summary>
        /// <param name="qcInfo">已写质量与安全管理记录信息</param>
        /// <returns>是否成功</returns>
        public bool OpenDocument(QCDocInfo qcInfo)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return false;
            if (qcInfo == null)
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            this.m_documentUpdated = false;

            DocumentInfo document = DocumentInfo.Create(qcInfo);
            bool success = this.documentControl1.OpenQCDocument(document);
            this.HandleUILayout(qcInfo.DocTypeID);

            this.ClearData(qcInfo);
            if (this.documentControl1.Document != null && this.tooltxtCreatorName.IsDisposed != true)
            {
                this.tooldtpRecordTime.Value = this.documentControl1.Document.RecordTime;
                this.tooltxtCreatorName.Text = this.documentControl1.Document.CreatorName;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return success;
        }

        ///// <summary>
        ///// 打开指定的已写质量与安全管理记录文档
        ///// </summary>
        ///// <param name="docInfo">已写质量与安全管理记录信息</param>
        ///// <returns>是否成功</returns>
        //public bool OpenDocument(QCTypeInfo typeInfo, QCDocInfo docInfo, bool bIsNewDocument)
        //{
        //    if (this.IsDisposed || !this.IsHandleCreated)
        //        return false;
        //    if (docInfo == null || typeInfo == null)
        //        return false;
        //    this.m_QCTypeInfo = typeInfo;
        //    this.m_QCDocInfo = docInfo;
        //    GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
        //    Application.DoEvents();
        //    this.m_documentUpdated = false;
        //    this.tooltxtCreatorName.Text = docInfo.CreatorName;
        //    GlobalMethods.UI.SetCursor(this, Cursors.Default);
        //    return true;
        //}

        /// <summary>
        /// 检查指定的已写的质量与安全管理记录文档前判断是否该文档状态是驳回
        /// </summary>
        /// <param name="qcInfo">已写质量与安全管理记录信息</param>
        /// <returns>是否成功</returns>
        public bool CheckDocument(QCDocInfo qcInfo)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return false;
            if (qcInfo == null)
                return false;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            this.m_documentUpdated = false;

            DocumentInfo document = DocumentInfo.Create(qcInfo);

            bool success = this.documentControl1.OpenQCDocument(document);
            bool checkin = this.documentControl1.CheckFormData("驳回可删除判断");
            this.HandleUILayout(qcInfo.DocTypeID);
            if (this.documentControl1.Document != null)
            {
                this.tooldtpRecordTime.Value = this.documentControl1.Document.RecordTime;
                this.tooltxtCreatorName.Text = this.documentControl1.Document.CreatorName;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return checkin;
        }

        /// <summary>
        /// 保存当前质量与安全管理记录数据
        /// </summary>
        /// <returns>是否成功</returns>
        public bool SaveDocument()
        {
            if (this.Document == null)
                return false;
            Application.DoEvents();
            short shRet = SystemConst.ReturnValue.OK;
            if (!this.CheckRight())
            {
                MessageBoxEx.Show("当前登录护士无保存权限！");
                return false;
            }
            if (this.Document.DocState != DocumentState.New)
            {
                QCDocInfo docInfo = null;
                shRet = DocVersion(this.documentControl1.Document.DocSetID, ref docInfo);
                if (shRet != SystemConst.ReturnValue.OK && shRet != SystemConst.ReturnValue.NO_FOUND)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return true;
                }
                if (this.documentControl1.Document.DocVersion != docInfo.DocVersion)
                {
                    MessageBoxEx.Show(string.Format("该文档已有人操作过，请重新进入打开!"));
                    this.documentControl1.IsModified = false;
                    return false;
                }
            }

            QCDocList qcDocList = new QCDocList();
            string szCreatorID = SystemContext.Instance.LoginUser.ID;
            shRet = QCService.Instance.GetQCDocInfos(szCreatorID, this.documentControl1.Document.DocTypeID, ref qcDocList);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            if (this.QCTypeInfo != null && this.QCTypeInfo.IsRepeated == false && qcDocList.Count > 0
                && this.documentControl1.Document.DocSetID != qcDocList[0].DocSetID)
            {
                MessageBoxEx.Show(string.Format("该不可重复文档已有人创建，请重新进入打开!"));
                this.documentControl1.IsModified = false;
                return false;
            }

            if (!this.documentControl1.EndEdit())
                return false;
            if (!this.documentControl1.CheckFormData(null))
                return false;
            if (!this.documentControl1.SaveQCDocument())
                return false;

            DocumentInfo document = this.documentControl1.Document;
            bool success = this.documentControl1.CommitQCDocument(document);
            if (success)
            {
                this.m_documentUpdated = true;
                this.documentControl1.IsModified = false;

                if (this.IsSaveReturn(document.DocTypeID))
                {
                    base.Hide();
                }
                this.OnDocumentUpdated(EventArgs.Empty);

                object param = string.Empty;
                this.documentControl1.SaveFormData(ref param);
            }
            return success;
        }

        /// <summary>
        /// 判断是否为最新版本
        /// </summary>
        /// <param name="szQCSetID">文档集ID</param>
        /// <param name="DocInfo">文档信息</param>
        /// <returns>short</returns>
        public short DocVersion(string szQCSetID, ref QCDocInfo docInfo)
        {
            short shRet = SystemConst.ReturnValue.OK;
            shRet = QCService.Instance.GetLatestQCInfo(szQCSetID, ref docInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.NO_FOUND;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 是否保存后直接返回列表
        /// </summary>
        /// <returns>true or false</returns>
        private bool IsSaveReturn(string szQCTypeID)
        {
            QCTypeInfo qcTypeInfo = FormCache.Instance.GetQCTypeInfo(szQCTypeID);
            if (qcTypeInfo == null)
            {
                return false;
            }
            string key = SystemConst.ConfigKey.QC_SAVE_RETURN;
            return qcTypeInfo.IsRepeated && SystemContext.Instance.GetConfig(key, false);
        }

        /// <summary>
        /// 根据质量与安全管理记录属性信息处理UI界面上显示的按钮
        /// </summary>
        /// <param name="szQCTypeID">文档类型ID</param>
        private void HandleUILayout(string szQCTypeID)
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

            QCTypeInfo qcTypeInfo = FormCache.Instance.GetQCTypeInfo(szQCTypeID);
            if (qcTypeInfo == null)
                return;
            this.Text = qcTypeInfo.QCTypeName;
        }

        /// <summary>
        /// 移动端数据重现  先清空数据 再从summary中读取(也可用于其他表单的数据清除)
        /// </summary>
        /// <returns>true or false</returns>
        private bool ClearData(QCDocInfo docInfo)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            object bclearData = this.documentControl1.GetFormData("是否清空数据");
            if ((bclearData is bool) ? (bool)bclearData : false)
            {
                this.documentControl1.ClearFormData();
                this.documentControl1.UpdateFormData("数据重新加载", string.Empty);
            }
            Dictionary<string, KeyData> dicSummaryDatas = new Dictionary<string, KeyData>();
            //数据自动加载
            DocumentService.Instance.GetSummaryData(docInfo.DocSetID, false, ref dicSummaryDatas);
            if (dicSummaryDatas.Count > 0)
                this.documentControl1.UpdateFormKeyData(dicSummaryDatas);
            this.documentControl1.IsModified = false;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns> 
        public bool CheckModifyState()
        {
            if (!this.IsModified)
                return false;

            string message = string.Format("{0}已经被修改,是否保存？", this.m_QCTypeInfo.QCTypeName);
            DialogResult result = MessageBoxEx.ShowQuestion(message);
            if (result == DialogResult.Yes)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                bool success = this.SaveDocument();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }
            return (result == DialogResult.Cancel);
        }

        private void tooldtpRecordTime_ValueChanged(object sender, EventArgs e)
        {
            DateTime dtRecordTime = this.tooldtpRecordTime.Value;
            if (this.Document != null)
                this.Document.ModifyRecordTime(dtRecordTime);

            if (!this.m_bValueChangedEnabled)
                return;
            this.documentControl1.UpdateFormData("更新记录时间", dtRecordTime);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.toolbtnReturn.PerformClick();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SaveDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }


        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private bool CheckRight()
        {
            NurUserRight userRight = RightController.Instance.UserRight;
            if (userRight.ShowNursingAssessForm.Value)
                return true;
            return false;
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
                this.m_bValueChangedEnabled = false;
                if (e.Data is DateTime)
                    this.tooldtpRecordTime.Value = (DateTime)e.Data;
                this.m_bValueChangedEnabled = true;
            }
            else if (e.Name == "保存")
            {
                this.toolbtnSave.PerformClick();
            }
            else if (e.Name == "返回")
            {
                this.toolbtnReturn.PerformClick();
            }
        }
    }
}