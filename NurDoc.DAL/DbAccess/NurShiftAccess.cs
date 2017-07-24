// ***********************************************************
// 数据库访问层护士交班操作有关的数据访问接口.
// Creator:YangMingkun  Date:2012-3-20
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
    public class NurShiftAccess : DBAccessBase
    {
        #region "交班字典数据管理"
        /// <summary>
        /// 获取指定病区的交班班次列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstDeptInfos">科室信息列表</param>
        /// <param name="lstShiftRankInfos">交班班次列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftRankInfos(string szWardCode, List<DeptInfo> lstDeptInfos, ref List<ShiftRankInfo> lstShiftRankInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftRankInfos == null)
                lstShiftRankInfos = new List<ShiftRankInfo>();
            lstShiftRankInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.ShiftRankDictTable.RANK_CODE, ServerData.ShiftRankDictTable.RANK_NO
                , ServerData.ShiftRankDictTable.RANK_NAME
                , ServerData.ShiftRankDictTable.START_POINT, ServerData.ShiftRankDictTable.END_POINT
                , ServerData.ShiftRankDictTable.WARD_CODE, ServerData.ShiftRankDictTable.WARD_NAME);
            string szTable = ServerData.DataTable.NUR_SHIFT_RANK_DICT;
            string szCondition = string.Empty;
            if (lstDeptInfos == null)
                lstDeptInfos = new List<DeptInfo>();
            if (lstDeptInfos != null && lstDeptInfos.Count > 0)
            {
                for (int index = 0; index <= lstDeptInfos.Count - 1; index++)
                {
                    DeptInfo DeptInfo = lstDeptInfos[index];
                    szCondition = string.Format(" {0} OR {1}='{2}' "
                        , szCondition
                        , ServerData.ShiftRankDictTable.WARD_CODE, DeptInfo.DeptCode);
                }
            }
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
            {
                szCondition = string.Format("{0}='{1}' {2}"
                    , ServerData.ShiftRankDictTable.WARD_CODE, szWardCode
                    , szCondition);
            }
            string szOrder = string.Format("{0},{1}"
                , ServerData.ShiftRankDictTable.WARD_NAME, ServerData.ShiftRankDictTable.RANK_NO);
            string szSQL = string.Empty;
            if (GlobalMethods.Misc.IsEmptyString(szCondition))
                szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField, szTable, szOrder);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);
            
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return this.GetWardShiftRankInfos(ref lstShiftRankInfos);
                do
                {
                    ShiftRankInfo shiftRankInfo = new ShiftRankInfo();
                    if (!dataReader.IsDBNull(0)) shiftRankInfo.RankCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftRankInfo.RankNo = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) shiftRankInfo.RankName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftRankInfo.StartPoint = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) shiftRankInfo.EndPoint = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) shiftRankInfo.WardCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) shiftRankInfo.WardName = dataReader.GetString(6);
                    lstShiftRankInfos.Add(shiftRankInfo);
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
        /// 获取全院的交班班次列表
        /// </summary>
        /// <param name="lstShiftRankInfos">交班班次列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWardShiftRankInfos(ref List<ShiftRankInfo> lstShiftRankInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftRankInfos == null)
                lstShiftRankInfos = new List<ShiftRankInfo>();
            lstShiftRankInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.ShiftRankDictTable.RANK_CODE, ServerData.ShiftRankDictTable.RANK_NO
                , ServerData.ShiftRankDictTable.RANK_NAME
                , ServerData.ShiftRankDictTable.START_POINT, ServerData.ShiftRankDictTable.END_POINT
                , ServerData.ShiftRankDictTable.WARD_CODE, ServerData.ShiftRankDictTable.WARD_NAME);
            string szTable = ServerData.DataTable.NUR_SHIFT_RANK_DICT;
            string szCondition =  string.Format("{0}='ALL'", ServerData.ShiftRankDictTable.WARD_CODE);
            string szOrder = string.Format("{0},{1}"
                , ServerData.ShiftRankDictTable.WARD_NAME, ServerData.ShiftRankDictTable.RANK_NO);
            string szSQL = string.Empty;
            if (GlobalMethods.Misc.IsEmptyString(szCondition))
                szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField, szTable, szOrder);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftRankInfo shiftRankInfo = new ShiftRankInfo();
                    if (!dataReader.IsDBNull(0)) shiftRankInfo.RankCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftRankInfo.RankNo = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) shiftRankInfo.RankName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftRankInfo.StartPoint = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) shiftRankInfo.EndPoint = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) shiftRankInfo.WardCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) shiftRankInfo.WardName = dataReader.GetString(6);
                    lstShiftRankInfos.Add(shiftRankInfo);
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
        /// 保存指定的交班班次信息
        /// </summary>
        /// <param name="shiftRankInfo">交班班次信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftRankInfo(ShiftRankInfo shiftRankInfo)
        {
            if (shiftRankInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.ShiftRankDictTable.RANK_CODE, ServerData.ShiftRankDictTable.RANK_NO
                , ServerData.ShiftRankDictTable.RANK_NAME
                , ServerData.ShiftRankDictTable.START_POINT, ServerData.ShiftRankDictTable.END_POINT
                , ServerData.ShiftRankDictTable.WARD_CODE, ServerData.ShiftRankDictTable.WARD_NAME);
            string szValue = string.Format("'{0}',{1},'{2}','{3}','{4}','{5}','{6}'"
                , shiftRankInfo.RankCode, shiftRankInfo.RankNo, shiftRankInfo.RankName
                , shiftRankInfo.StartPoint, shiftRankInfo.EndPoint
                , shiftRankInfo.WardCode, shiftRankInfo.WardName);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_RANK_DICT, szField, szValue);

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
        /// 更新指定的交班班次信息
        /// </summary>
        /// <param name="szRankCode">交班班次代码</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <param name="shiftRankInfo">交班新班次信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftRankInfo(string szRankCode, string szWardCode, ShiftRankInfo shiftRankInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szRankCode) || shiftRankInfo == null)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateShiftRankInfo"
                    , new string[] { "szRankCode", "shiftRankInfo" }, new object[] { szRankCode, shiftRankInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3},{4}='{5}',{6}='{7}',{8}='{9}',{10}='{11}',{12}='{13}'"
                , ServerData.ShiftRankDictTable.RANK_CODE, shiftRankInfo.RankCode
                , ServerData.ShiftRankDictTable.RANK_NO, shiftRankInfo.RankNo
                , ServerData.ShiftRankDictTable.RANK_NAME, shiftRankInfo.RankName
                , ServerData.ShiftRankDictTable.START_POINT, shiftRankInfo.StartPoint
                , ServerData.ShiftRankDictTable.END_POINT, shiftRankInfo.EndPoint
                , ServerData.ShiftRankDictTable.WARD_CODE, shiftRankInfo.WardCode
                , ServerData.ShiftRankDictTable.WARD_NAME, shiftRankInfo.WardName);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ShiftRankDictTable.RANK_CODE, szRankCode, ServerData.ShiftRankDictTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_RANK_DICT, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateShiftRankInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的交班班次信息
        /// </summary>
        /// <param name="szRankCode">交班班次代码</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftRankInfo(string szRankCode, string szWardCode)
        {
            if (GlobalMethods.Misc.IsEmptyString(szRankCode))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ShiftRankDictTable.RANK_CODE, szRankCode, ServerData.ShiftRankDictTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_SHIFT_RANK_DICT, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }
        #endregion

        #region "交班动态配置管理"
        /// <summary>
        /// 获取指定交班动态配置信息
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstShiftConfigInfos">交班动态配置列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftConfigInfos(string[] szWardCodes, ref List<ShiftConfigInfo> lstShiftConfigInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftConfigInfos == null)
                lstShiftConfigInfos = new List<ShiftConfigInfo>();
            lstShiftConfigInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}"
                , ServerData.ShiftConfigDictTable.ITEM_CODE, ServerData.ShiftConfigDictTable.ITEM_NO
                , ServerData.ShiftConfigDictTable.ITEM_NAME, ServerData.ShiftConfigDictTable.ITEM_WIDTH
                , ServerData.ShiftConfigDictTable.IS_VISIBLE, ServerData.ShiftConfigDictTable.IS_MIDDLE
                , ServerData.ShiftConfigDictTable.IS_IMPORTANT, ServerData.ShiftConfigDictTable.WARD_CODE
                , ServerData.ShiftConfigDictTable.WARD_NAME);
            string szTable = ServerData.DataTable.NUR_SHIFT_CONFIG_DICT;
            string szCondition = string.Empty;
            if (szWardCodes != null)
            {
                for (int i = 0; i < szWardCodes.Length; i++)
                {
                    szCondition += "'" + szWardCodes[i] + "'";
                    if (i != szWardCodes.Length - 1)
                        szCondition += ",";
                }
                szCondition = string.Format("{0} in ({1})", ServerData.ShiftConfigDictTable.WARD_CODE, szCondition);
            }
            string szOrder = string.Format("{0},{1}"
                , ServerData.ShiftConfigDictTable.WARD_NAME, ServerData.ShiftConfigDictTable.ITEM_NO);
            string szSQL = string.Empty;
            if (GlobalMethods.Misc.IsEmptyString(szCondition))
                szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField, szTable, szOrder);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftConfigInfo shiftConfigInfo = new ShiftConfigInfo();
                    if (!dataReader.IsDBNull(0)) shiftConfigInfo.ItemCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftConfigInfo.ItemNo = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) shiftConfigInfo.ItemName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftConfigInfo.ItemWidth = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4)) shiftConfigInfo.Visible = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5)) shiftConfigInfo.Middle = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6)) shiftConfigInfo.Important = dataReader.GetValue(6).ToString().Equals("1");
                    if (!dataReader.IsDBNull(7)) shiftConfigInfo.WardCode = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) shiftConfigInfo.WardName = dataReader.GetString(8);
                    lstShiftConfigInfos.Add(shiftConfigInfo);
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
        /// 保存指定的交班动态配置信息
        /// </summary>
        /// <param name="shiftConfigInfo">交班动态配置信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftConfigInfo(ShiftConfigInfo shiftConfigInfo)
        {
            if (shiftConfigInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}"
                , ServerData.ShiftConfigDictTable.ITEM_CODE, ServerData.ShiftConfigDictTable.ITEM_NO
                , ServerData.ShiftConfigDictTable.ITEM_NAME, ServerData.ShiftConfigDictTable.ITEM_WIDTH
                , ServerData.ShiftConfigDictTable.IS_VISIBLE, ServerData.ShiftConfigDictTable.IS_MIDDLE
                , ServerData.ShiftConfigDictTable.IS_IMPORTANT, ServerData.ShiftConfigDictTable.WARD_CODE
                , ServerData.ShiftConfigDictTable.WARD_NAME);
            string szValue = string.Format("'{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}'"
                , shiftConfigInfo.ItemCode, shiftConfigInfo.ItemNo, shiftConfigInfo.ItemName, shiftConfigInfo.ItemWidth, shiftConfigInfo.Visible ? 1 : 0
                , shiftConfigInfo.Middle ? 1 : 0, shiftConfigInfo.Important ? 1 : 0, shiftConfigInfo.WardCode, shiftConfigInfo.WardName);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_CONFIG_DICT, szField, szValue);

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
        /// 更新指定的交班动态配置信息
        /// </summary>
        /// <param name="szItemCode">交班动态配置代码</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <param name="shiftConfigInfo">交班新动态配置信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftConfigInfo(string szItemCode, string szWardCode, ShiftConfigInfo shiftConfigInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szItemCode) || shiftConfigInfo == null)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateShiftConfigInfo"
                    , new string[] { "szItemCode", "shiftConfigInfo" }, new object[] { szItemCode, shiftConfigInfo }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}={3},{4}='{5}',{6}='{7}',{8}='{9}',{10}='{11}',{12}='{13}',{14}='{15}',{16}='{17}'"
                , ServerData.ShiftConfigDictTable.ITEM_CODE, shiftConfigInfo.ItemCode
                , ServerData.ShiftConfigDictTable.ITEM_NO, shiftConfigInfo.ItemNo
                , ServerData.ShiftConfigDictTable.ITEM_NAME, shiftConfigInfo.ItemName
                , ServerData.ShiftConfigDictTable.ITEM_WIDTH, shiftConfigInfo.ItemWidth
                , ServerData.ShiftConfigDictTable.IS_VISIBLE, shiftConfigInfo.Visible ? 1 : 0
                , ServerData.ShiftConfigDictTable.IS_MIDDLE, shiftConfigInfo.Middle ? 1 : 0
                , ServerData.ShiftConfigDictTable.IS_IMPORTANT, shiftConfigInfo.Important ? 1 : 0
                , ServerData.ShiftConfigDictTable.WARD_CODE, shiftConfigInfo.WardCode
                , ServerData.ShiftConfigDictTable.WARD_NAME, shiftConfigInfo.WardName);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ShiftConfigDictTable.ITEM_CODE, szItemCode, ServerData.ShiftConfigDictTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_CONFIG_DICT, szField, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateShiftRankInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的交班动态配置信息
        /// </summary>
        /// <param name="szItemCode">交班动态配置代码</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftConfigInfo(string szItemCode, string szWardCode)
        {
            if (GlobalMethods.Misc.IsEmptyString(szItemCode))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ShiftConfigDictTable.ITEM_CODE, szItemCode, ServerData.ShiftConfigDictTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_SHIFT_CONFIG_DICT, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }
        #endregion

        /// <summary>
        /// 保存指定的交班病区状况信息
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="lstShiftWardStatus">病区状况信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftWardStatus(string szShiftRecordID, List<ShiftWardStatus> lstShiftWardStatus)
        {
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            if (lstShiftWardStatus == null)
                lstShiftWardStatus = new List<ShiftWardStatus>();

            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存指定的交班病区状况信息
        /// </summary>
        /// <param name="shiftWardStatus">病区状况信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftWardStatus(ShiftWardStatus shiftWardStatus)
        {
            if (shiftWardStatus == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0},{1},{2},{3}"
                , ServerData.ShiftWardStatusTable.SHIFT_RECORD_ID, ServerData.ShiftWardStatusTable.SHIFT_ITEM_NAME
                , ServerData.ShiftWardStatusTable.SHIFT_ITEM_CODE, ServerData.ShiftWardStatusTable.SHIFT_ITEM_DESC);
            string szValues = string.Format("'{0}','{1}','{2}','{3}'"
                , shiftWardStatus.ShiftRecordID, shiftWardStatus.ShiftItemName
                , shiftWardStatus.ShiftItemCode, shiftWardStatus.ShiftItemDesc);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_WARD_STATUS, szFields, szValues);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                if (base.DataAccess.IsConstraintConflictExpception(ex))
                {
                    if (this.UpdateShiftWardStatus(shiftWardStatus) == ServerData.ExecuteResult.OK)
                        return ServerData.ExecuteResult.OK;
                }
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新交班病区状况信息
        /// </summary>
        /// <param name="shiftWardStatus">病区状况信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftWardStatus(ShiftWardStatus shiftWardStatus)
        {
            if (shiftWardStatus == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}='{1}',{2}='{3}',{4}='{5}'"
                , ServerData.ShiftWardStatusTable.SHIFT_ITEM_NAME, shiftWardStatus.ShiftItemName
                , ServerData.ShiftWardStatusTable.SHIFT_ITEM_CODE, shiftWardStatus.ShiftItemCode
                , ServerData.ShiftWardStatusTable.SHIFT_ITEM_DESC, shiftWardStatus.ShiftItemDesc);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ShiftWardStatusTable.SHIFT_RECORD_ID, shiftWardStatus.ShiftRecordID
                , ServerData.ShiftWardStatusTable.SHIFT_ITEM_NAME, shiftWardStatus.ShiftItemName);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_WARD_STATUS, szFields, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateShiftWardStatus", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取指定的交班记录对应的病区状况信息列表
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="lstShiftWardStatus">病区状况信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftWardStatusList(string szShiftRecordID, ref List<ShiftWardStatus> lstShiftWardStatus)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftWardStatus == null)
                lstShiftWardStatus = new List<ShiftWardStatus>();
            lstShiftWardStatus.Clear();

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.ShiftWardStatusTable.SHIFT_RECORD_ID, ServerData.ShiftWardStatusTable.SHIFT_ITEM_NAME
                , ServerData.ShiftWardStatusTable.SHIFT_ITEM_CODE, ServerData.ShiftWardStatusTable.SHIFT_ITEM_DESC);
            string szTable = ServerData.DataTable.NUR_SHIFT_WARD_STATUS;
            string szCondition = string.Format("{0}='{1}'", ServerData.ShiftRecordTable.SHIFT_RECORD_ID, szShiftRecordID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftWardStatus shiftWardStatus = new ShiftWardStatus();
                    if (!dataReader.IsDBNull(0)) shiftWardStatus.ShiftRecordID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftWardStatus.ShiftItemName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) shiftWardStatus.ShiftItemCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftWardStatus.ShiftItemDesc = dataReader.GetString(3);
                    lstShiftWardStatus.Add(shiftWardStatus);
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
        /// 获取指定的病区和交班日期对应的病区状况信息列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="lstShiftWardStatus">病区状况信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftWardStatusList(string szWardCode, DateTime dtShiftDate, ref List<ShiftWardStatus> lstShiftWardStatus)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftWardStatus == null)
                lstShiftWardStatus = new List<ShiftWardStatus>();
            lstShiftWardStatus.Clear();

            string szField = string.Format("B.{0},B.{1},B.{2},B.{3}"
                , ServerData.ShiftWardStatusTable.SHIFT_RECORD_ID, ServerData.ShiftWardStatusTable.SHIFT_ITEM_NAME
                , ServerData.ShiftWardStatusTable.SHIFT_ITEM_CODE, ServerData.ShiftWardStatusTable.SHIFT_ITEM_DESC);
            string szTable = string.Format("{0} A,{1} B"
                , ServerData.DataTable.NUR_SHIFT_INDEX, ServerData.DataTable.NUR_SHIFT_WARD_STATUS);
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}={3} AND A.{4}=B.{5}"
                , ServerData.ShiftRecordTable.WARD_CODE, szWardCode
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(dtShiftDate)
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftPatientTable.SHIFT_RECORD_ID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftWardStatus shiftWardStatus = new ShiftWardStatus();
                    if (!dataReader.IsDBNull(0)) shiftWardStatus.ShiftRecordID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftWardStatus.ShiftItemName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) shiftWardStatus.ShiftItemCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftWardStatus.ShiftItemDesc = dataReader.GetString(3);
                    lstShiftWardStatus.Add(shiftWardStatus);
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
        /// 保存指定的交班病人信息
        /// </summary>
        /// <param name="shiftPatient">交班病人信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftPatient(ShiftPatient shiftPatient)
        {
            if (shiftPatient == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29}"
                , ServerData.ShiftPatientTable.SHIFT_RECORD_ID, ServerData.ShiftPatientTable.SHIFT_ITEM_NAME
                , ServerData.ShiftPatientTable.PATIENT_ID, ServerData.ShiftPatientTable.PATIENT_NO
                , ServerData.ShiftPatientTable.VISIT_ID, ServerData.ShiftPatientTable.SUB_ID
                , ServerData.ShiftPatientTable.PATIENT_NAME, ServerData.ShiftPatientTable.PATIENT_AGE
                , ServerData.ShiftPatientTable.WARD_CODE, ServerData.ShiftPatientTable.WARD_NAME
                , ServerData.ShiftPatientTable.BED_CODE, ServerData.ShiftPatientTable.DIAGNOSIS
                , ServerData.ShiftPatientTable.TEMPERATURE_VALUE, ServerData.ShiftPatientTable.TEMPERATURE_TYPE
                , ServerData.ShiftPatientTable.PULSE_VALUE, ServerData.ShiftPatientTable.BREATH_VALUE
                , ServerData.ShiftPatientTable.VITAL_TIME, ServerData.ShiftPatientTable.SPECIAL_ITEM1
                , ServerData.ShiftPatientTable.SPECIAL_ITEM2, ServerData.ShiftPatientTable.SPECIAL_ITEM3
                , ServerData.ShiftPatientTable.SPECIAL_ITEM4, ServerData.ShiftPatientTable.SPECIAL_ITEM5
                , ServerData.ShiftPatientTable.SHIFT_CONTENT, ServerData.ShiftPatientTable.SHOW_VALUE
                , ServerData.ShiftPatientTable.SHIFT_ITEM_ALIAS, ServerData.ShiftPatientTable.BLOODPRESSURE_VALUE
                , ServerData.ShiftPatientTable.DIET, ServerData.ShiftPatientTable.ADVERSEREACTION
                , ServerData.ShiftPatientTable.REQUESTDOCTOR_NAME, ServerData.ShiftPatientTable.PARENTDOCTOR_NAME);
            string szValues = string.Format("'{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},{14},{15},{16},'{17}','{18}','{19}','{20}','{21}',{22},'{23}','{24}','{25}','{26}','{27}','{28}','{29}'"
                , shiftPatient.ShiftRecordID, shiftPatient.ShiftItemName, shiftPatient.PatientID
                , shiftPatient.PatientNo, shiftPatient.VisitID, shiftPatient.SubID
                , shiftPatient.PatientName, shiftPatient.PatientAge, shiftPatient.WardCode
                , shiftPatient.WardName, shiftPatient.BedCode, shiftPatient.Diagnosis
                , shiftPatient.TemperatureValue, shiftPatient.TemperatureType, shiftPatient.PulseValue
                , shiftPatient.BreathValue, base.DataAccess.GetSqlTimeFormat(shiftPatient.VitalTime)
                , shiftPatient.SpecialItem1, shiftPatient.SpecialItem2, shiftPatient.SpecialItem3
                , shiftPatient.SpecialItem4, shiftPatient.SpecialItem5, base.DataAccess.GetSqlParamName("SHIFT_CONTENT")
                , shiftPatient.ShowValue ? "1" : "0", shiftPatient.ShiftItemAlias, shiftPatient.BloodPressure
                , shiftPatient.Diet, shiftPatient.AdverseReaction, shiftPatient.RequestDoctor, shiftPatient.ParentDoctor);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_PATIENT, szFields, szValues);
            
            DbParameter[] pmi = new DbParameter[1];
            pmi[0] = new DbParameter("SHIFT_CONTENT", shiftPatient.ShiftContent);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                if (base.DataAccess.IsConstraintConflictExpception(ex))
                {
                    if (this.UpdateShiftPatient(shiftPatient) == ServerData.ExecuteResult.OK)
                        return ServerData.ExecuteResult.OK;
                }
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新指定的交班病人信息
        /// </summary>
        /// <param name="shiftPatient">交班病人信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftPatient(ShiftPatient shiftPatient)
        {
            if (shiftPatient == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}='{1}',{2}={3},{4}='{5}',{6}='{7}',{8}='{9}',{10}='{11}',{12}='{13}',{14}='{15}',{16}={17},"
                + "{18}={19},{20}={21},{22}={23},{24}={25},{26}='{27}',{28}='{29}',{30}='{31}',{32}='{33}',{34}='{35}',{36}={37},{38}='{39}'"
                + ",{40}='{41}',{42}='{43}',{44}='{45}',{46}='{47}',{48}='{49}',{50}='{51}'"
                , ServerData.ShiftPatientTable.SHIFT_ITEM_NAME, shiftPatient.ShiftItemName
                , ServerData.ShiftPatientTable.PATIENT_NO, shiftPatient.PatientNo
                , ServerData.ShiftPatientTable.PATIENT_NAME, shiftPatient.PatientName
                , ServerData.ShiftPatientTable.PATIENT_AGE, shiftPatient.PatientAge
                , ServerData.ShiftPatientTable.WARD_CODE, shiftPatient.WardCode
                , ServerData.ShiftPatientTable.WARD_NAME, shiftPatient.WardName
                , ServerData.ShiftPatientTable.BED_CODE, shiftPatient.BedCode
                , ServerData.ShiftPatientTable.DIAGNOSIS, shiftPatient.Diagnosis
                , ServerData.ShiftPatientTable.TEMPERATURE_VALUE, shiftPatient.TemperatureValue
                , ServerData.ShiftPatientTable.TEMPERATURE_TYPE, shiftPatient.TemperatureType
                , ServerData.ShiftPatientTable.PULSE_VALUE, shiftPatient.PulseValue
                , ServerData.ShiftPatientTable.BREATH_VALUE, shiftPatient.BreathValue
                , ServerData.ShiftPatientTable.VITAL_TIME, base.DataAccess.GetSqlTimeFormat(shiftPatient.VitalTime)
                , ServerData.ShiftPatientTable.SPECIAL_ITEM1, shiftPatient.SpecialItem1
                , ServerData.ShiftPatientTable.SPECIAL_ITEM2, shiftPatient.SpecialItem2
                , ServerData.ShiftPatientTable.SPECIAL_ITEM3, shiftPatient.SpecialItem3
                , ServerData.ShiftPatientTable.SPECIAL_ITEM4, shiftPatient.SpecialItem4
                , ServerData.ShiftPatientTable.SPECIAL_ITEM5, shiftPatient.SpecialItem5
                , ServerData.ShiftPatientTable.SHIFT_CONTENT, base.DataAccess.GetSqlParamName("SHIFT_CONTENT")
                , ServerData.ShiftPatientTable.SHOW_VALUE, shiftPatient.ShowValue ? "1" : "0"
                , ServerData.ShiftPatientTable.SHIFT_ITEM_ALIAS, shiftPatient.ShiftItemAlias
                , ServerData.ShiftPatientTable.BLOODPRESSURE_VALUE, shiftPatient.BloodPressure
                , ServerData.ShiftPatientTable.DIET, shiftPatient.Diet
                , ServerData.ShiftPatientTable.ADVERSEREACTION, shiftPatient.AdverseReaction
                , ServerData.ShiftPatientTable.REQUESTDOCTOR_NAME, shiftPatient.RequestDoctor
                , ServerData.ShiftPatientTable.PARENTDOCTOR_NAME, shiftPatient.ParentDoctor);
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='{7}'"
                , ServerData.ShiftPatientTable.SHIFT_RECORD_ID, shiftPatient.ShiftRecordID
                , ServerData.ShiftPatientTable.PATIENT_ID, shiftPatient.PatientID
                , ServerData.ShiftPatientTable.VISIT_ID, shiftPatient.VisitID
                , ServerData.ShiftPatientTable.SUB_ID, shiftPatient.SubID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_PATIENT, szFields, szCondition);
            
            DbParameter[] pmi = new DbParameter[1];
            pmi[0] = new DbParameter("SHIFT_CONTENT", shiftPatient.ShiftContent);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateShiftPatient", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的交班病人信息
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="szPatientID">交班病人ID</param>
        /// <param name="szVisitID">交班病人就诊ID</param>
        /// <param name="szSubID">交班病人子ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftPatient(string szShiftRecordID, string szPatientID, string szVisitID, string szSubID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='{7}'"
                , ServerData.ShiftPatientTable.SHIFT_RECORD_ID, szShiftRecordID
                , ServerData.ShiftPatientTable.PATIENT_ID, szPatientID
                , ServerData.ShiftPatientTable.VISIT_ID, szVisitID
                , ServerData.ShiftPatientTable.SUB_ID, szSubID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_SHIFT_PATIENT, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }

        /// <summary>
        /// 获取指定的交班记录对应的交班病人列表
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="lstShiftPatients">交班病人列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftPatientList(string szShiftRecordID, ref List<ShiftPatient> lstShiftPatients)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftPatients == null)
                lstShiftPatients = new List<ShiftPatient>();
            lstShiftPatients.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                + ",{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29}"
                , ServerData.ShiftPatientTable.SHIFT_RECORD_ID, ServerData.ShiftPatientTable.SHIFT_ITEM_NAME
                , ServerData.ShiftPatientTable.PATIENT_ID, ServerData.ShiftPatientTable.PATIENT_NO
                , ServerData.ShiftPatientTable.VISIT_ID, ServerData.ShiftPatientTable.SUB_ID
                , ServerData.ShiftPatientTable.PATIENT_NAME, ServerData.ShiftPatientTable.PATIENT_AGE
                , ServerData.ShiftPatientTable.WARD_CODE, ServerData.ShiftPatientTable.WARD_NAME
                , ServerData.ShiftPatientTable.BED_CODE, ServerData.ShiftPatientTable.DIAGNOSIS
                , ServerData.ShiftPatientTable.TEMPERATURE_VALUE, ServerData.ShiftPatientTable.TEMPERATURE_TYPE
                , ServerData.ShiftPatientTable.PULSE_VALUE, ServerData.ShiftPatientTable.BREATH_VALUE
                , ServerData.ShiftPatientTable.VITAL_TIME, ServerData.ShiftPatientTable.SPECIAL_ITEM1
                , ServerData.ShiftPatientTable.SPECIAL_ITEM2, ServerData.ShiftPatientTable.SPECIAL_ITEM3
                , ServerData.ShiftPatientTable.SPECIAL_ITEM4, ServerData.ShiftPatientTable.SPECIAL_ITEM5
                , ServerData.ShiftPatientTable.SHIFT_CONTENT, ServerData.ShiftPatientTable.SHOW_VALUE
                , ServerData.ShiftPatientTable.SHIFT_ITEM_ALIAS, ServerData.ShiftPatientTable.BLOODPRESSURE_VALUE
                , ServerData.ShiftPatientTable.DIET, ServerData.ShiftPatientTable.ADVERSEREACTION
                , ServerData.ShiftPatientTable.REQUESTDOCTOR_NAME, ServerData.ShiftPatientTable.PARENTDOCTOR_NAME);
            string szTable = ServerData.DataTable.NUR_SHIFT_PATIENT;
            string szCondition = string.Format("{0}='{1}'"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, szShiftRecordID);
            string szOrder = ServerData.ShiftPatientTable.SHIFT_ITEM_ALIAS;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);
            
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftPatient shiftPatient = new ShiftPatient();
                    if (!dataReader.IsDBNull(0)) shiftPatient.ShiftRecordID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftPatient.ShiftItemName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) shiftPatient.PatientID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftPatient.PatientNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4)) shiftPatient.VisitID = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) shiftPatient.SubID = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) shiftPatient.PatientName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) shiftPatient.PatientAge = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) shiftPatient.WardCode = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) shiftPatient.WardName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) shiftPatient.BedCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) shiftPatient.Diagnosis = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) shiftPatient.TemperatureValue = float.Parse(dataReader.GetValue(12).ToString());
                    if (!dataReader.IsDBNull(13)) shiftPatient.TemperatureType = int.Parse(dataReader.GetValue(13).ToString());
                    if (!dataReader.IsDBNull(14)) shiftPatient.PulseValue = int.Parse(dataReader.GetValue(14).ToString());
                    if (!dataReader.IsDBNull(15)) shiftPatient.BreathValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16)) shiftPatient.VitalTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17)) shiftPatient.SpecialItem1 = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) shiftPatient.SpecialItem2 = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) shiftPatient.SpecialItem3 = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20)) shiftPatient.SpecialItem4 = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21)) shiftPatient.SpecialItem5 = dataReader.GetString(21);
                    if (!dataReader.IsDBNull(22)) shiftPatient.ShiftContent = dataReader.GetString(22);
                    if (!dataReader.IsDBNull(23)) shiftPatient.ShowValue = dataReader.GetValue(23).ToString().Equals("1");
                    if (!dataReader.IsDBNull(24)) shiftPatient.ShiftItemAlias = dataReader.GetString(24);
                    if (!dataReader.IsDBNull(25)) shiftPatient.BloodPressure = dataReader.GetString(25);
                    if (!dataReader.IsDBNull(26)) shiftPatient.Diet = dataReader.GetString(26);
                    if (!dataReader.IsDBNull(27)) shiftPatient.AdverseReaction = dataReader.GetString(27);
                    if (!dataReader.IsDBNull(28)) shiftPatient.RequestDoctor = dataReader.GetString(28);
                    if (!dataReader.IsDBNull(29)) shiftPatient.ParentDoctor = dataReader.GetString(29);
                    lstShiftPatients.Add(shiftPatient);
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
        /// 获取指定病区指定交班日期对应的交班病人列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="lstShiftPatients">交班病人列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftPatientList(string szWardCode, DateTime dtShiftDate, ref List<ShiftPatient> lstShiftPatients)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftPatients == null)
                lstShiftPatients = new List<ShiftPatient>();
            lstShiftPatients.Clear();

            string szField = string.Format("B.{0},B.{1},B.{2},B.{3},B.{4},B.{5},B.{6},B.{7},B.{8},B.{9},B.{10},B.{11}"
                + ",B.{12},B.{13},B.{14},B.{15},B.{16},B.{17},B.{18},B.{19},B.{20},B.{21},B.{22},A.{23},B.{24},C.{25}"
                + ",B.{26},NVL(D.{27},'') {27},B.{28},B.{29},B.{30},B.{31},B.{32}"
                , ServerData.ShiftPatientTable.SHIFT_RECORD_ID, ServerData.ShiftPatientTable.SHIFT_ITEM_NAME
                , ServerData.ShiftPatientTable.PATIENT_ID, ServerData.ShiftPatientTable.PATIENT_NO
                , ServerData.ShiftPatientTable.VISIT_ID, ServerData.ShiftPatientTable.SUB_ID
                , ServerData.ShiftPatientTable.PATIENT_NAME, ServerData.ShiftPatientTable.PATIENT_AGE
                , ServerData.ShiftPatientTable.WARD_CODE, ServerData.ShiftPatientTable.WARD_NAME
                , ServerData.ShiftPatientTable.BED_CODE, ServerData.ShiftPatientTable.DIAGNOSIS
                , ServerData.ShiftPatientTable.TEMPERATURE_VALUE, ServerData.ShiftPatientTable.TEMPERATURE_TYPE
                , ServerData.ShiftPatientTable.PULSE_VALUE, ServerData.ShiftPatientTable.BREATH_VALUE
                , ServerData.ShiftPatientTable.VITAL_TIME, ServerData.ShiftPatientTable.SPECIAL_ITEM1
                , ServerData.ShiftPatientTable.SPECIAL_ITEM2, ServerData.ShiftPatientTable.SPECIAL_ITEM3
                , ServerData.ShiftPatientTable.SPECIAL_ITEM4, ServerData.ShiftPatientTable.SPECIAL_ITEM5
                , ServerData.ShiftPatientTable.SHIFT_CONTENT, ServerData.ShiftRecordTable.MODIFIER_NAME
                , ServerData.ShiftPatientTable.SHOW_VALUE, ServerData.PatVisitView.VISIT_TIME
                , ServerData.ShiftPatientTable.SHIFT_ITEM_ALIAS, ServerData.CommonDictTable.ITEM_NO
                , ServerData.ShiftPatientTable.BLOODPRESSURE_VALUE, ServerData.ShiftPatientTable.DIET
                , ServerData.ShiftPatientTable.ADVERSEREACTION, ServerData.ShiftPatientTable.REQUESTDOCTOR_NAME
                , ServerData.ShiftPatientTable.PARENTDOCTOR_NAME);
            string szTable = string.Format("{0} A,{1} B,{2} C,{3} D", ServerData.DataTable.NUR_SHIFT_INDEX
                , ServerData.DataTable.NUR_SHIFT_PATIENT, ServerData.DataView.PAT_VISIT, ServerData.DataTable.NUR_COMMON_DICT);
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}={3} AND A.{4}=B.{5} AND B.{6}=C.{7} AND B.{8}=C.{9} AND (B.{10}=+D.{11} or B.{10} is null ) AND D.{12}='SHIFT_ITEM' AND (D.{13} = '{1}' OR D.{13} = 'ALL')"
                , ServerData.ShiftRecordTable.WARD_CODE, szWardCode
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(dtShiftDate)
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftPatientTable.SHIFT_RECORD_ID
                , ServerData.ShiftPatientTable.PATIENT_ID, ServerData.PatVisitView.PATIENT_ID
                , ServerData.ShiftPatientTable.VISIT_ID, ServerData.PatVisitView.VISIT_ID
                , ServerData.ShiftPatientTable.SHIFT_ITEM_NAME, ServerData.CommonDictTable.ITEM_NAME
                , ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictTable.WARD_CODE);
            string szOrder = string.Format("{0}, {1}, {2}, {3}", ServerData.ShiftPatientTable.SHIFT_RECORD_ID, ServerData.CommonDictTable.ITEM_NO
                , ServerData.ShiftPatientTable.SHIFT_ITEM_ALIAS, ServerData.ShiftPatientTable.BED_CODE);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);
            
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftPatient shiftPatient = new ShiftPatient();
                    if (!dataReader.IsDBNull(0)) shiftPatient.ShiftRecordID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftPatient.ShiftItemName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) shiftPatient.PatientID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftPatient.PatientNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4)) shiftPatient.VisitID = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) shiftPatient.SubID = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) shiftPatient.PatientName = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) shiftPatient.PatientAge = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) shiftPatient.WardCode = dataReader.GetString(8);
                    if (!dataReader.IsDBNull(9)) shiftPatient.WardName = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) shiftPatient.BedCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) shiftPatient.Diagnosis = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) shiftPatient.TemperatureValue = float.Parse(dataReader.GetValue(12).ToString());
                    if (!dataReader.IsDBNull(13)) shiftPatient.TemperatureType = int.Parse(dataReader.GetValue(13).ToString());
                    if (!dataReader.IsDBNull(14)) shiftPatient.PulseValue = int.Parse(dataReader.GetValue(14).ToString());
                    if (!dataReader.IsDBNull(15)) shiftPatient.BreathValue = int.Parse(dataReader.GetValue(15).ToString());
                    if (!dataReader.IsDBNull(16)) shiftPatient.VitalTime = dataReader.GetDateTime(16);
                    if (!dataReader.IsDBNull(17)) shiftPatient.SpecialItem1 = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) shiftPatient.SpecialItem2 = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) shiftPatient.SpecialItem3 = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20)) shiftPatient.SpecialItem4 = dataReader.GetString(20);
                    if (!dataReader.IsDBNull(21)) shiftPatient.SpecialItem5 = dataReader.GetString(21);
                    if (!dataReader.IsDBNull(22)) shiftPatient.ShiftContent = dataReader.GetString(22);
                    if (!dataReader.IsDBNull(23)) shiftPatient.ModifierName = dataReader.GetString(23);
                    if (!dataReader.IsDBNull(24)) shiftPatient.ShowValue = dataReader.GetValue(24).ToString().Equals("1");
                    if (!dataReader.IsDBNull(25)) shiftPatient.VisitTime = dataReader.GetDateTime(25);
                    if (!dataReader.IsDBNull(26)) shiftPatient.ShiftItemAlias = dataReader.GetString(26);
                    if (!dataReader.IsDBNull(28)) shiftPatient.BloodPressure = dataReader.GetString(28);
                    if (!dataReader.IsDBNull(29)) shiftPatient.Diet = dataReader.GetString(29);
                    if (!dataReader.IsDBNull(30)) shiftPatient.AdverseReaction = dataReader.GetString(30);
                    if (!dataReader.IsDBNull(31)) shiftPatient.RequestDoctor = dataReader.GetString(31);
                    if (!dataReader.IsDBNull(32)) shiftPatient.ParentDoctor = dataReader.GetString(32);
                    lstShiftPatients.Add(shiftPatient);
                } while (dataReader.Read());
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取对应病人的就诊时间
        /// </summary>
        /// <param name="szPatientID">病人ID号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="tVisitTime">就诊时间</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetPatVisitTime(string szPatientID, string szVisitID, ref DateTime tVisitTime)
        {
            if (string.IsNullOrEmpty(szPatientID) || string.IsNullOrEmpty(szVisitID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format(" A.{0} ", ServerData.PatVisitView.VISIT_TIME);
            string szTable = string.Format("{0} A ", ServerData.DataView.PAT_VISIT);
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}={3} "
                , ServerData.PatVisitView.PATIENT_ID, szPatientID
                , ServerData.PatVisitView.VISIT_ID, szVisitID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    if (!dataReader.IsDBNull(0)) tVisitTime = dataReader.GetDateTime(0);
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
        /// 保存指定的交班主记录
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNursingShiftInfo(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftRecordTable.SHIFT_RECORD_DATE
                , ServerData.ShiftRecordTable.SHIFT_RECORD_TIME
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE, ServerData.ShiftRecordTable.WARD_CODE
                , ServerData.ShiftRecordTable.WARD_NAME, ServerData.ShiftRecordTable.CREATOR_ID
                , ServerData.ShiftRecordTable.CREATOR_NAME, ServerData.ShiftRecordTable.CREATE_TIME
                , ServerData.ShiftRecordTable.MODIFIER_ID, ServerData.ShiftRecordTable.MODIFIER_NAME
                , ServerData.ShiftRecordTable.MODIFY_TIME, ServerData.ShiftRecordTable.FIRST_SIGN_ID
                , ServerData.ShiftRecordTable.FIRST_SIGN_NAME, ServerData.ShiftRecordTable.FIRST_SIGN_TIME
                , ServerData.ShiftRecordTable.SECOND_SIGN_ID, ServerData.ShiftRecordTable.SECOND_SIGN_NAME
                , ServerData.ShiftRecordTable.SECOND_SIGN_TIME);
            string szValues = string.Format("'{0}',{1},{2},'{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}',{11},'{12}','{13}',{14},'{15}','{16}',{17}"
                , nursingShiftInfo.ShiftRecordID, base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.ShiftRecordDate)
                , base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.ShiftRecordTime)
                , nursingShiftInfo.ShiftRankCode, nursingShiftInfo.WardCode, nursingShiftInfo.WardName
                , nursingShiftInfo.CreatorID, nursingShiftInfo.CreatorName
                , base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.CreateTime)
                , nursingShiftInfo.ModifierID, nursingShiftInfo.ModifierName
                , base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.ModifyTime)
                , nursingShiftInfo.FirstSignID, nursingShiftInfo.FirstSignName
                , base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.FirstSignTime)
                , nursingShiftInfo.SecondSignID, nursingShiftInfo.SecondSignName
                , base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.SecondSignTime));
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_INDEX, szFields, szValues);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                if (base.DataAccess.IsConstraintConflictExpception(ex))
                {
                    if (this.UpdateNursingShiftInfo(nursingShiftInfo) == ServerData.ExecuteResult.OK)
                        return ServerData.ExecuteResult.OK;
                }
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新交班主记录信息
        /// </summary>
        /// <param name="nursingShiftInfo">交班主记录信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateNursingShiftInfo(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}={1},{2}={3},{4}='{5}',{6}='{7}',{8}='{9}',{10}={11},{12}='{13}',{14}='{15}',{16}={17},{18}='{19}',{20}='{21}',{22}={23}"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.ShiftRecordDate)
                , ServerData.ShiftRecordTable.SHIFT_RECORD_TIME, base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.ShiftRecordTime)
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE, nursingShiftInfo.ShiftRankCode
                , ServerData.ShiftRecordTable.MODIFIER_ID, nursingShiftInfo.ModifierID
                , ServerData.ShiftRecordTable.MODIFIER_NAME, nursingShiftInfo.ModifierName
                , ServerData.ShiftRecordTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.ModifyTime)
                , ServerData.ShiftRecordTable.FIRST_SIGN_ID, nursingShiftInfo.FirstSignID
                , ServerData.ShiftRecordTable.FIRST_SIGN_NAME, nursingShiftInfo.FirstSignName
                , ServerData.ShiftRecordTable.FIRST_SIGN_TIME, base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.FirstSignTime)
                , ServerData.ShiftRecordTable.SECOND_SIGN_ID, nursingShiftInfo.SecondSignID
                , ServerData.ShiftRecordTable.SECOND_SIGN_NAME, nursingShiftInfo.SecondSignName
                , ServerData.ShiftRecordTable.SECOND_SIGN_TIME, base.DataAccess.GetSqlTimeFormat(nursingShiftInfo.SecondSignTime));

            string szCondition = string.Format("{0}='{1}'", ServerData.ShiftRecordTable.SHIFT_RECORD_ID, nursingShiftInfo.ShiftRecordID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_INDEX, szFields, szCondition);
           
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateNursingShiftInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取指定病区指定交班日期及班次对应的交班主记录
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="szRankCode">交班班次</param>
        /// <param name="nursingShiftInfo">交班主记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingShiftInfo(string szWardCode, DateTime dtShiftDate, string szRankCode, ref NursingShiftInfo nursingShiftInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftRecordTable.SHIFT_RECORD_DATE
                , ServerData.ShiftRecordTable.SHIFT_RECORD_TIME
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE
                , ServerData.ShiftRecordTable.WARD_CODE, ServerData.ShiftRecordTable.WARD_NAME
                , ServerData.ShiftRecordTable.CREATOR_ID, ServerData.ShiftRecordTable.CREATOR_NAME
                , ServerData.ShiftRecordTable.CREATE_TIME
                , ServerData.ShiftRecordTable.MODIFIER_ID, ServerData.ShiftRecordTable.MODIFIER_NAME
                , ServerData.ShiftRecordTable.MODIFY_TIME
                , ServerData.ShiftRecordTable.FIRST_SIGN_ID, ServerData.ShiftRecordTable.FIRST_SIGN_NAME
                , ServerData.ShiftRecordTable.FIRST_SIGN_TIME
                , ServerData.ShiftRecordTable.SECOND_SIGN_ID, ServerData.ShiftRecordTable.SECOND_SIGN_NAME
                , ServerData.ShiftRecordTable.SECOND_SIGN_TIME);
            string szTable = ServerData.DataTable.NUR_SHIFT_INDEX;
            string szCondition = string.Format("{0}={1} AND {2}='{3}' AND {4}='{5}'"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(dtShiftDate)
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE, szRankCode
                , ServerData.ShiftRecordTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (nursingShiftInfo == null)
                    nursingShiftInfo = new NursingShiftInfo();
                if (!dataReader.IsDBNull(0)) nursingShiftInfo.ShiftRecordID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1)) nursingShiftInfo.ShiftRecordDate = dataReader.GetDateTime(1);
                if (!dataReader.IsDBNull(2)) nursingShiftInfo.ShiftRecordTime = dataReader.GetDateTime(2);
                if (!dataReader.IsDBNull(3)) nursingShiftInfo.ShiftRankCode = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4)) nursingShiftInfo.WardCode = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5)) nursingShiftInfo.WardName = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6)) nursingShiftInfo.CreatorID = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7)) nursingShiftInfo.CreatorName = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8)) nursingShiftInfo.CreateTime = dataReader.GetDateTime(8);
                if (!dataReader.IsDBNull(9)) nursingShiftInfo.ModifierID = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10)) nursingShiftInfo.ModifierName = dataReader.GetString(10);
                if (!dataReader.IsDBNull(11)) nursingShiftInfo.ModifyTime = dataReader.GetDateTime(11);
                if (!dataReader.IsDBNull(12)) nursingShiftInfo.FirstSignID = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13)) nursingShiftInfo.FirstSignName = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14)) nursingShiftInfo.FirstSignTime = dataReader.GetDateTime(14);
                if (!dataReader.IsDBNull(15)) nursingShiftInfo.SecondSignID = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16)) nursingShiftInfo.SecondSignName = dataReader.GetString(16);
                if (!dataReader.IsDBNull(17)) nursingShiftInfo.SecondSignTime = dataReader.GetDateTime(17);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 获取指定病区指定交班日期内的所有班次的交班记录
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="lstNursingShiftInfos">交班记录列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingShiftInfos(string szWardCode, DateTime dtShiftDate, ref List<NursingShiftInfo> lstNursingShiftInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstNursingShiftInfos == null)
                lstNursingShiftInfos = new List<NursingShiftInfo>();
            lstNursingShiftInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftRecordTable.SHIFT_RECORD_DATE
                , ServerData.ShiftRecordTable.SHIFT_RECORD_TIME
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE
                , ServerData.ShiftRecordTable.WARD_CODE, ServerData.ShiftRecordTable.WARD_NAME
                , ServerData.ShiftRecordTable.CREATOR_ID, ServerData.ShiftRecordTable.CREATOR_NAME
                , ServerData.ShiftRecordTable.CREATE_TIME
                , ServerData.ShiftRecordTable.MODIFIER_ID, ServerData.ShiftRecordTable.MODIFIER_NAME
                , ServerData.ShiftRecordTable.MODIFY_TIME
                , ServerData.ShiftRecordTable.FIRST_SIGN_ID, ServerData.ShiftRecordTable.FIRST_SIGN_NAME
                , ServerData.ShiftRecordTable.FIRST_SIGN_TIME
                , ServerData.ShiftRecordTable.SECOND_SIGN_ID, ServerData.ShiftRecordTable.SECOND_SIGN_NAME
                , ServerData.ShiftRecordTable.SECOND_SIGN_TIME);
            string szTable = ServerData.DataTable.NUR_SHIFT_INDEX;
            string szCondition = string.Format("{0}='{1}' AND {2}={3} "
                , ServerData.ShiftRecordTable.WARD_CODE, szWardCode
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(dtShiftDate));
            string szOrder = ServerData.ShiftRecordTable.CREATE_TIME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
                    if (!dataReader.IsDBNull(0)) nursingShiftInfo.ShiftRecordID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) nursingShiftInfo.ShiftRecordDate = dataReader.GetDateTime(1);
                    if (!dataReader.IsDBNull(2)) nursingShiftInfo.ShiftRecordTime = dataReader.GetDateTime(2);
                    if (!dataReader.IsDBNull(3)) nursingShiftInfo.ShiftRankCode = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) nursingShiftInfo.WardCode = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) nursingShiftInfo.WardName = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) nursingShiftInfo.CreatorID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) nursingShiftInfo.CreatorName = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) nursingShiftInfo.CreateTime = dataReader.GetDateTime(8);
                    if (!dataReader.IsDBNull(9)) nursingShiftInfo.ModifierID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) nursingShiftInfo.ModifierName = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) nursingShiftInfo.ModifyTime = dataReader.GetDateTime(11);
                    if (!dataReader.IsDBNull(12)) nursingShiftInfo.FirstSignID = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) nursingShiftInfo.FirstSignName = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) nursingShiftInfo.FirstSignTime = dataReader.GetDateTime(14);
                    if (!dataReader.IsDBNull(15)) nursingShiftInfo.SecondSignID = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) nursingShiftInfo.SecondSignName = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) nursingShiftInfo.SecondSignTime = dataReader.GetDateTime(17);
                    lstNursingShiftInfos.Add(nursingShiftInfo);
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
        /// 获取指定病区内的所有交班项目配置别名
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstShiftItemAliasInfos">交班项目配置别名列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftItemAliasInfos(string szWardCode, ref List<ShiftItemAliasInfo> lstShiftItemAliasInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftItemAliasInfos == null)
                lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            lstShiftItemAliasInfos.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4}", ServerData.ShiftItemAliasTable.ITEM_ALIAS
                , ServerData.ShiftItemAliasTable.ITEM_ALIASCODE, ServerData.ShiftItemAliasTable.ITEM_NAME
                , ServerData.ShiftItemAliasTable.ITEM_CODE, ServerData.ShiftItemAliasTable.WARD_NAME);
            string szTable = ServerData.DataTable.NUR_SHIFT_ITEMALIAS;
            string szCondition = string.Format("{0}='{1}'", ServerData.ShiftItemAliasTable.WARD_CODE, szWardCode);
            string szOrder = ServerData.ShiftItemAliasTable.ITEM_ALIAS;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftItemAliasInfo shiftItemAliasInfo = new ShiftItemAliasInfo();
                    if (!dataReader.IsDBNull(0)) shiftItemAliasInfo.ItemAlias = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftItemAliasInfo.ItemAliasCode = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) shiftItemAliasInfo.ItemName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftItemAliasInfo.ItemCode = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) shiftItemAliasInfo.WardName = dataReader.GetString(4);
                    lstShiftItemAliasInfos.Add(shiftItemAliasInfo);
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
        /// 删除指定病区内的所有交班项目配置别名
        /// </summary>
        /// <param name="szItemAliasCode">交班项目配置别名代码</param>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftItemAlias(string szItemAliasCode, string szWardCode)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szTable = ServerData.DataTable.NUR_SHIFT_ITEMALIAS;
            string szCondition = string.Format("{0}='{1}' and {2}='{3}'"
                , ServerData.ShiftItemAliasTable.ITEM_ALIASCODE, szItemAliasCode
                , ServerData.ShiftItemAliasTable.WARD_CODE, szWardCode);
           
            string szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
            
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 更新指定病区的指定交班项目配置别名
        /// </summary>
        /// <param name="szItemAliasCode">交班项目别名代码</param>
        /// <param name="szItemAlias">交班项目别名</param>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftItemAlias(string szItemAliasCode, string szItemAlias, string szItemName, string szWardCode)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}='{1}', {2}='{3}'", ServerData.ShiftItemAliasTable.ITEM_ALIAS, szItemAlias
                ,ServerData.ShiftItemAliasTable.ITEM_NAME, szItemName); 
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.ShiftItemAliasTable.ITEM_ALIASCODE, szItemAliasCode
                , ServerData.ShiftItemAliasTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_ITEMALIAS, szFields, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存指定病区的指定交班项目配置别名
        /// </summary>
        /// <param name="shiftItemAliasInfo">交班项目配置</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftItemAlias(ShiftItemAliasInfo shiftItemAliasInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0},{1},{2},{3},{4},{5}"
                ,ServerData.ShiftItemAliasTable.ITEM_NAME, ServerData.ShiftItemAliasTable.ITEM_CODE
                , ServerData.ShiftItemAliasTable.ITEM_ALIAS, ServerData.ShiftItemAliasTable.ITEM_ALIASCODE
                , ServerData.ShiftItemAliasTable.WARD_NAME, ServerData.ShiftItemAliasTable.WARD_CODE);

            string szValues = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}'"
                , shiftItemAliasInfo.ItemName, shiftItemAliasInfo.ItemCode
                , shiftItemAliasInfo.ItemAlias, shiftItemAliasInfo.ItemAliasCode
                , shiftItemAliasInfo.WardName, shiftItemAliasInfo.WardCode);

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_ITEMALIAS, szFields, szValues);
            
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                if (base.DataAccess.IsConstraintConflictExpception(ex))
                {
                    if (this.UpdateShiftItemAlias(shiftItemAliasInfo.ItemAliasCode, shiftItemAliasInfo.ItemAlias, shiftItemAliasInfo.ItemName, shiftItemAliasInfo.WardCode) == ServerData.ExecuteResult.OK)
                        return ServerData.ExecuteResult.OK;
                }
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        #region "特殊病人病情交接班"

        /// <summary>
        /// 获取指定病区指定交班日期对应的交班主记录
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="specialShiftInfo">特殊病人病情交接班记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSpecialShiftInfo(string szWardCode, DateTime dtShiftDate, ref SpecialShiftInfo specialShiftInfo)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftRecordTable.SHIFT_RECORD_DATE
                , ServerData.ShiftRecordTable.SHIFT_RECORD_TIME
                , ServerData.ShiftRecordTable.WARD_CODE, ServerData.ShiftRecordTable.WARD_NAME
                , ServerData.ShiftRecordTable.CREATOR_ID, ServerData.ShiftRecordTable.CREATOR_NAME
                , ServerData.ShiftRecordTable.CREATE_TIME
                , ServerData.ShiftRecordTable.MODIFIER_ID, ServerData.ShiftRecordTable.MODIFIER_NAME
                , ServerData.ShiftRecordTable.MODIFY_TIME
                , ServerData.ShiftRecordTable.FIRST_SIGN_ID, ServerData.ShiftRecordTable.FIRST_SIGN_NAME
                , ServerData.ShiftRecordTable.FIRST_SIGN_TIME
                , ServerData.ShiftRecordTable.SECOND_SIGN_ID, ServerData.ShiftRecordTable.SECOND_SIGN_NAME
                , ServerData.ShiftRecordTable.SECOND_SIGN_TIME);
            string szTable = ServerData.DataTable.NUR_SHIFT_INDEX;
            string szRankCode = "-1";
            string szCondition = string.Format("{0}={1} AND {2}='{3}' AND {4}='{5}'"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(dtShiftDate)
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE, szRankCode
                , ServerData.ShiftRecordTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (specialShiftInfo == null)
                    specialShiftInfo = new SpecialShiftInfo();
                if (!dataReader.IsDBNull(0)) specialShiftInfo.ShiftRecordID = dataReader.GetString(0);
                if (!dataReader.IsDBNull(1)) specialShiftInfo.ShiftRecordDate = dataReader.GetDateTime(1);
                if (!dataReader.IsDBNull(2)) specialShiftInfo.ShiftRecordTime = dataReader.GetDateTime(2);
                if (!dataReader.IsDBNull(3)) specialShiftInfo.WardCode = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4)) specialShiftInfo.WardName = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5)) specialShiftInfo.CreatorID = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6)) specialShiftInfo.CreatorName = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7)) specialShiftInfo.CreateTime = dataReader.GetDateTime(7);
                if (!dataReader.IsDBNull(8)) specialShiftInfo.ModifierID = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9)) specialShiftInfo.ModifierName = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10)) specialShiftInfo.ModifyTime = dataReader.GetDateTime(10);
                if (!dataReader.IsDBNull(11)) specialShiftInfo.FirstSignID = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12)) specialShiftInfo.FirstSignName = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13)) specialShiftInfo.FirstSignTime = dataReader.GetDateTime(13);
                if (!dataReader.IsDBNull(14)) specialShiftInfo.SecondSignID = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15)) specialShiftInfo.SecondSignName = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16)) specialShiftInfo.SecondSignTime = dataReader.GetDateTime(16);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// 保存指定的特殊病人病情交班主记录
        /// </summary>
        /// <param name="specialShiftInfo">特殊病人病情交接班主记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveSpecialShiftInfo(SpecialShiftInfo specialShiftInfo)
        {
            if (specialShiftInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftRecordTable.SHIFT_RECORD_DATE
                , ServerData.ShiftRecordTable.SHIFT_RECORD_TIME
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE, ServerData.ShiftRecordTable.WARD_CODE
                , ServerData.ShiftRecordTable.WARD_NAME, ServerData.ShiftRecordTable.CREATOR_ID
                , ServerData.ShiftRecordTable.CREATOR_NAME, ServerData.ShiftRecordTable.CREATE_TIME
                , ServerData.ShiftRecordTable.MODIFIER_ID, ServerData.ShiftRecordTable.MODIFIER_NAME
                , ServerData.ShiftRecordTable.MODIFY_TIME, ServerData.ShiftRecordTable.FIRST_SIGN_ID
                , ServerData.ShiftRecordTable.FIRST_SIGN_NAME, ServerData.ShiftRecordTable.FIRST_SIGN_TIME
                , ServerData.ShiftRecordTable.SECOND_SIGN_ID, ServerData.ShiftRecordTable.SECOND_SIGN_NAME
                , ServerData.ShiftRecordTable.SECOND_SIGN_TIME);
            string szValues = string.Format("'{0}',{1},{2},'{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}',{11},'{12}','{13}',{14},'{15}','{16}',{17}"
                , specialShiftInfo.ShiftRecordID, base.DataAccess.GetSqlTimeFormat(specialShiftInfo.ShiftRecordDate)
                , base.DataAccess.GetSqlTimeFormat(specialShiftInfo.ShiftRecordTime)
                , "-1", specialShiftInfo.WardCode, specialShiftInfo.WardName
                , specialShiftInfo.CreatorID, specialShiftInfo.CreatorName
                , base.DataAccess.GetSqlTimeFormat(specialShiftInfo.CreateTime)
                , specialShiftInfo.ModifierID, specialShiftInfo.ModifierName
                , base.DataAccess.GetSqlTimeFormat(specialShiftInfo.ModifyTime)
                , specialShiftInfo.FirstSignID, specialShiftInfo.FirstSignName
                , base.DataAccess.GetSqlTimeFormat(specialShiftInfo.FirstSignTime)
                , specialShiftInfo.SecondSignID, specialShiftInfo.SecondSignName
                , base.DataAccess.GetSqlTimeFormat(specialShiftInfo.SecondSignTime));
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_INDEX, szFields, szValues);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                if (base.DataAccess.IsConstraintConflictExpception(ex))
                {
                    if (this.UpdateSpecialShiftInfo(specialShiftInfo) == ServerData.ExecuteResult.OK)
                        return ServerData.ExecuteResult.OK;
                }
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新交班主记录信息
        /// </summary>
        /// <param name="specialShiftInfo">交班主记录信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateSpecialShiftInfo(SpecialShiftInfo specialShiftInfo)
        {
            if (specialShiftInfo == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}={1},{2}={3},{4}='{5}',{6}='{7}',{8}='{9}',{10}={11},{12}='{13}',{14}='{15}',{16}={17},{18}='{19}',{20}='{21}',{22}={23}"
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(specialShiftInfo.ShiftRecordDate)
                , ServerData.ShiftRecordTable.SHIFT_RECORD_TIME, base.DataAccess.GetSqlTimeFormat(specialShiftInfo.ShiftRecordTime)
                , ServerData.ShiftRecordTable.SHIFT_RANK_CODE, "-1"
                , ServerData.ShiftRecordTable.MODIFIER_ID, specialShiftInfo.ModifierID
                , ServerData.ShiftRecordTable.MODIFIER_NAME, specialShiftInfo.ModifierName
                , ServerData.ShiftRecordTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(specialShiftInfo.ModifyTime)
                , ServerData.ShiftRecordTable.FIRST_SIGN_ID, specialShiftInfo.FirstSignID
                , ServerData.ShiftRecordTable.FIRST_SIGN_NAME, specialShiftInfo.FirstSignName
                , ServerData.ShiftRecordTable.FIRST_SIGN_TIME, base.DataAccess.GetSqlTimeFormat(specialShiftInfo.FirstSignTime)
                , ServerData.ShiftRecordTable.SECOND_SIGN_ID, specialShiftInfo.SecondSignID
                , ServerData.ShiftRecordTable.SECOND_SIGN_NAME, specialShiftInfo.SecondSignName
                , ServerData.ShiftRecordTable.SECOND_SIGN_TIME, base.DataAccess.GetSqlTimeFormat(specialShiftInfo.SecondSignTime));

            string szCondition = string.Format("{0}='{1}'", ServerData.ShiftRecordTable.SHIFT_RECORD_ID, specialShiftInfo.ShiftRecordID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_INDEX, szFields, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateSpecialShiftInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 保存指定的特殊病人交班信息
        /// </summary>
        /// <param name="shiftSpecialPatient">特殊病人交班信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftSpecialPatient(ShiftSpecialPatient shiftSpecialPatient)
        {
            if (shiftSpecialPatient == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}"
                , ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID, ServerData.ShiftSpecialPatientTable.BED_CODE
                , ServerData.ShiftSpecialPatientTable.PATIENT_ID, ServerData.ShiftSpecialPatientTable.PATIENT_NAME
                , ServerData.ShiftSpecialPatientTable.PATIENT_SEX, ServerData.ShiftSpecialPatientTable.PATIENT_AGE
                , ServerData.ShiftSpecialPatientTable.PATIENT_NO, ServerData.ShiftSpecialPatientTable.VISIT_ID
                , ServerData.ShiftSpecialPatientTable.LOG_DATE_TIME, ServerData.ShiftSpecialPatientTable.SUB_ID
                , ServerData.ShiftSpecialPatientTable.WARD_CODE, ServerData.ShiftSpecialPatientTable.WARD_NAME
                , ServerData.ShiftSpecialPatientTable.NURSING_CLASS, ServerData.ShiftSpecialPatientTable.DIET
                , ServerData.ShiftSpecialPatientTable.REQUESTDOCTOR_NAME, ServerData.ShiftSpecialPatientTable.ALLERGICDRUG
                , ServerData.ShiftSpecialPatientTable.ADVERSEREACTIONDRUG, ServerData.ShiftSpecialPatientTable.DIAGNOSIS
                , ServerData.ShiftSpecialPatientTable.OTHERSINFO, ServerData.ShiftSpecialPatientTable.REMARK
                , ServerData.ShiftSpecialPatientTable.SHIFT_DATE);

            string szValues = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}',{20}"
                , shiftSpecialPatient.ShiftRecordID, shiftSpecialPatient.BedCode, shiftSpecialPatient.PatientID
                , shiftSpecialPatient.PatientName, shiftSpecialPatient.PatientSex, shiftSpecialPatient.PatientAge
                , shiftSpecialPatient.PatientNo, shiftSpecialPatient.VisitID, shiftSpecialPatient.LogDateTime == DateTime.MinValue ? "''" : base.DataAccess.GetSqlTimeFormat(shiftSpecialPatient.LogDateTime)
                , shiftSpecialPatient.SubID, shiftSpecialPatient.WardCode, shiftSpecialPatient.WardName
                , shiftSpecialPatient.NursingClass, shiftSpecialPatient.Diet, shiftSpecialPatient.RequestDoctor
                , shiftSpecialPatient.AllergicDrug, shiftSpecialPatient.AdverseReactionDrug
                , shiftSpecialPatient.Diagnosis, shiftSpecialPatient.OthersInfo, shiftSpecialPatient.Remark
                , base.DataAccess.GetSqlTimeFormat(shiftSpecialPatient.ShiftDate));

            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_SHIFT_SPECIALPATIENT, szFields, szValues);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                if (base.DataAccess.IsConstraintConflictExpception(ex))
                {
                    if (this.UpdateShiftSpecialPatient(shiftSpecialPatient) == ServerData.ExecuteResult.OK)
                        return ServerData.ExecuteResult.OK;
                }
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 更新指定的特殊病人交班信息
        /// </summary>
        /// <param name="shiftSpecialPatient">特殊病人交班信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftSpecialPatient(ShiftSpecialPatient shiftSpecialPatient)
        {
            if (shiftSpecialPatient == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szFields = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}='{7}',{8}='{9}',{10}='{11}',{12}='{13}',{14}='{15}',{16}={17},"
                + "{18}='{19}',{20}='{21}',{22}='{23}',{24}='{25}',{26}='{27}',{28}='{29}',{30}='{31}',{32}='{33}',{34}='{35}',{36}='{37}',{38}='{39}'"
                + ",{40}={41}"
                , ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID,shiftSpecialPatient.ShiftRecordID
                , ServerData.ShiftSpecialPatientTable.BED_CODE,shiftSpecialPatient.BedCode
                , ServerData.ShiftSpecialPatientTable.PATIENT_ID,shiftSpecialPatient.PatientID
                , ServerData.ShiftSpecialPatientTable.PATIENT_NAME, shiftSpecialPatient.PatientName
                , ServerData.ShiftSpecialPatientTable.PATIENT_SEX, shiftSpecialPatient.PatientSex
                , ServerData.ShiftSpecialPatientTable.PATIENT_AGE, shiftSpecialPatient.PatientAge
                , ServerData.ShiftSpecialPatientTable.PATIENT_NO,shiftSpecialPatient.PatientNo
                , ServerData.ShiftSpecialPatientTable.VISIT_ID,shiftSpecialPatient.VisitID
                , ServerData.ShiftSpecialPatientTable.LOG_DATE_TIME, shiftSpecialPatient.LogDateTime == DateTime.MinValue ? "''" : base.DataAccess.GetSqlTimeFormat(shiftSpecialPatient.LogDateTime)
                , ServerData.ShiftSpecialPatientTable.SUB_ID, shiftSpecialPatient.SubID
                , ServerData.ShiftSpecialPatientTable.WARD_CODE, shiftSpecialPatient.WardCode
                , ServerData.ShiftSpecialPatientTable.WARD_NAME, shiftSpecialPatient.WardName
                , ServerData.ShiftSpecialPatientTable.NURSING_CLASS, shiftSpecialPatient.NursingClass
                , ServerData.ShiftSpecialPatientTable.DIET, shiftSpecialPatient.Diet
                , ServerData.ShiftSpecialPatientTable.REQUESTDOCTOR_NAME, shiftSpecialPatient.RequestDoctor
                , ServerData.ShiftSpecialPatientTable.ALLERGICDRUG, shiftSpecialPatient.AllergicDrug
                , ServerData.ShiftSpecialPatientTable.ADVERSEREACTIONDRUG, shiftSpecialPatient.AdverseReactionDrug
                , ServerData.ShiftSpecialPatientTable.DIAGNOSIS, shiftSpecialPatient.Diagnosis
                , ServerData.ShiftSpecialPatientTable.OTHERSINFO, shiftSpecialPatient.OthersInfo
                , ServerData.ShiftSpecialPatientTable.REMARK, shiftSpecialPatient.Remark
                , ServerData.ShiftSpecialPatientTable.SHIFT_DATE, base.DataAccess.GetSqlTimeFormat(shiftSpecialPatient.ShiftDate));
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='{7}'"
                , ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID, shiftSpecialPatient.ShiftRecordID
                , ServerData.ShiftSpecialPatientTable.PATIENT_ID, shiftSpecialPatient.PatientID
                , ServerData.ShiftSpecialPatientTable.VISIT_ID, shiftSpecialPatient.VisitID
                , ServerData.ShiftSpecialPatientTable.SUB_ID, shiftSpecialPatient.SubID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.NUR_SHIFT_SPECIALPATIENT, szFields, szCondition);
           
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("NurShiftAccess.UpdateShiftSpecialPatient", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取指定的交班记录对应的特殊病人交班列表
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="lstShiftSpecialPatients">交班病人列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftSpecialPatientList(string szShiftRecordID, ref List<ShiftSpecialPatient> lstShiftSpecialPatients)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftSpecialPatients == null)
                lstShiftSpecialPatients = new List<ShiftSpecialPatient>();
            lstShiftSpecialPatients.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}"
                , ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID, ServerData.ShiftSpecialPatientTable.BED_CODE
                , ServerData.ShiftSpecialPatientTable.PATIENT_ID, ServerData.ShiftSpecialPatientTable.PATIENT_NAME
                , ServerData.ShiftSpecialPatientTable.PATIENT_SEX, ServerData.ShiftSpecialPatientTable.PATIENT_AGE
                , ServerData.ShiftSpecialPatientTable.PATIENT_NO, ServerData.ShiftSpecialPatientTable.VISIT_ID
                , ServerData.ShiftSpecialPatientTable.LOG_DATE_TIME, ServerData.ShiftSpecialPatientTable.SUB_ID
                , ServerData.ShiftSpecialPatientTable.WARD_CODE, ServerData.ShiftSpecialPatientTable.WARD_NAME
                , ServerData.ShiftSpecialPatientTable.NURSING_CLASS, ServerData.ShiftSpecialPatientTable.DIET
                , ServerData.ShiftSpecialPatientTable.REQUESTDOCTOR_NAME, ServerData.ShiftSpecialPatientTable.ALLERGICDRUG
                , ServerData.ShiftSpecialPatientTable.ADVERSEREACTIONDRUG, ServerData.ShiftSpecialPatientTable.DIAGNOSIS
                , ServerData.ShiftSpecialPatientTable.OTHERSINFO, ServerData.ShiftSpecialPatientTable.REMARK
                , ServerData.ShiftSpecialPatientTable.SHIFT_DATE);
            string szTable = ServerData.DataTable.NUR_SHIFT_SPECIALPATIENT;
            string szCondition = string.Format("{0}='{1}'"
                , ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID, szShiftRecordID);
            string szOrder = ServerData.ShiftSpecialPatientTable.BED_CODE;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftSpecialPatient shiftSpecialPatient = new ShiftSpecialPatient();
                    if (!dataReader.IsDBNull(0)) shiftSpecialPatient.ShiftRecordID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftSpecialPatient.BedCode = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) shiftSpecialPatient.PatientID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftSpecialPatient.PatientName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) shiftSpecialPatient.PatientSex = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) shiftSpecialPatient.PatientAge = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) shiftSpecialPatient.PatientNo = int.Parse(dataReader.GetValue(6).ToString());
                    if (!dataReader.IsDBNull(7)) shiftSpecialPatient.VisitID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) shiftSpecialPatient.LogDateTime = dataReader.GetDateTime(8);
                    if (!dataReader.IsDBNull(9)) shiftSpecialPatient.SubID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) shiftSpecialPatient.WardCode = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) shiftSpecialPatient.WardName = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) shiftSpecialPatient.NursingClass = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) shiftSpecialPatient.Diet = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) shiftSpecialPatient.RequestDoctor = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) shiftSpecialPatient.AllergicDrug = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) shiftSpecialPatient.AdverseReactionDrug = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) shiftSpecialPatient.Diagnosis = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) shiftSpecialPatient.OthersInfo = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) shiftSpecialPatient.Remark = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20)) shiftSpecialPatient.ShiftDate = dataReader.GetDateTime(20);
                    lstShiftSpecialPatients.Add(shiftSpecialPatient);
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
        /// 获取指定病区指定交班日期对应的特殊病人交班列表
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="lstShiftSpecialPatients">特殊病人交班列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftSpecialPatientList(string szWardCode, DateTime dtShiftDate, ref List<ShiftSpecialPatient> lstShiftSpecialPatients)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstShiftSpecialPatients == null)
                lstShiftSpecialPatients = new List<ShiftSpecialPatient>();
            lstShiftSpecialPatients.Clear();

            string szField = string.Format("B.{0},B.{1},B.{2},B.{3},B.{4},B.{5},B.{6},B.{7},B.{8},B.{9},B.{10},B.{11}"
                + ",B.{12},B.{13},B.{14},B.{15},B.{16},B.{17},B.{18},B.{19},B.{20}"
                , ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID, ServerData.ShiftSpecialPatientTable.BED_CODE
                , ServerData.ShiftSpecialPatientTable.PATIENT_ID, ServerData.ShiftSpecialPatientTable.PATIENT_NAME
                , ServerData.ShiftSpecialPatientTable.PATIENT_SEX, ServerData.ShiftSpecialPatientTable.PATIENT_AGE
                , ServerData.ShiftSpecialPatientTable.PATIENT_NO, ServerData.ShiftSpecialPatientTable.VISIT_ID
                , ServerData.ShiftSpecialPatientTable.LOG_DATE_TIME, ServerData.ShiftSpecialPatientTable.SUB_ID
                , ServerData.ShiftSpecialPatientTable.DIAGNOSIS, ServerData.ShiftSpecialPatientTable.WARD_CODE
                , ServerData.ShiftSpecialPatientTable.WARD_NAME, ServerData.ShiftSpecialPatientTable.NURSING_CLASS
                , ServerData.ShiftSpecialPatientTable.DIET, ServerData.ShiftSpecialPatientTable.REQUESTDOCTOR_NAME
                , ServerData.ShiftSpecialPatientTable.ALLERGICDRUG, ServerData.ShiftSpecialPatientTable.ADVERSEREACTIONDRUG
                , ServerData.ShiftSpecialPatientTable.DIAGNOSIS, ServerData.ShiftSpecialPatientTable.OTHERSINFO
                , ServerData.ShiftSpecialPatientTable.REMARK);
               
            string szTable = string.Format("{0} A,{1} B", ServerData.DataTable.NUR_SHIFT_INDEX
                , ServerData.DataTable.NUR_SHIFT_SPECIALPATIENT);
            string szCondition = string.Format("A.{0}='{1}' AND A.{2}={3} AND A.{4}=B.{5} "
                , ServerData.ShiftRecordTable.WARD_CODE, szWardCode
                , ServerData.ShiftRecordTable.SHIFT_RECORD_DATE, base.DataAccess.GetSqlTimeFormat(dtShiftDate)
                , ServerData.ShiftRecordTable.SHIFT_RECORD_ID, ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID);
               
            string szOrder = string.Format("{0}", ServerData.ShiftSpecialPatientTable.BED_CODE);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    ShiftSpecialPatient shiftSpecialPatient = new ShiftSpecialPatient();
                    if (!dataReader.IsDBNull(0)) shiftSpecialPatient.ShiftRecordID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) shiftSpecialPatient.BedCode = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) shiftSpecialPatient.PatientID = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) shiftSpecialPatient.PatientName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) shiftSpecialPatient.PatientSex = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) shiftSpecialPatient.PatientAge = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) shiftSpecialPatient.PatientNo = int.Parse(dataReader.GetValue(6).ToString());
                    if (!dataReader.IsDBNull(7)) shiftSpecialPatient.VisitID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) shiftSpecialPatient.LogDateTime = dataReader.GetDateTime(8);
                    if (!dataReader.IsDBNull(9)) shiftSpecialPatient.SubID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) shiftSpecialPatient.Diagnosis = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) shiftSpecialPatient.WardCode = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) shiftSpecialPatient.WardName = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) shiftSpecialPatient.NursingClass = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14)) shiftSpecialPatient.Diet = dataReader.GetString(14);
                    if (!dataReader.IsDBNull(15)) shiftSpecialPatient.RequestDoctor = dataReader.GetString(15);
                    if (!dataReader.IsDBNull(16)) shiftSpecialPatient.AllergicDrug = dataReader.GetString(16);
                    if (!dataReader.IsDBNull(17)) shiftSpecialPatient.AdverseReactionDrug = dataReader.GetString(17);
                    if (!dataReader.IsDBNull(18)) shiftSpecialPatient.Diagnosis = dataReader.GetString(18);
                    if (!dataReader.IsDBNull(19)) shiftSpecialPatient.OthersInfo = dataReader.GetString(19);
                    if (!dataReader.IsDBNull(20)) shiftSpecialPatient.Remark = dataReader.GetString(20);
                    lstShiftSpecialPatients.Add(shiftSpecialPatient);
                } while (dataReader.Read());
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定的特殊病人交班信息
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="szPatientID">交班病人ID</param>
        /// <param name="szVisitID">交班病人就诊ID</param>
        /// <param name="szSubID">交班病人子ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftSpecialPatient(string szShiftRecordID, string szPatientID, string szVisitID, string szSubID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='{7}'"
                , ServerData.ShiftSpecialPatientTable.SHIFT_RECORD_ID, szShiftRecordID
                , ServerData.ShiftSpecialPatientTable.PATIENT_ID, szPatientID
                , ServerData.ShiftSpecialPatientTable.VISIT_ID, szVisitID
                , ServerData.ShiftSpecialPatientTable.SUB_ID, szSubID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.NUR_SHIFT_SPECIALPATIENT, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
        }


        #endregion
    }
}
