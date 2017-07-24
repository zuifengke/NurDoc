// ***********************************************************
// ������Ӳ���ϵͳ,����ָ�ش���.
// Creator:Yechongchong  Date:2014-1-5
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.DockSuite;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Utilities;
using Heren.NurDoc.Frame.Dialogs;
using Heren.NurDoc.Utilities.Forms;
using Heren.NurDoc.PatPage.Utility;

namespace Heren.NurDoc.Frame.DockForms
{
    internal partial class NursingQCForm : DockContentBase
    {
        /// <summary>
        /// ���ڱ��没��ָ�ص���ر�
        /// </summary>
        private List<DocTypeInfo> lstDocTypeInfo = null;

        public NursingQCForm(MainFrame parent)
            : base(parent)
        {
            this.InitializeComponent();
            this.CloseButtonVisible = false;
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;

            string text = SystemContext.Instance.WindowName.NursingQC;
            this.Text = SystemContext.Instance.WindowName.GetName(text);
            this.Index = SystemContext.Instance.WindowName.GetIndex(text, 500);
            this.Hide();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents();

            this.dtBeginTime.Value = SysTimeService.Instance.Now.AddDays(1 - SysTimeService.Instance.Now.Day);
            this.dtEndTime.Value = SysTimeService.Instance.Now.AddMonths(1).AddDays(-SysTimeService.Instance.Now.Day);
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.LoadQcExamineModule();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        /// <summary>
        /// �󶨲���ָ�ر��б����ó�ʼֵ
        /// </summary>
        private void LoadQcExamineModule()
        {
            if (this.BindQCExamintModule())
            {
                this.cboBingLiType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// �󶨲���ָ�ر��б������ؼ�
        /// </summary>
        /// <returns>true Or false</returns>
        private bool BindQCExamintModule()
        {
            string szApply = ServerData.DocTypeApplyEnv.NURSING_QC;
            lstDocTypeInfo = FormCache.Instance.GetDocTypeList(szApply);
            if (lstDocTypeInfo == null || lstDocTypeInfo.Count <= 0)
            {
                MessageBoxEx.ShowError("���ز���ָ�ر�ʧ��!");
                return false;
            }
            bool bExistsValueAble = false;
            foreach (DocTypeInfo docTypeInfo in lstDocTypeInfo)
            {
                if (docTypeInfo == null
                    || GlobalMethods.Misc.IsEmptyString(docTypeInfo.DocTypeID))
                    continue;
                if (!docTypeInfo.IsValid || !docTypeInfo.IsVisible)
                    continue;
                if (docTypeInfo.IsFolder)
                    continue;
                if (!FormCache.Instance.IsWardDocType(docTypeInfo.DocTypeID))
                    continue;
                this.cboBingLiType.Items.Add(docTypeInfo.DocTypeName);
                bExistsValueAble = true;
            }
            return bExistsValueAble;
        }

        /// <summary>
        /// ���ر�
        /// </summary>
        /// <returns>���سɹ����</returns>
        private bool LoadQcExamine()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_QC;
            string szTempletName = this.cboBingLiType.Text;
            DocTypeInfo docTypeInfo =
                FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                //MessageBoxEx.ShowError(szTempletName + "����û������!");
                this.Close();
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError(szTempletName + "������ʧ��!");
                return false;
            }
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// ��ѯ�����½�������
        /// </summary>
        private void SearchAndUpdateForm()
        {
            if (this.cboBingLiType.Text == string.Empty)
            {
                return;
            }
            if (this.LoadQcExamine() == false)
            {
                MessageBoxEx.ShowError("������ʧ��!");
            }
            else
            {
                object visible = this.formControl1.GetFormData("�Ƿ���ʾ������˰�ť");
                this.btnQCAll.Visible = (visible is bool) ? (bool)visible : false;
                StringBuilder szSearchCondition = new StringBuilder();
                string szFormat = "{0};{1};{2};{3};{4}";
                if (this.chkInHospital.Checked == true)
                {
                    szSearchCondition.AppendFormat(szFormat, "TRUE", "FALSE", string.Empty, string.Empty, string.Empty);
                }
                else if (this.chkOutHospital.Checked == true)
                {
                    szSearchCondition.AppendFormat(szFormat, "FALSE", "TRUE", this.dtBeginTime.Text, this.dtEndTime.Text, string.Empty);
                }
                else
                {
                    if (this.txtPatientID.Enabled == true && this.txtPatientID.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("����ID����Ϊ��!");
                        return;
                    }
                    szSearchCondition.AppendFormat(szFormat, "FALSE", "FALSE", string.Empty, string.Empty, this.txtPatientID.Text.Trim());
                }
                szSearchCondition.AppendFormat(this.chkNotExamine.Checked ? ";TRUE" : ";");
                szSearchCondition.AppendFormat(this.chkExamine.Checked ? ";TRUE" : ";");
                this.formControl1.UpdateFormData("��ѯ����", szSearchCondition.ToString());
                this.formControl1.UpdateFormData("ˢ����ͼ", true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.SearchAndUpdateForm();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void chkInHospital_Click(object sender, EventArgs e)
        {
            if (this.chkInHospital.Checked)
            {
                this.chkOutHospital.Checked = false;
                this.dtBeginTime.Enabled = false;
                this.dtEndTime.Enabled = false;
            }

            if (this.chkInHospital.Checked == false && this.chkOutHospital.Checked == false)
            {
                this.txtPatientID.Enabled = true;
            }
            else
            {
                this.txtPatientID.Enabled = false;
            }
        }

        private void chkOutHospital_Click(object sender, EventArgs e)
        {
            if (this.chkOutHospital.Checked)
            {
                this.chkInHospital.Checked = false;
                this.dtBeginTime.Enabled = true;
                this.dtEndTime.Enabled = true;
            }
            else
            {
                this.dtBeginTime.Enabled = false;
                this.dtEndTime.Enabled = false;
            }

            if (this.chkInHospital.Checked == false && this.chkOutHospital.Checked == false)
            {
                this.txtPatientID.Enabled = true;
            }
            else
            {
                this.txtPatientID.Enabled = false;
            }
        }

        private void cboBingLiType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SearchAndUpdateForm();
        }

        private void formControl1_CustomEvent(object sender, Heren.Common.Forms.Editor.CustomEventArgs e)
        {
            if (e.Name == "���没���ʿ���Ϣ")
            {
                QcExamineHandler.Instance.HandleQcExamineSave(e.Data);
            }
            else if (e.Name == "���ĵ����")
            {
                List<NurDocInfo> lstNurDocInfo = new List<NurDocInfo>();
                QcExamineHandler.Instance.getNurDocInfoFromString(e.Data, ref lstNurDocInfo);
                QcNurDocForm qcform = new QcNurDocForm(lstNurDocInfo, GlobalMethods.Convert.StringToValue(e.Param, 0));
                qcform.ShowDialog();

                this.SearchAndUpdateForm();
            }
            else if (e.Name == "�����µ����")
            {
                //���µ���ֵΪDataTable����Ӧ�����к�   DataTable �������� pid vid
                if (e.Data is DataTable)
                {
                    DataTable dt = e.Data as DataTable;
                    if (dt.Columns.Count != 2)
                    {
                        MessageBoxEx.Show("���µ��򿪲�������");
                        return;
                    }
                    QcTemperature qcform = new QcTemperature(e.Data as DataTable, GlobalMethods.Convert.StringToValue(e.Param, 0));
                    qcform.ShowDialog();

                    this.SearchAndUpdateForm();
                }
            }
            else if (e.Name == "��ҽ�������")
            {
                //ҽ������ֵΪDataTable����Ӧ�����к�   DataTable �������� pid vid
                if (e.Data is DataTable)
                {
                    DataTable dt = e.Data as DataTable;
                    if (dt.Columns.Count != 2)
                    {
                        MessageBoxEx.Show("ҽ�����򿪲�������");
                        return;
                    }
                    QcOrderRecForm qcform = new QcOrderRecForm(e.Data as DataTable, GlobalMethods.Convert.StringToValue(e.Param, 0));
                    qcform.ShowDialog();

                    this.SearchAndUpdateForm();
                }
            }
            else if (e.Name == "�򿪻����¼�����")
            {
                //�����¼����ֵΪDataTable����Ӧ�����к�   DataTable �������� pid vid
                if (e.Data is DataTable)
                {
                    DataTable dt = e.Data as DataTable;
                    if (dt.Columns.Count != 2)
                    {
                        MessageBoxEx.Show("�����¼���򿪲�������");
                        return;
                    }
                    QcNurRecForm qcform = new QcNurRecForm(e.Data as DataTable, GlobalMethods.Convert.StringToValue(e.Param, 0));
                    qcform.ShowDialog();

                    this.SearchAndUpdateForm();
                }
            }
        }

        private void chkNotExamine_CheckedChanged(object sender, EventArgs e)
        {
            StringBuilder sbExamineState = new StringBuilder();
            sbExamineState.Append(this.chkNotExamine.Checked ? "δ���" : string.Empty);
            sbExamineState.Append("||");
            sbExamineState.Append(this.chkExamine.Checked ? "�����" : string.Empty);
            this.formControl1.UpdateFormData("����״̬ˢ����ͼ", sbExamineState.ToString());
        }

        private void chkExamine_CheckedChanged(object sender, EventArgs e)
        {
            StringBuilder sbExamineState = new StringBuilder();
            sbExamineState.Append(this.chkNotExamine.Checked ? "δ���" : string.Empty);
            sbExamineState.Append("||");
            sbExamineState.Append(this.chkExamine.Checked ? "�����" : string.Empty);
            this.formControl1.UpdateFormData("����״̬ˢ����ͼ", sbExamineState.ToString());
        }

        private void btnQCAll_Click(object sender, EventArgs e)
        {
            this.formControl1.GetFormData("�������");
        }
    }
}