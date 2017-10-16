using System;
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
using wtPayModel.PublicFundModel;

namespace wtPay.FormPublicFund
{
    /// <summary>
    /// FormPublicLoanDetailedInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicLoanDetailedInfo : UserControl
    {
        List<LoanDetailedInfoData> list = null;

        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";
        string custNo3 = "";

        public FormPublicLoanDetailedInfo()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFund");
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {

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
            load();
        }
        private void load()
        {
            try
            {
                //获取查询结果
                list = ((LoanDetailedInfo)Util.JumpUtil.ParamsMap["info"]).data;

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
                log.Write("error:个人公积金贷款还款明细加载异常："+ex.Message);
            }
        }
        int row1 = 0, row2 = 0, row3 = 0;
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
                    if (count == 1) { setRow1(list[i]); row1 = i; }
                    if (count == 2) { setRow2(list[i]); row2 = i; }
                    if (count == 3) { setRow3(list[i]); row3 = i; }
                    if (count == 3)
                    {
                        break;
                    }
                }
            }catch(Exception ex)
            {
                log.Write("error:FormpublicLoanDetailedInfo:setPage():"+ex.Message);
            }
        }
        void clear()
        {
            panel1.Visibility = Visibility.Hidden;
            panel2.Visibility = Visibility.Hidden;
            panel3.Visibility = Visibility.Hidden;
        }
        //行一
        void setRow1(LoanDetailedInfoData info)
        {
            custNo1 = info.data1;
            data1lbl1.Text = info.data1;
            data3lbl1.Text = info.data3;
            data4lbl1.Text = info.data4;
            panel1.Visibility = Visibility.Visible;
        }
        void setRow2(LoanDetailedInfoData info)
        {
            custNo2 = info.data1;
            data1lbl2.Text = info.data1;
            data3lbl2.Text = info.data3;
            data4lbl2.Text = info.data4;
            panel2.Visibility = Visibility.Visible;
        }
        void setRow3(LoanDetailedInfoData info)
        {
            custNo3 = info.data1;
            data1lbl3.Text = info.data1;
            data3lbl3.Text = info.data3;
            data4lbl3.Text = info.data4;
            panel3.Visibility = Visibility.Visible;
        }

        private void panel1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(list[row1]);
        }

        private void panel1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void panel1_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            setText(list[row1]);
        }

        private void panel2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(list[row2]);
        }

        private void panel3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            setText(list[row3]);
        }

        void setText(LoanDetailedInfoData info)
        {
            clearText();
            data1.Text = info.data1;
            data2.Text = info.data2;
            data3.Text = info.data3;
            data4.Text = info.data4;
            data5.Text = info.data5;
            data6.Text = info.data6;
            data7.Text = info.data7;
            data8.Text = info.data8;
        }
        void clearText()
        {
            data1.Text = "";
            data2.Text = "";
            data3.Text = "";
            data4.Text = "";
            data5.Text = "";
            data6.Text = "";
            data7.Text = "";
            data8.Text = "";

        }
    }
}
