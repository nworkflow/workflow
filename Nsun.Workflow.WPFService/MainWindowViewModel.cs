using Nsun.Servcie.Common;
using Nsun.Workflow.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Nsun.Service.Common
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ServiceHost _serviceHost;

        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set
            {
                _msg = value;
                RaisePorpertyChange("Msg");
            }
        }

        public ICommand StartCommand
        {
            get;
            private set;
        }

        public ICommand StopCommand
        {
            get;
            private set;
        }


        public void StartSer()
        {
            _serviceHost.Open();   
        }

        public void StopSer()
        {
            _serviceHost.Close();
        }

        public MainWindowViewModel()
        {
            _serviceHost = new ServiceHost(typeof(Service1));
            _serviceHost.Opened += (s, r) =>
            {
                Msg += "服务开启";
            };

            _serviceHost.Closed += (s, r) =>
            {
                Msg += "服务关闭";
            };

            StartCommand = new RelayCommand<string>((t) =>
            {
                _serviceHost.Open();              
            });

            StopCommand = new RelayCommand<string>((t) =>
            {
                _serviceHost.Close();
            });

         
        }
    }
}
