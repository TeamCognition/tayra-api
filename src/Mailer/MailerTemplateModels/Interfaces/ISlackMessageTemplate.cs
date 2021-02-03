using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer.Contracts
{
    public interface ISlackMessageTemplate 
    {
        public string Title { get; set; }
        public string GetSlackTemplate();
    }
}