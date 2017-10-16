using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisBase
{
    public abstract class AnalysisBaseLKL : AnalysisInlet
    {
        private string sub(string s, int dest)
        {
            if (s.Length != dest)
            {
                return s.Substring(0, dest);
            }
            return s;
        }
        /// <summary>
        /// 获取报头
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public override ResultData analysisHeader(byte[] dataStr, ResultData rd)
        {
            rd.value = Convert.ToInt64(PosProtocol.bytesToHexString(PosProtocol.subbyte(dataStr, 0, rd.pos)), 16).ToString();
            rd.name = "Header";
            return new ResultData(rd);
        }
        /// <summary>
        /// 获取analysisTPDU
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public override ResultData analysisTPDU(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 5, dataStr);
            rd.name = "TPDU";
            return new ResultData(rd);
        }
        /// <summary>
        /// 获取消息类型
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public override ResultData analysisMsgType(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 2, dataStr);
            rd.name = "MsgType";
            return new ResultData(rd);
        }
        public override ResultData analysisBitMap(byte[] dataStr, ResultData rd)
        {
            analysisBit(dataStr, rd, 8);
            rd.name = "BitMap";
            return new ResultData(rd);
        }
        public ResultData analysisBit1(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit1";
            return new ResultData(rd);
        }
        public ResultData analysisBit2(byte[] dataStr, ResultData rd)
        {
            varRightBcd(dataStr, rd, 1);
            rd.name = "卡号";
            return new ResultData(rd);
        }
        public ResultData analysisBit3(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 3, dataStr);
            rd.name = "交易处理码";
            return new ResultData(rd);
        }
        public ResultData analysisBit4(byte[] dataStr, ResultData rd)
        {

            fixedRightBcd(rd, 6, dataStr);
            rd.name = "交易金额";
            return new ResultData(rd);
        }
        public ResultData analysisBit5(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 1, dataStr);
            rd.name = "IC卡类型";
            return new ResultData(rd);
        }
        public ResultData analysisBit6(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit6";
            return new ResultData(rd);
        }
        public ResultData analysisBit7(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 5, dataStr);
            rd.name = "analysisBit7";
            return new ResultData(rd);
        }
        public ResultData analysisBit8(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit8";
            return new ResultData(rd);
        }
        public ResultData analysisBit9(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit9";
            return new ResultData(rd);
        }
        public ResultData analysisBit10(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit10";
            return new ResultData(rd);
        }
        public ResultData analysisBit11(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 3, dataStr);
            rd.name = "pos流水号";
            return new ResultData(rd);
        }
        public ResultData analysisBit12(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 3, dataStr);
            rd.name = "受卡方所在地时间";
            return new ResultData(rd);
        }
        public ResultData analysisBit13(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 2, dataStr);
            rd.name = "受卡方所在地日期";
            return new ResultData(rd);
        }
        public ResultData analysisBit14(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 2, dataStr);
            rd.name = "analysisBit14";
            return new ResultData(rd);
        }
        public ResultData analysisBit15(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 2, dataStr);
            rd.name = "清算日期";
            return new ResultData(rd);
        }
        public ResultData analysisBit16(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit16";
            return new ResultData(rd);
        }
        public ResultData analysisBit17(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit17";
            return new ResultData(rd);
        }
        public ResultData analysisBit18(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit18";
            return new ResultData(rd);
        }
        public ResultData analysisBit19(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit19";
            return new ResultData(rd);
        }
        public ResultData analysisBit20(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit20";
            return new ResultData(rd);
        }
        public ResultData analysisBit21(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit21";
            return new ResultData(rd);
        }
        public ResultData analysisBit22(byte[] dataStr, ResultData rd)
        {

            fixedRightBcd(rd, 2, dataStr);
            rd.name = "服务点输入方式码，手工，磁条之类";
            return new ResultData(rd);
        }
        public ResultData analysisBit23(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 2, dataStr);
            rd.name = "analysisBit23";
            return new ResultData(rd);
        }
        public ResultData analysisBit24(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit24";
            return new ResultData(rd);
        }
        public ResultData analysisBit25(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 1, dataStr);
            rd.name = "服务点条件码";
            return new ResultData(rd);
        }
        public ResultData analysisBit26(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 1, dataStr);
            rd.name = "服务点PIN获取码";
            return new ResultData(rd);
        }
        public ResultData analysisBit27(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit27";
            return new ResultData(rd);
        }
        public ResultData analysisBit28(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 1, dataStr);
            rd.name = "analysisBit28";
            return new ResultData(rd);
        }
        public ResultData analysisBit29(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit29";
            return new ResultData(rd);
        }
        public ResultData analysisBit30(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit30";
            return new ResultData(rd);
        }
        public ResultData analysisBit31(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit31";
            return new ResultData(rd);
        }
        public ResultData analysisBit32(byte[] dataStr, ResultData rd)
        {
            varRightBcd(dataStr, rd, 1);
            rd.name = "受理方标识码，认可pos机具";
            return new ResultData(rd);
        }
        public ResultData analysisBit33(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit33";
            return new ResultData(rd);
        }
        public ResultData analysisBit34(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit34";
            return new ResultData(rd);
        }
        public ResultData analysisBit35(byte[] dataStr, ResultData rd)
        {
            varRightBcd(dataStr, rd, 1);

            rd.name = "2磁道数据";
            return new ResultData(rd);
        }
        public ResultData analysisBit36(byte[] dataStr, ResultData rd)
        {
            varRightBcd(dataStr, rd, 2);
            rd.name = "3磁道数据";
            return new ResultData(rd);
        }
        public ResultData analysisBit37(byte[] dataStr, ResultData rd)
        {
            fixedAsciiStr(rd, 12, dataStr);
            rd.name = "检索参考号";
            return new ResultData(rd);
        }
        public ResultData analysisBit38(byte[] dataStr, ResultData rd)
        {
            fixedAsciiStr(rd, 6, dataStr);
            rd.name = "授权标识应答码";
            return new ResultData(rd);
        }
        public ResultData analysisBit39(byte[] dataStr, ResultData rd)
        {
            fixedAsciiStr(rd, 2, dataStr);
            rd.name = "应答码";
            return new ResultData(rd);
        }
        public ResultData analysisBit40(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit40";
            return new ResultData(rd);
        }
        public ResultData analysisBit41(byte[] dataStr, ResultData rd)
        {

            fixedAsciiStr(rd, 8, dataStr);
            rd.name = "受卡机终端标识码";
            return new ResultData(rd);
        }
        public ResultData analysisBit42(byte[] dataStr, ResultData rd)
        {
            fixedAsciiStr(rd, 15, dataStr);
            rd.name = "商户代码";
            return new ResultData(rd);
        }
        public ResultData analysisBit43(byte[] dataStr, ResultData rd)
        {
            fixedAsciiStr(rd, 40, dataStr);
            rd.name = "analysisBit43";
            return new ResultData(rd);
        }
        public ResultData analysisBit44(byte[] dataStr, ResultData rd)
        {
            varAnalysisBit(dataStr, rd, 1);
            rd.name = "analysisBit44";
            return new ResultData(rd);
        }
        public ResultData analysisBit45(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit45";
            return new ResultData(rd);
        }
        public ResultData analysisBit46(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit46";
            return new ResultData(rd);
        }
        public ResultData analysisBit47(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit47";
            return new ResultData(rd);
        }
        public ResultData analysisBit48(byte[] dataStr, ResultData rd)
        {
            varAsciiStr(dataStr, rd, 2);
            rd.name = "私有自定义";
            return new ResultData(rd);
        }
        public ResultData analysisBit49(byte[] dataStr, ResultData rd)
        {
            fixedAsciiStr(rd, 3, dataStr);
            rd.name = "交易货币代码";
            return new ResultData(rd);
        }
        public ResultData analysisBit50(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit50";
            return new ResultData(rd);
        }
        public ResultData analysisBit51(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit51";
            return new ResultData(rd);
        }
        public ResultData analysisBit52(byte[] dataStr, ResultData rd)
        {
            analysisBit(dataStr, rd, 8);
            rd.name = "密码PIN";
            return new ResultData(rd);
        }
        public ResultData analysisBit53(byte[] dataStr, ResultData rd)
        {
            fixedRightBcd(rd, 8, dataStr);
            rd.name = "analysisBit53";
            return new ResultData(rd);
        }
        public ResultData analysisBit54(byte[] dataStr, ResultData rd)
        {
            varAsciiStr(dataStr, rd, 2);
            PosProtocol.rightBcdComplete(rd, 3);
            rd.name = "附加金额";
            return new ResultData(rd);
        }
        public ResultData analysisBit55(byte[] dataStr, ResultData rd)
        {
            varAsciiStr(dataStr, rd, 2);
            rd.name = "analysisBit55";
            return new ResultData(rd);
        }
        public ResultData analysisBit56(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit56";
            return new ResultData(rd);
        }
        public ResultData analysisBit57(byte[] dataStr, ResultData rd)
        {

            varAsciiStr(dataStr, rd, 2);
            rd.name = "analysisBit57";
            return new ResultData(rd);
        }
        public ResultData analysisBit58(byte[] dataStr, ResultData rd)
        {
            varRightBcd(dataStr, rd, 2);
            PosProtocol.rightBcdComplete(rd, 3);
            rd.name = "电子钱包标准的交易信息";

            return new ResultData(rd);
        }
        public ResultData analysisBit59(byte[] dataStr, ResultData rd)
        {
            rd.name = "analysisBit59";
            varAsciiStr(dataStr, rd, 2);
            return new ResultData(rd);
        }
        public ResultData analysisBit60(byte[] dataStr, ResultData rd)
        {
            varAsciiStr(dataStr, rd, 2);
            rd.name = "自定义域";
            return new ResultData(rd);
        }
        public ResultData analysisBit61(byte[] dataStr, ResultData rd)
        {
            varAsciiStr(dataStr, rd, 2);
            rd.name = "原始信息域";
            return new ResultData(rd);
        }
        public ResultData analysisBit62(byte[] dataStr, ResultData rd)
        {
            varAsciiStr(dataStr, rd, 2);
            rd.name = "自定义域终端密钥";
            return new ResultData(rd);
        }
        public ResultData analysisBit63(byte[] dataStr, ResultData rd)
        {
            varAsciiStr(dataStr, rd, 2);

            rd.name = "自定义域";
            return new ResultData(rd);
        }
        public ResultData analysisBit64(byte[] dataStr, ResultData rd)
        {
            this.analysisBit(dataStr, rd, 8);
            rd.name = "MAC";
            return new ResultData(rd);
        }

        /// <summary>
        /// 固定长度BCD解码
        /// </summary>
        /// <param name="rd"></param>
        public override void fixedRightBcd(ResultData rd, int len, byte[] dataStr)
        {
            rd.value = PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, len));
            rd.pos = rd.pos + len;
        }
        /// <summary>
        /// 固定长度ascii
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="len"></param>
        /// <param name="dataStr"></param>
        public void fixedAsciiStr(ResultData rd, int len, byte[] dataStr)
        {
            rd.value = PosProtocol.toAsciiStr(PosProtocol.subbyte(dataStr, rd.pos, len));
            rd.pos = rd.pos + len;
        }
        /// <summary>
        /// 可变长度BCD
        /// </summary>
        /// <param name="rd"></param>
        public void varRightBcd(byte[] dataStr, ResultData rd, int len)
        {
            int cardNolength = Convert.ToInt32(PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, len)));
            rd.pos += len;
            int bcdcardNolength = PosProtocol.bcdlength(cardNolength);
            //如果长度大于 已知位数，则截取掉最后多余的位数
            string cardNo = sub(PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, bcdcardNolength)), cardNolength);
            // cardNo = Convert.ToString(Convert.ToInt64(cardNo), 16);
            rd.pos += bcdcardNolength;
            rd.value = cardNo;
        }
        /// <summary>
        /// 可变长度ascii
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <param name="len"></param>
        public void varAsciiStr(byte[] dataStr, ResultData rd, int len)
        {

            int datalength = Convert.ToInt32(PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, len)));
            rd.pos += len;
            //如果长度大于 已知位数，则截取掉最后多余的位数
            string data = PosProtocol.toAsciiStr(PosProtocol.subbyte(dataStr, rd.pos, datalength));
            rd.pos += datalength;
            rd.value = data;

        }


        public override void analysisBit(byte[] dataStr, ResultData rd, int len)
        {
            rd.value = PosProtocol.bytesToHexString(PosProtocol.subbyte(dataStr, rd.pos, len));
            rd.pos = rd.pos + len;
        }
        public void varAnalysisBit(byte[] dataStr, ResultData rd, int len)
        {
            int datalength = Convert.ToInt32(PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, len)));
            rd.pos += len;
            //如果长度大于 已知位数，则截取掉最后多余的位数
            string data = PosProtocol.bytesToHexString(PosProtocol.subbyte(dataStr, rd.pos, datalength));
            rd.pos += datalength;
            rd.value = data;
        }

        public override Dictionary<string, ResultData> analysis(byte[] b)
        {
            try
            {
                Dictionary<string, ResultData> list = new Dictionary<string, ResultData>();

                ResultData rd = new ResultData(2);

                list.Add("header", this.analysisHeader(b, rd));
                list.Add("tpdu", this.analysisTPDU(b, rd));
                list.Add("msgType", this.analysisMsgType(b, rd));
                Console.Write(rd.value);
                ResultData rse = this.analysisBitMap(b, rd);
                Int64 bitmap = Convert.ToInt64(rse.value, 16);
                list.Add("map", rse);
                string _bitmap = PosProtocol.leftpad(PosProtocol.ToBinaryString(bitmap));
                Console.WriteLine("\n" + _bitmap);
                for (int i = 0; i < _bitmap.Length; i++)
                {
                    if (_bitmap.Substring(i, 1) == "1")
                    {
                        var method = this.GetType().GetMethod("analysisBit" + (i + 1));
                        object obj = new object();
                        obj = method.Invoke(this, new Object[] { b, rd });


                        if (obj != null)
                        {
                            ResultData e = (ResultData)obj;
                            // Console.WriteLine(method.Name + ":" + e.value + "\n");
                            list.Add("" + (i + 1), e);
                        }
                    }
                    //method.Invoke(instance, null);
                    //TODO 
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;

            }

        }
    }
}
