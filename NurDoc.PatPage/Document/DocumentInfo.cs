// ***********************************************************
// �ĵ���Ϣ��.���ڴ�ŵ�ǰ�ͻ����ĵ������л�����Ϣ.
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
        /// ��ȡ�ĵ�ID
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
        }

        private string m_szDocTypeID = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ�����ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
        }

        private string m_szDocTitle = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ�����
        /// </summary>
        public string DocTitle
        {
            get { return this.m_szDocTitle; }
        }

        private string m_szDocSetID = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ������ĵ������
        /// </summary>
        public string DocSetID
        {
            get { return this.m_szDocSetID; }
        }

        private int m_nDocVersion = 1;

        /// <summary>
        /// ��ȡ�ĵ��汾
        /// </summary>
        public int DocVersion
        {
            get { return this.m_nDocVersion; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ�������ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ�������
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
        }

        private DateTime m_dtDocTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// ��ȡ�ĵ�����ʱ��
        /// </summary>
        public DateTime DocTime
        {
            get { return this.m_dtDocTime; }
        }

        private string m_szModifierID = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ�����޸���ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ�����޸���
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
        }

        private DateTime m_dtModifyTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// ��ȡ�ĵ�����޸�ʱ��
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// ��ȡ����Ψһ���
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// ��ȡ�����
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
        }

        private DateTime m_dtVisitTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// ��ȡ����ʱ��
        /// </summary>
        public DateTime VisitTime
        {
            get { return this.m_dtVisitTime; }
        }

        private string m_szVisitType = string.Empty;

        /// <summary>
        /// ��ȡ��������(OP-���IP-סԺ)
        /// </summary>
        public string VisitType
        {
            get { return this.m_szVisitType; }
        }

        private string m_szSubID = string.Empty;

        /// <summary>
        /// ��ȡ������ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// ��ȡ���˲�������
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// ��ȡ���˲�������
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
        }

        private string m_szSignCode = string.Empty;

        /// <summary>
        /// ��ȡǩ������
        /// </summary>
        public string SignCode
        {
            get { return this.m_szSignCode; }
        }

        private string m_szConfidCode = string.Empty;

        /// <summary>
        /// ��ȡ���ܵȼ�
        /// </summary>
        public string ConfidCode
        {
            get { return this.m_szConfidCode; }
        }

        private int m_nOrderValue = -1;

        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        public int OrderValue
        {
            get { return this.m_nOrderValue; }
        }

        private DocumentState m_eDocState = DocumentState.None;

        /// <summary>
        /// ��ȡ�ͻ����ĵ�״̬
        /// </summary>
        public DocumentState DocState
        {
            get { return this.m_eDocState; }
        }

        private string m_szRecordID = string.Empty;

        /// <summary>
        /// ��ȡ�ĵ������Ļ����¼ID
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
        }

        private DateTime m_dtRecordTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// ��ȡ�ĵ�ʵ�ʼ�¼ʱ��
        /// </summary>
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
        }

        private string m_caller = null;

        /// <summary>
        /// ��ȡ�����õ����ڸ��ӹ�ϵʱ,
        /// ��ǰ�ĵ������ĵ����ĸ��ؼ�����
        /// </summary>
        public string Caller
        {
            get { return this.m_caller; }
            set { this.m_caller = value; }
        }

        private byte[] m_byteDocData = null;

        /// <summary>
        /// ��ȡ�����ô�������ĵ�����
        /// </summary>
        public byte[] DocData
        {
            get { return this.m_byteDocData; }
            set { this.m_byteDocData = value; }
        }

        private List<SummaryData> m_summaryData = null;

        /// <summary>
        /// ��ȡ�����ô������ժҪ����
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
        /// ��ȡ�����ô�����������밲ȫ�����¼ժҪ����
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
        /// ��ȡ��ǰ�ĵ������ĵ��б�
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
        /// ��ȡ��һ���汾ID
        /// </summary>
        /// <returns>��һ���汾ID</returns>
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
        /// ��ȡǰһ���汾ID
        /// </summary>
        /// <returns>ǰһ���汾ID</returns>
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
        /// ��ȡ��һ���汾ID
        /// </summary>
        /// <returns>��һ���汾ID</returns>
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
        /// ��ȡ�޶�ǰ�ĵ�ID��
        /// </summary>
        /// <returns>�޶�ǰ�ĵ�ID��</returns>
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
        /// ���Ƶ�ǰDocumentInfo����
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
        /// ���Ƶ�ǰDocumentInfo����
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
        /// ����һ�������µ�ID��,�Լ��ĵ���ID���ַ���
        /// </summary>
        /// <param name="dtDocTime">�ĵ�ʱ��</param>
        /// <param name="nVersion">�汾��</param>
        /// <param name="szDocSetID">���ص��ĵ������</param>
        /// <param name="szDocID">�����ĵ����</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateDocID(DateTime dtDocTime, int nVersion, ref string szDocSetID, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(this.m_szPatientID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateDocID", "����IDΪ��!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (GlobalMethods.Misc.IsEmptyString(this.m_szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateDocID", "�ĵ�����IDΪ��!");
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
        /// ��ָ���ĵ����д���������һ���°汾��ID��
        /// </summary>
        /// <param name="szDocSetID">�ĵ������</param>
        /// <param name="nVersion">�汾��</param>
        /// <param name="szDocID">�����ĵ����</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateDocID(string szDocSetID, int nVersion, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateDocID", "������ĵ���IDΪ��!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (nVersion <= 0)
                nVersion = 1;
            szDocID = string.Format("{0}_{1}", szDocSetID, nVersion);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ����һ�������밲ȫ�����¼�µ�ID��,�Լ��ĵ���ID���ַ���
        /// </summary>
        /// <param name="dtDocTime">�ĵ�ʱ��</param>
        /// <param name="nVersion">�汾��</param>
        /// <param name="szDocSetID">���ص��ĵ������</param>
        /// <param name="szDocID">�����ĵ����</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateQCDocID(DateTime dtDocTime, int nVersion, ref string szDocSetID, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(this.m_szDocTypeID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateQCDocID", "�ĵ�����IDΪ��!");
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
        /// ���ָ����ID���Ƿ����ĵ���ID
        /// </summary>
        /// <param name="szDocID">�ĵ�ID</param>
        /// <returns>�Ƿ����ĵ���ID</returns>
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
        /// ��ȡָ����ID���ַ����а������ĵ���ID
        /// </summary>
        /// <param name="szDocID">�ĵ�ID</param>
        /// <returns>�ĵ���ID</returns>
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
        /// �����ĵ��޶�ʱ�����ĵ���Ϣ������޸Ĺ���
        /// </summary>
        /// <param name="userInfo">�û���Ϣ</param>
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
        /// �����ĵ�������ɺ�����ĵ���Ϣ������޸Ĺ���
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
        /// �����ĵ�������ɺ�����ĵ���Ϣ������޸Ĺ���
        /// </summary>
        /// <param name="szNewTitle">�ĵ��±���</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short ModifyDocumentTitle(string szNewTitle)
        {
            this.m_szDocTitle = szNewTitle;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// �޸Ĳ�������ʱ��
        /// </summary>
        /// <param name="dtDocTime">��������ʱ��</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short ModifyDocTime(DateTime dtDocTime)
        {
            this.m_dtDocTime = dtDocTime;
            this.m_dtModifyTime = dtDocTime;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// �޸Ĳ�����ҽ��ʵ�ʼ�¼ʱ��
        /// </summary>
        /// <param name="dtRecordTime">ʵ�ʼ�¼ʱ��</param>
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
        /// ����ǰDocumentInfo����ת��Ϊ���ݿ���ĵ���Ϣ����
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
        /// �Ӹ��������ݿ���ĵ���Ϣ����,����DocumentInfo����
        /// </summary>
        /// <param name="docInfo">���ݿ���ĵ���Ϣ����</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(NurDocInfo docInfo)
        {
            if (docInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "ҽ���ĵ���Ϣ����Ϊ��!");
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
        /// �Ӹ��������ݿ���ĵ���Ϣ����,����DocumentInfo����
        /// </summary>
        /// <param name="docInfo">���ݿ���ĵ���Ϣ����</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(QCDocInfo docInfo)
        {
            if (docInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "ҽ���ĵ���Ϣ����Ϊ��!");
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
        /// ���³�ʼ���ĵ���Ϣ�����е�����
        /// </summary>
        /// <param name="docInfo">���ݿ���ĵ���Ϣ����</param>
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
        /// �Ӳ��˾����Լ��ĵ����͵���Ϣ����һ��DocumentInfo����
        /// </summary>
        /// <param name="docTypeInfo">�ĵ�������Ϣ</param>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <param name="szRecordID">������¼ID��</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(DocTypeInfo docTypeInfo, PatVisitInfo patVisit, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�û���Ϣ����Ϊ��!");
                return null;
            }
            if (patVisit == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "������Ϣ����Ϊ��!");
                return null;
            }
            if (docTypeInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�ĵ�������Ϣ����Ϊ��!");
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
        /// �Ӳ��˾����Լ��ĵ����͵���Ϣ����һ��DocumentInfo����
        /// </summary>
        /// <param name="docTypeInfo">�ĵ�������Ϣ</param>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <param name="szRecordID">������¼ID��</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(DocTypeInfo docTypeInfo, PatVisitInfo patVisit, string szRecordID, DateTime creatTime)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�û���Ϣ����Ϊ��!");
                return null;
            }
            if (patVisit == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "������Ϣ����Ϊ��!");
                return null;
            }
            if (docTypeInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�ĵ�������Ϣ����Ϊ��!");
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
        /// �Ӳ��˾����Լ��ĵ����͵���Ϣ����һ��DocumentInfo����
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�������Ϣ</param>
        /// <param name="szDocTypeName">�ĵ�������Ϣ</param>
        /// <param name="patVisit">���˾�����Ϣ</param>
        /// <param name="szRecordID">������¼ID��</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(string szDocTypeID, string szDocTypeName, PatVisitInfo patVisit, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�û���Ϣ����Ϊ��!");
                return null;
            }
            if (patVisit == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "������Ϣ����Ϊ��!");
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
        /// �ӵ�ǰ�û��Լ��ĵ����͵���Ϣ����һ��DocumentInfo����
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�������Ϣ</param>
        /// <param name="szDocTypeName">�ĵ�������Ϣ</param>
        /// <param name="szRecordID">������¼ID��</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(string szDocTypeID, string szDocTypeName, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�û���Ϣ����Ϊ��!");
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
        /// �������밲ȫ�����¼�Լ��ĵ����͵���Ϣ����һ��DocumentInfo����
        /// </summary>
        /// <param name="qcTypeInfo">�ĵ�������Ϣ</param>
        /// <param name="szRecordID">������¼ID��</param>
        /// <returns>DocumentInfo</returns>
        public static DocumentInfo Create(QCTypeInfo qcTypeInfo, string szRecordID)
        {
            if (SystemContext.Instance.LoginUser == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�û���Ϣ����Ϊ��!");
                return null;
            }
            if (qcTypeInfo == null)
            {
                LogManager.Instance.WriteLog("DocumentInfo.Create", "�ĵ�������Ϣ����Ϊ��!");
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
        /// ����ǰDocumentInfo����ת��Ϊ���ݿ���ĵ���Ϣ����
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
        /// ��ָ���ĵ����д��������밲ȫ�����¼��һ���°汾��ID��
        /// </summary>
        /// <param name="szDocSetID">�ĵ������</param>
        /// <param name="nVersion">�汾��</param>
        /// <param name="szDocID">�����ĵ����</param>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        private short CreateQCDocID(string szDocSetID, int nVersion, ref string szDocID)
        {
            if (GlobalMethods.Misc.IsEmptyString(szDocSetID))
            {
                LogManager.Instance.WriteLog("DocumentInfo.CreateQCDocID", "������ĵ���IDΪ��!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (nVersion <= 0)
                nVersion = 1;
            szDocID = string.Format("{0}_{1}", szDocSetID, nVersion);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ���³�ʼ���ĵ���Ϣ�����е�����
        /// </summary>
        /// <param name="docInfo">���ݿ���ĵ���Ϣ����</param>
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
    /// ���ĵ����϶���
    /// </summary>
    public class ChildDocumentCollection : CollectionBase
    {
        private DocumentInfo m_docuemnt = null;

        /// <summary>
        /// �����ĵ��б��л�ȡָ�����������ĵ���Ϣ
        /// </summary>
        /// <param name="index">������</param>
        /// <returns>�ĵ���Ϣ</returns>
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
        /// �����ĵ��б��л�ȡָ���ĵ��������ĵ���Ϣ
        /// </summary>
        /// <param name="docid">�ĵ���ID��</param>
        /// <returns>DocumentInfo</returns>
        /// <remarks>ע��������ĵ�ID����Ҫ���ĵ�����</remarks>
        public DocumentInfo this[string docid]
        {
            get { return this.GetChildDocument(docid); }
        }

        /// <summary>
        /// �����ĵ��б��л�ȡָ���ĵ����ͺ͵����ߵ����ĵ���Ϣ
        /// </summary>
        /// <param name="doctypeid">�ĵ�����ID��</param>
        /// <param name="caller">�����߱�ʶ</param>
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
        /// �����ĵ��б��л�ȡָ���ĵ��������ĵ���Ϣ
        /// </summary>
        /// <param name="szDocID">�ĵ���ID��</param>
        /// <returns>DocumentInfo</returns>
        /// <remarks>ע��������ĵ�ID����Ҫ���ĵ�����</remarks>
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
        /// �����ĵ��б��л�ȡָ���ĵ����ͺ͵����ߵ����ĵ���Ϣ
        /// </summary>
        /// <param name="szDocTypeID">�ĵ�����ID��</param>
        /// <param name="szCaller">�����߱�ʶ</param>
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
