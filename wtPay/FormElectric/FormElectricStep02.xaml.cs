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
using System.Windows.Threading;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel;
using wtPayModel.ElecModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormElectric
{
    /// <summary>
    /// FormElectricStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormElectricStep02 : UserControl
    {
        Thread queryThread = null;
        List<ElecQueryDianFeiDetail> list = null;
        DispatcherTimer timerLoad = null;

        Thread rollingThread = null;

        private delegate void isShowLabelDelegate(Label label, bool isShow);
        private delegate void isShowTextBlockDelegate(TextBlock textBlock,bool isShow);
        private delegate void isShowButtonDelegate(Button button,bool isShow);
        private delegate void isShowGridDelegate(Grid grid,bool isShow);

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";
        string custNo3 = "";

        string address = "";
        string yonhumingcheng = "";
        string jfmx = "";
        public FormElectricStep02()
        {
            InitializeComponent();
        }

        private void isShowTextBlock(TextBlock textBlock, bool isShow)
        {
            if (isShow)
            {
                textBlock.Visibility = Visibility.Visible;
            }
            else
            {
                textBlock.Visibility = Visibility.Hidden;
            }
        }

        private void isShowButton(Button button,bool isShow)
        {
            if (isShow)
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {
                button.Visibility = Visibility.Hidden;
            }
        }
        private void isShowGrid(Grid grid,bool isShow)
        {
            if (isShow)
            {
                grid.Visibility = Visibility.Visible;
            }
            else
            {
                grid.Visibility = Visibility.Hidden;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormElectricStep01");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Payment.elecPayParam != null)
                {
                    if (Payment.elecPayParam.IsArrearage == true)
                    {
                        //预缴费
                        if (PayStaticParam.isHaveRMB == true)
                        {
                            //识币器
                            Util.JumpUtil.jumpCommonPage("FormPayType");
                            return;
                        }
                        else
                        {
                            Util.JumpUtil.jumpCommonPage("FormElectricStep03");
                            return;
                        }
                    }
                    else
                    {
                        //Payment.elecPayParam.Param.jfmx = jfmx;
                        //new FormJump().openForm(sender, e, this, new FormElectricStep04(payParam));
                        //Util.JumpUtil.jumpCommonPage("FormElectricStep04");
                    }
                }
                //预缴费
                if (PayStaticParam.isHaveRMB == true)
                {
                    //识币器
                    Util.JumpUtil.jumpCommonPage("FormPayType");
                    return;
                }
                else
                {
                    Util.JumpUtil.jumpCommonPage("FormElectricStep03");
                    return;
                }
                return;
            }catch(Exception ex)
            {
                log.Write("error:FormElectricStep02:Button_Click_1:"+ex.Message);
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
                loadlbl.Visibility = Visibility.Visible;
                pagelbl.Visibility = Visibility.Hidden;
                Payment.elecPayParam.IsArrearage = false;
                yingcang();
                SysBLL.Player("正在查询.wav");
                lblAccountInfo.Text = "正在查询...";
                clear();
                queryThread = new Thread(delegate() { query(); });
                queryThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormElectricStep02:load():"+ex.Message);
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (loadlbl.Content.ToString().Length >= 6)
            {
                loadlbl.Content = "";
            }
            else
            {
                loadlbl.Content += ".";
            }
        }

        private void isShowLabel(Label label, bool isShow)
        {
            if (isShow)
            {
                label.Visibility = Visibility.Visible;
            }
            else
            {
                label.Visibility = Visibility.Hidden;
            }
        }
        private void setTextBlockText(TextBlock textBlock,string value)
        {
            textBlock.Text = value;
        }
        private void yingcang()
        {
            //panel6.Visible = false;
            button4.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), button4,false);
            button3.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), button3, false);
            btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), btnRecharg,false);
            panel4.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel4,false);
            //lblUserName.Visible=false;
            lblUserName1.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblUserName1,false);
            lblDatee.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblDatee, false);
            lblMoney.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblMoney, false);
            blancelbl.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), blancelbl, false);
        }
        private void xianshi()
        {
            button4.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), button4, true);
            button3.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), button3, true);
            //lblUserName.Visibility =  Visibility.Visible;
            lblUserName1.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblUserName1, true);
            lblDatee.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblDatee, true);
            lblMoney.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblMoney, true);
            blancelbl.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), blancelbl, true);
        }

        public void query()
        {
            try
            {
                ElecQueryElecInfo info = ElecAccess.QueryElec(Payment.elecPayParam.Account);
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, false); 
                try
                {
                    //ElecQueryUserInfo userInfo = ElecAccess.QueryUser(Payment.elecPayParam.Account);
                    //address = userInfo.msgrsp.userInfoList[0].yddz;
                    address = info.msgrsp.yddz;
                    addresslbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),this.addresslbl, info.msgrsp.yddz);
                    if (address.Length > 10)
                    {
                        // timer1.Enabled = true; 
                    }
                }
                catch (Exception ex) { }
                

                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    //欠费
                    if ("0001".Equals(info.msgrsp.retcode))
                    {
                        btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), btnRecharg,true);
                        //提示
                        lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, "查询无欠费，可进行预收");
                        //电费账号
                        lblBalance.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblBalance, Payment.elecPayParam.Account);
                        yonhumingchengLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), yonhumingchengLbl, info.msgrsp.yhmc);
                        if (info.msgrsp.ysje != null)
                        {
                            if (info.msgrsp.ysje.Length > 0)
                                lblAmount.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAmount, info.msgrsp.ysje);
                            else
                                lblAmount.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAmount, "0.0");
                        }
                        //if (info.msgrsp.qfje != null)
                        //{
                        //    //欠费金额
                        //    if (info.msgrsp.qfje.Length > 0)
                        //        qfjeLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), qfjeLbl, info.msgrsp.qfje);
                        //    else
                        //        qfjeLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), qfjeLbl, "0.0");
                        //}
                        Payment.elecPayParam.IsArrearage = true;
                        //Payment.elecPayParam.Account = Payment.elecPayParam.Account;
                        //btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),btnRecharg,true);
                        panel4.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel4,true);
                        try
                        {
                            rollingThread = new Thread(delegate() { rolling(); });
                            rollingThread.Start();
                        }catch(ThreadAbortException ae) { log.Write("error:"+ae.Message); }
                        catch(Exception ex) { log.Write("error:电力查询信息滚动异常："+ex.Message); }
                        SysBLL.Player("继续缴费请点击确定按钮.wav");
                        return;
                    }
                    else if (!"0000".Equals(info.msgrsp.retcode))
                    {
                        loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel),this.loadlbl,false);
                        //激活按钮
                        btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),this.btnRecharg,false);
                        if (info.msgrsp.retshow.Length > 0)
                        {
                            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, info.msgrsp.retshow);
                            return;
                        }
                        else
                        {
                            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, "查询失败，请稍后再试");
                            return;
                        }
                    }
                    else if (info.msgrsp.retshow != null)
                    {
                        if (info.msgrsp.retshow == null) info.msgrsp.retshow = "";
                        string retshow = SysBLL.ClearBlank(info.msgrsp.retshow);
                        if (retshow.Length <= 15)
                        {
                            //提示
                            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, retshow);
                            loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), this.loadlbl, false);
                            return;
                        }
                    }
                    else
                    {
                        PrintInfo(WtException.formatMsg(WtExceptionCode.Bus.BUS_QUERY));
                    }

                }
                if (info.msgrsp.dfze != null)
                {
                    btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), btnRecharg, false);
                    //提示
                    lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, "您的账号信息");
                    loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), this.loadlbl, false);
                    ////电费账号
                    //lblBalance.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText)Text = account;
                    yonhumingcheng = info.msgrsp.yhmc;
                    ////余额
                    //if (info.msgrsp.ysje.Length > 0)
                    //    lblAmount.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText)Text = info.msgrsp.ysje;
                    //else
                    //    lblAmount.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText)Text = "0.0";
                    ////欠费金额
                    //if (info.msgrsp.qfje.Length > 0)
                    //    qfjeLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText)Text = info.msgrsp.qfje;
                    //else
                    //    qfjeLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText)Text = "0.0";
                    panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid),panel2,true);
                    //btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),btnRecharg,true);
                    //Payment.elecPayParam.UserInputAmount = info.msgrsp.qfje;
                    //Payment.elecPayParam.Account = Payment.elecPayParam.Account;
                    //获取支付参数
                    Payment.elecPayParam.Param = new ElecPayresParam();
                    Payment.elecPayParam.Param.dzpc = info.msgrsp.dzpc;
                    Payment.elecPayParam.Param.yhbh = info.msgrsp.yhbh;
                    Payment.elecPayParam.Param.ysje = info.msgrsp.ysje;
                    Payment.elecPayParam.Param.jfbs = info.msgrsp.dfbs;
                    list = info.msgrsp.dianFeiDetail;
                    if (list.Count > 0)
                    {
                        string str = "";
                        for (int i = 0; i < list.Count; i++)
                        {
                            str = list[i].yhbh + "-" + list[i].ysbz + "-" + list[i].bbyjje + "-" + list[i].dfje + "-" + list[i].wyjje + "-" + list[i].sctw + "-" + list[i].bctw + "|";
                            jfmx = jfmx + str;
                        }
                        jfmx = jfmx.Remove(jfmx.Length - 1, 1);
                    }
                    pushlist();
                    pagelbl.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock),pagelbl,true);
                    //激活按钮
                    //btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),btnRecharg,true);
                    btnRecharg.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), btnRecharg, false);
                    SysBLL.Player("继续缴费请点击充值按钮.wav");
                    return;
                }
            }
            catch (ThreadAbortException ae) { log.Write("error:FormElectricStep02:query():"+ae.Message); }
            catch (WtException ex)
            {
                if (ex.getCode() == WtExceptionCode.Sys.NETWORK)
                {
                    PrintInfo(WtException.formatMsg(WtExceptionCode.Bus.BUS_QUERY, ex.Message));
                }
                else
                {
                    PrintInfo(ex);
                }


            }
            catch (Exception e)
            {

                PrintInfo(WtException.formatMsg(WtExceptionCode.Bus.BUS_QUERY, e.Message));

            }

        }
        string addressTemp = "";
        private void rolling()
        {
            try
            {
                Thread.Sleep(1000*2);
                addressTemp = address;
                while (true)
                {
                    Thread.Sleep(400);
                    if (addressTemp.Length == 2)
                    {
                        addressTemp = address;
                    }
                    addresslbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), addresslbl, addressTemp);
                    addressTemp = addressTemp.Remove(0, 1);
                }
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex) { log.Write("error:电力查询信息滚动异常："+ex.Message); }
        }
        private void pushlist()
        {
            if (list.Count > 0)
            {
                xianshi();
                lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo,"您的账单信息");
                pageCount = list.Count / 3;
                if ((list.Count % 3) != 0)
                {
                    pageCount++;
                }
                setPage(page);
                button4.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),button4,true);
                button3.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),button3,true);
            }
        }

        int i1 = 0, i2 = 0, i3 = 0;
        //设置页面
        private void setPage(int page)
        {
            pagelbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), pagelbl, "第" + (page + 1) + "页");
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
        }
        private void clear()
        {
            panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, false);
            panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, false);
            panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, false);
        }
        string ysztStr = "";
        //行一
        private void setRow1(ElecQueryDianFeiDetail info)
        {
            //custNo1 = info.;
            username1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), username1,yonhumingcheng);
            dfny1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), dfny1, info.dfny);
            bbyjje1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), bbyjje1, info.bbyjje);
            ysztStr = info.yszt;
            if ("01".Equals(ysztStr))
            {
                ysztStr = "欠费";
            }
            if ("02".Equals(ysztStr))
            {
                ysztStr = "部分结清";
            }
            yszt1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), yszt1, ysztStr);
            panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, true);
        }
        private void setRow2(ElecQueryDianFeiDetail info)
        {
            //custNo2 = info.custNo;
            username2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), username2, yonhumingcheng); 
            dfny2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),dfny2, info.dfny);
            bbyjje2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), bbyjje2, info.bbyjje);
            ysztStr = info.yszt;
            if ("01".Equals(ysztStr))
            {
                ysztStr = "欠费";
            }
            if ("02".Equals(ysztStr))
            {
                ysztStr = "部分结清";
            }
            yszt2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), yszt2, ysztStr);
            panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, true);
        }
        private void setRow3(ElecQueryDianFeiDetail info)
        {
            //custNo3 = info.custNo;
            username3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), username3, yonhumingcheng);
            dfny3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), dfny3, info.dfny);
            bbyjje3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),bbyjje3, info.bbyjje);
            ysztStr = info.yszt;
            if ("01".Equals(ysztStr))
            {
                ysztStr = "欠费";
            }
            if ("02".Equals(ysztStr))
            {
                ysztStr = "部分结清";
            }
            yszt3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),yszt3, ysztStr);
            panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, true);
        }
        private void PrintInfo(WtException e)
        {
            PrintInfo(e.getMsg());
        }
        private void PrintInfo(string msg)
        {
            //读取万通卡失败
            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, msg);
            loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), this.loadlbl, false);
        }


        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page = page - 1;
                setPage(page);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            int count = page;
            count++;
            if (count < pageCount)
            {
                page = page + 1;
                setPage(page);
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (list.Count > 0)
            {
                int i = i1;
                //Payment.elecPayParam.Param.jfmx = list[i].yhbh + "-" + list[i].ysbz + "-" + list[i].bbyjje + "-" + list[i].dfje + "-" + list[i].wyjje + "-" + list[i].sctw + "-" + list[i].bctw;
                Payment.elecPayParam.ElecQueryDianFeiDetail = list[i];
                Payment.elecPayParam.UserInputMoney = list[i].bbyjje;
            }
            Util.JumpUtil.jumpCommonPage("FormElectricStep03");
            ////预缴费
            //if (PayStaticParam.isHaveRMB == true)
            //{
            //    //识币器
            //    Util.JumpUtil.jumpCommonPage("FormPayType");
            //    return;
            //}
            //else
            //{
            //    Util.JumpUtil.jumpCommonPage("FormElectricStep03");
            //    return;
            //}
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;

                queryThread.Abort();
                queryThread.DisableComObjectEagerCleanup();
                queryThread = null;

                rollingThread.Abort();
                rollingThread.DisableComObjectEagerCleanup();
                rollingThread = null;
                
            }
            catch(Exception ex) {
                log.Write("error:FormElectricStep02:UserControl_Unloaded:"+ex.Message);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (list.Count > 0)
            {
                int i = i2;
                //Payment.elecPayParam.Param.jfmx = list[i].yhbh + "-" + list[i].ysbz + "-" + list[i].bbyjje + "-" + list[i].dfje + "-" + list[i].wyjje + "-" + list[i].sctw + "-" + list[i].bctw;
                Payment.elecPayParam.ElecQueryDianFeiDetail = list[i];
                Payment.elecPayParam.UserInputMoney = list[i].bbyjje;
            }
            Util.JumpUtil.jumpCommonPage("FormElectricStep03");
            ////预缴费
            //if (PayStaticParam.isHaveRMB == true)
            //{
            //    //识币器
            //    Util.JumpUtil.jumpCommonPage("FormPayType");
            //    return;
            //}
            //else
            //{
            //    Util.JumpUtil.jumpCommonPage("FormElectricStep03");
            //    return;
            //}
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (list.Count > 0)
            {
                int i = i3;
                //Payment.elecPayParam.Param.jfmx = list[i].yhbh + "-" + list[i].ysbz + "-" + list[i].bbyjje + "-" + list[i].dfje + "-" + list[i].wyjje + "-" + list[i].sctw + "-" + list[i].bctw;
                Payment.elecPayParam.ElecQueryDianFeiDetail = list[i];
                Payment.elecPayParam.UserInputMoney = list[i].bbyjje;
            }
            Util.JumpUtil.jumpCommonPage("FormElectricStep03");
            ////预缴费
            //if (PayStaticParam.isHaveRMB == true)
            //{
            //    //识币器
            //    Util.JumpUtil.jumpCommonPage("FormPayType");
            //    return;
            //}
            //else
            //{
            //    Util.JumpUtil.jumpCommonPage("FormElectricStep03");
            //    return;
            //}
        }
    }
}
