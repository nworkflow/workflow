using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    public class NSRoutingData : Entity, IValidatableObject
    {
        public NSRoutingData()
        {
        }

        public Guid InstanceId { get; set; }
        public string TransData { get; set; }
        public string TransOut { get; set; }
        public string TransIn { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
