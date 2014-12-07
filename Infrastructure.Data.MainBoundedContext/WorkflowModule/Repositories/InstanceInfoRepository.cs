using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories
{
    public class InstanceInfoRepository : Repository<NSInstanceInfo>, INSInstanceInfoRespository
    {
        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public InstanceInfoRepository(MainWorkflowUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }


        public override NSInstanceInfo Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;

                var set = currentUnitOfWork.CreateSet<NSInstanceInfo>();

                return set.Where(c => c.Id == id)
                          .SingleOrDefault();
            }
            else
                return null;
        }


        public List<NSInstanceInfo> GetNSInstanceInfosByTaskId(Guid taskId)
        {
            if (taskId != null)
            {
                var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;

                var set = currentUnitOfWork.CreateSet<NSInstanceInfo>();

                return set.Where(c => c.TaskId == taskId).ToList();
            }
            else
                return null;
        }

    }
}
