// ***********************************************************
// 护理病历系统数据库访问层配置数据访问有关的接口.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;
using Heren.Common.Libraries.Ftp;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class ConfigAccess : DBAccessBase
    {
        /// <summary>
        /// 测试方法，后期删除
        /// </summary>
        /// <param name="szTest"></param>
        /// <returns></returns>
        public short Test(string szTest)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                //RestHandler.Instance.ClearParameters();
                //RestHandler.Instance.AddParameter("szTest", szTest);
                //short result = RestHandler.Instance.Get<DocStatusInfo>("MedDocAccess/GetDocStatusInfo", ref docStatusInfo);
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            return ServerData.ExecuteResult.OK;
        }



        #region"配置字典表访问"
        /// <summary>
        /// 查询配置字典表获取指定的配置数据
        /// </summary>
        /// <param name="szGroupName">配置组名称</param>
        /// <param name="szConfigName">配置项名称(为空时返回该组所有配置项)</param>
        /// <param name="lstConfigInfos">返回的配置项及其配置数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetConfigData(string szGroupName, string szConfigName, ref List<ConfigInfo> lstConfigInfos)
        {
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("groupName", szGroupName);
                RestHandler.Instance.AddParameter("configName", szConfigName);
                short shRet= RestHandler.Instance.Get<ConfigInfo>("ConfigAccess/GetConfigData", ref lstConfigInfos);
                return shRet;
            }
            else
            {
                if (base.DataAccess == null)
                    return ServerData.ExecuteResult.PARAM_ERROR;

                string szField = string.Format("{0},{1},{2},{3}"
                    , ServerData.ConfigDictTable.GROUP_NAME, ServerData.ConfigDictTable.CONFIG_NAME
                    , ServerData.ConfigDictTable.CONFIG_VALUE, ServerData.ConfigDictTable.CONFIG_DESC);

                string szCondition = null;
                if (!GlobalMethods.Misc.IsEmptyString(szGroupName))
                {
                    szCondition = string.Format("{0}='{1}'", ServerData.ConfigDictTable.GROUP_NAME, szGroupName);
                    if (!GlobalMethods.Misc.IsEmptyString(szConfigName))
                    {
                        szCondition = string.Format("{0} AND {1}='{2}'", szCondition
                            , ServerData.ConfigDictTable.CONFIG_NAME, szConfigName);
                    }
                }

                string szOrder = string.Format("{0},{1}"
                    , ServerData.ConfigDictTable.GROUP_NAME, ServerData.ConfigDictTable.CONFIG_NAME);

                string szSQL = null;
                if (string.IsNullOrEmpty(szCondition))
                {
                    szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC
                        , szField, ServerData.DataTable.SYSTEM_CONFIG, szOrder);
                }
                else
                {
                    szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC
                        , szField, ServerData.DataTable.SYSTEM_CONFIG, szCondition, szOrder);
                }

                IDataReader dataReader = null;
                try
                {
                    dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                    if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    {
                        return ServerData.ExecuteResult.RES_NO_FOUND;
                    }
                    if (lstConfigInfos == null)
                        lstConfigInfos = new List<ConfigInfo>();
                    do
                    {
                        ConfigInfo configInfo = new ConfigInfo();
                        if (!dataReader.IsDBNull(0)) configInfo.GroupName = dataReader.GetString(0);
                        if (!dataReader.IsDBNull(1)) configInfo.ConfigName = dataReader.GetString(1);
                        if (!dataReader.IsDBNull(2)) configInfo.ConfigValue = dataReader.GetString(2);
                        if (!dataReader.IsDBNull(3)) configInfo.ConfigDesc = dataReader.GetString(3);
                        lstConfigInfos.Add(configInfo);
                    } while (dataReader.Read());
                    return ServerData.ExecuteResult.OK;
                }
                catch (Exception ex)
                {
                    return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
                }
                finally { base.DataAccess.CloseConnnection(false); }
            }
        }

        /// <summary>
        /// 修改配置字典表中指定的配置数据
        /// </summary>
        /// <param name="configInfo">配置项及其配置数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveConfigData(ConfigInfo configInfo)
        {
            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.ConfigDictTable.GROUP_NAME, ServerData.ConfigDictTable.CONFIG_NAME
                , ServerData.ConfigDictTable.CONFIG_VALUE, ServerData.ConfigDictTable.CONFIG_DESC);
            string szValue = string.Format("'{0}','{1}',{2},'{3}'"
                , configInfo.GroupName, configInfo.ConfigName
                , base.DataAccess.GetSqlParamName("ConfigValue"), configInfo.ConfigDesc);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.SYSTEM_CONFIG, szField, szValue);

            DbParameter[] pmi = new DbParameter[1];
            pmi[0] = new DbParameter("ConfigValue", configInfo.ConfigValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return nCount > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// 修改配置字典表中指定的配置数据
        /// </summary>
        /// <param name="szGroupName">配置组</param>
        /// <param name="szConfigName">配置项</param>
        /// <param name="configInfo">配置项及其配置数据</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateConfigData(string szGroupName, string szConfigName, ConfigInfo configInfo)
        {
            string szField = string.Format("{0}='{1}',{2}='{3}',{4}={5},{6}='{7}'"
                , ServerData.ConfigDictTable.GROUP_NAME, configInfo.GroupName
                , ServerData.ConfigDictTable.CONFIG_NAME, configInfo.ConfigName
                , ServerData.ConfigDictTable.CONFIG_VALUE, base.DataAccess.GetSqlParamName("ConfigValue")
                , ServerData.ConfigDictTable.CONFIG_DESC, configInfo.ConfigDesc);
            string szCondition = string.Format("{0}='{1}' and {2}='{3}'"
                , ServerData.ConfigDictTable.GROUP_NAME, szGroupName
                , ServerData.ConfigDictTable.CONFIG_NAME, szConfigName);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.SYSTEM_CONFIG, szField, szCondition);

            DbParameter[] pmi = new DbParameter[1];
            pmi[0] = new DbParameter("ConfigValue", configInfo.ConfigValue);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return nCount > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// 删除配置字典表中指定的配置数据
        /// </summary>
        /// <param name="szGroupName">配置组</param>
        /// <param name="szConfigName">配置项</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteConfigData(string szGroupName, string szConfigName)
        {
            if (GlobalMethods.Misc.IsEmptyString(szGroupName))
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.ConfigDictTable.GROUP_NAME, szGroupName);
            if (!GlobalMethods.Misc.IsEmptyString(szConfigName))
            {
                szCondition = string.Format("{0} AND {1}='{2}'", szCondition
                    , ServerData.ConfigDictTable.CONFIG_NAME, szConfigName);
            }
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.SYSTEM_CONFIG, szCondition);

            int count = 0;
            try
            {
                count = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return count > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }

        /// <summary>
        /// 获取文档存储模式
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        public StorageMode GetStorageMode()
        {
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.GetConfigData(ServerData.ConfigKey.STORAGE_MODE, ServerData.ConfigKey.STORAGE_MODE, ref lstConfigInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return StorageMode.Unknown;

            if (shRet != ServerData.ExecuteResult.OK || lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return StorageMode.Unknown;

            ConfigInfo configInfo = lstConfigInfos[0];
            if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigValue))
                return StorageMode.Unknown;

            configInfo.ConfigValue = configInfo.ConfigValue.Trim().ToUpper();
            return (configInfo.ConfigValue == ServerData.ConfigKey.STORAGE_MODE_FTP) ? StorageMode.FTP : StorageMode.DB;
        }

        /// <summary>
        /// 获取配置字典表中FTP服务器的访问参数
        /// </summary>
        /// <param name="ftpConfig">IP地址</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetDocFtpParams(ref FtpConfig ftpConfig)
        {
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.GetConfigData(ServerData.ConfigKey.DOC_FTP, null, ref lstConfigInfos);

            ftpConfig = new FtpConfig();
            if (lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return shRet;

            for (int index = 0; index < lstConfigInfos.Count; index++)
            {
                ConfigInfo configInfo = lstConfigInfos[index];
                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigName))
                    continue;

                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigValue))
                    configInfo.ConfigValue = string.Empty;

                configInfo.ConfigName = configInfo.ConfigName.Trim().ToUpper();

                if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_IP))
                {
                    ftpConfig.FtpIP = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_PORT))
                {
                    int nPort = 0;
                    if (int.TryParse(configInfo.ConfigValue, out nPort))
                        ftpConfig.FtpPort = nPort;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_USER))
                {
                    ftpConfig.FtpUser = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_PWD))
                {
                    ftpConfig.FtpPwd = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_MODE))
                {
                    if (configInfo.ConfigValue == "1")
                        ftpConfig.FtpMode = FtpMode.PORT;
                }
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取配置字典表中程序升级FTP服务器的访问参数
        /// </summary>
        /// <param name="ftpConfig">IP地址</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetUpgradeFtpParams(ref FtpConfig ftpConfig)
        {
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.GetConfigData(ServerData.ConfigKey.UPGRADE_FTP, null, ref lstConfigInfos);

            ftpConfig = new FtpConfig();
            if (lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return shRet;

            for (int index = 0; index < lstConfigInfos.Count; index++)
            {
                ConfigInfo configInfo = lstConfigInfos[index];
                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigName))
                    continue;

                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigValue))
                    configInfo.ConfigValue = string.Empty;

                configInfo.ConfigName = configInfo.ConfigName.Trim().ToUpper();

                if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_IP))
                {
                    ftpConfig.FtpIP = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_PORT))
                {
                    int nPort = 0;
                    if (int.TryParse(configInfo.ConfigValue, out nPort))
                        ftpConfig.FtpPort = nPort;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_USER))
                {
                    ftpConfig.FtpUser = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_PWD))
                {
                    ftpConfig.FtpPwd = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_MODE))
                {
                    if (configInfo.ConfigValue == "1")
                        ftpConfig.FtpMode = FtpMode.PORT;
                }
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取配置字典表中护理信息库FTP服务器的访问参数
        /// </summary>
        /// <param name="ftpConfig">IP地址</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetInfoLibFtpParams(ref FtpConfig ftpConfig)
        {
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.GetConfigData(ServerData.ConfigKey.INFOLIB_FTP, null, ref lstConfigInfos);

            ftpConfig = new FtpConfig();
            if (lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return shRet;

            for (int index = 0; index < lstConfigInfos.Count; index++)
            {
                ConfigInfo configInfo = lstConfigInfos[index];
                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigName))
                    continue;

                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigValue))
                    configInfo.ConfigValue = string.Empty;

                configInfo.ConfigName = configInfo.ConfigName.Trim().ToUpper();

                if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_IP))
                {
                    ftpConfig.FtpIP = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_PORT))
                {
                    int nPort = 0;
                    if (int.TryParse(configInfo.ConfigValue, out nPort))
                        ftpConfig.FtpPort = nPort;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_USER))
                {
                    ftpConfig.FtpUser = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_PWD))
                {
                    ftpConfig.FtpPwd = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.FTP_MODE))
                {
                    if (configInfo.ConfigValue == "1")
                        ftpConfig.FtpMode = FtpMode.PORT;
                }
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取护理病历系统选项在配置字典表中的配置选项
        /// </summary>
        /// <param name="systemOption">配置选项列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSystemOptions(ref SystemOption systemOption)
        {
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.GetConfigData(ServerData.ConfigKey.SYSTEM_OPTION, null, ref lstConfigInfos);

            systemOption = new SystemOption();
            if (lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return shRet;

            for (int index = 0; index < lstConfigInfos.Count; index++)
            {
                ConfigInfo configInfo = lstConfigInfos[index];
                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigName))
                    continue;

                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigValue))
                    configInfo.ConfigValue = string.Empty;

                configInfo.ConfigName = configInfo.ConfigName.Trim().ToUpper();

                if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_CERT_CODE))
                {
                    systemOption.CertCode = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_HOSPITAL_NAME))
                {
                    systemOption.HospitalName = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_SINGLE_PATIENT))
                {
                    systemOption.SinglePatientMode = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_EMRS_VITAL))
                {
                    systemOption.EmrsVital = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_NURREC_DOUBLESIGN))
                {
                    systemOption.NursingRecordDoubleSign = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_OPTION_BATCH_RECORD_PRINT))
                {
                    systemOption.OptionBatchRecordPrint = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_REC_TABLE_INPUT))
                {
                    systemOption.OptionRecTableInput = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_REC_SHOW_BY_TYPE))
                {
                    systemOption.OptionRecShowByType = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_REC_SHOW_BY_TYPE_START_TIME))
                {
                    DateTime StartTime = DateTime.MinValue;
                    if (DateTime.TryParse(configInfo.ConfigValue, out StartTime))
                        systemOption.OptionRecShowByTypeStartTime = StartTime;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_OPTION_ORDERS_PRINT))
                {
                    systemOption.OptionOrdersPrint = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_OPTION_CALCULATE_SUMMARY))
                {
                    systemOption.OptionCalculateSummary = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 0);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_OPTION_VITAL_BP_COUNT))
                {
                    systemOption.OptionBPCount = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 0);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTION_TASK_MESSAGE_TIME))
                {
                    systemOption.TaskMessageTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 0);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.DOC_PIGEONHOLE))
                {
                    systemOption.DocPigeonhole = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.REC_SUMMARYNAME))
                {
                    systemOption.RecSummaryName = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.PLAN_ORDERS_REC))
                {
                    systemOption.PlanOrderRec = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_SPECIAL_NURSING_INCREASE))
                {
                    systemOption.SpecialNursingIncrease = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_NURRECORD_UNIT))
                {
                    systemOption.NurRecordUnit = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.NPC_NEW_MODEL))
                {
                    systemOption.NpcNewModel = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.VITAL_SIGNS_SYNC))
                {
                    systemOption.VitalSignsSync = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.NCP_LIST_SORT_YW))
                {
                    systemOption.NcpListStatusSort = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.DOC_LIST_LOAD_TIME))
                {
                    systemOption.DocLoadTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 0);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.NUR_REC_SCHEMA_LIST_SHOWALL))
                {
                    systemOption.ShowAllInNurRec = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.NUR_SPECIAL_DELETE_DIRECTLY))
                {
                    systemOption.NursingSpecialDeleteDirectly = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.NUR_SHIFT_ITEM_SORT))
                {
                    systemOption.ShiftItemSort = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.NUR_REC_MAX_PREVIEWPAGES))
                {
                    systemOption.MaxPreviewPages = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 100);
                }
                else if(configInfo.ConfigName.Equals(ServerData.ConfigKey.SYSTEM_OPTINT_SAVEASBUTTONVISIBLE))
                {
                    systemOption.SaveAsButtonVisible = configInfo.ConfigValue == "1";
                }
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取窗口名称在配置字典表中的配置选项
        /// </summary>
        /// <param name="windowName">配置选项列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetWindowNames(ref WindowName windowName)
        {
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.GetConfigData(ServerData.ConfigKey.WINDOW_NAME, null, ref lstConfigInfos);

            windowName = new WindowName();
            if (lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return shRet;

            for (int index = 0; index < lstConfigInfos.Count; index++)
            {
                ConfigInfo configInfo = lstConfigInfos[index];
                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigName))
                    continue;

                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigValue))
                    configInfo.ConfigValue = string.Empty;

                configInfo.ConfigName = configInfo.ConfigName.Trim().ToUpper();

                if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_BATCH_RECORD))
                {
                    windowName.BatchRecord = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_BED_VIEW))
                {
                    windowName.BedView = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURDOC_LIST1))
                {
                    windowName.DocumentList1 = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURDOC_LIST2))
                {
                    windowName.DocumentList2 = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURDOC_LIST3))
                {
                    windowName.DocumentList3 = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURDOC_LIST4))
                {
                    windowName.DocumentList4 = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_SPECIAL_NURSING))
                {
                    windowName.SpecialNursing = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_WORKSHIFT_RECORD))
                {
                    windowName.WorkShiftRecord = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_WORD_DOCUMENT))
                {
                    windowName.WordDocument = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_VITAL_SIGNS_GRAPH))
                {
                    windowName.VitalSignsGraph = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_ORDERS_RECORD))
                {
                    windowName.OrdersRecord = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURSING_RECORD))
                {
                    windowName.NursingRecord = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_SPECIAL_PATIENT))
                {
                    windowName.SpecialPatient = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURSING_CONSULT))
                {
                    windowName.NursingConsult = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NUR_CARE_PLAN))
                {
                    windowName.NurCarePlan = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURSING_TASK))
                {
                    windowName.NursingTask = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURSING_STAT))
                {
                    windowName.NursingStat = configInfo.ConfigValue;
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_INTEGRATE_QUERY))
                {
                    windowName.IntegrateQuery = configInfo.ConfigValue;
                }

                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.WINDOW_NAME_NURSING_MONITOR))
                {
                    windowName.NursingMonitor = configInfo.ConfigValue;
                }
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 获取用户权限在配置字典表中的配置选项
        /// </summary>
        /// <param name="rightConfig">配置选项列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetRightConfig(ref RightConfig rightConfig)
        {
            List<ConfigInfo> lstConfigInfos = null;
            short shRet = this.GetConfigData(ServerData.ConfigKey.USER_RIGHT, null, ref lstConfigInfos);

            rightConfig = new RightConfig();
            if (lstConfigInfos == null || lstConfigInfos.Count <= 0)
                return shRet;

            for (int index = 0; index < lstConfigInfos.Count; index++)
            {
                ConfigInfo configInfo = lstConfigInfos[index];
                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigName))
                    continue;

                if (GlobalMethods.Misc.IsEmptyString(configInfo.ConfigValue))
                    configInfo.ConfigValue = string.Empty;

                configInfo.ConfigName = configInfo.ConfigName.Trim().ToUpper();

                if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_APPROVEL_DATA_MODIFY))
                {
                    rightConfig.ApprovelDataModify = configInfo.ConfigValue == "1";
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_STUDENT_NURSE_EDIT_TIME))
                {
                    rightConfig.StudentNurseEditTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 24);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_GENERAL_NURSE_EDIT_TIME))
                {
                    rightConfig.GeneralNurseEditTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 24);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_QUALITY_NURSE_EDIT_TIME))
                {
                    rightConfig.QualityNurseEditTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 168);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_HIGHER_NURSE_EDIT_TIME))
                {
                    rightConfig.HigherNurseEditTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 168);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_HEAD_NURSE_EDIT_TIME))
                {
                    rightConfig.HeadNurseEditTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 168);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_LEADER_NURSE_EDIT_TIME))
                {
                    rightConfig.LeaderNurseEditTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 2160);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_STUDENT_NURSE_GRANT_TIME))
                {
                    rightConfig.StudentNurseGrantTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 0);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_GENERAL_NURSE_GRANT_TIME))
                {
                    rightConfig.GeneralNurseGrantTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 0);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_QUALITY_NURSE_GRANT_TIME))
                {
                    rightConfig.QualityNurseGrantTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 168);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_HIGHER_NURSE_GRANT_TIME))
                {
                    rightConfig.HigherNurseGrantTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 168);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_HEAD_NURSE_GRANT_TIME))
                {
                    rightConfig.HeadNurseGrantTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 168);
                }
                else if (configInfo.ConfigName.Equals(ServerData.ConfigKey.USER_RIGHT_LEADER_NURSE_GRANT_TIME))
                {
                    rightConfig.LeaderNurseGrantTime = GlobalMethods.Convert.StringToValue(configInfo.ConfigValue, 2160);
                }

            }
            return ServerData.ExecuteResult.OK;
        }
        #endregion

        #region "表格视图方案和列维护"
        /// <summary>
        /// 获取指定类型指定ID号的表格模板列表
        /// </summary>
        /// <param name="szSchemaType">方案类型</param>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstGridViewSchemas">表格模板列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetGridViewSchemas(string szSchemaType, string szWardCode, ref List<GridViewSchema> lstGridViewSchemas)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstGridViewSchemas == null)
                lstGridViewSchemas = new List<GridViewSchema>();
            lstGridViewSchemas.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.GridViewSchemaTable.SCHEMA_ID, ServerData.GridViewSchemaTable.SCHEMA_TYPE
                , ServerData.GridViewSchemaTable.SCHEMA_NAME
                , ServerData.GridViewSchemaTable.WARD_CODE, ServerData.GridViewSchemaTable.WARD_NAME
                , ServerData.GridViewSchemaTable.IS_DEFAULT
                , ServerData.GridViewSchemaTable.CREATOR_ID, ServerData.GridViewSchemaTable.CREATOR_NAME
                , ServerData.GridViewSchemaTable.CREATE_TIME, ServerData.GridViewSchemaTable.MODIFIER_ID
                , ServerData.GridViewSchemaTable.MODIFIER_NAME, ServerData.GridViewSchemaTable.MODIFY_TIME
                , ServerData.GridViewSchemaTable.SCHEMA_FLAG, ServerData.GridViewSchemaTable.RELATIVE_SCHEMA_ID);
            string szTable = ServerData.DataTable.GRID_VIEW_SCHEMA;
            string szCondition = string.Format("{0}='{1}'", ServerData.GridViewSchemaTable.SCHEMA_TYPE, szSchemaType);
            if (!GlobalMethods.Misc.IsEmptyString(szWardCode))
            {
                szCondition += string.Format(" AND ({0}='{1}' OR {0}='' OR {0} IS NULL)"
                    , ServerData.GridViewSchemaTable.WARD_CODE, szWardCode);
            }
            string szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, szTable, szCondition);

            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    GridViewSchema gridViewSchema = new GridViewSchema();
                    if (!dataReader.IsDBNull(0)) gridViewSchema.SchemaID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) gridViewSchema.SchemaType = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) gridViewSchema.SchemaName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) gridViewSchema.WardCode = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) gridViewSchema.WardName = dataReader.GetString(4);
                    if (!dataReader.IsDBNull(5)) gridViewSchema.IsDefault = dataReader.GetValue(5).ToString() == "1";
                    if (!dataReader.IsDBNull(6)) gridViewSchema.CreateID = dataReader.GetString(6);
                    if (!dataReader.IsDBNull(7)) gridViewSchema.CreateName = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) gridViewSchema.CreateTime = dataReader.GetDateTime(8);
                    if (!dataReader.IsDBNull(9)) gridViewSchema.ModiferID = dataReader.GetString(9);
                    if (!dataReader.IsDBNull(10)) gridViewSchema.ModiferName = dataReader.GetString(10);
                    if (!dataReader.IsDBNull(11)) gridViewSchema.ModifyTime = dataReader.GetDateTime(11);
                    if (!dataReader.IsDBNull(12)) gridViewSchema.SchemaFlag = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) gridViewSchema.RelatvieSchemaId = dataReader.GetString(13);
                    lstGridViewSchemas.Add(gridViewSchema);
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
        /// 保存指定的表格模板信息
        /// </summary>
        /// <param name="gridViewSchema">表格模板信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveGridViewSchema(GridViewSchema gridViewSchema)
        {
            if (gridViewSchema == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.GridViewSchemaTable.SCHEMA_ID, ServerData.GridViewSchemaTable.SCHEMA_TYPE
                , ServerData.GridViewSchemaTable.SCHEMA_NAME
                , ServerData.GridViewSchemaTable.WARD_CODE, ServerData.GridViewSchemaTable.WARD_NAME
                , ServerData.GridViewSchemaTable.IS_DEFAULT
                , ServerData.GridViewSchemaTable.CREATOR_ID, ServerData.GridViewSchemaTable.CREATOR_NAME
                , ServerData.GridViewSchemaTable.CREATE_TIME, ServerData.GridViewSchemaTable.MODIFIER_ID
                , ServerData.GridViewSchemaTable.MODIFIER_NAME, ServerData.GridViewSchemaTable.MODIFY_TIME
                , ServerData.GridViewSchemaTable.SCHEMA_FLAG, ServerData.GridViewSchemaTable.RELATIVE_SCHEMA_ID);
            string szValue = string.Format("'{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}',{8},'{9}','{10}',{11},'{12}','{13}'"
                , gridViewSchema.SchemaID, gridViewSchema.SchemaType, gridViewSchema.SchemaName
                , gridViewSchema.WardCode, gridViewSchema.WardName, gridViewSchema.IsDefault ? "1" : "0"
                , gridViewSchema.CreateID, gridViewSchema.CreateName
                , base.DataAccess.GetSqlTimeFormat(gridViewSchema.CreateTime)
                , gridViewSchema.ModiferID, gridViewSchema.ModiferName
                , base.DataAccess.GetSqlTimeFormat(gridViewSchema.ModifyTime), gridViewSchema.SchemaFlag
                , gridViewSchema.RelatvieSchemaId);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.GRID_VIEW_SCHEMA, szField, szValue);

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
        /// 更新指定的表格模板信息
        /// </summary>
        /// <param name="szSchemaID">模板ID号</param>
        /// <param name="gridViewSchema">表格模板信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateGridViewSchema(string szSchemaID, GridViewSchema gridViewSchema)
        {
            if (GlobalMethods.Misc.IsEmptyString(szSchemaID) || gridViewSchema == null)
            {
                LogManager.Instance.WriteLog("ConfigAccess.UpdateGridViewSchema", new string[] { "gridViewSchema" },
                    new object[] { gridViewSchema }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0}='{1}',{2}='{3}',{4}='{5}',{6}='{7}',{8}='{9}',{10}={11},{12}='{13}',{14}='{15}'"
                , ServerData.GridViewSchemaTable.SCHEMA_NAME, gridViewSchema.SchemaName
                , ServerData.GridViewSchemaTable.WARD_CODE, gridViewSchema.WardCode
                , ServerData.GridViewSchemaTable.WARD_NAME, gridViewSchema.WardName
                , ServerData.GridViewSchemaTable.MODIFIER_ID, gridViewSchema.ModiferID
                , ServerData.GridViewSchemaTable.MODIFIER_NAME, gridViewSchema.ModiferName
                , ServerData.GridViewSchemaTable.MODIFY_TIME, base.DataAccess.GetSqlTimeFormat(gridViewSchema.ModifyTime)
                , ServerData.GridViewSchemaTable.SCHEMA_FLAG, gridViewSchema.SchemaFlag
                , ServerData.GridViewSchemaTable.RELATIVE_SCHEMA_ID, gridViewSchema.RelatvieSchemaId);
            string szCondition = string.Format("{0}='{1}'", ServerData.GridViewSchemaTable.SCHEMA_ID, szSchemaID);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.GRID_VIEW_SCHEMA, szField, szCondition);

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
                LogManager.Instance.WriteLog("ConfigAccess.UpdateGridViewSchema", new string[] { "szSQL" }, new object[] { szSQL }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.ACCESS_ERROR;
            }
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 设置病区的批量录入默认方案
        /// </summary>
        /// <param name="szSchemaID">模板ID</param>
        /// <param name="szSchemaType">模板类别</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SetDefaultGridViewSchema(string szSchemaID, string szSchemaType, string szWardCode)
        {
            if (GlobalMethods.Misc.IsEmptyString(szWardCode)
                || GlobalMethods.Misc.IsEmptyString(szSchemaType) || GlobalMethods.Misc.IsEmptyString(szSchemaID))
            {
                LogManager.Instance.WriteLog("ConfigAccess.SetDefaultGridViewSchema"
                    , new string[] { "szWardCode", "szSchemaType", "szSchemaID" }
                    , new object[] { szWardCode, szSchemaType, szSchemaID }, "参数不能为空");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            //取消掉之前设置的默认方案
            string szField = string.Format("{0}={1}", ServerData.GridViewSchemaTable.IS_DEFAULT, "0");
            string szCondition = string.Format("{0}='{1}' AND {2}='{3}'"
                , ServerData.GridViewSchemaTable.WARD_CODE, szWardCode, ServerData.GridViewSchemaTable.SCHEMA_TYPE, szSchemaType);
            string szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.GRID_VIEW_SCHEMA, szField, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }

            //将当前指定的方案设置为默认方案
            szField = string.Format("{0}={1}", ServerData.GridViewSchemaTable.IS_DEFAULT, "1");
            szCondition = string.Format("{0}='{1}' AND {2}='{3}' "
                , ServerData.GridViewSchemaTable.SCHEMA_ID, szSchemaID, ServerData.GridViewSchemaTable.SCHEMA_TYPE, szSchemaType);
            szSQL = string.Format(ServerData.SQL.UPDATE, ServerData.DataTable.GRID_VIEW_SCHEMA, szField, szCondition);
            try
            {
                base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                base.DataAccess.AbortTransaction();
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            base.DataAccess.CommitTransaction(true);
            return ServerData.ExecuteResult.OK;
        }

        /// <summary>
        /// 删除指定ID号对应的表格模板
        /// </summary>
        /// <param name="szSchemaID">模板ID</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteGridViewSchema(string szSchemaID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szSchemaID))
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szCondition = string.Format("{0}='{1}'", ServerData.GridViewSchemaTable.SCHEMA_ID, szSchemaID);
            string szSQL = string.Format(ServerData.SQL.DELETE, ServerData.DataTable.GRID_VIEW_SCHEMA, szCondition);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            if (nCount > 0)
                this.DeleteGridViewColumns(szSchemaID);
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }

        /// <summary>
        /// 获取表格模板ID号对应的所有表格列
        /// </summary>
        /// <param name="szSchemaID">模板ID</param>
        /// <param name="lstGridViewColumns">模板列列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetGridViewColumns(string szSchemaID, ref List<GridViewColumn> lstGridViewColumns)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (lstGridViewColumns == null)
                lstGridViewColumns = new List<GridViewColumn>();
            lstGridViewColumns.Clear();

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.GridViewColumnTable.SCHEMA_ID, ServerData.GridViewColumnTable.COLUMN_ID
                , ServerData.GridViewColumnTable.COLUMN_NAME, ServerData.GridViewColumnTable.COLUMN_TAG
                , ServerData.GridViewColumnTable.COLUMN_INDEX, ServerData.GridViewColumnTable.COLUMN_TYPE
                , ServerData.GridViewColumnTable.COLUMN_WIDTH, ServerData.GridViewColumnTable.COLUMN_UNIT
                , ServerData.GridViewColumnTable.IS_VISIBLE, ServerData.GridViewColumnTable.IS_PRINT
                , ServerData.GridViewColumnTable.IS_MIDDLE, ServerData.GridViewColumnTable.COLUMN_ITEMS
                , ServerData.GridViewColumnTable.DOCTYPE_ID, ServerData.GridViewColumnTable.COLUMN_GROUP);

            string szSQL = null;
            if (GlobalMethods.Misc.IsEmptyString(szSchemaID))
            {
                szSQL = string.Format(ServerData.SQL.SELECT_ORDER_ASC
                    , szField, ServerData.DataTable.GRID_VIEW_COLUMN, ServerData.GridViewColumnTable.COLUMN_INDEX);
            }
            else
            {
                string szCondition = string.Format("{0}='{1}'", ServerData.GridViewColumnTable.SCHEMA_ID, szSchemaID);
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE_ORDER_ASC
                    , szField, ServerData.DataTable.GRID_VIEW_COLUMN, szCondition, ServerData.GridViewColumnTable.COLUMN_INDEX);
            }
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                do
                {
                    GridViewColumn gridViewColumn = new GridViewColumn();
                    if (!dataReader.IsDBNull(0)) gridViewColumn.SchemaID = dataReader.GetString(0);
                    if (!dataReader.IsDBNull(1)) gridViewColumn.ColumnID = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2)) gridViewColumn.ColumnName = dataReader.GetString(2);
                    if (!dataReader.IsDBNull(3)) gridViewColumn.ColumnTag = dataReader.GetString(3);
                    if (!dataReader.IsDBNull(4)) gridViewColumn.ColumnIndex = int.Parse(dataReader.GetValue(4).ToString());
                    if (!dataReader.IsDBNull(5)) gridViewColumn.ColumnType = dataReader.GetString(5);
                    if (!dataReader.IsDBNull(6)) gridViewColumn.ColumnWidth = int.Parse(dataReader.GetValue(6).ToString());
                    if (!dataReader.IsDBNull(7)) gridViewColumn.ColumnUnit = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8)) gridViewColumn.IsVisible = dataReader.GetValue(8).ToString() == "1";
                    if (!dataReader.IsDBNull(9)) gridViewColumn.IsPrint = dataReader.GetValue(9).ToString() == "1";
                    if (!dataReader.IsDBNull(10)) gridViewColumn.IsMiddle = dataReader.GetValue(10).ToString() == "1";
                    if (!dataReader.IsDBNull(11)) gridViewColumn.ColumnItems = dataReader.GetString(11);
                    if (!dataReader.IsDBNull(12)) gridViewColumn.DocTypeID = dataReader.GetString(12);
                    if (!dataReader.IsDBNull(13)) gridViewColumn.ColumnGroup = dataReader.GetString(13);
                    lstGridViewColumns.Add(gridViewColumn);
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
        /// 保存指定的一系列表格列列表
        /// </summary>
        /// <param name="szSchemaID">表格模板ID</param>
        /// <param name="lstGridViewColumns">模板包含列列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveGridViewColumns(string szSchemaID, List<GridViewColumn> lstGridViewColumns)
        {
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
                return ServerData.ExecuteResult.EXCEPTION;

            short shRet = this.DeleteGridViewColumns(szSchemaID);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }

            if (lstGridViewColumns == null)
                lstGridViewColumns = new List<GridViewColumn>();

            foreach (GridViewColumn gridViewColumn in lstGridViewColumns)
            {
                if (gridViewColumn == null)
                    continue;
                shRet = this.SaveGridViewColumn(gridViewColumn);
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
        /// 保存指定的表格模板中的一列
        /// </summary>
        /// <param name="gridViewColumn">表格模板列信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveGridViewColumn(GridViewColumn gridViewColumn)
        {
            if (gridViewColumn == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                , ServerData.GridViewColumnTable.SCHEMA_ID, ServerData.GridViewColumnTable.COLUMN_ID
                , ServerData.GridViewColumnTable.COLUMN_NAME, ServerData.GridViewColumnTable.COLUMN_TAG
                , ServerData.GridViewColumnTable.COLUMN_INDEX, ServerData.GridViewColumnTable.COLUMN_TYPE
                , ServerData.GridViewColumnTable.COLUMN_WIDTH, ServerData.GridViewColumnTable.COLUMN_UNIT
                , ServerData.GridViewColumnTable.IS_VISIBLE, ServerData.GridViewColumnTable.IS_PRINT
                , ServerData.GridViewColumnTable.IS_MIDDLE, ServerData.GridViewColumnTable.COLUMN_ITEMS
                , ServerData.GridViewColumnTable.DOCTYPE_ID, ServerData.GridViewColumnTable.COLUMN_GROUP);
            string szValue = string.Format("'{0}','{1}','{2}','{3}',{4},'{5}',{6},'{7}',{8},{9},{10},{11},'{12}','{13}'"
                , gridViewColumn.SchemaID, gridViewColumn.ColumnID, gridViewColumn.ColumnName
                , gridViewColumn.ColumnTag, gridViewColumn.ColumnIndex
                , gridViewColumn.ColumnType, gridViewColumn.ColumnWidth, gridViewColumn.ColumnUnit
                , gridViewColumn.IsVisible ? "1" : "0", gridViewColumn.IsPrint ? "1" : "0", gridViewColumn.IsMiddle ? "1" : "0"
                , base.DataAccess.GetSqlParamName("ColumnItems"), gridViewColumn.DocTypeID
                , gridViewColumn.ColumnGroup);
            string szSQL = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.GRID_VIEW_COLUMN, szField, szValue);

            DbParameter[] pmi = new DbParameter[1];
            pmi[0] = new DbParameter("ColumnItems", gridViewColumn.ColumnItems);

            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text, ref pmi);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return nCount > 0 ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.EXCEPTION;
        }

        /// <summary>
        /// 删除指定表格模板包含的所有列
        /// </summary>
        /// <param name="szSchemaID">表格模板ID</param>
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
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.RES_NO_FOUND;
        }
        #endregion
    }
}
