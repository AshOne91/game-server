using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core
{
    public class Passport
    {
        public static string Encrypt(string id, string extra)
        {
            string text = id + _SEPARATOR + extra;

            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] salt = Encoding.UTF8.GetBytes(KEY);

            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(KEY, salt);
            ICryptoTransform Encryptor = _RijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(plainText, 0, plainText.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cryptBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();
            string cryptResult = Convert.ToBase64String(cryptBytes);

            return cryptResult;
        }
        public static string Decrypt(string passport)
        {
            byte[] encryptData = Convert.FromBase64String(passport);
            byte[] salt = Encoding.UTF8.GetBytes(KEY);

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(KEY, salt);
            ICryptoTransform Decryptor = _RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream(encryptData);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

            byte[] PlainText = new byte[encryptData.Length];

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
            memoryStream.Close();
            cryptoStream.Close();

            string decryptedData = Encoding.UTF8.GetString(PlainText, 0, DecryptedCount);

            return decryptedData;
        }
        public static bool Vertify(string passport, out string id, out string extra)
        {
            string text = Decrypt(passport);

            string[] words = text.Split(_SEPARATOR);

            if ( words.Length == 2)
            {
                id = words[0];
                extra = words[1];
                return true;
            }
            id = "";
            extra = "";
            return false;
        }

        public static string KEY = "pA!oU}y6mSZ,VQT3%ddnp[y(PVxs4u7O";
        private static string _SEPARATOR = "|EUTTEUM|";
        private static RijndaelManaged _RijndaelCipher = new RijndaelManaged();
    }
}
