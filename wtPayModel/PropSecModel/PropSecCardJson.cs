using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropSecModel
{
    public class PropSecCardJson
    {
        public string G_0001 { get; set; }//varchar2(20) ----公共字段----------
        public string G_2202 { get; set; }//G_T_022.G_2202%TYPE, ----交易类型
        public string G_2203 { get; set; }//G_T_022.G_2203%TYPE, ----交易金额
        public string G_2204 { get; set; }//G_T_022.G_2204%TYPE, ----交易量--------------- 
        public string G_2205 { get; set; }//G_T_022.G_2205%TYPE, ----实收金额
        public string G_2206 { get; set; }//G_T_022.G_2206%TYPE, ----补助金额
        public string G_2207 { get; set; }//G_T_022.G_2207%TYPE, ---补助量
        public string G_2208 { get; set; }//G_T_022.G_2208%TYPE, ----损耗金额
        public string G_2209 { get; set; }//G_T_022.G_2209%TYPE, ----损耗量
        public string G_0503 { get; set; }//G_T_005.G_0503%TYPE, ----表型
        public string G_0504 { get; set; }//G_T_019.G_0504%TYPE, ----报警量
        public string G_0505 { get; set; }//G_T_019.G_0505%TYPE, ----报警量
        public string G_0506 { get; set; }//G_T_019.G_0506%TYPE, ----关阀量
        public string G_0507 { get; set; }//G_T_019.G_0507%TYPE, ----囤积量
        public string G_0508 { get; set; }//G_T_019.G_0508%TYPE, ----损耗
        public string G_0509 { get; set; }//G_T_019.G_0509%TYPE, ---允许负荷
        public string G_1003 { get; set; }//G_T_010.G_1003%TYPE, ----卡片类型---- 
        public string G_0510 { get; set; }//G_T_019.G_0510%TYPE, --  表具常数----- 
        public string G_0511 { get; set; }//G_T_019.G_0511%TYPE, ---- 互感倍数
        public string G_1902 { get; set; }//G_T_019.G_1902%TYPE, --- 过载次数
        public string G_1102 { get; set; }//G_T_011.G_1102%TYPE, --  允许补卡次数
        public string G_1103 { get; set; }//G_T_011.G_1103%TYPE, --  透支金额
        public string G_1104 { get; set; }//G_T_011.G_1104%TYPE, --  透支量
        public string G_1105 { get; set; }//G_T_011.G_1105%TYPE, --  抄表月
        public string G_1106 { get; set; }//G_T_011.G_1106%TYPE, --  抄表日
        public string G_1107 { get; set; }//G_T_011.G_1107%TYPE, --  结算日
        public string G_1108 { get; set; }//G_T_011.G_1108%TYPE, --  限购次数
        public string G_1109 { get; set; }//G_T_011.G_1109%TYPE, --  购买下限
        public string G_1110 { get; set; }//G_T_011.G_1110%TYPE, -- 购买上限
        public string G_1111 { get; set; }//G_T_011.G_1111%TYPE, -- 允许欠费次数
        public string G_1112 { get; set; }//G_T_011.G_1112%TYPE, -- 允许换表次数
        public string G_1113 { get; set; }//G_T_011.G_1113%TYPE, -- 卡片价格
        public string G_0806 { get; set; }//G_T_008.G_0805%TYPE, ---单位编号-- 同小区号-- 
        public string G_1802 { get; set; }//G_T_018.G_1802%TYPE, ---小区业主编号（户号）---- 
        public string G_2008 { get; set; }//G_T_020.G_2008%TYPE, ----购买次数-------------- 
        public string G_2002 { get; set; }//G_T_020.G_2002%TYPE, ----购买量
        public string G_2003 { get; set; }//G_T_020.G_2003%TYPE, ----购买金额
        public string G_2014 { get; set; }//G_T_020.G_2014%TYPE, ---插卡时间
        public string G_2015 { get; set; }//G_T_020.G_2015%TYPE, ----插表标识--------------- 
        public string G_0107 { get; set; }//G_T_018.G_0107%TYPE, --操作人
        public string G_1302 { get; set; }//MYTYPE, ----阶梯量
        public string G_1304 { get; set; }//MYTYPE, -----阶梯价格
        public string G_0903 { get; set; }//G_T_019.G_0903%TYPE, ------分类号
        public string G_1903 { get; set; }//G_T_009.G_1903%TYPE, ---表具编号--------------- 
        public string G_0105 { get; set; }//G_T_001.G_0105%TYPE, ----表具状态
        public string G_0602 { get; set; }// G_T_006.G_0602%TYPE, -----卡名称
        public string G_0108 { get; set; }//G_T_001.G_0108%TYPE, ----物业公司编号
        public string G_2017 { get; set; }//G_T_020.G_2017%TYPE, -----累计购水金额
        public string G_2004 { get; set; }//G_T_020.G_2004%TYPE, ----总用量
        public string G_2013 { get; set; }//G_T_020.G_2013%TYPE, -----磁保护时间
        public string G_2006 { get; set; }//G_T_020.G_2006%TYPE, ----剩余金额
        public string G_1203 { get; set; }//G_T_012.G_1203%TYPE, ----模式
        public string G_1204 { get; set; }//G_T_012.G_1204%TYPE, ----单价
        public string G_1206 { get; set; }//G_T_012.G_1206%TYPE, -----价格开始时间
        public string G_0002 { get; set; }//VARCHAR2(10) -----开户购水标识
        public string G_1006 { get; set; }//G_T_010.G_1006%TYPE, ----预设值
        public string G_0003 { get; set; }//VARCHAR2(10), ----脉冲宽度
        public string G_0004 { get; set; }// VARCHAR2(10) ----表具版本
        public string G_0806_1 { get; set; }// G_T_008.G_0805%TYPE, ---新单位编号
        public string G_1003_1 { get; set; }//G_T_010.G_1003%TYPE, -----工具卡类型
        public string G_0005 { get; set; }// VARCHAR(20), ----自定义参数  （西安--校时）
        public string G_1005 { get; set; }// G_T_010.G_1005%TYPE  ----使用次数

    }
}
