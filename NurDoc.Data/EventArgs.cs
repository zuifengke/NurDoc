// ***********************************************************
// 护理电子病历系统,全局事件的参数定义类.
// Creator:YangMingkun  Date:2012-8-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    /// <summary>
    /// 登录用户改变前事件参数类定义
    /// </summary>
    public class UserChangingEventArgs : EventArgs
    {
        private UserInfo m_userInfo = null;

        public UserInfo UserInfo
        {
            get { return this.m_userInfo; }
        }

        private bool m_cancel = false;

        public bool Cancel
        {
            get { return this.m_cancel; }
            set { this.m_cancel = value; }
        }

        public UserChangingEventArgs(UserInfo userInfo)
        {
            this.m_userInfo = userInfo;
        }
    }

    public delegate void UserChangingEventHandler(object sender, UserChangingEventArgs e);

    /// <summary>
    /// 登录用户改变后事件参数类定义
    /// </summary>
    public class UserChangedEventArgs : EventArgs
    {
        private UserInfo m_prevUser = null;

        public UserInfo PrevUser
        {
            get { return this.m_prevUser; }
        }

        public UserChangedEventArgs(UserInfo prevUser)
        {
            this.m_prevUser = prevUser;
        }
    }

    public delegate void UserChangedEventHandler(object sender, UserChangedEventArgs e);

    /// <summary>
    /// 请求打开病人窗口事件参数类定义
    /// </summary>
    public class RequestOpenPatientEventArgs : EventArgs
    {
        private string m_patientID = null;

        public string PatientID
        {
            get { return this.m_patientID; }
        }

        private string m_visitID = null;

        public string VisitID
        {
            get { return this.m_visitID; }
        }

        private bool m_cancel = false;

        public bool Cancel
        {
            get { return this.m_cancel; }
            set { this.m_cancel = value; }
        }

        public RequestOpenPatientEventArgs(string patientID, string visitID)
        {
            this.m_patientID = patientID;
            this.m_visitID = visitID;
        }
    }

    public delegate void RequestOpenPatientEventHandler(object sender, RequestOpenPatientEventArgs e);
}
