// ***********************************************************
// 病案质控系统用于病人列表窗口中显示病人信息的控件.
// Creator:YangMingkun  Date:2009-11-3
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace MedQCSys.Controls.PatInfoList
{
    internal class PatInfoCard : Control
    {
        private const int CONTENT_LINE_SPACE = 6;

        private PatVisitInfo m_PatVisitInfo = null;
        /// <summary>
        /// 获取或设置病人就诊日志信息
        /// </summary>
        [Browsable(false)]
        public PatVisitInfo PatVisitInfo
        {
            get { return this.m_PatVisitInfo; }
            set
            {
                this.m_PatVisitInfo = value;
                this.Invalidate();
            }
        }

        private bool m_bSelected = false;
        /// <summary>
        /// 获取或设置列表项是否处于选中状态
        /// </summary>
        [Browsable(false)]
        public bool Selected
        {
            get { return this.m_bSelected; }
            set
            {
                if (this.m_bSelected == value)
                    return;
                this.m_bSelected = value;
                if (this.m_bSelected)
                    this.Height = this.m_nCaptionHeight + (this.m_ContentFont.Height + CONTENT_LINE_SPACE) * 12;
                else
                    this.Height = this.m_nCaptionHeight;
                base.Invalidate();
            }
        }

        private int m_nCaptionHeight = 28;
        /// <summary>
        /// 获取或设置显示标题高度
        /// </summary>
        [Description("获取或设置显示标题高度")]
        public int CaptionHeight
        {
            get { return this.m_nCaptionHeight; }
            set
            {
                this.m_nCaptionHeight = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 获取或设置列表项背景色
        /// </summary>
        [Description("获取或设置列表项背景色")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置列表项的前景色
        /// </summary>
        [Description("获取或设置列表项的前景色")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                base.Invalidate();
            }
        }

        private Color m_BorderColor = SystemColors.Highlight;
        /// <summary>
        /// 获取或设置TabCapion边界色
        /// </summary>
        [Description("获取或设置TabCapion边界色")]
        public Color BorderColor
        {
            get { return this.m_BorderColor; }
            set
            {
                this.m_BorderColor = value;
                base.Invalidate();
            }
        }

        private Color m_GradientBackColor = Color.Lavender;
        /// <summary>
        /// 获取或设置TabCapion梯度渐变背景色
        /// </summary>
        [Description("获取或设置TabCapion梯度渐变背景色")]
        public Color GradientBackColor
        {
            get { return this.m_GradientBackColor; }
            set
            {
                this.m_GradientBackColor = value;
                base.Invalidate();
            }
        }

        private Color m_MouseOverBackColor = Color.White;
        /// <summary>
        /// 获取或设置光标经过时列表项的背景色
        /// </summary>
        [Description("获取或设置光标经过时列表项的背景色")]
        public Color MouseOverBackColor
        {
            get { return this.m_MouseOverBackColor; }
            set
            {
                this.m_MouseOverBackColor = value;
                base.Invalidate();
            }
        }

        private Color m_ActiveBackColor = Color.White;
        /// <summary>
        /// 获取或设置列表项活动状态下的背景色
        /// </summary>
        [Description("获取或设置列表项活动状态下的背景色")]
        public Color ActiveBackColor
        {
            get { return this.m_ActiveBackColor; }
            set
            {
                this.m_ActiveBackColor = value;
                base.Invalidate();
            }
        }

        private Color m_MouseOverForeColor = Color.Blue;
        /// <summary>
        /// 获取或设置光标经过时列表项的前景色
        /// </summary>
        [Description("获取或设置光标经过时列表项的前景色")]
        public Color MouseOverForeColor
        {
            get { return this.m_MouseOverForeColor; }
            set
            {
                this.m_MouseOverForeColor = value;
                base.Invalidate();
            }
        }

        private Color m_ActiveForeColor = Color.Blue;
        /// <summary>
        /// 获取或设置列表项活动状态下的前景色
        /// </summary>
        [Description("获取或设置列表项活动状态下的前景色")]
        public Color ActiveForeColor
        {
            get { return this.m_ActiveForeColor; }
            set
            {
                this.m_ActiveForeColor = value;
                base.Invalidate();
            }
        }

        private Font m_ContentFont = new Font("宋体", 9, FontStyle.Regular);
        /// <summary>
        /// 获取或设置就诊卡内容字体
        /// </summary>
        [Description("获取或设置就诊卡内容字体")]
        public Font ContentFont
        {
            get { return this.m_ContentFont; }
            set
            {
                this.m_ContentFont = value;
                this.Invalidate();
            }
        }

        private Font m_MouseOverFont = new Font("宋体", 10.5f, FontStyle.Bold);
        /// <summary>
        /// 获取或设置光标经过时列表项的字体
        /// </summary>
        [Description("获取或设置光标经过时列表项的字体")]
        public Font MouseOverFont
        {
            get { return this.m_MouseOverFont; }
            set
            {
                this.m_MouseOverFont = value;
                base.Invalidate();
            }
        }

        private Font m_ActiveFont = new Font("宋体", 10.5f, FontStyle.Bold);
        /// <summary>
        /// 获取或设置列表项活动状态下的字体
        /// </summary>
        [Description("获取或设置列表项活动状态下的字体")]
        public Font ActiveFont
        {
            get { return this.m_ActiveFont; }
            set
            {
                this.m_ActiveFont = value;
                base.Invalidate();
            }
        }

        public PatInfoCard()
        {
            this.Height = this.m_nCaptionHeight;
            this.Font = new Font("宋体", 10.5f, FontStyle.Regular);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// 绘制控件边界
        /// </summary>
        /// <param name="g">绘图对象Graphics</param>
        private void DrawBackground(Graphics g)
        {
            Rectangle ctrlRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            if (ctrlRect.Width <= 0 || ctrlRect.Height <= 0)
                return;
            if (this.m_bSelected)
            {
                SolidBrush solidBrush = new SolidBrush(this.m_ActiveBackColor);
                g.FillRectangle(solidBrush, ctrlRect);
                solidBrush.Dispose();
            }
            else if (this.m_bMouseOver)
            {
                LinearGradientBrush brush = new LinearGradientBrush(ctrlRect, this.m_GradientBackColor, this.BackColor, LinearGradientMode.Vertical);
                g.FillRectangle(brush, ctrlRect);
                brush.Dispose();
            }
            else
            {
                LinearGradientBrush brush = new LinearGradientBrush(ctrlRect, this.m_GradientBackColor, this.BackColor, LinearGradientMode.Horizontal);
                g.FillRectangle(brush, ctrlRect);
                brush.Dispose();
            }

            if (this.m_bSelected || this.m_bMouseOver)
            {
                Pen pen = new Pen(this.m_BorderColor);
                pen.Color = Color.LightGray;
                g.DrawRectangle(pen, ctrlRect);
                pen.Dispose();
            }
        }

        /// <summary>
        /// 绘制控件显示文本
        /// </summary>
        /// <param name="g">绘图对象Graphics</param>
        private void DrawCaption(Graphics g)
        {
            if (this.m_PatVisitInfo == null)
                return;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoWrap;


            Font font = null;
            Color foreColor = Color.Empty;
            if (this.m_bSelected)
            {
                font = this.m_ActiveFont;
                foreColor = this.m_ActiveForeColor;
            }
            else if (this.m_bMouseOver)
            {
                font = this.m_MouseOverFont;
                foreColor = this.m_MouseOverForeColor;
            }
            else
            {
                font = this.Font;
                foreColor = this.ForeColor;
            }
            int iSizeCount = 8;
            //床号
            Rectangle stBedCodeRect = new Rectangle(8, 0, this.Width * 1 / iSizeCount, this.m_nCaptionHeight);
            //病人姓名
            Rectangle stPatientNameRect = new Rectangle(stBedCodeRect.Right, 0, this.Width * 3 / iSizeCount , this.m_nCaptionHeight);
            //质检状态
            Rectangle stQualityRect = new Rectangle(stPatientNameRect.Right, 0, this.Width * 1 / iSizeCount , this.m_nCaptionHeight);
            //经治医生信息
            Rectangle stRequestRect = new Rectangle(stQualityRect.Right, 0, this.Width * 1 / iSizeCount, this.m_nCaptionHeight);
            //病案评分
            Rectangle stScoreRect = new Rectangle(stRequestRect.Right, 0, this.Width * 1 / iSizeCount , this.m_nCaptionHeight);
            //病人性别
            Rectangle stPatientSex = new Rectangle(stScoreRect.Right, 0, this.Width * 1 / iSizeCount, this.m_nCaptionHeight);

            stringFormat.Alignment = StringAlignment.Near;
            SolidBrush brush = new SolidBrush(foreColor);

            //绘制床号
            if (!string.IsNullOrEmpty(this.m_PatVisitInfo.BedCode) && !stBedCodeRect.IsEmpty)
            {
                g.DrawString(this.m_PatVisitInfo.BedCode, font, brush, stBedCodeRect, stringFormat);
            }

            //绘制病人姓名
            if (!string.IsNullOrEmpty(this.m_PatVisitInfo.PatientName) && !stPatientNameRect.IsEmpty)
            {
                if (this.m_PatVisitInfo.PatientCondition == "急")
                {
                    brush.Color = Color.Lime;
                }
                else if (this.m_PatVisitInfo.PatientCondition == "危")
                {
                    brush.Color = Color.Red;
                }
                string szPatientName = string.Format("{0}[{1}]", this.m_PatVisitInfo.PatientName, this.m_PatVisitInfo.VisitId);
                g.DrawString(szPatientName, font, brush, stPatientNameRect, stringFormat);
            }
            //绘制病人男女性别
            if (!string.IsNullOrEmpty(this.m_PatVisitInfo.PatientSex) && !stPatientSex.IsEmpty)
            {
                if (this.m_PatVisitInfo.PatientSex == "男")
                {
                    brush.Color = Color.Blue;
                    g.DrawString("♂", font, brush, stPatientSex, stringFormat);
                }
                else if (this.m_PatVisitInfo.PatientSex == "女")
                {
                    brush.Color = Color.OrangeRed;
                    g.DrawString("♀", font, brush, stPatientSex, stringFormat);
                }

            }


            //绘制质控状态
            if (!string.IsNullOrEmpty(this.m_PatVisitInfo.QCResultStatus) && !stQualityRect.IsEmpty)
            {
                if (this.m_PatVisitInfo.QCResultStatus == ServerData.MedQCStatus.PASS)
                {
                    brush.Color = Color.Lime;
                    g.DrawString("*", font, brush, stQualityRect, stringFormat);
                }
                else if (this.m_PatVisitInfo.QCResultStatus == ServerData.MedQCStatus.EXIST_BUG)
                {
                    brush.Color = Color.Red;
                    g.DrawString("*", font, brush, stQualityRect, stringFormat);
                }
                else if (this.m_PatVisitInfo.QCResultStatus == ServerData.MedQCStatus.SERIOUS_BUG)
                {
                    brush.Color = Color.Red;
                    g.DrawString("**", font, brush, stQualityRect, stringFormat);
                }
            }
            //绘制病案评分
            if (!string.IsNullOrEmpty(this.m_PatVisitInfo.QCResultStatus) &&
                !string.IsNullOrEmpty(this.m_PatVisitInfo.TotalScore) && !stScoreRect.IsEmpty)
            {
                if (this.m_PatVisitInfo.QCResultStatus == ServerData.MedQCStatus.PASS)
                {
                    brush.Color = Color.Lime;
                    g.DrawString(this.m_PatVisitInfo.TotalScore, font, brush, stScoreRect, stringFormat);
                }
                else if (this.m_PatVisitInfo.QCResultStatus == ServerData.MedQCStatus.EXIST_BUG || this.m_PatVisitInfo.QCResultStatus == ServerData.MedQCStatus.SERIOUS_BUG)
                {
                    brush.Color = Color.Red;
                    g.DrawString(this.m_PatVisitInfo.TotalScore, font, brush, stScoreRect, stringFormat);
                }
            }
            //绘制三级医生信息

            if (!stRequestRect.IsEmpty)
            {
                brush.Color = Color.Black;
                g.DrawString(this.m_PatVisitInfo.InchargeDoctor, font, brush, stRequestRect, stringFormat);
            }
            brush.Dispose();
            stringFormat.Dispose();
        }

        /// <summary>
        /// 绘制控件显示文本
        /// </summary>
        /// <param name="g">绘图对象Graphics</param>
        private void DrawContent(Graphics g)
        {
            Font font = new Font("宋体", 10.5f, FontStyle.Regular);
            SolidBrush brush = new SolidBrush(Color.Blue);
            Rectangle rect = new Rectangle(10, this.m_nCaptionHeight, this.Width, this.FontHeight + CONTENT_LINE_SPACE);

            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.PatientId), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("病 人 ID", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.InpNo), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("住 院 号", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.PatientSex), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("性    别", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.BirthTime == this.m_PatVisitInfo.DefaultTime ? "" : GlobalMethods.SysTime.GetAgeText(this.m_PatVisitInfo.BirthTime, this.m_PatVisitInfo.VisitTime)), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("年    龄", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.ChargeType), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("费    别", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.VisitTime.ToString("yyyy年M月d日 HH:mm")), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("入院时间", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.DeptName), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("入院科室", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.Diagnosis), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("诊    断", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.PatientCondition), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("病    情", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.NursingClass), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("护理等级", font, brush, rect.Location);

            rect.Y += rect.Height;
            brush.Color = Color.Blue;
            g.DrawString(string.Format("         {0}", this.m_PatVisitInfo.InchargeDoctor), font, brush, rect.Location);
            brush.Color = Color.Black;
            g.DrawString("经治医生", font, brush, rect.Location);

            brush.Dispose();
        }

        private bool m_bMouseOver = false;
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.m_bMouseOver = true;
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.m_bMouseOver = false;
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DrawBackground(e.Graphics);
            this.DrawCaption(e.Graphics);
            if (this.m_bSelected)
                this.DrawContent(e.Graphics);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            this.Focus();
        }
    }
}
