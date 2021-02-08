using System;
using System.IO;
using System.Net.Mime;
using Tayra.Common;
using Tayra.Mailer.Contracts;
using Tayra.Mailer.Enums;

namespace Tayra.Mailer.MailerTemplateModels
{
    public class PraiseReceivedTemplateModel : ISlackMessageTemplate , IEmailTemplate
    {
        public string TemplateKey => "praise-received";
        
        public string RecipientId { get; set; }
        public string Subject { get; set; }
        public string FirstName { get; set; }
        public string SenderName { get; set; }
        public string Link { get; set; }
        public PraiseTypes PraiseType { get; set; }

        public PraiseReceivedTemplateModel(string subject, string firstName, string senderName, string link, PraiseTypes praiseType)
        {
            Subject = subject;
            FirstName = firstName;
            SenderName = senderName;
            Link = link;
            PraiseType = praiseType;
        }

        public string GetEmailTemplate()
        {
            string fileName;
            switch (PraiseType)
            {
                case PraiseTypes.Helper:
                    fileName = "PraiseReceived-Helper.cshtml";
                    break;
                case PraiseTypes.TeamPlayer:
                    fileName = "PraiseReceived-TeamPlayer.cshtml";
                    break;
                case PraiseTypes.HardWorker:
                   fileName = "PraiseReceived-HardWorker.cshtml";
                    break;
                default:
                    throw new ApplicationException("Praise received type not supported");
            }
            var result = MailerUtils.BuildTemplateForEmail(this,fileName);
            return result;
        }

        public string GetSlackTemplate()
        {
            throw new System.NotImplementedException();
        }
    }
}