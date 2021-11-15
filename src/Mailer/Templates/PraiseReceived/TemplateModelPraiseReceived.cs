using System;
using Tayra.Common;

namespace Tayra.Mailer.Templates.PraiseReceived
{
    public class TemplateModelPraiseReceived : IEmailTemplate
    {
        public string RecipientId { get; set; } // ?
        public string Subject { get; set; }
        public string ReceiverName { get; set; }
        public string SenderName { get; set; }
        public string Url { get; set; }
        public PraiseTypes PraiseType { get; set; }

        public TemplateModelPraiseReceived(string subject, string receiverName, string senderName, string url, PraiseTypes praiseType)
        {
            Subject = subject;
            ReceiverName = receiverName;
            SenderName = senderName;
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