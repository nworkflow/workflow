using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Routing.Validate;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace Nsun.Workflow.Core.Routing
{
    /// <summary>
    /// 路由规则
    /// </summary>
    public class RoutingRule
    {
        // 模板
        private XElement _template;
        // 导航站，用于控制节点的出栈与入栈
        private Stack<RoutingModel> _navStack;
        // 并发验证
        private Action<RoutingModel> _parallelValidate = null;
        // 控制一个节点只走一次，防止循环
        private HashSet<string> _transAll = new HashSet<string>();
        /// <summary>
        /// 导航队列
        /// </summary>
        public Stack<RoutingModel> NavStack
        {
            get
            {
                if (_navStack == null)
                {
                    _navStack = new Stack<RoutingModel>();
                }
                return _navStack;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="template">模板</param>
        public RoutingRule(XElement template)
        {
            _template = template;
        }

        public static readonly List<string> SpecialType = new List<string>() {
                ActivityTypeEnum.NotifyMsg.ToString(),
                ActivityTypeEnum.Parallel.ToString()
        };

        /// <summary>
        /// 路由
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>后续的路由</returns>
        public RoutingModel TransGraph()
        {
            var startAcvity = XmlHelper.GetStartActivit(_template);
            var model = new RoutingModel(_template, startAcvity, true);
            TransGraph(model);
            return null;
        }


        /// <summary>
        /// 准备Model
        /// </summary>
        /// <param name="element">当前的活动</param>
        /// <param name="routingModel">验证规则列表</param>
        /// <returns>返回生成好的模型</returns>
        public void TransGraph(RoutingModel routingModel, Dictionary<ValidateFunEnum, IRoutingValidate> valis = null)
        {
            Guid id = routingModel.Key;
            if (_transAll.Contains(routingModel.Name))
                return;
            valis = valis ?? new Dictionary<ValidateFunEnum, IRoutingValidate>();

            if (NavStack.Any(p => p.Key == id))
            {
                RoutingModel routingModle = NavStack.First(p => p.Key == id);
                if (!routingModel.Trigger)
                    routingModle.Pop();
                if (routingModle.Trigger)
                {
                    if (valis.ContainsKey(ValidateFunEnum.ParallelValidate))
                        valis[ValidateFunEnum.ParallelValidate].RoutingValidate(routingModel);
                    if (valis.ContainsKey(ValidateFunEnum.PropertyValidate))
                        valis[ValidateFunEnum.PropertyValidate].RoutingValidate(routingModel);

                    _transAll.Add(routingModel.Name);
                    if (routingModle.TransStack.Any())
                    {
                        // 获取子
                        routingModle.TransStack.ToList().ForEach(p =>
                        {
                            var newRoutingModel = new RoutingModel(_template, p.Key, true);
                            TransGraph(newRoutingModel, valis);
                        });
                    }
                }
                else
                {
                    if (valis.ContainsKey(ValidateFunEnum.ParallelValidate))
                        valis[ValidateFunEnum.ParallelValidate].RoutingValidate(routingModel);
                    if (valis.ContainsKey(ValidateFunEnum.PropertyValidate))
                        valis[ValidateFunEnum.PropertyValidate].RoutingValidate(routingModel);
                }
            }
            else
            {
                NavStack.Push(routingModel);
                TransGraph(routingModel, valis);
            }
        }

        /// <summary>
        /// 检查模板是否合法
        /// 【检验 并发】
        /// </summary>
        /// <returns>返回操作结果</returns>
        public string TemplateValidate()
        {
            // 用于控制并发,在开始的时候，压入栈,开始的时候压入栈，然后遇到并发结束出栈这出栈。
            // 如果并发结束出栈，而并发开始还没有加入，则记录错。
            var startAcvity = XmlHelper.GetStartActivit(_template);
            if (startAcvity == null)
                return "模板必须有起始节点";
            var model = new RoutingModel(_template, startAcvity, true);
            var validatePlugs = new Dictionary<ValidateFunEnum, IRoutingValidate>() { 
                {ValidateFunEnum.ParallelValidate, new ParallelRoutingValidate()},
                {ValidateFunEnum.PropertyValidate,new PropertyRoutingValidate()}
            };
            TransGraph(model, validatePlugs);

            var result = new ResultInfo();
            foreach (var iValidate in validatePlugs)
            {
                var validateResult = iValidate.Value.GetResult();
                if (validateResult.HasError)
                    result.AppendError(validateResult.GetResult(Environment.NewLine));
            }

            return result.HasError ? result.GetResult(Environment.NewLine) : "并发验证通过";
        }



        /// <summary>
        /// 路由信息
        /// </summary>
        /// <returns>返回节点所有的路由信息</returns>
        public HashSet<string> Routing()
        {
            return _transAll;
        }


        /// <summary>
        /// 验证规则
        /// </summary>
        public enum ValidateFunEnum
        {
            ParallelValidate,
            PropertyValidate
        }
    }
}
