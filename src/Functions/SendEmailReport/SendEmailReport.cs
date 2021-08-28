using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using Tayra.Mailer;

namespace Tayra.Functions.SendEmailReport
{
    public class SendEmailReport
    {
        private readonly IMailerService _mailerService;

        public SendEmailReport()
        {
            _mailerService = new MailerService();
        }

        [Function(nameof(SendEmailReport))]
        public void Run([TimerTrigger("*/10 * * * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(SendEmailReport));
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            _mailerService.SendEmail("armin.odobasic@tayra.io", "Weekly report", "Hi, this is your weekly report.");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
