using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisBase
{
   public class AnalysisBaseLKLSign:AnalysisBaseLKL
    {
        public new ResultData analysisBit57(byte[] dataStr, ResultData rd)
        {
            int len = 2;
            int datalength = Convert.ToInt32(PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, len)));
            rd.pos += len;
            //如果长度大于 已知位数，则截取掉最后多余的位数
            string data = PosProtocol.toAsciiStr(PosProtocol.subbyte(dataStr, rd.pos, datalength));
            rd.pos += datalength;
            rd.value = datalength + "," + data;
            rd.name = "analysisBit57";
            return new ResultData(rd);
        }

       public new ResultData analysisBit44(byte[] dataStr, ResultData rd)
        {
            int len = 1;
            int datalength = Convert.ToInt32(PosProtocol.bcd2Str(PosProtocol.subbyte(dataStr, rd.pos, len)));
            rd.pos += len;
            //如果长度大于 已知位数，则截取掉最后多余的位数
            string data = PosProtocol.bytesToHexString(PosProtocol.subbyte(dataStr, rd.pos, datalength));
            rd.pos += datalength;
            rd.value = data;
            return new ResultData(rd);
        }
        
       
    }
}
