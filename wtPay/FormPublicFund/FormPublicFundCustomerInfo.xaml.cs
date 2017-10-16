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
    /// FormPublicFundCustomerInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicFundCustomerInfo : UserControl
    {
        CustomerInfo info = null;
        public FormPublicFundCustomerInfo()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFund");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        void load()
        {
            try {
                //获取查询的结果
                info=(CustomerInfo)Util.JumpUtil.ParamsMap["info"];

                pername.Text = info.data.pername;
                sex.Text = getsex(info.data.sex);
                cardtype.Text = setCardType(info.data.cardtype);
                cardcode.Text = info.data.cardcode;
                birthday.Text = info.data.birthday;
                phone.Text = info.data.phone;
                email.Text = info.data.email;
                lblNo.Text = info.data.percode;
            }catch(Exception ex)
            {
                log.Write("error:FormPublicFundCustomerInfo：load():" + ex.Message);
            }
        }
        string getsex(string value)
        {
            if ("1".Equals(value))
            {
                return "男";
            }
            else
            {
                return "女";
            }

        }
        string setCardType(string type)
        {
            switch (type.Trim())
            {
                case "01":
                    return "身份证";
                case "02":
                    return "军官证";
                case "03":
                    return "护照";
                case "99":
                    return "其他";
                default:
                    return "";
            }
        }
    }
}
