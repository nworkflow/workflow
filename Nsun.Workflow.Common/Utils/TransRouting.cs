using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Tools.Common;
using Nsun.Workflow.Core.Activities;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Common.Utils
{
    public class TransRoutingBase : ITransRouting
    {
        protected void RoutingTemplate(XElement templateInfo, XElement currentNode, NSNodeInfo node, MainWorkflowUnitOfWork context, string nodeType)
        {
            TransDec.GetTransRouting(AllITrans, nodeType).Routing(templateInfo, currentNode, node, context);
        }

        public virtual void Routing(XElement templateInfo, XElement currentNode, NSNodeInfo node, MainWorkflowUnitOfWork context)
        {
        }

        private RoutingType _transType;
        public virtual RoutingType TransType
        {
            get { return _transType; }
            set { _transType = value; }
        }

        protected List<ITransRouting> _allITrans = null;
        public List<ITransRouting> AllITrans
        {
            get
            {
                return _allITrans;
            }
            set
            {
                _allITrans = value;
            }
        }

        protected class PersistenNodes
        {
            private List<string> Persistences = new List<string>() { "Process" };
            public bool CreateNodeInfo(XElement currentNode)
            {
                return Persistences.Contains(currentNode.Element("BType").Value);
            }
        }

        public NSNodeInfo GetDefaultNSNodeInfo(XElement xElement, Guid taskId, Guid instanceId)
        {
            NSNodeInfo nsNodeInfo = new NSNodeInfo();
            nsNodeInfo.Id = Guid.NewGuid();
            nsNodeInfo.InstanceId = instanceId;
            nsNodeInfo.TaskId = taskId;
            nsNodeInfo.NodeName = xElement.Element("Name").Value;
            nsNodeInfo.NodeId = Guid.Parse(xElement.Element("ID").Value);
            nsNodeInfo.RunState = RunStateConst.RUNNING;
            nsNodeInfo.CreateTime = DateTime.Now;

            return nsNodeInfo;
        }

    }


    public sealed class DecisionRouting : TransRoutingBase
    {
        public override RoutingType TransType
        {
            get { return RoutingType._DS; }
        }

        public override void Routing(XElement templateInfo, XElement currentNode, NSNodeInfo node, MainWorkflowUnitOfWork context)
        {
            // 查找下一个节点,所有父为Switch的下一个节点的
            var childrenIds = from c in templateInfo.Elements("Connections").First().Elements("Connection")
                              where c.Element("SourceID").Value == currentNode.Element("ID").Value && c.Element("Condition").Value == node.SubmitResult
                              select c.Element("SinkID").Value;

            if (childrenIds.Count() == 0)
                return;

#warning  找到匹配的那个条件---条件待匹配
            var nodeInfos = from c in templateInfo.Elements("DesignerItems").Elements("DesignerItem")
                            where childrenIds.Contains(c.Element("ID").Value)
                            select c;

            foreach (var item in nodeInfos)
            {
                RoutingTemplate(templateInfo, item, node, context, item.Element("EType").Value);
            }
        }
    }


    public sealed class ParallelRouting : TransRoutingBase
    {
        public override RoutingType TransType
        {
            get { return RoutingType._PR; }
        }

        public override void Routing(XElement templateInfo, XElement currentNode, NSNodeInfo node, MainWorkflowUnitOfWork context)
        {
            // 查询是否所有的节点都已经执行完成？？
            // 查找下一个节点,所有父为Switch的下一个节点的
            var parentIds = from c in templateInfo.Elements("Connections").First().Elements("Connection")
                            where c.Element("SinkID").Value == currentNode.Element("ID").Value
                            select c.Element("SourceID").Value;

            // 查找是否已经存在了没有完成的计数器
            NSNodeGroup alreadyExistGroup = context.NsNodeGroups.Where(p => p.TaskId == node.TaskId
                && p.InstanceId == node.InstanceId
                && p.Finshed == false || p.Finshed == null).FirstOrDefault();

            if (alreadyExistGroup != null)
            {
                alreadyExistGroup.GroupCounter = ++alreadyExistGroup.GroupCounter;
                context.SetModified<NSNodeGroup>(alreadyExistGroup);
            }
            else
            {
                if (alreadyExistGroup == null)
                    alreadyExistGroup = new NSNodeGroup();

                alreadyExistGroup.Id = Guid.NewGuid();
                alreadyExistGroup.GroupCounter = 1;
                alreadyExistGroup.GroupName = "Parallel";
                alreadyExistGroup.TaskId = node.TaskId;
                alreadyExistGroup.InstanceId = node.InstanceId;
                alreadyExistGroup.Finshed = false;
                context.NsNodeGroups.Add(alreadyExistGroup);
            }

            if (parentIds.Count() == alreadyExistGroup.GroupCounter)
            {
                // 查找下一个节点,所有父为Switch的下一个节点的
                var childrenIds = from c in templateInfo.Elements("Connections").First().Elements("Connection")
                                  where c.Element("SourceID").Value == currentNode.Element("ID").Value
                                  select c.Element("SinkID").Value;
                alreadyExistGroup.Finshed = true;
                if (childrenIds.Count() == 0)
                    return;

                var nodeInfos = from c in templateInfo.Elements("DesignerItems").Elements("DesignerItem")
                                where childrenIds.Contains(c.Element("ID").Value)
                                select c;
                foreach (var item in nodeInfos)
                {
                    RoutingTemplate(templateInfo, item, node, context, item.Element("EType").Value);
                }
                return;
            }
        }
    }


    public sealed class ProcessRouting : TransRoutingBase
    {
        public override RoutingType TransType
        {
            get { return RoutingType._PR; }
        }

        public override void Routing(XElement templateInfo, XElement currentNode, NSNodeInfo node, MainWorkflowUnitOfWork context)
        {
            NSNodeInfo nsNodeInfo = new NSNodeInfo();
            nsNodeInfo.Id = Guid.NewGuid();
            nsNodeInfo.InstanceId = node.InstanceId;
            nsNodeInfo.TaskId = node.TaskId;
            nsNodeInfo.NodeName = currentNode.Element("Name").Value;
            nsNodeInfo.NodeId = Guid.Parse(currentNode.Element("ID").Value);
            nsNodeInfo.RunState = RunStateConst.RUNNING;
            nsNodeInfo.CreateTime = DateTime.Now;
            context.NSNodeInfos.Add(nsNodeInfo);
        }
    }


    public class TransDec
    {
        public static ITransRouting GetTransRouting(List<ITransRouting> trans, string nodeType)
        {
            return trans.Where(p => p.TransType.ToString() == nodeType).First();
        }
    }
}
