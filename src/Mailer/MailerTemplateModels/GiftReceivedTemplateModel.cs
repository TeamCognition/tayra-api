using System;
using System.IO;
using RazorLight;
using Tayra.Mailer.Contracts;
namespace Tayra.Mailer.MailerTemplateModels
{
    public class GiftReceivedTemplateModel : ISlackMessageTemplate , IEmailTemplate
    {
        public  string FirstName { get; set; }
        
        public string SenderName { get; set; }

        public string TemplateKey => "gift-received";
        public string Title { get; set; }
        
        public string GiftLink { get; set; }
        
        public string SlackUserId { get; set; }

       public GiftReceivedTemplateModel(string firstName, string senderName, string giftLink, string title)
        {
            FirstName = firstName;
            SenderName = senderName;
            GiftLink = giftLink;
            Title = title;
        }

       public GiftReceivedTemplateModel(string slackUserId, string giftLink, string title)
       {
           SlackUserId = $@"<@{slackUserId}>";
           GiftLink = giftLink;
           Title = title;
       }
        public  string GetEmailTemplate()
        {
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                $@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","EmailTemplates");
            var result = MailerUtils.BuildTemplateForEmail(path, this, "GiftReceivedTemplate.cshtml");
            return result;
        }

        public string GetSlackTemplate()
        {
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                $@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","SlackTemplates","GiftReceivedTemplate.json");
            var json = File.ReadAllText(path);
            var result = MailerUtils.BuildTemplateForSlack(this, json, TemplateKey);
            return result;
        }
    }
}