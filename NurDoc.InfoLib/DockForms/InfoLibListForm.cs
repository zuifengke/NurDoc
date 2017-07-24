// **************************************************************
// ������Ӳ���ϵͳ����ģ��֮�ı�ģ��༭����
// Creator:YangMingkun  Date:2012-9-5
// Copyright : Heren Health Services Co.,Ltd.
// **************************************************************
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.InfoLib;

namespace Heren.NurDoc.InfoLib.DockForms
{
    internal partial class InfoLibListForm : Form
    {
        private InfoLibInfo m_InfoLibInfo = null;

        /// <summary>
        /// ��ȡ�����õ�ǰģ�����Ϣ
        /// </summary>
        [Browsable(false)]
        public InfoLibInfo InfoLibInfo
        {
            get { return this.m_InfoLibInfo; }
            set { this.m_InfoLibInfo = value; }
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

        private InfoLibForm m_InfoLibForm = null;

        public InfoLibListForm()
            : this(null)
        {
        }

        public InfoLibListForm(InfoLibForm templetForm)
        {
            this.InitializeComponent();
            this.m_InfoLibForm = templetForm;
            this.m_WinWordDocForm = new WinWordDocForm();
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            if (this.m_InfoLibForm != null && !this.m_InfoLibForm.IsDisposed)
                this.m_InfoLibForm.ImportSelectedContent();
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
        }

        private void txtTempletContent_TextChanged(object sender, EventArgs e)
        {
            this.IsModified = true;
        }

        /// <summary>
        /// ��ʾ�ĵ��б�
        /// </summary>
        public void ShowInfoLibList()
        {
            GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.WaitCursor);
            this.Update();

            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<InfoLibInfo> lstInfoLibInfos = null;
            short shRet = SystemConst.ReturnValue.OK;
            if (this.InfoLibInfo.CreatorID != SystemContext.Instance.LoginUser.ID)
            {
                if (this.InfoLibInfo.ShareLevel == ServerData.ShareLevel.DEPART)
                    shRet = InfoLibService.Instance.GetDeptChildInfoLibInfos(this.InfoLibInfo.InfoID, SystemContext.Instance.LoginUser.WardCode, true, ref lstInfoLibInfos);
                else if (this.InfoLibInfo.ShareLevel == ServerData.ShareLevel.HOSPITAL)
                    shRet = InfoLibService.Instance.GetHospitalChildInfoLibInfos(InfoLibInfo.InfoID, ref lstInfoLibInfos);
            }
            else
                shRet = InfoLibService.Instance.GetPersonalChildInfoLibInfos(szUserID, this.InfoLibInfo.InfoID, ref lstInfoLibInfos);

            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.Default);
                MessageBoxEx.Show("�������Ļ�����Ϣ�б�����ʧ��!");
                return;
            }
            if (lstInfoLibInfos == null)
                lstInfoLibInfos = new List<InfoLibInfo>();
            this.dataGridView1.Rows.Clear();
            foreach (InfoLibInfo item in lstInfoLibInfos)
            {
                if (item != null && !item.IsFolder && item.ParentID == this.InfoLibInfo.InfoID)
                {
                    int rowIndex = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[rowIndex].Cells[this.colCheck.Index].Value = false;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colName.Index].Value = item.InfoName;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colSize.Index].Value = string.Empty;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colModifyTime.Index].Value = item.ModifyTime.ToShortDateString();
                    this.dataGridView1.Rows[rowIndex].Cells[this.colStatus.Index].Value = item.Status;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colSize.Index].Value = item.InfoSize;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colShareLevel.Index].Value = ServerData.ShareLevel.TransShareLevel(item.ShareLevel);
                    this.dataGridView1.Rows[rowIndex].Tag = item;
                }
            }

            GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.Default);
        }

        /// <summary>
        /// ���ز���ָ�����ı�ģ��
        /// </summary>
        /// <returns>DataLayer.SystemConst.ReturnValue</returns>
        public short OpenTemplet()
        {
            this.IsModified = false;
            if (this.m_InfoLibInfo == null)
                return SystemConst.ReturnValue.FAILED;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);

            string szInfoID = this.m_InfoLibInfo.InfoID;
            byte[] byteTempletData = null;
            short shRet = InfoLibService.Instance.GetInfoLib(szInfoID, ref byteTempletData);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("��{0}���ı�ģ������ʧ��!", this.m_InfoLibInfo.InfoName));
                return SystemConst.ReturnValue.FAILED;
            }

            string szTempletContent = string.Empty;
            if (!GlobalMethods.Convert.BytesToString(byteTempletData, ref szTempletContent))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("��{0}���ı�ģ�����ʧ��!", this.m_InfoLibInfo.InfoName));
                return SystemConst.ReturnValue.FAILED;
            }
            // this.txtTempletContent.Text = szTempletContent;
            this.IsModified = false;
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            return SystemConst.ReturnValue.OK;
        }

       

        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        public static string CountSize(long Size)
        {
            string m_strSize = string.Empty;
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < KBCount)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= KBCount && FactSize < MBCount)
                m_strSize = (FactSize / KBCount).ToString("F2") + " KB";
            else if (FactSize >= MBCount && FactSize < GBCount)
                m_strSize = (FactSize / MBCount).ToString("F2") + " MB";
            else if (FactSize >= GBCount && FactSize < TBCount)
                m_strSize = (FactSize / GBCount).ToString("F2") + " GB";
            else if (FactSize >= TBCount)
                m_strSize = (FactSize / TBCount).ToString("F2") + " TB";
            return m_strSize;
        }

        /// <summary>
        /// ����һ���µ�ģ����Ϣ����
        /// </summary>
        /// <returns>TextTempletInfo</returns>
        private InfoLibInfo MakeInfoLibInfo()
        {
            if (SystemContext.Instance.LoginUser == null)
                return null;

            InfoLibInfo infoLibInfo = new InfoLibInfo();
            infoLibInfo.InfoName = string.Empty;
            infoLibInfo.CreatorID = SystemContext.Instance.LoginUser.ID;
            infoLibInfo.CreatorName = SystemContext.Instance.LoginUser.Name;
            infoLibInfo.CreateTime = SysTimeService.Instance.Now;
            infoLibInfo.ModifyTime = infoLibInfo.CreateTime;
            infoLibInfo.WardCode = SystemContext.Instance.LoginUser.WardCode;
            infoLibInfo.WardName = SystemContext.Instance.LoginUser.WardName;
            infoLibInfo.IsFolder = false;
            infoLibInfo.ParentID = this.InfoLibInfo.InfoID;
            infoLibInfo.ShareLevel = ServerData.ShareLevel.PERSONAL;
            infoLibInfo.InfoID = infoLibInfo.MakeInfoID();
            return infoLibInfo;
        }

        private WinWordDocForm m_WinWordDocForm = null;

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            InfoLibInfo infoLibInfo = this.dataGridView1.Rows[e.RowIndex].Tag as InfoLibInfo;
            if (InfoLibInfo == null)
                return;
            GlobalMethods.UI.SetCursor(this, Cursors.WaitCursor);
            this.m_WinWordDocForm = new WinWordDocForm();
            string szLocalFilePath = string.Empty;
            short shRet = InfoLibCache.Instance.GetInfoLibTemplet(infoLibInfo, ref szLocalFilePath);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("�ĵ�����ʧ��");
                return;
            }
            shRet = this.m_WinWordDocForm.OpenDocument(szLocalFilePath);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("���ĵ�ʧ��");
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            if (shRet != SystemConst.ReturnValue.OK)
                return;
            m_WinWordDocForm.ShowDialog();
        }

        /// <summary>
        /// �ؼ��ּ���
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.WaitCursor);
            this.Update();

            string szInfoName = this.txtInfoName.Text.Trim();
            List<InfoLibInfo> lstInfoLibInfos = null;
            short shRet = InfoLibService.Instance.GetInfoLibInfosByName(szInfoName, ref lstInfoLibInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.Default);
                MessageBoxEx.Show("�������Ļ�����Ϣ�б�����ʧ��!");
                return;
            }
            if (lstInfoLibInfos == null)
                lstInfoLibInfos = new List<InfoLibInfo>();
            this.dataGridView1.Rows.Clear();
            foreach (InfoLibInfo item in lstInfoLibInfos)
            {
                if (item != null && !item.IsFolder)
                {
                    int rowIndex = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[rowIndex].Cells[this.colCheck.Index].Value = false;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colName.Index].Value = item.InfoName;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colSize.Index].Value = string.Empty;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colModifyTime.Index].Value = item.ModifyTime.ToShortDateString();
                    this.dataGridView1.Rows[rowIndex].Cells[this.colStatus.Index].Value = item.Status;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colSize.Index].Value = item.InfoSize;
                    this.dataGridView1.Rows[rowIndex].Cells[this.colShareLevel.Index].Value = ServerData.ShareLevel.TransShareLevel(item.ShareLevel);
                    this.dataGridView1.Rows[rowIndex].Tag = item;
                }
            }

            GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.Default);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            InfoLibInfo infoLibInfo = null;
            if (e.RowIndex < 0)
                return;
            for (int index = 0; index < this.dataGridView1.Rows.Count; index++)
            {
                if (index != e.RowIndex)
                    this.dataGridView1.Rows[index].Cells[this.colCheck.Index].Value = false;
                else
                {
                    bool bChecked = Boolean.Parse(this.dataGridView1.Rows[e.RowIndex].Cells[this.colCheck.Index].Value.ToString());
                    if (bChecked)

                        this.dataGridView1.Rows[e.RowIndex].Cells[this.colCheck.Index].Value = false;
                    else
                    {
                        this.dataGridView1.Rows[e.RowIndex].Cells[this.colCheck.Index].Value = true;
                        infoLibInfo = this.dataGridView1.Rows[e.RowIndex].Tag as InfoLibInfo;
                    }
                }
            }
            this.ShowSelectedInfoLib(infoLibInfo);
        }

        /// <summary>
        /// ��ȡ�û�ѡ�е�ģ���ı���Ϣ
        /// </summary>
        private void ShowSelectedInfoLib(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null)
            {
                return;
            }

            string szCreatorName = null;
            if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.CreatorName))
                szCreatorName = "���߲���";
            else
                szCreatorName = infoLibInfo.CreatorName;

            string szDeptName = null;
            if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.WardName))
                szDeptName = "���Ҳ���";
            else
                szDeptName = infoLibInfo.WardName;

            string szLastModifyTime = null;
            if (infoLibInfo.ModifyTime == infoLibInfo.DefaultTime)
                szLastModifyTime = "����޸�ʱ�䲻��";
            else
                szLastModifyTime = infoLibInfo.ModifyTime.ToString("yyyy-MM-dd HH:mm");

            this.ShowStatusMassage(string.Format("{0}��{1}��{2}��{3}��{4}��{5}"
                , infoLibInfo.IsFolder ? "Ŀ¼" : "ģ��", infoLibInfo.InfoName, szCreatorName
                , szDeptName, ServerData.ShareLevel.TransShareLevel(infoLibInfo.ShareLevel), szLastModifyTime));
        }

        private void toolbtnDelete_Click(object sender, EventArgs e)
        {
            int count = 0;
            InfoLibInfo infoLibInfo = new InfoLibInfo();
            for (int index = 0; index < this.dataGridView1.Rows.Count; index++)
            {
                bool bChecked = Boolean.Parse(this.dataGridView1.Rows[index].Cells[this.colCheck.Index].Value.ToString());
                if (bChecked)
                {
                    count++;
                    infoLibInfo = this.dataGridView1.Rows[index].Tag as InfoLibInfo;
                }
            }
            if (count > 0)
            {
                if (MessageBoxEx.ShowConfirm("ȷ��ɾ���ļ���") == DialogResult.OK)
                {
                    short shRet = InfoLibService.Instance.DeleteInfoLibToFTP(infoLibInfo);
                    if (shRet != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.Show("ɾ���ļ�ʧ��");
                        return;
                    }
                    else
                    {
                        this.ShowInfoLibList();
                    }
                }
            }
        }

        private void toolbtnDowload_Click(object sender, EventArgs e)
        {
            int count = 0;
            InfoLibInfo infoLibInfo = new InfoLibInfo();
            for (int index = 0; index < this.dataGridView1.Rows.Count; index++)
            {
                bool bChecked = Boolean.Parse(this.dataGridView1.Rows[index].Cells[this.colCheck.Index].Value.ToString());
                if (bChecked)
                {
                    count++;
                    infoLibInfo = this.dataGridView1.Rows[index].Tag as InfoLibInfo;
                }
            }
            if (count == 0)
            {
                MessageBoxEx.ShowMessage("��ѡ��Ҫ���ص��ļ�");
                return;
            }
            //�����ĵ�
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = infoLibInfo.InfoName;
            fileDialog.Filter = "office word 2003(*.doc)|*.doc|�����ļ�(*.*)|*.*";
            fileDialog.Title = "���Ϊ";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string szFileName = fileDialog.FileName;
                string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(infoLibInfo);
                short shRet = InfoLibService.Instance.DownloadInfoLibFile(szRemoteFile, szFileName);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowMessage("����ʧ��");
                    return;
                }
                MessageBoxEx.ShowMessage("���سɹ�");
            }
        }

        /// <summary>
        /// �޸�ѡ��ģ��Ĺ���ȼ�
        /// </summary>
        /// <param name="szNewLevel">�µĹ���ȼ�</param>
        private void ModifyTempletShareLevel(string szNewLevel)
        {
            int count = 0;
            int rowIndex = 0;
            InfoLibInfo infoLibInfo = new InfoLibInfo();
            for (int index = 0; index < this.dataGridView1.Rows.Count; index++)
            {
                bool bChecked = Boolean.Parse(this.dataGridView1.Rows[index].Cells[this.colCheck.Index].Value.ToString());
                if (bChecked)
                {
                    count++;
                    rowIndex = index;
                    infoLibInfo = this.dataGridView1.Rows[index].Tag as InfoLibInfo;
                }
            }
            if (count == 0)
            {
                MessageBoxEx.ShowMessage("��ѡ��Ҫ������ļ�");
                return;
            }
            if (infoLibInfo == null)
                return;

            if (!infoLibInfo.IsFolder && szNewLevel == infoLibInfo.ShareLevel)
                return;

            if (infoLibInfo.CreatorID != SystemContext.Instance.LoginUser.ID)
            {
                MessageBoxEx.Show("��û��Ȩ���޸Ĵ�ģ��Ĺ���ȼ�!");
                return;
            }

            short shRet = SystemConst.ReturnValue.OK;
            GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.WaitCursor);

            shRet = InfoLibService.Instance.ModifyInfoLibShareLevel(infoLibInfo.InfoID, szNewLevel);

            GlobalMethods.UI.SetCursor(this.m_InfoLibForm, Cursors.Default);
            if (shRet == SystemConst.ReturnValue.OK)
            {
                infoLibInfo.ShareLevel = szNewLevel;
                this.dataGridView1.Rows[rowIndex].Cells[this.colShareLevel.Index].Value = ServerData.ShareLevel.TransShareLevel(szNewLevel);
                this.dataGridView1.Rows[rowIndex].Tag = infoLibInfo;
                return;
            }
            MessageBoxEx.Show(string.Format("��{0}��ģ�干��ȼ��޸�ʧ��!", infoLibInfo.InfoName));
        }

        private void mnuSharePersonal_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.PERSONAL);
        }

        private void mnuShareDepart_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.DEPART);
        }

        private void mnuShareHospital_Click(object sender, EventArgs e)
        {
            this.ModifyTempletShareLevel(ServerData.ShareLevel.HOSPITAL);
        }

        private void ShowStatusMassage(string szMessage)
        {
            this.m_InfoLibForm.ShowStatusMessage(szMessage);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int count = 0;
            int rowIndex = 0;
            InfoLibInfo infoLibInfo = new InfoLibInfo();
            for (int index = 0; index < this.dataGridView1.Rows.Count; index++)
            {
                bool bChecked = Boolean.Parse(this.dataGridView1.Rows[index].Cells[this.colCheck.Index].Value.ToString());
                if (bChecked)
                {
                    count++;
                    rowIndex = index;
                    infoLibInfo = this.dataGridView1.Rows[index].Tag as InfoLibInfo;
                }
            }
            if (count == 0)
            {
                MessageBoxEx.ShowMessage("��ѡ��Ҫ������ļ�");
                return;
            }
            if (infoLibInfo == null)
                return;

            InfoLibRenameForm frmInfoLibRename = new InfoLibRenameForm();
            frmInfoLibRename.InfoLibInfo = infoLibInfo;
            if (frmInfoLibRename.ShowDialog() == DialogResult.OK)
            {
                infoLibInfo = frmInfoLibRename.InfoLibInfo;
                InfoLibService.Instance.ModifyInfoLibName(infoLibInfo.InfoID, infoLibInfo.InfoName);
                this.dataGridView1.Rows[rowIndex].Cells[this.colName.Index].Value = frmInfoLibRename.InfoLibInfo.InfoName;
                this.dataGridView1.Rows[rowIndex].Tag = infoLibInfo;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            
            //��ʾ�Ƿ��ϴ��滻ѡ�е��ļ�
            //int count = 0;
           
            string szFilePath = fileDialog.FileName;
            this.m_WinWordDocForm = new WinWordDocForm();
            this.m_WinWordDocForm.OpenDocument(szFilePath);
            this.m_WinWordDocForm.ShowDialog();
        }

        private void toolbtnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            if (this.m_InfoLibInfo == null)
            {
                MessageBoxEx.Show(string.Format("��ѡ���ļ��ϴ�Ŀ¼!"));
                return;
            }
            //��ʾ�Ƿ��ϴ��滻ѡ�е��ļ�
            int count = 0;
            InfoLibInfo infoLibInfo = new InfoLibInfo();
            string szFilePath = fileDialog.FileName;
            short shRet = SystemConst.ReturnValue.OK;
            for (int index = 0; index < this.dataGridView1.Rows.Count; index++)
            {
                bool bChecked = Boolean.Parse(this.dataGridView1.Rows[index].Cells[this.colCheck.Index].Value.ToString());
                if (bChecked)
                {
                    count++;
                    infoLibInfo = this.dataGridView1.Rows[index].Tag as InfoLibInfo;
                }
            }
            if (count > 0)
            {
                if (MessageBoxEx.ShowConfirm("�´����ļ�ʱ��ȡ����ѡ,ȷ��Ҫ�滻ѡ�е��ļ���?") == DialogResult.OK)
                {
                    infoLibInfo.InfoSize = CountSize(new FileInfo(szFilePath).Length);
                    shRet = InfoLibService.Instance.UpdateInfoLibToFtp(infoLibInfo, szFilePath);
                }
            }
            else
            {
                infoLibInfo = this.MakeInfoLibInfo();
                infoLibInfo.InfoName = System.IO.Path.GetFileName(szFilePath);
                infoLibInfo.InfoType = System.IO.Path.GetExtension(fileDialog.FileName);
                infoLibInfo.InfoSize = CountSize(new FileInfo(szFilePath).Length);
                shRet = InfoLibService.Instance.SaveInfoLibToFTP(infoLibInfo, szFilePath);
            }
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show(string.Format("��{0}���ϴ�ʧ��!", infoLibInfo.InfoName));
                return;
            }
            this.ShowInfoLibList();
        }
       
    }
}