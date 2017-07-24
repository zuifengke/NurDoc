// ***********************************************************
// 护理电子病历系统,护士排班一览视图窗口.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.CardView;
using Heren.Common.Forms;
using Heren.Common.DockSuite;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class RosteringCardForm : DockContentBase
    {
        private Thread m_cardAddThread = null;

        private delegate void cardAddDelegate();

        /// <summary>
        /// 白板是否底端显示
        /// </summary>
        private bool m_whiteboardBottom = false;

        /// <summary>
        /// 获取或设置白板是否底端显示
        /// </summary>
        [Browsable(false)]
        public bool WhiteboardBottom
        {
            get { return this.m_whiteboardBottom; }
            set { this.SwitchWhiteboardLocation(value); }
        }

        /// <summary>
        /// 获取卡片容器控件
        /// </summary>
        private Control CardPanel
        {
            get
            {
                if (this.formControl1 == null)
                    return null;
                if (this.formControl1.IsDisposed)
                    return null;
                if (this.formControl1.Controls.Count <= 0)
                    return null;
                Control control = this.formControl1.Controls[0];
                if (control == null || control.IsDisposed)
                    return null;
                return control;
            }
        }

        public RosteringCardForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.whiteBoardControl1.MainFrame = parent;
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document | DockAreas.DockLeft
                | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom;

            string text = SystemContext.Instance.WindowName.RosteringView;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 100);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            string key = SystemConst.ConfigKey.WHITEBOARD_LOCATION;
            int location = SystemContext.Instance.GetConfig(key, 0);
            if (location == 1)
                this.SwitchWhiteboardLocation(true);
            else
                this.SwitchWhiteboardLocation(false);

            Application.DoEvents();
            this.whiteBoardControl1.Initialize(true);
            this.RefreshView();
            this.LoadRosDetailInfoTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 切换白板显示位置
        /// </summary>
        /// <param name="toBottom">显示到底端</param>
        public void SwitchWhiteboardLocation(bool toBottom)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (toBottom)
                this.whiteBoardControl1.Dock = DockStyle.Bottom;
            else
                this.whiteBoardControl1.Dock = DockStyle.Top;
            string key = SystemConst.ConfigKey.WHITEBOARD_LOCATION;
            string value = toBottom ? "1" : "0";
            SystemContext.Instance.WriteConfig(key, value);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnDisplayDeptChanged(EventArgs e)
        {
            base.OnDisplayDeptChanged(e);
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        /// <summary>
        /// 刷新当前显示视图
        /// </summary>
        public override void RefreshView()
        {
            base.RefreshView();
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RefreshCardView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 刷新视图
        /// </summary>
        private void RefreshCardView()
        {
            this.cardListView1.SuspendLayout();
            this.cardListView1.Cards.Clear();
            this.cardListView1.PerformLayout();
            this.cardListView1.BringToFront();
            this.Update();

            if (this.formControl1.Controls.Count <= 0)
            {
                this.LoadRosCardTemplet();
            }
            Control cardpanel = this.CardPanel;
            if (cardpanel != null)
            {
                if (cardpanel.Dock != DockStyle.None)
                    cardpanel.Dock = DockStyle.None;
                if (this.cardListView1.CardSize != cardpanel.Size)
                    this.cardListView1.CardSize = cardpanel.Size;
            }            

            if (this.m_cardAddThread == null || !this.m_cardAddThread.IsAlive)
            {
                this.m_cardAddThread = new Thread(new ThreadStart(this.StartCardAdd));
            }
            if (this.m_cardAddThread.ThreadState == ThreadState.Running)
                return;
            this.m_cardAddThread.Start();
            
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void StartCardAdd()
        {
            if (this.cardListView1.InvokeRequired)
            {
                cardAddDelegate caDelegate = new cardAddDelegate(AddCard);

                this.BeginInvoke(caDelegate, new object[] {});
            }
            else
            { AddCard(); }
        }

        /// <summary>
        /// 加载排班细卡
        /// </summary>
        private void AddCard()
        {
            //加载各个病区护士排班信息
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
    
            DataTable dtDeptInfo = new DataTable();
            dtDeptInfo = SystemContext.Instance.DeptTable;

            LinearGradient NormalGradient = new LinearGradient();
            if (SystemContext.Instance.PatientCardColorList["鼠标正常颜色上"] != null)
                NormalGradient.StartColor = (Color)SystemContext.Instance.PatientCardColorList["鼠标正常颜色上"];
            if (SystemContext.Instance.PatientCardColorList["鼠标正常颜色下"] != null)
                NormalGradient.EndColor = (Color)SystemContext.Instance.PatientCardColorList["鼠标正常颜色下"];

            LinearGradient HoverGradient = new LinearGradient();
            if (SystemContext.Instance.PatientCardColorList["鼠标悬停颜色上"] != null)
                HoverGradient.StartColor = (Color)SystemContext.Instance.PatientCardColorList["鼠标悬停颜色上"];
            if (SystemContext.Instance.PatientCardColorList["鼠标悬停颜色下"] != null)
                HoverGradient.EndColor = (Color)SystemContext.Instance.PatientCardColorList["鼠标悬停颜色下"];

            LinearGradient SelectionGradient = new LinearGradient();
            if (SystemContext.Instance.PatientCardColorList["鼠标选中颜色上"] != null)
                SelectionGradient.StartColor = (Color)SystemContext.Instance.PatientCardColorList["鼠标悬停颜色上"];
            if (SystemContext.Instance.PatientCardColorList["鼠标选中颜色下"] != null)
                SelectionGradient.EndColor = (Color)SystemContext.Instance.PatientCardColorList["鼠标悬停颜色下"];

            if (dtDeptInfo.Rows.Count > 0)
            {
                this.cardListView1.SuspendLayout();
                foreach (DataRow row in dtDeptInfo.Rows)
                {
                    CardListItem card = new CardListItem();
                    card.NormalGradient = NormalGradient;
                    card.HoverGradient = HoverGradient;
                    card.SelectionGradient = SelectionGradient;
                    this.cardListView1.Cards.Add(card);
                    this.UpdateCard(card, row["DeptCode"].ToString());
                }
                this.cardListView1.PerformLayout();
            }
        }

        /// <summary>
        /// 使用数据集中的行更新指定的卡片视图
        /// </summary>
        /// <param name="card">床卡对象</param>
        /// <param name="szDeptCode">科室代码</param>
        private void UpdateCard(CardListItem card, string szDeptCode)
        {
            if (card == null || szDeptCode == null)
                return;

            card.Elements.Clear();
            card.Tag = szDeptCode;
            this.formControl1.UpdateFormData("统计信息", szDeptCode);

            Control cardpanel = this.CardPanel;
            if (cardpanel == null)
                return;
            foreach (Control control in cardpanel.Controls)
            {
                XLabel label = control as XLabel;
                if (label == null || control as IControl == null)
                    continue;
                if (!label.Enabled)
                    continue;
                if (label.Image != null)
                {
                    IconElement iconElement = new IconElement();
                    iconElement.Tag = control;
                    iconElement.Image = label.Image;
                    iconElement.Bounds = control.Bounds;
                    if (this.formControl1.GetBindingEvent(label, "Click") != null)
                        iconElement.MouseAnimation = true;
                    card.Elements.Add(iconElement);
                    continue;
                }
                LabelElement labelElement = new LabelElement();
                labelElement.Tag = control;
                labelElement.Text = control.Text;
                labelElement.Font = control.Font;
                labelElement.Bounds = control.Bounds;
                labelElement.ForeColor = control.ForeColor;
                labelElement.BackColor = control.BackColor;
                labelElement.Format = GlobalMethods.Convert.GetStringFormat(label.TextAlign);
                labelElement.Format.FormatFlags = StringFormatFlags.NoWrap;
                labelElement.BorderStyle = label.BorderStyle;
                if (this.formControl1.GetBindingEvent(label, "Click") != null)
                    labelElement.MouseAnimation = true;
                card.Elements.Add(labelElement);
            }
        }

        /// <summary>
        /// 加载病人床卡信息视图模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadRosCardTemplet()
        {
            this.Update();
            string szApplyEnv = ServerData.DocTypeApplyEnv.BED_VIEW;
            string szFormName = string.Empty;
            szFormName = SystemConst.FormName.ROSTERING_CARD;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szFormName);
            if (docTypeInfo == null)
                return false;

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowErrorFormat("{0}模板内容下载失败!", null, szFormName);
                return false;
            }

            this.formControl1.Load(docTypeInfo, byteFormData);
            return true;
        }

        /// <summary>
        /// 加载护士排班详细信息视图模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadRosDetailInfoTemplet()
        {
            this.Update();
            string szApplyEnv = ServerData.DocTypeApplyEnv.BED_VIEW;
            string szFormName = string.Empty;
            szFormName = SystemConst.FormName.ROSTERING_CARD;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szFormName);
            if (docTypeInfo == null)
                return false;
            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowErrorFormat("{0}模板内容下载失败!", null, szFormName);
                return false;
            }
            this.formControl2.Load(docTypeInfo, byteFormData);
            return true;
        }

        private void cardListView1_CardElementClick(object sender, CardElementEventArgs e)
        {
            if (e.Card == null || e.Element == null)
                return;
            string szDeptCode = e.Card.Tag as string;
            this.formControl1.UpdateFormData("排班信息", szDeptCode);
            IControl control = e.Element.Tag as IControl;
            if (control != null)
                control.PerformClick(null);
        }

        private void cardListView1_CardDoubleClick(object sender, CardEventArgs e)
        {
            if (this.MainFrame == null || this.MainFrame.IsDisposed)
                return;
            if (this.cardListView1.SelectedCards.Count <= 0)
                return;
            this.MainFrame.ShowRosteringTemp(this.cardListView1.SelectedCards[0].Tag as string, "排班");
        }

        private void mnuNewNursingDoc_Click(object sender, EventArgs e)
        {
            if (this.MainFrame == null || this.MainFrame.IsDisposed)
                return;
            if (this.cardListView1.SelectedCards.Count <= 0)
                return;
            this.MainFrame.ShowRosteringTemp(this.cardListView1.SelectedCards[0].Tag as string, "排班");
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {

        }
    }
}