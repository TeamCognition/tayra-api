using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using ToPdfConvertor;

namespace Tayra.Mailer
{
    internal static class EmailService
    {
        public const string noReplyAddress = "noreply@tayra.io";

        private const string apiKey = "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";
        
        public static Response SendEmail(string recipient, string subject, string body)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(noReplyAddress);
            var to = new EmailAddress(recipient);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            return client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }
        
        public static Response SendEmailWithAttachment(string recipient, string subject, string body)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(noReplyAddress);
            var to = new EmailAddress(recipient);
            var generatedPdf = ToPdfConvertorService.ConvertHtmlToPdf(body);
           // var pdfFile = File.ReadAllBytes(pathOfGeneratedPdf);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            msg.AddAttachment(
                filename: $"{subject} - {DateTime.UtcNow.ToShortDateString()}",
                base64Content: Convert.ToBase64String(generatedPdf),
                type: "application/pdf",
                disposition: "attachment");
            return client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }
    }
}
