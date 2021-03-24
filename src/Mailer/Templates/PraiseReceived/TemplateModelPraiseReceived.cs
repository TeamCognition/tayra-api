using System;
using Tayra.Common;

namespace Tayra.Mailer.Templates.PraiseReceived
{
    public class TemplateModelPraiseReceived : IEmailTemplate
    {
        public string RecipientId { get; set; } // ?
        public string Subject { get; set; }
        public string ReceiverFirstName { get; set; }
        public string SenderFirstName { get; set; }
        public string Url { get; set; }
        public PraiseTypes PraiseType { get; set; }

        public TemplateModelPraiseReceived(string subject, string receiverFirstName, string senderFirstName, string url, PraiseTypes praiseType)
        {
            Subject = subject;
            ReceiverFirstName = receiverFirstName;
            SenderFirstName = senderFirstName;
            Url = url;
            PraiseType = praiseType;
        }
        
        public string EmailTemplateFileName => 
            PraiseType switch
            {
                PraiseTypes.Helper => "EmailTemplate_PraiseReceived-Helper.cshtml",
                PraiseTypes.TeamPlayer => "EmailTemplate_PraiseReceived-TeamPlayer.cshtml",
                PraiseTypes.HardWorker => "EmailTemplate_PraiseReceived-HardWorker.cshtml",
                _ => throw new ApplicationException("Praise received type not supported")
            };
    }
}