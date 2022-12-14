using System;
using System.Text;
using System.Security.Cryptography;

namespace OpenCodeChems.Server.Utils
{
    /// <summary>
    /// This class encrypt the password
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// method that encrypt the password
        /// </summary>
        /// <param name="text"> string that encrypt</param>
        /// <returns></returns>
        public string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
    }
}