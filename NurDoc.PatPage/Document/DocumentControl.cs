// ***********************************************************
// 护理电子病历系统,表单编辑器控件,
// 主要对底层Common.Forms.dll中的表单编辑器控件做了一次封装.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.Forms;
using Heren.Common.Forms.Editor;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.PatPage.Utility;

namespace Heren.NurDoc.PatPage.Document
{
    public partial class DocumentControl : FormEditor
    {
        private DocumentInfo m_document = null;

        /// <summary>
        /// 获取当前表单编辑器里加载的文档对象
        /// </summary>
        [Browsable(false)]
        public DocumentInfo Document
        {
            get { return this.m_document; }
            set { this.m_document = value; }
        }

        #region"文档打开"
        /// <summary>
        /// 关闭当前文档
        /// </summary>
        /// <returns>是否关闭成功</returns>
        public bool CloseDocument()
        {
            this.m_document = null;
            this.IsModified = false;
            return this.Load(new byte[0]);
        }

        /// <summary>
        /// 打开指定病历信息的病历
        /// </summary>
        /// <param name="document">病历信息</param>
        /// <returns>是否打开成功</returns>
        public bool OpenDocument(DocumentInfo document)
        {
            this.CloseDocument();
            if (document == null || document.DocState == DocumentState.None)
            {
                MessageBoxEx.Show("无法打开病历文档,文档信息创建失败!");
                return false;
            }

            this.m_document = document.Clone() as DocumentInfo;

            //处理病历新建
            if (this.m_document.DocData == null
                && this.m_document.DocState == DocumentState.New)
                return this.CreateDocument();

            //下载病历数据
            if (this.m_document.DocData == null)
            {
                string szDocID = this.m_document.DocID;
                byte[] byteDocData = null;
                short shRet = DocumentService.Instance.GetDocByID(szDocID, ref byteDocData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("病历数据下载失败!");
                    return false;
                }
                this.m_document.DocData = byteDocData;
            }

            //获取病历类型
            string szDocTypeID = this.m_document.DocTypeID;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.Show("病历模板信息下载失败!");
                return false;
            }

