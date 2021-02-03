using System;
using System.IO;
using RazorLight;
using Tayra.Mailer.Contracts;
namespace Tayra.Mailer.MailerTemplateModels
{
    public class GiftReceivedTemplateModel : ISlackMessageTemplate , IEmailTemplate
    {
        
        private const string TemplateKey = "gift-received";
        
        private const string EmailTemplateFileName = "GiftReceivedTemplate.cshtml";

        private const string SlackTemplateFileName = "GiftReceivedTemplate.json";
        public  string FirstName { get; set; }
        
        public string SenderName { get; set; }
        
        public string Subject { get; set; }
        
        public string GiftLink { get; set; }
        
        public string SlackUserId { get; set; }

       public GiftReceivedTemplateModel(string firstName, string senderName, string giftLink, string subject)
        {
            FirstName = firstName;
            SenderName = senderName;
            GiftLink = giftLink;
            Subject =subject ;
        }

       public GiftReceivedTemplateModel(string slackUserId, string giftLink, string subject)
       {
           SlackUserId = $@"<@{slackUserId}>";
           GiftLink = giftLink;
           Subject = subject;
       }
       
       public  string GetEmailTemplate()
        {
            var result = MailerUtils.BuildTemplateForEmail( this, EmailTemplateFileName);
            return result;
        }

        public string GetSlackTemplate()
        {
            var result = MailerUtils.BuildTemplateForSlack(this, TemplateKey, SlackTemplateFileName);
            return result;
        }
    }

    public class SlackAttachments
    {
        public object[] Attachments { get; set; }
    }
}