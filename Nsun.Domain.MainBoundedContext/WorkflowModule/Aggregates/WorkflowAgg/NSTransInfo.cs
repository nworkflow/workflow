using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    public class NSTransInfo : Entity, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
