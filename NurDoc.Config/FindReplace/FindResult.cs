// ***********************************************************
// ���������ù���ϵͳ,�ű��༭���ű��ı����ҽ����Ϣ����
// Author : YangMingkun, Date : 2013-5-4
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Text;
using System.Drawing;
using Heren.Common.TextEditor.Document;

namespace Heren.NurDoc.Config.FindReplace
{
    internal class FindResult
    {
        private int m_offset = 0;

        /// <summary>
        /// ��ȡ��ǰ���ҵ��ַ�����
        /// </summary>
        public int Offset
        {
            get { return this.m_offset; }
        }

        private int m_length = 0;

        /// <summary>
        /// ��ȡ��ǰ���ҵ����ַ�����
        /// </summary>
        public int Length
        {
            get { return this.m_length; }
        }

        private int m_line = 0;

        /// <summary>
        /// ��ȡ��ǰ���ҵ����ַ�������
        /// </summary>
        public int Line
        {
            get { return this.m_line; }
        }

        private string m_text = string.Empty;

        /// <summary>
        /// ��ȡ��ǰ���ҵ����ı���
        /// </summary>
        public string Text
        {
            get { return this.m_text; }
        }

        private string m_templetID = string.Empty;

        /// <summary>
        /// ��ȡ��ѯ����ĵ�ID��
        /// </summary>
        public string TempletID
        {
            get { return this.m_templetID; }
        }

        private string m_templetName = string.Empty;

        /// <summary>
        /// ��ȡ��ѯ����ĵ���
        /// </summary>
        public string TempletName
        {
            get { return this.m_templetName; }
        }

        private string m_fileType = string.Empty;

        /// <summary>
        /// ��ѯ�������
        /// </summary>
        public string FileType
        {
            get { return this.m_fileType; }
        }

        public FindResult(int offset, int length, int line, string text)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            this.m_offset = offset;
            this.m_length = length;
            this.m_line = line;
            this.m_text = text;
        }

        public FindResult(int offset, int length, int line, string text, string templetID, string templetName
            , string Filetype)
            : this(offset, length, line, text)
        {
            this.m_templetID = templetID;
            this.m_templetName = templetName;
            this.m_fileType = Filetype;
        }

        public virtual Point GetStartPosition(IDocument document)
        {
            return document.OffsetToPosition(this.Offset);
        }

        public virtual Point GetEndPosition(IDocument document)
        {
            return document.OffsetToPosition(this.Offset + this.Length);
        }

        public override string ToString()
        {
            return string.Format("[FindResult: Offset={0}, Length={1}, Line={2}, Text={3}]", this.Offset, this.Length, this.Line, this.Text);
        }
    }
}
