using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Nsun.Workflow.Core.EnumExt;

namespace Nsun.Workflow.Core.Utils
{
    public class TransRoutingHelper
    {
        public static IEnumerable<Guid> InstanceFinished(HashSet<Guid> nextActivities,Guid taskId,Guid instanceId,MainWorkflowUnitOfWork context)
        {
            // 当前流程实例已经结束
            if (nextActivities == null || nextActivities.Count() == 0)
            {
                // 查找路由表中，是否存在当前任务的并且为当前模板的ID
                var routingInfos = context.NsRoutingInfos.Where(p => p.TaskId == taskId && p.InstanceId == instanceId && !(p.Finshed ?? false)).ToList();
                var routingParents = routingInfos.Select(p => p.ParentId);
                    // 找到
                string runState = RunStateConst.RUNNING;
                var runParent = context.NSNodeInfos.Where(p => routingParents.Contains(p.Id) && p.RunState == runState).ToList();
                    runParent.ForEach(p=>{
                        p.RunState = RunStateConst.END.ToString();
                    });

                routingInfos.ForEach(p => p.Finshed =true);

                return routingParents;
            }

            return new List<Guid>();
        }
    }
}
