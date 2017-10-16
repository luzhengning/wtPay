using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ConfigModel
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class ConfigSysParam
    {
        /// <summary>
        /// 小区编号
        /// </summary>
        public static string ResidentialNo = null;

        public static GifBusiness gifBusiness = new GifBusiness();
    }
    public enum GifBusiness
    {
        no=-1,
        prop2Water_IC = 0,
        prop2Water_RFID = 1,
        prop2Elec_IC = 2,
        prop2Elec_RFID=3,
        wintop_RFID=4,
        gas_IC=5,
    }
    public class ConfigSysUtils
    {
        public static string getReadCardGif()
        {
            switch (ConfigSysParam.gifBusiness)
            {
                case GifBusiness.prop2Water_IC:
                    return "\\sysImage\\GIF\\waterGIF\\IC\\payGIF.gif";
                case GifBusiness.prop2Water_RFID:
                    return "\\sysImage\\GIF\\waterGIF\\RFID\\payGIF.gif";
                case GifBusiness.prop2Elec_IC:
                    return "\\sysImage\\GIF\\elecGIF\\IC\\payGIF.gif";
                case GifBusiness.prop2Elec_RFID:
                    return "\\sysImage\\GIF\\elecGIF\\RFID\\payGIF.gif";
                case GifBusiness.wintop_RFID:
                    return "\\sysImage\\GIF\\wintopGIF\\payGIF.gif";
                case GifBusiness.gas_IC:
                    return "\\sysImage\\GIF\\gasGIF\\xianfengGIF\\payGIF.gif";
            }
            return null;
        }
    }
}
