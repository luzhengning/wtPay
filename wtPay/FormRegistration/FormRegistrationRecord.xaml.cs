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
using wtPayDAL;
using wtPayModel.ExpressModel;
using wtPayModel.PublicFundModel;
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormPublicFundCustomerInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistrationRecord : UserControl
    {
        private string queryInfo = "查询失败，请稍后再试...";
        private string loadInfo = "正在查询，请稍后...";

        private delegate void setTextBlockTextDelegate(TextBlock tb,string value);
        private delegate void setTextBlockVisibilityDelegate(TextBlock tb,bool isShow);
        private delegate void getTextBlockDelegate(FormRegistrationRecord obj, string name);

        TextBlock timeTextBlock = null;
        TextBlock addressTextBlock = null;
        TextBlock cardIdBlock = null;

        //查询线程
        Thread queryThread = null;
        public FormRegistrationRecord()
        {
            InitializeComponent();
        }
        private void setTextBlockText(TextBlock tb, string value)
        {
            tb.Text = value;
        }
        private void setTextBlockVisibility(TextBlock tb, bool isShow)
        {
            if (isShow)
            {
                tb.Visibility = Visibility.Visible;
            }
            else
            {
                tb.Visibility = Visibility.Hidden;
            }
        }
        private void getTextBlock(FormRegistrationRecord obj, string name)
        {
            object o = obj.FindName(name);
            if(name.Contains("name"))
                timeTextBlock= (TextBlock)(o);
            if (name.Contains("time"))
                addressTextBlock = (TextBlock)(o);
            if (name.Contains("cardId"))
                cardIdBlock = (TextBlock)(o);
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistration");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
           
            try {
                setShowinfo(loadInfo);
                showinfo.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), showinfo, true);
                timeTitle.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), timeTitle, false);
                nameTitle.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), nameTitle, false);
                cardTitle.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), cardTitle, false);
                for (int i = 2; i <= 17; i++)
                {
                    ((TextBlock)(this.FindName("name" + i))).Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), ((TextBlock)(this.FindName("name" + i))), false);
                    ((TextBlock)(this.FindName("time" + i))).Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), ((TextBlock)(this.FindName("time" + i))), false);
                    ((TextBlock)(this.FindName("cardId" + i))).Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), ((TextBlock)(this.FindName("cardId" + i))), false);
                }
                queryThread = new Thread(delegate() { query(); });
                queryThread.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormPublicFundCustomerInfo：load():" + ex.Message);
            }
        }

        private void setShowinfo(string info)
        {
            showinfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),showinfo, info);
        }
        private void setTextBlockTextRow(string timeName,string addressName,string cardIdname,string time,string address,string cardId)
        {
            log.Write("测试："+cardIdname+":"+cardId);
            this.Dispatcher.Invoke(new getTextBlockDelegate(getTextBlock), this, timeName);
            this.Dispatcher.Invoke(new getTextBlockDelegate(getTextBlock), this, addressName);
            this.Dispatcher.Invoke(new getTextBlockDelegate(getTextBlock), this, cardIdname);
            timeTextBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), timeTextBlock, time);
            addressTextBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), addressTextBlock, address);
            cardIdBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), cardIdBlock, cardId);

            timeTextBlock.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), timeTextBlock, true);
            addressTextBlock.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), addressTextBlock, true);
            cardIdBlock.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), cardIdBlock, true);
        }
        private void query()
        {
            try
            {
                zhongtieQuery();
            }
            catch(ThreadAbortException ae) { log.Write("error:FormRegistrationRecord:query():" + ae.Message); setShowinfo(queryInfo); }
            catch (Exception ex)
            {
                setShowinfo(queryInfo);
                log.Write("error:FormRegistrationRecord:query():" + ex.Message);
            }
        }
        private void zhongtieQuery()
        {
            RegistrationRecordQueryInfo info = RegistrationAccess.RegistrationRecordQuery(RegistrationClass.registrationRecordQueryParam);
            if (info.record==null)
            {
                setShowinfo(info.Error_Msg);
                return;
            }
            int j = 0;
            for (int i = 2; i <= 17; i++)
            {
                if (info.record.Count==j) break;
                setTextBlockTextRow("name" + i, "time" + i, "cardId"+i, info.record[j].patient_name, info.record[j].hb_date+"     "+ info.record[j].flow_no,info.record[j].id_no);
                j++;
            }
            if (j > 0)
            {
                showinfo.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), showinfo, false);
                timeTitle.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), timeTitle, true);
                nameTitle.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), nameTitle, true);
                cardTitle.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), cardTitle, true);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (queryThread.IsAlive)
                {
                    queryThread.Abort();
                    queryThread.DisableComObjectEagerCleanup();
                    queryThread = null;
                }
            }catch(Exception ex)
            {
                log.Write("error:FormExpressResult:UserControl_Unloaded:" + ex.Message);
            }
        }
    }
}
