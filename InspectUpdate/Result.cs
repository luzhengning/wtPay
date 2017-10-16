using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectUpdate
{
    public class Result
    {
        public int code { get; set; }

        public string message { get; set; }

        public List<Dictionary<string, string>> data { get; set; }
    }
}
