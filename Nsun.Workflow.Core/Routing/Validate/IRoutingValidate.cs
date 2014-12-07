using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Routing.Validate
{
    public interface IRoutingValidate
    {
        ResultInfo RoutingValidate(RoutingModel model);
        ResultInfo  GetResult();
    }
}
