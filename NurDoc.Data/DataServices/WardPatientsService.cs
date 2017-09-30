// ***********************************************************
// 护理电子病历系统,
// 病人列表评估视图数据访问接口封装类.
// Creator:YangMingkun  Date:2013-9-22
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.NurDoc.DAL;
using Heren.Common.Libraries;

namespace Heren.NurDoc.Data.DataServices
{
    public class WardPatientsService
    {
        private static WardPatientsService m_instance = null;

        /// <summary>
        /// 获取表单数据访问服务实例
        /// </summary>
        public static WardPatientsService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new WardPatientsService();
                return m_instance;
            }
        }

        private WardPatientsService()
        {
        }

        /// <summary>
        /// 获取护士所管辖病区的患者评估情况一览图信息.
        /// 其中包括患者的基本信息、任务列表、风险列表
        /// </summary>
        /// <param name="dtPatientInfo">病人信息DT</param>
        /// <param name="dtNursingAssessInfo">含病人年龄等基本信息的任务与风险列表</param>
        /// <returns>0-成功;1-失败</returns>
        public short GetWardPatientsNursingAssess(DataTable dtPatientInfo, ref DataTable dtNursingAssessInfo)
        {
            string szPatientIDList = string.Empty;
            if (dtPatientInfo == null)
            {
                LogManager.Instance.WriteLog("GetWardPatientsNursingAssess", "参数不正确：dtPatientInfo是空");
                return SystemConst.ReturnValue.FAILED;
            }
            if (dtPatientInfo != null)
            {
                foreach (DataRow row in dtPatientInfo.Rows)
                {
                    string szPatientID = row["Patient_ID"].ToString();
                    if (GlobalMethods.Misc.IsEmptyString(szPatientIDList))
                    {
                        szPatientIDList = string.Format("'{0}'", szPatientID);
                        continue;
                    }
                    szPatientIDList = string.Format("{0},'{1}'"
                        , szPatientIDList, szPatientID);
                }
            }
            List<PatVisitInfo> lstPatVisitInfo = new List<PatVisitInfo>();//病人列表

            short shRet = SystemContext.Instance.PatVisitAccess.GetInPatVisitInfoList(null, szPatientIDList, ref lstPatVisitInfo);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
            {
                LogManager.Instance.WriteLog("GetWardPatientsNursingAssess", "获取当前病区病人列表不存在");
                return SystemConst.ReturnValue.FAILED;
            }
            if (shRet != ServerData.ExecuteResult.OK)
            {
                LogManager.Instance.WriteLog("GetWardPatientsNursingAssess", "获取当前病区在院病人列表失败");
                return SystemConst.ReturnValue.FAILED;
            }
            string szWardCode = lstPatVisitInfo[0].WardCode;
            NurDocList lstNurDocLst = new NurDocList();//文档列表
            List<SummaryData> lstSummaryData = new List<SummaryData>();//摘要数据列表
            DateTime dtCurrentTime = SysTimeService.Instance.Now;
            List<ShiftRankInfo> lstNursingShiftInfo = new List<ShiftRankInfo>();
            ShiftRankInfo shiftRankInfo = null;
            shRet = NurShiftService.Instance.GetShiftRankInfos(szWardCode, null, ref lstNursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                LogManager.Instance.WriteLog("GetWardPatientsNursingAssess", "未查询到科室班次配置信息");
                return shRet;
            }

            foreach (ShiftRankInfo item in lstNursingShiftInfo)
            {
                item.UpdateShiftRankTime(dtCurrentTime);
                if (dtCurrentTime >= item.StartTime && dtCurrentTime <= item.EndTime)
                {
                    shiftRankInfo = item;
                    break;
                }
            }
            if (shiftRankInfo == null)
            {
                LogManager.Instance.WriteLog("GetWardPatientsNursingAssess", "获取当前班次信息失败");
                return SystemConst.ReturnValue.FAILED;
            }

            //得到评估视图数据结构
            dtNursingAssessInfo = new DataTable();
            dtNursingAssessInfo.Columns.Add("PATIENT_ID"); //患者ID
            dtNursingAssessInfo.Columns.Add("VISIT_ID"); //就诊ID
            dtNursingAssessInfo.Columns.Add("BED_NO"); //床号
            dtNursingAssessInfo.Columns.Add("BED_LABEL");// '床标号
            dtNursingAssessInfo.Columns.Add("NAME"); //'姓名
            dtNursingAssessInfo.Columns.Add("SEX"); //'性别
            dtNursingAssessInfo.Columns.Add("AGE"); //'年龄 
            dtNursingAssessInfo.Columns.Add("charge_type"); //'费别 
            dtNursingAssessInfo.Columns.Add("diagnosis"); //'诊断
            dtNursingAssessInfo.Columns.Add("allergy_drugs"); //'过敏史
            dtNursingAssessInfo.Columns.Add("dept_code"); //'科室代码
            dtNursingAssessInfo.Columns.Add("dept_name"); //'科室名称
            dtNursingAssessInfo.Columns.Add("ward_code"); //'病区代码
            dtNursingAssessInfo.Columns.Add("WARD_NAME"); //'科室 
            dtNursingAssessInfo.Columns.Add("birth_time"); //出生日期
            dtNursingAssessInfo.Columns.Add("visit_time"); //'入科时间
            dtNursingAssessInfo.Columns.Add("inp_no"); //'住院号
            dtNursingAssessInfo.Columns.Add("dtAdmWardDateTime"); //'住院号
            dtNursingAssessInfo.Columns.Add("incharge_doctor"); //'住院号

            dtNursingAssessInfo.Columns.Add("PATIENT_CONDITION"); //'病情 
            dtNursingAssessInfo.Columns.Add("NURSING_CLASS"); //'护理等级 
            dtNursingAssessInfo.Columns.Add("BABY_INDICATOR"); //'婴儿标志
            dtNursingAssessInfo.Columns.Add("IsAssessTask"); //'是否有任务(0:没有，1：有)
            dtNursingAssessInfo.Columns.Add("YCFX"); //'是否有压疮风险(0:没有，1：有) 
            dtNursingAssessInfo.Columns.Add("YCFX_DT");// '最后一条压疮风险评估记录时间
            dtNursingAssessInfo.Columns.Add("TT"); //'是否有疼痛(0:没有，1：有) 
            dtNursingAssessInfo.Columns.Add("TT_DT"); //'最后疼痛
            dtNursingAssessInfo.Columns.Add("ZCFX");// '是否有坠床风险(0:没有，1：有) 
            dtNursingAssessInfo.Columns.Add("ZCFX_DT"); //''最后一条坠床风险评估记录时间
            dtNursingAssessInfo.Columns.Add("DDFX"); //'是否有跌倒风险(0:没有，1：有) 
            dtNursingAssessInfo.Columns.Add("DDFX_DT"); //'最后一条跌倒风险评估记录时间
            dtNursingAssessInfo.Columns.Add("DGHTFX");//'是否有导管滑脱风险(0:没有，1：有) 
            dtNursingAssessInfo.Columns.Add("DGHTFX_DT");// '最后一条导管滑脱风险评估记录时间
            dtNursingAssessInfo.Columns.Add("DGHTDS");// '导管滑脱度数(I度，II度，III度) 
            dtNursingAssessInfo.Columns.Add("JMWSFX");// '是否有静脉外渗风险(0:没有，1：有) 
            dtNursingAssessInfo.Columns.Add("JMWSFX_DT"); //'最后一条静脉外渗风险评估记录时间
            dtNursingAssessInfo.Columns.Add("GMSFX");// '是否有过敏史风险(0:没有，1：有) 
            dtNursingAssessInfo.Columns.Add("GMSFX_DT"); //'最后一条入院评估记录时间
            dtNursingAssessInfo.Columns.Add("GMSNR"); //'过敏史内容 

            string szDocTypeID = "'100010','100020','100030','100040','100050','100060','100070','100080','100090','100100','656733'";
            shRet = SystemContext.Instance.DocumentAccess.GetDocInfos(szWardCode, szDocTypeID, ref lstNurDocLst);
            string szDataName = "'风险类型','风险','总分','过敏史','评估频次','是否完成'";
            shRet = SystemContext.Instance.DocumentAccess.GetSummaryDatas(szWardCode, szDataName, ref lstSummaryData);

            string szDataValue = string.Empty;
            foreach (PatVisitInfo patVisitInfo in lstPatVisitInfo)
            {
                int ryCount = 0;  //'成人入院评估 100010   儿童入院评估  656733
                int ycCount = 0;  //'压疮评估 100020
                int ddzcCount = 0; // '跌倒坠床评估 100030
                int dghtCount = 0; // '导管滑脱评估 100040
                int ttCount = 0;  // '疼痛评估 100050
                int jmwsCount = 0;    // '静脉外渗 100060
                int yyCount = 0;  //  '营养评估 100070
                int ysjCount = 0; //  '约束具 100090
                int fsCount = 0; // '翻身卡 100100
                //int fanshenka = 0;  // '是否建立翻身卡

                DataRow row = dtNursingAssessInfo.Rows.Add();
                row["PATIENT_ID"] = patVisitInfo.PatientId; //'患者ID
                row["VISIT_ID"] = patVisitInfo.VisitId; //'就诊ID
                row["BED_NO"] = patVisitInfo.BedCode; //'床号
                row["BED_LABEL"] = patVisitInfo.BedLabel; //'床标号
                row["NAME"] = patVisitInfo.PatientName;//'姓名
                row["SEX"] = patVisitInfo.PatientSex;// '性别
                row["AGE"] = GlobalMethods.SysTime.GetAgeText(
                    patVisitInfo.BirthTime,
                    patVisitInfo.VisitTime);//'年龄
                row["dtAdmWardDateTime"] = patVisitInfo.VisitTime; //'入科时间
                row["BABY_INDICATOR"] = string.Empty;// '婴儿标志
                row["WARD_NAME"] = patVisitInfo.WardName;// '科室
                row["PATIENT_CONDITION"] = patVisitInfo.PatientCondition; ///'病情
                row["NURSING_CLASS"] = patVisitInfo.NursingClass;// '护理等级
                row["charge_type"] = patVisitInfo.ChargeType;
                row["birth_time"] = patVisitInfo.BirthTime; //出生日期
                row["inp_no"] = patVisitInfo.InpNo;//入院号
                row["visit_time"] = patVisitInfo.VisitTime;//入科时间
                row["dept_code"] = patVisitInfo.DeptCode;
                row["dept_name"] = patVisitInfo.DeptName;
                row["ward_code"] = patVisitInfo.WardCode;
                row["ward_name"] = patVisitInfo.WardName;
                row["incharge_doctor"] = patVisitInfo.InchargeDoctor;
                row["diagnosis"] = patVisitInfo.Diagnosis;
                row["allergy_drugs"] = patVisitInfo.AllergyDrugs;

                foreach (NurDocInfo nurDocInfo in lstNurDocLst)
                {
                    if (patVisitInfo.PatientId != nurDocInfo.PatientID || patVisitInfo.VisitId != nurDocInfo.VisitID)
                    {
                        continue;
                    }
                    //入院评估 最后一次的记录的 是否有过敏史
                    if ((nurDocInfo.DocTypeID == "100010" || nurDocInfo.DocTypeID == "656733") && ryCount == 0)
                    {
                        ryCount += 1; //已做过入院评估，无需再做 接下去也无需再判断是否有风险 按照时间倒序来查询的 查最近一次的入院评估
                        szDataValue = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "过敏史");
                        if (!GlobalMethods.Misc.IsEmptyString(szDataValue) && szDataValue.Trim() != "无")
                        {
                            row["GMSFX"] = "1"; //'有过敏风险
                            row["GMSNR"] = szDataValue;//'过敏史内容
                        }
                        else
                        {
                            row["GMSFX"] = "0";//  '无过敏风险
                            row["GMSNR"] = string.Empty;//  '过敏史内容
                        }
                        row["GMSFX_DT"] = nurDocInfo.RecordTime;
                    }
                    //压疮风险 最后一次的记录 是否有风险 评分≤14分，为易发生压疮危险人群，须采取预防压疮的护理措施，每班需评估一次
                    if (nurDocInfo.DocTypeID == "100020" && row["YCFX"].ToString() == string.Empty)
                    {
                        szDataValue = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "总分");
                        if (!GlobalMethods.Misc.IsEmptyString(szDataValue))
                        {
                            if (GlobalMethods.Convert.StringToValue(szDataValue, 0) <= 14)
                                row["YCFX"] = "1";
                            else
                                row["YCFX"] = "0";
                        }
                        row["YCFX_DT"] = nurDocInfo.RecordTime;

                        //判断当前班次是否有压疮评估任务 最后一次评估有风险 当前班次有评估记录

                        if (nurDocInfo.RecordTime >= shiftRankInfo.StartTime && row["YCFX"].ToString() == "1")
                        {
                            ycCount += 1;
                        }
                    }
                    //跌倒坠床评估 存在跌倒（坠床）风险需每班评估一次（最后一次评估无风险则解除频次评估任务）
                    if (nurDocInfo.DocTypeID == "100030" && row["ZCFX"].ToString() == string.Empty)
                    {
                        szDataValue = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "风险类型");
                        //string szComplete = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "是否完成");
                        //if (szComplete == "√")
                        //{
                        //    row["ZCFX"] = "0";
                        //    row["DDFX"] = "0";
                        //    szDataValue = string.Empty;
                        //}
                        //if (szDataValue == "跌倒")
                        //{
                        //    row["ZCFX"] = "0";
                        //    row["DDFX"] = "1";
                        //}
                        //else if (szDataValue == "坠床")
                        //{
                        //    row["ZCFX"] = "1";
                        //    row["DDFX"] = "0";
                        //}
                        //else 
                        if (szDataValue.Trim() == "跌倒/坠床" || szDataValue == "跌倒" || szDataValue == "坠床")
                        {
                            row["ZCFX"] = "1";
                            row["DDFX"] = "1";
                        }
                        else
                        {
                            row["ZCFX"] = "0";
                            row["DDFX"] = "0";
                        }

                        row["ZCFX_DT"] = nurDocInfo.RecordTime;
                        row["DDFX_DT"] = nurDocInfo.RecordTime;
                        //   '跌倒坠床是否有评估任务
                        szDataValue = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "评估频次");
                        if (szDataValue == "1/B")
                        {
                            if (nurDocInfo.RecordTime >= shiftRankInfo.StartTime)
                            {
                                ddzcCount += 1;
                            }
                        }
                    }
                    //'导管滑脱风险判断及频次：Ⅰ度：＜8分，1次/周；Ⅱ度：8-12分，1次/日；Ⅲ度：＞12分，1次/班；描述方法：分/度（最后一次评估无导管或导管已拔除则解除风险评估任务）
                    if (nurDocInfo.DocTypeID == "100040" && row["DGHTFX"].ToString() == string.Empty)
                    {
                        szDataValue = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "评估频次");
                        //string szComplete = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "是否完成");
                        //if (szComplete == "√")
                        //{
                        //    row["DGHTFX"] = "0";
                        //    row["DGHTDS"] = string.Empty;
                        //    szDataValue = string.Empty;
                        //}
                        if (szDataValue == "1/B")
                        {
                            row["DGHTFX"] = "1";
                            row["DGHTDS"] = "III";
                        }
                        else if (szDataValue == "1/D")
                        {
                            row["DGHTFX"] = "1";
                            row["DGHTDS"] = "II";
                            //查找当天是否做过该评估任务
                            //每天第一班的起始时间和最后一班结束时间
                            DateTime dtBeginTime = dtCurrentTime.Date;
                            DateTime dtEndTime = dtCurrentTime.Date;
                            if (dtCurrentTime.Hour >= 0 && dtCurrentTime.Hour <= 7)
                            {
                                dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 7, 0, 0);
                                dtBeginTime = dtEndTime.AddDays(-1);
                            }
                            else
                            {
                                dtBeginTime = new DateTime(dtBeginTime.Year, dtBeginTime.Month, dtBeginTime.Day, 7, 0, 0);
                                dtEndTime = dtBeginTime.AddDays(1);
                            }
                            NurDocList nurdocs = new NurDocList();
                            DocumentService.Instance.GetDocInfos(patVisitInfo.PatientId, patVisitInfo.VisitId, "100040", dtBeginTime, dtEndTime, ref nurdocs);
                            if (nurdocs != null && nurdocs.Count > 0)
                            {
                                dghtCount += 1; //已做过导管滑脱评估
                            }
                        }
                        else if (szDataValue == "1/W")
                        {
                            row["DGHTFX"] = "1";
                            row["DGHTDS"] = "I";
                            //查询这一周 有没有做过导管滑脱评估
                            //'每天第一班的起始时间和最后一班结束时间
                            DateTime dtBeginTime = dtCurrentTime.Date;
                            DateTime dtEndTime = dtCurrentTime.Date;
                            if (dtCurrentTime.Hour >= 0 && dtCurrentTime.Hour <= 7)
                            {
                                dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 7, 0, 0);
                                dtBeginTime = dtEndTime.AddDays(-1);
                            }
                            else
                            {
                                dtBeginTime = new DateTime(dtBeginTime.Year, dtBeginTime.Month, dtBeginTime.Day, 7, 0, 0);
                                dtEndTime = dtBeginTime.AddDays(1);
                            }
                            NurDocList nurdocs = new NurDocList();
                            DocumentService.Instance.GetDocInfos(patVisitInfo.PatientId, patVisitInfo.VisitId, "100040", dtBeginTime.AddDays(-7), dtEndTime, ref nurdocs);
                            if (nurdocs != null && nurdocs.Count > 0)
                            {
                                dghtCount += 1; //已做过导管滑脱评估
                            }
                        }
                        else
                        {
                            row["DGHTFX"] = "0";
                            row["DGHTDS"] = string.Empty;
                        }
                        row["DGHTFX_DT"] = nurDocInfo.RecordTime;
                    }
                    //'疼痛评估
                    if (nurDocInfo.DocTypeID == "100050" && row["TT"].ToString() == string.Empty)
                    {
                        szDataValue = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "评估频次");
                        if (szDataValue == "1/B")
                        {
                            row["TT"] = "1";
                            if (nurDocInfo.RecordTime >= shiftRankInfo.StartTime)
                            {
                                ttCount += 1;
                            }
                        }
                        else if (szDataValue == "1/D")
                        {
                            row["TT"] = "1";
                            //   '查找当天是否做过该评估任务
                            //'每天第一班的起始时间和最后一班结束时间
                            DateTime dtBeginTime = dtCurrentTime.Date;
                            DateTime dtEndTime = dtCurrentTime.Date;
                            if (dtCurrentTime.Hour >= 0 && dtCurrentTime.Hour <= 7)
                            {
                                dtEndTime = new DateTime(dtEndTime.Year, dtEndTime.Month, dtEndTime.Day, 7, 0, 0);
                                dtBeginTime = dtEndTime.AddDays(-1);
                            }
                            else
                            {
                                dtBeginTime = new DateTime(dtBeginTime.Year, dtBeginTime.Month, dtBeginTime.Day, 7, 0, 0);
                                dtEndTime = dtBeginTime.AddDays(1);
                            }
                            NurDocList nurdocs = new NurDocList();
                            DocumentService.Instance.GetDocInfos(patVisitInfo.PatientId, patVisitInfo.VisitId, "100050", dtBeginTime, dtEndTime, ref nurdocs);
                            if (nurdocs != null && nurdocs.Count > 0)
                            {
                                ttCount += 1; //当天已经做过疼痛评估
                            }
                        }
                        else
                        {
                            row["TT"] = "0";
                        }
                        row["TT_DT"] = nurDocInfo.RecordTime;
                    }
                    //'静脉外渗风险100060
                    if (nurDocInfo.DocTypeID == "100060" && row["JMWSFX"].ToString() == string.Empty)
                    {
                        szDataValue = GetSummaryData(lstSummaryData, nurDocInfo.DocSetID, "评估频次");

                        if (szDataValue == "1/B")
                            row["JMWSFX"] = "1";
                        else
                            row["JMWSFX"] = "0";

                        row["JMWSFX_DT"] = nurDocInfo.RecordTime;

                        // '判断当前班次是否有压疮评估任务 最后一次评估有风险 当前班次有评估记录

                        if (nurDocInfo.RecordTime >= shiftRankInfo.StartTime && row["JMWSFX"].ToString() == "1")
                        {
                            ycCount += 1;
                        }
                    }
                    //'营养评估评估 100070
                    if (nurDocInfo.DocTypeID == "100070" && yyCount == 0)
                        ycCount += 1;
                    // '约束具观察
                    if (nurDocInfo.DocTypeID == "100090" && ysjCount < 8)
                        ycCount += 1;
                    //'翻身卡
                    if (nurDocInfo.DocTypeID == "100100" && fsCount < 4)
                        fsCount += 1;
                }
                // 是否有任务
                if (ryCount == 0 || ycCount == 0 || ddzcCount == 0 || ttCount == 0 || jmwsCount == 0 || yyCount == 0 || ysjCount < 8 || fsCount < 4)
                    row["IsAssessTask"] = "1";
                else
                    row["IsAssessTask"] = "0";
            }
            return (short)(dtNursingAssessInfo == null ? 1 : 0);
        }

        private string GetSummaryData(List<SummaryData> lstSummaryData, string szDocSetID, string szDataName)
        {
            if (lstSummaryData.Count <= 0)
                return string.Empty;
            foreach (SummaryData summaryData in lstSummaryData)
            {
                if (summaryData.DocID == szDocSetID && summaryData.DataName == szDataName)
                {
                    return summaryData.DataValue;
                }
            }
            return string.Empty;
        }
    }
}
