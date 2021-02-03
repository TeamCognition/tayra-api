﻿using System;
using System.Text.Encodings.Web;
using Tayra.Common;
using Tayra.Connectors.Slack;
using Tayra.Connectors.Slack.DTOs;
using Tayra.Mailer.Contracts;
using Tayra.Mailer.MailerTemplateModels;
using System.Web;

namespace Tayra.Mailer
{
    public class MailerService : IMailerService
    {

        public object SendSlackMessage(string recipient, ISlackMessageTemplate slackMessageTemplate)
        {
            
            var message = slackMessageTemplate.GetSlackTemplate();
            string decodedMessage = HttpUtility.UrlDecode(message);
            return SlackService.SendSlackMessage("xoxb-698826045604-1117671360278-zB1nNQLCkjI3iR8qXuvZGM7E", new SlackMessageRequestDto{
                Attachments = decodedMessage,
                Text = slackMessageTemplate.Title,
                Channel = recipient
            });
        }

        public object SendEmail(string recipient, string sender, IEmailTemplate emailTemplate)
        {
           return EmailService.SendEmail(sender, recipient, emailTemplate.Title,
                emailTemplate.GetEmailTemplate());
        }
    }
}