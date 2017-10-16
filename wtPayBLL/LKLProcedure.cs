
using AnalysisBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using wtPayCommon;
using wtPayModel;
using wtPayModel.PayParamModel;
using wtPayModel.WintopModel;

namespace wtPayBLL
{
    public class LKLProcedure
    {
        static string LakalaIp = SysConfigHelper.readerNode("lklPayIp");//127.0.0.1 测试：124.74.143.162
        static int LakalaPort = Convert.ToInt32(SysConfigHelper.readerNode("lklPort"));//8885
        public static string mackey = "7FE0913BE0914338";
        public static string pinKey = "D5CD1F19A186342CD5B015D0C75D26C8";
        /// <summary>
        /// 客户端
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        public static byte[] Client(string ip, int port, byte[] messages)
        {
            string sfd = BCDUtil.byteArrToString(messages);
            byte[] datas;
            try
            {
                //1.发送数据                
                TcpClient client = new TcpClient(ip, port);
                IPEndPoint ipendpoint = client.Client.RemoteEndPoint as IPEndPoint;
                NetworkStream stream = client.GetStream();
                stream.Write(messages, 0, messages.Length);


                //2.接收状态,长度<1024字节
                BCDUtil.printData("发送数据", messages);
                //读取超时时间，10秒
                stream.ReadTimeout = 60000;
                datas = readData(stream);
                //3.关闭对象
                stream.Close();
                client.Close();
                return datas;
            }
            catch (Exception ex)
            {
                throw new WtException(WtExceptionCode.Sys.NETWORK, ex.Message);
            }
        }

        public static byte[] readData(NetworkStream stream)
        {
            byte[] temp = new byte[8 * 1024];
            byte[] len = new byte[2];
            int length = stream.Read(temp, 0, temp.Length);
            long size = Convert.ToInt64(PosProtocol.bytesToHexString(PosProtocol.subbyte(temp, 0, 2)), 16);
            byte[] result = new byte[size + 2];
            while (size > length)
            {
                if (stream.CanRead)
                    length += stream.Read(temp, 2, Convert.ToInt32(size));
            }
            Array.Copy(temp, result, length);
            return result;
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public static SignParam sign()
        {
            try
            {
                AnalysisBaseLKLSign lklSign = new AnalysisBaseLKLSign();
                //签到
                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(3, "910000");
                data.Add(11, "000001");
                data.Add(25, "00");
                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(57, "LUZHENGNING A8V2.1.1.5912.01  6176 8637727");
                data.Add(60, "010000");
                data.Add(61, "100000011");
                SignParam param = new SignParam();
                param.data = BCDUtil.byteArrToString(EncryptLakala.EncryptData("0820", "6009070000", EncryptLakala.encryptLKL(data, null)));
                param.shop_type = "0";
                return param;
            }
            catch (WtException e)
            {
                throw e;
            }
            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Sys.LKL_SIGN, e.Message);
            }

        }
        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="mackey"></param>
        /// <returns></returns>

