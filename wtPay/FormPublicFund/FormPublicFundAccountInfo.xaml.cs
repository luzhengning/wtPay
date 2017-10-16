using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using wtPayModel.PublicFundModel;

namespace wtPay.FormPublicFund
{
    /// <summary>
    /// FormPublicFundAccountInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicFundAccountInfo : UserControl
    {
        List<PublicAccountInfoDatas> list = null;

        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";
        string custNo3 = "";
        public FormPublicFundAccountInfo()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFundCardType");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFund");
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

        //窗体load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void load()
        {
            try
            {
                //获取查询的数据
                list=((PublicAccountInfo)Util.JumpUtil.ParamsMap["info"]).data;

                clear();
                pageCount = list.Count / 3;
                if ((list.Count % 3) != 0)
                {
                    pageCount++;
                }
                setPage(page);
                setText(list[0]);
            }
            catch(Exception ex)
            {
                log.Write("error：FormPublicFundAccountInfo：load()：" + ex.Message);
            }
        }
        int i1 = 0, i2 = 0, i3 = 0;
        //设置页面
        void setPage(int page)
        {
            try {
                if (list == null) return;
                clear();
                int count = 0;
                for (int i = page * 3; i < list.Count; i++)
                {
                    count++;
                    if (count == 1) { setRow1(list[i]); i1 = i; }
                    if (count == 2) { setRow2(list[i]); i2 = i; }
                    if (count == 3) { setRow3(list[i]); i3 = i; }
                    if (count == 3)
                    {
                        break;
                    }
                }
            }catch(Exception ex)
            {
                log.Write("error：FormPublicFundAccountInfo：setPage()：" + ex.Message);
            }
        }
        void clear()
        {
            panel1.Visibility = Visibility.Hidden;
            panel2.Visibility = Visibility.Hidden;
            panel3.Visibility = Visibility.Hidden;
        }
        //行一
        void setRow1(PublicAccountInfoDatas info)
        {
            custNo1 = info.data1;
            data2lbl1.Text = info.data2;
            data3lbl1.Text = info.data3;
            data8lbl1.Text = info.data8;
            panel1.Visibility = Visibility.Visible;
        }
        void setRow2(PublicAccountInfoDatas info)
        {
            custNo2 = info.data1;
            data2lbl2.Text = info.data2;
            data3lbl2.Text = info.data3;
            data8lbl2.Text = info.data8;
            panel2.Visibility = Visibility.Visible;
        }
        void setRow3(PublicAccountInfoDatas info)
        {
            custNo3 = info.data1;
            data2lbl3.Text = info.data2;
            data3lbl3.Text = info.data3;
            data8lbl3.Text = info.data8;
            panel3.Visibility = Visibility.Visible;
        }

        private void panel1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(list[i1]);
        }

        private void panel2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(list[i2]);
        }

        private void panel3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(list[i3]);
        }

        void setText(PublicAccountInfoDatas info)
        {
            clearText();
            data2.Text = info.data2;
            data3.Text = info.data3;
            data4.Text = info.data4;
            data5.Text = info.data5;
            data6.Text = info.data6;
            data7.Text = info.data7;
            data9.Text = info.data9;
        }
        void clearText()
        {
            data2.Text = "";
            data3.Text = "";
            data4.Text = "";
            data5.Text = "";
            data6.Text = "";
            data7.Text = "";
            data9.Text = "";
        }
    }
}
