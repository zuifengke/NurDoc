// ***********************************************************
// 护理电子病历系统,系统启动参数解析器类.
// Creator:YangMingkun  Date:2012-9-11
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.Startup
{
    public class StartupArgs
    {
        private static StartupArgs m_instance = null;

        /// <summary>
        /// 获取命令行参数类实例
        /// </summary>
        public static StartupArgs Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new StartupArgs();
                return m_instance;
            }
        }

        private StartupArgs()
        {
        }

        private IntPtr m_hHostHandle = IntPtr.Zero;

        /// <summary>
        /// 获取传入的宿主系统句柄
        /// </summary>
        public IntPtr HostHandle
        {
            get { return this.m_hHostHandle; }
        }

        private UserInfo m_userInfo = null;

        /// <summary>
        /// 获取传入的用户ID
        /// </summary>
        public UserInfo UserInfo
        {
            get { return this.m_userInfo; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取传入的病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取传入的病人就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
        }

        private string m_bSinglePatMod = string.Empty;

        /// <summary>
        /// 获取传入的文书开启模式
        /// </summary>
        public bool SinglePatMod
        {
            get
            {
                if (this.m_bSinglePatMod == string.Empty)
                    return false;
                else if (this.m_bSinglePatMod == "0")
                    return false;
                else if (this.m_bSinglePatMod == "1")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 将指定的启动参数解析为所在内存地址
        /// </summary>
        /// <param name="args">启动参数</param>
        /// <returns>IntPtr</returns>
        public IntPtr MakeArgsPtr(params string[] args)
        {
            if (args == null || args.Length <= 0)
                return IntPtr.Zero;

            StringBuilder sbArgsData = new StringBuilder();
            foreach (string arg in args)
            {
                sbArgsData.Append(arg);
            }
            return GlobalMethods.Win32.StringToPtr(sbArgsData.ToString());
        }

        /// <summary>
        /// 解析外部系统传给本系统的启动参数;
        /// 解析成功后会设置本类中的各属性
        /// </summary>
        /// <param name="hArgsData">COPYDATASTRUCT结构句柄</param>
        public void ParsePtrArgs(IntPtr hArgsData)
        {
            this.ParseArrArgs(GlobalMethods.Win32.PtrToString(hArgsData));
        }

        /// <summary>
        /// 解析外部系统传给本系统的启动参数;
        /// 解析成功后会设置本类中的各属性
        /// </summary>
        /// <param name="args">启动参数</param>
        public void ParseArrArgs(params string[] args)
        {
            this.m_hHostHandle = IntPtr.Zero;
            this.m_userInfo = new UserInfo();
            this.m_szPatientID = string.Empty;
            this.m_szVisitID = string.Empty;
            if (args == null || args.Length <= 0)
                throw new Exception("您传入的启动参数为空!");

            string szEscapeChar = SystemConst.StartupArgs.ESCAPE_CHAR;
            string szEscapedChar = SystemConst.StartupArgs.ESCAPED_CHAR;
            StringBuilder sbArgsData = new StringBuilder();
            foreach (string arg in args)
                sbArgsData.Append(arg);
            string szArgsData = sbArgsData.ToString().Replace(szEscapeChar, szEscapedChar);

            int nStartIndex = 0;
            int nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex < nStartIndex)
                throw new Exception("宿主句柄参数不能为空!");
            string szHostHandle = szArgsData.Substring(nStartIndex, nFieldIndex);
            this.m_hHostHandle = (IntPtr)GlobalMethods.Convert.StringToValue(szHostHandle, 0);

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.ID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex).ToUpper();
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.ID))
                throw new Exception("用户ID参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.Name = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.Name))
                throw new Exception("用户姓名参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex < nStartIndex)
                throw new Exception("您必须传入密码参数!");
            this.m_userInfo.Password = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.DeptCode = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.DeptCode))
                throw new Exception("用户科室代码参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.DeptName = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.DeptName))
                throw new Exception("用户科室名称参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.WardCode = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.WardCode))
                throw new Exception("用户病区代码参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_userInfo.WardName = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_userInfo.WardName))
                throw new Exception("用户病区名称参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            if (nStartIndex >= szArgsData.Length)
                return;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.FIELD_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex)
                this.m_szPatientID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_szPatientID))
                throw new Exception("病人ID参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex) this.m_szVisitID = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (GlobalMethods.Misc.IsEmptyString(this.m_szVisitID))
                throw new Exception("病人就诊ID参数不能为空!");

            nStartIndex = nFieldIndex + 1;
            nFieldIndex = szArgsData.IndexOf(SystemConst.StartupArgs.GROUP_SPLIT, nStartIndex);
            if (nFieldIndex > nStartIndex) this.m_bSinglePatMod = szArgsData.Substring(nStartIndex, nFieldIndex - nStartIndex);
            if (nFieldIndex < 0) this.m_bSinglePatMod = "0";
            //if (GlobalMethods.Misc.IsEmptyString(this.m_bSinglePatMod))
            //    throw new Exception("打开模式参数不能为空!");
        }
    }
}
