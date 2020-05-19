﻿namespace Tayra.Mailer
{
    public class EmailPraiseReceivedDTO : ITemplateEmailDTO
    {
        public string TemplateId { get => "d-8879e33f7031462e8aae35aaf043b554"; }
        public object TemplateData { get; set; }

        public EmailPraiseReceivedDTO(string praiserUsername)
        {
            TemplateData = new
            {
                Username = praiserUsername
            };
        }
    }
}