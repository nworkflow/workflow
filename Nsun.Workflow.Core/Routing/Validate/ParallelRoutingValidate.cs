using Nsun.Workflow.Core.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Routing.Validate
{
    public class ParallelRoutingValidate:RoutingValidateBase
    {
        protected Stack<RoutingModel> ParalleStack = new Stack<RoutingModel>();
        public override Utils.ResultInfo RoutingValidate(RoutingModel model)
        {
            if (model.Type == ActivityTypeEnum.Process_Parallel)
            {
                var msg = ParallelPropertyValidate(model);
                if (string.IsNullOrEmpty(msg))
                {
                    if (ParalleStack.Any(p => p.Group == model.Group))
                    {
                        var existParallelStart = ParalleStack.First(p => p.Group == model.Group);
                        Info.AppendError(string.Format("并发开始{0}和并发开始{1}分组{2}冲突", existParallelStart.Name, model.Name, model.Group));
                        return Info;
                    }
                    ParalleStack.Push(model);
                }
                else
                {
                    Info.AppendError(msg);
                }
            }
            else if (model.Type == ActivityTypeEnum.Parallel)
            {
                var msg = ParallelPropertyValidate(model);
                if (string.IsNullOrEmpty(msg))
                {
                    if (ParalleStack.Any())
                    {
                        var lastModel = ParalleStack.Last();
                        if (lastModel.Type == ActivityTypeEnum.Process_Parallel
                            && lastModel.Group == model.Group)
                        {
                            if (lastModel.DefaultCount != model.DefaultCount)
                            {
                                ParallelSetMsg(model.Name, string.Format("并发开始{0}和并发结束{1}数量分支数量必须匹配", model.Name, lastModel.Name));
                                return Info;
                            }

                            lastModel.Pop();
                            if (lastModel.ParallelTrigger)
                                ParalleStack.Pop();
                        }
                        return Info;
                    }

                    ParallelSetMsg(model.Name, string.Format("并发结束{0}必须有对应的并发开始", model.Name));

                }
                else
                {
                    Info.AppendError(msg);
                }
            }
            return base.RoutingValidate(model);
        }


        /// <summary>
        /// 并发分支信息填写验证
        /// </summary>
        /// <param name="model">要验证的信息</param>
        /// <returns>验证结果</returns>
        private string ParallelPropertyValidate(RoutingModel model)
        {
            if (string.IsNullOrEmpty(model.Group))
                return model.Name + "必须有分组信息";
            return string.Empty;
        }

        private void ParallelSetMsg(string nodeName, string msg)
        {
            if (this.Info.AlreadyExist(nodeName))
                this.Info.AppendError(nodeName + msg);
        }

        public override Utils.ResultInfo GetResult()
        {

            if (ParalleStack.Any())
                ParalleStack.ToList().ForEach(p =>
                {
                    this.Info.AppendError(string.Format("并发开始{0},分组{1}必须有对应的并发结束", p.Name, p.Group));
                });
            return base.GetResult();
        }
    }
}
