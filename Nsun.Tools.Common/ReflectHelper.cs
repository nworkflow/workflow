using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nsun.Tools.Common
{
    public class ReflectHelper
    {
        public static T Dycopy<T>(T target, T source)
        {
            foreach (var item in typeof(T).GetProperties())
            {
                item.SetValue(target, item.GetValue(source, null), null);
            }
            return target;
        }


        public static T NewT<T>(string[] sources) where T : new()
        {
            T t = new T();
            lock (t)
            {
                foreach (var item in typeof(T).GetProperties())
                {
                    string map = sources.Where(p => p.Replace('-','_').Contains(item.Name)).FirstOrDefault();
                    
                    if (!string.IsNullOrEmpty(map))
                    {
                        if (map.Contains(','))
                        {
                            var innerSoucre = map.Split(',');
                            string sp = innerSoucre.Where(p => p.Replace('-','_').Contains(item.Name)).First();
                            
                            item.SetValue(t,ConvertString.ToType(item.PropertyType, sp.Split('=')[1]), null);
                        }
                        else
                            item.SetValue(t, ConvertString.ToType(item.PropertyType, map.Split('=')[1]), null);
                    }
                    
                }

                return t;
            }
        }
    }


    public class ConvertString
    {
        public static object ToType(Type type, string value)
        {

            if (type == typeof(string)) { return value; }
            if (type==typeof(Int32?))
            {
                type = typeof(int);
                if (string.IsNullOrEmpty(value))
                    return null;
            }
            else if (type == typeof(Decimal?))
            {
                type = typeof(Decimal);
                if (string.IsNullOrEmpty(value))
                    return null;
            }
       
            MethodInfo parseMethod = null;
            foreach (MethodInfo mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public)) { if (mi.Name == "Parse" && mi.GetParameters().Length == 1) { parseMethod = mi; break; } }
            if (parseMethod == null) { throw new ArgumentException(string.Format("Type: {0} has not Parse static method!", type)); }
            return parseMethod.Invoke(null, new object[] { value });
        }
    }
}
