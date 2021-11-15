using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub.Common
{
    public class Edge<T>
    {
        [JsonProperty("node")]
        public T Node { get; set; }
    }
}