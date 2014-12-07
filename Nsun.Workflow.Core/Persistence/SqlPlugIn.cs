using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Persistence
{
    public class SqlPlugIn : IPersistence
    {
        private DbContext _context;
        public DbContext Context
        {
            get
            {
                if (_context == null)
                    _context = new MainWorkflowUnitOfWork();
                return _context;
            }
            set
            {
                if (_context != value)
                    _context = value;
            }
        }

        private MainWorkflowUnitOfWork SqlDbContext
        {
            get { return Context as MainWorkflowUnitOfWork; }
        }

        public SqlPlugIn()
        {
        }


        public void CreateInstance(NSInstanceInfo instance)
        {
            SqlDbContext.NsInstanceInfos.Add(instance);
        }


        public void CreateRoutingInfo(NSRoutingInfo routing)
        {
            SqlDbContext.NsRoutingInfos.Add(routing);
        }


        public NSRoutingInfo CreateParallelRoutingInfo(Guid taskId, Guid instanceId, string groupName)
        {
            var routingInfo = SqlDbContext.NsRoutingInfos.FirstOrDefault(p => p.InstanceId == instanceId && p.GroupName == groupName);
            if (routingInfo == null)
            {
                routingInfo = new NSRoutingInfo();
                routingInfo.GroupName = groupName;
                routingInfo.Id = Guid.NewGuid();
                routingInfo.InstanceId = instanceId;
                routingInfo.TaskId = taskId;
                routingInfo.Finshed = false;
                routingInfo.GroupCounter = 1;
                SqlDbContext.NsRoutingInfos.Add(routingInfo);
                return routingInfo;
            }
            else
            {
                if (routingInfo.Finshed == true)
                {
                    routingInfo.Finshed = false;
                    routingInfo.GroupCounter = 1;
                    return routingInfo;
                }
                else
                {
                    int counter = routingInfo.GroupCounter + 1;
                    routingInfo.GroupCounter = counter;
                    return routingInfo;
                }
            }
        }


        #region Activity

        public void FinishActivity(NSNodeInfo node)
        {
            if (node != null)
            {
                // 修改状态
                node.RunState = RunStateConst.END.ToString();
            }
        }


        public void CreateActivity(NSNodeInfo node)
        {
            SqlDbContext.NSNodeInfos.Add(node);
        }


        public NSNodeInfo GetActivity(Guid taskId, Guid instanceId, Guid nodeId)
        {
            return SqlDbContext.NSNodeInfos.Where(p => p.TaskId == taskId && p.InstanceId == instanceId && p.NodeId == nodeId && p.RunState == RunStateConst.RUNNING).FirstOrDefault();
        }


        public NSNodeInfo GetActivityByID(Guid nodeId)
        {
            return SqlDbContext.NSNodeInfos.FirstOrDefault(p => p.Id == nodeId && p.RunState == RunStateConst.RUNNING);
        }

        #endregion (Activity)


        public NSTemplateInfo GetTemplateInfo(Guid templateId)
        {
            return SqlDbContext.NSTemplateInfos.Where(p => p.Id == templateId).FirstOrDefault();
        }


        public NSInstanceInfo GetInsanceInfo(Guid instanceId)
        {
            return SqlDbContext.NsInstanceInfos.Where(p => p.Id == instanceId).FirstOrDefault();
        }


        public void NewContext()
        {
            this.Context = new MainWorkflowUnitOfWork();
        }


        public NSInstanceInfo FinishInstance(Guid instanceId)
        {
            var instance = SqlDbContext.NsInstanceInfos.FirstOrDefault(p => p.Id == instanceId);
            if (instance != null)
                instance.RunState = RunStateConst.END;
            return instance;
        }


        public NSInstanceInfo GetInstance(Guid insanceId)
        {
            return SqlDbContext.NsInstanceInfos.FirstOrDefault(p => p.Id == insanceId);
        }


        public NSRoutingInfo GetRoutingInfo(Guid taskId, Guid instanceId)
        {
            return SqlDbContext.NsRoutingInfos.FirstOrDefault(p => p.TaskId == taskId && p.InstanceId == instanceId);
        }


        public NSNodeInfo FinishRouting(Guid taskId, Guid parentNodeId,string groupName)
        {
            var routingInfos = SqlDbContext.NsRoutingInfos.Where(p => p.TaskId == taskId && p.ParentId == parentNodeId && p.GroupName == groupName && !(p.Finshed ?? false)).ToList();
            var routingParents = routingInfos.Select(p => p.ParentId);
            // 找到
            routingInfos.ForEach(p => p.Finshed = true);
            string runState = RunStateConst.RUNNING;
            var runParent = SqlDbContext.NSNodeInfos.FirstOrDefault(p =>p.Id==parentNodeId && p.RunState == runState);

            return runParent;
        }


        public void FinishParallel(Guid taskId, Guid instanceId, string groupName)
        {
            var routingInfos = SqlDbContext.NsRoutingInfos.Where(p => p.TaskId == taskId && p.InstanceId == instanceId && p.GroupName == groupName && !(p.Finshed ?? false)).ToList();
            var routingParents = routingInfos.Select(p => p.ParentId);
            // 找到
            routingInfos.ForEach(p => p.Finshed = true);
        }


        /// <summary>
        /// 结束节点通过Nodeid 和 InstanceId 
        /// </summary>
        /// <param name="instanceId">实例ID</param>
        /// <param name="nodeIds">节点ID</param>
        /// <param name="runState">取值RunStateConst</param>
        public void FinishActivityByNodeIds(Guid instanceId, List<Guid> nodeIds, string runState)
        {
            if(nodeIds==null)
                return ;
            var nodes =  SqlDbContext.NSNodeInfos.Where(p=>p.InstanceId==instanceId&&nodeIds.Contains(p.NodeId));
            nodes.ToList().ForEach(p=>{
            p.RunState=runState;
            });
        }


        public void FinishTask(Guid taskId)
        {
            var unFinishInsntace = SqlDbContext.NsInstanceInfos.FirstOrDefault(p => p.TaskId == taskId && p.RunState == RunStateConst.RUNNING);
            if (unFinishInsntace==null)
            {
               var task=  SqlDbContext.NsTaskInfos.FirstOrDefault(p => p.TaskId == taskId);
               if (task != null)
               {
                   task.EndTime = DateTime.Now;
                   task.RunState = RunStateConst.END;
               }
            }
        }


        public void CreateGroup(NSNodeGroup group)
        {
            SqlDbContext.NsNodeGroups.Add(group);
        }


        public bool PopGroup(Guid taskId, Guid instanceNodeId, string group)
        {
            var nodeGroup = SqlDbContext.NsNodeGroups.FirstOrDefault(p=>p.TaskId==taskId
                && p.InstanceId == instanceNodeId
                &&p.GroupName==group
                &&p.Finshed!=true);
            if (nodeGroup != null)
            {
                if (nodeGroup.GroupCounter > 0)
                    nodeGroup.GroupCounter = nodeGroup.GroupCounter--;
                if (nodeGroup.GroupCounter == 0)
                {
                    nodeGroup.Finshed = true;
                }
            }
            return nodeGroup.GroupCounter > 0;
        }
    }
}
