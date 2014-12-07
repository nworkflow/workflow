using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories;
using Nsun.Workflow.Intall.InitInfo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Nsun.Workflow.Intall
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Set default initializer for MainBCUnitOfWork
             Database.SetInitializer<MainWorkflowUnitOfWork>(new MainWorkflowUnitOfWorkInitializer());
            //Database.SetInitializer<MainWorkflowUnitOfWork>(new DropCreateDatabaseIfModelChanges<MainWorkflowUnitOfWork>());
           // TemplateRepository t = new TemplateRepository(new MainWorkflowUnitOfWork());  
            
        }
    }
}
