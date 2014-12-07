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

namespace Nsun.Tools.WorkflowDesigner.Views
{
    /// <summary>
    /// NewTemplate.xaml 的互動邏輯
    /// </summary>
    public partial class NewTemplate : Window
    {
        NewTemplateViewModel vm = null;
        public NewTemplate()
        {
            InitializeComponent();
        }


        public NewTemplate(string content)
        {
            InitializeComponent();
            vm= Resources["vm"] as NewTemplateViewModel;
            vm.WindowAction = () => { this.Close(); };
            vm.TemplateXML = content;
        }


        public NewTemplate(string templateName, string templateType, string content)
        {
            InitializeComponent();
        }
    }
}
