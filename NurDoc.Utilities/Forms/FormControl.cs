// **************************************************************
// ������Ӳ���ϵͳ֮�ײ���ؼ���װ��,��Ҫ����һЩ����ҵ���߼�
// Creator:YangMingkun  Date:2012-9-28
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using System.ComponentModel;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Forms.Editor;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Utilities.Forms
{
    public class FormControl : FormEditor
    {
        private DocTypeInfo m_docTypeInfo = null;

        /// <summary>
        /// ��ȡ�����õ�ǰ�ĵ��б��ڹ������ĵ����Ͷ���
        /// </summary>
        [Browsable(false)]
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
        }

        /// <summary>
        /// ��ָ���ı�ģ��
        /// </summary>
        /// <param name="docTypeInfo">����Ϣ</param>
        /// <returns>�Ƿ�򿪳ɹ�</returns>
        public bool Load(DocTypeInfo docTypeInfo, byte[] byteFormData)
        {
            this.m_docTypeInfo = docTypeInfo;
            return base.Load(byteFormData);
        }

        /// <summary>
        /// ��ָ���ı�ģ��
        /// </summary>
        /// <param name="docTypeInfo">����Ϣ</param>
        /// <returns>�Ƿ�򿪳ɹ�</returns>
        public bool OpenForm(DocTypeInfo docTypeInfo)
        {
            this.m_docTypeInfo = docTypeInfo;
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("����ʧ��,����ϢΪ��!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowErrorFormat("{0}ģ����������ʧ��!", null, docTypeInfo.DocTypeName);
                return false;
            }
            return this.Load(byteFormData);
        }

        /// <summary>
        /// ���浱ǰ������
        /// </summary>
        /// <returns>�Ƿ񱣴�ɹ�</returns>
        public bool SaveForm()
        {
            object param = string.Empty;
            return this.SaveFormData(ref param);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                if (this.m_docTypeInfo == null)
                    return false;
                string szDocTypeID = this.m_docTypeInfo.DocTypeID;
                if (string.IsNullOrEmpty(szDocTypeID))
                    return true;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                //���²�ѯ��ȡ�ĵ�������Ϣ
                DocTypeInfo docTypeInfo = null;
                short shRet = DocTypeService.Instance.GetDocTypeInfo(szDocTypeID, ref docTypeInfo);
                // ���������������İ汾��ͬ,���������¼���
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
                    MessageBoxEx.Show("ˢ��ʧ��");
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return true;
                }
                byte[] byteDocData = null;
                this.Save(ref byteDocData);
                result = this.Load(byteDocData, byteTempletData);
                if (!result)
                {
                    MessageBoxEx.Show("ˢ��ʧ��");
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return true;
                }
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// ���ű��е�KeyData�����б�ת��ΪժҪ���ݶ����б�
        /// </summary>
        /// <param name="keyDataList">���ؼ������б�</param>
        /// <returns>ժҪ�����б�</returns>
        private List<SummaryData> GetSummaryData(List<KeyData> keyDataList)
        {
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            if (this.m_docTypeInfo == null)
                return lstSummaryData;

            if (keyDataList == null)
                return lstSummaryData;

            IEnumerator<KeyData> enumerator = keyDataList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SummaryData summaryData = new SummaryData();
                summaryData.DocTypeID = this.m_docTypeInfo.DocTypeID;
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
        /// �����������¼���ݵ�DataTable���󱣴浽�����¼���ݱ�.
        /// ͬʱ�����������¼������ժҪ�����б��浽ժҪ���ݱ���
        /// </summary>
        /// <param name="table">�����¼���ݵ�DataTable����</param>
        /// <param name="keyDataList">�����¼������ժҪ����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool SaveNursingRec(DataTable table, List<KeyData> keyDataList)
        {
            if (table == null || table.Rows.Count <= 0)
                return false;

            DataRow row = table.Rows[0];
            NursingRecInfo nursingRecInfo = new NursingRecInfo();

            bool isDelete = false;
            if (!row.IsNull("is_delete"))
                isDelete = GlobalMethods.Convert.StringToValue(row["is_delete"], false);

            if (!row.IsNull("record_id"))
                nursingRecInfo.RecordID = row["record_id"] as string;
            if (!row.IsNull("patient_id"))
                nursingRecInfo.PatientID = row["patient_id"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.PatientID = PatientTable.Instance.ActivePatient.PatientID;
            if (!row.IsNull("patient_name"))
                nursingRecInfo.PatientName = row["patient_name"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.PatientName = PatientTable.Instance.ActivePatient.PatientName;
            if (!row.IsNull("visit_id"))
                nursingRecInfo.VisitID = row["visit_id"] as string;
            else if (PatientTable.Instance.ActivePatient != null)
                nursingRecInfo.VisitID = PatientTable.Instance.ActivePatient.VisitID;

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
            if (SystemContext.Instance.LoginUser != null)
                nursingRecInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
            if (SystemContext.Instance.LoginUser != null)
                nursingRecInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
            if (!row.IsNull("recorder1_id"))
                nursingRecInfo.Recorder1ID = row["recorder1_id"] as string;
            if (!row.IsNull("recorder1_name"))
                nursingRecInfo.Recorder1Name = row["recorder1_name"] as string;
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
            if (!row.IsNull("record_data"))
            {
                nursingRecInfo.RecordDate = GlobalMethods.Convert.StringToValue(row["record_data"], DateTime.Now);
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
            if (string.IsNullOrEmpty(nursingRecInfo.RecordID))
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

        #region"����ӡ"
        /// <summary>
        /// ��ӡ��ǰ��
        /// </summary>
        public void PrintForm(bool bIsContinuedPrint)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
            explorerForm.ReportFileData = byteReportData;
            explorerForm.ReportParamData.Add("�Ƿ�����", bIsContinuedPrint);
            explorerForm.ReportParamData.Add("��ӡ����", this.ExportXml(true));
            explorerForm.ShowDialog();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// ��ȡ��ӡ����ģ������
        /// </summary>
        /// <param name="szReportName">��ӡ����ģ������</param>
        /// <returns>byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            this.Update();
            if (this.m_docTypeInfo == null)
            {
                MessageBoxEx.ShowError("�޷���ȡ�ĵ�������Ϣ!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_DOC_FORM;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.m_docTypeInfo.DocTypeName;
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowErrorFormat("{0}��ӡ����û������!", null, szReportName);
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowErrorFormat("{0}��ӡ������������ʧ��!", null, szReportName);
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

        protected override void OnQueryContext(QueryContextEventArgs e)
        {
            base.OnQueryContext(e);
            if (e.Success)
                return;
            if (e.Name == "�����ʹ���")
            {
                if (this.m_docTypeInfo != null)
                    e.Value = this.m_docTypeInfo.DocTypeID;
                e.Success = true;
                return;
            }
            if (e.Name == "����������")
            {
                if (this.m_docTypeInfo != null)
                    e.Value = this.m_docTypeInfo.DocTypeName;
                e.Success = true;
                return;
            }
            object value = e.Value;
            e.Success = SystemContext.Instance.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }

        protected override void OnCustomEvent(object sender, CustomEventArgs e)
        {
            base.OnCustomEvent(sender, e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (e.Name == "�����ı�ģ��")
            {
                e.Result = UtilitiesHandler.Instance.ShowTextTempletForm();
            }
            else if (e.Name == "����ҽ����¼")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowOrdersImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "�������¼")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowExamImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "��������¼")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowTestImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "���뻤���¼")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowNurRecImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "���뻤���¼Ĭ������")
            {
                string szPidVid = e.Param == null ? string.Empty : e.Param.ToString();
                if (string.IsNullOrEmpty(szPidVid))
                    return;
                string[] arrPidVid = szPidVid.Split(',');

                string szSearchDate = e.Data == null ? string.Empty : e.Data.ToString();
                string[] arrSearchDate = szSearchDate.Split(',');
                DateTime dtFrom = DateTime.MinValue;
                DateTime dtTo = DateTime.MinValue;
                if (string.IsNullOrEmpty(szSearchDate)
                    || arrSearchDate.Length <= 1
                    || arrPidVid.Length <= 1
                    || !DateTime.TryParse(arrSearchDate[0], out dtFrom)
                    || !DateTime.TryParse(arrSearchDate[1], out dtTo)
                    || dtFrom == DateTime.MinValue
                    || dtTo == DateTime.MinValue
                    || dtFrom > dtTo)
                {
                    e.Result = UtilitiesHandler.Instance.ShowNurRecImportForm(arrPidVid[0], arrPidVid[1]);
                    return;
                }
                e.Result = UtilitiesHandler.Instance.ShowNurRecImportForm(arrPidVid[0], arrPidVid[1], dtFrom, dtTo);
            }
            else if (e.Name == "����ѡ��Ի���")
            {
                bool multiSelected = false;
                if (e.Param != null)
                    multiSelected = GlobalMethods.Convert.StringToValue(e.Param.ToString(), false);
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = new DataTable();
                e.Result = UtilitiesHandler.Instance.ShowDeptSelectDialog(0, multiSelected, table);
            }
            else if (e.Name == "����ѡ��Ի���")
            {
                bool multiSelected = false;
                if (e.Param != null)
                    multiSelected = GlobalMethods.Convert.StringToValue(e.Param.ToString(), false);
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = new DataTable();
                e.Result = UtilitiesHandler.Instance.ShowDeptSelectDialog(2, multiSelected, table);
            }
            else if (e.Name == "�û�ѡ��Ի���")
            {
                bool multiSelected = false;
                if (e.Param != null)
                    multiSelected = GlobalMethods.Convert.StringToValue(e.Param.ToString(), false);
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = new DataTable();
                e.Result = UtilitiesHandler.Instance.ShowUserSelectDialog(multiSelected, table);
            }
            else if (e.Name == "���滤���¼")
            {
                e.Result = this.SaveNursingRec(e.Param as DataTable, e.Data as List<KeyData>);
            }
            else if (e.Name == "��ȡ��������")
            {
                if (e.Param == null)
                    return;
                e.Result = this.GetReportFileData(e.Param.ToString());
            }
            else if (e.Name == "�����")
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
                explorerForm.ReportParamData.Add("�Ƿ�����", true);
                explorerForm.ReportParamData.Add("�����к�", e.Param);
                explorerForm.ReportParamData.Add("��ӡ����", this.ExportXml(true));
                explorerForm.ShowDialog();
            }
            else if (e.Name == "��ӡ" || e.Name == "��ӡ��")
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
                explorerForm.ReportParamData.Add("�Ƿ�����", e.Param);
                explorerForm.ReportParamData.Add("��ӡ����", this.ExportXml(true));
                explorerForm.ShowDialog();
            }
            else if (e.Name == "ģ���ڴ�ӡ")
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
                explorerForm.ReportParamData.Add("�Ƿ�����", true);
                explorerForm.ReportParamData.Add("��ӡ����", e.Param);
                explorerForm.ShowDialog();
            }
            else if (e.Name == "����ĵ���Ϣ")
            {
                if (e.Data == null)
                    return;
                string szDocId = e.Data.ToString();
                NurDocInfo nurdocInfo = null;
                short shRet = DocumentService.Instance.GetDocInfo(szDocId, ref nurdocInfo);
                if (shRet == SystemConst.ReturnValue.OK && nurdocInfo != null)
                {
                    DocTypeInfo docTypeInfo = null;
                    shRet = DocTypeService.Instance.GetDocTypeInfo(nurdocInfo.DocTypeID, ref docTypeInfo);
                    if (shRet == SystemConst.ReturnValue.OK && docTypeInfo != null)
                    {
                        byte[] data = null;
                        shRet = DocumentService.Instance.GetDocByID(szDocId, ref data);
                        if (shRet == SystemConst.ReturnValue.OK)
                        {
                            DocumentReaderForm readerform = new DocumentReaderForm(docTypeInfo,data);
                            readerform.ShowDialog();
                        }
                    }
                }
            }
            else if (e.Name == "����ʳ�ﺬˮ��")
            {
                e.Result = UtilitiesHandler.Instance.ShowFoodEleImportForm();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnRequestChildForm(RequestChildFormEventArgs e)
        {
            base.OnRequestChildForm(e);
            if (e.FormInfo == null)
            {
                MessageBoxEx.ShowError("��̨ģ���д����,����ı���Ϣ����Ϊ��!");
                return;
            }

            //����e.FormInfo.ID�ɽű��д���,��ʾ��Ҫ���õ��ӱ�������ID��
            //����e.FormInfo.Caller�ɽű��д���,��ʾ��ǰ�������ĸ��ؼ�����������
            string szDocTypeID = e.FormInfo.ID;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowErrorFormat("û���ҵ�ID�š�{0}����Ӧ��ģ��!", null, szDocTypeID);
                return;
            }
            FormEditForm form = new FormEditForm(docTypeInfo);

            //����e.Data�ɽű��д���,���������ӱ�,���ӱ���ʼ��ʾһЩ����
            form.FormEditor.UpdateFormData("������", e.Data);
            if (form.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                //����e.Data���ӱ������,�ٷ��ظ���ǰ��,��ǰ���ٸ����Լ�����ʾ
                e.Cancel = false;
                e.Data = form.FormEditor.GetFormData("������");
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
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = SystemContext.Instance.GetSystemContext(e.Name, ref value);
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
    }
}
