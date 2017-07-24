// ***********************************************************
// ������Ӳ���ϵͳ,���Ӱ��������޸Ĵ���.
// Creator:YangMingkun  Date:2013-7-12
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.Common.Controls.TableView;
using Heren.NurDoc.Data;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Frame.DockForms;

namespace Heren.NurDoc.Frame.Dialogs
{
    internal partial class ShiftStatusEditForm : HerenForm
    {
        private NursingShiftInfo m_nursingShiftInfo = null;

        private DateTime? m_defaultShiftDate = null;

        /// <summary>
        ///  ��ȡ������Ĭ����ʾ�Ľ���ʱ��
        /// </summary>
        [Browsable(false)]
        public DateTime? DefaultShiftDate
        {
            get { return this.m_defaultShiftDate; }
            set { this.m_defaultShiftDate = value; }
        }

        private string m_defaultRankCode = string.Empty;

        /// <summary>
        ///  ��ȡ������Ĭ����ʾ�Ľ�����
        /// </summary>
        [Browsable(false)]
        public string DefaultRankCode
        {
            get { return this.m_defaultRankCode; }
            set { this.m_defaultRankCode = value; }
        }

        private bool m_bIsNewNursingShift = true;

        /// <summary>
        /// ��ȡ��ǰ�����Ƿ����½���
        /// </summary>
        public bool IsNewNursingShift
        {
            get { return this.m_bIsNewNursingShift; }
        }

        /// <summary>
        /// ��ʶ��ǰ�༭�������Ƿ�����ݿ��и��¹�
        /// </summary>
        private bool m_bRecordUpdated = false;

        /// <summary>
        /// ��ȡ��ǰ�༭�������Ƿ�����ݿ��и��¹�
        /// </summary>
        public bool RecordUpdated
        {
            get { return this.m_bRecordUpdated; }
        }

        /// <summary>
        /// ��ʶ�ؼ�ֵ�ı��¼��Ƿ����,�����ظ�ִ��
        /// </summary>
        private bool m_bValueChangedEnabled = true;


        private string m_szShiftStatusName = "";

        public string ShiftStatusName
        {
            get { return this.m_szShiftStatusName; }
            set { this.m_szShiftStatusName = value; }
        }

        public ShiftStatusEditForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;

            //����Ĭ����ʾ�Ľ�������
            if (this.m_defaultShiftDate == null || !this.m_defaultShiftDate.HasValue)
                this.tooldtpShiftDate.Value = SysTimeService.Instance.Now.Date;
            else
                this.tooldtpShiftDate.Value = this.m_defaultShiftDate.Value.Date;

            this.Update();

            //���ؽ����������б�
            this.LoadShiftRankList();

            //���ز�����̬�༭��
            this.Update();
            this.LoadWardStatusTemplet();

            //���ص�ǰ����Ľ����¼
            if (this.m_nursingShiftInfo == null)
                this.m_nursingShiftInfo = this.GetSelectedShiftInfo();
            this.LoadNursingShiftData(this.m_nursingShiftInfo);

            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = this.CheckModifyState();
        }

        /// <summary>
        /// ��鵱ǰ���ݱ༭�����Ƿ��Ѿ��ı�
        /// </summary>
        /// <returns>�Ƿ���δ�ύ</returns>
        private bool HasUncommit()
        {
            if (this.formControl1.IsModified)
                return true;
            return false;
        }

        /// <summary>
        /// ��鲢���浱ǰ���ݱ༭���ڵ��޸�
        /// </summary>
        /// <returns>�Ƿ�ȡ��</returns>
        private bool CheckModifyState()
        {
            if (!this.HasUncommit())
                return false;

            DialogResult result = MessageBoxEx.ShowQuestion("��ǰ�����������޸�,�Ƿ񱣴棿");
            if (result == DialogResult.Yes)
                return !this.SaveNursingShiftData();
            return (result == DialogResult.Cancel);
        }

