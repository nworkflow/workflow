using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Workflow.Core.Routing;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Models
{
    public sealed class ParalleBookmark : BookmarkBase
    {
        private string _parallelGroup;

        public string ParallelGroup
        {
            get { return _parallelGroup; }
            set
            {
                _parallelGroup = value;
                RaisePropertyChanged("ParallelGroup");
            }
        }

       

        public ParalleBookmark()
        {
            this.NodeType = "Parallel";
            this.Size = new KeyValuePair<int, int>(80,20);
            this.CanPersistence = false;
            this.IsPersistence = false;
        }


        public override void StartBuiness(Dtos.TransInfoDto transInfoDto)
        {
            if (transInfoDto.Activity == null)
                throw new Exception("并发结束节点解析有误！");

            var groupName = XmlHelper.GetSafeValue(transInfoDto.Activity, ActivityConst.GROUPNAME);

            // 生成记录节点
            var routingInfo = transInfoDto.Persistence.CreateParallelRoutingInfo(transInfoDto.TaskId, transInfoDto.InstanceId, groupName);
            var startParalle = XmlHelper.GetAllActivitiesByType(XElement.Parse(transInfoDto.TemplateXml), EnumExt.ActivityTypeEnum.Process).First(p => XmlHelper.GetSafeValue(p, ActivityConst.GROUPNAME) == groupName);
            var nextActivityCounts = XmlHelper.GetAllNextActivities(transInfoDto.TemplateXml, Guid.Parse(XmlHelper.GetSafeValue(startParalle, ActivityConst.ID)));
            if (nextActivityCounts != null)
            {
                if (routingInfo.GroupCounter == nextActivityCounts.Count())
                {
                    var copyInfo = transInfoDto.GetCopyInfo();
                    copyInfo.GroupName = groupName;
                    routingInfo.Finshed = true;
                    End(copyInfo);
                }
            }

            base.StartBuiness(transInfoDto);
        }


        public override void EndBuiness(Dtos.TransInfoDto transInfoDto)
        {
            var newAcivities = XmlHelper.GetAllNextActivities(transInfoDto.TemplateXml, Guid.Parse(XmlHelper.GetSafeValue(transInfoDto.Activity, ActivityConst.ID)));
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
