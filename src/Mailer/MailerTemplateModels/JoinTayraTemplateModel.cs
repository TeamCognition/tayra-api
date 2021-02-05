using System.IO;
using Tayra.Mailer.Contracts;
using Tayra.Mailer.Enums;

namespace Tayra.Mailer.MailerTemplateModels
{
    public class JoinTayraTemplateModel : IEmailTemplate
    {
        public string Subject { get; set; }
        
        public string Sender { get; set; }
        
        public string Link { get; set; }
        
        public JoinTayraType JoinTayraType { get; set; }

        public JoinTayraTemplateModel(string subject, string sender, string link, JoinTayraType joinTayraType)
        {
            Subject = subject;
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
            
            var result = MailerUtils.BuildTemplateForEmail( this, fileName);
            return result;

        }
    }
}