using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Validation
{
    public interface ITemplateValidation
    {
        string ValidateMsg(XElement template,List<string> runStates,ref Dictionary<string,Stack<string>> exeResults);
    }
}
