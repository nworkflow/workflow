using Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Nsun.Workflow.Common.Utils;
using System.Data.Entity.Validation;
using Nsun.Workflow.Core.Validation;
using Nsun.Workflow.Common.DTOs;
using Nsun.Tools.Logger;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Utils;
using Nsun.Workflow.Core;
using Nsun.Workflow.Core.Persistence;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.Routing;

namespace Nsun.Workflow.Common
{
    /// <summary>
    /// &copyRight leozhao nusn.ltd.
    /// </summary>
    public partial interface IService1
    {
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="startInfo">启动信息</param>
        /// <param name="errorInfos">错误信息</param>
        /// <returns></returns>
        [OperationContract]
        Guid StartWorkflowInfo(StartInfo_DTO startInfo, ref string errorInfos);

        /// <summary>
        /// 获取当前任务下的所有节点
        /// </summary>
        /// <param name="taskId">任务DI</param>
        /// <returns></returns>
        [OperationContract]
        List<NSInstanceInfo> GetNSInfoInfosByTaskId(Guid taskId);

        /// <summary>
        /// 获取当前实例下的所有节点
        /// </summary>
        /// <param name="instanceId">实例ID</param>
        /// <returns></returns>
        [OperationContract]
        List<NSNodeInfo> GetNSNodeInfosByInstanceId(Guid instanceId);

        [OperationContract]
        List<NSRoutingData> GetNSRouingData(Guid id);

        /// <summary>
        /// 提交流程
        /// </summary>
        /// <param name="submitInfo">提交信息</param>
        /// <returns></returns>
        [OperationContract]
        string SubmitWorkflow(SubmitInfo_DTO submitInfo);

        /// <summary>
        /// 获取模板上的所有节点（可持久的）
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <returns></returns>
        [OperationContract]
        List<InstanceNode_DTO> GetNodesByTemplateId(Guid templateId);


        [OperationContract]
        NSTemplateInfo GetTemplateByIds(Guid templateId);
       
    }


    public partial class Service1 : IService1
    {
        private ISubmitValidation _iSubmitValidation = new SubmitValidate();
        public Guid StartWorkflowInfo(StartInfo_DTO startInfo, ref string errorInfos)
        {
            try
            {
                HashSet<Guid> newActivities = new HashSet<Guid>();

                NSTaskInfo nsTaskInfo = new NSTaskInfo();
                nsTaskInfo.Id = Guid.NewGuid();
                nsTaskInfo.TaskName = startInfo.TaskName;
                nsTaskInfo.TaskType = startInfo.TaskType;
                nsTaskInfo.RunState = RunStateConst.RUNNING;
                nsTaskInfo.CreateTime = DateTime.Now;

                NSInstanceInfo nsIntanceInfo = new NSInstanceInfo();
                nsIntanceInfo.Id = Guid.NewGuid();
                nsIntanceInfo.TaskId = nsTaskInfo.Id;
                nsIntanceInfo.StartTime = DateTime.Now;
                nsIntanceInfo.TemplateId = startInfo.TemplateId;
                nsIntanceInfo.TemplateName = startInfo.TemplateName;
                nsIntanceInfo.RunState = RunStateConst.RUNNING;

                // StartWorkflow 
                TemplateRepository t = new TemplateRepository(Context);
                NSTemplateInfo templateInfo = t.Get(startInfo.TemplateId);
                if (templateInfo == null)
                {
                    errorInfos = "模板不能为空！";
                    return Guid.Empty;
                }

                string templateXML = templateInfo.TemplateText;
                XElement doc = XElement.Parse(templateInfo.TemplateText);
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
                        nsNodeInfo.InstanceId = nsIntanceInfo.Id;
                        nsNodeInfo.TaskId = nsTaskInfo.Id;
                        nsNodeInfo.NodeId = Guid.Parse(item.ToString());
                        nsNodeInfo.NodeName = nodeInfo.First().Element("Name").Value;
                        nsNodeInfo.NodeId = Guid.Parse(nodeInfo.First().Element("ID").Value);
                        nsNodeInfo.RunState = RunStateConst.RUNNING;
                        nsNodeInfo.CreateTime = DateTime.Now;
                        Context.NSNodeInfos.Add(nsNodeInfo);
                    }
                    else
                    {
                        // only prepare taskid and instanceid
                        NSNodeInfo nsNodeInfo = new NSNodeInfo();
                        nsNodeInfo.Id = Guid.NewGuid();
                        nsNodeInfo.InstanceId = nsIntanceInfo.Id;
                        nsNodeInfo.TaskId = nsTaskInfo.Id;
                        // running routting
                        TransRouting.Routing(doc, nodeInfo.First(), nsNodeInfo, Context,newActivities);
                    }
                }

                Context.NsInstanceInfos.Add(nsIntanceInfo);
                Context.NsTaskInfos.Add(nsTaskInfo);
                Context.SaveChanges();

                errorInfos = string.Empty;
                return nsTaskInfo.Id;
            }
            catch (Exception e)
            {
                errorInfos = e.ToString();
                return Guid.Empty;
            }

