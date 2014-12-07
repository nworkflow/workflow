using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Utils
{
    /// <summary>
    /// 信息帮助类,用于处理返回至信息
    /// </summary>
    public static class BackMsgHelper
    {
        public static List<string> Infos = new List<string>() { };
        public static string GetMsg(this string msg, out bool isError)
        {
            if (!string.IsNullOrEmpty(msg) && msg.Length > 5)
            {
                foreach (var info in Infos)
                {
                    if (msg.StartsWith(info))
                    {
                        isError = false;
                        return msg.Remove(0, info.Length);
                    }
                }
            }
            isError = true;
            return msg;
        }
    }
}
