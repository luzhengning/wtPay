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
    /// FormSocialSecurityInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecurityInfo : UserControl
    {
        List<SocialSecurityInfoData> list = null;

        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";
        string custNo3 = "";

        public FormSocialSecurityInfo()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityInput");
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
        int count = 0;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            count = page;
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
                list = ((SocialSecurityInfo)Util.JumpUtil.ParamsMap["info"]).data;
                //清空页面参数
                clear();
                //显示数据
                pageCount = list.Count / 3;
                if ((list.Count % 3) != 0)
                {
                    pageCount++;
                }
                setPage(page);
                setText(list[0]);
            }
            catch (Exception e)
            {
                log.Write("error:个人参保信息结果页面异常："+e.Message);
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
                log.Write("error:FormSocialSecurityInfo:setPage():"+ex.Message);
            }
        }
        void clear()
        {
            panel1.Visibility = Visibility.Hidden;
            panel2.Visibility = Visibility.Hidden;
            panel3.Visibility = Visibility.Hidden;
        }
        //行一
        void setRow1(SocialSecurityInfoData info)
        {
            custNo1 = info.grcard;
            timelbl1.Text = info.time;
            grcardlbl1.Text = info.grcard;
            typelbl1.Text = info.insuredType;
            panel1.Visibility = Visibility.Visible;
        }
        void setRow2(SocialSecurityInfoData info)
        {
            custNo2 = info.grcard;
            timelbl2.Text = info.time;
            grcardlbl2.Text = info.grcard;
            typelbl2.Text = info.insuredType;
            panel2.Visibility = Visibility.Visible;
        }
        void setRow3(SocialSecurityInfoData info)
        {
            custNo3 = info.grcard;
            timelbl3.Text = info.time;
            grcardlbl3.Text = info.grcard;
            typelbl3.Text = info.insuredType;
            panel3.Visibility = Visibility.Visible;
        }

        //面板点击事件
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

        void setText(SocialSecurityInfoData info)
        {
            clearText();
            time.Text = info.time;
            grcard.Text = info.grcard;
            CompanyNo.Text = info.CompanyNo;
            CompanyName.Text = info.CompanyName;
            insuredType.Text = info.insuredType;
            state.Text = info.state;
        }
        void clearText()
        {
            time.Text = "";
            grcard.Text = "";
            CompanyNo.Text = "";
            CompanyName.Text = "";
            insuredType.Text = "";
            state.Text = "";
        }
    }
}
