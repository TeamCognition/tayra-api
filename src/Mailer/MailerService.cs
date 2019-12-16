using SendGrid;
using SendGrid.Helpers.Mail;

namespace Tayra.Mailer
{
    public static class MailerService// : IEmailService
    {
        private static string noReplyAddress = "noreply@tayra.io";

        public static Response SendEmail(string recipient, ITemplateEmailDTO dto)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(noReplyAddress);
            var to = new EmailAddress(recipient);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, dto.TemplateId, dto.TemplateData);
            return client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }

        public static Response SendEmail(string sender, string recipient, string subject, string body)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(sender, "Tayra Admin");
            var to = new EmailAddress(recipient, "CTO Haris");
            var plainTextContent = body;
            var htmlContent = $"<strong>{body}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }
    }
}
