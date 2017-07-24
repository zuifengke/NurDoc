// ***********************************************************
// 护理电子病历系统,数据访问层接口封装之配置数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-25
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class ConfigService
    {
        private static ConfigService m_instance = null;

        /// <summary>
        /// 获取系统公共数据服务实例
        /// </summary>
        public static ConfigService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ConfigService();
                return m_instance;
            }
        }

        private ConfigService()
        {
        }

        #region"后台配置数据访问"
        /// <summary>
        /// 查询配置字典表获取指定的配置数据
        /// </summary>
        /// <param name="szGroupName">配置组名称</param>
        /// <param name="szConfigName">配置项名称(为空时返回该组所有配置项)</param>
        /// <param name="lstConfigInfos">返回的配置项及其配置数据</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short GetConfigData(string szGroupName, string szConfigName, ref List<ConfigInfo> lstConfigInfos)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.ConfigAccess.GetConfigData(szGroupName, szConfigName, ref lstConfigInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != SystemConst.ReturnValue.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存配置字典表中新的配置数据
        /// </summary>
        /// <param name="configInfo">配置项及其配置数据</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short SaveConfigData(ConfigInfo configInfo)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.ConfigAccess.SaveConfigData(configInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 修改配置字典表中指定的配置数据
        /// </summary>
        /// <param name="szGroupName">配置组</param>
        /// <param name="szConfigName">配置项</param>
        /// <param name="configInfo">配置项及其配置数据</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short UpdateConfigData(string szGroupName, string szConfigName, ConfigInfo configInfo)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.ConfigAccess.UpdateConfigData(szGroupName, szConfigName, configInfo);
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除配置字典表中指定的配置数据
        /// </summary>
        /// <param name="szGroupName">配置组</param>
        /// <param name="szConfigName">配置项</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short DeleteConfigData(string szGroupName, string szConfigName)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.ConfigAccess.DeleteConfigData(szGroupName, szConfigName);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取窗口名称在配置字典表中的配置选项
        /// </summary>
        /// <param name="windowName">配置选项列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetWindowNames(ref WindowName windowName)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.ConfigAccess.GetWindowNames(ref windowName);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取用户权限在配置字典表中的配置选项
        /// </summary>
        /// <param name="rightConfig">配置选项列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetRightConfig(ref RightConfig rightConfig)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.ConfigAccess.GetRightConfig(ref rightConfig);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取病历文档系统在配置字典表中的配置选项
        /// </summary>
        /// <param name="systemOption">配置选项列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetSystemOptions(ref SystemOption systemOption)
        {
            if (SystemContext.Instance.ConfigAccess == null)
                return SystemConst.ReturnValue.FAILED;

            short shRet = SystemContext.Instance.ConfigAccess.GetSystemOptions(ref systemOption);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }
        #endregion

        #region "表格视图方案和列维护"
        /// <summary>
        /// 获取指定类型指定ID号的表格模板列表
        /// </summary>
        /// <param name="szSchemaType">方案类型</param>
        /// <param name="szDeptCode">病区代码</param>
        /// <param name="lstGridViewSchemas">表格模板列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetGridViewSchemas(string szSchemaType, string szDeptCode, ref List<GridViewSchema> lstGridViewSchemas)
        {
            short shRet = SystemContext.Instance.ConfigAccess.GetGridViewSchemas(szSchemaType, szDeptCode, ref lstGridViewSchemas);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的表格模板信息
        /// </summary>
        /// <param name="gridViewSchema">表格模板信息</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveGridViewSchema(GridViewSchema gridViewSchema)
        {
            short shRet = SystemContext.Instance.ConfigAccess.SaveGridViewSchema(gridViewSchema);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新指定的表格模板信息
        /// </summary>
        /// <param name="szSchemaID">模板ID号</param>
        /// <param name="gridViewSchema">表格模板信息</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short UpdateGridViewSchema(string szSchemaID, GridViewSchema gridViewSchema)
        {
            short shRet = SystemContext.Instance.ConfigAccess.UpdateGridViewSchema(szSchemaID, gridViewSchema);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 设置病区的批量录入默认方案
        /// </summary>
        /// <param name="szSchemaID">模板ID</param>
        /// <param name="szSchemaType">模板类别</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SetDefaultGridViewSchema(string szSchemaID, string szSchemaType, string szWardCode)
        {
            short shRet = SystemContext.Instance.ConfigAccess.SetDefaultGridViewSchema(szSchemaID, szSchemaType, szWardCode);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除指定ID号对应的表格模板
        /// </summary>
        /// <param name="szSchemaID">模板ID</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteGridViewSchema(string szSchemaID)
        {
            short shRet = SystemContext.Instance.ConfigAccess.DeleteGridViewSchema(szSchemaID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取表格模板ID号对应的所有表格列
        /// </summary>
        /// <param name="szSchemaID">模板ID</param>
        /// <param name="lstGridViewColumns">模板列列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short GetGridViewColumns(string szSchemaID, ref List<GridViewColumn> lstGridViewColumns)
        {
            short shRet = SystemContext.Instance.ConfigAccess.GetGridViewColumns(szSchemaID, ref lstGridViewColumns);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的一系列表格列列表
        /// </summary>
        /// <param name="szSchemaID">表格模板ID</param>
        /// <param name="lstGridViewColumns">模板包含列列表</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveGridViewColumns(string szSchemaID, List<GridViewColumn> lstGridViewColumns)
        {
            short shRet = SystemContext.Instance.ConfigAccess.SaveGridViewColumns(szSchemaID, lstGridViewColumns);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的表格模板中的一列
        /// </summary>
        /// <param name="gridViewColumn">表格模板列信息</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short SaveGridViewColumn(GridViewColumn gridViewColumn)
        {
            short shRet = SystemContext.Instance.ConfigAccess.SaveGridViewColumn(gridViewColumn);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除指定表格模板包含的所有列
        /// </summary>
        /// <param name="szSchemaID">表格模板ID</param>
        /// <returns>SystemData.ExecuteResult</returns>
        public short DeleteGridViewColumns(string szSchemaID)
        {
            short shRet = SystemContext.Instance.ConfigAccess.DeleteGridViewColumns(szSchemaID);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
        #endregion
    }
}