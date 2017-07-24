// ***********************************************************
// 护理电子病历系统,护理记录数据批量录入模块处理器.
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
        /// 护理记录评估单编辑器,用于临时打开列绑定的表单
        /// </summary>
        private DocumentControl m_formEditor = null;

        public NursingRecHandler(DataTableView dataTableView, GridViewSchema schema)
            : base(dataTableView, schema)
        {
            this.m_formEditor = new DocumentControl();
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
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "正在加载护理记录数据，请稍候...");

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
                List<SummaryData> lstSummaryData = null;
                short shRet = DocumentService.Instance.GetSummaryData(szPatientID, szVisitID, szSubID
                    , dtBeginTime, dtEndTime, ref lstSummaryData);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowErrorFormat("病人“{0}”的护理记录数据下载失败!", null, patVisitInfo.PatientName);
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
        /// 将指定的摘要数据对象加载到指定表格行
        /// </summary>
        /// <param name="row">指定表格行</param>
        /// <param name="summaryData">摘要数据对象</param>
        /// <returns>是否加载成功</returns>
        private bool LoadGridViewRowData(DataGridViewRow row, SummaryData summaryData)
        {
            string szRecordData = string.Empty;
            DataGridViewCell cell = null;

            if (SystemContext.Instance.IsBodyTemperatureType(summaryData.DataName)
                && this.m_htColumnTable.Contains("体温"))
            {
                ComboBoxTextBoxColumn column = this.m_htColumnTable["体温"] as ComboBoxTextBoxColumn;
                if (column != null && column.Index >= 0 && column.Index < row.Cells.Count)
                {
                    cell = row.Cells[column.Index];
                    szRecordData = SystemContext.Instance.GetVitalSignsName(summaryData.DataName)
                        + column.ValueSeparator + summaryData.DataValue;
                }
            }
            else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("血压Low")
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
                        szRecordData = "/" + summaryData.DataValue;
                    else
                        szRecordData = szCellValue + summaryData.DataValue;
                }
            }
            else if (summaryData.DataName == SystemContext.Instance.GetVitalSignsName("血压high")
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
        /// 针对当前病人列表,保存所有体征数据
        /// </summary>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>是否保存成功</returns>
        public override bool SaveGridViewData(DateTime dtRecordTime, string szSchemaId)
        {
            Form parentForm = this.m_dataTableView.FindForm();
            if (parentForm == null || parentForm.IsDisposed)
                return false;
            parentForm.Update();
            if (!this.m_dataTableView.EndEdit())
                return false;
            int nRowsCount = this.m_dataTableView.Rows.Count;
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "正在更新护理记录数据，请稍候...");

            foreach (DataTableViewRow row in this.m_dataTableView.Rows)
            {
                WorkProcess.Instance.Show(row.Index, false);
                if (!row.IsModifiedRow)
                    continue;

                if (!this.SaveRowRecordData(row, dtRecordTime, szSchemaId))
                {
                    this.m_dataTableView.SelectRow(row);
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowError("护理记录数据保存失败!");
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
        /// 获取指定行所有单元格对应的表单摘要数据列表
        /// </summary>
        /// <param name="row">指定的行</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>是否保存成功</returns>
        private bool SaveRowRecordData(DataGridViewRow row, DateTime dtRecordTime, string szSchemaId)
        {
            if (row == null || row.Index < 0)
                return false;

            //用于缓存表单类型ID与表单摘要数据对应关系的哈希表
            Dictionary<string, List<SummaryData>> dictSummaryData =
                new Dictionary<string, List<SummaryData>>();

            //将各单元格中数据转换为摘要数据,并按关联的表单类型分类
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
                        string message = "批量录入列配置有误,“{0}”列没有绑定表单!";
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

            //获取当前病人当前时间点,已保存的护理记录;
            //如果有多个,那么仅取最后一个,并将某些属性更新到数据表;
            //如果没有获取到,那么创建一条新的护理记录,并将其保存到数据表.
            PatVisitInfo patVisitInfo = row.Tag as PatVisitInfo;
            if (patVisitInfo == null)
                return false;
            List<NursingRecInfo> lstNursingRecInfos = null;
            short shRet = NurRecService.Instance.GetNursingRecList(patVisitInfo.PatientID
                , patVisitInfo.VisitID, patVisitInfo.SubID, szSchemaId
                , dtRecordTime, dtRecordTime, ref lstNursingRecInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("已写护理记录列表查询失败!");
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
                    MessageBoxEx.ShowError("保存护理记录失败!");
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
                    MessageBoxEx.ShowError("更新护理记录失败!");
                    return false;
                }
                DocumentService.Instance.GetRecordDocInfos(nursingRecInfo.RecordID, ref lstDocInfos);
            }

            //根据批量录入窗口中各列绑定的表单类型,获取已保存的表单;
            //如果有多个,那么仅取最后一个并打开;
            //如果没有获取到,那么创建一份此类型对应的新的表单并打开;
            //然后将转换出来的摘要数据列表中有关该表单类型的摘要数据,设置到表单并保存.
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
                        string message = "批量录入列配置有误,没有找到ID号为“{0}”的表单!";
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
                        MessageBoxEx.ShowWarningFormat("“{0}”数据保存失败,在“{1}”表单中没有对象接收此数据!"
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
        /// 创建一条新的护理记录主索引信息
        /// </summary>
        /// <param name="patVisit">病人就诊信息</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>护理记录主索引信息</returns>
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
        /// 修改指定的待更新的护理记录单主索引信息
        /// </summary>
        /// <param name="nursingRecInfo">护理记录主索引信息</param>
        /// <returns>护理记录主索引信息</returns>
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
        /// 获取指定单元格对应的表单摘要数据列表
        /// </summary>
        /// <param name="cell">指定的单元格</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>体征项列表</returns>
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
                string szValue1 = "体温";
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
                summaryData1.DataName = "体温";
                lstSummaryData.Add(summaryData1);
            }
            else if (gridViewColumn.ColumnTag == "血压")
            {
                string szValue1 = string.Empty;
                string szValue2 = string.Empty;

                int index = szCellValue.IndexOf('/');
                if (index > 0)
                    szValue1 = szCellValue.Substring(0, index).Trim();
                if (index >= 0 && index < szCellValue.Length - 1)
                    szValue2 = szCellValue.Substring(index + 1).Trim();

                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("血压high");
                summaryData.DataValue = szValue1;
                lstSummaryData.Add(summaryData);

                summaryData = summaryData.Clone() as SummaryData;
                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("血压Low");
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
