using System;

namespace Cog.Core
{
    public static class ArrayExtensions
    {
        public static T[] EnsureSize<T>(this T[] source, int desiredSize)
        {
            var diff = desiredSize - source.Length;
            if (diff == 0) return source;
            if (diff < 0) throw new Exception("array extensinos desiredSize value invalid");

            var newArr = new T[desiredSize];
            source.CopyTo(newArr, diff);

            return newArr;
        }
    }
}