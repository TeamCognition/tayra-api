using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer.Contracts
{
    public interface ISlackMessageTemplate 
    {
        public string TemplateKey { get; }
        public string Title { get; set; }
        public string GetSlackTemplate();
    }
}