        /// <summary>
        /// ���ؽ����������б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadShiftRankList()
        {
            this.toolcboShiftRank.Items.Clear();
            this.toolcboShiftRank.Items.AddRange(SystemContext.Instance.GetWardShiftRankList());

            object selectedItem = null;
            foreach (object item in this.toolcboShiftRank.Items)
            {
                ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                if (shiftRankInfo.RankCode == this.m_defaultRankCode)
                    selectedItem = item;
            }
            if (selectedItem != null)
                this.toolcboShiftRank.SelectedItem = selectedItem;
            else
                this.toolcboShiftRank.SelectedItem = this.GetCurrentShiftRank();
            return true;
        }

        /// <summary>
        /// ���ز�����̬��Ϣ�༭��
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadWardStatusTemplet()
        {
            string szApplyEnv = ServerData.DocTypeApplyEnv.NURSING_WORKSHIFT;
            string szTempletName = this.m_szShiftStatusName;
            DocTypeInfo docTypeInfo = FormCache.Instance.GetWardDocType(szApplyEnv, szTempletName);
            if (docTypeInfo == null)
            {
                MessageBoxEx.ShowError(string.Format("��{{0}}�༭����û������!", szTempletName));
                return false;
            }

            byte[] byteFormData = null;
            if (!FormCache.Instance.GetFormTemplet(docTypeInfo, ref byteFormData))
            {
                MessageBoxEx.ShowError(string.Format("��{{0}}�༭������ʧ��!", szTempletName));
                return false;
            }
            return this.formControl1.Load(docTypeInfo, byteFormData);
        }

        /// <summary>
        /// ���ݵ�ǰ�����¼������Ϣ���ص��ν�������
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadNursingShiftData(NursingShiftInfo nursingShiftInfo)
        {
            if (this.CheckModifyState())
            {
                return false;
            }
            if (nursingShiftInfo == null)
            {
                this.Text = string.Format("�༭{0}(����)", this.ShiftStatusName);
                this.m_bIsNewNursingShift = true;
                this.m_nursingShiftInfo = this.CreateNursingShiftInfo();
                NursingShiftInfo nursingShiftInfoDB = new NursingShiftInfo();
                short shRet = NurShiftService.Instance.GetNursingShiftInfo(
                    this.m_nursingShiftInfo.WardCode,
                    this.m_nursingShiftInfo.ShiftRecordDate,
                    this.m_nursingShiftInfo.ShiftRankCode,
                    ref nursingShiftInfoDB);
                if (shRet != ServerData.ExecuteResult.RES_NO_FOUND)
                {
                    MessageBoxEx.Show("�����������ݿ������иð�����ݣ��޷������������´�ҳ�档");
                    this.m_bRecordUpdated = true;//ʹ���Ӱ�������ˢ�����ݡ�
                    this.Close();
                }
            }
            else
            {
                this.Text = string.Format("�༭{0}(�޸�)", this.ShiftStatusName);
                this.m_bIsNewNursingShift = false;
                this.m_nursingShiftInfo = nursingShiftInfo;
            }
            this.ShowShiftDateAndRank(this.m_nursingShiftInfo);

            if (!this.m_bIsNewNursingShift)
            {
                this.LoadWardStatusData(this.m_nursingShiftInfo);
            }
            else
            {
                this.LoadWardStatusData(null);
            }
            return true;
        }

