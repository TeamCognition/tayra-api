using SendGrid;
using SendGrid.Helpers.Mail;

namespace Tayra.Mailer
{
    public static class EmailService// : IEmailService
    {
        public const string noReplyAddress = "noreply@tayra.io";

        //private const string apiKey = "e2b4ffc49b9667cc448decd2841058fa"; mailchimp
        private const string apiKey = "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";

        public static Response SendEmail(string recipient, ITemplateEmailDTO dto)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(noReplyAddress);
            var to = new EmailAddress(recipient);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, dto.TemplateId, dto.TemplateData);
            return client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }

        public static Response SendEmail(string sender, string recipient, string subject, string body)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(sender, "Tayra Admin");
            var to = new EmailAddress(recipient, "CTO Haris");
            var plainTextContent = "body";
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }
    }
}
