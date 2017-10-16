using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace wtPayCommon
{
    public class DLLRdetmx
    {
        //打开端口
        [DllImport("wyzh.ocx")]
        public static extern int WF000(int port, StringBuilder baud, StringBuilder data);

        
    }
}
