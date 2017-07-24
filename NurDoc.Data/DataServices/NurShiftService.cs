// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之护士交班数据访问接口封装类.
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
    public class NurShiftService
    {
        private static NurShiftService m_instance = null;

        /// <summary>
        /// 获取护士护理工作相关服务实例
        /// </summary>
        public static NurShiftService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NurShiftService();
                return m_instance;
            }
        }

        private NurShiftService()
        {
        }

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
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftRankInfos(szWardCode, lstDeptInfos, ref lstShiftRankInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的交班班次信息
        /// </summary>
        /// <param name="shiftRankInfo">交班班次信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftRankInfo(ShiftRankInfo shiftRankInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveShiftRankInfo(shiftRankInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftRankInfo(szRankCode, szWardCode, shiftRankInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除指定的交班班次信息
        /// </summary>
        /// <param name="szRankCode">交班班次代码</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftRankInfo(string szRankCode, string szWardCode)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftRankInfo(szRankCode, szWardCode);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
        #endregion

        #region "交班字典数据管理"
        /// <summary>
        /// 获取指定交班动态配置信息
        /// </summary>
        /// <param name="szWardCodes">病区代码组</param>
        /// <param name="lstShiftConfigInfos">交班动态配置列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftConfigInfos(string[] szWardCodes, ref List<ShiftConfigInfo> lstShiftConfigInfos)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftConfigInfos(szWardCodes, ref lstShiftConfigInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的交班动态配置信息
        /// </summary>
        /// <param name="shiftConfigInfo">交班动态配置信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftConfigInfo(ShiftConfigInfo shiftConfigInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveShiftConfigInfo(shiftConfigInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftConfigInfo(szItemCode, szWardCode, shiftConfigInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除指定的交班动态配置信息
        /// </summary>
        /// <param name="szItemCode">交班动态配置代码</param>
        /// <param name="szWardCode">所属病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftConfigInfo(string szItemCode, string szWardCode)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftConfigInfo(szItemCode, szWardCode);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
        #endregion

        /// <summary>
        /// 保存指定的交班病区状况信息
        /// </summary>
        /// <param name="shiftWardStatus">病区状况信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftWardStatus(ShiftWardStatus shiftWardStatus)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveShiftWardStatus(shiftWardStatus);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新交班病区状况信息
        /// </summary>
        /// <param name="shiftWardStatus">病区状况信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftWardStatus(ShiftWardStatus shiftWardStatus)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftWardStatus(shiftWardStatus);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定的交班记录对应的病区状况信息列表
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="lstShiftWardStatus">病区状况信息列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftWardStatusList(string szShiftRecordID, ref List<ShiftWardStatus> lstShiftWardStatus)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftWardStatusList(szShiftRecordID, ref lstShiftWardStatus);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftWardStatusList(szWardCode, dtShiftDate, ref lstShiftWardStatus);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的交班病人信息
        /// </summary>
        /// <param name="shiftPatient">交班病人信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftPatient(ShiftPatient shiftPatient)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveShiftPatient(shiftPatient);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新交班病人信息
        /// </summary>
        /// <param name="shiftPatient">交班病人信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftPatient(ShiftPatient shiftPatient)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftPatient(shiftPatient);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftPatient(szShiftRecordID
                , szPatientID, szVisitID, szSubID);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定的交班记录对应的交班病人列表
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="lstShiftPatients">交班病人列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftPatientList(string szShiftRecordID, ref List<ShiftPatient> lstShiftPatients)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftPatientList(szShiftRecordID, ref lstShiftPatients);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftPatientList(szWardCode, dtShiftDate, ref lstShiftPatients);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的交班主记录
        /// </summary>
        /// <param name="nursingShiftInfo">交班记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNursingShiftInfo(NursingShiftInfo nursingShiftInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveNursingShiftInfo(nursingShiftInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新交班主记录信息
        /// </summary>
        /// <param name="nursingShiftInfo">交班主记录信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateNursingShiftInfo(NursingShiftInfo nursingShiftInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.UpdateNursingShiftInfo(nursingShiftInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定病区指定交班日期及班次对应的交班主记录
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="szRankCode">交班班次</param>
        /// <param name="nursingShiftInfo">交班主记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetNursingShiftInfo(string szWardCode, DateTime dtShiftDate, string szRankCode
            , ref NursingShiftInfo nursingShiftInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.GetNursingShiftInfo(szWardCode, dtShiftDate
                , szRankCode, ref nursingShiftInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return ServerData.ExecuteResult.RES_NO_FOUND;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.GetNursingShiftInfos(szWardCode, dtShiftDate, ref lstNursingShiftInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定病区的所有交班项目配置别名
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="lstShiftItemAliasInfos">交班项目配置别名列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftItemAliasInfos(string szWardCode, ref List<ShiftItemAliasInfo> lstShiftItemAliasInfos)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftItemAliasInfos(szWardCode, ref lstShiftItemAliasInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }


        /// <summary>
        /// 删除指定病区的指定交班项目配置别名
        /// </summary>
        /// <param name="szItemAliasCode">交班项目配置别名代码</param>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftItemAlias(string szItemAliasCode, string szWardCode)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftItemAlias(szItemAliasCode, szWardCode);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 更新指定病区的指定交班项目配置别名
        /// </summary>
        /// <param name="szItemAliasCode">需要更新的交班项目别名</param>
        /// <param name="szItemAlias">交班项目别名</param>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateShiftItemAlias(string szItemAliasCode, string szItemAlias, string szItemName, string szWardCode)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftItemAlias(szItemAliasCode, szItemAlias, szItemName, szWardCode);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定病区的指定交班项目配置别名
        /// </summary>
        /// <param name="shiftItemAliasInfo">交班项目配置</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftItemAlias(ShiftItemAliasInfo shiftItemAliasInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveShiftItemAlias(shiftItemAliasInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        #region "特殊病人病情交接班"
        /// <summary>
        /// 获取指定病区指定交班日期对应的特殊病人病情交班主记录
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <param name="dtShiftDate">交班日期</param>
        /// <param name="specialShiftInfo">特殊病人病情交接班主记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetSpecialShiftInfo(string szWardCode, DateTime dtShiftDate, ref SpecialShiftInfo specialShiftInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.GetSpecialShiftInfo(szWardCode, dtShiftDate
                , ref specialShiftInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return ServerData.ExecuteResult.RES_NO_FOUND;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 保存指定的特殊病人病情交班主记录
        /// </summary>
        /// <param name="specialShiftInfo">特殊病人病情交接班主记录</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveSpecialShiftInfo(SpecialShiftInfo specialShiftInfo)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveSpecialShiftInfo(specialShiftInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

         /// <summary>
        /// 保存指定的交班病人信息
        /// </summary>
        /// <param name="shiftSpecialPatient">交班病人信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveShiftSpecialPatient(ShiftSpecialPatient shiftSpecialPatient)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.SaveShiftSpecialPatient(shiftSpecialPatient);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取指定的交班记录对应的特殊病人交班列表
        /// </summary>
        /// <param name="szShiftRecordID">交班记录ID</param>
        /// <param name="lstShiftSpecialPatients">特殊病人交班列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetShiftSpecialPatientList(string szShiftRecordID, ref List<ShiftSpecialPatient> lstShiftSpecialPatients)
        {
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftSpecialPatientList(szShiftRecordID, ref lstShiftSpecialPatients);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.GetShiftSpecialPatientList(szWardCode, dtShiftDate, ref lstShiftSpecialPatients);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
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
            short shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftSpecialPatient(szShiftRecordID
                , szPatientID, szVisitID, szSubID);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        #endregion
    }
}