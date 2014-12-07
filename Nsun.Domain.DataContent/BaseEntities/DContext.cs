using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using NHibernate;
using Nsun.Domain.DataContent;

namespace Nsun.Domain.DataContent
{
    public partial class DContext
    {
        // Place your custom code here.
        
        #region Override Methods

        protected override string GetConnectionString(string databaseName)
        {
            return base.GetConnectionString(databaseName);
        }

        #endregion
    }
}