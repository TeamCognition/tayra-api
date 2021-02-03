using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer.Contracts
{
    public interface IEmailTemplate 
    {
        public string Title { get; set; }
        public string GetEmailTemplate();
    }
}