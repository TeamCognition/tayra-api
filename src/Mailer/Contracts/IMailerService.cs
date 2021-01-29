using Tayra.Common;
using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer.Contracts
{
    public interface IMailerService
    {
        public object SendSlackMessage(string recipient, ISlackMessageTemplate slackMessageTemplate);
        public object SendEmail(string recipient,string sender, IEmailTemplate emailTemplate);
    }
}