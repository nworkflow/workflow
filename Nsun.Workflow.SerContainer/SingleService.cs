using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.SerContainer
{
    public class SingleService
    {
        private SingleService() { }

        private static WFService.Service1Client _singleClient;
        public static WFService.Service1Client SingleClient
        {
            get
            {
                if (_singleClient == null)
                    _singleClient = new WFService.Service1Client();
                return _singleClient;
            }
        }
    }
}
