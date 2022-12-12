using System;
using System.Text;
using System.Security.Cryptography;

namespace OpenCodeChems.Server.Utils
{
    public class Encryption
    {
        public string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
    }
}