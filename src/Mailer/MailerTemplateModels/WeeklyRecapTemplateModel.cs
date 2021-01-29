using Tayra.Mailer.Contracts;

namespace Tayra.Mailer.MailerTemplateModels
{
    public class WeeklyRecapTemplateModel : ISlackMessageTemplate,IEmailTemplate
    {
        public string TemplateKey { get; set; }
        public string RecipientId { get; set; }
        public string GetEmailTemplate()
        {
            throw new System.NotImplementedException();
        }

        public string GetSlackTemplate()
        {
            throw new System.NotImplementedException();
        }
    }
}