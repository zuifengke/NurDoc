// ***********************************************************
// 护理电子病历系统,病历指控窗口.
// Creator:YeChongchong  Date:2014-1-5
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.DockSuite;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Frame.DockForms;
using Heren.NurDoc.PatPage.Document;
using Heren.NurDoc.Utilities.Dialogs;

namespace Heren.NurDoc.Frame
{
    internal partial class QcNurDocForm : HerenForm
    {
        private DocumentInfo m_document = null;

        /// <summary>
        /// 获取当前表单的文档信息
        /// </summary>
        [Browsable(false)]
        public DocumentInfo Document
        {
            get { return this.m_document; }
        }

        // 获取或设置当前的索引
        private int m_index = 0;

        /// <summary>
        /// 获取当前窗口的表单编辑器
        /// </summary>
        [Browsable(false)]
        public DocumentControl FormEditor
        {
            get { return this.documentControl1; }
        }

        public List<NurDocInfo> m_lstNurDocInfo = null;

        public QcNurDocForm(NurDocInfo nurDocInfo)
        {
            this.InitializeComponent();
            this.m_document = DocumentInfo.Create(nurDocInfo);
            if (this.m_document == null)
            {
                MessageBoxEx.Show("文档加载失败！");
                return;
            }
            this.m_lstNurDocInfo = new List<NurDocInfo>();
            this.m_lstNurDocInfo.Add(nurDocInfo);
            this.m_index = 0;
            
            this.toollblDocName.Text = this.m_document.DocTitle;
        }

