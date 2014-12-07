using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories
{
    public class NodeInfoRepository : Repository<NSNodeInfo>, INSNodeInfoRespository
    {
        public NodeInfoRepository(MainWorkflowUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override NSNodeInfo Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;

                var set = currentUnitOfWork.CreateSet<NSNodeInfo>();

                return set.Where(c => c.Id == id)
                          .SingleOrDefault();
            }
            return null;
        }

        public List<NSNodeInfo> GetNSNodeInfosByInstanceId(Guid instanceId)
        {
            if (instanceId != Guid.Empty)
            {
                var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;
                var set = currentUnitOfWork.CreateSet<NSNodeInfo>();
                return set.Where(c => c.InstanceId == instanceId).ToList();
            }
            return null;
        }


        public List<NSNodeInfo> GetNSNodeInfosByInstanceIdAndRunState(Guid instanceId, string runState)
        {
            if (instanceId != Guid.Empty)
            {
                var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;
                var set = currentUnitOfWork.CreateSet<NSNodeInfo>();
                return set.Where(c => c.InstanceId == instanceId&&c.RunState==runState).ToList();
            }
            return null;
        }
    }
}
