// ***********************************************************
// 护理电子病历系统,病人窗口之病人信息条中病人列表选择控件.
// Creator:YangMingkun  Date:2012-7-7
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.PatPage.Controls
{
    internal class PatientListControl : Control
    {
        private PatientListBox m_PatientListBox = null;
        private PatVisitInfo m_patVisit = null;
        private bool m_bPatientTableChangedEventEnabled = true;

        /// <summary>
        /// 该变量用于标记鼠标是否经过该控件
        /// </summary>
        private bool m_bMouseOver = false;

        /// <summary>
        /// 当控件内部病人信息改变前触发
        /// </summary>
        [Description("当控件内部病人信息改变前触发")]
        public event PatientInfoChangingEventHandler PatientInfoChanging;

        protected virtual void OnPatientInfoChanging(PatientInfoChangingEventArgs e)
        {
            if (this.PatientInfoChanging != null)
                this.PatientInfoChanging(this, e);
        }

        public PatientListControl()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.m_PatientListBox != null && !this.m_PatientListBox.IsDisposed)
            {
                this.m_PatientListBox.Dispose();
                PatientTable.Instance.PatientTableChanged -=
                    new EventHandler(this.System_PatientTableChanged);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.ShowPatientListBox();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        public void RefreshView(PatVisitInfo patVisit)
        {
            this.m_patVisit = patVisit;
            this.Invalidate();
        }

        private void ShowPatientListBox()
        {
            this.m_bPatientTableChangedEventEnabled = false;
            this.InitPatientListBox();

            if (!this.m_PatientListBox.Visible)
                this.m_PatientListBox.Visible = true;
            this.m_PatientListBox.BringToFront();
            this.m_PatientListBox.Focus();
            if (this.m_patVisit != null && this.m_PatientListBox.Items.Count > 0)
            {
                foreach (object item in this.m_PatientListBox.Items)
                {
                    PatVisitInfo patVisit = item as PatVisitInfo;
                    if (patVisit == null)
                        continue;
                    if (patVisit.PatientId == this.m_patVisit.PatientId
                        && patVisit.VisitId == this.m_patVisit.VisitId)
                    {
                        this.m_PatientListBox.SelectedItem = patVisit;
                        break;
                    }
                }
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            this.m_PatientListBox.DroppedDown = true;
            this.m_bPatientTableChangedEventEnabled = true;
        }

        private void LoadListItems()
        {
            if (this.m_PatientListBox == null || this.m_PatientListBox.IsDisposed)
                return;
            this.m_PatientListBox.Items.Clear();
            if (SystemContext.Instance.LoginUser == null)
                return;
            DataTable data = PatientTable.Instance.DeptPatientTable;
            if (data != null && data.Rows.Count > 0)
            {
                for (int index = 0; index < data.Rows.Count; index++)
                {
                    if (PatientTable.Instance.IsEmptyBed(data.Rows[index]))
                        continue;
                    PatVisitInfo patVisit = PatientTable.Instance.GetPatVisit(data.Rows[index]);
                    if (patVisit != null)
                        this.m_PatientListBox.Items.Add(patVisit);
                }
            }
        }

        private void InitPatientListBox()
        {
            if (this.m_PatientListBox != null && !this.m_PatientListBox.IsDisposed)
                return;
            this.m_PatientListBox = new PatientListBox();
            this.m_PatientListBox.Visible = false;
            this.m_PatientListBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.m_PatientListBox.Parent = this;
            this.m_PatientListBox.Left = 0;
            this.m_PatientListBox.Top = (this.Height - this.m_PatientListBox.Height) / 2;
            this.m_PatientListBox.Width = this.Width;
            this.LoadListItems();

            this.m_PatientListBox.Leave += new EventHandler(this.PatientListBox_Leave);
            this.m_PatientListBox.KeyDown += new KeyEventHandler(this.PatientListBox_KeyDown);
            PatientTable.Instance.PatientTableChanged += new EventHandler(this.System_PatientTableChanged);
            this.m_PatientListBox.SelectedIndexChanged += new EventHandler(this.PatientListBox_SelectedIndexChanged);
        }

        private void System_PatientTableChanged(object sender, EventArgs e)
        {
            this.m_bPatientTableChangedEventEnabled = false;
            this.LoadListItems();
            this.m_bPatientTableChangedEventEnabled = true;
        }

        private void PatientListBox_Leave(object sender, EventArgs e)
        {
            if (this.m_PatientListBox != null && !this.m_PatientListBox.IsDisposed)
                this.m_PatientListBox.Hide();
        }

        private void PatientListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.m_PatientListBox.DroppedDown)
                this.m_PatientListBox.DroppedDown = false;
            if (e.KeyData != Keys.Enter)
                return;
            string szInputText = this.m_PatientListBox.Text;
            foreach (object item in this.m_PatientListBox.Items)
            {
                PatVisitInfo patVisit = item as PatVisitInfo;
                if (patVisit == null)
                    continue;
                if (patVisit.BedCode == szInputText || patVisit.PatientName == szInputText)
                {
                    this.m_PatientListBox.SelectedItem = patVisit;
                    break;
                }
            }
        }

        private void PatientListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_bPatientTableChangedEventEnabled)
                return;
            PatVisitInfo patVisit = this.m_PatientListBox.SelectedItem as PatVisitInfo;
            PatientInfoChangingEventArgs changingEventArgs =
                new PatientInfoChangingEventArgs(this.m_patVisit, patVisit);
            this.OnPatientInfoChanging(changingEventArgs);
            if (!changingEventArgs.Cancel)
                this.RefreshView(patVisit);
            if (this.m_PatientListBox != null && !this.m_PatientListBox.IsDisposed)
                this.m_PatientListBox.Hide();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.m_bMouseOver = true;
            base.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_bMouseOver = false;
            base.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DrawBorder(e.Graphics);

            string szNursingClass = null;
            string szBedCode = null;
            string szPatientName = null;
            if (this.m_patVisit == null
                || this.m_patVisit.VisitTime == this.m_patVisit.DefaultTime)
            {
                szNursingClass = " ";
                szBedCode = "空";
                szPatientName = "请选择";
            }
            else
            {
                szNursingClass = this.m_patVisit.NursingClass;
                szBedCode = this.m_patVisit.BedCode;
                if (this.m_patVisit.PatientSex == ServerData.PatientSex.Male)
                    szPatientName = "♂ " + this.m_patVisit.PatientName;
                else if (this.m_patVisit.PatientSex == ServerData.PatientSex.Female)
                    szPatientName = "♀ " + this.m_patVisit.PatientName;
                else
                    szPatientName = this.m_patVisit.PatientName;
            }

            //护理等级
            Color color = PatientTable.Instance.GetColor(szNursingClass);
            SolidBrush brush = new SolidBrush(color);
            Pen pen = new Pen(Color.Gray);

            Rectangle bounds = new Rectangle(2, (this.Height - 12) / 2, 12, 12);
            e.Graphics.FillRectangle(brush, bounds);
            pen.Color = Color.Black;
            e.Graphics.DrawRectangle(pen, bounds);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.NoWrap;

            //床号
            if (!string.IsNullOrEmpty(szBedCode))
                szBedCode = szBedCode + "床";
            brush.Color = this.ForeColor;
            bounds.X = bounds.Right + 2;
            bounds.Y = 2;
            bounds.Width = 50;
            bounds.Height = this.Height;
            e.Graphics.DrawString(szBedCode, this.Font, brush, bounds, format);

            //病人姓名
            brush.Color = this.ForeColor;
            bounds.X = bounds.Right + 2;
            bounds.Y = 2;
            bounds.Width = this.Width - bounds.X;
            bounds.Height = this.Height;
            e.Graphics.DrawString(szPatientName, this.Font, brush, bounds, format);
            format.Dispose();
        }

        private void DrawBorder(Graphics g)
        {
            if (this.m_bMouseOver || this.DesignMode)
            {
                ControlPaint.DrawBorder3D(g, this.ClientRectangle, Border3DStyle.RaisedOuter);
            }
        }

        private class PatientListBox : ComboBox
        {
            /// <summary>
            /// 获取或设置列表项绘制模式
            /// </summary>
            [Description("获取或设置列表项绘制模式")]
            [DefaultValue(typeof(DrawMode), "OwnerDrawFixed")]
            new public DrawMode DrawMode
            {
                get { return base.DrawMode; }
                set { base.DrawMode = value; }
            }

            /// <summary>
            /// 获取或设置列表最大显示项数
            /// </summary>
            [Description("获取或设置列表最大显示项数")]
            [DefaultValue(24)]
            new public int MaxDropDownItems
            {
                get { return base.MaxDropDownItems; }
                set { base.MaxDropDownItems = value; }
            }

            public PatientListBox()
                : base()
            {
                base.DrawMode = DrawMode.OwnerDrawFixed;
                base.MaxDropDownItems = 24;
                base.DropDownWidth = 160;
            }

            protected override void OnMeasureItem(MeasureItemEventArgs e)
            {
                base.OnMeasureItem(e);
                e.ItemHeight = e.ItemHeight + 2;
            }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                if (e.Index < 0 || e.Index >= this.Items.Count)
                    return;
                PatVisitInfo patVisit = this.Items[e.Index] as PatVisitInfo;
                if (patVisit == null)
                {
                    base.OnDrawItem(e);
                    return;
                }
                e.DrawBackground();
                e.DrawFocusRectangle();

                //护理等级
                string szNuringClass = patVisit.NursingClass;
                Color color = PatientTable.Instance.GetColor(patVisit.NursingClass);
                SolidBrush brush = new SolidBrush(color);
                Pen pen = new Pen(Color.Gray);

                Rectangle bounds = e.Bounds;
                bounds.Width = this.ItemHeight - 4;
                bounds.Height = bounds.Width;
                bounds.X += 2;
                bounds.Y += 2;
                e.Graphics.FillRectangle(brush, bounds);
                pen.Color = Color.Black;
                e.Graphics.DrawRectangle(pen, bounds);

                //床号
                string szBedCode = string.Empty;
                if (!string.IsNullOrEmpty(patVisit.BedCode))
                    szBedCode = patVisit.BedCode + "床";
                brush.Color = e.ForeColor;
                bounds.X = bounds.Right + 4;
                bounds.Width = 64;
                e.Graphics.DrawString(szBedCode, e.Font, brush, bounds.X, bounds.Y);

                //病人姓名
                brush.Color = e.ForeColor;
                bounds.X = bounds.Right + 2;
                bounds.Width = e.Bounds.Width - 66;
                string szShowText = patVisit.PatientName;
                if (patVisit.PatientSex == ServerData.PatientSex.Male)
                    szShowText = "♂ " + patVisit.PatientName;
                else if (patVisit.PatientSex == ServerData.PatientSex.Female)
                    szShowText = "♀ " + patVisit.PatientName;
                e.Graphics.DrawString(szShowText, e.Font, brush, bounds.X, bounds.Y);
                pen.Dispose();
                brush.Dispose();
            }
        }
    }
}
