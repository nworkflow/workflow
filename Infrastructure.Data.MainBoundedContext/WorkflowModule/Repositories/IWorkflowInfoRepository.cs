using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories
{
    public interface INSTemplateInfoRepository:IRepository<NSTemplateInfo>
    {
    }


    public interface INSTemplateTypeRepository : IRepository<NSTemplateType>
    {
    }

    public interface INSTaskInfoRespository : IRepository<NSTaskInfo>
    {
    }

    public interface INSInstanceInfoRespository : IRepository<NSInstanceInfo>
    {
    }

    public interface INSNodeInfoRespository : IRepository<NSNodeInfo>
    {
    }

    public interface INSRoutingInfoRespositorry : IRepository<NSRoutingInfo>
    {
    }
}