        /// <summary>
        /// 计算mac 公共类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static byte[] calculateMac(Dictionary<int, string> data, byte[] countMacData)
        {

            //  Console.WriteLine(BCDUtil.byteArrToString(countMacData));
            string str = MacUtils.DESMAC(mackey, BCDUtil.byteArrToString(countMacData));
            //Console.WriteLine("bcd2Str:" + PosProtocol.bcd2Str(System.Text.Encoding.Default.GetBytes(str.Substring(0, 8))));
            //MAC转byte[]
            String strMac = PosProtocol.bcd2Str(System.Text.Encoding.Default.GetBytes(str.Substring(0, 8)));
            byte[] macByte = BCDUtil.str2Bcd(BCDUtil.leftpad(Convert.ToString(Convert.ToInt64(strMac), 10), 16));
            return macByte;
        }
        /// <summary>
        /// 获取8583报文格式的时间
        /// </summary>
        /// <returns></returns>
        static string getMMDDHHMMSSTime()
        {
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return getMMDD() + getHHMMSS();
        }
        /// <summary>
        /// 获取当前月日
        /// </summary>
        /// <returns></returns>
        static string getMMDD()
        {
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            return month + day;
        }
        /// <summary>
        /// 获取当前时分秒
        /// </summary>
        /// <returns></returns>
        public static string getHHMMSS()
        {

            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return hour + minute + second;
        }
        /// <summary>
        /// 时间处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string TimeStrLenZero(string str)
        {
            int length = str.Length;
            if (length != 2)
            {
                str = "0" + str;
            }
            return str;
        }
        /// <summary>
        /// 签退
        /// </summary>
        /// <returns></returns>
        public static byte[] signOut()
        {

            try
            {
                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(3, "910000");
                data.Add(7, getMMDDHHMMSSTime());
                data.Add(11, "000005");
                data.Add(25, "00");
                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(60, "010000");
                data.Add(62, "003690000004");
                data.Add(63, "003690000004");
                return Client(LakalaIp, LakalaPort, EncryptLakala.EncryptData("0820", "6009070000", EncryptLakala.encryptLKL(data, calculateMac(data, EncryptLakala.countMacData("0820", EncryptLakala.encryptLKL(data, null)))))); //发送数据

            }
            catch (WtException e)
            {
                throw e;
            }
            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Sys.LKL_SIGN_OUT, e.Message);
            }


        }
        /// <summary>
        /// 换算PIN
        /// </summary>
        /// <param name="pwd">用户输入的密码</param>
        /// <param name="cardNo">获取的ic卡卡号</param>
        /// <returns></returns>
        public static string getPin(string pwd, string cardNo)
        {
            string result = string.Empty;
            int len = cardNo.Length - 4;
            string pwdLen = pwd.Length + "";
            if (pwdLen.Length == 1)
            {
                pwd = "0" + pwdLen + pwd;
            }
            string cardNoCover = "";

            if (len < 12)
            {
                cardNo = cardNo.Substring(4, len);
                for (int i = 0; i < 12 - len; i++)
                {
                    cardNoCover += "f";
                }
            }
            else
            {
                cardNo = cardNo.Substring(3, 12);
            }
            cardNo = cardNoCover + cardNo;
            for (int i = 0; i < 16; i++)
            {
                if (cardNo.Length < 16)
                {
                    cardNo = "0" + cardNo;
                }
                if (pwd.Length < 16)
                {
                    pwd = pwd + "f";
                }
            }

            string orx = MacUtils.xOrString(cardNo, pwd);
            //Console.WriteLine("orx="+ orx);
            result = _3DES.getDes3EncryptedText(pinKey, orx);
            // Console.WriteLine("result="+ result);
            return result;
        }
        /// <summary>
        /// 打印数据到控制的 同时重要功能：如果返回80 更新pinKey 与 macKey
        /// </summary>
        /// <param name="data">传入服务器返回byte数组</param>
        /// <returns>返回信息为系统返回信息</returns>
        public static string handleDataMac(byte[] data)
        {
            if (data == null || data.Length <= 0)
            {
                return "无返回";
            }
            AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            Dictionary<string, ResultData> rd = lk.analysis(data);
            if (rd.ContainsKey("44"))
            {
                ResultData _44 = rd["44"];
                //获取44域中的密文密钥
                string mackeyExpress = _44.value.Substring(_44.value.Length - 16, 16);
                //将密文密钥解析明文密钥
                mackey = DESHelper.Decrypt_DES16(mackeyExpress, "1010101010101010");
                Console.WriteLine("mackey=" + mackey);
                pinKey = _44.value.Substring(0, 32);
                Console.WriteLine("加密前pinKey:" + pinKey);
                pinKey = DESHelper.Decrypt_DES16(pinKey.Substring(0, 16), "1010101010101010") + DESHelper.Decrypt_DES16(pinKey.Substring(16, 16), "1010101010101010");
                Console.WriteLine("加密后pinKey:" + pinKey);
            }
            foreach (var item in rd)
            {
                Console.WriteLine(item.Value.name + ":::::" + item.Value.value);
            }
            return rd["39"].value;
        }
        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="msgNo"></param>
        /// <returns></returns>
        static string handleDataMsg(string msgNo)
        {
            switch (msgNo)
            {
                case "00":
                    return "交易成功";
                case "01":
                    return "查询发卡方";
                case "02":
                    return "CALL BANK 查询";
                case "03":
                    return "无效商户";
                case "05":
                    return "不承兑";
                case "10":
                    return "承兑部分金额";
                case "12":
                    return "无效交易";
                case "13":
                    return "无效金额";
                case "14":
                    return "无此卡号";
                case "19":
                    return "稍候重做交易";
                case "23":
                    return "不能接受的交易费";
                case "24":
                    return "接收者不支持";
                case "25":
                    return "记录不存在";
                case "26":
                    return "重复的文件更新记录";
                case "27":
                    return "文件更新域错";
                case "28":
                    return "文件锁定";
                case "29":
                    return "文件更新不成功";
                case "30":
                    return "格式错误";
                case "31":
                    return "交换站不支持代理方";
                case "33":
                    return "到期卡, 请没收";
                case "34":
                    return "舞弊嫌疑, 请没收";
                case "35":
                    return "与受卡行联系";
                case "36":
                    return "";
                case "40":
                    return "";
                case "41":
                    return "";
                case "43":
                    return "";
                case "51":
                    return "";
                case "53":
                    return "";
                case "54":
                    return "";
                case "55":
                    return "";
                case "56":
                    return "";
                case "57":
                    return "";
                case "58":
                    return "";
                case "59":
                    return "";
                case "61":
                    return "";
                case "65":
                    return "";
                case "68":
                    return "";
                case "75":
                    return "";
                case "76":
                    return "";
                case "78":
                    return "";
                case "79":
                    return "";
                case "80":
                    return "";
                case "81":
                    return "";
                case "82":
                    return "";
                case "83":
                    return "";
                case "84":
                    return "";
                case "85":
                    return "";
                case "86":
                    return "";
                case "88":
                    return "";
                case "89":
                    return "";
                case "90":
                    return "";
                case "91":
                    return "";
                case "92":
                    return "";
                case "93":
                    return "";
                case "95":
                    return "";
                case "96":
                    return "";
                case "C0":
                    return "";
            }
            return null;
        }

        public static Dictionary<string, string> icPay(string orderNo, string batchNo, string cardNo, String pwd, string cidao2, string cidao3, string period, string serialNo, string area55, string money, TradeRecord tradeRecord, string secondShopNo, string secondClientNo)
        {
            try
            {
                string _7 = getMMDDHHMMSSTime();

                tradeRecord.shop_type = "0";
                tradeRecord.order_type = "1";
                tradeRecord.order_no = orderNo;
                tradeRecord.amount = money;
                tradeRecord.batch_no = batchNo + "|" + _7 + "|" + cardNo;

                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, cardNo);
                map.Add("2", cardNo);

                data.Add(3, "000000");


                data.Add(4, wtPayUtils.ConvertMoney(money));



                data.Add(7, _7);
                data.Add(11, orderNo);//流水号不能重复
                map.Add("11", orderNo);

                //data.Add(14, period);//卡过期时间
                data.Add(22, "051");//输入方式
                if (serialNo != null && !"".Equals(serialNo))
                {
                    data.Add(23, BCDUtil.leftpad(serialNo, 4));
                    map.Add("23", serialNo);

                }

                data.Add(25, "00");

                if (cidao2 != null && !"".Equals(cidao2))
                {
                    data.Add(35, cidao2);
                }
                map.Add("35", cidao2);

                if (cidao3 != null && !"".Equals(cidao3))
                {
                    data.Add(36, cidao3);
                }

                map.Add("36", cidao3);
                //data.Add(41, "29003690");
                //data.Add(42, "822595053310011");
                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                map.Add("41", data[41]);
                tradeRecord.termail_no = data[41];

                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(49, "156");
                data.Add(52, pwd);
                data.Add(53, "2600000000000000");
                if (area55 != null && !"".Equals(area55))
                {
                    data.Add(55, area55);//ic卡交易信息
                    map.Add("55", area55);
                }

                data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
                if (secondShopNo != null && !"".Equals(secondShopNo) && secondClientNo != null && !"".Equals(secondClientNo))
                {
                    data.Add(59, field59(secondShopNo, secondClientNo, data[4], "0"));
                }

                data.Add(60, "01000050");
                data.Add(62, batchNo + orderNo);//票据号不能重复
                map.Add("batch_no", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);


                byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送支付的数据：" + sendStr);

                byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

                //接收的数据
                string receiveStr = BCDUtil.byteArrToString(recData);
                log.Write("接收支付的数据：" + receiveStr);

                AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
                Dictionary<string, ResultData> rd = lk.analysis(recData);
                if (rd == null)
                {
                    return null;
                }

                //string recode = handleDataMac(recData); //发送数据
                string recode = rd["39"].value;
                if ("00".Equals(recode))
                {
                    tradeRecord.lkl_wt_state = "1";
                }
                else if ("80".Equals(recode))
                {
                    DeviceState.SendState("333333");
                }
                else
                {
                    tradeRecord.lkl_wt_state = "2";

                }

                map.Add("recode", recode);
                map.Add("sendStr", BCDUtil.byteArrToString(sendData));
                map.Add("receiveStr", BCDUtil.byteArrToString(recData));
                map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);
                map.Add("cloudOrderNo", rd["37"].value + data[62]);

                tradeRecord.termail_no = data[41];

                string serialStr = JsonConvert.SerializeObject(map);
                tradeRecord.data_id = serialStr;
                TradeBLL.SendOrderPayRecord(tradeRecord);


                return map;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.LKL_PAY, e.Message);
            }



        }

        public static Dictionary<string, string> icPay(string orderNo, string batchNo, string cardNo, String pwd, string cidao2, string cidao3, string period, string serialNo, string area55, string money,  string secondShopNo,string secondClientNo)
        {
            try
            {
                string _7 = getMMDDHHMMSSTime();
                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, cardNo);
                map.Add("2", cardNo);

                data.Add(3, "000000");

                
                data.Add(4, wtPayUtils.ConvertMoney(money));
                


                data.Add(7, _7);
                data.Add(11, orderNo);//流水号不能重复
                map.Add("11", orderNo);

                //data.Add(14, period);//卡过期时间
                data.Add(22, "051");//输入方式
                if (serialNo!=null&&!"".Equals(serialNo))
                {
                    data.Add(23, BCDUtil.leftpad(serialNo, 4));
                    map.Add("23", serialNo);

                }

                data.Add(25, "00");

                if (cidao2!=null && !"".Equals(cidao2))
                {
                    data.Add(35, cidao2);
                }
                map.Add("35", cidao2);

                if (cidao3 != null && !"".Equals(cidao3))
                {
                    data.Add(36, cidao3);
                }
              
                map.Add("36", cidao3);
                //data.Add(41, "29003690");
                //data.Add(42, "822595053310011");
                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                map.Add("41", data[41]);
                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(49, "156");
                data.Add(52, pwd);
                data.Add(53, "2600000000000000");
                if (area55 != null && !"".Equals(area55))
                {
                    data.Add(55, area55);//ic卡交易信息
                    map.Add("55", area55);
                }  

                data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
                if (secondShopNo!= null && !"".Equals(secondShopNo) && secondClientNo!=null && !"".Equals(secondClientNo))
                {
                    data.Add(59, field59(secondShopNo, secondClientNo, data[4], "0"));
                }
             
                data.Add(60, "01000050");
                data.Add(62, batchNo + orderNo);//票据号不能重复
                map.Add("batch_no", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);


                byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送支付的数据：" + sendStr);

                byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

                //接收的数据
                string receiveStr = BCDUtil.byteArrToString(recData);
                log.Write("接收支付的数据：" + receiveStr);

                AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
                Dictionary<string, ResultData> rd = lk.analysis(recData);
                if (rd == null)
                {
                    return null;
                }

                //string recode = handleDataMac(recData); //发送数据
                string recode = rd["39"].value;
                if ("00".Equals(recode))
                {
                }else if ("80".Equals(recode))
                {
                    DeviceState.SendState("333333");
                }
                else
                {

                }

                map.Add("recode", recode);
                map.Add("sendStr", BCDUtil.byteArrToString(sendData));
                map.Add("receiveStr", BCDUtil.byteArrToString(recData));
                map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);
                map.Add("cloudOrderNo",rd["37"].value+data[62]);
                


                return map;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.LKL_PAY, e.Message);
            }



        }
        public static void icPay(string orderNo, string batchNo, string cardNo, String pwd, string cidao2, string cidao3, string period, string serialNo, string area55, string money,   string secondShopNo, string secondClientNo,ref string sendDataStr)
        {
            try
            {
                log.Write("报文实际扣款金额：" + money);
                string _7 = getMMDDHHMMSSTime();
                
                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, cardNo);
                map.Add("2", cardNo);

                data.Add(3, "000000");


                data.Add(4, wtPayUtils.ConvertMoney(money));



                data.Add(7, _7);
                data.Add(11, orderNo);//流水号不能重复
                map.Add("11", orderNo);

                //data.Add(14, period);//卡过期时间
                data.Add(22, "051");//输入方式
                if (serialNo != null && !"".Equals(serialNo))
                {
                    data.Add(23, BCDUtil.leftpad(serialNo, 4));
                    map.Add("23", serialNo);

                }

                data.Add(25, "00");

                if (cidao2 != null && !"".Equals(cidao2))
                {
                    data.Add(35, cidao2);
                }
                map.Add("35", cidao2);

                if (cidao3 != null && !"".Equals(cidao3))
                {
                    data.Add(36, cidao3);
                }

                map.Add("36", cidao3);
                //data.Add(41, "29003690");
                //data.Add(42, "822595053310011");
                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                map.Add("41", data[41]);
                //data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(42, BCDUtil.leftpad(secondShopNo, 15)+BCDUtil.leftpad(secondClientNo, 8)); 
                data.Add(49, "156");
                data.Add(52, pwd);
                data.Add(53, "2600000000000000");
                if (area55 != null && !"".Equals(area55))
                {
                    data.Add(55, area55);//ic卡交易信息
                    map.Add("55", area55);
                }

                data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
                if (secondShopNo != null && !"".Equals(secondShopNo) && secondClientNo != null && !"".Equals(secondClientNo))
                {
                    data.Add(59, field59(secondShopNo, secondClientNo, data[4], "0"));
                }

                data.Add(60, "01000050");
                data.Add(62, batchNo + orderNo);//票据号不能重复
                map.Add("batch_no", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));
                byte[] mac = PasswordBLL.GetMac(1, countMacData);


                byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                sendDataStr = sendStr;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.LKL_PAY, e.Message);
            }



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant">分账商户</param>
        /// <param name="terminalNo">分账终端号</param>
        /// <param name="amount">分账金额</param>
        /// <param name="poundageType">手续费标识</param>
        /// <returns></returns>
        static string field59(string tenant, string terminalNo, string amount, string poundageType)
        {
            tenant = BCDUtil.leftpad(tenant, 15);
            terminalNo = BCDUtil.leftpad(terminalNo, 8);
            return tenant + terminalNo + amount + poundageType;
        }
        public static Dictionary<string, string> pay(string orderNo, string batchNo, string cardNo, string pwd, string cidao2, string cidao3, string money, TradeRecord tradeRecord, string secondShopNo, string secondClientNo)
        {
            try
            {
                tradeRecord.shop_type = "0";
                tradeRecord.order_type = "1";
                tradeRecord.order_no = orderNo;
                tradeRecord.amount = money;
                string _7 = getMMDDHHMMSSTime();

                tradeRecord.batch_no = batchNo + "|" + _7 + "|" + cardNo;
                Dictionary<string, string> map = new Dictionary<string, string>();


                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, cardNo);
                map.Add("2", cardNo);

                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(money));
                data.Add(7, _7);
                data.Add(11, orderNo);
                map.Add("11", data[11]);

                data.Add(22, "021");
                data.Add(25, "00");

                if (cidao2 != null && !"".Equals(cidao2))
                {
                    data.Add(35, cidao2);
                }
                map.Add("35", cidao2);

                if (cidao3 != null && !"".Equals(cidao3))
                {
                    data.Add(36, cidao3);
                }
                map.Add("36", cidao3);

                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                map.Add("41", data[41]);
                tradeRecord.termail_no = data[41];
                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(49, "156");
                data.Add(52, pwd);
                data.Add(53, "2600000000000000");
                //data.Add(55, "01");
                data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");

                if (secondShopNo != null && !"".Equals(secondShopNo) && secondClientNo != null && !"".Equals(secondClientNo))
                {
                    data.Add(59, field59(secondShopNo, secondClientNo, data[4], "0"));
                }

                data.Add(60, "010000");
                data.Add(62, batchNo + orderNo);
                map.Add("batchNo", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);

                byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送支付的数据：" + sendStr);

                byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

                //接收的数据
                string result = BCDUtil.byteArrToString(recData);
                log.Write("接收支付的数据：" + result);
                AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
                Dictionary<string, ResultData> rd = lk.analysis(recData);
                if (rd == null)
                {
                    return null;
                }

                string recode = handleDataMac(recData); //发送数据
                if ("00".Equals(recode))
                {
                    tradeRecord.lkl_wt_state = "1";
                }
                else if ("80".Equals(recode))
                {
                    DeviceState.SendState("333333");
                }
                else
                {
                    tradeRecord.lkl_wt_state = "2";

                }

                map.Add("23", "");
                map.Add("55", "");
                map.Add("recode", recode);
                map.Add("sendStr", BCDUtil.byteArrToString(sendData));
                map.Add("receiveStr", BCDUtil.byteArrToString(recData));
                map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);
                map.Add("cloudOrderNo", rd["37"].value + data[62]);

                string serialStr = JsonConvert.SerializeObject(map);
                tradeRecord.data_id = serialStr;
                TradeBLL.SendOrderPayRecord(tradeRecord);
                return map;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.LKL_PAY, e.Message);
            }

        }

        public static Dictionary<string, string> pay(string orderNo, string batchNo, string cardNo, string pwd, string cidao2, string cidao3, string money,  string secondShopNo,string  secondClientNo)
        {
            try
            {
                string _7 = getMMDDHHMMSSTime();
                
                Dictionary<string, string> map = new Dictionary<string, string>();


                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, cardNo);
                map.Add("2", cardNo);

                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(money));
                data.Add(7, _7);
                data.Add(11, orderNo);
                map.Add("11", data[11]);

                data.Add(22, "021");
                data.Add(25, "00");

                if (cidao2 != null && !"".Equals(cidao2))
                {
                    data.Add(35, cidao2);
                }
                map.Add("35", cidao2);
            
                if (cidao3 != null && !"".Equals(cidao3))
                {
                    data.Add(36, cidao3);
                }
                map.Add("36", cidao3);

                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                map.Add("41", data[41]);
                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(49, "156");
                data.Add(52, pwd);
                data.Add(53, "2600000000000000");
                //data.Add(55, "01");
                data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
                
                if (secondShopNo != null && !"".Equals(secondShopNo) && secondClientNo != null && !"".Equals(secondClientNo))
                {
                    data.Add(59, field59(secondShopNo, secondClientNo, data[4], "0"));
                }

                data.Add(60, "010000");
                data.Add(62, batchNo + orderNo);
                map.Add("batchNo", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);

                byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送支付的数据：" + sendStr);

                byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

                //接收的数据
                string result = BCDUtil.byteArrToString(recData);
                log.Write("接收支付的数据：" + result);
                AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
                Dictionary<string, ResultData> rd = lk.analysis(recData);
                if (rd == null)
                {
                    return null;
                }

                string recode = handleDataMac(recData); //发送数据
                if ("00".Equals(recode))
                {
                    
                }
                else if ("80".Equals(recode))
                {
                    DeviceState.SendState("333333");
                }
                else
                {
                    

                }

                map.Add("23", "");
                map.Add("55", "");
                map.Add("recode", recode);
                map.Add("sendStr", BCDUtil.byteArrToString(sendData));
                map.Add("receiveStr", BCDUtil.byteArrToString(recData));
                map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);
                map.Add("cloudOrderNo", rd["37"].value + data[62]);

                string serialStr = JsonConvert.SerializeObject(map);
               
                return map;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.LKL_PAY, e.Message);
            }

        }
        public static void pay(string orderNo, string batchNo, string cardNo, string pwd, string cidao2, string cidao3, string money,   string secondShopNo, string secondClientNo,ref string sendDataStr)
        {
            try
            {
                log.Write("报文实际扣款金额："+money);
                string _7 = getMMDDHHMMSSTime();
                Dictionary<string, string> map = new Dictionary<string, string>();


                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, cardNo);
                map.Add("2", cardNo);

                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(money));
                data.Add(7, _7);
                data.Add(11, orderNo);
                map.Add("11", data[11]);

                data.Add(22, "021");
                data.Add(25, "00");

                if (cidao2 != null && !"".Equals(cidao2))
                {
                    data.Add(35, cidao2);
                }
                map.Add("35", cidao2);

                if (cidao3 != null && !"".Equals(cidao3))
                {
                    data.Add(36, cidao3);
                }
                map.Add("36", cidao3);

                data.Add(41, SysConfigHelper.readerNode("LklClientNo"));
                map.Add("41", data[41]);
                //data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                //data.Add(42, secondShopNo); 
                data.Add(42, BCDUtil.leftpad(secondShopNo, 15) + BCDUtil.leftpad(secondClientNo, 8));
                data.Add(49, "156");
                data.Add(52, pwd);
                data.Add(53, "2600000000000000");
                //data.Add(55, "01");
                data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");

                if (secondShopNo != null && !"".Equals(secondShopNo) && secondClientNo != null && !"".Equals(secondClientNo))
                {
                    data.Add(59, field59(secondShopNo, secondClientNo, data[4], "0"));
                }

                data.Add(60, "010000");
                data.Add(62, batchNo + orderNo);
                map.Add("batchNo", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);

                byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                sendDataStr = sendStr;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.LKL_PAY, e.Message);
            }

        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> correct(Dictionary<string, string> icParams, string money, string orderNo, string batchNo, TradeRecord tradeRecord)
        {
            try
            {
                tradeRecord.order_type = "2";
                string termailNo = SysConfigHelper.readerNode("LklClientNo");
                tradeRecord.amount = money;
                tradeRecord.batch_no = batchNo;
                tradeRecord.order_no = orderNo;
                tradeRecord.termail_no = termailNo;
                tradeRecord.cloud_state = false;
                tradeRecord.lkl_wt_state = "0";
                tradeRecord.order_state = "1";
                tradeRecord.shop_type = "0";

                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, icParams["cardNo"]);
                data.Add(3, "000000");
                ///待确认
                data.Add(4, wtPayUtils.ConvertMoney(money));
                data.Add(7, getMMDDHHMMSSTime());
                data.Add(11, orderNo);//关联订单号
                map.Add("11", data[11]);

                data.Add(22, "051");
                data.Add(25, "00");

                if (icParams["cidao2"] != null && !"".Equals(icParams["cidao2"]))
                {

                    data.Add(35, icParams["cidao2"]);
                }


                if (icParams["cidao3"] != null && !"".Equals(icParams["cidao3"]))
                {

                    data.Add(36, icParams["cidao3"]);
                }


                data.Add(41, termailNo);
                map.Add("41", data[41]);

                tradeRecord.termail_no = data[41];

                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(49, "156");
                data.Add(60, "010000");
                data.Add(62, batchNo + orderNo);
                map.Add("batchNo", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0400", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);

                byte[] sendData = EncryptLakala.EncryptData("0400", "6009070000", EncryptLakala.encryptLKL(data, mac));

                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送冲正的数据：" + sendStr);

                byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

                //接收的数据
                string result = BCDUtil.byteArrToString(recData);
                log.Write("接收冲正的数据：" + result);

                AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
                Dictionary<string, ResultData> rd = lk.analysis(recData);
                if (rd == null)
                {
                    return null;
                }

                string recode = handleDataMac(recData); //发送数据
                if ("00".Equals(recode))
                {
                    tradeRecord.lkl_wt_state = "1";
                }

                map.Add("recode", recode);
                map.Add("sendStr", BCDUtil.byteArrToString(sendData));
                map.Add("receiveStr", BCDUtil.byteArrToString(recData));
                map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

                string serialStr = JsonConvert.SerializeObject(map);
                tradeRecord.data_id = serialStr;
                TradeBLL.SendOrderPayRecord(tradeRecord);

                return map;



                //byte[] countMacData = EncryptLakala.countMacData("0400", EncryptLakala.encryptLKL(data, null));

                //byte[] sendData = EncryptLakala.EncryptData("0400", "6009070000", EncryptLakala.encryptLKL(data, calculateMac(data, countMacData)));
                //return Client(LakalaIp, LakalaPort, sendData); //发送数据

            }
            catch (Exception e)
            {

                return new Dictionary<string, string>();
            }
        }


        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> correct(Dictionary<string, string> icParams, string money, string orderNo, string batchNo )
        {
            try
            {
                string termailNo = SysConfigHelper.readerNode("LklClientNo");

                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, icParams["cardNo"]);
                data.Add(3, "000000");
                ///待确认
                data.Add(4, wtPayUtils.ConvertMoney(money));
                data.Add(7, getMMDDHHMMSSTime());
                data.Add(11, orderNo);//关联订单号
                map.Add("11",data[11]);
                
                data.Add(22, "051");
                data.Add(25, "00");

                if (icParams["cidao2"]!=null && !"".Equals(icParams["cidao2"])) {

                    data.Add(35, icParams["cidao2"]);
                }


                if (icParams["cidao3"] != null && !"".Equals(icParams["cidao3"]))
                {

                    data.Add(36, icParams["cidao3"]);
                }

                
                data.Add(41, termailNo);
                map.Add("41", data[41]);
                
                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(49, "156");
                data.Add(60, "010000");
                data.Add(62, batchNo + orderNo);
                map.Add("batchNo", batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0400", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);

                byte[] sendData = EncryptLakala.EncryptData("0400", "6009070000", EncryptLakala.encryptLKL(data, mac));

                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送冲正的数据：" + sendStr);
                
                byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

                //接收的数据
                string result = BCDUtil.byteArrToString(recData);
                log.Write("接收冲正的数据：" + result);

                AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
                Dictionary<string, ResultData> rd = lk.analysis(recData);
                if (rd == null)
                {
                    return null;
                }

                string recode = handleDataMac(recData); //发送数据
                if ("00".Equals(recode))
                {
                   
                }

                map.Add("recode", recode);
                map.Add("sendStr", BCDUtil.byteArrToString(sendData));
                map.Add("receiveStr", BCDUtil.byteArrToString(recData));
                map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

                string serialStr = JsonConvert.SerializeObject(map);

                return map;



                //byte[] countMacData = EncryptLakala.countMacData("0400", EncryptLakala.encryptLKL(data, null));

                //byte[] sendData = EncryptLakala.EncryptData("0400", "6009070000", EncryptLakala.encryptLKL(data, calculateMac(data, countMacData)));
                //return Client(LakalaIp, LakalaPort, sendData); //发送数据

            }
            catch (Exception e)
            {

                return new Dictionary<string, string>();
            }
        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>// , , , ref sendDataStr);
        public static void correct(PayParam p,ref string sendDataStr)
        {
            try
            {
                string termailNo = SysConfigHelper.readerNode("LklClientNo");
                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, p.icParams["cardNo"]);
                data.Add(3, "000000");
                ///待确认
                data.Add(4, wtPayUtils.ConvertMoney(p.rechageAmount));
                data.Add(7, getMMDDHHMMSSTime());
                data.Add(11, p.WtLklorderNo);//关联订单号
                map.Add("11", data[11]);

                data.Add(22, "051");
                data.Add(25, "00");

                if (p.icParams["cidao2"] != null && !"".Equals(p.icParams["cidao2"]))
                {

                    data.Add(35, p.icParams["cidao2"]);
                }


                if (p.icParams["cidao3"] != null && !"".Equals(p.icParams["cidao3"]))
                {

                    data.Add(36, p.icParams["cidao3"]);
                }


                data.Add(41, termailNo);
                map.Add("41", data[41]);
                
                data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
                data.Add(49, "156");
                data.Add(60, "010000");
                data.Add(62, p.batchNo + p.WtLklorderNo);
                map.Add("batchNo", p.batchNo);
                byte[] countMacData = EncryptLakala.countMacData("0400", EncryptLakala.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(1, countMacData);

                byte[] sendData = EncryptLakala.EncryptData("0400", "6009070000", EncryptLakala.encryptLKL(data, mac));
                sendDataStr = BCDUtil.byteArrToString(sendData);
               
            }
            catch (Exception e)
            {
                
            }
        }
        public static Dictionary<string, string> PayOrder(String pwd, string money, Dictionary<string, string> icParams, string orderNo, string batchNo, TradeRecord tradeRecord, string secondShopNo, string secondClientNo)
        {
            tradeRecord.relation_order = "";

            tradeRecord.amount = money;
            try
            {
                if (icParams.ContainsKey("area55"))
                {

                    return icPay(orderNo, batchNo, icParams["cardNo"], pwd, icParams["cidao2"], icParams["cidao3"], icParams["period"], icParams["serialNo"], icParams["area55"], money, tradeRecord, secondShopNo, secondClientNo);
                }


                else
                {
                    return pay(orderNo, batchNo, icParams["cardNo"], pwd, icParams["cidao2"], icParams["cidao3"], money, tradeRecord, secondShopNo, secondClientNo);
                }
            }
            catch (WtException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new WtException(WtExceptionCode.Card.UNION_READ_CARD, e.Message);
            }
        }

        public static Dictionary<string, string> PayOrder(String pwd, string money, Dictionary<string, string> icParams, string orderNo, string batchNo,  string secondShopNo,string secondClientNo)
        {
            try
            { 
                if (icParams.ContainsKey("area55")){
                    
                    return icPay(orderNo, batchNo, icParams["cardNo"], pwd, icParams["cidao2"], icParams["cidao3"], icParams["period"], icParams["serialNo"], icParams["area55"], money, secondShopNo, secondClientNo);
                }

            
                else
                {
                    return pay(orderNo, batchNo, icParams["cardNo"], pwd, icParams["cidao2"], icParams["cidao3"], money,   secondShopNo, secondClientNo);
                }
            }
            catch (WtException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new WtException(WtExceptionCode.Card.UNION_READ_CARD, e.Message);
            }
        }

        //(p.pwd, p.rechageAmount, p.icParams, p.orderNo, p.batchNo, p.MERCHANTNO_shopNo, p.TERMINALNO_clientNo,ref sendDataStr);
        public static void PayOrder(PayParam p, ref string sendDataStr)
        {
            try
            {
                if (p.icParams.ContainsKey("area55"))
                {

                    icPay(p.WtLklorderNo, p.batchNo, p.icParams["cardNo"], p.pwd, p.icParams["cidao2"], p.icParams["cidao3"], p.icParams["period"], p.icParams["serialNo"], p.icParams["area55"], p.rechageAmount, p.MERCHANTNO_shopNo, p.TERMINALNO_clientNo, ref sendDataStr);
                }
                else
                {
                    pay(p.WtLklorderNo, p.batchNo, p.icParams["cardNo"], p.pwd, p.icParams["cidao2"], p.icParams["cidao3"], p.rechageAmount, p.MERCHANTNO_shopNo, p.TERMINALNO_clientNo, ref sendDataStr);
                }
            }
            catch (WtException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new WtException(WtExceptionCode.Card.UNION_READ_CARD, e.Message);
            }
        }
        /// <summary>
        /// 消费撤销
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> payRevoke(string _2, string _23, string _35, string _36, string _55, string pwd, string money, string orderNo, string batchNo, TradeRecord tradeRecord)
        {
            string _22 = "051";
            //判断是IC卡还是磁条卡
            if (_55 == null || "".Equals(_55))
            {
                return PayRevokeByCitiao(_2, _35, _36, pwd, money, orderNo, batchNo, tradeRecord);
            }

            string theOrderNo = LKLProcedure.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            string shopNo = SysConfigHelper.readerNode("LklShopNo");
            tradeRecord.amount = money;
            tradeRecord.batch_no = batchNo;
            tradeRecord.order_no = theOrderNo;
            tradeRecord.relation_order = orderNo;
            tradeRecord.termail_no = termailNo;
            tradeRecord.lkl_wt_shop_no = shopNo;

            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);//关联订单号
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(23, BCDUtil.leftpad(_23, 4));
            data.Add(25, "00");
            data.Add(35, _35);
            if (_36 != null && !"".Equals(_36.Trim()))
            {
                data.Add(36, _36);
            }

            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");
            data.Add(55, _55);
            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "01000050");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo + theOrderNo + orderNo);//"123456" + tranNo + tranNo
            map.Add("batchNo", batchNo);

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            log.Write("发送退款的数据：" + sendStr);

            byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

            //接收的数据
            string result = BCDUtil.byteArrToString(recData);
            log.Write("接收退款的数据：" + result);

            AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            Dictionary<string, ResultData> rd = lk.analysis(recData);
            if (rd == null)
            {
                return null;
            }

            string recode = handleDataMac(recData); //发送数据
            if ("00".Equals(recode))
            {
                tradeRecord.lkl_wt_state = "1";
            }
            map.Add("recode", recode);
            map.Add("sendStr", BCDUtil.byteArrToString(sendData));
            map.Add("receiveStr", BCDUtil.byteArrToString(recData));
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

            string serialStr = JsonConvert.SerializeObject(map);
            tradeRecord.data_id = serialStr;
            tradeRecord.reconc_str = "";
            TradeBLL.SendOrderRefundRecord(tradeRecord);
            return map;
        }

        /// <summary>
        /// 消费撤销
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> payRevoke(Dictionary<string, string> icParams,string pwd, string money, string orderNo, string batchNo )
        {
            string theOrderNo = GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            Dictionary<string, string> map = new Dictionary<string, string>();
          
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, icParams["cardNo"]);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(23, BCDUtil.leftpad(icParams["serialNo"], 4));
            data.Add(25, "00");
            data.Add(35, icParams["cidao2"]);
            if (icParams["cidao3"] != null && !"".Equals(icParams["cidao3"]))
            {
                data.Add(36, icParams["cidao3"]);
            }
         
            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, SysConfigHelper.readerNode("LklShopNo"));
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");
            data.Add(55, icParams["area55"]);
            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "01000050");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo + theOrderNo + orderNo);//"123456" + tranNo + tranNo

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            log.Write("发送退款的数据：" + sendStr);

            byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

            //接收的数据
            string result = BCDUtil.byteArrToString(recData);
            log.Write("接收退款的数据：" + result);

            AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            Dictionary<string, ResultData> rd = lk.analysis(recData);
            if (rd == null)
            {
                return null;
            }

            string recode = handleDataMac(recData); //发送数据
            if ("00".Equals(recode))
            {
            }

            map.Add("recode", recode);
            map.Add("sendStr", BCDUtil.byteArrToString(sendData));
            map.Add("receiveStr", BCDUtil.byteArrToString(recData));
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

            string serialStr = JsonConvert.SerializeObject(map);

            return map;


            //byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));
            //BCDUtil.printData("mac计算数据", countMacData);
            //Console.WriteLine("加密macKey" + mackey);
            //byte[] mac = calculateMac(data, countMacData);
            //Console.WriteLine("macmacmacmacmacmac:" + BCDUtil.byteArrToString(mac));
            //byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            //byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;


            //return recData;
        }

        /// <summary>
        /// 拉卡拉消费撤销
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> payRevoke(string _2,string _23,string _35,string _36,string _55, string pwd, string money, string orderNo, string batchNo)
        {
            string _22 = "051";
            //判断是IC卡还是磁条卡
            if (_55 ==null || "".Equals(_55))
            {
                return PayRevokeByCitiao( _2,  _35,  _36,  pwd,  money,  orderNo,  batchNo);
            }

            string theOrderNo = LKLProcedure.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            string shopNo = SysConfigHelper.readerNode("LklShopNo");

            Dictionary<string, string> map = new Dictionary<string, string>();
           
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);//关联订单号
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(23, BCDUtil.leftpad(_23, 4));
            data.Add(25, "00");
            data.Add(35, _35);
            if (_36 != null && !"".Equals(_36.Trim()))
            {
                data.Add(36, _36);
            }

            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");
            data.Add(55, _55);
            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "01000050");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo + theOrderNo + orderNo);//"123456" + tranNo + tranNo
            map.Add("batchNo", batchNo);

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            log.Write("发送退款的数据：" + sendStr);

            byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

            //接收的数据
            string result = BCDUtil.byteArrToString(recData);
            log.Write("接收退款的数据：" + result);

            AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            Dictionary<string, ResultData> rd = lk.analysis(recData);
            if (rd == null)
            {
                return null;
            }

            string recode = handleDataMac(recData); //发送数据
            if ("00".Equals(recode))
            {
            }
            map.Add("recode", recode);
            map.Add("sendStr", BCDUtil.byteArrToString(sendData));
            map.Add("receiveStr", BCDUtil.byteArrToString(recData));
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

            string serialStr = JsonConvert.SerializeObject(map);
            return map;
        }
        /// <summary>
        /// 拉卡拉消费撤销
        /// </summary>
        /// <returns></returns>
        public static string wtPayRevoke(Dictionary<string,string> list, PayParam payParam)
        {
            string _2 = list["2"].Substring(0, list["2"].Length-1);
            log.Write("卡号："+ list["2"]);
            string _35 = SysBLL.IcBankDictionary["cidao2"];
            string _36 = SysBLL.IcBankDictionary["cidao3"];
            string _55 = "";
            string pwd = payParam.pwd;
            string money = payParam.rechageAmount;
            string orderNo = payParam.WtLklorderNo;
            string batchNo = payParam.batchNo;
            string _22 = "051";
            //判断是IC卡还是磁条卡
            if (!SysBLL.IcBankDictionary.ContainsKey("area55"))
            { 
                //_55 = SysBLL.IcBankDictionary["area55"];
                //if (_55 == null || "".Equals(_55))
                //{
                    return LKLPayRevokeByCitiao(_2, _35, _36, pwd, money, orderNo, batchNo);
                //}
            }
            else
            {
                _55 = SysBLL.IcBankDictionary["area55"];
            }

            string theOrderNo = LKLProcedure.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            string shopNo = SysConfigHelper.readerNode("LklShopNo");

            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);//关联订单号
            map.Add("11", data[11]);

            data.Add(22, "051");
            if(SysBLL.IcBankDictionary.ContainsKey("serialNo"))
            data.Add(23, BCDUtil.leftpad(SysBLL.IcBankDictionary["serialNo"], 4));
            data.Add(25, "00");
            data.Add(35, _35);
            if (_36 != null && !"".Equals(_36.Trim()))
            {
                data.Add(36, _36);
            }

            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");
            data.Add(55, _55);
            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "01000050");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo  + theOrderNo + orderNo);//"123456" + tranNo + tranNo
            map.Add("batchNo", batchNo);

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            
            return sendStr;
        }
        private static Dictionary<string, string> PayRevokeByCitiao(string _2, string _35, string _36, string pwd, string money, string orderNo, string batchNo, TradeRecord tradeRecord)
        {
            string theOrderNo = LKLProcedure.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            string shopNo = SysConfigHelper.readerNode("LklShopNo");
            tradeRecord.amount = money;
            tradeRecord.batch_no = batchNo;
            tradeRecord.order_no = theOrderNo;
            tradeRecord.relation_order = orderNo;
            tradeRecord.termail_no = termailNo;
            tradeRecord.lkl_wt_shop_no = shopNo;

            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);//关联订单号
            map.Add("11", data[11]);

            data.Add(22, "021");

            data.Add(25, "00");

            if (_35 != null && !"".Equals(_35.Trim()))
            {
                data.Add(35, _35);
            }

            if (_36 != null && !"".Equals(_36.Trim()))
            {
                data.Add(36, _36);
            }

            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");

            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "010000");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo + theOrderNo + orderNo);//"123456" + tranNo + tranNo
            map.Add("batchNo", batchNo);

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            log.Write("发送退款的数据：" + sendStr);

            byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

            //接收的数据
            string result = BCDUtil.byteArrToString(recData);
            log.Write("接收退款的数据：" + result);

            AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            Dictionary<string, ResultData> rd = lk.analysis(recData);
            if (rd == null)
            {
                return null;
            }

            string recode = handleDataMac(recData); //发送数据
            if ("00".Equals(recode))
            {
                tradeRecord.lkl_wt_state = "1";
            }
            log.Write("退款应答码：" + recode);

            map.Add("recode", recode);
            map.Add("sendStr", BCDUtil.byteArrToString(sendData));
            map.Add("receiveStr", BCDUtil.byteArrToString(recData));
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

            string serialStr = JsonConvert.SerializeObject(map);
            tradeRecord.data_id = serialStr;
            tradeRecord.reconc_str = "";
            TradeBLL.SendOrderRefundRecord(tradeRecord);
            return map;
        }
        private static Dictionary<string, string> PayRevokeByCitiao(string _2, string _35, string _36, string pwd, string money, string orderNo, string batchNo)
        {
            string theOrderNo = LKLProcedure.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            string shopNo = SysConfigHelper.readerNode("LklShopNo");
            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);//关联订单号
            map.Add("11", data[11]);

            data.Add(22, "021");
           
            data.Add(25, "00");

            if (_35 != null && !"".Equals(_35.Trim()))
            {
                data.Add(35, _35);
            }
           
            if (_36 != null && !"".Equals(_36.Trim()))
            {
                data.Add(36, _36);
            }

            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");
           
            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "010000");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo + theOrderNo + orderNo);//"123456" + tranNo + tranNo
            map.Add("batchNo", batchNo);

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            log.Write("发送退款的数据：" + sendStr);

            byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

            //接收的数据
            string result = BCDUtil.byteArrToString(recData);
            log.Write("接收退款的数据：" + result);

            AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            Dictionary<string, ResultData> rd = lk.analysis(recData);
            if (rd == null)
            {
                return null;
            }

            string recode = handleDataMac(recData); //发送数据
            if ("00".Equals(recode))
            {
            }
            log.Write("退款应答码：" + recode);

            map.Add("recode", recode);
            map.Add("sendStr", BCDUtil.byteArrToString(sendData));
            map.Add("receiveStr", BCDUtil.byteArrToString(recData));
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

            string serialStr = JsonConvert.SerializeObject(map);
            return map;
        }
        private static string LKLPayRevokeByCitiao(string _2, string _35, string _36, string pwd, string money, string orderNo, string batchNo)
        {
            string theOrderNo = LKLProcedure.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            string shopNo = SysConfigHelper.readerNode("LklShopNo");

            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);//关联订单号
            map.Add("11", data[11]);

            data.Add(22, "021");

            data.Add(25, "00");

            if (_35 != null && !"".Equals(_35.Trim()))
            {
                data.Add(35, _35);
            }

            if (_36 != null && !"".Equals(_36.Trim()))
            {
                data.Add(36, _36);
            }

            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");

            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "010000");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo + theOrderNo + orderNo);//"123456" + tranNo + tranNo
            map.Add("batchNo", batchNo);

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            
            return sendStr;
        }

        /// <summary>
        /// 签到后，批次号+1
        /// </summary>
        public static void RebuildBatchNo()
        {
            //string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "LklPay.xml";
            string batchNo = SysConfigHelper.readerNode("LklBatchNo");
            string newBatchNo = (Convert.ToInt32(batchNo) + 1).ToString();
            SysConfigHelper.writerNode("LklBatchNo", newBatchNo);
            //SysConfigHelper.writerNode(xmlPath, "orderNo", "100000", "config");
        }
        /// <summary>
        /// 获取交易批次号
        /// </summary>
        /// <returns></returns>
        public static string GetBatchNo()
        {
            //string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "LklPay.xml";
            string batchNo = SysConfigHelper.readerNode("LklBatchNo");
            return batchNo;
        }

        /// <summary>
        /// 获取交易流水号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNo()
        {
            //string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "LklPay.xml";
            string orderNo = SysConfigHelper.readerNode("LklOrderNo");
            string newOrderNo = (Convert.ToInt32(orderNo) + 1).ToString();
            SysConfigHelper.writerNode("LklOrderNo", newOrderNo);
            return orderNo;
        }
        /// <summary>
        /// 拉卡拉消费撤销
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> payRevoke1(Dictionary<string, string> list, PayParam payParam)
        {
            string _2=list["2"];
            string _23=list["23"];
            string _35=list["35"];
            string _36 = list["36"]; ;
            string _55=list["55"];
            string pwd=payParam.pwd;
            string money=payParam.rechageAmount;
            string orderNo=payParam.cloudNo;
            string batchNo = payParam.batchNo;
            string _22 = "051";
            //判断是IC卡还是磁条卡
            if (_55 == null || "".Equals(_55))
            {
                return PayRevokeByCitiao(_2, _35, _36, pwd, money, orderNo, batchNo);
            }

            string theOrderNo = LKLProcedure.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("LklClientNo");
            string shopNo = SysConfigHelper.readerNode("LklShopNo");

            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(money));
            data.Add(11, theOrderNo);//关联订单号
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(23, BCDUtil.leftpad(_23, 4));
            data.Add(25, "00");
            data.Add(35, _35);
            if (_36 != null && !"".Equals(_36.Trim()))
            {
                data.Add(36, _36);
            }

            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(49, "156");
            data.Add(52, pwd);
            data.Add(53, "2600000000000000");
            data.Add(55, _55);
            data.Add(57, "BEIKEWEITUO A8V2.1.1.5844.0");
            data.Add(60, "01000050");
            //上一个的票据号
            //123456:批次号 第一个tranNo 本单流水号  第二个tranNo 退款单流水号
            data.Add(62, batchNo + theOrderNo + orderNo);//"123456" + tranNo + tranNo
            map.Add("batchNo", batchNo);

            byte[] countMacData = EncryptLakala.countMacData("0200", EncryptLakala.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(1, countMacData);

            byte[] sendData = EncryptLakala.EncryptData("0200", "6009070000", EncryptLakala.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            log.Write("发送退款的数据：" + sendStr);

            byte[] recData = Client(LakalaIp, LakalaPort, sendData); //发送数据;

            //接收的数据
            string result = BCDUtil.byteArrToString(recData);
            log.Write("接收退款的数据：" + result);

            AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            Dictionary<string, ResultData> rd = lk.analysis(recData);
            if (rd == null)
            {
                return null;
            }

            string recode = handleDataMac(recData); //发送数据
            if ("00".Equals(recode))
            {
            }
            map.Add("recode", recode);
            map.Add("sendStr", BCDUtil.byteArrToString(sendData));
            map.Add("receiveStr", BCDUtil.byteArrToString(recData));
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42] + "|" + data[62]);

            string serialStr = JsonConvert.SerializeObject(map);
            return map;
        }
    }
}
