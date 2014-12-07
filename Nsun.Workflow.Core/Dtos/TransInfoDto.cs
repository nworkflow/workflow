using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Dtos
{
    /// <summary>
    /// 交互实体
    /// </summary>
    public class TransInfoDto
    {
        public Guid TaskId;
        public Guid InstanceId;
        public Guid ParentInstanceId;
        public Guid InstanceNodeId;
        public Guid ParentId;
        public string GroupName;
        public int GroupCounter;
        public ActivityTypeEnum ActivityType;
        public string Export;
        public string Import;
        public string TemplateXml;
        public XElement Activity;
        public List<XElement> NextActivities;
        public Dictionary<string, string> TransDic;
        public DbContext Context;
        public string Condition;
        public IPersistence Persistence;
        public SubmitTypeEnum SubmitType;
        public TransInfoDto GetCopyInfo()
        {
            TransInfoDto copyInfo = new TransInfoDto()
            {
                Export = this.Export,
                Context = this.Context,
                ActivityType = this.ActivityType,
                GroupCounter = this.GroupCounter,
                GroupName = this.GroupName,
                Import = this.Import,
                InstanceId = this.InstanceId,
                InstanceNodeId = this.InstanceNodeId,
                NextActivities = this.NextActivities,
                ParentId = this.ParentId,
                ParentInstanceId = this.ParentInstanceId,
                TaskId = this.TaskId,
                TemplateXml = this.TemplateXml,
                TransDic = this.TransDic,
                Persistence = this.Persistence,
                Activity = this.Activity,
                Condition = this.Condition,
                SubmitType=this.SubmitType
            };
            return copyInfo;
        }
    }
}
