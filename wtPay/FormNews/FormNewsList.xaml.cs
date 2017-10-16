﻿using System;
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
using wtPay.FormExpress;
using wtPayBLL;
using wtPayDAL;
using wtPayDAL.NewsDAL;
using wtPayModel.ExpressModel;
using wtPayModel.NewsModel;
using wtPayModel.PublicFundModel;

namespace wtPay.FormNews
{
    /// <summary>
    /// FormPublicFundCustomerInfo.xaml 的交互逻辑
    /// </summary>
    public partial class FormNewsList : UserControl
    {
        private string queryInfo = "未查询到记录，请稍后再试...";
        private string loadInfo = "正在查询，请稍后...";

        private Dictionary<int, string[]> resultInfo = new Dictionary<int, string[]>(); 

        private delegate void setTextBlockTextDelegate(TextBlock tb,string value);
        private delegate void setTextBlockVisibilityDelegate(TextBlock tb,bool isShow);
        private delegate void getTextBlockDelegate(FormNewsList obj, string name);
        private delegate void setPanelShowDelegate(FormNewsList result);

        TextBlock timeTextBlock = null;
        TextBlock addressTextBlock = null;

        NewsListInfo newsListInfo=null;

        int pageNo = 0;

        //查询线程
        Thread queryThread = null;
        public FormNewsList()
        {
            InitializeComponent();
        }
        private void setPanelShow(FormNewsList result)
        {
            for (int i = 2; i <= 12; i++)
            {
                ((TextBlock)(result.FindName("time" + i))).Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), ((TextBlock)(this.FindName("time" + i))), false);
                ((TextBlock)(result.FindName("address" + i))).Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), ((TextBlock)(this.FindName("address" + i))), false);
                ((TextBlock)(result.FindName("time" + i))).Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), ((TextBlock)(this.FindName("time" + i))), "");
                ((TextBlock)(result.FindName("address" + i))).Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), ((TextBlock)(this.FindName("address" + i))), "");
            }
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
        private void getTextBlock(FormNewsList obj, string name)
        {
            object o = obj.FindName(name);
            if(name.Contains("time"))
                timeTextBlock= (TextBlock)(o);
            if (name.Contains("address"))
                addressTextBlock = (TextBlock)(o);
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
           
            try {
                pageNo = 0;
                resultInfo.Clear();
                setShowinfo(loadInfo);
                showinfo.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), showinfo, true);
                hideTitle();
                clear();
                queryThread = new Thread(delegate() { query(); });
                queryThread.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormPublicFundCustomerInfo：load():" + ex.Message);
            }
        }
        private void clear()
        {
            for (int i = 2; i <= 12; i++)
            {
                ((TextBlock)(this.FindName("time" + i))).Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), ((TextBlock)(this.FindName("time" + i))),false);
                ((TextBlock)(this.FindName("address" + i))).Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), ((TextBlock)(this.FindName("address" + i))), false);
                ((TextBlock)(this.FindName("time" + i))).Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), ((TextBlock)(this.FindName("time" + i))),"");
                ((TextBlock)(this.FindName("address" + i))).Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), ((TextBlock)(this.FindName("address" + i))), "");
            }
        }
        private void setShowinfo(string info)
        {
            showinfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),showinfo, info);
        }
        private void setTextBlockTextRow(string timeName,string addressName,string time,string address)
        {
            this.Dispatcher.Invoke(new getTextBlockDelegate(getTextBlock), this, timeName);
            this.Dispatcher.Invoke(new getTextBlockDelegate(getTextBlock), this, addressName);  
            timeTextBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), timeTextBlock, time);
            addressTextBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), addressTextBlock, address+"【点击查看】");
            timeTextBlock.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), timeTextBlock, true);
            addressTextBlock.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), addressTextBlock, true);
        }
        /// <summary>
        /// 快递查询
        /// </summary>
        private void query()
        {
            try
            {
                Query();
                //showinfo.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility),showinfo,true);
            }
            catch(ThreadAbortException ae) {
                log.Write("error:FormExpressResult:query():" + ae.Message); setShowinfo(queryInfo);
                time1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), time1, false);
                //address1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), address1, false);
            }
            catch (Exception ex)
            {
                setShowinfo(queryInfo);
                log.Write("error:FormExpressResult:query():" + ex.Message);
                time1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), time1, false);
                //address1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), address1, false);
            }
        }
        private void hideTitle()
        {
            time1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), time1, false);
            //address1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), address1, false);
        }
        private void showTitle()
        {
            time1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), time1, true);
            //address1.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), address1, true);
        }
        /// <summary>
        /// 查询
        /// </summary>
        private void Query()
        {
            QueryNewsParam param = new QueryNewsParam();
            param.conName = "政务信息列表查询";
            newsListInfo = null;
            newsListInfo = NewsAccess.QueryNewsLists(param);
            if ("9999".Equals(newsListInfo.success))
            {
                setShowinfo(newsListInfo.msg);
                hideTitle();
                return;
            }
            int j = newsListInfo.data.Count;
            j--;
            int count = 0;
            for(int i = j; i >= 0; i--)
            {
                resultInfo.Add(count,new string[] { newsListInfo.data[i].pubTime, newsListInfo.data[i].title });
                count++;
            }
            setPage(pageNo);

            if (newsListInfo.data.Count == 0)
            {
                hideTitle();
                throw new Exception();
            }
            else
            {
                showTitle();
                showinfo.Dispatcher.Invoke(new setTextBlockVisibilityDelegate(setTextBlockVisibility), showinfo, false);
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

        //上一页
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pageNo == 0) return;
            pageNo--;
            setPage(pageNo);
        }
        //下一页
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pageNo++;
            setPage(pageNo);
        }

        private void setPage(int page)
        {
            try
            {
                this.Dispatcher.Invoke(new setPanelShowDelegate(setPanelShow), this);
                int count = 2;
                int countpage = ((page * 11) + 12);
                int i = 0;
                for (i = (page * 11); i <= countpage; i++)
                {
                    if (!resultInfo.ContainsKey(i))
                    {
                        if (count == 2)
                        {
                            if (pageNo < 0) { pageNo = 0; return; }
                            pageNo--;
                            setPage(pageNo);
                        }
                        return;
                    }
                    setTextBlockTextRow("time" + count, "address" + count, resultInfo[i][0], resultInfo[i][1] );
                    count++;
                }
            }catch(Exception ex)
            {
                log.Write("error:FormExpressResult:setPage(int page):" + ex.Message);
            }
            
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock txt = (TextBlock)sender;
                foreach (NewsListInfoData data in newsListInfo.data)
                {
                    if (txt.Text.Contains(data.title))
                    {
                        SysBLL.newsListInfoData = data;
                        Util.JumpUtil.jumpCommonPage("FormNewsDetails");
                        return;
                    }
                }
            }catch(Exception ex)
            {
                log.Write("error:FormNewsList:Grid_MouseLeftButtonDown："+ex.Message);
            }
        }
    }
}
