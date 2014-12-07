using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace Nsun.Common
{
    public class ZipUtil
    {
        #region Public Methods

        public static void Unzip(string zipFile, string targetFolder)
        {
            using (var s = new ZipInputStream(File.OpenRead(zipFile)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string fileName = Path.GetFileName(theEntry.Name);

                    //生成解压目录
                    if(!Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);

                    if (fileName != String.Empty)
                    {
                        //如果文件的壓縮後的大小為0那麼說明這個文件是空的因此不需要進行讀出寫入
                        if (theEntry.CompressedSize == 0)
                        {
                            break;
                        }
                        string newFileName = Path.Combine(targetFolder, theEntry.Name);
                        //解压文件到指定的目录
                        string directoryName = Path.GetDirectoryName(newFileName);
                        //建立下面的目录和子目录
                        Directory.CreateDirectory(directoryName);

                        FileStream streamWriter = File.Create(newFileName);

                        var data = new byte[2048];
                        while (true)
                        {
                            int size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }

                    //delete tmp files
                    string[] files = Directory.GetFiles(targetFolder, "*.tmp");
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("The specified file " + fileToZip +
                                                " could not be found. Zipping aborted.");
            }

            var streamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read);
            FileStream zipFile = File.Create(zipedFile);
            var zipStream = new ZipOutputStream(zipFile);
            var zipEntry = new ZipEntry("ZippedFile");
            zipStream.PutNextEntry(zipEntry);
            zipStream.SetLevel(compressionLevel);
            var buffer = new byte[blockSize];
            int size = streamToZip.Read(buffer, 0, buffer.Length);
            zipStream.Write(buffer, 0, size);
            try
            {
                while (size < streamToZip.Length)
                {
                    int sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            finally
            {
                zipStream.Finish();
                zipStream.Close();
                streamToZip.Close();
            }
        }

        public static void ZipFolder(string sourceDir, string zipFile)
        {
            //string filenames = Directory.GetFiles(args[0]);
            var targetDir = Path.GetDirectoryName(zipFile);
            if(!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            var crc = new Crc32();
            var s = new ZipOutputStream(File.Create(zipFile));

            s.SetLevel(6); // 0 - store only to 9 - means best compression

            var di = new DirectoryInfo(sourceDir);

            FileInfo[] a = di.GetFiles();

            try
            {
                //压缩这个目录下的所有文件
                WriteStream(ref s, a, crc, sourceDir);
                //压缩这个目录下子目录及其文件
                Direct(di, ref s, crc,sourceDir);
            }
            finally
            {
                s.Finish();
                s.Close();
            }
        }

        #endregion

        #region Methods

        private static void Direct(DirectoryInfo di, ref ZipOutputStream s, Crc32 crc,string root)
        {
            DirectoryInfo[] dirs = di.GetDirectories("*");

            //遍历目录下面的所有的子目录
            foreach (DirectoryInfo dirNext in dirs)
            {
                //将该目录下的所有文件添加到 ZipOutputStream s 压缩流里面
                FileInfo[] a = dirNext.GetFiles();
                WriteStream(ref s, a, crc, root);

                //递归调用直到把所有的目录遍历完成
                Direct(dirNext, ref s, crc,root);
            }
        }

        private static void WriteStream(ref ZipOutputStream s, FileInfo[] a, Crc32 crc, string root)
        {
            foreach (FileInfo fi in a)
            {
                //string fifn = fi.FullName;
                FileStream fs = fi.OpenRead();

                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);

                //ZipEntry entry = new ZipEntry(file);   Path.GetFileName(file)
                //string file = fi.FullName;
                //if(!string.IsNullOrEmpty(root))
                //    file = file.Replace(root, "");

                var entry = new ZipEntry(fi.FullName.Substring(root.Length+1));

                entry.DateTime = DateTime.Now;

                // set Size and the crc, because the information
                // about the size and crc should be stored in the header
                // if it is not set it is automatically written in the footer.
                // (in this case size == crc == -1 in the header)
                // Some ZIP programs have problems with zip files that don't store
                // the size and crc in the header.
                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);

                entry.Crc = crc.Value;

                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion
    }
}