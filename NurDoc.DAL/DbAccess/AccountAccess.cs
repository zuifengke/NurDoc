// ***********************************************************
// 护理病历系统数据库访问层用户账户访问有关的数据访问接口.
// Creator:YangMingkun  Date:2012-3-31
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class AccountAccess : DBAccessBase
    {
        /// <summary>
        /// 验证指定的用户账户及密码
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szUserPwd">密码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short VerifyUser(string szUserID, string szUserPwd)
        {
            //获取数据库中的用户及密码
            if (GlobalMethods.Misc.IsEmptyString(szUserID))
            {
                LogManager.Instance.WriteLog("AccountAccess.VerifyUser", "用户ID不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.EXCEPTION;

            //先获取用户信息
            string szField = ServerData.UserRightTable.USER_PWD;
            string szCondition = string.Format("{0}='{1}'", ServerData.UserRightTable.USER_ID, szUserID);
            string szTable = ServerData.DataTable.USER_RIGHT;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            string szPwdText = null;
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (!dataReader.IsDBNull(0)) szPwdText = dataReader.GetString(0).Trim();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }

            //验证密码
            if (GlobalMethods.Misc.IsEmptyString(szPwdText))
            {
                if (GlobalMethods.Misc.IsEmptyString(szUserPwd))
                    return ServerData.ExecuteResult.OK;
                else
                    return ServerData.ExecuteResult.PARAM_ERROR;
            }
            return (GlobalMethods.Security.GetSummaryMD5(szUserPwd) == szPwdText.ToUpper()) ?
                ServerData.ExecuteResult.OK : ServerData.ExecuteResult.PARAM_ERROR;
        }

        /// <summary>
        /// 验证指定的用户账户及密码
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="szOldPwd">旧密码</param>
        /// <param name="szNewPwd">新密码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyUserPwd(string szUserID, string szOldPwd, string szNewPwd)
        {
            if (szOldPwd == null || szOldPwd.Trim() == string.Empty)
                szOldPwd = string.Empty;

            //验证用户及密码
            short shRet = this.VerifyUser(szUserID, szOldPwd);
            if (shRet != ServerData.ExecuteResult.OK)
                return shRet;

            if (szNewPwd == null || szNewPwd.Trim() == string.Empty)
                szNewPwd = string.Empty;

            if (szOldPwd.Trim() == szNewPwd.Trim())
                return ServerData.ExecuteResult.OK;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.EXCEPTION;

            if (szNewPwd != string.Empty)
                szNewPwd = GlobalMethods.Security.GetSummaryMD5(szNewPwd);

            string szTable = ServerData.DataTable.USER_RIGHT;
            string szField = string.Format("{0}='{1}'", ServerData.UserRightTable.USER_PWD, szNewPwd);
            string szCondition = string.Format("{0}='{1}'", ServerData.UserRightTable.USER_ID, szUserID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.ACCESS_ERROR;
        }

        /// <summary>
        /// 将指定用户密码置空
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ResetUserPwd(string szUserID)
        {
            if (szUserID == null || szUserID.Trim() == string.Empty)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szNewPwd = string.Empty;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.EXCEPTION;

            if (szNewPwd != string.Empty)
                szNewPwd = GlobalMethods.Security.GetSummaryMD5(szNewPwd);

            string szTable = ServerData.DataTable.USER_RIGHT;
            string szField = string.Format("{0}='{1}'", ServerData.UserRightTable.USER_PWD, szNewPwd);
            string szCondition = string.Format("{0}='{1}'", ServerData.UserRightTable.USER_ID, szUserID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.ACCESS_ERROR;
        }

        /// <summary>
        /// 查询获取指定的用户是否已配置权限
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="rightType">用户权限类型</param>
        /// <param name="nCount">返回的记录数</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short ExistRightInfo(string szUserID, UserRightType rightType, ref int nCount)
        {
            if (GlobalMethods.Misc.IsEmptyString(szUserID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'AND {2}='{3}'"
                , ServerData.UserRightTable.USER_ID, szUserID
                , ServerData.UserRightTable.RIGHT_TYPE, UserRightBase.GetRightTypeName(rightType));
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, "COUNT(*)", ServerData.DataTable.USER_RIGHT, szCondition);

            nCount = 0;
            try
            {
                object objValue = base.DataAccess.ExecuteScalar(szSQL, CommandType.Text);
                if (objValue == null || objValue == System.DBNull.Value)
                    nCount = 0;
                if (!int.TryParse(objValue.ToString(), out nCount))
                    nCount = 0;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取所有编辑器用户权限
        /// </summary>
        /// <param name="rightType">用户权限类型</param>
        /// <param name="lstUserRight">用户权限信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetUserRight(UserRightType rightType, ref List<UserRightBase> lstUserRight)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2}"
                , ServerData.UserRightTable.USER_ID, ServerData.UserRightTable.RIGHT_CODE
                , ServerData.UserRightTable.RIGHT_DESC);
            string szTable = ServerData.DataTable.USER_RIGHT;
            string szCondition = string.Format("{0}='{1}'", ServerData.UserRightTable.RIGHT_TYPE
                , UserRightBase.GetRightTypeName(rightType));
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstUserRight == null)
                    lstUserRight = new List<UserRightBase>();
                lstUserRight.Clear();

                do
                {
                    UserRightBase userRight = UserRightBase.Create(rightType);
                    userRight.UserID = dataReader.GetString(0).Trim();
                    if (!dataReader.IsDBNull(2))
                        userRight.RightDesc = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(1))
                        userRight.SetRightCode(dataReader.GetString(1));
                    lstUserRight.Add(userRight);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取编辑器指定用户权限
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="rightType">权限类型</param>
        /// <param name="userRight">用户权限信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetUserRight(string szUserID, UserRightType rightType, ref UserRightBase userRight)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2}"
                , ServerData.UserRightTable.USER_ID, ServerData.UserRightTable.RIGHT_CODE
                , ServerData.UserRightTable.RIGHT_DESC);
            string szTable = ServerData.DataTable.USER_RIGHT;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.UserRightTable.USER_ID, szUserID
                , ServerData.UserRightTable.RIGHT_TYPE, UserRightBase.GetRightTypeName(rightType));
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                userRight = UserRightBase.Create(rightType);
                userRight.UserID = dataReader.GetString(0).Trim();
                if (!dataReader.IsDBNull(2))
                    userRight.RightDesc = dataReader.GetString(2);
                if (!dataReader.IsDBNull(1))
                    userRight.SetRightCode(dataReader.GetString(1));
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 保存新的编辑器用户权限
        /// </summary>
        /// <param name="userRight">用户权限</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveUserRight(UserRightBase userRight)
        {
            if (userRight == null || GlobalMethods.Misc.IsEmptyString(userRight.UserID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            int count = 0;
            short shRet = this.ExistRightInfo(userRight.UserID, userRight.RightType, ref count);
            if (shRet != ServerData.ExecuteResult.OK)
                return shRet;
            if (count > 0)
                return this.UpdateUserRight(userRight);

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.UserRightTable.USER_ID, ServerData.UserRightTable.RIGHT_CODE
                , ServerData.UserRightTable.RIGHT_DESC, ServerData.UserRightTable.RIGHT_TYPE);
            string szValue = string.Format("'{0}','{1}','{2}','{3}'"
                , userRight.UserID, userRight.GetRightCode()
                , userRight.RightDesc, UserRightBase.GetRightTypeName(userRight.RightType));
            string szTable = ServerData.DataTable.USER_RIGHT;
            string szSQL = string.Format(ServerData.SQL.INSERT, szTable, szField, szValue);

            count = 0;
            try
            {
                count = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (count > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.ACCESS_ERROR;
        }

        /// <summary>
        /// 更新已有编辑器用户权限
        /// </summary>
        /// <param name="userRight">用户权限</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateUserRight(UserRightBase userRight)
        {
            if (userRight == null || GlobalMethods.Misc.IsEmptyString(userRight.UserID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}'", ServerData.UserRightTable.RIGHT_CODE, userRight.GetRightCode());
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.UserRightTable.USER_ID, userRight.UserID
                , ServerData.UserRightTable.RIGHT_TYPE, UserRightBase.GetRightTypeName(userRight.RightType));
            string szTable = ServerData.DataTable.USER_RIGHT;
            string szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);

            int count = 0;
            try
            {
                count = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (count <= 0) ? ServerData.ExecuteResult.RES_NO_FOUND : ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 根据用户ID获取指定的用户信息
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="userInfo">返回的用户信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetUserInfo(string szUserID, ref UserInfo userInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szUserID))
            {
                LogManager.Instance.WriteLog("AccountAccess.GetUserInfo", "用户ID不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //先获取用户信息
            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.UserView.USER_ID, ServerData.UserView.USER_NAME
                , ServerData.UserView.DEPT_CODE, ServerData.UserView.DEPT_NAME);
            string szCondition = string.Format("{0}='{1}'", ServerData.UserView.USER_ID, szUserID);
            string szTable = ServerData.DataView.USER;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (userInfo == null)
                    userInfo = new UserInfo();
                if (!dataReader.IsDBNull(0)) userInfo.ID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1)) userInfo.Name = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2)) userInfo.DeptCode = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3)) userInfo.DeptName = dataReader.GetString(3);
                userInfo.WardCode = userInfo.DeptCode;
                userInfo.WardName = userInfo.DeptName;
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据用户名获取指定的用户信息
        /// </summary>
        /// <param name="szUserName">用户名</param>
        /// <param name="lstUserInfo">用户信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetUserInfo(string szUserName, ref List<UserInfo> lstUserInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szUserName))
            {
                LogManager.Instance.WriteLog("AccountAccess.GetUserInfo", "用户姓名不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //先获取用户信息
            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.UserView.USER_ID, ServerData.UserView.USER_NAME
                , ServerData.UserView.DEPT_CODE, ServerData.UserView.DEPT_NAME);
            string szCondition = string.Format("{0} like '%{1}%'", ServerData.UserView.USER_NAME, szUserName);
            string szTable = ServerData.DataView.USER;
            string szOrder = ServerData.UserView.DEPT_CODE;
            string szSQL = string.Format(ServerData.SQL.SELECT_DISTINCT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstUserInfo == null)
                    lstUserInfo = new List<UserInfo>();
                do
                {
                    UserInfo userInfo = new UserInfo();
                    if (!dataReader.IsDBNull(0)) userInfo.ID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) userInfo.Name = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) userInfo.DeptCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) userInfo.DeptName = dataReader.GetString(3);
                    userInfo.WardCode = userInfo.DeptCode;
                    userInfo.WardName = userInfo.DeptName;
                    lstUserInfo.Add(userInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取全院所有用户列表
        /// </summary>
        /// <param name="lstUserInfos">用户列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAllUserInfos(ref List<UserInfo> lstUserInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //先获取用户信息
            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.UserView.USER_ID, ServerData.UserView.USER_NAME
                , ServerData.UserView.DEPT_CODE, ServerData.UserView.DEPT_NAME);
            string szTable = ServerData.DataView.USER;
            string szOrder = ServerData.UserView.USER_NAME;
            string szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField, szTable, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstUserInfos == null)
                    lstUserInfos = new List<UserInfo>();
                do
                {
                    UserInfo userInfo = new UserInfo();
                    if (!dataReader.IsDBNull(0)) userInfo.ID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) userInfo.Name = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) userInfo.DeptCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) userInfo.DeptName = dataReader.GetString(3);
                    userInfo.WardCode = userInfo.DeptCode;
                    userInfo.WardName = userInfo.DeptName;
                    lstUserInfos.Add(userInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定用户的科室信息
        /// </summary>
        /// <param name="szUserID">用户ID</param>
        /// <param name="lstDeptInfos">返回的科室信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetUserDept(string szUserID, ref List<DeptInfo> lstDeptInfos)
        {
            if (GlobalMethods.Misc.IsEmptyString(szUserID))
            {
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
            {
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            string szField = string.Format("A.{0},B.{1},B.{2},B.{3},B.{4},B.{5},B.{6}"
                , ServerData.UserDeptView.DEPT_CODE, ServerData.DeptView.DEPT_NAME
                , ServerData.DeptView.IS_CLINIC, ServerData.DeptView.IS_WARD
                , ServerData.DeptView.IS_OUTP, ServerData.DeptView.IS_NURSE
                , ServerData.DeptView.INPUT_CODE);
            string szCondition = string.Format("A.{0}={1} AND A.{2}=B.{3}", ServerData.UserDeptView.USER_ID, base.DataAccess.GetSqlParamName("USER_ID")
                , ServerData.UserDeptView.DEPT_CODE, ServerData.DeptView.DEPT_CODE);
            string szTable = string.Format("{0} A,{1} B", ServerData.DataView.USER_DEPT, ServerData.DataView.DEPT);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            DbParameter[] pmi = new DbParameter[1];
            pmi[0] = new DbParameter("USER_ID", szUserID);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text, ref pmi);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstDeptInfos == null)
                    lstDeptInfos = new List<DeptInfo>();
                do
                {
                    DeptInfo deptInfo = new DeptInfo();
                    if (!dataReader.IsDBNull(0)) deptInfo.DeptCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) deptInfo.DeptName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) deptInfo.IsClinicDept = (dataReader.GetValue(2).ToString() == "1");
                    if (!dataReader.IsDBNull(3)) deptInfo.IsWardDept = (dataReader.GetValue(3).ToString() == "1");
                    if (!dataReader.IsDBNull(4)) deptInfo.IsOutpDept = (dataReader.GetValue(4).ToString() == "1");
                    if (!dataReader.IsDBNull(5)) deptInfo.IsNurseDept = (dataReader.GetValue(5).ToString() == "1");
                    if (!dataReader.IsDBNull(6)) deptInfo.InputCode = dataReader.GetString(6);
                    lstDeptInfos.Add(deptInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据科室编号获取用户信息列表
        /// </summary>
        /// <param name="szDeptCode">科室编号</param>
        /// <param name="lstUserInfos">用户信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDeptUserList(string szDeptCode, ref List<UserInfo> lstUserInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            //先获取用户信息
            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.UserView.USER_ID, ServerData.UserView.USER_NAME
                , ServerData.UserView.DEPT_CODE, ServerData.UserView.DEPT_NAME);
            string szTable = ServerData.DataView.USER;
            string szCondition = string.Format("{0}='{1}'", ServerData.UserView.DEPT_CODE, szDeptCode);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstUserInfos == null)
                    lstUserInfos = new List<UserInfo>();
                do
                {
                    UserInfo userInfo = new UserInfo();
                    if (!dataReader.IsDBNull(0)) userInfo.ID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) userInfo.Name = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) userInfo.DeptCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) userInfo.DeptName = dataReader.GetString(3);
                    userInfo.WardCode = userInfo.DeptCode;
                    userInfo.WardName = userInfo.DeptName;
                    lstUserInfos.Add(userInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 根据用户组代码获取组包含的用户信息列表
        /// </summary>
        /// <param name="szGroupCode">组代码</param>
        /// <param name="lstUserGroups">用户信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetGroupUserList(string szGroupCode, ref List<UserGroup> lstUserGroups)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("A.{0},A.{1},B.{2},B.{3},B.{4},B.{5}"
                , ServerData.UserGroupTable.GROUP_CODE, ServerData.UserGroupTable.PRIORITY
                , ServerData.UserView.USER_ID, ServerData.UserView.USER_NAME
                , ServerData.UserView.DEPT_CODE, ServerData.UserView.DEPT_NAME);
            string szTable = string.Format("{0} A, {1} B"
                , ServerData.DataTable.USER_GROUP, ServerData.DataView.USER);
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}=B.{3}"
                , ServerData.UserGroupTable.GROUP_CODE, szGroupCode
                , ServerData.UserGroupTable.USER_ID, ServerData.UserView.USER_ID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstUserGroups == null)
                    lstUserGroups = new List<UserGroup>();
                do
                {
                    UserGroup userGroup = new UserGroup();
                    if (!dataReader.IsDBNull(0)) userGroup.GroupCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) userGroup.Priority = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) userGroup.UserInfo.ID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) userGroup.UserInfo.Name = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) userGroup.UserInfo.DeptCode = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) userGroup.UserInfo.DeptName = dataReader.GetString(5);
                    userGroup.UserInfo.WardCode = userGroup.UserInfo.DeptCode;
                    userGroup.UserInfo.WardName = userGroup.UserInfo.DeptName;
                    lstUserGroups.Add(userGroup);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 保存新的一条用户组数据到用户组信息表
        /// </summary>
        /// <param name="userGroup">用户组信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveUserGroup(UserGroup userGroup)
        {
            if (userGroup == null || userGroup.UserInfo == null)
                return ServerData.ExecuteResult.RES_NO_FOUND;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2}"
                , ServerData.UserGroupTable.GROUP_CODE, ServerData.UserGroupTable.USER_ID
                , ServerData.UserGroupTable.PRIORITY);
            string szValue = string.Format("'{0}','{1}',{2}"
                , userGroup.GroupCode, userGroup.UserInfo.ID, userGroup.Priority.ToString());
            string szTable = ServerData.DataTable.USER_GROUP;
            string szSQL = string.Format(ServerData.SQL.INSERT, szTable, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return nCount > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// 更新指定的一条用户组数据到用户组信息表
        /// </summary>
        /// <param name="szGroupCode">组代码</param>
        /// <param name="szUserID">用户ID</param>
        /// <param name="userGroup">用户组信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateUserGroup(string szGroupCode, string szUserID, UserGroup userGroup)
        {
            if (userGroup == null || userGroup.UserInfo == null)
                return ServerData.ExecuteResult.RES_NO_FOUND;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}'"
                , ServerData.UserGroupTable.GROUP_CODE, userGroup.GroupCode
                , ServerData.UserGroupTable.USER_ID, userGroup.UserInfo.ID
                , ServerData.UserGroupTable.PRIORITY, userGroup.Priority);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.UserGroupTable.GROUP_CODE, szGroupCode
                , ServerData.UserGroupTable.USER_ID, szUserID);
            string szTable = ServerData.DataTable.USER_GROUP;
            string szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return nCount > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// 从用户组信息表中删除指定的一条用户组数据
        /// </summary>
        /// <param name="szGroupCode">组代码</param>
        /// <param name="szUserID">用户ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteUserGroup(string szGroupCode, string szUserID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szTable = ServerData.DataTable.USER_GROUP;
            string szCondition = string.Format("{0}='{1}' and {2}='{3}'"
                , ServerData.UserGroupTable.GROUP_CODE, szGroupCode, ServerData.UserGroupTable.USER_ID, szUserID);
            string szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return nCount > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }
    }
}
