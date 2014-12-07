using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Utils
{
    public class WFRoutingException:Exception
    {
        public WFRoutingException(string msg):base(msg)
        {
            msg = string.Format("流程解析存在错误：", Environment.NewLine, msg);
        }
    }

    public class WFDesignException : Exception
    {
        public WFDesignException(string msg)
            : base(msg)
        {
            msg = string.Format("流程模板设计有误：{0}",Environment.NewLine,msg);
        }
    }
}
