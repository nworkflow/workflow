using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Nsun.Workflow.Common
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public partial interface IService1
    {
    }

    public partial class Service1 : IService1
    {
        MainWorkflowUnitOfWork _context;
        public MainWorkflowUnitOfWork Context
        {
            get
            {
                if (_context == null)
                    _context = new MainWorkflowUnitOfWork();
                return _context;
            }
        }
    }
}
