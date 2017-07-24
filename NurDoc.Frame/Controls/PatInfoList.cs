// ***********************************************************
// �����ʿ�ϵͳ���ڲ����б�������ʾ������Ϣ�б�Ŀؼ�.
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
        /// ���û��л�ѡ����ʱ����
        /// </summary>
        [Description("���û��л�ѡ����ʱ����")]
        public event EventHandler CardSelectedChanged = null;

        /// <summary>
        /// ���û��л�ѡ����ʱ����
        /// </summary>
        [Description("���û��л�ѡ����ʱ����")]
        public event CancelEventHandler CardSelectedChanging = null;

        /// <summary>
        /// ���û������б���ʱ����
        /// </summary>
        [Description("���û������б���ʱ����")]
        public event MouseEventHandler CardMouseClick = null;

        private PatInfoCard m_selectedCard = null;
        /// <summary>
        /// ��ȡ�����õ�ǰѡ�еĲ�����Ϣ��
        /// </summary>
        [Browsable(false)]
        [Description("��ȡ�����õ�ǰѡ�еĲ�����Ϣ��")]
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
