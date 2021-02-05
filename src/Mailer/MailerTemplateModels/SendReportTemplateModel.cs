using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tayra.Mailer.Contracts;

namespace Tayra.Mailer.MailerTemplateModels
{
    public class SendReportTemplateModel : IEmailTemplate
    {
        private const string FileName = "SendReport.cshtml";
        public string FirstName { get; set; }
        
        public string SegmentName { get; set; }
        
        public string ReportType { get; set; }

        public string ReportLink { get; set; }
        
        public string Subject { get; set; }

        public SendReportTemplateModel(string firstName, string segmentName, string reportType,string reportLink, string subject)
        {
            FirstName = firstName;
            SegmentName = segmentName;
            ReportType = reportType;
            Subject = subject;
            ReportLink = reportLink;
        }

        public string GetEmailTemplate()
        {
            var result = MailerUtils.BuildTemplateForEmail( this,FileName);
            return result;
        }
    }
}