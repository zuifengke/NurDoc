// ***********************************************************
// 护理电子病历系统,护理申请数据保存更新处理类.
// Creator:YeChongchong  Date:2014-1-14
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.PatPage.Document;

namespace Heren.NurDoc.Frame.DockForms
{
    internal class QcExamineHandler
    {
        private static QcExamineHandler m_instance = null;

        /// <summary>
        /// 获取实例
        /// </summary>
        public static QcExamineHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new QcExamineHandler();
                return m_instance;
            }
        }

        private QcExamineHandler()
        {
        }

        /// <summary>
        /// 保存质控信息
        /// </summary>
        /// <param name="examineData">质控信息数据DataTable 或 字符串</param>
        /// <returns>保存成功与否</returns>
        public bool HandleQcExamineSave(object examineData)
        {
            if (examineData == null)
                return false;

            if (examineData is DataTable)
            {
                DataTable table = examineData as DataTable;
                if (table == null)
                    return false;
                List<QCExamineInfo> lstqcExamineinfo = new List<QCExamineInfo>();
                if (!getQcExamineInfoFromDataTable(table, ref lstqcExamineinfo))
                {
                    return false;
                }
                int intCounts = lstqcExamineinfo.Count;
                int intErrorCount = 0;
                foreach (QCExamineInfo qcExamineInfo in lstqcExamineinfo)
                {
                    QCExamineInfo newQcExamineinfo = new QCExamineInfo();
                    short shRet = QCExamineService.Instance.GetQcExamineInfo(qcExamineInfo.QcContentKey, qcExamineInfo.QcContentType, qcExamineInfo.PatientID, qcExamineInfo.VisitID, ref newQcExamineinfo);
                    if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                    {
                        shRet = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                        if (shRet != SystemConst.ReturnValue.OK)
                        {
                            intErrorCount += 1;
                            continue;
                        }
                    }
                    else if (shRet == ServerData.ExecuteResult.OK)
                    {
                        if (newQcExamineinfo.QcExamineStatus != "1")
                        {
                            short shRet1 = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                            if (shRet1 != SystemConst.ReturnValue.OK)
                            {
                                intErrorCount += 1;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        intErrorCount += 1;
                    }
                }
                StringBuilder sbError = new StringBuilder();
                sbError.AppendFormat("需保存或更新数据：{0}条", intCounts);
                sbError.AppendLine();
                sbError.AppendFormat("失败数据：{0}条", intErrorCount);
                MessageBox.Show(sbError.ToString());
            }
            else
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                QCExamineInfo newQcExamineinfo = new QCExamineInfo();
                if (!this.getQcExamineInfoFromString(examineData, ref qcExamineInfo))
                {
                    return false;
                }

                short shRet = QCExamineService.Instance.GetQcExamineInfo(qcExamineInfo.QcContentKey, qcExamineInfo.QcContentType, qcExamineInfo.PatientID, qcExamineInfo.VisitID, ref newQcExamineinfo);
                if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    short ShRet2 = QCExamineService.Instance.SaveQcExamine(qcExamineInfo);
                    if (ShRet2 != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.ShowError("病历质控数据新增失败");
                        return false;
                    }
                }
                else if (shRet == ServerData.ExecuteResult.OK)
                {
                    short shRet1 = QCExamineService.Instance.UpdateQcExamine(qcExamineInfo);
                    if (shRet1 != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.ShowError("病历质控数据修改失败");
                        return false;
                    }
                }
                else
                {
                    MessageBoxEx.ShowError("病历质控数据修改失败");
                    return false;
                }
            }
            return true;
        }

        public bool getQcExamineInfoFromDataTable(DataTable dt, ref List<QCExamineInfo> lstQcExamineInfo)
        {
            if (dt.Rows.Count <= 0)
            {
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                QCExamineInfo qcExamineInfo = new QCExamineInfo();
                qcExamineInfo.QcContentKey = dr["QcContentKey"].ToString();
                qcExamineInfo.QcContentType = dr["QcContentType"].ToString();
                qcExamineInfo.PatientID = dr["PatientID"].ToString();
                qcExamineInfo.PatientName = dr["PatientName"].ToString();
                qcExamineInfo.VisitID = dr["VisitID"].ToString();
                qcExamineInfo.WardCode = dr["WardCode"].ToString();
                qcExamineInfo.WardName = dr["WardName"].ToString();
                qcExamineInfo.QcExamineStatus = dr["QcExamineStatus"].ToString();
                qcExamineInfo.QcExamineContent = dr["QcExamineContent"].ToString();
                qcExamineInfo.QcExamineID = dr["QcExamineID"].ToString();
                qcExamineInfo.QcExamineName = dr["QcExamineName"].ToString();
                qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
                lstQcExamineInfo.Add(qcExamineInfo);
            }
            return true;
        }

        public bool getQcExamineInfoFromString(object examineData, ref QCExamineInfo qcExamineInfo)
        {
            string[] strParamDatas = examineData.ToString().Split(';');
            if (strParamDatas.Length != 11)
            {
                MessageBoxEx.Show("审核参数传递错误");
                return false;
            }

            qcExamineInfo.QcContentKey = strParamDatas[0];
            qcExamineInfo.QcContentType = strParamDatas[1];
            qcExamineInfo.PatientID = strParamDatas[2];
            qcExamineInfo.PatientName = strParamDatas[3];
            qcExamineInfo.VisitID = strParamDatas[4];
            qcExamineInfo.WardCode = strParamDatas[5];
            qcExamineInfo.WardName = strParamDatas[6];
            qcExamineInfo.QcExamineStatus = strParamDatas[7];
            qcExamineInfo.QcExamineContent = strParamDatas[8];
            qcExamineInfo.QcExamineID = strParamDatas[9];
            qcExamineInfo.QcExamineName = strParamDatas[10];
            qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
            return true;
        }

        public bool getNurDocInfoFromString(object strData, ref List<NurDocInfo> lstNurDocInfo)
        {
            string[] strParamDatas = strData.ToString().Split(';');
            if (strParamDatas.Length <= 0)
            {
                return false;
            }
            foreach (string strDocId in strParamDatas)
            {
                if (strDocId.Trim() == string.Empty)
                {
                    continue;
                }
                NurDocInfo nurDocInfo = new NurDocInfo();
                DocumentService.Instance.GetDocInfo(strDocId,ref nurDocInfo);
                lstNurDocInfo.Add(nurDocInfo);
            }
            return true;
        }

        private void PigeonholeDocStatus(String Doc_Id)
        {
            DocStatusInfo docStatusInfo = new DocStatusInfo();
            short shRet = DocumentService.Instance.GetDocStatusInfo(Doc_Id, ref docStatusInfo);
            if (shRet == ServerData.ExecuteResult.OK)
            {
                docStatusInfo.DocStatus = ServerData.DocStatus.ARCHIVED;
                DocumentService.Instance.ModifyOldDocStatus(ref docStatusInfo);
            }
        }
        
        /// <summary>
        /// 创建指控信息
        /// </summary>
        /// <param name="patVisitInfo">病人就诊信息</param>
        /// <param name="Key">内容标示</param>
        /// <param name="Type">内容类型</param>
        /// <returns>指控信息</returns>
        public QCExamineInfo Create(PatVisitInfo patVisitInfo, string Key, string Type)
        {
            QCExamineInfo qcExamineInfo = new QCExamineInfo();
            qcExamineInfo.QcContentKey = Key;
            qcExamineInfo.QcContentType = Type;
            qcExamineInfo.PatientID = patVisitInfo.PatientID;
            qcExamineInfo.PatientName = patVisitInfo.PatientName;
            qcExamineInfo.VisitID = patVisitInfo.VisitID;
            qcExamineInfo.WardCode = patVisitInfo.WardCode;
            qcExamineInfo.WardName = patVisitInfo.WardName;
            qcExamineInfo.QcExamineStatus = "1";
            qcExamineInfo.QcExamineContent = string.Empty;
            qcExamineInfo.QcExamineID = SystemContext.Instance.LoginUser.ID;
            qcExamineInfo.QcExamineName = SystemContext.Instance.LoginUser.Name;
            qcExamineInfo.QcExamineTime = SysTimeService.Instance.Now;
            return qcExamineInfo;
        }
    }
}