        /// <summary>
        /// ����ָ�������¼��Ӧ�Ĳ�����̬�������
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool LoadWardStatusData(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
            {
                this.formControl1.UpdateFormData("���ض�̬��Ϣ", null);
                if (!this.m_bIsNewNursingShift)
                    this.formControl1.IsModified = false;
                return true;
            }
            string szShiftID = nursingShiftInfo.ShiftRecordID;
            List<ShiftWardStatus> lstShiftWardStatus = null;
            short shRet = NurShiftService.Instance.GetShiftWardStatusList(szShiftID, ref lstShiftWardStatus);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("��ѯ���ಡ����̬��Ϣʧ��!");
                return false;
            }
            if (lstShiftWardStatus == null || lstShiftWardStatus.Count <= 0)
                return true;
            this.formControl1.UpdateFormData("���ض�̬��Ϣ", lstShiftWardStatus);
            if (!this.m_bIsNewNursingShift)
                this.formControl1.IsModified = false;
            return true;
        }

        /// <summary>
        /// ��ȡ��������б��е�ǰѡ�еĽ�����
        /// </summary>
        /// <returns>������</returns>
        private ShiftRankInfo GetSelectedShiftRank()
        {
            ShiftRankInfo shiftRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            if (shiftRankInfo != null)
                shiftRankInfo.UpdateShiftRankTime(this.tooldtpShiftDate.Value);
            return shiftRankInfo;
        }

        /// <summary>
        /// ���ݵ�ǰʱ����㲢���ؽ�����
        /// </summary>
        /// <returns>������</returns>
        private ShiftRankInfo GetCurrentShiftRank()
        {
            if (this.toolcboShiftRank.Items.Count <= 0)
                return null;
            DateTime dtShiftTime = SysTimeService.Instance.Now;
            foreach (object item in this.toolcboShiftRank.Items)
            {
                ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                shiftRankInfo.UpdateShiftRankTime(dtShiftTime.Date);
                if (dtShiftTime >= shiftRankInfo.StartTime && dtShiftTime < shiftRankInfo.EndTime)
                    return shiftRankInfo;
            }
            return this.toolcboShiftRank.Items[0] as ShiftRankInfo;
        }

        /// <summary>
        /// ��ʾָ�������¼��Ӧ�Ľ����κ�����
        /// </summary>
        /// <param name="nursingShiftInfo">�����¼</param>
        private void ShowShiftDateAndRank(NursingShiftInfo nursingShiftInfo)
        {
            if (nursingShiftInfo == null)
                return;
            DateTime dtShiftDate = nursingShiftInfo.ShiftRecordDate.Date;
            if (this.tooldtpShiftDate.Value != dtShiftDate)
                this.tooldtpShiftDate.Value = dtShiftDate;

            ShiftRankInfo selectedRankInfo = this.toolcboShiftRank.SelectedItem as ShiftRankInfo;
            if (selectedRankInfo == null || selectedRankInfo.RankCode != nursingShiftInfo.ShiftRankCode)
            {
                foreach (object item in this.toolcboShiftRank.Items)
                {
                    ShiftRankInfo shiftRankInfo = item as ShiftRankInfo;
                    if (shiftRankInfo.RankCode == nursingShiftInfo.ShiftRankCode)
                    {
                        this.toolcboShiftRank.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// ����һ���µĻ���������¼
        /// </summary>
        /// <returns>����������¼</returns>
        private NursingShiftInfo CreateNursingShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            int iSRSelectedIndex = this.toolcboShiftRank.SelectedIndex;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value;
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return null;
            return NursingShiftHandler.Instance.Create(SystemContext.Instance.LoginUser, dtShiftDate, SysTimeService.Instance.Now, shiftRankInfo, iSRSelectedIndex);
        }



        /// <summary>
        /// ��ȡ�û���ǰѡ������ںͰ�ζ�Ӧ�Ľ�����Ϣ
        /// </summary>
        /// <returns>������Ϣ</returns>
        private NursingShiftInfo GetSelectedShiftInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return null;
            string szRankCode = shiftRankInfo.RankCode;
            NursingShiftInfo nursingShiftInfo = null;
            short shRet = NurShiftService.Instance.GetNursingShiftInfo(szWardCode, dtShiftDate
                , szRankCode, ref nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK
                && shRet != ServerData.ExecuteResult.RES_NO_FOUND)
            {
                MessageBoxEx.Show("��ѯ��������¼ʧ��!");
                return null;
            }
            return nursingShiftInfo;
        }

        /// <summary>
        /// ���浱ǰ���޸ĵĲ�����̬�����ಡ�˵Ƚ�������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
        private bool SaveNursingShiftData()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (this.m_nursingShiftInfo == null)
            {
                MessageBoxEx.Show("��������¼��ϢΪ��,�޷�����!");
                return false;
            }
            if (!RightController.Instance.CanEditShiftRec(this.m_nursingShiftInfo))
                return false;

            short shRet = SystemConst.ReturnValue.OK;

            UserInfo userInfo = SystemContext.Instance.LoginUser;
            this.m_nursingShiftInfo.ModifierID = userInfo.ID;
            this.m_nursingShiftInfo.ModifierName = userInfo.Name;
            this.m_nursingShiftInfo.ModifyTime = SysTimeService.Instance.Now;

            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo != null)
                this.m_nursingShiftInfo.ShiftRankCode = shiftRankInfo.RankCode;

            DateTime dtShiftDate = this.tooldtpShiftDate.Value.Date;
            this.m_nursingShiftInfo.ShiftRecordDate = dtShiftDate;

            //���ð�ν����������Ƿ��ظ�
            NursingShiftInfo nursingShiftInfoDB = null;
            shRet = NurShiftService.Instance.GetNursingShiftInfo(
               this.m_nursingShiftInfo.WardCode,
               this.m_nursingShiftInfo.ShiftRecordDate,
               this.m_nursingShiftInfo.ShiftRankCode,
               ref nursingShiftInfoDB);
            if (shRet != ServerData.ExecuteResult.RES_NO_FOUND
                && nursingShiftInfoDB.ShiftRecordID != this.m_nursingShiftInfo.ShiftRecordID)
            {
                MessageBoxEx.Show("�����������ݿ������иð�����ݣ��޷������������´�ҳ�档");
                this.m_bRecordUpdated = true;//ʹ���Ӱ�������ˢ�����ݡ�
                //�����Ƿ񱣴���ʾ��
                this.formControl1.IsModified = false;
                this.Close();
            }

            //���潻�ಡ����̬
            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            if (this.formControl1.IsModified || this.m_bIsNewNursingShift)
                lstShiftWardStatus = this.GetShiftWardStatusList();
            foreach (ShiftWardStatus shiftWardStatus in lstShiftWardStatus)
            {
                shiftWardStatus.ShiftRecordID = this.m_nursingShiftInfo.ShiftRecordID;
                shRet = NurShiftService.Instance.SaveShiftWardStatus(shiftWardStatus);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.Show("���潻�ಡ����̬ʧ��!");
                    return false;
                }
            }
            this.formControl1.IsModified = false;

            //���潻����������¼
            shRet = NurShiftService.Instance.SaveNursingShiftInfo(this.m_nursingShiftInfo);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("���潻������¼��Ϣʧ��!");
                return false;
            }
            this.m_bRecordUpdated = true;
            return true;
        }

        /// <summary>
        /// ��ȡ����ʱ������̬��Ϣ�б�
        /// </summary>
        /// <returns>������̬��Ϣ�б�</returns>
        public List<ShiftWardStatus> GetShiftWardStatusList()
        {
            if (!this.formControl1.EndEdit())
                return null;

            List<ShiftWardStatus> lstShiftWardStatus = new List<ShiftWardStatus>();
            DataTable table = this.formControl1.GetFormData("��ȡ��̬��Ϣ") as DataTable;
            if (table == null)
                return lstShiftWardStatus;
            foreach (DataRow row in table.Rows)
            {
                ShiftWardStatus shiftWardStatus = new ShiftWardStatus();
                if (shiftWardStatus.FromDataRow(row))
                    lstShiftWardStatus.Add(shiftWardStatus);
            }
            return lstShiftWardStatus;
        }

        private void tooldtpShiftDate_ValueChanged(object sender, EventArgs e)
        {
            if (!this.m_bValueChangedEnabled)
                return;

            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;
            this.LoadNursingShiftData(this.GetSelectedShiftInfo());
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolcboShiftRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShiftRankInfo shiftRankInfo = this.GetSelectedShiftRank();
            if (shiftRankInfo == null)
                return;
            this.toollblRankTime.Text = string.Format("{0} - {1}"
                , shiftRankInfo.StartPoint, shiftRankInfo.EndPoint);

            if (!this.m_bValueChangedEnabled)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_bValueChangedEnabled = false;
            this.LoadNursingShiftData(this.GetSelectedShiftInfo());
            this.m_bValueChangedEnabled = true;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.SaveNursingShiftData())
                this.DialogResult = DialogResult.OK;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void toolbtnReturn_Click(object sender, EventArgs e)
        {
            if (this.m_bRecordUpdated)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
        }
    }
}