            //病历内容为空
            bool isTemplet = false;
            if (this.m_document.DocData == null
                || this.m_document.DocData.Length <= 0)
            {
                isTemplet = true;
                byte[] byteTempletData = null;
                if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData))
                {
                    MessageBoxEx.Show("病历模板数据下载失败!");
                    return false;
                }
                this.m_document.DocData = byteTempletData;
            }

            //如果当前已写的表单对应的模板有修改,
            //那么自动将用户数据转到新的模板里去
            bool success = false;
            if (!isTemplet && this.m_document.ModifyTime < docTypeInfo.ModifyTime)
            {
                byte[] byteTempletData = null;
                if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData))
                {
                    MessageBoxEx.Show("病历模板新版本下载失败!无法更新表单!");
                    return false;
                }
                success = this.Load(this.m_document.DocData, byteTempletData);
            }
            return success ? success : this.Load(this.m_document.DocData);
        }

        /// <summary>
        /// 创建一份新的文档
        /// </summary>
        /// <returns>是否创建成功</returns>
        private bool CreateDocument()
        {
            if (this.m_document == null)
                return false;
            byte[] byteTempletData = null;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(this.m_document.DocTypeID);
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.Show("无法创建病历文档,病历模板下载失败!");
                return false;
            }
            return this.Load(byteTempletData);
        }

        /// <summary>
        /// 打开指定质量与安全管理记录信息
        /// </summary>
        /// <param name="document">质量与安全管理记录信息</param>
        /// <returns>是否打开成功</returns>
        public bool OpenQCDocument(DocumentInfo document)
        {
            this.CloseDocument();
            if (document == null || document.DocState == DocumentState.None)
            {
                MessageBoxEx.Show("无法打开质量与安全管理记录文档,文档信息创建失败!");
                return false;
            }

            this.m_document = document.Clone() as DocumentInfo;

            //处理质量与安全管理记录新建
            if (this.m_document.DocData == null
                && this.m_document.DocState == DocumentState.New)
                return this.CreateDocument();

            //下载质量与安全管理记录数据
            if (this.m_document.DocData == null)
            {
                string szDocID = this.m_document.DocID;
                byte[] byteDocData = null;
                short shRet = QCService.Instance.GetDocByID(szDocID, ref byteDocData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("质量与安全管理记录数据下载失败!");
                    return false;
                }
                this.m_document.DocData = byteDocData;
            }

            //获取质量与安全管理记录类型
            string szDocTypeID = this.m_document.DocTypeID;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.Show("质量与安全管理记录模板信息下载失败!");
                return false;
            }

            //质量与安全管理记录内容为空
            bool isTemplet = false;
            if (this.m_document.DocData == null
                || this.m_document.DocData.Length <= 0)
            {
                isTemplet = true;
                byte[] byteTempletData = null;
                if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData))
                {
                    MessageBoxEx.Show("质量与安全管理记录模板数据下载失败!");
                    return false;
                }
                this.m_document.DocData = byteTempletData;
            }

            //如果当前已写的表单对应的模板有修改,
            //那么自动将用户数据转到新的模板里去
            bool success = false;
            if (!isTemplet && this.m_document.ModifyTime < docTypeInfo.ModifyTime)
            {
                byte[] byteTempletData = null;
                if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData))
                {
                    MessageBoxEx.Show("质量与安全管理记录模板新版本下载失败!无法更新表单!");
                    return false;
                }
                success = this.Load(this.m_document.DocData, byteTempletData);
            }
            return success ? success : this.Load(this.m_document.DocData);
        }
        #endregion

        #region"文档保存"
        /// <summary>
        /// 将当前病历信息保存到内存
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool SaveDocument()
        {
            if (this.m_document == null)
            {
                MessageBoxEx.Show("无法保存病历文档,文档信息对象非法!");
                return false;
            }

            byte[] byteDocData = null;
            this.Save(ref byteDocData);
            if (byteDocData == null || byteDocData.Length <= 0)
            {
                MessageBoxEx.Show("无法保存病历文档,文档数据生成失败!");
                return false;
            }
            this.m_document.DocData = byteDocData;
            this.m_document.SummaryData.Clear();
            this.m_document.SummaryData.AddRange(this.GetSummaryData());
            return true;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

                string szDocTypeID = this.Document.DocTypeID;
                if (string.IsNullOrEmpty(szDocTypeID))
                    return true;
                //重新查询获取文档类型信息
                DocTypeInfo docTypeInfo = null;
                short shRet = DocTypeService.Instance.GetDocTypeInfo(szDocTypeID, ref docTypeInfo);
                // 如果本地与服务器的版本相同,则无需重新加载
                DateTime dtModifyTime = FormCache.Instance.GetFormModifyTime(docTypeInfo.DocTypeID);
                if (dtModifyTime.CompareTo(docTypeInfo.ModifyTime) == 0)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return true;
                }
                byte[] byteTempletData = null;
                bool result = FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData);
                if (!result)
                {
                    MessageBoxEx.Show("刷新失败");
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return true;
                }
                byte[] byteDocData = null;
                this.Save(ref byteDocData);
                result = this.Load(byteDocData, byteTempletData);
                if (!result)
                {
                    MessageBoxEx.Show("刷新失败");
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return true;
                }
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// 获取当前表单编辑窗口中的摘要数据
        /// </summary>
        /// <returns>摘要数据列表</returns>
        private List<SummaryData> GetSummaryData()
        {
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            if (this.m_document == null)
                return lstSummaryData;

            List<KeyData> keyDataList = this.GetKeyDataList();
            if (keyDataList == null)
                return lstSummaryData;

            IEnumerator<KeyData> enumerator = keyDataList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SummaryData summaryData = new SummaryData();
                summaryData.DocID = this.m_document.DocSetID;
                summaryData.DocTypeID = this.m_document.DocTypeID;
                summaryData.PatientID = this.m_document.PatientID;
                summaryData.VisitID = this.m_document.VisitID;
                summaryData.RecordID = this.m_document.RecordID;
                summaryData.WardCode = this.m_document.WardCode;

                summaryData.SubID = this.m_document.SubID;
                if (!GlobalMethods.Misc.IsEmptyString(enumerator.Current.Tag))
                    summaryData.SubID = enumerator.Current.Tag;
                summaryData.DataName = enumerator.Current.Name;
                summaryData.DataCode = enumerator.Current.Code;
                summaryData.DataType = enumerator.Current.Type;
                summaryData.DataUnit = enumerator.Current.Unit;
                if (enumerator.Current.RecordTime == null)
                    summaryData.DataTime = this.m_document.RecordTime;
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
        /// 将当前病历提交到服务器
        /// </summary>
        /// <param name="document">病历信息</param>
        /// <returns>是否提交成功</returns>
        public bool CommitDocument(DocumentInfo document)
        {
            if (document == null)
                return false;

            short shRet = SystemConst.ReturnValue.OK;
            if (document.DocState == DocumentState.New)
            {
                NurDocInfo docInfo = document.ToDocInfo();
                shRet = DocumentService.Instance.SaveDoc(docInfo, document.DocData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("无法保存病历文档,保存至服务器失败!");
                    return false;
                }
            }
            else
            {
                document.HandleDocumentEdit(SystemContext.Instance.LoginUser);

                NurDocInfo docInfo = document.ToDocInfo();
                byte[] byteDocData = document.DocData;
                string szDocID = document.DocID;
                if (document.DocState == DocumentState.Revise)
                    szDocID = document.GetPrevVersionID();
                shRet = DocumentService.Instance.UpdateDoc(szDocID, docInfo, null, byteDocData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("无法更新病历文档,更新至服务器失败!");
                    return false;
                }
            }

            //保存当前病历摘要数据列表
            shRet = DocumentService.Instance.SaveSummaryData(document.DocSetID, document.SummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                MessageBoxEx.Show("病历保存成功,但摘要数据保存失败!");
            //删除由PDA录入的老摘要数据
            if (!string.IsNullOrEmpty(document.RecordID))
            {
                shRet = DocumentService.Instance.DeleteSummaryData(document.RecordID);
                if (shRet != SystemConst.ReturnValue.OK)
                    MessageBoxEx.Show("移除老摘要数据失败！");
            }

            //将文档状态由新建切换为编辑
            document.HandleDocumentEdit(SystemContext.Instance.LoginUser);

            //循环保存当前病历所有子文档
            foreach (DocumentInfo child in document.Childs)
            {
                if (!this.CommitDocument(child))
                    return false;
                DocumentService.Instance.SaveChildDocInfo(document.DocSetID, child.Caller, child.DocSetID);
            }
            return true;
        }

        /// <summary>
        /// 将当前质量与安全管理记录提交到服务器
        /// </summary>
        /// <param name="document">病历信息</param>
        /// <returns>是否提交成功</returns>
        public bool CommitQCDocument(DocumentInfo document)
        {
            if (document == null)
                return false;

            short shRet = SystemConst.ReturnValue.OK;
            if (document.DocState == DocumentState.New)
            {
                QCDocInfo docInfo = document.ToQCInfo();
                shRet = QCService.Instance.SaveDoc(docInfo, document.DocData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("无法保存病历文档,保存至服务器失败!");
                    return false;
                }
            }
            else
            {
                document.HandleDocumentEdit(SystemContext.Instance.LoginUser);

                QCDocInfo docInfo = document.ToQCInfo();
                byte[] byteDocData = document.DocData;
                string szDocID = document.DocID;
                if (document.DocState == DocumentState.Revise)
                    szDocID = document.GetPrevVersionID();
                shRet = QCService.Instance.UpdateDoc(szDocID, docInfo, null, byteDocData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("无法更新病历文档,更新至服务器失败!");
                    return false;
                }
            }
            
            //保存当前病历摘要数据列表
            shRet = QCService.Instance.SaveQCSummaryData(document.DocSetID, document.QCSummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                MessageBoxEx.Show("病历保存成功,但摘要数据保存失败!");
            //删除由PDA录入的老摘要数据
            if (!string.IsNullOrEmpty(document.RecordID))
            {
                shRet = DocumentService.Instance.DeleteSummaryData(document.RecordID);
                if (shRet != SystemConst.ReturnValue.OK)
                    MessageBoxEx.Show("移除老摘要数据失败！");
            }

            //将文档状态由新建切换为编辑
            document.HandleDocumentEdit(SystemContext.Instance.LoginUser);

            //循环保存当前病历所有子文档
            foreach (DocumentInfo child in document.Childs)
            {
                if (!this.CommitQCDocument(child))
                    return false;
                DocumentService.Instance.SaveChildDocInfo(document.DocSetID, child.Caller, child.DocSetID);
            }
            return true;
        }
        
        /// <summary>
        /// 将当前病历信息保存到内存
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool SaveQCDocument()
        {
            if (this.m_document == null)
            {
                MessageBoxEx.Show("无法保存病历文档,文档信息对象非法!");
                return false;
            }

            byte[] byteDocData = null;
            this.Save(ref byteDocData);
            if (byteDocData == null || byteDocData.Length <= 0)
            {
                MessageBoxEx.Show("无法保存病历文档,文档数据生成失败!");
                return false;
            }
            this.m_document.DocData = byteDocData;
            this.m_document.QCSummaryData.Clear();
            this.m_document.QCSummaryData.AddRange(this.GetQCSummaryData());
            return true;
        }

        /// <summary>
        /// 获取当前表单编辑窗口中的摘要数据
        /// </summary>
        /// <returns>摘要数据列表</returns>
        private List<QCSummaryData> GetQCSummaryData()
        {
            List<QCSummaryData> lstQCSummaryData = new List<QCSummaryData>();
            if (this.m_document == null)
                return lstQCSummaryData;

            List<KeyData> keyDataList = this.GetKeyDataList();
            if (keyDataList == null)
                return lstQCSummaryData;

            IEnumerator<KeyData> enumerator = keyDataList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                QCSummaryData qcSummaryData = new QCSummaryData();
                qcSummaryData.DocID = this.m_document.DocSetID;
                qcSummaryData.DocTypeID = this.m_document.DocTypeID;
                qcSummaryData.RecordID = this.m_document.RecordID;
                qcSummaryData.WardCode = this.m_document.WardCode;

                qcSummaryData.DataName = enumerator.Current.Name;
                qcSummaryData.DataCode = enumerator.Current.Code;
                qcSummaryData.DataType = enumerator.Current.Type;
                qcSummaryData.DataUnit = enumerator.Current.Unit;
                if (enumerator.Current.RecordTime == null)
                    qcSummaryData.DataTime = this.m_document.RecordTime;
                else
                    qcSummaryData.DataTime = enumerator.Current.RecordTime.Value.AddSeconds(-enumerator.Current.RecordTime.Value.Second);
                if (enumerator.Current.Value != null)
                    qcSummaryData.DataValue = enumerator.Current.Value.ToString();

                qcSummaryData.Category = enumerator.Current.Category;
                qcSummaryData.ContainsTime = enumerator.Current.ContainsTime;
                qcSummaryData.Remarks = enumerator.Current.Remarks;
                lstQCSummaryData.Add(qcSummaryData);
            }
            return lstQCSummaryData;
        }
        #endregion

        #region"文档打印"
        /// <summary>
        /// 预览当前表单
        /// </summary>
        /// <returns>是否执行成功</returns>
        public bool PreviewDocument()
        {
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData == null)
                return false;
            Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
            explorerForm.ReportFileData = byteReportData;
            explorerForm.ReportParamData.Add("打印数据", this.ExportXml(true));
            if (explorerForm.ShowDialog() == DialogResult.OK)
                return true;
            return false;
        }

        /// <summary>
        /// 打印当前表单
        /// </summary>
        /// <returns>是否执行成功</returns>
        public bool PrintDocument()
        {
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData == null)
                return false;
            Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
            explorerForm.AutoStartPrint = true;
            explorerForm.ReportFileData = byteReportData;
            explorerForm.ReportParamData.Add("打印数据", this.ExportXml(true));
            if (explorerForm.ShowDialog() == DialogResult.OK)
                return true;
            return false;
        }

        /// <summary>
        /// 加载打印报表模板
        /// </summary>
        /// <param name="szReportName">加载打印报表模板名</param>
        /// <returns>加载打印报表模板byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            this.Update();
            DocTypeInfo docTypeInfo = null;
            if (this.m_document != null)
                docTypeInfo = FormCache.Instance.GetDocTypeInfo(this.m_document.DocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("无法获取文档类型信息!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_DOC_FORM;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = docTypeInfo.DocTypeName;
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowErrorFormat("{0}打印报表还没有制作!", null, szReportName);
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowErrorFormat("{0}打印报表内容下载失败!", null, szReportName);
                return null;
            }
            return byteTempletData;
        }

        private Heren.Common.Report.ReportExplorerForm GetReportExplorerForm()
        {
            Heren.Common.Report.ReportExplorerForm reportExplorerForm =
                new Heren.Common.Report.ReportExplorerForm();
            reportExplorerForm.WindowState = FormWindowState.Maximized;
            reportExplorerForm.QueryContext +=
                new Heren.Common.Report.QueryContextEventHandler(this.ReportExplorerForm_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new Heren.Common.Report.ExecuteQueryEventHandler(this.ReportExplorerForm_ExecuteQuery);
            reportExplorerForm.NotifyNextReport +=
                new Heren.Common.Report.NotifyNextReportEventHandler(this.ReportExplorerForm_NotifyNextReport);
            return reportExplorerForm;
        }
        #endregion

        /// <summary>
        /// 根据指定的病历类型ID号,以及调用者获取对应子文档信息对象,包括内容
        /// </summary>
        /// <param name="szDocTypeID">子文档类型ID号</param>
        /// <param name="szCaller">子文档调用者</param>
        /// <returns>子文档信息对象</returns>
        private DocumentInfo GetChildDocument(string szDocTypeID, string szCaller)
        {
            if (this.m_document == null)
            {
                MessageBoxEx.Show("无法打开子文档,当前文档信息非法!");
                return null;
            }
            if (szCaller != null)
                szCaller = szCaller.ToLower();

            //从内存缓存中获取文档
            DocumentInfo childDocument = null;
            childDocument = this.m_document.Childs[szDocTypeID, szCaller];
            if (childDocument != null)
                return childDocument;

            //如果当前病历是新建的
            if (this.m_document.DocState == DocumentState.New)
                return this.NewChildDocument(szDocTypeID, szCaller);

            //如果当前打开的是已写病历,那么先获取之前已写的子文档
            string szDocID = this.m_document.DocSetID;
            string szChildID = null;
            short shRet = DocumentService.Instance.GetChildDocID(szDocID, szCaller, ref szChildID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("无法打开子文档,子文档信息查询失败!");
                return null;
            }

            //如果子文档ID号获取到,那么尝试从本地缓存中取
            //如果没有获取到,那么说明之前没有写过,则新建
            if (!GlobalMethods.Misc.IsEmptyString(szChildID))
                childDocument = this.m_document.Childs[szChildID];
            else
                childDocument = this.NewChildDocument(szDocTypeID, szCaller);
            if (childDocument != null)
                return childDocument;

            //如果本地缓存中,没有获取到子文档信息,那么去下载
            //注意这里的文档ID号需要用文档集号,因为文档集号是不变的
            NurDocInfo docInfo = null;
            DocumentService.Instance.GetLatestDocInfo(szChildID, ref docInfo);
            if (docInfo == null)
            {
                StringBuilder sbTipInfo = new StringBuilder();
                sbTipInfo.Append("无法打开子文档,子文档信息查询失败!\r\n");
                sbTipInfo.Append("请点击“确定”按钮,系统将为您重新打开一个!");
                MessageBoxEx.Show(sbTipInfo.ToString());
                return this.NewChildDocument(szDocTypeID, szCaller);
            }

            byte[] byteDocData = null;
            DocumentService.Instance.GetDocByID(docInfo.DocID, ref byteDocData);
            if (byteDocData == null)
            {
                StringBuilder sbTipInfo = new StringBuilder();
                sbTipInfo.Append("无法打开子文档,子文档信息查询失败!\r\n");
                sbTipInfo.Append("请点击“确定”按钮,系统将为您重新打开一个!");
                MessageBoxEx.Show(sbTipInfo.ToString());
                return this.NewChildDocument(szDocTypeID, szCaller);
            }

            childDocument = DocumentInfo.Create(docInfo);
            childDocument.Caller = szCaller;
            childDocument.DocData = byteDocData;
            return childDocument;
        }

        /// <summary>
        /// 创建一个新的子文档信息对象,包括内容
        /// </summary>
        /// <param name="szDocTypeID">子文档类型ID号</param>
        /// <param name="szCaller">子文档调用者</param>
        /// <returns>子文档信息对象</returns>
        private DocumentInfo NewChildDocument(string szDocTypeID, string szCaller)
        {
            PatVisitInfo patVisit = PatientTable.Instance.ActivePatient;
            if (patVisit == null)
            {
                MessageBoxEx.Show("您没有选择任何病人!");
                return null;
            }
            if (this.m_document == null)
            {
                MessageBoxEx.Show("当前病历信息非法!无法创建子病历!");
                return null;
            }

            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowFormat("没有找到ID号“{0}”对应的模板!", szDocTypeID);
                return null;
            }

            byte[] byteDocData = null;
            FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteDocData);
            if (byteDocData == null)
            {
                MessageBoxEx.ShowFormat("模板“{0}”下载失败!", docTypeInfo.DocTypeName);
                return null;
            }
            if (szCaller != null)
                szCaller = szCaller.ToLower();

            string szRecordID = this.m_document.RecordID;
            DocumentInfo childDocument = DocumentInfo.Create(docTypeInfo, patVisit, szRecordID);
            childDocument.Caller = szCaller;
            childDocument.DocData = byteDocData;
            return childDocument;
        }

        /// <summary>
        /// 将脚本中的KeyData对象列表转换为摘要数据对象列表
        /// </summary>
        /// <param name="keyDataList">表单关键数据列表</param>
        /// <returns>摘要数据列表</returns>
        private List<SummaryData> GetSummaryData(List<KeyData> keyDataList)
        {
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            if (this.m_document == null)
                return lstSummaryData;

            if (keyDataList == null)
                return lstSummaryData;

            IEnumerator<KeyData> enumerator = keyDataList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SummaryData summaryData = new SummaryData();
                summaryData.DocTypeID = this.m_document.DocTypeID;
                summaryData.DataName = enumerator.Current.Name;
                summaryData.DataCode = enumerator.Current.Code;
                summaryData.DataType = enumerator.Current.Type;
                summaryData.DataUnit = enumerator.Current.Unit;
                if (enumerator.Current.RecordTime == null)
                    summaryData.DataTime = SysTimeService.Instance.Now;
                else
                    summaryData.DataTime = enumerator.Current.RecordTime.Value;
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
        /// 将包含护理记录数据的DataTable对象保存到护理记录数据表.
        /// 同时将这条护理记录包含的摘要数据列表保存到摘要数据表中
        /// </summary>
        /// <param name="table">护理记录数据的DataTable对象</param>
        /// <param name="keyDataList">护理记录包含的摘要数据</param>
        /// <returns>是否成功</returns>
        private bool SaveNursingRec(DataTable table, List<KeyData> keyDataList)
        {
            if (table == null || table.Rows.Count <= 0)
                return false;

            DataRow row = table.Rows[0];
            NursingRecInfo nursingRecInfo = new NursingRecInfo();

            bool isDelete = false;
            if (!row.IsNull("is_delete"))
                isDelete = GlobalMethods.Convert.StringToValue(row["is_delete"], false);

            if (!row.IsNull("patient_id"))
                nursingRecInfo.PatientID = row["patient_id"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.PatientID = PatientTable.Instance.ActivePatient.PatientId;

            if (!row.IsNull("visit_id"))
                nursingRecInfo.VisitID = row["patient_id"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.VisitID = PatientTable.Instance.ActivePatient.VisitId;

            if (!row.IsNull("sub_id"))
                nursingRecInfo.SubID = row["sub_id"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.SubID = PatientTable.Instance.ActivePatient.SubID;

            if (!row.IsNull("ward_code"))
                nursingRecInfo.WardCode = row["ward_code"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.WardCode = PatientTable.Instance.ActivePatient.WardCode;

            if (!row.IsNull("ward_name"))
                nursingRecInfo.WardName = row["ward_name"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.WardName = PatientTable.Instance.ActivePatient.WardName;

            nursingRecInfo.CreateTime = SysTimeService.Instance.Now;
            if (SystemContext.Instance.LoginUser != null)
                nursingRecInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            if (SystemContext.Instance.LoginUser != null)
                nursingRecInfo.CreatorName = SystemContext.Instance.LoginUser.Name;

            if (!row.IsNull("record_content"))
                nursingRecInfo.RecordContent = row["record_content"] as string;
            if (!row.IsNull("record_remarks"))
                nursingRecInfo.RecordRemarks = row["record_remarks"] as string;
            if (!row.IsNull("summary_flag"))
            {
                nursingRecInfo.SummaryFlag =
                    GlobalMethods.Convert.StringToValue(row["summary_flag"], 0);
            }
            if (!row.IsNull("summary_name"))
                nursingRecInfo.SummaryName = row["summary_name"] as string;
            if (!row.IsNull("summary_start_time"))
            {
                nursingRecInfo.SummaryStartTime =
                    GlobalMethods.Convert.StringToValue(row["summary_start_time"], DateTime.Now);
            }

            if (!row.IsNull("record_time"))
            {
                nursingRecInfo.RecordTime =
                    GlobalMethods.Convert.StringToValue(row["record_time"], DateTime.Now);
            }

            DateTime dtRecordTime = nursingRecInfo.RecordTime;
            if (!row.IsNull("record_time_old"))
            {
                dtRecordTime =
                    GlobalMethods.Convert.StringToValue(row["record_time_old"], DateTime.Now);
            }
            nursingRecInfo.RecordID = nursingRecInfo.MakeRecordID();

            short shRet = SystemConst.ReturnValue.OK;
            if (isDelete)
            {
                string szPatientID = nursingRecInfo.PatientID;
                string szVisitID = nursingRecInfo.VisitID;
                string szSubID = nursingRecInfo.SubID;
                string szModifierID = nursingRecInfo.ModifierID;
                string szModifierName = nursingRecInfo.ModifierName;
                shRet = NurRecService.Instance.DeleteNursingRec(szPatientID, szVisitID, szSubID
                    , dtRecordTime, szModifierID, szModifierName);
            }
            else
            {
                List<SummaryData> summaryDataList = this.GetSummaryData(keyDataList);
                shRet = NurRecService.Instance.SaveNursingRec(dtRecordTime, ref nursingRecInfo, summaryDataList);
            }
            return shRet == SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取当前护理病历文档有关的系统上下文数据
        /// </summary>
        /// <param name="name">上下文名称</param>
        /// <param name="value">上下文数据</param>
        /// <returns>是否成功</returns>
        private bool GetSystemContext(string name, ref object value)
        {
            if (name == "创建人")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.CreatorName;
                return true;
            }
            if (name == "创建时间")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.DocTime;
                return true;
            }
            if (name == "修改人")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.ModifierName;
                return true;
            }
            if (name == "修改时间")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.ModifyTime;
                return true;
            }
            if (name == "记录时间" || name == "评估时间")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.RecordTime.AddSeconds(-this.m_document.RecordTime.Second);
                return true;
            }
            if (name == "文档ID" || name == "文档编号")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.DocID;
                return true;
            }
            if (name == "文档类型" || name == "文档类型编号")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.DocTypeID;
                return true;
            }
            if (name == "文档集ID" || name == "文档集编号")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.DocSetID;
                return true;
            }
            if (name == "护理记录ID" || name == "护理记录编号")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.RecordID;
                return true;
            }
            if (name == "表单新建权限")
            {
                if (this.m_document == null)
                    return false;
                value = RightController.Instance.CanCreateNurDoc();
                return true;
            }
            if (name == "表单编辑权限")
            {
                if (this.m_document == null)
                    return false;
                NurDocInfo docinfo = new NurDocInfo();
                DocumentService.Instance.GetDocInfo(this.m_document.DocID, ref docinfo);
                value = RightController.Instance.CanEditNurDoc(docinfo);
                return true;
            }
            if (name == "体征编辑权限")
            {
                if (this.m_document == null)
                    return false;
                value = RightController.Instance.CanEditVitalSigns();
                return true;
            }
            if (name == "表单类型代码")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.DocTypeID;
                return true;
            }
            if (name == "表单类型名称")
            {
                if (this.m_document == null)
                    return false;
                value = this.m_document.DocTitle;
                return true;
            }
            if (name == "病历状态")
            {
                if (this.m_document == null)
                    return false;
                if (this.m_document.DocState == DocumentState.New)
                    value = "新建";
                else if (this.m_document.DocState == DocumentState.Edit)
                    value = "编辑";
                else if (this.m_document.DocState == DocumentState.Revise)
                    value = "修订";
                else if (this.m_document.DocState == DocumentState.View)
                    value = "只读";
                return true;
            }
            if (name == "申请信息")
            {
                if (this.m_document == null)
                    return false;
                NurApplyStatusInfo NurApplyStatusInfo = new NurApplyStatusInfo();
                NurApplyService.Instance.GetNurApplyStatusInfoByDocSetID(m_document.DocSetID, ref NurApplyStatusInfo);
                value = NurApplyStatusInfo.ToDataTable();
                return true;
            }
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        protected override void OnQueryContext(QueryContextEventArgs e)
        {
            base.OnQueryContext(e);
            if (!e.Success)
            {
                object value = e.Value;
                e.Success = this.GetSystemContext(e.Name, ref value);
                if (e.Success) e.Value = value;
            }
        }

        protected override void OnCustomEvent(object sender, CustomEventArgs e)
        {
            base.OnCustomEvent(sender, e);
            if (e.Name == "导入文本模板")
                e.Result = UtilitiesHandler.Instance.ShowTextTempletForm();
            else if (e.Name == "导入医嘱记录")
                e.Result = UtilitiesHandler.Instance.ShowOrdersImportForm(e.Data == null ? string.Empty : e.Data.ToString());
            else if (e.Name == "导入检查记录")
                e.Result = UtilitiesHandler.Instance.ShowExamImportForm();
            else if (e.Name == "导入检验记录")
                e.Result = UtilitiesHandler.Instance.ShowTestImportForm();
            else if (e.Name == "导入护理记录")
                e.Result = UtilitiesHandler.Instance.ShowNurRecImportForm();
            else if (e.Name == "导入交班记录")
                e.Result = UtilitiesHandler.Instance.ImportCurrentShift(e.Data.ToString());
            else if (e.Name == "科室选择对话框")
            {
                bool multiSelected = false;
                if (e.Param != null)
                    multiSelected = GlobalMethods.Convert.StringToValue(e.Param.ToString(), false);
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = new DataTable();
                e.Result = UtilitiesHandler.Instance.ShowDeptSelectDialog(0, multiSelected, table);
            }
            else if (e.Name == "病区选择对话框")
            {
                bool multiSelected = false;
                if (e.Param != null)
                    multiSelected = GlobalMethods.Convert.StringToValue(e.Param.ToString(), false);
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = new DataTable();
                e.Result = UtilitiesHandler.Instance.ShowDeptSelectDialog(2, multiSelected, table);
            }
            else if (e.Name == "用户选择对话框")
            {
                bool multiSelected = false;
                if (e.Param != null)
                    multiSelected = GlobalMethods.Convert.StringToValue(e.Param.ToString(), false);
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = new DataTable();
                e.Result = UtilitiesHandler.Instance.ShowUserSelectDialog(multiSelected, table);
            }
            else if (e.Name == "导入产前记录")
            {
                string szDocTypeID = e.Param.ToString();
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = new DataTable();
                e.Result = UtilitiesHandler.Instance.ShowAntenatalDialog(szDocTypeID, table);
            }
            else if (e.Name == "保存护理记录")
            {
                e.Result = this.SaveNursingRec(e.Param as DataTable, e.Data as List<KeyData>);
            }
            else if (e.Name == "获取报表数据")
            {
                if (e.Param == null)
                    return;
                e.Result = this.GetReportFileData(e.Param.ToString());
            }
            else if (e.Name == "续打表单")
            {
                string szReportName = null;
                if (e.Data != null)
                    szReportName = e.Data.ToString();

                byte[] byteReportData = this.GetReportFileData(szReportName);
                if (byteReportData == null)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", true);
                explorerForm.ReportParamData.Add("续打行号", e.Param);
                explorerForm.ReportParamData.Add("打印数据", this.ExportXml(true));
                explorerForm.ShowDialog();
            }
            else if (e.Name == "打印" || e.Name == "打印表单")
            {
                string szReportName = null;
                if (e.Data != null)
                    szReportName = e.Data.ToString();

                byte[] byteReportData = this.GetReportFileData(szReportName);
                if (byteReportData == null)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", e.Param);
                explorerForm.ReportParamData.Add("打印数据", this.ExportXml(true));
                explorerForm.ShowDialog();
            }
            else if (e.Name == "保存护理申请信息")
            {
                NurApplyHandler.Instance.HandleNurApplySave(this.m_document, true, e.Data);
            }
            else if (e.Name == "更新护理申请信息")
            {
                NurApplyHandler.Instance.HandleNurApplySave(this.m_document, false, e.Data);
            }
            else if (e.Name == "刷新病人信息")
            {
                if (this.IsDisposed || !this.IsHandleCreated)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.Update();
                PatientTable.Instance.ReloadPatientInfoFromServer();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            else if (e.Name == "刷新数据")
            {
                if (this.IsDisposed || !this.IsHandleCreated)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                SystemContext.Instance.OnDataChanged(this, e);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            else if (e.Name == "导入食物含水量")
                e.Result = UtilitiesHandler.Instance.ShowFoodEleImportForm();
        }

        protected override void OnRequestChildForm(RequestChildFormEventArgs e)
        {
            base.OnRequestChildForm(e);
            if (e.FormInfo == null)
            {
                MessageBoxEx.ShowError("后台模板编写有误,请求的表单信息不能为空!");
                return;
            }

            //参数e.FormInfo.ID由脚本中传出,表示需要调用的子表单的类型ID号
            //参数e.FormInfo.Caller由脚本中传出,表示当前表单中是哪个控件发出的请求
            DocumentInfo childDocument = this.GetChildDocument(e.FormInfo.ID, e.FormInfo.Caller);
            if (childDocument == null)
                return;

            ChildDocEditForm form = new ChildDocEditForm(childDocument);

            //参数e.Data由脚本中传出,用来传给子表单,让子表单初始显示一些数据
            form.FormEditor.UpdateFormData("表单数据", e.Data);
            if (form.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                //参数e.Data经子表单处理后,再返回给当前表单,当前表单再更新自己的显示
                e.Cancel = false;
                e.Data = form.FormEditor.GetFormData("表单数据");

                //参数e.FormInfo.Persist由脚本传出,表示子表单是否需要持久化保存到数据库
                if (e.FormInfo.Persist) this.m_document.Childs.Add(form.Document);
            }
        }

        protected override void OnExecuteUpdate(ExecuteUpdateEventArgs e)
        {
            base.OnExecuteUpdate(e);
            if (CommonService.Instance.ExecuteUpdate(e.IsProc, e.SQL) == SystemConst.ReturnValue.OK)
                e.Success = true;
        }

        protected override void OnExecuteQuery(ExecuteQueryEventArgs e)
        {
            base.OnExecuteQuery(e);
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SqlInfo, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DocumentControl
            // 
            this.ResumeLayout(false);
        }
    }
}