        public QcNurDocForm(List<NurDocInfo> lstNurDocInfo, int intIndex)
        {
            this.InitializeComponent();
            if (lstNurDocInfo != null)
            {
                this.m_lstNurDocInfo = lstNurDocInfo;
                this.m_index = intIndex;
                this.m_document = DocumentInfo.Create(lstNurDocInfo[intIndex]);
                this.toollblDocName.Text = this.m_document.DocTitle;
            }
            else
            {
                MessageBoxEx.Show("文档列表不可为空！");
                return;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.documentControl1.OpenDocument(this.m_document);
            this.SetQcStatus(this.m_document);
            if (this.FormEditor.Controls.Count > 0)
            {
                Size size = this.FormEditor.Controls[0].Size;
                this.FormEditor.Controls[0].Enabled = false;
                GlobalMethods.UI.LocateScreenCenter(this, size);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 根据当前界面文档信息查询质控审核状态并更改界面显示
        /// </summary>
        /// <param name="documentInfo">文档信息</param>
        private void SetQcStatus(DocumentInfo documentInfo)
        {
            QCExamineInfo qcExamineInfo = new QCExamineInfo();
            short shRet = QCExamineService.Instance.GetQcExamineInfo(documentInfo.DocID, documentInfo.DocTitle, documentInfo.PatientID, documentInfo.VisitID, ref qcExamineInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                this.toollblStatus.Text = ServerData.ExamineStatus.QC_NONE;
                this.toolbtnQc.Enabled = true;
                this.toolbtnQcCancel.Enabled = false;
                this.toolbtnQcQuestion.Enabled = true;
            }
            else
            {
                if (qcExamineInfo.QcExamineStatus == "1")
                {
                    this.toollblStatus.Text = ServerData.ExamineStatus.QC_OK;
                    this.toolbtnQc.Enabled = false;
                    this.toolbtnQcCancel.Enabled = true;
                    this.toolbtnQcQuestion.Enabled = false;
                }
                else
                {
                    this.toollblStatus.Text = ServerData.ExamineStatus.QC_MARK;
                    this.toolbtnQc.Enabled = true;
                    this.toolbtnQcCancel.Enabled = false;
                    this.toolbtnQcQuestion.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 打开表单,并更新审核状态
        /// </summary>
        /// <param name="nurDocInfo">文档信息类</param>
        private void Open_Document(NurDocInfo nurDocInfo)
        {
            this.m_document = DocumentInfo.Create(nurDocInfo);
            this.toollblDocName.Text = this.m_document.DocTitle;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.documentControl1.OpenDocument(this.m_document);
            this.SetQcStatus(this.m_document);
            if (this.FormEditor.Controls.Count > 0)
            {
                Size size = this.FormEditor.Controls[0].Size;
                this.FormEditor.Controls[0].Enabled = false;
                GlobalMethods.UI.LocateScreenCenter(this, size);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 根据文档信息创建质控信息类
        /// </summary>
        /// <param name="documentInfo">文档信息</param>
        /// <returns>质控信息类</returns>
        private QCExamineInfo Create(DocumentInfo documentInfo)
        {
            QCExamineInfo qcExamineInfo = new QCExamineInfo();
            qcExamineInfo.QcContentKey = documentInfo.DocID;
            qcExamineInfo.QcContentType = documentInfo.DocTitle;
            qcExamineInfo.PatientID = documentInfo.PatientID;
            qcExamineInfo.PatientName = documentInfo.PatientName;
            qcExamineInfo.VisitID = documentInfo.VisitID;
            qcExamineInfo.WardCode = documentInfo.WardCode;
            qcExamineInfo.WardName = documentInfo.WardName;
            qcExamineInfo.QcExamineStatus = string.Empty;
            qcExamineInfo.QcExamineContent = string.Empty;
            qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
            qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
            qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
            return qcExamineInfo;
        }

        /// <summary>
        /// 更新文档相关审核状态
        /// </summary>
        private void RefreshData()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.Open_Document(this.m_lstNurDocInfo[this.m_index]);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnLast_Click(object sender, EventArgs e)
        {
            if (this.m_index <= 0)
            {
                MessageBox.Show("已经是第一份");
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.documentControl1.Focus();
            this.Open_Document(this.m_lstNurDocInfo[--this.m_index]);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnNext_Click(object sender, EventArgs e)
        {
            if (this.m_index >= m_lstNurDocInfo.Count - 1)
            {
                MessageBox.Show("已经是最后一份");
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.documentControl1.Focus();
            this.Open_Document(this.m_lstNurDocInfo[++this.m_index]);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnQc_Click(object sender, EventArgs e)
        {
            if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_NONE)
            {
                QCExamineInfo qcExamineInfo = this.Create(m_document);
                qcExamineInfo.QcExamineStatus = "1";
                qcExamineInfo.QcExamineContent = string.Empty;
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核失败！");
                    return;
                }
            }
            else if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_document.DocID, this.m_document.DocTitle, this.m_document.PatientID, this.m_document.VisitID, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }
                qcExamineInfo.QcExamineStatus = "1";
                qcExamineInfo.QcExamineContent = string.Empty;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核失败！");
                    return;
                }
            }
            this.RefreshData();
        }

        private void toolbtnQcCancel_Click(object sender, EventArgs e)
        {
            if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_OK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_document.DocID, this.m_document.DocTitle, this.m_document.PatientID, this.m_document.VisitID, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("理由", qcExamineInfo.QcExamineContent);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("取消审核失败！");
                    return;
                }
            }
            this.RefreshData();
        }

        private void toolbtnQcQuestion_Click(object sender, EventArgs e)
        {
            if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_NONE)
            {
                ReasonDialog reasonDialog = new ReasonDialog("理由", string.Empty);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                QCExamineInfo qcExamineInfo = this.Create(m_document);
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                short shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("标记审核内容失败！");
                    return;
                }
            }
            else if (this.toollblStatus.Text == ServerData.ExamineStatus.QC_MARK)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                short shRet = QCExamineService.Instance.GetQcExamineInfo(this.m_document.DocID, this.m_document.DocTitle, this.m_document.PatientID, this.m_document.VisitID, ref qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("审核数据查询失败！");
                    return;
                }

                ReasonDialog reasonDialog = new ReasonDialog("理由", qcExamineInfo.QcExamineContent);
                if (reasonDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                qcExamineInfo.QcExamineStatus = "0";
                qcExamineInfo.QcExamineContent = reasonDialog.Reason;
                qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
                qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                shRet = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    MessageBoxEx.Show("标记审核内容失败！");
                    return;
                }
            }
            this.RefreshData();
        }
    }
}