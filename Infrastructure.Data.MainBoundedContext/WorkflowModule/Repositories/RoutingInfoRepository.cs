using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories
{
    public class RoutingInfoRepository : Repository<NSRoutingInfo>,INSRoutingInfoRespositorry
    {
        public RoutingInfoRepository(MainWorkflowUnitOfWork unitOfWork): base(unitOfWork)
        {
        }
    }
}
