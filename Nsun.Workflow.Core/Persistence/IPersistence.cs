using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Persistence
{
    public interface IPersistence
    {
        DbContext Context
        {
            get;
            set;
        }

        void NewContext();

        void CreateActivity(NSNodeInfo node);
        void FinishActivity(NSNodeInfo node);
        void FinishActivityByNodeIds(Guid instanceId, List<Guid> nodeIds,string runState);
        NSNodeInfo GetActivity(Guid taskId, Guid instanceId, Guid nodeId) ;
        NSNodeInfo GetActivityByID(Guid nodeId);

        void CreateInstance(NSInstanceInfo instance);
        NSInstanceInfo FinishInstance(Guid instanceId);
        NSInstanceInfo GetInstance(Guid insanceId);

        void FinishTask(Guid taskId);

        void CreateRoutingInfo(NSRoutingInfo routing);
        NSRoutingInfo CreateParallelRoutingInfo(Guid taskId, Guid instanceId, string groupName);
        NSRoutingInfo GetRoutingInfo(Guid taskId, Guid instanceId);
        NSNodeInfo FinishRouting(Guid taskId, Guid nodeId,string groupName);
        void FinishParallel(Guid taskId, Guid instanceId, string groupName);
        NSTemplateInfo GetTemplateInfo(Guid templateId);
        NSInstanceInfo GetInsanceInfo(Guid instanceId);
        void CreateGroup(NSNodeGroup group);
        bool PopGroup(Guid taskId, Guid instanceNodeId, string group);
    }
}
