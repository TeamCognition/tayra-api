﻿namespace Tayra.Mailer.Templates.GiftReceived
{
    public class TemplateModelGiftReceived : ISlackMessageTemplate , IEmailTemplate
    {
        public string TemplateKey => "gift-received";
        public string SlackTemplateFileName => "SlackTemplate_GiftReceived.json";
        public string EmailTemplateFileName => "EmailTemplate_GiftReceived.cshtml";
        public  string ReceiverName { get; set; }
        
        public string SenderName { get; set; }
        
        public string Subject { get; set; }
        
        public string GiftLink { get; set; }
        
        public string SlackUserId { get; set; }

       public TemplateModelGiftReceived(string receiverName, string senderName, string giftLink, string subject)
        {
            ReceiverName = receiverName;
            SenderName = senderName;
            GiftLink = giftLink;
            Subject = subject;
        }

       public TemplateModelGiftReceived(string slackUserId, string giftLink, string subject)
       {
           SlackUserId = $@"<@{slackUserId}>";
           GiftLink = giftLink;
           Subject = subject;
       }
    }
}