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
using wtPayModel.SocialSecurityModel;

namespace wtPay.FormSocialSecurity
{
    /// <summary>
    /// FormSocialSecurityMedicalAccountConsume.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecurityPensionGrant : UserControl
    {
        PensionGrantInfo info = null;


        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";
        string custNo3 = "";
        public FormSocialSecurityPensionGrant()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityPensionAccount");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page = page - 1;
                setPage(page);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int count = page;
            count++;
            if (count < pageCount)
            {
                page = page + 1;
                setPage(page);
            }
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            page = 0;
            load();
        }
        private void load()
        {
            try
            {
                info = (PensionGrantInfo)Util.JumpUtil.ParamsMap["info"];
                //清空页面参数
                clear();
                //数据加载
                pageCount = info.data.Count / 3;
                if ((info.data.Count % 3) != 0)
                {
                    pageCount++;
                }
                setPage(page);
                setText(info.data[0]);
            }
            catch(Exception ex)
            {
                log.Write("error:医疗账户消费信息加载异常："+ex.Message);
            }
        }
        int i1 = 0, i2 = 0, i3 = 0;
        //设置页面
        void setPage(int page)
        {
            try {
                if (info.data == null) return;
                clear();
                int count = 0;
                for (int i = page * 3; i < info.data.Count; i++)
                {
                    count++;
                    if (count == 1) { setRow1(info.data[i]); i1 = i; }
                    if (count == 2) { setRow2(info.data[i]); i2 = i; }
                    if (count == 3) { setRow3(info.data[i]); i3 = i; }
                    if (count == 3)
                    {
                        break;
                    }
                }
            }catch(Exception ex)
            {
                log.Write("error:FormSocialSecurityMedicalAccountConsume:setPage():"+ex.Message);
            }
        }
        void clear()
        {
            panel1.Visibility = Visibility.Hidden;
            panel2.Visibility = Visibility.Hidden;
            panel3.Visibility = Visibility.Hidden;
        }
        //行一
        void setRow1(PensionGrantInfoData info)
        {
            row_remark1.Text = info.remark3;
            grcard1.Text = info.grcard;
            qstime1.Text = info.time;
            panel1.Visibility = Visibility.Visible;
        }
        void setRow2(PensionGrantInfoData info)
        {
            row_remark2.Text = info.remark3;
            grcard2.Text = info.grcard;
            qstime2.Text = info.time;
            panel2.Visibility = Visibility.Visible;
        }
        void setRow3(PensionGrantInfoData info)
        {
            row_remark3.Text = info.remark3;
            grcard3.Text = info.grcard;
            qstime3.Text = info.time;
            panel3.Visibility = Visibility.Visible;
        }

        private void panel1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(info.data[i1]);
        }

        private void panel2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(info.data[i2]);
        }

        private void panel3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(info.data[i3]);
        }

        void setText(PensionGrantInfoData info)
        {
            clearText();
            grcard.Text = info.grcard;
            time.Text = info.time;
            amountDue.Text = info.amountDue;
            //actualAmount.Text = info.actualAmount;
            remark1.Text = info.remark1;
            remark2.Text = info.remark2;
            remark3.Text = info.remark3;
        }
        void clearText()
        {
            grcard.Text = "";
        }
    }
}
