// ***********************************************************
// ���ݿ���ʲ������밲ȫ�����¼������.
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
    public class QCTypeAccess : DBAccessBase
    {
        #region"�����밲ȫ�����¼ģ���д�ӿ�"
        /// <summary>
        /// ��ȡ���������밲ȫ����������Ϣ�б�
        /// </summary>
        /// <param name="szApplyEnv">��Ӧ�û���</param>
        /// <param name="lstQCTypeInfos">�����밲ȫ����������Ϣ�б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetQCTypeInfos(string szApplyEnv, ref List<QCTypeInfo> lstQCTypeInfos)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA);
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

                if (lstQCTypeInfos == null)
                    lstQCTypeInfos = new List<QCTypeInfo>();
                do
                {
                    QCTypeInfo qcTypeInfo = new QCTypeInfo();
                    qcTypeInfo.QCTypeID = dataReader.GetString(0);
                    qcTypeInfo.QCTypeName = dataReader.GetString(1);
                    qcTypeInfo.ApplyEnv = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3))
                        qcTypeInfo.QCTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                    if (!dataReader.IsDBNull(4))
                        qcTypeInfo.IsRepeated = dataReader.GetValue(4).ToString().Equals("1");
                    if (!dataReader.IsDBNull(5))
                        qcTypeInfo.IsValid = dataReader.GetValue(5).ToString().Equals("1");
                    if (!dataReader.IsDBNull(6))
                        qcTypeInfo.IsVisible = dataReader.GetValue(6).ToString().Equals("1");
                    if (!dataReader.IsDBNull(7))
                        qcTypeInfo.PrintMode = (FormPrintMode)int.Parse(dataReader.GetValue(7).ToString());
                    if (!dataReader.IsDBNull(8))
                        qcTypeInfo.IsFolder = dataReader.GetValue(8).ToString().Equals("1");
                    if (!dataReader.IsDBNull(9))
                        qcTypeInfo.ParentID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10))
                        qcTypeInfo.ModifyTime = dataReader.GetDateTime(10);
                    if (!dataReader.IsDBNull(11))
                        qcTypeInfo.SortColumn = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12))
                        qcTypeInfo.SortMode = (SortMode)int.Parse(dataReader.GetValue(12).ToString());
                    if (!dataReader.IsDBNull(13))
                        qcTypeInfo.SchemaID = dataReader.GetString(13);
                    lstQCTypeInfos.Add(qcTypeInfo);
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
        public short GetQCTypeInfo(string szQCTypeID, ref QCTypeInfo qcTypeInfo)
        {
            if (GlobalMethods.Misc.IsEmptyString(szQCTypeID))
            {
                LogManager.Instance.WriteLog("QCTypeAccess.GetQCTypeInfo", new string[] { "szQCTypeID" }
                    , new object[] { szQCTypeID }, "��������Ϊ��");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            szQCTypeID = szQCTypeID.Trim();
            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.DocTypeTable.DOCTYPE_ID, ServerData.DocTypeTable.DOCTYPE_NAME, ServerData.DocTypeTable.APPLY_ENV
                , ServerData.DocTypeTable.DOCTYPE_NO, ServerData.DocTypeTable.IS_REPEATED, ServerData.DocTypeTable.IS_VALID
                , ServerData.DocTypeTable.IS_VISIBLE, ServerData.DocTypeTable.PRINT_MODE, ServerData.DocTypeTable.IS_FOLDER
                , ServerData.DocTypeTable.PARENT_ID, ServerData.DocTypeTable.MODIFY_TIME, ServerData.DocTypeTable.SORT_COLUMN
                , ServerData.DocTypeTable.SORT_MODE, ServerData.DocTypeTable.SCHEMA);
            string szCondition = string.Format("{0}='{1}'", ServerData.DocTypeTable.DOCTYPE_ID, szQCTypeID);
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC, szField
                , ServerData.DataTable.DOC_TYPE, szCondition, ServerData.DocTypeTable.DOCTYPE_NO);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    LogManager.Instance.WriteLog("QCTypeAccess.GetQCTypeInfo", new string[] { "szSQL" }
                        , new object[] { szSQL }, "��¼������!");
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (qcTypeInfo == null)
                    qcTypeInfo = new QCTypeInfo();
                qcTypeInfo.QCTypeID = dataReader.GetString(0);
                qcTypeInfo.QCTypeName = dataReader.GetString(1);
                qcTypeInfo.ApplyEnv = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    qcTypeInfo.QCTypeNo = int.Parse(dataReader.GetValue(3).ToString());
                if (!dataReader.IsDBNull(4))
                    qcTypeInfo.IsRepeated = dataReader.GetValue(4).ToString().Equals("1");
                if (!dataReader.IsDBNull(5))
                    qcTypeInfo.IsValid = dataReader.GetValue(5).ToString().Equals("1");
                if (!dataReader.IsDBNull(6))
                    qcTypeInfo.IsVisible = dataReader.GetValue(6).ToString().Equals("1");
                if (!dataReader.IsDBNull(7))
                    qcTypeInfo.PrintMode = (FormPrintMode)int.Parse(dataReader.GetValue(7).ToString());
                if (!dataReader.IsDBNull(8))
                    qcTypeInfo.IsFolder = dataReader.GetValue(8).ToString().Equals("1");
                if (!dataReader.IsDBNull(9))
                    qcTypeInfo.ParentID = dataReader.GetString(9);
                if (!dataReader.IsDBNull(10))
                    qcTypeInfo.ModifyTime = dataReader.GetDateTime(10);
                if (!dataReader.IsDBNull(11))
                    qcTypeInfo.SortColumn = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12))
                    qcTypeInfo.SortMode = (SortMode)int.Parse(dataReader.GetValue(12).ToString());
                if (!dataReader.IsDBNull(13))
                    qcTypeInfo.SchemaID = dataReader.GetString(13);
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL���ִ��ʧ��!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion
    }
}
