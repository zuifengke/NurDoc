// ***********************************************************
// 护理电子病历系统,床位卡列表视图窗口.
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
    internal partial class PatientCardForm : DockContentBase
    {
        /// <summary>
        /// 病人列表视图枚举
        /// </summary>
        private PatientView m_patientView = PatientView.PatientList;

        /// <summary>
        /// 获取或设置病人列表视图
        /// </summary>
        [Browsable(false)]
        public PatientView PatientView
        {
            get { return this.m_patientView; }
            set { this.SwitchView(value); }
        }

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

        public PatientCardForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.whiteBoardControl1.MainFrame = parent;
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document | DockAreas.DockLeft
                | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom;

            string text = SystemContext.Instance.WindowName.BedView;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 100);
        }

        protected override void OnPatientTableChanged()
        {
            base.OnPatientTableChanged();
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            Application.DoEvents();

            string key = SystemConst.ConfigKey.PATIENT_VIEW;
            int mode = SystemContext.Instance.GetConfig(key, 0);
            if (mode == 1)
                this.m_patientView = PatientView.SimpleCard;
            else if (mode == 2)
                this.m_patientView = PatientView.PatientList;
            else
                this.m_patientView = PatientView.DetailCard;

            key = SystemConst.ConfigKey.WHITEBOARD_LOCATION;
            int location = SystemContext.Instance.GetConfig(key, 0);
            if (location == 1)
                this.SwitchWhiteboardLocation(true);
            else
                this.SwitchWhiteboardLocation(false);

            Application.DoEvents();
            this.whiteBoardControl1.Initialize();
            this.RefreshView();
            this.LoadPatientDetailInfoTemplet();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientTableChanged)
                this.RefreshView();
        }

        /// <summary>
        /// 切换病人列表视图
        /// </summary>
        public void SwitchView(PatientView view)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            this.cardListView1.SuspendLayout();
            this.cardListView1.Cards.Clear();
            this.cardListView1.PerformLayout();
            this.formControl1.Controls.Clear();

            this.m_patientView = view;

            string key = SystemConst.ConfigKey.PATIENT_VIEW;
            string value = ((short)view).ToString();
            SystemContext.Instance.WriteConfig(key, value);
            this.RefreshView();
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

        /// <summary>
        /// 刷新当前显示视图
        /// </summary>
        public override void RefreshView()
        {
            base.RefreshView();
            if (this.IsDisposed || !this.IsHandleCreated)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
           
            if (this.m_patientView == PatientView.PatientList)
                this.RefreshListView();
            else
                this.RefreshCardView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 刷新列表模式的视图
        /// </summary>
        private void RefreshListView()
        {
            this.formControl1.BringToFront();
            this.Update();
            if (this.formControl1.Controls.Count <= 0)
            {
                this.LoadPatientBedCardTemplet();
            }
            Control cardpanel = this.CardPanel;
            if (cardpanel != null)
            {
                if (cardpanel.Dock != DockStyle.Fill)
                    cardpanel.Dock = DockStyle.Fill;
                if (cardpanel.Padding.All != 4)
                    cardpanel.Padding = new Padding(4);
            }

            //加载病区在院的病人列表
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            DataTable data = PatientTable.Instance.DisplayPatientTable;
            this.formControl1.UpdateFormData("病人信息", data);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 刷新床卡模式的视图
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
                this.LoadPatientBedCardTemplet();
            }
            Control cardpanel = this.CardPanel;
            if (cardpanel != null)
            {
                if (cardpanel.Dock != DockStyle.None)
                    cardpanel.Dock = DockStyle.None;
                if (this.cardListView1.CardSize != cardpanel.Size)
                    this.cardListView1.CardSize = cardpanel.Size;
            }

            //加载病区在院的病人列表
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            DataTable data = PatientTable.Instance.DisplayPatientTable;

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

            if (data != null && data.Rows.Count > 0)
            {
                this.cardListView1.SuspendLayout();
                for (int index = 0; index < data.Rows.Count; index++)
                {
                    CardListItem card = new CardListItem();
                    card.NormalGradient = NormalGradient;
                    card.HoverGradient = HoverGradient;
                    card.SelectionGradient = SelectionGradient;
                    this.cardListView1.Cards.Add(card);
                    this.UpdateCard(card, data.Rows[index]);
                }
                this.cardListView1.PerformLayout();
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// 使用数据集中的行更新指定的卡片视图
        /// </summary>
        /// <param name="card">床卡对象</param>
        /// <param name="row">数据集行</param>
        private void UpdateCard(CardListItem card, DataRow row)
        {
            if (card == null || row == null)
                return;

            PatVisitInfo patVisit = PatientTable.Instance.GetPatVisit(row);
            card.Elements.Clear();
            card.Tag = row;
            if (!GlobalMethods.Misc.IsEmptyString(patVisit.PatientID))
                card.Data = patVisit;

            this.formControl1.UpdateFormData("病人信息", row);

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
        private bool LoadPatientBedCardTemplet()
        {
            this.Update();
            string szApplyEnv = ServerData.DocTypeApplyEnv.BED_VIEW;
            string szFormName = string.Empty;
            if (this.m_patientView == PatientView.SimpleCard)
                szFormName = SystemConst.FormName.BEDCARD_SIMPLE_CARD;
            else if (this.m_patientView == PatientView.PatientList)
                szFormName = SystemConst.FormName.BEDCARD_PATIENT_LIST;
            else
                szFormName = SystemConst.FormName.BEDCARD_DETAIL_CARD;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szFormName);
            if (docTypeInfo == null)
            {
                //MessageBoxEx.ShowErrorFormat("{0}模板还没有制作!", null, szFormName);
                return false;
            }

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
        /// 加载病人详细信息视图模板
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadPatientDetailInfoTemplet()
        {
            this.Update();
            string szApplyEnv = ServerData.DocTypeApplyEnv.BED_VIEW;
            string szFormName = string.Empty;
            szFormName = SystemConst.FormName.PATIENT_DETAIL_INFO;
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
            DataRow row = e.Card.Tag as DataRow;
            this.formControl1.UpdateFormData("病人信息", row);
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
            this.MainFrame.ShowPatientPageForm(this.cardListView1.SelectedCards[0].Data as PatVisitInfo);
        }

        private void mnuNewNursingDoc_Click(object sender, EventArgs e)
        {
            if (this.MainFrame == null || this.MainFrame.IsDisposed)
                return;
            if (this.cardListView1.SelectedCards.Count <= 0)
                return;
            this.MainFrame.ShowPatientPageForm(this.cardListView1.SelectedCards[0].Data as PatVisitInfo);
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "打开病人窗口")
            {
                if (this.MainFrame != null)
                    this.MainFrame.ShowPatientPageForm(e.Param);
            }
            if (e.Name == "打开病人信息")
            {
                if (this.formControl2.DocTypeInfo == null)
                    return;
                if (this.arrowSplitter1.Visible == false)
                {
                    this.arrowSplitter1.Visible = true;
                    this.formControl2.Visible = true;
                }
                this.formControl2.UpdateFormData("病人信息", e.Param);
            }
            if (e.Name == "打开病人同床婴儿窗口")
            {
                if (!RightController.Instance.UserRight.BabyRecVitalSign.Value)
                {
                    MessageBoxEx.Show("婴儿体征录入权限不够");
                    return;
                }
                if (this.cardListView1.SelectedCards.Count <= 0)
                    return;

                if (this.MainFrame != null)
                {
                    PatVisitInfo patVisitInfo = new PatVisitInfo();
                    PatVisitInfo childPatVisitInfo = new PatVisitInfo();
                    patVisitInfo = this.cardListView1.SelectedCards[0].Data as PatVisitInfo;
                    childPatVisitInfo = patVisitInfo.Clone() as PatVisitInfo;
                    object szBirthTime = this.formControl1.GetFormData("婴儿出生时间");
                    if (szBirthTime == null)
                        return;
                    childPatVisitInfo.BirthTime = DateTime.Parse(szBirthTime.ToString());
                    childPatVisitInfo.VisitTime = childPatVisitInfo.BirthTime;
                    childPatVisitInfo.SubID = e.Data.ToString();
                    childPatVisitInfo.PatientName = string.Format("{0}{1}", patVisitInfo.PatientName, e.Param.ToString());
                    childPatVisitInfo.PatientID = string.Format("{0}_{1}", patVisitInfo.PatientID, childPatVisitInfo.SubID);
                    if (e.Param.ToString().Contains("子"))
                        childPatVisitInfo.PatientSex = ServerData.PatientSex.Male;
                    else
                        childPatVisitInfo.PatientSex = ServerData.PatientSex.Female;
                    this.MainFrame.ShowPatientPageForm(childPatVisitInfo);
                }
            }
        }

        private void PatientCardForm_Load(object sender, EventArgs e)
        {
            string key = SystemConst.ConfigKey.PATIENT_VIEW;
            int mode = SystemContext.Instance.GetConfig(key, 0);
            if (mode == 2)
                m_patientView = PatientView.PatientList;
            if (mode == 1)
                m_patientView = PatientView.SimpleCard;
            if (mode == 0)
                m_patientView = PatientView.DetailCard;
        }
    }
}
