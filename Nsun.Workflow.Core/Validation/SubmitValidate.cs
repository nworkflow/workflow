using Nsun.Tools.Common;
using Nsun.Workflow.Core.Dtos;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Validation
{
    public class SubmitValidate : ISubmitValidation
    {
        public string ValidateMsg(string submitMsg, TransInfoDto dto)
        {
            // 如果是以命令开头
            if (submitMsg.StartsWith("_"))
            {
                if (submitMsg == SubmitTypeEnum._SP.ToString())
                {
                    List<Guid> searchNodes = new List<Guid>();
                    var nodeId = Guid.Parse(XmlHelper.GetSafeValue(dto.Activity, ActivityConst.ID));
                    searchNodes.Add(nodeId);
                    Stack<string> parallelStarts = new Stack<string>();
                    Stack<string> parallelEnds = new Stack<string>();
                    bool? stopParallelValidate = null;
                    // 检查是否有起始的节点
                    Action<XElement> next = null;
                    Action<XElement> forward = null;

                    forward = (e) =>
                    {
                        var parentActivities = XmlHelper.GetAllForwardActivities(dto.TemplateXml, nodeId);
                        foreach (var parentActivity in parentActivities)
                        {
                            var id = Guid.Parse(XmlHelper.GetSafeValue(parentActivity, ActivityConst.ID));

                            string bType = XmlHelper.GetSafeValue(parentActivity, ActivityConst.TYPE);
                            if (bType == ActivityTypeEnum.Process.ToString())
                            {
                                StandBookmark bookmark = JsonHelper.JsonToT<StandBookmark>(XmlHelper.GetSafeValue(parentActivity, ActivityConst.DETAILS));
                                if (bookmark.IsParallel)
                                {
                                    if (parallelEnds.Contains(bookmark.GroupName))
                                    {
                                        parallelEnds.Pop();
                                    }
                                    else
                                    {
                                        if (parallelEnds.Count() == 0)
                                            stopParallelValidate = true;
                                    }
                                }
                            }
                            else if (bType == ActivityTypeEnum.Parallel.ToString())
                            {
                                var groupName = XmlHelper.GetSafeValue(parentActivity, ActivityConst.GROUPNAME);
                                if (parallelEnds.Contains(XmlHelper.GetSafeValue(parentActivity, ActivityConst.GROUPNAME)))
                                {
                                    parallelEnds.Push(groupName);
                                    break;
                                }
                            }

                            if (!stopParallelValidate.HasValue && !searchNodes.Contains(id))
                            {
                                searchNodes.Add(id);
                                forward(parentActivity);
                            }
                        }
                    };

                    forward(dto.Activity);

                    if (stopParallelValidate != true)
                    {
                        return "该节点不支持并发结束";
                    }
                    return string.Empty;
                }
                else if (submitMsg.StartsWith(SubmitTypeEnum._BK.ToString()))
                {
                    if (submitMsg.Split(':').Count() == 2)
                        return string.Empty;
                    else
                        return "退回必须要填写退回的节点名称！";
                }
                else if (submitMsg.StartsWith(SubmitTypeEnum._SB.ToString()) || submitMsg.StartsWith(SubmitTypeEnum._SP.ToString()))
                    return string.Empty;
                else return "提交结构不合法！";
            }

            return string.Empty;
        }
    }
}
