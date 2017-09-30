using Heren.Common.Libraries;
using Heren.Common.RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

namespace Heren.NurDoc.DAL
{

    #region"RestResult"
    /// <summary>
    /// REST服务器返回的结果数据类型
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    internal class RestResult<T>
    {
        private T m_objectValue = default(T);
        /// <summary>
        /// 获取或设置自定义对象值
        /// </summary>
        public T ObjectValue
        {
            get { return this.m_objectValue; }
            set { this.m_objectValue = value; }
        }

        private List<T> m_objectListValue = null;
        /// <summary>
        /// 获取或设置自定义对象列表值
        /// </summary>
        public List<T> ObjectListValue
        {
            get { return this.m_objectListValue; }
            set { this.m_objectListValue = value; }
        }

        private short m_returnValue = 0;
        /// <summary>
        /// 获取或设置查询结果返回值
        /// </summary>
        public short ReturnValue
        {
            get { return this.m_returnValue; }
            set { this.m_returnValue = value; }
        }
    }
    #endregion

    #region"RestParameter"
    /// <summary>
    /// REST服务器返回的结果数据类型
    /// </summary>
    internal class RestParameter
    {
        protected object m_parameter1 = null;
        /// <summary>
        /// 获取或设置参数1
        /// </summary>
        public virtual object Parameter1
        {
            get { return this.m_parameter1; }
            set { this.m_parameter1 = value; }
        }

        protected object m_parameter2 = null;
        /// <summary>
        /// 获取或设置参数2
        /// </summary>
        public virtual object Parameter2
        {
            get { return this.m_parameter2; }
            set { this.m_parameter2 = value; }
        }

        public RestParameter()
        {
        }

        public RestParameter(object parameter1, object parameter2)
        {
            this.m_parameter1 = parameter1;
            this.m_parameter2 = parameter2;
        }
    }
    #endregion


    public class RestHandler
    {
        private static RestHandler m_instance = null;
        /// <summary>
        /// 获取REST服务访问处理器实例
        /// </summary>
        public static RestHandler Instance
        {
            get
            {
                if (m_instance == null)
                {
                    string url = GetNurDocRestURL();
                    m_instance = new RestHandler(url);
                }
                return m_instance;
            }
        }

        private string m_url = null;
        public RestHandler(string url)
        {
            this.m_url = url;
            ServicePointManager.DefaultConnectionLimit = 512;
        }

        private RestRequest m_request = null;
        /// <summary>
        /// 获取REST服务访问请求对象
        /// </summary>
        private RestRequest Request
        {
            get
            {
                if (this.m_request == null)
                    this.m_request = new RestRequest();
                return this.m_request;
            }
        }

        private RestClient m_client = null;
        /// <summary>
        /// 获取REST服务访问客户端
        /// </summary>
        private RestClient Client
        {
            get
            {
                if (this.m_client == null)
                    this.m_client = new RestClient(this.m_url);
                return this.m_client;
            }
        }

        /// <summary>
        /// 获取REST服务访问请求超时时间
        /// </summary>
        public int Timeout
        {
            get { return this.Client.Timeout; }
            set
            {
                if (value > 0)
                {
                    this.Client.Timeout = value;
                    this.Client.ReadWriteTimeout = value;
                }
                else
                {
                    this.Client.Timeout = 100000;//100秒
                    this.Client.ReadWriteTimeout = 300000;//300秒
                }
            }
        }

        #region"URL"
        /// <summary>
        /// 获取MedDoc Rest服务URL
        /// </summary>
        /// <returns>URL字符串</returns>
        private static string GetNurDocRestURL()
        {
            string url = ServerParam.Instance.GetRestAccess();
            return url;
        }

        ///// <summary>
        ///// 获取XMLDB Rest服务URL
        ///// </summary>
        ///// <returns>URL字符串</returns>
        //private static string GetXMLDBRestURL()
        //{
        //    string key = SystemConsts.ConfigFile.REST_SERVICE_URL_XDB;
        //    string url = SystemConfig.Instance.Get(key, string.Empty);

        //    key = SystemConsts.ConfigFile.CONFIG_ENCRYPT_KEY;
        //    url = GlobalMethods.Security.DecryptText(url, key);
        //    return url;
        //}
        #endregion

