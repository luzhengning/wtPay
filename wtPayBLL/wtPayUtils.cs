using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnalysisBase;
using wtPayCommon;
using System.Windows.Forms;
namespace wtPayBLL
{
    public class wtPayUtils
    {


        public static string ConvertMoney(string money)
        {


            double _tempPayMoney = Convert.ToDouble(money) * 100;
            return BCDUtil.leftpad(decimal.Parse(_tempPayMoney + "").ToString(),12);
            //int _tempPayMoney = (Int32)(Convert.ToDouble(money) * 100);
            //return BCDUtil.leftpad(Convert.ToString(_tempPayMoney), 12);

        }

        public static int Fibonacci(int times)
        {
            int a=1, b=1, c=0;
            if (times == 1 || times == 2)
            {
                return 1;
            }
            for (int i = 1; i <= times; i++)
            {
                c = a + b;
                a = b;
                b = c;
            }
            return c;
        }
        public static void PrintInfo(WtException e, Label label)
        {
            PrintInfo(e, label, null);

        }
        public static void PrintInfo(WtException e,Label label,PictureBox box)
        {
            PrintInfo(e.getMsg(), label,box);
        }
        public static void PrintInfo(string msg, Label label)
        {
            PrintInfo(msg, label, null);
        }
        public static void PrintInfo(string msg, Label label,PictureBox box)
        {
            try
            {
                label.Text = msg;
                if (box != null)
                {
                    box.Hide();
                }
            }catch(Exception ex) { }
        }
    }
}
