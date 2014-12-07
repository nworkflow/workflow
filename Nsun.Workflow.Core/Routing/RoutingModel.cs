using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Nsun.Workflow.Core.Utils;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Models;
using Nsun.Tools.Common;
using Nsun.Workflow.Core.Activities;
namespace Nsun.Workflow.Core.Routing
{
    /// <summary>
    /// 路由Model 
    /// date 20141023
    /// </summary>
    public class RoutingModel
    {
        private XElement _template;
        public RoutingModel()
        {
        }

        /// <summary>
        /// 构造,支持自动转换
        /// </summary>
        /// <param name="doc">模板</param>
        /// <param name="id">当前节点(起始节点)</param>
        /// <param name="trans">是否获取后续节点</param>
        public RoutingModel(XElement doc, Guid id, bool trans = false)
        {
            this.Key = id;
            var element = XmlHelper.GetElementById(doc, id);
            Init(doc, element, trans);
        }

        /// <summary>
        /// 重构构造函数
        /// </summary>
        /// <param name="doc">模板</param>
        /// <param name="element">当前节点</param>
        /// <param name="trans">是否获取后续节点</param>
        public RoutingModel(XElement doc, XElement element, bool trans = false)
        {
            this.Key = element.ParseById();
            Init(doc, element, trans);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="doc">模板</param>
        /// <param name="element">当前节点</param>
        /// <param name="trans">是否获取后续节点</param>
        private void Init(XElement doc,XElement element,bool trans)
        {
            this._template = doc;
            this.Name = element.ParseByName();
            this.Group = element.ParseByGroupName();
            this.DefaultCount = GetTriggerCountAndType(element);
            this.Count = DefaultCount;
            if (trans)
            {
                var nextActivities = XmlHelper.GetAllNextActivities(doc.ToString(), this.Key).ToList();
                nextActivities.ForEach(p =>
                {
                    TransStack.Push(new RoutingModel(doc, p, false));
                });
            }
        }


        /// <summary>
        /// 关键字
        /// </summary>
        public Guid Key;
        /// <summary>
        /// 分组
        /// </summary>
        public string Group;
        /// <summary>
        ///  数量
        /// </summary>
        public int Count;
        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 节点类型
        /// </summary>
        public ActivityTypeEnum Type;
        /// <summary>
        /// 默认数量
        /// </summary>
        public int DefaultCount;
        /// <summary>
        /// 条件
        /// </summary>
        public string Decision;
        /// <summary>
        /// 子流程模板
        /// </summary>
        public Guid SubTemplateId;
        private Stack<RoutingModel> _transStack;
        /// <summary>
        ///  堆栈，用于存储规则路径
        /// </summary>
        public Stack<RoutingModel> TransStack
        {
            get
            {
                if (_transStack == null)
                    _transStack = new Stack<RoutingModel>();
                return _transStack;
            }
        }

        /// <summary>
        /// 是否达到出栈规则
        /// </summary>
        public bool Trigger
        {
            get { return Count == 0 || !RoutingRule.SpecialType.Contains(this.Type.ToString()); }
        }

        /// <summary>
        /// 并发出栈规则
        /// </summary>
        public bool ParallelTrigger
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// 出栈
        /// </summary>
        public void Pop()
        {
            if (Count > 0)
                --Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private int GetTriggerCountAndType(XElement element)
        {
            // 获取节点类型
            var type = element.ParseByActivityType();
            // 当是标准节点的时候，需要进一步判断是否是并发开始
            if (element.ParseByActivityType() == ActivityTypeEnum.Process.ToString())
            {
                var obj = (StandBookmark)JsonHelper.JsonToT<StandBookmark>(element.Element(ActivityConst.DETAILS).Value);
                // 如果遇到并发的起点，说明是并发内部的并发
                if (obj.IsParallel)
                    this.Type = ActivityTypeEnum.Process_Parallel;
                else
                    this.Type = ActivityTypeEnum.Process;
            }
            else
            {
                //  其他类型，需要转换成对应的Enum类型
                this.Type = EnumHelper.GetEnumByString<ActivityTypeEnum>(type);

                if (this.Type == ActivityTypeEnum.Decision)
                {
                    var obj = (SwitchBookmark)JsonHelper.JsonToT<SwitchBookmark>(element.Element(ActivityConst.DETAILS).Value);
                    this.Decision = obj.Condition;
                }
                else
                {
                    var obj = (SubRoutingBookmark)JsonHelper.JsonToT<SubRoutingBookmark>(element.Element(ActivityConst.DETAILS).Value);
                    if (string.IsNullOrEmpty(obj.TemplateName))
                        obj.TemplateId = Guid.Empty;
                    this.SubTemplateId = obj.TemplateId;
                }
            }

            
            // 该类型是否包含在特殊节点中（消息，并发结束等需要知道多少个父在调用）
            if (RoutingRule.SpecialType.Contains(type))
            {
                this.Type = EnumHelper.GetEnumByString<ActivityTypeEnum>(type);
                return XmlHelper.GetAllForwardActivities(_template.ToString(), element.ParseById()).ToList().Count;
            }

            // 非特殊类型的节点，返回包含多少个后续节点
            return XmlHelper.GetAllNextActivities(_template.ToString(), element.ParseById()).ToList().Count;
        }

    }
}
