namespace Tayra.Mailer.Templates.NewReport
{
    public class TemplateModelNewReport : IEmailTemplate
    {
        public string EmailTemplateFileName => "NewReport/EmailTemplate.cshtml";
        public string FirstName { get; set; }
        
        public string SegmentName { get; set; }
        
        public string ReportType { get; set; }

        public string ReportLink { get; set; }
        
        public string Subject { get; set; }

        public TemplateModelNewReport(string firstName, string segmentName, string reportType, string reportLink, string subject)
        {
            FirstName = firstName;
            SegmentName = segmentName;
            ReportType = reportType;
            Subject = subject;
            ReportLink = reportLink;
        }
    }
}