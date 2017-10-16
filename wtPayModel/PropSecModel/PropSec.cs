using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropSecModel
{
    public class PropSec
    {
        /// <summary>
        /// json转对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public PropSecCardJson JsonToModel(string json)
        {
            return JsonConvert.DeserializeObject<PropSecCardJson>(json);
        }
    }
}
