using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Nsun.Workflow.Common.Models
{
    [DataContract]
    public class ResultInfo
    {
        [DataMember]
        public bool HasError { get; set; }
        [DataMember]
        public List<string> Infos { get; set; }
        [DataMember]
        public string Result { get; set; }

        public void AppendInfo(string msg)
        {
            if (Infos == null)
                Infos = new List<string>();
            Infos.Add(msg);
        }

        public void AppendError(string msg)
        {
            if (Infos == null)
                Infos = new List<string>();
            HasError = true;
            Infos.Add(msg);
        }

        public string GetResult(string split)
        {
            return string.Join(split, Infos ?? new List<string>());
        }
    }
}
