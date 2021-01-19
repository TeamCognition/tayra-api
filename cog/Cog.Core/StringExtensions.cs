using System;

namespace Cog.Core
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Provides default value if input string is null, empty or white space.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Original string or default value.</returns>
        public static string DefaultIfEmpty(this string input, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(input)) return defaultValue;
            return input;
        }

        /// <summary>
        ///     Converts string to boolean value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Bool value.</returns>
        public static bool ToBool(this string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            value = value.ToLower().Trim();
            if (value == "true") return true;
            if (value == "false") return false;
            if (value == "1") return true;
            if (value == "0") return false;
            throw new ArgumentOutOfRangeException(value, $"{value} is not a boolean value.");
        }

        /// <summary>
        ///     Converts string to Byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string value)
        {
            return Convert.FromBase64String(value);
        }

        public static string RemoveAllWhitespaces(this string input)
        {
            if (input == null) return input;

            var len = input.Length;
            var src = input.ToCharArray();
            var dstIdx = 0;

            for (var i = 0; i < len; i++)
            {
                var ch = src[i];

                switch (ch)
                {
                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':

                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':

                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':

                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':

                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                        continue;

                    default:
                        src[dstIdx++] = ch;
                        break;
                }
            }

            return new string(src, 0, dstIdx);
        }

        public static string ToLowerFirst(this string input)
        {
            if (string.IsNullOrEmpty(input)) // || char.IsLower(str, 0))
                return input;

            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }
        
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}