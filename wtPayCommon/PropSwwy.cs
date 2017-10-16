using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace wtPayCommon
{
    public class PropSwwy
    {
        [DllImport("swwydll.dll", EntryPoint = "WF001", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr WF001(StringBuilder prm_str1, StringBuilder prm_str2, StringBuilder prm_str3, StringBuilder prm_str4, StringBuilder prm_str5, StringBuilder prm_str6);

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        [DllImport("swwydll.dll", EntryPoint = "WF002", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr WF002(StringBuilder prm_str1, StringBuilder prm_str2, StringBuilder prm_str3, StringBuilder prm_str4, StringBuilder prm_str5, StringBuilder prm_str6, StringBuilder prm_str7, StringBuilder prm_str8, StringBuilder prm_str9, StringBuilder prm_str10, StringBuilder prm_rtninfo1, StringBuilder prm_rtninfo2);










    }
}
