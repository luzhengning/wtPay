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
    /// FormPublicLoanBalanceInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicLoanBalanceInfo : UserControl
    {
        LoanBalanceInfo info = null;
        public FormPublicLoanBalanceInfo()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFund");
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicLoanDetailedInfo");
        }

        //窗体load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try {
                //获取查询结果
                info = (LoanBalanceInfo)Util.JumpUtil.ParamsMap["info"];

                pername.Text = info.data.pername;
                //cardtype.Text = info.data.cardtype;
                cardtype.Text = "身份证";
                cardcode.Text = info.data.cardcode;
                agrcode.Text = info.data.agrcode;
                payacc.Text = info.data.payacc;
                loanmny.Text = info.data.loanmny;
                loanbal.Text = info.data.loanbal;
                payedmths.Text = info.data.payedmths;

                overmths.Text = info.data.overmths;
                lastpaydate.Text = info.data.lastpaydate;
                totoalmny.Text = info.data.totoalmny;
                corpus.Text = info.data.corpus;
                interests.Text = info.data.interests;
                overmny.Text = info.data.overmny;
            }catch(Exception ex)
            {
                log.Write("error:个人公积金贷款余额加载异常："+ex.Message);
            }

        }
    }
}
