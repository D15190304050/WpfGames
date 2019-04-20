using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GobangClient
{
    /// <summary>
    /// <see cref="Encrypter" />类提供了用MD5算法对给定的字符串进行加密的方法。
    /// </summary>
    public static class Encrypter
    {
        /// <summary>
        /// 用于对信息进行加密的MD5算法。
        /// </summary>
        private static MD5 md5Encrypter;

        /// <summary>
        /// 初始化用于对信息进行加密的MD5算法。
        /// </summary>
        static Encrypter()
        {
            md5Encrypter = new MD5CryptoServiceProvider();
        }

        // Calculates and returns the MD5 hash value of the specified string password.
        /// <summary>
        /// 用MD5算法对给定的字符串进行加密。
        /// </summary>
        /// <param name="password">需要加密的字符串。</param>
        /// <returns>对给定的字符串加密后得到的MD5值。</returns>
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
