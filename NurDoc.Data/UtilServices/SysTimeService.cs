// ***********************************************************
// 护理电子病历系统,系统时钟维护类,
// 避免每次都从数据库读取服务器时间.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class SysTimeService
    {
        private static SysTimeService m_instance = null;

        /// <summary>
        /// 获取服务器时间服务实例
        /// </summary>
        public static SysTimeService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new SysTimeService();
                return m_instance;
            }
        }

        private SystemClock m_systemClock = null;

        private SysTimeService()
        {
            this.m_systemClock =
                new SystemClock(new SystemClock.SyncTimeCallback(this.SyncTime));
        }

        public DateTime Now
        {
            get
            {
                //return this.m_systemClock.Now;
                return DateTime.Now;
            }
        }

        public void Stop()
        {
            this.m_systemClock.Dispose();
        }

        private bool SyncTime(ref DateTime dtNow)
        {
            try
            {
                return SystemContext.Instance.CommonAccess.GetServerTime(ref dtNow) == SystemConst.ReturnValue.OK;
            }
            catch { return false; }
        }
    }
}
