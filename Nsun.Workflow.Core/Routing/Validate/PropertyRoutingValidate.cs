using Nsun.Workflow.Core.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Routing.Validate
{
    public class PropertyRoutingValidate : RoutingValidateBase
    {
        public override Utils.ResultInfo RoutingValidate(RoutingModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                this.Info.AppendError("节点必须填写名称");
            if (model.Type == ActivityTypeEnum.Decision)
                if (string.IsNullOrEmpty(model.Decision))
                    this.Info.AppendError(string.Format("分支节点{0}必须填写条件", model.Name));
            if (model.Type == ActivityTypeEnum.SubRouting)
                if (model.SubTemplateId == null || model.SubTemplateId == Guid.Empty)
                    this.Info.AppendError(string.Format("子流程{0}必须填写子流程模板", model.Name));

            return base.RoutingValidate(model);
        }
    }
}
