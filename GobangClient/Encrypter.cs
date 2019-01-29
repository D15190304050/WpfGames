using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GobangClient
{
    public static class Encrypter
    {
        private static MD5 md5Encrypter;

        static Encrypter()
        {
            md5Encrypter = new MD5CryptoServiceProvider();
        }

        // Calculates and returns the MD5 hash value of the specified string password.
        public static string Encrypt(string password)
        {
            byte[] md5Bytes = md5Encrypter.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder encryptedPassword = new StringBuilder();
            foreach (byte b in md5Bytes)
                encryptedPassword.Append(b.ToString("X2"));

            return encryptedPassword.ToString();
        }
    }
}
