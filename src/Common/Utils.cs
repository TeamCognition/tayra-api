using System.Collections.Generic;
using System.Linq;

namespace Tayra.Common
{
    public static class Utils
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) {
            return enumerable == null || !enumerable.Any();
        }
    }
}