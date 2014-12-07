using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nsun.Tools.Common
{
    public class FileHelper
    {
        public static void CreateFile(string path,string content)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            using (var stream = File.OpenWrite(path))
            {
                byte[] by = UTF8Encoding.UTF8.GetBytes(content);
                stream.Write(by,0,by.Count());
            }
        }
    }
}
