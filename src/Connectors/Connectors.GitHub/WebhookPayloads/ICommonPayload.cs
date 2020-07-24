namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public interface ICommonPayload
    {
        public string Action { get; set; }
        public object Sender { get; set; }
        public object Repository { get; set; }
        public object Organization { get; set; }
        public object Installation { get; set; }
    }
}