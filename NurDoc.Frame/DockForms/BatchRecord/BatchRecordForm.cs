// ***********************************************************
// 护理电子病历系统,病人体征批量录入模块.
// Creator:ZhangLian  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame.DockForms;
using Heren.NurDoc.Frame.Dialogs;
using Heren.NurDoc.Utilities.Forms;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class BatchRecordForm : DockContentBase
    {
        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        /// <returns>修改状态</returns>
        public override bool IsModified
        {
            get
            {
                foreach (DataTableViewRow row in this.dataTableView1.Rows)
                {
                    if (row != null && row.IsModifiedRow)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 批量录入数据的加载和保存处理器
        /// </summary>
        private BatchRecordHandler m_batchRecordHandler = null;

        public BatchRecordForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            if (!SystemContext.Instance.SystemOption.OptionBatchRecordPrint)
                this.toolbtnPrint.Visible = false;

            string text = SystemContext.Instance.WindowName.BatchRecord;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 400);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.RefreshView();
        }

        public override void RefreshView()
        {
            base.RefreshView();
            this.Update();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //加载体征项目列方案
            this.LoadColumnSchemaList();

            DateTime Now = SysTimeService.Instance.Now;
            this.tooldtpRecordDate.Value = Now.Date;
            this.tooldtpRecordTime.Value = this.GetTimeNoSeconds(Now);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public override bool CheckModifyState()
        {
            GridViewSchema shcema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            if (shcema == null)
                return false;
            if (shcema.SchemaFlag == ServerData.SchemaFlag.NUR_SCRIPT_TIME)
            {
                if (this.formControl1 != null && this.formControl1.IsModified)
                {
                    DialogResult result = MessageBoxEx.ShowQuestion("体征记录已经被修改,是否保存？");
                    if (result == DialogResult.Yes)
                    {
                        object time = this.GetTimeNoSeconds(this.tooldtpRecordTime.Value);
                        this.formControl1.SaveFormData(ref time);
                    }
                    return (result == DialogResult.Cancel);
                }
                return false;
            }
            else
            {
                if (!this.IsModified)
                    return false;

                DialogResult result = MessageBoxEx.ShowQuestion("体征记录已经被修改,是否保存？");
                if (result == DialogResult.Yes)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                    DateTime dtRecordTime = this.GetCurrentRecordTime();
                    bool success = this.m_batchRecordHandler.SaveGridViewData(dtRecordTime, shcema.RelatvieSchemaId);
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return !success;
                }
                return (result == DialogResult.Cancel);
            }
        }

        /// <summary>
        /// 加载缺省的列显示方案
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadColumnSchemaList()
        {
            this.toolcboSchemaList.Items.Clear();
            this.Update();
            if (SystemContext.Instance.LoginUser == null)
                return false;

            //获取列显示方案列表
            string szSchemaType = ServerData.GridViewSchema.BATCH_RECORD;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<GridViewSchema> lstGridViewSchemas = null;
            short shRet = ConfigService.Instance.GetGridViewSchemas(szSchemaType, szWardCode, ref lstGridViewSchemas);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.ShowError("列显示方案列表下载失败!");
                return false;
            }
            if (lstGridViewSchemas == null)
                lstGridViewSchemas = new List<GridViewSchema>();

            //检查是否有私有方案
            bool bHasPrivate = false;
            foreach (GridViewSchema schema in lstGridViewSchemas)
            {
                if (schema != null
                    && !GlobalMethods.Misc.IsEmptyString(schema.WardCode))
                {
                    bHasPrivate = true;
                    break;
                }
            }

            //加载列显示方案列表
            GridViewSchema defaultSchema = null;
            foreach (GridViewSchema schema in lstGridViewSchemas)
            {
                if (schema == null)
                    continue;
                if (bHasPrivate && GlobalMethods.Misc.IsEmptyString(schema.WardCode))
                    continue;
                if (schema.IsDefault)
                    defaultSchema = schema;
                this.toolcboSchemaList.Items.Add(schema);
            }
            this.toolcboSchemaList.SelectedItem = defaultSchema;
            if (this.toolcboSchemaList.SelectedIndex < 0 && this.toolcboSchemaList.Items.Count > 0)
                this.toolcboSchemaList.SelectedIndex = 0;
            return true;
        }

        /// <summary>
        /// 加载指定的方案对应的时间点列表
        /// </summary>
        /// <param name="schema">列显示方案</param>
        /// <returns>是否加载成功</returns>
        private bool LoadTimePointList(GridViewSchema schema)
        {
            this.toolcboRecordTime.Items.Clear();

            if (schema == null)
                return false;
            int[] arrTimePoints = SystemContext.Instance.TimePointTable[schema.SchemaName] as int[];
            if (arrTimePoints == null)
                arrTimePoints = new int[0];
            object[] items = new object[arrTimePoints.Length];
            arrTimePoints.CopyTo(items, 0);
            this.toolcboRecordTime.Items.AddRange(items);
            return true;
        }

        /// <summary>
        /// 切换批量录入列显示方案
        /// </summary>
        /// <param name="schema">方案信息</param>
        /// <returns>是否切换成功</returns>
        private bool SwitchSchema(GridViewSchema schema)
        {
            if (schema == null)
                return false;
            this.LoadTimePointList(schema);

            this.m_batchRecordHandler =
                BatchRecordHandler.CreateBatchHandler(this.dataTableView1, schema);
            if (!this.m_batchRecordHandler.LoadGridViewColumns(schema))
                return false;

            if (this.toolcboRecordTime.SelectedIndex < 0)
            {
                this.dataTableView1.Rows.Clear();
            }
            else
            {
                if (this.LoadPatVisitList(null))
                    this.m_batchRecordHandler.LoadGridViewData(this.GetCurrentRecordTime());
            }
            this.toolcboRecordTime.Text = string.Empty;
            return true;
        }

        /// <summary>
        /// 获取当前体征记录的时间
        /// </summary>
        /// <returns>体征记录时间</returns>
        private DateTime GetCurrentRecordTime()
        {
            DateTime dtRecordTime = SysTimeService.Instance.Now;
            if (this.tooldtpRecordDate.Value != null)
                dtRecordTime = this.tooldtpRecordDate.Value;

            string szTimePoint = SysTimeService.Instance.Now.Hour.ToString();
            if (this.toolcboRecordTime.SelectedItem != null)
                szTimePoint = this.toolcboRecordTime.SelectedItem.ToString();

            int nTimePoint = dtRecordTime.Hour;
            int.TryParse(szTimePoint, out nTimePoint);
            return new DateTime(dtRecordTime.Year, dtRecordTime.Month, dtRecordTime.Day, nTimePoint, 0, 0);
        }

        /// <summary>
        /// 获取当前体征记录的时间
        /// </summary>
        /// <returns>体征记录时间</returns>
        private DateTime GetTimeNoSeconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }

        /// <summary>
        /// 根据列配置标记 修改界面是否显示时间点还是显示时间框具体时间  并显示列配置
        /// </summary>
        /// <param name="schema"></param>
        private void HandleUILayout(GridViewSchema schema)
        {
            if (schema.SchemaFlag == ServerData.SchemaFlag.NUR_SCRIPT_TIME)
            {
                this.tooldtpRecordDate.Visible = false;
                this.tooldtpRecordTime.Visible = true;
                this.toollblRecordTime.Visible = false;
                this.toolcboRecordTime.Visible = false;
                this.toolbtnFilterPatient.Visible = false;
                this.dataTableView1.Visible = false;

                this.LoadFilterTemplet(schema);
                this.formControl1.Visible = true;
            }
            else
            {
                this.tooldtpRecordDate.Visible = true;
                this.tooldtpRecordTime.Visible = false;
                this.toollblRecordTime.Visible = true;
                this.toolcboRecordTime.Visible = true;
                this.toolbtnFilterPatient.Visible = true;
                this.dataTableView1.Visible = true;
                this.formControl1.Visible = false;
            }
        }

        /// <summary>
        /// 加载病人筛选表单
        /// </summary>
        /// <returns>是否成功</returns>
        private bool LoadFilterTemplet(GridViewSchema schema)
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.BATCH_RECORD_FILTER;
            DocTypeInfo docTypeInfo = null;
            if (schema == null)
            {
                docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            }
            else
            {
                string szTempletName = schema.SchemaName;
                docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            }
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("病人列表筛选模板还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("病人列表筛选模板内容下载失败!");
                return false;
            }
            bool success = this.formControl1.Load(byteFormData);
            this.formControl1.UpdateFormData("更新体征时间", this.GetTimeNoSeconds(this.tooldtpRecordTime.Value));
            return success;
        }

        /// <summary>
        /// 加载指定的病人列表到当前批量录入表格
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadPatVisitList(PatVisitInfo[] lstPatVisits)
        {
            this.dataTableView1.Focus();
            this.dataTableView1.Rows.Clear();
            this.Update();
            if (lstPatVisits == null)
            {
                BatchRecFilterForm patientFilterForm = new BatchRecFilterForm();
                patientFilterForm.Schema =
                    this.toolcboSchemaList.SelectedItem as GridViewSchema;
                patientFilterForm.RecordTime = this.GetCurrentRecordTime();
                if (patientFilterForm.LoadFilterTemplet())
                    lstPatVisits = patientFilterForm.SelectedPatients;
                patientFilterForm.Dispose();
            }
            if (lstPatVisits == null || lstPatVisits.Length <= 0)
                return false;

            for (int index = 0; index < lstPatVisits.Length; index++)
            {
                PatVisitInfo patVisitInfo = lstPatVisits[index];
                if (patVisitInfo == null)
                    continue;
                int rowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[rowIndex];

                row.Tag = patVisitInfo;
                row.Cells[this.colBedNo.Index].Value = patVisitInfo.BedCode;
                row.Cells[this.colPatientName.Index].Value = patVisitInfo.PatientName;
                row.Cells[this.colPatientID.Index].Value = patVisitInfo.PatientId;
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            this.dataTableView1.RefreshRowColor();
            return true;
        }

        /// <summary>
        /// 获取系统上下文
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool GetSystemContext(string name, ref object value)
        {
            if (name == "体征时间")
            {
                value = this.GetTimeNoSeconds(this.tooldtpRecordTime.Value);
                return true;
            }
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        /// <summary>
        /// 当时间点不在标准版血压时间点时 隐藏血压列
        /// </summary>
        private void ColumnBPHandler()
        {
            DateTime dtRecordTime = SysTimeService.Instance.Now;
            string szTimePoint = dtRecordTime.Hour.ToString();
            if (this.toolcboRecordTime.SelectedItem != null)
                szTimePoint = this.toolcboRecordTime.SelectedItem.ToString();

            int nTimePoint = dtRecordTime.Hour;
            int.TryParse(szTimePoint, out nTimePoint);
            foreach (DataGridViewColumn column in this.dataTableView1.Columns)
            {
                if (column.Index <= 2)
                    continue;
                GridViewColumn gridViewColumn = column.Tag as GridViewColumn;
                if (gridViewColumn.ColumnTag == "血压" && !SystemContext.Instance.IsTimeInStandardList(nTimePoint))
                    column.Visible = false;
                else
                    column.Visible = true;
            }
        }

        #region"验证数据合法性"
        /// <summary>
        /// 验证数值数据类型单元格合法性
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="value">验证值</param>
        /// <returns>是否验证成功</returns>
        private bool ValidateNumericCellValue(DataGridViewCell cell, object value)
        {
            if (GlobalMethods.Misc.IsEmptyString(value))
                return true;
            string szCellValue = GlobalMethods.Convert.SBCToDBC(value.ToString()).Trim();
            string szNumericText = "0123456789.";
            foreach (char ch in szCellValue)
            {
                if (!szNumericText.Contains(ch.ToString()))
                {
                    MessageBoxEx.ShowError("请正确输入数值数据!");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证呼吸数据类型单元格合法性
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="value">验证值</param>
        /// <returns>是否验证成功</returns>
        private bool ValidateBreathCellValue(DataGridViewCell cell, object value)
        {
            if (GlobalMethods.Misc.IsEmptyString(value))
                return true;
            string szCellValue = GlobalMethods.Convert.SBCToDBC(value.ToString()).Trim();

            DataGridViewColumn column = this.dataTableView1.Columns[cell.ColumnIndex];
            if (this.ValidateNumericCellValue(cell, szCellValue))
            {
                double BreathNum = Double.Parse(szCellValue);
                if (BreathNum > 200)
                {
                    MessageBoxEx.ShowError("呼吸超出200次/分");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证心率数据类型单元格合法性
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="value">验证值</param>
        /// <returns>是否验证成功</returns>
        private bool ValidateHeartRateCellValue(DataGridViewCell cell, object value)
        {
            if (GlobalMethods.Misc.IsEmptyString(value))
                return true;
            string szCellValue = GlobalMethods.Convert.SBCToDBC(value.ToString()).Trim();

            DataGridViewColumn column = this.dataTableView1.Columns[cell.ColumnIndex];
            if (this.ValidateNumericCellValue(cell, szCellValue))
            {
                double HeartRateNum = Double.Parse(szCellValue);
                if (HeartRateNum > 500)
                {
                    MessageBoxEx.ShowError("心率超出500次/分");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证脉搏数据类型单元格合法性
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="value">验证值</param>
        /// <returns>是否验证成功</returns>
        private bool ValidatePulseCellValue(DataGridViewCell cell, object value)
        {
            if (GlobalMethods.Misc.IsEmptyString(value))
                return true;
            string szCellValue = GlobalMethods.Convert.SBCToDBC(value.ToString()).Trim();

            DataGridViewColumn column = this.dataTableView1.Columns[cell.ColumnIndex];
            if (this.ValidateNumericCellValue(cell, szCellValue))
            {
                double PulseNum = Double.Parse(szCellValue);
                if (PulseNum > 500)
                {
                    MessageBoxEx.ShowError("脉搏超出500次/分！");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证体温数据类型单元格合法性
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="value">验证值</param>
        /// <returns>是否验证成功</returns>
        private bool ValidateTemperatureCellValue(DataGridViewCell cell, object value)
        {
            if (GlobalMethods.Misc.IsEmptyString(value))
                return true;
            string szCellValue = GlobalMethods.Convert.SBCToDBC(value.ToString()).Trim();

            DataGridViewColumn column = this.dataTableView1.Columns[cell.ColumnIndex];
            if (column as ComboBoxTextBoxColumn == null)
                return true;

            char separator = ((ComboBoxTextBoxColumn)column).ValueSeparator;
            if (szCellValue == separator.ToString())
                return true;

            int index = szCellValue.IndexOf(separator);
            if (index <= 0)
            {
                MessageBoxEx.ShowError("请正确输入体温类型和数据!");
                return false;
            }
            if (index >= szCellValue.Length - 1)
                return true;
            if (index >= 0 && this.ValidateNumericCellValue(cell, szCellValue.Substring(index + 1)))
            {
                string[] num = szCellValue.Split(separator);
                double temperature = Double.Parse(num[1]);
                if (temperature > 46.5 || temperature < 14.2)
                {
                    MessageBoxEx.ShowError("体温应介于14.2℃和46.5℃之间!");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证血压数据类型单元格合法性
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="value">验证值</param>
        /// <returns>是否验证成功</returns> 
        private bool ValidateBPCellValue(DataGridViewCell cell, object value)
        {
            if (GlobalMethods.Misc.IsEmptyString(value))
                return true;
            string szCellValue = GlobalMethods.Convert.SBCToDBC(value.ToString()).Trim();
            if (szCellValue.Trim() == "/" || szCellValue.Trim() == "未测")
                return true;
            int index = szCellValue.IndexOf('/');
            if (index > 0 && index < szCellValue.Length - 1
                && this.ValidateNumericCellValue(cell, szCellValue.Substring(0, index)))
            {
                return this.ValidateNumericCellValue(cell, szCellValue.Substring(index + 1));
            }
            else
            {
                MessageBoxEx.ShowError("血压输入格式是“收缩压/舒张压”,请确认格式及数据是否正确!");
                return false;
            }
        }
        #endregion

        #region "打印批量记录单"
        /// <summary>
        /// 加载打印批量记录单打印报表
        /// </summary>
        /// <param name="szReportName">报表数据名称</param>
        /// <returns>报表数据</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            string szApplyEnv = ServerData.ReportTypeApplyEnv.BATCH_RECORD;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.toolcboSchemaList.Text.Trim();
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

        private void toolcboSchemaList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.HandleUILayout(this.toolcboSchemaList.SelectedItem as GridViewSchema);
            this.SwitchSchema(this.toolcboSchemaList.SelectedItem as GridViewSchema);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void tooldtpRecordDate_ValueChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.toolcboRecordTime.SelectedIndex >= 0)
            {
                if (this.LoadPatVisitList(null) && this.m_batchRecordHandler != null)
                    this.m_batchRecordHandler.LoadGridViewData(this.GetCurrentRecordTime());
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboRecordTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (SystemContext.Instance.SystemOption.EmrsVital)
                ColumnBPHandler();
            if (this.LoadPatVisitList(null) && this.m_batchRecordHandler != null)
                this.m_batchRecordHandler.LoadGridViewData(this.GetCurrentRecordTime());
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnFilterPatient_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            BatchRecFilterForm patientFilterForm = new BatchRecFilterForm();
            patientFilterForm.Schema =
                this.toolcboSchemaList.SelectedItem as GridViewSchema;
            patientFilterForm.RecordTime = this.GetCurrentRecordTime();

            if (patientFilterForm.ShowDialog() != DialogResult.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            if (!this.LoadPatVisitList(patientFilterForm.SelectedPatients))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            if (this.m_batchRecordHandler != null)
                this.m_batchRecordHandler.LoadGridViewData(this.GetCurrentRecordTime());
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            if (!RightController.Instance.UserRight.EditVitalSigns.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限修改体征记录!");
                return;
            }
            GridViewSchema shcema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            if (shcema.SchemaFlag == ServerData.SchemaFlag.NUR_SCRIPT_TIME)
            {
                object time = this.GetTimeNoSeconds(this.tooldtpRecordTime.Value);
                this.formControl1.SaveFormData(ref time);
            }
            else
            {
                //判断是否有选择时刻点
                if (this.toolcboRecordTime.SelectedIndex < 0)
                {
                    MessageBoxEx.ShowWarning("保存前请选择时间点!");
                    return;
                }
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                if (this.m_batchRecordHandler != null)
                {
                    this.m_batchRecordHandler.SaveGridViewData(this.GetCurrentRecordTime(), shcema.RelatvieSchemaId);
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnPrint_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (!RightController.Instance.UserRight.PrintVitalSigns.Value)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.ShowWarning("您没有权限打印体征记录!");
                return;
            }
            GridViewSchema shcema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            if (shcema.SchemaFlag == ServerData.SchemaFlag.NUR_SCRIPT_TIME)
            {
                byte[] byteReportData = this.GetReportFileData(null);
                if (byteReportData == null)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("打印数据", this.formControl1.ExportXml(true));
                explorerForm.ShowDialog();
            }
            else
            {
                if (this.dataTableView1.Rows.Count <= 0)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                byte[] byteReportData = this.GetReportFileData(null);
                if (byteReportData == null)
                {
                    GlobalMethods.UI.SetCursor(this, Cursors.Default);
                    return;
                }
                DataTable table = GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 1);
                Heren.Common.Report.ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("是否续打", false);
                explorerForm.ReportParamData.Add("打印数据", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.dataTableView1.BeginEdit(true);
        }

        private void dataTableView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == this.colBedNo.Index)
                e.Cancel = true;
            else if (e.ColumnIndex == this.colPatientID.Index)
                e.Cancel = true;
            else if (e.ColumnIndex == this.colPatientName.Index)
                e.Cancel = true;
        }

        private void dataTableView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            DataTableViewRow row = this.dataTableView1.Rows[e.RowIndex];
            DataGridViewColumn column = this.dataTableView1.Columns[e.ColumnIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            string szCellValue = string.Empty;
            if (cell.Value != null)
                szCellValue = cell.Value.ToString();

            //记忆最后一次选择的体温类型
            if (column is ComboBoxTextBoxColumn)
            {
                string szRecordName = szCellValue;
                ComboBoxTextBoxColumn comboBoxTextBoxColumn = (ComboBoxTextBoxColumn)column;
                char separator = comboBoxTextBoxColumn.ValueSeparator;
                int separatorIndex = szCellValue.IndexOf(separator);
                if (separatorIndex >= 0 && separatorIndex < szCellValue.Length)
                    szRecordName = szCellValue.Substring(0, separatorIndex);

                int index = comboBoxTextBoxColumn.Items.IndexOf(szRecordName);
                if (index >= 0)
                    comboBoxTextBoxColumn.DefaultSelectedIndex = index;
            }
        }

        private void dataTableView1_NumericValidating(object sender, NumericValidatingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
            DataGridViewColumn column = this.dataTableView1.Columns[e.ColumnIndex];
            GridViewColumn gridViewColumn = column.Tag as GridViewColumn;
            if (gridViewColumn == null)
                return;
            if (gridViewColumn.ColumnType == ServerData.DataType.NUMERIC)
                e.IsNumeric = true;
        }

        private void dataTableView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.Cancel || e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            DataGridViewRow row = this.dataTableView1.Rows[e.RowIndex];
            DataGridViewColumn column = this.dataTableView1.Columns[e.ColumnIndex];
            GridViewColumn gridViewColumn = column.Tag as GridViewColumn;
            if (gridViewColumn == null)
                return;

            string szColumnTag = gridViewColumn.ColumnTag;
            if (GlobalMethods.Misc.IsEmptyString(szColumnTag))
                szColumnTag = gridViewColumn.ColumnName;

            if (szColumnTag == "血压")
            {
                e.Cancel = !this.ValidateBPCellValue(row.Cells[column.Index], e.FormattedValue);
            }
            else if (column as ComboBoxTextBoxColumn != null)
            {
                e.Cancel = !this.ValidateTemperatureCellValue(row.Cells[column.Index], e.FormattedValue);
            }
            else if (szColumnTag == "呼吸")
            {
                e.Cancel = !this.ValidateBreathCellValue(row.Cells[column.Index], e.FormattedValue);
            }
            else if (szColumnTag == "心率")
            {
                e.Cancel = !this.ValidateHeartRateCellValue(row.Cells[column.Index], e.FormattedValue);
            }
            else if (szColumnTag == "脉搏")
            {
                e.Cancel = !this.ValidatePulseCellValue(row.Cells[column.Index], e.FormattedValue);
            }
            else if (gridViewColumn.ColumnType == ServerData.DataType.NUMERIC)
            {
                e.Cancel = !this.ValidateNumericCellValue(row.Cells[column.Index], e.FormattedValue);
            }
        }

        private void dataTableView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.dataTableView1.RefreshRowColor();
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            if (e.Name == "体征记录时间")
            {
                GridViewSchema schema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
                if (schema.SchemaFlag == ServerData.SchemaFlag.NUR_SCRIPT_TIME)
                    e.Value = this.GetTimeNoSeconds(this.tooldtpRecordTime.Value);
                else
                    e.Value = this.GetCurrentRecordTime();
                e.Success = true;
                return;
            }
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

        private void tooldtpRecordTime_ValueChanged(object sender, EventArgs e)
        {
            GridViewSchema schema = this.toolcboSchemaList.SelectedItem as GridViewSchema;
            if (this.formControl1.Visible && schema.SchemaFlag == ServerData.SchemaFlag.NUR_SCRIPT_TIME)
            {
                this.formControl1.UpdateFormData("更新体征时间", this.GetTimeNoSeconds(this.tooldtpRecordTime.Value));
            }
        }

        private void formControl1_QueryContext(object sender, Heren.Common.Forms.Editor.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;

        }

        private void formControl1_ExecuteQuery(object sender, Heren.Common.Forms.Editor.ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            if (CommonService.Instance.ExecuteQuery(e.SQL, out result) == SystemConst.ReturnValue.OK)
            {
                e.Success = true;
                e.Result = result;
            }
        }

        private void formControl1_ExecuteUpdate(object sender, Heren.Common.Forms.Editor.ExecuteUpdateEventArgs e)
        {
            //CommonService.Instance.ExecuteUpdate(false
        }
    }
}
