// ***********************************************************
// ������Ӳ���ϵͳ,���ݷ��ʲ�ӿڷ�װ
// ֮�ṩ��PDA����Χϵͳ���ʱ����ݽӿڷ�װ��.
// ע�⣺������Ӳ���ϵͳ�����벻Ҫʹ�ô���.
// Creator:YangMingkun  Date:2013-7-1
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Heren.Common.Libraries;
using Heren.Common.Forms.Editor;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class FormDataService
    {
        private static FormDataService m_instance = null;

        /// <summary>
        /// ��ȡ�����ݷ��ʷ���ʵ��
        /// </summary>
        public static FormDataService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new FormDataService();
                return m_instance;
            }
        }

        private FormDataService()
        {
        }

        /// <summary>
        /// ��ȡָ��ģ���µ�ָ������(���ñ�GetFormData�ӿ�)
        /// </summary>
        /// <param name="szDocTypeID">���ݲ���</param>
        /// <param name="param">���ݲ���</param>
        /// <returns>DataTable</returns>
        public object GetFormData(string szDocTypeID, object param)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                LogManager.Instance.WriteLog("FormCache.GetFormData"
                    , string.Format("û���ҵ�{0}��Ӧ�ı�", szDocTypeID));
                return null;
            }

            byte[] byteTempletData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData))
            {
                LogManager.Instance.WriteLog("FormCache.GetFormData"
                 , string.Format("����{0}��Ӧ�ı�ʧ��", szDocTypeID));
                return null;
            }

            FormEditor formEditor = new FormEditor();
            formEditor.ExecuteQuery +=
                new ExecuteQueryEventHandler(this.FormEditor_ExecuteQuery);
            formEditor.QueryContext += 
                new QueryContextEventHandler(this.FormEditor_QueryContext);              
                
            if (!formEditor.Load(byteTempletData))
            {
                LogManager.Instance.WriteLog("FormCache.GetFormData"
                 , string.Format("����{0}��Ӧ�ı�ʧ��", szDocTypeID));
                return null;
            }
            return formEditor.GetFormData(param);
        }

        private void FormEditor_ExecuteQuery(object sender, ExecuteQueryEventArgs e)
        {
            DataSet result = null;
            short shRet = CommonService.Instance.ExecuteQuery(e.SQL, out result);
            e.Result = result;
            e.Success = (shRet == SystemConst.ReturnValue.OK);
        }

        private void FormEditor_QueryContext(object sender, QueryContextEventArgs e)
        {
            object value = e.Value;
            e.Success = SystemContext.Instance.GetSystemContext(e.Name, ref value);
            if (e.Success) e.Value = value;
        }

        /// <summary>
        /// ��ȡ�����˱������������б�
        /// </summary>
        /// <param name="dtPatientInfo">��PID��VID�Ĳ��˱�</param>
        /// <param name="dtAssessTaskInfo">���˱��������б�</param>
        /// <returns>0-�ɹ�;1-ʧ��</returns>
        public short GetPatientAssessTaskList(DataTable dtPatientInfo, ref DataTable dtAssessTaskInfo)
        {
            dtAssessTaskInfo = this.GetFormData("100120", dtPatientInfo) as DataTable;
            return (short)(dtAssessTaskInfo == null ? 1 : 0);
        }

        /// <summary>
        /// ��ȡ��ʿ����Ͻ�����Ļ����������һ��ͼ��Ϣ.
        /// ���а������ߵĻ�����Ϣ�������б������б�
        /// </summary>
        /// <param name="dtPatientList">��PID��VID�Ĳ����б�</param>
        /// <param name="dtNursingAssessInfo">����������Ȼ�����Ϣ������������б�</param>
        /// <returns>0-�ɹ�;1-ʧ��</returns>
        public short GetWardPatientsNursingAssess(DataTable dtPatientList, ref DataTable dtNursingAssessInfo)
        {
            dtNursingAssessInfo = this.GetFormData("200010", dtPatientList) as DataTable;
            return (short)(dtNursingAssessInfo == null ? 1 : 0);
        }
    }
}
