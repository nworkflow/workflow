using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Persistence
{
    /// <summary>
    /// 持久化工厂，用于扩展其他持久化数据库
    /// </summary>
    public interface IPersistenceFactory
    {
        void Persistence();
    }
}
