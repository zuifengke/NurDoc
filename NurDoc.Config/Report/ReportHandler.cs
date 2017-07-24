// ***********************************************************
// ���������ù���ϵͳ,�������ģ�������.
// ��Ҫ���𱨱��ļ�������,ʵ���������,�����ر����ļ�
// Author : YangMingkun, Date : 2012-6-6
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Report.Runtime;
using Heren.Common.Report.Loader;
using Heren.Common.VectorEditor.Shapes;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Config.Dialogs;
using Heren.NurDoc.Config.DockForms;

namespace Heren.NurDoc.Config.Report
{
    internal class ReportHandler
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

        public DesignEditForm ActiveReport
        {
            get
            {
                DesignEditForm activeReport =
                    this.MainForm.ActiveDocument as DesignEditForm;
                if (activeReport == null || activeReport.IsDisposed)
                    return null;
                return activeReport;
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

        private static ReportHandler m_instance = null;

        /// <summary>
        /// ��ȡ��ǰ��������ʵ��
        /// </summary>
        public static ReportHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ReportHandler();
                return m_instance;
            }
        }

        private ReportHandler()
        {
        }

        /// <summary>
        /// ��ʼ��������������
        /// </summary>
        /// <param name="mainForm">�����򴰿�</param>
        public void InitReportHandler(MainForm mainForm)
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
            DesignEditForm designForm = this.ActiveReport;
            ScriptEditForm scriptForm = this.ActiveScript;
            if (scriptForm == null && designForm == null)
                return;

            if (designForm != null)
                scriptForm = this.GetScriptForm(designForm);
            else if (scriptForm != null)
                designForm = this.GetDesignForm(scriptForm);

            ReportFileParser parser = new ReportFileParser();
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

        internal DesignEditForm GetDesignForm(ReportTypeInfo reportTemplet)
        {
            if (reportTemplet == null)
                return null;
            return this.GetDesignForm(reportTemplet.ReportTypeID);
        }

        internal DesignEditForm GetDesignForm(string szTempletID)
        {
            if (this.MainForm == null || string.IsNullOrEmpty(szTempletID))
                return null;
            foreach (IDockContent content in this.MainForm.Documents)
            {
                DesignEditForm designForm = content as DesignEditForm;
                if (designForm == null || designForm.ReportTemplet == null)
                    continue;
                if (designForm.ReportTemplet.ReportTypeID == szTempletID)
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
            return scriptEditForm.Open(designForm.ReportTemplet, designForm.HndfFile);
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
            return designEditForm.Open(scriptForm.ReportTemplet, scriptForm.HndfFile);
        }

        internal short CreateReport(List<ElementBase> elements)
        {
            DesignEditForm designEditForm = new DesignEditForm(this.MainForm);
            designEditForm.FlagCode = Guid.NewGuid().ToString();
            this.MainForm.OpenDesignEditForm(designEditForm);
            bool success = designEditForm.Open(null, null);
            if (success)
                success = designEditForm.SetElements(elements);
            return success ? SystemConst.ReturnValue.OK : SystemConst.ReturnValue.FAILED;
        }

        internal short OpenReport()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "������ģ��(*.hrdt)|*.hrdt|�����ļ�(*.*)|*.*";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() != DialogResult.OK)
                return SystemConst.ReturnValue.CANCEL;

            DesignEditForm designEditForm = new DesignEditForm(this.MainForm);
            designEditForm.FlagCode = Guid.NewGuid().ToString();
            this.MainForm.OpenDesignEditForm(designEditForm);

