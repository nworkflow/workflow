using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.EnumExt
{
    public enum SubmitTypeEnum
    {
        _SP=0, // StopParallel
        _BK=-1,// Back
        _SB=1, // Submit
        _CN=9, // Condition
    }


    public class RunStateConst
    {
        public static readonly string RUNNING ="running";
        public static readonly string TRANS = "trans";
        public static readonly string END ="end";
        public static readonly string ERROR = "error";
        public static readonly string URGENCY = "urgency";
        public static readonly string STOP = "stop";
    }
}
