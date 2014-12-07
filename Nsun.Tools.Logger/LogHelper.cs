using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"Log4\log4net.config", Watch = true)] 
namespace Nsun.Tools.Logger
{
    public class LogHelper
    {
        public static void WriteLog(string logger)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("logger");
            log.Info(logger);
            Debug.WriteLine(logger);
        }
    }
}
