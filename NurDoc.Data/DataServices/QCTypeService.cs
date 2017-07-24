// ***********************************************************
// ������Ӳ���ϵͳ,
// ���ݷ��ʲ�ӿڷ�װ֮��������ȼ�¼�ȱ��ĵ����ݷ��ʽӿڷ�װ��.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class QCTypeService
    {
        private static QCTypeService m_instance = null;

        /// <summary>
        /// ��ȡ�����밲ȫ�����¼�ĵ�����ʵ��
        /// </summary>
        public static QCTypeService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new QCTypeService();
                return m_instance;
            }
        }

        private QCTypeService()
        {
        }

        #region"�����밲ȫ�����¼�������ݷ��ʽӿ�"
        /// <summary>
        /// ��ȡ���������밲ȫ����������Ϣ
        /// </summary>
        /// <param name="szApplyEnv">��Ӧ�û���</param>
        /// <param name="lstQCTypeInfos">���ص������밲ȫ����������Ϣ�б�</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetQCTypeInfos(string szApplyEnv, ref List<QCTypeInfo> lstQCTypeInfos)
        {
            short shRet = SystemContext.Instance.QCTypeAccess.GetQCTypeInfos(szApplyEnv, ref lstQCTypeInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// ��ȡָ�������밲ȫ�����¼����ID�ŵ������밲ȫ�����¼������Ϣ
        /// </summary>
        /// <param name="szQCTypeID">�����밲ȫ�����¼����ID��</param>
        /// <param name="qcTypeInfo">���ص������밲ȫ�����¼������Ϣ</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short GetQCTypeInfo(string szQCTypeID, ref QCTypeInfo qcTypeInfo)
        {
            return SystemContext.Instance.QCTypeAccess.GetQCTypeInfo(szQCTypeID, ref qcTypeInfo);
        }
        #endregion
    }
}
