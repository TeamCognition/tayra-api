namespace Tayra.Mailer
{
    public class EmailInviteDTO : ITemplateEmailDTO
    {
        public string TemplateId { get => "d-40d288c3499b4735b56979f29222b45e"; }
        public object TemplateData { get; set; }

        public EmailInviteDTO(string host, string inviteCode)
        {
            var protocol = host.StartsWith("localhost:", System.StringComparison.Ordinal) ? "http" : "https";
            var link = host == "tajra" ? "tayra-demo.azurewebsites.net" : host; //TODO: this is temp, when tenant get identified by the real url host, this can be deleted
            TemplateData = new
            {
                InviteLink = $"{protocol}://{link}/join?invitationCode={inviteCode}&tenant={host}"
            };
        }
    }
}
