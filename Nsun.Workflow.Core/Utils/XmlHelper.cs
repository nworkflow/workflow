using Nsun.Tools.Common;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Utils
{
    public class XmlHelper
    {

        /// <summary>
        /// 是否有值
        /// </summary>
        /// <param name="xElement">XElement</param>
        /// <param name="element">要判断的值</param>
        /// <returns>结果</returns>
        public static bool HasValue(XElement xElement, string element)
        {
            if (xElement == null || xElement.Element(element) == null)
                return false;
            return true;
        }


        /// <summary>
        /// 获取值，不会抛出异常
        /// </summary>
        /// <param name="xElement">XElement</param>
        /// <param name="element">要判断的值</param>
        /// <returns>结果</returns>
        public static string GetSafeValue(XElement xElement, string element)
        {
            if (HasValue(xElement, element))
                return xElement.Element(element).Value;
            return string.Empty;
        }


        /// <summary>
        /// 根据节点名称获取节点列表
        /// </summary>
        /// <param name="templateXml">模板</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>查询结果集</returns>
        public static IEnumerable<XElement> GetActivitiesByName(string templateXml, string nodeName)
        {
            if (string.IsNullOrEmpty(templateXml))
                throw new Exception("模板不能为空");
            if (string.IsNullOrEmpty(nodeName))
                throw new Exception("节点名称不能为空");

            XElement template = XElement.Parse(templateXml);
            var nodeInfos = from c in template.Elements(ActivityConst.DESIGNERITEMS).Elements(ActivityConst.DESIGNERITEM)
                            where c.Element(ActivityConst.NAME).Value == nodeName
                            select c;

            return nodeInfos;
        }


        /// <summary>
        /// 获取模板上的所有活动
        /// </summary>
        /// <param name="templateXml"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> GetAllActivities(string templateXml)
        {
            if (string.IsNullOrEmpty(templateXml))
                throw new Exception("模板不能为空");

            XElement template = XElement.Parse(templateXml);
            var nodeInfos = from c in template.Elements(ActivityConst.DESIGNERITEMS).Elements(ActivityConst.DESIGNERITEM)
                            select c;

            return nodeInfos;
        }


        /// <summary>
        /// 通过类型获取模板下的所有活动
        /// </summary>
        /// <param name="doc">模板</param>
        /// <param name="activityTypeEnum">活动类型</param>
        /// <returns>查询的结果集合</returns>
        public static IEnumerable<XElement> GetAllActivitiesByType(XElement doc, ActivityTypeEnum activityTypeEnum)
        {
            var nodeInfos = from c in doc.Elements(ActivityConst.DESIGNERITEMS).Elements(ActivityConst.DESIGNERITEM)
                            where c.Element(ActivityConst.TYPE).Value == activityTypeEnum.ToString()
                            select c;

            return nodeInfos;
        }


        /// <summary>
        /// 获取模板的起始节点
        /// </summary>
        /// <param name="doc">模板</param>
        /// <returns>返回第一个节点</returns>
        public static XElement GetStartActivit(XElement doc)
        {
            return GetAllActivitiesByType(doc, ActivityTypeEnum.Start).FirstOrDefault();
        }

        public static IEnumerable<XElement> GetAllActivitys(XElement doc)
        {
            var nodeInfos = from c in doc.Elements(ActivityConst.DESIGNERITEMS).Elements(ActivityConst.DESIGNERITEM)
                            select c;
            return nodeInfos;
        }


        /// <summary>
        /// 获取所有连接
        /// </summary>
        /// <param name="doc">模板</param>
        /// <param name="nodeId">当前节点</param>
        /// <returns>当前节点对应的所有连接</returns>
        public static IEnumerable<XElement> GetAllLinkesById(XElement doc, Guid nodeId, bool parent = false)
        {
            var linkInfos = from c in doc.Elements(ActivityConst.CONNECTIONS).Elements(ActivityConst.CONNECTION)
                            where c.Element(ActivityConst.SOURCEID).Value == nodeId.ToString()
                            select c;

            return linkInfos;
        }


        public static XElement GetElementById(XElement doc, Guid nodeId)
        {
            return GetAllActivities(doc.ToString()).First(p => p.ParseById() == nodeId);
        }


        /// <summary>
        /// 获取所有上面节点的连接
        /// </summary>
        /// <param name="doc">模板</param>
        /// <param name="nodeId">当前节点</param>
        /// <returns>当前节点对应的所有连接</returns>
        public static IEnumerable<XElement> GetAllForwardLinkesById(XElement doc, Guid nodeId, bool parent = false)
        {
            var linkInfos = from c in doc.Elements(ActivityConst.CONNECTIONS).Elements(ActivityConst.CONNECTION)
                            where c.Element(ActivityConst.SINKID).Value == nodeId.ToString()
                            select c;

            return linkInfos;
        }


        /// <summary>
        /// 获取所有节点的后续节点
        /// </summary>
        /// <param name="templateXml"模板字符串></param>
        /// <param name="currentNodeId">当前节点ID</param>
        /// <returns>后续节点（Activities）特殊处理</returns>
        public static IEnumerable<XElement> GetAllNextActivities(string templateXml, Guid currentNodeId)
        {
            var doc = XElement.Parse(templateXml);
            var links = GetAllLinkesById(doc, currentNodeId).Select(p => p.Element(ActivityConst.SINKID).Value);
            return GetAllActivitys(doc).Where(p => links.Contains(p.Element(ActivityConst.ID).Value));
        }


        public static IEnumerable<XElement> GetAllForwardActivities(string templateXml, Guid currentNodeId)
        {
            var doc = XElement.Parse(templateXml);
            var sources = GetAllForwardLinkesById(doc, currentNodeId).Select(p => p.Element(ActivityConst.SOURCEID).Value);
            return GetAllActivitys(doc).Where(p => sources.Contains(p.Element(ActivityConst.ID).Value));
        }


        /// <summary>
        /// 根据条件获取条件上的对应活动
        /// </summary>
        /// <param name="templateXml">模板</param>
        /// <param name="currentNodeId">当前节点ID</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static IEnumerable<XElement> GetNextActivityByCondition(string templateXml, Guid currentNodeId, string condition)
        {
            if (string.IsNullOrEmpty(condition))
                return null;

            var doc = XElement.Parse(templateXml);
            List<XElement> nextActivities = new List<XElement>();
            var equalLinks = GetAllLinkesById(doc, currentNodeId).Where(p => XmlHelper.GetSafeValue(p, ActivityConst.CONDITION).Equals(condition));
            equalLinks.ToList().ForEach(p =>
            {
                nextActivities.Add(GetAllActivitys(doc).First(m => XmlHelper.GetSafeValue(m, ActivityConst.ID) == XmlHelper.GetSafeValue(p, ActivityConst.SINKID)));
            });
            return nextActivities;
        }


        /// <summary>
        /// 获取模板上的第一个节点
        /// </summary>
        /// <param name="doc">当前模板</param>
        /// <returns>获取下面的节点</returns>
        public static IEnumerable<XElement> GetTemplateFirstActivity(XElement doc)
        {
            var startActivity = GetTemplateStartActivity(doc);
            return GetAllNextActivities(doc.ToString(), Guid.Parse(GetSafeValue(startActivity, ActivityConst.ID)));
        }


        /// <summary>
        /// 获取起始节点
        /// </summary>
        /// <param name="doc">模板</param>
        /// <returns>查询结果</returns>
        public static XElement GetTemplateStartActivity(XElement doc)
        {
            return doc.Element(ActivityConst.DESIGNERITEMS).Elements(ActivityConst.DESIGNERITEM).First(p => p.Element(ActivityConst.TYPE).Value == ActivityTypeEnum.Start.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="nodeId"></param>
        /// <param name="parallelStart"></param>
        public static void GetFirstParalle(XElement template, Guid nodeId, ref XElement parallelStart)
        {
            // 查找子节点中的第一并发
            var custs = from c in template.Elements("Connections").First().Elements("Connection")
                        where c.Element("SinkID").Value == nodeId.ToString()
                        select c.Element("SourceID").Value;


            // 找到所有的父
            foreach (var item in custs)
            {
                // 获取节点名称
                var nodeInfos = from c in template.Elements("DesignerItems").Elements("DesignerItem")
                                where c.Element("ID").Value == item
                                select c;
                foreach (var nodeInfo in nodeInfos)
                {
                    Guid nextId = Guid.Parse(nodeInfo.Element("ID").Value);

                    if (nodeInfo.Element("BType").Value == "Process")
                    {
                        var obj = (StandBookmark)JsonHelper.JsonToT<StandBookmark>(nodeInfo.Element("Details").Value);

                        // 如果遇到并发的起点，说明是并发内部的并发
                        if (!obj.IsParallel)
                        {
                            GetFirstParalle(template, nextId, ref parallelStart);
                        }
                        else
                        {
                            parallelStart = nodeInfo;
                            break;
                        }
                    }
                    else
                    {
                        GetFirstParalle(template, nextId, ref parallelStart);
                    }
                }
            }
        }


        public static void TraversingGraphExt(XElement doc, Action<XElement> exc = null)
        {
            string templateXml = doc.ToString();
            // 控制并发的节点
            Queue<AciviyItem> _parallelStack = new Queue<AciviyItem>();
            HashSet<Guid> nodeIds = new HashSet<Guid>();
            XElement firstActivity = GetAllActivitiesByType(doc, ActivityTypeEnum.Start).FirstOrDefault();
            if (firstActivity == null)
            {
                throw new WFDesignException("起点节点不能为空！");
            }

            Action<XElement> transingGraphAct = null;
            var firstReallActivity = GetAllNextActivities(templateXml, firstActivity.ParseById()).First();
            if (firstReallActivity == null)
                return;

            transingGraphAct = (e) =>
            {
                Guid currentId = e.ParseById();
                // 如果在集合中已经存在，则判断是否为并发结束
                // 当前节点类型
                var activityType = EnumHelper.GetEnumByString<ActivityTypeEnum>(e.ParseByActivityType());
                if (nodeIds.Contains(currentId))
                {
                    return;
                }

                if (exc != null)
                    exc(e);

                // 防止重复的集合
                nodeIds.Add(currentId);

                // 所有下一个节点的查询集合
                var allNextActivities = GetAllNextActivities(templateXml, currentId).ToList();

                // 首次循环开关，用于控制并发执行第一个节点之后，其他的节点进入队列排队
                allNextActivities.ForEach(p =>
                {
                    transingGraphAct(p);
                });
            };

            // 启动深度查找
            transingGraphAct(firstReallActivity);
        }


        /// <summary>
        /// 遍历流程图【深度优先算法】
        /// </summary>
        /// <param name="doc">模板doc</param>
        /// <param name="exc">扩展执行</param>
        public static void TraversingGraph(XElement doc, Action<XElement> exc = null)
        {
            // 控制并发的节点
            Stack<AciviyItem> _parallelStack = new Stack<AciviyItem>();
            string templateXml = doc.ToString();
            HashSet<Guid> nodeIds = new HashSet<Guid>();
            XElement firstActivity = GetAllActivitiesByType(doc, ActivityTypeEnum.Start).FirstOrDefault();
            if (firstActivity == null)
            {
                throw new WFDesignException("起点节点不能为空！");
            }

            Action<XElement> transingGraphAct = null;
            var firstReallActivity = GetAllNextActivities(templateXml, firstActivity.ParseById()).First();
            if (firstReallActivity == null)
                return;

            transingGraphAct = (e) =>
            {
                if (e == null)
                    return;
                Guid currentId = e.ParseById();
                // 如果在集合中已经存在，则判断是否为并发结束
                // 当前节点类型
                var activityType = EnumHelper.GetEnumByString<ActivityTypeEnum>(e.ParseByActivityType());
                var groupName = e.ParseByGroupName();

                #region 重复处理
                if (nodeIds.Contains(currentId))
                {
                    // 如果是并发结束
                    if (activityType == ActivityTypeEnum.Parallel)
                    {
                        // 走过并发之后，将并发总数量--
                        var activity = _parallelStack.Where(p => p.GroupName == e.ParseByName()).FirstOrDefault(); // 如果不存在，则说明模板设置错误
                        if (activity == null)
                            throw new WFDesignException(string.Format("并发节点[{0}]没有匹配的分组[{1}]", e.ParseByName(), groupName));
                        activity.Pop();
                        if (activity.Completed)
                        {
                            // 如果并发没有完成
                            if (_parallelStack.Count() > 0)
                            {
                                // 将并发结束移除队列
                                var nextActivity = _parallelStack.Pop();
                                // 获取并发的所有下一个节点
                                var nextActivities = GetAllNextActivities(templateXml, currentId);
                                // 深度查找
                                nextActivities.ToList().ForEach(p =>
                                {
                                    // 继续深度递归
                                    transingGraphAct(p);
                                });
                                return;
                            }
                        }
                        else
                        {
                            // 并发没有完成的情况
                            if (_parallelStack.Count() > 0)
                            {
                                // 将并发数量--
                                var first = _parallelStack.Where(p => p.ActivitId == currentId).FirstOrDefault();
                                if (first == null)
                                    throw new WFDesignException(string.Format("并发节点[{0}]没有匹配的分组[{1}]", e.ParseByName(), groupName));
                                if (first.ParallelCount > 0)
                                    first.Pop();

                                // 如果并发没有完成
                                if (_parallelStack.Count() > 0)
                                {
                                    // 将并发结束移除队列
                                    _parallelStack.Pop();
                                    // 查找下一个节点【并发中的其他节点】
                                    var nextActivity = _parallelStack.First();
                                    // 继续深度递归
                                    transingGraphAct(nextActivity.Activit);
                                    return;
                                }
                            }
                        }
                    }

                    return;
                }

                #endregion(重复处理)

                #region 当前节点处理

                // 防止重复的集合
                nodeIds.Add(currentId);

                // 所有下一个节点的查询集合
                var allNextActivities = GetAllNextActivities(templateXml, currentId).ToList();

                // 是否是并发起点的开关
                bool isParallelStart = false;
                // 如果当前节点类型是标准节点
                if (activityType == ActivityTypeEnum.Process)
                {
                    var sbookmark = JsonHelper.JsonToT<StandBookmark>(e.ParseByDetails());
                    // 标准节点-->并发起点
                    if (sbookmark.IsParallel == true)
                    {
                        // 设置开关开
                        isParallelStart = true;
                        if (allNextActivities.Count <= 1)
                            throw new WFDesignException(string.Format("节点[{0}]设置生并发，数量必须大于1", e.ParseByName()));
                        _parallelStack.Push(new AciviyItem(currentId, activityType, sbookmark.GroupName, e, allNextActivities.Count));
                    }
                }
                else if (activityType == ActivityTypeEnum.Parallel)
                {
                    // 走过并发之后，将并发总数量--
                    var activity = _parallelStack.Where(p => p.GroupName == groupName).FirstOrDefault();
                    if (activity != null)
                    {
                        activity.Pop();
                        var otherParallel = _parallelStack.Pop();
                        transingGraphAct(otherParallel.Activit);
                    }
                    else
                    {
                        throw new WFDesignException(string.Format("并发节点[{0}]没有匹配的分组[{1}]", e.ParseByName(), groupName));
                    }
                    return;
                }
                else if (activityType == ActivityTypeEnum.NotifyMsg)
                {
                    var notifyMsg = _parallelStack.Where(p => p.GroupName == groupName).FirstOrDefault();
                    if (notifyMsg != null)
                    {
                        notifyMsg.Pop();
                        if (notifyMsg.Completed)
                        {
                            transingGraphAct(notifyMsg.Activit);
                        }
                    }
                    else
                    {
                        _parallelStack.Push(new AciviyItem(Guid.NewGuid(), ActivityTypeEnum.NotifyMsg, groupName, e));
                    }
                }

                #endregion (当前节点处理)

                #region 扩展处理
                if (exc != null)
                    exc(e);
                #endregion(扩展处理)

                #region 后续节点处理

                // 首次循环开关，用于控制并发执行第一个节点之后，其他的节点进入队列排队
                bool @switch = false;
                XElement parallelLeft = null;
                XElement parallelNext = null;
                allNextActivities.ForEach(p =>
                {
                    // 如果不是并发或者是并发之后的首个节点
                    if (!@switch || !isParallelStart)
                    {
                        @switch = true;
                        if (isParallelStart)
                            parallelLeft = p;
                        else
                            parallelNext = p;
                    }
                    else
                    {
                        int count = 0;
                        if (p.ParseByActivityType() == ActivityTypeEnum.Process.ToString())
                        {
                            var bookmark = JsonHelper.JsonToT<StandBookmark>(p.ParseByDetails());
                            if (bookmark.IsParallel)
                            {
                                count = GetAllNextActivities(templateXml, currentId).Count();
                            }
                        }
                        _parallelStack.Push(new AciviyItem(p.ParseById(), EnumHelper.GetEnumByString<ActivityTypeEnum>(p.ParseByActivityType()), p.ParseByName(), p, count));
                    }
                });
                // 继续深度递归
                if (parallelLeft != null)
                    transingGraphAct(parallelLeft);
                else
                    transingGraphAct(parallelNext);

                #endregion (后续节点处理)
            };

            // 启动深度查找
            transingGraphAct(firstReallActivity);
        }


        /// <summary>
        /// 并发结束
        /// </summary>
        /// <param name="doc">template</param>
        /// <param name="exc">扩展执行</param>
        public static void TraversingGraphParallel(XElement doc, Guid nodeId, HashSet<Guid> nodeIds, Action<XElement> exc = null)
        {
            // 控制并发的节点
            Stack<AciviyItem> _parallelStack = new Stack<AciviyItem>();
            string templateXml = doc.ToString();
            if (nodeIds == null)
                nodeIds = new HashSet<Guid>();
            Action<XElement> transingGraphAct = null;
            var firstReallActivity = GetAllNextActivities(templateXml, nodeId).First();
            if (firstReallActivity == null)
                throw new WFRoutingException("节点不存在");

            transingGraphAct = (e) =>
            {
                Guid currentId = e.ParseById();
                // 如果在集合中已经存在，则判断是否为并发结束
                // 当前节点类型
                var activityType = EnumHelper.GetEnumByString<ActivityTypeEnum>(e.ParseByActivityType());
                var groupName = e.ParseByGroupName();

                #region 重复处理
                if (nodeIds.Contains(currentId))
                {
                    // 如果是并发结束
                    if (activityType == ActivityTypeEnum.Parallel)
                    {
                        // 走过并发之后，将并发总数量--
                        var activity = _parallelStack.Where(p => p.GroupName == e.ParseByName()).FirstOrDefault(); // 如果不存在，则说明模板设置错误
                        if (activity == null)
                            throw new WFDesignException(string.Format("并发节点[{0}]没有匹配的分组[{1}]", e.ParseByName(), groupName));
                        activity.Pop();
                        if (activity.Completed)
                        {

                            // 如果并发没有完成
                            if (_parallelStack.Count() > 0)
                            {
                                // 将并发结束移除队列
                                var nextActivity = _parallelStack.Pop();
                                // 获取并发的所有下一个节点
                                var nextActivities = GetAllNextActivities(templateXml, currentId);
                                // 深度查找
                                nextActivities.ToList().ForEach(p =>
                                {
                                    // 继续深度递归
                                    transingGraphAct(p);
                                });
                                return;
                            }
                        }
                        else
                        {
                            // 并发没有完成的情况
                            if (_parallelStack.Count() > 0)
                            {
                                // 将并发数量--
                                var first = _parallelStack.Where(p => p.ActivitId == currentId).FirstOrDefault();
                                if (first == null)
                                    throw new WFDesignException(string.Format("并发节点[{0}]没有匹配的分组[{1}]", e.ParseByName(), groupName));
                                if (first.ParallelCount > 0)
                                    first.Pop();

                                // 如果并发没有完成
                                if (_parallelStack.Count() > 0)
                                {
                                    // 将并发结束移除队列
                                    _parallelStack.Pop();
                                    // 查找下一个节点【并发中的其他节点】
                                    var nextActivity = _parallelStack.First();
                                    // 继续深度递归
                                    transingGraphAct(nextActivity.Activit);
                                    return;
                                }
                            }
                        }
                    }

                    return;
                }

                #endregion(重复处理)

                #region 当前节点处理

                // 防止重复的集合
                nodeIds.Add(currentId);

                // 所有下一个节点的查询集合
                var allNextActivities = GetAllNextActivities(templateXml, currentId).ToList();

                // 是否是并发起点的开关
                bool isParallelStart = false;
                // 如果当前节点类型是标准节点
                if (activityType == ActivityTypeEnum.Process)
                {
                    var sbookmark = JsonHelper.JsonToT<StandBookmark>(e.ParseByDetails());
                    // 标准节点-->并发起点
                    if (sbookmark.IsParallel == true)
                    {
                        // 设置开关开
                        isParallelStart = true;
                        if (allNextActivities.Count <= 1)
                            throw new WFDesignException(string.Format("节点[{0}]设置生并发，数量必须大于1", e.ParseByName()));
                        _parallelStack.Push(new AciviyItem(currentId, activityType, sbookmark.GroupName, e, allNextActivities.Count));
                    }
                }
                else if (activityType == ActivityTypeEnum.Parallel)
                {
                    // 走过并发之后，将并发总数量--
                    var activity = _parallelStack.Where(p => p.GroupName == groupName).FirstOrDefault();
                    if (activity != null)
                    {
                        activity.Pop();
                        var otherParallel = _parallelStack.Pop();
                        transingGraphAct(otherParallel.Activit);
                    }
                    else
                    {
                        throw new WFDesignException(string.Format("并发节点[{0}]没有匹配的分组[{1}]", e.ParseByName(), groupName));
                    }
                    return;
                }

                #endregion (当前节点处理)

                #region 扩展处理
                if (exc != null)
                    exc(e);
                #endregion(扩展处理)

                #region 后续节点处理

                // 首次循环开关，用于控制并发执行第一个节点之后，其他的节点进入队列排队
                bool @switch = false;
                XElement parallelLeft = null;
                XElement parallelNext = null;
                allNextActivities.ForEach(p =>
                {
                    // 如果不是并发或者是并发之后的首个节点
                    if (!@switch || !isParallelStart)
                    {
                        @switch = true;
                        if (isParallelStart)
                            parallelLeft = p;
                        else
                            parallelNext = p;
                    }
                    else
                    {
                        int count = 0;
                        if (p.ParseByActivityType() == ActivityTypeEnum.Process.ToString())
                        {
                            var bookmark = JsonHelper.JsonToT<StandBookmark>(p.ParseByDetails());
                            if (bookmark.IsParallel)
                            {
                                count = GetAllNextActivities(templateXml, currentId).Count();
                            }
                        }
                        _parallelStack.Push(new AciviyItem(p.ParseById(), EnumHelper.GetEnumByString<ActivityTypeEnum>(p.ParseByActivityType()), p.ParseByName(), p, count));
                    }
                });
                // 继续深度递归
                if (parallelLeft != null)
                    transingGraphAct(parallelLeft);
                else
                    transingGraphAct(parallelNext);

                #endregion (后续节点处理)
            };

            // 启动深度查找
            transingGraphAct(firstReallActivity);
        }


        public static void ParallelValidate(string doc, Guid nodeId)
        {
            // 1. 找出所有的 活动
            // 2. 读取出数量
            var allActivities = GetAllActivities(doc);
            var process = allActivities.Where(p => p.ParseByActivityType() == ActivityTypeEnum.Process.ToString());
            var allParalles = new List<string>();
            process.ToList().ForEach(p =>
            {
                var obj = (StandBookmark)JsonHelper.JsonToT<StandBookmark>(p.Element("Details").Value);
                if (obj.IsParallel)
                    allParalles.Add(p.ParseByName());
            });
            var allParallelEnds = allActivities.Where(p => p.ParseByActivityType() == ActivityTypeEnum.Parallel.ToString());
            var allEnds = allParallelEnds.Select(p => p.ParseByName());
            var withNoStart = allParallelEnds.Where(p => allParalles.Contains(p.ParseByName()));
            var withNoEnd = allEnds.Where(p => !allParalles.Contains(p));

            // 检查 是否合法

        }
    }
}
