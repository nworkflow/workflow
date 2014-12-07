using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.EnumExt
{
    /// <summary>
    /// RULE = [Command]:[Condition]
    /// </summary>
    public enum SubmitCommand
    {
        _SP, //= "Stop",
        _OR, //= "Overrule",
        _PS, //= "Pass",
        _CS //= "Decision",
    }
}
