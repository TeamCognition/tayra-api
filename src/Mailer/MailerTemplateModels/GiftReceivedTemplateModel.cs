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
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,$@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","EmailTemplates");

            // string path =Path.(@"TemplatesFiles");
            // Console.WriteLine(path);
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(path)
                .UseMemoryCachingProvider().Build();
            GiftReceivedTemplateModel model = this;
            string result =engine.CompileRenderAsync("GiftReceivedTemplate.cshtml", model).GetAwaiter().GetResult();
            return result;
        }

        public string GetSlackTemplate()
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,$@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","SlackTemplates","GiftReceivedTemplate.json");
            
            string json = File.ReadAllText(path);
            string template = $"[{json}]";
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(GiftReceivedTemplateModel))
                .UseMemoryCachingProvider()
                .Build();
            GiftReceivedTemplateModel model = this;
            string result =  engine.CompileRenderStringAsync("templateKey", template, model).GetAwaiter().GetResult();
            return result;
        }
    }

    public class SlackAttachments
    {
        public object[] Attachments { get; set; }
    }
}