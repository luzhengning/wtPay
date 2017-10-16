using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.RegistrationModel
{
    /// <summary>
    /// 科室
    /// </summary>
    public class DepartmentClass
    {
        public static string DepartmentAppId = "YYGH002";
    }
    /// <summary>
    /// 科室查询参数
    /// </summary>
    public class DepartmentParam
    {
        public string appId { get; set;}
        public string conName { get; set; }
        public string hospital_code { get; set; }
        public string pageNo { get; set; }
        public string pageSize { get; set; }
    }
    /// <summary>
    /// 科室列表
    /// </summary>
    public class DepartmentInfo
    {
        public DepartmentInfoData data { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
    }
    public class DepartmentInfoData
    {
        public string total { get; set; }
        public string pageNo { get; set; }
        public string pageSize { get; set; }
        public List<DepartmentInfoDataRows> rows { get; set; }
    }
    public class DepartmentInfoDataRows
    {
        public string dept_code { get; set;}
        public string dept_name { get; set; }
        public string hospital_code { get; set; }
        public string rn { get; set; }
    }
}
