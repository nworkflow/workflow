using Nsun.NSFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Nsun.NSFramework
{
    public class MessageHelper
    {
        public static void ShowMessageBox(string message)
        {
            new CMessageBox(message);
        }


        public static void ShowConfirmBox(string message, Action nextMethod, Action cancelMethod = null)
        {
            var msg = MessageBox.Show(message, "确认框", MessageBoxButton.OKCancel);
            if (msg.ToString() == "OK")
                nextMethod();
            else
            {
                if (cancelMethod == null)
                    cancelMethod();
            }
        }
    }
}
