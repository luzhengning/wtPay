using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wtPayBLL;

namespace FormPage
{
    public partial class FormLoad : Form
    {
        public FormLoad()
        {
            InitializeComponent();
            
        }

        private void FormLoad_Load(object sender, EventArgs e)
        {
            //隐藏Windows任务栏
            SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_HIDE);
            //设置鼠标是否可见
            SysBLL.ShowCursor(SysBLL.IsShowCursor);
        }
    }
}
