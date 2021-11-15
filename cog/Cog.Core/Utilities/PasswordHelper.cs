using System;
using System.Text;
using Konscious.Security.Cryptography;
using NSec.Cryptography;

namespace Cog.Core
{
    //TODO: https://www.meziantou.net/how-to-store-a-password-in-a-web-application.htm
    public static class PasswordHelper
    {
        private static Argon2i InitArgon(byte[] pwd, byte[] salt = null, byte[] data = null)
        {
            return new(pwd)
            {
                DegreeOfParallelism = 2,
                MemorySize = 1 << 13, //8192
                Iterations = 4,
                Salt = salt,
                AssociatedData = data
            };
        }

        public static byte[] Hash(string password, byte[] salt)
        {
            return InitArgon(password.ToBytes(), salt).GetBytes(128);
        }

        public static bool Verify(byte[] password, byte[] salt, string toVerify)
        {
            return Compare(password, Hash(toVerify, salt));
        }

        public static byte[] GenerateSalt(int count = 8)
        {
            return RandomGenerator.Default.GenerateBytes(count);
        }

        public static byte[] ToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static bool Compare(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
        {
            return a1.SequenceEqual(a2);
        }
    }
}