using System;
using System.Security.Cryptography;
using System.Text;

namespace AngOE.Common
{
    public static class SHA256Hash
    {
        /// <summary>
        /// Create a SHA256 hash from a given string
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string CreateHash(string original)
        {
            string hashBase64;

            using (var sha256 = new SHA256Managed())
            {
                var originalBytes = Encoding.UTF8.GetBytes(original);
                var encodedBytes = sha256.ComputeHash(originalBytes);
                hashBase64 = Convert.ToBase64String(encodedBytes);
            }

            return hashBase64;
        }
    }
}