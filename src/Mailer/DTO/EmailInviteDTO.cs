namespace Tayra.Mailer
{
    public class EmailInviteDTO : ITemplateEmailDTO
    {
        public string TemplateId { get => "d-40d288c3499b4735b56979f29222b45e"; }
        public object TemplateData { get; set; }

        public EmailInviteDTO(string host, string inviteCode)
        {
            TemplateData = new
            {
                InviteLink = $"https://{host}/join?invitationCode={inviteCode}"
            };
        }
    }
}
