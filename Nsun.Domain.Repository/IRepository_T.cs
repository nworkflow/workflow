using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Domain.Repository
{
    public interface IRepository<T>
    {
        void InsertMethod(T t);
        void UpdateMethod(T t);
        void DeleteMethod(T t);
        IList<T> QueryMethod(string strQuery);
    }
}
