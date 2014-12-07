using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Nsun.Workflow.Common.DTOs
{
    [DataContract]
    public class ExpandData
    {
        [DataMember]
        public int Counter
        {
            get;
            set;
        }


        [DataMember]
        public string Info
        {
            get;
            set;
        }


        [DataMember]
        public Dictionary<string, string> Exp
        {
            get;
            set;
        }
    }
}
