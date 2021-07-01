using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ARchGLCloud.Core.Security
{
    public class SignatureUtiltity
    {
        public static string DetectionSignature(string appId, string secretId, string secretKey, long ticks)
        {
            var plainText = string.Format("a={0}&s={1}&t={2}", appId, secretId, ticks);

            using (HMACSHA1 mac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = mac.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                var pText = Encoding.UTF8.GetBytes(plainText);
                var all = new byte[hash.Length + pText.Length];
                Array.Copy(hash, 0, all, 0, hash.Length);
                Array.Copy(pText, 0, all, hash.Length, pText.Length);
                return Convert.ToBase64String(all);
            }
        }
    }
}
