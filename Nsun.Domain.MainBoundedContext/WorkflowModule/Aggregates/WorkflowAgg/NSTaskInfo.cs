using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    public class NSTaskInfo : Entity, IValidatableObject
    {
        /*[TaskId]
      ,[TaskName]
      ,[TaskType]
      ,[TaskDes]
      ,[TemplateId]
      ,[RunState]*/
 
        public Guid TaskId { get; set; }
        [MaxLength(50)]
        public string TaskName { get; set; }
        [MaxLength(50)]
        public string TaskType { get; set; }
        public Guid TemplateId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EndTime { get; set; }
        [MaxLength(10)]
        public string RunState { get; set; }
       

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
