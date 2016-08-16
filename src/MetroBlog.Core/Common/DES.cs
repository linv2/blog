using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MetroBlog.Core.Common
{
    public class DES
    {
        public static string Encrypt(string sourceString, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(sourceString))
            {
                return null;
            }
            try
            {

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);

                            cs.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                    catch
                    {
                        return sourceString;
                    }
                }
            }
            catch { }

            return null;
        }
        public static string Decrypt(string encryptedString, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(encryptedString))
            {
                return null;
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return encryptedString;
                }
            }
        }
    }
}
