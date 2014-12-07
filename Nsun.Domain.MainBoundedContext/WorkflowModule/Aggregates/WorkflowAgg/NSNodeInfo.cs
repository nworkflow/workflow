using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    public class NSNodeInfo : Entity, IValidatableObject
    {

        public NSNodeInfo()
        {
        }

        [MaxLength(500)]
        public string NodeName { get; set; }
        public Guid TaskId { get; set; }
        public string NodeDes { get; set; }
        [MaxLength(250)]
        public string TransUserNo { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        [MaxLength(250)]
        public string SubmitResult { get; set; }
        [MaxLength(250)]
        public string SubmitOptions { get; set; }
        [MaxLength(250)]
        public string SubmitUserNo { get; set; }
        [MaxLength(10)]
        public string RunState { get; set; }
        public Guid? ParentId { get; set; }
        public Guid InstanceId { get; set; }
        public Guid NodeId { get; set; }
        [MaxLength(50)]
        public string GroupName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
