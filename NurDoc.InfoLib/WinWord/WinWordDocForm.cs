using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heren.Common.DockSuite;
using Heren.NurDoc.Data;
using Heren.NurDoc.InfoLib.DockForms;
using Heren.NurDoc.DAL;
using Heren.Common.Libraries;

namespace Heren.NurDoc.InfoLib
{
    internal partial class WinWordDocForm : DockContentBase
    {
        private InfoLibInfo m_InfoLibInfo = null;

        public InfoLibInfo InfoLibInfo
        {
            get { return this.m_InfoLibInfo; }
            set { this.m_InfoLibInfo = value; }
        }

        public WinWordDocForm()
        {
            InitializeComponent();
            this.ShowHint = DockState.Document;
            this.DockAreas = DockAreas.Document;
            this.winWordCtrl1.ShowInternalMenuStrip = false;
        }

        /// <summary>
        /// 打开指定的Word文档
        /// </summary>
        /// <param name="szFilePath">文件路径</param>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short OpenDocument(string szFilePath)
        {
            this.winWordCtrl1.OpenDocument(szFilePath);
            return SystemConst.ReturnValue.OK;
        }

        /// <summary>
        /// 关闭当前文档
        /// </summary>
        /// <returns>DataLayer.SystemData.ReturnValue</returns>
        public short CloseDocument()
        {
            this.winWordCtrl1.CloseDocument();
            this.winWordCtrl1.CloseWordApplication();
            return SystemConst.ReturnValue.OK;
        }

        private void WinWordDocForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CloseDocument();
        }
    }
}