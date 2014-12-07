using Nsun.Tools.Common;
using Nsun.Workflow.Core.Args;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Routing
{
    /// <summary>
    /// Routing 路由
    /// </summary>
    public class RoutingHost
    {
        /// <summary>
        /// 路由工厂
        /// </summary>
        /// <param name="dto"></param>
        public void RoutingFactory(TransInfoDto dto)
        {
            IBookmark bookmarkInfo = BookmarkFactory.GetBookmark(dto.ActivityType);
            bookmarkInfo.Start(dto);
        }


        /// <summary>
        /// 路由工厂
        /// </summary>
        /// <param name="newActivities">新的集合</param>
        /// <param name="dto">传输的实体</param>
        public void RoutingFactory(List<XElement> newActivities, TransInfoDto dto)
        {
            if (newActivities != null && newActivities.Count > 0)
            {
                newActivities.ToList().ForEach(p =>
                 {
                     var copyDto = dto.GetCopyInfo();
                     copyDto.InstanceNodeId = p.ParseById();
                     copyDto.Activity = p;
                     copyDto.GroupName = p.ParseByGroupName();
                     copyDto.ActivityType = EnumHelper.GetEnumByString<ActivityTypeEnum>(XmlHelper.GetSafeValue(p, ActivityConst.TYPE));
                     RoutingFactory(copyDto);
                 });
            }
            else if (newActivities != null && newActivities.Count() == 0)
            {
                EndInstance(dto);
            }
        }


        /// <summary>
        /// 结束流程
        /// </summary>
        /// <param name="dto">要用到的Dto</param>
        public void EndInstance(TransInfoDto dto)
        {
            // 结束当前流程
            var instance = dto.Persistence.FinishInstance(dto.InstanceId);
            // 查找是否需要回归到父流程
            if (instance.ParentNodeId.HasValue)
            {
                var backToParent = dto.Persistence.FinishRouting(dto.TaskId, instance.ParentNodeId.Value, ActivityConst.GROUPNAME_INSTANCE);
                // 如果不回归到主流程上
                if (backToParent == null)
                {
                    // 结束当前任务
                    dto.Persistence.FinishTask(dto.TaskId);
                }
                else
                {
                    // 回归到上一个点
                    var copyInfo = dto.GetCopyInfo();
                    copyInfo.InstanceId = backToParent.InstanceId;
                    copyInfo.ActivityType = ActivityTypeEnum.SubRouting;
                    copyInfo.InstanceNodeId = backToParent.Id;
                    EndRouing(copyInfo);
                }
            }
            else
            {
                // 结束任务
                dto.Persistence.FinishTask(dto.TaskId);
            }
        }


        /// <summary>
        /// 结束节点
        /// </summary>
        /// <param name="dto"></param>
        public void EndRouing(TransInfoDto dto)
        {
            IBookmark bookmarkInfo = BookmarkFactory.GetBookmark(dto.ActivityType);
            // 结束并发
            if (dto.SubmitType == SubmitTypeEnum._SP)
            {
                XElement parallelStart = null;
                // 获取对应的并发等待节点
                var doc = XElement.Parse(dto.TemplateXml);
                XmlHelper.GetFirstParalle(doc, dto.InstanceNodeId, ref parallelStart);

                if (parallelStart == null)
                    throw new Exception("没有找到并发起始节点");

                if (XmlHelper.HasValue(parallelStart, "GroupName"))
                {
                    HashSet<Guid> parallelNodes = new HashSet<Guid>();
                    Dictionary<string, int> parallelCount = new Dictionary<string, int>();
                    var copyInfo = dto.GetCopyInfo();
                    copyInfo.InstanceNodeId = Guid.Parse(XmlHelper.GetSafeValue(parallelStart, ActivityConst.ID));
                    parallelCount.Add(copyInfo.TaskId + XmlHelper.GetSafeValue(parallelStart, "GroupName"), 1);
                    XmlHelper.TraversingGraphParallel(doc, parallelStart.ParseById(), null);
                    StopAll(parallelStart.Element("GroupName").Value, copyInfo, parallelNodes, ref parallelCount);
                    dto.Persistence.FinishActivityByNodeIds(copyInfo.InstanceId, parallelNodes.ToList(), RunStateConst.STOP);
                }

                return;
            }

            bookmarkInfo.Ending += (s, r) =>
            {
                var args = r as WFArgs;
                if (args.Parameter.NextActivities != null)
                    RoutingFactory(args.Parameter.NextActivities, dto);
            };
            bookmarkInfo.End(dto);
        }


        /// <summary>
        /// 停止所有
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="dto">传输Dto</param>
        /// <param name="parallelNodes">并发内部节点</param>
        /// <param name="parallelCount">并发数量（并发内并发）</param>
        private void StopAll(string groupName, TransInfoDto dto, HashSet<Guid> parallelNodes, ref Dictionary<string, int> parallelCount)
        {
            var template = XElement.Parse(dto.TemplateXml);

            // 查找父节点中的第一个节点
            var custs = from c in template.Elements("Connections").First().Elements("Connection")
                        where c.Element("SourceID").Value == dto.InstanceNodeId.ToString()
                        select c.Element("SinkID").Value;

            // 获取节点名称
            var nodeInfoLinks = from c in template.Elements("DesignerItems").Elements("DesignerItem")
                                where custs.Contains(c.Element("ID").Value)
                                select c;

            foreach (var item in nodeInfoLinks)
            {
                var copyInfo = dto.GetCopyInfo();
                copyInfo.SubmitType = SubmitTypeEnum._SB;
                copyInfo.InstanceNodeId = item.ParseById();
                if (item.ParseByActivityType() == "Process")
                {
                    var activiy = JsonHelper.JsonToT<StandBookmark>(item.ParseByDetails());
                    if (activiy.IsParallel == true)
                    {
                        if (!parallelCount.Keys.Contains(copyInfo.TaskId + groupName))
                            parallelCount.Add(copyInfo.TaskId + groupName, nodeInfoLinks.Count());
                    }

                    parallelNodes.Add(copyInfo.InstanceNodeId);
                    StopAll(groupName, copyInfo, parallelNodes, ref parallelCount);
                }
                else
                {
                    if (item.ParseByActivityType() == "Parallel" && item.ParseByGroupName() == groupName)
                    {
                        copyInfo.GroupName = groupName;
                        parallelCount[copyInfo.TaskId + groupName] = parallelCount[copyInfo.TaskId + groupName] - 1;
                        if (parallelCount[copyInfo.TaskId + groupName] == 0)
                        {
                            parallelCount.Remove(copyInfo.TaskId + groupName);
                            copyInfo.Persistence.FinishParallel(copyInfo.TaskId, copyInfo.InstanceId, groupName);
                            var nextActivities = XmlHelper.GetAllNextActivities(copyInfo.TemplateXml, Guid.Parse(XmlHelper.GetSafeValue(item, ActivityConst.ID)));
                            if (nextActivities != null && nextActivities.Count() > 0)
                                RoutingFactory(nextActivities.ToList(), copyInfo);
                        }
                    }
                    else
                    {
                        parallelCount[copyInfo.TaskId + item.ParseByGroupName()] = parallelCount[copyInfo.TaskId + item.ParseByGroupName()] - 1;
                        if (parallelCount[copyInfo.TaskId + groupName] == 0)
                            StopAll(groupName, copyInfo, parallelNodes, ref parallelCount);
                    }
                }
            }
        }
    }
}
