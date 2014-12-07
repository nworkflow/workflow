using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Tools.Common.Filters
{
    public interface IFilter
    {
        bool IsValidate(string strSource);
    }
}
