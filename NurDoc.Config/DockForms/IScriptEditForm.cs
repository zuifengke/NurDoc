// ***********************************************************
// ���������ù���ϵͳ,ģ�塢����ȵĽű��༭������Ľӿ�.
// Author : YangMingkun, Date : 2012-6-6
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Heren.NurDoc.Config.DockForms
{
    internal interface IScriptEditForm
    {
        /// <summary>
        /// ��ȡ��ǰ�ű��༭�������Ƿ��ѱ��ͷ�
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// ��ȡ��ǰ�ű��༭�������Ƿ��ѱ��޸�
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// ����ǰ�ű��༭�����ڹ�궨λ��ָ������
        /// </summary>
        /// <param name="line">�к�</param>
        /// <param name="column">�к�</param>
        void LocateTo(int line, int column);

        /// <summary>
        /// ����ǰ�ű��༭�����ڹ�궨λ��ָ�����ı�
        /// </summary>
        /// <param name="offset">����λ��</param>
        /// <param name="length">����</param>
        void LocateToText(int offset, int length);

        /// <summary>
        /// ��ȡ��ǰѡ�е��ı�
        /// </summary>
        /// <returns>ѡ�е��ı�</returns>
        string GetSelectedText();

        /// <summary>
        /// ������ָ�����ı�ƥ��������ı�
        /// </summary>
        /// <param name="szFindText">�ı�</param>
        /// <param name="bMatchCase">�Ƿ�ƥ���Сд</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        void FindText(string szFindText, bool bMatchCase);

        /// <summary>
        /// ������ģ��ű��в�����ָ�����ı�ƥ��������ı�
        /// </summary>
        /// <param name="szFindText">�ı�</param>
        /// <param name="bMatchCase">�Ƿ�ƥ���Сд</param>
        void FindTextInAllTemplet(string szFindText, bool bMatchCase);

        /// <summary>
        /// ���Ҳ��滻ָ�����ı�
        /// </summary>
        /// <param name="szFindText">�����ı�</param>
        /// <param name="szReplaceText">�滻�ı�</param>
        /// <param name="bMatchCase">�Ƿ�ƥ���Сд</param>
        /// <param name="bReplaceAll">�Ƿ��滻����</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        void ReplaceText(string szFindText, string szReplaceText, bool bMatchCase, bool bReplaceAll);
    }
}
