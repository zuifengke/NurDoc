// ***********************************************************
// 护理电子病历系统,体征数据批量录入模块处理器.
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
        /// 针对当前病人列表,加载指定时间点的体征数据
        /// </summary>
        /// <param name="dtRecordTime">时间点</param>
        /// <returns>是否加载成功</returns>
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
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "正在加载体征数据，请稍候...");

            foreach (DataTableViewRow row in this.m_dataTableView.Rows)
            {
                WorkProcess.Instance.Show(row.Index + 1, false);

                //清空已显示的体征数据
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex > 2)
                        cell.Value = null;
                }

                //查询加载新的体征数据
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
                    MessageBoxEx.ShowErrorFormat("病人“{0}”的体征数据下载失败!", null, patVisitInfo.PatientName);
                    return false;
                }
                if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
                    foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                    {
                        //首先排除大便次数等需要今日录入昨天数据的体征数据
                        if (SystemContext.Instance.VitalSignTimeBeforeOneDay.Contains(vitalSignsData.RecordName))
                            continue;
                        this.LoadGridViewRowData(row, vitalSignsData, dtRecordTime);
                    }
                //获取一天只录一次的体征数据
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
                //获取录入时间减一天的体征数据（大便次数/灌肠后大便）等
                if (SystemContext.Instance.VitalSignTimeBeforeOneDay != string.Empty)
                {
                    shRet = VitalSignsService.Instance.GetVitalSignsList(szPatientID, szVisitID, szSubID
                        , dtBeginTime.AddHours(-dtBeginTime.Hour).AddDays(-1), dtEndTime.AddHours(-dtEndTime.Hour).AddDays(-1), ref lstVitalSignsData);
                    if (lstVitalSignsData != null && lstVitalSignsData.Count > 0)
                        foreach (VitalSignsData vitalSignsData in lstVitalSignsData)
                        {
                            //显示大便次数等需要今日录入昨天数据的体征数据
                            if (SystemContext.Instance.VitalSignTimeBeforeOneDay.Contains(vitalSignsData.RecordName))
                                this.LoadGridViewRowData(row, vitalSignsData, dtRecordTime);
                        }
                }
            }
            WorkProcess.Instance.Close();
            return true;
        }

        /// <summary>
        /// 将指定的体征数据对象加载到指定表格行
        /// </summary>
        /// <param name="row">指定表格行</param>
        /// <param name="vitalSignsData">体征数据对象</param>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewRowData(DataGridViewRow row, VitalSignsData vitalSignsData, DateTime dtRecordTime)
        {
            string szRecordData = string.Empty;
            DataGridViewCell cell = null;

            if (SystemContext.Instance.IsBodyTemperatureType(vitalSignsData.RecordName)
                && this.m_htColumnTable.Contains("体温"))
            {
                ComboBoxTextBoxColumn column = this.m_htColumnTable["体温"] as ComboBoxTextBoxColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];
                    szRecordData = SystemContext.Instance.GetVitalSignsName(vitalSignsData.RecordName)
                        + column.ValueSeparator + vitalSignsData.RecordData;
                }
            }
            if (!SystemContext.Instance.SystemOption.EmrsVital
                && vitalSignsData.RecordName == SystemContext.Instance.GetVitalSignsName("血压Low")
                && this.m_htColumnTable.Contains("血压"))
            {
                DataGridViewColumn column = this.m_htColumnTable["血压"] as DataGridViewColumn;
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
                && vitalSignsData.RecordName == SystemContext.Instance.GetVitalSignsName("血压high")
                && this.m_htColumnTable.Contains("血压"))
            {
                DataGridViewColumn column = this.m_htColumnTable["血压"] as DataGridViewColumn;
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
            && vitalSignsData.RecordName.Contains("血压high")
            && this.m_htColumnTable.Contains("血压"))
            {
                //默认录入时刻为4 8 12 16 为别为  血压high 血压high2 血压high3 血压high4  可在起始页中配置4个时间点
                if ((dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[0] && vitalSignsData.RecordName == "血压high")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1] && vitalSignsData.RecordName == "血压high2")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2] && vitalSignsData.RecordName == "血压high3")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3] && vitalSignsData.RecordName == "血压high4"))
                {
                    DataGridViewColumn column = this.m_htColumnTable["血压"] as DataGridViewColumn;
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
            && vitalSignsData.RecordName.Contains("血压Low")
            && this.m_htColumnTable.Contains("血压"))
            {
                //默认录入时刻为4 8 12 16 为别为  血压high 血压high2 血压high3 血压high4  可在起始页中配置4个时间点
                if ((dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[0] && vitalSignsData.RecordName == "血压Low")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1] && vitalSignsData.RecordName == "血压Low2")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2] && vitalSignsData.RecordName == "血压Low3")
                    || (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3] && vitalSignsData.RecordName == "血压Low4"))
                {
                    DataGridViewColumn column = this.m_htColumnTable["血压"] as DataGridViewColumn;
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
        /// 针对当前病人列表,保存所有体征数据
        /// </summary>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>是否保存成功</returns>
        public override bool SaveGridViewData(DateTime dtRecordTime,string szSchemaId)
        {
            Form parentForm = this.m_dataTableView.FindForm();
            if (parentForm == null || parentForm.IsDisposed)
                return false;
            parentForm.Update();
            if (!this.m_dataTableView.EndEdit())
                return false;
            int nRowsCount = this.m_dataTableView.Rows.Count;
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "正在更新体征数据，请稍候...");

            List<VitalSignsData> lstVitalSignsData = new List<VitalSignsData>();
            foreach (DataTableViewRow row in this.m_dataTableView.Rows)
            {
                WorkProcess.Instance.Show(row.Index, false);
                if (!row.IsModifiedRow)
                    continue;

                PatVisitInfo patVisitInfo = row.Tag as PatVisitInfo;
                if (patVisitInfo == null)
                    continue;

                //将已修改的体征数据加入到保存列表
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex <= 2)
                        continue;
                    List<VitalSignsData> list = this.GetCellRecordData(cell, dtRecordTime);
                    if (list != null && list.Count > 0)
                        lstVitalSignsData.AddRange(list);
                }
            }

            WorkProcess.Instance.Show("正在提交到数据库，请稍候...", nRowsCount, false);

            //将已修改的体征数据列表,提交保存到数据库
            short shRet = SystemConst.ReturnValue.OK;
            if (lstVitalSignsData.Count > 0)
                shRet = VitalSignsService.Instance.SaveVitalSignsData(lstVitalSignsData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                WorkProcess.Instance.Close();
                MessageBoxEx.ShowError("体征数据保存失败!");
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
        /// 获取指定单元格对应的体征数据列表
        /// </summary>
        /// <param name="cell">指定的单元格</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>体征项列表</returns>
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
            //处理体征不带时间点
            if (SystemContext.Instance.VitalSignNoTimePoint.Contains(gridViewColumn.ColumnTag))
                vitalSignsData.RecordTime = vitalSignsData.RecordTime.AddHours(-vitalSignsData.RecordTime.Hour);
            //处理体征时间减一天，主要是昨日大便问题
            if (SystemContext.Instance.VitalSignTimeBeforeOneDay.Contains(gridViewColumn.ColumnTag))
            {
                vitalSignsData.RecordDate = vitalSignsData.RecordDate.AddDays(-1);
                vitalSignsData.RecordTime = vitalSignsData.RecordTime.AddDays(-1);
            }
            if (column as ComboBoxTextBoxColumn != null)
            {
                string szValue1 = "体温";
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
                && gridViewColumn.ColumnTag == "血压")
            {
                string szValue1 = string.Empty;
                string szValue2 = string.Empty;

                int index = szCellValue.IndexOf('/');
                if (index > 0)
                    szValue1 = szCellValue.Substring(0, index).Trim();
                if (index >= 0 && index < szCellValue.Length - 1)
                    szValue2 = szCellValue.Substring(index + 1).Trim();

                vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压high");
                vitalSignsData.RecordData = szValue1;
                lstVitalSignsData.Add(vitalSignsData);

                vitalSignsData = vitalSignsData.Clone() as VitalSignsData;
                vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压Low");
                vitalSignsData.RecordData = szValue2;
                lstVitalSignsData.Add(vitalSignsData);
            }
            //默认录入时刻为4 8 12 16 为别为  血压high 血压high2 血压high3 血压high4  可在起始页中配置4个时间点
            else if (SystemContext.Instance.SystemOption.EmrsVital
           && gridViewColumn.ColumnTag == "血压")
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
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压high");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压high2");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压high3");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压high4");
                vitalSignsData.RecordData = szValue1;
                vitalSignsData.RecordTime = dtRecordTime.Date;
                lstVitalSignsData.Add(vitalSignsData);

                vitalSignsData = vitalSignsData.Clone() as VitalSignsData;
                if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[0])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压Low");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[1])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压Low2");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[2])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压Low3");
                else if (dtRecordTime.Hour == SystemContext.Instance.StandardBPTimePoint[3])
                    vitalSignsData.RecordName = SystemContext.Instance.GetVitalSignsName("血压Low4");
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
