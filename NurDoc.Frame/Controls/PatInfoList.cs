// ***********************************************************
// 病案质控系统用于病人列表窗口中显示病人信息列表的控件.
// Creator:YangMingkun  Date:2009-11-3
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Heren.NurDoc.DAL;
namespace MedQCSys.Controls.PatInfoList
{
    internal class PatInfoList : Panel
    {
        /// <summary>
        /// 当用户切换选中项时触发
        /// </summary>
        [Description("当用户切换选中项时触发")]
        public event EventHandler CardSelectedChanged = null;

        /// <summary>
        /// 当用户切换选中项时触发
        /// </summary>
        [Description("当用户切换选中项时触发")]
        public event CancelEventHandler CardSelectedChanging = null;

        /// <summary>
        /// 当用户单击列表项时触发
        /// </summary>
        [Description("当用户单击列表项时触发")]
        public event MouseEventHandler CardMouseClick = null;

        private PatInfoCard m_selectedCard = null;
        /// <summary>
        /// 获取或设置当前选中的病人信息卡
        /// </summary>
        [Browsable(false)]
        [Description("获取或设置当前选中的病人信息卡")]
        public PatInfoCard SelectedCard
        {
            get { return this.m_selectedCard; }
            set
            {
                if (this.m_selectedCard == value)
                    return;

                CancelEventArgs cancelEventArgs = new CancelEventArgs();
                if (this.CardSelectedChanging != null)
                    this.CardSelectedChanging(this, cancelEventArgs);
                if (cancelEventArgs.Cancel)
                    return;

                this.SuspendLayout();
                if (this.m_selectedCard != null && !this.m_selectedCard.IsDisposed)
                    this.m_selectedCard.Selected = false;
                this.m_selectedCard = value;
                if (this.m_selectedCard != null && !this.m_selectedCard.IsDisposed)
                    this.m_selectedCard.Selected = true;
                this.ResumeLayout(true);

                if (this.m_selectedCard != null)
                    this.ScrollControlIntoView(this.m_selectedCard);

                this.Update();
                if (this.CardSelectedChanged != null)
                    this.CardSelectedChanged(this, EventArgs.Empty);
            }
        }

        public PatInfoList()
        {
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.Fixed3D;
            this.AutoScroll = true;
            this.Padding = new Padding(1);
        }

        public PatInfoCard AddPatInfo(PatVisitInfo patVisitInfo)
        {
            PatInfoCard patInfoCard = new PatInfoCard();
            patInfoCard.Dock = DockStyle.Top;
            patInfoCard.PatVisitInfo = patVisitInfo;
            patInfoCard.MouseUp += new MouseEventHandler(this.patInfoCard_MouseUp);
            patInfoCard.Tag = patVisitInfo;
            this.Controls.Add(patInfoCard);
            return patInfoCard;
        }

        public void ClearPatInfo()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            this.ResumeLayout(true);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            this.Update();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Focus();
        }

        private void patInfoCard_MouseUp(object sender, MouseEventArgs e)
        {
            Control ctrl = sender as Control;
            if (!ctrl.ClientRectangle.Contains(e.Location))
                return;

            this.SelectedCard = sender as PatInfoCard;

            if (this.CardMouseClick != null) this.CardMouseClick(sender, e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
    }
}
