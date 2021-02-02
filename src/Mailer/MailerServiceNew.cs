using System;
using Tayra.Common;
using Tayra.Connectors.Slack;
using Tayra.Connectors.Slack.DTOs;
using Tayra.Mailer.Contracts;
using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer
{
    public class MailerServiceNew : IMailerService
    {

        public object SendSlackMessage(string recipient, ISlackMessageTemplate slackMessageTemplate)
        {
            Console.WriteLine();
            return SlackService.SendSlackMessage("xoxb-698826045604-1117671360278-zB1nNQLCkjI3iR8qXuvZGM7E", new SlackMessageRequestDto{
                Attachments = slackMessageTemplate.GetSlackTemplate(),
                Text = "Gift",
                Channel = recipient
            });
        }

        public object SendEmail(string recipient, string sender, IEmailTemplate emailTemplate)
        {
           return MailerService.SendEmail("faykohamad@gmail.com", "eficet89@gmail.com", "Testing",
                emailTemplate.GetEmailTemplate());
        }
    }
}