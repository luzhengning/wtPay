using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.RegistrationModel
{
    /// <summary>
    /// 物业登录
    /// </summary>
    public class PropLogin
    {
    }
    public class PropLoginNameParam
    {
        public string mobile { get; set; }
        public string password { get; set; }
    }
    /// <summary>
    /// 物业登录接口返回
    /// </summary>
    public class PropLoginNameInfo
    {
        public string code { get; set; }
        public string msgCode { get; set; }
        public string msg { get; set; }
        public PropLoginInfoNameData data { get; set;}
        public string appImg { get; set; }
    }
    public class PropLoginInfoNameData
    {
        public string userCode { get; set;}
        public string userName { get; set; }
    }
}
//1).code=0000 & msgCode=0000 	    登录成功 data为预约挂号所需数据, 包含用户名、手机号、用户编号
//2).code=9999 & msgCode=9955		账号密码不匹配,终端需提示用户重新输入登录信息进行登录
//3).code=9999 & msgCode=9960		用户编号不存在,终端需提示用户进行三维之家注册
//4).code=9999 & msgCode=9999		登录异常,终端需提示用户暂时无法进行登录认证
