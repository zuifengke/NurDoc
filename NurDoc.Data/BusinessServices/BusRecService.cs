// ***********************************************************
// 护理电子病历系统,
// 用于移动层接口调用 保存护理记录单及相关信息
// Creator:Ycc  Date:2014-4-28
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.Common.Forms.Editor;

namespace Heren.NurDoc.Data
{
    public class BusRecService
    {
        private static BusRecService m_instance = null;

        /// <summary>
        /// 获取护士护理工作相关服务实例
        /// </summary>
        public static BusRecService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new BusRecService();
                return m_instance;
            }
        }

        private BusRecService()
        {
        }

        /// <summary>
        /// 移动护理调用用于新增护理记录单
        /// </summary>
        /// <param name="szPatientID">病历号</param>
        /// <param name="szVisitID">就诊号</param>
        /// <param name="szUserId">用户ID</param>
        /// <param name="szDocTypeId">文档ID</param>
        /// <param name="lstKeyData">摘要list</param>
        /// <param name="szRecordTime">记录时间</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short SaveNursingRec(string szPatientID, string szVisitID, string szUserId, string szDocTypeId, List<KeyData> lstKeyData, DateTime szRecordTime)
        {
            LogManager.Instance.TextLogOnly = true;
            ServerParam.Instance.ThrowExOnError = true;
            try
            {
                LogManager.Instance.WriteLog(string.Format("szPatientID={0},szVisitID={1},szUserId={2},szDocTypeId={3},szRecordTime={4}", szPatientID, szVisitID, szUserId, szDocTypeId, szRecordTime));
                if (GlobalMethods.Misc.IsEmptyString(szPatientID) || GlobalMethods.Misc.IsEmptyString(szVisitID) || GlobalMethods.Misc.IsEmptyString(szUserId))
                {
                    throw new Exception("病历号，就诊号和用户Id不可为空!");
                    //return SystemConst.ReturnValue.FAILED;
                }
                LogManager.Instance.WriteLog("信息校验成功");
                #region 获取病人信息，用户信息，文档信息
                PatVisitInfo szPatVisitInfo = null;
                short shRet = SystemContext.Instance.PatVisitAccess.GetPatVisitInfo(szPatientID, szVisitID, ref szPatVisitInfo);
                if (shRet == ServerData.ExecuteResult.PARAM_ERROR)
                {
                    throw new Exception("护理电子病历数据库连接失败");
                }
                else if (shRet != ServerData.ExecuteResult.OK)
                {
                    throw new Exception(string.Format("检索病历号={0},就诊号={1}的病人时失败!", szPatientID, szVisitID));
                    //return SystemConst.ReturnValue.FAILED;
                }

                LogManager.Instance.WriteLog("病人检索成功");
                UserInfo szUserInfo = null;
                shRet = SystemContext.Instance.AccountAccess.GetUserInfo(szUserId, ref szUserInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    throw new Exception(string.Format("检索用户ID={0}的护士时失败!", szUserId));
                    //return SystemConst.ReturnValue.FAILED;
                }

                LogManager.Instance.WriteLog("用户检索成功");
                DocTypeInfo szDocTypeInfo = null;
                List<DocTypeInfo> lstDocTypeInfos = null;
                //缺省护理记录文档状态id时，索取护理记录单栏目下的第一个文档
                if (!GlobalMethods.Misc.IsEmptyString(szDocTypeId))
                {
                    shRet = SystemContext.Instance.DocTypeAccess.GetDocTypeInfo(szDocTypeId, ref szDocTypeInfo);
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        throw new Exception(string.Format("检索文档ID={0}的文档时失败!", szDocTypeId));
                        //return SystemConst.ReturnValue.FAILED;
                    }
                }
                else
                {
                    string szDocType = ServerData.DocTypeApplyEnv.NURSING_RECORD;
                    shRet = SystemContext.Instance.DocTypeAccess.GetDocTypeInfos(szDocType, ref lstDocTypeInfos);
                    if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                    {
                        throw new Exception("护理记录单栏下无文档!");
                        //return SystemConst.ReturnValue.FAILED;
                    }
                    if (shRet != ServerData.ExecuteResult.OK)
                    {
                        throw new Exception("检索护理记录单栏时失败!");
                        //return SystemConst.ReturnValue.FAILED;
                    }
                    foreach (DocTypeInfo doctype in lstDocTypeInfos)
                    {
                        if (doctype.IsFolder == false && doctype.IsValid == true)
                        {
                            szDocTypeInfo = doctype;
                            break;
                        }
                    }
                    if (szDocTypeInfo == null)
                    {
                        throw new Exception("护理记录单栏下无可用文档!");
                        //return SystemConst.ReturnValue.FAILED;
                    }
                }

                LogManager.Instance.WriteLog("文档类型检索成功");
                #endregion

                #region 添加数据
                NursingRecInfo nursingRecInfo = this.CreateNursingRecord(szUserInfo, szPatVisitInfo, szRecordTime);

                shRet = SystemContext.Instance.NurRecAccess.SaveNursingRec(nursingRecInfo);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    throw new Exception("保存护理记录失败!");
                    //return SystemConst.ReturnValue.FAILED;
                }
                LogManager.Instance.WriteLog("保存护理记录成功");
                NurDocInfo docinfo = this.CreateDocinfo(szDocTypeInfo, szPatVisitInfo, szUserInfo);
                //同步护理记录单信息
                docinfo.RecordID = nursingRecInfo.RecordID;
                docinfo.RecordTime = nursingRecInfo.RecordTime;

                //保存文档信息及文档状态信息
                shRet = SystemContext.Instance.DocumentAccess.SaveDoc(docinfo, null);
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    throw new Exception("无法保存病历文档,保存至服务器失败!");
                    //return SystemConst.ReturnValue.FAILED;
                }

                LogManager.Instance.WriteLog("保存护理文档成功");
                //保存当前病历摘要数据列表
                shRet = SystemContext.Instance.DocumentAccess.SaveSummaryData(docinfo.DocSetID, this.GetSummaryData(docinfo, lstKeyData));
                if (shRet != ServerData.ExecuteResult.OK)
                {
                    throw new Exception("病历保存成功,但摘要数据保存失败!");
                    //return SystemConst.ReturnValue.FAILED;
                }
                LogManager.Instance.WriteLog("保存摘要成功");
                #endregion
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLog(e.Message);
                throw e;
            }
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 创建一条新的护理记录信息
        /// </summary>
        /// <returns>护理记录信息</returns>
        private NursingRecInfo CreateNursingRecord(UserInfo loginUser, PatVisitInfo ActivePatient, DateTime szRecordTime)
        {
            NursingRecInfo nursingRecInfo = new NursingRecInfo();
            nursingRecInfo.CreatorID = loginUser.ID;
            nursingRecInfo.CreatorName = loginUser.Name;
            nursingRecInfo.CreateTime = SysTimeService.Instance.Now;
            nursingRecInfo.ModifierID = nursingRecInfo.CreatorID;
            nursingRecInfo.ModifierName = nursingRecInfo.CreatorName;
            nursingRecInfo.ModifyTime = nursingRecInfo.CreateTime;
            nursingRecInfo.PatientID = ActivePatient.PatientId;
            nursingRecInfo.VisitID = ActivePatient.VisitId;
            nursingRecInfo.SubID = ActivePatient.SubID;
            nursingRecInfo.Recorder1Name = nursingRecInfo.CreatorName;
            nursingRecInfo.Recorder1ID = nursingRecInfo.CreatorID;
            nursingRecInfo.Recorder2ID = nursingRecInfo.CreatorID;
            nursingRecInfo.Recorder2Name = nursingRecInfo.CreatorName;

            nursingRecInfo.RecordTime = szRecordTime;
            nursingRecInfo.RecordDate = nursingRecInfo.RecordTime.Date;
            nursingRecInfo.RecordTime = new DateTime(nursingRecInfo.RecordTime.Year
                , nursingRecInfo.RecordTime.Month, nursingRecInfo.RecordTime.Day
                , nursingRecInfo.RecordTime.Hour, nursingRecInfo.RecordTime.Minute, 0);

            nursingRecInfo.RecordID = nursingRecInfo.MakeRecordID();
            nursingRecInfo.PatientName = ActivePatient.PatientName;
            nursingRecInfo.WardCode = ActivePatient.WardCode;
            nursingRecInfo.WardName = ActivePatient.WardName;
            return nursingRecInfo;
        }

        /// <summary>
        /// 将当前DocumentInfo对象转换为数据库端文档信息对象
        /// </summary>
        /// <returns>MDSDBLib.MedDocInfo</returns>
        private NurDocInfo CreateDocinfo(DocTypeInfo docTypeInfo, PatVisitInfo patVisit, UserInfo userInfo)
        {
            NurDocInfo docInfo = new NurDocInfo();
            docInfo.ConfidCode = patVisit.ConfidCode;
            docInfo.CreatorID = userInfo.ID;
            docInfo.CreatorName = userInfo.Name;
            docInfo.WardCode = patVisit.WardCode;
            docInfo.WardName = patVisit.WardName;
            docInfo.DocTime = SysTimeService.Instance.Now;
            docInfo.DocTitle = docTypeInfo.DocTypeName;
            docInfo.DocTypeID = docTypeInfo.DocTypeID;
            docInfo.DocVersion = 1;
            docInfo.ModifierID = userInfo.ID;
            docInfo.ModifierName = userInfo.Name;
            docInfo.ModifyTime = docInfo.DocTime;
            docInfo.PatientID = patVisit.PatientId;
            docInfo.PatientName = patVisit.PatientName;
            docInfo.SignCode = string.Empty;
            docInfo.VisitID = patVisit.VisitId;
            docInfo.VisitTime = patVisit.VisitTime;
            docInfo.VisitType = patVisit.VisitType;
            docInfo.SubID = patVisit.SubID;

            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = this.CreateDocID(docInfo.DocTime, docInfo.DocVersion, patVisit.PatientId,
                            docTypeInfo.DocTypeID, ref szDocSetID, ref szDocID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                return null;
            }

            docInfo.DocID = szDocID;
            docInfo.DocSetID = szDocSetID;
            docInfo.OrderValue = -1;
            return docInfo;
        }

        /// <summary>
        /// 创建一个病历新的ID号,以及文档集ID号字符串
        /// </summary>
        /// <param name="dtDocTime">文档时间</param>
        /// <param name="nVersion">版本号</param>
        /// <param name="szPatientId">病人ID</param>
        /// <param name="szDocTypeId">文档类型ID</param>
        /// <param name="szDocSetID">返回的文档集编号</param>
        /// <param name="szDocID">返回文档编号</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateDocID(DateTime dtDocTime, int nVersion, string szPatientId, string szDocTypeId, ref string szDocSetID, ref string szDocID)
        {
            if (nVersion <= 0)
            {
                nVersion = 1;
            }
            szDocSetID = string.Format("{0}_{1}_{2}"
                , szPatientId.Trim()
                , szDocTypeId.Trim()
                , dtDocTime.ToString("yyyyMMddHHmmss")
            );
            szDocID = string.Format("{0}_{1}", szDocSetID, nVersion);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 获取当前表单编辑窗口中的摘要数据
        /// </summary>
        /// <returns>摘要数据列表</returns>
        private List<SummaryData> GetSummaryData(NurDocInfo nurDocInfo, List<KeyData> lstKeyData)
        {
            List<SummaryData> lstSummaryData = new List<SummaryData>();
            if (nurDocInfo == null)
                return lstSummaryData;

            if (lstKeyData == null)
                return lstSummaryData;

            IEnumerator<KeyData> enumerator = lstKeyData.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SummaryData summaryData = new SummaryData();
                summaryData.DocID = nurDocInfo.DocSetID;
                summaryData.DocTypeID = nurDocInfo.DocTypeID;
                summaryData.PatientID = nurDocInfo.PatientID;
                summaryData.VisitID = nurDocInfo.VisitID;
                summaryData.RecordID = nurDocInfo.RecordID;
                summaryData.WardCode = nurDocInfo.WardCode;

                summaryData.SubID = nurDocInfo.SubID;
                if (!GlobalMethods.Misc.IsEmptyString(enumerator.Current.Tag))
                    summaryData.SubID = enumerator.Current.Tag;
                summaryData.DataName = enumerator.Current.Name;
                summaryData.DataCode = enumerator.Current.Code;
                summaryData.DataType = enumerator.Current.Type;
                summaryData.DataUnit = enumerator.Current.Unit;
                if (enumerator.Current.RecordTime == null)
                    summaryData.DataTime = nurDocInfo.RecordTime;
                else
                    summaryData.DataTime = enumerator.Current.RecordTime.Value.AddSeconds(-enumerator.Current.RecordTime.Value.Second);
                if (enumerator.Current.Value != null)
                    summaryData.DataValue = enumerator.Current.Value.ToString();

                summaryData.Category = enumerator.Current.Category;
                summaryData.ContainsTime = enumerator.Current.ContainsTime;
                summaryData.Remarks = enumerator.Current.Remarks;
                lstSummaryData.Add(summaryData);
            }
            return lstSummaryData;
        }
    }
}