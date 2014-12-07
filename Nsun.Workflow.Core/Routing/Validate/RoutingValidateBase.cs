using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Routing.Validate
{
    public class RoutingValidateBase:IRoutingValidate
    {
        protected ResultInfo Info = new ResultInfo();
        public virtual Utils.ResultInfo RoutingValidate(RoutingModel model)
        {
            return Info;
        }

        public virtual ResultInfo GetResult()
        {
            return Info;
        }
    }
}
