using System.Collections.Generic;
using Tayra.Connectors.GitHub.Common;

namespace Tayra.Connectors.GitHub.Helper
{
    public static class MapGQResponse<T>
    {
        public static List<T> MapResponseToCommitType(List<Edge<T>> response)
        {
            var edges = response;
            List<T> mappedList = new List<T>();
            foreach (var edge in edges)
            {
                mappedList.Add(edge.Node);
            }

            return mappedList;
        }
    }
}