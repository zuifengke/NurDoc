// ***********************************************************
// 护理电子病历系统,对象属性处理类
// Creator:OuFengFang  Date:2013-2-3
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

using Heren.Common.Libraries;

namespace Heren.NurDoc.Data
{
    public class PropertyService
    {
        private static PropertyService m_instance = null;

        /// <summary>
        /// 获取PropertyService实例
        /// </summary>
        public static PropertyService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PropertyService();
                return m_instance;
            }
        }

        private PropertyService()
        {
        }

        /// <summary>
        /// 从指定类型中获取指定名称的属性信息
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <param name="szPropertyName">指定的属性名称</param>
        /// <returns>PropertyInfo</returns>
        public PropertyInfo GetPropertyInfo(Type type, string szPropertyName)
        {
            if (type == null || GlobalMethods.Misc.IsEmptyString(szPropertyName))
                return null;
            try
            {
                return type.GetProperty(szPropertyName);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("PropertyService.GetPropertyInfo", ex);
                return null;
            }
        }

        /// <summary>
        /// 从指定的对象实例中获取指定的属性的值
        /// </summary>
        /// <param name="obj">指定的对象实例</param>
        /// <param name="propertyInfo">指定的属性</param>
        /// <returns>object</returns>
        public object GetPropertyValue(object obj, PropertyInfo propertyInfo)
        {
            if (obj == null || propertyInfo == null)
                return null;
            try
            {
                return propertyInfo.GetValue(obj, null);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("PropertyService.GetPropertyValue", ex);
                return null;
            }
        }

        /// <summary>
        /// 从指定的数据对象中获取的指定属性的值
        /// </summary>
        /// <param name="target">指定的对象实例</param>
        /// <param name="szPropertyText">属性名称列表</param>
        /// <param name="objValue">返回的属性值</param>
        /// <returns>bool</returns>
        public bool GetPropertyValue(object target,string szPropertyText, ref object objValue)
        {
            string[] arrPropertyName = szPropertyText.Split('.');
            if (arrPropertyName == null)
                return false;
                PropertyInfo propertyInfo = null;
            for (int index = 0; index < arrPropertyName.Length; index++)
            {
                string szPropertyName = arrPropertyName[index];
                if (target == null || string.IsNullOrEmpty(szPropertyName))
                    continue;
                propertyInfo = this.GetPropertyInfo(target.GetType(), szPropertyName);
                if (propertyInfo != null)
                    target = this.GetPropertyValue(target, propertyInfo);
            }
            if (target != this) objValue = target;
            return true;
        }

        /// <summary>
        /// 使用args数组内的数据逐个替换szFormatText字符串中以"{}"表示的占位符
        /// </summary>
        /// <param name="szFormatText">指定的字符串</param>
        /// <param name="args">用于替换占位符的参数</param>
        /// <returns>当替换成功那么返回非null,否则出现异常则返回null</returns>
        public  string FormatString(string szFormatText, params string[] args)
        {
            try
            {
                return string.Format(szFormatText, args);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("PropertyService.FormatString", ex);
                return null;
            }
        }
    }
}
