using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Utils
{
    public static class StringBuilderExt
    {
        public static void AppendMsg(this StringBuilder sb, string msg)
        {
            if (sb == null)
                sb = new StringBuilder();
            if (sb.Length != 0)
                sb.Append(Environment.NewLine);
            sb.Append(msg);
        }
    }
}
