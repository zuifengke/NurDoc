// ***********************************************************
// 护理电子病历系统,
// 数据访问层接口封装之入院评估单等表单文档数据访问接口封装类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Heren.NurDoc.DAL;
using Heren.Common.Forms.Editor;

namespace Heren.NurDoc.Data
{
    public class EvaDocInfoService
    {
        private static EvaDocInfoService m_instance = null;

        /// <summary>
        /// 获取护理文档服务实例
        /// </summary>
        public static EvaDocInfoService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new EvaDocInfoService();
                return m_instance;
            }
        }

        private EvaDocInfoService()
        {
        }

        public short GetEvaDocInfo(string szEvaID, ref EvaDocInfo docInfo)
        {
            return SystemContext.Instance.EvaDocInfoAccess.GetEvaDocInfo(szEvaID, ref docInfo);
        }

        public short GetEvaDocInfos(string szWardCode, string szDocTypeID, ref List<EvaDocInfo> lstDocInfos)
        {
            return this.GetEvaDocInfos(szWardCode, szDocTypeID, DateTime.MinValue, DateTime.MaxValue, ref lstDocInfos);
        }

        public short GetEvaDocInfos(string szWardCode, string szDocTypeID
            , DateTime dtBeginTime, DateTime dtEndTime, ref List<EvaDocInfo> lstDocInfos)
        {
            short shRet = SystemContext.Instance.EvaDocInfoAccess.GetEvaDocInfos(szWardCode, szDocTypeID
                , dtBeginTime, dtEndTime, ref lstDocInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        public short GetItemDatasByID(string szEvaID, ref List<EvaDocItemInfo> lstEvaDatas)
        {
            short shRet = SystemContext.Instance.EvaDocInfoAccess.GetItemDatasByID(szEvaID, ref lstEvaDatas);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        public short UpdateEvaDocDatas(EvaDocInfo oldDocInfo, EvaDocInfo newDocInfo, List<EvaDocItemInfo> datas)
        {
            short shRet = SystemContext.Instance.EvaDocInfoAccess.UpdateEvaDocDatas(oldDocInfo, newDocInfo, datas);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        public short AddEvaDocDatas(EvaDocInfo newDocInfo, List<EvaDocItemInfo> datas)
        {
            short shRet = SystemContext.Instance.EvaDocInfoAccess.AddEvaDocDatas(newDocInfo, datas);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }

        public short DeleteEvaDocInfo(EvaDocInfo docInfo)
        {
            short shRet = SystemContext.Instance.EvaDocInfoAccess.DeleteEvaDocInfo(docInfo);
            if (shRet != ServerData.ExecuteResult.OK)
                return SystemConst.ReturnValue.FAILED;
            return SystemConst.ReturnValue.OK;
        }
    }
}