using Nsun.Workflow.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Models
{
    public sealed class StartBookmark : BookmarkBase
    {
        public StartBookmark()
        {
            this.NodeType = "Start";
            this.CanPersistence = false;
        }

        public override string GetSerialContent()
        {
            return base.GetSerialContent(this.GetType());
        }
    }
}
