using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;
using wtPayDAL.SysAccessDAL;
using wtPayModel.PaymentModel;

namespace wtPay.FormBroadCas
{
    /// <summary>
    /// FormBroadCasStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormBroadCasStep01 : UserControl
    {
        public FormBroadCasStep01()
        {
            InitializeComponent();
        }
        
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length > 0)
            {
                Payment.broadCasPayParam.Account = inputBox.Text;
                Util.JumpUtil.jumpCommonPage("FormBroadCasStep02");
            }
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                //缴费类型
                SysBLL.payCostType = 6;
                SysBLL.Player("缴费账号.wav");
                //初始化文本框
                inputBox.Text = "";
                keyboard.textBox = inputBox;
                //初始化缴费程序参数
                Payment.broadCasPayParam = null;
                Payment.broadCasPayParam = new BroadCasPayParam();
                //设置提示语
                showinfo.Text = PayPromptAccess.getPrompt(GcManage.gcType);
            }
            catch(Exception ex)
            {
                log.Write("error:FormBroadCasStep01:load():" +ex.Message);
            }
        }

        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length > 20)
                {
                    inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length-1,1);
                }
            }catch(Exception ex)
            {
                log.Write("error:FormBroadCasStep01:inputBox_TextChanged:"+ex.Message);
            }
        }

        private void inputBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if(inputBox.Text.Length>20)
            inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length - 1, 1);
        }
    }
}
