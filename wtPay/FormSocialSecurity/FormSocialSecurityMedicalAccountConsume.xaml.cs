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
using wtPayModel.SocialSecurityModel;

namespace wtPay.FormSocialSecurity
{
    /// <summary>
    /// FormSocialSecurityMedicalAccountConsume.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecurityMedicalAccountConsume : UserControl
    {
        List<MedicalAccountConsumeInfoData> list = null;


        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";
        string custNo3 = "";
        public FormSocialSecurityMedicalAccountConsume()
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
            load();
        }
        private void load()
        {
            try
            {
                page = 0;
                list = ((MedicalAccountConsumeInfo)Util.JumpUtil.ParamsMap["info"]).data;
                //清空页面参数
                clear();
                //数据加载
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
                log.Write("error:医疗账户消费信息加载异常："+ex.Message);
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
        void setRow1(MedicalAccountConsumeInfoData info)
        {
            custNo1 = info.grcard;
            namelbl1.Text = info.name;
            hospitalNamelbl1.Text = info.time;
            pricelbl1.Text = info.expense;
            panel1.Visibility = Visibility.Visible;
        }
        void setRow2(MedicalAccountConsumeInfoData info)
        {
            custNo2 = info.grcard;
            namelbl2.Text = info.name;
            hospitalNamelbl2.Text = info.time;
            pricelbl2.Text = info.expense;
            panel2.Visibility = Visibility.Visible;
        }
        void setRow3(MedicalAccountConsumeInfoData info)
        {
            custNo3 = info.grcard;
            namelbl3.Text = info.name;
            hospitalNamelbl3.Text = info.time;
            pricelbl3.Text = info.expense;
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

        void setText(MedicalAccountConsumeInfoData info)
        {
            clearText();
            grcard.Text = info.grcard;
            name.Text = info.name;
            idcard.Text = info.idcard;
            hospitalNumber.Text = info.hospitalNumber;
            hospitalName.Text = info.hospitalName;
            time.Text = info.time;
            expense.Text = info.expense;
        }
        void clearText()
        {
            grcard.Text = "";
            name.Text = "";
            idcard.Text = "";
            hospitalNumber.Text = "";
            hospitalName.Text = "";
            time.Text = "";
            expense.Text = "";
        }
    }
}
