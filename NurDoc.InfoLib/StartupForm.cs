// ***********************************************************
// 护理电子病历系统,系统启动屏幕窗口.
// Creator:YangMingkun  Date:2012-9-11
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Heren.NurDoc.InfoLib
{
    public class StartupForm : Form
    {
        private static StartupForm m_instance = null;

        public static StartupForm Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new StartupForm();
                return m_instance;
            }
        }

        private StartupForm()
        {
            this.Size = new Size(520, 332);
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Cursor = Cursors.WaitCursor;
            this.BackgroundImage = Properties.Resources.SplashImage;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.BackgroundImage != null)
                this.BackgroundImage.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Update();
            this.Cursor = Cursors.WaitCursor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = this.ClientRectangle;

            // 3D边界
            ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.RaisedOuter);

            // 获取颜色
            Color color = Color.Yellow;
            if (this.BackgroundImage != null)
            {
                Bitmap bmp = new Bitmap(this.BackgroundImage);
                color = bmp.GetPixel(0, 0);
                color = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                bmp.Dispose();
            }

            // 产品版权
            StringFormat stringFormat = new StringFormat();
            SolidBrush brush = new SolidBrush(color);
            string szCopyRight = "© 2011-2014 浙江和仁科技有限公司版权所有，保留所有权利。";
            rect.X = 8;
            rect.Y = this.Height - this.Font.Height - 8;
            rect.Width = this.Width - (rect.X * 2);
            rect.Height = this.Font.Height;
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(szCopyRight, this.Font, brush, rect, stringFormat);

            // 产品版本
            string szProductVersion = "Nursing EMRS, Version: " + Application.ProductVersion;
            rect.Y -= this.Font.Height + 8;
            e.Graphics.DrawString(szProductVersion, this.Font, brush, rect, stringFormat);
            brush.Dispose();
            stringFormat.Dispose();
        }
    }
}
