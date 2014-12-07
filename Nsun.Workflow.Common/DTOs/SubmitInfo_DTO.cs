using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Nsun.Workflow.Common.DTOs
{
    [DataContract]
    public class SubmitInfo_DTO
    {
        [DataMember]
        public Guid NodeId
        {
            get;
            set;
        }

        [DataMember]
        public string Condition
        {
            get;
            set;
        }


        [DataMember]
        public string SubmitResult
        {
            get;
            set;
        }

        [DataMember]
        public List<ExpandData> ExpandDatas
        {
            get;
            set;
        }
    }
}
