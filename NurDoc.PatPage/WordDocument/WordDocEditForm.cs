// **************************************************************
// ������Ӳ���word�ĵ��༭����
// Creator:Ycc  Date:2015-7-16
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;

namespace Heren.NurDoc.PatPage.WordDocument
{
    internal partial class WordDocEditForm : Form
    {
        private WordTempInfo m_WordTempInfo = null;

        /// <summary>
        /// ��ȡ�����õ�ǰģ�����Ϣ
        /// </summary>
        [Browsable(false)]
        public WordTempInfo WordTempInfo
        {
            get { return this.m_WordTempInfo; }
            set { this.m_WordTempInfo = value; }
        }

        private bool m_bIsModified = false;

        /// <summary>
        /// ��ȡ�����õ�ǰģ���Ƿ����޸�
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get { return this.m_bIsModified; }
            set { this.m_bIsModified = value; }
        }

        private string m_szFilePath = string.Empty;

        /// <summary>
        /// ��ȡ�����õ�ǰģ���Ƿ����޸�
        /// </summary>
        public string szFilePath
        {
            get { return this.m_szFilePath; }
            set { this.m_szFilePath = value; }
        }

        private WordDocForm m_wordtempForm = null;

        public WordDocEditForm()
            : this(null)
        {
        }

        public WordDocEditForm(WordDocForm worddocForm)
        {
            this.InitializeComponent();
            this.m_wordtempForm = worddocForm;
            this.winWordCtrl1.ShowInternalMenuStrip = false;
        }

