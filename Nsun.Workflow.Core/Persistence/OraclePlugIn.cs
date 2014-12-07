using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Persistence
{
    public class OraclePlugIn
    {


        public DbContext Context
        {
            get { throw new NotImplementedException(); }
        }

        public void CreateActivity(Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg.NSNodeInfo node, DbContext t)
        {
            throw new NotImplementedException();
        }

        public void CreateInstance(Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg.NSInstanceInfo instance, DbContext t)
        {
            throw new NotImplementedException();
        }

        public void CreateRoutingInfo(Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg.NSRoutingInfo routing, DbContext t)
        {
            throw new NotImplementedException();
        }

        public void FinishSubRouting(Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg.NSNodeInfo node, DbContext t)
        {
            throw new NotImplementedException();
        }

        public Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg.NSNodeInfo GetActivity(Guid taskId, Guid instanceId, Guid nodeId, DbContext t)
        {
            throw new NotImplementedException();
        }

        public Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg.NSTemplateInfo GetTemplateInfo(Guid templateId, DbContext t)
        {
            throw new NotImplementedException();
        }

        public Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg.NSInstanceInfo GetInsanceInfo(Guid instanceId, DbContext t)
        {
            throw new NotImplementedException();
        }
    }
}
