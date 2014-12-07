using Nsun.Tools.Common;
using Nsun.Workflow.Core.Activities;
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
    public class DesignRuleValidate : ITemplateValidation
    {
        public string ValidateMsg(XElement doc, List<string> runStates,ref Dictionary<string,Stack<string>> errorInfos)
        {
            try
            {
                errorInfos = new Dictionary<string, Stack<string>>();
                StringBuilder msg = new StringBuilder();
                var nodeInfos = XmlHelper.GetAllActivitiesByType(doc, EnumExt.ActivityTypeEnum.Start);

                if (nodeInfos.Count() != 1)
                    msg.AppendMsg("模板必须有起始节点");
 
                int withoutName = 0;

                #region 并发

                Action<XElement> exc = null;
                Stack<string> parallelStacks = new Stack<string>();
                exc = (e) =>
                {
                    runStates.Add(e.ParseByName());
                    // 检查节点名称
                    if (string.IsNullOrEmpty(e.ParseByName()))
                        withoutName++;

                    // 获取并发起点
                    var activityType = e.ParseByActivityType();
                     
                    if (activityType == EnumExt.ActivityTypeEnum.SubRouting.ToString())
                    {
                        // 检验是否填写子流程模板
                        var bookmark = JsonHelper.JsonToT<SubRoutingBookmark>(e.ParseByDetails());
                        // 检验是否填写子流程模板
                        if (string.IsNullOrEmpty(bookmark.TemplateName))
                            msg.AppendMsg(string.Format("节点[{0}]必须填写子模板", e.ParseByName()));
                    }
                    // 验证多子流程是否填写模板
                    else if (activityType == EnumExt.ActivityTypeEnum.SubRoutings.ToString())
                    {
                        var bookmark = JsonHelper.JsonToT<SubRoutingsBookmark>(e.ParseByDetails());
                        // 检验是否填写子流程模板
                        if (string.IsNullOrEmpty(bookmark.TemplateName))
                            msg.AppendMsg(string.Format("节点[{0}]必须填写子模板", e.ParseByName()));
                    }
                };

                XmlHelper.TraversingGraph(doc, exc);
                if (withoutName != 0)
                    msg.AppendMsg(string.Format("存在{0}个节点没有填写节点名称", withoutName));

                if (parallelStacks.Count() != 0)
                {
                    errorInfos.Add(ExeResultConst.NoParallelEnds, parallelStacks);
                    msg.AppendMsg(string.Format("并发节点必须有匹配的结束并发"));
                }

                #endregion (并发)
                return msg.ToString();
            }
            catch (Exception ex)
            {
                return string.Format(ex.Message);
            }

            return string.Empty;
        }
    }
}
