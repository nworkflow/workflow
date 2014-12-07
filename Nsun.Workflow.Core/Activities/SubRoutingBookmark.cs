using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Tools.Common;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.Persistence;
using Nsun.Workflow.Core.Routing;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Activities
{
    public sealed class SubRoutingBookmark : BookmarkBase
    {
        private string _templateName;
        [System.ComponentModel.DisplayName("模板名称")]
        public string TemplateName
        {
            get { return _templateName; }
            set
            {
                _templateName = value;
                RaisePropertyChanged("TemplateName");
            }
        }

        private Guid _templateId;
        [System.ComponentModel.Browsable(false)]
        public Guid TemplateId
        {
            get { return _templateId; }
            set
            {
                _templateId = value;
                RaisePropertyChanged("TemplateId");
            }
        }

  
        public SubRoutingBookmark()
        {
            this.NodeType = "SubRouting";
            this.Size = new KeyValuePair<int, int>(100, 24);
            this.CanPersistence = false;
        }


        public override string GetSerialContent()
        {
            return base.GetSerialContent(this.GetType());
        }
        

        public override void StartBuiness(TransInfoDto transInfo)
        {
                // 启动新流程，记录到流程的ID，任务ID等信息
                IPersistence p = transInfo.Persistence;
                SubRoutingBookmark submitRouting = JsonHelper.JsonToT<SubRoutingBookmark>(XmlHelper.GetSafeValue(transInfo.Activity, ActivityConst.DETAILS));
                this.TemplateName = submitRouting.TemplateName;
                this.TemplateId = submitRouting.TemplateId;
                this.Name = submitRouting.Name;

                // 生成回归点
                NSNodeInfo nsNodeInfo = new NSNodeInfo();
                nsNodeInfo.Id = Guid.NewGuid();
                nsNodeInfo.TaskId = transInfo.TaskId;
                nsNodeInfo.InstanceId = transInfo.InstanceId;
                nsNodeInfo.NodeName = this.Name;
                nsNodeInfo.ParentId = transInfo.ParentId;
                nsNodeInfo.RunState = EnumExt.RunStateConst.RUNNING;
                nsNodeInfo.CreateTime = DateTime.Now;
                nsNodeInfo.NodeId = Guid.Parse(XmlHelper.GetSafeValue(transInfo.Activity, ActivityConst.ID));
                p.CreateActivity(nsNodeInfo);

                // 生成路由信息
                NSRoutingInfo nsRoutingInfo = new NSRoutingInfo();
                nsRoutingInfo.Id = Guid.NewGuid();
                nsRoutingInfo.InstanceId = transInfo.InstanceId;
                nsRoutingInfo.ParentId = nsNodeInfo.Id;
                nsRoutingInfo.TaskId = transInfo.TaskId;
                nsRoutingInfo.GroupName = ActivityConst.GROUPNAME_INSTANCE;
                nsRoutingInfo.GroupCounter = 1;
                p.CreateRoutingInfo(nsRoutingInfo);

                // 流程实例信息
                NSInstanceInfo insanceInfo = new NSInstanceInfo();
                insanceInfo.InstanceName = submitRouting.TemplateName;
                insanceInfo.RunState = EnumExt.RunStateConst.RUNNING;
                insanceInfo.StartTime = DateTime.Now;
                insanceInfo.TemplateId = this.TemplateId;
                insanceInfo.Id = Guid.NewGuid();
                insanceInfo.TaskId = transInfo.TaskId;
                insanceInfo.TemplateName = this.TemplateName;
                insanceInfo.ParentNodeId = nsNodeInfo.Id;
                p.CreateInstance(insanceInfo);

                TransInfoDto copyInfo = transInfo.GetCopyInfo();
                copyInfo.InstanceId = insanceInfo.Id;

                copyInfo.TemplateXml = p.GetTemplateInfo(insanceInfo.TemplateId).TemplateText;
                // 生成子流程的第一个节点
                var startActivities = XmlHelper.GetTemplateFirstActivity(XElement.Parse(copyInfo.TemplateXml));
                if (startActivities != null)
                {
                    new RoutingHost().RoutingFactory(startActivities.ToList(), copyInfo);
                }
                base.StartBuiness(transInfo);
        }


        public override void EndBuiness(TransInfoDto transInfoDto)
        {
            // 启动新流程，记录到流程的ID，任务ID等信息
            IPersistence p = transInfoDto.Persistence;
            // 结束当前点状态
            NSNodeInfo nsNodeInfo = p.GetActivityByID(transInfoDto.InstanceNodeId);
            NSInstanceInfo nsInstance= p.GetInsanceInfo(nsNodeInfo.InstanceId);
            NSTemplateInfo nsTemplate = p.GetTemplateInfo(nsInstance.TemplateId);

            p.FinishActivity(nsNodeInfo);
            //TODO: 产生新的节点，如果在回归之后继续执行逻辑，则要有全局的传递，以后在进行添加
            var nextActivities = XmlHelper.GetAllNextActivities(nsTemplate.TemplateText, nsNodeInfo.NodeId);
            if (nextActivities != null)
            {
                new RoutingHost().RoutingFactory(nextActivities.ToList(), transInfoDto);
            }
            base.EndBuiness(transInfoDto);
        }
    }
}
