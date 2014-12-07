using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    public class NSInstanceInfo : Entity, IValidatableObject
    {
        /*  [Id]
      ,[InstanceName]
      ,[TemplateName]
      ,[TemplateId]
      ,[TemplateDes]
      ,[RunState]*/

        public NSInstanceInfo()
        {
        }
 
        [MaxLength(250)]
        public string InstanceName { get; set; }
        [MaxLength(250)]
        public string TemplateName { get; set; }
        public Guid TemplateId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [MaxLength(10)]
        public string RunState { get; set; }
        public Guid TaskId { get; set; }
        public Guid? ParentNodeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
