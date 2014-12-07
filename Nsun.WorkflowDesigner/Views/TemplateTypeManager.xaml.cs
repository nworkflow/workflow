using Nsun.Tools.WorkflowDesignerViewModel;
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

namespace DiagramDesigner.Views
{
    /// <summary>
    /// TemplateTypeManager.xaml 的互動邏輯
    /// </summary>
    public partial class TemplateTypeManager : Window
    {
        TemplateTypeManagerViewModel _vm;
        public TemplateTypeManager()
        {
            InitializeComponent();
             _vm= Resources["vm"] as TemplateTypeManagerViewModel;
             _vm.WindowAction = CloseWindow;
        }

        public void CloseWindow()
        {
            this.Close();
        }
    }
}
