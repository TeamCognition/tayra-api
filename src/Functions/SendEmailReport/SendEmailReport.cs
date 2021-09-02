using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SyncFunctions;
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
        public async Task RunAsync([TimerTrigger("*/10 * * * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(SendEmailReport));
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            List<Tenant> tenants = await GetAllTenants();

            foreach (var tenant in tenants)
            {
                await GenerateAndSendTenantEmails(tenant);
            }
        }

        #region Private methods

        private async Task<List<Tenant>> GetAllTenants()
        {
            return await _catalogDb.TenantInfo.ToListAsync();
        }

        private async Task<bool> GenerateAndSendTenantEmails(Tenant tenant)
        {
            using (var organizationDb = new OrganizationDbContext(tenant, null))
            {
                var managerAssignements = await GetAllManagerAssignements(organizationDb);

                var managerIdentities = await GetAllManagerIdentities(managerAssignements);

                foreach (var segmentAssignement in managerAssignements)
                {
                    var managerIdentity = GetManagerIdentity(managerIdentities, segmentAssignement);
                    GenerateAndSendManagerEmail(managerIdentity, segmentAssignement);
                }
            }

            return true;
        }

        private bool GenerateAndSendManagerEmail(IdentityEmail managerIdentity, ProfileAssignment segmentAssignement)
        {
            var emailMessage = GenerateEmailMessage(segmentAssignement);

            var response = _mailerService.SendEmail(managerIdentity.Email, _emailSubject, emailMessage);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private IdentityEmail GetManagerIdentity(ICollection<IdentityEmail> managerIdentities, ProfileAssignment segmentAssignement)
        {
            return managerIdentities.FirstOrDefault(x => x.IdentityId == segmentAssignement.Profile.IdentityId);
        }

        private async Task<List<IdentityEmail>> GetAllManagerIdentities(ICollection<ProfileAssignment> managerAssignements)
        {
            var managerIdentities = await _catalogDb.IdentityEmails.Where(x => managerAssignements.Select(y => y.Profile.IdentityId)
                                                                                                  .Contains(x.IdentityId))
                                                                   .ToListAsync();

            return managerIdentities;
        }

        private async Task<List<ProfileAssignment>> GetAllManagerAssignements(OrganizationDbContext organizationDb)
        {
            var profileAssignements = await organizationDb.ProfileAssignments.Include(x => x.Segment)
                                                                             .Include(x => x.Profile)
                                                                             .Where(x => x.Profile.Role == ProfileRoles.Manager)
                                                                             .ToListAsync();

            return profileAssignements;
        }

        private string GenerateEmailMessage(ProfileAssignment profileAssignment)
        {
            string message = $"Hello {profileAssignment.Profile?.FullName}, this is your weekly report for segment {profileAssignment.Segment?.Name}";

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
