// ***********************************************************
// 护理电子病历系统,
// 病人任务列表数据访问接口封装类.
// Creator:YangMingkun  Date:2013-9-22
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.NurDoc.DAL;
using Heren.Common.Libraries;

namespace Heren.NurDoc.Data
{
    public class PatientTaskService
    {
        private static PatientTaskService m_instance = null;

        /// <summary>
        /// 获取表单数据访问服务实例
        /// </summary>
        public static PatientTaskService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PatientTaskService();
                return m_instance;
            }
        }

        private PatientTaskService()
        {
        }

        /// <summary>
        /// 获取单病人本班评估任务列表
        /// </summary>
        /// <param name="dtPatientInfo">含PID和VID的病人表</param>
        /// <param name="dtAssessTaskInfo">病人本班任务列表</param>
        /// <returns>0-成功;1-失败</returns>
        public short GetPatientAssessTaskList(DataTable dtPatientInfo, ref DataTable dtAssessTaskInfo)
        {
            try
            {
                //当前时间
                DateTime dtCurrentTime = SysTimeService.Instance.Now;
                DateTime dt1 = DateTime.Now;
                string szBeginTime = dtPatientInfo.Rows[0]["BeginTime"].ToString();// '本班起始时间
                string szEndTime = dtPatientInfo.Rows[0]["EndTime"].ToString();// '本班结束时间
                string szPreBeginTime = dtPatientInfo.Rows[0]["PreBeginTime"].ToString(); //'上个班起始时间
                string szPreEndTime = dtPatientInfo.Rows[0]["PreEndTime"].ToString();// '上个班结束时间
                string szPatientID = dtPatientInfo.Rows[0]["PATIENT_ID"].ToString();
                string szVisitID = dtPatientInfo.Rows[0]["VISIT_ID"].ToString();
                dtAssessTaskInfo = new DataTable();
                dtAssessTaskInfo.Columns.Add("PATIENT_ID");//'患者ID
                dtAssessTaskInfo.Columns.Add("VISIT_ID"); //'就诊ID
                dtAssessTaskInfo.Columns.Add("RECORD_TIME");// '时间
                dtAssessTaskInfo.Columns.Add("ASSESS_NAME");//'评估名称
                dtAssessTaskInfo.Columns.Add("ASSESS_DOC_ID");//'评估单记录ID
                dtAssessTaskInfo.Columns.Add("ASSESS_ID");//'评估单类型ID
                dtAssessTaskInfo.Columns.Add("ASSESS_RESULT");// '结果
                dtAssessTaskInfo.Columns.Add("MEASURE");//'措施 
                dtAssessTaskInfo.Columns.Add("RecordName");//'评估人
                dtAssessTaskInfo.Columns.Add("IsTaskState");// '任务完成状态（必要项）
                // '上个班次未完成的评估任务   //入院评估单跟时间无关  需分开查询
                string szDocTypeID = "100020,100030,100040,100050,100060,100070,100080,100090,100100";
                string szRYDocTypeID = "100010,656733";
                NurDocList lstNurDocInfo = new NurDocList();
                if (szPreBeginTime != string.Empty || szPreEndTime != string.Empty)
                {
                    this.GetDocInfos(szPatientID, szVisitID, szDocTypeID, DateTime.Parse(szPreBeginTime), DateTime.Parse(szPreEndTime), ref lstNurDocInfo);
                    lstNurDocInfo.SortByTime(true);
                    foreach (NurDocInfo nurDocInfo in lstNurDocInfo)
                    {
                        if (nurDocInfo.szFinished != "√")
                        {
                            DataRow row = dtAssessTaskInfo.Rows.Add();
                            row["PATIENT_ID"] = szPatientID;
                            row["VISIT_ID"] = szVisitID;
                            row["IsTaskState"] = "0";
                            row["RECORD_TIME"] = nurDocInfo.RecordTime;
                            row["ASSESS_NAME"] = nurDocInfo.DocTitle;
                            row["ASSESS_ID"] = nurDocInfo.DocTypeID;
                            row["ASSESS_DOC_ID"] = nurDocInfo.DocID;
                            row["ASSESS_RESULT"] = nurDocInfo.Result;
                            row["MEASURE"] = nurDocInfo.Measures;
                            row["RecordName"] = nurDocInfo.ModifierName;
                        }
                    }
                }

                //获得当前班次评估记录 '100010','100020','100030','100040','100050','100060','100070','100080','100090','100100',新增儿童入院'656733'
                int ryCount = 0;
                int ycCount = 0;
                int ddzcCount = 0;
                int dghtCount = 0;
                int ttCount = 0;
                int jmwsCount = 0;
                int yyCount = 0;
                int ysjCount = 0;
                int fsCount = 0;
                Boolean fsStopFlag = false;//标记翻身卡是否终止
                Boolean ysjStopFlag = false; //标记约束具观察是否终止
                lstNurDocInfo.Clear();
                this.GetDocInfos(szPatientID, szVisitID, szDocTypeID, DateTime.Parse(szBeginTime), DateTime.Parse(szEndTime), ref lstNurDocInfo);
                NurDocList lstRYNurDocInfo = new NurDocList();
                this.GetDocInfos(szPatientID, szVisitID, szRYDocTypeID, DateTime.MinValue, DateTime.MaxValue, ref lstRYNurDocInfo);
                lstNurDocInfo.AddRange(lstRYNurDocInfo);
                lstNurDocInfo.SortByTime(true);

                foreach (NurDocInfo nurDocInfo in lstNurDocInfo)
                {
                    string szDocType = nurDocInfo.DocTypeID;
                    string szDocTitle = nurDocInfo.DocTitle;
                    string szDocSetID = nurDocInfo.DocSetID;
                    string szDocID = nurDocInfo.DocID;
                    string szRecordName = nurDocInfo.ModifierName;
                    string szRecordTime = nurDocInfo.RecordTime.ToString();
                    if (szDocType == "100010" || szDocType == "656733")//成人入院评估//儿童入院评估
                    {
                        ryCount += 1;
                        if (ryCount > 1)
                            continue;
                        if (nurDocInfo.Frequency == "终止")
                            continue;
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = string.Empty;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100020") //'压疮风险评估
                    {
                        ycCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                            continue;

                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100030") //''跌倒（坠床）风险评估
                    {
                        ddzcCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                            continue;

                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100040") //'导管滑脱风险评估
                    {
                        dghtCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                            continue;

                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100050") //'疼痛评估
                    {
                        ttCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                            continue;

                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100060") //'静脉外渗风险评估
                    {
                        jmwsCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                            continue;

                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100070") //'营养评估
                    {
                        yyCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                            continue;

                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100090") //'约束具观察记录
                    {
                        ysjCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                        {
                            ysjStopFlag = true;
                            continue;
                        }
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                    else if (szDocType == "100100") //'翻身记录
                    {
                        fsCount += 1;
                        if (nurDocInfo.Frequency == "终止")
                        {
                            fsStopFlag = true;
                            continue;
                        }
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "1";
                        row["RECORD_TIME"] = szRecordTime;
                        row["ASSESS_NAME"] = szDocTitle;
                        row["ASSESS_ID"] = szDocType;
                        row["ASSESS_DOC_ID"] = szDocID;
                        row["ASSESS_RESULT"] = nurDocInfo.Result;
                        row["MEASURE"] = nurDocInfo.Measures;
                        row["RecordName"] = szRecordName;
                    }
                }

                if (ryCount == 0)// '判断是否需要添加入院评估记录(成人或者儿童)
                {
                    NurDocList nurdocs = new NurDocList();
                    SystemContext.Instance.DocumentAccess.GetDocInfos(szPatientID, szVisitID, "100010,656733", ref nurdocs);
                    if (nurdocs != null && nurdocs.Count <= 0)
                    {
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "0";
                        row["RECORD_TIME"] = string.Empty;
                        row["ASSESS_NAME"] = "入院护理评估";
                        row["ASSESS_ID"] = "100010,656733";
                        row["ASSESS_RESULT"] = string.Empty;
                        row["MEASURE"] = string.Empty;
                    }
                }
                if (ycCount == 0)//  '判断是否需要添加压疮风险评估任务
                {
                    string szTaskTime = GetSummaryData(szPatientID, szVisitID, "100020", "评估频次");
                    if (szTaskTime == "0" || szTaskTime == "1/B")
                    {
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "0";
                        row["RECORD_TIME"] = string.Empty;
                        row["ASSESS_NAME"] = "压疮风险评估";
                        row["ASSESS_ID"] = "100020";
                        row["ASSESS_RESULT"] = string.Empty;
                        row["MEASURE"] = string.Empty;
                    }
                }
                if (ddzcCount == 0)//  '判断是否需要添加跌倒（坠床）风险评估
                {
                    string szTaskTime = GetSummaryData(szPatientID, szVisitID, "100030", "评估频次");
                    if (szTaskTime == "0" || szTaskTime == "1/B")
                    {
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "0";
                        row["RECORD_TIME"] = string.Empty;
                        row["ASSESS_NAME"] = "跌倒(坠床)风险评估";
                        row["ASSESS_ID"] = "100030";
                        row["ASSESS_RESULT"] = string.Empty;
                        row["MEASURE"] = string.Empty;
                    }
                }
                if (dghtCount == 0)//  '判断是否需要添加导管滑脱风险评估任务
                {
                    string szTaskTime = GetSummaryData(szPatientID, szVisitID, "100040", "评估频次");
                    if (szTaskTime == "0" || szTaskTime == "1/B")
                    {
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "0";
                        row["RECORD_TIME"] = string.Empty;
                        row["ASSESS_NAME"] = "导管滑脱风险评估";
                        row["ASSESS_ID"] = "100040";
                        row["ASSESS_RESULT"] = string.Empty;
                        row["MEASURE"] = string.Empty;
                    }
                    else if (szTaskTime == "1/D")
                    {
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
                        DocumentService.Instance.GetDocInfos(szPatientID, szVisitID, "100040", dtBeginTime, dtEndTime, ref nurdocs);
                        if (nurdocs != null && nurdocs.Count > 0)
                        {
                            DataRow row = dtAssessTaskInfo.Rows.Add();
                            row["PATIENT_ID"] = szPatientID;
                            row["VISIT_ID"] = szVisitID;
                            row["IsTaskState"] = "0";
                            row["RECORD_TIME"] = string.Empty;
                            row["ASSESS_NAME"] = "导管滑脱风险评估";
                            row["ASSESS_ID"] = "100040";
                            row["ASSESS_RESULT"] = string.Empty;
                            row["MEASURE"] = string.Empty;
                        }
                    }
                    else if (szTaskTime == "1/W")
                    {
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
                        DocumentService.Instance.GetDocInfos(szPatientID, szVisitID, "100040", dtBeginTime.AddDays(-7), dtEndTime, ref nurdocs);
                        if (nurdocs != null && nurdocs.Count > 0)
                        {
                            DataRow row = dtAssessTaskInfo.Rows.Add();
                            row["PATIENT_ID"] = szPatientID;
                            row["VISIT_ID"] = szVisitID;
                            row["IsTaskState"] = "0";
                            row["RECORD_TIME"] = string.Empty;
                            row["ASSESS_NAME"] = "导管滑脱风险评估";
                            row["ASSESS_ID"] = "100040";
                            row["ASSESS_RESULT"] = string.Empty;
                            row["MEASURE"] = string.Empty;
                        }
                    }
                }
                if (ttCount == 0)//  '判断是否需要添加疼痛评估任务
                {
                    string szTaskTime = GetSummaryData(szPatientID, szVisitID, "100050", "评估频次");
                    if (szTaskTime == "0" || szTaskTime == "1/B")
                    {
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "0";
                        row["RECORD_TIME"] = string.Empty;
                        row["ASSESS_NAME"] = "疼痛评估";
                        row["ASSESS_ID"] = "100050";
                        row["ASSESS_RESULT"] = string.Empty;
                        row["MEASURE"] = string.Empty;
                    }
                    else if (szTaskTime == "1/D")
                    {
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
                        DocumentService.Instance.GetDocInfos(szPatientID, szVisitID, "100050", dtBeginTime, dtEndTime, ref nurdocs);
                        if (nurdocs != null && nurdocs.Count <= 0)
                        {
                            DataRow row = dtAssessTaskInfo.Rows.Add();
                            row["PATIENT_ID"] = szPatientID;
                            row["VISIT_ID"] = szVisitID;
                            row["IsTaskState"] = "0";
                            row["RECORD_TIME"] = string.Empty;
                            row["ASSESS_NAME"] = "疼痛评估";
                            row["ASSESS_ID"] = "100050";
                            row["ASSESS_RESULT"] = string.Empty;
                            row["MEASURE"] = string.Empty;
                        }
                    }
                }
                if (jmwsCount == 0)//  '判断是否需要添加静脉外渗风险评估任务
                {
                    string szTaskTime = GetSummaryData(szPatientID, szVisitID, "100060", "评估频次");
                    if (szTaskTime == "0" || szTaskTime == "1/B")
                    {
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "0";
                        row["RECORD_TIME"] = string.Empty;
                        row["ASSESS_NAME"] = "静脉外渗风险评估";
                        row["ASSESS_ID"] = "100060";
                        row["ASSESS_RESULT"] = string.Empty;
                        row["MEASURE"] = string.Empty;
                    }
                }
                if (yyCount == 0)//  '判断是否需要添加营养评估记录
                {
                    NurDocList nurdocs = new NurDocList();
                    SystemContext.Instance.DocumentAccess.GetDocInfos(szPatientID, szVisitID, "100070", ref nurdocs);
                    if (nurdocs != null && nurdocs.Count <= 0)
                    {
                        DataRow row = dtAssessTaskInfo.Rows.Add();
                        row["PATIENT_ID"] = szPatientID;
                        row["VISIT_ID"] = szVisitID;
                        row["IsTaskState"] = "0";
                        row["RECORD_TIME"] = string.Empty;
                        row["ASSESS_NAME"] = "营养评估";
                        row["ASSESS_ID"] = "100070";
                        row["ASSESS_RESULT"] = string.Empty;
                        row["MEASURE"] = string.Empty;
                    }
                }
                if (ysjCount < 8 && !ysjStopFlag)//  '约束具观察记录 每小时一次
                {
                    DataRow row = dtAssessTaskInfo.Rows.Add();
                    row["PATIENT_ID"] = szPatientID;
                    row["VISIT_ID"] = szVisitID;
                    row["IsTaskState"] = "0";
                    row["RECORD_TIME"] = string.Empty;
                    row["ASSESS_NAME"] = "约束具观察记录";
                    row["ASSESS_ID"] = "100090";
                    row["ASSESS_RESULT"] = string.Empty;
                    row["MEASURE"] = string.Empty;
                }
                if (fsCount < 4 && !fsStopFlag)//  '翻身记录
                {
                    DataRow row = dtAssessTaskInfo.Rows.Add();
                    row["PATIENT_ID"] = szPatientID;
                    row["VISIT_ID"] = szVisitID;
                    row["IsTaskState"] = "0";
                    row["RECORD_TIME"] = string.Empty;
                    row["ASSESS_NAME"] = "翻身记录";
                    row["ASSESS_ID"] = "100100";
                    row["ASSESS_RESULT"] = string.Empty;
                    row["MEASURE"] = string.Empty;
                }
                return (short)(dtAssessTaskInfo == null ? SystemConst.ReturnValue.FAILED : SystemConst.ReturnValue.OK);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("PatientTaskService.GetPatientAssessTaskList"
                      , null, null, "获取单病人评估任务失败!", ex);
                return SystemConst.ReturnValue.FAILED;
            }
        }

        /// <summary>
        /// 得到评估文档列表，包含评估结果，评估频次，措施等信息
        /// </summary>
        /// <param name="szPatientID">病人ID</param>
        /// <param name="szVisitID">病人VID</param>
        /// <param name="szDocTypeID">文档模板id</param>
        /// <param name="dtBeginTime">开始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="lstDocInfos">文档信息列表</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetDocInfos(string szPatientID, string szVisitID, string szDocTypeID
            , DateTime dtBeginTime, DateTime dtEndTime, ref NurDocList lstDocInfos)
        {
            lstDocInfos.Clear();
            NurDocList DocInfos = new NurDocList();
            short shRet = DocumentService.Instance.GetDocInfos(szPatientID, szVisitID, szDocTypeID, dtBeginTime, dtEndTime, ref DocInfos);
            if (shRet != SystemConst.ReturnValue.OK)
                return shRet;
            string szDataName = "'总分','措施','评估频次','过敏史','安全情况','是否已签署知情同意书','皮肤状况','卧位','是否完成'";
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            shRet = SystemContext.Instance.DocumentAccess.GetSummaryDatas(szPatientID, szVisitID, szDataName,false, ref lstSummaryData);
            if (shRet == SystemConst.ReturnValue.EXCEPTION)
                return shRet;
            foreach (NurDocInfo docInfo in DocInfos)
            {
                foreach (SummaryData data in lstSummaryData)
                {
                    if (docInfo.DocSetID != data.DocID)
                        continue;
                    if (data.DataName == "评估频次")
                    {
                        docInfo.Frequency = data.DataValue;
                        if (!string.IsNullOrEmpty(data.DataValue) && data.DataValue.Equals("终止"))
                        {
                            break;
                        }
                    }
                    if (data.DataName == "是否完成")
                    {
                        docInfo.szFinished = data.DataValue;
                    }
                    if (data.DataName == "总分" || data.DataName == "过敏史")
                    {
                        docInfo.Result = data.DataValue;
                    }
                    else if (data.DataName == "安全情况" || data.DataName == "是否已签署知情同意书")
                    {
                        docInfo.Result += data.DataValue + ",";
                    }
                    else if (data.DataName == "皮肤状况" || data.DataName == "卧位")
                    {
                        docInfo.Result += data.DataValue + ",";
                    }
                    else if (data.DataName == "措施")
                    {
                        docInfo.Measures = data.DataValue;
                    }
                }
                if (!string.IsNullOrEmpty(docInfo.Result) && docInfo.Result.EndsWith(","))
                    docInfo.Result = docInfo.Result.TrimEnd(',');
                lstDocInfos.Add(docInfo);
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 通过文档ID和名称取得摘要数数据
        /// </summary>
        /// <param name="szDocId">文档集ID</param>
        /// <param name="szDataName">字段名</param>
        /// <returns>字段值</returns>
        public string GetSummaryData(string szDocId, string szDataName)
        {
            SummaryData summaryData = new SummaryData();
            short shRet = DocumentService.Instance.GetSummaryData(szDocId, szDataName, ref summaryData);
            if (shRet == SystemConst.ReturnValue.OK)
                return summaryData.DataValue;
            else
                return string.Empty;
        }

        /// <summary>
        /// 取得最后一次的摘要数据
        /// </summary>
        /// <param name="szPatientID">病人编号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="szDocTypeId">文档类型ID</param>
        /// <param name="szDataName">摘要名称</param>
        /// <returns>字段值</returns>
        public string GetSummaryData(string szPatientID, string szVisitID, string szDocTypeId, string szDataName)
        {
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            short shRet = DocumentService.Instance.GetSummaryData(szPatientID, szVisitID, szDocTypeId, szDataName, ref lstSummaryData);
            if (shRet == ServerData.ExecuteResult.OK && lstSummaryData.Count > 0)
            {
                return lstSummaryData[0].DataValue.ToString();
            }
            else
            {
                return "0";//未做评估
            }
        }
    }
}
