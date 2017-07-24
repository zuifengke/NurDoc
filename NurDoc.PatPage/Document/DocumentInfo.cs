// ***********************************************************
// 文档信息类.用于存放当前客户端文档的所有基本信息.
// Creator:YangMingkun  Date:2012-3-20
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;

namespace Heren.NurDoc.PatPage.Document
{
    public class DocumentInfo : ICloneable
    {
        private string m_szDocID = string.Empty;

        /// <summary>
        /// 获取文档ID
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
        }

        private string m_szDocTypeID = string.Empty;

        /// <summary>
        /// 获取文档类型ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
        }

        private string m_szDocTitle = string.Empty;

        /// <summary>
        /// 获取文档标题
        /// </summary>
        public string DocTitle
        {
            get { return this.m_szDocTitle; }
        }

        private string m_szDocSetID = string.Empty;

        /// <summary>
        /// 获取文档所属文档集编号
        /// </summary>
        public string DocSetID
        {
            get { return this.m_szDocSetID; }
        }

        private int m_nDocVersion = 1;

        /// <summary>
        /// 获取文档版本
        /// </summary>
        public int DocVersion
        {
            get { return this.m_nDocVersion; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// 获取文档创建者ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取文档创建者
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
        }

        private DateTime m_dtDocTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// 获取文档创建时间
        /// </summary>
        public DateTime DocTime
        {
            get { return this.m_dtDocTime; }
        }

        private string m_szModifierID = string.Empty;

        /// <summary>
        /// 获取文档最后修改者ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取文档最后修改者
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
        }

        private DateTime m_dtModifyTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// 获取文档最后修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取病人唯一编号
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// 获取病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取就诊号
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
        }

        private DateTime m_dtVisitTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// 获取就诊时间
        /// </summary>
        public DateTime VisitTime
        {
            get { return this.m_dtVisitTime; }
        }

        private string m_szVisitType = string.Empty;

        /// <summary>
        /// 获取就诊类型(OP-门诊，IP-住院)
        /// </summary>
        public string VisitType
        {
            get { return this.m_szVisitType; }
        }

        private string m_szSubID = string.Empty;

        /// <summary>
        /// 获取病人子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取病人病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取病人病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
        }

        private string m_szSignCode = string.Empty;

        /// <summary>
        /// 获取签名代码
        /// </summary>
        public string SignCode
        {
            get { return this.m_szSignCode; }
        }

        private string m_szConfidCode = string.Empty;

        /// <summary>
        /// 获取机密等级
        /// </summary>
        public string ConfidCode
        {
            get { return this.m_szConfidCode; }
        }

        private int m_nOrderValue = -1;

        /// <summary>
        /// 获取排序值
        /// </summary>
        public int OrderValue
        {
            get { return this.m_nOrderValue; }
        }

        private DocumentState m_eDocState = DocumentState.None;

        /// <summary>
        /// 获取客户端文档状态
        /// </summary>
        public DocumentState DocState
        {
            get { return this.m_eDocState; }
        }

        private string m_szRecordID = string.Empty;

        /// <summary>
        /// 获取文档关联的护理记录ID
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
        }

        private DateTime m_dtRecordTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// 获取文档实际记录时间
        /// </summary>
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
        }

        private string m_caller = null;

        /// <summary>
        /// 获取或设置当存在父子关系时,
        /// 当前文档被父文档的哪个控件调用
        /// </summary>
        public string Caller
        {
            get { return this.m_caller; }
            set { this.m_caller = value; }
        }

        private byte[] m_byteDocData = null;

        /// <summary>
        /// 获取或设置待保存的文档数据
        /// </summary>
        public byte[] DocData
        {
            get { return this.m_byteDocData; }
            set { this.m_byteDocData = value; }
        }

        private List<SummaryData> m_summaryData = null;

        /// <summary>
        /// 获取或设置待保存的摘要数据
        /// </summary>
        public List<SummaryData> SummaryData
        {
            get
            {
                if (this.m_summaryData == null)
                    this.m_summaryData = new List<SummaryData>();
                return this.m_summaryData;
            }
        }

        private List<QCSummaryData> m_qcSummaryData = null;

        /// <summary>
        /// 获取或设置待保存的质量与安全管理记录摘要数据
        /// </summary>
        public List<QCSummaryData> QCSummaryData
        {
            get
            {
                if (this.m_qcSummaryData == null)
                    this.m_qcSummaryData = new List<QCSummaryData>();
                return this.m_qcSummaryData;
            }
        }

        private ChildDocumentCollection m_childs = null;

        /// <summary>
        /// 获取当前文档的子文档列表
        /// </summary>
        public ChildDocumentCollection Childs
        {
            get
            {
                if (this.m_childs == null)
                    this.m_childs = new ChildDocumentCollection(this);
                return this.m_childs;
            }
        }

        private string m_szFirstVersionID = string.Empty;

        /// <summary>
        /// 获取第一个版本ID
        /// </summary>
        /// <returns>第一个版本ID</returns>
        internal string GetFirstVersionID()
        {
            if (!GlobalMethods.Misc.IsEmptyString(this.m_szFirstVersionID))
                return this.m_szFirstVersionID;
            int nFirstVersion = 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = this.CreateDocID(this.m_szDocSetID, nFirstVersion, ref szDocID);
            if (shRet == SystemConst.ReturnValue.OK)
                this.m_szFirstVersionID = szDocID;
            else
                this.m_szFirstVersionID = this.m_szDocID;
            return this.m_szFirstVersionID;
        }

        private string m_szPrevVersionID = string.Empty;

        /// <summary>
        /// 获取前一个版本ID
        /// </summary>
        /// <returns>前一个版本ID</returns>
        internal string GetPrevVersionID()
        {
            if (!GlobalMethods.Misc.IsEmptyString(this.m_szPrevVersionID))
                return this.m_szPrevVersionID;
            if (this.m_nDocVersion <= 1)
                return this.m_szDocID;
            int nPrevVersion = this.m_nDocVersion - 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = this.CreateDocID(this.m_szDocSetID, nPrevVersion, ref szDocID);
            if (shRet == SystemConst.ReturnValue.OK)
                this.m_szPrevVersionID = szDocID;
            else
                this.m_szPrevVersionID = this.m_szDocID;
            return this.m_szPrevVersionID;
        }

        private string m_szNextVersionID = string.Empty;

        /// <summary>
        /// 获取下一个版本ID
        /// </summary>
        /// <returns>下一个版本ID</returns>
        internal string GetNextVersionID()
        {
            if (!GlobalMethods.Misc.IsEmptyString(this.m_szNextVersionID))
                return this.m_szNextVersionID;
            int nNextVersion = this.m_nDocVersion + 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = this.CreateDocID(this.m_szDocSetID, nNextVersion, ref szDocID);
            if (shRet == SystemConst.ReturnValue.OK)
                this.m_szNextVersionID = szDocID;
            else
                this.m_szNextVersionID = this.m_szDocID;
            return this.m_szNextVersionID;
        }

        /// <summary>
        /// 获取修订前文档ID号
        /// </summary>
        /// <returns>修订前文档ID号</returns>
        internal string GetBeforeReviseID()
        {
            if (this.m_eDocState != DocumentState.Revise)
                return this.DocID;
            else
                return this.GetPrevVersionID();
        }

        public override string ToString()
        {
            return this.m_szDocTitle;
        }

        /// <summary>
        /// 复制当前DocumentInfo对象
        /// </summary>
        /// <returns>object</returns>
        public object Clone()
        {
            DocumentInfo document = new DocumentInfo();
            document.m_eDocState = this.m_eDocState;
            document.m_szCreatorID = this.m_szCreatorID;
            document.m_szCreatorName = this.m_szCreatorName;
            document.m_szWardCode = this.m_szWardCode;
            document.m_szWardName = this.m_szWardName;
            document.m_dtDocTime = this.m_dtDocTime;
            document.m_szDocTitle = this.m_szDocTitle;
            document.m_szDocTypeID = this.m_szDocTypeID;
            document.m_nDocVersion = this.m_nDocVersion;
            document.m_szModifierID = this.m_szModifierID;
            document.m_szModifierName = this.m_szModifierName;
            document.m_dtModifyTime = this.m_dtModifyTime;
            document.m_szPatientID = this.m_szPatientID;
            document.m_szPatientName = this.m_szPatientName;
            document.m_szVisitID = this.m_szVisitID;
            document.m_dtVisitTime = this.m_dtVisitTime;
            document.m_szVisitType = this.m_szVisitType;
            document.m_szSubID = this.m_szSubID;
            document.m_szDocID = this.m_szDocID;
            document.m_szDocSetID = this.m_szDocSetID;
            document.m_szSignCode = this.m_szSignCode;
            document.m_szConfidCode = this.m_szConfidCode;
            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;
            document.m_dtRecordTime = this.m_dtRecordTime;
            document.m_szRecordID = this.m_szRecordID;

            document.m_caller = this.m_caller;
            if (this.m_byteDocData != null)
                document.m_byteDocData = this.m_byteDocData.Clone() as byte[];
            foreach (DocumentInfo child in this.Childs)
                document.Childs.Add(child.Clone() as DocumentInfo);
            return document;
        }

        /// <summary>
        /// 复制当前DocumentInfo对象
        /// </summary>
        /// <returns>object</returns>
        public object QCClone()
        {
            DocumentInfo document = new DocumentInfo();
            document.m_eDocState = this.m_eDocState;
            document.m_szCreatorID = this.m_szCreatorID;
            document.m_szCreatorName = this.m_szCreatorName;
            document.m_szWardCode = this.m_szWardCode;
            document.m_szWardName = this.m_szWardName;
            document.m_dtDocTime = this.m_dtDocTime;
            document.m_szDocTitle = this.m_szDocTitle;
            document.m_szDocTypeID = this.m_szDocTypeID;
            document.m_nDocVersion = this.m_nDocVersion;
            document.m_szModifierID = this.m_szModifierID;
            document.m_szModifierName = this.m_szModifierName;
            document.m_dtModifyTime = this.m_dtModifyTime;
            document.m_szDocID = this.m_szDocID;
            document.m_szDocSetID = this.m_szDocSetID;
            document.m_szSignCode = this.m_szSignCode;
            document.m_szConfidCode = this.m_szConfidCode;
            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;
            document.m_dtRecordTime = this.m_dtRecordTime;
            document.m_szRecordID = this.m_szRecordID;
            document.m_caller = this.m_caller;
            if (this.m_byteDocData != null)
                document.m_byteDocData = this.m_byteDocData.Clone() as byte[];
            foreach (DocumentInfo child in this.Childs)
                document.Childs.Add(child.QCClone() as DocumentInfo);
            return document;
        }

        /// <summary>
        /// 创建一个病历新的ID号,以及文档集ID号字符串
        /// </summary>
        /// <param name="dtDocTime">文档时间</param>
        /// <param name="nVersion">版本号</param>
        /// <param name="szDocSetID">返回的文档集编号</param>
        /// <param name="szDocID">返回文档编号</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateDocID(DateTime dtDocTime, int nVersion, ref string szDocSetID, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(this.m_szPatientID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateDocID", "病人ID为空!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (GlobalMethods.Misc.IsEmptyString(this.m_szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateDocID", "文档类型ID为空!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (nVersion <= 0)
            {
                nVersion = 1;
            }
            szDocSetID = string.Format("{0}_{1}_{2}"
                , this.m_szPatientID.Trim()
                , this.m_szDocTypeID.Trim()
                , dtDocTime.ToString("yyyyMMddHHmmss")
            );
            szDocID = string.Format("{0}_{1}", szDocSetID, nVersion);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 在指定文档集中创建病历的一个新版本的ID号
        /// </summary>
        /// <param name="szDocSetID">文档集编号</param>
        /// <param name="nVersion">版本号</param>
        /// <param name="szDocID">返回文档编号</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateDocID(string szDocSetID, int nVersion, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateDocID", "传入的文档集ID为空!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (nVersion <= 0)
                nVersion = 1;
            szDocID = string.Format("{0}_{1}", szDocSetID, nVersion);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 创建一个质量与安全管理记录新的ID号,以及文档集ID号字符串
        /// </summary>
        /// <param name="dtDocTime">文档时间</param>
        /// <param name="nVersion">版本号</param>
        /// <param name="szDocSetID">返回的文档集编号</param>
        /// <param name="szDocID">返回文档编号</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateQCDocID(DateTime dtDocTime, int nVersion, ref string szDocSetID, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(this.m_szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateQCDocID", "文档类型ID为空!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (nVersion <= 0)
            {
                nVersion = 1;
            }
            szDocSetID = string.Format("{0}_{1}_{2}"
                , SystemContext.Instance.LoginUser.ID.Trim()
                , this.m_szDocTypeID.Trim()
                , dtDocTime.ToString("yyyyMMddHHmmss")
            );
            szDocID = string.Format("{0}_{1}", szDocSetID, nVersion);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 检查指定的ID号是否是文档集ID
        /// </summary>
        /// <param name="szDocID">文档ID</param>
        /// <returns>是否是文档集ID</returns>
        public static bool IsDocSetID(string szDocID)
        {
            if (string.IsNullOrEmpty(szDocID))
                return false;
            string szDocTimeFormat = "yyyyMMddHHmmss";
            int index = szDocID.LastIndexOf('_') + 1;
            if (szDocID.Length - index == szDocTimeFormat.Length)
                return true;
            return false;
        }

        /// <summary>
        /// 获取指定的ID号字符串中包含的文档集ID
        /// </summary>
        /// <param name="szDocID">文档ID</param>
        /// <returns>文档集ID</returns>
        public static string GetDocSetID(string szDocID)
        {
            if (string.IsNullOrEmpty(szDocID))
                return szDocID;
            if (DocumentInfo.IsDocSetID(szDocID))
                return szDocID;
            int index = szDocID.LastIndexOf('_');
            if (index <= 0)
                return szDocID;
            return szDocID.Substring(0, index);
        }

        /// <summary>
        /// 处理文档修订时对于文档信息对象的修改过程
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short HandleDocumentEdit(UserInfo userInfo)
        {
            if (userInfo == null || string.IsNullOrEmpty(userInfo.ID))
                return SystemConst.ReturnValue.FAILED;

            this.m_szFirstVersionID = string.Empty;
            this.m_szPrevVersionID = string.Empty;
            this.m_szNextVersionID = string.Empty;

            if (this.ModifierID.ToUpper() == userInfo.ID.ToUpper())
            {
                this.m_eDocState = DocumentState.Edit;
            }
            else
            {
                this.m_szDocID = this.GetNextVersionID();
                this.m_szModifierID = userInfo.ID;
                this.m_szModifierName = userInfo.Name;
                this.m_nDocVersion += 1;
                this.m_eDocState = DocumentState.Revise;
            }
            this.m_dtModifyTime = SysTimeService.Instance.Now;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 处理文档保存完成后对于文档信息对象的修改过程
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        internal short HandleDocumentSaved()
        {
            this.m_szFirstVersionID = string.Empty;
            this.m_szPrevVersionID = string.Empty;
            this.m_szNextVersionID = string.Empty;

            this.m_eDocState = DocumentState.View;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 处理文档保存完成后对于文档信息对象的修改过程
        /// </summary>
        /// <param name="szNewTitle">文档新标题</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short ModifyDocumentTitle(string szNewTitle)
        {
            this.m_szDocTitle = szNewTitle;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 修改病历生成时间
        /// </summary>
        /// <param name="dtDocTime">病历生成时间</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short ModifyDocTime(DateTime dtDocTime)
        {
            this.m_dtDocTime = dtDocTime;
            this.m_dtModifyTime = dtDocTime;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 修改病历中医生实际记录时间
        /// </summary>
        /// <param name="dtRecordTime">实际记录时间</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short ModifyRecordTime(DateTime dtRecordTime)
        {
            this.m_dtRecordTime = dtRecordTime;
            return SystemConst.ReturnValue.OK;
        }

        public void SaveDoc(byte[] byteDocData)
        {
            this.m_byteDocData = byteDocData;
        }

        /// <summary>
        /// 将当前DocumentInfo对象转换为数据库端文档信息对象
        /// </summary>
        /// <returns>MDSDBLib.MedDocInfo</returns>
        public NurDocInfo ToDocInfo()
        {
            NurDocInfo docInfo = new NurDocInfo();
            docInfo.ConfidCode = this.m_szConfidCode;
            docInfo.CreatorID = this.m_szCreatorID;
            docInfo.CreatorName = this.m_szCreatorName;
            docInfo.WardCode = this.m_szWardCode;
            docInfo.WardName = this.m_szWardName;
            docInfo.DocTime = this.m_dtDocTime;
            docInfo.DocTitle = this.m_szDocTitle;
            docInfo.DocTypeID = this.m_szDocTypeID;
            docInfo.DocVersion = this.m_nDocVersion;
            docInfo.ModifierID = this.m_szModifierID;
            docInfo.ModifierName = this.m_szModifierName;
            docInfo.ModifyTime = this.m_dtModifyTime;
            docInfo.PatientID = this.m_szPatientID;
            docInfo.PatientName = this.m_szPatientName;
            docInfo.SignCode = this.m_szSignCode;
            docInfo.VisitID = this.m_szVisitID;
            docInfo.VisitTime = this.m_dtVisitTime;
            docInfo.VisitType = this.m_szVisitType;
            docInfo.SubID = this.m_szSubID;
            docInfo.DocID = this.m_szDocID;
            docInfo.DocSetID = this.m_szDocSetID;
            docInfo.OrderValue = this.m_nOrderValue;
            docInfo.RecordTime = this.m_dtRecordTime;
            docInfo.RecordID = this.m_szRecordID;
            return docInfo;
        }

        /// <summary>
        /// 从给定的数据库端文档信息对象,创建DocumentInfo对象
        /// </summary>
        /// <param name="docInfo">数据库端文档信息对象</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(NurDocInfo docInfo)
        {
            if (docInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "医疗文档信息对象为空!");
                return null;
            }

            DocumentInfo document = new DocumentInfo();
            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;

            document.m_eDocState = DocumentState.View;
            document.m_szConfidCode = docInfo.ConfidCode;
            document.m_szCreatorID = docInfo.CreatorID;
            document.m_szCreatorName = docInfo.CreatorName;
            document.m_szWardCode = docInfo.WardCode;
            document.m_szWardName = docInfo.WardName;
            document.m_dtDocTime = docInfo.DocTime;
            document.m_szDocTitle = docInfo.DocTitle;
            document.m_szDocTypeID = docInfo.DocTypeID;
            document.m_nDocVersion = docInfo.DocVersion;
            document.m_szModifierID = docInfo.ModifierID;
            document.m_szModifierName = docInfo.ModifierName;
            document.m_dtModifyTime = docInfo.ModifyTime;
            document.m_szPatientID = docInfo.PatientID;
            document.m_szPatientName = docInfo.PatientName;
            document.m_szSignCode = docInfo.SignCode;
            document.m_szVisitID = docInfo.VisitID;
            document.m_dtVisitTime = docInfo.VisitTime;
            document.m_szVisitType = docInfo.VisitType;
            document.m_szSubID = docInfo.SubID;
            document.m_szDocID = docInfo.DocID;
            document.m_szDocSetID = docInfo.DocSetID;
            document.m_nOrderValue = docInfo.OrderValue;
            document.m_szRecordID = docInfo.RecordID;
            if (docInfo.RecordTime == docInfo.DefaultTime)
                document.m_dtRecordTime = docInfo.DocTime;
            else
                document.m_dtRecordTime = docInfo.RecordTime;
            return document;
        }

        /// <summary>
        /// 从给定的数据库端文档信息对象,创建DocumentInfo对象
        /// </summary>
        /// <param name="docInfo">数据库端文档信息对象</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(QCDocInfo docInfo)
        {
            if (docInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "医疗文档信息对象为空!");
                return null;
            }

            DocumentInfo document = new DocumentInfo();
            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;

            document.m_eDocState = DocumentState.View;
            document.m_szConfidCode = docInfo.ConfidCode;
            document.m_szCreatorID = docInfo.CreatorID;
            document.m_szCreatorName = docInfo.CreatorName;
            document.m_szWardCode = docInfo.WardCode;
            document.m_szWardName = docInfo.WardName;
            document.m_dtDocTime = docInfo.DocTime;
            document.m_szDocTitle = docInfo.DocTitle;
            document.m_szDocTypeID = docInfo.DocTypeID;
            document.m_nDocVersion = docInfo.DocVersion;
            document.m_szModifierID = docInfo.ModifierID;
            document.m_szModifierName = docInfo.ModifierName;
            document.m_dtModifyTime = docInfo.ModifyTime;
            document.m_szSignCode = docInfo.SignCode;
            document.m_szDocID = docInfo.DocID;
            document.m_szDocSetID = docInfo.DocSetID;
            document.m_nOrderValue = docInfo.OrderValue;
            document.m_szRecordID = docInfo.RecordID;
            if (docInfo.RecordTime == docInfo.DefaultTime)
                document.m_dtRecordTime = docInfo.DocTime;
            else
                document.m_dtRecordTime = docInfo.RecordTime;
            return document;
        }

        /// <summary>
        /// 重新初始化文档信息对象中的数据
        /// </summary>
        /// <param name="docInfo">数据库端文档信息对象</param>
        public void ReInitialize(NurDocInfo docInfo)
        {
            if (docInfo == null)
                return;
            this.m_szFirstVersionID = string.Empty;
            this.m_szPrevVersionID = string.Empty;
            this.m_szNextVersionID = string.Empty;

            this.m_eDocState = DocumentState.View;
            this.m_szConfidCode = docInfo.ConfidCode;
            this.m_szCreatorID = docInfo.CreatorID;
            this.m_szCreatorName = docInfo.CreatorName;
            this.m_szWardCode = docInfo.WardCode;
            this.m_szWardName = docInfo.WardName;
            this.m_dtDocTime = docInfo.DocTime;
            this.m_szDocTitle = docInfo.DocTitle;
            this.m_szDocTypeID = docInfo.DocTypeID;
            this.m_nDocVersion = docInfo.DocVersion;
            this.m_szModifierID = docInfo.ModifierID;
            this.m_szModifierName = docInfo.ModifierName;
            this.m_dtModifyTime = docInfo.ModifyTime;
            this.m_szPatientID = docInfo.PatientID;
            this.m_szPatientName = docInfo.PatientName;
            this.m_szSignCode = docInfo.SignCode;
            this.m_szVisitID = docInfo.VisitID;
            this.m_dtVisitTime = docInfo.VisitTime;
            this.m_szVisitType = docInfo.VisitType;
            this.m_szSubID = docInfo.SubID;
            this.m_szDocID = docInfo.DocID;
            this.m_szDocSetID = docInfo.DocSetID;
            this.m_nOrderValue = docInfo.OrderValue;
            this.m_szRecordID = docInfo.RecordID;
            this.m_dtRecordTime = docInfo.RecordTime;
        }

        /// <summary>
        /// 从病人就诊以及文档类型等信息创建一个DocumentInfo对象
        /// </summary>
        /// <param name="docTypeInfo">文档类型信息</param>
        /// <param name="patVisit">病人就诊信息</param>
        /// <param name="szRecordID">关联记录ID号</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(DocTypeInfo docTypeInfo, PatVisitInfo patVisit, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "用户信息对象为空!");
                return null;
            }
            if (patVisit == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "病人信息对象为空!");
                return null;
            }
            if (docTypeInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "文档类型信息对象为空!");
                return null;
            }
            DocumentInfo document = new DocumentInfo();
            document.m_szPatientID = patVisit.PatientID;
            document.m_szDocTypeID = docTypeInfo.DocTypeID;
            document.m_dtDocTime = SysTimeService.Instance.Now;
            document.m_nDocVersion = 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = document.CreateDocID(document.DocTime, document.DocVersion, ref szDocSetID, ref szDocID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                document = null;
                return null;
            }
            document.m_szDocID = szDocID;
            document.m_szDocSetID = szDocSetID;

            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;

            document.m_eDocState = DocumentState.New;
            document.m_szCreatorID = SystemContext.Instance.LoginUser.ID;
            document.m_szCreatorName = SystemContext.Instance.LoginUser.Name;
            document.m_szWardCode = patVisit.WardCode;
            document.m_szWardName = patVisit.WardName;
            document.m_szDocTitle = docTypeInfo.DocTypeName;
            document.m_szModifierID = document.CreatorID;
            document.m_szModifierName = document.CreatorName;
            document.m_dtModifyTime = document.DocTime;
            document.m_szPatientName = patVisit.PatientName;
            document.m_szVisitID = patVisit.VisitID;
            document.m_dtVisitTime = patVisit.VisitTime;
            document.m_szVisitType = patVisit.VisitType;
            document.m_szSubID = patVisit.SubID;
            document.m_dtRecordTime = document.DocTime;
            document.m_szRecordID = szRecordID;
            document.m_szSignCode = string.Empty;
            document.m_szConfidCode = patVisit.ConfidCode;
            return document;
        }

        /// <summary>
        /// 从病人就诊以及文档类型等信息创建一个DocumentInfo对象
        /// </summary>
        /// <param name="docTypeInfo">文档类型信息</param>
        /// <param name="patVisit">病人就诊信息</param>
        /// <param name="szRecordID">关联记录ID号</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(DocTypeInfo docTypeInfo, PatVisitInfo patVisit, string szRecordID, DateTime creatTime)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "用户信息对象为空!");
                return null;
            }
            if (patVisit == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "病人信息对象为空!");
                return null;
            }
            if (docTypeInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "文档类型信息对象为空!");
                return null;
            }
            DocumentInfo document = new DocumentInfo();
            document.m_szPatientID = patVisit.PatientID;
            document.m_szDocTypeID = docTypeInfo.DocTypeID;
            document.m_dtDocTime = creatTime;
            document.m_nDocVersion = 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = document.CreateDocID(document.DocTime, document.DocVersion, ref szDocSetID, ref szDocID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                document = null;
                return null;
            }
            document.m_szDocID = szDocID;
            document.m_szDocSetID = szDocSetID;

            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;

            document.m_eDocState = DocumentState.New;
            document.m_szCreatorID = SystemContext.Instance.LoginUser.ID;
            document.m_szCreatorName = SystemContext.Instance.LoginUser.Name;
            document.m_szWardCode = patVisit.WardCode;
            document.m_szWardName = patVisit.WardName;
            document.m_szDocTitle = docTypeInfo.DocTypeName;
            document.m_szModifierID = document.CreatorID;
            document.m_szModifierName = document.CreatorName;
            document.m_dtModifyTime = document.DocTime;
            document.m_szPatientName = patVisit.PatientName;
            document.m_szVisitID = patVisit.VisitID;
            document.m_dtVisitTime = patVisit.VisitTime;
            document.m_szVisitType = patVisit.VisitType;
            document.m_szSubID = patVisit.SubID;
            document.m_dtRecordTime = document.DocTime;
            document.m_szRecordID = szRecordID;
            document.m_szSignCode = string.Empty;
            document.m_szConfidCode = patVisit.ConfidCode;
            return document;
        }

        /// <summary>
        /// 从病人就诊以及文档类型等信息创建一个DocumentInfo对象
        /// </summary>
        /// <param name="szDocTypeID">文档类型信息</param>
        /// <param name="szDocTypeName">文档类型信息</param>
        /// <param name="patVisit">病人就诊信息</param>
        /// <param name="szRecordID">关联记录ID号</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(string szDocTypeID, string szDocTypeName, PatVisitInfo patVisit, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "用户信息对象为空!");
                return null;
            }
            if (patVisit == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "病人信息对象为空!");
                return null;
            }
            DocumentInfo document = new DocumentInfo();
            document.m_szPatientID = patVisit.PatientID;
            document.m_szDocTypeID = szDocTypeID;
            document.m_dtDocTime = SysTimeService.Instance.Now;
            document.m_nDocVersion = 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = document.CreateDocID(document.DocTime, document.DocVersion, ref szDocSetID, ref szDocID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                document = null;
                return null;
            }
            document.m_szDocID = szDocID;
            document.m_szDocSetID = szDocSetID;

            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;

            document.m_eDocState = DocumentState.New;
            document.m_szCreatorID = SystemContext.Instance.LoginUser.ID;
            document.m_szCreatorName = SystemContext.Instance.LoginUser.Name;
            document.m_szWardCode = patVisit.WardCode;
            document.m_szWardName = patVisit.WardName;
            document.m_szDocTitle = szDocTypeName;
            document.m_szModifierID = document.CreatorID;
            document.m_szModifierName = document.CreatorName;
            document.m_dtModifyTime = document.DocTime;
            document.m_szPatientName = patVisit.PatientName;
            document.m_szVisitID = patVisit.VisitID;
            document.m_dtVisitTime = patVisit.VisitTime;
            document.m_szVisitType = patVisit.VisitType;
            document.m_szSubID = patVisit.SubID;
            document.m_dtRecordTime = document.DocTime;
            document.m_szRecordID = szRecordID;
            document.m_szSignCode = string.Empty;
            document.m_szConfidCode = patVisit.ConfidCode;
            return document;
        }

        /// <summary>
        /// 从当前用户以及文档类型等信息创建一个DocumentInfo对象
        /// </summary>
        /// <param name="szDocTypeID">文档类型信息</param>
        /// <param name="szDocTypeName">文档类型信息</param>
        /// <param name="szRecordID">关联记录ID号</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(string szDocTypeID, string szDocTypeName, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "用户信息对象为空!");
                return null;
            }
            
            DocumentInfo document = new DocumentInfo();
            document.m_szDocTypeID = szDocTypeID;
            document.m_dtDocTime = SysTimeService.Instance.Now;
            document.m_nDocVersion = 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = document.CreateQCDocID(document.DocTime, document.DocVersion, ref szDocSetID, ref szDocID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                document = null;
                return null;
            }
            document.m_szDocID = szDocID;
            document.m_szDocSetID = szDocSetID;

            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;

            document.m_eDocState = DocumentState.New;
            document.m_szCreatorID = SystemContext.Instance.LoginUser.ID;
            document.m_szCreatorName = SystemContext.Instance.LoginUser.Name;
            document.m_szWardCode = SystemContext.Instance.LoginUser.WardCode;
            document.m_szWardName = SystemContext.Instance.LoginUser.WardName;
            document.m_szDocTitle = szDocTypeName;
            document.m_szModifierID = document.CreatorID;
            document.m_szModifierName = document.CreatorName;
            document.m_dtModifyTime = document.DocTime;
            document.m_dtRecordTime = document.DocTime;
            document.m_szRecordID = szRecordID;
            document.m_szSignCode = string.Empty;
            document.m_szConfidCode = string.Empty;//patVisit.ConfidCode;
            return document;
        }

        /// <summary>
        /// 从质量与安全管理记录以及文档类型等信息创建一个DocumentInfo对象
        /// </summary>
        /// <param name="qcTypeInfo">文档类型信息</param>
        /// <param name="szRecordID">关联记录ID号</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(QCTypeInfo qcTypeInfo, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "用户信息对象为空!");
                return null;
            }
            if (qcTypeInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "文档类型信息对象为空!");
                return null;
            }
            DocumentInfo document = new DocumentInfo();
            document.m_szDocTypeID = qcTypeInfo.QCTypeID;
            document.m_dtDocTime = SysTimeService.Instance.Now;
            document.m_nDocVersion = 1;
            string szDocSetID = string.Empty;
            string szDocID = string.Empty;
            short shRet = document.CreateQCDocID(document.DocTime, document.DocVersion, ref szDocSetID, ref szDocID);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                document = null;
                return null;
            }
            document.m_szDocID = szDocID;
            document.m_szDocSetID = szDocSetID;
            document.m_szFirstVersionID = string.Empty;
            document.m_szPrevVersionID = string.Empty;
            document.m_szNextVersionID = string.Empty;
            document.m_eDocState = DocumentState.New;
            document.m_szCreatorID = SystemContext.Instance.LoginUser.ID;
            document.m_szCreatorName = SystemContext.Instance.LoginUser.Name;
            document.m_szWardCode = SystemContext.Instance.LoginUser.WardCode;
            document.m_szWardName = SystemContext.Instance.LoginUser.WardName;
            document.m_szDocTitle = qcTypeInfo.QCTypeName;
            document.m_szModifierID = document.CreatorID;
            document.m_szModifierName = document.CreatorName;
            document.m_dtModifyTime = document.DocTime;
            document.m_dtRecordTime = document.DocTime;
            document.m_szRecordID = szRecordID;
            document.m_szSignCode = string.Empty;
            document.m_szConfidCode = string.Empty;// patVisit.ConfidCode;
            return document;
        }

        /// <summary>
        /// 将当前DocumentInfo对象转换为数据库端文档信息对象
        /// </summary>
        /// <returns>MDSDBLib.MedQSMInfo</returns>
        public QCDocInfo ToQCInfo()
        {
            QCDocInfo docInfo = new QCDocInfo();
            docInfo.ConfidCode = this.m_szConfidCode;
            docInfo.CreatorID = this.m_szCreatorID;
            docInfo.CreatorName = this.m_szCreatorName;
            docInfo.WardCode = this.m_szWardCode;
            docInfo.WardName = this.m_szWardName;
            docInfo.DocTime = this.m_dtDocTime;
            docInfo.DocTitle = this.m_szDocTitle;
            docInfo.DocTypeID = this.m_szDocTypeID;
            docInfo.DocVersion = this.m_nDocVersion;
            docInfo.ModifierID = this.m_szModifierID;
            docInfo.ModifierName = this.m_szModifierName;
            docInfo.ModifyTime = this.m_dtModifyTime;
            docInfo.SignCode = this.m_szSignCode;
            docInfo.DocID = this.m_szDocID;
            docInfo.DocSetID = this.m_szDocSetID;
            docInfo.OrderValue = this.m_nOrderValue;
            docInfo.RecordTime = this.m_dtRecordTime;
            docInfo.RecordID = this.m_szRecordID;
            return docInfo;
        }

        /// <summary>
        /// 在指定文档集中创建质量与安全管理记录的一个新版本的ID号
        /// </summary>
        /// <param name="szDocSetID">文档集编号</param>
        /// <param name="nVersion">版本号</param>
        /// <param name="szDocID">返回文档编号</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateQCDocID(string szDocSetID, int nVersion, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateQCDocID", "传入的文档集ID为空!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (nVersion <= 0)
                nVersion = 1;
            szDocID = string.Format("{0}_{1}", szDocSetID, nVersion);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 重新初始化文档信息对象中的数据
        /// </summary>
        /// <param name="docInfo">数据库端文档信息对象</param>
        public void ReInitializeQC(QCDocInfo docInfo)
        {
            if (docInfo == null)
                return;
            this.m_szFirstVersionID = string.Empty;
            this.m_szPrevVersionID = string.Empty;
            this.m_szNextVersionID = string.Empty;

            this.m_eDocState = DocumentState.View;
            this.m_szConfidCode = docInfo.ConfidCode;
            this.m_szCreatorID = docInfo.CreatorID;
            this.m_szCreatorName = docInfo.CreatorName;
            this.m_szWardCode = docInfo.WardCode;
            this.m_szWardName = docInfo.WardName;
            this.m_dtDocTime = docInfo.DocTime;
            this.m_szDocTitle = docInfo.DocTitle;
            this.m_szDocTypeID = docInfo.DocTypeID;
            this.m_nDocVersion = docInfo.DocVersion;
            this.m_szModifierID = docInfo.ModifierID;
            this.m_szModifierName = docInfo.ModifierName;
            this.m_dtModifyTime = docInfo.ModifyTime;
            this.m_szDocID = docInfo.DocID;
            this.m_szDocSetID = docInfo.DocSetID;
            this.m_nOrderValue = docInfo.OrderValue;
            this.m_szRecordID = docInfo.RecordID;
            this.m_dtRecordTime = docInfo.RecordTime;
        }
    }
    

    /// <summary>
    /// 子文档集合对象
    /// </summary>
    public class ChildDocumentCollection : CollectionBase
    {
        private DocumentInfo m_docuemnt = null;

        /// <summary>
        /// 从子文档列表中获取指定索引的子文档信息
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns>文档信息</returns>
        public DocumentInfo this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    return null;
                return base.InnerList[index] as DocumentInfo;
            }
        }

        /// <summary>
        /// 从子文档列表中获取指定文档集的子文档信息
        /// </summary>
        /// <param name="docid">文档集ID号</param>
        /// <returns>DocumentInfo</returns>
        /// <remarks>注意这里的文档ID号需要用文档集号</remarks>
        public DocumentInfo this[string docid]
        {
            get { return this.GetChildDocument(docid); }
        }

        /// <summary>
        /// 从子文档列表中获取指定文档类型和调用者的子文档信息
        /// </summary>
        /// <param name="doctypeid">文档类型ID号</param>
        /// <param name="caller">调用者标识</param>
        /// <returns>DocumentInfo</returns>
        public DocumentInfo this[string doctypeid, string caller]
        {
            get { return this.GetChildDocument(doctypeid, caller); }
        }

        public ChildDocumentCollection(DocumentInfo document)
        {
            this.m_docuemnt = document;
        }

        public override string ToString()
        {
            return string.Format("Count={0}", this.Count);
        }

        public void Add(DocumentInfo document)
        {
            if (document == null)
                return;
            int index = 0;
            while (index < this.Count)
            {
                DocumentInfo current = this[index++];
                if (current.DocID == document.DocID)
                {
                    this.Remove(current);
                    break;
                }
            }
            base.InnerList.Add(document);
        }

        public void Remove(DocumentInfo document)
        {
            base.InnerList.Remove(document);
        }

        /// <summary>
        /// 从子文档列表中获取指定文档集的子文档信息
        /// </summary>
        /// <param name="szDocID">文档集ID号</param>
        /// <returns>DocumentInfo</returns>
        /// <remarks>注意这里的文档ID号需要用文档集号</remarks>
        public DocumentInfo GetChildDocument(string szDocID)
        {
            foreach (DocumentInfo document in this)
            {
                if (document.DocSetID == szDocID)
                    return document;
            }
            return null;
        }

        /// <summary>
        /// 从子文档列表中获取指定文档类型和调用者的子文档信息
        /// </summary>
        /// <param name="szDocTypeID">文档类型ID号</param>
        /// <param name="szCaller">调用者标识</param>
        /// <returns>DocumentInfo</returns>
        public DocumentInfo GetChildDocument(string szDocTypeID, string szCaller)
        {
            foreach (DocumentInfo document in this)
            {
                if (document.DocTypeID == szDocTypeID && document.Caller == szCaller)
                    return document;
            }
            return null;
        }
    }
}
