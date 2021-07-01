using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ARchGLCloud.Core.Security
{
    /// <summary>
    /// 提供用于计算指定文件哈希值的方法
    /// <example>例如计算文件的MD5值:
    /// <code>
    ///   string hashMd5=HashHelper.ComputeMD5("MyFile.txt");
    /// </code>
    /// </example>
    /// <example>例如计算文件的CRC32值:
    /// <code>
    ///   string hashCrc32 = HashHelper.ComputeCRC32("MyFile.txt");
    /// </code>
    /// </example>
    /// <example>例如计算文件的SHA1值:
    /// <code>
    ///   string hashSha1 =HashHelper.ComputeSHA1("MyFile.txt");
    /// </code>
    /// </example>
    /// </summary>
    public sealed class HashUtility
    {
        /// <summary>
        ///  计算指定文件的MD5值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static string ComputeMD5(string fileName)
        {
            string hashMD5 = string.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //计算文件的MD5值
                    MD5 calculator = MD5.Create();
                    Byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X2"));
                    }
                    hashMD5 = stringBuilder.ToString();
                }//关闭文件流
            }//结束计算
            return hashMD5;
        }
        /// <summary>
        ///  计算指定文件的CRC32值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static string ComputeCRC32(string fileName)
        {
            string hashCRC32 = string.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //计算文件的CSC32值
                    Crc32 calculator = new Crc32();
                    Byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X2"));
                    }
                    hashCRC32 = stringBuilder.ToString();
                }
            }
            return hashCRC32;
        }
        /// <summary>
        ///  计算指定文件的SHA1值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static string ComputeSHA1(string fileName)
        {
            string hashSHA1 = string.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //计算文件的SHA1值
                    SHA1 calculator = SHA1.Create();
                    byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X2"));
                    }

                    hashSHA1 = stringBuilder.ToString();
                }
            }
            return hashSHA1;
        }


        public static string HashSHA1(string path)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] hash;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
            {
                hash = sha1.ComputeHash(fs);
            }

            return BitConverter.ToString(hash);
        }
    }
}
