// ***********************************************************
// 护理电子病历系统外部程序库访问接口服务.
// Creator:YangMingkun  Date:2012-11-4
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Heren.NurDoc.Data
{
    public class ExternService
    {
        public struct HerenCert
        {
            [DllImport("HerenCert.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int VerifyUser(string productName, string userName, string certCode);
        }
    }
}
