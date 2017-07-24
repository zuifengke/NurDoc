// **************************************************************
// 护理电子病历系统公用模块之文本模板编辑窗口
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
        /// 获取或设置当前模板的信息
        /// </summary>
        [Browsable(false)]
        public InfoLibInfo InfoLibInfo
        {
            get { return this.m_InfoLibInfo; }
            set { this.m_InfoLibInfo = value; }
        }

        private bool m_bIsModified = false;

        /// <summary>
        /// 获取或设置当前模板是否已修改
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
        /// 显示文档列表
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
                MessageBoxEx.Show("您创建的护理信息列表下载失败!");
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
        /// 下载并打开指定的文本模板
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
                MessageBoxEx.Show(string.Format("“{0}”文本模板下载失败!", this.m_InfoLibInfo.InfoName));
                return SystemConst.ReturnValue.FAILED;
            }

            string szTempletContent = string.Empty;
            if (!GlobalMethods.Convert.BytesToString(byteTempletData, ref szTempletContent))
            {
                GlobalMethods.UI.SetCursor(this, Cursors.Default);
                MessageBoxEx.Show(string.Format("“{0}”文本模板加载失败!", this.m_InfoLibInfo.InfoName));
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
        /// 生成一个新的模板信息对象
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
                MessageBoxEx.Show("文档下载失败");
                return;
            }
            shRet = this.m_WinWordDocForm.OpenDocument(szLocalFilePath);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("打开文档失败");
                return;
            }
            GlobalMethods.UI.SetCursor(this, Cursors.Default);
            if (shRet != SystemConst.ReturnValue.OK)
                return;
            m_WinWordDocForm.ShowDialog();
        }

        /// <summary>
        /// 关键字检索
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
                MessageBoxEx.Show("您创建的护理信息列表下载失败!");
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
        /// 获取用户选中的模板文本信息
        /// </summary>
        private void ShowSelectedInfoLib(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null)
            {
                return;
            }

            string szCreatorName = null;
            if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.CreatorName))
                szCreatorName = "作者不详";
            else
                szCreatorName = infoLibInfo.CreatorName;

            string szDeptName = null;
            if (GlobalMethods.Misc.IsEmptyString(infoLibInfo.WardName))
                szDeptName = "科室不详";
            else
                szDeptName = infoLibInfo.WardName;

            string szLastModifyTime = null;
            if (infoLibInfo.ModifyTime == infoLibInfo.DefaultTime)
                szLastModifyTime = "最后修改时间不详";
            else
                szLastModifyTime = infoLibInfo.ModifyTime.ToString("yyyy-MM-dd HH:mm");

            this.ShowStatusMassage(string.Format("{0}：{1}，{2}，{3}，{4}，{5}"
                , infoLibInfo.IsFolder ? "目录" : "模板", infoLibInfo.InfoName, szCreatorName
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
                if (MessageBoxEx.ShowConfirm("确认删除文件吗") == DialogResult.OK)
                {
                    short shRet = InfoLibService.Instance.DeleteInfoLibToFTP(infoLibInfo);
                    if (shRet != SystemConst.ReturnValue.OK)
                    {
                        MessageBoxEx.Show("删除文件失败");
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
                MessageBoxEx.ShowMessage("请选择要下载的文件");
                return;
            }
            //下载文档
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = infoLibInfo.InfoName;
            fileDialog.Filter = "office word 2003(*.doc)|*.doc|所有文件(*.*)|*.*";
            fileDialog.Title = "另存为";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string szFileName = fileDialog.FileName;
                string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(infoLibInfo);
                short shRet = InfoLibService.Instance.DownloadInfoLibFile(szRemoteFile, szFileName);
                if (shRet != SystemConst.ReturnValue.OK)
                {
                    MessageBoxEx.ShowMessage("下载失败");
                    return;
                }
                MessageBoxEx.ShowMessage("下载成功");
            }
        }

        /// <summary>
        /// 修改选中模板的共享等级
        /// </summary>
        /// <param name="szNewLevel">新的共享等级</param>
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
                MessageBoxEx.ShowMessage("请选择要共享的文件");
                return;
            }
            if (infoLibInfo == null)
                return;

            if (!infoLibInfo.IsFolder && szNewLevel == infoLibInfo.ShareLevel)
                return;

            if (infoLibInfo.CreatorID != SystemContext.Instance.LoginUser.ID)
            {
                MessageBoxEx.Show("您没有权限修改此模板的共享等级!");
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
            MessageBoxEx.Show(string.Format("“{0}”模板共享等级修改失败!", infoLibInfo.InfoName));
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
                MessageBoxEx.ShowMessage("请选择要共享的文件");
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
            
            //提示是否上传替换选中的文件
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
                MessageBoxEx.Show(string.Format("请选择文件上传目录!"));
                return;
            }
            //提示是否上传替换选中的文件
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
                if (MessageBoxEx.ShowConfirm("新创建文件时请取消勾选,确认要替换选中的文件吗?") == DialogResult.OK)
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
                MessageBoxEx.Show(string.Format("“{0}”上传失败!", infoLibInfo.InfoName));
                return;
            }
            this.ShowInfoLibList();
        }
       
    }
}