using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace wtPayCommon
{
    /// <summary>
    /// 用来读取燃气卡：先锋卡、金卡
    /// </summary>
    public class CJ201
    {
        public static int handle;
        //打开端口
        [DllImport("CJ201api.dll")]
        public static extern int Open_Com(int port, int baud, int data, int parity, int stop);

        //关闭端口
        [DllImport("CJ201api.dll")]
        public static extern int Close_Com(int Devfd);

        //选择Memory卡类型
        [DllImport("CJ201api.dll")]
        public static extern int select_CardType(int Devfd, int Card_Id);

        //上电命令
        [DllImport("CJ201api.dll")]
        public static extern int power_on(int Devfd, int wait_time);

        //读IC卡函数
        [DllImport("CJ201api.dll")]
        public static extern int read_icc(int Devfd, int rd_addr, int rd_len, ref string rd_buf);

        //写IC卡函数
        [DllImport("CJ201api.dll")]
        public static extern int write_icc(int Devfd, int wr_addr, int wr_len, ref string wr_buf);

        //下电函数
        [DllImport("CJ201api.dll")]
        public static extern int exit_icc(int Devfd);

        //校验密码函数
        [DllImport("CJ201api.dll")]
        public static extern int comp_icc(int Devfd, ref string comp_sc);

        //更改密码函数
        [DllImport("CJ201api.dll")]
        public static extern int chan_icc(int Devfd, ref string chan_sc);

        /// <summary>
        /// 金卡读卡函数
        /// icdev Open_Com返回的设备句柄
        /// vskh 用户卡号
        /// vlql 卡中气量
        /// vlzyql 总用气量
        /// lpInfo 出错信息，最长不超过50字节
        /// 返回0正确，其他错误。
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="vskh"></param>
        /// <param name="vlql"></param>
        /// <param name="vlzyql"></param>
        /// <param name="lpInfo"></param>
        /// <returns></returns>
        [DllImport("goldcard_zz.dll")]
        public static extern int GoldCard_Read_zz(int icdev, StringBuilder vskh, ref int vlql, ref int vlzyql, StringBuilder lpInfo);

        /// <summary>
        /// 金卡写卡函数
        /// icdev Open_Com返回的设备句柄
        /// vskh 用户卡号
        /// vlql 气量
        /// lpInfo 出错信息，最长不超过50字节
        /// 返回0正确，其他错误。
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="vskh"></param>
        /// <param name="vlql"></param>
        /// <param name="lpInfo"></param>
        /// <returns></returns>
        [DllImport("goldcard_zz.dll")]
        public static extern int GoldCard_Write_zz(int icdev, StringBuilder vskh, int vlql1, StringBuilder lpInfo);

        /// <summary>
        /// 读先锋卡函数
        /// userid:用户编号
        /// gasvalue:气量（金额）
        /// port:串口号  1--4
        /// jebz:1为金额卡
        /// times:次数，气量卡则为0
        /// </summary>
        /// <param name="port"></param>
        /// <param name="userid"></param>
        /// <param name="gasvalue"></param>
        /// <param name="jebz"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        [DllImport("lzdll.dll")]
        public static extern int read_card_lz2(int port, StringBuilder userid, ref double gasvalue, ref int jebz, ref int times);

        /// <summary>
        /// 写先锋卡函数
        /// userid:用户编号
        /// gasvalue:气量（金额）
        /// port:串口号  1--4
        /// jebz:1为金额卡
        /// times:次数，气量卡则为0
        /// </summary>
        /// <param name="port"></param>
        /// <param name="userid"></param>
        /// <param name="gasvalue"></param>
        /// <param name="jebz"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        [DllImport("lzdll.dll")]
        public static extern int write_card_lz2(int port, StringBuilder userid, double gasvalue, int times);

    }
}
