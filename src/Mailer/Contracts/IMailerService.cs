using Tayra.Common;
using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer.Contracts
{
    public interface IMailerService
    {
        public object SendSlackMessage(string recipient, ISlackMessageTemplate slackMessageTemplate);
        public object SendEmail(string recipient,string sender, IEmailTemplate emailTemplate);
        public object SendEmail(string recipient, string sender, string subject, string message);
        public object SendEmailWithAttachment(string recipient, string sender, IEmailTemplate emailTemplate);
    }
}