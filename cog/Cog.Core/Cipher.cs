using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cog.Core
{
    // Taken from Stack answer: http://stackoverflow.com/a/5518092/654708
    //TODO: THIS IS NOT PRODUCTION READY YET, IV NEEDS TO BE RANDOMIZED
    public static class Cipher
    {
        private static byte[] key = { 59, 72, 211, 92, 45, 249, 113, 147, 110, 240, 253, 242, 148, 61, 20, 29, 191, 122, 110, 155, 142, 9, 86, 22, 35, 96, 222, 218, 34, 26, 11, 76 };

        // a hardcoded IV should not be used for production AES-CBC code
        // IVs should be unpredictable per ciphertext
        private static byte[] vector = { 149, 227, 146, 176, 221, 225, 185, 18, 111, 156, 34, 81, 16, 211, 219, 156 };

        private static readonly ICryptoTransform encryptor, decryptor;
        private static readonly UTF8Encoding encoder;

        static Cipher()
        {
            using (var rm = new RijndaelManaged())
		    {
			    encryptor = rm.CreateEncryptor(key, vector);
			    decryptor = rm.CreateDecryptor(key, vector);
		    }

		    encoder = new UTF8Encoding();
        }

        public static string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public static string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public static byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public static byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        private static byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }

        #region Maybe separate class

        public static string Base64UrlEncode(this string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var s = Convert.ToBase64String(bytes); // Regular base64 encoder
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }

        public static string Base64UrlDecode(this string value)
        {
            var s = value;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    s += "==";
                    break; // Two pad chars
                case 3:
                    s += "=";
                    break; // One pad char
                default:
                    throw new Exception("Illegal base64 url string!");
            }

            var bytes = Convert.FromBase64String(s); // Standard base64 decoder
            return Encoding.UTF8.GetString(bytes);
        }
        #endregion
    }
}
