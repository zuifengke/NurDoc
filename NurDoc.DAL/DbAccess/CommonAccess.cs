// ***********************************************************
// ������ϵͳ���ݿ���ʲ㹫�����ݷ����йصĽӿ�.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class CommonAccess : DBAccessBase
    {
        /// <summary>
        /// ��ȡ���ݿ������ʱ��
        /// </summary>
        /// <param name="dtSysDate">���ݿ������ʱ��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetServerTime(ref DateTime dtSysDate)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szSQL = null;
            if (base.DataAccess.DatabaseType == DatabaseType.SQLSERVER)
            {
                szSQL = string.Format(ServerData.SQL.SELECT, "CONVERT(VARCHAR(20), GETDATE(), 20)");
            }
            else if (base.DataAccess.DatabaseType == DatabaseType.ORACLE)
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, "SYSDATE", "DUAL");
            }
            else
            {
                dtSysDate = DateTime.Now;
                return ServerData.ExecuteResult.OK;
            }
            object oRet = null;
            try
            {
                oRet = base.DataAccess.ExecuteScalar(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            try
            {
                dtSysDate = DateTime.Parse(oRet.ToString());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                string error = string.Format("�޷�����{0}��ת��ΪDateTime!", oRet);
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, error);
            }
        }

        /// <summary>
        /// ִ��ָ����SQL����ѯ
        /// </summary>
        /// <param name="sql">��ѯ��SQL���</param>
        /// <param name="result">��ѯ���صĽ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ExecuteQuery(string sql, out DataSet result)
        {
            result = null;
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            try
            {
                result = base.DataAccess.ExecuteDataSet(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), sql, "SQL���ִ��ʧ��!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ִ��ָ����SQL����ѯ,�Ѱ󶨱����ķ�ʽ
        /// </summary>
        /// <param name="sqlInfo">��ѯ��SqlInfo����</param>
        /// <param name="result">��ѯ���صĽ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ExecuteQuery(SqlInfo sqlInfo, out DataSet result)
        {
            result = null;
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;
            try
            {
                if (sqlInfo.Args == null)
                    result = base.DataAccess.ExecuteDataSet(sqlInfo.SQL, CommandType.Text);
                else
                {
                    DbParameter[] pmi = new DbParameter[sqlInfo.Args.Length];
                    for (int i = 0; i < sqlInfo.Args.Length; i++)
                    {
                        pmi[i] = new DbParameter(i.ToString(), sqlInfo.Args[i]);
                    }
                    result = base.DataAccess.ExecuteDataSet(sqlInfo.SQL, CommandType.Text, ref pmi);
                }
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), sqlInfo.SQL, "SQL���ִ��ʧ��!");
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ִ��ָ����һϵ�е�SQL������
        /// </summary>
        /// <param name="isProc">�����SQL�Ƿ��Ǵ洢����</param>
        /// <param name="sqlarray">��ѯ��SQL��伯��</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ExecuteUpdate(bool isProc, params string[] sqlarray)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            if (sqlarray == null)
                sqlarray = new string[0];
            foreach (string sql in sqlarray)
            {
                try
                {
                    if (!isProc)
                        base.DataAccess.ExecuteNonQuery(sql, CommandType.Text);
                    else
                        base.DataAccess.ExecuteNonQuery(sql, CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    base.DataAccess.AbortTransaction();
                    return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), sql, "SQL���ִ��ʧ��!");
                }
            }
            base.DataAccess.CommitTransaction(true);
            return ServerData.ExecuteResult.OK;
        }

        /// ��ȡ�ٴ����ԵĿ����б�
        /// </summary>
        /// <param name="lstDeptInfo">���صĿ����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetClinicDeptList(ref List<DeptInfo> lstDeptInfo)
        {
            return this.GetDeptList(ServerData.DeptView.IS_CLINIC, ref lstDeptInfo);
        }

        /// <summary>
        /// ��ȡ�������ԵĿ����б�
        /// </summary>
        /// <param name="lstDeptInfo">���صĿ����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWardDeptList(ref List<DeptInfo> lstDeptInfo)
        {
            return this.GetDeptList(ServerData.DeptView.IS_WARD, ref lstDeptInfo);
        }

        /// <summary>
        /// ��ȡ�������ԵĿ����б�
        /// </summary>
        /// <param name="lstDeptInfo">���صĿ����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNurseDeptList(ref List<DeptInfo> lstDeptInfo)
        {
            return this.GetDeptList(ServerData.DeptView.IS_NURSE, ref lstDeptInfo);
        }

        /// <summary>
        /// ��ȡ�������ԵĿ����б�
        /// </summary>
        /// <param name="lstDeptInfo">���صĿ����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetOutPDeptList(ref List<DeptInfo> lstDeptInfo)
        {
            return this.GetDeptList(ServerData.DeptView.IS_OUTP, ref lstDeptInfo);
        }

        /// <summary>
        /// ��ȡ�û������ԵĿ����б�
        /// </summary>
        /// <param name="lstDeptInfo">���صĿ����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetUserGroupList(ref List<DeptInfo> lstDeptInfo)
        {
            return this.GetDeptList(ServerData.DeptView.IS_GROUP, ref lstDeptInfo);
        }

        /// <summary>
        /// ��ȡָ�������ֶ����Ŀ����б�
        /// </summary>
        /// <param name="szAttrField">���������ֶ���</param>
        /// <param name="lstDeptInfo">���صĿ����б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetDeptList(string szAttrField, ref List<DeptInfo> lstDeptInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szAttrField))
            {
                LogManager.Instance.WriteLog("CommonAccess.GetDeptList", "���������ֶ�������Ϊ��!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.DeptView.DEPT_CODE, ServerData.DeptView.DEPT_NAME
                , ServerData.DeptView.IS_CLINIC, ServerData.DeptView.IS_WARD
                , ServerData.DeptView.IS_OUTP, ServerData.DeptView.IS_NURSE
                , ServerData.DeptView.IS_GROUP, ServerData.DeptView.INPUT_CODE);
            string szCondition = string.Format("{0}=1", szAttrField);
            string szTable = ServerData.DataView.DEPT;
            string szOrder = ServerData.DeptView.DEPT_NAME;
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField, szTable, szCondition, szOrder);
            
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }

                if (lstDeptInfo == null)
                    lstDeptInfo = new List<DeptInfo>();
                do
                {
                    DeptInfo deptInfo = new DeptInfo();
                    if (!dataReader.IsDBNull(0)) deptInfo.DeptCode = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) deptInfo.DeptName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) deptInfo.IsClinicDept = (dataReader.GetValue(2).ToString() == "1");
                    if (!dataReader.IsDBNull(3)) deptInfo.IsWardDept = (dataReader.GetValue(3).ToString() == "1");
                    if (!dataReader.IsDBNull(4)) deptInfo.IsOutpDept = (dataReader.GetValue(4).ToString() == "1");
                    if (!dataReader.IsDBNull(5)) deptInfo.IsNurseDept = (dataReader.GetValue(5).ToString() == "1");
                    if (!dataReader.IsDBNull(6)) deptInfo.IsUserGroup = (dataReader.GetValue(6).ToString() == "1");
                    if (!dataReader.IsDBNull(7)) deptInfo.InputCode = dataReader.GetString(7);
                    lstDeptInfo.Add(deptInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ȡȫԺ���п�����Ϣ
        /// </summary>
        /// <param name="lstDeptInfos">������Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAllDeptInfos(ref List<DeptInfo> lstDeptInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}"
                , ServerData.DeptView.DEPT_CODE, ServerData.DeptView.DEPT_NAME
                , ServerData.DeptView.IS_CLINIC, ServerData.DeptView.IS_WARD
                , ServerData.DeptView.IS_OUTP, ServerData.DeptView.IS_NURSE
                , ServerData.DeptView.IS_GROUP, ServerData.DeptView.INPUT_CODE);
            string szTable = ServerData.DataView.DEPT;
            string szOrder = ServerData.DeptView.DEPT_NAME;
            string szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField, szTable, szOrder);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
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
                    if (!dataReader.IsDBNull(6)) deptInfo.IsUserGroup = (dataReader.GetValue(6).ToString() == "1");
                    if (!dataReader.IsDBNull(7)) deptInfo.InputCode = dataReader.GetString(7);
                    lstDeptInfos.Add(deptInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ѯ�����ֵ��,��ȡ�����ֵ���д洢���ֵ������б�
        /// </summary>
        /// <param name="lstDictTypeList">�ֵ������б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetCommonDictTypeList(ref List<string> lstDictTypeList)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstDictTypeList == null)
                lstDictTypeList = new List<string>();
            lstDictTypeList.Clear();

            string szField = string.Format("DISTINCT {0}", ServerData.CommonDictTable.ITEM_TYPE);
            string szTable = ServerData.DataTable.NUR_COMMON_DICT;
            string szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC
                , szField, szTable, ServerData.CommonDictTable.ITEM_TYPE);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    if (!dataReader.IsDBNull(0)) lstDictTypeList.Add(dataReader.GetString(0));
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ��ѯ�����ֵ��,��ȡָ���ֵ����͵��ֵ�����
        /// </summary>
        /// <param name="szItemType">�ֵ�����</param>
        /// <param name="lstCommonDictItems">�ֵ���Ŀ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetCommonDict(string szItemType, ref List<CommonDictItem> lstCommonDictItems)
        {
            return this.GetCommonDict(szItemType, null, ref lstCommonDictItems);
        }

        /// <summary>
        /// ��ѯ�����ֵ��,��ȡָ���ֵ����͵��ֵ�����
        /// </summary>
        /// <param name="szItemType">�ֵ�����</param>
        /// <param name="szWardCode">��������</param>
        /// <param name="lstCommonDictItems">�ֵ���Ŀ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetCommonDict(string szItemType, string szWardCode, ref List<CommonDictItem> lstCommonDictItems)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstCommonDictItems == null)
                lstCommonDictItems = new List<CommonDictItem>();
            lstCommonDictItems.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictTable.ITEM_NO
                , ServerData.CommonDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_NAME
                , ServerData.CommonDictTable.WARD_CODE, ServerData.CommonDictTable.WARD_NAME
                , ServerData.CommonDictTable.INPUT_CODE);
            string szTable = ServerData.DataTable.NUR_COMMON_DICT;
            string szCondition = string.Format("{0}='{1}'", ServerData.CommonDictTable.ITEM_TYPE, szItemType);
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
            {
                szCondition = string.Format("{0} AND ({1}='{2}' OR {1}='ALL')"
                    , szCondition, ServerData.CommonDictTable.WARD_CODE, szWardCode);
            }
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC
                , szField, szTable, szCondition, ServerData.CommonDictTable.ITEM_NO);
           
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    CommonDictItem commonDictItem = new CommonDictItem();
                    if (!dataReader.IsDBNull(0)) commonDictItem.ItemType = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) commonDictItem.ItemNo = int.Parse(dataReader.GetValue(1).ToString());
                    if (!dataReader.IsDBNull(2)) commonDictItem.ItemCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) commonDictItem.ItemName = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) commonDictItem.WardCode = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) commonDictItem.WardName = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) commonDictItem.InputCode = dataReader.GetString(6);
                    lstCommonDictItems.Add(commonDictItem);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// �ӹ����ֵ����ɾ��ָ����һ���ֵ�����
        /// </summary>
        /// <param name="szItemType">�ֵ�����</param>
        /// <param name="szItemCode">��Ŀ����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteCommonDictItem(string szItemType, string szItemCode)
        {
            return this.DeleteCommonDictItem(szItemType, szItemCode, "ALL");
        }

        /// <summary>
        /// �ӹ����ֵ����ɾ��ָ����һ���ֵ�����
        /// </summary>
        /// <param name="szItemType">�ֵ�����</param>
        /// <param name="szItemCode">��Ŀ����</param>
        /// <param name="szWardCode">��������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteCommonDictItem(string szItemType, string szItemCode, string szWardCode)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            string szTable = ServerData.DataTable.NUR_COMMON_DICT;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'"
                , ServerData.CommonDictTable.ITEM_TYPE, szItemType, ServerData.CommonDictTable.ITEM_CODE, szItemCode
                , ServerData.CommonDictTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }

            if (szItemType == ServerData.CommonDictType.USER_GROUP)
            {
                szTable = ServerData.DataTable.USER_GROUP;
                szCondition = string.Format("{0}='{1}'", ServerData.UserGroupTable.GROUP_CODE, szItemCode);
                szSQL = string.Format(ServerData.SQL.DELETE, szTable, szCondition);
                try
                {
                    base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                }
                catch (Exception ex)
                {
                    base.DataAccess.AbortTransaction();
                    return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
                }
            }
            return base.DataAccess.CommitTransaction(true) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// �����µ�һ���ֵ����ݵ������ֵ��
        /// </summary>
        /// <param name="commonDictItem">�ֵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveCommonDictItem(CommonDictItem commonDictItem)
        {
            if (commonDictItem == null)
                return ServerData.ExecuteResult.RES_NO_FOUND;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
                , ServerData.CommonDictTable.ITEM_TYPE, ServerData.CommonDictTable.ITEM_NO
                , ServerData.CommonDictTable.ITEM_CODE, ServerData.CommonDictTable.ITEM_NAME
                , ServerData.CommonDictTable.WARD_CODE, ServerData.CommonDictTable.WARD_NAME
                , ServerData.CommonDictTable.INPUT_CODE);
            string szValue = string.Format("'{0}',{1},'{2}','{3}','{4}','{5}','{6}'"
                , commonDictItem.ItemType, commonDictItem.ItemNo
                , commonDictItem.ItemCode, commonDictItem.ItemName
                , commonDictItem.WardCode, commonDictItem.WardName, commonDictItem.InputCode);
            string szTable = ServerData.DataTable.NUR_COMMON_DICT;
            string szSQL = string.Format(ServerData.SQL.INSERT, szTable, szField, szValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            return nCount > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// ���¹����ֵ����ָ����һ���ֵ�����
        /// </summary>
        /// <param name="szItemType">�ֵ�����</param>
        /// <param name="szItemCode">�ֵ���Ŀ����</param>
        /// <param name="commonDictItem">�µ��ֵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateCommonDictItem(string szItemType, string szItemCode, CommonDictItem commonDictItem)
        {
            return this.UpdateCommonDictItem(szItemType, szItemCode, "ALL", commonDictItem);
        }

        /// <summary>
        /// ���¹����ֵ����ָ����һ���ֵ�����
        /// </summary>
        /// <param name="szItemType">�ֵ�����</param>
        /// <param name="szItemCode">�ֵ���Ŀ����</param>
        /// <param name="szWardCode">��������</param>
        /// <param name="commonDictItem">�µ��ֵ�����</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateCommonDictItem(string szItemType, string szItemCode, string szWardCode, CommonDictItem commonDictItem)
        {
            if (commonDictItem == null)
                return ServerData.ExecuteResult.RES_NO_FOUND;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            string szField = string.Format("{0}='{1}',{2}={3},{4}='{5}',{6}='{7}',{8}='{9}',{10}='{11}',{12}='{13}'"
                , ServerData.CommonDictTable.ITEM_TYPE, commonDictItem.ItemType
                , ServerData.CommonDictTable.ITEM_NO, commonDictItem.ItemNo
                , ServerData.CommonDictTable.ITEM_CODE, commonDictItem.ItemCode
                , ServerData.CommonDictTable.ITEM_NAME, commonDictItem.ItemName
                , ServerData.CommonDictTable.WARD_CODE, commonDictItem.WardCode
                , ServerData.CommonDictTable.WARD_NAME, commonDictItem.WardName
                , ServerData.CommonDictTable.INPUT_CODE, commonDictItem.InputCode);
            string szTable = ServerData.DataTable.NUR_COMMON_DICT;
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'"
                , ServerData.CommonDictTable.ITEM_TYPE, szItemType, ServerData.CommonDictTable.ITEM_CODE, szItemCode
                , ServerData.CommonDictTable.WARD_CODE, szWardCode);
            string szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);

            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }

            if (szItemType == ServerData.CommonDictType.USER_GROUP)
            {
                szField = string.Format("{0}='{1}'", ServerData.UserGroupTable.GROUP_CODE, commonDictItem.ItemCode);
                szTable = ServerData.DataTable.USER_GROUP;
                szCondition = string.Format("{0}='{1}'", ServerData.UserGroupTable.GROUP_CODE, szItemCode);
                szSQL = string.Format(ServerData.SQL.UPDATE, szTable, szField, szCondition);
                try
                {
                    base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
                }
                catch (Exception ex)
                {
                    base.DataAccess.AbortTransaction();
                    return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
                }
            }
            return base.DataAccess.CommitTransaction(true) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// �����ʿ�ϵͳ,��ȡ�����������������Ϣ�ֵ��б�
        /// </summary>
        /// <param name="lstQCEventTypes">�����������������Ϣ�ֵ��б�</param>
        /// <returns>ServerData.ReturnValue</returns>
        public short GetQCEventTypeList(ref List<QCEventType> lstQCEventTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4}", ServerData.QCMessageTypeView.SERIAL_NO
               , ServerData.QCMessageTypeView.QA_EVENT_TYPE, ServerData.QCMessageTypeView.INPUT_CODE
               , ServerData.QCMessageTypeView.PARENT_CODE, ServerData.QCMessageTypeView.MAX_SCORE);
            string szCondtion = string.Format("{0}='{1}'", ServerData.QCMessageTypeView.APPLY_ENV, "NURDOC");
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataView.MSG_TYPE_V, szCondtion, ServerData.QCMessageTypeView.SERIAL_NO);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstQCEventTypes == null)
                    lstQCEventTypes = new List<QCEventType>();
                do
                {
                    QCEventType qcEventType = new QCEventType();
                    if (!dataReader.IsDBNull(0)) qcEventType.SerialNo = dataReader.GetValue(0).ToString();
                    if (!dataReader.IsDBNull(1)) qcEventType.TypeDesc = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) qcEventType.InputCode = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) qcEventType.ParnetCode = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcEventType.MaxScore = Convert.ToDouble(dataReader.GetValue(4));
                    lstQCEventTypes.Add(qcEventType);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally
            {
                base.DataAccess.CloseConnnection(false);
            }
        }

        /// <summary>
        /// �����������ͻ�ȡ�ʿط�����Ϣ�ֵ��б�
        /// </summary>
        /// <param name="szQuestionType">��������</param>
        /// <param name="lstQCMessageTemplets">�ʿط�����Ϣ�ֵ��б�</param>
        /// <returns>MedDocSys.Common.ServerData.ReturnValue</returns>
        public short GetQCMessageTempletList(string szQuestionType, ref List<QCMessageTemplet> lstQCMessageTemplets)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6}"
               , ServerData.QCMessageTempletView.SERIAL_NO, ServerData.QCMessageTempletView.QC_MSG_CODE, ServerData.QCMessageTempletView.QA_EVENT_TYPE
               , ServerData.QCMessageTempletView.MESSAGE, ServerData.QCMessageTempletView.SCORE, ServerData.QCMessageTempletView.INPUT_CODE
               , ServerData.QCMessageTempletView.MESSAGE_TITLE);
            string szCondition = string.Format(" {0}='{1}' ", ServerData.QCMessageTempletView.APPLY_ENV, "NURDOC");
            if (!string.IsNullOrEmpty(szQuestionType))
                szCondition += string.Format(" AND {0}='{1}'", ServerData.QCMessageTempletView.QA_EVENT_TYPE, szQuestionType);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataView.MSG_TEMPLET_V, szCondition, ServerData.QCMessageTempletView.SERIAL_NO);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstQCMessageTemplets == null)
                    lstQCMessageTemplets = new List<QCMessageTemplet>();
                do
                {
                    QCMessageTemplet qcMessageTemplet = new QCMessageTemplet();
                    if (!dataReader.IsDBNull(0)) qcMessageTemplet.SerialNo = dataReader.GetValue(0).ToString();
                    if (!dataReader.IsDBNull(1)) qcMessageTemplet.QCMsgCode = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) qcMessageTemplet.QCEventType = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) qcMessageTemplet.Message = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) qcMessageTemplet.Score = dataReader.GetValue(4).ToString();
                    if (!dataReader.IsDBNull(5)) qcMessageTemplet.InputCode = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) qcMessageTemplet.MessageTitle = dataReader.GetString(6);
                    lstQCMessageTemplets.Add(qcMessageTemplet);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally
            {
                base.DataAccess.CloseConnnection(false);
            }
        }
    }
}
