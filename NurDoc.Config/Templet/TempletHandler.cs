// ***********************************************************
// ���������ù���ϵͳ,��ģ�����ģ�������.
// ��Ҫ�����ģ���ļ�������,ʵ���������,�����ر�ģ���ļ�
// Author : YangMingkun, Date : 2012-6-6
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Forms.Loader;
using Heren.Common.Forms.Runtime;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Config.DockForms;

namespace Heren.NurDoc.Config.Templet
{
    internal class TempletHandler
    {
        private MainForm m_mainForm = null;

        /// <summary>
        /// ��ȡ��ǰ�����򴰿ڶ���
        /// </summary>
        public MainForm MainForm
        {
            get
            {
                if (this.m_mainForm == null)
                    return null;
                if (this.m_mainForm.IsDisposed)
                    return null;
                return this.m_mainForm;
            }
        }

        public DesignEditForm ActiveTemplet
        {
            get
            {
                DesignEditForm activeTemplet =
                    this.MainForm.ActiveDocument as DesignEditForm;
                if (activeTemplet == null || activeTemplet.IsDisposed)
                    return null;
                return activeTemplet;
            }
        }

        public ScriptEditForm ActiveScript
        {
            get
            {
                ScriptEditForm activeScript =
                    this.MainForm.ActiveDocument as ScriptEditForm;
                if (activeScript == null || activeScript.IsDisposed)
                    return null;
                return activeScript;
            }
        }

        private static TempletHandler m_instance = null;

