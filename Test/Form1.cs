using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayDAL.Pay;
using wtPayModel.PayParamModel;
using wtPayModel.PropSecModel;

namespace Test
{
    public partial class Form1 : Form
    {
        PropSecAccess access = new PropSecAccess();
        
        Thread readMoneyThread = null;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            SysBLL.IsTest = "测试";
            SysBLL.Authcode = access.login();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(1024);
            //打开端口
            int status = PropSwwyBLL.WF001(
                new StringBuilder(""),
                new StringBuilder(""),
                new StringBuilder("W000000001"),
                new StringBuilder("8"),
                new StringBuilder("01"),
                result);
            listViewAdd("打开端口：" +"状态:"+status.ToString()+"返回:"+result.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(1024);
            //打开端口
            int status = PropSwwyBLL.WF001(
                new StringBuilder(""),
                new StringBuilder(""),
                new StringBuilder("W000000001"),
                new StringBuilder("8"),
                new StringBuilder("02"),
                result);
            listViewAdd("关闭端口：" + "状态:" + status.ToString() + "返回:" + result.ToString());
        }
        private void listViewAdd(string value)
        {
            resultListView.Items.Add(value);
            resultListView.Items[resultListView.Items.Count - 1].Selected = true; ;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            PropSecQueryParam param = new PropSecQueryParam();
            param.SC10009 = "";
            param.SC10010 = "01";
            param.SC10011 = "01";
            excute("W000000001", "01", "01","08","01");

           
        }
        public void excute(string _9,string _10,string _11,string serviceType,string cardType)
        {
            PropSecQueryParam param = new PropSecQueryParam();
            param.SC10009 = _9;
            param.SC10010 = _10;
            param.SC10011 = _11;
            PropSecQueryInfo info = access.query(param);
            StringBuilder result1 = new StringBuilder(2048);
            StringBuilder result2 = new StringBuilder(2048);
            IntPtr status = PropSwwyBLL.WF002(
                new StringBuilder(serviceType),//业务类型
                new StringBuilder(cardType),//卡片种类
                new StringBuilder("01"),//卡片版本
                new StringBuilder(info.msgrsp.SC10011),//，业务步骤
                new StringBuilder(""),//卡片唯一识别号

                new StringBuilder(""),//物业公司编号
                new StringBuilder(""),//小区编号
                new StringBuilder(_9),//表具产商编号
                new StringBuilder("8"),//端口号
                result1,//返回说明
                new StringBuilder(info.msgrsp.SC20003),//业务输入信息
               result2//业务返回信息
                );
            string result = Marshal.PtrToStringAnsi(status);
            MessageBox.Show(result);

            PropSecCardJson card = new PropSec().JsonToModel(result2.ToString());
            //SC10007 = card.G_0806;
            SC10008 = card.G_1802;
            merchantNo = info.msgrsp.merchantNo;
            listViewAdd("读卡：" + "状态:" + result.ToString() + "返回说明:" + result1.ToString() + "返回信息：" + result2.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetBillType();
        }
        private void SetBillType()
        {
            StringBuilder sb = new StringBuilder();
            int result = TTCurrency.TT_SetBillType(127, sb);
            listViewAdd("接收金额：" + result + "," + sb.ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int result=TTCurrency.TT_EnableCash(60,sb);
            listViewAdd("开始投币："+result+","+sb.ToString());

            Thread.Sleep(2000);
            readMoneyThread = new Thread(delegate() { readMoney(); });
            readMoneyThread.Start();
        }
        private void readMoney()
        {
            StringBuilder sb = new StringBuilder();
            int money = 0;
            int countMoney = 0;
            while (true)
            {
                money=TTCurrency.TT_GetMoney(sb);
                if (money <= 0) continue;
                listViewAdd(money+"元");
                countMoney += money;
                Thread.Sleep(100); //延时100毫秒
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
           int result= TTCurrency.TT_DisableCash(sb);
            listViewAdd("禁止投币："+result.ToString()+","+sb.ToString());
        }
        //string SC10007 = "";
        string SC10008 = "";
        string merchantNo = "";
        private void button5_Click_1(object sender, EventArgs e)
        {
            PropSecOrderParam param = new PropSecOrderParam();
            param.shopType = "1";
            param.AMOUNT = "12";
            param.paymentAmout = "0";
            param.SC10009 = "W000000001";
            param.SC10010 = "01";
            param.SC10007 = "XQ00000221";
            param.SC10008 = SC10008;
            param.merchantNo = merchantNo;
            PropSecOrderInfo info=access.order(param);

            PayAccess payAccess = new PayAccess();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("serviceType", "5_2");
            parameters.Add("realAmout", info.msgrsp.realAmout);
            parameters.Add("tr.shop_type", "1");
            parameters.Add("tr.cloud_no", info.msgrsp.orderNo);
            parameters.Add("terminalNo", SysConfigHelper.readerNode("ClientNo"));
            parameters.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
            PayResultInfo payinfo=payAccess.PayResNewAcc(parameters);

            StringBuilder result1 = new StringBuilder(2048);
            StringBuilder result2 = new StringBuilder(2048);
            IntPtr status = PropSwwyBLL.WF002(
                new StringBuilder("02"),//业务类型
                new StringBuilder("01"),//卡片种类
                new StringBuilder("01"),//卡片版本
                new StringBuilder(""),//，业务步骤
                new StringBuilder(""),//卡片唯一识别号
                new StringBuilder(""),//物业公司编号
                new StringBuilder(""),//小区编号
                new StringBuilder("W000000001"),//表具产商编号
                new StringBuilder("8"),//端口号
                result1,//返回说明
                new StringBuilder(payinfo.SC20003.ToString()),//业务输入信息
               result2//业务返回信息
                );
            string result = Marshal.PtrToStringAnsi(status);
            MessageBox.Show(result);
            PropSecCardJson card = new PropSec().JsonToModel(result2.ToString());
            //SC10007 = card.G_0806;
            SC10008 = card.G_1802;
            listViewAdd("写卡：" + "状态:" + result.ToString() + "返回说明:" + result1.ToString() + "返回信息：" + result2.ToString());

            //IntPtr intPtr = ReturnString();
            //string str = Marshal.PtrToStringAnsi(intPtr);
        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    byte[] b = BCDUtil.HexStrToByteArray(textBox1.Text);
        //    AnalysisBaseLKL ab = new AnalysisBaseLKLSign();
        //    Dictionary<string,ResultData> dic =   ab.analysis(b);
        //    foreach(string key in dic.Keys)
        //    {
        //        textBox2.AppendText(key + ":" + dic[key].name + ":" + dic[key].value+"\n");
        //    }
        //}
    }
}
