using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Common.Utils
{
    public interface ITransRouting
    {
        RoutingType TransType{get;}

        List<ITransRouting> AllITrans { get; set; }

        void Routing(XElement templateInfo, XElement currentNode, NSNodeInfo node, MainWorkflowUnitOfWork context);
    }
}
