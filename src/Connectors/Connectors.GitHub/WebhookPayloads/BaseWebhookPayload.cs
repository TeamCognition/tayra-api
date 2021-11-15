using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public class BaseWebhookPayload
    {
        /// <summary>
        /// Most webhook payloads contain an action property that contains the specific activity that triggered the event.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The user that triggered the event. This property is included in every webhook payload.
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// The repository where the event occured. Webhook payloads contain the repository property when the event occurs from activity in a repository.
        /// </summary>
        public RepositoryDto Repository { get; set; }

        /// <summary>
        /// Webhook payloads contain the organization object when the webhook is configured for an organization or the event occurs from activity in a repository owned by an organization.
        /// </summary>
        public object Organization { get; set; }

        /// <summary>
        /// The GitHub App installation. Webhook payloads contain the installation property when the event is configured for and sent to a GitHub App. For more information, see "Building GitHub App."
        /// </summary>
        public object Installation { get; set; }

        public class RepositoryDto
        {
            [JsonProperty("id")]
            public string Id { get; private set; }

            [JsonProperty("node_id")]
            public string NodeId { get; private set; }

            [JsonProperty("name")]
            public string Name { get; private set; }

            [JsonProperty("full_name")]
            public string FullName { get; private set; }
        }
    }
}