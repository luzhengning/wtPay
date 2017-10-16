using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace wtPay
{
    /// <summary>
    /// keyboard.xaml 的交互逻辑
    /// </summary>
    public partial class keyboard : UserControl
    {

        public TextBox textBox;


        public keyboard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBox == null)
            {
                return;
            }
            Button btn = sender as Button;
            textBox.Text += btn.Uid;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.textBox == null)
            {
                return;
            }
            this.textBox.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.textBox == null)
            {
                return;
            }
            if (this.textBox.Text.Equals(""))
            {
                return;
            }
            this.textBox.Text = this.textBox.Text.Substring(0, this.textBox.Text.Length - 1);
        }
        
    }
}
