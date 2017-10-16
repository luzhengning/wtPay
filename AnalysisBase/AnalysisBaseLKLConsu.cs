using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisBase
{
    public class AnalysisBaseLKLConsu : AnalysisBaseLKL
    {
        public new ResultData analysisBit55(byte[] dataStr, ResultData rd)
        {
            int len = 2;
            int datalength = Convert.ToInt32(PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, len)));
            rd.pos += len;
            //如果长度大于 已知位数，则截取掉最后多余的位数
            string data = BCDUtil.byteArrToString(PosProtocol.subbyte(dataStr, rd.pos, datalength));
            rd.pos += datalength;
            rd.value = data;
            rd.name = "analysisBit55";
            return new ResultData(rd);
        }
        public new ResultData analysisBit58(byte[] dataStr, ResultData rd)
        {
            int len = 3;
            int cardNolength = Convert.ToInt32(PosProtocol.toAsciiStr(PosProtocol.subbyte(dataStr, rd.pos, len)));
            rd.pos += len;

            //如果长度大于 已知位数，则截取掉最后多余的位数
            string cardNo = PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, cardNolength));
            // cardNo = Convert.ToString(Convert.ToInt64(cardNo), 16);
            rd.pos += cardNolength;
            rd.value = cardNo;
            rd.name = "电子钱包标准的交易信息";
            return new ResultData(rd);
        }

        public  Dictionary<string, ResultData> analysisIC(byte[] b)
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
                        Console.WriteLine(method.Name + ":" + e.value + "\n");
                        list.Add("" + (i + 1), e);
                    }
                }
                //method.Invoke(instance, null);
                //TODO 
            }

            return list;
        }
    }
}
