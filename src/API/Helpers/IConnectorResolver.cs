using Tayra.Connectors.Common;
using Tayra.Common;

namespace Tayra.API.Helpers
{
    public interface IConnectorResolver
    {
        T Get<T>(string type) where T : IConnector;

        T Get<T>(IntegrationType integrationType) where T : IConnector;
    }
}