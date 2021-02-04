using System.IO;
using Tayra.Mailer.Contracts;
using Tayra.Mailer.Enums;

namespace Tayra.Mailer.MailerTemplateModels
{
    public class JoinTayraTemplateModel : IEmailTemplate
    {
        public string Title { get; set; }
        public string Sender { get; set; }
        public string Link { get; set; }

        public JoinTayraType JoinTayraType { get; set; }

        public JoinTayraTemplateModel(string title, string sender, string link,JoinTayraType joinTayraType)
        {
            Title = title;
            Sender = sender;
            Link = link;
            JoinTayraType = joinTayraType;
        }
        public string GetEmailTemplate()
        {
            var fileName = "JoinTayraDefault.cshtml";
            if (JoinTayraType == JoinTayraType.FirstAdmin)
            {
                fileName = "JoinTayraFirstAdmin.cshtml";
            }
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                $@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","EmailTemplates");
            var result = MailerUtils.BuildTemplateForEmail(path, this,fileName);
            return result;

        }
    }
}