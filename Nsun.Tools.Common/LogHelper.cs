using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nsun.Tools.Common
{

    ///<summary>
    /// 简单日志帮助类
    /// date   2013-6-24
    /// author LeoZhao
    ///<summary>
    public class LogHelper
    {
        public static void WriteSystemLog(string msg, bool isError = true)
        {
            if (!File.Exists(Environment.CurrentDirectory + "/Log.txt"))
                File.Create(Environment.CurrentDirectory + "/Log.txt");
            using (StreamWriter streamWriter = new StreamWriter(Environment.CurrentDirectory+"/Log.txt",true))
            {
                streamWriter.Write(msg);
            }
        }
    }
}
