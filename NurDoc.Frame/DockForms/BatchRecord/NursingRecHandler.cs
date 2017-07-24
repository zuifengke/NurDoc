// ***********************************************************
// ������Ӳ���ϵͳ,�����¼��������¼��ģ�鴦����.
// Creator:YangMingkun  Date:2013-1-9
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.Frame.DockForms
{
    internal class NursingRecHandler : BatchRecordHandler
    {
        /// <summary>
        /// �����¼�������༭��,������ʱ���а󶨵ı�
        /// </summary>
        private DocumentControl m_formEditor = null;

        public NursingRecHandler(DataTableView dataTableView, GridViewSchema schema)
            : base(dataTableView, schema)
        {
            this.m_formEditor = new DocumentControl();
        }

        /// <summary>
        /// ��Ե�ǰ�����б�,����ָ��ʱ������������
        /// </summary>
        /// <param name="dtRecordTime">ʱ���</param>
        /// <returns>�Ƿ���سɹ�</returns>
        public override bool LoadGridViewData(DateTime dtRecordTime)
        {
            if (this.m_htColumnTable == null || this.m_htColumnTable.Count <= 0)
                return false;
            this.m_dataTableView.Focus();
            Form parentForm = this.m_dataTableView.FindForm();
            if (parentForm == null || parentForm.IsDisposed)
                return false;
            parentForm.Update();
            int nRowsCount = this.m_dataTableView.Rows.Count;
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "���ڼ��ػ����¼���ݣ����Ժ�...");

            foreach (DataTableViewRow row in this.m_dataTableView.Rows)
            {
                WorkProcess.Instance.Show(row.Index + 1, false);

                //�������ʾ����������
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex > 2)
                        cell.Value = null;
                }

                //��ѯ�����µ���������
                PatVisitInfo patVisitInfo = row.Tag as PatVisitInfo;
                if (patVisitInfo == null)
                    continue;
                string szPatientID = patVisitInfo.PatientID;
                string szVisitID = patVisitInfo.VisitID;
                string szSubID = patVisitInfo.SubID;

                DateTime dtBeginTime = dtRecordTime;
                DateTime dtEndTime = dtRecordTime;
                List<SummaryData> lstSummaryData = null;
                short shRet = DocumentService.Instance.GetSummaryData(szPatientID, szVisitID, szSubID
                    , dtBeginTime, dtEndTime, ref lstSummaryData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowErrorFormat("���ˡ�{0}���Ļ����¼��������ʧ��!", null, patVisitInfo.PatientName);
                    return false;
                }
                if (lstSummaryData == null || lstSummaryData.Count <= 0)
                    continue;

                foreach (SummaryData summaryData in lstSummaryData)
                    this.LoadGridViewRowData(row, summaryData);
            }
            WorkProcess.Instance.Close();
            return true;
        }

        /// <summary>
        /// ��ָ����ժҪ���ݶ�����ص�ָ�������
        /// </summary>
        /// <param name="row">ָ�������</param>
        /// <param name="summaryData">ժҪ���ݶ���</param>
        /// <returns>�Ƿ���سɹ�</returns>
        private bool LoadGridViewRowData(DataGridViewRow row, SummaryData summaryData)
        {
            string szRecordData = string.Empty;
            DataGridViewCell cell = null;

            if (SystemContext.Instance.IsBodyTemperatureType(summaryData.DataName)
                && this.m_htColumnTable.Contains("����"))
            {
                ComboBoxTextBoxColumn column = this.m_htColumnTable["����"] as ComboBoxTextBoxColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];
                    szRecordData = SystemContext.Instance.GetVitalSignsName(summaryData.DataName)
                        + column.ValueSeparator + summaryData.DataValue;
                }
            }
            else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("ѪѹLow")
                && this.m_htColumnTable.Contains("Ѫѹ"))
            {
                DataGridViewColumn column = this.m_htColumnTable["Ѫѹ"] as DataGridViewColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];

                    string szCellValue = string.Empty;
                    if (cell.Value != null)
                        szCellValue = cell.Value.ToString().Trim();

                    if (szCellValue == null || !szCellValue.EndsWith("/"))
                        szRecordData = "/" + summaryData.DataValue;
                    else
                        szRecordData = szCellValue + summaryData.DataValue;
                }
            }
            else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("Ѫѹhigh")
                && this.m_htColumnTable.Contains("Ѫѹ"))
            {
                DataGridViewColumn column = this.m_htColumnTable["Ѫѹ"] as DataGridViewColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];

                    string szCellValue = string.Empty;
                    if (row.Cells[column.Index].Value != null)
                        szCellValue = row.Cells[column.Index].Value.ToString().Trim();

                    if (szCellValue == null || !szCellValue.StartsWith("/"))
                        szRecordData = summaryData.DataValue + "/";
                    else
                        szRecordData = summaryData.DataValue + szCellValue;
                }
            }
            else
            {
                string szRecordName = SystemContext.Instance.GetVitalSignsName(summaryData.DataName);
                DataGridViewColumn column = this.m_htColumnTable[szRecordName] as DataGridViewColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];
                    szRecordData = SystemContext.Instance.GetVitalSignsValue(summaryData.DataName
                        , summaryData.DataValue);
                }
            }

            if (cell != null && cell.ColumnIndex > 0 && cell.ColumnIndex < this.m_dataTableView.ColumnCount)
            {
                DataGridViewColumn column = this.m_dataTableView.Columns[cell.ColumnIndex];
                if (!(column is DataGridViewCheckBoxColumn))
                    cell.Value = szRecordData;
                else
                    cell.Value = GlobalMethods.Convert.StringToValue(szRecordData, false);
            }
            return true;
        }

        /// <summary>
        /// ��Ե�ǰ�����б�,����������������
        /// </summary>
        /// <param name="dtRecordTime">��¼ʱ��</param>
        /// <returns>�Ƿ񱣴�ɹ�</returns>
        public override bool SaveGridViewData(DateTime dtRecordTime, string szSchemaId)
        {
            Form parentForm = this.m_dataTableView.FindForm();
            if (parentForm == null || parentForm.IsDisposed)
                return false;
            parentForm.Update();
            if (!this.m_dataTableView.EndEdit())
                return false;
            int nRowsCount = this.m_dataTableView.Rows.Count;
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "���ڸ��»����¼���ݣ����Ժ�...");

            foreach (DataTableViewRow row in this.m_dataTableView.Rows)
            {
                WorkProcess.Instance.Show(row.Index, false);
                if (!row.IsModifiedRow)
                    continue;

                if (!this.SaveRowRecordData(row, dtRecordTime, szSchemaId))
                {
                    this.m_dataTableView.SelectRow(row);
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowError("�����¼���ݱ���ʧ��!");
                    return false;
                }
                else
                {
                    this.m_dataTableView.SetRowState(row, RowState.Normal);
                }
            }
            WorkProcess.Instance.Close();
            return true;
        }

        /// <summary>
        /// ��ȡָ�������е�Ԫ���Ӧ�ı�ժҪ�����б�
        /// </summary>
        /// <param name="row">ָ������</param>
        /// <param name="dtRecordTime">��¼ʱ��</param>
        /// <returns>�Ƿ񱣴�ɹ�</returns>
        private bool SaveRowRecordData(DataGridViewRow row, DateTime dtRecordTime, string szSchemaId)
        {
            if (row == null || row.Index < 0)
                return false;

            //���ڻ��������ID���ժҪ���ݶ�Ӧ��ϵ�Ĺ�ϣ��
            Dictionary<string, List<SummaryData>> dictSummaryData =
                new Dictionary<string, List<SummaryData>>();

            //������Ԫ��������ת��ΪժҪ����,���������ı����ͷ���
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex <= 2)
                    continue;
                List<SummaryData> lstSummaryData = this.GetCellRecordData(cell, dtRecordTime);
                if (lstSummaryData == null || lstSummaryData.Count <= 0)
                    continue;
                foreach (SummaryData summaryData in lstSummaryData)
                {
                    if (GlobalMethods.Misc.IsEmptyString(summaryData.DocTypeID))
                    {
                        string message = "����¼������������,��{0}����û�а󶨱�!";
                        MessageBoxEx.ShowErrorFormat(message, null, summaryData.DataName);
                        continue;
                    }
                    if (!dictSummaryData.ContainsKey(summaryData.DocTypeID))
                    {
                        List<SummaryData> list = new List<SummaryData>();
                        list.Add(summaryData);
                        dictSummaryData.Add(summaryData.DocTypeID, list);
                    }
                    else
                    {
                        List<SummaryData> list = dictSummaryData[summaryData.DocTypeID];
                        if (list != null)
                            list.Add(summaryData);
                    }
                }
            }
            if (dictSummaryData.Count <= 0)
                return true;

            //��ȡ��ǰ���˵�ǰʱ���,�ѱ���Ļ����¼;
            //����ж��,��ô��ȡ���һ��,����ĳЩ���Ը��µ����ݱ�;
            //���û�л�ȡ��,��ô����һ���µĻ����¼,�����䱣�浽���ݱ�.
            PatVisitInfo patVisitInfo = row.Tag as PatVisitInfo;
            if (patVisitInfo == null)
                return false;
            List<NursingRecInfo> lstNursingRecInfos = null;
            short shRet = NurRecService.Instance.GetNursingRecList(patVisitInfo.PatientID
                , patVisitInfo.VisitID, patVisitInfo.SubID, szSchemaId
                , dtRecordTime, dtRecordTime, ref lstNursingRecInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("��д�����¼�б��ѯʧ��!");
                return false;
            }
            NurDocList lstDocInfos = null;
            NursingRecInfo nursingRecInfo = null;
            if (lstNursingRecInfos == null || lstNursingRecInfos.Count <= 0)
            {
                nursingRecInfo = this.CreateNursingRecord(patVisitInfo, dtRecordTime, szSchemaId);
                shRet = NurRecService.Instance.SaveNursingRec(nursingRecInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("���滤���¼ʧ��!");
                    return false;
                }
            }
            else
            {
                nursingRecInfo = lstNursingRecInfos[lstNursingRecInfos.Count - 1];
                nursingRecInfo = this.ModifyNursingRecord(nursingRecInfo);
                shRet = NurRecService.Instance.UpdateNursingRec(nursingRecInfo);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowError("���»����¼ʧ��!");
                    return false;
                }
                DocumentService.Instance.GetRecordDocInfos(nursingRecInfo.RecordID, ref lstDocInfos);
            }

            //��������¼�봰���и��а󶨵ı�����,��ȡ�ѱ���ı�;
            //����ж��,��ô��ȡ���һ������;
            //���û�л�ȡ��,��ô����һ�ݴ����Ͷ�Ӧ���µı�����;
            //Ȼ��ת��������ժҪ�����б����йظñ����͵�ժҪ����,���õ���������.
            foreach (string szDocTypeID in dictSummaryData.Keys)
            {
                NurDocInfo currDocInfo = null;
                if (lstDocInfos != null && lstDocInfos.Count > 0)
                {
                    foreach (NurDocInfo docInfo in lstDocInfos)
                    {
                        if (docInfo.DocTypeID == szDocTypeID)
                            currDocInfo = docInfo;
                    }
                }
                if (currDocInfo != null)
                {
                    DocumentInfo document = DocumentInfo.Create(currDocInfo);
                    if (!this.m_formEditor.OpenDocument(document))
                        return false;
                }
                else
                {
                    DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
                    if (docTypeInfo == null)
                    {
                        string message = "����¼������������,û���ҵ�ID��Ϊ��{0}���ı�!";
                        MessageBoxEx.ShowErrorFormat(message, null, szDocTypeID);
                        return false;
                    }
                    DocumentInfo document =
                        DocumentInfo.Create(docTypeInfo, patVisitInfo, nursingRecInfo.RecordID);
                    if (!this.m_formEditor.OpenDocument(document))
                        return false;
                    if (this.m_formEditor.Document == null)
                        return false;
                    this.m_formEditor.Document.ModifyRecordTime(nursingRecInfo.RecordTime);
                }

                List<SummaryData> lstSummaryData = dictSummaryData[szDocTypeID];
                foreach (SummaryData summaryData in lstSummaryData)
                {
                    if (!this.m_formEditor.UpdateFormData(summaryData.DataName, summaryData.DataValue))
                    {
                        MessageBoxEx.ShowWarningFormat("��{0}�����ݱ���ʧ��,�ڡ�{1}������û�ж�����մ�����!"
                            , null, summaryData.DataName, this.m_formEditor.Document.DocTitle);
                    }
                }
                if (!this.m_formEditor.SaveDocument())
                    return false;
                if (!this.m_formEditor.CommitDocument(this.m_formEditor.Document))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// ����һ���µĻ����¼��������Ϣ
        /// </summary>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <param name="dtRecordTime">��¼ʱ��</param>
        /// <returns>�����¼��������Ϣ</returns>
        private NursingRecInfo CreateNursingRecord(PatVisitInfo patVisit, DateTime dtRecordTime, string SchemaId)
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            if (patVisit == null)
                return null;
            NursingRecInfo nursingRecInfo = new NursingRecInfo();
            nursingRecInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            nursingRecInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            nursingRecInfo.CreateTime = SysTimeService.Instance.Now;
            nursingRecInfo.ModifierID = nursingRecInfo.CreatorID;
            nursingRecInfo.ModifierName = nursingRecInfo.CreatorName;
            nursingRecInfo.ModifyTime = nursingRecInfo.CreateTime;
            nursingRecInfo.PatientID = patVisit.PatientID;
            nursingRecInfo.VisitID = patVisit.VisitID;
            nursingRecInfo.SubID = patVisit.SubID;
            nursingRecInfo.Recorder1Name = nursingRecInfo.CreatorName;
            nursingRecInfo.Recorder1ID = nursingRecInfo.CreatorID;
            nursingRecInfo.Recorder2ID = nursingRecInfo.CreatorID;
            nursingRecInfo.Recorder2Name = nursingRecInfo.CreatorName;

            nursingRecInfo.RecordTime = dtRecordTime;
            nursingRecInfo.RecordDate = nursingRecInfo.RecordTime.Date;

            nursingRecInfo.PatientName = patVisit.PatientName;
            nursingRecInfo.WardCode = patVisit.WardCode;
            nursingRecInfo.WardName = patVisit.WardName;
            nursingRecInfo.RecordID = nursingRecInfo.MakeRecordID();
            nursingRecInfo.SchemaID = SchemaId;
            return nursingRecInfo;
        }

        /// <summary>
        /// �޸�ָ���Ĵ����µĻ����¼����������Ϣ
        /// </summary>
        /// <param name="nursingRecInfo">�����¼��������Ϣ</param>
        /// <returns>�����¼��������Ϣ</returns>
        private NursingRecInfo ModifyNursingRecord(NursingRecInfo nursingRecInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return nursingRecInfo;
            if (nursingRecInfo != null)
            {
                nursingRecInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
                nursingRecInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
                nursingRecInfo.ModifyTime = SysTimeService.Instance.Now;
                nursingRecInfo.Recorder2ID = nursingRecInfo.ModifierID;
                nursingRecInfo.Recorder2Name = nursingRecInfo.ModifierName;
            }
            return nursingRecInfo;
        }

        /// <summary>
        /// ��ȡָ����Ԫ���Ӧ�ı�ժҪ�����б�
        /// </summary>
        /// <param name="cell">ָ���ĵ�Ԫ��</param>
        /// <param name="dtRecordTime">��¼ʱ��</param>
        /// <returns>�������б�</returns>
        private List<SummaryData> GetCellRecordData(DataGridViewCell cell, DateTime dtRecordTime)
        {
            if (this.m_dataTableView == null || cell == null)
                return null;
            DataTableViewRow row = this.m_dataTableView.Rows[cell.RowIndex];
            PatVisitInfo patVisitInfo = row.Tag as PatVisitInfo;
            if (patVisitInfo == null)
                return null;
            DataGridViewColumn column = this.m_dataTableView.Columns[cell.ColumnIndex];
            GridViewColumn gridViewColumn = column.Tag as GridViewColumn;
            if (gridViewColumn == null)
                return null;

            string szCellValue = string.Empty;
            if (cell.Value != null)
                szCellValue = cell.Value.ToString().Trim();

            List<SummaryData> lstSummaryData = new List<SummaryData>();

            SummaryData summaryData = new SummaryData();
            summaryData.PatientID = patVisitInfo.PatientID;
            summaryData.VisitID = patVisitInfo.VisitID;
            summaryData.SubID = patVisitInfo.SubID;
            summaryData.DataTime = dtRecordTime;
            summaryData.Category = 1;
            summaryData.DataType = gridViewColumn.ColumnType;
            summaryData.DataUnit = gridViewColumn.ColumnUnit;
            summaryData.DocTypeID = gridViewColumn.DocTypeID;

            if (column as ComboBoxTextBoxColumn != null)
            {
                string szValue1 = "����";
                string szValue2 = string.Empty;

                char separator = ((ComboBoxTextBoxColumn)column).ValueSeparator;
                int index = szCellValue.IndexOf(separator);
                if (index > 0)
                    szValue1 = szCellValue.Substring(0, index).Trim();
                if (index >= 0 && index < szCellValue.Length - 1)
                    szValue2 = szCellValue.Substring(index + 1).Trim();

                summaryData.DataName = SystemContext.Instance.GetVitalSignsName(szValue1);
                summaryData.DataValue = szValue2;
                lstSummaryData.Add(summaryData);

                SummaryData summaryData1 = summaryData.Clone() as SummaryData;
                summaryData1.DataName = "����";
                lstSummaryData.Add(summaryData1);
            }
            else if (gridViewColumn.ColumnTag == "Ѫѹ")
            {
                string szValue1 = string.Empty;
                string szValue2 = string.Empty;

                int index = szCellValue.IndexOf('/');
                if (index > 0)
                    szValue1 = szCellValue.Substring(0, index).Trim();
                if (index >= 0 && index < szCellValue.Length - 1)
                    szValue2 = szCellValue.Substring(index + 1).Trim();

                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("Ѫѹhigh");
                summaryData.DataValue = szValue1;
                lstSummaryData.Add(summaryData);

                summaryData = summaryData.Clone() as SummaryData;
                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("ѪѹLow");
                summaryData.DataValue = szValue2;
                lstSummaryData.Add(summaryData);
            }
            else
            {
                if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnTag))
                    summaryData.DataName = gridViewColumn.ColumnTag;
                else
                    summaryData.DataName = gridViewColumn.ColumnName;
                summaryData.DataName = SystemContext.Instance.GetVitalSignsName(summaryData.DataName);
                summaryData.DataValue = SystemContext.Instance.GetVitalSignsValue(summaryData.DataName, szCellValue);
                lstSummaryData.Add(summaryData);
            }
            return lstSummaryData;
        }
    }
}
