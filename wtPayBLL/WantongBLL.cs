using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtPayModel;
using wtPayCommon;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;
using AnalysisBase;
using System.Net.Sockets;
using wtPayModel.WintopModel;
using System.Net;
using wtPayModel.PayParamModel;

namespace wtPayBLL
{
    public class WantongBLL
    {
        //public override Card getCardInfo(string cardNo)
        //{
        //    //TODO 调用万通卡查询余额的方法，返回卡片信息

        //    return null;
        //}

        //public override bool Recharge(string cardNo, int gasValue)
        //{
        //    throw new NotImplementedException();
        //}

        //public override bool Recharge(string cardNo, double moneyCount)
        //{
        //    //TODO 调用拉卡拉消费的方法
        //    //TODO 调用拉卡拉消费的方法，如果失败，提示错误，跳出方法

        //    //TODO 调用拉卡拉消费的方法，如果成功，调用万通卡的充值方法
        //    return false;

        //}

        /// <summary>
        /// 调用读卡器，拿到万通卡卡号
        /// </summary>
        /// <returns></returns>
        public string GetCardNo()
        {
            string port = SysConfigHelper.readerNode("CRT310Port");
            StringBuilder info = new StringBuilder(260);
            int handle = CRT310.CommOpen("COM" + port);
            try
            {

                //进卡控制
                CRT310.CRT310_CardSetting(handle, 0x3, 0x1);
                //停卡位置
                CRT310.CRT310_CardPosition(handle, 0x4);

                Byte atPosition = new byte();
                Byte frontSetting = new byte();
                Byte rearSetting = new byte();

                //读取状态
                int hasCard = CRT310.CRT310_GetStatus(handle, ref atPosition, ref frontSetting, ref rearSetting);//卡没插好，怎么办

                Byte cpuType = new byte();
                Byte[] exdata = new byte[1024];
                int exdataLen = 0;
                //CPU卡复位
                CRT310.CPU_ColdReset(handle, 0x0, ref cpuType, exdata, ref exdataLen);

                executeAPDU(handle, "00A40000023F00");

                executeAPDU(handle, "00A40000023F20");

                string cardNoSource = executeAPDU(handle, "00B0950000");
                //进卡控制
                CRT310.CRT310_CardSetting(handle, 0x1, 0x1);

                string result = DecryptCardNo(cardNoSource);
                return result;
            }
            catch (Exception e)
            {
                throw new WtException(WtExceptionCode.Card.WT_READ_CARD, e.Message);
            }
            finally
            {
                //关闭端口
                CRT310.CommClose(handle);
            }
        }


