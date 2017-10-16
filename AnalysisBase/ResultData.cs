using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisBase
{
    public class ResultData
    {
        public string value { get; set; }
        public int pos { get; set; }
        public string name { get; set; }
        public ResultData(int pos)
        {
            this.pos = pos;
        }
        public ResultData()
        {
            this.pos = pos;
        }
        public ResultData(ResultData result)
        {
            this.pos = result.pos;
            this.value = result.value;
            this.name = result.name;
        }
    }
}