            return Guid.Empty;
        }
        private string RUNNING = RunStateConst.RUNNING;
        private static Locker _locker;
        public static Locker @Locker
        {
            get
            {
                if (_locker == null)
                    _locker = new Locker(60000);
                return _locker;
            }
        }


        private void StopAllRuningNode(Guid nodeId)
        {
        }


        public List<NSInstanceInfo> GetNSInfoInfosByTaskId(Guid taskId)
        {
            InstanceInfoRepository iir = new InstanceInfoRepository(Context);
            return iir.GetNSInstanceInfosByTaskId(taskId);
        }


        public List<NSNodeInfo> GetNSNodeInfosByInstanceId(Guid instanceId)
        {
            NodeInfoRepository nir = new NodeInfoRepository(Context);
            return nir.GetNSNodeInfosByInstanceIdAndRunState(instanceId, RunStateConst.RUNNING);
        }


//        public string SubmitWorkflow(SubmitInfo_DTO submitInfo)
//        {
//            if (submitInfo == null)
//                return "提交信息不能为空！";

//            if (!string.IsNullOrEmpty(_iSubmitValidation.ValidateMsg(submitInfo.SubmitResult)))
//                return _iSubmitValidation.ValidateMsg(submitInfo.SubmitResult);

//            if (!Locker.Add(submitInfo.NodeId))
//                return "请不要重复提交！";

//            // 读取模板
//            TemplateRepository tr = new TemplateRepository(Context);
//            InstanceInfoRepository iir = new InstanceInfoRepository(Context);
//            NodeInfoRepository nir = new NodeInfoRepository(Context);
//            HashSet<Guid> newActivities = new HashSet<Guid>();
//            var node = nir.Get(submitInfo.NodeId);

//            if (node != null)
//            {
//                node.SubmitResult = submitInfo.SubmitResult;
//                var instance = iir.Get(node.InstanceId);
//                if (instance != null)
//                {
//                    var templateInfo = tr.Get(instance.TemplateId);

//                    // 解析
//                    XElement doc = XElement.Parse(templateInfo.TemplateText);

//                    // 结束并发
//                    if (submitInfo.SubmitResult.StartsWith(SubmitTypeEnum._SP.ToString()))
//                    {

//                        // 停止并发
//                        TransRouting.StopParallel(doc, instance.TaskId, node, Context, node.NodeId);
//                    }
//                    // 驳回
//                    else if (submitInfo.SubmitResult.StartsWith(SubmitTypeEnum._BK.ToString()))
//                    {
//                        // 找到要驳回的点的名称
//                        string backNodeName = submitInfo.SubmitResult.Substring(4);

//                        // 在模板上找到的对应的点
//                        XElement templateXml = XElement.Parse(templateInfo.TemplateText);
//                        var nodeInfos = from c in templateXml.Elements("DesignerItems").Elements("DesignerItem")
//                                        where c.Element("Name").Value == backNodeName
//                                        select c;
//                        if (nodeInfos.Count() == 0)
//                            throw new Exception("驳回的点不存在！");
//                        // 结束该点下的所有点

//#warning 上一节点，下一个节点
//                        HashSet<Guid> stopAllNodes = new HashSet<Guid>();
//                        HashSet<Guid> hasAlreadyFinds = new HashSet<Guid>();
//                        TransRouting.FindAllNext(templateXml, nodeInfos.First(), node.TaskId, node.InstanceId, Context, stopAllNodes, hasAlreadyFinds);
//                        stopAllNodes.Add(node.NodeId);
//                        var stopNodeInfos = Context.NSNodeInfos.Where(p => p.InstanceId == node.InstanceId && p.RunState == RUNNING && stopAllNodes.Contains(p.NodeId));
//                        foreach (var nodeInfo in stopNodeInfos)
//                        {
//                            nodeInfo.RunState = RunStateEnum.end.ToString();
//                        }
//                        TransRouting.Routing(doc, nodeInfos.First(), node, Context, newActivities);
//                    }
//                    else
//                    {
//                        ///  查找驳回点并生成新的节点
//                        var custsExc = from c in doc.Elements("Connections").First().Elements("Connection")
//                                       where c.Element("SourceID").Value == node.NodeId.ToString()
//                                       select c.Element("SinkID").Value;

//                        // 结束当前的点
//                        node.RunState = RunStateEnum.end.ToString();
//                        nir.Modify(node);

//                        foreach (var item in custsExc)
//                        {
//                            // 获取节点名称
//                            var nodeInfos = from c in doc.Elements("DesignerItems").Elements("DesignerItem")
//                                            where c.Element("ID").Value == item
//                                            select c;
//                            foreach (var nodeInfo in nodeInfos)
//                            {
//                                TransRouting.Routing(doc, nodeInfo, node, Context, newActivities);
//                            }
//                        }
//                    }


//                    // 回到父流程节点
//                    var finisheds = TransRoutingHelper.InstanceFinished(newActivities, node.TaskId, node.InstanceId, Context);
//                    // 新生成的节点
//                    foreach (var item in finisheds)
//                    {

//                        // 首先获取模板上面的ID
//                        var parentNode = nir.Get(item);
                      
