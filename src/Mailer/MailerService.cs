using System;
using System.IO;
using RazorLight;
using SendGrid;
using Tayra.Connectors.Slack;
using Tayra.Connectors.Slack.DTOs;

namespace Tayra.Mailer
{
    public class MailerService : IMailerService
    {
        public object SendSlackMessage(string botToken, string channel, ISlackMessageTemplate template)
        {
            var message = BuildSlackMessageFromTemplate(template);
            return SlackService.SendSlackMessage(botToken, new SlackMessageRequestDto
            {
                Attachments = message,
                Text = template.Subject,
                Channel = channel
            });
        }

        public Response SendEmail(string recipient, IEmailTemplate emailTemplate)
        {
           return EmailService.SendEmail(recipient, emailTemplate.Subject,
               BuildEmailFromTemplate(emailTemplate, emailTemplate.EmailTemplateFileName));
        }

        public Response SendEmail(string recipient, string subject, string message)
        {
            return EmailService.SendEmail(recipient, subject, message);
        }

        public Response SendEmailWithAttachment(string recipient, IEmailTemplate emailTemplate)
        {
            return EmailService.SendEmailWithAttachment(recipient, emailTemplate.Subject,
                BuildEmailFromTemplate(emailTemplate, emailTemplate.EmailTemplateFileName));
        }
        
        private static string BuildEmailFromTemplate<T>(T model,string templatePath) where T: IEmailTemplate
        {
            var rootPath = AppContext.BaseDirectory;
            
            // var folderPathh = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
            //     $@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","EmailTemplates");
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(rootPath)
                .UseMemoryCachingProvider()
                .Build();
            
            return engine.CompileRenderAsync(templatePath, model).GetAwaiter().GetResult();
        }

        private static string BuildSlackMessageFromTemplate<T>(T model) where T: ISlackMessageTemplate
        {
            var rootPath = AppContext.BaseDirectory;
            
            var templateJson = File.ReadAllText(Path.Combine(rootPath, model.SlackTemplateFileName));
            string template = $"[{templateJson}]";
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(T))
                .UseMemoryCachingProvider()
                .Build();
            return engine.CompileRenderStringAsync(model.TemplateKey, template, model).GetAwaiter().GetResult();
        }
    }
}