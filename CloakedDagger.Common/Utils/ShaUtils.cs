using System;
using System.Security.Cryptography;
using System.Text;

namespace CloakedDagger.Common.Utils
{
    public static class ShaUtils
    {
        public static string Sha256HashString(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return string.Empty;
            }

            using var sha = SHA256.Create();
            
            var bytes = Encoding.UTF8.GetBytes(val);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}