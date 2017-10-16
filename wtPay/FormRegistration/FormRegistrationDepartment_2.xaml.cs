using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;
using wtPayDAL;
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistrationDepartment_2 : UserControl
    {
        //页码
        int pageNo = 1;
        //科室列表
        DepartmentInfo departmentList = null;
        //是否正在查询
        bool isQuery = false;
        private delegate void setShowGridDelegate(FormRegistrationDepartment_2 from, string Gridname, bool isShow);
        private delegate void setButtonValueDelegate(FormRegistrationDepartment_2 form, string gridName, string TextBlockName, string value);
        private delegate void setTextBlockDelegate(TextBlock tb, string value);

        private Thread queryThread = null;
        public FormRegistrationDepartment_2()
        {
            InitializeComponent();
        }
        private void setTextBlock(TextBlock tb, string value)
        {
            tb.Text = value;
        }
        private void setShowGrid(FormRegistrationDepartment_2 from, string Gridname, bool isShow)
        {
            if (isShow) ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Visible;
            else ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Hidden;
        }
        private void setButtonValue(FormRegistrationDepartment_2 form, string gridName, string TextBlockName, string value)
        {
            ((Grid)(form.FindName(gridName))).Visibility = Visibility.Visible;
            ((Label)(form.FindName(TextBlockName))).Content = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistrationHospital_1");
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                //清空面板
                clear();
                pageNo = 1;
                queryThread = new Thread(delegate () { query(pageNo); });
                queryThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:load():" + ex.Message);
            }
        }
        //查询
        private void query(int pageNo)
        {
            try
            {
                resultInfo("正在查询，请稍后...");
                isQuery = true;
                //清空面板
                clear();
                departmentList = null;
                DepartmentParam param = new DepartmentParam();
                param.pageNo = pageNo.ToString();
                param.hospital_code = RegistrationClass.hospitalInfo.hospital_code;
                param.pageSize = "12";
                departmentList = RegistrationAccess.DepartmentQuery(param);
                if ("9999".Equals(departmentList.success))
                {
                    resultInfo(departmentList.msg);
                    return;
                }
                if (departmentList.data.rows.Count == 0)
                {
                    resultInfo("该医院暂未提供预约信息");
                    return;
                }
                for (int i = 0; i < departmentList.data.rows.Count; i++)
                    setButton(this, ("button" + (i + 1)), ("name" + (i + 1)), departmentList.data.rows[i].dept_name);
                resultInfo("请选择科室");
                return;
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                resultInfo("暂无预约信息");
                log.Write("error:FormRegistrationDepartment_2:query():" + ex.Message);
            }
            finally
            {
                isQuery = false;
            }
        }
        private void resultInfo(String value)
        {
            showInfo.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), showInfo, value);
        }
        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="form"></param>
        /// <param name="gridName"></param>
        /// <param name="TextBlockName"></param>
        /// <param name="value"></param>
        private void setButton(FormRegistrationDepartment_2 form, string gridName, string TextBlockName, string value)
        {
            this.Dispatcher.Invoke(new setButtonValueDelegate(setButtonValue), form, gridName, TextBlockName, value);
        }
        //清空面板
        private void clear()
        {
            try
            {
                for (int i = 1; i <= 12; i++)
                {
                    this.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), this, ("button" + i), false);
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:clear():" + ex.Message);
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isQuery == false)
                {
                    if (pageNo == 1) return;
                    queryThread.Abort();
                    queryThread.DisableComObjectEagerCleanup();
                    queryThread = null;
                    pageNo -= 1;
                    //查询户号线程
                    queryThread = new Thread(delegate () { query(pageNo); });
                    queryThread.Start();
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:Button_Click_1:" + ex.Message);
            }
        }

        private void name1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label grid = (Label)sender;
                for (int i = 0; i <= departmentList.data.rows.Count; i++)
                {
                    if (grid.Content.Equals(departmentList.data.rows[i].dept_name))
                    {
                        RegistrationClass.departmentInfo = departmentList.data.rows[i];
                        RegistrationClass.registrationAddress.DepartmentName = grid.Content.ToString();
                        Util.JumpUtil.jumpCommonPage("FormRegistrationDoctor_3");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }

        private void Button_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isQuery == false)
                {
                    queryThread.Abort();
                    queryThread.DisableComObjectEagerCleanup();
                    queryThread = null;
                    pageNo += 1;
                    //查询户号线程
                    queryThread = new Thread(delegate () { query(pageNo); });
                    queryThread.Start();
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:Button_Click:" + ex.Message);
            }
        }
    }
}
