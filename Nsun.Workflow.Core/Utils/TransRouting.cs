using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Tools.Common;
using Nsun.Workflow.Core.Activities;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.Persistence;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Utils
{
    public class TransRouting
    {
        private static List<string> Persistences = new List<string>() { "Process" };
        private static Dictionary<string, int> _parallelCount = new Dictionary<string, int>();


        public static bool CreateNodeInfo(XElement currentNode)
        {
            return Persistences.Contains(currentNode.Element("BType").Value);
        }


        public static void Routing(XElement templateInfo, XElement currentNode, NSNodeInfo node, MainWorkflowUnitOfWork context, HashSet<Guid> newActivities)
        {
            switch (currentNode.Element("BType").Value)
            {
                case "Decision":
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
                            Routing(templateInfo, item, node, context, newActivities);
                        }
                    }
                    break;
                case "Parallel":
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
                                Routing(templateInfo, item, node, context, newActivities);
                            }
                            return;
                        }
                    }
                    break;
                case "Process":
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
                        newActivities.Add(nsNodeInfo.NodeId);
                    }
                    break;
                case "SubRouting":
                    {
                        SubRoutingBookmark submitRoutingBookmark = new SubRoutingBookmark();
                        var dbPlugIn =  DBFactory.GetPersistencePlugIn();

                        TransInfoDto tranInfoDto = new TransInfoDto();
                        tranInfoDto.InstanceId=node.InstanceId;
                        tranInfoDto.TaskId=node.TaskId;
                        //TODO:容易出错的地方，暂时没有处理的
                        tranInfoDto.TemplateXml = dbPlugIn.GetTemplateInfo(dbPlugIn.GetInsanceInfo(node.InstanceId).Id).TemplateText;
                        tranInfoDto.ParentId = node.Id;

                        submitRoutingBookmark.Start(tranInfoDto);

                        return;

                        // 生成第一节点 调用启动流程
                        NSNodeInfo parentNodeInfo = new NSNodeInfo();
                        parentNodeInfo.Id = Guid.NewGuid();
                        parentNodeInfo.InstanceId = node.InstanceId;
                        parentNodeInfo.TaskId = node.TaskId;
                        parentNodeInfo.NodeName = currentNode.Element("Name").Value;
                        parentNodeInfo.NodeId = Guid.Parse(currentNode.Element("ID").Value);
                        parentNodeInfo.RunState = RunStateConst.RUNNING;
                        parentNodeInfo.CreateTime = DateTime.Now;
                        context.NSNodeInfos.Add(parentNodeInfo);

                        NSInstanceInfo instance = new NSInstanceInfo();
                        instance.InstanceName = "";//读取的模板名称
                        instance.TaskId = node.TaskId;
                        instance.TemplateId = Guid.NewGuid();// 湖区模板ID
                        instance.RunState = RunStateConst.RUNNING;
                        instance.StartTime = DateTime.Now;
                        instance.Id = Guid.NewGuid();
                        instance.TemplateId = JsonHelper.JsonToT<SubRoutingBookmark>(currentNode.Element("Details").Value).TemplateId;
                        context.NsInstanceInfos.Add(instance);
                        // 如果有个字段用来区分是子流程，那么就可以轻松的进行操作。

                        // 生成第一个点
                        var childTemplate = context.NSTemplateInfos.First(p => p.Id == instance.TemplateId);
                        string templateXML = childTemplate.TemplateText;
                        XElement doc = XElement.Parse(templateXML);
                        var nodeInfos = from c in doc.Elements("DesignerItems").Elements("DesignerItem")
                                        where c.Element("BType").Value == "Start"
                                        select c;
                        // start node
                        var startNode = nodeInfos.First();
                        // start node id
                        string id = startNode.Element("ID").Value;
                        // start node next nodes
                        var custs = from c in doc.Elements("Connections").First().Elements("Connection")
                                    where c.Element("SourceID").Value == id
                                    select c.Element("SinkID").Value;
                        // get all nodes startnode link
                        foreach (var item in custs)
                        {
                            // get nodeinfo
                            var nodeInfo = from c in doc.Elements("DesignerItems").Elements("DesignerItem")
                                           where c.Element("ID").Value == item
                                           select c;

                            //  create node info if need 
                            if (TransRouting.CreateNodeInfo(nodeInfo.First()))
                            {
                                NSNodeInfo nsNodeInfo = new NSNodeInfo();
                                nsNodeInfo.Id = Guid.NewGuid();
                                nsNodeInfo.InstanceId = instance.Id;
                                nsNodeInfo.TaskId = instance.TaskId;
                                nsNodeInfo.NodeId = Guid.Parse(item.ToString());
                                nsNodeInfo.NodeName = nodeInfo.First().Element("Name").Value;
                                nsNodeInfo.NodeId = Guid.Parse(nodeInfo.First().Element("ID").Value);
                                nsNodeInfo.RunState = RunStateConst.RUNNING;
                                nsNodeInfo.CreateTime = DateTime.Now;
                                context.NSNodeInfos.Add(nsNodeInfo);
                            }
                            else
                            {
                                // only prepare taskid and instanceid
                                NSNodeInfo nsNodeInfo = new NSNodeInfo();
                                nsNodeInfo.Id = Guid.NewGuid();
                                nsNodeInfo.InstanceId = instance.Id;
                                nsNodeInfo.TaskId = instance.TaskId;
                                // running routting
                                TransRouting.Routing(doc, nodeInfo.First(), nsNodeInfo, context, newActivities);
                            }
                        }

                        NSRoutingInfo routingInfo = new NSRoutingInfo();
                        routingInfo.InstanceId = instance.Id;
                        routingInfo.TaskId = node.TaskId;
                        routingInfo.Id = Guid.NewGuid();
                        routingInfo.ParentId = parentNodeInfo.Id;

                        context.NsRoutingInfos.Add(routingInfo);
                    }
                    break;
            }
        }


        /// <summary>
        /// 停止并发流程
        /// </summary>
        /// <param name="template">模板</param>
        /// <param name="taskId">任务ID</param>
        /// <param name="nodeId">节点ID</param>
        public static void StopParallel(XElement template, Guid taskId, NSNodeInfo submitNodeInfo, MainWorkflowUnitOfWork context, Guid nodeId)
        {
            XElement parallelStart = null;
            // 获取对应的并发等待节点
            GetFirstParalle(template, nodeId, ref parallelStart);

            if (parallelStart == null)
                throw new Exception("没有找到并发起始节点");

            if (XmlHelper.HasValue(parallelStart, "GroupName"))
            {
                HashSet<Guid> parallelNodes = new HashSet<Guid>();
                StopAll(parallelStart.Element("GroupName").Value, template, parallelStart, taskId, submitNodeInfo.InstanceId, context, parallelNodes);
                var stopNodeInfos = context.NSNodeInfos.Where(p => p.InstanceId == submitNodeInfo.InstanceId && parallelNodes.Contains(p.NodeId));
                foreach (var nodeInfo in stopNodeInfos)
                {
                    nodeInfo.RunState = "stop";
                }
                context.SaveChanges();
            }
        }


        private static void StopAll(string groupName, XElement template, XElement nodeInfoPass, Guid taskId, Guid instancId, MainWorkflowUnitOfWork context, HashSet<Guid> parallelNodes)
        {
            // 查找父节点中的第一个节点
            var custs = from c in template.Elements("Connections").First().Elements("Connection")
                        where c.Element("SourceID").Value == nodeInfoPass.Element("ID").Value
                        select c.Element("SinkID").Value;

            // 获取节点名称
            var nodeInfoLinks = from c in template.Elements("DesignerItems").Elements("DesignerItem")
                                where custs.Contains(c.Element("ID").Value)
                                select c;

            foreach (var item in nodeInfoLinks)
            {
                if (item.Element("BType").Value == "Process")
                {
                    if (!_parallelCount.Keys.Contains(taskId + groupName))
                        _parallelCount.Add(taskId + groupName, nodeInfoLinks.Count());

                    parallelNodes.Add(Guid.Parse(item.Element("ID").Value));
                    StopAll(groupName, template, item, taskId, instancId, context, parallelNodes);
                }
                else
                {
                    if (item.Element("BType").Value == "Parallel" && item.Element("GroupName").Value == groupName)
                    {
                        _parallelCount[taskId + groupName] = _parallelCount[taskId + groupName] - 1;
                        if (_parallelCount[taskId + groupName] == 0)
                        {
                            _parallelCount.Remove(taskId + groupName);
                            // 查找下一个节点,所有父为Switch的下一个节点的
                            var childrenIds = from c in template.Elements("Connections").First().Elements("Connection")
                                              where c.Element("SourceID").Value == item.Element("ID").Value
                                              select c.Element("SinkID").Value;

                            if (childrenIds.Count() == 0)
                                return;

                            var nodeInfos = from c in template.Elements("DesignerItems").Elements("DesignerItem")
                                            where childrenIds.Contains(c.Element("ID").Value)
                                            select c;
                            foreach (var nodeInfo in nodeInfos)
                            {
                                NSNodeInfo nsNodeInfo = GetDefaultNSNodeInfo(nodeInfo, taskId, instancId);
                                Routing(template, nodeInfo, nsNodeInfo, context, parallelNodes);
                            }
                        }
                    }
                    else
                        StopAll(groupName, template, nodeInfoPass, taskId, instancId, context, parallelNodes);
                }
            }
        }


        private static NSNodeInfo GetDefaultNSNodeInfo(XElement xElement, Guid taskId, Guid insanceId)
        {
            NSNodeInfo nsNodeInfo = new NSNodeInfo();
            nsNodeInfo.Id = Guid.NewGuid();
            nsNodeInfo.InstanceId = insanceId;
            nsNodeInfo.TaskId = taskId;
            nsNodeInfo.NodeName = xElement.Element("Name").Value;
            nsNodeInfo.NodeId = Guid.Parse(xElement.Element("ID").Value);
            nsNodeInfo.RunState = RunStateConst.RUNNING;
            nsNodeInfo.CreateTime = DateTime.Now;

            return nsNodeInfo;
        }


        private static void GetFirstParalle(XElement template, Guid nodeId, ref XElement parallelStart)
        {
            // 查找子节点中的第一并发
            var custs = from c in template.Elements("Connections").First().Elements("Connection")
                        where c.Element("SinkID").Value == nodeId.ToString()
                        select c.Element("SourceID").Value;


            // 找到所有的父
            foreach (var item in custs)
            {
                // 获取节点名称
                var nodeInfos = from c in template.Elements("DesignerItems").Elements("DesignerItem")
                                where c.Element("ID").Value == item
                                select c;
                foreach (var nodeInfo in nodeInfos)
                {
                    Guid nextId = Guid.Parse(nodeInfo.Element("ID").Value);

                    if (nodeInfo.Element("BType").Value == "Process")
                    {
                        StandBookmark standardBookmark = new StandBookmark();
                        var obj = (StandBookmark)JsonHelper.JsonToObject(nodeInfo.Element("Details").Value, standardBookmark);

                        // 如果遇到并发的起点，说明是并发内部的并发
                        if (!obj.IsParallel)
                        {
                            GetFirstParalle(template, nextId, ref parallelStart);
                        }
                        else
                        {
                            parallelStart = nodeInfo;
                            break;
                        }
                    }
                    else
                    {
                        GetFirstParalle(template, nextId, ref parallelStart);
                    }
                }
            }
        }


        /// <summary>
        /// TODO:暂停的只有主流程和子流程，而不能够暂停分支流程
        /// 驳回只能够驳回到主流程？
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="template"></param>
        /// <param name="nodeInfoPass"></param>
        /// <param name="taskId"></param>
        /// <param name="instancId"></param>
        /// <param name="context"></param>
        /// <param name="parallelNodes"></param>
        public static void FindAllNext(XElement template, XElement nodeInfoPass, Guid taskId, Guid instancId, MainWorkflowUnitOfWork context, HashSet<Guid> parallelNodes, HashSet<Guid> alreadyFinds)
        {
            Guid passNodeId = Guid.Parse(nodeInfoPass.Element("ID").Value);
            if (alreadyFinds.Contains(passNodeId))
                return;

            // 查找父节点中的第一个节点
            var custs = (from c in template.Elements("Connections").First().Elements("Connection")
                         where c.Element("SourceID").Value == nodeInfoPass.Element("ID").Value
                         select c.Element("SinkID").Value).ToList();

            // 获取节点名称
            var nodeInfoLinks = (from c in template.Elements("DesignerItems").Elements("DesignerItem")
                                 where custs.Contains(c.Element("ID").Value)
                                 select c).ToList();

            foreach (var item in nodeInfoLinks)
            {
                Guid nodeId = Guid.Parse(item.Element("ID").Value);
                alreadyFinds.Add(nodeId);
                if (item.Element("BType").Value == "Process")
                {
                    parallelNodes.Add(nodeId);
                }
                FindAllNext(template, item, taskId, instancId, context, parallelNodes, alreadyFinds);
            }
        }


        public static void FinishSubRouting<T>(TransInfoDto transDto)
        {
            var sqlDbContext = transDto.Context as MainWorkflowUnitOfWork;
            var newActivities = new HashSet<Guid>();
            var nextActivities = XmlHelper.GetAllNextActivities(transDto.TemplateXml, transDto.InstanceNodeId);
            nextActivities.ToList().ForEach(p =>
            {
                newActivities.Add(Guid.Parse(p.Element(ActivityConst.ID).Value));
            });
            // 回到父流程节点
            var finisheds = TransRoutingHelper.InstanceFinished(newActivities, transDto.TaskId, transDto.InstanceId, sqlDbContext);
            // 新生成的节点
            foreach (var item in finisheds)
            {

                // 首先获取模板上面的ID
                var parentNode = sqlDbContext.NSNodeInfos.First(p => p.TaskId == transDto.TaskId && p.InstanceId == transDto.InstanceId && p.Id == transDto.ParentId);

                if (parentNode != null)
                {
                    var instnace = sqlDbContext.NsInstanceInfos.First(p => p.Id == transDto.InstanceId);
                    XElement parentDoc = XElement.Parse(sqlDbContext.NSTemplateInfos.First(p => p.Id == instnace.TemplateId).TemplateText);
                    // 获取节点名称
                    var nodeInfos = from c in parentDoc.Elements("DesignerItems").Elements("DesignerItem")
                                    where c.Element("ID").Value == parentNode.NodeId.ToString()
                                    select c;

                    foreach (var nodeInfo in nodeInfos)
                    {
                        TransRouting.Routing(parentDoc, nodeInfo, parentNode, sqlDbContext, newActivities);
                    }
                }
            }
        }
    }
}
