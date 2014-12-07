using Nsun.Service.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nsun.Workflow.WPFService
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowViewModel();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                _vm.StartSer();
                this.imgSerState.Opacity = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                _vm.StopSer();
                this.imgSerState.Opacity = 0.5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (true)
                this.imgSerState.Opacity = 0.8;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
