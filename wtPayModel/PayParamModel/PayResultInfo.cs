using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PayParamModel
{
    public class PayResultInfo
    {
        public string code { get; set; }
        public string msgCode { get; set; }
        public string msg { get; set; }
        public Dictionary<string, string> map { get; set; }
        public object SC20003 { get; set; }
    }
}
