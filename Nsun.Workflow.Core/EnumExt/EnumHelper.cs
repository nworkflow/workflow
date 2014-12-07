using Nsun.Tools.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Core.EnumExt
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 将字符串转换成枚举
        /// </summary>
        /// <typeparam name="T">要转换成枚举的类型</typeparam>
        /// <param name="strEnum">字符串</param>
        /// <returns>转换的结果</returns>
        public static T GetEnumByString<T>(string strEnum)
        {
            T t = default(T);
            try
            {
                t = (T)Enum.Parse(typeof(T), strEnum, false);

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }
            return t;
        }
    }
}
