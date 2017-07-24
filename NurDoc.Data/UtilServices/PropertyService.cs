// ***********************************************************
// ������Ӳ���ϵͳ,�������Դ�����
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
        /// ��ȡPropertyServiceʵ��
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
        /// ��ָ�������л�ȡָ�����Ƶ�������Ϣ
        /// </summary>
        /// <param name="type">ָ������</param>
        /// <param name="szPropertyName">ָ������������</param>
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
        /// ��ָ���Ķ���ʵ���л�ȡָ�������Ե�ֵ
        /// </summary>
        /// <param name="obj">ָ���Ķ���ʵ��</param>
        /// <param name="propertyInfo">ָ��������</param>
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
        /// ��ָ�������ݶ����л�ȡ��ָ�����Ե�ֵ
        /// </summary>
        /// <param name="target">ָ���Ķ���ʵ��</param>
        /// <param name="szPropertyText">���������б�</param>
        /// <param name="objValue">���ص�����ֵ</param>
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
        /// ʹ��args�����ڵ���������滻szFormatText�ַ�������"{}"��ʾ��ռλ��
        /// </summary>
        /// <param name="szFormatText">ָ�����ַ���</param>
        /// <param name="args">�����滻ռλ���Ĳ���</param>
        /// <returns>���滻�ɹ���ô���ط�null,��������쳣�򷵻�null</returns>
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
