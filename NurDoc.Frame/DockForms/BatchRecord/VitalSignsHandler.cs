// ***********************************************************
// ������Ӳ���ϵͳ,������������¼��ģ�鴦����.
// Creator:YangMingkun  Date:2013-1-9
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.DockForms
{
    internal class VitalSignsHandler : BatchRecordHandler
    {
        public VitalSignsHandler(DataTableView dataTableView, GridViewSchema schema)
            : base(dataTableView, schema)
        {
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
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "���ڼ����������ݣ����Ժ�...");

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
                List<VitalSignsData> lstVitalSignsData = null;
                short shRet = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, szSubID
                    , dtBeginTime, dtEndTime, ref lstVitalSignsData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowErrorFormat("���ˡ�{0}����������������ʧ��!", null, patVisitInfo.PatientName);
                    return false;
                }
                if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
                    foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                    {
                        //�����ų�����������Ҫ����¼���������ݵ���������
                        if (SystemContext.Instance.VitalSignTimeBeforeOneDay.Contains(vitalSignsData.RecordName))
                            continue;
                        this.LoadGridViewRowData(row, vitalSignsData, dtRecordTime);
                    }
                //��ȡһ��ֻ¼һ�ε���������
                if (SystemContext.Instance.VitalSignNoTimePoint != string.Empty)
                {
                    shRet = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, szSubID
                        , dtBeginTime.AddHours(-dtBeginTime.Hour), dtEndTime.AddHours(-dtEndTime.Hour), ref lstVitalSignsData);
                    if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
                    {
                        foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                            this.LoadGridViewRowData(row, vitalSignsData, dtRecordTime);
                    }
                }
                //��ȡ¼��ʱ���һ����������ݣ�������/�೦���㣩��
                if (SystemContext.Instance.VitalSignTimeBeforeOneDay != string.Empty)
                {
                    shRet = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, szSubID
                        , dtBeginTime.AddHours(-dtBeginTime.Hour).AddDays(-1), dtEndTime.AddHours(-dtEndTime.Hour).AddDays(-1), ref lstVitalSignsData);
                    if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
                        foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                        {
                            //��ʾ����������Ҫ����¼���������ݵ���������
                            if (SystemContext.Instance.VitalSignTimeBeforeOneDay.Contains(vitalSignsData.RecordName))
                                this.LoadGridViewRowData(row, vitalSignsData, dtRecordTime);
                        }
                }
            }
            WorkProcess.Instance.Close();
            return true;
        }

        /// <summary>
        /// ��ָ�����������ݶ�����ص�ָ�������
        /// </summary>
        /// <param name="row">ָ�������</param>
        /// <param name="vitalSignsData">�������ݶ���</param>
        /// <returns>�Ƿ���سɹ�</returns>
        private bool LoadGridViewRowData(DataGridViewRow row, VitalSignsData vitalSignsData, DateTime dtRecordTime)
        {
            string szRecordData = string.Empty;
            DataGridViewCell cell = null;

            if (SystemContext.Instance.IsBodyTemperatureType(vitalSignsData.RecordName)
                && this.m_htColumnTable.Contains("����"))
            {
                ComboBoxTextBoxColumn column = this.m_htColumnTable["����"] as ComboBoxTextBoxColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];
                    szRecordData = SystemContext.Instance.GetVitalSignsName(vitalSignsData.RecordName)
                        + column.ValueSeparator + vitalSignsData.RecordData;
                }
            }
            if (!SystemContext.Instance.SystemOption.EmrsVital
                && vitalSignsData.RecordName == SystemContext.Instance.GetVitalSignsName("ѪѹLow")
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
                        szRecordData = "/" + vitalSignsData.RecordData;
                    else
                        szRecordData = szCellValue + vitalSignsData.RecordData;
                }
            }
            else if (!SystemContext.Instance.SystemOption.EmrsVital
                && vitalSignsData.RecordName == SystemContext.Instance.GetVitalSignsName("Ѫѹhigh")
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
                        szRecordData = vitalSignsData.RecordData + "/";
                    else
                        szRecordData = vitalSignsData.RecordData + szCellValue;
                }
            }

            else if (SystemContext.Instance.SystemOption.EmrsVital
            && vitalSignsData.RecordName.Contains("Ѫѹhigh")
            && this.m_htColumnTable.Contains("Ѫѹ"))
            {
                //Ĭ��¼��ʱ��Ϊ4 8 12 16 Ϊ��Ϊ  Ѫѹhigh Ѫѹhigh2 Ѫѹhigh3 Ѫѹhigh4  ������ʼҳ������4��ʱ���
                if ((dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[0] && vitalSignsData.RecordName == "Ѫѹhigh")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1] && vitalSignsData.RecordName == "Ѫѹhigh2")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2] && vitalSignsData.RecordName == "Ѫѹhigh3")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3] && vitalSignsData.RecordName == "Ѫѹhigh4"))
                {
                    DataGridViewColumn column = this.m_htColumnTable["Ѫѹ"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                    {
                        cell = row.Cells[column.Index];

                        string szCellValue = string.Empty;
                        if (row.Cells[column.Index].Value != null)
                            szCellValue = row.Cells[column.Index].Value.ToString().Trim();

                        if (szCellValue == null || !szCellValue.StartsWith("/"))
                            szRecordData = vitalSignsData.RecordData + "/";
                        else
                            szRecordData = vitalSignsData.RecordData + szCellValue;
                    }
                }
            }
            else if (SystemContext.Instance.SystemOption.EmrsVital
            && vitalSignsData.RecordName.Contains("ѪѹLow")
            && this.m_htColumnTable.Contains("Ѫѹ"))
            {
                //Ĭ��¼��ʱ��Ϊ4 8 12 16 Ϊ��Ϊ  Ѫѹhigh Ѫѹhigh2 Ѫѹhigh3 Ѫѹhigh4  ������ʼҳ������4��ʱ���
                if ((dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[0] && vitalSignsData.RecordName == "ѪѹLow")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1] && vitalSignsData.RecordName == "ѪѹLow2")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2] && vitalSignsData.RecordName == "ѪѹLow3")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3] && vitalSignsData.RecordName == "ѪѹLow4"))
                {
                    DataGridViewColumn column = this.m_htColumnTable["Ѫѹ"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                    {
                        cell = row.Cells[column.Index];

                        string szCellValue = string.Empty;
                        if (cell.Value != null)
                            szCellValue = cell.Value.ToString().Trim();

                        if (szCellValue == null || !szCellValue.EndsWith("/"))
                            szRecordData = "/" + vitalSignsData.RecordData;
                        else
                            szRecordData = szCellValue + vitalSignsData.RecordData;
                    }
                }
            }

            else
            {
                string szRecordName = SystemContext.Instance.GetVitalSignsName(vitalSignsData.RecordName);
                DataGridViewColumn column = this.m_htColumnTable[szRecordName] as DataGridViewColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];
                    szRecordData = SystemContext.Instance.GetVitalSignsValue(vitalSignsData.RecordName
                        , vitalSignsData.RecordData);
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
        public override bool SaveGridViewData(DateTime dtRecordTime,string szSchemaId)
        {
            Form parentForm = this.m_dataTableView.FindForm();
            if (parentForm == null || parentForm.IsDisposed)
                return false;
            parentForm.Update();
            if (!this.m_dataTableView.EndEdit())
                return false;
            int nRowsCount = this.m_dataTableView.Rows.Count;
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "���ڸ����������ݣ����Ժ�...");

            List<VitalSignsData> lstVitalSignsData = new List<VitalSignsData>();
            foreach (DataTableViewRow row in this.m_dataTableView.Rows)
            {
                WorkProcess.Instance.Show(row.Index, false);
                if (!row.IsModifiedRow)
                    continue;

                PatVisitInfo patVisitInfo = row.Tag as PatVisitInfo;
                if (patVisitInfo == null)
                    continue;

                //�����޸ĵ��������ݼ��뵽�����б�
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex <= 2)
                        continue;
                    List<VitalSignsData> list = this.GetCellRecordData(cell, dtRecordTime);
                    if (list != null && list.Count > 0)
                        lstVitalSignsData.AddRange(list);
                }
            }

            WorkProcess.Instance.Show("�����ύ�����ݿ⣬���Ժ�...", nRowsCount, false);

            //�����޸ĵ����������б�,�ύ���浽���ݿ�
            short shRet = SystemConst.ReturnValue.OK;
            if (lstVitalSignsData.Count > 0)
                shRet = VitalSignsService.Instance.SaveVitalSignsData(lstVitalSignsData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                WorkProcess.Instance.Close();
                MessageBoxEx.ShowError("�������ݱ���ʧ��!");
                return false;
            }
            else
            {
                foreach (DataTableViewRow row in this.m_dataTableView.Rows)
                {
                    if (row.IsModifiedRow)
                        this.m_dataTableView.SetRowState(row, RowState.Normal);
                }
                WorkProcess.Instance.Close();
            }
            return true;
        }

        /// <summary>
        /// ��ȡָ����Ԫ���Ӧ�����������б�
        /// </summary>
        /// <param name="cell">ָ���ĵ�Ԫ��</param>
        /// <param name="dtRecordTime">��¼ʱ��</param>
        /// <returns>�������б�</returns>
        private List<VitalSignsData> GetCellRecordData(DataGridViewCell cell, DateTime dtRecordTime)
        {
            if (cell == null)
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

            List<VitalSignsData> lstVitalSignsData = new List<VitalSignsData>();

            VitalSignsData vitalSignsData = new VitalSignsData();
            vitalSignsData.PatientID = patVisitInfo.PatientID;
            vitalSignsData.VisitID = patVisitInfo.VisitID;
            vitalSignsData.SubID = patVisitInfo.SubID;
            vitalSignsData.RecordDate = dtRecordTime.Date;
            vitalSignsData.RecordTime = dtRecordTime;
            vitalSignsData.Category = 1;
            vitalSignsData.DataType = gridViewColumn.ColumnType;
            vitalSignsData.DataUnit = gridViewColumn.ColumnUnit;
            //������������ʱ���
            if (SystemContext.Instance.VitalSignNoTimePoint.Contains(gridViewColumn.ColumnTag))
                vitalSignsData.RecordTime = vitalSignsData.RecordTime.AddHours(-vitalSignsData.RecordTime.Hour);
            //��������ʱ���һ�죬��Ҫ�����մ������
            if (SystemContext.Instance.VitalSignTimeBeforeOneDay.Contains(gridViewColumn.ColumnTag))
            {
                vitalSignsData.RecordDate = vitalSignsData.RecordDate.AddDays(-1);
                vitalSignsData.RecordTime = vitalSignsData.RecordTime.AddDays(-1);
            }
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

                vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName(szValue1);
                vitalSignsData.RecordData = szValue2;
                lstVitalSignsData.Add(vitalSignsData);
            }
            else if (!SystemContext.Instance.SystemOption.EmrsVital
                && gridViewColumn.ColumnTag == "Ѫѹ")
            {
                string szValue1 = string.Empty;
                string szValue2 = string.Empty;

                int index = szCellValue.IndexOf('/');
                if (index > 0)
                    szValue1 = szCellValue.Substring(0, index).Trim();
                if (index >= 0 && index < szCellValue.Length - 1)
                    szValue2 = szCellValue.Substring(index + 1).Trim();

                vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("Ѫѹhigh");
                vitalSignsData.RecordData = szValue1;
                lstVitalSignsData.Add(vitalSignsData);

                vitalSignsData = vitalSignsData.Clone() as VitalSignsData;
                vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("ѪѹLow");
                vitalSignsData.RecordData = szValue2;
                lstVitalSignsData.Add(vitalSignsData);
            }
            //Ĭ��¼��ʱ��Ϊ4 8 12 16 Ϊ��Ϊ  Ѫѹhigh Ѫѹhigh2 Ѫѹhigh3 Ѫѹhigh4  ������ʼҳ������4��ʱ���
            else if (SystemContext.Instance.SystemOption.EmrsVital
           && gridViewColumn.ColumnTag == "Ѫѹ")
            {
                if (!SystemContext.Instance.IsTimeInStandardList(dtRecordTime.Hour))
                    return null;
                string szValue1 = string.Empty;
                string szValue2 = string.Empty;

                int index = szCellValue.IndexOf('/');
                if (index > 0)
                    szValue1 = szCellValue.Substring(0, index).Trim();
                if (index >= 0 && index < szCellValue.Length - 1)
                    szValue2 = szCellValue.Substring(index + 1).Trim();
                if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[0])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("Ѫѹhigh");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("Ѫѹhigh2");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("Ѫѹhigh3");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("Ѫѹhigh4");
                vitalSignsData.RecordData = szValue1;
                vitalSignsData.RecordTime = dtRecordTime.Date;
                lstVitalSignsData.Add(vitalSignsData);

                vitalSignsData = vitalSignsData.Clone() as VitalSignsData;
                if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[0])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("ѪѹLow");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("ѪѹLow2");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("ѪѹLow3");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("ѪѹLow4");
                vitalSignsData.RecordData = szValue2;
                lstVitalSignsData.Add(vitalSignsData);
            }
            else
            {
                if (!GlobalMethods.Misc.IsEmptyString(gridViewColumn.ColumnTag))
                    vitalSignsData.RecordName = gridViewColumn.ColumnTag;
                else
                    vitalSignsData.RecordName = gridViewColumn.ColumnName;
                vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName(vitalSignsData.RecordName);
                vitalSignsData.RecordData = SystemContext.Instance.GetVitalSignsValue(vitalSignsData.RecordName, szCellValue);
                lstVitalSignsData.Add(vitalSignsData);
            }
            return lstVitalSignsData;
        }
    }
}
