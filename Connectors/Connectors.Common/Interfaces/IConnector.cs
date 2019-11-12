using Tayra.Common;

namespace Tayra.Connectors.Common
{
    public interface IConnector
    {
        IntegrationType Type { get; }
    }
}