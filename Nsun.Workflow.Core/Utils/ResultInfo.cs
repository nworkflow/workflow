using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Utils
{
    public class ResultInfo
    {
        public bool HasError { get; set; }
        public List<string> Infos { get; set; }
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

        public bool AlreadyExist(string startKey)
        {
            if (Infos == null)
                Infos = new List<string>();
            return Infos.FirstOrDefault(p => p.StartsWith(startKey)) == null;
        }

        public string GetResult(string split)
        {
            return string.Join(split, Infos ?? new List<string>());
        }
    }
}
