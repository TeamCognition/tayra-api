using SendGrid;

namespace Tayra.Mailer
{
    public interface IMailerService
    {
        public object SendSlackMessage(string botToken, string channel, ISlackMessageTemplate template);
        public Response SendEmail(string recipient, IEmailTemplate emailTemplate);
        public Response SendEmail(string recipient, string subject, string message);
        public Response SendEmailWithAttachment(string recipient, IEmailTemplate emailTemplate);
    }
}