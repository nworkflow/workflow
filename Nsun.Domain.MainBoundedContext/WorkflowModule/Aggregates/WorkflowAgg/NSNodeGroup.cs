using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    public class NSNodeGroup: Entity, IValidatableObject
    {
        public NSNodeGroup()
        {
        }


        public Guid TaskId
        {
            get;
            set;
        }


        public Guid InstanceId
        {
            get;
            set;
        }


        [MaxLength(50)]
        public string GroupName
        {
            get;
            set;
        }


        public int GroupCounter
        {
            get;
            set;
        }


        public bool? Finshed
        {
            get;
            set;
        }
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
