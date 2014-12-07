using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    /// <summary>
    ///[TemplateType]
    ///[TemplateDes]
    ///[Version]
    /// </summary>
    public class NSTemplateType : Entity, IValidatableObject
    {
        [MaxLength(50)]
        public string TemplateType { get; set; }
        public string TemplateDes { get; set; }
        [MaxLength(10)]
        public string Version { get; set; }
        public int? Status { get; set; }

        public NSTemplateType()
        {
        }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(TemplateType) || Id == Guid.Empty)
                return new List<ValidationResult>() { new ValidationResult("类型或逐渐不能为空！") };
            return null;
        }
    }
}
