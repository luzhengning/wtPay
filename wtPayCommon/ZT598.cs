using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wtPayCommon
{
    public class ZT598
    {
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="OpenParm"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_OpenDevice")]//打开端口
        public static extern int TT_OpenDevice(StringBuilder Port, StringBuilder OpenParm, StringBuilder szMsg);
        /// <summary>
        /// 激活键盘
        /// </summary>
        /// <param name="workmode"></param>
        /// <param name="AutoInput"></param>
        /// <param name="timeout">秒</param>
        /// <param name="maxlen"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_EnableKeyboard")]//打开端口
        public static extern int TT_EnableKeyboard(int workmode, int AutoInput, int timeout, int maxlen, StringBuilder szMsg);
        /// <summary>
        /// 获取界面显示输入
        /// </summary>
        /// <param name="input"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_GetInput")]//打开端口
        public static extern int TT_GetInput(StringBuilder input, StringBuilder szMsg);
        /// <summary>
        /// 设置卡号
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_SetCardNo")]//打开端口
        public static extern int TT_SetCardNo(StringBuilder cardNo, StringBuilder szMsg);
        /// <summary>
        /// 获取计算的Pin
        /// </summary>
        /// <param name="outData"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_GetPin")]//打开端口
        public static extern int TT_GetPin(StringBuilder outData, StringBuilder szMsg);

        /// <summary>
        /// 下载主密钥
        /// </summary>
        /// <param name="Key">密钥</param>
        /// <param name="midx">主密钥ID</param>
        
        /// <param name="szMsg"></param>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_LoadMasterKey")]//打开端口
        public static extern int TT_LoadMasterKey(StringBuilder Key, int midx,  StringBuilder szMsg);




        /// <summary>
        /// 设置工作密钥
        /// </summary>
        /// <param name="Key">密钥</param>
        /// <param name="midx">主密钥ID</param>
        /// <param name="kidx">当前密钥ID</param>
        /// <param name="szMsg"></param>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_LoadWorkKey")]//打开端口
        public static extern int TT_LoadWorkKey(StringBuilder Key, int midx, int kidx, StringBuilder szMsg);
        /// <summary>
        /// 激活工作密钥 只用激活密码pin
        /// </summary>
        /// <param name="midx"></param>
        /// <param name="kidx"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_ActivateWorkKey")]//打开端口
        public static extern int TT_ActivateWorkKey(int midx, int kidx, StringBuilder szMsg);
        /// <summary>
        /// 获取mac DES 算法
        /// </summary>
        /// <param name="MacType">拉卡拉为1，代表DES算法；万通为2，代表SM4</param>
        /// <param name=""></param>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_MAC")]//打开端口
        public static extern int TT_MAC(byte MacType, byte[] macBlock, int macBlockLen, byte[] mac, StringBuilder szMsg);
        /// <summary>
        /// 设置加密模式
        /// </summary>
        /// <param name="nMode">0：des加密,1：sm4加密</param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_SetCryptMode")]//打开端口
        public static extern int TT_SetCryptMode(int nMode, StringBuilder szMsg);

        [DllImport("TTKeyboard.dll", EntryPoint = "TT_GetDeviceStatus")]//检测应用状态
        public static extern int TT_GetDeviceStatus(StringBuilder szMsg);

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("TTKeyboard.dll", EntryPoint = "TT_DisableKeyboard")]
        public static extern int TT_DisableKeyboard(StringBuilder info);

    }
}