            return designEditForm.Open(openDialog.FileName) ?
                SystemConst.ReturnValue.OK : SystemConst.ReturnValue.FAILED;
        }

        internal short OpenReport(ReportTypeInfo reportTemplet)
        {
            if (reportTemplet == null)
                return SystemConst.ReturnValue.FAILED;

            DesignEditForm designEditForm = this.GetDesignForm(reportTemplet);
            if (designEditForm != null)
            {
                designEditForm.Activate();
                return SystemConst.ReturnValue.OK;
            }
            designEditForm = new DesignEditForm(this.MainForm);
            designEditForm.FlagCode = Guid.NewGuid().ToString();
            this.MainForm.OpenDesignEditForm(designEditForm);

            string szTempletID = reportTemplet.ReportTypeID;
            string szHndfFile = string.Format("{0}\\Cache\\{1}.hrdt"
                , GlobalMethods.Misc.GetWorkingPath(), szTempletID);

            byte[] byteTempletData = null;
            short shRet = TempletService.Instance.GetReportTemplet(szTempletID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
                return shRet;

            GlobalMethods.IO.WriteFileBytes(szHndfFile, byteTempletData);
            return designEditForm.Open(reportTemplet, szHndfFile) ?
                SystemConst.ReturnValue.OK : SystemConst.ReturnValue.FAILED;
        }

        /// <summary>
        /// ���浱ǰ���ڱ༭��ģ���ļ�
        /// </summary>
        /// <returns>bool</returns>
        internal bool SaveReport()
        {
            DesignEditForm designForm = this.ActiveReport;
            ScriptEditForm scriptForm = this.ActiveScript;
            if (scriptForm == null && designForm == null)
                return false;

            if (designForm != null)
                scriptForm = this.GetScriptForm(designForm);
            else if (scriptForm != null)
                designForm = this.GetDesignForm(scriptForm);

            ReportFileParser parser = new ReportFileParser();
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
            parser.MakeReportData(szDesignData, szScriptData, out byteTempletData);

            DialogResult result = MessageBoxEx.ShowQuestion("�Ƿ��ύ����������"
                + "\r\n�ύ��������,�������ǡ���ť!\r\n�����浽����,�������񡱰�ť!");
            if (result == DialogResult.Cancel)
                return false;
            bool success = true;
            if (result == DialogResult.No)
                success = this.SaveReportToLocal(byteTempletData);
            else
                success = this.SaveReportToServer(byteTempletData);
            if (success)
            {
                if (designForm != null) designForm.IsModified = false;
                if (scriptForm != null) scriptForm.IsModified = false;
            }
            return success;
        }

        private bool SaveReportToLocal(byte[] byteTempletData)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "������ģ��(*.hrdt)|*.hrdt|�����ļ�(*.*)|*.*";
            saveDialog.Title = "���Ϊ����ģ���ļ�";
            saveDialog.RestoreDirectory = true;

            string szFileName = string.Empty;
            if (this.ActiveReport != null)
                szFileName = this.ActiveReport.Text + ".hrdt";
            else if (this.ActiveScript != null)
                szFileName = this.ActiveScript.Text + ".hrdt";
            szFileName = szFileName.Replace("(���)", string.Empty);
            szFileName = szFileName.Replace("(�ű�)", string.Empty);
            szFileName = szFileName.Replace(".hrdt", string.Empty);
            szFileName = szFileName.Replace("*", string.Empty);
            saveDialog.FileName = szFileName.Replace(" ", string.Empty);

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return false;

            if (!GlobalMethods.IO.WriteFileBytes(saveDialog.FileName, byteTempletData))
            {
                MessageBoxEx.Show("��������д���ļ�ʧ��!");
                return false;
            }
            return true;
        }

        private bool SaveReportToServer(byte[] byteTempletData)
        {
            //��ȡ��ǰ������Ϣ
            ReportTypeInfo reportTemplet = null;
            if (this.ActiveReport != null)
                reportTemplet = this.ActiveReport.ReportTemplet;
            else if (this.ActiveScript != null)
                reportTemplet = this.ActiveScript.ReportTemplet;

            ReportSelectForm frmTempletSelect = new ReportSelectForm();
            frmTempletSelect.Description = "��ѡ����Ҫ���µ�Ŀ�걨��ģ�壺";
            frmTempletSelect.MultiSelect = false;
            if (reportTemplet != null)
            {
                frmTempletSelect.ReportType = reportTemplet.ApplyEnv;
                frmTempletSelect.DefaultTempletID = reportTemplet.ReportTypeID;
            }
            if (frmTempletSelect.ShowDialog() != DialogResult.OK)
                return false;
            if (frmTempletSelect.SelectedTemplets == null)
                return false;
            if (frmTempletSelect.SelectedTemplets.Count <= 0)
                return false;

            reportTemplet = frmTempletSelect.SelectedTemplets[0];
            short shRet = TempletService.Instance.SaveReportTemplet(reportTemplet.ReportTypeID, byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                string szTempletName = reportTemplet.ReportTypeName;
                MessageBoxEx.Show(string.Format("����ģ�塰{0}������ʧ��!", szTempletName));
                return false;
            }
            return true;
        }
    }
}
