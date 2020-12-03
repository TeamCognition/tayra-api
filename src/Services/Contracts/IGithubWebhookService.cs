using Newtonsoft.Json.Linq;
using Tayra.Connectors.GitHub;

namespace Tayra.Services.Contracts
{
    public interface IGithubWebhookService
    {
        public string HandleWebhook(JObject jObject,string ghEvent);
    }
}