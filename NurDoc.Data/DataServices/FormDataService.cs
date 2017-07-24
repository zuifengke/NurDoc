// ***********************************************************
// 护理电子病历系统,数据访问层接口封装
// 之提供给PDA等外围系统访问表单数据接口封装类.
// 注意：护理电子病历系统本身请不要使用此类.
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
        /// 获取表单数据访问服务实例
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
        /// 获取指定模板下的指定数据(调用表单GetFormData接口)
        /// </summary>
        /// <param name="szDocTypeID">数据参数</param>
        /// <param name="param">数据参数</param>
        /// <returns>DataTable</returns>
        public object GetFormData(string szDocTypeID, object param)
        {
            DocTypeInfo docTypeInfo = FormCache.Instance.GetDocTypeInfo(szDocTypeID);
            if (docTypeInfo == null)
            {
                LogManager.Instance.WriteLog("FormCache.GetFormData"
                    , string.Format("没有找到{0}对应的表单", szDocTypeID));
                return null;
            }

            byte[] byteTempletData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteTempletData))
            {
                LogManager.Instance.WriteLog("FormCache.GetFormData"
                 , string.Format("下载{0}对应的表单失败", szDocTypeID));
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
                 , string.Format("加载{0}对应的表单失败", szDocTypeID));
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
        /// 获取单病人本班评估任务列表
        /// </summary>
        /// <param name="dtPatientInfo">含PID和VID的病人表</param>
        /// <param name="dtAssessTaskInfo">病人本班任务列表</param>
        /// <returns>0-成功;1-失败</returns>
        public short GetPatientAssessTaskList(DataTable dtPatientInfo, ref DataTable dtAssessTaskInfo)
        {
            dtAssessTaskInfo = this.GetFormData("100120", dtPatientInfo) as DataTable;
            return (short)(dtAssessTaskInfo == null ? 1 : 0);
        }

        /// <summary>
        /// 获取护士所管辖病区的患者评估情况一览图信息.
        /// 其中包括患者的基本信息、任务列表、风险列表
        /// </summary>
        /// <param name="dtPatientList">含PID和VID的病人列表</param>
        /// <param name="dtNursingAssessInfo">含病人年龄等基本信息的任务与风险列表</param>
        /// <returns>0-成功;1-失败</returns>
        public short GetWardPatientsNursingAssess(DataTable dtPatientList, ref DataTable dtNursingAssessInfo)
        {
            dtNursingAssessInfo = this.GetFormData("200010", dtPatientList) as DataTable;
            return (short)(dtNursingAssessInfo == null ? 1 : 0);
        }
    }
}
