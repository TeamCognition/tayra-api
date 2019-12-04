using Tayra.Common;
using Tayra.Connectors.Common;

namespace Tayra.Connectors.App.Helpers
{
    public interface IConnectorResolver
    {
        T Get<T>(string type) where T : IConnector;

        T Get<T>(IntegrationType integrationType) where T : IConnector;
    }
}