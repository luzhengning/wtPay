using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wtPayBLL;

namespace wtPay
{
    public partial class FormLoad : Form
    {
        public FormLoad()
        {
            InitializeComponent();
            //隐藏Windows任务栏
            SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_HIDE);
        }

        private void FormLoad_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void FormLoad_Load(object sender, EventArgs e)
        {
            
        }
    }
}
