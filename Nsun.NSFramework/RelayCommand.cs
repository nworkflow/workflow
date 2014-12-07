using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Nsun.NSFramework
{
    public class RelayCommand:ICommand
    {
        private Action _hander;
        public RelayCommand(Action hander)
        {
            _hander = hander;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _hander();
        }
    }
}
