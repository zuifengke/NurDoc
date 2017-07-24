// ***********************************************************
// 护理电子病历系统,病人选项卡主窗口控件.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.PatPage.Document;
using Heren.NurDoc.PatPage.NCP;
using Heren.NurDoc.PatPage.NurRec;
using Heren.NurDoc.PatPage.Consult;
using Heren.NurDoc.PatPage.NurRecTableInput;
using Heren.NurDoc.PatPage.WordDocument;

namespace Heren.NurDoc.PatPage
{
    public partial class PatientPageControl : UserControl
    {
        private PatVisitInfo m_patientVisit = null;

        /// <summary>
        /// 获取当前窗口的病人就诊信息
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
        public bool IsModified
        {
            get
            {
                foreach (IDockContent content in this.dockPanel1.Documents)
                {
                    DockContentBase contentBase = content as DockContentBase;
                    if (contentBase != null && contentBase.IsModified)
                        return true;
                }
                return false;
            }
        }

        #region"事件"
        /// <summary>
        /// 当活动的文档窗口改变活动状态时触发
        /// </summary>
        [Description("当活动的文档窗口改变活动状态时触发")]
        public event EventHandler ActiveDocumentChanged;

        internal virtual void OnActiveDocumentChanged(EventArgs e)
        {
            if (this.ActiveDocumentChanged == null)
                return;
            try
            {
                this.ActiveDocumentChanged(this, e);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("MedDocCtrl.OnActiveDocumentChanged", ex);
            }
        }

        /// <summary>
        /// 当活动的停靠窗口改变活动状态时触发
        /// </summary>
        [Description("当活动的停靠窗口改变活动状态时触发")]
        public event EventHandler ActiveContentChanged;

        internal virtual void OnActiveContentChanged(EventArgs e)
        {
            if (this.ActiveContentChanged == null)
                return;
            try
            {
                this.ActiveContentChanged(this, e);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("MedDocCtrl.OnActiveContentChanged", ex);
            }
        }

        /// <summary>
        /// 病人就诊等系统基本信息改变前触发
        /// </summary>
        [Description("病人就诊等系统基本信息改变前触发")]
        public event PatientInfoChangingEventHandler PatientInfoChanging;

        internal virtual void OnPatientInfoChanging(PatientInfoChangingEventArgs e)
        {
            if (this.PatientInfoChanging == null)
                return;
            try
            {
                this.PatientInfoChanging(this, e);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("PatientPageControl.OnPatientInfoChanging", ex);
            }
        }

        /// <summary>
        /// 病人就诊等系统基本信息改变时触发
        /// </summary>
        [Description("病人就诊等系统基本信息改变时触发")]
        public event EventHandler PatientInfoChanged;

        internal virtual void OnPatientInfoChanged(EventArgs e)
        {
            if (this.PatientInfoChanged == null)
                return;
            try
            {
                this.PatientInfoChanged(this, e);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("PatientPageControl.OnPatientInfoChanged", ex);
            }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeComponent();
            this.UpdateBounds();
            this.dockPanel1.ShowDocumentSubhead = false;
            this.patientInfoStrip1.PatientPageControl = this;
            this.patientInfoStrip1.Visible = true;
        }

        public void SetPatientInfoStripVisible(bool bvisible)
        {
            this.patientInfoStrip1.Visible = bvisible;
        }

        /// <summary>
        /// 切换当前病人窗口中显示的活动病人
        /// </summary>
        /// <param name="patVisit">新的病人信息</param>
        /// <returns>是否成功</returns>
        public bool SwitchPatient(PatVisitInfo patVisit)
        {
            if (patVisit != null
                && patVisit.IsPatVisitSame(this.m_patientVisit))
            {
                return true;
            }
            PatientInfoChangingEventArgs e =
                new PatientInfoChangingEventArgs(this.m_patientVisit, patVisit);
            this.OnPatientInfoChanging(e);
            if (e.Cancel)
                return false;

            if (patVisit == null)
            {
                this.CloseAllContentModules();
            }
            else
            {
                //仅第1次加载病人时加载各子窗口
                if (this.m_patientVisit == null)
                    this.LoadContentModules();
            }

            if (PatientTable.Instance.ActivePatient == this.m_patientVisit)
                PatientTable.Instance.ActivePatient = patVisit;
            this.m_patientVisit = patVisit;
            this.OnPatientInfoChanged(EventArgs.Empty);
            return true;
        }

