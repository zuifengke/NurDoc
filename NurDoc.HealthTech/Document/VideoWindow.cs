using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NurDoc.HealthTech.Document
{
    public partial class VideoWindow : Form
    {
        public VideoWindow()
        {
            InitializeComponent();
        }

        private string m_szURL = string.Empty;

        public string URL
        {
            get { return this.m_szURL; }
            set { this.m_szURL = value; }
        }

        private void VideoWindow_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = m_szURL;// @"http://10.10.76.64:8080/11.mp4";
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
        }
    }
}