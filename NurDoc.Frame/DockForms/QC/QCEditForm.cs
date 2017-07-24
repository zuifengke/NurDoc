// ***********************************************************
// ������Ӳ���ϵͳ,�����밲ȫ�����¼���ĵ��༭����.
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
        /// ��ȡ��ǰ���༭���������밲ȫ�����¼�ĵ���Ϣ
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
        /// ��ȡ�����ñ��༭���Ƿ���ʾ���ذ�ť
        /// </summary>
        [DefaultValue(true)]
        [Description("��ȡ�����ñ��༭���Ƿ���ʾ���ذ�ť")]
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
        /// ��ȡ�����õ�ǰ�Ƿ��һ���½������밲ȫ�����¼
        /// </summary>
        [Browsable(false)]
        public bool IsFirstRecord
        {
            get { return this.m_bIsFirstRecord; }
            set { this.m_bIsFirstRecord = value; }
        }

        /// <summary>
        /// ��ȡ��ǰ���������Ƿ��Ѿ����޸�
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
        /// ���ĵ����������ʱ����
        /// </summary>
        [Description("���ĵ����������ʱ����")]
        public event EventHandler DocumentUpdated;

        protected virtual void OnDocumentUpdated(EventArgs e)
        {
            if (this.DocumentUpdated != null)
                this.DocumentUpdated(this, e);
        }

        /// <summary>
        /// ��ȡ��ǰ�ĵ��༭�����Ƿ���¹�
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
        /// ��ʶ�ؼ�ֵ�ı��¼��Ƿ����,�����ظ�ִ��
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
            //Ĭ�ϻ�ȡ��Ӧ��doctype�Ա㱣��ʱ�ж�
            QCTypeInfo qcTypeInfo = FormCache.Instance.GetQCTypeInfo(this.m_document.DocTypeID);
            this.m_QCTypeInfo = qcTypeInfo;
            if (this.m_document.DocState == DocumentState.New)
                this.CreateDocument(qcTypeInfo);
            else
                this.OpenDocument(this.m_document.ToQCInfo());
            GlobalMethods.UI.SetCursor(this, Cursors.Default);            
        }

        /// <summary>
        /// �رյ�ǰ�ĵ�����ʾ
        /// </summary>
        public void CloseDocument()
        {
            this.documentControl1.CloseDocument();
        }

        /// <summary>
        /// ����ָ���������밲ȫ�����¼���ʹ����µ������밲ȫ�����¼
        /// </summary>
        /// <param name="qcTypeInfo">�����밲ȫ�����¼������Ϣ</param>
        /// <returns>�Ƿ�ɹ�</returns>
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

            //������Ĭ�ϵļ�¼ʱ��Ϊ��ǰʱ�䣨���ڵڶ�������ȡ�ϴμ�¼ʱ�䣩xpp
            DateTime dtNow = SysTimeService.Instance.Now;
            dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day
                , dtNow.Hour, dtNow.Minute, 0);
            if (this.tooldtpRecordTime.IsDisposed != true)
                this.tooldtpRecordTime.Value = dtNow;

            if (this.m_bIsFirstRecord)
            {
                object firstTime = this.documentControl1.GetFormData("��һ�μ�¼ʱ��");
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
        /// ����ָ���������밲ȫ�����¼���ʹ����µ������밲ȫ�����¼
        /// </summary>
        /// <param name="qcTypeInfo">�����밲ȫ�����¼������Ϣ</param>
        /// <param name="creatTime">����ʱ��</param>
        /// <returns>�Ƿ�ɹ�</returns>
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

            //������Ĭ�ϵļ�¼ʱ��Ϊ��ǰʱ�䣨���ڵڶ�������ȡ�ϴμ�¼ʱ�䣩xpp
            DateTime dtTime = creatTime;
            dtTime = new DateTime(dtTime.Year, dtTime.Month, dtTime.Day
                , dtTime.Hour, dtTime.Minute, 0);
            if (this.tooldtpRecordTime.IsDisposed != true)
                this.tooldtpRecordTime.Value = dtTime;

            if (this.m_bIsFirstRecord)
            {
                object firstTime = this.documentControl1.GetFormData("��һ�μ�¼ʱ��");
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
        /// ���ݵ�ǰ����ʾ���ĵ����´����ĵ�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
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
                MessageBoxEx.ShowFormat("û���ҵ���{0}��������!", null, szQCTypeID);
                return false;
            }
            bool bDocumentUpdated = this.m_documentUpdated;
            this.CreateDocument(qcTypeInfo);
            this.m_documentUpdated = bDocumentUpdated;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// ��ָ������д�������밲ȫ�����¼��Ϣ�ĵ�
        /// </summary>
        /// <param name="qcInfo">��д�����밲ȫ�����¼��Ϣ</param>
        /// <returns>�Ƿ�ɹ�</returns>
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
        ///// ��ָ������д�����밲ȫ�����¼�ĵ�
        ///// </summary>
        ///// <param name="docInfo">��д�����밲ȫ�����¼��Ϣ</param>
        ///// <returns>�Ƿ�ɹ�</returns>
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
        /// ���ָ������д�������밲ȫ�����¼�ĵ�ǰ�ж��Ƿ���ĵ�״̬�ǲ���
        /// </summary>
        /// <param name="qcInfo">��д�����밲ȫ�����¼��Ϣ</param>
        /// <returns>�Ƿ�ɹ�</returns>
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
            bool checkin = this.documentControl1.CheckFormData("���ؿ�ɾ���ж�");
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
        /// ���浱ǰ�����밲ȫ�����¼����
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool SaveDocument()
        {
            if (this.Document == null)
                return false;
            Application.DoEvents();
            short shRet = SystemConst.ReturnValue.OK;
            if (!this.CheckRight())
            {
                MessageBoxEx.Show("��ǰ��¼��ʿ�ޱ���Ȩ�ޣ�");
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
                    MessageBoxEx.Show(string.Format("���ĵ������˲������������½����!"));
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
                MessageBoxEx.Show(string.Format("�ò����ظ��ĵ������˴����������½����!"));
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
        /// �ж��Ƿ�Ϊ���°汾
        /// </summary>
        /// <param name="szQCSetID">�ĵ���ID</param>
        /// <param name="DocInfo">�ĵ���Ϣ</param>
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
        /// �Ƿ񱣴��ֱ�ӷ����б�
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
        /// ���������밲ȫ�����¼������Ϣ����UI��������ʾ�İ�ť
        /// </summary>
        /// <param name="szQCTypeID">�ĵ�����ID</param>
        private void HandleUILayout(string szQCTypeID)
        {
            Application.DoEvents();
            object visible = this.documentControl1.GetFormData("�������ɼ�");
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
        /// �ƶ�����������  ��������� �ٴ�summary�ж�ȡ(Ҳ���������������������)
        /// </summary>
        /// <returns>true or false</returns>
        private bool ClearData(QCDocInfo docInfo)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            object bclearData = this.documentControl1.GetFormData("�Ƿ��������");
            if ((bclearData is bool) ? (bool)bclearData : false)
            {
                this.documentControl1.ClearFormData();
                this.documentControl1.UpdateFormData("�������¼���", string.Empty);
            }
            Dictionary<string, KeyData> dicSummaryDatas = new Dictionary<string, KeyData>();
            //�����Զ�����
            DocumentService.Instance.GetSummaryData(docInfo.DocSetID, false, ref dicSummaryDatas);
            if (dicSummaryDatas.Count > 0)
                this.documentControl1.UpdateFormKeyData(dicSummaryDatas);
            this.documentControl1.IsModified = false;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// ��鵱ǰ�����¼�༭�����Ƿ��Ѿ��ı�
        /// </summary>
        /// <returns>�Ƿ�ȡ��</returns> 
        public bool CheckModifyState()
        {
            if (!this.IsModified)
                return false;

            string message = string.Format("{0}�Ѿ����޸�,�Ƿ񱣴棿", this.m_QCTypeInfo.QCTypeName);
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
            this.documentControl1.UpdateFormData("���¼�¼ʱ��", dtRecordTime);
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
            if (e.Name == "����ʱ��" || e.Name == "��¼ʱ��")
            {
                e.Success = true;
                e.Value = this.tooldtpRecordTime.Value.AddSeconds(-this.tooldtpRecordTime.Value.Second);
            }
        }

        private void documentControl1_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.Name == "���¼�¼ʱ��")
            {
                if (e.Data == null)
                    return;
                this.m_bValueChangedEnabled = false;
                if (e.Data is DateTime)
                    this.tooldtpRecordTime.Value = (DateTime)e.Data;
                this.m_bValueChangedEnabled = true;
            }
            else if (e.Name == "����")
            {
                this.toolbtnSave.PerformClick();
            }
            else if (e.Name == "����")
            {
                this.toolbtnReturn.PerformClick();
            }
        }
    }
}