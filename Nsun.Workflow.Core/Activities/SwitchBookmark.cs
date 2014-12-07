using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Routing;
using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Models
{
    public sealed class SwitchBookmark : BookmarkBase
    {
        public SwitchBookmark()
        {
            this.NodeType = "Decision";
            this.Size = new KeyValuePair<int, int>(80, 60);
            this.CanPersistence = false;
        }

        private string _condition;
        [System.ComponentModel.Category("主属性")]
        [System.ComponentModel.DisplayName("分支条件")]
        public string Condition
        {
            get { return _condition; }
            set
            {
                _condition = value;
                RaisePropertyChanged("Condition");
            }
        }


        public override string Validate()
        {
            // 检验是否为C#语法
            return base.Validate();
        }

        public override string GetSerialContent()
        {
            //使用反射读取Attribute           
            return base.GetSerialContent(this.GetType());
        }


        public override void StartBuiness(Dtos.TransInfoDto transInfoDto)
        {
            var conditionResult = string.Empty;
            // 判断条件
            if (!string.IsNullOrEmpty(transInfoDto.Condition))
                conditionResult = transInfoDto.Condition;
            else
                return;

            var newAcivities =  XmlHelper.GetNextActivityByCondition(transInfoDto.TemplateXml,Guid.Parse(XmlHelper.GetSafeValue(transInfoDto.Activity,ActivityConst.ID)),conditionResult);
            if (newAcivities != null)
            {
                new RoutingHost().RoutingFactory(newAcivities.ToList(), transInfoDto);
            }
            base.StartBuiness(transInfoDto);
        }
    }
}
