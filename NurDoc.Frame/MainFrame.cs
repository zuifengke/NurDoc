// ***********************************************************
// ������Ӳ���ϵͳ,ϵͳ������.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Heren.Common.DockSuite;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame.Controls;
using Heren.NurDoc.Frame.Dialogs;
using Heren.NurDoc.Frame.DockForms;

namespace Heren.NurDoc.Frame
{
    public partial class MainFrame : HerenForm
    {
        /// <summary>
        /// ��ȡ��ǰ���������ʾ���Ӵ����б�
        /// </summary>
        public IDockContent[] Contents
        {
            get
            {
                int count = this.dockPanel1.Contents.Count;
                IDockContent[] contents = new IDockContent[count];
                this.dockPanel1.Contents.CopyTo(contents, 0);
                return contents;
            }
        }

        /// <summary>
        /// ��ͣ�����ڸı�״̬ʱ����
        /// </summary>
        [Description("��ͣ�����ڸı�״̬ʱ����")]
        public event EventHandler ActiveContentChanged;

        protected virtual void OnActiveContentChanged(EventArgs e)
        {
            if (this.ActiveContentChanged != null)
                this.ActiveContentChanged(this, e);
        }

        /// <summary>
        /// ���ĵ����ڸı�״̬ʱ����
        /// </summary>
        [Description("���ĵ����ڸı�״̬ʱ����")]
        public event EventHandler ActiveDocumentChanged;

        protected virtual void OnActiveDocumentChanged(EventArgs e)
        {
            if (this.ActiveDocumentChanged != null)
                this.ActiveDocumentChanged(this, e);
        }

        public MainFrame()
        {
            this.InitializeComponent();
            this.Text = SystemContext.Instance.SystemName;
            this.nursingHomePage1.MainFrame = this;
            this.statusBarControl1.MainFrame = this;

            //����DockPanel�ĵ�Tab������ɫ
            DockPaneStripSkin skin =
                this.dockPanel1.Skin.DockPaneStripSkin;
            ToolStripSkin toolStripSkin =
                SkinService.Instance.ToolStripSkin;
            skin.DocumentGradient.DockStripGradient.StartColor =
                toolStripSkin.PaneBackgroundGradient.EndColor;
            skin.DocumentGradient.DockStripGradient.EndColor =
                toolStripSkin.PaneBackgroundGradient.EndColor;
            this.dockPanel1.Skin.DockPaneStripSkin = skin;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RestoreWindowState();
            this.SaveWindowState = true;
            this.Icon = Properties.Resources.SysIcon;
            this.statusBarControl1.Initialize();
            this.ShowNursingTaskForm(false);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = this.CheckModifyState(true);
        }

        /// <summary>
        /// ��鵱ǰ�򿪵����д����Ƿ���δ������޸�
        /// </summary>
        /// <param name="bClosePatientForm">�رղ��˴���</param>
        /// <returns>�Ƿ�ȡ��</returns>
        private bool CheckModifyState(bool bClosePatientForm)
        {
            Application.DoEvents();
            IDockContent[] documents = this.dockPanel1.DocumentsToArray();
            foreach (IDockContent content in documents)
            {
                DockContentBase contentBase = content as DockContentBase;
                if (contentBase == null || contentBase.IsDisposed)
                    continue;
                if (contentBase.IsModified)
                    contentBase.DockHandler.Activate();
                if (contentBase.CheckModifyState())
                    return true;
                if (bClosePatientForm && contentBase is PatientPageForm)
                    contentBase.Dispose();
            }
            return false;
        }

