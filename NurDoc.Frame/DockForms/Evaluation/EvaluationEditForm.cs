// ***********************************************************
// 护理电子病历系统,表单文档编辑窗口.
// Creator:YangMingkun  Date:2012-7-2
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.Common.Forms.Editor;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.Common.Forms.Loader;
using Heren.Common.Report;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class EvaluationEditForm : HerenForm
    {
        private EvaDocInfo m_evaDocInfo = null;

        /// <summary>
        /// 获取当前表单编辑器绑定病历文档信息
        /// </summary>
        [Browsable(false)]
        public EvaDocInfo EvaDocInfo
        {
            get { return this.m_evaDocInfo; }
            //set { this.m_evaDocInfo = value; }
        }

        private EvaTypeInfo m_evaTypeInfo = null;

        public EvaTypeInfo EvaTypeInfo
        {
            get { return this.m_evaTypeInfo; }
            //set { this.m_evaTypeInfo = value; }
        }

        /// <summary>
        /// 获取当前窗口数据是否已经被修改
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get
            {
                return this.dataTableView1.HasModified();
            }
        }

        /// <summary>
        /// 当文档被更新完成时触发
        /// </summary>
        [Description("当文档被更新完成时触发")]
        public event EventHandler DocumentUpdated;

        protected virtual void OnDocumentUpdated(EventArgs e)
        {
            if (this.DocumentUpdated != null)
                this.DocumentUpdated(this, e);
        }

        /// <summary>
        /// 获取当前文档编辑器表单是否更新过
        /// </summary>
        private bool m_documentUpdated = false;

        public EvaluationEditForm()
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.SysIcon;

            DateTime dtNow = SysTimeService.Instance.Now;
            dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day
                , dtNow.Hour, dtNow.Minute, 0);
            this.tooldtpRecordTime.Value = dtNow;
        }

        new public void Hide()
        {
            if (!this.CheckModifyState())
            {
                base.Hide();
                for (int i = 0; i < this.dataTableView1.Rows.Count; i++)
                    this.dataTableView1.SetRowState(i, RowState.Normal);
                if (this.m_documentUpdated)
                    this.OnDocumentUpdated(EventArgs.Empty);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
            if (!this.Modal || this.m_evaDocInfo == null)
                return;
        }

        private bool CheckRight()
        {
            NurUserRight userRight = RightController.Instance.UserRight;
            if (userRight.LeaderNurse.Value || userRight.HeadNurse.Value || userRight.HigherNurse.Value)
                return true;
            return false;
        }

        /// <summary>
        /// 将当前病历数据另存为模板
        /// </summary>
        /// <returns>是否成功</returns>
        public bool SaveDocumentAs(DataTable dtDeptInfo)
        {
            if (dtDeptInfo == null || dtDeptInfo.Rows.Count <= 0)
            {
                MessageBoxEx.ShowWarning("当前没有选中需要另存为模板的护理单元!");
                return false;
            }
            if (this.m_evaDocInfo == null)
                return false;
            Application.DoEvents();

            if (!this.CheckRight())
            {
                MessageBoxEx.Show("当前登录护士无保存权限！");
                return false;
            }
            Form parentForm = this.dataTableView1.FindForm();
            if (parentForm == null || parentForm.IsDisposed)
                return false;
            parentForm.Update();
            int nRowsCount = dtDeptInfo.Rows.Count;
            WorkProcess.Instance.Initialize(parentForm, nRowsCount, "正在另存为模板数据，请稍候...");
            for (int i = 0; i < dtDeptInfo.Rows.Count; i++)
            {
                WorkProcess.Instance.Show(i, false);

                this.m_evaDocInfo.WardCode = dtDeptInfo.Rows[i][0].ToString();
                this.m_evaDocInfo.WardName = dtDeptInfo.Rows[i][1].ToString();
                this.m_evaDocInfo.EvaID = this.m_evaDocInfo.MakeEvaID();
                EvaDocInfo docInfo = null;
                short shRet = EvaDocInfoService.Instance.GetEvaDocInfo(this.m_evaDocInfo.EvaID, ref docInfo);
                if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    //新增文档
                    this.m_evaDocInfo.ModifyTime = this.tooldtpRecordTime.Value;
                    if (GlobalMethods.Misc.IsEmptyString(this.tooltxtScore.Text))
                        this.m_evaDocInfo.Score = 0;
                    else
                    {
                        decimal value = 0;
                        if (decimal.TryParse(this.tooltxtScore.Text, out value))
                            this.m_evaDocInfo.Score = value;
                    }
                    this.m_evaDocInfo.Remark = this.tooltxtRemark.Text;
                    List<EvaDocItemInfo> datas = null;
                    this.GetItemData(this.m_evaDocInfo.EvaID, ref datas);
                    shRet = EvaDocInfoService.Instance.AddEvaDocDatas(this.m_evaDocInfo, datas);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        WorkProcess.Instance.Close();
                        return false;
                    }
                }
                else if (shRet == ServerData.ExecuteResult.OK)
                {
                    if (docInfo.Status == ServerData.DocStatus.CANCELED)
                    {
                        WorkProcess.Instance.Close();
                        MessageBoxEx.ShowWarning("该页评估信息已被他人修改过，请刷新列表重新打开!");
                        return false;
                    }
                    else
                    {
                        EvaDocInfo newDocInfo = docInfo.Clone() as EvaDocInfo;
                        newDocInfo.Version = docInfo.Version + 1;
                        newDocInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
                        newDocInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
                        newDocInfo.ModifyTime = this.tooldtpRecordTime.Value;
                        newDocInfo.Status = ServerData.DocStatus.NORMAL;
                        newDocInfo.EvaID = newDocInfo.MakeEvaID(this.m_evaDocInfo.EvaID);
                        if (GlobalMethods.Misc.IsEmptyString(this.tooltxtScore.Text))
                            newDocInfo.Score = 0;
                        else
                        {
                            decimal value = 0;
                            if (decimal.TryParse(this.tooltxtScore.Text, out value))
                                newDocInfo.Score = value;
                        }
                        newDocInfo.Remark = this.tooltxtRemark.Text;
                        List<EvaDocItemInfo> datas = null;
                        this.GetItemData(newDocInfo.EvaID, ref datas);
                        shRet = EvaDocInfoService.Instance.UpdateEvaDocDatas(docInfo, newDocInfo, datas);
                        if (shRet != ServerData.ExecuteResult.OK)
                        {
                            WorkProcess.Instance.Close();
                            return false;
                        }
                        this.m_evaDocInfo = newDocInfo;
                    }
                }
                else
                {
                    WorkProcess.Instance.Close();
                    return false;
                }
            }
            this.m_documentUpdated = true;
            this.SetRowsNormal();
            WorkProcess.Instance.Close();
            return true;
        }

        /// <summary>
        /// 保存当前病历数据
        /// </summary>
        /// <returns>是否成功</returns>
        public bool SaveDocument()
        {
            if (this.m_evaDocInfo == null)
                return false;
            Application.DoEvents();

            this.dataTableView1.EndEdit();
            if (!this.CheckRight())
            {
                MessageBoxEx.Show("当前登录护士无保存权限！");
                return false;
            }

            EvaDocInfo docInfo = null;
            short shRet = EvaDocInfoService.Instance.GetEvaDocInfo(this.m_evaDocInfo.EvaID, ref docInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
            {
                //新增文档
                this.m_evaDocInfo.ModifyTime = this.tooldtpRecordTime.Value;
                if (GlobalMethods.Misc.IsEmptyString(this.tooltxtScore.Text))
                    this.m_evaDocInfo.Score = 0;
                else
                {
                    decimal value = 0;
                    if (decimal.TryParse(this.tooltxtScore.Text, out value))
                        this.m_evaDocInfo.Score = value;
                }
                this.m_evaDocInfo.Remark = this.tooltxtRemark.Text;
                List<EvaDocItemInfo> datas = null;
                this.GetItemData(this.m_evaDocInfo.EvaID, ref datas);
                shRet = EvaDocInfoService.Instance.AddEvaDocDatas(this.m_evaDocInfo, datas);
                if (shRet != ServerData.ExecuteResult.OK)
                    return false;
            }
            else if (shRet == ServerData.ExecuteResult.OK)
            {
                if (docInfo.Status == ServerData.DocStatus.CANCELED)
                {
                    MessageBoxEx.ShowWarning("该页评估信息已被他人修改过，请刷新列表重新打开!");
                    return false;
                }
                else
                {
                    EvaDocInfo newDocInfo = docInfo.Clone() as EvaDocInfo;
                    newDocInfo.Version = docInfo.Version + 1;
                    newDocInfo.ModifierID = SystemContext.Instance.LoginUser.ID;
                    newDocInfo.ModifierName = SystemContext.Instance.LoginUser.Name;
                    newDocInfo.ModifyTime = this.tooldtpRecordTime.Value;
                    newDocInfo.EvaID = newDocInfo.MakeEvaID(this.m_evaDocInfo.EvaID);
                    if (GlobalMethods.Misc.IsEmptyString(this.tooltxtScore.Text))
                        newDocInfo.Score = 0;
                    else
                    {
                        decimal value = 0;
                        if (decimal.TryParse(this.tooltxtScore.Text, out value))
                            newDocInfo.Score = value;
                    }
                    newDocInfo.Remark = this.tooltxtRemark.Text;
                    List<EvaDocItemInfo> datas = null;
                    this.GetItemData(newDocInfo.EvaID, ref datas);
                    shRet = EvaDocInfoService.Instance.UpdateEvaDocDatas(docInfo, newDocInfo, datas);
                    if (shRet != ServerData.ExecuteResult.OK)
                        return false;
                    this.m_evaDocInfo = newDocInfo;
                }
            }
            else
                return false;
            this.m_documentUpdated = true;
            this.SetRowsNormal();
            return true;
        }

        private bool GetItemData(string szEvaID, ref List<EvaDocItemInfo> datas)
        {
            DataGridViewTextBoxCell TextboxCell;
            DataGridViewCheckBoxCell CheckboxCell;
            datas = new List<EvaDocItemInfo>();
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                EvaTypeItem item = row.Tag as EvaTypeItem;
                if (item == null)
                    continue;
                EvaDocItemInfo data = new EvaDocItemInfo();
                data.EvaID = szEvaID;
                data.ItemNo = item.ItemNo;
                if (item.ItemType == "输入框")
                {
                    TextboxCell = row.Cells[this.colValue.Index] as DataGridViewTextBoxCell;
                    if (TextboxCell.Value != null)
                        data.ItemValue = TextboxCell.Value.ToString();
                }
                else
                {
                    CheckboxCell = row.Cells[this.colValue.Index] as DataGridViewCheckBoxCell;
                    if ((bool)CheckboxCell.EditedFormattedValue == true)
                        data.ItemValue = "TRUE";
                    else
                        data.ItemValue = "FALSE";
                }
                if (row.Cells[this.colRemark.Index].Value != null)
                    data.ItemRemark = row.Cells[this.colRemark.Index].Value.ToString();
                datas.Add(data);
            }
            return true;
        }

        public bool LoadItems(EvaTypeInfo typeinfo, bool bIsNewDocument)
        {
            List<EvaTypeItem> lstItems = new List<EvaTypeItem>();
            short shRet = EvaTypeService.Instance.GetEvaTypeItems(typeinfo, ref lstItems);
            if (shRet != SystemConst.ReturnValue.OK)
                return false;
            this.dataTableView1.Rows.Clear();
            int index = 0;
            foreach (EvaTypeItem item in lstItems)
            {
                if (bIsNewDocument == true && item.ItemEnable.ToString().Equals("True"))
                    continue;
                index = this.dataTableView1.Rows.Add();
                if (item.ItemTextBlod)
                {
                    Font fon = new Font("宋体", 9f, FontStyle.Bold);
                    this.dataTableView1.Rows[index].Cells[this.colContent.Index].Style.Font = fon;
                }
                this.dataTableView1.Rows[index].Cells[0].Value = item.ItemText;
                if (item.ItemType == "输入框")
                {
                    this.dataTableView1.Rows[index].Cells[this.colValue.Index] = new DataGridViewTextBoxCell();
                    this.dataTableView1.Rows[index].Cells[this.colValue.Index].Value = item.ItemDefaultValue;
                }
                else
                {
                    if (item.ItemDefaultValue == "是" || item.ItemDefaultValue.ToUpper() == "TRUE")
                        this.dataTableView1.Rows[index].Cells[this.colValue.Index].Value = true;
                    else
                        this.dataTableView1.Rows[index].Cells[this.colValue.Index].Value = false;
                }
                this.dataTableView1.Rows[index].ReadOnly = item.ItemReadOnly;

                this.dataTableView1.Rows[index].Tag = item;
            }
            return true;
        }

        private void SetRowsNormal()
        {
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
        }

        /// <summary>
        /// 打开指定的已写的病历信息的病历文档
        /// </summary>
        /// <param name="docInfo">已写病历信息</param>
        /// <returns>是否成功</returns>
        public bool OpenDocument(EvaTypeInfo typeInfo, EvaDocInfo docInfo, bool bIsNewDocument)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return false;
            if (docInfo == null || typeInfo == null)
                return false;
            this.m_evaTypeInfo = typeInfo;
            this.m_evaDocInfo = docInfo;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.m_documentUpdated = false;

            this.tooltxtRemark.Visible = typeInfo.HaveRemark;
            this.toollblRemark.Visible = typeInfo.HaveRemark;
            this.tooltxtCreatorName.Text = docInfo.CreatorName;
            this.tooltxtScore.Text = docInfo.Score.ToString();
            this.tooltxtRemark.Text = docInfo.Remark;

            //初始化选项
            this.LoadItems(typeInfo, bIsNewDocument);
            //加载选项勾选情况
            this.LoadItemData(docInfo.EvaID);

            this.SetRowsNormal();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return true;
        }

        private bool LoadItemData(string szEvaId)
        {
            List<EvaDocItemInfo> lst = null;
            short shRet = EvaDocInfoService.Instance.GetItemDatasByID(szEvaId, ref lst);
            if (shRet != SystemConst.ReturnValue.OK || lst == null)
                return false;
            bool bExistNo = false;
            string value = "";
            string szRemark = "";
            List<DataTableViewRow> dtRowLst = new List<DataTableViewRow>();
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                bExistNo = false;
                EvaTypeItem item = row.Tag as EvaTypeItem;
                foreach (EvaDocItemInfo ItemData in lst)
                {
                    if (item.ItemNo.Trim() == ItemData.ItemNo.Trim())
                    {
                        bExistNo = true;
                        value = ItemData.ItemValue;
                        szRemark = ItemData.ItemRemark;
                        break;
                    }
                }
                if (bExistNo)
                {
                    if (item.ItemType == "输入框")
                    {
                        row.Cells[this.colValue.Index].Value = value;
                    }
                    else
                    {
                        if (value.Trim().ToUpper() == "TRUE" || value.Trim() == "是")
                            row.Cells[this.colValue.Index].Value = true;
                    }
                    row.Cells[this.colRemark.Index].Value = szRemark;
                }
                else
                    dtRowLst.Add(row);
            }
            for (int i = 0; i < dtRowLst.Count; i++)
            {
                this.dataTableView1.Rows.Remove(dtRowLst[i]);
            }
            return true;
        }

        /// <summary>
        /// 检查当前护理记录编辑窗口是否已经改变
        /// </summary>
        /// <returns>是否取消</returns>
        /// 
        public bool CheckModifyState()
        {
            if (!this.IsModified)
                return false;

            string message = string.Format("{0}已经被修改,是否保存？", this.m_evaTypeInfo.EvaTypeName);
            DialogResult result = MessageBoxEx.ShowQuestion(message);
            if (result == DialogResult.Yes)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                bool success = this.SaveDocument();
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return !success;
            }
            return (result == DialogResult.Cancel);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SaveDocument();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSaveAs_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            //显示科室选择表，默认护理单元(2)
            bool multiSelected = true;
            bool deptTypeEnabled = false;
            DataTable table = new DataTable();
            DataTable dtDeptInfo = new DataTable();
            dtDeptInfo = UtilitiesHandler.Instance.ShowDeptSelectDialog(2, multiSelected, table, deptTypeEnabled);
            SaveDocumentAs(dtDeptInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        #region "打印文档记录"
        /// <summary>
        /// 加载护理记录单打印模板
        /// </summary>
        /// <param name="szReportName">护理记录单打印模板名</param>
        /// <returns>护理记录单打印模板byte[]</returns>
        private byte[] GetReportFileData(string szReportName)
        {
            if (this.m_evaTypeInfo == null || GlobalMethods.Misc.IsEmptyString(this.m_evaTypeInfo.EvaTypeName))
            {
                MessageBoxEx.ShowError("无法获取护理评价类型信息!");
                return null;
            }

            string szApplyEnv = ServerData.ReportTypeApplyEnv.NUR_DOC_FORM;
            if (GlobalMethods.Misc.IsEmptyString(szReportName))
                szReportName = this.m_evaTypeInfo.EvaTypeName;
            ReportTypeInfo reportTypeInfo =
                ReportCache.Instance.GetWardReportType(szApplyEnv, szReportName);
            if (reportTypeInfo == null)
            {
                MessageBoxEx.ShowError("当前护理评价的打印报表还没有制作!");
                return null;
            }

            byte[] byteTempletData = null;
            if (!ReportCache.Instance.GetReportTemplet(reportTypeInfo, ref byteTempletData))
            {
                MessageBoxEx.ShowError("当前护理评价的打印报表内容下载失败!");
                return null;
            }
            return byteTempletData;
        }

        private bool GetSystemContext(string name, ref object value)
        {
            return SystemContext.Instance.GetSystemContext(name, ref value);
        }

        private ReportExplorerForm GetReportExplorerForm()
        {
            ReportExplorerForm reportExplorerForm = new ReportExplorerForm();
            reportExplorerForm.WindowState = FormWindowState.Maximized;
            reportExplorerForm.QueryContext +=
                new Heren.Common.Report.QueryContextEventHandler(this.ReportExplorerForm_QueryContext);
            reportExplorerForm.ExecuteQuery +=
                new Heren.Common.Report.ExecuteQueryEventHandler(this.ReportExplorerForm_ExecuteQuery);
            reportExplorerForm.NotifyNextReport +=
                new NotifyNextReportEventHandler(this.ReportExplorerForm_NotifyNextReport);
            return reportExplorerForm;
        }

        private void ReportExplorerForm_QueryContext(object sender, Heren.Common.Report.QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = this.GetSystemContext(e.Name, ref value);
            e.Value = value;
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
        #endregion

        private void toolbtnPrint_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.CheckModifyState())
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }

            this.Update();
            byte[] byteReportData = this.GetReportFileData(null);
            if (byteReportData != null)
            {
                DataTable table =
                    GlobalMethods.Table.GetDataTable(this.dataTableView1, false, 0);
                ReportExplorerForm explorerForm = this.GetReportExplorerForm();
                explorerForm.ReportFileData = byteReportData;
                explorerForm.ReportParamData.Add("主信息", this.m_evaDocInfo);
                explorerForm.ReportParamData.Add("列表信息", table);
                explorerForm.ShowDialog();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            this.dataTableView1.EndEdit();
            this.Hide();
        }

        private void tooldtpRecordTime_ValueChanged(object sender, EventArgs e)
        {
            DateTime dtRecordTime = this.tooldtpRecordTime.Value;
            //if (this.Document != null)
            //    this.Document.ModifyRecordTime(dtRecordTime);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.toolbtnReturn.PerformClick();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolbtnCalculate_Click(object sender, EventArgs e)
        {
            DataGridViewTextBoxCell TextboxCell;
            DataGridViewCheckBoxCell CheckboxCell;
            decimal iScore = 0;
            int iCount = 0;
            foreach (DataTableViewRow row in this.dataTableView1.Rows)
            {
                EvaTypeItem item = row.Tag as EvaTypeItem;
                if (item.ItemInCount == false)
                    continue;
                iCount++;
                if (item.ItemType == "输入框")
                {
                    TextboxCell = row.Cells[1] as DataGridViewTextBoxCell;
                    if (TextboxCell.Value != null)
                        iScore += GlobalMethods.Convert.StringToValue(TextboxCell.Value.ToString(), 0);
                }
                else
                {
                    CheckboxCell = row.Cells[1] as DataGridViewCheckBoxCell;
                    if ((bool)CheckboxCell.EditedFormattedValue == true)
                        iScore += item.ItemScore;
                }
            }
            if (iCount == 0)
                this.tooltxtScore.Text = "0";
            else
                this.tooltxtScore.Text = ((iScore / iCount) * 100).ToString("#0.00");
        }
    }
}