        /// <summary>
        /// CRT603读万通卡
        /// </summary>
        /// <returns></returns>
        public string GetCardNoByBusReader()
        {
            try
            {
                string port = SysConfigHelper.readerNode("CRT603Port");
                //int openRet = CRT603.CRT603Vx_OpenConnection(Int32.Parse(port), 19200);

                //上电
                int iOutAtrLen = 0;
                byte[] byOutAtrData = new byte[1024];
                int chipRet = CRT603.CRT603Vx_RF_chipPower(ref iOutAtrLen, byOutAtrData);

                string firstAction = "00A40000023F00";
                Byte[] sendData = System.Text.Encoding.ASCII.GetBytes(firstAction);

                int[] exdataLen1 = new int[2];
                Byte[] RxData = new Byte[1024];

                int ret = CRT603.CRT603Vx_RF_SendApdu(sendData.Length, sendData, exdataLen1, RxData);
                if (ret == 0)
                {
                    string secondAction = "00A40000023F20";
                    sendData = System.Text.Encoding.ASCII.GetBytes(secondAction);
                    int ret2 = CRT603.CRT603Vx_RF_SendApdu(sendData.Length, sendData, exdataLen1, RxData);

                    if (ret2 == 0)
                    {
                        string thirdAction = "00B0950000";
                        sendData = System.Text.Encoding.ASCII.GetBytes(thirdAction);
                        int ret3 = CRT603.CRT603Vx_RF_SendApdu(sendData.Length, sendData, exdataLen1, RxData);
                        string strRecv3 = "";
                        for (int i = 0; i < exdataLen1[0]; i++)
                        {
                            strRecv3 += string.Format("{0:X2}", RxData[i]);

                        }
                        //CRT603.CRT603Vx_CloseConnection();
                        if (strRecv3.Length == 0) return null;
                        string result = DecryptCardNo(strRecv3);
                        if (result == null)
                        {
                            return null;
                        }
                        return result;
                    }
                }
                //CRT603.CRT603Vx_CloseConnection();
                return null;
            }
            catch (Exception e)
            {
                log.Write("error:读取万通卡异常："+e.Message);
                return null;
            }
        }
        /// <summary>
        /// 读卡器执行APDU指令
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="apdu"></param>
        /// <returns></returns>
        public string executeAPDU(int handle, string apdu)
        {
            string str1 = apdu;
            Byte[] sendData = new byte[str1.Length / 2];
            for (int N = 0; N < (str1.Length / 2); N++)
            {
                sendData[N] = byte.Parse(str1.Substring(N * 2, 2), NumberStyles.HexNumber);
            }
            Byte[] exdata1 = new byte[1024];
            int exdataLen1 = 0;
            //执行APDU
            CRT310.CPU_T0_C_APDU(handle, sendData.Length, sendData, exdata1, ref exdataLen1);

            int n;
            string StrBuf = "";

            for (n = 0; n < exdataLen1; n++)
            {
                StrBuf += exdata1[n].ToString("X2");
            }
            return StrBuf;
        }
        /// <summary>
        /// 对APDU指令读出来的结果，进行解密，拿到万通卡卡号
        /// </summary>
        /// <param name="cardNoSource"></param>
        /// <returns></returns>
        public string DecryptCardNo(string cardNoSource)
        {
            try
            {
                //准备解密
                byte[] cardNoArray = System.Text.Encoding.ASCII.GetBytes(cardNoSource.Substring(98, 48));

                //分散因子
                byte[] appgen = System.Text.Encoding.ASCII.GetBytes(cardNoSource.Substring(64, 16));

                //万通卡卡号解密

                StringBuilder cardNo = new StringBuilder(260);

                CRT310.F005(cardNoArray, cardNo, appgen);

                return cardNo.ToString();
            }
            catch (Exception e) { return null; }
        }

        public static string LakalaIp = SysConfigHelper.readerNode("payIp");//124.74.143.162
        public static int LakalaPort = Convert.ToInt32(SysConfigHelper.readerNode("wtPort"));//6022
        public static string matherKey = "83D7BB2A87F20D7B0F60A4B45E6FD36A";//官方给的key
        public static string sonKey = "1928B1490019409EA6583D4668237EEB"; //string.Empty;//自己颁发的key38A6FBD59F876A82E598F93BED474B40
        public static string mackey = "0CEA58541761E2609539C7469D02A6E5";//string.Empty;
        public static string pinKey = "CFB06A0F384F99DB226E1BAFA752F68A";// string.Empty;

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
                BCDUtil.printData("推送数据", messages);

                //2.接收状态,长度<1024字节
                byte[] bytes = new Byte[1024 * 8];
                string data = string.Empty;
                //读取超时时间，10秒
                stream.ReadTimeout = 30000;
                int length = stream.Read(bytes, 0, bytes.Length);
                if (length == 2)
                {
                    string headLenStr1 = Convert.ToString(bytes[0], 10);
                    string headLenStr2 = Convert.ToString(bytes[1], 10);
                    int headLen = Convert.ToInt32(headLenStr1 + headLenStr2);
                    length = stream.Read(bytes, length, (headLen));
                    length += 2;
                }

