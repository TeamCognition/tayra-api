using Newtonsoft.Json.Linq;

namespace Tayra.Services.webhooks
{
    public interface IWebhook
    {
        public string HandleWebhook(JObject jObject, string ghEvent);
    }
}