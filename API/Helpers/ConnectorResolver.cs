using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Tayra.Connectors.Common;
using Tayra.Common;

namespace Tayra.API.Helpers
{
    public class ConnectorResolver : IConnectorResolver
    {
        public ConnectorResolver(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; set; }

        public T Get<T>(string type) where T : IConnector
        {
            var serviceType = Enum.Parse<IntegrationType>(type, true);
            return Get<T>(serviceType);
        }

        public T Get<T>(IntegrationType integrationType) where T : IConnector
        {
            var services = ServiceProvider.GetServices<T>();
            return services.FirstOrDefault(x => x.Type == integrationType);
        }
    }
}