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
            return SlackService.SendSlackMessage("", new SlackMessageRequestDto{
                Attachments = slackMessageTemplate.GetSlackTemplate(),
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