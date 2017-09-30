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

namespace Heren.NurDoc.PatPage.NurRecTableInput
{
    internal class NursingRecHandler
    {
        /// <summary>
        /// ����֧������¼�����ݵ���ʾ�ͱ༭�ı��ؼ�
        /// </summary>
        protected DataTableView m_dataTableView = null;

        /// <summary>
        /// ���ڴ洢����¼������Ϣ���пؼ����ϵ�Ĺ�ϣ��
        /// </summary>
        protected Hashtable m_htColumnTable = null;

        /// <summary>
        /// ���ڻ��浱ǰ���ڻ״̬����¼�뷽��
        /// </summary>
        protected GridViewSchema m_schema = null;

        /// <summary>
        /// �����¼�������༭��,������ʱ���а󶨵ı�
        /// </summary>
        private DocumentControl m_formEditor = null;

        public NursingRecHandler(DataTableView dataTableView, GridViewSchema schema, Hashtable htColumnTable)
        {
            this.m_dataTableView = dataTableView;
            this.m_schema = schema;
            this.m_htColumnTable = htColumnTable;
            this.m_formEditor = new DocumentControl();
        }

        /// <summary>
        /// �������и��µĻ����¼
        /// </summary>
        /// <returns>�Ƿ񱣴�ɹ�</returns>
        public bool SaveGridViewData()
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
                if (!row.IsModifiedRow && !row.IsDeletedRow && !row.IsNewRow)
                    continue;

