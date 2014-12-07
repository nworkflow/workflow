using Nsun.Tools.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.Persistence
{
    public class DBFactory
    {
        public static IPersistence GetPersistencePlugIn()
        {
            string dbConfig = ConfigTools.GetAppValue("PersistenceDataBase");
            switch (dbConfig)
            {
                case "SQL":
                    return new SqlPlugIn();
                case "ORACLE":
                   // return new OraclePlugIn();
                    return null;
                default :
                    return new SqlPlugIn();
            }
        }
    }
}
