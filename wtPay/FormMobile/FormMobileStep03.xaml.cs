﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using wtPayModel.PaymentModel;

namespace wtPay.FormMobile
{
    /// <summary>
    /// FormMobileStep3.xaml 的交互逻辑
    /// </summary>
    public partial class FormMobileStep03 : UserControl
    {
        public FormMobileStep03()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try {
                //SysBLL.payCostType = 3;
                if (txtRechargeAmount.Text.Length == 0)
                {
                    this.lblShowInfo1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red"));
                    this.lblShowInfo1.Text = "充值金额必须大于0";
                    return;
                }
                if (txtRechargeAmount.Text.Substring(0, 1).Equals("0"))
                {
                    return;
                }
                if (txtRechargeAmount.Text.Length>4)
                {
                    return;
                }
                //payParam.rechageAmount = txtRechargeAmount.Text;

                int userPay = Convert.ToInt32(txtRechargeAmount.Text);
                if (userPay > 500)
                {
                    this.lblShowInfo1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red"));
                    this.lblShowInfo1.Text = "提示：单笔最大缴费金额不能超过500元，请重新输入！";
                }
                else
                {
                    //输入金额
                    Payment.mobilePayParam.UserInputMoney = txtRechargeAmount.Text;
                   Util.JumpUtil.jumpCommonPage("FormReadCard");
                }
            }catch(Exception ex)
            {
                log.Write("error:FormMobileStep03:确定_Click:"+ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormMobileStep01");
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                lblShowInfo1.Foreground = Brushes.White;
                SysBLL.Player("请输入充值金额.wav");
                this.lblShowInfo1.Text = "提示：话费最大充值金额为500元";
                //初始化文本框
                txtRechargeAmount.Text = "";
                keyboard.textBox = this.txtRechargeAmount;
            }
            catch (Exception ex)
            {
                log.Write("error：FormMobileStep03：load():" + ex.Message);
            }
        }

        private void txtRechargeAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtRechargeAmount.Text.Length > 3)
                {
                    txtRechargeAmount.Text = txtRechargeAmount.Text.Remove(txtRechargeAmount.Text.Length - 1, 1);
                }
                if (Convert.ToInt32(txtRechargeAmount.Text) > 500)
                {
                    lblShowInfo1.Foreground = Brushes.Red;
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }
}
