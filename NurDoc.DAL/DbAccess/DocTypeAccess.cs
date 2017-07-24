// ***********************************************************
// ���ݿ���ʲ��ĵ������ֵ����ݷ�����.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class DocTypeAccess : DBAccessBase
    {
        #region"�����������ݷ��ʽӿ�"
        /// <summary>
        /// ��ȡ�����ĵ�������Ϣ�б�
        /// </summary>
        /// <param name="szApplyEnv">��Ӧ�û���</param>
        /// <param name="lstDocTypeInfos">�ĵ�������Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocTypeInfos(string szApplyEnv, ref List<DocTypeInfo> lstDocTypeInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA, ServerData.DocTypeTable.IS_QC_VISIBLE);

            string szSQL = null;
            if (GlobalMethods.Misc.IsEmptyString(szApplyEnv))
            {
                szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC, szField
                    , ServerData.DataTable.DOC_TYPE, ServerData.DocTypeTable.DOCTYPE_NO);
            }
            else
            {
                string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.APPLY_ENV, szApplyEnv);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                    , ServerData.DataTable.DOC_TYPE, szCondition, ServerData.DocTypeTable.DOCTYPE_NO);
            }
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;

                if (lstDocTypeInfos == null)
                    lstDocTypeInfos = new List<DocTypeInfo>();
                do
                {
                    DocTypeInfo docTypeInfo = new DocTypeInfo();
                    docTypeInfo.DocTypeID = dataReader.GetString(0);
                    docTypeInfo.DocTypeName = dataReader.GetString(1);
                    docTypeInfo.ApplyEnv = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        docTypeInfo.DocTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4))
                        docTypeInfo.IsRepeated = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        docTypeInfo.IsValid = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6))
                        docTypeInfo.IsVisible = dataReader.GetValue(6).ToString().Equals("1");
                    if (!dataReader.IsDBNull(7))
                        docTypeInfo.PrintMode = (FormPrintMode)int.Parse(dataReader.GetValue(7).ToString());
                    if (!dataReader.IsDBNull(8))
                        docTypeInfo.IsFolder = dataReader.GetValue(8).ToString().Equals("1");
                    if (!dataReader.IsDBNull(9))
                        docTypeInfo.ParentID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10))
                        docTypeInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11))
                        docTypeInfo.SortColumn = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12))
                        docTypeInfo.SortMode = (SortMode)int.Parse(dataReader.GetValue(12).ToString());
                    if (!dataReader.IsDBNull(13))
                        docTypeInfo.SchemaID = dataReader.GetString(13);
                    if (!dataReader.IsDBNull(14))
                        docTypeInfo.IsQCVisible = dataReader.GetValue(14).ToString().Equals("1");
                    lstDocTypeInfos.Add(docTypeInfo);
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
        /// ����ָ���ĵ����ʹ����ȡ�ĵ�������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">�ĵ����ʹ���</param>
        /// <param name="docTypeInfo">�ĵ�������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocTypeInfo(string szDocTypeID, ref DocTypeInfo docTypeInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.GetDocTypeInfo", new string[] { "szDocTypeID" }
                    , new object[] { szDocTypeID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocTypeID = szDocTypeID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA, ServerData.DocTypeTable.IS_QC_VISIBLE);
            string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataTable.DOC_TYPE, szCondition, ServerData.DocTypeTable.DOCTYPE_NO);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("DocTypeAccess.GetDocTypeInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "��¼������!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (docTypeInfo == null)
                    docTypeInfo = new DocTypeInfo();
                docTypeInfo.DocTypeID = dataReader.GetString(0);
                docTypeInfo.DocTypeName = dataReader.GetString(1);
                docTypeInfo.ApplyEnv = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    docTypeInfo.DocTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                if (!dataReader.IsDBNull(4))
                    docTypeInfo.IsRepeated = dataReader.GetValue(4).ToString().Equals("1");
                if (!dataReader.IsDBNull(5))
                    docTypeInfo.IsValid = dataReader.GetValue(5).ToString().Equals("1");
                if (!dataReader.IsDBNull(6))
                    docTypeInfo.IsVisible = dataReader.GetValue(6).ToString().Equals("1");
                if (!dataReader.IsDBNull(7))
                    docTypeInfo.PrintMode = (FormPrintMode)int.Parse(dataReader.GetValue(7).ToString());
                if (!dataReader.IsDBNull(8))
                    docTypeInfo.IsFolder = dataReader.GetValue(8).ToString().Equals("1");
                if (!dataReader.IsDBNull(9))
                    docTypeInfo.ParentID = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10))
                    docTypeInfo.ModifyTime = dataReader.GetDateTime(10);
                if (!dataReader.IsDBNull(11))
                    docTypeInfo.SortColumn = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12))
                    docTypeInfo.SortMode = (SortMode)int.Parse(dataReader.GetValue(12).ToString());
                if (!dataReader.IsDBNull(13))
                    docTypeInfo.SchemaID = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14))
                    docTypeInfo.IsQCVisible = dataReader.GetValue(14).ToString().Equals("1");
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }

        /// <summary>
        /// ����һ���µĲ�������������Ϣ
        /// </summary>
        /// <param name="docTypeInfo">��������</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveDocTypeInfo(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || GlobalMethods.Misc.IsEmptyString(docTypeInfo.DocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.SaveDocTypeInfo", new string[] { "docTypeInfo" }
                    , new object[] { docTypeInfo }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA,ServerData.DocTypeTable.IS_QC_VISIBLE);
            string szValue = string.Format("'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8},'{9}',{10},'{11}','{12}','{13}',{14}"
                , docTypeInfo.DocTypeID, docTypeInfo.DocTypeName, docTypeInfo.ApplyEnv, docTypeInfo.DocTypeNo
                , docTypeInfo.IsRepeated ? "1" : "0", docTypeInfo.IsValid ? "1" : "0"
                , docTypeInfo.IsVisible ? "1" : "0", ((int)docTypeInfo.PrintMode).ToString(), docTypeInfo.IsFolder ? "1" : "0"
                , docTypeInfo.ParentID, base.DataAccess.GetSystemTimeSql(), docTypeInfo.SortColumn, ((int)docTypeInfo.SortMode).ToString()
                , docTypeInfo.SchemaID,docTypeInfo.IsQCVisible ?"1":"0");
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.DOC_TYPE, szField, szValue);
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocTypeAccess.SaveDocTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ����ָ���ĵ����ʹ�������ĵ�������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID��</param>
        /// <param name="docTypeInfo">�ĵ�������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ModifyDocTypeInfo(string szDocTypeID, DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null || GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.ModifyDocTypeInfo", new string[] { "docTypeInfo" }
                    , new object[] { docTypeInfo }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szDocTypeID = szDocTypeID.Trim();
            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}={7},{8}={9},{10}={11},{12}={13},{14}={15},{16}={17},{18}='{19}',{20}={21},{22}='{23}',{24}={25},{26}='{27}',{28}={29}"
                , ServerData.DocTypeTable.DOCTYPE_ID, docTypeInfo.DocTypeID
                , ServerData.DocTypeTable.DOCTYPE_NAME, docTypeInfo.DocTypeName
                , ServerData.DocTypeTable.APPLY_ENV, docTypeInfo.ApplyEnv
                , ServerData.DocTypeTable.DOCTYPE_NO, docTypeInfo.DocTypeNo.ToString()
                , ServerData.DocTypeTable.IS_REPEATED, docTypeInfo.IsRepeated ? "1" : "0"
                , ServerData.DocTypeTable.IS_VALID, docTypeInfo.IsValid ? "1" : "0"
                , ServerData.DocTypeTable.IS_VISIBLE, docTypeInfo.IsVisible ? "1" : "0"
                , ServerData.DocTypeTable.PRINT_MODE, ((int)docTypeInfo.PrintMode).ToString()
                , ServerData.DocTypeTable.IS_FOLDER, docTypeInfo.IsFolder ? "1" : "0"
                , ServerData.DocTypeTable.PARENT_ID, docTypeInfo.ParentID
                , ServerData.DocTypeTable.MODIFY_TIME, base.DataAccess.GetSystemTimeSql()
                , ServerData.DocTypeTable.SORT_COLUMN, docTypeInfo.SortColumn
                , ServerData.DocTypeTable.SORT_MODE, ((int)docTypeInfo.SortMode).ToString()
                , ServerData.DocTypeTable.SCHEMA, docTypeInfo.SchemaID
                , ServerData.DocTypeTable.IS_QC_VISIBLE, docTypeInfo.IsQCVisible ? "1" : "0");
            string szCondiction = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.DOC_TYPE, szField, szCondiction);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocTypeAccess.ModifyDocTypeInfo", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ɾ��ָ����һϵ�еĲ�������
        /// </summary>
        /// <param name="lstDocTypeID">��������ID�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteDocTypeInfos(List<string> lstDocTypeID)
        {
            if (lstDocTypeID == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = ServerData.ExecuteResult.OK;
            for (int index = 0; index < lstDocTypeID.Count; index++)
            {
                string szDocTypeID = lstDocTypeID[index];
                if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
                    continue;
                shRet = this.DeleteDocTypeInfo(szDocTypeID);
                if (shRet != ServerData.ExecuteResult.OK)
                    break;
            }
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
            }
            else
            {
                if (!base.DataAccess.CommitTransaction(true))
                    return ServerData.ExecuteResult.EXCEPTION;
            }
            return shRet;
        }

        /// <summary>
        /// ���ݲ�������ID,ɾ��һ����������������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">��������ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteDocTypeInfo(string szDocTypeID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.DeleteDocTypeInfo", new string[] { "szDocTypeID" },
                    new object[] { szDocTypeID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.DOC_TYPE, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            this.DeleteWardDocTypes(szDocTypeID);        //ɾ��������������
            this.DeleteGridViewColumns(szDocTypeID);      //ɾ����������ģ��ժҪ��
            return (nCount > 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }

        /// <summary>
        /// ɾ��ָ�����ģ�������������
        /// </summary>
        /// <param name="szSchemaID">���ģ��ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteGridViewColumns(string szSchemaID)
        {
            if (string.IsNullOrEmpty(szSchemaID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'"
                , ServerData.GridViewColumnTable.SCHEMA_ID, szSchemaID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.GRID_VIEW_COLUMN, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }
        #endregion

        #region"�������������������ݷ��ʽӿ�"
        /// <summary>
        /// ��ȡ�����ĵ����ʹ���Ӧ�õ��Ĳ����б�
        /// </summary>
        /// <param name="szWardCode">��������</param>
        /// <param name="lstWardDocTypes">�������������б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWardDocTypeList(string szWardCode, ref List<WardDocType> lstWardDocTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, ServerData.WardDocTypeTable.DOCTYPE_NAME
                , ServerData.WardDocTypeTable.WARD_CODE, ServerData.WardDocTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardDocTypeTable.WARD_CODE, szWardCode);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_DOC_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_DOC_TYPE);
            }

            if (lstWardDocTypes == null)
                lstWardDocTypes = new List<WardDocType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardDocType wardDocType = new WardDocType();
                    wardDocType.DocTypeID = dataReader.GetString(0);
                    wardDocType.DocTypeName = dataReader.GetString(1);
                    wardDocType.WardCode = dataReader.GetString(2);
                    wardDocType.WardName = dataReader.GetString(3);
                    lstWardDocTypes.Add(wardDocType);
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
        /// ��ȡ�����ĵ����ʹ���Ӧ�õ��Ĳ����б�
        /// </summary>
        /// <param name="szDocTypeID">��������ID��</param>
        /// <param name="lstWardDocTypes">�������������б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocTypeDeptList(string szDocTypeID, ref List<WardDocType> lstWardDocTypes)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, ServerData.WardDocTypeTable.DOCTYPE_NAME
                , ServerData.WardDocTypeTable.WARD_CODE, ServerData.WardDocTypeTable.WARD_NAME);

            string szSQL = null;
            if (!GlobalMethods.Misc.IsEmptyString(szDocTypeID))
            {
                string szCondition = string.Format("{0}='{1}'"
                       , ServerData.WardDocTypeTable.DOCTYPE_ID, szDocTypeID);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE
                    , szField, ServerData.DataTable.WARD_DOC_TYPE, szCondition);
            }
            else
            {
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.WARD_DOC_TYPE);
            }

            if (lstWardDocTypes == null)
                lstWardDocTypes = new List<WardDocType>();
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.OK;
                do
                {
                    WardDocType wardDocType = new WardDocType();
                    wardDocType.DocTypeID = dataReader.GetString(0);
                    wardDocType.DocTypeName = dataReader.GetString(1);
                    wardDocType.WardCode = dataReader.GetString(2);
                    wardDocType.WardName = dataReader.GetString(3);
                    lstWardDocTypes.Add(wardDocType);
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
        /// ����ָ����һϵ�в�����������������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID</param>
        /// <param name="wardDocTypes">������������������Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardDocTypes(string szDocTypeID, List<WardDocType> wardDocTypes)
        {
            if (wardDocTypes == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = this.DeleteWardDocTypes(szDocTypeID);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            foreach (WardDocType wardDocType in wardDocTypes)
            {
                if (wardDocType == null)
                    continue;
                shRet = this.SaveWardDocType(wardDocType);
                if (shRet != ServerData.ExecuteResult.OK)
                    break;
            }
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
            }
            else
            {
                if (!base.DataAccess.CommitTransaction(true))
                    return ServerData.ExecuteResult.EXCEPTION;
            }
            return shRet;
        }

        /// <summary>
        /// ����һ���µĲ�����������������Ϣ
        /// </summary>
        /// <param name="wardDocType">��������������Ϣ</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveWardDocType(WardDocType wardDocType)
        {
            if (wardDocType == null || GlobalMethods.Misc.IsEmptyString(wardDocType.DocTypeID))
            {
                LogManager.Instance.WriteLog("DocTypeAccess.SaveWardDocType", new string[] { "wardDocType" }
                    , new object[] { wardDocType }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, ServerData.WardDocTypeTable.DOCTYPE_NAME
                , ServerData.WardDocTypeTable.WARD_CODE, ServerData.WardDocTypeTable.WARD_NAME);
            string szValue = string.Format("'{0}','{1}','{2}','{3}'"
                , wardDocType.DocTypeID, wardDocType.DocTypeName, wardDocType.WardCode, wardDocType.WardName);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.WARD_DOC_TYPE, szField, szValue);
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("DocTypeAccess.SaveWardDocType", new string[] { "szSQL" }, new object[] { szSQL }, "SQL���ִ�к󷵻�0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// ���ݲ������ʹ���ɾ����ǰ�������Ͷ�Ӧ�Ĳ�����������������Ϣ
        /// </summary>
        /// <param name="szDocTypeID">��������ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteWardDocTypes(string szDocTypeID)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'"
                , ServerData.WardDocTypeTable.DOCTYPE_ID, szDocTypeID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.WARD_DOC_TYPE, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.OTHER_ERROR;
        }
        #endregion
    }
}
