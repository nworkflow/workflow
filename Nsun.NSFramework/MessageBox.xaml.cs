using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nsun.NSFrameWork
{
    /// <summary>
    /// MessageBox.xaml 的互動邏輯
    /// </summary>
    public partial class CMessageBox : Window
    {
        public CMessageBox(string msg)
        {
            InitializeComponent();
            this.txtMsg.Text = msg;
            this.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
