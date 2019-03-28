using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Itok.HelperMethods
{
    public static class ItokHelper
    {
        /// <summary>
        ///     CompareVersion
        /// </summary>
        /// <param name="leftVersion"></param>
        /// <param name="rightVersion"></param>
        /// <returns>left > right -> 1, left = right -> 0, else -1</returns>
        public static int CompareVersion(string leftVersion, string rightVersion)
        {
            var v1Array = leftVersion.Split('.').ToList();
            var v2Array = rightVersion.Split('.').ToList();
            var max = Math.Max(v1Array.Count, v2Array.Count);
            for (var i = 0; i < max; i++)
            {
                if (v1Array.Count < i + 1)
                {
                    v1Array.Add("0");
                }

                if (v2Array.Count < i + 1)
                {
                    v2Array.Add("0");
                }
            }

            var result = 0;
            for (var index = 0; index < v1Array.Count; index++)
            {
                var leftItem = Int32.Parse(v1Array[index]);
                var rightItem = Int32.Parse(v2Array[index]);
                if (leftItem < rightItem)
                {
                    result = -1;
                }

                if (leftItem > rightItem)
                {
                    result = 1;
                }

                if (result != 0)
                {
                    return result;
                }
            }

            return result;
        }

        public static string GetMd5(string str)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(str);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public static string CompressGZipString(string input, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var buffer = encoding.GetBytes(input);
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory,
                    CompressionMode.Compress, true))
                {
                    gzip.Write(buffer, 0, buffer.Length);
                }

                return Convert.ToBase64String(memory.ToArray());
            }
        }

        public static string DecompressGZipString(string input, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var gZipBuffer = Convert.FromBase64String(input);
            using (var stream = new GZipStream(new MemoryStream(gZipBuffer),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    } while (count > 0);

                    return encoding.GetString(memory.ToArray());
                }
            }
        }

        public static string UnZipFile(string zipFile, string targetFolder = null)
        {
            if (!File.Exists(zipFile))
            {
                throw new FileNotFoundException();
            }

            if (String.IsNullOrEmpty(targetFolder))
            {
                targetFolder = $@"{Path.GetDirectoryName(zipFile)}\{Path.GetFileNameWithoutExtension(zipFile)}";
            }

            EnsureDirExist(targetFolder);
            var archive = ZipFile.Open(zipFile, ZipArchiveMode.Read);
            archive.ExtractToDirectory(targetFolder);
            return targetFolder;
        }

        public static void EnsureDirExist(string dirPath, bool deleteExisted = false)
        {
            if (Directory.Exists(dirPath))
            {
                if (deleteExisted)
                {
                    Directory.Delete(dirPath, true);
                }
                else
                {
                    return;
                }
            }

            Directory.CreateDirectory(dirPath);
        }

        public static TObj ConvertToObj<TObj>(string str)
        {
            return string.IsNullOrEmpty(str) ? default : JsonConvert.DeserializeObject<TObj>(str);
        }
    }
}