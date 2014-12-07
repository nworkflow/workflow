using Nsun.Workflow.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Args
{
    public class WFArgs:EventArgs
    {
        public WFArgs(TransInfoDto transDto)
        {
            this.Parameter = transDto;
        }

        public TransInfoDto Parameter
        {
            get;
            private set;
        }
    }
}
