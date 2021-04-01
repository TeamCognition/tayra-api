using Tayra.Common;

namespace Tayra.Mailer.Templates.JoinTayra
{
    public class TemplateModelJoinTayra : IEmailTemplate
    {
        public string Subject { get; set; }
        
        public string SenderName { get; set; }
        
        public string Url { get; set; }
        
        public ProfileRoles ProfileRole { get; set; }
        
        public TemplateModelJoinTayra(string subject, string senderName, string host, string inviteCode, ProfileRoles profileRole)
        {
            Subject = subject;
            SenderName = senderName;
            ProfileRole = profileRole;
            
            var protocol = host.StartsWith("localhost:", System.StringComparison.Ordinal) || host.EndsWith("local") ? "http" : "https";
            Url = $"{protocol}://{host}/join?invitationCode={inviteCode}";
        }
        
        public string EmailTemplateFileName => 
            ProfileRole == ProfileRoles.Admin
                ? "EmailTemplate_JoinTayra-Admin.cshtml"
                : "EmailTemplate_JoinTayra.cshtml";
    }
}