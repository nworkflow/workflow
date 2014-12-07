using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsun.Tools.Common
{
    /// <summary>
    /// Config文件处理工具
    /// </summary>
    public static class ConfigTools
    {
        /// <summary>
        /// 得到App值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetAppValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            try
            {
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                    return config.AppSettings.Settings[key].Value;
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设定App值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>0：错误 1：修改 2：插入</returns>
        public static int SetAppValue(string key, string value)
        {
            // 参数空验证
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return 0;

            try
            {
                // 配置文件空验证
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                if (config == null)
                    return 0;

                int result = 0;
                if (config.AppSettings.Settings[key] != null)
                {
                    config.AppSettings.Settings[key].Value = value;
                    result = 1;
                }
                else
                {
                    config.AppSettings.Settings.Add(key, value);
                    result = 2;
                }

                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 得到链接字符串
        /// </summary>
        /// <param name="conKey"></param>
        /// <returns></returns>
        public static string GetConnection(string conKey)
        {
            if (string.IsNullOrEmpty(conKey))
                return string.Empty;

            try
            {
                System.Configuration.ConnectionStringSettings cons = System.Configuration.ConfigurationManager.ConnectionStrings[conKey];
                if (cons == null)
                    return string.Empty;
                return cons.ConnectionString;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设定连接字符串
        /// </summary>
        /// <param name="conKey">连接字符串键</param>
        /// <param name="dataSource">连接字符串dataSource</param>
        /// <param name="initialCatalog">连接字符串initialCatalog</param>
        /// <returns>0：错误 1：修改 2：插入</returns>
        public static int SetConnection(string conKey, string dataSource, string initialCatalog)
        {
            // 参数空验证
            if (string.IsNullOrEmpty(conKey))
                return 0;
            if (string.IsNullOrEmpty(dataSource))
                return 0;
            if (string.IsNullOrEmpty(initialCatalog))
                return 0;

            try
            {
                // 配置文件空验证
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                if (config == null)
                    return 0;

                int result = 0;
                string tempConnectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", dataSource, initialCatalog);

                if (config.ConnectionStrings.ConnectionStrings[conKey] != null)
                {
                    config.ConnectionStrings.ConnectionStrings[conKey].ConnectionString = tempConnectionString;
                    result = 1;
                }
                else
                {
                    System.Configuration.ConnectionStringSettings csSettings = new System.Configuration.ConnectionStringSettings(conKey, tempConnectionString, "System.Data.SqlClient");
                    config.ConnectionStrings.ConnectionStrings.Add(csSettings);
                    result = 2;
                }

                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
