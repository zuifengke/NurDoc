// **************************************************************
// 护理电子病历系统之底层表单控件封装层,主要处理一些基础业务逻辑
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
        /// 获取或设置当前文档列表窗口关联的文档类型对象
        /// </summary>
        [Browsable(false)]
        public DocTypeInfo DocTypeInfo
        {
            get { return this.m_docTypeInfo; }
        }

        /// <summary>
        /// 打开指定的表单模板
        /// </summary>
        /// <param name="docTypeInfo">表单信息</param>
        /// <returns>是否打开成功</returns>
        public bool Load(DocTypeInfo docTypeInfo, byte[] byteFormData)
        {
            this.m_docTypeInfo = docTypeInfo;
            return base.Load(byteFormData);
        }

        /// <summary>
        /// 打开指定的表单模板
        /// </summary>
        /// <param name="docTypeInfo">表单信息</param>
        /// <returns>是否打开成功</returns>
        public bool OpenForm(DocTypeInfo docTypeInfo)
        {
            this.m_docTypeInfo = docTypeInfo;
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("表单打开失败,表单信息为空!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowErrorFormat("{0}模板内容下载失败!", null, docTypeInfo.DocTypeName);
                return false;
            }
            return this.Load(byteFormData);
        }

        /// <summary>
        /// 保存当前表单内容
        /// </summary>
        /// <returns>是否保存成功</returns>
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
        /// 将脚本中的KeyData对象列表转换为摘要数据对象列表
        /// </summary>
        /// <param name="keyDataList">表单关键数据列表</param>
        /// <returns>摘要数据列表</returns>
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

        #region"表单打印"
        /// <summary>
        /// 打印当前表单
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
            explorerForm.ReportParamData.Add("是否续打", bIsContinuedPrint);
            explorerForm.ReportParamData.Add("打印数据", this.ExportXml(true));
            explorerForm.ShowDialog();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 获取打印报表模板内容
        /// </summary>
        /// <param name="szReportName">打印报表模板名称</param>
        /// <returns>byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            this.Update();
            if (this.m_docTypeInfo == null)
            {
                MessageBoxEx.ShowError("无法获取文档类型信息!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_DOC_FORM;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.m_docTypeInfo.DocTypeName;
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

        protected override void OnQueryContext(QueryContextEventArgs e)
        {
            base.OnQueryContext(e);
            if (e.Success)
                return;
            if (e.Name == "表单类型代码")
            {
                if (this.m_docTypeInfo != null)
                    e.Value = this.m_docTypeInfo.DocTypeID;
                e.Success = true;
                return;
            }
            if (e.Name == "表单类型名称")
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
            if (e.Name == "导入文本模板")
            {
                e.Result = UtilitiesHandler.Instance.ShowTextTempletForm();
            }
            else if (e.Name == "导入医嘱记录")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowOrdersImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "导入检查记录")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowExamImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "导入检验记录")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowTestImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "导入护理记录")
            {
                string szPatientID = e.Param == null ? string.Empty : e.Param.ToString();
                string szVisitID = e.Data == null ? string.Empty : e.Data.ToString();
                e.Result = UtilitiesHandler.Instance.ShowNurRecImportForm(szPatientID, szVisitID);
            }
            else if (e.Name == "导入护理记录默认日期")
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
            else if (e.Name == "模板内打印")
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
                explorerForm.ReportParamData.Add("打印数据", e.Param);
                explorerForm.ShowDialog();
            }
            else if (e.Name == "浏览文档信息")
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
            else if (e.Name == "导入食物含水量")
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
                MessageBoxEx.ShowError("后台模板编写有误,请求的表单信息不能为空!");
                return;
            }

            //参数e.FormInfo.ID由脚本中传出,表示需要调用的子表单的类型ID号
            //参数e.FormInfo.Caller由脚本中传出,表示当前表单中是哪个控件发出的请求
            string szDocTypeID = e.FormInfo.ID;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowErrorFormat("没有找到ID号“{0}”对应的模板!", null, szDocTypeID);
                return;
            }
            FormEditForm form = new FormEditForm(docTypeInfo);

            //参数e.Data由脚本中传出,用来传给子表单,让子表单初始显示一些数据
            form.FormEditor.UpdateFormData("表单数据", e.Data);
            if (form.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                //参数e.Data经子表单处理后,再返回给当前表单,当前表单再更新自己的显示
                e.Cancel = false;
                e.Data = form.FormEditor.GetFormData("表单数据");
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
