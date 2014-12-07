using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Tools.Common
{
    public class StringTools
    {
 
        public static string GetSystemInfoFormate(string message)
        {
            return Environment.NewLine + "--------------System Message----------" + Environment.NewLine + message;
        }

        public static string ConnectionString;
    }
}