        #region"Parameter"
        /// <summary>
        /// 调用新的接口前,清空参数集合
        /// </summary>
        public void ClearParameters()
        {
            if (this.m_request == null)
                return;
            this.m_request.Parameters.Clear();
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(byte[] value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddJsonBody(value);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string[] value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddJsonBody(value);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(List<string> value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddJsonBody(value);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter<T>(T value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddJsonBody(value);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter<T>(T[] value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddJsonBody(value);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter<T>(List<T> value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddJsonBody(value);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        public void AddParameter(object param1, object param2)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddJsonBody(new RestParameter(param1, param2));
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string name, string value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string name, bool value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string name, Enum value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string name, int value)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            this.m_request.AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>
        /// 调用新的接口前,添加新的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="date">参数值</param>
        public void AddParameter(string name, DateTime date)
        {
            if (this.m_request == null)
                this.m_request = new RestRequest();
            string value = string.Concat(
                date.Year, "-", date.Month, "-", date.Day, " ",
                date.Hour, ":", date.Minute, ":", date.Second);
            this.m_request.AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>
        /// 获取当前添加的参数名集合
        /// </summary>
        /// <returns>参数名集合</returns>
        private string[] GetParameterNames()
        {
            if (this.m_request == null)
                return new string[0];

            string[] names = new string[this.m_request.Parameters.Count + 2];
            names[0] = "resource";
            names[1] = "method";
            for (int index = 0; index < names.Length - 2; index++)
            {
                names[index + 2] = this.m_request.Parameters[index].Name;
            }
            return names;
        }

        /// <summary>
        /// 获取当前添加的参数值集合
        /// </summary>
        /// <returns>参数值集合</returns>
        private string[] GetParameterValues()
        {
            if (this.m_request == null)
                return new string[0];

            string[] values = new string[this.m_request.Parameters.Count + 2];
            values[0] = this.m_request.Resource;
            values[1] = this.m_request.Method.ToString();
            for (int index = 0; index < values.Length - 2; index++)
            {
                Parameter parameter = this.m_request.Parameters[index];
                if (parameter.Value == null)
                    values[index + 2] = "null";
                else
                    values[index + 2] = parameter.Value.ToString();
            }
            return values;
        }
        #endregion

        #region"Put"
        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put(string method)
        {
            RestResult<object> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put(string method, ref string value)
        {
            RestResult<string> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的整型数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put(string method, ref int value)
        {
            RestResult<int> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的日期数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put(string method, ref DateTime value)
        {
            RestResult<DateTime> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字节数组数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put(string method, ref byte[] value)
        {
            RestResult<byte[]> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数组数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put(string method, ref string[] value)
        {
            RestResult<List<string>> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                if (result.ObjectValue != null)
                    value = result.ObjectValue.ToArray();
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的对象数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put<T>(string method, ref T value)
        {
            RestResult<T> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,返回一个对象集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的对象数据集合</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Put<T>(string method, ref List<T> values)
        {
            RestResult<T> result = null;
            short returnValue = this.Execute(method, Method.PUT, ref result);
            if (result != null)
            {
                values = result.ObjectListValue;
                return result.ReturnValue;
            }
            return returnValue;
        }
        #endregion

        #region"Post"
        /// <summary>
        /// 以POST请求方式调用REST服务接口,更新数据库
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post(string method)
        {
            RestResult<object> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post(string method, ref string value)
        {
            RestResult<string> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的整型数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post(string method, ref int value)
        {
            RestResult<int> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的日期数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post(string method, ref DateTime value)
        {
            RestResult<DateTime> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字节数组数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post(string method, ref byte[] value)
        {
            RestResult<byte[]> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数组数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post(string method, ref string[] value)
        {
            RestResult<List<string>> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                if (result.ObjectValue != null)
                    value = result.ObjectValue.ToArray();
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以POST请求方式调用REST服务接口,更新数据库,同时返回一个对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的对象数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post<T>(string method, ref T value)
        {
            RestResult<T> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以POST请求方式调用REST服务接口,更新数据库,返回一个对象集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的对象数据集合</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Post<T>(string method, ref List<T> values)
        {
            RestResult<T> result = null;
            short returnValue = this.Execute(method, Method.POST, ref result);
            if (result != null)
            {
                values = result.ObjectListValue;
                return result.ReturnValue;
            }
            return returnValue;
        }
        #endregion

        #region"Delete"
        /// <summary>
        /// 以DELETE请求方式调用REST服务接口,删除数据
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Delete(string method)
        {
            RestResult<object> result = null;
            short returnValue = this.Execute(method, Method.DELETE, ref result);
            if (result != null)
            {
                return result.ReturnValue;
            }
            return returnValue;
        }
        #endregion

        #region"Get"
        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get(string method)
        {
            RestResult<string> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get(string method, ref string value)
        {
            RestResult<string> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的整型数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get(string method, ref int value)
        {
            RestResult<int> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的日期数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get(string method, ref DateTime value)
        {
            RestResult<DateTime> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字节数组数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get(string method, ref byte[] value)
        {
            RestResult<byte[]> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数组数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get(string method, ref string[] value)
        {
            RestResult<List<string>> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                if (result.ObjectValue != null)
                    value = result.ObjectValue.ToArray();
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的对象数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get<T>(string method, ref T value)
        {
            RestResult<T> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                value = result.ObjectValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个对象集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的对象数据集合</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get<T>(string method, ref List<T> values)
        {
            RestResult<T> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result != null)
            {
                if (result.ObjectListValue == null)
                    return result.ReturnValue;
                values = result.ObjectListValue;
                return result.ReturnValue;
            }
            return returnValue;
        }

        /// <summary>
        /// 以GET请求方式调用REST服务接口,查询数据库,返回一个数据集
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的数据集</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short Get(string method, ref DataSet value)
        {
            RestResult<List<string>> result = null;
            short returnValue = this.Execute(method, Method.GET, ref result);
            if (result == null)
                return returnValue;

            if (result.ObjectListValue == null || result.ObjectListValue.Count <= 1)
                return result.ReturnValue;

            value = new DataSet();
            DataTable table = value.Tables.Add();

            bool isAllColumnsTextType = true;
            List<string> dataTypes = result.ObjectListValue[0];
            List<string> dataTypes2 = result.ObjectListValue[1];
            if (dataTypes[0] != null && dataTypes[0].Contains("{\""))
            {
                Dictionary<string, string> lstDict = SimpleJson.DeserializeObject<Dictionary<string, string>>(dataTypes[0]);
                Dictionary<string, object> lstDict2 = SimpleJson.DeserializeObject<Dictionary<string, object>>(dataTypes2[0]);
                List<string> lstColName = new List<string>();
                List<string> lstColType = new List<string>();
                List<string> lstColName2 = new List<string>();
                List<object> lstColValue = new List<object>();
                foreach (string key in lstDict.Keys)
                {
                    lstColName.Add(key);
                }
                foreach (string szValue in lstDict.Values)
                {
                    lstColType.Add(szValue);
                }
                foreach (string key in lstDict2.Keys)
                {
                    lstColName2.Add(key);
                }

                for (int index = 0; lstColName2 != null && index < lstColName2.Count; index++)
                {
                    string colName = string.Empty;
                    foreach (char chr in lstColName2[index])
                    {
                        if (char.IsUpper(chr))
                        {
                            colName += "_" + chr;
                        }
                        else { colName += chr; }
                    }

                    string colType = string.Empty;
                    for (int j = 0; lstColName != null && lstColType != null && j < lstColType.Count; j++)
                    {
                        if (lstColName[j] == lstColName2[index])
                        {
                            colType = lstColType[j];
                            break;
                        }
                    }
                    switch (colType.ToLower())
                    {
                        case "byte[]":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(byte[]));
                            break;
                        case "short":
                        case "integer":
                        case "long":
                        case "float":
                        case "double":
                        case "bigdecimal":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(decimal));
                            break;
                        case "boolean":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(bool));
                            break;
                        case "date":
                        case "timestamp":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(DateTime));
                            break;
                        default:
                            table.Columns.Add(index.ToString(), typeof(string));
                            break;
                    }
                    table.Columns[index].ColumnName = colName.ToLower();
                }
                for (int listIndex = 1; listIndex < result.ObjectListValue.Count; listIndex++)
                {
                    List<string> list = result.ObjectListValue[listIndex];
                    if (list == null)
                        continue;
                    lstColName2 = new List<string>();
                    lstColValue = new List<object>();
                    lstDict2 = SimpleJson.DeserializeObject<Dictionary<string, object>>(list[0]);

                    foreach (string key in lstDict2.Keys)
                    {
                        lstColName2.Add(key);
                    }
                    foreach (object szValue in lstDict2.Values)
                    {
                        lstColValue.Add(szValue);
                    }
                    if (isAllColumnsTextType)
                    {
                        table.Rows.Add(lstColValue.ToArray());
                        continue;
                    }
                    object[] array = new object[lstColValue.Count];
                    for (int index = 0; index < lstColValue.Count; index++)
                    {
                        string colType = string.Empty;
                        for (int j = 0; lstColName != null && lstColType != null && j < lstColType.Count; j++)
                        {
                            if (lstColName[j] == lstColName2[index])
                            {
                                colType = lstColType[j];
                                break;
                            }
                        }
                        switch (colType.ToLower())
                        {
                            case "byte[]":
                                byte[] data = null;
                                GlobalMethods.Convert.Base64ToBytes(lstColValue[index].ToString(), ref data);
                                array[index] = data;
                                break;
                            case "short":
                            case "integer":
                            case "long":
                            case "float":
                            case "double":
                            case "bigdecimal":
                                array[index] = GlobalMethods.Convert.StringToValue(lstColValue[index], (decimal)0);
                                break;
                            case "boolean":
                                array[index] = GlobalMethods.Convert.StringToValue(lstColValue[index], false);
                                break;
                            case "date":
                            case "timestamp":
                                decimal ticks = GlobalMethods.Convert.StringToValue(lstColValue[index], (decimal)0);
                                array[index] = GlobalMethods.SysTime.GetJavaTime((double)ticks);
                                break;
                            default:
                                array[index] = lstColValue[index];
                                break;
                        }
                    }
                    table.Rows.Add(array);
                }
            }
            else
            {
                for (int index = 0; dataTypes != null && index < dataTypes.Count; index++)
                {
                    string typeName = dataTypes[index];
                    if (string.IsNullOrEmpty(typeName))
                        typeName = string.Empty;
                    switch (typeName.ToLower())
                    {
                        case "byte[]":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(byte[]));
                            break;
                        case "short":
                        case "integer":
                        case "long":
                        case "float":
                        case "double":
                        case "bigdecimal":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(decimal));
                            break;
                        case "boolean":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(bool));
                            break;
                        case "date":
                        case "timestamp":
                            isAllColumnsTextType = false;
                            table.Columns.Add(index.ToString(), typeof(DateTime));
                            break;
                        default:
                            table.Columns.Add(index.ToString(), typeof(string));
                            break;
                    }
                }

                for (int listIndex = 1; listIndex < result.ObjectListValue.Count; listIndex++)
                {
                    List<string> list = result.ObjectListValue[listIndex];
                    if (list == null)
                        continue;
                    if (isAllColumnsTextType)
                    {
                        table.Rows.Add(list.ToArray());
                        continue;
                    }
                    object[] array = new object[list.Count];
                    for (int index = 0; index < list.Count; index++)
                    {
                        string typeName = dataTypes[index];
                        if (string.IsNullOrEmpty(typeName))
                            typeName = string.Empty;
                        switch (typeName.ToLower())
                        {
                            case "byte[]":
                                byte[] data = null;
                                GlobalMethods.Convert.Base64ToBytes(list[index], ref data);
                                array[index] = data;
                                break;
                            case "short":
                            case "integer":
                            case "long":
                            case "float":
                            case "double":
                            case "bigdecimal":
                                array[index] = GlobalMethods.Convert.StringToValue(list[index], (decimal)0);
                                break;
                            case "boolean":
                                array[index] = GlobalMethods.Convert.StringToValue(list[index], false);
                                break;
                            case "date":
                            case "timestamp":
                                decimal ticks = GlobalMethods.Convert.StringToValue(list[index], (decimal)0);
                                array[index] = GlobalMethods.SysTime.GetJavaTime((double)ticks);
                                break;
                            default:
                                array[index] = list[index];
                                break;
                        }
                    }
                    table.Rows.Add(array);
                }
            }
            return result.ReturnValue;
        }
        #endregion

        /// <summary>
        /// 以PUT请求方式调用REST服务接口,操作数据库,同时返回结果数据
        /// </summary>
        /// <param name="method">接口地址</param>
        /// <param name="value">返回的字符串数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        protected virtual short Execute<T>(string method, Method command, ref T result) where T : new()
        {
            this.Request.Resource = method;
            this.Request.Method = command;

            IRestResponse<T> response = null;
            try
            {
                response = this.Client.Execute<T>(this.Request);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("RestHandler.Execute<T>"
                    , this.GetParameterNames(), this.GetParameterValues(), ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }

            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
            {
                result = response.Data;
                return ServerData.ExecuteResult.OK;
            }
            else
            {
                StringBuilder exceptionMessage = new StringBuilder();
                exceptionMessage.AppendLine("ErrorMessage：" + response.ErrorMessage);
                exceptionMessage.Append("    ResponseContent：" + response.Content);
                LogManager.Instance.WriteLog("RestHandler.Execute<T>", this.GetParameterNames()
                    , this.GetParameterValues(), response.StatusDescription, new Exception(exceptionMessage.ToString()));
                return ServerData.ExecuteResult.EXCEPTION;
            }
        }

    }
}
