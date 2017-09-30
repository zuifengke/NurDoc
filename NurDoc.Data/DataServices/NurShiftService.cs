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
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public class NurShiftService : DBAccessBase
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter(lstDeptInfos);
                shRet = RestHandler.Instance.Post<ShiftRankInfo>("NurShiftAccess/GetShiftRankInfos", ref lstShiftRankInfos);
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetShiftRankInfos(szWardCode, lstDeptInfos, ref lstShiftRankInfos);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(shiftRankInfo);
                shRet = RestHandler.Instance.Post("NurShiftAccess/SaveShiftRankInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.SaveShiftRankInfo(shiftRankInfo);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szRankCode", szRankCode);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter(shiftRankInfo);
                shRet = RestHandler.Instance.Post("NurShiftAccess/UpdateShiftRankInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftRankInfo(szRankCode, szWardCode, shiftRankInfo);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szRankCode", szRankCode);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                shRet = RestHandler.Instance.Put("NurShiftAccess/DeleteShiftRankInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftRankInfo(szRankCode, szWardCode);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                List<string> lstWardCodes = new List<string>();
                if (szWardCodes != null) { 
                for (int i = 0; i < szWardCodes.Length; i++)
                    lstWardCodes.Add(szWardCodes[i]);
                }
                RestHandler.Instance.AddParameter(lstWardCodes);
                shRet = RestHandler.Instance.Post<ShiftConfigInfo>("NurShiftAccess/GetShiftConfigInfos", ref lstShiftConfigInfos);
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetShiftConfigInfos(szWardCodes, ref lstShiftConfigInfos);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(shiftConfigInfo);
                shRet = RestHandler.Instance.Post("NurShiftAccess/SaveShiftConfigInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.SaveShiftConfigInfo(shiftConfigInfo);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szItemCode", szItemCode);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter(shiftConfigInfo);
                shRet = RestHandler.Instance.Post("NurShiftAccess/UpdateShiftConfigInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftConfigInfo(szItemCode, szWardCode, shiftConfigInfo);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szItemCode", szItemCode);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                shRet = RestHandler.Instance.Put("NurShiftAccess/DeleteShiftConfigInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftConfigInfo(szItemCode, szWardCode);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(shiftWardStatus);
                shRet = RestHandler.Instance.Post("NurShiftAccess/SaveShiftWardStatus");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.SaveShiftWardStatus(shiftWardStatus);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(shiftWardStatus);
                shRet = RestHandler.Instance.Post("NurShiftAccess/UpdateShiftWardStatus");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftWardStatus(shiftWardStatus);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szShiftRecordID", szShiftRecordID);
                shRet = RestHandler.Instance.Get<ShiftWardStatus>("NurShiftAccess/GetShiftWardStatusList1", ref lstShiftWardStatus);
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetShiftWardStatusList(szShiftRecordID, ref lstShiftWardStatus);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter("dtShiftDate", dtShiftDate);
                shRet = RestHandler.Instance.Get<ShiftWardStatus>("NurShiftAccess/GetShiftWardStatusList2", ref lstShiftWardStatus);
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetShiftWardStatusList(szWardCode, dtShiftDate, ref lstShiftWardStatus);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(shiftPatient);
                shRet = RestHandler.Instance.Post("NurShiftAccess/SaveShiftPatient");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.SaveShiftPatient(shiftPatient);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(shiftPatient);
                shRet = RestHandler.Instance.Post("NurShiftAccess/UpdateShiftPatient");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.UpdateShiftPatient(shiftPatient);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szShiftRecordID", szShiftRecordID);
                RestHandler.Instance.AddParameter("szPatientID", szPatientID);
                RestHandler.Instance.AddParameter("szVisitID", szVisitID);
                RestHandler.Instance.AddParameter("szSubID", szSubID); 
                shRet = RestHandler.Instance.Put("NurShiftAccess/DeleteShiftPatient");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftPatient(szShiftRecordID
                , szPatientID, szVisitID, szSubID);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szShiftRecordID", szShiftRecordID);
                shRet = RestHandler.Instance.Get<ShiftPatient>("NurShiftAccess/GetShiftPatientList1", ref lstShiftPatients);
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetShiftPatientList(szShiftRecordID, ref lstShiftPatients);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter("dtShiftDate", dtShiftDate);
                shRet = RestHandler.Instance.Get<ShiftPatient>("NurShiftAccess/GetShiftPatientList2", ref lstShiftPatients);
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetShiftPatientList(szWardCode, dtShiftDate, ref lstShiftPatients);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(nursingShiftInfo);
                shRet = RestHandler.Instance.Post("NurShiftAccess/SaveNursingShiftInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.SaveNursingShiftInfo(nursingShiftInfo);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter(nursingShiftInfo);
                shRet = RestHandler.Instance.Post("NurShiftAccess/UpdateNursingShiftInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.UpdateNursingShiftInfo(nursingShiftInfo);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter("dtShiftDate", dtShiftDate);
                RestHandler.Instance.AddParameter("szRankCode", szRankCode);
                shRet = RestHandler.Instance.Get<NursingShiftInfo>("NurShiftAccess/GetNursingShiftInfo", ref nursingShiftInfo);
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetNursingShiftInfo(szWardCode, dtShiftDate
                , szRankCode, ref nursingShiftInfo);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                RestHandler.Instance.AddParameter("dtShiftDate", dtShiftDate);
                shRet = RestHandler.Instance.Get<NursingShiftInfo>("NurShiftAccess/GetNursingShiftInfos", ref lstNursingShiftInfos);
                if (lstNursingShiftInfos == null)
                    lstNursingShiftInfos = new List<NursingShiftInfo>();
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetNursingShiftInfos(szWardCode, dtShiftDate, ref lstNursingShiftInfos);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                shRet = RestHandler.Instance.Get<ShiftItemAliasInfo>("NurShiftAccess/GetShiftItemAliasInfos", ref lstShiftItemAliasInfos);
                if (lstShiftItemAliasInfos == null)
                    lstShiftItemAliasInfos = new List<ShiftItemAliasInfo>();
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.GetShiftItemAliasInfos(szWardCode, ref lstShiftItemAliasInfos);
            }
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
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szItemAliasCode", szItemAliasCode);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                shRet = RestHandler.Instance.Put("NurShiftAccess/DeleteShiftItemAlias");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftItemAlias(szItemAliasCode, szWardCode);
            }
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
        /// 保存指定的交班特殊病人信息
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

        /// <summary>
        /// 删除指定病区的指定交班动态信息
        /// </summary>
        /// <param name="szShiftRecordID">交班索引代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftWardStatusInfo(string szShiftRecordID)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szShiftRecordID", szShiftRecordID);
                shRet = RestHandler.Instance.Put("NurShiftAccess/DeleteShiftWardStatusInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftWardStatusInfo(szShiftRecordID);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 删除指定病区的指定交班索引信息
        /// </summary>
        /// <param name="szShiftRecordID">交班索引代码</param>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteShiftIndexInfo(string szShiftRecordID, string szWardCode)
        {
            short shRet = ServerData.ExecuteResult.OK;
            if (base.ConnectionMode == ConnectionMode.Rest)
            {
                RestHandler.Instance.ClearParameters();
                RestHandler.Instance.AddParameter("szShiftRecordID", szShiftRecordID);
                RestHandler.Instance.AddParameter("szWardCode", szWardCode);
                shRet = RestHandler.Instance.Put("NurShiftAccess/DeleteShiftIndexInfo");
            }
            else
            {
                shRet = SystemContext.Instance.NurShiftAccess.DeleteShiftIndexInfo(szShiftRecordID, szWardCode);
            }
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
    }
}