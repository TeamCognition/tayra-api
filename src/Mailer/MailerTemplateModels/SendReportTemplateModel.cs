using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tayra.Mailer.Contracts;

namespace Tayra.Mailer.MailerTemplateModels
{
    public class SendReportTemplateModel : IEmailTemplate
    {
        public string FirstName { get; set; }
        public string SegmentName { get; set; }
        public string ReportType { get; set; }

        public string ReportLink { get; set; }
        public string Title { get; set; }

        public SendReportTemplateModel(string firstName, string segmentName, string reportType,string reportLink, string title)
        {
            FirstName = firstName;
            SegmentName = segmentName;
            ReportType = reportType;
            Title = title;
            ReportLink = reportLink;
        }

        public string GetEmailTemplate()
        {
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                $@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","EmailTemplates");
            var result = MailerUtils.BuildTemplateForEmail(path, this,"SendReport.cshtml");
            return result;
        }
    }
}