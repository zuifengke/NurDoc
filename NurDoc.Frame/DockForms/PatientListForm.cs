// ***********************************************************
// 护理电子病历系统,主窗口左侧病人快捷列表窗口.
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
using Heren.Common.DockSuite;
using Heren.Common.Controls.TableView;
using Heren.Common.Libraries;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class PatientListForm : DockContentBase
    {
        public PatientListForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.Icon = Properties.Resources.PersonsIcon;
            this.AutoHidePortion = this.Width;
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.DockLeftAutoHide;
            this.DockAreas = DockAreas.DockLeft | DockAreas.DockRight
                | DockAreas.DockTop | DockAreas.DockBottom;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnPatientTableChanged()
        {
            base.OnPatientTableChanged();
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this)
                this.RefreshView();
        }

        protected override void OnActiveContentChanged()
        {
            if (this.Pane == null || this.Pane.IsDisposed)
                return;
            if (this.Pane.ActiveContent == this && this.PatientTableChanged)
                this.RefreshView();
        }

        public override void RefreshView()
        {
            base.RefreshView();
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.dataTableView1.Rows.Clear();
            if (SystemContext.Instance.LoginUser == null)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            this.Update();

            DataTable data = PatientTable.Instance.DisplayPatientTable;
            if (data == null || data.Rows.Count <= 0)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                return;
            }
            for (int index = 0; index < data.Rows.Count; index++)
            {
                if (PatientTable.Instance.IsEmptyBed(data.Rows[index]))
                    continue;
                PatVisitInfo patVisit = PatientTable.Instance.GetPatVisit(data.Rows[index]);
                int nRowIndex = this.dataTableView1.Rows.Add();
                DataTableViewRow row = this.dataTableView1.Rows[nRowIndex];
                this.SetRowData(row, patVisit);
                this.dataTableView1.SetRowState(row, RowState.Normal);
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void SetRowData(DataTableViewRow row, PatVisitInfo patVisitInfo)
        {
            if (row == null || row.Index < 0 || patVisitInfo == null)
                return;
            row.Tag = patVisitInfo;
            row.Cells[this.colBedNumber.Index].Value = patVisitInfo.BedCode;
            if (patVisitInfo.PatientSex == ServerData.PatientSex.Male)
                row.Cells[this.colName.Index].Value = "♂ " + patVisitInfo.PatientName;
            else if (patVisitInfo.PatientSex == ServerData.PatientSex.Female)
                row.Cells[this.colName.Index].Value = "♀ " + patVisitInfo.PatientName;
            else
                row.Cells[this.colName.Index].Value = patVisitInfo.PatientName;
        }

        private void dataTableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.MainFrame == null || this.MainFrame.IsDisposed)
                return;
            if (this.dataTableView1.Rows.Count <= 0)
                return;
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.MainFrame.ShowPatientPageForm(this.dataTableView1.SelectedRows[0].Tag as PatVisitInfo);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void dataTableView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != this.colNursingClass.Index)
                return;
            e.Handled = true;
            e.PaintBackground(e.ClipBounds, true);
            e.PaintContent(e.ClipBounds);
            PatVisitInfo patVisit = this.dataTableView1.Rows[e.RowIndex].Tag as PatVisitInfo;
            if (patVisit != null)
            {
                Color color = PatientTable.Instance.GetColor(patVisit.NursingClass);
                SolidBrush brush = new SolidBrush(color);
                Pen pen = new Pen(Color.Gray);
                Rectangle rect = e.CellBounds;
                rect = new Rectangle(rect.X + ((rect.Width - 12) / 2), rect.Y + ((rect.Height - 12) / 2), 12, 12);
                e.Graphics.FillRectangle(brush, rect);
                e.Graphics.DrawRectangle(pen, rect);
                brush.Dispose();
                pen.Dispose();
            }
        }
    }
}
