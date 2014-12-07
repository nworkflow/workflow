using Nsun.Workflow.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Validation
{
    public interface ISubmitValidation
    {
        string ValidateMsg(String submitMsg,TransInfoDto dto);
    }
}
