using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace wtPayCommon
{
    /// <summary>
    /// 电动读卡器：用来读银行卡、万通卡
    /// </summary>
    public class CRT310
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="strVerion">版本信息字符串</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int GetSysVerion(int ComHandle, ref string strVerion);

        /// <summary>
        /// 获取读卡器命令执行失败
        /// </summary>
        /// <param name="Errorcode">错误代码</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int GetErrCode(ref int Errorcode);

        /// <summary>
        /// 打开串口,波特率默认为9600bps
        /// </summary>
        /// <param name="Port">串口号字符串</param>
        /// <returns>返回句柄</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CommOpen(string Port);

        /// <summary>
        /// 按指定的波特率打开串口
        /// </summary>
        /// <param name="Port">串口号字符串</param>
        /// <param name="_BaudOption">// 1=> 波特率为1200
        // 2=> 波特率为2400
        // 3=> 波特率为4800
        // 4=> 波特率为9600
        // 5=> 波特率为19200
        // 6=> 波特率为38400
        // 7=> 波特率为57600</param>
        /// <returns>句柄</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CommOpenWithBaut(string Port, Byte _BaudOption);

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="ComHandle">句柄</param>
        /// <returns>0=成功</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CommClose(int ComHandle);

        /// <summary>
        /// 设置卡机通讯波特率
        /// </summary>
        /// <param name="ComHandle">句柄</param>
        /// <param name="_BaudOption">// 1=> 波特率为1200
        // 2=> 波特率为2400
        // 3=> 波特率为4800
        // 4=> 波特率为9600
        // 5=> 波特率为19200
        // 6=> 波特率为38400
        // 7=> 波特率为57600</param>
        /// <returns>如果函数调用成功，返回值为0</returns>
        /// 注意：成功设置后，需要关闭上位机串口，然后用对应的波特率重新打开上微机串口，否则不能通讯。
        [DllImport("CRT_310.dll")]
        public static extern int CRT_R_SetComm(int ComHandle, Byte _BaudOption);

        /// <summary>
        /// CRT310 复位读卡机。
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="_Eject">_Eject:弹卡选择。 0=不弹卡 1=前端弹卡 2=后端弹卡</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_Reset(int ComHandle, Byte _Eject);

        /// <summary>
        /// 读卡机序列号
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="_SNData">卡机序列号</param>
        /// <param name="dataLen">数据长度。</param>
        /// <returns>如果函数调用成功，返回值为0。数组_SNData 中存放卡机序列号。_ dataLen 中存放卡机序列号数据长度</returns>
        /// CRT310_ReadSnr(HANDLE ComHandle, BYTE _SNData[], BYTE *_dataLen)
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_ReadSnr(int ComHandle, Byte[] _SNData, ref int dataLen);

        /// <summary>
        /// 写卡机序列号
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="_SNData">卡机序列号</param>
        /// <param name="dataLen">数据长度。</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        /// 注：卡机序列号数据包的长度 （0 ~ 1６byte）
        /// 卡序列号数据包：以ＡＳＣＩＩ码存贮：
        /// 如卡机的序列号的数据为 “CRT-310” 则卡机序列号数据包为：“ 0x43,0x52,0x54,0x2D,0x33,0x31,0x30”
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_WriteSnr(int ComHandle, Byte[] _SNData, ref Byte dataLen);

        /// <summary>
        /// CRT310 进卡控制设置。
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="_CardIn">前端进卡设置：</param>
        /// =0x1 不允许；
        ///=0x2 磁卡方式（磁信号+开关同时有效）进卡使能, 只允许磁卡从前端开闸门进卡；
        ///=0x3 开关信号方式进卡使能，允许磁卡，IC 卡，M1 射频卡，双界面卡从前端开闸门进卡。
        ///=0x4 磁信号方式进卡使能, 针对薄磁卡等一些纸卡进卡；
        /// <param name="_EnableBackIn">是否允许后端进卡。 0x0=允许；0x1=不允许。</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_CardSetting(int ComHandle, Byte _CardIn, Byte _EnableBackIn);

        /// <summary>
        /// CRT310 停卡位置设置。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_Position">=0x1 进卡后停卡在前端位置，不持卡。
        ///=0x2 进卡后停卡在前端位置，并持卡。
        ///=0x3 进卡后停卡在卡机内位置，但是IC 卡触点没有与卡接触，M1 射频卡可以进行读写操作。
        ///=0x4 进卡后停卡在卡机内位置，同时将IC 卡座触点与卡接触，直接可进行IC 卡操作。
        ///=0x5 进卡后停卡在后端位置，并持卡。
        ///=0x6 进卡后将卡从后端弹出，不持卡。</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_CardPosition(int ComHandle, Byte _Position);

        /// <summary>
        /// 从读卡机读取状态信息。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_atPosition">=0X46 卡机内有长卡(卡的长度长于标准卡长度)
        /// =0X47 卡机内有短卡(卡的长度短于标准卡长度)
        ///=0X48 卡机前端,不持卡位置有卡。
        ///=0X49 卡机前端持卡位置有卡。
        ///=0X4A 卡机内停卡位置有卡。
        ///=0X4B 卡机内IC 卡操作位置有卡，并且IC 卡触点已下落。
        ///=0X4C 卡机后端持卡位置有卡。
        ///=0X4D 卡机后端不持卡位置有卡。
        ///=0X4E 卡机内无卡。</param>
        /// <param name="_frontSetting">=0X49 卡机允许磁信号方式进卡,只允许磁卡开闸门进卡。
        ///=0X4A 卡机允许开关信号方式进卡，允许磁卡，IC 卡，M1 射频卡，双界面卡进卡。
        ///=0X4B 卡机允许磁信号方式进卡，允许纸磁卡，薄卡进卡。
        ///=0X4E 卡机禁止进卡。</param>
        /// <param name="_rearSetting">=0X4A 卡机允许后端进卡，允许磁卡，IC 卡，M1 射频卡，双面卡进卡。
        ///=0X4E 卡机禁止后端进卡。</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_GetStatus(int ComHandle, ref Byte _atPosition, ref Byte _frontSetting, ref Byte _rearSetting);

        /// <summary>
        /// 读 CRT310 感应器状态
        /// </summary>
        /// <param name="ComHandle"></param>
        /// <param name="_PSS0">_PSS0—PSS5,红外传感器状态：</param>
        /// <param name="_PSS1">PSSx=0X30 表示此传感器位置上未探测到卡片；</param>
        /// <param name="_PSS2">PSSx=0X31 表示探测到有卡片。</param>
        /// <param name="_PSS3"></param>
        /// <param name="_PSS4"></param>
        /// <param name="_PSS5"></param>
        /// <param name="_CTSW">闸门状态信息,CTSW=0X30 表示闸门已关闭；CTSW=0X31 表示闸门已打开。</param>
        /// <param name="_KSW">开关进卡传感受器状态.KSW=0X30 表示开关没有检测到卡片插入闸门信号；KSW=0X31 表示开关检测到有卡片插入闸门。</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_SensorStatus(int ComHandle, ref Byte _PSS0, ref Byte _PSS1, ref Byte _PSS2, ref Byte _PSS3, ref Byte _PSS4, ref Byte _PSS5, ref Byte _CTSW, ref Byte _KSW);

        /// <summary>
        /// CRT310 走卡位置设置。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_Position">=0x1 将卡重新走位到前端位置，不持卡。
        ///=0x2 将卡重新走位到前端位置，并持卡。
        ///=0x3 将卡重新走位到卡机内位置,操作成功后可进行M1 射频卡操作。
        ///=0x4 将卡重新走位到卡机内位置，并将IC 卡触点落下，操作成功后可进行接触式IC 卡操作。
        ///=0x5 将卡重新走位到后端位置，并持卡。
        ///=0x6 将卡重新走位后端位置，不持卡。
        ///=0x7 将异常长度卡（短卡，长卡）清出卡机内，将卡向后端弹卡，对于短卡还需人工在卡口插正常卡辅助操作.
        ///=0x8 启动清洁卡操作</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_MovePosition(int ComHandle, Byte _Position);

        /// <summary>
        /// 串口句柄。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_ONOFF">=0x0 亮指示灯,=0x1 灭指示灯</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_LEDSet(int ComHandle, Byte _ONOFF);

        /// <summary>
        /// CRT310 指示灯亮/灭时间设置。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_T1">亮指示灯时间值（_T1 值为0x00-0xFF，时间值为0.25 秒 X _T1）</param>
        /// <param name="_T2">灭指示灯时间值（_T2 值为0x00-0xFF，时间值为0.25 秒 X _T2）</param>
        /// <returns>如果函数调用成功，返回值为0。</returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT310_LEDTime(int ComHandle, Byte _T1, Byte _T2);

        /// <summary>
        /// 读磁轨数据。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_mode">数据模式。读卡模式： 0x30 以ASCII 码读卡数据;0x31 以二进制码读卡数据</param>
        /// <param name="_track">磁轨。指定轨道号： 0x30 磁卡三轨都不读
        /// 0x31 读磁卡一轨
        ///0x32 读磁卡二轨
        ///0x33 读磁卡三轨
        ///0x34 读磁卡一二轨
        ///0x35 读磁卡二三轨
        ///0x36 读磁卡一三轨
        ///0x37 读磁卡一二三轨</param>
        /// <param name="_BlockDataLen">数据包长度。</param>
        /// <param name="_BlockData">数据内容。pdf,第10页</param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int MC_ReadTrack(int ComHandle, Byte _mode, Byte _track, ref int _BlockDataLen, byte[] _BlockData);

        /// <summary>
        /// 检测是否是M1 卡片。
        /// </summary>
        /// <param name="ComHandle"></param>
        /// 如果函数调用失败，返回值不为0。
        ///P=‘N’（0X4E） 寻卡不成功
        ///P =‘E’（0X45） 卡机内无卡
        ///P =‘W’（0X57） 卡不在允许操作的位置上。
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int RF_DetectCard(int ComHandle);

        /// <summary>
        /// 获取M1 卡片的序列号。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_CardIDLen">序列号长度</param>
        /// <param name="_CardID">卡片号码</param>
        /// <returns>如果函数调用失败，返回值不为0。
        ///P=‘N’（0X4E） 获取卡序列号失败，并返回空序列号（0X00，0X00，0X00，0X00）
        ///P=‘E’（0X45） 卡机内无卡</returns>
        [DllImport("CRT_310.dll")]
        public static extern int RF_GetCardID(int ComHandle, ref Byte _CardIDLen, Byte[] _CardID);

        /// <summary>
        /// CPU 卡复位。
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="_CPUMode">复位模式</param>
        /// <param name="_CPUType">返回的CPU 卡类型</param>
        /// <param name="_exData">返回数据</param>
        /// <param name="_exdataLen">返回数据的长度。</param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int CPU_ColdReset(int ComHandle, byte _CPUMode, ref byte _CPUType, byte[] _exData, ref int _exdataLen);

        /// <summary>
        /// CPU 卡热复位。
        /// </summary>
        /// <param name="ComHandle">串口句柄。</param>
        /// <param name="_CPUType">返回的CPU 卡类型</param>
        /// <param name="_exData">返回数据。</param>
        /// <param name="_exdataLen">返回数据的长度。</param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int CPU_WarmReset(int ComHandle, ref byte _CPUType, ref byte[] _exData, ref int _exdataLen);

        /// <summary>
        /// CPU(T=0)卡C-APDU 命令。
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="_dataLen">C-APDU 命令长度。</param>
        /// <param name="_APDUData">CUP 卡C-APDU 命令。</param>
        /// <param name="_exData">返回数据</param>
        /// <param name="_exdataLen">返回数据的长度。</param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int CPU_T0_C_APDU(int ComHandle, int _dataLen, byte[] _APDUData, byte[] _exData, ref int _exdataLen);

        /// <summary>
        /// CPU(T=1)卡C-APDU 命令。
        /// </summary>
        /// <param name="ComHandle">串口句柄</param>
        /// <param name="_dataLen">C-APDU 命令长度。</param>
        /// <param name="_APDUData">CUP 卡C-APDU 命令。</param>
        /// <param name="_exData">返回数据。</param>
        /// <param name="_exdataLen">返回数据的长度。</param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int CPU_T1_C_APDU(int ComHandle, int _dataLen, byte[] _APDUData, ref byte[] _exData, ref int _exdataLen);

        /// <summary>
        /// IC卡上电
        /// </summary>
        /// <param name="ComHandle"></param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT_IC_CardOpen(int ComHandle);

        /// <summary>
        /// IC卡下电
        /// </summary>
        /// <param name="ComHandle"></param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT_IC_CloseCard(int ComHandle);


        /// <summary>
        /// 检测卡的类型
        /// </summary>
        /// <param name="ComHandle"></param>
        /// <param name="_CardType"></param>
        /// <param name="_CardInfor"></param>
        /// <returns></returns>
        [DllImport("CRT_310.dll")]
        public static extern int CRT_R_DetectCard(int ComHandle, ref byte _CardType, ref byte _CardInfor);

        /// <summary>
        /// 打开设备端口
        /// </summary>
        /// <param name="Port">输入端口号</param>
        /// <param name="OpenParm">波特率</param>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_OpenDevice(StringBuilder Port, StringBuilder OpenParm, StringBuilder szMsg);

        /// <summary>
        /// 关闭设备端口
        /// </summary>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_CloseDevice(StringBuilder szMsg);

        /// <summary>
        /// 取设备状态
        /// </summary>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0：设备正常，准备就绪</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_GetDeviceStatus(StringBuilder szMsg);

        /// <summary>
        /// 获取动态库版本
        /// </summary>
        /// <param name="DllEdition">返回DLL版本</param>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0：设置成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_GetDllEdition(StringBuilder DllEdition, StringBuilder szMsg);

        /// <summary>
        /// 等待用户插卡
        /// </summary>
        /// <param name="timeout">超过此时间后，自动关闭读卡器,0－不限制超时时间</param>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_WaitCard(int timeout, StringBuilder szMsg);

        /// <summary>
        /// 读磁卡数据
        /// </summary>
        /// <param name="pData">返回存储读取的内容，磁道内容</param>
        /// <param name="TrackNo">需读取的磁道，值为1－7
        /// 1代表读1磁道；
        ///2代表读2磁道；
        ///3代表读3磁道；
        ///4代表读1，2，3磁道，各磁道数据用“|”分隔
        ///</param>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_GetTrackData(byte[] pData, int TrackNo, StringBuilder szMsg);

        /// <summary>
        /// 弹卡
        /// </summary>
        /// <param name="Position">弹卡方向
        /// 0-前弹卡
        /// 1－后端弹卡
        /// 2－前持卡
        /// 3－后持卡
        /// </param>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_EjectCard(int Position, StringBuilder szMsg);

        /// <summary>
        /// 取消等待用户插卡
        /// </summary>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_CancelWait(StringBuilder szMsg);

        /// <summary>
        /// 设置卡类型
        /// </summary>
        /// <param name="Mode">0：磁卡 1：所有卡</param>
        /// <param name="szMsg">返回信息</param>
        /// <returns></returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_SetCardMode(int Mode, StringBuilder szMsg);

        /// <summary>
        /// CPU卡设置卡座
        /// </summary>
        /// <param name="CardSlotNo">卡编号：大卡：0；主板上的SAM 卡：1；SAM 卡板上的SAM 卡 依次为：2、3、4、5</param>
        /// <param name="szMsg">返回信息</param>
        /// <returns>0成功，其它值失败</returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_CPUSetCardSlotNo(int CardSlotNo, StringBuilder szMsg);

        /// <summary>
        /// CPU卡上/下电
        /// </summary>
        /// <param name="PowerType">1：上电  0：下电</param>
        /// <param name="szMsg">返回信息</param>
        /// <returns></returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_CPUPowerOnOrDown(int PowerType, StringBuilder szMsg);

        /// <summary>
        /// CPU卡数据操作
        /// </summary>
        /// <param name="pszSendData">发送的数据</param>
        /// <param name="nSendLen">发送数据的长度</param>
        /// <param name="pszReadData">返回的数据</param>
        /// <param name="dwRead">返回的数据长度</param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_CPUSendAPDU(byte[] pszSendData, int nSendLen, byte[] pszReadData, ref int dwRead, StringBuilder szMsg);



        [DllImport("TTReaderCardCZ.dll")]
        public static extern int TT_CPUGetICCardInfo(Byte[] outDate, StringBuilder szMsg);


        /// <summary>
        /// 万通卡卡号解密
        /// </summary>
        /// <param name="cardNoSource"></param>
        /// <param name="cardNo"></param>
        /// <param name="appgen"></param>
        /// <returns></returns>
        [DllImport("swsmk.dll")]
        public static extern int F005(byte[] cardNoSource, StringBuilder cardNo, byte[] appgen);


    
      
    }
}
