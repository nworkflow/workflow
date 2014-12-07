using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Tools.Common;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.Persistence;
using Nsun.Workflow.Core.Routing;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Activities
{
    public class SubRoutingsBookmark : BookmarkBase
    {
        public SubRoutingsBookmark()
        {
            this.NodeType = "SubRoutings";
            this.Size = new KeyValuePair<int, int>(100, 24);
            this.CanPersistence = false;
        }

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

        public override string GetSerialContent()
        {
            return base.GetSerialContent(this.GetType());
        }
        

        public override void StartBuiness(Dtos.TransInfoDto transInfo)
        {
            // 启动新流程，记录到流程的ID，任务ID等信息
            IPersistence p = transInfo.Persistence;
            SubRoutingsBookmark submitRouting = JsonHelper.JsonToT<SubRoutingsBookmark>(XmlHelper.GetSafeValue(transInfo.Activity, ActivityConst.DETAILS));
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
            nsRoutingInfo.ParentId = nsNodeInfo.Id;
            nsRoutingInfo.InstanceId = Guid.NewGuid();
            nsRoutingInfo.TaskId = transInfo.TaskId;
            nsRoutingInfo.GroupName = ActivityConst.GROUPNAME_INSTANCE;
            nsRoutingInfo.GroupCounter = transInfo.GroupCounter;
  
            p.CreateRoutingInfo(nsRoutingInfo);

            for (int i = 0; i < transInfo.GroupCounter; i++)
            {
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
            }

            base.StartBuiness(transInfo);
        }
    }
}
