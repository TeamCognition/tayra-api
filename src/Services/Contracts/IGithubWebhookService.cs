using Newtonsoft.Json.Linq;

namespace Tayra.Services.webhooks
{
    public interface IGithubWebhookService
    {
        public string HandleWebhook(JObject jObject, string ghEvent);
    }
}