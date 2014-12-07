using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DiagramDesigner.Resources.Stencils
{
    public partial class FlowChartStencils : ResourceDictionary
    {
        public string Msg
        {
            get;
            set;
        }

        public FlowChartStencils()
        {
            InitializeComponent();
        }

        public void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("AAA");
        }

        //private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        //{
        //    MessageBox.Show("成功了");
        //}
        public void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageBox.Show("成功了");
        }
    }
}
