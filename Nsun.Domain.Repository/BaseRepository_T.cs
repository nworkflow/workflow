using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Domain.Repository
{
    public class BaseRepository<T> : IRepository<T>
    {
        protected ISession Session { get; set; }
        public BaseRepository()
        {
            Session = new NHibernateHelper().GetSession();
        }


        public void InsertMethod(T t)
        {
            Session.Save(t);
            Session.Flush();
        }


        public void UpdateMethod(T t)
        {
            Session.Update(t);
            Session.Flush();
        }


        public void DeleteMethod(T t)
        {
            Session.Delete(t);
            Session.Flush();
        }


        public IList<T> QueryMethod(string strQuery)
        {
            return Session.CreateQuery(strQuery).List<T>();
        }
    }
}
