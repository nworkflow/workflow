using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nsun.Workflow.Core.Routing;
using System.Xml.Linq;

namespace Nsun.Workflow.Activities.Testing
{
    [TestClass]
    public class TransRoutingUnitTest
    {
        [TestMethod]
        public void TransRouting()
        {
            Workflow.SerContainer.WFService.Service1Client _client = new Workflow.SerContainer.WFService.Service1Client();
            var template = _client.GetTemplateByIds(Guid.Parse(""));
            XElement.Parse(template.TemplateText);
        }
    }
}
