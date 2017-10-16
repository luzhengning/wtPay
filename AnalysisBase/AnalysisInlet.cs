using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisBase
{
    /// <summary>
    /// 解析入口
    /// </summary>
  public abstract class AnalysisInlet
    {
        /// <summary>
        /// 获取报头
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public abstract ResultData analysisHeader(byte[] dataStr, ResultData rd);
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public abstract Dictionary<string, ResultData> analysis(byte[] b);
        /// <summary>
        /// 获取analysisTPDU
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public abstract ResultData analysisTPDU(byte[] dataStr, ResultData rd);



        /// <summary>
        /// 获取消息类型
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public abstract ResultData analysisMsgType(byte[] dataStr, ResultData rd);
        public abstract ResultData analysisBitMap(byte[] dataStr, ResultData rd);
        /// <summary>
        /// 子类实现的方法
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="v"></param>
        /// <param name="dataStr"></param>
        public abstract void fixedRightBcd(ResultData rd, int v, byte[] dataStr);
        /// <summary>
        /// 子类实现的方法
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="rd"></param>
        /// <param name="v"></param>
        public abstract void analysisBit(byte[] dataStr, ResultData rd, int v);

    }
}