        #region"加载病人功能模块"
        /// <summary>
        /// 移除所有已打开的窗口
        /// </summary>
        private void CloseAllContentModules()
        {
            while (this.dockPanel1.Contents.Count > 0)
                this.dockPanel1.Contents[0].DockHandler.Close();
        }

        /// <summary>
        /// 加载病人选项卡下的模块列表
        /// </summary>
        private void LoadContentModules()
        {
            //移除所有已打开的窗口
            this.CloseAllContentModules();

            //初始化系统模块
            List<DockContentBase> contents = this.GetSystemModule();

            //初始化由系统提供的带有文档列表模式的病人功能模块插件
            contents.AddRange(this.GetDocumentModule());

            //初始化界面完全由医院自定义的个性化的病人功能模块插件
            contents.AddRange(this.GetPluginModule());

            //对模块进行排序并呈现
            contents.Sort(new Comparison<DockContentBase>(this.CompareForm));
            foreach (DockContentBase content in contents)
            {
                content.IsHidden = false;
                content.Show(this.dockPanel1, false);
            }
        }

        /// <summary>
        /// 获取系统默认的病人功能模块
        /// </summary>
        /// <returns>模块窗口列表</returns>
        private List<DockContentBase> GetSystemModule()
        {
            List<DockContentBase> contents = new List<DockContentBase>();

            if (RightController.Instance.UserRight.ShowWordDocumentForm.Value)
                contents.Add(new WordDocForm(this));
            if (RightController.Instance.UserRight.ShowNursingGraphForm.Value)
                contents.Add(new GraphNursingForm(this));
            if (RightController.Instance.UserRight.ShowVitalSignsGraphForm.Value)
                contents.Add(new VitalSignsGraphForm(this));
            if (RightController.Instance.UserRight.ShowOrderRecordForm.Value)
                contents.Add(new OrdersRecordForm(this));
            if (RightController.Instance.UserRight.ShowNursingRecordForm.Value)
                if (SystemContext.Instance.SystemOption.OptionRecTableInput)
                    contents.Add(new NursingRecordTableForm(this));
                else
                    contents.Add(new NursingRecordForm(this));
            if (RightController.Instance.UserRight.ShowNursingAssessmentForm.Value)
                contents.Add(new NursingAssessMentForm(this));
            if (RightController.Instance.UserRight.ShowDocumentListForm1.Value)
                contents.Add(new SpecialNursingForm(this, 1));
            if (RightController.Instance.UserRight.ShowDocumentListForm2.Value)
                contents.Add(new SpecialNursingForm(this, 2));
            if (RightController.Instance.UserRight.ShowDocumentListForm3.Value)
                contents.Add(new SpecialNursingForm(this, 3));
            if (RightController.Instance.UserRight.ShowDocumentListForm4.Value)
                contents.Add(new SpecialNursingForm(this, 4));
            if (RightController.Instance.UserRight.ShowSpecialNursingForm.Value)
                contents.Add(new SpecialNursingForm(this, 0));
            if (RightController.Instance.UserRight.ShowNursingConsultForm.Value)
                contents.Add(new NursingConsultForm(this));
            if (RightController.Instance.UserRight.ShowNurCardPlanForm.Value)
                contents.Add(new NursingCarePlanForm(this));
            if (RightController.Instance.UserRight.ShowIntegrateQueryForm.Value)
                contents.Add(new IntegratedQueryForm(this));
            return contents;
        }

        /// <summary>
        /// 获取由系统提供的带有文档列表模式的病人功能模块插件
        /// </summary>
        /// <returns>模块窗口列表</returns>
        private List<DockContentBase> GetDocumentModule()
        {
            List<DockContentBase> contents = new List<DockContentBase>();

            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_DOCUMENT;
            List<DocTypeInfo> lstDocTypeInfos = FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
            {
                MessageBoxEx.ShowError("病人模块插件列表下载失败!");
            }
            if (lstDocTypeInfos == null)
                lstDocTypeInfos = new List<DocTypeInfo>();
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (!docTypeInfo.IsVisible || !docTypeInfo.IsValid)
                    continue;
                if (docTypeInfo.IsFolder)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                DocumentListForm documentListForm = new DocumentListForm(this);
                documentListForm.DocTypeInfo = docTypeInfo;
                documentListForm.Text = docTypeInfo.DocTypeName;
                documentListForm.Index = docTypeInfo.DocTypeNo;
                contents.Add(documentListForm);
            }
            return contents;
        }

