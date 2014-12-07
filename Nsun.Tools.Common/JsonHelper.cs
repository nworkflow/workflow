using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Tools.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;


    public class JsonHelper
    {
        // 从一个对象信息生成Json串   
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                byte[] dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(dataBytes);
            }
        }

        // 从一个Json串生成对象信息  
        public static object JsonToObject(string jsonStr, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
           using(MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
            return serializer.ReadObject(mStream);
        }


        public static T JsonToT<T>(string jsonStr) where T:class
        {
            if(string.IsNullOrEmpty(jsonStr))
                return null;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using(MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
            return serializer.ReadObject(mStream) as T;
        }
    }
}
