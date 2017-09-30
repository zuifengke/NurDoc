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

namespace Heren.NurDoc.PatPage.NurRecTableInput
{
    internal class NursingRecHandler
    {
        /// <summary>
        /// 用来支持批量录入数据的显示和编辑的表格控件
        /// </summary>
        protected DataTableView m_dataTableView = null;

        /// <summary>
        /// 用于存储批量录入列信息和列控件间关系的哈希表
        /// </summary>
        protected Hashtable m_htColumnTable = null;

        /// <summary>
        /// 用于缓存当前处于活动状态的列录入方案
        /// </summary>
        protected GridViewSchema m_schema = null;

        /// <summary>
        /// 护理记录评估单编辑器,用于临时打开列绑定的表单
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
        /// 保存所有更新的护理记录
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool SaveGridViewData()
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
                if (!row.IsModifiedRow && !row.IsDeletedRow && !row.IsNewRow)
                    continue;

                if (!this.SaveRowRecordData(row))
                {
                    this.m_dataTableView.SelectRow(row);
                    WorkProcess.Instance.Close();
                    MessageBoxEx.ShowError("护理记录数据保存失败!");
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

                    //特殊处理创建人列
                    DataGridViewColumn column = this.m_htColumnTable["创建人"] as DataGridViewColumn;
                    if (column != null && column.Index >= 0)
                    {
                        row.Cells[column.Index].Value = nursingRecInfo.CreatorName;
                    }

                    //特殊处理修改人列
                    column = this.m_htColumnTable["修改人"] as DataGridViewColumn;
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
        /// 获取指定行所有单元格对应的表单摘要数据列表
        /// </summary>
        /// <param name="row">指定的行</param>
        /// <returns>是否保存成功</returns>
        private bool SaveRowRecordData(DataGridViewRow row)
        {
            if (row == null || row.Index < 0)
                return false;
            NursingRecInfo nursingRecInfo = row.Tag as NursingRecInfo;
            if (nursingRecInfo == null)
                return false;
            //如果改行属于护理小结 则不需要保存
            if (nursingRecInfo.SummaryFlag == 1)
                return true;
            //表单摘要数据集合
            List<SummaryData> lstSummaryData = new List<SummaryData>();

            //将各单元格中数据转换为摘要数据,并按关联的表单类型分类
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

            //保存当前时间点的护理记录单及摘要数据
            short shRet = NurRecService.Instance.SaveNursingRec(nursingRecInfo.RecordTime, ref nursingRecInfo, lstSummaryData);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            row.Tag = nursingRecInfo;
            return true;
        }

        /// <summary>
        /// 体征处理前的摘要数据
        /// </summary>
        /// <param name="lstSummaryData">体征处理后的摘要数据</param>
        public void HandleSummaryDatas(ref List<SummaryData> lstSummaryData)
        {
            List<SummaryData> lstSummaryDataCopy = new List<SummaryData>();
            lstSummaryDataCopy.AddRange(lstSummaryData);
            List<SummaryData> lstSummaryDataAdd = new List<SummaryData>();
            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData.DataName == "体温" || summaryData.DataName == "呼吸" || summaryData.DataName == "脉搏"
                    || summaryData.DataName == "心率" || summaryData.DataName == "体重" || summaryData.DataName == "腋温" 
                    || summaryData.DataName == SystemContext.Instance.GetVitalSignsName("血压high")
                    || summaryData.DataName == SystemContext.Instance.GetVitalSignsName("血压Low"))
                {
                    summaryData.Category = 1;
                }
                string szRetValue = string.Empty;
                if (summaryData.DataName == "入量名称" && IsSummaryDatasContains(lstSummaryDataCopy, "入量量", ref szRetValue))
                {
                    SummaryData summaryRLData = summaryData.Clone() as SummaryData;
                    summaryRLData.DataName = summaryData.DataValue;
                    summaryRLData.DataValue = szRetValue;
                    summaryRLData.Category = 1;
                    summaryRLData.Remarks = "入量";
                    lstSummaryDataAdd.Add(summaryRLData);
                }
                else if (summaryData.DataName == "出量名称" && IsSummaryDatasContains(lstSummaryDataCopy, "出量量", ref szRetValue))
                {
                    SummaryData summaryCLData = summaryData.Clone() as SummaryData;
                    summaryCLData.DataName = summaryData.DataValue;
                    summaryCLData.DataValue = szRetValue;
                    summaryCLData.Category = 1;
                    summaryCLData.Remarks = "出量";
                    lstSummaryDataAdd.Add(summaryCLData);
                }
            }
            lstSummaryData.AddRange(lstSummaryDataAdd);
        }

        /// <summary>
        /// 判断在摘要列表中是否存在指定名称的摘要数据并返回摘要值
        /// </summary>
        /// <param name="lstSummaryData">摘要列表</param>
        /// <param name="name">查询名称</param>
        /// <param name="value">返回值</param>
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
        /// 创建一条新的护理记录主索引信息
        /// </summary>
        /// <param name="patVisit">病人就诊信息</param>
        /// <param name="dtRecordTime">记录时间</param>
        /// <returns>护理记录主索引信息</returns>
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
            summaryData.DocTypeID = gridViewColumn.SchemaID;//将配置SchemaID作为摘要的文档类型ID号
            summaryData.RecordID = nursingRecInfo.RecordID;

            if (gridViewColumn.ColumnTag == "记录内容")
            {
                nursingRecInfo.RecordContent = szCellValue;
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
                if (index < 0)
                {
                    szValue1 = szCellValue.ToString();
                }
                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("血压high");
                summaryData.DataValue = szValue1;
                lstSummaryData.Add(summaryData);

                summaryData = summaryData.Clone() as SummaryData;
                summaryData.DataName = SystemContext.Instance.GetVitalSignsName("血压Low");
                summaryData.DataValue = szValue2;
                lstSummaryData.Add(summaryData);
            }
            else if (gridViewColumn.ColumnTag == "创建人" || gridViewColumn.ColumnTag == "修改人"
                || gridViewColumn.ColumnTag == "入量累计" || gridViewColumn.ColumnTag == "出量累计")
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
