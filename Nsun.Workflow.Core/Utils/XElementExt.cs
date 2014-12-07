using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Workflow.Core.Utils
{
    public static class XElementExt
    {
        /// <summary>
        /// 获取Guid通过Id类型
        /// </summary>
        /// <param name="guid">扩展</param>
        /// <param name="activity">当前的实例</param>
        /// <returns>转换结果</returns>
        public static Guid ParseById(this XElement activity)
        {
            if (activity == null)
                return Guid.Empty;
            return Guid.Parse(XmlHelper.GetSafeValue(activity, ActivityConst.ID));
        }


        public static Guid ParseByLinkId(this XElement activity)
        {
            if (activity == null)
                return Guid.Empty;
            return Guid.Parse(XmlHelper.GetSafeValue(activity, ActivityConst.SINKID));
        }


        public static Guid ParseBySourceId(this XElement activity)
        {
            if (activity == null)
                return Guid.Empty;
            return Guid.Parse(XmlHelper.GetSafeValue(activity, ActivityConst.SOURCEID));
        }


        public static string ParseByGroupName(this XElement activity)
        {
            if (activity == null)
                return string.Empty;
            return XmlHelper.GetSafeValue(activity, ActivityConst.GROUPNAME);
        }


        public static string ParseByActivityType(this XElement activity)
        {
            if (activity == null)
                return string.Empty;
            return XmlHelper.GetSafeValue(activity, ActivityConst.TYPE);
        }

        public static string ParseByDetails(this XElement activity)
        {
            if (activity == null)
                return string.Empty;
            return XmlHelper.GetSafeValue(activity, ActivityConst.DETAILS);
        }


        public static string ParseByName(this XElement activity)
        {
            if (activity == null)
                return string.Empty;
            return XmlHelper.GetSafeValue(activity, ActivityConst.NAME);
        }
    }
}