//                        if (parentNode != null)
//                        {
//                            var instnace = iir.Get(parentNode.InstanceId);
//                            XElement parentDoc =XElement.Parse(tr.Get(instance.TemplateId).TemplateText);
//                            // 获取节点名称
//                            var nodeInfos = from c in parentDoc.Elements("DesignerItems").Elements("DesignerItem")
//                                            where c.Element("ID").Value == parentNode.NodeId.ToString()
//                                            select c;
//                            foreach (var nodeInfo in nodeInfos)
//                            {
//                                TransRouting.Routing(doc, nodeInfo, parentNode, Context, newActivities);
//                            }
//                        }
//                    }
//                }
//            }

//            try
//            {
//                Context.SaveChanges();
//            }
//            catch (DbEntityValidationException e)
//            {
//                StringBuilder sb = new StringBuilder();
//                foreach (var eve in e.EntityValidationErrors)
//                {
//                    sb.Append(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
//                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
//                    foreach (var ve in eve.ValidationErrors)
//                    {
//                        sb.Append(string.Format("- Property: \"{0}\", Error: \"{1}\"",
//                            ve.PropertyName, ve.ErrorMessage));
//                    }
//                }
//                return sb.Append("持久化失败！").ToString();
//            }
//            catch (Exception ex)
//            {
               
//                return "提交失败";
//            }

//            return "提交成功！";
//        }

        public string SubmitWorkflow(SubmitInfo_DTO submitInfo)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                #region 验证
                if (submitInfo == null)
                    return "提交信息不能为空！";

                if (!Locker.Add(submitInfo.NodeId))
                    return "请不要重复提交！";
                #endregion (验证)

                #region 获取节点信息
                TransInfoDto transInfoDto = new TransInfoDto();
                transInfoDto.Persistence = DBFactory.GetPersistencePlugIn();
                transInfoDto.Context = transInfoDto.Persistence.Context;
                transInfoDto.GroupCounter = submitInfo.ExpandDatas == null ? 0 : submitInfo.ExpandDatas.Count();
                var activity = transInfoDto.Persistence.GetActivityByID(submitInfo.NodeId);
                if (activity == null)
                    return "要办理的节点已经不存在！";
                #endregion (获取节点信息)

                #region 获取模板信息
                var instance = transInfoDto.Persistence.GetInsanceInfo(activity.InstanceId);
                var template = transInfoDto.Persistence.GetTemplateInfo(instance.TemplateId);
                #endregion (获取模板信息)


                #region 生成DTO信息
                transInfoDto.TaskId = activity.TaskId;
                transInfoDto.InstanceId = activity.InstanceId;
                transInfoDto.InstanceNodeId = activity.NodeId;
                transInfoDto.TemplateXml = template.TemplateText;
                transInfoDto.ActivityType = ActivityTypeEnum.Process;
                transInfoDto.Condition = submitInfo.Condition;
                transInfoDto.SubmitType = EnumHelper.GetEnumByString<SubmitTypeEnum>(submitInfo.SubmitResult);
                #endregion (生成DTO信息)

                var currentActivity = XmlHelper.GetActivitiesByName(template.TemplateText, activity.NodeName).First();
                transInfoDto.Activity = currentActivity;
                if (!string.IsNullOrEmpty(_iSubmitValidation.ValidateMsg(submitInfo.SubmitResult, transInfoDto)))
                {
                    Locker.Remove(submitInfo.NodeId);
                    return _iSubmitValidation.ValidateMsg(submitInfo.SubmitResult, transInfoDto);
                }

                #region 提交
                new RoutingHost().EndRouing(transInfoDto);
                #endregion (提交)

                transInfoDto.Persistence.Context.SaveChanges();
            }
            catch (WFRoutingException rex)
            {
                LogHelper.WriteLog(rex.Message+rex.ToString());
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
            }
           
            return result.ToString();
        }

        public List<InstanceNode_DTO> GetNodesByTemplateId(Guid templateId)
        {
            List<InstanceNode_DTO> instanceNode_DTOs = null;

            try
            {
                var templateInfo = Context.NSTemplateInfos.FirstOrDefault(p => p.Id == templateId);
                if (templateInfo != null)
                {
                    instanceNode_DTOs = new List<InstanceNode_DTO>();
                    var desItems = XmlHelper.GetAllActivities(templateInfo.TemplateText);
                    desItems.ToList().ForEach(p =>
                    {
                        instanceNode_DTOs.Add(new InstanceNode_DTO()
                        {
                            ID = Guid.Parse(p.Element(ActivityConst.ID).Value),
                            Name = p.Element(ActivityConst.NAME).Value
                        });
                    });
                }
                else
                {
                    throw new Exception("模板不存在");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }


            return instanceNode_DTOs;
        }


        public List<NSRoutingData> GetNSRouingData(Guid id)
        {
            return this.Context.NSRoutingDatas.Where(p => p.Id == id).ToList();
        }


        public NSTemplateInfo GetTemplateByIds(Guid templateId)
        {
            return this.Context.NSTemplateInfos.FirstOrDefault(p => p.Id == templateId);
        }
    }
}
