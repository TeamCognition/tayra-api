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

       public GiftReceivedTemplateModel(string firstName, string senderName,string giftLink)
        {
            FirstName = firstName;
            SenderName = senderName;
            GiftLink = giftLink;
        }
        public  string GetEmailTemplate()
        {
            // string path =Path.(@"TemplatesFiles");
            // Console.WriteLine(path);
            var engine = new RazorLightEngineBuilder().UseFileSystemProject("C:/Users/efi_c/OneDrive/Desktop/Tayra/Backend/tayra-api/src/Mailer/TemplatesFiles")
                .UseMemoryCachingProvider().Build();
            GiftReceivedTemplateModel model = this;
            Console.WriteLine(model);
            string result =engine.CompileRenderAsync("GiftReceivedTemplate.cshtml", model).GetAwaiter().GetResult();
            Console.WriteLine(result);
            return result;
        }

        public string GetSlackTemplate()
        {
            throw new System.NotImplementedException();
        }
    }
}