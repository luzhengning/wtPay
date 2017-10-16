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
using System.Windows.Threading;

namespace wtPay.usercontrol
{
    /// <summary>
    /// CountDownTimer.xaml 的交互逻辑
    /// </summary>
    public partial class CountDownTimer : UserControl
    {

        private DispatcherTimer timer;

        private int currentSecond;


        public CountDownTimer()
        {
            InitializeComponent();
        }



        public int beginSecond
        {
            get { return (int)GetValue(beginSecondProperty); }
            set { SetValue(beginSecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for beginSecond.  This enables animation, styling, binding, etc...
        public static DependencyProperty beginSecondProperty =
            DependencyProperty.Register("beginSecond", typeof(int), typeof(CountDownTimer), new PropertyMetadata(90));
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            currentSecond = 300;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// Timer触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {

            if (currentSecond > 0)
            {
                currentSecond -= 1;
                countText.Text = currentSecond.ToString();
            }
            else
            {
                wtPay.Util.JumpUtil.jumpMainPage();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
        }
    }
}
