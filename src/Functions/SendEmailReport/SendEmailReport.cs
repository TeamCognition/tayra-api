using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Functions.SendEmailReport
{
    public class SendEmailReport
    {
        private readonly CatalogDbContext _catalogDb;
        private readonly IMailerService _mailerService;
        private readonly string _emailSubject;

        public SendEmailReport(CatalogDbContext catalogDb)
        {
            _mailerService = new MailerService();
            _catalogDb = catalogDb;
            _emailSubject = "Your weekly Tayra report";
        }

        [Function(nameof(SendEmailReport))]
        public async Task RunAsync([TimerTrigger("*/5 * * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(SendEmailReport));
            logger.LogInformation($"C# Timer trigger function {nameof(SendEmailReport)} executed at: {DateTime.UtcNow} UTC.");

            List<Tenant> tenants = await GetAllTenants();

            foreach (var tenant in tenants)
            {
                await GenerateAndSendTenantEmails(tenant);
            }
        }

        #region Private methods

        private async Task<bool> GenerateAndSendTenantEmails(Tenant tenant)
        {
            bool isSuccessful = true;

            using (var organizationDb = new OrganizationDbContext(tenant, null))
            {
                var segments = await GetSegments(organizationDb);

                var adminAndManagerProfiles = await GetProfiles(organizationDb);
                var adminAndManagerIdentities = await GetIdentityEmails(adminAndManagerProfiles);

                foreach (var profile in adminAndManagerProfiles)
                {
                    var identityEmail = GetIdentityForProfile(adminAndManagerIdentities, profile);

                    bool isProfileSendingSuccessful = GenerateAndSendProfileEmails(segments, profile, identityEmail);

                    if (!isProfileSendingSuccessful)
                    {
                        isSuccessful = isProfileSendingSuccessful;
                    }
                }
            }

            return isSuccessful;
        }

        private bool GenerateAndSendProfileEmails(ICollection<Segment> segments, Profile profile, IdentityEmail identityEmail)
        {
            bool isSuccessful = true;

            var assignedSegments = segments;

            if (profile.Role == ProfileRoles.Manager)
            {
                assignedSegments = GetAssignedSegments(segments, profile);
            }

            foreach (var segment in assignedSegments)
            {
                bool isSegmentSendingSuccessful = GenerateAndSendEmail(identityEmail, profile, segment);

                if (!isSegmentSendingSuccessful)
                {
                    isSuccessful = isSegmentSendingSuccessful;
                }
            }

            return isSuccessful;
        }

        private bool GenerateAndSendEmail(IdentityEmail identityEmail, Profile profile, Segment segment)
        {
            var emailMessage = GenerateEmailMessage(profile, segment);

            var response = _mailerService.SendEmail(identityEmail.Email, _emailSubject, emailMessage);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private async Task<List<Tenant>> GetAllTenants()
        {
            return await _catalogDb.TenantInfo.ToListAsync();
        }

        private async Task<List<Segment>> GetSegments(OrganizationDbContext organizationDb)
        {
            return await organizationDb.Segments.AsNoTracking()
                                                .ToListAsync();
        }

        private List<Segment> GetAssignedSegments(ICollection<Segment> segments, Profile profile)
        {
            return segments.Where(x => profile.Assignments.Select(y => y.SegmentId)
                                                          .Contains(x.Id))
                           .ToList();
        }

        private async Task<List<IdentityEmail>> GetIdentityEmails(ICollection<Profile> adminAndManagerProfiles)
        {
            return await _catalogDb.IdentityEmails.AsNoTracking()
                                                  .Where(x => adminAndManagerProfiles.Select(y => y.IdentityId)
                                                                                     .Contains(x.IdentityId))
                                                  .ToListAsync();
        }

        private async Task<List<Profile>> GetProfiles(OrganizationDbContext organizationDb)
        {
            return await organizationDb.Profiles.Include(x => x.Assignments)
                                                .AsNoTracking()
                                                .Where(x => x.Role != ProfileRoles.Member)
                                                .ToListAsync();
        }

        private IdentityEmail GetIdentityForProfile(ICollection<IdentityEmail> identityEmails, Profile profile)
        {
            return identityEmails.FirstOrDefault(x => x.IdentityId == profile.IdentityId);
        }

        private string GenerateEmailMessage(Profile profile, Segment segment)
        {
            string message = $"Hello {profile?.FullName}, this is your weekly report for segment {segment?.Name}";

            return message;
        }

        #endregion
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
