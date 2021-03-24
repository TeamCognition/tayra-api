namespace Tayra.Mailer
{
    public interface ISlackMessageTemplate 
    {
        public string Subject { get; set; }
        public string TemplateKey { get; }
        public string SlackTemplateFileName { get; }
    }
}