        /// <summary>
        /// 获取界面完全由医院自定义的个性化的病人功能模块插件
        /// </summary>
        /// <returns>模块窗口列表</returns>
        private List<DockContentBase> GetPluginModule()
        {
            List<DockContentBase> contents = new List<DockContentBase>();

            string szApplyEnv = ServerData.DocTypeApplyEnv.PATIENT_MODULE;
            List<DocTypeInfo> lstDocTypeInfos = FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
            {
                MessageBoxEx.ShowError("病人模块插件列表下载失败!");
            }
            if (lstDocTypeInfos == null)
                lstDocTypeInfos = new List<DocTypeInfo>();
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (!docTypeInfo.IsVisible || !docTypeInfo.IsValid)
                    continue;
                if (docTypeInfo.IsFolder)
                    continue;
                PluginDocumentForm pluginDocumentForm = new PluginDocumentForm(this);
                pluginDocumentForm.DocTypeInfo = docTypeInfo;
                pluginDocumentForm.Text = docTypeInfo.DocTypeName;
                pluginDocumentForm.Index = docTypeInfo.DocTypeNo;
                contents.Add(pluginDocumentForm);
            }
            return contents;
        }

        /// <summary>
        /// 对窗口显示顺序进行排序
        /// </summary>
        /// <param name="content1">窗口1</param>
        /// <param name="content2">窗口2</param>
        /// <returns>比较结果</returns>
        private int CompareForm(DockContentBase content1, DockContentBase content2)
        {
            if (content1 == null && content2 != null)
                return -1;
            if (content1 != null && content2 == null)
                return 1;
            if (content1 == null && content2 == null)
                return 0;
            return content1.Index - content2.Index;
        }
        #endregion

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            this.OnActiveContentChanged(e);
        }

        private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            this.OnActiveDocumentChanged(e);
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <returns>是否成功</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的病历类型信息!", szDocTypeID));
                return false;
            }
            foreach (IDockContent content in this.dockPanel1.Documents)
            {
                DockContentBase contentBase = content as DockContentBase;
                if (contentBase != null && !contentBase.IsDisposed && contentBase.LocateToModule(docTypeInfo, szDocID))
                    return true;
            }
            MessageBoxEx.Show(string.Format("无法定位到病历类型“{0}”对应的模块!", docTypeInfo.DocTypeName));
            return false;
        }

        /// <summary>
        /// 根据传入的病历类型信息定位到指定的病历模块
        /// </summary>
        /// <param name="szDocTypeID">病历类型ID号</param>
        /// <param name="szDocID">已写病历ID号</param>
        /// <param name="bshowList">是否隐藏左侧列表</param>
        /// <returns>是否成功</returns>
        public bool LocateToModule(string szDocTypeID, string szDocID,bool bshowList)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的病历类型信息!", szDocTypeID));
                return false;
            }
            foreach (IDockContent content in this.dockPanel1.Documents)
            {
                DockContentBase contentBase = content as DockContentBase;
                if (contentBase != null && !contentBase.IsDisposed && contentBase.LocateToModule(docTypeInfo, szDocID, bshowList))
                    return true;
            }
            MessageBoxEx.Show(string.Format("无法定位到病历类型“{0}”对应的模块!", docTypeInfo.DocTypeName));
            return false;
        }

        /// <summary>
        /// 根据传入的模板ID，打开编辑模板
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID</param>
        /// <param name="szDocID">文档ID</param>
        /// <returns>是否成功</returns>
        public bool OpenRecordEditForm(string szDocTypeID, string szDocID)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.Show(string.Format("没有找到ID号为“{0}”的病历类型信息!", szDocTypeID));
                return false;
            }
            foreach (IDockContent content in this.dockPanel1.Documents)
            {
                DockContentBase contentBase = content as DockContentBase;
                if (contentBase != null && !contentBase.IsDisposed && contentBase.LocateToModule(docTypeInfo, szDocID))
                {
                    if (docTypeInfo.ApplyEnv == ServerData.DocTypeApplyEnv.NURSING_RECORD && docTypeInfo.IsRepeated == true)
                    {
                        NursingRecordForm nursingRecord = contentBase as NursingRecordForm;
                        nursingRecord.AddNewRecord(null, docTypeInfo);
                    }
                    return true;
                }
            }
            MessageBoxEx.Show(string.Format("无法定位到病历类型“{0}”对应的模块!", docTypeInfo.DocTypeName));
            return false;
        }

        /// <summary>
        /// 获取活动文档
        /// </summary>
        public IDockContent ActiveDocument
        {
            get
            {
                return this.dockPanel1.ActiveDocument;
            }
        }
    }
}