        /// <summary>
        /// ��ָ����Word�ĵ�
        /// </summary>
        /// <param name="szFilePath">�ļ�·��</param>
        /// <returns>DataL  ayer.SystemData.ReturnValue</returns>
        public short OpenDocument(string szFilePath)
        {
            this.winWordCtrl1.OpenDocument(szFilePath);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ����ָ����Word�ĵ�
        /// </summary>
        /// <param name="szFilePath">�ļ�·��</param>
        /// <returns>SystemConst.ReturnValue</returns>
        public short SaveDocument(string szFilePath)
        {
            this.winWordCtrl1.SaveDocument(szFilePath);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// �رյ�ǰ�ĵ�
        /// </summary>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short CloseDocument()
        {
            this.winWordCtrl1.CloseDocument();
            return SystemConst.ReturnValue.OK;
        }

        public short CloseWordApplication()
        {
            this.winWordCtrl1.CloseWordApplication();
            return SystemConst.ReturnValue.OK;
        }
        
        private void toolbtnImport_Click(object sender, EventArgs e)
        {
            this.ImportFile();
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            string newPath = GetPath() + @"\Templets\WordTemp" + @"\";
           GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
           String tempId = this.m_WordTempInfo.TempletID;
           string filePath = newPath + tempId + ".doc";
           this.SaveDocument(filePath);
            byte[] byteTempletData = null;
            byteTempletData = this.wordConvertByte(filePath);
            short shRet = WordTempService.Instance.UpdateWordTemp(this.m_WordTempInfo, byteTempletData);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
                this.IsModified = false;
            else
                MessageBoxEx.Show(string.Format("��{0}���ı�ģ�屣��ʧ��!", this.m_WordTempInfo.TempletName));
               GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// word�ļ�ת������������(���ڱ������ݿ�)
        /// </summary>
        /// <param name="wordPath">word�ļ�·��</param>
        /// <returns>������</returns>
        private byte[] wordConvertByte(string wordPath)
        {
            byte[] bytContent = null;
            System.IO.FileStream fs = null;
            System.IO.BinaryReader br = null;
            try
            {
                fs = new System.IO.FileStream(wordPath, System.IO.FileMode.Open,System.IO.FileAccess.Read,FileShare.ReadWrite);
             }
            catch
            {
            }
            br = new BinaryReader((Stream)fs);
            bytContent = br.ReadBytes((Int32)fs.Length);

            return bytContent;
        }

        private void txtTempletContent_TextChanged(object sender, EventArgs e)
        {
            this.IsModified = true;
        }

        /// <summary>
        /// ��ȡ��ǰ�ı�ģ����ѡ�е�����
        /// </summary>
        /// <returns>�ı�����</returns>
        public string GetSelectedContent()
        {
            return this.wordTempletContent.SelectedText;
        }

        /// <summary>
        /// ѡ�е�ǰ�ı�ģ������������
        /// </summary>
        public void SelectAll()
        {
            this.wordTempletContent.SelectAll();
        }

        /// <summary>
        /// �رյ�ǰģ��
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short CloseTemplet()
        {
            this.m_WordTempInfo = null;
            this.wordTempletContent.Text = null;
            this.IsModified = false;
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ���ز���ָ����wordģ��
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short OpenTemplet()
        {
            this.IsModified = false;
            if (this.m_WordTempInfo == null)
                return SystemConst.ReturnValue.FAILED;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            string szTempletID = this.m_WordTempInfo.TempletID;
            byte[] byteTempletData = null;
            short shRet = WordTempService.Instance.GetWordTemp(szTempletID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("��{0}��wordģ������ʧ��!", this.m_WordTempInfo.TempletName));
                return SystemConst.ReturnValue.FAILED;
            }

            string szTempletContent = string.Empty;

            String szFilePath = this.ByteConvertWord(byteTempletData, szTempletID);
            if (szFilePath == string.Empty)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("��{0}��wordģ�����ʧ��!", this.m_WordTempInfo.TempletName));
                return SystemConst.ReturnValue.FAILED;
            }
            this.OpenDocument(szFilePath);
            this.IsModified = false;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// ����������ת��Ϊword�ļ�
        /// </summary>
        /// <param name="data">����������</param>
        /// <param name="fileName">word�ļ���</param>
        /// <returns>word��������·��</returns>
        public string ByteConvertWord(byte[] data, string fileName)
        {
            string newPath = GetPath() + @"\Templets\WordTemp" + @"\";
            if (!System.IO.Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
           
            string filePath = newPath + fileName + ".doc";

            //this.winWordCtrl1.CloseDocument();
            if (System.IO.File.Exists(filePath))
            {
                return filePath;
            }
            FileStream fs = new FileStream(filePath, FileMode.CreateNew);
            BinaryWriter br = new BinaryWriter(fs);
            br.Write(data, 0, data.Length);
            br.Close();
            fs.Close();
            return filePath;
        }

        /// <summary>
        /// ��Ŀ����Ŀ¼
        /// </summary>
        /// <returns>string</returns>
        public string GetPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// ���浱ǰ��ʾ���ı�ģ��
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short SaveTemplet()
        {
            if (this.m_WordTempInfo == null || this.m_WordTempInfo.IsFolder)
            {
                MessageBoxEx.Show("��ǰ��ʾ��ģ�廹û�б�������,���ȴ���!");
                return SystemConst.ReturnValue.FAILED;
            }
            if (!this.m_wordtempForm.IsAllowModifyTemplet(this.m_WordTempInfo))
            {
                MessageBoxEx.Show("��û��Ȩ�ޱ��浱ǰģ��!");
                return SystemConst.ReturnValue.FAILED;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            //�����ĵ�����
            string szTempletContent = this.wordTempletContent.Text;
            byte[] byteTempletData = null;
            //if (!GlobalMethods.Convert.StringToBytes(szTempletContent, ref byteTempletData))
            //{
            //    GlobalMethods.UI.SetCursor(this, Cursors.Default);
            //    return SystemConst.ReturnValue.EXCEPTION;
            //}
         //   this.winWordCtrl1.
            //�ύ��������
            this.Update();
            short shRet = WordTempService.Instance.UpdateWordTemp(this.m_WordTempInfo, byteTempletData);
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
                this.IsModified = false;
            else
                MessageBoxEx.Show(string.Format("��{0}���ı�ģ�屣��ʧ��!", this.m_WordTempInfo.TempletName));
            return shRet;
        }

        private void ImportFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "��ѡ���ļ�";
            fileDialog.Filter = "word�ĵ�|*.doc;*.docx|�����ĵ�(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string szFilePath = fileDialog.FileName;
                this.winWordCtrl1.CloseDocument();
                this.winWordCtrl1.OpenDocument(szFilePath);
            }
        }
    }
}