                if (!this.SaveRowRecordData(row))
                {
                    this.m_dataTableView.SelectRow(row);
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowError("�����¼���ݱ���ʧ��!");
                    return false;
                }
                else
                {
                    this.m_dataTableView.SetRowState(row, RowState.Normal);
                    NursingRecInfo nursingRecInfo = row.Tag as NursingRecInfo;
                    if (nursingRecInfo.RecordID.Contains("$"))
                    {
                        row.Cells[0].ReadOnly = true;
                    }

                    //���⴦��������
                    DataGridViewColumn column = this.m_htColumnTable["������"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0)
                    {
                        row.Cells[column.Index].Value = nursingRecInfo.CreatorName;
                    }

                    //���⴦���޸�����
                    column = this.m_htColumnTable["�޸���"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0)
                    {
                        row.Cells[column.Index].Value = nursingRecInfo.ModifierName;
                    }
                }
            }
            WorkProcess.Instance.Close();
            return true;
        }

        /// <summary>
        /// ��ȡָ�������е�Ԫ���Ӧ�ı�ժҪ�����б�
        /// </summary>
        /// <param name="row">ָ������</param>
        /// <returns>�Ƿ񱣴�ɹ�</returns>
        private bool SaveRowRecordData(DataGridViewRow row)
        {
            if (row == null || row.Index < 0)
                return false;
            NursingRecInfo nursingRecInfo = row.Tag as NursingRecInfo;
            if (nursingRecInfo == null)
                return false;
            //����������ڻ���С�� ����Ҫ����
            if (nursingRecInfo.SummaryFlag == 1)
                return true;
            //��ժҪ���ݼ���
            List<SummaryData> lstSummaryData = new List<SummaryData>();

            //������Ԫ��������ת��ΪժҪ����,���������ı����ͷ���
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex <= 0)
                {
                    continue;
                }
                List<SummaryData> summaryDatas = this.GetCellRecordData(cell, nursingRecInfo.RecordTime);
                if (summaryDatas == null || summaryDatas.Count <= 0)
                    continue;
                foreach (SummaryData summaryData in summaryDatas)
                {
                    lstSummaryData.Add(summaryData);
                }
            }

            if (SystemContext.Instance.SystemOption.VitalSignsSync)
            {
                this.HandleSummaryDatas(ref lstSummaryData);
            }

            //���浱ǰʱ���Ļ����¼����ժҪ����
            short shRet = NurRecService.Instance.SaveNursingRec(nursingRecInfo.RecordTime, ref nursingRecInfo, lstSummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            row.Tag = nursingRecInfo;
            return true;
        }

        /// <summary>
        /// ��������ǰ��ժҪ����
        /// </summary>
        /// <param name="lstSummaryData">����������ժҪ����</param>
        public void HandleSummaryDatas(ref List<SummaryData> lstSummaryData)
        {
            List<SummaryData> lstSummaryDataCopy = new List<SummaryData>();
            lstSummaryDataCopy.AddRange(lstSummaryData);
            List<SummaryData> lstSummaryDataAdd = new List<SummaryData>();
            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData.DataName == "����" || summaryData.DataName == "����" || summaryData.DataName == "����"
                    || summaryData.DataName == "����" || summaryData.DataName == "����" || summaryData.DataName == "Ҹ��" 
                    || summaryData.DataName == SystemContext.Instance.GetVitalSignsName("Ѫѹhigh")
                    || summaryData.DataName == SystemContext.Instance.GetVitalSignsName("ѪѹLow"))
                {
                    summaryData.Category = 1;
                }
                string szRetValue = string.Empty;
                if (summaryData.DataName == "��������" && IsSummaryDatasContains(lstSummaryDataCopy, "������", ref szRetValue))
                {
                    SummaryData summaryRLData = summaryData.Clone() as SummaryData;
                    summaryRLData.DataName = summaryData.DataValue;
                    summaryRLData.DataValue = szRetValue;
                    summaryRLData.Category = 1;
                    summaryRLData.Remarks = "����";
                    lstSummaryDataAdd.Add(summaryRLData);
                }
                else if (summaryData.DataName == "��������" && IsSummaryDatasContains(lstSummaryDataCopy, "������", ref szRetValue))
                {
                    SummaryData summaryCLData = summaryData.Clone() as SummaryData;
                    summaryCLData.DataName = summaryData.DataValue;
                    summaryCLData.DataValue = szRetValue;
                    summaryCLData.Category = 1;
                    summaryCLData.Remarks = "����";
                    lstSummaryDataAdd.Add(summaryCLData);
                }
            }
            lstSummaryData.AddRange(lstSummaryDataAdd);
        }

        /// <summary>
        /// �ж���ժҪ�б����Ƿ����ָ�����Ƶ�ժҪ���ݲ�����ժҪֵ
        /// </summary>
        /// <param name="lstSummaryData">ժҪ�б�</param>
        /// <param name="name">��ѯ����</param>
        /// <param name="value">����ֵ</param>
        /// <returns>true or false</returns>
        private bool IsSummaryDatasContains(List<SummaryData> lstSummaryData, string name,ref string value)
        {
            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData.DataName == name)
                {
                    value = summaryData.DataValue;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ����һ���µĻ����¼��������Ϣ
        /// </summary>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <param name="dtRecordTime">��¼ʱ��</param>
        /// <returns>�����¼��������Ϣ</returns>
        public NursingRecInfo CreateNursingRecord(PatVisitInfo patVisit, DateTime dtRecordTime)
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
            nursingRecInfo.PatientID = patVisit.PatientId;
            nursingRecInfo.VisitID = patVisit.VisitId;
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
            NursingRecInfo nursingRecInfo = row.Tag as NursingRecInfo;
            if (nursingRecInfo == null)
                return null;
            DataGridViewColumn column = this.m_dataTableView.Columns[cell.ColumnIndex];
            GridViewColumn gridViewColumn = column.Tag as GridViewColumn;
            if (gridViewColumn == null)
                return null;

            string szCellValue = string.Empty;
            if (cell.Value != null)
                szCellValue = cell.Value.ToString();
            if (szCellValue == string.Empty)
                return null;
            List<SummaryData> lstSummaryData = new List<SummaryData>();

            SummaryData summaryData = new SummaryData();
            summaryData.PatientID = PatientTable.Instance.ActivePatient.PatientId;
            summaryData.VisitID = PatientTable.Instance.ActivePatient.VisitId;
            summaryData.SubID = PatientTable.Instance.ActivePatient.SubID;
            summaryData.DataTime = dtRecordTime;
            summaryData.Category = 0;
            summaryData.DataType = gridViewColumn.ColumnType;
            summaryData.DataUnit = gridViewColumn.ColumnUnit;
            summaryData.DocTypeID = gridViewColumn.SchemaID;//������SchemaID��ΪժҪ���ĵ�����ID��
            summaryData.RecordID = nursingRecInfo.RecordID;

            if (gridViewColumn.ColumnTag == "��¼����")
            {
                nursingRecInfo.RecordContent = szCellValue;
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
                if (index < 0)
                {
                    szValue1 = szCellValue.ToString();
                }
                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("Ѫѹhigh");
                summaryData.DataValue = szValue1;
                lstSummaryData.Add(summaryData);

                summaryData = summaryData.Clone() as SummaryData;
                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("ѪѹLow");
                summaryData.DataValue = szValue2;
                lstSummaryData.Add(summaryData);
            }
            else if (gridViewColumn.ColumnTag == "������" || gridViewColumn.ColumnTag == "�޸���"
                || gridViewColumn.ColumnTag == "�����ۼ�" || gridViewColumn.ColumnTag == "�����ۼ�")
            {
                return null;
            }
            else
            {
                if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnTag))
                    summaryData.DataName = gridViewColumn.ColumnTag;
                else
                    summaryData.DataName = gridViewColumn.ColumnName;
                summaryData.DataValue = SystemContext.Instance.GetVitalSignsValue(summaryData.DataName, szCellValue);
                lstSummaryData.Add(summaryData);
            }
            return lstSummaryData;
        }
    }
}
