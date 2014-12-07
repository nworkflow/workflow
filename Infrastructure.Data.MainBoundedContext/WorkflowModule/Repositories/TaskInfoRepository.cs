using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories
{
    public class TaskInfoRepository : Repository<NSTaskInfo>,INSTaskInfoRespository
    {

        public TaskInfoRepository(MainWorkflowUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override NSTaskInfo Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;

                var set = currentUnitOfWork.CreateSet<NSTaskInfo>();

                return set.Where(c => c.Id == id)
                          .SingleOrDefault();
            }
            else
                return null;
        }
    }
}
