using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Heren.NurDoc.HealthTech;
using Heren.Common.Libraries;
using Heren.Common.Controls;
using Heren.NurDoc.DAL;
using Heren.NurDoc.Data;
using Heren.NurDoc.Frame;
using Word;

namespace NurDoc.HealthTech
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 文件夹列表
        /// </summary>
        List<InfoLibInfo> m_lstInfoLibInfos = new List<InfoLibInfo>();
        string m_szHealthTechIISAddress = string.Empty;
        string m_szHealthTypeID = "HEALTH_TECH";

        private bool m_bSwitchSystemUser = false;

        /// <summary>
        /// 登陆用户
        /// </summary>
        private UserInfo m_loginUser = null;

        public MainForm()
        {
            InitializeComponent();
        }

        public UserInfo loginUser
        {
            get { return this.m_loginUser; }
            set { this.m_loginUser = value; }
        }

        public bool SwitchSystemUser(UserInfo NewLoginUser)
        {
            if (m_loginUser == NewLoginUser)
                return true;
            m_bSwitchSystemUser = true;
            m_loginUser = NewLoginUser;
            LoadContextMenu();

            return true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_szHealthTechIISAddress))
                m_szHealthTechIISAddress = ServerParam.Instance.GetHealthTechIISAddress();
            if (m_bSwitchSystemUser)
                return;
            LoadContextMenu();
        }

        private bool LoadContextMenu()
        {
            ///下载数据
            m_lstInfoLibInfos.Clear();
            short shRet = InfoLibService.Instance.GetAllLibInfosFolder(ref m_lstInfoLibInfos);
            if (shRet != ServerData.ExecuteResult.OK && shRet != ServerData.ExecuteResult.RES_NO_FOUND)
            {
                MessageBoxEx.Show("数据检索出错！");
                this.Close();
                return false;
            }

            menuStrip1.Items.Clear();
            ///加载一级目录
            foreach (InfoLibInfo InfoLibInfo in m_lstInfoLibInfos)
            {
                if (InfoLibInfo.ParentID == this.m_szHealthTypeID)
                {
                    ToolStripMenuItem subItem;
                    subItem = AddContextMenu(InfoLibInfo, menuStrip1.Items, null);
                }
            }

            ToolStripMenuItem tsmi = new ToolStripMenuItem("科室常用");
            tsmi.Tag = "科室常用";
            EventHandler callback = new EventHandler(MenuClicked);
            if (callback != null) tsmi.Click += callback;
            menuStrip1.Items.Add(tsmi);

            this.AddQuickFind(this.m_szHealthTypeID);
            //AddContextMenu("科室常用", subItem.DropDownItems, new EventHandler(MenuClicked));

            return true;
        }

        private ToolStripMenuItem AddContextMenu(InfoLibInfo InfoLibInfo, ToolStripItemCollection cms, EventHandler callback)
        {
            if (!string.IsNullOrEmpty(InfoLibInfo.InfoName) && InfoLibInfo.IsFolder)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(InfoLibInfo.InfoName);
                tsmi.Tag = InfoLibInfo;
                if (callback != null) tsmi.Click += callback;
                cms.Add(tsmi);
                foreach (InfoLibInfo InfoLibInfoSub in m_lstInfoLibInfos)
                {
                    if (InfoLibInfoSub.ParentID == InfoLibInfo.InfoID)
                        AddStripMenuMenu(InfoLibInfoSub, tsmi, new EventHandler(MenuClicked));
                }
                return tsmi;
            }
            return null;
        }

        private ToolStripMenuItem AddStripMenuMenu(InfoLibInfo InfoLibInfo, ToolStripMenuItem cms, EventHandler callback)
        {
            if (!string.IsNullOrEmpty(InfoLibInfo.InfoName) && InfoLibInfo.IsFolder)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(InfoLibInfo.InfoName);
                tsmi.Tag = InfoLibInfo;

                if (callback != null) tsmi.Click += callback;
                cms.DropDownItems.Add(tsmi);

                foreach (InfoLibInfo InfoLibInfoSub in m_lstInfoLibInfos)
                {
                    if (InfoLibInfoSub.ParentID == InfoLibInfo.InfoID)
                        AddStripMenuMenu(InfoLibInfoSub, tsmi, new EventHandler(MenuClicked));
                }

                return tsmi;
            }

            return null;
        }

        private void reflashDataGrid(string szInfoLibInfoID)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Tag = szInfoLibInfoID;
            AddSubFileListToDataGrid(szInfoLibInfoID);
        }

        /// <summary>
        /// 设置显示的文件夹地址
        /// </summary>
        private void SetAddress()
        {
            this.lblAddress.Text = GetAddress(string.Empty);
        }

        private string GetAddress(string szParentID)
        {
            string szInfoLibInfoID = szParentID;
            if (string.IsNullOrEmpty(szParentID))
                szInfoLibInfoID = dataGridView1.Tag as string;
            if (string.IsNullOrEmpty(szInfoLibInfoID) && string.IsNullOrEmpty(szParentID))
                return string.Empty;

            string szAddress = string.Empty;

            InfoLibInfo InfoLibInfo = GetParentInfoByID(szInfoLibInfoID);
            if (InfoLibInfo == null)
                return string.Empty;
            szAddress = InfoLibInfo.InfoName;
            if (InfoLibInfo.ParentID != this.m_szHealthTypeID && !string.IsNullOrEmpty(InfoLibInfo.ParentID))
            {
                InfoLibInfo SubInfoLibInfo = GetParentInfoByID(InfoLibInfo.ParentID);
                szAddress = SubInfoLibInfo.InfoName + ">>" + szAddress;
                if (SubInfoLibInfo.IsFolder)
                {
                    string szaddress1 = GetAddress(SubInfoLibInfo.ParentID);
                    if (!string.IsNullOrEmpty(szaddress1))
                        szAddress = GetAddress(SubInfoLibInfo.ParentID) + ">>" + szAddress;
                }
            }
            return szAddress;
        }

        /// <summary>
        /// 添加数据至列表
        /// </summary>
        /// <param name="szInfoLibInfoID">信息库文件ID号</param>
        private void AddSubFileListToDataGrid(string szInfoLibInfoID)
        {
            bool bchangeColor = true;
            foreach (InfoLibInfo InfoLibInfo in m_lstInfoLibInfos)
            {
                //筛选显示数据
                if ((!InfoLibInfo.IsFolder && InfoLibInfo.ParentID == szInfoLibInfoID)
                    && (InfoLibInfo.ShareLevel == ServerData.ShareLevel.HOSPITAL
                    || (InfoLibInfo.ShareLevel == ServerData.ShareLevel.DEPART && InfoLibInfo.WardCode == m_loginUser.WardCode)
                    || (InfoLibInfo.ShareLevel == ServerData.ShareLevel.PERSONAL && InfoLibInfo.CreatorID == m_loginUser.ID)))
                {
                    int index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[this.ColRowNo.Index].Value = index + 1;
                    dataGridView1.Rows[index].Cells[this.ColName.Index].Value = InfoLibInfo.InfoName;
                    dataGridView1.Rows[index].Cells[this.ColType.Index].Value = InfoLibInfo.InfoType;// "类型未知";
                    dataGridView1.Rows[index].Cells[this.ColFileSize.Index].Value = InfoLibInfo.InfoSize;// "大小未知";
                    dataGridView1.Rows[index].Cells[this.ColUpdateTime.Index].Value = InfoLibInfo.ModifyTime.ToString();
                    dataGridView1.Rows[index].Cells[this.ColStatue.Index].Value = "常用未知";
                    dataGridView1.Rows[index].Cells[this.ColFrom.Index].Value = GetParentNameByID(InfoLibInfo.ParentID);
                    if (bchangeColor)
                    {
                        Color color = Color.FromArgb(215, 228, 242);
                        for (int icellIndex = 0; icellIndex < dataGridView1.Rows[0].Cells.Count; icellIndex++)
                        {
                            dataGridView1.Rows[index].Cells[icellIndex].Style.BackColor = color;
                        }
                    }
                    bchangeColor = !bchangeColor;
                    dataGridView1.Rows[index].Tag = InfoLibInfo;
                }
                else if (InfoLibInfo.IsFolder && InfoLibInfo.ParentID == szInfoLibInfoID)
                {
                    AddSubFileListToDataGrid(InfoLibInfo.InfoID);
                }
            }
        }

        /// <summary>
        /// 添加快捷搜索列表
        /// </summary>
        /// <param name="szParrentID">父文件夹ID</param>
        /// <returns>是否成功</returns>
        private bool AddQuickFind(string szParrentID)
        {
            ///加载一级目录
            foreach (InfoLibInfo InfoLibInfo in m_lstInfoLibInfos)
            {
                if (InfoLibInfo.ParentID == szParrentID)
                {
                    if (InfoLibInfo.IsFolder == false)
                    {
                        this.cboQuickFind.Items.Add(InfoLibInfo.InfoName);
                    }
                    else
                    {
                        this.AddQuickFind(InfoLibInfo.InfoID);
                    }
                }
            }

            //for (int index = 0; index < this.m_lstInfoLibInfos.Count - 1; index++)
            //{
            //    if(m_lstInfoLibInfos[index].ParentID=="")
            //    this.cboQuickFind.Items.Add(this.dataGridView1.Rows[index].Cells[this.ColName.Index].Value);
            //}
            return true;
        }

        private void ShowSearchResalut(string szsearchKey, string szParrentID)
        {
            //
            bool bchangeColor = true;
            foreach (InfoLibInfo InfoLibInfo in m_lstInfoLibInfos)
            {
                if (InfoLibInfo.ParentID == szParrentID)
                {
                    if (InfoLibInfo.IsFolder == false)
                    {
                        if (InfoLibInfo.InfoName.ToUpper().Contains(szsearchKey.ToUpper()))
                        //this.cboQuickFind.Items.Add(InfoLibInfo.InfoName);
                        {
                            int index = dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].Cells[this.ColRowNo.Index].Value = index + 1;
                            dataGridView1.Rows[index].Cells[this.ColName.Index].Value = InfoLibInfo.InfoName;
                            dataGridView1.Rows[index].Cells[this.ColType.Index].Value = InfoLibInfo.InfoType;// "类型未知";
                            dataGridView1.Rows[index].Cells[this.ColFileSize.Index].Value = InfoLibInfo.InfoSize;// "大小未知";
                            dataGridView1.Rows[index].Cells[this.ColUpdateTime.Index].Value = InfoLibInfo.ModifyTime.ToString();
                            dataGridView1.Rows[index].Cells[this.ColStatue.Index].Value = "常用未知";
                            dataGridView1.Rows[index].Cells[this.ColFrom.Index].Value = GetParentNameByID(InfoLibInfo.ParentID);
                            if (bchangeColor)
                            {
                                Color color = Color.FromArgb(215, 228, 242);
                                for (int icellIndex = 0; icellIndex < dataGridView1.Rows[0].Cells.Count; icellIndex++)
                                {
                                    dataGridView1.Rows[index].Cells[icellIndex].Style.BackColor = color;
                                }
                            }
                            bchangeColor = !bchangeColor;
                            dataGridView1.Rows[index].Tag = InfoLibInfo;
                        }
                    }
                    else
                    {
                        this.ShowSearchResalut(szsearchKey, InfoLibInfo.InfoID);
                    }
                }
            }
        }

        private string GetParentNameByID(string szInfoLibInfoID)
        {
            foreach (InfoLibInfo InfoLibInfo in m_lstInfoLibInfos)
            {
                if (InfoLibInfo.InfoID == szInfoLibInfoID)
                {
                    return InfoLibInfo.InfoName;
                }
            }
            return string.Empty;
        }

        private InfoLibInfo GetParentInfoByID(string szInfoLibInfoID)
        {
            foreach (InfoLibInfo InfoLibInfo in m_lstInfoLibInfos)
            {
                if (InfoLibInfo.InfoID == szInfoLibInfoID)
                {
                    return InfoLibInfo;
                }
            }
            return null;
        }

        private void MenuClicked(object sender, EventArgs e)
        {
            InfoLibInfo InfoLibInfo = (sender as ToolStripMenuItem).Tag as InfoLibInfo;
            if (InfoLibInfo == null)
                return;

            reflashDataGrid(InfoLibInfo.InfoID);
            //收起菜单
            for (int index = 0; index < this.menuStrip1.Items.Count; index++)
            {
                ToolStripDropDownItem item = this.menuStrip1.Items[index] as ToolStripDropDownItem;
                item.HideDropDown();
            }
            //this.InvokeGotFocus(this.dataGridView1, e);
            SetAddress();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            InfoLibInfo InfoLibInfo = dataGridView1.Rows[e.RowIndex].Tag as InfoLibInfo;
            if (InfoLibInfo.InfoType == ServerData.InfoLibFileType.Mp4
                || InfoLibInfo.InfoType == ServerData.InfoLibFileType.WMA)
            {
                OpenVideoFile(InfoLibInfo);
            }
            else if (InfoLibInfo.InfoType == ServerData.InfoLibFileType.Docx
                || InfoLibInfo.InfoType == ServerData.InfoLibFileType.Doc)
            {
                OpenDocFile(InfoLibInfo);
            }
        }

        private bool m_bSaveMode = true;

        private void OpenVideoFile(InfoLibInfo InfoLibInfo)
        {
            //安全模式,需要验证密码
            if (this.m_bSaveMode)
            {
                InputPwdForm inputPwdForm = new InputPwdForm();
                if (inputPwdForm.ShowDialog() != DialogResult.OK)
                    return;
            }

            string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(InfoLibInfo);
            szRemoteFile = szRemoteFile.Replace("/INFOLIB", m_szHealthTechIISAddress);
            NurDoc.HealthTech.Document.VideoWindow VideoWindow = new NurDoc.HealthTech.Document.VideoWindow();
            VideoWindow.URL = szRemoteFile;
            VideoWindow.ShowDialog();
            VideoWindow.Dispose();
        }

        private WinWordDocForm m_WinWordDocForm = null;

        private void OpenDocFile(InfoLibInfo infoLibInfo)
        {
            if (infoLibInfo == null)
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

        private string GetFileFtpPath(InfoLibInfo InfoLibInfo)
        {
            string szFileName = InfoLibInfo.InfoName;// +InfoLibInfo.InfoType;
            string szlocalSavePath = ServerParam.Instance.WorkPath + "\\Catch\\" + szFileName;
            string szRemoteFile = ServerParam.Instance.GetFtpInfoPath(InfoLibInfo);
            short shRet = InfoLibService.Instance.DownloadInfoLibFile(szRemoteFile, szlocalSavePath);
            if (shRet != ServerData.ExecuteResult.OK)
                return string.Empty;

            return szlocalSavePath;
        }

        private void cboQuickFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            string szSelected = ((System.Windows.Forms.ComboBox)(sender)).SelectedItem.ToString();
            this.dataGridView1.Rows.Clear();
            ShowSearchResalut(szSelected, m_szHealthTypeID);
            //注释当前grid内快速检索
            //for (int index = 0; index < this.dataGridView1.Rows.Count - 1; index++)
            //{
            //    if (szSelected == this.dataGridView1.Rows[index].Cells[this.ColName.Index].Value.ToString())
            //    {
            //        this.dataGridView1.Rows[index].Selected = true;
            //        return;
            //    }
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string szText = this.cboQuickFind.Text;

            this.dataGridView1.Rows.Clear();
            this.lblAddress.Text = string.Empty;
            ShowSearchResalut(szText, m_szHealthTypeID);
        }

        private void cboQuickFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                string szText = this.cboQuickFind.Text;
                this.dataGridView1.Rows.Clear();
                this.lblAddress.Text = string.Empty;
                ShowSearchResalut(szText, m_szHealthTypeID);
            }
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            //改变安全模式，需要验证密码
            InputPwdForm inputPwdForm = new InputPwdForm();
            if (inputPwdForm.ShowDialog() != DialogResult.OK)
                return;

            if (this.m_bSaveMode)
                this.btnLock.ImageIndex = 1;
            else
                this.btnLock.ImageIndex = 0;
            this.m_bSaveMode = !this.m_bSaveMode;
        }
    }
}