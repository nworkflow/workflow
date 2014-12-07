using Nsun.Workflow.Intall.InitInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Nsun.Workflow.Intall
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Database.SetInitializer<TestUnit>(new DropCreateDatabaseIfModelChanges<TestUnit>());
            new MainWindow().Show();
            base.OnStartup(e);
        }
    }
}
