// ***********************************************************
// 护理电子病历系统,皮肤颜色配置类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Heren.NurDoc.Data
{
    public class SkinService
    {
        private static SkinService m_instance = null;

        /// <summary>
        /// 获取系统用户服务实例
        /// </summary>
        public static SkinService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new SkinService();
                return m_instance;
            }
        }

        private SkinService()
        {
            this.m_toolStripSkin = new ToolStripSkin();
        }

        private ToolStripSkin m_toolStripSkin = null;

        /// <summary>
        /// 获取或设置控件工具条皮肤颜色配置
        /// </summary>
        [Description("获取或设置控件工具条皮肤颜色配置")]
        public ToolStripSkin ToolStripSkin
        {
            get { return this.m_toolStripSkin; }
            set { this.m_toolStripSkin = value; }
        }

        /// <summary>
        /// 恢复皮肤颜色为默认值
        /// </summary>
        public void SetDefaultValue()
        {
            if (this.m_toolStripSkin == null)
                this.m_toolStripSkin = new ToolStripSkin();
            this.m_toolStripSkin.SetDefaultValue();
        }
    }

    /// <summary>
    /// 梯度渐变颜色类
    /// </summary>
    [TypeConverter(typeof(GradientColorConverter))]
    [Serializable]
    public class GradientColor
    {
        private Color m_startColor;
        private Color m_endColor;

        /// <summary>
        /// 获取或设置渐变起始颜色
        /// </summary>
        [Description("获取或设置渐变起始颜色")]
        [DefaultValue(typeof(Color), "Control")]
        public Color StartColor
        {
            get { return this.m_startColor; }
            set { this.m_startColor = value; }
        }

        /// <summary>
        /// 获取或设置渐变结束颜色
        /// </summary>
        [Description("获取或设置渐变结束颜色")]
        [DefaultValue(typeof(Color), "Control")]
        public Color EndColor
        {
            get { return this.m_endColor; }
            set { this.m_endColor = value; }
        }

        public GradientColor()
        {
            this.m_startColor = SystemColors.Control;
            this.m_endColor = SystemColors.Control;
        }
    }

    /// <summary>
    /// 单方向梯度渐变类
    /// </summary>
    [TypeConverter(typeof(SingleGradientConverter))]
    [Serializable]
    public class SingleGradient : GradientColor
    {
        private bool m_bHorizontalGradient = true;

        /// <summary>
        /// 获取或设置是采用横向渐变模式还是纵向渐变模式
        /// </summary>
        [Description("获取或设置是采用横向渐变模式还是纵向渐变模式")]
        [DefaultValue(true)]
        public bool HorizontalGradient
        {
            get { return this.m_bHorizontalGradient; }
            set { this.m_bHorizontalGradient = value; }
        }
    }

    /// <summary>
    /// 多方向梯度渐变类
    /// </summary>
    [TypeConverter(typeof(MultiGradientConverter))]
    [Serializable]
    public class MultiGradient : GradientColor
    {
        private LinearGradientMode m_gradientMode;

        /// <summary>
        /// 获取或设置皮肤颜色渐变模式
        /// </summary>
        [Description("获取或设置皮肤颜色渐变模式")]
        [DefaultValue(LinearGradientMode.Horizontal)]
        public LinearGradientMode GradientMode
        {
            get { return this.m_gradientMode; }
            set { this.m_gradientMode = value; }
        }

        public MultiGradient()
        {
            this.m_gradientMode = LinearGradientMode.Horizontal;
        }
    }

    /// <summary>
    /// 用于MedDoc控件中工具条的皮肤类
    /// </summary>
    [TypeConverter(typeof(ToolStripSkinConverter))]
    [Serializable]
    public class ToolStripSkin
    {
        private MultiGradient m_toolStripPanelGradient = null;

        /// <summary>
        /// 获取或设置控件工具条背景皮肤颜色配置
        /// </summary>
        [Description("获取或设置控件工具条背景皮肤颜色配置")]
        public MultiGradient ToolStripPanelGradient
        {
            get { return this.m_toolStripPanelGradient; }
            set { this.m_toolStripPanelGradient = value; }
        }

        private MultiGradient m_paneBackgroundGradient = null;

        /// <summary>
        /// 获取或设置控件工具条区域面板背景皮肤颜色配置
        /// </summary>
        [Description("获取或设置控件工具条区域面板背景皮肤颜色配置")]
        public MultiGradient PaneBackgroundGradient
        {
            get { return this.m_paneBackgroundGradient; }
            set { this.m_paneBackgroundGradient = value; }
        }

        private bool m_bShowCompanyLogo = true;

        /// <summary>
        /// 获取或设置是否显示公司Logo图标
        /// </summary>
        [Description("获取或设置是否显示公司Logo图标")]
        [DefaultValue(true)]
        public bool ShowCompanyLogo
        {
            get { return this.m_bShowCompanyLogo; }
            set { this.m_bShowCompanyLogo = value; }
        }

        public ToolStripSkin()
        {
            this.m_toolStripPanelGradient = new MultiGradient();
            this.m_paneBackgroundGradient = new MultiGradient();

            this.SetDefaultValue();
        }

        /// <summary>
        /// 恢复皮肤颜色为默认值
        /// </summary>
        public void SetDefaultValue()
        {
            if (this.m_toolStripPanelGradient == null)
                this.m_toolStripPanelGradient = new MultiGradient();
            this.m_toolStripPanelGradient.StartColor = Color.FromArgb(226, 238, 255);
            this.m_toolStripPanelGradient.EndColor = Color.FromArgb(123, 164, 224);
            this.m_toolStripPanelGradient.GradientMode = LinearGradientMode.Vertical;

            if (this.m_paneBackgroundGradient == null)
                this.m_paneBackgroundGradient = new MultiGradient();
            this.m_paneBackgroundGradient.StartColor = Color.FromArgb(123, 142, 224);
            this.m_paneBackgroundGradient.EndColor = Color.FromArgb(226, 238, 255);
            this.m_paneBackgroundGradient.GradientMode = LinearGradientMode.Vertical;
        }
    }

    /// <summary>
    /// 用于在VS IDE的窗体设计器中的属性窗口中对MedDoc控件皮肤进行可视化设置的转换器.
    /// </summary>
    public class GradientColorConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(GradientColor))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is GradientColor)
            {
                return "GradientColor";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// 用于在VS IDE的窗体设计器中的属性窗口中对MedDoc控件皮肤进行可视化设置的转换器.
    /// </summary>
    public class SingleGradientConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(SingleGradient))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is SingleGradient)
            {
                return "SingleGradient";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// 用于在VS IDE的窗体设计器中的属性窗口中对MedDoc控件皮肤进行可视化设置的转换器.
    /// </summary>
    public class MultiGradientConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MultiGradient))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is MultiGradient)
            {
                return "MultiGradient";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// 用于在VS IDE的窗体设计器中的属性窗口中对MedDoc控件中工具条皮肤进行可视化设置的转换器.
    /// </summary>
    public class ToolStripSkinConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(ToolStripSkin))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is ToolStripSkin)
            {
                return "ToolStripSkin";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
