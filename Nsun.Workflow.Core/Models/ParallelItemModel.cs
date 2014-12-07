using Nsun.Workflow.Core.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Models
{
    /// <summary>
    /// 并发处理项
    /// </summary>
    public class AciviyItem
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName
        {
            get;
            private set;
        }


        /// <summary>
        /// 节点ID
        /// </summary>
        public Guid ActivitId
        {
            get;
            private set;
        }

        /// <summary>
        /// 并发数量
        /// </summary>
        public int ParallelCount
        {
            get;
            private set;
        }


        /// <summary>
        /// 类型
        /// </summary>
        public ActivityTypeEnum ActivitType
        {
            get;
            private set;
        }

        public XElement Activit
        {
            get;
            private set;
        }

        /// <summary>
        /// 构造函数，初始化分组信息和数量
        /// </summary>
        /// <param name="groupName">并发分组名称</param>
        /// <param name="parallelCount">并发数量</param>
        public AciviyItem(Guid activityId,ActivityTypeEnum activityType,string groupName,XElement activity ,int parallelCount=0)
        {
            this.GroupName = groupName;
            this.ParallelCount = parallelCount;
            this.ActivitType = activityType;
            this.ActivitId = activityId;
            this.Activit = activity;
        }


        public bool Completed
        {
            get {return  ParallelCount == 0; }
        }

        public void Pop()
        {
            ParallelCount--;
        }
    }
}
