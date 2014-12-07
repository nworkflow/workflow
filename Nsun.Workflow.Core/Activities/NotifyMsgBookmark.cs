using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.Routing;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Activities
{
    public class NotifyMsgBookmark : BookmarkBase
    {
        public int MsgCounter { get; private set; }
        private const string _groupStart = "Notify_";
        public NotifyMsgBookmark()
        {
            NodeType = "NotifyMsg";
            Size = new KeyValuePair<int, int>(130, 25);
        }

        public override void StartBuiness(Dtos.TransInfoDto transInfoDto)
        {
            var allParents = XmlHelper.GetAllForwardActivities(transInfoDto.TemplateXml, transInfoDto.InstanceNodeId);
            // 生成记录节点
            var routingInfo = transInfoDto.Persistence.CreateParallelRoutingInfo(transInfoDto.TaskId, transInfoDto.InstanceId, _groupStart + this.GroupName);

            if (routingInfo.GroupCounter == allParents.Count())
            {   
                var copyInfo = transInfoDto.GetCopyInfo();
                copyInfo.GroupName = _groupStart + copyInfo.GroupName;
                routingInfo.Finshed = true;

                End(copyInfo);
            }

            base.StartBuiness(transInfoDto);
        }


        public override void EndBuiness(Dtos.TransInfoDto transInfoDto)
        {
            var newAcivities = XmlHelper.GetAllNextActivities(transInfoDto.TemplateXml, transInfoDto.InstanceNodeId);
            if (newAcivities != null)
            {
                new RoutingHost().RoutingFactory(newAcivities.ToList(), transInfoDto);
            }
            base.EndBuiness(transInfoDto);
        }

        public override string GetSerialContent()
        {
            return base.GetSerialContent(this.GetType());
        }
    }
}
