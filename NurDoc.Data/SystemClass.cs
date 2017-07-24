// ***********************************************************
// ��װ�ͻ��˽�������ڹ�������ͼ���
// Creator:YangMingkun  Date:2012-3-20
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Collections.Generic;

namespace Heren.NurDoc.Data
{
    #region "enum"
    /// <summary>
    /// �ͻ����ĵ�״̬
    /// </summary>
    public enum DocumentState
    {
        /// <summary>
        /// δ֪״̬(0)
        /// </summary>
        None = 0,

        /// <summary>
        /// �½�״̬(1)
        /// </summary>
        New = 1,

        /// <summary>
        /// �༭״̬(2)
        /// </summary>
        Edit = 2,

        /// <summary>
        /// �޶�״̬(3)
        /// </summary>
        Revise = 3,

        /// <summary>
        /// ���״̬(4)
        /// </summary>
        View = 4
    }

    /// <summary>
    /// �����б���ͼģʽ
    /// </summary>
    public enum PatientView
    {
        /// <summary>
        /// ϸ��(0)
        /// </summary>
        DetailCard = 0,

        /// <summary>
        /// ��(1)
        /// </summary>
        SimpleCard = 1,

        /// <summary>
        /// �����б�(2)
        /// </summary>
        PatientList = 2
    }
    #endregion

    #region "class"
    #endregion
}
