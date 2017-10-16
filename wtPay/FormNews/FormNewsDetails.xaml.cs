using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;
using System.Threading;
using wtPayModel.NewsModel;
using wtPayDAL.NewsDAL;

namespace wtPay.FormNews
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormNewsDetails : System.Windows.Controls.UserControl
    {
        Thread queryThread = null;

        private delegate void setWebHtmlDelegate(string value);

        private void setWebHtml(string value)
        {
            webPage.LoadHTML(value);
        }
        
        public FormNewsDetails()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webPage.Source = new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "//newLoad.html");
            Util.JumpUtil.jumpCommonPage("FormNewsList");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //webPage.Source = new Uri("http://gs.122.gov.cn/views/inquiry.html");
            webPage.Source = new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "//newLoad.html");
            queryThread = new Thread(delegate() { query(); });
            queryThread.Start();
        }

        private void query()
        {
            try
            {
                QueryNewsDetailsParam param = new QueryNewsDetailsParam();
                param.articleId = SysBLL.newsListInfoData.articleId;
                param.articlePath = SysBLL.newsListInfoData.articlePath;
                NewsDetailsInfo info= NewsAccess.QueryNewsDetailsInfo(param);
                string htmlText = info.data.content.Replace("body{", "body{ margin:20px;font-family:'微软雅黑';font-size: 20px;");
                htmlText= htmlText.Insert(htmlText.IndexOf("<p"), "<p style=\"text-align:center; color: #17242A;font-family: '微软雅黑';font-size: 24px;margin-bottom: 15px;\">" + info.data.title + "</p>"
                + "<p style = \"text-align: center;font-family: '微软雅黑';margin-top:5px;\"><span style=\"color:#960C0F;margin-right: 30px;\">http://www.lzbs.com.cn</span>"
                + "<span style=\"margin-right:70px;color: #2F4C59\"> " + info.data.createTime + " </span><span style=\"color:#2F4C59\"> 来源：" + info.data.sourceName + "</span></p><p>");
                webPage.Dispatcher.Invoke(new setWebHtmlDelegate(setWebHtml), htmlText);
            }
            catch(ThreadAbortException ae) { log.Write("error:FormNewsDetails:query:" + ae.Message); }
            catch(Exception e)
            {
                log.Write("error:FormNewsDetails:query:"+e.Message);
            }
        }

    }
}
