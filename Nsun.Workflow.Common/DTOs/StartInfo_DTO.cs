using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Nsun.Workflow.Common.DTOs
{
    [DataContract]
    public class StartInfo_DTO
    {
        [DataMember]
        public string TemplateName
        {
            get;
            set;
        }

        [DataMember]
        public Guid TemplateId
        {
            get;
            set;
        }

        [DataMember]
        public string TaskName
        {
            get;
            set;
        }

        [DataMember]
        public string TaskType
        {
            get;
            set;
        }
    }
}