                datas = new byte[length];
                Array.Copy(bytes, datas, length);
                BCDUtil.printData("接收数据", datas);
                //3.关闭对象
                stream.Close();
                client.Close();
                return datas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0:HH:mm:ss}->{1}", DateTime.Now, ex.Message));
            }
            //Console.ReadKey();
            datas = new byte[0];
            return datas;
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public static SignParam sign()
        {
            //签到
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(3, "000000");
            data.Add(41, SysConfigHelper.readerNode("ClientNo"));//终端代码
            data.Add(42, SysConfigHelper.readerNode("ShopNo"));//商户代码
            data.Add(63, "01");
            byte[] result = EncryptWT.EncryptData("0800", "6000000000", EncryptWT.encryptLKL(data, null));
            if (result == null || result.Length <= 0)
            {
                return null;
            }
            SignParam param = new SignParam();
            param.data = BCDUtil.byteArrToString(result);
            param.shop_type = "1";
            //Console.WriteLine();
            //AnalysisBaseWT lk = new AnalysisBaseWTSign();
            //Dictionary<string, ResultData> rd = lk.analysis(result);
            return param;
        }
        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="mackey"></param>
        /// <returns></returns>
        public static Dictionary<string, ResultData> pay(WintopPayParam param)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, param.wintopNo);//万通卡号
            data.Add(3, "000000");
            data.Add(4, countPrice(param.price));//交易金额
            data.Add(11, SysBLL.padLeft(getOrder(), 6)); //POS终端交易流水
            data.Add(25, "00");
            data.Add(41, SysConfigHelper.readerNode("ClientNo"));//终端代码
            data.Add(42, SysConfigHelper.readerNode("ShopNo"));//商户代码
            data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(62, getPin(param.pwd, param.wintopNo));//明文，卡号
            data.Add(63, "01");
            byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0200")));
            //Console.WriteLine();
            string datasss = BCDUtil.byteArrToString(sendData);
            //Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);
            if (result.Length > 0)
            {
                return lk.analysis(result); //发送数据
            }
            else
            {
                return null;
            }
        }
        public static string getOrder()
        {
            int num = Convert.ToInt32(SysConfigHelper.readerNode("orderNo"));
            num++;
            SysBLL.SetAppConfig("orderNo", num.ToString());
            return num.ToString();

        }
        public static string countPrice(string price)
        {
            price = Math.Round(Convert.ToDouble(price), 2).ToString();
            string[] s = price.Split(new char[] { '.' });
            if (s.Length == 1)
            {
                s[0] = s[0] + "00";
                price = s[0];
            }
            else
            {
                if (s[1].Length < 2)
                {
                    s[1] = s[1] + "0";
                    price = s[0] + s[1];
                }
            }

            price = price.Replace(".", "");
            if (price.Length < 12)
            {
                while (true)
                {
                    price = "0" + price;
                    if (price.Length >= 12)
                    {
                        break;
                    }
                }
            }
            return price;
        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ResultData> correct(WintopPayParam param)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, param.wintopNo);
            data.Add(3, "000000");
            data.Add(4, countPrice(param.price));
            data.Add(11, SysBLL.padLeft(getOrder(), 6));
            data.Add(25, "00");
            data.Add(39, "17");
            data.Add(41, SysConfigHelper.readerNode("ClientNo"));
            data.Add(42, SysConfigHelper.readerNode("ShopNo"));
            data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(63, "01");
            byte[] sendData = EncryptWT.EncryptData("0400", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0400")));

            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);
            if (result.Length > 0)
            {
                return lk.analysis(result); //发送数据
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 计算mac 公共类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] calculateMac(Dictionary<int, string> data, string msgType)
        {
            //计算MAC用到的数据
            byte[] countMacData = EncryptWT.countMacData(msgType, EncryptWT.encryptLKL(data, null));
            Console.WriteLine(BCDUtil.byteArrToString(countMacData));
            string str = MacUtils.CSMMAC(mackey, BCDUtil.byteArrToString(countMacData));//SysConfigHelper.readerNode("WTMacKey")
            Console.WriteLine("bcd2Str:" + PosProtocol.bcd2Str(System.Text.Encoding.Default.GetBytes(str.Substring(0, 8))));
            //MAC转byte[]
            String strMac = PosProtocol.bcd2Str(System.Text.Encoding.Default.GetBytes(str.Substring(0, 8).ToUpper()));
            byte[] macByte = BCDUtil.ToByteArray(strMac);
            return macByte;
        }
        /// <summary>
        /// 获取8583报文格式的时间
        /// </summary>
        /// <returns></returns>
        public static string getMMDDHHMMSSTime()
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
        public static string getMMDD()
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
        public static string TimeStrLenZero(string str)
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
        public static Dictionary<string, ResultData> signOut()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(3, "000000");
            data.Add(11, "000006");
            data.Add(12, getHHMMSS());
            data.Add(41, "pos00001");
            data.Add(42, "000000000000257");
            data.Add(60, "22000003000");
            data.Add(63, "01");
            byte[] sendData = EncryptWT.EncryptData("0820", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0400")));
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);
            if (result.Length > 0)
            {
                return lk.analysis(result); //发送数据
            }
            else
            {
                return null;
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
            for (int i = 0; i < 32; i++)
            {
                if (cardNo.Length < 32)
                {
                    cardNo = "0" + cardNo;
                }
                if (pwd.Length < 32)
                {
                    pwd = pwd + "f";
                }
            }

            string orx = MacUtils.xOrString(cardNo, pwd);
            //Console.WriteLine("orx="+ orx);
            result = BCDUtil.byteArrToString(CSm4.EncriptB(BCDUtil.ToByteArray(pinKey), BCDUtil.ToByteArray(orx)));//BCDUtil.ToByteArray(SysConfigHelper.readerNode("WTPinKey"))
            // Console.WriteLine("result="+ result);
            Console.WriteLine(result.ToUpper());
            return result.ToUpper();
        }
        /// <summary>
        /// 打印数据到控制的 同时重要功能：如果返回80 更新pinKey 与 macKey
        /// </summary>
        /// <param name="data">传入服务器返回byte数组</param>
        /// <returns>返回信息为系统返回信息</returns>
        public static string handleDataMac(Dictionary<string, ResultData> rd)
        {
            if (rd == null)
            {
                return "系统无返回数据！！";
            }
            if (rd.ContainsKey("62"))
            {
                ResultData _62 = rd["62"];
                //获取44域中的密文密钥
                string mackeyExpress = _62.value.Substring(_62.value.Length - 40, 40);
                //将密文密钥解析明文密钥
                //SysBLL.SetAppConfig("WTMacKey", validateKey(mackeyExpress, sonKey));
                mackey = validateKey(mackeyExpress, sonKey);
                //Console.WriteLine("mackey=" + mackey);
                string pinKeyExpress = _62.value.Substring(0, 40);
                //Console.WriteLine("加密key:" + sonKey);
                //SysBLL.SetAppConfig("WTPinKey", validateKey(pinKeyExpress, sonKey));
                pinKey = validateKey(pinKeyExpress, sonKey);
                //Console.WriteLine("加密后pinKey:" + pinKey);
            }
            foreach (var item in rd)
            {
                Console.WriteLine(item.Key + "-----" + item.Value.name + ":::::" + item.Value.value);
            }
            return rd["39"].value;
        }

        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="msgNo"></param>
        /// <returns></returns>
        public static string handleDataMsg(string msgNo)
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
        /// <summary>
        /// ZMK密钥下载
        /// </summary>
        /// <returns></returns>
        public static void downKey()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(3, "060000");
            data.Add(11, "000002");
            data.Add(41, "JC000021");
            data.Add(42, "000000000000106");
            data.Add(62, "83D7BB2A87F20D7B0F60A4B45E6FD36A");
            data.Add(63, "01");
            AnalysisBaseWT wt = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, EncryptWT.EncryptData("0800", "6000000000", EncryptWT.encryptLKL(data, null))); //发送数据
            Dictionary<string, ResultData> rd = wt.analysis(result);
            sonKey = validateKey("7f7518be4f02736f52f357d5b5ca63e786e1ffdd", matherKey);
            printInfo(rd);
        }
        /// <summary>
        /// 验证数据是否通过验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string validateKey(string data, string key)
        {
            string validata = data.Substring(data.Length - 8, 8).ToUpper();
            byte[] sources = BCDUtil.HexStrToByteArray(data.Substring(0, 32));
            byte[] desKeys = BCDUtil.HexStrToByteArray(key);
            byte[] encSource = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] encKeys = CSm4.DecriptB(desKeys, sources);
            if (PosProtocol.bytesToHexString(CSm4.EncriptB(encKeys, encSource)).Substring(0, 8).Equals(validata))
            {
                Console.WriteLine("密钥验证通过返回灌输密钥！！！！");
                return PosProtocol.bytesToHexString(encKeys);
            }
            return "";
        }

        public static void printInfo(Dictionary<string, ResultData> rd)
        {
            foreach (var item in rd)
            {
                Console.WriteLine(item.Key + "-----" + item.Value.name + ":::::" + item.Value.value);
            }
        }
    }
}
