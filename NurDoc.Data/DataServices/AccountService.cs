// ***********************************************************
// 护理电子病历系统,数据访问层接口封装之用户账户访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class AccountService
    {
        private static AccountService m_instance = null;

        /// <summary>
        /// 获取系统账户数据服务实例
        /// </summary>
        public static AccountService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new AccountService();
                return m_instance;
            }
        }

        private AccountService()
        {
        }

        /// <summary>
        /// 判断指定的用户和密码是否能够登录
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szUserPwd">用户密码</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short IsUserValid(string szUserID, string szUserPwd)
        {
            short shRet = ServerData.ExecuteResult.OK;
            try
            {
                shRet = SystemContext.Instance.AccountAccess.VerifyUser(szUserID, szUserPwd);
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowError("登录失败,系统无法验证用户信息!", ex.ToString());
                return SystemConst.ReturnValue.EXCEPTION;
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.FAILED;
            if (shRet == ServerData.ExecuteResult.PARAM_ERROR)
                return SystemConst.ReturnValue.FAILED;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.EXCEPTION;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据用户ID获取该用户的信息
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="userInfo">用户信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUserInfo(string szUserID, ref UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(szUserID))
                return SystemConst.ReturnValue.FAILED;
            try
            {
                short shRet = SystemContext.Instance.AccountAccess.GetUserInfo(szUserID, ref userInfo);
                if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                    return SystemConst.ReturnValue.FAILED;
                if (shRet != ServerData.ExecuteResult.OK)
                    return SystemConst.ReturnValue.EXCEPTION;
                return SystemConst.ReturnValue.OK;
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowErrorFormat("获取“{0}”的用户信息失败!", ex.ToString(), szUserID);
                return SystemConst.ReturnValue.EXCEPTION;
            }
        }

        /// <summary>
        /// 根据用户姓名获取符合该姓名的用户列表
        /// </summary>
        /// <param name="szUserName">用户姓名</param>
        /// <param name="lstUserInfos">用户信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUserInfo(string szUserName, ref List<UserInfo> lstUserInfos)
        {
            if (string.IsNullOrEmpty(szUserName))
                return SystemConst.ReturnValue.FAILED;
            try
            {
                short shRet = SystemContext.Instance.AccountAccess.GetUserInfo(szUserName, ref lstUserInfos);
                return shRet;
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowErrorFormat("获取“{0}”的用户信息失败!", ex.ToString(), szUserName);
                return SystemConst.ReturnValue.EXCEPTION;
            }
        }

        /// <summary>
        /// 验证指定的用户账户及密码
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szUserPwd">密码</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short VerifyUser(string szUserID, string szUserPwd)
        {
            return SystemContext.Instance.AccountAccess.VerifyUser(szUserID, szUserPwd);
        }

        /// <summary>
        /// 获取全院所有用户列表
        /// </summary>
        /// <param name="lstUserInfos">用户列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetAllUserInfos(ref List<UserInfo> lstUserInfos)
        {
            short shRet = SystemContext.Instance.AccountAccess.GetAllUserInfos(ref lstUserInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定用户的科室信息
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="lstDeptInfos">返回的科室信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUserDept(string szUserID, ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = SystemContext.Instance.AccountAccess.GetUserDept(szUserID, ref lstDeptInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取全院所有用户列表
        /// </summary>
        /// <param name="szDeptCode">科室代码</param>
        /// <param name="lstUserInfos">用户列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDeptUserList(string szDeptCode, ref List<UserInfo> lstUserInfos)
        {
            short shRet = SystemContext.Instance.AccountAccess.GetDeptUserList(szDeptCode, ref lstUserInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 验证指定的用户账户及密码
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szOldPwd">旧密码</param>
        /// <param name="szNewPwd">新密码</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short ModifyUserPwd(string szUserID, string szOldPwd, string szNewPwd)
        {
            short shRet = SystemContext.Instance.AccountAccess.ModifyUserPwd(szUserID, szOldPwd, szNewPwd);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.CANCEL;
            if (shRet == ServerData.ExecuteResult.PARAM_ERROR)
                return SystemConst.ReturnValue.FAILED;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        public short ResetUserPwd(string szUserID)
        {
            short shRet = SystemContext.Instance.AccountAccess.ResetUserPwd(szUserID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.CANCEL;
            if (shRet == ServerData.ExecuteResult.PARAM_ERROR)
                return SystemConst.ReturnValue.FAILED;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取所有编辑器用户权限
        /// </summary>
        /// <param name="rightType">用户权限类型</param>
        /// <param name="lstUserRight">用户权限信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUserRight(UserRightType rightType, ref List<UserRightBase> lstUserRight)
        {
            short shRet = SystemContext.Instance.AccountAccess.GetUserRight(rightType, ref lstUserRight);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取编辑器指定用户权限
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="rightType">权限类型</param>
        /// <param name="userRight">用户权限信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUserRight(string szUserID, UserRightType rightType, ref UserRightBase userRight)
        {
            short shRet = SystemContext.Instance.AccountAccess.GetUserRight(szUserID, rightType, ref userRight);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存新的编辑器用户权限
        /// </summary>
        /// <param name="userRight">用户权限</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveUserRight(UserRightBase userRight)
        {
            return SystemContext.Instance.AccountAccess.SaveUserRight(userRight);
        }

        /// <summary>
        /// 更新已有编辑器用户权限
        /// </summary>
        /// <param name="userRight">用户权限</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateUserRight(UserRightBase userRight)
        {
            return SystemContext.Instance.AccountAccess.UpdateUserRight(userRight);
        }

        /// <summary>
        /// 根据用户组代码获取组包含的用户信息列表
        /// </summary>
        /// <param name="szGroupCode">组代码</param>
        /// <param name="lstUserGroups">用户信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetGroupUserList(string szGroupCode, ref List<UserGroup> lstUserGroups)
        {
            short shRet = SystemContext.Instance.AccountAccess.GetGroupUserList(szGroupCode, ref lstUserGroups);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存新的一条用户组数据到用户组信息表
        /// </summary>
        /// <param name="userGroup">用户组信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveUserGroup(UserGroup userGroup)
        {
            short shRet = SystemContext.Instance.AccountAccess.SaveUserGroup(userGroup);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新指定的一条用户组数据到用户组信息表
        /// </summary>
        /// <param name="szGroupCode">组代码</param>
        /// <param name="szUserID">用户ID</param>
        /// <param name="userGroup">用户组信息</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateUserGroup(string szGroupCode, string szUserID, UserGroup userGroup)
        {
            short shRet = SystemContext.Instance.AccountAccess.UpdateUserGroup(szGroupCode, szUserID, userGroup);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 从用户组信息表中删除指定的一条用户组数据
        /// </summary>
        /// <param name="szGroupCode">组代码</param>
        /// <param name="szUserID">用户ID</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteUserGroup(string szGroupCode, string szUserID)
        {
            short shRet = SystemContext.Instance.AccountAccess.DeleteUserGroup(szGroupCode, szUserID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }
    }
}