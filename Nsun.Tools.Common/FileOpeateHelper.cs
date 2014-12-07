using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nsun.Tools.Common
{
    public class FileOpeateHelper
    {
        public static string GetFileText(string mappingFileName)
        {
            try
            {
                using (StreamReader sm = File.OpenText(Environment.CurrentDirectory + "/Mapping/nsunDb." + mappingFileName + ".mapping"))
                {
                    return sm.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "";
            }

        }


        /// <summary>
        /// 写包到文件
        /// </summary>
        /// <param name="path">值</param>
        /// <param name="package">包</param>
        /// <param name="newFile">文件</param>
        public static void WritePackgeToFile(string path, string package, bool newFile)
        {
            string STR = path + "STR" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
            package = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "  " + package;
            WriteFile(STR, package, newFile);
        }


        /// <summary>
        /// 写值到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void WriteValueToFile(string path, string value)
        {
            value = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "  " + value;
            string STROK = path + "STROK" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
            WriteFile(STROK, value);
        }


        /// <summary>
        /// 写固定路径的内容
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="value">要写的内容</param>
        public static void WriteFile(string path, string value)
        {
            try
            {
                using (StreamWriter sm = File.AppendText(path))
                {
                    sm.WriteLine(value);
                };
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 封装
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="value">值</param>
        /// <param name="newFile">是否创建新文件</param>
        private static void WriteFile(string filePath, string value, bool newFile)
        {
            try
            {
                // 新文件
                if (newFile)
                {
                    string STR = filePath;
                    string STROK = filePath.Replace("STR", "STROK");
                    if (!File.Exists(STR))
                    {
                        File.CreateText(STR);
                        WriteFile(STR, "----------------------------bigin receive----------------------------------" + Environment.NewLine);
                    }
                    if (!File.Exists(STROK))
                    {
                        File.CreateText(STROK);
                        WriteFile(STR, "----------------------------bigin receive----------------------------------" + Environment.NewLine);
                    }
                }
                // 追加原来的文件
                else
                {
                    using (StreamWriter sm = File.AppendText(filePath))
                    {
                        sm.WriteLine(value);
                    };
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 写系统日志
        /// </summary>
        public static void WriteSystemLog(string value)
        {
            string path = Environment.CurrentDirectory + @"/Log_System/" + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            value = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "  " + value;
            if (!File.Exists(path))
            {
                File.CreateText(path);
                WriteFile(path, "----------------------------bigin receive----------------------------------" + Environment.NewLine);
            }
            WriteFile(path, value);
        }

    }
}
