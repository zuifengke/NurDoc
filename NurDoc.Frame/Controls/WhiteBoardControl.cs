// ***********************************************************
// 护理电子病历系统,床位卡窗口顶部白板模块.
// Creator:YangMingkun  Date:2012-8-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame.Dialogs;
using Heren.NurDoc.Frame.DockForms;

namespace Heren.NurDoc.Frame.Controls
{
    internal partial class WhiteBoardControl : UserControl
    {
        private MainFrame m_mainFrame = null;

        /// <summary>
        /// 获取或设置所属的主程序窗口
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("获取或设置所属的主程序窗口")]
        public MainFrame MainFrame
        {
            get
            {
                if (this.m_mainFrame == null)
                    return null;
                if (this.m_mainFrame.IsDisposed)
                    return null;
                return this.m_mainFrame;
            }

            set
            {
                this.m_mainFrame = value;
            }
        }

        public WhiteBoardControl()
        {
            this.InitializeComponent();
            SystemContext.Instance.DisplayDeptChanged +=
                new EventHandler(this.System_DisplayDeptChanged);
        }

        /// <summary>
        /// 释放控件占用的资源
        /// </summary>
        public void DisposeControl()
        {
            this.formControl1.Dispose();
            SystemContext.Instance.DisplayDeptChanged -=
                new EventHandler(this.System_DisplayDeptChanged);
        }

        /// <summary>
        /// 初始化白板表单界面
        /// </summary>
        public void Initialize()
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.LoadWhiteboardTemplet();
            this.formControl1.UpdateFormData("刷新视图", null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 初始化排班一览表白板表单界面
        /// </summary>
        public void Initialize(bool bIsRostering)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();
            this.LoadWhiteboardTemplet(bIsRostering);
            this.formControl1.UpdateFormData("刷新视图", null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 加载病人列表过滤窗口模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadWhiteboardTemplet()
        {
            Application.DoEvents();
            string szApplyEnv = ServerData.DocTypeApplyEnv.WHITEBOARD_VIEW;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError("护士站白板视图模板还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError("护士站白板视图模板内容下载失败!");
                return false;
            }
            this.formControl1.Load(docTypeInfo, byteFormData);
            if (this.formControl1.Controls.Count > 0)
            {
                if (this.formControl1.Controls[0].Height > 0)
                    this.Height = this.formControl1.Controls[0].Height + this.Padding.Vertical;
                this.formControl1.Controls[0].Dock = DockStyle.Fill;
            }
            return true;
        }

        /// <summary>
        /// 加载护理排班一览窗口模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadWhiteboardTemplet(bool bIsRostering)
        {
            if (!bIsRostering) return false;
            Application.DoEvents();
            string szApplyEnv = ServerData.DocTypeApplyEnv.WHITEBOARD_VIEW;
            List<DocTypeInfo> lstDocTypeInfos = FormCache.Instance.GetDocTypeList(szApplyEnv);
            if (lstDocTypeInfos == null)
            {
                MessageBoxEx.ShowError("护理排班白板视图模板还没有制作!");
                return false;
            }

            byte[] byteFormData = null;
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfos)
            {
                if (docTypeInfo.DocTypeName.Contains("排班"))
                {
                    if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
                    {
                        MessageBoxEx.ShowError("护理排班白板视图模板内容下载失败!");
                        return false;
                    }
                    this.formControl1.Load(docTypeInfo, byteFormData);
                    break;
                }
            }
            if (this.formControl1.Controls.Count > 0)
            {
                if (this.formControl1.Controls[0].Height > 0)
                    this.Height = this.formControl1.Controls[0].Height + this.Padding.Vertical;
                this.formControl1.Controls[0].Dock = DockStyle.Fill;
            }
            return true;
        }

        /// <summary>
        /// 切换病人列表视图
        /// </summary>
        public void SwitchView(PatientView view)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
            if (this.MainFrame == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            IDockContent content = this.MainFrame.GetContent(typeof(PatientCardForm));
            PatientCardForm cardForm = content as PatientCardForm;
            if (cardForm != null && !cardForm.IsDisposed)
                cardForm.SwitchView(view);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 切换白板显示位置
        /// </summary>
        /// <param name="toBottom">显示到底端</param>
        public void SwitchLocation(bool toBottom)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
            if (this.MainFrame == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            IDockContent content = this.MainFrame.GetContent(typeof(PatientCardForm));
            PatientCardForm cardForm = content as PatientCardForm;
            if (cardForm != null && !cardForm.IsDisposed)
                cardForm.SwitchWhiteboardLocation(toBottom);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void System_DisplayDeptChanged(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            //当科室被切换后,通知表单去刷新视图
            this.formControl1.UpdateFormData("刷新视图", null);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Pen pen = new Pen(Color.SkyBlue);
            Rectangle rect = this.formControl1.Bounds;
            e.Graphics.DrawRectangle(pen, rect.X - 1, rect.Y - 1, rect.Width + 1, rect.Height + 1);
            pen.Dispose();
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "刷新列表" || e.Name == "刷新病人列表")
            {
                if (this.IsDisposed || !this.IsHandleCreated)
                    return;
                GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
                this.Update();
                DataTable table = e.Data as DataTable;
                if (table == null)
                    table = this.formControl1.GetFormData("病人列表") as DataTable;
                PatientTable.Instance.RefreshDisplayPatientList(table);
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
            }
            else if (e.Name == "切换床卡视图")
            {
                string view = e.Param != null ? e.Param.ToString() : string.Empty;
                if (view == "细卡视图")
                    this.SwitchView(PatientView.DetailCard);
                else if (view == "简卡视图")
                    this.SwitchView(PatientView.SimpleCard);
                else if (view == "列表视图")
                    this.SwitchView(PatientView.PatientList);
            }
            else if (e.Name == "切换白板位置")
            {
                string location = e.Param != null ? e.Param.ToString() : string.Empty;
                if (location == "顶端显示")
                    this.SwitchLocation(false);
                else if (location == "底端显示")
                    this.SwitchLocation(true);
            }
            else if (e.Name == "切换病区" || e.Name == "切换科室")
            {
                if (GlobalMethods.Misc.IsEmptyString(e.Param) || GlobalMethods.Misc.IsEmptyString(e.Data))
                    return;
                DeptInfo displayDept = new DeptInfo();
                displayDept.DeptCode = e.Param.ToString();
                displayDept.DeptName = e.Data.ToString();
                SystemContext.Instance.DisplayDept = displayDept;
                SystemContext.Instance.OnDisplayDeptChanged(this, EventArgs.Empty);
            }
            else if (e.Name == "切换用户" || e.Name == "切换登录用户")
            {
                const int WM_SWITCHUSER = NativeConstants.WM_USER + 1;
                if (this.MainFrame != null)
                    NativeMethods.User32.SendMessage(this.MainFrame.Handle, WM_SWITCHUSER, IntPtr.Zero, IntPtr.Zero);
            }
            else if (e.Name == "护理信息库")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.Arguments = e.Param.ToString().Replace(SystemConst.StartupArgs.ESCAPED_CHAR, SystemConst.StartupArgs.ESCAPE_CHAR);
                proc.StartInfo.FileName = string.Format(@"{0}\InfoLib.exe", Application.StartupPath);
                try
                {
                    proc.Start();
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show("无法启动护理信息系统!");
                    LogManager.Instance.WriteLog("MainForm.StartNurDocSys", null, null, "护理信息系统启动失败!", ex);
                }
            }
        }
    }
}
