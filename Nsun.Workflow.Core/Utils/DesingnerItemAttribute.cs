using Nsun.Workflow.Core.Activities;
using Nsun.Workflow.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Utils
{
    public class DesingnerItemStyle : Attribute
    {

        private static Dictionary<Type, string> DesignerContexts = new Dictionary<Type,string>();
        private static string path = @"Nsun.Workflow.Core.DesingerItemContext.{0}.context";

        public static string GetDesignerContext(Type designerItemType)
        {
            if (DesignerContexts.ContainsKey(designerItemType))
            {
                return DesignerContexts[designerItemType];
            }
            else
            {
                string desigerItemContextUrl = string.Format(path, designerItemType.Name);
                Stream ms = designerItemType.Assembly.GetManifestResourceStream(desigerItemContextUrl);
                byte[] bs = new byte[ms.Length];
                ms.Read(bs, 0, bs.Length);

                string style =  Encoding.UTF8.GetString(bs);
                DesignerContexts.Add(designerItemType, style);
                return style;
            }
        }
    }
}
