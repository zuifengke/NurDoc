// ***********************************************************
// 护理电子病历系统,数据访问层接口封装之公用数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Heren.NurDoc.DAL;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;

namespace Heren.NurDoc.Data
{
    public class CommonService
    {
        private static CommonService m_instance = null;

        /// <summary>
        /// 获取系统公共数据服务实例
        /// </summary>
        public static CommonService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new CommonService();
                return m_instance;
            }
        }

        private CommonService()
        {
        }

        /// <summary>
        /// 获取数据库服务器时间
        /// </summary>
        /// <param name="dtSysDate">数据库服务器时间</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetServerTime(ref DateTime dtSysDate)
        {
            return SystemContext.Instance.CommonAccess.GetServerTime(ref dtSysDate);
        }

        /// <summary>
        /// 执行指定的SQL语句查询
        /// </summary>
        /// <param name="sql">查询的SQL语句</param>
        /// <param name="result">查询返回的结果集</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short ExecuteQuery(string sql, out DataSet result)
        {
            return SystemContext.Instance.CommonAccess.ExecuteQuery(sql, out result);
        }

        /// <summary>
        /// 执行指定的SQL语句查询,已绑定变量的方式
        /// </summary>
        /// <param name="sqlInfo">查询的SqlInfo对象</param>
        /// <param name="result">查询返回的结果集</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short ExecuteQuery(SqlInfo sqlInfo, out DataSet result)
        {
            return SystemContext.Instance.CommonAccess.ExecuteQuery(sqlInfo, out result);
        }

        /// <summary>
        /// 执行指定的一系列的SQL语句更新
        /// </summary>
        /// <param name="isProc">传入的SQL是否是存储过程</param>
        /// <param name="sqlarray">查询的SQL语句集合</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short ExecuteUpdate(bool isProc, params string[] sqlarray)
        {
            return SystemContext.Instance.CommonAccess.ExecuteUpdate(isProc, sqlarray);
        }

        /// <summary>
        /// 获取全院所有科室信息
        /// </summary>
        /// <param name="lstDeptInfos">科室信息列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetAllDeptInfos(ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetAllDeptInfos(ref lstDeptInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取病区类型的科室列表
        /// </summary>
        /// <param name="lstDeptInfos">科室列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetWardDeptList(ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetWardDeptList(ref lstDeptInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取门诊类型的科室列表
        /// </summary>
        /// <param name="lstDeptInfos">科室列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetOutPDeptList(ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetOutPDeptList(ref lstDeptInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取护理单元类型的科室列表
        /// </summary>
        /// <param name="lstDeptInfos">科室列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetNurseDeptList(ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetNurseDeptList(ref lstDeptInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取临床类型的科室列表
        /// </summary>
        /// <param name="lstDeptInfos">科室列表</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetClinicDeptList(ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetClinicDeptList(ref lstDeptInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取用户组属性的科室列表
        /// </summary>
        /// <param name="lstDeptInfos">返回的科室列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetUserGroupList(ref List<DeptInfo> lstDeptInfos)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetUserGroupList(ref lstDeptInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 查询公共字典表,获取公共字典表中存储的字典类型列表
        /// </summary>
        /// <param name="lstDictTypeList">字典类型列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetCommonDictTypeList(ref List<string> lstDictTypeList)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetCommonDictTypeList(ref lstDictTypeList);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 查询公共字典表,获取指定字典类型的字典数据
        /// </summary>
        /// <param name="szItemType">字典类型</param>
        /// <param name="lstCommonDictItems">字典项目数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetCommonDict(string szItemType, ref List<CommonDictItem> lstCommonDictItems)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetCommonDict(szItemType, ref lstCommonDictItems);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 查询公共字典表,获取指定字典类型的字典数据
        /// </summary>
        /// <param name="szItemType">字典类型</param>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstCommonDictItems">字典项目数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetCommonDict(string szItemType, string szWardCode, ref List<CommonDictItem> lstCommonDictItems)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetCommonDict(szItemType, szWardCode, ref lstCommonDictItems);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 从公共字典表中删除指定的一条字典数据
        /// </summary>
        /// <param name="szItemType">字典类型</param>
        /// <param name="szItemCode">项目代码</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteCommonDictItem(string szItemType, string szItemCode)
        {
            short shRet = SystemContext.Instance.CommonAccess.DeleteCommonDictItem(szItemType, szItemCode);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 从公共字典表中删除指定的一条字典数据
        /// </summary>
        /// <param name="szItemType">字典类型</param>
        /// <param name="szItemCode">项目代码</param>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short DeleteCommonDictItem(string szItemType, string szItemCode, string szWardCode)
        {
            short shRet = SystemContext.Instance.CommonAccess.DeleteCommonDictItem(szItemType, szItemCode, szWardCode);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存新的一条字典数据到公共字典表
        /// </summary>
        /// <param name="commonDictItem">字典数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveCommonDictItem(CommonDictItem commonDictItem)
        {
            short shRet = SystemContext.Instance.CommonAccess.SaveCommonDictItem(commonDictItem);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新公共字典表中指定的一条字典数据
        /// </summary>
        /// <param name="szItemType">字典类型</param>
        /// <param name="szItemCode">字典项目代码</param>
        /// <param name="commonDictItem">新的字典数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateCommonDictItem(string szItemType, string szItemCode, CommonDictItem commonDictItem)
        {
            short shRet = SystemContext.Instance.CommonAccess.UpdateCommonDictItem(szItemType, szItemCode, commonDictItem);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新公共字典表中指定的一条字典数据
        /// </summary>
        /// <param name="szItemType">字典类型</param>
        /// <param name="szItemCode">字典项目代码</param>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="commonDictItem">新的字典数据</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short UpdateCommonDictItem(string szItemType, string szItemCode, string szWardCode, CommonDictItem commonDictItem)
        {
            short shRet = SystemContext.Instance.CommonAccess.UpdateCommonDictItem(szItemType, szItemCode, szWardCode, commonDictItem);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 病历质控系统,获取病案质量问题分类信息字典列表
        /// </summary>
        /// <param name="lstQCEventTypes">病案质量问题分类信息字典列表</param>
        /// <returns>MedDocSys.Common.SystemData.ReturnValue</returns>
        public short GetQCEventTypeList(ref List<QCEventType> lstQCEventTypes)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetQCEventTypeList(ref  lstQCEventTypes);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
            {
                return SystemConst.ReturnValue.FAILED;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 根据问题类型获取质控质检信息字典列表
        /// </summary>
        /// <param name="szQuestionType">问题类型</param>
        /// <param name="lstQCMessageTemplets">质控质检信息字典列表</param>
        /// <returns>MedDocSys.Common.SystemData.ReturnValue</returns>
        public short GetQCMessageTempletList(string szQuestionType, ref List<QCMessageTemplet> lstQCMessageTemplets)
        {
            short shRet = SystemContext.Instance.CommonAccess.GetQCMessageTempletList(szQuestionType, ref  lstQCMessageTemplets);
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