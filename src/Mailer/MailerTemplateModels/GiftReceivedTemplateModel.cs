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
        
        public string GiftLink { get; set; }
        
        public string SlackUserId { get; set; }

       public GiftReceivedTemplateModel(string firstName, string senderName,string giftLink)
        {
            FirstName = firstName;
            SenderName = senderName;
            GiftLink = giftLink;
        }

       public GiftReceivedTemplateModel(string slackUserId, string giftLink)
       {
           SlackUserId = $"*<@{slackUserId}>*";
           GiftLink = giftLink;
       }
        public  string GetEmailTemplate()
        {
            // string path =Path.(@"TemplatesFiles");
            // Console.WriteLine(path);
            var engine = new RazorLightEngineBuilder().UseFileSystemProject("C:/Users/TAYRA/Documents/GitHub/tayra-api/src/Mailer/TemplatesFiles/EmailTemplates")
                .UseMemoryCachingProvider().Build();
            GiftReceivedTemplateModel model = this;
            string result =engine.CompileRenderAsync("GiftReceivedTemplate.cshtml", model).GetAwaiter().GetResult();
            Console.WriteLine(AppContext.BaseDirectory);
            return result;
        }

        public string GetSlackTemplate()
        {
            string template = File.ReadAllText(
                "C:/Users/TAYRA/Documents/GitHub/tayra-api/src/Mailer/TemplatesFiles/SlackTemplates/GiftReceivedTemplate.json");
            Console.WriteLine(template);
            var engine = new RazorLightEngineBuilder()
                // required to have a default RazorLightProject type,
                // but not required to create a template from string.
                .UseEmbeddedResourcesProject(typeof(GiftReceivedTemplateModel))
                .UseMemoryCachingProvider()
                .Build(); 
            //string newString = " hejjjjjjjjjj @UserSlackId and @GiftLink";
            GiftReceivedTemplateModel model = this;
            string result =  engine.CompileRenderStringAsync("templateKey", template, model).GetAwaiter().GetResult();
            Console.WriteLine($"the result is{result}");
            return result;
        }
    }
}