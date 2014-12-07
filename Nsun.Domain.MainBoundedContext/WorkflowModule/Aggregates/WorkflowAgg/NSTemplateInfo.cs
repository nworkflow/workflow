using Microsoft.Samples.NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg
{
    public class NSTemplateInfo : Entity,IValidatableObject
    {
        [MaxLength(250)]
        public string TemplateName { get;  set; }
        public string TemplateText { get;  set; }
        [MaxLength(10)]
        public string Version { get;  set; }
        public DateTime? CreateTime { get;  set; }
        public bool IsActive { get;  set; }
        [MaxLength(50)]
        public string TemplateType { get;  set; }

        public NSTemplateInfo()
        {
        }


        public NSTemplateInfo(Guid templateId,string templateName, string templateText)
        {
            if(templateId==Guid.Empty)
                throw new ArgumentNullException("TemplateId");

            if (String.IsNullOrWhiteSpace(templateName))
                throw new ArgumentNullException("templateName");

            if (String.IsNullOrWhiteSpace(templateText))
                throw new ArgumentNullException("templateText");
       
            this.Id = templateId;
            this.TemplateName = templateName;
            this.TemplateText = templateText;
     
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