        /// <summary>
        /// ��ȡ��ǰģ�崦����ʵ��
        /// </summary>
        public static TempletHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new TempletHandler();
                return m_instance;
            }
        }

        private TempletHandler()
        {
        }

        /// <summary>
        /// ��ʼ��ģ�崦��������
        /// </summary>
        /// <param name="mainForm">�����򴰿�</param>
        public void InitTempletHandler(MainForm mainForm)
        {
            this.m_mainForm = mainForm;
        }

        private ErrorsListForm.CompileError[] GetCompileErrors(CompileResults results)
        {
            ErrorsListForm.CompileError[] errors = null;
            if (results == null)
                return null;
            errors = new ErrorsListForm.CompileError[results.Errors.Count];
            for (int index = 0; index < errors.Length; index++)
            {
                CompileError error = results.Errors[index];
                errors[index] = new ErrorsListForm.CompileError();
                errors[index].Line = error.Line;
                errors[index].Column = error.Column;
                errors[index].ErrorText = error.ErrorText;
                errors[index].FileName = error.FileName;
                errors[index].IsWarning = error.IsWarning;
            }
            return errors;
        }

        internal void ShowScriptTestForm()
        {
            DesignEditForm designForm = this.ActiveTemplet;
            ScriptEditForm scriptForm = this.ActiveScript;
            if (scriptForm == null && designForm == null)
                return;

            if (designForm != null)
                scriptForm = this.GetScriptForm(designForm);
            else if (scriptForm != null)
                designForm = this.GetDesignForm(scriptForm);

            FormFileParser parser = new FormFileParser();
            string szScriptData = null;
            if (scriptForm != null)
                szScriptData = scriptForm.Save();
            else
                szScriptData = parser.GetScriptData(designForm.HndfFile);

            string szDesignData = null;
            if (designForm != null)
                designForm.Save(ref szDesignData);
            else
                szDesignData = parser.GetDesignData(scriptForm.HndfFile);

            //����ű�
            ScriptProperty scriptProperty = new ScriptProperty();
            scriptProperty.ScriptText = szScriptData;
            CompileResults results = null;
            results = ScriptCompiler.Instance.CompileScript(scriptProperty);
            if (!results.HasErrors)
            {
                this.MainForm.ShowCompileErrorForm(null);
            }
            else
            {
                if (scriptForm == null)
                    this.OpenScriptEditForm(designForm);
                this.MainForm.ShowCompileErrorForm(this.GetCompileErrors(results));
                MessageBoxEx.Show("����ʧ�ܣ��޷��������Գ���");
                return;
            }

            ScriptTestForm scriptTestForm = new ScriptTestForm();
            scriptTestForm.ScriptData = szScriptData;
            scriptTestForm.DesignData = szDesignData;
            scriptTestForm.ShowDialog();
        }

        internal ScriptEditForm GetScriptForm(DesignEditForm designForm)
        {
            if (this.MainForm == null || designForm == null)
                return null;
            foreach (IDockContent content in this.MainForm.Documents)
            {
                ScriptEditForm scriptForm = content as ScriptEditForm;
                if (scriptForm == null)
                    continue;
                if (scriptForm.FlagCode == designForm.FlagCode)
                    return scriptForm;
                if (scriptForm.HndfFile == designForm.HndfFile)
                    return scriptForm;
            }
            return null;
        }

        internal DesignEditForm GetDesignForm(ScriptEditForm scriptForm)
        {
            if (this.MainForm == null || scriptForm == null)
                return null;
            foreach (IDockContent content in this.MainForm.Documents)
            {
                DesignEditForm designForm = content as DesignEditForm;
                if (designForm == null)
                    continue;
                if (designForm.FlagCode == scriptForm.FlagCode)
                    return designForm;
                if (designForm.HndfFile == scriptForm.HndfFile)
                    return designForm;
            }
            return null;
        }

        internal DesignEditForm GetDesignForm(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null)
                return null;
            return this.GetDesignForm(docTypeInfo.DocTypeID);
        }

        internal DesignEditForm GetDesignForm(string szDocTypeID)
        {
            if (this.MainForm == null || string.IsNullOrEmpty(szDocTypeID))
                return null;
            foreach (IDockContent content in this.MainForm.Documents)
            {
                DesignEditForm designForm = content as DesignEditForm;
                if (designForm == null || designForm.DocTypeInfo == null)
                    continue;
                if (designForm.DocTypeInfo.DocTypeID == szDocTypeID)
                    return designForm;
            }
            return null;
        }

        internal bool OpenScriptEditForm(DesignEditForm designForm)
        {
            if (designForm == null || designForm.IsDisposed)
                return false;

            ScriptEditForm scriptEditForm = this.GetScriptForm(designForm);
            if (scriptEditForm != null)
            {
                scriptEditForm.DockHandler.Activate();
                return true;
            }
            scriptEditForm = new ScriptEditForm(this.MainForm);
            scriptEditForm.FlagCode = designForm.FlagCode;
            this.MainForm.OpenScriptEditForm(scriptEditForm);
            return scriptEditForm.Open(designForm.DocTypeInfo, designForm.HndfFile);
        }

        internal bool OpenDesignEditForm(ScriptEditForm scriptForm)
        {
            if (scriptForm == null || scriptForm.IsDisposed)
                return false;

            DesignEditForm designEditForm = this.GetDesignForm(scriptForm);
            if (designEditForm != null)
            {
                designEditForm.DockHandler.Activate();
                return true;
            }
            designEditForm = new DesignEditForm(this.MainForm);
            designEditForm.FlagCode = scriptForm.FlagCode;
            this.MainForm.OpenDesignEditForm(designEditForm);
            return designEditForm.Open(scriptForm.DocTypeInfo, scriptForm.HndfFile);
        }

        internal short OpenTemplet()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "������ģ��(*.hndt)|*.hndt|�����ļ�(*.*)|*.*";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() != DialogResult.OK)
                return SystemConst.ReturnValue.CANCEL;

            DesignEditForm designEditForm = new DesignEditForm(this.MainForm);
            designEditForm.FlagCode = Guid.NewGuid().ToString();
            this.MainForm.OpenDesignEditForm(designEditForm);

            return designEditForm.Open(openDialog.FileName) ?
                SystemConst.ReturnValue.OK : SystemConst.ReturnValue.FAILED;
        }

        internal short OpenTemplet(DocTypeInfo docTypeInfo)
        {
            if (docTypeInfo == null)
                return SystemConst.ReturnValue.FAILED;

            DesignEditForm designEditForm = this.GetDesignForm(docTypeInfo);
            if (designEditForm != null)
            {
                designEditForm.Activate();
                return SystemConst.ReturnValue.OK;
            }
            designEditForm = new DesignEditForm(this.MainForm);
            designEditForm.FlagCode = Guid.NewGuid().ToString();
            this.MainForm.OpenDesignEditForm(designEditForm);

            string szDocTypeID = docTypeInfo.DocTypeID;
            string szHndfFile = string.Format("{0}\\Cache\\{1}.hndt"
                , GlobalMethods.Misc.GetWorkingPath(), szDocTypeID);

            byte[] byteTempletData = null;
            short shRet = TempletService.Instance.GetFormTemplet(szDocTypeID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
                return shRet;

            GlobalMethods.IO.WriteFileBytes(szHndfFile, byteTempletData);
            return designEditForm.Open(docTypeInfo, szHndfFile) ?
                SystemConst.ReturnValue.OK : SystemConst.ReturnValue.FAILED;
        }

        /// <summary>
        /// ���浱ǰ���ڱ༭��ģ���ļ�
        /// </summary>
        /// <returns>bool</returns>
        internal bool SaveTemplet()
        {
            DesignEditForm designForm = this.ActiveTemplet;
            ScriptEditForm scriptForm = this.ActiveScript;
            if (scriptForm == null && designForm == null)
                return false;

            if (designForm != null)
                scriptForm = this.GetScriptForm(designForm);
            else if (scriptForm != null)
                designForm = this.GetDesignForm(scriptForm);

            FormFileParser parser = new FormFileParser();
            string szScriptData = null;
            if (scriptForm != null)
                szScriptData = scriptForm.Save();
            else
                szScriptData = parser.GetScriptData(designForm.HndfFile);

            string szDesignData = null;
            if (designForm != null)
                designForm.Save(ref szDesignData);
            else
                szDesignData = parser.GetDesignData(scriptForm.HndfFile);

            byte[] byteTempletData = null;
            parser.MakeFormData(szDesignData, szScriptData, out byteTempletData);

            DialogResult result = MessageBoxEx.ShowQuestion("�Ƿ��ύ����������"
                + "\r\n�ύ��������,�������ǡ���ť!\r\n�����浽����,�������񡱰�ť!");
            if (result == DialogResult.Cancel)
                return false;
            bool success = true;
            if (result == DialogResult.No)
                success = this.SaveTempletToLocal(byteTempletData);
            else
                success = this.SaveTempletToServer(byteTempletData);
            if (success)
            {
                if (designForm != null) designForm.IsModified = false;
                if (scriptForm != null) scriptForm.IsModified = false;
            }
            return success;
        }

        private bool SaveTempletToLocal(byte[] byteTempletData)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "������ģ��(*.hndt)|*.hndt|�����ļ�(*.*)|*.*";
            saveDialog.Title = "���Ϊ����ģ���ļ�";
            saveDialog.RestoreDirectory = true;

            string szFileName = string.Empty;
            if (this.ActiveTemplet != null)
                szFileName = this.ActiveTemplet.Text + ".hndt";
            else if (this.ActiveScript != null)
                szFileName = this.ActiveScript.Text + ".hndt";
            szFileName = szFileName.Replace("(���)", string.Empty);
            szFileName = szFileName.Replace("(�ű�)", string.Empty);
            szFileName = szFileName.Replace(".hndt", string.Empty);
            szFileName = szFileName.Replace("*", string.Empty);
            saveDialog.FileName = szFileName.Replace(" ", string.Empty);

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return false;

            if (!GlobalMethods.IO.WriteFileBytes(saveDialog.FileName, byteTempletData))
            {
                MessageBoxEx.Show("�ĵ�����д���ļ�ʧ��!");
                return false;
            }
            return true;
        }

        private bool SaveTempletToServer(byte[] byteTempletData)
        {
            //��ȡ��ǰģ��������Ϣ
            DocTypeInfo docTypeInfo = null;
            if (this.ActiveTemplet != null)
                docTypeInfo = this.ActiveTemplet.DocTypeInfo;
            else if (this.ActiveScript != null)
                docTypeInfo = this.ActiveScript.DocTypeInfo;

            TempletSelectForm frmTempletSelect = new TempletSelectForm();
            frmTempletSelect.Description = "��ѡ����Ҫ���µ�Ŀ�겡��ģ�壺";
            frmTempletSelect.MultiSelect = false;
            if (docTypeInfo != null)
            {
                frmTempletSelect.ApplyEnv = docTypeInfo.ApplyEnv;
                frmTempletSelect.DefaultDocTypeID = docTypeInfo.DocTypeID;
            }
            if (frmTempletSelect.ShowDialog() != DialogResult.OK)
                return false;
            if (frmTempletSelect.SelectedDocTypes == null)
                return false;
            if (frmTempletSelect.SelectedDocTypes.Count <= 0)
                return false;

            docTypeInfo = frmTempletSelect.SelectedDocTypes[0];
            short shRet = TempletService.Instance.SaveFormTemplet(docTypeInfo.DocTypeID, byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                string szDocTypeName = docTypeInfo.DocTypeName;
                MessageBoxEx.Show(string.Format("ģ�塰{0}������ʧ��!", szDocTypeName));
                return false;
            }
            return true;
        }
    }
}