        /// <summary>
        /// ��ʾ�༭��ϵͳ���ڶԻ���,չʾϵͳ�汾������Ϣ
        /// </summary>
        private void ShowSystemAboutForm()
        {
            Application.DoEvents();
            string szVersionsFile = string.Format("{0}\\Versions.dat"
                , GlobalMethods.Misc.GetWorkingPath());

            DateTime dtLastWriteTime = DateTime.Now;
            GlobalMethods.IO.GetFileLastModifyTime(szVersionsFile, ref dtLastWriteTime);
            string szLastModifyTime = dtLastWriteTime.ToString("yyyyMMddHHmmss");

            string key = SystemConst.ConfigKey.USER_README_TIME;
            string szCurrModifyTime = SystemContext.Instance.GetConfig(key, string.Empty);
            if (szCurrModifyTime != szLastModifyTime)
            {
                this.Update();
                (new AboutSystemForm()).ShowDialog();
                SystemContext.Instance.WriteConfig(key, szLastModifyTime);
            }
        }

        /// <summary>
        /// ��ȡָ�����͵�����ʾ���Ӵ��ڶ���
        /// </summary>
        /// <param name="contentType">����</param>
        /// <returns>�Ӵ��ڶ���</returns>
        internal IDockContent GetContent(Type contentType)
        {
            foreach (IDockContent content in this.dockPanel1.Contents)
            {
                DockContentBase form = content as DockContentBase;
                if (form != null && form.GetType() == contentType)
                    return content;
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ�����������ʾ���Ӵ��ڶ���
        /// </summary>
        /// <param name="formText">����</param>
        /// <returns>�Ӵ��ڶ���</returns>
        internal IDockContent GetContent(string formText)
        {
            foreach (IDockContent content in this.dockPanel1.Contents)
            {
                DockContentBase form = content as DockContentBase;
                if (form != null && !form.IsDisposed && form.Text == formText)
                    return content;
            }
            return null;
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            this.OnActiveContentChanged(e);
        }

        private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            PatientTable.Instance.ActivePatient = null;
            PatientPageForm patientPageForm =
                this.dockPanel1.ActiveDocument as PatientPageForm;
            if (patientPageForm != null)
                PatientTable.Instance.ActivePatient = patientPageForm.PatVisitInfo;
            this.OnActiveDocumentChanged(e);
        }

        #region"����ȫ�ֹ���ģ��"
        /// <summary>
        /// ����ȫ�ֹ���ģ�鴰���б�
        /// </summary>
        private void LoadContentModules()
        {
            this.nursingHomePage1.SinglePatMod = false;
            this.nursingHomePage1.Initialize();

            while (this.dockPanel1.Contents.Count > 0)
                this.dockPanel1.Contents[0].DockHandler.Close();

            //��ʼ��ϵͳģ��
            List<DockContentBase> contents = this.GetSystemModule();

            //��ʼ�����ģ��
            contents.AddRange(this.GetPluginModule());

            //��ģ��������򲢳���
            contents.Sort(new Comparison<DockContentBase>(this.CompareForm));
            foreach (DockContentBase content in contents)
            {
                content.IsHidden = false;
                content.Show(this.dockPanel1, false);
            }
        }

        /// <summary>
        /// ����ȫ�ֹ���ģ�鴰���б�
        /// </summary>
        private void LoadSinglePatContentModules()
        {
            this.nursingHomePage1.SinglePatMod = true;
            this.nursingHomePage1.Initialize();

            while (this.dockPanel1.Contents.Count > 0)
                this.dockPanel1.Contents[0].DockHandler.Close();
        }

        /// <summary>
        /// ��ȡϵͳĬ�ϵ�ȫ�ֹ���ģ��
        /// </summary>
        /// <returns>ģ�鴰���б�</returns>
        private List<DockContentBase> GetSystemModule()
        {
            List<DockContentBase> contents = new List<DockContentBase>();
            if (RightController.Instance.UserRight.ShowPatientListForm.Value)
                contents.Add(new PatientListForm(this));
            if (RightController.Instance.UserRight.ShowSpecialPatientForm.Value)
                contents.Add(new SpecialPatientForm(this));
            if (RightController.Instance.UserRight.ShowBedViewForm.Value)
                contents.Add(new PatientCardForm(this));
            if (RightController.Instance.UserRight.ShowBatchRecordForm.Value)
                contents.Add(new BatchRecordForm(this));
            if (RightController.Instance.UserRight.ShowNursingTaskForm.Value)
                contents.Add(new NursingTaskForm(this));
            if (RightController.Instance.UserRight.ShowShiftRecordForm.Value)
                contents.Add(new NursingShiftForm(this));
            if (RightController.Instance.UserRight.ShowNursingStatForm.Value)
                contents.Add(new NursingStatForm(this));
            if (RightController.Instance.UserRight.ShowNursingAssessForm.Value)
            {
                contents.Add(new EvaluationForm(this));
                //contents.Add(new QCForm(this));
            }
            if (RightController.Instance.UserRight.ShowRosteringCardForm.Value)
            {
                int i = contents.Count;
                contents.Add(new RosteringCardForm(this));
                contents[i].Index = 500;
            }
            if (RightController.Instance.UserRight.ShowNursingQCForm.Value)
            {
                //��������ҽԺҪ����Ի�
                if (SystemContext.Instance.SystemOption.HospitalName == "������ҽ��ѧ����ҽԺ")
                {
                    contents.Add(new QCPatientListForm(this));
                    contents.Add(new QuestionListForm(this));
                }
                else
                {
                    contents.Add(new NursingQCForm(this));
                }
            }
            return contents;
        }

        /// <summary>
        /// ��ȡҽԺ�Զ�����Ի���ȫ�ֹ���ģ����
        /// </summary>
        /// <returns>ģ�鴰���б�</returns>
        private List<DockContentBase> GetPluginModule()
        {
            List<DockContentBase> contents = new List<DockContentBase>();

            string szApplyEnv = ServerData.DocTypeApplyEnv.GLOBAL_MODULE;
            List<DocTypeInfo> lstDocTypeInfos = FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
            {
                MessageBoxEx.ShowError("ȫ�ֹ���ģ�����б�����ʧ��!");
            }
            if (lstDocTypeInfos == null)
                lstDocTypeInfos = new List<DocTypeInfo>();
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (!docTypeInfo.IsVisible || !docTypeInfo.IsValid)
                    continue;
                if (docTypeInfo.IsFolder)
                    continue;
                //if (RightController.Instance.UserRight.ShowNursingQCForm.Value == true
                //    && docTypeInfo.IsQCVisible != true)
                //    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                if (RightController.Instance.UserRight.ShowNursingQCForm.Value != docTypeInfo.IsQCVisible)
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
        /// �Դ�����ʾ˳���������
        /// </summary>
        /// <param name="content1">����1</param>
        /// <param name="content2">����2</param>
        /// <returns>�ȽϽ��</returns>
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

        /// <summary>
        /// �л���ǰ��¼�û�Ϊ�µ��û�
        /// </summary>
        /// <param name="userInfo">���û���Ϣ</param>
        /// <returns>�Ƿ��л��ɹ�</returns>
        public bool SwitchSystemUser(UserInfo userInfo, bool bSinglePatMod)
        {
            if (userInfo == null || string.IsNullOrEmpty(userInfo.ID))
                return false;
            //����������־�ļ�
            this.DelLogs();

            UserInfo currentUser = null;
            if (SystemContext.Instance.LoginUser != null)
                currentUser = SystemContext.Instance.LoginUser;
            if (currentUser != null && currentUser.Equals(userInfo) && SystemContext.Instance.SinglePatMod == bSinglePatMod)
                return true;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.CheckModifyState(true))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return false;
            }

            SystemContext.Instance.LoginUser = userInfo.Clone() as UserInfo;
            if (!RightController.Instance.UserRight.NurDocSystem.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowError(string.Format("��û��Ȩ�޵�¼{0}!", SystemContext.Instance.SystemName));
                this.Close();
                return false;
            }

            if (currentUser == null)
            {
                int nRet = ExternService.HerenCert.VerifyUser(
                    "���ʻ�����Ӳ������",
                    SystemContext.Instance.SystemOption.HospitalName,
                    SystemContext.Instance.SystemOption.CertCode);
                if (nRet != 0)
                {
                    MessageBoxEx.Show(string.Format("��ǰ{0}δ����Ȩ���!", SystemContext.Instance.SystemName));
                    this.Close();
                    return false;
                }
            }

            this.Show();
            string szDeptName = SystemContext.Instance.LoginUser.DeptName;
            string szUserName = SystemContext.Instance.LoginUser.Name;
            this.Text = string.Format("{2} - {0} {1}", szDeptName, szUserName, SystemContext.Instance.SystemName);
            Application.DoEvents();
            if (bSinglePatMod)
                this.LoadSinglePatContentModules();
            else
                this.LoadContentModules();
            SystemContext.Instance.SinglePatMod = bSinglePatMod;
            Application.DoEvents();

            SystemConfig.Instance.Write(SystemConst.ConfigKey.DEFAULT_LOGIN_USERID, userInfo.ID);
            SystemContext.Instance.OnUserChanged(this, new UserChangedEventArgs(currentUser));
            this.ShowSystemAboutForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        /// <summary>
        /// �����־�ļ�
        /// </summary>
        private void DelLogs()
        {
            string szpath = AppDomain.CurrentDomain.BaseDirectory + "Logs\\TxtLog";
            if (Directory.Exists(szpath))
            {
                DirectoryInfo dir = new DirectoryInfo(szpath);
                FileInfo[] listfile = dir.GetFiles();
                //����־�ļ�����7����ʱ��ʼ���
                if (listfile.Length > 7)
                {
                    foreach (FileInfo file in listfile)
                    {
                        //����7���ڵ���־�ļ�
                        if (file.CreationTime < DateTime.Today.AddDays(-7) && File.Exists(file.FullName))
                        {
                            try
                            {
                                File.Delete(file.FullName);
                            }
                            catch (Exception e)
                            {
                                LogManager.Instance.WriteLog(e.Message);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ�Ѵ򿪵Ĳ��˴����е�һ������ѡ�����
        /// </summary>
        /// <returns>����ѡ�����</returns>
        internal PatientPageForm GetPatientPageForm()
        {
            foreach (IDockContent content in this.dockPanel1.Documents)
            {
                PatientPageForm patientPageForm = content as PatientPageForm;
                if (patientPageForm != null && !patientPageForm.IsDisposed)
                    return patientPageForm;
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ��������Ϣ��Ӧ�Ĳ���ѡ�����
        /// </summary>
        /// <param name="patVisit">������Ϣ</param>
        /// <returns>����ѡ�����</returns>
        internal PatientPageForm GetPatientPageForm(PatVisitInfo patVisit)
        {
            if (patVisit == null)
                return null;
            return this.GetPatientPageForm(patVisit.PatientId, patVisit.VisitId);
        }

        /// <summary>
        /// ��ȡָ��������Ϣ��Ӧ�Ĳ���ѡ�����
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <returns>����ѡ�����</returns>
        internal PatientPageForm GetPatientPageForm(string szPatientID, string szVisitID)
        {
            foreach (IDockContent content in this.dockPanel1.Documents)
            {
                PatientPageForm patientPageForm = content as PatientPageForm;
                if (patientPageForm == null || patientPageForm.IsDisposed)
                    continue;
                if (patientPageForm.PatVisitInfo == null)
                    continue;
                if (patientPageForm.PatVisitInfo.PatientId == szPatientID
                    && patientPageForm.PatVisitInfo.VisitId == szVisitID)
                    return patientPageForm;
            }
            return null;
        }

        /// <summary>
        /// ��ָ��������Ϣ�Ĳ���ѡ�����
        /// </summary>
        /// <param name="param">�����в�����Ϣ�Ķ���</param>
        public void ShowPatientPageForm(object param)
        {
            string szPatientID = null, szVisitID = null;
            string szDocTypeID = null, szDocID = null;
            if (param is string)
            {
                string[] arrParams = param.ToString().Split(';');
                szPatientID = (arrParams.Length > 0) ? arrParams[0] : string.Empty;
                szVisitID = (arrParams.Length > 1) ? arrParams[1] : string.Empty;
                szDocTypeID = (arrParams.Length > 2) ? arrParams[2] : string.Empty;
                szDocID = (arrParams.Length > 3) ? arrParams[3] : string.Empty;
            }
            else if (param is DataTable)
            {
                DataTable table = param as DataTable;
                szPatientID = GlobalMethods.Table.GetFieldValue(table, 0, "patient_id") as string;
                szVisitID = GlobalMethods.Table.GetFieldValue(table, 0, "visit_id") as string;
                szDocTypeID = GlobalMethods.Table.GetFieldValue(table, 0, "doctype_id") as string;
                szDocID = GlobalMethods.Table.GetFieldValue(table, 0, "doc_id") as string;
            }
            if (!string.IsNullOrEmpty(szPatientID) && !string.IsNullOrEmpty(szVisitID))
                this.ShowPatientPageForm(szPatientID, szVisitID, szDocTypeID, szDocID);
        }

        /// <summary>
        /// ��ߴ�ָ��������Ϣ�Ĳ���ѡ�����
        /// </summary>
        /// <param name="param">�����в�����Ϣ�Ķ���</param>
        public void ShowPatientPageFormAtSide(object param, object data)
        {
            string szPatientID = null, szVisitID = null;
            string szDocTypeID = null, szDocID = null;
            string szSide = null, szpercent = null;
            DockAlignment DockAlignment = new DockAlignment();
            double dproportion = 0.5;
            if (param is string)
            {
                string[] arrParams = param.ToString().Split(';');
                szPatientID = (arrParams.Length > 5) ? arrParams[0] : string.Empty;
                szVisitID = (arrParams.Length > 5) ? arrParams[1] : string.Empty;
                szDocTypeID = (arrParams.Length > 5) ? arrParams[2] : string.Empty;
                szDocID = (arrParams.Length > 5) ? arrParams[3] : string.Empty;
                szSide = (arrParams.Length > 5) ? arrParams[4] : string.Empty;
                if (szSide.Trim().ToLower() == "right")
                    DockAlignment = DockAlignment.Right;
                else if (szSide.Trim().ToLower() == "left")
                    DockAlignment = DockAlignment.Left;
                else if (szSide.Trim().ToLower() == "up")
                    DockAlignment = DockAlignment.Top;
                else if (szSide.Trim().ToLower() == "down")
                    DockAlignment = DockAlignment.Bottom;
                szpercent = (arrParams.Length > 5) ? arrParams[5] : string.Empty;
                if (!double.TryParse(szpercent, out dproportion))
                    return;
            }
            else if (param is DataTable)
            {
                DataTable table = param as DataTable;
                szPatientID = GlobalMethods.Table.GetFieldValue(table, 0, "patient_id") as string;
                szVisitID = GlobalMethods.Table.GetFieldValue(table, 0, "visit_id") as string;
                szDocTypeID = GlobalMethods.Table.GetFieldValue(table, 0, "doctype_id") as string;
                szDocID = GlobalMethods.Table.GetFieldValue(table, 0, "doc_id") as string;
                szSide = GlobalMethods.Table.GetFieldValue(table, 0, "side") as string;
                if (szSide.Trim().ToLower() == "right")
                    DockAlignment = DockAlignment.Right;
                else if (szSide.Trim().ToLower() == "left")
                    DockAlignment = DockAlignment.Left;
                else if (szSide.Trim().ToLower() == "up")
                    DockAlignment = DockAlignment.Top;
                else if (szSide.Trim().ToLower() == "down")
                    DockAlignment = DockAlignment.Bottom;
                szpercent = GlobalMethods.Table.GetFieldValue(table, 0, "percent") as string;
                if (!double.TryParse(szpercent, out dproportion))
                    return;
            }
            if (!string.IsNullOrEmpty(szPatientID) && !string.IsNullOrEmpty(szVisitID))
                this.ShowPatientPageForm(szPatientID, szVisitID, szDocTypeID, szDocID, DockAlignment, dproportion);
        }

        /// <summary>
        /// ��ָ�����˵Ĳ���ѡ�����
        /// </summary>
        /// <param name="patVisit">���ﲡ����Ϣ</param>
        public void ShowPatientPageForm(PatVisitInfo patVisit)
        {
            this.ShowPatientPageForm(patVisit, null, null);
        }

        /// <summary>
        /// �򿪴������񴰿�,������������
        /// </summary>
        /// <param name="IsShow">�Ƿ�λ�������б���</param>
        public void ShowNursingTaskForm(bool IsShow)
        {
            foreach (DockContentBase content in this.dockPanel1.Contents)
            {
                if (content is NursingTaskForm)
                {
                    //�����
                    if (!IsShow)
                        this.statusBarControl1.m_btnTaskMessage.Image = Properties.Resources.Mail_gif;
                    else
                        this.statusBarControl1.m_btnTaskMessage.Image = Properties.Resources.Mail;
                    content.Show(this.dockPanel1, IsShow);
                    NursingTaskForm frmNursingTask = content as NursingTaskForm;
                    frmNursingTask.RefreshTaskCount();
                }
            }
        }

        /// <summary>
        /// ��״̬����ʾ��������
        /// </summary>
        /// <param name="count">����</param>
        public void ShowTaskCountMessage(int count)
        {
            this.statusBarControl1.m_btnTaskMessage.Text = string.Format("({0})", count.ToString());
        }

        /// <summary>
        /// ��ָ������ָ����סԺ�Ĳ��˴���
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <returns>�Ƿ�򿪳ɹ�</returns>
        public bool ShowPatientPageForm(string szPatientID, string szVisitID)
        {
            return this.ShowPatientPageForm(szPatientID, szVisitID, null, null);
        }

        /// <summary>
        /// ��ָ�����˾�����Ϣ�Ĳ��˵�ָ���ĵ����ͺ�ָ���ĵ���¼���ĵ��༭����
        /// </summary>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="szDocID">�ĵ���¼ID</param>
        public void ShowPatientPageForm(PatVisitInfo patVisit, string szDocTypeID, string szDocID)
        {
            PatientPageForm patientPageForm = this.GetPatientPageForm(patVisit);
            if (patientPageForm == null || patientPageForm.IsDisposed)
            {
                if (SystemContext.Instance.SystemOption.SinglePatientMode)
                    patientPageForm = this.GetPatientPageForm();
            }
            if (patientPageForm == null || patientPageForm.IsDisposed)
            {
                patientPageForm = new PatientPageForm(this);
                patientPageForm.Show(this.dockPanel1);
            }
            patientPageForm.DockHandler.Activate();
            if (!patientPageForm.SwitchPatient(patVisit))
                return;

            //�򿪲���λ��ָ�����ѱ���ı����½���
            if (!GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                Application.DoEvents();
                patientPageForm.LocateToModule(szDocTypeID, szDocID);
            }
        }

        public void ShowPatientPageForm(string szPatientID, string szVisitID, string szDocTypeID
            , string szDocID, DockAlignment DockAlignment, double dproportion)
        {
            PatVisitInfo patVisit = PatientTable.Instance.GetPatVisit(szPatientID, szVisitID);
            if (patVisit == null)
                PatVisitService.Instance.GetPatVisitInfo(szPatientID, szVisitID, ref patVisit);
            if (patVisit == null)
            {
                MessageBoxEx.Show(string.Format("�޷���λ�����ˣ�PatientID={0}��VisitID={1} ��Ӧ��ģ��!", szPatientID, szVisitID));
                return;
            }
            PatientPageForm patientPageForm = this.GetPatientPageForm(patVisit);
            if (patientPageForm == null || patientPageForm.IsDisposed)
            {
                if (SystemContext.Instance.SystemOption.SinglePatientMode)
                    patientPageForm = this.GetPatientPageForm();
            }
            if (patientPageForm == null || patientPageForm.IsDisposed)
            {
                patientPageForm = new PatientPageForm(this);
                patientPageForm.DockPanel = this.dockPanel1;

                DockPane pane = null;
                IDockContent[] contents = this.dockPanel1.DocumentsToArray();
                for (int index = 0; index < contents.Length; index++)
                {
                    IDockContent content = contents[index];
                    if (content.DockHandler.Pane != contents[0].DockHandler.Pane)
                    {
                        pane = content.DockHandler.Pane;
                        break;
                    }
                }

                if (pane == null)
                    //this.dockPanel1.DockPaneFactory.CreateDockPane(patientPageForm, DockState.Document, true);
                    this.dockPanel1.DockPaneFactory.CreateDockPane(patientPageForm, this.dockPanel1.ActivePane, DockAlignment.Right, dproportion, true);
                else
                    patientPageForm.Show(pane, null);
            }
            patientPageForm.DockHandler.Activate();
            if (!patientPageForm.SwitchPatient(patVisit))
                return;

            //�򿪲���λ��ָ�����ѱ���ı����½���
            if (!GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                Application.DoEvents();
                patientPageForm.LocateToModule(szDocTypeID, szDocID, false);
            }
        }

        /// <summary>
        /// ��ָ������ָ����סԺ�Ĳ��˵�ָ���ĵ����ͺ�ָ���ĵ���¼���ĵ��༭����
        /// </summary>
        /// <param name="szPatientID">����ID</param>
        /// <param name="szVisitID">����ID</param>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="szDocID">�ĵ���¼ID</param>
        /// <returns>�Ƿ�򿪳ɹ�</returns>
        public bool ShowPatientPageForm(string szPatientID, string szVisitID, string szDocTypeID, string szDocID)
        {
            Application.DoEvents();
            if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID))
                return false;

            PatVisitInfo patVisit = PatientTable.Instance.GetPatVisit(szPatientID, szVisitID);
            if (patVisit != null)
            {
                this.ShowPatientPageForm(patVisit, szDocTypeID, szDocID);
                return true;
            }
            short shRet = PatVisitService.Instance.GetPatVisitInfo(szPatientID, szVisitID, ref patVisit);
            
            if (shRet == SystemConst.ReturnValue.FAILED)
                MessageBoxEx.ShowErrorFormat("û���ҵ�������={0},�����={1}�Ĳ���!", null, szPatientID, szVisitID);
            else if (shRet != SystemConst.ReturnValue.OK)
                MessageBoxEx.ShowErrorFormat("����������={0},�����={1}�Ĳ���ʱʧ��!", null, szPatientID, szVisitID);
            if (patVisit != null)
                this.ShowPatientPageForm(patVisit, szDocTypeID, szDocID);
            return (patVisit != null);
        }


        #region"��ʿ�Ű�һ����"

        /// <summary>
        /// ��ָ�������Ļ�ʿ�Ű�ģ�鴰��
        /// </summary>
        /// <param name="szDeptCode">���Ҵ���</param>
        public bool ShowRosteringTemp(string szDeptCode, string formText)
        {
            foreach (IDockContent content in this.dockPanel1.Contents)
            {
                DockContentBase form = content as DockContentBase;
                if (form != null && !form.IsDisposed && form.Text.EndsWith(formText))
                {
                    form.Show();
                    PluginDocumentForm pluginDocumentForm = form as PluginDocumentForm;
                    pluginDocumentForm.IsRostering = true;
                    pluginDocumentForm.DeptCode = szDeptCode;
                    pluginDocumentForm.RefreshView();
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
