using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using Nsun.Tools.Common;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Persistence;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Nsun.Workflow.Core.Activities;
using Nsun.Workflow.Core.Routing;
using System.Diagnostics;

namespace Nsun.Workflow.Core.Models
{
    public class StandBookmark : BookmarkBase
    {
        public StandBookmark()
        {
            NodeType = "Process";
            Size = new KeyValuePair<int, int>(130, 25);
            CanPersistence = true;
        }

        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.ReadOnly(true)]
        public override bool IsPersistence
        {
            get
            {
                return base.IsPersistence;
            }
            set
            {
                base.IsPersistence = value;
            }
        }

        private bool _isParallel;
        public bool IsParallel
        {
            get { return _isParallel; }
            set
            {
                _isParallel = value;
                RaisePropertyChanged("IsParallel");
            }
        }

        private string _submitOptions;
        public string SubmitOptions
        {
            get { return _submitOptions; }
            set
            {
                _submitOptions = value;
                RaisePropertyChanged("SubmitOptions");
            }
        }

        private string _transUserNo;
        [System.ComponentModel.DisplayName("办理人员")]
        public string TransUserNo
        {
            get { return _transUserNo; }
            set
            {
                _transUserNo = value;
                RaisePropertyChanged("TransUserNo");
            }
        }

        private string _nodeDes;
        [System.ComponentModel.DisplayName("节点描述")]
        public string NodeDes
        {
            get { return _nodeDes; }
            set
            {
                _nodeDes = value;
                RaisePropertyChanged("NodeDes");
            }
        }

        public override void StartBuiness(Dtos.TransInfoDto transInfoDto)
        {   
            // 初始化
            
                var activityInfo = JsonHelper.JsonToT<StandBookmark>(XmlHelper.GetSafeValue(transInfoDto.Activity, ActivityConst.DETAILS));
                this.NodeID = activityInfo.NodeID;
                this.NodeDes = activityInfo.NodeDes;
                this.GroupName = activityInfo.GroupName;
                this.CanPersistence = activityInfo.CanPersistence;
                this.TransUserNo = activityInfo.TransUserNo;
                this.SubmitOptions = activityInfo.SubmitOptions;
                this.Name = activityInfo.Name;
             

            // 生成新节点
            NSNodeInfo newActivity = new NSNodeInfo();
            newActivity.Id = Guid.NewGuid();
            newActivity.NodeName = activityInfo.Name;
            newActivity.NodeId = this.NodeID.Value;
            newActivity.InstanceId = transInfoDto.InstanceId;
            newActivity.ParentId = transInfoDto.ParentId;
            newActivity.RunState = EnumExt.RunStateConst.RUNNING;
            newActivity.SubmitOptions = SubmitOptions;
            newActivity.TaskId = transInfoDto.TaskId;
            newActivity.TransUserNo = TransUserNo;
            newActivity.CreateTime = DateTime.Now;
            newActivity.GroupName = GroupName;
            newActivity.NodeDes = NodeDes;

            // 保存节点
            transInfoDto.Persistence.CreateActivity(newActivity);

            // 调用其他附属信息
            base.StartBuiness(transInfoDto);
        }


        public override void EndBuiness(Dtos.TransInfoDto transInfoDto)
        {
            var copyDto = transInfoDto.GetCopyInfo();
            
            // 将当前的流程节点设置成end状态
            var currentActivity = copyDto.Persistence.GetActivity(transInfoDto.TaskId, copyDto.InstanceId, copyDto.InstanceNodeId);
            if (currentActivity != null)
            {
                copyDto.Persistence.FinishActivity(currentActivity);
            }

            var nextActivities = XmlHelper.GetAllNextActivities(copyDto.TemplateXml, copyDto.InstanceNodeId);
            // 启动新流程，记录到流程的ID，任务ID等信息
            if (nextActivities != null)
            {
                copyDto.NextActivities = nextActivities.ToList();
            }


            base.EndBuiness(copyDto);
        }


        public override string GetSerialContent()
        {
            return base.GetSerialContent(this.GetType());
        }
    }
}
