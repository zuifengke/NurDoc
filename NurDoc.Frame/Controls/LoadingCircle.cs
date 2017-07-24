using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Heren.NurDoc.Frame.Controls
{
    public partial class LoadingCircle : Control
    {
        private readonly System.Drawing.Color DefaultColor = System.Drawing.Color.DarkGray;
        private double[] m_Angles;
        private PointF m_CenterPoint;
        private System.Drawing.Color m_Color;
        private System.Drawing.Color[] m_Colors;
        private int m_InnerCircleRadius;
        private bool m_IsTimerActive;
        private int m_NumberOfSpoke;
        private int m_OuterCircleRadius;
        private int m_ProgressValue;
        private int m_SpokeThickness;
        private StylePresets m_StylePreset;
        private Timer m_Timer;
        private const double NumberOfDegreesInCircle = 360.0;
        private const double NumberOfDegreesInHalfCircle = 180.0;

        public LoadingCircle()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.m_Color = this.DefaultColor;
            this.GenerateColorsPallet();
            this.GetSpokesAngles();
            this.GetControlCenterPoint();
            this.m_Timer = new Timer();
            this.m_Timer.Tick += new EventHandler(this.aTimer_Tick);
            this.ActiveTimer();
            base.Resize += new EventHandler(this.LoadingCircle_Resize);
        }

        private void ActiveTimer()
        {
            if (this.m_IsTimerActive)
            {
                this.m_Timer.Start();
            }
            else
            {
                this.m_Timer.Stop();
                this.m_ProgressValue = 0;
            }
            this.GenerateColorsPallet();
            base.Invalidate();
        }

        private void aTimer_Tick(object sender, EventArgs e)
        {
            this.m_ProgressValue = ++this.m_ProgressValue % this.m_NumberOfSpoke;
            base.Invalidate();
        }

        private System.Drawing.Color Darken(System.Drawing.Color objColor, int intPercent)
        {
            int intRed = objColor.R;
            int intGreen = objColor.G;
            int intBlue = objColor.B;
            return System.Drawing.Color.FromArgb(intPercent, Math.Min(intRed, 0xff), Math.Min(intGreen, 0xff), Math.Min(intBlue, 0xff));
        }

        private void DrawLine(Graphics objGraphics, PointF objPointOne, PointF objPointTwo, System.Drawing.Color objColor, int intLineThickness)
        {
            using (Pen objPen = new Pen(new SolidBrush(objColor), (float)intLineThickness))
            {
                objPen.StartCap = LineCap.Round;
                objPen.EndCap = LineCap.Round;
                objGraphics.DrawLine(objPen, objPointOne, objPointTwo);
            }
        }

        private void GenerateColorsPallet()
        {
            this.m_Colors = this.GenerateColorsPallet(this.m_Color, this.Active, this.m_NumberOfSpoke);
        }

        private System.Drawing.Color[] GenerateColorsPallet(System.Drawing.Color objColor, bool blnShadeColor, int intNbSpoke)
        {
            System.Drawing.Color[] objColors = new System.Drawing.Color[this.NumberSpoke];
            byte bytIncrement = (byte)(0xff / this.NumberSpoke);
            byte PERCENTAGE_OF_DARKEN = 0;
            for (int intCursor = 0; intCursor < this.NumberSpoke; intCursor++)
            {
                if (blnShadeColor)
                {
                    if ((intCursor == 0) || (intCursor < (this.NumberSpoke - intNbSpoke)))
                    {
                        objColors[intCursor] = objColor;
                    }
                    else
                    {
                        PERCENTAGE_OF_DARKEN = (byte)(PERCENTAGE_OF_DARKEN + bytIncrement);
                        if (PERCENTAGE_OF_DARKEN > 0xff)
                        {
                            PERCENTAGE_OF_DARKEN = 0xff;
                        }
                        objColors[intCursor] = this.Darken(objColor, PERCENTAGE_OF_DARKEN);
                    }
                }
                else
                {
                    objColors[intCursor] = objColor;
                }
            }
            return objColors;
        }

        private void GetControlCenterPoint()
        {
            this.m_CenterPoint = this.GetControlCenterPoint(this);
        }

        private PointF GetControlCenterPoint(Control objControl)
        {
            return new PointF((float)(objControl.Width / 2), (float)((objControl.Height / 2) - 1));
        }

        private PointF GetCoordinate(PointF objCircleCenter, int intRadius, double _dblAngle)
        {
            double dblAngle = (3.1415926535897931 * _dblAngle) / 180.0;
            return new PointF(objCircleCenter.X + (intRadius * ((float)Math.Cos(dblAngle))), objCircleCenter.Y + (intRadius * ((float)Math.Sin(dblAngle))));
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize.Width = (this.m_OuterCircleRadius + this.m_SpokeThickness) * 2;
            return proposedSize;
        }

        private void GetSpokesAngles()
        {
            this.m_Angles = this.GetSpokesAngles(this.NumberSpoke);
        }

        private double[] GetSpokesAngles(int intNumberSpoke)
        {
            double[] Angles = new double[intNumberSpoke];
            double dblAngle = 360.0 / ((double)intNumberSpoke);
            for (int shtCounter = 0; shtCounter < intNumberSpoke; shtCounter++)
            {
                Angles[shtCounter] = (shtCounter == 0) ? dblAngle : (Angles[shtCounter - 1] + dblAngle);
            }
            return Angles;
        }

        private void LoadingCircle_Resize(object sender, EventArgs e)
        {
            this.GetControlCenterPoint();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            if (this.m_NumberOfSpoke > 0)
            {
                pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                int intPosition = this.m_ProgressValue;
                for (int intCounter = 0; intCounter < this.m_NumberOfSpoke; intCounter++)
                {
                    intPosition = intPosition % this.m_NumberOfSpoke;
                    this.DrawLine(pe.Graphics, this.GetCoordinate(this.m_CenterPoint, this.m_InnerCircleRadius, this.m_Angles[intPosition]), this.GetCoordinate(this.m_CenterPoint, this.m_OuterCircleRadius, this.m_Angles[intPosition]), this.m_Colors[intCounter], this.m_SpokeThickness);
                    intPosition++;
                }
            }
            base.OnPaint(pe);
        }

        public void SetCircleAppearance(int numberSpoke, int spokeThickness, int innerCircleRadius, int outerCircleRadius)
        {
            this.NumberSpoke = numberSpoke;
            this.SpokeThickness = spokeThickness;
            this.InnerCircleRadius = innerCircleRadius;
            this.OuterCircleRadius = outerCircleRadius;
            base.Invalidate();
        }

        [Category("LoadingCircle"), Description("获取和设置一个布尔值，表示当前控件是否激活。")]
        public bool Active
        {
            get
            {
                return this.m_IsTimerActive;
            }

            set
            {
                this.m_IsTimerActive = value;
                this.ActiveTimer();
            }
        }

        [TypeConverter("System.Drawing.ColorConverter"), Category("LoadingCircle"), Description("获取和设置控件高亮色")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.m_Color;
            }

            set
            {
                this.m_Color = value;
                this.GenerateColorsPallet();
                base.Invalidate();
            }
        }

        [Category("LoadingCircle"), Description("获取和设置内圆半径")]
        public int InnerCircleRadius
        {
            get
            {
                if (this.m_InnerCircleRadius == 0)
                {
                    this.m_InnerCircleRadius = 8;
                }
                return this.m_InnerCircleRadius;
            }

            set
            {
                this.m_InnerCircleRadius = value;
                base.Invalidate();
            }
        }

        [Category("LoadingCircle"), Description("获取和设置辐条数量")]
        public int NumberSpoke
        {
            get
            {
                if (this.m_NumberOfSpoke == 0)
                {
                    this.m_NumberOfSpoke = 10;
                }
                return this.m_NumberOfSpoke;
            }

            set
            {
                if ((this.m_NumberOfSpoke != value) && (this.m_NumberOfSpoke > 0))
                {
                    this.m_NumberOfSpoke = value;
                    this.GenerateColorsPallet();
                    this.GetSpokesAngles();
                    base.Invalidate();
                }
            }
        }

        [Category("LoadingCircle"), Description("获取和设置外围半径")]
        public int OuterCircleRadius
        {
            get
            {
                if (this.m_OuterCircleRadius == 0)
                {
                    this.m_OuterCircleRadius = 10;
                }
                return this.m_OuterCircleRadius;
            }

            set
            {
                this.m_OuterCircleRadius = value;
                base.Invalidate();
            }
        }

        [Description("获取和设置旋转速度。"), Category("LoadingCircle")]
        public int RotationSpeed
        {
            get
            {
                return this.m_Timer.Interval;
            }

            set
            {
                if (value > 0)
                {
                    this.m_Timer.Interval = value;
                }
            }
        }

        [Category("LoadingCircle"), Description("获取和设置辐条粗细程度。")]
        public int SpokeThickness
        {
            get
            {
                if (this.m_SpokeThickness <= 0)
                {
                    this.m_SpokeThickness = 4;
                }
                return this.m_SpokeThickness;
            }

            set
            {
                this.m_SpokeThickness = value;
                base.Invalidate();
            }
        }

        [Category("LoadingCircle"), DefaultValue(typeof(StylePresets), "Custom"), Description("快速设置预定义风格。")]
        public StylePresets StylePreset
        {
            get
            {
                return this.m_StylePreset;
            }

            set
            {
                this.m_StylePreset = value;
                switch (this.m_StylePreset)
                {
                    case StylePresets.MacOSX:
                        this.SetCircleAppearance(12, 2, 5, 11);
                        break;

                    case StylePresets.Firefox:
                        this.SetCircleAppearance(9, 4, 6, 7);
                        break;

                    case StylePresets.Microsoft:
                        this.SetCircleAppearance(0x18, 4, 8, 9);
                        break;

                    case StylePresets.Custom:
                        this.SetCircleAppearance(10, 4, 8, 10);
                        break;
                }
            }
        }

        public enum StylePresets
        {
            MacOSX,
            Firefox,
            Microsoft,
            Custom
        }
    }
}
